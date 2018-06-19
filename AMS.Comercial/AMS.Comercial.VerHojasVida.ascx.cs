namespace AMS.Comercial
{
	using System.IO;
	using System.Text;
	using System.Collections;
	using System.ComponentModel;
	using System.Globalization;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.Mail;
	using System.Web.UI;
	using System;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
	using AMS.Tools;
	using Ajax;

	/// <summary>
	///		Descripción breve de AMS_Comercial_VerHojasVida.
	/// </summary>
	public class AMS_Comercial_VerHojasVida : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label RemeasLabel;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList empleado;
		protected System.Web.UI.WebControls.Button Consultar;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label nombre;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label estado;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label fechaIngreso;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label estadoCivil;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.Label fechaNacimiento;
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.Image foto;
		string nit=null;
		int estadoEmpleado=0;
		protected System.Web.UI.WebControls.Label Label5;
		int estadoCivilEmpleado=0;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.Label Label16;
		protected System.Web.UI.WebControls.Label Label17;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.Label Label19;
		protected System.Web.UI.WebControls.Label Label21;
		protected System.Web.UI.WebControls.Label Label22;
		protected System.Web.UI.WebControls.Label Label23;
		protected System.Web.UI.WebControls.Label Label24;
		protected System.Web.UI.WebControls.Label Label25;
		protected System.Web.UI.WebControls.Label Label26;
		protected System.Web.UI.WebControls.Label Label27;
		protected System.Web.UI.WebControls.Label Label28;
		protected System.Web.UI.WebControls.Label Label29;
		protected System.Web.UI.WebControls.Label Label30;
		protected System.Web.UI.WebControls.Label Label54;
		protected System.Web.UI.WebControls.Label Label56;
		protected System.Web.UI.WebControls.Label Label57;
		protected System.Web.UI.WebControls.Label Label60;
		protected System.Web.UI.WebControls.Label Label62;
		protected System.Web.UI.WebControls.Label Label64;
		protected System.Web.UI.WebControls.Label Label66;
		protected System.Web.UI.WebControls.Label Label68;
		protected System.Web.UI.WebControls.Label Label70;
		protected System.Web.UI.WebControls.Label Label72;
		protected System.Web.UI.WebControls.Label nitLabel;
		protected System.Web.UI.WebControls.Label epsLabel;
		protected System.Web.UI.WebControls.Label naciemientoLabel;
		protected System.Web.UI.WebControls.Label sexoLabel;
		protected System.Web.UI.WebControls.Label TipoContrato;
		protected System.Web.UI.WebControls.Label FechaFinContrato;
		protected System.Web.UI.WebControls.Label ProfesionLabel;
		protected System.Web.UI.WebControls.Label LibretaProLabel;
		protected System.Web.UI.WebControls.Label SueldoALabel;
		protected System.Web.UI.WebControls.Label HijosLabel;
		protected System.Web.UI.WebControls.Label SueldoAnLabel;
		protected System.Web.UI.WebControls.Label FechaSALabel;
		protected System.Web.UI.WebControls.Label FechaSAnLabel;
		protected System.Web.UI.WebControls.Label SalarioPLabel;
		protected System.Web.UI.WebControls.Label SubTransLabel;
		protected System.Web.UI.WebControls.Label NumeroAepsLabel;
		protected System.Web.UI.WebControls.Label BancoLabel;
		protected System.Web.UI.WebControls.Label SangreLabel;
		protected System.Web.UI.WebControls.Label NumCFPLabel;
		protected System.Web.UI.WebControls.Label FondoPLabel;
		protected System.Web.UI.WebControls.Label FcesantiasLabel;
		protected System.Web.UI.WebControls.Label NumCFondoCLabel;
		protected System.Web.UI.WebControls.Label arpLabel;
		protected System.Web.UI.WebControls.Label NumCarpLabel;
		protected System.Web.UI.WebControls.Label LicenciaLabel;
		protected System.Web.UI.WebControls.Label CatLicenciaLabel;
		protected System.Web.UI.WebControls.Label FPagoLabel;
		string NombreFoto=null;
		protected DataSet lineas;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(empleado,"SELECT MNIT.MNIT_NOMBRES CONCAT ' 'CONCAT coalesce(MNIT.MNIT_NOMBRE2,'') CONCAT ' ' CONCAT MNIT.MNIT_APELLIDOS CONCAT ' ' CONCAT coalesce(MNIT.MNIT_APELLIDO2,'') from DBXSCHEMA.MNIT MNIT,DBXSCHEMA.MEMPLEADO MEMP WHERE MEMP.MNIT_NIT=MNIT.MNIT_NIT");
			}
		}
		public void Generar_Click(object sender, System.EventArgs e)
		{
			Panel1.Visible=true;
			nombre.Text=empleado.SelectedValue.ToString();
			
			nit=DBFunctions.SingleData("SELECT MNIT_NIT FROM DBXSCHEMA.MNIT WHERE MNIT_NOMBRES CONCAT ' 'CONCAT MNIT_NOMBRE2 CONCAT ' ' CONCAT MNIT_APELLIDOS CONCAT ' ' CONCAT MNIT_APELLIDO2 ='"+empleado.SelectedValue.ToString()+"' ");
			nitLabel.Text=nit.ToString();
			estadoEmpleado=Convert.ToInt32(DBFunctions.SingleData("select TEST_ESTADO from DBXSCHEMA.MEMPLEADO WHERE MNIT_NIT='"+nit+"' "));
			estado.Text=DBFunctions.SingleData("select TEST_NOMBRE from DBXSCHEMA.TESTADOEMPLEADO WHERE TEST_ESTADO='"+estadoEmpleado+"' ").ToString();
			fechaIngreso.Text=DBFunctions.SingleData("select MEMP_FECHINGRESO from DBXSCHEMA.MEMPLEADO WHERE MNIT_NIT='"+nit+"'").ToString();
			estadoCivilEmpleado=Convert.ToInt32(DBFunctions.SingleData("select TEST_ESTACIVIL from DBXSCHEMA.MEMPLEADO WHERE MNIT_NIT='"+nit+"'"));
			estadoCivil.Text=DBFunctions.SingleData("select TEST_NOMBRE FROM DBXSCHEMA.TESTADOCIVIL WHERE TEST_ESTACIVIL='"+estadoCivilEmpleado+"' ").ToString();
			fechaNacimiento.Text=DBFunctions.SingleData("select MEMP_FECHNACI from DBXSCHEMA.MEMPLEADO WHERE MNIT_NIT='"+nit+"'").ToString();
			NombreFoto=DBFunctions.SingleData("select MHV_REFOTO from DBXSCHEMA.MHOJAVIDA_FOTO WHERE MHV_NIT='"+nit+"'");
			string url=System.Configuration.ConfigurationManager.AppSettings["PathToImages"] +"FotoHV/" ;
			url=url + NombreFoto;
			foto.ImageUrl=url;
////////////////////////DEMAS Datos/////////////////////////////
			lineas = new DataSet();
			DBFunctions.Request(lineas,IncludeSchema.NO,"select * from DBXSCHEMA.MEMPLEADO where MNIT_NIT='"+nit+"'");
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
				string CodSexo=lineas.Tables[0].Rows[i].ItemArray[20].ToString();
				string descSexo=DBFunctions.SingleData("select TSEX_NOMBRE from DBXSCHEMA.TSEXO WHERE TSEX_CODIGO='"+CodSexo+"'");
				sexoLabel.Text=descSexo.ToString();
				string CodSangre=lineas.Tables[0].Rows[i].ItemArray[57].ToString();
				string RH=DBFunctions.SingleData("select TTIP_TIPOSANG from DBXSCHEMA.TTIPOSANGRE WHERE TTIP_SECUENCIA="+CodSangre+" ");
				SangreLabel.Text=RH.ToString();
				HijosLabel.Text=lineas.Tables[0].Rows[i].ItemArray[22].ToString();
				string codCiudad=lineas.Tables[0].Rows[i].ItemArray[3].ToString();
				string CiudadNaci=DBFunctions.SingleData("SELECT PCIU_NOMBRE from DBXSCHEMA.PCIUDAD WHERE PCIU_CODIGO='"+codCiudad+"'");
				naciemientoLabel.Text=CiudadNaci.ToString();
				FechaFinContrato.Text=lineas.Tables[0].Rows[i].ItemArray[11].ToString();
				string TipContato=lineas.Tables[0].Rows[i].ItemArray[8].ToString();
				string descContato=DBFunctions.SingleData("select TCON_NOMBRE from DBXSCHEMA.TCONTRATONOMINA WHERE TCON_CONTRATO='"+TipContato+"' ");
				TipoContrato.Text=descContato.ToString();
				string Profesion=lineas.Tables[0].Rows[i].ItemArray[14].ToString();
				string DProfesion=DBFunctions.SingleData("select PCAR_NOMBCARG from DBXSCHEMA.PCARGOEMPLEADO WHERE PCAR_CODICARGO='"+Profesion+"' ");
				ProfesionLabel.Text=DProfesion.ToString();
				LibretaProLabel.Text=lineas.Tables[0].Rows[i].ItemArray[16].ToString();
				string ForPago=lineas.Tables[0].Rows[i].ItemArray[48].ToString();
				string DForPago=DBFunctions.SingleData("select TFOR_DESCRIPCION from DBXSCHEMA.TFORMAPAGO WHERE TFOR_PAGO='"+ForPago+"' ");
				FPagoLabel.Text=DForPago.ToString();
				string CodBanco=lineas.Tables[0].Rows[i].ItemArray[49].ToString();
				string Banco=DBFunctions.SingleData("select PBAN_DESCRIPCION from DBXSCHEMA.PBANCO WHERE PBAN_CODIGO='"+CodBanco+"' ");
				BancoLabel.Text=Banco.ToString();
				
				SueldoALabel.Text=lineas.Tables[0].Rows[i].ItemArray[24].ToString();
				FechaSALabel.Text=lineas.Tables[0].Rows[i].ItemArray[25].ToString();

				SueldoAnLabel.Text=lineas.Tables[0].Rows[i].ItemArray[26].ToString();
				FechaSAnLabel.Text=lineas.Tables[0].Rows[i].ItemArray[27].ToString();

				SalarioPLabel.Text=lineas.Tables[0].Rows[i].ItemArray[28].ToString();
				string CodSubsidio=lineas.Tables[0].Rows[i].ItemArray[29].ToString();
				string Subsidio=DBFunctions.SingleData("select TSUB_DESCRIPCION from DBXSCHEMA.TSUBTRANSPORTE WHERE TSUB_TRANSP='"+CodSubsidio+"' ");
				SubTransLabel.Text=Subsidio.ToString();

				string CodEps=lineas.Tables[0].Rows[i].ItemArray[30].ToString();
				string EPS=DBFunctions.SingleData("select PEPS_NOMBEPS from DBXSCHEMA.PEPS WHERE PEPS_CODIEPS='"+CodEps+"' ");
				epsLabel.Text=EPS.ToString();
				NumeroAepsLabel.Text=lineas.Tables[0].Rows[i].ItemArray[31].ToString();

				string CodFondoPe=lineas.Tables[0].Rows[i].ItemArray[32].ToString();
				string FondoPens=DBFunctions.SingleData("select PFON_NOMBPENS from DBXSCHEMA.PFONDOPENSION WHERE PFON_CODIPENS='"+CodFondoPe+"' ");				
				FondoPLabel.Text=FondoPens.ToString();

				NumCFPLabel.Text=lineas.Tables[0].Rows[i].ItemArray[33].ToString();

				string CodFoCesantias=lineas.Tables[0].Rows[i].ItemArray[35].ToString();
				string FondoCesantias=DBFunctions.SingleData("SELECT PFON_NOMBFOND from DBXSCHEMA.PFONDOCESANTIAS WHERE PFON_CODIFOND='"+CodFoCesantias+"' ");
				FcesantiasLabel.Text=FondoCesantias.ToString();

				NumCFondoCLabel.Text=lineas.Tables[0].Rows[i].ItemArray[36].ToString();
                
				string CodArp=lineas.Tables[0].Rows[i].ItemArray[37].ToString();
				string ARP=DBFunctions.SingleData("select PARP_NOMBARP from DBXSCHEMA.PARP WHERE PARP_CODIARP='"+CodArp+"' ");
				arpLabel.Text=ARP.ToString();

				NumCarpLabel.Text=lineas.Tables[0].Rows[i].ItemArray[38].ToString();

				LicenciaLabel.Text=lineas.Tables[0].Rows[i].ItemArray[42].ToString();
				CatLicenciaLabel.Text=lineas.Tables[0].Rows[i].ItemArray[43].ToString();
				

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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
