<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Automotriz.UnificarTemparioOperaciones.ascx.cs" Inherits="AMS.Automotriz.UnificarTemparioOperaciones" %>
<fieldset>
<br>
A continuación digite la descripción completa o parcial de una operación de tempario que desee unificar.
<br><br>
<table class="filstersIn">
    <tr>
        <td>
            Descripción Operación de Tempario:
        </td>
        <td>
            <asp:TextBox id="txtOperacion" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button id="ver" onclick="btnVer_Click" runat="server" Text="Ver Operaciones"></asp:Button>
        </td>
    </tr>
</table>
</fieldset>
<br>
<br>

<asp:DataGrid id="operaciones" runat="server" AutoGenerateColumns="false" GridLines="Vertical"
		ShowFooter="True" cssclass="datagrid">
		<FooterStyle CssClass="footer"></FooterStyle>
		<HeaderStyle CssClass="header"></HeaderStyle>
		<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
		<SelectedItemStyle CssClass="selected"></SelectedItemStyle>
		<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
		<ItemStyle CssClass="item"></ItemStyle>
		<Columns>
			<asp:TemplateColumn HeaderText="CODIGO OPERACION">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ptem_operacion") %>
				</ItemTemplate>
			</asp:TemplateColumn>

			<asp:TemplateColumn HeaderText="DESCRIPCION DE LA OPERACION">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ptem_descripcion") %>
				</ItemTemplate>
			</asp:TemplateColumn>

            <asp:TemplateColumn HeaderText="EVENTOS">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "VECES") %>
				</ItemTemplate>
			</asp:TemplateColumn>

            <asp:TemplateColumn HeaderText="CORRECTA">
				<ItemTemplate>
                    <center>
                    <asp:RadioButton id="rbsira" runat="server" onClick="hacerUnico(this);" /> 
                    </center>
                </ItemTemplate>
			</asp:TemplateColumn>

            <asp:TemplateColumn HeaderText="ERRADAS" >
                <HeaderTemplate>
                   ERRADAS <asp:CheckBox id="chkAll" runat="server" Checked="False" onClick="checkAll(this);"></asp:CheckBox>
                </HeaderTemplate>
				<ItemTemplate>
					<center><asp:CheckBox id="chkErrada" runat="server" Checked="False" ></asp:CheckBox></center>
				</ItemTemplate>
			</asp:TemplateColumn>

		</Columns>
</asp:DataGrid>
<br />
<p align="right">
<asp:Button id="btnUnificar" onclick="btnUnificar_Click" runat="server" Text="Unificar Operaciones" CausesValidation="False"
    UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Cargando...')"></asp:Button>
</p>


<script type ="text/javascript" type="text/javascript" >

    function hacerUnico(pin) {
        all = document.getElementsByTagName("input");
        for (i = 0; i < all.length; i++) {
            if (all[i].type == "radio" && all[i].id.indexOf("operaciones") > -1) {
                all[i].checked = false;
                all[i + 1].disabled = false;
                if (all[i].name == pin.name) {
                    all[i].checked = true;
                    all[i + 1].disabled = true;
                    all[i + 1].checked = false;
                }
            }
        }

    }

    function checkAll(chk) {
        all = document.getElementsByTagName("input");
        for (i = 0; i < all.length; i++) {
            if (all[i].type == "checkbox" && all[i].id.indexOf("operaciones") > -1) {
                all[i].checked = chk.checked;
                if(all[i].disabled == true){
                    all[i].checked = false;
                }
            }
        }
    }

    
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
