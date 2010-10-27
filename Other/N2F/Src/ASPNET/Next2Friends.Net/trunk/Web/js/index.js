
function addHandler( Target , theEvent , Handler, useCapture ) {
    // (c) Mark Lennox of http://www.webpusher.ie 2004
    eval( "var onTarget = Target.on" + theEvent + ";" );
    if ( Target.addEventListener ) {
        Target.addEventListener( theEvent , Handler , useCapture );
    } else if ( Target.attachEvent ) {
        Target.attachEvent( "on" + theEvent , Handler );
    } else if ( onTarget ) { // theory start
        onTarget = function piggyback() {
            onTarget();
            Handler();
        };
    } else { onTarget = Handler(); } // theory end
    return true; // for Netscape 6
}

// Array Remove - By John Resig (MIT Licensed)
    Array.prototype.remove = function(from, to) {
      var rest = this.slice((to || from) + 1 || this.length);
      this.length = from < 0 ? this.length + from : from;
      return this.push.apply(this, rest);
    };
    
    var LiveStreams = new Array();
    
    function livePush(id,v,th,t,r,p){
        var present = false;
        for(var i=0;i<LiveStreams.length;i++){
            if(LiveStreams[i].id==id){
                present = true;
            }
        }
        
        if(!present){
            var livebc = videoSlider.insert(0,id,v,th,t,r,p);
            LiveStreams.push(new Array(id,th,t));
        }
    }
    
    function ajaxGetLiveBroadcasts(){
        IndexPage.GetLB(ajaxGetLiveBroadcasts_Callback); 
    }

    function ajaxGetLiveBroadcasts_Callback(response){

        if(response.value!=null){
            var lb = response.value;
            
            for(var j=0;j<LiveStreams.length;j++){
                var present = false;
                for(var i=0;i<lb.length;i++){
                   if(LiveStreams[j][0]==lb[i].UniqueID){
                        present = true;
                        break;
                   } 
                }
                // if this stream no longer exists then turn it into an archive
                if(!present){
                    videoSlider.remove(LiveStreams[j][0]); 
                    var index = LiveStreams[j][2].indexOf(':');
	                var nickname = LiveStreams[j][2].substring(0,index);
                    var video = 'http://www.next2friends.com/user/' + nickname + '/video/'+ LiveStreams[j][0]+'.flv';                 
                    videoSlider.insert(LiveStreams.length, '', video, LiveStreams[j][1], LiveStreams[j][2], false, false );
                    LiveStreams.remove(j);
                    j--;
                }
            }
            
            for(var i=0;i<lb.length;i++){
            var present = false;
                for(var j=0;j<LiveStreams.length;j++){
                   if(LiveStreams[j][0]==lb[i].UniqueID){
                            present = true;
                            break;
                     } 
                 }
                 
                 if(!present){
                    livePush(lb[i].UniqueID, lb[i].ThumbnailURL, lb[i].ThumbnailURL, lb[i].Title, true, false);
                 }
             }                       
         }
    }

    

    
function player(fl,fr,play,live,id){
    var p = new SWFObject("flvplayer.swf","n2fplayer","327","260","7");
    p.addParam("allowfullscreen","true");
    p.addParam("autostart","true");
    p.addParam('wmode','transparent');
    p.addVariable("Ad",false);
    p.addVariable("image",fr);
    //p.addVariable("logoUrl",null);
    
    
    if(live){
        p.addVariable("videofile",id);
        p.addVariable("live","true");
        p.addParam('bufferlength','2');
        p.addVariable('bufferlength','2');
        
    }else{
        p.addVariable("file",fl);
        p.addVariable("live","false");
    }
    
    p.addVariable("width","327");
    p.addVariable("autostart",play);
    p.addVariable("height","260");
    p.write("divVideoPlayer");
}

var cvids = new Array(5);

function ajaxGetTopVideos(RankingType){
 updateCurrentTab(RankingType);
  if(cvids[RankingType]==null){
        IndexPage.GetTopVideos(RankingType, ajaxGetVideos_Callback);
    }else{
        updateVideoLister(cvids[RankingType]);
  }
}

function ajaxGetVideos_Callback(response,args){
    updateVideoLister(response.value);
    cvids[args.args.TabType] = response.value;
}

function updateVideoLister(html){
    $('#fvids').html(html)
}

function updateCurrentTab(RankingType){
    $('#vidTab1').removeClass('current');
    $('#vidTab2').removeClass('current');
    $('#vidTab3').removeClass('current');
    $('#vidTab4').removeClass('current');
    $('#vidTab'+RankingType).addClass('current');
}

