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
	public partial  class EjecutarEfectividad : System.Web.UI.UserControl
	{
		protected DataTable cheques;
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
				fecha.Text=DateTime.Now.AddDays(Convert.ToDouble(DBFunctions.SingleData("SELECT ctes_diasefec FROM ctesoreria"))*-1).ToString("yyyy-MM-dd");
			else
			{
				if(Session["cheques"]!=null)
					cheques=(DataTable)Session["cheques"];
			}
		}
		
		protected void Cargar_TablaCheques()
		{
			cheques=new DataTable();
			cheques.Columns.Add("NUMEROCHEQUE",typeof(string));
			cheques.Columns.Add("PREFIJOTESORERIA",typeof(string));
			cheques.Columns.Add("NUMEROTESORERIA",typeof(string));
			cheques.Columns.Add("PREFIJORECIBO",typeof(string));
			cheques.Columns.Add("NUMERORECIBO",typeof(string));
			cheques.Columns.Add("TIPOPAGO",typeof(string));
			cheques.Columns.Add("VALOR",typeof(double));
		}
		
		protected void Cambiar_Fecha(object Sender,EventArgs e)
		{
			if(calendarioFecha.SelectedDate.Date>DateTime.Now.Date)
                Utils.MostrarAlerta(Response, "La fecha es invalida, debe ser menor o igual a la fecha actual");
			else
				fecha.Text=calendarioFecha.SelectedDate.Date.ToString("yyyy-MM-dd");
		}
		
		protected void btnAceptar_Click(object Sender,EventArgs e)
		{
			DataRow fila;
			DateTime fechaReal=Convert.ToDateTime(fecha.Text);
			DataSet ds=new DataSet();
			ArrayList sqlStrings=new ArrayList();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT D.dtes_numerodoc,M.pdoc_codigo,M.mtes_numero,C.pdoc_codigo,C.mcaj_numero,P.ttip_codigo,D.dtes_valor  FROM dbxschema.mtesoreria M,dbxschema.dtesoreriadocumentos D,dbxschema.mcajapago P,dbxschema.mcaja C WHERE M.pdoc_codigo=D.mtes_codigo AND M.mtes_numero=D.mtes_numero AND C.pdoc_codigo=D.mcaj_codigo AND C.mcaj_numero=D.mcaj_numero AND C.pdoc_codigo=P.pdoc_codigo AND C.mcaj_numero=P.mcaj_numero AND P.test_estado='G' AND M.mtes_fecha<='"+fechaReal.ToString("yyyy-MM-dd")+"' AND P.ttip_codigo IN('C','T','D')");
			if(ds.Tables[0].Rows.Count!=0)
			{
				if(Session["cheques"]==null)
					this.Cargar_TablaCheques();
				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					fila=cheques.NewRow();
					fila[0]=ds.Tables[0].Rows[i][0].ToString();
					fila[1]=ds.Tables[0].Rows[i][1].ToString();
					fila[2]=ds.Tables[0].Rows[i][2].ToString();
					fila[3]=ds.Tables[0].Rows[i][3].ToString();
					fila[4]=ds.Tables[0].Rows[i][4].ToString();
					fila[5]=ds.Tables[0].Rows[i][5].ToString();
					fila[6]=Convert.ToDouble(ds.Tables[0].Rows[i][6]);
					cheques.Rows.Add(fila);
					Session["cheques"]=cheques;
				}
				for(int i=0;i<cheques.Rows.Count;i++)
				{
					sqlStrings.Add("UPDATE mcajapago SET test_estado='E' WHERE mcpag_numerodoc='"+cheques.Rows[i][0].ToString()+"' AND ttip_codigo='"+cheques.Rows[i][5].ToString()+"' AND test_estado='G' AND pdoc_codigo='"+cheques.Rows[i][3].ToString()+"' AND mcaj_numero="+cheques.Rows[i][4].ToString()+"");
					sqlStrings.Add("UPDATE mtesoreriasaldos SET mtes_saldoencanje=mtes_saldoencanje-"+Convert.ToDouble(cheques.Rows[i][6])+" WHERE mtes_codigo='"+cheques.Rows[i][1].ToString()+"' AND mtes_numero="+cheques.Rows[i][2].ToString()+"");
				}
				if(DBFunctions.Transaction(sqlStrings))
				{
					lb.Text+="<br>"+DBFunctions.exceptions;
					Session.Clear();
				}
				else
				{
					lb.Text+="Error <br>"+DBFunctions.exceptions;
				}
			}
			else
            Utils.MostrarAlerta(Response, "No hay cheques para ejecutar");
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
