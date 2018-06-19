// created on 02/12/2004 at 17:17
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
using AMS.DB;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Automotriz
{
	public partial class ProgramacionCitas : System.Web.UI.UserControl
	{
		protected DataTable tablaConsulta;
				
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				//bind.PutDatasIntoDropDownList(taller,"SELECT palm_descripcion FROM palmacen WHERE talm_tipoalma='T' ORDER BY palm_almacen ASC");
                bind.PutDatasIntoDropDownList(taller, "SELECT palm_descripcion FROM palmacen pa where (pa.pcen_centtal is not null  or pcen_centcoli is not null) and pa.tvig_vigencia='V' order by pa.PALM_DESCRIPCION;");
				bind.PutDatasIntoDropDownList(tipoOrden,"SELECT remarks FROM sysibm.syscolumns WHERE tbcreator='DBXSCHEMA' AND tbname='MCITATALLER'");
				DatasToControls.EstablecerDefectoDropDownList(tipoOrden,"Hora de la cita");
				fechaInicial.SelectedDate = DateTime.Now;
				fechaFinal.SelectedDate = DateTime.Now;
			}
			this.Construccion_Controles_Consulta_Avanzada();			
		}		
		
		
		protected void Realizar_Consulta(Object sender, EventArgs e)
		{
			string sentenciaSelect = "SELECT MC.* FROM mcitataller MC ";
			//Antes de empezar a realizar la consulta debemos verificar las fechas
			DateTime fechaInicialConsulta = fechaInicial.SelectedDate;
			DateTime fechaFinalConsulta = fechaFinal.SelectedDate;
			if(fechaFinalConsulta>=fechaInicialConsulta)
			{
				if(fechaFinalConsulta>fechaInicialConsulta)
                    sentenciaSelect += ", pvendedor PV, palmacen PAL WHERE pv.pven_vigencia='V' and MC.mcit_codven = PV.pven_codigo AND PV.palm_almacen=PAL.palm_almacen AND MC.mcit_fecha>'" + fechaInicialConsulta.Date.ToString("yyyy-MM-dd") + "' AND MC.mcit_fecha<'" + fechaFinalConsulta.Date.ToString("yyyy-MM-dd") + "' ";
				else
                    sentenciaSelect += ", pvendedor PV, palmacen PAL WHERE pv.pven_vigencia='V' and MC.mcit_codven = PV.pven_codigo AND PV.palm_almacen=PAL.palm_almacen AND MC.mcit_fecha='" + fechaInicialConsulta.Date.ToString("yyyy-MM-dd") + "' ";
					//Ahora determinador el orden
                sentenciaSelect += "AND PAL.palm_almacen='" + DBFunctions.SingleData("SELECT palm_almacen FROM palmacen WHERE tvig_vigencia='V' and palm_descripcion='" + taller.SelectedItem.ToString() + "'") + "' ORDER BY MC." + (DBFunctions.SingleData("SELECT name FROM sysibm.syscolumns WHERE remarks='" + tipoOrden.SelectedItem.ToString().Trim() + "'")).Trim() + " " + formaOrden.SelectedValue + "";				
				this.Consutar(sentenciaSelect);
			}
			else
                Utils.MostrarAlerta(Response, "La fecha final debe ser mayor o igual que la inicial para poder realizar la consulta");
			
		}
		
		protected void Realizar_Consulta_Avanzada(Object sender, EventArgs e)
		{
			bool error = false;
			string sentenciaSelect = "SELECT MC.* FROM mcitataller MC ";
			int i, cantidadCondicionales=0;
			DataSet foraneas = new DataSet();
			DBFunctions.Request(foraneas,IncludeSchema.NO,"SELECT fkcolnames, reftbname, pkcolnames FROM sysibm.sysrels WHERE tbname='MCITATALLER' AND creator='DBXSCHEMA'");
			DataSet columnas = new DataSet();
			DBFunctions.Request(columnas,IncludeSchema.NO,"SELECT name , remarks FROM sysibm.syscolumns WHERE tbcreator='DBXSCHEMA' AND tbname='MCITATALLER'");
			string condicionales = "";
			for(i=0;i<columnas.Tables[0].Rows.Count;i++)
			{
				if(((CheckBox)((TableCell)((TableRow)((Table)controlesConsulta.Controls[0]).Controls[i]).Controls[2]).Controls[0]).Checked == true)
				{
					if(cantidadCondicionales==0)
					{
						condicionales = "WHERE ";
						cantidadCondicionales += 1;
					}
					//el caso del textbox
					if(((TableCell)((TableRow)((Table)controlesConsulta.Controls[0]).Controls[i]).Controls[1]).Controls[0].GetType().ToString() == "System.Web.UI.WebControls.TextBox")
					{
						if(((TextBox)((TableCell)((TableRow)((Table)controlesConsulta.Controls[0]).Controls[i]).Controls[1]).Controls[0]).Text == "")
						{
							error = true;
                            Utils.MostrarAlerta(Response, "El Campo de Text Correspondiente a " + ((Label)((TableCell)((TableRow)((Table)controlesConsulta.Controls[0]).Controls[i]).Controls[0]).Controls[0]).Text + " esta vacio");
						}
						else
						{
							if(cantidadCondicionales==1)
							{
								condicionales+="MC."+(DBFunctions.SingleData("SELECT name FROM sysibm.syscolumns WHERE remarks='"+((Label)((TableCell)((TableRow)((Table)controlesConsulta.Controls[0]).Controls[i]).Controls[0]).Controls[0]).Text.Trim()+"'")).Trim()+"='"+((TextBox)((TableCell)((TableRow)((Table)controlesConsulta.Controls[0]).Controls[i]).Controls[1]).Controls[0]).Text+"'";
								cantidadCondicionales+=1;
							}										
							else
								condicionales+=" AND MC."+(DBFunctions.SingleData("SELECT name FROM sysibm.syscolumns WHERE remarks='"+((Label)((TableCell)((TableRow)((Table)controlesConsulta.Controls[0]).Controls[i]).Controls[0]).Controls[0]).Text.Trim()+"'")).Trim()+"='"+((TextBox)((TableCell)((TableRow)((Table)controlesConsulta.Controls[0]).Controls[i]).Controls[1]).Controls[0]).Text+"'";
						}									
					}
					//el caso del dropdownlist
					else if(((TableCell)((TableRow)((Table)controlesConsulta.Controls[0]).Controls[i]).Controls[1]).Controls[0].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
					{
						int ubicacionForanea = this.Buscar_Posicion_Foranea((DBFunctions.SingleData("SELECT name FROM sysibm.syscolumns WHERE remarks='"+((Label)((TableCell)((TableRow)((Table)controlesConsulta.Controls[0]).Controls[i]).Controls[0]).Controls[0]).Text.Trim()+"'")),foraneas);
						string nombreCampo1 = DBFunctions.SingleData("SELECT name FROM sysibm.syscolumns WHERE tbcreator='DBXSCHEMA' AND tbname='"+foraneas.Tables[0].Rows[ubicacionForanea][1].ToString().Trim()+"' AND colno=0");
						string nombreCampo2 = DBFunctions.SingleData("SELECT name FROM sysibm.syscolumns WHERE tbcreator='DBXSCHEMA' AND tbname='"+foraneas.Tables[0].Rows[ubicacionForanea][1].ToString().Trim()+"' AND colno=1");
						string nombreCampoBusqueda = (DBFunctions.SingleData("SELECT name FROM sysibm.syscolumns WHERE remarks='"+((Label)((TableCell)((TableRow)((Table)controlesConsulta.Controls[0]).Controls[i]).Controls[0]).Controls[0]).Text.Trim()+"'")).Trim();
						if(nombreCampoBusqueda=="PCAT_CODIGO")
						{
							nombreCampo2 = nombreCampo1;
						}
						if(cantidadCondicionales==1)
						{
							condicionales+=" MC."+nombreCampoBusqueda+"='"+((DropDownList)((TableCell)((TableRow)((Table)controlesConsulta.Controls[0]).Controls[i]).Controls[1]).Controls[0]).SelectedValue+"'";
							cantidadCondicionales+=1;									
						}
						else
						{
							condicionales+=" AND MC."+nombreCampoBusqueda+"='"+((DropDownList)((TableCell)((TableRow)((Table)controlesConsulta.Controls[0]).Controls[i]).Controls[1]).Controls[0]).SelectedValue+"'";
						}										
					}
				}
			}
			//Aqui terminamos de construir el select de la busqueda avanzada
			if(cantidadCondicionales!=0)
			{
				sentenciaSelect +=condicionales;
			}
			if(!error)
				this.Consutar(sentenciaSelect);
		}
		
		protected void Consutar(string sentenciaSelect)
		{
			DataSet consultaFinal = new DataSet();
			DBFunctions.Request(consultaFinal,IncludeSchema.NO,sentenciaSelect);
			this.LLenar_Grilla_Consulta(consultaFinal);
		}
		
		protected void Construccion_Controles_Consulta_Avanzada()
		{
			int i;
			Table tableControls = new Table();
			tableControls.BorderWidth = new Unit("0");
			//Aqui vamos a traer las llaves foraneas de mcitataller
			DataSet foraneas = new DataSet();
			DBFunctions.Request(foraneas,IncludeSchema.NO,"SELECT fkcolnames, reftbname, pkcolnames FROM sysibm.sysrels WHERE tbname='MCITATALLER' AND creator='DBXSCHEMA'");
			DataSet columnas = new DataSet();
			DBFunctions.Request(columnas,IncludeSchema.NO,"SELECT name , remarks FROM sysibm.syscolumns WHERE tbcreator='DBXSCHEMA' AND tbname='MCITATALLER'");
			for(i=0;i<columnas.Tables[0].Rows.Count;i++)
			{
				TableRow r = new TableRow();
				TableCell c1 = new TableCell();
				TableCell c2 = new TableCell();
				TableCell c3 = new TableCell();
				Label nombreColumna = new Label();
				nombreColumna.Text = columnas.Tables[0].Rows[i][1].ToString()+" ";
				c1.Controls.Add(nombreColumna);
				TextBox opcionUno = new TextBox();
				DropDownList opcionDos = new DropDownList();
				CheckBox utilizacion = new CheckBox();
				utilizacion.Text = " Desea Utilizar este Parametro:  Si?";
				utilizacion.TextAlign = TextAlign.Left;				
				if(this.Buscar_Foranea(columnas.Tables[0].Rows[i][0].ToString(),foraneas))
				{
					//Ahora vamos a cargar los datos de la tabla a la cual hace referencia
					DatasToControls bind = new DatasToControls();
					//Cambio provisional
					if((columnas.Tables[0].Rows[i][0].ToString().Trim())=="PCAT_CODIGO")
						bind.PutDatasIntoDropDownList(opcionDos,"SELECT pcat_codigo FROM "+foraneas.Tables[0].Rows[this.Buscar_Posicion_Foranea(columnas.Tables[0].Rows[i][0].ToString(),foraneas)][1].ToString().Trim()+"");
					else
						bind.PutDatasIntoDropDownList(opcionDos,"SELECT * FROM "+foraneas.Tables[0].Rows[this.Buscar_Posicion_Foranea(columnas.Tables[0].Rows[i][0].ToString(),foraneas)][1].ToString().Trim()+"");
					c2.Controls.Add(opcionDos);
				}					
				else
					c2.Controls.Add(opcionUno);
				c3.Controls.Add(utilizacion);
				r.Cells.Add(c1);
				r.Cells.Add(c2);
				r.Cells.Add(c3);
				c1.Width = new Unit("33%");
				c2.Width = new Unit("33%");
				c3.Width = new Unit("33%");
				tableControls.Rows.Add(r);
			}
			controlesConsulta.Controls.Add(tableControls);
			Session["controlesConsulta"] = controlesConsulta;
		}
		
		
		protected bool Buscar_Foranea (string nombreColumna, DataSet foraneas)
		{
			bool encontrado = false;
			int i;
			for(i=0;i<foraneas.Tables[0].Rows.Count;i++)
			{
				if((foraneas.Tables[0].Rows[i][0].ToString().Trim())==nombreColumna.Trim())
					encontrado = true;									
			}
			return encontrado;
		}
		
		protected int Buscar_Posicion_Foranea (string nombreColumna, DataSet foraneas)
		{
			int posicionForanea = 0;
			int i;
			for(i=0;i<foraneas.Tables[0].Rows.Count;i++)
			{
				if((foraneas.Tables[0].Rows[i][0].ToString().Trim())==nombreColumna.Trim())
					posicionForanea = i;
			}
			return posicionForanea;
		}	
		
		protected void Preparar_Tabla_Consulta()
		{
			tablaConsulta = new DataTable();
			tablaConsulta.Columns.Add(new DataColumn("FECHA",System.Type.GetType("System.String")));
			tablaConsulta.Columns.Add(new DataColumn("HORA",System.Type.GetType("System.String")));
			tablaConsulta.Columns.Add(new DataColumn("RECEPCIONISTA",System.Type.GetType("System.String")));
			tablaConsulta.Columns.Add(new DataColumn("CATALOGO",System.Type.GetType("System.String")));
			tablaConsulta.Columns.Add(new DataColumn("PLACA",System.Type.GetType("System.String")));
			tablaConsulta.Columns.Add(new DataColumn("NOMBCLIENTE",System.Type.GetType("System.String")));
			tablaConsulta.Columns.Add(new DataColumn("TELEFONO",System.Type.GetType("System.String")));
			tablaConsulta.Columns.Add(new DataColumn("MOVIL",System.Type.GetType("System.String")));
			tablaConsulta.Columns.Add(new DataColumn("CORREO",System.Type.GetType("System.String")));
			tablaConsulta.Columns.Add(new DataColumn("SERVICIO",System.Type.GetType("System.String")));
			tablaConsulta.Columns.Add(new DataColumn("ESTCITA",System.Type.GetType("System.String")));
		}
		
		protected void LLenar_Grilla_Consulta(DataSet consulta)
		{
			this.Preparar_Tabla_Consulta();
			for(int i=0;i<consulta.Tables[0].Rows.Count;i++)
			{
				DataRow fila = tablaConsulta.NewRow();
				fila["FECHA"] = (System.Convert.ToDateTime(consulta.Tables[0].Rows[i][0].ToString())).Date.ToString("yyyy-MM-dd");
				fila["HORA"] = consulta.Tables[0].Rows[i][1].ToString(); 
				fila["RECEPCIONISTA"] = DBFunctions.SingleData("SELECT pven_nombre FROM pvendedor WHERE pven_codigo='"+consulta.Tables[0].Rows[i][2].ToString()+"'");
				fila["CATALOGO"] = consulta.Tables[0].Rows[i][3].ToString(); 
				fila["PLACA"] = consulta.Tables[0].Rows[i][4].ToString(); 
				fila["NOMBCLIENTE"] = consulta.Tables[0].Rows[i][5].ToString(); 
				fila["TELEFONO"] = consulta.Tables[0].Rows[i][6].ToString();
				fila["MOVIL"] = consulta.Tables[0].Rows[i][7].ToString();
				fila["CORREO"] = consulta.Tables[0].Rows[i][8].ToString();
                fila["SERVICIO"] = DBFunctions.SingleData("SELECT pkit_nombre FROM pkit WHERE PKIT_VIGENCIA = 'V' and pkit_codigo='" + consulta.Tables[0].Rows[i][9].ToString() + "'");
				fila["ESTCITA"] = DBFunctions.SingleData("SELECT testcit_desccita FROM testadocita WHERE testcit_estacita='"+consulta.Tables[0].Rows[i][10].ToString()+"'");
				tablaConsulta.Rows.Add(fila);
			}
			resultadoConsulta.DataSource = tablaConsulta;
			resultadoConsulta.DataBind();
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
