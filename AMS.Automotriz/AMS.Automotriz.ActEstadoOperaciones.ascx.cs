
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
	using System.Collections;
	using System.Configuration;
    using AMS.Tools;
    using System.Text;
    using System.IO;
    using System.Web.UI;

namespace AMS.Automotriz
{
	/// <summary>
	///		Descripción breve de AMS_Automotriz_ActEstadoOperaciones.
	/// </summary>
	public partial class AMS_Automotriz_ActEstadoOperaciones : System.Web.UI.UserControl
	{
		protected DataTable tablaOperaciones;
        protected DataSet dsAux;

        protected string indexPage= ConfigurationManager.AppSettings["MainIndexPage"];

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
                dgOperaciones.Visible = true;
				//string usuario=HttpContext.Current.User.Identity.Name.ToLower();
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlMecanicos,"Select pven_codigo, TRIM(pven_nombre) concat ' - ' concat pven_codigo from DBXSCHEMA.pvendedor where TVEND_CODIGO='MG' and pven_vigencia='V' order by pven_nombre asc");
                if (ddlMecanicos.Items.Count > 1)
                    ddlMecanicos.Items.Insert(0, "Seleccione:..");
                string sql = "Select test_estaoper,test_nombre from DBXSCHEMA.TESTADOOPERACION where Test_estaoper in ('I','A') order by test_nombre; " +
                             "Select test_estaoper,test_nombre from DBXSCHEMA.TESTADOOPERACION where Test_estaoper not in ('A','S') order by test_nombre; " +
                             "Select test_estaoper,test_nombre from DBXSCHEMA.TESTADOOPERACION where Test_estaoper not in ('A','S','C') order by test_nombre; ";

