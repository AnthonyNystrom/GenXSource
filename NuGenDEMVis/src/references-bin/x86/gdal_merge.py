#!/usr/bin/env python
###############################################################################
# $Id: gdal_merge.py 10048 2006-09-28 03:41:30Z fwarmerdam $
#
# Project:  InSAR Peppers
# Purpose:  Module to extract data from many rasters into one output.
# Author:   Frank Warmerdam, warmerdam@pobox.com
#
###############################################################################
# Copyright (c) 2000, Atlantis Scientific Inc. (www.atlsci.com)
# 
# This library is free software; you can redistribute it and/or
# modify it under the terms of the GNU Library General Public
# License as published by the Free Software Foundation; either
# version 2 of the License, or (at your option) any later version.
# 
# This library is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
# Library General Public License for more details.
# 
# You should have received a copy of the GNU Library General Public
# License along with this library; if not, write to the
# Free Software Foundation, Inc., 59 Temple Place - Suite 330,
# Boston, MA 02111-1307, USA.
###############################################################################
# 
#  $Log$
#  Revision 1.26  2006/09/28 03:41:30  fwarmerdam
#  existing output file needs to be opened in update mode!
#
#  Revision 1.25  2006/04/20 13:27:57  fwarmerdam
#  Added error checks on Driver's Create support and success of Create.
#
#  Revision 1.24  2006/01/26 23:40:39  fwarmerdam
#  treat nodata as a float.
#
#  Revision 1.23  2005/11/25 02:13:36  fwarmerdam
#  Added --help-general.
#
#  Revision 1.22  2005/11/16 18:30:49  fwarmerdam
#  Fixed round off issue with output file size.
#
#  Revision 1.21  2005/08/18 15:45:15  fwarmerdam
#  Added the -createonly switch.
#
#  Revision 1.20  2005/07/19 03:33:39  fwarmerdam
#  removed left over global_list
#
#  Revision 1.19  2005/06/23 19:51:51  fwarmerdam
#  Fixed support for non-square pixels c/o Matt Giger
#  http://bugzilla.remotesensing.org/show_bug.cgi?id=874
#
#  Revision 1.18  2005/03/29 22:40:00  fwarmerdam
#  Added -ot option.
#
#  Revision 1.17  2005/02/23 18:29:07  fwarmerdam
#  Accept "either spelling" of separate.
#
#  Revision 1.16  2005/02/23 18:23:00  fwarmerdam
#  Added -seperate to the usage message.
#
#  Revision 1.15  2004/09/02 22:06:24  warmerda
#  Added a bit of commandline error reporting.
#
#  Revision 1.14  2004/08/23 15:05:27  warmerda
#  Added projection setting for new files.
#
#  Revision 1.13  2004/04/02 22:31:26  warmerda
#  Use -of for format.
#
#  Revision 1.12  2004/04/02 17:40:44  warmerda
#  added GDALGeneralCmdLineProcessor() support
#
#  Revision 1.11  2004/03/26 17:11:42  warmerda
#  added -init
#
#  Revision 1.10  2003/04/22 14:42:45  warmerda
#  Don't import Numeric unless we need it.
#
#  Revision 1.9  2003/04/22 13:30:05  warmerda
#  Added -co flag.
#
#  Revision 1.8  2003/03/07 16:26:39  warmerda
#  fixed up for ungeoreferenced files, supress extra error
#
#  Revision 1.7  2003/01/28 15:00:13  warmerda
#  applied patch for multi-band support from Ken Boss
#
#  Revision 1.6  2003/01/20 22:19:08  warmerda
#  added nodata support
#
#  Revision 1.5  2002/12/12 14:54:42  warmerda
#  added the -pct flag to copy over a pct
#
#  Revision 1.4  2002/12/12 14:48:12  warmerda
#  removed broken options arg to gdal.Create()
#
#  Revision 1.3  2002/04/03 21:12:05  warmerda
#  added -separate flag for Gerald Buckmaster
#
#  Revision 1.2  2000/11/29 20:36:18  warmerda
#  allow output file to be preexisting
#
#  Revision 1.1  2000/11/29 20:23:13  warmerda
#  New
#
#

import gdal
import sys

verbose = 0


