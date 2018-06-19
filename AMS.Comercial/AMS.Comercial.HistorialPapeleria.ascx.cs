namespace AMS.Comercial
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
    using Microsoft.Web.UI.WebControls;
    using AMS.DB;
    using AMS.Forms;
    using Ajax;
    using AMS.DBManager;

	/// <summary>
	///		Descripción breve de AMS_Comercial_HistorialPapeleria.
	/// </summary>
	public class AMS_Comercial_HistorialPapeleria : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlTipoDocumento;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.TextBox txtFecha;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Button btnConsultar;
		protected System.Web.UI.WebControls.DataGrid dgrPapeleria;
		protected System.Web.UI.WebControls.Panel pnlPapeleria;
		protected System.Web.UI.WebControls.DropDownList ddlMovimiento;
		protected System.Web.UI.WebControls.Label lblError;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				//Agencias
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				ddlAgencia.Items.Insert(0,new ListItem("No asignada","0"));
				//Tipos
				DataSet dsTipos=new DataSet();
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlTipoDocumento,
					"select tdoc_codigo as valor,tdoc_nombre as texto from DBXSCHEMA.TDOCU_TRANS ORDER BY tdoc_nombre;");
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
			this.btnConsultar.Click += new System.EventHandler(this.btnConsultar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        protected void btnConsultar_Click(object sender, System.EventArgs e)
        {
            string sqlC = "SELECT ";
            long agencia = Convert.ToInt16(ddlAgencia.SelectedValue);
            if (ddlMovimiento.SelectedValue == "R")
            {
                sqlC += "MRECEPCION_NUMERO AS NUMERO, TDOC_NOMBRE, NUMERO_TALONARIOS, " +
                "rtrim(char(cast(right(rtrim(char(DOCUMENTO_INICIAL)), " + AMS.Comercial.Tiquetes.lenTiquete + ") as integer))) as DOCUMENTO_INICIAL, " +
                "rtrim(char(cast(right(rtrim(char(DOCUMENTO_FINAL)), " + AMS.Comercial.Tiquetes.lenTiquete + ") as integer))) as DOCUMENTO_FINAL, " +
                "FECHA_REPORTE, MNIT_RESPONSALE, coalesce(mnit_APELLIDOS,'') concat coalesce( mnit_APELLIDO2,'')  concat ' ' concat mnit_NOMBRES concat ' '"+
                "  concat coalesce(mnit_NOMBRE2 ,'') AS NOMBRE_RESPONSABLE, '" +
                (ddlTipoDocumento.SelectedValue.Equals("TIQ") ? ddlAgencia.SelectedItem.Text : "No asignada") + "' AS AGENCIA " +
                "FROM MRECEPCION_PAPELERIA MA, TDOCU_TRANS TD, MNIT NIT " +
                "WHERE  NIT.MNIT_NIT= MA.MNIT_RESPONSALE AND TD.TDOC_CODIGO=MA.TDOC_CODIGO AND TD.TDOC_CODIGO='" + ddlTipoDocumento.SelectedValue + "'";
                if (txtFecha.Text.Length > 0) sqlC += "AND FECHA_REPORTE='" + txtFecha.Text + "' ";
            }
            if (ddlMovimiento.SelectedValue == "D")
            {
                sqlC += "MDESPACHO_NUMERO AS NUMERO, TDOC_NOMBRE, NUMERO_TALONARIOS, " +
                "rtrim(char(cast(right(rtrim(char(DOCUMENTO_INICIAL)), " + AMS.Comercial.Tiquetes.lenTiquete + ") as integer))) as DOCUMENTO_INICIAL, " +
                "rtrim(char(cast(right(rtrim(char(DOCUMENTO_FINAL)), " + AMS.Comercial.Tiquetes.lenTiquete + ") as integer))) as DOCUMENTO_FINAL, " +
                "FECHA_REPORTE, MNIT_RESPONSALE,coalesce(mnit_APELLIDOS,'') concat coalesce( mnit_APELLIDO2,'') concat ' ' concat mnit_NOMBRES concat ' '" +
                " concat coalesce(mnit_NOMBRE2 ,'')AS NOMBRE_RESPONSABLE, MG.MAGE_NOMBRE AS AGENCIA " +
                "FROM MDESPACHO_PAPELERIA MA, TDOCU_TRANS TD, MNIT NIT, magencia MG " +
                "WHERE TD.TDOC_CODIGO=MA.TDOC_CODIGO AND MG.MAG_CODIGO=MA.MAG_CODIGO" + " AND NIT.MNIT_NIT= MA.MNIT_RESPONSALE AND TD.TDOC_CODIGO='" + ddlTipoDocumento.SelectedValue + "' ";
                if(agencia!=0)sqlC += " AND MA.MAG_CODIGO= "+agencia+" ";
                if (txtFecha.Text.Length > 0) sqlC += " AND FECHA_REPORTE='" + txtFecha.Text + "' ";
            }
            if (ddlMovimiento.SelectedValue == "A")
            {
                sqlC += "MASIGNACION_NUMERO AS NUMERO, TDOC_NOMBRE, NUMERO_TALONARIOS, " +
                    "rtrim(char(cast(right(rtrim(char(DOCUMENTO_INICIAL)), " + AMS.Comercial.Tiquetes.lenTiquete + ") as integer))) as DOCUMENTO_INICIAL, " +
                    "rtrim(char(cast(right(rtrim(char(DOCUMENTO_FINAL)), " + AMS.Comercial.Tiquetes.lenTiquete + ") as integer))) as DOCUMENTO_FINAL, " +
                "FECHA_REPORTE, MNIT_RESPONSALE, coalesce(mnit_APELLIDOS,'') concat coalesce( mnit_APELLIDO2,'') concat ' ' concat mnit_NOMBRES concat ' '"+
                " concat coalesce(mnit_NOMBRE2 ,'')AS NOMBRE_RESPONSABLE, MG.MAGE_NOMBRE AS AGENCIA " +
                "FROM MASIGNACION_PAPELERIA MA, TDOCU_TRANS TD, MNIT NIT, magencia MG " +
                "WHERE TD.TDOC_CODIGO=MA.TDOC_CODIGO AND MG.MAG_CODIGO=MA.MAG_CODIGO AND NIT.MNIT_NIT= MA.MNIT_RESPONSALE AND TD.TDOC_CODIGO='" + ddlTipoDocumento.SelectedValue + "' ";
                if (agencia != 0) sqlC += " AND MA.MAG_CODIGO= " + agencia + " ";
                if (txtFecha.Text.Length > 0) sqlC += " AND FECHA_REPORTE='" + txtFecha.Text + "' ";
            
            }
            if (agencia == 0 && !ddlTipoDocumento.SelectedValue.Equals("TIQ"))
                sqlC += "AND DOCUMENTO_INICIAL<" + ((long)Math.Pow(10, AMS.Comercial.Tiquetes.lenTiquete)).ToString();
                  
            if(ddlTipoDocumento.SelectedValue.Equals("TIQ"))
            {               
                sqlC += " AND DOCUMENTO_INICIAL<" + ((agencia + 1) * ((long)Math.Pow(10, AMS.Comercial.Tiquetes.lenTiquete))).ToString() + " ";
                sqlC += " AND DOCUMENTO_INICIAL>=" + ((agencia) * ((long)Math.Pow(10, AMS.Comercial.Tiquetes.lenTiquete))).ToString() + " ";
            }
            sqlC += " ORDER BY FECHA_REPORTE DESC;";
            DataSet dsHistorial = new DataSet();
            DBFunctions.Request(dsHistorial, IncludeSchema.NO, sqlC);
            dgrPapeleria.DataSource = dsHistorial.Tables[0];
            dgrPapeleria.DataBind();
        }
    }
}
