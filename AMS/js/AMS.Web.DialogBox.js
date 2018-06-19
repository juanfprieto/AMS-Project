/*Dialog Box*/

function loadSrc()
{
     var params = location.href.substring(location.href.indexOf("title"),location.href.length);
     document.all.iFrameModalDialog.src="AMS.Web.DialogBox.aspx?" + params;
}

function DialogBox(title, redirect)
{
   var modalDialogHeight = "225";
   var modalDialogWidth = "400";
   param = showModalDialog("AMS.Web.DialogBoxIFrame.aspx?title=" + title + "",null,"dialogHeight=" + modalDialogHeight + "px;dialogWidth=" + modalDialogWidth + "px");
   if(param != null)
   {
      //location.href="AMS.Web.index.aspx?process="+redirect.substring(0,redirect.length)+"";
      location.href="AMS.Web.index.aspx?process="+redirect+"";
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

