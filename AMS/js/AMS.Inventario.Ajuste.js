//drct.
function Totales(obj,v){
	if(obj)
		if(isNaN(obj.value))
			obj.value="0";
	var e=document.forms[0].elements.length;
	if(v==1&&parseFloat(obj.value)<v)
		obj.value="0";
	var n=(e-12)/9;
	var c;
	var o,o0,o1,o2,o3,o4,o5,o6,a,t=0;
	for(c=2;c<n+2;c++){
		o="_ctl1:dgItems:_ctl"+c.toString();
		o0=o+":edit_1";
		o1=o+":edit_2";
		o2=o+":edit_3";
		o3=o+":edit_4";
		o4=o+":edit_5";
		o5=o+":edit_6";
		o6=o+":edit_7";
		var a1=parseFloat(document.all[o1].value);//cantidad usuario
		var a2=parseFloat(document.all[o4].value);//cantactual
		if(a1<0){
			document.all[o2].readOnly=true;
			document.all[o2].value=document.all[o4].value;
			var a3=parseFloat(document.all[o5].value);//cant_actual
			var a4=parseFloat(document.all[o6].value);//cant_asig
			if(Math.abs(a1)>a3-a4){
				alert("No es valida la cantidad dada para el producto "+document.all[o0].value+"!");
				document.all[o1].value="0";
			}
		}
		else
			document.all[o2].readOnly=false;
		a=(parseFloat(document.all[o1].value)*parseFloat(document.all[o2].value));
		t+=a;
		document.all[o3].value=a.toFixed(2).toString();
	}
	var oT="_ctl1:txtTotal";
	document.all[oT].value=t.toFixed(2).toString();
}