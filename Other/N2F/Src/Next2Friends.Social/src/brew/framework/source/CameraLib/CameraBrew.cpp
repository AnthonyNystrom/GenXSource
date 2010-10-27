#include "CameraBrew.h"
#include "Application.h"
#include "FileSystem.h"

#include "AEEAppGen.h"
#include "AEEStdLib.h"
#include "AEECamera.h"



CameraBrew::CameraBrew() 
	:	pICamera	(NULL)
	,	counter		(0)
{

	pApp = (AEEApplet*)GETAPPINSTANCE();

	UTILS_LOG(EDMP_NOTICE, "pApp(0x%p): clsID(%d), m_pIShell(0x%p)", pApp, pApp->clsID, pApp->m_pIShell);

	state = ECS_FAILED;

	int32 res;

#ifdef WIN32
	if(res = 0)
#else
	if ( SUCCESS == (res = ISHELL_CreateInstance(pApp->m_pIShell, AEECLSID_CAMERA, (void **)&pICamera)) )
#endif
	{
		UTILS_LOG(EDMP_DEBUG,"CAMERA: create instance done");

		{//get size list
			AEESize **ppList;
			boolean * pbRange;

			int32 res;
			if ( SUCCESS == (res = ICAMERA_GetDisplaySizeList (pICamera, ppList, pbRange)) )
			{
#ifdef LOGS_ON
				while (*ppList)
				{
					UTILS_LOG(EDMP_DEBUG, "ICAMERA_GetDisplaySizeList: list size: %dx%d", (*ppList)->cx, (*ppList)->cy);
					++ppList;
				}
				UTILS_LOG(EDMP_DEBUG, "ICAMERA_GetDisplaySizeList: is paired list: %d", (*pbRange));
#endif
			}
			else
			{
				UTILS_LOG(EDMP_DEBUG, "CAMERA: ICAMERA_GetDisplaySizeList() failed: %d", res);
			}
		}
	}
	else
	{
		if (pListener)
		{
			pListener->OnCameraFailed();
		}
		state = ECS_FAILED;
		UTILS_LOG(EDMP_DEBUG, "CAMERA: create instance failed");
#ifdef LOGS_ON

		switch (res)
		{
		case ENOMEMORY:
			{
				UTILS_LOG(EDMP_DEBUG, "CAMERA ERROR: ENOMEMORY");
			}break;

		case ECLASSNOTSUPPORT:
			{
				UTILS_LOG(EDMP_DEBUG, "CAMERA ERROR: ECLASSNOTSUPPORT");
			}break;

		case EBADPARM:
			{
				UTILS_LOG(EDMP_DEBUG, "CAMERA ERROR: EBADPARM");
			}break;

		default:
			{
				UTILS_LOG(EDMP_DEBUG, "CAMERA ERROR: code %d", res);
			}break;
		}
#endif

		return;
	}

	state = ECS_READY;
}

CameraBrew::~CameraBrew()
{
	if(pICamera)
	{
		StopPreview();
		ICAMERA_Release(pICamera);
		pICamera = NULL;
	}
}



bool CameraBrew::StopPreview()
{
	UTILS_LOG(EDMP_DEBUG, "Camera::StopPreview:  pCamera = %p", pICamera);
	if (ECS_PREVIEW == state)
	{
		if(pICamera)
		{
			UTILS_LOG(EDMP_DEBUG, "Camera::StopPreview:  Stopping camera");
			ICAMERA_RegisterNotify(pICamera, NULL, NULL);
			ICAMERA_Stop(pICamera);
			state = ECS_READY;
			return true;
		}
	}
	return false;
}



