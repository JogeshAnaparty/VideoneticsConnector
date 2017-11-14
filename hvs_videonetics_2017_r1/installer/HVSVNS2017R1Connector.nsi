Section "SetVersion"
	!ifdef version
		!define PRODUCT_VERSION ${version}
	!else
		!define PRODUCT_VERSION "5.2.6"
	!endif
SectionEnd

!define PRODUCT_PUBLISHER "Hitachi Vantara"
!define PRODUCT_WEB_SITE "http://www.hitachivantara.com/"
!define APP_ROOT "Hitachi Data Systems"

!define PRODUCT_NAME  "Hitachi HVSVNS2017R1Connector"
!define APP_NAME      "HVSVNS2017R1Connector"
!define SOURCE_PATH   "..\src\vs2015\HVS.Video.VMS.Videonetics.2017.R1.WinService\bin\Release"
!define MUI_ICON      "..\src\vs2015\HVS.Video.VMS.Videonetics.2017.R1.WinService\bin\Release\App.ico"

!include ..\..\scripts\installers\HVSInstallerBase.nsh