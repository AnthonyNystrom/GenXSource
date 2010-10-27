
package genetibase.java.microedition;

/**
 * @author Zyl
 *         <p/>
 *         Calling the free() method in that interface suggests that the implemented interface expend effort toward recycling unused objects in order to make the memory they currently occupy available for quick reuse
 */
public interface Deallocatable
{
    /**
     * Reallocates resources
     */
    void free();

}
