//***************************************************************************
//***************************************************************************
//MASCARA PARA DAR FORMATO A LAS REFERENCIAS DE LOS ITEMS DE ACUEDO A SU LINEA DE BODEGA
function ItemMask(objTb,objCmb)
{
	var valor = objTb.value;
   	var back = valor;
	var ultimoCaracter = valor.charAt(valor.length-1);
   	var charCode = event.keyCode;
   	if(charCode >40)
    {
    	///MANEJO DE REFERENCIAS MAZDA
		if(objCmb.value == 'MZ')
		{
			//Debemos determinar cuantos guiones se han ingresado
			var contadorGuiones = 0;
			var indiceBusqueda = 0;
			var indice = valor.indexOf('-');
			while(indice != -1)
			{
				contadorGuiones = contadorGuiones + 1;
				valor = valor.substring(indice+1,valor.length);
				indice = valor.indexOf('-');
			}
			valor = back;
			//Si el contador nos da cero debemos verificar que lo digitado no supere 4
			if(contadorGuiones == 0)
			{
				if(valor.length>4)
					objTb.value = valor.substring(0,valor.length-1)+"-"+ultimoCaracter;
			}
			//Si el contador es igual a 1 y el ultimo caracter digitado es 109 revisamos la primer parte de la referencia
			if(contadorGuiones == 1 && ultimoCaracter == '-')
			{
				back = valor;
				valor = valor.substring(0,valor.length-1);
				if(valor.length != 4)
				{
					alert("Valor Invalido. La primer parte de la referencia mazda no puede ser diferente de 4");
					objTb.value = back.substring(0,back.length-1);
				}
			}
			//Si el contador es igual a 1 y el ultimo caracter es diferente de - revisamos que no la segunda parte de la referencia no sea mayor a 4
			if(contadorGuiones == 1 && ultimoCaracter != '-')
			{
				back = valor;
				var indice = valor.indexOf('-');
				while(indice != -1)
				{
					valor = valor.substring(indice+1,valor.length);
					indice = valor.indexOf('-');
				}
				if(valor.length>2)
					objTb.value = back.substring(0,back.length-1)+"-"+ultimoCaracter;
			}
			//Si el contador es igual a 2 y la ultima el ultimo caracter es igual a 109. 
			if(contadorGuiones == 2 && ultimoCaracter == '-')
			{
				back = valor;
				valor = valor.substring(0,valor.length-1);
				var indice = valor.indexOf('-');
				while(indice != -1)
				{
					valor = valor.substring(indice+1,valor.length);
					indice = valor.indexOf('-');
				}
				if(valor.length != 2)
				{
					alert("Valor Invalido. La segunda parte de la referencia mazda no puede ser diferente de 2");
					objTb.value = back.substring(0,back.length-1);
				}
			}
			//Si el contador es igual a 2 y la ultima el ultimo caracter es diferente a 109. 
			if(contadorGuiones == 2 && ultimoCaracter != '-')
			{
				back = valor;
				var indice = valor.indexOf('-');
				while(indice != -1)
				{
					valor = valor.substring(indice+1,valor.length);
					indice = valor.indexOf('-');
				}
				if(valor.length>4)
					objTb.value = back.substring(0,back.length-1)+"-"+ultimoCaracter;
			}
			//Si el contador es igual a 3 y la ultima el ultimo caracter es igual a 109. 
			if(contadorGuiones == 3 && ultimoCaracter == '-')
			{
				back = valor;
				valor = valor.substring(0,valor.length-1);
				var indice = valor.indexOf('-');
				while(indice != -1)
				{
					valor = valor.substring(indice+1,valor.length);
					indice = valor.indexOf('-');
				}
				if(valor.length != 2)
				{
					if(valor.length != 3)
					{
						if(valor.length != 4)
						{
							alert("Valor Invalido. La tercera parte de la referencia mazda no puede ser diferente de 2,3 o 4");
							objTb.value = back.substring(0,back.length-1);
						}
					}
				}
			}
			//Si el contador es igual a 3 y la ultima el ultimo caracter es diferente a 109. 
			if(contadorGuiones == 3 && ultimoCaracter != '-')
			{
				back = valor;
				var indice = valor.indexOf('-');
				while(indice != -1)
				{
					valor = valor.substring(indice+1,valor.length);
					indice = valor.indexOf('-');
				}
				if(valor.length>2)
					objTb.value = back.substring(0,back.length-2)+"-"+back.substring(back.length-2,back.length-1)+ultimoCaracter;
			}
			//Si el contador es igual a 4 y la ultima el ultimo caracter es igual a 109. 
			if(contadorGuiones == 4 && ultimoCaracter != '-')
			{
				back = valor;
				var indice = valor.indexOf('-');
				while(indice != -1)
				{
					valor = valor.substring(indice+1,valor.length);
					indice = valor.indexOf('-');
				}
				if(valor.length>2)
				{
					alert("Valor Maximo Alcanzado");
					objTb.value = back.substring(0,back.length-1);
				}
			}
			//Si el contador es igual a 4 y la ultima el ultimo caracter es igual a 109. 
			if(contadorGuiones == 4 && ultimoCaracter == '-')
			{
				back = valor;
				valor = valor.substring(0,valor.length-1);
				var indice = valor.indexOf('-');
				while(indice != -1)
				{
					valor = valor.substring(indice+1,valor.length);
					indice = valor.indexOf('-');
				}
				if(valor.length>1)
				{
					alert("Cuando la referencia se divide en 5; la cuarta parte de la referencia debe ser de longitud 1");
					objTb.value = back.substring(0,back.length-1);
				}
			}
			//Si el contador es mayor a 4
			if(contadorGuiones>4)
			{
				alert("Valor Maximo Alcanzado");
				objTb.value = valor.substring(0,valor.length-1);
			}
		}
		///FIN MANEJO DE REFERENCIAS MAZDA
		//INICIO DE MANEJO DE REFERENCIAS HYUNDAY-TOYOTA
		if(objCmb.value == 'HY')
		{
			//Debemos determinar cuantos guiones se han ingresado 
			var contadorGuiones = 0;
			var indice = valor.indexOf('-');
			while(indice != -1)
			{
				contadorGuiones = contadorGuiones + 1;
				valor = valor.substring(indice+1,valor.length);
				indice = valor.indexOf('-');
			}
			valor = back;
			//Si la cantidad de guiones es superior a 1 debemos informar del error
			if(contadorGuiones <= 1)
			{
				if(contadorGuiones == 0 && ultimoCaracter != '-')
				{
					//Debemos determinar si es la longitud del texto ingresada es mayor a 5 o no y si el ultimo caracter es -
					if(valor.length>5)
						objTb.value = valor.substring(0,5)+'-'+valor.substring(5,valor.length);
				}
				if(contadorGuiones == 1 && ultimoCaracter == '-')
				{
					valor = valor.substring(0,valor.length-1);
					if(valor.length != 5)
					{
						alert("La parte inicial de la referencia debe ser extrictamente de longitud 5");
						objTb.value = valor;
					}
				}
			}
			else
			{
				alert("La cantidad de guiones ingresada es invalida. No puede ser mayor a 1");
				objTb.value = valor.substring(0,valor.length-1);
			}
		}
		//FIN MANEJO DE REFERENCIAS HYUNDAY-TOYOTA
		//INICIO MANEJO DE REFERENCIAS FORD
		if(objCmb.value == 'FR')
		{
			//Debemos determinar cuantos guiones se han ingresado 
			var contadorGuiones = 0;
			var indice = valor.indexOf('-');
			while(indice != -1)
			{
				contadorGuiones = contadorGuiones + 1;
				valor = valor.substring(indice+1,valor.length);
				indice = valor.indexOf('-');
			}
			valor = back;
			if(contadorGuiones == 0)
			{
				if(valor.length > 10)
					objTb.value = valor.substring(0,10)+'-'+valor.substring(10,valor.length);
			}
			if(contadorGuiones == 1 && ultimoCaracter != '-')
			{
				back = valor;
				var indice = valor.indexOf('-');
				while(indice != -1)
				{
					valor = valor.substring(indice+1,valor.length);
					indice = valor.indexOf('-');
				}
				if(valor.length>10)
					objTb.value = back.substring(0,back.length-1)+'-'+ultimoCaracter;
			}
			if(contadorGuiones == 2 && ultimoCaracter == '-')
			{
				back = valor;
				valor = valor.substring(0,valor.length-1);
				var indice = valor.indexOf('-');
				while(indice != -1)
				{
					valor = valor.substring(indice+1,valor.length);
					indice = valor.indexOf('-');
				}
				if(valor.length<1 || valor.length>10)
				{
					alert("En las referencias tipo Ford la segunda parte debe tener una longitud mayor que 0 y menor igual que 10");
					objTb.value = back.substring(0,back.length-1);
				}
			}
			if(contadorGuiones == 2 && ultimoCaracter != '-')
			{
				back = valor;
				var indice = valor.indexOf('-');
				while(indice != -1)
				{
					valor = valor.substring(indice+1,valor.length);
					indice = valor.indexOf('-');
				}
				if(valor.length>8)
				{
					alert("Valor Maximo Alcanzado");
					objTb.value = back.substring(0,back.length-1);
				}
			}
			if(contadorGuiones == 3)
			{
				alert("Valor Maximo Alcanzado");
				objTb.value = valor.substring(0,valor.length-1);
			}
		}
		//FIN MANEJO DE REFERENCIAS FORD
		//INICIO MANEJO DE REFERENCIAS CHEVROLET
		if(objCmb.value == 'CH')
		{
			if(valor.length == 1)
				objTb.value = '0000000000000000'+ultimoCaracter;
			else
			{
				if(valor.length == 18)
					objTb.value = valor.substring(1,valor.length);
			}
		}
		//FIN MANEJO DE REFERENCIAS CHEVROLET
		//INICIO MANEJO DE REFERENCIAS GENERALES
		if(objCmb.value == 'GR')
			objTb.value = valor;
		//FIN MANEJO DE REFERENCIAS GENERALES
    }
   	else if(charCode == 38 || charCode == 40 || charCode == 37 || charCode == 39 || charCode == 36 || charCode == 35 || charCode == 33 || charCode == 34 || charCode == 16)
   	{
   		objTb.value = valor;
   	}
   	else if(charCode == 8)
   	{
   		if(objCmb.value == 'CH')
			objTb.value = valor.substring(0,valor.length)+'0';
   	}
}
//***************************************************************************
//***************************************************************************
//FIN MASCARA PARA DAR FORMATO A LAS REFERENCIAS DE LOS ITEMS DE ACUEDO A SU LINEA DE BODEGA
//***************************************************************************
//***************************************************************************
//EVENTO PARA MANEJAR EL CAMBIO DEL DROPDOWNLIST DE LAS LINEAS Y DAR FORMATO AL TEXTO DEL CUADRO DEL TEXTO 
function ChangeLine(objCmb,objTb)
{
	var valor = objTb.value;
   	var back = valor;
   	var base = valor;
	if(valor.length>0)
	{
		var indice = valor.indexOf('-');
		//Debemos eliminar los guiones que encontremos
		while(indice != -1)
		{
			base = base.substring(0,indice)+base.substring(indice+1,base.length);
			indice = base.indexOf('-');
		}
		back = base;
		//Ahora en base tenemos el valor sin guiones y ahora debemos darle un formato de acuerdo 
		//Si la referencia es mazda 
		if(objCmb.value == 'MZ')
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
		//Si la referencia es hyunday-toyota
		if(objCmb.value == 'HY')
		{
			if(back.length > 5)
				base = base.substring(0,5)+'-'+base.substring(5,base.length);
		}
		//Si la referencia es ford
		if(objCmb.value == 'FR')
		{
			if(back.length > 10)
				base = base.substring(0,10)+'-'+base.substring(10,base.length);
			if(back.length > 20)
				base = base.substring(0,21)+'-'+base.substring(21,base.length);
			if(back.length > 28)
				base = base.substring(0,30);
		}
		//Si la referencia es Chevrolet
		if(objCmb.value == 'CH')
		{
			var zeros = '00000000000000000';
			if(back.length >= 17)
				base = base.substring(0,17);
			else
				base = zeros.substring(0,17-base.length)+base;
		}
		//Si la referencia es Generica
		if(objCmb.value == 'GR')
			base = base;
		objTb.value = base;
	}
}
//***************************************************************************
//FIN EVENTO PARA MANEJAR EL CAMBIO DEL DROPDOWNLIST DE LAS LINEAS Y DAR FORMATO AL TEXTO DEL CUADRO DEL TEXTO 

