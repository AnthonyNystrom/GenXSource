namespace Next2Friends
{
	/**
	* Callback used in asynchronous requests
	*/
	class MRequestObserver
	{
	public:
		virtual void HandleRequestCompletedL(const TInt& aError) = 0;
	};

	class MPhotoOrganise
	{
	public:
		// Synchronous methods
		/**
		* Returns KErrNone if succeeds
		*/
		virtual TInt DeviceUploadPhoto(const TDesC& aNickName, const TDesC& aWebPassword,
			const TFileName& aFilePath, const TTime& aTime) = 0;

		// Asynchronous methods
		/**
		* Returns KErrNone if succeeds
		*/
		virtual TInt DeviceUploadPhoto(const TDesC& aNickName, const TDesC& aWebPassword,
			const TFileName& aFilePath, const TTime& aTime, const MRequestObserver& aObserver) = 0;

		/**
		* Returns KErrNone if succeeds
		*/
		virtual TInt CancelRequest() = 0;
	};
}