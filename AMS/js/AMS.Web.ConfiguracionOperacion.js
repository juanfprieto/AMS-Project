/*Dialog Box*/

function loadSrc()
{
     var params = location.href.substring(location.href.indexOf("tabla"),location.href.length);
     document.all.iFrameModalDialog.src="AMS.Web.ConfiguracionOperacion.aspx?" + params;
}

function CuadroConfiguracion(idResul,tabla,campo)
{
   var modalDialogHeight = "700";
   var modalDialogWidth = "700";
   param = showModalDialog("AMS.Web.ConfiguracionOperacionIFrame.aspx?tabla=" + tabla + "&campo="+campo+"",null,"dialogHeight=" + modalDialogHeight + "px;dialogWidth=" + modalDialogWidth + "px");
   if(param != null)
   {
      document.all[idResul].value= param;
   }  
}

function GetValueA(param){
    window.returnValue = param;
    window.close();
}
function GetValue(param)
{
    window.returnValue = param;
    window.close();
}

