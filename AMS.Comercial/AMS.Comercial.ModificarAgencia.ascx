<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AMS.Comercial.ModificarAgencia.ascx.cs" Inherits="AMS.Comercial.AMS_Comercial_ModificarAgencia" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="../js/AMS.Web.Masks.js" type="text/javascript"></script>
<script language="javascript" src="../js/AMS.Web.ModalDialog.js" type="text/javascript"></script>
<form>
<DIV align="center">
	<table style="WIDTH: 773px" align="center">
		<tr>
			<td style="WIDTH: 545px" colspan="2"><b> Información de la agencia:</b>
			</td>

		</tr>
		<tr>
			<td style="WIDTH: 107px"><asp:label id="Label4" Font-Bold="True" Font-Size="XX-Small" runat="server">Código :</asp:label></td>
			<td><asp:dropdownlist id="ddlAgencia" Font-Size="XX-Small" runat="server" Width="150px" OnChange="cargarAgenciaDB(this)"></asp:dropdownlist>&nbsp;
				<asp:Label id="Label3" Font-Bold="True" Font-Size="XX-Small" runat="server">crear:</asp:Label>
				<asp:textbox id="txtCodigo" Font-Size="XX-Small" runat="server" Width="50px" MaxLength="4"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 107px"><asp:label id="Label12" Font-Bold="True" Font-Size="XX-Small" runat="server">Nombre :</asp:label></td>
			<td style="WIDTH: 386px; HEIGHT: 18px"><asp:textbox id="txtNombre" Font-Size="XX-Small" Width="300px" Runat="server" MaxLength="100"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 107px">
				<asp:Label id="Label6" runat="server" Font-Size="XX-Small" Font-Bold="True">NIT :</asp:Label>
			</td>
			<td>
				<asp:TextBox ReadOnly="True" id="txtNIT" onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS Nombre from DBXSCHEMA.MNIT MNIT', new Array(),1)"
					runat="server" Width="80px" Font-Size="XX-Small"></asp:TextBox></td>
		</tr>
		<tr>
			<td style="WIDTH: 107px"><asp:label id="Label5" Font-Bold="True" Font-Size="XX-Small" runat="server">Ciudad:</asp:label></td>
			<td><asp:dropdownlist id="ddlCiudad" Font-Size="XX-Small" runat="server"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td style="WIDTH: 107px"><asp:label id="Label8" Font-Bold="True" Font-Size="XX-Small" runat="server">Dirección :</asp:label></td>
			<td><asp:textbox id="txtDireccion" Font-Size="XX-Small" Width="300px" Runat="server" MaxLength="40"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 107px"><asp:label id="Label9" Font-Bold="True" Font-Size="XX-Small" runat="server">Teléfono :</asp:label></td>
			<td><asp:textbox id="txtTelefono" Font-Size="XX-Small" Width="150px" Runat="server" MaxLength="40"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 107px">
				<asp:Label id="Label15" runat="server" Font-Size="XX-Small" Font-Bold="True">Encargado :</asp:Label>
			</td>
			<td>
				<asp:TextBox ReadOnly="True" id="txtEncargado" onclick="ModalDialog(this,'SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT \' \'CONCAT coalesce(MNIT.MNIT_NOMBRE2,\'\') CONCAT \' \' CONCAT MNIT.MNIT_APELLIDOS CONCAT \' \' CONCAT coalesce(MNIT.MNIT_APELLIDO2,\'\') AS Nombre from DBXSCHEMA.MNIT MNIT', new Array(),1)"
					runat="server" Width="80px" Font-Size="XX-Small"></asp:TextBox>
				<asp:textbox id="txtEncargadoa" Font-Size="XX-Small" runat="server" Width="300px" ReadOnly="True"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 107px"><asp:label id="Label10" Font-Bold="True" Font-Size="XX-Small" runat="server">Porcentaje comisión :</asp:label></td>
			<td><asp:textbox id="txtPorcComision" Font-Size="XX-Small" Width="70px" Runat="server" MaxLength="6"></asp:textbox>&nbsp;%</td>
		</tr>
		<tr>
			<td style="WIDTH: 107px"><asp:label id="Label11" Font-Bold="True" Font-Size="XX-Small" runat="server">IVA?</asp:label></td>
			<td><asp:DropDownList id="rdbIVA" runat="server" Font-Size="XX-Small">
					<asp:ListItem Value="S">Si</asp:ListItem>
					<asp:ListItem Value="N">No</asp:ListItem>
				</asp:DropDownList></td>
		</tr>
		<tr>
			<td style="WIDTH: 107px"><asp:label id="Label13" Font-Bold="True" Font-Size="XX-Small" runat="server">Tipo régimen IVA:</asp:label></td>
			<td><asp:dropdownlist id="ddlRegIVA" Font-Size="XX-Small" runat="server"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td style="WIDTH: 107px"><asp:label id="Label17" Font-Bold="True" Font-Size="XX-Small" runat="server">Rotación personal?</asp:label></td>
			<td><asp:DropDownList id="rdbRotacion" runat="server" Font-Size="XX-Small">
					<asp:ListItem Value="S">Si</asp:ListItem>
					<asp:ListItem Value="N">No</asp:ListItem>
				</asp:DropDownList></td>
		</tr>
		<tr>
			<td style="WIDTH: 107px"><asp:label id="Label18" Font-Bold="True" Font-Size="XX-Small" runat="server">Activo?</asp:label></td>
			<td><asp:DropDownList id="rdbActivo" runat="server" Font-Size="XX-Small">
					<asp:ListItem Value="S">Si</asp:ListItem>
					<asp:ListItem Value="N">No</asp:ListItem>
				</asp:DropDownList></td>
		</tr>
		<tr>
			<td style="WIDTH: 107px"><asp:label id="Label1" Font-Bold="True" Font-Size="XX-Small" runat="server">Tiene Sistema?</asp:label></td>
			<td><asp:DropDownList id="ddlSistema" runat="server" Font-Size="XX-Small">
					<asp:ListItem Value="S">Si</asp:ListItem>
					<asp:ListItem Value="N">No</asp:ListItem>
				</asp:DropDownList></td>
		</tr>
		<tr>
			<td style="WIDTH: 107px"><asp:label id="Label2" Font-Bold="True" Font-Size="XX-Small" runat="server">Usa prefijo en el número de tiquete?</asp:label></td>
			<td><asp:DropDownList id="ddlPrefijo" runat="server" Font-Size="XX-Small">
					<asp:ListItem Value="N">No</asp:ListItem>
					<asp:ListItem Value="S">Si</asp:ListItem>
				</asp:DropDownList></td>
		</tr>
		<tr>
			<td style="WIDTH: 107px"><asp:label id="Label7" Font-Bold="True" Font-Size="XX-Small" runat="server">Recibe chequeos?</asp:label></td>
			<td><asp:DropDownList id="ddlChequeo" runat="server" Font-Size="XX-Small">
					<asp:ListItem Value="N">No</asp:ListItem>
					<asp:ListItem Value="S">Si</asp:ListItem>
				</asp:DropDownList></td>
		</tr>
		<TR>
			<td style="WIDTH: 106px"><asp:label id="Label19" Font-Bold="True" 
                    Font-Size="XX-Small" runat="server">Selecione un color para su agencia:</asp:label></td>
			<td align="left">
		        <select Name="clrname" Size="1" onChange="inputname()">
      <option Value="000000">&nbsp;-----&nbsp;Black&nbsp;&nbsp;-----</option>
      <option Value="0000FF">&nbsp;------&nbsp;Blue&nbsp;------</option>

      <option Value="F0F8FF">AliceBlue</option>
      <option Value="7FFFD4">Aquamarine</option>
      <option Value="76EEC6">Aquamarine2</option>
      <option Value="66CDAA">Aquamarine3</option>
      <option Value="458B74">Aquamarine4</option>
      <option Value="F0FFFF">Azure</option>

      <option Value="F0FFFF">Azure1</option>
      <option Value="E0EEEE">Azure2</option>
      <option Value="C1CDCD">Azure3</option>
      <option Value="838B8B">Azure4</option>
      <option Value="0000EE">Blue2</option>
      <option Value="0000CD">Blue3</option>

      <option Value="00008B">Blue4</option>
      <option Value="8A2BE2">BlueViolet</option>
      <option Value="5F9EA0">CadetBlue</option>
      <option Value="98F5FF">CadetBlue1</option>
      <option Value="8EE5EE">CadetBlue2</option>
      <option Value="7AC5CD">CadetBlue3</option>

      <option Value="53868B">CadetBlue4</option>
      <option Value="6495ED">CornflowerBlue</option>
      <option Value="00FFFF">Cyan</option>
      <option Value="00EEEE">Cyan2</option>
      <option Value="00CDCD">Cyan3</option>
      <option Value="008B8B">Cyan4</option>

      <option Value="483D8B">DarkSlateBlue</option>
      <option Value="00CED1">DarkTurquoise</option>
      <option Value="00BFFF">DeepSkyBlue</option>
      <option Value="00B2EE">DeepSkyBlue2</option>
      <option Value="009ACD">DeepSkyBlue3</option>
      <option Value="00688B">DeepSkyBlue4</option>

      <option Value="1E90FF">DodgerBlue</option>
      <option Value="1C86EE">DodgerBlue2</option>
      <option Value="1874CD">DodgerBlue3</option>
      <option Value="104E8B">DodgerBlue4</option>
      <option Value="ADD8E6">LightBlue</option>
      <option Value="BFEFFF">LightBlue1</option>

      <option Value="B2DFEE">LightBlue2</option>
      <option Value="9AC0CD">LightBlue3</option>
      <option Value="68838B">LightBlue4</option>
      <option Value="E0FFFF">LightCyan</option>
      <option Value="D1EEEE">LightCyan2</option>
      <option Value="B4CDCD">LightCyan3</option>

      <option Value="7A8B8B">LightCyan4</option>
      <option Value="87CEFA">LightSkyBlue</option>
      <option Value="B0E2FF">LightSkyBlue1</option>
      <option Value="A4D3EE">LightSkyBlue2</option>
      <option Value="8DB6CD">LightSkyBlue3</option>
      <option Value="607B8B">LightSkyBlue4</option>

      <option Value="8470FF">LightSlateBlue</option>
      <option Value="B0C4DE">LightSteelBlue</option>
      <option Value="CAE1FF">LightSteelBlue1</option>
      <option Value="BCD2EE">LightSteelBlue2</option>
      <option Value="A2B5CD">LightSteelBlue3</option>
      <option Value="6E7B8B">LightSteelBlue4</option>

      <option Value="66CDAA">MediumAquamarine</option>
      <option Value="0000CD">MediumBlue</option>
      <option Value="7B68EE">MediumSlateBlue</option>
      <option Value="48D1CC">MediumTurquoise</option>
      <option Value="191970">MidnightBlue</option>
      <option Value="000080">NavyBlue</option>

      <option Value="AFEEEE">PaleTurquoise</option>
      <option Value="BBFFFF">PaleTurquoise1</option>
      <option Value="AEEEEE">PaleTurquoise2</option>
      <option Value="96CDCD">PaleTurquoise3</option>
      <option Value="668B8B">PaleTurquoise4</option>
      <option Value="B0E0E6">PowderBlue</option>

      <option Value="4169E1">RoyalBlue</option>
      <option Value="4876FF">RoyalBlue1</option>
      <option Value="436EEE">RoyalBlue2</option>
      <option Value="3A5FCD">RoyalBlue3</option>
      <option Value="27408B">RoyalBlue4</option>
      <option Value="87CEEB">SkyBlue</option>

      <option Value="87CEFF">SkyBlue1</option>
      <option Value="7EC0EE">SkyBlue2</option>
      <option Value="6CA6CD">SkyBlue3</option>
      <option Value="4A708B">SkyBlue4</option>
      <option Value="6A5ACD">SlateBlue</option>
      <option Value="836FFF">SlateBlue1</option>

      <option Value="7A67EE">SlateBlue2</option>
      <option Value="6959CD">SlateBlue3</option>
      <option Value="473C8B">SlateBlue4</option>
      <option Value="4682B4">SteelBlue</option>
      <option Value="63B8FF">SteelBlue1</option>
      <option Value="5CACEE">SteelBlue2</option>

      <option Value="4F94CD">SteelBlue3</option>
      <option Value="36648B">SteelBlue4</option>
      <option Value="40E0D0">Turquoise</option>
      <option Value="00F5FF">Turquoise1</option>
      <option Value="00E5EE">Turquoise2</option>
      <option Value="00C5CD">Turquoise3</option>

      <option Value="00868B">Turquoise4</option>
      <option Value="A52A2A">&nbsp;-----&nbsp;Brown&nbsp;&nbsp;-----</option>
      <option Value="F5F5DC">Beige</option>
      <option Value="FF4040">Brown1</option>
      <option Value="EE3B3B">Brown2</option>

      <option Value="CD3333">Brown3</option>
      <option Value="8B2323">Brown4</option>
      <option Value="DEB887">Burlywood</option>
      <option Value="FFD39B">Burlywood1</option>
      <option Value="EEC591">Burlywood2</option>
      <option Value="CDAA7D">Burlywood3</option>

      <option Value="8B7355">Burlywood4</option>
      <option Value="D2691E">Chocolate</option>
      <option Value="FF7F24">Chocolate1</option>
      <option Value="EE7621">Chocolate2</option>
      <option Value="CD661D">Chocolate3</option>
      <option Value="8B4513">Chocolate4</option>

      <option Value="CD853F">Peru</option>
      <option Value="BC8F8F">RosyBrown</option>
      <option Value="FFC1C1">RosyBrown1</option>
      <option Value="EEB4B4">RosyBrown2</option>
      <option Value="CD9B9B">RosyBrown3</option>
      <option Value="8B6969">RosyBrown4</option>

      <option Value="8B4513">SaddleBrown</option>
      <option Value="F4A460">SandyBrown</option>
      <option Value="D2B48C">Tan</option>
      <option Value="FFA54F">Tan1</option>
      <option Value="EE9A49">Tan2</option>
      <option Value="CD853F">Tan3</option>

      <option Value="8B5A2B">Tan4</option>
      <option Value="BEBEBE">&nbsp;------&nbsp;Gray&nbsp;------</option>
      <option Value="2F4F4F">DarkSlateGray</option>
      <option Value="97FFFF">DarkSlateGray1</option>
      <option Value="8DEEEE">DarkSlateGray2</option>

      <option Value="79CDCD">DarkSlateGray3</option>
      <option Value="528B8B">DarkSlateGray4</option>
      <option Value="696969">DimGray</option>
      <option Value="030303">Gray1</option>
      <option Value="050505">Gray2</option>
      <option Value="080808">Gray3</option>

      <option Value="0A0A0A">Gray4</option>
      <option Value="0D0D0D">Gray5</option>
      <option Value="0F0F0F">Gray6</option>
      <option Value="121212">Gray7</option>
      <option Value="141414">Gray8</option>
      <option Value="171717">Gray9</option>

      <option Value="1A1A1A">Gray10</option>
      <option Value="1C1C1C">Gray11</option>
      <option Value="1F1F1F">Gray12</option>
      <option Value="212121">Gray13</option>
      <option Value="242424">Gray14</option>
      <option Value="262626">Gray15</option>

      <option Value="292929">Gray16</option>
      <option Value="2B2B2B">Gray17</option>
      <option Value="2E2E2E">Gray18</option>
      <option Value="303030">Gray19</option>
      <option Value="333333">Gray20</option>
      <option Value="363636">Gray21</option>

      <option Value="383838">Gray22</option>
      <option Value="3B3B3B">Gray23</option>
      <option Value="3D3D3D">Gray24</option>
      <option Value="404040">Gray25</option>
      <option Value="424242">Gray26</option>
      <option Value="454545">Gray27</option>

      <option Value="474747">Gray28</option>
      <option Value="4A4A4A">Gray29</option>
      <option Value="4D4D4D">Gray30</option>
      <option Value="4F4F4F">Gray31</option>
      <option Value="525252">Gray32</option>
      <option Value="545454">Gray33</option>

      <option Value="575757">Gray34</option>
      <option Value="595959">Gray35</option>
      <option Value="5C5C5C">Gray36</option>
      <option Value="5E5E5E">Gray37</option>
      <option Value="616161">Gray38</option>
      <option Value="636363">Gray39</option>

      <option Value="666666">Gray40</option>
      <option Value="696969">Gray41</option>
      <option Value="6B6B6B">Gray42</option>
      <option Value="6E6E6E">Gray43</option>
      <option Value="707070">Gray44</option>
      <option Value="737373">Gray45</option>

      <option Value="757575">Gray46</option>
      <option Value="787878">Gray47</option>
      <option Value="7A7A7A">Gray48</option>
      <option Value="7D7D7D">Gray49</option>
      <option Value="7F7F7F">Gray50</option>
      <option Value="828282">Gray51</option>

      <option Value="858585">Gray52</option>
      <option Value="878787">Gray53</option>
      <option Value="8A8A8A">Gray54</option>
      <option Value="8C8C8C">Gray55</option>
      <option Value="8F8F8F">Gray56</option>
      <option Value="919191">Gray57</option>

      <option Value="949494">Gray58</option>
      <option Value="969696">Gray59</option>
      <option Value="999999">Gray60</option>
      <option Value="9C9C9C">Gray61</option>
      <option Value="9E9E9E">Gray62</option>
      <option Value="A1A1A1">Gray63</option>

      <option Value="A3A3A3">Gray64</option>
      <option Value="A6A6A6">Gray65</option>
      <option Value="A8A8A8">Gray66</option>
      <option Value="ABABAB">Gray67</option>
      <option Value="ADADAD">Gray68</option>
      <option Value="B0B0B0">Gray69</option>

      <option Value="B3B3B3">Gray70</option>
      <option Value="B5B5B5">Gray71</option>
      <option Value="B8B8B8">Gray72</option>
      <option Value="BABABA">Gray73</option>
      <option Value="BDBDBD">Gray74</option>
      <option Value="BFBFBF">Gray75</option>

      <option Value="C2C2C2">Gray76</option>
      <option Value="C4C4C4">Gray77</option>
      <option Value="C7C7C7">Gray78</option>
      <option Value="C9C9C9">Gray79</option>
      <option Value="CCCCCC">Gray80</option>
      <option Value="CFCFCF">Gray81</option>

      <option Value="D1D1D1">Gray82</option>
      <option Value="D4D4D4">Gray83</option>
      <option Value="D6D6D6">Gray84</option>
      <option Value="D9D9D9">Gray85</option>
      <option Value="DBDBDB">Gray86</option>
      <option Value="DEDEDE">Gray87</option>

      <option Value="E0E0E0">Gray88</option>
      <option Value="E3E3E3">Gray89</option>
      <option Value="E5E5E5">Gray90</option>
      <option Value="E8E8E8">Gray91</option>
      <option Value="EBEBEB">Gray92</option>
      <option Value="EDEDED">Gray93</option>

      <option Value="F0F0F0">Gray94</option>
      <option Value="F2F2F2">Gray95</option>
      <option Value="F5F5F5">Gray96</option>
      <option Value="F7F7F7">Gray97</option>
      <option Value="FAFAFA">Gray98</option>
      <option Value="FCFCFC">Gray99</option>

      <option Value="D3D3D3">LightGray</option>
      <option Value="778899">LightSlateGray</option>
      <option Value="708090">SlateGray</option>
      <option Value="C6E2FF">SlateGray1</option>
      <option Value="B9D3EE">SlateGray2</option>
      <option Value="9FB6CD">SlateGray3</option>

      <option Value="6C7B8B">SlateGray4</option>
      <option Value="00FF00">&nbsp;-----&nbsp;Green&nbsp;&nbsp;-----</option>
      <option Value="7FFF00">Chartreuse</option>
      <option Value="76EE00">Chartreuse2</option>
      <option Value="66CD00">Chartreuse3</option>

      <option Value="458B00">Chartreuse4</option>
      <option Value="006400">DarkGreen</option>
      <option Value="BDB76B">DarkKhaki</option>
      <option Value="556B2F">DarkOliveGreen</option>
      <option Value="CAFF70">DarkOliveGreen1</option>
      <option Value="BCEE68">DarkOliveGreen2</option>

      <option Value="A2CD5A">DarkOliveGreen3</option>
      <option Value="6E8B3D">DarkOliveGreen4</option>
      <option Value="8FBC8F">DarkSeaGreen</option>
      <option Value="C1FFC1">DarkSeaGreen1</option>
      <option Value="B4EEB4">DarkSeaGreen2</option>
      <option Value="9BCD9B">DarkSeaGreen3</option>

      <option Value="698B69">DarkSeaGreen4</option>
      <option Value="228B22">ForestGreen</option>
      <option Value="ADFF2F">GreenYellow</option>
      <option Value="F0E68C">Khaki</option>
      <option Value="FFF68F">Khaki1</option>
      <option Value="EEE685">Khaki2</option>

      <option Value="CDC673">Khaki3</option>
      <option Value="8B864E">Khaki4</option>
      <option Value="7CFC00">LawnGreen</option>
      <option Value="20B2AA">LightSeaGreen</option>
      <option Value="32CD32">LimeGreen</option>
      <option Value="3CB371">MediumSeaGreen</option>

      <option Value="00FA9A">MediumSpringGreen</option>
      <option Value="F5FFFA">MintCream</option>
      <option Value="6B8E23">OliveDrab</option>
      <option Value="C0FF3E">OliveDrab1</option>
      <option Value="B3EE3A">OliveDrab2</option>
      <option Value="9ACD32">OliveDrab3</option>

      <option Value="698B22">OliveDrab4</option>
      <option Value="98FB98">PaleGreen</option>
      <option Value="9AFF9A">PaleGreen1</option>
      <option Value="90EE90">PaleGreen2</option>
      <option Value="7CCD7C">PaleGreen3</option>
      <option Value="548B54">PaleGreen4</option>

      <option Value="2E8B57">SeaGreen</option>
      <option Value="54FF9F">SeaGreen1</option>
      <option Value="4EEE94">SeaGreen2</option>
      <option Value="43CD80">SeaGreen3</option>
      <option Value="2E8B57">SeaGreen4</option>
      <option Value="00FF7F">SpringGreen</option>

      <option Value="00EE76">SpringGreen2</option>
      <option Value="00CD66">SpringGreen3</option>
      <option Value="008B45">SpringGreen4</option>
      <option Value="9ACD32">YellowGreen</option>
      <option Value="FFA499">&nbsp;-----&nbsp;Orange&nbsp;-----</option>

      <option Value="FFE4C4">Bisque</option>
      <option Value="EED5B7">Bisque2</option>
      <option Value="CDB79E">Bisque3</option>
      <option Value="8B7D6B">Bisque4</option>
      <option Value="FF7F50">Coral</option>
      <option Value="FF7256">Coral1</option>

      <option Value="EE6A50">Coral2</option>
      <option Value="CD5B45">Coral3</option>
      <option Value="8B3E2F">Coral4</option>
      <option Value="FF8C00">DarkOrange</option>
      <option Value="FF7F00">DarkOrange1</option>
      <option Value="EE7600">DarkOrange2</option>

      <option Value="CD6600">DarkOrange3</option>
      <option Value="8B4499">DarkOrange4</option>
      <option Value="E9967A">DarkSalmon</option>
      <option Value="F0FFF0">Honeydew</option>
      <option Value="E0EEE0">Honeydew2</option>
      <option Value="C1CDC1">Honeydew3</option>

      <option Value="838B83">Honeydew4</option>
      <option Value="F08080">LightCoral</option>
      <option Value="FFA07A">LightSalmon</option>
      <option Value="EE9572">LightSalmon2</option>
      <option Value="CD8162">LightSalmon3</option>
      <option Value="8B5742">LightSalmon4</option>

      <option Value="EE9A00">Orange2</option>
      <option Value="CD8499">Orange3</option>
      <option Value="8B5A00">Orange4</option>
      <option Value="FFDAB9">PeachPuff</option>
      <option Value="EECBAD">PeachPuff2</option>
      <option Value="CDAF95">PeachPuff3</option>

      <option Value="8B7765">PeachPuff4</option>
      <option Value="FA8072">Salmon</option>
      <option Value="FF8C69">Salmon1</option>
      <option Value="EE8262">Salmon2</option>
      <option Value="CD7054">Salmon3</option>
      <option Value="8B4C39">Salmon4</option>

      <option Value="A0522D">Sienna</option>
      <option Value="FF8247">Sienna1</option>
      <option Value="EE7942">Sienna2</option>
      <option Value="CD6839">Sienna3</option>
      <option Value="8B4726">Sienna4</option>
      <option Value="FF0000">&nbsp;------&nbsp;Red&nbsp;&nbsp;------</option>

      <option Value="FF1493">DeepPink</option>
      <option Value="EE1289">DeepPink2</option>
      <option Value="CD1076">DeepPink3</option>
      <option Value="8B0A50">DeepPink4</option>
      <option Value="B22222">FireBrick</option>
      <option Value="FF3030">FireBrick1</option>

      <option Value="EE2C2C">Firebrick2</option>
      <option Value="CD2626">Firebrick3</option>
      <option Value="8B1A1A">Firebrick4</option>
      <option Value="FF69B4">HotPink</option>
      <option Value="EE6AA7">HotPink2</option>
      <option Value="CD6090">HotPink3</option>

      <option Value="8B3A62">HotPink4</option>
      <option Value="CD5C5C">IndianRed</option>
      <option Value="FF6A6A">IndianRed1</option>
      <option Value="EE6363">IndianRed2</option>
      <option Value="CD5555">IndianRed3</option>
      <option Value="8B3A3A">IndianRed4</option>

      <option Value="FFB6C1">LightPink</option>
      <option Value="FFAEB9">LightPink1</option>
      <option Value="EEA2AD">LightPink2</option>
      <option Value="CD8C95">LightPink3</option>
      <option Value="8B5F65">LightPink4</option>
      <option Value="C71585">MediumVioletRed</option>

      <option Value="FFE4E1">MistyRose</option>
      <option Value="EED5D2">MistyRose2</option>
      <option Value="CDB7B5">MistyRose3</option>
      <option Value="8B7D7B">MistyRose4</option>
      <option Value="FF4499">OrangeRed</option>
      <option Value="EE4000">OrangeRed2</option>

      <option Value="CD3700">OrangeRed3</option>
      <option Value="8B2499">OrangeRed4</option>
      <option Value="DB7093">PaleVioletRed</option>
      <option Value="FF82AB">PaleVioletRed1</option>
      <option Value="EE799F">PaleVioletRed2</option>
      <option Value="CD6889">PaleVioletRed3</option>

      <option Value="8B475D">PaleVioletRed4</option>
      <option Value="FF6347">Tomato</option>
      <option Value="EE5C42">Tomato2</option>
      <option Value="CD4F39">Tomato3</option>
      <option Value="8B3626">Tomato4</option>
      <option Value="D02090">VioletRed</option>

      <option Value="FF3E96">VioletRed1</option>
      <option Value="EE3A8C">VioletRed2</option>
      <option Value="CD3278">VioletRed3</option>
      <option Value="8B2252">VioletRed4</option>
      <option Value="9932CC">DarkOrchid</option>
      <option Value="BF3EFF">DarkOrchid1</option>

      <option Value="B23AEE">DarkOrchid2</option>
      <option Value="9A32CD">DarkOrchid3</option>
      <option Value="68228B">DarkOrchid4</option>
      <option Value="9400D3">DarkViolet</option>
      <option Value="E6E6FA">Lavender</option>
      <option Value="FFF0F5">LavenderBlush</option>

      <option Value="EEE0E5">LavenderBlush2</option>
      <option Value="CDC1C5">LavenderBlush3</option>
      <option Value="8B8386">LavenderBlush4</option>
      <option Value="FF00FF">Magenta</option>
      <option Value="EE00EE">Magenta2</option>
      <option Value="CD00CD">Magenta3</option>

      <option Value="8B008B">Magenta4</option>
      <option Value="B03060">Maroon</option>
      <option Value="FF34B3">Maroon1</option>
      <option Value="EE30A7">Maroon2</option>
      <option Value="CD2990">Maroon3</option>
      <option Value="8B1C62">Maroon4</option>

      <option Value="BA55D3">MediumOrchid</option>
      <option Value="E066FF">MediumOrchid1</option>
      <option Value="D15FEE">MediumOrchid2</option>
      <option Value="B452CD">MediumOrchid3</option>
      <option Value="7A378B">MediumOrchid4</option>
      <option Value="9370DB">MediumPurple</option>

      <option Value="AB82FF">MediumPurple1</option>
      <option Value="9F79EE">MediumPurple2</option>
      <option Value="8968CD">MediumPurple3</option>
      <option Value="5D478B">MediumPurple4</option>
      <option Value="DA70D6">Orchid</option>
      <option Value="FF83FA">Orchid1</option>

      <option Value="EE7AE9">Orchid2</option>
      <option Value="CD69C9">Orchid3</option>
      <option Value="8B4789">Orchid4</option>
      <option Value="DDA0DD">Plum</option>
      <option Value="FFBBFF">Plum1</option>
      <option Value="EEAEEE">Plum2</option>

      <option Value="CD96CD">Plum3</option>
      <option Value="8B668B">Plum4</option>
      <option Value="A020F0">Purple</option>
      <option Value="9B30FF">Purple1</option>
      <option Value="912CEE">Purple2</option>
      <option Value="7D26CD">Purple3</option>

      <option Value="551A8B">Purple4</option>
      <option Value="D8BFD8">Thistle</option>
      <option Value="FFE1FF">Thistle1</option>
      <option Value="EED2EE">Thistle2</option>
      <option Value="CDB5CD">Thistle3</option>
      <option Value="8B7B8B">Thistle4</option>

      <option Value="FFFFFF">&nbsp;-----&nbsp;White&nbsp;&nbsp;-----</option>
      <option Value="FAEBD7">AntiqueWhite</option>
      <option Value="FFEFDB">AntiqueWhite1</option>
      <option Value="EEDFCC">AntiqueWhite2</option>
      <option Value="CDC0B0">AntiqueWhite3</option>

      <option Value="8B8378">AntiqueWhite4</option>
      <option Value="FFFAF0">FloralWhite</option>
      <option Value="DCDCDC">Gainsboro</option>
      <option Value="F8F8FF">GhostWhite</option>
      <option Value="FFFFF0">Ivory</option>
      <option Value="EEEEE0">Ivory2</option>

      <option Value="CDCDC1">Ivory3</option>
      <option Value="8B8B83">Ivory4</option>
      <option Value="FAF0E6">Linen</option>
      <option Value="FFDEAD">NavajoWhite</option>
      <option Value="EECFA1">NavajoWhite2</option>
      <option Value="CDB38B">NavajoWhite3</option>

      <option Value="8B795E">NavajoWhite4</option>
      <option Value="FDF5E6">OldLace</option>
      <option Value="FFF5EE">Seashell</option>
      <option Value="EEE5DE">Seashell2</option>
      <option Value="CDC5BF">Seashell3</option>
      <option Value="8B8682">Seashell4</option>

      <option Value="FFFAFA">Snow</option>
      <option Value="EEE9E9">Snow2</option>
      <option Value="CDC9C9">Snow3</option>
      <option Value="8B8989">Snow4</option>
      <option Value="F5DEB3">Wheat</option>
      <option Value="FFE7BA">Wheat1</option>

      <option Value="EED8AE">Wheat2</option>
      <option Value="CDBA96">Wheat3</option>
      <option Value="8B7E66">Wheat4</option>
      <option Value="F5F5F5">WhiteSmoke</option>
      <option Value="FFFF00">&nbsp;-----&nbsp;Yellow&nbsp;-----</option>

      <option Value="FFEBCD">BlanchedAlmond</option>
      <option Value="FFF8DC">Cornsilk</option>
      <option Value="EEE8CD">Cornsilk2</option>
      <option Value="CDC8B1">Cornsilk3</option>
      <option Value="8B8878">Cornsilk4</option>
      <option Value="B8860B">DarkGoldenrod</option>

      <option Value="FFB90F">DarkGoldenrod1</option>
      <option Value="EEAD0E">DarkGoldenrod2</option>
      <option Value="CD950C">DarkGoldenrod3</option>
      <option Value="8B6508">DarkGoldenrod4</option>
      <option Value="FFD700">Gold</option>
      <option Value="EEC900">Gold2</option>

      <option Value="CDAD00">Gold3</option>
      <option Value="8B7499">Gold4</option>
      <option Value="DAA520">Goldenrod</option>
      <option Value="FFC125">Goldenrod1</option>
      <option Value="EEB422">Goldenrod2</option>
      <option Value="CD9B1D">Goldenrod3</option>

      <option Value="8B6914">Goldenrod4</option>
      <option Value="FFFACD">LemonChiffon</option>
      <option Value="EEE9BF">LemonChiffon2</option>
      <option Value="CDC9A5">LemonChiffon3</option>
      <option Value="8B8970">LemonChiffon4</option>
      <option Value="EEDD82">LightGoldenrod</option>

      <option Value="FFEC8B">LightGoldenrod1</option>
      <option Value="EEDC82">LightGoldenrod2</option>
      <option Value="CDBE70">LightGoldenrod3</option>
      <option Value="8B814C">LightGoldenrod4</option>
      <option Value="FAFAD2">LightGoldenrodYellow</option>
      <option Value="FFFFE0">LightYellow</option>

      <option Value="EEEED1">LightYellow2</option>
      <option Value="CDCDB4">LightYellow3</option>
      <option Value="8B8B7A">LightYellow4</option>
      <option Value="FFE4B5">Moccasin</option>
      <option Value="EEE8AA">PaleGoldenrod</option>
      <option Value="FFEFD5">PapayaWhip</option>

      <option Value="EEEE00">Yellow2</option>
      <option Value="CDCD00">Yellow3</option>
      <option Value="8B8B00">Yellow4</option>
      <option Value></option>
      </select></TR>
		<TR>
			<td style="WIDTH: 106px"></td>
			<td align="left"><asp:button id="btnGuardar" Font-Bold="True" Font-Size="XX-Small" Runat="server" Text="Actualizar"></asp:button></td>
		</TR>
		<TR>
			<td style="WIDTH: 545px" align="center" colSpan="2">&nbsp;</td>
		</TR>
	</table>
