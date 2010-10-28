// SyntaxColorizer.cpp: implementation of the CSyntaxColorizer class.
//
// Version:	1.0.0
// Author:	Jeff Schering jeffschering@hotmail.com
// Date:	Jan 2001
// Copyright 2001 by Jeff Schering
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "SyntaxColorizer.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

#define _AFX_SECURE_NO_DEPRECATE
#include "afxcmn.h"
//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CSyntaxColorizer::CSyntaxColorizer()
{
	createDefaultCharFormat();
	SetCommentColor(CLR_COMMENT);
	SetStringColor(CLR_STRING);
	createTables();
	m_pskKeyword = NULL;
	createDefaultKeywordList();
}

CSyntaxColorizer::~CSyntaxColorizer()
{
	//Bug fix by e-yes, applied by HOMO_PROGRAMMATIS <homo_programmatis@rt.mipt.ru>
	//ClearKeywordList() contains operation on tables - it causes errors
	//old code
	/**/	//deleteTables();
	/**/	//ClearKeywordList();
	//new code
	/**/	ClearKeywordList();
	/**/	deleteTables();
	//end of changes by HOMO_PROGRAMMATIS
}

//////////////////////////////////////////////////////////////////////
// Member Functions
//////////////////////////////////////////////////////////////////////
void CSyntaxColorizer::createDefaultCharFormat()
{
	m_cfDefault.dwMask = CFM_CHARSET | CFM_FACE | CFM_SIZE | CFM_OFFSET | CFM_COLOR;
	m_cfDefault.dwMask ^= CFM_ITALIC ^ CFM_BOLD ^ CFM_STRIKEOUT ^ CFM_UNDERLINE;
	m_cfDefault.dwEffects = 0;
	m_cfDefault.yHeight = 160; //10pts * 20 twips/point = 200 twips
	m_cfDefault.bCharSet = ANSI_CHARSET;
	m_cfDefault.bPitchAndFamily = FIXED_PITCH | FF_MODERN;
	m_cfDefault.yOffset = 0;
	m_cfDefault.crTextColor = CLR_PLAIN;
	//strcpy(m_cfDefault.szFaceName,"Courier New");#OBSOLETE
	strcpy_s(m_cfDefault.szFaceName, sizeof(m_cfDefault.szFaceName),"Courier New");
	m_cfDefault.cbSize = sizeof(m_cfDefault);

	m_cfComment = m_cfDefault;
	m_cfString = m_cfDefault;
}

void CSyntaxColorizer::createDefaultKeywordList()
{
	LPTSTR sKeywords = "__asm,else,main,struct,__assume,enum,"
		"__multiple_inheritance,switch,auto,__except,__single_inheritance,"
		"template,__based,explicit,__virtual_inheritance,this,bool,extern,"
		"mutable,thread,break,false,naked,throw,case,__fastcall,namespace,"
		"true,catch,__finally,new,try,__cdecl,float,noreturn,__try,char,for,"
		"operator,typedef,class,friend,private,typeid,const,goto,protected,"
		"typename,const_cast,if,public,union,continue,inline,register,"
		"unsigned,__declspec,__inline,reinterpret_cast,using,declaration,"
		"directive,default,int,return,uuid,delete,__int8,short,"
		"__uuidof,dllexport,__int16,signed,virtual,dllimport,__int32,sizeof,"
		"void,do,__int64,static,volatile,double,__leave,static_cast,wmain,"
		"dynamic_cast,long,__stdcall,while";
	LPTSTR sDirectives = "#define,#elif,#else,#endif,#error,#ifdef,"
		"#ifndef,#import,#include,#line,#pragma,#undef";
	LPTSTR sPragmas = "alloc_text,comment,init_seg1,optimize,auto_inline,"
		"component,inline_depth,pack,bss_seg,data_seg,"
		"inline_recursion,pointers_to_members1,check_stack,"
		"function,intrinsic,setlocale,code_seg,hdrstop,message,"
		"vtordisp1,const_seg,include_alias,once,warning"; 

	AddKeyword(sKeywords,CLR_KEYWORD,GRP_KEYWORD);
	AddKeyword(sDirectives,CLR_KEYWORD,GRP_KEYWORD);
	AddKeyword(sPragmas,CLR_KEYWORD,GRP_KEYWORD);
}

