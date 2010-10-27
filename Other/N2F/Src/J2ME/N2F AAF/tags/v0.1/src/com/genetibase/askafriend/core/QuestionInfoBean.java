package com.genetibase.askafriend.core;

import com.genetibase.askafriend.ui.AbstractBean;

public class QuestionInfoBean extends AbstractBean 
{
	private String questionId, questionTxt, lastCommentId;
	private boolean hasNewComments;
	private long timePosted = 0;

	public QuestionInfoBean(String questionId, String questionTxt, 
			String lastCommentId, long timePosted)
	{
		super();
		this.questionId = questionId;
		this.timePosted = timePosted;
		this.questionTxt = questionTxt;
		this.lastCommentId = lastCommentId;
	}

	public String getQuestionId() {
		return questionId;
	}

	public String getQuestionTxt() {
		return questionTxt;
	}
	
	public String getLastCommentId() {
		return lastCommentId;
	}

	public void setLastCommentId(String lastCommentId) {
		this.lastCommentId = lastCommentId;
	}
	
	public void setHasNewComments(boolean val) {
		this.hasNewComments = val;
	}
	
	public void setQuestionTxt(String text) {
		this.questionTxt = text;
	}

	public boolean hasNewComments() {
		return hasNewComments;
	}
	
	public int hashCode() {
		final int PRIME = 31;
		int result = 1;
		result = PRIME * result + ((questionId == null) ? 0 : questionId.hashCode());
		return result;
	}

	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		final QuestionInfoBean other = (QuestionInfoBean) obj;
		if (questionId == null) {
			if (other.questionId != null)
				return false;
		} else if (!questionId.equals(other.questionId))
			return false;
		return true;
	}
	
	public String toString() 
	{
		StringBuffer sb = new StringBuffer("QuestionBean[");
		sb.append("id=").append(questionId);
		sb.append(" text=").append(questionTxt);
		sb.append(" last comment id=").append(lastCommentId);
		sb.append(" last comment date=").append(timePosted).append("]");
		return sb.toString();
	}

	public long getLastTimeCommentPosted() {
		return timePosted;
	}

	public void setLastTimeCommentPosted(String timePosted) {
		if (timePosted!=null && timePosted.trim().length()>0)
		this.timePosted = Long.parseLong(timePosted.trim());
	}
	
}
