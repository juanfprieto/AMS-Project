using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using System.Configuration;
using AMS.Tools;


namespace AMS.Finanzas.Tesoreria
{
    public partial class ConciliacionManual : System.Web.UI.UserControl
    {
        protected DataTable dtSobBan, dtSobTes;

        protected void Page_Load(object Sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Llenar_dgSobBan();
                this.Llenar_dgSobTes();
            }
            else
            {
                if (Session["dtSobBan"] != null)
                    dtSobBan = (DataTable)Session["dtSobBan"];
                           
                 if (Session["dtSobTes"] != null)
                    dtSobTes = (DataTable)Session["dtSobTes"];
                
            }
        }

        protected void Cargar_dtSobBan()
        {
            dtSobBan = new DataTable();
            dtSobBan.Columns.Add("NUMEROBAN", typeof(string));
            dtSobBan.Columns.Add("VALORBAN", typeof(double));
        }

        protected void Cargar_dtSobTes()
        {
            dtSobTes = new DataTable();
            dtSobTes.Columns.Add("PREFTES", typeof(string));
            dtSobTes.Columns.Add("NUMTES", typeof(string));
            dtSobTes.Columns.Add("VALORTES", typeof(double));
        }

        protected void Llenar_dgSobBan()
        {
            DataRow fila;
            if (Session["dtConciliacion"] != null)
            {
                if (Request.QueryString["col"] == "3")
                {
                    if (dtSobBan == null)
                        this.Cargar_dtSobBan();

                    for (int i = 0; i < ((DataTable)Session["dtConciliacion"]).Rows.Count; i++)
                    {
                        if (!Convert.ToBoolean(((DataTable)Session["dtConciliacion"]).Rows[i][3]))
                        {
                            fila = dtSobBan.NewRow();
                            fila[0] = ((DataTable)Session["dtConciliacion"]).Rows[i][1].ToString();
                            fila[1] = Convert.ToDouble(((DataTable)Session["dtConciliacion"]).Rows[i][2]);
                            dtSobBan.Rows.Add(fila);
                            dgSobBan.DataSource = dtSobBan;
                            dgSobBan.DataBind();
                            Session["dtSobBan"] = dtSobBan;
                        }
                    }
                }
                else if (Request.QueryString["col"] == "4")
                {
                    if (dtSobBan == null)
                        this.Cargar_dtSobBan();



                    for (int i = 0; i < ((DataTable)Session["dtConciliacion"]).Rows.Count; i++)
                    {
                        if (!Convert.ToBoolean(((DataTable)Session["dtConciliacion"]).Rows[i][4]))
                        {
                            fila = dtSobBan.NewRow();
                            fila[0] = ((DataTable)Session["dtConciliacion"]).Rows[i][1].ToString();
                            if (Convert.ToDouble(((DataTable)Session["dtConciliacion"]).Rows[i][2]) != 0)
                                fila[1] = Convert.ToDouble(((DataTable)Session["dtConciliacion"]).Rows[i][2]);
                            else if (Convert.ToDouble(((DataTable)Session["dtConciliacion"]).Rows[i][3]) != 0)
                                fila[1] = Convert.ToDouble(((DataTable)Session["dtConciliacion"]).Rows[i][3]) * -1;
                            dtSobBan.Rows.Add(fila);
                            dgSobBan.DataSource = dtSobBan;
                            dgSobBan.DataBind();
                            Session["dtSobBan"] = dtSobBan;
                        }
                    }
                }
            }
            else
                Utils.MostrarAlerta(Response, "Error Interno. Repita el proceso completo");
        }

        protected void Llenar_dgSobTes()
        {
            DataRow fila;
            if (Session["dtMovimiento"] != null)
            {
                if (dtSobTes == null)
                    this.Cargar_dtSobTes();
                for (int i = 0; i < ((DataTable)Session["dtMovimiento"]).Rows.Count; i++)
                {
                    if (!Convert.ToBoolean(((DataTable)Session["dtMovimiento"]).Rows[i][4]))
                    {
                        fila = dtSobTes.NewRow();
                        fila[0] = ((DataTable)Session["dtMovimiento"]).Rows[i][1].ToString();
                        fila[1] = ((DataTable)Session["dtMovimiento"]).Rows[i][2].ToString();
                        fila[2] = Convert.ToDouble(((DataTable)Session["dtMovimiento"]).Rows[i][3]);
                        dtSobTes.Rows.Add(fila);
                        dgSobTes.DataSource = dtSobTes;
                        dgSobTes.DataBind();
                        Session["dtSobTes"] = dtSobTes;
                    }
                }
            }
        }

