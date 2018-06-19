namespace AMS.Produccion
{
	using System;
	using System.Web;
	using System.Data;
	using System.Configuration;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Web.Security;
	using System.Collections;
	using System.ComponentModel;
	using System.Drawing;
	using System.Web.SessionState;
	using AMS.DB;
	using AMS.Forms;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Produccion_ArbolEnsamble.
	/// </summary>
	public partial class AMS_Produccion_ArbolEnsamble : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!Page.IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				if(Request.QueryString["ens"]!=null)
                    bind.PutDatasIntoDropDownList(ddlEnsambles, "SELECT pens_codigo, PCAT_CODIGO concat ' [' concat pens_descripcion concat ']'  FROM PENSAMBLEPRODUCCION WHERE PENS_VIGENTE='S' AND PCAT_CODIGO IS NOT NULL ORDER BY pens_descripcion;");
				else
                    bind.PutDatasIntoDropDownList(ddlEnsambles, "SELECT pens_codigo, MITE_CODIGO concat ' [' concat pens_descripcion concat ']' FROM PENSAMBLEPRODUCCION WHERE PENS_VIGENTE='S' AND MITE_CODIGO IS NOT NULL ORDER BY MITE_CODIGO;");
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

		}
		#endregion

		//Seleccionar ensamble a graficar
		protected void btnSeleccionar_Click(object sender, System.EventArgs e)
		{

            tvControl.Nodes.Clear();
			//Agregar ensamble raiz
            //plcArbol.Visible = true;
            fldArbol.Visible = true;
			AgregarEnsamble(ddlEnsambles.SelectedValue, null, 0, "", -1);

			//Costeo
			try
			{
				CosteoProduccion costeo;
				string codElem="";
				//USAR PRIMERA PLANTA ENCONTRADA:

				string planta="";
				if(Request.QueryString["ens"]!=null)
					planta=DBFunctions.SingleData("SELECT MPLA_CODIGO FROM MPLANTAS WHERE TPRO_CODIGO='E' FETCH FIRST 1 ROWS ONLY;");
				else
					planta=DBFunctions.SingleData("SELECT MPLA_CODIGO FROM MPLANTAS WHERE TPRO_CODIGO='P' FETCH FIRST 1 ROWS ONLY;");
				//CIF:
				DateTime fechaA=DateTime.Now.AddMonths(-1);
				double CIF=CosteoProduccion.TraerCIF(planta,fechaA.Year,fechaA.Month);
				//Horas laboradas al mes
				string horasMes=DBFunctions.SingleData("SELECT cnom_horaslabxmes from CNOMINA");
				//Carga Prestacional
				double cargaPrestacional=Convert.ToDouble(DBFunctions.SingleData("SELECT CNOM_PORCCARGPRES from CNOMINA"));
				//Costo hora planta
				double costoHoraPlanta=Convert.ToDouble(DBFunctions.SingleData("SELECT MPLA_COSTOHORA FROM MPLANTAS WHERE MPLA_CODIGO='"+planta+"';"));
				//Capacidad Planta
				double capacidadPlanta=Convert.ToDouble(DBFunctions.SingleData("SELECT MPLA_CAPACIDAD FROM MPLANTAS WHERE MPLA_CODIGO='"+planta+"';"));
				
				//Produccion
				if(Request.QueryString["ens"]==null)
				{
					codElem=DBFunctions.SingleData("select mite_codigo from PENSAMBLEPRODUCCION "+
						"where pens_codigo='"+ddlEnsambles.SelectedValue+"';");
					costeo=new CosteoProduccion(
						codElem, 1,
						ddlEnsambles.SelectedValue, TipoCosteo.Produccion,
						CIF, horasMes, costoHoraPlanta, capacidadPlanta,cargaPrestacional);
				}
				//Ensambles
				else
				{
					codElem=DBFunctions.SingleData("select pcat_codigo from PENSAMBLEPRODUCCION "+
						"where pens_codigo='"+ddlEnsambles.SelectedValue+"';");
					costeo=new CosteoProduccion(
						codElem, 1,
						ddlEnsambles.SelectedValue, TipoCosteo.Ensamble,
						CIF, horasMes, costoHoraPlanta, capacidadPlanta,cargaPrestacional);
				}
				lblCosto.Text=costeo.Calcular().ToString("C");
				if(costeo.error.Length>0)
                Utils.MostrarAlerta(Response, "" + costeo.error + "");
				lblCosteo.Text=costeo.debug;
			}
			catch
			{
				lblCosto.Text="Error calculando el costeo.";
			}
		}

		//Agregar un ensamble al arbol
		private void AgregarEnsamble(string ensamble, TreeNode raiz, double cantidad, string itemsAnt, int orden)
		{
			//Consultar ensamble
			DataSet dsEnsamble=new DataSet();
			DBFunctions.Request(dsEnsamble,IncludeSchema.NO,"SELECT * FROM PENSAMBLEPRODUCCION WHERE PENS_CODIGO='"+ensamble+"';");
			DataRow drEnsamble=dsEnsamble.Tables[0].Rows[0];
			TreeNode tn = new TreeNode();

			//Evitar sumar el item varias veces
			string itemsAnteriores=itemsAnt;
			//Evitar loop infinito
			if(itemsAnteriores.IndexOf("<"+ensamble+"/>")>0){
                Utils.MostrarAlerta(Response, "El ensamble " + ensamble + " aparece como subensamble de él mismo o de uno de sus hijos: " + itemsAnteriores + "!");
				return;
			}
			itemsAnteriores+="<"+ensamble+"/>";

			//Ensamble vehiculo
			if(drEnsamble["PCAT_CODIGO"].ToString().Length>0)
			{
				if(orden>-1) tn.Text = orden.ToString("0")+"<br>";
				tn.Text += "<br>"+"["+ensamble+"] "+drEnsamble["PENS_DESCRIPCION"]+"<BR>"+drEnsamble["PCAT_CODIGO"];
				if(cantidad>0)
					tn.Text += "<br>X "+cantidad;
				if(drEnsamble["PENS_PROPIO"].ToString()=="S"){
					tn.ImageUrl = "../img/TreeView/ens_enspropio.gif";}
				else{
					tn.ImageUrl = "../img/TreeView/ens_ensterceros.gif";}
				AgregarElementosEnsamble(ensamble,tn,itemsAnteriores);
			}
			//Ensamble item
			if(drEnsamble["MITE_CODIGO"].ToString().Length>0)
			{
				if(orden>-1) tn.Text = orden.ToString("0")+"<br>";
				tn.Text += "["+ensamble+"] "+drEnsamble["PENS_DESCRIPCION"]+"<br>"+drEnsamble["MITE_CODIGO"];
				if(cantidad>0)
					tn.Text += "<br>X "+cantidad;
				if(drEnsamble["PENS_PROPIO"].ToString()=="S")
				{
					tn.ImageUrl = "../img/TreeView/ens_prodpropio.gif";}
				else
				{
					tn.ImageUrl = "../img/TreeView/ens_prodterceros.gif";}
				AgregarElementosEnsamble(ensamble,tn,itemsAnteriores);
			}
			tn.Expanded=true;
			if(raiz==null)
                tvControl.Nodes.Add(tn);
			else
				raiz.ChildNodes.Add(tn);
		}
		//Agrega los elementos del ensamble
		private void AgregarElementosEnsamble(string ensamble,TreeNode raiz,string itemsAnt)
		{
			bool items=true,operaciones=true;
			int nItem=0,nOperacion=0,oItem=0,oOperacion=0;

			//Consultar Items
			DataSet dsItems=new DataSet();
			DBFunctions.Request(dsItems,IncludeSchema.NO,"SELECT * FROM MENSAMBLEPRODUCCIONITEMS WHERE PENS_CODIGO='"+ensamble+"' ORDER BY MENS_NIVEL;");
			
			//Consultar Operaciones
			DataSet dsOperaciones=new DataSet();
			DBFunctions.Request(dsOperaciones,IncludeSchema.NO,"SELECT * FROM MENSAMBLEPRODUCCIONOPERACIONES WHERE PENS_CODIGO='"+ensamble+"' ORDER BY MENS_NIVEL;");
			
			while(items||operaciones)
			{
				if(nItem>=dsItems.Tables[0].Rows.Count)
					items=false;
				if(nOperacion>=dsOperaciones.Tables[0].Rows.Count)
					operaciones=false;

				if(items) oItem=Convert.ToInt16(dsItems.Tables[0].Rows[nItem]["MENS_NIVEL"]);
				if(operaciones) oOperacion=Convert.ToInt16(dsOperaciones.Tables[0].Rows[nOperacion]["MENS_NIVEL"]);
				
				bool ensambleT=false;

				if(items)
					if(!operaciones || (oItem<=oOperacion)){
						DataRow drItem=dsItems.Tables[0].Rows[nItem];
						string ensA=DBFunctions.SingleData(
							"SELECT PENS_CODIGO FROM PENSAMBLEPRODUCCION "+
							"WHERE MITE_CODIGO='"+drItem["MITE_CODIGO"]+"' AND PENS_VIGENTE='S';");
						ensambleT=DBFunctions.SingleData(
							"SELECT TPRO_CODIGO "+
							"FROM MENSAMBLEPRODUCCIONITEMS "+
							"WHERE PENS_CODIGO='"+ensamble+"' AND MITE_CODIGO='"+drItem["MITE_CODIGO"].ToString()+"';").Equals("E");
						if(ensA.Length>0 && !ensambleT)
							AgregarEnsamble(ensA,raiz,Convert.ToDouble(drItem["MENS_CANTIDAD"]),itemsAnt,Convert.ToInt16(drItem["MENS_NIVEL"]));
						else
							AgregarItem(drItem["MITE_CODIGO"].ToString(), Convert.ToDouble(drItem["MENS_CANTIDAD"]), raiz,Convert.ToInt16(drItem["MENS_NIVEL"]));
						nItem++;
					}

				if(operaciones)
					if(!items || (oOperacion<=oItem))
					{
						DataRow drOperacion=dsOperaciones.Tables[0].Rows[nOperacion];
						AgregarOperacion(drOperacion["PTEM_OPERACION"].ToString(),drOperacion["MMAQ_CODIGO"].ToString(),drOperacion["PCAR_CODICARGO"].ToString(),raiz,Convert.ToInt16(drOperacion["MENS_NIVEL"]));
						nOperacion++;
					}
			}
		}

		//Agrega operacion a un nodo
		private void AgregarOperacion(string operacion, string maquina, string cargo, TreeNode raiz, int orden)
		{
			DataSet dsOperacion=new DataSet();
			DBFunctions.Request(dsOperacion,IncludeSchema.NO,"SELECT * FROM PTEMPARIOPRODUCCION WHERE PTEM_OPERACION='"+operacion+"';");
			DataSet dsMaquina=new DataSet();
			DBFunctions.Request(dsMaquina,IncludeSchema.NO,"SELECT * FROM MMAQUINAS WHERE MMAQ_CODIGO='"+maquina+"';");
			string cargoT=DBFunctions.SingleData("SELECT PCAR_NOMBCARG FROM pcargoempleado WHERE PCAR_CODICARGO='"+cargo+"';");
			DataRow drOperacion=dsOperacion.Tables[0].Rows[0];
			TreeNode tn=new TreeNode();
			tn.ImageUrl = "../img/TreeView/ens_operacion.gif";
			tn.Text = orden.ToString("0")+"<br>";
			tn.Text+="["+operacion+"] "+drOperacion["PTEM_DESCRIPCION"]+"<br>"+drOperacion["PTEM_TIEMPOESTANDAR"]+" segs.";
			if(dsMaquina.Tables[0].Rows.Count>0)
				tn.Text+="<br>"+maquina+": "+dsMaquina.Tables[0].Rows[0]["MMAQ_FUNCIONES"];
			tn.Text+="<br>"+cargoT;
			raiz.ChildNodes.Add(tn);
		}

		//Agregar un item a un nodo
		private void AgregarItem(string item, double cantidad, TreeNode raiz, int orden)
		{
			DataSet dsItem=new DataSet();
			DBFunctions.Request(dsItem,IncludeSchema.NO,"SELECT * FROM MITEMS MI, PUNIDAD PU WHERE PU.PUNI_CODIGO=MI.PUNI_CODIGO AND MI.MITE_CODIGO='"+item+"';");
			DataRow drItem=dsItem.Tables[0].Rows[0];
			TreeNode tn=new TreeNode();
			tn.ImageUrl = "../img/TreeView/ens_item.gif";
			tn.Text = orden.ToString("0")+"<br>";
			tn.Text += "["+item+"] "+drItem["MITE_NOMBRE"]+"<br>X "+cantidad+" ("+drItem["PUNI_NOMBRE"].ToString()+")";
			raiz.ChildNodes.Add(tn);
		}
	}
}
