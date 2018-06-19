using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AMS.DB;
using AMS.Forms;


namespace AMS.Gerencial
{
    public partial class RegistroMensual : System.Web.UI.UserControl
    {
        #region variables
        //almacena el valor del radibutton seleccionado
        public const string CADENANULO = "NULL";
        public const string FORMATOFECHA = "yyyy-MM-dd";
        DatasToControls bind = new DatasToControls();
        ArrayList sqlLista = new ArrayList();
        
        
        #endregion variables

        #region PROPIEDADES
        
        //control de exepciones de los datos almacenados por el usuario
        public int consecutivo { get { return InterpretarCadenaVaciaEntero(txtConsecutivo.Text); } set { txtConsecutivo.Text = InterpretarValorNuloAMostrar(value); } }
        public string concesion { get { return ddlConcesion.SelectedValue; } set { DatasToControls.EstablecerValueDropDownList(ddlConcesion, value); } }
        public string concesionario { get { return ddlConcesionario.SelectedValue; } set { DatasToControls.EstablecerValueDropDownList(ddlConcesionario, value); } }
        
        public string fechadiligenciamiento{ get { return txtFechadiligenciamiento.Text; } set { txtFechadiligenciamiento.Text = value; } }
        
        public string mes0{ get { return ddlmes0.SelectedValue; } set { DatasToControls.EstablecerValueDropDownList(ddlmes0, value); } }
        public string año{ get { return ddlaño.SelectedValue; } set { DatasToControls.EstablecerValueDropDownList(ddlaño, value); } }
        public int diaslaborales{ get { return InterpretarCadenaVaciaEntero(txtDiaslaborales.Text); } set { txtDiaslaborales.Text = InterpretarValorNuloAMostrar(value); } }
        public string diligenciadopor{ get { return txtDiligenciadopor.Text; } set { txtDiligenciadopor.Text = value; } }
        
        public string mes { get { return ddlregistros.SelectedValue; } set { DatasToControls.EstablecerValueDropDownList(ddlregistros, value); } }

        public int entradascli { get { return InterpretarCadenaVaciaEntero(txtentradascli.Text); } set { txtentradascli.Text = InterpretarValorNuloAMostrar(value); } }
        public int entradasgara { get { return InterpretarCadenaVaciaEntero(txtentradasga.Text); } set { txtentradasga.Text = InterpretarValorNuloAMostrar(value); } }
        public int entradasinter { get { return InterpretarCadenaVaciaEntero(txtentradasint.Text); } set { txtentradasint.Text = InterpretarValorNuloAMostrar(value); } }
        public int entradasserv { get { return InterpretarCadenaVaciaEntero(txtentradasserv.Text); } set { txtentradasserv.Text = InterpretarValorNuloAMostrar(value); } }
        public int entradaspdi { get { return InterpretarCadenaVaciaEntero(txtentradaspdi.Text); } set { txtentradaspdi.Text = InterpretarValorNuloAMostrar(value); } }

        public double manocli { get { return InterpretarCadenaVaciaDoble(txtmanodeobracli.Text); } set { txtmanodeobracli.Text = InterpretarValorNuloAMostrar(value); } }
        public double manogara { get { return InterpretarCadenaVaciaDoble(txtmanodeobragara.Text); } set { txtmanodeobragara.Text = InterpretarValorNuloAMostrar(value); } }
        public double manoint { get { return InterpretarCadenaVaciaDoble(txtmanodeobraint.Text); } set { txtmanodeobraint.Text = InterpretarValorNuloAMostrar(value); } }
        public double manopdi { get { return InterpretarCadenaVaciaDoble(txtmanodeobrapdi.Text); } set { txtmanodeobrapdi.Text = InterpretarValorNuloAMostrar(value); } }
        public double repcli { get { return InterpretarCadenaVaciaDoble(txtrepuestoscli.Text); } set { txtrepuestoscli.Text = InterpretarValorNuloAMostrar(value); } }
        public double repgara { get { return InterpretarCadenaVaciaDoble(txtrepuestosgara.Text); } set { txtrepuestosgara.Text = InterpretarValorNuloAMostrar(value); } }
        public double repint { get { return InterpretarCadenaVaciaDoble(txtrepuestosint.Text); } set { txtrepuestosint.Text = InterpretarValorNuloAMostrar(value); } }
        public double repmos { get { return InterpretarCadenaVaciaDoble(txtrepuestosmos.Text); } set {txtrepuestosmos.Text = InterpretarValorNuloAMostrar(value); } }

