!define RUNTIMES_PATH "Runtimes\"
!define PACKAGES_PATH "Prerequisites\"

!define PREREQUISITES_PATH "..\..\prerequisites\"

Var OS_VERSION

;--------------------------------
; Include Modern UI
!include "MUI2.nsh"
!include "x64.nsh"

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "${APP_NAME}_ver${PRODUCT_VERSION}_x64.exe"


;----------------------
; Require Admin provileges
RequestExecutionLevel admin ;Require admin rights on NT6+ (When UAC is turned on)
!include LogicLib.nsh

Function .onInit
UserInfo::GetAccountType
pop $0
${If} $0 != "admin" ; Require admin rights on NT4+
    MessageBox mb_iconstop "Administrator rights required!"
    SetErrorLevel 740 ; ERROR_ELEVATION_REQUIRED
    Quit
${EndIf}

; Check running Windows NT
ClearErrors 
ReadRegStr $OS_VERSION HKLM "SOFTWARE\Microsoft\Windows NT\CurrentVersion" CurrentVersion
${If} ${Errors}
  MessageBox mb_iconstop "The version of the operating system you are using is not supported by the software you are attempting to install."
  Quit
${Else}
  StrCmp $OS_VERSION '5.0' not_supported
  StrCmp $OS_VERSION '5.1' not_supported
  StrCmp $OS_VERSION '5.2' not_supported
  StrCmp $OS_VERSION '6.0' supported  
  StrCmp $OS_VERSION '6.1' supported
  StrCmp $OS_VERSION '6.2' supported
  StrCmp $OS_VERSION '6.3' supported
  StrCmp $OS_VERSION '6.4' supported
  
  not_supported:
    MessageBox mb_iconstop "The version of the operating system you are using is not supported by the software you are attempting to install."
    Quit	
  supported:	
	; Continue with the installation process
${EndIf}
FunctionEnd


;-------------------------
; The default installation directory
InstallDir "$PROGRAMFILES64\${APP_ROOT}\${APP_NAME}${PRODUCT_VERSION}"

AutoCloseWindow false
ShowInstDetails show
ShowUnInstDetails show
; BGGradient 000000 800000 FFFFFF
BGGradient 359FDC 000030 FFFFFF
InstallColors 359FDC 000030
SilentInstall normal
XPStyle on
CheckBitmap "${NSISDIR}\Contrib\Graphics\Checks\classic-cross.bmp"
InstProgressFlags smooth colored
BrandingText " "

AllowRootDirInstall false

; Registry key to check for directory (so if you install again, it will overwrite the old one automatically)
InstallDirRegKey HKLM "Software\Wow6432Node\${APP_ROOT}\${APP_NAME}${PRODUCT_VERSION}" "Install_Dir"

; Request application privileges for Windows Vista
RequestExecutionLevel admin

;--------------------------------
; Interface Settings

  !define MUI_ABORTWARNING

;--------------------------------
; Pages

; Page directory
; Page instfiles
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

; UninstPage uninstConfirm
; UninstPage instfiles

;--------------------------------

; Languages

  !insertmacro MUI_LANGUAGE "English"

;--------------------------------

; The stuff to install
Section "" ; No components page, name is not important

  SectionIn RO

  ; Set output path to the installation\packages directory.
  SetOutPath $INSTDIR\${PACKAGES_PATH}
  
 !ifdef VCCLegacy
 File ${PREREQUISITES_PATH}\VCRedist\vc2008redist\vc2008redist_x64.exe
 File ${PREREQUISITES_PATH}\VCRedist\vc2008redist\vc2008redist_x86.exe
 File ${PREREQUISITES_PATH}\VCRedist\vc2008redist\vc2008redist_x64_sp1.exe
 File ${PREREQUISITES_PATH}\VCRedist\vc2008redist\vc2008redist_x86_sp1.exe
 File ${PREREQUISITES_PATH}\VCRedist\vc2010redist\vc2010redist_x64.exe
 File ${PREREQUISITES_PATH}\VCRedist\vc2010redist\vc2010redist_x86.exe
!endif

 File ${PREREQUISITES_PATH}\VCRedist\vc2015redist\vc2015redist_x64.exe
 File ${PREREQUISITES_PATH}\VCRedist\vc2015redist\vc2015redist_x86.exe
 File ${PREREQUISITES_PATH}\KB2999226\Windows6.0-KB2999226-x64.msu
 File ${PREREQUISITES_PATH}\KB2999226\Windows6.0-KB2999226-x86.msu
 File ${PREREQUISITES_PATH}\KB2999226\Windows6.1-KB2999226-x64.msu
 File ${PREREQUISITES_PATH}\KB2999226\Windows6.1-KB2999226-x86.msu
 File ${PREREQUISITES_PATH}\KB2999226\Windows8.1-KB2999226-x64.msu
 File ${PREREQUISITES_PATH}\KB2999226\Windows8.1-KB2999226-x86.msu
 File ${PREREQUISITES_PATH}\KB2999226\Windows8-RT-KB2999226-x64.msu
 File ${PREREQUISITES_PATH}\KB2999226\Windows8-RT-KB2999226-x86.msu


    ; Set output path to the installation directory.
  SetOutPath $INSTDIR

!ifdef CCR2008
  ; Install Microsoft CCR2008 Runtime Redists
  ExecWait 'MsCCR2008R2.exe /S /v"/qb"'
  ExecWait 'MsCCR2008R3.exe /S /v"/qb"'
