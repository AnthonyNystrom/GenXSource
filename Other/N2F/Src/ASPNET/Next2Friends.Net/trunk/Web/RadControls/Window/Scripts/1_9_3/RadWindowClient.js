if("undefined"==typeof (RadWindowNamespace)){
RadWindowNamespace=new Object();
}
Object.Extend=function(_1,_2){
for(var _3 in _2){
_1[_3]=_2[_3];
}
};
if(typeof (window.RadControlsNamespace)=="undefined"){
window.RadControlsNamespace=new Object();
}
RadControlsNamespace.AppendStyleSheet=function(_4,_5,_6){
if(!_6){
return;
}
if(!_4){
document.write("<"+"link"+" rel='stylesheet' type='text/css' href='"+_6+"' />");
}else{
var _7=document.createElement("LINK");
_7.rel="stylesheet";
_7.type="text/css";
_7.href=_6;
document.getElementById(_5+"StyleSheetHolder").appendChild(_7);
}
};
RadWindowNamespace.RadUtil_Trim=function(_8){
return _8.replace(/^\s{1,}/ig,"").replace(/\s{1,}$/ig,"");
};
RadWindowNamespace.RadUtil_Format=function(_9){
for(var i=1;i<arguments.length;i++){
_9=_9.replace(new RegExp("\\{"+(i-1)+"\\}","ig"),arguments[i]);
}
return _9;
};
RadWindowNamespace.RadUtil_EncodeContent=function(_b,_c){
var _d=new Array("%","<",">","!","\"","#","$","&","'","(",")",",",":",";","=","?","[","\\","]","^","`","{","|","}","~","+");
var _e=_b;
if(_c){
for(var i=0;i<_d.length;i++){
_e=_e.replace(new RegExp("\\x"+_d[i].charCodeAt(0).toString(16),"ig"),"%"+_d[i].charCodeAt(0).toString(16));
}
}else{
for(var i=_d.length-1;i>=0;i--){
_e=_e.replace(new RegExp("%"+_d[i].charCodeAt(0).toString(16),"ig"),_d[i]);
}
}
return _e;
};
RadWindowNamespace.RadUtil_PinnedList={};
RadWindowNamespace.RadUtil_SetPinned=function(_10,_11){
if(_10){
_11.TopOffset=parseInt(_11.style.top)-RadWindowNamespace.RadGetScrollTop(document);
_11.LeftOffset=parseInt(_11.style.left)-RadWindowNamespace.RadGetScrollLeft(document);
var _12=window.setInterval(function(){
RadWindowNamespace.RadUtil_UpdatePinnedElementPosition(_11);
},100);
RadWindowNamespace.RadUtil_PinnedList[_12]=_11;
}else{
var _13=null;
var _14=RadWindowNamespace.RadUtil_PinnedList;
for(var _15 in _14){
if(_14[_15]==_11){
_13=_15;
break;
}
}
if(null!=_13){
window.clearInterval(_13);
RadWindowNamespace.RadUtil_PinnedList[_13]=null;
}
_11.TopOffset=null;
_11.LeftOffset=null;
}
};
RadWindowNamespace.RadUtil_UpdatePinnedElementPosition=function(_16){
var _17=(_16.LeftOffset!=null)?_16.LeftOffset+RadWindowNamespace.RadGetScrollLeft(document):parseInt(_16.style.left);
var top=(_16.TopOffset!=null)?_16.TopOffset+RadWindowNamespace.RadGetScrollTop(document):parseInt(_16.style.top);
if(_16.MoveTo){
_16.MoveTo(_17,top);
}
};
RadWindowNamespace.RadUtil_SetOnTop=function(_19){
var _1a=GetRadWindowManager();
var _1b=_1a.GetNewZidex();
if(_19.Overlay&&_19.Overlay.style){
_19.Overlay.style.zIndex=_1b;
}
_19.style.zIndex=_1b;
};
RadWindowNamespace.RadUtil_EnableScrolling=function(_1c,_1d){
if(_1c){
document.body.style.overflow=_1d?_1d:"";
document.documentElement.style.overflow=_1d?_1d:"";
}else{
document.body.style.overflow="hidden";
document.documentElement.style.overflow="hidden";
}
};
RadWindowNamespace.RadUtil_GetBrowserInnerRect=function(_1e){
if(!_1e){
_1e=window;
}
var _1f=_1e.document;
var _20={};
if(document.all){
if(document.documentElement&&document.documentElement.clientHeight){
_20.width=_1f.documentElement.clientWidth;
_20.height=_1f.documentElement.clientHeight;
}else{
_20.width=_1f.body.clientWidth;
_20.height=_1f.body.clientHeight;
}
}else{
_20.width=window.innerWidth?parseInt(_1e.innerWidth):parseInt(_1f.body.clientWidth);
_20.height=window.innerHeight?parseInt(_1e.innerHeight):parseInt(_1f.body.clientHeight);
}
return _20;
};
RadWindowNamespace.RadUtil_GetBrowserRect=function(_21){
if(!_21){
_21=window;
}
var _22=_21.document;
var _23={};
if(_22.all&&"CSS1Compat"==_22.compatMode&&!_21.opera){
_23.width=_22.documentElement.clientWidth;
_23.height=_22.documentElement.clientHeight;
}else{
_23.width=window.innerWidth?parseInt(_21.innerWidth):parseInt(_22.body.clientWidth);
_23.height=window.innerHeight?parseInt(_21.innerHeight):parseInt(_22.body.clientHeight);
}
_23.top=RadWindowNamespace.RadGetScrollTop(_22);
_23.left=RadWindowNamespace.RadGetScrollLeft(_22);
return _23;
};
RadWindowNamespace.RadGetScrollTop=function(_24){
if(_24.documentElement&&_24.documentElement.scrollTop){
return _24.documentElement.scrollTop;
}else{
return _24.body.scrollTop;
}
};
RadWindowNamespace.RadGetScrollLeft=function(_25){
if(_25.documentElement&&_25.documentElement.scrollLeft){
return _25.documentElement.scrollLeft;
}else{
return _25.body.scrollLeft;
}
};
RadWindowNamespace.Box={GetOuterWidth:function(_26){
var _27=this.GetCurrentStyle(_26);
return _26.offsetWidth+this.GetHorizontalMarginValue(_27);
},GetOuterHeight:function(_28){
var _29=this.GetCurrentStyle(_28);
return _28.offsetHeight+this.GetVerticalMarginValue(_29);
},GetInnerWidth:function(_2a){
var _2b=this.GetCurrentStyle(_2a);
return _2a.offsetWidth-this.GetHorizontalPaddingAndBorderValue(_2b);
},GetInnerHeight:function(_2c){
var _2d=this.GetCurrentStyle(_2c);
return _2c.offsetHeight-this.GetVerticalPaddingAndBorderValue(_2d);
},SetOuterWidth:function(_2e,_2f){
var _30=this.GetCurrentStyle(_2e);
if(_30){
_2f-=this.GetHorizontalMarginValue(_30);
}
if(this.IsCompat()){
if(_30){
_2f-=this.GetHorizontalPaddingAndBorderValue(_30);
}
}
if(_2f<0){
_2e.style.width="auto";
}else{
_2e.style.width=_2f+"px";
}
},SetOuterHeight:function(_31,_32){
var _33=_32;
var _34=this.GetCurrentStyle(_31);
_32-=this.GetVerticalMarginValue(_34);
var _35=0;
if(this.IsCompat()){
_35=this.GetVerticalPaddingAndBorderValue(_34);
_32-=_35;
}
_31.style.height=_32+"px";
var _36=this.GetOuterHeight(_31);
if(_36!=0&&_36!=_33){
var _37=(_36-_33);
var _38=(_33-_37);
if(_38>0){
_31.style.height=(_38-_35)+"px";
}
}
},SafeParseInt:function(_39){
var _3a=parseInt(_39);
return isNaN(_3a)?0:_3a;
},GetStyleValues:function(_3b){
if(!_3b){
return 0;
}
var _3c=0;
for(var i=1;i<arguments.length;i++){
_3c+=this.SafeParseInt(_3b[arguments[i]]);
}
return _3c;
},GetHorizontalPaddingAndBorderValue:function(_3e){
return this.GetStyleValues(_3e,"borderLeftWidth","paddingLeft","paddingRight","borderRightWidth");
},GetVerticalPaddingAndBorderValue:function(_3f){
return this.GetStyleValues(_3f,"borderTopWidth","paddingTop","paddingBottom","borderBottomWidth");
},GetHorizontalMarginValue:function(_40){
return this.GetStyleValues(_40,"marginLeft","marginRight");
},GetVerticalMarginValue:function(_41){
return this.GetStyleValues(_41,"marginTop","marginBottom");
},GetCurrentStyle:function(_42){
if(_42.currentStyle){
return _42.currentStyle;
}else{
if(document.defaultView&&document.defaultView.getComputedStyle){
return document.defaultView.getComputedStyle(_42,null);
}else{
return null;
}
}
},IsCompat:function(){
return true;
}};
RadWindowNamespace.RadGetElementRect=function(_43){
if(!_43){
_43=this;
}
var _44=0;
var top=0;
var _46=RadWindowNamespace.Box.GetOuterWidth(_43);
var _47=RadWindowNamespace.Box.GetOuterHeight(_43);
while(_43.offsetParent){
_44+=_43.offsetLeft;
top+=_43.offsetTop;
_43=_43.offsetParent;
}
if(_43.x){
_44=_43.x;
}
if(_43.y){
top=_43.y;
}
var _48={};
_48.left=RadWindowNamespace.RadUtil_GetIntValue(_44,0);
_48.top=RadWindowNamespace.RadUtil_GetIntValue(top,0);
_48.width=RadWindowNamespace.RadUtil_GetIntValue(_46,0);
_48.height=RadWindowNamespace.RadUtil_GetIntValue(_47,0);
return _48;
};
RadWindowNamespace.RadUtil_DetectBrowser=function(_49){
_49=_49.toLowerCase();
if("ie"==_49){
_49="msie";
}else{
if("mozilla"==_49||"firefox"==_49){
_49="compatible";
}
}
var _4a=navigator.userAgent.toLowerCase();
place=_4a.indexOf(_49)+1;
if(place){
return true;
}else{
return false;
}
};
RadWindowNamespace.RadUtil_IsDocumentLoaded=function(_4b){
return (null!=document.readyState&&"complete"!=document.readyState)?false:true;
};
RadWindowNamespace.RadUtil_GetIntValue=function(_4c,_4d){
if(!_4d){
_4d=0;
}
var _4e=parseInt(_4c);
return (isNaN(_4e)?_4d:_4e);
};
RadWindowNamespace.RadUtil_GetEventSource=function(e){
if(null==e){
return null;
}
return (e.srcElement?e.srcElement:e.target);
};
RadWindowNamespace.RadUtil_CancelEvent=function(_50){
if(!_50){
return;
}
_50.returnValue=false;
_50.cancelBubble=true;
if(_50.stopPropagation){
_50.stopPropagation();
}
if(_50.preventDefault){
_50.preventDefault();
}
return false;
};
RadWindowNamespace.RadUtil_AttachEventEx=function(_51,_52,_53){
_52=RadWindowNamespace.RadUtil_FixEventName(_52);
if(_51.addEventListener){
_51.addEventListener(_52,_53,false);
}else{
if(_51.attachEvent){
_51.attachEvent(_52,_53);
}
}
};
RadWindowNamespace.RadUtil_DetachEventEx=function(_54,_55,_56){
_55=RadWindowNamespace.RadUtil_FixEventName(_55);
if(_54.addEventListener){
_54.removeEventListener(_55,_56,false);
}else{
if(_54.detachEvent){
_54.detachEvent(_55,_56);
}
}
};
RadWindowNamespace.RadUtil_StartsWith=function(_57,_58){
if(typeof (_58)!="string"){
return false;
}
return (0==_57.indexOf(_58));
};
RadWindowNamespace.RadUtil_FixEventName=function(_59){
_59=_59.toLowerCase();
if(document.addEventListener){
if(RadWindowNamespace.RadUtil_StartsWith(_59,"on")){
return _59.substr(2);
}else{
return _59;
}
}else{
if(document.attachEvent&&!RadWindowNamespace.RadUtil_StartsWith(_59,"on")){
return "on"+_59;
}else{
return _59;
}
}
};;;RadWindowNamespace.CurrentDragTarget=null;
RadWindowNamespace.MakeMoveable=function(_1,_2,_3,_4,_5){
if(!_1||_1.Move){
return;
}
Object.Extend(_1,RadWindowNamespace.RadMoveableObject);
_1.AllowMove=(_5)?true:false;
if(_4!=false){
Object.Extend(_1,RadWindowNamespace.ResizableObject);
_1.InitResize();
}
_1.onmouseout=function(e){
if(""!=this.style.cursor){
this.style.cursor="";
}
};
_1.onmousedown=function(e){
if(!this.MoveEnabled){
return;
}
if(!e){
e=window.event;
}
if(this.SetOnTop){
this.SetOnTop();
}
if(!this.DragMode){
if(this.AllowResize&&this.ResizeDir){
this.DragMode=2;
this.StartDrag(e);
}else{
if(this.AllowMove&&this.GripHitTest(e)){
this.DragMode=1;
}
}
}
return RadWindowNamespace.RadUtil_CancelEvent(e);
};
_1.onmouseup=function(e){
this.DragMode="";
RadWindowNamespace.HideOverlayImage();
if(this.OnMouseUp){
this.OnMouseUp(e);
}
};
_1.onmousemove=function(e){
if(!e){
e=window.event;
}
if(this.DragMode){
this.StartDrag(e);
return;
}
if(this.IsMoving()){
return;
}
if(!this.IsResizing()&&null!=this.CalcResizeDir){
this.ResizeDir=this.CalcResizeDir(e);
this.style.cursor=this.ResizeDir;
}
};
if(_3!=false&&document.all&&!window.opera){
RadWindowNamespace.EnableOverlayIframe(_1);
}
};
RadWindowNamespace.RadMoveableObject={OnDragStart:null,OnDragEnd:null,OnMouseUp:null,OnResize:null,OnShow:null,OnHide:null,AllowMove:true,AllowResize:true,UseDragHelper:true,DragMode:null,MoveEnabled:true,EnableMove:function(_a){
this.MoveEnabled=_a;
},StartDrag:function(_b){
if(this.DragStarted){
return;
}
this.MouseX=_b.clientX;
this.MouseY=_b.clientY;
RadWindowNamespace.RadUtil_AttachEventEx(document,"onmouseup",RadWindowNamespace.GeneralMouseUp);
RadWindowNamespace.RadUtil_AttachEventEx(document,"onmousemove",RadWindowNamespace.GeneralMouseMove);
RadWindowNamespace.CurrentDragTarget=this;
if(this.UseDragHelper){
this.DragHelper=RadWindowNamespace.GetDragHelper();
this.DragHelper.Show(this.GetRect());
}
if(this.OnDragStart){
this.OnDragStart(_b);
}
this.ActualZindex=this.style.zIndex;
this.style.zIndex=50000;
RadWindowNamespace.ShowOverlayImage(this);
this.DragHelper.style.zIndex=this.style.zIndex+1;
this.DragStarted=true;
},EndDrag:function(_c){
if(this.DragHelper){
var rc=this.DragHelper.GetRect();
if(1==this.DragMode){
this.MoveTo(rc.left,rc.top);
}else{
if(2==this.DragMode){
this.SetSize(rc.width,rc.height);
this.MoveTo(rc.left,rc.top);
}
}
}
this.CancelDrag(_c);
if(this.OnDragEnd){
this.OnDragEnd(_c);
}
},CancelDrag:function(_e){
RadWindowNamespace.CurrentDragTarget=null;
this.DragStarted=false;
RadWindowNamespace.HideOverlayImage();
var _f=document;
RadWindowNamespace.RadUtil_DetachEventEx(_f,"onmouseup",RadWindowNamespace.GeneralMouseUp);
RadWindowNamespace.RadUtil_DetachEventEx(_f,"onmousemove",RadWindowNamespace.GeneralMouseMove);
if(this.DragHelper){
this.DragHelper.Hide();
this.DragHelper=null;
}
this.DragMode="";
this.style.zIndex=this.ActualZindex;
this.Show();
},DoDrag:function(e){
if(1==this.DragMode){
this.Move(e);
}else{
if(2==this.DragMode){
this.Resize(e);
}
}
this.MouseX=e.clientX;
this.MouseY=e.clientY;
},GripHitTest:function(_11){
var _12=RadWindowNamespace.RadUtil_GetEventSource(_11);
try{
while(_12&&null!=_12.getAttribute){
if(null!=_12.getAttribute("grip")){
return _12;
}else{
_12=_12.parentNode;
}
}
}
catch(e){
}
return null;
},Move:function(_13){
var dX=_13.clientX-this.MouseX;
var dY=_13.clientY-this.MouseY;
this.DragHelper.MoveBy(dX,dY);
},MoveBy:function(dX,dY){
if(!this.Left){
this.Left=parseInt(this.style.left);
}
if(!this.Top){
this.Top=parseInt(this.style.top);
}
this.MoveTo(this.Left+dX,this.Top+dY);
},MoveTo:function(x,y){
if(isNaN(x)||isNaN(y)){
return;
}
this.Left=x;
this.Top=y;
this.style.position="absolute";
this.style.left=this.Left+"px";
this.style.top=this.Top+"px";
if(this.NeedOverlay){
this.SetOverlay();
this.NeedOverlay=false;
}
if(this.Overlay){
this.Overlay.style.top=this.style.top;
this.Overlay.style.left=this.style.left;
}
},SetSize:function(_1a,_1b,_1c){
_1a=parseInt(_1a);
_1b=parseInt(_1b);
if(_1a<5||_1b<5){
return;
}
if(!isNaN(_1a)&&_1a>=0){
RadWindowNamespace.Box.SetOuterWidth(this,_1a);
if(this.Overlay){
RadWindowNamespace.Box.SetOuterWidth(this.Overlay,_1a);
}
}
if(!isNaN(_1b)&&_1b>=0){
RadWindowNamespace.Box.SetOuterHeight(this,_1b);
if(this.Overlay){
this.Overlay.style.height=_1b+"px";
}
}
if((false!=_1c)&&this.OnResize&&"function"==typeof (this.OnResize)){
this.OnResize();
}
},GetRect:function(){
if(this==RadWindowNamespace.CurrentDragTarget&&this.DragHelper&&this.DragHelper.IsVisible()){
return RadWindowNamespace.RadGetElementRect(this.DragHelper);
}else{
return RadWindowNamespace.RadGetElementRect(this);
}
},SetPosition:function(_1d){
if(_1d){
this.MoveTo(_1d.left,_1d.top);
this.SetSize(_1d.width,_1d.height);
}
},SetOnTop:function(){
var _1e=0;
var _1f=0;
var _20=this.parentNode.childNodes;
var _21;
for(var i=0;i<_20.length;i++){
_21=_20[i];
if(1!=_21.nodeType){
continue;
}
_1f=parseInt(_21.style.zIndex);
if(_1f>_1e){
_1e=_1f;
}
}
this.style.zIndex=_1e+1;
},Show:function(_23){
this.style.display=this.OldDisplayMode?this.OldDisplayMode:"";
if(null!=_23){
this.SetPosition(_23);
}
this.SetOnTop();
if(this.ShowOverlay){
this.ShowOverlay();
}
if(this.OnShow){
this.OnShow();
}
},Hide:function(){
if(!this.IsVisible()){
return;
}
this.OldDisplayMode=this.style.display;
this.style.display="none";
if(this.HideOverlay){
this.HideOverlay();
}
if(this.OnHide){
this.OnHide();
}
},IsVisible:function(){
return (this.style.display!="none");
},IsResizing:function(){
return (2==this.DragMode);
},IsMoving:function(){
return (1==this.DragMode);
},DisableMove:function(){
this.CalcResizeDir=null;
this.Resize=null;
this.Inflate=null;
this.InitResize=null;
this.SetResizeDirs=null;
this.onmousemove=null;
this.onmouseup=null;
this.onmouseout=null;
this.onmousedown=null;
this.StartDrag=null;
this.EndDrag=null;
this.CancelDrag=null;
this.DoDrag=null;
this.GripHitTest=null;
this.Move=null;
this.MoveBy=null;
this.MoveTo=null;
this.SetOnTop=null;
this.GetRect=null;
this.SetPosition=null;
this.SetOverlay=null;
this.ShowOverlay=null;
this.HideOverlay=null;
this.IsOverlayVisible=null;
this.Show=null;
this.Hide=null;
this.IsVisible=null;
this.IsResizing=null;
this.IsMoving=null;
this.DragHelper=null;
this.OnDragStart=null;
this.OnDragEnd=null;
this.OnMouseUp=null;
this.OnResize=null;
this.OnShow=null;
this.OnHide=null;
this.DragMode=null;
this.GlobalDragHelper=null;
this.Overlay=null;
this.GeneralMouseUp=null;
this.GeneralMouseMove=null;
}};
RadWindowNamespace.GeneralMouseUp=function(e){
if(!RadWindowNamespace.CurrentDragTarget){
return;
}
if(!e){
e=window.event;
}
var _25=RadWindowNamespace.CurrentDragTarget;
_25.EndDrag(e);
_25.DragMode="";
};
RadWindowNamespace.MoveCounter=0;
RadWindowNamespace.GeneralMouseMove=function(e){
var _27=RadWindowNamespace.CurrentDragTarget;
if(!_27){
return;
}
if(RadWindowNamespace.MoveCounter++%2){
_27.DoDrag(e);
}
RadWindowNamespace.RadUtil_CancelEvent(e);
};
RadWindowNamespace.GlobalDragHelper=null;
RadWindowNamespace.GetDragHelper=function(){
if(this.GlobalDragHelper){
return this.GlobalDragHelper;
}
var _28=document.createElement("DIV");
document.body.appendChild(_28);
_28.style.position="absolute";
_28.style.top=10;
_28.style.left=10;
_28.style.width=100;
_28.style.height=100;
_28.style.display="none";
_28.className="RadWDragHelper";
RadWindowNamespace.MakeMoveable(_28,false,false,true);
this.GlobalDragHelper=_28;
return _28;
};
RadWindowNamespace.EnableOverlayIframe=function(obj){
obj.SetOverlay=function(){
var frm=document.createElement("IFRAME");
frm=frm.cloneNode(true);
frm.src="javascript:'';";
frm.frameBorder=0;
frm.scrolling="no";
frm.style.filter="progid:DXImageTransform.Microsoft.Alpha(opacity=0)";
var _2b=frm.style;
_2b.overflow="hidden";
this.Overlay=frm;
};
obj.ShowOverlay=function(){
this.parentNode.insertBefore(this.Overlay,this);
var _2c=this.Overlay.style;
_2c.display="inline";
_2c.position="absolute";
var _2d=this.GetRect();
_2c.width=_2d.width+"px";
_2c.height=_2d.height+"px";
_2c.left=_2d.left+"px";
_2c.top=_2d.top+"px";
};
obj.HideOverlay=function(){
this.Overlay.style.display="none";
};
if("complete"==document.readyState){
obj.SetOverlay();
}else{
obj.NeedOverlay=true;
}
};
RadWindowNamespace.GetOverlayImage=function(){
if(!this.OverlayImage){
var img=document.createElement("IMG");
if(document.all){
var _2f=new Function("return false");
img.setAttribute("unselectable","on");
img.setAttribute("galleryimg","no");
img.onselectstart=_2f;
img.ondragstart=_2f;
img.onmouseover=_2f;
img.onmousemove=_2f;
}
img.onmouseup=RadWindowNamespace.HideOverlayImage;
var _30=img.style;
_30.display="none";
_30.position="absolute";
_30.left=_30.top="0px";
if(null!=document.readyState&&"complete"!=document.readyState){
RadWindowNamespace.RadUtil_AttachEventEx(window,"load",function(){
document.body.appendChild(img);
});
}else{
document.body.appendChild(img);
}
this.OverlayImage=img;
}
return this.OverlayImage;
};
RadWindowNamespace.ShowOverlayImage=function(_31){
var _32=RadWindowNamespace.GetOverlayImage();
if(_32){
var _33=RadWindowNamespace.RadUtil_GetBrowserInnerRect();
var _34=_32.style;
_34.display="";
_34.width=parseInt(_33.width)+"px";
_34.height=parseInt(_33.height)+"px";
_34.top=RadWindowNamespace.RadGetScrollTop(_32.ownerDocument);
_34.left=RadWindowNamespace.RadGetScrollLeft(_32.ownerDocument);
if(_31&&_31.style.zIndex){
var _35=_31.style.zIndex;
_34.zIndex=_35;
_31.style.zIndex=_35++;
}
}
};
RadWindowNamespace.HideOverlayImage=function(e){
var _37=RadWindowNamespace.GetOverlayImage();
if(_37){
_37.style.display="none";
}
RadWindowNamespace.GeneralMouseUp(e);
};;RadWindowMinimizeMode={SameLocation:1,MinimizeZone:2,Default:1};
RadWindowBehavior={None:0,Resize:1,Minimize:2,Close:4,Pin:8,Maximize:16,Move:32,Reload:64,Default:(1+2+4+8+16+32+64)};
function RadWindowInitialize(id,_2,_3,_4,_5,_6,_7,_8,_9,_a,_b,_c,_d,_e,_f,_10,_11,_12,_13,_14,_15,_16,_17,_18,_19,_1a,_1b,_1c,_1d){
var _1e=GetRadWindowManager();
if(_4){
_1e.CreateSplash(_b,_c);
return;
}
var _1f=_1e.CreateWindowObject(id);
_1f._events=[];
_1f.Name=_2;
_1f.JsName=_3;
if(_6){
_1f["OnClientShow"]=_6;
}
if(_7){
_1f["OnClientClose"]=_7;
}
if(_8){
_1f["OnClientPageLoad"]=_8;
}
if(_1d){
_1f.ClientCallBackFunction=_1d;
}
if(_9){
_1f._iconUrl=_9;
}
if(_a){
_1f._minimizeIconUrl=_a;
}
if(_5){
_1f._url=_5;
}
if(_b){
_1f.Width=_b;
}
if(_c){
_1f.Height=_c;
}
if(_d){
_1f.private_SetSizeValue("Left",_d,false);
}
if(_e){
_1f.private_SetSizeValue("Top",_e,false);
}
if(_f){
_1f._title=_f;
}
if(_10){
_1f._minimizeZoneId=_10;
}
if(_11!=RadWindowBehavior.None){
_1f._initialBehavior=_11;
}
if(_12!=RadWindowBehavior.Default){
_1f._behavior=_12;
}
if(_13!=RadWindowMinimizeMode.Default){
_1f._minimizeMode=_13;
}
if(_18!=RadWindowClass.prototype._offsetElementId){
_1f._offsetElementId=_18;
}
if(_19!=RadWindowClass.prototype._openerElementId){
_1f._openerElementId=_19;
}
if(_1f._openerElementId){
var _20=function(){
_1f.SetOpenerElementId(_1f._openerElementId);
};
var _21=document.getElementById(_1f._openerElementId);
if(!_21){
RadWindowNamespace.RadUtil_AttachEventEx(window,"load",_20);
}else{
_20();
}
}
if(_15!=null){
_1f._visibleStatusbar=_15;
}
if(_16!=null){
_1f._visibleTitlebar=_16;
}
if(_17!=null){
_1f._visibleOnPageLoad=_17;
}
if(_1a!=null){
_1f._destroyOnClose=_1a;
}
if(_1b!=null){
_1f._reloadOnShow=_1b;
}
if(_1c!=null){
_1f._showContentDuringLoad=_1c;
}
if(_14!=null){
_1f._modal=_14;
}
if(_1f._visibleOnPageLoad){
_1f.Show();
}
return _1f;
}
function RadWindowClass(id){
this.IsIE=RadWindowNamespace.RadUtil_DetectBrowser("ie");
this.Id=id;
this.JsName=id;
this.Name="";
this.BrowserWindow=window;
}
RadWindowClass.prototype.ClientCallBackFunction=null;
RadWindowClass.prototype.BrowserWindow=null;
RadWindowClass.prototype.Width=300;
RadWindowClass.prototype.Height=300;
RadWindowClass.prototype.Left=null;
RadWindowClass.prototype.Top=null;
RadWindowClass.prototype._title="";
RadWindowClass.prototype._url=null;
RadWindowClass.prototype._minimizeZoneId="";
RadWindowClass.prototype._offsetElementId="";
RadWindowClass.prototype._openerElementId="";
RadWindowClass.prototype._iconUrl="";
RadWindowClass.prototype._minimizeIconUrl="";
RadWindowClass.prototype._language="en_US";
RadWindowClass.prototype._localization={};
RadWindowClass.prototype._schemeBasePath=null;
RadWindowClass.prototype._behavior=RadWindowBehavior.Default;
RadWindowClass.prototype._initialBehavior=RadWindowBehavior.None;
RadWindowClass.prototype._minimizeMode=RadWindowMinimizeMode.Default;
RadWindowClass.prototype._maximized=false;
RadWindowClass.prototype._minimized=false;
RadWindowClass.prototype._pinned=false;
RadWindowClass.prototype._closed=false;
RadWindowClass.prototype._modal=false;
RadWindowClass.prototype._loaded=false;
RadWindowClass.prototype._visibleStatusbar=true;
RadWindowClass.prototype._visibleTitlebar=true;
RadWindowClass.prototype._visibleOnPageLoad=false;
RadWindowClass.prototype._created=false;
RadWindowClass.prototype._destroyOnClose=false;
RadWindowClass.prototype._reloadOnShow=false;
RadWindowClass.prototype._events=null;
RadWindowClass.prototype._showContentDuringLoad=false;
RadWindowClass.prototype._iframe=null;
RadWindowClass.prototype.WrapperElement=null;
RadWindowClass.prototype.TitleElement=null;
RadWindowClass.prototype.HeaderRowElement=null;
RadWindowClass.prototype.LoadingWrapper=null;
RadWindowClass.prototype.MinimizedWindow=null;
RadWindowClass.prototype.ClassicWindow=null;
RadWindowClass.prototype.SetOffsetElementId=function(id){
this._offsetElementId=id;
};
RadWindowClass.prototype.SetOpenerElementId=function(id){
var _25=this;
_25._openerElementId=id;
var _26=document.getElementById(_25._openerElementId);
if(_26){
var _27=_26.onclick;
_26.onclick=function(e){
if(_27){
_27();
}
_25.Show();
if(e){
RadWindowNamespace.RadUtil_CancelEvent(e);
}
return false;
};
}else{
alert(_25.Id+" (OpenerElementId)- Could not find element on page with id "+_25._openerElementId);
}
};
RadWindowClass.prototype.ExecuteServerClientEvent=function(_29){
var _2a=this[_29];
if(!_2a){
return;
}
if(typeof (_2a)=="string"){
_2a=eval(_2a);
}
if(typeof (_2a)!="function"){
return;
}
try{
return _2a(this);
}
catch(e){
alert("Exception while executing client event "+_29+" Error:"+e.message);
}
return true;
};
RadWindowClass.prototype.AutoResize=function(){
try{
var frm=this._iframe;
var _2c=function(){
var _2d=frm.clientHeight;
var _2e=frm.contentWindow.document.body.scrollHeight;
if(_2e>_2d){
frm.style.height=parseInt(_2e)+"px";
}
if(!document.all){
frm.style.marginTop="-3px";
frm.style.marginBottom="-3px";
}
};
_2c();
}
catch(e){
}
};
RadWindowClass.prototype.Dispose=function(){
try{
this.Close();
var _2f=document.getElementById(this._openerElementId);
if(_2f){
_2f.onclick=null;
}
if(this.WrapperElement){
if(this.WrapperElement.DisableMove){
this.WrapperElement.DisableMove();
}
this.WrapperElement=null;
}
if(this.HeaderRowElement){
this.HeaderRowElement.ondblclick=null;
}
this.HeaderRowElement=null;
this.TitleElement=null;
this.StatusElement=null;
if(this.IframeDocumentClickHandler&&this.IframeDocument){
RadWindowNamespace.RadUtil_DetachEventEx(this.IframeDocument,"click",this.IframeDocumentClickHandler);
this.IframeDocumentClickHandler=null;
this.IframeDocument=null;
}
if(this.IframeLoadHandler){
RadWindowNamespace.RadUtil_DetachEventEx(this._iframe,"load",this.IframeLoadHandler);
this.IframeLoadHandler=null;
}
if(this._iframe){
this._iframe.src="javascript:'<html></html>';";
this._iframe=null;
}
window[this.JsName]=null;
this.LoadingWrapper=null;
if(this.MinimizedWindow&&this.MinimizedWindow.Dispose){
this.MinimizedWindow.Dispose();
}
this.MinimizedWindow=null;
if(this.ModalElement&&this.ModalElement.Dispose){
this.ModalElement.Dispose();
}
this.ModalElement=null;
this._events=null;
this.RestoreRect=null;
var _30=document.getElementById("WindowHolder_"+this.Id);
if(_30){
_30.innerHTML="";
}
}
catch(e){
}
};
RadWindowClass.prototype.AttachClientEvent=function(_31,_32){
if(!_32){
return;
}else{
if(null==this._events[_31]){
this._events[_31]=[];
}
var _33=this._events[_31];
if(typeof (_32)=="string"){
_32=eval(_32);
}
_33[_33.length]=_32;
}
};
RadWindowClass.prototype.ExecuteClientEvent=function(_34){
var _35=this._events[_34];
if(null!=_35){
var _36=_35.length;
for(var i=0;i<_36;i++){
try{
_35[i](this);
}
catch(e){
}
}
}
return true;
};
RadWindowClass.prototype.SetModal=function(_38){
this._modal=_38;
if(this._modal&&!this.ModalElement){
this.ModalElement=new RadWindowNamespace.RadWindowModal(this);
}else{
if(!this._modal&&this.ModalElement){
this.ModalElement.Dispose();
this.ModalElement=null;
}
}
};
RadWindowClass.prototype.MoveTo=function(x,y){
var _3b=this;
if(!_3b.WrapperElement){
return;
}
x=parseInt(x);
y=parseInt(y);
_3b.WrapperElement.MoveTo(x,y);
if(!_3b.RestoreRect){
_3b.RestoreRect={};
}
_3b.RestoreRect=_3b.WrapperElement.GetRect();
};
RadWindowClass.prototype.SetWidth=function(_3c){
if(!this.WrapperElement){
this.Width=_3c;
return;
}
var _3d=this.WrapperElement.GetRect();
this.SetSize(_3c,_3d.height);
};
RadWindowClass.prototype.SetHeight=function(_3e){
if(!this.WrapperElement){
this.Height=_3e;
return;
}
var _3f=this.WrapperElement.GetRect();
this.SetSize(_3f.width,_3e);
};
RadWindowClass.prototype.GetWidth=function(){
if(!this.WrapperElement){
return this.Width;
}
var _40=this.WrapperElement.GetRect();
if(_40.width==0&&this.RestoreRect){
return this.RestoreRect.width;
}
return _40.width;
};
RadWindowClass.prototype.GetHeight=function(){
if(!this.WrapperElement){
return this.Height;
}
var _41=this.WrapperElement.GetRect();
if(_41.height==0&&this.RestoreRect){
return this.RestoreRect.height;
}
return _41.height;
};
RadWindowClass.prototype.GetLeftPosition=function(){
if(!this.WrapperElement){
return this.Left;
}
var _42=this.WrapperElement.GetRect();
if(this.IsMinimized()||(_42.left==0&&this.RestoreRect)){
return this.RestoreRect.left;
}
return _42.left;
};
RadWindowClass.prototype.GetTopPosition=function(){
if(!this.WrapperElement){
return this.Top;
}
var _43=this.WrapperElement.GetRect();
if(this.IsMinimized()||(_43.top==0&&this.RestoreRect)){
return this.RestoreRect.top;
}
return _43.top;
};
RadWindowClass.prototype.SetTitle=function(_44){
if(!_44){
return;
}
if(this.TitleElement){
this.TitleElement.innerHTML=_44;
}
this._title=_44;
};
RadWindowClass.prototype.GetWindowManager=function(){
return GetRadWindowManager();
};
RadWindowClass.prototype.GetRectangle=function(){
if(this.IsVisible()){
return this.WrapperElement.GetRect();
}else{
return this.RestoreRect;
}
};
RadWindowClass.prototype.Center=function(){
var _45=this.WrapperElement;
var _46=RadWindowNamespace.RadGetElementRect(_45);
var _47=RadWindowNamespace.RadUtil_GetBrowserRect();
var _48=_46.width;
var _49=_46.height;
var x=_47.left+((_47.width-parseInt(_48))/2);
var y=_47.top+((_47.height-parseInt(_49))/2);
if(!isNaN(x)){
_45.style.left=(x)+"px";
}
if(!isNaN(y)){
_45.style.top=(y)+"px";
}
};
RadWindowClass.prototype.SetVisible=function(_4c){
if(_4c){
if(!this.WrapperElement.Show){
this.WrapperElement.style.display="";
return;
}
var _4d=this.RestoreRect;
if(_4d){
this.WrapperElement.MoveTo(_4d.left,_4d.top);
}
this.WrapperElement.Show();
if(_4d){
this.WrapperElement.SetSize(_4d.width,_4d.height);
}
this._closed=false;
}else{
if(!this.IsVisible()){
return;
}
if(this.WrapperElement.Hide){
this.WrapperElement.Hide();
}else{
this.WrapperElement.style.display="none";
}
}
this.UpdateStatus();
};
RadWindowClass.prototype.SetSize=function(_4e,_4f){
var _50=this;
var _51=function(){
if(_50.StatusElement){
_50.StatusElement.style.width="";
}
var _52=parseInt(_4e);
var _53=parseInt(_4f);
_50.Width=_52;
_50.Height=_53;
if(_50.WrapperElement.SetSize){
_50.WrapperElement.SetSize(_52,_53);
}else{
_50.WrapperElement.style.height=_53+"px";
_50.WrapperElement.style.width=_52+"px";
}
if(_50.RestoreRect){
_50.RestoreRect.width=_52;
_50.RestoreRect.height=_53;
}
_50.UpdateStatus();
};
_51();
};
RadWindowClass.prototype.Create=function(){
if(!this.WrapperElement){
var _54=GetRadWindowManager();
var _55=this.private_BuildWindowHtml();
var _56=document.createElement("SPAN");
_56.setAttribute("id","WindowHolder_"+this.Id);
document.body.appendChild(_56);
_56.innerHTML=_55;
this.WrapperElement=document.getElementById("RadWindowWrapperElement"+this.Id);
this.TitleElement=document.getElementById("RadWindowTitle"+this.Id);
this.HeaderRowElement=document.getElementById("RadWindowHeaderRow"+this.Id);
this.StatusElement=document.getElementById("RadWStatus"+this.Id);
this._iframe=document.getElementById("RadWindowContentFrame"+this.Id);
this.LoadingWrapper=document.getElementById("RadWindowLoadingWrapper"+this.Id);
RadWindowNamespace.MakeMoveable(this.WrapperElement,useDragHelper=true,this._useOverlay,this.IsBehaviorEnabled(RadWindowBehavior.Resize),this.IsBehaviorEnabled(RadWindowBehavior.Move));
var _57=this;
this.WrapperElement.OnMouseUp=function(){
_57.SetActive(true);
};
this.WrapperElement.OnDragEnd=function(){
_57.RestoreRect=_57.WrapperElement.GetRect();
_57.SetActive(true);
_57.UpdateStatus();
_57.ExecuteClientEvent("ondragend");
};
if(this.IsBehaviorEnabled(RadWindowBehavior.Maximize)&&this.HeaderRowElement){
this.HeaderRowElement.ondblclick=function(){
_57.ToggleMaximize();
};
}
this.CreateBackReference();
}
this._created=true;
};
RadWindowClass.prototype.Show=function(){
var _58=this;
var _59=function(){
var _5a=false;
if(!_58._created){
_58.Create();
if(RadWindowBehavior.Minimize&_58._initialBehavior){
_58.Minimize();
_5a=true;
}else{
if(RadWindowBehavior.Maximize&_58._initialBehavior){
_58.Maximize();
_5a=true;
}
}
}
if(_58._url&&(!_58._loaded||_58._reloadOnShow)){
_58.SetUrl(_58._url);
}
if(_5a){
return;
}
if(!_58.RestoreRect){
var _5b=_58.WrapperElement;
if(_58.WrapperElement&&_58.WrapperElement.SetSize){
_58.WrapperElement.SetSize(_58.Width,_58.Height);
}
if(!_58.IsVisible()){
_5b.Show();
}
var _5c=_58.GetLeftTopPosition();
x=_5c.left;
y=_5c.top;
_58.SetVisible(true);
if(_58.IsIE&&"CSS1Compat"==document.compatMode){
_5b.SetSize(_58.Width,_58.Height);
}
_58.MoveTo(x,y);
}else{
_58.SetVisible(true);
}
if(RadWindowBehavior.Pin&_58._initialBehavior){
_58._pinned=false;
_58.TogglePin();
}
if(_58.IsMinimizeModeEnabled(RadWindowMinimizeMode.MinimizeZone)&&!_58.MinimizedWindow){
_58.MinimizedWindow=new RadWindowNamespace.RadWindowMinimize(_58);
}
_58.SetModal(_58._modal);
_58.SetTitle(_58._title);
if(_58._modal){
_58.SetActive(true);
}
_58._closed=false;
if(_58.IsMinimized()){
_58.ExecuteClientEvent("onrestore");
}
_58._minimized=false;
_58.ExecuteClientEvent("onshow");
_58.ExecuteServerClientEvent("OnClientShow");
_58.SizePending=null;
_58.MovePending=null;
};
if((null!=document.readyState&&"complete"!=document.readyState)){
RadWindowNamespace.RadUtil_AttachEventEx(window,"load",_59);
}else{
_59();
}
};
RadWindowClass.prototype.Hide=function(){
this.SetVisible(false);
};
RadWindowClass.prototype.GetLeftTopPosition=function(){
var _5d=this;
var x=null,y=null;
if(_5d._offsetElementId){
var _60=document.getElementById(_5d._offsetElementId);
if(_60){
var _61=RadWindowNamespace.RadGetElementRect(_60);
if(_61){
x=(_61.left+(this.Left?parseInt(this.Left):0));
y=(_61.top+(this.Top?parseInt(this.Top):0));
}
}
}
if(null==x||null==y){
var _62=RadWindowNamespace.RadUtil_GetBrowserRect();
x=RadWindowNamespace.RadGetScrollLeft(document)+(_5d.Left?parseInt(_5d.Left):(_62.width-parseInt(_5d.GetWidth()))/2);
y=RadWindowNamespace.RadGetScrollTop(document)+(_5d.Top?parseInt(_5d.Top):(_62.height-parseInt(_5d.GetHeight()))/2);
var _63=parseInt(_5d.GetHeight());
if(_63<_62.height){
var _64=_63-_62.height;
if(_64>0){
y+=_64/2;
}
}
}
return {left:x,top:y};
};
RadWindowClass.prototype.CallBack=function(_65,_66){
if(true!=_66){
this.Close();
}
var _67=this.ClientCallBackFunction;
if(_67){
if("string"==typeof (_67)){
_67=eval(_67);
}
if("function"==typeof (_67)){
_67(this,_65);
}
}
};
RadWindowClass.prototype.CreateBackReference=function(){
var _68=this;
if(!_68.Argument){
_68.Argument={};
}
var _69=this._iframe;
try{
if(this.ClassicWindow){
var _6a=this.ClassicWindow;
_6a.radWindow=_68;
}else{
_69.radWindow=_68;
if(_69.contentWindow!=null){
_69.contentWindow.radWindow=_68;
}
}
}
catch(e){
}
};
RadWindowClass.prototype.GetContentFrame=function(){
return this._iframe;
};
RadWindowClass.prototype.GetTitlebar=function(){
return this.TitleElement;
};
RadWindowClass.prototype.GetStatusbar=function(){
return this.StatusElement;
};
RadWindowClass.prototype.SetContent=function(_6b){
this.CreateBackReference();
var frm=this._iframe;
if(_6b){
var _6d="function GetRadWindow(){"+"var oWindow = null;"+"if (window.radWindow) oWindow = window.radWindow;"+"else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;"+"return oWindow; }";
if(window.opera){
frm.src=this._schemeBasePath+"../../Opera.html";
var _6e=this;
frm.attachEvent("onload",function(){
frm.contentWindow.document.body.innerHTML=_6b;
re=new RegExp("<(SCRIPT)([^>]*)>([\\s\\S]*?)</(SCRIPT)([^>]*)>","ig");
var _6f=[];
_6b.replace(re,function(_70,a,b,c){
_6f[_6f.length]=c;
return _70;
});
var _74=frm.contentWindow.document.body;
var _75=_74.getElementsByTagName("SCRIPT");
var _76=_75.length;
for(var i=0;i<_76;i++){
var _78=_6f[i];
if(_78.indexOf("GetLocalizedString")>-1){
var _79=_78.indexOf("GetLocalizedString(");
var _7a=_78.indexOf(")",_79+19);
var _7b=_78.substring(_79+20,_7a-1);
_75[i].parentNode.innerHTML+=_6e.GetLocalizedString(_7b);
}
}
var _7c=frm.contentWindow.document;
var _7d=_7c.createElement("SCRIPT");
_74.insertBefore(_7d,_74.firstChild);
_7d.innerHTML=_6d;
var _7e=_74.getElementsByTagName("STYLE");
if(_7e&&_7e.length>0){
var _7f=new RegExp("<(STYLE)([^>]*)>[\\s\\S]*?</(STYLE)([^>]*)>","ig");
var stl=_6b.match(_7f);
var _81=_7e[0];
if(stl){
_81.innerText=stl;
}
}
});
}else{
function fillContent(doc){
doc.write("");
doc.close();
doc.open();
doc.write("<"+"script"+">"+_6d+"<"+"/script>"+_6b);
doc.close();
}
if(!frm.contentWindow||!frm.contentWindow.document){
frm.src=this._schemeBasePath+"../../Opera.html";
var _6e=this;
frm.addEventListener("load",function(){
fillContent(frm.contentWindow.document);
},false);
}else{
fillContent(frm.contentWindow.document);
}
}
}
};
RadWindowClass.prototype.GetUrl=function(){
return this._url;
};
RadWindowClass.prototype.SetUrl=function(url){
var _84=url;
this._url=_84;
var _85=_84;
if(this._reloadOnShow){
var str="rwndrnd="+Math.random();
if(_85.indexOf("?")>-1){
str="&"+str;
}else{
str="?"+str;
}
_85+=str;
}
this._iframe.src=_85;
if(!this._showContentDuringLoad){
this._iframe.style.width="0px";
this._iframe.style.height="0px";
}
this.LoadingWrapper.style.display="";
var _87=this;
var _88=function(){
_87.LoadingWrapper.style.display="none";
_87._iframe.style.width="100%";
_87._iframe.style.height="100%";
if(!_87.IsVisible()||_87.IsActive()||_87.IsClosed()){
}else{
_87.SetActive(true);
}
try{
_87.IframeDocument=_87._iframe.contentWindow.document;
_87.IframeDocumentClickHandler=function(e){
if(!_87.IsVisible()||_87.IsActive()||_87.IsClosed()){
return;
}
_87.SetActive(true);
};
RadWindowNamespace.RadUtil_AttachEventEx(_87.IframeDocument,"click",_87.IframeDocumentClickHandler);
if(_87.IframeDocument.title){
_87.SetTitle(_87.IframeDocument.title);
}
}
catch(e){
}
_87.ExecuteServerClientEvent("OnClientPageLoad");
_87.ExecuteClientEvent("onwindowload");
};
if(!this._loaded){
this.IframeLoadHandler=_88;
RadWindowNamespace.RadUtil_AttachEventEx(this._iframe,"load",this.IframeLoadHandler);
if(window.opera&&this._iframe.attachEvent){
this._iframe.attachEvent("onload",_88);
}
}
this._loaded=true;
};
RadWindowClass.prototype.Reload=function(){
this.LoadingWrapper.style.display="";
try{
this._iframe.contentWindow.location.reload();
}
catch(e){
this.LoadingWrapper.style.display="none";
}
};
RadWindowClass.prototype.SetActive=function(_8a){
if(false!=_8a){
var _8b=this._url;
try{
_8b=this._iframe.contentWindow.location.href;
if(_8b.indexOf("javascript")==0){
_8b="";
}
}
catch(e){
_8b="";
}
if(!this.GetStatus()){
}
this.private_SetActiveCssClass(true);
RadWindowNamespace.RadUtil_SetOnTop(this.WrapperElement);
var _8c=GetRadWindowManager();
var _8d=_8c.GetActiveWindow();
if(this==_8d){
return;
}else{
_8c.SetActiveWindow(this);
}
}
};
RadWindowClass.prototype.SetActiveProtected=function(_8e){
this.private_SetActiveCssClass(_8e);
if(_8e){
var _8f=this.WrapperElement;
if(!this.IsIE&&(this._minimizeMode!=RadWindowMinimizeMode.MinimizeZone)){
_8f.Hide();
_8f.Show();
}
}
this.ExecuteClientEvent(_8e?"onactivate":"ondeactivate");
};
RadWindowClass.prototype.SetStatus=function(_90){
if(this.StatusElement){
this.StatusElement.value=_90;
var _91=this.StatusElement.parentNode;
var _92=_91&&_91.offsetWidth>0?_91.offsetWidth-5:"";
if(_92){
_92+="px";
}
this.StatusElement.style.width=_92;
}
};
RadWindowClass.prototype.UpdateStatus=function(_93){
if(this.StatusElement){
var _94=this;
this.StatusElement.style.width="";
window.setTimeout(function(){
_94.SetStatus(_94.GetStatus());
});
}
};
RadWindowClass.prototype.GetStatus=function(){
if(this.StatusElement){
return this.StatusElement.value;
}
};
RadWindowClass.prototype.Minimize=function(){
if(!this._created||this._closed||this._minimized){
return;
}
this.WrapperElement.Hide();
RadWindowNamespace.RadUtil_EnableScrolling(true);
this._minimized=true;
this._maximized=false;
var _95=GetRadWindowManager();
if(this==_95.GetActiveWindow()){
_95.ActiveWindow=null;
}
if(!this.MinimizedWindow){
this.MinimizedWindow=new RadWindowNamespace.RadWindowMinimize(this);
}
this.ExecuteClientEvent("onminimize");
};
RadWindowClass.prototype.ToggleMaximize=function(){
var _96=this._pinned;
if(_96){
this.TogglePin();
}
if(this._maximized){
this.Restore();
}else{
this.Maximize();
}
if(_96){
this.TogglePin();
}
};
RadWindowClass.prototype.Restore=function(){
if(!this._created){
return;
}
if(this.WrapperElement&&this.WrapperElement.EnableMove){
this.WrapperElement.EnableMove(true);
}
if(this.OnResizeHanlder){
RadWindowNamespace.RadUtil_DetachEventEx(window,"resize",this.OnResizeHanlder);
this.OnResizeHanlder=null;
}
RadWindowNamespace.RadUtil_EnableScrolling(true,this.PageOverflow);
this.PageOverflow=null;
var _97=this.RestoreRect;
if(!_97){
var _98=this.GetLeftTopPosition();
this.RestoreRect={width:this.Width,height:this.Height,top:_98.top,left:_98.left};
_97=this.RestoreRect;
}
this.SetVisible(true);
var _99=this.WrapperElement;
_99.SetSize(_97.width,_97.height);
_99.MoveTo(_97.left,_97.top);
this.private_SetAttribute("ToggleMaximizeButton","title",this.GetLocalizedString("Maximize"));
this.private_SetAttribute("ToggleMaximizeButton","src",this.private_GetImageUrl("Maximize.gif"));
this._maximized=false;
this._minimized=false;
this.SetActive(true);
this.ExecuteClientEvent("onrestore");
return this;
};
RadWindowClass.prototype.Maximize=function(){
if(!this._created){
return;
}
this.SetVisible(true);
if(this.WrapperElement&&this.WrapperElement.EnableMove){
this.WrapperElement.EnableMove(false);
}
this.PageOverflow=document.body.style.overflow;
RadWindowNamespace.RadUtil_EnableScrolling(false);
var _9a=RadWindowNamespace.RadUtil_GetBrowserRect();
this.WrapperElement.MoveTo(_9a.left,_9a.top);
this.WrapperElement.SetSize(_9a.width,_9a.height,false);
this.private_SetAttribute("ToggleMaximizeButton","title",this.GetLocalizedString("Restore"));
this.private_SetAttribute("ToggleMaximizeButton","src",this.private_GetImageUrl("Restore.gif"));
this._maximized=true;
this._minimized=false;
this.SetActive(true);
if(!this.OnResizeHanlder){
var _9b=this.WrapperElement;
var _9c=this;
this.OnResizeHanlder=function(){
var _9d=RadWindowNamespace.RadUtil_GetBrowserRect();
_9b.MoveTo(_9d.left,_9d.top);
_9b.SetSize(_9d.width,_9d.height,false);
};
RadWindowNamespace.RadUtil_AttachEventEx(window,"resize",this.OnResizeHanlder);
}
this.ExecuteClientEvent("onmaximize");
};
RadWindowClass.prototype.Close=function(_9e){
if(!this._created||this._closed){
return;
}
RadWindowNamespace.RadUtil_EnableScrolling(true);
this.SetVisible(false);
this._closed=true;
if(null!=_9e){
this.CallBack(_9e);
}
var _9f=GetRadWindowManager();
if(this==_9f.GetActiveWindow()){
_9f.ActiveWindow=null;
}
this.ExecuteClientEvent("onclose");
if(_9f._singleNonMinimizedWindow&&this.IsMinimizeModeEnabled(RadWindowMinimizeMode.MinimizeZone)){
this.ExecuteServerClientEvent("OnClientClose");
this.Argument=null;
return;
}
if(this.WindowToSetActive){
this.WindowToSetActive.SetActive(true);
this.WindowToSetActive=null;
}else{
_9f.FocusNextWindow(this);
}
if(true==this._destroyOnClose){
_9f.UnregisterWindow(this);
}
this.ExecuteServerClientEvent("OnClientClose");
this.Argument=null;
};
RadWindowClass.prototype.TogglePin=function(){
if(!this._created){
return;
}
this._pinned=!this._pinned;
var _a0=this._pinned?"PinOn":"PinOff";
this.private_SetAttribute("TogglePinButton","title",this.GetLocalizedString(_a0));
this.private_SetAttribute("TogglePinButton","src",this.private_GetImageUrl(_a0+".gif"));
RadWindowNamespace.RadUtil_SetPinned(this._pinned,this.WrapperElement);
this.ExecuteClientEvent("ontogglepin");
};
RadWindowClass.prototype.private_SetAttribute=function(id,_a2,_a3){
var _a4=document.getElementById(id+this.Id);
if(_a4&&_a4.setAttribute){
_a4.setAttribute(_a2,_a3,0);
}
};
RadWindowClass.prototype.private_SetSizeValue=function(_a5,_a6,_a7){
if(null==_a6||""==_a6){
if(!_a7){
return;
}
}else{
this[_a5]=_a6;
}
};
RadWindowClass.prototype.private_GetImageUrl=function(_a8){
return this._schemeBasePath+"Img/"+_a8;
};
RadWindowClass.prototype.GetLocalizedString=function(key){
var str=this._localization[key];
return str?str:key;
};
RadWindowClass.prototype.toString=function(){
return "object [RadWindow id="+this.Id+"]";
};
RadWindowClass.prototype.private_SetActiveCssClass=function(_ab){
this.WrapperElement.className=_ab?"RadWWrapperActive":"RadWWrapperInactive";
};
RadWindowClass.prototype.IsMaximized=function(){
return this._maximized;
};
RadWindowClass.prototype.IsMinimized=function(){
return this._minimized;
};
RadWindowClass.prototype.IsModal=function(){
return this._modal;
};
RadWindowClass.prototype.IsClosed=function(){
return this._closed;
};
RadWindowClass.prototype.IsPinned=function(){
return this._pinned;
};
RadWindowClass.prototype.IsVisible=function(){
return (this.WrapperElement&&this.WrapperElement.style.display!="none");
};
RadWindowClass.prototype.IsActive=function(){
try{
var _ac=GetRadWindowManager();
return (_ac.GetActiveWindow()==this);
}
catch(e){
}
};
RadWindowClass.prototype.IsMinimizeModeEnabled=function(_ad){
return _ad&this._minimizeMode;
};
RadWindowClass.prototype.IsBehaviorEnabled=function(_ae){
return _ae&this._behavior?true:false;
};
RadWindowClass.prototype.private_RenderCommandButtons=function(){
var id=this.Id;
var _b0=this.JsName;
var _b1="";
if(!this._modal&&this.IsBehaviorEnabled(RadWindowBehavior.Pin)){
_b1+="\t\t<td width='1' title='"+this.GetLocalizedString("PinOff")+"' class='RadWWrapperHeaderCenter' nowrap>\n"+"<img onmousedown='return RadWindowNamespace.RadUtil_CancelEvent(event);' class='RadWButton' border='0' src='"+this.private_GetImageUrl("PinOff.gif")+"' id='TogglePinButton"+id+"' onclick='"+_b0+".TogglePin();return false;' ondblclick='return RadWindowNamespace.RadUtil_CancelEvent(event);'/>"+"</td>\n";
}
if(this.IsBehaviorEnabled(RadWindowBehavior.Reload)){
_b1+="\t\t<td width='1' class='RadWWrapperHeaderCenter' nowrap>\n"+"\t\t\t\t\t<img onmousedown='return RadWindowNamespace.RadUtil_CancelEvent(event);'  class='RadWButton' border='0' src='"+this.private_GetImageUrl("Reload.gif")+"' title='"+this.GetLocalizedString("Reload")+"' id='ReloadButton"+id+"' onclick='"+_b0+".Reload();return false;' ondblclick='return RadWindowNamespace.RadUtil_CancelEvent(event);'/>"+"\t\t\t</td>\n";
}
if(this.IsBehaviorEnabled(RadWindowBehavior.Minimize)){
_b1+="\t\t<td width='1'  class='RadWWrapperHeaderCenter' nowrap>\n"+"\t\t\t\t\t<img onmousedown='return RadWindowNamespace.RadUtil_CancelEvent(event);' class='RadWButton' border='0' src='"+this.private_GetImageUrl("Minimize.gif")+"' title='"+this.GetLocalizedString("Minimize")+"' id='MinimizeButton"+id+"'  onclick='"+_b0+".Minimize();return false;'/>"+"\t\t\t</td>\n";
}
if(this.IsBehaviorEnabled(RadWindowBehavior.Maximize)){
_b1+="\t\t<td width='1' class='RadWWrapperHeaderCenter' nowrap>\n"+"\t\t\t\t\t<img onmousedown='return RadWindowNamespace.RadUtil_CancelEvent(event);' class='RadWButton' border='0' src='"+this.private_GetImageUrl("Maximize.gif")+"' title='"+this.GetLocalizedString("Maximize")+"' id='ToggleMaximizeButton"+id+"' onclick='"+_b0+".ToggleMaximize();return false;'/>"+"\t\t\t</td>\n";
}
if(this.IsBehaviorEnabled(RadWindowBehavior.Close)){
_b1+="\t\t\t<td width='1' title='"+this.GetLocalizedString("Close")+"' class='RadWWrapperHeaderCenter' nowrap>\n"+"\t\t\t\t\t<img onmousedown='return RadWindowNamespace.RadUtil_CancelEvent(event);' class='RadWButton' border='0' src='"+this.private_GetImageUrl("Close.gif")+"' id='CloseButton"+id+"'  onclick='"+_b0+".Close();return false;'/>\n"+"\t\t\t\t</td>\n";
}
return _b1;
};
RadWindowClass.prototype.private_BuildWindowHtml=function(){
var id=this.Id;
var _b3=this.JsName;
var _b4=this.Name;
var url=document.all?"javascript:'';":"";
var _b6="";
_b6+="\t\t<table border=0 id='RadWindowWrapperElement"+id+"' class='RadWWrapperActive' style='display:none;z-index:"+this.Zindex+";width:"+this.Width+";height:"+this.Height+";position:absolute;' cellspacing='0' cellpadding='0'>\n"+"\t\t  <tbody style='"+(document.all?"":"height:100%")+"'>"+"\t\t\t<tr class='RadWTitleRow' "+"\t\t\t\tstyle='"+(this._visibleTitlebar?"":"display:none")+"'>\n"+"\t\t\t\t<td width='1' style='height:3px;' class='RadWWrapperHeaderLeft' nowrap></td>\n"+"\t\t\t\t<td valign='top' unselectable='on' grip='true' titleGrip='show' width='100%' style='height:3px;' class='RadWWrapperHeaderCenter' nowrap='true' >\n"+" <div class='RadWHeaderTopResizer'>&nbsp;</div>"+"\t\t<table border=0 cellspacing='0' cellpadding=0' width='100%' ><tr>"+"<td class='RadWWrapperHeaderCenter'>\n"+"\t\t\t\t\t<img ondblclick='"+_b3+".Close();return RadWindowNamespace.RadUtil_CancelEvent(event);' class='RadWIcon' src='"+this._iconUrl+"' align='absmiddle' border='0'>"+"\t\t\t\t</td><td id='RadWindowHeaderRow"+id+"' class='RadWWrapperHeaderCenter' nowrap width='100%'>\t<span id='RadWindowTitle"+id+"' unselectable='on' onselectstart='return false;' class='RadWTitleText'>"+this._title+"</span>\n"+"\t\t\t\t</td>";
_b6+=this.private_RenderCommandButtons();
_b6+="\t\t\t</tr></table> </td>\n";
_b6+="\t\t\t\t<td width='1' class='RadWWrapperHeaderRight' nowrap></td>\n"+"\t\t\t</tr>\n";
_b6+="\t\t\t<tr height='100%' style='height:100%' >\n"+"\t\t\t\t<td align='left' id='RadWindowContentTD"+id+"' colspan='8' style='width:100%;height:100%;'>\n"+"\t\t\t\t\t<table style='border:0px solid red;width:100%;height:100%;' cellspacing='0' cellpadding='0'>\n"+"\t\t\t\t\t\t<tbody style='height:100%'><tr height='100%' style='height:100%'>"+"\t\t\t\t\t\t\t<td rowspan=2 width='1' class='RadWWrapperBodyLeft' nowrap>&nbsp;</td>\n"+"\t\t\t\t\t\t\t<td height='100%' style='height:100%' width='100%' class='RadWWrapperBodyCenter' valign='bottom' align='left' onselectstart='return false;'>\n"+"\t\t\t\t\t\t\t\t\t<iframe class='RadWContentFrame' name='"+_b4+"' frameborder='0' style='border:0px solid green;width:100%;height:100%;' id='RadWindowContentFrame"+id+"' src='"+url+"' border='no'  ></iframe>"+"\t\t\t\t\t\t\t</td>"+"\t\t\t\t\t\t\t<td rowspan=2 width='1' class='RadWWrapperBodyRight' nowrap>&nbsp;</td>"+"\t\t\t\t\t\t</tr>"+"\t\t\t\t\t<tr style='height:1px;'><td class='RadWStatusRow'>"+"<div class='RadWStatus' style='"+(this._visibleStatusbar?"":"display:none")+"'> "+"\t\t\t\t\t<span class='RadWLoadingWrapper' style='display:none;white-space:nowrap' id='RadWindowLoadingWrapper"+id+"'>"+"\t\t\t\t\t<img align='absmiddle' src='"+this.private_GetImageUrl("loading.gif")+"' border='0'> "+this.GetLocalizedString("Loading")+"</span> "+"\t\t\t\t\t<input style='font:icon;border:0px solid red;background-color:transparent;' unselectable='on' type='text' onselect='return false;' onbeforeactivate='return false;' onmousedown='return false;'  id='RadWStatus"+id+"'/>"+"\t\t\t</div></td></tr>"+"\t\t\t\t\t</tbody></table>"+"\t\t\t\t</td>\n"+"\t\t\t</tr>\n";
_b6+="\t\t\t<tr>\n"+"\t\t\t\t<td colspan='8' width='100%' height='1'>"+"\t\t\t\t\t<table border='0' width='100%' height='1' cellspacing='0' cellpadding='0'>\n"+"\t\t\t\t\t\t<tr>\n"+"\t\t\t\t\t\t\t<td width='1' class='RadWWrapperFooterLeft' nowrap>&nbsp;</td>\n"+"\t\t\t\t\t\t\t<td width='100%' class='RadWWrapperFooterCenter' nowrap>&nbsp;</td>\t\t\n"+"\t\t\t\t\t\t\t<td width='1' class='RadWWrapperFooterRight' nowrap>&nbsp;</td>\n"+"\t\t\t\t\t\t</tr>\n"+"\t\t\t\t\t</table>\n"+"\t\t\t\t</td>\n"+"\t\t\t</tr>\n"+"\t\t</tbody></table>\n";
return _b6;
};;RadWindowNamespace.RadWindowClassicEmptyFunction=function(){
};
RadWindowNamespace.RadWindowClassic={Create:RadWindowNamespace.RadWindowClassicEmptyFunction,Minimize:RadWindowNamespace.RadWindowClassicEmptyFunction,Maximize:RadWindowNamespace.RadWindowClassicEmptyFunction,Restore:RadWindowNamespace.RadWindowClassicEmptyFunction,TogglePin:RadWindowNamespace.RadWindowClassicEmptyFunction,SetModal:RadWindowNamespace.RadWindowClassicEmptyFunction,Cascade:RadWindowNamespace.RadWindowClassicEmptyFunction,Tile:RadWindowNamespace.RadWindowClassicEmptyFunction,SetUrl:function(_1){
var _2=this.ClassicWindow;
try{
_2.location.href=_1;
}
catch(e){
}
},Show:function(_3){
if(_3){
this._url=_3;
}
var _4="width="+this.Width+", height="+this.Height+", scrollbars=yes"+", resizable="+(this.IsBehaviorEnabled(RadWindowBehavior.Resize)?"yes":"no");
this.ClassicWindow=window.open(this._url,this.Name,_4);
this.CreateBackReference();
},SetPosition:function(_5,_6){
if(this.ClassicWindow){
this.ClassicWindow.dialogLeft=_5;
this.ClassicWindow.dialogTop=_6;
}
},SetSize:function(_7,_8){
var _9=this.ClassicWindow;
if(_9){
if(_9.dialogWidth&&_9.dialogHeight){
_9.dialogWidth=_7;
_9.dialogHeight=_8;
}else{
_9.resizeTo(_7,_8);
}
}
},Dispose:function(){
this.Close();
this.ClassicWindow=null;
},Close:function(){
this.ClassicWindow.close();
},SetActiveProtected:function(){
if(false!=setActive){
this.ClassicWindow.focus();
}else{
this.ClassicWindow.blur();
}
},GetWidth:function(){
var _a=this.ClassicWindow;
if(_a){
if(_a.dialogWidth){
return parseInt(_a.dialogWidth);
}else{
if(window.outerWidth){
return parseInt(window.outerWidth);
}else{
if(_a.document.domain==window.document.domain){
var _b=RadWindowNamespace.RadUtil_GetBrowserRect(_a);
if(_b){
return (_b.width);
}
}
}
}
}
return 100;
},SetWidth:function(_c){
var _d=this.ClassicWindow;
if(_d){
if(_d.dialogWidth){
_d.dialogTop=_d.screenTop-31;
_d.dialogLeft=_d.screenLeft-4;
_d.dialogWidth=_c+"px";
}else{
_d.outerWidth=_c;
}
}
},GetHeight:function(){
var _e=this.ClassicWindow;
if(_e){
if(_e.dialogHeight){
return parseInt(_e.dialogHeight);
}else{
if(window.outerHeight){
return (parseInt(window.outerHeight));
}else{
if(_e.document.domain==window.document.domain){
var _f=RadWindowNamespace.RadUtil_GetBrowserRect(_e);
if(_f){
return (_f.height+30);
}
}
}
}
}
return 30;
},SetHeight:function(_10){
var _11=this.ClassicWindow;
if(_11.dialogWidth){
_11.dialogTop=_11.screenTop-30;
_11.dialogLeft=_11.screenLeft-4;
_11.dialogHeight=_10+"px";
}else{
_11.outerHeight=_10;
}
},IsVisible:function(){
if(!this._closed&&this.ClassicWindow&&!this.ClassicWindow.closed){
return true;
}
return false;
}};;function RadWindowManagerInitialize(id,_2,_3,_4,_5,_6,_7,_8,_9,_a,_b,_c,_d,_e,_f,_10,_11,_12,_13,_14,_15,_16,_17,_18,_19,_1a,_1b,_1c,_1d,_1e,_1f,_20,_21,_22){
var _23=null;
if(RadWindowNamespace.WindowManager){
if(RadWindowNamespace.WindowManager.Id==id){
var _24=RadWindowNamespace.WindowManager.Windows;
for(var i=0;i<_24.length;i++){
_24[i].Dispose();
}
}else{
_23=RadWindowNamespace.WindowManager.Windows;
}
RadWindowNamespace.WindowManager.Windows=null;
RadWindowNamespace.WindowManager=null;
}
var _26=GetRadWindowManager();
if(_23){
_26.Windows=_23;
}
_26.Id=id;
_26.InitializePage(_3+"Img/transp.gif");
_26._applicationPath=_2;
_26._schemeBasePath=_3;
_26._singleNonMinimizedWindow=_7;
_26._useClassicWindows=_5;
_26._localization=eval("localization_"+(_4?_4:"en_US"));
_26._useOverlay=_1f;
_26._enableStandardPopups=_20;
_26._preserveClientState=(_8==true);
RadWindowNamespace.RadUtil_AttachEventEx(window,"unload",_26.RadWindowManagerDispose);
if(_26._preserveClientState){
RadWindowNamespace.RadUtil_AttachEventEx(window,"load",RadWindowNamespace.RadWindowStateManager.RestoreState);
}
_26.CreateBrowserCommands();
if(_26.EnableShortcuts){
_26.EnableShortcuts(_9);
}
var _27=new RadWindowClass("");
_27._events=[];
_27._schemeBasePath=_3;
if(_a){
_27["OnClientShow"]=_a;
}
if(_b){
_27["OnClientClose"]=_b;
}
if(_c){
_27["OnClientPageLoad"]=_c;
}
_27.ClientCallBackFunction=_6;
_27.Width=_f?_f:"500px";
_27.Height=_10?_10:"200px";
_27.private_SetSizeValue("Left",_11,false);
_27.private_SetSizeValue("Top",_12,false);
_27._iconUrl=_d?_d:_3+"Img/defaultIcon.gif";
_27._minimizeIconUrl=_e?_e:_27._iconUrl;
_27._title=_13;
_27._minimizeZoneId=_14;
_27._initialBehavior=_15;
_27._behavior=_16;
_27._minimizeMode=_17;
_27._visibleStatusbar=_19;
_27._visibleTitlebar=_1a;
_27._visibleOnPageLoad=_1b;
_27._modal=_18;
_27._localization=_26._localization;
_27._offsetElementId=_1c;
_27._openerElementId=_1d;
_27._destroyOnClose=_1e;
_27._reloadOnShow=_21;
_27._useOverlay=_26._useOverlay;
_27._showContentDuringLoad=_22;
_26._defaultWindow=_27;
}
RadWindowNamespace.WindowManager=null;
function GetRadWindowManager(){
if(null==RadWindowNamespace.WindowManager){
RadWindowNamespace.WindowManager=new RadWindowManagerClass();
}
return RadWindowNamespace.WindowManager;
}
function RadWindowManagerClass(){
this.Windows=[];
this.HistoryStack=[];
this.PageInitilized=false;
this.Zindex=3000;
this.JavascriptObjectId="radWindow_";
this.ActiveWindow=null;
this._singleNonMinimizedWindow=false;
this._schemeBasePath=null;
this._useOverlay=true;
}
RadWindowManagerClass.prototype.CreateSplash=function(_28,_29){
this.SplashWidth=_28?_28:this._defaultWindow.Width;
this.SplashHeight=_29?_29:this._defaultWindow.Height;
this.ShowSplash();
};
RadWindowManagerClass.prototype.ShowSplash=function(_2a){
var _2b=this;
var _2c=_2b.GetSplashTemplate(_2b._defaultWindow);
var _2d=document.getElementById("RadWSplashHolder");
_2d.style.position="absolute";
_2d.style.zIndex=""+5000;
_2d.innerHTML=_2c;
if(false==_2a){
_2d.style.display="none";
}else{
_2d.style.display="";
var _2e=document.body;
_2e.insertBefore(_2d,_2e.firstChild);
var _2f=this.SplashWidth;
var _30=this.SplashHeight;
var _31=function(){
var _32=RadWindowNamespace.RadGetElementRect(_2d);
var _33=RadWindowNamespace.RadUtil_GetBrowserRect();
var _34=50-Math.floor((parseInt(_2f)*100/_33.width)/2);
var _35=50-Math.ceil((parseInt(_30)*100/_33.height)/2);
_2d.style.left=_34+"%";
_2d.style.top=_35+"%";
};
_31();
if(!this.SplashInitialized){
this.SplashInitialized=true;
RadWindowNamespace.RadUtil_AttachEventEx(window,"resize",_31);
RadWindowNamespace.RadUtil_AttachEventEx(window,"scroll",_31);
RadWindowNamespace.RadUtil_AttachEventEx(window,"load",function(){
window.setTimeout(function(){
_2d.innerHTML="";
},200);
});
}
}
return _2d;
};
RadWindowManagerClass.prototype.RadWindowManagerDispose=function(){
var _36=GetRadWindowManager();
if(_36._preserveClientState&&RadWindowNamespace.RadWindowStateManager){
RadWindowNamespace.RadWindowStateManager.SaveState();
}
try{
_36.ExecuteAll("Dispose");
}
catch(e){
}
_36.Windows=null;
_36.HistoryStack=null;
_36.ActiveWindow=null;
};
RadWindowManagerClass.prototype.InitializePage=function(_37){
if(this.PageInitilized){
return;
}
if(_37){
var _38=RadWindowNamespace.GetOverlayImage();
if(_38){
_38.src=_37;
}
}
this.PageInitilized=true;
};
RadWindowManagerClass.prototype.Open=function(url,_3a){
var _3b=this;
var _3c=_3b.GetWindowByName(_3a);
if(!_3c){
_3c=_3b.CreateWindowObject(_3a);
}
if(_3b._useClassicWindows){
_3c.Show(url);
}else{
_3c.Show();
if(url){
_3c.SetUrl(url);
}
}
return _3c;
};
RadWindowManagerClass.prototype.CreateWindowObject=function(id){
var _3e=this.GetNewZidex();
if(!id){
id="RadWindowClass"+_3e;
}
var _3f=new RadWindowClass(id);
_3f.Zindex=_3e;
this.Windows[this.Windows.length]=_3f;
var _40=this.JavascriptObjectId+_3f.Id;
window[_40]=_3f;
var _41=this._defaultWindow;
if(_41){
for(var _42 in _41){
if("function"!=typeof (_41[_42])){
_3f[_42]=_41[_42];
}
}
}
if(this._useClassicWindows){
Object.Extend(_3f,RadWindowNamespace.RadWindowClassic);
}
_3f.Id=id;
_3f.Name=id;
_3f.JsName=_40;
_3f._events=[];
return _3f;
};
RadWindowManagerClass.prototype.GetNewZidex=function(){
var _43=this.Zindex;
var _44=this.Windows;
var _45=_44.length;
for(var i=0;i<_45;i++){
oWnd=_44[i];
if(oWnd.WrapperElement&&oWnd.WrapperElement.style.zIndex>_43){
_43=oWnd.WrapperElement.style.zIndex;
}
}
oWnd=null;
return (this.Zindex=++_43);
};
RadWindowManagerClass.prototype.CreateStandardPopup=function(_47,_48){
var _49=this.CreateWindowObject(_47);
_49._behavior=RadWindowBehavior.Close+RadWindowBehavior.Move;
_49._initialBehavior=RadWindowBehavior.None;
_49._minimizeMode=RadWindowClass.prototype._minimizeMode;
_49._minimizeZoneId="";
_49._offsetElementId="";
_49._openerElementId="";
_49._visibleStatusbar=false;
_49._destroyOnClose=true;
_49.Create();
_49.SetTitle(_49.GetLocalizedString(_47));
_49.SetModal(true);
var _4a={Text:_48};
var _4b=this.private_GetTemplate(_47+"Template",this._defaultWindow,_4a);
if(_4b){
_49.SetContent(_4b);
}
return _49;
};
RadWindowManagerClass.prototype.GetSplashTemplate=function(_4c){
return this.private_GetTemplate("SplashTemplate",_4c);
};
RadWindowManagerClass.prototype.GetMinimizeTemplate=function(_4d){
return this.private_GetTemplate("MinimizeTemplate",_4d);
};
RadWindowManagerClass.prototype.private_GetTemplate=function(_4e,_4f,_50){
var _51=document.getElementById(this.Id+"_"+_4e);
var _52=RadWindowNamespace.RadUtil_EncodeContent(_51.value,false);
_52=RadWindowNamespace.RadUtil_Format(RadWindowNamespace.RadUtil_Trim(_52),_4f.Id,_4f.JsName,this._schemeBasePath,_4f._minimizeIconUrl,_4f._title,"",_50?_50.Text:"");
return _52;
};
RadWindowManagerClass.prototype.private_GetWindowsSortedByZindex=function(){
var _53=this.Windows.concat([]);
var _54=function(_55,_56){
if(_55.Zindex==_55.Zindex){
return 0;
}
return (_55.Zindex<_55.Zindex?-1:1);
};
return _53.sort(_54);
};
RadWindowManagerClass.prototype.private_RemoveArrayMember=function(_57,_58){
if(!_57||_57.length<1){
return;
}
for(var i=0;i<_57.length;i++){
if(_57[i]==_58){
_57.splice(i,1);
return;
}
}
};
RadWindowManagerClass.prototype.private_AddArrayMember=function(_5a,_5b){
if(!_5a){
return;
}
_5a[_5a.length]=_5b;
};
RadWindowManagerClass.prototype.GetActiveWindow=function(){
return this.ActiveWindow;
};
RadWindowManagerClass.prototype.SetActiveWindow=function(_5c){
var _5d=this.ActiveWindow;
this.ActiveWindow=_5c;
if(_5d&&_5d!=_5c){
_5d.SetActiveProtected(false);
}
this.ActiveWindow.SetActiveProtected(true);
if(this._singleNonMinimizedWindow){
this.MinimizeInactiveWindows();
}
this.private_RemoveArrayMember(this.HistoryStack,this.ActiveWindow);
this.private_AddArrayMember(this.HistoryStack,this.ActiveWindow);
};
RadWindowManagerClass.prototype.FocusNextWindow=function(_5e){
var _5f=function(_60,_61){
if(_60&&_60._created&&!_60.IsClosed()&&(!_60.IsMinimized()||_61._singleNonMinimizedWindow)){
_60.SetActive(true);
return true;
}
return false;
};
if(null!=_5e){
this.private_RemoveArrayMember(this.HistoryStack,_5e);
var _62=this.HistoryStack.length>0?this.HistoryStack[this.HistoryStack.length-1]:null;
if(_62){
var _63=_5f(_62,this);
if(_63){
return;
}
}
}
var _64=this.ActiveWindow;
var _65=this.Windows.concat([]);
if(!_64){
_5f(_65[0],this);
}else{
var _66=0;
var _67=false;
var i=0;
for(;i<_65.length;i++){
if(_64==_65[i]){
_66=i;
_67=true;
break;
}
}
if(_67){
var _69=function(_6a,_6b,_6c){
for(var _6d=_6a;_6d<_6b;_6d++){
var _6e=_5f(_65[_6d],_6c);
if(_6e){
return true;
}
}
};
var _6f=_69(i+1,_65.length,this);
if(!_6f){
_69(0,_66,this);
}
}
}
};
RadWindowManagerClass.prototype.UnregisterWindow=function(_70){
if(!_70){
return;
}
this.private_RemoveArrayMember(this.Windows,_70);
this.private_RemoveArrayMember(this.HistoryStack,_70);
if(_70.Dispose){
_70.Dispose();
}
if(_70==this.ActiveWindow){
this.ActiveWindow=null;
}
};
RadWindowManagerClass.prototype.GetWindowById=function(id){
var _72=this.Windows;
for(var i=0;i<_72.length;i++){
var _74=_72[i];
if(id==_74.Id){
if(!_74._created){
_74.Create();
}
return _74;
}
}
return null;
};
RadWindowManagerClass.prototype.GetWindowByName=function(_75){
var _76=this.Windows;
for(var i=0;i<_76.length;i++){
var _78=_76[i];
if(_75==_78.Name){
if(!_78._created){
_78.Create();
}
return _78;
}
}
return null;
};
RadWindowManagerClass.prototype.GetWindowObjects=function(){
return this.Windows;
};
RadWindowManagerClass.prototype.GetWindows=function(){
return this.Windows;
};
RadWindowManagerClass.prototype.Cascade=function(){
var _79=40;
var _7a=40;
var _7b=this.private_GetWindowsSortedByZindex();
for(var i=0;i<_7b.length;i++){
var _7d=_7b[i];
if(!_7d._closed&&_7d._created){
var _7e=_7d.Restore();
_7d.MoveTo(_79,_7a);
RadWindowNamespace.RadUtil_SetOnTop(_7d.WrapperElement);
_79+=25;
_7a+=25;
}
}
};
RadWindowManagerClass.prototype.Tile=function(){
var _7f=this.private_GetWindowsSortedByZindex();
var _80=0;
for(var i=0;i<_7f.length;i++){
var _82=_7f[i];
if(!_82._closed&&_82._created){
_80++;
}
}
var _83=5;
var _84=0;
var _85=1;
if(_80<=_83){
_84=_80;
}else{
var i=2;
while((_80*i)<(_83*(i+1))){
i++;
if(i>6){
break;
}
}
_85=i;
_84=Math.ceil(_80/_85);
}
var _86=RadWindowNamespace.RadUtil_GetBrowserRect();
var _87=Math.floor(_86.width/_84);
var _88=Math.floor(_86.height/_85);
var _89=RadWindowNamespace.RadGetScrollLeft(document);
var top=RadWindowNamespace.RadGetScrollTop(document);
var _8b=0;
for(var i=0;i<_7f.length;i++){
var _82=_7f[i];
if(!_82._closed&&_82._created){
_8b++;
if((_8b-1)%(_84)==0&&_8b>_84){
top+=_88;
_89=RadWindowNamespace.RadGetScrollLeft(document);
}
_82.Restore();
_82.MoveTo(_89,top);
_82.SetSize(_87,_88);
_89+=_87;
}
}
};
RadWindowManagerClass.prototype.Fire=function(_8c){
if(this[_8c]&&"function"==typeof (this[_8c])){
this[_8c]();
}
};
RadWindowManagerClass.prototype.MinimizeInactiveWindows=function(){
var _8d=this.ActiveWindow;
var _8e=this.Windows;
var _8f=_8e.length;
for(var i=0;i<_8f;i++){
var _91=_8e[i];
if(_91!=_8d){
_91.Minimize();
}
}
};
RadWindowManagerClass.prototype.EscapeActiveWindow=function(){
var _92=this.GetActiveWindow();
if(_92){
var _93=_92.WrapperElement;
if(_93.IsMoving()||_93.IsResizing()){
_93.CancelDrag();
}else{
_92.Close();
}
}
};
RadWindowManagerClass.prototype.ExecuteActiveWindow=function(_94){
if(this.ActiveWindow&&"function"==typeof (this.ActiveWindow[_94])){
this.ActiveWindow[_94]();
}
};
RadWindowManagerClass.prototype.CloseActiveWindow=function(){
this.ExecuteActiveWindow("Close");
};
RadWindowManagerClass.prototype.MinimizeActiveWindow=function(){
this.ExecuteActiveWindow("Minimize");
};
RadWindowManagerClass.prototype.RestoreActiveWindow=function(){
this.ExecuteActiveWindow("Restore");
};
RadWindowManagerClass.prototype.CloseAll=function(){
this.ExecuteAll("Close");
};
RadWindowManagerClass.prototype.ShowAll=function(){
this.ExecuteAll("Show");
};
RadWindowManagerClass.prototype.MinimizeAll=function(){
this.ExecuteAll("Minimize");
};
RadWindowManagerClass.prototype.MaximizeAll=function(){
this.ExecuteAll("Maximize");
};
RadWindowManagerClass.prototype.RestoreAll=function(){
this.ExecuteAll("Restore");
};
RadWindowManagerClass.prototype.ExecuteAll=function(_95){
if(!this.Windows){
return;
}
var _96=this.Windows.concat([]);
for(var i=0;i<_96.length;i++){
_96[i][_95]();
}
};;RadWindowManagerClass.prototype.CreateBrowserCommands=function(){
var _1=this;
window.radopen=function(_2,_3){
var _4=function(){
var _5=_1.Open(_2,_3);
return _5;
};
if(!RadWindowNamespace.RadUtil_IsDocumentLoaded()){
RadWindowNamespace.RadUtil_AttachEventEx(window,"load",_4);
return null;
}else{
return _4();
}
};
modalDialogCallBack=function(_6){
if(this.Close!=RadWindowClass.prototype.Close){
this.Close=RadWindowClass.prototype.Close;
}
this.Close();
if(this.ModalCallBackFunction){
this.ModalCallBackFunction(_6,this.CallerObj);
}
this.Argument=null;
};
window.radsplash=function(_7){
return _1.ShowSplash(_7);
};
window.radalert=function(_8,_9,_a,_b){
if(!_1._enableStandardPopups){
alert(_8);
}else{
var _c=function(){
if(!_9){
_9=280;
}
if(!_a){
_a=200;
}
var _d=_1.CreateStandardPopup("Alert",_8);
_d.WindowToSetActive=_1.GetActiveWindow();
if(typeof (_b)!="undefined"){
_d.SetTitle(_b);
}
_d.SetSize(_9,_a);
_d["OnClientShow"]=function(){
_d.AutoResize();
_d.Center();
};
window.setTimeout(function(){
_d.Show();
},0);
return _d;
};
if(!RadWindowNamespace.RadUtil_IsDocumentLoaded()){
RadWindowNamespace.RadUtil_AttachEventEx(window,"load",_c);
return null;
}else{
return _c();
}
}
};
window.radprompt=function(_e,_f,_10,_11,_12,_13){
if(!_1._enableStandardPopups){
return prompt(_e);
}else{
var _14=function(){
if(!_10){
_10=280;
}
if(!_11){
_11=210;
}
var _15=_1.CreateStandardPopup("Prompt",_e);
_15.ModalCallBackFunction=_f;
_15.CallerObj=_12;
_15.WindowToSetActive=_1.GetActiveWindow();
if(typeof (_13)!="undefined"){
_15.SetTitle(_13);
}
_15.SetSize(_10,_11);
_15["OnClientShow"]=function(){
_15.AutoResize();
_15.Center();
};
window.setTimeout(function(){
_15.Show();
},0);
_15.Close=function(_16){
if(null==_16){
_16="";
}
_15.Close=RadWindowClass.prototype.Close;
_15.ModalDialogCallBack(_16);
};
_15.ModalDialogCallBack=modalDialogCallBack;
return _15;
};
if(!RadWindowNamespace.RadUtil_IsDocumentLoaded()){
RadWindowNamespace.RadUtil_AttachEventEx(window,"load",_14);
return null;
}else{
return _14();
}
}
};
window.radconfirm=function(_17,_18,_19,_1a,_1b,_1c){
if(!_1._enableStandardPopups){
return confirm(_17);
}else{
var _1d=function(){
if(!_19){
_19=280;
}
if(!_1a){
_1a=210;
}
var _1e=_1.CreateStandardPopup("Confirm",_17);
_1e.ModalCallBackFunction=_18;
_1e.CallerObj=_1b;
_1e.WindowToSetActive=_1.GetActiveWindow();
if(typeof (_1c)!="undefined"){
_1e.SetTitle(_1c);
}
_1e.SetSize(_19,_1a);
_1e["OnClientShow"]=function(){
_1e.AutoResize();
_1e.Center();
};
window.setTimeout(function(){
_1e.Show();
},0);
_1e.Close=function(_1f){
if(null==_1f){
_1f=false;
}
_1e.Close=RadWindowClass.prototype.Close;
_1e.ModalDialogCallBack(_1f);
};
_1e.ModalDialogCallBack=modalDialogCallBack;
return _1e;
};
if(!RadWindowNamespace.RadUtil_IsDocumentLoaded()){
RadWindowNamespace.RadUtil_AttachEventEx(window,"load",_1d);
return null;
}else{
return _1d();
}
}
};
};;RadWindowManagerClass.prototype.EnableShortcuts=function(_1){
try{
this.KeyboardManager={};
Object.Extend(this.KeyboardManager,RadWindowNamespace.RadWindowKeyboardManager);
for(var i=0;i<_1.length;i++){
this.AddShortcut(_1[i][0],_1[i][1]);
}
var _3=this;
RadWindowNamespace.RadUtil_AttachEventEx(document,"keydown",function(_4){
_3.OnKeyDown(_4);
});
}
catch(e){
}
};
RadWindowManagerClass.prototype.OnKeyDown=function(_5){
var _6=RadWindowNamespace.RadUtil_GetEventSource(_5);
if(this.KeyboardManager&&_6){
var _7=this.KeyboardManager.HitTest(_5.keyCode,_5.ctrlKey,(null!=_5.ctrlLeft?_5.ctrlLeft:_5.ctrlKey),_5.shiftKey,(null!=_5.shiftLeft?_5.shiftLeft:_5.shiftKey),_5.altKey,(null!=_5.altLeft?_5.altLeft:_5.altKey));
if(null!=_7){
this.Fire(_7.Name);
RadWindowNamespace.RadUtil_CancelEvent(_5);
}
}
};
RadWindowManagerClass.prototype.AddShortcut=function(_8,_9){
if(this.KeyboardManager){
this.KeyboardManager.AddShortcut(_8,_9);
}
};
RadWindowManagerClass.prototype.RemoveShortcut=function(_a){
if(this.KeyboardManager){
this.KeyboardManager.RemoveShortcut(_a);
}
};
RadWindowManagerClass.prototype.SetShortcut=function(_b,_c){
if(this.KeyboardManager){
this.KeyboardManager.SetShortcut(_b,_c);
}
};
RadWindowNamespace.RadWindowKeyboardManager={Shortcuts:[],Dispose:function(){
this.Shortcuts=null;
},AddShortcut:function(_d,_e){
var rs=new RadWindowNamespace.RadShortcut(_d,_e);
rs.HashValue=this.GetShortcutHashValue(rs);
this.Shortcuts[rs.HashValue]=rs;
},RemoveShortcut:function(_10){
var _11=this.FindByName(_10);
if(_11){
this.Shortcuts[_11.HashValue]=null;
}
},SetShortcut:function(_12,_13){
this.RemoveShortcut(_12);
this.AddShortcut(_12,_13);
},HitTest:function(_14,_15,_16,_17,_18,_19,_1a){
var _1b=this.GetHashValue(_14,_15,_16,_17,_18,_19,_1a);
return this.Shortcuts[_1b];
},GetHashValue:function(_1c,_1d,_1e,_1f,_20,_21,_22){
var _23=_1c&65535;
var _24=0;
_24|=(_1d?RadWindowNamespace.KF_CTRL:0);
_24|=(_1f?RadWindowNamespace.KF_SHIFT:0);
_24|=(_21?RadWindowNamespace.KF_LSHIFT:0);
_23|=(_24<<16);
return _23;
},GetShortcutHashValue:function(_25){
return this.GetHashValue(_25.KeyCode,_25.CtrlKey,_25.LeftCtrlKey,_25.ShiftKey,_25.LeftShiftKey,_25.AltKey,_25.LeftAltKey);
},FindByName:function(_26){
var _27;
for(var _28 in this.Shortcuts){
_27=this.Shortcuts[_28];
if(null!=_27&&_27.Name==_26){
return _27;
}
}
return null;
}};
RadWindowNamespace.KF_CTRL=(1<<0);
RadWindowNamespace.KF_CTRL=(1<<1);
RadWindowNamespace.KF_SHIFT=(1<<2);
RadWindowNamespace.KF_LSHIFT=(1<<3);
RadWindowNamespace.KF_LSHIFT=(1<<4);
RadWindowNamespace.KF_LALT=(1<<5);
RadWindowNamespace.RadShortcut=function(_29,_2a){
this.CtrlKey=false;
this.ShiftKey=false;
this.AltKey=false;
this.KeyCode=0;
this.Name=_29;
this.ParseShortcutString(_2a);
};
RadWindowNamespace.RadShortcut.prototype.ParseShortcutString=function(_2b){
if("string"==typeof (_2b)){
this.CtrlKey=false;
this.LeftCtrlKey=false;
this.ShiftKey=false;
this.LeftShiftKey=false;
this.AltKey=false;
this.LeftAltKey=false;
this.KeyCode=0;
_2b=_2b.replace(/\s*/gi,"");
_2b=_2b.replace(/\+\+/gi,"+PLUS");
var _2c=_2b.split("+");
var _2d="";
for(var i=0;i<_2c.length;i++){
_2d=_2c[i].toUpperCase();
switch(_2d){
case "LCTRL":
this.LeftCtrlKey=true;
case "CTRL":
this.CtrlKey=true;
break;
case "LSHIFT":
this.LeftShiftKey=true;
case "SHIFT":
this.ShiftKey=true;
break;
case "LALT":
this.LeftAltKey=true;
case "ALT":
this.AltKey=true;
break;
case "F1":
this.KeyCode=112;
break;
case "F2":
this.KeyCode=113;
break;
case "F3":
this.KeyCode=114;
break;
case "F4":
this.KeyCode=115;
break;
case "F5":
this.KeyCode=116;
break;
case "F6":
this.KeyCode=117;
break;
case "F7":
this.KeyCode=118;
break;
case "F8":
this.KeyCode=119;
break;
case "F9":
this.KeyCode=120;
break;
case "F10":
this.KeyCode=121;
break;
case "F11":
this.KeyCode=122;
break;
case "F12":
this.KeyCode=123;
break;
case "ENTER":
this.KeyCode=13;
break;
case "HOME":
this.KeyCode=36;
break;
case "END":
this.KeyCode=35;
break;
case "LEFT":
this.KeyCode=37;
break;
case "RIGHT":
this.KeyCode=39;
break;
case "UP":
this.KeyCode=38;
break;
case "DOWN":
this.KeyCode=40;
break;
case "PAGEUP":
this.KeyCode=33;
break;
case "PAGEDOWN":
this.KeyCode=34;
break;
case "SPACE":
this.KeyCode=32;
break;
case "TAB":
this.KeyCode=9;
break;
case "BACK":
this.KeyCode=8;
break;
case "CONTEXT":
this.KeyCode=93;
break;
case "ESCAPE":
case "ESC":
this.KeyCode=27;
break;
case "DELETE":
case "DEL":
this.KeyCode=46;
break;
case "INSERT":
case "INS":
this.KeyCode=45;
break;
case "PLUS":
this.KeyCode="+".charCodeAt(0);
break;
default:
this.KeyCode=_2d.charCodeAt(0);
break;
}
}
}else{
throw {description:"Invalid shortcut string"};
}
};;RadWindowNamespace.RadWindowMinimize=function(_1){
this.Window=_1;
this.Id=_1.Id;
var _2=GetRadWindowManager();
var _3=_2.GetMinimizeTemplate(_1);
var _4=document.createElement("div");
_4.innerHTML=_3;
document.body.appendChild(_4);
this.MinimizedElement=(_4.childNodes.length==1)?_4.firstChild:_4;
var _5=this;
var _6=function(){
_5.SetVisible(false);
};
var _7=this.MinimizedElement;
if(_7){
_7.setAttribute("id","RadWMinimized"+this.Id);
_7.className="RadWMinimizedActive";
_7.onmouseover=function(){
this.CurClassName=this.className;
this.className="RadWMinimizedOver";
};
_7.onmouseout=function(){
this.className=this.CurClassName;
};
}
var _8=_5.FindElement("RadWMinimizedTitle");
if(_8){
_5.TitleElement=_8;
}
var _9=_5.FindElement("RadWMinimizedClose");
if(_9){
if(!_1.IsBehaviorEnabled(RadWindowBehavior.Close)){
_9.style.display="none";
}else{
_9.onclick=function(e){
_1.Close();
};
}
}
if(_1.IsMinimizeModeEnabled(RadWindowMinimizeMode.MinimizeZone)){
var _b=_5.FindElement("RadWMinimizedRestore");
if(_b){
_b.style.display="none";
}
this.MinimizedElement.onclick=function(){
if(_1.IsClosed()){
return;
}
if(!_1.IsVisible()){
_1.Show();
}
_1.SetActive(true);
_7.CurClassName=_7.className="RadWMinimizedActive";
};
_1.AttachClientEvent("onshow",this.OnRadWindowMinimize);
}else{
var _b=_5.FindElement("RadWMinimizedRestore");
if(_b){
_b.onclick=function(){
_5.RestoreWindow();
};
}
this.MinimizedElement.ondblclick=function(){
_5.RestoreWindow();
};
if(this.MinimizedElement.tagName=="TABLE"&&this.MinimizedElement.rows.length>0){
var _c=this.MinimizedElement.rows[0].cells[1];
if(_c){
_c.setAttribute("grip","true");
_c.setAttribute("titleGrip","show");
}
}
RadWindowNamespace.MakeMoveable(this.MinimizedElement,useDragHelper=true,useOverlay=true,resizable=false,_1.IsBehaviorEnabled(RadWindowBehavior.Move));
_7.OnDragEnd=function(){
var _d=this.GetRect();
var _e=_1.RestoreRect;
if(_e){
_e.top=_d.top;
_e.left=_d.left;
}
};
_1.AttachClientEvent("onmaximize",_6);
_1.AttachClientEvent("onrestore",_6);
}
_1.AttachClientEvent("onclose",_6);
_1.AttachClientEvent("onminimize",this.OnRadWindowMinimize);
_1.AttachClientEvent("onactivate",function(){
_5.SetActiveProtected(true);
});
_1.AttachClientEvent("ondeactivate",function(){
_5.SetActiveProtected(false);
});
_1.AttachClientEvent("onwindowload",function(){
if(_5.TitleElement){
_5.TitleElement.innerHTML=_1._title;
}
var _f=_5.MinimizedElement;
if("none"!=_f.style.display){
if(!_5.PageLoadInterval&&!_1.IsVisible()){
_5.PageLoadedCount=0;
_5.PageLoadInterval=window.setInterval(function(){
_5.MinimizedElement.className=((_5.PageLoadedCount++)%2==0)?"RadWMinimizedActive":"RadWMinimizedPageLoaded";
if(11==_5.PageLoadedCount){
window.clearInterval(_5.PageLoadInterval);
_5.PageLoadInterval=null;
_5.PageLoadedCount=0;
}
},500);
}
}
});
};
RadWindowNamespace.RadWindowMinimize.prototype={FindElement:function(_10){
return document.getElementById(_10+this.Id);
},Dispose:function(_11){
this.Window=null;
var _12=this.MinimizedElement;
_12.ondblclick=null;
_12.onclick=null;
_12.Overlay=null;
_12.OnDragEnd=null;
if(_12.parentNode&&_12.parentNode.removeChild){
_12.parentNode.removeChild(_12);
_12.removeAttribute("id");
}
this.MinimizedElement=null;
},OnRadWindowMinimize:function(_13){
var _14=_13._minimizeMode;
var _15=_13.MinimizedWindow;
if(_15.TitleElement){
_15.TitleElement.innerHTML=_13._title;
}
if(!(RadWindowMinimizeMode.SameLocation==_14)){
if(_13._minimizeZoneId){
var _16=document.getElementById(_13._minimizeZoneId);
if(_16){
var _17=_15.MinimizedElement;
if(_17.parentNode!=_16){
_17.parentNode.removeChild(_17);
_16.appendChild(_17);
_17.style.position="";
}
_17.style.display="inline";
}
return;
}
}
var _18=_13.GetRectangle();
var x=null,y=null;
if(!_18){
var _18=_15.Window.GetLeftTopPosition();
x=_18.left;
y=_18.top;
}else{
x=_18.left;
y=_18.top;
}
if(_15.MinimizedElement.MoveTo){
_15.MinimizedElement.MoveTo(x,y);
}
_15.SetVisible(true);
_15.SetPinState();
},RestoreWindow:function(){
var _1b=this.Window;
var _1c=this.MinimizedElement;
var _1d=_1c.GetRect();
var _1e=_1b.RestoreRect;
if(_1e){
_1e.top=_1d.top;
_1e.left=_1d.left;
_1b.Restore();
}else{
if(!_1b.IsVisible()){
_1b.Show();
}
_1b.SetSize(_1b.Width,_1b.Height);
_1b.MoveTo(_1d.left,_1d.top);
_1b.SetActive(true);
}
_1c.Hide();
},SetActiveProtected:function(_1f){
var _20=this.MinimizedElement;
if("none"==_20.style.display){
return;
}
if(_1f){
if(this.Window.IsMinimizeModeEnabled(RadWindowMinimizeMode.MinimizeZone)){
if(!this.Window.IsVisible()){
this.Window.Show();
}
}else{
RadWindowNamespace.RadUtil_SetOnTop(_20);
}
}
_20.className=_1f?"RadWMinimizedActive":"RadWMinimizedInactive";
},SetPinState:function(){
RadWindowNamespace.RadUtil_SetPinned(this.Window.IsPinned(),this.MinimizedElement);
},SetVisible:function(_21){
var _22=this.MinimizedElement;
if(_21){
if(_22.Show){
_22.Show();
}else{
_22.style.display="";
}
var _23=_22.GetRect();
_22.SetSize(_23.width,_23.height,false);
}else{
if(_22.Hide){
_22.Hide();
}else{
_22.style.display="none";
}
}
}};;RadWindowNamespace.GetModalOverlayImage=function(){
if(!RadWindowNamespace.ModalImage){
var _1=document.createElement("IMG");
_1.src=GetRadWindowManager()._schemeBasePath+"Img/transp.gif";
_1.setAttribute("unselectable","on");
_1.setAttribute("galleryimg","no");
_1.onselectstart=RadWindowNamespace.RadUtil_CancelEvent;
_1.ondragstart=RadWindowNamespace.RadUtil_CancelEvent;
_1.onmouseover=RadWindowNamespace.RadUtil_CancelEvent;
_1.onmousemove=RadWindowNamespace.RadUtil_CancelEvent;
_1.onmouseup=RadWindowNamespace.RadUtil_CancelEvent;
_1.style.position="absolute";
_1.className="RadWModalImage";
RadWindowNamespace.ModalImage=_1;
}
return RadWindowNamespace.ModalImage;
};
RadWindowNamespace.ShowModalOverlayImage=function(){
function oModalFun(){
var _2=RadWindowNamespace.GetModalOverlayImage();
if(_2.style.display=="none"){
return;
}
var _3=(document.documentElement.scrollLeft?document.documentElement.scrollLeft:document.body.scrollLeft);
var _4=(document.documentElement.scrollTop?document.documentElement.scrollTop:document.body.scrollTop);
var _5;
if(window.innerWidth){
_5=Math.min(window.innerWidth,document.documentElement.clientWidth);
}else{
_5=Math.max(document.body.clientWidth,document.documentElement.clientWidth);
}
var _6;
if(window.innerHeight){
_6=Math.max(window.innerHeight,document.documentElement.clientHeight);
}else{
if(document.documentElement.clientHeight>0){
_6=document.documentElement.clientHeight;
}else{
_6=document.body.clientHeight;
}
}
_2.style.left=_3+"px";
_2.style.top=_4+"px";
_2.style.width=_5+"px";
_2.style.height=_6+"px";
var _7=GetRadWindowManager().GetActiveWindow();
if(!_7.IsModal()){
return;
}
if(!_7.Top&&!_7.Left){
_7.Center();
}
}
if(!RadWindowNamespace.AttachedHandlers){
RadWindowNamespace._resizeHandler=oModalFun;
RadWindowNamespace._scrollHandler=oModalFun;
RadWindowNamespace.RadUtil_AttachEventEx(window,"resize",RadWindowNamespace._resizeHandler);
RadWindowNamespace.RadUtil_AttachEventEx(window,"scroll",RadWindowNamespace._scrollHandler);
RadWindowNamespace.AttachedHandlers=true;
}
var _8=GetRadWindowManager().GetActiveWindow();
var _9=_8.WrapperElement;
var _a=RadWindowNamespace.GetModalOverlayImage();
if(_9&&_9.style.zIndex){
if(null!=document.readyState&&"complete"!=document.readyState){
return;
}else{
if(_a.parentNode!=document.body){
document.body.appendChild(_a);
}
var _b=parseInt(_9.style.zIndex)+1;
_a.style.display="";
window.setTimeout(function(){
if(!_8.IsActive()){
return;
}
_a.style.zIndex=""+(_b+(document.all?0:-3));
_9.style.zIndex=""+_b;
if(_8&&_8.WrapperElement){
_8.WrapperElement.Show();
}
},100);
}
}
RadWindowNamespace._resizeHandler();
};
RadWindowNamespace.HideModalOverlayImage=function(){
if(RadWindowNamespace.ModalImage){
this.ModalImage.style.display="none";
}
};
RadWindowNamespace.RadWindowModal=function(_c){
this.Window=_c;
var _d=this;
var _e=function(){
_d.SetActiveProtected(true);
};
_c.AttachClientEvent("onactivate",_e);
_c.AttachClientEvent("onrestore",_e);
var _f=function(){
_d.SetActiveProtected(false);
};
_c.AttachClientEvent("onclose",_f);
_c.AttachClientEvent("onminimize",_f);
_c.private_SetActiveCssClass=function(_10){
_c.WrapperElement.className=_10?"RadWWrapperModal":"RadWWrapperInactive";
};
this.InputElementsState=[];
};
RadWindowNamespace.RadWindowModal.prototype.Dispose=function(){
this.Window=null;
this.InputElementsState=null;
};
RadWindowNamespace.RadWindowModal.prototype.SetActiveProtected=function(_11){
var _12=this.Window.GetWindowManager();
if(_11&&!this.Window._closed){
if(this.Window._minimized&&!this.Window.IsMinimizeModeEnabled(RadWindowMinimizeMode.MinimizeZone)){
return;
}
RadWindowNamespace.ShowModalOverlayImage(this.Window);
this.DisableInputElements();
if(_12&&_12.AddShortcut){
_12.DisableTabKey=function(){
};
_12.AddShortcut("DisableTabKey","TAB");
}
}else{
if(_12&&_12.AddShortcut){
_12.DisableTabKey=null;
_12.RemoveShortcut("DisableTabKey");
}
RadWindowNamespace.HideModalOverlayImage();
this.RestoreInputElementsState();
}
};
RadWindowNamespace.RadWindowModal.prototype.DisableInputElements=function(){
if(this.Window.IsIE&&!this.DisabledDrodpowns){
this.InputElementsState=[];
var _13=document.getElementsByTagName("SELECT");
for(var i=0;i<_13.length;i++){
this.InputElementsState[i]={inputElement:_13[i],isDisabled:_13[i].disabled};
_13[i].setAttribute("disabled","true");
}
this.DisabledDrodpowns=true;
}
};
RadWindowNamespace.RadWindowModal.prototype.RestoreInputElementsState=function(){
if(this.Window.IsIE){
this.DisabledDrodpowns=false;
for(var i=0;i<this.InputElementsState.length;i++){
var _o=this.InputElementsState[i];
_o.inputElement.setAttribute("disabled",_o.isDisabled);
}
}
};;RadWindowNamespace.RadWindowStateManager={SaveState:function(){
var _1=GetRadWindowManager();
var _2=_1.GetWindowObjects();
if(!_2){
return;
}
for(i=0;i<_2.length;i++){
var _3=_2[i];
var _4=(_3.IsVisible()||_3.IsMinimized())+"@"+_3.GetWidth()+"@"+_3.GetHeight()+"@"+_3.GetLeftPosition()+"@"+_3.GetTopPosition()+"@"+_3.IsMinimized();
this.SetRadWindowCookie(_3.Id,_4);
}
},RestoreState:function(){
function restoreWindow(_5,_6){
var _7=_6.split("@");
if(_7.length>1){
if("true"==_7[0]&&!_5.IsVisible()){
_5.Show();
}
window.setTimeout(function(){
if(parseInt(_7[1])>0){
_5.SetWidth(_7[1]);
}
if(parseInt(_7[2])>0){
_5.SetHeight(_7[2]);
}
_5.MoveTo(_7[3],_7[4]);
if("true"==_7[5]){
_5.Minimize();
}
},1);
}
}
var _8=RadWindowNamespace.RadWindowStateManager;
var _9=GetRadWindowManager();
var _a=_9.GetWindowObjects();
for(i=0;i<_a.length;i++){
var _b=_a[i];
var _c=_8.GetRadWindowCookie(_b.Id);
if(_c){
restoreWindow(_b,_c);
}
}
},GetOnlyCookie:function(){
var _d="RadWindowCookie";
var _e=document.cookie.split("; ");
for(var i=0;i<_e.length;i++){
var _10=_e[i].split("=");
if(_d==_10[0]){
return _10[1];
}
}
return null;
},SetRadWindowCookie:function(_11,_12){
_11="["+_11+"]";
var _13=this.GetOnlyCookie();
var _14="";
var _15="";
if(_13){
var _16=_13.split(_11);
if(_16&&_16.length>1){
_14=_16[0];
_15=_16[1].substr(_16[1].indexOf("#")+1);
}else{
_15=_13;
}
}
var _17=new Date();
_17.setFullYear(_17.getFullYear()+10);
document.cookie="RadWindowCookie"+"="+(_14+_11+"-"+_12+"#"+_15)+";path=/;expires="+_17.toUTCString()+";";
},GetRadWindowCookie:function(_18){
var _19=this.GetOnlyCookie();
if(!_19){
return;
}
var _1a=null;
_18="["+_18+"]";
var _1b=_19.indexOf(_18);
if(_1b>=0){
var _1c=_1b+_18.length+1;
_1a=_19.substring(_1c,_19.indexOf("#",_1c));
}
return _1a;
}};;RadWindowNamespace.ThresholdX=5;
RadWindowNamespace.ThresholdY=5;
RadWindowNamespace.ResizableObject={EnableResize:true,InitResizeElemsArray:function(){
if(this.ResizeInitialized){
return;
}else{
this.ResizeInitialized=true;
}
this.ResizeCursors=["nw-resize","n-resize","ne-resize","w-resize","e-resize","sw-resize","s-resize","se-resize"];
var _1=[];
_1[0]=this.rows[0].cells[0];
_1[1]=this.rows[0].cells[1].getElementsByTagName("DIV")[0];
_1[2]=this.rows[0].cells[this.rows[0].cells.length-1];
var _2=this.rows[1].getElementsByTagName("TABLE")[0].rows[0];
_1[3]=_2.cells[0];
_1[4]=_2.cells[2];
var _3=this.rows[this.rows.length-1].getElementsByTagName("TABLE")[0].rows[0];
_1[5]=_3.cells[0];
_1[6]=_3.cells[1];
_1[7]=_3.cells[2];
for(var i=0;i<_1.length;i++){
obj=_1[i];
if(obj){
obj.style.cursor=this.ResizeCursors[i];
}
}
this.ResizeElems=_1;
},GetComputedStyle:function(_5,_6,_7){
if(_5.ownerDocument.defaultView&&_5.ownerDocument.defaultView.getComputedStyle){
try{
return _5.ownerDocument.defaultView.getComputedStyle(_5,_7||null)[_6];
}
catch(ev){
}
}else{
if(_5&&_5.currentStyle){
return _5.currentStyle[_6];
}
}
return null;
},CalcResizeDir2:function(_8,_9,_a){
if(!this.EnableResize){
return "";
}
var _b=_8.srcElement?_8.srcElement:_8.target;
this.InitResizeElemsArray();
var _c=this.LastCursor;
this.LastCursor="";
var _c=this.GetComputedStyle(_b,"cursor");
if(!_c||_c=="default"){
_c="";
}
return _c;
},CalcResizeDir:function(_d,_e,_f){
if(this.tagName=="TABLE"){
return this.CalcResizeDir2(_d,_e,_f);
}
if(!this.EnableResize){
return "";
}
var _10=_d.srcElement?_d.srcElement:_d.target;
if(_10!=this){
return "";
}
var rc=this.GetRect();
var _12="";
if(null==_e){
_e=RadWindowNamespace.ThresholdX;
}
if(null==_f){
_f=RadWindowNamespace.ThresholdY;
}
var _13,_14;
if(null!=_d.offsetY){
_13=_d.offsetX;
_14=_d.offsetY;
}else{
if(null!=_d.layerY){
_13=_d.layerX;
_14=_d.layerY;
}
}
if(_14<=_f&&this.AllowNorth){
_12+="n";
}else{
if((rc.height-_14)<=_f&&this.AllowSouth){
_12+="s";
}
}
if(_13<=_e&&this.AllowWest){
_12+="w";
}else{
if((rc.width-_13)<=_e&&this.AllowEast){
_12+="e";
}
}
return (""!=_12?(_12+"-resize"):"");
},Resize:function(_15){
var dX=_15.clientX-this.MouseX;
var dY=_15.clientY-this.MouseY;
this.style.cursor=this.ResizeDir;
switch(this.ResizeDir){
case "n-resize":
this.Inflate(0,dY,null,null);
break;
case "s-resize":
this.Inflate(0,0,0,dY);
break;
case "w-resize":
this.Inflate(dX,0,null,null);
break;
case "e-resize":
this.Inflate(0,0,dX,0);
break;
case "ne-resize":
this.Inflate(0,dY,dX,null);
break;
case "nw-resize":
this.Inflate(dX,dY,null,null);
break;
case "se-resize":
this.Inflate(0,0,dX,dY);
break;
case "sw-resize":
this.Inflate(dX,0,null,dY);
break;
default:
break;
}
},Inflate:function(_18,_19,_1a,_1b){
var rc=this.GetRect();
var top=rc.top+_19;
var _1e=rc.left+_18;
if(top<0){
_19=-rc.top;
}
if(_1e<0){
_18=-rc.left;
}
top=rc.top+_19;
_1e=rc.left+_18;
if(null==_1a){
_1a=-_18;
}
if(null==_1b){
_1b=-_19;
}
var _1f=rc.width+_1a;
var _20=rc.height+_1b;
_1f=Math.max(this.MinWidth,_1f);
_1f=Math.min(this.MaxWidth,_1f);
_20=Math.max(this.MinHeight,_20);
_20=Math.min(this.MaxHeight,_20);
var _21=(this.DragHelper?this.DragHelper:this);
if(rc.width!=_1f&&_1f>5){
_21.MoveBy(_18,0);
_21.SetSize(_1f,null);
}
if(rc.height!=_20&&_20>5){
_21.MoveBy(0,_19);
_21.SetSize(null,_20);
}
},SetResizeDirs:function(_22){
this.AllowNorth=(-1!=_22.indexOf("n"));
this.AllowSouth=(-1!=_22.indexOf("s"));
this.AllowEast=(-1!=_22.indexOf("e"));
this.AllowWest=(-1!=_22.indexOf("w"));
},InitResize:function(){
var _23=this.getAttribute("resize");
if("string"==typeof (_23)){
_23=_23.toLowerCase();
}else{
_23="nsew";
}
this.SetResizeDirs(_23);
this.MinWidth=RadWindowNamespace.RadUtil_GetIntValue(this.getAttribute("minWidth"));
this.MaxWidth=RadWindowNamespace.RadUtil_GetIntValue(this.getAttribute("maxWidth"),100000);
this.MinHeight=RadWindowNamespace.RadUtil_GetIntValue(this.getAttribute("minHeight"));
this.MaxHeight=RadWindowNamespace.RadUtil_GetIntValue(this.getAttribute("maxHeight"),100000);
}};;//BEGIN_ATLAS_NOTIFY
if (typeof(Sys) != "undefined"){if (Sys.Application != null && Sys.Application.notifyScriptLoaded != null){Sys.Application.notifyScriptLoaded();}}
//END_ATLAS_NOTIFY
