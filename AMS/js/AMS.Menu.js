var aPadre="";
var ol=1;
var ov=0;
var mnov=0;
var ovd=0;
var mmen=1;
document.onmouseover = domouseover;

function domouseover(){
    if(ovd>=2)
        document.all(ol).style.visibility="hidden";
    mnov=0;
   ovd=ovd+1;
   if(ovd>100)ovd=100;
}

function showmenu(elmnt){
    document.all(mmen).style.visibility="hidden";
    document.all(elmnt).style.visibility="visible";
    mainIframe.style.visibility = "hidden";
    mmen=elmnt;
}

function showsubmenu(elmnt, padre){
    if(padre==aPadre)
        document.all(ol).style.visibility="hidden";
    document.all(elmnt).style.visibility="visible";
    mainIframe.style.visibility = "hidden";
    if(elmnt == 'subchild23' || elmnt == 'subchild24' || elmnt == 'subchild25' || elmnt == 'subchild26')
        document.all(elmnt).style.marginLeft = "-380";
    ol=elmnt;
    aPadre=padre;
}

function hidemenu(elmnt){
    document.all(elmnt).style.visibility="hidden";
    mainIframe.style.visibility = "visible";
}

function mover(obj){
   ov=1;
   mnov=1;
   ovd=0;
}

function over(obj){
	obj.style.background="#e0e0e0";
    obj.style.zIndex=1;
    mnov=1;
    ov=1;
    ovd=0;
}

function out(obj){
	obj.style.background="#ededed";
    mnov=0;
}

function Press(elmnt,subels){
	var charCode = event.keyCode;
	if(charCode == 27)
	{
		hidemenu(elmnt);
		for(var i=0;i<subels;i++)
		{
			if(document.all('subchild'+i)!=null)
				{
					hidemenu('subchild'+i);
				}
		}
	}
}
