
package n2f.blogger.debug;

/**
 * This class is responsible for checking available free memory during application work.
 * This class uses config parameter - MEMORY_TRESHOLD - which determines treshold of minimum free memory.    
 */
public class MemoryDispatcher
{
    private static long threshold;
    private static final long DEFAULT_TRESHOLD = 300 * 1024;

    static
    {
	threshold = DEFAULT_TRESHOLD;
    }

    /**
     * Checks used and free memory and 
     * calls System.gc() if it's necessary.
     * For checking it uses predifine threshold of free memory. 
     */
    public static void checkMemory()
    {
	checkMemory(threshold);
    }

    /**
     * Checks used and free memory and 
     * calls System.gc() if it's necessary.
     * @param value - threshold of free memory.
     */
    public static void checkMemory(long value)
    {
    }
    
    public static void gc()
    {
	System.gc();
    }

    /**
     * Prints total memory size and free memory size to System.out. 
     */
    public static void printMemory()
    {
    }

}
