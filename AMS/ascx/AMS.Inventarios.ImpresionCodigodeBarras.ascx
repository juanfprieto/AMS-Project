<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AMS.Inventarios.ImpresionCodigodeBarras.ascx.cs"
    Inherits="AMS.Inventarios.ImpresionCodigodeBarras" %>
    <fieldset>
    <legend><H4>Información Para la Imprecion de Codigo de Barras</H4> </legend>
    <br/>
    <table id="Table" class="filtersIn">
        <tr>
            <td>
             Escoja el Almacen :
            </td>
            <td>
               <asp:dropdownlist id="ddlAlmc" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAlmc_OnSelectedIndexChanged" ></asp:dropdownlist>
            </td>
            <td>
                    Escoja el Prefijo :
            </td>
            <td>
                <asp:dropdownlist id="ddlPrefijo" runat="server"></asp:dropdownlist>
            </td>
            <td>
                    Escoja el numero de entrada :
            </td>
            <td>
                <asp:dropdownlist id="ddlNumEntrada" runat="server"></asp:dropdownlist>
            </td>
            <td>
                <asp:button commandname="DelDatasRow" text="Imprimir" id="btnIpr" runat="server"
                    width="80px" />
            </td>
        </tr>
    </table>
    </fieldset>
    
						
						