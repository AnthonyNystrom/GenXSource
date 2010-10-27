/**
* 
*
* Use to handle loading & conversion of XML to Object
* 
*
* @author		James Rowley (biffer@biffcom.com)
* @version		0.1
*
*
* revision history:
* [0.1] Begin 18/1/06
* 
* 
* @toDo	
* 
* */

import mx.utils.Delegate;
import mx.events.EventDispatcher;
import com.rapierdev.data.XML2Object;


/**
* @class 	XMLManager
* @tooltip  Uses the XML2Object class to load XML & convert into object
* @author	James Rowley (biffer@biffcom.com)
* @version	0.1
*/



class com.rapierdev.data.XMLManager {
	
	var addEventListener:Function;
	var removeEventListener:Function;
	var dispatchEvent:Function
		
		
	private var globalXML:XML;
	private var globalObj:Object;
	private var inited:Boolean;
	
	private var xmlUrl:String;
	private var xmlInfo:Object;
	
	private static var instance:XMLManager;
	
	// \\\\\\\\\\\\\\\\\\\\\\\\\
	//
	// Constructor
	//
	// \\\\\\\\\\\\\\\\\\\\\\\\\\
	
	function XMLManager() {

		EventDispatcher.initialize(this);
		init()

	}
	
	// Create Singleton instance
	public static function getInstance():XMLManager {
		
		if (instance == null) {
			instance = new XMLManager();
			
		}
		
		return instance;
	}
		

	
	/**
	* @method toString
	* @tooltip return the type of object we are dealing with.
	*/
	public function toString():String 
	{
		return "\n***-[com.datacontrol.XMLManager]\n ";
	}
	
	/* ==================================================
					public functions
	===================================================== */
	
	
	  /**
    *
    * @method 		loadObjects
	 * @tooltip 	Loads XML
    * @param   	xmlurl (String) 		\\ Path to XML
    * @param   	xmlid (String)			\\ Identifier
    * @public
    */
	
	public function loadObjects(xmlurl:String,xmlinfo:Object):Void {
		

		xmlUrl = xmlurl
		xmlInfo= xmlinfo
		globalXML.load(xmlUrl);		
		
	}
	
	
	
 	/* ==================================================
						private functions
	===================================================== */
	
	
	
	/**
    *
    * @method 		init
	 * @tooltip 	Initialise
    * @private 
    */
	
	private function init():Void {
		
		
		globalXML = new XML();	
		globalXML.ignoreWhite = true;		
		globalXML.onLoad = Delegate.create(this,createGlobalObject);
		
		
	}
	
	
	
	/**
    *
    * @method 		createGlobalObject
	 * @tooltip 	Determines success of XML Load and fires events || callbacks 
    * @param   	success (Boolean) 			\\ XML Success
    */
	
	private function createGlobalObject(success):Void {
		
		if(success) {
			// Parse XML using XML2Object
			globalObj = new XML2Object().parseXML (globalXML);
			dispatchEvent({type:'onXMLLoad',target:this, xmlobj:globalObj,id:xmlInfo.id,event:xmlInfo.event})		
		} else {			
			dispatchEvent({type:'onXMLFail',target:this, xmlobj:globalObj,id:xmlInfo.id,status:globalXML.status})
		}
			
		
	}
	
	
	
	
	
	
}