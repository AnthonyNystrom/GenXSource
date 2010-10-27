#============================================================================
#  Name:
#    $(TARGET).MAK
#
#  Description:
#    Makefile to build the $(TARGET) downloadable module.
#
#   The following nmake targets are available in this makefile:
#
#     all           - make .elf and .mod image files (default)
#     clean         - delete object directory and image files
#     filename.o    - make object file
#
#   The above targets can be made with the following command:
#
#     nmake /f $(TARGET).mak [target]
#
#  Assumptions:
#    1. The environment variable ADSHOME is set to the root directory of the
#       arm tools.
#    2. The version of ADS is 1.2 or above.
#
#  Notes:
#    None.
#
#
#        Copyright © 2000-2001 QUALCOMM Incorporated.
#               All Rights Reserved.
#            QUALCOMM Proprietary/GTDR
#
#----------------------------------------------------------------------------
#============================================================================

include	.\brew.inc
include	.\files.inc

TARGET		= next2friends
TARGETTYPE	= elf
MODULE		= mod				# Downloadable module extension

OBJDIR		= next2friends
OUTDIR		= .\!phone\Next2Friends				# Directory for output target

APPINCL		= -I.\framework\include\GUI\framework -I.\framework\include\NetLib -I.\framework\include\EncoderLib -I.\framework\include\PhotoLib -I.\framework\include\CameraLib -I.\include -I..\independent\resources -I.\framework\include -I..\independent\source
APPLIBS		= .\framework\lib\framework\Framework312.a .\framework\lib\NetLib\NetLib.a .\framework\lib\EncoderLib\EncoderLib.a .\framework\lib\CameraLib\CameraLib.a .\framework\lib\PhotoLib\PhotoLib.a

#----------------------------------------------------------------------------
# Setup compiler options
#----------------------------------------------------------------------------

OBJ_CMD		= -o			# Command line option to specify output filename
LINK_CMD	= -o 
INC			= -I. -I$(AEEINCPATH) $(APPINCL)
BINFORMAT	= -bin
OUT_CMD		= -output

#----------------------------------------------------------------------------
# Default target
#----------------------------------------------------------------------------
all :  $(TARGET).$(MODULE)

#----------------------------------------------------------------------------
# Clean target
#----------------------------------------------------------------------------

# The object subdirectory, target image file, and target hex file are deleted.

clean :
	@echo ---------------------------------------------------------------
	@echo CLEAN
	-del /f $(OBJS)
	-del /f $(TARGET).$(TARGETTYPE)
	-del /f $(TARGET).$(MODULE)

	@echo ---------------------------------------------------------------

#============================================================================
#                           DEFAULT SUFFIX RULES
#============================================================================

SRC_CPP_FILE	= $(@F:.o=.cpp)					# Input source file specification
SRC_C_FILE		= $(@F:.o=.c)					# Input source file specification
OBJ_FILE		= $(OBJ_CMD) $(OBJDIR)\$(@F)				# Output object file specification

.SUFFIXES :
.SUFFIXES : .o .dep .c .cpp

#--------------------------------------------------------------------------
# C code inference rules
#----------------------------------------------------------------------------

.c.o:
	@echo $(@F)
	@$(CC) $(CCFLAGS) $(INC) $(OBJ_FILE) $(SRC_C_FILE)

.cpp.o:
	@echo $(@F)
	@$(CCP) $(CCPFLAGS) $(INC) $(OBJ_FILE) $(SRC_CPP_FILE)

#===============================================================================
#                           MODULE SPECIFIC RULES
#===============================================================================

#----------------------------------------------------------------------------
# Lib file targets
#----------------------------------------------------------------------------

$(TARGET).$(MODULE) : $(TARGET).$(TARGETTYPE)
	@echo ---------------------------------------------------------------
	@echo $@
#	@$(HEXTOOL)  $(OBJDIR)\$(TARGET).$(TARGETTYPE) $(BINFORMAT) $(OUTDIR)\$(TARGET).$(MODULE)
	@$(HEXTOOL)  $(OUT_CMD) $(OUTDIR)\$(TARGET).$(MODULE) $(OBJDIR)\$(TARGET).$(TARGETTYPE)

#	@del /f $(OBJDIR)\$(OBJS)
#	@del /f $$(OBJDIR)\(TARGET).$(TARGETTYPE)
	@echo ---------------------------------------------------------------

$(TARGET).$(TARGETTYPE) : $(OBJS)
	@echo TARGET $@
	@$(LD) $(LINK_CMD) $(OBJDIR)\$(TARGET).$(TARGETTYPE) $(LDFLAGS) $(OBJS) $(APPLIBS)

#----------------------------------------------------------------------------
# Applet Specific Rules
#----------------------------------------------------------------------------

RULE1 = .\source
{$(RULE1)}.cpp.o:
	@echo $(@F)
	@$(CPP) $(CCFLAGS) $(INC) $(OBJ_FILE)  $(RULE1)\$(SRC_CPP_FILE)

RULE2 = ..\independent\source
{$(RULE2)}.cpp.o:
	@echo $(@F)
	@$(CPP) $(CCFLAGS) $(INC) $(OBJ_FILE)  $(RULE2)\$(SRC_CPP_FILE)

# --------------------------------------------
# DEPENDENCY LIST, DO NOT EDIT BELOW THIS LINE
# --------------------------------------------