bool CameraBrew::StartPreview()
{
	counter = 0;

	UTILS_LOG(EDMP_DEBUG, "CAMERA:: StartPreview");
	if (NULL == pICamera)//for emulator
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA:: StartPreview:  CAMERA == NULL");
		state = ECS_FAILED;
		return false;
	}

	if ( (ECS_READY != state) && (ECS_SNAPSHOT_DONE != state) )
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA:: StartPreview:  (ECS_READY != state) && (ECS_SNAPSHOT_DONE != state)");
		return false;
	}

	if (SUCCESS == ICAMERA_RegisterNotify(pICamera, cbPreviewNotify, this))
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA: register preview notify done");
	}
	else
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA: register preview notify failed");
		state = ECS_FAILED;
		if (pListener)
		{
			pListener->OnCameraFailed();
		}
		return false;
	}

	AEESize sz;
	sz.cx = previewRect.dx;
	sz.cy = previewRect.dy;

	UTILS_LOG(EDMP_DEBUG, "CAMERA: size width = %d,  height = %d" , sz.cx, sz.cy);
	//fix device bug: restores camera size after snapshot mode
	int32 res;
	if ( SUCCESS == (res = ICAMERA_SetSize(pICamera, &sz)) )
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA: set size done");
	}
	else
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA: set size failed. ERROR code: %d", res);
	}

	if ( SUCCESS == (res = ICAMERA_SetDisplaySize(pICamera, &sz)) )
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA: set display size done");
	}
	else
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA: set display size failed. ERROR code: %d", res);
		state = ECS_FAILED;
		if (pListener)
		{
			pListener->OnCameraFailed();
		}
		return false;
	}
	
	if (SUCCESS == ICAMERA_Preview(pICamera))
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA: start preview done");
	}
	else
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA: preview failed");
		state = ECS_FAILED;
		if (pListener)
		{
			pListener->OnCameraFailed();
		}
		return false;
	}

	state = ECS_PREVIEW;

	return true;
}


bool CameraBrew::MakeSnapshot()
{


	UTILS_LOG(EDMP_DEBUG, "CAMERA::MakeSnapshot: pICamera = %p  state = %d", pICamera, state);
	if(NULL == pICamera || !*fileName)
	{
		return false;
	}




	if (ECS_PREVIEW == state)
	{
		state = ECS_SNAPSHOT;
		if (SUCCESS == ICAMERA_Stop(pICamera))
		{
			UTILS_LOG(EDMP_DEBUG, "CAMERA: stop camera done");
			return true;
		}
		else
		{
			state = ECS_SNAPSHOT_FAILED;
			UTILS_LOG(EDMP_DEBUG, "CAMERA: stop camera failed");
			return false;
		}
	}

	return false;

}

bool CameraBrew::Snapshot()
{
	UTILS_LOG(EDMP_DEBUG, "CAMERA: SNAPSHOT");

	if (SUCCESS == ICAMERA_RegisterNotify(pICamera, cbSnapshotNotify, this))
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA: register snapshot notify done");
	}
	else
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA: register snapshot notify failed");
		if (pListener)
		{
			pListener->OnCameraFailed();
		}
		return false;
	}



	AEEMediaData md;
	md.clsData = MMD_FILE_NAME;
	md.pData = (void*)fileName;
	md.dwSize = 0;

	if (SUCCESS == ICAMERA_SetMediaData(pICamera, &md, AEECLSID_WINBMP))
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA: set media data done");
	}
	else
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA: set media data failed");
		if (pListener)
		{
			pListener->OnCameraFailed();
		}
		return false;
	}

	if (SUCCESS == ICAMERA_SetVideoEncode(pICamera, AEECLSID_WINBMP, 0))
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA: set video encode done");
	}
	else
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA: set video encode failed");
		if (pListener)
		{
			pListener->OnCameraFailed();
		}
		return false;
	}

	AEESize sz;

	sz.cx = snapshotWidth;
	sz.cy = snapshotHeight;
	UTILS_LOG(EDMP_DEBUG, "Size::  cx = %d,  cy = %d", sz.cx, sz.cy);
	if (SUCCESS == ICAMERA_SetSize(pICamera, &sz))
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA: set size done");
	}
	else
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA: set size failed");
		if (pListener)
		{
			pListener->OnCameraFailed();
		}
		return false;
	}

	if (SUCCESS == ICAMERA_SetDisplaySize(pICamera, &sz))
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA: set display size done");
	}
	else
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA: set display size failed");
		return false;
	}

	if (SUCCESS == ICAMERA_RecordSnapshot(pICamera))
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA: record snapshot start");
	}
	else
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA: record snapshot failed");
		return false;
	}
	return true;

}

