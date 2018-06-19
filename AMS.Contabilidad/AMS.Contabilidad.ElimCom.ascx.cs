namespace AMS.Contabilidad
{
	using System;
	using System.Configuration;
	using System.IO;
	using System.Collections;
	using System.ComponentModel;
	using System.Data;
	using System.Data.Odbc;
	using System.Drawing;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Xml;
	using AMS.DB;
	using AMS.Forms;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Contabilidad_ElimCom.
	/// </summary>
	public partial class AMS_Contabilidad_ElimCom : System.Web.UI.UserControl
	{

        protected DataTable dtInserts;
        protected DataRow dr;
        protected ArrayList types = new ArrayList();
        protected ArrayList lbFields = new ArrayList();
        protected int i, j;

        protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
                string yr = DBFunctions.SingleData("SELECT pano_ano FROM ccontabilidad");
                bind.PutDatasIntoDropDownList(yearElim, "SELECT pano_ano FROM pano where pano_ano >= "+ yr +" order by 1 desc");
                bind.PutDatasIntoDropDownList(yearEdit, "SELECT pano_ano FROM pano where pano_ano >= "+ yr +" order by 1 desc");
                DatasToControls.EstablecerDefectoDropDownList(yearElim, yr);
                DatasToControls.EstablecerDefectoDropDownList(yearEdit, yr);
                bind.PutDatasIntoDropDownList(monthElim, "SELECT pmes_mes, pmes_nombre FROM pmes WHERE pmes_mes != 0 AND pmes_mes != 13 order by 1");
                bind.PutDatasIntoDropDownList(monthEdit, "SELECT pmes_mes, pmes_nombre FROM pmes WHERE pmes_mes != 0 AND pmes_mes != 13 order by 1");
                string mt = DBFunctions.SingleData("SELECT pmes_mes FROM ccontabilidad");
                DatasToControls.EstablecerDefectoDropDownList(monthEdit, DBFunctions.SingleData("SELECT pmes_nombre FROM pmes WHERE pmes_mes=" + mt + ""));
                DatasToControls.EstablecerDefectoDropDownList(monthElim, DBFunctions.SingleData("SELECT pmes_nombre FROM pmes WHERE pmes_mes=" + mt + ""));

                //bind.PutDatasIntoDropDownList(typeDoc2, "SELECT pdoc_codigo,pdoc_codigo concat ' - ' concat pdoc_nombre FROM pdocumento where tvig_vigencia is null or tvig_vigencia='V' order by pdoc_nombre");
                Utils.llenarPrefijos(Response, ref typeDoc2 , "%", "%", "%");
                //bind.PutDatasIntoDropDownList(typeDoc3, "SELECT pdoc_codigo,pdoc_codigo concat ' - ' concat pdoc_nombre FROM pdocumento where tvig_vigencia is null or tvig_vigencia='V' order by pdoc_nombre");
                Utils.llenarPrefijos(Response, ref typeDoc3 , "%", "%", "%");
				if(Request.QueryString["falloDoc"] == "1")
                {
                    Utils.MostrarAlerta(Response, "El proceso se realizó correctamente! \nPero se generó un error al guardar los documentos soporte.");
                }
                RecargarDatosEditar();
                RecargarDatosEliminar();
				Eliminar.Attributes.Add("onclick","return confirm('Esta seguro de realizar este proceso?\\nLos datos eliminados no se podran recuperar.');");
			}
		}

		private void RecargarDatosEliminar()
		{
			DatasToControls bind = new DatasToControls();
			string parametro = typeDoc3.SelectedValue;
			comprobanteInicio.Items.Clear();
			comprobanteFin.Items.Clear();
            bind.PutDatasIntoDropDownList(comprobanteInicio, "SELECT mcom_numedocu FROM mcomprobante WHERE pdoc_codigo='" + typeDoc3.SelectedValue + "' AND pano_ano=" + yearElim.SelectedValue + " AND pmes_mes=" + monthElim.SelectedValue + " ORDER BY mcom_numedocu ASC");
            bind.PutDatasIntoDropDownList(comprobanteFin, "SELECT mcom_numedocu FROM mcomprobante WHERE pdoc_codigo='" + typeDoc3.SelectedValue + "' AND pano_ano=" + yearElim.SelectedValue + " AND pmes_mes=" + monthElim.SelectedValue + " ORDER BY mcom_numedocu ASC");
		}
        private void RecargarDatosEditar()
        {
            DatasToControls bind = new DatasToControls();
            string parametro = typeDoc2.SelectedValue;
            comprobante.Items.Clear();
            bind.PutDatasIntoDropDownList(comprobante, "SELECT mcom_numedocu FROM mcomprobante WHERE pdoc_codigo='" + typeDoc2.SelectedValue + "' AND pano_ano=" + yearEdit.SelectedValue + " AND pmes_mes=" + monthEdit.SelectedValue + " ORDER BY mcom_numedocu ASC");
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

		protected void Eliminar_Click(object sender, System.EventArgs e)
		{
            if (!FechaValida(yearElim.SelectedValue, monthElim.SelectedValue))
            {
                if (!DBFunctions.RecordExist("SELECT TTIPE_CODIGO FROM SUSUARIO WHERE TTIPE_CODIGO = 'AS' AND SUSU_LOGIN = '" + HttpContext.Current.User.Identity.Name + "' "))
                {
                    Utils.MostrarAlerta(Response, "Fecha no válida.");
                    return;
                }
                else
                    Utils.MostrarAlerta(Response, "Sr(a) Contador(a), la Fecha del documnento es menor a la vigencia contable, su perfíl si permite este proceso");
             }

            string tipoComp = typeDoc3.SelectedValue;
			int inicio = Convert.ToInt32(comprobanteInicio.SelectedValue);
			int fin = Convert.ToInt32(comprobanteFin.SelectedValue);
			string errorDelete = String.Empty;

            for (int i=inicio;i<=fin;i++)
			{
				//Se revisa si existe o no el comprobante para eliminarlo
				if(DBFunctions.RecordExist("SELECT pdoc_codigo FROM mcomprobante WHERE pdoc_codigo='"+tipoComp+"' AND mcom_numedocu="+i))
				{
					Comprobante compDelete = new Comprobante();
					compDelete.Type   = tipoComp;
					compDelete.Number = i.ToString();
					compDelete.Year   = DBFunctions.SingleData("SELECT pano_ano FROM mcomprobante WHERE pdoc_codigo='"+tipoComp+"' AND mcom_numedocu="+i.ToString());
					compDelete.Month  = DBFunctions.SingleData("SELECT pmes_mes FROM mcomprobante WHERE pdoc_codigo='"+tipoComp+"' AND mcom_numedocu="+i.ToString());
					if(!compDelete.DeleteRecord(i.ToString()))
						errorDelete += "Se ha presentado un problema eliminando el comprobante "+tipoComp+"-"+i+"\\n";
				}
			}
			if(errorDelete != String.Empty)
                Utils.MostrarAlerta(Response, errorDelete);
			RecargarDatosEliminar();
		}

        protected void EliminarTodos_Click(object sender, System.EventArgs e)
        {
            if (!FechaValida(yearElim.SelectedValue, monthElim.SelectedValue))
            {
                Utils.MostrarAlerta(Response, "Fecha no válida.");
                return;
            }

            string tipoComp = "";// typeDoc3.SelectedValue;

            int inicio = 0;// Convert.ToInt32(comprobanteInicio.SelectedValue);
            int fin = 0;// Convert.ToInt32(comprobanteFin.SelectedValue);
            string errorDelete = String.Empty;

            for (int k = 0; k < typeDoc3.Items.Count; k++)
            {
                tipoComp = typeDoc3.Items[k].Value;

                //              if (tipoComp != ""+tComprobante.SelectedValue+"'" && tipoComp != "9031")
                {
                    try
                    {
                        inicio = Convert.ToInt16(DBFunctions.SingleData("SELECT mcom_numedocu FROM mcomprobante WHERE pdoc_codigo='" + tipoComp + "' AND pano_ano=" + yearElim.SelectedValue + " AND pmes_mes=" + monthElim.SelectedValue + " ORDER BY mcom_numedocu ASC"));
                        fin = Convert.ToInt16(DBFunctions.SingleData("SELECT mcom_numedocu FROM mcomprobante WHERE pdoc_codigo='" + tipoComp + "' AND pano_ano=" + yearElim.SelectedValue + " AND pmes_mes=" + monthElim.SelectedValue + " ORDER BY mcom_numedocu DESC;"));

                        for (int i = inicio; i <= fin; i++)
                        {
                            //return;
                            //Se revisa si existe o no el comprobante para eliminarlo
                            if(DBFunctions.RecordExist("SELECT pdoc_codigo FROM mcomprobante WHERE pdoc_codigo='" + tipoComp + "' AND mcom_numedocu=" + i))
                            {
                                Comprobante compDelete = new Comprobante();
                                compDelete.Type = tipoComp;
                                compDelete.Number = i.ToString();
                                compDelete.Year = DBFunctions.SingleData("SELECT pano_ano FROM mcomprobante WHERE pdoc_codigo='" + tipoComp + "' AND mcom_numedocu=" + i.ToString());
                                compDelete.Month = DBFunctions.SingleData("SELECT pmes_mes FROM mcomprobante WHERE pdoc_codigo='" + tipoComp + "' AND mcom_numedocu=" + i.ToString());
                                if (!compDelete.DeleteRecord(i.ToString()))
                                    errorDelete += "Se ha presentado un problema eliminando el comprobante " + tipoComp + "-" + i + "\\n";
                            }
                        }
                    }
                    catch (Exception err)
                    {}
                }
            }
            
            if (errorDelete != String.Empty)
                Utils.MostrarAlerta(Response, errorDelete);
            RecargarDatosEliminar();
        }

        protected void ChangeParametersElim(object sender, EventArgs e)
        {
            RecargarDatosEliminar();
        }

        protected void ChangeParametersEdit(object sender, EventArgs e)
        {
            RecargarDatosEditar();
        }

        private bool FechaValida(string ano, string mes)
        {
            DateTime fechaUso = new DateTime(Convert.ToInt32(ano), Convert.ToInt32(mes), 2);
            DateTime fechaMin = new DateTime(Convert.ToInt32(DBFunctions.SingleData("SELECT PANO_ANO FROM CCONTABILIDAD;")), Convert.ToInt32(DBFunctions.SingleData("SELECT PMES_MES FROM CCONTABILIDAD;")), 1);
            return (fechaUso > fechaMin);
        }
        protected void EdiComp(object sender, EventArgs e)
        {
            string date = "", typeNum = "", detail = "", num = "", usuario = "";
            DataSet dsTmp = new DataSet();
            if (!FechaValida(yearEdit.SelectedValue, monthEdit.SelectedValue))
            {
                if (!DBFunctions.RecordExist("SELECT TTIPE_CODIGO FROM SUSUARIO WHERE TTIPE_CODIGO = 'AS' AND SUSU_LOGIN = '" + HttpContext.Current.User.Identity.Name + "' "))
                {
                    Utils.MostrarAlerta(Response, "Fecha no válida, es menor a la vigencia contable");
                    return;
                }
                else
                    Utils.MostrarAlerta(Response, "Sr(a) Contador(a), la Fecha del documnento es menor a la vigencia contable, su perfíl si permite este proceso");
            }
            InitDataTable();
            Session["dtInserts"] = dtInserts;
        
            try
            {
                String query = "select mcom_fecha, pdoc_codigo, mcom_razon, mcom_numedocu, pano_ano, pmes_mes, mcom_usuario from mcomprobante where mcom_numedocu=" + comprobante.SelectedItem.Value + " and pdoc_codigo = '" + typeDoc2.SelectedValue + "' order by pdoc_codigo, mcom_numedocu";
                DBFunctions.Request(dsTmp, IncludeSchema.NO, query);
                string y = dsTmp.Tables[0].Rows[0].ItemArray[4].ToString();
                string m = dsTmp.Tables[0].Rows[0].ItemArray[5].ToString();
                date     = dsTmp.Tables[0].Rows[0].ItemArray[0].ToString();
                typeNum  = dsTmp.Tables[0].Rows[0].ItemArray[1].ToString();
                detail   = dsTmp.Tables[0].Rows[0].ItemArray[2].ToString();
                if(dsTmp.Tables[0].Rows[0].ItemArray[6].ToString() != "")
                {
                    usuario = dsTmp.Tables[0].Rows[0].ItemArray[6].ToString();
                }
                else
                {
                    usuario = "";
                }
                
                num = dsTmp.Tables[0].Rows[0].ItemArray[3].ToString();

                Session["ano"] = y;
                Session["mes"] = m;
                string referencia = typeDoc2.SelectedItem.Text;
                Response.Redirect("AMS.Web.Index.aspx?process=Contabilidad.CompGrid&cod=" + Request.QueryString["cod"] + "&detail=&idComp=" + comprobante.SelectedItem.Value + "&typeNum=" + typeDoc2.SelectedValue + "&consecutivo=true&year=" + y + "&month=" + m + "&date=" + date + "&action=edit" + "&ref=" + referencia + "" + "&name=" + usuario + "");
            }
            catch
            {
                Utils.MostrarAlerta(Response, "No existen comprobantes para este periodo");
            }
        }

        protected void PopulateDataTable()
        {
            DataSet dsTmp = new DataSet();
            DBFunctions.Request(dsTmp, IncludeSchema.NO, "select mcue_codipuc, mnit_nit, dcue_codirefe, dcue_numerefe, dcue_detalle, palm_almacen, pcen_codigo, dcue_valodebi, dcue_valocred, dcue_valobase, dcue_NIIFdebi, dcue_NIIFcred from dcuenta where mcom_numedocu=" + comprobante.SelectedItem.Value + "");
            for (int x = 0; x < dsTmp.Tables[0].Rows.Count - 1; x++)
            {
                dr = dtInserts.NewRow();

                for (int j = 0; j < 7; j++)
                    dr[j] = dsTmp.Tables[0].Rows[x].ItemArray[j].ToString();

                dr[7] = dsTmp.Tables[0].Rows[x].ItemArray[7].ToString();
                dr[8] = dsTmp.Tables[0].Rows[x].ItemArray[8].ToString();
                dr[9] = dsTmp.Tables[0].Rows[x].ItemArray[9].ToString();
                dr[10] = dsTmp.Tables[0].Rows[x].ItemArray[10].ToString();
                dr[11] = dsTmp.Tables[0].Rows[x].ItemArray[11].ToString();

                dtInserts.Rows.Add(dr);
            }
        }

        protected void InitDataTable()
        {
            lbFields.Clear();
            lbFields.Add("cuenta");//0
            lbFields.Add("nit");//1
            lbFields.Add("pref");//2
            lbFields.Add("docref");//3
            lbFields.Add("detalle");//4
            lbFields.Add("sede");//5
            lbFields.Add("ccosto");//6
            lbFields.Add("debito");//7
            lbFields.Add("credito");//8
            lbFields.Add("base");//9
            lbFields.Add("debitoNiif");//7
            lbFields.Add("creditoNiif");//8

            types.Clear();
            types.Add(typeof(string));
            types.Add(typeof(string));
            types.Add(typeof(string));
            types.Add(typeof(string));
            types.Add(typeof(string));
            types.Add(typeof(string));
            types.Add(typeof(string));
            types.Add(typeof(double));
            types.Add(typeof(double));
            types.Add(typeof(double));
            types.Add(typeof(double));
            types.Add(typeof(double));

            dtInserts = new DataTable();
            for (i = 0; i < lbFields.Count; i++)
                dtInserts.Columns.Add(new DataColumn((string)lbFields[i], (Type)types[i]));
            //infoLabel.Text +="<br>"+(string)lbFields[i];
        }
	}
}
