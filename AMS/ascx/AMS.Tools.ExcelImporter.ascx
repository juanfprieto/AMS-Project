<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AMS.Tools.ExcelImporter.ascx.cs" Inherits="AMS.Tools.ExcelImporter" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>

<script type="text/javascript">
	function Mostrar_Div()
	{
		if(divIns.style.display=='none')
			divIns.style.display='';
		else
			divIns.style.display='none'
	}
</script>

<fieldset>
    <table>
	    <tbody align="left">
		    <tr>
			    <td><a href="javascript:Mostrar_Div();">Haga Click para ver las Instrucciones</a>
			    </td>
		    </tr>
		    <tr>
			    <td>
				    <div id="divIns" style="DISPLAY: none">
					    <table>
						    <tr>
							    <td></td>
						    </tr>
						    <tr>
							    <td>Para que el proceso funcione corectamente, asegurese de seguir los siguientes
								    pasos :</td>
						    </tr>
						    <tr>
							    <td></td>
						    </tr>
						    <tr>
							    <td>1. El archivo XLSX, debe tener la misma cantidad de
								    columnas que la tabla seleccionada, para información sobre el número de
								    columnas, consulte con su administrador del sistema.</td>
						    </tr>
						    <tr>
							    <td>2. La primera fila debe contener un nombre identificador de
								    la columna Ej: CODIGO, entre la fila de titulo y las filas de datos no puede
								    haber espacios o filas vacias.</td>
						    </tr>
                            <tr>
							    <td>3. Las columnas de la tabla excel deben estar en el mismo orden que las columnas de la tabla en la Base De Datos</td>
						    </tr>
                            <tr>
							    <td>4. <b>IMPORTANTE:</b> los valores numéricos no deben contener puntos(.) ni comas(,) a excepción de los valores decimales, estos sólo pueden contener punto(.)</td>
						    </tr>
						    <tr>
							    <td>5. Seleccione la totalidad de los datos que desea insertar
								    (incluyendo las columnas con los nombres)..&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
						    </tr>
						    <tr>
							    <td>6. Dirijase a la barra de menú&nbsp;en&nbsp;Insertar - Nombre - Definir...&nbsp;&nbsp;&nbsp;</td>
						    </tr>
						    <tr>
							    <td>7. En el cuadro de dialogo que aparecerá, dele el nombre
								    de la tabla que desea subir. Ej: MITEMS, MCUENTA y de click en
								    Aceptar.&nbsp;Para información sobre los nombres de las tablas consulte con su
								    administrador del sistema.</td>
						    </tr>
						    <tr>
							    <td>8. Guarde su archivo en la ubicación y con el nombre que
								    desee.</td>
						    </tr>
                            <tr>
							    <td><b>IMPORTANTE:</b> Asegúrese de que tiene el mismo número de columnas que el de la tabla principal.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
						    </tr>
					    </table>
				    </div>
			    </td>
		    </tr>
	    </tbody>
    </table>
    <p>Seleccione su archivo Excel: <input id="filUpl" type="file" runat="server" /> </p>
    <p>Seleccione la tabla correspondiente:
	    <asp:dropdownlist id="ddltabla" runat="server"></asp:dropdownlist></p>
    <p>
        <asp:button id="btnSubidaRapida" runat="server" Text="Subida Rápida" onclick="SubirArchivo" UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Cargando...')"></asp:button>
        Oprimiendo este botón se sube el archivo Excel a la base de datos de forma inmediata. use este botón si está seguro de que su archivo Excel cumple con los requisitos anteriormente mencionado(Proceso rápido).
    </p>
        <p>
            <asp:button id="btnSubidaNormal" runat="server" Text="Revisar y Subir" onclick="SubirArchivo" UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Cargando...')"></asp:button>
            Oprimiendo este botón se realiza una revisión al archivo y se sube a la Base de datos, en caso de fallar, se detallan las filas en las que falla(Este proceso es mucho más lento)
        </p>
    <p><asp:label id="lb" runat="server"></asp:label></p>
    <p><asp:label id="lbError" runat="server" Text=" "></asp:label></p>
</fieldset>
<script type="text/javascript">
 function clickOnce(btn, msg)
        {
            // Comprobamos si se está haciendo una validación
            if (typeof(Page_ClientValidate) == 'function') 
            {
                // Si se está haciendo una validación, volver si ésta da resultado false
                if (Page_ClientValidate() == false) { return false; }
            }
            
            // Asegurarse de que el botón sea del tipo button, nunca del tipo submit
            if (btn.getAttribute('type') == 'button')
            {
                // El atributo msg es totalmente opcional. 
                // Será el texto que muestre el botón mientras esté deshabilitado
                if (!msg || (msg='undefined')) { msg = 'Se está subiendo el archivo a la BD...'; }
                
                btn.value = msg;

                // La magia verdadera :D
                btn.disabled = true;
            }
            
            return true;
        }	
</script>		
