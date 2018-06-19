// created on 31/10/2003 at 11:01
using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using Ajax;
using AMS.Tools;
using System.Text.RegularExpressions;

namespace AMS.Contabilidad
{
    public partial class CompGrid : System.Web.UI.UserControl
    {
        #region Atributos
        protected TextBox tbBase;
        protected TextBox plantV;
        protected int limSup = 20, limInf = 2;
        protected bool insertions = true;
        protected DataTable dtInserts;
        protected DataView dvInserts;
        protected DataRow dr;
        protected DataColumn dc;
        protected ArrayList types = new ArrayList();
        protected ArrayList lbFields = new ArrayList();
        protected int i = 0;
        protected double porcentajeTolerancia = 10;
        protected bool errorgrilla = false;
        protected DataSet dsComprobante = new DataSet();
        protected bool consecutivoAutomatico = false;
        protected ArrayList archivos = new ArrayList();

        #endregion

        #region Eventos

        protected void Page_Load(object sender, System.EventArgs e)
		{
            if (Request.QueryString["consecutivo"] == "true")
            {
                idComp.Attributes.Add("readonly", "true");
                consecutivoAutomatico = true;
            }
			lbRef.Text=Request.QueryString["typeNum"]+" "+Request.QueryString["ref"];
			LoadDataColumns();
            if (Session["dtInserts"] == null)
            {
                LoadDataTable();
                if (dtInserts.Rows.Count > 0)
                {
                    Button3.Enabled = true;
                }
                else
                {
                    Button3.Enabled = false;
                }
            }
            else
            {
                dtInserts = (DataTable)Session["dtInserts"];
                Button3.Enabled = true;
            }
            if (!IsPostBack)
			{
                lbDate.Text = Request.QueryString["date"];
                int y = (Convert.ToDateTime(lbDate.Text)).Year;
                int compPost = Convert.ToInt32(DBFunctions.SingleData("SELECT COALESCE(COUNT (*),0) FROM MCOMPROBANTE WHERE PANO_ANO > " + y + ""));
                if (compPost > 0)
                {
                    int y1 = y + 1;
                    Utils.MostrarAlerta(Response, "Ya existen " + compPost + " documentos contabilizados para el año " + y1 + ", DEBE REPETIR el CIERRE ANUAL para trasladar los saldos cuando usted INSERTE, MODIFIQUE o ELIMINE documentos en el año " + y);
                }
                if (Request.QueryString["action"]=="new")
				{
                    lbDate.Text = Request.QueryString["date"];	
					if(Request.QueryString["idComp"] == null)
					{
						ConsecutivoComprobante();
					}
					else
					{
						idComp.Text = Request.QueryString["idComp"];
						tbDetail.Text = Request.QueryString["detail"];
						Diference();
					}
					if(Request.QueryString["plant"] == "True")
						CargarRenglonesPlantilla(Request["tPlant"],Convert.ToInt32(Request["cPlant"]),Request["typeNum"],Convert.ToInt32(idComp.Text));
                    BindDatas();
				}
				else if(Request.QueryString["action"]=="edit")
				{
                    //Consulta de movimientos dentro de un comprobante especifico//
                    //lbDate.Text = Request.QueryString["date"];

                    lbDate.Text = Convert.ToDateTime(DBFunctions.SingleData("SELECT mcom_fecha FROM mcomprobante where pdoc_codigo='"+ Request["typeNum"] +"' AND mcom_numedocu=" + Request.QueryString["idcomp"] +"")).ToString("yyyy-MM-dd");
                    dtInserts.Clear();
					DataSet dsTmp = new DataSet();
                    DBFunctions.Request(dsTmp, IncludeSchema.NO, "select mcue_codipuc, mnit_nit, dcue_codirefe, dcue_numerefe, dcue_detalle, palm_almacen, pcen_codigo, dcue_valodebi, dcue_valocred, dcue_valobase, dcue_NIIFdebi, dcue_NIIFcred from dcuenta where pdoc_codigo='" + Request["typeNum"] + "' AND mcom_numedocu=" + Request.QueryString["idcomp"] + "");
					//Vamos a llenar el DataTable
					for (int x=0; x < dsTmp.Tables[0].Rows.Count; x++)
					{
						dr = dtInserts.NewRow();

						for(int j=0; j<7; j++)
							dr[j] = dsTmp.Tables[0].Rows[x].ItemArray[j].ToString();

						dr[7] = dsTmp.Tables[0].Rows[x].ItemArray[7].ToString();
						dr[8] = dsTmp.Tables[0].Rows[x].ItemArray[8].ToString();
						dr[9] = dsTmp.Tables[0].Rows[x].ItemArray[9].ToString();
                        dr[10] = dsTmp.Tables[0].Rows[x].ItemArray[10].ToString();
                        dr[11] = dsTmp.Tables[0].Rows[x].ItemArray[11].ToString();

						dtInserts.Rows.Add(dr);
					}
					//////////////////////////////
					idComp.Text = Request.QueryString["idComp"];
					tbDetail.Text = DBFunctions.SingleData("SELECT mcom_razon FROM mcomprobante where pdoc_codigo='"+ Request["typeNum"] +"' AND mcom_numedocu=" + Request.QueryString["idcomp"] +"");
					
					BindDatas();
					Diference();
				}

				else if(Request.QueryString["action"]=="inflac")
				{

				}
			}
            if (Session["archivos"] != null)
                archivos = (ArrayList)Session["archivos"];
            
        }

