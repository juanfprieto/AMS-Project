using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using AMS.Forms;
using AMS.DB;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial class ReversarPreRecepcion : System.Web.UI.UserControl
	{
		#region Atributos
		protected DataTable dtItemsRDir;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        string prefijoPreRecep = "";
        int numeroPreRecep = 0;

        #endregion

        #region Eventos
        protected void Page_Load(object sender, System.EventArgs e)
		{
            if (Session["dtItemsRDir"] == null || !IsPostBack)
            {
                if (!IsPostBack)
                {
                    Session.Remove("dtItemsRDir");
                }
                this.PrepararDtItemsRDir();
            }
            else
                dtItemsRDir = (DataTable)Session["dtItemsRDir"];
		}
		
		protected void CargarNit(Object Sender, EventArgs E)
		{
			//Revisamos que el nit ingresado sea un nit de proveedor 
			if(DBFunctions.RecordExist("SELECT * FROM mproveedor WHERE mnit_nit='"+tbNit.Text.Trim()+"'"))
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlPreRecep,"SELECT MPR.pdoc_codigo CONCAT '-' CONCAT CAST(MPR.mpre_numero AS CHARACTER(10)) FROM mprerecepcion MPR, ditems DIT WHERE MPR.pdoc_codigo = DIT.pdoc_codigo AND MPR.mpre_numero = DIT.dite_numedocu AND MPR.mpre_nitprov = '"+tbNit.Text.Trim()+"' GROUP BY MPR.pdoc_codigo CONCAT '-' CONCAT CAST(MPR.mpre_numero AS CHARACTER(10)) HAVING SUM(DIT.dite_cantdevo)<=0");
				if(ddlPreRecep.Items.Count>0)
					plPreRecep.Visible = true;
				else
				{
					plPreRecep.Visible = false;
                    Utils.MostrarAlerta(Response, "No existe ninguna pre-recepción disponible para reversar del nit " + tbNit.Text.Trim() + "");
				}
			}
			else
                Utils.MostrarAlerta(Response, "El nit " + tbNit.Text.Trim() + " no corresponde a un proveedor.\\nRevisar Por Favor");
		}
		
		protected void CargarPreRecepcion(Object Sender, EventArgs E)
		{
            
            if (ddlPreRecep.SelectedValue.Split('-').Length == 3)
            {
                prefijoPreRecep = (ddlPreRecep.SelectedValue.Split('-'))[0] + "-" + (ddlPreRecep.SelectedValue.Split('-'))[1];
                numeroPreRecep = Convert.ToInt32((ddlPreRecep.SelectedValue.Split('-'))[2]);

            }
            else
            {
                prefijoPreRecep = (ddlPreRecep.SelectedValue.Split('-'))[0] ;
                numeroPreRecep = Convert.ToInt32((ddlPreRecep.SelectedValue.Split('-'))[1]);
            }

            ViewState["prefijoPreRecep"] = prefijoPreRecep;
            ViewState["numeroPreRecep"] = numeroPreRecep;

            //Traemos los items que se encuentran dentro de la prerecepcion
            DataSet ds = new DataSet();
            DBFunctions.Request(ds, IncludeSchema.NO,
                 @"SELECT * FROM (
                     SELECT DBXSCHEMA.EDITARREFERENCIAS(DIT.mite_codigo,PLIN.plin_tipo), MIT.mite_nombre, MIT.plin_codigo, DIT.dite_cantidad - COALESCE(DIL.DITE_CANTIDAD,0) AS CANTIDAD, DIT.dite_valounit, 
                            DIT.dite_porcdesc, DIT.piva_porciva, DIT.mite_codigo, DIT.palm_almacen, DIT.DITE_PREFDOCUREFE, DIT.DITE_NUMEDOCUREFE 
                       FROM mitems MIT, plineaitem PLIN, ditems DIT
                      	    LEFT JOIN DITEMS DIL ON DIT.PDOC_CODIGO = DIL.DITE_PREFDOCUREFE AND DIT.DITE_NUMEDOCU = DIL.DITE_NUMEDOCUREFE AND DIT.MITE_CODIGO = DIL.MITE_CODIGO AND DIL.TMOV_TIPOMOVI = 11 
                      WHERE DIT.mite_codigo = MIT.mite_codigo AND DIT.pdoc_codigo = '" + prefijoPreRecep + @"' AND DIT.dite_numedocu = " + numeroPreRecep + @" AND DIT.TMOV_TIPOMOVI = 10 AND MIT.plin_codigo=PLIN.plin_codigo 
                      ORDER BY MIT.plin_codigo, DIT.mite_codigo) AS A WHERE CANTIDAD > 0;");
            //    "SELECT DBXSCHEMA.EDITARREFERENCIAS(DIT.mite_codigo,PLIN.plin_tipo), MIT.mite_nombre, MIT.plin_codigo, DIT.dite_cantidad, DIT.dite_valounit, DIT.dite_porcdesc, DIT.piva_porciva, DIT.mite_codigo, DIT.palm_almacen FROM ditems DIT, mitems MIT, plineaitem PLIN WHERE DIT.mite_codigo = MIT.mite_codigo AND DIT.pdoc_codigo = '"+prefijoPreRecep+"' AND DIT.dite_numedocu = "+numeroPreRecep+" AND MIT.plin_codigo=PLIN.plin_codigo ORDER BY MIT.plin_codigo, DIT.mite_codigo");
			//Empezamos a recorrer el dataset y a agregar los items que van hacer reversados
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				DataRow fila = dtItemsRDir.NewRow();
				fila[0] = ds.Tables[0].Rows[i][0].ToString();
				fila[1] = ds.Tables[0].Rows[i][1].ToString();
				fila[2] = ds.Tables[0].Rows[i][2].ToString();
				fila[3] = Convert.ToDouble(ds.Tables[0].Rows[i][3]);
				fila[4] = Convert.ToDouble(ds.Tables[0].Rows[i][4]);
				fila[5] = Convert.ToDouble(ds.Tables[0].Rows[i][5]);
				fila[6] = Convert.ToDouble(ds.Tables[0].Rows[i][6]);
				fila[7] = Total(Convert.ToDouble(ds.Tables[0].Rows[i][4]),Convert.ToDouble(ds.Tables[0].Rows[i][3]),Convert.ToDouble(ds.Tables[0].Rows[i][5]),Convert.ToDouble(ds.Tables[0].Rows[i][6]));
				fila[8] = ds.Tables[0].Rows[i][7].ToString();
				fila[9] = ds.Tables[0].Rows[i][8].ToString();
                fila[10] = ds.Tables[0].Rows[i][9].ToString();
                fila[11] = ds.Tables[0].Rows[i][10].ToString();
                dtItemsRDir.Rows.Add(fila);
			}
			BindDatas();
		}
		
		protected void ReversarPreRecep(Object Sender, EventArgs E)
		{
			ArrayList sqlStrings = new ArrayList();
			string ano_cinv = DBFunctions.SingleData("SELECT pano_ano from cinventario");
			//Cuando Reversamos la prerecepcion debemos modificar los saldos de almacen y los saldos de item en general
			//de cada item
			for(int i=0;i<dtItemsRDir.Rows.Count;i++)
			{
				string almacen = dtItemsRDir.Rows[i][9].ToString();
                string prefijoPedido = "";
                string numeroPedido="";

                //Modificacion en msaldoitem y msaldoitemalmacen
                sqlStrings.Add("UPDATE msaldoitem SET msal_cantactual=msal_cantactual-" + dtItemsRDir.Rows[i][3].ToString() + "  WHERE mite_codigo='" + dtItemsRDir.Rows[i][8].ToString() + "' AND pano_ano=" + ano_cinv + "");
                sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_cantactual=msal_cantactual-" + dtItemsRDir.Rows[i][3].ToString() + " WHERE mite_codigo='" + dtItemsRDir.Rows[i][8].ToString() + "' AND pano_ano=" + ano_cinv + " AND palm_almacen='" + almacen + "'");

                prefijoPedido = dtItemsRDir.Rows[i][10].ToString();
                numeroPedido = dtItemsRDir.Rows[i][11].ToString();

                if (ddlPreRecep.SelectedValue.Split('-').Length == 3)
                {
                   
                    //Ahora revisamos si existe un pedido a proveedor registrado

                    // string prefijoPedido = (ddlPreRecep.SelectedValue.Split('-'))[0] + "-" + (ddlPreRecep.SelectedValue.Split('-'))[1] ;
                    // prefijoPedido = DBFunctions.SingleData("SELECT dite_prefdocurefe FROM ditems WHERE pdoc_codigo='" + (ddlPreRecep.SelectedValue.Split('-'))[0] + "-" + (ddlPreRecep.SelectedValue.Split('-'))[1] + "' AND dite_numedocu=" + (ddlPreRecep.SelectedValue.Split('-'))[2] + " AND mite_codigo='" + dtItemsRDir.Rows[i][8].ToString() + "'");
                    //string  numeroPedido= (ddlPreRecep.SelectedValue.Split('-'))[2];
                    // numeroPedido = DBFunctions.SingleData("SELECT DITE_NUMEDOCU FROM ditems WHERE pdoc_codigo='" + (ddlPreRecep.SelectedValue.Split('-'))[0] + "-" + (ddlPreRecep.SelectedValue.Split('-'))[1] + "' AND dite_numedocu=" + (ddlPreRecep.SelectedValue.Split('-'))[2] + " AND mite_codigo='" + dtItemsRDir.Rows[i][8].ToString() + "'");
                    //Si existe modificamos los registros del dpedidoitem 
                    if (DBFunctions.RecordExist("SELECT * FROM dpedidoitem WHERE pped_codigo='" + prefijoPedido + "' AND mped_numepedi=" + numeroPedido + " AND mite_codigo='" + dtItemsRDir.Rows[i][8].ToString() + "'"))
                        sqlStrings.Add("UPDATE dpedidoitem SET dped_cantasig=dped_cantasig-" + dtItemsRDir.Rows[i][3].ToString() + " WHERE pped_codigo='" + prefijoPedido + "' AND mped_numepedi=" + numeroPedido + " AND mite_codigo='" + dtItemsRDir.Rows[i][8].ToString() + "'");
                    //Ahora eliminamos todos los registros de la prerecepcion
                    //Es necesario recalcular el costopromedio ????????????????

                    sqlStrings.Add("DELETE FROM ditems WHERE pdoc_codigo='" + (ddlPreRecep.SelectedValue.Split('-'))[0] + "-" + (ddlPreRecep.SelectedValue.Split('-'))[1] + "' AND dite_numedocu=" + (ddlPreRecep.SelectedValue.Split('-'))[2] + " AND mite_codigo='" + dtItemsRDir.Rows[i][8].ToString() + "'");
                }
                else
                {
                    
                    //Ahora revisamos si existe un pedido a proveedor registrado

                    // string prefijoPedido = (ddlPreRecep.SelectedValue.Split('-'))[0] + "-" + (ddlPreRecep.SelectedValue.Split('-'))[1] ;
                    //prefijoPedido = DBFunctions.SingleData("SELECT dite_prefdocurefe FROM ditems WHERE pdoc_codigo='" + (ddlPreRecep.SelectedValue.Split('-'))[0] +  "' AND dite_numedocu=" + (ddlPreRecep.SelectedValue.Split('-'))[1] + " AND mite_codigo='" + dtItemsRDir.Rows[i][8].ToString() + "'");
                    //string  numeroPedido= (ddlPreRecep.SelectedValue.Split('-'))[2];
                    //numeroPedido = DBFunctions.SingleData("SELECT DITE_NUMEDOCU FROM ditems WHERE pdoc_codigo='" + (ddlPreRecep.SelectedValue.Split('-'))[0] +  "' AND dite_numedocu=" + (ddlPreRecep.SelectedValue.Split('-'))[1] + " AND mite_codigo='" + dtItemsRDir.Rows[i][8].ToString() + "'");
                    //Si existe modificamos los registros del dpedidoitem 
                    if (DBFunctions.RecordExist("SELECT * FROM dpedidoitem WHERE pped_codigo='" + prefijoPedido + "' AND mped_numepedi=" + numeroPedido + " AND mite_codigo='" + dtItemsRDir.Rows[i][8].ToString() + "'"))
                        sqlStrings.Add("UPDATE dpedidoitem SET dped_cantasig=dped_cantasig-" + dtItemsRDir.Rows[i][3].ToString() + " WHERE pped_codigo='" + prefijoPedido + "' AND mped_numepedi=" + numeroPedido + " AND mite_codigo='" + dtItemsRDir.Rows[i][8].ToString() + "'");
                    //Ahora eliminamos todos los registros de la prerecepcion
                    //Es necesario recalcular el costopromedio ????????????????

                    sqlStrings.Add("DELETE FROM ditems WHERE pdoc_codigo='" + (ddlPreRecep.SelectedValue.Split('-'))[0] +  "' AND dite_numedocu=" + (ddlPreRecep.SelectedValue.Split('-'))[1] + " AND mite_codigo='" + dtItemsRDir.Rows[i][8].ToString() + "'");

                }
            }
            prefijoPreRecep = ViewState["prefijoPreRecep"].ToString();
            numeroPreRecep  = Convert.ToInt16(ViewState["numeroPreRecep"].ToString());

            sqlStrings.Add("DELETE FROM MPRERECEPCION WHERE pdoc_codigo='" + prefijoPreRecep + "' AND MPRE_numeRO =" + numeroPreRecep + ";");

            //Ahora eliminamos el registro de mprerecepcion
            //sqlStrings.Add("DELETE FROM mprerecepcion WHERE pdoc_codigo='" + (ddlPreRecep.SelectedValue.Split('-'))[0] + "-" + (ddlPreRecep.SelectedValue.Split('-'))[1] + "' AND mpre_numero=" + (ddlPreRecep.SelectedValue.Split('-'))[2] + "");
            //Actualización Base de datos
            if (DBFunctions.Transaction(sqlStrings)){
				//lb.Text += "<br>bien una chimba"+DBFunctions.exceptions;
               
                Response.Redirect("" + indexPage + "?process=Inventarios.ReversarPrerecepciones");
            }
           
			else
				lb.Text += "<br>Error :"+DBFunctions.exceptions;
		}
		
		#endregion
		
		#region Metodos
		protected void PrepararDtItemsRDir()
		{
			dtItemsRDir = new DataTable();
			dtItemsRDir.Columns.Add(new DataColumn("mite_codigo",System.Type.GetType("System.String")));//0
			dtItemsRDir.Columns.Add(new DataColumn("mite_nombre",System.Type.GetType("System.String")));//1
			dtItemsRDir.Columns.Add(new DataColumn("plin_codigo",System.Type.GetType("System.String")));//2
			dtItemsRDir.Columns.Add(new DataColumn("mite_canting",System.Type.GetType("System.Double")));//3
			dtItemsRDir.Columns.Add(new DataColumn("mite_precio",System.Type.GetType("System.Double")));//4
			dtItemsRDir.Columns.Add(new DataColumn("mite_desc",System.Type.GetType("System.Double")));//5
			dtItemsRDir.Columns.Add(new DataColumn("mite_iva",System.Type.GetType("System.Double")));//6
			dtItemsRDir.Columns.Add(new DataColumn("mite_tot",System.Type.GetType("System.Double")));//7
			dtItemsRDir.Columns.Add(new DataColumn("mite_codiori",System.Type.GetType("System.String")));//8
			dtItemsRDir.Columns.Add(new DataColumn("palm_almacen",System.Type.GetType("System.String")));//9
            dtItemsRDir.Columns.Add(new DataColumn("pped_codigo", System.Type.GetType("System.String")));//10
            dtItemsRDir.Columns.Add(new DataColumn("mped_numepedi", System.Type.GetType("System.String")));//11
        }
		
		protected void BindDatas()
		{
			Session["dtItemsRDir"] = dtItemsRDir;
			dgItemsRDir.DataSource = dtItemsRDir;
			dgItemsRDir.DataBind();
			DatasToControls.JustificacionGrilla(dgItemsRDir,dtItemsRDir);
			if(dtItemsRDir.Rows.Count>0)
			{
				btnReversar.Visible = true;
				tbNit.Enabled = tbNita.Enabled = ddlPreRecep.Enabled = btnSelecNIT.Enabled = btnCargar.Enabled = false;
			}
			else
			{
				btnReversar.Visible = false;
				tbNit.Enabled = tbNita.Enabled = ddlPreRecep.Enabled = btnSelecNIT.Enabled = btnCargar.Enabled = true;
                Utils.MostrarAlerta(Response, "Esta PreRecepción no tiene items para reversar");
			}
		}
		
		private double Total(double precio, double cantidad ,double descuento, double iva)
		{
			double total = 0;
	        total = cantidad*precio;
			total = total-((descuento/100)*total);
			total = total+(total*(iva/100));
			return(total);
		}
		#endregion
		
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
