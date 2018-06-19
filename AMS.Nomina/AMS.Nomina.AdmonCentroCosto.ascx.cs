namespace FabianNomina
{
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
    using AMS.Tools;

	/// <summary>
	///		Configuracion de los Centros de Costo para cada empleado, validando no sumar 
	///		mas del 100%
	/// </summary>
	public class AMS_Nomina_AdmonCentroCosto : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Button Button1;
		protected System.Web.UI.WebControls.DataGrid gridRtns;
		protected System.Web.UI.WebControls.DropDownList DDLEMPLEADO;
		protected DataTable tablaPagos,tablaRtns,numerosCheques;
		protected System.Web.UI.WebControls.Label lb;
		protected System.Web.UI.WebControls.Button btn_guardar;
		
		protected void GuardarDatosCentrosCosto(object sender, EventArgs e)
		{
			int i,valPorcentaje=0;
			DataTable Datos = new DataTable();
			Datos=(DataTable)Session["tablaRtns"];
			ArrayList Insercion  = new ArrayList();
			if(gridRtns.Items.Count>0)			{
                DBFunctions.NonQuery("delete  from dbxschema.MEMPLEADOPCENTROCOSTO where memp_codiempl='" + DDLEMPLEADO.SelectedValue + "'");
				for (i=0;i<Datos.Rows.Count;i++)
				{
					valPorcentaje+=int.Parse(Datos.Rows[i][3].ToString());
				}
				if (valPorcentaje==100)
				{
					for (i=0;i<Datos.Rows.Count;i++)
					{
                        Insercion.Add("insert into dbxschema.mempleadopcentrocosto values ('" + Datos.Rows[i][0].ToString() + "','" + Datos.Rows[i][2].ToString() + "','" + Datos.Rows[i][3].ToString() + "','" + Datos.Rows[i][1].ToString() + "' )");
					}				
				}
				else
				{
                    Utils.MostrarAlerta(Response, "'Porfavor Complete el porcentaje 100%");
				}
				
				if (DBFunctions.Transaction(Insercion))
				{
				//	lb.Text="Bien"+DBFunctions.exceptions;
                    Utils.MostrarAlerta(Response, "Se ha actualizado correctamente");
				}
				else
				{
					lb.Text="error"+DBFunctions.exceptions;
				}
				
			}
			else
			{
                DBFunctions.NonQuery("delete  from dbxschema.MEMPLEADOPCENTROCOSTO where memp_codiempl='" + DDLEMPLEADO.SelectedValue + "'");
                Utils.MostrarAlerta(Response, "Este empleado no cuenta con ningún centro de costo");
			}
		}


		protected  void CargarDatosEmpleado(object sender, EventArgs e)
		{
			this.Cargar_Tabla_Rtns();
			this.Buscar_Centros_Costo(DDLEMPLEADO.SelectedValue);
			gridRtns.Visible=true;
		}
		
		protected void Buscar_Centros_Costo(string codempleado)
		{
		int i;
		DataSet centrocosto = new DataSet();
		DBFunctions.Request(centrocosto,IncludeSchema.NO,"Select * from DBXSCHEMA.MEMPLEADOPCENTROCOSTO where memp_codiempl='"+codempleado+"' ORDER BY MEMP_CODIEMPL,PCEN_CODIGO");
			if (centrocosto.Tables[0].Rows.Count==0)
			{
                Utils.MostrarAlerta(Response, "Este empleado no tiene ingresado ningun Centro de Costo");
				Session.Clear();
				gridRtns.DataSource=tablaRtns;
				gridRtns.DataBind();
			}
			else
			{
				for (i=0;i<centrocosto.Tables[0].Rows.Count;i++)
				{
                    this.ingresar_datos_datatable(centrocosto.Tables[0].Rows[i][0].ToString(), centrocosto.Tables[0].Rows[i][3].ToString(), centrocosto.Tables[0].Rows[i][1].ToString(), double.Parse(centrocosto.Tables[0].Rows[i][2].ToString()));
				}
				Session["tablaRtns"]=tablaRtns;
			}	
		}

		protected void Cargar_Tabla_Rtns()
		{
			tablaRtns=new DataTable();
			tablaRtns.Columns.Add(new DataColumn("CODEMP", typeof(string)));
            tablaRtns.Columns.Add(new DataColumn("ALMACEN", typeof(string)));
			tablaRtns.Columns.Add(new DataColumn("CENTROCOSTO", typeof(string)));
			tablaRtns.Columns.Add(new DataColumn("PORC", typeof(double)));
		}

        protected void ingresar_datos_datatable(string CODEMP, string ALMACEN, string CENTROCOSTO, double PORC)
		{			
			DataRow fila = tablaRtns.NewRow();
			fila["CODEMP"]=CODEMP;
            fila["ALMACEN"] = ALMACEN;
			fila["CENTROCOSTO"]=CENTROCOSTO;
			fila["PORC"]=PORC;
			tablaRtns.Rows.Add(fila);			
			gridRtns.DataSource = tablaRtns;
			gridRtns.DataBind();			
		}
		
		
		protected void gridRtns_Item(object Sender,DataGridCommandEventArgs e)
		{
			int i, Porcentaje=0;
            DatasToControls paramas = new DatasToControls();            
			//DataRow fila;
			if(((Button)e.CommandSource).CommandName=="AgregarCentroCosto")
			{
				//Response.Write("<script language:javascript>alert('Solo puede haber una retención de ese tipo');</script>");	
				if((((TextBox)e.Item.Cells[2].Controls[1]).Text==""))
                    Utils.MostrarAlerta(Response, "Debe ingresar un Centro de Costo");
								
				else if ((((TextBox)e.Item.Cells[3].Controls[1]).Text==""))
                    Utils.MostrarAlerta(Response, "Debe ingresar un Porcentaje");

                else if ((((TextBox)e.Item.Cells[1].Controls[1]).Text == ""))
                    Utils.MostrarAlerta(Response, "Debe ingresar un Almacen");

				else
				{
					if(Session["tablaRtns"]==null)
						this.Cargar_Tabla_Rtns();
					//Averiguar cuanto porcentaje tengo en tabla
					for (i=0;i<tablaRtns.Rows.Count;i++)
					{
						Porcentaje+=int.Parse(tablaRtns.Rows[i][3].ToString());
					}
					//Validar que sea un entero
					if (DatasToControls.ValidarInt(""+((TextBox)e.Item.Cells[3].Controls[1]).Text+""))
					{
						Porcentaje+=Convert.ToInt32(((TextBox)e.Item.Cells[3].Controls[1]).Text);	
						if (Porcentaje>100)
						{
                            Utils.MostrarAlerta(Response, "Supero el maximo permitido 100%");
						}
						else
						{										
							DataRow fila=tablaRtns.NewRow();
							fila["CODEMP"]=DDLEMPLEADO.SelectedValue;
                            fila["ALMACEN"] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
							fila["CENTROCOSTO"]=((TextBox)e.Item.Cells[2].Controls[1]).Text;
							fila["PORC"]=Convert.ToInt32(((TextBox)e.Item.Cells[3].Controls[1]).Text);
							tablaRtns.Rows.Add(fila);
							gridRtns.DataSource=tablaRtns;
							gridRtns.DataBind();
							Session["tablaRtns"]=tablaRtns;										
						}					
					}
					else
					{
                        Utils.MostrarAlerta(Response, "Los valores ingresados no pueden ser decimales, porfavor corrija");
					}
									
					//Response.Write("<script language:javascript>alert('Porcentaje "+Porcentaje+"');</script>");
										
				}
			}
			else if(((Button)e.CommandSource).CommandName=="RemoverCentroCosto")
			{
				Porcentaje-=Convert.ToInt32(tablaRtns.Rows[e.Item.DataSetIndex]["PORC"]);
				tablaRtns.Rows[e.Item.DataSetIndex].Delete();
                Utils.MostrarAlerta(Response, "Atencion:Si elige guardar este dato sera borrado.");
				gridRtns.DataSource=tablaRtns;
				gridRtns.DataBind();
				Session["tablaRtns"]=tablaRtns;
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				DatasToControls  paramas = new DatasToControls();
                paramas.PutDatasIntoDropDownList(DDLEMPLEADO, "SELECT M.MEMP_CODIEMPL, N.MNIT_APELLIDOS CONCAT ' ' CONCAT coalesce(N.MNIT_APELLIDO2,'') CONCAT ' ' CONCAT N.MNIT_NOMBRES CONCAT ' ' CONCAT coalesce(N.MNIT_NOMBRE2,'')  CONCAT ' - ' CONCAT m.memp_codiempl FROM DBXSCHEMA.MEMPLEADO M, DBXSCHEMA.MNIT N WHERE M.MNIT_NIT=N.MNIT_NIT and M.test_estado='1' ORDER BY N.MNIT_APELLIDOS");
				this.Cargar_Tabla_Rtns();
				this.Buscar_Centros_Costo(DDLEMPLEADO.SelectedValue);
				gridRtns.Visible=true;

			}
			else 
			{ 
				if (Session["tablaRtns"]!=null)
					tablaRtns=(DataTable)Session["tablaRtns"];				
			}		

			
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
			this.btn_guardar.Click += new System.EventHandler(this.GuardarDatosCentrosCosto);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	}
}
