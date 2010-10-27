# -*- coding: UTF-8 -*-


import os
import sys
import string
from array import array
import os.path
import shutil
import filecmp
import msvcrt


if(len(sys.argv) < 2):
	print """
Usage:
RestoreBrh.py brhName
"""
	sys.exit()


brhName = sys.argv.pop(1)

if not os.path.isfile(brhName):
	sys.exit()

pointIndex = brhName.rfind( "." )
name = brhName[: pointIndex]

brhTemp1Name = name + ".tmp1"
brhTemp2Name = name + ".tmp2"

brhFile = open( brhName, "r" )

ln = brhFile.readline()
fl = []

while ln:
	if "_FILE_SIZE" in ln or "_FILE_CRC" in ln:
		ln = brhFile.readline()
		continue
	fl.append(ln)
	ln = brhFile.readline()
brhFile.close()



brhFile = open( brhTemp2Name, "w" )

for i in range( len( fl ) ):
	brhFile.write( fl[i] )

brhFile.close()


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
