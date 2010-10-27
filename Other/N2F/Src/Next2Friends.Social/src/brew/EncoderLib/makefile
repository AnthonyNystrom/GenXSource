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
#        Copyright © 2000-2003 QUALCOMM Incorporated.
#               All Rights Reserved.
#            QUALCOMM Proprietary/GTDR
#
#----------------------------------------------------------------------------
#============================================================================

include	..\brew.inc
include	.\files.inc

TARGET		= EncoderLib		# Target name
TARGETTYPE	= a					# Target image file format

OUTDIR		= ..\framework\lib\encoderlib					# Directory for output target

APPINCL		=-I..\framework\include\encoderlib -I..\framework\include\encoderlib\libjpeg -I..\framework\include\gui\framework			# Include directory


#----------------------------------------------------------------------------
# Setup compiler options
#----------------------------------------------------------------------------

OBJ_CMD		= -o				# Command line option to specify output filename
INC			= -I. -I$(AEEINCPATH) $(APPINCL)


!IFDEF DEBUG
TARGET	= $(TARGET)_d
DBGEN	= -g					# Enable debug
OBJDIR	= $(OBJDIR)Debug
!ELSE
OBJDIR	= $(OBJDIR)Release
!ENDIF

#----------------------------------------------------------------------------
# Default target
#----------------------------------------------------------------------------
all : MakeDir $(TARGET).$(TARGETTYPE)

MakeDir : 
	@if not exist $(OBJDIR) md $(OBJDIR)

#----------------------------------------------------------------------------
# Clean target
#----------------------------------------------------------------------------
clean :
	@echo ---------------------------------------------------------------
	@echo CLEAN
	-del /q /f $(OBJDIR)\*.*
	-del /q /f $(OUTDIR)\$(TARGET).$(TARGETTYPE)
	-rd $(OBJDIR)
	@echo ---------------------------------------------------------------

#============================================================================
#                           DEFAULT SUFFIX RULES
#============================================================================

SRC_CPP_FILE = $(@F:.o=.cpp)                # Input source file specification
SRC_C_FILE = $(@F:.o=.c)
OBJ_FILE = $(OBJ_CMD) $(OBJDIR)\$(@F)   # Output object file specification

.SUFFIXES :
.SUFFIXES : .o .dep .cpp .c

#--------------------------------------------------------------------------
# C code inference rules
#----------------------------------------------------------------------------

.cpp.o:
	@echo $(@F)
	@$(CPP) $(CFLAGS) $(INC) $(OBJ_FILE) $(SRC_CPP_FILE)

{$(AEESRCPATH)}.c.o:
	@echo $(@F)
	@$(CC) $(CCFLAGS) $(INC) $(OBJ_FILE) $(AEESRCPATH)\$(SRC_C_FILE)

#===============================================================================
#                           MODULE SPECIFIC RULES
#===============================================================================

$(TARGET).$(TARGETTYPE) : $(OBJS)
	@echo ---------------------------------------------------------------
	@echo TARGET $@
	@$(LA) $(LAFLAGS) $(OUTDIR)\$(TARGET).$(TARGETTYPE) $(OBJDIR)\*.o
	@echo ---------------------------------------------------------------

RULE1 = ..\framework\source\encoderlib

{$(RULE1)}.cpp.o:
	@echo $(@F)
	@$(CPP) $(CCFLAGS) $(DBGEN) $(INC) $(OBJ_FILE) $(RULE1)\$(SRC_CPP_FILE) 
		

RULE2 = ..\framework\source\encoderlib\libjpeg

{$(RULE2)}.c.o:
	@echo $(@F)
	@$(CC) $(CCFLAGS) $(DBGEN) $(INC) $(OBJ_FILE) $(RULE2)\$(SRC_C_FILE) 
# --------------------------------------------
# DEPENDENCY LIST, DO NOT EDIT BELOW THIS LINE
# --------------------------------------------

AEEAppGen.o:	$(AEESRCPATH)\AEEAppGen.c
AEEModGen.o:	$(AEESRCPATH)\AEEModGen.c
