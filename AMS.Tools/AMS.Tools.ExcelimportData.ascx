<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Tools.ExcelimportData.ascx.cs" Inherits="AMS.Tools.ExcelimportData" %>
<script>
    function mostrarInfo() {
        var estado = $('#infoBody').css('display');
        if(estado == 'none')
            $("#infoBody").show('blind');
        else
            $("#infoBody").hide('blind');
    }
</script>

<table style="width: 100%; margin: 0; display: inline-block;">
    <tr>
        <td>
            <asp:Label ID="label1" runat="server" Text="Archivo Excel:"></asp:Label>
            <asp:FileUpload ID="xlsUpload" runat="server" Font-Size="Small" />
            <asp:Button ID="btnUpload" runat="server" Text="Cargar"  OnClick="btnUpload_Click"/>
            <asp:Label ID="lblResultado" runat="server" Text="" style="color: Green;"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <div id="info" style="color: blue; text-decoration: underline;  cursor:pointer;" onClick="mostrarInfo()">Click Instrucciones!</div>
            <div id="infoBody" style="display:none;">
                <TABLE>
					<TR>
						<TD></TD>
					</TR>
					<TR>
						<TD>Para que el proceso funcione corectamente, asegurese de seguir los siguientes
							pasos :</TD>
					</TR>
					<TR>
						<TD></TD>
					</TR>
					<TR>
						<TD>1. El archivo XLSX, debe tener la misma cantidad de
							columnas que la tabla seleccionada, para información sobre el número de
							columnas, consulte con su administrador del sistema.</TD>
					</TR>
					<TR>
						<TD>2. La primera fila debe contener los nombres identificadores de cada
							columna Ej: CODIGO, CUENTA, CIUDAD... (entre la fila de titulo y las filas de datos no puede
							haber espacios o filas vacias)</TD>
					</TR>
					<TR>
						<TD>3. Seleccione la totalidad de los datos que desea insertar
							(incluyendo las columnas con los nombres).&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
					</TR>
					<tr>
						<TD>4. Dirijase a la barra de menú en Formulas -
							Administrador de Nombres - Nuevo...&nbsp;&nbsp;&nbsp;</TD>
					</TR>
					<TR>
						<TD>5. En el cuadro de dialogo que aparecerá, llene el campo de Nombre como TABLA y click en
							Aceptar, y a continuancion click en Cerrar.&nbsp;Para mas información consulte con su
							administrador del sistema.</TD>
					</TR>
					<TR>
						<TD>6. Guarde su archivo en la ubicación y con el nombre que
							desee.</TD>
					</TR>
				</TABLE>
            </div>
        </td>
    </tr>
</table>
    
