package DeviceStream;

public class Bucket{

	private String _memberID;
	private String _name;
	private String _password;
	private String _streamName;

	public String getMemberID(){
		return _memberID;
	}

	public String getName(){
		return _name;
	}

	public String getPassword(){
		return _password;
	}

	public String getStreamName(){
		return _streamName;
	}
	
	public Bucket(){
		// default
	}
	
	public static Bucket parseMemberID(String str){
	
		Bucket bucket = new Bucket();
		
		if (str == null){
		    throw new NullPointerException("str");
		}

		String[] parts = str.split("/");

		if (parts.length < 4){
		    throw new IllegalArgumentException("str");
		}

		bucket._memberID = parts[0];
		bucket._password = parts[1];
		bucket._streamName = parts[2];
		bucket._name = parts[3];
	
		return bucket;
	}
}