        public int retrabajocli { get { return InterpretarCadenaVaciaEntero(txthorasretracli.Text); } set { txthorasretracli.Text = InterpretarValorNuloAMostrar(value); } }
        public int retrabajogara { get { return InterpretarCadenaVaciaEntero(txthorasretragara.Text); } set { txthorasretragara.Text = InterpretarValorNuloAMostrar(value); } }
        public int retrabajoint { get { return InterpretarCadenaVaciaEntero(txthorasretraint.Text); } set { txthorasretraint.Text = InterpretarValorNuloAMostrar(value); } }
        public int retrabajootros { get { return InterpretarCadenaVaciaEntero(txthorasretraotr.Text); } set { txthorasretraotr.Text = InterpretarValorNuloAMostrar(value); } }
        public int facturadascli { get { return InterpretarCadenaVaciaEntero(txthorasfacturadascli.Text); } set { txthorasfacturadascli.Text = InterpretarValorNuloAMostrar(value); } }
        public int facturadasgara { get { return InterpretarCadenaVaciaEntero(txthorasfacturadasgara.Text); } set { txthorasfacturadasgara.Text = InterpretarValorNuloAMostrar(value); } }
        public int facturadasint { get { return InterpretarCadenaVaciaEntero(txthorasfacturadasint.Text); } set { txthorasfacturadasint.Text = InterpretarValorNuloAMostrar(value); } }

        public int proapren { get { return InterpretarCadenaVaciaEntero(txtproductivoapren.Text); } set { txtproductivoapren.Text = InterpretarValorNuloAMostrar(value); } }
        public int prolato { get { return InterpretarCadenaVaciaEntero(txtproductivolatoneria.Text); } set { txtproductivolatoneria.Text = InterpretarValorNuloAMostrar(value); } }
        public int prolava { get { return InterpretarCadenaVaciaEntero(txtproductivolavadores.Text); } set { txtproductivolavadores.Text = InterpretarValorNuloAMostrar(value); } }
        public int prootros { get { return InterpretarCadenaVaciaEntero(txtproductivootros.Text); } set { txtproductivootros.Text = InterpretarValorNuloAMostrar(value); } }
        public int protec { get { return InterpretarCadenaVaciaEntero(txtproductivotec.Text); } set { txtproductivotec.Text = InterpretarValorNuloAMostrar(value); } }
        public int noproadmin { get { return InterpretarCadenaVaciaEntero(txtnoproadmin.Text); } set { txtnoproadmin.Text = InterpretarValorNuloAMostrar(value); } }
        public int noproase{ get { return InterpretarCadenaVaciaEntero(txtnoproasesor.Text); } set { txtnoproasesor.Text = InterpretarValorNuloAMostrar(value); } }
        public int noprogeren { get { return InterpretarCadenaVaciaEntero(txtnoprogerente.Text); } set { txtnoprogerente.Text = InterpretarValorNuloAMostrar(value); } }
        public int noprojefe { get { return InterpretarCadenaVaciaEntero(txtnoprojefe.Text); } set { txtnoprojefe.Text = InterpretarValorNuloAMostrar(value); } }
        public int noprootros { get { return InterpretarCadenaVaciaEntero(txtnoprootros.Text); } set { txtnoprootros.Text = InterpretarValorNuloAMostrar(value); } }
        
        #endregion PROPIEDADES
        
        #region METODOS
        public string ValidarValorCadena(object valor)
        {  
            string valorValidado = CADENANULO;

            if (valor is string)
            {
                if ((string)valor == string.Empty)
                    valorValidado = CADENANULO;
                else
                    valorValidado = "'" + valor.ToString() + "'";
            }
            else if (valor is int)
            {
                if ((int)valor == -1)
                    valorValidado = CADENANULO;
                else
                    valorValidado = valor.ToString();
            }
            else if (valor is double)
            {
                if ((double)valor == -1)
                    valorValidado = CADENANULO;
                else
                    valorValidado = valor.ToString();
            }
            else if (valor is DateTime)
            {
                if ((DateTime)valor == DateTime.MinValue)
                    valorValidado = CADENANULO;
                else
                    valorValidado = "DATE('" + Convert.ToDateTime(valor).ToString("yyyy-MM-dd") + "')";
            }         

            return valorValidado;
        }
        public int InterpretarCadenaVaciaEntero(string cadena)
        {
            if (cadena == "")
                return -1;
            else
                return Convert.ToInt32(cadena);
        }
        public double InterpretarCadenaVaciaDoble(string cadena)
        {
            if (cadena == "")
                return 0;
            else
                return Convert.ToDouble(cadena);
        }
        public string InterpretarValorNuloAMostrar(object valor)

