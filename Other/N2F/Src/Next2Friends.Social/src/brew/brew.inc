BREW_HOME = $(BREWDIR)
ARM_HOME  = $(ARMHOME)
ELF_TO_MOD= $(ELFTOMODDIR)

CC   	="$(ARM_HOME)\bin\armcc"		# C compiler
CPP  	="$(ARM_HOME)\bin\armcpp"		# CPP compiler
LD   	="$(ARM_HOME)\bin\armlink"		# Linker
LA   	="$(ARM_HOME)\bin\armar"		# Library archiver
#HEXTOOL	="$(ARM_HOME)\bin\fromelf"		# ARM ADS utility to create hex file from image
#HEXTOOL	="C:\Program Files\BREW Elf2Mod\bin\elf2mod.exe"		# ARM ADS utility to create hex file from image
HEXTOOL	="$(ELF_TO_MOD)\elf2mod.exe"		# ARM ADS utility to create hex file from image


AEESRCPATH = $(BREW_HOME)\src
AEEINCPATH = $(BREW_HOME)\inc

#CCFLAGS = -asm -fs -c -DDYNAMIC_APP -cpu ARM7TDMI -apcs /ropi/interwork -littleend -zo -fa -Otime -O2 -DBREW -DRELEASE_BUILD -Ec -Wx
#CPPFLAGS = -asm -fs -c -DDYNAMIC_APP -cpu ARM7TDMI -apcs /ropi/interwork -littleend -zo -fa -Otime -O2 -DBREW -DRELEASE_BUILD -Ec -Wx
#LDFLAGS = -ropi -rwpi -first AEEMod_Load -entry AEEMod_Load -remove -info sizes,unused,totals
#LAFLAGS = -r 

#CCFLAGS = -asm -fs -c -DDYNAMIC_APP -cpu ARM7TDMI -apcs /ropi/interwork -littleend -zo -fa -Otime -O2 -DBREW -DRELEASE_BUILD -Ec -Wx
#CPPFLAGS = -asm -fs -c -DDYNAMIC_APP -cpu ARM7TDMI -apcs /ropi/interwork -littleend -zo -fa -Otime -O2 -DBREW -DRELEASE_BUILD -Ec -Wx
CCFLAGS = -asm -fs -c -DDYNAMIC_APP -cpu ARM7TDMI -apcs /ropi/interwork -littleend -zo -fa -Ospace -O2 -DBREW -DRELEASE_BUILD -Ec -Wx
CPPFLAGS = -asm -fs -c -DDYNAMIC_APP -cpu ARM7TDMI -apcs /ropi/interwork -littleend -zo -fa -Ospace -O2 -DBREW -DRELEASE_BUILD -Ec -Wx
LDFLAGS = -reloc -split -first AEEMod_Load -entry AEEMod_Load -remove -info sizes,unused,totals
LAFLAGS = -r 
