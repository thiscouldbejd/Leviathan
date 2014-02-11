echo off
rem *****************************
rem
rem Information
rem -----------
rem Script: Generate-Resources.bat
rem Creation Date: 21/06/2010
rem Last Modified: 21/06/2010
rem
rem *****************************
rem
rem Descrption
rem ----------
rem Creates Resource Files in Target Directory
rem
rem *****************************
rem
rem Arguments
rem ---------
rem 1 = Solution Resource Directory = $(SolutionDir)Resources
rem 2 = Project Resource Directory = $(ProjectDir)Resources
rem 3 = Default Language = en-GB
rem
rem *****************************
echo on

del %2\*.resources

for %%A in (%1\*.%3.txt) do copy %%A %1\%%~nA /V /Y

for %%B in (%1\*.%3) do (copy %%B %1\%%~nB.txt /V /Y & del %%B)

for %%C in (%1\*.txt) do "%RESGEN_PATH%\ResGen.exe" %%C %2\%%~nC.resources