<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AMS.Automotriz.PanelPrecios.aspx.cs" Inherits="AMS.Automotriz.PanelPrecios" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head runat="server">
    <title>Panel de Precios</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <LINK href="../img/AMS.ico" type="image/ico" rel="icon">

    <script type="text/javascript" src="../js/jquery.js"></script>
    <script type="text/javascript" src="../js/modernizr.js"></script>
<style>
body{
    background-color: black;
    color: white;
    font-family: sans-serif;
}

#logoMarca{
    position: absolute;
    background: black;
    top: 10%;
    left: 14%;
    width: 123px;
    height: 123px;
}

#imagenAuto{
    position: absolute;
    background: black;
    top: 10%;
    left: 26%;
    width: 400px;
    height: 250px;
}

#logoGrupo{
    position: absolute;
    background: black;
    top: 10%;
    left: 52%;
    width: 188px;
    height: 67px;
    font-size: 30px;
    font-weight: bold;
}

#logoConcesionario{
    position: absolute;
    background: black;
    top: 10%;
    left: 68%;
    width: 220px;
    height: 110px;
}

#contenedorSlider{
    position: absolute;
    background: black;
    top: 43%;
    left: 8%;
    width: 1368px;
    height: 286px;
    overflow: hidden;
}

#sliderPrecios{
    position: absolute;
    background: black;
    width: 1400px;
    height: 286px;
    left: 500px;
    overflow: visible;
}

#informacion{
    position: absolute;
    background: black;
    top: 82%;
    left: 11%;
    width: 1160px;
    height: 65px;
    font-size: 19px;
}

#notas{
    position: absolute;
    background: black;
    top: 94%;
    left: 35%;
    width: 30%;
}

.tarjetaPrecio{
    background-color: black;
    width: 250px;
    padding: 10px;
    height: 266px;
    display: inline-block;
    overflow: hidden;
    font-size: 12px;
    font-stretch: condensed;
}

ul {
    padding: 5px;
}

.imagen{
    width: 100%;
    height: 100%;
}
</style>
<asp:ScriptManager ID="ScriptManager" runat="server" />
<script>
    var arrayGrupos;
    var cont = 0;
    var contFilas = 0;

    $(document).ready(function () {
        obtenerGrupos();
    });

    function obtenerGrupos()
    {
        PanelPrecios.GetGrupos(obtenerGrupos_CallBack);
    }

    function obtenerGrupos_CallBack(response)
    {
        arrayGrupos = response.value;
           
        cargarGrupo((arrayGrupos[cont])[0].Value);
    }

    function cargarGrupo(codigoGrupo)
    {
        PanelPrecios.GetListaPrecios(codigoGrupo, cargarGrupo_CallBack);
    }

    function cargarGrupo_CallBack(response)
    {
        var respuesta = response.value;
        
        if(respuesta.Rows.length!=0)
        {
            $("#sliderPrecios").html("");
            $("#logoGrupo").html(respuesta.Rows[0].GRUPO);
            $("#imagenAuto").html("<img class='imagen' src='" + "../uploads/" + respuesta.Rows[0].IMAGEN + "' />");

            var nfilas = respuesta.Rows.length / 5;
            var arrFilas = [];
            var w = 0;
            var finFicha = 0;

            //Se ingresa en un array el html necesario para construir filas de a 5 tarjetas por grupo de catalogo.
            for (y = 0; y < nfilas; y++) {
                var ficha = "";

                if ((y + 1) * 5 < respuesta.Rows.length)
                    finFicha = (y + 1) * 5;
                else
                    finFicha += (nfilas - y) * 5;

                for (w; w < finFicha; w++) {
                    ficha += "<div class='tarjetaPrecio'><font color='red' style='font-size: 21px;'>" +
                                respuesta.Rows[w].KM + " mil km</font><br /><br />" +
                                "<b><center><font size='6'>" + respuesta.Rows[w].VALOR + "</font></center><br />" +
                                "<font color='red' size='4' style='margin-left: -10px;'>Reemplazar</font><br /><br />" +
                                "<ul type = disk >" +
                                respuesta.Rows[w].ELEMENTO +
                                "</ul></b></div>";
                }
                arrFilas.push(ficha);
            }

            contFilas = 0;
            loopFilasGrupo(arrFilas);
        }
        else
        {
            incrementarGrupo();
        }
    }

    function loopFilasGrupo(arrFilas)
    {
        $("#sliderPrecios").html(arrFilas[contFilas]);


        $("#sliderPrecios").animate({ left: '0px' }, {
            duration: 700,
            specialEasing: {
                duration: 'slow',
                easing: 'easeOutBack'
            },
            complete: function () {
                setTimeout(function () {
                    $("#sliderPrecios").css({ left: '700px' });
                    contFilas++;
                    if (contFilas < arrFilas.length) {
                        loopFilasGrupo(arrFilas);
                    }
                    else {
                        incrementarGrupo();
                    }
                }, 5000);
            }
        });
    }

    function incrementarGrupo()
    {
        if (cont < (arrayGrupos.length - 1))
            cont++;
        else
            cont = 0;

        cargarGrupo((arrayGrupos[cont])[0].Value);
    }

</script>
</head>
<body>
    <form id="form3" runat="server">
        <div>
            <div id="logoMarca"><img src="..\img\logos_empresas\logoPrecios.png" /></div>
            <div id="imagenAuto"></div>
            <div id="logoGrupo"></div>
            <div id="logoConcesionario"><img src="..\img\logos_empresas\logoConcesionario.png" /></div>
            <div id="contenedorSlider">
                <div id="sliderPrecios">
                </div>
            </div>
            <div id="informacion">
                <font style="color: red">Todos los mantenimientos programados incluyen:</font> Funcionamiento del sistema eléctrico, nivel y estado de líquidos, 
                test de batería, inspección de frenos delanteros y traseros, inspección suspensiónb, rótulas y rodamientos, 
                presión y estado de llantas, estado de las plumillas limpiaparabrisas y verificación y diagnóstico computarizado Mazda M-MDS.</div>
            <div id="notas">Precios incluyen: Respuestos, Mano de obra e IVA. Repuestos adicionales se informará el precio</div>
        </div>
        <div id="log"></div>
    </form>
</body>
</html>
