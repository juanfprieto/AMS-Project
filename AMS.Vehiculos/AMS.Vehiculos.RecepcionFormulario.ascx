<%@ Control Language="c#" CodeBehind="AMS.Vehiculos.RecepcionFormulario.ascx.cs" AutoEventWireup="True" Inherits="AMS.Vehiculos.RecepcionFormulario" %>
    
<script type ="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/jquery-ui.js"></script>
<script type ="text/javascript">
    $(document).ready(function ()
    {
        document.getElementById("<%= ddlprefijomazda.ClientID%>").style.visibility = "hidden";
        document.getElementById("<%= lbpref.ClientID%>").style.visibility = "hidden";
        if ($('#' + '<%= chkExcel.ClientID %>').prop('checked'))
        {
            $('#' + '<%= divExcel.ClientID %>').show("fast");
        } else
            $('#' + '<%= divExcel.ClientID %>').hide();

        $('#' + '<%= chkExcel.ClientID %>').click(function ()
        {
            if ($('#' + '<%= chkExcel.ClientID %>').prop('checked'))
            {
                $('#' + '<%= divExcel.ClientID %>').show("fast");
            }
            else
                $('#' + '<%= divExcel.ClientID %>').toggle("fast");
        });
    });
    
    /*function mostrarDocu()
    {
        var checking = document.getElementById('');
        var divsito = document.getElementById('');
        if (checking.checked) {
            divsito.style.visibility = 'visible';
        } else
            divsito.style.visibility = 'hidden';
    }*/
