#pragma once

struct TRecentUploadItem
{
	TRecentUploadItem()
	{
		isFinished = true;
	}

	CString			fileTitle;
	CString			filePath;
	SYSTEMTIME		dateTime;
	bool			isFinished;
};

typedef CSimpleArray<TRecentUploadItem>		TRecentUploads;
