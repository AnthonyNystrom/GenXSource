#import "OutputStream.h"
#import "AskComment.h"

@implementation OutputStream

- (id)init
{
	os = [[NSOutputStream outputStreamToMemory] retain];
	[os open];
	return self;
}

- (void) dealloc
{
	[os close];
	[os release];
	
	[super dealloc];
}

- (void)writeByte: (int)byte
{
	[os write:(const unsigned char*)&byte maxLength:1];
}

- (void) writeByteArray:(char*)value size:(int)size
{
	[self writeInt: size];
	for(int i = 0; i < size; ++i)
	{
		[self writeByte: value[i]];
	}
}
 
- (NSData*)getData
{
	[os close];
	return [os propertyForKey:NSStreamDataWrittenToMemoryStreamKey];
}
 
- (void) write: (id) data
{
	if([data isKindOfClass:[NSNumber class]])
	{
		NSNumber * number = (NSNumber*)data;
		NSLog(@"type is %s", [number objCType]);
	}
}

- (void) writeArray:(id)value size:(int)size
{
	
}

- (void) writeBool:(BOOL)value
{
	int res = value == YES ? 1 : 0;
	[self writeByte:res];
}

- (void) writeBoolArray:(BOOL*)value size:(int)size
{
	[self writeInt: size];
	for(int i = 0; i < size; ++i)
	{
		[self writeBool: value[i]];
	}
}

- (void) writeShort:(short)value
{
	[self writeByte:value & 0xff];
	[self writeByte:value >> 8 & 0xff];
}

- (void) writeShortArray:(short*)value size:(int)size
{
	[self writeInt: size];
	for(int i = 0; i < size; ++i)
	{
		[self writeShort: value[i]];
	}
}

- (void) writeInt:(int)value
{
	[self writeByte:value & 0xff];
	[self writeByte:value >> 8 & 0xff];
	[self writeByte:value >> 16 & 0xff];
	[self writeByte:value >> 24 & 0xff];
}

- (void) writeIntArray:(int*)value size:(int)size
{
	[self writeInt: size];
	for(int i = 0; i < size; ++i)
	{
		[self writeInt: value[i]];
	}
}

- (void) writeLong:(long long)value
{
	[self writeByte:value & 0xff];
	[self writeByte:value >> 8 & 0xff];
	[self writeByte:value >> 16 & 0xff];
	[self writeByte:value >> 24 & 0xff];
	[self writeByte:value >> 32 & 0xff];
	[self writeByte:value >> 40 & 0xff];
	[self writeByte:value >> 48 & 0xff];
	[self writeByte:value >> 56 & 0xff];	
}

- (void) writeLongArray:(long long*)value size:(int)size
{
	[self writeInt: size];
	for(int i = 0; i < size; ++i)
	{
		[self writeLong: value[i]];
	}
}

- (void) writeString:(NSString*)value
{
	int len = [value length];
	
	for(int i = 0; i < len; ++i)
	{
		[self writeShort:[value characterAtIndex:i]];
	}
	[self writeShort:0];
}

- (void) writeStringArray:(NSString**)value size:(int)size
{
	[self writeInt: size];
	for(int i = 0; i < size; ++i)
	{
		[self writeString: value[i]];
	}
}

- (void) writeAskcomment:(AskComment*)newComment
{
	[self writeInt:newComment->id];
	[self writeInt:newComment->askquestionid];
	[self writeString:newComment->nickname];
	[self writeString:newComment->text];
	[self writeString:newComment->dtcreated];
}


@end
