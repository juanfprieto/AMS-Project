function HideElement(sender,obj)
{
	if(document.all[String(sender)].value == 'Mostrar')
	{
		document.all[String(obj)].style.visibility="visible";
		document.all[String(sender)].value = "Ocultar";
	}
	else if(document.all[String(sender)].value == 'Ocultar')
	{
		document.all[String(obj)].style.visibility="hidden";
		document.all[String(sender)].value = "Mostrar";
	}
}

//Agrega comas a un numero sin formato ej: 123,456.45
function formatoValor(val){
	valS=""+val;
	valS=valS.replace(/\,/g,'');
	if(valS.length==0 || isNaN(valS))
		return("0");
	else{
		res="";
		prN=valS.split('.');
		if(prN.length>1)tnum=prN[0];
		else tnum=valS;
		lenN=tnum.length;
		if(lenN<=3)res=tnum;
		else{
			lenT=Math.ceil(lenN/3);
			resL=(lenN%3);
			if(resL==0)resL=3;
			for(numD=1;numD<=lenT;numD++){
				if(numD!=lenT){
					if(numD==1)
						res=tnum.substring(lenN-(numD*3),lenN-(numD*3)+3);
					else
						res=tnum.substring(lenN-(numD*3),lenN-(numD*3)+3)+","+res;
				}
				else{
					res=tnum.substring(0,resL)+","+res;
				}
			}
		}
		if(prN.length>1)res=res+"."+prN[1];
		return(res);
	}
}