void CSyntaxColorizer::createTables()
{
	m_pTableZero = new unsigned char[256]; m_pTableOne   = new unsigned char[256];
	m_pTableTwo  = new unsigned char[256]; m_pTableThree = new unsigned char[256];
	m_pTableFour = new unsigned char[256]; m_pAllowable  = new unsigned char[256];

	memset(m_pTableZero,SKIP,256); memset(m_pTableOne,SKIP,256);
	memset(m_pTableTwo,SKIP,256);  memset(m_pTableThree,SKIP,256);
	memset(m_pTableFour,SKIP,256); memset(m_pAllowable,false,256);

	*(m_pTableZero + '"') = DQSTART; *(m_pTableZero + '\'')  = SQSTART;
	*(m_pTableZero + '/') = CMSTART; *(m_pTableOne + '"')    = DQEND;
	*(m_pTableTwo + '\'') = SQEND;   *(m_pTableThree + '\n') = SLEND;
	*(m_pTableFour + '*') = MLEND;

	*(m_pAllowable + '\n') = true; *(m_pAllowable + '\r') = true;
	*(m_pAllowable + '\t') = true; *(m_pAllowable + '\0') = true;
	*(m_pAllowable + ' ')  = true; *(m_pAllowable + ';')  = true;
	*(m_pAllowable + '(')  = true; *(m_pAllowable + ')')  = true;
	*(m_pAllowable + '{')  = true; *(m_pAllowable + '}')  = true;
	*(m_pAllowable + '[')  = true; *(m_pAllowable + ']')  = true;
	*(m_pAllowable + '*')  = true;
}

void CSyntaxColorizer::deleteTables()
{
	delete m_pTableZero;  delete m_pTableOne;  delete m_pTableTwo;
	delete m_pTableThree; delete m_pTableFour; delete m_pAllowable;
}

void CSyntaxColorizer::AddKeyword(LPCTSTR Keyword, COLORREF cr, int grp)
{
	LPTSTR token,next_token;

	//make a copy of Keyword so that strtok will operate correctly
	int iSize = strlen(Keyword) + 1;
	LPTSTR keyword = new TCHAR[iSize];
	//strcpy(keyword,Keyword);#OBSOLETE
	strcpy_s(keyword,iSize,Keyword);

	CHARFORMAT cf = m_cfDefault;
	cf.crTextColor = cr;

	//token = strtok(keyword,",");#OBSOLETE
	token = strtok_s(keyword,",",&next_token);
	while(token != NULL)
	{
		if(_stricmp(token,"rem") == 0)
			*(m_pTableTwo + '\n') = SLEND; //set single quote as comment start
		addKey(token,cf,grp);
		//token = strtok(NULL,",");#OBSOLETE
		token = strtok_s(NULL,",",&next_token);
	}
	delete keyword;
}

void CSyntaxColorizer::AddKeyword(LPCTSTR Keyword, CHARFORMAT cf, int grp)
{
	LPTSTR token,next_token;

	//make a copy of Keyword so that strtok will operate correctly
	int iSize = strlen(Keyword) + 1;
	LPTSTR keyword = new TCHAR[iSize];
	//strcpy(keyword,Keyword);#OBSOLETE
	strcpy_s(keyword,iSize,Keyword);

	//token = strtok(keyword,",");#OBSOLETE
	token = strtok_s(keyword,",",&next_token);
	while(token != NULL)
	{
		if(_stricmp(token,"rem") == 0)
			*(m_pTableTwo + '\n') = SLEND; //set single quote as comment start
		addKey(token,cf,grp);
		//token = strtok(NULL,",");#OBSOLETE
		token = strtok_s(NULL,",",&next_token);

	}
	delete keyword;
}

