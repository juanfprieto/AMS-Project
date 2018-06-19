<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AMS.Web.JMenu.aspx.cs" Inherits="AMS.Web.JMenu" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="../css/jqtree.css" type="text/css" media="screen" />
    <script type="text/javascript" src="../js/jquery.js"></script>
    <script type="text/javascript" src="../js/tree.jquery.js"></script>
    <link rel="stylesheet" href="../css/tabber.css" type="text/css" media="screen" />
    
    
    <script type="text/javascript">
        $(document).on("ready", inicio);

        function inicio() {
            JMenu.Cargar_OpcionesMenu('', Cargar_Opciones_CallBack);
        }

        function Cargar_Opciones_CallBack(response) {
            var respuesta = response.value;
            var data = [];

            for (var i = 0; i < respuesta.Tables[0].Rows.length; i++) {
                var optionHijo = [];
                var padre = respuesta.Tables[0].Rows[i].PADRE;

                for (var j = 0; j < respuesta.Tables[1].Rows.length; j++) {
                    if (padre == respuesta.Tables[1].Rows[j].PADRE) {
                        var optionNieto = [];
                        var hijo = respuesta.Tables[1].Rows[j].HIJO;

                        for (var k = 0; k < respuesta.Tables[2].Rows.length; k++) {
                            if (hijo == respuesta.Tables[2].Rows[k].HIJO) {
                                var urlMenu = respuesta.Tables[2].Rows[k].URL + "&cod=&name=&path=" + respuesta.Tables[2].Rows[k].NIETO;
                                var iconMenu = '<img src="../img/jMenu/iconMenu.png">';
                                var textMenu = '<a href="javascript:cargarArchivo(\'' + urlMenu + '\')">' + respuesta.Tables[2].Rows[k].NIETO + '</a>';
                                var optionN =
                                    { label: iconMenu + ' ' + textMenu,
                                        id: "NNN" + k
                                    };

                                optionNieto.push(optionN);
                            }
                        }

                        var optionH =
                            { label: hijo,
                                id: "HH" + j,
                              children: optionNieto
                           };

                        optionHijo.push(optionH);
                    }
                }
                
                var option =
                    { label: padre,
                        id: "P" + i,
                      children: optionHijo
                    };

                data.push(option);
            }
            
            var urlMenuT = "AMS.Web.CerrarSesion.aspx";
            var iconMenuT = '<img src="../img/jMenu/iconMenu.png">';
            var textMenuT = '<a href="javascript:cargarArchivo(\'' + urlMenuT + '\')" >Terminar Sesion</a>';
            var optionT =
                 { label: iconMenuT + ' ' + textMenuT,
                    id: "T"
                };

            data.push(optionT);

            if (respuesta.Tables[4].Rows[0].GEMP_NOMBRE === "sac" || respuesta.Tables[4].Rows[0].GEMP_NOMBRE === "aspx")
            {
                if (respuesta.Tables[3].Rows[0].TIPO_USUARIO === "AS" || respuesta.Tables[3].Rows[0].TIPO_USUARIO === "SP") {
                    var urlMenuT = "AMS.Web.index.aspx?process=Tools.Actualizaciones";
                    var iconMenuT = '<br /><br /> <br /><img src="../img/jMenu/iconMenu.png">';
                    var textMenuT = '<a href="javascript:cargarArchivo(\'' + urlMenuT + '\')">ACTUALIZA MANUAL</a>';
                    var optionT =
                     {
                         label: iconMenuT + ' ' + textMenuT,
                         id: "T"
                     };
                    data.push(optionT);
                }
            }

            $('#tree1').tree({
                data: data,
                autoEscape: false,
                autoOpen: false,
                saveState: false,
                closedIcon: $('<img src="../img/jMenu/closeFolder.png">'),
                openedIcon: $('<img src="../img/jMenu/openFolder.png">')
            });

            $('#tree1').on("click", function (e) {
                var node = $('#tree1').tree('getSelectedNode');
                $('#tree1').tree('toggle', node);
                if (node.id.indexOf('NNN') == -1 && node.id.indexOf('T') == -1) {
                    $('#tree1').tree('selectNode', null);
                }
            });
        }
    </script>
    <style>
        #menuContenedor
        {
            font-size: 11px;
            font-family: tahoma;
        }
        a {
            text-decoration: none;
            color: #000000;
        }
        ul.tabbernav {
            font: normal 11px Verdana, sans-serif;
        }
        .tabbertab {
            overflow-x: hidden;
        }
        .tabberlive {
            width: 102%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="tabber" id="mytab1">
      <div class="tabbertab" title="Menu Ecas">
        <asp:Label ID="lblEmpresa" Runat="server" style="font-family: tahoma;"></asp:Label>
        <div id="menuContenedor">
            <div id="tree1" style="width: 450px;"></div>
        </div>
        </div></div>
    </form>
</body>
</html>
