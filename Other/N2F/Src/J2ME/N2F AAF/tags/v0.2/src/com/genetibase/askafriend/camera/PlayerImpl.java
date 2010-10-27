package com.genetibase.askafriend.camera;

import javax.microedition.media.Manager;
import javax.microedition.media.MediaException;
import javax.microedition.media.Player;
import javax.microedition.media.PlayerListener;

import com.genetibase.askafriend.common.AbstractErrorManager;
import com.genetibase.askafriend.common.Deallocatable;
import com.genetibase.askafriend.common.ErrorEvent;

/**
 * This class responds for Player feature and implements PlayerEx interface.
 */
public class PlayerImpl extends AbstractErrorManager implements SimplePlayer, Runnable, Deallocatable {

	private static final byte TYPE_URL = 0;

	private byte sourceType = 0;

	private Object source = null;

	protected volatile Player player;

	protected Thread dThread;

	protected Object dThreadLock = new Object();

	protected Object pauseLock = new Object();

	protected volatile boolean interrupted, paused, inited;

//	private Vector eventListeners = new Vector();

//	public void addEventListener(EventListener eventListener) {
//		if (!eventListeners.contains(eventListener)) {
//			eventListeners.addElement(eventListener);
//		}
//	}
//
//	public void removeEventListener(EventListener eventListener) {
//		eventListeners.removeElement(eventListener);
//	}
//
//	private void fireListener(int state) {
//		for (int i = 0; i < eventListeners.size(); i++) {
//			((EventListener) eventListeners.elementAt(i))
//					.playerStateChanged(state);
//		}
//	}

	/**
	 * Default constructor.
	 */
	public PlayerImpl() {
	}

	/**
	 * Return current player instance.
	 * 
	 * @return player instance.
	 */
	public Player getPlayer() {
		return player;
	}

	/**
	 * Sets player.
	 * 
	 * @param player -
	 *            player to set.
	 */
	public void setPlayer(Player player) {
		stopAndClose();
		this.player = player;
	}

	protected void play_() {
		// player was paused

		if (player != null) {
			// wake up paused thread
			synchronized (pauseLock) {
				paused = false;
				pauseLock.notify();
			}
			try {
				if (getState() > Player.CLOSED)
					player.start();
			} catch (MediaException me) {
				player = null;
				fireError(me.getMessage(), 
						new ErrorEvent(this, 
								new RuntimeException("Sorry, MediaException happened in player"),
								"Sorry, MediaException happened in player", ErrorEvent.MEDIA_FAILURE));
				me.printStackTrace();
			}

			return;
		}

		// start new player
		synchronized (dThreadLock) {
			stopping();
			interrupted = false;
			paused = false;
			dThread = new Thread(this);
			dThread.start();
		}
	}

	protected void stopping() {
		if (player == null)
			return;
		synchronized (dThreadLock) {
			interrupted = true;
			// wake up thread if it is paused
			synchronized (pauseLock) {
				pauseLock.notify();
			}
		}
	}

	/**
	 * workaround: for recoding don't need call player.close() otherwise need
	 * call player.close()
	 */
	public void stopAndClose() {
		stopAndClose(true);
	}

	private Object playerMutex = new Object();

	private void stopAndClose(boolean needToClose) {
		if (getState() > Player.CLOSED) {
			try {
				if (needToClose) {
					synchronized (playerMutex) {
						player.close();
					}
				}
			} catch (Exception e) {
				e.printStackTrace();
			} finally {
				if (getState() == Player.CLOSED) {
					synchronized (playerMutex) {
						player = null;
					}
				}
			}
		} else {
			player = null;
//			fireListener(SimplePlayer.CLOSED);
		}
		paused = false;
	}


	/**
	 * 
	 */
	public boolean isPlaying() {
		// TODO RunnablePlayer without isBuffering
		return (player != null) && (player.getState() >= Player.STARTED);
	}

	private void createPlayer(String url) {
		try {
			player = Manager.createPlayer(url);
		} catch (Exception ex) {
			ex.printStackTrace();
			if (player != null) {
				player.close();
				player = null;
			}
			fireError(ex.getMessage(), 
					new ErrorEvent(this, 
							new RuntimeException("Sorry, MediaException happened in player"),
							"Sorry, MediaException happened in player", ErrorEvent.MEDIA_FAILURE));

		}
	}

	/**
	 * @param source
	 */
	public void createPlayer(Object source) {
		this.source = source;
		if (source instanceof String) {
			sourceType = TYPE_URL;
		}

		switch (sourceType) {
		case TYPE_URL:
			createPlayer((String) source);
		}
	}

