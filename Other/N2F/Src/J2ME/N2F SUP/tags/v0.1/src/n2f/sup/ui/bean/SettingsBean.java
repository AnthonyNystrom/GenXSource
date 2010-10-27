package n2f.sup.ui.bean;

import n2f.sup.common.Resoursable;
import n2f.sup.core.CommonKeys;


public class SettingsBean extends AbstractBean {
	private String login;
	private String password;
	private String webMemberId;
	
	public SettingsBean(Resoursable resoursable) {
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
	
	public void saveBean(Resoursable resoursable) {
		writeParameters(resoursable);
	}

	private void readParameters(Resoursable resoursable) {
		login = resoursable.getProperty(CommonKeys.LOGIN);
		password = resoursable.getProperty(CommonKeys.PASSWORD);
		webMemberId = resoursable.getProperty(CommonKeys.WEBMEMBERID);
	}

	private void writeParameters(Resoursable resoursable) {
		resoursable.setProperty(CommonKeys.LOGIN, login);
		resoursable.setProperty(CommonKeys.PASSWORD, password);
		resoursable.setProperty(CommonKeys.WEBMEMBERID, webMemberId);
	}
}
