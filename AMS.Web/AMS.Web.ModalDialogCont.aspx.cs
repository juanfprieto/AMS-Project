using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AMS.DB;

namespace AMS.Web
{
	public partial class ModalDialogCont : System.Web.UI.Page
	{
        //Constructor
        public ModalDialogCont()
        {
            Page.Init += new System.EventHandler(Page_Load);
        }

		protected void Page_Load(object sender, EventArgs e)
		{
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            if (!IsPostBack)
            {
                DataSet dsTabla = new DataSet();
                String sql = Request.QueryString["sql"].ToString();
                DBFunctions.Request(dsTabla, IncludeSchema.NO, sql);

                if (dsTabla.Tables[0].Rows.Count > 0)
                {
                    dgTable.DataSource = dsTabla.Tables[0];
                    dgTable.DataBind();
                }

            }
		}
	}

}