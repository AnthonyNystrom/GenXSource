
#import "InputStream.h"

@implementation InputStream

- (id) initWithData:(NSData*)data
{
	is = [[NSInputStream inputStreamWithData:data]retain];
	[is open];
	return self;
}

- (void) dealloc
{
	[is close];
	[is release];
	
	[super dealloc];
}

- (int) readByte
{
	uint8_t retValue;
	[is read:(uint8_t*)&retValue maxLength:1];
	return retValue;
}

- (int) readByteArray:(char**)data
{
	int count = [self readInt];
	*data = new char[count];
	
	for(int i = 0; i < count; ++i)
	{
		(*data)[i] = [self readByte];
	}
	
	return count;
}

- (BOOL) readBool
{
	int res = [self readByte];
	return res == 0 ? false : true;
}

- (int) readBoolArray: (BOOL**)data
{
	int count = [self readInt];
	*data = new BOOL[count];
	
	for(int i = 0; i < count; ++i)
	{
		(*data)[i] = [self readBool];
	}
	
	return count;
}

- (short) readShort
{
	return ([self readByte] | ([self readByte] << 8));
}

- (int) readShortArray: (short**)data
{
	int count = [self readInt];
	*data = new short[count];
	
	for(int i = 0; i < count; ++i)
	{
		(*data)[i] = [self readShort];
	}
	
	return count;
}

- (int) readInt
{
	return ([self readByte] | ([self readByte] << 8) | ([self readByte] << 16) | ([self readByte] << 24));
}

- (int) readIntArray: (int**)data
{
	int count = [self readInt];
	*data = new int[count];
	
	for(int i = 0; i < count; ++i)
	{
		(*data)[i] = [self readInt];
	}
	
	return count;
}

- (long long) readLong
{
	return ((long long)[self readByte] | ((long long)[self readByte] << 8) | ((long long)[self readByte] << 16) | ((long long)[self readByte] << 24) | 
		  ((long long)[self readByte] << 32) | ((long long)[self readByte] << 40) | ((long long)[self readByte] << 48) | ((long long)[self readByte] << 56));
}

- (int) readLongArray: (long long**)data
{
	int count = [self readInt];
	*data = new long long[count];
	
	for(int i = 0; i < count; ++i)
	{
		(*data)[i] = [self readLong];
	}
	
	return count;
}

- (NSString*) readString
{	
	const int bufSize = 512;
	short * buf = new short[bufSize];
	
	int cnt = 0;
	short ch = -1;
	while(ch != 0)
	{
		ch = [self readShort];
		
		if(ch != 0)
		{
			buf[cnt++] = ch;
			if(cnt == bufSize)
			{
				NSLog(@"InputStream::readString error: buffer size exceeded");
				return 0;
			}
		}
	}
	NSString * string = [[NSString stringWithCharacters:(unichar*)buf length:cnt]retain];
	
	delete[] buf;
		
	return string;
}

- (int) readStringArray: (NSString***)data
{
	int count = [self readInt];
	*data = new NSString*[count];
	
	for(int i = 0; i < count; ++i)
	{
		(*data)[i] = [self readString];
	}
	
	return count;
}

- (DashboardNewFriend*) readDashboardnewfriend
{
	DashboardNewFriend * out = [DashboardNewFriend new];
	out->datetime = [self readString];
	out->nickname1 = [self readString];
	out->nickname2 = [self readString];
	return out;
}

- (NSArray*) readDashboardnewfriendArray
{
	int count = [self readInt];
	NSMutableArray * out = [[NSMutableArray alloc] initWithCapacity:count];
	for(int i = 0; i < count; ++i)
	{
		DashboardNewFriend * newFriend = [self readDashboardnewfriend];
		[out addObject:newFriend];
		[newFriend release];
	}
	return out;
}

- (DashboardWallComment*) readDashboardwallcomment
{
	DashboardWallComment * out = [DashboardWallComment new];
	out->datetime = [self readString];
	out->nickname1 = [self readString];
	out->nickname2 = [self readString];
	out->text = [self readString];
	return out;
}

- (NSArray*) readDashboardwallcommentArray
{
	int count = [self readInt];
	NSMutableArray * out = [[NSMutableArray alloc] initWithCapacity:count];
	for(int i = 0; i < count; ++i)
	{
		DashboardWallComment * newFriend = [self readDashboardwallcomment];
		[out addObject:newFriend];
		[newFriend release];
	}
	return out;
	
}

- (DashboardPhoto*) readDashboardphoto
{
	DashboardPhoto * out = [DashboardPhoto new];
	out->datetime = [self readString];
	out->nickname = [self readString];
	out->text = [self readString];
	out->title = [self readString];
	return out;	
}

- (NSArray*) readDashboardphotoArray
{
	int count = [self readInt];
	NSMutableArray * out = [[NSMutableArray alloc] initWithCapacity:count];
	for(int i = 0; i < count; ++i)
	{
		DashboardPhoto * newFriend = [self readDashboardphoto];
		[out addObject:newFriend];
		[newFriend release];
	}
	return out;
}

- (DashboardVideo*) readDashboardvideo
{
	DashboardVideo * out = [DashboardVideo new];
	out->datetime = [self readString];
	out->nickname = [self readString];
	out->text = [self readString];
	out->title = [self readString];
	return out;	
}

- (NSArray*) readDashboardvideoArray
{
	int count = [self readInt];
	NSMutableArray * out = [[NSMutableArray alloc] initWithCapacity:count];
	for(int i = 0; i < count; ++i)
	{
		DashboardVideo* newFriend = [self readDashboardvideo];
		[out addObject:newFriend];
		[newFriend release];
	}
	return out;
}

- (AskComment*) readAskcomment
{
	AskComment * out = [AskComment new];
	out->id = [self readInt];
	out->askquestionid = [self readInt];
	out->nickname = [self readString];
	out->text = [self readString];
	out->dtcreated = [self readString];
	
	return out;
}

- (AskQuestionStruct*) readAskquestionstruct
{
	AskQuestionStruct * out = [AskQuestionStruct new];
	out->id = [self readInt];
	out->question = [self readString];
	out->dtcreated = [self readString];
	
	return out;
}

- (AskResponse*) readAskresponse
{
	AskResponse * out = [AskResponse new];
	out->askquestionid = [self readInt];
	out->question = [self readString];
	out->size = [self readByteArray:&out->photobase64binary];
	out->responsesSize = [self readIntArray:&out->responsevalues];
	out->average = [self readInt];
	out->responsetype = [self readInt];
	NSString ** customresponses;
	[self readStringArray:&customresponses];
	out->customresponses = [[NSArray alloc] initWithObjects: customresponses[0], customresponses[1], 0];
	
	return out;
}

- (AskQuestionConfirm*) readAskquestionconfirm
{
	AskQuestionConfirm * out = [AskQuestionConfirm new];
	out->adverturl = [self readString];
	out->advertimage = [self readString];
	out->askquestionid = [self readString];
	
	return out;
}

@end