		protected void DgInserts_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			if(e.Item.ItemType == ListItemType.Footer)	{
				((TextBox)e.Item.Cells[7].FindControl("valToInsert8")).Attributes.Add("onblur",   "ManejoTbsCredDeb(this," + ((TextBox)e.Item.Cells[8].FindControl("valToInsert9")).ClientID+");");
				((TextBox)e.Item.Cells[8].FindControl("valToInsert9")).Attributes.Add("onblur",   "ManejoTbsCredDeb(this," + ((TextBox)e.Item.Cells[7].FindControl("valToInsert8")).ClientID+");");
                ((TextBox)e.Item.Cells[10].FindControl("valToInsert11")).Attributes.Add("onblur", "ManejoTbsCredDeb(this," + ((TextBox)e.Item.Cells[11].FindControl("valToInsert12")).ClientID + ");");
                ((TextBox)e.Item.Cells[11].FindControl("valToInsert12")).Attributes.Add("onblur", "ManejoTbsCredDeb(this," + ((TextBox)e.Item.Cells[10].FindControl("valToInsert11")).ClientID + ");");
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[5].Controls[1]), "SELECT palm_almacen, palm_descripcion FROM palmacen where TVIG_VIGENCIA = 'V'");
				bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[6].Controls[1]),"SELECT pcen_codigo, pcen_nombre FROM pcentrocosto where timp_codigo <> 'N' ");
				if(dtInserts.Rows.Count > 14)
					for(i=0; i<e.Item.Cells.Count-2; i++)
					{
						Label lb = new Label();
						lb.Text = (string)lbFields[i];
						e.Item.Cells[i].Controls.Add(lb);
					}
				if(dtInserts.Rows.Count > 0)
				{
					((TextBox)e.Item.Cells[1].Controls[1]).Text = dtInserts.Rows[dtInserts.Rows.Count-1][1].ToString();
					((TextBox)e.Item.Cells[2].Controls[1]).Text = dtInserts.Rows[dtInserts.Rows.Count-1][2].ToString();
					((TextBox)e.Item.Cells[3].Controls[1]).Text = dtInserts.Rows[dtInserts.Rows.Count-1][3].ToString();
					((TextBox)e.Item.Cells[4].Controls[1]).Text = dtInserts.Rows[dtInserts.Rows.Count-1][4].ToString();
					((DropDownList)e.Item.Cells[5].Controls[1]).SelectedIndex=((DropDownList)e.Item.Cells[5].Controls[1]).Items.IndexOf(((DropDownList)e.Item.Cells[5].Controls[1]).Items.FindByValue(dtInserts.Rows[dtInserts.Rows.Count-1][5].ToString()));
					((DropDownList)e.Item.Cells[6].Controls[1]).SelectedIndex=((DropDownList)e.Item.Cells[6].Controls[1]).Items.IndexOf(((DropDownList)e.Item.Cells[6].Controls[1]).Items.FindByValue(dtInserts.Rows[dtInserts.Rows.Count-1][6].ToString()));
				}
			}	 		
			if(e.Item.ItemType == ListItemType.EditItem)
			{
				((Button)e.Item.Cells[13].Controls[0]).Attributes.Add("onmouseover","focus(); checkBase();");
				bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[5].FindControl("ddlSedeEdicion")), "SELECT palm_almacen, palm_descripcion FROM palmacen WHERE TVIG_VIGENCIA = 'V'");
				try{((DropDownList)e.Item.Cells[5].FindControl("ddlSedeEdicion")).SelectedValue = dtInserts.Rows[e.Item.ItemIndex][5].ToString();}catch{}
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[6].FindControl("ddlCCEdicion")), "SELECT pcen_codigo, pcen_nombre FROM pcentrocosto where timp_codigo <> 'N' ");
				try{((DropDownList)e.Item.Cells[6].FindControl("ddlCCEdicion")).SelectedValue = dtInserts.Rows[e.Item.ItemIndex][6].ToString();}catch{}
			}
		}

		protected void DgInserts_AddAndDel(object sender, DataGridCommandEventArgs e)
		{
			if(((Button)e.CommandSource).CommandName == "AddDatasRow" && CheckValues(e))
			{
                bool errorImp = false;
                string mensajeImp = "";
                string tipoCuenta = DBFunctions.SingleData(
                      @"SELECT TCUE_CODIGO FROM mcuenta WHERE mcue_codipuc ='" + ((TextBox)e.Item.Cells[0].Controls[1]).Text + @"' 
                        AND timp_codigo IN ('A','P') 
                        and '" + ((TextBox)e.Item.Cells[0].Controls[1]).Text + @"' not in (select mcue_codipuc     from ccontabilidad)  
                        and '" + ((TextBox)e.Item.Cells[0].Controls[1]).Text + @"' not in (select mcue_codipucnomi from ccontabilidad)" );
                if ((((TextBox)e.Item.Cells[7].FindControl("valToInsert8")).Text) == "" || (((TextBox)e.Item.Cells[7].FindControl("valToInsert8")).Text) == "0")
                    (((TextBox)e.Item.Cells[7].FindControl("valToInsert8")).Text) = "0.00";
                if ((((TextBox)e.Item.Cells[8].FindControl("valToInsert9")).Text) == "" || (((TextBox)e.Item.Cells[8].FindControl("valToInsert9")).Text) == "0")
                    (((TextBox)e.Item.Cells[8].FindControl("valToInsert9")).Text) = "0.00";
                if ((((TextBox)e.Item.Cells[9].FindControl("valToInsert10")).Text) == "" || (((TextBox)e.Item.Cells[9].FindControl("valToInsert10")).Text) == "0")
                    (((TextBox)e.Item.Cells[9].FindControl("valToInsert10")).Text) = "0.00";
                if ((((TextBox)e.Item.Cells[10].FindControl("valToInsert11")).Text) == "" || (((TextBox)e.Item.Cells[10].FindControl("valToInsert11")).Text) == "0")
                    (((TextBox)e.Item.Cells[10].FindControl("valToInsert11")).Text) = "0.00";
                if ((((TextBox)e.Item.Cells[11].FindControl("valToInsert12")).Text) == "" || (((TextBox)e.Item.Cells[11].FindControl("valToInsert12")).Text) == "0")
                    (((TextBox)e.Item.Cells[11].FindControl("valToInsert12")).Text) = "0.00";
                if (tipoCuenta == "" || tipoCuenta == null)
                {
                    mensajeImp += "No existe la cuenta especificada o su tipo de imputación NO es válido.\\n";
                    errorImp = true;
				}
                else
				{
                    if (DBFunctions.SingleData("SELECT tbas_codigo FROM mcuenta WHERE mcue_codipuc ='" + ((TextBox)e.Item.Cells[0].Controls[1]).Text + "' AND timp_codigo IN ('A','P')") == "B")
					{
						double valorBaseConsultaIngreso = 0, valorIngresoDC = 0, valorBaseCuentaPorcentaje = 0, valorBaseCalculado = 0, valorDefinitivo = 0;
                        try { valorIngresoDC = Convert.ToDouble(((TextBox)e.Item.Cells[7].FindControl("valToInsert8")).Text); }
                        catch { valorIngresoDC = Convert.ToDouble(((TextBox)e.Item.Cells[8].FindControl("valToInsert9")).Text); }
						try{valorBaseCuentaPorcentaje = Convert.ToDouble(DBFunctions.SingleData("SELECT mcue_basegrav FROM mcuenta WHERE mcue_codipuc ='" + ((TextBox)e.Item.Cells[0].Controls[1]).Text + "' AND timp_codigo IN ('A','P')"));}catch{}
						try{valorBaseConsultaIngreso = Convert.ToDouble(((TextBox)e.Item.Cells[9].Controls[1]).Text);}catch{}
						try{valorBaseCalculado = (valorIngresoDC*100)/valorBaseCuentaPorcentaje;}catch{}
						if(valorIngresoDC < valorBaseCalculado-(valorBaseCalculado*(porcentajeTolerancia/100)) || valorIngresoDC > valorBaseCalculado+(valorBaseCalculado*(porcentajeTolerancia/100)))
							valorDefinitivo = valorBaseCalculado;
						else
							valorDefinitivo = valorIngresoDC;
						if(DBFunctions.SingleData("SELECT cemp_liqudeci FROM cempresa") == "S")
							((TextBox)e.Item.Cells[9].Controls[1]).Text = valorDefinitivo.ToString("n");
						else if(DBFunctions.SingleData("SELECT cemp_liqudeci FROM cempresa") == "N")
							    ((TextBox)e.Item.Cells[9].Controls[1]).Text = Math.Round(valorDefinitivo).ToString("n");
					}
				}
                if (tipoCuenta == "N" && (((TextBox)e.Item.Cells[7].FindControl("valToInsert8")).Text != "0.00" || ((TextBox)e.Item.Cells[8].FindControl("valToInsert9")).Text != "0.00"))
                {
                    mensajeImp += "La cuenta es tipo SOLO NIIF por lo cual NO puede tener partidas en los débitos o créditos FISCALES.\\n";
                    errorImp = true;
                }
                if (tipoCuenta == "F" && (((TextBox)e.Item.Cells[10].FindControl("valToInsert11")).Text != "0.00" || ((TextBox)e.Item.Cells[11].FindControl("valToInsert12")).Text != "0.00"))
                {
                    mensajeImp += "La cuenta es tipo SOLO FISCAL por lo cual NO puede tener partidas en los débitos o créditos NIIFs.\\n";
                    errorImp = true;
                }
                if (((TextBox)e.Item.Cells[1].Controls[1]).Text == "" || DBFunctions.SingleData("SELECT mnit_nit FROM mnit WHERE mnit_nit ='" + ((TextBox)e.Item.Cells[1].Controls[1]).Text + "'")!=((TextBox)e.Item.Cells[1].Controls[1]).Text )
				{
                    mensajeImp += "No existe el nit especificado.\\n";
                    errorImp = true;
                }
                /*
                if (((TextBox)e.Item.Cells[5].Controls[1]).Text == "" || DBFunctions.SingleData("SELECT PALM_ALMACEN FROM PALMACEN WHERE PALM_ALMACEN ='" + ((TextBox)e.Item.Cells[5].Controls[1]).Text + "'") != ((TextBox)e.Item.Cells[5].Controls[1]).Text )
                {
                    mensajeImp += "No existe la SEDE especificada.\\n";
                    errorImp = true;
                }
                if (((TextBox)e.Item.Cells[6].Controls[1]).Text == "" || DBFunctions.SingleData("SELECT pcen_codigo FROM pcentrocosto WHERE timp_codigo <> 'N' and pcen_codigo ='" + ((TextBox)e.Item.Cells[6].Controls[1]).Text + "'") != ((TextBox)e.Item.Cells[6].Controls[1]).Text )
                {
                    mensajeImp += "No existe el CENTRO DE COSTOS especificado o está definido como NO imputable.\\n";
                    errorImp = true;
                }
                */
                if (((TextBox)e.Item.Cells[4].Controls[1]).Text.Length==0)
				{
                    mensajeImp += "Debe epecificar el detalle.\\n";
                    errorImp = true;
                }
                if (errorImp)
                {
                    Utils.MostrarAlerta(Response, mensajeImp);
                    return;
                }

                dr = dtInserts.NewRow();
				TextBox tb;
				for(i=0; i<lbFields.Count; i++)
				{
					if(i==5||i==6)
					{
						dr[i] = ((DropDownList)e.Item.Cells[i].Controls[1]).SelectedValue;
					}
					else
					{
						tb = (TextBox)e.Item.Cells[i].Controls[1];
						if(tb.Text == "")
							tb.Text = "0";
						dr[i] = tb.Text;
					}
				}
				dtInserts.Rows.Add(dr);
				Button1.Enabled = false;
			}
			if(((Button)e.CommandSource).CommandName == "DelDatasRow")
			{
				try
				{
					dtInserts.Rows.Remove(dtInserts.Rows[e.Item.ItemIndex]);
				}
				catch
				{
					dtInserts.Rows.Clear();
				}
			}
			Diference();
			BindDatas();
		}

		protected void RecordComp(Object Sender, EventArgs E)
		{
            //Ahora si el usuario crea una nota contable, su nombre se quedará registrado
            //string usuario = HttpContext.Current.User.Identity.Name.ToLower();
            if (tbDetail.Text.Length == 0)
            {
                Utils.MostrarAlerta(Response, "Por favor escriba la razón del comprobante.");
                return;
            }

			string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
            if (!consecutivoAutomatico)  // Consecutivo Manual se valida que no exita
            {
                if (Request.QueryString["action"] == "new" && DBFunctions.SingleData("SELECT pdoc_codigo FROM mcomprobante WHERE pdoc_codigo ='" + Request.QueryString["typeNum"] + "' AND MCOM_NUMEDOCU=" + idComp.Text) == Request.QueryString["typeNum"])
                {   //Revisar que no exista el comprobante
                     Utils.MostrarAlerta(Response, "Ya existe un comprobante con ese numero, por favor cambielo.");
                     return;
                 }
            }
            if (consecutivoAutomatico)  // Consecutivo Automatico se busca hasta que no exista 
            {
                bool salir = false;
                do 
                   if (Request.QueryString["action"] == "new" && DBFunctions.SingleData("SELECT pdoc_codigo FROM mcomprobante WHERE pdoc_codigo ='" + Request.QueryString["typeNum"] + "' AND MCOM_NUMEDOCU=" + idComp.Text) == Request.QueryString["typeNum"])
                       idComp.Text = (Convert.ToInt32(idComp.Text.ToString()) + 1).ToString();
                   else salir = true;
                while(!salir);
            }

            if (tbDiferencia.Text == "0" && tbDiferenciaNiif.Text == "0")
			{
				//lbInfo.Text += "dtInserts num rows: " + dtInserts.Rows.Count.ToString() + "<br>";
				//dtInserts.Rows.Remove(dtInserts.Rows[dtInserts.Rows.Count-1]);
				Comprobante comp = new Comprobante(dtInserts);
				// 2004-06-15
				// Verifico si el comprobante no existe
				/*while (VerificoComprobante(Request.QueryString["typeNum"],Convert.ToInt32(idComp.Text)))
					{
						lbInfo.Text += "Comprobante " + Request.QueryString["typeNum"] + " - " + idComp.Text  + "Existe! <br> Se tomara un Consecutivo Automatico!<br>";
						if ((Convert.ToInt32(idComp.Text)-1).ToString() == DBFunctions.SingleData("SELECT pdoc_ultidocu FROM pdocumento WHERE pdoc_codigo ='" + Request.QueryString["typeNum"] + "'").ToString())
							ActualizaConsecutivo(Request.QueryString["typeNum"]);
						//Obtengo el nuevo consecutivo
						ConsecutivoComprobante();
					}*/
				//Actualizo el consecutivo en 1

				//if ((Convert.ToInt32(idComp.Text)-1).ToString() == DBFunctions.SingleData("SELECT pdoc_ultidocu FROM pdocumento WHERE pdoc_codigo ='" + Request.QueryString["typeNum"] + "'").ToString() )
				//	ActualizaConsecutivo(Request.QueryString["typeNum"]);

				comp.Type = Request.QueryString["typeNum"];
				comp.Number = idComp.Text;
				comp.Year = Request.QueryString["year"];
				
				comp.Month = Request.QueryString["month"];
				
			
				comp.Date = Convert.ToDateTime(lbDate.Text).ToString("yyyy-MM-dd");
				comp.Detail = tbDetail.Text;
                if (Request.QueryString["name"] != "" )
                {
                    if (Request.QueryString["name"] == HttpContext.Current.User.Identity.Name.ToLower())
                    {
                        comp.User = Request.QueryString["name"];
                    }
                    else
                    {
                        comp.User = Request.QueryString["name"].Replace( HttpContext.Current.User.Identity.Name.ToLower() , "") + ", " + HttpContext.Current.User.Identity.Name.ToLower();
                        comp.User = comp.User.Replace(", ,", ",");
                    }
                }
                else
                {
                    comp.User = HttpContext.Current.User.Identity.Name.ToLower();
                }
				comp.Value = tbDebito.Text;
				comp.Consecutivo = Convert.ToBoolean(Request.QueryString["consecutivo"]);
  

				if(Request.QueryString["action"]=="new")
				{
                    ArrayList sqlStrings = new ArrayList();
                    if (comp.CommitValues(ref sqlStrings))
					{
                        if (DBFunctions.Transaction(sqlStrings))
                        {
                            if(archivos.Count > 0)
                            {
                                if (!insertarDocumentos(comp.Type, comp.Number))
                                {
                                    dtInserts.Clear();
                                    Session.Clear();
                                    Response.Redirect("" + indexPage + "?process=Contabilidad.CapEdiCom&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&pref=" + Request.QueryString["typeNum"] + "&num=" + idComp.Text + "&falloDoc=1");
                                }
                            }
                            dtInserts.Clear();
                            Session.Clear();
                            Response.Redirect("" + indexPage + "?process=Contabilidad.CapEdiCom&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&pref=" + Request.QueryString["typeNum"] + "&num=" + idComp.Text);
                        }
                        else
                        {
                            lbInfo.Text += "Error al grabar.";
                        }
					}
					else
						lbInfo.Text += comp.ProcessMsg;
				}
				else if(Request.QueryString["action"]=="edit")
				{
					if(comp.UpdateRecord(idComp.Text))
					{
                        if (archivos.Count > 0)
                        {
                            if (!insertarDocumentos(comp.Type, comp.Number))
                            {
                                dtInserts.Clear();
                                Session.Clear();
                                Response.Redirect("" + indexPage + "?process=Contabilidad.ElimCom&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&falloDoc=1");
                            }
                            /*dtInserts.Clear();
                            Session.Clear();
                            Response.Redirect("" + indexPage + "?process=Contabilidad.ElimCom&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "");*/
                        }
                        dtInserts.Clear();
						Session.Clear();
						lbInfo.Text += comp.ProcessMsg;
                        Response.Redirect("" + indexPage + "?process=Contabilidad.ElimCom&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "");
					}
					else
						lbInfo.Text += comp.ProcessMsg;
					/*if(comp.DeleteRecord(idComp.Text))
						{
							if(comp.CommitValues())
							{
								dtInserts.Clear();
								Session.Clear();
								Response.Redirect("" + indexPage + "?process=Contabilidad.CapEdiCom&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "");
							}
							else
							{
								lbInfo.Text += comp.ProcessMsg;
							}
						}
						else
							lbInfo.Text+=comp.ProcessMsg;*/
				}
				//Response.Write("<script language='javascript'>alert('"+comp.Basura+"');</script>");
			}
			else
                Utils.MostrarAlerta(Response, "No se puede grabar el comprobante debido a que las sumas son desiguales.");
			
		}

		protected void CancelComp(Object Sender, EventArgs E)
		{
			dtInserts.Clear();
			Session.Clear();
			string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
            Response.Redirect("" + indexPage + "?process=Contabilidad.ElimCom&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "");
			this.ClearChildViewState();
		}

		public void DgInserts_Edit(Object sender, DataGridCommandEventArgs e)
		{
			dgInserts.EditItemIndex = (int)e.Item.ItemIndex;
			BindDatas();
		}

		public void DgInserts_Update(Object sender, DataGridCommandEventArgs e)
		{
			if(CheckValuesUpdate(e))
			{
				if(DBFunctions.SingleData("SELECT mcue_codipuc FROM mcuenta WHERE mcue_codipuc ='" + ((TextBox)e.Item.Cells[0].Controls[1]).Text + "' AND timp_codigo IN ('A','P')")!=((TextBox)e.Item.Cells[0].Controls[1]).Text)
				{
                    Utils.MostrarAlerta(Response, "No existe la cuenta especificada o su tipo de imputacion no es valido.");
					return;
				}
				else
				{
					if(DBFunctions.SingleData("SELECT tbas_codigo FROM mcuenta WHERE mcue_codipuc ='" + ((TextBox)e.Item.Cells[0].FindControl("edit_1")).Text + "' AND timp_codigo IN ('A','P')") == "B")
					{
						double valorBaseConsultaIngreso = 0, valorIngresoDC = 0, valorBaseCuentaPorcentaje = 0, valorBaseCalculado = 0, valorDefinitivo = 0;
						try{valorIngresoDC = Convert.ToDouble(((TextBox)e.Item.Cells[7].FindControl("edit_8")).Text);}
						catch{valorIngresoDC = 0;}
						if(valorIngresoDC == 0)
						{
							try{valorIngresoDC = Convert.ToDouble(((TextBox)e.Item.Cells[8].FindControl("edit_9")).Text);}
							catch{valorIngresoDC = 0;}
						}
						try{valorBaseCuentaPorcentaje = Convert.ToDouble(DBFunctions.SingleData("SELECT mcue_basegrav FROM mcuenta WHERE mcue_codipuc ='" + ((TextBox)e.Item.Cells[0].FindControl("edit_1")).Text + "' AND timp_codigo IN ('A','P')"));}
						catch{}
						try{valorBaseConsultaIngreso = Convert.ToDouble(((TextBox)e.Item.Cells[9].FindControl("edit_10")).Text);}
						catch{}
						try{valorBaseCalculado = (valorIngresoDC*100)/valorBaseCuentaPorcentaje;}
						catch{}
						if(valorIngresoDC < valorBaseCalculado-(valorBaseCalculado*(porcentajeTolerancia/100)) || valorIngresoDC > valorBaseCalculado+(valorBaseCalculado*(porcentajeTolerancia/100)))
							valorDefinitivo = valorBaseCalculado;
						else
							valorDefinitivo = valorIngresoDC;
						if(DBFunctions.SingleData("SELECT cemp_liqudeci FROM cempresa") == "S")
							((TextBox)e.Item.Cells[9].Controls[1]).Text = valorDefinitivo.ToString("n");
						else 
                     //       if(DBFunctions.SingleData("SELECT cemp_liqudeci FROM cempresa") == "N")  //NO tiene logica esta pregunta porque es el false de la pregunta anterior
							((TextBox)e.Item.Cells[9].Controls[1]).Text = Math.Round(valorDefinitivo).ToString("n");
					}
				}
				if(DBFunctions.SingleData("SELECT mnit_nit FROM mnit WHERE mnit_nit ='" + ((TextBox)e.Item.Cells[1].Controls[1]).Text + "'")!=((TextBox)e.Item.Cells[1].Controls[1]).Text)
				{
                    Utils.MostrarAlerta(Response, "No existe el nit especificado.");
					return;
				}
				if(((TextBox)e.Item.Cells[4].Controls[1]).Text.Length==0)
				{
                    Utils.MostrarAlerta(Response, "Debe epecificar el detalle.");
					return;
				}
                for (i = 7; i < 12; i++)
                {
                    if (((TextBox)e.Item.Cells[i].Controls[1]).Text.Length == 0)
                    {
                        Utils.MostrarAlerta(Response, "Debe especificar el Valor, se convierte a CERO.");
                        return;
                    }
                }
                    
				for(i=0; i<lbFields.Count; i++)
				{
					if(i != 5 && i != 6)
						dtInserts.Rows[dgInserts.EditItemIndex][i] = ((TextBox)e.Item.FindControl("edit_" + (i+1).ToString())).Text;
					else if (i==5)
						dtInserts.Rows[dgInserts.EditItemIndex][i] = ((DropDownList)e.Item.FindControl("ddlSedeEdicion")).SelectedValue;
					else if (i==6)
						dtInserts.Rows[dgInserts.EditItemIndex][i] = ((DropDownList)e.Item.FindControl("ddlCCEdicion")).SelectedValue;
				}
                Diference();
				dgInserts.EditItemIndex = -1;
                Session["dtInserts"] = dtInserts;
                BindDatas();
                
            }
		}

		public void DgInserts_Cancel(Object sender, DataGridCommandEventArgs e)
		{
			dgInserts.EditItemIndex = -1;
			BindDatas();
		}

		protected void btnCargar_Click(object sender, System.EventArgs e)
		{
            string msgError = "";
            if (flArchivoExcel.PostedFile.FileName.ToString() == string.Empty)
                msgError = "No ha seleccionado un archivo EXCEL Válido'";
			else
			{
                try
				{
					string[] file = flArchivoExcel.PostedFile.FileName.ToString().Split('\\');
                    string fileName = file[file.Length - 1];
                    string[] fileNameParts = fileName.Split('.');
                    string fileExtension = fileNameParts[fileNameParts.Length - 1];

                    if (fileExtension.ToUpper() != "XLS" && fileExtension.ToUpper() != "XLSX")
                        Utils.MostrarAlerta(Response, "No es un archivo de Excel");
					else
					{
                        flArchivoExcel.PostedFile.SaveAs(ConfigurationManager.AppSettings["PathToImportsExcel"] + fileName);
                        ExcelFunctions exc = new ExcelFunctions(ConfigurationManager.AppSettings["PathToImportsExcel"] + fileName);
                        bool leiArchivo = false;
                        
                        try
                        {
                            exc.Request(dsComprobante, IncludeSchema.NO, "SELECT * FROM COMPROBANTE");
                            leiArchivo = true;
                        }
                        catch
                        {
                            Utils.MostrarAlerta(Response, "No se pudo leer ningún registro en el archivo de Excel");
                        }
                        if (leiArchivo)
                        {
                            if (dsComprobante.Tables.Count == 0)
                                Utils.MostrarAlerta(Response, "No se encontro ningún registro en el archivo de Excel");
                            else
                            {
                                if(dsComprobante.Tables[0].Rows.Count == 0)
                                {
                                    Utils.MostrarAlerta(Response, "No se encontro ningún registro en el archivo de Excel");
                                    return;
                                }
                                for (int i = 0; i < dsComprobante.Tables[0].Columns.Count; i++)
                                {
                                    dsComprobante.Tables[0].Columns[i].ColumnName = dsComprobante.Tables[0].Rows[0][i].ToString();
                                }
                                dsComprobante.Tables[0].Rows[0].Delete();
                                dsComprobante.Tables[0].Rows[0].AcceptChanges();
                                for (int i = 0; i < dsComprobante.Tables[0].Rows.Count; i++)
                                {
                                    DataRow dr = dtInserts.NewRow();
                                    try
                                    {
                                        dr["cuenta"] = dsComprobante.Tables[0].Rows[i]["CUENTA"].ToString();
                                    }
                                    catch
                                    {
                                        dr["cuenta"] = " ERROR ";
                                        msgError += "renglón " + i.ToString() + " Error en la Cuenta \n";
                                    }

                                    try
                                    {
                                        dr["nit"] = dsComprobante.Tables[0].Rows[i]["NIT"].ToString();
                                    }
                                    catch
                                    {
                                        dr["nit"] = " ERROR ";
                                        msgError += "renglón " + i.ToString() + " Error en el Nit \n";
                                    }

                                    try
                                    {
                                        dr["pref"] = dsComprobante.Tables[0].Rows[i]["PREF"].ToString();
                                    }
                                    catch
                                    {
                                        dr["pref"] = " ERROR ";
                                        msgError += "renglón " + i.ToString() + " Error en el Prefijo \n";
                                    }

                                    try
                                    {
                                        dr["docref"] = dsComprobante.Tables[0].Rows[i]["COMPROB"].ToString();
                                    }
                                    catch
                                    {
                                        dr["docref"] = 0;
                                        msgError += "renglón " + i.ToString() + " Error en el Documento Referencia \n";
                                    }

                                    try
                                    {
                                        dr["detalle"] = dsComprobante.Tables[0].Rows[i]["DETALLE"].ToString();
                                    }
                                    catch
                                    {
                                        dr["detalle"] = " ERROR ";
                                        msgError += "renglón " + i.ToString() + " Error en el Detalle \n";
                                    }

                                    try
                                    {
                                        dr["sede"] = dsComprobante.Tables[0].Rows[i]["SEDE"].ToString();
                                    }
                                    catch
                                    {
                                        dr["sede"] = " ERROR ";
                                        msgError += "renglón " + i.ToString() + " Error en la Sede \n";
                                    }

                                    try
                                    {
                                        dr["ccosto"] = dsComprobante.Tables[0].Rows[i]["CCOSTOS"].ToString();
                                    }
                                    catch
                                    {
                                        dr["ccosto"] = "ERROR";
                                        msgError += "renglón " + i.ToString() + " Error en el centro de Costo \n";
                                    }

                                    try
                                    {
                                        dr["debito"] = Convert.ToDouble(dsComprobante.Tables[0].Rows[i]["DEBITO"].ToString().Replace(",", "."));
                                    }
                                    catch
                                    {
                                        dr["debito"] = 0;
                                        msgError += "renglón " + i.ToString() + " Error en el débito \n";
                                    }

                                    try
                                    {
                                        dr["credito"] = Convert.ToDouble(dsComprobante.Tables[0].Rows[i]["CREDITO"].ToString().Replace(",","."));
                                    }
                                    catch
                                    {
                                        dr["credito"] = 0;
                                        msgError += "renglón " + i.ToString() + " Error en el crédito \n";
                                    }

                                    try
                                    {
                                        dr["base"] = Convert.ToDouble(dsComprobante.Tables[0].Rows[i]["BASE"].ToString().Replace(",", "."));
                                    }
                                    catch
                                    {
                                        dr["base"] = 0;
                                        msgError += "renglón " + i.ToString() + " Error en la Base \n";
                                    }

                                    try
                                    {
                                        dr["debitoNiif"] = Convert.ToDouble(dsComprobante.Tables[0].Rows[i]["DEBITONiif"].ToString().Replace(",", "."));
                                    }
                                    catch
                                    {
                                        dr["debitoNiif"] = 0;
                                        msgError += "renglón " + i.ToString() + " Error en el débito NIIF \n";
                                    }

                                    try
                                    {
                                        dr["creditoNiif"] = Convert.ToDouble(dsComprobante.Tables[0].Rows[i]["CREDITONiif"].ToString().Replace(",", "."));
                                    }
                                    catch
                                    {
                                        dr["creditoNiif"] = 0;
                                        msgError += "renglón " + i.ToString() + " Error en el crédito NIIF \n";
                                    }

                                    dtInserts.Rows.Add(dr);
                                }
                            }
 					        Diference();
						    BindDatas();
                        }
                    }
				}
				catch(Exception ex)
				{
					Page.RegisterClientScriptBlock("status",ex.ToString() + "--- Error ---" + msgError);
				}
			}
            if (msgError.Length > 0)
                //     Utils.MostrarAlerta(Response, msgError);
                Page.RegisterClientScriptBlock("status", "--- Error ---" + msgError);
        }

        protected void cargarDocsContables(object sender, System.EventArgs e)
        {
            if (this.uploadDocCont.PostedFile.FileName.Length == 0)
                Utils.MostrarAlerta(Response, "No ha especificado un nombre de archivo");
            else
            {
                //HttpPostedFile file = uploadDocCont.PostedFile;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFile fileC = Request.Files[i];
                    if (fileC.ContentLength > 0)
                    {
                        if (fileC.FileName.Contains("~"))
                        {
                            Utils.MostrarAlerta(Response, "El nombre del archivo: " + fileC.FileName + "no es un nombre válido, por favor cámbielo.(no puede contener caracteres especiales.)");
                            archivos.Clear();
                            Session["archivos"] = null;
                            return;
                        }
                        archivos.Add(fileC);
                        lbArchivos.Text += "<br>" + fileC.FileName;
                    }
                }
                Session["archivos"] = archivos;
            }
            //divDocumentos.Visible = true;
        }
        		//Validar
		protected void Button3_Click(object sender, System.EventArgs e)
		{
			Catalogo o_Catalgo = new Catalogo();
			CentroCosto o_CentroCosto = new CentroCosto();
			Almacen o_Almacen = new Almacen();
			Varios  o_Varios = new Varios();
			string  valor = "";
            string  prefRefe = "";
            Int32   numeRefe = 0;
			double  ValorBase = 0;
			for(int i=0;i<dtInserts.Rows.Count;i++)
			{
				valor = dtInserts.Rows[i][0].ToString();
//				valor = dgInserts.Items[i].Cells[0].Text;					
				o_Catalgo.p_AsignarCuenta = valor;
				if (o_Catalgo.p_Verificar == false)
				{
					dgInserts.Items[i].Cells[0].BackColor = Color.Red;
					errorgrilla = true;
				}
				else
				{
					ValorBase = Double.Parse(dtInserts.Rows[i][9].ToString());
					if (o_Catalgo.p_TBAS_CODIGO == "B")
					{
						if (ValorBase == 0)
						{
							dgInserts.Items[i].Cells[9].BackColor = Color.Red;
							errorgrilla = true;
						}
					}
				}
				valor = dtInserts.Rows[i][1].ToString();  // Nit
				if (o_Varios.verificarnit(valor) == false)
				{
					dgInserts.Items[i].Cells[1].BackColor = Color.Red;
					errorgrilla = true;
				}
                prefRefe =  dtInserts.Rows[i][2].ToString();  // Prefijo documento Referencia
                if (prefRefe.Length == 0 || prefRefe.Length > 6)
                {
                    dgInserts.Items[i].Cells[2].BackColor = Color.Red;
                    errorgrilla = true;
                }
                try
                {
                    numeRefe = Convert.ToInt32(dtInserts.Rows[i][3].ToString());   // Numero documento Referencia
                }
                catch
                {
                    dgInserts.Items[i].Cells[3].BackColor = Color.Red;
                    errorgrilla = true;
                }
                prefRefe = dtInserts.Rows[i][4].ToString();  // detalle o razon 
                if (prefRefe.Length < 3 || prefRefe.Length > 256)
                {
                    dgInserts.Items[i].Cells[4].BackColor = Color.Red;
                    errorgrilla = true;
                }
              	valor = dtInserts.Rows[i][6].ToString();					
				o_CentroCosto.p_AsignarCentro= valor;
				if (o_CentroCosto.p_Verificar == false)
				{
					dgInserts.Items[i].Cells[6].BackColor = Color.Red;
					errorgrilla = true;
				}
	            valor = dtInserts.Rows[i][5].ToString();
				o_Almacen.p_AsignarAlmacen = valor;
				if (o_Almacen.p_Verificar == false)
				{
					dgInserts.Items[i].Cells[5].BackColor = Color.Red;
					errorgrilla = true;
				}
			}
			if (errorgrilla)
			{
				Button1.Enabled = false;
			}
			else
			{
				Button1.Enabled = true;
			}	
			
		}

		#endregion

		#region Metodos

		protected void LoadDataColumns()
		{
			lbFields.Add("cuenta");
			lbFields.Add("nit");
			lbFields.Add("pref");
			lbFields.Add("docref");
			lbFields.Add("detalle");
			lbFields.Add("sede");
			lbFields.Add("ccosto");
			lbFields.Add("debito");
			lbFields.Add("credito");
			lbFields.Add("base");
            lbFields.Add("debitoNiif");
            lbFields.Add("creditoNiif");

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
		}

		protected void LoadDataTable()
		{
			dtInserts = new DataTable();
			for(i=0; i<lbFields.Count; i++)
				dtInserts.Columns.Add(new DataColumn((string)lbFields[i], (Type)types[i]));
			Session["dtInserts"] = dtInserts;
		}

		protected void BindDatas()
		{
			dgInserts.EnableViewState = true;
			dvInserts = new DataView(dtInserts);
			dgInserts.DataSource = dtInserts;
			dgInserts.DataBind();
		}

		protected bool CheckValues(DataGridCommandEventArgs e)
		{
			bool check=true;

			if(((TextBox)e.Item.Cells[0].Controls[1]).Text == "" && ((TextBox)e.Item.Cells[1].Controls[1]).Text == "" && ((TextBox)e.Item.Cells[2].Controls[1]).Text == "" &&
				((TextBox)e.Item.Cells[3].Controls[1]).Text == "" && ((TextBox)e.Item.Cells[4].Controls[1]).Text == "" &&
				((DropDownList)e.Item.Cells[5].Controls[1]).Items.Count>0 && ((DropDownList)e.Item.Cells[6].Controls[1]).Items.Count>0)
				check=false;

            if (((TextBox)e.Item.Cells[7].Controls[1]).Text == "" && ((TextBox)e.Item.Cells[8].Controls[1]).Text == "" && ((TextBox)e.Item.Cells[9].Controls[1]).Text == "" && ((TextBox)e.Item.Cells[10].Controls[1]).Text == "" && ((TextBox)e.Item.Cells[11].Controls[1]).Text == "")
				check=false;

            if (((TextBox)e.Item.Cells[7].Controls[1]).Text == "0" && ((TextBox)e.Item.Cells[8].Controls[1]).Text == "0" && ((TextBox)e.Item.Cells[9].Controls[1]).Text == "0" && ((TextBox)e.Item.Cells[10].Controls[1]).Text == "0" && ((TextBox)e.Item.Cells[11].Controls[1]).Text == "0")
				check=false;

			return check;
		}

		protected bool CheckValuesUpdate(DataGridCommandEventArgs e)
		{
			bool check=true;
			if(((TextBox)e.Item.Cells[0].FindControl("edit_1")).Text == String.Empty)
				check = false;
			if(((TextBox)e.Item.Cells[1].FindControl("edit_2")).Text == String.Empty)
				check = false;
			if(((TextBox)e.Item.Cells[2].FindControl("edit_3")).Text == String.Empty)
				check = false;
			if(((TextBox)e.Item.Cells[3].FindControl("edit_4")).Text == String.Empty)
				check = false;
			if(((TextBox)e.Item.Cells[4].FindControl("edit_5")).Text == String.Empty)
				check = false;
			if(((DropDownList)e.Item.Cells[5].FindControl("ddlSedeEdicion")).Items.Count == 0)
				check = false;
			if(((DropDownList)e.Item.Cells[6].FindControl("ddlCCEdicion")).Items.Count == 0)
				check = false;
			if(((TextBox)e.Item.Cells[7].FindControl("edit_8")).Text == String.Empty && ((TextBox)e.Item.Cells[8].FindControl("edit_9")).Text == String.Empty)
				check = false;
			return check;
		}

		protected void Diference()
		{
            double debito = 0, credito = 0, debitoNiif = 0, creditoNiif = 0;
			for(i=0; i<dtInserts.Rows.Count; i++)
			{
				debito += Math.Round(Convert.ToDouble(dtInserts.Rows[i][7]),2);
				credito += Math.Round(Convert.ToDouble(dtInserts.Rows[i][8]),2);
                debitoNiif += Math.Round(Convert.ToDouble(dtInserts.Rows[i][10]), 2);
                creditoNiif += Math.Round(Convert.ToDouble(dtInserts.Rows[i][11]), 2);
			}
			tbDebito.Text = debito.ToString();
			tbCredito.Text = credito.ToString();
            tbDebitoNiif.Text = debitoNiif.ToString();
            tbCreditoNiif.Text = creditoNiif.ToString();
			tbDiferencia.Text = Math.Round(debito-credito,2).ToString();
            tbDiferenciaNiif.Text = Math.Round(debitoNiif - creditoNiif, 2).ToString();
		}

		public bool VerificoComprobante(string tipoPre, int numComp)
		{
			if(DBFunctions.RecordExist("SELECT * FROM mcomprobante WHERE pdoc_codigo = '" + tipoPre + "' AND mcom_numedocu = " + numComp ))
				return true;
			else
				return false;
		}

		public void ActualizaConsecutivo(string codigo)
		{
			int n =0;
			n = DBFunctions.NonQuery("UPDATE pdocumento SET pdoc_ultidocu = pdoc_ultidocu + 1 WHERE pdoc_codigo ='" + codigo + "'");
		}

		public void ConsecutivoComprobante()
		{
			string lastId = (string) DBFunctions.SingleData("SELECT pdoc_ultidocu FROM pdocumento WHERE pdoc_codigo = '" + Request.QueryString["typeNum"] + "'");
			if(lastId == "")
				idComp.Text = "1";
			else
			{
				idComp.Text = (Convert.ToInt32(lastId)+1).ToString();		//id.ToString();
			}
		}

		private void DateEdit()
		{
			string date=Request.QueryString["date"];
			int    index1 = date.IndexOf(' ');
			string dateNew=date.Substring(0,index1);
			lbDate.Text=dateNew;
		}

		private void CargarRenglonesPlantilla(string prefijoPlantilla, int numeroPlantilla, string prefijoComprobante, int numeroComprobante)
		{
			DataSet dsConsulta = new DataSet();
            DBFunctions.Request(dsConsulta, IncludeSchema.NO, "SELECT mcue_codipuc, mnit_nit, '" + prefijoComprobante + "', " + numeroComprobante + ", '', palm_almacen, pcen_codigo, dcue_valodebi, dcue_valocred, dcue_valobase, dcue_NIIFdebi, dcue_NIIFcred FROM dcuenta WHERE pdoc_codigo='" + prefijoPlantilla + "' AND MCOM_NUMEDOCU=" + numeroPlantilla);
			for(int i=0;i<dsConsulta.Tables[0].Rows.Count;i++)
			{
				DataRow dr = dtInserts.NewRow();
				for(int j=0;j<dtInserts.Columns.Count;j++)
					dr[j] = dsConsulta.Tables[0].Rows[i][j];
				dtInserts.Rows.Add(dr);
			}
			Diference();
			BindDatas();
		}
        protected bool insertarDocumentos(string prefijo, string numero)
        {
            string sqlDocu, nombArchivo, usuario;
            string docuEscan = "No aplica";
            DataSet ds = new DataSet();
            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT MCOD_NOMBDOCUMENTO FROM MCOMPROBANTEDOCUMENTO WHERE PDOC_CODIGO = '" + prefijo + "' AND MCOM_NUMEDOCU = " + numero);
            if (ds.Tables.Count == 0)
                return false;
            usuario = HttpContext.Current.User.Identity.Name.ToLower();
            ArrayList sqlDocumentos = new ArrayList();
            //string path = ConfigurationSettings.AppSettings["PathToUpDocCont"];
            string path = ConfigurationManager.AppSettings["PathToUpDocCont"];

            for (int i = 0; i < archivos.Count; i++)
            {   //                               [alt + 126]
                nombArchivo = GlobalData.EMPRESA + "~" + prefijo + "~" + numero + "~" + ((HttpPostedFile)archivos[i]).FileName; //.Split('\\');
                try
                {
                    ((HttpPostedFile)archivos[i]).SaveAs(path + nombArchivo); //[nombre.Length - 1]);

                    if (ds.Tables[0].Select("MCOD_NOMBDOCUMENTO = '" + ((HttpPostedFile)archivos[i]).FileName + "'").Length == 0)
                    {
                        string nombDocu = ((HttpPostedFile)archivos[i]).FileName;
                        //Escribir todos los nombres, porque si en un futuro se agregan más campos, esto evita el caos de generar un error por cantidad de columnas y de datos insertados.
                        sqlDocu = @"INSERT INTO MCOMPROBANTEDOCUMENTO (PDOC_CODIGO, MCOM_NUMEDOCU, MCOD_NOMBDOCUMENTO, MCOD_NOMBARCHIVO, MCOD_ESCANER, MCOD_FECHA, SUSU_LOGIN)
                                            VALUES ('" + prefijo + "', " + numero + ", '" + nombDocu + "', '" + nombArchivo + "', '" + docuEscan + "', " + "DEFAULT, '" + usuario + "');";
                        sqlDocumentos.Add(sqlDocu);
                        //if (!File.Exists(path + nombArchivo))
                        //{

                        //}
                    }
                }
                catch
                {
                    return false;
                }
            }
            if (!DBFunctions.Transaction(sqlDocumentos))
            {
                return false;
                //lbInfo.Text += "Proceso realizado correctamente! Pero hubo un error al guardar los archivos: <br /> Error al grabar archivos de soporte. <br />" + DBFunctions.exceptions;
                //Utils.MostrarAlerta(Response, "Se ha realizado el proceso correctamente!! pero por un eror desconocido no ha sido posible archivar los documentos soporte, por favor contacte a ECAS");
                //Button1.Visible = false;
                //Response.Redirect("" + indexPage + "?process=Contabilidad.CapEdiCom&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&pref=" + Request.QueryString["typeNum"] + "&num=" + idComp.Text + "&falloDoc=1");
            }
            else
            {
                return true;
                //dtInserts.Clear();
                //Session.Clear();
                //Response.Redirect("" + indexPage + "?process=Contabilidad.CapEdiCom&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&pref=" + Request.QueryString["typeNum"] + "&num=" + idComp.Text);
            }
        }

		#endregion

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
