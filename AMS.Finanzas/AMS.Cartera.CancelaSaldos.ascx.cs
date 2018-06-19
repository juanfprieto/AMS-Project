
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using System.Configuration;
using System.Web.Mail;
using System.Globalization;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using AMS.DBManager;
using AMS.Tools;



namespace AMS.Finanzas
{
	/// <summary>
	///		Descripción breve de AMS_Cartera_CancelaSaldos.
	/// </summary>
	public partial class AMS_Cartera_CancelaSaldos : System.Web.UI.UserControl
	{
		protected string    Tope = "";
		protected DataTable DataTable1,DataTable2;
		protected string    table, proceso;
		protected Label     lbInfo;
		protected ArrayList sqlstring = new ArrayList();
		protected double    TotalRecibo = 0;
		protected DataSet   facturas;
        
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Tope = DBFunctions.SingleData("Select ccar_valorfrontera from DBXSCHEMA.CCARTERA");
			
            proceso = Request.QueryString["proceso"];
            if (!IsPostBack)
            { 
                DatasToControls  bind = new DatasToControls();
                //bind.PutDatasIntoDropDownList(DDLPrefijo, "select pdoc_codigo,pdoc_nombre from dbxschema.pdocumento where tdoc_tipodocu ='RC' and tvig_vigencia = 'V' order by pdoc_nombre");


                if (Request.QueryString["autor"] == "C")
                {
                    Utils.llenarPrefijos(Response, ref DDLPrefijo, "%", "%", "CX");
                }
                else
                {
                    if (Request.QueryString["autor"] == "P") 
                        Utils.llenarPrefijos(Response, ref DDLPrefijo, "%", "%", "CE");
                    else
                        Utils.MostrarAlerta(Response, "No ha definido en el menu el tipo de cartera a cancelar ... C ó P ");
                }
                
                
                tbTope.Text = Tope;

                if (proceso == "Docmto")
                {
                    psaldos.Visible = false;
                    pcancela.Visible = true;
                    //DDLDocumento.Visible = true;
                    bind.PutDatasIntoDropDownList(DDLDocumento, "SELECT PDOC_CODIGO, PDOC_NOMBRE FROM PDOCUMENTO WHERE TDOC_TIPODOCU IN ('FC','NC','CI','NI')");                   
                }
                else
                {
                    psaldos.Visible = true;
                    pcancela.Visible = true;
                    pdocumento.Visible = false;
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

		}
		#endregion

		protected void Efectuar_Click(object sender, System.EventArgs e)
		{
			if (double.Parse(tbTope.Text.ToString()) > Double.Parse(Tope.ToString()))
			{
                Utils.MostrarAlerta(Response, "El valor no puede exceder del tope parametrizado (" + Tope + ")");
			}
			else
			{
				EfectuarProceso();
			}
		}

		protected void EfectuarProceso()
		{
			string NumeroDocto = DBFunctions.SingleData("Select pdoc_ultidocu from pdocumento where pdoc_codigo = '" + DDLPrefijo.SelectedValue.ToString() + "'");
			preparargrilla();
            string sql = "";

            if(Request.QueryString["autor"]== "C")
            {


                if (proceso == "Docmto")
                {
                    sql = @"Select a.pdoc_codigo,a.mfac_numedocu,a.mnit_nit,b.mnit_apellidos,mnit_apellido2,mnit_nombres,mnit_nombre2,a.mfac_factura, 
                            (a.mfac_valofact+a.mfac_valoiva+a.mfac_valoflet+a.mfac_valoivaflet-a.mfac_valorete - a.mfac_valoabon) as saldo 
                             from DBXSCHEMA.MFACTURACLIENTE a,dbxschema.mnit b  
                             where a.mnit_nit = b.mnit_nit and  (a.mfac_valofact+a.mfac_valoiva+a.mfac_valoflet+a.mfac_valoivaflet-a.mfac_valorete - a.mfac_valoabon) <> 0 
                              and a.pdoc_codigo = '" + DDLDocumento.SelectedValue + "' order by a.mnit_nit;";
                }
                else
                {
                    sql = @"Select a.pdoc_codigo,a.mfac_numedocu,a.mnit_nit,b.mnit_apellidos,mnit_apellido2,mnit_nombres,mnit_nombre2,a.mfac_factura, 
                            (a.mfac_valofact+a.mfac_valoiva+a.mfac_valoflet+a.mfac_valoivaflet-a.mfac_valorete - a.mfac_valoabon) as saldo 
                             from DBXSCHEMA.MFACTURACLIENTE a,dbxschema.mnit b, dbxschema.pdOcumento p  
                             where a.mnit_nit = b.mnit_nit and a.pdoc_codigo = p.pdoc_codigo and p.tdoc_tipodocu in ('FC','NC') AND MFAC_TIPODOCU <> 'I'
                             and (a.mfac_valofact+a.mfac_valoiva+a.mfac_valoflet+a.mfac_valoivaflet-a.mfac_valorete - a.mfac_valoabon) <= " + tbTope.Text +@"
                             and (a.mfac_valofact+a.mfac_valoiva+a.mfac_valoflet+a.mfac_valoivaflet-a.mfac_valorete - a.mfac_valoabon) >= -" + tbTope.Text + @"
                             and (a.mfac_valofact+a.mfac_valoiva+a.mfac_valoflet+a.mfac_valoivaflet-a.mfac_valorete - a.mfac_valoabon) <> 0 order by a.mnit_nit";
                }
            }
            else if (Request.QueryString["autor"] == "P")
            {

                if (proceso == "Docmto")
                {
                    sql = "Select a.pdoc_codigo,a.mfac_numedocu,a.mnit_nit,b.mnit_apellidos,mnit_apellido2,mnit_nombres,mnit_nombre2,a.mfac_factura,";
                    sql = sql + "(a.mfac_valofact+a.mfac_valoiva+a.mfac_valoflet+a.mfac_valoivaflet-a.mfac_valorete - a.mfac_valoabon) as saldo";
                    sql = sql + " from DBXSCHEMA.MFACTURAPROVEEDOR  a,dbxschema.mnit b ";
                    sql = sql + " where a.mnit_nit = b.mnit_nit and  (a.mfac_valofact+a.mfac_valoiva+a.mfac_valoflet+a.mfac_valoivaflet-a.mfac_valorete - a.mfac_valoabon) <> 0 and a.pdoc_codigo = '" + DDLDocumento.SelectedValue + "' order by a.mnit_nit;";
                }
                else
                {
                    sql = "Select a.pdoc_codiordepago,a.mfac_numeordepago,a.mnit_nit,b.mnit_apellidos,mnit_apellido2,mnit_nombres,mnit_nombre2,a.mfac_factura,";
                    sql = sql + "(a.mfac_valofact+a.mfac_valoiva+a.mfac_valoflet+a.mfac_valoivaflet-a.mfac_valorete - a.mfac_valoabon) as saldo";
                    sql = sql + " from DBXSCHEMA.MFACTURAPROVEEDOR a,dbxschema.mnit b ";
                    sql = sql + " where a.mnit_nit = b.mnit_nit and  (a.mfac_valofact+a.mfac_valoiva+a.mfac_valoflet+a.mfac_valoivaflet-a.mfac_valorete - a.mfac_valoabon) <= " + tbTope.Text;
                    sql = sql + " and (a.mfac_valofact+a.mfac_valoiva+a.mfac_valoflet+a.mfac_valoivaflet-a.mfac_valorete - a.mfac_valoabon) >= -" + tbTope.Text;
                    sql = sql + " and (a.mfac_valofact+a.mfac_valoiva+a.mfac_valoflet+a.mfac_valoivaflet-a.mfac_valorete - a.mfac_valoabon) <> 0 order by a.mnit_nit";
                }

            }

        

            facturas = new DataSet();
		
			DBFunctions.Request(facturas,IncludeSchema.NO,sql);
			int TotalFilas = facturas.Tables[0].Rows.Count;
			if (TotalFilas > 0)
			{
				for (int j=0;j<facturas.Tables[0].Rows.Count;j++)
				{
                    DateTime fechaX = Convert.ToDateTime(facturas.Tables[0].Rows[j][7].ToString());
                    string prefijo = facturas.Tables[0].Rows[j][0].ToString();
				    string numero = facturas.Tables[0].Rows[j][1].ToString();
				    string nit = facturas.Tables[0].Rows[j][2].ToString();
				    string nombre = facturas.Tables[0].Rows[j][3].ToString().Trim() + " " + facturas.Tables[0].Rows[j][4].ToString().Trim()+ " " + facturas.Tables[0].Rows[j][5].ToString().Trim() + " " + facturas.Tables[0].Rows[j][6].ToString().Trim();
				    string fecha = fechaX.ToString("yyyy-MM-dd");
				    string saldo = facturas.Tables[0].Rows[j][8].ToString();
					ingresardatos(prefijo,double.Parse(numero),nit,nombre,fecha,double.Parse(saldo));
				}
				Session["DataTable1"]=DataTable1;
				Session["facturas"]= facturas;
				Procesar.Enabled = true;
			}
			//Session["rep"]=RenderHtml();
		}

		protected void Procesar_Click(object sender, System.EventArgs e)
		{
            Procesar.Enabled = false;
            //Response.Redirect("");
            //JFSC 11022008 Poner en comentario por no ser usado
            //bool estado=false;

            facturas = (DataSet)Session["facturas"];
			if (facturas.Tables[0].Rows.Count > 0)
			{
				string fechaproceso = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string fecha        = DateTime.Now.ToString("yyyy-MM-dd");
				string prefijoRecibo = DDLPrefijo.SelectedValue.ToString();
				string nit          = DBFunctions.SingleData("SELECT MNIT_NIT FROM cempresa");
				string flujocajaG   = DBFunctions.SingleData("SELECT PFLU_CODGEN FROM PFLUJOCAJAESPECIFICO FETCH FIRST 1 ROW ONLY");
				string flujocajaE   = DBFunctions.SingleData("SELECT PFLU_CODIGO FROM PFLUJOCAJAESPECIFICO FETCH FIRST 1 ROW ONLY");
			
				string usuario      = HttpContext.Current.User.Identity.Name.ToString();
				int numeroFinal     = System.Convert.ToInt32(DBFunctions.SingleData("SELECT pdoc_Numefina FROM pdocumento WHERE pdoc_codigo='"+DDLPrefijo.SelectedValue.ToString()+"'"));
				int numeroRecibo    = System.Convert.ToInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu FROM pdocumento WHERE pdoc_codigo='"+DDLPrefijo.SelectedValue.ToString()+"'"));
				numeroRecibo++;

                
				//Lo primero es verificar que el numero del recibo este dentro del rango y que no exista
                while (DBFunctions.RecordExist("SELECT * FROM dbxschema.mcaja WHERE pdoc_codigo='" + DDLPrefijo.SelectedValue.ToString() + "' AND mcaj_numero=" + numeroRecibo + ""))
                    numeroRecibo++;

                string sede = DBFunctions.SingleData("select palm_almacen from DBXSCHEMA.cCONTABILIDAD;");
                if (sede.Length == 0)
                {
                    Utils.MostrarAlerta(Response, "No HA PARAMETRIZADO LA SEDE en los parametros de CONTABILIDAD. Proceso Cancelado");
                    return;
                }

                
                if (Request.QueryString["autor"] == "C")
                {
                   
                    if (numeroRecibo > numeroFinal)
                        Utils.MostrarAlerta(Response, "Error : El número de recibo se encuentra fuera del rango permitido");
                    else
                    {
                        for (int j = 0; j < facturas.Tables[0].Rows.Count; j++)
                        {
                            bool seleccionado = ((CheckBox)DATAGRIDFACTURAS.Items[j].Cells[6].FindControl("cbRows")).Checked;
                            if (seleccionado)
                            {
                                string prefijo = facturas.Tables[0].Rows[j][0].ToString();
                                string numero = facturas.Tables[0].Rows[j][1].ToString();
                                string nit1 = facturas.Tables[0].Rows[j][2].ToString();
                                string nombre = facturas.Tables[0].Rows[j][3].ToString().Trim() + " " + facturas.Tables[0].Rows[j][4].ToString().Trim() + " " + facturas.Tables[0].Rows[j][5].ToString().Trim() + " " + facturas.Tables[0].Rows[j][6].ToString().Trim();
                                string fecha1 = facturas.Tables[0].Rows[j][7].ToString();
                                string saldo = facturas.Tables[0].Rows[j][8].ToString();
                                TotalRecibo = TotalRecibo + double.Parse(saldo);

                                sqlstring.Add("UPDATE mfacturacliente SET tvig_vigencia='C',mfac_valoabon=mfac_valoabon+" + saldo + ",mfac_pago='" + fecha + "' WHERE pdoc_codigo='" + prefijo + "' AND mfac_numedocu=" + numero.ToString() + "");
                                sqlstring.Add("INSERT INTO ddetallefacturacliente VALUES('" + prefijo + "'," + numero.ToString() + ",'" + prefijoRecibo.Trim() + "'," + numeroRecibo.ToString().Trim() + "," + saldo + ",'Cancelación saldo Menor Cuantía : " + prefijoRecibo + " - " + numeroRecibo + "')");
                            }
                        }

                        //string sede = DBFunctions.SingleData("select palm_almacen from DBXSCHEMA.cCONTABILIDAD;");
                        //if (sede.Length == 0)
                        //{
                        //    Utils.MostrarAlerta(Response, "No HA PARAMETRIZADO LA SEDE en los parametros de CONTABILIDAD. Proceso Cancelado");
                        //    return;
                        //}
                        //sentencias
                        sqlstring.Add("INSERT INTO mcaja VALUES('" + prefijoRecibo + "'," + numeroRecibo.ToString() + ",'" + fecha + "','I','" + nit + "','" + nit + "','Cancelacion facturas de menor cuantia'," + TotalRecibo.ToString() + "," + TotalRecibo.ToString() + ",'" + sede + "','" + usuario + "','" + fechaproceso + "','P')");
                                                
                        sqlstring.Add("INSERT INTO dflujocaja VALUES('" + prefijoRecibo + "'," + numeroRecibo.ToString() + ",'" + flujocajaG + "','" + flujocajaE + "')");
                        sqlstring.Add("UPDATE pdocumento SET pdoc_ultidocu=" + numeroRecibo.ToString() + " WHERE pdoc_codigo='" + prefijoRecibo + "'");
                        sqlstring.Add("INSERT INTO mcajapago VALUES('" + prefijoRecibo + "'," + numeroRecibo.ToString() + ",0,'DC','E',null,null,null,'" + fecha + "'," + TotalRecibo.ToString() + ",1,0,'Nacional',null,null,null)");

                                            
                    }
                }
                else if (Request.QueryString["autor"] == "P") 
                {              
                    if (numeroRecibo > numeroFinal)
                        Utils.MostrarAlerta(Response, "Error : El número de recibo se encuentra fuera del rango permitido");
                    else
                    {
                        //sentencias
                        sqlstring.Add("INSERT INTO mcaja VALUES('" + prefijoRecibo + "'," + numeroRecibo.ToString() + ",'" + fecha + "','I','" + nit + "','" + nit + "','Cancelacion facturas de menor cuantia'," + TotalRecibo.ToString() + "," + TotalRecibo.ToString() + ",'" + sede + "','" + usuario + "','" + fechaproceso + "','P')");
                        sqlstring.Add("INSERT INTO dflujocaja VALUES('" + prefijoRecibo + "'," + numeroRecibo.ToString() + ",'" + flujocajaG + "','" + flujocajaE + "')");
                        sqlstring.Add("UPDATE pdocumento SET pdoc_ultidocu=" + numeroRecibo.ToString() + " WHERE pdoc_codigo='" + prefijoRecibo + "'");
                        sqlstring.Add("INSERT INTO mcajapago VALUES('" + prefijoRecibo + "'," + numeroRecibo.ToString() + ",0,'DC','E',null,null,null,'" + fecha + "'," + TotalRecibo.ToString() + ",1,0,'Nacional',null,null,null)");

                        for (int j = 0; j < facturas.Tables[0].Rows.Count; j++)
                        {
                            bool seleccionado = ((CheckBox)DATAGRIDFACTURAS.Items[j].Cells[6].FindControl("cbRows")).Checked;
                            if (seleccionado)
                            {
                                string prefijo = facturas.Tables[0].Rows[j][0].ToString();
                                string numero = facturas.Tables[0].Rows[j][1].ToString();
                                string nit1 = facturas.Tables[0].Rows[j][2].ToString();
                                string nombre = facturas.Tables[0].Rows[j][3].ToString().Trim() + " " + facturas.Tables[0].Rows[j][4].ToString().Trim() + " " + facturas.Tables[0].Rows[j][5].ToString().Trim() + " " + facturas.Tables[0].Rows[j][6].ToString().Trim();
                                string fecha1 = facturas.Tables[0].Rows[j][7].ToString();
                                string saldo = facturas.Tables[0].Rows[j][8].ToString();
                                TotalRecibo = TotalRecibo + double.Parse(saldo);

                                sqlstring.Add("UPDATE mfacturaproveedor SET tvig_vigencia='C',mfac_valoabon=mfac_valoabon+" + saldo + ",mfac_pago='" + fecha + "' WHERE pdoc_codiordepago='" + prefijo + "' AND mfac_numeordepago=" + numero.ToString() + "");
                                sqlstring.Add("INSERT INTO dcajaproveedor VALUES('"+ prefijoRecibo + "'," + numeroRecibo.ToString().Trim() + ",'" + prefijo + "'," + numero.ToString() + "," + saldo + ")");
                            }
                        }
                    }
                }
               

                if (DBFunctions.Transaction(sqlstring))
                {
                    Utils.MostrarAlerta(Response, "Proceso Efectuado Perfectamente : " + prefijoRecibo + " - " + numeroRecibo.ToString() + "");
                    facturas.Clear();
                    Session["facturas"] = facturas;

                }
                else
                {
                    Utils.MostrarAlerta(Response, "Error : No se pudo realizar el proceso, verifique ");
                    lbInfo.Text = DBFunctions.exceptions;
                }
                
			}
			else
			{
                Response.Redirect(ConfigurationManager.AppSettings["Web.Default"]+ "?process=Cartera.CancelaSaldos&proceso=Docmto&cod=&name=&path=Cancelación de Saldos de Cartera por Documento");
            }
            
		}
		
		protected void preparargrilla()
		{			
			DataTable1 = new DataTable();
			DataTable1.Columns.Add(new DataColumn("PREFIJO",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("NUMERO",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("NIT",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("NOMBRE",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("FECHA",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("SALDO",System.Type.GetType("System.Double")));
		}

		
		protected void ingresardatos(string prefijo,double numero,string nit,string nombre,string fecha,double saldo)
		{
			DataRow fila=DataTable1.NewRow();
			fila["PREFIJO"]=prefijo;
			fila["NUMERO"]=numero;
			fila["NIT"]=nit;
			fila["NOMBRE"]=nombre;
			fila["FECHA"]=fecha;
			fila["SALDO"]=saldo;
			DataTable1.Rows.Add(fila);
			DATAGRIDFACTURAS.DataSource=DataTable1;
			DATAGRIDFACTURAS.DataBind();
		}

		public void SendMail(Object Sender, ImageClickEventArgs E)
		{
			MailMessage MyMail = new MailMessage();
			MyMail.From = ConfigurationManager.AppSettings["EmailFrom"];
			MyMail.To = tbEmail.Text;
			MyMail.Subject = "Proceso : "+DBFunctions.SingleData("SELECT remarks FROM SYSIBM.SYSTABLES WHERE name='"+table+"'");
			MyMail.Body = (RenderHtml());
			MyMail.BodyFormat = MailFormat.Html;
			try
			{
				SmtpMail.Send(MyMail);}
			catch(Exception e)
			{
				lbInfo.Text = e.ToString();
			}
		}

		protected void Grid_Change(Object sender, DataGridPageChangedEventArgs e) 
		{
			DATAGRIDFACTURAS.CurrentPageIndex = e.NewPageIndex;
			DATAGRIDFACTURAS.DataSource = DataTable1;
		    DATAGRIDFACTURAS.DataBind();
		}

		protected string RenderHtml()
		{
			StringBuilder SB= new StringBuilder();
			StringWriter SW= new StringWriter(SB);
            HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
			phGrilla.RenderControl(htmlTW);
			return SB.ToString();
		}
	}
}
