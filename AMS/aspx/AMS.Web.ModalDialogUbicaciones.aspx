<%@ Page Language="c#" Debug="true" autoeventwireup="True" codebehind="AMS.Web.ModalDialogUbicaciones.aspx.cs" Inherits="AMS.Web.ModalDialogUbicaciones" %>
<%@ outputcache duration="1" varybyparam="tipProc" %>
<HTML>
	<HEAD>
		<script language="javascript" src="../js/AMS.Web.ModalDialogUbicaciones.js" type="text/javascript"></script>
		<script language="javascript">
        <%if(Request.QueryString["Vals"]!=null)Response.Write("GetValueA('"+Request.QueryString["Vals"]+"');");%>
		</script>
		<link href="../css/AMS.css" type="text/css" rel="stylesheet">
			<script language="javascript">
				function CerrarVentana(){window.close();}
				function EliminarItemUbi(strParamCont, strParamVal){if(!confirm('Esta Seguro de eliminar esta referencia en esta ubicación?'))return false;__doPostBack(strParamCont,strParamVal);}
				function EliminarUbic(strParamCont, strParamVal){if(!confirm('Esta Seguro de eliminar esta ubicación?'))return false;__doPostBack(strParamCont,strParamVal);}
			</script>
	</HEAD>
	<body>
		<form runat="server">
			<fieldset style=" WIDTH: 98%;HEIGHT: 100px;" >
				<legend>Informacion General</legend>
				<table class="main" style="WIDTH: 594px; HEIGHT: 46px">
					<tbody>
						<tr>
							<td>
								Almacen :
								<asp:DropDownList id="ddlAlmacen" runat="server"></asp:DropDownList>
							</td>
							<td>
								Ubicación Nivel 1 :&nbsp;
								<asp:DropDownList id="ddlUbiNiv1" runat="server"></asp:DropDownList>
							</td>
							<td align="right">
								<asp:Button id="btnMostrar" runat="server" Text="Mostrar "></asp:Button>
							</td>
						</tr>
						<tr>
							<td>
								Codigo Item :
								<asp:Label id="lbCodI" runat="server"></asp:Label></td>
							<td>
								Nombre Item :
								<asp:Label id="lbNomI" runat="server"></asp:Label></td>
							<td>
								Linea de Bodega :
								<asp:Label id="lbLinBod" runat="server"></asp:Label></td>
						</tr>
					</tbody>
				</table>
			</fieldset>
    <asp:PlaceHolder id="plInfoUbiNiv2" runat="server">
			</p>
			<fieldset style=" WIDTH: 98%;HEIGHT: 91px;" >
				<LEGEND>Datos Configuración de 
Ubicación Nivel 2</LEGEND>
				<TABLE class="main">
					<TR>
						<TD align="Justify" colSpan="2">A continuación se solicitara el nombre de la 
							ubicación de nivel 2 que se configurara, y luego escogera un espacio en la 
							grilla de&nbsp;la parte inferior. Por lo general, las ubicaciones de nivel 2 
							corresponden a estanterias, paredes de ubicación o ganchos. La nueva ubicación 
							de nivel 2 pertenece a
							<asp:Label id="lbNomUbiNiv1" runat="server" font-bold="True"></asp:Label>.</TD>
					</TR>
					<TR>
						<TD>Nombre de Ubicación :</TD>
						<TD align="right">
							<asp:TextBox id="tbNomUbiNiv2" runat="server" Width="225px" MaxLength="30" CssClass="AlineacionDerecha"></asp:TextBox></TD>
					</TR>
				</TABLE>
			</FIELDSET></asp:PlaceHolder>
			<P></P>
			<p>
				<asp:PlaceHolder id="plInfoUbiNiv3" runat="server">
			</p>
			<FIELDSET style="WIDTH: 621px; HEIGHT: 48px">
				<LEGEND>Manejo de Filas</LEGEND>
				<TABLE class="main" style="WIDTH: 621px" cellSpacing="5">
					<TR>
						<TD colSpan="3">
							<asp:Label id="lbInfo1" runat="server" forecolor="RoyalBlue" text="AGREGAR FILA"></asp:Label></TD>
					</TR>
					<TR>
						<TD>Nombre Fila :
							<asp:TextBox id="tbNomFil" runat="server" MaxLength="30"></asp:TextBox>&nbsp;</TD>
						<TD>Dividir en&nbsp;
							<asp:TextBox id="tbDivCeldas" runat="server" Width="56px" CssClass="AlineacionDerecha">1</asp:TextBox>&nbsp;cajones</TD>
						<TD align="right">
							<asp:Button id="btnAgrFil" onclick="AgregarFilaNiv2" runat="server" Text="Agregar Fila"></asp:Button></TD>
					</TR>
					<TR>
						<TD align="Justify" colSpan="3">
							<P>Si no ingresa un Nombre para la fila se le asignara automaticamente un valor 
								Alfabetico (A,B,C,etc). El número de cajones debe ser mayor o igual a 1.
							</P>
						</TD>
					</TR>
				</TABLE>
			</FIELDSET>
			</asp:PlaceHolder>
			<P></P>
			<p>
				<asp:DataGrid id="dgUbicaciones" runat="server" GridLines="None" CellPadding="0" CellSpacing="8"
					CssClass="main">
					<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
				</asp:DataGrid>
			</p>
			<p>
				<asp:LinkButton id="lnkVolver" onclick="Volver" runat="server" CssClass="PunteroMano" Visible="False">Volver</asp:LinkButton>
				&nbsp;<asp:Label id="lbCerrar" onclick="CerrarVentana();" runat="server" cssclass="PunteroMano">Cerrar</asp:Label>
			</p>
			<p>
				<asp:Label id="lb" runat="server"></asp:Label>
				<asp:linkbutton id="lnkEliminarItem" runat="server" Visible="False" CommandArgument="jejejejeje">Eliminar 1</asp:linkbutton><asp:linkbutton id="lnkAgregarItem" runat="server" Visible="False">Agregar</asp:linkbutton><asp:linkbutton id="lnkEliminarUbicacion" runat="server" Visible="False">Eliminar 2</asp:linkbutton>
			</p>
		</form>
	</body>
</HTML>