        {
            string valorValidado = string.Empty;

            if (valor is string)
            {
                if ((string)valor == string.Empty)
                    valorValidado = string.Empty;
                else
                    valorValidado = valor.ToString();
            }
            else if (valor is int)
            {
                if ((int)valor == -1)
                    valorValidado = string.Empty;
                else
                    valorValidado = valor.ToString();
            }
            else if (valor is double)
            {
                if ((double)valor == -1)
                    valorValidado = string.Empty;
                else
                    valorValidado = valor.ToString();
            }
            else if (valor is DateTime)
            {
                if ((DateTime)valor == DateTime.MinValue)
                    valorValidado = string.Empty;
                else
                    valorValidado = Convert.ToDateTime(valor).ToString(FORMATOFECHA);
            }

            return valorValidado;
        }
        private void Ingresar_datos()
        {
            if (RadioButton1.Checked == true)
            {

                sqlLista.Add(string.Format(@"insert into MCONCESION
                (
                MCON_SECUENCIA,
                PGAM_CODIGO,  
                PCON_CODIGO,  
                PANO_ANO,  
                PMES_MES,
                MCON_FECHDILI,  
                MCON_USUADILI,  
                MCON_DIASLABO,  
                MCON_ENTRTALLCLIE,
                MCON_ENTRTALLGARA,
                MCON_ENTRTALLINTE,  
                MCON_ENTRTALLSERV,
                MCON_ENTRTALLPDI,  
                MCON_HORAFACTTALLECLIE,
                MCON_HORAFACTTALLEGARA,
                MCON_HORAFACTTALLINTE,  
                MCON_VALOFACTTALLCLIE,  
                MCON_VALOFACTTALLGARA,
                MCON_VALOFACTTALLINTE,  
                MCON_VALOFACTTALLPDI,   
                MCON_VALOFACTREPUCLIE,
                MCON_VALOFACTREPUGARA,  
                MCON_VALOFACTREPUINTE,
                MCON_VALOFACTREPUMOST,
                MCON_HORARETRABAJCLIE,  
                MCON_HORARETRABAJGARA,  
                MCON_HORARETRABAJINTE,
                MCON_ENTRTRABOTROTALL,  
                MCON_PERSTALLPRODTECN,
                MCON_PERSTALLPRODAPRE,  
                MCON_PERSTALLPRODLAPI,  
                MCON_PERSTALLPRODLAVA,
                MCON_PERSTALLPRODOTRO,  
                MCON_PERSTALLNOPRGESE,  
                MCON_PERSTALLNOPRJETA,
                MCON_PERSTALLNOPRASSE,  
                MCON_PERSTALLNOPRADMI,  
                MCON_PERSTALLNOPROTRO
                )
	            values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},
                        {16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},
                        {32},{33},{34},{35},{36},{37})", ValidarValorCadena(consecutivo),
                                                    ValidarValorCadena(concesion),
                                                    ValidarValorCadena(concesionario),
                                                    ValidarValorCadena(año),
                                                    ValidarValorCadena(mes0),
                                                    ValidarValorCadena(fechadiligenciamiento),
                                                    ValidarValorCadena(diligenciadopor),
                                                    ValidarValorCadena(diaslaborales),
                                                    ValidarValorCadena(entradascli),
                                                    ValidarValorCadena(entradasgara),
                                                    ValidarValorCadena(entradasinter),
                                                    ValidarValorCadena(entradasserv),
                                                    ValidarValorCadena(entradaspdi),
                                                    ValidarValorCadena(facturadascli),
                                                    ValidarValorCadena(facturadasgara),
                                                    ValidarValorCadena(facturadasint),
                                                    ValidarValorCadena(manocli),
                                                    ValidarValorCadena(manogara),
                                                    ValidarValorCadena(manoint),
                                                    ValidarValorCadena(manopdi),
                                                    ValidarValorCadena(repcli),
                                                    ValidarValorCadena(repgara),
                                                    ValidarValorCadena(repint),
                                                    ValidarValorCadena(repmos),
                                                    ValidarValorCadena(retrabajocli),
                                                    ValidarValorCadena(retrabajogara),
                                                    ValidarValorCadena(retrabajoint),
                                                    ValidarValorCadena(retrabajootros),
                                                    ValidarValorCadena(protec),
                                                    ValidarValorCadena(proapren),
                                                    ValidarValorCadena(prolato),
                                                    ValidarValorCadena(prolava),
                                                    ValidarValorCadena(prootros),
                                                    ValidarValorCadena(noprogeren),
                                                    ValidarValorCadena(noprojefe),
                                                    ValidarValorCadena(noproase),
                                                    ValidarValorCadena(noproadmin),
                                                    ValidarValorCadena(noprootros)

                                                    ));
                bool procesoSatisfactorio = DBFunctions.Transaction(sqlLista);
                if (procesoSatisfactorio)
                    Label46.Text = "Estado...Registro Exitoso";
                else

