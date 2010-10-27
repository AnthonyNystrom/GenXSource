package n2f.sup.common.utils;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;

/**
 * This interface provides the common interface for object presentation to binary format (serialization) 
 * and Object restoration from binary data to correct instance(de serialization)
 *    
 * @author Zyl
 */
public interface Serializable {
	/**
	 * Writes instance of object to DataOutputStream. 
	 * @param out - DataOutputStream
	 * @throws IOException
	 */
	void writeObject(DataOutputStream out) throws IOException;

	/**
	 * Reads object from DataInputStream and restores it to correct instance. 
	 * @param in - DataInputStream
	 * @throws IOException
	 */
	void readObject(DataInputStream in) throws IOException;
	
}
