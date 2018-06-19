namespace AMS.Vehiculos
{
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
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Vehiculos_ModificarSeguimientoDiario.
	/// </summary>
	public partial class AMS_Vehiculos_ModificarSeguimientoDiario : System.Web.UI.UserControl
	{
		protected DataSet lineas;
		//JFSC 11022008 Poner en comentario por no ser usado
		//int consecutivo=0;
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!Page.IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				int contador1=0;
				if(contador1==0)
				{
					bind.PutDatasIntoDropDownList(vendedor,"SELECT PVEN_CODIGO,PVEN_NOMBRE from DBXSCHEMA.PVENDEDOR ORDER BY PVEN_NOMBRE");
					ListItem it=new ListItem("--Seleccione Vendedor--","0");
					vendedor.Items.Insert(0,it);
				}
			
			}	
		}
		public void Buscar_Click(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			string PasswordS=password.Text.ToString();
			string PasswordVendedor=DBFunctions.SingleData("select PVEN_CLAVE from DBXSCHEMA.PVENDEDOR WHERE PVEN_CODIGO='"+vendedor.SelectedValue.ToString()+"'");
			if(PasswordS.Equals(PasswordVendedor))
			{
			Panel1.Visible=true;
			int contador2=0;
				if(contador2==0)
				{
					bind.PutDatasIntoDropDownList(registros,"select DVISI_SECUENCIA,CAST(DVISI_SECUENCIA AS CHARACTER(10)) CONCAT '  'CONCAT DVISI_NOMBRE CONCAT '  'CONCAT CAST(DVISI_FECHAVISI AS CHARACTER(10))from DBXSCHEMA.DVISITADIARIACLIENTES where PVEN_CODIGO='"+vendedor.SelectedValue.ToString()+"' ");
					ListItem it=new ListItem("--Seleccione Registro--","0");
					registros.Items.Insert(0,it);				
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

		}
		#endregion

		protected void registros_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			
			Panel2.Visible=true;
			///////////
			lineas = new DataSet();                                
			DBFunctions.Request(lineas,IncludeSchema.NO,"select DVISI_NOMBRE,DVISI_TELEFIJO,DVISI_TELEMOVIL,DVISI_TELEOFICINA,DVISI_EMAIL,DVISI_OBSERVACIONES,DVISI_FECHAVISI,PCAT_CODIGO,PCLAS_CODIGOVENTA,PVEN_CODIGO,PPRO_CODIGO,PRESULC_SECUENCIA,DVISI_FECHACONTACTO from DBXSCHEMA.DVISITADIARIACLIENTES WHERE DVISI_SECUENCIA="+registros.SelectedValue.ToString()+" ");
			////////////////
			DatasToControls bind = new DatasToControls();
			
			bind.PutDatasIntoDropDownList(catalogo,"select ppre.PCAT_CODIGO,pcat.pcat_descripcion from DBXSCHEMA.PPRECIOVEHICULO ppre, dbxschema.pcatalogovehiculo pcat WHERE ppre.pcat_codigo=pcat.pcat_codigo");
			DatasToControls.EstablecerValueDropDownList(catalogo,lineas.Tables[0].Rows[0][7].ToString());

			bind.PutDatasIntoDropDownList(medio,"select PCLAS_CODIGOVENTA,PCLAS_VENTADESCRIP from DBXSCHEMA.PCLASEVENTAVEHICULO ORDER BY PCLAS_VENTADESCRIP");
			DatasToControls.EstablecerValueDropDownList(medio,lineas.Tables[0].Rows[0][8].ToString());

			bind.PutDatasIntoDropDownList(vendedor2,"SELECT PVEN_CODIGO,PVEN_NOMBRE from DBXSCHEMA.PVENDEDOR ORDER BY PVEN_NOMBRE");
			DatasToControls.EstablecerValueDropDownList(vendedor2,lineas.Tables[0].Rows[0][9].ToString());
			vendedor2.Enabled=false;

			bind.PutDatasIntoDropDownList(prospecto,"select PPRO_CODIGO,PPRO_DESCP from DBXSCHEMA.PPROSPECTO ORDER BY PPRO_CODIGO");
			DatasToControls.EstablecerValueDropDownList(prospecto,lineas.Tables[0].Rows[0][10].ToString());

			bind.PutDatasIntoDropDownList(tipoContacto,"SELECT presulc_secuencia, presulc_descripcion FROM dbxschema.presultadocontacto");
			DatasToControls.EstablecerValueDropDownList(tipoContacto,lineas.Tables[0].Rows[0][11].ToString());

			secuencia.Text=registros.SelectedValue.ToString();
			/////////////////
			nombre.Text=lineas.Tables[0].Rows[0].ItemArray[0].ToString();
			telefono.Text=lineas.Tables[0].Rows[0].ItemArray[1].ToString();
			telefonomovil.Text=lineas.Tables[0].Rows[0].ItemArray[2].ToString();
			telefonooficina.Text=lineas.Tables[0].Rows[0].ItemArray[3].ToString();
			email.Text=lineas.Tables[0].Rows[0].ItemArray[4].ToString();
			observaciones.Text=lineas.Tables[0].Rows[0].ItemArray[5].ToString();
			fecha.Text=Convert.ToDateTime(lineas.Tables[0].Rows[0].ItemArray[6]).ToString("yyyy-MM-dd");
			fechaContacto.Text=Convert.ToDateTime(lineas.Tables[0].Rows[0][12]).ToString("yyyy-MM-dd");
			Actualizar.Enabled=true;
			/////////////////
		}
		public void Actualizar_Click(object sender, System.EventArgs e)
		{
			Actualizar.Enabled=false;
			DBFunctions.NonQuery("UPDATE DBXSCHEMA.DVISITADIARIACLIENTES SET DVISI_NOMBRE='"+nombre.Text.ToString()+"',DVISI_TELEFIJO='"+telefono.Text.ToString()+"',DVISI_TELEMOVIL='"+telefonomovil.Text.ToString()+"',DVISI_TELEOFICINA='"+telefonooficina.Text.ToString()+"',DVISI_EMAIL='"+email.Text.ToString()+"',DVISI_OBSERVACIONES='"+observaciones.Text.ToString()+"',DVISI_FECHAVISI='"+fecha.Text.ToString()+"',PCAT_CODIGO='"+catalogo.SelectedValue.ToString()+"',PCLAS_CODIGOVENTA='"+medio.SelectedValue.ToString()+"',PVEN_CODIGO='"+vendedor2.SelectedValue.ToString()+"',PPRO_CODIGO='"+prospecto.SelectedValue.ToString()+"',PRESULC_SECUENCIA="+tipoContacto.SelectedValue+",DVISI_FECHACONTACTO='"+fechaContacto.Text+"' WHERE DVISI_SECUENCIA="+registros.SelectedValue.ToString()+" ");
            Utils.MostrarAlerta(Response, "Registro Modificado Exitosamente");
			Response.Redirect(indexPage+"?process=Vehiculos.ModificarSeguimientoDiario");
		}
	}
}
