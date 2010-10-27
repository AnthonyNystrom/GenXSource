set VERSION=0.9b-alpha
set CONFIGURATION=release

if "%1"=="upload" (
	build\NAnt\NAnt -f:build\nprof.build -D:build.version=%VERSION% -D:build.configuration=%CONFIGURATION% upload
) else (
	build\NAnt\NAnt -f:build\nprof.build -D:build.version=%VERSION% -D:build.configuration=%CONFIGURATION% package
)

if errorlevel 1 pause