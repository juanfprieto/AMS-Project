<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.DBManager.ValidaScriptDinamicoDB2.ascx.cs" Inherits="AMS.DBManager.ValidaScriptDinamicoDB2" %>

<table>
    <tr>
        <th>
            Revisor de Script Dinamico DB2
        </th>
    </tr>
    <tr>
        <td>
            A continuacion digite el script dinamico que desea revisar:
        </td>
    </tr>
    <tr>
        <td>
            <asp:TextBox ID="txtScript" runat="server" Height="400px" Columns="50" Rows="50" 
                TextMode="MultiLine" Wrap="true" UseSubmitBehavior="false"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="btnVerificar" Text="Verificar" runat="server" OnClick="VerificarScript" />
        </td>
    </tr>
    <tr>
        <td>
            Resultados:<br />
            <asp:Label ID="lblResultado" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:datagrid id="grdResultados" runat="server"
	            onPageIndexChanged="CambioPagina" AllowPaging="True" ShowFooter="True" AutoGenerateColumns="True"
	            cssclass="datagrid">
	            <FooterStyle CssClass="footer"></FooterStyle>
	            <HeaderStyle CssClass="header"></HeaderStyle>
	            <PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
	            <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
	            <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
	            <ItemStyle CssClass="item"></ItemStyle>
            </asp:datagrid>
        </td>
    </tr>
    
</table>