# =============================================================================
def raster_copy( s_fh, s_xoff, s_yoff, s_xsize, s_ysize, s_band_n,
                 t_fh, t_xoff, t_yoff, t_xsize, t_ysize, t_band_n,
                 nodata=None ):

    if nodata is not None:
        return raster_copy_with_nodata(
            s_fh, s_xoff, s_yoff, s_xsize, s_ysize, s_band_n,
            t_fh, t_xoff, t_yoff, t_xsize, t_ysize, t_band_n,
            nodata )

    if verbose != 0:
        print 'Copy %d,%d,%d,%d to %d,%d,%d,%d.' \
              % (s_xoff, s_yoff, s_xsize, s_ysize,
             t_xoff, t_yoff, t_xsize, t_ysize )

    s_band = s_fh.GetRasterBand( s_band_n )
    t_band = t_fh.GetRasterBand( t_band_n )

    data = s_band.ReadRaster( s_xoff, s_yoff, s_xsize, s_ysize,
                              t_xsize, t_ysize, t_band.DataType )
    t_band.WriteRaster( t_xoff, t_yoff, t_xsize, t_ysize,
                        data, t_xsize, t_ysize, t_band.DataType )
        

    return 0
    
# =============================================================================
def raster_copy_with_nodata( s_fh, s_xoff, s_yoff, s_xsize, s_ysize, s_band_n,
                             t_fh, t_xoff, t_yoff, t_xsize, t_ysize, t_band_n,
                             nodata ):

    import Numeric
    
    if verbose != 0:
        print 'Copy %d,%d,%d,%d to %d,%d,%d,%d.' \
              % (s_xoff, s_yoff, s_xsize, s_ysize,
             t_xoff, t_yoff, t_xsize, t_ysize )

    s_band = s_fh.GetRasterBand( s_band_n )
    t_band = t_fh.GetRasterBand( t_band_n )

    data_src = s_band.ReadAsArray( s_xoff, s_yoff, s_xsize, s_ysize,
                                   t_xsize, t_ysize )
    data_dst = t_band.ReadAsArray( t_xoff, t_yoff, t_xsize, t_ysize )

    nodata_test = Numeric.equal(data_src,nodata)
    to_write = Numeric.choose( nodata_test, (data_src, data_dst) )
                               
    t_band.WriteArray( to_write, t_xoff, t_yoff )

    return 0
    
# =============================================================================
def names_to_fileinfos( names ):
    """
    Translate a list of GDAL filenames, into file_info objects.

    names -- list of valid GDAL dataset names.

    Returns a list of file_info objects.  There may be less file_info objects
    than names if some of the names could not be opened as GDAL files.
    """
    
    file_infos = []
    for name in names:
        fi = file_info()
        if fi.init_from_name( name ) == 1:
            file_infos.append( fi )

    return file_infos

