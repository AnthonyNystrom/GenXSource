package n2f.sup.common.network;

public interface ResponseListener 
{
	int RESPONSE_TYPE_REPLY = 0;
	int RESPONSE_TYPE_PRIVATE_QUESTION = 1;
	
	void notifyNewResponseReceived(int type);
}
