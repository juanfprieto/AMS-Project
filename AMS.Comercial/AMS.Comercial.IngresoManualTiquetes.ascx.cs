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
	using AMS.Tools;
	/// <summary>
	///		Descripción breve de AMS_Comercial_IngresoManualTiquetes.
	/// </summary>
	public class AMS_Comercial_IngresoManualTiquetes : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label GirosLabel;
		//int cantidad=0;
		//double valor=0;
		//double ValorPlanilla=0;
		//string ValorFormato=null;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList agencia;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.DropDownList despachador;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.TextBox planilla;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DataGrid Grid;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.TextBox fecha;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.TextBox hora;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Button Guardar;
		protected System.Web.UI.WebControls.DropDownList bus;
		protected System.Web.UI.WebControls.DropDownList DDLESTADO;
		protected System.Web.UI.WebControls.DropDownList DDLDESTINO;
		protected System.Web.UI.WebControls.DropDownList TTIPOTIQUETE;

		//int bandera=0;
		int Posicion=0;
		string query=null;
		int a=0;
		string []Info;

		private void Page_Load(object sender, System.EventArgs e)
		{
			
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				//Grid
				fecha.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");
				hora.Text=DateTime.Now.Hour.ToString()+"-"+DateTime.Now.Minute.ToString()+"-"+DateTime.Now.Second.ToString();
				int cont=0;
				if(cont==0)
				{
					bind.PutDatasIntoDropDownList(agencia,"Select MOFI_CODIGO,MOFI_DESCRIPCION from DBXSCHEMA.MOFICINA ORDER BY MOFI_CODIGO;");
					ListItem it=new ListItem("--Agencias --","0");
					agencia.Items.Insert(0,it);
				}
				int contBus=0;
				if(contBus==0)
				{
					bind.PutDatasIntoDropDownList(bus,"");
					ListItem it=new ListItem("--VEHICULOS --","0");
					bus.Items.Insert(0,it);
				}
				else
				{
					if (Session["Posicion"]!=null)
					{
						Posicion=Convert.ToInt32(Session["Posicion"]);
					}
					if(Session["Info"] != null)
					{
						Info=(string[])Session["Info"]; 
					}
				}

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
			this.agencia.SelectedIndexChanged += new System.EventHandler(this.agencia_SelectedIndexChanged);
			this.Grid.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.Grid_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		

		private void agencia_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			int conteo=0;
			if(conteo==0)
			{
				bind.PutDatasIntoDropDownList(despachador,"select PTIQUE_CEDULA,PTIQUE_NOMBRE from DBXSCHEMA.PTIQUETERO WHERE MOFI_CODIGO='"+agencia.SelectedValue.ToString()+"'");
				ListItem it=new ListItem("--Despachador--","0");
				despachador.Items.Insert(0,it);
			}
		

			
		
		}

		private void tiquetero_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
			
		}
		protected void DgInserts_AddAndDel(object sender, DataGridCommandEventArgs e)
		{
			
			if(((Button)e.CommandSource).CommandName == "AddDatasRow")
			{
				
				

				int numero=Convert.ToInt32(((TextBox)e.Item.Cells[0].Controls[1]).Text);
				int cantidad = Convert.ToInt32(((TextBox)e.Item.Cells[4].Controls[1]).Text);
				double valor=Convert.ToInt32(((TextBox)e.Item.Cells[3].Controls[1]).Text);
				string destino=((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue[1].ToString();
				string estado=((DropDownList)e.Item.Cells[1].Controls[1]).SelectedValue[1].ToString();
				string Tipo=((DropDownList)e.Item.Cells[5].Controls[1]).SelectedValue[1].ToString();
				
				query=numero.ToString()+"-"+cantidad.ToString()+"-"+valor.ToString()+"-"+destino.ToString()+"-"+estado.ToString()+"-"+Tipo.ToString();
				a++;
				Session["Posicion"]=a;
				Session["Info"]=query.ToString();
					
				//Referencias.Guardar(((TextBox)e.Item.Cells[0].Controls[1]).Text.Trim(),ref codI,(((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue.Split('-'))[1]);
					
				
			
			}
		}
			
		
		
		private void Guardar_OnClick(object sender, System.EventArgs e)
		{
		string []InQuery=(string[])Session["Info"];
		int b=Convert.ToInt32(InQuery.Length);
		string []QueryFrac=new string[InQuery.Length];
			
			for(int c=0;c<=b;c++)
			{
				//insercion datos
					string QueryDB="INSERT INTO MPLANILLA_VIAJE VALUES("+planilla.Text.ToString()+","+agencia+","+despachador.SelectedValue.ToString()+","+QueryFrac[0]+","+QueryFrac[1]+","+QueryFrac[2]+","+QueryFrac[3]+","+QueryFrac[4]+","+QueryFrac[5]+") ";
					//                                                     planilla                    agencia                   tiquetero                  #tiquete          estado           destino          valor             cantidad          tipo tiquete

				
			}

			
		}

		private void Grid_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item ||e.Item.ItemType==ListItemType.AlternatingItem )
			{
				DatasToControls bind = new  DatasToControls();
				int codigoTestado=0;
				if(codigoTestado==0)
				{
					bind.PutDatasIntoDropDownList(DDLESTADO,"Select TTIQ_CODIGO,TTIQ_DESCRIPCION from DBXSCHEMA.TTIQUETE");
					ListItem it=new ListItem("-Estado-","0");
					DDLESTADO.Items.Insert(0,it);
				}
				int codigoAge=0;
				if(codigoAge==0)
				{
					bind.PutDatasIntoDropDownList(DDLDESTINO,"Select TCIU_CODIGO,TCIU_NOMBRE from DBXSCHEMA.TCIUDAD ORDER BY TCIU_CODIGO");
					ListItem it=new ListItem("-DESTINO-","0");
					DDLDESTINO.Items.Insert(0,it);
				}
				int codigoTipo=0;
				
				if(codigoTipo==0)
				{
					bind.PutDatasIntoDropDownList(TTIPOTIQUETE,"Select TTIQ_CODIGO,TTIQ_DESCRIPCION from DBXSCHEMA.TTIQUETE");
					ListItem it=new ListItem("-DESTINO-","0");
					TTIPOTIQUETE.Items.Insert(0,it);
				}
			}
		}
		

		
		

		

	
	
	}
}


