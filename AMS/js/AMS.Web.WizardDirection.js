/*Modal Dialog*/

function loadSrc()
{
    var params = location.href.substring(location.href.indexOf("params"), location.href.length);
    var valD=location.href.substring(location.href.indexOf("valD=")+5, location.href.length);
    document.all.iFrameModalDialog.src="AMS.Web.WizardDirection.aspx?valD="+valD;
}

function WizardDirection(obj)
{
   var modalDialogHeight = "290";
   var modalDialogWidth = "525";

   var control;
   var splitId;
   var newId;
   var param;
   var urlM2 = "AMS.Web.WizardDirection.aspx?valD=" + obj.value.replace(" ", "%20");
   //param = showModalDialog("AMS.Web.WizardDirectionIFrame.aspx?valD="+obj.value.replace(" ", "%20"), "help", "dialogHeight=" + modalDialogHeight + "px;dialogWidth=" + modalDialogWidth + "px");

   $("#dialog").html('<iframe src="' + urlM2 + '" height="240" widht="' + modalDialogWidth + '" frameborder="0" ></iframe>' +
                    '<form name="formName"><input type="hidden" value="' + obj.id + '" name="objeton" id="objetoi"/></form>');
   $("#dialog").css({ 'visibility': 'visible', 'background': 'rgba(198,198,228, 0.7)', 'box-shadow' : '2px 2px 5px #222' });
   $("#dialog").dialog({ modal: true, title: 'Direccion', height: modalDialogHeight, width: modalDialogWidth });
   $(".ui-widget-overlay").css({ background: "#000", opacity: 0.46 });
   $("#dialogI").html = obj;

   if(param != null)
   {
      obj.value = param;
   }
}

function CerrarVentana(obOut) {
    var idObj = window.document.getElementById('objetoi').value;
    var objS = window.document.getElementById(idObj);
    objS.value = obOut;
    $("#dialog").dialog("close");
}


function GetValue(obOut)
{
    window.returnValue = obOut.value;
    window.close();
}
