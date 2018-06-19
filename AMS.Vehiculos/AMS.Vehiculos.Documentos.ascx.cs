namespace AMS.Vehiculos
{
	using System;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Collections;
	using Ajax;
	using AMS.CriptoServiceProvider;
	using AMS.DB;
	using AMS.Forms;
    using AMS.Documentos;
    using AMS.Tools;


	/// <summary>
	///		Descripción breve de AMS_Vehiculos_Documentos.
	/// </summary>
	public partial class AMS_Vehiculos_Documentos : System.Web.UI.UserControl
	{
		protected bool reload=false;
		private FormatosDocumentos formatoRecibo =  new FormatosDocumentos();

		string txtFechaDocS,txtValoDocS,txtFechaVencDocS,txtObservaS,txtTramitadorS,txtNumDocS,ddlVinS,ddlCodDocS,ddlcatalogoS,ddlEntreCliS;
		string ConString;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS.Vehiculos.AMS_Vehiculos_Documentos));

            if (IsPostBack)
            {
                if (Request.Form[txtFechaDoc.UniqueID].ToString() == "" || Request.Form[txtFechaVencDoc.UniqueID].ToString() == "" || Request.Form[ddlEntreCli.UniqueID].ToString() == "ND")
                {
                    return;
                    
                }
                //CargarVin(ddlcatalogo.SelectedValue);
            } 
            
            txtFechaDocS = Convert.ToDateTime(Request.Form[txtFechaDoc.UniqueID]).ToString("yyyy-MM-dd");
            txtFechaVencDocS = Convert.ToDateTime(Request.Form[txtFechaVencDoc.UniqueID]).ToString("yyyy-MM-dd");
            txtValoDocS=Request.Form[txtValoDoc.UniqueID];
			txtTramitadorS=Request.Form[txtTramitador.UniqueID];
			txtNumDocS=Request.Form[txtNumDoc.UniqueID];
			txtObservaS=Request.Form[txtObserva.UniqueID];
			ddlVinS=Request.Form[ddlVin.UniqueID];
			ddlCodDocS=Request.Form[ddlCodDoc.UniqueID];
			ddlcatalogoS=Request.Form[ddlcatalogo.UniqueID];
			ddlEntreCliS=Request.Form[ddlEntreCli.UniqueID];
			
            if (!IsPostBack)
			{
                string sql = "SELECT DISTINCT PCAT_CODIGO \n" +
                     "FROM MCATALOGOVEHICULO MC  \n" +
                     "  INNER JOIN mvehiculo mv ON MC.MCAT_VIN = MV.MCAT_VIN";
                Utils.FillDll(ddlcatalogo, sql, false);

                sql = String.Format("SELECT MV.mcat_vin as VIN \n" +
                     "FROM MCATALOGOVEHICULO MC  \n" +
                     "  INNER JOIN mvehiculo mv ON MC.MCAT_VIN = MV.MCAT_VIN \n" +
                     "WHERE MC.pcat_codigo='{0}'"
                     , ddlcatalogo.SelectedValue);
                Utils.FillDll(ddlVin, sql, false);

                sql = "select pdoc_codigo,pdoc_codigo concat '-'  concat pdoc_nombre from dbxschema.PDOCUMENTOVEHICULO";
                Utils.FillDll(ddlCodDoc, sql, false);


                sql = String.Format(
                    "SELECT MVEH_CODIGO, \n" +
                     "       PCAT_CODIGO, \n" +
                     "       mvd.MCAT_VIN, \n" +
                     "       mvd.PDOC_CODIDOCU, \n" +
                     "       MVEH_NUMEDOCU, \n" +
                     "       MVEH_FECHINGRESO, \n" +
                     "       MVEH_VALODOCU, \n" +
                     "       MVEH_FECHENTREGA, \n" +
                     "       MVEH_OBSERVAC, \n" +
                     "       MVEH_NOMBRETRAMITA, \n" +
                     "       TRES_SINO \n" +
                     "FROM dbxschema.mvehiculodocumento mvd, \n" +
                     "     mcatalogovehiculo mc \n" +
                     "WHERE PCAT_CODIGO = '{0}' \n" +
                     "AND   mvd.mcat_vin = '{1}' \n" +
                     "AND   mvd.PDOC_CODIDOCU = '{2}' \n" +
                     "AND   mvd.mcat_vin = mc.mcat_vin"
                     , ddlcatalogo.SelectedValue
                     , ddlVin.SelectedValue
                     , ddlCodDoc.SelectedValue);
				DataSet d = new DataSet();
				DBFunctions.Request(d,IncludeSchema.NO,sql);
				if(d.Tables[0].Rows.Count>0)
				{
					if (d.Tables[0].Rows[0][10].ToString() == "S")
						ddlEntreCli.SelectedIndex=1;
					else
						ddlEntreCli.SelectedIndex=0;

					txtFechaDoc.Text=d.Tables[0].Rows[0][5].ToString();
					txtValoDoc.Text=d.Tables[0].Rows[0][6].ToString();
					txtFechaVencDoc.Text=d.Tables[0].Rows[0][7].ToString();
					txtObserva.Text=d.Tables[0].Rows[0][8].ToString();
					txtTramitador.Text=d.Tables[0].Rows[0][9].ToString();
					txtNumDoc.Text=d.Tables[0].Rows[0][4].ToString();	
				}
				else
				{
                    txtFechaDoc.Text="";
                    txtValoDoc.Text="";
					txtFechaVencDoc.Text="";
					txtObserva.Text="";
					txtTramitador.Text="";
					txtNumDoc.Text="";
					ddlEntreCli.SelectedIndex=2;
				}			
			}
			else
			{
				if (reload)
				{
					DataSet d = new DataSet();
                    DBFunctions.Request(d, IncludeSchema.NO, "select MVEH_CODIGO,PCAT_CODIGO,mvd.MCAT_VIN,PDOC_CODIDOCU,MVEH_NUMEDOCU,MVEH_FECHINGRESO,MVEH_VALODOCU,MVEH_FECHENTREGA,MVEH_OBSERVAC,MVEH_NOMBRETRAMITA,TRES_SINO from dbxschema.mvehiculodocumento mvd, mcatalogovehiculo mc where PCAT_CODIGO='" + ddlcatalogo.SelectedValue + "' and mvd.mcat_vin='" + ddlVin.SelectedValue + "' and PDOC_CODIDOCU='" + ddlCodDoc.SelectedValue + "'  and mvd.mcat_vin = mc.mcat_vin ");
					txtFechaDoc.Text=Convert.ToDateTime(d.Tables[0].Rows[0][5]).ToString("yyyy-MM-dd");
					txtValoDoc.Text=d.Tables[0].Rows[0][6].ToString();
					txtFechaVencDoc.Text=Convert.ToDateTime(d.Tables[0].Rows[0][7]).ToString("yyyy-MM-dd");
					txtObserva.Text=d.Tables[0].Rows[0][8].ToString();
					txtTramitador.Text=d.Tables[0].Rows[0][9].ToString();
					txtNumDoc.Text=d.Tables[0].Rows[0][4].ToString();
					reload=false;
				
				}
			}
		}

		protected void gen_rpt(object sender, EventArgs e)
		{
			
			DBFunctions.NonQuery("DROP VIEW DBXSCHEMA.VVEHICULOS_DOCUMENTOS_R");
			DBFunctions.NonQuery("create VIEW DBXSCHEMA.VVEHICULOS_DOCUMENTOS_R AS select * from dbxschema.vvehiculos_documentos where tipo='"+ddlcatalogoS+"' and VIN='"+ddlVinS+"'");
			
			ConString=System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
			Label lbvacio= new Label();
			string[] Formulas = new string[8];
			string[] ValFormulas = new string[8];
			string header = "AMS_HEADER.rpt";
			string footer = "AMS_FOOTER.rpt";
			DataSet tempDS = new DataSet();
			//JFSC 11022008 Poner en comentario por no ser usado
			//string where = "",filtro="";
			//string[] filtros;
			Formulas[0] = "CLIENTE";
			Formulas[1] = "NIT";
			Formulas[2] = "TITULO";
			Formulas[3] = "TITULO1";
			Formulas[4] = "SELECCION1";
			Formulas[5] = "SELECCION2";
			Formulas[6] = "VERSION";
			Formulas[7] = "REPORTE";

			string empresa= DBFunctions.SingleData("select  cemp_nombre from dbxschema.cempresa");
			ValFormulas[0] = ""+empresa+""; //nombre empresa
			string nit= DBFunctions.SingleData("select  mnit_nit from dbxschema.cempresa");
			
			DataSet datosReporte= new DataSet();
			string ruta=System.Configuration.ConfigurationManager.AppSettings["PathToPreviews"];
			ValFormulas[1] = ""+nit+"" ;
			ValFormulas[2] = "Acta Entrega Vehiculos"; //titulo rpt
			ValFormulas[3] = "SISTEMA DE VEHICULOS"; //subtitulo Sistema de Nomina
			ValFormulas[4] = ""; //año mes quince
			ValFormulas[5] = ""; //
			ValFormulas[6] = "ECAS - AMS VER 1.0.0.";
			Imprimir funcion=new Imprimir();
			string servidor=ConfigurationManager.AppSettings["Server" + GlobalData.getEMPRESA()];
			string database=ConfigurationManager.AppSettings["DataBase" + GlobalData.getEMPRESA()];
			string usuario=ConfigurationManager.AppSettings["UID"];
			string password=ConfigurationManager.AppSettings["PWD" + GlobalData.getEMPRESA()];
			AMS.CriptoServiceProvider.Crypto miCripto=new Crypto(AMS.CriptoServiceProvider.Crypto.CryptoProvider.TripleDES);
			miCripto.IV=ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
			miCripto.Key=ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
			string newPwd=miCripto.DescifrarCadena(password);
            string nomA = "Acta Entrega Vehiculos";
			funcion.PreviewReport2("AMS.Vehiculos.Documentos",header,footer,1,1,1,"","",nomA,"pdf",usuario,newPwd,Formulas,ValFormulas,lbvacio);
			Response.Write("<script language:javascript>w=window.open('"+funcion.Documento+"','','HEIGHT=600,WIDTH=600');</script>");
			DatasToControls param=  new DatasToControls();
            param.PutDatasIntoDropDownList(ddlcatalogo, "select distinct pcat_codigo from dbxschema.mvehiculo MV, MCATALOGOVEHICULO MC WHERE MC.MCAT_VIN = MV.MCat_VIN");
            param.PutDatasIntoDropDownList(ddlVin, "SELECT MV.MCAT_VIN AS VIN FROM MVEHICULO MV, mcatalogovehiculo MC WHERE MC.MCAT_VIN = MV.MCAT_VIN AND MC.PCAT_CODIGO='" + ddlcatalogo.SelectedValue + "'");
			param.PutDatasIntoDropDownList(ddlCodDoc,"select pdoc_codigo,pdoc_codigo concat '-'  concat pdoc_nombre from dbxschema.PDOCUMENTOVEHICULO");
			DataSet d = new DataSet();
            DBFunctions.Request(d, IncludeSchema.NO, "select MVEH_CODIGO,PCAT_CODIGO,mvd.MCAT_VIN,PDOC_CODIDOCU,MVEH_NUMEDOCU,MVEH_FECHINGRESO,MVEH_VALODOCU,MVEH_FECHENTREGA,MVEH_OBSERVAC,MVEH_NOMBRETRAMITA,TRES_SINO from dbxschema.mvehiculodocumento mvd, mcatalogovehiculo mc where PCAT_CODIGO='" + ddlcatalogo.SelectedValue + "' and mvd.mcat_vin='" + ddlVin.SelectedValue + "' and PDOC_CODIDOCU='" + ddlCodDoc.SelectedValue + "' and mvd.mcat_vin = mc.mcat_vin  ");
			if(d.Tables[0].Rows.Count>0)
			{
				txtFechaDoc.Text=d.Tables[0].Rows[0][5].ToString();
				txtValoDoc.Text=d.Tables[0].Rows[0][6].ToString();
				txtFechaVencDoc.Text=d.Tables[0].Rows[0][7].ToString();
				txtObserva.Text=d.Tables[0].Rows[0][8].ToString();
				txtTramitador.Text=d.Tables[0].Rows[0][9].ToString();
				txtNumDoc.Text=d.Tables[0].Rows[0][4].ToString();	
			}
			else
			{
				txtFechaDoc.Text="";
				txtValoDoc.Text="";
				txtFechaVencDoc.Text="";
				txtObserva.Text="";
				txtTramitador.Text="";
				txtNumDoc.Text="";
			}
			
		}

		[Ajax.AjaxMethod]
		public DataSet CargarVin(string pcatalogo)
		{
			DataSet Vins= new DataSet();
			//DBFunctions.Request(Vins,IncludeSchema.NO,"select mcat_vin as VIN from dbxschema.mvehiculo where pcat_codigo='"+pcatalogo+"'");
            DBFunctions.Request(Vins,IncludeSchema.NO,"SELECT MV.MCAT_VIN AS VIN FROM MVEHICULO MV, mcatalogovehiculo MC WHERE MC.MCAT_VIN = MV.MCAT_VIN AND MC.PCAT_CODIGO= '" + pcatalogo +"'");
			return Vins;
			
		}

		[Ajax.AjaxMethod]
		public DataSet CargarDatos(string doc, string cat,string vin)
		{

            string uno = "select MVEH_CODIGO,PCAT_CODIGO,mvd.MCAT_VIN,PDOC_CODIDOCU,MVEH_NUMEDOCU,MVEH_FECHINGRESO,MVEH_VALODOCU,MVEH_FECHENTREGA,MVEH_OBSERVAC,MVEH_NOMBRETRAMITA,TRES_SINO from dbxschema.mvehiculodocumento mvd, mcatalogovehiculo mc where PCAT_CODIGO='" + cat + "' and mvd.mcat_vin='" + vin + "' and PDOC_CODIDOCU='" + doc + "' and mvd.mcat_vin = mc.mcat_vin ";
			DataSet ds = new DataSet();
            DBFunctions.Request(ds, IncludeSchema.NO, "select MVEH_CODIGO,PCAT_CODIGO,mvd.MCAT_VIN,PDOC_CODIDOCU,MVEH_NUMEDOCU,MVEH_FECHINGRESO,MVEH_VALODOCU,MVEH_FECHENTREGA,MVEH_OBSERVAC,MVEH_NOMBRETRAMITA,TRES_SINO from dbxschema.mvehiculodocumento mvd, mcatalogovehiculo mc where PCAT_CODIGO='" + cat + "' and mvd.mcat_vin='" + vin + "' and PDOC_CODIDOCU='" + doc + "' and mvd.mcat_vin = mc.mcat_vin ");
			
			return ds;
		}


		[Ajax.AjaxMethod]
		public DataSet CargarDatos2(string vin ,string cat,string doc)
		{	
			DataSet ds = new DataSet();
            DBFunctions.Request(ds, IncludeSchema.NO, "select MVEH_CODIGO,PCAT_CODIGO,mvd.MCAT_VIN,PDOC_CODIDOCU,MVEH_NUMEDOCU,MVEH_FECHINGRESO,MVEH_VALODOCU,MVEH_FECHENTREGA,MVEH_OBSERVAC,MVEH_NOMBRETRAMITA,TRES_SINO from dbxschema.mvehiculodocumento mvd, mcatalogovehiculo mc where PCAT_CODIGO='" + cat + "' and mvd.mcat_vin='" + vin + "' and PDOC_CODIDOCU='" + doc + "' and mvd.mcat_vin = mc.mcat_vin ");
			return ds;
		}

		protected void GuardarDoc(Object Sender,System.EventArgs e)
		{
            if (txtFechaDoc.Text == "" || txtFechaVencDoc.Text == "")
            {
                Utils.MostrarAlerta(Response, "Las fechas son campos obligatorios!");
                DatasToControls postVin = new DatasToControls();
                postVin.PutDatasIntoDropDownList(ddlVin, "SELECT MV.MCAT_VIN AS VIN FROM MVEHICULO MV, mcatalogovehiculo MC WHERE MC.MCAT_VIN = MV.MCAT_VIN AND MC.PCAT_CODIGO='" + ddlcatalogo.SelectedValue + "'");
                return;
            }
           
            if (ddlEntreCli.SelectedIndex == 2)
            {
                Utils.MostrarAlerta(Response, "No ha seleccionado si fue entregado al cliente o no..");
                DatasToControls postVin = new DatasToControls();
                postVin.PutDatasIntoDropDownList(ddlVin, "SELECT MV.MCAT_VIN AS VIN FROM MVEHICULO MV, mcatalogovehiculo MC WHERE MC.MCAT_VIN = MV.MCAT_VIN AND MC.PCAT_CODIGO='" + ddlcatalogo.SelectedValue + "'");
                return;
            }
			DBFunctions.NonQuery("delete from dbxschema.mvehiculodocumento where mcat_vin='"+ddlVinS+"' and pdoc_codidocu='"+ddlCodDocS+"'");			
			DBFunctions.NonQuery("insert into dbxschema.MVEHICULODOCUMENTO values (default,'"+ddlVinS+"','"+ddlCodDocS+"','"+txtNumDocS+"','"+txtFechaDocS+"',"+txtValoDocS+",'"+txtFechaVencDocS+"','"+txtObservaS+"','"+txtTramitadorS+"','"+ddlEntreCli.SelectedValue+"',NULL)");
            DataSet d = new DataSet();
            DBFunctions.Request(d, IncludeSchema.NO, "select MVEH_CODIGO,PCAT_CODIGO,mvd.MCAT_VIN,PDOC_CODIDOCU,MVEH_NUMEDOCU,MVEH_FECHINGRESO,MVEH_VALODOCU,MVEH_FECHENTREGA,MVEH_OBSERVAC,MVEH_NOMBRETRAMITA,TRES_SINO from dbxschema.mvehiculodocumento mvd, mcatalogovehiculo mc where MC.PCAT_CODIGO='" + ddlcatalogoS + "' and mvd.mcat_vin='" + ddlVinS + "' and PDOC_CODIDOCU='" + ddlCodDocS + "' and mvd.mcat_vin = mc.mcat_vin ");
             if (d.Tables[0].Rows.Count == 0)
            {
                Utils.MostrarAlerta(Response, "Algo está mal con los datos insertados..");
                return;
            }
            txtFechaDoc.Text = Convert.ToDateTime(d.Tables[0].Rows[0][5]).ToString("yyyy-MM-dd");
            txtFechaVencDoc.Text = Convert.ToDateTime(d.Tables[0].Rows[0][7]).ToString("yyyy-MM-dd");
			txtValoDoc.Text=d.Tables[0].Rows[0][6].ToString();
			txtObserva.Text=d.Tables[0].Rows[0][8].ToString();
			txtTramitador.Text=d.Tables[0].Rows[0][9].ToString();
			txtNumDoc.Text=d.Tables[0].Rows[0][4].ToString();
			//DatasToControls.EstablecerDefectoDropDownList(ddl);
			DatasToControls param = new DatasToControls();
            param.PutDatasIntoDropDownList(ddlVin, "SELECT MV.MCAT_VIN AS VIN FROM MVEHICULO MV, mcatalogovehiculo MC WHERE MC.MCAT_VIN = MV.MCAT_VIN AND MC.PCAT_CODIGO= '" + ddlcatalogoS + "'");
			DatasToControls.EstablecerDefectoDropDownList(ddlVin,ddlVinS);

            Utils.MostrarAlerta(Response, "Información actualizada correctamente!");
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
