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
PrestoreBrh.py brhName
"""
	sys.exit()


brhName = sys.argv.pop(1)

if not os.path.isfile(brhName):
	sys.exit()

pointIndex = brhName.rfind( "." )
name = brhName[: pointIndex]

brhTemp1Name = name + ".tmp1"
brhTemp2Name = name + ".tmp2"

shutil.copy2(brhName, brhTemp1Name)
os.remove(brhName)


