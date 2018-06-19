<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Tools.Email.ascx.cs" Inherits="AMS.Tools.AMS_Tools_Email" %>
<style>
    .boxSizeCorreo{
        width: 428px;
    }
    .fontCorreo{
        color:black;
    }
</style>
<fieldset style="padding: 3px; margin: 0" class="boxSizeCorreo">
    <asp:label id="lbInfo" Text="Enviar reporte por Correo:"
			    Visible="true" Runat="server" class="fontCorreo"></asp:label>
    <asp:textbox id="tbMail" runat="server" width="230px" Visible="true" placeholder="miCorreo@ejemplo.com"></asp:textbox>
    <asp:imagebutton id="imgBtnMail" runat="server" Visible="True" ImageUrl="../img/AMS.Icon.Mail.png"
				ToolTip="Enviar Correo" onclick="ImageButton1_Click" style="vertical-align: bottom;"></asp:imagebutton>
	<br>
	<asp:label id="lbMail" runat="server"  visible="true" class="fontCorreo"><font size="1">*Para enviar a varias dírecciones, por favor separelas por comas(,)</font></asp:label>
    <asp:label id="lblResult" runat="server" ></asp:label>
</fieldset>
<asp:label id="lb" runat="server"></asp:label>
