<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Vehiculos.ModificarSeguimientoDiario.ascx.cs" Inherits="AMS.Vehiculos.AMS_Vehiculos_ModificarSeguimientoDiario" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<fieldset>    
<legend>Modificar Diario Vendedores Usuario</legend>
  <table id="Table1" class="filtersIn">
	<tbody>Usuario
		    <tr>
                <td>	
		    <TR>
			    <TD>
                    <asp:label id="Label1" Font-Bold="True" runat="server">Vendedor:</asp:label>
               <br />
                    <asp:dropdownlist id="vendedor" class="dmediano" runat="server" ></asp:dropdownlist>
                </TD>
		    </TR>
		    <TR>
			    <TD>
                    <asp:label id="Label2" Font-Bold="True" runat="server" >Password:</asp:label>
               <br />
                    <asp:textbox id="password" class="tpequeno" runat="server"  TextMode="Password"></asp:textbox>
                </TD>
		    </TR>
		    <TR>
			    <TD>
                    <asp:button id="Buscar" onclick="Buscar_Click" ForeColor="Black" Font-Bold="True" runat="server" Text="Buscar"></asp:button>
                </TD>
			</TR>
                </TD>
		    </TR>
	    </tbody>
	</TABLE>
</FIELDSET>

<asp:panel id="Panel1" runat="server" Width="632px" Height="336px" Visible="False">

<FIELDSET >
 <LEGEND>Registros</LEGEND>
	<TABLE id="Table1" class="filtersIn">
        <tbody>
			<TR>

				<TD>
                        <asp:Label id="Label3"  runat="server" Font-Bold="True" ForeColor="Black">Resgistros Existentes:</asp:Label>
                </TD>
				<TD>
                        <asp:DropDownList id="registros" runat="server" Font-Bold="True" Width="368px" AutoPostBack="True" onselectedindexchanged="registros_SelectedIndexChanged"></asp:DropDownList>
                </TD>
            </TR>
        </tbody>
    </TABLE>
</FIELDSET>

<asp:Panel id="Panel2" runat="server" Visible="False" Height="224px">


