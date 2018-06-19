<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Automotriz.DatosCliente.ascx.cs" Inherits="AMS.Automotriz.DatosCliente" %>
	<link href="../css/semaforo.css" type="text/css" rel="stylesheet" />
    <table class="filters" style="height: 4px">

    <script language:javascript>

        function operar(objCambiar) {
            if (objCambiar == 'imgrojo') {
                $("#" + "<%= HiColores.ClientID %>").val("rojo");
                $("#imgrojo").attr("src", "../img/semaforo/rojo.png");
                $("#imgamarillo").attr("src", "../img/semaforo/amarillo2.png");
                $("#imgverde").attr("src", "../img/semaforo/verde2.png");
            }
            else if (objCambiar == 'imgamarillo') {
                $("#" + "<%= HiColores.ClientID %>").val("amarillo");
                $("#imgamarillo").attr("src", "../img/semaforo/amarillo.png");
                $("#imgrojo").attr("src", "../img/semaforo/rojo2.png");
                $("#imgverde").attr("src", "../img/semaforo/verde2.png");
            }
            else if (objCambiar == 'imgverde'){
                $("#" + "<%= HiColores.ClientID %>").val("verde");
                $("#imgverde").attr("src", "../img/semaforo/verde.png");
                $("#imgamarillo").attr("src", "../img/semaforo/amarillo2.png");
                $("#imgrojo").attr("src", "../img/semaforo/rojo2.png");
            }
        }

        $(function () {
            var hVal1 = $("#" + "<%= HiColores.ClientID %>").val();
            if (hVal1 != "") {
                hVal1 = "../img/semaforo/" + hVal1 + ".png";
                if (hVal1.indexOf("rojo") != -1) {
                    $("#imgrojo").attr("src", hVal1);
                }
                else if (hVal1.indexOf("amarillo") != -1) {
                    $("#imgamarillo").attr("src", hVal1);
                }
                else if (hVal1.indexOf("verde") != -1) {
                    $("#imgverde").attr("src", hVal1);
                }
            }
            else
                $("#imgverde").attr("src", "../img/semaforo/verde.png");
        });

       
      </script>
		<tbody>
			<tr>            
				<table id="Table" class="filtersIn">
					<td>Datos Cliente</td>
						<tr>
							<td><asp:Label  ID="lblPrimerNombre" runat="server" Text="Primer Nombre :"></asp:Label></td>
							<td colspan="3"><asp:textbox id="nombre" runat="server" EnableViewState="False"></asp:textbox></td>
							<td><asp:Label  ID="lblSegundoNombre" runat="server" Text="Segundo Nombre :"></asp:Label></td>
							<td colspan="3"><asp:textbox id="nombre2" runat="server" EnableViewState="False"></asp:textbox></td>
						   
						</tr>
						<tr>
						 <td><asp:Label  ID="lblPrimerApellido" runat="server" Text="Primer Apellido :"></asp:Label></td>
							<td colspan="3"><asp:textbox id="apellido" runat="server" EnableViewState="False"></asp:textbox></td>
							<td><asp:Label  ID="lblSegundoApellido" runat="server" Text="Segundo Apellido :"></asp:Label></td>
							<td colspan="3"><asp:textbox id="apellido2" runat="server" EnableViewState="False"></asp:textbox></td>
						</tr>
						<tr>
							<td>Telefono :</td>
							<td><asp:textbox id="telefono" runat="server" class="tpequeno"></asp:textbox></td>
							<td align=right>Movil :</td>
							<td><asp:textbox id="celular" runat="server" class="apequeno"></asp:textbox></td>
						</tr>
						<tr>
							<td>Correo Electrónico:</td>
							<td colspan="3"><asp:textbox id="correo" runat="server"></asp:textbox></td>
                            <td>Estado cliente:</td>
                            <td> 
                            <ul id= "semaforo">
                                <asp:HiddenField ID="HiColores"  runat="server" Value=""/>
                                <li><div id="divrojo"  onclick="operar('imgrojo');"><img id="imgrojo" src="../img/semaforo/rojo2.png" /></div></li>
                                <li><div id="divamarillo" onclick="operar('imgamarillo');"><img id="imgamarillo"  src = "../img/semaforo/amarillo2.png"/></div></li>  
                                <li><div id="divverde" runat="server" onclick="operar('imgverde');" Default = "true"><img id="imgverde"  src = "../img/semaforo/verde2.png"/></div></li>
                            </ul>
                            </td>
					   </tr>
                                                                   				  
				</table>
				
				<asp:button id="GuardarCliente" onclick="GuardarCliente_OnClick" runat="server" Text="Guardar Cliente"></asp:button>
			 
			</tr>
		</tbody>
	</table>