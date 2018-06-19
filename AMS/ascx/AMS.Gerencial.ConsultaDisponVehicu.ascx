<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Gerencial.ConsultaDisponVehicu.ascx.cs" Inherits="AMS.Gerencial.AMS_Gerencial_ConsultaDisponVehicu" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
<FIELDSET>
<asp:button id="Generar" onclick="Generar_Click" runat="server" Text="Consultar Disponibilidad"></asp:button>
</FIELDSET>
<asp:panel id="Panel1" runat="server" Height="525px" Visible="True">
<asp:Panel id=Panel2 runat="server" Height="525px" Visible="False">
<br />
<fieldset>
<TABLE id=Table class="filtersIn">
  <TR>
    <TD>
<asp:Label id=Label1 runat="server" ForeColor="blue" Font-Bold="True">Disponibilidad de Vehículos</asp:Label></TD></TR>

</TABLE>
</fieldset>

<LEGEND>Vehículos Nuevos</LEGEND>

<DIV style="OVERFLOW: auto; WIDTH: 851px; HEIGHT: 364px">
<asp:datagrid id=Grid runat="server" cssclass="datagrid" AllowSorting="True" PageSize="5" HorizontalAlign="Center" AutoGenerateColumns="False">
					<FooterStyle CssClass="footer"></FooterStyle>
					<HeaderStyle CssClass="header"></HeaderStyle>
					<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
                    <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
					<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
					<ItemStyle CssClass="item"></ItemStyle>
					<Columns>
						<asp:TemplateColumn HeaderText="Código">
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "CODIGO") %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Descripción">
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="VIN">
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "VIN") %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Modelo">
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "MODELO") %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="D. O.">
							<ItemStyle HorizontalAlign="Left"></ItemStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "MOTOR") %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Color">
							<ItemStyle HorizontalAlign="left"></ItemStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "COLOR") %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Ubicación">
							<ItemStyle HorizontalAlign="left"></ItemStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "UBICACION") %>
							</ItemTemplate>
						</asp:TemplateColumn>

						<asp:TemplateColumn HeaderText="Estado">
							<ItemStyle HorizontalAlign="left"></ItemStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "ESTADO") %>
							</ItemTemplate>
						</asp:TemplateColumn>

						<asp:TemplateColumn HeaderText="Días">
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "DIAS") %>
							</ItemTemplate>
						</asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Origen">
							<ItemStyle HorizontalAlign="Left"></ItemStyle>
							<ItemTemplate>
								<%# DataBinder.Eval(Container.DataItem, "ORIGEN") %>
							</ItemTemplate>
						</asp:TemplateColumn>

					</Columns>
				</asp:datagrid>
                </DIV>
                 
<TABLE id=Table2 cellSpacing=1 cellPadding=1 width=300 border=0>
 
</TABLE>

<LEGEND>Vehículos Usados</LEGEND>
<DIV style="OVERFLOW: auto; WIDTH: 851px; HEIGHT: 148px">
<asp:datagrid id=Grid2 runat="server" PageSize="5" CssClass="datagrid" HorizontalAlign="Center" AutoGenerateColumns="False">
				<FooterStyle CssClass="footer"></FooterStyle>
				<HeaderStyle CssClass="header"></HeaderStyle>
				<PagerStyle CssClass="pager" mode="NumericPages"></PagerStyle>
                <SelectedItemStyle CssClass="selected"></SelectedItemStyle>
				<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
				<ItemStyle CssClass="item"></ItemStyle>
				<Columns>
					<asp:TemplateColumn HeaderText="Código">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "CODIGO2") %>
						</ItemTemplate>
					</asp:TemplateColumn>

					<asp:TemplateColumn HeaderText="Descripción">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "DESCRIPCION2") %>
						</ItemTemplate>
					</asp:TemplateColumn>

					<asp:TemplateColumn HeaderText="PLACA">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "VIN2") %>
						</ItemTemplate>
					</asp:TemplateColumn>

					<asp:TemplateColumn HeaderText="Modelo">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "MODELO2") %>
						</ItemTemplate>
					</asp:TemplateColumn>

					<asp:TemplateColumn HeaderText="D.O.">
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "MOTOR2") %>
						</ItemTemplate>
					</asp:TemplateColumn>

					<asp:TemplateColumn HeaderText="Color">
						<ItemStyle HorizontalAlign="left"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "COLOR2") %>
						</ItemTemplate>
					</asp:TemplateColumn>

					<asp:TemplateColumn HeaderText="Ubicación">
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "UBICACION2") %>
						</ItemTemplate>
					</asp:TemplateColumn>

					<asp:TemplateColumn HeaderText="Estado">
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "ESTADO2") %>
						</ItemTemplate>
					</asp:TemplateColumn>

					<asp:TemplateColumn HeaderText="Días">
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "DIAS2") %>
						</ItemTemplate>
					</asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="Origen">
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "ORIGEN2") %>
						</ItemTemplate>
					</asp:TemplateColumn>

				</Columns>
			</asp:datagrid></DIV>
        
