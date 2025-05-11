@echo off
rem This script aids in bundling plugins for Thunderstore distribution.
SETLOCAL

set projectDir=%1
if not exist %projectDir% echo "Project does not exist: %projectDir%" && exit

cd %projectDir%
if %projectDir:~0,2%==.\ set projectDir=%projectDir:~2%
if %projectDir:~-1%==\ set projectDir=%projectDir:~0,-1%
set target=.\dist\kruft.%projectDir%.zip
del %target%
"C:\Program Files\7-Zip\7z.exe" a %target% ".\README.md" ".\bundle\*"
cd ..

ENDLOCAL