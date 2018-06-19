<%@ Control Language="c#" codebehind="AMS.Automotriz.HojaVida.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.HojaVida" %>

<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type ="text/javascript" src="../js/generales.js"></script>
<script type="text/javascript">
    $(function () {
        //mirar documentacion JQueryUI sobre datepicker
                var fechaVal = $("#<%=txtVenta.ClientID%>").val();
        $("#<%=txtVenta.ClientID%>").datepicker();
        $("#<%=txtVenta.ClientID%>").datepicker("option", "dateFormat", "yy-mm-dd");
        $("#<%=txtVenta.ClientID%>").datepicker("option", "showAnim", "slideDown");
        $("#<%=txtVenta.ClientID%>").datepicker("option", "showOn", "button");
        $("#<%=txtVenta.ClientID%>").datepicker("option", "buttonImage", "../img/AMS.Calendar.png");
        $("#<%=txtVenta.ClientID%>").datepicker("option", "buttonImageOnly", "true");
        $("#<%=txtVenta.ClientID%>").datepicker("option", "buttonText", "Seleccionar Fecha");
        $("#<%=txtVenta.ClientID%>").val(fechaVal);
    });

   function cargarLupa(obj) {
        var txt;
        var variable;
        if (obj == "MCAT_VIN") {
            variable = "MCAT_VIN";
            txt = document.getElementById("<%=txtVin.ClientID%>");
        }
        else if (obj == "MCAT_PLACA") {
            variable = "MCAT_PLACA";
            txt = document.getElementById("<%=txtPlaca.ClientID%>");
        }
        else if (obj == "MCAT_MOTOR") {
            variable = "MCAT_MOTOR";
            txt = document.getElementById("<%=txtMotor.ClientID%>");
        }
        else if (obj == "MNIT_NIT") {
            variable = "MNIT_NIT";
            txt = document.getElementById("<%=txtNit.ClientID%>");
        }
        else if (obj == "MCAT_SERIE") {
            variable = "MCAT_SERIE";
            txt = document.getElementById("<%=txtSerie.ClientID%>");
        }
        else if (obj == "MCAT_CHASIS") {
            variable = "MCAT_CHASIS";
            txt = document.getElementById("<%=txtChasis.ClientID%>");
        }
        ModalDialog(txt, 'SELECT ' + variable + ' FROM MCATALOGOVEHICULO', new Array(), 1);
    }
</script>
<%--<asp:ScriptManager ID="scriptManager1" runat="server" ></asp:ScriptManager>--%>