</script>
<script type="text/javascript">     
    function abrirEmergente() 
    // En NUEVOS solo debe cargar los nits de las casas matrices de las cuales se venden vehiculo
    {
        ModalDialog(<%=ddlProv.ClientID%>, "SELECT MPR.mnit_nit as NIT, MNI.mnit_apellidos CONCAT ' ' CONCAT coalesce(MNI.mnit_apellido2,'') CONCAT ' ' CONCAT MNI.mnit_nombres AS PROVEEDOR FROM mnit MNI, mproveedor MPR, PCASAMATRIZ CM WHERE MNI.mnit_nit = MPR.mnit_nit AND MNI.mnit_nit = CM.mnit_nit order by MNI.mnit_apellidos", new Array());
    }
         
    function abrirEmergente1() 
    // En usados solo debe existir el consignante del vehiculo registrado en MNIT y debe poderse crear desde aqui mismo
    {
        ModalDialog(<%=ddlProv.ClientID%>, "SELECT MNI.mnit_nit as nit, MNI.mnit_apellidos CONCAT ' ' CONCAT coalesce(MNI.mnit_apellido2,'') CONCAT ' ' CONCAT MNI.mnit_nombres CONCAT ' ' CONCAT coalesce(MNI.mnit_nombre2,'') as Consignante FROM mnit MNI order by mnit_nit", new Array(),1);
    }

    function abrirEmergente2() 
    // En usados solo debe existir el consignante del vehiculo registrado en MNIT y debe poderse crear desde aqui mismo
    {
            
        ModalDialog(<%=txtProv.ClientID%>, "SELECT MNI.mnit_nit as nit, MNI.mnit_apellidos CONCAT ' ' CONCAT coalesce(MNI.mnit_apellido2,'') CONCAT ' ' CONCAT MNI.mnit_nombres CONCAT ' ' CONCAT coalesce(MNI.mnit_nombre2,'') as Consignante FROM mnit MNI order by mnit_nit", new Array(),1);
    }
        
    function Cargar_Nombre(obj)
    {
        RecepcionFormulario.Cargar_Nombre(obj.value, Cargar_Nombre_CallBack);
    }

    function Cargar_Nombre_CallBack(response)
    {
	    var respuesta=response.value;
	    if(respuesta.Tables[0].Rows.length==0 || respuesta.Tables[1].Rows.length==0)
	    {
	        var ced = document.getElementById("<%=txtProved.ClientID%>");
		    ced.value='';
	    }
	    else
	    {
	        var nombre = document.getElementById("<%=txtProved.ClientID%>");
		    if(respuesta.Tables[1].Rows.length!=0)
		    {
			    if(respuesta.Tables[1].Rows[0].NOMBRE!='')
			    {
				    nombre.value=respuesta.Tables[1].Rows[0].NOMBRE;
                    
			    }
		    }
	    }
    }
    function Cargar_Prefijo(obj)
    {
        RecepcionFormulario.Cargar_Prefijo(obj.value, Cargar_Prefijo_CallBack);
    }
    function Cargar_Prefijo_CallBack(response)
    {
        $("#_ctl1_ddlprefijomazda").hide();
        var Result = response.value;
        var prfmz = document.getElementById('_ctl1_ddlprefijomazda');
        if(Result.Tables[0].Rows.length!=0)
        {
            document.getElementById("<%= ddlprefijomazda.ClientID%>").style.visibility = "visible";
            document.getElementById("<%= lbpref.ClientID%>").style.visibility = "visible";
            $("#_ctl1_ddlprefijomazda").show();
            for(var i = 0; i < Result.Tables[0].Rows.length; i++) {
                var opt = document.createElement('option');
                opt.innerHTML = Result.Tables[0].Rows[i]['2'];
                opt.value = Result.Tables[0].Rows[i]['PDOC_CODIGO'];
                prfmz.appendChild(opt);            
            }
        }
    }
    function Pasar_Prefijo(idPrefijo) 
    {
        document.getElementById("<%=HiPrefijos.ClientID%>").value = idPrefijo.value;
        return true;
    } 
    function Mostrar_Alerta()
    {
        //alert('A continuación se detallarán las instrucciones para subir el archivo Excel: \n\nPaso1: Seleccione la tabla dentro del archivo que desea subir y asígnele el nombre RECEPCION. \nPaso2: Los nombres de los campos deben ser: \nCATALOGO\nCOLOR\nVALOR\nVIN\nMOTOR\nSERIE\nCHASIS\nAÑO MODELO\nTIPO SERVICIO\nNUMERO MANIFIESTO\nFECHA MANIFIESTO formato: (yyyy-mm-dd) ej: 2016-09-19\nNUMERO ADUANA\nFECHA LEVANTE formato: (yyyy-mm-dd) ej: 2016-10-25\nNUMERO LEVANTE\nFECHA ENTRADA formato: (yyyy-mm-dd) ej: 2016-09-19\nFECHA VENCE formato: (yyyy-mm-dd) ej: 2016-09-19\nFECHA DISPONIBLE formato: (yyyy-mm-dd) ej: 2016-09-19\nNUMERO DO\nALMACEN\nNota1: El campo COLOR debe tener el código del color. \nNota2: El campo VALOR debe tener sólo números(sin pesos($) ni comas(,)).');
        alert('Paso1: Seleccione la tabla dentro del archivo que desea subir y asígnele el nombre RECEPCION. \nPaso2: Los nombres de las columnas deben ser: \nCATALOGO, PREFIJO (Documento de la Entrada de Proveedor), FACTURA ej: F-12345, VIN, MOTOR, SERIE, CHASIS, AÑO MODELO, COLOR (Código),  VALOR, TIPO SERVICIO(Código), NUMERO MANIFIESTO formato: (yyyy-mm-dd) ej: 2016-09-19, FECHA MANIFIESTO (formato: yyyy-mm-dd), NUMERO ADUANA, FECHA LEVANTE (formato: yyyy-mm-dd), NUMERO LEVANTE, FECHA ENTRADA (formato: yyyy-mm-dd), FECHA VENCE (formato: yyyy-mm-dd), FECHA DISPONIBLE (formato: yyyy-mm-dd), NUMERO DO, ALMACEN (Código), FECHA RECEPCION (formato: yyyy-mm-dd)\nNota1: El campo VALOR debe tener sólo números(sin pesos($) ni comas(,)).')
    }
