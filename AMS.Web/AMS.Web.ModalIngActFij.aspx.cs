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

namespace AMS.Web
{
	public partial class ModalActivos : System.Web.UI.Page
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
				tbfecfaccom.Text=tbfecing.Text=tbfecinidep.Text=DateTime.Now.ToString("yyyy-MM-dd");
				bind.PutDatasIntoDropDownList(ddlcencos,"SELECT pcen_codigo,pcen_nombre FROM dbxschema.pcentrocosto");
				bind.PutDatasIntoDropDownList(ddlpref,"SELECT DISTINCT MFAC.pdoc_codiordepago,MFAC.pdoc_codiordepago CONCAT ' - ' CONCAT PDOC.pdoc_descripcion FROM mfacturaproveedor MFAC,pdocumento PDOC WHERE MFAC.pdoc_codiordepago=PDOC.pdoc_codigo AND PDOC.tdoc_tipodocu='FP' ORDER BY pdoc_codiordepago ASC");
				bind.PutDatasIntoDropDownList(ddlnum,"SELECT mfac_numeordepago FROM mfacturaproveedor WHERE pdoc_codiordepago='"+ddlpref.SelectedValue+"'");
				bind.PutDatasIntoDropDownList(ddlvig,"SELECT * FROM tvigencia");
				DatasToControls.EstablecerValueDropDownList(ddlvig,"V");
			}
		}
		
		protected void ddlPref_IndexChanged(object Sender,EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlnum,"SELECT mfac_numeordepago FROM mfacturaproveedor WHERE pdoc_codiordepago='"+ddlpref.SelectedValue+"'");
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
					Response.Write("<script language:javascript>alert('"+err+"');</script>");
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
					Response.Write("<script language:javascript>alert('"+err+"');</script>");
				
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
					Response.Write("<script language:javascript>alert('"+err+"');</script>");
					
			}
			else if(e.CommandName=="guardar")
			{
				if(!Ejecutar_Validaciones_pnlCuentas(ref err))
				{
					lnbSig.Enabled=false;
					lnbAnt.Enabled=false;
					tbcuereal.Enabled=tbcuedep.Enabled=tbcuegasdep.Enabled=tbcueinf.Enabled=tbcueinfdep.Enabled=tbcormoninf.Enabled=tbcormondep.Enabled=false;
					Response.Write("<script language:javascript>alert('Ahora puede guardar su activo fijo');</script>");
					btnGuardar.Visible=true;
				}
				else
					Response.Write("<script language:javascript>alert('"+err+"');</script>");
			}
		}
						
		protected void btnGuardar_Click(object Sender,EventArgs e)
		{
			string sql="INSERT INTO mactivofijo VALUES('"+tbcod.Text+"','"+tbdesc.Text+"','"+ddlcencos.SelectedValue+"','"+tbmarca.Text+"','"+tbmodelo.Text+"','"+tbplaca.Text+"',"+
			    	    "'"+tbfecfaccom.Text+"','"+tbfecing.Text+"','"+tbfecinidep.Text+"','"+tbnit.Text+"','"+ddlpref.SelectedValue+"',"+ddlnum.SelectedValue+","+tbnumped.Text+","+
			        	""+Convert.ToDouble(tbvalhis.Text)+","+Convert.ToDouble(tbvalhisdol.Text)+","+Convert.ToDouble(tbvalmej.Text)+","+Convert.ToDouble(tbvalinf.Text)+","+Convert.ToDouble(tbvaldep.Text)+","+Convert.ToDouble(tbvalinfdep.Text)+","+tbnumcuo.Text+","+
			         	"'"+ddlvig.SelectedValue+"','"+tbcuereal.Text+"','"+tbcuedep.Text+"','"+tbcuegasdep.Text+"','"+tbcueinf.Text+"','"+tbcueinfdep.Text+"','"+tbcormoninf.Text+"','"+tbcormondep.Text+"')";
			Session["instFac"]=sql;
			string jsparams="javascript: GetValue('"+this.tbcod.Text+","+this.tbdesc.Text+","+Convert.ToDouble(this.tbvalhis.Text).ToString()+"')";
		}
		
		protected void btnCancelar_Click(object Sender,EventArgs e)
		{
			Response.Redirect(MainPage+"?process=Tesoreria.ActivosFijos.Ingresar");
		}
		
		protected bool Ejecutar_Validaciones_pnlDatos(ref string err)
		{
			bool error=false;
			if(DBFunctions.RecordExist("SELECT * FROM mactivofijo WHERE mafj_codiacti='"+tbcod.Text+"'"))
			{
				error=true;
				err="El código del activo fijo ya existe";
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
			if(!DBFunctions.RecordExist("SELECT * FROM mcuenta WHERE mcue_codipuc='"+tbcuereal.Text+"'"))
			{
				error=true;
				err="La cuenta real del activo fijo no existe";
			}
			else if(!DBFunctions.RecordExist("SELECT * FROM mcuenta WHERE mcue_codipuc='"+tbcuedep.Text+"'"))
			{
				error=true;
				err="La cuenta de depreciación del activo fijo no existe";
			}
			else if(!DBFunctions.RecordExist("SELECT * FROM mcuenta WHERE mcue_codipuc='"+tbcuegasdep.Text+"'"))
			{
				error=true;
				err="La cuenta de gasto de la depreciación del activo fijo no existe";
			}
			else if(!DBFunctions.RecordExist("SELECT * FROM mcuenta WHERE mcue_codipuc='"+tbcueinf.Text+"'"))
			{
				error=true;
				err="La cuenta de inflación del activo fijo no existe";
			}
			else if(!DBFunctions.RecordExist("SELECT * FROM mcuenta WHERE mcue_codipuc='"+tbcueinfdep.Text+"'"))
			{
				error=true;
				err="La cuenta de inflación de la depreciación del activo fijo no existe";
			}
			else if(!DBFunctions.RecordExist("SELECT * FROM mcuenta WHERE mcue_codipuc='"+tbcormoninf.Text+"'"))
			{
				error=true;
				err="La cuenta de la corrección monetaria de la inflación no existe";
			}
			else if(!DBFunctions.RecordExist("SELECT * FROM mcuenta WHERE mcue_codipuc='"+tbcormondep.Text+"'"))
			{
				error=true;
				err="La cuenta de la corrección monetaria de la depreciación no existe";
			}
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
