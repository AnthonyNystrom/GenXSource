REM ECHO Copy to DIST directory.

SET KEYSTORE_ALIAS=genetibase
SET KEYSTORE_NAME=.keystore
SET KEYSTORE_PWD=tony6472

REM ECHO Signing JARs...

jarsigner -keystore %KEYSTORE_NAME% -storepass %KEYSTORE_PWD% N2F_MediaUploader.jar %KEYSTORE_ALIAS%
FOR %%F IN (lib/*.jar) DO jarsigner -keystore %KEYSTORE_NAME% -storepass %KEYSTORE_PWD% lib/%%F %KEYSTORE_ALIAS%

REM ECHO Verfiying JARs...

jarsigner -verify N2F_MediaUploader.jar
FOR %%F IN (lib/*.jar) DO jarsigner -verify lib/%%F