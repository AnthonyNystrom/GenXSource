@class InputStream;
@class OutputStream;

@interface NetworkSystem : NSObject
{

}

+ (InputStream*)makeRequestTo:(NSString*)url from:(OutputStream*)os;


@end
