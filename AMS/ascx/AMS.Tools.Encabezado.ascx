<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Tools.Encabezado.ascx.cs" Inherits="AMS.Tools.Encabezado" %>

 <script language="javascript" type="text/javascript">
    function cambiar(obj) {
        var caja = document.body.clientWidth;
        var pantalla = screen.width;
        if ((pantalla == 1024 && caja >= 1000) || (pantalla == 1280 && caja >= 1256) || (pantalla == 800 && caja >= 776) || (pantalla == 640 && caja >= 664))
        { obj.value = "Poner"; } else { obj.value = "Quitar"; } s = obj.value == "Quitar"; destino = (s) ? 0 : 30; incremento = (s) ? -1 : 1; origen = (s) ? 30 : 0; obj.value = (s) ? "Poner" : "Quitar"; mover(origen, destino, incremento);
    } function mover(origen, destino, incremento) {
        origen += incremento; top.document.getElementById('indexFrameSet').cols = origen + '%,*'; if (origen != destino) { o = origen; d = destino; i = incremento; setTimeout("mover(o,d,i)", 50); }
    }
</script>

<table id="Table" class="filtersIn">
	<tbody>
		<tr>
			<td>
				<h2><font color="#2a2a2a">&nbsp; <input type="image" src="../img/marcos.jpg"  onclick="cambiar(this);" value="Quitar" style="WIDTH:16px;CURSOR:pointer">&nbsp;&nbsp;
						<asp:label id="lbSystemName" runat="server" font-names="Tahoma" font-size="16pt" forecolor="#424242">AMS</asp:label><asp:label id="Label1" runat="server" font-names="Tahoma" font-size="16pt" forecolor="#424242">-</asp:label><asp:label id="lbCompanyName" runat="server" font-names="Tahoma" font-size="16pt" forecolor="#424242">
                Sistema eCAS</asp:label></font></h2>
			</td>
			<td align="right" valign="top">
				<p><font color="#2a2a2a">&nbsp;
						<b><asp:label id="lblUsuario" runat="server" font-names="Tahoma" font-size="10pt" forecolor="#424242">Usuario</asp:label></b>
					</font>
				</p>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				&nbsp;&nbsp;&nbsp;<asp:label id="infoProcess" runat="server" font-names="Arial" font-size="12px" forecolor="RoyalBlue"
						font-bold="True" backcolor="#F2F2F2" cssclass="infoProcess" Width="100%"></asp:label>
			</td>
		</tr>
	</tbody>
</table>
