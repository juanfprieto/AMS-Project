<%@ Control Language="c#" Debug="true" autoeventwireup="True" codebehind="AMS.DBManager.Inserts.ascx.cs" Inherits="AMS.DBManager.Inserts" %>
<script type ="text/javascript" src="../js/AMS.Web.Masks.js"></script>
<script type ="text/javascript" src="../js/AMS.Web.WizardDirection.js"></script>
<script src="../js/nicEdit-latest.js" type="text/javascript"></script>
<style>
    .nicEdit-main{
        background-color:white;
        max-height: 600px;
        overflow: scroll !important;
        padding: 30px;
    }
    td img:hover
    {
        transform:none;
    }
</style>
<script type ="text/javascript">
    <% if(Request.QueryString["reg"]=="1")Response.Write("alert('Registro actualizado.');"); %>
    <% if(Request.QueryString["reg"]=="0")Response.Write("alert('Registro creado Satisfactoriamente.');"); %>
    <% if(Request.QueryString["reg"]=="0")Response.Write("alert('"+Request.QueryString["val"]+"');"); %>
    
    $(function() {
        $("#tabs").tabs();
        $('#_ctl1_' + parametroJS)
        .css({
            'font-family': 'cursive',
            'font-style': 'italic',
            'max-width': '1000px',
            'max-height': '800px',
            'font-size': 'medium',
            'width': '700px',
            'height': '600px'
        });

        //new nicEditor().panelInstance('_ctl1_' + parametroJS);
    });

    bkLib.onDomLoaded(function () {
        new nicEditor({
            fullPanel: true, onSave: function (content, id, instance) {
                alert('save button clicked for element ' + id + ' = ' + content);
            }
        }).panelInstance('_ctl1_' + parametroJS);

    });
   
    function readURL(input, imagen) 
    {
        if (input.files && input.files[0]) 
        {
            var reader = new FileReader();

            var extension = input.files[0].name;
            extension = extension.substr( (extension.lastIndexOf('.') +1) );
            
            if(extension == "pdf")
            {
                reader.onload = function (e) {
                    $('#' + imagen)
                        .attr('src', '../img/pdf_icon.png')
                        .width(45)
                        .height(60);
                    
                    $('#' + imagen).unbind( "click" );
                                    
                    $('#' + imagen).click(function() {
                        window.open(e.target.result, "_blank", "toolbar=0");
                    });
                };
                $('#' + imagen).css( 'cursor', 'pointer' );

            }
            else
            {
                reader.onload = function (e) {
                $('#' + imagen)
                    .attr('src', e.target.result)
                    .width(170)
                    .height(180);
                };
                $('#' + imagen).unbind( "click" );
                $('#' + imagen).css( 'cursor', 'auto' );
            }

            reader.readAsDataURL(input.files[0]);
        }
    }

    function digitoVerificacion(nit,dv){
    
        var nitStr = nit.value;
        
        if(/^(\d{9})$/.test(nitStr)){

            var aux;
            var residuo = 0;
            var primos = new Array(15);
            primos[0] = 3;
            primos[1] = 7;
            primos[2] = 13;
            primos[3] = 17;
            primos[4] = 19;
            primos[5] = 23;
            primos[6] = 29;
            primos[7] = 37;
            primos[8] = 41;
            primos[9] = 43;
            primos[10] = 47;
            primos[11] = 53;
            primos[12] = 59;
            primos[13] = 67;
            primos[14] = 71;
            
            var i;
            for(i=0;i<nitStr.length;i++){
                
                aux = nitStr.charAt(nitStr.length-1-i);
                residuo += parseInt(aux) * primos[i]; 
            }
            residuo %= 11;
            
            if(residuo > 1)
                residuo = 11 - residuo;
                
            dv.value = residuo;
        }
        else if(/\D/.test(nitStr)){
            alert('El nit sólo debe contener números!');
        }
    }

    function ValidarIdentificacion(ddlIdentificacion, txtNom1, txtNom2, txtApe2, ddlSociedad, ddlRegimen)
    {
        if(ddlIdentificacion.value == "N")
        {
            //alert("Este es un nit");
            //apagar los campos de nombre1, nombre2 y apellido2 y todos = null.
            txtNom1.value = "";
            txtNom2.value = "";
            txtApe2.value = "";
            txtNom1.disabled = true;
            txtNom2.disabled = true;
            txtApe2.disabled = true;
            $("#" + ddlSociedad.id).val("S");
            $("#" + ddlRegimen.id).val("C");
        }
        else
        {
            //alert("No nit");
            txtNom1.disabled = false;
            txtNom2.disabled = false;
            txtApe2.disabled = false;
            $("#" + ddlSociedad.id).val("N");
            $("#" + ddlRegimen.id).val("N");
        }
    }

    function ValidarDigitoVerificacion(objDigito, objNit, objTipoDocumento)
    { 
        var vpri, x, y, z, i, nit1, dv1;
        nit1=objNit.value;
        if(objTipoDocumento.value == 'N')
        {
            if (isNaN(nit1))
            {
                objDigito.value = "";
                alert('El valor digitado no es un numero valido!');		
            } 
            else 
            {
                vpri = new Array(16); 
                x=0 ; y=0 ; z=nit1.length ;
                vpri[1]=3;
                vpri[2]=7;
                vpri[3]=13; 
                vpri[4]=17;
                vpri[5]=19;
                vpri[6]=23;
                vpri[7]=29;
                vpri[8]=37;
                vpri[9]=41;
                vpri[10]=43;
                vpri[11]=47;  
                vpri[12]=53;  
                vpri[13]=59; 
                vpri[14]=67; 
                vpri[15]=71;
                for(i=0 ; i<z ; i++)
                { 
 	                y=(nit1.substr(i,1));
 	                //document.write(y+"x"+ vpri[z-i] +":");
                    x+=(y*vpri[z-i]);
 	                //document.write(x+"<br>");		
                } 
                y=x%11
                //document.write(y+"<br>");
                if (y > 1)
                {
                    dv1=11-y;
                } 
                else 
                {
                    dv1=y;
                }

                if(dv1 != objDigito.value)
                {
                    alert('El dígito de verificación no coincide con el Número de Identificación. Por favor revisar.');
                    objDigito.value = "";
                }
            }
        }
    }
