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
using AMS.Forms;
using AMS.DB;
using AMS.DBManager;
using System.Configuration;

namespace AMS.Garantias
{
	/// <summary>
	/// Descripción breve de AMS_Garantias_VerKit.
	/// </summary>
	public partial class AMS_Garantias_VerKit : System.Web.UI.Page
	{

	
		protected void Page_Load(object sender, System.EventArgs e)
		{	
			DataSet  VerKit = new DataSet();	
			if (!IsPostBack)
			{
				if(Request.QueryString["cod"]!=null)
				{
					Panel1.Visible= false;
					Panel2.Visible= false;
					DBFunctions.Request(VerKit, IncludeSchema.NO, "Select  ptem_operacion,ptem_descripcion  from MKITOPERACION mk inner join ptempario pt on pt.ptem_operacion = mk.mkit_operacion where mkit_codikitoper='"+Request.QueryString["cod"]+"' ; Select  DBXSCHEMA.EDITARREFERENCIAS(MI.MITE_CODIGO,PLI.PLIN_TIPO) as Codigo, mi.mite_nombre as Item  from MKITITEM mk inner join MITEMS mi ON mi.mite_codigo = mk.mkit_coditem inner join PLINEAITEM PLI on   MI.PLIN_CODIGO=PLI.PLIN_CODIGO WHERE  mk.mkit_codikit = '"+Request.QueryString["cod"]+"'");
					dgOperaciones.DataSource = VerKit.Tables[0];
					dgItems.DataSource= VerKit.Tables[1];
					DatasToControls.Aplicar_Formato_Grilla(dgOperaciones);
					DatasToControls.Aplicar_Formato_Grilla(dgItems);
					dgOperaciones.DataBind();
					dgItems.DataBind();
					if (DBFunctions.RecordExist("Select mkit_codikitoper from DBXSCHEMA.MKITOPERACION where mkit_codikitoper = '"+Request.QueryString["cod"]+"'"))
						Panel1.Visible= true;
					if (DBFunctions.RecordExist("Select mkit_codikit from DBXSCHEMA.MKITITEM where mkit_codikit = '"+Request.QueryString["cod"]+"'"))
					Panel2.Visible= true;
				
					
				
				}
			}
		}

		#region Código generado por el Diseñador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Diseñador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion


	}
}
