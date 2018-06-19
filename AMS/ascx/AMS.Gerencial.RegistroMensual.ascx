<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Gerencial.RegistroMensual.ascx.cs" Inherits="AMS.Gerencial.RegistroMensual" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        #form1
        {
            width: 893px;
            height: 618px;
            margin-left: 0px;
            margin-right: 133px;
        }
        #TextBox        
        {  
            background-color: #E8F3FF;  
            
        } 
        </style>
</head>
<body runat="server"> 
<div id="Div8" 
        style="width:880px; 
        height: 176px;">
        <div id="Div9" 
        style="width:494px; 
        height: 176px;
        float:left; 
        margin-bottom: 0px;                 
        margin-right : 0px;"
        >
        
            <asp:Panel ID="Panel2" runat="server" BackColor="#E8F3FF" BorderColor="#003399" 
    BorderStyle="Groove" Font-Bold="True" Font-Italic="False" Font-Names="Tahoma" 
        Font-Size="X-Small" Direction="LeftToRight" HorizontalAlign="Left" 
        Width="494px" Height="172px" GroupingText="Info General">
        &nbsp;&nbsp;&nbsp;&nbsp;
        <br />
        &nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label7" runat="server" Text="Consecutivo:"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="txtConsecutivo" runat="server" Height="20px" class="tpequeno" 
            Wrap="False" BackColor="#F8F8F8"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label8" runat="server" Text="Concesion"></asp:Label>
                :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="ddlConcesion" runat="server" Height="16px" class="tpequeno">
        </asp:DropDownList>
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <br />       &nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label9" runat="server" Text="Concesionario:"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:DropDownList ID="ddlConcesionario" 
                    runat="server" Height="16px" class="dgrande" AutoPostBack="True" 
                    onselectedindexchanged="ddlConcesionario_SelectedIndexChanged">
        </asp:DropDownList>
        <br />
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label10" runat="server" Text="Fecha de diligenciamiento:"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label42" runat="server" Text="Mes:"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label43" runat="server" Text="Año:"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label11" runat="server" Text="Dias Laborales:"></asp:Label>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtFechadiligenciamiento" runat="server" Height="20px" 
                    class="tmediano" Wrap="False"></asp:TextBox>
&nbsp;<asp:DropDownList ID="ddlmes0" runat="server" Height="16px" class="dpequeno">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;
                <asp:DropDownList ID="ddlaño" runat="server" Height="16px" class="dpequeno">
                </asp:DropDownList>
                &nbsp; &nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtDiaslaborales" runat="server" Height="20px" class="dpequeno" 
                    Wrap="False"></asp:TextBox>
                <br />
                <br />
                &nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label12" runat="server" Text="Diligenciado por:"></asp:Label>
        &nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="txtDiligenciadopor" runat="server" Height="20px" class="tgrande"
            Wrap="False"></asp:TextBox>
        <br />
        <br />
</asp:Panel>
        </div>
        <div id="Div10" 
        style="height:215px; width:25px; float:left;">
        </div>
        <div id="Div11" 
        style="width:339px; 
        height: 215px;
        float:left; 
        margin-bottom: 0px;                 
        margin-right : 0px;"
        >
        
            
            <asp:Label ID="Label46" runat="server" 
                    Text="Estado..." Font-Bold="True" ForeColor="Red"></asp:Label>
                <br />
        
            
            <asp:Panel ID="Panel7" runat="server" BackColor="#E8F3FF" BorderColor="#003399" 
                BorderStyle="Groove" Font-Bold="True" Font-Names="Tahoma" Font-Size="X-Small" 
                GroupingText="Acciones de Registro" Height="159px" Width="297px">
                <br />
                <asp:RadioButton ID="RadioButton1" runat="server" GroupName="acciones" 
                    oncheckedchanged="RadioButton1_CheckedChanged1" Text="Añadir" 
                    AutoPostBack="True" />
                &nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="RadioButton2" runat="server" GroupName="acciones" 
                    oncheckedchanged="RadioButton2_CheckedChanged" Text="Modificar" 
                    AutoPostBack="True" />
                &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Button1" runat="server" Height="19px" 
                    onclick="Button1_Click1" Text="Guardar" />
                <br />
                <br />
                &nbsp;<asp:Label ID="Label45" runat="server" 
                    Text="Seleccione el mes que desea modificar:" Visible="False"></asp:Label>
                <br />
                <br />
                &nbsp;<asp:DropDownList ID="ddlregistros" runat="server" Height="16px" 
                    Visible="False" Width="134px">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="Button2" runat="server" Height="20px" onclick="Button2_Click" 
                    Text="Ver Detalles" Visible="False" />
                <br />
                &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </asp:Panel>
            <br />
            <br />
        
            
        </div>
        

