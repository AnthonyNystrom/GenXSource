# -*- coding: UTF-8 -*-

import os
import sys
import string
from array import array
import os.path
import shutil
import filecmp
import msvcrt



def CheckFile( fileName, filePath ):
	pointIndex = fileName.find( "." )
	type = fileName[pointIndex + 1:]
	if type != "conv":
		return

	convFile = open( os.path.join( filePath, fileName ), "r" )

	filestrip = ["Bmp2Spa.exe\n"]
	ln = convFile.readline()
	while ln:
		filestrip.append(ln)
		ln = convFile.readline()
	convFile.close()

	convFile = open( os.path.join( filePath, fileName ), "w" )
	i = 0
	while i < len( filestrip ):
		convFile.write(filestrip[i])
		i+=1
	convFile.close()

	





def CheckDir( pathForCheck ):
	
	lst = os.listdir( pathForCheck )

	print pathForCheck

	print lst

#check files	
	i = 0
	while i < len( lst ):
		if lst[i][0] == '.':
			i+=1
			continue
		fl = lst[i]
		if fl.find( "." ) != -1:
			CheckFile( fl, pathForCheck )
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


			CheckDir( newPath )
		i+=1








if(len(sys.argv) < 2):
	print """
Usage:
addconv.py rootDir
"""
	sys.exit()

root = sys.argv.pop(1)


root = os.path.abspath(root)


CheckDir(root)

print "-------------------- convertation end"
