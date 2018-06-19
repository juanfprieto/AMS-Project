//********************************************************************************
//MANEJO DE	FORMATO	FECHA AÑOS-MES-DIA(YYYY-MM-DD	)************************
//********************************************************************************
function DateMask(obj)
{
	var	valor =	obj.value;
	var	back = valor;
	var	contadorGuiones	= 0;
	var	ultimoCaracter = valor.charAt(valor.length-1);
	var	indice = valor.indexOf('-');
	if(ultimoCaracter.charCodeAt() >= 48 &&	ultimoCaracter.charCodeAt()	<= 57)
	{
		while(indice !=	-1)
		{
			contadorGuiones	= contadorGuiones +	1;
			valor =	valor.substring(indice+1,valor.length);
			indice = valor.indexOf('-');
		}
		valor =	back;
		if(contadorGuiones == 0	&& valor.length>4)
			obj.value =	valor.substring(0,4)+"-"+valor.substring(4,valor.length);
		if(contadorGuiones == 1	&& valor.length>7)
		{
			obj.value =	valor.substring(0,7)+"-"+valor.substring(7,valor.length);
			valor =	obj.value;
			var	splitVals =	valor.split('-');
			var	mes	= parseInt(splitVals[1]);
			if(splitVals[1]	== "08")
				mes	= 8;
			else if(splitVals[1] ==	"09")
				mes	= 9;
			if(mes<=0 || mes>12)
			{
				alert("\nEl	mes	ingresado es invalido.\n Revise	Por	Favor!");
				obj.value =	splitVals[0]+"-";
			}
		}
		if(contadorGuiones == 2	&& valor.length	== 10)
		{
			var	splitVals =	valor.split('-');
			var	ano	= parseInt(splitVals[0]);
			var	mes	= parseInt(splitVals[1]);
			if(splitVals[1]	== "08")
				mes	= 8;
			else if(splitVals[1] ==	"09")
				mes	= 9;
			var	dias = parseInt(splitVals[2]);
			if(splitVals[2]	== "08")
				dias = 8;
			else if(splitVals[2] ==	"09")
				dias = 9;
			if(dias	== 0 ||	dias > 31)
			{
				alert("La cantidad de dias ingresada es invalida.\n Numero maximo de dias 31.\n Revise Por Favor!");
				obj.value =	splitVals[0]+"-"+splitVals[1]+"-";
			}//31
			if(mes == 1	|| mes == 3	|| mes == 5	|| mes == 7	|| mes == 8	|| mes == 10 ||	mes	== 12)
			{
				if(dias	> 31)
				{
					alert("La cantidad de dias ingresada es invalida.\n Numero maximo de dias 31.\n Revise Por Favor!");
					obj.value =	splitVals[0]+"-"+splitVals[1]+"-";
				}
			}//30
			if(mes == 4	|| mes == 6	|| mes == 9	|| mes == 11)
			{
				if(dias	> 30)
				{
					alert("La cantidad de dias ingresada es invalida.\n Numero maximo de dias 30.\n Revise Por Favor!");
					obj.value =	splitVals[0]+"-"+splitVals[1]+"-";
				}
			}//29
			if(mes == 2)
			{
				var bisiesto = false;

				if (((ano % 4 == 0) && (ano % 100 != 0)) || ((ano % 4 == 0) && (ano % 400 == 0)))
					bisiesto = true;

				if (bisiesto)
				{
					if(dias	> 29)
					{
						alert("La cantidad de dias ingresada es invalida.\n Numero maximo de dias 29.\n Revise Por Favor!");
						obj.value =	splitVals[0]+"-"+splitVals[1]+"-";				
					}
				}
				else
				{
					if(dias	> 28)
					{
						alert("La cantidad de dias ingresada es invalida.\n Numero maximo de dias 28.\n Revise Por Favor!");
						obj.value =	splitVals[0]+"-"+splitVals[1]+"-";				
					}
				}				
			}
		}
		if(contadorGuiones == 2	&& valor.length>10)
		{
			obj.value =	valor.substring(0,valor.length-1);
			alert("Se ha excedido la longitud de fecha");
		}
	}
	else
		obj.value =	valor.substring(0,valor.length-1);
}
function NumericMask(obj){
    obj.value=formatoValor(obj.value);
}
//********************************************************************************
//MANEJO DE	FORMATO	NUMERICO SEPARANDO UNIDADES	MILES, ETC************************
//********************************************************************************
function NumericMaskE(obj,e)
{
    if(obj.readOnly)
    {
        return;
    }
    var valor = obj.value;
    var negativo = 0;
    if (valor.charAt(0) == "-") {
        negativo = 1;
        valor = valor.replace("-", "");
        obj.value = valor;
    }

   var back = valor;
   var charCode;
   var modulo = 0;
   var parteEntera = 0;
   
   var ultimoCaracter = valor.charAt(valor.length - 1);

   if (window.event){
      charCode=event.keyCode;
   }
   else{
      charCode=e.which;
   }
   //Si	encuentra un punto
   if(valor.indexOf('.')==-1)
   {
		modulo = valor.length%4;
		parteEntera	= Math.floor(valor.length/4);
   }
   //Si	no encuentra punto
   else
   {
		var	valorTemp =	valor.substring(0,valor.indexOf('.'));
		modulo = valorTemp.length%4;
		parteEntera	= Math.floor(valorTemp.length/4);
   }
   //Si	la captura del evento de teclado es	mayor al codigo	40,	que	es donde
   //empiezan los números
   if(charCode >40)
   {
		//Si el	ultimo caracter	digitado es	mayor al codigo	48 (0) y es	menor o	igual al 57	(9)
		//o	el ultimo caracter digitado	es el codigo 46	(.)
		if((ultimoCaracter.charCodeAt()	>= 48 && ultimoCaracter.charCodeAt() <=	57)	|| (ultimoCaracter.charCodeAt()	== 46))
		{
			//Si la	longitud es	mayor a	3
			if(valor.length>3)
			{
				//Si el	ultimo caracter	digitado es	mayo al	48 (0) y menor al 57 (9) y
				//el valor digitado	no es punto
				if((ultimoCaracter.charCodeAt()	>= 48 && ultimoCaracter.charCodeAt() <=	57)&&(valor.indexOf('.')==-1))
				{
					if(modulo == 0)
					{
						for(var	i=3;i<(parteEntera*4)-1;i+=4)
							valor =	valor.substring(0,i)+valor.substring(i+1,i+2)+','+valor.substring(i+2,valor.length);
						obj.value =	valor.substring(0,1)+','+valor.substring(1,valor.length);
					}					
					else if(modulo == 2)
					{
						for(var	i=3;i<(parteEntera*4)-1;i+=4)
							valor =	valor.substring(0,i+2)+valor.substring(i+3,i+4)+','+valor.substring(i+4,valor.length);
						obj.value =	valor.substring(0,1)+valor.substring(2,3)+','+valor.substring(3,valor.length);
					}
					else if(modulo == 3)
					{
						for(var	i=3;i<(parteEntera*4)-1;i+=4)
							valor =	valor.substring(0,i+3)+valor.substring(i+4,i+5)+','+valor.substring(i+5,valor.length);
						obj.value =	valor.substring(0,2)+valor.substring(3,4)+','+valor.substring(4,valor.length);
					}
				}
				else 
				{
					var	valorTemp =	valor.substring(0,valor.length-1);
					if((valorTemp.indexOf('.')!=-1)&&(ultimoCaracter.charCodeAt() == 46))
					{
						alert("Error: Ya se	ha colocado	el punto decimal");
						obj.value =	valor.substring(0,valor.length-1);
					}
				}
			}
		}
		else
			obj.value =	valor.substring(0,valor.length-1);
		//Ahora	debemos	revisar	si no se cometieron	errores
	   if(valor.length>3)
	   {
			var	contadorComas =	0;
			var	indice = 0;
			var	search = obj.value;
			var	other =	"";
			if(search.indexOf('.')!=-1)
			{
				search = search.substring(0,search.indexOf('.'));
				other =	obj.value.substring(search.indexOf('.'),obj.value.length);
			}
			while(indice!=-1)
			{
				indice = search.indexOf(',',indice);
				if(indice != -1)
				{
					contadorComas += 1;
					indice += 1;
				}
			}
			if(contadorComas !=	parteEntera)
			{
				alert("Error: Se ha	digitado mas de	un caracter");
				obj.value =	back.substring(0,back.lastIndexOf(',')+4)+other;
			}
		}
   }
   else	if(charCode	== 38 || charCode == 40	|| charCode	== 37 || charCode == 39	|| charCode	== 36 || charCode == 35	|| charCode	== 33 || charCode == 34	|| charCode	== 16)
   {
		obj.value =	valor;
   }
   else	if(charCode	== 8)
   {
		if(valor.length>3)
		{
			if(valor.indexOf('.')==-1)
			{
				////////////////////////////////
				if(modulo == 2)
				{
					for(var	i=0;i<(parteEntera*4)-1;i+=4)
						valor =	valor.substring(0,i)+valor.substring(i,i+2)+','+valor.substring(i+2,i+3)+valor.substring(i+4,valor.length);
					obj.value =	valor;
				}
				else if(modulo == 1)
				{
					for(var	i=0;i<(parteEntera*4)-1;i+=4)
						valor =	valor.substring(0,i)+valor.substring(i,i+1)+','+valor.substring(i+1,i+2)+valor.substring(i+3,valor.length);
					obj.value =	valor;
				}
				else if(modulo == 0)
				{
					valor =	valor.substring(0,1)+valor.substring(2,valor.length);
					for(var	i=4;i<(parteEntera*4)-1;i+=4)
						valor =	valor.substring(0,i-1)+','+valor.substring(i-1,i)+valor.substring(i+1,valor.length);
					obj.value =	valor;
				}
				///////////////////////////////////
			}
			else
			{
				if((valor.indexOf('.')+1)==valor.length)
					obj.value =	valor.substring(0,valor.length-1);
			}
		}
   }
   else	if(charCode	== 32)
       obj.value = valor.substring(0, valor.length - 1);

    if(negativo == 1)
        obj.value = "-"+obj.value;
}