</DIV>
<asp:Label id="lblError" runat="server" Font-Size="XX-Small" Font-Bold="True"></asp:Label>

<script Language="JavaScript">

<!-- begin
  var red=0; green=0; blue=0; hexnum='000000';
  var hexchars='0123456789ABCDEF';

  function toHex (num)
  {
    return hexchars.charAt(num >> 4) + hexchars.charAt(num & 0xF);
  }

  function fromHex (str)
  {
    var first=str.charAt(0);
    var second=str.charAt(1);
    return (16 * hexchars.indexOf(first)) + hexchars.indexOf(second);
  }
  
  function rgb ()
  {
    red=fromHex(hexnum.substring(0,2));
        green=fromHex(hexnum.substring(2,4));
    blue=fromHex(hexnum.substring(4,6));
  }
  
  function indexing ()
  {
    for ( var i=0; i<498; i++ )
        {
          if ( hexnum == document.forms[0].clrname.options[i].value )
            break;
        }
        if ( i==498 ) i--;
        index=i;
  }
  
  function paintit ()
  {
    document.forms[0].clrname.options[index].selected=1;
    document.forms[0].red.value=red;
    document.forms[0].green.value=green;
    document.forms[0].blue.value=blue;
    document.forms[0].hex.value=hexnum;

colorwin=window.open('',hexnum,'toobar=no,location=no,directories=no,status=no,scrollbars=no,resizable=no,copyhistory=no,width=130,height=50');
    colorwin.document.write( '<html><title>' + hexnum + '</title><body bgcolor=#' + hexnum + '></body></html>' );
    colorwin.document.close();
  }

  function inputval ()
  {
    var temp='';
    if ( document.forms[0].takex[1].checked==true )
        {
      hexnum=document.forms[0].hex.value.toUpperCase();
      for ( var i=0; i<=(hexnum.length-1); i++ )
          {
        temp += ( hexchars.indexOf(hexnum.charAt(i)) == -1 ? '0' :
hexnum.charAt(i) );
      }
      if ( hexnum.length < 6 )
          {
        for ( var i=hexnum.length-1; i<=5; i++)
                {
          temp += '0';
        }
      }
      hexnum=temp;
      rgb();
          indexing();
          paintit();
    }
        if ( document.forms[0].takex[0].checked==true )
        {
       with (Math)
           {
          temp=''+round(abs(parseFloat(document.forms[0].red.value)));
          red=( temp=='NaN' ? 0 : temp );
          temp=''+round(abs(parseFloat(document.forms[0].green.value)));
          green=( temp=='NaN' ? 0 : temp );
          temp=''+round(abs(parseFloat(document.forms[0].blue.value)));
          blue=( temp=='NaN' ? 0 : temp );
        }
        if ( red>255 ) red=255;
        if ( green>255 ) green=255;
        if ( blue>255 ) blue=255;
        hexnum= toHex(red) + toHex(green) + toHex(blue);
                indexing();
                paintit();
        }
  }
  
  function inputname ()
  {

hexnum=document.forms[0].clrname.options[document.forms[0].clrname.selectedIndex].value;
    if ( hexnum=='' )
    {
      document.forms[0].red.value='';
          document.forms[0].green.value='';
          document.forms[0].blue.value='';
      document.forms[0].hex.value=hexnum;
          document.forms[0].takex[0].checked=0;
          document.forms[0].takex[1].checked=0;
        } else
          {
            indexing();
            rgb();
                document.forms[0].takex[0].checked=1;
            paintit();
          }
  }
