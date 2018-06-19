var notesPrevOnKeyDown = function () {return true};
var notesPrevOnKeyUp = function () {return true};
var ob_dnd_tve,ob_dnd_tvr,ob_dnd_tvt=0,ob_dnd_tvq=0,ob_dnd_tvy=false;
var ob_dnd_tvu=null,ob_dnd_tvi=null;
var tree_dd_path="",tree_dd_id="",ob_dnd_tvo=0,ob_dnd_tvp=0,ob_dnd_tva=true;
var ob_dnd_tvs=0,ob_dnd_tvd=0,objGrid;

// register events
try {document.registerEvents(Event.KEYDOWN)} catch (e) {};
try { document.registerEvents(Event.KEYUP) } catch (e) { };

function ob_t2Grid_GetMouseCoords(event) { if (window.event) { event = window.event; } var mouseX = 0; var mouseY = 0; if (event.pageX && event.pageY) { mouseX = event.pageX; mouseY = event.pageY; } else if (event.clientX && event.clientY) { mouseX = event.clientX + (document.documentElement.scrollLeft ? document.documentElement.scrollLeft : document.body.scrollLeft); mouseY = event.clientY + (document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop); } return [mouseX, mouseY]; } 

function ob_attachDragAndDrop(el)
{
	el.ondragstart = function(){return false;};
	if(document.all)
	{
		el.onmousedown=new Function("ob_dnd_t10(null,this);");
		el.onmouseup=new Function("ob_dnd_t12();");
	}
	else
	{
		el.setAttribute("onmousedown","ob_dnd_t10(event,this);");
		el.setAttribute("onmouseup","ob_dnd_t12();");
	}
}

function ob_dnd_t10(event,el)
{
	// EVENT. Before Drag start.
    objGrid = document.getElementById(grid2id);//grid2;
	ob_dnd_tvy=true;
	ob_dnd_tvi=el;
	
	// events for mouse move
	document.onmousemove=function(e){ob_dnd_t11(e);o_A=null;};
	document.onmouseup=function(e){ob_dnd_t13(e, null, null, null);};
	document.onselectstart=function(){return false;};
	document.onmousedown=function(){return false;};
}

function ob_dnd_t12()
{
	ob_dnd_tvy=false;
	
	// remove the events
	document.onmousemove=null;
	//ob_t53(false);
	document.onselectstart=function(){return true;};
	document.onmousedown=function(){return true;};
}

function ob_dnd_t11(event)
{
	if(ob_dnd_tva==true)
	{
		if(window.event)
		{
			var event=window.event;
			ob_dnd_tvo=event.x;
			ob_dnd_tvp=event.y;
		}
		else
		{
			ob_dnd_tvo=event.pageX;
			ob_dnd_tvp=event.pageY;
		}
		ob_dnd_tva=false;
		return;
	}
	else
	{
		if(window.event)
		{
			var event=window.event;
			ob_dnd_tvs=event.x;
			ob_dnd_tvd=event.y;
		}
		else
		{
			ob_dnd_tvs=event.pageX;
			ob_dnd_tvd=event.pageY;
		}
	}
	
	if((Math.abs(ob_dnd_tvs-ob_dnd_tvo)>5)||(Math.abs(ob_dnd_tvd-ob_dnd_tvp)>5)){}
	else{return;}
	
	if(ob_dnd_tvy==false) return;
	if(ob_dnd_tvu==null)
	{
		// create the dragable div
		ob_dnd_tvu = document.createElement('div');
		document.body.appendChild(ob_dnd_tvu);
		ob_dnd_tvu.id="ob_dnd_drag";

		// create the note ghost
		var tbl = document.createElement("TABLE");
		tbl.id = ob_dnd_tvi.id;
		var eb = tbl.appendChild(document.createElement("tbody"));
		var etr = eb.appendChild(document.createElement("tr"));
		var etdIcon = etr.appendChild(document.createElement("td"));
		var etdContent = etr.appendChild(document.createElement("td"));
		var img = etdIcon.appendChild(document.createElement("img"));
				
		img.src = ob_getRecordIconSrc();
		
        ob_dnd_tvu.className = "ob_t2";
		etdIcon.className="ob_t2";
		etdContent.className="ob_t2";
		etdContent.innerHTML=ob_getRecordHtml(ob_dnd_tvi);		
		ob_dnd_tvu.appendChild(tbl);

		ob_dnd_tvu.style.width=200;
		ob_dnd_tvu.style.position = "absolute";
		ob_dnd_tvu.style.zIndex="999";
		ob_dnd_tvu.style.filter="Alpha(Opacity='80',FinishOpacity='0',Style='1',StartX='0',StartY='0',FinishX='100',FinishY='100')";		
		
		if(window.event) 
		{
			ob_dnd_tvt=document.body.scrollLeft;
			ob_dnd_tvq=document.body.scrollTop;
		}
		else
		{
			ob_dnd_tve=event.pageX;
			ob_dnd_tvr=event.pageY;
		}
	}

//	// show the ghost div at the mouse coords
	/*if(window.event)
	{
		var event=window.event;
		ob_dnd_tvu.style.left=event.x+ob_dnd_tvt-5+'px';
		ob_dnd_tvu.style.top=event.y+ob_dnd_tvq-5+'px';
	}
	else
	{
		ob_dnd_tvu.style.left=event.pageX-5+'px';
		ob_dnd_tvu.style.top=event.pageY-5+'px';
	}*/

    var mouseCoords = ob_t2Grid_GetMouseCoords(event);
    ob_dnd_tvu.style.left = mouseCoords[0] - 5 + 'px';
    ob_dnd_tvu.style.top = mouseCoords[1] - 5 + 'px';
		
	// get the top and bottom of the grid
	var top=ob_dnd_t14(objGrid);
	var bottom=top+objGrid.offsetHeight;

	// scroll if needed
	if((top-ob_dnd_tvu.offsetTop)>-20&&objGrid.scrollTop>0)
	{
		objGrid.scrollTop=objGrid.scrollTop-6;
	}
	
	if((ob_dnd_tvu.offsetTop-bottom)>-40)
	{
		objGrid.scrollTop=objGrid.scrollTop+6;
	}
}

