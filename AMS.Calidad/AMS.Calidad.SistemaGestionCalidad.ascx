<%@ Control Language="c#" AutoEventWireup="True" 
Codebehind="AMS.Calidad.SistemaGestionCalidad.ascx.cs" Inherits="AMS.Calidad.SistemaGestionCalidad" %>
<script type="text/javascript">
    function mostrarDocumento()
    {
        var valor = document.getElementById('<%=ddlprocDocs.ClientID%>').value;
        SistemaGestionCalidad.Abrir_Documento(valor,abrir_Documento_CallBack);
    }

    function abrir_Documento_CallBack(response) {
        var respuesta = response.value;
        if (respuesta != "" && respuesta != undefined) {
            $("#divDocument").html(respuesta);
            $("#divContDocument").css("visibility", "visible");
            alert(PDF)
        }
        else {
            $("#divDocument").html("");
            $("#divContDocument").css("visibility", "hidden");
            alert("No se ha escrito este procedimiento!")
        }
    }
    
    function mostrarformato()
    {
        var valor = document.getElementById('<%=ddlform.ClientID%>').value;
        SistemaGestionCalidad.Abrir_Formato(valor,abrir_Formato_CallBack);
    }

    function abrir_Formato_CallBack(response) {
        var respuesta = response.value;
        if (respuesta != "" && respuesta != undefined) {
            $("#divDocument").html(respuesta);
            $("#divContDocument").css("visibility", "visible");
            alert(PDF)
        }
        else {
            $("#divDocument").html("");
            $("#divContDocument").css("visibility", "hidden");
            alert("No se ha escrito este procedimiento!")
        }
    }

    function cerrarDocumento() {
        $("#divContDocument").css("visibility", "hidden");
    }
</script>
<fieldset>
<TABLE id="Table" class="filtersIn">
	<TR>
		<TD>
			<asp:Label id="Label1" runat="server">Procedimiento</asp:Label></TD>
		<TD></TD>
		<TD>
			<asp:Label id="Label2" runat="server">Formato</asp:Label></TD>
	</TR>
	<TR>
		<TD>
			<asp:DropDownList id="ddlproc" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlproc_SelectedIndexChanged"></asp:DropDownList></TD>
		<TD>&nbsp;&nbsp;</TD>
		<TD>&nbsp;
			<asp:DropDownList id="ddlform" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlform_SelectedIndexChanged"></asp:DropDownList>
         <br /><asp:Button id="Button2" runat="server" Text="Mostrar Formato" OnClientClick="mostrarformato(); return false;"  CssClass="noEspera"></asp:Button>
            </TD>
	</TR>
    <TR>
		<TD>
            <asp:DropDownList id="ddlprocDocs" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlprocDocs_Changed"></asp:DropDownList>
<br /><asp:Button id="btnMostrarDoc" runat="server" Text="Mostrar Documento" OnClientClick="mostrarDocumento(); return false;"  CssClass="noEspera"></asp:Button></TD>
        <TD>&nbsp;&nbsp;</TD>
    </TR>
</TABLE>
<P></P>
<asp:Button id="Button1" runat="server" Text="Reiniciar" OnClick="Button1_Click"></asp:Button>
</fieldset>

 <div id="divContDocument" style="
        width: 698px;
    height: 680px;
    position: absolute;
    top: 90px;
    left:20%;
    overflow: hidden; visibility: hidden">

        <div id="divDocument" 
            style="position: absolute;
        background-color: white;
        width: 670px;
        height: 650px;
        left: 1px;
        top: 1px;
        padding: 70px;
        overflow-y: scroll;
        box-shadow: 4px 10px 22px #888888;
        border-radius: 4px;
        border-style: solid;"></div>
        
       <asp:Button id="btnCerrarDocument" runat="server" Text="Cerrar"  
            style="padding: 1px;
        position: absolute;
        top: 14px;
        right: 43px;" OnClientClick="cerrarDocumento(); return false;" CssClass="noEspera"></asp:Button>
     </div>