	// TODO: review the code
	public void run() {
		if (!interrupted)
			synchronized (dThreadLock) {
				if (source != null) {
					createPlayer(source);
					addStateLitener();
				}
			}

		if (player == null) {
			// can't create player
			synchronized (dThreadLock) {
				dThread = null;
				dThreadLock.notify();
				return;
			}
		}
		if (player != null && !paused) {
			startPlayer();
		}
	}

	private void addStateLitener() {
		if (player != null)
			player.addPlayerListener(new PlayerListener() {

				public void playerUpdate(Player player, String eventType,
						Object eventData) {

					if (eventType.equals(PlayerListener.STARTED)) {
						log("-=STARTED:");
//						fireListener(SimplePlayer.STARTED);
					} else if (eventType.equals(PlayerListener.CLOSED)) {
						PlayerImpl.this.player = null;
						log("-==CLOSED==-");
						// kill the waiting thread
						interrupted = true;
//						fireListener(SimplePlayer.CLOSED);
					} else if (eventType
							.equals(PlayerListener.DEVICE_AVAILABLE)) {
						log("-==DEVICE_AVAILABLE==-");
					} else if (eventType
							.equals(PlayerListener.DEVICE_UNAVAILABLE)) {
						log("-=DEVICE_UNAVAILABLE=-");
					} else if (eventType.equals(PlayerListener.END_OF_MEDIA)) {
						log("PLAYER.END_OF_MEDIA");
					} else if (eventType.equals(PlayerListener.STOPPED)) {
						log("-==STOPPED==-");
					} else if (eventType.equals(PlayerListener.ERROR)) {
						log("--=PLAYER_ERROR: player state:" + getState());
					}
				}

			});
	}

	protected void prefetchPlayer() throws MediaException {
		if (player != null) {
			player.realize();
			//commented because of test
//			player.prefetch();
		} else if (player == null) {
			throw new NullPointerException("PlayerImpl.prefetchPlayer()");
		}
	}

	// ???????????
	// TODO review
	public void startPlayer() {
		try {
			if (player != null) {
				synchronized (playerMutex) {
					if (getState() == SimplePlayer.UNREALIZED) {
						player.realize();
						player.prefetch();
					}
					if (getState() == Player.PREFETCHED) {
						player.start();
					}
				}
			}

			paused = false;
		} catch (MediaException ex) {
			ex.printStackTrace();
			fireError(ex.getMessage(), 
					new ErrorEvent(this, 
							new RuntimeException("Sorry, MediaException happened in player"),
							"Sorry, MediaException happened in player", ErrorEvent.MEDIA_FAILURE));

		} catch (Exception ex) {
			ex.printStackTrace();
		}

		condition();

		synchronized (dThreadLock) {
			dThread = null;
			dThreadLock.notify();
		}
		// There should be notification that run was finished
	}

	protected void condition() {
		// mtime update loop
		while (!interrupted) {
			try {
				Thread.sleep(100);
			} catch (Exception ex) {
				ex.printStackTrace();
			}
			// pause the loop if player paused
			synchronized (pauseLock) {
				if (paused) {
					try {
						pauseLock.wait();
					} catch (InterruptedException ie) {
						ie.printStackTrace();
					}
				}
			}
		}
	}

	public void play() {
		stopping();
		play_();
	}

	/**
	 * 
	 */
	public void play(Object id) {
		if (id != null) {
			this.source = id;
			if (source instanceof String) {
				sourceType = TYPE_URL;
			}
			play();
		}
	}

	/**
	 * 
	 */
	public void play(Object id, String cType) {
		this.source = id;
		play(id);
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @see com.musiwave.de.player.PlayerEx#getState()
	 */
	public int getState() {
		int ret = SimplePlayer.CLOSED;
		if (player != null)
			ret = player.getState();
		return ret;
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @see com.musiwave.de.player.SimplePlayer#free()
	 */
	public void free() {
		removeAllErrorListeners();		
		stopAndClose();
	}

	public void setInterrupted(boolean interrupted) {
		this.interrupted = interrupted;
	}

	public void setSource(Object source, String contentType) {
		this.source = source;
	}

	/**
	 * @see com.musiwave.de.media.Recorder#close()
	 */
	public void close() {
		stopAndClose();
	}

	public void notify(int state) {
		if (state == STATE_PAUSE) {
			close();
		}
	}
}
