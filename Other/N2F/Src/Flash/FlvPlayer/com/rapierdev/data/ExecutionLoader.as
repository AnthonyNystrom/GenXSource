


/**
 *
 * ExecutionLoader - loads the XML for required execution
 * 
 * 
 * @author J. Rowley
 * @version .1
 * 
 **/
 
 
import mx.events.EventDispatcher;
import mx.utils.Delegate;

import com.rapierdev.data.*
import com.rapierdev.model.*
import com.rapierdev.view.*



class com.rapierdev.data.ExecutionLoader   {
	
	var addEventListener						:Function;
	var removeEventListener					:Function;
	var dispatchEvent							:Function
	
	private static var instance			:ExecutionLoader;
	private static var EXEC_PATH			:String = "getExecutionListAsXMLByClientAndCampaign.php";
	
		
	private var _execXMLManager			:XMLManager;
	private var _dataManager				:DataManager;
	private var _curCampaignIndex			:Number


	private var loadRef;
	private var failRef;
	
	
	function ExecutionLoader(){
		
		EventDispatcher.initialize(this);
		
		addEventListener('loadData',Delegate.create(this,loadData));
		
		_dataManager = DataManager.getInstance();
		
	}
	
	/* 
	* 
	* Singleton
	*
	*/ 
	
	public static function getInstance():ExecutionLoader 
	{
		
		if (instance == null) {
			instance = new ExecutionLoader();			
		}
		
		return instance;
	}
	
	
	/* 
	*
	* loadData
	* 
	* @info 					loadData from XML for Execution
	* @param 				evt (Object)
	* 
	*/
	private function loadData(evt:Object):Void {
		
		_curCampaignIndex =  evt.info.id;
		

		loadRef = Delegate.create(this,onExecutionLoad);
		failRef = Delegate.create(this,onExecutionFail);
		
		_execXMLManager = new XMLManager();
		_execXMLManager.addEventListener('onXMLLoad',loadRef);
		_execXMLManager.addEventListener('onXMLFail',failRef);

		var $tmp = _dataManager.getCampaignModel().xml[0].campaigns[0].item[_curCampaignIndex]
		var $clientid = $tmp.attributes.clientid;
		var $campaignid = $tmp.attributes.campaignid;
		
		
		_execXMLManager.loadObjects(DataManager.getInstance().getBaseUrl()+EXEC_PATH+'?clientid='+$clientid+'&campaignid='+$campaignid+'&hidesub=1',{event:'onExecutionDataReady',id:'executionList'});
		
	//	_execXMLManager.loadObjects('xml/getExecutionList.xml',{event:'onExecutionDataReady',id:'executionList'});
		
		var selCampaign = CampaignView.getInstance().dataProvider[_curCampaignIndex].path

		selCampaign.loader._alpha = 0;
		selCampaign.loader._visible = true;
		selCampaign.loader.alphaTo(100,.2,'linear')
		
		
	}
	
	public function clearLoader() {
		
		
		_execXMLManager.removeEventListener('onXMLLoad',loadRef);
		_execXMLManager.removeEventListener('onXMLFail',failRef);
		CampaignView.getInstance().dataProvider[_curCampaignIndex].path.loader._visible = false;
		
	}
	
	
	public function toString() {
		
		return ('[com.rapierdev.data.ExecutionLoader]')
		
	}
	
	/* 
	*
	* onExecutionLoad
	* 
	* @info 					Create execution list
	* @param 				obj (Object)
	* 
	* 
	* 			
	*/
	private function onExecutionLoad(obj:Object):Void {
		
		CampaignView.getInstance().dataProvider[_curCampaignIndex].path.loader._visible = false;
		
		_dataManager.dispatchEvent({type:obj.event,ref:obj});
		dispatchEvent({type:'onExecutionReady',target:this,id:_curCampaignIndex})
	
	}
	
	/* 
	*
	* onExecutionFail
	* 
	* @info 					Throw error
	* @param 				obj (Object)
	* 
	*/
	private function onExecutionFail(obj:Object):Void {
		CampaignView.getInstance().dataProvider[_curCampaignIndex].path.loader._visible = false;
		
		dispatchEvent({type:'onExecutionFail',target:this});
		
	}
	

}