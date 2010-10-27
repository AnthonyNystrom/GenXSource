@class InputStream;
@class OutputStream;

@interface BaseService : NSObject
{
	NSString		* url;

	InputStream	* is;
	OutputStream	* os;
}

@property(assign) InputStream * is;
@property(assign) OutputStream * os;

- (id) init;
- (void) prepare:(NSInteger) requestId;
- (void) commit;
- (void) dealloc;

@end