//***************************************************************************
//***************************************************************************
//MASCARA PARA DAR FORMATO A LAS REFERENCIAS DE	LOS	ITEMS DE ACUEDO	A SU LINEA DE BODEGA
function ItemMask(objTb,objCmb)
{
	var	valor =	objTb.value;
	var	back = valor;
	var	ultimoCaracter = valor.charAt(valor.length-1);
	var	charCode = event.keyCode;
	var	splitVals =	objCmb.value.split('-');
	if(charCode	>40)
	{
		///MANEJO DE REFERENCIAS MAZDA
		if(splitVals[1]	== 'MZ')
		{
			//Debemos determinar cuantos guiones se	han	ingresado
			var	contadorGuiones	= 0;
			var	indiceBusqueda = 0;
			var	indice = valor.indexOf('-');
			while(indice !=	-1)
			{
				contadorGuiones	= contadorGuiones +	1;
				valor =	valor.substring(indice+1,valor.length);
				indice = valor.indexOf('-');
			}
			valor =	back;
			//Si el	contador nos da	cero debemos verificar que lo digitado no supere 4
			if(contadorGuiones == 0)
			{
				if(valor.length>4)
					objTb.value	= valor.substring(0,valor.length-1)+"-"+ultimoCaracter;
			}
			//Si el	contador es	igual a	1 y	el ultimo caracter digitado	es 109 revisamos la	primer parte de	la referencia
			if(contadorGuiones == 1	&& ultimoCaracter == '-')
			{
				back = valor;
				valor =	valor.substring(0,valor.length-1);
				if(valor.length	!= 4)
				{
					alert("Valor Invalido. La primer parte de la referencia	mazda no puede ser diferente de	4");
					objTb.value	= back.substring(0,back.length-1);
				}
			}
			//Si el	contador es	igual a	1 y	el ultimo caracter es diferente	de - revisamos que no la segunda parte de la referencia	no sea mayor a 4
			if(contadorGuiones == 1	&& ultimoCaracter != '-')
			{
				back = valor;
				var	indice = valor.indexOf('-');
				while(indice !=	-1)
				{
					valor =	valor.substring(indice+1,valor.length);
					indice = valor.indexOf('-');
				}
				if(valor.length>2)
					objTb.value	= back.substring(0,back.length-1)+"-"+ultimoCaracter;
			}
			//Si el	contador es	igual a	2 y	la ultima el ultimo	caracter es	igual a	109. 
			if(contadorGuiones == 2	&& ultimoCaracter == '-')
			{
				back = valor;
				valor =	valor.substring(0,valor.length-1);
				var	indice = valor.indexOf('-');
				while(indice !=	-1)
				{
					valor =	valor.substring(indice+1,valor.length);
					indice = valor.indexOf('-');
				}
				if(valor.length	!= 2)
				{
					alert("Valor Invalido. La segunda parte	de la referencia mazda no puede	ser	diferente de 2");
					objTb.value	= back.substring(0,back.length-1);
				}
			}
			//Si el	contador es	igual a	2 y	la ultima el ultimo	caracter es	diferente a	109. 
			if(contadorGuiones == 2	&& ultimoCaracter != '-')
			{
				back = valor;
				var	indice = valor.indexOf('-');
				while(indice !=	-1)
				{
					valor =	valor.substring(indice+1,valor.length);
					indice = valor.indexOf('-');
				}
				if(valor.length>4)
					objTb.value	= back.substring(0,back.length-1)+"-"+ultimoCaracter;
			}
			//Si el	contador es	igual a	3 y	la ultima el ultimo	caracter es	igual a	109. 
			if(contadorGuiones == 3	&& ultimoCaracter == '-')
			{
				back = valor;
				valor =	valor.substring(0,valor.length-1);
				var	indice = valor.indexOf('-');
				while(indice !=	-1)
				{
					valor =	valor.substring(indice+1,valor.length);
					indice = valor.indexOf('-');
				}
				if(valor.length	!= 2)
				{
					if(valor.length	!= 3)
					{
						if(valor.length	!= 4)
						{
							alert("Valor Invalido. La tercera parte	de la referencia mazda no puede	ser	diferente de 2,3 o 4");
							objTb.value	= back.substring(0,back.length-1);
						}
					}
				}
			}
			//Si el	contador es	igual a	3 y	la ultima el ultimo	caracter es	diferente a	109. 
			if(contadorGuiones == 3	&& ultimoCaracter != '-')
			{
				back = valor;
				var	indice = valor.indexOf('-');
				while(indice !=	-1)
				{
					valor =	valor.substring(indice+1,valor.length);
					indice = valor.indexOf('-');
				}
				if(valor.length>2)
					objTb.value	= back.substring(0,back.length-2)+"-"+back.substring(back.length-2,back.length-1)+ultimoCaracter;
			}
			//Si el	contador es	igual a	4 y	la ultima el ultimo	caracter es	igual a	109. 
			if(contadorGuiones == 4	&& ultimoCaracter != '-')
			{
				back = valor;
				var	indice = valor.indexOf('-');
				while(indice !=	-1)
				{
					valor =	valor.substring(indice+1,valor.length);
					indice = valor.indexOf('-');
				}
				if(valor.length>2)
				{
					alert("Valor Maximo	Alcanzado");
					objTb.value	= back.substring(0,back.length-1);
				}
			}
			//Si el	contador es	igual a	4 y	la ultima el ultimo	caracter es	igual a	109. 
			if(contadorGuiones == 4	&& ultimoCaracter == '-')
			{
				back = valor;
				valor =	valor.substring(0,valor.length-1);
				var	indice = valor.indexOf('-');
				while(indice !=	-1)
				{
					valor =	valor.substring(indice+1,valor.length);
					indice = valor.indexOf('-');
				}
				if(valor.length>1)
				{
					alert("Cuando la referencia	se divide en 5;	la cuarta parte	de la referencia debe ser de longitud 1");
					objTb.value	= back.substring(0,back.length-1);
				}
			}
			//Si el	contador es	mayor a	4
			if(contadorGuiones>4)
			{
				alert("Valor Maximo	Alcanzado");
				objTb.value	= valor.substring(0,valor.length-1);
			}
		}
		///FIN MANEJO DE REFERENCIAS MAZDA
		//INICIO DE	MANEJO DE REFERENCIAS HYUNDAY-TOYOTA
		if(splitVals[1]	== 'HY')
		{
			//Debemos determinar cuantos guiones se	han	ingresado 
			var	contadorGuiones	= 0;
			var	indice = valor.indexOf('-');
			while(indice !=	-1)
			{
				contadorGuiones	= contadorGuiones +	1;
				valor =	valor.substring(indice+1,valor.length);
				indice = valor.indexOf('-');
			}
			valor =	back;
			//Si la	cantidad de	guiones	es superior	a 1	debemos	informar del error
			if(contadorGuiones <= 1)
			{
				if(contadorGuiones == 0	&& ultimoCaracter != '-')
				{
					//Debemos determinar si	es la longitud del texto ingresada es mayor	a 5	o no y si el ultimo	caracter es	-
					if(valor.length>5)
						objTb.value	= valor.substring(0,5)+'-'+valor.substring(5,valor.length);
				}
				if(contadorGuiones == 1	&& ultimoCaracter == '-')
				{
					valor =	valor.substring(0,valor.length-1);
					if(valor.length	!= 5)
					{
						alert("La parte	inicial	de la referencia debe ser extrictamente	de longitud	5");
						objTb.value	= valor;
					}
				}
			}
			else
			{
				alert("La cantidad de guiones ingresada	es invalida. No	puede ser mayor	a 1");
				objTb.value	= valor.substring(0,valor.length-1);
			}
		}
		//FIN MANEJO DE	REFERENCIAS	HYUNDAY-TOYOTA
		//INICIO MANEJO	DE REFERENCIAS FORD
		if(splitVals[1]	== 'FR')
		{
			//Debemos determinar cuantos guiones se	han	ingresado 
			var	contadorGuiones	= 0;
			var	indice = valor.indexOf('-');
			while(indice !=	-1)
			{
				contadorGuiones	= contadorGuiones +	1;
				valor =	valor.substring(indice+1,valor.length);
				indice = valor.indexOf('-');
			}
			valor =	back;
			if(contadorGuiones == 0)
			{
				if(valor.length	> 10)
					objTb.value	= valor.substring(0,10)+'-'+valor.substring(10,valor.length);
			}
			if(contadorGuiones == 1	&& ultimoCaracter != '-')
			{
				back = valor;
				var	indice = valor.indexOf('-');
				while(indice !=	-1)
				{
					valor =	valor.substring(indice+1,valor.length);
					indice = valor.indexOf('-');
				}
				if(valor.length>10)
					objTb.value	= back.substring(0,back.length-1)+'-'+ultimoCaracter;
			}
			if(contadorGuiones == 2	&& ultimoCaracter == '-')
			{
				back = valor;
				valor =	valor.substring(0,valor.length-1);
				var	indice = valor.indexOf('-');
				while(indice !=	-1)
				{
					valor =	valor.substring(indice+1,valor.length);
					indice = valor.indexOf('-');
				}
				if(valor.length<1 || valor.length>10)
				{
					alert("En las referencias tipo Ford	la segunda parte debe tener	una	longitud mayor que 0 y menor igual que 10");
					objTb.value	= back.substring(0,back.length-1);
				}
			}
			if(contadorGuiones == 2	&& ultimoCaracter != '-')
			{
				back = valor;
				var	indice = valor.indexOf('-');
				while(indice !=	-1)
				{
					valor =	valor.substring(indice+1,valor.length);
					indice = valor.indexOf('-');
				}
				if(valor.length>8)
				{
					alert("Valor Maximo	Alcanzado");
					objTb.value	= back.substring(0,back.length-1);
				}
			}
			if(contadorGuiones == 3)
			{
				alert("Valor Maximo	Alcanzado");
				objTb.value	= valor.substring(0,valor.length-1);
			}
		}
		//FIN MANEJO DE	REFERENCIAS	FORD
		//INICIO MANEJO	DE REFERENCIAS CHEVROLET
		if(splitVals[1]	== 'CH')
		{
			if(valor.length	== 1)
				objTb.value	= '0000000000000000'+ultimoCaracter;
			else
			{
				if(valor.length	== 18)
					objTb.value	= valor.substring(1,valor.length);
			}
		}
		//FIN MANEJO DE	REFERENCIAS	CHEVROLET
		//INICIO MANEJO	DE REFERENCIAS GENERALES
		if(splitVals[1]	== 'GR'	|| splitVals[1]	== 'RN')
			objTb.value	= valor;
		//FIN MANEJO DE	REFERENCIAS	GENERALES
	}
	else if(charCode ==	38 || charCode == 40 ||	charCode ==	37 || charCode == 39 ||	charCode ==	36 || charCode == 35 ||	charCode ==	33 || charCode == 34 ||	charCode ==	16)
		objTb.value	= valor;
	else if(charCode ==	8)
	{
		if(splitVals[1]	== 'CH')
			objTb.value	= valor.substring(0,valor.length)+'0';
	}
}
//***************************************************************************
//***************************************************************************
//FIN MASCARA PARA DAR FORMATO A LAS REFERENCIAS DE	LOS	ITEMS DE ACUEDO	A SU LINEA DE BODEGA
//***************************************************************************
//***************************************************************************
//EVENTO PARA MANEJAR EL CAMBIO	DEL	DROPDOWNLIST DE	LAS	LINEAS Y DAR FORMATO AL	TEXTO DEL CUADRO DEL TEXTO 
//El VALUE DEL OBJETO SELECT VIENE DIVIDO EN 2 POR UNA RAYITA(-), EN LA	PRIMER PARTE VIENE EL CODIGO DE	PLINEA Y EN	EL SEGUNDO EL TIPO DE LINEA	QUE	ES LA QUE UTILIZAMOS PARA EL PROCESO DE	DIVISION
function ChangeLine(objCmb,objTb)
{
	var	valor =	objTb.value;
	var	back = valor;
	var	base = valor;
	var	splitVals =	objCmb.value.split('-');
	if(valor.length>0)
	{
		var	indice = valor.indexOf('-');
		//Debemos eliminar los guiones que encontremos
		while(indice !=	-1)
		{
			base = base.substring(0,indice)+base.substring(indice+1,base.length);
			indice = base.indexOf('-');
		}
		back = base;
		//Ahora	en base	tenemos	el valor sin guiones y ahora debemos darle un formato de acuerdo 
		//Si la	referencia es mazda	
		if(splitVals[1]	== 'MZ')
		{
			if(back.length > 4)
				base = base.substring(0,4)+'-'+base.substring(4,base.length);
			if(back.length > 6)
				base = base.substring(0,7)+'-'+base.substring(7,base.length);
			if(back.length > 10)
				base = base.substring(0,12)+'-'+base.substring(12,base.length);
			if(back.length >= 13)
				base = base.substring(0,14)+'-'+base.substring(14,17);
		}
		//Si la	referencia es hyunday-toyota
		if(splitVals[1]	== 'HY')
		{
			if(back.length > 5)
				base = base.substring(0,5)+'-'+base.substring(5,base.length);
		}
		//Si la	referencia es ford
		if(splitVals[1]	== 'FR')
		{
			if(back.length > 10)
				base = base.substring(0,10)+'-'+base.substring(10,base.length);
			if(back.length > 20)
				base = base.substring(0,21)+'-'+base.substring(21,base.length);
			if(back.length > 28)
				base = base.substring(0,30);
		}
		//Si la	referencia es Chevrolet
		if(splitVals[1]	== 'CH')
		{
			var	zeros =	'00000000000000000';
			if(back.length >= 17)
				base = base.substring(0,17);
			else
				base = zeros.substring(0,17-base.length)+base;
		}
		//Si la	referencia es Generica
		if(splitVals[1]	== 'GR'	|| splitVals[1]	== 'RN')
			base = base;
		objTb.value	= base;
	}
}