# *****************************************************************************
class file_info:
    """A class holding information about a GDAL file."""

    def init_from_name(self, filename):
        """
        Initialize file_info from filename

        filename -- Name of file to read.

        Returns 1 on success or 0 if the file can't be opened.
        """
        fh = gdal.Open( filename )
        if fh is None:
            return 0

        self.filename = filename
        self.bands = fh.RasterCount
        self.xsize = fh.RasterXSize
        self.ysize = fh.RasterYSize
        self.band_type = fh.GetRasterBand(1).DataType
        self.projection = fh.GetProjection()
        self.geotransform = fh.GetGeoTransform()
        self.ulx = self.geotransform[0]
        self.uly = self.geotransform[3]
        self.lrx = self.ulx + self.geotransform[1] * self.xsize
        self.lry = self.uly + self.geotransform[5] * self.ysize

        ct = fh.GetRasterBand(1).GetRasterColorTable()
        if ct is not None:
            self.ct = ct.Clone()
        else:
            self.ct = None

        return 1

    def report( self ):
        print 'Filename: '+ self.filename
        print 'File Size: %dx%dx%d' \
              % (self.xsize, self.ysize, self.bands)
        print 'Pixel Size: %f x %f' \
              % (self.geotransform[1],self.geotransform[5])
        print 'UL:(%f,%f)   LR:(%f,%f)' \
              % (self.ulx,self.uly,self.lrx,self.lry)

    def copy_into( self, t_fh, s_band = 1, t_band = 1, nodata_arg=None ):
        """
        Copy this files image into target file.

        This method will compute the overlap area of the file_info objects
        file, and the target gdal.Dataset object, and copy the image data
        for the common window area.  It is assumed that the files are in
        a compatible projection ... no checking or warping is done.  However,
        if the destination file is a different resolution, or different
        image pixel type, the appropriate resampling and conversions will
        be done (using normal GDAL promotion/demotion rules).

        t_fh -- gdal.Dataset object for the file into which some or all
        of this file may be copied.

        Returns 1 on success (or if nothing needs to be copied), and zero one
        failure.
        """
        t_geotransform = t_fh.GetGeoTransform()
        t_ulx = t_geotransform[0]
        t_uly = t_geotransform[3]
        t_lrx = t_geotransform[0] + t_fh.RasterXSize * t_geotransform[1]
        t_lry = t_geotransform[3] + t_fh.RasterYSize * t_geotransform[5]

        # figure out intersection region
        tgw_ulx = max(t_ulx,self.ulx)
        tgw_lrx = min(t_lrx,self.lrx)
        if t_geotransform[5] < 0:
            tgw_uly = min(t_uly,self.uly)
            tgw_lry = max(t_lry,self.lry)
        else:
            tgw_uly = max(t_uly,self.uly)
            tgw_lry = min(t_lry,self.lry)
        
        # do they even intersect?
        if tgw_ulx >= tgw_lrx:
            return 1
        if t_geotransform[5] < 0 and tgw_uly <= tgw_lry:
            return 1
        if t_geotransform[5] > 0 and tgw_uly >= tgw_lry:
            return 1
            
        # compute target window in pixel coordinates.
        tw_xoff = int((tgw_ulx - t_geotransform[0]) / t_geotransform[1] + 0.1)
        tw_yoff = int((tgw_uly - t_geotransform[3]) / t_geotransform[5] + 0.1)
        tw_xsize = int((tgw_lrx - t_geotransform[0])/t_geotransform[1] + 0.5) \
                   - tw_xoff
        tw_ysize = int((tgw_lry - t_geotransform[3])/t_geotransform[5] + 0.5) \
                   - tw_yoff

        if tw_xsize < 1 or tw_ysize < 1:
            return 1

        # Compute source window in pixel coordinates.
        sw_xoff = int((tgw_ulx - self.geotransform[0]) / self.geotransform[1])
        sw_yoff = int((tgw_uly - self.geotransform[3]) / self.geotransform[5])
        sw_xsize = int((tgw_lrx - self.geotransform[0]) \
                       / self.geotransform[1] + 0.5) - sw_xoff
        sw_ysize = int((tgw_lry - self.geotransform[3]) \
                       / self.geotransform[5] + 0.5) - sw_yoff

        if sw_xsize < 1 or sw_ysize < 1:
            return 1

        # Open the source file, and copy the selected region.
        s_fh = gdal.Open( self.filename )

        return \
            raster_copy( s_fh, sw_xoff, sw_yoff, sw_xsize, sw_ysize, s_band,
                         t_fh, tw_xoff, tw_yoff, tw_xsize, tw_ysize, t_band,
                         nodata_arg )


# =============================================================================
def Usage():
    print 'Usage: gdal_merge.py [-o out_filename] [-of out_format] [-co NAME=VALUE]*'
    print '                     [-ps pixelsize_x pixelsize_y] [-separate] [-v] [-pct]'
    print '                     [-ul_lr ulx uly lrx lry] [-n nodata_value] [-init value]'
    print '                     [-ot datatype] [-createonly] input_files'
    print '                     [--help-general]'
    print

# =============================================================================
#
# Program mainline.
#

