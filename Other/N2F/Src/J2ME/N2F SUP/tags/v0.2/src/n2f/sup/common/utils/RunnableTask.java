package n2f.sup.common.utils;

/**
 * This interface is a part of WorkerThread design pattern.
 * It describes task that should be invoked from another Thread.
 */
public interface RunnableTask {
	int TYPE_SCALE_IMAGE = 3;
    /**
     * This method should contain core logic of runnable task.
     *
     * @throws Exception - throws if anything wrong :)
     */
    void execute() throws Exception;

    void interrupt();
    
    int getType();
    
//    void fire(Object source, int status);
	
}