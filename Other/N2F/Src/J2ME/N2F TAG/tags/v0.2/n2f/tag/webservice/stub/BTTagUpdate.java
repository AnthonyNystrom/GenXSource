// This class was generated by the JAXRPC SI, do not edit.
// Contents subject to change without notice.
// JSR-172 Reference Implementation wscompile 1.0, using: JAX-RPC Standard Implementation (1.1, build R59)

package n2f.tag.webservice.stub;


public class BTTagUpdate {
    protected n2f.tag.webservice.stub.ArrayOfString tagValidationString;
    protected n2f.tag.webservice.stub.ArrayOfString deviceTagID;
    
    public BTTagUpdate() {
    }
    
    public BTTagUpdate(n2f.tag.webservice.stub.ArrayOfString tagValidationString, n2f.tag.webservice.stub.ArrayOfString deviceTagID) {
        this.tagValidationString = tagValidationString;
        this.deviceTagID = deviceTagID;
    }
    
    public n2f.tag.webservice.stub.ArrayOfString getTagValidationString() {
        return tagValidationString;
    }
    
    public void setTagValidationString(n2f.tag.webservice.stub.ArrayOfString tagValidationString) {
        this.tagValidationString = tagValidationString;
    }
    
    public n2f.tag.webservice.stub.ArrayOfString getDeviceTagID() {
        return deviceTagID;
    }
    
    public void setDeviceTagID(n2f.tag.webservice.stub.ArrayOfString deviceTagID) {
        this.deviceTagID = deviceTagID;
    }
}
