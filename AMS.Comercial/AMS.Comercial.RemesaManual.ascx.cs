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
	///		Descripción breve de AMS_Comercial_RemesaManual.
	/// </summary>
	public class AMS_Comercial_RemesaManual : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.Panel Panel2;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.TextBox numerem;
		protected System.Web.UI.WebControls.TextBox fecharem;
		protected System.Web.UI.WebControls.TextBox planilla;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.DropDownList PaisO;
		protected System.Web.UI.WebControls.DropDownList PaisD;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.DropDownList DepartamentoO;
		protected System.Web.UI.WebControls.DropDownList CiuO;
		protected System.Web.UI.WebControls.DropDownList DepartamentoD;
		protected System.Web.UI.WebControls.DropDownList CiuD;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.TextBox valor;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.TextBox observaciones;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.TextBox IdE;
		protected System.Web.UI.WebControls.TextBox NombreE;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.TextBox TelE;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.TextBox DirE;
		protected System.Web.UI.WebControls.Label Label16;
		protected System.Web.UI.WebControls.Label Label17;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.Label Label19;
		protected System.Web.UI.WebControls.Label Label20;
		protected System.Web.UI.WebControls.DropDownList Bus;
		protected System.Web.UI.WebControls.Label Label21;
		protected System.Web.UI.WebControls.Label busN;
		protected System.Web.UI.WebControls.Button Guardar;
		protected System.Web.UI.WebControls.TextBox DirD;
		protected System.Web.UI.WebControls.TextBox TelR;
		protected System.Web.UI.WebControls.TextBox IdR;
		protected System.Web.UI.WebControls.TextBox NombreR;
		protected System.Web.UI.WebControls.Label Label22;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator2;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator3;
		protected System.Web.UI.WebControls.Panel Panel3;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!Page.IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(Bus,"select MCAT_PLACA from DBXSCHEMA.MBUSAFILIADO ORDER BY MCAT_PLACA");//carga el pais de origen
				bind.PutDatasIntoDropDownList(PaisO,"Select TPAI_NOMBRE from DBXSCHEMA.TPAIS ORDER BY TPAI_PAIS");//carga el pais de origen
				bind.PutDatasIntoDropDownList(PaisD,"Select TPAI_NOMBRE from DBXSCHEMA.TPAIS ORDER BY TPAI_PAIS;");//carga el pais de destino
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
			this.PaisO.SelectedIndexChanged += new System.EventHandler(this.PaisO_SelectedIndexChanged);
			this.PaisD.SelectedIndexChanged += new System.EventHandler(this.PaisD_SelectedIndexChanged);
			this.DepartamentoO.SelectedIndexChanged += new System.EventHandler(this.DepartamentoO_SelectedIndexChanged);
			this.DepartamentoD.SelectedIndexChanged += new System.EventHandler(this.DepartamentoD_SelectedIndexChanged);
			this.Bus.SelectedIndexChanged += new System.EventHandler(this.Bus_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		public void PaisO_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			int CodigoPais1=Convert.ToInt32(DBFunctions.SingleData("SELECT TPAI_PAIS FROM DBXSCHEMA.TPAIS WHERE TPAI_NOMBRE ='"+PaisO.SelectedValue.ToString()+"'"));
			if(DepartamentoO.Items.Count==0)
			{
				bind.PutDatasIntoDropDownList(DepartamentoO,"Select TDEP_NOMBRE as DEPARTAMENTO from DBXSCHEMA.TDEPARTAMENTO WHERE TPAI_PAIS = "+CodigoPais1+" ORDER BY TDEP_NOMBRE");
				ListItem it=new ListItem("--Departamento --","0");
				DepartamentoO.Items.Insert(0,it);
			}
			
		}

		public void DepartamentoO_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			int CodigoDepartamento1=Convert.ToInt32(DBFunctions.SingleData("SELECT TDEP_CODIGO FROM DBXSCHEMA.TDEPARTAMENTO WHERE TDEP_NOMBRE ='"+DepartamentoO.SelectedValue.ToString()+"'"));
			if(CiuO.Items.Count==0)
			{
				bind.PutDatasIntoDropDownList(CiuO,"Select TCIU_NOMBRE as CIUDAD from DBXSCHEMA.TCIUDAD WHERE TDEP_CODIGO = '"+CodigoDepartamento1+"' ORDER BY TCIU_NOMBRE");
				ListItem it=new ListItem("--Ciudad --","0");
				CiuO.Items.Insert(0,it);
				//esta funcion carga las ciudades del departamento seleccionado
			}
		}

		public void PaisD_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			int CodigoPais2=Convert.ToInt32(DBFunctions.SingleData("SELECT TPAI_PAIS FROM DBXSCHEMA.TPAIS WHERE TPAI_NOMBRE ='"+PaisD.SelectedValue.ToString()+"'"));
			if(DepartamentoD.Items.Count==0)
			{
				bind.PutDatasIntoDropDownList(DepartamentoD,"Select TDEP_NOMBRE as DEPARTAMENTO from DBXSCHEMA.TDEPARTAMENTO WHERE TPAI_PAIS = "+CodigoPais2+" ORDER BY TDEP_NOMBRE");
				ListItem it=new ListItem("--Departamento --","0");
				DepartamentoD.Items.Insert(0,it);
			}
		}

		public void DepartamentoD_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			int CodigoDepartamento2=Convert.ToInt32(DBFunctions.SingleData("SELECT TDEP_CODIGO FROM DBXSCHEMA.TDEPARTAMENTO WHERE TDEP_NOMBRE ='"+DepartamentoD.SelectedValue.ToString()+"'"));
			if(CiuD.Items.Count==0)
			{
				bind.PutDatasIntoDropDownList(CiuD,"Select TCIU_NOMBRE as CIUDAD from DBXSCHEMA.TCIUDAD WHERE TDEP_CODIGO = '"+CodigoDepartamento2+"' ORDER BY TCIU_NOMBRE");
				ListItem it=new ListItem("--Ciudad --","0");
				CiuD.Items.Insert(0,it);
				//esta funcion carga las ciudades del departamento seleccionado
			}
		}
		public void Grabar_Click(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			int codremesa=Convert.ToInt32(numerem.Text.ToString());
			string codigoR=DBFunctions.SingleData("select COD_REMESA from DBXSCHEMA.MREMESA_MANUAL where COD_REMESA="+codremesa+"");
			int CiuOcod=Convert.ToInt32(DBFunctions.SingleData("select TCIU_CODIGO from DBXSCHEMA.TCIUDAD WHERE TCIU_NOMBRE='"+CiuO.SelectedValue.ToString()+"'"));
			int CiuDcod=Convert.ToInt32(DBFunctions.SingleData("select TCIU_CODIGO from DBXSCHEMA.TCIUDAD WHERE TCIU_NOMBRE='"+CiuD.SelectedValue.ToString()+"'"));
			if(codigoR.Equals("") || codigoR.Equals(null))
			{
			
				DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MREMESA_MANUAL VALUES(DEFAULT,"+codremesa+","+planilla.Text.ToString()+",'"+fecharem.Text.ToString()+"',"+CiuOcod.ToString()+","+CiuDcod.ToString()+",'"+NombreE.Text.ToString()+"','"+NombreR.Text.ToString()+"','"+DirD.Text.ToString()+"','"+DirE.Text.ToString()+"','"+IdE.Text.ToString()+"','"+IdR.Text.ToString()+"',"+TelE.Text.ToString()+","+TelR.Text.ToString()+",'"+Bus.SelectedValue.ToString()+"','"+observaciones.Text.ToString()+"',"+valor.Text.ToString()+")");//query para insertar directo a la base de datos.
				Label22.Text=DBFunctions.exceptions;  //labael para mostrar las excepciones al momento de insertar o modificar la base de datos.
				Response.Write("<script language='javascript'>alert('Remesa Creada Satisfactoriamente');</script>");
			}
			else
			{
				Response.Write("<script language='javascript'>alert('Remesa Existente,Seleccione Otro Numero');</script>");
			}
			
			
			
			
			//en este evento se guarda toda la informacion del formulario de remesas en la base de datos
		}

		private void Bus_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string busNum=DBFunctions.SingleData("select MBUS_NUMERO from DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+Bus.SelectedValue.ToString()+"'");
			busN.Text=busNum.ToString();
		}
	}
}
