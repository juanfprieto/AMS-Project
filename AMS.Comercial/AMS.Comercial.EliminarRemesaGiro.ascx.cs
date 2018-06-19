namespace AMS.Comercial
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
    using AMS.DB;
    using AMS.Forms;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de EliminarRemesaGiro.
	/// </summary>
	public class EliminarRemesaGiro : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlTipoAsociar;
		protected System.Web.UI.WebControls.Label Label35;
		protected System.Web.UI.WebControls.Panel pnlPlanilla;
		protected System.Web.UI.WebControls.Panel pnlTipoAsociar;
		protected System.Web.UI.WebControls.Panel pnlCrear;
        protected System.Web.UI.WebControls.Panel pnlConsulta;
		protected System.Web.UI.WebControls.Button btnEliminar;
		protected System.Web.UI.WebControls.TextBox txtNumeroDocumento;
		protected System.Web.UI.WebControls.Label Label8;
        protected System.Web.UI.WebControls.DataGrid dgrVentas;
	
		private void Page_Load(object sender, System.EventArgs e)
        {

            btnEliminar.Enabled = false;
            string nitResponsable = DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='" + HttpContext.Current.User.Identity.Name.ToString().ToLower() + "';");
            Agencias.TraerAgenciasUsuario(ddlAgencia);
           
            
			// Introducir aquí el código de usuario para inicializar la página
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
			this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            this.ddlTipoAsociar.SelectedIndexChanged +=new System.EventHandler(this.ddlTipoAsociar_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);
            this.txtNumeroDocumento.TextChanged += new System.EventHandler(this.txtNumeroDocumento_TextChanged);
            this.ddlAgencia.SelectedIndexChanged += new System.EventHandler(this.ddlAgencia_SelectedIndexChanged);
		}
		#endregion


        private void ddlAgencia_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            
        //    txtNumeroDocumento.Text = "";
        }
		

		//Cambia el tipo
		private void ddlTipoAsociar_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string FechaHoy  = DateTime.Now.ToString("yyyy-MM-dd");
            txtNumeroDocumento.Text = "";
			string tipo=ddlTipoAsociar.SelectedValue;
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			DatasToControls bind=new DatasToControls();
            DataSet dsMovimiento = new DataSet();   
            			
			//
		
	


			
		}
        private void txtNumeroDocumento_TextChanged(object sender, System.EventArgs e)
        {
            string responsable;
            string agencia = ddlAgencia.SelectedValue;
            string tipo = ddlTipoAsociar.SelectedValue;
            string documento = txtNumeroDocumento.Text;
            DatasToControls bind = new DatasToControls();
            DataSet dsMovimiento = new DataSet();



            //Valida que no este vacio el campo
            if (documento.Length == 0)
            {
                return;
            }

            if (tipo.Equals("Giros"))
            {//validar que si corresponda el documento.
                if (DBFunctions.RecordExist("select num_documento from dbxschema.mgiros where fecha_recibe='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and num_documento=" + documento + " and mag_agencia_origen=" + agencia + ";"))
                {
                    responsable = " ";
                    DBFunctions.Request(dsMovimiento, IncludeSchema.NO, "select TDOC_CODIGO, NUM_DOCUMENTO, MAG_AGENCIA_ORIGEN, "+
                   " MNIT_RESPONSABLE_RECIBE, MAG_AGENCIA_DESTINO,  MPLA_CODIGO, MNIT_EMISOR,"+
                   " COSTO_GIRO, VALOR_GIRO, FECHA_RECIBE,TEST_CODIGO from dbxschema.mgiros where fecha_recibe='"+DateTime.Now.ToString("yyyy-MM-dd")+"' and test_codigo<>'C' and num_documento=" + documento + " and mag_agencia_origen=" + agencia + ";");
                    dgrVentas.DataSource = dsMovimiento.Tables[0].DefaultView;
                    dgrVentas.DataBind();
                    btnEliminar.Enabled = true;

                }
                else
                {
                    Utils.MostrarAlerta(Response, "No se encontro Informacion con ese Documento");
                    btnEliminar.Enabled = false;
                }
            }
            if (tipo.Equals("Remesas"))
            {
                if (DBFunctions.RecordExist("select num_documento from dbxschema.mencomiendas where fecha_entrega is NULL and num_documento=" + documento + " and mag_recibe=" + agencia + ";"))
                {
                    responsable = " ";
                    DBFunctions.Request(dsMovimiento, IncludeSchema.NO, "select TDOC_CODIGO, NUM_DOCUMENTO, MAG_recibe, " +
                   " MNIT_RESPONSABLE_RECIBE, MAG_entrega,  MPLA_CODIGO, MNIT_EMISOR," +
                   " VALOR_total, FECHA_RECIBE,TEST_codigo from dbxschema.mencomiendas where mcat_placa is NULL and test_codigo<>'C' and num_documento=" + documento + " and mag_recibe=" + agencia + ";");
                    dgrVentas.DataSource = dsMovimiento.Tables[0].DefaultView;
                    dgrVentas.DataBind();
                    btnEliminar.Enabled = true;

                }
                else
                {
                    Utils.MostrarAlerta(Response, "No se encontro Informacion con ese Documento");
                    btnEliminar.Enabled = false;
                }
            }

            
        }
		private void btnEliminar_Click(object sender, System.EventArgs e)
		{

			//Actualizar papeleria
		/*	
            
            sqlStrings.Add("DELETE FROM DBXSCHEMA.MCONTROL_PAPELERIA  WHERE NUM_DOCUMENTO="+numDocumento+";");
			//Insertar giro
			sqlStrings.Add("DELETE FROM DBXSCHEMA.MGIROS WHERE NUM_DOCUMENTO="+numDocumento+";");

			//Actualizar planilla->num linea si es Real
			if ((tipo.Equals("M") || tipo.Equals("V")) && !planilla.Equals("NULL"))
				sqlStrings.Add("UPDATE DBXSCHEMA.MPLANILLAVIAJE SET NUMERO_LINEAS=NUMERO_LINEAS+1 WHERE MPLA_CODIGO="+planilla+";");
				
			if(DBFunctions.Transaction(sqlStrings))
			{
				lblNumDocumento.Text=numDocumento;
				pnlImprimir.Visible=true;
				pnlCrear.Visible=false;
				ViewState["Giro"]=numDocumento;
			}
			else
				lblError.Text += "Error: " + DBFunctions.exceptions + "<br><br>";
		
             */
          }
           
		
    }
	
		 
}
