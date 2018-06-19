<%@ Control Language="C#" %>

<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<p>
    A continuación se presenta los datos necesarios para ingrear un nuevo auto al sistema,
    si el auto ya existe solo debe de darse el nśmero de placa y el password del carro. 
</p>
<fieldset>
    <p>
    </p>
    <legend>Datos del Vehiculo</legend>Nśmero de Placa : 
    <asp:TextBox id="placa" runat="server" class="tpequeno"></asp:TextBox>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Password : 
    <asp:TextBox id="password" runat="server" class="tpequeno" TextMode="Password"></asp:TextBox>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
    <asp:Button id="cargar" runat="server" class="bpequeno" Text="Cargar "></asp:Button>
    <p>
        Catalogo :&nbsp; 
        <asp:DropDownList id="catalogo" runat="server" class="dmediano"></asp:DropDownList>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;Nśmero de Identicación (V.I.N)&nbsp; 
        <asp:TextBox id="vin" runat="server" class="tmediano"></asp:TextBox>
        &nbsp; 
    </p>
    <p>
        Motor : 
        <asp:TextBox id="motor" runat="server" class="tmediano"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Nit del propietario : 
        <asp:TextBox id="nitPropietario" runat="server" class="tmediano"></asp:TextBox>
    </p>
    <p>
        (*)Serie : 
        <asp:TextBox id="serie" runat="server" class="tmediano"></asp:TextBox>
        &nbsp;&nbsp;&nbsp; (*)Chasis : 
        <asp:TextBox id="chasis" runat="server" class="tmediano"></asp:TextBox>
    </p>
    <p>
        Color : 
        <asp:DropDownList id="color" runat="server" class="dmediano"></asp:DropDownList>
        &nbsp;&nbsp;&nbsp; Ańo Modelo: 
        <asp:TextBox id="anoModelo" runat="server" class="tmediano"></asp:TextBox>
    </p>
    <p>
        Tipo de Servicio : 
        <asp:DropDownList id="tipoServicio" runat="server" class="dmediano"></asp:DropDownList>
        &nbsp;&nbsp;&nbsp;&nbsp;(*)Fecha Vencimiento Seguro: 
        <asp:TextBox id="fechaSeguro" runat="server" class="tpequeno"></asp:TextBox>
        &nbsp; (ańo-mes-dia) 
    </p>
    <p>
        Concesionario Vendedor: &nbsp;<asp:TextBox id="consVendedor" runat="server" class="tpequeno"></asp:TextBox>
        &nbsp;&nbsp;&nbsp; Fecha de Venta : 
        <asp:TextBox id="fechaVenta" runat="server" class="tpequeno"></asp:TextBox>
        &nbsp; (ańo-mes-dia) 
    </p>
    <p>
        Kilometraje de Venta : 
        <asp:TextBox id="kilometrajeVenta" onkeyup="NumericMaskE(this,event)" runat="server" class="tpequeno"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        Nśmero de Radio : 
        <asp:TextBox id="numeroRadio" runat="server" class="tpequeno"></asp:TextBox>
    </p>
    <p>
        Ultimo Kilometraje : 
        <asp:TextBox id="ultimoKilometraje" onkeyup="NumericMaskE(this,event)" runat="server" class="tpequeno"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Kilometraje
        Promedio : 
        <asp:TextBox id="kilometrajePromedio" onkeyup="NumericMaskE(this,event)" runat="server" class="tpequeno"></asp:TextBox>
    </p>
    <p>
        Los campos * no son obligatorios&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
        <asp:Button id="grabarVehiculo" runat="server" class="bpequeno" Text="Grabar "></asp:Button>
    </p>
</fieldset> 