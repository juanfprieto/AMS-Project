<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.AdminProyectos.RegistrarActividad.ascx.cs" Inherits="AMS.AdminProyectos.RegistrarActividad" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script>
    function VerificarCliente(obj) {
        RegistrarActividad.Verificar_Cliente(obj.value, Verificar_Cliente_CallBack);
    }
        function Verificar_Cliente_CallBack(response) {
        var respuesta = response.value;
        var contactos = "";
        var codigo = document.getElementById("<%=txtCodigo.ClientID%>");
        var cedula = document.getElementById("<%=txtCedula.ClientID%>");
        var nombre = document.getElementById("<%=txtNombre.ClientID%>");
        var escolaridad = document.getElementById("<%=txtEscolaridad.ClientID%>");
        var entidadP = document.getElementById("<%=txtEntidad.ClientID%>");
        var responsable = document.getElementById("<%=txtResponsable.ClientID%>");
        var tablaDatos = document.getElementById("<%=infoDatos.ClientID%>");
        var labelError = document.getElementById("<%=lbInfoNit.ClientID%>");
        var btnGuardar = document.getElementById("<%=btnRegistrar.ClientID%>");
        try {
            if (respuesta.Tables[0].Rows.length > 0) {
                contactos = "Se han encontrado contactos registrados por este cliente: \n\n";

                for (var i = 0; i < respuesta.Tables[1].Rows.length; i++) {
                    contactos = contactos + respuesta.Tables[1].Rows[i].CONTACTOS + '\n\n'
                }
                alert(contactos);
            }

        } catch (e) { }
        
        nombre.readOnly = true;
        escolaridad.readOnly = true;
        entidadP.readOnly = true;
        responsable.readOnly = true;

        if (respuesta.Tables[0].Rows.length > 0)
        {
            $(labelError).text("");
            $(tablaDatos).show();
            nombre.value = respuesta.Tables[0].Rows[0].NOMBRE;
            cedula.value = respuesta.Tables[0].Rows[0].CEDULA;
            escolaridad.value = respuesta.Tables[0].Rows[0].ESCOLARIDAD;
            entidadP.value = respuesta.Tables[0].Rows[0].ENTIDADP;
            responsable.value = respuesta.Tables[0].Rows[0].RESPONSABLE;
            if (nombre.value.trim() == "" || nombre.value == " ")
            {
                nombre.readOnly = false;
            }
            if (escolaridad.value.trim() == "" || escolaridad.value == " ")
            {
                escolaridad.readOnly = false;
            }
            if (entidadP.value.trim() == "" || entidadP.value == " ")
            {
                entidadP.readOnly = false;
            }
            if (responsable.value.trim() == "" || responsable.value == " ")
            {
                responsable.readOnly = false;
            } 
        }
        else {
            nombre.value = "";
            escolaridad.value = "";
            entidadP.value = "";
            responsable.value = "";
            if (codigo.value != '' && codigo.value != undefined) {
                ModalDialog(codigo, '*MBENEFICIARIO' + '*' + codigo.value, new Array());
            }
        }
    }

    function abrirEmergente(obj) {
        var nit = document.getElementById('_ctl1_' + obj);
        ModalDialog(nit, '**NITS_BENEFICIARIO', new Array(), 1);
    }

    function abrirEmergenteCedula(obj) {
        var nit = document.getElementById('_ctl1_' + obj);
        ModalDialog(nit, '**NITS_CEDULABENEFICIARIO', new Array(), 1);
    }

        function VerificarCedula(obj) {
            RegistrarActividad.Verificar_Cedula(obj.value, Verificar_Cedula_CallBack);
    }
        function Verificar_Cedula_CallBack(response) {
        var respuesta = response.value;
        var contactos = "";
        var codigo = document.getElementById("<%=txtCodigo.ClientID%>");
        var cedula = document.getElementById("<%=txtCedula.ClientID%>");
        var nombre = document.getElementById("<%=txtNombre.ClientID%>");
        var escolaridad = document.getElementById("<%=txtEscolaridad.ClientID%>");
        var entidadP = document.getElementById("<%=txtEntidad.ClientID%>");
        var responsable = document.getElementById("<%=txtResponsable.ClientID%>");
        var tablaDatos = document.getElementById("<%=infoDatos.ClientID%>");
        var labelError = document.getElementById("<%=lbInfoNit.ClientID%>");
        var btnGuardar = document.getElementById("<%=btnRegistrar.ClientID%>");
        try {
            if (respuesta.Tables[0].Rows.length > 0) {
                contactos = "Se han encontrado contactos registrados por este cliente: \n\n";

                for (var i = 0; i < respuesta.Tables[1].Rows.length; i++) {
                    contactos = contactos + respuesta.Tables[1].Rows[i].CONTACTOS + '\n\n'
                }
                alert(contactos);
            }

        } catch (e) { }
        
        nombre.readOnly = true;
        escolaridad.readOnly = true;
        entidadP.readOnly = true;
        responsable.readOnly = true;

        if (respuesta.Tables[0].Rows.length > 0)
        {
            $(labelError).text("");
            $(tablaDatos).show();
            nombre.value = respuesta.Tables[0].Rows[0].NOMBRE;
            codigo.value = respuesta.Tables[0].Rows[0].CODIGO;
            escolaridad.value = respuesta.Tables[0].Rows[0].ESCOLARIDAD;
            entidadP.value = respuesta.Tables[0].Rows[0].ENTIDADP;
            responsable.value = respuesta.Tables[0].Rows[0].RESPONSABLE;
            if (nombre.value.trim() == "" || nombre.value == " ")
            {
                nombre.readOnly = false;
            }
            if (escolaridad.value.trim() == "" || escolaridad.value == " ")
            {
                escolaridad.readOnly = false;
            }
            if (entidadP.value.trim() == "" || entidadP.value == " ")
            {
                entidadP.readOnly = false;
            }
            if (responsable.value.trim() == "" || responsable.value == " ")
            {
                responsable.readOnly = false;
            } 
        }
        else {
            nombre.value = "";
            escolaridad.value = "";
            entidadP.value = "";
            responsable.value = "";
            if (cedula.value != '' && cedula.value != undefined) {
                ModalDialog(cedula, '*MBENEFICIARIO' + '*' + cedula.value, new Array());
            }
        }
    }


