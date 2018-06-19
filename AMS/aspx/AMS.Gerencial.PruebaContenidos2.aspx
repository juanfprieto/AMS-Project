<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AMS.Gerencial.PruebaContenidos2.aspx.cs" Inherits="AMS.Gerencial.AMS_Gerencial_PruebaContenidos2" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Grid" Assembly="obout_Grid_NET" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>AMS</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<LINK href="../img/AMS.ico" type="image/ico" rel="icon">
        <style>
            .box {
                    position: fixed;
                    left: 10px;
                    top: 0px;
                    background:white;
                }       
        </style>
</head>
<body >
<div class="box"><asp:placeholder id="gridHolder" runat="server"></asp:placeholder></div> 
<br><br><br><br><br>
<form id="form1" method="post" runat="server">
    <obout:Grid id="Grid2" AutoGenerateColumns="false" AllowGrouping="true" ShowGroupsInfo="true" Serialize="true" ShowLoadingMessage="true"
                PageSize="200" AllowColumnResizing="true" AllowColumnReordering="true" runat="server" EnableRecordHover="true" 
                AllowAddingRecords="false" Language="es"  ShowCollapsedGroups="true" GroupBy = "Marca,Familia" 
                FolderStyle="../grid/styles/style_4" FolderLocalization="../grid/localization" OnRowDataBound="OnGridRowDataBound" >   
        <Columns>
            <obout:Column   DataField="Marca"               HeaderText="Codigo"         runat="server"  Wrap="true" AllowGroupBy="true" Width="130" />
            <obout:Column   DataField="Familia"             HeaderText="Descripcion"    runat="server"  Wrap="true" AllowGroupBy="true" Width="160" />
            <obout:Column   DataField="vin"                 HeaderText="VIN"            runat="server"  Wrap="true" AllowGroupBy="true" Width="160" />
            <obout:Column   DataField="modelo"              HeaderText="Modelo"         runat="server"  Wrap="true" AllowGroupBy="true" Width="70" />
            <obout:Column   DataField="precio_con_iva"      HeaderText="Precio"         runat="server"  Wrap="true" AllowGroupBy="true" Width="140" DataFormatString="{0:C}"   />
            <obout:Column   DataField="d_o"                 HeaderText="D.O."           runat="server"  Wrap="true" AllowGroupBy="true" Width="90" />
            <obout:Column   DataField="color"               HeaderText="Color"          runat="server"  Wrap="true" AllowGroupBy="true" Width="110" />
            <obout:Column   DataField="ubicacion"           HeaderText="Ubicación"      runat="server"  Wrap="true" AllowGroupBy="true" Width="120" />
            <obout:Column   DataField="estado"              HeaderText="Estado"         runat="server"  Wrap="true" AllowGroupBy="true" Width="100" />
            <obout:Column   DataField="dias"                HeaderText="Dias"           runat="server"  Wrap="true" AllowGroupBy="true" Width="70" />
            <obout:Column   DataField="origen"              HeaderText="Origen"         runat="server"  Wrap="true" AllowGroupBy="true" Width="100" />
            <obout:Column   DataField="pago"                HeaderText="Pago"           runat="server"  Wrap="true" AllowGroupBy="true" Width="110" />
        </Columns>
        <GroupingSettings AllowChanges="false">
        </GroupingSettings>
        <TemplateSettings GroupHeaderTemplateId="GroupTemplate" />
        <Templates>
            <obout:GridTemplate runat="server" ID="GroupTemplate">
                <Template>
                    <u><%# Container.Column.HeaderText %></u> : <b><i><%# Container.Value %></i></b> (<%# Container.Group.PageRecordsCount %> <%# Container.Group.PageRecordsCount > 1 ? "registros" : "registro" %>)
                </Template>
            </obout:GridTemplate>
        </Templates>
    </obout:Grid> 
</form>
<br>
</body>
</html>
