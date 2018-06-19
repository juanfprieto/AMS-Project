namespace AMS.Garantias
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.Forms;
	using AMS.DB;
	using AMS.DBManager;
	using System.Configuration;
	using System.Collections;
    using AMS.Documentos;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Garantias_Otorgamiento.
	/// </summary>
	public partial class AMS_Garantias_Otorgamiento : System.Web.UI.UserControl
	{
		#region ATRIBUTOS

		protected DataTable dtRepuestos, dtOperaciones;
		protected DataSet cgarantia;
		#endregion
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				Session.Clear();
				if(Request.QueryString["exito"]!=null)
                Utils.MostrarAlerta(Response, "GARANTIA VALIDA ");
				lbCodcat.Text=Request.QueryString["catalogo"];
				DataSet cgarantia = new DataSet();
				DBFunctions.Request(cgarantia,IncludeSchema.NO,"select * from cgarantia");
				lbGrupo.Text = DBFunctions.SingleData("select pgru_grupo from pcatalogovehiculo where pcat_codigo = '"+lbCodcat.Text+"'").ToString();
				lbNoncat.Text= DBFunctions.SingleData("select pcat_descripcion  from pcatalogovehiculo where pcat_codigo = '"+lbCodcat.Text+"'").ToString();
				dtRepuestos = new DataTable();
				dtOperaciones= new DataTable();
				dtOperaciones.Columns.Add("ptem_operacion");
				dtOperaciones.Columns.Add("ptem_descripcion");
				dtOperaciones.Columns.Add("Valor",typeof(double));
				dtOperaciones.Columns.Add("exeivaOper");
				dtOperaciones.Columns.Add("ivaOper");
				dtOperaciones.Columns.Add("cubresn");
				dtOperaciones.Columns.Add("tipoliq");

				dtRepuestos.Columns.Add("Codigo");
				dtRepuestos.Columns.Add("ITEM");
				dtRepuestos.Columns.Add("ValorIt");
				dtRepuestos.Columns.Add("linea");
				dtRepuestos.Columns.Add("cantidad");
				dtRepuestos.Columns.Add("cubresino");

				dgItems.DataSource= dtRepuestos;
				dgOpers.DataSource= dtOperaciones;
				dgItems.DataBind();
				dgOpers.DataBind();
				DatasToControls.Aplicar_Formato_Grilla(dgItems);
				DatasToControls.Aplicar_Formato_Grilla(dgOpers);
				Session["dtRepuestos"]= dtRepuestos;
				Session["dtOperaciones"]= dtOperaciones;
				Session["cgarantia"]= cgarantia;
		
			}
			else 
			{	dtRepuestos=(DataTable)Session["dtRepuestos"];	
				dtOperaciones=(DataTable)Session["dtOperaciones"];
				cgarantia= (DataSet)Session["cgarantia"];
				cargarValores();
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
		///		Método necesario para admitir el Diseñador. No se puede modificar
		///		el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			this.dgItems.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgItems_ItemCommand);
			this.dgItems.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgItems_ItemDataBound);
			this.dgOpers.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgOpers_ItemCommand);
			this.dgOpers.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgOpers_ItemDataBound);

		}
		#endregion

		#region EVENTOS

		//Agrega  **ITEMS** a la grilla individualmente y remueve items no deseados
		private void dgItems_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{bool ban = true; 
			DataTable dt = new DataTable();
			dt.Columns.Add("Codigo");
			dt.Columns.Add("ITEM");
			dt.Columns.Add("ValorIt");
			dt.Columns.Add("linea");
			dt.Columns.Add("cantidad");
			dt.Columns.Add("cubresino");
			DataRow fila;
			fila=dt.NewRow();
			if(e.CommandName== "AgregarItem")
			{
				if(((TextBox)e.Item.Cells[0].FindControl("tbCodItem")).Text != "")
				{
					fila["Codigo"]=((TextBox)e.Item.Cells[0].FindControl("tbCodItem")).Text;
					fila["ITEM"]=((TextBox)e.Item.Cells[1].FindControl("tbCodItema")).Text ;
					fila["linea"]= ((DropDownList)e.Item.Cells[3].FindControl("ddlLineas")).SelectedValue;
					dt.Rows.Add(fila);
					//cargarValores();
					cargarRepuestos(dt);
				}
			}

			else  if(e.CommandName== "BorrarItem")
			{
				//cargarValores();
				dtRepuestos.Rows[e.Item.ItemIndex].Delete();
				dtRepuestos.AcceptChanges();
				dgItems.DataSource=dtRepuestos;
				dgItems.DataBind();
				for (int i = 0; i< dgItems.Items.Count ; i++)
				{
					if (dtRepuestos.Rows[i][5].ToString()  == "No")
					{  dgItems.Items[i].Cells[5].BackColor = Color.Orange;	
							e1.Visible= true;
						ban = false;
					}
				}
				if (ban)
				{
					lbParcial.Text ="";	e1.Visible= false;
				}
				Session["dtRepuestos"]=dtRepuestos;
			}
		}

		//Agrega **OPERACIONES** a la grilla individualmente y remueve items no deseados
		private void dgOpers_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{bool ban = true; 
			DataTable dt = new DataTable();
			dt.Columns.Add("ptem_operacion");
			dt.Columns.Add("ptem_descripcion");
			dt.Columns.Add("Valor",typeof(double));
			dt.Columns.Add("exeivaOper");
			dt.Columns.Add("ivaOper");
			dt.Columns.Add("cubresn");
			dt.Columns.Add("tipoliq");

			DataRow fila;
			fila=dt.NewRow();
			if(e.CommandName== "AgregarOper")
			{
				if(((TextBox)e.Item.Cells[0].FindControl("tbCodOper")).Text != "")
				{
					fila["ptem_operacion"]=((TextBox)e.Item.Cells[0].FindControl("tbCodOper")).Text ;
					fila["ptem_descripcion"]=((TextBox)e.Item.Cells[1].FindControl("tbCodOpera")).Text ;
					fila["exeivaOper"] = Convert.ToString(DBFunctions.SingleData("select  TRES_SINO from ptempario inner join trespuestasino on ptem_exceiva = TRES_SINO where ptem_operacion= '"+fila["ptem_operacion"].ToString()+"'"));	
					if (fila["exeivaOper"].ToString()=="N") fila["ivaOper"]=DBFunctions.SingleData("select cemp_porciva  from cempresa ");
					else 	fila["ivaOper"]= "0";	
					dt.Rows.Add(fila);
					cargarOpers(dt);
				}			
			}
			else  if(e.CommandName== "BorrarOper")
			{
				//cargarValores();
				dtOperaciones.Rows[e.Item.ItemIndex].Delete();
				dtOperaciones.AcceptChanges();
				dgOpers.DataSource=dtOperaciones;
				dgOpers.DataBind();

				for (int i = 0; i< dgOpers.Items.Count ; i++)
				{
					if (dtOperaciones.Rows[i][5].ToString() == "N")
					{
						ban= false;	
						e2.Visible= true;
						dgOpers.Items[i].Cells[0].BackColor= Color.Orange;
					}
				}
				if (ban)
				{
					lbParcial2.Text =""; 	
					e2.Visible= false;
				}
				Session["dtOperaciones"]=dtOperaciones;
			}
		}

		//carga la linea y la cantidad de los repuestos
		private void dgItems_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if (e.Item.ItemType== ListItemType.Footer)
			{	DatasToControls bind = new DatasToControls();		
				bind.PutDatasIntoDropDownList(((DropDownList)e.Item.FindControl("ddlLineas")), "SELECT plin_codigo CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem");//Linea de Bodega
				((TextBox)e.Item.FindControl("tbCodItem")).Attributes.Add("ondblclick","MostrarRefs("+((TextBox)e.Item.FindControl("tbCodItem")).ClientID+","+((DropDownList)e.Item.FindControl("ddlLineas")).ClientID+");");
				((TextBox)e.Item.FindControl("tbCodItem")).Attributes.Add("onkeyup","ItemMask("+((TextBox)e.Item.FindControl("tbCodItem")).ClientID+","+((DropDownList)e.Item.FindControl("ddlLineas")).ClientID+");");
				((DropDownList)e.Item.FindControl("ddlLineas")).Attributes.Add("onchange","ChangeLine("+((DropDownList)e.Item.FindControl("ddlLineas")).ClientID+","+((TextBox)e.Item.FindControl("tbCodItem")).ClientID+");");
			}
		}
		private void dgOpers_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if (e.Item.ItemType== ListItemType.Item ||e.Item.ItemType== ListItemType.AlternatingItem)
			{
				if (dtOperaciones.Rows[e.Item.DataSetIndex][6].ToString() == "B")
					((TextBox)e.Item.Cells[2].FindControl("tbValoper")).Visible= true;

				if (dtOperaciones.Rows[e.Item.DataSetIndex][3].ToString() == "N") 
				e.Item.Cells[3].Text= "No";
				else e.Item.Cells[3].Text= "Si";
			}
		}

		protected void btGuardar_Click(object sender, System.EventArgs e)
		{
			ArrayList query = new ArrayList();
			string usuario=HttpContext.Current.User.Identity.Name;
			string nitdealer  =Convert.ToString( DBFunctions.SingleData("Select mnit_nit  from DBXSCHEMA.SUSUARIO where susu_login='"+usuario+"'"));
			//query.Add("insert into mgarantia  values ('"+Request.QueryString("prefijo")+"','"+Request.QueryString("numero")+"','"+nitdealer+"','"+Request.QueryString("nitclient")+"', '"+DateTime.Now.ToString("yyyy-MM-dd")+"', '"+lbCodcat.Text+"', '"+Request.QueryString("vin")+"',1,default, ) ");
		}

		#endregion

		#region METODOS
		//agrega operaciones sin repetir a la grilla de operaciones
		private void cargarOpers(DataTable oper)
		{ 	DataRow[] rep  = null;
					for (int i = 0 ; i< oper.Rows.Count; i++)
					{	
						rep= dtOperaciones.Select("ptem_operacion='"+oper.Rows[i][0].ToString()+"'");
						if (rep.Length == 0)
						{
							DataRow fila;
							fila=dtOperaciones.NewRow();
							fila["ptem_operacion"]=oper.Rows[i][0].ToString();
							fila["ptem_descripcion"]=oper.Rows[i][1].ToString();
							fila["exeivaOper"]= oper.Rows[i][3].ToString();
							fila["ivaOper"]= oper.Rows[i][4].ToString();
							fila["cubresn"]= "S";
							string tipoLiquidacion = DBFunctions.SingleData("SELECT ttip_codiliqu FROM ptempario WHERE ptem_operacion='"+oper.Rows[i][0].ToString()+"'");
							 fila["tipoliq"]= tipoLiquidacion;
											double porcentajeIva = Convert.ToDouble(DBFunctions.SingleData("SELECT cemp_porciva FROM cempresa"));
											double valorOperacion = 0,valorHora=0,tiempoGarantia=0;
											bool nogarantia = false;
		
											if(tipoLiquidacion=="F")
												valorOperacion = Convert.ToDouble(DBFunctions.SingleData("SELECT ptem_valooper FROM ptempario WHERE ptem_operacion='"+oper.Rows[i][0].ToString()+"'"));
											else if(tipoLiquidacion=="T")
											{
													valorHora=Convert.ToDouble(DBFunctions.SingleData("SELECT ppreta_valohoragtia FROM ppreciotaller WHERE ppreta_codigo='"+cgarantia.Tables[0].Rows[0][4]+"'"));
												if (valorHora==0)
													nogarantia = true;
												try
												{
													 tiempoGarantia=Convert.ToDouble(DBFunctions.SingleData("SELECT ptie_tiemgara FROM ptiempotaller WHERE ptie_grupcata='"+lbGrupo.Text+"' AND ptie_tempario='"+oper.Rows[i][0].ToString()+"'"));
													if (tiempoGarantia==0) nogarantia = true;
														if (nogarantia)
													{
														tiempoGarantia=Convert.ToDouble(DBFunctions.SingleData("SELECT ptie_tiemclie FROM ptiempotaller WHERE ptie_grupcata='"+lbGrupo.Text+"' AND ptie_tempario='"+oper.Rows[i][0].ToString()+"'"));			
														valorHora=Convert.ToDouble(DBFunctions.SingleData("SELECT ppreta_valohoraclie FROM ppreciotaller WHERE ppreta_codigo='"+cgarantia.Tables[0].Rows[0][4]+"'"));
													}
												}
												catch
												{
													if(Convert.ToDouble(DBFunctions.SingleData("SELECT ptem_tiempoestandar FROM dbxschema.ptempario WHERE ptem_operacion='"+oper.Rows[i][0].ToString()+"'"))==0)
														tiempoGarantia=1;
													else
														tiempoGarantia=Convert.ToDouble(DBFunctions.SingleData("SELECT ptem_tiempoestandar FROM dbxschema.ptempario WHERE ptem_operacion='"+oper.Rows[i][0].ToString()+"'"));
												}
												
												//valorOperacion = (Convert.ToDouble(DBFunctions.SingleData("SELECT ppreta_valohoraclie FROM ppreciotaller WHERE ppreta_codigo='"+listaPrecios+"'")))*(Convert.ToDouble(DBFunctions.SingleData("SELECT ptie_tiemclie FROM ptiempotaller WHERE ptie_grupcata='"+grupoCatalogo+"' AND ptie_tempario='"+((TextBox)e.Item.Cells[1].Controls[1]).Text+"'")));
												valorOperacion=valorHora*tiempoGarantia;
											}
											if(tipoLiquidacion!="B")
											{
												if(oper.Rows[i][3].ToString() =="N")
													valorOperacion = valorOperacion + (valorOperacion*(porcentajeIva/100));				
											}
								fila["Valor"] =  valorOperacion;

							dtOperaciones.Rows.Add(fila);
						}
					}
					Session["dtOperaciones"] = dtOperaciones;
					dgOpers.DataSource = dtOperaciones;
					DatasToControls.Aplicar_Formato_Grilla(dgOpers);
					dgOpers.DataBind();		
					bool ban= true;

			for (int i = 0; i< dgOpers.Items.Count ; i++)
			{double tiempoGarantia=0,valorHora=0;
				try 
				{
					tiempoGarantia =Convert.ToDouble(DBFunctions.SingleData("SELECT ptie_tiemgara FROM ptiempotaller WHERE ptie_grupcata='"+lbGrupo.Text+"' AND ptie_tempario='"+oper.Rows[i][0].ToString()+"'"));
					valorHora=Convert.ToDouble(DBFunctions.SingleData("SELECT ppreta_valohoragtia FROM ppreciotaller WHERE ppreta_codigo='"+cgarantia.Tables[0].Rows[0][4]+"'"));
				}
	
				catch
				{
					dgOpers.Items[i].Cells[0].BackColor= Color.Orange;				
					lbParcial2.Text = "Esta solicitud contiene Operaciónes que no son cubiertas por garantía; En caso de ser aprobada estas Operaciónes correran por cuenta del cliente.";
					dtOperaciones.Rows[i][5]= "N";
				}
				if (tiempoGarantia  == 0 || valorHora== 0 ) 
				{
					dgOpers.Items[i].Cells[0].BackColor= Color.Orange;				
					lbParcial2.Text = "Esta solicitud contiene Operaciónes que no son cubiertas por garantía; En caso de ser aprobada estas Operaciónes correran por cuenta del cliente.";
					dtOperaciones.Rows[i][5]= "N";
					e2.Visible= true;
					ban= false;
				}
				if (ban)
					lbParcial2.Text ="";
			}
		}

		//lo mismo de cargarOpers pero con los repuestos
		private void cargarRepuestos (DataTable Repuestos)
		{ string str = "";
			bool ban = true;
				DataRow[] rep  = null;
				for (int i = 0 ; i< Repuestos.Rows.Count; i++)
				{	
					rep= dtRepuestos.Select("Codigo='"+Repuestos.Rows[i][0].ToString()+"'");
					if (rep.Length == 0)
					{
						DataRow fila;
						fila=dtRepuestos.NewRow();
						fila["Codigo"]=Repuestos.Rows[i][0].ToString();
						fila["ITEM"]=Repuestos.Rows[i][1].ToString();
						fila["linea"]=Repuestos.Rows[i][3].ToString();
						Referencias.Guardar(Repuestos.Rows[i][0].ToString(), ref str, DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='"+(Repuestos.Rows[i][3].ToString().Split('-'))[0]+"'"));
						fila["cantidad"]= 1;
						fila["cubresino"]= repuestosCubreSN(str);
						fila["ValorIt"]= DBFunctions.SingleData("select mpre_precio from mprecioitem where mite_codigo = '"+str+"' and ppre_codigo = '"+cgarantia.Tables[0].Rows[0][5]+"' ");
						dtRepuestos.Rows.Add(fila);
					}
				}
				Session["dtRepuestos"] = dtRepuestos;
				dgItems.DataSource = dtRepuestos;
				DatasToControls.Aplicar_Formato_Grilla(dgItems);
				dgItems.DataBind();
				for (int i = 0; i< dgItems.Items.Count ; i++)
				{
					if (dtRepuestos.Rows[i][5].ToString()  == "No")
					{
						dgItems.Items[i].Cells[5].BackColor = Color.Orange;		
						lbParcial.Text = "Esta solicitud contiene repuestos que no son cubiertos por garantía; En caso de ser aprobada estos repuestos correran por cuenta del cliente.";
						e1.Visible= true;
						ban= false;
					}
					if (ban)
						lbParcial.Text ="";
				}
		}

		//llena y guarda los Valor unitario de los repuestos y operaciones
		private void cargarValores()
		{
			for (int i =0 ; i< dtOperaciones.Rows.Count ; i++)
			{
				if (((TextBox)dgOpers.Items[i].Cells[2].FindControl("tbValoper")).Text == ""&& ((TextBox)dgOpers.Items[i].Cells[2].FindControl("tbValoper")).Visible==true)
					((TextBox)dgOpers.Items[i].Cells[2].FindControl("tbValoper")).Text= "0";
				dtOperaciones.Rows[i][2]= 	((TextBox)dgOpers.Items[i].Cells[2].FindControl("tbValoper")).Text;
		
			}
			for (int i =0 ; i< dtRepuestos.Rows.Count ; i++)
			{
				if(((TextBox)dgItems.Items[i].Cells[2].FindControl("tbCantItem")).Text == "")
					((TextBox)dgItems.Items[i].Cells[2].FindControl("tbCantItem")).Text="0";
				dtRepuestos.Rows[i][4]=((TextBox)dgItems.Items[i].Cells[2].FindControl("tbCantItem")).Text; 
			}
		}

		private string  repuestosCubreSN(string str){
			string siono = DBFunctions.SingleData("select mite_cubresino from mitemgarantia where mite_codigo = '"+str+"'").ToString();
			if(siono == "N")
				return  "No";
			else if (siono == "") return "Si";
			else 
			{
				double tolerancia = Convert.ToDouble ( DBFunctions.SingleData("Select cgar_kilotoler  from CGARANTIA"));//porsentaje de tolerancia de kilometraje para otorgar la garantia
				tolerancia = (tolerancia/100)+1;
				tolerancia =Convert.ToDouble(DBFunctions.SingleData("select mite_kilom from mitemgarantia where mite_codigo = '"+str+"'"))* tolerancia;
				if (Convert.ToDouble(Request.QueryString["kilometraje"]) <= tolerancia)
					return "Si";
				else return "No";				
			}
		
		}

#endregion

		protected void dgItems_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}
		

	
	}
}
