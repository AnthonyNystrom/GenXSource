# -*- coding: UTF-8 -*-

import os
import sys
import string
from array import array
import os.path
import shutil
import filecmp
import msvcrt

global convDir
global toolDir
global outDepends
global depDir




def CheckFile( fileName, filePath, tempPath, packPath ):

	global outDepends

	pointIndex = fileName.find( "." )
	type = fileName[pointIndex + 1:]
	if type == "conv":
		return
	convFileName = fileName[:pointIndex] + ".conv"
	if not os.path.isfile( os.path.join( filePath, convFileName ) ):
		return

	depends = [fileName, convFileName]
	outD = []

	convFile = open( os.path.join( filePath, convFileName ), "r" )

	tool = convFile.readline()
	tool = tool.strip()
	params = ""
	if tool:
		params = convFile.readline()
		params = params.strip()
		params = " " + params
		params = fileName + params
		params = " " + params
		params = os.path.join( toolDir, tool ) + params
		
		addDepends = convFile.readline()
		addDepends = addDepends.expandtabs(1)
		addDepends = addDepends.strip()
		depends += addDepends.split()
		

#		while addDepends:
#			spIndex = addDepends.find( " " )
#			if spIndex == -1:
#				newDep = addDepends
#				addDepends = ""
#			else:
#				newDep = addDepends[:spIndex]
#				addDepends = addDepends[spIndex:]
#				addDepends = addDepends.strip()
#			depends.append(newDep)

		outDN = convFile.readline()
		outDN = outDN.expandtabs(1)
		outDN = outDN.strip()
		outD = outDN.split()

		for i in range(len(outD)):
			outD[i] = os.path.abspath(outD[i])

	convFile.close()

	#check for changes


######## compare files
	isIdent	= True
	for i in range(len(depends)):
		if not os.path.isfile( os.path.join( tempPath, depends[i] ) ):
			isIdent = False
			break
		if not filecmp.cmp(os.path.join( filePath, depends[i] ), os.path.join( tempPath, depends[i] )):
			isIdent = False
			break

		
	for i in range(len(outD)):
		if not outD[i] in outDepends:
			outDepends.append(outD[i])
		cdir = os.path.basename(outD[i])
		if not os.path.isfile( os.path.join( depDir, cdir ) ):
			isIdent = False
			continue
		if not filecmp.cmp( outD[i], os.path.join( depDir, cdir )):
			isIdent = False
	
	if isIdent:
		return


######## starting convertation

	i = 0
	while i < len( depends ):
		shutil.copy( os.path.join( filePath, depends[i] ), os.path.join( convDir, depends[i] ) )
		i += 1


	if not tool:
		print "Copying resource %s" % fileName
		shutil.copy( os.path.join( convDir, fileName ), os.path.join( packPath, fileName ) )
	
		i = 0
		while i < len( depends ):
			shutil.move( os.path.join( convDir, depends[i] ), os.path.join( tempPath, depends[i] ) )
			i += 1
		return

	pl = params.split()
	print pl

#	os.spawnl(os.P_WAIT, pl[0] )

	try:
		os.spawnv(os.P_WAIT, os.path.join( toolDir, tool ), pl )
	except:
		print "-------------------------------------"
		print "Convertation FAILED!!!"
		print "! utilite returns an error !"
		print "-------------------------------------"
		msvcrt.getch()
		sys.exit()


	lst = os.listdir( convDir )
	if len( lst ) <= len(depends) :
		print "-------------------------------------"
		print "Convertation FAILED!!!"
		print "! Can't find any files after convertation !"
		print "-------------------------------------"
		msvcrt.getch()
		sys.exit()


	i = 0
	while i < len( depends ):
		shutil.move( os.path.join( convDir, depends[i] ), os.path.join( tempPath, depends[i] ) )
		i += 1
	
	lst = os.listdir( convDir )
	if not lst:
		print "-------------------------------------"
		print "Convertation FAILED!!!"
		print "-------------------------------------"
		msvcrt.getch()
		sys.exit()

	i = 0
	while i < len( lst ):
		shutil.move( os.path.join( convDir, lst[i] ), os.path.join( packPath, lst[i] ) )
		i += 1
	





def CheckDir( pathForCheck, pathForTemp, pathForPack ):
	
	if not os.path.isdir( pathForTemp ):
		os.mkdir( pathForTemp )
	if not os.path.isdir( pathForPack ):
		os.mkdir( pathForPack )
	if not os.path.isdir(depDir):
		os.mkdir(depDir)

	lst = os.listdir( pathForCheck )

#	print pathForCheck

#	print lst

#check files	
	i = 0
	while i < len( lst ):
		if lst[i][0] == '.':
			i+=1
			continue
		fl = lst[i]
		if fl.find( "." ) != -1:
			CheckFile( fl, pathForCheck, pathForTemp, pathForPack )
		i+=1
#check directories
	i = 0
	while i < len( lst ):
		if lst[i][0] == '.':
			i+=1
			continue
		dr = lst[i]
		if dr.find( "." ) == -1:
			newPath = os.path.join( pathForCheck, dr )
			newPath = os.path.normpath( newPath)

			newTemp = os.path.join( pathForTemp, dr )
			newTemp = os.path.normpath( newTemp )

			newPack = os.path.join( pathForPack, dr )
			newPack = os.path.normpath( newPack )


			CheckDir( newPath, newTemp, newPack )
		i+=1








if(len(sys.argv) < 4):
	print """
Usage:
ConvertRes.py rootDir toolsDir packDir
"""
	sys.exit()

root = sys.argv.pop(1)

toolDir = sys.argv.pop(1)

packRoot = sys.argv.pop(1)



curPath = os.getcwd()

root = os.path.abspath(root)
toolDir = os.path.abspath(toolDir)
packRoot = os.path.abspath(packRoot)



tempRoot = os.path.abspath("./$PreConv$")

convDir = os.path.abspath("./$convTemp$")

depDir = os.path.join(tempRoot, ".OutDepends")


if os.path.isdir(convDir):
	shutil.rmtree(convDir)

os.mkdir(convDir)

os.chdir(convDir)

outDepends = []


CheckDir(root, tempRoot, packRoot)

for i in range(len(outDepends)):
	cdir = os.path.join( depDir, os.path.basename( outDepends[i] ) )
	shutil.copy( outDepends[i], cdir )
	


os.chdir(curPath)

print "-------------------- convertation end"
