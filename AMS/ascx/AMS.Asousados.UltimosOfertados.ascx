<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Asousados.UltimosOfertados.ascx.cs" Inherits="AMS.Asousados.UltimosOfertados" %>

<asp:Panel id="offerVehicle" runat="server" Visible="true">
    <center><h1><u>Vehiculos Ofertados en las ultimas 24 horas</u></h1></center><br><br>
    <div id="VehiculosOfertados" runat= "server"></div>
</asp:Panel>

<style type="text/css">
.tarjeta 
		{
		    background:rgba(255, 216, 0, 0.7);
		    border:solid 1px gray;
		    -moz-border-radius: 0.5em;
            -webkit-border-radius: 0.5em;
            border-radius: 0.5em;
		    display:inline-block;
		    margin:1em 1em 1em 1em;
		    text-align:center;
		    width: 300px;
		    box-shadow: 4px 7px 15px #888888;
		    padding: 15px;
		}
</style>
