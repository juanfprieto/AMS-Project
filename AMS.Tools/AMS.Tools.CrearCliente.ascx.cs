using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Xml;
using AMS.DB;
using AMS.Forms;

namespace AMS.Tools
{

	public partial class CrearCliente : System.Web.UI.UserControl
	{
        protected Panel pnlInfo, pnlContrato, pnlProductos, pnlContactos;
        protected TextBox tbmarcas, tbnomger, tbnomcon, tbnomrev, tbfecini, tbfecfin, tbvalcon, tbnumvis;
        protected DropDownList ddlnit, ddltarifa, ddlvig;
        protected DataGrid dgProductos, dgContactos;
        protected DataTable dtProductos, dtContactos;
        protected LinkButton lnbAnt, lnbSig;
        protected Button btnCancelar, btnGuardar;
        protected Label lb;
		protected string MainPage=ConfigurationManager.AppSettings["MainIndexPage"];
        protected string mainPath = ConfigurationManager.AppSettings["MainPath"];
		protected XmlDocument webConfig;
		protected XmlNode autorizacion;
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				lnbAnt.CommandName="pnlInfo";
				lnbSig.CommandName="pnlContrato";
				DatasToControls bind=new DatasToControls();
				if(Request.QueryString["nit"]==null)
				{
                    bind.PutDatasIntoDropDownList(ddlnit, "SELECT mnit_nit, CASE WHEN TNIT_TIPONIT = 'N' THEN mnit_nit CONCAT ' - ' CONCAT MNIT_DIGITO CONCAT ' ' CONCAT MNIT_APELLIDOS ELSE  mnit_nit CONCAT '  ' CONCAT MNIT_APELLIDOS CONCAT ' ' CONCAT COALESCE(MNIT_APELLIDO2,'') CONCAT ' ' CONCAT MNIT_NOMBRES CONCAT ' ' CONCAT COALESCE(MNIT_NOMBRE2,'') END FROM mnit order by 2");
                    //ddlnit.Attributes.Add("")
					bind.PutDatasIntoDropDownList(ddltarifa,"SELECT ptar_codigo,ptar_descripcion FROM dbxschema.ptarifas");
					bind.PutDatasIntoDropDownList(ddlvig,"SELECT tvig_codigo,tvig_nombre FROM dbxschema.tvigencia");
					DatasToControls.EstablecerValueDropDownList(ddlvig,"V");
					this.Llenar_dtProductos_Inicial();
					this.Cargar_dtContactos();
					this.Bind_Datas_dtContactos();
				}
				else
					Establecer_Valores();
			}
			else
			{
				if(Session["dtProductos"]!=null)
					dtProductos=(DataTable)Session["dtProductos"];
				if(Session["dtContactos"]!=null)
					dtContactos=(DataTable)Session["dtContactos"];
			}
			webConfig = new XmlDocument();
			try
			{
				webConfig.Load(mainPath+"Web.Config");
				autorizacion = webConfig.DocumentElement;
				XmlNode root = webConfig.DocumentElement;
				XmlNodeList oNodeList = root.ChildNodes;
				foreach (XmlNode oCurrentNode in oNodeList)
				{
					if (oCurrentNode.HasChildNodes)
					{
						foreach(XmlNode oCurrentSubNode in oCurrentNode.ChildNodes)
						{
							if(oCurrentSubNode.Name.Trim()=="authentication")
								autorizacion = oCurrentSubNode;
						}
					}
				}
			}
			catch(Exception exc)
			{
				lb.Text = exc.ToString();
			}
		}
		
		protected void dgContactos_ItemCommand(object Sender,DataGridCommandEventArgs e)
		{
            
			if(((Button)e.CommandSource).CommandName=="agregar")	
			{
                string nit = "";
                try
                {
                    nit = ((TextBox)e.Item.Cells[0].Controls[1]).Text.Split('-')[0].Trim();
                }
                catch(Exception z)
                {
                    nit = ((TextBox)e.Item.Cells[0].Controls[1]).Text.Trim();
                }
				if(dtContactos.Rows.Count==5)
                    Utils.MostrarAlerta(Response, "Solo puede tener máximo 5 contactos");
                else if (DBFunctions.SingleData("SELECT COUNT(*) FROM dbxschema.mnit WHERE mnit_nit="+nit+"") != "1")
                    Utils.MostrarAlerta(Response, "El nit seleccionado no pertenece a un contacto, Cree este nit previamente");
                //else if(DBFunctions.RecordExist("SELECT * FROM mcontacto WHERE mnit_nitcon='"+nit+"'"))
                //    Utils.MostrarAlerta(Response, "El nit ya esta registrado como contacto");
				else if(((TextBox)e.Item.Cells[1].Controls[1]).Text=="" || ((TextBox)e.Item.Cells[2].Controls[1]).Text=="" || ((TextBox)e.Item.Cells[3].Controls[1]).Text=="")
                    Utils.MostrarAlerta(Response, "Falta un campo por llenar en la tabla");
				else if(Verificar_dtContactos(((TextBox)e.Item.Cells[0].Controls[1]).Text))
                    Utils.MostrarAlerta(Response, "Ya ingresó este contacto anteriormente");
				else
				{
					DataRow fila=dtContactos.NewRow();
					fila[0]=((TextBox)e.Item.Cells[0].Controls[1]).Text;
					fila[1]=((TextBox)e.Item.Cells[1].Controls[1]).Text;
					fila[2]=((TextBox)e.Item.Cells[2].Controls[1]).Text;
					fila[3]=((TextBox)e.Item.Cells[3].Controls[1]).Text;
					dtContactos.Rows.Add(fila);
					this.Bind_Datas_dtContactos();
				}
			}
			else if(((Button)e.CommandSource).CommandName=="eliminar")
			{
                string NIT = (dtContactos.Rows[e.Item.DataSetIndex][0]).ToString().Substring(0, (dtContactos.Rows[e.Item.DataSetIndex][0]).ToString().IndexOf('-') - 1).Trim();

				if(Request.QueryString["nit"]==null)
				{
					dtContactos.Rows[e.Item.DataSetIndex].Delete();
					dtContactos.AcceptChanges();
					this.Bind_Datas_dtContactos();
				}
				else
				{
					//Si es un contacto ya registrado y tiene un usuario asignado, borro el usuario del web.config
                    if (DBFunctions.NonQuery("UPDATE dbxschema.mcontacto SET test_codigo='I' WHERE mnit_nitcli='" + Request.QueryString["nit"] + "' AND mnit_nitcon='" + NIT + "'") == 1)
                    {
                        this.Eliminar_Usuario(dtContactos.Rows[e.Item.DataSetIndex][0].ToString());
                        dtContactos.Rows[e.Item.DataSetIndex].Delete();
                        dtContactos.AcceptChanges();
                        this.Bind_Datas_dtContactos();
                    }
                    else if (DBFunctions.NonQuery("UPDATE dbxschema.mcontacto SET test_codigo='I' WHERE mnit_nitcli='" + Request.QueryString["nit"] + "' AND mnit_nitcon='" + NIT + "'") == 0)
                    {
                        dtContactos.Rows[e.Item.DataSetIndex].Delete();
                        dtContactos.AcceptChanges();
                        this.Bind_Datas_dtContactos();
                    }
				}
			}
		}

		protected void dgContactos_EditCommand(object Sender,DataGridCommandEventArgs e)
		{
			dgContactos.EditItemIndex=e.Item.DataSetIndex;
			this.Bind_Datas_dtContactos();
		}
		
		protected void dgContactos_CancelCommand(object Sender,DataGridCommandEventArgs e)
		{
			dgContactos.EditItemIndex=-1;
			this.Bind_Datas_dtContactos();
		}

		protected void dgContactos_UpdateCommand(object Sender,DataGridCommandEventArgs e)
		{
			dtContactos.Rows[e.Item.DataSetIndex][1]=((TextBox)e.Item.Cells[1].FindControl("tbedsedcon")).Text;
			dtContactos.Rows[e.Item.DataSetIndex][2]=((TextBox)e.Item.Cells[2].FindControl("tbeddepcon")).Text;
			dtContactos.Rows[e.Item.DataSetIndex][3]=((TextBox)e.Item.Cells[3].FindControl("tbedcarcon")).Text;
			dgContactos.EditItemIndex=-1;
			this.Bind_Datas_dtContactos();
		}

		protected void lnbAnt_Command(object Sender,CommandEventArgs e)
		{
			if(e.CommandName=="pnlInfo")
			{
				pnlInfo.Visible=true;
				pnlContrato.Visible=false;
				pnlContactos.Visible=false;
				pnlProductos.Visible=false;
				lnbAnt.Enabled=false;//voy para
				lnbSig.CommandName="pnlContrato";//vengo de
			}
			else if(e.CommandName=="pnlContrato")
			{
				pnlInfo.Visible=false;
				pnlContrato.Visible=true;
				pnlContactos.Visible=false;
				pnlProductos.Visible=false;
				lnbAnt.CommandName="pnlInfo";//voy para
				lnbSig.CommandName="pnlContactos";//vengo de
				lnbSig.Enabled=true;
			}
			else if(e.CommandName=="pnlContactos")
			{
				pnlInfo.Visible=false;
				pnlContrato.Visible=false;
				pnlContactos.Visible=true;
				pnlProductos.Visible=false;
				lnbAnt.CommandName="pnlContrato";//voy para
				lnbSig.CommandName="pnlProductos";//vengo de
				lnbSig.Enabled=true;
			}
		}
		
		protected void lnbSig_Command(object Sender,CommandEventArgs e)
		{
			string err="";
			if(e.CommandName=="pnlContrato")
			{
				if(!Ejecutar_Validaciones_pnlInfo(ref err))
				{
					pnlInfo.Visible=false;
					pnlContrato.Visible=true;
					pnlContactos.Visible=false;
					pnlProductos.Visible=false;
					lnbSig.CommandName="pnlContactos";//voy para
					lnbAnt.CommandName="pnlInfo";// vengo de
					lnbAnt.Enabled=true;
				}
				else
                Utils.MostrarAlerta(Response, "" + err + "");
			}
			else if(e.CommandName=="pnlContactos")
			{
				if(!Ejecutar_Validaciones_pnlContrato(ref err))
				{
					pnlInfo.Visible=false;
					pnlContrato.Visible=false;
					pnlContactos.Visible=true;
					pnlProductos.Visible=false;
					lnbSig.CommandName="pnlProductos";//voy para
					lnbAnt.CommandName="pnlContrato";//vengo de
				}
				else
                    Utils.MostrarAlerta(Response, "" + err + "");
			}
			else if(e.CommandName=="pnlProductos")
			{
				if(!Ejecutar_Validaciones_pnlContactos(ref err))
				{
					pnlInfo.Visible=false;
					pnlContrato.Visible=false;
					pnlContactos.Visible=false;
					pnlProductos.Visible=true;
					lnbSig.CommandName="guardar";//voy para
					lnbAnt.CommandName="pnlContactos";//vengo de
				}
				else
                    Utils.MostrarAlerta(Response, "" + err + "");
			}
			else if(e.CommandName=="guardar")
			{
				if(!Ejecutar_Validaciones_pnlProductos(ref err))
				{
					lnbSig.Enabled=false;
					lnbAnt.Enabled=false;
					dgProductos.Enabled=false;
					Cambiar_dtProductos();
                    Utils.MostrarAlerta(Response, "Ahora puede guardar la información del cliente");
					btnGuardar.Visible=true;
				}
				else
                    Utils.MostrarAlerta(Response, "" + err + "");
			}
		}
		
		protected void btnGuardar_Click(object Sender,EventArgs e)
		{
			ArrayList sqls=new ArrayList();
			if(Request.QueryString["nit"]==null)
			{
				sqls.Add("INSERT INTO mclientesac VALUES('"+this.ddlnit.SelectedValue+"','"+this.ddltarifa.SelectedValue+"','"+this.tbmarcas.Text+"','"+this.tbnomger.Text+"','"+this.tbnomcon.Text+"','"+this.tbnomrev.Text+"','"+this.ddlvig.SelectedValue+"')");
				sqls.Add("INSERT INTO mcontratocliente VALUES(DEFAULT,'"+this.ddlnit.SelectedValue+"','"+this.tbfecini.Text+"','"+this.tbfecfin.Text+"',"+Convert.ToDouble(this.tbvalcon.Text)+","+this.tbnumvis.Text+")");
				Llenar_Arraylist_sqls(ref sqls);
				if(DBFunctions.Transaction(sqls))
					Response.Redirect(MainPage+"?process=Tools.AdministrarClientes");
					//lb.Text+="Bien "+DBFunctions.exceptions;
				else
					lb.Text+="Error "+DBFunctions.exceptions;
			}
			else
			{
				sqls.Add("UPDATE mclientesac SET ptar_codigo='"+this.ddltarifa.SelectedValue+"',mcli_marcas='"+this.tbmarcas.Text+"',mcli_gerente='"+this.tbnomger.Text+"',mcli_contador='"+this.tbnomcon.Text+"',mcli_revisor='"+this.tbnomrev.Text+"',tvig_codigo='"+this.ddlvig.SelectedValue+"' WHERE mnit_nit='"+Request.QueryString["nit"]+"'");
				sqls.Add("UPDATE mcontratocliente SET mcon_fechaini='"+this.tbfecini.Text+"',mcon_fechafin='"+this.tbfecfin.Text+"',mcon_valormens="+Convert.ToDouble(this.tbvalcon.Text)+",mcon_visitas="+this.tbnumvis.Text+" WHERE mcon_id="+DBFunctions.SingleData("SELECT mcon_id FROM mcontratocliente WHERE mnit_nit='"+Request.QueryString["nit"]+"'")+"");
				Llenar_Arraylist_sqls(ref sqls);
				if(DBFunctions.Transaction(sqls))
                    Response.Redirect(MainPage + "?process=Tools.AdministrarClientes&Mod=1&nomClie=" + ddlnit.SelectedItem.Text + "");
					//lb.Text+="Bien "+DBFunctions.exceptions;
				else
					lb.Text+="Error "+DBFunctions.exceptions;
			}
		}
		
		protected void btnCancelar_Click(object Sender,EventArgs e)
		{
			Response.Redirect(MainPage+"?process=Tools.AdministrarClientes");
		}
		
		protected void Establecer_Valores()
		{
			DatasToControls bind=new DatasToControls();
			DataSet ds=new DataSet();
			ds=DBFunctions.Request(ds,IncludeSchema.NO,"SELECT * FROM mclientesac WHERE mnit_nit='"+Request.QueryString["nit"]+"';"+
			                       "SELECT * FROM mcontratocliente WHERE mcon_id=(SELECT mcon_id FROM mcontratocliente WHERE mnit_nit='"+Request.QueryString["nit"]+"');"+
			                       "SELECT * FROM dclienteproductos WHERE mnit_nit='"+Request.QueryString["nit"]+"';"+
			                       "SELECT mc.mnit_nitcon CONCAT ' - ' CONCAT MN.mnit_nombres CONCAT ' ' CONCAT MN.mnit_nombre2 CONCAT ' ' " +
                                   "CONCAT MN.mnit_apellidos CONCAT ' ' CONCAT MN.mnit_apellido2 AS NIT, mc.mcon_sede AS SEDE, mc.mcon_departamento AS DEPTO, mc.mcon_cargo AS CARGO " +
                                   "FROM mcontacto mc, mnit mn WHERE  mc.mnit_nitcli='" + Request.QueryString["nit"] + "' AND test_codigo='A' AND mc.mnit_nitcon=mn.mnit_nit;");
            if (Request.QueryString["nit"]==null)
            {
                bind.PutDatasIntoDropDownList(ddlnit, "SELECT MNIT.mnit_nit,MNIT.mnit_nit CONCAT ' - ' CONCAT MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_nombre2 CONCAT ' ' CONCAT MNIT.mnit_apellidos CONCAT ' ' CONCAT MNIT.mnit_apellido2 FROM dbxschema.mnit MNIT,dbxschema.mclientesac MCLI WHERE MNIT.mnit_nit=MCLI.mnit_nit;");
                DatasToControls.EstablecerValueDropDownList(ddlnit, ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                bind.PutDatasIntoDropDownList(ddlnit, "SELECT MNIT.mnit_nit,MNIT.mnit_nit CONCAT ' - ' CONCAT MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_nombre2 CONCAT ' ' CONCAT MNIT.mnit_apellidos CONCAT ' ' CONCAT MNIT.mnit_apellido2 FROM dbxschema.mnit MNIT WHERE MNIT.mnit_nit=" + Request.QueryString["nit"].ToString() + ";");
            }
            ddlnit.Enabled=false;
			bind.PutDatasIntoDropDownList(ddltarifa,"SELECT ptar_codigo,ptar_descripcion FROM dbxschema.ptarifas");
			DatasToControls.EstablecerValueDropDownList(ddltarifa,ds.Tables[0].Rows[0][1].ToString());
			this.tbmarcas.Text=ds.Tables[0].Rows[0][2].ToString();
			this.tbnomger.Text=ds.Tables[0].Rows[0][3].ToString();
			this.tbnomcon.Text=ds.Tables[0].Rows[0][4].ToString();
			this.tbnomrev.Text=ds.Tables[0].Rows[0][5].ToString();
			bind.PutDatasIntoDropDownList(ddlvig,"SELECT tvig_codigo,tvig_nombre FROM dbxschema.tvigencia");
			DatasToControls.EstablecerValueDropDownList(ddlvig,ds.Tables[0].Rows[0][6].ToString());
			this.tbfecini.Text=Convert.ToDateTime(ds.Tables[1].Rows[0][2]).ToString("yyyy-MM-dd");
			this.tbfecfin.Text=Convert.ToDateTime(ds.Tables[1].Rows[0][3]).ToString("yyyy-MM-dd");
			this.tbvalcon.Text=Convert.ToDouble(ds.Tables[1].Rows[0][4]).ToString();
			this.tbnumvis.Text=ds.Tables[1].Rows[0][5].ToString();
			Llenar_dtProductos_Inicial();
			Poner_Chulos();
			this.Cargar_dtContactos();
			dtContactos=ds.Tables[3];
			Bind_Datas_dtContactos();
		}
		
		protected void Cargar_dtProductos()
		{
			dtProductos=new DataTable();
			dtProductos.Columns.Add("ID",typeof(string));
			dtProductos.Columns.Add("NOMBRE",typeof(string));
			dtProductos.Columns.Add("POSEE",typeof(bool));
		}
		
		protected void Cargar_dtContactos()
		{
			dtContactos=new DataTable();
			dtContactos.Columns.Add("NIT",typeof(string));
			dtContactos.Columns.Add("SEDE",typeof(string));
			dtContactos.Columns.Add("DEPTO",typeof(string));
			dtContactos.Columns.Add("CARGO",typeof(string));
		}
		
		protected void Bind_Datas_dtContactos()
		{
			dgContactos.DataSource=dtContactos;
			dgContactos.DataBind();
			Session["dtContactos"]=dtContactos;
		}
		
		protected void Llenar_dtProductos_Inicial()
		{
			DataSet ds=new DataSet();
			DataRow fila;
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT * FROM pproducto");
			if(dtProductos==null)
				Cargar_dtProductos();
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				fila=dtProductos.NewRow();
				fila[0]=ds.Tables[0].Rows[i][0].ToString();
				fila[1]=ds.Tables[0].Rows[i][1].ToString();
				fila[2]=false;
				dtProductos.Rows.Add(fila);
				dgProductos.DataSource=dtProductos;
				dgProductos.DataBind();
				Session["dtProductos"]=dtProductos;
			}
		}
		
		protected void Poner_Chulos()
		{
			DataSet ds=new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT ppro_id FROM dclienteproductos WHERE mnit_nit='"+Request.QueryString["nit"]+"'");
			for(int i=0;i<dtProductos.Rows.Count;i++)
			{
				for(int j=0;j<ds.Tables[0].Rows.Count;j++)
				{
					if(ds.Tables[0].Rows[j][0].ToString()==dtProductos.Rows[i][0].ToString())
						((CheckBox)dgProductos.Items[i].Cells[2].Controls[1]).Checked=true;
				}
			}
		}
		
		protected bool Verificar_dgProductos()
		{
			bool error=false;
			int cont=0;
			for(int i=0;i<dgProductos.Items.Count;i++)
			{
				if(!(((CheckBox)dgProductos.Items[i].Cells[2].Controls[1]).Checked))
					cont++;
			}
			if(cont==dgProductos.Items.Count)
				error=true;
			return error;
		}
		
		protected void Cambiar_dtProductos()
		{
			for(int i=0;i<dgProductos.Items.Count;i++)
			{
				if((((CheckBox)dgProductos.Items[i].Cells[2].Controls[1]).Checked))
				{
					dtProductos.Rows[i][2]=true;
					Session["dtProductos"]=dtProductos;
				}
				else
				{
					dtProductos.Rows[i][2]=false;
					Session["dtProductos"]=dtProductos;
				}
			}
		}
		
		protected bool Verificar_dtContactos()
		{
			bool error=false;
			if(dtContactos.Rows.Count==0)
				error=true;
			return error;
		}
		
		protected bool Verificar_dtContactos(string nit)
		{
			bool error=false;
			DataRow[] busqueda=dtContactos.Select("NIT='"+nit+"'");
			if(busqueda.Length!=0)
				error=true;
			return error;
		}
		
		protected bool Ejecutar_Validaciones_pnlInfo(ref string err)
		{
			bool error=false;
			if(Request.QueryString["nit"]==null)
			{
				if(ddltarifa.Items.Count==0)
				{
					error=true;
					err="No hay tarifas, por favor revise su configuración general";
				}
				else if(DBFunctions.RecordExist("SELECT * FROM mclientesac WHERE mnit_nit='"+this.ddlnit.SelectedValue+"'"))
				{
					error=true;
					err="Este nit ya es un cliente registrado";
				}
			}
			return error;
		}
		
		protected bool Ejecutar_Validaciones_pnlContrato(ref string err)
		{
			bool error=false;
            try
            {
                if (Convert.ToDateTime(tbfecini.Text) >= Convert.ToDateTime(tbfecfin.Text))
                {
                    error = true;
                    err = "La fecha de inicio del contrato no puede ser mayor o igual a la fecha de finalización";
                }
                else if (Convert.ToDateTime(tbfecini.Text) > DateTime.Now.Date)
                {
                    error = true;
                    err = "La fecha de inicio del contrato no puede ser mayor a la actual";
                }
            }
            catch
            {
                error = true;
                err = "La fecha de inicio del contrato no puede ser mayor o igual a la fecha de finalización";
            }
            return error;
		}
		
		protected bool Ejecutar_Validaciones_pnlContactos(ref string err)
		{
			bool error=false;
			if(Verificar_dtContactos())
			{
				error=true;
				err="No ha agregado contactos";
			}
			return error;
		}
		
		protected bool Ejecutar_Validaciones_pnlProductos(ref string err)
		{
			bool error=false;
			if(Verificar_dgProductos())
			{
				error=true;
				err="No ha escogido ningun producto";
			}
			return error;
		}
		
		protected void Llenar_Arraylist_sqls(ref ArrayList sqls)
		{
            //Si estoy ingresando un nuevo cliente
			if(Request.QueryString["nit"]==null)
			{
				for(int i=0;i<dtProductos.Rows.Count;i++)
				{
					if(Convert.ToBoolean(dtProductos.Rows[i][2]))
						sqls.Add("INSERT INTO dclienteproductos VALUES('"+ddlnit.SelectedValue+"','"+dtProductos.Rows[i][0].ToString()+"')");
				}

                for (int i = 0; i < dtContactos.Rows.Count; i++)
                {
                    string[] NitContacto;
                    try
                    {
                        NitContacto = dtContactos.Rows[i][0].ToString().Split('-');
                    }catch
                    {
                        NitContacto = new string[1];
                        NitContacto[0] = dtContactos.Rows[i][0].ToString().Trim();
                    }
                    string usu = HttpContext.Current.User.Identity.Name.ToLower();
                    string susuCodigo = DBFunctions.SingleData("select susu_codigo from susuario where SUSU_LOGIN='" + usu + "'");
                    sqls.Add("INSERT INTO mcontacto VALUES('" + ddlnit.SelectedValue + "','" + NitContacto[0].Trim() + "','" + dtContactos.Rows[i][1].ToString() + "','" + dtContactos.Rows[i][2].ToString() + "','" + dtContactos.Rows[i][3].ToString() + "'," + susuCodigo + ",'A')");
                }
			}
			//Si estoy modificando un cliente existente
			else
			{
				sqls.Add("DELETE FROM dclienteproductos WHERE mnit_nit='"+Request.QueryString["nit"]+"'");
				//sqls.Add("DELETE FROM mcontacto WHERE mnit_nitcli='"+Request.QueryString["nit"]+"' AND test_codigo='A'");
				for(int i=0;i<dtProductos.Rows.Count;i++)
				{
					if(Convert.ToBoolean(dtProductos.Rows[i][2]))
						sqls.Add("INSERT INTO dclienteproductos VALUES('"+Request.QueryString["nit"]+"','"+dtProductos.Rows[i][0].ToString()+"')");
				}
				for(int i=0;i<dtContactos.Rows.Count;i++)
				{
                    string usu = HttpContext.Current.User.Identity.Name.ToLower();
                    string susuCodigo = DBFunctions.SingleData("select susu_codigo from susuario where SUSU_LOGIN='" + usu + "'");
                    //string susuCodigo = DBFunctions.SingleData("select susu_codigo from susuario where mnit_nit='" + dtContactos.Rows[i][0].ToString().Substring(0, dtContactos.Rows[i][0].ToString().IndexOf('-') - 1).Trim() + "'");
                    if (!DBFunctions.RecordExist("SELECT * FROM mcontacto WHERE mnit_nitcon='" + dtContactos.Rows[i][0].ToString().Substring(0, dtContactos.Rows[i][0].ToString().IndexOf('-') - 1).Trim() + "' AND mnit_nitcli ='" + Request.QueryString["nit"] + "'"))
						sqls.Add("INSERT INTO mcontacto VALUES('"+Request.QueryString["nit"]+"','"+dtContactos.Rows[i][0].ToString().Substring(0, dtContactos.Rows[i][0].ToString().IndexOf('-') - 1).Trim()+"','"+dtContactos.Rows[i][1].ToString()+"','"+dtContactos.Rows[i][2].ToString()+"','"+dtContactos.Rows[i][3].ToString()+"'," + susuCodigo + ",'A')");
					else
                        sqls.Add("UPDATE mcontacto SET mcon_sede='" + dtContactos.Rows[i][1].ToString() + "',mcon_departamento='" + dtContactos.Rows[i][2].ToString() + "',mcon_cargo='" + dtContactos.Rows[i][3].ToString() + "',test_codigo='A' WHERE mnit_nitcli='" + Request.QueryString["nit"] + "' AND mnit_nitcon='" + dtContactos.Rows[i][0].ToString().Substring(0, dtContactos.Rows[i][0].ToString().IndexOf('-') - 1).Trim() + "'");
				}
			}
		}

		protected void Eliminar_Usuario(string contacto)
		{
			//Este procedimiento me sirve para eliminar el usuario enlazado a un contacto cuando se elimina 
			//dicho contacto, si el contacto no tiene enlazadas solicitudes lo puedo eliminar sin problema,
			//si tiene solicitudes, solo lo dejo en estado inactivo
			bool encontrado = false;
			XmlNode credenciales = (autorizacion.ChildNodes[0]).ChildNodes[1];
			XmlNode credencialUsuario = webConfig.DocumentElement;
			string usuario=DBFunctions.SingleData("SELECT susu_login FROM susuario WHERE susu_codigo=(SELECT susu_codigo FROM mcontacto where mnit_nitcli='"+Request.QueryString["nit"]+"' AND mnit_nitcon='"+contacto+"')");
			if(usuario!="")
			{
				foreach(XmlNode oCurrentNode in credenciales.ChildNodes)
				{
					if(oCurrentNode.Attributes[0].Value.ToString() == usuario)
					{
						credencialUsuario = oCurrentNode;
						encontrado = true;
						break;
					}
				}
				if(encontrado)
				{
					if(DBFunctions.RecordExist("SELECT * FROM msolicitud WHERE mnit_nitcli='"+Request.QueryString["nit"]+"' AND mnit_nitcon='"+contacto+"'"))
						DBFunctions.NonQuery("UPDATE susuario SET test_codigo='I' WHERE susu_login='"+usuario+"'");		
					else
						DBFunctions.NonQuery("DELETE FROM susuario WHERE susu_login='"+usuario+"'");		
					credenciales.RemoveChild(credencialUsuario);
					webConfig.Save(mainPath+"Web.Config");
                    Utils.MostrarAlerta(Response, "Usuario Eliminado Con Exito");
				}
			}
			else
            Utils.MostrarAlerta(Response, "Este contacto no tenia un usuario asignado. Aun así la eliminación ha sido exitosa");
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
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
