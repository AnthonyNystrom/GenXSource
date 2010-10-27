#include "CameraWM.h"
#include "Application.h"
#include "FileSystem.h"

#include <aygshell.h>
#include <windowsx.h>

#define MAX_FILE_NAME                         MAX_PATH

CameraWM::CameraWM() 
{




}

CameraWM::~CameraWM()
{
}



bool CameraWM::StopPreview()
{
	return true;
}



bool CameraWM::StartPreview()
{
	HRESULT         hReturn;
	SHCAMERACAPTURE shcc;




	// Specify the arguments of SHCAMERACAPTURE
	ZeroMemory(&shcc, sizeof(shcc));
	shcc.cbSize             = sizeof(shcc);
	shcc.hwndOwner          = NULL;
	shcc.pszInitialDir      = (LPCTSTR)fileDir;
	shcc.pszDefaultFileName = (LPCTSTR)fileName;
	shcc.pszTitle           = L"Take picture";
	shcc.StillQuality       = CAMERACAPTURE_STILLQUALITY_NORMAL;
	shcc.VideoTypes         = CAMERACAPTURE_VIDEOTYPE_STANDARD;
	shcc.nResolutionWidth   = snapshotWidth;
	shcc.nResolutionHeight  = snapshotHeight;
	shcc.nVideoTimeLimit    = 0;
	shcc.Mode               = CAMERACAPTURE_MODE_STILL;

	// Call SHCameraCapture() function
	//g_bCameraRunning = TRUE;
	hReturn = SHCameraCapture(&shcc);
	//g_bCameraRunning = FALSE;

	// Check the return codes of the SHCameraCapture() function
	switch (hReturn)
	{
	case S_OK:
		// The method completed successfully.
		if (pListener)
		{
			pListener->OnSnaphotCreated();
		}
		//CHR(StringCchPrintf(szMessage, ARRAYSIZE(szMessage), szFormat, shcc.szFile));
		break;
	case S_FALSE:
		// The user canceled the Camera Capture dialog box.
		if (pListener)
		{
			pListener->OnCameraCancelled();
		}
		break;
	case E_INVALIDARG:
		// An invalid argument was specified.
		if (pListener)
		{
			pListener->OnCameraFailed();
		}
		break;
	case E_OUTOFMEMORY:
		// There is not enough memory to save the image or video.
		if (pListener)
		{
			pListener->OnCameraFailed();
		}
		break;
	case HRESULT_FROM_WIN32(ERROR_RESOURCE_DISABLED):
		// The camera is disabled.
		if (pListener)
		{
			pListener->OnCameraFailed();
		}
		break;
	default:
		// An unknown error occurred.
		if (pListener)
		{
			pListener->OnCameraFailed();
		}
		break;
	}
	return true;
}


bool CameraWM::MakeSnapshot()
{




	return true;

}




bool CameraWM::SetPreviewPos( int32 x, int32 y )
{
	previewRect.x = x;
	previewRect.y = y;
	return true;
}

int32 CameraWM::SetPreviewWidth( uint32 width )
{
	previewRect.dx = width & (~0x01);
	previewRect.dy = ((previewRect.dx * 3) / 4) & (~0x01);
	return previewRect.dy;
}

int32 CameraWM::SetPreviewHeight( uint32 height )
{
	previewRect.dy = height & (~0x01);
	previewRect.dx = ((previewRect.dy * 4) / 3) & (~0x01);
	return previewRect.dx;
}

int32 CameraWM::SetSnapshotWidth( uint32 width )
{
	snapshotWidth = width & (~0x01);
	snapshotHeight = ((snapshotWidth * 3) / 4) & (~0x01);
	return snapshotHeight;
}

int32 CameraWM::SetSnapshotHeight( uint32 height )
{
	snapshotHeight = height & (~0x01);
	snapshotWidth = ((snapshotHeight * 4) / 3) & (~0x01);
	return snapshotWidth;
}

bool CameraWM::SetFileForSnapshot( char8* capturedName )
{
	const char *ptr = capturedName + Utils::StrLen(capturedName);
	while (ptr != capturedName)
	{
		if (*ptr == '\\')
		{
			ptr++;
			break;
		}
		ptr--;
	}

	if (ptr != capturedName)
	{
		MultiByteToWideChar(CP_ACP, MB_PRECOMPOSED, ptr, -1, (LPWSTR)fileName, MAX_FILENAME_SIZE);
		MultiByteToWideChar(CP_ACP, MB_PRECOMPOSED, capturedName, -1, (LPWSTR)fileDir, MAX_FILENAME_SIZE);
		fileDir[ptr - capturedName] = 0;
	}
	else
	{
		Utils::StrToWstr(capturedName, fileName, sizeof(fileName));
		GetModuleFileName(NULL, (LPWSTR)fileDir, MAX_FILE_PATH);
		int i = Utils::WStrLen(fileDir);
		do
		{
			if(fileDir[i-1] == L'\\')
			{
				fileDir[i] = 0;
				break;
			}
			i--;
		}
		while(i>=0);
	}
	return true;
}

