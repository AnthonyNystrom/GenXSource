package com.genetibase.askafriend.core;

import java.util.Enumeration;
import java.util.Vector;

import com.genetibase.askafriend.ui.LocaleUI;

public class Question {
	
	public static final String[] answerTypes = 
		new String[]{LocaleUI.OPTIONS_CHOICEGROUP_1_1, 
		LocaleUI.OPTIONS_CHOICEGROUP_1_2, 
		LocaleUI.OPTIONS_CHOICEGROUP_1_3, 
		LocaleUI.OPTIONS_CHOICEGROUP_1_4};
	public static final String[] answerDurations = 
		new String[]{LocaleUI.OPTIONS_CHOICEGROUP_2_1,
		LocaleUI.OPTIONS_CHOICEGROUP_2_2,
		LocaleUI.OPTIONS_CHOICEGROUP_2_3,
		LocaleUI.OPTIONS_CHOICEGROUP_2_4};
	
	private String[] pictures;
	private int selAnswerTypeIdx = 0;
	private int selAnswerDurationIdx = 0;
	private String[] userAnswers;
	private static Question instance;
	private StringBuffer questionText = new StringBuffer(256);
	private boolean privateQuestion;
	
	private Question() {
		super();
	}
	
	public synchronized static Question getInstance() {
		if (instance == null)
			instance = new Question();
		return instance;
	}
	
	public String[] getPictureNames() {
		return pictures;
	}
	
	public void setPictureNames(Vector pictures) {
		this.pictures = new String[pictures.size()];
		int i = 0;
		for (Enumeration en = pictures.elements();en.hasMoreElements();i++) {
			ItemLight2 item = (ItemLight2) en.nextElement();
			this.pictures[i] = item.getPath() + item.getTitle();
		}
	}
	
	public int getPicturesCount() 
	{
		return this.pictures == null? 0: this.pictures.length;
	}
	
	public int getSelAnswerDurationIdx() {
		return selAnswerDurationIdx;
	}
	
	public void setSelAnswerDurationIdx(int selAnswerDurationIdx) {
		this.selAnswerDurationIdx = selAnswerDurationIdx;
	}
	
	public int getSelAnswerTypeIdx() {
		return selAnswerTypeIdx;
	}
	
	public void setSelAnswerTypeIdx(int selAnswerTypeIdx) {
		this.selAnswerTypeIdx = selAnswerTypeIdx;
	}
	
	public String getSelectedAnswerType() {
		return answerTypes[selAnswerTypeIdx];
	}

	public String getSelectedAnswerDuration() {
		return answerTypes[selAnswerDurationIdx];
	}

	public String[] getUserAnswers() {
		return userAnswers;
	}

	public void setUserAnswers(Vector answers) {
		this.userAnswers = new String[answers.size()];
		int i = 0;
		for (Enumeration en = answers.elements();en.hasMoreElements();i++) {
			this.userAnswers[i] = (String) en.nextElement();
		}
	}
	
	public String getQuestionText() {
		return questionText.toString();
	}

	public void setQuestionText(String question) {
		if (question!=null && !question.equals(questionText)) {
			questionText.setLength(0);
			this.questionText.append(question);
		}
			
	}

	public boolean isPrivateQuestion() {
		return privateQuestion;
	}

	public void setPrivateQuestion(boolean privateQuestion) {
		this.privateQuestion = privateQuestion;
	}

	public synchronized void clear() 
	{
		if (questionText != null)
			questionText.setLength(0);
		this.questionText = null;
		this.userAnswers = null;
		this.pictures = null;
		instance = null;
	}

}
