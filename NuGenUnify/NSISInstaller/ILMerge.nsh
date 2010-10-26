
!macro CheckILMerge
  !define ILMERGE_URL "http://download.microsoft.com/download/1/3/4/1347C99E-9DFB-4252-8F6D-A3129A069F79/ILMerge.msi"
  DetailPrint "Checking ILMerge..."
  ;callee register save
  Push $0
  Push $1
  Push $2
  Push $3
  Push $4
  Push $5

${If} ${FileExists} `$PROGRAMFILES\Microsoft\ILMerge\ILMerge.exe`
      DetailPrint "ILMerge is found."
      Goto NewILMerge
${Else}
       DetailPrint "ILMerge not found, download is required for program to run."
       Goto NoILMerge
${EndIf}

NoILMerge:
    MessageBox MB_YESNOCANCEL|MB_ICONEXCLAMATION \
    "ILMerge not installed.$\nDownload ILMerge version from www.microsoft.com?" \
    /SD IDYES IDYES DownloadILMerge IDNO NewILMerge
    goto GiveUpILMerge ;IDCANCEL
DownloadILMerge:
  DetailPrint "Beginning download of latest ILMerge."
  NSISDL::download ${ILMERGE_URL} "$TEMP\ILMerge.msi"
  DetailPrint "Completed download."
  Pop $0
  ${If} $0 == "cancel"
    MessageBox MB_YESNO|MB_ICONEXCLAMATION \
    "Download cancelled.  Continue Installation?" \
    IDYES NewILMerge IDNO GiveUpILMerge
  ${ElseIf} $0 != "success"
    MessageBox MB_YESNO|MB_ICONEXCLAMATION \
    "Download failed:$\n$0$\n$\nContinue Installation?" \
    IDYES NewILMerge IDNO GiveUpILMerge
  ${EndIf}
  DetailPrint "Pausing installation while downloaded ILMerge installer runs."
  
  ExecWait '"msiexec" /i "$TEMP\ILMerge.msi" /qn'
  DetailPrint "Completed ILMerge install/update. Removing ILMerge installer."
  Delete "$TEMP\ILMerge.msi"
  DetailPrint "ILMerge installer removed."
  goto NewILMerge
 
GiveUpILMerge:
  Abort "Installation cancelled by user."
 
NewILMerge:
  DetailPrint "Proceeding with remainder of installation."
  Pop $0
  Pop $1
  Pop $2
  Pop $3
  Pop $4
  Pop $5
!macroend