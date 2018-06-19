// created on 13/12/2004 at 12:32
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
using System.Configuration;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Automotriz
{
	public partial class OperacionesPeritaje : System.Web.UI.UserControl
	{
		protected DataGrid []grillasGruposPeritaje;
		protected DataTable []tablasAsociadas;
		protected ArrayList codigosGruposPeritaje;
		protected int cantidadGrillas;
		protected double valorPeritaje;
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				Session["tablasAsociadas"] = "";
				Session["cantidadGrillas"] = "";
				Session["codigosGruposPeritaje"] = "";				
			}
         	this.Construir_Grillas();
			if(Session["cantidadGrillas"]!=null)
				cantidadGrillas = System.Convert.ToInt32(Session["cantidadGrillas"]);
			if(Session["tablasAsociadas"]!=null)
			{
				//Copiamos las tablas que vienen del Session
				tablasAsociadas = new DataTable[cantidadGrillas];
				for(int i=0;i<cantidadGrillas;i++)
				{
					tablasAsociadas[i] = ((DataTable[])Session["tablasAsociadas"])[i].Copy();
				}
			}
			if(Session["codigosGruposPeritaje"]!=null)
				codigosGruposPeritaje = (ArrayList)Session["codigosGruposPeritaje"];
		}		
		
		protected void Validar_Formulario(Object  Sender, EventArgs e)
		{
			bool problema = false;
			valorPeritaje = 0;
			for(int i=0;i<cantidadGrillas;i++)
			{
				if(!this.Validar_Grillas_Peritaje(((DataGrid)gruposPeritaje.Controls[((i*2)+1)])))
					problema = true;				
			}
			if(problema)
			{
                Utils.MostrarAlerta(Response, "Algun Valor es Invalido");
			}				
			else
			{
				//Calculamos el total del peritaje con el iva
				double porcentajeIva = System.Convert.ToDouble(DBFunctions.SingleData("SELECT cemp_porciva FROM cempresa"));
				double iva = valorPeritaje*(porcentajeIva/100);
				costoPeritaje.Text ="SUB-TOTAL : "+valorPeritaje.ToString("C");
				costoPeritaje.Text +="<br>VALOR IVA : "+iva.ToString("C");
				costoPeritaje.Text +="<br>TOTAL : "+(valorPeritaje+iva).ToString("C");
				//En caso de que la validacion haya sido exitosa, debemos activar el boton de grabar
				Control principal = (this.Parent).Parent;
				((Button)principal.FindControl("grabar")).Enabled = true;
                Utils.MostrarAlerta(Response, "Ahora Puede Grabar la Orden de Trabajo. Después de grabar, espere e imprima los formatos.");
			}
		}		
		
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////
		protected void Construir_Grillas()
		{
			codigosGruposPeritaje = new ArrayList();
			DataSet gruposPeritajeConstruccion = new DataSet();
			DBFunctions.Request(gruposPeritajeConstruccion,IncludeSchema.NO,"SELECT * FROM pgrupoperitaje");
			cantidadGrillas = gruposPeritajeConstruccion.Tables[0].Rows.Count;
			Session["cantidadGrillas"] = cantidadGrillas;
			grillasGruposPeritaje = new DataGrid[cantidadGrillas];
			tablasAsociadas = new DataTable[cantidadGrillas];
			for(int i=0;i<cantidadGrillas;i++)
			{
				DataTable tablaAsociada = new DataTable();
				this.Construir_Tabla(tablaAsociada,gruposPeritajeConstruccion.Tables[0].Rows[i][1].ToString());
				DataGrid grilla = new DataGrid();
				grilla.Width = new Unit(700);
				grilla.EnableViewState = true;
				gruposPeritaje.Controls.Add(new LiteralControl("<br><br>"));
				gruposPeritaje.Controls.Add(grilla);
				this.Llenar_Grilla(grilla,tablaAsociada,gruposPeritajeConstruccion.Tables[0].Rows[i][0].ToString(),gruposPeritajeConstruccion.Tables[0].Rows[i][1].ToString());
				DatasToControls.Aplicar_Formato_Grilla(grilla);
				tablasAsociadas[i] = tablaAsociada;
				codigosGruposPeritaje.Add(gruposPeritajeConstruccion.Tables[0].Rows[i][0].ToString());
			}
			Session["tablasAsociadas"] = tablasAsociadas;
			Session["codigosGruposPeritaje"] = codigosGruposPeritaje;
		}
		
		protected void Construir_Tabla(DataTable tabla ,string grupo)
		{
			tabla.Columns.Add(new DataColumn(grupo,System.Type.GetType("System.String")));
			tabla.Columns.Add(new DataColumn("ESTADO",System.Type.GetType("System.String")));
			tabla.Columns.Add(new DataColumn("DETALLE",System.Type.GetType("System.String")));
			tabla.Columns.Add(new DataColumn("COSTO",System.Type.GetType("System.String")));
		}
		
		protected void Llenar_Grilla(DataGrid grilla, DataTable tablaAsociada, string codigoGrupo, string grupo)
		{
			DataSet operacionesPeritaje = new DataSet();
			DBFunctions.Request(operacionesPeritaje,IncludeSchema.NO,"SELECT pitp_descripcion FROM pitemperitaje WHERE pgrp_codigo='"+codigoGrupo+"'");
			for(int i=0;i<operacionesPeritaje.Tables[0].Rows.Count;i++)
			{
				DataRow fila = tablaAsociada.NewRow();
				fila[grupo] = operacionesPeritaje.Tables[0].Rows[i][0].ToString();
				tablaAsociada.Rows.Add(fila);
			}
			grilla.DataSource = tablaAsociada;
			grilla.DataBind();
			for(int i=0;i<grilla.Items.Count;i++)
			{
				RadioButtonList estados = new RadioButtonList();
				estados.RepeatDirection = RepeatDirection.Horizontal;
				TextBox detalle = new TextBox();
				TextBox costo = new TextBox();
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoRadioButtonList(estados,"SELECT tespe_descripcion FROM testadoperitaje ORDER BY tespe_jerar ASC");
				grilla.Items[i].Cells[1].Controls.Add(estados);
				grilla.Items[i].Cells[2].Controls.Add(detalle);
				grilla.Items[i].Cells[3].Controls.Add(costo);				
			}			
		}
		
		protected bool Validar_Grillas_Peritaje(DataGrid grilla)
		{
			bool validacion = true;
			for(int i=0;i<grilla.Items.Count;i++)
			{
				string costo = ((TextBox)grilla.Items[i].Cells[3].Controls[0]).Text;
				if(costo!="")
				{
					if(!DatasToControls.ValidarDouble(costo))
						validacion = false;
					else
						valorPeritaje += System.Convert.ToDouble(costo);
				}				
			}
			return validacion;
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

