
#pragma once
using namespace System;
using namespace System::Windows::Forms;
using namespace System::Data;

System::String^ TWCharToMStr(const TCHAR * tStr);
TCHAR * MStrToTWChar( System::String^ mStr);
String^ CStringToMStr( CString cStr);
CString  MStrToCString( String^ mStr);
void ShowError(String^ message);
void ShowInfo(String^ message);
DialogResult ShowAsk(String^ message);
	