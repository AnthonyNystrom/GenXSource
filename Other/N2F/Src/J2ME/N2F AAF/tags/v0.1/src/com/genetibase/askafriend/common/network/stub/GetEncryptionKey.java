// This class was generated by the JAXRPC SI, do not edit.
// Contents subject to change without notice.
// JSR-172 Reference Implementation wscompile 1.0, using: JAX-RPC Standard Implementation (1.1, build R59)

package com.genetibase.askafriend.common.network.stub;


public class GetEncryptionKey {
    protected java.lang.String webMemberID;
    protected java.lang.String webPassword;
    
    public GetEncryptionKey() {
    }
    
    public GetEncryptionKey(java.lang.String webMemberID, java.lang.String webPassword) {
        this.webMemberID = webMemberID;
        this.webPassword = webPassword;
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
}
