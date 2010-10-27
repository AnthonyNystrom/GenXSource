
SET VAR_JAR_NAME=%1
SET VAR_SRC_JAD_NAME=%2
SET VAR_DST_JAD_NAME=%3

SET ALIAS_CERT=genetibase
SET KEYSTORE_PWD=tony6472

SET VAR_JADTOOL=D:\SonyEricsson\JavaME_SDK_CLDC\PC_Emulation\WTK2\bin\JADTool.jar
SET VAR_JADTOOL2=D:\SonyEricsson\JavaME_SDK_CLDC\PC_Emulation\WTK2\bin\JADTool2.jar

REM SET PATH=%PATH%;D:\jdk1.5.0_05\bin
REM ECHO vars1 = %VAR_JAR_NAME% %VAR_SRC_JAD_NAME% %VAR_DST_JAD_NAME%
REM ECHO vars2 = %ALIAS_CERT% %KEYSTORE_PWD% %VAR_JADTOOL% %VAR_JADTOOL2%



REM sign the jar  
D:\jdk1.5.0_05\bin\jarsigner -keypass %KEYSTORE_PWD% -storepass %KEYSTORE_PWD% %VAR_JAR_NAME% %ALIAS_CERT%

REM Verify signed jar
D:\jdk1.5.0_05\bin\jarsigner -verify -verbose -certs %VAR_JAR_NAME% > tmpCertVerify.txt

REM add certificate chain - 1-1
java -jar %VAR_JADTOOL2% -addcert -alias %ALIAS_CERT% -chainnum 1 -certnum 1 -inputjad %VAR_SRC_JAD_NAME% -outputjad %VAR_SRC_JAD_NAME%tmp1.jad

REM add certificate chain - 1-2
java -jar %VAR_JADTOOL2% -addcert -alias %ALIAS_CERT% -chainnum 1 -certnum 2 -inputjad %VAR_SRC_JAD_NAME%tmp1.jad -outputjad %VAR_SRC_JAD_NAME%tmp2.jad

REM add RSA signature to jad
java -jar %VAR_JADTOOL2% -addjarsig -jarfile %VAR_JAR_NAME% -keypass %KEYSTORE_PWD% -alias %ALIAS_CERT% -inputjad %VAR_SRC_JAD_NAME%tmp2.jad -outputjad %VAR_DST_JAD_NAME%

REM set midlet jar filesize in jad
java -cp .;D:\JavaTool\_filesizer JarFileSizerExe %VAR_DST_JAD_NAME% %VAR_JAR_NAME%
java -cp .;D:\JavaTool\_filesizer JarFileSizerExe %VAR_SRC_JAD_NAME% %VAR_JAR_NAME%

del %VAR_SRC_JAD_NAME%tmp1.jad
del %VAR_SRC_JAD_NAME%tmp2.jad
