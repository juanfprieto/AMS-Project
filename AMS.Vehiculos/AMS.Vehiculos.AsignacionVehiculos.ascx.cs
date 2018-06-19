// created on 03/02/2005 at 10:51
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Timers;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using Ajax;
using AMS.Tools;

namespace AMS.Vehiculos
{
	public partial class AsignacionVehiculos : System.Web.UI.UserControl
	{
		#region Variables

		protected System.Web.UI.WebControls.Image imglupa;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected string vin;
		protected System.Web.UI.WebControls.DropDownList Dropdownlist2;
		protected DataTable vehis;
		protected DatasToControls bind = new DatasToControls();
        protected int flag = 0;

		#endregion

		#region Metodos

		protected void cambiarFondoCeldas(DataSet vehiculos)
		{
			for (int i=0;i<vehiculos.Tables[0].Rows.Count;i++)
			{
				DataRow fila = vehiculos.Tables[0].Rows[i];

				bool valido = true;

				if (fila["PCOL_CODRGB"] == DBNull.Value)
				{
                    string mensaje =string.Format("El código RGB para el color [{0}] no ha sido especificado en la lista de colores. Revise Por Favor!",fila["PCOL_DESCRIPCION"]);
                    Utils.MostrarAlerta(Response, mensaje);
					valido = false;
				}

				if ((valido) && (fila["PCOL_CODRGB"].ToString().Length != 6))
				{
                    string mensaje1 =string.Format("El formato del código RGB para el color [{0}] no es valido. Revise Por Favor en la Lista de Colores!",fila["PCOL_DESCRIPCION"]);
                    Utils.MostrarAlerta(Response, mensaje1);
					valido = false;
				}

				if (valido)
				{
                    int red, green, blue;
                    try {red = System.Int32.Parse(fila["PCOL_CODRGB"].ToString().Substring(0,2),System.Globalization.NumberStyles.AllowHexSpecifier);}
					catch {red = 0;}
                    try {green = System.Int32.Parse(fila["PCOL_CODRGB"].ToString().Substring(2,2),System.Globalization.NumberStyles.AllowHexSpecifier);}
					catch {green = 0;}
                    try { blue = System.Int32.Parse(fila["PCOL_CODRGB"].ToString().Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);}
					catch {blue = 0;} 
                    try {dgVehiculos.Items[i].Cells[2].BackColor = Color.FromArgb(red,green,blue);} 
					catch {dgVehiculos.Items[i].Cells[2].BackColor = Color.White;}
				}
			}
		}

		protected void Preparar_Tabla_Vehis()
		{
			vehis = new DataTable();
			vehis.Columns.Add(new DataColumn("vinVehiculo",System.Type.GetType("System.String"))); //0
			vehis.Columns.Add(new DataColumn("color",System.Type.GetType("System.String"))); //1
			vehis.Columns.Add(new DataColumn("diasInv",System.Type.GetType("System.String"))); //2

		}
		
		protected void ingresarDatos(string vinVehiculo,string color,string diasInv)
		{
			DataRow fila = vehis.NewRow();
			fila["vinVehiculo"] = vinVehiculo;
			fila["color"] = color;
			fila["diasInv"] = diasInv;
			vehis.Rows.Add(fila);
			dgVehiculos.DataSource = vehis;
			dgVehiculos.DataBind();
            
			DatasToControls.Aplicar_Formato_Grilla(dgVehiculos);
			DatasToControls.JustificacionGrilla(dgVehiculos,vehis);
		}
		
