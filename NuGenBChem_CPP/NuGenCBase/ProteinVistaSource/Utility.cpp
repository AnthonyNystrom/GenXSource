#include "StdAfx.h"
#include "Utility.h"
#include "ProteinVistaView.h"
#include "Interface.h"

void DisplayScript(TCHAR * strBuff)
{
	HWND hwnd = FindWindow(NULL, "DisplayScript");
	if ( hwnd == NULL )
		return;

	COPYDATASTRUCT cds={0,};
	cds.dwData = 0;
	cds.cbData = strlen(strBuff)+1;
	cds.lpData = strBuff;

	SendMessage(hwnd, WM_COPYDATA, NULL, (LPARAM)&cds);
}


//	utility function
void WriteString(CFile &fp, CString & str)
{
	long strLen = str.GetLength();

	fp.Write(&strLen, sizeof(long));
	fp.Write(str, strLen);
}

void ReadString(CFile & fp, CString & str)
{
	long strLen;
	fp.Read(&strLen, sizeof(long));
	TCHAR buff[MAX_PATH*4] = {0,};
	fp.Read(buff, strLen);
	str = CString(buff);
}
void OutputTextMsg(CString msg)
{
	GetMainActiveView()->OutPutLog(msg);
}
BOOL CheckFileExist(CString filename)
{
	DWORD fileAttr = GetFileAttributes(filename);
	if (0xFFFFFFFF == fileAttr)
	{	//	존재하지 않는다.
		return FALSE;
	}

	try
	{
		CFile file(filename, CFile::modeRead);
		if ( file.m_hFile != CFile::hFileNull )
		{	//	존재하면,
			//
			if ( file.GetLength() == 0L )
			{
				file.Close();
				if ( DeleteFile(filename) != 0 )
					return FALSE;		//	지워졌다면, 존재하지 않는것이다.
			}

			file.Close();
			return TRUE;
		}
	}
	catch(...)
	{
		return FALSE;
	}

	return FALSE;
}
 