if __name__ == '__main__':

    names = []
    format = 'GTiff'
    out_file = 'out.tif'

    ulx = None
    psize_x = None
    separate = 0
    copy_pct = 0
    nodata = None
    create_options = []
    pre_init = None
    band_type = None
    createonly = 0

    gdal.AllRegister()
    argv = gdal.GeneralCmdLineProcessor( sys.argv )
    if argv is None:
        sys.exit( 0 )

    # Parse command line arguments.
    i = 1
    while i < len(argv):
        arg = argv[i]

        if arg == '-o':
            i = i + 1
            out_file = argv[i]

        elif arg == '-v':
            verbose = 1

        elif arg == '-createonly':
            createonly = 1

        elif arg == '-separate':
            separate = 1

        elif arg == '-seperate':
            separate = 1

        elif arg == '-pct':
            copy_pct = 1

        elif arg == '-ot':
            i = i + 1
            band_type = gdal.GetDataTypeByName( argv[i] )
            if band_type == gdal.GDT_Unknown:
                print 'Unknown GDAL data type: ', argv[i]
                sys.exit( 1 )

        elif arg == '-init':
            i = i + 1
            pre_init = float(argv[i])

        elif arg == '-n':
            i = i + 1
            nodata = float(argv[i])

        elif arg == '-f':
            # for backward compatibility.
            i = i + 1
            format = argv[i]

        elif arg == '-of':
            i = i + 1
            format = argv[i]

        elif arg == '-co':
            i = i + 1
            create_options.append( argv[i] )

        elif arg == '-ps':
            psize_x = float(argv[i+1])
            psize_y = -1 * abs(float(argv[i+2]))
            i = i + 2

        elif arg == '-ul_lr':
            ulx = float(argv[i+1])
            uly = float(argv[i+2])
            lrx = float(argv[i+3])
            lry = float(argv[i+4])
            i = i + 4

        elif arg[:1] == '-':
            print 'Unrecognised command option: ', arg
            Usage()
            sys.exit( 1 )

        else:
            names.append( arg )
            
        i = i + 1

    if len(names) == 0:
        print 'No input files selected.'
        Usage()
        sys.exit( 1 )

    Driver = gdal.GetDriverByName(format)
    if Driver is None:
        print 'Format driver %s not found, pick a supported driver.' % format
        sys.exit( 1 )

    DriverMD = Driver.GetMetadata()
    if not DriverMD.has_key('DCAP_CREATE'):
        print 'Format driver %s does not support creation and piecewise writing.\nPlease select a format that does, such as GTiff (the default) or HFA (Erdas Imagine).' % format
        sys.exit( 1 )

    # Collect information on all the source files.
    file_infos = names_to_fileinfos( names )

    if ulx is None:
        ulx = file_infos[0].ulx
        uly = file_infos[0].uly
        lrx = file_infos[0].lrx
        lry = file_infos[0].lry
        
        for fi in file_infos:
            ulx = min(ulx, fi.ulx)
            uly = max(uly, fi.uly)
            lrx = max(lrx, fi.lrx)
            lry = min(lry, fi.lry)

    if psize_x is None:
        psize_x = file_infos[0].geotransform[1]
        psize_y = file_infos[0].geotransform[5]

    if band_type is None:
        band_type = file_infos[0].band_type

    # Try opening as an existing file.
    gdal.PushErrorHandler( 'CPLQuietErrorHandler' )
    t_fh = gdal.Open( out_file, gdal.GA_Update )
    gdal.PopErrorHandler()
    
    # Create output file if it does not already exist.
    if t_fh is None:
        geotransform = [ulx, psize_x, 0, uly, 0, psize_y]

        xsize = int((lrx - ulx) / geotransform[1] + 0.5)
        ysize = int((lry - uly) / geotransform[5] + 0.5)

        if separate != 0:
            bands = len(file_infos)
        else:
            bands = file_infos[0].bands

        t_fh = Driver.Create( out_file, xsize, ysize, bands,
                              band_type, create_options )
        if t_fh is None:
            print 'Creation failed, terminating gdal_merge.'
            sys.exit( 1 )
            
        t_fh.SetGeoTransform( geotransform )
        t_fh.SetProjection( file_infos[0].projection )

        if copy_pct:
            t_fh.GetRasterBand(1).SetRasterColorTable(file_infos[0].ct)
    else:
        if separate != 0:
            bands = len(file_infos)
        else:
            bands = min(file_infos[0].bands,t_fh.RasterCount)

    # Do we need to pre-initialize the whole mosaic file to some value?
    if pre_init is not None:
        for i in range(t_fh.RasterCount):
            t_fh.GetRasterBand(i+1).Fill( pre_init )

    # Copy data from source files into output file.
    t_band = 1
    for fi in file_infos:
        if createonly != 0:
            continue
        
        if verbose != 0:
            print
            fi.report()

        if separate == 0 :
            for band in range(1, bands+1):
                fi.copy_into( t_fh, band, band, nodata )
        else:
            fi.copy_into( t_fh, 1, t_band, nodata )
            t_band = t_band+1
            
    # Force file to be closed.
    t_fh = None
