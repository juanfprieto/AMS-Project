<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AMS.SAC_Asesoria.ConsultarSolicitudClientO.aspx.cs" Inherits="AMS.SAC_Asesoria.ConsultarSolicitudClientO" %>
<%@ Register TagPrefix="chart" Namespace="System.Web.UI.DataVisualization.Charting" Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AMS</title>
	<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
	<meta content="C#" name="CODE_LANGUAGE">
	<meta content="JavaScript" name="vs_defaultClientScript">
	<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	<LINK href="../img/AMS.ico" type="image/ico" rel="icon">
	<LINK href="../css/AMS.css" type="text/css" rel="stylesheet">

    <style>
		.box {
				position: fixed;
				width: 100%;
				height: 100px;
				left: 0px;
				top: 0px;
				background:white;
				z-index: 444;
			}
		.ob_gCS, .ob_gCS div, .ob_gCS_F, .ob_gCS_F div
			{
					background-color: #FFFFFF !important;
			}
    </style>
</head>
<body>
    <br><br><br><br><br>

    <div class="box"><asp:placeholder id="plcEncabezado" runat="server" /></div>

    <form id="form1" runat="server">
        <asp:placeholder id="plcReporte" runat="server" />
    </form>
</body>
</html>
