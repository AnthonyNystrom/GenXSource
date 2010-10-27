# ============================================================================
#  Name	 : build_help.mk
#  Part of  : Aaf
#
#  Description: This make file will build the application help file (.hlp)
# 
# ============================================================================

do_nothing :
	@rem do_nothing

MAKMAKE :
	cshlpcmp Aaf.cshlp
ifeq (WINS,$(findstring WINS, $(PLATFORM)))
	copy Aaf_0xe2536f82.hlp $(EPOCROOT)epoc32\$(PLATFORM)\c\resource\help
endif

BLD : do_nothing

CLEAN :
	del Aaf_0xe2536f82.hlp
	del Aaf_0xe2536f82.hlp.hrh

LIB : do_nothing

CLEANLIB : do_nothing

RESOURCE : do_nothing
		
FREEZE : do_nothing

SAVESPACE : do_nothing

RELEASABLES :
	@echo Aaf_0xe2536f82.hlp

FINAL : do_nothing