void CameraBrew::cbPreviewNotify(void *pUser, AEECameraNotify *pn)
{
	CameraBrew *pCamera = (CameraBrew*)pUser;
	if ((NULL == pCamera) || (NULL == pn))
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA:: NULL: preview: counter = %d", pCamera->counter);
		return;
	}
	UTILS_LOG(EDMP_DEBUG, "CAMERA:: cbPreviewNotify counter = %d", pCamera->counter);

	switch (pn->nStatus)
	{

	case CAM_STATUS_START:
		// Preview has begun...
		UTILS_LOG(EDMP_DEBUG, "CAMERA:: cbPreviewNotify: CAM_STATUS_START");
		if (pCamera->pListener)
		{
			pCamera->pListener->OnPreviewStarted();
		}
		
		break;

	case CAM_STATUS_FRAME:
		{
			pCamera->counter++;
			UTILS_LOG(EDMP_DEBUG, "CAMERA:: preview: counter = %d", pCamera->counter);

//#ifndef LG9800_CAMERA 	//sim
			UTILS_LOG(EDMP_DEBUG, "CAMERA: DrawFrame!?");

			IBitmap *pFrame; 
			if (SUCCESS != ICAMERA_GetFrame(pCamera->pICamera, &pFrame))
			{
				UTILS_LOG(EDMP_DEBUG, "CAMERA:: cbPreview: Failed GetFrame");
				return;
			}
			else
			{
				UTILS_LOG(EDMP_DEBUG, "CAMERA:: cbPreview: GetFrame SUCCESS !!");
			}

 			AEEBitmapInfo bi;
 			IBITMAP_GetInfo(pFrame, &bi, sizeof(bi));
			UTILS_LOG(EDMP_DEBUG, "SIZE: x = %d, y = %d, w = %d, h = %d", pCamera->previewRect.x, pCamera->previewRect.y, bi.cx, bi.cy);
			if (bi.cx == pCamera->previewRect.dy && bi.cy == pCamera->previewRect.dx)
			{
				UTILS_LOG(EDMP_DEBUG, "CAMERA:: revert rect size");
				bi.cx = pCamera->previewRect.dx;
				bi.cy = pCamera->previewRect.dy;
			}

 			IDISPLAY_BitBlt(pCamera->pApp->m_pIDisplay,
 				pCamera->previewRect.x, pCamera->previewRect.y, bi.cx, bi.cy, pFrame, 0, 0, AEE_RO_COPY);

			IBITMAP_Release( pFrame );
//#endif			
			break;
		}

	case CAM_STATUS_DONE:
		// ICAMERA_Stop() was called and preview operation was stopped.
		if (CAM_CMD_START == pn->nCmd)
		{
			UTILS_LOG(EDMP_DEBUG, "CAMERA: preview stop done");

			if (ECS_SNAPSHOT == pCamera->state)
			{
				if (!pCamera->Snapshot())
				{
					UTILS_LOG(EDMP_DEBUG, "CAMERA: MakeSnapshot Failed");
					pCamera->state = ECS_SNAPSHOT_FAILED;
				}
			}
		}
		else
		{
			UTILS_LOG(EDMP_DEBUG, "CAMERA: preview stop failed");
		}
		break;

	case CAM_STATUS_ABORT:
		// Preview was aborted.
		UTILS_LOG(EDMP_DEBUG, "CAMERA: preview CAM_STATUS_ABORT");
		if (pCamera->pListener)
		{
			pCamera->pListener->OnPreviewStopped();
		}
		break;
	}
}


