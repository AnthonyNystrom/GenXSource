package com.genetibase.askafriend.camera;

import javax.microedition.media.Player;

import com.genetibase.askafriend.common.PauseListener;

/**
 * This interface defines basic player functionality.
 * 
 * @author obraztsov
 */
public interface SimplePlayer extends PauseListener {
	
    /**
     * The returned value indicating that the requested time is unknown
     */
    long TIME_UNKNOWN = Player.TIME_UNKNOWN;
	
    /**
     * The state of the Player indicating that the Player is closed.
     * Calling close on the Player puts it in the CLOSED state. In the CLOSED state, the Player has released most of its resources and must not be used again
     */
    int CLOSED = Player.CLOSED;
	
    /**
     * The state of the Player indicating that it has acquired all the resources to begin playing
     * Once realized, a Player may still need to perform a number of time-consuming tasks before it is ready to be started. For example, it may need to acquire scarce or exclusive resources, fill buffers with media data, or perform other start-up processing
     */
    int PREFETCHED = Player.PREFETCHED;
	
    /**
     * The state of the Player indicating that it has acquired the required information but not the resources to function
     * A Player is in the REALIZED state when it has obtained the information required to acquire the media resources. Realizing a Player can be a resource and time consuming process. The Player may have to communicate with a server, read a file, or interact with a set of objects
     */
    int REALIZED = Player.REALIZED;
	
    /**
     * The state of the Player indicating that the Player has already started
     * Once prefetched, a Player can enter the STARTED state by calling the start method. A STARTED Player  means the Player is running and processing data. A Player returns to the PREFETCHED state when it stops, because the stop method was invoked, or it has reached the end of the media.
     */
    int STARTED = Player.STARTED;
	
    int END_OF_MEDIA = -12345;
	
    /**
     * The state of the Player indicating that it has not acquired the required information and resources to function
     * A Player starts in the UNREALIZED state. An unrealized Player does not have enough information to acquire all the resources it needs to function.
     */
    int UNREALIZED = Player.UNREALIZED;
	
    /**
     * This method starts playing track by the id
     */
    public void play(Object id, String type) ;
	
    /**
     * This method starts playing current determined playList
     */
    void play();
	
//    /**
//     * Set the volume using a linear point scale with values between 0 and 100
//     * @param level
//     */
//    void changeVolume(int level);
	
    /**
     * This method returns weather playeer is playing track
     * @return
     */
    boolean isPlaying();
	
//    /**
//     * Stops the Player. It will pause the playback at the current media time
//     */
//    void stop();
	
//    /**
//     * Pauses the Player. It will pause the playback at the current media time
//     */
//    void pause();
//	
//    /**
//     * Get the duration of the media. The value returned is the media's duration when played at the default rate.
//     * If the duration cannot be determined (for example, the Player is presenting live media) getDuration returns TIME_UNKNOWN.
//     * @return the duration of the media.
//     */
//    long getDuration();
//	
//    /**
//     * Returns the volume level with values between 0 and 100
//     * @return the volume level
//     */
//    int getVolume();
//	
//    /**
//     * Gets this Player's current media time. If the media time cannot be determined, getMediaTime returns TIME_UNKNOWN
//     */
//    long getMediaTime();
	
    /**
     * Gets the current state of this Player. The possible states are: UNREALIZED, REALIZED, PREFETCHED, STARTED, CLOSED
     * @return The Player's current state
     */
    int getState();
//    
//    boolean isPlayerInited();
}
