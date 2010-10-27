#include "CameraSis9.h"
#include "Application.h"
#include "FileSystem.h"




CameraSis::CameraSis() 
{
}

CameraSis::~CameraSis()
{
}



bool CameraSis::StopPreview()
{

	return true;
}



bool CameraSis::StartPreview()
{

	return true;
}


bool CameraSis::MakeSnapshot()
{



	return true;

}


bool CameraSis::SetPreviewPos( int32 x, int32 y )
{
	previewRect.x = x;
	previewRect.y = y;
	return true;
}

int32 CameraSis::SetPreviewWidth( uint32 width )
{
	previewRect.dx = width & (~0x01);
	previewRect.dy = ((previewRect.dx * 3) / 4) & (~0x01);
	return previewRect.dy;
}

int32 CameraSis::SetPreviewHeight( uint32 height )
{
	previewRect.dy = height & (~0x01);
	previewRect.dx = ((previewRect.dy * 4) / 3) & (~0x01);
	return previewRect.dx;
}

int32 CameraSis::SetSnapshotWidth( uint32 width )
{
	snapshotWidth = width & (~0x01);
	snapshotHeight = ((snapshotWidth * 3) / 4) & (~0x01);
	return snapshotHeight;
}

int32 CameraSis::SetSnapshotHeight( uint32 height )
{
	snapshotHeight = height & (~0x01);
	snapshotWidth = ((snapshotHeight * 4) / 3) & (~0x01);
	return snapshotWidth;
}

bool CameraSis::SetFileForSnapshot( char8* capturedName )
{
	Utils::StrCpy(fileName, capturedName);
	return true;
}

