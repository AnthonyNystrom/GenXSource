#!/usr/bin/env python
#******************************************************************************
#  $Id: gdalimport.py 9355 2006-03-21 21:54:00Z fwarmerdam $
# 
#  Name:     gdalimport
#  Project:  GDAL Python Interface
#  Purpose:  Import a GDAL supported file to Tiled GeoTIFF, and build overviews
#  Author:   Frank Warmerdam, warmerdam@pobox.com
# 
#******************************************************************************
#  Copyright (c) 2000, Frank Warmerdam
# 
#  Permission is hereby granted, free of charge, to any person obtaining a
#  copy of this software and associated documentation files (the "Software"),
#  to deal in the Software without restriction, including without limitation
#  the rights to use, copy, modify, merge, publish, distribute, sublicense,
#  and/or sell copies of the Software, and to permit persons to whom the
#  Software is furnished to do so, subject to the following conditions:
# 
#  The above copyright notice and this permission notice shall be included
#  in all copies or substantial portions of the Software.
# 
#  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
#  OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
#  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
#  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
#  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
#  FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
#  DEALINGS IN THE SOFTWARE.
#******************************************************************************
# 
# $Log$
# Revision 1.5  2006/03/21 21:54:00  fwarmerdam
# fixup headers
#
# Revision 1.4  2005/11/25 02:15:50  fwarmerdam
# Added --help-general in usage message.
#
# Revision 1.3  2004/04/02 17:40:44  warmerda
# added GDALGeneralCmdLineProcessor() support
#
# Revision 1.2  2000/06/27 16:48:57  warmerda
# added progress func support
#
# Revision 1.1  2000/06/26 21:11:21  warmerda
# New
#

import gdal
import sys
import os.path

gdal.AllRegister()
argv = gdal.GeneralCmdLineProcessor( sys.argv )
if argv is None:
    sys.exit( 0 )

if len(argv) < 2:
    print "Usage: gdalimport.py [--help-general] source_file [newfile]"
    sys.exit(1)

def progress_cb( complete, message, cb_data ):
    print cb_data, complete
    

filename = argv[1]
dataset = gdal.Open( filename )
if dataset is None:
    print 'Unable to open ', filename
    sys.exit(1)

geotiff = gdal.GetDriverByName("GTiff")
if geotiff is None:
    print 'GeoTIFF driver not registered.'
    sys.exit(1)

if len(argv) < 3: 
    newbase, ext = os.path.splitext(os.path.basename(filename))
    newfile = newbase + ".tif"
    i = 0
    while os.path.isfile(newfile):
        i = i+1
        newfile = newbase+"_"+str(i)+".tif"
else:
    newfile = argv[2]

print 'Importing to Tiled GeoTIFF file:', newfile
new_dataset = geotiff.CreateCopy( newfile, dataset, 0,
                                  ['TILED=YES',],
                                  callback = progress_cb,
                                  callback_data = 'Translate: ' )
dataset = None

print 'Building overviews'
new_dataset.BuildOverviews( "average", callback=progress_cb,
                            callback_data = 'Overviews: ' )
new_dataset = None

print 'Done'



