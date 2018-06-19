using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Management;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using AMS.CriptoServiceProvider;
using AMS.DB;

namespace AMS.Documentos
{
	public class Imprimir
	{
		private string mensajes,nombreReporte,pathReports,pathWrite,documento;
		private DataTable dtValPar;
		public ReportDocument oRpt;
		private bool exito, odbcReport = false;
		
		public string Mensajes{set{mensajes=value;} get{return mensajes;}}
		public DataTable DtValPar{set{dtValPar=value;} get{return dtValPar;}}
		public bool Exito{get{return exito;}}
        public bool OdbcReport { set { odbcReport = value; } get { return odbcReport; } }

        private AMS.DB.DB2Functions db2f = new DB2Functions();
		public string Documento { set { documento = value; } get { return documento; } }

		/// <summary>
		/// Constructor por defecto
		/// </summary>
		public Imprimir()
		{
            CrystalDecisions.Shared.SharedUtils.RequestLcid = 9226; //set report locale to colombia :3
			nombreReporte=pathReports=pathWrite="";
			exito=true;
			//oRpt=oHeader=oFooter=null;
		}

		/// <summary>
		/// Constructor diseñado para cargar los parametros que vengan incluidos en el reporte
		/// </summary>
		/// <param name="rpt">string nombre del rpt sin extensión</param>
		public Imprimir(string rpt)
		{
            CrystalDecisions.Shared.SharedUtils.RequestLcid = 9226; //set report locale to colombia :3
			try
			{
				nombreReporte=rpt;
				pathReports= ConfigurationManager.AppSettings["PathToReportsSource"];
				pathWrite=ConfigurationManager.AppSettings["PathToReports"];
                oRpt = new ReportDocument();
				oRpt.Load(pathReports+nombreReporte+".rpt");
               
				exito=true;
			}
			catch(Exception e)
			{
				AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.ErrorCrystalReports,string.Empty,e,
                    e.Message + "<br>" + e.InnerException + "<br>" + e.HelpLink + "<br>" + e.Source + "<br>" + 
                    e.StackTrace + "<br>Error al cargar el archivo [" + pathReports + nombreReporte + ".rpt]");
				mensajes+="Error al cargar el archivo. Detalles <br>"+e.Message;
			}
            
		}

        public static string ImprimirRPT(string nomReporte, Hashtable parametros)
        {
            Imprimir reporte = new Imprimir();

            Label lbvacio = new Label();
            DataTable dt = new DataTable();
            string[] Formulas = new string[0];
            string[] ValFormulas = new string[0];
            string usuario = ConfigurationManager.AppSettings["UID"];
            string password = ConfigurationManager.AppSettings["PWD" + GlobalData.EMPRESA];
            Crypto miCripto = new Crypto(Crypto.CryptoProvider.TripleDES);
            miCripto.IV = ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
            miCripto.Key = ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
            string newPwd = miCripto.DescifrarCadena(password);
            string nomA = nomReporte;

            dt.Columns.Add("NOMBRE", typeof(string));
            dt.Columns.Add("VALOR", typeof(object));

            foreach (string key in parametros.Keys)
            {
                DataRow row = dt.NewRow();
                row[0] = key;
                row[1] = parametros[key];
                dt.Rows.Add(row);
            }

            reporte.DtValPar = dt;
            reporte.PreviewReport2(nomReporte, "", "", 1, 1, 1, "", "", nomA, "pdf", usuario, newPwd, Formulas, ValFormulas, lbvacio);
            reporte.ReportUnload();

            return reporte.Documento;
        }

