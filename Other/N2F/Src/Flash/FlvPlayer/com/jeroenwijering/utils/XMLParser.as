/**
* Parse XML file and return a simple, associative array.
*
* @author	Jeroen Wijering
* @version	1.0
**/


class com.jeroenwijering.utils.XMLParser {


	/** Flash XML object the file is loaded into. **/
	private var input:XML;
	/** The object the XML is parsed into **/
	private var output:Object; 
	/** The XML's HTTP status **/
	private var status:Number;


	/** Constructor, sets up XML object **/
	function XMLParser() {
	};


	/** Start parsing **/
	public function parse(lnk:String) {
		var ref = this;
		input = new XML();
		output = new Object();
		input.ignoreWhite = true;
		input.onHTTPStatus = function(stt) { 
			ref.status = stt;
		};
		input.onLoad = function(scs:Boolean) { 
			if(scs) {
				ref.processRoot();
			} else {
				ref.onParseError(ref.status);
			}
		};
		if(_root._url.indexOf("file://") > -1) {
			input.load(lnk); 
		} else if(lnk.indexOf('?') > -1) {
			input.load(lnk+'&'+random(999));
		} else { 
			input.load(lnk+'?'+random(999));
		}
	};


	/** Process the root XML node **/
	private function processRoot() {
		processNode(input.firstChild,output);
		delete input;
		onParseComplete(output);
	};


	/** Process a specific node **/
	private function processNode(nod:XMLNode,obj:Object) {
		obj['name'] = nod.nodeName;
		for(var att in nod.attributes) {
			obj[att] = nod.attributes[att];
		}
		if(nod.childNodes.length < 2 && nod.firstChild.nodeName == null) {
			obj['value'] = nod.firstChild.nodeValue;
		} else {
			var chn = nod.firstChild;
			var i = 0;
			while(chn != undefined) {
				obj[i] = new Object();
				processNode(chn,obj[i]);
				chn = chn.nextSibling;
				i++;
			}
		}
	};


	/** Invoked when parsing is completed. **/
	public function onParseComplete(obj:Object) {};


	/** Invoked when parsing is completed. **/
	public function onParseError(nbr:Number) {};


}