function ChangeLine2(objCmb,objTb,objTb2)
{
	var	valor =	objTb.value;
	var	valor2 = objTb2.value;
	var	back = valor;
	var	back2 =	valor2
	var	base = valor;
	var	base2 =	valor2;
	var	splitVals =	objCmb.value.split('-');
	if(valor.length>0)
	{
		var	indice = valor.indexOf('-');
		//Debemos eliminar los guiones que encontremos
		while(indice !=	-1)
		{
			base = base.substring(0,indice)+base.substring(indice+1,base.length);
			indice = base.indexOf('-');
		}
		back = base;
		//Ahora	en base	tenemos	el valor sin guiones y ahora debemos darle un formato de acuerdo 
		//Si la	referencia es mazda	
		if(splitVals[1]	== 'MZ')
		{
			if(back.length > 4)
				base = base.substring(0,4)+'-'+base.substring(4,base.length);
			if(back.length > 6)
				base = base.substring(0,7)+'-'+base.substring(7,base.length);
			if(back.length > 10)
				base = base.substring(0,12)+'-'+base.substring(12,base.length);
			if(back.length >= 13)
				base = base.substring(0,14)+'-'+base.substring(14,17);
		}
		//Si la	referencia es hyunday-toyota
		if(splitVals[1]	== 'HY')
		{
			if(back.length > 5)
				base = base.substring(0,5)+'-'+base.substring(5,base.length);
		}
		//Si la	referencia es ford
		if(splitVals[1]	== 'FR')
		{
			if(back.length > 10)
				base = base.substring(0,10)+'-'+base.substring(10,base.length);
			if(back.length > 20)
				base = base.substring(0,21)+'-'+base.substring(21,base.length);
			if(back.length > 28)
				base = base.substring(0,30);
		}
		//Si la	referencia es Chevrolet
		if(splitVals[1]	== 'CH')
		{
			var	zeros =	'00000000000000000';
			if(back.length >= 17)
				base = base.substring(0,17);
			else
				base = zeros.substring(0,17-base.length)+base;
		}
		//Si la	referencia es Generica
		if(splitVals[1]	== 'GR'	|| splitVals[1]	== 'RN')
			base = base;
		objTb.value	= base;
	}
	//*************************************************************************
	if(valor2.length>0)
	{
		var	indice = valor2.indexOf('-');
		//Debemos eliminar los guiones que encontremos
		while(indice !=	-1)
		{
			base2 =	base2.substring(0,indice)+base2.substring(indice+1,base2.length);
			indice = base2.indexOf('-');
		}
		back2 =	base2;
		//Ahora	en base	tenemos	el valor sin guiones y ahora debemos darle un formato de acuerdo 
		//Si la	referencia es mazda	
		if(splitVals[1]	== 'MZ')
		{
			if(back2.length	> 4)
				base2 =	base2.substring(0,4)+'-'+base2.substring(4,base2.length);
			if(back2.length	> 6)
				base2 =	base2.substring(0,7)+'-'+base2.substring(7,base2.length);
			if(back2.length	> 10)
				base2 =	base2.substring(0,12)+'-'+base2.substring(12,base2.length);
			if(back2.length	>= 13)
				base2 =	base2.substring(0,14)+'-'+base2.substring(14,17);
		}
		//Si la	referencia es hyunday-toyota
		if(splitVals[1]	== 'HY')
		{
			if(back2.length	> 5)
				base2 =	base2.substring(0,5)+'-'+base2.substring(5,base2.length);
		}
		//Si la	referencia es ford
		if(splitVals[1]	== 'FR')
		{
			if(back2.length	> 10)
				base2 =	base2.substring(0,10)+'-'+base2.substring(10,base2.length);
			if(back2.length	> 20)
				base2 =	base2.substring(0,21)+'-'+base2.substring(21,base2.length);
			if(back2.length	> 28)
				base2 =	base2.substring(0,30);
		}
		//Si la	referencia es Chevrolet
		if(splitVals[1]	== 'CH')
		{
			var	zeros =	'00000000000000000';
			if(back2.length	>= 17)
				base2 =	base2.substring(0,17);
			else
				base2 =	zeros.substring(0,17-base2.length)+base2;
		}
		//Si la	referencia es Generica
		if(splitVals[1]== 'GR' || splitVals[1]== 'RN')
			base2 =	base2;
		objTb2.value = base2;
	}
}
//***************************************************************************
//FIN EVENTO PARA MANEJAR EL CAMBIO	DEL	DROPDOWNLIST DE	LAS	LINEAS Y DAR FORMATO AL	TEXTO DEL CUADRO DEL TEXTO 
//***************************************************************************
//Funcion para eliminar	las	comas del formato numerico

