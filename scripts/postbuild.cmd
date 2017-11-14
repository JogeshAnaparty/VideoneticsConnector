@REM - -----------------------------------------------------
@REM   Hitachi Data Systems - HVS
@REM   Video Connector service Postbuild script
@REM
@REM - -------------------------------------------------------

@REM As of today March 2017 VS2017 has a bug that prevent to manage correctly post build action.
@REM We will copy the sdk and ffmpeg files in the prebuild

set currentDir=%~dp0
set sdkDir=%1
set targetPath=%2
set targetDir=%2\..\
set toolsDir=%~dp0..\tools\

if %sdkDir% == "" goto tools

cd %currentDir%

@REM ---------------------------
@REM Copy SDK to target folder
@REM ---------------------------
echo Copying SDK from  %sdkDir%x86 to %targetDir% 
xcopy %sdkDir%x86\*.* %targetDir%  /y /c /r
xcopy %sdkDir%x64\*.* %targetDir%  /y /c /r

:tools

cd %toolsDir%

@REM ---------------------------
@REM Copy ffmpeg from tools dir
@REM ---------------------------
echo Copying ffmpeg to %targetDir% 
xcopy ffmpeg.exe %targetDir% /y /c /r


cd %toolsDir%CodeSigning
echo move to %toolsDir%CodeSigning\

@REM ---------------------
@REM Sign the application
@REM ---------------------
echo Sign assembly  %targetPath%

@REM Check this, at the moment is not working ??
@REM signtool.exe sign /f code_signing.pfx /p H1t4cH1C0dE51gnINcErt /tr http://tsa.starfieldtech.com /td SHA256 %targetPath% 2>nul 1>nul