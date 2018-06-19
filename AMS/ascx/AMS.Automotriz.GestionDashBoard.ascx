<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Automotriz.GestionDashBoard.ascx.cs" Inherits="AMS.Automotriz.GestionDashBoard" %>
<style type="text/css">
    body 
    {
        background-image: url('../img/fondoDashBoard.jpg');
    }
    .imgBoard
    {
        width: 100%;
        height: 100%;
    }
    #divArea
    {
        position: absolute;
        width:60%;
        height: 65%;
        background-color: transparent;
        left:20%;
        /*background-color: Blue;*/
    }
    .Area
    {
        position: absolute;
        width:60%;
        height: 65%;
        background-color: transparent;
        left:20%;
        /*background-color: Blue;*/
    }
    #divFondoA
    {
        position: absolute;
        width: 50%;
        left:0%;
        height: 70%;
        top:-1%;
    }
    #divFondoB
    {
        position: absolute;
        width: 50%;
        left:47%;
        height: 70%;
    }
    #divFondoC
    {
        position: absolute;
        width: 85%;
        top:43%;
        height: 50%;
        left:5%;
    }
    #divBotonA
    {
        position: absolute;
        color: White;
        background-color: transparent;
        top: 18%;
        left: 17%;
        width: 76%;
        height: 60%;
        text-align: center;
        font-size: 16px;
        cursor:pointer;
    }
    #divBotonB
    {
        position: absolute;
        color: White;
        background-color: transparent;
        top: 17%;
        left: 7%;
        width: 76%;
        height: 61%;
        text-align: center;
        font-size: 16px;
        cursor:pointer;
    }
    #divBotonC
    {
        position: absolute;
        color: White;
        background-color: transparent;
        top: 36%;
        left: 16%;
        width: 65%;
        height: 54%;
        text-align: center;
        font-size: 16px;
        cursor:pointer;
    }
    #divLogo
    {
        position: absolute;
        background-color: yellow;
        border-radius: 80%;
        overflow:hidden;
        height:33%;
        width:34%;
        top:29%;
        left:31%;
        box-shadow: 2px 9px 40px #000000;
        left: 31%;
    }
    
    .areaTorre
    {
        position: absolute;
        width: 90%;
        height: 70%;
        background-color: transparent;
        /*background-color: yellow;*/
        left: 5%;
        top: 27%;
    }
    .panelTorre
    {
        position: absolute;
        width: 90%;
        height: 24%;
        border-radius: 11px;
        border-style: solid;
        border-color: white;
        border-width: 5px;
        left:5%;
    }
    #panelTorreA
    {
        /*background-color: blue;*/
        top: 2%;
    }
    #panelTorreA_1
    {
        position:absolute; 
        left:5%;
        top:20%; 
        font-size:20px;
        font-weight:bold;
        width:15%;
        /*background-color:green; */
        color: White;
        text-align: center;
    }
    .cuadrito
    {
        position: absolute;
        width: 11%;
        height: 80%;
        top: 10%;
        border-radius: 7px;
        text-align: center;
        font-size: 10px;
    }
    #panelTorreB
    {
        /*background-color: Aqua;*/
        background-color:transparent;
        top: 31%;
    }
    #panelTorreB_1
    {
        position:absolute; 
        left:5%;
        top:20%; 
        font-size:20px;
        font-weight:bold;
        width:15%;
        /*background-color:green; */
        background-color:transparent;
        color: White; 
        text-align: center;
    }
    #panelTorreC
    {
        background-color:transparent;
        /*background-color: Lime;*/
        top: 60%;
    }
    #panelTorreC_1
    {
        position:absolute; 
        left:5%;
        top:20%; 
        font-size:20px;
        font-weight:bold;
        width:15%;
        background-color:transparent;
        /*background-color:green; */
        color: White;
        text-align: center;
    }
    .info
    {
        position:absolute;
        /*background-color: Red;*/
        width: 100%;
        font-size: 59px;
        text-align: center;
        height: 72%;
        font-weight: bold;
    }
    .infoCuadrito
    {
        position:absolute;
        /*background-color: Red;*/
        width: 100%;
        font-size: 15px;
        text-align: center;
        height: 72%;
        font-weight: bold;
    }
    .minuCuadro
    {
        width: 20%;
        border-radius: 5px;
        display: inline-block;
        text-align:center;
        margin: 2px;
    }
    .cuadritoMedio
    {
        position: absolute;
        width: 8%;
        height: 40%;
        top: 10%;
        border-radius: 7px;
        text-align: center;
        font-size: 10px;
    }
    .menuDash
    {
        position:absolute;    
        width:80%;
        left:10%;
        background-color: transparent;
        /*background-color: chocolate;*/
        height: 22%;
    }
    #divMenu
    {
        position:absolute;
        left:30%;
        width:30%;
        height:60%;
        color:White;
        text-align:center;
    }
    .divReturn
    {
        position:absolute;
        left: 80%;
        width: 15%;
        heig                                                                                                                                                                                                                            ht:30%;
        color:White;
        text-align:center;
    }
    .divFechaTxt
    {
        position:absolute;
        color: Yellow;
        font-size: 24px;
        font-weight: bold;
    }
