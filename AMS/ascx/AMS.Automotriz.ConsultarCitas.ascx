<%@ Control Language="c#" codebehind="AMS.Automotriz.ConsultarCitas.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.ConsultarCitas" %>
<fieldset>
<table class="filters">
    <tbody>
        <tr>
            <th class="filterHead">
			   <IMG height="70" src="../img/AMS.Flyers.Consultar.png" border="0">
			</th>
            <td>
                <p>
                <table id="" class="filtersIn">
                <tr>
                <td>
                    Por favor digite el numero de la Placa de su automovil&nbsp; :&nbsp;&nbsp; 
                    <asp:TextBox id="placa" runat="server" class="tmediano"></asp:TextBox>
        <br />
                    <asp:Button id="consulta" onclick="Realizar_Consulta" runat="server" Width="169px" Text="Consultar"></asp:Button>
                    </td>
                </tr>
                    </table>
                </p>
            </td>
        </tr>
    </tbody>
</table>

<p>
    <asp:PlaceHolder id="resultadoConsulta" runat="server" EnableViewState="False"></asp:PlaceHolder>
</p>
<p>
    <asp:Label id="lb" runat="server"></asp:Label>
</p>
<p>
</p>
</fieldset>