void CSyntaxColorizer::addKey(LPCTSTR Keyword, CHARFORMAT cf, int grp) //add in ascending order
{
	SKeyword* pskNewKey = new SKeyword;
	SKeyword* prev,*curr;

	//the string pointed to by Keyword is only temporary, so make a copy 
	// of it for our list

	int iSize = strlen(Keyword)+1;
	pskNewKey->keyword = new TCHAR[iSize];
	//strcpy(pskNewKey->keyword,Keyword);#OBSOLETE
	strcpy_s(pskNewKey->keyword,iSize ,Keyword);

	pskNewKey->keylen = strlen(Keyword);
	pskNewKey->cf = cf;
	pskNewKey->group = grp;
	pskNewKey->pNext = NULL;
	*(m_pTableZero + pskNewKey->keyword[0]) = KEYWORD;

	//if list is empty, add first node
	if(m_pskKeyword == NULL)
		m_pskKeyword = pskNewKey; 
	else
	{
		//check to see if new node goes before first node
		if(strcmp(Keyword,m_pskKeyword->keyword) < 0)
		{
			pskNewKey->pNext = m_pskKeyword;
			m_pskKeyword = pskNewKey;
		}
		//check to see if new keyword already exists at the first node
		else if(strcmp(Keyword,m_pskKeyword->keyword) == 0)
		{
			//the keyword exists, so replace the existing with the new
			pskNewKey->pNext = m_pskKeyword->pNext;
			delete m_pskKeyword->keyword; delete m_pskKeyword;
			m_pskKeyword = pskNewKey;
		}
		else
		{
			prev = m_pskKeyword;
			curr = m_pskKeyword->pNext;
			while(curr != NULL && strcmp(curr->keyword,Keyword) < 0)
			{
				prev = curr;
				curr = curr->pNext;
			}
			if(curr != NULL && strcmp(curr->keyword,Keyword) == 0)
			{
				//the keyword exists, so replace the existing with the new
				prev->pNext = pskNewKey;
				pskNewKey->pNext = curr->pNext;
				delete curr->keyword; delete curr;
			}
			else
			{
				pskNewKey->pNext = curr;
				prev->pNext = pskNewKey;
			}
		}
	}
}

void CSyntaxColorizer::ClearKeywordList()
{
	SKeyword* pTemp = m_pskKeyword;

	while(m_pskKeyword != NULL)
	{
		*(m_pTableZero + m_pskKeyword->keyword[0]) = SKIP;
		if(_stricmp(m_pskKeyword->keyword,"rem") == 0)
			*(m_pTableTwo + '\n') = SKIP;
		pTemp = m_pskKeyword->pNext;
		delete m_pskKeyword->keyword;
		delete m_pskKeyword;
		m_pskKeyword = pTemp;
	}
}

CString CSyntaxColorizer::GetKeywordList()
{
	CString sList;
	SKeyword* pTemp = m_pskKeyword;

	while(pTemp != NULL)
	{
		sList += pTemp->keyword;
		sList += ",";
		pTemp = pTemp->pNext;
	}
	sList.TrimRight(',');
	return sList;
}

CString CSyntaxColorizer::GetKeywordList(int grp)
{
	CString sList;
	SKeyword* pTemp = m_pskKeyword;

	while(pTemp != NULL)
	{
		if(pTemp->group == grp)
		{
			sList += pTemp->keyword;
			sList += ",";
		}
		pTemp = pTemp->pNext;
	}
	sList.TrimRight(',');
	return sList;
}

void CSyntaxColorizer::SetCommentColor(COLORREF cr)
{
	CHARFORMAT cf = m_cfComment;

	cf.dwMask = CFM_COLOR;
	cf.crTextColor = cr;

	SetCommentStyle(cf);
}

void CSyntaxColorizer::SetStringColor(COLORREF cr)
{
	CHARFORMAT cf = m_cfString;

	cf.dwMask = CFM_COLOR;
	cf.crTextColor = cr;

	SetStringStyle(cf);
}

