using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using AMS.DB;
using AMS.DBManager;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Finanzas.Cartera
{
	public partial  class PruebaXML2 : System.Web.UI.UserControl
	{
		protected DataSet ds;
		protected ArrayList sqlStrings;
			
		protected void Page_Load(object Sender, EventArgs e)
		{
			ds=new DataSet();
			sqlStrings=new ArrayList();
			int numeroRC=0,numeroCE;
			numeroRC=Convert.ToInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu FROM pdocumento WHERE pdoc_codigo='ANRC'"));
			numeroCE=Convert.ToInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu FROM pdocumento WHERE pdoc_codigo='ANCE'"));
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT * FROM dbxschema.manulacioncaja");
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				if(DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='"+ds.Tables[0].Rows[i][2].ToString()+"'")=="RC")
				{
					sqlStrings.Add("UPDATE manulacioncaja SET manu_numeanul="+(numeroRC+1)+" WHERE pdoc_codigo='"+ds.Tables[0].Rows[i][2].ToString()+"' AND mcaj_numero="+ds.Tables[0].Rows[i][3].ToString()+"");
					numeroRC++;
				}
				else if(DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='"+ds.Tables[0].Rows[i][2].ToString()+"'")=="CE")
				{
					sqlStrings.Add("UPDATE manulacioncaja SET manu_numeanul="+(numeroCE+1)+" WHERE pdoc_codigo='"+ds.Tables[0].Rows[i][2].ToString()+"' AND mcaj_numero="+ds.Tables[0].Rows[i][3].ToString()+"");
					numeroCE++;
				}
				else if(DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='"+ds.Tables[0].Rows[i][2].ToString()+"'")=="RP")
				{
					sqlStrings.Add("UPDATE manulacioncaja SET manu_numeanul="+(numeroRC+1)+" WHERE pdoc_codigo='"+ds.Tables[0].Rows[i][2].ToString()+"' AND mcaj_numero="+ds.Tables[0].Rows[i][3].ToString()+"");
					numeroRC++;
				}
			}
			sqlStrings.Add("UPDATE dbxschema.pdocumento set pdoc_ultidocu="+numeroRC+" WHERE pdoc_codigo='ANRC'");
			sqlStrings.Add("UPDATE dbxschema.pdocumento set pdoc_ultidocu="+numeroCE+" WHERE pdoc_codigo='ANCE'");
			if(DBFunctions.Transaction(sqlStrings))
				lb.Text+="Bien "+DBFunctions.exceptions;
			else
				lb.Text+="Error "+DBFunctions.exceptions;
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
