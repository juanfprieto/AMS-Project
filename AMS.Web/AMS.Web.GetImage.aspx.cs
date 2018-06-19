using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Drawing;

namespace AMS.Web
{
	public partial class GetImage : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            string img = Request.QueryString["img"];
            string ruta = ConfigurationManager.AppSettings["ImgServPath"];//"\\\\10.51.250.131\\Ecommerce\\";

            if (img == null || img == "")
            {
                Response.WriteFile("~/img/empty.jpg");
                Response.ContentType = "image/jpg";
                return;
            } 

            try
            {
                System.Drawing.Image newImage = new Bitmap(340, 340);
                System.Drawing.Image image = System.Drawing.Image.FromFile(String.Format("{0}{1}.jpg", ruta, img));
                Graphics g = Graphics.FromImage(newImage);
                g.DrawImage(image, 0, 0, 340, 340);

                newImage.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                Response.ContentType = "image/jpg";
            }
            catch
            {
                Response.WriteFile("~/img/empty.jpg");
                Response.ContentType = "image/jpg";
            }
		}
	}
}