//***************************************************************************
//MANEJO DE OTROS EVENTOS DE TECLADO PARA PEDIDOS DE CLIENTE
function CalcularValorBaseDescuento(objDescuento,objValBasVeh,objValIvaVeh,objValVeh,objTotalVehiculo,objOtrosElementos,objTotalVenta,valorVehiculo,iva,objHdValorVehiculo,objrecordDescuento,objDescuentoA)
{
			
	iva2="1."+iva;
	NumericMask(objDescuento);
		
	if(objDescuento.value.length>0)
	{
		objrecordDescuento.value=objDescuento.value;
		objDescuentoA.value=objDescuento.value;
		var stringValorDescuento = EliminarComas(objDescuento.value);		
		var valorDescuento = parseFloat(stringValorDescuento);
		var stringTotalVehiculo = EliminarComas(objHdValorVehiculo.value);		
		stringTotalVehiculo=stringTotalVehiculo.substring(1,objHdValorVehiculo.length);
		var TotalVehiculo = parseFloat(stringTotalVehiculo);
		objValVeh.value =String(TotalVehiculo-valorDescuento);
		objValVeh.value = Math.round(objValVeh.value);
		objValBasVeh.value = objValVeh.value/iva2;
		objValIvaVeh.value = (objValBasVeh.value *iva)/100;
	}
	else
	{
		
		var stringTotalVehiculo = EliminarComas(objHdValorVehiculo.value);		
		stringTotalVehiculo=stringTotalVehiculo.substring(1,stringTotalVehiculo.length);
		var TotalVehiculo = parseFloat(stringTotalVehiculo);
		objValBasVeh.value = TotalVehiculo/iva2;
		objValIvaVeh.value = (objValBasVeh.value *iva)/100;
		objValVeh.value =Math.round(TotalVehiculo);
		objrecordDescuento.value=0;
		objDescuentoA.value=0;
		
				
	}
	ApplyNumericMask(objValBasVeh);
	ApplyNumericMask(objValIvaVeh);
	ApplyNumericMask(objValVeh);
	objValBasVeh.value = "$"+objValBasVeh.value;
	objValIvaVeh.value = "$"+objValIvaVeh.value;
	objValVeh.value = "$"+objValVeh.value;
	objTotalVehiculo.value = objValVeh.value;	
	CalculoTotalVenta(objTotalVehiculo,objOtrosElementos,objTotalVenta);
}


