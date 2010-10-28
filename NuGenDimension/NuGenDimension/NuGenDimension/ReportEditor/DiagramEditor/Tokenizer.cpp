/* ==========================================================================
	Class :			CTokenizer

	Date :			06/18/04

	Purpose :		"CTokenizer" is a very simple class to tokenize a string 
					into a string array.	

	Description :	The string is chopped up and put into an internal 
					"CStringArray". With "GetAt", different types of data can 
					be read from the different elements.

	Usage :			Create an instance of "CTokenizer" on the stack. Set 
					the string to parse in the "ctor", and the delimiter if 
					not a comma. The "ctor" automatically tokenize the input
					string.

					Call "GetSize" to get the number of 
					tokens, and "GetAt" to get a specific token.
	
					You can also reuse the tokenizer by calling "Init".
   ========================================================================
	Changes :		28/8  2004	Changed a char to TCHAR to allow UNICODE 
								building (Enrico Detoma)
   ========================================================================*/

#include "stdafx.h"
#include "Tokenizer.h"


////////////////////////////////////////////////////////////////////
// Public functions
//
CTokenizer::CTokenizer( CString strInput, const CString & strDelimiter )
/* ============================================================
	Function :		CTokenizer::CTokenizer
	Description :	Constructor.
	Access :		Public
					
	Return :		void
	Parameters :	CString strInput				-	String to 
														tokenize
					const CString & strDelimiter	-	Delimiter, 
														defaults to
														comma.
	Usage :			Should normally be created on the stack.

   ============================================================*/
{

	Init(strInput, strDelimiter);

}

void CTokenizer::Init( const CString & strInput, const CString & strDelimiter )
/* ============================================================
	Function :		CTokenizer::Init
	Description :	Reinitializes and tokenizes the tokenizer 
					with "strInput". "strDelimiter" is the 
					delimiter to use.
	Access :		Public
					
	Return :		void
	Parameters :	const CString & strInput		-	New string
					const CString & strDelimiter	-	Delimiter,
														defaults to 
														comma

	Usage :			Call to reinitialize the tokenizer.

   ============================================================*/
{

	CString copy( strInput );
	m_stra.RemoveAll();
	int nFound = copy.Find( strDelimiter );

	while(nFound != -1)
	{
		CString strLeft;
		strLeft = copy.Left( nFound );
		copy = copy.Right( copy.GetLength() - ( nFound + 1 ) );

		m_stra.Add( strLeft );
		nFound = copy.Find( strDelimiter );
	}

	m_stra.Add( copy );

}

int CTokenizer::GetSize(  ) const
/* ============================================================
	Function :		CTokenizer::GetSize
	Description :	Gets the number of tokens in the tokenizer.
	Access :		Public
					
	Return :		int	-	Number of tokens.
	Parameters :	none

	Usage :			Call to get the number of tokens in the 
					tokenizer.

   ============================================================*/
{

	return m_stra.GetSize();

}

void CTokenizer::GetAt( int nIndex, CString & str ) const
/* ============================================================
	Function :		CTokenizer::GetAt
	Description :	Get the token at "nIndex" and put it in 
					"str".
	Access :		Public
					
	Return :		void
	Parameters :	int nIndex		- Index to get token from
					CString & str	- Result

	Usage :			Call to get the value of the token at 
					"index".

   ============================================================*/
{

		if( nIndex < m_stra.GetSize() )
			str = m_stra.GetAt( nIndex );
		else
			str = _T( "" );

}

void CTokenizer::GetAt( int nIndex, int & var ) const
/* ============================================================
	Function :		CTokenizer::GetAt
	Description :	Get the token at "nIndex" and put it in 
					"var".
	Access :		Public
					
	Return :		void
	Parameters :	int nIndex	- Index to get token from
					int & var	- Result

	Usage :			Call to get the value of the token at 
					"index".

   ============================================================*/
{

		if( nIndex < m_stra.GetSize() )
			var = _ttoi( m_stra.GetAt( nIndex ) );
		else
			var = 0;

}

void CTokenizer::GetAt( int nIndex, WORD & var ) const
/* ============================================================
	Function :		CTokenizer::GetAt
	Description :	Get the token at "nIndex" and put it in 
					"var".
	Access :		Public
					
	Return :		void
	Parameters :	int nIndex	- Index to get token from
					WORD & var	- Result

	Usage :			Call to get the value of the token at 
					"index".

   ============================================================*/
{

		if( nIndex < m_stra.GetSize() )
			var = static_cast< WORD >( _ttoi( m_stra.GetAt( nIndex ) ) );
		else
			var = 0;

}

void CTokenizer::GetAt( int nIndex, double & var ) const
/* ============================================================
	Function :		CTokenizer::GetAt
	Description :	Get the token at "nIndex" and put it in 
					"var".
	Access :		Public
					
	Return :		void
	Parameters :	int nIndex		- Index to get token from
					double & var	- Result

	Usage :			Call to get the value of the token at 
					"index".

   ============================================================*/
{

		TCHAR   *stop;
		if( nIndex < m_stra.GetSize() )
			var = _tcstod( m_stra.GetAt( nIndex ), &stop );
		else
			var = 0.0;

}

void CTokenizer::GetAt( int nIndex, DWORD & var ) const
/* ============================================================
	Function :		CTokenizer::GetAt
	Description :	Get the token at "nIndex" and put it in 
					"var".
	Access :		Public
					
	Return :		void
	Parameters :	int nIndex	- Index to get token from
					DWORD & var	- Result

	Usage :			Call to get the value of the token at 
					"index".

   ============================================================*/
{

		if( nIndex < m_stra.GetSize() )
			var = static_cast< DWORD >( _ttoi( m_stra.GetAt( nIndex ) ) );
		else
			var = 0;

}