		protected void Llenar_Datos_Pedido()
		{
			if(numeroPedido.Items.Count==0)return;
			nitPrincipal.Text    = DBFunctions.SingleData("SELECT mnit_nit FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedido.SelectedValue.ToString()+"' AND mped_numepedi="+numeroPedido.SelectedItem.ToString()+"");
			nombrePrincipal.Text = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit='"+nitPrincipal.Text+"'");
			nitAlterno.Text      = DBFunctions.SingleData("SELECT mnit_nit2 FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedido.SelectedValue.ToString()+"' AND mped_numepedi="+numeroPedido.SelectedItem.ToString()+"");
			nombreAlterno.Text   = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit='"+nitAlterno.Text+"'");
			//tipoVehiculoPedido.Text = DBFunctions.SingleData("SELECT pcat_codigo FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedido.SelectedValue.ToString()+"' AND mped_numepedi="+numeroPedido.SelectedItem.ToString()+"");
            tipoVehiculoPedido.Text = DBFunctions.SingleData("SELECT mp.pcat_codigo CONCAT ' ' CONCAT	pc.pcat_descripcion FROM mpedidovehiculo mp, pcatalogovehiculo pc WHERE pdoc_codigo='" + prefijoPedido.SelectedValue.ToString() + "' and mp.pcat_codigo = pc.pcat_codigo AND mped_numepedi=" + numeroPedido.SelectedItem.ToString() + "");
            anoModeloPedido.Text = DBFunctions.SingleData("SELECT pano_ano FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedido.SelectedValue.ToString()+"' AND mped_numepedi="+numeroPedido.SelectedItem.ToString()+"");
			colorPrincipal.Text  = DBFunctions.SingleData("SELECT pcol_descripcion FROM pcolor WHERE pcol_codigo=(SELECT pcol_codigo FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedido.SelectedValue.ToString()+"' AND mped_numepedi="+numeroPedido.SelectedItem.ToString()+")");
			colorOpcional.Text   = DBFunctions.SingleData("SELECT pcol_descripcion FROM pcolor WHERE pcol_codigo=(SELECT pcol_codigoalte FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedido.SelectedValue.ToString()+"' AND mped_numepedi="+numeroPedido.SelectedItem.ToString()+")");
		}
		
		protected void Limpiar_Datos_Pedido()
		{
			nitPrincipal.Text       = "";
			nombrePrincipal.Text    = "";
			nitAlterno.Text         = "";
			nombreAlterno.Text      = "";
			tipoVehiculoPedido.Text = "";
			anoModeloPedido.Text    = "";
			colorPrincipal.Text     = "";
			colorOpcional.Text      = "";
		}
		
		protected void Llenar_Datos_Vehiculo(string vin)
		{
			if ((catalogoVehiculo.SelectedValue != string.Empty) || (vinVehiculo.SelectedValue != string.Empty))
			{
                string fechRecepcionCadena;
                string fechDisponibleCadena;
                string diasInvCadena;

                switch(Request.QueryString["acci"])
				{
					case "A":
                        anoModeloVehiculo.Text = DBFunctions.SingleData("SELECT mcat_anomode FROM mcatalogovehiculo WHERE pcat_codigo='" + catalogoVehiculo.SelectedValue.ToString() + "' AND mcat_vin='" + vin + "'");
                        colorVehiculo.Text = DBFunctions.SingleData("select pcol_descripcion from dbxschema.pcolor where pcol_codigo=(SELECT pcol_codigo FROM mcatalogovehiculo WHERE pcat_codigo='" + catalogoVehiculo.SelectedValue.ToString() + "' AND mcat_vin='" + vin + "')");
                        //  claseVehiculo.Text  = DBFunctions.SingleData("SELECT tcla_nombre FROM tclasevehiculo WHERE tcla_clase=(SELECT tcla_codigo FROM mvehiculo WHERE pcat_codigo='" + catalogoVehiculo.SelectedValue.ToString() + "' AND mcat_vin='" + vin + "')");
                        claseVehiculo.Text = DBFunctions.SingleData("SELECT tcla_nombre FROM tclasevehiculo WHERE tcla_clase=(SELECT tcla_codigo FROM mvehiculo WHERE mcat_vin='" + vin + "' and TEST_TIPOESTA IN (10,20))");
                        tipoServicio.Text = DBFunctions.SingleData("Select tser_nombserv from DBXSCHEMA.TSERVICIOVEHICULO where tser_tiposerv=(SELECT tser_tiposerv FROM mcatalogovehiculo WHERE pcat_codigo='" + catalogoVehiculo.SelectedValue.ToString() + "' AND mcat_vin='" + vin + "')");

                        //nuevos
                        //	numeInventario.Text = DBFunctions.SingleData("SELECT mveh_inventario FROM mvehiculo WHERE pcat_codigo='"+catalogoVehiculo.SelectedValue.ToString()+"' AND mcat_vin='"+vin+"'");
                        numeInventario.Text = DBFunctions.SingleData("SELECT mveh_inventario FROM mvehiculo WHERE mcat_vin='" + vin + "' and TEST_TIPOESTA IN (10,20)");
                     
                        //  numeRecepcion.Text  = DBFunctions.SingleData("SELECT mveh_numerece FROM mvehiculo WHERE pcat_codigo='" + catalogoVehiculo.SelectedValue.ToString() + "' AND mcat_vin='" + vin + "'");
                        numeRecepcion.Text = DBFunctions.SingleData("SELECT mveh_numerece FROM mvehiculo WHERE mcat_vin='" + vin + "' and TEST_TIPOESTA IN (10,20)");
                        

                        //	string fechRecepcionCadena = DBFunctions.SingleData("SELECT mveh_fechrece FROM mvehiculo WHERE pcat_codigo='"+catalogoVehiculo.SelectedValue.ToString()+"' AND mcat_vin='"+vin+"'");
                       fechRecepcionCadena = DBFunctions.SingleData("SELECT mveh_fechrece FROM mvehiculo WHERE mcat_vin='" + vin + "' and TEST_TIPOESTA IN (10,20)");
                       if (fechRecepcionCadena != string.Empty)
                            fechRecepcion.Text = System.Convert.ToDateTime(fechRecepcionCadena).ToString("yyyy-MM-dd");

                        //	string fechDisponibleCadena = DBFunctions.SingleData("SELECT mveh_fechdisp FROM mvehiculo WHERE pcat_codigo='"+catalogoVehiculo.SelectedValue.ToString()+"' AND mcat_vin='"+vin+"'");
                        fechDisponibleCadena = DBFunctions.SingleData("SELECT mveh_fechdisp FROM mvehiculo WHERE mcat_vin='" + vin + "' and TEST_TIPOESTA IN (10,20)");
                        if (fechDisponibleCadena != string.Empty)
                            fechDisponible.Text = System.Convert.ToDateTime(fechDisponibleCadena).ToString("yyyy-MM-dd");

                        //	numeManifiesto.Text = DBFunctions.SingleData("SELECT mveh_numemani FROM mvehiculo WHERE pcat_codigo='"+catalogoVehiculo.SelectedValue.ToString()+"' AND mcat_vin='"+vin+"'");
                        numeManifiesto.Text = DBFunctions.SingleData("SELECT mveh_numemani FROM mvehiculo WHERE mcat_vin='" + vin + "' and TEST_TIPOESTA IN (10,20)");
                        
                        motor.Text = DBFunctions.SingleData("SELECT mcat_motor FROM mcatalogovehiculo WHERE pcat_codigo='" + catalogoVehiculo.SelectedValue.ToString() + "' AND mcat_vin='" + vin + "' ");
                        chasis.Text = DBFunctions.SingleData("SELECT mcat_chasis FROM mcatalogovehiculo WHERE pcat_codigo='" + catalogoVehiculo.SelectedValue.ToString() + "' AND mcat_vin='" + vin + "' ");
                        placa.Text = DBFunctions.SingleData("SELECT mcat_placa FROM mcatalogovehiculo WHERE pcat_codigo='" + catalogoVehiculo.SelectedValue.ToString() + "' AND mcat_vin='" + vin + "' ");
                        diasInvCadena = DBFunctions.SingleData("SELECT mveh_fechrece FROM dbxschema.mvehiculo WHERE mcat_vin='" + vin + "' and TEST_TIPOESTA IN (10,20)");
                        if (diasInvCadena != string.Empty)
                        {
                            TimeSpan diasInv = DateTime.Now - (Convert.ToDateTime(diasInvCadena));
                            diasInventario.Text = Convert.ToString(diasInv.Days);
                        }
                        this.vin = numeInventario.Text;
                        Session["vin"] = this.vin;
                    break;
                    case "D":
                    anoModeloVehiculo.Text = DBFunctions.SingleData("SELECT mcat_anomode FROM mcatalogovehiculo WHERE pcat_codigo='" + catalogoVehiculo.SelectedValue.ToString() + "' AND mcat_vin='" + vin + "'");
                    colorVehiculo.Text = DBFunctions.SingleData("select pcol_descripcion from dbxschema.pcolor where pcol_codigo=(SELECT pcol_codigo FROM mcatalogovehiculo WHERE pcat_codigo='" + catalogoVehiculo.SelectedValue.ToString() + "' AND mcat_vin='" + vin + "')");
                    //  claseVehiculo.Text  = DBFunctions.SingleData("SELECT tcla_nombre FROM tclasevehiculo WHERE tcla_clase=(SELECT tcla_codigo FROM mvehiculo WHERE pcat_codigo='" + catalogoVehiculo.SelectedValue.ToString() + "' AND mcat_vin='" + vin + "')");
                    //claseVehiculo.Text = DBFunctions.SingleData("SELECT tcla_nombre FROM tclasevehiculo WHERE tcla_clase=(SELECT tcla_codigo FROM mvehiculo WHERE mcat_vin='" + vin + "' and TEST_TIPOESTA IN (10,20))");
                    claseVehiculo.Text = DBFunctions.SingleData("SELECT tcla_nombre FROM tclasevehiculo WHERE tcla_clase=(SELECT tcla_codigo FROM mvehiculo WHERE mcat_vin='" + vin + "' and TEST_TIPOESTA IN (30))");
                    tipoServicio.Text = DBFunctions.SingleData("Select tser_nombserv from DBXSCHEMA.TSERVICIOVEHICULO where tser_tiposerv=(SELECT tser_tiposerv FROM mcatalogovehiculo WHERE pcat_codigo='" + catalogoVehiculo.SelectedValue.ToString() + "' AND mcat_vin='" + vin + "')");

                    //nuevos
                    //	numeInventario.Text = DBFunctions.SingleData("SELECT mveh_inventario FROM mvehiculo WHERE pcat_codigo='"+catalogoVehiculo.SelectedValue.ToString()+"' AND mcat_vin='"+vin+"'");
                    //numeInventario.Text = DBFunctions.SingleData("SELECT mveh_inventario FROM mvehiculo WHERE mcat_vin='" + vin + "' and TEST_TIPOESTA IN (10,20)");
                    numeInventario.Text = DBFunctions.SingleData("SELECT mveh_inventario FROM mvehiculo WHERE mcat_vin='" + vin + "' and TEST_TIPOESTA IN (30)");

                    //  numeRecepcion.Text  = DBFunctions.SingleData("SELECT mveh_numerece FROM mvehiculo WHERE pcat_codigo='" + catalogoVehiculo.SelectedValue.ToString() + "' AND mcat_vin='" + vin + "'");
                    //    numeRecepcion.Text = DBFunctions.SingleData("SELECT mveh_numerece FROM mvehiculo WHERE mcat_vin='" + vin + "' and TEST_TIPOESTA IN (10,20)");
                    numeRecepcion.Text = DBFunctions.SingleData("SELECT mveh_numerece FROM mvehiculo WHERE mcat_vin='" + vin + "' and TEST_TIPOESTA IN (30)");

                    //	string fechRecepcionCadena = DBFunctions.SingleData("SELECT mveh_fechrece FROM mvehiculo WHERE pcat_codigo='"+catalogoVehiculo.SelectedValue.ToString()+"' AND mcat_vin='"+vin+"'");
                    //   string fechRecepcionCadena = DBFunctions.SingleData("SELECT mveh_fechrece FROM mvehiculo WHERE mcat_vin='" + vin + "' and TEST_TIPOESTA IN (10,20)");
                    fechRecepcionCadena = DBFunctions.SingleData("SELECT mveh_fechrece FROM mvehiculo WHERE mcat_vin='" + vin + "' and TEST_TIPOESTA IN (30)");
                    if (fechRecepcionCadena != string.Empty)
                        fechRecepcion.Text = System.Convert.ToDateTime(fechRecepcionCadena).ToString("yyyy-MM-dd");

                    //	string fechDisponibleCadena = DBFunctions.SingleData("SELECT mveh_fechdisp FROM mvehiculo WHERE pcat_codigo='"+catalogoVehiculo.SelectedValue.ToString()+"' AND mcat_vin='"+vin+"'");
                    //    string fechDisponibleCadena = DBFunctions.SingleData("SELECT mveh_fechdisp FROM mvehiculo WHERE mcat_vin='" + vin + "' and TEST_TIPOESTA IN (10,20)");
                    fechDisponibleCadena = DBFunctions.SingleData("SELECT mveh_fechdisp FROM mvehiculo WHERE mcat_vin='" + vin + "' and TEST_TIPOESTA IN (30)");
                    if (fechDisponibleCadena != string.Empty)
                        fechDisponible.Text = System.Convert.ToDateTime(fechDisponibleCadena).ToString("yyyy-MM-dd");

                    //	numeManifiesto.Text = DBFunctions.SingleData("SELECT mveh_numemani FROM mvehiculo WHERE pcat_codigo='"+catalogoVehiculo.SelectedValue.ToString()+"' AND mcat_vin='"+vin+"'");
                    //   numeManifiesto.Text = DBFunctions.SingleData("SELECT mveh_numemani FROM mvehiculo WHERE mcat_vin='" + vin + "' and TEST_TIPOESTA IN (10,20)");
                    numeManifiesto.Text = DBFunctions.SingleData("SELECT mveh_numemani FROM mvehiculo WHERE mcat_vin='" + vin + "' and TEST_TIPOESTA IN (30)");

                    motor.Text = DBFunctions.SingleData("SELECT mcat_motor FROM mcatalogovehiculo WHERE pcat_codigo='" + catalogoVehiculo.SelectedValue.ToString() + "' AND mcat_vin='" + vin + "' ");
                    chasis.Text = DBFunctions.SingleData("SELECT mcat_chasis FROM mcatalogovehiculo WHERE pcat_codigo='" + catalogoVehiculo.SelectedValue.ToString() + "' AND mcat_vin='" + vin + "' ");
                    placa.Text = DBFunctions.SingleData("SELECT mcat_placa FROM mcatalogovehiculo WHERE pcat_codigo='" + catalogoVehiculo.SelectedValue.ToString() + "' AND mcat_vin='" + vin + "' ");
                    //  string diasInvCadena = DBFunctions.SingleData("SELECT mveh_fechrece FROM dbxschema.mvehiculo WHERE mcat_vin='" + vin + "' and TEST_TIPOESTA IN (10,20)");
                    diasInvCadena = DBFunctions.SingleData("SELECT mveh_fechrece FROM dbxschema.mvehiculo WHERE mcat_vin='" + vin + "' and TEST_TIPOESTA IN (30)");
                    if (diasInvCadena != string.Empty)
                    {
                        TimeSpan diasInv = DateTime.Now - (Convert.ToDateTime(diasInvCadena));
                        diasInventario.Text = Convert.ToString(diasInv.Days);
                    }
                    this.vin = numeInventario.Text;
                    Session["vin"] = this.vin;
                    break;
                }
               
				
			}
		}
		
		protected void Limpiar_Datos_Vehiculo()
		{
			anoModeloVehiculo.Text = "";
			colorVehiculo.Text = "";
			claseVehiculo.Text = "";
			tipoServicio.Text = "";
			numeInventario.Text = "";
			numeRecepcion.Text = "";
			fechRecepcion.Text = "";
			fechDisponible.Text = "";
			numeManifiesto.Text = "";
			motor.Text= "";
			chasis.Text= "";
			placa.Text= "";
			diasInventario.Text="";
		}


		#endregion

		#region Eventos

		protected void Page_Load(object sender, System.EventArgs e)
		{
			Preparar_Tabla_Vehis();
			if(!IsPostBack)
			{
                if (Request.QueryString["pErr"] != null)
                {
                    flag = 1;
                    Utils.MostrarAlerta(Response, "No hay vehiculos disponibles para este catalogo.");
                }
				switch(Request.QueryString["acci"])
				{
					case "A":
					{
						bind.PutDatasIntoDropDownList(prefijoPedido,string.Format(Documento.DOCUMENTOSTIPO,"PC"));
						Cambio_Tipo_Documento(this,null);
						catalogoVehiculo.Enabled = false;
						vinVehiculo.Visible = false;
						lbVinVehiculo.Visible = false;
					}
						break;
					case "D":
					{
						bind.PutDatasIntoDropDownList(prefijoPedido,string.Format(Documento.DOCUMENTOSTIPO,"PC"));
						Cambio_Tipo_Documento(this,null);
					
						// Ahora debemos bloquear los dropdownlist que permiten la carga de los datos.
						catalogoVehiculo.Enabled = false;
						vinVehiculo.Enabled = false;
						botonAccion.Text = "Desasignar";
						panel.Visible=true;
					}
						break;
				}
			}
			else
			{
				if (Session["vin"] != null)
					vin = (string)Session["vin"];
			}
		}

		protected void Cambio_Tipo_Documento(Object  Sender, EventArgs e)
		{
			switch (Request.QueryString["acci"])
			{
				case "A": 
					bind.PutDatasIntoDropDownList(numeroPedido,"SELECT mped_numepedi FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedido.SelectedValue.ToString()+"' AND (test_tipoesta=10) ORDER BY mped_numepedi");
                    bind.PutDatasIntoDropDownList(catalogoVehiculo,CatalogoVehiculos.CATALOGOVEHICULOSRECEPCIONADOS);
                    numeroPedido.Items.Insert(0, new ListItem("Seleccione...", ""));
                    //Cambio_Numero_Documento(this,null);
					break;
				case "D":
				{
					bind.PutDatasIntoDropDownList(numeroPedido,"SELECT mped_numepedi FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedido.SelectedValue.ToString()+"' AND (test_tipoesta=20) ORDER BY mped_numepedi");
                    bind.PutDatasIntoDropDownList(catalogoVehiculo, CatalogoVehiculos.CATALOGOVEHICULOSASIGNADOS);
                    numeroPedido.Items.Insert(0, new ListItem("Seleccione...", ""));
                    //Cambio_Numero_Documento(this,null);
					break;
				}
			}
			if(numeroPedido.Items.Count == 0)
				this.Limpiar_Datos_Pedido();
		}
		
		protected void Cambio_Numero_Documento(Object  Sender, EventArgs e)
		{
			this.Llenar_Datos_Pedido();
			switch (Request.QueryString["acci"])
			{
				case "A":
					DataSet dsCatalog = new DataSet();
                    if (numeroPedido.Items.Count > 0)
                        //Si no existe en catalogo
                        DBFunctions.Request(dsCatalog, IncludeSchema.NO, "select pcat_codigo from (SELECT " +
                                                                        "pcat_codigo, '[' concat pcat_codigo concat '] - [' concat pcat_descripcion concat ']' descripcion " +
                                                                        "FROM dbxschema.pcatalogovehiculo WHERE pcat_codigo IN (Select DISTINCT MC.pcat_codigo from DBXSCHEMA.MVEHICULO mv,  " +
                                                                        "McatalogoVEHICULO MC WHERE test_tipoesta IN (10,20) AND MC.MCAT_VIN = MV.MCat_VIN)) as t1 where pcat_codigo = " +
                                                                        "(Select MPED.PCAT_CODIGO from DBXSCHEMA.MPEDIDOVEHICULO MPED " +
                                                                        "WHERE MPED.PDOC_CODIGO='" + prefijoPedido.SelectedValue.ToString() + "' AND MPED.MPED_NUMEPEDI="+numeroPedido.SelectedItem.ToString()+") " +
                                                                        "order by pcat_codigo");
                    if (dsCatalog.Tables[0].Rows.Count != 0 || flag == 1)
                    {
                        DatasToControls.EstablecerValueDropDownList(catalogoVehiculo, DBFunctions.SingleData("Select PCAT_CODIGO from DBXSCHEMA.MPEDIDOVEHICULO WHERE PDOC_CODIGO='" + prefijoPedido.SelectedValue.ToString() + "' AND MPED_NUMEPEDI=" + numeroPedido.SelectedItem.ToString()));
                    }
                    else
                    {
                        Response.Redirect(indexPage + "?process=Vehiculos.AsignacionVehiculos&acci=A&pErr=1");
                    }
                     
                    Cambio_Catalogo(this,null);
					break;
				case "D":
					if(numeroPedido.Items.Count>0)
                        DatasToControls.EstablecerValueDropDownList(catalogoVehiculo, DBFunctions.SingleData("Select mc.pcat_codigo from DBXSCHEMA.mcatalogovehiculo mc, DBXSCHEMA.mvehiculo mv, DBXSCHEMA.masignacionvehiculo mav WHERE mc.mcat_vin = mv.mcat_vin and mv.mveh_inventario = mav.mveh_inventario and mav.PDOC_CODIGO='" + prefijoPedido.SelectedValue.ToString() + "' AND mav.MPED_NUMEPEDI=" + numeroPedido.SelectedItem.ToString())); 
					Cambio_Catalogo(this,null);				
					break;
			}
		}
		
		protected void Cambio_Catalogo(Object  Sender, EventArgs e)
		{
			switch (Request.QueryString["acci"])
			{
				case "A": 
				{
					bind.PutDatasIntoDropDownList(vinVehiculo,string.Format(Vehiculos.VEHICULOSRECEPCIONADOS,catalogoVehiculo.SelectedValue));	
					Cambio_Vin(this,null); 

					DataSet vehiculos = new DataSet();
					Session.Remove("vin");
					vin="";
                    DBFunctions.Request(vehiculos, IncludeSchema.NO, "SELECT mcat.mcat_vin, pcol_descripcion || ' - ' || mcat.mcat_anomode  as pcol_descripcion, PCOL_CODRGB, mveh_inventario , mcat.pcat_codigo,pcol.pcol_codigo,mveh_fechrece FROM dbxschema.mvehiculo mveh,DBXSCHEMA.MCATALOGOVEHICULO mcat,dbxschema.pcolor pcol WHERE  mveh.mcat_vin =mcat.mcat_vin and mcat.pcol_codigo=pcol.pcol_codigo and MCAT.pcat_codigo='" + catalogoVehiculo.SelectedValue.ToString() + "' AND (test_tipoesta=10 OR test_tipoesta=20) order by mveh_fechrece asc");
					if (vehiculos.Tables[0].Rows.Count!=0)
					{
						for (int i=0;i<vehiculos.Tables[0].Rows.Count;i++)
						{
							TimeSpan diasInv = new TimeSpan();
							diasInv = DateTime.Now-(Convert.ToDateTime(vehiculos.Tables[0].Rows[i][6].ToString()));
							diasInventario.Text = Convert.ToString(diasInv.Days);
							ingresarDatos(vehiculos.Tables[0].Rows[i][0].ToString(),vehiculos.Tables[0].Rows[i][1].ToString(),diasInventario.Text);
						}
						cambiarFondoCeldas(vehiculos);
					}
					else
					{
						dgVehiculos.DataSource = null;
						dgVehiculos.DataBind();	
						vin = "";
						Session.Remove("vin");
					}
					if ((vehiculos.Tables.Count == 1) && (vehiculos.Tables[0].Rows.Count == 0))
						Limpiar_Datos_Vehiculo();
					this.panel.Visible = false;
                    flag = 0;
				}
					break;
				case "D":
				{
					bind.PutDatasIntoDropDownList(vinVehiculo,string.Format(Vehiculos.VEHICULOSASIGNADOS,catalogoVehiculo.SelectedValue));	
					DatasToControls.EstablecerValueDropDownList(vinVehiculo,DBFunctions.SingleData(string.Format("Select MVEH_INVENTARIO from DBXSCHEMA.MASIGNACIONVEHICULO WHERE PDOC_CODIGO = '{0}' AND MPED_NUMEPEDI = {1}",prefijoPedido.SelectedValue.ToString(),numeroPedido.SelectedValue.ToString())));	
					Cambio_Vin(this,null);
					dgVehiculos.Visible = false;
					this.panel.Visible = true; 
				}
					break;
			}
		}

		protected void validar(object sender,DataGridCommandEventArgs e)
		{
			if(((Button)e.CommandSource).CommandName == "Seleccionar")
			{
				this.Llenar_Datos_Vehiculo(dgVehiculos.Items[e.Item.DataSetIndex].Cells[0].Text);
				this.panel.Visible = true;
			}
		}

		protected void Cambio_Vin(Object Sender, EventArgs e)
		{
			Llenar_Datos_Vehiculo(DBFunctions.SingleData(string.Format("Select MCAT_VIN from DBXSCHEMA.MVEHICULO WHERE MVEH_INVENTARIO = {0}",vinVehiculo.SelectedValue.ToString())));
			this.panel.Visible = true;
		}

		protected void Realizar_Accion(Object  Sender, EventArgs e)
		{
			switch(Request.QueryString["acci"])
			{
				case "A":
				{
                    //Obtener si coincide el tipo de servicio. Para aplicar ipoconsumo.
                    DataSet dsTipoServ = new DataSet();
                    DBFunctions.Request(dsTipoServ, IncludeSchema.NO, 
                        "SELECT mp.tser_tiposerv FROM mcatalogovehiculo mc, mpedidovehiculo mp where  mc.pcat_codigo=mp.pcat_codigo " +
                        "and mcat_vin=(select mcat_vin from MVEHICULO WHERE MVEH_INVENTARIO ='" + vin + "') and mp.pdoc_codigo='" + prefijoPedido.SelectedValue + "' and mp.mped_numepedi=" + numeroPedido.SelectedValue + " and (mp.tser_tiposerv=mc.tser_tiposerv or mp.tser_tiposerv='');");
				
                   	//Primero revisamos si los valores de numero pedido y numero de vin no sean vacias
				
					//if((hdnumpedi.Value=="")||(vinVehiculo.Items.Count==0))
					if((numeroPedido.SelectedValue == "") || (vin == "") || (vin == null))
                        Utils.MostrarAlerta(Response, "El número de Pedido es vacio,o no ha seleccionado un vehiculo.Por Favor Revise");
                    else if (dsTipoServ.Tables[0].Rows.Count == 0)
                            Utils.MostrarAlerta(Response, "El tipo de servicio del catálogo seleccionado no coincide con el tipo de servicio de creación de pedido. Por favor seleccione el catálogo adecuado.");
                        else
					    {
						    PedidoCliente pedidoAsignacion = new PedidoCliente();
                            String fechaHoy = DateTime.Now.Date.ToString("yyyy-MM-dd"); ;
                            String usuario = HttpContext.Current.User.Identity.Name.ToLower();

                            if (pedidoAsignacion.Asignar_Vehiculo_Pedido(prefijoPedido.SelectedValue.ToString(), numeroPedido.SelectedValue.ToString(), vin, fechaHoy, usuario))
                                Response.Redirect("" + indexPage + "?process=Vehiculos.AsignacionFacturacionVehiculos");
                            else
                            {
                                lb.Text = pedidoAsignacion.ProcessMsg;
                                Utils.MostrarAlerta(Response, "Error en Asignación");
                            }
                        }
				}
					break;
				case "D":
				{
					//Primero revisamos si el numero de pedido es vacio o no
					if(numeroPedido.SelectedValue == "")
                        Utils.MostrarAlerta(Response, "El número de Pedido es Vacio, Revise Por Favor");
					else
					{
						PedidoCliente pedidoDesasignacion = new PedidoCliente();
						if(pedidoDesasignacion.Desasignar_Vehiculo_Pedido(prefijoPedido.SelectedValue.ToString(),numeroPedido.SelectedValue.ToString(),numeInventario.Text))
							//lb.Text = "<br>bien : "+pedidoDesasignacion.ProcessMsg;
							Response.Redirect("" + indexPage + "?process=Vehiculos.AsignacionFacturacionVehiculos");
						else
							//lb.Text = pedidoDesasignacion.ProcessMsg;
                            Utils.MostrarAlerta(Response, "Error en Desasignación");
					}
				}
					break;
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
