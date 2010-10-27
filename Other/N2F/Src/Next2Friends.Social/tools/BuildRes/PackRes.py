# -*- coding: UTF-8 -*-

import os
import sys
import string
from array import array
import os.path
import shutil
import filecmp
import msvcrt

global IdCounter
global toolDir
global brxFile
global brhList
global oldBrhList

def GetNum(str):
	if str.find("//") != -1:
		str = str[:str.find("//")]
	str = str.expandtabs( 1 )
	str = str.strip()
	pt = str.rfind(" ")
	str = str[pt + 1:]
	return string.atoi(str)


def PackFile( fileName, packDirectory, counter ):

	pointIndex = fileName.rfind( "." )
	type = fileName[pointIndex + 1:]
	name = fileName[: pointIndex]

	if type == "pck" or type == "crc":
		return counter

	numStr = "%i" % counter
	idStr = "IDB"
	for ch in name:
		if ch in 'ABCDEFGHIJKLMNOPQRSTUVWXYZ':
			idStr = idStr + "_" + ch
		else:
			idStr = idStr + ch.upper()

	findID = -1
	for i in range(len(oldBrhList)):
		if idStr in oldBrhList[i]:
			findID = i
			break

	if findID < 0:
		counter += 1
	else:
		numStr = "%i" % GetNum(oldBrhList[findID])
		

	packLine = "<Binary Id=\""
	packLine += numStr
	packLine += "\" Name=\""
	packLine += idStr
	packLine += "\" Packed=\"1\"> <File>"
	packLine += os.path.join( packDirectory, fileName )
	packLine += "</File><Comment></Comment></Binary>"
	
	brxFile.write(packLine)

	ln = "#define	" + idStr + "    " + numStr

	brhList.append(	ln )

	
	return counter



def AddToPack(packDir, counter):
	lst = os.listdir( packDir )

#	print packDir

#check files	
	i = 0
	while i < len( lst ):
		if lst[i][0] == '.':
			i+=1
			continue
		fl = lst[i]
		if fl.find( "." ) != -1:
			counter = PackFile( fl, packDir, counter )
		i+=1
#check directories
	i = 0
	while i < len( lst ):
		if lst[i][0] == '.':
			i+=1
			continue
		dr = lst[i]
		if dr.find( "." ) == -1:
			counter = AddToPack( os.path.join( packDir, dr ), counter )
		i+=1

	return counter











if(len(sys.argv) < 4):
	print """
Usage:
PackRes.py rootDir toolsDir brxName
"""
	sys.exit()


root = sys.argv.pop(1)

toolDir = sys.argv.pop(1)

brxName = sys.argv.pop(1)

if os.path.isfile(brxName):
	os.remove(brxName)


pointIndex = brxName.rfind( "." )
name = brxName[: pointIndex]

brhName = name + ".h"
brhTemp1Name = name + ".tmp1"
brhTemp2Name = name + ".tmp2"
barName = name + ".bar"

defName = name.upper()

oldBrhList = []

lastIndex = 2000

if os.path.isfile(brhName):
	#reading old brh file if exists
	obFIle = open( brhName, "r" )
	strline = obFIle.readline()
	while strline:
		if "IDB_" in strline:
			oldBrhList.append(strline)
			nm = GetNum(strline)
			if nm > lastIndex:
				lastIndex = nm
				
	
		strline = obFIle.readline()
		
	obFIle.close()

	
	shutil.copy2(brhName, brhTemp1Name)
	os.remove(brhName)

lastIndex += 1


brxFile = open( brxName, "w" )
brhFile = open( brhTemp2Name, "w" )

startLine = "<?xml version=\"1.0\" encoding=\"UTF-8\"?> <BREWRes AdvancedResVersion=\"1.0\"> <Binaries>"

brxFile.write(startLine)

brhList = ["#ifndef __%s_BRH__" % defName, "#define __%s_BRH__" % defName, " ", "//DO NOT MODIFY THIS FILE!", " "]
brhList.append("#define %s_RES_FILE \"" % defName + barName + "\"")
brhList.append(" ")
brhList.append("#define %s_BINARY_FIRST 2001" % defName)
brhList.append("#define %s_BINARY_LAST " % defName)
brhList.append(" ")
brhList.append(" ")
brhList.append(" ")


root = os.path.abspath(root)
toolDir = os.path.abspath(toolDir)


lastItem = AddToPack(root, lastIndex)
lastItem -= 1


endLine = "</Binaries><XMLArrays></XMLArrays><Strings></Strings></BREWRes>"
brxFile.write(endLine)
brxFile.close()

brhList[8] += "%i" % lastItem

li = len( brhList ) - 1
while True:
	changes = False
	for i in range( 12, li ):
		if GetNum(brhList[i]) > GetNum(brhList[i + 1]):
			ts = brhList[i]
			brhList.remove(brhList[i])
			brhList.append(ts)
			changes = True
	if not changes:
		break
			
		

brhList.append(" ")
brhList.append(" ")
brhList.append(" ")
brhList.append("#endif // __%s_BRH__" % defName)

i = 0
while i < len(brhList):
	brhList[i] += "\n"
	brhFile.write(brhList[i])
	i += 1

brhFile.close()


params = os.path.join( toolDir, "respackc.exe" ) + " " + brxName
pl = params.split()
print pl

os.spawnv(os.P_WAIT, os.path.join( toolDir, "respackc.exe" ), pl )


#--------- waiting cycle
b = True
while b:
	if os.path.isfile(brhName):
		try:
			os.remove(brhName)
			b = False
		except:
			b = True
	else:
		b = False

#--------- waiting cycle



if os.path.isfile(brhTemp1Name):
	if filecmp.cmp(brhTemp1Name, brhTemp2Name) == True:
		shutil.copy2(brhTemp1Name, brhName)
	else:
		shutil.copy2(brhTemp2Name, brhName)
	os.remove(brhTemp1Name)
	os.remove(brhTemp2Name)
else:
	shutil.copy2(brhTemp2Name, brhName)
	os.remove(brhTemp2Name)



print "------------------------------------packing end"
