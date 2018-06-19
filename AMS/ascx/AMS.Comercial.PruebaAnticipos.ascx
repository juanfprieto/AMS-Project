<%@ Control Language="c#" Debug="true" autoeventwireup="false" codebehind="AMS.Comercial.ActualizarAnticipos.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ActualizarAnticipos" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language=javascript>
<!--

//-->
</script>



<script language=javascript src="../js/AMS.Web.Masks.js" type=text/javascript></script>

<script language=javascript src="../js/AMS.Web.ModalDialog.js" 
type=text/javascript></script>

<style type=text/css>TABLE.paginador { BORDER-TOP-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BACKGROUND-COLOR: white; BORDER-RIGHT-WIDTH: 0px }
	</style>

<p>
<script language=JavaScript>
 
</script>

<table cellSpacing=0 cellPadding=0 border=0>
  <tr>
    <td>
      <table class=head style="HEIGHT: 31px" bgColor=#e0e0e0 
      >
        <tbody>
        <tr>
          <td style="WIDTH: 100px" colSpan=1 
            &gt;<b>Documento </B><asp:textbox id=TextNumeroDocumento Width="96px" runat="server"></asp:textbox></td>
          <td style="WIDTH: 74px" colSpan=2 
            &gt;<b>Placa:</B> <asp:textbox id=TxtPlaca ondblclick="ModalDialog(this,'SELECT mcat_placa AS Placa, rtrim(char(mbus_numero)) as numero from DBXSCHEMA.mbusafiliado where testa_codigo>0', new Array(),1);TraerBus(this.value);" Width="80px" runat="server" MaxLength="6" Font-Size="XX-Small"></asp:textbox>
          <td style="WIDTH: 100px" colSpan=3 
            &gt;<b>Agencia </B><asp:dropdownlist id=ddlAgencia runat="server"></asp:dropdownlist></td>
          <td style="WIDTH: 100px" colSpan=4 
            &gt;<b>Concepto </B><asp:dropdownlist id=ddlConcepto runat="server"></asp:dropdownlist></td>
          <td style="WIDTH: 100px" colSpan=5 
            &gt;<b>Responsable </B><asp:textbox id=TxtResponsable ondblclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS NOMBRE from DBXSCHEMA.MNIT MNIT', new Array(),1)" Width="80px" runat="server" Font-Size="XX-Small" ReadOnly="False"></asp:textbox>
          <td style="WIDTH: 100px" colSpan=6 &gt;<b>Fecha 
            </B><asp:textbox id=TextFecha onkeyup=DateMask(this) Width="88px" runat="server"></asp:textbox></td>
          <td style="WIDTH: 100px" colSpan=7 
            &gt;<b>Planilla </B><asp:textbox id=TextPlanilla Width="80px" runat="server"></asp:textbox></td></tr>
        <TR>
          <TD align=center colSpan=2><asp:button id=btnBuscar Font-Size="XX-Small" Text="Buscar" Runat="server" Font-Bold="True"></asp:button></TD></TR></tbody></table></td></tr>
  <tr>
    <td><asp:placeholder id=toolsHolder 
       runat="server" visible="false">
      <TABLE class=tools width=780>
        <TR>
          <TD width=16><IMG height=30 src="../img/AMS.Flyers.Tools.png" 
            border=0></TD>
          <TD width=380></TD></TR></TABLE></asp:placeholder></td></tr>
  <tr>
    <td><asp:label id=lbInfo runat="server"></asp:label></td></tr>
  <tr>
    <td>&lt;<br>
<asp:Panel Runat="server" Visible="False" ID="pnlAnticipos">
      <DIV id=divItems 
      style="BORDER-TOP: black 1px solid; OVERFLOW: auto; WIDTH: 780px; HEIGHT: 500px" 
      align=left>
  <asp:datagrid id=dgrAnticipos runat="server" PageSize="30" AllowPaging="True" AutoGenerateColumns="False" ShowFooter="False">

		 <AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
		  <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
		  <HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
		  <Columns>
			<asp:TemplateColumn>
				<ItemTemplate>
					<asp:ImageButton id="btnEditar" ImageUrl="../img/Edit.jpg" AlternateText="Editar Registro" Runat="server" CommandName="Actualizar" Height="18px"></asp:ImageButton>
				</ItemTemplate>
			</asp:TemplateColumn>
			
			<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NUM_DOCUMENTO" HeaderText="DOCUMENTO"></asp:BoundColumn>
			<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="FECHA_DOCUMENTO" HeaderText="FECHA"></asp:BoundColumn>
			<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="CODIGO_AGENCIA" HeaderText="CDGO"></asp:BoundColumn>
			<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_AGENCIA" HeaderText="AGENCIA"></asp:BoundColumn>
			<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="MCAT_PLACA" HeaderText="PLACA"></asp:BoundColumn>
			<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="BUS" HeaderText="BUS"></asp:BoundColumn>
			<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="MPLA_CODIGO" HeaderText="PLANILLA"></asp:BoundColumn>
			<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="CODIGO_CONCEPTO" HeaderText="CDGO"></asp:BoundColumn>
			<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NOMBRE_CONCEPTO" HeaderText="CONCEPTO"></asp:BoundColumn>
			<asp:BoundColumn ItemStyle-HorizontalAlign="Left" DataField="NIT_RESPONSABLE" HeaderText="NIT RESPNSBLE"></asp:BoundColumn>
			<asp:BoundColumn ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" DataField="VALOR_GASTO" HeaderText="VALOR"></asp:BoundColumn>
		</Columns>
	</asp:datagrid></DIV></asp:Panel>
<div align=center>
<table class=paginador cellSpacing=0 cellPadding=0 border=0>
  <tr>
    <td vAlign=middle align=right width="45%" 
      ><asp:placeholder id=plcPaginacionIS 
       runat="server"></asp:placeholder></td>
    <td vAlign=middle align=center><asp:label id=lblPaginaActual runat="server"></asp:label></td>
    <td vAlign=middle align=left width="40%" 
      ><asp:placeholder id=plcPaginacionDS 
       runat="server"></asp:placeholder></td></tr>
  <tr>
    <td vAlign=middle align=right width="45%" 
      ><asp:placeholder id=plcPaginacionII 
       runat="server"></asp:placeholder></td>
    <td vAlign=middle align=center>&nbsp;</td>
    <td vAlign=middle align=left width="40%" 
      ><asp:placeholder id=plcPaginacionDI 
       
runat="server"></asp:placeholder></td></tr></table></div><br>
<DIV id=scroll1 
style="OVERFLOW: auto; WIDTH: 632px; LINE-HEIGHT: 0px; HEIGHT: 20px" 
onscroll=OnScroll(this);>
<DIV id=spacing1 style="VISIBILITY: hidden">&nbsp;</DIV></DIV>
<p><asp:label id=lblResult runat="server"></asp:label></p>
<p><asp:textbox id=txtSort runat="server" Visible="False"></asp:textbox><asp:textbox id=txtFilt runat="server" Visible="False"></asp:textbox><asp:textbox id=likestr runat="server" Visible="False"></asp:textbox><asp:table id=Table1 runat="server"></asp:table></p>
<p><asp:datagrid id=Datagrid1 Width="700px" runat="server" ShowFooter="False" AutoGenerateColumns="False">
	  <AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
		  <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" BackColor="#EEEEEE"></ItemStyle>
		  <HeaderStyle Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
	</asp:datagrid></p>
      <DIV></DIV></td>
    <P></P></tr>
  <P></P>
  <P></P></p>