                dsAux = new DataSet();
                DBFunctions.Request(dsAux, IncludeSchema.NO, sql);
                Session["ddlOp"] = dsAux;
                //ViewState["ddlOperacion"] = dsAux.Tables[0];
			}
			else
			{
				if (Session["tablaOperaciones"]!=null)
					tablaOperaciones=(DataTable)Session["tablaOperaciones"];
			}
		}

		protected void ValidarPass(object sender, EventArgs e)
		{
			string claveBd=DBFunctions.SingleData("select pven_clave from DBXSCHEMA.PVENDEDOR where pven_codigo='"+ddlMecanicos.SelectedValue+"' ");
			if (txtClave.Text.Equals(claveBd))
			{
				Generar(ddlMecanicos.SelectedValue);
                lbMail.Visible = txtMail.Visible = fldMail.Visible = true;
				//ddlMecanicos.Enabled=false;
			}
			else
                Utils.MostrarAlerta(Response, "La Clave es invalida, Revise por favor");
		}
       
        
		protected void Preparar_Tabla_Operaciones()
		{
			tablaOperaciones = new DataTable();
			tablaOperaciones.Columns.Add(new DataColumn("PREFIJO",System.Type.GetType("System.String")));
			tablaOperaciones.Columns.Add(new DataColumn("NUMERO",System.Type.GetType("System.String")));
			tablaOperaciones.Columns.Add(new DataColumn("CODIGO",System.Type.GetType("System.String")));
			tablaOperaciones.Columns.Add(new DataColumn("OPERACION",System.Type.GetType("System.String"))); 
			tablaOperaciones.Columns.Add(new DataColumn("ESTADO_OPERACION",System.Type.GetType("System.String")));
			tablaOperaciones.Columns.Add(new DataColumn("OBSERVACIONES",System.Type.GetType("System.String")));
			tablaOperaciones.Columns.Add(new DataColumn("PLACA",System.Type.GetType("System.String")));
            tablaOperaciones.Columns.Add(new DataColumn("FECHA", System.Type.GetType("System.String")));
		}

        protected void ingresar_datos_Operaciones(string codigo, string operacion, string estadoO, string prefijo, string numeroOT, string placa, string observaciones, string fecha)
		{
			DataRow fila = tablaOperaciones.NewRow();
			fila["PREFIJO"] = prefijo;
			fila["NUMERO"]  = numeroOT;
			fila["CODIGO"]  = codigo;
			fila["OPERACION"]=operacion;
			fila["ESTADO_OPERACION"]=estadoO;
			fila["PLACA"] = placa;
            fila["OBSERVACIONES"] = observaciones;
            fila["FECHA"] = fecha;
			tablaOperaciones.Rows.Add(fila);
		}

		
		protected void DataBound_Operaciones(object sender,DataGridItemEventArgs e)
		{
			if (e.Item.ItemType==ListItemType.Item || e.Item.ItemType==ListItemType.AlternatingItem)
			{
                //así estaba
                //DataTable dt = (DataTable)ViewState["ddlOperacion"];
                DataTable dt;

                if (tablaOperaciones.Rows[e.Item.DataSetIndex][4].ToString() == "A")
                    dt = ((DataSet)Session["ddlOp"]).Tables[0];
                    //dt = dsAux.Tables[0];
                else if (tablaOperaciones.Rows[e.Item.DataSetIndex][4].ToString() == "I")
                    dt = ((DataSet)Session["ddlOp"]).Tables[1];
                    //dt = dsAux.Tables[1];
                else
                    dt = ((DataSet)Session["ddlOp"]).Tables[2];
                    //dt = dsAux.Tables[2];

                DropDownList ddl = (DropDownList)e.Item.Cells[4].FindControl("ddlOperaciones");
                TextBox txt = (TextBox)e.Item.Cells[4].FindControl("txtObservaciones");
                try
                {
                    ddl.DataSource = dt;
                    ddl.DataValueField = dt.Columns[0].ColumnName;
                    ddl.DataTextField = dt.Columns[1].ColumnName;
                    ddl.DataBind();
                    DatasToControls.EstablecerValueDropDownList(ddl, tablaOperaciones.Rows[e.Item.DataSetIndex][4].ToString());
                    int num = 0;
                    //for(int i = 0; i < tablaOperaciones.Rows.Count; i ++)
                    //{
                    //    if(tablaOperaciones.Rows[i][4].ToString() == "I")
                    //    {
                    //        num = i;
                    //        for(int j = 0; j < tablaOperaciones.Rows.Count; j++)
                    //        {
                    //            if(j == num) { ddl.Enabled = true; }
                    //            else
                    //            {
                    //                ddl.Enabled = false;
                    //            }
                    //        }
                    //    }
                    //}
                    txt.Text = tablaOperaciones.Rows[e.Item.DataSetIndex][5].ToString();
                }
                catch (Exception i)
                {

                }
                  
			}
		}

		protected void Operaciones(string codigo)
		{
			DataSet Operacion = new DataSet();

            string sqlOpers = String.Format(
             @"SELECT dorden.ptem_operacion,      
                     ptem.ptem_descripcion,      
                     dorden.test_estado,      
                     morden.pdoc_codigo,      
                     morden.mord_numeorde,      
                     mcat.mcat_placa CONCAT ' - ' CONCAT pcat.pcat_descripcion,      
                     dob.DOBSOT_OBSERVACIONES,      
                     dorden.DORD_FECHCUMP      
              FROM DORDENOPERACION dorden       
                INNER JOIN ptempario ptem ON (dorden.ptem_operacion = ptem.ptem_operacion)       
                INNER JOIN morden morden ON (morden.pdoc_codigo = dorden.pdoc_codigo AND morden.mord_numeorde = dorden.mord_numeorde AND morden.test_estado <> 'F')        
                INNER JOIN Mcatalogovehiculo Mcat ON (morden.Mcat_VIN = MCAT.Mcat_VIN)       
                INNER JOIN pcatalogovehiculo pcat ON (pcat.pcat_codigo = Mcat.pcat_codigo)       
                LEFT JOIN dobservacionesot dob ON (dob.pdoc_codigo = dorden.pdoc_codigo AND dob.mord_numeorde = dorden.mord_numeorde AND dob.ptem_operacion = dorden.ptem_operacion)        
              WHERE dorden.pven_codigo = '{0}' and dorden.test_estado <> 'X' and dorden.test_estado <> 'C'      
              ORDER BY mcat.mcat_placa"
             , codigo);
            DBFunctions.Request(Operacion, IncludeSchema.NO, sqlOpers);

            if (Operacion.Tables[0].Rows.Count > 0)
            {
                this.Preparar_Tabla_Operaciones();
                ddlMecanicos.Enabled=false;
                for (int i = 0; i < Operacion.Tables[0].Rows.Count; i++)
                {
                    this.ingresar_datos_Operaciones(
                        Operacion.Tables[0].Rows[i][0].ToString(),   // código
                        Operacion.Tables[0].Rows[i][1].ToString(),   // operación
                        Operacion.Tables[0].Rows[i][2].ToString(),   // Estado
                        Operacion.Tables[0].Rows[i][3].ToString(),   // Pref OT
                        Operacion.Tables[0].Rows[i][4].ToString(),   // Num OT
                        Operacion.Tables[0].Rows[i][5].ToString(),   // Placa
                        Operacion.Tables[0].Rows[i][6].ToString(),   // Observaciones
                        Operacion.Tables[0].Rows[i][7].ToString());  // Fecha
                }
                this.dgOperaciones.DataSource = tablaOperaciones;
                dgOperaciones.DataBind();
                DatasToControls.Aplicar_Formato_Grilla(dgOperaciones);
                DatasToControls.JustificacionGrilla(dgOperaciones, tablaOperaciones);

                btnActualizar.Visible = true;
                btnValidarPass.Visible = false;
                dgOperaciones.Visible = true;
            }
            else
            {
                Utils.MostrarAlerta(Response, "El técnico seleccionado, No tiene Operaciones");
                ddlMecanicos.Enabled = true;
                btnActualizar.Visible = false;
                btnValidarPass.Visible = true;
                dgOperaciones.Visible = false;
                dgOperaciones.DataSource = null;
                dgOperaciones.DataBind();
            }
		}
	
		protected void Generar(string user)
        {
            this.Operaciones(user);
            Session["tablaOperaciones"] = tablaOperaciones;
        }

        protected void Actualizar(Object Sender, EventArgs e)
        {
            string tiempoLi = "0";
            string claveBd = DBFunctions.SingleData("select pven_clave from DBXSCHEMA.PVENDEDOR where pven_codigo='" + ddlMecanicos.SelectedValue + "' ");
            
            String sqlOper = String.Format(
                 @"SELECT coalesce(ptiempotaller.ptie_tiemclie,0),      
                     coalesce(dorden.dord_tiemliqu,0),      
                     dob.DOBSOT_SECUENCIA,      
                     dob.DOBSOT_OBSERVACIONES      
              FROM DBXSCHEMA.DORDENOPERACION dorden       
                INNER JOIN dbxschema.ptempario ptem ON (dorden.ptem_operacion = ptem.ptem_operacion)       
                INNER JOIN dbxschema.morden morden ON (morden.pdoc_codigo = dorden.pdoc_codigo AND morden.mord_numeorde = dorden.mord_numeorde AND morden.test_estado <> 'F')   
                INNER JOIN dbxschema.mcatalogovehiculo mcat ON (morden.mcat_vin = mcat.mcat_vin)       
                INNER JOIN dbxschema.pcatalogovehiculo pcat ON (pcat.pcat_codigo = mcat.pcat_codigo)       
                INNER JOIN dbxschema.pgrupocatalogo pgrup ON (pcat.pgru_grupo = pgrup.pgru_grupo)       
                LEFT JOIN dbxschema.ptiempotaller ptiempotaller ON ptiempotaller.ptie_grupcata = pgrup.pgru_grupo AND ptiempotaller.ptie_tempario = ptem.ptem_operacion 
                LEFT JOIN dobservacionesot dob ON (dob.pdoc_codigo = dorden.pdoc_codigo AND dob.mord_numeorde = dorden.mord_numeorde AND dob.ptem_operacion = dorden.ptem_operacion)  
               WHERE dorden.pven_codigo = '{0}' and dorden.test_estado <> 'X' and dorden.test_estado <> 'C' ORDER BY mcat.mcat_placa;"
                 , ddlMecanicos.SelectedValue);
            DataSet Operacion = new DataSet();
            DBFunctions.Request(Operacion, IncludeSchema.NO, sqlOper);
            ArrayList sql = new ArrayList();
            string fechaActual = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            for (int i = 0; i < tablaOperaciones.Rows.Count; i++)
            {
               

                string nuevoEstado = ((DropDownList)dgOperaciones.Items[i].Cells[4].FindControl("ddlOperaciones")).SelectedValue.ToString();
                string nuevaObservacion = ((TextBox)dgOperaciones.Items[i].Cells[5].FindControl("txtObservaciones")).Text;

               
                if (tablaOperaciones.Rows[i][4].ToString() != nuevoEstado)
                {
                    sql.Add("update DBXSCHEMA.DORDENOPERACION set test_estado='" + nuevoEstado +
                        "', dord_fechcump='" + fechaActual + 
                        "' where ptem_operacion='" + tablaOperaciones.Rows[i][2].ToString() + 
                        "' and pdoc_codigo='" + tablaOperaciones.Rows[i][0].ToString() + "' and mord_numeorde=" + tablaOperaciones.Rows[i][1].ToString() + "  ");

                    DateTime fechaAnterior = Convert.ToDateTime(tablaOperaciones.Rows[i][7].ToString());

                    sql.Add("insert into DBXSCHEMA.DESTADISTICAOPERACION values (default,'" + 
                        tablaOperaciones.Rows[i][0].ToString() + "'," + 
                        tablaOperaciones.Rows[i][1].ToString() + ",'" + 
                        tablaOperaciones.Rows[i][2].ToString() + "','" + 
                        ddlMecanicos.SelectedValue + "','" +
                        nuevoEstado + "','" + fechaActual + "','" +
                        tablaOperaciones.Rows[i][4].ToString() + "','" +
                        fechaAnterior.ToString("yyyy-MM-dd HH:mm:ss") + "')");

                    //Operacion.Tables[0].Rows[i][0].ToString();

                    if (((DropDownList)dgOperaciones.Items[i].Cells[4].FindControl("ddlOperaciones")).SelectedValue.ToString() == "C")
                    {
                        if (Operacion.Tables[0].Rows[i][1].ToString() == "0.00" && Operacion.Tables[0].Rows[i][0].ToString() != "0.00")
                            tiempoLi = Operacion.Tables[0].Rows[i][0].ToString();
                        else
                            tiempoLi = Operacion.Tables[0].Rows[i][1].ToString();
                        
                        sql.Add("update DBXSCHEMA.DORDENOPERACION set dord_tiemliqu=" + tiempoLi +
                            ", dord_fechcump='" + fechaActual + 
                            "' where ptem_operacion='" + tablaOperaciones.Rows[i][2].ToString() + 
                            "' and pdoc_codigo='" + tablaOperaciones.Rows[i][0].ToString() + 
                            "' and mord_numeorde=" + tablaOperaciones.Rows[i][1].ToString());
                    }
                }

                if (nuevaObservacion != Operacion.Tables[0].Rows[i][3].ToString())
                    if (Operacion.Tables[0].Rows[i][3].ToString() == "")
                        sql.Add("insert into DBXSCHEMA.DOBSERVACIONESOT values (default,'" + tablaOperaciones.Rows[i][0].ToString() + "'," + tablaOperaciones.Rows[i][1].ToString() + ",'" + tablaOperaciones.Rows[i][2].ToString() + "','" + ((TextBox)dgOperaciones.Items[i].Cells[5].FindControl("txtObservaciones")).Text + "','" + ddlMecanicos.SelectedValue + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')  ");
                    else
                        sql.Add(String.Format("update dobservacionesot set DOBSOT_OBSERVACIONES='{0}', DOBSOT_PROCESADO='{1}' where DOBSOT_SECUENCIA={2}",
                            nuevaObservacion, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Operacion.Tables[0].Rows[i][2].ToString()));
                
                if (nuevoEstado == "R" && txtMail.Text != "")
                {
                    tablaOperaciones.Rows[i][4] = "Repuestos Pendientes";
                    tablaOperaciones.Rows[i][5] = nuevaObservacion;
                }
            }
            if(txtMail.Text != "" && txtMail.Text.Contains("@"))
            {
               // DataRow newRow = new DataRow();
                DataTable dtMail = tablaOperaciones.Clone();

                //Sacamos las filas que hayan cambiado o sean del tipo R (Repuestos pendientes)
                DataRow[] listDr = tablaOperaciones.Select("ESTADO_OPERACION = 'Repuestos Pendientes'");
                //Y agregamos dichas filas
                dtMail = listDr.CopyToDataTable<DataRow>();

                dgMail.DataSource = dtMail;
                dgMail.BackColor = System.Drawing.Color.Azure;
                dgMail.HeaderStyle.BackColor = System.Drawing.Color.Coral;
                dgMail.DataBind();

                if (dgMail.Items.Count > 0)
                {
                    try
                    {
                        AMS.Tools.Utils.EnviarMail(txtMail.Text, "Ha Recibido un Correo: " + "Repuestos pendientes", RenderHtml(), TipoCorreo.HTML, "");
                        Utils.MostrarAlerta(Response, "Email con Reporte ha sido enviado correctamente a: " + txtMail.Text);
                    }
                    catch (Exception z)
                    {
                        Utils.MostrarAlerta(Response, "Ocurrió un problema con el envio de correo");
                    }
                }
            }
            try
            {
                lbMail.Visible = fldMail.Visible = txtMail.Visible = plhEnvioMail.Visible = false;
                ddlMecanicos.Enabled = true;
                btnActualizar.Visible = false;
                btnValidarPass.Visible = true;
                dgOperaciones.Visible = false;
                dgOperaciones.DataSource = null;
                dgOperaciones.DataBind();

                DBFunctions.Transaction(sql);
                if (sql.Count > 0)
                    Utils.MostrarAlerta(Response, "La actualización se realizó exitosamente");
                else
                    Utils.MostrarAlerta(Response, "NO se han modificado operaciones para actualizar ...");
            }
            catch (Exception ex)
            {
                error.Text = ex.Message;
            }
        }

        //convertir plh para enviar por mail
        protected string RenderHtml()
        {
            StringBuilder SB = new StringBuilder();
            StringWriter SW = new StringWriter(SB);
            HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
            plhEnvioMail.RenderControl(htmlTW);
            
            return SB.ToString();
        }

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
		#endregion
	}
}
