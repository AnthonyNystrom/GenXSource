// This class was generated by the JAXRPC SI, do not edit.
// Contents subject to change without notice.
// JSR-172 Reference Implementation wscompile 1.0, using: JAX-RPC Standard Implementation (1.1, build R59)

package com.genetibase.askafriend.common.network.stub;


public class AttachPhoto {
    protected java.lang.String webMemberID;
    protected java.lang.String webPassword;
    protected java.lang.String webAskAFriendID;
    protected int indexOrder;
    protected java.lang.String photoBase64String;
    
    public AttachPhoto() {
    }
    
    public AttachPhoto(java.lang.String webMemberID, java.lang.String webPassword, java.lang.String webAskAFriendID, int indexOrder, java.lang.String photoBase64String) {
        this.webMemberID = webMemberID;
        this.webPassword = webPassword;
        this.webAskAFriendID = webAskAFriendID;
        this.indexOrder = indexOrder;
        this.photoBase64String = photoBase64String;
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
    
    public java.lang.String getWebAskAFriendID() {
        return webAskAFriendID;
    }
    
    public void setWebAskAFriendID(java.lang.String webAskAFriendID) {
        this.webAskAFriendID = webAskAFriendID;
    }
    
    public int getIndexOrder() {
        return indexOrder;
    }
    
    public void setIndexOrder(int indexOrder) {
        this.indexOrder = indexOrder;
    }
    
    public java.lang.String getPhotoBase64String() {
        return photoBase64String;
    }
    
    public void setPhotoBase64String(java.lang.String photoBase64String) {
        this.photoBase64String = photoBase64String;
    }
}