</script>
<p>
    <script type="text/javascript" src="../js/AMS.Web.ModalDialog.js"></script>
    <script type="text/javascript" src="../js/AMS.Web.Masks.js"></script>
</p>
<p>
    <asp:PlaceHolder id="vehPedPlHl" runat="server" ></asp:PlaceHolder>
</p>

<fieldset id="fldVehiculos" runat="server">
    <legend class="Legends">Vehículos Relacionados al Pedido</legend>
    <table>
        <tr>
            <td>
                Nuevo:
                <asp:radiobutton id="rbnuevo" groupname="grupo1" runat="server" OnCheckedChanged="llamar_nuevo" autopostback="true"></asp:radiobutton><br />
                Usado:
                <asp:radiobutton id="rbusado" groupname="grupo1" runat="server" OnCheckedChanged="llamar_usado" autopostback="true"></asp:radiobutton>
            </td>
        </tr>
        <tr>
            <td>
                <asp:PlaceHolder id="plhInfoPed" runat="server" visible="false" >
                    <table>
                        <tbody>
                            <tr>
                                <td>
                                    Prefijo del Pedido :
                                    <asp:Label id="prefPed" runat="server"></asp:Label>
                                </td>
                                <td>
                                    Número del Pedido :
                                    <asp:Label id="numPed" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Proveedor :
                                    <asp:Label id="lbPrPed" runat="server"></asp:Label>
                                </td>
                                <td>
                                    Fecha Recepción :
                                    <asp:Label id="lbFchPed" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <td>
                <asp:PlaceHolder id="plhInfoRcp" runat="server" visible="false" >
                    <table>
                        <tbody>
                            <tr>
                                <td>
                                    Proveedor :
                                        <asp:DropDownList id="ddlProv" runat="server" Width="250px" onchange="Cargar_Prefijo(this);"></asp:DropDownList>
                                        <asp:TextBox id="txtProv" runat="server" class="tmediano" visible="false" onblur="Cargar_Nombre(this);"></asp:TextBox>
                                        <asp:Image id="imglupa1" visible="false" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente();"></asp:Image>
                                        <asp:Image id="imglupa2" visible="false" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente1();"></asp:Image>
                                        <asp:Image id="imglupa3" visible="false" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente2();"></asp:Image>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label id="lblnb" Text="Nombre :" runat="server"></asp:Label>
                                    <asp:TextBox id="txtProved" runat="server" Width="266px"></asp:TextBox>
                                </td>
                                    
                            </tr>
                            <tr>
                                <td>
                                    Fecha Recepción :
                                    <asp:Label id="lbFch" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Ubicación de la Recepción :
                                    <asp:DropDownList id="ubicacion" runat="server" class="dmediano"></asp:DropDownList>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox id="chkExcel" runat="server" AutoPostBack="false" Text="Deseo subir un Excel!"/>
                <div id="divExcel" runat="server">
                    <a href="javascript:Mostrar_Alerta();">Desea subir un Excel? Click Aquí!</a><br />
                    <asp:Label ID="lbpref" runat="server" style="display:inline-block;">Proveedor</asp:Label>
                    <asp:HiddenField ID="HiPrefijos"  runat="server" Value=""/>
                    <asp:DropDownList ID="ddlprefijomazda" runat="server" onblur="Pasar_Prefijo(this)"></asp:DropDownList>
                    <input id="filUpl" runat="server" type="file" style="display:inline-block;"/>
                    <asp:Button id="btnCargar" runat="server" Width="90px" Text="Cargar" OnClick="vamoACargarlo"></asp:Button>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <br /><asp:Label ID="lbExcel" runat="server" ></asp:Label>
            </td>
        </tr>
    </table>
