<%@ Page language="c#" Codebehind="AMS.Comercial.Encomiendas.aspx.cs" AutoEventWireup="false" Inherits="AMS.Comercial.AMS_Comercial_Encomiendas" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
	</HEAD>
	<body onload="javascript:Imprimir();">
		<form runat="server" ID="Form1">
			<OBJECT id="prtEncomienda" height="1" width="1" classid="clsid:2D16B95E-F5BF-47BA-836E-099EC23A2B49"
				VIEWASTEXT>
				<PARAM NAME="_Version" VALUE="65536">
				<PARAM NAME="_ExtentX" VALUE="26">
				<PARAM NAME="_ExtentY" VALUE="26">
				<PARAM NAME="_StockProps" VALUE="0">
			</OBJECT>
			<SCRIPT LANGUAGE="JavaScript">
			function Imprimir(){
				var prtEncomienda=document.getElementById("prtEncomienda");
				prtEncomienda.InputParameter = '<%=txtEncomienda%>';
				prtEncomienda.LoadParameter();
				while(prtEncomienda.OutputParameter.length>0){
					setTimeout("void",1000);
				}
				window.close();
			}
			</SCRIPT>
		</form>
	</body>
</HTML>