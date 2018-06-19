<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Tools.AutorizacionClienteLista.ascx.cs" Inherits="AMS.Tools.AutorizacionClienteLista" %>

<FIELDSET>
	<p>
    <TABLE class="filtersIn" >
        <tr>
            <td>
                <div id="divMendaje"  runat="server" style="text-align : justify; cursor:move;">
                    En virtud de la ley 1581 de 2012, el titular de la información declara que la entrega de forma libre y voluntaria, bajo el conocimiento de que la misma entrará a formar parte  de la base de datos de la empresa para el tratamiento (recolección, almacenamiento, uso, circulación y supresión) de los datos, en actividades comerciales y técnicas de la marca y sus productos (incluyendo campañas comerciales, promocionales y de servicio), y desde ya acepta que la información pueda ser procesada y compartida por la empresa de acuerdo con la regulación vigente. La empresa garantiza el cumplimiento de los principios  señalados en la Ley 1581 de 2012 y sus decretos reglamentarios, en particular lo relacionado con la seguridad y la confidencialidad con la que se manejan los datos entregados. Es claro para el titular de la información que podrá actualizar, rectificar o solicitar  la eliminación de sus datos en cualquier momento y solicitar prueba de la presente autorización a la empresa.
                </div>
            </td>
        </tr>
    </TABLE>
    </p>
    <p>
	<TABLE class="filtersIn" >
        <tr>
            <td>
                <b>Nit:</b> <asp:textbox id="txtNit" class="tpequeno" runat="server"></asp:textbox><asp:Image id="imglupa1" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente('txtNit');"></asp:Image>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:button id="btnConsultar" Runat="server" UseSubmitBehavior="false" onClientClick="espera();" OnClick="ConsultarRegistroBaseDatos" Text="Consultar" class="bpequeno"></asp:button>
            </td>
        </tr>
    </TABLE>
    </p>
    <p>
    <asp:placeholder id="plhListaDB" runat="server" Visible="false">
        <asp:DataGrid id="grillaElementos" runat="server" cssclass="datagrid"  AutoGenerateColumns="false" ShowFooter="false"  Enabled="true" Width="500" OnItemDataBound="dgElementosBound">
            <FooterStyle CssClass="footer" ></FooterStyle>
			<HeaderStyle CssClass="header"></HeaderStyle>
			<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
			<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
			<ItemStyle CssClass="item"></ItemStyle>
			<Columns>
				<asp:TemplateColumn HeaderText="Tipo Base De Datos" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "TBAS_NOMBRE") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Autorizar" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
					<ItemTemplate>
						<asp:radiobutton id="rdSi" runat="server" enabled="true" GroupName="grupo" Text="Si"></asp:radiobutton> &nbsp;&nbsp;&nbsp;
                        <asp:radiobutton id="rdNo" runat="server" enabled="true" GroupName="grupo" Text="No"></asp:radiobutton> 
                        
					</ItemTemplate>
				</asp:TemplateColumn>
			</Columns>
		</asp:DataGrid>
    </asp:placeholder>
    </p>
    <p>
        <center>
            <asp:button id="btnGuardar" Runat="server" UseSubmitBehavior="false" onClientClick="espera();" onClick="GuardarRegistro" Text="Guardar Registro" class="bpequeno" Visible="false"></asp:button>
        </center>
    </p>
</FIELDSET>

<script type="text/javascript">
    function abrirEmergente(obj) {
        var nit = document.getElementById('<%=txtNit.ClientID %>');
        ModalDialog(nit, 'SELECT NIT.mnit_nit AS NIT, NIT.mnit_APELLIDOS CONCAT \' \' CONCAT COALESCE(NIT.mnit_APELLIDO2,\'\') CONCAT \' \' CONCAT NIT.mnit_NOMBRES CONCAT \' \' CONCAT COALESCE(NIT.mnit_NOMBRE2,\'\') AS NOMBRE FROM mnit NIT WHERE NIT.mnit_nit NOT IN (SELECT PNI.pnital_nittaller FROM pnittaller PNI) AND NIT.mnit_nit NOT IN (SELECT CEM.mnit_nit FROM cempresa CEM) order by NOMBRE', new Array(), 1);
    }
</script>
