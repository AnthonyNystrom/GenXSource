# DotNET version checking macro.
# Written by AnarkiNet(AnarkiNet@gmail.com) originally, modified by eyal0 (for use in http://www.sourceforge.net/projects/itwister), hacked up by PSIMS APS MEDILINK 20090703 for dotnet 3.5
# Downloads and runs the Microsoft .NET Framework version 2.0 Redistributable and runs it if the user does not have the correct version.
# To use, call the macro with a string:
# 'CheckDotNET3Point5'
# All register variables are saved and restored by CheckDotNet
# No output

!macro CheckDotNET3Point5
  !define DOTNET_URL "http://download.microsoft.com/download/7/0/3/703455ee-a747-4cc8-bd3e-98a615c3aedb/dotNetFx35setup.exe"
  DetailPrint "Checking your .NET Framework version..."
  ;callee register save
  Push $0
  Push $1
  Push $2
  Push $3
  Push $4
  Push $5
  Push $6 ;backup of intsalled ver
  Push $7 ;backup of DoNetReqVer
  Push $8

  StrCpy $7 "3.5.0"

  loop:
  EnumRegKey $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP" $8
  StrCmp $1 "" done ;jump to end if no more registry keys
  IntOp $8 $8 + 1
  StrCpy $0 $1
  goto loop
  done:

  ${If} $0 == 0
    DetailPrint ".NET Framework not found, download is required for program to run."
    Goto NoDotNET
  ${EndIf}

  StrCpy $1 $0 1 1

  ${If} $1 > 3
    Goto NewDotNET
  ${EndIf}

  StrCpy $2 $0 1 3

  ${If} $1 == 3
    ${If} $2 > 4
      Goto NewDotNET
    ${EndIf}
  ${EndIf}

  StrCpy $3 $0 "" 5

  ${If} $3 == ""
    StrCpy $3 "0"
  ${EndIf}

  StrCpy $6 "$1.$2.$3"

  Goto OldDotNET


  ${If} $0 < 0
    DetailPrint ".NET Framework Version found: $6, but is older than the required version: $7"
    Goto OldDotNET
  ${Else}
    DetailPrint ".NET Framework Version found: $6, equal or newer to required version: $7."
    Goto NewDotNET
  ${EndIf}

NoDotNET:
    MessageBox MB_YESNOCANCEL|MB_ICONEXCLAMATION \
    ".NET Framework not installed.$\nRequired Version: $7 or greater.$\nDownload .NET Framework version from www.microsoft.com?" \
    /SD IDYES IDYES DownloadDotNET IDNO NewDotNET
    goto GiveUpDotNET ;IDCANCEL
OldDotNET:
    MessageBox MB_YESNOCANCEL|MB_ICONEXCLAMATION \
    "Your .NET Framework version: $6.$\nRequired Version: $7 or greater.$\nDownload .NET Framework version from www.microsoft.com?" \
    /SD IDYES IDYES DownloadDotNET IDNO NewDotNET
    goto GiveUpDotNET ;IDCANCEL

DownloadDotNET:
  DetailPrint "Beginning download of latest .NET Framework version."
  NSISDL::download ${DOTNET_URL} "$TEMP\dotNetFx35setup.exe"
  DetailPrint "Completed download."
  Pop $0
  ${If} $0 == "cancel"
    MessageBox MB_YESNO|MB_ICONEXCLAMATION \
    "Download cancelled.  Continue Installation?" \
    IDYES NewDotNET IDNO GiveUpDotNET
  ${ElseIf} $0 != "success"
    MessageBox MB_YESNO|MB_ICONEXCLAMATION \
    "Download failed:$\n$0$\n$\nContinue Installation?" \
    IDYES NewDotNET IDNO GiveUpDotNET
  ${EndIf}
  DetailPrint "Pausing installation while downloaded .NET Framework installer runs."
  ExecWait "$TEMP\dotNetFx35setup.exe /qb"
  DetailPrint "Completed .NET Framework install/update. Removing .NET Framework installer."
  Delete "$TEMP\dotNetFx35setup.exe"
  DetailPrint ".NET Framework installer removed."
  goto NewDotNet

GiveUpDotNET:
  Abort "Installation cancelled by user."

NewDotNET:
  DetailPrint "Proceeding with remainder of installation."

  ; register re-load, should be popped in reverse order
  Pop $8
  Pop $7 ;backup of DoNetReqVer
  Pop $6 ;backup of intsalled ver
  Pop $5
  Pop $4
  Pop $3
  Pop $2
  Pop $1
  Pop $0

!macroend