</fieldset>
<p>
</p>
<p>
    <asp:DataGrid id="vehPed" runat="server" cssclass="datagrid" OnItemDataBound="vehPedItemDataBound"
        AutoGenerateColumns="false" GridLines="Vertical" ShowFooter="True" OnItemCommand="dgSeleccion_Evento">
        <footerstyle cssclass="footer"></footerstyle>
        <headerstyle cssclass="header"></headerstyle>
        <selecteditemstyle cssclass="selected"></selecteditemstyle>
        <alternatingitemstyle cssclass="alternate"></alternatingitemstyle>
        <itemstyle cssclass="item"></itemstyle>
        <columns>
				<asp:TemplateColumn HeaderText="ID">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "ID") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="CATALOGO">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "CATALOGO") %>
					</ItemTemplate>
					<FooterTemplate>
						<asp:DropDownList id="ddlCatalogo" runat="server"></asp:DropDownList>
					</FooterTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="COLOR">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "COLOR") %>
					</ItemTemplate>
					<FooterTemplate>
						<asp:DropDownList id="ddlColor" class="dmediano" runat="server"></asp:DropDownList>
					</FooterTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="VALOR UNITARIO">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "VALOR","{0:C}") %>
					</ItemTemplate>
					<FooterTemplate>
						<asp:TextBox id="valor" runat="server" class="tmediano" onkeyup="NumericMaskE(this,event)"></asp:TextBox>
						<asp:RequiredFieldValidator id="validatorValor" runat="server" ControlToValidate="valor" Display="Dynamic" Font-Name="Arial" Font-Size="11">*</asp:RequiredFieldValidator>
						<asp:RegularExpressionValidator id="validatorValor2" runat="server" ControlToValidate="valor" Display="Dynamic" Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\.]+">*</asp:RegularExpressionValidator>
					</FooterTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="ACCI&#211;N">
					<ItemTemplate>
						<asp:CheckBox id="chkAcc" Text="Recibido" runat="server" TextAlign="Left"></asp:CheckBox>
					</ItemTemplate>
					<FooterTemplate>
						<asp:Button CommandName="AddDatasRow" Text="Agregar" ID="btnAdd" runat="server" />
					</FooterTemplate>
				</asp:TemplateColumn>
			</columns>
    </asp:DataGrid></p>
<p>
    <asp:Button id="btnAcpt1" onclick="Aceptar_Vehiculos" runat="server" Text="Aceptar" CausesValidation="False"></asp:Button>
    <asp:Button id="btnAceptarExcel" onclick="Aceptar_Vehiculos_Excel" runat="server" Text="Confirmación" Visible = "false"></asp:Button>
