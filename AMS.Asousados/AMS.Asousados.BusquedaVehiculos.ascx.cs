using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using AMS.DB;
using AMS.Tools;
using System.Data;

namespace AMS.Asousados
{
	public partial class BusquedaVehiculos : System.Web.UI.UserControl
	{
        private const string ID_IMPAR = "I";
        private const string ID_PAR = "P";
        private const string ID_NUMERO = "N";
        private const string ID_MODESPECIFICO = "MOD";
        private string placa = ""; 

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
           //     txtPlaca.Visible = false;
                llenarDDLs();

                ddlReferenciaPrincipal.Enabled = false;
                ddlReferenciaSecundaria.Enabled = false;
            }
		}

        protected void btnBusquedaInteligente_Click(object sender, EventArgs e)
        {
            string sql = getSelectBusquedaInteligente();
            string opcionInteligente = txtBusquedaInteligente.Text;
            btnLimpiarParametros_Click(sender, e);

            llenarDataGrid(sql);
            txtBusquedaInteligente.Text = opcionInteligente;  // se recupera el texto de la opcion de consulta inteligente
        }


        protected void btnBusquedaParametros_Click(object sender, EventArgs e)
        {
            string sqlParametros = getSelectBusquedaParametros();
            string sqlInteligente = getSelectBusquedaInteligente();
            string sql;

            if (sqlInteligente != null) // retorna null cuando el campo está vacio
                sql = String.Format("{0} INTERSECT {1}", sqlInteligente, sqlParametros);
            else
                sql = sqlParametros;

            llenarDataGrid(sql);
        }

        protected void btnLimpiarParametros_Click(object sender, EventArgs e)
        {
            llenarDDLs();
            txtPlaca.Text = "";
            txtBusquedaInteligente.Text = "";
            ddlReferenciaPrincipal.Items.Insert(0, "Seleccione...");
            ddlReferenciaSecundaria.Items.Insert(0, "Seleccione...");
            ddlReferenciaPrincipal.SelectedIndex = 0;
            ddlReferenciaSecundaria.SelectedIndex = 0;
            lblError.Text = "";
            txtModelo.Text = "";
        }

        protected void ddls_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValueAux;
            string sqlAux;
            string where = CatalogoWhereBuilder();

            if (ddlMarca.SelectedValue != "0")
                ddlReferenciaPrincipal.Enabled = true;
            
            else
            {
                ddlReferenciaPrincipal.Enabled = false;
                ddlReferenciaSecundaria.Enabled = false;
            }

            if (ddlReferenciaPrincipal.SelectedValue != "0")
                ddlReferenciaSecundaria.Enabled = true;
            else
                ddlReferenciaSecundaria.Enabled = false;

            if (!sender.Equals(ddlClase))
            {
                selectedValueAux = ddlClase.SelectedValue;
                sqlAux = String.Format(
                 "SELECT DISTINCT pcl.PCLA_CODIGO, pcl.PCLA_NOMBRE " +
                 "FROM PCLASEVEHICULO pcl " +
                 "  RIGHT JOIN pcatalogovehiculo pc ON pc.PCLA_codigo = pcl.PCLA_codigo " +
                 "{0} ORDER BY pcl.PCLA_NOMBRE", where);
                Utils.FillDll(ddlClase, sqlAux, true);
                if (ddlClase.Items.FindByValue(selectedValueAux) != null)
                    ddlClase.SelectedValue = selectedValueAux;
            }

            if (!sender.Equals(ddlMarca))
            {
                selectedValueAux = ddlMarca.SelectedValue;
                sqlAux = String.Format(
                 "SELECT DISTINCT PMV.PMAR_CODIGO, PMV.PMAR_NOMBRE " +
                 "FROM PMARCA PMV " +
                 "  RIGHT JOIN pcatalogovehiculo pc ON PMV.PMAR_CODIGO = pc.PMAR_CODIGO " +
                 "{0} ORDER BY PMV.PMAR_NOMBRE", where);
                Utils.FillDll(ddlMarca, sqlAux, true);
                if (ddlMarca.Items.FindByValue(selectedValueAux) != null)
                    ddlMarca.SelectedValue = selectedValueAux;
            }

            if (!sender.Equals(ddlReferenciaPrincipal))
            {
                selectedValueAux = ddlReferenciaPrincipal.SelectedValue;
                sqlAux = String.Format(
                 "SELECT DISTINCT PGC.PGRU_GRUPO, PGC.PGRU_NOMBRE " +
                 "FROM PGRUPOCATALOGO PGC " +
                 "  RIGHT JOIN pcatalogovehiculo pc ON PGC.PGRU_GRUPO = pc.PGRU_GRUPO " +
                 "{0} ORDER BY PGC.PGRU_NOMBRE", where);
                Utils.FillDll(ddlReferenciaPrincipal, sqlAux, true);
                if (ddlReferenciaPrincipal.Items.FindByValue(selectedValueAux) != null)
                    ddlReferenciaPrincipal.SelectedValue = selectedValueAux;
            }

            if (!sender.Equals(ddlReferenciaSecundaria))
            {
                selectedValueAux = ddlReferenciaSecundaria.SelectedValue;
                sqlAux = String.Format(
                 "SELECT DISTINCT PSGC.PSGRU_GRUPO, PSGC.PSGRU_NOMBRE " +
                 "FROM PSUBGRUPOVEHICULO PSGC " +
                 "  RIGHT JOIN pcatalogovehiculo pc ON PSGC.PSGRU_GRUPO = pc.PSGRU_CODIGO " +
                 "{0} ORDER BY PSGC.PSGRU_NOMBRE", where);
                Utils.FillDll(ddlReferenciaSecundaria, sqlAux, true);
                if (ddlReferenciaSecundaria.Items.FindByValue(selectedValueAux) != null)
                    ddlReferenciaSecundaria.SelectedValue = selectedValueAux;
            }

            if (!sender.Equals(ddlCarroceria))
            {
                selectedValueAux = ddlCarroceria.SelectedValue;
                sqlAux = String.Format(
                 "SELECT DISTINCT PCA.PCAR_CODIGO, PCA.PCAR_NOMBRE " +
                 "FROM PCARROCERIA PCA " +
                 "  RIGHT JOIN pcatalogovehiculo pc ON pc.PCAR_CODIGO = PCA.PCAR_CODIGO " +
                 "{0} ORDER BY PCA.PCAR_NOMBRE", where);
                Utils.FillDll(ddlCarroceria, sqlAux, true);
                if (ddlCarroceria.Items.FindByValue(selectedValueAux) != null)
                    ddlCarroceria.SelectedValue = selectedValueAux;
            }

            if (!sender.Equals(ddlCaja))
            {
                selectedValueAux = ddlCaja.SelectedValue;
                sqlAux = String.Format(
                 "SELECT DISTINCT RCC.RCAJ_CODIGO, RCC.RCAJ_NOMBRE " +
                 "FROM RcajaCAMBIOS RCC " +
                 "  RIGHT JOIN pcatalogovehiculo pc ON RCC.RCAJ_CODIGO = pc.RCAJ_CODIGO " +
                 "{0} ORDER BY RCC.RCAJ_NOMBRE", where);
                Utils.FillDll(ddlCaja, sqlAux, true);
                if (ddlCaja.Items.FindByValue(selectedValueAux) != null)
                    ddlCaja.SelectedValue = selectedValueAux;
            }

            if (!sender.Equals(ddlTraccion))
            {
                selectedValueAux = ddlTraccion.SelectedValue;
                sqlAux = String.Format(
                 "SELECT DISTINCT Rt.Rtra_CODIGO, Rt.Rtra_DESCRIPC " +
                 "FROM RtraccionVEHICULO Rt " +
                 "  RIGHT JOIN pcatalogovehiculo pc ON Rt.Rtra_CODIGO = pc.Rtra_CODIGO " +
                 "{0} ORDER BY Rt.Rtra_DESCRIPC", where);
                Utils.FillDll(ddlTraccion, sqlAux, true);
                if (ddlTraccion.Items.FindByValue(selectedValueAux) != null)
                    ddlTraccion.SelectedValue = selectedValueAux;
            }

        }

        private string CatalogoWhereBuilder()
        {
            string where = "";

            string clase = ddlClase.SelectedValue;
            string marca = ddlMarca.SelectedValue;
            string refPrincipal = ddlReferenciaPrincipal.SelectedValue;
            string refSecundaria = ddlReferenciaSecundaria.SelectedValue;
            string carroceria = ddlCarroceria.SelectedValue;
            string tipoCombustible = ddlTipoCombustible.SelectedValue;
            string caja = ddlCaja.SelectedValue;
            string traccion = ddlTraccion.SelectedValue;

            if (clase != "0" && clase != "")
                where += string.Format("AND PC.PCLA_CODIGO = '{0}' ", clase);
            if (marca != "0" && marca != "")
                where += string.Format("AND PC.PMAR_CODIGO = '{0}' ", marca);
            if (refPrincipal != "0" && refPrincipal != "" && refPrincipal != "Seleccione...")
                where += string.Format("AND PC.PGRU_GRUPO = '{0}' ", refPrincipal);
            if (refSecundaria != "0" && refSecundaria != "" && refSecundaria != "Seleccione...")
                where += string.Format("AND PC.PSGRU_CODIGO = '{0}' ", refSecundaria);
            if (carroceria != "0" && carroceria != "")
                where += string.Format("AND PC.PCAR_CODIGO = '{0}' ", carroceria);
            if (tipoCombustible != "0" && tipoCombustible != "")
                where += string.Format("AND PC.PCOM_CODIGO = '{0}' ", tipoCombustible);
            if (caja != "0" && caja != "")
                where += string.Format("AND PC.RCAJ_CODIGO = '{0}' ", caja);
            if (traccion != "0" && traccion != "")
                where += string.Format("AND PC.RTRA_CODIGO = '{0}' ", traccion);


            return where.Length > 3 ? " where " + where.Substring(3) : "";
        }

        private void llenarDDLs()
        {
            Utils.FillDll(ddlAsociado, "select mc.mnit_nit, mn.mnit_nombres || ' ' || mn.mnit_apellidos from MCONCESIONARIO mc inner join mnit mn on mc.mnit_nit = mn.mnit_nit order by mcon_gerente", true);
            Utils.FillDll(ddlClase, "SELECT PCLA_CODIGO, PCLA_NOMBRE FROM PCLASEVEHICULO ORDER BY PCLA_NOMBRE", true);
            Utils.FillDll(ddlMarca, "SELECT PMAR_CODIGO, PMAR_NOMBRE FROM PMARCA ORDER BY PMAR_NOMBRE", true);
            Utils.FillDll(ddlCarroceria, "SELECT PCAR_CODIGO, PCAR_NOMBRE FROM PCARROCERIA ORDER BY PCAR_NOMBRE", true);
            Utils.FillDll(ddlTipoCombustible, "SELECT PCOM_CODIGO, PCOM_NOMBRE FROM PcombustibleMOTOR PCOM_NOMBRE", true);
            Utils.FillDll(ddlColor, "select pcol_codigo, pcol_descripcion from pcolor order by pcol_descripcion", true);
            Utils.FillDll(ddlCaja, "SELECT RCAJ_CODIGO, RCAJ_NOMBRE FROM RcajaCAMBIOS order by RCAJ_NOMBRE", true);
            Utils.FillDll(ddlTraccion, "SELECT Rtra_CODIGO, Rtra_DESCRIPC FROM RtraccionVEHICULO order by Rtra_DESCRIPC", true);

            cargarRangosKilometraje();
            cargarRangosPrecio();
            cargarRangosModelo();
            cargarRangosCilindraje();
            cargarOpcPlacas();
        }

        private void cargarOpcPlacas()
        {
            ArrayList ddlSource = new ArrayList();
            ddlSource.Add(new DictionaryEntry(ID_PAR, "Par"));
            ddlSource.Add(new DictionaryEntry(ID_IMPAR, "Impar"));
            ddlSource.Add(new DictionaryEntry(ID_NUMERO, "Número"));

            Utils.FillDll(ddlPlaca, ddlSource, true);
        }

        private void cargarRangosCilindraje()
        {
            ArrayList list = DBFunctions.RequestAsCollection("select pccil_limsuperior from PCONFIGRANGOCILINDRAJES order by pccil_limsuperior");
            ArrayList ddlSource = new ArrayList();
            DictionaryEntry entry;
            string key, value;
            int lowLimit = 0;

            foreach (Hashtable hash in list)
            {
                lowLimit++;
                int highLimit = (int)hash["PCCIL_LIMSUPERIOR"];
                key = String.Format("{0}-{1}", lowLimit, highLimit);
                value = String.Format("{0:N0} cc - {1:N0} cc",  lowLimit, highLimit);
                entry = new DictionaryEntry(key, value);
                ddlSource.Add(entry);
                lowLimit = highLimit;
            }

            value = String.Format("Mayor a {0:N0}", lowLimit);
            entry = new DictionaryEntry(lowLimit, value);
            ddlSource.Add(entry);

            Utils.FillDll(ddlCilindraje, ddlSource, true);
        }

        private void cargarRangosPrecio()
        {
            ArrayList list = DBFunctions.RequestAsCollection("select pcpre_limsuperior from pconfigrangoprecios order by pcpre_limsuperior");
            ArrayList ddlSource = new ArrayList();
            DictionaryEntry entry;
            string key, value;
            int lowLimit = 0;

            foreach (Hashtable hash in list)
            {
                lowLimit++;
                int highLimit = (int)hash["PCPRE_LIMSUPERIOR"];
                key = String.Format("{0}-{1}", lowLimit, highLimit);
                value = String.Format("{0:C0} - {1:C0}", lowLimit, highLimit);
                entry = new DictionaryEntry(key, value);
                ddlSource.Add(entry);
                lowLimit = highLimit;
            }

            value = String.Format("Mayor a {0:C0}", lowLimit);
            entry = new DictionaryEntry(lowLimit, value);
            ddlSource.Add(entry);

            Utils.FillDll(ddlPrecio, ddlSource, true);
        }

        private void cargarRangosKilometraje()
        {
            ArrayList list = DBFunctions.RequestAsCollection("select pckil_limsuperior from pconfigrangokilometraje order by pckil_limsuperior");
            ArrayList ddlSource = new ArrayList();
            DictionaryEntry entry;
            string key, value;
            int lowLimit = 0;

            foreach (Hashtable hash in list)
            {
                lowLimit++;
                int highLimit = (int)hash["PCKIL_LIMSUPERIOR"];
                key = String.Format("{0}-{1}", lowLimit, highLimit);
                value = String.Format("{0:N0} - {1:N0}", lowLimit, highLimit);
                entry = new DictionaryEntry(key, value);
                ddlSource.Add(entry);
                lowLimit = highLimit;
            }

            value = String.Format("Mayor a {0:N0}", lowLimit);
            entry = new DictionaryEntry(lowLimit, value);
            ddlSource.Add(entry);

            Utils.FillDll(ddlKilometraje, ddlSource, true);
        }

        private void cargarRangosModelo()
        {
            ArrayList list = DBFunctions.RequestAsCollection("select pcmod_limsuperior from PCONFIGRANGOMODELO order by pcmod_limsuperior");
            ArrayList ddlSource = new ArrayList();
            string firstYear = DBFunctions.SingleData("select min(pano_detalle) from pano");
            int lowLimit = Convert.ToInt32(firstYear) -1;

            foreach (Hashtable hash in list)
            {
                lowLimit++;
                int highLimit = (int)hash["PCMOD_LIMSUPERIOR"];
                string value = String.Format("{0} - {1}", lowLimit, highLimit);
                DictionaryEntry entry = new DictionaryEntry(value, value);
                ddlSource.Add(entry);
                lowLimit = highLimit;
            }
            ddlSource.Add(new DictionaryEntry(ID_MODESPECIFICO, "Específico"));
            Utils.FillDll(ddlModelo, ddlSource, true);
        }

        private string getWhereCilindraje(string cilindraje)
        {
            string[] ciSplit = cilindraje.Split('-');

            string sqlCil = null;
            string whereCil = "";

            if (ciSplit.Length > 1) // rango
                sqlCil = string.Format("select rcil_codigo from RCILINDRAJEMOTOR where rcil_valornum between {0} and {1}", ciSplit[0].Trim(), ciSplit[1].Trim());
            else // mayor que
                sqlCil = string.Format("select rcil_codigo from RCILINDRAJEMOTOR where rcil_valornum > {0}", ciSplit[0].Trim());

            ArrayList arrCilindrajes = DBFunctions.RequestAsCollection(sqlCil);
            foreach (Hashtable hashCil in arrCilindrajes)
                whereCil += string.Format("'{0}',", hashCil["RCIL_CODIGO"]);

            if (whereCil.Length > 0)
            {
                whereCil = string.Format("({0})", whereCil.Substring(0, whereCil.Length - 1));
                return string.Format("AND PC.RCIL_CODIGO IN {0} ", whereCil);
            }

            return "AND PC.RCIL_CODIGO = 'infiniteVoid' ";
        }

        private string getWhereKilometraje(string kilometraje)
        {
            string[] kmSplit = kilometraje.Split('-');

            if (kmSplit.Length > 1) // rango
                return string.Format("AND MV.MCAT_NUMEULTIKILO BETWEEN '{0}' AND '{1}' ", kmSplit[0].Trim(), kmSplit[1].Trim());
            else // mayor que
                return string.Format("AND MV.MCAT_NUMEULTIKILO > '{0}' ", kmSplit[0].Trim());
        }

        private string getWherePrecio(string precio)
        {
            string[] prSplit = precio.Split('-');

            if (prSplit.Length > 1) // rango
                return string.Format("AND MV.MVEH_PRECVENT BETWEEN '{0}' AND '{1}' ", prSplit[0].Trim(), prSplit[1].Trim());
            else // mayor que
                return string.Format("AND MV.MVEH_PRECVENT> '{0}' ", prSplit[0].Trim());
        }

        private string getWherePlaca(string placa)
        {
            string where = ""; // "AND MV.TSER_TIPOSERV = '1' "; // si busca placa, solo se busca servicio particular (publico no tiene pico y placa)

            if (placa.Equals(ID_NUMERO))
            {
                placa = txtPlaca.Text;
                if (!Utils.EsEntero(placa))
                    return null;
                
                where += string.Format("AND MV.MCAT_PLACA like '%{0}' ", placa);
            }
            else if (placa.Equals(ID_PAR))
                where += "AND (MV.MCAT_PLACA like '%2' OR " +
                    "MV.MCAT_PLACA like '%4' OR " +
                    "MV.MCAT_PLACA like '%6' OR " +
                    "MV.MCAT_PLACA like '%8' OR " +
                    "MV.MCAT_PLACA like '%0') ";

            else if (placa.Equals(ID_IMPAR))
                where += "AND (MV.MCAT_PLACA like '%1' OR " +
                    "MV.MCAT_PLACA like '%3' OR " +
                    "MV.MCAT_PLACA like '%5' OR " +
                    "MV.MCAT_PLACA like '%7' OR " +
                    "MV.MCAT_PLACA like '%9') ";

            return where;
        }

        private string getWhereModelo(string modelo)
        {
            if (modelo.Equals(ID_MODESPECIFICO))
            {
                modelo = txtModelo.Text;
                if (!Utils.EsEntero(modelo))
                    return null;

                return string.Format("AND MV.PANO_ANOMODE = {0} ", modelo);
            }
            else
            {
                string[] modSplit = modelo.Split('-');
                return string.Format("AND MV.PANO_ANOMODE BETWEEN {0} AND {1} ", modSplit[0].Trim(), modSplit[1].Trim());
            }
        }

        private string getSelectBusquedaParametros()
        {
            string where = "";
            string sql = "SELECT MV.MVEH_SECUENCIA as ID " +
             "FROM mvehiculousado MV " +
             "LEFT JOIN pcatalogovehiculo PC ON MV.PCAT_CODIGO=PC.PCAT_CODIGO " +
             "WHERE MV.test_tipoesta = 20 AND MV.PUBI_CODIGO IS NOT NULL {0} ";

            string asociado = ddlAsociado.SelectedValue;
            string clase = ddlClase.SelectedValue;
            string marca = ddlMarca.SelectedValue;
            string refPrincipal = ddlReferenciaPrincipal.SelectedValue;
            string refSecundaria = ddlReferenciaSecundaria.SelectedValue;
            string carroceria = ddlCarroceria.SelectedValue;
            string cilindraje = ddlCilindraje.SelectedValue;
            string tipoCombustible = ddlTipoCombustible.SelectedValue;
            string color = ddlColor.SelectedValue;
            string caja = ddlCaja.SelectedValue;
            string traccion = ddlTraccion.SelectedValue;
            string kilometraje = ddlKilometraje.SelectedValue;
            string precio = ddlPrecio.SelectedValue;
            placa = ddlPlaca.SelectedValue;
            
            string modelo = ddlModelo.SelectedValue;

            if (asociado != "0" && asociado != "") // valor por defecto de "Seleccione..."
                where += string.Format("AND MV.MNIT_NIT = '{0}' ", asociado);
            if (clase != "0" && clase != "")
                where += string.Format("AND PC.PCLA_CODIGO = '{0}' ", clase);
            if (marca != "0" && marca != "")
                where += string.Format("AND PC.PMAR_CODIGO = '{0}' ", marca);
            if (refPrincipal != "0" && refPrincipal != "" && refPrincipal != "Seleccione...")
                where += string.Format("AND PC.PGRU_GRUPO = '{0}' ", refPrincipal);
            if (refSecundaria != "0" && refSecundaria != "" && refSecundaria != "Seleccione...")
                where += string.Format("AND PC.PSGRU_CODIGO = '{0}' ", refSecundaria);
            if (carroceria != "0" && carroceria != "")
                where += string.Format("AND PC.PCAR_CODIGO = '{0}' ", carroceria);
            if (tipoCombustible != "0" && tipoCombustible != "")
                where += string.Format("AND PC.PCOM_CODIGO = '{0}' ", tipoCombustible);
            if (color != "0" && color != "")
                where += string.Format("AND MV.PCOL_CODIGO = '{0}' ", color);
            if (caja != "0" && caja != "")
                where += string.Format("AND PC.RCAJ_CODIGO = '{0}' ", caja);
            if (traccion != "0" && traccion != "")
                where += string.Format("AND PC.RTRA_CODIGO = '{0}' ", traccion);
            if (cilindraje != "0" && cilindraje != "")
                where += getWhereCilindraje(cilindraje);
            if (kilometraje != "0" && kilometraje != "")
                where += getWhereKilometraje(kilometraje);
            if (precio != "0" && precio != "")
                where += getWherePrecio(precio);
            if (placa != "0" && placa != "")
            {
                string placaWhere = getWherePlaca(placa);

                if (placaWhere == null)
                {
                    Utils.MostrarAlerta(Response, "En el Campo de Placa, digite un número");
                    return null;
                }

                where += placaWhere;
            }
            if (modelo != "0" && modelo != "")
            {
                string modeloWhere = getWhereModelo(modelo);

                if (modeloWhere == null)
                {
                    Utils.MostrarAlerta(Response, "En el Campo de Modelo, digite un número");
                    return null;
                }

                where += modeloWhere;
            }

            return String.Format(sql, where);
        }

        private string getSelectBusquedaInteligente()
        {
            string searchStr = txtBusquedaInteligente.Text;
            if (searchStr == "")
                return null;

            string[] busquedas = searchStr.Split(' ');

            string sql = "SELECT ID FROM VUSADO_INFO {0}";
            string where = "";

            foreach (string busqueda in busquedas)
            {
                if (where.Length == 0)
                    where = String.Format("WHERE INFO LIKE '%&{0}&%' ", busqueda.ToUpper());
                else
                    where += String.Format("AND INFO LIKE '%&{0}&%' ", busqueda.ToUpper());
            }

            return string.Format(sql, where);
        }

        private void LimpiarGrid()
        {
            dgCarros.DataSource = null;
            dgCarros.DataBind();
            Utils.MostrarAlerta(Response,"No existen resultados con este criterio");
        }

        protected void opcionPlaca(object sender, EventArgs e)
        {
            if (ddlPlaca.SelectedValue == "N")
                txtPlaca.Visible = true;
            else
                txtPlaca.Visible = false;
        }

        private void llenarDataGrid(string sqlIds)
        {
            ArrayList ids = DBFunctions.RequestAsCollection(sqlIds);
            
            if (ids == null)
            {
                LimpiarGrid();
                return;
            }

            string where = "'";
            foreach (Hashtable id in ids)
                where += id["ID"].ToString() + "','";

            if (ids.Count <= 0)
            {
                LimpiarGrid();
                return;
            }
            where = where.Substring(0, where.Length - 2);

            string sql = string.Format(
             "SELECT DISTINCT PCL.PCLA_NOMBRE AS \"Tipo\",   \n" +
             "       PMA.PMAR_NOMBRE AS \"Marca\",   \n" +
             "       PGR.PGRU_NOMBRE AS \"Referencia Principal\",   \n" +
             "       COALESCE(PSG.PSGRU_NOMBRE,'') AS \"Referencia Secundaria\",   \n" +
             "       MV.PANO_ANOMODE AS \"Modelo\",   \n" +
             "       PCR.PCAR_NOMBRE AS \"Carrocería\",   \n" +
             "       RCI.RCIL_NOMBRE AS \"Cilindraje\",   \n" +
             "       PCAT_CABALLAJE CONCAT ' (hp)' AS \"Caballaje\",   \n" +
             "       PCOM.PCOM_NOMBRE AS \"Tipo de Combustible\",   \n" +
             "       RAL.RALI_NOMBRE AS \"Aspiración\",   \n" +
             "       RTR.RTRA_DESCRIPC AS \"Tracción\",   \n" +
             "       RCA.RCAJ_NOMBRE AS \"Caja\",   \n" +
             "       PCOL.PCOL_DESCRIPCION AS \"Color\",   \n" +
             "       MV.MCAT_NUMEULTIKILO AS \"Kilometraje\",   \n" +
             "       MV.MVEH_PRECVENT AS \"Precio\",   \n" +
             "       MV.MCAT_PLACA AS \"Placa\",   \n" +
             "       PCI.PCIU_NOMBRE AS \"Ciudad Placa\",   \n" +
             "       NIT.MNIT_APELLIDOS AS \"Asociado\",  \n" +
             "       PUB.PUBI_DIRECCION AS \"Dirección\",  \n" +
             "       PUB.PUBI_TELEFONO AS \"Teléfono\",  \n" +
             "       PCI2.PCIU_NOMBRE AS \"Ciudad\", \n" +
             "       MOD(CAST(SUBSTRING(MV.MCAT_PLACA,4,1,CODEUNITS16) AS INTEGER) * second(current time) + \n" +
             "       CAST(SUBSTRING(MV.MCAT_PLACA,5,1,CODEUNITS16) AS INTEGER) * minute(current time) + \n" +
             "       CAST(SUBSTRING(MV.MCAT_PLACA,6,1,CODEUNITS16) AS INTEGER) * hour(current time) +   \n" +
             "       ((hour(current time) + minute(current time) + second(current time)) * LENGTH(PUB.PUBI_TELEFONO)),  \n" +
             "       LENGTH(PCI.PCIU_NOMBRE) * 10) AS \"RANDOM\" \n" +
             "FROM MVEHICULOUSADO MV    \n" +
             "  LEFT JOIN PCIUDAD PCI ON MV.PCIU_CODIGO = PCI.PCIU_CODIGO    \n" +
             "  LEFT JOIN PCOLOR PCOL ON MV.PCOL_CODIGO = PCOL.PCOL_CODIGO    \n" +
             "  LEFT JOIN PUBICACION PUBI ON MV.PUBI_CODIGO = PUBI.PUBI_SECUENCIA    \n" +
             "  LEFT JOIN TESTADOVEHICULO TEST ON MV.TEST_TIPOESTA = TEST.TEST_TIPOESTA  \n" +
             "  INNER JOIN MNIT NIT ON MV.MNIT_NIT = NIT.MNIT_NIT  \n" +
             "	LEFT JOIN pubicacion PUB   \n" +
             "		LEFT JOIN PCIUDAD PCI2 ON PUB.PCIU_CODIGO = PCI2.PCIU_CODIGO  \n" +
             "	ON MV.PUBI_CODIGO = PUB.PUBI_SECUENCIA  \n" +
             "  LEFT JOIN PCATALOGOVEHICULO PCA  \n" +
             "    LEFT JOIN RCAJACAMBIOS RCA ON PCA.RCAJ_CODIGO = RCA.RCAJ_CODIGO    \n" +
             "    LEFT JOIN RALIMENTACIONMOTOR RAL ON PCA.RALI_CODIGO = RAL.RALI_CODIGO    \n" +
             "    LEFT JOIN RCILINDRAJEMOTOR RCI ON PCA.RCIL_CODIGO = RCI.RCIL_CODIGO    \n" +
             "    LEFT JOIN RTRACCIONVEHICULO RTR ON PCA.RTRA_CODIGO = RTR.RTRA_CODIGO    \n" +
             "    LEFT JOIN PCARROCERIA PCR ON PCA.PCAR_CODIGO = PCR.PCAR_CODIGO    \n" +
             "    LEFT JOIN PCOMBUSTIBLEMOTOR PCOM ON PCA.PCOM_CODIGO = PCOM.PCOM_CODIGO    \n" +
             "    LEFT JOIN PMARCA PMA ON PCA.PMAR_CODIGO = PMA.PMAR_CODIGO    \n" +
             "    LEFT JOIN PCLASEVEHICULO PCL ON PCA.PCLA_CODIGO = PCL.PCLA_CODIGO    \n" +
             "    LEFT JOIN PGRUPOCATALOGO PGR ON PCA.PGRU_GRUPO = PGR.PGRU_GRUPO    \n" +
             "    LEFT JOIN PSUBGRUPOVEHICULO PSG ON PCA.PSGRU_CODIGO = PSG.PSGRU_GRUPO    \n" +
             "  ON MV.PCAT_CODIGO = PCA.PCAT_CODIGO  \n" +
             " WHERE MV.MVEH_SECUENCIA IN ({0}) \n" +
             " ORDER BY RANDOM"
            , where);

            DataSet ds = new DataSet();

            ds = DBFunctions.Request(ds, IncludeSchema.NO, sql);
            ds.Tables[0].Columns.Remove("RANDOM");
            dgCarros.DataSource = ds;
            dgCarros.DataBind();
        }
	}
}
