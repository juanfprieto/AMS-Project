namespace AMS.Nomina
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
	using AMS.Tools;


	/// <summary>
	///		Descripción breve de AMS_Nomina_RegistroAsistencia.
	/// </summary>
	public class AMS_Nomina_RegistroAsistencia : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.TextBox txtClave;
		protected System.Web.UI.WebControls.DataGrid dgAsistencia;
		protected System.Web.UI.WebControls.DropDownList ddlTarjetasEmpleados;
		protected System.Web.UI.WebControls.Button Btn_Entrada;
		protected System.Web.UI.WebControls.DataGrid dgOperaciones;
		protected System.Web.UI.WebControls.Button Btn_salida;

		private DatasToControls DTC = new DatasToControls();		
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if (!IsPostBack)
			{
				armarlista();
			}						
		}
		

		public void llenarEmpleados (System.Web.UI.WebControls.DropDownList LB)
		{
			DTC.PutDatasIntoDropDownList(LB,"SELECT M.MEMP_CODIEMPL, MT.MEMP_CODIEMPL CONCAT ' ' CONCAT N.MNIT_APELLIDOS CONCAT ' ' CONCAT N.MNIT_NOMBRES FROM DBXSCHEMA.MEMPLEADO M, DBXSCHEMA.MNIT N, DBXSCHEMA.MTARJETARELOJ MT WHERE M.MNIT_NIT=N.MNIT_NIT AND ( MT.MEMP_CODIEMPL=M.MEMP_CODIEMPL) and(mt.tvig_vigencia='V') union Select vende.pven_codigo,pven_nombre from DBXSCHEMA.MTARJETARELOJ mtar,DBXSCHEMA.PVENDEDOR vende where mtar.pven_codigo = vende.pven_codigo and(mtar.tvig_vigencia='V')");			
		}
		
		  public void armarlista()
		{
			 
			llenarEmpleados(ddlTarjetasEmpleados);
		}		
		public bool validar()
		{
			if(txtClave.Text!="")
			{
				//Empleado o_Empleado = new Empleado("='" +ddlTarjetasEmpleados.SelectedValue.ToString() +"'");
				string query=@"Select mtar_clave from DBXSCHEMA.MTARJETARELOJ where (memp_codiempl='"+ddlTarjetasEmpleados.SelectedValue+"') or (pven_codigo='"+ddlTarjetasEmpleados.SelectedValue+"') and (tvig_vigencia = 'V') ";
				string auxClave=DBFunctions.SingleData(query);
				
				if (txtClave.Text==auxClave)
				{						
					return true;
				}
				else					
				return false;
            }
            else Utils.MostrarAlerta(Response, "LA CLAVE NO SE HA DIGITADO");
          
			return false;
			
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
			this.Btn_Entrada.Click += new System.EventHandler(this.Btn_Entrada_Click);
			this.Btn_salida.Click += new System.EventHandler(this.Btn_salida_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void Btn_Entrada_Click(object sender, System.EventArgs e)
		{				
			if(validar())
			{
				string query=@"Select mtar_numero from DBXSCHEMA.MTARJETARELOJ where memp_codiempl='"+ddlTarjetasEmpleados.SelectedValue+"' or (pven_codigo='"+ddlTarjetasEmpleados.SelectedValue+"') and tvig_vigencia = 'V' ";
				string tarjeta=DBFunctions.SingleData(query);  
				string nombre=ddlTarjetasEmpleados.SelectedItem.ToString(); 
				DBFunctions.NonQuery("insert INTO DBXSCHEMA.DTARJETARELOJ VALUES(DEFAULT,'E',"+tarjeta+",CURRENT TIMESTAMP)");
                Utils.MostrarAlerta(Response, "BIENVENIDO  " + nombre + "  QUE TENGA UN BUEN DIA");
				ddlTarjetasEmpleados.ClearSelection();
				
			}else
            Utils.MostrarAlerta(Response, "ERROR EN LA VALIDACION DEL USUARIO");
				//ddlTarjetasEmpleados.ClearSelection();
			   
			}			

		private void Btn_salida_Click(object sender, System.EventArgs e)
		{
			validar();
			if(validar())
			{
				string query=@"Select mtar_numero from DBXSCHEMA.MTARJETARELOJ where memp_codiempl='"+ddlTarjetasEmpleados.SelectedValue+"'and tvig_vigencia = 'V' ";
				string tarjeta=DBFunctions.SingleData(query);

				string nombre=ddlTarjetasEmpleados.SelectedItem.ToString();
				DBFunctions.NonQuery("insert INTO DBXSCHEMA.DTARJETARELOJ VALUES(DEFAULT,'S',"+tarjeta+",CURRENT TIMESTAMP)");
                Utils.MostrarAlerta(Response, "" + nombre + "GRACIAS POR SER UNA PARTE IMPORTANTE PARA LA EMPRESA");
				
			}
			else
            Utils.MostrarAlerta(Response, "ERROR EN LA VALIDACION DEL USUARIO");
			ddlTarjetasEmpleados.ClearSelection();
		
		}

		
	}
}
