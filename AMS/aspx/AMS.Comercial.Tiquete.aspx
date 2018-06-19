<%@ Page Language="c#" Debug="true" autoeventwireup="false" codebehind="AMS.Comercial.Tiquete.aspx.cs" Inherits="AMS.Comercial.Tiquete" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
	</HEAD>
	<body onload="javascript:Imprimir();">
		<form runat="server">
			<OBJECT id="prtTiquete" height="1" width="1" classid="clsid:2D16B95E-F5BF-47BA-836E-099EC23A2B49"
				VIEWASTEXT>
				<PARAM NAME="_Version" VALUE="65536">
				<PARAM NAME="_ExtentX" VALUE="26">
				<PARAM NAME="_ExtentY" VALUE="26">
				<PARAM NAME="_StockProps" VALUE="0">
			</OBJECT>
			<SCRIPT LANGUAGE="JavaScript">

			function Imprimir(){
				var prtTiquete=document.getElementById("prtTiquete");
				prtTiquete.InputParameter = '<%=txtTiquete%>';
				prtTiquete.LoadParameter();
				while(prtTiquete.OutputParameter.length>0){
					setTimeout("void",1000);
				}
				window.close();
			}
			</SCRIPT>
		</form>
	</body>
</HTML>
