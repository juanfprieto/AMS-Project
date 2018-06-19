function loadSrc()
{
     var params = location.href.substring(location.href.indexOf("tipProc"),location.href.length);
     document.all.iFrameModalDialog.src="AMS.Web.ModalDialogUbicaciones.aspx?" + params;
}

function ModalDialogUbic(valProc,codAlma,codItem,codOrig,objddlOut)
{
	var modalDialogHeight = "700";
   	var modalDialogWidth = "900";
   	var param;
   	param = showModalDialog("AMS.Web.ModalDialogUbicacionesIFrame.aspx?tipProc=" + valProc + "&codAlma=" + codAlma + "&codItem=" + codItem + "&codOri=" + codOrig,"help","dialogHeight=" + modalDialogHeight + "px;dialogWidth=" + modalDialogWidth + "px");
   	if(param != null)
   	{
   		if(valProc == 'N')
   		{
   			var splitParam = param.split('*');
   			var splitTexto = splitParam[1].split(',');
   			var newOption = new Option(splitTexto[0]+" "+splitTexto[1]+" "+splitTexto[2],splitParam[0]);
   			objddlOut.options.add(newOption);
   			objddlOut.selectedIndex = objddlOut.length - 1;
   		}
   	}
}

function GetValueA(param)
{
    window.returnValue = param;
    window.close();
}
