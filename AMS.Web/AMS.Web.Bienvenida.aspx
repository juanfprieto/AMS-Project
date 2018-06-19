<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AMS.Web.Bienvenida.aspx.cs" Inherits="AMS.Web.Bienvenida" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit"%>

<!DOCTYPE html>
<html>
<head >
     <script type="text/javascript" src="../js/jquery.js"></script>
        <script type="text/javascript" src="../js/jquery-ui.js"></script>
        <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
        <script type="text/javascript" src="../js/ui/jquery.ui.core.js" ></script>
        <script type="text/javascript" src="../js/ui/jquery.ui.datepicker.js" ></script>
        <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
        <script type ="text/javascript" src="../js/generales.js"></script>
        <script type ="text/javascript" src="../js/angular.js"></script>
               
        <!--estilos, normalizacion y bootstrap-->
        <link href="../css/normalize.css" type="text/css" rel="stylesheet" />
        <link href="../css/AMS.css" type="text/css" rel="stylesheet" />
        <link href="../css/bootstrap.min.css" rel="stylesheet"/>
        <link href="../css/bootstrap-theme.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="../js/bootstrap.min.js"></script>
        <script type="text/javascript" src="../js/npm.js"></script>
        <!-- fin de estilos, normalizacion y bootstrap-->
        <link href="../css/AMS.Menu.css" type="text/css" rel="stylesheet" media="screen" />        
        <link href="../css/tabber.css" type="text/css" rel="stylesheet" media="screen" />
        <link href="../css/jquery-ui.css" type="text/css" rel="stylesheet" media="screen" />
        

		<meta charset="utf-8" />
    <script type="text/javascript">
        var variable;
        //$(document).on("ready", cambioMensaje1());
        $(document).ready(function () {
            //setTimeout(cambioMensaje1, 5000);
            setInterval(cambioMensaje1, 7200);
            //alert('ki onda');
            //inicio(j);
            //j++;
            //cambioMensaje1();
            //cambioMensaje();
        });

        function cambioMensaje1()
        {
            Bienvenida.mostrarMensaje(cambiar_Mensaje_Callback);
            //setTimeout(cambioMensaje1(), 5000);
        }
        function cambiar_Mensaje_Callback(response)
        {
            respuesta = response.value;
            //alert('la prueba sí funciona');
            $('#' + '<%=mensajeAct.ClientID%>').animate
            (
                {
                "right": "150px",
                opacity: '0'
                },
                {
                    easing: 'swing',
                    duration: 900,
                    complete: function ()
                    {
                        $('#' + '<%=mensajeAct.ClientID%>').animate(
                            {
                                "right": "0px",
                                opacity: '1'
                            },
                            {
                                easing: 'swing',
                                duration: 800,
                                complete: function ()
                                {
                                    $('#' + '<%=mensajeAct.ClientID%>').animate(
                                        {
                                            color: "red"
                                        },450
                                    ).animate(
                                    {
                                        color: "black"
                                    }, 450)
                                }
                            }
                        ).text(respuesta)
                    }
                }
            );
            
        }
    </script> 
</head>
<body>
    <form id="frm" runat="server" method="POST">
        <div class="centerWelcome" >
            <asp:Label id="lblUserWelcome"  runat="server" style="position: relative; z-index: 1;"></asp:Label> <br /><br />
            
            <asp:Image runat="server" ImageAlign="Middle" style="position:relative;" ImageUrl="../img/jMenu/welcome3.png"></asp:Image> 
            <br /><br /><br />
            <%--<asp:Label id="mensajeAct" runat="server" style="font-size:15px;"> Recuerde, todos los días de 1:00 pm a 1:30 pm se realizan actualizaciones, favor <b>NO</b> ingresar al aplicativo en ese lapso de tiempo.</asp:Label> <br />--%>
            <asp:Label id="mensajeAct" runat="server" style="position:relative; font-size: 18px;" ></asp:Label> <br />
         
            <%--<asp:TextBox ID="txtAngular" runat="server" ng-model="testing" Width="150px" style="top:140px"></asp:TextBox><br />--%> 
          
         
            <!--prueba dumi Angular JS -->
            <%--<input type="text" ng-model="testing" style="position:relative; width:150px;top:120px"/>
            <asp:Label runat="server" style="position:relative; top:130px;">Hola!! testing</asp:Label>--%>

            <!-- boton ajax que evita postback -->
            <%--<asp:Button id="btn" OnclientClick="cambioMensaje1(); return false" runat="server"/>--%>
             
        </div>
        
    </form>
</body>
</html>
