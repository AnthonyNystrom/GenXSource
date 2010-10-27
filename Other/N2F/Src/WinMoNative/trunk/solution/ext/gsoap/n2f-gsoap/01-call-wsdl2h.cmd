@echo off

echo Calling wsdl2h to generate soap headers for each service

@echo on
rem N2F MemberServices service	http://next2friends.com:90/memberservices.asmx
.\..\gsoap-2.7.10\gsoap\bin\win32\wsdl2h -I.\..\gsoap-2.7.10\gsoap\import -nMemberServices -NMemberServices -w -f -o .\wsdl-h\MemberServices.h .\wsdl\MemberServices.wsdl


rem N2F PhotoOrganise service	http://next2friends.com:90/photoorganise.asmx
rem wsdl2h -IG:\wrk\n2f\samples\lib\gsoap-2.7\gsoap\import -n PhotoOrganise -N PhotoOrganiseServices -w -f -o .\wsdl-h\PhotoOrganise.h .\wsdl\PhotoOrganise.wsdl
.\..\gsoap-2.7.10\gsoap\bin\win32\wsdl2h -I.\..\gsoap-2.7.10\gsoap\import -nPhotoOrganise -NPhotoOrganise -w -f -o .\wsdl-h\PhotoOrganise.h .\wsdl\PhotoOrganise.wsdl


@echo on
rem N2F MemberServices service v.2	http://services.next2friends.com/n2fwebservices/memberservices.asmx
.\..\gsoap-2.7.10\gsoap\bin\win32\wsdl2h -I.\..\gsoap-2.7.10\gsoap\import -nMemberServicesV2 -NMemberServicesV2 -w -f -o .\wsdl-h\MemberServices.v2.h .\wsdl\MemberServices.v2.wsdl


rem N2F PhotoOrganise service v.2	http://services.next2friends.com/n2fwebservices/photoorganise.asmx
.\..\gsoap-2.7.10\gsoap\bin\win32\wsdl2h -I.\..\gsoap-2.7.10\gsoap\import -nPhotoOrganiseV2 -NPhotoOrganiseV2 -w -f -o .\wsdl-h\PhotoOrganise.v2.h .\wsdl\PhotoOrganise.v2.wsdl

rem N2F MemberService v.3 service http://next2friends.com:90/MemberService.asmx
.\..\gsoap-2.7.10\gsoap\bin\win32\wsdl2h -I.\..\gsoap-2.7.10\gsoap\import -nMemberServiceV3 -NMemberServiceV3 -w -f -o .\wsdl-h\MemberService.V3.h .\wsdl\MemberService.V3.wsdl

rem N2F SnapUpservice http://next2friends.com:90/SnapUpService.asmx
.\..\gsoap-2.7.10\gsoap\bin\win32\wsdl2h -I.\..\gsoap-2.7.10\gsoap\import -nSnapUpService -NSnapUpService -w -f -o .\wsdl-h\SnapUpService.h .\wsdl\SnapUpService.wsdl


@echo off

echo After headers are generated from wsdl, do not forget to evelope each header's content into the custom namespaces!
echo Only after that lauch the 02-gen-soapsrc.cmd
