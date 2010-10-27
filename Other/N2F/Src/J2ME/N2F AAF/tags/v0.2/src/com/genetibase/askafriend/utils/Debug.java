package com.genetibase.askafriend.utils;

import com.genetibase.askafriend.common.Resoursable;

/**
 * The main aim of this class is to save and check permissions.
 */
public class Debug {
    public static boolean consoleOutput = true, debug = true, systemOutput = true;

    /**
     * reads permissions form config
     */
    public static void init (Resoursable resoursable) {
        consoleOutput = checkFlag( "com-debug-output-console", resoursable);
        debug = checkFlag( "com-debug-debug", resoursable);
        systemOutput = checkFlag("com-debug-output-system", resoursable);

    }

    /**
     * Checks flags
     *
     * @param midlet
     * @param s
     * @return
     */
    private static boolean checkFlag( String s, Resoursable resoursable) {
		//TODO: make this method correct
        return true;//"on".equals(resourcable.getConfigValue(s,PluginKey.APPL_CONFIG)) || Common.TRUE.equals(resourcable.getConfigValue(s, PluginKey.APPL_CONFIG));
    }

//    /**
//     * Shows concole
//     *
//     * @param display
//     */
//    public static void showConsole(Display display) {
//        Console.show(display);
//    }
//
//    /**
//     * generates BEEP tone signal according count number
//     *
//     * @param count
//     */
//    public static void beep(int count) {
//        if (!debug) return;
//        try {
//            Manager.playTone(80 + 2 * count, 100 * count, 70);
//        } catch (MediaException e) {
//        	e.printStackTrace();
//			Debug.println("BEEP Error");
//            error("Beep", e);
//        }
//    }
//
    /**
     * Prints String on concoles
     *
     * @param s
     */
    public static void print(String s) {
		
        if (!debug) return;
        if (consoleOutput) Console.print(s);
        if (systemOutput) System.out.print(s);
    }

    /**
     * Prints String on concole and returns carriage
     *
     * @param s
     */
    public static void println(String s) {
        print(s + '\n');
    }

    /**
     * Prints throwable on concole with appropriate comment
     *
     * @param comment
     * @param t
     */
    public static void error(String comment, Throwable t) {
        println("ERROR: " + comment + " - " + t);
    }

    /**
     * Checks permissons on concole output
     *
     * @return
     */
    public static boolean isConsoleOutput() {
        return consoleOutput;
    }

    /**
     * determines permissions for concole output
     *
     * @param consoleOutput
     */
    public static void setConsoleOutput(boolean consoleOutput) {
        Debug.consoleOutput = consoleOutput;
    }

    /**
     * Checks Debug mode
     *
     * @return
     */
    public static boolean isDebug() {
        return debug;
    }

    /**
     * Determines Debug mode
     *
     * @param debug
     */
    public static void setDebug(boolean debug) {
        Debug.debug = debug;
    }

    /**
     * Checks sysoutput mode
     *
     * @return
     */
    public static boolean isSystemOutput() {
        return systemOutput;
    }

    /**
     * Determines Sysoutput mode
     *
     * @param systemOutput
     */
    public static void setSystemOutput(boolean systemOutput) {
        Debug.systemOutput = systemOutput;
    }
	
}
