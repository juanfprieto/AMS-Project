//drct.
function Totales(obj,v){
	if(obj)
		if(isNaN(obj.value))
			obj.value="0";
	var e=document.forms[0].elements.length;
	if(v==1&&parseFloat(obj.value)<v)
		obj.value="0";
	var n=(e-21)/10;
	var c;
	var o,o0,o1,o2,o3,o4,o5,t1,T=0,TA=0;
	//alert("n"+n.toString());
	for(c=2;c<n+2;c++){
		o="_ctl1:dgItems:_ctl"+c.toString();
		o0=o+":edit_1";
		o1=o+":edit_2";
		o2=o+":edit_3";
		o3=o+":edit_4";
		o4=o+":edit_5";
		o5=o+":edit_6";
		o6=o+":edit_7";
		o7=o+":edit_8";
		var cant=parseFloat(document.all[o1].value);	//Cantidad dada
		var punid=parseFloat(document.all[o2].value);	//Precio Unid.
		var iva=parseFloat(document.all[o3].value)/100;	//IVA
		var dcto=parseFloat(document.all[o4].value)/100;//Descuento
		if(dcto<0||dcto>1){
			alert("El valor del descuento no es valido!");
			document.all[o4].value="0.00";}
		if(cant<0){
			alert("La cantidad dada no es valida!");
			document.all[o1].value="0.00";}
		var p=cant*punid;
		t1=p+(p*iva);
		t1=t1-(dcto*t1);
		T+=t1;
		var cantP=parseFloat(document.all[o1].value);//Pedida
		var cantD=parseFloat(document.all[o7].value);//Disponible
		var cn=cantD-cantP;
		var cna;
		if(cn>=0)
			cna=parseFloat(document.all[o1].value);
		else
			cna=cantD.toFixed(2).toString();
		TA+=cna;

		document.all[o6].value=cna.toFixed(2).toString();
		document.all[o5].value=t1.toFixed(2).toString();
	}
	var oT="_ctl1:txtTotal";
	document.all[oT].value=T.toFixed(2).toString();
	var nI="_ctl1:txtNumItem";
	document.all[nI].value=n.toString();
	var aT="_ctl1:txtTotAsig";
	document.all[aT].value=TA.toString();
}
