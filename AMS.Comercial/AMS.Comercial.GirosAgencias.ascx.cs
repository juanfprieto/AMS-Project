namespace AMS.Comercial
{
	using System.IO;
	using System.Text;
	using System.Collections;
	using System.ComponentModel;
	using System.Globalization;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.Mail;
	using System.Web.UI;
	using System;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
	using AMS.Tools;
	using Ajax;
	//Realizado por:
	//Ing.German Lopera
	//2007-03-16
	/// <summary>
	///		Descripción breve de AMS_Comercial_GirosAgencias.
	/// </summary>
	public class AMS_Comercial_GirosAgencias : System.Web.UI.UserControl
	{
		//esta clase realiza giros directos entre agencias
		//basicamente funciona como una consignacion entre agencias
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label CodigoLabel;
		protected System.Web.UI.WebControls.Label FechaLabel;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.TextBox NombreEmisor;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.TextBox TelEmisor;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.TextBox CedulaEmisor;
		protected System.Web.UI.WebControls.TextBox NombreDestinatario;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.TextBox CedulaDestinatario;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.TextBox TelDestinatario;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.TextBox ValorGiro;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.TextBox CostoGiro;
		protected System.Web.UI.WebControls.DropDownList Destino;
		protected System.Web.UI.WebControls.Button Guardar;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.Label GirosLabel;
		protected System.Web.UI.WebControls.Button Calcular;
		protected System.Web.UI.WebControls.DropDownList Origen;
		string ValorFormato=null;
		
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.TextBox ValorTotal;
		double valor=0;
		double valor2=0;
		string ValorFormato2=null;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.Label Label16;
		protected System.Web.UI.WebControls.Label Label17;
		double saldo=0;
		protected System.Web.UI.WebControls.RegularExpressionValidator RegularExpressionValidator1;
		protected System.Web.UI.WebControls.RegularExpressionValidator RegularExpressionValidator2;
		protected System.Web.UI.WebControls.RegularExpressionValidator RegularExpressionValidator3;
		protected System.Web.UI.WebControls.RegularExpressionValidator RegularExpressionValidator4;
		protected System.Web.UI.WebControls.RegularExpressionValidator RegularExpressionValidator5;
		double porcentaje=0;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			// cargamos datos relevantes como la fecha
			// el codigo del giro
			// la oficina de donde se realiza el giro y la oficina a donde vamos consginar el giro
			
			
			if(!Page.IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				//////////////
				int consecutivo=0;

				string consecutivo1=null;
				consecutivo1=DBFunctions.SingleData("select max(MGIRO_CODGIRO) from DBXSCHEMA.MGIRO");
				//esta pequeña condicion verifica el codigo de la reserva
				//en caso de que sea null que significa que es el primer ingreso
				//se hace una operacion para no generar el error de imput string que se genera
				//por recibir un NULL,con esto podemos solucionarlo
				if(consecutivo1.Equals("null")||consecutivo1.Equals(""))
					consecutivo=0;
				else
				{
					consecutivo=Convert.ToInt32(DBFunctions.SingleData("select max(MGIRO_CODGIRO) from DBXSCHEMA.MGIRO"));
					
				}
				consecutivo=consecutivo+1;
				/////////////
				FechaLabel.Text= DateTime.Now.ToString("yyyy-MM-dd");
				CodigoLabel.Text=consecutivo.ToString();
				//esta condicion,se usa para verificar que hallan datos en la consulta para llenar el dropdown
				if(Origen.Items.Count==0)
				{
					bind.PutDatasIntoDropDownList(Origen,"select MOFI_DESCRIPCION from DBXSCHEMA.MOFICINA ORDER BY MOFI_DESCRIPCION");
					ListItem it=new ListItem("--Seleccione Una Opcion --","0");
					Origen.Items.Insert(0,it);
				}
				if(Destino.Items.Count==0)
				{
					bind.PutDatasIntoDropDownList(Destino,"select MOFI_DESCRIPCION from DBXSCHEMA.MOFICINA ORDER BY MOFI_DESCRIPCION");
					ListItem it=new ListItem("--Seleccione Una Opcion --","0");
					Destino.Items.Insert(0,it);
				}
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		

		public void Calcular_Click(object sender, System.EventArgs e)
		{
			
			saldo=Convert.ToDouble(ValorGiro.Text.ToString());
			porcentaje=Convert.ToDouble(DBFunctions.SingleData("select PORCENTAJE_GIRO FROM DBXSCHEMA.TPORCENTAJEGIRO"));
			//para generar elvalor que se cobra sobre el giro 
			//se hace la siguiente operacion,se saca el porcentaje de la  tabla
			//TPORCENTAJEGIRO y se le suma al valor ingresado
			valor=saldo;
			valor2=saldo+(saldo*(porcentaje/100));
			valor=(valor*(porcentaje/100));
			ValorFormato=String.Format("{0:C}",valor);
			ValorFormato2=String.Format("{0:C}",valor2);
			//se muestran 2 campos, el primero es el del valor del giro osea el 5%
			//el segundo es el valor del giro + le valor del porcentaje
			//este ultimo valor es el que debe recepcionar el cajero o el operario
			CostoGiro.Text=ValorFormato.ToString();
			ValorTotal.Text=ValorFormato2.ToString();
			//estas 2 ultimas instrucciones son para darle formato de moneda
			//a ambos campos
		}

		public void Guardar_click(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			string AgenciaOrigen=null;
			string AgenciaDestino=null;
			//esta funcion guarda los datos relevantes sobre el giro,datos del destinatario,del emisor,y el monto del giro
			porcentaje=Convert.ToDouble(DBFunctions.SingleData("select PORCENTAJE_GIRO FROM DBXSCHEMA.TPORCENTAJEGIRO"));
			valor=Convert.ToDouble(ValorGiro.Text.ToString());
			valor=(valor*(porcentaje/100));
			
			/////////////////////////////////
			AgenciaOrigen=DBFunctions.SingleData("select MOFI_CODIGO from DBXSCHEMA.MOFICINA WHERE MOFI_DESCRIPCION ='"+Origen.SelectedValue.ToString()+"' ");
			AgenciaDestino=DBFunctions.SingleData("select MOFI_CODIGO from DBXSCHEMA.MOFICINA WHERE MOFI_DESCRIPCION ='"+Destino.SelectedValue.ToString()+"' ");
			DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MGIRO VALUES(DEFAULT,'"+FechaLabel.Text.ToString()+"','"+AgenciaOrigen.ToString()+"','"+AgenciaDestino.ToString()+"','"+NombreEmisor.Text.ToString()+"','"+CedulaEmisor.Text.ToString()+"',"+TelEmisor.Text.ToString()+",'"+NombreDestinatario.Text.ToString()+"','"+CedulaDestinatario.Text.ToString()+"',"+TelDestinatario.Text.ToString()+","+ValorGiro.Text.ToString()+","+valor+","+2+" )");//query para insertar directo a la base de datos.
			//estas dos ultimas inserciones se hacen para meter los usuarios frecuentes de este servicio
			DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.TUSUARIOREMESA VALUES('"+NombreEmisor.Text.ToString()+"') ");
			DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.TUSUARIOREMESA VALUES('"+NombreDestinatario.Text.ToString()+"') ");
			//DB.Text=DBFunctions.exceptions;
            Utils.MostrarAlerta(Response, "Giro Realizado");
		}
	}
}
