using AMS.DB;

namespace AMS.Web
{
	using System;
	using System.Web;
	using System.Configuration;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Web.Security;
	using System.ServiceProcess;
	
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Web.SessionState;
	using System.Data.Odbc;
	using System.Data;


	/// <summary>
	///		Descripción breve de Menu.
	/// </summary>
	public partial class Menu : System.Web.UI.Page
	{
		protected Label infoProcess, lbSystemName, lbCompanyName, lb;
		protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];
		protected string nameSystem = ConfigurationManager.AppSettings["SystemName"] + ".";
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected string process;
		protected string userName;
		protected System.Web.UI.HtmlControls.HtmlForm Form;
		protected string userCod;
		protected string path="";
		protected string usuario;
		protected DataSet ds = new DataSet();
		protected DataSet ds1 = new DataSet();
		protected System.Web.UI.WebControls.TextBox Textbox1;
		protected System.Web.UI.WebControls.TextBox Textbox2;
		private Boolean yapinto = true;

		private string buscado = "";
	
		
		public Menu()
		{
			Page.Init += new System.EventHandler(Page_Load);
		}
  	
		private void Proceso_Logueo()
		{
			if(Request.QueryString["first"]==null)
			{
				//Si se ingresa al servidor de ecas con un usuario distinto a invitado
				if(HttpContext.Current.Server.MachineName=="WEBMASTER" && HttpContext.Current.User.Identity.Name.ToLower()!="invitado")
				{
					//Si el usuario no ha ingresado y no tiene ip registrada
					if((DBFunctions.SingleData("SELECT susu_flagingr FROM susuario WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"'")=="N")&&(DBFunctions.SingleData("SELECT susu_ipaddr FROM susuario WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"'")==""))
					{
						//Cargo el menu y pongo el flag de ingreso
						BuildMenu();
						DBFunctions.NonQuery("UPDATE susuario SET susu_flagingr='L', susu_ipaddr='"+Request.UserHostName.Trim()+"' WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"'");
					}
						//Si el usuario esta registrado con anterioridad
					else if(DBFunctions.SingleData("SELECT susu_flagingr FROM susuario WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"'")=="L")
					{
						//Si la ip de donde esta ingresando es la misma que la que ya esta registrada en la BD
						if(DBFunctions.SingleData("SELECT susu_ipaddr FROM susuario WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"'")==Request.UserHostName)
							//Cargo el menu, no es necesario poner flag, porq yo lo tiene
							BuildMenu();
						else
						{
							//Termino la sesión, lo deslogueo, lo mando al default y muestro el error
							Session.Abandon();
							FormsAuthentication.SignOut();
							Response.Redirect(indexPage+"?error=erruser");
						}
					}
				}
				//Si se ingresa al servidor de ecas como invitado
				else if(HttpContext.Current.Server.MachineName=="WEBMASTER" && HttpContext.Current.User.Identity.Name.ToLower()=="invitado")
				{
					//Cargo el menu
					BuildMenu();
				}
				//Si se ingresa a cualquier otro servidor
				else if(HttpContext.Current.Server.MachineName!="WEBMASTER")
				{
					//Si el usuario no ha ingresado y no tiene ip registrada
					//if((DBFunctions.SingleData("SELECT susu_flagingr FROM susuario WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"'")=="N")&&(DBFunctions.SingleData("SELECT susu_ipaddr FROM susuario WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"'")==""))
					if((DBFunctions.SingleData("SELECT susu_flagingr FROM susuario WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"'")=="N"))
					{
						//Borrar IP anterior
						DBFunctions.NonQuery("UPDATE susuario SET susu_ipaddr=null WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"'");
						//Cargo el menu y pongo el flag de ingreso
						BuildMenu();
						DBFunctions.NonQuery("UPDATE susuario SET susu_flagingr='L', susu_ipaddr='"+Request.UserHostName.Trim()+"' WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"'");
					}
						//Si el usuario esta registrado con anterioridad
					else if(DBFunctions.SingleData("SELECT susu_flagingr FROM susuario WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"'")=="L")
					{
						//Si la ip de donde esta ingresando es la misma que la que ya esta registrada en la BD
                        if (DBFunctions.SingleData("SELECT susu_ipaddr FROM susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToString().ToLower() + "'") == Request.UserHostName)
                        //Cargo el menu, no es necesario poner flag, porq yo lo tiene
                            BuildMenu();
                        else
                        {
                            Response.Redirect("AMS.Web.CerrarSesion.aspx?error=erruser");
                            //Termino la sesión, lo deslogueo, lo mando al default y muestro el error
                            //Session.Abandon();
                            //FormsAuthentication.SignOut();
                            //Response.Redirect(indexPage+"?error=erruser");
                            //this.RegisterStartupScript("redirect","<script>parent.location.href = 'AMS.Web.Inicio.aspx';</script>");
                        }
					}
				}
			}
			else
			{
				process = Request.QueryString["process"];
			}
		}
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
            string funcstr = @"function ManualSelect(sel, obj){ trvMenu.selectedNodeID='5';alert(sel); }";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ManualSelect", funcstr, true);

            if (!IsPostBack)
            {
                trvMenu.HoverNodeStyle.CssClass = "HoveredNode";
                trvMenu.LeafNodeStyle.CssClass = "DefaultNode";
                trvMenu.SelectedNodeStyle.CssClass = "SelectedNode";
                trvMenu.ControlStyle.CssClass = "DefaultNode";
                trvMenu.NodeStyle.CssClass = "DefaultNode";
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["PantallaBienvenida"]) == true)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "top.frames['Opciones'].location = 'AMS.Web.Index.aspx?process=Asousados.UltimosOfertados'", true);
                }
            }
            else yapinto = false;
            Sistema.Value = ConfigurationManager.AppSettings["SystemName"];
			Label3.Text = ConfigurationManager.AppSettings["CompanyName"];
			usuario =  HttpContext.Current.User.Identity.Name;
			Proceso_Logueo();
            
		}

        protected void EventoMenu(object sender, EventArgs e)
        {
            TreeNode node = trvMenu.SelectedNode;
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "cargaModulo", "top.frames['Opciones'].location.href = '" + node.Value + "';", true);
            
            Page.ClientScript.RegisterStartupScript(this.GetType(), "cargaModulo",
                        "if(top.frames['Opciones'].location.href == '" + node.Value + "') {" +
                        "    top.frames['Opciones'].location.reload(true); " +
                        "} else {" +
                        "    top.frames['Opciones'].location.href = '" + node.Value + "' };", true);
        }

		private void InitializeComponent()
		{

		}

		protected void Buscar_Click(object sender, System.EventArgs e)
		{
            try
			{
				string tt = look.Text;
				string navegar = "";
				string Sql = 
                    "SELECT SMEN.smen_opcion, SMEN.smen_url FROM smenu SMEN, spermiso SPER " +
                    "WHERE SMEN.smen_codigo=SPER.smen_codigo AND SPER.ttipe_codigo=(SELECT ttipe_codigo " +
                    "FROM susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "') " +
                    "AND SMEN.smen_opcion like '%" + tt + "%' " +
                    "ORDER BY SMEN.smen_opcion ASC;" ;

				ds1 = DBFunctions.Request(ds1, IncludeSchema.NO, Sql);
				buscado += "<div class=\"ListBox\" style=\"width:100%;height:100%;overflow:auto;\">";
				buscado += "<table class=\"ResultsTable\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">";
			
				for(int i=0; i<ds1.Tables[0].Rows.Count; i++)
				{
					path = ds1.Tables[0].Rows[i][0].ToString();
					navegar = ds1.Tables[0].Rows[i][1].ToString() + "&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&path=" + path;
					buscado += "<tr><td class=\"ResultCell\"><nobr><a class='ResultLink' href='" + navegar + "' target='Opciones'>" + path + "</a></nobr></td>";
				}
				buscado += "</table></div>";
				Resultado.Text = buscado;
			}
			catch ( Exception Ex)
			{
				buscado = Ex.Message;

			}
		}
		

		override protected void OnInit( EventArgs e )
		{
			InitializeComponent();
			base.OnInit(e);
		}

		protected void BuildMenu()
		{
			if (yapinto)
			{
				TreeNode root = new TreeNode();
				root.Text = "Menu Principal";
				root.Expanded = true;
                root.SelectAction = TreeNodeSelectAction.Expand;
				try
				{
                    //ds = DBFunctions.Request(ds, IncludeSchema.NO, "select SMEN.smen_opcion FROM dbxschema.smenu SMEN WHERE smen.smen_padre = 'ND' ORDER BY SMEN.smen_opcion ASC;SELECT SMEN.smen_opcion, SMEN.smen_padre, SMEN.smen_url FROM smenu SMEN, spermiso SPER WHERE SMEN.smen_codigo=SPER.smen_codigo AND SPER.ttipe_codigo=(SELECT ttipe_codigo FROM susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "') ORDER BY SMEN.smen_opcion ASC;SELECT SMEN.smen_opcion, SMEN.smen_padre, SMEN.smen_url FROM dbxschema.smenu SMEN WHERE smen_url = 'ND' and SMEN.smen_opcion in(select distinct smen_padre from dbxschema.vmenu1 where ttipe_codigo = (SELECT ttipe_codigo FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "')) order by smen.smen_padre,smen.smen_opcion");
					
                    ds = DBFunctions.Request(ds, IncludeSchema.NO, 
                    "select SMEN.smen_opcion FROM dbxschema.smenu SMEN WHERE smen.smen_padre = 'ND' " +
                    "and smen_opcion IN ( " +
                    "SELECT SMEN.smen_padre FROM dbxschema.smenu SMEN WHERE smen_url = 'ND' and SMEN.smen_opcion in " +
                    "(select distinct smen_padre from dbxschema.vmenu1 where ttipe_codigo = (SELECT ttipe_codigo FROM dbxschema.susuario " +
                    "WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "')) " +
                    ")ORDER BY SMEN.smen_opcion ASC; " +
                    "SELECT SMEN.smen_opcion, SMEN.smen_padre, SMEN.smen_url, SMEN.smen_manual, SMEN.smen_manualopt FROM smenu SMEN, spermiso SPER WHERE SMEN.smen_codigo=SPER.smen_codigo " +
                    "AND SPER.ttipe_codigo=(SELECT ttipe_codigo FROM susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "') " + 
                    "ORDER BY SMEN.smen_opcion ASC; " +
                    "SELECT SMEN.smen_opcion, SMEN.smen_padre, SMEN.smen_url FROM dbxschema.smenu SMEN WHERE smen_url = 'ND' and SMEN.smen_opcion in " +
                    "(select distinct smen_padre from dbxschema.vmenu1 where ttipe_codigo = (SELECT ttipe_codigo FROM dbxschema.susuario " +
                    "WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "')) order by smen.smen_padre,smen.smen_opcion; ");

                    for(int i=0; i<ds.Tables[0].Rows.Count; i++)
					{
						TreeNode tn = new TreeNode();
						tn.Text = ds.Tables[0].Rows[i][0].ToString();
                        tn.Expanded = false;
                        tn.SelectAction = TreeNodeSelectAction.Expand;
                        //tn.SelectAction = TreeNodeSelectAction.SelectExpand;

						root.ChildNodes.Add(tn);
						ItemsFromLevelTwo(ds.Tables[0].Rows[i][0].ToString(),tn);
					}
					TreeNode tn1 = new TreeNode();
					tn1.Text = "Terminar Sesion";
					tn1.NavigateUrl = "AMS.Web.CerrarSesion.aspx";
					tn1.Target = "Opciones";

					root.ChildNodes.Add( tn1 );
				}
				catch(Exception e)
				{
					string rr = e.Message;
				}
				root.Expanded = true;
				trvMenu.Nodes.Add( root );

				yapinto= false;
			}
		}

		protected void ItemsFromLevelTwo(string father, TreeNode Papa)
		{
			for(int i=0; i<ds.Tables[2].Rows.Count; i++)
			{
				if(father == ds.Tables[2].Rows[i][1].ToString())
				{
					TreeNode tn = new TreeNode();
					tn.Text = ds.Tables[2].Rows[i][0].ToString();
					Papa.ChildNodes.Add( tn );
                    tn.Expanded = false;
                    tn.SelectAction = TreeNodeSelectAction.Expand;

                    //tn.NavigateUrl = "javascript:ManualSelect('trvMenu',this)";

					ItemsFromLevelThree(ds.Tables[2].Rows[i][0].ToString(), tn);
				}
			}
		}

		protected void ItemsFromLevelThree(string father,TreeNode Abuelo)
		{
			for(int i=0; i<ds.Tables[1].Rows.Count; i++)
			{
				if(father == ds.Tables[1].Rows[i][1].ToString())
				{
                    TreeNode tn = new TreeNode();
					path = ds.Tables[1].Rows[i][0].ToString();
                    string manual = ds.Tables[1].Rows[i][3] != null ? ds.Tables[1].Rows[i][3].ToString() : null;
                    string manualOpt = ds.Tables[1].Rows[i][4] != null ? ds.Tables[1].Rows[i][4].ToString() : null;
                    string url = ds.Tables[1].Rows[i][2].ToString() + "&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&path=" + path;

                    if (manual != null && manual != "")
                        url += String.Format("&manual={0}&manualOpt={1}", manual, manualOpt);

                    tn.Text = ds.Tables[1].Rows[i][0].ToString();
                    tn.NavigateUrl = url;
					tn.Target = "Opciones";
                    tn.Value = url;

					Abuelo.ChildNodes.Add(tn);
				}
			}
		}


	}
}