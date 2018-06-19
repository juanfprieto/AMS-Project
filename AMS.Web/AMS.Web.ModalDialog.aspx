<%@ outputcache location="Client" duration="120" VaryByControl="dgTable" %>
<%@ Page Language="c#" Debug="true" autoeventwireup="True" codebehind="AMS.Web.ModalDialog.aspx.cs" Inherits="AMS.Web.ModalDialog" %>
<!DOCTYPE html>
<HTML lang="es">
    <HEAD>
		<meta charset="utf-8" />
		<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
		<script language="javascript">

        <%
            if(Request.QueryString["Vals"]!=null)
            {
                String parametros = Request.QueryString["Vals"];
                Response.Write("parent.terminarDialogo( '" + parametros.Replace("*", ",") + "','');");
            }
        %>
		</script>
		<LINK href="../css/AMS.css" type="text/css" rel="stylesheet">
	</HEAD>

	<body>
		<form runat="server">
        <fieldset>
            <table class="filtersIn">
			    <tbody>
				    <tr>
					    <td>
                            <FONT SIZE=2><b>Buscar:</b></FONT>&nbsp;
                            <asp:textbox id="tbWord" class="tpequeno" runat="server" ></asp:textbox>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <FONT SIZE=2><b>En:</b></FONT>&nbsp;
                            <asp:dropdownlist id="ddlCols" class="dpequeno" runat="server"></asp:dropdownlist>
                        </td>
                    </tr>
                    <tr>
                        <td >
						    <asp:button id="btSearch" onclick="Search" runat="server" Text="Buscar" UseSubmitBehavior="false" 
                                OnClientClick="clickOnce(this, 'Cargando...')"></asp:button>&nbsp;&nbsp;
                            <asp:button id="btInserta" onclick="Inserta" runat="server" Text="Insertar" UseSubmitBehavior="false" 
                                OnClientClick="clickOnce(this, 'Cargando...')"></asp:button>&nbsp;&nbsp;
                            <asp:button id="btnLimpiar" onclick="LimpiarCache" runat="server" Text="Recargar Datos" UseSubmitBehavior="false" 
                                OnClientClick="clickOnce(this, 'Cargando...')"></asp:button>
                            <asp:label id="lbTitle" runat="server"></asp:label>
                        </td>
				    </tr>
			    </tbody>
		    </table>
        </fieldset>
		<table>
			<tbody>
				<tr>
					<td>
							<asp:datagrid id="dgTable" runat="server" PageSize="100" OnItemDataBound="DgHelp_ItemDataBound"
								BorderStyle="Ridge" BorderWidth="2px" BorderColor="White" BackColor="White" CellPadding="3"
								GridLines="None" CellSpacing="1" OnPageIndexChanged="dgHelp_Page" AllowPaging="True" EnableViewState="True"
								AutoGenerateColumns="True">
								<FooterStyle forecolor="Black"  font-size="13px" backcolor="#C6C3C6"></FooterStyle>
								<HeaderStyle font-bold="True" font-size="13px" forecolor="#E7E7FF" backcolor="#4A3C8C"></HeaderStyle>
								<PagerStyle horizontalalign="Center" font-size="13px" forecolor="Black" position="TopAndBottom" backcolor="#C6C3C6"
									mode="NumericPages"></PagerStyle>
								<SelectedItemStyle font-bold="True"  font-size="13px" forecolor="White" backcolor="#9471DE"></SelectedItemStyle>
								<ItemStyle forecolor="Black" backcolor="#DEDFDE"  font-size="13px"></ItemStyle>
							</asp:datagrid>
					</td>

				</tr>
				<tr>
					<td>
						<p style="TEXT-ALIGN: center"><asp:label id="lb" runat="server"></asp:label></p>
					</td>
				</tr>
			</tbody>
		</table>
		<br>
		<p style="TEXT-ALIGN: center"><asp:textbox id="insTabla" runat="server" Visible="False"></asp:textbox></asp:textbox></p>
		<p style="TEXT-ALIGN: center"></p>
        
		</form>
	</body>
</HTML>

<script language="javascript">
 function clickOnce(btn, msg)
        {
            // Comprobamos si se está haciendo una validación
            if (typeof(Page_ClientValidate) == 'function') 
            {
                // Si se está haciendo una validación, volver si ésta da resultado false
                if (Page_ClientValidate() == false) { return false; }
            }
            
            // Asegurarse de que el botón sea del tipo button, nunca del tipo submit
            if (btn.getAttribute('type') == 'button')
            {
                // El atributo msg es totalmente opcional. 
                // Será el texto que muestre el botón mientras esté deshabilitado
                if (!msg || (msg='undefined')) { msg = 'Procesando..'; }
                
                btn.value = msg;

                // La magia verdadera :D
                btn.disabled = true;
            }
            
            return true;
        }
</script>
