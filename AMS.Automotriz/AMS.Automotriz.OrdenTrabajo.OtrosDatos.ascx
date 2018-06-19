 <%@ Control Language="c#" codebehind="AMS.Automotriz.OrdenesTaller.OtrosDatos.ascx.cs" autoeventwireup="True" Inherits="AMS.Automotriz.OtrosDatos" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
 
 <script language='javascript' src='../js/jquery.ui.touchpunch.min.js' type='text/javascript'></script>

 <link href="../css/AMS.CarroTouch.css" type="text/css" rel="stylesheet" />

<script language:javascript>
    $(function () {

        $("#btnLimpiar").click(function () {
            $("#cont_gdelantizq").text("");
            $("#cont_capo").text("");
            $("#cont_gdelantdere").text("");
            $("#cont_pdelantizq").text("");
            $("#cont_ptrasizq").text("");
            $("#cont_techo").text("");
            $("#cont_pdelantdere").text("");
            $("#cont_ptrasder").text("");
            $("#cont_gtrasizq").text("");
            $("#cont_maletero").text("");
            $("#cont_gtrasder").text("");

            var idGdelantizq = "<%= txt_gdelantizq.ClientID %>";
            document.getElementById(idGdelantizq).value = "";
            var idCapo = "<%= txt_capo.ClientID %>";
            document.getElementById(idCapo).value = "";
            var idGdelantdere = "<%= txt_gdelantdere.ClientID %>";
            document.getElementById(idGdelantdere).value = "";
            var idPdelantizq = "<%= txt_pdelantizq.ClientID %>";
            document.getElementById(idPdelantizq).value = "";
            var idPtrasizq = "<%= txt_ptrasizq.ClientID %>";
            document.getElementById(idPtrasizq).value = "";
            var idTecho = "<%= txt_techo.ClientID %>";
            document.getElementById(idTecho).value = "";         
            var idPdelantdere = "<%= txt_pdelantdere.ClientID %>";
            document.getElementById(idPdelantdere).value = "";    
            var idPtrasder = "<%= txt_ptrasder.ClientID %>";
            document.getElementById(idPtrasder).value = "";
            var idGtrasizq = "<%= txt_gtrasizq.ClientID %>";
            document.getElementById(idGtrasizq).value = "";            
            var idMaletero = "<%= txt_maletero.ClientID %>";
            document.getElementById(idMaletero).value = "";
            var idGtrasder = "<%= txt_gtrasder.ClientID %>";
            document.getElementById(idGtrasder).value = "";


        });

        $("#golpe").draggable({ helper: "clone" });
        $("#mancha").draggable({ helper: "clone" });
        $("#raya").draggable({ helper: "clone" });

        $("#gdelantizq").droppable({
            drop: function (event, ui) {
                var opcion = "";
                var cont_gdelantizq = $("#cont_gdelantizq").text();

                if (ui.draggable.attr("id").indexOf("golpe") != -1) {
                    opcion = "X";
                } else if (ui.draggable.attr("id").indexOf("mancha") != -1) {
                    opcion = "O";
                } else if (ui.draggable.attr("id").indexOf("raya") != -1) {
                    opcion = "-";
                }

                if (cont_gdelantizq.indexOf(opcion) == -1)
                    cont_gdelantizq += opcion;

                $("#cont_gdelantizq").text(cont_gdelantizq);
                var idGdelantizq = "<%= txt_gdelantizq.ClientID %>";
                document.getElementById(idGdelantizq).value = cont_gdelantizq;
            }
        });
        $("#capo").droppable({
            drop: function (event, ui) {
                var opcion = "";
                var cont_capo = $("#cont_capo").text();

                if (ui.draggable.attr("id").indexOf("golpe") != -1) {
                    opcion = "X";
                } else if (ui.draggable.attr("id").indexOf("mancha") != -1) {
                    opcion = "O";
                } else if (ui.draggable.attr("id").indexOf("raya") != -1) {
                    opcion = "-";
                }

                if (cont_capo.indexOf(opcion) == -1)
                    cont_capo += opcion;

                $("#cont_capo").text(cont_capo);
                var idCapo = "<%= txt_capo.ClientID %>";
                document.getElementById(idCapo).value = cont_capo;
            }
        });
        $("#gdelantdere").droppable({
            drop: function (event, ui) {
                var opcion = "";
                var cont_gdelantdere = $("#cont_gdelantdere").text();

                if (ui.draggable.attr("id").indexOf("golpe") != -1) {
                    opcion = "X";
                } else if (ui.draggable.attr("id").indexOf("mancha") != -1) {
                    opcion = "O";
                } else if (ui.draggable.attr("id").indexOf("raya") != -1) {
                    opcion = "-";
                }

                if (cont_gdelantdere.indexOf(opcion) == -1)
                    cont_gdelantdere += opcion;

                $("#cont_gdelantdere").text(cont_gdelantdere);
                var idGdelantdere = "<%= txt_gdelantdere.ClientID %>";
                document.getElementById(idGdelantdere).value = cont_gdelantdere;
            }
        });






        $("#pdelantizq").droppable({
            drop: function (event, ui) {
                var opcion = "";
                var cont_pdelantizq = $("#cont_pdelantizq").text();

                if (ui.draggable.attr("id").indexOf("golpe") != -1) {
                    opcion = "X";
                } else if (ui.draggable.attr("id").indexOf("mancha") != -1) {
                    opcion = "O";
                } else if (ui.draggable.attr("id").indexOf("raya") != -1) {
                    opcion = "-";
                }

                if (cont_pdelantizq.indexOf(opcion) == -1)
                    cont_pdelantizq += opcion;

                $("#cont_pdelantizq").text(cont_pdelantizq);
                var idPdelantizq = "<%= txt_pdelantizq.ClientID %>";
                document.getElementById(idPdelantizq).value = cont_pdelantizq;
            }
        });
        $("#ptrasizq").droppable({
            drop: function (event, ui) {
                var opcion = "";
                var cont_ptrasizq = $("#cont_ptrasizq").text();

                if (ui.draggable.attr("id").indexOf("golpe") != -1) {
                    opcion = "X";
                } else if (ui.draggable.attr("id").indexOf("mancha") != -1) {
                    opcion = "O";
                } else if (ui.draggable.attr("id").indexOf("raya") != -1) {
                    opcion = "-";
                }

                if (cont_ptrasizq.indexOf(opcion) == -1)
                    cont_ptrasizq += opcion;

                $("#cont_ptrasizq").text(cont_ptrasizq);
                var idPtrasizq = "<%= txt_ptrasizq.ClientID %>";
                document.getElementById(idPtrasizq).value = cont_ptrasizq;
            }
        });

        $("#techo").droppable({
            drop: function (event, ui) {
                var opcion = "";
                var cont_techo = $("#cont_techo").text();

                if (ui.draggable.attr("id").indexOf("golpe") != -1) {
                    opcion = "X";
                } else if (ui.draggable.attr("id").indexOf("mancha") != -1) {
                    opcion = "O";
                } else if (ui.draggable.attr("id").indexOf("raya") != -1) {
                    opcion = "-";
                }

                if (cont_techo.indexOf(opcion) == -1)
                    cont_techo += opcion;

                $("#cont_techo").text(cont_techo);
                var idTecho = "<%= txt_techo.ClientID %>";
                document.getElementById(idTecho).value = cont_techo;
            }
        });

        $("#pdelantdere").droppable({
            drop: function (event, ui) {
                var opcion = "";
                var cont_pdelantdere = $("#cont_pdelantdere").text();

                if (ui.draggable.attr("id").indexOf("golpe") != -1) {
                    opcion = "X";
                } else if (ui.draggable.attr("id").indexOf("mancha") != -1) {
                    opcion = "O";
                } else if (ui.draggable.attr("id").indexOf("raya") != -1) {
                    opcion = "-";
                }

                if (cont_pdelantdere.indexOf(opcion) == -1)
                    cont_pdelantdere += opcion;

                $("#cont_pdelantdere").text(cont_pdelantdere);
                var idPdelantdere = "<%= txt_pdelantdere.ClientID %>";
                document.getElementById(idPdelantdere).value = cont_pdelantdere;
            }
        });
        
        
        $("#ptrasder").droppable({
            drop: function (event, ui) {
                var opcion = "";
                var cont_ptrasder = $("#cont_ptrasder").text();

                if (ui.draggable.attr("id").indexOf("golpe") != -1) {
                    opcion = "X";
                } else if (ui.draggable.attr("id").indexOf("mancha") != -1) {
                    opcion = "O";
                } else if (ui.draggable.attr("id").indexOf("raya") != -1) {
                    opcion = "-";
                }

                if (cont_ptrasder.indexOf(opcion) == -1)
                    cont_ptrasder += opcion;

                $("#cont_ptrasder").text(cont_ptrasder);
                var idPtrasder = "<%= txt_ptrasder.ClientID %>";
                document.getElementById(idPtrasder).value = cont_ptrasder;
            }
        });

        $("#gtrasizq").droppable({
            drop: function (event, ui) {
                var opcion = "";
                var cont_gtrasizq = $("#cont_gtrasizq").text();

                if (ui.draggable.attr("id").indexOf("golpe") != -1) {
                    opcion = "X";
                } else if (ui.draggable.attr("id").indexOf("mancha") != -1) {
                    opcion = "O";
                } else if (ui.draggable.attr("id").indexOf("raya") != -1) {
                    opcion = "-";
                }

                if (cont_gtrasizq.indexOf(opcion) == -1)
                    cont_gtrasizq += opcion;

                $("#cont_gtrasizq").text(cont_gtrasizq);
                var idGtrasizq = "<%= txt_gtrasizq.ClientID %>";
                document.getElementById(idGtrasizq).value = cont_gtrasizq;
            }
        });

        $("#maletero").droppable({
            drop: function (event, ui) {
                var opcion = "";
                var cont_maletero = $("#cont_maletero").text();

                if (ui.draggable.attr("id").indexOf("golpe") != -1) {
                    opcion = "X";
                } else if (ui.draggable.attr("id").indexOf("mancha") != -1) {
                    opcion = "O";
                } else if (ui.draggable.attr("id").indexOf("raya") != -1) {
                    opcion = "-";
                }

                if (cont_maletero.indexOf(opcion) == -1)
                    cont_maletero += opcion;

                $("#cont_maletero").text(cont_maletero);
                var idMaletero = "<%= txt_maletero.ClientID %>";
                document.getElementById(idMaletero).value = cont_maletero;
            }
        });

        $("#gtrasder").droppable({
            drop: function (event, ui) {
                var opcion = "";
                var cont_gtrasder = $("#cont_gtrasder").text();

                if (ui.draggable.attr("id").indexOf("golpe") != -1) {
                    opcion = "X";
                } else if (ui.draggable.attr("id").indexOf("mancha") != -1) {
                    opcion = "O";
                } else if (ui.draggable.attr("id").indexOf("raya") != -1) {
                    opcion = "-";
                }

                if (cont_gtrasder.indexOf(opcion) == -1)
                    cont_gtrasder += opcion;

                $("#cont_gtrasder").text(cont_gtrasder);
                var idGtrasder = "<%= txt_gtrasder.ClientID %>";
                document.getElementById(idGtrasder).value = cont_gtrasder;
            }
        });

    });
