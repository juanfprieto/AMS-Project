namespace AMS.Marketing
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
	using System.Collections;
	using System.Configuration;
    using AMS.Tools;
	/// <summary>
	///		Descripción breve de AMS_Marketing_CreacionEncuesta.
	/// </summary>
	public partial class CreacionEncuesta : System.Web.UI.UserControl
	{
		protected DataTable dtTabla;
		protected DropDownList ddlRespuestas;
		protected string MainPage=ConfigurationManager.AppSettings["MainIndexPage"];

		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				Session.Clear();
				if(Request["CodEnc"]!=null)
                Utils.MostrarAlerta(Response, "Se ha creado satisfactoriamente la encuesta con código " + Request["CodEnc"] + "");
				lnbAnt.CommandName="Pnlaplicacion";
				lnbSig.CommandName="PnlPregunta";
				this.Cargar_dtTabla();
				this.Bind_Datas_dtTabla();				
			}
			else
			{
				if(Session["dtTabla"]!=null)
					dtTabla=(DataTable)Session["dtTabla"];
			}
		}

		private void lnbAnt_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
		{		
			if(e.CommandName=="Pnlaplicacion")
			{
				Pnlaplicacion.Visible=true;
				PnlPregunta.Visible=false;
				//PnlRespuesta.Visible=false;
				lnbAnt.Enabled=false;//voy para
				lnbSig.CommandName="PnlPregunta";//vengo de
			}
			else if(e.CommandName=="PnlPregunta")
			{
				//Response.Write("<script language:javascript>alert('Entró');</script>");	
				Pnlaplicacion.Visible=false;
				PnlPregunta.Visible=true;
				LbPreguntas.Visible=true;
				//PnlRespuesta.Visible=false;
				lnbAnt.CommandName="Pnlaplicacion";//voy para
				lnbSig.CommandName="PnlRespuesta";//vengo de
				lnbSig.Enabled=true;
				lnbAnt.Enabled=true;
				this.Datagrid1.Enabled=true;
			}
		}

		private void lnbSig_Command(object Sender,CommandEventArgs e)
		{
			if(e.CommandName=="Pnlaplicacion")
			{
				Pnlaplicacion.Visible=true;
				PnlPregunta.Visible=false;
				//PnlRespuesta.Visible=false;
				lnbSig.CommandName="PnlPregunta";//voy para
				//lnbAnt.CommandName="Pnlaplicacion";//vengo de
			}
			else if(e.CommandName=="PnlPregunta")
			{
				Pnlaplicacion.Visible=false;
				PnlPregunta.Visible=true;
				LbPreguntas.Visible=true;
				//this.Datagrid1.Visible=true;
				//PnlRespuesta.Visible=false;
				lnbSig.CommandName="guardar";//voy para
				lnbAnt.CommandName="Pnlaplicacion";//vengo de
				lnbSig.Enabled=true;
				lnbAnt.Enabled=true;
			}
			else if(e.CommandName=="guardar")
			{
				if(dtTabla.Rows.Count!=0)
				{
					this.Datagrid1.Enabled=false;
					lnbSig.Enabled=false;
					lnbAnt.Enabled=false;
                    Utils.MostrarAlerta(Response, "Ahora puede guardar la información de la encuesta");
					BtnGuardar.Visible=true;
				}
				else
                Utils.MostrarAlerta(Response, "No ha agregado preguntas a la encuesta");
			}
		}

			protected void Cargar_dtTabla()
			{
			dtTabla=new DataTable();
			dtTabla.Columns.Add("PREGUNTAS",typeof(string));
			dtTabla.Columns.Add("COMENTARIOS",typeof(string));
		}

		protected void Bind_Datas_dtTabla()
		{
			Datagrid1.DataSource=dtTabla;
			Datagrid1.DataBind();
			Session["dtTabla"]=dtTabla;
		}

		protected void Llenar_dtTabla_Inicial()
		{
			DataSet ds=new DataSet();
			DataRow fila;
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT * FROM PPREGUNTAENCUESTA");
			if(dtTabla==null)
				Cargar_dtTabla();
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				fila=dtTabla.NewRow();
				fila[0]=ds.Tables[0].Rows[i][0].ToString();
				fila[1]=ds.Tables[0].Rows[i][1].ToString();
				dtTabla.Rows.Add(fila);
				Datagrid1.DataSource=dtTabla;
				Datagrid1.DataBind();
				Session["dtTabla"]=dtTabla;
			}
		}

		protected void btnGuardar_Click(object Sender,EventArgs e)
		{
			ArrayList sqls=new ArrayList();
			sqls.Add("INSERT INTO mencuesta VALUES('"+this.TbnEncuesta.Text+"','"+this.TbNombre.Text+"','"+this.TbFecha.Text+"','"+this.TbRespon.Text+"','"+this.tbobj.Text+"')");
			for(int i=0;i<dtTabla.Rows.Count;i++)
				sqls.Add("INSERT INTO dencuesta VALUES('"+this.TbnEncuesta.Text+"','"+dtTabla.Rows[i][0].ToString()+"')");
			if(DBFunctions.Transaction(sqls))
				Response.Redirect(MainPage+"?process=Marketing.CreacionEncuesta&CodEnc="+TbnEncuesta.Text+"");
				//lb.Text+="Bien "+DBFunctions.exceptions;
			else
				lb.Text+="Error "+DBFunctions.exceptions;
		}

		protected void Datagrid1_ItemCommand(object Sender,DataGridCommandEventArgs e)
		{
			if(((Button)e.CommandSource).CommandName=="agregar")	
			{
				if(((TextBox)e.Item.Cells[0].Controls[1]).Text=="" || ((TextBox)e.Item.Cells[1].Controls[1]).Text=="")
                    Utils.MostrarAlerta(Response, "Falta un campo por llenar en la tabla");
				else if(this.Verificar_dtTabla(e))
                    Utils.MostrarAlerta(Response, "Esa pregunta ya fue agregada");
				else
				{
					DataRow fila=dtTabla.NewRow();
					fila[0]=((TextBox)e.Item.Cells[0].Controls[1]).Text;
					fila[1]=((TextBox)e.Item.Cells[1].Controls[1]).Text;
					dtTabla.Rows.Add(fila);
					this.Bind_Datas_dtTabla();
				}
			}
			else if(((Button)e.CommandSource).CommandName=="eliminar")
			{
				dtTabla.Rows[e.Item.DataSetIndex].Delete();
				this.Bind_Datas_dtTabla();
			}
		}

		protected bool Verificar_dtTabla(DataGridCommandEventArgs e)
		{
			bool error=false;
			DataRow[] pregunta=dtTabla.Select("PREGUNTAS='"+((TextBox)e.Item.Cells[0].Controls[1]).Text+"'");
			if(pregunta.Length!=0)
				error=true;
			return error;
		}

		#region Web Form Designer generated code
				override protected void OnInit(EventArgs e)
			{
					InitializeComponent();
					base.OnInit(e) ;
				}
		
				private void InitializeComponent()
				{
					this.Datagrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.Datagrid1_ItemCommand);
					this.lnbAnt.Command += new System.Web.UI.WebControls.CommandEventHandler(this.lnbAnt_Command);
					this.lnbSig.Command += new System.Web.UI.WebControls.CommandEventHandler(this.lnbSig_Command);

				}
					#endregion

		protected void BtnCancelar_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(MainPage+"?process=Marketing.CreacionEncuesta");
		}
	}
}
	

		
	