                    Label46.Text = "Estado...Error en la solicitud" + DBFunctions.exceptions;

            }
            else if (RadioButton2.Checked==true)
            {
                ArrayList sqlLista = new ArrayList();
                    sqlLista.Add(string.Format(@"UPDATE MCONCESION SET
				PGAM_CODIGO = {0},  
                PCON_CODIGO = {1},  
                PANO_ANO = {2},  
                PMES_MES = {3},
                MCON_FECHDILI = {4},  
                MCON_USUADILI = {5},  
                MCON_DIASLABO = {6},
                MCON_ENTRTALLCLIE = {7},
                MCON_ENTRTALLGARA = {8},
                MCON_ENTRTALLINTE = {9},  
                MCON_ENTRTALLSERV = {10},
                MCON_ENTRTALLPDI = {11},  
                MCON_HORAFACTTALLECLIE = {12},
                MCON_HORAFACTTALLEGARA = {13},
                MCON_HORAFACTTALLINTE = {14},  
                MCON_VALOFACTTALLCLIE = {15},  
                MCON_VALOFACTTALLGARA = {16},
                MCON_VALOFACTTALLINTE = {17},  
                MCON_VALOFACTTALLPDI = {18},   
                MCON_VALOFACTREPUCLIE = {19},
                MCON_VALOFACTREPUGARA = {20},  
                MCON_VALOFACTREPUINTE = {21},
                MCON_VALOFACTREPUMOST = {22},
                MCON_HORARETRABAJCLIE = {23},  
                MCON_HORARETRABAJGARA = {24},  
                MCON_HORARETRABAJINTE = {25},
                MCON_ENTRTRABOTROTALL = {26},  
                MCON_PERSTALLPRODTECN = {27},
                MCON_PERSTALLPRODAPRE = {28},  
                MCON_PERSTALLPRODLAPI = {29},  
                MCON_PERSTALLPRODLAVA = {30},
                MCON_PERSTALLPRODOTRO = {31},  
                MCON_PERSTALLNOPRGESE = {32},  
                MCON_PERSTALLNOPRJETA = {33},
                MCON_PERSTALLNOPRASSE = {34},  
                MCON_PERSTALLNOPRADMI = {35},  
                MCON_PERSTALLNOPROTRO = {36} WHERE MCON_SECUENCIA = " + consecutivo, 
                                                    ValidarValorCadena(concesion),
                                                    ValidarValorCadena(concesionario),
                                                    ValidarValorCadena(año),
                                                    ValidarValorCadena(mes0),
                                                    ValidarValorCadena(fechadiligenciamiento),
                                                    ValidarValorCadena(diligenciadopor),
                                                    ValidarValorCadena(diaslaborales),
                                                    ValidarValorCadena(entradascli),
                                                    ValidarValorCadena(entradasgara),
                                                    ValidarValorCadena(entradasinter),
                                                    ValidarValorCadena(entradasserv),
                                                    ValidarValorCadena(entradaspdi),
                                                    ValidarValorCadena(facturadascli),
                                                    ValidarValorCadena(facturadasgara),
                                                    ValidarValorCadena(facturadasint),
                                                    ValidarValorCadena(manocli),
                                                    ValidarValorCadena(manogara),
                                                    ValidarValorCadena(manoint),
                                                    ValidarValorCadena(manopdi),
                                                    ValidarValorCadena(repcli),
                                                    ValidarValorCadena(repgara),
                                                    ValidarValorCadena(repint),
                                                    ValidarValorCadena(repmos),
                                                    ValidarValorCadena(retrabajocli),
                                                    ValidarValorCadena(retrabajogara),
                                                    ValidarValorCadena(retrabajoint),
                                                    ValidarValorCadena(retrabajootros),
                                                    ValidarValorCadena(protec),
                                                    ValidarValorCadena(proapren),
                                                    ValidarValorCadena(prolato),
                                                    ValidarValorCadena(prolava),
                                                    ValidarValorCadena(prootros),
                                                    ValidarValorCadena(noprogeren),
                                                    ValidarValorCadena(noprojefe),
                                                    ValidarValorCadena(noproase),
                                                    ValidarValorCadena(noproadmin),
                                                    ValidarValorCadena(noprootros)));
                bool procesoSatisfactorio = DBFunctions.Transaction(sqlLista);
                if (procesoSatisfactorio)
                    Label46.Text = "Estado...Registro Exitoso";
                else
                    Label46.Text = "Estado...Error en la solicitud" + DBFunctions.exceptions;
            }
        }
        private void clear_txt()
        {
            txtConsecutivo.Text = "";
            txtDiaslaborales.Text = "";
            txtDiligenciadopor.Text = "";
            txtentradascli.Text = "";
            txtentradasga.Text = "";
            txtentradasint.Text = "";
            txtentradaspdi.Text = "";
            txtentradasserv.Text = "";
            txtFechadiligenciamiento.Text = "";
            txthorasfacturadascli.Text = "";
            txthorasfacturadasgara.Text = "";
            txthorasfacturadasint.Text = "";
            txthorasretracli.Text = "";
            txthorasretragara.Text = "";
            txthorasretraint.Text = "";
            txthorasretramost.Text = "";
            txthorasretraotr.Text = "";
            txthorasretrapdi.Text = "";
            txtmanodeobracli.Text = "";
            txtmanodeobragara.Text = "";
            txtmanodeobraint.Text = "";
            txtmanodeobramos.Text = "";
            txtmanodeobraotr.Text = "";
            txtmanodeobrapdi.Text = "";
            txtnoproadmin.Text = "";
            txtnoproasesor.Text = "";
            txtnoprogerente.Text = "";
            txtnoprojefe.Text = "";
            txtnoprootros.Text = "";
            txtproductivoapren.Text = "";
            txtproductivolatoneria.Text = "";
            txtproductivolavadores.Text = "";
            txtproductivootros.Text = "";
            txtproductivotec.Text = "";
            txtrepuestoscli.Text = "";
            txtrepuestosgara.Text = "";
            txtrepuestosint.Text = "";
            txtrepuestosmos.Text = "";
            txtrepuestosotr.Text = "";
            txtrepuestospdi.Text = "";



                
        }  
        private void Obtener_datos()
        {
           
            consecutivo =InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_SECUENCIA from MCONCESION where MCON_SECUENCIA = "+ddlregistros.SelectedValue+ " and PCON_CODIGO = '"+ddlConcesionario.SelectedValue+"'"));
            concesion = DBFunctions.SingleData("select PGAM_CODIGO from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'");
            concesion = DBFunctions.SingleData("select PCON_CODIGO from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'");
            año = DBFunctions.SingleData("select PANO_ANO from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'");
            mes0 = DBFunctions.SingleData("select PMES_MES from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'");
            fechadiligenciamiento = DBFunctions.SingleData("select cast(MCON_FECHDILI as char(10)) from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'");
               
                    
            diligenciadopor = DBFunctions.SingleData("select MCON_USUADILI from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'");
            
            diaslaborales = InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_DIASLABO from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            entradascli =  InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_ENTRTALLCLIE from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            entradasgara =  InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_ENTRTALLGARA from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            entradasinter =  InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_ENTRTALLINTE from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            entradasserv =  InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_ENTRTALLSERV from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            entradaspdi =  InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_ENTRTALLPDI from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            facturadascli =  InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_HORAFACTTALLECLIE from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            facturadasgara =  InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_HORAFACTTALLEGARA from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            facturadasint =  InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_HORAFACTTALLINTE from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            manocli =  InterpretarCadenaVaciaDoble(DBFunctions.SingleData("select MCON_VALOFACTTALLCLIE from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            manogara =  InterpretarCadenaVaciaDoble(DBFunctions.SingleData("select MCON_VALOFACTTALLGARA from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            manoint =  InterpretarCadenaVaciaDoble(DBFunctions.SingleData("select MCON_VALOFACTTALLINTE from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            manopdi =  InterpretarCadenaVaciaDoble(DBFunctions.SingleData("select MCON_VALOFACTTALLPDI from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            repcli =  InterpretarCadenaVaciaDoble(DBFunctions.SingleData("select  MCON_VALOFACTREPUCLIE from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            repgara =  InterpretarCadenaVaciaDoble(DBFunctions.SingleData("select MCON_VALOFACTREPUGARA from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            repint =  InterpretarCadenaVaciaDoble(DBFunctions.SingleData("select MCON_VALOFACTREPUINTEfrom MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            repmos =  InterpretarCadenaVaciaDoble(DBFunctions.SingleData("select MCON_VALOFACTREPUMOST from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            retrabajocli = InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_HORARETRABAJCLIE from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            retrabajogara =  InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_HORARETRABAJGARA from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            retrabajoint =  InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_HORARETRABAJINTE from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            retrabajootros =  InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_ENTRTRABOTROTALL from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            protec = InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_PERSTALLPRODTECN from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            proapren =  InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_PERSTALLPRODAPRE from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            prolato =  InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_PERSTALLPRODLAPI from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            prolava =  InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_PERSTALLPRODLAVA from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            prootros =  InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_PERSTALLPRODOTRO from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            noprogeren =  InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_PERSTALLNOPRGESE from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            noprojefe =  InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_PERSTALLNOPRJETA from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            noproase =  InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_PERSTALLNOPRASSE from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            noproadmin =  InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_PERSTALLNOPRADMI from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            noprootros =  InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select MCON_PERSTALLNOPROTRO from MCONCESION where MCON_SECUENCIA = " + ddlregistros.SelectedValue + " and PCON_CODIGO = '" + ddlConcesionario.SelectedValue + "'"));
            
            
            
        }
        #endregion METODOS

        #region EVENTOS
        protected void Page_Load(object sender, EventArgs e)
        {
            txtFechadiligenciamiento.Text = DateTime.Now.ToString("yyyy-MM-dd");
            if (!Page.IsPostBack)
            {
                
                bind.PutDatasIntoDropDownList(ddlConcesion, string.Format("select PGAM_CODIGO, PGAM_NOMBRE from DBXSCHEMA.PGAMA order by PGAM_CODIGO;"));
                bind.PutDatasIntoDropDownList(ddlConcesionario, string.Format("select PCON_CODIGO, PCON_NOMBRE from  DBXSCHEMA.PCONCESIONARIO order by  PCON_CODIGO;"));
                bind.PutDatasIntoDropDownList(ddlaño, "SELECT PANO_ANO, PANO_ANO FROM DBXSCHEMA.PANO;");
                bind.PutDatasIntoDropDownList(ddlmes0, "SELECT PMES_MES, PMES_NOMBRE FROM DBXSCHEMA.PMES;");
                
               
            } 
         } 
                                
        protected void Button1_Click1(object sender, EventArgs e)
        {
            Ingresar_datos();
            sqlLista.Clear();
            clear_txt();
            
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Obtener_datos();             
        }

        protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Label45.Visible = true;
            ddlregistros.Visible = true;
            Button2.Visible = true;
            Label46.Text = "Estado...";
               
        }

        protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
        {
            Label45.Visible = false;
            ddlregistros.Visible = false;
            Button2.Visible = false;
            Label46.Text = "Estado...";
            
            
        }
        #endregion EVENTOS

        protected void ddlConcesionario_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (RadioButton2.Checked == true)
                bind.PutDatasIntoDropDownList(ddlregistros, string.Format("select MCON_SECUENCIA, cast(MCON_FECHDILI as char(10)) from MCONCESION WHERE DATE(MCON_FECHDILI) >= DATE(CURRENT DATE) - 3 MONTHS and PCON_CODIGO = '" + concesionario + "'"));
            else
                ddlregistros.Items.Clear();
        }
                        
    }
}