function CalculoTotalVenta(objTotalVehiculo,objOtrosElementos,objTotalVenta)
{
	var stringValorVehiculo = EliminarComas(objTotalVehiculo.value.substring(1,objTotalVehiculo.value.length));
	var stringValorOtros = EliminarComas(objOtrosElementos.value.substring(1,objOtrosElementos.value.length));
	var valorVehiculo = parseFloat(stringValorVehiculo);
	var valorOtros = parseFloat(stringValorOtros);
	objTotalVenta.value = String(valorVehiculo);
	objTotalVenta.value = String(valorVehiculo+valorOtros);
	objTotalVenta.value = Math.round(objTotalVenta.value);
	ApplyNumericMask(objTotalVenta);
	objTotalVenta.value = "$"+objTotalVenta.value;
}

function CalculoTotalVenta2(objValorDescuento,objValorVehiculo,objValorIVAVehiculo,objValorElementosVenta,objValorIVAElementosVenta,objValorTotalVenta,valorVehiculo,iva)
{
    //ApplyNumericMask(objValorDescuento);
	var stringValorDescuento = EliminarComas(objValorDescuento.value);
	var stringValorElementosVenta = EliminarComas(objValorElementosVenta.value.substring(1,objValorElementosVenta.value.length));
	var stringValorIVAElementosVenta = EliminarComas(objValorIVAElementosVenta.value.substring(1,objValorIVAElementosVenta.value.length));
	if(stringValorDescuento.length > 0)
	{
		var valorDescuento = parseFloat(stringValorDescuento);
		objValorVehiculo.value = String(valorVehiculo);
		objValorIVAVehiculo.value = String((valorVehiculo)*(iva/100));
	}
	else
	{
		objValorVehiculo.value = String(valorVehiculo - 0);
		objValorIVAVehiculo.value = String((valorVehiculo - 0)*(iva/100));
	}
	ApplyNumericMask(objValorVehiculo);
	ApplyNumericMask(objValorIVAVehiculo);
	objValorVehiculo.value = "$"+objValorVehiculo.value;
	objValorIVAVehiculo.value = "$"+objValorIVAVehiculo.value;
	//Traemos el valor de los elementos de venta y el iva
	var stringValorVehiculo = EliminarComas(objValorVehiculo.value.substring(1,objValorVehiculo.value.length));
	var stringValorIVAVehiculo = EliminarComas(objValorIVAVehiculo.value.substring(1,objValorIVAVehiculo.value.length));
	var stringValorElementosVenta = EliminarComas(objValorElementosVenta.value.substring(1,objValorElementosVenta.value.length));
	var stringValorIVAElementosVenta = EliminarComas(objValorIVAElementosVenta.value.substring(1,objValorIVAElementosVenta.value.length));
	var valorVehiculo = parseFloat(stringValorVehiculo);
	var valorIVAVehiculo = parseFloat(stringValorIVAVehiculo);
	var valorElementosVenta = parseFloat(stringValorElementosVenta);
	var valorIVAElementosVenta = parseFloat(stringValorIVAElementosVenta);
	objValorTotalVenta.value = String(valorVehiculo+valorIVAVehiculo+valorElementosVenta+valorIVAElementosVenta);
	ApplyNumericMask(objValorTotalVenta);
	objValorTotalVenta.value = "$"+objValorTotalVenta.value;
}

