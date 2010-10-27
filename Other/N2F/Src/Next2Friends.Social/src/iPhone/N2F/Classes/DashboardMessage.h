#import "BaseScreen.h"

@interface DashboardMessage : BaseScreen
{
	IBOutlet UILabel * date;
	IBOutlet UITextView * text;
}
@property (retain) UILabel * date;
@property (retain) UITextView * text;

- (IBAction) onBack:(id)sender;

@end