void CameraBrew::cbSnapshotNotify(void *pUser, AEECameraNotify *pn)
{
	CameraBrew *pCamera = (CameraBrew*)pUser;
	if ((NULL == pCamera) || (NULL == pn))
	{
		UTILS_LOG(EDMP_DEBUG, "CAMERA::cbSnapshotNotify : camera  = NULL");
		return;
	}
	UTILS_LOG(EDMP_DEBUG, "CAMERA::cbSnapshotNotify:  status = %d cmd = %d  subcmd = %d", pn->nStatus, pn->nCmd, pn->nSubCmd);

	switch (pn->nStatus)
	{
	case CAM_STATUS_START:
		UTILS_LOG(EDMP_DEBUG, "CAMERA::cbSnapshotNotify : CAM_STATUS_START");
		// Snapshot has begun...
		break;

	case CAM_STATUS_DONE:
		UTILS_LOG(EDMP_DEBUG, "CAMERA::cbSnapshotNotify : CAM_STATUS_DONE");
		if (CAM_CMD_ENCODESNAPSHOT == pn->nCmd)
		{
			UTILS_LOG(EDMP_DEBUG, "CAMERA: encode done");

			if (pCamera->pListener)
			{
				pCamera->pListener->OnSnaphotCreated();
			}


			pCamera->StopPreview();
			pCamera->state = ECS_SNAPSHOT_DONE;
		}
		break;

	case CAM_STATUS_FAIL:
		UTILS_LOG(EDMP_DEBUG, "CAMERA::cbSnapshotNotify : CAM_STATUS_FAIL");
		if (CAM_CMD_ENCODESNAPSHOT == pn->nCmd)
		{
			UTILS_LOG(EDMP_DEBUG, "CAMERA: encode failed");
			if (pCamera->pListener)
			{
				pCamera->pListener->OnCameraFailed();
			}
			pCamera->StopPreview();
			pCamera->state = ECS_FAILED;
			
		}
		break;

	case CAM_STATUS_ABORT:
		UTILS_LOG(EDMP_DEBUG, "CAMERA::cbSnapshotNotify : CAM_STATUS_ABORT");
		UTILS_LOG(EDMP_DEBUG, "CAMERA: abort snapshot Cmd: %d, subCmd: %d", pn->nCmd, pn->nSubCmd);
		if (pCamera->pListener)
		{
			pCamera->pListener->OnCameraFailed();
		}

		pCamera->StopPreview();
		pCamera->state = ECS_FAILED;
		break;
	}

	UTILS_LOG(EDMP_DEBUG, "CAMERA::cbSnapshotNotify:  --------------------------end!!   pCamera->state = %d", pCamera->state);
}


bool CameraBrew::SetPreviewPos( int32 x, int32 y )
{
	previewRect.x = x;
	previewRect.y = y;
	return true;
}

int32 CameraBrew::SetPreviewWidth( uint32 width )
{
	previewRect.dx = width & (~0x01);
	previewRect.dy = ((previewRect.dx * 3) / 4) & (~0x01);
	return previewRect.dy;
}

int32 CameraBrew::SetPreviewHeight( uint32 height )
{
	previewRect.dy = height & (~0x01);
	previewRect.dx = ((previewRect.dy * 4) / 3) & (~0x01);
	return previewRect.dx;
}

int32 CameraBrew::SetSnapshotWidth( uint32 width )
{
	snapshotWidth = width & (~0x01);
	snapshotHeight = ((snapshotWidth * 3) / 4) & (~0x01);
	return snapshotHeight;
}

int32 CameraBrew::SetSnapshotHeight( uint32 height )
{
	snapshotHeight = height & (~0x01);
	snapshotWidth = ((snapshotHeight * 4) / 3) & (~0x01);
	return snapshotWidth;
}

bool CameraBrew::SetFileForSnapshot( char8* capturedName )
{
	Utils::StrCpy(fileName, capturedName);
	return true;
}

