<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.RemesaManual.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_RemesaManual" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<HR style="WIDTH: 148.66%; HEIGHT: 8px" width="148.66%" color="#000099" SIZE="8">
&nbsp;
<TABLE id="Table1" style="WIDTH: 1079px; HEIGHT: 366px" cellSpacing="0" cellPadding="0"
	width="1079" border="0">
	<TR>
		<TD style="WIDTH: 619px; HEIGHT: 224px"><asp:panel id="Panel1" runat="server" Width="566px">
      <FIELDSET style="WIDTH: 678px; HEIGHT: 246px"><LEGEND>Datos 
Remesa</LEGEND>
      <TABLE id=Table2 style="WIDTH: 662px; HEIGHT: 206px" cellSpacing=0 
      cellPadding=0 width=662 border=0>
        <TR>
          <TD>
<asp:Label id=Label1 runat="server" ForeColor="Black" Font-Bold="True">Numero Remesa</asp:Label></TD>
          <TD style="WIDTH: 201px">
<asp:TextBox id=numerem Width="78px" runat="server"></asp:TextBox>
<asp:RequiredFieldValidator id=RequiredFieldValidator1 runat="server" ErrorMessage="Campo Requerido" ControlToValidate="numerem" Font-Size="XX-Small">Campo Requerido</asp:RequiredFieldValidator></TD>
          <TD>
<asp:Label id=Label4 runat="server" ForeColor="Black" Font-Bold="True">Fecha</asp:Label></TD>
          <TD style="WIDTH: 215px">
<asp:TextBox id=fecharem Width="118px" runat="server"></asp:TextBox>
<asp:RequiredFieldValidator id=RequiredFieldValidator3 runat="server" ErrorMessage="Campo Requerido" ControlToValidate="fecharem" Font-Size="XX-Small">Campo Requerido</asp:RequiredFieldValidator></TD></TR>
        <TR>
          <TD>
<asp:Label id=Label2 runat="server" ForeColor="Black" Font-Bold="True">Planilla</asp:Label></TD>
          <TD style="WIDTH: 201px">
<asp:TextBox id=planilla Width="76px" runat="server"></asp:TextBox>
<asp:RequiredFieldValidator id=RequiredFieldValidator2 runat="server" ErrorMessage="Campo Requerido" ControlToValidate="planilla" Font-Size="XX-Small">Campo Requerido</asp:RequiredFieldValidator></TD>
          <TD></TD>
          <TD style="WIDTH: 215px"></TD></TR>
        <TR>
          <TD style="HEIGHT: 21px">
<asp:Label id=Label3 runat="server" ForeColor="Black" Font-Bold="True">Pais Origen</asp:Label></TD>
          <TD style="WIDTH: 201px; HEIGHT: 21px">
<asp:DropDownList id=PaisO runat="server" AutoPostBack="True"></asp:DropDownList></TD>
          <TD style="HEIGHT: 21px">
<asp:Label id=Label5 runat="server" ForeColor="Black" Font-Bold="True">Pais Destino</asp:Label></TD>
          <TD style="WIDTH: 215px; HEIGHT: 21px">
<asp:DropDownList id=PaisD runat="server" AutoPostBack="True"></asp:DropDownList></TD></TR>
        <TR>
          <TD style="HEIGHT: 15px">
<asp:Label id=Label6 runat="server" ForeColor="Black" Font-Bold="True">Departamento Origen</asp:Label></TD>
          <TD style="WIDTH: 201px; HEIGHT: 15px">
<asp:DropDownList id=DepartamentoO Width="140px" runat="server" AutoPostBack="True"></asp:DropDownList></TD>
          <TD style="HEIGHT: 15px">
<asp:Label id=Label7 runat="server" ForeColor="Black" Font-Bold="True">Departamento Destino</asp:Label></TD>
          <TD style="WIDTH: 215px; HEIGHT: 15px">
<asp:DropDownList id=DepartamentoD Width="140px" runat="server" AutoPostBack="True"></asp:DropDownList></TD></TR>
        <TR>
          <TD>
<asp:Label id=Label8 runat="server" ForeColor="Black" Font-Bold="True">Ciudad Origen</asp:Label></TD>
          <TD style="WIDTH: 201px">
<asp:DropDownList id=CiuO Width="140px" runat="server"></asp:DropDownList></TD>
          <TD>
<asp:Label id=Label9 runat="server" ForeColor="Black" Font-Bold="True">Ciudad Destino</asp:Label></TD>
          <TD style="WIDTH: 215px">
<asp:DropDownList id=CiuD Width="140px" runat="server"></asp:DropDownList></TD></TR>
        <TR>
          <TD style="HEIGHT: 16px">
<asp:Label id=Label10 runat="server" ForeColor="Black" Font-Bold="True">Valor Remesa</asp:Label></TD>
          <TD style="WIDTH: 201px; HEIGHT: 16px">
