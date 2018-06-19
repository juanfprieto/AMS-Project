using System;
using System.Text;
using System.IO;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.Mail;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.Documentos;
using AMS.DB;
using AMS.Tools;
using ClosedXML.Excel;

namespace AMS.Inventarios
{
    public partial class AdminSugerido : System.Web.UI.UserControl
    {
        protected DataTable dtSugerido;
       //protected System.Web.UI.WebControls.CheckBoxList CheckLinea;
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{//el comentario
				DatasToControls bind = new DatasToControls();
				lbFecha.Text = DateTime.Now.ToShortDateString();
				bind.PutDatasIntoDropDownList(ddlLinea, "SELECT plin_codigo, plin_codigo || ' - ' || plin_nombre FROM plineaitem ORDER BY plin_nombre");
                bind.PutDatasIntoDropDownList(ddlSugerido, "SELECT tsug_codigo, tsug_nombre FROM tsugerido");
                bind.PutDatasIntoDropDownList(ddlProveedor,"SELECT MPR.mnit_nit, MPR.mnit_nit CONCAT ' - ' CONCAT MNI.mnit_apellidos CONCAT ' ' CONCAT MNI.mnit_nombres FROM mproveedor MPR, mnit MNI WHERE MPR.mnit_nit=MNI.mnit_nit AND MPR.MNIT_NIT IN (SELECT DISTINCT MNIT_NIT FROM MITEMS) ORDER BY MPR.mnit_nit");
			}
		}
		
		protected void CambioFiltro(object Sender, EventArgs E)
		{
			if(rblGrupoItems.SelectedValue == "T")
				plProveedor.Visible = false;
			else if(rblGrupoItems.SelectedValue == "P")
				    plProveedor.Visible = true;
		}
		
		protected void CrearSugerido(object Sender, EventArgs E)
		{
			int ano_cinv  = Convert.ToInt32(DBFunctions.SingleData("SELECT pano_ano from cinventario"));
			int mes_cinv  = Convert.ToInt32(DBFunctions.SingleData("SELECT pmes_mes from cinventario"));
            string linea = ddlLinea.SelectedValue;

            if (CheckLinea.Checked)
                linea = "zZ";
            string salida = PedidoSugerido.CrearPedidoSugerido(mes_cinv,ano_cinv, linea);
			if(salida != "")
				lb.Text += salida;
			else
                Utils.MostrarAlerta(Response, "Proceso Completado!");
     	}

