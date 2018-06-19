using System;
using System.Data;
using System.Web;
using System.Web.Caching;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.ServiceProcess;
using AMS.DB;

namespace AMS.Web
{
	/// <summary>
	/// Descripción breve de MobileIndex.
	/// </summary>
	public partial class MobileIndex : System.Web.UI.Page
	{
		protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];
		protected string nameSystem = ConfigurationManager.AppSettings["SystemName"] + ".";
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected string process;
		protected string userName;
		protected string userCod;
		protected string tabla;
	
		public MobileIndex()
		{
			Page.Init += new System.EventHandler(Page_Load);
		}
  	
				
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string Control = "";
			userCod = Request.QueryString["cod"];
			userName = Request.QueryString["name"];
			infoProcess.Text = ConfigurationManager.AppSettings["systemName"];

			try
			{
				if(Session["AMS_USER"]==null)
					Session["AMS_USER"]=DBFunctions.SingleData("SELECT U.SUSU_NOMBRE CONCAT ' (' CONCAT U.SUSU_LOGIN CONCAT ')<BR>' CONCAT P.TTIPE_DESCRIPCION FROM SUSUARIO U,TTIPOPERFIL P WHERE U.TTIPE_CODIGO=P.TTIPE_CODIGO AND U.SUSU_LOGIN='"+HttpContext.Current.User.Identity.Name+"';");
				lblUsuario.Text=Session["AMS_USER"].ToString();
			}
			catch(Exception ex)
			{
				lblUsuario.Text=ex.Message;
			}
			
			if(!IsPostBack)
			{
				string ruta,padre,hijo;
				bool err=false;
				int nm=0;
				ruta=Request.QueryString["path"];
				hijo=ruta;
				while(!err && nm++<3)
				{
					padre=DBFunctions.SingleData("SELECT sp.smen_opcion FROM smenu sp, smenu sh WHERE sh.smen_opcion='"+hijo+"' and sp.smen_opcion=sh.smen_padre;");
					if(padre.Length==0)break;
					hijo=padre;
					//if(nm==1)ruta="<font size=2>"+ruta+"</font>";
					ruta=padre + " / "+ruta;
				}
				ViewState["AMS_RUTA"]=ruta;
				LoadMenu();
			}
			if(ViewState["AMS_RUTA"]==null)
				ViewState["AMS_RUTA"]="AMS";
			
			if(Request.QueryString["process"] != null)
			{
				process = Request.QueryString["process"];
				tabla = Request.QueryString["table"];
				ControlsFromProcess();
			}
			else
			{
				Image img = new Image();
				img.AlternateText = "Bienvenido al sistema AMS";
				img.ImageUrl = ConfigurationManager.AppSettings["BootImageMobile"];
				img.ImageAlign = ImageAlign.Left;
				gridHolder.Controls.Add(img);
			}
		}

		private void InitializeComponent()
		{
		}

		private void ControlsFromProcess()
		{
			if(process.Length>0)
				gridHolder.Controls.Add(LoadControl("" + pathToControls + nameSystem + process + ".ascx"));
			if(ViewState["AMS_RUTA"]!=null)
				infoProcess.Text=ViewState["AMS_RUTA"].ToString();
			else
				infoProcess.Text=Request.QueryString["path"];

		}

		private void LoadMenu()
		{
			//DataTable dtMenuM=(DataTable)Cache.Get("DT_MOBILEMENU");
			DataTable dtMenuM=null;
			if(dtMenuM==null)
			{
				int n,m;
				DataSet dsMenuM=new DataSet();
				DataRow[]drLinks;
				DataRow drM;
				string path;
				dtMenuM=new DataTable();
				dtMenuM.Columns.Add("TEXTO",typeof(string));
				DBFunctions.Request(dsMenuM,IncludeSchema.NO, 
					"SELECT smen_codigo, smen_opcion, smen_padre, smen_url FROM SMENU WHERE smen_padre<>'ND' AND smen_url='ND' ORDER BY smen_opcion;"+
					"SELECT smen_codigo, smen_opcion, smen_padre, smen_url FROM SMENU WHERE smen_movil='S' ORDER BY smen_padre, smen_opcion;");
				for(n=0;n<dsMenuM.Tables[0].Rows.Count;n++)
				{
					drLinks=dsMenuM.Tables[1].Select("smen_padre='"+dsMenuM.Tables[0].Rows[n]["smen_opcion"].ToString()+"'");
					if(drLinks.Length>0)
					{
						drM=dtMenuM.NewRow();
						drM[0]=dsMenuM.Tables[0].Rows[n]["smen_opcion"].ToString();
						path=dsMenuM.Tables[0].Rows[n]["smen_opcion"].ToString();
						dtMenuM.Rows.Add(drM);
						for(m=0;m<drLinks.Length;m++){
							drM=dtMenuM.NewRow();
							drM[0]="&nbsp;&nbsp;<a href='"+drLinks[m]["smen_url"].ToString()+
								"&cod="+Request.QueryString["cod"]+"&name="+Request.QueryString["name"]+"&path="+path+"'"+
								">"+drLinks[m]["smen_opcion"].ToString()+"</a>";
							dtMenuM.Rows.Add(drM);
						}
					}
				}
				drM=dtMenuM.NewRow();
				drM[0]="<a href='AMS.Web.CerrarSesion.aspx'>Cerrar Sesion</a>";
				dtMenuM.Rows.Add(drM);
				Cache.Insert("DT_MOBILEMENU", dtMenuM, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero, CacheItemPriority.High, null);
			}
			dgrMenu.DataSource=dtMenuM;
			dgrMenu.DataBind();
		}
		public void dgrMenu_ItemDataBound(object sender, DataGridItemEventArgs e){
			if(e.Item.ItemType==ListItemType.Item||e.Item.ItemType==ListItemType.AlternatingItem)
				if(DataBinder.Eval(e.Item.DataItem, "TEXTO").ToString().IndexOf("&nbsp;")!=0)
					e.Item.Cells[0].BackColor=System.Drawing.Color.Gainsboro;
		}
	}
}
