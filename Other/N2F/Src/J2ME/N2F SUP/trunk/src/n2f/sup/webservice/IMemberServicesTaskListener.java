/* ------------------------------------------------
 * IMemberServicesTaskListener.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.sup.webservice;

/**
 * @author Alex Nesterov
 */
public interface IMemberServicesTaskListener
{
    void getMemberIDCompleted(String webMemberID, String login, String password);
}