</div>
<div></div>

<div id="Div1" 
         
        
        style="width:830px; height: 226px;">

    <br />
    <div id="Div2" 
         style="height:226px; width:174px; 
         float:left;          margin-bottom: 0px;">
  
    <asp:Panel ID="Panel3" runat="server" BackColor="#E8F3FF" BorderColor="#003399" 
    BorderStyle="Groove" Font-Bold="True" Font-Italic="False" Font-Names="Tahoma" 
        Font-Size="X-Small" Direction="LeftToRight" HorizontalAlign="Left" 
        Width="193px" Height="226px" Font-Overline="False" 
            GroupingText="Entradas Taller">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
        &nbsp;<asp:Label ID="Label16" runat="server" style="text-align: center" 
            Text="Cliente:" ForeColor="#000C26"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        
        <asp:TextBox ID="txtentradascli" runat="server" Height="20px" class="tpequeno"
            Wrap="False"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;
                     
        <br />
        <br />
        
        &nbsp;<asp:Label ID="Label17" runat="server" style="text-align: center" 
            Text="Garantia:" ForeColor="#000C26"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtentradasga" runat="server" 
            Height="20px" class="tpequeno" 
            Wrap="False"></asp:TextBox>
        &nbsp;&nbsp;<br />
        &nbsp;&nbsp;
             <br />
             
        
        &nbsp;<asp:Label ID="Label18" runat="server" style="text-align: center" 
            Text="Interno:" ForeColor="#000C26"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtentradasint" runat="server" 
            Height="20px" class="tpequeno"
            Wrap="False"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
            <br />
        
                &nbsp;<asp:Label ID="Label19" runat="server" style="text-align: center" 
            Text="Servicio:" ForeColor="#000C26"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="txtentradasserv" runat="server" Height="20px" class="tpequeno" 
            Wrap="False"></asp:TextBox>
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;
        
        <br />
        
                &nbsp;<asp:Label ID="Label20" runat="server" style="text-align: center" 
                    Text="PDI:" ForeColor="#000C26"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="txtentradaspdi" runat="server" Height="20px" class="tpequeno" 
            Wrap="False"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;<br />
