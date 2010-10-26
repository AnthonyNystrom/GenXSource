#include "stdafx.h"
#include "ProteinVista.h"
#include "ProteinVistaView.h"
#include "ProteinVistaRenderer.h"

#include "ScreenShot.h"
#include "wmvFile.h"
#include "FileDialogExtSaveImage.h"

#include "SaveMovieDialog.h"
#include "Interface.h"

using namespace Gdiplus;
using namespace std;

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


void CProteinVistaView::OnFileScreenshot()
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	CWnd* ownerWnd = CWnd::FromHandle(GetMainApp()->m_CanvsHandle);
	static char szFilter[] = "PNG File (*.png)|*.png|BMP File (*.bmp)|*.bmp|JPG File (*.jpg)|*.jpg|DIB File (*.dib)|*.dib|All Files (*.*)|*.*||";
	CFileDialogExtSaveImage	saveDialog(FALSE, "png", NULL, OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT, szFilter ,ownerWnd);

	saveDialog.m_imageWidth = saveDialog.m_imageWidthDefault = m_pProteinVistaRenderer->m_d3dsdBackBuffer.Width;
	saveDialog.m_imageHeight = saveDialog.m_imageHeightDefault = m_pProteinVistaRenderer->m_d3dsdBackBuffer.Height;

	if ( saveDialog.DoModal() == IDOK )
	{
		D3DXIMAGE_FILEFORMAT format [] = { D3DXIFF_PNG, D3DXIFF_BMP, D3DXIFF_JPG, D3DXIFF_DIB };

		//AfxGetApp()->DoWaitCursor(1);		// 1->>display the hourglass cursor

		m_pProteinVistaRenderer->SaveScreenImage(saveDialog.GetPathName(), saveDialog.m_imageWidth, saveDialog.m_imageHeight, format[saveDialog.m_comboBoxImageFormat] );

		//AfxGetApp()->DoWaitCursor(-1);		// restore
	}
}
 
HRESULT CProteinVistaRenderer::SaveScreenImage ( LPCSTR fileName, long imageWidth, long imageHeight , D3DXIMAGE_FILEFORMAT format)
{
	HRESULT hr;
	LPDIRECT3DDEVICE9 pDev = GetD3DDevice();

	BOOL bChangeSize = TRUE;
	if ( imageWidth == -1 || imageHeight == -1 )
	{
		bChangeSize = FALSE;
		imageWidth = m_d3dpp.BackBufferWidth;
		imageHeight = m_d3dpp.BackBufferHeight;
	}

	//    홀수이면 짝수로 바꾼다.
	if ( imageWidth/2 != (imageWidth+1)/2 )
	{
		imageWidth = (imageWidth/2)*2;
		bChangeSize = TRUE;
	}
	if ( imageHeight/2 != (imageHeight+1)/2 )
	{
		imageHeight = (imageHeight/2)*2;
		bChangeSize = TRUE;
	}

	long imageWidthOld = m_d3dpp.BackBufferWidth;
	long imageHeightOld = m_d3dpp.BackBufferHeight;

	if ( bChangeSize == TRUE )
	{
		m_d3dpp.BackBufferWidth  = imageWidth;
		m_d3dpp.BackBufferHeight = imageHeight;

		m_d3dSettings.Windowed_Width = m_d3dpp.BackBufferWidth;
		m_d3dSettings.Windowed_Height = m_d3dpp.BackBufferHeight;

		if( FAILED( hr = Reset3DEnvironment() ) )
		{
			if( hr == D3DERR_DEVICELOST )
			{
				m_bDeviceLost = true;
			}
			else
			{
				return E_FAIL;
			}
		}
	}

	LPDIRECT3DSURFACE9 renderTargetOrig = NULL;	
	hr = pDev->GetRenderTarget(0,&renderTargetOrig);

	LPDIRECT3DSURFACE9 renderTargetCapture; // 캡쳐된 내용이 들어갈 서피스
	D3DSURFACE_DESC desc;
	renderTargetOrig->GetDesc(&desc);

	pDev->CreateRenderTarget(desc.Width, desc.Height, desc.Format, desc.MultiSampleType , desc.MultiSampleQuality, FALSE , &renderTargetCapture, NULL);

	pDev->SetRenderTarget(0, renderTargetCapture);
	pDev->Clear(0, NULL, D3DCLEAR_TARGET, 0, 1, 0);

	//    
	Render();

	pDev->SetRenderTarget(0, renderTargetOrig);
	SAFE_RELEASE(renderTargetOrig);

	// save the image to specified file
	CString filename(fileName);
	hr=D3DXSaveSurfaceToFile(filename,format,renderTargetCapture,NULL,NULL);
	if ( FAILED(hr) )
		return hr;

	SAFE_RELEASE(renderTargetCapture);

	//
	// return status of save to caller
	// 
	if ( bChangeSize == TRUE )
	{
		m_d3dpp.BackBufferWidth  = imageWidthOld;
		m_d3dpp.BackBufferHeight = imageHeightOld;

		m_d3dSettings.Windowed_Width = m_d3dpp.BackBufferWidth;
		m_d3dSettings.Windowed_Height = m_d3dpp.BackBufferHeight;

		if( FAILED( hr = Reset3DEnvironment() ) )
		{
			if( hr == D3DERR_DEVICELOST )
			{
				m_bDeviceLost = true;
			}
			else
			{
				return E_FAIL;
			}
		}

		Render3DEnvironment();
	}

	return hr;
}

