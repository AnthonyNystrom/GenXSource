@echo off

echo Start generation of SOAP code


@echo on
rem gSOAP environment
.\..\gsoap-2.7.10\gsoap\bin\win32\soapcpp2 -C -penv -d.\ready-src\ .\wsdl-h\env.h

rem N2F MemberServices service	http://next2friends.com:90/memberservices.asmx
.\..\gsoap-2.7.10\gsoap\bin\win32\soapcpp2 -C -n -x -pMemberServices -d.\ready-src\ .\wsdl-h\MemberServices.h

rem N2F PhotoOrganise service	http://next2friends.com:90/photoorganise.asmx
.\..\gsoap-2.7.10\gsoap\bin\win32\soapcpp2 -C -n -x -pPhotoOrganise -d.\ready-src\ .\wsdl-h\PhotoOrganise.h

rem N2F MemberServices service v.2	http://services.next2friends.com/n2fwebservices/memberservices.asmx
.\..\gsoap-2.7.10\gsoap\bin\win32\soapcpp2 -C -n -x -pMemberServicesV2 -d.\ready-src\ .\wsdl-h\MemberServices.v2.h

rem N2F PhotoOrganise service v.2	http://services.next2friends.com/n2fwebservices/photoorganise.asmx
.\..\gsoap-2.7.10\gsoap\bin\win32\soapcpp2 -C -n -x -pPhotoOrganiseV2 -d.\ready-src\ .\wsdl-h\PhotoOrganise.v2.h

rem N2F MemberService v.3 service	http://next2friends.com:90/MemberService.asmx
.\..\gsoap-2.7.10\gsoap\bin\win32\soapcpp2 -C -n -x -pMemberServiceV3 -d.\ready-src\ .\wsdl-h\MemberService.v3.h


rem N2F SnapUpService service	http://next2friends.com:90/SnapUpService.asmx
.\..\gsoap-2.7.10\gsoap\bin\win32\soapcpp2 -C -n -x -pSnapUpService -d.\ready-src\ .\wsdl-h\SnapUpService.h

@echo off
echo Finished SOAP code generation!