</p>
<asp:PlaceHolder id="infVehPlHl" runat="server">
    <asp:PlaceHolder id="plUbicVehRet" runat="server">
        <table class="filstersIn">
            <tr>
                <td>
                    Ubicación Vehículos Retoma :
                </td>
                <td align="right">
                    <asp:DropDownList id="ddlUbiRet" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Prefijo Factura Retoma :
                </td>
                <td align="right">
                    <asp:DropDownList id="ddlPrefFactRet" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
    <legend class="Legends">Información Técnica</legend>

    <asp:DataGrid id="infTcn" runat="server" GridLines="Vertical" cssclass="datagrid"
        AutoGenerateColumns="false" OnItemDataBound="infTcnItemDataBound">
        <headerstyle cssclass="header"></headerstyle>
        <selecteditemstyle cssclass="selected"></selecteditemstyle>
        <alternatingitemstyle cssclass="alternate"></alternatingitemstyle>
        <itemstyle cssclass="item"></itemstyle>
        <columns>
			<asp:TemplateColumn HeaderText="ID">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ID") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="VIN">
				<ItemTemplate>
					<asp:TextBox id="vin" runat="server" style="width : 190px;"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorVin" runat="server" ControlToValidate="vin" Display="Dynamic" Font-Name="Arial" Font-Size="11">*</asp:RequiredFieldValidator>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="MOTOR">
				<ItemTemplate>
					<asp:TextBox id="motor" runat="server" class="tmediano"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorMotor" runat="server" ControlToValidate="motor" Display="Dynamic" Font-Name="Arial" Font-Size="11">*</asp:RequiredFieldValidator>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="SERIE">
				<ItemTemplate>
					<asp:TextBox id="serie" runat="server" Width="145"></asp:TextBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="CHASIS">
				<ItemTemplate>
					<asp:TextBox id="chasis" runat="server" Width="145"></asp:TextBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="COLOR">
				<ItemTemplate>
					<asp:DropDownList id="ddlColor" Width="220" runat="server"></asp:DropDownList>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="AÑO MODELO">
				<ItemTemplate>
					<asp:DropDownList id="anoModelo" runat="server" Width="60"></asp:DropDownList>
					<asp:RequiredFieldValidator id="validatorAnoModelo" runat="server" ControlToValidate="anoModelo" Display="Dynamic" Font-Name="Arial" Font-Size="11" class="dmediano">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="validatorAnoModelo2" runat="server" ControlToValidate="anoModelo" Display="Dynamic" Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}">*</asp:RegularExpressionValidator>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="TIPO SERVICIO">
				<ItemTemplate>
					<asp:DropDownList id="ddlTipoServicio" runat="server" Width="105"></asp:DropDownList>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="PLACA" Visible="true">
				<ItemTemplate>
					<asp:TextBox id="txtPlaca" runat="server" class="tmediano"></asp:TextBox>
				</ItemTemplate>
			</asp:TemplateColumn>
		</columns>
    </asp:DataGrid>
    <br />
    <br />
    <legend class="Legends">Información Comercial</legend>
    <asp:DataGrid id="infCmc" runat="server" OnItemCommand="Cargar_Info" GridLines="Vertical"
        cssclass="datagrid" AutoGenerateColumns="false" OnItemDataBound="infCmcItemDataBound">
        <headerstyle cssclass="header"></headerstyle>
        <selecteditemstyle cssclass="selected"></selecteditemstyle>
        <alternatingitemstyle cssclass="alternate"></alternatingitemstyle>
        <itemstyle cssclass="item"></itemstyle>
        <columns>
			<asp:TemplateColumn HeaderText="ID">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ID") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="NÚMERO DE INVENTARIO">
				<ItemTemplate>
					<asp:TextBox id="numInv" runat="server" class="tpequeno" ReadOnly="True"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorNumInv" runat="server" ControlToValidate="numInv" Display="Dynamic" Font-Name="Arial" Font-Size="11">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="validatorNumInv2" runat="server" ControlToValidate="numInv" Display="Dynamic" Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]+">*</asp:RegularExpressionValidator>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="NÚMERO DE RECEPCIÓN">
				<ItemTemplate>
					<asp:TextBox id="numRcp" runat="server" class="tpequeno"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorNumRcp" runat="server" ControlToValidate="numRcp" Display="Dynamic" Font-Name="Arial" Font-Size="11">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="validatorNumRcp2" runat="server" ControlToValidate="numRcp" Display="Dynamic" Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]+">*</asp:RegularExpressionValidator>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="FECHA DE RECEPCIÓN">
				<ItemTemplate>
					<asp:TextBox id="fchRcp" onkeyup="DateMask(this)" runat="server" class="tmediano"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorFchRcp" runat="server" ControlToValidate="fchRcp" Display="Dynamic" Font-Name="Arial" Font-Size="11">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="validatorFchRcp2" runat="server" ControlToValidate="fchRcp" Display="Dynamic" Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}">*</asp:RegularExpressionValidator>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="FECHA DE DISPONI BILIDAD">
				<ItemTemplate>
					<asp:TextBox id="fchDsp" onkeyup="DateMask(this)" runat="server" class="tmediano"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorFchDsp" runat="server" ControlToValidate="fchDsp" Display="Dynamic" Font-Name="Arial" Font-Size="11">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="validatorFchDsp2" runat="server" ControlToValidate="fchDsp" Display="Dynamic" Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}">*</asp:RegularExpressionValidator>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="KILOMETRAJE RECEPCIÓN">
				<ItemTemplate>
					<asp:TextBox id="klRcp" runat="server" class="tpequeno" onkeyup="NumericMaskE(this,event)"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorKlRcp" runat="server" ControlToValidate="klRcp" Display="Dynamic" Font-Name="Arial" Font-Size="11">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="validatorKlRcp2" runat="server" ControlToValidate="klRcp" Display="Dynamic" Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\.]+">*</asp:RegularExpressionValidator>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="CLASE DE VEHICULO">
				<ItemTemplate>
					<asp:DropDownList id="ddlClaseVehiculo" runat="server" class="tmediano"></asp:DropDownList>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="NÚMERO MANIFIESTO">
				<ItemTemplate>
					<asp:TextBox id="numMan" runat="server" class="tmediano"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorNumMan" runat="server" ControlToValidate="numMan" Display="Dynamic" Font-Name="Arial" Font-Size="11">*</asp:RequiredFieldValidator>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="FECHA MANIFIESTO">
				<ItemTemplate>
					<asp:TextBox id="fchMan" onkeyup="DateMask(this)" runat="server" class="tmediano"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorFchMan" runat="server" ControlToValidate="fchMan" Display="Dynamic" Font-Name="Arial" Font-Size="11">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="validatorFchMan2" runat="server" ControlToValidate="fchMan" Display="Dynamic" Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}">*</asp:RegularExpressionValidator>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="NÚMERO DE ADUANA">
				<ItemTemplate>
					<asp:TextBox id="numAdu" runat="server" class="tmediano"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorNumAdu" runat="server" ControlToValidate="numAdu" Display="Dynamic" Font-Name="Arial" Font-Size="11">*</asp:RequiredFieldValidator>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="FECHA LEVANTE">
				<ItemTemplate>
					<asp:TextBox id="numDO" onkeyup="DateMask(this)" runat="server" class="tmediano"></asp:TextBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="NÚMERO DE LEVANTE">
				<ItemTemplate>
					<asp:TextBox id="numLvt" runat="server" class="tmediano"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorNumLvt" runat="server" ControlToValidate="numLvt" Display="Dynamic" Font-Name="Arial" Font-Size="11">*</asp:RequiredFieldValidator>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="TIPO DE COMPRA">
				<ItemTemplate>
					<asp:DropDownList id="ddlTipoCompra" runat="server" class="dmediano"></asp:DropDownList>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="VALOR VEHICULO">
				<ItemTemplate>
					<asp:TextBox ReadOnly="false" id="vlVehRcp" runat="server" class="tmediano" onkeyup="NumericMaskE(this,event)"></asp:TextBox>
					<asp:RequiredFieldValidator id="validatorVlVehRcp" runat="server" ControlToValidate="vlVehRcp" Display="Dynamic" Font-Name="Arial" Font-Size="11">*</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="validatorVlVehRcp2" runat="server" ControlToValidate="vlVehRcp" Display="Dynamic" Font-Name="Arial" Font-Size="11" ASPClass="RegularExpressionValidator" ValidationExpression="[0-9\,\.]+">*</asp:RegularExpressionValidator>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn>
				<HeaderTemplate>
					<asp:Button id="btn_LlenarGrilla" CommandName="Cargar" CausesValidation="False" Runat="server" Text="Llenar"></asp:Button>
				</HeaderTemplate>
			</asp:TemplateColumn>
		</columns>
    </asp:DataGrid>
    <p>
        Valor Total de Vehículos :
        <asp:Label id="lbTotal" runat="server"></asp:Label>

    </p>
    <p>
        <asp:PlaceHolder ID="plcRets" Runat="server"></asp:PlaceHolder>
    </p>
    <p>
        <asp:Button id="btnAcpt2" onclick="Aceptar_Informacion" runat="server" Text="Aceptar"></asp:Button>
        <asp:Button id="btnAceptarExcel2" onclick="Aceptar_Informacion_Excel" runat="server" Text="Aceptar Excel" Visible = "false"></asp:Button>

    </p>
</asp:PlaceHolder>
<p>
    <asp:Label id="lb" runat="server"></asp:Label>
</p>
