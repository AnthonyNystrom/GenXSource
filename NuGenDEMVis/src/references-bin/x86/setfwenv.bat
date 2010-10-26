@echo off
REM ==========================================================================
REM This script is normally invoked from FWTools\setfw.bat

REM The FWTOOLS_OVERRIDE environment variable allows you to set a the 
REM install directory in your environment, instead of having to edit 
REM setfw.bat.  This is especially useful if you frequently upgrade to new 
REM versions, and don't want to have to edit the file every time. 

IF exist "%FWTOOLS_OVERRIDE%\setfw.bat" SET FWTOOLS_DIR=%FWTOOLS_OVERRIDE%

IF exist "%FWTOOLS_DIR%\setfw.bat" goto skip_err

echo FWTOOLS_DIR not set properly in setfw.bat, please fix and rerun.
goto ALL_DONE

:SKIP_ERR

PATH=%FWTOOLS_DIR%\bin;%FWTOOLS_DIR%\python;%PATH%
set PYTHONPATH=%FWTOOLS_DIR%\pymod
set PROJ_LIB=%FWTOOLS_DIR%\proj_lib
set GEOTIFF_CSV=%FWTOOLS_DIR%\data
set GDAL_DATA=%FWTOOLS_DIR%\data
set GDAL_DRIVER_PATH=%FWTOOLS_DIR%\gdal_plugins
REM set CPL_DEBUG=ON

:ALL_DONE