void CSyntaxColorizer::SetGroupStyle(int grp, CHARFORMAT cf)
{
	SKeyword* pTemp = m_pskKeyword;

	while(pTemp != NULL)
	{
		if(pTemp->group == grp)
		{
			pTemp->cf = cf;
		}
		pTemp = pTemp->pNext;
	}
}

void CSyntaxColorizer::GetGroupStyle(int grp, CHARFORMAT &cf)
{
	SKeyword* pTemp = m_pskKeyword;

	while(pTemp != NULL)
	{
		if(pTemp->group == grp)
		{
			cf = pTemp->cf;
			pTemp = NULL;
		}
		else
		{
			pTemp = pTemp->pNext;
			//if grp is not found, return default style
			if(pTemp == NULL) cf = m_cfDefault;
		}
	}
}

void CSyntaxColorizer::SetGroupColor(int grp, COLORREF cr)
{
	CHARFORMAT cf;
	GetGroupStyle(grp,cf);

	cf.dwMask = CFM_COLOR;
	cf.crTextColor = cr;

	SetGroupStyle(grp,cf);
}

void CSyntaxColorizer::Colorize(CHARRANGE cr, CRichEditCtrl *pCtrl)
{
	//Code optimization (old code deleted) by HOMO_PROGRAMMATIS <homo_programmatis@rt.mipt.ru>
	/**/	Colorize(cr.cpMin, cr.cpMax, pCtrl);
	//end of changes by HOMO_PROGRAMMATIS
}

void CSyntaxColorizer::Colorize(long nStartChar, long nEndChar, CRichEditCtrl *pCtrl)
{
	//No Selection-splashing upgrade by HOMO_PROGRAMMATIS <homo_programmatis@rt.mipt.ru>
	/**/	CHARRANGE l_OldSelectionRange;
	/**/	pCtrl->GetSel(l_OldSelectionRange);
	/**/	SendMessage(pCtrl->m_hWnd, EM_HIDESELECTION, 1, 0);
	//end of changes by HOMO_PROGRAMMATIS


	long nTextLength = 0;

	if(nStartChar == 0 && nEndChar == -1) //send entire text of rich edit box
	{
		nTextLength = pCtrl->GetTextLength();

		//if there is alot of text in the Rich Edit (>64K) then GetWindowText doesn't
		//work. We have to select all of the text, and then use GetSelText
		pCtrl->SetSel(0,-1);
	}
	else
	{
		//set up the text buffer; add 1 because zero-based array
		nTextLength = nEndChar - nStartChar + 1;

		pCtrl->SetSel(nStartChar,nEndChar);
	}
	
	//LPTSTR lpszBuf = new TCHAR[nTextLength+1];
	//pCtrl->GetSelText(lpszBuf);#OBSOLETE
	CString csBuf=pCtrl->GetSelText();
	pCtrl->SetSelectionCharFormat(m_cfDefault);

	//colorize(lpszBuf,pCtrl,nStartChar);
	colorize((LPTSTR)csBuf.GetBuffer(),pCtrl,nStartChar);

	//delete lpszBuf;

	//No Selection-splashing upgrade by HOMO_PROGRAMMATIS <homo_programmatis@rt.mipt.ru>
	/**/	pCtrl->SetSel(l_OldSelectionRange);
	/**/	SendMessage(pCtrl->m_hWnd, EM_HIDESELECTION, 0, 0);
	//end of changes by HOMO_PROGRAMMATIS
}