//==========================================================================================================
//==========================================================================================================

void CProteinVistaApp::OnMakeMovie()
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	HRESULT hr;
	CSaveMovieDialog saveMovieDialog;

	saveMovieDialog.m_imageWidth = saveMovieDialog.m_imageWidthDefault = 1280;
	saveMovieDialog.m_imageHeight = saveMovieDialog.m_imageHeightDefault = 800;

	if ( saveMovieDialog.DoModal() == IDOK )
	{
		CStringArray & strArrayFilename = saveMovieDialog.m_strArrayFilename;
		if ( strArrayFilename.GetSize() == 0 )
		{
			AfxMessageBox( _T("Select image files") );
			return;
		}

		CSTLIntArray	arrayFrame;

		//    check all bitmap file
		int imageWidthBase;
		int imageHeightBase;
		for ( int i = 0 ; i < strArrayFilename.GetSize() ; i++ )
		{
			USES_CONVERSION;
			Bitmap * bitmap = Bitmap::FromFile(A2W(strArrayFilename[i]));
			if ( bitmap == NULL )
			{
				CString strText;
				strText.Format( _T("Cannot find file: %s") , strArrayFilename[i] );
				AfxMessageBox(strText);
				return;
			}

			int width = bitmap->GetWidth();
			int height = bitmap->GetHeight();
			delete bitmap;

			//    프레임은 전부 1로 설정
			arrayFrame.push_back(1);

			if ( i == 0 )
			{
				imageWidthBase = width;
				imageHeightBase = height;

				if ( width/2 != (width+1)/2 || height/2 != (height+1)/2 )
				{
					CString strText;
					strText.Format( _T("The width and height of Images has to have even size. %s has odd size") , strArrayFilename[i] );
					AfxMessageBox(strText);
					return;
				}
			}
			else
			{
				if ( imageWidthBase != width || imageHeightBase != height )
				{
					CString strText;
					strText.Format( _T("All Images have to have same size. %s is different size") , strArrayFilename[i] );
					AfxMessageBox(strText);
					return;
				}
			}
		}

		long imageWidth = saveMovieDialog.m_imageWidth;
		long imageHeight = saveMovieDialog.m_imageHeight;

		if ( saveMovieDialog.m_checkCurrentImageSize == TRUE )
		{
			USES_CONVERSION;
			Bitmap * bitmap = Bitmap::FromFile(A2W(strArrayFilename[0]));
			imageWidth = bitmap->GetWidth();
			imageHeight = bitmap->GetHeight();
			delete bitmap;
		}

		AfxGetApp()->DoWaitCursor(1);		// 1->>display the hourglass cursor
		hr = MakeMovieWithImages(saveMovieDialog.m_strMovieFilename, strArrayFilename, arrayFrame, imageWidth, imageHeight, saveMovieDialog.m_fps );
		AfxGetApp()->DoWaitCursor(-1);		// restore

		if ( FAILED(hr) )
		{


		}
	}
}

