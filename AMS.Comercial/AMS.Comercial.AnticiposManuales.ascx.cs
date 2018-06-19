namespace AMS.Comercial 
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Microsoft.Web.UI.WebControls;
	using AMS.DB;
	using AMS.Forms;
	using Ajax;
	using AMS.DBManager;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Comercial_AnticiposManuales.
	/// </summary>
	public class AMS_Comercial_AnticiposManuales : System.Web.UI.UserControl
	{
		#region Controles

		protected System.Web.UI.WebControls.Label Label23;
		protected System.Web.UI.WebControls.TextBox txtNIT;
		protected System.Web.UI.WebControls.Label Label24;
		protected System.Web.UI.WebControls.Label Label25;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.TextBox txtDescripcion;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.TextBox txtCantidad;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.Label Label16;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.Panel pnlDatos;
		protected System.Web.UI.WebControls.TextBox txtNITRresponsable;
		protected System.Web.UI.WebControls.TextBox txtNITa;
		protected System.Web.UI.WebControls.TextBox txtNITb;
		protected System.Web.UI.WebControls.TextBox txtValorUnidad;
		protected System.Web.UI.WebControls.TextBox txtValorTotal;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label lblDescripcion;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label lblCantidad;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label lblValorUnidad;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label lblValorTotal;
		protected System.Web.UI.WebControls.Button btnRegistrar;
		protected System.Web.UI.WebControls.Panel pnlConfirma;
		protected System.Web.UI.WebControls.Button btnAtras;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label lblNIT;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label lblPlaca;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.TextBox txtFecha;
		protected System.Web.UI.WebControls.Label Label17;
		protected System.Web.UI.WebControls.Label lblFecha;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.TextBox txtNumDocReferencia;
		protected System.Web.UI.WebControls.Label Label19;
		protected System.Web.UI.WebControls.Label lblNumDocumentoRef;
		protected System.Web.UI.WebControls.Label Label21;
		protected System.Web.UI.WebControls.Label lblNumDocumento;
		protected System.Web.UI.WebControls.TextBox txtResponsable;
		protected System.Web.UI.WebControls.Panel pnlPlanilla;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.DropDownList ddlAgenciaPapeleria;
		protected System.Web.UI.WebControls.Label Label22;
		protected System.Web.UI.WebControls.DropDownList ddlPersonalAgencia;
        protected System.Web.UI.WebControls.DropDownList ddlTipoAsociar;
		protected System.Web.UI.WebControls.Label Label20;
		protected System.Web.UI.WebControls.TextBox TextNumero;
		protected System.Web.UI.WebControls.Label Label12;
        protected System.Web.UI.WebControls.TextBox txtPlanilla;
		//protected System.Web.UI.WebControls.DropDownList ddlPlanilla;
		//protected System.Web.UI.WebControls.Panel pnlConcepto;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.TextBox txtPlaca;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlConcepto;
		string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
            
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Comercial_AnticiposManuales));
		
            if (!IsPostBack)
			{
				DatasToControls bind=  new DatasToControls();
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				ddlAgencia.Items.Insert(0,new ListItem("--Seleccione--","0"));
				Agencias.TraerAgencias(ddlAgenciaPapeleria);

				ddlAgenciaPapeleria.Items.Insert(0,new ListItem("--Agencia Papeleria Anticipo--",""));
				
				//btnGuardar.Attributes.Add("onClick", "return(validarGasto());");
				
				txtFecha.Text=DateTime.Now.ToString("yyyy-MM-dd");
						
				txtCantidad.Attributes.Add("onkeyup","NumericMask(this);Totales();");
				txtValorUnidad.Attributes.Add("onkeyup","NumericMask(this);Totales();");
                txtNIT.Attributes.Add("onKeyDown", "KeyDownHandlerNIT();if(event.keyCode==13)return(false);");
			}
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
			this.ddlAgencia.SelectedIndexChanged += new System.EventHandler(this.ddlAgencia_SelectedIndexChanged);
            this.ddlTipoAsociar.SelectedIndexChanged += new System.EventHandler(this.ddlTipoAsociar_SelectedIndexChanged);
            //this.ddlAgenciaPapeleria.SelectedIndexChanged += new System.EventHandler(this.ddlAgenciaPapeleria_SelectedIndexChanged);
			this.ddlPersonalAgencia.SelectedIndexChanged += new System.EventHandler(this.ddlPersonalAgencia_SelectedIndexChanged);
			//this.ddlPlanilla.SelectedIndexChanged += new System.EventHandler(this.ddlPlanilla_SelectedIndexChanged);
			this.ddlConcepto.SelectedIndexChanged += new System.EventHandler(this.ddlConcepto_SelectedIndexChanged);
			this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
			this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);
			this.btnAtras.Click += new System.EventHandler(this.btnAtras_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		//CAMBIA LA AGENCIA
		private void ddlAgencia_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			pnlDatos.Visible=false;
			pnlPlanilla.Visible=true;
			//pnlConcepto.Visible=true;
			//ddlConcepto.SelectedIndex=0;
            pnlDatos.Visible = false;
            txtPlaca.Enabled = true;

            if(ddlTipoAsociar.SelectedValue.Equals("A"))
            {
                Label10.Text = "Agencia :";
                txtPlaca.Text = ddlAgencia.SelectedValue;
                txtPlaca.Enabled = false;
            }
            else if (ddlTipoAsociar.SelectedValue.Equals("B"))
            {
                Label10.Text = "Placa del Bus :";
                txtPlaca.Text = "";
            }
            else if (ddlTipoAsociar.SelectedValue.Equals("C"))
            {
                txtPlaca.Text = DBFunctions.SingleData("SELECT * FROM palmacen WHERE palm_descripcion='ADMINISTRACION';");
                txtPlaca.Enabled = false;
            }

			int agencia=Convert.ToInt16(ddlAgencia.SelectedValue);
			Agencias.TraerPersonalAgencia(ddlPersonalAgencia,agencia);
			ddlPersonalAgencia.Items.Insert(0,new ListItem("--Seleccione--",""));
			
		}

        //CAMBIO TIPO DE ANTICIPO
        private void ddlTipoAsociar_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            txtPlaca.Enabled = true;

            if (ddlTipoAsociar.SelectedValue.Equals("A"))
            {
                Label10.Text = "Agencia :";
                txtPlaca.Text = ddlAgencia.SelectedValue;
                txtPlaca.Enabled = false;
            }
            else if (ddlTipoAsociar.SelectedValue.Equals("B"))
            {
                Label10.Text = "Placa del Bus :";
                txtPlaca.Text = "";
            }
            else if (ddlTipoAsociar.SelectedValue.Equals("C"))
            {
                txtPlaca.Text = DBFunctions.SingleData("SELECT * FROM palmacen WHERE palm_descripcion='ADMINISTRACION';");
                txtPlaca.Enabled = false;
            }

            if (!ddlPersonalAgencia.SelectedValue.Equals("0"))
            {
                ddlPersonalAgencia_SelectedIndexChanged(sender, e);
            }
        }

        //valida y completa datos en ddlTipoAsociar
        //....

		/*CAMBIA LA AGENCIA Papeleria
		private void ddlAgenciaPapeleria_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int agencia=Convert.ToInt16(ddlAgenciaPapeleria.SelectedValue);	
			
		}
		*/
		//SELECCIONA EL RESPONSABLE DEL ANTICIPO AGENCIA
		private void ddlPersonalAgencia_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			string agencia=ddlAgencia.SelectedValue; 
			string nitResponsable=ddlPersonalAgencia.SelectedValue;
            string aplica = "";

			string CargoUsuario =DBFunctions.SingleData("select pcar_codigo from DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES where mag_codigo="+ agencia+"  and mnit_nit='"+nitResponsable+"';");
			
			if(CargoUsuario.Length==0)
			{
				CargoUsuario = "0";
			}
			//Planillas del usuario
			//ddlPlanilla.Items.Clear();
            //bind.PutDatasIntoDropDownList(ddlPlanilla,
            //    "select mp.mpla_codigo, mp.mpla_codigo "+
            //    "from dbxschema.mplanillaviaje mp "+
            //    "where mp.mag_codigo="+ddlAgencia.SelectedValue+" and mp.fecha_liquidacion is null;");
            //    //"where mp.mag_codigo="+ddlAgencia.SelectedValue+" and mp.fecha_liquidacion is null and mnit_responsable='"+nitResponsable+"';");
			//ddlPlanilla.Items.Insert(0,new ListItem("---no definida---",""));				
			//Conceptos
            
            if (ddlTipoAsociar.SelectedValue.Equals("A"))
            {
                aplica = "APLICA_AGENCIA";
            }
            else if (ddlTipoAsociar.SelectedValue.Equals("B"))
            {
                aplica = "APLICA_BUS";
            }
            else if (ddlTipoAsociar.SelectedValue.Equals("C"))
            {
                aplica = "APLICA_COOPERATIVA";
            }

            bind.PutDatasIntoDropDownList(ddlConcepto,
                    "SELECT tct.tcon_codigo, tct.nombre " +
                    "FROM DBXSCHEMA.MAUTORIZACION_GASTO_TRANSPORTE mag, DBXSCHEMA.TCONCEPTOS_TRANSPORTES tct " +
                    "WHERE mag.mag_codigo=" + ddlAgencia.SelectedValue + "  AND mag.pcar_codigo=" + CargoUsuario +
                    " AND mag.tcon_codigo=tct.tcon_codigo AND " + aplica + "='S' AND tct.tdoc_codigo='ANT';");

            ddlConcepto.Items.Insert(0,new ListItem("---seleccione---",""));
		}

        [Ajax.AjaxMethod]
        public String Planilla_Changed(string planilla)
		{
            //txtPlaca.Text=null;
            //if(ddlPlanilla.SelectedValue.Length==0)return;
            string placa =DBFunctions.SingleData(
                "SELECT mv.mcat_placa from dbxschema.mplanillaviaje mp, dbxschema.mviaje mv "+
                "where mv.mviaje_numero=mp.mviaje_numero and mp.mrut_codigo= mv.mrut_codigo and mp.mpla_codigo=" + planilla);
			//pnlConcepto.Visible=true;
            return placa;
		}

        //CAMBIO DE CONCEPTO
		private void ddlConcepto_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			pnlDatos.Visible=false;
            Boolean agen = false;

			if(ddlConcepto.SelectedValue.Length==0)return;
			pnlDatos.Visible=true;
			//txtCantidad.Text=Convert.ToDouble(DBFunctions.SingleData("SELECT cantidad_consumo FROM DBXSCHEMA.TGASTOS_TRANSPORTES WHERE TCON_CODIGO="+ddlConcepto.SelectedValue)).ToString("###,###,##0.##");
            txtNIT.Enabled = false;
            if (ddlTipoAsociar.SelectedValue.Equals("A"))
            {
                txtNIT.Text = DBFunctions.SingleData("SELECT mnit_encargado FROM DBXSCHEMA.MAGENCIA WHERE mag_codigo =" + ddlAgencia.SelectedValue + ";");
            }
            else if (ddlTipoAsociar.SelectedValue.Equals("B"))
            {
                txtNIT.Text = DBFunctions.SingleData("SELECT mnit_asociado FROM DBXSCHEMA.MBUSAFILIADO WHERE mcat_placa='" + txtPlaca.Text + "';");
            }
            else if (ddlTipoAsociar.SelectedValue.Equals("C"))
            {
                txtNIT.Text = "";
                txtNITa.Text = "";
                txtNITb.Text = "";
                agen = true;
                txtNIT.Enabled = true;
            }

            if (agen == false)
            {
                DataSet dsReceptor = new DataSet();
                if (txtNIT.Text == "")
                    txtNIT.Text = "1000000000"; //nit provisional, cuando no hay placa...

                    DBFunctions.Request(dsReceptor, IncludeSchema.NO,
                    "SELECT mnit_nombres CONCAT ' ' CONCAT mnit_nombre2 AS nombre, mnit_apellidos CONCAT ' ' CONCAT mnit_apellido2 AS apellido " +
                    "FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='" + txtNIT.Text + "';");

                    txtNITa.Text = dsReceptor.Tables[0].Rows[0]["NOMBRE"].ToString();
                    txtNITb.Text = dsReceptor.Tables[0].Rows[0]["APELLIDO"].ToString();

            }

            txtValorUnidad.Text=Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(valor_unidad,0) FROM DBXSCHEMA.TGASTOS_TRANSPORTES WHERE TCON_CODIGO="+ddlConcepto.SelectedValue)).ToString("###,###,##0.##");
			//txtValorTotal.Text=Convert.ToDouble(DBFunctions.SingleData("SELECT valor_total FROM DBXSCHEMA.TGASTOS_TRANSPORTES WHERE TCON_CODIGO="+ddlConcepto.SelectedValue)).ToString("###,###,##0.##");
		}

		private void btnGuardar_Click(object sender, System.EventArgs e)
		{
			string MensajesError = "";
			bool HayErrores = false;
			string agenciaPapeleria;
			string numDocumento = TextNumero.Text;
			string agencia=ddlAgencia.SelectedValue;
			
			double ValotTotal = 0;
			string nitResponsable=ddlPersonalAgencia.SelectedValue; 
			DateTime fecha;

			if(ddlAgenciaPapeleria.SelectedValue.Length>0)
				agenciaPapeleria=ddlAgenciaPapeleria.SelectedValue;
			else
				agenciaPapeleria=ddlAgencia.SelectedValue;
			//Verificar papeleria
			if(!DBFunctions.RecordExist("SELECT NUM_DOCUMENTO FROM DBXSCHEMA.MCONTROL_PAPELERIA WHERE TDOC_CODIGO='ANT' AND FECHA_ANULACION IS NULL AND FECHA_USO IS NULL AND TIPO_DOCUMENTO='M'  AND MAG_CODIGO="+agenciaPapeleria+" AND NUM_DOCUMENTO="+numDocumento+" ORDER BY NUM_DOCUMENTO FETCH FIRST 1 ROWS ONLY"))
			{
				MensajesError += "La papeleria del anticipo/servicio no esta disponible para la Agencia: "+agenciaPapeleria+" ";
				MensajesError +="\\r\\n";
				HayErrores=true;
			}	
			/*Responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			
			if(nitResponsable.Length==0)
			{
				MensajesError += "El usuario actual (responsable) no tiene un NIT asociado. ";
				HayErrores=true;
			}
			*/
			//Concepto
			if(ddlConcepto.SelectedValue.Length==0)
			{
				MensajesError += "Debe seleccionar el concepto. "+"\\r\\n";
				HayErrores=true;
			}
			//Verificar placa
			if(txtPlaca.Text.Trim().Length>0)
			{
                if (ddlTipoAsociar.SelectedValue.Equals("B"))
                {
                    if (!DBFunctions.RecordExist("select mcat_placa from dbxschema.mbusafiliado where mcat_placa='" + txtPlaca.Text + "' AND TESTA_CODIGO NOT IN (-1,0,3);"))
                    {
                        MensajesError += "La placa registrada no existe.";
                        MensajesError += "\\r\\n";
                        HayErrores = true;
                    }
                }
			}
			else
				txtPlaca.Text = null;
			//Validaciones
			
			if(txtNIT.Text.Trim().Length==0)
			{
				MensajesError += "Debe digitar los datos del receptor. ";
				MensajesError +="\\r\\n";
				HayErrores=true;
			}
			if(txtDescripcion.Text.Trim().Length==0)
			{
				MensajesError += "Debe digitar la descripción. ";
				MensajesError +="\\r\\n";
				HayErrores=true;
			}
			try
			{
				if(double.Parse(txtCantidad.Text)<0)throw(new Exception());
			}
			catch
			{
				MensajesError += "La cantidad no es válida. "+"\\r\\n";
				HayErrores=true;
			}
			try
			{
				if(double.Parse(txtValorUnidad.Text)<0)throw(new Exception());
			}
			catch
				{
					MensajesError += "El valor de la unidad no es válido. "+"\\r\\n";
					HayErrores=true;
				}
				
			try
			{
				if(double.Parse(txtValorTotal.Text)<0)throw(new Exception());
				 ValotTotal = double.Parse(txtValorTotal.Text.Replace(",","")); 
			}
			catch
			{
				MensajesError += "El valor total no es válido. "+"\\r\\n";
				HayErrores=true;
			}
			fecha=Convert.ToDateTime(txtFecha.Text);
			try
			{
				fecha=Convert.ToDateTime(txtFecha.Text);
			}
			catch
			{
				MensajesError += "Fecha no válida.. "+"\\r\\n";
				HayErrores=true;
			}
				
			//Verifica cierre mensual y diario
			
			int anio = fecha.Year;
			int mes  = fecha.Month;
			int periodo = anio * 100 + mes;
            
			string estado=DBFunctions.SingleData("select estado_cierre from DBXSCHEMA.periodos_cierre_transporte where numero_periodo="+periodo+";");
			if(estado.Length==0 || estado=="C")
			{
				MensajesError += "El periodo de la fecha esta cerrado o NO Existe. "+"\\r\\n";
				HayErrores=true;
			}
			// Si existe es porque ya se contabilizo para la Agencia_dia
			if (DBFunctions.RecordExist("select MAG_CODIGO from DBXSCHEMA.DCIERRE_DIARIO_AGENCIA where MAG_CODIGO =  "+agencia+" and FECHA_CONTABILIZACION = '"+fecha.ToString("yyyy-MM-dd")+"';"))
			{
				MensajesError += "La Agencia-fecha ya se Contabilizo"+"\\r\\n";
				HayErrores=true;
			}
			string CargoUsuario =DBFunctions.SingleData("select pcar_codigo from DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES where mag_codigo="+ddlAgencia.SelectedValue+"  and mnit_nit='"+nitResponsable+"';");
			//double ValotTotal = double.Parse(txtValorTotal.Text.Replace(",","")); 
			string valorAutorizado = DBFunctions.SingleData("SELECT coalesce(valor_maximo_autorizacion,0) from DBXSCHEMA.MAUTORIZACION_GASTO_TRANSPORTE " +
					                              "WHERE mag_codigo="+ddlAgencia.SelectedValue+"  and pcar_codigo="+CargoUsuario+"  and  TCON_CODIGO = "+ddlConcepto.SelectedValue+";");
			if(valorAutorizado.Length==0)
			{
				MensajesError += "El Cargo del empleado no tiene autorizacion para el anticipo. "+"\\r\\n";
				HayErrores=true;
			}
			else 
			{
				double ValorMaximo = Convert.ToDouble(valorAutorizado);
				//double ValorMaximo = Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(valor_maximo_autorizacion,0) from DBXSCHEMA.MAUTORIZACION_GASTO_TRANSPORTE " +
				//		                              "WHERE mag_codigo="+ddlAgencia.SelectedValue+"  and pcar_codigo="+CargoUsuario+"  and  TCON_CODIGO = "+ddlConcepto.SelectedValue+";"));	
				if(ValotTotal > ValorMaximo) 
				{
					MensajesError += "El valor del ANT. es mayor al maximo autorizado:$"+ValorMaximo.ToString("###,###,###")+""+"\\r\\n";
					HayErrores=true;
				}
			}
		    //if(MensajesError.Length==0)
			if (HayErrores)
			{
				//strActScript+="alert('"+MensajesError+"');"
                Utils.MostrarAlerta(Response, "" + MensajesError + "");
				return;
			}
			lblFecha.Text=txtFecha.Text;
			lblPlaca.Text=txtPlaca.Text;
			lblDescripcion.Text=txtDescripcion.Text.Trim();
			lblCantidad.Text=txtCantidad.Text;
			lblValorUnidad.Text=txtValorUnidad.Text;
			lblValorTotal.Text=txtValorTotal.Text;
			lblNIT.Text=txtNIT.Text+"<br>"+txtNITa.Text+" "+txtNITb.Text;
			//lblNumDocumento.Text=Anticipos.TraerSiguienteAnticipoVirtual();
			lblNumDocumentoRef.Text=txtNumDocReferencia.Text;
			lblNumDocumento.Text = TextNumero.Text;
			pnlConfirma.Visible=true;
			pnlDatos.Visible=false;
			ddlAgencia.Enabled=false;
			ddlAgenciaPapeleria.Enabled=false;
			ddlPersonalAgencia.Enabled=false;
            txtPlanilla.Enabled = false;
            //ddlPlanilla.Enabled=false;
			ddlConcepto.Enabled=false;
			txtPlaca.Enabled=false;
			TextNumero.Enabled=false;
		}

		private void btnAtras_Click(object sender, System.EventArgs e)
		{
			pnlConfirma.Visible=false;
			txtPlaca.Enabled=true;
			TextNumero.Enabled=true;
			pnlDatos.Visible=true;
			ddlAgencia.Enabled=true;
			ddlAgenciaPapeleria.Enabled=true;
			ddlPersonalAgencia.Enabled=true;
            txtPlanilla.Enabled = true;
			//ddlPlanilla.Enabled=true;
			ddlConcepto.Enabled=true;
		}

		private void btnRegistrar_Click(object sender, System.EventArgs e)
		{
			//Guardar
			long  numDocumento = Convert.ToInt32(TextNumero.Text);
			string agencia=ddlAgencia.SelectedValue;
			string nitReceptor=txtNIT.Text.Trim();
			string nitResponsable=ddlPersonalAgencia.SelectedValue; 
			//string planilla=ddlPlanilla.SelectedValue;
            string planilla = txtPlanilla.Text;
			/*
			//Responsable
			//string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(nitResponsable.Length==0)
			{
				Response.Write("<script language='javascript'>alert('El usuario actual (responsable) no tiene un NIT asociado.');</script>");
				return;
			}
			*/			
			string numLineaS;
			string tipoUnidad;
			
			string docReferencia=txtNumDocReferencia.Text.Trim();
			
			if(ddlAgenciaPapeleria.SelectedValue.Length>0)
				docReferencia += " AgPapel:" + ddlAgenciaPapeleria.SelectedValue;
			docReferencia=(docReferencia.Length==0)?"NULL":"'"+docReferencia+"'";
			string placa=lblPlaca.Text.Trim();
			if(placa.Length==0)
			   placa="NULL";
			else 
               placa="'"+placa+"'";
			if(planilla.Length==0){
				planilla="NULL";
				numLineaS="NULL";}
			else
				numLineaS=DBFunctions.SingleData("SELECT NUMERO_LINEAS+1 FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+planilla);
			tipoUnidad=DBFunctions.SingleData("SELECT tund_consumo FROM DBXSCHEMA.TGASTOS_TRANSPORTES WHERE TCON_CODIGO="+ddlConcepto.SelectedValue);
			ArrayList sqlStrings = new ArrayList();

			//Actualizar papeleria
			sqlStrings.Add("UPDATE MCONTROL_PAPELERIA SET FECHA_USO='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',MPLA_CODIGO="+planilla+" ,MNIT_RESPONSABLE = '"+nitResponsable+"' WHERE TDOC_CODIGO='ANT' AND NUM_DOCUMENTO="+numDocumento+";");
			//Insertar servicio/anticipo
			sqlStrings.Add("INSERT INTO DBXSCHEMA.MGASTOS_TRANSPORTES VALUES('ANT',"+numDocumento+",'V','"+lblFecha.Text+"',"+placa+","+planilla+","+numLineaS+","+ddlConcepto.SelectedValue+","+agencia+",'"+nitResponsable+"','"+nitReceptor+"','"+lblDescripcion.Text+"',"+lblCantidad.Text.Replace(",","")+",'"+tipoUnidad+"',"+lblValorUnidad.Text.Replace(",","")+","+lblValorTotal.Text.Replace(",","")+",'A',"+docReferencia+");");

			//Actualizar planilla->num linea si es Real
			if(!planilla.Equals("NULL"))
				sqlStrings.Add("UPDATE DBXSCHEMA.MPLANILLAVIAJE SET NUMERO_LINEAS=NUMERO_LINEAS+1 WHERE MPLA_CODIGO="+planilla+";");
			
			if(DBFunctions.Transaction(sqlStrings))
				Response.Redirect(indexPage+"?process=Comercial.AnticiposManuales&path="+Request.QueryString["path"]+"&act=1&ant="+numDocumento);
			else
				lblError.Text += "Error: " + DBFunctions.exceptions;
		}


        #region AJAX
        //Traer NIT
        [Ajax.AjaxMethod]
        public string TraaerNIT(string NIT)
        {
            DataSet dsNIT = new DataSet();
            string nombre = "", apellido = "";
            DBFunctions.Request(dsNIT, IncludeSchema.NO, "select mnit_nombres, mnit_apellidos FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='" + NIT + "';");
            if (dsNIT.Tables[0].Rows.Count > 0)
            {
                nombre = dsNIT.Tables[0].Rows[0][0].ToString();
                apellido = dsNIT.Tables[0].Rows[0][1].ToString();
             }
            return (nombre + "|" + apellido);
        }
                #endregion AJAX



	}
}