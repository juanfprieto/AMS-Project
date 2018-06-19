<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Marketing.AccionMarketing.ascx.cs" Inherits="AMS.Marketing.AMS_Marketing_AccionMarketing" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="ew" Namespace="eWorld.UI" Assembly="eWorld.UI" %>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<link rel="stylesheet" href="../css/tabber.css" TYPE="text/css" MEDIA="screen">
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script type="text/javascript">
    <%=strCookie%> 
    document.write('<style type="text/css">.tabber{display:none;}<\/style>');
    var tabberOptions = {
      'cookie':"tabber",'onLoad': function(argsObj){
        var t = argsObj.tabber;
        var i;
        if (t.id) {t.cookie = t.id + t.cookie;}
        i = parseInt(getCookie(t.cookie));
        if (isNaN(i)) { return; }
        t.tabShow(i);},
      'onClick':function(argsObj){
        var c = argsObj.tabber.cookie;
        var i = argsObj.index;
        setCookie(c, i);}
    };
    function setCookie(name, value, expires, path, domain, secure) {
        document.cookie= name + "=" + escape(value) +
            ((expires) ? "; expires=" + expires.toGMTString() : "") +
            ((path) ? "; path=" + path : "") +
            ((domain) ? "; domain=" + domain : "") +
            ((secure) ? "; secure" : "");
    }
    function getCookie(name) {
        var dc = document.cookie;
        var prefix = name + "=";
        var begin = dc.indexOf("; " + prefix);
        if (begin == -1) {
            begin = dc.indexOf(prefix);
            if (begin != 0) return null;
        } else {
            begin += 2;
        }
        var end = document.cookie.indexOf(";", begin);
        if (end == -1) {
            end = dc.length;
        }
        return unescape(dc.substring(begin + prefix.length, end));
    }
    function deleteCookie(name, path, domain) {
        if (getCookie(name)) {
            document.cookie = name + "=" +
                ((path) ? "; path=" + path : "") +
                ((domain) ? "; domain=" + domain : "") +
                "; expires=Thu, 01-Jan-70 00:00:01 GMT";
        }
    }
