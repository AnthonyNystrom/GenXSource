namespace Next2Friends
{
	/**
	* Used in asynchronous requests
	*/
	enum TRequestType
	{
		EEncryptionKey,
		EMemberId,
		ETagId
	};

	/**
	* Callback used in asynchronous requests
	*/
	class MRequestObserver
	{
	public:
		virtual void HandleRequestCompletedL(const TInt& aError, const TRequestType& aType, const TDesC& aReturnValue) = 0;
	};

	class MMemberServices
	{
	public:
		// Synchronous methods
		/**
		* Returns NULL in case of failure
		*/
		virtual HBufC* GetEncryptionKey(const TDesC& aNickName, const TDesC& aWebPassword) = 0;

		/**
		* Returns NULL in case of failure
		*/
		virtual HBufC* GetMemberId(const TDesC& aNickName, const TDesC& aWebPassword) = 0;

		/**
		* Returns NULL in case of failure
		*/
		virtual HBufC* GetTagId(const TDesC& aNickName, const TDesC& aWebPassword) = 0;

		// Asynchronous methods
		/**
		* Returns KErrNone if succeeds
		*/
		virtual TInt GetEncryptionKey(const TDesC& aNickName, const TDesC& aWebPassword, const MRequestObserver& aObserver) = 0;

		/**
		* Returns KErrNone if succeeds
		*/
		virtual TInt GetMemberId(const TDesC& aNickName, const TDesC& aWebPassword, const MRequestObserver& aObserver) = 0;

		/**
		* Returns KErrNone if succeeds
		*/
		virtual TInt GetTagId(const TDesC& aNickName, const TDesC& aWebPassword, const MRequestObserver& aObserver) = 0;

		/**
		* Returns KErrNone if succeeds.
		* It is supposed that several DISTINCT asynchronous requests could be handled concurrently.
		*/
		virtual TInt CancelRequest(const TRequestType& aRequestType) = 0;
	};
}