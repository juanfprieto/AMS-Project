<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Vehiculos.RelacionarDocumentos.ascx.cs" Inherits="AMS.Automotriz.AMS_Automotriz_RelacionarDocumentos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:label id="GirosLabel" Font-Size="Medium" Font-Bold="True" runat="server">Ingreso Documentos Vehiculos</asp:label>

<fieldset>
<table id="Table1" class="filtersIn">
	<TR>
		<TD>
			<asp:Label id="Label1" runat="server">Vehiculos:</asp:Label></TD>
            </tr>
            <tr>
		<TD>
			<asp:DropDownList id="VehiDrop" class="dmediano" runat="server"></asp:DropDownList><asp:Image id="imglupa" runat="server" ImageUrl="../img/AMS.Search.png"></asp:Image></TD>
	</TR>
	<TR>
		<TD>
			<asp:Button id="Generar" runat="server" Text="Generar" OnClick="Generar_Click"></asp:Button></TD>
		<TD></TD>
	</TR>
</TABLE>
<asp:Panel id="Panel1" runat="server" Visible="False">

<P>
<TABLE id=Table2 cellSpacing=0 cellPadding=0 border=0>
  <TR>
    <TD>
<asp:Label id=Label2 runat="server" Font-Bold="True" ForeColor="Black">Vehículo:</asp:Label></TD>
    <TD>
<asp:Label id=VehiLabel runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD></TR>
  <TR>
    <TD>
<asp:Label id=Label3 runat="server" Font-Bold="True" ForeColor="Black">Propietario:</asp:Label></TD>
    <TD>
<asp:Label id=PropLabel runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD></TR>
  <TR>
    <TD>
<asp:Label id=Label6 runat="server" Font-Bold="True" ForeColor="Black">Modelo:</asp:Label></TD>
    <TD>
<asp:Label id=ModeloLabel runat="server" Font-Bold="True" ForeColor="Red">Label</asp:Label></TD></TR>
  <TR>
    <TD>
      <HR style="WIDTH: 148.66%; HEIGHT: 8px" width="148.66%" color=#000099 
      SIZE=8>
      &nbsp;</TD>
    <TD></TD></TR>
  <TR>
    <TD>
<asp:Label id=Label11 runat="server" Font-Bold="True" ForeColor="Red">Imagenes de Referencia</asp:Label></TD>
    <TD></TD></TR>
  <TR>
    <TD>
<asp:Label id=Label4 runat="server" Font-Bold="True" ForeColor="Black">SOAT:</asp:Label></TD>
    <TD>
<asp:TextBox id=soat runat="server"></asp:TextBox></TD></TR>
  <TR>
    <TD>
<asp:Label id=Label5 runat="server" Font-Bold="True" ForeColor="Black">Tarjeta de Propiedad:</asp:Label></TD>
    <TD>
<asp:TextBox id=propiedad runat="server"></asp:TextBox></TD></TR>
  <TR>
    <TD>
<asp:Label id=Label7 runat="server" Font-Bold="True" Visible="False" ForeColor="Black">Poliza de Garantia:</asp:Label></TD>
    <TD>
<asp:TextBox id=pgarantia runat="server"></asp:TextBox></TD></TR>
  <TR>
    <TD>
<asp:Label id=Label8 runat="server" Font-Bold="True" ForeColor="Black">Certificado de Empadronamiento:</asp:Label></TD>
    <TD>
<asp:TextBox id=dimportacion runat="server"></asp:TextBox></TD></TR>
  <TR>
    <TD>
<asp:Label id=Label9 runat="server" Font-Bold="True" ForeColor="Black">Improntas:</asp:Label></TD>
    <TD>
<asp:TextBox id=improntas runat="server"></asp:TextBox></TD></TR>
  <TR>
    <TD>
<asp:Label id=Label10 runat="server" Font-Bold="True" ForeColor="Black">Recepción Vehículo:</asp:Label></TD>
    <TD>
<asp:TextBox id=pbateria runat="server"></asp:TextBox></TD></TR>
  <TR>
    <TD>
<asp:Label id=Label12 runat="server" Font-Bold="True" ForeColor="Black">Factura:</asp:Label></TD>
    <TD>
<asp:TextBox id=facturaV runat="server"></asp:TextBox></TD></TR>
  <TR>
    <TD>
<asp:Label id=Label13 runat="server" Font-Bold="True" ForeColor="Black">Inventario de Entraga Vehículo:</asp:Label></TD>
    <TD>
<asp:TextBox id=inventarioentrega runat="server"></asp:TextBox></TD></TR>
  <TR>
    <TD></TD>
    <TD></TD></TR>
  <TR>
    <TD>
<asp:Button id=Guardar onclick=Guardar_Click runat="server" Text="Guardar"></asp:Button></TD>
    <TD></TD></TR>
    </TABLE>
    </P>
</asp:Panel>
</fieldset>