function EliminarComas(numericValue)
{
	var	outPut = numericValue;
	var	indice = outPut.indexOf(',');
	while(indice !=	-1)
	{
		outPut = outPut.substring(0,indice)+outPut.substring(indice+1,outPut.length);
		indice = outPut.indexOf(',');
	}
	return outPut;
}

//***************************************************************************
//Aplicar Mascara Numerica a un	valor establecido
function ApplyNumericMask(obj)
{
	var	valor =	obj.value;
	var	valorEntero	= "";
	var	valorDecimal = "";
	var	indicePunto	= valor.indexOf('.');
	if(indicePunto != -1)
	{
		valorEntero	= valor.substring(0,indicePunto);
		valorDecimal = valor.substring(indicePunto+1,valor.length);
	}
	else
		valorEntero	= valor;
	//Ahora	determinamos si	es necesario aplicar las comas de division
	if(valorEntero.length <= 3)
		obj.value =	valorEntero;
	else
	{
		var	movil =	valorEntero.length-3;
		while(movil>0)
		{
			valorEntero	= valorEntero.substring(0,movil)+','+valorEntero.substring(movil,valorEntero.length);
			movil =	movil-3;
		}
		obj.value =	valorEntero;
	}
	if(valorDecimal.length > 0)
	{
		if(valorDecimal.length <= 2)
			obj.value =	obj.value+'.'+valorDecimal;
		else
			obj.value =	obj.value+'.'+valorDecimal.substring(0,2);
	}
}

//***************************************************************************
//CONVERTIR EL TEXTO DE UN CUADRO (MAYUSCULAS Y MINUSCULAS)
//***************************************************************************
function aMayusculas(obj)
{
    obj.value = obj.value.toUpperCase();
}
function aMinusculas(obj) {
    obj.value = obj.value.toLowerCase();
}

//***************************************************************************
//MANEJO DE FORMATO ALFA NUMERICO, SUPRIMIENDO USO DE SIMBOLOS O CARACTERES ESPECIALES
//***************************************************************************
function AlfaNumMask(obj)
{
	var	valor =	obj.value;
	var	ultimoCaracter = valor.charAt(valor.length-1);
	
	var pattern=/[a-zA-Z0-9\s]/;

	if(pattern.test(ultimoCaracter))
	{
		obj.value =	valor;
	}
	else
	{
		obj.value =	valor.substring(0,valor.length-1);
		alert("Debe ingresar unicamente caracteres alfanumericos!");
	}
}

//usar sólo con onkeypress
function numberMask(evt, obj) {
    var valor = evt.key;
    var esLetra = isNaN(valor);
    if (esLetra || valor == " ") {
        return false;
    } else
        return true;
}