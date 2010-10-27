namespace Next2Friends
{
	/**
	* Used in asynchronous requests.
	*/
	enum TRequestType
	{
		EPhotoUploading,
		EQuestionCompleting,
		ECommentsRetrieving,
		EResponseRetrieving,
		EUserQuestionsRetrieving,
		EPrivateQuestionsRetrieving,
		EQuestionSubmitting
	};

	/**
	* Comment object structure.
	*/
	struct TComment
	{
		TDesC8* iNickName;
		TDesC8* iMemberId;
		TDesC8* iWebId;
		TDesC* iText;
		TTime iTime;
	};

	/**
	* User question object structure.
	*/
	struct TUserQuestion
	{
		TDesC* iText;
		TDesC8* iWebId;
	};

	/**
	* Private question object structure.
	*/
	struct TPrivateQuestion
	{
		TDesC8* iNickName;
		TDesC* iText;
		TDesC* iUrl;
		TTime iTime;
	};

	/**
	* Callback used in asynchronous requests
	*/
	class MRequestObserver
	{
	public:
		virtual void HandleRequestCompletedL(const TInt& aError, const TRequestType& aType) = 0;
	};

	class MAskAFriend
	{
	public:
		// Synchronous methods
		/**
		* Returns KErrNone if succeeds
		*/
		virtual TInt AttachPhoto(const TDesC8& aMemberId, const TDesC8& aWebPassword, 
			const TDesC8& aQuestionId, const TInt& aIndexOrder, const TFileName& aFilePath) = 0;

		/**
		* Returns KErrNone if succeeds
		*/
		virtual TInt CompleteQuestion(const TDesC8& aMemberId, const TDesC8& aWebPassword, 
			const TDesC8& aQuestionId) = 0;

		/**
		* Returns KErrNone if succeeds
		*/
		virtual TInt GetComments(const TDesC8& aMemberId, const TDesC8& aWebPassword, 
			const TDesC8& aQuestionId, const TDesC8& aLastCommentId) = 0;

		/**
		* Returns KErrNone if succeeds
		*/
		virtual TInt GetResponse(const TDesC8& aMemberId, const TDesC8& aWebPassword, 
			const TDesC8& aQuestionId) = 0;

		/**
		* Returns KErrNone if succeeds
		*/
		virtual TInt GetUserQuestions(const TDesC8& aMemberId, const TDesC8& aWebPassword,
			const TDesC8& aLastQuestionId) = 0;

		/**
		* Returns KErrNone if succeeds
		*/
		virtual TInt GetPrivateQuestions(const TDesC8& aMemberId, const TDesC8& aWebPassword) = 0;

		/**
		* Returns KErrNone if succeeds
		*/
		virtual TInt SubmitQuestion(const TDesC8& aMemberId, const TDesC8& aWebPassword,
			const TDesC& aText,	const TInt& aPhotosNumber, const TInt& aResponseType,
			const HBufC** aCustomResponses, const TInt& aDuration, const TBool& aPrivacyMark) = 0;	

		// Asynchronous methods
		/**
		* Returns KErrNone if succeeds
		*/
		virtual TInt AttachPhoto(const TDesC8& aMemberId, const TDesC8& aWebPassword, 
			const TDesC8& aQuestionId, const TInt& aIndexOrder, const TFileName& aFilePath, const MRequestObserver& aObserver) = 0;

		/**
		* Returns KErrNone if succeeds
		*/
		virtual TInt CompleteQuestion(const TDesC8& aMemberId, const TDesC8& aWebPassword, 
			const TDesC8& aQuestionId, const MRequestObserver& aObserver) = 0;

		/**
		* Returns KErrNone if succeeds
		*/
		virtual TInt GetComments(const TDesC8& aMemberId, const TDesC8& aWebPassword, 
			const TDesC8& aQuestionId, const TDesC8& aLastCommentId, const MRequestObserver& aObserver) = 0;

		/**
		* Returns KErrNone if succeeds
		*/
		virtual TInt GetResponse(const TDesC8& aMemberId, const TDesC8& aWebPassword, 
			const TDesC8& aQuestionId, const MRequestObserver& aObserver) = 0;

		/**
		* Returns KErrNone if succeeds
		*/
		virtual TInt GetUserQuestions(const TDesC8& aMemberId, const TDesC8& aWebPassword,
			const TDesC8& aLastQuestionId, const MRequestObserver& aObserver) = 0;

		/**
		* Returns KErrNone if succeeds
		*/
		virtual TInt GetPrivateQuestions(const TDesC8& aMemberId, const TDesC8& aWebPassword, const MRequestObserver& aObserver) = 0;

		/**
		* Returns KErrNone if succeeds
		*/
		virtual TInt SubmitQuestion(const TDesC8& aMemberId, const TDesC8& aWebPassword,
			const TDesC& aText,	const TInt& aPhotosNumber, const TInt& aResponseType,
			const HBufC** aCustomResponses, const TInt& aDuration, const TBool& aPrivacyMark, const MRequestObserver& aObserver) = 0;

		/**
		* Returns KErrNone if succeeds.
		* It is supposed that several DISTINCT asynchronous requests could be handled concurrently.
		*/
		virtual TInt CancelRequest(const TRequestType& aRequestType) = 0;

		// Getters methods
		/**
		* Get the last retrieved array of comments for the specified question (empty if no comment is available)
		*/
		void CommentsRequestResult(const TDesC8& iQuestionId, RPointerArray<TComment> &aReturnArray);
		/**
		* Gets the last retrieved array of user questions (empty if no question is available)
		*/
		void UQuestionsRequestResult(RPointerArray<TUserQuestion> &aReturnArray);
		/**
		* Gets the last retrieved array of user questions (empty if no question is available)
		*/
		void PQuestionsRequestResult(RPointerArray<TPrivateQuestion> &aReturnArray);
	};
}