!endif

  ; Put file there
  !define REGPATH Software\Wow6432Node\${APP_ROOT}\${APP_NAME}${PRODUCT_VERSION}
  !define REG_UN_PATH Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}${PRODUCT_VERSION}
  File /r /x *.vshost.exe.* /x *.pdb /x *.xml /x *.dll.config ${SOURCE_PATH}\*
  File ${SOURCE_PATH}\log4net.xml
  
  ; Write the installation path into the registry
  WriteRegStr HKLM "SOFTWARE\Wow6432Node\${APP_ROOT}\${APP_NAME}${PRODUCT_VERSION}" "Install_Dir" "$INSTDIR"

  ; Write the uninstall keys for Windows
  WriteRegStr HKLM ${REG_UN_PATH} "DisplayName" "${APP_NAME} ${PRODUCT_VERSION}"
  WriteRegStr HKLM ${REG_UN_PATH} "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr HKLM ${REG_UN_PATH} "DisplayIcon" "$INSTDIR\${APP_NAME}.exe,0"
  WriteRegDWORD HKLM ${REG_UN_PATH} "EstimatedSize" 8400
  WriteRegStr HKLM ${REG_UN_PATH} "Publisher" "Hitachi Data System"
  WriteRegStr HKLM ${REG_UN_PATH} "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM ${REG_UN_PATH} "NoModify" 1
  WriteRegDWORD HKLM ${REG_UN_PATH} "NoRepair" 1
  WriteUninstaller "uninstall.exe"
  
  ; Install both VCRedist 2015 x64 and x86 and C Universal Update(KB2999226) x64 and x86 to support all video connector services
  StrCmp $OS_VERSION '6.0' lbl_winnt_vista  
  StrCmp $OS_VERSION '6.1' lbl_winnt_7
  StrCmp $OS_VERSION '6.2' lbl_winnt_8
  StrCmp $OS_VERSION '6.3' lbl_winnt_81
  StrCmp $OS_VERSION '6.4' lbl_winnt_10

  lbl_winnt_vista:    
    ExecWait 'msiexec /i Prerequisites\Windows6.0-KB2999226-x64.msu /qn'
    ExecWait 'msiexec /i Prerequisites\Windows6.0-KB2999226-x86.msu /qn'
    Goto continue
  lbl_winnt_7:
    ExecWait 'msiexec /i Prerequisites\Windows6.1-KB2999226-x64.msu /qn'
    ExecWait 'msiexec /i Prerequisites\Windows6.1-KB2999226-x86.msu /qn'
    Goto continue
  lbl_winnt_8:
    ExecWait 'msiexec /i Prerequisites\Windows8-RT-KB2999226-x64.msu /qn'
    ExecWait 'msiexec /i Prerequisites\Windows8-RT-KB2999226-x86.msu /qn'
    Goto continue
  lbl_winnt_81:
    ExecWait 'msiexec /i Prerequisites\Windows8.1-KB2999226-x64.msu /qn'
    ExecWait 'msiexec /i Prerequisites\Windows8.1-KB2999226-x86.msu /qn'
    Goto continue
  lbl_winnt_10:
    ExecWait 'msiexec /i Prerequisites\Windows8.1-KB2999226-x64.msu /qn'
    ExecWait 'msiexec /i Prerequisites\Windows8.1-KB2999226-x86.msu /qn'
  continue:
    ExecWait 'Prerequisites\vc2015redist_x64.exe /S /v"/qb!"'
    ExecWait 'Prerequisites\vc2015redist_x86.exe /S /v"/qb!"'
	
	!ifdef VCCLegacy
		ExecWait 'Prerequisites\vc2008redist_x64.exe /S /v"/qb!"'
		ExecWait 'Prerequisites\vc2008redist_x86.exe /S /v"/qb!"'
		ExecWait 'Prerequisites\vc2008redist_x64_sp1.exe /S /v"/qb!"'
		ExecWait 'Prerequisites\vc2008redist_x86_sp1.exe /S /v"/qb!"'
		ExecWait 'Prerequisites\vc2010redist_x64.exe /S /v"/qb"'
		ExecWait 'Prerequisites\vc2010redist_x86.exe /S /v"/qb"'
	!endif

  ; Install service
  !ifdef EXE_NAME
    ExecShell "runas" "$INSTDIR\${EXE_NAME}.exe" 'install --sudo'
  !else
    ExecShell "runas" "$INSTDIR\${APP_NAME}.exe" 'install --sudo'
!endif

SectionEnd ; end the section

;--------------------------------

; Uninstaller

Section "Uninstall"

  ; Remove service
  !ifdef EXE_NAME 
   ExecWait "$INSTDIR\${EXE_NAME}.exe stop"
   ExecWait "$INSTDIR\${EXE_NAME}.exe uninstall"
  !else
    ExecWait "$INSTDIR\${APP_NAME}.exe stop"
    ExecWait "$INSTDIR\${APP_NAME}.exe uninstall"
  !endif

    

  ; Remove registry keys
  DeleteRegKey HKLM ${REG_UN_PATH}
  DeleteRegKey HKLM "SOFTWARE\Wow6432Node\${APP_ROOT}\${APP_NAME}${PRODUCT_VERSION}"

  ; Delete Event Log
  DeleteRegKey HKLM "SYSTEM\CurrentControlSet\services\eventlog\${APP_NAME}Log\${APP_NAME}Log"
  DeleteRegKey HKLM "SYSTEM\CurrentControlSet\services\eventlog\${APP_NAME}Log"

  ; Remove directories used
  RMDir /r $INSTDIR

SectionEnd