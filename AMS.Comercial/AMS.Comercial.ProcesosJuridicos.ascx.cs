namespace AMS.Comercial
{
	using System;
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
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.Forms;
	using AMS.DB;

	/// <summary>
	///		Descripción breve de AMS_Comercial_ProcesosJuridicos.
	/// </summary>
	public class AMS_Comercial_ProcesosJuridicos : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.Label Label16;
		protected System.Web.UI.WebControls.DropDownList añoA;
		protected System.Web.UI.WebControls.DropDownList mesA;
		protected System.Web.UI.WebControls.TextBox diaA;
		protected System.Web.UI.WebControls.DropDownList placa;
		protected System.Web.UI.WebControls.DropDownList demandado;
		protected System.Web.UI.WebControls.TextBox demandante;
		protected System.Web.UI.WebControls.TextBox asunto;
		protected System.Web.UI.WebControls.TextBox juzgado;
		protected System.Web.UI.WebControls.TextBox procesojuz;
		protected System.Web.UI.WebControls.TextBox pretenciones;
		protected System.Web.UI.WebControls.TextBox observaciones;
		protected System.Web.UI.WebControls.TextBox activiades;
		protected System.Web.UI.WebControls.Label Label17;
		protected System.Web.UI.WebControls.TextBox obserult;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.Label Label19;
		protected System.Web.UI.WebControls.Label Label20;
		protected System.Web.UI.WebControls.Label Label21;
		protected System.Web.UI.WebControls.Button Guardar;
		protected System.Web.UI.WebControls.DropDownList añoU;
		protected System.Web.UI.WebControls.DropDownList mesU;
		protected System.Web.UI.WebControls.TextBox diaU;
		protected System.Web.UI.WebControls.Label RemeasLabel;
		protected System.Web.UI.WebControls.Label numproceso;
		int consecutivo=0;
		string asociado=null;
		string chofer=null;
		string propietario=null;
		string asociadoN=null;
		string choferN=null;
		protected System.Web.UI.WebControls.TextBox docdemandante;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.DropDownList estadoproceso;
		protected System.Web.UI.WebControls.RegularExpressionValidator RegularExpressionValidator1;
		protected System.Web.UI.WebControls.RegularExpressionValidator RegularExpressionValidator2;
		protected System.Web.UI.WebControls.Label Label22;
		protected System.Web.UI.WebControls.Label Label23;
		protected System.Web.UI.WebControls.TextBox ubicacion;
		protected System.Web.UI.WebControls.Label Label24;
		protected System.Web.UI.WebControls.TextBox proxima;
		string propietarioN=null;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				
				bind.PutDatasIntoDropDownList(añoA, "SELECT pano_ano FROM DBXSCHEMA.pano");
				bind.PutDatasIntoDropDownList(mesA, "SELECT  pmes_nombre FROM DBXSCHEMA.pmes");		
				bind.PutDatasIntoDropDownList(añoU, "SELECT pano_ano FROM DBXSCHEMA.pano");
				bind.PutDatasIntoDropDownList(mesU, "SELECT  pmes_nombre FROM DBXSCHEMA.pmes");		
				
				string consecutivoS=DBFunctions.SingleData("select max(MPRO_NUMEROI) from DBXSCHEMA.MPROJURIDICO");
				if(consecutivoS.Equals(null) || consecutivoS.Equals(""))
				{
					consecutivo=1;
				}
				else
				{
					consecutivo=Convert.ToInt32(DBFunctions.SingleData("select max(MPRO_NUMEROI) from DBXSCHEMA.MPROJURIDICO"))+1;
				}
				numproceso.Text=consecutivo.ToString();
				
				
				if(placa.Items.Count==0)
				{
					bind.PutDatasIntoDropDownList(placa,"select MCAT_PLACA from DBXSCHEMA.MBUSAFILIADO ORDER BY MCAT_PLACA");
					ListItem it=new ListItem("--Seleccion--","0");
					placa.Items.Insert(0,it);
				
				}
			
				bind.PutDatasIntoDropDownList(estadoproceso,"select TEST_DESCRIPCION from DBXSCHEMA.TESTADO");


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
			this.placa.SelectedIndexChanged += new System.EventHandler(this.placa_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void placa_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			
			asociado=DBFunctions.SingleData("select MNIT_ASOCIADO from DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"'");
			chofer=DBFunctions.SingleData("select MNIT_NITCHOFER from DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"'");
			propietario=DBFunctions.SingleData("select MNIT_NITPROPIETARIO from DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placa.SelectedValue.ToString()+"' ");
			////obtener nombres
			asociadoN=DBFunctions.SingleData("select MNIT_NOMBRES CONCAT ' 'CONCAT MNIT_NOMBRE2 CONCAT ' ' CONCAT MNIT_APELLIDOS CONCAT ' ' CONCAT MNIT_APELLIDO2 from DBXSCHEMA.MNIT WHERE MNIT_NIT='"+asociado+"'");
			choferN=DBFunctions.SingleData("select MNIT_NOMBRES CONCAT ' 'CONCAT MNIT_NOMBRE2 CONCAT ' ' CONCAT MNIT_APELLIDOS CONCAT ' ' CONCAT MNIT_APELLIDO2 from DBXSCHEMA.MNIT WHERE MNIT_NIT='"+chofer+"'");
			propietarioN=DBFunctions.SingleData("select MNIT_NOMBRES CONCAT ' 'CONCAT MNIT_NOMBRE2 CONCAT ' ' CONCAT MNIT_APELLIDOS CONCAT ' ' CONCAT MNIT_APELLIDO2 from DBXSCHEMA.MNIT WHERE MNIT_NIT='"+propietario+"'");
			//////fin nombres
			demandado.Items.Insert(0,asociadoN.ToString());
			demandado.Items.Insert(1,choferN.ToString());
			demandado.Items.Insert(2,propietarioN.ToString());
			

		}//fin seleccion de demandados
		public void Guardar_Click(object sender, System.EventArgs e)
		{
			//////////////FECHA////////////
			int Año1=Convert.ToInt32(añoA.SelectedValue.ToString());
			int Año2=Convert.ToInt32(añoU.SelectedValue.ToString());
			int Mes1=Convert.ToInt32(DBFunctions.SingleData("select PMES_MES from DBXSCHEMA.PMES WHERE PMES_NOMBRE='"+mesA.SelectedValue.ToString()+"'"));
			int Mes2=Convert.ToInt32(DBFunctions.SingleData("select PMES_MES from DBXSCHEMA.PMES WHERE PMES_NOMBRE='"+mesU.SelectedValue.ToString()+"'"));
			int dia1=1;
			int dia2=1;
			string dia1S=diaA.Text;
			string dia2S=diaU.Text;
			if(dia1S.ToString().Equals(null) || dia1S.ToString().Equals("0"))
			{
				dia1=1;
			}
			else
			{
				dia1=Convert.ToInt32(diaA.Text.ToString());
			}
			if(dia2S.ToString().Equals(null) || dia2S.ToString().Equals("0"))
			{
				dia2=Convert.ToInt32(DateTime.Now.Day);
			}
			else
			{
				dia2=Convert.ToInt32(diaU.Text.ToString());
			}

			string fecha = Año1 + "-" + Mes1 + "-" + dia1; 
			string fecha2= Año2 + "-" + Mes2 + "-" + dia2; 

			///////////////////////////////FIN FECHAS//////////////
			string nit=DBFunctions.SingleData("select MNIT.MNIT_NIT FROM DBXSCHEMA.MNIT MNIT WHERE MNIT.MNIT_NOMBRES CONCAT ' ' CONCAT coalesce(MNIT.MNIT_NOMBRE2,'') CONCAT ' ' CONCAT MNIT.MNIT_APELLIDOS CONCAT ' 'CONCAT coalesce(MNIT.MNIT_APELLIDO2,'') ='"+demandado.SelectedValue.ToString()+"' ");
			///////// CODIGO ESTADO PROCESO/////
			int codigoestado=Convert.ToInt32(DBFunctions.SingleData("select TEST_ESTADO from DBXSCHEMA.TESTADO WHERE TEST_DESCRIPCION='"+estadoproceso.SelectedValue.ToString()+"' "));
			////// FIN CODIGO ESTADO PROCESO //////
			DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MPROJURIDICO VALUES(DEFAULT,'"+fecha.ToString()+"','"+placa.SelectedValue.ToString()+"','"+nit.ToString()+"','"+demandante.Text.ToString()+"','"+docdemandante.Text.ToString()+"','"+asunto.Text.ToString()+"','"+juzgado.Text.ToString()+"','"+procesojuz.Text.ToString()+"','"+pretenciones.Text.ToString()+"','"+observaciones.Text.ToString()+"','"+activiades.Text.ToString()+"','"+fecha2.ToString()+"','"+obserult.Text.ToString()+"',"+codigoestado+",'"+ubicacion.Text.ToString()+"','"+proxima.Text.ToString()+"')");
			Label22.Text=DBFunctions.exceptions;  //label para mostrar las excepciones al momento de insertar o modificar la base de datos.
			Response.Write("<script language='javascript'>alert('Proceso Almacenado');</script>");
			//en este evento se guarda toda la informacion del formulario 
		}

	}
}
