
@class AskComment;
@interface OutputStream : NSObject
{
	NSOutputStream * os;
}

- (id) init;
- (void) dealloc;
- (NSData*) getData;

- (void) writeByte:(int)byte;
- (void) writeByteArray:(char*)value size:(int)size;
- (void) write:(id)value;
- (void) writeArray:(id)value size:(int)size;

- (void) writeBool:(BOOL)value;
- (void) writeBoolArray:(BOOL*)value size:(int)size;

- (void) writeShort:(short)value;
- (void) writeShortArray:(short*)value size:(int)size;

- (void) writeInt:(int)value;
- (void) writeIntArray:(int*)value size:(int)size;

- (void) writeLong:(long long)value;
- (void) writeLongArray:(long long*)value size:(int)size;

- (void) writeString:(NSString*)value;
- (void) writeStringArray:(NSString**)value size:(int)size;

- (void) writeAskcomment:(AskComment*)newComment;


@end