</style>
<script type="text/javascript">
    function cambio_torre() {
        $('#divFondoB').slideUp('slow', function () { document.getElementById('<%= btnPostback.ClientID %>').click(); });
    }

</script>
<div id="divArea" runat="server" class="Area">
    <div id="divFondoA">
        <img src="../img/fondoA.png" class="imgBoard"/>
        <div id="divBotonA"><br /><b>KPI</b></div>
    </div>
    <div id="divFondoB">
        <img src="../img/fondoB.png" class="imgBoard"/>
        <asp:Button runat="server" id="btnPostback" style="display:none" onclick="Abrir_Torre" />
        <div id="divBotonB" onclick="cambio_torre();"><br /><b>TORRE<br />DE<br />CONTROL</b></div>
    </div>
    <div id="divFondoC">
        <img src="../img/fondoC.png" class="imgBoard"/>
        <div id="divBotonC"><br /><br /><b>INVENTARIOS</b></div>
    </div>
    <div id="divLogo">
        <img src="../img/logoDashBoard.png" class="imgBoard"/>
    </div>
</div>

<div id="divMenuDash" runat="server" class="menuDash">
    <div id="divFechaTxt" runat="server" class="divFechaTxt" >
        AGO 11 - 2014
    </div>
    <div id="divMenu" >
        <img src="../img/menuDash.png" class="imgBoard"/>
        <div id="divMenuTxt" style="position:absolute;top: 19%;left: 28%;font-size: 24px;">TORRE DE <br />CONTROL</div>
    </div>
</div>
<asp:Panel runat="server" ID="divReturn" class="divReturn" >
    <asp:ImageButton id="buttonimg" runat="server" ImageUrl="../img/menuDashBoard.png" style="position:absolute;top:5%;left: 29%;width: 60%;" onclick="RegresarMenu"></asp:ImageButton>
</asp:Panel>

