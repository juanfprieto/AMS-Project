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
	///		Descripción breve de AMS_Comercial_PlanillaChequeo.
	/// </summary>
	public class AMS_Comercial_PlanillaChequeo : System.Web.UI.UserControl
	{
		
		#region Controles
		string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.TextBox txtPlanilla;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.TextBox txtInspector;
		protected System.Web.UI.WebControls.TextBox txtInspectora;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.DropDownList ddlAgenciaChequeada;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlRuta;
        protected System.Web.UI.WebControls.RadioButtonList chkpago;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.TextBox txtNumeroBus;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.TextBox txtNumeroBusa;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.TextBox txtFecha;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.DropDownList ddlHora;
		protected System.Web.UI.WebControls.DropDownList ddlMinuto;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.TextBox txtConductor;
		protected System.Web.UI.WebControls.TextBox txtConductora;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.TextBox txtNumTiquete;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.TextBox txtNumSinTiquete;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.TextBox txtNumTotal;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.TextBox txtLugar;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.DataGrid dgrDocumentos;
		public string strActScript;
		protected System.Web.UI.WebControls.Panel pnlDestinos;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.TextBox txtTotalPre;
        protected System.Web.UI.WebControls.CheckBox chkTransbordo;
        protected System.Web.UI.WebControls.TextBox txtbusRecibe;
        protected System.Web.UI.WebControls.CheckBox chkseguridadSocial;
        protected System.Web.UI.WebControls.TextBox txtvalorTransbordo;
        protected System.Web.UI.WebControls.Label lblTransbordo, lblvalorTransbordo;
        protected System.Web.UI.WebControls.TextBox txtObservaciones;
      
        public string strTotal, cargo;
		#endregion Controles

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Comercial_PlanillaChequeo));
			if (!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				//Agencias
				DataRow drC;
				DataSet dsAgencias=new DataSet();
				DBFunctions.Request(dsAgencias,IncludeSchema.NO,
					"select rtrim(char(mag_codigo)) as valor,mage_nombre as texto from DBXSCHEMA.MAGENCIA where chequeo='S' ORDER BY mage_nombre;");
				drC=dsAgencias.Tables[0].NewRow();
				drC[0]="";
				drC[1]="--seleccione--";
				dsAgencias.Tables[0].Rows.InsertAt(drC,0);
				ddlAgencia.DataSource=dsAgencias.Tables[0];
				ddlAgencia.DataTextField="texto";
				ddlAgencia.DataValueField="valor";
				ddlAgencia.DataBind();
				bind.PutDatasIntoDropDownList(ddlRuta,"select mrut_codigo,mrut_codigo concat '(' concat mrut_descripcion concat ')' from DBXSCHEMA.mrutas WHERE mrut_clase=2 order by mrut_codigo;");
				ListItem itm=new ListItem("---Ruta---","");
				ddlRuta.Items.Insert(0,itm);
              //  bind.PutDatasIntoDropDownList(ddlAgenciaChequeada, "");
			
				for(int i=0;i<24;i++)
					ddlHora.Items.Add(new ListItem(i.ToString("00"),i.ToString()));
				for(int i=0;i<60;i++)
					ddlMinuto.Items.Add(new ListItem(i.ToString("00"),i.ToString()));
	
				if(Request.QueryString["act"]!=null && Request.QueryString["pln"]!=null)
                Utils.MostrarAlerta(Response, "La planilla de chequeo ha sido registrada con el número");

             
                txtNumeroBus.Attributes.Add("onKeyDown", "KeyDownHandlerPlaca();if(event.keyCode==13)return(false);");
                txtConductor.Attributes.Add("onKeyDown", "KeyDownHandlerPlaca();if(event.keyCode==13)return(false);");
                lblTransbordo.Visible = false;
                lblvalorTransbordo.Visible = false;
                txtvalorTransbordo.Visible = false;
                txtbusRecibe.Visible = false;
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
        [Ajax.AjaxMethod]
		private void InitializeComponent()
		{
			this.ddlRuta.SelectedIndexChanged += new System.EventHandler(this.ddlRuta_SelectedIndexChanged);
      //      this.ddlAgenciaChequeada.SelectedIndexChanged += new System.EventHandler(this.ddlAgenciaChequeada_SelectedIndexChanged);

			this.dgrDocumentos.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrDocumentos_ItemDataBound);
			this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click1);
            this.txtConductor.TextChanged += new System.EventHandler(this.txtConductor_TextChanged);
            this.Load += new System.EventHandler(this.Page_Load);
            this.chkTransbordo.CheckedChanged += new System.EventHandler(this.chkTransbordo_CheckedChanged);
        }
		#endregion

		private void dgrDocumentos_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item ||e.Item.ItemType==ListItemType.AlternatingItem )
			{
				TextBox txtRuta=(TextBox)e.Item.FindControl("txtRuta");
				TextBox txtCantidad=(TextBox)e.Item.FindControl("txtCantidad");
				TextBox txtValor=(TextBox)e.Item.FindControl("txtValor");
				TextBox txtTotal=(TextBox)e.Item.FindControl("txtTotal");

				string objsScr="'"+txtCantidad.ClientID+"','"+txtValor.ClientID+"','"+txtTotal.ClientID+"'";
				txtCantidad.Attributes.Add("onkeyup","Total("+objsScr+");NumericMask(this)");
				txtValor.Attributes.Add("onkeyup","Total("+objsScr+");NumericMask(this)");
				strTotal+=txtTotal.ClientID+",";

			}
			if(e.Item.ItemType==ListItemType.Footer)
			{
				strTotal+=((TextBox)e.Item.FindControl("txtSumaTotal")).ClientID;
				ViewState["strTotal"]=strTotal;
			}
		}
        
		private void btnGuardar_Click1(object sender, System.EventArgs e)
		{
			long   numPlanilla=0;
            int    conTiquete, sinTiquete, totalTiquetes;
            double valorTotalPre, valorTransbordo;
            string inspector = txtInspector.Text, rutaP = ddlRuta.SelectedValue, conductor = txtConductor.Text, pago = chkpago.Text;
            int    agencia = Convert.ToInt32(ddlAgencia.SelectedValue), agenciaChq = Convert.ToInt32(ddlAgenciaChequeada.SelectedValue), horaChq = Convert.ToInt32(ddlHora.SelectedValue), minutoChq = Convert.ToInt32(ddlMinuto.SelectedValue);
            string placa, placaRecibe = txtbusRecibe.Text.Trim(), seguridadSocial, observaciones = txtObservaciones.Text;
           
			DateTime fechaChequeo;
			//Validaciones
			#region Generales
			//No. planilla
			try
			{
				numPlanilla=Convert.ToInt64(txtPlanilla.Text);}
			catch{
                Utils.MostrarAlerta(Response, "Debe dar el numero de planilla.");
				return;}
			//Responsable
			//if(!DBFunctions.RecordExist("SELECT MNIT.MNIT_NIT FROM DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MEMPLEADO ME WHERE ME.MNIT_NIT=MNIT.MNIT_NIT AND MNIT.MNIT_NIT='"+inspector+"' AND ME.PCAR_CODICARGO='IR'")){
			if(!DBFunctions.RecordExist("SELECT MNIT.MNIT_NIT FROM DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES PA WHERE PA.MNIT_NIT=MNIT.MNIT_NIT AND MNIT.MNIT_NIT='"+inspector+"' AND PA.PCAR_CODIGO = 2 AND PA.MAG_CODIGO = 9303 ")){
                Utils.MostrarAlerta(Response, "No se encontró el inspector.");
				return;}
			//Papeleria
			if(!DBFunctions.RecordExist("SELECT MNIT_RESPONSABLE FROM DBXSCHEMA.MCONTROL_PAPELERIA WHERE MNIT_RESPONSABLE='"+inspector+"' AND TDOC_CODIGO='PLACHQ' AND NUM_DOCUMENTO="+numPlanilla+" AND FECHA_USO IS NULL;")){
                Utils.MostrarAlerta(Response, "No se encontró la papelería para el inspector o ya se utilizó.");
				return;}
			//Ya existe?
			if(DBFunctions.RecordExist("SELECT MPLA_CODIGO FROM DBXSCHEMA.MPLANILLAVIAJE_CHEQUEO WHERE MPLA_CODIGO="+numPlanilla+";")){
                Utils.MostrarAlerta(Response, "Ya se registró la planilla.");
				return;}
			//Agencia
	
            //PENDIENTE DE COMO SE QUIERE TRABAJAR!!!!!!!
           /* if(agencia.Length==0){
				Response.Write("<script language='javascript'>alert('Debe seleccionar la agencia.');</script>");
				return;}
			
            */

            //opcion de pago
            if (pago.Length == 0)
            {
                Utils.MostrarAlerta(Response, "Debe seleccionar si se pago o no.");
                return;
            }
            //Ruta principal
			if(rutaP.Length==0){
                Utils.MostrarAlerta(Response, "Debe seleccionar la ruta principal.");
				return;}
			//Fecha
			if(txtFecha.Text.Length==0){
                Utils.MostrarAlerta(Response, "Debe dar una fecha de chequeo.");
				return;}
			try{
				fechaChequeo=Convert.ToDateTime(txtFecha.Text+" "+ddlHora.SelectedValue+":"+ddlMinuto.SelectedValue+":00");}
			catch{
                Utils.MostrarAlerta(Response, "Debe dar una fecha de chequeo válida.");
				return;}
			//Bus
			placa=DBFunctions.SingleData("SELECT MCAT_PLACA FROM DBXSCHEMA.MBUSAFILIADO WHERE MBUS_NUMERO="+txtNumeroBus.Text.Trim());
			if(placa.Length==0){
                Utils.MostrarAlerta(Response, "No se encontró el bus.");
				return;}
			//Conductor
			if(conductor.Length==0){
                Utils.MostrarAlerta(Response, "Debe dar el conductor.");
				return;}
			//No. Tiquetes
			try{
				conTiquete=Convert.ToInt16(txtNumTiquete.Text);
				if(conTiquete<0)throw(new Exception());}
			catch{
                Utils.MostrarAlerta(Response, "Número de pasajeros con tiquete no válido.");
				return;}
			try{
				sinTiquete=Convert.ToInt16(txtNumSinTiquete.Text);
				if(sinTiquete<0)throw(new Exception());}
			catch{
                Utils.MostrarAlerta(Response, "Número de pasajeros sin tiquete no válido.");
				return;}
			try{
				totalTiquetes=Convert.ToInt16(txtNumTotal.Text);
				if(totalTiquetes<0)throw(new Exception());}
			catch{
                Utils.MostrarAlerta(Response, "Número de pasajeros totales no válido.");
				return;}
			if(totalTiquetes!=sinTiquete+conTiquete){
                Utils.MostrarAlerta(Response, "Número de pasajeros totales no coincide con el número de pasajeros con y sin tiquete.");
				return;}

			//Valor total
			try
			{
				valorTotalPre=Convert.ToDouble(txtTotalPre.Text);
				if(valorTotalPre<0)throw(new Exception());}
			catch{
                Utils.MostrarAlerta(Response, "El valor total no es válido.");
				return;}

            //  transbordo
            if(chkTransbordo.Checked && txtbusRecibe.Text.Length == 0)
            {
                Utils.MostrarAlerta(Response, "Falta Placa del Bus que recibe el Transbordo.");
                return;
            }

            if (chkTransbordo.Checked && txtvalorTransbordo.Text.Length == 0)
            {
                Utils.MostrarAlerta(Response, "Falta el valor del Transbordo.");
                return;
            }
            else
            {
                if (txtvalorTransbordo.Text.Length == 0 || txtbusRecibe.Text.Length == 0)
                {
                    txtbusRecibe.Text = "";
                    
                    txtvalorTransbordo.Text = "0";
                }
                valorTransbordo = Convert.ToDouble(txtvalorTransbordo.Text);
            }

            if (chkTransbordo.Checked )
            {
                placaRecibe = DBFunctions.SingleData("SELECT MCAT_PLACA FROM DBXSCHEMA.MBUSAFILIADO WHERE MBUS_NUMERO=" + txtbusRecibe.Text.Trim());
                if (placaRecibe.Length == 0)
                {
                    Utils.MostrarAlerta(Response, "No se encontró el bus que Recibe Transbordo.");
                    return;
                }
            }
            else
            {
                placaRecibe = "";
            }

            // seguridad social auxiliar
            if (chkseguridadSocial.Checked == true)
                seguridadSocial = "S";
            else
                seguridadSocial = "N";
        
            if (DBFunctions.RecordExist("select mnit_nit from DBXSCHEMA.MRELEVADORES_TRANSPORTES where  mnit_nit='" + conductor + "'"))
            {
                Utils.MostrarAlerta(Response, "Es un Conductor Relevador");
                cargo = "R";
            }

            if (DBFunctions.RecordExist("select mnit_nit from DBXSCHEMA.MEMPLEADO where  mnit_nit='" + conductor + "' and test_estado=1 AND PCAR_CODICARGO='CO'"))
            {
                Utils.MostrarAlerta(Response, "Es un Conductor Titular");
                cargo = "T";
            }


			#endregion Generales
			#region   Tiquetes
			ArrayList arlRutas=new ArrayList();
			ArrayList sqlUpd=new ArrayList();
			string errores="";
			string rutaT;
			int    cantidadT=sinTiquete;
			double valorT=valorTotalPre;
            pago = pago + "" + cargo; 
           
			sqlUpd.Add("UPDATE DBXSCHEMA.MCONTROL_PAPELERIA SET FECHA_USO='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' WHERE TDOC_CODIGO='PLACHQ' AND NUM_DOCUMENTO="+numPlanilla+";");
            sqlUpd.Add("INSERT INTO DBXSCHEMA.MPLANILLAVIAJE_CHEQUEO VALUES(" + numPlanilla + "," + agencia + ",'" + rutaP + "','" + placa + "','" + fechaChequeo.ToString("yyyy-MM-dd") + "'," + horaChq + "," + minutoChq + "," + agenciaChq + ",'" + conductor + "','" + inspector + "'," + conTiquete + "," + sinTiquete + "," + valorTotalPre + ",'" + txtLugar.Text + "','" + pago + "','" + placaRecibe + "'," + valorTransbordo + ",'" + seguridadSocial + "','"+observaciones+"');");
			

            sqlUpd.Add("INSERT INTO DBXSCHEMA.DPLANILLAVIAJE_CHEQUEO VALUES(" + numPlanilla + ",'" + rutaP + "'," + cantidadT + "," + valorT + ");");
            
			#endregion Tiquetes

			
			if(errores.Length==0){
				if(DBFunctions.Transaction(sqlUpd))
					Response.Redirect(indexPage+"?process=Comercial.PlanillaChequeo&act=1&path="+Request.QueryString["path"]+"&pln="+numPlanilla);
				else
					lblError.Text=DBFunctions.exceptions;}
			else strActScript+="alert('"+errores+"');";
		}

        [Ajax.AjaxMethod]
        private void txtConductor_TextChanged(object sender, System.EventArgs e)
        {
            string conductor = txtConductor.Text;
            DataSet dsNIT = new DataSet();

            if (DBFunctions.RecordExist("select mnit_nit from DBXSCHEMA.MRELEVADORES_TRANSPORTES where  mnit_nit='" + conductor + "'"))
            {
                Utils.MostrarAlerta(Response, "Es un Conductor Relevador");
                DBFunctions.Request(dsNIT, IncludeSchema.NO, "select mnit_nombres, mnit_apellidos FROM DBXSCHEMA.MNIT M, DBXSCHEMA.MRELEVADORES_TRANSPORTES ME WHERE M.MNIT_NIT='" + conductor + "' AND  ME.MNIT_NIT=M.MNIT_NIT ;");
                txtConductora.Text = dsNIT.Tables[0].Rows[0][0].ToString() + " " + dsNIT.Tables[0].Rows[0][1].ToString();
                cargo = "R";
            }


            if (DBFunctions.RecordExist("select mnit_nit from DBXSCHEMA.MEMPLEADO where  mnit_nit='" + conductor + "' and test_estado=1 AND PCAR_CODICARGO='CO'"))
            {

                Utils.MostrarAlerta(Response, "Es un Conductor Titular");
                DBFunctions.Request(dsNIT, IncludeSchema.NO, "select mnit_nombres, mnit_apellidos FROM DBXSCHEMA.MNIT M, DBXSCHEMA.MEMPLEADO ME WHERE M.MNIT_NIT='" + conductor + "' AND  ME.MNIT_NIT=M.MNIT_NIT AND ME.PCAR_CODICARGO='CO' and test_estado=1;");
                txtConductora.Text = dsNIT.Tables[0].Rows[0][0].ToString() + " " + dsNIT.Tables[0].Rows[0][1].ToString();
                cargo = "T";
            }
            else if (!DBFunctions.RecordExist("select mnit_nit from DBXSCHEMA.MRELEVADORES_TRANSPORTES where  mnit_nit='" + conductor + "'") && !DBFunctions.RecordExist("select mnit_nit from DBXSCHEMA.MEMPLEADO where  mnit_nit='" + conductor + "' and test_estado=1 AND PCAR_CODICARGO='CO'"))
            Utils.MostrarAlerta(Response, "No se encontro registrado.");


            return;


        }

        [Ajax.AjaxMethod]
        private void chkTransbordo_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkTransbordo.Checked == true)
            {
                lblTransbordo.Visible = true;
                lblvalorTransbordo.Visible = true;
                txtvalorTransbordo.Visible = true;
                txtbusRecibe.Visible = true;
            }
            else
            {
                lblTransbordo.Visible = false;
                lblvalorTransbordo.Visible = false; 
                txtvalorTransbordo.Visible = false;
                txtbusRecibe.Visible = false;
            }
	
        }

        public void traeralgo() {
            string conductor = txtConductor.Text;
            DataSet dsNIT = new DataSet();

            if (DBFunctions.RecordExist("select mnit_nit from DBXSCHEMA.MRELEVADORES_TRANSPORTES where  mnit_nit='" + conductor + "'"))
            {
                Utils.MostrarAlerta(Response, "Es un Conductor Relevador");
                DBFunctions.Request(dsNIT, IncludeSchema.NO, "select mnit_nombres, mnit_apellidos FROM DBXSCHEMA.MNIT M, DBXSCHEMA.MRELEVADORES_TRANSPORTES ME WHERE M.MNIT_NIT='" + conductor + "' AND  ME.MNIT_NIT=M.MNIT_NIT ;");
                txtConductora.Text = dsNIT.Tables[0].Rows[0][0].ToString() + " " + dsNIT.Tables[0].Rows[0][1].ToString();
                cargo = "R";
            }


            if (DBFunctions.RecordExist("select mnit_nit from DBXSCHEMA.MEMPLEADO where  mnit_nit='" + conductor + "' and test_estado=1 AND PCAR_CODICARGO='CO'"))
            {

                Utils.MostrarAlerta(Response, "Es un Conductor Titular");
                DBFunctions.Request(dsNIT, IncludeSchema.NO, "select mnit_nombres, mnit_apellidos FROM DBXSCHEMA.MNIT M, DBXSCHEMA.MEMPLEADO ME WHERE M.MNIT_NIT='" + conductor + "' AND  ME.MNIT_NIT=M.MNIT_NIT AND ME.PCAR_CODICARGO='CO' and test_estado=1;");
                txtConductora.Text = dsNIT.Tables[0].Rows[0][0].ToString() + " " + dsNIT.Tables[0].Rows[0][1].ToString();
                cargo = "T";
            }


            else if (!DBFunctions.RecordExist("select mnit_nit from DBXSCHEMA.MRELEVADORES_TRANSPORTES where  mnit_nit='" + conductor + "'") && !DBFunctions.RecordExist("select mnit_nit from DBXSCHEMA.MEMPLEADO where  mnit_nit='" + conductor + "' and test_estado=1 AND PCAR_CODICARGO='CO'"))
            Utils.MostrarAlerta(Response, "No se encontro registrado.");


            return;
        }


        [Ajax.AjaxMethod]
        private void ddlAgenciaChequeada_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string slqAgencia = "SELECT DISTINCT MA.MAG_CODIGO AS CODIGO, MA.MAGE_NOMBRE AS DESCRIPCION " +
                                 " FROM DBXSCHEMA.MRUTAS MR, DBXSCHEMA.MRUTA_INTERMEDIA mi, MAGENCIA MA " +
                                 " where (mi.mruta_secundaria = mr.mrut_codigo and mi.mruta_principal='" + ddlRuta.SelectedValue + "' " +
                                 " AND MR.PCIU_COD = MA.PCIU_CODIGO) OR MA.MAGE_NOMBRE LIKE '%ADMIN%' ORDER BY MA.MAGE_NOMBRE; ";
            DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(ddlAgenciaChequeada, "slqAgencia");
            ListItem itm = new ListItem("--Agencia Chequeada","0000");
            ddlAgenciaChequeada.Items.Insert(0, itm);
        }

        [Ajax.AjaxMethod]	
         private void ddlRuta_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            string slqAgencia = "SELECT DISTINCT MA.MAG_CODIGO AS CODIGO, MA.MAGE_NOMBRE AS DESCRIPCION " +
                                " FROM DBXSCHEMA.MRUTAS MR, DBXSCHEMA.MRUTA_INTERMEDIA mi, MAGENCIA MA " +
                                " where (mi.mruta_secundaria = mr.mrut_codigo and mi.mruta_principal='" + ddlRuta.SelectedValue + "' " +
                                " AND MR.PCIU_COD = MA.PCIU_CODIGO) OR MA.MAGE_NOMBRE LIKE '%ADMIN%' ORDER BY MA.MAGE_NOMBRE; ";
            DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(ddlAgenciaChequeada, slqAgencia);
            ListItem itm = new ListItem("--Agencia Chequeada", "0000");
            ddlAgenciaChequeada.Items.Insert(0, itm);
            btnGuardar.Enabled = true;
		}
      
        #region AJAX
        
        //traer la placa
        [Ajax.AjaxMethod]
        public string TraerPlaca(string numero)
        {
            DataSet dsNIT = new DataSet();
            string placa = "";
            DBFunctions.Request(dsNIT, IncludeSchema.NO, "SELECT mbus_numero as numero,mcat_placa AS placa from DBXSCHEMA.mbusafiliado where testa_codigo>0 and mbus_numero=" + numero + ";");
            if (dsNIT.Tables[0].Rows.Count > 0)
            {
                placa = dsNIT.Tables[0].Rows[0][1].ToString();
               
            }
            return (placa);
        }

        public void TraerNit(string numero)
        {
            traeralgo();
            
        }


        #endregion AJAX

        



	}
}