<fieldset>
<table class="filters">
	<tr>
		<th class="filterHead">
				<img height="100" src="../img/AMS.Flyers.ConsultarHojas.png" border="0">
			</th>
		<td>
			<table class="filters">
				<tr>
					<td colspan="4">
						Datos del vehículo a consultar
					</td>
				</tr>
				<tr>
					<td>VIN :</td>
					<td><asp:TextBox id="txtVin" class="tmediano" runat="server" Width="80%" ondblclick="ModalDialog(this,'SELECT MCAT_VIN FROM MCATALOGOVEHICULO',new Array(),1)"></asp:TextBox>
                    <asp:Image id="Image1" runat="server" ImageUrl="../img/AMS.Search.png" onClick="cargarLupa('MCAT_VIN');"></asp:Image> </td>
					<td>Placa :</td>
					<td><asp:TextBox id="txtPlaca" class="tmediano" runat="server" Width="80%" ondblclick="ModalDialog(this,'SELECT MCAT_PLACA FROM MCATALOGOVEHICULO',new Array(),1)"></asp:TextBox>
                    <asp:Image id="Image2" runat="server" ImageUrl="../img/AMS.Search.png" onClick="cargarLupa('MCAT_PLACA');"></asp:Image> </td>
				</tr>
				<tr>
					<td>Motor :</td>
					<td><asp:TextBox id="txtMotor" class="tmediano"  runat="server" Width="80%" ondblclick="ModalDialog(this,'SELECT MCAT_MOTOR FROM MCATALOGOVEHICULO',new Array(),1)"></asp:TextBox>
                    <asp:Image id="Image3" runat="server" ImageUrl="../img/AMS.Search.png" onClick="cargarLupa('MCAT_MOTOR');"></asp:Image> </td>
					<td>NIT Propietario :</td>
					<td><asp:TextBox id="txtNit" class="tmediano" runat="server" Width="80%" ondblclick="ModalDialog(this,'SELECT MNIT_NIT FROM MCATALOGOVEHICULO',new Array(),1)"></asp:TextBox>
                        <asp:Image id="imglupa4" runat="server" ImageUrl="../img/AMS.Search.png" onClick="cargarLupa('MNIT_NIT');"></asp:Image> </td>
				</tr>
				<tr>
					<td>Serie :</td>
					<td><asp:TextBox id="txtSerie" class="tmediano" runat="server" Width="80%" ondblclick="ModalDialog(this,'SELECT MCAT_SERIE FROM MCATALOGOVEHICULO',new Array(),1)"></asp:TextBox>
                    <asp:Image id="Image5" runat="server" ImageUrl="../img/AMS.Search.png" onClick="cargarLupa('MCAT_SERIE');"></asp:Image> </td>
					<td>Chasis :</td>
					<td><asp:TextBox id="txtChasis" class="tmediano" runat="server" Width="80%" ondblclick="ModalDialog(this,'SELECT MCAT_CHASIS FROM MCATALOGOVEHICULO',new Array(),1)"></asp:TextBox>
                    <asp:Image id="Image6" runat="server" ImageUrl="../img/AMS.Search.png" onClick="cargarLupa('MCAT_CHASIS');"></asp:Image> </td>
				</tr>
				<tr>
					<td>Color :</td>
					<td><asp:dropdownlist id="ddlColor" Runat="server" AutoPostBack="false"></asp:dropdownlist></td>
					<td>Año Modelo :</td>
					<td><asp:TextBox id="txtAno" class="tmediano" runat="server"></asp:TextBox></td>
				</tr>
				<tr>
					<td>Tipo de Servicio :</td>
					<td><asp:dropdownlist id="ddlServicio" Runat="server" AutoPostBack="false"></asp:dropdownlist></td>
					<td>Vencimiento Seguro :</td>
					<td><asp:TextBox id="txtSeguro" onkeyup="DateMask(this)" class="tmediano" runat="server"></asp:TextBox></td>
				</tr>
				<tr>
					<td>Concesionario Vendedor :</td>
					<td><asp:TextBox id="txtConcesionario" class="tmediano" runat="server"></asp:TextBox></td>
					<td>Fecha Venta :</td>
					<td><asp:TextBox id="txtVenta" onkeyup="DateMask(this)" class="tmediano" runat="server"></asp:TextBox></td>                   
				</tr>
				<tr>
					<td>Kilometraje Venta :</td>
					<td><asp:TextBox id="txtKilometrajeV" class="tmediano" runat="server"></asp:TextBox></td>
					<td>Número Radio :</td>
					<td><asp:TextBox id="txtRadio" class="tmediano" runat="server"></asp:TextBox></td>
				</tr>
				<tr>
					<td>Último Kilometraje :</td>
					<td><asp:TextBox id="txtKilometrajeU" class="tmediano" runat="server"></asp:TextBox></td>
					<td>Kilometraje Promedio :</td>
					<td><asp:TextBox id="txtKilometrajeP" class="tmediano" runat="server"></asp:TextBox></td>
				</tr>
				<tr>
					<td>Categoría :</td>
					<td><asp:TextBox id="txtCategoria" class="tmediano" runat="server"></asp:TextBox></td>
				</tr>
				<tr>
					<td>
						<asp:Button id="consultarHoja" onclick="Consultar_Hoja" Text="Consultar Hoja" width = "150"
							runat="server"></asp:Button>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
</fieldset>

			<asp:DataGrid id="dgrCatalogos" runat="server" AutoGenerateColumns="false" OnItemCommand="dgTable_Procesos" GridLines="Vertical" DataKeyField="MCAT_PLACA" cssclass="datagrid">
						<FooterStyle CssClass="footer"></FooterStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="item"></ItemStyle>
				<Columns>
					<asp:TemplateColumn>
						<ItemTemplate>
							<asp:ImageButton id="btEdit" ImageUrl="../img/Edit.jpg" AlternateText="Editar Registro" Runat="server"
								CommandName="Edit" Height="18px"></asp:ImageButton>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="MCAT_VIN" HeaderText="VIN"></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_PLACA" HeaderText="PLACA"></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_MOTOR" HeaderText="MOTOR"></asp:BoundColumn>
					<asp:BoundColumn DataField="MNIT_NIT" HeaderText="PROPIETARIO"></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_SERIE" HeaderText="SERIE"></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_CHASIS" HeaderText="CHASIS"></asp:BoundColumn>
					<asp:BoundColumn DataField="PCOL_CODIGO" HeaderText="COLOR"></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_ANOMODE" HeaderText="AÑO"></asp:BoundColumn>
					<asp:BoundColumn DataField="TSER_TIPOSERV" HeaderText="TIPO"></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_VENCSEGUOBLI" HeaderText="VENCE<BR>SEGURO" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_CONCVEND" HeaderText="CONCESIONARIO"></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_VENTA" HeaderText="VENTA" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_NUMEKILOVENT" HeaderText="KMS<BR>VENTA"></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_NUMERADIO" HeaderText="RADIO"></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_NUMEULTIKILO" HeaderText="KMS<BR>ULT."></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_NUMEKILOPROM" HeaderText="KMS<BR>PROM."></asp:BoundColumn>
					<asp:BoundColumn DataField="MCAT_CATEGORIA" HeaderText="CATEG."></asp:BoundColumn>
				</Columns>
			</asp:DataGrid>

<asp:Label id="lb" runat="server"></asp:Label>
<P></P>