        protected void btnEjecutar_Click(object Sender, EventArgs e)
        {
            //Si hay datos para realizar cruces manuales, verifico las grillas
            ArrayList sqlStrings = new ArrayList();
            if (dgSobTes.Items.Count != 0 && dgSobBan.Items.Count != 0)
            {
                if (Validar_Grillas(dgSobBan, 2) || Validar_Grillas(dgSobTes, 3))
                    Utils.MostrarAlerta(Response, "No ha chequeado ninguna casilla en alguna de las tablas");

                if (Validar_Sumas())
                    Utils.MostrarAlerta(Response, "La suma de las cantidades es distinta. Revise");
                else
                {
                    this.Alistar_Conciliacion(ref sqlStrings);
                    if (DBFunctions.Transaction(sqlStrings))
                    {
                       

                        lb.Text = DBFunctions.exceptions;
                        Session.Clear();
                    }
                    else
                        lb.Text = "Error " + DBFunctions.exceptions;
                    /*for(int i=0;i<sqlStrings.Count;i++)
                        lb.Text+=sqlStrings[i]+"<br>";*/
                }
            }
            else
            {
                this.Alistar_Conciliacion(ref sqlStrings);
                if (DBFunctions.Transaction(sqlStrings))
                {


                    lb.Text = DBFunctions.exceptions;
                    Session.Clear();
                }
                else
                    lb.Text = "Error " + DBFunctions.exceptions;
                /*for(int i=0;i<sqlStrings.Count;i++)
                    lb.Text+=sqlStrings[i]+"<br>";*/
            }
        }

        protected bool Validar_Grillas(DataGrid grilla, int pos)
        {
            bool error = false;
            int cont = 0;
            for (int i = 0; i < grilla.Items.Count; i++)
            {
                if (!((CheckBox)grilla.Items[i].Cells[pos].Controls[1]).Checked)
                    cont++;
            }
            if (cont == grilla.Items.Count)
                error = true;
            return error;
        }

        protected bool Validar_Sumas()
        {
            bool suma = false;
            double valorBan = 0, valorTes = 0;
            for (int i = 0; i < dgSobBan.Items.Count; i++)
            {
                if (((CheckBox)dgSobBan.Items[i].Cells[2].Controls[1]).Checked)
                    valorBan = valorBan + Convert.ToDouble(dtSobBan.Rows[i][1]);
            }

            for (int i = 0; i < dgSobTes.Items.Count; i++)
            {
                if (((CheckBox)dgSobTes.Items[i].Cells[3].Controls[1]).Checked)
                    valorTes = valorTes + Convert.ToDouble(dtSobTes.Rows[i][2]);
            }
            if (valorBan != valorTes)
                suma = true;
            return suma;
        }

        protected void Alistar_Conciliacion(ref ArrayList sqlStrings)
        {
            //Guardo primero la conciliacion automatica
            DataRow[] tesoreria = ((DataTable)Session["dtMovimiento"]).Select("CRUZADO='True'");
            DataRow[] banco = ((DataTable)Session["dtConciliacion"]).Select("CRUZADO='True'");
            for (int i = 0; i < tesoreria.Length; i++)
            {
                for (int j = 0; j < banco.Length; j++)
                {
                    if (Request.QueryString["col"] == "3")
                    {
                        if (tesoreria[i][5].ToString() == banco[j][4].ToString())
                            sqlStrings.Add("INSERT INTO mconciliacion VALUES(default," + Request.QueryString["mes"] + "," + DateTime.Now.Year + ",'" + tesoreria[i][1].ToString() + "'," + tesoreria[i][2].ToString() + ",'" + Request.QueryString["cnt"] + "'," + tesoreria[i][3].ToString() + ",'A','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");
                    }
                    else if (Request.QueryString["col"] == "4")
                    {
                        if (tesoreria[i][5].ToString() == banco[j][5].ToString())
                            sqlStrings.Add("INSERT INTO mconciliacion VALUES(default," + Request.QueryString["mes"] + "," + DateTime.Now.Year + ",'" + tesoreria[i][1].ToString() + "'," + tesoreria[i][2].ToString() + ",'" + Request.QueryString["cnt"] + "'," + tesoreria[i][3].ToString() + ",'A','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");
                    }
                }
            }
            //Despues guardo los cruces manuales


            if (dgSobTes.Items.Count != 0 && dgSobBan.Items.Count != 0)
            {
                int contTeso = 0;
                for (int i = 0; i < dgSobTes.Items.Count; i++)
                {
                    if (((CheckBox)dgSobTes.Items[i].Cells[3].Controls[1]).Checked)
                    {
                        sqlStrings.Add("INSERT INTO mconciliacion VALUES(default," + Request.QueryString["mes"] + "," + DateTime.Now.Year + ",'" + dtSobTes.Rows[i - contTeso][0].ToString() + "'," + dtSobTes.Rows[i - contTeso][1].ToString() + ",'" + Request.QueryString["cnt"] + "'," + dtSobTes.Rows[i - contTeso][2].ToString() + ",'M','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");
                        


                    }
                    else
                        sqlStrings.Add("INSERT INTO mpendientesconciliacion VALUES('" + dtSobTes.Rows[i - contTeso][0].ToString() + "'," + dtSobTes.Rows[i - contTeso][1].ToString() + ",'" + Request.QueryString["cnt"] + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "'," + dtSobTes.Rows[i - contTeso][2].ToString() + ")");
                }
               
                int contBanco = 0;
                for (int i = 0; i < dgSobBan.Items.Count; i++)
                {
                    if (((CheckBox)dgSobBan.Items[i].Cells[2].Controls[1]).Checked)
                    {
                        sqlStrings.Add("INSERT INTO mconciliacion VALUES(default," + Request.QueryString["mes"] + "," + DateTime.Now.Year + ",null,null,'" + Request.QueryString["cnt"] + "'," + dtSobBan.Rows[i - contBanco][1].ToString() + ",'M','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");
                       
                    }
                }

               



            }
        }

        ////////////////////////////////////////////////
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
