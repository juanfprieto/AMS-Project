namespace AMS.Nomina
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Nomina_AdministrarNovedades.
	/// </summary>
	public class AMS_Nomina_AdministrarNovedades : System.Web.UI.UserControl
	{
		#region Atributos

		protected System.Web.UI.WebControls.DropDownList ddlano;
		protected System.Web.UI.WebControls.DropDownList ddlmes;
		protected System.Web.UI.WebControls.DropDownList ddlquincena;
		protected System.Web.UI.WebControls.Button btnAceptar;
		protected System.Web.UI.WebControls.DataGrid dgNov;
		protected DataTable dtNov=new DataTable();
		protected DataView vista;
		protected System.Web.UI.WebControls.Label lb;
		protected System.Web.UI.WebControls.CheckBox Todos;
		protected System.Web.UI.WebControls.DropDownList ddlConceptos;
		protected System.Web.UI.WebControls.CheckBox Conceptos;
		protected System.Web.UI.WebControls.TextBox txtEmpleado;
		protected System.Web.UI.WebControls.TextBox txtEmpleadoa;
		protected string err="";
        protected string[] fecha = null;
		
		#endregion
		
		#region Eventos

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			Armar_DataSource();
			if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlano,"SELECT pano_ano,pano_detalle FROM dbxschema.pano order by 1 desc");
				DatasToControls.EstablecerValueDropDownList(ddlano,DBFunctions.SingleData("SELECT cnom_ano FROM dbxschema.cnomina"));
				bind.PutDatasIntoDropDownList(ddlmes,"SELECT pmes_mes,pmes_nombre FROM dbxschema.pmes order by 1");
				DatasToControls.EstablecerValueDropDownList(ddlmes,DBFunctions.SingleData("SELECT cnom_mes FROM dbxschema.cnomina"));
				bind.PutDatasIntoDropDownList(ddlquincena,"SELECT tper_periodo,tper_descripcion FROM dbxschema.tperiodonomina order by 1");
				DatasToControls.EstablecerValueDropDownList(ddlquincena,DBFunctions.SingleData("SELECT cnom_quincena FROM dbxschema.cnomina"));
				//bind.PutDatasIntoDropDownList(ddlEmpleados,"Select memp_codiempl,memp_codiempl from DBXSCHEMA.MEMPLEADO where test_estado <> '4' ORDER BY MEMP_CODIEMPL");
				//DatasToControls.EstablecerValueDropDownList(ddlEmpleados,DBFunctions.SingleData("Select memp_codiempl from DBXSCHEMA.MEMPLEADO where test_estado <> '4'"));
				bind.PutDatasIntoDropDownList(ddlConceptos,"Select pcon_concepto,pcon_nombconc from DBXSCHEMA.PCONCEPTONOMINA where pcon_claseconc = 'N' ORDER BY pcon_nombconc");
				DatasToControls.EstablecerValueDropDownList(ddlConceptos,DBFunctions.SingleData("Select pcon_concepto from DBXSCHEMA.PCONCEPTONOMINA"));

				this.dgNov_BindDatas();
            }
            else
            {
                
            }
		}

		private void btnAceptar_Click(object sender, System.EventArgs e)
		{
            if (txtEmpleado.Text.Length == 0)
            {
                Utils.MostrarAlerta(Response, "Debe ingresar un empleado!");
                return;
            }
			Armar_DataSource();
			this.dgNov_BindDatas();
		}

		private void dgNov_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			if(e.CommandName=="agregar")
			{
				if(!this.Realizar_Validaciones((((TextBox)e.Item.Cells[0].FindControl("tbempf")).Text.Trim()),(((TextBox)e.Item.Cells[1].FindControl("tbconf")).Text.Split('-'))[0].Trim(),((TextBox)e.Item.Cells[2].FindControl("tbnovf")).Text,((TextBox)e.Item.Cells[3].FindControl("tbvaltotf")).Text,((TextBox)e.Item.Cells[4].FindControl("tbcantf")).Text,((TextBox)e.Item.Cells[5].FindControl("tbfecf")).Text,ref err))
				{
					int ret=DBFunctions.NonQuery("INSERT INTO dbxschema.mnovedadesnomina VALUES(default,'"+(((TextBox)e.Item.Cells[0].FindControl("tbempf")).Text.Trim()+"','"+(((TextBox)e.Item.Cells[1].FindControl("tbconf")).Text.Split('-'))[0].Trim()+"','"+((TextBox)e.Item.Cells[2].FindControl("tbnovf")).Text+"',"+Convert.ToDouble(((TextBox)e.Item.Cells[3].FindControl("tbvaltotf")).Text)+","+Convert.ToDouble(((TextBox)e.Item.Cells[4].FindControl("tbcantf")).Text)+",'"+((TextBox)e.Item.Cells[5].FindControl("tbfecf")).Text+"')"));
					if(ret==1)
					{
                        Utils.MostrarAlerta(Response, "Novedad Creada Satisfactoriamente");
						Armar_DataSource();
						dgNov_BindDatas();
					}
					else
					{
                        Utils.MostrarAlerta(Response, "Error al crear la novedad. Revise la parte inferior de la pantalla para mas detalles");
						lb.Text="Error : "+DBFunctions.exceptions;
					}
				}
				else
				{
                    Utils.MostrarAlerta(Response, "" + err + "");
					dgNov_BindDatas();
				}
			}
			else if(e.CommandName=="eliminar")
			{
                Armar_DataSource();
                int ret = DBFunctions.NonQuery("DELETE FROM dbxschema.mnovedadesnomina WHERE mnov_secuencia=" + dtNov.Rows[e.Item.DataSetIndex][0].ToString()+"");
				if(ret==1)
				{
                    Utils.MostrarAlerta(Response, "Novedad Eliminada Satisfactoriamente");
					Armar_DataSource();
					dgNov_BindDatas();
				}
				else
				{
                    Utils.MostrarAlerta(Response, "Imposible eliminar la novedad. Revise la parte inferior de la pantalla para mas detalles");
					dgNov_BindDatas();
					lb.Text="Error : "+DBFunctions.exceptions;
				}
			}
		}

		private void dgNov_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			dgNov.EditItemIndex=e.Item.ItemIndex;
            Armar_DataSource();
			this.dgNov_BindDatas();
		}

		private void dgNov_UpdateCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
            Armar_DataSource();

			if(!this.Realizar_Validaciones((((TextBox)e.Item.Cells[0].FindControl("tbemp")).Text.Split('-'))[0].Trim(),
                                            (((TextBox)e.Item.Cells[1].FindControl("tbcon")).Text.Split('-'))[0].Trim(),
                                            ((TextBox)e.Item.Cells[2].FindControl("tbnov")).Text,
                                            ((TextBox)e.Item.Cells[3].FindControl("tbvaltot")).Text,
                                            ((TextBox)e.Item.Cells[4].FindControl("tbcant")).Text,
                                            ((TextBox)e.Item.Cells[5].FindControl("tbfec")).Text,ref err))
			{
				int ret=DBFunctions.NonQuery("UPDATE dbxschema.mnovedadesnomina SET memp_codiempl='"+
                        (((TextBox)e.Item.Cells[0].FindControl("tbemp")).Text.Split('-'))[0].Trim()+
                        "',pcon_concepto='"+(((TextBox)e.Item.Cells[1].FindControl("tbcon")).Text.Split('-'))[0].Trim()+
                        "',mnov_novedad='"+((TextBox)e.Item.Cells[2].FindControl("tbnov")).Text+
                        "',mnov_valrtotl="+Convert.ToDouble(((TextBox)e.Item.Cells[3].FindControl("tbvaltot")).Text)+
                        ",mnov_cantidad="+Convert.ToDouble(((TextBox)e.Item.Cells[4].FindControl("tbcant")).Text)+",mnov_fecha='"+
                        ((TextBox)e.Item.Cells[5].FindControl("tbfec")).Text+"' WHERE mnov_secuencia="+dtNov.Rows[e.Item.DataSetIndex][0].ToString()+"");
				if(ret==1)
				{
                    Utils.MostrarAlerta(Response, "Novedad Actualizada Satisfactoriamente");
					dgNov.EditItemIndex=-1;
					Armar_DataSource();
					dgNov_BindDatas();
				}
				else
				{
                    Utils.MostrarAlerta(Response, "Error al actualizar la novedad. Revise la parte inferior de la pantalla para mas detalles");
					lb.Text="Detalles del error : <br>"+DBFunctions.exceptions;
				}
			}
			else
			{
                Utils.MostrarAlerta(Response, "" + err + "");
				dgNov_BindDatas();
			}
		}

		private void dgNov_CancelCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			dgNov.EditItemIndex=-1;
            Armar_DataSource();
			this.dgNov_BindDatas();
		}

		private void dgNov_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
            Armar_DataSource();
            dgNov.DataSource = vista;
			dgNov.CurrentPageIndex=e.NewPageIndex;
            dgNov.DataBind();
            //this.dgNov_BindDatas();
		}

		private void dgNov_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Footer)
			{
				((TextBox)e.Item.Cells[5].FindControl("tbfecf")).Text=DateTime.Now.Date.ToString("yyyy-MM-dd");
				((TextBox)e.Item.Cells[4].FindControl("tbcantf")).Text="0";
			}
		}

		#endregion
		
		#region Métodos

		private void Cargar_Datos()
		{
			DataSet ds=new DataSet();
			string sql = "SELECT MNOV.mnov_secuencia AS SECUENCIA,MNOV.memp_codiempl CONCAT ' - ' CONCAT MNIT.mnit_apellidos CONCAT ' ' CONCAT COALESCE(MNIT.mnit_apellido2,'') CONCAT ' ' CONCAT MNIT.mnit_nombres CONCAT ' ' CONCAT COALESCE(MNIT.mnit_nombre2,'') AS EMPLEADO,MNOV.pcon_concepto CONCAT ' - ' CONCAT PCON.pcon_nombconc AS CONCEPTO,MNOV.mnov_novedad AS DOCUMENTO,MNOV.mnov_valrtotl AS VALOR,MNOV.mnov_cantidad AS CANTIDAD,MNOV.mnov_fecha AS FECHA FROM dbxschema.mempleado MEMP,dbxschema.mnit MNIT,dbxschema.mnovedadesnomina MNOV,dbxschema.pconceptonomina PCON WHERE MEMP.mnit_nit=MNIT.mnit_nit AND MNOV.memp_codiempl=MEMP.memp_codiempl AND PCON.pcon_concepto=MNOV.pcon_concepto ";
			if (this.Todos.Checked == false)
			{
			   //sql= sql + " and  mnov.memp_codiempl = '" + this.ddlEmpleados.SelectedValue + "' ";
				if(!IsPostBack)return;
				
				sql= sql + " and  mnov.memp_codiempl = '" + this.txtEmpleado.Text + "' ";
			}
			if (this.Conceptos.Checked == false)
			{
                sql = sql + " and MNOV.pcon_concepto = '" + this.ddlConceptos.SelectedValue + "' ";
			}

            /*
			if(ddlano.Items.Count>0 && ddlmes.Items.Count>0 && ddlquincena.Items.Count>0)
				fecha=(this.Armar_Fecha(Convert.ToInt32(ddlano.SelectedValue),Convert.ToInt32(ddlmes.SelectedValue),Convert.ToInt32(ddlquincena.SelectedValue))).Split(';');
			else
                */
				fecha=(this.Armar_Fecha(Convert.ToInt32(DBFunctions.SingleData("SELECT cnom_ano FROM dbxschema.cnomina")),Convert.ToInt32(DBFunctions.SingleData("SELECT cnom_mes FROM dbxschema.cnomina")),Convert.ToInt32(DBFunctions.SingleData("SELECT cnom_quincena FROM dbxschema.cnomina")))).Split(';');
			sql= sql + " " + " AND MNOV.mnov_fecha BETWEEN '"+fecha[0]+"' AND '"+fecha[1]+"' ORDER BY MNOV.memp_codiempl ASC";
			DBFunctions.Request(ds,IncludeSchema.NO,sql);
			dtNov=ds.Tables[0];
			dtNov.AcceptChanges();
		}

		private void Cargar_dtNov()
		{
			dtNov.Columns.Add("SECUENCIA",typeof(string));
			dtNov.Columns.Add("EMPLEADO",typeof(string));
			dtNov.Columns.Add("CONCEPTO",typeof(string));
			dtNov.Columns.Add("DOCUMENTO",typeof(string));
			dtNov.Columns.Add("VALOR",typeof(double));
			dtNov.Columns.Add("CANTIDAD",typeof(int));
			dtNov.Columns.Add("FECHA",typeof(string));
		}

		private void dgNov_BindDatas()
		{
            try
            {
                dgNov.DataSource = vista;
                dgNov.DataBind();
            }
            catch (Exception)
            {
               dgNov.CurrentPageIndex = dgNov.CurrentPageIndex;
            }
		}

		private void Armar_DataSource()
		{
			Session.Remove("dtNov");
			if(Session["dtNov"]==null)
			{
				if(dtNov==null)
					Cargar_dtNov();
				Cargar_Datos();
				Session["dtNov"]=dtNov;
			} 
			else
				dtNov=(DataTable)Session["dtNov"];
			vista=new DataView(dtNov);
			return;
		}
		
		private string Armar_Fecha(int ano,int mes,int quincena)
		{
			string formaPago=DBFunctions.SingleData("SELECT cnom_opciquinomens FROM dbxschema.cnomina");
			string fecha="";
			//Quincenal
			if(formaPago=="1")
			{
				//Si la quincena es la 1, armo una fecha de 01 a 15
				if(quincena==1)
				{
					if(mes>=1 && mes <=9)
						fecha+=ano+"-0"+mes+"-01;"+ano+"-0"+mes+"-15";
					else
						fecha+=ano+"-"+mes+"-01;"+ano+"-"+mes+"-15";
				}
					//Si la quincena es la 2, armo una fecha de 16-30
				else if(quincena==2)
				{
					if(Bisiesto(ano))
					{
						//Si es febrero y es bisiesto
						if(mes==2)
							fecha+=ano+"-0"+mes+"-16;"+ano+"-0"+mes+"-29";
							//Si es un mes de 31 dias y esta entre los primeros nueve 
						else if(mes==1 || mes==3 || mes==5 || mes==7 || mes==8)
							fecha+=ano+"-0"+mes+"-16;"+ano+"-0"+mes+"-30";   // 31
							//Si es un mes de 31 dias y esta en los ultimos tres
						else if(mes==10 || mes==12)
							fecha+=ano+"-"+mes+"-16;"+ano+"-"+mes+"-30";   // 31
							//Si es un mes de 30 dias y esta entre los primeros nueve
						else if(mes==4 || mes==6 || mes==9)
							fecha+=ano+"-0"+mes+"-16;"+ano+"-0"+mes+"-30";
						else
							fecha+=ano+"-"+mes+"-16;"+ano+"-"+mes+"-30";
					}
					else
					{
						//Si es febrero
						if(mes==2)
							fecha+=ano+"-0"+mes+"-16;"+ano+"-0"+mes+"-28";
							//Si es un mes de 31 dias y esta entre los primeros nueve 
						else if(mes==1 || mes==3 || mes==5 || mes==7 || mes==8)
							fecha+=ano+"-0"+mes+"-16;"+ano+"-0"+mes+"-30";   // 31
							//Si es un mes de 31 dias y esta enlos ultimos tres
						else if(mes==10 || mes==12)
							fecha+=ano+"-"+mes+"-16;"+ano+"-"+mes+"-30";  //   31
							//Si es un mes de 30 dias y esta entre los primeros nueve
						else if(mes==4 || mes==6 || mes==9)
							fecha+=ano+"-0"+mes+"-16;"+ano+"-0"+mes+"-30";
						else
							fecha+=ano+"-"+mes+"-16;"+ano+"-"+mes+"-30";
					}
				}
			}
				//Mensual
			else if(formaPago=="2")
			{
				if(Bisiesto(ano))
				{
					//Si es febrero y es bisiesto
					if(mes==2)
						fecha+=ano+"-0"+mes+"-01;"+ano+"-0"+mes+"-29";
						//Si es un mes de 31 dias y esta entre los primeros nueve 
					else if(mes==1 || mes==3 || mes==5 || mes==7 || mes==8)
						fecha+=ano+"-0"+mes+"-01;"+ano+"-0"+mes+"-30";   //  31
						//Si es un mes de 31 dias y esta enlos ultimos tres
					else if(mes==10 || mes==12)
						fecha+=ano+"-"+mes+"-01;"+ano+"-"+mes+"-30";   //  31
						//Si es un mes de 30 dias y esta entre los primeros nueve
					else if(mes==4 || mes==6 || mes==9)
						fecha+=ano+"-0"+mes+"-01;"+ano+"-"+mes+"-30";
					else
						fecha+=ano+"-"+mes+"-01;"+ano+"-"+mes+"-30";
				}
				else
				{
					//Si es febrero
					if(mes==2)
						fecha+=ano+"-0"+mes+"-01;"+ano+"-0"+mes+"-28";
						//Si es un mes de 31 dias y esta entre los primeros nueve 
					else if(mes==1 || mes==3 || mes==5 || mes==7 || mes==8)
						fecha+=ano+"-0"+mes+"-01;"+ano+"-0"+mes+"-30";   //  31
						//Si es un mes de 31 dias y esta enlos ultimos tres
					else if(mes==10 || mes==12)
						fecha+=ano+"-"+mes+"-01;"+ano+"-"+mes+"-30";   // 31
						//Si es un mes de 30 dias y esta entre los primeros nueve
					else if(mes==4 || mes==6 || mes==9)
						fecha+=ano+"-0"+mes+"-01;"+ano+"-"+mes+"-30";
					else
						fecha+=ano+"-"+mes+"-01;"+ano+"-"+mes+"-30";
				}
			}
			return fecha;
		}

		private bool Bisiesto(int ano)
		{
			if (ano%4!=0)
				return false;
			else
			{
				if (ano%4==0 && ano%100!=0)
					return true;
				else
				{
					if (ano%4==0 && ano%100==0 && ano%400==0)
						return true;
					else
						return false;
				}
			}
		}

		private bool Realizar_Validaciones(string emp,string con,string nov,string val,string cant,string fec,ref string err)
		{
			bool error=false;
            if(emp==string.Empty || con==string.Empty || nov==string.Empty || !DatasToControls.ValidarDouble(val) || !DatasToControls.ValidarDouble(cant) || !DatasToControls.ValidarDateTime(fec))
			{
				error=true;
				err="Algun dato es invalido";
			}

            if (!DBFunctions.RecordExist("select MEMP_CODIEMPL from dbxschema.mempleado  where MEMP_CODIEMPL='" + emp + "' and TEST_ESTADO <> '4' "))
            {
                error = true;
                err = "El empleado no existe o esta retirado.";
            }

            string tipoValor = DBFunctions.SingleData("select PCON_DESCCANT from dbxschema.PCONCEPTONOMINA  where PCON_CONCEPTO='" + con + "' ");
            if (tipoValor == "4")  // concepto es del tipo valor
            {
                if (val.ToString() == "0" || val.ToString() == "0.00" || val.ToString() == "")
                {
                    error = true;
                    err = "El concepto es de tipo Valor y la Columna Valor esta vacia.";
                }
                else
                    if (cant.ToString() != "0" && cant.ToString() != "0.00" && cant.ToString() != "")
                    {
                        error = true;
                        err = "El concepto es de tipo Valor y la Columna Cantidad no esta en CERO.";
                    }
            }
            if (tipoValor != "4")  // concepto es del tipo cantidad
            {
                if (cant.ToString() == "0" || cant.ToString() == "0.00" || cant.ToString() == "")
                {
                    error = true;
                    err = "El concepto es de tipo Cantidad y la Columna Cantidad esta vacia.";
                }
                else
                    if (val.ToString() != "0" && val.ToString() != "0.00" && val.ToString() != "")
                    {
                        error = true;
                        err = "El concepto es de tipo Cantidad y la Columna Valor no esta en CERO.";
                    }
            }

            if ((String.Compare(fec.ToString(),fecha[1].ToString()) > 0) || (String.Compare(fec.ToString(),fecha[0].ToString()) < 0))
            {
                error = true;
                err = "La fecha de la Novedad está errada, fuera del rango de la vigencia de la Nómina, Solo permite novedades del período Vigente";
            }

			return error;
		}

   		#endregion

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
			this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
			this.dgNov.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgNov_ItemCommand);
			this.dgNov.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgNov_PageIndexChanged);
			this.dgNov.CancelCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgNov_CancelCommand);
			this.dgNov.EditCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgNov_EditCommand);
			this.dgNov.UpdateCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgNov_UpdateCommand);
			this.dgNov.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgNov_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

        protected void btnAceptar_Click1(object sender, EventArgs e)
        {

        }


	}
}