&nbsp;<br />
    </asp:Panel>
  </div>
    <div id="Div3" 
        style="height:226px; width:25px; float:left;">
    </div>  
    <div id="Div7" 
         style="height:226px; width:566px; 
         float:left;          margin-bottom: 0px;">
        <asp:Panel ID="Panel6" runat="server" 
        BackColor="#E8F3FF"                          BorderColor="#003399"     BorderStyle="Groove" 
                Font-Bold="True"          Font-Italic="False" 
                Font-Names="Tahoma"       Font-Size="X-Small" 
                Direction="LeftToRight"   HorizontalAlign="Left" 
                Width="579px"             Height="181px" 
                Font-Overline="False"     GroupingText="Facturacion">
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label31" runat="server" style="text-align: center" 
            Text="Cliente"></asp:Label>
        &nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label32" runat="server" style="text-align: center" 
            Text="Garantia"></asp:Label>
        &nbsp;&nbsp;&nbsp;<asp:Label ID="Label33" runat="server" style="text-align: center" 
            Text="Interno" Font-Names="Tahoma" Font-Size="X-Small"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label39" runat="server" ForeColor="Black" 
            style="text-align: center" Text="PDI"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label40" runat="server" ForeColor="Black" 
            style="text-align: center" Text="Mostrador"></asp:Label>
        &nbsp;&nbsp;<asp:Label ID="Label41" runat="server" ForeColor="Black" 
            style="text-align: center" Text="Otros"></asp:Label>
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
        &nbsp;<asp:Label ID="Label36" runat="server" ForeColor="#000C26" 
            style="text-align: center" Text="Mano de obra:"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="txtmanodeobracli" runat="server" Height="20px" class="tpequeno" 
                Wrap="False"></asp:TextBox>
        &nbsp;<asp:TextBox ID="txtmanodeobragara" runat="server" Height="20px" class="tpequeno" 
            Wrap="False"></asp:TextBox>
        &nbsp;<asp:TextBox ID="txtmanodeobraint" runat="server" Height="20px" class="tpequeno" 
            Wrap="False"></asp:TextBox>
        &nbsp;<asp:TextBox ID="txtmanodeobrapdi" runat="server" Height="20px" class="tpequeno"
            Wrap="False"></asp:TextBox>
        &nbsp;<asp:TextBox ID="txtmanodeobramos" runat="server" Height="20px" class="tpequeno" 
            Wrap="False" BackColor="#E8F3FF" Enabled="False"></asp:TextBox>
        &nbsp;<asp:TextBox ID="txtmanodeobraotr" runat="server" Height="20px" class="tpequeno" 
            Wrap="False" BackColor="#E8F3FF" Enabled="False"></asp:TextBox>
        <br />
        <br />
        &nbsp;<asp:Label ID="Label37" runat="server" ForeColor="#000C26" 
            style="text-align: center" Text="Repuestos:"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="txtrepuestoscli" runat="server" Height="20px" class="tpequeno" 
                Wrap="False"></asp:TextBox>
        &nbsp;<asp:TextBox ID="txtrepuestosgara" runat="server" Height="20px" class="tpequeno" 
            Wrap="False"></asp:TextBox>
            &nbsp;<asp:TextBox ID="txtrepuestosint" runat="server" Height="20px" class="tpequeno" 
                Wrap="False"></asp:TextBox>
   
        &nbsp;<asp:TextBox ID="txtrepuestospdi" runat="server" Height="20px" class="tpequeno" 
            Wrap="False" ReadOnly="True" BackColor="#E8F3FF" Enabled="False"></asp:TextBox>
        &nbsp;<asp:TextBox ID="txtrepuestosmos" runat="server" Height="20px" class="tpequeno"
            Wrap="False"></asp:TextBox>
        &nbsp;<asp:TextBox ID="txtrepuestosotr" runat="server" Height="20px" class="tpequeno" 
            Wrap="False" ReadOnly="True" BackColor="#E8F3FF" Enabled="False"></asp:TextBox>
        <br />
        <br />
        &nbsp;&nbsp;<asp:Label ID="Label44" runat="server" ForeColor="#000C26" 
                style="text-align: center" Text="Horas de retrabajo:"></asp:Label>
            &nbsp;&nbsp;<asp:TextBox ID="txthorasretracli" runat="server" Height="20px" class="tpequeno"
                Wrap="False"></asp:TextBox>
            &nbsp;<asp:TextBox ID="txthorasretragara" runat="server" Height="20px" class="tpequeno" 
            Wrap="False" ></asp:TextBox>
                 &nbsp;<asp:TextBox ID="txthorasretraint" 
                     runat="server" Height="20px" class="tpequeno" 
            Wrap="False" ></asp:TextBox>
                 &nbsp;<asp:TextBox ID="txthorasretrapdi" runat="server" Height="20px" 
            class="tpequeno" Wrap="False" ReadOnly="True" BackColor="#E8F3FF" Enabled="False"></asp:TextBox>
&nbsp;<asp:TextBox ID="txthorasretramost" runat="server" Height="20px" class="tpequeno" Wrap="False" 
                ReadOnly="True" BackColor="#E8F3FF" Enabled="False"></asp:TextBox>
        &nbsp;<asp:TextBox ID="txthorasretraotr" runat="server" Height="20px" class="tpequeno" 
            Wrap="False"></asp:TextBox>
        &nbsp;<br />
            <br />
            &nbsp;<asp:Label ID="Label38" runat="server" ForeColor="#000C26" 
                style="text-align: center" Text="Horas de facturadas:"></asp:Label>
            &nbsp;<asp:TextBox ID="txthorasfacturadascli" runat="server" Height="20px" class="tpequeno" 
                Wrap="False"></asp:TextBox>
            &nbsp;<asp:TextBox ID="txthorasfacturadasgara" runat="server" Height="20px" class="tpequeno" 
                Wrap="False"></asp:TextBox>
            &nbsp;<asp:TextBox ID="txthorasfacturadasint" runat="server" Height="20px" class="tpequeno" 
                Wrap="False"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Panel>
    
  </div>
    </div>
