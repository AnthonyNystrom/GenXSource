#include "stdafx.h"
#include "Utils.h"

using namespace System;
using namespace System::Windows::Forms;
using namespace System::Data;


System::String^ TWCharToMStr(const TCHAR * tStr)
{
	System::String ^ str =gcnew System::String(tStr);
	return str; 
}

TCHAR * MStrToTWChar( System::String^ mStr)
{
	System::IntPtr p =System::Runtime::InteropServices::Marshal::StringToHGlobalUni(mStr);
	TCHAR* pUni = static_cast<TCHAR*>(p.ToPointer());
	System::Runtime::InteropServices::Marshal::FreeHGlobal(p);
	return pUni;
}
String^ CStringToMStr( CString cStr)
{
	String ^ stringEx = gcnew String( cStr );
	return stringEx;
}

CString  MStrToCString( String^ mStr)
{
	char* str2 = (char*)(void*)System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(mStr);
	CString target = str2;
	System::Runtime::InteropServices::Marshal::FreeHGlobal((IntPtr)str2);
	return target;
}

void ShowError(String^ message)
{
	MessageBox::Show(message, "Error", MessageBoxButtons::OK, MessageBoxIcon::Error);
}

void ShowInfo(String^ message)
{
	MessageBox::Show(message, "Information", MessageBoxButtons::OK, MessageBoxIcon::Information);
}

DialogResult ShowAsk(String^ message)
{
	return MessageBox::Show(message, "Question", MessageBoxButtons::YesNo, MessageBoxIcon::Question);
}
