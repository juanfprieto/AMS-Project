using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;

namespace AMS.Comercial
{
	public class ProcesoReservas : System.Web.UI.UserControl
	{
		protected Label idRuta, dscRuta, ciuOrigen, ciuDestino, fchSalida, hrSalida, tipVehiculo, plcVehiculo, capacidad, disponibilidad, total;
		protected DropDownList oficinas;
		protected Label lb;
		protected DataGrid grillaInformativa;
		protected DataTable tablaTiquetes;
		protected System.Web.UI.WebControls.Button recalculo;
		protected System.Web.UI.WebControls.Button comprar;
		protected System.Web.UI.WebControls.TextBox TextBox1;
		protected System.Web.UI.WebControls.TextBox TextBox2;
		protected System.Web.UI.WebControls.DropDownList ddlsubruta;
		protected System.Web.UI.WebControls.Button reservar;
		protected CheckBoxList formaPago;		
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				Session.Clear();
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoCheckBox(formaPago,"SELECT ttip_codigo, ttip_nombre FROM ttipopago");
				this.Llenar_Grilla_Tiquetes(Request.QueryString["Pars"].Split('Z'));
			}
			if(Session["tablaTiquetes"]==null)
			{
				this.Preparar_Tabla_Tiquetes();
				Session["tablaTiquetes"] = tablaTiquetes;
			}
			else if(Session["tablaTiquetes"]!=null)
				tablaTiquetes = ((DataTable)Session["tablaTiquetes"]);
		}
		
		protected void Preparar_Tabla_Tiquetes()
		{
			tablaTiquetes = new DataTable();
			tablaTiquetes.Columns.Add(new DataColumn("NUMSILLA",typeof(string)));
			tablaTiquetes.Columns.Add(new DataColumn("VALORTIQUETE",typeof(double)));
		}
		
		protected void Llenar_Grilla_Tiquetes(string []pars)
		{
			this.Preparar_Tabla_Tiquetes();
			//Cargamos el numero de la ruta
			string numeroRuta = pars[0];
			double valorTotal = 0;
			//Ahora armamos la dupla de fila - columna
			for(int i=1;i<pars.Length;i+=2)
			{
				int numeroFila = System.Convert.ToInt32(pars[i])+1;
				int numeroColumna = System.Convert.ToInt32(pars[i+1])+1;
				//Se verifica si este puesto no se encuentra dentro de dpuestoruta como cancelado, reservado, etc
				if(!DBFunctions.RecordExist("SELECT * FROM dpuestoruta WHERE drut_codigo="+numeroRuta+" AND mpue_posfila="+numeroFila+" AND mpue_poscolumna="+numeroColumna+" AND (test_codigo<>'DN' AND test_codigo<>'OC' AND test_codigo<>'RV')"))
				{
					DataRow fila = tablaTiquetes.NewRow();
					fila["NUMSILLA"] = numeroFila+"-"+numeroColumna;
					fila["VALORTIQUETE"] = System.Convert.ToDouble(DBFunctions.SingleData("SELECT mrut_valor FROM mruta WHERE mrut_codigo=(SELECT mrut_codigo FROM druta WHERE drut_codigo="+numeroRuta+")"));
					valorTotal += System.Convert.ToDouble(DBFunctions.SingleData("SELECT mrut_valor FROM mruta WHERE mrut_codigo=(SELECT mrut_codigo FROM druta WHERE drut_codigo="+numeroRuta+")"));
					tablaTiquetes.Rows.Add(fila);
				}
				else
					lb.Text +="puesto no valido"+numeroFila.ToString()+"-"+numeroColumna.ToString();
			}
			total.Text = valorTotal.ToString("C");
			Session["tablaTiquetes"] = tablaTiquetes;
			this.grillaInformativa.DataSource = tablaTiquetes;
			this.grillaInformativa.DataBind();
			this.Llenar_Datos_Ruta(numeroRuta);
		}
		
		protected void Llenar_Datos_Ruta(string numeroRuta)
		{
			idRuta.Text = numeroRuta;
			dscRuta.Text = DBFunctions.SingleData("SELECT mrut_descripcion FROM mruta WHERE mrut_codigo=(SELECT mrut_codigo FROM druta WHERE drut_codigo="+numeroRuta+")");
			ciuOrigen.Text = DBFunctions.SingleData("SELECT ciudad.tciu_nombre FROM dbxschema.tciudad ciudad,dbxschema.mruta mruta,dbxschema.druta druta WHERE druta.mrut_codigo=mruta.mrut_codigo and mruta.tciu_cod=ciudad.tciu_codigo and druta.drut_codigo="+numeroRuta+"");
			ciuDestino.Text = DBFunctions.SingleData("SELECT ciudad.tciu_nombre FROM dbxschema.tciudad ciudad,dbxschema.mruta mruta,dbxschema.druta druta WHERE druta.mrut_codigo=mruta.mrut_codigo and mruta.tciu_coddes=ciudad.tciu_codigo and druta.drut_codigo="+numeroRuta+"");
			fchSalida.Text = Convert.ToDateTime(DBFunctions.SingleData("SELECT drut_fecha FROM dbxschema.druta WHERE drut_codigo="+numeroRuta+"")).ToString("yyyy-MM-dd");
			hrSalida.Text = DBFunctions.SingleData("SELECT drut_horasal FROM dbxschema.druta WHERE drut_codigo="+numeroRuta+"").ToString();
			tipVehiculo.Text = DBFunctions.SingleData("SELECT pcat_codigo CONCAT ' ' CONCAT pcat_descripcion FROM dbxschema.pcatalogovehiculo WHERE pcat_codigo=(SELECT pcat_codigo FROM dbxschema.mcatalogovehiculo WHERE mcat_placa=(select mcat_placa from dbxschema.mbusafiliado where mcat_placa=(select mcat_placa from dbxschema.druta where drut_codigo="+numeroRuta+")))");
			plcVehiculo.Text = DBFunctions.SingleData("SELECT mcat_placa FROM druta WHERE drut_codigo="+numeroRuta+"");
			capacidad.Text = DBFunctions.SingleData("SELECT pcat.pcat_capacidad FROM dbxschema.pcatalogovehiculo pcat,dbxschema.druta druta,dbxschema.mruta mruta,dbxschema.mbusafiliado mbus,dbxschema.mcatalogovehiculo mcat WHERE druta.drut_codigo="+numeroRuta+" and druta.mrut_codigo=mruta.mrut_codigo and druta.mcat_placa=mbus.mcat_placa and mbus.mcat_placa=mcat.mcat_placa and pcat.pcat_codigo=mcat.pcat_codigo");
			disponibilidad.Text = ((System.Convert.ToInt32(capacidad.Text))-(System.Convert.ToInt32(DBFunctions.SingleData("SELECT COUNT(*) FROM dpuestoruta WHERE drut_codigo="+numeroRuta+" AND (test_codigo<>'LI')").Trim()))).ToString();
			DatasToControls bind1 = new DatasToControls();
			bind1.PutDatasIntoDropDownList(oficinas,"SELECT mofi_codigo, mofi_descripcion FROM dbxschema.moficina");
				
		}
		
		protected void Recalcula_Total(Object  Sender, EventArgs e)
		{
			double valor = 0;
			//debemos recorrer la grilla y verificar cuales si han sido chuleados para ser vendidos
			for(int i=0;i<grillaInformativa.Items.Count;i++)
			{
				if(((CheckBox)grillaInformativa.Items[i].Cells[3].Controls[1]).Checked)
					valor += System.Convert.ToDouble(tablaTiquetes.Rows[i][1]);
			}
			total.Text = valor.ToString("C");
		}
		
		protected void Reservar_Tiquetes(Object  Sender, EventArgs e)
		{
			//Primero debemos verificar que se halla elegido alguna forma de pago
			int i;
			//int j;
			int c=0;
			int ultiTQ = System.Convert.ToInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu FROM dbxschema.pdocumento WHERE pdoc_codigo='TQP'"));
			string numerosTiquetes = "";
			string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
			ArrayList sqlStrings = new ArrayList();
			//Ahora pasamos a verificar la grilla que los campos que se encuentren seleccionados si tengan un nombre y documento valido
				for(i=0;i<grillaInformativa.Items.Count;i++)
				{
					if(((CheckBox)grillaInformativa.Items[i].Cells[3].Controls[1]).Checked)
					{
						//verificamos que el nombre y documento sean diferentes de vacio
						if((((TextBox)grillaInformativa.Items[i].Cells[1].Controls[1]).Text==" ")||(((TextBox)grillaInformativa.Items[i].Cells[2].Controls[1]).Text==" "))
							Response.Write("<script language:javascript>alert('El nombre o documento de algun cliente es vacio. Por favor revise');</script>");
						else
						{
							c+=1;
							string []posicion = tablaTiquetes.Rows[i][0].ToString().Split('-');
							sqlStrings.Add("INSERT INTO dtiquete VALUES("+(ultiTQ+c).ToString()+","+idRuta.Text+",'"+((TextBox)grillaInformativa.Items[i].Cells[1].Controls[1]).Text+"','S','"+this.oficinas.SelectedValue.ToString()+"','SEGURO DE VIAJE PAGO',"+(System.Convert.ToInt32(posicion[0])-1).ToString()+","+(System.Convert.ToInt32(posicion[1])-1).ToString()+",1,'"+ciuOrigen.Text+"','"+ciuDestino.Text+"','"+fchSalida.Text+"','"+hrSalida.Text+"')");
							string ruta=DBFunctions.SingleData("select mbus.mbus_numero from dbxschema.mbusafiliado mbus,dbxschema.druta drut,dbxschema.mruta mrut where drut.drut_codigo=mrut.mrut_codigo and drut.drut_codigo="+idRuta.Text+" and mbus.mcat_placa=drut.mcat_placa");
							string mruta=DBFunctions.SingleData("select mrut_codigo from dbxschema.druta where drut_codigo="+idRuta.Text+"");
							sqlStrings.Add("INSERT INTO dpuestoruta VALUES("+idRuta.Text+","+(System.Convert.ToInt32(posicion[0])-1).ToString()+","+(System.Convert.ToInt32(posicion[1])-1).ToString()+",'RS',"+ruta+","+mruta+",'"+ciuOrigen.Text+"','"+ciuDestino.Text+"')");
							sqlStrings.Add("INSERT INTO TESTADOPUESTO VALUES('RS','RESERVADO',2,"+ruta+","+mruta+")");
							Response.Write("<script language:javascript>alert('Se ha Hecho una Reserva');</script>");
		
							numerosTiquetes += "Z"+(ultiTQ+c).ToString();
						}
					}
				}
				sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu = pdoc_ultidocu+"+c.ToString()+" WHERE pdoc_codigo='TQP'");
				if(DBFunctions.Transaction(sqlStrings))
				{
					Response.Redirect("" + indexPage + "?process=Comercial.VentaTiquetes");
					
				}
				else
					lb.Text +="<BR> Error: Error en La Reserva<br>"+DBFunctions.exceptions;
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