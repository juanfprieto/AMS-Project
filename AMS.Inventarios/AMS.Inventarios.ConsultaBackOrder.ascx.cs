using System;
using System.Data;
using AMS.Forms;
using AMS.DB;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial class ConsultaBackOrder : System.Web.UI.UserControl
	{
		protected DataTable dtConsulta;
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Session.Clear();
			DatasToControls.Aplicar_Formato_Grilla(dgConsulta);
			if(!IsPostBack)
			{
				plFil2.Visible = plFil3.Visible = false;
				DatasToControls bind = new DatasToControls();
 				bind.PutDatasIntoDropDownList(ddlLinea,"SELECT plin_codigo CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem");
 				tbFiltroCodI.Attributes.Add("ondblclick","MostrarRefs("+tbFiltroCodI.ClientID+","+ddlLinea.ClientID+");");
 				tbFiltroCodI.Attributes.Add("onkeyup","ItemMask("+tbFiltroCodI.ClientID+","+ddlLinea.ClientID+");");
 				ddlLinea.Attributes.Add("onchange","ChangeLine("+ddlLinea.ClientID+","+tbFiltroCodI.ClientID+");");
 				//Traemos los prefijos de los pedidos que tienen pendientes
 				bind.PutDatasIntoDropDownList(ddlPrefPed,"SELECT DISTINCT PPE.pped_codigo,PPE.pped_nombre FROM ppedido PPE, dpedidoitem DPI WHERE PPE.pped_codigo=DPI.pped_codigo AND (DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact)>0");
 				bind.PutDatasIntoDropDownList(ddlNumPed,"SELECT DISTINCT DPI.mped_numepedi FROM ppedido PPE, dpedidoitem DPI WHERE PPE.pped_codigo=DPI.pped_codigo AND (DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact)>0 AND DPI.pped_codigo='"+ddlPrefPed.SelectedValue+"'");
			}
		}
		
		protected void PreparardtConsulta(string opcion)
		{
			dtConsulta = new DataTable();
            dtConsulta.Columns.Add(new DataColumn("NIT", System.Type.GetType("System.String")));//0
            dtConsulta.Columns.Add(new DataColumn("NOMBRE CLIENTE", System.Type.GetType("System.String")));//1

            dtConsulta.Columns.Add(new DataColumn("CODIGO REFERENCIA",System.Type.GetType("System.String")));//0
			dtConsulta.Columns.Add(new DataColumn("DESCRIPCION REFERENCIA",System.Type.GetType("System.String")));//1
			if(opcion == "P")
			{
				dtConsulta.Columns.Add(new DataColumn("PREFIJO PEDIDO",System.Type.GetType("System.String")));//2
				dtConsulta.Columns.Add(new DataColumn("NUMERO PEDIDO",System.Type.GetType("System.String")));//3
			}
			if(opcion == "I")
				dtConsulta.Columns.Add(new DataColumn("ALMACEN",System.Type.GetType("System.String")));//2
			dtConsulta.Columns.Add(new DataColumn("CANTIDAD PENDIENTE",System.Type.GetType("System.Double")));//3
			DataSet ds = new DataSet();
			//DBFunctions.Request(ds,IncludeSchema.NO,"SELECT palm_descripcion FROM palmacen WHERE talm_tipoalma='A' ORDER BY palm_almacen");
            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT palm_descripcion FROM palmacen WHERE PCEN_CENTINV IS NOT NULL and tvig_vigencia='V' ORDER BY PALM_DESCRIPCION");
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				dtConsulta.Columns.Add(new DataColumn("DISPONIBILIDAD EN "+ds.Tables[0].Rows[i][0].ToString().ToUpper(),System.Type.GetType("System.Double")));//i
		}
		
		protected void CambioGrupoConsulta(Object Sender, EventArgs E)
		{
			if(rdlstFiltro.SelectedValue == "T")
			{
				plFil1.Visible = true;
				plFil2.Visible = plFil3.Visible = false;
			}
			else if(rdlstFiltro.SelectedValue == "F")
			{
				if(ddlFiltro.SelectedValue == "I")
					plFil2.Visible = true;
				else if(ddlFiltro.SelectedValue == "P")
					plFil3.Visible = true;
				plFil1.Visible = false;
			}
		}
		
		protected void CambioPrefijoPedido(Object Sender, EventArgs E)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlNumPed,"SELECT DISTINCT DPI.mped_numepedi FROM ppedido PPE, dpedidoitem DPI WHERE PPE.pped_codigo=DPI.pped_codigo AND (DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact)>0 AND DPI.pped_codigo='"+ddlPrefPed.SelectedValue+"'");
		}
		
		protected void ConsultarFiltro1(Object Sender, EventArgs E)
		{
			//Debemos primero revisar si vamos a realizar la consulta de todos dentro del filtro seleccionado
			if(rdlstFiltro.SelectedValue == "T")
				RealizarConsulta(0,ddlFiltro.SelectedValue);
		}
		
		protected void ConsultarFiltro2(Object Sender, EventArgs E)
		{
			string codI = "";
			if(!Referencias.Guardar(tbFiltroCodI.Text.Trim(),ref codI,(ddlLinea.SelectedValue.Trim().Split('-'))[1]))
			{
                Utils.MostrarAlerta(Response, "El codigo " + tbFiltroCodI.Text.Trim() + " no es valido para la linea de bodega " + ddlLinea.SelectedItem.Text + ".\\nRevise Por Favor!");
				return;
			}
			if(!Referencias.RevisionSustitucion(ref codI,(ddlLinea.SelectedValue.Trim().Split('-'))[1]))
			{
                Utils.MostrarAlerta(Response, "El codigo " + codI + " no se encuentra registrado.\\nRevise Por Favor!");
				return;
			}
			string codI2 = "";
			Referencias.Editar(codI,ref codI2,(ddlLinea.SelectedValue.Trim().Split('-'))[1]);
			if(codI2 != tbFiltroCodI.Text.Trim())
                Utils.MostrarAlerta(Response, "El codigo " + tbFiltroCodI.Text.Trim() + " se ha sustituido.\\nEl codigo actual es " + codI2 + "!");
			if(!DBFunctions.RecordExist("SELECT * FROM dpedidoitem WHERE mite_codigo='"+codI+"' AND (dped_cantpedi-dped_cantasig-dped_cantfact)>0 AND mped_clasregi='C'"))
			{
                Utils.MostrarAlerta(Response, "El Item " + tbFiltroCodI.Text.Trim() + " no tiene pendientes en ningun pedido!");
				return;
			}
			tbFiltroCodI.Text = codI2;
			RealizarConsulta(1,ddlFiltro.SelectedValue);
		}
		
		protected void ConsultarFiltro3(Object Sender, EventArgs E)
		{
			RealizarConsulta(1,ddlFiltro.SelectedValue);
		}
		
		protected void RealizarConsulta(int opcionGrupo, string opcionFiltro)
		{
			PreparardtConsulta(opcionFiltro);
			string codI = "";
			Referencias.Guardar(tbFiltroCodI.Text.Trim(),ref codI,(ddlLinea.SelectedValue.Trim().Split('-'))[1]);
			//for(int i=0;i<this.dtConsulta.Columns.Count;i++)
			string sqlQuery = "";
			if(opcionGrupo==0 && opcionFiltro=="I")
                sqlQuery = @"SELECT VM.MNIT_NIT,
                                   VM.NOMBRE,
                                   DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo,PLIN.plin_tipo) AS CODIGO,
                                   MIT.mite_nombre,
                                   PAL.palm_descripcion,
                                   SUM(DPI.dped_cantpedi) - SUM(DPI.dped_cantasig) - SUM(DPI.dped_cantfact)
                            FROM dpedidoitem DPI,
                                 mpedidoitem MPI,
                                 mitems MIT,
                                 VMNIT VM,
                                 palmacen PAL,
                                 plineaitem PLIN,
                                 PPEDIDO PPED
                            WHERE PAL.tvig_vigencia = 'V'
                            AND   DPI.mite_codigo = MIT.mite_codigo
                            AND   DPI.mped_clasregi = MPI.mped_claseregi
                            AND   DPI.mnit_nit = MPI.mnit_nit
                            AND   VM.MNIT_NIT = MPI.MNIT_NIT
                            AND   DPI.pped_codigo = MPI.pped_codigo
                            AND   DPI.mped_numepedi = MPI.mped_numepedi
                            AND   MPI.palm_almacen = PAL.palm_almacen
                            AND   DPI.mped_clasregi = 'C'
                            AND   PLIN.plin_codigo = MIT.plin_codigo
                            AND   MPI.PPED_CODIGO = PPED.PPED_CODIGO
                            AND   PPED.TPED_CODIGO <> 'C'
                            GROUP BY VM.MNIT_NIT,
                                     VM.NOMBRE,
                                     DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo,PLIN.plin_tipo),
                                     MIT.mite_nombre,
                                     PAL.palm_descripcion
                            HAVING (SUM(DPI.dped_cantpedi) - SUM(DPI.dped_cantasig) - SUM(DPI.dped_cantfact)) > 0";
			else if(opcionGrupo==0 && opcionFiltro=="P")
				sqlQuery = @"SELECT VM.MNIT_NIT, VM.NOMBRE, DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo,PLIN.plin_tipo) AS CODIGO,
                                   MIT.mite_nombre,
                                   DPI.pped_codigo,
                                   DPI.mped_numepedi,
                                   DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact
                            FROM dpedidoitem DPI,
                                 mitems MIT,
                                  VMNIT VM,
                                 plineaitem PLIN
                            WHERE DPI.mite_codigo = MIT.mite_codigo
                            AND DPI.mped_clasregi = 'C'
                            AND(DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact) > 0
                            AND PLIN.plin_codigo = MIT.plin_codigo
                            AND DPI.MNIT_NIT = VM.MNIT_NIT
                            ORDER BY DPI.pped_codigo,
                                     DPI.mped_numepedi";
			else if(opcionGrupo==1 && opcionFiltro=="I")
                sqlQuery = "SELECT DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo,PLIN.plin_tipo) AS CODIGO, MIT.mite_nombre, PAL.palm_descripcion, SUM(DPI.dped_cantpedi)- SUM(DPI.dped_cantasig)- SUM(DPI.dped_cantfact) FROM dpedidoitem DPI, mpedidoitem MPI, mitems MIT, palmacen PAL, plineaitem PLIN WHERE tvig_vigencia='V' and DPI.mite_codigo=MIT.mite_codigo AND DPI.mped_clasregi=MPI.mped_claseregi AND DPI.mnit_nit=MPI.mnit_nit AND DPI.pped_codigo=MPI.pped_codigo AND DPI.mped_numepedi=MPI.mped_numepedi AND MPI.palm_almacen = PAL.palm_almacen AND DPI.mped_clasregi='C' AND DPI.mite_codigo='" + codI + "' AND MIT.plin_codigo = PLIN.plin_codigo GROUP BY DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo,PLIN.plin_tipo), MIT.mite_nombre, PAL.palm_descripcion HAVING (SUM(DPI.dped_cantpedi)- SUM(DPI.dped_cantasig)- SUM(DPI.dped_cantfact))>0";
			else if(opcionGrupo==1 && opcionFiltro=="P")
				sqlQuery = "SELECT DBXSCHEMA.EDITARREFERENCIAS(DPI.mite_codigo,PLIN.plin_tipo) AS CODIGO, MIT.mite_nombre, DPI.pped_codigo, DPI.mped_numepedi, DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact FROM dpedidoitem DPI, mitems MIT, plineaitem PLIN WHERE DPI.mite_codigo=MIT.mite_codigo AND DPI.mped_clasregi='C' AND DPI.pped_codigo='"+ddlPrefPed.SelectedValue+"' AND DPI.mped_numepedi="+ddlNumPed.SelectedValue+" AND (DPI.dped_cantpedi - DPI.dped_cantasig - DPI.dped_cantfact)>0 AND PLIN.plin_codigo=MIT.plin_codigo ORDER BY DPI.pped_codigo, DPI.mped_numepedi";
			DataSet ds = new DataSet();
			//DBFunctions.Request(ds,IncludeSchema.NO,sqlQuery+";SELECT palm_almacen FROM palmacen WHERE talm_tipoalma='A' ORDER BY palm_almacen");
            DBFunctions.Request(ds, IncludeSchema.NO, sqlQuery + ";SELECT palm_almacen FROM palmacen WHERE PCEN_CENTINV IS NOT NULL and tvig_vigencia='V' ORDER BY PALM_DESCRIPCION");
            string PRU = sqlQuery + ";SELECT palm_almacen FROM palmacen WHERE PCEN_CENTINV IS NOT NULL and tvig_vigencia='V' ORDER BY PALM_DESCRIPCION";
            for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				DataRow fila = this.dtConsulta.NewRow();
				int ultimaColumna = ds.Tables[0].Columns.Count;
				for(int j=0;j<ds.Tables[0].Columns.Count;j++)
				{
					if(ds.Tables[0].Rows[i][j].GetType().ToString() == "System.String")
						fila[j] = ds.Tables[0].Rows[i][j].ToString();
					else if(ds.Tables[0].Rows[i][j].GetType().ToString() == "System.Int32")
						try{fila[j] = Convert.ToInt32(ds.Tables[0].Rows[i][j].ToString().Trim());}catch{fila[j] = 0;}
					else if(ds.Tables[0].Rows[i][j].GetType().ToString() == "System.Double" || ds.Tables[0].Rows[i][j].GetType().ToString() == "System.Decimal")
						try{fila[j] = Convert.ToDouble(ds.Tables[0].Rows[i][j].ToString().Trim());}catch{fila[j] = 0;}
				}
				//Ahora vamos a agregar la disponibilidad de este item por almacen
				for(int j=0;j<ds.Tables[1].Rows.Count;j++)
				{
                    try { fila[ultimaColumna] = Convert.ToDouble(DBFunctions.SingleData("Select MSIA.msal_cantactual from DBXSCHEMA.MSALDOITEMALMACEN MSIA, DBXSCHEMA.CINVENTARIO CI  where MSIA.mite_codigo='" + ds.Tables[0].Rows[i]["CODIGO"].ToString() + "' AND MSIA.palm_almacen='" + ds.Tables[1].Rows[j][0].ToString() + "' and  MSIA.PANO_ANO = CI.PANO_ANO group by MSIA.msal_cantactual, mite_codigo, palm_almacen").Trim()); }
					catch{fila[ultimaColumna] = 0;}
					ultimaColumna+=1;
				}
				this.dtConsulta.Rows.Add(fila);
			}
			dgConsulta.DataSource = dtConsulta;
			dgConsulta.DataBind();
			DatasToControls.JustificacionGrilla(dgConsulta,dtConsulta);
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
	}
}	
