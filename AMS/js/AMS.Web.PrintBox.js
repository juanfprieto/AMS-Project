/*Print Box*/

function loadSrc()
{
     var params = location.href.substring(location.href.indexOf("codForm"),location.href.length);
     document.all.iFrameModalDialog.src="AMS.Web.PrintBox.aspx?" + params;
}

function DialogBox(codigoFormato, prefDocu, numeDocu)
{
   var modalDialogHeight = "700";
   var modalDialogWidth = "1000";
   //window.open("AMS.Web.PrintBoxIFrame.aspx?codForm=" + codigoFormato + "&prefDocu=" + prefDocu+ "&numeDocu=" + numeDocu + "",null,"dialogHeight=" + modalDialogHeight + "px;dialogWidth=" + modalDialogWidth + "px;");
   window.open("AMS.Web.PrintBox.aspx?codForm=" + codigoFormato + "&prefDocu=" + prefDocu+ "&numeDocu=" + numeDocu + "",null,"Height=" + modalDialogHeight + "px,Width=" + modalDialogWidth + "px");
}
