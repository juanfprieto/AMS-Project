using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AMS.DB;
using AMS.Forms;

namespace AMS.Tools
{
	public partial  class ComentariosTablas : System.Web.UI.UserControl
	{
		protected DataTable dtTools;
		protected DataSet ds;
		protected DataTable dtCampos,dtCamposFK;
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				Recargar_scampoayuda();
				DatasToControls bind=new DatasToControls();
				Session.Clear();
				bind.PutDatasIntoDropDownList(ddltabla,"SELECT distinct scam_tabla FROM  scampoayuda");
				bind.PutDatasIntoDropDownList(ddlcampo,"SELECT scam_campo FROM scampoayuda WHERE scam_tabla='"+ddltabla.SelectedValue+"'");
			}
			else
			{
				if(Session["dtCampos"]!=null)
					dtCampos=(DataTable)Session["dtCampos"];
			}
		}
		
		protected void Recargar_scampoayuda()
		{
			ArrayList del=new ArrayList();
			ds=new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT * FROM dbxschema.scampoayuda;"+
								"SELECT tbname,name,remarks FROM sysibm.syscolumns WHERE tbcreator='DBXSCHEMA'");
			//Recorro la tabla scampoyuda, si uno de los campos q tiene ya no existe en el diccionario
			//de datos, lo elimino
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				DataRow[] iguales=ds.Tables[1].Select("tbname='"+ds.Tables[0].Rows[i][0].ToString()+"' AND name='"+ds.Tables[0].Rows[i][1].ToString()+"'");
				if(iguales.Length==0)
					del.Add("DELETE FROM dbxschema.scampoayuda WHERE scam_tabla='"+ds.Tables[0].Rows[i][0].ToString()+"' AND scam_campo='"+ds.Tables[0].Rows[i][1].ToString()+"'");
			}
			//Borra de scampoayuda las que no tienen comentario
			del.Add("DELETE FROM dbxschema.scampoayuda WHERE scam_ayuda = ''");
			if(DBFunctions.Transaction(del))
			{
				for(int i=0;i<ds.Tables[1].Rows.Count;i++)
					//Inserto las que no concuerdan con las que ya existen
					DBFunctions.NonQuery("INSERT INTO scampoayuda VALUES('"+ds.Tables[1].Rows[i][0].ToString()+"','"+ds.Tables[1].Rows[i][1].ToString()+"','"+ds.Tables[1].Rows[i][2].ToString()+"','')");
			}
		}
		
		protected void Preparar_dtCampos()
		{
			dtCampos=new DataTable();
			dtCampos.Columns.Add("TABLA",typeof(string));
			dtCampos.Columns.Add("CAMPO",typeof(string));
			dtCampos.Columns.Add("COMENTARIO",typeof(string));
			dtCampos.Columns.Add("AYUDA",typeof(string));
		}
		
		protected void Llenar_dtCampos()
		{
			dtCampos=null;
			DataRow fila;
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				if(dtCampos==null)
					this.Preparar_dtCampos();
				fila=dtCampos.NewRow();
				fila[0]=ds.Tables[0].Rows[i][0].ToString();
				fila[1]=ds.Tables[0].Rows[i][1].ToString();
				fila[2]=ds.Tables[0].Rows[i][2].ToString();
				fila[3]=ds.Tables[0].Rows[i][3].ToString();
				dtCampos.Rows.Add(fila);
				Bind_Datas(dtCampos);
			}
		}
		
		protected void Bind_Datas(DataTable dtCampos)
		{
			dgTools.DataSource=dtCampos;
			dgTools.DataBind();
			Session["dtCampos"]=dtCampos;
		}
		
		protected void Realizar_Busqueda()
		{
			if(chbtabla.Checked && chbcampo.Checked)
			{
				ds=new DataSet();
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT scam_tabla,scam_campo,scam_comentario,scam_ayuda FROM scampoayuda WHERE scam_tabla='"+this.ddltabla.SelectedValue+"' AND scam_campo='"+this.ddlcampo.SelectedValue+"'");
				Llenar_dtCampos();
				Bind_Datas(dtCampos);
				lnbimp1.Visible=lnbimp2.Visible=true;
			}
			else if(chbtabla.Checked && !chbcampo.Checked)
			{
				ds=new DataSet();
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT scam_tabla,scam_campo,scam_comentario,scam_ayuda FROM scampoayuda WHERE scam_tabla='"+this.ddltabla.SelectedValue+"'");
				Llenar_dtCampos();
				Bind_Datas(dtCampos);
				lnbimp1.Visible=lnbimp2.Visible=true;
			}
			else if(!chbtabla.Checked && chbcampo.Checked)
			{
				ds=new DataSet();
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT scam_tabla,scam_campo,scam_comentario,scam_ayuda FROM scampoayuda WHERE scam_campo='"+this.ddlcampo.SelectedValue+"'");
				Llenar_dtCampos();
				Bind_Datas(dtCampos);
				lnbimp1.Visible=lnbimp2.Visible=true;
			}
			else
			{
                Utils.MostrarAlerta(Response, "No hay opciones de busqueda seleccionadas");
				lnbimp1.Visible=lnbimp2.Visible=false;
			}
		}
		
		protected void ddltabla_SelectedIndexChanged(object Sender,EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlcampo,"SELECT scam_campo FROM scampoayuda WHERE scam_tabla='"+ddltabla.SelectedValue+"'");
		}
		
		protected void btnBuscar_Click(object Sender,EventArgs e)
		{
			Realizar_Busqueda();
		}
		
		protected void dgTools_EditCommand(object Sender,DataGridCommandEventArgs e)
		{
			dgTools.EditItemIndex=e.Item.DataSetIndex;
			Bind_Datas(dtCampos);
		}
				
		protected void dgTools_UpdateCommand(object Sender,DataGridCommandEventArgs e)
		{
			ArrayList sql=new ArrayList();
			sql.Add("UPDATE scampoayuda SET scam_ayuda='"+((TextBox)e.Item.FindControl("tbcom")).Text+"' WHERE scam_tabla='"+e.Item.Cells[0].Text+"' AND scam_campo='"+e.Item.Cells[1].Text+"'");
			if(DBFunctions.Transaction(sql))
			{		
				dtCampos.Rows[e.Item.DataSetIndex][3]=((TextBox)e.Item.FindControl("tbcom")).Text;
				dgTools.EditItemIndex=-1;
				Bind_Datas(dtCampos);
			}
			else
				lb.Text+="Error "+DBFunctions.exceptions;
		}
		
		protected void dgTools_CancelCommand(object Sender,DataGridCommandEventArgs e)
		{
			dgTools.EditItemIndex=-1;
			Bind_Datas(dtCampos);
		}

		protected void dgTools_ItemCommand(object Sender,DataGridCommandEventArgs e)
		{
			if(e.CommandName=="imprimir")
			{
				ArrayList arr=new ArrayList();
				for(int i=0;i<dtCampos.Columns.Count;i++)
					arr.Add(dtCampos.Rows[e.Item.DataSetIndex][i].ToString());
				Session["datimp"]=arr;
				Response.Write("<script language:javascript>w=window.open('AMS.Tools.Impresion.aspx?tip=1')</script>");
			}
		}

		protected void lnbimp1_Click(object sender, System.EventArgs e)
		{
			Session["datimp"]=dtCampos;
			Response.Write("<script language:javascript>w=window.open('AMS.Tools.Impresion.aspx?tip=2')</script>");
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
