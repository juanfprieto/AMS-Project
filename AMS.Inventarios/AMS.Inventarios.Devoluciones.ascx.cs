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
using AMS.Documentos;
using AMS.Contabilidad;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial class Devoluciones : System.Web.UI.UserControl
	{
		protected ArrayList types = new ArrayList();
		protected ArrayList lbFields = new ArrayList();
    	protected DataTable dtSource;
		protected DataSet ds;
		protected string Tipo;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		private FormatosDocumentos formatoFactura=new FormatosDocumentos();
        ProceHecEco contaOnline = new ProceHecEco();
        protected double  valorflete = 0, valorivaflete = 0;
        double valorNeto = 0, valorIva = 0;
        double numeroItems = 0;

        //LOAD--------------------------------------------------------
        protected void Page_Load(object sender, System.EventArgs e)
		{
            Ajax.Utility.RegisterTypeForAjax(typeof(Devoluciones));
            Tipo = Request.QueryString["actor"]; //Miramos si es una devolucion de cliente o de proveedor
			this.ClearChildViewState();
			if(Tipo=="P")//nits de proveedores
                txtNIT.Attributes["ondblclick"] = "ModalDialog(this, 'Select t1.mnit_nit as NIT, t1.mnit_nombres concat \\' \\' concat t1.mnit_apellidos as Nombre from MNIT as t1,MPROVEEDOR as t2 where t1.mnit_nit=t2.mnit_nit order by t1.mnit_nit', new Array());";
			else//nits de clientes
				txtNIT.Attributes["ondblclick"] = "ModalDialog(this, 'SELECT mnit_nit as NIT, mnit_nombres concat \\' \\' concat mnit_apellidos as Nombre FROM mnit order by mnit_nit', new Array());";
			if(!IsPostBack)
			{
				Session.Clear();
                btnAjus.Attributes.Add("onclick","javascript:" + 
                  btnAjus.ClientID + ".disabled=true;" + 
                  this.Page.GetPostBackEventReference(btnAjus));
				if(Request.QueryString["prefN"]!=null &&Request.QueryString["numN"]!=null)
				{
					if(Request.QueryString["actor"]=="C")
					{
						formatoFactura=new FormatosDocumentos();
                        Utils.MostrarAlerta(Response, "Se ha generado la devolución con prefijo " + Request.QueryString["prefN"] + " y número " + Request.QueryString["numN"] + "");
						try
						{
							formatoFactura.Prefijo=Request.QueryString["prefN"];
							formatoFactura.Numero=Convert.ToInt32(Request.QueryString["numN"]);
							formatoFactura.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["prefN"]+"'");
							if(formatoFactura.Codigo!=string.Empty)
							{
								if(formatoFactura.Cargar_Formato())
									Response.Write("<script language:javascript>w=window.open('"+formatoFactura.Documento+"','','HEIGHT=600,WIDTH=800');</script>");
							}
                            contaOnline.contabilizarOnline(formatoFactura.Prefijo.ToString(), Convert.ToInt32(formatoFactura.Numero.ToString()), DateTime.Now,"");
 						}
						catch
						{
							lbInfo.Text="Error al generar el formato. Detalles : <br>"+formatoFactura.Mensajes;
						}
					}
					else if(Request.QueryString["actor"]=="P")
					{
						formatoFactura=new FormatosDocumentos();
                        Utils.MostrarAlerta(Response, "Se ha generado la devolución con prefijo " + Request.QueryString["prefN"] + " y número " + Request.QueryString["numN"] + "");
						try
						{
							formatoFactura.Prefijo=Request.QueryString["prefN"];
							formatoFactura.Numero=Convert.ToInt32(Request.QueryString["numN"]);
							formatoFactura.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["prefN"]+"'");
							if(formatoFactura.Codigo!=string.Empty)
							{
								if(formatoFactura.Cargar_Formato())
									Response.Write("<script language:javascript>w=window.open('"+formatoFactura.Documento+"','','HEIGHT=600,WIDTH=800');</script>");
							}
                            contaOnline.contabilizarOnline(formatoFactura.Prefijo.ToString(), Convert.ToInt32(formatoFactura.Numero.ToString()), DateTime.Now, "");
						}
						catch
						{
							lbInfo.Text="Error al generar el formato. Detalles : <br>"+formatoFactura.Mensajes;
						}
					}
				}
				DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(ddlAlmacen, "SELECT palm_almacen, palm_descripcion FROM palmacen WHERE PCEN_CENTINV IS NOT NULL and tvig_vigencia='V' ORDER BY PALM_DESCRIPCION");
                if(ddlAlmacen.Items.Count > 1)
                    ddlAlmacen.Items.Insert(0,"Seleccione:..");
                else
                {
                    if (ddlAlmacen.Items.Count == 1)
                        bind.PutDatasIntoDropDownList(ddlVendedor, "SELECT pven_codigo, pven_nombre FROM pvendedor WHERE tvend_codigo in ('VM','TT') AND PVEN_VIGENCIA = 'V' ORDER BY PVEN_NOMBRE");
                    else
                    {
                        Utils.MostrarAlerta(Response, "NO hay almacenes parametrizados para manejar Inventarios");
                        Page.Visible = false;
                    }
                }
                IFormatProvider culture = new System.Globalization.CultureInfo("es-CO", true);
				tbDate.Text = DateTime.Now.GetDateTimeFormats()[6];
				calDate.SelectedDate=DateTime.Now;
			}
			LoadDataColumns();
			LoadDataTable();
			if((DataTable)Session["dtInsertsDC"]!=null)
   				dtSource = (DataTable)Session["dtInsertsDC"];
			else
				Session["dtInsertsDC"] = dtSource;
			if(ddlFact.Items.Count == 0)
				plhFact.Visible = false;
		}
	
		//VALIDAR DATOS DE FACTURA, NIT, etc.------------------------------------------------------------
		protected bool ValidarDatos()
		{
			return(true);
		}
		
		//CARGAR FACTURA------------------------------	
		protected void CambiaFact(Object Sender, EventArgs E)
		{
			string preF = "",numF = "";

			string sa = ddlFact.SelectedValue.ToString();               //La factura que va a ser devuelta
			preF = sa.Substring(0,sa.LastIndexOf("-")).Trim();          //prefijo factura
			numF = sa.ToString().Substring(sa.LastIndexOf("-")+1).Trim();//numero factura
			ds   = new DataSet();
			if(Tipo=="C")
			{
				// Manejo de devoluciones de clientes
				// En ordenes de trabajo NO SE PERMITE devolución de items a FACTURAS de CARGO FACTURADO
                // de presentarse devolucion de items a factura con cargo facturado de orden de trabajo, se debe hacer nota administrativa y ajuste al inventario
//                string sqlDev = @"SELECT DBXSCHEMA.EDITARREFERENCIAS(codigo,plin_tipo),codigo, mite_nombre, dite_valounit, plin_codigo,dite_porcdesc ,piva_porciva, DITE_PREFDOCUREFE, DITE_NUMEDOCUREFE, SALIDA, DEVUELTA  
//                       FROM (SELECT DISTINCT DIT.mite_codigo AS codigo, PLIN.plin_tipo, DIT.mite_codigo, MIT.mite_nombre, DIT.dite_valounit, MIT.plin_codigo, DIT.dite_porcdesc,  
                //SELECT ORIGINAL 
//                        DIT.piva_porciva, DIT.DITE_PREFDOCUREFE, DIT.DITE_NUMEDOCUREFE, DIT.DITE_CANTIDAD as salida, COALESCE(SUM(DITD.DITE_CANTIDAD),0) AS devuelta  
//                       FROM dbxschema.mitems AS MIT, plineaitem AS PLIN, dbxschema.ditems AS DIT  
//                        LEFT JOIN dbxschema.ditems AS DITd on DIT.pdoc_codigo=DITd.DITE_prefdocuREFE AND DIT.dite_numedocu=DITd.dite_numedocuREFE AND DITd.Mite_CODIGO=DIT.MITE_codigo AND DITd.tmov_tipomovi IN (61,81,91)  
//                       WHERE DIT.mite_codigo=MIT.mite_codigo AND DIT.pdoc_codigo='" + preF + @"' AND DIT.dite_numedocu=" + numF + @" AND MIT.plin_codigo=PLIN.plin_codigo
//                         AND DIT.PDOC_CODIGO || DIT.dite_numedocu NOT IN (
//                                SELECT PDOC_FACTURA|| MFAC_NUMERO FROM mordenTRANSFERENCIA MT, MFACTURACLIENTETALLER MF
//                                where pdoc_FACTURA = DIT.pdoc_codigo AND MFAC_NUMERO = DIT.dite_numedocu AND MT.PDOC_CODIGO = MF.PDOC_PREFORDETRAB AND MT.MORD_NUMEORDE = MF.MORD_NUMEORDE
//                                AND MF.TCAR_CARGO = MT.TCAR_CARGO)
//                        GROUP BY PLIN.plin_tipo, DIT.mite_codigo, MIT.mite_nombre, DIT.dite_valounit, MIT.plin_codigo, DIT.dite_porcdesc,  
//                        DIT.piva_porciva, DIT.DITE_PREFDOCUREFE, DIT.DITE_NUMEDOCUREFE, DIT.DITE_CANTIDAD
//                        ) REGISTROS ORDER BY plin_codigo,codigo; ";

                string sqlDev = @"SELECT DBXSCHEMA.EDITARREFERENCIAS (codigo,plin_tipo),codigo, mite_nombre,dite_valounit,plin_codigo, dite_porcdesc, piva_porciva,DITE_PREFDOCUREFE,DITE_NUMEDOCUREFE,SALIDA,DEVUELTA, 0 AS FLETE, 0 AS IVAFLETE
                                    FROM (SELECT DISTINCT DIT.mite_codigo AS codigo,PLIN.plin_tipo,DIT.mite_codigo,MIT.mite_nombre, DIT.dite_valounit,MIT.plin_codigo, DIT.dite_porcdesc,DIT.piva_porciva, DIT.DITE_PREFDOCUREFE,DIT.DITE_NUMEDOCUREFE,
                                    DIT.DITE_CANTIDAD AS salida,COALESCE(SUM(DITD.DITE_CANTIDAD),0) AS devuelta
                                    FROM dbxschema.mitems AS MIT,plineaitem AS PLIN, dbxschema.ditems AS DIT
                                    LEFT JOIN dbxschema.ditems AS DITd ON DIT.pdoc_codigo = DITd.DITE_prefdocuREFE AND DIT.dite_numedocu = DITd.dite_numedocuREFE AND DITd.Mite_CODIGO = DIT.MITE_codigo AND DITd.tmov_tipomovi IN (61,81,91)
                                    WHERE DIT.mite_codigo = MIT.mite_codigo AND   DIT.pdoc_codigo = '" + preF + @"'  AND   DIT.dite_numedocu = " + numF + @" AND   MIT.plin_codigo = PLIN.plin_codigo 
                                    AND   DIT.PDOC_CODIGO || DIT.dite_numedocu NOT IN (SELECT PDOC_FACTURA|| MFAC_NUMERO
                                            FROM  mordenTRANSFERENCIA MT, MFACTURACLIENTETALLER MF
                                            WHERE pdoc_FACTURA = DIT.pdoc_codigo AND   MFAC_NUMERO = DIT.dite_numedocu AND   MT.PDOC_CODIGO = MF.PDOC_PREFORDETRAB AND   MT.MORD_NUMEORDE = MF.MORD_NUMEORDE AND   MF.TCAR_CARGO = MT.TCAR_CARGO)
                                    GROUP BY PLIN.plin_tipo,DIT.mite_codigo, MIT.mite_nombre, DIT.dite_valounit,MIT.plin_codigo,DIT.dite_porcdesc, DIT.piva_porciva, DIT.DITE_PREFDOCUREFE,
                                             DIT.DITE_NUMEDOCUREFE,DIT.DITE_CANTIDAD
                                    ) REGISTROS
                                    ORDER BY plin_codigo, codigo;";
             
                DBFunctions.Request(ds, IncludeSchema.NO, sqlDev);

          		int n;
				dtSource.Rows.Clear();
				for(n=0;n<ds.Tables[0].Rows.Count;n++)
				{
					double cantidadSalida = 0, cantidadDevuelta = 0 ;
                    cantidadSalida   = Convert.ToDouble(ds.Tables[0].Rows[n][9].ToString());
                    cantidadDevuelta = Convert.ToDouble(ds.Tables[0].Rows[n][10].ToString());
                    if ((cantidadSalida - cantidadDevuelta) > 0)
					{
						DataRow fila = dtSource.NewRow();
						fila[0] = ds.Tables[0].Rows[n][0].ToString();
						fila[1] = ds.Tables[0].Rows[n][2].ToString();
                        fila[2] = cantidadSalida - cantidadDevuelta;
                        fila[3] = cantidadSalida - cantidadDevuelta;
						fila[4] = Convert.ToDouble(ds.Tables[0].Rows[n][3]);    // valor unitario
						fila[5] = Convert.ToDouble(ds.Tables[0].Rows[n][5]);    // % descuento
						fila[6] = Convert.ToDouble(ds.Tables[0].Rows[n][6]);    // % iva
						fila[7] = Total(Convert.ToDouble(ds.Tables[0].Rows[n][3]), cantidadSalida - cantidadDevuelta, Convert.ToDouble(ds.Tables[0].Rows[n][5]), Convert.ToDouble(ds.Tables[0].Rows[n][6]));
						fila[8] = ds.Tables[0].Rows[n][4].ToString();
						fila[9] = 0;
                        fila[10] = ds.Tables[0].Rows[n]["dite_prefdocurefe"].ToString();
                        fila[11] = ds.Tables[0].Rows[n]["dite_numedocurefe"].ToString();
                        fila[12] = cantidadSalida - cantidadDevuelta; ;
						dtSource.Rows.Add(fila);
                        fila[13] = ds.Tables[0].Rows[n][11].ToString();
                        fila[14] = ds.Tables[0].Rows[n][12].ToString();
					}
				}
				if(dtSource.Rows.Count==0)
                    Utils.MostrarAlerta(Response, "La factura NO tiene items o ya ha sido devuelta o el cargo YA está facturado !");
				//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			}
			else if(Tipo=="P")
			{
                //string slqDev = "SELECT ITEM, MITE_CODIGO, MITE_NOMBRE, DITE_VALOUNIT, PLIN_CODIGO, DITE_PORCDESC, PIVA_PORCIVA, PDOC_CODIGO, DITE_NUMEDOCU, SUM(CANTIDAD) AS ENTRADA, SUM(DEVUELTA) AS DEVUELTA " +
                //                " FROM ( SELECT DBXSCHEMA.EDITARREFERENCIAS(DIT.mite_codigo,PLIN.plin_tipo) AS ITEM ,DIT.mite_codigo, MIT.mite_nombre, DIT.dite_valounit, MIT.plin_codigo, " +
                //                "  DIT.dite_porcdesc ,DIT.piva_porciva, DIT.PDOC_CODIGO, DIT.DITE_NUMEDOCU, sum(DIT.DITE_CANTIDAD) as cantidad, 0 as devuelta " +
                //                " FROM dbxschema.mitems AS MIT, plineaitem AS PLIN, dbxschema.ditems AS DIT " +
                //                " WHERE DIT.mite_codigo=MIT.mite_codigo AND DIT.pdoc_codigo = '" + preF + "' AND DIT.dite_numedocu = " + numF + " AND PLIN.plin_codigo=MIT.plin_codigo AND DIT.tmov_tipomovi IN (11,30) " +
                //                " group by DIT.mite_codigo,PLIN.plin_tipo,MIT.mite_nombre, DIT.dite_valounit, MIT.plin_codigo, DIT.dite_porcdesc ,DIT.piva_porciva, DIT.PDOC_CODIGO, DIT.DITE_NUMEDOCU " +
                //                "union " +
                //                " SELECT DBXSCHEMA.EDITARREFERENCIAS(DIT.mite_codigo,PLIN.plin_tipo),DIT.mite_codigo, MIT.mite_nombre, DIT.dite_valounit, MIT.plin_codigo, " +
                //                "  DIT.dite_porcdesc ,DIT.piva_porciva, DIT.DITE_PREFDOCUREFE, DIT.DITE_NUMEDOCUREFE, 0 as cantidad, SUM(DIT.DITE_CANTIDAD) as devuelta " +
                //                " FROM dbxschema.mitems AS MIT, plineaitem AS PLIN, dbxschema.ditems AS DIT " +
                //                " WHERE DIT.mite_codigo=MIT.mite_codigo AND DIT.DITE_PREFDOCUREFE = '" + preF + "' AND DIT.dite_numedocuREFE = " + numF + " AND PLIN.plin_codigo=MIT.plin_codigo AND DIT.tmov_tipomovi=31  " +
                //                " group by DIT.mite_codigo,PLIN.plin_tipo,MIT.mite_nombre, DIT.dite_valounit, MIT.plin_codigo, DIT.dite_porcdesc ,DIT.piva_porciva, DIT.DITE_PREFDOCUREFE, DIT.DITE_NUMEDOCUREFE ) " +
                //                "GROUP BY ITEM, MITE_CODIGO, MITE_NOMBRE, DITE_VALOUNIT, PLIN_CODIGO, DITE_PORCDESC, PIVA_PORCIVA, PDOC_CODIGO, DITE_NUMEDOCU " +
                //                "ORDER BY plin_codigo, mite_codigo; ";

                string slqDev = @"SELECT ITEM, MITE_CODIGO,MITE_NOMBRE,DITE_VALOUNIT,PLIN_CODIGO,DITE_PORCDESC,PIVA_PORCIVA,PDOC_CODIGO,DITE_NUMEDOCU,SUM(CANTIDAD) AS ENTRADA, SUM(DEVUELTA) AS DEVUELTA,FLETE,IVAFLETE
                                    FROM (SELECT DBXSCHEMA.EDITARREFERENCIAS (DIT.mite_codigo,PLIN.plin_tipo) AS ITEM,DIT.mite_codigo, MIT.mite_nombre,DIT.dite_valounit,MIT.plin_codigo,DIT.dite_porcdesc,DIT.piva_porciva,DIT.PDOC_CODIGO,DIT.DITE_NUMEDOCU,
                                    0 AS FLETE, 0 AS IVAFLETE,SUM(DIT.DITE_CANTIDAD) AS cantidad, 0 AS devuelta
                                    FROM dbxschema.mitems AS MIT,
                                         dbxschema.plineaitem AS PLIN,
                                         dbxschema.ditems AS DIT,
                                         dbxschema.MFACTURAPROVEEDOR AS MFP
                                    WHERE DIT.mite_codigo = MIT.mite_codigo AND   DIT.pdoc_codigo = '" + preF + @"'    AND   DIT.dite_numedocu = '" + numF + @"' AND   PLIN.plin_codigo = MIT.plin_codigo 
                                      AND DIT.tmov_tipomovi IN (11,30)
                                      AND MFP.PDOC_CODIORDEPAGO =  DIT.pdoc_codigo AND   MFP.MFAC_NUMEORDEPAGO = DIT.dite_numedocu
                                    GROUP BY DIT.mite_codigo,PLIN.plin_tipo,MIT.mite_nombre, DIT.dite_valounit,MIT.plin_codigo,DIT.dite_porcdesc,DIT.piva_porciva,DIT.PDOC_CODIGO, DIT.DITE_NUMEDOCU

                                    UNION
                                    
                                    SELECT DBXSCHEMA.EDITARREFERENCIAS (DIT.mite_codigo,PLIN.plin_tipo),DIT.mite_codigo, MIT.mite_nombre, DIT.dite_valounit, MIT.plin_codigo,DIT.dite_porcdesc,DIT.piva_porciva,DIT.DITE_PREFDOCUREFE, DIT.DITE_NUMEDOCUREFE,
                                    MFP.MFAC_VALOFLET AS FLETE,MFP.MFAC_VALOIVAFLET AS IVAFLETE, 0 AS cantidad, SUM(DIT.DITE_CANTIDAD) AS devuelta
                                    FROM dbxschema.mitems AS MIT, 
                                         dbxschema.plineaitem AS PLIN,
                                         dbxschema.MFACTURAPROVEEDOR AS MFP, 
                                         dbxschema.ditems AS DIT
                                    WHERE DIT.mite_codigo = MIT.mite_codigo AND   DIT.DITE_PREFDOCUREFE = '" + preF + @"' AND   DIT.dite_numedocuREFE = '" + numF + @"' AND   PLIN.plin_codigo = MIT.plin_codigo AND   MFP.PDOC_CODIORDEPAGO =  DIT.pdoc_codigo AND   MFP.MFAC_NUMEORDEPAGO = DIT.dite_numedocu
                                    AND   DIT.tmov_tipomovi = 31
                                    GROUP BY DIT.mite_codigo, PLIN.plin_tipo, MIT.mite_nombre, DIT.dite_valounit, MIT.plin_codigo,  DIT.dite_porcdesc, DIT.piva_porciva, DIT.DITE_PREFDOCUREFE,MFP.MFAC_VALOFLET,MFP.MFAC_VALOIVAFLET, DIT.DITE_NUMEDOCUREFE)
                                    GROUP BY ITEM,MITE_CODIGO,  MITE_NOMBRE, DITE_VALOUNIT, PLIN_CODIGO, DITE_PORCDESC,PIVA_PORCIVA, PDOC_CODIGO,FLETE, IVAFLETE,DITE_NUMEDOCU
                                    ORDER BY plin_codigo,mite_codigo;";
				DBFunctions.Request(ds, IncludeSchema.NO,slqDev);  // se carga tanto la entrada como las devoluciones en un solo select para todos los registros de la entrada
  				int n;
				dtSource.Rows.Clear();
                if (ds.Tables[0].Rows.Count == 0)
                    Utils.MostrarAlerta(Response, "La factura NO tiene items o ya ha sido devuelta !");
                for (n=0;n<ds.Tables[0].Rows.Count;n++)
				{
					double cantidadEntrada = 0, cantidadaDevolver = 0, cantidadDevuelta = 0;
                    cantidadEntrada  = Convert.ToDouble(ds.Tables[0].Rows[n][9].ToString());
                    cantidadDevuelta = Convert.ToDouble(ds.Tables[0].Rows[n][10].ToString());
                    cantidadaDevolver = cantidadEntrada - cantidadDevuelta;
                    if ((cantidadEntrada - cantidadDevuelta) > 0)
					{
						DataRow fila = dtSource.NewRow();
						fila[0] = ds.Tables[0].Rows[n][0].ToString();
						fila[1] = ds.Tables[0].Rows[n][2].ToString();
                        fila[2] = cantidadEntrada - cantidadDevuelta;
						fila[3] = 0;
						fila[4] = Convert.ToDouble(ds.Tables[0].Rows[n][3]);
						fila[5] = Convert.ToDouble(ds.Tables[0].Rows[n][5]);
						fila[6] = Convert.ToDouble(ds.Tables[0].Rows[n][6]);
                        fila[7] = Total(Convert.ToDouble(ds.Tables[0].Rows[n][3]), cantidadEntrada - cantidadDevuelta, Convert.ToDouble(ds.Tables[0].Rows[n][5]), Convert.ToDouble(ds.Tables[0].Rows[n][6]));
						fila[8] = ds.Tables[0].Rows[n][4].ToString();
						fila[9] = 0;
                        fila[10] = ds.Tables[0].Rows[n][7].ToString();  // prefijo entrada
                        fila[11] = ds.Tables[0].Rows[n][8].ToString();  // numero entrada
                        fila[12] = cantidadaDevolver;
                        fila[13] = ds.Tables[0].Rows[n][11].ToString();
                        fila[14] = ds.Tables[0].Rows[n][12].ToString();
						dtSource.Rows.Add(fila);
					}
				}
			}
			BindDatas();
		}

        protected void SelNIT(Object Sender, EventArgs E)
        {
            if (txtNIT.Text.Length == 0)
            {
                Utils.MostrarAlerta(Response, "Debe ingresar el NIT!");
                return;
            }
            if (txtNITa.Text == "")
                txtNITa.Text = DBFunctions.SingleData("select nombre from vmnit where mnit_nit = '" + txtNIT.Text + "'; ");
            if ((DBFunctions.RecordExist("SELECT * FROM PNITTALLER WHERE PNITAL_NITTALLER='" + txtNIT.Text + "';")) || (DBFunctions.RecordExist("SELECT * FROM PNITproduccion WHERE PNIT_NITprod='" + txtNIT.Text + "';")))
            {
                ViewState["TIPONIT"] = "NT";
                ViewState["TIPOPRO"] = "IT";
            }
            else
            {
                ViewState["TIPONIT"] = "NC";
                ViewState["TIPOPRO"] = "IC";
            }
            txtNIT.Enabled = false;
            btnSelNIT.Visible = false;
            plcPrefNum.Visible = true;
            CambiarPrefijo();
        }
		//CAMBIAR EL NIT(CONFIRMAR)------------------------------
		protected void CambiaNIT(Object Sender, EventArgs E)
		{
			//OJO
			//VALIDAR TIPO NITS (CLIENTE Y PROVEEDOR)
			ddlFact.Items.Clear();
			DatasToControls bind = new DatasToControls();
			string sqlT = "";
            if (txtNIT.Text.Length == 0)
            {
                return;
            }
            if (!DBFunctions.RecordExist("SELECT MNIT_NIT FROM MNIT WHERE MNIT_NIT='" + txtNIT.Text + "';"))
            {
                Utils.MostrarAlerta(Response, "El NIT no existe!");
                return;
            }
            if (ddlVendedor.SelectedValue == "Seleccione:.." || ddlVendedor.SelectedValue == "Seleccione..." || ddlVendedor.SelectedValue == "0" )
            {
                Utils.MostrarAlerta(Response, "NO HA seleccionado un responsable (Vendedor)!");
                return;
            }
            if (ddlAlmacen.SelectedValue == "Seleccione:..")
            {
                Utils.MostrarAlerta(Response, "NO HA seleccionado una Sede (Almacén)!");
                return;
            }
            if (ddlCodigo.Items.Count == 0 || ddlCodigo.SelectedValue == "Seleccione:.." || ddlCodigo.SelectedValue == "Seleccione...")
            {
                Utils.MostrarAlerta(Response, "NO HA seleccionado un Documento para realizar la Devolución!");
                return;
            }

            txtNITa.Text = DBFunctions.SingleData("SELECT nombre FROM vMNIT WHERE MNIT_NIT='" + txtNIT.Text + "';");
			if(Tipo=="P")//Tipo Proveedor
			{
				if(!DBFunctions.RecordExist("SELECT * FROM mproveedor WHERE mnit_nit = '"+txtNIT.Text+"'"))
				{
                    Utils.MostrarAlerta(Response, "El NIT no pertenece a un proveedor!");
					return;
				}
				//Debemos determinar si se puede aun se pueden devolver facturas de este proveedor
				//es decir que aun tenemos items para devolver
                sqlT = "SELECT pdoc_codiordepago CONCAT ' - ' CONCAT CAST(mfac_numeordepago as character(10)) AS CODIGO FROM mfacturaproveedor WHERE mnit_nit = '" + txtNIT.Text.Trim() + "' AND MFAC_INDIDOCU <> 'N' ORDER BY 1; ";
                DataSet ds = new DataSet();
				DBFunctions.Request(ds,IncludeSchema.NO,sqlT);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlFact.Items.Add(new ListItem(ds.Tables[0].Rows[i][0].ToString(), ds.Tables[0].Rows[i][0].ToString()));
                }
            }
			else if(Tipo=="C")//Tipo Cliente
			{
				// Verificar NIT NT o NC
                // Ahora debemos determinar si el cliente es un Taller, una Planta de Producción o una Ppersona_Venta Mostrador
				// Si el cliente es un taller debemos cargar las transferencias de las ordenes de trabajo abiertas las cuales tengan repuestos asignados
                if (DBFunctions.RecordExist("SELECT * FROM pnittaller WHERE pnital_nittaller='" + txtNIT.Text.Trim() + "' "))
                {   // transferencias a Taller
                    sqlT = "SELECT CODIGO, REFERENCIA, SUM(VENTA), SUM(DEVOL) FROM ( " +
                    " SELECT DISTINCT MFC.pdoc_codigo CONCAT '-' CONCAT VARCHAR_FORMAT(MFC.mfac_numedocu,'99999999') AS CODIGO, " +
                    " DIT.mite_codigo AS REFERENCIA, SUM(DIT.dite_cantidad) as venta, 0 as devol " +
                    " FROM morden MOR, mordentransferencia MOT, mfacturacliente MFC, ditems DIT " +
                    " WHERE MOR.pdoc_codigo = MOT.pdoc_codigo AND MOR.mord_numeorde = MOT.mord_numeorde AND MOR.test_estado = 'A' " +
                    " AND MFC.pdoc_codigo = MOT.pdoc_factura AND MFC.mfac_numedocu = MOT.mfac_numero AND MFC.pdoc_codigo = DIT.pdoc_codigo " +
                    " AND MFC.mfac_numedocu = DIT.dite_numedocu AND DIT.tmov_tipomovi = 80 AND MFC.mnit_nit='" + txtNIT.Text.Trim() + "' " +
                    " group by MFC.pdoc_codigo, MFC.mfac_numedocu, DIT.mite_codigo "+
                    "  union " +
                    " SELECT DISTINCT MFC.pdoc_codigo CONCAT '-' CONCAT VARCHAR_FORMAT(MFC.mfac_numedocu,'99999999') AS CODIGO, " +
                    " DId.mite_codigo AS REFERENCIA, 0 as venta, COALESCE(SUM(DID.dite_cantidad),0) as devol " +
                    " FROM morden MOR, mordentransferencia MOT, mfacturacliente MFC, ditems DId " +
                    " WHERE MOR.pdoc_codigo = MOT.pdoc_codigo AND MOR.mord_numeorde = MOT.mord_numeorde AND MOR.test_estado = 'A' " +
                    " AND MFC.pdoc_codigo = MOT.pdoc_factura AND MFC.mfac_numedocu = MOT.mfac_numero " +
                    " AND MFC.pdoc_codigo = DID.dite_prefdocurefe AND MFC.mfac_numedocu = DID.dite_numedocurefe AND DId.tmov_tipomovi = 81 AND MFC.mnit_nit='" + txtNIT.Text.Trim() + "' " +
                    " group by MFC.pdoc_codigo, MFC.mfac_numedocu, DId.mite_codigo ORDER BY 1,2) AS A GROUP BY CODIGO, REFERENCIA; ";

                    if(!DBFunctions.RecordExist("SELECT TDOC_TIPODOCU FROM PDOCUMENTO WHERE PDOC_CODIGO='" + ddlCodigo.SelectedValue + "' AND TDOC_TIPODOCU='NT';"))
                    {
                        Utils.MostrarAlerta(Response, "Prefijo no válido para un taller, debe ser tipo NT!");
                          return;
                    }
                }
                else
                {
                    if (DBFunctions.RecordExist("SELECT * FROM pnitPRODUCCION WHERE pnit_nitPROD='" + txtNIT.Text.Trim() + "' "))
                    {   // transferencias a Planta de Produccion
                        sqlT = "SELECT CODIGO, REFERENCIA, SUM(VENTA), SUM(DEVOL) FROM ( " +
                            "SELECT DISTINCT MFC.pdoc_codigo CONCAT '-' CONCAT VARCHAR_FORMAT(MFC.mfac_numedocu,'99999999') AS CODIGO, " +
                            " DIT.mite_codigo AS REFERENCIA, SUM(DIT.dite_cantidad) as venta, 0 as devol " +
                            " FROM mordenPRODUCCION MOR, mordenPRODUCCIONtransferencia MOT, mfacturacliente MFC, ditems DIT " +
                            " WHERE MOR.pdoc_codigo = MOT.pdoc_codigo AND MOR.mord_numeorde = MOT.mord_numeorde AND MOR.test_estado = 'A' " +
                            " AND MFC.pdoc_codigo = MOT.pdoc_factura AND MFC.mfac_numedocu = MOT.mfac_numero AND MFC.pdoc_codigo = DIT.pdoc_codigo " +
                            " AND MFC.mfac_numedocu = DIT.dite_numedocu AND DIT.tmov_tipomovi = 80 AND MFC.mnit_nit='" + txtNIT.Text.Trim() + "' " +
                            " group by MFC.pdoc_codigo, MFC.mfac_numedocu, DIT.mite_codigo "+
                            "  union " +
                            "SELECT DISTINCT MFC.pdoc_codigo CONCAT '-' CONCAT VARCHAR_FORMAT(MFC.mfac_numedocu,'99999999') AS CODIGO, " +
                            " DId.mite_codigo AS REFERENCIA, 0 as venta, COALESCE(SUM(DID.dite_cantidad),0) as devol " +
                            " FROM mordenPRODUCCION MOR, mordenPRODUCCIONtransferencia MOT, mfacturacliente MFC, ditems DId " +
                            " WHERE MOR.pdoc_codigo = MOT.pdoc_codigo AND MOR.mord_numeorde = MOT.mord_numeorde AND MOR.test_estado = 'A' " +
                            " AND MFC.pdoc_codigo = MOT.pdoc_factura AND MFC.mfac_numedocu = MOT.mfac_numero " +
                            " AND MFC.pdoc_codigo = DID.dite_prefdocurefe AND MFC.mfac_numedocu = DID.dite_numedocurefe AND DId.tmov_tipomovi = 81 AND MFC.mnit_nit='" + txtNIT.Text.Trim() + "' " +
                            " group by MFC.pdoc_codigo, MFC.mfac_numedocu, DId.mite_codigo ORDER BY 1,2) AS A GROUP BY CODIGO, REFERENCIA; ";
                        // Documento debe ser NT
                        if (!DBFunctions.RecordExist(
                            "SELECT TDOC_TIPODOCU FROM PDOCUMENTO WHERE PDOC_CODIGO='" + ddlCodigo.SelectedValue + "' AND TDOC_TIPODOCU='NT';"))
                        {
                            Utils.MostrarAlerta(Response, "Prefijo no válido para un taller, debe ser tipo NT!");
                            return;
                        }
                    }
                    else
                    {   // Ventas por Mostrador y Consumos 
                        sqlT = "SELECT CODIGO, REFERENCIA, SUM(VENTA), SUM(DEVOL) FROM ( " +
                            "SELECT DISTINCT MFC.pdoc_codigo CONCAT '-' CONCAT VARCHAR_FORMAT(MFC.mfac_numedocu,'99999999') AS CODIGO, " +
                            " DIT.mite_codigo AS REFERENCIA, SUM(DIT.dite_cantidad) as venta, 0 as devol " +
                            "FROM mfacturacliente MFC, ditems DIT " +
                            "WHERE MFC.pdoc_codigo = DIT.pdoc_codigo " +
                            "AND MFC.mfac_numedocu = DIT.dite_numedocu AND DIT.tmov_tipomovi in (60,90) AND MFC.mnit_nit='" + txtNIT.Text.Trim() + "' " +
                            "group by MFC.pdoc_codigo, MFC.mfac_numedocu, DIT.mite_codigo " +
                            " union " +
                            "SELECT DISTINCT MFC.pdoc_codigo CONCAT '-' CONCAT CAST(MFC.mfac_numedocu AS CHARACTER(15)) AS CODIGO, " +
                            " DId.mite_codigo AS REFERENCIA, 0 as venta, COALESCE(SUM(DID.dite_cantidad),0) as devol " +
                            "FROM mfacturacliente MFC, ditems DId " +
                            "WHERE MFC.pdoc_codigo = DID.dite_prefdocurefe AND MFC.mfac_numedocu = DID.dite_numedocurefe AND DId.tmov_tipomovi in (61,91) " +
                            "AND MFC.mnit_nit='" + txtNIT.Text.Trim() + "' " +
                            "group by MFC.pdoc_codigo, MFC.mfac_numedocu, DId.mite_codigo ORDER BY 1,2 ) AS A GROUP BY CODIGO, REFERENCIA; "; 
                        // Documento debe ser NC
                        if (!DBFunctions.RecordExist(
                           "SELECT TDOC_TIPODOCU FROM PDOCUMENTO WHERE PDOC_CODIGO='" + ddlCodigo.SelectedValue + "' AND TDOC_TIPODOCU='NC';"))
                        {
                            Utils.MostrarAlerta(Response, "Prefijo no válido para un cliente, debe ser tipo NC!");
                            return;
                        }
                    }
                }
				//Aqui vamos a determinar si de esta factura todavia se pueden devolver
				DataSet ds = new DataSet();
				DBFunctions.Request(ds,IncludeSchema.NO,sqlT);
                double cantidadSalida = 0, cantidadEntrada = 0;
                bool condicion = false;
                string estaFactura = "";
                try
                {   if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                          //  for (int ih = 0; ih < ds.Tables[1].Rows.Count; ih++)
                          //  {
                          //      if (ds.Tables[0].Rows[i][0].ToString() == ds.Tables[1].Rows[ih][0].ToString())
                          //      {
                                    condicion = false;
                                    DataRow[] selection1 = ds.Tables[0].Select("CODIGO='" + ds.Tables[0].Rows[i][0].ToString() + "'");
                                    try { cantidadSalida = Convert.ToDouble(ds.Tables[0].Rows[i][2]); }
                                    catch { }
                                    try { cantidadEntrada = Convert.ToDouble(ds.Tables[0].Rows[i][3]); }
                                    catch { }
                                    if (cantidadSalida - cantidadEntrada > 0)
                                    {
                                        if (ds.Tables[0].Rows[i][0].ToString() != estaFactura)
                                        {
                                            condicion = true;
                                            estaFactura = ds.Tables[0].Rows[i][0].ToString();
                                        }
                                    }
                                    if (condicion) 
                                    {
                                        ddlFact.Items.Add(new ListItem(ds.Tables[0].Rows[i][0].ToString(), ds.Tables[0].Rows[i][0].ToString()));
                                        //string ddlValue = ds.Tables[0].Rows[i][0].ToString() + "  Orden:" + ds.Tables[0].Rows[i][4].ToString() + " Placa:" + ds.Tables[0].Rows[i][5].ToString();
                                        //ddlFact.Items.Add(new ListItem(ds.Tables[0].Rows[i][0].ToString(), ddlValue));
                                    }
                          //      }
                         //  }
                        }
                    }
                }
                catch
                {
                    Utils.MostrarAlerta(Response, "El NIT NO tiene facturas pendientes!");
                    Button1.Enabled = false;
                    return;
                }
			}
            if(ddlFact.Items.Count>0)
				plhFact.Visible=true;
			else
                Utils.MostrarAlerta(Response, "No existen facturas para el NIT especificado!");
		}
	
		//GRILLAS CANCELAR EDICION--------------------------------------------- 	
 		public void DgInserts_Cancel(Object sender, DataGridCommandEventArgs e)
 		{
			dgItems.EditItemIndex=-1;
	 		BindDatas();
 		}

		//BINDATAS---------------------
		protected void BindDatas()
		{
			bool ed=false;
			int i;
			if(dtSource.Rows.Count==0)
			{
				dgItems.EditItemIndex=-1;
				ed=true;
			}
			ddlCodigo.Enabled=ddlAlmacen.Enabled=ddlVendedor.Enabled=txtNIT.Enabled=txtNITa.Enabled=tbDate.Enabled=ddlFact.Enabled=btnSelNIT.Enabled=btnSelFact.Enabled=ed;
			btnAjus.Enabled=!ed;
			dgItems.DataSource=dtSource;
			dgItems.DataBind();
			for(i=0;i<dgItems.Columns.Count;i++)
			{
				if(i>=5 && i<=7)
					for(int j=0;j<dgItems.Items.Count;j++)
						dgItems.Items[j].Cells[i].HorizontalAlign = HorizontalAlign.Right;
			}
			Session["dtInsertsDC"]=dtSource;
            valorNeto = valorIva = numeroItems = 0;
            for (i=0;i<dtSource.Rows.Count;i++)
			{
				valorNeto += ((Convert.ToDouble(dtSource.Rows[i][4]) - (Convert.ToDouble(dtSource.Rows[i][4]) * (Convert.ToDouble(dtSource.Rows[i][5]) /100)))*Convert.ToDouble(dtSource.Rows[i][9]));
				valorIva += ((Convert.ToDouble(dtSource.Rows[i][4]) - (Convert.ToDouble(dtSource.Rows[i][4]) * (Convert.ToDouble(dtSource.Rows[i][5]) /100)))*Convert.ToDouble(dtSource.Rows[i][9]))*(Convert.ToDouble(dtSource.Rows[i][6])/100);
				numeroItems += Convert.ToDouble(dtSource.Rows[i][9]);
			}
			valorNeto= Math.Round(valorNeto*1,0);  // Hector ago.18.2008 Redondeo 
            valorIva = Math.Round(valorIva*1,0);   // Hector ago.18.2008 Redondeo

			tbValNeto.Text  = valorNeto.ToString("C");
			tbValIva.Text   = valorIva.ToString("C");
			txtNumUnid.Text = numeroItems.ToString("N");
			txtNumItem.Text = dtSource.Rows.Count.ToString("N");
            btnAjus.Enabled = (numeroItems > 0);
            txtvaloflet.Text = valorflete.ToString("N");
            txtvaloivaflet.Text = valorivaflete.ToString("N");
		}

		//CREAR TABLA------------------------	
		protected void LoadDataColumns()
		{
			lbFields.Add("mite_codigo");//0 codigo Item
			types.Add(typeof(string));
			lbFields.Add("mite_nombre");//1 nombre Item
			types.Add(typeof(string));
			lbFields.Add("mite_cantped");//2 Cantidad Inicial
			types.Add(typeof(double));
			lbFields.Add("mite_cantfac");//3 Cantidad a facturar
			types.Add(typeof(double));
			lbFields.Add("mite_precio");//4 Valor unidad
			types.Add(typeof(double));
			lbFields.Add("mite_desc");//5 Descuento
			types.Add(typeof(double));
			lbFields.Add("mite_iva");//6 Iva
			types.Add(typeof(double));
			lbFields.Add("msal_tot");//7 Total
			types.Add(typeof(double));
			lbFields.Add("plin_codigo");//8 Linea
			types.Add(typeof(string));
			lbFields.Add("cantidad");//9 Cantidad a Devolver
			types.Add(typeof(double));
            lbFields.Add("dite_prefdocurefe");//10 prefijo pedido
            types.Add(typeof(string));
            lbFields.Add("dite_numedocurefe");//11 numero pedido
            types.Add(typeof(string));
            lbFields.Add("dite_cantdevo");//12 Cantidad Devuelta
            types.Add(typeof(double));
            lbFields.Add("flete");//13 Flete
            types.Add(typeof(double));
            lbFields.Add("ivaflete");//14 ivaflete
            types.Add(typeof(double));

		}
		
		protected void LoadDataTable()
		{
			dtSource = new DataTable();
	        for(int i=0; i<lbFields.Count; i++)
            	dtSource.Columns.Add(new DataColumn((string)lbFields[i], (Type)types[i]));
		}

		//ACTUALIZAR GRILLAS---------------------------------------------------
    	public void DgInserts_Update(Object sender, DataGridCommandEventArgs e)
    	{
 			string refI = "";
			Referencias.Guardar(dtSource.Rows[e.Item.DataSetIndex][0].ToString(),ref refI,DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='"+dtSource.Rows[e.Item.DataSetIndex][8].ToString()+"'"));
			int cantActual = Convert.ToInt32(DBFunctions.SingleData("select dbxschema.cantactl('"+refI+"',"+DBFunctions.SingleData("SELECT pano_ano FROM cinventario")+") FROM dbxschema.mitems where mite_codigo='"+refI+"'"));
			if(dtSource.Rows.Count==0)
 				return;
        	double cantf = Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text);
            string refActual = e.Item.Cells[2].Text;
	      	if(cantf<0)
	      		cantf=0;
            bool errado = false;

            if (dtSource.Rows.Count > 0)
            {
                for (int h = 0; h < dtSource.Rows.Count; h++)
                {
                    if (dtSource.Rows[h][0].ToString() == refActual && Convert.ToDouble(dtSource.Rows[h][3]) > 0 && h != dgItems.EditItemIndex)
                    {
                        Utils.MostrarAlerta(Response, "el código del Item ya existe en la relación, haga otra devolución para este item !");
                        errado = true;
                        return;  // No se permite el mismo item en la misma devolución porque la llave unica de ditems no lo acepta.
                    }
                }
            }
              
			if(Tipo=="C")
			{
				if(cantf <= Convert.ToDouble(dtSource.Rows[dgItems.EditItemIndex][2]))
				{
					dtSource.Rows[dgItems.EditItemIndex][3] = cantf;
					dtSource.Rows[dgItems.EditItemIndex][9] = cantf;
					dtSource.Rows[dgItems.EditItemIndex][7] = Total(Convert.ToDouble(dtSource.Rows[dgItems.EditItemIndex][4]),cantf,Convert.ToDouble(dtSource.Rows[dgItems.EditItemIndex][5]),Convert.ToDouble(dtSource.Rows[dgItems.EditItemIndex][6]));
				}
				else
                    Utils.MostrarAlerta(Response, "Cantidad para Devolución Inválida!");
			}
			else 
                if(Tipo=="P")
			    {
                    if (cantf <= cantActual)
                    {
                        if (cantf <= Convert.ToDouble(dtSource.Rows[dgItems.EditItemIndex][2]))
                        {
                            dtSource.Rows[dgItems.EditItemIndex][3] = cantf;
                            dtSource.Rows[dgItems.EditItemIndex][9] = cantf;
                            dtSource.Rows[dgItems.EditItemIndex][7] = Total(Convert.ToDouble(dtSource.Rows[dgItems.EditItemIndex][4]), cantf, Convert.ToDouble(dtSource.Rows[dgItems.EditItemIndex][5]), Convert.ToDouble(dtSource.Rows[dgItems.EditItemIndex][6]));
                        }
                        else
                            Utils.MostrarAlerta(Response, "Cantidad para Devolución Inválida!");
                    }
                    else
                        Utils.MostrarAlerta(Response, "No hay Cantidad Disponible para la Devolución!");
			    }

	    	dgItems.EditItemIndex=-1;
			if(!errado)
                BindDatas();
    	}

		//EDITAR GRILLAS-----------------------------------------------------
		public void DgInserts_Edit(Object sender, DataGridCommandEventArgs e)
		{
     		if(dtSource.Rows.Count>0)
     			dgItems.EditItemIndex=(int)e.Item.ItemIndex;
	    	BindDatas();
		}

		//OPERACIONES GRILLAS----------------------------------------------------    
		public void DgInserts_AddAndDel(object sender, DataGridCommandEventArgs e)
		{
			if(((Button)e.CommandSource).CommandName == "DelDatasRow")
			{
            	try
            	{
        			dtSource.Rows.Remove(dtSource.Rows[e.Item.ItemIndex]);
	            	dgItems.EditItemIndex=-1;
    	    	}catch{};
        	}
	        if(((Button)e.CommandSource).CommandName == "ClearRows")
	        {
            	try
            	{
        			dtSource.Rows.Clear();
	            	dgItems.EditItemIndex=-1;
    	    	}catch{};
	        }
            BindDatas();
		}
	
		//CAMBIAR FECHA-------------------------------------------	
		protected void ChangeDate(Object Sender, EventArgs E)
		{
			if(dtSource.Rows.Count==0)
				tbDate.Text = calDate.SelectedDate.GetDateTimeFormats()[6];
		}

		//REALIZAR PROCESO-------------------------------------	
		protected void NewAjust(Object Sender, EventArgs E)
		{
            string preF = "",numF = "";
			string sa   = ddlFact.SelectedValue.ToString();
			string palm = ddlAlmacen.SelectedValue;
			string vend = ddlVendedor.SelectedItem.Value;
			string carg = DBFunctions.SingleData("SELECT TVEND_CODIGO FROM PVENDEDOR WHERE PVEN_CODIGO='"+vend+"'");
            string ccos = DBFunctions.SingleData("SELECT pcen_centinv FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + palm + "'");
			string pano = DBFunctions.SingleData("SELECT pano_ano FROM cinventario");
			preF = sa.Substring(0,sa.LastIndexOf("-")).Trim();
			numF = sa.ToString().Substring(sa.LastIndexOf("-")+1).Trim();
			NotaDevolucionCliente notaCliente = new NotaDevolucionCliente();
			NotaDevolucionProveedor notaProveedor = new NotaDevolucionProveedor();
			Movimiento Mov = new Movimiento();
			string nit = "";
			//OJO DEVOLUCION CLIENTE Y PROOVEDOR
			int tipomovi;//Devolucion cliente
			uint numNota = 0;
			if(Tipo=="P")
			{
				tipomovi=31;//Proveedor
                nit = DBFunctions.SingleData(" SELECT mnit_nit FROM mfacturaproveedor WHERE pdoc_codiordepago='" + preF + "' AND mfac_numeordepago=" + numF + "");
			}
			else 
			{       // aqui traer todas las variables en un solo select en un array 
				nit = DBFunctions.SingleData("SELECT mnit_nit FROM mfacturacliente WHERE pdoc_codigo='"+preF+"' AND mfac_numedocu="+numF+"");
				//Debemos revisar si el nit es de tipo cliente o taller
				if(DBFunctions.RecordExist("SELECT * FROM pnittaller WHERE pnital_nittaller='"+nit+"'"))
				    tipomovi = 81;      //Taller
				else
                    if (DBFunctions.RecordExist("SELECT mfac_tipodocu FROM mfacturacliente WHERE pdoc_codigo='" + preF + "' AND mfac_numedocu=" + numF + " and mfac_tipodocu = 'I' "))
                        tipomovi = 61;  //Consumo Internor
                    else
                        tipomovi = 91;  //Cliente
			}

			//Ahora vamos con el manejo de las notas de devolucion a cliente
			if(Tipo=="C")
			{
				double valorDevolucion = 0;
				try{valorDevolucion = Convert.ToDouble(tbValNeto.Text.Substring(1));}
				catch{}
				double valorIvaDevolucion = 0;
				try{valorIvaDevolucion = Convert.ToDouble(tbValIva.Text.Substring(1));}
				catch{}
				double valorRetencionFactura = 0;
				try{valorRetencionFactura = Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valorete FROM mfacturaCLIENTE WHERE pdoc_codigo='"+preF+"' AND mfac_numeDOCU="+numF+""));}
				catch{}
				double valorRetencionDevolucion = 0;
				if(valorRetencionFactura > 0)
				{
					double valorNetoFactura = Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact + mfac_valoflet FROM mfacturaCLIENTE WHERE pdoc_codigo='"+preF+"' AND mfac_numeDOCU="+numF+""));
					try{valorRetencionDevolucion = (valorDevolucion/valorNetoFactura)*valorRetencionFactura;}
					catch{ valorRetencionDevolucion = 0; }
				}
				notaCliente = new NotaDevolucionCliente(ddlCodigo.SelectedValue,preF,Convert.ToUInt32(lblNumDev.Text.Trim()),Convert.ToUInt32(numF.Trim()),"N","FA",
														valorDevolucion,valorIvaDevolucion,valorRetencionDevolucion,DateTime.Now,HttpContext.Current.User.Identity.Name.ToLower(),null);  // FALTA ENVIAR LA TABLA DE RETENCIONES CAUSADAS A CLIENTE
                notaCliente.ObservacionDevolucion = txtObs.Text;
                notaCliente.GrabarNotaDevolucionCliente(false);
				numNota = notaCliente.NumeroNota;
			}
			else if(Tipo=="P")
            {
               
				double valorDevolucion = 0;
				try{valorDevolucion = Convert.ToDouble(tbValNeto.Text.Substring(1));}
                catch { valorDevolucion = 0; }
				double valorIvaDevolucion = 0;
				try{valorIvaDevolucion = Convert.ToDouble(tbValIva.Text.Substring(1));}
                catch { valorIvaDevolucion = 0; }
				double valorRetencionFactura = 0;
                double valorfleteDevolucion = 0;
                try { valorfleteDevolucion = Convert.ToDouble(txtvaloflet.Text); }
                catch { valorfleteDevolucion = 0; }
                double valorivafleteDevolucion = 0;
                try { valorivafleteDevolucion = Convert.ToDouble(txtvaloivaflet.Text); }
                catch { valorivafleteDevolucion = 0; }       
				try{valorRetencionFactura = Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valorete FROM mfacturaproveedor WHERE pdoc_codiordepago='"+preF+"' AND mfac_numeordepago="+numF+""));}
                catch { valorRetencionFactura = 0; }
				double valorRetencionDevolucion = 0;
				if(valorRetencionFactura > 0)
				{
					double valorNetoFactura = Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact + mfac_valoflet FROM mfacturaproveedor WHERE pdoc_codiordepago='"+preF+"' AND mfac_numeordepago="+numF+""));
					try{valorRetencionDevolucion = (valorDevolucion/valorNetoFactura)*valorRetencionFactura;}
                    catch { valorRetencionDevolucion = 0; }                    
				}


               
				notaProveedor = new NotaDevolucionProveedor(ddlCodigo.SelectedValue,preF,Convert.ToUInt32(lblNumDev.Text.Trim()),Convert.ToUInt32(numF.Trim()),"N","FA",
															valorDevolucion,valorIvaDevolucion,valorRetencionDevolucion,DateTime.Now,HttpContext.Current.User.Identity.Name.ToLower());
                notaProveedor.Observacion = txtObs.Text;
                double valorflete = Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoflet FROM mfacturaproveedor WHERE pdoc_codiordepago='" + preF + "' AND mfac_numeordepago=" + numF + ""));
                double valorivaflete = Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoIVAflet FROM mfacturaproveedor WHERE pdoc_codiordepago='" + preF + "' AND mfac_numeordepago=" + numF + ""));
                if (valorflete < valorfleteDevolucion)
                {
                    Utils.MostrarAlerta(Response, "El valor del flete es mayor que el valor de la devolucion!");
                    return;
                }
                else if (valorivaflete < valorivafleteDevolucion)
                {
                        Utils.MostrarAlerta(Response, "El valor del iva flete es mayor que el valor del iva!");
                        return;
                }

                notaProveedor.ValorivafleteDevolucion = Convert.ToDouble(txtvaloivaflet.Text);
                notaProveedor.ValorfleteDevolucion = Convert.ToDouble(txtvaloflet.Text);              

				notaProveedor.GrabarNotaDevolucionProveedor(false);
				numNota = notaProveedor.NumeroNota;
			}
			//Movimiento de Kardex
			Mov = new Movimiento(ddlCodigo.SelectedValue,numNota,
			                     preF,Convert.ToUInt32(numF),tipomovi,txtNIT.Text,palm,calDate.SelectedDate,
			   	                 vend,carg,ccos,"N","");
			int n;
			double valorIvaFactura = 0;
			string ano_cont = DBFunctions.SingleData("SELECT pano_ano from cinventario");
            ArrayList sqlStrings = new ArrayList();
            ArrayList sqlStringsUpd = new ArrayList();
			for(n=0;n<dtSource.Rows.Count;n++)
			{
                if (Convert.ToDouble(dtSource.Rows[n]["cantidad"]) != 0)
				{
					string codI = "";
					Referencias.Guardar(dtSource.Rows[n][0].ToString(),ref codI,DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='"+dtSource.Rows[n][8].ToString()+"'"));
					double cant = Convert.ToDouble(dtSource.Rows[n][3]);
					double valU = Convert.ToDouble(dtSource.Rows[n][4]);
					double costP = 0,costPH = 0,costPA = 0,costPHA = 0, pIva = 0, pDes = 0, invI = 0, invIA = 0;
					pIva = Convert.ToDouble(dtSource.Rows[n][6]);
					pDes = Convert.ToDouble(dtSource.Rows[n][5]);
					valorIvaFactura += (((valU*cant)*pIva)/(pIva+100));
					DateTime fecha=calDate.SelectedDate;
					DataSet da = new DataSet();
					try{costP = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costprom FROM msaldoitem WHERE pano_ano="+pano+" AND mite_codigo='"+codI+"'"));}
					catch{costP=0;}
					try{costPH = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costpromhist FROM msaldoitem WHERE pano_ano="+pano+" AND mite_codigo='"+codI+"'"));}
					catch{costPH=0;}
					try{costPA = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costprom FROM msaldoitemalmacen WHERE pano_ano="+pano+" AND palm_almacen='"+palm+"' AND mite_codigo='"+codI+"'"));}
					catch{costPA=0;}
					try{costPHA = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costpromhist FROM msaldoitemalmacen WHERE pano_ano="+pano+" AND palm_almacen='"+palm+"' AND mite_codigo='"+codI+"'"));}
					catch{costPHA=0;}
					try{invI = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_cantactual FROM msaldoitem WHERE pano_ano="+pano+" AND mite_codigo='"+codI+"'"));}
					catch{invI=0;}
					try{invIA = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_cantactual FROM msaldoitemalmacen WHERE pano_ano="+pano+" AND palm_almacen='"+palm+"' AND mite_codigo='"+codI+"'"));}
					catch{invIA=0;}
					Mov.InsertaFila(codI,cant,valU,costP,costPA,pIva,pDes,0,costPH,costPHA,valU,invI,invIA,0,"","");

                    //Actualizar cantidad devuelta
                    sqlStringsUpd.Add(
                        "UPDATE DITEMS "+
                        "SET DITE_CANTDEVO=DITE_CANTDEVO+" + Convert.ToDouble(dtSource.Rows[n]["cantidad"]) + " " +
                        "WHERE PDOC_CODIGO='" + ddlCodigo.SelectedValue + "' AND DITE_NUMEDOCU=" + numNota + " AND " +
                        "MITE_CODIGO='"+codI+"';");
				
                    string sqlPed = "UPDATE DPEDIDOITEM "+
                        "SET DPED_CANTFACT=DPED_CANTFACT - " + Convert.ToDouble(dtSource.Rows[n]["cantidad"]) + " " +
                        "WHERE PPED_CODIGO='" + dtSource.Rows[n]["dite_prefdocurefe"].ToString() + "' AND MPED_NUMEPEDI="+ dtSource.Rows[n]["dite_numedocurefe"].ToString() +" AND " +
                        "MITE_CODIGO='"+codI+"' ";
            //        sqlStringsUpd.Add(sqlPed);
                    sqlStrings.Add(sqlPed);
                }
			}
			if(DBFunctions.RecordExist("SELECT * FROM pnittaller WHERE pnital_nittaller='"+nit+"'"))
				valorIvaFactura = 0;
			//Realizamos la grabacion del registro de mfacturacliente
			if(Tipo=="C")
			{
				Mov.RealizarMov(false);
				for(n=0;n<notaCliente.SqlStrings.Count;n++)
					sqlStrings.Add(notaCliente.SqlStrings[n].ToString());
				for(n=0;n<Mov.SqlStrings.Count;n++)
					sqlStrings.Add(Mov.SqlStrings[n].ToString());
                for (n = 0; n < sqlStringsUpd.Count; n++)
                    sqlStrings.Add(sqlStringsUpd[n].ToString());
				if(DBFunctions.Transaction(sqlStrings))
				{
					//lbInfo.Text += "<br>Bien : "+DBFunctions.exceptions;
					Response.Redirect("" + indexPage + "?process=Inventarios.Devoluciones&actor="+Tipo+"&prefN="+notaCliente.PrefijoNota+"&numN="+notaCliente.NumeroNota+"");
				}
				else
					lbInfo.Text += "<br>Error : "+DBFunctions.exceptions;
			}
			else if(Tipo=="P")
			{
				/////////////////////Realizar devolucion a proveedor de repuestos
				Mov.RealizarMov(false);
				for(n=0;n<notaProveedor.SqlStrings.Count;n++)
					sqlStrings.Add(notaProveedor.SqlStrings[n].ToString());
				for(n=0;n<Mov.SqlStrings.Count;n++)
					sqlStrings.Add(Mov.SqlStrings[n].ToString());
				if(DBFunctions.Transaction(sqlStrings))
				{
					//lbInfo.Text += "<br>Bien : "+DBFunctions.exceptions;
					Response.Redirect("" + indexPage + "?process=Inventarios.Devoluciones&actor="+Tipo+"&prefN="+notaProveedor.PrefijoNota+"&numN="+notaProveedor.NumeroNota+"");
				}
				else
					lbInfo.Text += "<br>Error : "+DBFunctions.exceptions;
			}
		}
		
		private double Total(double precio, double cantidad ,double descuento, double iva)
		{
			double total = 0;
	        total = cantidad*precio;
			total = total-Math.Round(total*(descuento/100),0);
			total = total+Math.Round(total*(iva/100),0);
			return(total);
		}

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

        protected void CambioAlmacen(Object Sender, EventArgs E)
        {
            llenarVendedores();
            if (ViewState["TIPOPRO"] != null)
                CambiarPrefijo();
        }

        private void llenarVendedores()
        {
            String sql = @"SELECT pv.pven_codigo,  
                    pv.pven_nombre  
              FROM  pvendedor pv  
              INNER JOIN pvendedoralmacen pva ON pva.pven_codigo = pv.pven_codigo  
              WHERE pva.palm_almacen = '" + ddlAlmacen.SelectedValue + @"'  
              AND   (pv.tvend_codigo = 'VM' OR pv.tvend_codigo = 'TT')  
              AND   pv.pven_vigencia = 'V'  
              ORDER BY pv.PVEN_NOMBRE";

            Utils.FillDll(ddlVendedor, sql, true);
            if (ddlVendedor.Items.Count == 0)
            {
                Utils.MostrarAlerta(Response, "NO hay vendedores asignados a la sede");
                //Page.Visible = false;
                return;
            }
        }

        private void CambiarPrefijo() {
            string Tipo = Request.QueryString["actor"];
            string almacen = ddlAlmacen.SelectedValue;
            string prE = "";
            string sql = "";
            DatasToControls bind = new DatasToControls();
            
            if (Tipo == "P")
            {
                prE = "NP";
                sql = "SELECT p.pdoc_codigo, p.pdoc_nombre FROM pdocumento as P, pdocumentohecho as PH WHERE p.tdoc_tipodocu = 'NP' and PH.tpro_proceso = 'IP' AND P.PDOC_CODIGO = PH.PDOC_CODIGO AND PH.PALM_ALMACEN='" + almacen + "' ";
                bind.PutDatasIntoDropDownList(ddlCodigo,sql);
                imgPrefijo.Attributes.Add("OnClick", "ModalDialog(" + ddlCodigo.ClientID + ",'SELECT p.pdoc_codigo, p.pdoc_nombre FROM pdocumento as P, pdocumentohecho as PH WHERE p.tdoc_tipodocu = \\'NP\\' and PH.tpro_proceso = \\'IP\\' AND P.PDOC_CODIGO = PH.PDOC_CODIGO AND PH.PALM_ALMACEN=\\'" + almacen + "\\'' ,new Array() );"); 
            }
            else
            {
                prE = ViewState["TIPOPRO"].ToString();
                sql = "SELECT p.pdoc_codigo, p.pdoc_nombre FROM pdocumento as P, pdocumentohecho as PH WHERE (p.tdoc_tipodocu = '" + ViewState["TIPONIT"].ToString() + "') and PH.tpro_proceso = '" + ViewState["TIPOPRO"] + "' AND P.PDOC_CODIGO = PH.PDOC_CODIGO AND PH.PALM_ALMACEN='" + almacen + "' ";
                bind.PutDatasIntoDropDownList(ddlCodigo, sql);
                imgPrefijo.Attributes.Add("OnClick", "ModalDialog(" + ddlCodigo.ClientID + ",'SELECT p.pdoc_codigo, p.pdoc_nombre FROM pdocumento as P, pdocumentohecho as PH WHERE (p.tdoc_tipodocu = \\'" + ViewState["TIPONIT"].ToString() + "\\') and PH.tpro_proceso = \\'" + ViewState["TIPOPRO"] + "\\' AND P.PDOC_CODIGO = PH.PDOC_CODIGO AND PH.PALM_ALMACEN=\\'" + almacen + "\\'' ,new Array() );");
            }
            lblNumDev.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo= '" + ddlCodigo.SelectedValue + "'");//El numero de la devolucion

            if (!DBFunctions.RecordExist(sql))
                Utils.MostrarAlerta(Response, "Debe definir al menos un prefijo del tipo '" + prE + "");
       
        }

        protected void ddlCodigo_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblNumDev.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo= '" + ddlCodigo.SelectedValue + "'");//El numero de la devolucion
        }

        protected void Check_Clicked(object sender, EventArgs e)
        {
            int nF;
            string mensaje = "";
            CheckBox chkSel = (CheckBox)sender;
            nF = ((DataGridItem)chkSel.Parent.Parent).ItemIndex;
            if (chkSel.Checked)
            { 
                for (int i = 0; i < dgItems.Items.Count; i++)
                {
                    int cantActual = Convert.ToInt32(DBFunctions.SingleData("select dbxschema.cantactl('" + dgItems.Items[i].Cells[2].Text + "'," + DBFunctions.SingleData("SELECT pano_ano FROM cinventario") + ") FROM dbxschema.mitems where mite_codigo='" + dgItems.Items[i].Cells[2].Text + "'"));
                    int cantf = Convert.ToInt32(dgItems.Items[i].Cells[5].Text);
                    if (Tipo == "C")
                    {
                        if (cantf <= Convert.ToInt16(dgItems.Items[i].Cells[5].Text))
                        {                            
                            ((CheckBox)dgItems.Items[i].Cells[11].FindControl("cbRows")).Checked = true;
                            dgItems.Items[i].Cells[6].Text = dgItems.Items[i].Cells[5].Text;
                            valorNeto += ((Convert.ToDouble(dtSource.Rows[i][4]) - (Convert.ToDouble(dtSource.Rows[i][4]) * (Convert.ToDouble(dtSource.Rows[i][5]) / 100))) * Convert.ToDouble(dgItems.Items[i].Cells[5].Text));
                            valorIva += ((Convert.ToDouble(dtSource.Rows[i][4]) - (Convert.ToDouble(dtSource.Rows[i][4]) * (Convert.ToDouble(dtSource.Rows[i][5]) / 100))) * Convert.ToDouble(dgItems.Items[i].Cells[5].Text)) * (Convert.ToDouble(dtSource.Rows[i][6]) / 100);
                            numeroItems += Convert.ToDouble(dgItems.Items[i].Cells[5].Text);
                        }
                        else
                            Utils.MostrarAlerta(Response, "Cantidad para Devolución Inválida!");
                    }
                    else
                if (Tipo == "P")
                    {
                        if (cantf <= cantActual)
                        {
                            if (cantf <= Convert.ToDouble(dtSource.Rows[i][2]))
                            {                                
                                ((CheckBox)dgItems.Items[i].Cells[11].FindControl("cbRows")).Checked = true;
                                dgItems.Items[i].Cells[6].Text = dgItems.Items[i].Cells[5].Text;
                                valorNeto += ((Convert.ToDouble(dtSource.Rows[i][4]) - (Convert.ToDouble(dtSource.Rows[i][4]) * (Convert.ToDouble(dtSource.Rows[i][5]) / 100))) * Convert.ToDouble(dgItems.Items[i].Cells[5].Text));
                                valorIva += ((Convert.ToDouble(dtSource.Rows[i][4]) - (Convert.ToDouble(dtSource.Rows[i][4]) * (Convert.ToDouble(dtSource.Rows[i][5]) / 100))) * Convert.ToDouble(dgItems.Items[i].Cells[5].Text)) * (Convert.ToDouble(dtSource.Rows[i][6]) / 100);
                                numeroItems += Convert.ToDouble(dgItems.Items[i].Cells[5].Text);
                            }
                            else
                                Utils.MostrarAlerta(Response, "Cantidad para Devolución Inválida!");
                        }
                        else
                            Utils.MostrarAlerta(Response, "No hay Cantidad Disponible para la Devolución!");
                    }
                    //if (cantActual < Convert.ToInt16(dgItems.Items[i].Cells[5].Text))
                    //{
                    //    mensaje += "Cantidad para Devolución Inválida en el item "+ dgItems.Items[i].Cells[2].Text + "!  ";
                    //    ((CheckBox)dgItems.Items[i].Cells[11].FindControl("cbRows")).Enabled = false;
                    //}
                    //else
                    //{
                    //    ((CheckBox)dgItems.Items[i].Cells[11].FindControl("cbRows")).Checked = true;
                    //    dgItems.Items[i].Cells[6].Text = dgItems.Items[i].Cells[5].Text;
                    //    valorNeto += ((Convert.ToDouble(dtSource.Rows[i][4]) - (Convert.ToDouble(dtSource.Rows[i][4]) * (Convert.ToDouble(dtSource.Rows[i][5]) / 100))) * Convert.ToDouble(dgItems.Items[i].Cells[5].Text));
                    //    valorIva += ((Convert.ToDouble(dtSource.Rows[i][4]) - (Convert.ToDouble(dtSource.Rows[i][4]) * (Convert.ToDouble(dtSource.Rows[i][5]) / 100))) * Convert.ToDouble(dgItems.Items[i].Cells[5].Text)) * (Convert.ToDouble(dtSource.Rows[i][6]) / 100);
                    //    numeroItems += Convert.ToDouble(dgItems.Items[i].Cells[5].Text);
                    //}
                }
               
            }
            else
            { 
                for (int i = 0; i < dgItems.Items.Count; i++)
                {
                    ((CheckBox)dgItems.Items[i].Cells[11].FindControl("cbRows")).Checked = false;
                    dgItems.Items[i].Cells[6].Text = "0";
                }
            }
            if (mensaje != "")
            { Utils.MostrarAlerta(Response, mensaje);
                tbValNeto.Text = valorNeto.ToString("C");
                tbValIva.Text = valorIva.ToString("C");
                txtNumUnid.Text = numeroItems.ToString("N");
                txtNumItem.Text = dtSource.Rows.Count.ToString("N");
                btnAjus.Enabled = (numeroItems > 0);
                txtvaloflet.Text = valorflete.ToString("N");
                txtvaloivaflet.Text = valorivaflete.ToString("N");
            }
            else
            {
                tbValNeto.Text = valorNeto.ToString("C");
                tbValIva.Text = valorIva.ToString("C");
                txtNumUnid.Text = numeroItems.ToString("N");
                txtNumItem.Text = dtSource.Rows.Count.ToString("N");
                btnAjus.Enabled = (numeroItems > 0);
                txtvaloflet.Text = valorflete.ToString("N");
                txtvaloivaflet.Text = valorivaflete.ToString("N");
            }
        }   
    }
}
