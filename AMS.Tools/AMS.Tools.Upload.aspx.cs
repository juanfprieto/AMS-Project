using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace AMS.Tools
{
    public partial class AMS_Tools_Upload : System.Web.UI.Page
    {
        protected string pathToImages = ConfigurationSettings.AppSettings["PathToImages"];
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpPostedFile file = null;
            if (Request.Files.Count > 0)
            {
                file = Request.Files[0];
                string [] witdh = Request.Headers.GetValues("X-File-Width");

                Random rnd = new Random();
                int dice = rnd.Next(100, 999); // creates a number between 1 and 12

                string imagePath = pathToImages + "imgManuales/" + dice + "_" + file.FileName;
                file.SaveAs(Server.MapPath(imagePath));
                System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath(imagePath ));
                Response.Output.WriteLine(imagePath + "*" + img.Width + "*");
            }
        }
    }
}