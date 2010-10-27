/* ------------------------------------------------
 * IAdvertListListener.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark;

/**
 * This interface should be implemented by the objects that are intending to
 * receive events related to the changes to the advert list.
 * @author Alex Nesterov
 */
interface IAdvertListListener
{
    /** Invoked when the list of advertisements changes. */
    void advertListChanged(AdvertListEvent e);
}
