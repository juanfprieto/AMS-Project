using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using System.Configuration;
using AMS.Tools;

namespace AMS.Finanzas.Cartera
{
	public partial  class EmisionOrden : System.Web.UI.UserControl
	{
		protected DataTable dtVehiculos;
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				//Si se realizo la busqueda con catálogo de vehículo y existe la consulta de vehículos
				//y la validación del DataSet retorna falso (el dataset esta bien)
				if(Request.QueryString["tipo"]=="CV" && Session["ds"]!=null && !this.validar_Ds())
				{
					this.Llenar_dtVehiculos();
				}
				//Si se realizo la busqueda con la placa del vehiculo y existe la consulta de vehículos
				//y la validación del DataSet retorna falso (el dataset esta bien)
				else if(Request.QueryString["tipo"]=="PV" && Session["ds"]!=null && !this.validar_Ds())
				{
					this.Llenar_dtVehiculos();
				}
				//Si se realizo la busqueda con orden de trabajo y existe la consulta de vehículos
				//y la validación del DataSet retorna falso (el dataset esta bien)
				else if(Request.QueryString["tipo"]=="OT" && Session["ds"]!=null && !this.validar_Ds())
				{
					this.Llenar_dtVehiculos("OT");
				}
				//Si se realizo la busqueda con nit y existe la consulta de vehículos
				//y la validación del DataSet retorna falso (el dataset esta bien)
				else if(Request.QueryString["tipo"]=="NP" && Session["ds"]!=null && !this.validar_Ds())
				{
					this.Llenar_dtVehiculos();
				}
				//Si no se cumple ninguna de las 4 condiciones
				else
					Response.Redirect(indexPage+"?process=Cartera.EmisionOrdenSalidaVehiculo&err=1");
			}
			else
			{
				if(Session["dtVehiculos"]!=null)
					dtVehiculos=(DataTable)Session["dtVehiculos"];
			}
		}
		
		protected void Cargar_dtVehiculos()
		{
			dtVehiculos=new DataTable();
			dtVehiculos.Columns.Add("CC",typeof(string));//0
			dtVehiculos.Columns.Add("NOMBRE",typeof(string));//1
			dtVehiculos.Columns.Add("CATALOGO",typeof(string));//2
			dtVehiculos.Columns.Add("VIN",typeof(string));//3
			dtVehiculos.Columns.Add("PLACA",typeof(string));//4
			dtVehiculos.Columns.Add("MOTOR",typeof(string));//5
			dtVehiculos.Columns.Add("COLOR",typeof(string));//6
            dtVehiculos.Columns.Add("OT", typeof(string));//7
            dtVehiculos.Columns.Add("NUM_OT", typeof(string));//8
            dtVehiculos.Columns.Add("ESTADO",typeof(bool));//9
			dtVehiculos.Columns.Add("TABLA",typeof(string));//10
           
        }
		
		protected bool validar_Ds()
		{
			bool estado=false;
			int cont=0;
			DataSet ds=new DataSet();
			ds=(DataSet)Session["ds"];
			for(int i=0;i<ds.Tables.Count;i++)
				cont++;
			if(cont==0)
				estado=true;
			return estado;
		}
		
		protected void Llenar_dtVehiculos()
		{
			//ds.Tables[0] = Orden
			//ds.Tables[1] = Vehiculo
			DataSet ds=new DataSet();
			DataRow fila;
			ds=(DataSet)Session["ds"];
			if(Session["dtVehiculos"]==null)
				this.Cargar_dtVehiculos();
			if(ds.Tables.Count > 1)
			{
				//Si viene algo con la orden pero nada en la de vehiculos
				if(ds.Tables[0].Rows.Count!=0 && ds.Tables[1].Rows.Count==0)
				{
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						fila=dtVehiculos.NewRow();
						fila[0]=ds.Tables[0].Rows[i][0].ToString();
						fila[1]=ds.Tables[0].Rows[i][1].ToString();
						fila[2]=ds.Tables[0].Rows[i][2].ToString();
						fila[3]=ds.Tables[0].Rows[i][3].ToString();
						fila[4]=ds.Tables[0].Rows[i][4].ToString();
						fila[5]=ds.Tables[0].Rows[i][5].ToString();
						fila[6]=ds.Tables[0].Rows[i][6].ToString();
                        fila[7] = ds.Tables[0].Rows[i][7].ToString();
                        fila[8] = ds.Tables[0].Rows[i][8].ToString();
                        fila[9] = false;
                        fila[10] = "O";

                        dtVehiculos.Rows.Add(fila);
					}
					dgVehiculos.DataSource=dtVehiculos;
					dgVehiculos.DataBind();
					Session["dtVehiculos"]=dtVehiculos;
				}
				//Si no viene nada con la orden y viene algo con vehiculo
				else if(ds.Tables[0].Rows.Count==0 && ds.Tables[1].Rows.Count!=0)
				{
					for(int i=0;i<ds.Tables[1].Rows.Count;i++)
					{
						fila=dtVehiculos.NewRow();
						fila[0]=ds.Tables[1].Rows[i][0].ToString();
						fila[1]=ds.Tables[1].Rows[i][1].ToString();
						fila[2]=ds.Tables[1].Rows[i][2].ToString();
						fila[3]=ds.Tables[1].Rows[i][3].ToString();
						fila[4]=ds.Tables[1].Rows[i][4].ToString();
						fila[5]=ds.Tables[1].Rows[i][5].ToString();
						fila[6]=ds.Tables[1].Rows[i][6].ToString();
                        fila[7] = ds.Tables[1].Rows[i][7].ToString(); ;
                        fila[8] = ds.Tables[1].Rows[i][8].ToString(); ;
                        fila[9] = false;
                        fila[10] = "O";

                        dtVehiculos.Rows.Add(fila);
					}
					dgVehiculos.DataSource=dtVehiculos;
					dgVehiculos.DataBind();
					Session["dtVehiculos"]=dtVehiculos;
				}
				//Si vienen datos de ambas
				else if(ds.Tables[0].Rows.Count!=0 && ds.Tables[1].Rows.Count!=0)
				{
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						fila=dtVehiculos.NewRow();
						fila[0]=ds.Tables[0].Rows[i][0].ToString();
						fila[1]=ds.Tables[0].Rows[i][1].ToString();
						fila[2]=ds.Tables[0].Rows[i][2].ToString();
						fila[3]=ds.Tables[0].Rows[i][3].ToString();
						fila[4]=ds.Tables[0].Rows[i][4].ToString();
						fila[5]=ds.Tables[0].Rows[i][5].ToString();
						fila[6]=ds.Tables[0].Rows[i][6].ToString();
                        fila[7] = ds.Tables[0].Rows[i][7].ToString(); ;
                        fila[8] = ds.Tables[0].Rows[i][8].ToString(); ;
                        fila[9] = false;
                        fila[10] = "O";

                        dtVehiculos.Rows.Add(fila);
					}
					for(int i=0;i<ds.Tables[1].Rows.Count;i++)
					{
						fila=dtVehiculos.NewRow();
						fila[0]=ds.Tables[1].Rows[i][0].ToString();
						fila[1]=ds.Tables[1].Rows[i][1].ToString();
						fila[2]=ds.Tables[1].Rows[i][2].ToString();
						fila[3]=ds.Tables[1].Rows[i][3].ToString();
						fila[4]=ds.Tables[1].Rows[i][4].ToString();
						fila[5]=ds.Tables[1].Rows[i][5].ToString();
						fila[6]=ds.Tables[1].Rows[i][6].ToString();
                        fila[7] = ds.Tables[1].Rows[i][7].ToString(); ;
                        fila[8] = ds.Tables[1].Rows[i][8].ToString(); ;
                        fila[9] = false;
                        fila[10] = "O";

                        dtVehiculos.Rows.Add(fila);
					}
					dgVehiculos.DataSource=dtVehiculos;
					dgVehiculos.DataBind();
					Session["dtVehiculos"]=dtVehiculos;
				}
				//Si no vienen datos en ninguna de las dos
				else if(ds.Tables[0].Rows.Count==0 && ds.Tables[1].Rows.Count==0)
					    Response.Redirect(indexPage+"?process=Cartera.EmisionOrdenSalidaVehiculo&err=2");
			}
			else
            {
                Response.Redirect(indexPage + "?process=Cartera.EmisionOrdenSalidaVehiculo&err=1");
            }
		}
		
		protected void Llenar_dtVehiculos(string tipo)
		{
			DataSet ds=new DataSet();
			DataRow fila;
			ds=(DataSet)Session["ds"];
			if(Session["dtVehiculos"]==null)
				this.Cargar_dtVehiculos();
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				fila=dtVehiculos.NewRow();
				fila[0]=ds.Tables[0].Rows[i][0].ToString();
				fila[1]=ds.Tables[0].Rows[i][1].ToString();
				fila[2]=ds.Tables[0].Rows[i][2].ToString();
				fila[3]=ds.Tables[0].Rows[i][3].ToString();
				fila[4]=ds.Tables[0].Rows[i][4].ToString();
				fila[5]=ds.Tables[0].Rows[i][5].ToString();
				fila[6]=ds.Tables[0].Rows[i][6].ToString();
                fila[7] = ds.Tables[0].Rows[i][7].ToString(); ;
                fila[8] = ds.Tables[0].Rows[i][8].ToString(); ;
                fila[9]=false;
				fila[10]="O";

                dtVehiculos.Rows.Add(fila);
			}
			dgVehiculos.DataSource=dtVehiculos;
			dgVehiculos.DataBind();
			Session["dtVehiculos"]=dtVehiculos;
		}
		
		protected bool Validar_dgVehiculos()
		{
			bool estado=false;
			int cont=0;
			for(int i=0;i<dgVehiculos.Items.Count;i++)
			{
				if(!((CheckBox)dgVehiculos.Items[i].Cells[9].Controls[1]).Checked)
					cont++;
				else
				{
					dtVehiculos.Rows[i][9]=true;
					Session["dtVehiculos"]=dtVehiculos;
				}
			}
			if(cont==dgVehiculos.Items.Count)
				estado=true;
			return estado;
		}
		
		protected void btnGenerar_Click(object Sender,EventArgs e)
		{
			if(!Validar_dgVehiculos())
			{
				if(Request.QueryString["error"]==null)
				{
					for(int i=0;i<dtVehiculos.Rows.Count;i++)
					{
						if(Convert.ToBoolean(dtVehiculos.Rows[i][9]))
						{
							if(dtVehiculos.Rows[i][10].ToString()=="V")
							{
								DBFunctions.NonQuery("UPDATE mvehiculo SET test_tipoesta=60,mveh_fechentr='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")+"' WHERE mcat_vin='"+dtVehiculos.Rows[i][3].ToString()+"' AND test_tipoesta=40");
								lb.Text+=DBFunctions.exceptions;							
							}
							else if(dtVehiculos.Rows[i][10].ToString()=="O")
							{
								DBFunctions.NonQuery("UPDATE morden SET test_estado='E',mord_salida='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")+"' WHERE mcat_vin='"+dtVehiculos.Rows[i][3].ToString()+"' AND test_estado='F'");
								lb.Text+=DBFunctions.exceptions;
							}
						}
					}
					if(Request.QueryString["verf"]==null && Request.QueryString["mens"]==null)
						Response.Redirect(indexPage+"?process=Cartera.EmisionOrdenSalidaVehiculo.Reporte");
					else
						Response.Redirect(indexPage+"?process=Cartera.EmisionOrdenSalidaVehiculo.Reporte&verf="+Request.QueryString["verf"]+"&mens="+Request.QueryString["mens"]+"");
				}
				else if(Request.QueryString["error"]!=null)
					    Response.Redirect(indexPage+"?process=Cartera.EmisionOrdenSalidaVehiculo.Reporte&error=V");
			}
			else
                Utils.MostrarAlerta(Response, "Debe escoger por lo menos un vehículo");
		}

		protected void btnCancelar_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(indexPage+"?process=Cartera.EmisionOrdenSalidaVehiculo");
		}
		
		////////////////////////////////////////////////
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{

		}
		#endregion

		
	}
}
