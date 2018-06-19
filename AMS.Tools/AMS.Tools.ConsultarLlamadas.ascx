<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Tools.ConsultarLlamadas.ascx.cs" Inherits="AMS.Tools.ConsultarLlamadas" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript">
	function MostrarDiv(idDiv)
	{
		var divDetalle = document.getElementById(idDiv);
		if(divDetalle.style.display == 'none')
			divDetalle.style.display = '';
		else
			divDetalle.style.display = 'none';
	}
</script>
<P>Seleccione las llamadas que desea consultar, se encuentran ordenadas en orden cronológico de la mas reciente a la mas antigua :
</P>
<P>
	<asp:RadioButtonList id="rblLlam" runat="server" RepeatDirection="Horizontal" Width="416px" OnSelectedIndexChanged="rblLlam_SelectedIndexChanged" AutoPostBack="True">
		<asp:ListItem Value="C">Llamadas Creadas</asp:ListItem>
		<asp:ListItem Value="D">Llamadas Cerradas</asp:ListItem>
		<asp:ListItem Value="R">Llamadas Revisadas</asp:ListItem>
	</asp:RadioButtonList></P>
<P>
	<asp:DataGrid id="dgCreadas" runat="server" ShowHeader=True HeaderStyle-BackColor=#000084 ForeColor=White AutoGenerateColumns="False" onItemCommand="dgCreadas_ItemCommand">
		<Columns>
			<asp:TemplateColumn HeaderText="Identificador de la Llamada y Persona Que Llamo : " ItemStyle-BackColor=White>
			<ItemTemplate>
				<table style="BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; BACKGROUND-COLOR: white; BORDER-BOTTOM-STYLE: none">
				<tr>
					<td>
					</td>
				</tr>
				<tr>
					<td>
					</td>
				</tr>
				<tr>
					<td>
						<a href="javascript:MostrarDiv('divDetalle<%# DataBinder.Eval(Container.DataItem, "ID") %>');"><%# DataBinder.Eval(Container.DataItem, "ID") %>&nbsp;-&nbsp;<%# DataBinder.Eval(Container.DataItem, "DE") %> - Haga Click para ver detalles</a>
					</td>
				</tr>
				<tr>
				<td>
					<div id='divDetalle<%# DataBinder.Eval(Container.DataItem, "ID") %>' style="display:none">
						<table style="FONT-WEIGHT: bold; COLOR: black">
							<tr>
								<td></td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td >
									Identificador : 
								</td>
								<td>&nbsp;&nbsp;&nbsp;</td>
								<td>
									<%# DataBinder.Eval(Container.DataItem, "ID") %>
								</td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td >
									Persona que Llamo : 
								</td>
								<td>&nbsp;&nbsp;&nbsp;</td>
								<td >
									<%# DataBinder.Eval(Container.DataItem, "DE") %>
								</td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td>
									Persona que Recibio la Llamada: 
								</td>
								<td>&nbsp;&nbsp;&nbsp;</td>
								<td>
									<%# DataBinder.Eval(Container.DataItem, "RECIBIO") %>
								</td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td>
									Acción a Seguir : 
								</td>
								<td>&nbsp;&nbsp;&nbsp;</td>
								<td>
									<%# DataBinder.Eval(Container.DataItem, "ACCION") %>
								</td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td>
									Estado de la Llamada : 
								</td>
								<td>&nbsp;&nbsp;&nbsp;</td>
								<td>
									<%# DataBinder.Eval(Container.DataItem, "ESTADO") %>
								</td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td>
									Fecha de la Llamada : 
								</td>
								<td>&nbsp;&nbsp;&nbsp;</td>
								<td>
									<%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "FECHA")).ToString("yyyy-MM-dd") %>
								</td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td>
									Hora de la Llamada : 
								</td>
								<td>&nbsp;&nbsp;&nbsp;</td>
								<td>
									<%# DataBinder.Eval(Container.DataItem, "HORA") %>
								</td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td>
									Mensaje : 
								</td>
								<td>&nbsp;&nbsp;&nbsp;</td>
								<td>
									<%# DataBinder.Eval(Container.DataItem, "MENSAJE") %>
								</td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td>
									Comentario : 
								</td>
								<td>&nbsp;&nbsp;&nbsp;</td>
								<td>
									<asp:TextBox ID=tbcom Runat=server TextMode=MultiLine Width=300 Text='<%# DataBinder.Eval(Container.DataItem, "COMENTARIO") %>'></asp:TextBox>
								</td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>	
								<td>
									Marcar la llamada como :  
								</td>
							</tr>
							<tr>
								<td>
									<asp:RadioButtonList ID=rblmar Runat=server RepeatDirection=Horizontal>
										<asp:ListItem Selected=False Value=D>Cerrada</asp:ListItem>
										<asp:ListItem Selected=True Value=R>Revisada</asp:ListItem>
									</asp:RadioButtonList>
								</td> 
								<td>&nbsp;&nbsp;&nbsp;</td>
								<td>
									<asp:Button ID=btnGrabar Runat=server Text="Cambiar Estado" CommandName=guardar></asp:Button>
								</td>
							</tr>
						</table>
					</div>
				</td>
				</tr>
				</table>
			</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
	<asp:DataGrid id="dgCerradas" runat="server" ShowHeader=True HeaderStyle-BackColor=#000084 ForeColor=White AutoGenerateColumns="False">
		<Columns>
			<asp:TemplateColumn HeaderText="Identificador de la Llamada y Persona Que Llamo : " ItemStyle-BackColor=White>
			<ItemTemplate>
				<table style="BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; BACKGROUND-COLOR: white; BORDER-BOTTOM-STYLE: none">
				<tr>
					<td>
					</td>
				</tr>
				<tr>
					<td>
					</td>
				</tr>
				<tr>
					<td>
						<a href="javascript:MostrarDiv('divDetalle<%# DataBinder.Eval(Container.DataItem, "ID") %>');"><%# DataBinder.Eval(Container.DataItem, "ID") %>&nbsp;-&nbsp;<%# DataBinder.Eval(Container.DataItem, "DE") %> - Haga Click para ver detalles</a>
					</td>
				</tr>
				<tr>
				<td>
					<div id='divDetalle<%# DataBinder.Eval(Container.DataItem, "ID") %>' style="display:none">
						<table style="FONT-WEIGHT: bold; COLOR: black">
							<tr>
								<td></td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td>
									Identificador : 
								</td>
								<td>&nbsp;&nbsp;&nbsp;</td>
								<td>
									<%# DataBinder.Eval(Container.DataItem, "ID") %>
								</td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td>
									Persona que Llamo : 
								</td>
								<td>&nbsp;&nbsp;&nbsp;</td>
								<td>
									<%# DataBinder.Eval(Container.DataItem, "DE") %>
								</td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td>
									Persona que Recibio la Llamada: 
								</td>
								<td>&nbsp;&nbsp;&nbsp;</td>
								<td>
									<%# DataBinder.Eval(Container.DataItem, "RECIBIO") %>
								</td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td>
									Acción a Seguir : 
								</td>
								<td>&nbsp;&nbsp;&nbsp;</td>
								<td>
									<%# DataBinder.Eval(Container.DataItem, "ACCION") %>
								</td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td>
									Estado de la Llamada : 
								</td>
								<td>&nbsp;&nbsp;&nbsp;</td>
								<td>
									<%# DataBinder.Eval(Container.DataItem, "ESTADO") %>
								</td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td>
									Fecha de la Llamada : 
								</td>
								<td>&nbsp;&nbsp;&nbsp;</td>
								<td>
									<%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "FECHA")).ToString("yyyy-MM-dd") %>
								</td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td>
									Hora de la Llamada : 
								</td>
								<td>&nbsp;&nbsp;&nbsp;</td>
								<td>
									<%# DataBinder.Eval(Container.DataItem, "HORA") %>
								</td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td>
									Mensaje : 
								</td>
								<td>&nbsp;&nbsp;&nbsp;</td>
								<td>
									<%# DataBinder.Eval(Container.DataItem, "MENSAJE") %>
								</td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td>
									Comentario : 
								</td>
								<td>&nbsp;&nbsp;&nbsp;</td>
								<td>
									<%# DataBinder.Eval(Container.DataItem, "COMENTARIO") %>
								</td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr>
								<td></td>
							</tr>
						</table>
					</div>
				</td>
				</tr>
				</table>
			</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
	</P>
	<asp:Button ID=btnSalir Runat=server Text=Salir OnClick=btnSalir_Click></asp:Button>
<P>
	<asp:Label id="lb" runat="server"></asp:Label></P>