using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using System.Web;
using System.Web.UI;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial class ArchivoLista : System.Web.UI.UserControl 
	{
		protected DataTable dtListaArchivo;
		protected DataTable dtUpdate,dtNew,dtError;
        protected DataTable dtNuevos, dtExistentes, dtFallos;
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				Session.Clear();
				lbListaPrecio.Text = DBFunctions.SingleData("SELECT ppre_nombre FROM pprecioitem WHERE ppre_codigo='"+Request.QueryString["codLista"]+"'");
				lnkExportarExcel.Visible=false;
				lnkExportarExcel2.Visible=false;
			}
			if(Session["dtListaArchivo"]==null)
				this.dtListaArchivo = new DataTable();
			else
				this.dtListaArchivo = (DataTable)Session["dtListaArchivo"];
			if(Session["dtUpdate"]==null)
				this.dtUpdate = new DataTable();
			else
				this.dtUpdate = (DataTable)Session["dtUpdate"];
			if(Session["dtNew"]==null)
				this.dtNew = new DataTable();
			else
				this.dtNew = (DataTable)Session["dtNew"];
			if(Session["dtError"]==null)
				this.dtError = new DataTable();
			else
				this.dtError = (DataTable)Session["dtError"];
		}
		
		protected void AceptarArchivo(Object Sender, EventArgs E)
		{
            dtNuevos = dtExistentes = dtFallos = new DataTable();

			string[] files = fDocument.PostedFile.FileName.Split('\\');

            if ((files[files.Length - 1].Split('.'))[1].Trim().ToUpper() == "XLS" || (files[files.Length - 1].Split('.'))[1].Trim().ToUpper() == "XLSX")
			{
				fDocument.PostedFile.SaveAs(ConfigurationManager.AppSettings["PathToImportsExcel"]+files[files.Length-1]);
				
				//Ahora llamamos la clase que me permite traer los datos de una hoja de excel mediante ADO
				ExcelFunctions excelManager = new ExcelFunctions(ConfigurationManager.AppSettings["PathToImportsExcel"]+files[files.Length-1]);
				
				DataSet ds = new DataSet();
				ds = excelManager.Request(ds,IncludeSchema.NO,"SELECT * FROM ListaPrecios");
				
				if(ds.Tables.Count>0)
				{
					lnkExportarExcel.Visible=true;
					lnkExportarExcel2.Visible=true;

                    if((files[files.Length - 1].Split('.'))[1].Trim().ToUpper() == "XLSX")
                    {
                        for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                        {
                            ds.Tables[0].Columns[i].ColumnName = ds.Tables[0].Rows[0][i].ToString();
                        }
                        ds.Tables[0].Rows[0].Delete();
                        ds.Tables[0].Rows[0].AcceptChanges();
                    }
                    else
                    {
                        Utils.MostrarAlerta(Response, "El formato del excel no puede ser diferente de la versión 2010 o más reciente (.xlsx)");
                        return;
                    }

                    if (ds.Tables[0].Columns.Count == 3)
					{
						Session["dtListaArchivo"] = ds.Tables[0];

						ListaPrecios listaActualizar = new ListaPrecios(Request.QueryString["codLista"]);
                        listaActualizar.ds = ds;
                        string rta;
                        rta = listaActualizar.ActualizarListaArchivo(ds.Tables[0], ref dtExistentes, ref dtNuevos, ref dtFallos, false);
                        if(rta.Length > 0)
                        {
                            Utils.MostrarAlerta(Response, rta);
                            return;
                        }

                        //listaActualizar.ActualizarListaArchivo(ds.Tables[0],ref dtUpdate, ref dtNew, ref dtError, false); //método viejo


                        plUpload.Visible = false;
						plResultado.Visible = true;

                        dgUpdate.DataSource = dtExistentes;
                        Session["dtUpdate"] = dtExistentes;
                        dgUpdate.DataBind();

                        dgError.DataSource = dtFallos;
                        Session["dtError"] = dtFallos;
                        dgError.DataBind();

                        dgNew.DataSource = dtNuevos;
                        Session["dtNew"] = dtNuevos;
                        dgNew.DataBind();

                        //dgUpdate.DataSource = dtUpdate;
                        //Session["dtUpdate"] = dtUpdate;
                        //dgError.DataSource = dtError;
                        //Session["dtError"] = dtError;
                        //dgUpdate.DataBind();




                        DatasToControls.JustificacionGrilla(dgUpdate, dtExistentes);
                        DatasToControls.JustificacionGrilla(dgError, dtFallos);
                        DatasToControls.JustificacionGrilla(dgNew, dtNuevos);

                        //DatasToControls.JustificacionGrilla(dgUpdate,dtUpdate);
                        //DatasToControls.JustificacionGrilla(dgError,dtError);
                    }
                    else
                        Utils.MostrarAlerta(Response, "El archivo no tiene el número de columnas indicado. Revise Por Favor!");
						
				}
				else
                    Utils.MostrarAlerta(Response, "No se ha podido cargar el archivo, no se ha dado el nombre correcto al rango de celdas. Revise Por Favor!");
					
			}
			else
                Utils.MostrarAlerta(Response, "Tipo de Archivo Invalido!");
				
		}

		protected void lnkExportarExcel_Click(object sender, System.EventArgs e)
		{
			Response.Clear();
			Response.AddHeader("content-disposition", "attachment;filename=listaInv.xls");
			Response.Charset = "Unicode";
			Response.Cache.SetCacheability(HttpCacheability.NoCache);
			Response.ContentType = "application/vnd.xls";
			System.IO.StringWriter stringWrite = new System.IO.StringWriter();
			System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
			dgUpdate.RenderControl(htmlWrite);
			Response.Write(stringWrite.ToString());
			Response.End();
		}
		
		protected void lnkExportarExcel_Click2(object sender, System.EventArgs e)
		{
			Response.Clear();
			Response.AddHeader("content-disposition", "attachment;filename=listaInvErrados.xls");
			Response.Charset = "Unicode";
			Response.Cache.SetCacheability(HttpCacheability.NoCache);
			Response.ContentType = "application/vnd.xls";
			System.IO.StringWriter stringWrite = new System.IO.StringWriter();
			System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
			dgError.RenderControl(htmlWrite);
			Response.Write(stringWrite.ToString());
			Response.End();
		}


		protected void RealizarProceso(Object Sender, EventArgs E)
		{
			ListaPrecios listaActualizar = new ListaPrecios(Request.QueryString["codLista"]);

			DataTable dtUpdate = dtNew = dtError = new DataTable();
            /*
             * Así es como estaba
             * 
             * if (listaActualizar.ActualizarListaArchivo((DataTable)Session["dtListaArchivo"], ref dtUpdate, ref dtNew, ref dtError, true))
                Response.Redirect("" + indexPage + "?process=Inventarios.ListaPrecios");
			   lb.Text += "<br> Mensaje :" + listaActualizar.ProcessMsg;

             */

            /*
             * nuevo metodo aqui
             */
             //Esta tabla tiene TODOS los elementos
            //DataTable tablaListasTodos = (DataTable)Session["dtListaArchivo"];

            //Esta tabla tienes los elementos que van con error(Los que no se actualizarían)
            //DataTable tablaListasFallos = (DataTable)Session["dtError"];

            //Esta tabla tiene los elementos válidos ya existentes en la tabla mprecioitem
            DataTable tablaListaActualiza = (DataTable)Session["dtUpdate"];

            //Esta tabla tiene los elementos válidos nuevos en la tabla mprecioitem
            DataTable tablaListaNuevos = (DataTable)Session["dtNew"];

            string rta = listaActualizar.ActualizarPrecios(tablaListaActualiza, tablaListaNuevos, true);
            if(rta.ToLower() == "ok")
            {
                Session.Clear();
                Response.Redirect(indexPage + "?process=Inventarios.ListaPrecios&hecho=1");
            }
            else
                lb.Text = rta;
        }

        protected void DgUpdate_Page(Object sender, DataGridPageChangedEventArgs e)
		{
			this.dgUpdate.CurrentPageIndex = e.NewPageIndex;
	 		this.dgUpdate.DataSource = dtUpdate;
	 		this.dgUpdate.DataBind();
	 		DatasToControls.JustificacionGrilla(dgUpdate,dtUpdate);
 		}
		
		protected void DgNew_Page(Object sender, DataGridPageChangedEventArgs e)
		{
			this.dgNew.CurrentPageIndex = e.NewPageIndex;
	 		this.dgNew.DataSource = dtNew;
	 		this.dgNew.DataBind();
	 		DatasToControls.JustificacionGrilla(dgNew,dtNuevos);
 		}
		
		protected void DgError_Page(Object sender, DataGridPageChangedEventArgs e)
		{
			this.dgError.CurrentPageIndex = e.NewPageIndex;
	 		this.dgError.DataSource = dtError;
	 		this.dgError.DataBind();
	 		DatasToControls.JustificacionGrilla(dgError,dtError);
 		}
		
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