        protected void ddlLinea_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        protected void ConsultarSugerido(object Sender, EventArgs E)
		{
			this.PrepararDtSugerido();
			ArrayList mesesAnos = new ArrayList();
			bool continuar = true;
			int  ano_cinv  = Convert.ToInt32(DBFunctions.SingleData("SELECT pano_ano from cinventario"));
			int  anoPivote = ano_cinv;
			int  mes_cinv  = Convert.ToInt32(DBFunctions.SingleData("SELECT pmes_mes from cinventario"));
			int  mesPivote = mes_cinv;
            
			//Revisamos si existen registros dentro de la tabla de msugeridoitem
			if(!DBFunctions.RecordExist("SELECT * FROM msugeridoitem"))
			{
				string salida = PedidoSugerido.CrearPedidoSugerido(mes_cinv,ano_cinv, "zZ"); // zZ   todas las líneas
				if(salida != "")
				{
					lb.Text += salida;
					continuar = false;
				}
			}
			if(continuar)
			{
				lbInfo.Text = "";
				lbInfo.Text += "<br>Tipo Listado : "+this.ddlSugerido.SelectedItem.Text.ToUpper()+"";
				lbInfo.Text += "<br>Fecha : "+DateTime.Now.ToShortDateString()+"";
                int [,] anomes = new int [12,2];
                for (int i = 0; i < 12; i++)
                {
                    anomes[i, 0] = anoPivote;
                    anomes[i, 1] = mesPivote;
                    mesesAnos.Add(mesPivote.ToString() + "-" + anoPivote.ToString());
					mesPivote = mesPivote - 1;
					if(mesPivote<=0)
					{
							mesPivote = 12;
							anoPivote = anoPivote - 1;
					}
			    }

                string sqlQuery = "select DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) AS mite_codigo, MIT.mite_nombre, mite_clasabc as abc, msal_cantactual, msal_cantasig, MSUG.msuge_cantidad, "+
                       " msal_costprom, (MSUG.msuge_cantidad * MSI.MSAL_COSTPROM) AS VRSUGERIDO, MSUG.msuge_demanprome, msal_unidtrans, msal_unidpendi, " +
                       "coalesce(ma0.mDEM_cantidad,0),coalesce(ma1.mDEM_cantidad,0),coalesce(ma2.mDEM_cantidad,0),coalesce(ma3.mDEM_cantidad,0),coalesce(ma4.mDEM_cantidad,0),coalesce(ma5.mDEM_cantidad,0), "+
                       "coalesce(ma6.mDEM_cantidad,0),coalesce(ma7.mDEM_cantidad,0),coalesce(ma8.mDEM_cantidad,0),coalesce(ma9.mDEM_cantidad,0),coalesce(ma10.mDEM_cantidad,0),coalesce(ma11.mDEM_cantidad,0) " +  
                       "FROM msugeridoitem MSUG, plineaitem PLIN, msaldoitem msi, mitems MIT "+
                       " left join mDEMANDAitem ma0  on mit.mite_codigo=ma0.mite_codigo  AND ma0.pano_ano="+ anomes [0,0] +"  AND ma0.pmes_mes="+ anomes [0,1] +" " +
                       " left join mDEMANDAitem ma1  on mit.mite_codigo=ma1.mite_codigo  AND ma1.pano_ano="+ anomes [1,0] +"  AND ma1.pmes_mes="+ anomes [1,1] +" "+
                       " left join mDEMANDAitem ma2  on mit.mite_codigo=ma2.mite_codigo  AND ma2.pano_ano="+ anomes [2,0] +"  AND ma2.pmes_mes="+ anomes [2,1] +" "+
                       " left join mDEMANDAitem ma3  on mit.mite_codigo=ma3.mite_codigo  AND ma3.pano_ano="+ anomes [3,0] +"  AND ma3.pmes_mes="+ anomes [3,1] +" "+
                       " left join mDEMANDAitem ma4  on mit.mite_codigo=ma4.mite_codigo  AND ma4.pano_ano="+ anomes [4,0] +"  AND ma4.pmes_mes="+ anomes [4,1] +" "+
                       " left join mDEMANDAitem ma5  on mit.mite_codigo=ma5.mite_codigo  AND ma5.pano_ano="+ anomes [5,0] +"  AND ma5.pmes_mes="+ anomes [5,1] +" "+
                       " left join mDEMANDAitem ma6  on mit.mite_codigo=ma6.mite_codigo  AND ma6.pano_ano="+ anomes [6,0] +"  AND ma6.pmes_mes="+ anomes [6,1] +" "+
                       " left join mDEMANDAitem ma7  on mit.mite_codigo=ma7.mite_codigo  AND ma7.pano_ano="+ anomes [7,0] +"  AND ma7.pmes_mes="+ anomes [7,1] +" "+
                       " left join mDEMANDAitem ma8  on mit.mite_codigo=ma8.mite_codigo  AND ma8.pano_ano="+ anomes [8,0] +"  AND ma8.pmes_mes="+ anomes [8,1] +" "+
                       " left join mDEMANDAitem ma9  on mit.mite_codigo=ma9.mite_codigo  AND ma9.pano_ano="+ anomes [9,0] +"  AND ma9.pmes_mes="+ anomes [9,1] +" "+
                       " left join mDEMANDAitem ma10 on mit.mite_codigo=ma10.mite_codigo AND ma10.pano_ano="+ anomes [10,0] +" AND ma10.pmes_mes="+ anomes [10,1] +" "+
                       " left join mDEMANDAitem ma11 on mit.mite_codigo=ma11.mite_codigo AND ma11.pano_ano="+ anomes [11,0] +" AND ma11.pmes_mes="+ anomes [11,1] +" "+
                       "WHERE MSUG.mite_codigo=MIT.mite_codigo AND MSUG.tsug_codigo=" + this.ddlSugerido.SelectedValue + " AND MIT.plin_codigo=PLIN.plin_codigo and mit.mite_codigo = msi.mite_codigo and msi.pano_ano = " + ano_cinv + " "+
                //       " AND MIT.MITE_CODIGO = '0C-904-4L0G' "+
                       " ";
				//								0									1                  							2				3						4		
				if(this.rblGrupoItems.SelectedValue == "P")
				{
					sqlQuery += " AND MIT.mnit_nit='"+this.ddlProveedor.SelectedValue+"'";
					lbInfo.Text += "<br>Proveedor : "+ddlProveedor.SelectedItem.Text.ToUpper()+"";
				}
				DataSet ds = new DataSet();
				DBFunctions.Request(ds,IncludeSchema.NO,sqlQuery);
				double totalunidadesSugeridas = 0, valorTotalSugerido = 0;
				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					DataRow fila = this.dtSugerido.NewRow();
					fila[0] = ds.Tables[0].Rows[i][0].ToString();
					fila[1] = ds.Tables[0].Rows[i][1].ToString();
					fila[2] = ds.Tables[0].Rows[i][2].ToString();
                    fila[3] = ds.Tables[0].Rows[i][3].ToString();
                    fila[4] = ds.Tables[0].Rows[i][4].ToString();
                    fila[5] = ds.Tables[0].Rows[i][5].ToString();
                    fila[6] = ds.Tables[0].Rows[i][6].ToString();
                    fila[7] = ds.Tables[0].Rows[i][7].ToString();
                    fila[8] = ds.Tables[0].Rows[i][8].ToString();
                    fila[9] = ds.Tables[0].Rows[i][9].ToString();
                    fila[10] = ds.Tables[0].Rows[i][10].ToString();

                    totalunidadesSugeridas +=Convert.ToDouble(ds.Tables[0].Rows[i][5]);
					valorTotalSugerido += Convert.ToDouble(fila[6]);

					for(int j=11;j<23;j++)
					{
						fila[j] = Convert.ToDouble(ds.Tables[0].Rows[i][j]);
					}
					this.dtSugerido.Rows.Add(fila);
				}
                ViewState["dgSugerido"] = dtSugerido;
				this.BindDgSugerido(mesesAnos);
				////////////////////////////////////
				lbInfo2.Text = "";
				lbInfo2.Text += "<br>Total Items Sugeridos : "+ds.Tables[0].Rows.Count+"";
				lbInfo2.Text += "<br>Total Unidades Sugeridas : "+totalunidadesSugeridas.ToString("N")+"";
				lbInfo2.Text += "<br>Valor Total del Sugerido : "+valorTotalSugerido.ToString("C")+"";
			}
			else
                Utils.MostrarAlerta(Response, "Se ha presentado un error en el proceso!");
		}
		
		public void SendMail(Object Sender, ImageClickEventArgs E)
		{
            string urlImagen = "http://ecas.co/images/" + GlobalData.getEMPRESA() + ".png";
            string nombre = "Informe Pedidos Sugeridos";
            string empresa = DBFunctions.SingleDataGlobal("select gemp_descripcion from gempresa where gemp_nombre='" + GlobalData.getEMPRESA() + "';");
            //if(this.dgSugerido.Items.Count>0)
            //{
            //	StringBuilder SB= new StringBuilder();
            // 	    	StringWriter SW= new StringWriter(SB);
            //      	HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
            //     		plReport.RenderControl(htmlTW);
            //     		MailMessage MyMail = new MailMessage();
            //			  	MyMail.From = ConfigurationManager.AppSettings["EmailFrom"];
            //   		MyMail.To = tbEmail.Text;
            //  			MyMail.Subject = "SUGERIDO";
            //  			MyMail.Body = SB.ToString();
            // 	  		MyMail.BodyFormat = MailFormat.Html;
            //	try{
            // 	   			SmtpMail.Send(MyMail);}
            // 			catch(Exception e){
            // 		 	  lb.Text = e.ToString();
            //  		}
            //}
            //else
            dtSugerido = (DataTable)ViewState["dgSugerido"];
            dtSugerido.TableName = "Pedido Sigerido";

            //Create a New Workbook.

            XLWorkbook wb = new XLWorkbook();

            //Add the DataTable as Excel Worksheet.

            wb.Worksheets.Add(dtSugerido);



            MemoryStream memoryStream = new MemoryStream();

            //Save the Excel Workbook to MemoryStream.

            wb.SaveAs(memoryStream);

            string mensajeExcel =
                    @"<div style='position: absolute; background-color:#EEEFD9;width: 35%;border-radius: 10px;margin: auto;padding: 20px;box-shadow: 1px 7px 9px #888888;'>
	                	    <img style='width: 20%; position: absolute; right: 2%;' src='" + urlImagen + @"' /><br><br>
		                    <b><font size='5'>Excel Generado:</font></b>
		                    <br>" + nombre +
                       @"<br><br>
		                    <b>Reciba un cordial saludo</b>, <br>
		                    Ha recibido un Excel usando el Sistema Ecas <br>
                            Dicho Excel se encuentra disponible como archivo <br>
                            adjunto en este correo.
		                    <br><br>
	                        <b>Gracias por su atención.</b>
		                    <br>
		                    <i>eCAS-AMS.</i>
	                    </div>
                        <br><br>";

            //Convert MemoryStream to Byte array.

            byte[] bytes = memoryStream.ToArray();

            memoryStream.Close();
            try
            {
                AMS_Tools_Email.enviarMail(tbEmail.Text, "Ha recibido un Reporte Excel de " + empresa, mensajeExcel, TipoCorreo.HTML, bytes);

                Utils.MostrarAlerta(Response, "Email enviado satisfactoriamente a: " + tbEmail.Text);
                //Response.Redirect(indexPage + "?process=DBManager.Selects&table=" + ds.DataSetName);
            }
            catch (Exception z)
            {
                lb.Text = z.Message;
            }
        }
		
		protected void PrepararDtSugerido()
		{
			dtSugerido = new DataTable();
			dtSugerido.Columns.Add(new DataColumn("CODIGO",System.Type.GetType("System.String")));//0
			dtSugerido.Columns.Add(new DataColumn("NOMBRE",System.Type.GetType("System.String")));//1
            dtSugerido.Columns.Add(new DataColumn("ABC", System.Type.GetType("System.String")));//2
            dtSugerido.Columns.Add(new DataColumn("QACTUAL",System.Type.GetType("System.Double")));//3
            dtSugerido.Columns.Add(new DataColumn("QASIGNADA", System.Type.GetType("System.Double")));//3
            dtSugerido.Columns.Add(new DataColumn("QSUGERIDO", System.Type.GetType("System.Double")));//8
            dtSugerido.Columns.Add(new DataColumn("COSTOPROMEDIO", System.Type.GetType("System.Double")));//6
            dtSugerido.Columns.Add(new DataColumn("VALORSUGERIDO", System.Type.GetType("System.Double")));//9
            dtSugerido.Columns.Add(new DataColumn("DEMANDAPROMEDIO", System.Type.GetType("System.Double")));//7
            dtSugerido.Columns.Add(new DataColumn("QTRANSITO",System.Type.GetType("System.Double")));//4
			dtSugerido.Columns.Add(new DataColumn("QBACKORDER",System.Type.GetType("System.Double")));//5
			dtSugerido.Columns.Add(new DataColumn("DEMANDAMES12",System.Type.GetType("System.Double")));//10
			dtSugerido.Columns.Add(new DataColumn("DEMANDAMES11",System.Type.GetType("System.Double")));//11
			dtSugerido.Columns.Add(new DataColumn("DEMANDAMES10",System.Type.GetType("System.Double")));//12
			dtSugerido.Columns.Add(new DataColumn("DEMANDAMES9",System.Type.GetType("System.Double")));//13
			dtSugerido.Columns.Add(new DataColumn("DEMANDAMES8",System.Type.GetType("System.Double")));//14
			dtSugerido.Columns.Add(new DataColumn("DEMANDAMES7",System.Type.GetType("System.Double")));//15
			dtSugerido.Columns.Add(new DataColumn("DEMANDAMES6",System.Type.GetType("System.Double")));//16
			dtSugerido.Columns.Add(new DataColumn("DEMANDAMES5",System.Type.GetType("System.Double")));//17
			dtSugerido.Columns.Add(new DataColumn("DEMANDAMES4",System.Type.GetType("System.Double")));//18
			dtSugerido.Columns.Add(new DataColumn("DEMANDAMES3",System.Type.GetType("System.Double")));//19
			dtSugerido.Columns.Add(new DataColumn("DEMANDAMES2",System.Type.GetType("System.Double")));//20
			dtSugerido.Columns.Add(new DataColumn("DEMANDAMES1",System.Type.GetType("System.Double")));//21
		}
		
		protected void BindDgSugerido()	
		{
			dgSugerido.DataSource = dtSugerido;
			dgSugerido.DataBind();
			DatasToControls.JustificacionGrilla(dgSugerido,dtSugerido);
		}
		
		protected void BindDgSugerido(ArrayList mesesAnos)
		{
			if(dtSugerido.Rows.Count>1)
			{
				for(int j=11;j<23;j++)
				{
					string[] mesAnoLabel = mesesAnos[j-11].ToString().Split('-');
					string mes = DBFunctions.SingleData("SELECT pmes_nombre FROM pmes WHERE pmes_mes="+mesAnoLabel[0]+"");
					dgSugerido.Columns[j].HeaderText = mes+"-"+mesAnoLabel[1];
				}
			}
			dgSugerido.DataSource = dtSugerido;
			dgSugerido.DataBind();
			DatasToControls.JustificacionGrilla(dgSugerido,dtSugerido);
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

        protected void ImprimirExcelGrid(Object Sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                DateTime fecha = DateTime.Now;
                string nombreArchivo = "PEDIDOSUGERIDO" + "_" + fecha.ToShortDateString() + "_" + fecha.ToShortTimeString();
                base.Response.Clear();
                base.Response.AddHeader("content-disposition", "attachment;filename=" + nombreArchivo + ".xls");
                base.Response.Charset = "Unicode";
                base.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                base.Response.ContentType = "application/vnd.xls";
                StringWriter stringWrite = new StringWriter();
                HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

                dt = (DataTable)ViewState["dgSugerido"];
                DataGrid dgAux = new DataGrid();
                dgAux.DataSource = dt;
                dgAux.DataBind();
                dgAux.RenderControl(htmlWrite);

                base.Response.Write(stringWrite.ToString());
                base.Response.End();
            }
            catch (Exception ex)
            {
                lb.Text = "Couldn't create Excel file.\r\nException: " + ex.Message;
                return;
            }
        }
	}

    //protected void dgTable_Procesos(object sender, DataGridCommandEventArgs e) 
    //    {
						
    //        #region Imprimir
			
    //            lbInfo.Text = e.CommandName;
    //            string processTitle="";
    //            string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
    			
    //            table=Request.QueryString["table"];

    //            if(Request.QueryString["processTitle"] != null)
    //                processTitle = Request.QueryString["processTitle"];
    			
    //            Response.Redirect(indexPage + "?process=DBManager.Preview&table="+table+"&pks="+GenerarLlave(e.Item.DataSetIndex).Replace("'","*")+"&path=" + Request.QueryString["path"]);
			
    //        #endregion Imprimir
    //    }

   
}
