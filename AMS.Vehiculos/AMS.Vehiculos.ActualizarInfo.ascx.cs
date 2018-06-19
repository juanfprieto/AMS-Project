namespace AMS.Vehiculos
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
	using System.Web.Caching;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Vehiculos_ActualizarInfo.
	/// </summary>
	public partial class AMS_Vehiculos_ActualizarInfo : System.Web.UI.UserControl
	{
		string PCAT_CODIGO, MCAT_VIN;
        int    MVEH_INVENTARIO;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				DatasToControls param = new  DatasToControls();
				pan1.Visible=false;
                param.PutDatasIntoDropDownList(ddlVehiculo, "SELECT mc.PCAT_CODIGO CONCAT '/' concat MV.MCAT_VIN CONCAT '/' concat mveh_inventario  FROM DBXSCHEMA.MVEHICULO mv, DBXSCHEMA.McatalogoVEHICULO mc where mc.mcat_vin = mv.mcat_vin");
				string [] vehi= ddlVehiculo.SelectedValue.Split('/');
				if(vehi.Length==0)
				{
                    Utils.MostrarAlerta(Response, "No tiene vehículos para actualizar la información");
				}
				else
				{
					PCAT_CODIGO=vehi[0];
					MCAT_VIN= vehi[1];
                    MVEH_INVENTARIO = Convert.ToInt32(vehi[2].ToString());
				}

                imglupa.Attributes.Add("OnClick", "ModalDialog(" + ddlVehiculo.ClientID + ",'SELECT mc.PCAT_CODIGO CONCAT \\'/\\' concat mveh.MCAT_VIN AS CATVIN,MC.PCAT_CODIGO AS CATALOGO,MVEH.MCAT_VIN AS VIN FROM DBXSCHEMA.MVEHICULO MVEH, DBXSCHEMA.McatalogoVEHICULO Mc where mvEH.mcat_vin = mc.mcat_vin' ,new Array() );");
			}
			else
			{
				if (Session["PCAT_CODIGO"]!=null)
					PCAT_CODIGO=(string) Session["PCAT_CODIGO"];

				if (Session["MCAT_VIN"]!=null)
					MCAT_VIN=(string) Session["MCAT_VIN"];

                if (Session["MVEH_INVENTARIO"] != null)
                    MVEH_INVENTARIO = (int)Session["MVEH_INVENTARIO"];
			}
			

		}

		protected void cargaDatos(object sender, System.EventArgs e)
		{
			DataSet info = new DataSet();
			string [] vehi= ddlVehiculo.SelectedValue.Split('/');
			PCAT_CODIGO=vehi[0];
            MCAT_VIN = vehi[1];
			MVEH_INVENTARIO = Convert.ToInt32(vehi[2].ToString());
			DBFunctions.Request(info,IncludeSchema.NO,"Select mveh_numemani,mveh_fechmani,mveh_numed_o,mveh_numelevante,mveh_aduana from DBXSCHEMA.MVEHICULO where mcat_vin='"+MCAT_VIN+"'");
			txtFechaMan.Text=Convert.ToDateTime(info.Tables[0].Rows[0][1]).ToString("yyyy-MM-dd");
			txtNumMan.Text=info.Tables[0].Rows[0][0].ToString();
			txtNumDO.Text=info.Tables[0].Rows[0][2].ToString();
			txtAduana.Text=info.Tables[0].Rows[0][4].ToString();
			txtLevante.Text=info.Tables[0].Rows[0][3].ToString();
			pan1.Visible=true;
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
		///		Método necesario para admitir el Diseñador. No se puede modificar
		///		el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		
		protected void ActualizarInfo(object sender, System.EventArgs e)
		{
			string [] vehis= ddlVehiculo.SelectedValue.Split('/');
			PCAT_CODIGO=vehis[0];
			MCAT_VIN = vehis[1];
          //  vehis = ddlVehiculo.SelectedValue.Split('#');
          //  string mveh_inventario = vehis[1];
            MVEH_INVENTARIO = Convert.ToInt32(vehis[2].ToString());

            DBFunctions.NonQuery("update DBXSCHEMA.MVEHICULO set mveh_numemani='" + txtNumMan.Text + "' ,mveh_fechmani='" + txtFechaMan.Text + "',mveh_numed_o='" + txtNumDO.Text + "',mveh_numelevante='" + txtLevante.Text + "',mveh_aduana='" + txtAduana.Text + "' where mcat_vin='" + MCAT_VIN + "' and mveh_inventario = "+ MVEH_INVENTARIO +" ");
            Utils.MostrarAlerta(Response, "Cambios Realizados con exito..");
		}

		

		
	}
}
