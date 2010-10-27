

@interface Question : NSObject
{
	NSString	* question;
	int		responseType;
	int		duration;
	BOOL		isPrivate;
	NSString	* responseA;
	NSString	* responseB;
	NSMutableArray * photos;
	int		recordId;
	BOOL		sendNow;
}

@property (retain) NSString	* question;
@property (retain) NSString	* responseA;
@property (retain) NSString	* responseB;
@property (retain) NSMutableArray * photos;
@property (assign) int responseType;
@property (assign) int duration;
@property (assign) BOOL isPrivate;
@property (assign) int recordId;
@property (assign) BOOL sendNow;

- (id) init;
- (void) dealloc;

@end
