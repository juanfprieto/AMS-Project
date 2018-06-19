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
	public partial class ManejoVarios : System.Web.UI.UserControl
	{
		protected Button aceptar,cancelar,btnCargar,btnCargarPed;
		protected DropDownList prefijoPedido,numeroPedido;
		protected Control padre,encabezado,datosCliente,pagos,noCausados,documentos;
		protected DataTable tablaAbonos,tablaPost,tablaAbonosDev;
		protected ArrayList pagosDev=new ArrayList();
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			padre=(this.Parent).Parent;
			encabezado=((PlaceHolder)padre.FindControl("phEncabezado")).Controls[0];
			//datosCliente=((PlaceHolder)padre.FindControl("phDatosCliente")).Controls[0];
			pagos=((PlaceHolder)padre.FindControl("phPagos")).Controls[0];
			noCausados=((PlaceHolder)padre.FindControl("phNoCausados")).Controls[0];
			documentos=((PlaceHolder)padre.FindControl("phDocumentos")).Controls[0];
			if(!IsPostBack)
			{
				
			}
			else
			{
				if(Session["tablaAbonos"]!=null)
					tablaAbonos=(DataTable)Session["tablaAbonos"];
				if(Session["tablaPost"]!=null)
					tablaPost=(DataTable)Session["tablaPost"];
				if(Session["tablaAbonosDev"]!=null)
					tablaAbonosDev=(DataTable)Session["tablaAbonosDev"];
				if(Session["pagosDev"]!=null)
					pagosDev=(ArrayList)Session["pagosDev"];
			}
		}
		
		protected void cargar_tablaAbonos()
		{
			tablaAbonos=new DataTable();
			tablaAbonos.Columns.Add("PREFIJO",typeof(string));
			tablaAbonos.Columns.Add("NUMERO",typeof(string));
			tablaAbonos.Columns.Add("VALORPEDIDO",typeof(double));
			tablaAbonos.Columns.Add("VALORABONADO",typeof(double));
			tablaAbonos.Columns.Add("ABONO",typeof(double));
			tablaAbonos.Columns.Add("ESTADO",typeof(bool));
            tablaAbonos.Columns.Add("RETOMAS", typeof(double));
        }
		
		protected void cargar_tablaPost()
		{
			tablaPost=new DataTable();
			tablaPost.Columns.Add("TIPO",typeof(string));
			tablaPost.Columns.Add("CODIGOBANCO",typeof(string));
			tablaPost.Columns.Add("NUMERODOCUMENTO",typeof(string));
			tablaPost.Columns.Add("TIPOMONEDA",typeof(string));
			tablaPost.Columns.Add("VALOR",typeof(double));
			tablaPost.Columns.Add("VALORTC",typeof(double));
			tablaPost.Columns.Add("FECHA",typeof(string));
			tablaPost.Columns.Add("ESTADO",typeof(string));
			tablaPost.Columns.Add("FECPRO",typeof(string));
			tablaPost.Columns.Add("INTERES",typeof(double));
			tablaPost.Columns.Add("PRORROGA",typeof(bool));
		}
		
		protected void cargar_tablaAbonosDev()
		{
			tablaAbonosDev=new DataTable();
			tablaAbonosDev.Columns.Add("PREFIJO",typeof(string));
			tablaAbonosDev.Columns.Add("NUMERO",typeof(string));
			tablaAbonosDev.Columns.Add("VALOR",typeof(double));
			tablaAbonosDev.Columns.Add("ABONADO",typeof(double));
			tablaAbonosDev.Columns.Add("ESTADO",typeof(bool));
            tablaAbonosDev.Columns.Add("RETOMAS", typeof(double));
        }
		
		protected void prefijoPedido_IndexChanged(object Sender,EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(numeroPedido,"SELECT mped_numepedi FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedido.SelectedValue+"' AND test_tipoesta IN(10,20)");
		}

        protected void CargarPanel(object Sender, EventArgs e)
        {
            Utils.MostrarAlerta(Response, "Realice la relación de pagos");
            ((PlaceHolder)padre.FindControl("phNoCausados")).Visible = true;
            ((ImageButton)padre.FindControl("btnNoCausados")).ImageUrl = "../img/AMS.BotonContraer.png";
            ((ImageButton)padre.FindControl("btnNoCausados")).Enabled = true;
            Esconder_Controles();
		}

		protected void Esconder_Controles()
		{
			((PlaceHolder)padre.FindControl("phVarios")).Visible=false;
			((ImageButton)padre.FindControl("btnVarios")).ImageUrl="../img/AMS.BotonExpandir.png";
		}
		
		public void Llenar_GridAnticipos()
		{
			DataSet ds;
			DataRow fila;
			ds=new DataSet();
			double valorItems=0;
			//double porcentajeIva = 0;
			//DBFunctions.Request(ds,IncludeSchema.NO,"SELECT M.pdoc_codigo,M.mped_numepedi,M.mped_valounit- m.pmed_valodesc, CASE WHEN (SELECT SUM(mant_valorecicaja) FROM manticipovehiculo WHERE mped_codigo='"+prefijoPedido.SelectedValue+"' AND mped_numepedi="+numeroPedido.SelectedValue+") IS NULL THEN 0 ELSE (SELECT SUM(mant_valorecicaja) FROM manticipovehiculo WHERE mped_codigo='"+prefijoPedido.SelectedValue+"' AND mped_numepedi="+numeroPedido.SelectedValue+") END FROM mpedidovehiculo M WHERE M.test_tipoesta IN(10,20) AND M.pdoc_codigo='"+prefijoPedido.SelectedValue+"' AND M.mped_numepedi="+numeroPedido.SelectedValue+"");
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT M.pdoc_codigo,M.mped_numepedi,M.mped_valounit - m.mped_valodesc,M.pcat_codigo FROM dbxschema.mpedidovehiculo M WHERE M.test_tipoesta IN(10,20) AND (M.mnit_nit IN ('"+((TextBox)encabezado.FindControl("datCli")).Text+"','"+((TextBox)encabezado.FindControl("datBen")).Text+"') OR M.mnit_nit2 IN ('"+((TextBox)encabezado.FindControl("datCli")).Text+"','"+((TextBox)encabezado.FindControl("datBen")).Text+"'))");
			if(ds.Tables[0].Rows.Count!=0)
			{
				if(Session["tablaAbonos"]==null)
					this.cargar_tablaAbonos();
				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					DataSet ds1=new DataSet();
					DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT dped_valoitem,piva_porciva FROM dpedidovehiculo WHERE pdoc_codigo='"+ds.Tables[0].Rows[i][0].ToString()+"' AND mped_numepedi="+ds.Tables[0].Rows[i][1].ToString()+"");
					for(int j=0;j<ds1.Tables[0].Rows.Count;j++)
						valorItems=valorItems+(Convert.ToDouble(ds1.Tables[0].Rows[j][0])+(Convert.ToDouble(ds1.Tables[0].Rows[j][0])*Convert.ToDouble(ds1.Tables[0].Rows[j][1])/100));
					fila=tablaAbonos.NewRow();
					fila[0]=ds.Tables[0].Rows[i][0].ToString();//Prefijo
					fila[1]=ds.Tables[0].Rows[i][1].ToString();//Numero
					fila[2]=Convert.ToDouble(ds.Tables[0].Rows[i][2].ToString())+valorItems;//Valor
					fila[3]=DBFunctions.SingleData("SELECT CASE WHEN SUM(mant_valorecicaja) IS NULL THEN 0 ELSE SUM(mant_valorecicaja) END FROM dbxschema.manticipovehiculo WHERE mped_codigo='"+ds.Tables[0].Rows[i][0].ToString()+"' AND mped_numepedi="+ds.Tables[0].Rows[i][1].ToString()+" AND test_estado NOT IN (40)");//Anticipos
                    //fila[4] = 8.9;
                    //           if (ds.Tables[0].Rows.Count > 1)
					    //fila[4] = 0;//Faltante
         //           else
         //               fila[4] = Convert.ToDouble(fila[2]) - Convert.ToDouble(fila[3]);//Faltante
                    fila[4] = Convert.ToDouble(fila[2]) - Convert.ToDouble(fila[3]);//Faltante
					fila[5]=false;
                    try
                    {
                        fila[6] = DBFunctions.SingleData("select DPED_VALORECI from DPEDIDOVEHICULORETOMA WHERE PDOC_CODIGO = '" + ds.Tables[0].Rows[i][0].ToString() + "' AND MPED_NUMEPEDI = " + ds.Tables[0].Rows[i][1].ToString());
                    }catch(Exception z)
                    {
                        fila[6] = 0.0;
                    }
                    

                    tablaAbonos.Rows.Add(fila);
					gridAbonos.DataSource=tablaAbonos;
					gridAbonos.DataBind();
					Session["tablaAbonos"]=tablaAbonos;
                    valorItems = 0;
				}
			}
			else
                Utils.MostrarAlerta(Response, "No hay pedidos para estos clientes");
		}
		
		protected void gridAbonos_Edit(object Sender,DataGridCommandEventArgs e)
		{
			gridAbonos.EditItemIndex=e.Item.DataSetIndex;
			gridAbonos.DataSource=tablaAbonos;
			gridAbonos.DataBind();
			Session["tablaAbonos"]=tablaAbonos;
		}
		
		protected void gridAbonos_Update(object Sender,DataGridCommandEventArgs e)
		{
			if(!DatasToControls.ValidarDouble(((TextBox)e.Item.FindControl("tbabono")).Text))
                Utils.MostrarAlerta(Response, "Valor Invalido, si digito el signo pesos, quitelo");
            //else if(DatasToControls.ValidarDouble(((TextBox)e.Item.FindControl("tbabono")).Text) && Convert.ToDouble(((TextBox)e.Item.FindControl("tbabono")).Text)==0)
            //    Utils.MostrarAlerta(Response, "No se pueden abonar 0 pesos");
			else if(Convert.ToDouble(((TextBox)e.Item.FindControl("tbabono")).Text)>(Convert.ToDouble(tablaAbonos.Rows[e.Item.DataSetIndex][2])-Convert.ToDouble(tablaAbonos.Rows[e.Item.DataSetIndex][3])))
                Utils.MostrarAlerta(Response, "El valor abonado es mayor al saldo faltante, revise sus datos");
			else
			{
				tablaAbonos.Rows[e.Item.DataSetIndex][4]=Convert.ToDouble(((TextBox)e.Item.FindControl("tbabono")).Text);
				tablaAbonos.Rows[e.Item.DataSetIndex][5]=true;

                double valores = 0;
                for (int k = 0; k < tablaAbonos.Rows.Count; k++ )
                {
                    valores += Convert.ToDouble(tablaAbonos.Rows[k][4]);
                }

                ((Label)pagos.FindControl("lbInfo")).Text = "Total Anticipos a Vehículos : " + valores.ToString("C");
                //((Label)pagos.FindControl("lbInfo")).Text = "Total Anticipos a Vehículos : " + Convert.ToDouble(tablaAbonos.Rows[e.Item.DataSetIndex][4]).ToString("C");
                //Utils.MostrarAlerta(Response, "Realice la relación de pagos");
                //this.Esconder_Controles();
                //((PlaceHolder)padre.FindControl("phNoCausados")).Visible = true;
                //((ImageButton)padre.FindControl("btnNoCausados")).ImageUrl = "../img/AMS.BotonContraer.png";
                //((ImageButton)padre.FindControl("btnNoCausados")).Enabled = true;

                //((PlaceHolder)padre.FindControl("phPagos")).Visible=true;
                //((ImageButton)padre.FindControl("btnPagos")).ImageUrl="../img/AMS.BotonContraer.png";
                //((ImageButton)padre.FindControl("btnPagos")).Enabled=true;
				gridAbonos.EditItemIndex=-1;
				gridAbonos.DataSource=tablaAbonos;
				gridAbonos.DataBind();
				Session["tablaAbonos"]=tablaAbonos;
                //codigo opcional Session["tablaPagos"]
                //Session.Remove("tablaPagos");
			}
		}
		
		protected void gridAbonos_Cancel(object Sender,DataGridCommandEventArgs e)
		{
			gridAbonos.EditItemIndex=-1;
			gridAbonos.DataSource=tablaAbonos;
			gridAbonos.DataBind();
			Session["tablaAbonos"]=tablaAbonos;
		}
		
		public void Llenar_GrillaProrrogas()
		{
			DataSet ds;
			DataRow fila;
			ds=new DataSet();
			int fec_max=Convert.ToInt32(DBFunctions.SingleData("SELECT ccar_maxprorrogas FROM ccartera"));
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT T.ttip_nombre,P.pban_codigo,P.mcpag_numerodoc,P.mcpag_tipomoneda,P.mcpag_valor/P.mcpag_valortasacambio,P.mcpag_valortasacambio,P.mcpag_fecha,G.test_nombre FROM mcajapago P,mcaja M,pdocumento D,ttipopago T,testadocajapago G WHERE M.pdoc_codigo=P.pdoc_codigo AND M.mcaj_numero=P.mcaj_numero AND M.pdoc_codigo=D.pdoc_codigo AND P.ttip_codigo=T.ttip_codigo AND P.test_estado=G.test_estado AND (M.mnit_nit IN('"+((TextBox)encabezado.FindControl("datCli")).Text+"','"+((TextBox)encabezado.FindControl("datBen")).Text+"') OR M.mnit_nitben IN('"+((TextBox)encabezado.FindControl("datCli")).Text+"','"+((TextBox)encabezado.FindControl("datBen")).Text+"')) AND P.ttip_codigo='C' AND P.test_estado='C' AND D.tdoc_tipodocu NOT IN('CE') AND M.test_estadodoc NOT IN('N','C') AND dbxschema.add_months(P.mcpag_fecha,"+fec_max+")>CURRENT DATE");
			if(ds.Tables[0].Rows.Count!=0)
			{
				if(Session["tablaPost"]==null)
					this.cargar_tablaPost();
				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					fila=tablaPost.NewRow();
					fila[0]=ds.Tables[0].Rows[i][0].ToString();
					fila[1]=ds.Tables[0].Rows[i][1].ToString();
					fila[2]=ds.Tables[0].Rows[i][2].ToString();
					fila[3]=ds.Tables[0].Rows[i][3].ToString();
					fila[4]=Convert.ToDouble(ds.Tables[0].Rows[i][4].ToString());
					fila[5]=Convert.ToDouble(ds.Tables[0].Rows[i][5].ToString());
					fila[6]=Convert.ToDateTime(ds.Tables[0].Rows[i][6].ToString()).ToString("yyyy-MM-dd");
					fila[7]=ds.Tables[0].Rows[i][7].ToString();
					fila[9]=0;
					fila[10]=false;
					tablaPost.Rows.Add(fila);
					gridPost.DataSource=tablaPost;
					gridPost.DataBind();
					Session["tablaPost"]=tablaPost;
				}
			}
			else
                Utils.MostrarAlerta(Response, "No existen cheques");
		}
		
		protected void gridPost_Update(object Sender,DataGridCommandEventArgs e)
		{
			DateTime fecha=Convert.ToDateTime(tablaPost.Rows[e.Item.DataSetIndex][6].ToString());
			int fec_max=Convert.ToInt32(DBFunctions.SingleData("SELECT ccar_maxprorrogas FROM ccartera"));
			if(!DatasToControls.ValidarDateTime(((TextBox)e.Item.FindControl("tbfec")).Text))
                Utils.MostrarAlerta(Response, "Fecha de Prorroga Invalida");
			else if(Convert.ToDateTime(((TextBox)e.Item.FindControl("tbfec")).Text)>=((fecha.AddMonths(fec_max))))
                Utils.MostrarAlerta(Response, "La Fecha de Prorroga excede "+fec_max+" meses. Revise");
			else if(!DatasToControls.ValidarDouble(((TextBox)e.Item.FindControl("tbint")).Text))
			{
                Utils.MostrarAlerta(Response, "Valor de Intereses Inválido");
				((TextBox)e.Item.FindControl("tbint")).Text="0";
			}
			else
			{
				tablaPost.Rows[e.Item.DataSetIndex][8]=((TextBox)e.Item.FindControl("tbfec")).Text;
				tablaPost.Rows[e.Item.DataSetIndex][9]=Convert.ToDouble(((TextBox)e.Item.FindControl("tbint")).Text);
				tablaPost.Rows[e.Item.DataSetIndex][10]=true;
				gridPost.EditItemIndex=-1;
				gridPost.DataSource=tablaPost;
				gridPost.DataBind();
				Session["tablaPost"]=tablaPost;
				btnAceptar.Enabled=true;
				lbProrroga.Text=(Sumar_Prorrogas()).ToString("C");
			}
		}
		
		protected void gridPost_Cancel(object Sender,DataGridCommandEventArgs e)
		{
			tablaPost.Rows[e.Item.DataSetIndex][8]="";
			tablaPost.Rows[e.Item.DataSetIndex][9]=0;
			tablaPost.Rows[e.Item.DataSetIndex][10]=false;
			gridPost.DataSource=tablaPost;
			gridPost.EditItemIndex=-1;
			gridPost.DataBind();
			Session["tablaPost"]=tablaPost;
		}
		
		protected void gridPost_Edit(object Sender,DataGridCommandEventArgs e)
		{
			gridPost.EditItemIndex=e.Item.DataSetIndex;
			gridPost.DataSource=tablaPost;
			gridPost.DataBind();
			Session["tablaPost"]=tablaPost;
		}
		
		protected double Sumar_Prorrogas()
		{
			double valor=0;
			for(int i=0;i<tablaPost.Rows.Count;i++)
			{
				if(Convert.ToBoolean(tablaPost.Rows[i][10]))
				{
					valor=valor+Convert.ToDouble(tablaPost.Rows[i][9].ToString());
				}
			}
			return valor;
		}
		
		protected double Restar_Prorrogas()
		{
			double valor=0;
			for(int i=0;i<tablaPost.Rows.Count;i++)
			{
				if(!Convert.ToBoolean(tablaPost.Rows[i][10]))
					valor=valor-Convert.ToDouble(tablaPost.Rows[i][9].ToString());
			}
			return valor;
		}
		
		protected bool Verificar_tablaPost()
		{
			bool estado=false;
			int cont=0;
			for(int i=0;i<tablaPost.Rows.Count;i++)
			{
				if(!Convert.ToBoolean(tablaPost.Rows[i][10]))
					cont++;
			}
			if(cont==tablaPost.Rows.Count)
				estado=true;
			return estado;
		}
		
		protected void btnAceptar_Click(object Sender,EventArgs e)
		{
			/*Aqui la idea es la siguiente, resulta que puede haber un proceso de prorroga en el cual no se cobra ningun tipo
			  de interes por la prorroga. En ese caso, creo un pago por cero pesos y guardo de una vez. Sino, lo mando a la
			  tabla de pagos y realizo el proceso normal*/
			if(lbProrroga.Text=="$0.00" && Verificar_tablaPost())
                Utils.MostrarAlerta(Response, "No hay prorrogas, revise sus datos.");
			else if(lbProrroga.Text=="$0.00" && !Verificar_tablaPost())
			{
				DataRow fila;
				DataTable pago=new DataTable();
				if(Session["tablaPagos"]!=null)
				{
					pago=(DataTable)Session["tablaPagos"];
					fila=pago.NewRow();
					fila[0]="E";
					fila[3]="Nacional";
					fila[4]=0;
					fila[5]=1;
					fila[6]=0;
					fila[7]=DateTime.Now.ToString("yyyy-MM-dd");
					fila[8]="C";
					pago.Rows.Add(fila);
					Session["tablaPagos"]=pago;
					((Button)padre.FindControl("guardar")).Enabled=true;
                    Utils.MostrarAlerta(Response, "Ahora puede guardar su recibo");
					btnAceptar.Enabled=false;
					this.Esconder_Controles();
				}
			}
			else if(lbProrroga.Text!="$0.00")
			{
				this.Esconder_Controles();
				((PlaceHolder)padre.FindControl("phPagos")).Visible=true;
				((ImageButton)padre.FindControl("btnPagos")).ImageUrl="../img/AMS.BotonContraer.png";
				((ImageButton)padre.FindControl("btnPagos")).Enabled=true;
				((Label)pagos.FindControl("lbInfo")).Text="Total Intereses por Prorrogas : "+lbProrroga.Text;
			}
		}
		
		public void Cargar_GrillaPedidosDevueltos()
		{
			DataSet ds=new DataSet();
			DataRow fila;
            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT M.pdoc_codigo,M.mped_numepedi,sum(M.mped_valounit - m.mped_valodesc), SUM(V.mant_valorecicaja) FROM mpedidovehiculo M,manticipovehiculo V WHERE M.pdoc_codigo=V.mped_codigo AND M.mped_numepedi=V.mped_numepedi AND (M.mnit_nit IN('" + ((TextBox)encabezado.FindControl("datCli")).Text + "','" + ((TextBox)encabezado.FindControl("datBen")).Text + "')) AND M.test_tipoesta IN (10) and v.test_estado not in (40) GROUP BY M.pdoc_codigo,M.mped_numepedi ");
			if(ds.Tables[0].Rows.Count!=0)
			{
				if(Session["tablaAbonosDev"]==null)
					this.cargar_tablaAbonosDev();
				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					fila=tablaAbonosDev.NewRow();
					fila[0]=ds.Tables[0].Rows[i][0].ToString();
					fila[1]=ds.Tables[0].Rows[i][1].ToString();
					fila[2]=Convert.ToDouble(ds.Tables[0].Rows[i][2]);
					fila[3]=Convert.ToDouble(ds.Tables[0].Rows[i][3]);
					fila[4]=false;
					tablaAbonosDev.Rows.Add(fila);
				}
				gridAbonosDev.DataSource=tablaAbonosDev;
				gridAbonosDev.DataBind();
				Session["tablaAbonosDev"]=tablaAbonosDev;
			}
			else
                Utils.MostrarAlerta(Response, "Los pedidos de este cliente ya se encuentran asignados o facturados \\n o no tienen abonos, por lo tanto es imposible la devolución");
		}	
		
		protected bool Verificar_grillaAbonosDev()
		{
			bool exito=false;
			int cont=0;
			for(int i=0;i<gridAbonosDev.Items.Count;i++)
			{
				if(((CheckBox)gridAbonosDev.Items[i].Cells[4].Controls[1]).Checked==false)
					cont++;
			}
			if(cont==gridAbonosDev.Items.Count)
				exito=true;
			return exito;
		}
		
		protected void btnDevolver_Click(object Sender,EventArgs e)
		{
			//Aqui la idea es la siguiente, me devuelven el pedido que obligatoriamente solo puede estar creado, ese pedido
			//tiene uno anticipos, los cuales se realizaron con RC saco el valor de cada RC  y lo mando a dcajadevolucionpedido
			//tomo el valor q esta en el textbox de la grilla y por ese valor se genera el egreso, si dicho valor es menor a la
			//sumatoria de todos los RC de ese pedido, genero un registro en dcajafaltantedevolucionpedido
			DataSet ds=new DataSet();
			double valorDevolverPedido=0,totalDevolverPedido=0;
			//Ahora verificamos que por lo menos un pedido ha sido seleccionado para devolver
			if(!Verificar_grillaAbonosDev())
			{
				//Recorremos la tabla de abonos devueltos y miramos si la casilla de verificacion esta activada, si lo esta
				//sacamos todos los pagos q tengan los anticipos a ese abono
				for(int i=0;i<gridAbonosDev.Items.Count;i++)
				{
					if(((CheckBox)gridAbonosDev.Items[i].Cells[4].Controls[1]).Checked)
					{
						if(DatasToControls.ValidarDouble(((TextBox)gridAbonosDev.Items[i].Cells[3].FindControl("tbdev")).Text))
						{
                            //Los valores devueltos deben ser == y no >=
							if(Convert.ToDouble(((TextBox)gridAbonosDev.Items[i].Cells[3].FindControl("tbdev")).Text) == Convert.ToDouble(tablaAbonosDev.Rows[i][3]))
							{
								valorDevolverPedido=Convert.ToDouble(((TextBox)gridAbonosDev.Items[i].Cells[3].FindControl("tbdev")).Text);
								//totalDevolverPedido=Convert.ToDouble(tablaAbonosDev.Rows[i][3]);
                                totalDevolverPedido += valorDevolverPedido;
								pagosDev.Add("INSERT INTO dcajadevolucionpedido VALUES(default,'@1',@2,'"+tablaAbonosDev.Rows[i][0].ToString()+"',"+tablaAbonosDev.Rows[i][1].ToString()+","+Convert.ToDouble(((TextBox)gridAbonosDev.Items[i].Cells[3].FindControl("tbdev")).Text)+")");
                                pagosDev.Add("UPDATE MANTICIPOVEHICULO SET TEST_ESTADO = 40 WHERE PDOC_CODIGO = '@1' AND MCAJ_NUMERO = @2 AND MPED_CODIGO = '" + tablaAbonosDev.Rows[i][0].ToString() + "' AND MPED_NUMEPEDI = " + tablaAbonosDev.Rows[i][1].ToString() + ";");
                                Session["pagosDev"] = pagosDev;
								tablaAbonosDev.Rows[i][4]=true;
								
                                //pagosDev.Add("INSERT INTO dcajafaltantedevolucionpedido VALUES('@1',@2,'"+tablaAbonosDev.Rows[i][0].ToString()+"',"+tablaAbonosDev.Rows[i][1].ToString()+","+(totalDevolverPedido-valorDevolverPedido).ToString()+")");
                                //Session["pagosDev"]=pagosDev;
							}
							else
							{
                                Utils.MostrarAlerta(Response, "El valor a devolver es diferente al valor abonado.'");
								return;
							}
						}
						else
						{
                            Utils.MostrarAlerta(Response, "Digite un valor por favor");
							return;
						}
					}
				}
				Session["tablaAbonosDev"] = tablaAbonosDev;
				this.Esconder_Controles();
				((PlaceHolder)padre.FindControl("phPagos")).Visible=true;
				((ImageButton)padre.FindControl("btnPagos")).ImageUrl="../img/AMS.BotonContraer.png";
				((ImageButton)padre.FindControl("btnPagos")).Enabled=true;
				//((Label)pagos.FindControl("lbInfo")).Text="Total Devoluciones Abonos : "+totalDevolverPedido.ToString("C");
                ((Label)pagos.FindControl("lbInfo")).Text = "Total Devoluciones Abonos : " + totalDevolverPedido.ToString("C");
			}
			else
                Utils.MostrarAlerta(Response, "Debe escoger por lo menos un pedido");
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
