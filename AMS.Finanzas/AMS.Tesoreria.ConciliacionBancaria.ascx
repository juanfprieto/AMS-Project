<%@ Control Language="c#" codebehind="AMS.Finanzas.Tesoreria.ConciliacionBancaria.ascx.cs" autoeventwireup="True" Inherits="AMS.Finanzas.Tesoreria.ConciliacionBancaria" %>
<fieldset>
 <table id="Table2" class="filtersIn">
    <tbody>
        <tr> 
            <td>
                Escoja el mes 
            </td>
            <td>
                <asp:DropDownList id="ddlMes" class="dpequeno" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Escoja la cuenta corriente 
            </td>
            <td>
                <asp:DropDownList id="ddlCC" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button id="btnAceptar" style="Z-INDEX: 101; LEFT: 303px; TOP: 65px" onclick="btnAceptar_Click" runat="server" Text="Aceptar"></asp:Button>
            </td>
        </tr>
    </tbody>
</table>
<p>
</p>
<p>
    <asp:Panel id="pnlInfo" runat="server" Visible="False" Height="600px" Width="800px">
         <table id="Table1" class="filtersIn">
            <tbody>
                <tr>
                    <td>
                        Instrucciones 
                    </td>
                </tr>
            </tbody>
        </table>
         <table id="Table3" class="filtersIn">
            <tbody>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        Para que el proceso de la conciliación bancaria funcione perféctamente, siga las siguientes
                        &nbsp;instrucciones:</td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        1. Descargue de la página oficial de su banco, su reporte de movimiento mensual.</td>
                </tr>
                <tr>
                    <td>
                        2. Teniendolo descargado llevelo a Microsoft Excel con el siguiente formato: 
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;a) La primera columna debe tener el nombre de FECHA.</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;b) La segunda columna debe llamarse DOCUMENTO.</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;c) Dependiendo de como tenga su movimiento, haga lo siguiente:</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Si el valor de las transacciones se encuentra
                        en un solo campo (no dividido como Valor Débito y Crédito),&nbsp; entonces la tercera
                        columna se debera llamar VALOR.</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Si el valor de las transacciones se encuentra
                        discriminado como Valor Débito y Valor Crédito, entonces debe llamar a una columna
                        VALOR DEBITO y a otra VALOR CREDITO.</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;d) Inserte los valores respectivos debajo del nombre de&nbsp;cada
                        columna.</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;e) Habiendo insertado todos los valores, seleccione la totalidad
                        de los datos que insertó (incluyendo las columnas con los nombres).</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;f) Dirijase a la barra de menú&nbsp;en&nbsp;Insertar - Nombre
                        - Definir...</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;g) En el cuadro de dialogo que aparecerá, dele el nombre de
                        CONCILIACION y click en Aceptar</td>
                </tr>
                <tr>
                    <td>
                        3. Guarde su archivo de Excel con el nombre&nbsp;y la ubicación que desee.</td>
                </tr>
                <tr>
                    <td>
                        4. Teniendo su archivo guardado, haga click&nbsp;en el botón Examinar que aparece
                        justo debajo de estas instrucciones.</td>
                </tr>
                <tr>
                    <td>
                        5. Busque la ubicación donde guardo su archivo, escoja su archivo haciendo click sobre
                        el y de click de nuevo en Abrir</td>
                </tr>
                <tr>
                    <td>
                        6. Finalmente haga click en el botón Aceptar Documento, si todo esta bien, sera redirigido
                        a otra página distinta.</td>
                </tr>
            </tbody>
        </table>
        <p></p>
        <table>
            <tbody>
                <tr>
                    <td>
                        <p>
                            Seleccione su archivo de movimiento bancario : 
                            <input id="uploadFile" type="file" runat="server" />
                        </p>
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:Button id="btnAceptarDoc" onclick="btnAceptarDoc_Click" runat="server" Text="Aceptar Documento"></asp:Button>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
</p>
<p>
    <asp:Label id="lb" runat="server"></asp:Label>
</p>
</fieldset>