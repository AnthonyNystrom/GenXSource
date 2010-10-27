#pragma once

//! Enumeration for supported web-services
enum TWebServiceType {
	EWSUnknown = 0,
	EWS_N2F_MemberServices		= 1,		//	http://next2friends.com:90/memberservices.asmx
	EWS_N2F_MemberServices_v2	= 2,	//	http://services.next2friends.com/n2fwebservices/memberservices.asmx
	EWS_N2F_PhotoOrganise		= 3,		//	http://next2friends.com:90/photoorganise.asmx
	EWS_N2F_PhotoOrganise_v2	= 4,	//	http://services.next2friends.com/n2fwebservices/photoorganise.asmx
	EWS_N2F_MemberService_v3	= 5,	//	http://next2friends.com:90/MemberService.asmx
	EWS_N2F_SnapUpService		= 6,		//	http://next2friends.com:90/SnapUpService.asmx
};