</script>
<fieldset width="50%">
    <legend class=Legends>Resgitro de Actividades</legend>
     <table id="Table1" class="filtersIn">
         <tr>
             <td>
             <asp:Label ID="lbGad" runat="server">GAD</asp:Label><br />
             <asp:DropDownList ID="ddlGad" AutoPostBack="True" CssClass="dmediano" runat="server" OnSelectedIndexChanged="ddlgad_CargarProyecto"></asp:DropDownList>
             </td>
         </tr>
          <tr>
             <td>
             <asp:Label ID="lbProyecto" runat="server">Proyecto</asp:Label><br />
             <asp:DropDownList ID="ddlProyecto" AutoPostBack="True" CssClass="dmediano" runat="server" OnSelectedIndexChanged="ddlproyecto_CargarActividad"></asp:DropDownList>
             </td>
         </tr>
         <tr>
             <td>
                 <asp:Label ID="lbActividad" runat="server">Actividades</asp:Label><br />
                 <asp:DropDownList ID="ddlActividad" AutoPostBack="True" CssClass="dmediano" runat="server" OnSelectedIndexChanged="ddlproyecto_CargarDatos"></asp:DropDownList>
             </td>
         </tr>
         <tr>
             <td>
             <asp:Label ID="lbObj" runat="server">Objetivos</asp:Label><br />
             <asp:DropDownList ID="ddlObj" CssClass="dmediano" runat="server"></asp:DropDownList>
             </td>
         </tr>
         <tr>
             <td>
             <asp:Label ID="lbResult" runat="server">Resultados del Objetivo</asp:Label><br />
             <asp:DropDownList ID="ddlResult" CssClass="dmediano" runat="server"></asp:DropDownList>
             </td>
         </tr>
         <tr>
             <td>
                 <asp:Label ID="lbIndicadores" runat="server">Indicadores</asp:Label><br />
                 <asp:DropDownList ID="ddlIndicador" CssClass="dmediano" runat="server"></asp:DropDownList>
             </td>
         </tr>
     </table>
     <fieldset>     
     <table id="infoDatos" class="filtersIn" runat="server">
      <tr>
          <td>
             <asp:Label ID="Label1" runat="server">Beneficiario</asp:Label><br />
             <asp:TextBox ID="txtCodigo" onblur="VerificarCliente(this);" placeholder="Codigo" CssClass="tmediano" runat="server"></asp:TextBox>
             <asp:Image id="imglupa1" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente('txtCodigo');"></asp:Image><br />
             <asp:Label ID="lbInfoNit" runat="server" style="font-size: large; font-family: unset; color: red;"></asp:Label>
          </td>
          <td>
             <asp:TextBox ID="txtCedula" placeholder="Cedula" CssClass="tmediano" onblur="VerificarCedula(this);" runat="server"></asp:TextBox>
             <asp:Image id="Image1" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergenteCedula('txtCedula');"></asp:Image><br />
          </td>
       </tr>
       <tr>
         <td>
            <asp:Label ID="lbNombre" runat="server">Nombre</asp:Label><br />
            <asp:TextBox ID="txtNombre" CssClass="tmediano" runat="server"></asp:TextBox>
          </td>
       </tr>
       <tr>
            <td>
                <asp:Label ID="lbGrado" runat="server">Grado de Escolaridad</asp:Label><br />
                <asp:TextBox ID="txtEscolaridad" CssClass="tmediano" runat="server"></asp:TextBox>
            </td>
       </tr>
        <tr>
            <td>
                <asp:Label ID="lbRespo" runat="server">Entidad Patrocinadora</asp:Label><br />
                <asp:TextBox ID="txtEntidad" CssClass="tmediano" runat="server"></asp:TextBox>
            </td>
       </tr>
       <tr>
         <td>
            <asp:Label ID="Label2" runat="server">Responsable</asp:Label><br />
            <asp:TextBox ID="txtResponsable" CssClass="tmediano" runat="server"></asp:TextBox>
         </td>
       </tr>
       </table>
       </fieldset>
       <tr>
           <td>
                <asp:button id="btnRegistrar" runat="server" OnClick="Registrar_Actividad" Text="Registrar"></asp:button>
                <asp:Label ID="lError" runat="server" style="font-size: large; font-family: unset; color: red;"></asp:Label>
           </td>
       </tr>
     </asp:Panel>
 </fieldset>