HRESULT CProteinVistaApp::MakeMovieWithImages(CString movieFilename, CStringArray & strArrayFilename, CSTLIntArray & arrayFrame, 
										int width, int height, int fps )
{
	HRESULT hr;
	USES_CONVERSION;

	if ( strArrayFilename.GetSize() == 0 )
		return E_FAIL;

	int	bitmapWidth = width;
	int	bitmapHeight = height;

	if ( bitmapWidth == -1 || bitmapHeight == -1 )
	{
		Bitmap * bitmap = Bitmap::FromFile(A2W(strArrayFilename[0]));
		if ( bitmap == NULL )
		{
			return E_FAIL;
		}		

		bitmapWidth = bitmap->GetWidth();
		bitmapHeight = bitmap->GetHeight();

		delete bitmap;
	}

	IWMProfile *pProfile=NULL;
	{
		IWMProfileManager *pProfileManager=NULL;

		if(FAILED(WMCreateProfileManager(&pProfileManager)))
		{
			AfxMessageBox(_T("Unable to Create WindowsMedia Profile Manager"));
			return E_FAIL;
		}

		CString profileFilename;
		profileFilename.Format("%sHD_Recording.xml" , ((CProteinVistaApp *)AfxGetApp())->m_strBaseResPath );

		// Reading buffer
		CFile* pFile = new CFile(profileFilename, CFile::modeRead );
		if ( pFile->m_hFile != CFile::hFileNull )
		{
			long fileLen = pFile->GetLength()*sizeof(wchar_t) * 2;
			wchar_t * buffer = new wchar_t [fileLen];
			wchar_t * bufferProfile = new wchar_t [fileLen];
			ZeroMemory(buffer, fileLen);
			ZeroMemory(bufferProfile, fileLen);

			long nRead = pFile->Read( buffer, fileLen );
			pFile->Close();

			swprintf(bufferProfile, buffer, bitmapWidth, bitmapHeight, 
											bitmapWidth, bitmapHeight, 
											bitmapWidth, bitmapHeight );

			if(FAILED(pProfileManager->LoadProfileByData(bufferProfile, &pProfile)))
			{
				AfxMessageBox(_T("Unable to Load Data Profile"));
				return E_FAIL;
			}

			delete [] buffer;
			delete [] bufferProfile;

			SAFE_RELEASE(pProfileManager);
			SAFE_DELETE(pFile);
		}
		else
		{
			SAFE_RELEASE(pProfileManager);
			AfxMessageBox(_T("Cannot find HD_Recording.xml"));
			return E_FAIL;
		}
	}

	//
	//	movieFilename 을 지운다.
	TCHAR drive[MAX_PATH];
	TCHAR dir[MAX_PATH];
	TCHAR fname[MAX_PATH];
	TCHAR ext[MAX_PATH];
	_splitpath(movieFilename, drive, dir, fname, ext );

	CString strMovieFilename;
	if ( drive[0] != NULL && dir[0] != NULL )
	{
		strMovieFilename = CString(movieFilename);
	}
	else
	{
		strMovieFilename.Format(_T("%s%s%s") , ((CProteinVistaApp *)AfxGetApp())->m_strBaseScriptMovie, fname, ext );
	}

	DeleteFile(strMovieFilename);

	//
	CwmvFile	wmvFile (strMovieFilename, pProfile, fps);
	if ( wmvFile.GetLastErrorMessage() != CString (_T("Method Succeeded") ) )
	{
		CString strText;
		strText.Format( "%s: Cannot open %s", wmvFile.GetLastErrorMessage(), movieFilename );
		AfxMessageBox ( strText );
		
		return E_FAIL;
	}

	for ( int i = 0 ; i < strArrayFilename.GetSize(); i++ )
	{
		USES_CONVERSION;
		Bitmap * bitmap = Bitmap::FromFile(A2W(strArrayFilename[i]));

		Color colorBackground;
		HBITMAP hBitmap;
		bitmap->GetHBITMAP(colorBackground,&hBitmap);

		long frames = arrayFrame[i];
		for ( int j = 0 ; j < frames ; j++ )
		{
			hr = wmvFile.AppendNewFrame(hBitmap);
		}
		if ( FAILED(hr) )
		{
			CString strText;
			strText.Format( "Movie Making Error: %s, %s", wmvFile.GetLastErrorMessage(), strArrayFilename[i] );
			AfxMessageBox ( strText );
			
			delete bitmap;

			return E_FAIL;
		}
		delete bitmap;
	}

	return S_OK;
}


