/* ------------------------------------------------
 * DESKeyGenerator.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.java.security.generators;

import genetibase.java.security.CipherKeyGenerator;
import genetibase.java.security.params.DESParameters;

public class DESKeyGenerator
    extends CipherKeyGenerator
{
    public byte[] generateKey()
    {
        byte[]  newKey = new byte[DESParameters.DES_KEY_LENGTH];

        do
        {
            random.nextBytes(newKey);

            DESParameters.setOddParity(newKey);
        }
        while (DESParameters.isWeakKey(newKey, 0));

        return newKey;
    }
}
