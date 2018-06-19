var loadingImage = '../img/loading.gif';		
var closeButton = '../img/close.gif';
function getPageScroll(){
	var yScroll;
	if (self.pageYOffset) {
		yScroll = self.pageYOffset;
	} else if (document.documentElement && document.documentElement.scrollTop){
		yScroll = document.documentElement.scrollTop;
	} else if (document.body) {
		yScroll = document.body.scrollTop;
	}
	arrayPageScroll = new Array('',yScroll) 
	return arrayPageScroll;
}
function getPageSize(){
	var xScroll, yScroll;
	if (window.innerHeight && window.scrollMaxY) {	
		xScroll = document.body.scrollWidth;
		yScroll = window.innerHeight + window.scrollMaxY;
	} else if (document.body.scrollHeight > document.body.offsetHeight){
		xScroll = document.body.scrollWidth;
		yScroll = document.body.scrollHeight;
	} else {
		xScroll = document.body.offsetWidth;
		yScroll = document.body.offsetHeight;
	}
	var windowWidth, windowHeight;
	if (self.innerHeight) {
		windowWidth = self.innerWidth;
		windowHeight = self.innerHeight;
	} else if (document.documentElement && document.documentElement.clientHeight) {
		windowWidth = document.documentElement.clientWidth;
		windowHeight = document.documentElement.clientHeight;
	} else if (document.body) {
		windowWidth = document.body.clientWidth;
		windowHeight = document.body.clientHeight;
	}
	if(yScroll < windowHeight){
		pageHeight = windowHeight;
	} else { 
		pageHeight = yScroll;
	}
	if(xScroll < windowWidth){	
		pageWidth = windowWidth;
	} else {
		pageWidth = xScroll;
	}
	arrayPageSize = new Array(pageWidth,pageHeight,windowWidth,windowHeight) 
	return arrayPageSize;
}
function pause(numberMillis) {
	var now = new Date();
	var exitTime = now.getTime() + numberMillis;
	while (true) {
		now = new Date();
		if (now.getTime() > exitTime)
			return;
	}
}
function getKey(e){
	if (e == null) { // ie
		keycode = event.keyCode;
	} else { // mozilla
		keycode = e.which;
	}
	key = String.fromCharCode(keycode).toLowerCase();
	
	if(key == 'x'){ hideLightbox(); }
}
function listenKey () {	document.onkeypress = getKey; }
function showLightbox(arraySelectsNoHidden)
{
	var objOverlay = document.getElementById('overlay');
	var objLightbox = document.getElementById('lightbox');
	var lightBoxHeight = $('#lightbox').height();
	var lightBoxWidth = $('#lightbox').width();
	var arrayPageSize = getPageSize() - 100;
	var arrayPageScroll = getPageScroll();
	objOverlay.style.height = (arrayPageSize[1] + 'px');
	objOverlay.style.width = (arrayPageSize[0] + 'px')
	objOverlay.style.display = 'block';		

	document.captureEvents(Event.MOUSEMOVE);
	tempX = event.pageX;
	tempX -= (parseInt(lightBoxWidth))/1.6;
	tempY = event.pageY;
	tempY -= parseInt(lightBoxHeight);
	tempY -= 8;

	var lightboxTop = arrayPageScroll[1] + ((arrayPageSize[3] - 35 - 400) / 2);
	var lightboxLeft = ((arrayPageSize[0] - 20 - 400) / 2);
	objLightbox.style.top = (tempY < 0) ? "0px" : tempY + "px";
	objLightbox.style.left = (tempX < 0) ? "0px" : tempX + "px";
	objLightbox.style.top = (tempY);
	objLightbox.style.left = (tempX);
    if (navigator.appVersion.indexOf("MSIE")!=-1){
		pause(250);
	} 
	selects = document.getElementsByTagName("select");
    for (i = 0; i != selects.length; i++) {
		if(!findElementArray(arraySelectsNoHidden,selects[i].id))
		 selects[i].style.visibility = "hidden";
    }
    //objLightbox.style.display = 'block';
    
    
	arrayPageSize = getPageSize();
	objOverlay.style.height = (arrayPageSize[1] + 'px');
	listenKey();
	//$("#lightbox").show('slow');
	$("#lightbox").fadeIn("slow");
	//$("#lightbox").toggle("slow");
	return false;
}
function hideLightbox()
{
	objOverlay = document.getElementById('overlay');
	objLightbox = document.getElementById('lightbox');
	objOverlay.style.display = 'none';
	$("#lightbox").fadeOut("slow");
	selects = document.getElementsByTagName("select");
    for (i = 0; i != selects.length; i++) {
		selects[i].style.visibility = "visible";
	}
    document.onkeypress = '';
    return false;
}
function initLightbox()
{
	if (!document.getElementsByTagName){ return; }
	var anchors = document.getElementsByTagName("a");
	for (var i=0; i<anchors.length; i++){
		var anchor = anchors[i];

		if (anchor.getAttribute("href") && (anchor.getAttribute("rel") == "lightbox")){
			anchor.onclick = function () {showLightbox(this); return false;}
		}
	}
	var objBody = document.getElementsByTagName("body").item(0);
	var arrayPageSize = getPageSize();
	var arrayPageScroll = getPageScroll();
}
function addLoadEvent(func)
{	
	var oldonload = window.onload;
	if (typeof window.onload != 'function'){
    	window.onload = func;
	} else {
		window.onload = function(){
		oldonload();
		func();
		}
	}

}
function findElementArray(varArray,varValue)
{
	var retorno = false;
	for(var i=0;i<varArray.length;i++)
	{
		if(varArray[i] == varValue)
			retorno = true;
	}
	return retorno
}
addLoadEvent(initLightbox);