function CalculoTotalPagos(objEfectivo,objFinanciado,objCheques,objOtraForma,objParcial,objRetoma,objTotal,objDiferencia,totalVenta,indicativo)
{
	if(String(indicativo) == "1")
		NumericMask(objEfectivo);
	if(String(indicativo) == "2")
		NumericMask(objFinanciado);
	if(String(indicativo) == "3")
		NumericMask(objCheques);
	if(String(indicativo) == "4")
		NumericMask(objOtraForma);
	var stringEfectivo = EliminarComas(objEfectivo.value);
	var stringFinanciado = EliminarComas(objFinanciado.value);
	var stringCheques = EliminarComas(objCheques.value);
	var stringOtraForma = EliminarComas(objOtraForma.value);
	var stringRetoma = EliminarComas(objRetoma.value.substring(1,objRetoma.value.length));
	var valorEfectivo = 0;
	var valorFinanciado = 0;
	var valorCheques = 0;
	var valorOtrasFormas = 0;
	var valorRetomas = 0;
	if(stringEfectivo.length > 0)
		valorEfectivo = parseFloat(stringEfectivo);
	if(stringFinanciado.length > 0)
		valorFinanciado = parseFloat(stringFinanciado);
	if(stringCheques.length > 0)
		valorCheques = parseFloat(stringCheques);
	if(stringOtraForma.length > 0)
		valorOtrasFormas = parseFloat(stringOtraForma);
	if(stringRetoma.length > 0)
		valorRetomas = parseFloat(stringRetoma);
	objParcial.value = String(valorEfectivo+valorFinanciado+valorCheques+valorOtrasFormas);
	objTotal.value = String(valorEfectivo+valorFinanciado+valorCheques+valorOtrasFormas+valorRetomas);
	ApplyNumericMask(objParcial);
	ApplyNumericMask(objTotal);
	objParcial.value = "$"+objParcial.value;
	objTotal.value = "$"+objTotal.value;
	CalculoDiferenciaPagos(objTotal,objDiferencia,totalVenta);
}

