// This class was generated by the JAXRPC SI, do not edit.
// Contents subject to change without notice.
// JSR-172 Reference Implementation wscompile 1.0, using: JAX-RPC Standard Implementation (1.1, build R59)

package com.genetibase.askafriend.common.network.stub;


public class GetNewAAFQuestionCommentIDs {
    protected java.lang.String webMemberID;
    protected java.lang.String webPassword;
    protected com.genetibase.askafriend.common.network.stub.ArrayOfString webAskAFriendIDs;
    protected com.genetibase.askafriend.common.network.stub.ArrayOfString webLastAskAFriendQuestionIDs;
    
    public GetNewAAFQuestionCommentIDs() {
    }
    
    public GetNewAAFQuestionCommentIDs(java.lang.String webMemberID, java.lang.String webPassword, com.genetibase.askafriend.common.network.stub.ArrayOfString webAskAFriendIDs, com.genetibase.askafriend.common.network.stub.ArrayOfString webLastAskAFriendQuestionIDs) {
        this.webMemberID = webMemberID;
        this.webPassword = webPassword;
        this.webAskAFriendIDs = webAskAFriendIDs;
        this.webLastAskAFriendQuestionIDs = webLastAskAFriendQuestionIDs;
    }
    
    public java.lang.String getWebMemberID() {
        return webMemberID;
    }
    
    public void setWebMemberID(java.lang.String webMemberID) {
        this.webMemberID = webMemberID;
    }
    
    public java.lang.String getWebPassword() {
        return webPassword;
    }
    
    public void setWebPassword(java.lang.String webPassword) {
        this.webPassword = webPassword;
    }
    
    public com.genetibase.askafriend.common.network.stub.ArrayOfString getWebAskAFriendIDs() {
        return webAskAFriendIDs;
    }
    
    public void setWebAskAFriendIDs(com.genetibase.askafriend.common.network.stub.ArrayOfString webAskAFriendIDs) {
        this.webAskAFriendIDs = webAskAFriendIDs;
    }
    
    public com.genetibase.askafriend.common.network.stub.ArrayOfString getWebLastAskAFriendQuestionIDs() {
        return webLastAskAFriendQuestionIDs;
    }
    
    public void setWebLastAskAFriendQuestionIDs(com.genetibase.askafriend.common.network.stub.ArrayOfString webLastAskAFriendQuestionIDs) {
        this.webLastAskAFriendQuestionIDs = webLastAskAFriendQuestionIDs;
    }
}