function ob_dnd_t13(event, ob_dnd_tvh, ob_dnd_tvj)
{      
	var e,lensrc,s,s2;ob_dnd_tva=true;

	// if no node as start node
	if(ob_dnd_tvu==null){return;}
	
	// get mouse position
	if (ob_dnd_tvh == null || ob_dnd_tvj == null)
	{
		if(window.event)
		{
			var event=window.event;
			var ob_dnd_tvh=event.x+ob_dnd_tvt;
			var ob_dnd_tvj=event.y+ob_dnd_tvq;
		}
		else
		{
			var ob_dnd_tvh=event.pageX;
			var ob_dnd_tvj=event.pageY;
		}
	}
	
	var ob_dnd_tvf,flagReturn=false;
	ob_dnd_tvu.style.display="none";
	
	// get all child table tags of grid
	items=document.getElementsByTagName("TABLE");
	
	for(i=0; i < items.length; i++)
	{
	    //alert(items[i].parentNode.innerHTML);
		// get top of item
		var top=ob_dnd_t14(items[i]);// - objGrid.scrollTop;
		// get left of item
		var left=ob_dnd_t15(items[i]);// - objGrid.scrollLeft;
		// if mouse up was inside this item
		
		if(items[i].tagName == "TABLE" && (ob_dnd_tvj >= top && ob_dnd_tvj <= items[i].offsetHeight + top) && (ob_dnd_tvh >= left&&ob_dnd_tvh <= items[i].offsetWidth+left))
		{
			// set this as the drop target
			var g1 = document.getElementById(grid1id);
			var w = g1.style.width;
			var wi = parseInt(w.substr(0, w.indexOf('px')));

			if(left > ob_dnd_t15(g1) + wi + 10) 
			    ob_dnd_tvf = items[i];
		}
	} 

	// if not a tree node, set drop target to null
	if(flagReturn==true) ob_dnd_tvf=null;
	
	
	// if drop target not null
	if(ob_dnd_tvf!=null)
	{				    
		ob_dnd_handleMove (ob_dnd_tvi, ob_dnd_tvf);		

		// remove the mouse events
		document.onmousemove="";
		
		// remove the ghost div
		document.body.removeChild(ob_dnd_tvu);
	}
	else
	{
		// remove the ghost div
		document.body.removeChild(document.getElementById("ob_dnd_drag"));
	}

	ob_dnd_tvu=null;
	ob_dnd_tvy=false;
	// restore key events
	document.onkeydown = notesPrevOnKeyDown;
	document.onkeyup = notesPrevOnKeyUp;
	// restore mouse events
	document.onselectstart=function(){return true;};
	document.onmousedown=function(){return true;};
	document.onmouseup=null;
	document.onmousemove = "";		
	// EVENT. After Drag & Drop finished.
}

// get left position
function ob_dnd_t15(vz){
    var pos=0;
    if(vz.offsetParent){
        while(vz.offsetParent){
            pos+=vz.offsetLeft;vz=vz.offsetParent;
        }
    }else if(vz.x)
        pos+=vz.x;
        return pos;
}

// get top position
function ob_dnd_t14(ue){
    var pos=0;
    if(ue.offsetParent){
        while(ue.offsetParent){
            pos+=ue.offsetTop;
            ue=ue.offsetParent;
        }
    }else if (ue.y)
        pos+=ue.y;
        return pos;
}

function NotesKeyModifierWatch(event)
{	
	if (window.event) event = window.event;	
}

function ob_dnd_handleMove (ob_drag_item, ob_drag_destination)
{    		
	updateDropNodeToGrid(ob_drag_item);
	return true;
}

function ob_getRecordHtml(oRecord)
{
    return oRecord.childNodes[1].firstChild.firstChild.innerHTML;
}

function ob_getRecordIconSrc()
{
   return "resources/images/square_yellowS.gif";
}

var droppingNode = null;
function updateDropNodeToGrid(id){  
	//update to DB
	droppingNode = ob_getRecordHtml(id)
	ob_post.AddParam("CompanyName", droppingNode);
	ob_post.post(null, "insertRecord", clientRefresh);
}

function clientRefresh(effectedRecords){
	if (effectedRecords > 0)
	{
		grid2.refresh();
		attachDndToRecords();
		alert('Inserted a new Record (Company Name: "' + droppingNode + '"). In a real application, the database will be updated. ');
	}else{
		alert('Can not insert a new Record.');
	}
	droppingNode = null;
}