<div id="divAreaTorre" runat="server" class="areaTorre">
    <div id="panelTorreA" class="panelTorre">
        <div id="panelTorreA_1">
            TALLER MOVIMIENTO VEHICULAR
        </div>
        <div id="panelTorreA_2" class="cuadrito" style="background-color:rgb(169,207,70); left:40%;">
            CITAS<br />
            <div id="divCitasTxt" class="info" runat="server"></div>
        </div>
        <div id="panelTorreA_3" class="cuadrito" style="background-color:rgb(245,134,52); left:55%;">
           INGRESADOS<br />
           <div id="divIngresadosTxt" class="info" runat="server"></div>
        </div>
        <div id="panelTorreA_4" class="cuadrito" style="background-color:white; left:70%;">
            ACTUAL<br />
            <div id="divActualTxt" class="info" runat="server"></div>
        </div>
        <div id="panelTorreA_5" class="cuadrito" style="background-color:rgb(169,207,70); left:85%;">
            ATENTIDO MES<br />
            <div id="divAtentidoTxt" class="info" runat="server"></div>
        </div>
    </div>

    <div id="panelTorreB" class="panelTorre">
        <div id="panelTorreB_1">
            VITRINA MOVIMIENTO
        </div>
        <div id="panelTorreB_2" class="cuadrito" style="background-color:rgb(169,207,70); left:25%;">
            VENDIDOS DIA<br />
            <div id="divVendidosTxt" class="info">6</div>
        </div>
        <div id="panelTorreB_3" class="cuadrito" style="background-color:white; left:40%;">
            CLIENTES ATENDIDOS AL DIA<br />
            <div id="divClientesDiaTxt" class="info">9</div>
        </div>
        <div id="panelTorreB_4" class="cuadrito" style="background-color:rgb(245,134,52); left:55%;">
            UNDS VENDIDAS MES<br />
            <div id="divUnidadesMesTxt" class="info">50</div>
        </div>
        <div id="panelTorreB_5" class="cuadrito" style="background-color:transparent; left:68%; width:22%; text-align:left;">
            <div class="minuCuadro"  style="background-color:rgb(169,207,70);"><font size="1">PICANTO</font><br />
            <font size="4">7</font>
            </div>
            <div class="minuCuadro"  style="background-color:rgb(169,207,70);"><font size="1">RIO</font><br />
            <font size="4">3</font>
            </div>
            <div class="minuCuadro"  style="background-color:rgb(169,207,70);"><font size="1">SORENTO</font><br />
            <font size="4">2</font>
            </div>
            <div class="minuCuadro"  style="background-color:rgb(245,134,52);"><font size="1">TAXI 5-5</font><br />
            <font size="4">6</font>
            </div>
            <div class="minuCuadro"  style="background-color:rgb(245,134,52);"><font size="1">PREGIO</font><br />
            <font size="4">1</font>
            </div>
            <div class="minuCuadro"  style="background-color:rgb(245,134,52);"><font size="1">SOUL</font><br />
            <font size="4">1</font>
            </div>
        </div>
        <div id="panelTorreB_6" class="cuadrito" style="background-color:yellow; left:92%; width:7%;">
            <br /><br /><br />EXISTENCIAS
        </div>
    </div>

     <div id="panelTorreC" class="panelTorre">
        <div id="panelTorreC_1">
            TALLER FACTURACIÓN
        </div>

        <div id="panelTorreC_2" class="cuadrito" style="background-color:rgb(169,207,70); left:25%;">
            FACTURADO DIA<br />
            <div id="divFacturaDiaTxt" class="infoCuadrito"><br /><br />$12.390.000</div>
        </div>
        <div id="panelTorreC_3" class="cuadritoMedio" style="background-color:rgb(169,207,70); left:37%;" >
            MECANICA<br />
            <div id="divMecanicaTxt" class="infoCuadrito"><br />$1.390.000</div>
        </div>
        <div id="panelTorreC_4" class="cuadritoMedio" style="background-color:rgb(169,207,70); left:46%;" >
            REPUESTOS<br />
            <div id="divRepuestosTxt" class="infoCuadrito"><br />$7.000.000</div>
        </div>
        <div id="panelTorreC_5" class="cuadritoMedio" style="background-color:rgb(245,134,52); left:37%; top:52%;" >
            LATONERIA<br />
            <div id="divLatoneriaTxt" class="infoCuadrito"><br />$2.000.000</div>
        </div>
        <div id="panelTorreC_6" class="cuadritoMedio" style="background-color:rgb(245,134,52); left:46%; top:52%;" >
            PINTURA<br />
            <div id="divPinturaTxt" class="infoCuadrito"><br />$2.000.000</div>
        </div>
        <div id="panelTorreC_7" style="position:absolute; background-color:transparent; left:36%; top:0%;text-align: center;color:White;">
            DIA
        </div>

        <div id="panelTorreC_2B" class="cuadrito" style="background-color:rgb(169,207,70); left:56%;">
            FACTURADO MES<br />
            <div id="divFacturaMesTxt" class="infoCuadrito"><br /><br />$193.390.000</div>
        </div>
        <div id="panelTorreC_3B" class="cuadritoMedio" style="background-color:rgb(169,207,70); left:68%;" >
            MECANICA<br />
            <div id="divMecanicaMesTxt" class="infoCuadrito"><br />$15.390.000</div>
        </div>
        <div id="panelTorreC_4B" class="cuadritoMedio" style="background-color:rgb(169,207,70); left:77%;" >
            REPUESTOS<br />
            <div id="divRepuestosMesTxt" class="infoCuadrito"><br />$110.000.000</div>
        </div>
        <div id="panelTorreC_5B" class="cuadritoMedio" style="background-color:rgb(245,134,52); left:68%; top:52%;" >
            LATONERIA<br />
            <div id="divLatoneriaMesTxt" class="infoCuadrito"><br />$33.000.000</div>
        </div>
        <div id="panelTorreC_6B" class="cuadritoMedio" style="background-color:rgb(245,134,52); left:77%; top:52%;" >
            PINTURA<br />
            <div id="divPinturaMesTxt" class="infoCuadrito"><br />$35.000.000</div>
        </div>
        <div id="panelTorreC_7B" style="position:absolute; background-color:transparent; left:64%; top:0%;text-align: center;color:White;">
            ACUMULADO MES
        </div>

        <div id="panelTorreC_8" class="cuadritoMedio" style="background-color:rgb(169,207,70); left:89%;" >
            INGRESADO MES<br />
            <div id="div3" class="infoCuadrito"><br />75</div>
        </div>
        <div id="panelTorreC_9" class="cuadritoMedio" style="background-color:rgb(245,134,52); left:89%; top:52%;" >
            ATENTIDOS MES<br />
            <div id="div7" class="infoCuadrito"><br />62</div>
        </div>
    </div>
</div>
