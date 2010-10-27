
#import "QuestionTabDelegate.h"
#import "N2FAppDelegate.h"
#import "AskQuestion.h"
#import "QuestionOptions.h"
#import "QuestionImages.h"

@implementation QuestionTabDelegate

- (void)tabBar:(UITabBar *)tabBar didSelectItem:(UITabBarItem *)item
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	AskQuestion * askQuestion = (AskQuestion*)delegate.askQuestionController.view;
	QuestionOptions * quOptions = (QuestionOptions*)delegate.questionOptionsController.view;
	QuestionImages * quImages = (QuestionImages*)delegate.questionImagesController.view;
	
	if(tabBar == askQuestion.tabBar)
	{
		if(item == askQuestion.itemOptions)
		{
			[delegate changeScreen:delegate.questionOptionsController];
		}
		else if(item == askQuestion.itemImages)
		{
			[delegate changeScreen:delegate.questionImagesController];
		}
	}
	else if(tabBar == quOptions.tabBar)
	{
		if(item == quOptions.itemQuestion)
		{
			[delegate changeScreen:delegate.askQuestionController];
		}
		else if(item == quOptions.itemImages)
		{
			[delegate changeScreen:delegate.questionImagesController];
		}
	}
	else if(tabBar == quImages.tabBar)
	{
		if(item == quImages.itemQuestion)
		{
			[delegate changeScreen:delegate.askQuestionController];
		}
		else if(item == quImages.itemOptions)
		{
			[delegate changeScreen:delegate.questionOptionsController];
		}
	}
}

@end
