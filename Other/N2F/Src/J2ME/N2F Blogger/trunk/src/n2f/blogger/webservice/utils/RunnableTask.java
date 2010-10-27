
package n2f.blogger.webservice.utils;

/**
 * This interface is a part of WorkerThread design pattern.
 * It describes task that should be invoked from another Thread.
 */
public interface RunnableTask
{
    int TYPE_SCALE_IMAGE = 3;
    int TYPE_SMS_SEND = 19;

    /**
     * This method should contain core logic of runnable task.
     *
     * @throws Exception - throws if anything wrong :)
     */
    void execute() throws Exception;

    void interrupt();

    int getType();

}
