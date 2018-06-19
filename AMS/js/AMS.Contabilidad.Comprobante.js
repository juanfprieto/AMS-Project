/*Comprobante*/
function ManejoTbsCredDeb(obj1,obj2)
{
	if(obj1.value.length > 0)		
	{
		obj2.value = '';
		obj2.readOnly = true;
	}
	else
		obj2.readOnly = false;
}

function retValueNit(valor){
    var arr = showModalDialog( "selNit.htm","","font-family:Verdana; font-size:12; dialogWidth:30em; dialogHeight:15em ; help : no ; status :no;" );
    if(arr!=null){
        document.forma.elements[valor].value=arr;
    }
}

var debito = "";
var credito = "";

function PutDatas(obj, container)
{

  if(container == "sede")
    obj.value = document.all._ctl1_lbSede.value;

  if(container == "centros")
    obj.value = document.all._ctl1_lbCcosto.value;

  if(container == 'detail')
    obj.value = document.all._ctl1_tbDetail.value;

  if(container == 'numRef')
    obj.value = document.all._ctl1_idComp.value;

  if(container == 'docType')
  {
    url = location.href;
    obj.value = url.substring(url.indexOf("typeNum")+8,url.indexOf("&consecutivo="));
  }
  if(container == "debito")
  {
     debito = obj.value;
     obj.blur();
  }
  if(container == "credito")
  {
     credito = obj.value;
     obj.blur();
  }
}

function DandC(input, who)
{
  if(who == "debito" && credito != "")
     input.blur();

  if(who == "credito" && debito != "")
     input.blur();
}

var beforeCal = 0;
var baseInput = null;
function putBase(input)
{
	var cal = 0;
	var base = document.all.tbBase.value;
	baseInput = input;
	//alert(baseInput.value);
	if(debito != "" && base != "0")
           cal = debito*100/base;

	if(credito != "" && base != "0")
           cal = credito*100/base;
    if(!base)cal=0;	
	//input.value = cal.toString().replace(".", ",");
	input.value = cal;
	beforeCal = cal;

	if(beforeCal != 0)
	   input.select();
}

function rightBase(input)
{
	var limSup=0, limInf=0;

	limInf = beforeCal - ((beforeCal*3)/100);
	limSup = beforeCal + ((beforeCal*3)/100);
	if(input.value > limInf && input.value < limSup ||
           input.value.toString().replace(",",".") > limInf && input.value.toString().replace(",",".") < limSup)
	   return true;
	else
	{
	   alert("Base fuera de rango");
	   //input.value = beforeCal.toString().replace(".",",");
	   input.value = beforeCal.toString();
	   return false;
	}
}

function checkBase()
{
	if(baseInput != null && baseInput.value == "0")
       putBase(baseInput);
}