</asp:Panel>
</asp:panel>
<fieldset>
<TABLE id="Table1" class="filtersIn">
  <TR>
    <TD><asp:Label id=Label5 runat="server" Font-Bold="True">Total Vehículos Nuevos</asp:Label></TD>
    <TD><asp:Label id=totalNu runat="server" Font-Bold="True">0</asp:Label></TD>
    <TD><asp:Label id=Label4 runat="server" Font-Bold="True">Total Precio Vehículos Nuevos</asp:Label></TD>
    <TD align=right><asp:Label id=totalCostoN runat="server" Font-Bold="True">0</asp:Label></TD>
 </TR>
 <TR>
    <TD><asp:Label id=Label6 runat="server" Font-Bold="True">Total Vehículos Usados</asp:Label></TD>
    <TD><asp:Label id=totalUsa runat="server" Font-Bold="True">0</asp:Label></TD>
    <TD><asp:Label id=Label16 runat="server" Font-Bold="True">Total Precio Vehículos Usados</asp:Label></TD>
    <TD align=right><asp:Label id=totalCostoU runat="server" Font-Bold="True">0</asp:Label></TD>
 </TR>
  <TR>
    <TD><asp:Label id=Label17 runat="server" Font-Bold="True">Total Vehículos</asp:Label></TD>
    <TD><asp:Label id=totalVeh runat="server" Font-Bold="True">0</asp:Label></TD>
    <TD><asp:Label id=Label19 runat="server" Font-Bold="True">Total Precio Vehículos</asp:Label></TD>
    <TD align=right><asp:Label id=totalCost runat="server" Font-Bold="True">0</asp:Label></TD>
 </TR>
</TABLE>
</fieldset>
<br />
<fieldset>
<asp:Panel ID="pnlClick" runat="server" CssClass="pnlCSS">
<div style="background-image:url('../img/blueBorder.jpg'); height:30px; vertical-align:middle">
<div style="float:left; color:black;padding:5px 5px 0 0">
Collapsible Panel
</div>
<div style="float:right; color:black; padding:5px 5px 0 0">
<asp:Label ID="lblMessage" runat="server" Text="Label"/>
<asp:Image ID="imgArrows" runat="server" />
</div>
<div style="clear:both"></div>
</div>
</asp:Panel>

<asp:Panel ID="pnlCollapsable" runat="server" Height="50px" CssClass="pnlCSS">
<table align="center" width="100%" id="Table4" class="filtersIn">
<tr>
<td></td>
<td>
<b>Registration Form</b>
</td>
</tr>
<tr>
<td align="right" >
UserName:
</td>
<td>
<asp:TextBox ID="txtuser" runat="server"/>
</td>
</tr>
<tr>
<td align="right" >
Password:
</td>
<td>
<asp:TextBox ID="txtpwd" runat="server"/>
</td>
</tr>
<tr>
<td align="right">
FirstName:
</td>
<td>
<asp:TextBox ID="txtfname" runat="server"/>
</td>
</tr>
<tr>
<td align="right">
LastName:
</td>
<td>
<asp:TextBox ID="txtlname" runat="server"/>
</td>
</tr>
<tr>
<td align="right">
Email:
</td>
<td>
<asp:TextBox ID="txtEmail" runat="server"/>
</td>
</tr>
<tr>
<td align="right" >
Phone No:
</td>
<td>
<asp:TextBox ID="txtphone" runat="server"/>
</td>
</tr>
<tr>
<td align="right" >
Location:
</td>
<td align="left">
<asp:TextBox ID="txtlocation" runat="server"/>
</td>
</tr>
<tr>
<td></td>
<td align="left" >
<asp:Button ID="btnsubmit" runat="server" Text="Save"/>
<input type="reset" value="Reset" />
</td>
</tr>
</table>
</asp:Panel>
</fieldset>


<style type="text/css">
.pnlCSS{
font-weight: bold;
cursor: pointer;
border: solid 1px #c0c0c0;
width:30%;
}
</style>




<ajax:CollapsiblePanelExtender 
ID="CollapsiblePanelExtender2" 
runat="server"
CollapseControlID="pnlClick"
Collapsed="true"
ExpandControlID="pnlClick"
TextLabelID="lblMessage"
CollapsedText="Show"
ExpandedText="Hide"
ImageControlID="imgArrows"
CollapsedImage="../img/downarrow.jpg"
ExpandedImage="../img/uparrow.jpg"
ExpandDirection="Vertical"
TargetControlID="pnlCollapsable"
ScrollContents="false"></ajax:CollapsiblePanelExtender>