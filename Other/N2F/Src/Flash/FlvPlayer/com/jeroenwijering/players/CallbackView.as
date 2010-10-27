﻿/**
* Callback to serverside script for statistics handling.
* It sends the current file,title,id and state on start and complete.
*
* @author	Jeroen Wijering
* @author	Nate Hanna
* @version	1.6
**/


import com.jeroenwijering.players.*;


class com.jeroenwijering.players.CallbackView extends AbstractView { 


	/** Currently playing item **/
	private var currentItem:Number;
	/** Currently playing item **/
	private var varsObject:LoadVars;
	/** Boolean for if a start call has already been sent for an item. **/
	private var playSent:Boolean = false;
	/** Small interval so both complete and play events won't be issued **/
	private var playSentInt:Number;
	/** Timestamp of the start of the movie **/
	private var startStamp:Number;


	/** Constructor **/
	function CallbackView(ctr:AbstractController,cfg:Object,fed:Object) {
		super(ctr,cfg,fed);
		if(config['callback'] != "analytics") {
			varsObject = new LoadVars();
		}
	};


	/** Send a callback on state change **/
	private function setState(pr1:Number) {
		if(pr1 == 3) {
			var dur = Math.round(new Date().valueOf()/1000 - startStamp);
			sendVars("stop",dur,true);
			playSent = false;
		} else if (pr1 == 2 && playSent == false) {
			playSentInt = setInterval(this,"sendVars",500,"start",0);
			playSent = true;
			startStamp = new Date().valueOf()/1000;
		}
	};


	/** save the currently playing item **/
	private function setItem(pr1:Number) {
		if(playSent == true && currentItem != undefined)  {
			var dur = Math.round(new Date().valueOf()/1000 - startStamp);
			sendVars("stop",dur,false);
			playSent = false;
		}
		currentItem = pr1; 
	};


	/** sending the current file,title,id,state,timestamp to callback **/
	private function sendVars(stt:String,dur:Number,cpl:Boolean) {
		clearInterval(playSentInt);
		if(config['callback'] == "analytics") {
			var idx = feeder.feed[currentItem]["file"].lastIndexOf("/");
			var fil = feeder.feed[currentItem]["file"].substr(idx+1);
			if(stt == "start") {
				getURL("javascript:urchinTracker('/start_stream/"+fil+"');");
			} else if (stt == "stop" && cpl == true) {
				getURL("javascript:urchinTracker('/end_stream/"+fil+"');");
			}
		} else {
			varsObject.file = feeder.feed[currentItem]["file"];
			varsObject.title = feeder.feed[currentItem]["title"];
			varsObject.id = feeder.feed[currentItem]["id"];
			varsObject.state = stt;
			varsObject.duration = dur;
			varsObject.sendAndLoad(config["callback"],varsObject,"POST");
		}
	};


}