function GetRadUpload(_1){
return window[_1];
}
if(typeof (RadUploadNameSpace)=="undefined"){
RadUploadNameSpace={};
}
RadUploadNameSpace.RadUpload=function(_2){
this.Initialized=false;
RadControlsNamespace.EventMixin.Initialize(this);
RadControlsNamespace.DomEventsMixin.Initialize(this);
this.Id=_2[0];
this.UpdateFormProperties(document.getElementById(_2[1]));
this.Language=_2[2];
this.FocusOnLoad=_2[3];
this.Enabled=_2[4];
this.MaxFileCount=_2[5];
this.InitialFileInputsCount=_2[6];
this.EnableFileInputSkinning=_2[7];
if(RadControlsNamespace.Browser.IsSafari||(RadControlsNamespace.Browser.IsOpera&&!RadControlsNamespace.Browser.IsOpera9)){
this.EnableFileInputSkinning=false;
}
this.ReadOnlyFileInputs=_2[8];
this.AllowedFileExtensions=_2[9];
this.ShowCheckboxes=_2[10]&1;
this.ShowRemoveButtons=_2[10]&2;
this.ShowClearButtons=_2[10]&4;
this.OnClientAdded=_2[11];
this.OnClientAdding=_2[12];
this.OnClientDeleting=_2[13];
this.OnClientClearing=_2[14];
this.OnClientFileSelected=_2[15];
this.OnClientDeletingSelected=_2[16];
this.CurrentIndex=0;
this.ButtonArea=document.getElementById(this.Id+"ButtonArea");
this.ListContainer=document.getElementById(this.Id+"ListContainer");
if(!document.readyState||document.readyState=="complete"){
this.InnerConstructor();
}else{
this.AttachDomEvent(window,"load","InnerConstructor");
}
};
RadUploadNameSpace.RadUpload.prototype={InnerConstructor:function(_3){
this.DeleteXhtmlValiationRow();
this.AddButton=this.InitButton(document.getElementById(this.Id+"AddButton"),"Add","AddFileInput");
this.DeleteButton=this.InitButton(document.getElementById(this.Id+"DeleteButton"),"Delete","DeleteSelectedFileInputs");
this.FakeFileInputTemplate=this.CreateFakeInputTemplate();
var _4=this.MaxFileCount==0?this.InitialFileInputsCount:Math.min(this.InitialFileInputsCount,this.MaxFileCount);
for(var i=0;i<_4;i++){
this.AddFileInput();
}
this.SetAddDeleteButtonStates();
this.Initialized=true;
},AddFileInput:function(_6){
var _7=this.AddFileInputAt(this.ListContainer.rows.length);
if(this.Initialized){
try{
_7.focus();
}
catch(ex){
}
}
},AddFileInputAt:function(_8){
if(typeof (_8)=="undefined"||_8>this.ListContainer.rows.length){
_8=this.ListContainer.rows.length;
}
if(this.MaxFileCount>0&&_8>=this.MaxFileCount){
return;
}
if(this.Initialized){
var _9=this.RaiseEvent("OnClientAdding",new RadUploadNameSpace.RadUploadEventArgs(null));
if(_9==false){
return;
}
}
this.AddFileInputAtInternal(_8);
},AddFileInputAtInternal:function(_a){
var _b=this.ListContainer.insertRow(_a);
this.AttachDomEvent(_b,"click","RowClicked");
var _c;
if(this.ShowCheckboxes){
_c=_b.insertCell(_b.cells.length);
this.AppendCheckBox(_c);
}
_c=_b.insertCell(_b.cells.length);
this.AppendStyledFileInput(_c);
if(this.ShowClearButtons){
_c=_b.insertCell(_b.cells.length);
this.AppendClearButton(_c);
}
if(this.ShowRemoveButtons){
_c=_b.insertCell(_b.cells.length);
this.AppendRemoveButton(_c);
}
this.SetAddDeleteButtonStates();
this.RaiseEvent("OnClientAdded",{Row:_b});
this.CurrentIndex++;
return _b;
},AppendCheckBox:function(_d){
var _e=document.createElement("input");
_e.type="checkbox";
_e.id=_e.name=this.Id+"checkbox"+this.CurrentIndex;
_d.appendChild(_e);
_e.className="RadUploadFileSelector";
_e.disabled=!this.Enabled;
return _e;
},AppendClearButton:function(_f){
var _10=document.createElement("input");
_10.type="button";
_10.id=this.Id+"clear"+this.CurrentIndex;
_f.appendChild(_10);
this.InitButton(_10,"Clear");
_10.className="RadUploadClearButton";
_10.name="ClearInput";
_10.disabled=!this.Enabled;
return _10;
},AppendRemoveButton:function(_11){
var _12=document.createElement("input");
_12.type="button";
_12.id=this.Id+"remove"+this.CurrentIndex;
_11.appendChild(_12);
_12.value=RadUploadNameSpace.Localization[this.Language]["Remove"];
_12.className="RadUploadRemoveButton";
_12.name="RemoveRow";
_12.disabled=!this.Enabled;
return _12;
},AppendStyledFileInput:function(_13){
var _14=this.CreateFileInput();
this.AttachDomEvent(_14,"change","FileSelected");
if(this.EnableFileInputSkinning){
_14.className="RealFileInput";
var div=document.createElement("div");
_13.appendChild(div);
div.style.position="relative";
div.appendChild(this.FakeFileInputTemplate.cloneNode(true));
div.appendChild(_14);
if(!this.ReadOnlyFileInputs){
this.AttachDomEvent(_14,"keyup","SyncFileInputContent");
}else{
this.AttachDomEvent(_14,"keydown","CancelEvent");
}
return div;
}else{
_13.appendChild(_14);
_14.className="NoSkinnedFileUnput";
if(this.ReadOnlyFileInputs){
this.AttachDomEvent(_14,"keydown","CancelEvent");
}
return _14;
}
},CancelEvent:function(_16){
if(!_16){
_16=window.event;
}
if(!_16){
return false;
}
_16.returnValue=false;
_16.cancelBubble=true;
if(_16.stopPropagation){
_16.stopPropagation();
}
if(_16.preventDefault){
_16.preventDefault();
}
return false;
},ClearFileInputAt:function(_17){
var row=this.ListContainer.rows[_17];
if(row){
var _19=this.RaiseEvent("OnClientClearing",new RadUploadNameSpace.RadUploadEventArgs(this.GetFileInputFrom(row)));
if(_19==false){
return false;
}
this.DeleteFileInputAt(_17,true);
this.AddFileInputAtInternal(_17,true);
}
},CreateFakeInputTemplate:function(){
var _1a=document.createElement("div");
_1a.style.position="absolute";
_1a.style.top=0;
_1a.style.left=0;
_1a.style.zIndex=1;
var _1b=document.createElement("input");
_1b.type="text";
_1b.className="RadUploadInputField";
_1a.appendChild(_1b);
var _1c=document.createElement("input");
_1c.type="button";
_1a.appendChild(_1c);
this.InitButton(_1c,"Select");
_1c.disabled=!this.Enabled;
_1c.className="RadUploadSelectButton";
return _1a;
},CreateFileInput:function(){
var _1d=document.createElement("input");
_1d.type="file";
_1d.name=this.GetID("file");
_1d.id=this.GetID("file");
_1d.disabled=!this.Enabled;
return _1d;
},DeleteFileInputAt:function(_1e,_1f){
var row=this.ListContainer.rows[_1e];
if(row){
if(!_1f){
var _21=this.RaiseEvent("OnClientDeleting",new RadUploadNameSpace.RadUploadEventArgs(this.GetFileInputFrom(row)));
if(_21==false){
return false;
}
}
row.parentNode.removeChild(row);
this.SetAddDeleteButtonStates();
}
},DeleteSelectedFileInputs:function(_22){
var _23=[];
var _24=[];
for(var i=this.ListContainer.rows.length-1;i>=0;i--){
var _26=this.ListContainer.rows[i];
var _27=_26.cells[0].childNodes[0];
if(_27.checked){
_24[_24.length]=i;
_23[_23.length]=this.GetFileInputFrom(_26);
}
}
var _28=this.RaiseEvent("OnClientDeletingSelected",new RadUploadNameSpace.RadUploadDeleteSelectedEventArgs(_23));
if(_28==false){
return;
}
for(var i=0;i<_24.length;i++){
this.DeleteFileInputAt(_24[i],true);
}
},DeleteXhtmlValiationRow:function(){
var _29=this.ListContainer.rows[0];
_29.parentNode.removeChild(_29);
},FileSelected:function(e){
if(this.EnableFileInputSkinning){
this.SyncFileInputContent(e);
}
var _2b=e.srcElement?e.srcElement:e.target;
_2b.alt=_2b.title=_2b.value;
this.RaiseEvent("OnClientFileSelected",new RadUploadNameSpace.RadUploadEventArgs(_2b));
},GetFileInputFrom:function(row){
var _2d=row.getElementsByTagName("input");
for(var i=0;i<_2d.length;i++){
if(_2d[i].type=="file"){
return _2d[i];
}
}
return null;
},GetFileInputs:function(){
var _2f=[];
for(var i=0;i<this.ListContainer.rows.length;i++){
_2f[_2f.length]=this.GetFileInputFrom(this.ListContainer.rows[i]);
}
return _2f;
},GetID:function(_31){
return this.Id+_31+this.CurrentIndex;
},GetParentRow:function(_32){
if(_32){
if(_32.tagName.toLowerCase()=="tr"){
return _32;
}else{
return this.GetParentRow(_32.parentNode);
}
}
return null;
},InitButton:function(_33,_34,_35){
if(_33){
_33.value=RadUploadNameSpace.Localization[this.Language][_34];
if(this.Enabled){
if(_35){
this.AttachDomEvent(_33,"click",_35);
}
}else{
_33.disabled=true;
}
}
return _33;
},IsExtensionValid:function(_36){
if(_36==""){
return true;
}
for(var i=0;i<this.AllowedFileExtensions.length;i++){
var _38=this.AllowedFileExtensions[i].substring(1);
var _39=new RegExp("."+_38+"$","ig");
if(_36.match(_39)){
return true;
}
}
return false;
},RowClicked:function(e){
var _3b=e.srcElement?e.srcElement:e.target;
var _3c=this.GetParentRow(_3b);
if(_3b.name=="RemoveRow"){
this.DeleteFileInputAt(_3c.rowIndex);
}else{
if(_3b.name=="ClearInput"){
this.ClearFileInputAt(_3c.rowIndex);
}
}
},SetAddDeleteButtonStates:function(){
this.SetButtonState(this.DeleteButton,this.ListContainer.rows.length>0);
this.SetButtonState(this.AddButton,(this.MaxFileCount<=0)||(this.ListContainer.rows.length<this.MaxFileCount));
},SetButtonState:function(_3d,_3e){
if(_3d){
_3d.className=_3e?"RadUploadButton":"RadUploadButtonDisabled";
}
},SyncFileInputContent:function(e){
var _40=e.srcElement?e.srcElement:e.target;
var _41=_40.parentNode.childNodes[0].childNodes[0];
if(_40!==_41){
_41.value=_40.value;
_41.title=_40.value;
_41.disabled=true;
}
},UpdateFormProperties:function(_42){
if(!_42){
_42=document.forms[0];
}
_42.enctype=_42.encoding="multipart/form-data";
},ValidateExtensions:function(){
for(var i=0;i<this.ListContainer.rows.length;i++){
var _44=this.GetFileInputFrom(this.ListContainer.rows[i]).value;
if(!this.IsExtensionValid(_44)){
return false;
}
}
return true;
}};
RadUploadNameSpace.RadUpload.ApplySkin=function(_45){
};;if(typeof window.RadControlsNamespace=="undefined"){
window.RadControlsNamespace={};
}
if(typeof (window.RadControlsNamespace.Browser)=="undefined"||typeof (window.RadControlsNamespace.Browser.Version)==null||window.RadControlsNamespace.Browser.Version<1){
window.RadControlsNamespace.Browser={Version:1};
window.RadControlsNamespace.Browser.ParseBrowserInfo=function(){
this.IsMacIE=(navigator.appName=="Microsoft Internet Explorer")&&((navigator.userAgent.toLowerCase().indexOf("mac")!=-1)||(navigator.appVersion.toLowerCase().indexOf("mac")!=-1));
this.IsSafari=(navigator.userAgent.toLowerCase().indexOf("safari")!=-1);
this.IsSafari3=(this.IsSafari&&navigator.userAgent.toLowerCase().indexOf("ersion/3.")!=-1);
this.IsMozilla=window.netscape&&!window.opera;
this.IsFirefox=window.netscape&&!window.opera;
this.IsNetscape=/Netscape/.test(navigator.userAgent);
this.IsOpera=window.opera;
this.IsOpera9=window.opera&&(parseInt(window.opera.version())>8);
this.IsIE=!this.IsMacIE&&!this.IsMozilla&&!this.IsOpera&&!this.IsSafari;
this.IsIE7=/MSIE 7/.test(navigator.appVersion);
this.StandardsMode=this.IsSafari||this.IsOpera9||this.IsMozilla||document.compatMode=="CSS1Compat";
this.IsMac=/Mac/.test(navigator.userAgent);
};
RadControlsNamespace.Browser.ParseBrowserInfo();
};if(typeof window.RadControlsNamespace=="undefined"){
window.RadControlsNamespace={};
}
RadControlsNamespace.DomEventsMixin=function(){
};
RadControlsNamespace.DomEventsMixin.Initialize=function(_1){
_1.AttachDomEvent=this.AttachDomEvent;
_1.DetachDomEvent=this.DetachDomEvent;
_1.DisposeDomEvents=this.DisposeDomEvents;
_1.ClearEventPointers=this.ClearEventPointers;
_1.RegisterForAutomaticDisposal=this.RegisterForAutomaticDisposal;
_1.AutomaticDispose=this.AutomaticDispose;
_1.CreateEventHandler=this.CreateEventHandler;
_1.private_AttachDomEvent=this.private_AttachDomEvent;
_1.ClearEventPointers();
};
RadControlsNamespace.DomEventsMixin.CreateEventHandler=function(_2){
var _3=this;
return function(e){
if(!e){
e=window.event;
}
return _3[_2](e);
};
};
RadControlsNamespace.DomEventsMixin.AttachDomEvent=function(_5,_6,_7){
var _8=this.CreateEventHandler(_7);
this._eventPointers[this._eventPointers.length]=[_5,_6,_8];
this.private_AttachDomEvent(_5,_6,_8);
};
RadControlsNamespace.DomEventsMixin.private_AttachDomEvent=function(_9,_a,_b){
if(_9.attachEvent){
_9.attachEvent("on"+_a,_b);
}else{
if(_9.addEventListener){
_9.addEventListener(_a,_b,false);
}
}
};
RadControlsNamespace.DomEventsMixin.DetachDomEvent=function(_c,_d,_e){
if(_c.detachEvent){
_c.detachEvent("on"+_d,_e);
}
};
RadControlsNamespace.DomEventsMixin.DisposeDomEvents=function(){
for(var i=0;i<this._eventPointers.length;i++){
this.DetachDomEvent(this._eventPointers[i][0],this._eventPointers[i][1],this._eventPointers[i][2]);
}
this.ClearEventPointers();
};
RadControlsNamespace.DomEventsMixin.RegisterForAutomaticDisposal=function(_10){
var me=this;
var _12=this.CreateEventHandler(_10);
var _13=function(){
_12();
me.DisposeDomEvents();
me=null;
};
this.private_AttachDomEvent(window,"unload",_13);
};
RadControlsNamespace.DomEventsMixin.ClearEventPointers=function(){
this._eventPointers=[];
};;if(typeof window.RadControlsNamespace=="undefined"){
window.RadControlsNamespace={};
}
if(typeof (window.RadControlsNamespace.EventMixin)=="undefined"||typeof (window.RadControlsNamespace.EventMixin.Version)==null||window.RadControlsNamespace.EventMixin.Version<2){
RadControlsNamespace.EventMixin={Version:2,Initialize:function(_1){
_1._listeners={};
_1._eventsEnabled=true;
_1.AttachEvent=this.AttachEvent;
_1.DetachEvent=this.DetachEvent;
_1.RaiseEvent=this.RaiseEvent;
_1.EnableEvents=this.EnableEvents;
_1.DisableEvents=this.DisableEvents;
_1.DisposeEventHandlers=this.DisposeEventHandlers;
},DisableEvents:function(){
this._eventsEnabled=false;
},EnableEvents:function(){
this._eventsEnabled=true;
},AttachEvent:function(_2,_3){
if(!this._listeners[_2]){
this._listeners[_2]=[];
}
this._listeners[_2][this._listeners[_2].length]=(RadControlsNamespace.EventMixin.ResolveFunction(_3));
},DetachEvent:function(_4,_5){
var _6=this._listeners[_4];
if(!_6){
return false;
}
var _7=RadControlsNamespace.EventMixin.ResolveFunction(_5);
for(var i=0;i<_6.length;i++){
if(_7==_6[i]){
_6.splice(i,1);
return true;
}
}
return false;
},DisposeEventHandlers:function(){
for(var _9 in this._listeners){
var _a=null;
if(this._listeners.hasOwnProperty(_9)){
_a=this._listeners[_9];
for(var i=0;i<_a.length;i++){
_a[i]=null;
}
_a=null;
}
}
},ResolveFunction:function(_c){
if(typeof (_c)=="function"){
return _c;
}else{
if(typeof (window[_c])=="function"){
return window[_c];
}else{
return new Function("var Sender = arguments[0]; var Arguments = arguments[1];"+_c);
}
}
},RaiseEvent:function(_d,_e){
if(!this._eventsEnabled){
return true;
}
var _f=true;
if(this[_d]){
var _10=RadControlsNamespace.EventMixin.ResolveFunction(this[_d])(this,_e);
if(typeof (_10)=="undefined"){
_10=true;
}
_f=_f&&_10;
}
if(!this._listeners[_d]){
return _f;
}
for(var i=0;i<this._listeners[_d].length;i++){
var _12=this._listeners[_d][i];
var _10=_12(this,_e);
if(typeof (_10)=="undefined"){
_10=true;
}
_f=_f&&_10;
}
return _f;
}};
};if(typeof (RadUploadNameSpace)=="undefined"){
RadUploadNameSpace={};
}
if(typeof (RadUploadNameSpace.Localization)=="undefined"){
RadUploadNameSpace.Localization=[];
}
RadUploadNameSpace.Localization.ProcessRawArray=function(_1){
var _2=_1[0];
if(typeof (RadUploadNameSpace.Localization[_2])=="undefined"){
RadUploadNameSpace.Localization[_2]=[];
}
for(var i=1;i<_1.length;i+=2){
RadUploadNameSpace.Localization[_2][_1[i]]=_1[i+1];
}
};;if(typeof window.RadControlsNamespace=="undefined"){
window.RadControlsNamespace={};
}
if(typeof (window.RadControlsNamespace.Overlay)=="undefined"||typeof (window.RadControlsNamespace.Overlay.Version)==null||window.RadControlsNamespace.Overlay.Version<1.1){
window.RadControlsNamespace.Overlay=function(_1){
if(!this.SupportsOverlay()){
return;
}
this.Element=_1;
this.Shim=document.createElement("IFRAME");
this.Shim.src="javascript:'';";
this.Element.parentNode.insertBefore(this.Shim,this.Element);
if(_1.style.zIndex>0){
this.Shim.style.zIndex=_1.style.zIndex-1;
}
this.Shim.style.position="absolute";
this.Shim.style.border="0px";
this.Shim.frameBorder=0;
this.Shim.style.filter="progid:DXImageTransform.Microsoft.Alpha(style=0,opacity=0)";
this.Shim.disabled="disabled";
};
window.RadControlsNamespace.Overlay.Version=1.1;
RadControlsNamespace.Overlay.prototype.SupportsOverlay=function(){
return (RadControlsNamespace.Browser.IsIE);
};
RadControlsNamespace.Overlay.prototype.Update=function(){
if(!this.SupportsOverlay()){
return;
}
this.Shim.style.top=this.ToUnit(this.Element.style.top);
this.Shim.style.left=this.ToUnit(this.Element.style.left);
this.Shim.style.width=this.Element.offsetWidth+"px";
this.Shim.style.height=this.Element.offsetHeight+"px";
};
RadControlsNamespace.Overlay.prototype.ToUnit=function(_2){
if(!_2){
return "0px";
}
return parseInt(_2)+"px";
};
RadControlsNamespace.Overlay.prototype.Dispose=function(){
if(!this.SupportsOverlay()){
return;
}
if(this.Shim.parentNode){
this.Shim.parentNode.removeChild(this.Shim);
}
this.Element=null;
this.Shim=null;
};
};function GetRadProgressArea(_1){
return window[_1];
}
if(typeof (RadUploadNameSpace)=="undefined"){
RadUploadNameSpace={};
}
RadUploadNameSpace.ProgressAreaContainerName="Panel";
RadUploadNameSpace.RadProgressArea=function(_2){
this.Id=_2[0];
this.OnClientProgressUpdating=_2[1];
this.OnClientProgressBarUpdating=_2[2];
this.ProgressManagerFound=_2[3];
if(!this.ProgressManagerFound){
alert("Could not find an instance of RadProgressManager on the page. Are you missing the control declaration?");
}
RadControlsNamespace.EventMixin.Initialize(this);
RadControlsNamespace.DomEventsMixin.Initialize(this);
this.Element=document.getElementById(this.Id);
this.PrimaryProgressBarElement=this.FindElement("PrimaryProgressBar");
this.PrimaryTotalElement=this.FindElement("PrimaryTotal");
this.PrimaryValueElement=this.FindElement("PrimaryValue");
this.PrimaryPercentElement=this.FindElement("PrimaryPercent");
this.SecondaryProgressBarElement=this.FindElement("SecondaryProgressBar");
this.SecondaryTotalElement=this.FindElement("SecondaryTotal");
this.SecondaryValueElement=this.FindElement("SecondaryValue");
this.SecondaryPercentElement=this.FindElement("SecondaryPercent");
this.CurrentOperationElement=this.FindElement("CurrentOperation");
this.TimeElapsedElement=this.FindElement("TimeElapsed");
this.TimeEstimatedElement=this.FindElement("TimeEstimated");
this.SpeedElement=this.FindElement("Speed");
this.CancelButtonElement=this.FindElement("CancelButton");
this.CancelClicked=false;
if(this.CancelButtonElement){
this.AttachDomEvent(this.CancelButtonElement,"click","CancelRequest");
}
if(typeof (RadUploadNameSpace.ProgressAreas)=="undefined"){
RadUploadNameSpace.ProgressAreas=[];
}
this.RegisterForAutomaticDisposal("Hide");
RadUploadNameSpace.ProgressAreas[RadUploadNameSpace.ProgressAreas.length]=this;
};
RadUploadNameSpace.RadProgressArea.prototype={Update:function(_3){
if(this.RaiseEvent("OnClientProgressUpdating",{ProgressData:_3})==false){
return;
}
this.Show();
if(this.RaiseEvent("OnClientProgressBarUpdating",{ProgressValue:_3.PrimaryPercent,ProgressBarElementName:"PrimaryProgressBar",ProgressBarElement:this.PrimaryProgressBarElement})!=false){
this.UpdateHorizontalProgressBar(this.PrimaryProgressBarElement,_3.PrimaryPercent);
}
if(this.RaiseEvent("OnClientProgressBarUpdating",{ProgressValue:_3.SecondaryPercent,ProgressBarElementName:"SecondaryProgressBar",ProgressBarElement:this.SecondaryProgressBarElement})!=false){
this.UpdateHorizontalProgressBar(this.SecondaryProgressBarElement,_3.SecondaryPercent);
}
this.UpdateTextIndicator(this.PrimaryTotalElement,_3.PrimaryTotal);
this.UpdateTextIndicator(this.PrimaryValueElement,_3.PrimaryValue);
this.UpdateTextIndicator(this.PrimaryPercentElement,_3.PrimaryPercent);
this.UpdateTextIndicator(this.SecondaryTotalElement,_3.SecondaryTotal);
this.UpdateTextIndicator(this.SecondaryValueElement,_3.SecondaryValue);
this.UpdateTextIndicator(this.SecondaryPercentElement,_3.SecondaryPercent);
this.UpdateTextIndicator(this.CurrentOperationElement,_3.CurrentOperationText);
this.UpdateTextIndicator(this.TimeElapsedElement,_3.TimeElapsed);
this.UpdateTextIndicator(this.TimeEstimatedElement,_3.TimeEstimated);
this.UpdateTextIndicator(this.SpeedElement,_3.Speed);
},Show:function(){
this.Element.style.display="";
if(this.Element.style.position=="absolute"){
if(typeof (this.Overlay)=="undefined"){
this.Overlay=new RadControlsNamespace.Overlay(this.Element);
}
this.Overlay.Update();
}
},Hide:function(){
this.Element.style.display="none";
if(this.Overlay){
this.Overlay.Dispose();
this.Overlay=null;
}
},UpdateHorizontalProgressBar:function(_4,_5){
if(_4&&typeof (_5)!="undefined"){
_4.style.width=_5+"%";
}
},UpdateVerticalProgressBar:function(_6,_7){
if(_6&&typeof (_7)!="undefined"){
_6.style.height=_7+"%";
}
},UpdateTextIndicator:function(_8,_9){
if(_8&&typeof (_9)!="undefined"){
if(typeof (_8.value)=="string"){
_8.value=_9;
}else{
if(typeof (_8.innerHTML)=="string"){
_8.innerHTML=_9;
}
}
}
},CancelRequest:function(){
this.CancelClicked=true;
},FindElement:function(_a){
var _b=this.Id+"_"+RadUploadNameSpace.ProgressAreaContainerName+"_"+_a;
return document.getElementById(_b);
}};;function GetRadProgressManager(){
return window["RadProgressManager"];
}
if(typeof (RadUploadNameSpace)=="undefined"){
RadUploadNameSpace={};
}
RadUploadNameSpace.RadProgressManager=function(_1){
RadControlsNamespace.EventMixin.Initialize(this);
RadControlsNamespace.DomEventsMixin.Initialize(this);
this._refreshPeriod=Math.max(_1[0],50);
var _2=_1[1];
this.EnableMemoryOptimizationIdentifier=_1[2];
this.UniqueRequestIdentifier=_1[3];
this.PageGUID=_1[4];
this.OnClientProgressStarted=_1[5];
this.OnClientProgressUpdating=_1[6];
this.FormId=_1[7];
this.ShouldRegisterForSubmit=_1[8];
this.EnableMemoryOptimization=_1[9];
this.SuppressMissingHttpModuleError=_1[10];
this.OnClientSubmitting=_1[11];
this.TimeFormat="%HOURS%:%MINUTES%:%SECONDS%s";
this.InitializeForm();
this._callbackUrl=this.CreateCallbackUrl(_2);
this._waitingForResponse=false;
if(typeof (RadUploadNameSpace.ProgressAreas)=="undefined"){
RadUploadNameSpace.ProgressAreas=[];
}
};
RadUploadNameSpace.RadProgressManager.prototype={InitializeForm:function(){
var _3=document.getElementById(this.FormId);
if(!_3){
_3=document.forms[0];
}
this.UpdateFormAction(_3);
if(this.ShouldRegisterForSubmit==true){
this.RegisterForSubmit(_3);
}
},ClientSubmitHandler:function(_4){
if(this.RaiseEvent("OnClientSubmitting")==false){
this.CancelEvent(_4);
return false;
}
if(typeof (Page_IsValid)!="undefined"){
if(!Page_IsValid){
return;
}
}
this.StartProgressPolling();
},ResetCancelClicked:function(){
for(var i=0;i<RadUploadNameSpace.ProgressAreas.length;i++){
RadUploadNameSpace.ProgressAreas[i].CancelClicked=false;
}
this.InitializeForm();
},HideProgressAreas:function(){
for(var i=0;i<RadUploadNameSpace.ProgressAreas.length;i++){
RadUploadNameSpace.ProgressAreas[i].Hide();
}
},StartProgressPolling:function(){
this.InitSelectedFilesCount();
this.RaiseEvent("OnClientProgressStarted");
this._startTime=new Date();
this.MakeCallback();
},MakeCallback:function(){
if(!this._waitingForResponse){
this._waitingForResponse=true;
this.SendXmlHttpRequest();
}
},HandleCallback:function(){
if(this._xmlHttpRequest.readyState!=4){
return;
}
this._waitingForResponse=false;
if(this.ErrorOccured()){
return;
}
var _7=this._xmlHttpRequest.responseText;
if(_7){
try{
eval(_7);
}
catch(ex){
this.ShowInvalidContentMessage();
return;
}
if(rawProgressData){
if(this.EnableMemoryOptimization==true&&!this.SuppressMissingHttpModuleError&&rawProgressData.ProgressError){
alert(rawProgressData.ProgressError);
return;
}
if(rawProgressData.InProgress){
if(this._selectedFilesCount>0||rawProgressData.RadProgressContextCustomCounters){
this.ModifyProgressData(rawProgressData);
if(!this.UpdateProgressAreas(rawProgressData)){
this.HideProgressAreas();
this.ResetCancelClicked();
if(window.stop){
window.stop();
}else{
try{
document.execCommand("Stop");
}
catch(ex){
window.location.href=window.location.href;
}
}
return;
}
}
}
}
}
window.setTimeout(this.CreateEventHandler("MakeCallback"),this._refreshPeriod);
},ErrorOccured:function(){
if(!document.all){
return false;
}
if(this._xmlHttpRequest.status==404){
this.ShowNotFoundMessage();
}else{
if(this._xmlHttpRequest.status>0&&this._xmlHttpRequest.status!=200){
this.ShowGenericErrorMessage();
}else{
return false;
}
}
return true;
},ShowNotFoundMessage:function(){
alert("RadUpload Ajax callback error. Source url was not found: \n\r\n\r"+this._callbackUrl+"\n\r\n\rDid you register the RadUploadProgressHandler in web.config?"+"\r\n\r\nPlease, see the help for more details: RadUpload 2.x - Using RadUpload - Configuration - RadUploadProgressHandler.");
},ShowGenericErrorMessage:function(){
alert("RadUpload Ajax callback error. Source url returned error: "+this._xmlHttpRequest.status+" \n\r\n\r"+this._xmlHttpRequest.statusText+" \n\r\n\r"+this._callbackUrl+"\n\r\n\rDid you register the RadUploadProgressHandler in web.config?"+"\r\n\r\nPlease, see the help for more details: RadUpload 2.x - Using RadUpload - Configuration - RadUploadProgressHandler.");
},ShowInvalidContentMessage:function(){
alert("RadUpload Ajax callback error. Source url returned invalid content: \n\r\n\r"+this._xmlHttpRequest.responseText+"\n\r\n\r"+this._callbackUrl+"\n\r\n\rDid you register the RadUploadProgressHandler in web.config?"+"\r\n\r\nPlease, see the help for more details: RadUpload 2.x - Using RadUpload - Configuration - RadUploadProgressHandler.");
},UpdateProgressAreas:function(_8){
this.RaiseEvent("OnClientProgressUpdating",{ProgressData:_8});
for(var i=0;i<RadUploadNameSpace.ProgressAreas.length;i++){
var _a=RadUploadNameSpace.ProgressAreas[i];
if(_a.CancelClicked){
return false;
}
_a.Update(_8);
}
return true;
},ModifyProgressData:function(_b){
var _c=new Date()-this._startTime;
if(typeof (_b.TimeElapsed)=="undefined"){
_b.TimeElapsed=this.GetFormattedTime(this.ToSeconds(_c));
}
if(_b.RadUpload){
var _d=_b.RadUpload.RequestSize;
var _e=_b.RadUpload.Bytes;
if(typeof (_b.PrimaryTotal)=="undefined"){
_b.PrimaryTotal=this.FormatBytes(_d);
}
if(typeof (_b.PrimaryValue)=="undefined"){
_b.PrimaryValue=this.FormatBytes(_e);
}
if(typeof (_b.PrimaryPercent)=="undefined"){
_b.PrimaryPercent=Math.round(100*_e/_d);
}
if(typeof (_b.SecondaryTotal)=="undefined"){
_b.SecondaryTotal=this._selectedFilesCount;
}
if(typeof (_b.SecondaryValue)=="undefined"){
_b.SecondaryValue=_b.RadUpload.FilesCount;
}
if(typeof (_b.SecondaryPercent)=="undefined"){
_b.SecondaryPercent=Math.round(100*_b.RadUpload.FilesCount/(this._selectedFilesCount!=0?this._selectedFilesCount:1));
}
if(typeof (_b.CurrentOperationText)=="undefined"){
_b.CurrentOperationText=_b.RadUpload.CurrentFileName;
}
if(typeof (_b.Speed)=="undefined"){
if(this.ToSeconds(_c)==0){
_b.Speed=this.FormatBytes(0)+"/s";
}else{
_b.Speed=this.FormatBytes(_b.RadUpload.Bytes/this.ToSeconds(_c))+"/s";
}
}
}
if(typeof (_b.TimeEstimated)=="undefined"&&typeof (_b.PrimaryPercent)=="number"){
if(_b.PrimaryPercent==0){
_b.TimeEstimated=this.GetFormattedTime(this.ToSeconds(359999000));
}else{
_b.TimeEstimated=this.GetFormattedTime(this.ToSeconds(_c*(100/_b.PrimaryPercent-1)));
}
}
},ToSeconds:function(_f){
return Math.round(_f/1000);
},InitSelectedFilesCount:function(){
this._selectedFilesCount=0;
var _10=document.getElementsByTagName("input");
for(var i=0;i<_10.length;i++){
var _12=_10[i];
if(_12.type=="file"&&_12.value!=""){
this._selectedFilesCount++;
}
}
},CancelEvent:function(_13){
if(!_13){
_13=window.event;
}
if(!_13){
return false;
}
_13.returnValue=false;
_13.cancelBubble=true;
if(_13.stopPropagation){
_13.stopPropagation();
}
if(_13.preventDefault){
_13.preventDefault();
}
return false;
},SendXmlHttpRequest:function(){
if(typeof (XMLHttpRequest)!="undefined"){
this._xmlHttpRequest=new XMLHttpRequest();
}else{
if(typeof (ActiveXObject)!="undefined"){
this._xmlHttpRequest=new ActiveXObject("Microsoft.XMLHTTP");
}else{
return;
}
}
this._xmlHttpRequest.onreadystatechange=this.CreateEventHandler("HandleCallback");
this._xmlHttpRequest.open("GET",this.GetTimeStampedCallbackUrl(),true);
this._xmlHttpRequest.send("");
},CreateDelegate:function(_14,_15){
return function(){
_15.apply(_14,arguments);
};
},CreateCallbackUrl:function(_16){
var _17=_16.indexOf("?")<0?"?":"&";
return _16+_17+this.UniqueRequestIdentifier+"="+this.PageGUID;
},GetTimeStampedCallbackUrl:function(){
return this._callbackUrl+"&RadUploadTimeStamp="+new Date().getTime();
},RegisterForSubmit:function(_18){
this.RegisterForLinkButtons(_18);
this.RegisterForRegularButtons(_18);
},RegisterForLinkButtons:function(_19){
var _1a=this.CreateEventHandler("ClientSubmitHandler");
var _1b=_19.submit;
try{
_19.submit=function(){
if(_1a()==false){
return;
}
_19.submit=_1b;
_19.submit();
};
}
catch(exception){
try{
var _1c=__doPostBack;
__doPostBack=function(_1d,_1e){
var _1f=true;
if(typeof (Page_ClientValidate)=="function"){
_1f=Page_ClientValidate();
}
if(_1f){
if(_1a()==false){
return;
}
_1c(_1d,_1e);
}
};
}
catch(exception){
}
}
},RegisterForRegularButtons:function(_20){
this.AttachDomEvent(_20,"submit","ClientSubmitHandler");
},UpdateFormAction:function(_21){
if(typeof (_21.action)=="undefined"){
_21.action="";
}
if(_21.action.match(/\?/)){
_21.action=this.RemoveQueryStringParameter(_21.action,this.UniqueRequestIdentifier);
_21.action=this.RemoveQueryStringParameter(_21.action,this.EnableMemoryOptimizationIdentifier);
if(_21.action.substring(_21.action.length-1)!="?"){
_21.action+="&";
}
}else{
_21.action+="?";
}
_21.action+=this.UniqueRequestIdentifier+"="+this.PageGUID;
if(this.EnableMemoryOptimization){
_21.enctype=_21.encoding="multipart/form-data";
}else{
_21.action+="&"+this.EnableMemoryOptimizationIdentifier+"=false";
}
_21._initialAction=_21.action;
},RemoveQueryStringParameter:function(_22,_23){
var _24=new RegExp("&?"+_23+"=[^&]*");
if(_22.match(_24)){
return _22.replace(_24,"");
}
return _22;
},FormatBytes:function(_25){
var _26=_25/1024;
var _27=_26/1024;
if(_27>0.8){
return ""+Math.round(_27*100)/100+"MB";
}
if(_26>0.8){
return ""+Math.round(_26*100)/100+"kB";
}
return ""+_25+" bytes";
},GetFormattedTime:function(_28){
var _29=this.NormalizeTime(_28);
return this.TimeFormat.replace(/%HOURS%/,_29.Hours).replace(/%MINUTES%/,_29.Minutes).replace(/%SECONDS%/,_29.Seconds);
},NormalizeTime:function(_2a){
var _2b=_2a%60;
var _2c=Math.floor(_2a/60);
var _2d=_2c%60;
var _2e=Math.floor(_2c/60);
return {Hours:_2e,Minutes:_2d,Seconds:_2b};
}};;if(typeof (RadUploadNameSpace)=="undefined"){
RadUploadNameSpace={};
}
RadUploadNameSpace.UploadContainerName="Upload";
RadUploadNameSpace.SummaryContainerName="Summary";
RadUploadNameSpace.EmptyContainerName="Empty";
RadUploadNameSpace.RadTemplateUpload=function(_1){
this.Id=_1[0];
this.SummaryItems=_1[1];
RadControlsNamespace.EventMixin.Initialize(this);
RadControlsNamespace.DomEventsMixin.Initialize(this);
this.RegisterForAutomaticDisposal("Dispose");
this.SelectedFiles=[];
this.AddButtonElement=this.FindUploadElement("Add");
if(this.AddButtonElement){
this.AddButtonElement.onclick=this.CreateEventHandler("AddSelectedFile");
}
};
RadUploadNameSpace.RadTemplateUpload.prototype={AddSelectedFile:function(e){
var _3=document.getElementById(this.Id+"_"+RadUploadNameSpace.SummaryContainerName);
alert(_3.outerHTML);
var _4=_3.cloneNode(true);
alert(_4.outerHTML);
return false;
},Dispose:function(){
},SetSummary:function(){
var _5=document.getElementById(this.Id+"_"+RadUploadNameSpace.SummaryContainerName);
var _6=document.getElementById(this.Id+"_"+RadUploadNameSpace.EmptyContainerName);
_5.style.display=this.SelectedFiles.length>0?"block":"none";
_6.style.display=this.SelectedFiles.length>0?"none":"block";
},FindUploadElement:function(_7){
var _8=this.Id+"_"+RadUploadNameSpace.UploadContainerName+"_"+_7;
return document.getElementById(_8);
},FindSummaryElement:function(_9){
var _a=this.Id+"_"+RadUploadNameSpace.SummaryContainerName+"_"+_9;
return document.getElementById(_a);
}};;RadUploadNameSpace.RadUploadEventArgs=function(_1){
this.FileInputField=_1;
};
RadUploadNameSpace.RadUploadDeleteSelectedEventArgs=function(_2){
this.FileInputFields=_2;
};;if(typeof (RadUploadNameSpace)=="undefined"){
RadUploadNameSpace={};
}
RadUploadNameSpace.RadUploadSummaryItem=function(_1,_2){
this.ID=_1;
this.UploadTemplateFieldID=_2;
};
RadUploadNameSpace.RadUploadSummaryItem.prototype={};;if(typeof (window.RadControlsNamespace)=="undefined"){
window.RadControlsNamespace=new Object();
}
RadControlsNamespace.AppendStyleSheet=function(_1,_2,_3){
if(!_3){
return;
}
if(!_1){
document.write("<"+"link"+" rel='stylesheet' type='text/css' href='"+_3+"' />");
}else{
var _4=document.createElement("LINK");
_4.rel="stylesheet";
_4.type="text/css";
_4.href=_3;
document.getElementById(_2+"StyleSheetHolder").appendChild(_4);
}
};;//BEGIN_ATLAS_NOTIFY
if (typeof(Sys) != "undefined"){if (Sys.Application != null && Sys.Application.notifyScriptLoaded != null){Sys.Application.notifyScriptLoaded();}}
//END_ATLAS_NOTIFY
