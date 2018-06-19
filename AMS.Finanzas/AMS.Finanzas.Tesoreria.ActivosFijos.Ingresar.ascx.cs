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
	public partial class IngresarActivosFijos : System.Web.UI.UserControl
	{
		protected DataSet ds;
		protected string MainPage=ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				lnbAnt.CommandName="pnlDatos";
				lnbSig.CommandName="pnlCompra";

				if(Request.QueryString["act"]==null)
				{
					tbfecfaccom.Text=tbfecing.Text=tbfecinidep.Text=DateTime.Now.ToString("yyyy-MM-dd");
                    bind.PutDatasIntoDropDownList(ddlcencos, "SELECT pcen_codigo,pcen_nombre FROM dbxschema.pcentrocosto where timp_codigo <> 'N' order by 1 ");
					bind.PutDatasIntoDropDownList(ddlvig,"SELECT * FROM tvigencia");
					DatasToControls.EstablecerValueDropDownList(ddlvig,"V");
				}
				else
				{
					Establecer_Valores();
				}
			}
		}
		
		protected void lnbAnt_Command(object Sender,CommandEventArgs e)
		{
			if(e.CommandName=="pnlDatos")
			{
				pnlDatos.Visible=true;
				pnlCompra.Visible=false;
				pnlCostos.Visible=false;
				pnlCuentas.Visible=false;
				lnbAnt.Enabled=false;//voy para
				lnbSig.CommandName="pnlCompra";//vengo de
			}
			else if(e.CommandName=="pnlCompra")
			{
				pnlDatos.Visible=false;
				pnlCompra.Visible=true;
				pnlCostos.Visible=false;
				pnlCuentas.Visible=false;
				lnbAnt.CommandName="pnlDatos";//voy para
				lnbSig.CommandName="pnlCostos";//vengo de
			}
			else if(e.CommandName=="pnlCostos")
			{
				pnlDatos.Visible=false;
				pnlCompra.Visible=false;
				pnlCostos.Visible=true;
				pnlCuentas.Visible=false;
				lnbAnt.CommandName="pnlCompra";//voy para
				lnbSig.CommandName="pnlCuentas";//vengo de
				lnbSig.Enabled=true;
			}
		}
		
		protected void lnbSig_Command(object Sender,CommandEventArgs e)
		{
			string err="";
			if(e.CommandName=="pnlCompra")
			{
				if(!Ejecutar_Validaciones_pnlDatos(ref err))
				{
					pnlDatos.Visible=false;
					pnlCompra.Visible=true;
					pnlCostos.Visible=false;
					pnlCuentas.Visible=false;
					lnbSig.CommandName="pnlCostos";//voy para
					lnbAnt.CommandName="pnlDatos";// vengo de
					lnbAnt.Enabled=true;
				}
				else
                    Utils.MostrarAlerta(Response, "" + err + "");
			}
			else if(e.CommandName=="pnlCostos")
			{
				if(!Ejecutar_Validaciones_pnlCompra(ref err))
				{
					pnlDatos.Visible=false;
					pnlCompra.Visible=false;
					pnlCostos.Visible=true;
					pnlCuentas.Visible=false;
					lnbSig.CommandName="pnlCuentas";//voy para
					lnbAnt.CommandName="pnlCompra";//vengo de
				}
				else
                    Utils.MostrarAlerta(Response, "" + err + "");
				
			}
			else if(e.CommandName=="pnlCuentas")
			{
				if(!Ejecutar_Validaciones_pnlCostos(ref err))
				{
					pnlDatos.Visible=false;
					pnlCompra.Visible=false;
					pnlCostos.Visible=false;
					pnlCuentas.Visible=true;
					lnbSig.CommandName="guardar";//voy para
					lnbAnt.CommandName="pnlCostos";//vengo de
				}
				else
                    Utils.MostrarAlerta(Response, "" + err + "");
					
			}
			else if(e.CommandName=="guardar")
			{
				if(!Ejecutar_Validaciones_pnlCuentas(ref err))
				{
					lnbSig.Enabled=false;
					lnbAnt.Enabled=false;
					tbcuereal.Enabled=tbcuedep.Enabled=tbcuegasdep.Enabled=tbcueinf.Enabled=tbcueinfdep.Enabled=tbcormoninf.Enabled=tbcormondep.Enabled=false;
                    Utils.MostrarAlerta(Response, "Ahora puede guardar su activo fijo");
					btnGuardar.Visible=true;
				}
				else
                Utils.MostrarAlerta(Response, "" + err + "");
			}
		}
						
		protected void btnGuardar_Click(object Sender,EventArgs e)
		{
			ArrayList sqls=new ArrayList();
			if(Request.QueryString["act"]==null)
			{
                //sqls.Add("INSERT INTO mactivofijo VALUES('"+tbcod.Text+"','"+tbdesc.Text+"','"+ddlcencos.SelectedValue+"','"+tbmarca.Text+"','"+tbmodelo.Text+"','"+tbplaca.Text+"',"+
                //        "'"+tbfecfaccom.Text+"','"+tbfecing.Text+"','"+tbfecinidep.Text+"','"+tbnit.Text+"',"+tbnumped.Text+","+
                //        ""+Convert.ToDouble(tbvalhis.Text)+","+Convert.ToDouble(tbvalhisdol.Text)+","+Convert.ToDouble(tbvalmej.Text)+","+Convert.ToDouble(tbvalinf.Text)+","+Convert.ToDouble(tbvaldep.Text)+","+Convert.ToDouble(tbvalinfdep.Text)+","+tbnumcuo.Text+","+
                //        "'"+ddlvig.SelectedValue+"','"+tbcuereal.Text+"','"+tbcuedep.Text+"','"+tbcuegasdep.Text+"','"+tbcueinf.Text+"','"+tbcueinfdep.Text+"','"+tbcormoninf.Text+"','"+tbcormondep.Text+"')");

                sqls.Add("INSERT INTO mactivofijo VALUES('" + tbcod.Text + "','" + tbdesc.Text + "','" + ddlcencos.SelectedValue + "','" + tbmarca.Text + "'," +
                                                       "'" + tbmodelo.Text + "','" + tbplaca.Text + "'," + "'" + tbfecfaccom.Text + "', '" + tbfecing.Text + "', " +
                                                       "'" + tbfecinidep.Text + "','" + tbnit.Text + "'," + tbnumped.Text + "," + Convert.ToDouble(tbvalhis.Text) + "," +
                                                       Convert.ToDouble(tbvalhisdol.Text) + "," + Convert.ToDouble(tbvalmej.Text) + "," + Convert.ToDouble(tbvaldep.Text) + "," +
                                                       tbnumcuo.Text + "," + "'" + ddlvig.SelectedValue + "','" + tbcuereal.Text + "','" + tbcuedep.Text + "','" + tbcuegasdep.Text + "'," +
                                                       "'" + tbcuereal.Text + "','" + tbcuereal.Text + "',0,0,0,0,0)"); //Parametros para NIIF restantes, por parametrizar en tabla.

				if(DBFunctions.Transaction(sqls))
					Response.Redirect(MainPage+"?process=Tesoreria.ActivosFijos&fin=1");
					//lb.Text+="Bien "+DBFunctions.exceptions;
				else
					lb.Text+="Error "+DBFunctions.exceptions;
			}
			else
			{
                //sqls.Add("UPDATE mactivofijo SET mafj_descripcion='"+tbdesc.Text+"',pcco_centcost='"+ddlcencos.SelectedValue+"',mafj_marca='"+tbmarca.Text+"',mafj_modelo='"+tbmodelo.Text+"',mafj_placa='"+tbplaca.Text+"',"+
                //        "mafj_fechfact='"+tbfecfaccom.Text+"',mafj_ingreso='"+tbfecing.Text+"',mafj_fechinic='"+tbfecinidep.Text+"',mnit_nit='"+tbnit.Text+"',mafj_pedido="+tbnumped.Text+","+
                //        "mafj_valohist="+Convert.ToDouble(tbvalhis.Text)+",mafj_valodola="+Convert.ToDouble(tbvalhisdol.Text)+",mafj_valomejora="+Convert.ToDouble(tbvalmej.Text)+",mafj_inflacum="+Convert.ToDouble(tbvalinf.Text)+",mafj_depracum="+Convert.ToDouble(tbvaldep.Text)+",mafj_infldepracum="+Convert.ToDouble(tbvalinfdep.Text)+",mafj_cuotas="+tbnumcuo.Text+",tvig_vigencia='"+ddlvig.SelectedValue+"',"+
                //        "mafj_cuenactifijo='"+tbcuereal.Text+"',mafj_cuendepr='"+tbcuedep.Text+"',mafj_cuengastdepr='"+tbcuegasdep.Text+"',mafj_cueninflacti='"+tbcueinf.Text+"',mafj_cueninfldepr='"+tbcueinfdep.Text+"',mafj_cuencmoninfl='"+tbcormoninf.Text+"',mafj_cuencmondepr='"+tbcormondep.Text+"' WHERE mafj_codiacti='"+tbcod.Text+"'");
                sqls.Add("UPDATE mactivofijo SET mafj_descripcion='" + tbdesc.Text + "',pcco_centcost='" + ddlcencos.SelectedValue + "',mafj_marca='" + tbmarca.Text + "',mafj_modelo='" + tbmodelo.Text + "',mafj_placa='" + tbplaca.Text + "'," +
                        "mafj_fechfact='" + tbfecfaccom.Text + "',mafj_ingreso='" + tbfecing.Text + "',mafj_fechinic='" + tbfecinidep.Text + "',mnit_nit='" + tbnit.Text + "',mafj_pedido=" + tbnumped.Text + "," +
                        "mafj_valohist=" + Convert.ToDouble(tbvalhis.Text) + ",mafj_valodola=" + Convert.ToDouble(tbvalhisdol.Text) + ",mafj_valomejora=" + Convert.ToDouble(tbvalmej.Text) + ",mafj_depracum=" + Convert.ToDouble(tbvaldep.Text) + ",mafj_cuotas=" + tbnumcuo.Text + ",tvig_vigencia='" + ddlvig.SelectedValue + "'," +
                        "mafj_cuenactifijo='" + tbcuereal.Text + "',mafj_cuendepr='" + tbcuedep.Text + "',mafj_cuengastdepr='" + tbcuegasdep.Text + "' WHERE mafj_codiacti='" + tbcod.Text + "'");
				
                if(DBFunctions.Transaction(sqls))
                    Response.Redirect(MainPage + "?process=Tesoreria.ActivosFijos&fin=2");
					//lb.Text+="Bien "+DBFunctions.exceptions;
				else
					lb.Text+="Error "+DBFunctions.exceptions;
			}
		}
		
		protected void btnCancelar_Click(object Sender,EventArgs e)
		{
			Response.Redirect(MainPage+"?process=Tesoreria.ActivosFijos");
		}
		
		protected void Establecer_Valores()
		{
			DatasToControls bind=new DatasToControls();
			ds=new DataSet();
			ds=DBFunctions.Request(ds,IncludeSchema.NO,"SELECT * FROM mactivofijo WHERE mafj_codiacti='"+Request.QueryString["act"]+"'");
			tbcod.Text=ds.Tables[0].Rows[0][0].ToString();
			tbcod.Enabled=false;
			tbdesc.Text=ds.Tables[0].Rows[0][1].ToString();
            bind.PutDatasIntoDropDownList(ddlcencos, "SELECT pcen_codigo,pcen_nombre FROM dbxschema.pcentrocosto where timp_codigo <> 'N' order by 1");
			DatasToControls.EstablecerValueDropDownList(ddlcencos,ds.Tables[0].Rows[0][2].ToString());
			tbmarca.Text=ds.Tables[0].Rows[0][3].ToString();
			tbmodelo.Text=ds.Tables[0].Rows[0][4].ToString();
			tbplaca.Text=ds.Tables[0].Rows[0][5].ToString();
			tbfecfaccom.Text=Convert.ToDateTime(ds.Tables[0].Rows[0][6]).ToString("yyyy-MM-dd");
			tbfecing.Text=Convert.ToDateTime(ds.Tables[0].Rows[0][7]).ToString("yyyy-MM-dd");
			tbfecinidep.Text=Convert.ToDateTime(ds.Tables[0].Rows[0][8]).ToString("yyyy-MM-dd"); 
			tbnit.Text=ds.Tables[0].Rows[0][9].ToString();
			tbnumped.Text=ds.Tables[0].Rows[0][10].ToString();
			tbvalhis.Text=ds.Tables[0].Rows[0][11].ToString();
			tbvalhisdol.Text=ds.Tables[0].Rows[0][12].ToString();
			tbvalmej.Text=ds.Tables[0].Rows[0][13].ToString();
			tbvalinf.Text=ds.Tables[0].Rows[0][14].ToString();
			tbvaldep.Text=ds.Tables[0].Rows[0][15].ToString();
			tbvalinfdep.Text=ds.Tables[0].Rows[0][16].ToString();
			tbnumcuo.Text=ds.Tables[0].Rows[0][17].ToString();
			bind.PutDatasIntoDropDownList(ddlvig,"SELECT * FROM tvigencia");
			DatasToControls.EstablecerValueDropDownList(ddlvig,ds.Tables[0].Rows[0][18].ToString());
			tbcuereal.Text=ds.Tables[0].Rows[0][19].ToString();
			tbcuedep.Text=ds.Tables[0].Rows[0][20].ToString();
			tbcuegasdep.Text=ds.Tables[0].Rows[0][21].ToString();
			tbcueinf.Text=ds.Tables[0].Rows[0][22].ToString();
			tbcueinfdep.Text=ds.Tables[0].Rows[0][23].ToString();
			tbcormoninf.Text=ds.Tables[0].Rows[0][24].ToString();
			tbcormondep.Text=ds.Tables[0].Rows[0][25].ToString();
		}
		
		protected bool Ejecutar_Validaciones_pnlDatos(ref string err)
		{
			bool error=false;
			if(Request.QueryString["act"]==null)
			{
				if(DBFunctions.RecordExist("SELECT * FROM mactivofijo WHERE mafj_codiacti='"+tbcod.Text+"'"))
				{
					error=true;
					err="El código del activo fijo ya existe";
				}
			}
			return error;
		}
		
		protected bool Ejecutar_Validaciones_pnlCompra(ref string err)
		{
			bool error=false;
			if(Convert.ToDateTime(tbfecfaccom.Text)>DateTime.Now.Date)
			{
				error=true;
				err="La fecha de compra no puede ser mayor a la fecha actual";
			}
			else if(Convert.ToDateTime(tbfecing.Text)>DateTime.Now.Date)
			{
				error=true;
				err="La fecha de ingreso no puede ser mayor a la fecha actual";
			}
			else if(!DBFunctions.RecordExist("SELECT * FROM mnit WHERE mnit_nit='"+tbnit.Text+"'"))
			{
				error=true;
				err="El nit es inexistente";
			}
			return error;
		}
		
		protected bool Ejecutar_Validaciones_pnlCostos(ref string err)
		{
			bool error=false;
			if(Convert.ToDouble(tbvalhis.Text)==0 || Convert.ToDouble(tbvalhisdol.Text)==0)
			{
				error=true;
				err="El valor de compra o el valor en dolares no puede ser cero";
			}
			return error;
		}
		
		protected bool Ejecutar_Validaciones_pnlCuentas(ref string err)
		{
			bool error=false;
			if(!DBFunctions.RecordExist("SELECT * FROM mcuenta WHERE mcue_codipuc='"+tbcuereal.Text+"' and timp_codigo in ('A','P')"))
			{
				error=true;
				err="La cuenta real del activo fijo no existe o es NO Imputable";
			}
			else if(!DBFunctions.RecordExist("SELECT * FROM mcuenta WHERE mcue_codipuc='"+tbcuedep.Text+"' and timp_codigo in ('A','P')"))
			{
				error=true;
				err="La cuenta de depreciación del activo fijo no existe o es NO Imputable";
			}
			else if(!DBFunctions.RecordExist("SELECT * FROM mcuenta WHERE mcue_codipuc='"+tbcuegasdep.Text+"' and timp_codigo in ('A','P')"))
			{
				error=true;
				err="La cuenta de gasto de la depreciación del activo fijo no existe o es NO Imputable";
			}
            //else if(!DBFunctions.RecordExist("SELECT * FROM mcuenta WHERE mcue_codipuc='"+tbcueinf.Text+"' and timp_codigo in ('A','P')"))
            //{
            //    error=true;
            //    err="La cuenta de inflación del activo fijo no existe o es NO Imputable";
            //}
            //else if(!DBFunctions.RecordExist("SELECT * FROM mcuenta WHERE mcue_codipuc='"+tbcueinfdep.Text+"' and timp_codigo in ('A','P')"))
            //{
            //    error=true;
            //    err="La cuenta de inflación de la depreciación del activo fijo no existe o es NO Imputable";
            //}
            //else if(!DBFunctions.RecordExist("SELECT * FROM mcuenta WHERE mcue_codipuc='"+tbcormoninf.Text+"' and timp_codigo in ('A','P')"))
            //{
            //    error=true;
            //    err="La cuenta de la corrección monetaria de la inflación no existe o es NO Imputable";
            //}
            //else if(!DBFunctions.RecordExist("SELECT * FROM mcuenta WHERE mcue_codipuc='"+tbcormondep.Text+"' and timp_codigo in ('A','P')"))
            //{
            //    error=true;
            //    err="La cuenta de la corrección monetaria de la depreciación no existe o es NO Imputable";
            //}
			return error;
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