<FIELDSET>
  <LEGEND>Datos Registro</LEGEND>
	<TABLE id="Table2" class="filtersIn>
        <tbody>
		    <TR>
                 <td>
			    <TD>
                        <asp:Label id="Label4"  runat="server" Font-Bold="True" ForeColor="Black">Nombre Cliente:</asp:Label>
                </TD>
			    <TD><asp:TextBox id="nombre"  runat="server"></asp:TextBox><asp:RequiredFieldValidator id="RequiredFieldValidator1"  runat="server" Font-Bold="True" ControlToValidate="nombre" ErrorMessage="Campo Requerido"></asp:RequiredFieldValidator></TD></TR>
		    <TR>
			    <TD><asp:Label id="Label5"  runat="server" Font-Bold="True" ForeColor="Black">Telefono Fijo:</asp:Label></TD>
			    <TD><asp:TextBox id="telefono"  runat="server"></asp:TextBox><asp:RequiredFieldValidator id="RequiredFieldValidator2"  runat="server" Font-Bold="True" ControlToValidate="telefono" ErrorMessage="Campo Requerido"></asp:RequiredFieldValidator></TD></TR>
		    <TR>
			    <TD><asp:Label id="Label6"  runat="server" Font-Bold="True" ForeColor="Black">Telefono Movil:</asp:Label></TD>
			    <TD><asp:TextBox id="telefonomovil"  runat="server"></asp:TextBox></TD></TR>
		    <TR>
			    <TD><asp:Label id="Label7"  runat="server" Font-Bold="True" ForeColor="Black">Telefono Oficina:</asp:Label></TD>
			    <TD><asp:TextBox id="telefonooficina"  runat="server"></asp:TextBox></TD></TR>
		    <TR>
			    <TD><asp:Label id="Label8"  runat="server" Font-Bold="True" ForeColor="Black">E-mail Contacto:</asp:Label></TD>
			    <TD><asp:TextBox id="email"  runat="server"></asp:TextBox></TD></TR>
		    <TR>
			    <TD><asp:Label id="Label9"  runat="server" Font-Bold="True" ForeColor="Black">Observaciones:</asp:Label></TD>
			    <TD><asp:TextBox id="observaciones"  runat="server" Width="288px" TextMode="MultiLine" Height="80px"></asp:TextBox></TD></TR>
		    <TR>
			    <TD><asp:Label id="Label10"  runat="server" Font-Bold="True" ForeColor="Black">Fecha Visita:</asp:Label></TD>
			    <TD><asp:TextBox id="fecha"  runat="server"></asp:TextBox><asp:RequiredFieldValidator id="RequiredFieldValidator3"  runat="server" Font-Bold="True" ControlToValidate="fecha" ErrorMessage="Campo Requerido"></asp:RequiredFieldValidator></TD></TR>
		    <TR>
			    <TD><asp:Label id="Label11"  runat="server" Font-Bold="True" ForeColor="Black">Catálogo Vehículo:</asp:Label></TD>
			    <TD><asp:DropDownList id="catalogo"  runat="server" Width="340px"></asp:DropDownList><asp:RequiredFieldValidator id="RequiredFieldValidator4"  runat="server" Font-Bold="True" ControlToValidate="catalogo" ErrorMessage="Campo Requerido"></asp:RequiredFieldValidator></TD></TR>
		    <TR>
			    <TD><asp:Label id="Label12"  runat="server" Font-Bold="True" ForeColor="Black">Medio:</asp:Label></TD>
			    <TD><asp:DropDownList id="medio"  runat="server" Width="192px"></asp:DropDownList><asp:RequiredFieldValidator id="RequiredFieldValidator5"  runat="server" Font-Bold="True" ControlToValidate="medio" ErrorMessage="Campo Requerido"></asp:RequiredFieldValidator></TD></TR>
		    <TR>
			    <TD><asp:Label id="Label13"  runat="server" Font-Bold="True" ForeColor="Black">Vendedor:</asp:Label></TD>
			    <TD><asp:DropDownList id="vendedor2"  runat="server" Width="340px"></asp:DropDownList><asp:RequiredFieldValidator id="RequiredFieldValidator6"  runat="server" Font-Bold="True" ControlToValidate="vendedor" ErrorMessage="Campo Requerido"></asp:RequiredFieldValidator></TD></TR>
		    <TR>
			    <TD><asp:Label id="Label14"  runat="server" Font-Bold="True" ForeColor="Black">Prospecto:</asp:Label></TD>
			    <TD><asp:DropDownList id="prospecto"  runat="server"></asp:DropDownList></TD></TR>
		    <TR>
			    <TD><asp:Label id="Label16"  runat="server" Font-Bold="True" ForeColor="Black">Resultado Contacto:</asp:Label></TD>
			    <TD><asp:DropDownList id="tipoContacto"  runat="server"></asp:DropDownList></TD></TR>
		    <TR>
			    <TD><asp:Label id="Label17"  runat="server" Font-Bold="True" ForeColor="Black">Fecha de Contacto:</asp:Label></TD>
			    <TD><asp:textbox id="fechaContacto" onkeyup="DateMask(this)"  runat="server"></asp:textbox><asp:RequiredFieldValidator id="RequiredFieldValidator7"  runat="server" Font-Bold="True" ControlToValidate="fechaContacto" ErrorMessage="Campo Requerido"></asp:RequiredFieldValidator></TD></TR>
		    <TR>
			    <TD>
                        <asp:Label id="Label15"  runat="server" Font-Bold="True" ForeColor="Black">Secuencia:</asp:Label></TD>
			    <TD>
                        <asp:TextBox id="secuencia"  runat="server" Width="104px" ReadOnly="True"></asp:TextBox></TD></TR>
		    <TR>
			    <TD>
                        <asp:Button id="Actualizar" onclick="Actualizar_Click"  runat="server" Font-Bold="True" ForeColor="Black" Text="Actualizar"></asp:Button></TD>
			    <TD>
                </TD>
                </td>
            </TR>
    </TABLE>
</FIELDSET>

    </asp:Panel>
    </asp:panel>
