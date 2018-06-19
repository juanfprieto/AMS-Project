<%@ Control Language="c#" codebehind="AMS.Automotriz.AdminKitsCombos.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.AdminKitsCombos" %>
<link rel="stylesheet" href="../css/tabber.css" TYPE="text/css" MEDIA="screen">
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<!DOCTYPE html>
<fieldset>
<table class="filters">
	<tbody>
		<tr>
			<th class="filterHead">
			   <img height="70" src="../img/AMS.Flyers.News.png" border="0">
			</th>
			<td>
            <p> 
				<table id="table" class="filtersIn">
					<tr> 
                    <td>
								Código del Kit :
								<asp:TextBox id="tbCodigoKit" class="tpequeno" MaxLength="10" runat="server"></asp:TextBox>
								<asp:RequiredFieldValidator id="validatorTbCodigoKit" runat="server" Font-Name="Arial" Font-Size="11" Display="Dynamic"
									ControlToValidate="tbCodigoKit">*</asp:RequiredFieldValidator>
						<br />
								Descripción del Kit :&nbsp;&nbsp;
                                <asp:TextBox id="tbNombreKit" class="tgrande" MaxLength="60" runat="server"></asp:TextBox>
								<asp:RequiredFieldValidator id="validatorTbNombreKit" runat="server" Font-Name="Arial" Font-Size="11" Display="Dynamic"
									ControlToValidate="tbNombreKit">*</asp:RequiredFieldValidator>
						<br />
								Grupo de Catálogo :
								<asp:DropDownList id="ddlGrupos" class="dmediano" runat="server"></asp:DropDownList>
						<br />
								Lista de Precios :
								<asp:DropDownList id="ddlListasPrecios" class="dmediano" runat="server"></asp:DropDownList>
                        <br />
								Kilometraje Específico de Aplicación (<i>si es repetitivo, deje en blanco y parametrice en Plan de Mantenimiento Programado</i>) :
                                <asp:TextBox id="TextBoxKms" class="tpequeno" type="number" MaxLength="10" runat="server"  ></asp:TextBox>
                      	<br />
								Meses de aplicación repetitiva del Kit (<i>si NO aplica cada N meses, deje en blanco</i>) :&nbsp;&nbsp;
                                <asp:TextBox id="TextBoxMeses" class="tpequeno" type="number" MaxLength="4" runat="server"  ></asp:TextBox>		
             		    <br />
						
				<asp:Button id="btnAcpt1" onclick="Ingresar_Kits" runat="server" Text="Aceptar" onClientClick = "espera();"></asp:Button>
                </td>
                </tr>
                </table>
                </p>
			</td>
		</tr>
		<tr>
			<th class="filterHead">
			   <img height="70" src="../img/AMS.Flyers.Edits.png" border="0">
			</th>
			<td>
            <p>
            <table id="table" class="filtersIn">
            <tr>
            <td>
				Seleccione el Kit a editar :
				<asp:DropDownList id="ddlKitsEdit" class="dmediano" runat="server"></asp:DropDownList>
				<br />
				<asp:Button id="btnAcpt2" onclick="Editar_Kits" runat="server" Text="Aceptar" CausesValidation="False" onClientClick = "espera();"></asp:Button>
                </td>
                </tr>
                </table>
                </p>
			</td>
		</tr>
		<tr>
			<th class="filterHead">
			   <img height="70" src="../img/AMS.Flyers.Eliminar.png" border="0">
			</th>
			<td>
            <p>
            <table id="table" class="filtersIn">
            <tr>
            <td>

				Seleccione el Kit a eliminar :
				<asp:DropDownList id="ddlKitsDelete" class="dmediano" runat="server"></asp:DropDownList>
				<br />
				<asp:Button id="btnAcpt3" onclick="Borrar_Kits" runat="server" Text="Aceptar" CausesValidation="False" onClientClick = "espera();"></asp:Button>
             </td>
            </tr>
                </table>
                </p>
			</td>
		</tr>
	</tbody>
</table>
</fieldset>
<asp:Label id="lb" runat="server"></asp:Label>
