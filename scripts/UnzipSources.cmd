IF "%1"=="" GOTO NoFolder
IF EXIST "%programw6432%\7-Zip\7z.exe" (SET ZIP="%programw6432%\7-Zip\7z.exe") ELSE (set ZIP="%programfiles(x86)%\7-Zip\7z.exe")
CD %1
	FOR %%f IN (^"*.zip^") DO (
		REM %%~nf : get filename only
		IF NOT EXIST %%~nf (
			%ZIP% x -y %%f -o%1 >nul
		)
	)
GOTO End

:NoFolder
	ECHO No folder specified
GOTO End

:End