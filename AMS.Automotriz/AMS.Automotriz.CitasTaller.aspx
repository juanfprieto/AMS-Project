<%@ Page language="c#" Codebehind="AMS.Automotriz.CitasTaller.aspx.cs" AutoEventWireup="True" Inherits="AMS.Automotriz.AMS_Automotriz_CitasTaller" %>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
	<title>AMS - CITAS TALLER</title>
	<LINK href="../css/AMS.Automotriz.CitasTaller.css" type="text/css" rel="stylesheet">
	<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
	<meta content="C#" name="CODE_LANGUAGE">
	<meta content="JavaScript" name="vs_defaultClientScript">
	<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../css/normalize.css" type="text/css" rel="stylesheet" />
    <link href="../css/citasTabla.css" type="text/css" rel="stylesheet" />
     <link href="../css/ams.css" type="text/css" rel="stylesheet" />
    <script src="../js/jquery.js"></script>
    <script src="../js/citasScroll.js"></script>
    <script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
    <script type="text/javascript">
        var intervalo = 60000; //milisegundos cambio de pantalla planning.
        var idInt; //temporizador.

        $(document).on("ready", inicio);

        function inicio() {
            
            //Temporizador para cambio de paginas.
            var redirigir = "<%= Request.QueryString["rdr"]%>";
            var activado = "<%= Session["INICIAROTAR"]%>";

            if (redirigir == "1" || activado == "1") 
            {
                clearInterval(idInt);
                idInt = setInterval('SetURL()', intervalo);
                starttime();
            }
        }

        function SetURL() 
        {
            clearInterval(idInt);
            document.getElementById('<%=btnPostBack.ClientID %>').click();
        }

    </script>
    
</HEAD>
<body>
    <form id="Form1" method="post" runat="server">
        <asp:panel id="pnlFecha" Runat="server" Visible="True">
			<TABLE style="position: fixed; top:270px; left:100px;">
				<TR>
					<TD><P class="titulo">Fecha:</P></TD>
					<TD>&nbsp;</TD>
					<TD align="right"><asp:TextBox id="txtFecha" onkeyup="DateMask(this)" runat="server" width="100"></asp:TextBox></TD>
					<TD>&nbsp;</TD>
					<TD><asp:DropDownList id="ddlTaller" runat="server"></asp:DropDownList></TD>
					<TD>&nbsp;</TD>
					<TD><asp:ImageButton id="imgSeleccionar" runat="server" CssClass="boton" ImageUrl="../img/continuar_cuadrotaller.gif" OnClick="imgSeleccionar_Click"></asp:ImageButton></TD>
				</TR>
			</TABLE>
		</asp:panel>
        <asp:panel id="pnlCitas" Runat="server" Visible="False">
            <div id="citasTaller" runat="server"></div>
			<TABLE style="position: fixed; top:17px; background: rgba(205,205,244,1); left: 25px; width: 95%; z-index: 995; height:20px;">
				<TR>
					<TD align=left>
						<TABLE>
							<TR>
								<TD>
									<P class="fecha"><%=strFecha%></P>
								</TD>
								<TD>&nbsp;</TD>
								<TD><P class="hora"><%=strHora%></P></TD>
							</tr>
						</table>
					</TD>
					<TD>
						<TABLE>
							<TR>
								<TD id="verde">Cumplida</TD>
								<TD id="naranja">Retraso</TD>
								<TD id="rojo">Falla</TD>
								<TD id="amarillo">Pendiente</TD>
							</TR>
						</TABLE>
					</TD>
					<TD align=right>
						<asp:ImageButton id="imgActualizar" runat="server" ImageUrl="../img/actualizar_cuadrotaller.gif"
							CssClass="boton" onclick="imgActualizar_Click"></asp:ImageButton>&nbsp;
						<asp:ImageButton id="imgVolver" runat="server" ImageUrl="../img/atras_cuadrotaller.gif" CssClass="boton" onclick="imgVolver_Click"></asp:ImageButton>
                    </TD>
				</TR>
			</TABLE>
		</asp:panel>
		<asp:panel id="pnlImagenes" class="pnlImagenes" Runat="server" Visible="False">
            <table class="fondo">
                <tr><td align="center"><br><br><br><br><br>
                    <%=strImagen%>
                </td></tr>
            </table>
            <asp:ImageButton id="imgActualizarI" runat="server" Width="0" Height="0" onclick="imgActualizar_Click"></asp:ImageButton>
		</asp:Panel>
        <asp:Button ID="btnPostBack" runat="server" style="display:none;" />
    </form>

	<script type ="text/javascript">

		if(<%=(ViewState["CONTIMAGEN"]==null)?1:ViewState["CONTIMAGEN"]%><=1)btnActualizar=document.getElementById("<%=imgActualizar.ClientID%>");
		else btnActualizar=document.getElementById("<%=imgActualizarI.ClientID%>");
			
		function countdown() {
			nowtime= new Date();
			nowtime=nowtime.getTime();
			secondssinceloaded=(nowtime-starttime)/1000;
			reloadseconds=Math.round(refreshinterval-secondssinceloaded);
			if (refreshinterval>=secondssinceloaded) {
				var timer=setTimeout("countdown()",1000);
			}
			else {
				clearTimeout(timer);
                clearTimeout(scrolldelay);
				if(btnActualizar)
					btnActualizar.click(); 
			} 
		}

	    function stopScroll() {
	        clearTimeout(scrolldelay);
	    }

	</script>

</body>
</HTML>
