/*
============================================================================
 Name        : Next2Friends.pan
 Author      : 
 Version     :
 Copyright   : Your copyright notice
 Description : Application panic codes
============================================================================
*/

#ifndef __NEXT2FRIENDS_PAN__
#define __NEXT2FRIENDS_PAN__

/** Next2Friends application panic codes */
enum TNext2FriendsPanics
    {
    ENext2FriendsUi = 1
    // add further panics here
    };

inline void Panic(TNext2FriendsPanics aReason)
    {
    _LIT(applicationName,"Next2Friends");
    User::Panic(applicationName, aReason);
    }

#endif // __NEXT2FRIENDS_PAN__
