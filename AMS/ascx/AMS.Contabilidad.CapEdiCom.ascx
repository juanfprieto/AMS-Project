<%@ Register TagPrefix="ew" Namespace="eWorld.UI" Assembly="eWorld.UI" %>
<%@ Control Language="c#" codebehind="AMS.Contabilidad.CapEdiCom.ascx.cs" autoeventwireup="True" Inherits="AMS.Contabilidad.CapEdiCom" %>

<script type="text/javascript" >
    function abrirEmergente1() {
        var tipo = document.getElementById("<%=typeDoc.ClientID%>");
        ModalDialog(tipo, "SELECT pdoc_codigo AS CODIGO,pdoc_codigo concat ' - ' concat pdoc_nombre as NOMBRE FROM pdocumento where tvig_vigencia='V' and tdoc_tipodocu not in ('CT','CV','PC','PV','PI','OP','OT') order by pdoc_CODIGO", new Array(), 1);
    }
</script>

<p>
	<table class="filters">
		<tbody>
			<tr>
				
                <th class="filterHead">
                <img height="60" src="../img/AMS.Flyers.Filters.png" border="0">
			    </th>
                
               
				<td>
					<p>Año:
						<asp:dropdownlist id="year" class="dpequeno" runat="server" AutoPostBack="True"></asp:dropdownlist>&nbsp;&nbsp; 
						Mes:
						<asp:dropdownlist id="month" class="dpequeno" runat="server" AutoPostBack="True"></asp:dropdownlist>&nbsp; 
						Fecha:&nbsp;
						<ew:calendarpopup id="cpFechaComp" width="100" runat="server" ImageUrl="../img/AMS.Icon.Calendar.gif" ControlDisplay="TextBoxImage"
							culture="Spanish (Colombia)">
							<WeekdayStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="White"></WeekdayStyle>
							<MonthHeaderStyle Font-Size="X-Small" Font-Names="Arial" ForeColor="Black" BackColor="Silver"></MonthHeaderStyle>
							<OffMonthStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="#FF8080"></OffMonthStyle>
							<GoToTodayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
								BackColor="White"></GoToTodayStyle>
							<TodayDayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
								BackColor="LightGoldenrodYellow"></TodayDayStyle>
							<DayHeaderStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="LightBlue"></DayHeaderStyle>
							<WeekendStyle Font-Size="XX-Small" Font-Names="Arial" ForeColor="Black" BackColor="LightGray"></WeekendStyle>
							<SelectedDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
								BackColor="Khaki"></SelectedDateStyle>
							<ClearDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
								BackColor="White"></ClearDateStyle>
							<HolidayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
								BackColor="White"></HolidayStyle>
						</ew:calendarpopup></p>
					<p>Tipo:
						<asp:dropdownlist id="typeDoc" runat="server" class="dmediano"></asp:dropdownlist><asp:Image id="imglupa3" runat="server" ImageUrl="../img/AMS.Search.png" onClick="abrirEmergente1();"></asp:Image>
						<asp:button id="Button1" onclick="NewComp" runat="server" Text="Nuevo" Width="80px"></asp:button><br>
						&nbsp;<asp:panel id="Panel1" runat="server" Width="260px" BorderWidth="1px" BorderStyle="Solid" BackColor="#E0E0E0">Mes y año fiscal vigente: 
                        <asp:Label id="maVig" runat="server" backcolor="#E0E0E0"></asp:Label></asp:panel><br/>
						<asp:radiobuttonlist id="rblConsecutivo" runat="server" ForeColor="Gray" RepeatDirection="vertical">
							<asp:ListItem Value="true" Selected="True">Autom&#225;tico</asp:ListItem>
                            <asp:ListItem Value="false">Manual</asp:ListItem>
						</asp:radiobuttonlist></p>
					<p><asp:panel id="Panel2" runat="server" Width="344px" BorderWidth="1px" BorderStyle="Solid" BackColor="WhiteSmoke"
							BorderColor="Silver" Height="104px">
							<asp:CheckBox id="chkPlant" runat="server" Text="Usar Plantilla" BackColor="WhiteSmoke"></asp:CheckBox>
							<p>Tipo:
								<asp:DropDownList id="typePlant" AutoPostBack="true" runat="server" class="dmediano" OnSelectedIndexChanged="Cambio_ItemP"></asp:DropDownList>
                            </p>
							<p>Comprobante:
								<asp:DropDownList id="compPlant" class="dpequeno" runat="server"></asp:DropDownList>
                            </p>
						</asp:panel>
                    </p>
					<p></p>
					<p></p>
					<p></p>
				</td>
			</tr>
		</tbody></table>
</p>
<p><asp:label id="infoLabel" runat="server" width="680px"></asp:label></p>
<p></p>
