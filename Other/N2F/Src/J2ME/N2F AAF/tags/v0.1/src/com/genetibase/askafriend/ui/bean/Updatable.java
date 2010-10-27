package com.genetibase.askafriend.ui.bean;

import com.genetibase.askafriend.common.utils.Serializable;

public interface Updatable {
	void update(String id, Serializable serializable);
}
