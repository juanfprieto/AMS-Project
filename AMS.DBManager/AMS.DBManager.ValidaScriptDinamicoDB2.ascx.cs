using System;
using System.Data;
using AMS.DB;
using System.Web.UI.WebControls;

namespace AMS.DBManager
{
    public partial class ValidaScriptDinamicoDB2 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void VerificarScript(object Sender, EventArgs e)
        {
            if (txtScript.Text == "")
                Tools.Utils.MostrarAlerta(Response, "Digite un script primero en el area de texto!");
            else
            {
                DataSet ds = new DataSet();
                DBFunctions.Request(ds, IncludeSchema.YES, txtScript.Text);
                lblResultado.Text = DBFunctions.exceptions;

                if(ds.Tables.Count > 0)
                {
                    ViewState["Datos"] = ds;
                    grdResultados.DataSource = ds;
                    grdResultados.DataBind();
                }
                else
                {
                    grdResultados.DataBind();
                }
            }
        }

        protected void CambioPagina(Object sender, DataGridPageChangedEventArgs e)
        {
            grdResultados.CurrentPageIndex = e.NewPageIndex;
            
            grdResultados.DataSource = (DataSet) ViewState["Datos"];
            grdResultados.DataBind();
        }
    }
}