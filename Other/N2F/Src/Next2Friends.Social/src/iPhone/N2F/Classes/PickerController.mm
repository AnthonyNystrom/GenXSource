
#import "PickerController.h"

NSArray * types = [[NSArray alloc] initWithObjects: @"Yes/No", @"A/B", @"Rate", @"Image select", nil];
NSArray * times = [[NSArray alloc] initWithObjects: @"3 minutes", @"15 minutes", @"1 hour", @"1 day", nil];

@implementation PickerController

@synthesize responseType;
@synthesize duration;

- (NSInteger)numberOfComponentsInPickerView:(UIPickerView *)pickerView
{
	return 2;
}

- (NSInteger)pickerView:(UIPickerView *)pickerView numberOfRowsInComponent:(NSInteger)component
{
	if(0 == component)
		return [types count];
	else
		return [times count];
}

- (NSString *)pickerView:(UIPickerView *)pickerView titleForRow:(NSInteger)row forComponent:(NSInteger)component
{
	if(0 == component)
		return [types objectAtIndex:row];
	else
		return [times objectAtIndex:row];
}

- (void)pickerView:(UIPickerView *)pickerView didSelectRow:(NSInteger)row inComponent:(NSInteger)component
{
	if(0 == component)
		responseType = row;
	else
		duration = row;
}

@end
