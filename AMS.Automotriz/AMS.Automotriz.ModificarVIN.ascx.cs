using System;
using System.Text;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.Mail;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Automotriz
{
	public partial class AMS_Automotriz_ModificarVIN : System.Web.UI.UserControl
	{
		#region Atributos
		private DatasToControls bind = new DatasToControls();
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion

		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
            string tales = Request["__EVENTARGUMENT"];
            if (tales != null && tales != "")
            {
                txtVinViejo.Text = tales;
                cargarInfoVin(sender, e);
            }
            if (!IsPostBack)
			{
                if(Request.QueryString["exito"] != null)
                {
                    Utils.MostrarAlerta(Response, "Proceso realizado correctamente");
                }
                if(Request.QueryString["vin"] != null)
                {
                    txtVinViejo.Text = Request.QueryString["vin"];
                    cargarInfoVin(sender, e);
                }
			}
        }
        protected void cargarInfoVin(object sender, System.EventArgs e)
        {
            if(txtVinViejo.Text.Length > 0)
            {
                if (DBFunctions.RecordExist("SELECT MCAT_VIN FROM MCATALOGOVEHICULO WHERE MCAT_VIN = '" + txtVinViejo.Text + "'"))
                {
                    DataSet ds = new DataSet();
                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT MC.PCAT_CODIGO, MC.MCAT_PLACA, MC.MCAT_MOTOR, MC.MNIT_NIT, MC.PCOL_CODIGO, MC.MCAT_ANOMODE, MV.TCLA_CODIGO FROM MCATALOGOVEHICULO MC, MVEHICULO MV WHERE MC.MCAT_VIN = MV.MCAT_VIN AND MC.MCAT_VIN = '" + txtVinViejo.Text + "'; SELECT PCAT_CODIGO, MCAT_PLACA, MCAT_MOTOR, MNIT_NIT, PCOL_CODIGO, MCAT_ANOMODE FROM MCATALOGOVEHICULO WHERE MCAT_VIN = '" + txtVinViejo.Text + "';");
                    //empezamos con los label
                    if(ds.Tables[0].Rows.Count > 0)
                    {
                        lbCatalogo.Text = "<b style='color:blue'>CATÁLOGO:</b> " + ds.Tables[0].Rows[0]["PCAT_CODIGO"].ToString();
                        lbPlaca.Text = "<b style='color:blue'>PLACA:</b> " + ds.Tables[0].Rows[0]["MCAT_PLACA"].ToString();
                        lbMotor.Text = "<b style='color:blue'>MOTOR:</b> " + ds.Tables[0].Rows[0]["MCAT_MOTOR"].ToString();
                        lbNit.Text = "<b style='color:blue'>NIT:</b> " + ds.Tables[0].Rows[0]["MNIT_NIT"].ToString() + " - " + DBFunctions.SingleData("SELECT NOMBRE FROM VMNIT WHERE MNIT_NIT = '" + ds.Tables[0].Rows[0]["MNIT_NIT"].ToString() + "'");
                        lbColor.Text = "<b style='color:blue'>COLOR:</b> " + ds.Tables[0].Rows[0]["PCOL_CODIGO"].ToString() + " - " + DBFunctions.SingleData("SELECT PCOL_DESCRIPCION FROM PCOLOR WHERE PCOL_CODIGO = '" + ds.Tables[0].Rows[0]["PCOL_CODIGO"].ToString() + "'");
                        lbAno.Text = "<b style='color:blue'>AÑO MODELO:</b> " + ds.Tables[0].Rows[0]["MCAT_ANOMODE"].ToString();
                        lbEstado.Text = ds.Tables[0].Rows[0]["TCLA_CODIGO"].ToString() == "N" ? "Este vehículo es: Nuevo" : "Este vehículo es: Usado";
                    }
                    else
                    {
                        lbCatalogo.Text = "<b style='color:blue'>CATÁLOGO:</b> " + ds.Tables[1].Rows[0]["PCAT_CODIGO"].ToString();
                        lbPlaca.Text = "<b style='color:blue'>PLACA:</b> " + ds.Tables[1].Rows[0]["MCAT_PLACA"].ToString();
                        lbMotor.Text = "<b style='color:blue'>MOTOR:</b> " + ds.Tables[1].Rows[0]["MCAT_MOTOR"].ToString();
                        lbNit.Text = "<b style='color:blue'>NIT:</b> " + ds.Tables[1].Rows[0]["MNIT_NIT"].ToString() + " - " + DBFunctions.SingleData("SELECT NOMBRE FROM VMNIT WHERE MNIT_NIT = '" + ds.Tables[1].Rows[0]["MNIT_NIT"].ToString() + "'");
                        lbColor.Text = "<b style='color:blue'>COLOR:</b> " + ds.Tables[1].Rows[0]["PCOL_CODIGO"].ToString() + " - " + DBFunctions.SingleData("SELECT PCOL_DESCRIPCION FROM PCOLOR WHERE PCOL_CODIGO = '" + ds.Tables[1].Rows[0]["PCOL_CODIGO"].ToString() + "'");
                        lbAno.Text = "<b style='color:blue'>AÑO MODELO:</b> " + ds.Tables[1].Rows[0]["MCAT_ANOMODE"].ToString();
                        
                    }
                    btnConfirmarVin.Visible = fldInfoVin.Visible = lbText.Visible = txtVinNuevo.Visible = true;
                    txtVinViejo.Enabled = imglupa1.Visible = false;

                }
                else
                    Utils.MostrarAlerta(Response, "El Vin escrito no existe en la Base de Datos, por favor revise.");
            }
            else
                Utils.MostrarAlerta(Response, "Escriba un VIN..");
        }

        protected void confirmarVin(object sender, EventArgs e)
        {
            if (txtVinViejo.Text == txtVinNuevo.Text)
            {
                Utils.MostrarAlerta(Response, "El VIN no puede ser el mismo.");
                btnConfirmarVin.Enabled = true;
                return;
            }
            lbConfirmacion.Text = "";
            txtVinNuevo.Visible = btnConfirmarVin.Visible = false;
            if (DBFunctions.RecordExist("SELECT MCAT_VIN FROM MCATALOGOVEHICULO WHERE MCAT_VIN = '" + txtVinNuevo.Text + "'"))
            {
                lbConfirmacion.Text = "<font color='red' size='4.3px'> El VIN: " + txtVinNuevo.Text + " ya existe en la tabla principal!! </font> <br />";
                lbConfirmacion.Text += "&emsp; <font color='red' size='3.8px'>Se cambiará el VIN sólo en las tablas relaciones(El VIN nuevo ya existe).";
            }
            else
            {
                lbConfirmacion.Text = "<font color='green' size='4.3px'> El VIN: " + txtVinNuevo.Text + " no existe en la tabla principal!! </font> <br />";
                lbConfirmacion.Text += "&emsp; <font color='green' size=3.8px'>Se cambiará el Vin en la tabla principal y adyacentes(VIN nuevo No existe).";
            }
            lbText.Visible = false;
            btnCambio.Visible = true;
            btnCambio.Attributes.Add("onclick", "return confirm('No se podrá anular el cambio.. Está seguro de realizar el proceso?');");
        }

        protected void btnCambio_Click(object sender, System.EventArgs e)
		{
            string sql, sql2;
            bool continuidad = false;
            string catalogo = DBFunctions.SingleData("SELECT PCAT_CODIGO FROM PCATALOGOVEHICULO WHERE PCAT_CODIGO != '' FETCH FIRST ROW ONLY");
            string pivote = "1XXXOOO1";
            lbDescripcion.Text = "";
            DataSet listaTablasReferencia = new DataSet();
            ArrayList listaTablasVin = new ArrayList();
            string tablaPrincipal = "MCATALOGOVEHICULO";
            string campoVin = "MCAT_VIN";
            string color = DBFunctions.SingleData("SELECT PCOL_CODIGO FROM DBXSCHEMA.PCOLOR FETCH FIRST ROW ONLY");
            string servicio = DBFunctions.SingleData("SELECT TSER_TIPOSERV FROM DBXSCHEMA.TSERVICIOVEHICULO FETCH FIRST ROW ONLY");
            DBFunctions.Request(listaTablasReferencia, IncludeSchema.NO, "SELECT TBNAME, FKCOLNAMES, COLCOUNT FROM SYSIBM.SYSRELS WHERE FKCOLNAMES LIKE '%" + campoVin + "%'  AND REFTBNAME = '" + tablaPrincipal + "'; SELECT MCAT_VIN FROM " + tablaPrincipal + ";"); //cargamos todas las tablas foráneas
            
            //aunque existe la foraneidad teórica, algunas tablas no tienen la referencia de foraneidad, por eso hay que traer igual todas las tablas que usen a mcat_vin
            listaTablasVin = DBFunctions.RequestAsCollection("SELECT NAME, TBNAME, REMARKS FROM SYSIBM.SYSCOLUMNS WHERE NAME LIKE '%" + campoVin + "%' AND TBNAME NOT LIKE 'V%' AND TBCREATOR = 'DBXSCHEMA';");
            ArrayList script = new ArrayList();
            ArrayList script2 = new ArrayList();//se usan dos array cuando el vin nuevo no existe
            
            //Existe el nuevo VIN en la tabla principal?
            if(listaTablasReferencia.Tables[1].Select("MCAT_VIN = '" + txtVinNuevo.Text + "'").Length > 0)//DBFunctions.RecordExist("SELECT MCAT_VIN FROM " + tablaPrincipal + " WHERE MCAT_VIN = '" + txtVinNuevo.Text + "'"))
            {
                //este proceso cambia todos lo VIN de las tablas adyacentes al VIN escrito
                for (int i = 0; i < listaTablasVin.Count; i++)
                {
                    string tabla = ((Hashtable)listaTablasVin[i])["TBNAME"].ToString().Trim();//tabla que vamos sacando
                    if (tabla != tablaPrincipal)
                    {
                        if (DBFunctions.RecordExist("SELECT MCAT_VIN FROM " + tabla + " WHERE MCAT_VIN = '" + txtVinViejo.Text + "'"))
                        {
                            sql = "UPDATE " + tabla + " SET MCAT_VIN = '" + txtVinNuevo.Text + "' WHERE MCAT_VIN = '" + txtVinViejo.Text + "'";
                            script.Add(sql);
                        }
                    }
                }
                script.Add("DELETE FROM MCATALOGOVEHICULO WHERE MCAT_VIN = '" + txtVinViejo.Text + "'");
                if(DBFunctions.Transaction(script))
                    Response.Redirect(indexPage + "?process=Automotriz.ModificarVin&exito=1");

            }
            else//no existe VIN Nuevo en la tabla principal
            {
                if (listaTablasReferencia.Tables[1].Select("MCAT_VIN = '1XXXOOO1'").Length == 0)//DBFunctions.RecordExist("SELECT MCAT_VIN FROM " + tablaPrincipal + " WHERE MCAT_VIN = '1XXXOOO1'"))
                {
                    int pivot = DBFunctions.NonQuery(@"INSERT INTO DBXSCHEMA.MCATALOGOVEHICULO(
                                PCAT_CODIGO,
                                MCAT_VIN,
                                MCAT_PLACA,
                                MCAT_MOTOR,
                                MNIT_NIT,
                                MCAT_SERIE,
                                PCOL_CODIGO,
                                MCAT_ANOMODE,
                                TSER_TIPOSERV,
                                MCAT_CONCVEND,
                                MCAT_VENTA,
                                MCAT_NUMEKILOVENT,
                                MCAT_NUMEULTIKILO,
                                MCAT_NUMEKILOPROM,
                                MCAT_CATEGORIA,
                                MCAT_FECHULTIKILO,
                                MCAT_MATRICULA)
                                VALUES('" + catalogo + "','" + pivote + "','ZZZ999','BASE ECAS','800081328','BASE ECAS','" + color + "',2999,'" + servicio + "','2999-12-31',DATE '2016-12-01',0.0000,0.0000,0.0000,'Z','2015-12-12',null);");
                    if (pivot < 1)
                    {
                        lbDescripcion.Text = "No se pudo crear el registro pivote, contacte al administrador del sistema.";
                        return;
                    }
                }
                for (int i = 0; i < listaTablasVin.Count; i++)
                {
                    string tabla = ((Hashtable)listaTablasVin[i])["TBNAME"].ToString().Trim();//tabla que vamos sacando
                    if (tabla != tablaPrincipal)
                    {
                        if (DBFunctions.RecordExist("SELECT MCAT_VIN FROM " + tabla + " WHERE MCAT_VIN = '" + txtVinViejo.Text + "'"))
                        {
                            sql = "UPDATE " + tabla + " SET MCAT_VIN = '" + pivote + "' WHERE MCAT_VIN = '" + txtVinViejo.Text + "'";
                            sql2 = "UPDATE " + tabla + " SET MCAT_VIN = '" + txtVinNuevo.Text + "' WHERE MCAT_VIN = '" + pivote + "'";
                            script.Add(sql);
                            script2.Add(sql2);
                        }
                    }
                }
                script.Add("UPDATE " + tablaPrincipal + " SET MCAT_VIN = '" + txtVinNuevo.Text + "' WHERE MCAT_VIN = '" + txtVinViejo.Text + "'");

                if (DBFunctions.Transaction(script))
                {
                    continuidad = true;
                }
                else
                {
                    lbDescripcion.Text += "<br /> Se ha generado un problema actualizando los datos. -> " + DBFunctions.exceptions;
                }
                if (continuidad)
                {
                    if (DBFunctions.Transaction(script2))
                    {
                        Response.Redirect(indexPage + "?process=Automotriz.ModificarVin&exito=1");
                    }
                }
                else
                    Utils.MostrarAlerta(Response, "Ocurrió un problema tratando de cambiar el VIN");
            }
        }
        #endregion

        #region Metodos
        private void CargarVINsCombos(DropDownList ddlInstancia)
        {
            bind.PutDatasIntoDropDownList(ddlInstancia, "SELECT mcat_vin, mcat_vin FROM mcatalogovehiculo ORDER BY mcat_vin");
            ddlInstancia.Items.Insert(0, new ListItem("Seleccione ...", String.Empty));
        
        }
		#endregion

		#region Código generado por el Diseñador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Diseñador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Método necesario para admitir el Diseñador. No se puede modificar
		///		el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{

		}

        public static string ModificacionVINporTaller(string vinViejo, string vinNuevo)
        {
            /*
                ArrayList sqlStrings = new ArrayList();
                DataSet dsQuery = new DataSet();
                //DBFunctions.Request(dsQuery, IncludeSchema.NO, "SELECT pcat_codigo,mcat_vin,mcat_placa,mcat_motor,mnit_nit,mcat_serie,mcat_chasis,pcol_codigo,mcat_anomode,tser_tiposerv,mcat_vencseguobli,mcat_concvend,mcat_venta,mcat_numekilovent,mcat_numeradio,mcat_numeultikilo,mcat_numekiloprom,mcat_categoria,mcat_password,mcat_fechultikilo FROM mcatalogovehiculo WHERE pcat_codigo='" + catalogoViejo + "' AND mcat_vin='" + vinViejo + "';" +
                                                             "SELECT pcat_codigo,mcat_vin,mcat_placa,mcat_motor,mnit_nit,mcat_serie,mcat_chasis,pcol_codigo,mcat_anomode,tser_tiposerv,mcat_vencseguobli,mcat_concvend,mcat_venta,mcat_numekilovent,mcat_numeradio,mcat_numeultikilo,mcat_numekiloprom,mcat_categoria,mcat_password,mcat_fechultikilo FROM mcatalogovehiculo WHERE pcat_codigo='" + catalogoNuevo + "' AND mcat_vin='" + vinNuevo + "';");
                DateTime fechaVenceSeguro = Convert.ToDateTime(null);
                DateTime fechaVenta = Convert.ToDateTime(null);
                try { fechaVenceSeguro = Convert.ToDateTime(dsQuery.Tables[0].Rows[0]["mcat_vencseguobli"]); } catch { }
                try { fechaVenceSeguro = Convert.ToDateTime(dsQuery.Tables[0].Rows[0]["mcat_venta"]); } catch { }
                sqlStrings.Add("UPDATE mcatalogovehiculo SET mcat_placa='666ZZZ', mcat_motor='AAABBB777666ZZ.!' WHERE pcat_codigo='" + catalogoViejo + "' AND mcat_vin='" + vinViejo + "'");
                if (dsQuery.Tables[1].Rows.Count > 0)
                    sqlStrings.Add("UPDATE mcatalogovehiculo SET mcat_placa='" + dsQuery.Tables[0].Rows[0]["mcat_placa"] + "',mcat_motor='" + dsQuery.Tables[0].Rows[0]["mcat_motor"] + "',mnit_nit='" + dsQuery.Tables[0].Rows[0]["mnit_nit"] + "',mcat_serie='" + dsQuery.Tables[0].Rows[0]["mcat_serie"] + "',mcat_chasis='" + dsQuery.Tables[0].Rows[0]["mcat_chasis"] + "',pcol_codigo='" + dsQuery.Tables[0].Rows[0]["pcol_codigo"] + "',mcat_anomode=" + dsQuery.Tables[0].Rows[0]["mcat_anomode"] + ",tser_tiposerv='" + dsQuery.Tables[0].Rows[0]["tser_tiposerv"] + "',mcat_vencseguobli='" + fechaVenceSeguro.ToString("yyyy-MM-dd") + "',mcat_concvend='" + dsQuery.Tables[0].Rows[0]["mcat_concvend"] + "',mcat_venta='" + fechaVenta.ToString("yyyy-MM-dd") + "',mcat_numekilovent=" + dsQuery.Tables[0].Rows[0]["mcat_numekilovent"] + ",mcat_numeradio='" + dsQuery.Tables[0].Rows[0]["mcat_numeradio"] + "',mcat_numeultikilo=" + dsQuery.Tables[0].Rows[0]["mcat_numeultikilo"] + ",mcat_numekiloprom=" + dsQuery.Tables[0].Rows[0]["mcat_numekiloprom"] + ",mcat_categoria='" + dsQuery.Tables[0].Rows[0]["mcat_categoria"] + "',mcat_password='" + dsQuery.Tables[0].Rows[0]["mcat_password"] + "',mcat_fechultikilo='" + dsQuery.Tables[0].Rows[0]["mcat_fechultikilo"] + "' WHERE pcat_codigo='" + catalogoNuevo + "' AND mcat_vin='" + vinNuevo + "'");
                else
                    sqlStrings.Add("INSERT INTO mcatalogovehiculo(pcat_codigo,mcat_vin,mcat_placa,mcat_motor,mnit_nit,mcat_serie,mcat_chasis,pcol_codigo,mcat_anomode,tser_tiposerv,mcat_vencseguobli,mcat_concvend,mcat_venta,mcat_numekilovent,mcat_numeradio,mcat_numeultikilo,mcat_numekiloprom,mcat_categoria,mcat_password,mcat_fechultikilo) VALUES('" + catalogoNuevo + "','" + vinNuevo + "','" + dsQuery.Tables[0].Rows[0]["mcat_placa"] + "','" + dsQuery.Tables[0].Rows[0]["mcat_motor"] + "','" + dsQuery.Tables[0].Rows[0]["mnit_nit"] + "','" + dsQuery.Tables[0].Rows[0]["mcat_serie"] + "','" + dsQuery.Tables[0].Rows[0]["mcat_chasis"] + "','" + dsQuery.Tables[0].Rows[0]["pcol_codigo"] + "'," + dsQuery.Tables[0].Rows[0]["mcat_anomode"] + ",'" + dsQuery.Tables[0].Rows[0]["tser_tiposerv"] + "','" + fechaVenceSeguro.ToString("yyyy-MM-dd") + "','" + dsQuery.Tables[0].Rows[0]["mcat_concvend"] + "','" + fechaVenta.ToString("yyyy-MM-dd") + "'," + dsQuery.Tables[0].Rows[0]["mcat_numekilovent"] + ",'" + dsQuery.Tables[0].Rows[0]["mcat_numeradio"] + "'," + dsQuery.Tables[0].Rows[0]["mcat_numeultikilo"] + "," + dsQuery.Tables[0].Rows[0]["mcat_numekiloprom"] + ",'" + dsQuery.Tables[0].Rows[0]["mcat_categoria"] + "','" + dsQuery.Tables[0].Rows[0]["mcat_password"] + "','" + dsQuery.Tables[0].Rows[0]["mcat_fechultikilo"] + "',null)");
                sqlStrings.Add("UPDATE morden SET mcat_vin='" + vinNuevo + "' WHERE mcat_vin='" + vinViejo + "'");
                sqlStrings.Add("UPDATE mubicacionvehiculo SET pcat_codigo='" + catalogoNuevo + "',mcat_vin='" + vinNuevo + "' WHERE pcat_codigo='" + catalogoViejo + "' AND mcat_vin='" + vinViejo + "'");
                sqlStrings.Add("UPDATE mvehiculo SET mcat_vin='" + vinNuevo + "' WHERE mcat_vin='" + vinViejo + "'");
                sqlStrings.Add("delete from mcatalogovehiculo WHERE pcat_codigo='" + catalogoViejo + "' AND mcat_vin='" + vinViejo + "'");
                string status = String.Empty;
                if (!DBFunctions.Transaction(sqlStrings))
                    status = DBFunctions.exceptions;
                return status;
            */
            return null;
        }
        protected void modificarForaneidad(object sender, EventArgs z)
        {
            ArrayList script = new ArrayList();
            string campo, tablaPrincipal;
            campo = "PCAT_CODIGO";
            tablaPrincipal = "PCATALOGOVEHICULO";
            ArrayList listaTablasVin = new ArrayList();
            DataSet listaTablasReferencia = new DataSet();
            DBFunctions.Request(listaTablasReferencia, IncludeSchema.NO, "SELECT TBNAME, FKCOLNAMES, COLCOUNT FROM SYSIBM.SYSRELS WHERE FKCOLNAMES LIKE '%" + campo + "%'  AND REFTBNAME = '" + tablaPrincipal + "'; SELECT MCAT_VIN FROM " + tablaPrincipal + " ORDER BY TBNAME;"); //cargamos todas las tablas foráneas
            listaTablasVin = DBFunctions.RequestAsCollection("SELECT NAME, TBNAME, REMARKS FROM SYSIBM.SYSCOLUMNS WHERE NAME LIKE '%" + campo + "%' AND TBNAME NOT LIKE 'V%' AND TBCREATOR = 'DBXSCHEMA' ORDER BY TBNAME;");
            //ACÁ SE CREARÍAN TODOS LOS SCRIPTS DE FORANEIDAD, PERO AL APRECER NO SON NECESARIOS

            for (int i = 0; i < listaTablasVin.Count; i++)
            {
                string tabla = ((Hashtable)listaTablasVin[i])["TBNAME"].ToString();//tabla que vamos sacando
                if(DBFunctions.SingleData("SELECT TYPE FROM SYSCAT.TABLES WHERE TABNAME = '" + tabla + "' AND TABSCHEMA = 'DBXSCHEMA';") == "T")// T = tabla / V = view
                //if (listaTablasReferencia.Tables[0].Select("TBNAME = '" + tabla + "'").Length == 0 && tabla != tablaPrincipal)
                //{
                    //string campo1 = ((Hashtable)listaTablasVin[i])["NAME"].ToString().Trim();
                    //script.Add("ALTER TABLE " + tabla + " ADD CONSTRAINT FK_" + tabla + "_MCAT_VIN FOREIGN KEY (" + campo1 + ") REFERENCES DBXSCHEMA." + tablaPrincipal + "(" + campo + ");");
                    script.Add("ALTER TABLE " + tabla.Trim() + " ALTER COLUMN " + campo.Trim() + " SET DATA TYPE VARCHAR(15) ;");
                //}
            }
            if(DBFunctions.Transaction(script))
            {
                lbScript.Text = "Se han cambiado los tipos de datos de TODAS las tablas!";
            }
            else
            {
                lbScript.Text = "Fallamos!" + DBFunctions.exceptions;
            }
           // for(int i = 0; i < script.Count; i ++)
                //lbScript.Text += "<br /> " + script[i];
        }
        #endregion
    }
}
