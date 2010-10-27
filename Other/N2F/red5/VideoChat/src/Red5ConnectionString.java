package VideoChat;

public class Red5ConnectionString{

	private String _title;
	private String _nickname;
	private String _password;
	private String _streamName;
        private Integer _isPrivate;

	public String getTitle(){
		return _title;
	}

	public String getNickname(){
		return _nickname;
	}

	public String getPassword(){
		return _password;
	}

	public String getStreamName(){
		return _streamName;
	}
        
        public Integer getIsPrivate(){
		return _isPrivate;
	}
	
	public Red5ConnectionString(){
		// default
	}
	
	public static Red5ConnectionString parseConnectionString(String str){
	
		Red5ConnectionString bucket = new Red5ConnectionString();
		
		if (str == null){
		    throw new NullPointerException("str");
		}

		String[] parts = str.split("/");

		if (parts.length < 5){
		    throw new IllegalArgumentException("str");
		}

		bucket._nickname = parts[0];
		bucket._password = parts[1];
		bucket._streamName = parts[2];
		bucket._title = parts[3];
                
                try
                {
                    bucket._isPrivate = Integer.parseInt(parts[4]) ;
                }
                catch(Exception ex)
                {
                    System.out.println("Couldnt parse Privacy flag, defaulting to public");
                    bucket._isPrivate = 0;
                }
                
                System.out.println("Video privacy is set to "+bucket._isPrivate);
	
		return bucket;
	}
}