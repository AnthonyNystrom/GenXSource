# ============================================================================
#  Name	 : build_help.mk
#  Part of  : Sup
#
#  Description: This make file will build the application help file (.hlp)
# 
# ============================================================================

do_nothing :
	@rem do_nothing

BLD : do_nothing

MAKMAKE :
	cshlpcmp Sup.cshlp
ifeq (WINS,$(findstring WINS, $(PLATFORM)))
	copy Sup_0xeb83db8a.hlp $(EPOCROOT)epoc32\$(PLATFORM)\c\resource\help
endif

CLEAN :
	del Sup_0xeb83db8a.hlp
	del Sup_0xeb83db8a.hlp.hrh

LIB : do_nothing

CLEANLIB : do_nothing

RESOURCE : do_nothing
		
FREEZE : do_nothing

SAVESPACE : do_nothing

RELEASABLES :
	@echo Sup_0xeb83db8a.hlp

FINAL : do_nothing
