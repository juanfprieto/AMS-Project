<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Garantias.RegistrodeVentas.ascx.cs" Inherits="AMS.Garantias.AMS_Garantias_RegistrodeVentas" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<P>
	<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
	<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	<asp:panel id="Panel1" Width="480px" runat="server" BorderColor="White">
<P>Agente Vendedor 
<asp:Label id=lblUsuario runat="server" class="lmediano">Label</asp:Label></P>
<P>Nit&nbsp;&nbsp; 
<asp:Label id=lblNitDealer runat="server" class="lpequeno">Label</asp:Label></P>
<P>Ciudad&nbsp; 
<asp:Label id=lblCiudad runat="server" class="lmediano">Label</asp:Label></P>
<TABLE id=Table3 class="filtersIn">
  <TR>
    <TD>Carnet #</TD>
    <TD>
<asp:textbox id=tbCarnet runat="server"></asp:textbox>
<asp:requiredfieldvalidator id=RequiredFieldValidator2 runat="server" Width="8px" ErrorMessage="*" ControlToValidate="tbCarnet"></asp:requiredfieldvalidator></TD></TR></TABLE>
<P>DATOS DEL CLIENTE</P>
<TABLE id=Table2 class="filtersIn">
  <TR>
    <TD>C.C o Nit</TD>
    <TD>
<asp:textbox id=tbNitCliente onclick="ModalDialog(this,'select  mnit_nit as nit , mnit_nombres concat \' \' concat mnit_nombre2 concat \' \' concat mnit_apellidos concat \' \' concat mnit_apellido2 as nombre from mnit', new Array(),1)" runat="server"></asp:textbox>
<asp:requiredfieldvalidator id=RequiredFieldValidator1 runat="server" Width="8px" ErrorMessage="*" ControlToValidate="tbNitCliente"></asp:requiredfieldvalidator></TD></TR>
  <TR>
    <TD>&nbsp;Nombre</TD>
    <TD>
<asp:textbox id=tbNitClientea runat="server" class="tmediano"></asp:textbox></TD></TR></TABLE>
<P>DATOS DEL VEHICULO</P>
<TABLE id=Table1 class="filtersIn">
  <TR>
    <TD bgColor=#ffffff>
      <P>Chasis 
<asp:textbox id=tbChasis runat="server" class="tmediano"></asp:textbox>
<asp:requiredfieldvalidator id=RequiredFieldValidator3 runat="server" Width="8px" ErrorMessage="*" ControlToValidate="tbChasis"></asp:requiredfieldvalidator></P></TD>
    <TD vAlign=bottom bgColor=#ffffff>
<asp:button id=btBuscar runat="server" class="bpequeno" Text="Buscar" onclick="btBuscar_Click"></asp:button></TD></TR>
  <TR>
    <TD bgColor=#f5f5f5>
      <P>Catalogo </P></TD>
    <TD bgColor=#f5f5f5>
<asp:textbox id=tbCatalogo runat="server" class="tmediano"></asp:textbox></TD></TR>
  <TR>
    <TD bgColor=#ffffff>Motor </TD>
    <TD bgColor=#ffffff>
<asp:textbox id=tbMotor runat="server" class="tediano"></asp:textbox></TD></TR>
  <TR>
    <TD bgColor=#f5f5f5>
      <P>Modelo </P></TD>
    <TD bgColor=#f5f5f5>
<asp:textbox id=tbModelo runat="server" class="tmediano"></asp:textbox></TD></TR>
  <TR>
    <TD bgColor=#ffffff>Color </TD>
    <TD bgColor=#ffffff>
<asp:textbox id=tbColor runat="server" class="tmediano"></asp:textbox></TD></TR>
  <TR>
    <TD bgColor=#f5f5f5 colSpan=2>
      <P>
<asp:Label id=Label1 runat="server" class="lmediano">DATOS DE LA VENTA</asp:Label></P></TD></TR>
  <TR>
    <TD bgColor=#ffffff colSpan=2>
      <TABLE id=Table5 class="filtersIn">
        <TR>
          <TD bgColor=#ffffff>Nº de Factura</TD>
          <TD bgColor=#ffffff>
<asp:TextBox id=tbFactura runat="server"></asp:TextBox>
<asp:RegularExpressionValidator id=RegularExpressionValidator1 runat="server" ErrorMessage="#" ControlToValidate="tbFactura" ValidationExpression="\d+"></asp:RegularExpressionValidator></TD>
          <TD style="WIDTH: 82px" bgColor=#ffffff>Fecha de la factura</TD>
          <TD bgColor=#ffffff>
<asp:TextBox id=tbFechaFact onkeyup=DateMask(this); runat="server" class="tpequeno"></asp:TextBox></TD></TR>
        <TR>
          <TD bgColor=#f5f5f5>Valor de la factura</TD>
          <TD bgColor=#f5f5f5>
<asp:TextBox id=tbValofact runat="server"></asp:TextBox>
<asp:RegularExpressionValidator id=RegularExpressionValidator2 runat="server" ErrorMessage="#" ControlToValidate="tbValofact" ValidationExpression="\d+"></asp:RegularExpressionValidator></TD>
          <TD style="WIDTH: 82px" bgColor=#f5f5f5>Fecha de entrega</TD>
          <TD bgColor=#f5f5f5>
            <P>
<asp:textbox id=tbFechaEntrega onkeyup=DateMask(this); runat="server" class="tpequeno"></asp:textbox></P></TD></TR></TABLE>&nbsp; 
<asp:Button id=btIngresar runat="server" class="bpequeno" Text="Ingresar" onclick="btIngresar_Click"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
    </TD></TR></TABLE>
	</asp:panel>
<P></P>