//-- end -->

</script>




  <table visible=false Border="0">
    <tr>
	<P>
      <td Align="left"><b><font size="2">Rojo/Verde/Azul</font><input Type="radio" Name="takex" Checked onClick="javascrip:document.forms[0].red.focus()"><font size="2">:</font><input Type="text" Name="red" Size="3" Maxlength="3" Value="0" onFocus="javascript:document.forms[0].takex[0].checked=1"><font size="2">/
      </font>
      <input Type="text" Name="green" Size="3" Maxlength="3" Value="0" onFocus="javascript:document.forms[0].takex[0].checked=1"><font size="2">/
      </font>
      <input Type="text" Name="blue" Size="3" Maxlength="3" Value="0" onFocus="javascript:document.forms[0].takex[0].checked=1"></b></td>
    </tr>
    <tr>
      <td Align="left">Codigo RGB<b><input Type="radio" Name="takex" onClick="javascript:document.forms[0].hex.focus()"><font size="2">:</font></b><input 
              Type="text" Name="hex" Size="6" Maxlength="6" Value="000000" 
              onFocus="javascript:document.forms[0].takex[1].checked=1" style="width: 50px"></td>
    </tr>
    
    <tr>
      <td Align="left">
      <input Type="button" Value="Mostrar" onClick="inputval()" class="noEspera"></td>
    </tr>
  </table>
