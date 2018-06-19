namespace AMS.Marketing
{
	using System;
	using System.Collections;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
    using AMS.Tools;
    
	/// <summary>
	///		Descripción breve de AMS_Marketing_AplicacionEncuestas.
	/// </summary>
	public partial class AMS_Marketing_AplicacionEncuestas : System.Web.UI.UserControl
	{
		protected int i;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected Table tbForm = new Table();
		protected DataSet ds;
        protected string mensaje = "";
				
		protected void Page_Load(object sender, EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlencuestas,"SELECT MENC_CODIENCU,MENC_NOMBENCU FROM dbxschema.mencuesta");
				if(ddlencuestas.Items.Count!=0)
					ddlencuestas.Items.Insert(0,new ListItem("--Escoja una encuesta--",""));
                if(Request.QueryString["gr"]!=null)
                Utils.MostrarAlerta(Response, "La encuesta No." + Request.QueryString["gr"] + " ha sido guardada con exito");
				btnGrabar.Enabled=false;
			}
            if (!ddlencuestas.Enabled)
                SetForm();
		}

		protected void Cargar_Datos_Primarios()
		{
            ds = new DataSet();
            DBFunctions.Request(ds, IncludeSchema.NO,   "SELECT * FROM dBxschema.mencuesta WHERE MENC_CODIENCU='" + ddlencuestas.SelectedValue + "';" +
                                                        "SELECT dd.ppre_codipreg, pp.ppre_descpreg, pp.ttip_codigo " +
                                                        "FROM dbxschema.ddisenoencuesta dd, dbxschema.ppreguntaencuesta pp " +
                                                        "WHERE menc_codiencu='" + ddlencuestas.SelectedValue + "' and dd.ppre_codipreg=pp.ppre_codipreg;");                                    

            ViewState["DDISENOENCUESTA"] = ds;
            this.lbnum.Text=Convert.ToInt32(DBFunctions.SingleData(
                                        "SELECT CASE WHEN MAX(DENC_CODIRESU) IS NULL THEN 1 ELSE MAX(DENC_CODIRESU)+1 END " +
                                        "FROM DBXSCHEMA.dencuestaresultados WHERE  menc_codiencu='" + ddlencuestas.SelectedValue + "';")).ToString();
			
            this.lbfec.Text=DateTime.Now.ToString("yyyy-MM-dd");
			this.lbcod.Text=ddlencuestas.SelectedValue;
			this.lbnom.Text=ds.Tables[0].Rows[0][1].ToString();
			this.lbobj.Text=ds.Tables[0].Rows[0][4].ToString();
            this.txtFechaEnc.Text = DateTime.Now.ToString("yyyy-MM-dd");
            	
		}
		
		protected void SetForm()
		{
            ds = (DataSet)ViewState["DDISENOENCUESTA"];
            for(i=0; i<ds.Tables[1].Rows.Count; i++)
			{
				DataRow dr=ds.Tables[1].Rows[i];
				if(dr[2].ToString()=="C")
					this.MapComboType(dr);
				else if(dr[2].ToString()=="A")
					this.MapStringType(dr);
			}
			phForm1.Controls.Add(tbForm);
		}
						
		protected void MapStringType(DataRow dr)
		{
			//Como es un string pongo el label con el texto a mostrar y un textbox
			RequiredFieldValidator rfv;
            /*antiguo*/
            //Label lb= PutLabel((i+1).ToString()+"  "+dr[1].ToString());
            Label lb = PutLabel("[" + dr[0] + "]  " + dr[1].ToString());
            TextBox tb = PutTextBox();
			TableCell tc1 = new TableCell();
			tc1.Controls.Add(lb);
			TableCell tc2 = new TableCell();
			tc2.Controls.Add(tb);
			rfv = RequiredValidator(tb.ID,dr[0].ToString());
			tc2.Controls.Add(rfv);
			TableRow tr = new TableRow();
			tr.Cells.Add(tc1);
			tr.Cells.Add(tc2);
			tbForm.Rows.Add(tr);
		}
		
		protected void MapComboType(DataRow dr)
		{
            /*antiguo*/
			//Label lb=PutLabel((i+1).ToString()+"  "+dr[1].ToString());
            Label lb = PutLabel("[" + dr[0] + "]  " + dr[1].ToString());
            DropDownList ddl=new DropDownList();
			TextBox tb=new TextBox();
			TableCell tc1 = new TableCell();
			TableCell tc2 = new TableCell();
			TableCell tc3 = new TableCell();
			tc1.Controls.Add(lb);
			ddl=PutDropDownList(dr[0].ToString());
			tb=PutTextBox("algo");
			tc2.Controls.Add(ddl);
			tc3.Controls.Add(new LiteralControl("Comentario : "));
			tc3.Controls.Add(tb);
			TableRow tr = new TableRow();
			tr.Cells.Add(tc1);
			tr.Cells.Add(tc2);
			tr.Cells.Add(tc3);
			tbForm.Rows.Add(tr);
		}
		
		protected Label PutLabel(string text)
		{
			Label lb = new Label();
			lb.ID = "lb_" + i;
			lb.Text = text + ": ";
			return lb;
		}
		
		protected TextBox PutTextBox()
		{
			TextBox tb = new TextBox();
			tb.ID = "tb_" + i.ToString();
			tb.Width = new Unit(400);
			tb.Height = new Unit(60);
			tb.TextMode = TextBoxMode.MultiLine;
			return tb;
		}

		protected TextBox PutTextBox(string algo)
		{
			TextBox tb = new TextBox();
			tb.ID = "tbc_" + i.ToString();
			tb.Width = new Unit(200);
			tb.Height = new Unit(45);
			tb.TextMode = TextBoxMode.MultiLine;
			return tb;
		}
		
		protected DropDownList PutDropDownList(string pregunta)
		{
			DataSet tempDS = new DataSet();
            tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, 
                            "SELECT pr.pres_codiresp, pr.pres_descresp " +
                            "FROM dbxschema.dpreguntarespuesta dp, dbxschema.prespuestaencuesta pr " +
                            "WHERE dp.ppre_codipreg='" + pregunta + "' and dp.pres_codiresp=pr.pres_codiresp;");

			DropDownList ddl = new DropDownList();
			ddl.ID = "ddl_" + i.ToString();
			try
			{
				ddl.DataSource = tempDS.Tables[0];
				ddl.DataValueField = tempDS.Tables[0].Columns[0].ToString();
				ddl.DataTextField = tempDS.Tables[0].Columns[1].ToString();
				ddl.DataBind();
			}
			catch(Exception e)
			{
				lb.Text += e.ToString() + "<br>"+DBFunctions.exceptions;
			}
			tempDS.Clear();
			return ddl;
		}
		
		protected RegularExpressionValidator Validator(string controlID, string regex, string textError)
		{
			RegularExpressionValidator rev = new RegularExpressionValidator();
			rev.ErrorMessage = textError+ ". No Valido .";
			rev.ControlToValidate = controlID;
			rev.ValidationExpression = regex;
			rev.Display = ValidatorDisplay.None;
			return rev;
		}
		
		protected RequiredFieldValidator RequiredValidator(string controlID, string textError)
		{
			RequiredFieldValidator rfv = new RequiredFieldValidator();
			rfv.ErrorMessage = "Requerido";
			rfv.ControlToValidate = controlID;
			rfv.Display = ValidatorDisplay.Dynamic;
			return rfv;
		}

		protected void btnGrabar_Click(object sender, System.EventArgs e)
		{
			ArrayList sqls=new ArrayList();
            ds = (DataSet)ViewState["DDISENOENCUESTA"];
			for(int m=0;m<ds.Tables[1].Rows.Count;m++)
			{		
				if(ds.Tables[1].Rows[m][2].ToString()=="C")
				    sqls.Add("INSERT INTO DBXSCHEMA.DENCUESTARESULTADOS (MENC_CODIENCU, PPRE_CODIPREG, PRES_CODIRESP, DENC_FECHRESU, DENC_COMEENCU, MNIT_NIT) " +
                             "VALUES('" + ddlencuestas.SelectedValue + "','" + ds.Tables[1].Rows[m][0].ToString() + "','" + ((DropDownList)tbForm.Rows[m].Cells[1].Controls[0]).SelectedValue + "','" + this.lbfec.Text + "','" + ((TextBox)tbForm.Rows[m].Cells[2].Controls[1]).Text + "','" + this.txtNit.Text + "')");
                else if(ds.Tables[1].Rows[m][2].ToString()=="A")
                    sqls.Add("INSERT INTO DBXSCHEMA.DENCUESTARESULTADOS (MENC_CODIENCU, PPRE_CODIPREG, PRES_CODIRESP, DENC_FECHRESU, DENC_COMEENCU, MNIT_NIT) " +
                             "VALUES('" + ddlencuestas.SelectedValue + "','" + ds.Tables[1].Rows[m][0].ToString() + "',NULL,'" + this.lbfec.Text + "','" + ((TextBox)tbForm.Rows[m].Cells[1].Controls[0]).Text + "','" + this.txtNit.Text + "')");
            }
			if(DBFunctions.Transaction(sqls))
			{
				this.btnGrabar.Enabled=false;
                if (Session["encuesta"] != null)
                {
                    ddlencuestas.SelectedIndex = 0;
                    ddlencuestas.Enabled = true;
                    plcEncuesta.Visible = false;
                }
                else
                    Response.Redirect(indexPage+"?process=Marketing.AplicacionEncuestas&gr=" + this.lbcod.Text + "-" + this.lbnum.Text);
			}
			else
				lb.Text="Mal "+DBFunctions.exceptions;
		}

		protected void ddlencuestas_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			tbForm.Width = new Unit(800);
			tbForm.BackColor = Color.FromArgb(240,240,240);
			tbForm.Font.Name = "arial";
			//GetSchema();
			Cargar_Datos_Primarios();
			SetForm();
			ddlencuestas.Enabled = false;
			btnGrabar.Enabled = true;
            plcEncuesta.Visible = true;
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

		protected void btnCancelar_Click(object sender, System.EventArgs e)
		{
            if (Session["encuesta"] != null)
            {
                ddlencuestas.SelectedIndex = 0;
                ddlencuestas.Enabled = true;
                btnGrabar.Enabled = false;
                plcEncuesta.Visible = false;
            }
            else
			    Response.Redirect(indexPage+"?process=Marketing.AplicacionEncuestas");
		}
	}
}