function CalculoDiferenciaPagos(objTotal,objDiferencia,totalVenta)
{
	var stringTotalPagos = EliminarComas(objTotal.value.substring(1,objTotal.value.length));
	var stringTotalVenta = totalVenta;
	var valorTotalPagos = 0;
	var valorTotalVenta = parseFloat(stringTotalVenta);
	if(stringTotalPagos.length > 0)
		valorTotalPagos = parseFloat(stringTotalPagos);
	var diferencia = valorTotalVenta - valorTotalPagos;
	objDiferencia.value = String(diferencia);
	ApplyNumericMask(objDiferencia);
	if(diferencia >= 0)
		objDiferencia.value = "$"+objDiferencia.value;
	else
		objDiferencia.value = "($"+objDiferencia.value+")";
}

//***************************************************************************
//Funcion para eliminar las comas del formato numerico

function EliminarComas(numericValue)
{
	var outPut = numericValue;
	var indice = outPut.indexOf(',');
	while(indice != -1)
	{
		outPut = outPut.substring(0,indice)+outPut.substring(indice+1,outPut.length);
		indice = outPut.indexOf(',');
	}
	return outPut;
}

//***************************************************************************
//Aplicar Mascara Numerica a un valor establecido
function ApplyNumericMask(obj)
{
	var valor = obj.value;
	var valorEntero = "";
	var valorDecimal = "";
	var indicePunto = valor.indexOf('.');
	if(indicePunto != -1)
	{
		valorEntero = valor.substring(0,indicePunto);
		valorDecimal = valor.substring(indicePunto+1,valor.length);
	}
	else
		valorEntero = valor;
	//Ahora determinamos si es necesario aplicar las comas de division
	if(valorEntero.length <= 3)
		obj.value = valorEntero;
	else
	{
		var movil = valorEntero.length-3;
		while(movil>0)
		{
			valorEntero = valorEntero.substring(0,movil)+','+valorEntero.substring(movil,valorEntero.length);
			movil = movil-3;
		}
		obj.value = valorEntero;
	}
	if(valorDecimal.length > 0)
	{
		if(valorDecimal.length <= 2)
			obj.value = obj.value+'.'+valorDecimal;
		else
			obj.value = obj.value+'.'+valorDecimal.substring(0,2);
	}
}


