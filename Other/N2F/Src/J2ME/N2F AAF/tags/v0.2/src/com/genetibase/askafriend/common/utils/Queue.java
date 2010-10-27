package com.genetibase.askafriend.common.utils;

/**
 * This interface implements WorkerThread design pattern
 */
public interface Queue {

	/**
     * This method should put NetworkTask into WorkerThread
     *
     * @param runnableTask - see RunnableTask interface
     */
    void put(RunnableTask runnableTask);

    /**
     * Gets next RunnableTask in queue.
     * @return RunnableTask - It will be invoked in WorkerThread
     */
    RunnableTask getNext();
	
}