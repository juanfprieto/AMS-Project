<%@ Control Language="c#" codebehind="AMS.Comercial.Impresion.ascx.cs" autoeventwireup="false" Inherits="AMS.Comercial.Impresion" %>
<link href="../style/AMS.Prints.css" type="text/css" rel="stylesheet">
	<link href="../style/AMS.css" type="text/css" rel="stylesheet">
		<script language="JavaScript">
    function Lista() {
        w=window.open('AMS.DBManager.Reporte.aspx');
    }
		</script>
		<p>
			<asp:PlaceHolder id="toolsHolder" runat="server">
				<TABLE class="tools" width="780">
					<TR>
						<TD width="16"><IMG height="30" src="../img/AMS.Flyers.Tools.png" border="0"></TD>
						<td>Imprimir <A href="javascript: Lista()"><IMG height="18" alt="Imprimir" src="../img/AMS.Icon.Printer.png" width="20" border="0">
							</A>
						</TD>
						<td>&nbsp; &nbsp;Enviar por correo
							<asp:TextBox id="tbEmail" runat="server"></asp:TextBox></TD>
						<td>
							<asp:RegularExpressionValidator id="FromValidator2" style="LEFT: 100px; POSITION: absolute; TOP: 400px" runat="server"
								ErrorMessage="" ControlToValidate="tbEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
							<asp:ImageButton id="ibMail" runat="server" alt="Enviar por email" ImageUrl="../img/AMS.Icon.Mail.jpg"
								BorderWidth="0px"></asp:ImageButton></TD>
						<TD width="380"></TD>
					</TR>
				</TABLE>
			</asp:PlaceHolder>
		</p>
		<p>
			<asp:PlaceHolder id="tiquetes" runat="server"></asp:PlaceHolder>
		</p>
		<p>
			<asp:Label id="lb" runat="server"></asp:Label>
		</p>
