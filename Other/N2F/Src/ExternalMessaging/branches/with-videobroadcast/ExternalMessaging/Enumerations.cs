using System;
using System.ComponentModel;

public enum ContentType {ThreadReply = 9999/*Not exactly a comment type*/, 
    None = 0, 
    Video = 1, 
    Photo = 2, 
    Wall = 3, 
    NSpot = 4, 

    [Description("Mobile Video")]
    MobileVideo =5, 
    Blog = 6,
    AskAFriend = 7, 
    Member = 8,
    [Description("Photo Gallery")]
    PhotoGallery=21}