</script>

<p> 
	<asp:Label id="lbInfo" runat="server"></asp:Label>
</p> 
<p>
	<asp:LinkButton id="lnkVerTabla" onclick="VerTabla" runat="server" CssClass="btn btn-primary identar" CausesValidation="false">Ver Tabla</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:LinkButton id="lnkVerTablaPadre" onclick="VerTablaPadre" CssClass="btn btn-primary identar" runat="server" CausesValidation="false" Visible="false">Ver Tabla</asp:LinkButton>
</p>
<p>
	<asp:Label id="lbProcessTitle" runat="server" forecolor="Navy" font-bold="True" font-size="12px"
		font-names="Arial"></asp:Label>
</p>
<p>
    <asp:PlaceHolder id="phForm" runat="server"></asp:PlaceHolder>
    
</p>
<p>
	<asp:Button id="btInsert" onclick="InsertRecords" runat="server" cssClass="identarBoton" Text="Añadir nuevo registro" OnClientClick="espera();"></asp:Button>
	<asp:Button id="btUpdate" onclick="UpdateRecord" cssClass="identarBoton" runat="server" Text="Actualizar registro" OnClientClick="espera();">   </asp:Button>
    <asp:Button CausesValidation="false" id="btnRegresarModal" onclick="RegresarModal" runat="server" Text="REGRESAR" Visible="False"></asp:Button>
	<asp:Button CausesValidation="false" id="btVolver" onclick="CancelReturn" runat="server" Text="Volver al Padre" Visible="False"></asp:Button>
</p>
<p>
</p>
<asp:Panel ID="pnlDependencias" Runat="server" Visible="false">
<TABLE class=head #dcdcdc?>
  <TR>
    <TD colSpan=2>
      <P class="bg-success">Dependencias:</P></TD></TR>
  <TR>
    <TD colSpan=2>&nbsp;</TD></TR>
  <TR>
    <TD>
      <P>
<asp:datagrid id=dgrDependencias runat="server" AutoGenerateColumns="false">
						<AlternatingItemStyle BackColor="LightCyan"></AlternatingItemStyle>
						<ItemStyle Font-Size="Medium" HorizontalAlign="Center" BackColor="#B8F1FB"></ItemStyle>
						<HeaderStyle Font-Size="Medium" Font-Bold="True" ForeColor="White"  CssClass="gridHeader"></HeaderStyle>
						<Columns>
							<asp:HyperLinkColumn HeaderText="NOMBRE" DataNavigateUrlField="URL" DataNavigateUrlFormatString="../aspx/AMS.Web.Index.aspx?process=DBManager.Inserts&{0}" DataTextField="TABLA" ItemStyle-HorizontalAlign="Left" />
						</Columns>
					</asp:datagrid></P></TD>
    <TD>&nbsp;</TD></TR>
  <TR>
    <TD colSpan=2>&nbsp;</TD></TR></TABLE>
</asp:Panel>

 
 <script>

     function Cargar_Nombre(obj) 
     {
         Inserts.Cargar_Nombre(obj.value, Cargar_Nombre_CallBack);
     }

     function Cargar_Nombre_CallBack(response) {
         var respuesta = response.value;

         if (respuesta != "") 
         {
             alert("El nit ingresado ya existe, por favor revise");

         }

     }
</script>