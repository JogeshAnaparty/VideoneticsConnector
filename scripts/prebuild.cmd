@REM - -----------------------------------------------------
@REM   Hitachi Data Systems - HVS
@REM   Video Connector service Prebuild script
@REM
@REM - -------------------------------------------------------

set currentDir=%~dp0
set sdkDir=%1
set targetPath=%2
set targetDir=%2\..\
set toolsDir=%~dp0..\tools\


cd %currentDir%

@REM ---------------------------
@REM Unzip connector SDK (it is zipped on git to save space)
@REM ---------------------------

echo Unzip SDK (dir: %sdkDir%)
call UnzipSources.cmd "%sdkDir%"