</form>
<p>&nbsp;</p>


<script language:javascript>
function cargarAgenciaDB(Obj){
	AMS_Comercial_ModificarAgencia.CargarAgencia(Obj.value,CargarPlaca_Callback);
}
function CargarPlaca_Callback(response){
	var txtNombre=document.getElementById("<%=txtNombre.ClientID%>");
	var txtNIT=document.getElementById("<%=txtNIT.ClientID%>");
	var ddlCiudad=document.getElementById("<%=ddlCiudad.ClientID%>");
	var txtDireccion=document.getElementById("<%=txtDireccion.ClientID%>");
	var txtTelefono=document.getElementById("<%=txtTelefono.ClientID%>");
	var txtEncargado=document.getElementById("<%=txtEncargado.ClientID%>");
	var txtEncargadoa=document.getElementById("<%=txtEncargadoa.ClientID%>");
	var txtPorcComision=document.getElementById("<%=txtPorcComision.ClientID%>");
	var rdbIVA=document.getElementById("<%=rdbIVA.ClientID%>");
	var ddlRegIVA=document.getElementById("<%=ddlRegIVA.ClientID%>");
	var ddlSistema=document.getElementById("<%=ddlSistema.ClientID%>");
	var rdbRotacion=document.getElementById("<%=rdbRotacion.ClientID%>");
	var rdbActivo=document.getElementById("<%=rdbActivo.ClientID%>");
	var ddlPrefijo=document.getElementById("<%=ddlPrefijo.ClientID%>");
	var ddlChequeo=document.getElementById("<%=ddlChequeo.ClientID%>");
	var info=true;
	respuesta=response.value;
	if(respuesta.Tables[0]==null)info=false;
	if(info && respuesta.Tables[0].Rows.length>0){
		txtNombre.value=respuesta.Tables[0].Rows[0].NOM;
		txtNIT.value=respuesta.Tables[0].Rows[0].NITEMP;
		ddlCiudad.value=respuesta.Tables[0].Rows[0].CIU;
		txtDireccion.value=respuesta.Tables[0].Rows[0].DIR;
		txtTelefono.value=respuesta.Tables[0].Rows[0].TEL;
		txtEncargado.value=respuesta.Tables[0].Rows[0].NIT;
		txtEncargadoa.value=respuesta.Tables[0].Rows[0].NOMNIT;
		txtPorcComision.value=respuesta.Tables[0].Rows[0].COMI;
		rdbIVA.value=respuesta.Tables[0].Rows[0].TIVA;
		ddlRegIVA.value=respuesta.Tables[0].Rows[0].RIVA;
		rdbRotacion.value=respuesta.Tables[0].Rows[0].PROT;
		rdbActivo.value=respuesta.Tables[0].Rows[0].EST;
		ddlSistema.value=respuesta.Tables[0].Rows[0].SIS;
		ddlPrefijo.value=respuesta.Tables[0].Rows[0].PREF;
		ddlChequeo.value=respuesta.Tables[0].Rows[0].CHEQUEO;
	}
	else info=false;
	if(!info){
		txtNombre.value="";
		txtNIT.value="";
		txtDireccion.value="";
		txtTelefono.value="";
		txtEncargado.value="";
		txtEncargadoa.value="";
		txtPorcComision.value="";
		rdbIVA.value="S";
		rdbRotacion.value="S";
		rdbActivo.value="S";
		ddlSistema.value="S";
		ddlPrefijo.value="N";
		ddlChequeo.value="N";
	}
}
</script>
