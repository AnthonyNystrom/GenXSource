
@interface AskResponse : NSObject
{
@public
    int askquestionid;
    NSString* question;
	
    char* photobase64binary;
    int size;
	
    int* responsevalues;
    int responsesSize;
	
    int average;
    int responsetype;
    NSArray* customresponses;
};

@end