<asp:TextBox id=valor Width="119px" runat="server" BorderStyle="Dashed" BorderColor="Black" BackColor="Lime"></asp:TextBox></TD>
          <TD style="HEIGHT: 16px">
<asp:Label id=Label11 runat="server" ForeColor="Black" Font-Bold="True">Observaciones</asp:Label></TD>
          <TD style="WIDTH: 215px; HEIGHT: 16px">
<asp:TextBox id=observaciones runat="server" MaxLength="200" TextMode="MultiLine"></asp:TextBox></TD></TR>
        <TR>
          <TD>
<asp:Label id=Label20 runat="server" ForeColor="Black" Font-Bold="True">Placa Bus:</asp:Label></TD>
          <TD style="WIDTH: 201px">
<asp:DropDownList id=Bus Width="112px" runat="server" AutoPostBack="True"></asp:DropDownList></TD>
          <TD>
<asp:Label id=Label21 runat="server" ForeColor="Black" Font-Bold="True">Numero Bus:</asp:Label></TD>
          <TD style="WIDTH: 215px">
<asp:Label id=busN runat="server" ForeColor="Black" Font-Bold="True">NUMERO BUS</asp:Label></TD></TR></TABLE></FIELDSET>
			</asp:panel></TD>
	</TR>
	<tr>
		<TD style="WIDTH: 619px"><asp:panel id="Panel2" runat="server" Height="56px">
      <FIELDSET style="WIDTH: 456px; HEIGHT: 63px"><LEGEND>Datos Emisor</LEGEND>
      <TABLE id=Table3 style="WIDTH: 448px; HEIGHT: 67px" cellSpacing=0 
      cellPadding=0 width=448 border=0>
        <TR>
          <TD>
<asp:Label id=Label12 runat="server" ForeColor="Black" Font-Bold="True">Nombre</asp:Label></TD>
          <TD>
<asp:TextBox id=NombreE runat="server"></asp:TextBox></TD>
          <TD>
<asp:Label id=Label14 runat="server" ForeColor="Black" Font-Bold="True">Identificacion</asp:Label></TD>
          <TD style="WIDTH: 152px">
<asp:TextBox id=IdE runat="server"></asp:TextBox></TD></TR>
        <TR>
          <TD>
<asp:Label id=Label13 runat="server" ForeColor="Black" Font-Bold="True">Telefono</asp:Label></TD>
          <TD>
<asp:TextBox id=TelE runat="server"></asp:TextBox></TD>
          <TD>
<asp:Label id=Label15 runat="server" ForeColor="Black" Font-Bold="True">Direccion</asp:Label></TD>
          <TD style="WIDTH: 152px">
<asp:TextBox id=DirE runat="server"></asp:TextBox></TD></TR>
        <TR>
          <TD></TD>
          <TD></TD>
          <TD></TD>
          <TD style="WIDTH: 152px"></TD></TR></TABLE></FIELDSET>
			</asp:panel></TD>
		<TD><asp:panel id="Panel3" runat="server" Width="448px">
      <FIELDSET style="WIDTH: 456px; HEIGHT: 40px"><LEGEND>Datos 
      Receptor</LEGEND>
      <TABLE id=Table4 style="WIDTH: 448px; HEIGHT: 67px" cellSpacing=0 
      cellPadding=0 width=448 border=0>
        <TR>
          <TD>
<asp:Label id=Label19 runat="server" ForeColor="Black" Font-Bold="True">Nombre</asp:Label></TD>
          <TD>
<asp:TextBox id=NombreR runat="server"></asp:TextBox></TD>
          <TD>
<asp:Label id=Label18 runat="server" ForeColor="Black" Font-Bold="True">Identificacion</asp:Label></TD>
          <TD style="WIDTH: 152px">
<asp:TextBox id=IdR runat="server"></asp:TextBox></TD></TR>
        <TR>
          <TD>
<asp:Label id=Label17 runat="server" ForeColor="Black" Font-Bold="True">Telefono</asp:Label></TD>
          <TD>
<asp:TextBox id=TelR runat="server"></asp:TextBox></TD>
          <TD>
<asp:Label id=Label16 runat="server" ForeColor="Black" Font-Bold="True">Direccion</asp:Label></TD>
          <TD style="WIDTH: 152px">
<asp:TextBox id=DirD runat="server"></asp:TextBox></TD></TR>
        <TR>
          <TD></TD>
          <TD></TD>
          <TD></TD>
          <TD style="WIDTH: 152px"></TD></TR></TABLE></FIELDSET>
			</asp:panel></TD>
	</tr>
</TABLE>
<asp:button id="Guardar" onclick="Grabar_Click" runat="server" Text="Guardar"></asp:button>
<asp:Label id="Label22" runat="server">Label</asp:Label>
