set /p postName="Enter new posts name: "
REM set postName="testPost"
powershell.exe -file "build.ps1" -Target NewPost -ScriptArgs "-PostName=%postName%"
set /p postName="Press any key to continue"