<div style="height: 19px"></div>
<div id="Div12" 
        
        style="height:254px; width:633px; float:left; margin-bottom: 0px;">
    <div id="Div4" 
        style="height:226px; width:194px; float:left;">
   
  
    <asp:Panel ID="Panel4" runat="server" 
               BackColor="#E8F3FF" BorderColor="#003399" 
               BorderStyle="Groove" Font-Bold="True" 
               Font-Italic="False" Font-Names="Tahoma" 
               Font-Size="X-Small" Direction="LeftToRight"
               HorizontalAlign="Left" Width="195px" 
               Height="196px" GroupingText="Personal Productivo">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;<asp:Label ID="Label21" 
            runat="server" style="text-align: center" Text="Tecnicos:"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label22" runat="server" style="text-align: center" 
            Text="Aprendices:"></asp:Label>
        &nbsp;<br />
        &nbsp;<asp:TextBox ID="txtproductivotec" runat="server" Height="20px" class="tpequeno" 
            Wrap="False"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="txtproductivoapren" runat="server" Height="20px" class="tpequeno"
            Wrap="False"></asp:TextBox>
        <br />
        <br />
        &nbsp;<asp:Label ID="Label23" runat="server" style="text-align: center" 
            Text="Lavadores:"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label24" runat="server" style="text-align: center" Text="Otros:"></asp:Label>
        <br />
&nbsp;<asp:TextBox ID="txtproductivolavadores" runat="server" Height="20px" class="tpequeno" 
            Wrap="False"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtproductivootros" 
            runat="server" Height="20px" class="tpequeno"
            Wrap="False"></asp:TextBox>
        <br />
        <br />
&nbsp;<asp:Label ID="Label25" runat="server" style="text-align: center" 
            Text="Latoneria y Pintura:"></asp:Label>
        <br />
&nbsp;<asp:TextBox ID="txtproductivolatoneria" runat="server" Height="20px" class="tpequeno" 
            Wrap="False"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</asp:Panel>
  
    </div>
    <div id="Div5" 
        style="height:226px; width:25px; float:left;">
    </div>  
    <div id="Div6" 
        style="height:226px; width:332px; float:left;">
  
    <asp:Panel ID="Panel5" runat="server" BackColor="#E8F3FF" BorderColor="#003399" 
    BorderStyle="Groove" Font-Bold="True" Font-Italic="False" Font-Names="Tahoma" 
        Font-Size="X-Small" Direction="LeftToRight" HorizontalAlign="Left" 
        Width="335px" Height="199px" GroupingText="Personal no Productivo">
        &nbsp;<br />
        &nbsp;<asp:Label ID="Label26" runat="server" style="text-align: center" 
            Text="Gerente de servicio:"></asp:Label>
        &nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label27" runat="server" style="text-align: center" 
            Text="Jefe de taller:"></asp:Label>
        &nbsp;&nbsp;<br />
        &nbsp;<asp:TextBox ID="txtnoprogerente" runat="server" Height="20px" class="tpequeno" 
            Wrap="False"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="txtnoprojefe" runat="server" Height="20px" Width="60px" 
            Wrap="False"></asp:TextBox>
        <br />
        <br />
        &nbsp;<asp:Label ID="Label28" runat="server" style="text-align: center" 
            Text="Asesor de servicio:"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label30" runat="server" style="text-align: center" 
            Text="Administracion:"></asp:Label>
        &nbsp;&nbsp;&nbsp;<br />
        &nbsp;<asp:TextBox ID="txtnoproasesor" runat="server" Height="20px" class="tpequeno" 
            Wrap="False"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox 
            ID="txtnoproadmin" runat="server" Height="20px" 
            class="tpequeno" Wrap="False"></asp:TextBox>
        <br />
        <br />
        &nbsp;<asp:Label ID="Label29" runat="server" style="text-align: center" 
            Text="Otros:"></asp:Label>
        <br />
&nbsp;<asp:TextBox ID="txtnoprootros" runat="server" Height="20px" class="tpequeno" 
            Wrap="False"></asp:TextBox>
        &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</asp:Panel>
  
    </div>
    
  </div>
</body>