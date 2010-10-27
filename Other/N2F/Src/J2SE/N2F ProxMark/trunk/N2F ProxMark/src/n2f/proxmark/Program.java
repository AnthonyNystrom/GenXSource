/* ------------------------------------------------
 * Program.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark;

import javax.swing.SwingUtilities;

/**
 * @author Alex Nesterov
 */
final class Program
{
    /**
     * This class is immutable.
     */
    private Program()
    {
    }

    /**
     * @param args the command line arguments
     */
    public static void main(String... args)
    {
	Runnable runnable = new Runnable()
	{
	    public void run()
	    {
		MainForm mainForm = new MainForm();
		mainForm.setVisible(true);
	    }
	};
	SwingUtilities.invokeLater(runnable);
    }

}
