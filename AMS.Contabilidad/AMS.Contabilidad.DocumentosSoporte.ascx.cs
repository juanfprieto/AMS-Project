using AMS.DB;
using AMS.Forms;
using AMS.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMS.Contabilidad
{
    public partial class AMS_Contabilidad_DocumentosSoporte : System.Web.UI.UserControl
    {
        private DatasToControls bind = new DatasToControls();
        protected DataSet dsDocumentos = new DataSet();
        DataTable dtDatos = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                bind.PutDatasIntoDropDownList(ddlPrefijo, "SELECT PDOC_CODIGO, PDOC_CODIGO || ' - ' || PDOC_NOMBRE FROM PDOCUMENTO WHERE TVIG_VIGENCIA = 'V'");
                //ddlPrefijo.Items.Insert(0, new ListItem("Seleccione...", "0"));

            }
            if (Session["docuSoporte"] != null)
                dtDatos = (DataTable)Session["docuSoporte"];
            else
            prepararTablaDocumentos();
        }

        protected void cambioPrefijo(object sender, EventArgs z)
        {
            int n;
            bool isNumeric = int.TryParse(txtNumero.Text, out n);
            //if(ddlPrefijo.SelectedValue != "0")
            //txtNumero.Attributes.Add("ondbclick", "ModalDialog(this, 'SELECT MCOM_NUMEDOCU FROM MCOMPROBANTE WHERE PDOC_CODIGO = \\'" + ddlPrefijo.SelectedValue + "\\', new Array())");
            if (txtNumero.Text.Length > 0 && isNumeric)
            {
                if(DBFunctions.RecordExist("SELECT MCOM_NUMEDOCU FROM MCOMPROBANTE WHERE PDOC_CODIGO = '" + ddlPrefijo.SelectedValue + "' AND MCOM_NUMEDOCU = " + txtNumero.Text))
                {
                    DBFunctions.Request(dsDocumentos, IncludeSchema.NO, "SELECT PDOC_CODIGO AS PREFIJO, MCOM_NUMEDOCU AS NUMERO, MCOD_NOMBDOCUMENTO, MCOD_NOMBARCHIVO FROM MCOMPROBANTEDOCUMENTO WHERE PDOC_CODIGO = '" + ddlPrefijo.SelectedValue + "' AND MCOM_NUMEDOCU = " + txtNumero.Text);
                    if(dsDocumentos.Tables[0].Rows.Count > 0)
                    {
                        //llenar grid
                        dtDatos = dsDocumentos.Tables[0];
                        dgDocumentos.DataSource = dtDatos;
                        Session["docuSoporte"] = dtDatos;
                    }
                    else
                    {
                        Utils.MostrarAlerta(Response, "No existen documentos soporte para este comprobante.");
                        dtDatos.Clear();
                        dgDocumentos.DataSource = dtDatos;
                        Session["docuSoporte"] = null;


                    }
                    dgDocumentos.DataBind();
                }
                else
                {
                    Utils.MostrarAlerta(Response, "El comprobante: " + ddlPrefijo.SelectedValue + " - " + txtNumero.Text + " No se encontró, revise por favor.");
                }
            }
            else
            {
                Utils.MostrarAlerta(Response, "Tiene que escribir un número");
            }
        }
        protected void descargarDocumento(object sender, DataGridCommandEventArgs z)
        {
            int posicion = z.Item.ItemIndex;
            string nombArchivo = dtDatos.Rows[posicion]["MCOD_NOMBARCHIVO"].ToString();
            /*
                HttpPostedFile file = Request.Files[nombArchivo];
                //check file was submitted
                if (file != null && file.ContentLength > 0)
                {
                    string fname = Path.GetFileName(file.FileName);
                    file.SaveAs(Server.MapPath(Path.Combine("../documentos_contables/", fname)));
                }
            */
            string lbResume = "../documentos_contables/" + nombArchivo;
            string lbResume2 = "..\\documentos_contables\\" + nombArchivo;
            if (lbResume != string.Empty)
            {
                WebClient req = new WebClient();
                HttpResponse response = HttpContext.Current.Response;
                try
                {
                    //string filePath = lbResume;
                    string filePath = lbResume2;
                    response.ClearContent();
                    response.Clear();
                    response.ContentType = "image/JPEG";//"application/x-cdf";//"text/plain";
                    response.AddHeader("Content-Disposition", "attachment; filename=" + nombArchivo + ";");
                    //response.TransmitFile(Server.MapPath(filePath));
                    byte[] data = req.DownloadData(Server.MapPath(filePath));
                    response.BinaryWrite(data);
                    response.Flush();
                    response.End();

                    /*response.Clear();
                    //response.ClearContent();
                    //response.ClearHeaders();
                    response.Buffer = true;
                    response.AddHeader("Content-Disposition", "attachment;filename=" + nombArchivo + "");
                    byte[] data = req.DownloadData(Server.MapPath(filePath));
                    response.BinaryWrite(data);
                    response.End();*/
                }
                catch(Exception g)
                {
                    //req.CancelAsync();
                    //response.End();
                    Utils.MostrarAlerta(response, "Se generó un problema al trata de descargar el documento. " + g.Message);
                    return;
                }
                
            }


        }
        protected void prepararTablaDocumentos()
        {
            dtDatos = new DataTable();
            dtDatos.Columns.Add(new DataColumn("PREFIJO", System.Type.GetType("System.String")));
            dtDatos.Columns.Add(new DataColumn("NUMERO", System.Type.GetType("System.String")));
            dtDatos.Columns.Add(new DataColumn("MCOD_NOMBDOCUMENTO", System.Type.GetType("System.String")));
            dtDatos.Columns.Add(new DataColumn("MCOD_NOMBARCHIVO", System.Type.GetType("System.String")));
        }

    }
}