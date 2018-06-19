<%@ Control Language="c#" codebehind="AMS.Inventarios.ArchivoLista.ascx.cs" autoeventwireup="True" Inherits="AMS.Inventarios.ArchivoLista" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" src="../js/AMS.Tools.js" type="text/javascript"></script>


<asp:PlaceHolder ID="plhContenido" runat="server">
    
    <fieldset>
        <p>
            <b>Lista de Precios :&nbsp;&nbsp;<asp:label id="lbListaPrecio" runat="server"></asp:label> </b>
        </p>
        <asp:placeholder id="plUpload" runat="server">
		    <p >
                El Archivo de Excel que esta subiendo debe tener las siguientes 
			    caracteristicas :
		    </p>
		    <p >
                a) Los campos deben ir en el siguiente orden :
                <ol>
                    <li>Referencia</li>
                    <li>Linea o Bodega de Inventarios</li>
                    <li>Precio al Público</li>
                </ol>
		    </p>
            <p>
                 b) El Archivo Excel debe cumplir con las siguiente características :
                <ol>
                    <li>*Recuerde que los campos de la tabla Excel deben estar en formato de <B>Texto</B> </li>
                    <li>El rango de celdas (espacio de nombres) debe estar definido con el nombre de "LISTAPRECIOS"</li>
                    <li>El Excel debe estar en formato Excel 2010 (.xlsx)</li>
                    <li>Tenga en cuenta que la primera fila de su selección es Referencia, Linea y Precio al Público. Si no existe esa fila, deje la primera fila en blanco.</li>
                    <li style="font-size:15px; color:firebrick; text-decoration-color:orangered">El código del item debe existir en la base de Datos</li>
                </ol>  
            </p>
		    <p style="font-size: large;">Mayor información : <a href="http://www.ecas.co" target="blank" style="text-decoration: underline; font-size: large;">
				    Visite Nuestra Página</a>
		    </p>
		    <p>Archivo A Subir : <input id="fDocument" type="file" runat="server"/><br />
			    <asp:Button id="btnAceptar" onclick="AceptarArchivo" runat="server" Text="Aceptar"></asp:Button>
		    </p>
	    </asp:placeholder>
        <asp:placeholder id="plResultado" runat="server" visible="false">
            <table>
                <tr>
                    <td style="vertical-align: top;">
                        <fieldset style="width:99%">
                            <legend style="font-size: x-large;">Items Nuevos en Lista</legend>
                            <div style="OVERFLOW: auto; CLIP: rect(auto auto auto auto)">
                                <asp:datagrid id="dgNew" runat="server" cssclass="datagrid" AutoGenerateColumns="false" AllowPaging="true"
	                                BorderWidth="1px" GridLines="Vertical" BorderStyle="None"  CellPadding="3" OnPageIndexChanged="DgNew_Page"
	                                PageSize="1000" ShowHeader="true" ShowFooter="False">
	                                <FooterStyle cssclass="footer"></FooterStyle>
	                                <SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
	                                <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
	                                <ItemStyle cssclass="item"></ItemStyle>
	                                <HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
	                                <Columns>
		                                <asp:BoundColumn DataField="MITE_CODIGO" HeaderText="Codigo Item"></asp:BoundColumn>
		                                <asp:BoundColumn DataField="PPRE_CODIGO" HeaderText="Lista de Precio"></asp:BoundColumn>
		                                <asp:BoundColumn DataField="MPRE_PRECIO" HeaderText="Valor P&#250;blico" DataFormatString="{0:C}"></asp:BoundColumn>
	                                </Columns>
	                                <PagerStyle borderwidth="1px" borderstyle="Dotted" cssclass="pager"
		                            position="TopAndBottom" mode="NumericPages"></PagerStyle>
                                </asp:datagrid>
                            </div>
                        </fieldset>
                    </td>
                    <td style="vertical-align: top;">
                        <fieldset style="width:99%">
                            <legend style="font-size: x-large;">Items con Error</legend>
	                        <div style="OVERFLOW: auto; CLIP: rect(auto auto auto auto)">
                                <asp:datagrid id="dgError" runat="server" cssclass="datagrid" AutoGenerateColumns="false" AllowPaging="true"
		                            CellPadding="3" OnPageIndexChanged="DgError_Page" PageSize="1000" ShowHeader="true" ShowFooter="False">
			                        <FooterStyle cssclass="footer"></FooterStyle>
			                        <SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
			                        <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
			                        <ItemStyle cssclass="item"></ItemStyle>
			                        <HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
			                        <Columns>
				                        <asp:BoundColumn DataField="CODIGO" HeaderText="Codigo Item"></asp:BoundColumn>
				                        <asp:BoundColumn DataField="DESCRIPCION" HeaderText="Descripci&#243;n Item"></asp:BoundColumn>
				                        <asp:BoundColumn DataField="INCONVENIENTE" HeaderText="Inconveniente"></asp:BoundColumn>
				                        <asp:BoundColumn DataField="INDICADOR" HeaderText="Indicador Antiguedad"></asp:BoundColumn>
			                        </Columns>
			                        <PagerStyle borderwidth="1px" borderstyle="Dotted" cssclass="pager"
				                        position="TopAndBottom" mode="NumericPages"></PagerStyle>
		                        </asp:datagrid>
	                        </div>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;">
                        <fieldset style="width:99%">
                            <legend style="font-size: x-large;">Items Existentes en Lista</legend>
	                        <div style="OVERFLOW: auto; WIDTH: 480px; CLIP: rect(auto auto auto auto);">
                                <asp:datagrid id="dgUpdate" runat="server" AutoGenerateColumns="False" cssclass="datagrid"
			                        GridLines="Vertical" BorderStyle="None" CellPadding="3" AllowPaging="true"
			                        OnPageIndexChanged="DgUpdate_Page" PageSize="1000" ShowHeader="true" ShowFooter="False">
			                        <FooterStyle cssclass="footer"></FooterStyle>
			                        <SelectedItemStyle font-bold="True" cssclass="selected"></SelectedItemStyle>
			                        <AlternatingItemStyle cssclass="alternate"></AlternatingItemStyle>
			                        <ItemStyle cssclass="item"></ItemStyle>
			                        <HeaderStyle font-bold="True" cssclass="header"></HeaderStyle>
			                        <Columns>
				                        <asp:BoundColumn DataField="MITE_CODIGO" HeaderText="Codigo Item"></asp:BoundColumn>
				                        <asp:BoundColumn DataField="PPRE_CODIGO" HeaderText="Lista de Precio"></asp:BoundColumn>
				                        <asp:BoundColumn DataField="MPRE_PRECIO" HeaderText="Valor P&#250;blico" DataFormatString="{0:C}"></asp:BoundColumn>
			                        </Columns>
			                        <PagerStyle borderwidth="1px" borderstyle="Dotted" position="TopAndBottom"  mode="NumericPages" cssclass="pager"></PagerStyle>
		                        </asp:datagrid>
	                        </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
            
            <p>
                <asp:button id="btnRealizar" onclick="RealizarProceso" runat="server" Text="Actualizar Precios"></asp:button>
            </p>
        </asp:placeholder>
        <p></p>
        <p></p>
        <p>
            <asp:label id="lb" runat="server"></asp:label>
        </p>
        <p>
            <asp:linkbutton id="lnkExportarExcel" runat="server" onclick="lnkExportarExcel_Click2">Exportar Excel Incorrectos </asp:linkbutton>
        </p>
        <p>
            <asp:linkbutton id="lnkExportarExcel2" runat="server" onclick="lnkExportarExcel_Click">Exportar Excel Correctos </asp:linkbutton>
        </p>
    </fieldset>
</asp:PlaceHolder>
