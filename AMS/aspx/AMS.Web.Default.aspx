<%@ page language="c#" autoeventwireup="True" codebehind="AMS.Web.Default.aspx.cs" inherits="AMS.Web.Default" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <meta charset="iso-8859-1" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="content-type" content="tapplication/javascript; charset=UTF-8">
    <title>AMS - Inicio</title>
    <link href="../css/normalize.css" type="text/css" rel="stylesheet">
    <link href="../css/AMS.css" type="text/css" rel="stylesheet">
    <link rel="stylesheet" href="../css/bootstrap.min.css"/>
    <link rel="stylesheet" href="../css/bootstrap-theme.min.css"/>
    <script type="text/javascript" src="../js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../js/npm.js"></script>

    <%--<script language="javascript" src="../js/AMS.Popups.js" type="text/javascript"></script>--%>
</head>
    
<body><div class="row">
    <div>
        <asp:Image id="fondoLogin" runat="server" ImageAlign="Middle" ImageUrl="../img/fondo.jpg"></asp:image>
        <div class="col-md-12" id ="loginBox">
           <div class="center">
                <asp:Image id="Image3" runat="server" ImageAlign="Middle" ImageUrl="../img/AMS.LoginImage.png"></asp:Image>
           </div>
            <div class="row">
        <form runat="server" id="loginForm" class="col-md-4 center-block">
        <asp:label id="loginInfo" class="alert alert-danger" runat="server" font-names="Tahoma" font-size="10pt" font-bold="true"></asp:label><br />
        <p id="loginText" runat="server" class="alert alert-info">Digite su Usuario y Contrase&ntilde;a</p>
        <div class="form-group inner-addon left-addon"> 
            <i class="glyphicon glyphicon-user"></i>         
            <asp:textbox id="user" runat="server" placeholder="Usuario" class="form-control visible block" required="required"></asp:textbox><br />           
        </div>
            <div class="form-group inner-addon left-addon">  
            <i class="glyphicon glyphicon-lock"></i>
        <asp:textbox id="pass" runat="server" placeholder="Contrase&ntilde;a"  textmode="password" class="form-control visible block" required="required"></asp:textbox><br /> 
                </div>           
        <asp:button id="Button1" onclick="Validate" class="btn btn-primary" runat="server" text="Ingresar"></asp:button><br />
        <asp:label id="Label5" runat="server" forecolor="White" font-names="Tahoma" font-size="10pt" font-bold="true">Automobile Management System</asp:label>
        <br>
        <asp:label id="lbVersion" runat="server" forecolor="White" font-names="Tahoma" font-size="8pt" font-bold="true"></asp:label>
        <asp:label id="loginInfoM" runat="server" forecolor="White" visible="false"></asp:label>
        <br>
        <asp:label id="lbCopyright" runat="server" forecolor="White" font-names="Tahoma" font-size="7pt" font-bold="true">
            Desarrollado por Sistemas eCAS SAS. PBX (57 1) 5404493 - 3166933513  www.ecas.co<br>
            Colombia - 2016  <span class ="glyphicon glyphicon-copyright-mark"></span>  </asp:label>
            <asp:Image id="Image4" runat="server" ImageUrl="../img/iconologo/logoEcas.png"></asp:Image><br />
            <asp:label id="Label1"  runat="server" forecolor="#36A036" font-names="Tahoma" font-size="12px" font-bold="true">Trabajamos por la evoluci&oacute;n social y moral.</asp:label>
          </div>
    </div>    
     </form>
    </div>
    </div>
</body>
</html>