void CSyntaxColorizer::colorize(LPTSTR lpszBuf, CRichEditCtrl *pCtrl, long iOffset /*=0*/)
{
	//setup some vars
	CHARFORMAT cf;
	LPTSTR lpszTemp;
	long iStart;
	long x = 0;
	SKeyword* pskTemp = m_pskKeyword;
	unsigned char* pTable = m_pTableZero;

	//do the work
	while(lpszBuf[x])
	{
		//Bug fix: some 'char's are actually negative - caused 'read-below-array' before fix
		//fixed by HOMO_PROGRAMMATIS <homo_programmatis@rt.mipt.ru>
		//old code
		//		switch(pTable[lpszBuf[x]])
		//corrected variant
		/**/	switch(pTable[(_TUCHAR) lpszBuf[x]])
		//end of changes by HOMO_PROGRAMMATIS
		{
		case DQSTART:
			pTable = m_pTableOne;
			iStart = iOffset + x;
			break;
		case SQSTART:
			pTable = m_pTableTwo;
			iStart = iOffset + x;
			break;
		case CMSTART:
			if(lpszBuf[x+1] == '/')
			{
				pTable = m_pTableThree;
				iStart = iOffset + x;
				x++;
			}
			else if(lpszBuf[x+1] == '*')
			{
				pTable = m_pTableFour;
				iStart = iOffset + x;
				x++;
			}
			else if(lpszBuf[x] == '\'')
			{
				pTable = m_pTableThree;
				iStart = iOffset + x;
				x++;
			}

			break;
		case MLEND:
			if(lpszBuf[x+1] == '/')
			{
				x++;
				pTable = m_pTableZero;
				pCtrl->SetSel(iStart,iOffset + x+1);
				pCtrl->SetSelectionCharFormat(m_cfComment);
			}
			break;
		case SLEND:
			if(lpszBuf[x-2] != '\\') // line continuation character
			{
				pTable = m_pTableZero;
				pCtrl->SetSel(iStart,iOffset + x+1);
				pCtrl->SetSelectionCharFormat(m_cfComment);
			}
			break;
		case DQEND:
			pTable = m_pTableZero;
			pCtrl->SetSel(iStart,iOffset + x+1);
			pCtrl->SetSelectionCharFormat(m_cfString);
			break;
		case SQEND:
			if(lpszBuf[x-1] == '\\' && lpszBuf[x+1] == '\'')
				break;
			pTable = m_pTableZero;
			pCtrl->SetSel(iStart,iOffset + x+1);
			pCtrl->SetSelectionCharFormat(m_cfString);
			break;
		case KEYWORD:
			lpszTemp = lpszBuf+x;
			while(pskTemp != NULL)
			{
				if(pskTemp->keyword[0] == lpszTemp[0])
				{
					int x1=0,y1=0;iStart = iOffset + x;
					while(pskTemp->keyword[x1])
					{
						y1 += lpszTemp[x1] ^ pskTemp->keyword[x1];
						x1++;
					}
					if(y1 == 0 && (*(m_pAllowable + (lpszBuf[x-1])) && 
							*(m_pAllowable + (lpszBuf[x+pskTemp->keylen]))))
					{
						if(_stricmp(pskTemp->keyword,"rem") == 0)
						{
							pTable = m_pTableThree;
						}
						else 
						{
							x += pskTemp->keylen;
							pCtrl->SetSel(iStart,iOffset + x);
							pCtrl->SetSelectionCharFormat(pskTemp->cf);
						}
					}
				}
				pskTemp = pskTemp->pNext;
			}
			pskTemp = m_pskKeyword;
			break;
		case SKIP:;
		}
		x++;
	}
	//sometimes we get to the end of the file before the end of the string
	//or comment, so we deal with that situation here
	if(pTable == m_pTableOne)
	{
		cf = m_cfString;
		pCtrl->SetSel(iStart,iOffset + x+1);
		pCtrl->SetSelectionCharFormat(cf);
	}
	else if(pTable == m_pTableTwo)
	{
		cf = m_cfString;
		pCtrl->SetSel(iStart,iOffset + x+1);
		pCtrl->SetSelectionCharFormat(cf);
	}
	else if(pTable == m_pTableThree)
	{
		cf = m_cfComment;
		pCtrl->SetSel(iStart,iOffset + x+1);
		pCtrl->SetSelectionCharFormat(cf);
	}
	else if(pTable == m_pTableFour)
	{
		cf = m_cfComment;
		pCtrl->SetSel(iStart,iOffset + x+1);
		pCtrl->SetSelectionCharFormat(cf);
	}
}