</script>






   
    <table>
	    <tbody>
		    <tr>
			    <td>
				    <fieldset>
					    <legend class="Legends">Accesorios</legend>
					    <div id="divGrilla" style="OVERFLOW: auto;">
						    <asp:DataGrid id="accesorios" runat="server" AutoGenerateColumns="False">
							    <FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
							    <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
							    <AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
							    <ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
							    <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
							    <Columns>
								    <asp:TemplateColumn HeaderText="Incluido Si/No">
									    <ItemStyle HorizontalAlign="Center"></ItemStyle>
									    <ItemTemplate>
										    <asp:CheckBox ID="chbacc" Runat="server"></asp:CheckBox>
									    </ItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="Nombre">
									    <ItemTemplate>
										    <%# DataBinder.Eval(Container.DataItem, "NOMBRE") %>
									    </ItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="Cantidad y Detalle">
									    <ItemTemplate>
										    <asp:TextBox ID="tbcantacc" Runat="server" MaxLength="20"  onkeyup="aMayusculas(this)"></asp:TextBox>
									    </ItemTemplate>
								    </asp:TemplateColumn>
							    </Columns>
							    <PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
						    </asp:DataGrid>
					    </div>
				    </fieldset>
			    </td>
         
                <td>
                     <div id="carroceriaTouch" style="visibility: hidden">
                        <table id="tablaCarro"  >
                            <tr>
                                <td>
                                    <div id="gdelantizq">
                                            <asp:TextBox id="txt_gdelantizq" class="texb" runat="server" ></asp:TextBox>
                                        <img src="../img/guardabarrodelanteroizq.png" class="imagenAuto">
                                        <div id="cont_gdelantizq" class="contenidoPartes"></div>
                                    </div>
                                </td>
                                <td>
                                    <div id="capo">
                                            <asp:TextBox id="txt_capo" class="texb" runat="server" ></asp:TextBox>
                                        <img src="../img/capo.png" class="imagenAuto">
                                        <div id="cont_capo" class="contenidoPartes"></div>
                                    </div>
                                </td>
                                <td >
                                    <div id="gdelantdere">
                                        <asp:TextBox id="txt_gdelantdere" class="texb" runat="server" ></asp:TextBox>
                                        <img src="../img/guardabarrodelanteroder.png" class="imagenAuto">
                                        <div id="cont_gdelantdere" class="contenidoPartes"></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="pdelantizq">
                                            <asp:TextBox id="txt_pdelantizq" class="texb" runat="server" ></asp:TextBox>
                                        <img src="../img/puertadelantizq.png" class="imagenAuto">
                                        <div id="cont_pdelantizq" class="contenidoPartes"></div>
                                    </div>
                                </td>
                                <td rowspan="2">
                                    <div id="techo">
                                        <asp:TextBox id="txt_techo" class="texb" runat="server" ></asp:TextBox>
                                        <img src="../img/techo.png" class="imagenAuto">
                                        <div id="cont_techo" class="contenidoPartes"></div>  
                                    </div>
                                </td>
                                <td>
                                    <div id="pdelantdere">
                                            <asp:TextBox id="txt_pdelantdere" class="texb" runat="server" ></asp:TextBox>
                                        <img src="../img/puertadelander.png" class="imagenAuto">
                                        <div id="cont_pdelantdere" class="contenidoPartes"></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="ptrasizq">
                                        <asp:TextBox id="txt_ptrasizq" class="texb" runat="server" ></asp:TextBox>
                                        <img src="../img/puertatrasizq.png" class="imagenAuto">
                                        <div id="cont_ptrasizq" class="contenidoPartes"></div>
                                    </div>
                                </td>

                                <td>
                                    <div id="ptrasder">
                                        <asp:TextBox id="txt_ptrasder" class="texb" runat="server" ></asp:TextBox>
                                        <img src="../img/puertatrasder.png" class="imagenAuto">
                                        <div id="cont_ptrasder" class="contenidoPartes"></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="gtrasizq">
                                        <asp:TextBox id="txt_gtrasizq" class="texb" runat="server" ></asp:TextBox>
                                        <img src="../img/guardabarrotraseroizq.png" class="imagenAuto">
                                        <div id="cont_gtrasizq" class="contenidoPartes"></div>
                                    </div>
                                </td>
                                <td>
                                    <div id="maletero">
                                        <asp:TextBox id="txt_maletero" class="texb" runat="server" ></asp:TextBox>
                                        <img src="../img/maletero.png" class="imagenAuto">
                                        <div id="cont_maletero" class="contenidoPartes"></div>
                    
                                    </div>
                                </td>
                                <td>
                                    <div id="gtrasder">
                                        <asp:TextBox id="txt_gtrasder" class="texb" runat="server" ></asp:TextBox>
                                        <img src="../img/guardabarrotraseroder.png" class="imagenAuto">
                                        <div id="cont_gtrasder" class="contenidoPartes"></div>
                    
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div id="raya" class="botones">
                            <img src="../img/raya.png">
                            <br>Raya
                        </div>
                        <div id="mancha" class="botones">
                            <img src="../img/mancha.png">
                            <br>Mancha
                        </div>
                        <div id="golpe" class="botones">
                            <img src="../img/golpe.png">
                            <br>Golpe
                        </div>
                        <div id="btnLimpiar">limpiar</div>
                    </div>
 
                                       
                    <fieldset>
					    <legend class="Legends">Tipos_Plan (Garantia y Post_Venta)</legend>
                        TPG:
					    <asp:TextBox id="tp" Width="50px" runat="server" ReadOnly=true Visible=false></asp:TextBox>
					    <asp:DropDownList ID="ddltp" Runat="server" Visible=true></asp:DropDownList>
					    <BR>TPP:
					    <asp:TextBox id="tpp" Width="50px" runat="server" Visible =false ReadOnly=true></asp:TextBox><asp:TextBox id="tppVal" Width="200px" runat="server" ReadOnly=true></asp:TextBox>
				    </fieldset>
                   
				    <fieldset>
					    <legend class="Legends">Nivel de Combustible</legend>
					    <asp:RadioButtonList id="nivelCombustible" runat="server" RepeatDirection="Horizontal" RepeatColumns="3"
						    Height="24px"></asp:RadioButtonList>
				    </fieldset>
			   
				    <fieldset>
					    <legend class="Legends">Información Adicional</legend>
					    <table class="filtersIn">
						    <tr>
							    <td>
								    ¿Ofrecio medio alternativo de movilidad?
							    </td>
						    </tr>
						    <tr>
							    <td>
								    <asp:DropDownList ID="ddlencuesta" Runat="server"></asp:DropDownList>
                                    <asp:RequiredFieldValidator id="validatorDdlEncuesta" runat="server" Font-Name="Arial" Font-Size="11"
						ControlToValidate="ddlencuesta">*</asp:RequiredFieldValidator>
							    </td>
						    </tr>
						    <tr>
							    <td>
								    Vehículo ha sido revisado en elevador ?
							    </td>
						    </tr>
						    <tr>
							    <td>
								    <asp:DropDownList ID="ddlelevador" Runat="server"></asp:DropDownList>
                                    <asp:RequiredFieldValidator id="validatorDdlElevador" runat="server" Font-Name="Arial" Font-Size="11" 
						ControlToValidate="ddlelevador">*</asp:RequiredFieldValidator>
							    </td>
						    </tr>
						    <tr>
							    <td>
								    Va a entregar el presupuesto al Cliente?
							    </td>
						    </tr>
						    <tr>
							    <td>
								    <asp:DropDownList ID="ddlpresupuesto" Runat="server"></asp:DropDownList>
                                    <asp:RequiredFieldValidator id="validatorDdlPresupuesto" runat="server" Font-Name="Arial" Font-Size="11" 
						            ControlToValidate="ddlpresupuesto">*</asp:RequiredFieldValidator>
							    </td>
						    </tr>
					    </table>
				    </fieldset>
			    </td>
               
		    </tr>
		    <tr>
			    <td colspan="2">
				    <asp:Button id="confirmar" onclick="Confirmar" runat="server" Text="Validar" CausesValidation="False"></asp:Button>
			    </td>
		    
            </tr>
            

	    </tbody>
    </table>
    <asp:Label id="lb" runat="server"></asp:Label>
    <asp:ValidationSummary id="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False"></asp:ValidationSummary>


 