/*
* 
XML2Object.as

Version History
alessandro(at)sephiroth dot it

================================================== */


/**
* @class XML2Object
* @tooltip Converts an XML file into an object
* @convert	alessandro(at)sephiroth dot it
* @version	1.0.0
*/

class com.rapierdev.data.XML2Object extends XML {
   
    private var oResult:Object;
    private var oXML:XML;
   
   
	/* ==================================================
						public functions
	===================================================== */
   
    /**
    *
    * @usage  
    * @param   xmlinput
    * @param   allArray if set to true will put all nodes into an array, even if a single childNode
    * @return Object representing the XML
    */
    public function parseXML(xmlinput:XML, allArray:Boolean):Object
    {
        if(allArray == undefined){
            allArray = false;
        }
        this.oResult = new Object();
        this.oXML      = xmlinput;
        this.oResult = this.translateXML(null,null,null,null,allArray);
        return this.oResult;
    }
	
	
   
    /**
    * Transform a Flash Object into An XML
    * @usage   new XMLObject().parseObject( object, 'root_name')
    * @param   obj object to be parsed
    * @param   name name of the first root element
    * @return 	xml
    */
    public function parseObject( obj:Object, name:String ):XML
    {
        var tempXML:XML = new XML('<?xml version="1.0" encoding="utf-8"?>');
        var rootNode:XMLNode = tempXML.createElement(name);
        var returnedNode = this.translateObject( obj, tempXML, rootNode)
        rootNode.appendChild(returnedNode);
        tempXML.appendChild(rootNode);
        return tempXML;
    }
	
	 /**
    *
    * @usage  return xml;
    * @return XML
    */
    public function get xml():XML{		

        return this.oXML
    }
	
	
   
   
   
   
   	/* ==================================================
						private functions
	===================================================== */
   
      /**
	* @method 	translateObject
	* @tooltip 	Translates Object into XML
	* @param 	obj (Object)
	* @param 	tempXML (XML)
	* @param		parentNode (XMLNode);
	* @private	
	*/

   
    private function translateObject( obj:Object, tempXML:XML, parentNode:XMLNode )
    {
        var node:XMLNode
        switch(obj.__proto__){
            case Array.prototype:
                var firstVal = obj.shift()
                this.translateObject(firstVal, tempXML, parentNode);
                for(var a in obj){
                    node = parentNode.cloneNode(false)
                    parentNode.parentNode.appendChild(node)
                    this.translateObject(obj[a], tempXML, node);
                }
                break
            case Object.prototype:
                for(var a in obj){
                    if(a == "attributes"){
                        this.parseAttributes(obj[a], parentNode)
                    } else {
                        node = tempXML.createElement(a)
                        parentNode.appendChild(node)
                    }
                    this.translateObject(obj[a], tempXML, node);
                }
                break
            case String.prototype:
            case Boolean.prototype:
            case Number.prototype:
            case Date.prototype:
            default:
                var textNode = tempXML.createTextNode( obj.toString() );
                parentNode.appendChild(textNode);
                break
        }
        return parentNode
    }
   
   
    /**
	* @method 	parseAttributes
	* @tooltip 	Parse Attributes
	* @param 	obj (Object)
	* @private	
	*/
    private function parseAttributes(obj:Object,parentNode:XMLNode){
        for(var a in obj){
            parentNode.attributes[a] = obj[a]
        }
    }
   
   /**
	* @method 	translateXML
	* @tooltip 	Translates XML to Object
	* @param 	from (String)
	* @param 	path (String)
	* @param		name (String)
	* @param		pos (String)
	* @private	
	*/
    private function translateXML (from, path, name, pos) {
		var app = this;
		var xmlName:String;
		var nodes, node, old_path;
		if (path == undefined) {
		path = this;
		name = "oResult";
		}
		path = path[name];
		if (from == undefined) {
		from = new XML (app.xml);
		from.ignoreWhite = true;
		}
		if (from.hasChildNodes ()) {
		nodes = from.childNodes;
		if (pos != undefined) {
		var old_path = path;
		path = path[pos];
		}
		while (nodes.length > 0) {
		node = nodes.shift ();
		xmlName = node.nodeName.split("-").join("_");
		if (xmlName) {
		var __obj__ = new Object ();
		__obj__.attributes = node.attributes;
		__obj__.data = node.firstChild.nodeValue;
		if (pos != undefined) {
		var old_path = path;
		}
		if (path[xmlName]) {
		path[xmlName].push (__obj__);
		name = node.nodeName;
		pos = path[xmlName].length - 1;
		} else {
		path[xmlName] = new Array ();
		path[xmlName].push (__obj__);
		name = xmlName;
		pos = path[xmlName].length - 1;
		}
		}
		if (node.hasChildNodes ()) {
		this.translateXML (node, path, name, pos);
		}
		}
		}
		return this.oResult;
		}

   
   
} 