        public static string ImprimirRPT(HttpResponse response, string prefijo, int numero, bool mostrarPopUp)
        {
            FormatosDocumentos formatoFactura = new FormatosDocumentos();
            try
            {
                formatoFactura.Prefijo = prefijo;
                formatoFactura.Numero = numero;
                formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + prefijo + "'");
                if (formatoFactura.Codigo != string.Empty)
                {
                    if (formatoFactura.Cargar_Formato() && mostrarPopUp)
                        response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=600,WIDTH=800');</script>");

                    return formatoFactura.Documento;
                }
            }
            catch (Exception e)
            {
                string error = String.Format("Impresión de formato {0} - {1}", prefijo, numero);
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.ErrorCrystalReports, error, e, formatoFactura.Mensajes);
            }

            return formatoFactura.Mensajes;
        }

		/// <summary>
		/// metodo PreviewReport, con ete metodo se contruye el reporte y se exporta
		/// </summary>
		/// <param name="Rpt">Nombre del Archivo RPT a utilizar</param>
		/// <param name="Sentencia">Sentencia SQL a aplicar al reporte</param>
		/// <param name="Tabla">Nombre del la tabla a utilizar</param>
		/// <param name="archivo">nombre del archivo final del al exportacion</param>
		/// <param name="tipo">tipo de archivo a exportar</param>
		/// <param name="debug">Si o No segun el caso</param>
		/// <param name="Aplica">Nombre de la aplicacion que usa la funcion</param>
		/// <param name="nomserver">nombre del servidor</param>
		/// <param name="nombd">Nombre de la base de Datos</param>
		/// <param name="nomuser">Nombre del Usuario</param>
		/// <param name="passw">Contenido del Password</param>
		/// <param name="Nomformulas">Nombres de las formulas a utilizar</param>
		/// <param name="Valformulas">Contenido de las formulas a utilizar</param>
		//		public void PreviewReport2(string Rpt,string header,string footer,int copias,int PaginaInicial,int PaginaFinal,string Sentencia,string Tabla,string archivo,string tipo,string nomserver,string nombd,string nomuser,string passw,string[] Nomformulas,string[] Valformulas,Label lb)
		public void PreviewReport2(string Rpt,string header,string footer,int copias,int PaginaInicial,int PaginaFinal,string Sentencia,string Tabla,string archivo,string tipo,string nomuser,string passw,string[] Nomformulas,string[] Valformulas,Label lb)
		{
			string paso = "paso1";
			ExportOptions exportOpts = new ExportOptions();
			DiskFileDestinationOptions diskOpts = new DiskFileDestinationOptions();
			PdfRtfWordFormatOptions PDFFormatOpts = new PdfRtfWordFormatOptions();
			ExcelFormatOptions ExcelFormato = new ExcelFormatOptions();
			ReportDocument oRpt=new ReportDocument(); 
			ReportDocument oHeader = new ReportDocument(); 
			ReportDocument oFooter = new ReportDocument(); 
			TableLogOnInfo InfoConexBd;

			FormulaFieldDefinitions crFormulas;
			FormulaFieldDefinition crDefinicion;
			string FormulaName;

            try
            {
                if (this.oRpt == null)
                {
                    oRpt = new ReportDocument();
                    string NombreReporte = ConfigurationManager.AppSettings["PathToReportsSource"] + Rpt + ".rpt";
                    oRpt.Load(NombreReporte);
                    this.oRpt = oRpt;
                }
                else
                {
                    oRpt = this.oRpt;
                }

                EstablecerValoresParametros();
                paso = "paso :EstablecerValoresParametros";

                string subreportName;
                SubreportObject subreportObject;
                ReportDocument subreport = new ReportDocument();
                ReportDocument subreport2 = new ReportDocument();
                for (int h=0; h < oRpt.ReportDefinition.ReportObjects.Count; h++)
                {
                    if (oRpt.ReportDefinition.ReportObjects[h].Name.ToString().StartsWith("Subrepor"))
                    {
                        subreportObject = oRpt.ReportDefinition.ReportObjects[h] as SubreportObject;
                        if (subreportObject != null)
                        {
                            subreportName = subreportObject.SubreportName;
                            if (subreportName == "AMS_HEADER.rpt")
                                subreport = oRpt.OpenSubreport(subreportName);
                            if (subreportName == "AMS_FOOTER.rpt")
                                subreport2 = oRpt.OpenSubreport(subreportName);
                        }
                    }
                }

                paso = "paso :Formulas en Header";
                try
                {
                    crFormulas = subreport.DataDefinition.FormulaFields;
                    if (Nomformulas.Length > 0)
                    {
                        for (int i=0; i <= crFormulas.Count - 1; i++)
                        {
                            FormulaName = crFormulas[i].Name.ToString();
                            for (int ii= 0; ii < Nomformulas.Length; ii++)
                            {
                                if (FormulaName == Nomformulas[ii])
                                {
                                    crDefinicion = crFormulas[i];
                                    crDefinicion.Text = '"' + Valformulas[ii] + '"';
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { mensajes += ex.Message; }

                paso = "paso :Formulas en Footer";

                try
                {
                    crFormulas = subreport2.DataDefinition.FormulaFields;
                    if (Nomformulas.Length > 0)
                    {
                        for (int i=0; i <= crFormulas.Count - 1; i++)
                        {
                            FormulaName = crFormulas[i].Name.ToString();
                            for (int ii= 0; ii < Nomformulas.Length; ii++)
                            {
                                if (FormulaName == Nomformulas[ii])
                                {
                                    crDefinicion = crFormulas[i];
                                    crDefinicion.Text = '"' + Valformulas[ii] + '"';
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { mensajes += ex.Message; }

                //string tem = "", nomA = "";

                //ConnectionInfo connInfo = new ConnectionInfo();
                //connInfo.ServerName = ConfigurationManager.AppSettings["DataBase" + GlobalData.EMPRESA]; //"ECASSAS"; // "Driver={PostgreSQL ANSI};Server=myServer;Port=5432;";  
                //connInfo.DatabaseName = ConfigurationManager.AppSettings["DataBase" + GlobalData.EMPRESA]; ;
                //connInfo.UserID = nomuser;
                //connInfo.Password = passw;

                //TableLogOnInfo tableLogOnInfo = new TableLogOnInfo();
                //tableLogOnInfo.ConnectionInfo = connInfo;

                //foreach (CrystalDecisions.CrystalReports.Engine.Table table in oRpt.Database.Tables)
                //{
                //    table.ApplyLogOnInfo(tableLogOnInfo);
                //    table.LogOnInfo.ConnectionInfo.ServerName = connInfo.ServerName;
                //    table.LogOnInfo.ConnectionInfo.DatabaseName = connInfo.DatabaseName;
                //    table.LogOnInfo.ConnectionInfo.UserID = connInfo.UserID;
                //    table.LogOnInfo.ConnectionInfo.Password = connInfo.Password;

                //    // Apply the schema name to the table's location
                //    table.Location = "DBXSCHEMA." + table.Location;
                //}

                string tem = "", nomA = "";
                TableLogOnInfo crTableLogOnInfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();

                //crConnectionInfo.ServerName = ConfigurationManager.AppSettings["DataBase"];
                crConnectionInfo.ServerName = ConfigurationManager.AppSettings["DataBase" + GlobalData.getEMPRESA()];// para 2015
                crConnectionInfo.UserID = nomuser;
                crConnectionInfo.Password = passw;

                CrystalDecisions.CrystalReports.Engine.Database crDatabase;
                CrystalDecisions.CrystalReports.Engine.Tables crTables;

                crDatabase = oRpt.Database;
                crTables = crDatabase.Tables;

                foreach (CrystalDecisions.CrystalReports.Engine.Table tabla1 in oRpt.Database.Tables)
                {
                    if(odbcReport == false)
                    {
                        //Conexion normal...
                        tem = tabla1.Name.ToString();

                        InfoConexBd = tabla1.LogOnInfo;

                        InfoConexBd.ConnectionInfo.ServerName = ConfigurationManager.AppSettings["DataBase" + GlobalData.getEMPRESA()]; // para 2015
                                                                                                                                        //InfoConexBd.ConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["DataBase" + GlobalData.getEMPRESA()];
                        InfoConexBd.ConnectionInfo.UserID = nomuser;
                        InfoConexBd.ConnectionInfo.Password = passw;
                        InfoConexBd.TableName = "DBXSCHEMA." + tem;
                        tabla1.ApplyLogOnInfo(InfoConexBd);
                    }
                    else
                    {
                        //Conexion con ODBC...
                        tem = tabla1.Name.ToString();

                        InfoConexBd = tabla1.LogOnInfo;
                        InfoConexBd.ConnectionInfo.ServerName = "ecascloud";// ConfigurationManager.AppSettings["DataBase" + GlobalData.getEMPRESA()]; // para 2015
                                                                            //InfoConexBd.ConnectionInfo.ServerName = "Driver ={ IBM DB2 ODBC DRIVER}; Database = "+ ConfigurationManager.AppSettings["DataBase" + GlobalData.getEMPRESA()]  + "; Hostname = " + ConfigurationManager.AppSettings["Server" + GlobalData.getEMPRESA()]  + "; Port = 50000; Protocol = TCPIP; Uid = db2admin; Pwd =.ecas2010.;"; // para 2015
                        InfoConexBd.ConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["DataBase" + GlobalData.getEMPRESA()];
                        InfoConexBd.ConnectionInfo.UserID = nomuser;
                        InfoConexBd.ConnectionInfo.Password = passw;
                        //InfoConexBd.TableName = "DBXSCHEMA." + tem;
                        tabla1.ApplyLogOnInfo(InfoConexBd);

                        tabla1.Location = "DBXSCHEMA." + tabla1.Location;
                    }
                }

                //try
                //{
                //    //now update logon info for all sub-reports
                //    if (!oRpt.IsSubreport && oRpt.Subreports != null && oRpt.Subreports.Count > 0)
                //    {
                //        foreach (ReportDocument rd in oRpt.Subreports)
                //        {
                //            //SetCRLogOnInfo(rd, dataSource, userId, pwd);
                //            foreach (CrystalDecisions.CrystalReports.Engine.Table tabla1 in rd.Database.Tables)
                //            {
                //                tem = tabla1.Name.ToString();
                //                InfoConexBd = tabla1.LogOnInfo;
                //                //InfoConexBd.ConnectionInfo.ServerName = ConfigurationManager.AppSettings["DataBase"];
                //                InfoConexBd.ConnectionInfo.ServerName = ConfigurationManager.AppSettings["DataBase" + GlobalData.EMPRESA]; // para 2015
                //                InfoConexBd.ConnectionInfo.DatabaseName = "";
                //                InfoConexBd.ConnectionInfo.UserID = nomuser;
                //                InfoConexBd.ConnectionInfo.Password = passw;
                //                InfoConexBd.TableName = "DBXSCHEMA." + tem;
                //                tabla1.ApplyLogOnInfo(InfoConexBd);

                //            }
                //        }
                //    }
                //}
                //catch
                //{
                //}

                paso = "paso :Asigno Conexiones a tablas";
                exportOpts = oRpt.ExportOptions;

                if (tipo == "pdf")
                {
                    exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;
                    exportOpts.FormatOptions = PDFFormatOpts;
                }
                if (tipo == "doc")
                {
                    exportOpts.ExportFormatType = ExportFormatType.WordForWindows;
                    exportOpts.FormatOptions = PDFFormatOpts;

                }
                if (tipo == "xls")
                {
                    ExcelFormato.ExcelTabHasColumnHeadings = false;
                    //ExcelFormato.ExcelUseConstantColumnWidth = true;
                    //ExcelFormato.ExcelConstantColumnWidth = 300;
                    //ExcelFormato.ShowGridLines = true;
                    exportOpts.ExportFormatType = ExportFormatType.Excel;
                    exportOpts.FormatOptions = ExcelFormato;
                }
                if (tipo == "rtf")
                {
                    exportOpts.ExportFormatType = ExportFormatType.RichText;
                    exportOpts.FormatOptions = PDFFormatOpts;
                }

                exportOpts.ExportDestinationType = ExportDestinationType.DiskFile;
                nomA = archivo + "_" + HttpContext.Current.User.Identity.Name.ToLower() + "." + tipo;
                documento = ConfigurationManager.AppSettings["PathToPreviews"] + nomA;
                diskOpts.DiskFileName = ConfigurationManager.AppSettings["PathToReports"] + nomA;
                exportOpts.DestinationOptions = diskOpts;

                paso = "paso :antes de Export";
                if (File.Exists(ConfigurationManager.AppSettings["PathToReports"] + nomA))
                {
                    File.Delete(ConfigurationManager.AppSettings["PathToReports"] + nomA);
                }

                oRpt.Export();
            }
            catch (Exception ex)
            {
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.ErrorCrystalReports, string.Empty, ex,
                    "Error en PreviewReport : " +
                    ex.Message + "  .:.  " +
                    ex.InnerException + "  .:.  " +
                    ex.Source + "  .:.  " +
                    " en el " + paso);
                mensajes += ex.Message + "  .:.  " +
                    ex.InnerException + "  .:.  " +
                    ex.Source + "  .:.  ";
                exito = false;
            }
            finally
            {
                ReportUnload();
            }
		}

        //private static ConnectionInfo GetConnectionInfo(string user, string pass)
        //{
        //    // DbConnectionAttributes contains some, but not all, consts.
        //    var logonProperties = new DbConnectionAttributes();
        //    //logonProperties.Collection.Set("Connection String", @"Driver={SQL Server};Server=TODD-PC\SQLEXPRESS2;Trusted_Connection=Yes;");
        //    //logonProperties.Collection.Set("UseDSNProperties", false);
        //    logonProperties.Collection.Set("Database", "AYCOSERV"); //ConfigurationManager.AppSettings["DataBase" + GlobalData.EMPRESA]);
        //    logonProperties.Collection.Set("Server", "AYCOSERV");//ConfigurationManager.AppSettings["Server" + GlobalData.EMPRESA]);
        //    logonProperties.Collection.Set("UseDSNProperties", false);

        //    var connectionAttributes = new DbConnectionAttributes();
        //    connectionAttributes.Collection.Set("Database DLL", "crdb_db2cli.dll");
        //    connectionAttributes.Collection.Set("QE_DatabaseName", "AYCOSERV"); // ConfigurationManager.AppSettings["DataBase" + GlobalData.EMPRESA]); // String.Empty);
        //    connectionAttributes.Collection.Set("QE_DatabaseType", "DB2 Unicode");
        //    connectionAttributes.Collection.Set("QE_LogonProperties", logonProperties);
        //    connectionAttributes.Collection.Set("QE_ServerDescription", "AYCOSERV");//ConfigurationManager.AppSettings["Server" + GlobalData.EMPRESA]);
        //    connectionAttributes.Collection.Set("QE_SQLDB", true);
        //    connectionAttributes.Collection.Set("SSO Enabled", false);

        //    return new ConnectionInfo
        //    {
        //        Attributes = connectionAttributes,
        //        // These don't seem necessary, but we'll include them anyway: ReportDocument.Load does
        //        ServerName = "AYCOSERV", //ConfigurationManager.AppSettings["Server" + GlobalData.EMPRESA], //@"TODD-PC\SQLEXPRESS2",
        //        DatabaseName = "AYCOSERV", //ConfigurationManager.AppSettings["DataBase" + GlobalData.EMPRESA],
        //        UserID = user,
        //        Password = pass,
        //        Type = ConnectionInfoType.CRQE
        //    };
        //}

		public DataTable RetornarInformacionParametros()
		{
			DataTable dt=null;
			EstablecerColumnasDt(ref dt);
			if(this.oRpt!=null)
			{
				for(int i=0;i<oRpt.DataDefinition.ParameterFields.Count;i++)
				{
					string nombreReportePadre=oRpt.DataDefinition.ParameterFields[i].ReportName;
					if(nombreReportePadre=="")
					{
						DataRow fila=dt.NewRow();
						fila[0]=oRpt.DataDefinition.ParameterFields[i].Name;//Nombre del Parametro
						fila[1]=oRpt.DataDefinition.ParameterFields[i].PromptText;//Texto Descriptivo
						fila[2]=oRpt.DataDefinition.ParameterFields[i].ParameterType.ToString();//Tipo de Parametro
						fila[3]=oRpt.DataDefinition.ParameterFields[i].ValueType.ToString();//Tipo de Dato del Parametro
						fila[4]=oRpt.DataDefinition.ParameterFields[i].DiscreteOrRangeKind.ToString();//Como recibe lo valores, por rango o discretos o ambos
						dt.Rows.Add(fila);
					}
				}
			}
			return dt;
		}

		private void EstablecerColumnasDt(ref DataTable dt)
		{
			dt=new DataTable();
			dt.Columns.Add("NOMBRE",typeof(string));
			dt.Columns.Add("MOSTRAR",typeof(string));
			dt.Columns.Add("TIPOPAR",typeof(string));
			dt.Columns.Add("TIPODAT",typeof(string));
			dt.Columns.Add("RANGOS",typeof(string));
		}

		private void EstablecerValoresParametros()
		{
			DataTable dtPars=this.RetornarInformacionParametros();
			ParameterDiscreteValue discreteValue;
			ParameterValues currentValues;
			ParameterRangeValue rangeParam;
			for(int i=0;i<dtPars.Rows.Count;i++)
			{
				DataRow[] valores=dtValPar.Select("NOMBRE='"+dtPars.Rows[i][0].ToString()+"'");
				for(int j=0;j<valores.Length;j++)
				{ 
					//Si el parametro recibe valores entre un rango
					if(dtPars.Rows[i][4].ToString()=="DiscreteAndRangeValue" || dtPars.Rows[i][4].ToString()=="RangeValue")
					{
						//Si el nombre del parametro es el mismo
						if(dtPars.Rows[i][0].ToString()==valores[j][0].ToString())
						{
							//Si es una fecha
							if(dtPars.Rows[i][3].ToString()=="DateField")
							{
								try
								{
									rangeParam=new ParameterRangeValue();
									currentValues=new ParameterValues();
									rangeParam.StartValue=Convert.ToDateTime(valores[j][1]);
									rangeParam.EndValue=Convert.ToDateTime(valores[j+1][1]);
									currentValues.Add(rangeParam);
									oRpt.DataDefinition.ParameterFields[valores[j][0].ToString()].ApplyCurrentValues(currentValues);
									break;
								}
								catch(Exception e)
								{
									AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.ErrorCrystalReports,string.Empty,e,"Error en EstablecerValoresValue : " + e.Message);
									mensajes=e.Message;
								}
							}
								//Si es fecha/hora
							else if(dtPars.Rows[i][3].ToString()=="DateTimeField")
							{
								try
								{
									rangeParam=new ParameterRangeValue();
									currentValues=new ParameterValues();
									rangeParam.StartValue=Convert.ToDateTime(valores[j][1]);
									rangeParam.EndValue=Convert.ToDateTime(valores[j+1][1]);
									currentValues.Add(rangeParam);
									oRpt.DataDefinition.ParameterFields[valores[j][0].ToString()].ApplyCurrentValues(currentValues);
									break;
								}
								catch(Exception e)
								{
									AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.ErrorCrystalReports,string.Empty,e,"Error en EstablecerValoresValue : " + e.Message);
				
									mensajes=e.Message;
								}
							}
								//Si es un campo entero
							else if(dtPars.Rows[i][3].ToString()=="Int16sField" || dtPars.Rows[i][3].ToString()=="Int16uField" || dtPars.Rows[i][3].ToString()=="Int32sField" || dtPars.Rows[i][3].ToString()=="Int32uField" || dtPars.Rows[i][3].ToString()=="Int8sField" || dtPars.Rows[i][3].ToString()=="Int8uField")
							{
								try
								{
									rangeParam=new ParameterRangeValue();
									currentValues=new ParameterValues();
									rangeParam.StartValue=Convert.ToInt32(valores[j][1]);
									rangeParam.EndValue=Convert.ToInt32(valores[j+1][1]);
									currentValues.Add(rangeParam);
									oRpt.DataDefinition.ParameterFields[valores[j][0].ToString()].ApplyCurrentValues(currentValues);
									break;
								}
								catch(Exception e)
								{
									AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.ErrorCrystalReports,string.Empty,e,"Error en EstablecerValoresValue : " + e.Message);
									mensajes=e.Message;
								}
							}
								//Si es un campo numérico con decimales
							else if(dtPars.Rows[i][3].ToString()=="NumberField")
							{
								try
								{
									rangeParam=new ParameterRangeValue();
									currentValues=new ParameterValues();
									rangeParam.StartValue=Convert.ToDouble(valores[j][1]);
									rangeParam.EndValue=Convert.ToDouble(valores[j+1][1]);
									currentValues.Add(rangeParam);
									oRpt.DataDefinition.ParameterFields[valores[j][0].ToString()].ApplyCurrentValues(currentValues);
									break;
								}
								catch(Exception e)
								{
									AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.ErrorCrystalReports,string.Empty,e,"Error en EstablecerValoresValue : " + e.Message);
									mensajes=e.Message;
								}
							}
								//Si es un campo cadena de caracteres
							else if(dtPars.Rows[i][3].ToString()=="StringField")
							{
								try
								{
									rangeParam=new ParameterRangeValue();
									currentValues=new ParameterValues();
									rangeParam.StartValue=valores[j][1].ToString();
									rangeParam.EndValue=valores[j+1][1].ToString();
									currentValues.Add(rangeParam);
									oRpt.DataDefinition.ParameterFields[valores[j][0].ToString()].ApplyCurrentValues(currentValues);
									break;
								}
								catch(Exception e)
								{
									AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.ErrorCrystalReports,string.Empty,e,"Error en EstablecerValoresValue : " + e.Message);				
									mensajes=e.Message;
								}
							}
								//Si es un campo de hora
							else if(dtPars.Rows[i][3].ToString()=="TimeField")
							{
								try
								{
									rangeParam=new ParameterRangeValue();
									currentValues=new ParameterValues();
									rangeParam.StartValue=Convert.ToDateTime(valores[j][1]);
									rangeParam.EndValue=Convert.ToDateTime(valores[j+1][1]);
									currentValues.Add(rangeParam);
									oRpt.DataDefinition.ParameterFields[valores[j][0].ToString()].ApplyCurrentValues(currentValues);
									break;
								}
								catch(Exception e)
								{
									AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.ErrorCrystalReports,string.Empty,e,"Error en EstablecerValoresValue : " + e.Message);				
									mensajes=e.Message;
								}
							}
						}
					}
					else
					{
						if(dtPars.Rows[i][0].ToString()==valores[j][0].ToString())
						{
							//Si es una fecha
							if(dtPars.Rows[i][3].ToString()=="DateField")
							{
								try
								{
									discreteValue=new ParameterDiscreteValue();
									currentValues=new ParameterValues();
									discreteValue.Value=Convert.ToDateTime(valores[j][1]);
									currentValues.Add(discreteValue);
									oRpt.DataDefinition.ParameterFields[valores[j][0].ToString()].ApplyCurrentValues(currentValues);
									break;
								}
								catch(Exception e)
								{
									AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.ErrorCrystalReports,string.Empty,e,"Error en EstablecerValoresValue : " + e.Message);				
									mensajes=e.Message;
								}
							}
								//Si es fecha/hora
							else if(dtPars.Rows[i][3].ToString()=="DateTimeField")
							{
								try
								{
									discreteValue=new ParameterDiscreteValue();
									currentValues=new ParameterValues();
									discreteValue.Value=Convert.ToDateTime(valores[j][1]);
									currentValues.Add(discreteValue);
									oRpt.DataDefinition.ParameterFields[valores[j][0].ToString()].ApplyCurrentValues(currentValues);
									break;
								}
								catch(Exception e)
								{
									AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.ErrorCrystalReports,string.Empty,e,"Error en EstablecerValoresValue : " + e.Message);				
									mensajes=e.Message;
								}
							}
								//Si es un campo entero
							else if(dtPars.Rows[i][3].ToString()=="Int16sField" || dtPars.Rows[i][3].ToString()=="Int16uField" || dtPars.Rows[i][3].ToString()=="Int32sField" || dtPars.Rows[i][3].ToString()=="Int32uField" || dtPars.Rows[i][3].ToString()=="Int8sField" || dtPars.Rows[i][3].ToString()=="Int8uField")
							{
								try
								{
									discreteValue=new ParameterDiscreteValue();
									currentValues=new ParameterValues();
									discreteValue.Value=Convert.ToInt32(valores[j][1]);
									currentValues.Add(discreteValue);
									oRpt.DataDefinition.ParameterFields[valores[j][0].ToString()].ApplyCurrentValues(currentValues);
									break;
								}
								catch(Exception e)
								{
									AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.ErrorCrystalReports,string.Empty,e,"Error en EstablecerValoresValue : " + e.Message);				
									mensajes=e.Message;
								}
							}
								//Si es un campo numérico con decimales
							else if(dtPars.Rows[i][3].ToString()=="NumberField")
							{
								try
								{
									discreteValue=new ParameterDiscreteValue();
									currentValues=new ParameterValues();
									discreteValue.Value=Convert.ToDouble(valores[j][1]);
									currentValues.Add(discreteValue);
									oRpt.DataDefinition.ParameterFields[valores[j][0].ToString()].ApplyCurrentValues(currentValues);
									break;
								}
								catch(Exception e)
								{
									AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.ErrorCrystalReports,string.Empty,e,"Error en EstablecerValoresValue : " + e.Message);				
									mensajes=e.Message;
								}
							}
								//Si es un campo cadena de caracteres
							else if(dtPars.Rows[i][3].ToString()=="StringField")
							{
								try
								{
									discreteValue=new ParameterDiscreteValue();
									currentValues=new ParameterValues();
									discreteValue.Value=valores[j][1].ToString();
									currentValues.Add(discreteValue);
									oRpt.DataDefinition.ParameterFields[valores[j][0].ToString()].ApplyCurrentValues(currentValues);
									break;
								}
								catch(Exception e)
								{
									AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.ErrorCrystalReports,string.Empty,e,"Error en EstablecerValoresValue : " + e.Message);				
									mensajes=e.Message;
								}
							}
								//Si es un campo de hora
							else if(dtPars.Rows[i][3].ToString()=="TimeField")
							{
								try
								{
									discreteValue=new ParameterDiscreteValue();
									currentValues=new ParameterValues();
									discreteValue.Value=Convert.ToDateTime(valores[j][1]);
									currentValues.Add(discreteValue);
									oRpt.DataDefinition.ParameterFields[valores[j][0].ToString()].ApplyCurrentValues(currentValues);
									break;
								}
								catch(Exception e)
								{
									AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.ErrorCrystalReports,string.Empty,e,"Error en EstablecerValoresValue : " + e.Message);
									mensajes=e.Message;
								}
							}
						}
					}
				}
			}
		}
  

        public void ReportUnload()
        {
            try
            {
                oRpt.Close();
                oRpt.Dispose();
                GC.Collect();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }

	public class ListarPrinter
	{
		private ArrayList ValoresImpresora = new ArrayList();

		public ListarPrinter()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}

		public ArrayList Valores
		{
			get
			{
				return ValoresImpresora;
			}
			set
			{
				ValoresImpresora = value;
			}
		}

		public void mirar()
		{
			ManagementScope scope = new ManagementScope(@"\root\cimv2");
			scope.Connect();

			ManagementObjectSearcher searcher = new 
				ManagementObjectSearcher("SELECT * FROM Win32_Printer");

			string printerName = "";
				foreach (ManagementObject printer in searcher.Get())                                                                                                    
			{
				printerName = printer["Name"].ToString().ToLower();
				ValoresImpresora.Add(printerName);
			}
		}
	}

	public class FormatosDocumentos
	{
		private string prefijo,mensajes,codigo,documento;
		private string ConString=ConfigurationManager.AppSettings["ConnectionString"];
		private int numero;
		private DataSet ds;

		public string Prefijo{set{prefijo=value;} get{return prefijo;}}
		public string Mensajes{set{mensajes=value;} get{return mensajes;}}
		public string Codigo{set{codigo=value;} get{return codigo;}}
		public string Documento{set{documento=value;} get{return documento;}}
		public int Numero{set{numero=value;} get{return numero;}}

		private AMS.DB.DB2Functions db2f = new DB2Functions();

		public FormatosDocumentos()
		{
			ds=new DataSet();
			prefijo="";
			mensajes="";
			codigo="";
		}

		public void Obtener_Datos_Formato()
		{
			ds.Tables.Clear();
			ds.AcceptChanges();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT * FROM SFORMATODOCUMENTOCRYSTAL WHERE SFOR_CODIGO='"+codigo+"'");
		}

		public bool Cargar_Formato()
		{
			Obtener_Datos_Formato();
			string sql="";
			string crevis="";
			try
			{
				if(ds.Tables[0].Rows[0][9].ToString()=="N")
				{
					string vistasO=ds.Tables[0].Rows[0][3].ToString().Trim();
					if(vistasO.Length>0)
					{
						string[] vistas=vistasO.Split(';');
						if(vistas.Length>0)
						{
							for(int i=0;i<vistas.Length;i++)
								if (!vistas[i].ToUpper().Equals("VRELLENO") && DBFunctions.RecordExist("select NAME from sysibm.SYSTABLES where NAME='" + vistas[i] + "_R';"))
									sql+="DROP VIEW DBXSCHEMA."+vistas[i]+"_R;";
							DBFunctions.NonQuery(sql);
							for (int i = 0; i < vistas.Length; i++)
								if (!vistas[i].ToUpper().Equals("VRELLENO"))
									crevis+="CREATE VIEW DBXSCHEMA."+vistas[i]+"_R AS SELECT * FROM DBXSCHEMA."+vistas[i]+" WHERE "+ds.Tables[0].Rows[0][4].ToString()+"='"+this.prefijo+"' AND "+ds.Tables[0].Rows[0][5].ToString()+"="+this.numero.ToString()+";";
							DBFunctions.NonQuery(crevis);
						}
					}
				}
                string usuario=ConfigurationManager.AppSettings["UID"];
                string password = ConfigurationManager.AppSettings["PWD" + GlobalData.getEMPRESA()];
                AMS.CriptoServiceProvider.Crypto miCripto=new Crypto(AMS.CriptoServiceProvider.Crypto.CryptoProvider.TripleDES);
                miCripto.IV = ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
                miCripto.Key = ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
                string newPwd=miCripto.DescifrarCadena(password);
                PreviewReport(ds.Tables[0].Rows[0][2].ToString(), ds.Tables[0].Rows[0][1].ToString(), "pdf", usuario, newPwd, ds.Tables[0].Rows[0][6].ToString(), ds.Tables[0].Rows[0][7].ToString(), ds.Tables[0].Rows[0][8].ToString(), ds.Tables[0].Rows[0][4].ToString(), ds.Tables[0].Rows[0][5].ToString());
                //							rpt								nombre archivo			tipo  usuario contraseña			vista total						campo total					formula total letras					prefijo							numero
                return true; 
			}
			catch(Exception e) 
			{
				mensajes+="Error. Detalles "+e.Message;
				AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.ErrorCrystalReports,String.Empty,e,mensajes);
				return false;
			}
		}

		public void PreviewReport(string Rpt,string archivo,string tipo,string nomuser,string passw,string nomvistatotal,string nomtotal,string nomformulaletras,string pref,string num)
		{
			ExportOptions exportOpts = new ExportOptions();
			DiskFileDestinationOptions diskOpts = new DiskFileDestinationOptions();
			PdfRtfWordFormatOptions PDFFormatOpts = new PdfRtfWordFormatOptions();
			TableLogOnInfo InfoConexBd;
			FormulaFieldDefinitions crFormulas;
            ReportDocument oRpt = new ReportDocument();

			try
			{
				
				string NombreReporte = ConfigurationManager.AppSettings["PathToReportsSource"] + Rpt + ".rpt";
				oRpt.Load(NombreReporte); 
				string tem = "";
				foreach(CrystalDecisions.CrystalReports.Engine.Table tabla1 in oRpt.Database.Tables)
				{
					tem = tabla1.Name.ToString();
					InfoConexBd = tabla1.LogOnInfo;
                    InfoConexBd.ConnectionInfo.ServerName = ConfigurationManager.AppSettings["DataBase" + GlobalData.getEMPRESA()];
					InfoConexBd.ConnectionInfo.UserID = nomuser;
					InfoConexBd.ConnectionInfo.Password = passw;
					tabla1.ApplyLogOnInfo(InfoConexBd);
				}
				try
				{
					 
					ParameterDiscreteValue paramPrefijo=new ParameterDiscreteValue();
					ParameterDiscreteValue paramNumero=new ParameterDiscreteValue();
					ParameterValues currentPrefijo=new ParameterValues();
					ParameterValues currentNumero=new ParameterValues();
					paramPrefijo.Value=prefijo;
					paramNumero.Value=Convert.ToInt32(numero);
					currentPrefijo.Add(paramPrefijo);
					currentNumero.Add(paramNumero);
                    oRpt.DataDefinition.ParameterFields["Prefijo"].ApplyCurrentValues(currentPrefijo); //acá se estalla a ratos
                    oRpt.DataDefinition.ParameterFields["Numero"].ApplyCurrentValues(currentNumero);

                    //oRpt.SetParameterValue("Prefijo", prefijo);
                    //oRpt.SetParameterValue("Numero", numero);


                }
				catch(Exception e)
				{
					mensajes+="Error en PreviewReport :" + e.Message;
					AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.ErrorCrystalReports,string.Empty,e,mensajes);
				}
				if(nomtotal!=string.Empty && nomvistatotal!=string.Empty)
				{
					string[] arrNomTotal=nomtotal.Split(';');
					string[] arrNomVistaTotal=nomvistatotal.Split(';');
					string[] arrNomForTotal=nomformulaletras.Split(';');
					string valor="",letras="",valorCheque="",letrasCheque="";
					double val=0,valChe=0;
					//CASO ESPECIAL PARA LOS COMP DE EGRESO, TOCA PONER EL VALOR EN LETRAS DEL CHEQUE
					if(arrNomTotal.Length>1 && arrNomVistaTotal.Length>1 && arrNomForTotal.Length>1)
					{
						crFormulas=oRpt.DataDefinition.FormulaFields;
						//VALOR EN LETRAS CHEQUE
						valorCheque=DBFunctions.SingleData("SELECT "+arrNomTotal[1]+" FROM dbxschema."+arrNomVistaTotal[1]+" WHERE "+pref+"='"+prefijo+"' AND "+num+"="+numero+" FETCH FIRST 1 ROWS ONLY");
						valChe=Math.Round(Convert.ToDouble(valorCheque),2);
						Interprete miInterprete=new Interprete();
						letrasCheque=miInterprete.Letras(valChe.ToString());
						crFormulas[arrNomForTotal[1]].Text='"'+letrasCheque+'"';
						//VALOR EN LETRAS DOCUMENTO
						valor=DBFunctions.SingleData("SELECT "+arrNomTotal[0]+" FROM dbxschema."+arrNomVistaTotal[0]+" WHERE "+pref+"='"+prefijo+"' AND "+num+"="+numero+" FETCH FIRST 1 ROWS ONLY");
						val=Math.Round(Convert.ToDouble(valor),2);
						miInterprete=new Interprete();
						letras=miInterprete.Letras(val.ToString());
						crFormulas[arrNomForTotal[0]].Text='"'+"SON : "+letras+'"';
					}
					else
					{
						crFormulas=oRpt.DataDefinition.FormulaFields;
						valor=DBFunctions.SingleData("SELECT "+nomtotal+" FROM dbxschema."+nomvistatotal+" WHERE "+pref+"='"+prefijo+"' AND "+num+"="+numero+" FETCH FIRST 1 ROWS ONLY");
						val=Math.Round(Convert.ToDouble(valor),2);
						Interprete miInterprete=new Interprete();
						letras=miInterprete.Letras(val.ToString());
						crFormulas[nomformulaletras].Text='"'+"SON : "+letras+'"';
					}
				}
				string nomA = "";
				exportOpts = oRpt.ExportOptions;
				if (tipo == "pdf") 
				{
					exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;
					exportOpts.FormatOptions = PDFFormatOpts;
				}
				exportOpts.ExportDestinationType = ExportDestinationType.DiskFile;
				nomA = archivo + "-" + this.prefijo + "-" + this.numero.ToString() + "_" + HttpContext.Current.User.Identity.Name.ToLower() + "." + tipo;
				diskOpts.DiskFileName = ConfigurationManager.AppSettings["PathToReports"] + nomA;
				exportOpts.DestinationOptions = diskOpts;
                //    string hector = exportOpts.ToString();

                oRpt.Export();
				documento = ConfigurationManager.AppSettings["PathToPreviews"] + nomA;
				AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.InformacionCrystalRepots,string.Empty,null,"Documento a Imprimir :" + documento);
			}
			catch (Exception ex)
			{
				mensajes+="Error al generar formato. .:. <br> "
                    + ex.Message + " .:. <br> "
                    + ex.InnerException + " .:. <br> "
                    + ex.Source + " .:. <br> "
                    + ex.StackTrace + " .:. <br> "
                    + ex.HelpLink;
				AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.ErrorCrystalReports,string.Empty,ex,mensajes);
			}
            finally
            {
                ReportUnload(oRpt);
            }
		}

        public void ReportUnload(ReportDocument oRpt)
        {
            try
            {
                oRpt.Close();
                oRpt.Dispose();
                GC.Collect();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
	}
}