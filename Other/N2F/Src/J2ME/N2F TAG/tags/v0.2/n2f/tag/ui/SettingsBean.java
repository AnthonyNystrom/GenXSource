package n2f.tag.ui;

import javax.microedition.rms.RecordStoreException;

import n2f.tag.core.Settings;


public class SettingsBean extends AbstractBean {
	private String login;
	private String password;
	private String webMemberId;
	private static final String LOGIN = "login"; 
	private static final String PASSWORD = "password";
	private static final String WEB_ID = "webid";
	
	public SettingsBean(Settings resoursable) {
		readParameters(resoursable);
	}
	
	public String getLogin() {
		return login;
	}
	
	public void setLogin(String login) {
		this.login = login;
	}
	
	public String getPassword() {
		return password;
	}
	
	public void setPassword(String password) {
		this.password = password;
	}
	
	public String getWebMemberId() {
		return webMemberId;
	}

	public void setWebMemberId(String webMemberId) {
		this.webMemberId = webMemberId;
	}
	
	public int hashCode() {
		final int PRIME = 31;
		int result = 1;
		result = PRIME * result + ((login == null) ? 0 : login.hashCode());
		result = PRIME * result + ((password == null) ? 0 : password.hashCode());
		return result;
	}
	
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		final SettingsBean other = (SettingsBean) obj;
		if (login == null) {
			if (other.login != null)
				return false;
		} else if (!login.equals(other.login))
			return false;
		if (password == null) {
			if (other.password != null)
				return false;
		} else if (!password.equals(other.password))
			return false;
		return true;
	}
	
	public void saveBean(Settings resoursable) {
		writeParameters(resoursable);
	}

	private void readParameters(Settings resoursable) {
		
		login = resoursable.get(LOGIN);
		password = resoursable.get(PASSWORD);
		webMemberId = resoursable.get(WEB_ID);
	}

	private void writeParameters(Settings resoursable) {
		resoursable.put(LOGIN, login);
		resoursable.put(PASSWORD, password);
		resoursable.put(WEB_ID, webMemberId);
		resoursable.setHaveCredentials();
		try {
			resoursable.save();
		} catch (RecordStoreException e) {
			e.printStackTrace();
		}
	}
}
