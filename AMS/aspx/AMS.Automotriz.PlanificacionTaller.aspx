<%@ Page language="c#" Codebehind="AMS.Automotriz.PlanificacionTaller.aspx.cs" AutoEventWireup="True" Inherits="AMS.Automotriz.AMS_Automotriz_PlanificacionTaller" %>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <link href="../css/planningSty.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/jquery.js"></script>
    <script type="text/javascript" src="../js/planningScroll.js"></script>
	<title>AMS - Planning de Taller </title>
    <script type="text/javascript">
        var planning = 'taller';
    </script>
</head>
	<body>
    	<form id="Form1" method="post" runat="server">

            <table id="Table1" class="titulo">
	            <tr>
		            <td width="10%" height="60"><img src='../img/logo.jpg' width="100"></td>
		            <td width="50%"><img src='../img/planiing.png'></td>
		            <td width="20%"></td>
		            <td width="20%" align="left"><img src='../img/powered.png'></td>
	            </tr>
	            <tr>
		            <td colspan="3">
			            <b>OPERACIONES: &nbsp;<asp:label id="LabelT" runat="server" Font-Bold="True"></asp:label></b>
		            </td>
		            <td>
			            <input type="button" id="mover" onClick="cambioScroll(this);" value="Parar!">
			            <input type="button" onClick="cambioModo(this);" value="Pantalla">
		            </td>
	            </tr>
	            <tr>
		            <td colspan="2">
			            Estado Operaciones:<div class="estmorado">Repuestos</div>
			            <div class="estazul">Herramienta</div>
			            <div class="estcafe">Ausencia Mec.</div>
			            <div class="estverdeClaro">Inicia Op.</div>
		            </td>
		            <td colspan="2" align="right">
			            Linea de Tiempo:<div class="eRojo">+30 días</div>
			            <div class="eNaranja">-30 días</div>
			            <div class="eAmarillo">Hoy</div>
			            <div class="eVerde">Futuro</div>
		            </td>
	            </tr>
            </table>


            <div id="mecanicos" runat="server"></div>
            <div id="contenido" runat="server"></div>
            <div id="paginasPie" runat="server"></div>

            <div style="top: 760px; position: absolute">
			<asp:label id="Label11" runat="server" Font-Bold="True">OT Sin Asignar</asp:label><BR>
            <asp:datagrid id="Grid" runat="server" Width="100%" ForeColor="Gainsboro" AllowPaging="True" HorizontalAlign="Center"
				BackColor="LightGray" AutoGenerateColumns="False">
				<FooterStyle forecolor="Black" backcolor=DodgerBlue Font-Size=12></FooterStyle>
				<HeaderStyle font-bold="True" forecolor="White" backcolor="#000084"></HeaderStyle>
				<PagerStyle horizontalalign="Center" forecolor="Black" backcolor="#999999" mode="NumericPages"></PagerStyle>
				<SelectedItemStyle font-bold="True" forecolor="White" backcolor="#008A8C"></SelectedItemStyle>
				<AlternatingItemStyle backcolor="Gainsboro"></AlternatingItemStyle>
				<ItemStyle forecolor="Black" backcolor="#EEEEEE"></ItemStyle>
				<Columns>
					<asp:TemplateColumn HeaderText="Numero Orden">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "NUMORDEN") %>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Fecha de Entrada">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "FECHAENTRADA") %>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Hora de Entrada">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "HORAENTRADA") %>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Fecha a Entregar">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "FECHAENTREGA") %>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Hora a Entregar">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "HORAENTREGA") %>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
				<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
			</asp:datagrid>
            </div>
            <div id="contImg">
            <img id="imagenCarga" src="../img/Send3.gif" alt="Procesando..." /><br />
                <label>
                    <font size="4"><i>e</i></font>CAS Procesando...
                </label>
            </div>
			<BR>
		</form>
	</BODY>
</HTML>