</script>
<script type="text/javascript" src="../js/tabber.js"></script>
<asp:label id="lbTituloCliente" Runat="server"></asp:label>
<div class="tabber" id="mytab1">
    <div class="tabbertab" title="Basicos">
        <asp:label id="lblClienteN" Runat="server"></asp:label>
        <asp:DataGrid id="dgrNIT" runat="server" cssclass="datagrid" ShowFooter="False" AutoGenerateColumns="True" ShowHeader="false"
			CellPadding="3">
			<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
			<ItemStyle cssclass="item"></ItemStyle>
		</asp:DataGrid>
		<asp:button id="btnEditarN" runat="server" Text="Editar" CausesValidation="False" onclick="btnEditarN_Click"></asp:button>
    </div>
    <div class="tabbertab" title="Ficha">
        <asp:label id="lblClienteE" Runat="server"></asp:label>
        <asp:DataGrid id="dgrCliente" runat="server" cssclass="datagrid" ShowFooter="False" AutoGenerateColumns="True" ShowHeader="false"
			CellPadding="3">
			<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
			<ItemStyle cssclass="item"></ItemStyle>
		</asp:DataGrid>
		<asp:button id="btnEditar" runat="server" Text="Editar" CausesValidation="False" onclick="btnEditar_Click"></asp:button>
    </div>
    <div class="tabbertab" title="Vehiculos">
        <asp:label id="lblClienteV" Runat="server"></asp:label>
        <asp:DataGrid id="dgrVehiculos" runat="server" cssclass="datagrid"
		    CellPadding="3" GridLines="Vertical" AutoGenerateColumns="False" 
            BorderStyle="None" onitemdatabound="dgrVehiculos_ItemDataBound" 
            onitemcommand="dgrVehiculos_ItemCommand">
		    <HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		    <SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		    <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		    <ItemStyle cssclass="item"></ItemStyle>
		    <Columns>
			    <asp:BoundColumn DataField="PCAT_CODIGO" HeaderText="Catalogo"></asp:BoundColumn>
			    <asp:BoundColumn DataField="MCAT_VIN" HeaderText="VIN"></asp:BoundColumn>
			    <asp:BoundColumn DataField="MCAT_PLACA" HeaderText="Placa"></asp:BoundColumn>
			    <asp:BoundColumn DataField="MCAT_MOTOR" HeaderText="Motor"></asp:BoundColumn>
			    <asp:BoundColumn DataField="MCAT_ANOMODE" HeaderText="Año Modelo"></asp:BoundColumn>
		        <asp:ButtonColumn Text="Editar" CommandName="Editar"></asp:ButtonColumn>
		        <asp:ButtonColumn Text="Hoja Vida" CommandName="HV"></asp:ButtonColumn>
		    </Columns>
	    </asp:DataGrid><br>
	    <asp:label id="lblVehiAct" Runat="server"></asp:label>
	    <asp:DataGrid id="dgrVehiculo" runat="server" cssclass="datagrid" ShowFooter="False" AutoGenerateColumns="True" ShowHeader="false" CellPadding="3">
			<AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
			<ItemStyle cssclass="item"></ItemStyle>
		</asp:DataGrid>
    </div>
    <div class="tabbertab" title="Historial">
        <asp:label id="lblClienteH" Runat="server"></asp:label>
        <asp:DataGrid id="dgrHistorial" runat="server" cssclass="datagrid" CellPadding="3" GridLines="Vertical"  AutoGenerateColumns="False">
		    <HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		    <SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		    <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		    <ItemStyle cssclass="item"></ItemStyle>
		    <Columns>
			    <asp:BoundColumn DataField="DMAR_NUMEACTI" HeaderText="No."></asp:BoundColumn>
			    <asp:BoundColumn DataField="PACT_NOMBMARK" HeaderText="Actividad"></asp:BoundColumn>
			    <asp:BoundColumn DataField="DMAR_DETALLE" HeaderText="Detalle"></asp:BoundColumn>
			    <asp:BoundColumn DataField="DMAR_FECHACTI" HeaderText="Fecha" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
			    <asp:BoundColumn DataField="PVEN_NOMBRE" HeaderText="Asesor"></asp:BoundColumn>
			    <asp:BoundColumn DataField="PRES_DESCRIPCION" HeaderText="Resultado"></asp:BoundColumn>
			    <asp:BoundColumn DataField="DMAR_FECHPROX" HeaderText="Prox. Fecha" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
		    </Columns>
	    </asp:DataGrid>
    </div>
    <div class="tabbertab" title="Ordenes">
        <asp:label id="lblClienteO" Runat="server"></asp:label>
        <asp:DataGrid id="dgrOrdenes" runat="server" cssclass="datagrid"
		    CellPadding="3"  GridLines="Vertical" BorderWidth="1px"
		    AutoGenerateColumns="False">
		    <HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
		    <SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
		    <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
		    <ItemStyle cssclass="item"></ItemStyle>
		    <Columns>
			    <asp:BoundColumn DataField="ORDEN" HeaderText="Orden"></asp:BoundColumn>
			    <asp:BoundColumn DataField="ESTADO" HeaderText="Estado"></asp:BoundColumn>
			    <asp:BoundColumn DataField="PLACA" HeaderText="Placa"></asp:BoundColumn>
			    <asp:BoundColumn DataField="FECHA" HeaderText="Fecha" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
			    <asp:BoundColumn DataField="CARGO" HeaderText="Cargo"></asp:BoundColumn>
			    <asp:BoundColumn DataField="TRABAJO" HeaderText="Trabajo"></asp:BoundColumn>
			    <asp:BoundColumn DataField="KILOMETRAJE" HeaderText="Kilom."></asp:BoundColumn>
			    <asp:BoundColumn DataField="OBSCLI" HeaderText="Obs. Cliente"></asp:BoundColumn>
			    <asp:BoundColumn DataField="OBSREC" HeaderText="Obs. Recepcionista"></asp:BoundColumn>
			    <asp:BoundColumn DataField="FECHAS" HeaderText="Salida" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
		    </Columns>
	    </asp:DataGrid>
    </div>
    <div class="tabbertab" title="Cita">
        <asp:Panel ID="plcCita" runat=server></asp:Panel>
	</div>
    
    <div class="tabbertab" title="Encuesta">
        <asp:Panel ID="plcEncuesta" runat=server></asp:Panel>
	</div>
    <div class="tabbertab" title="Registro">
        <P>
	      <fieldset>
            <table class="filtersIn">
		        <tr>
			        <td>Número
			        </td>
			        <td><asp:label id="lblNumero" Runat="server"></asp:label></td>
		        </tr>
		        <tr>
			        <td>Cliente</td>
			        <td><asp:label id="lblCliente" Runat="server"></asp:label></td>
		        </tr>
		        <tr>
			        <td>Actividad</td>
			        <td><asp:dropdownlist id="ddlActividad" runat="server" AutoPostBack="true" 
                            ></asp:dropdownlist>
                    </td>
		        </tr>
		        <tr>
			        <td>Detalle</td>
			        <td><asp:textbox id="txtDetalle" runat="server" Height="80px" Width="200px" MaxLength="400" TextMode="MultiLine"></asp:textbox></TD>
		        </tr>
		        <tr>
			        <td>Fecha</td>
			        <td><asp:label id="lblFecha" Runat="server"></asp:label></td>
		        </tr>
		        <tr>
			        <td>Vendedor</td>
			        <td><asp:label id="lblVendedor" Runat="server"></asp:label></td>
		        </tr>
		        <tr>
			        <td>Resultado</td>
			        <td><asp:dropdownlist id="ddlResultado" runat="server"></asp:dropdownlist></td>
		        </tr>
		        <tr>
			        <td>Fecha Próxima Contacto </td>
			        <td>
                    <ew:calendarpopup id="txtFechaProx" ImageUrl="../img/AMS.Icon.Calendar.gif" ControlDisplay="TextBoxImage"
					runat="server" Culture="Spanish (Colombia)" Nullable="True">
					<WeekdayStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="White"></WeekdayStyle>
					<MonthHeaderStyle Font-Size="X-Small" Font-Names="Arial" ForeColor="Black" BackColor="Silver"></MonthHeaderStyle>
					<OffMonthStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="#FF8080"></OffMonthStyle>
					<GoToTodayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
						BackColor="White"></GoToTodayStyle>
					<TodayDayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
						BackColor="LightGoldenrodYellow"></TodayDayStyle>
					<DayHeaderStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="LightBlue"></DayHeaderStyle>
					<WeekendStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="LightGray"></WeekendStyle>
					<SelectedDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
						BackColor="Khaki"></SelectedDateStyle>
					<ClearDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
						BackColor="White"></ClearDateStyle>
					<HolidayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
						BackColor="White"></HolidayStyle>
				    </ew:calendarpopup>
                    </td>

		        </tr>
		        <tr>
			        <td>Mercaderista</td>
			        <td><asp:dropdownlist id="ddlMercadeista" runat="server"></asp:dropdownlist></td>
		        </tr>
		        <%--<tr>
			        <td>Duración Mins.</td>
			        <td><asp:textbox id="txtMinutos" onkeyup="NumericMaskE(this,event)" runat="server" Width="80px"></asp:textbox></td>
                </tr>--%>
	        </table>
        </fieldset>
        </P>
        <P><asp:button id="btnGrabar" runat="server" Text="Grabar" onclick="btnGrabar_Click"></asp:button></P>
	  </div>
	  
</div>
<P><asp:label id="lb" runat="server"></asp:label></P>
<asp:button id="btnCancelar" runat="server" Text="Regresar" CausesValidation="False" onclick="btnCancelar_Click"></asp:button>