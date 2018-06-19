namespace AMS.Gerencial
{
	using System.Collections;
	using System.ComponentModel;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.Services;
	using System.Web.SessionState;
	using System.Web.Services.Protocols;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.Forms;
	using AMS.DB;
	using System.Text;
	using AMS.Tools;
	using System;
	
	/// <summary>
	///		Descripción breve de AMS_Inventarios_ConsultaResumen.
	/// </summary>
	public partial class AMS_Gerencial_ConsultaResumen : System.Web.UI.UserControl
	{
		
		double inventarioinicial=0;
		string invInic=null;
		int año=0;
		int mes=0;
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				Panel1.Visible=true;
				DatasToControls bind = new DatasToControls();
				año=Convert.ToInt32(DBFunctions.SingleData("select PANO_ANO from DBXSCHEMA.CINVENTARIO"));
				mes=Convert.ToInt32(DBFunctions.SingleData("select PMES_MES from DBXSCHEMA.CINVENTARIO"));
		
			    inventarioinicial = Convert.ToDouble(DBFunctions.SingleData("select coalesce(sum(MSAL_CANTINVEINIC*MSAL_COSTPROM),0) from DBXSCHEMA.MSALDOITEM WHERE PANO_ANO=" + año + ""));	
				
				invInic=String.Format("{0:C}",inventarioinicial);
				InvInicial.Text=invInic.ToString();
				InvinicialMes.Text=invInic.ToString();

				
				double ventas1año=0;
				double ventas2año=0;
				double totvent1=0;
				double ventas1mes=0;
				double ventas2mes=0;
				double totvent2=0;
				string VentasFaño=null;
				string VentasFmes=null;
				double trans1año=0;
				double trans2año=0;
				double trans1mes=0;
				double trans2mes=0;
				double comp1año=0;
				double comp2año=0;
				double comp1mes=0;
				double comp2mes=0;
				double ajus1año=0;
                double ajus2año=0;
				double ajus1mes=0;
                double ajus2mes=0;
				double cosven1año=0;
				double cosven2año=0;
				double cosven1mes=0;
				double cosven2mes=0;
				double conin1año=0;
				double conin1mes=0;
				double tranta1año=0;
				double tranta2año=0;
				double tranta1mes=0;
				double tranta2mes=0;
				double margenfin=0;
				double margenfinmes=0;
				double margenreal=0;
				double margenrenmes=0;
				double invsaldopromaño=0;
				double invaltro=0;

				double invt2años=0;
				double invt1año=0;
				double invt6meses=0;
		
				//año
				//string ventas1añoS=DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=90 AND PANO_ANO="+año+" ");
                string ventas1añoS=DBFunctions.SingleData("SELECT SUM((DITE_CANTIDAD*DITE_VALOUNIT)*(1-(DITE_PORCDESC*0.01)))FROM DBXSCHEMA.DITEMS  WHERE PANO_ANO=" +año+ " and " +mes+ ">= 01 and PMES_MES< " +mes+ " and TMOV_TIPOMOVI = 90");
				if (ventas1añoS.Equals(null) || ventas1añoS.Equals(""))
				{
					ventas1año=0;
				}
				else
				{
					//ventas1año=Convert.ToDouble(DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=90 AND PANO_ANO="+año+" "));
                    ventas1año = Convert.ToDouble(DBFunctions.SingleData("SELECT SUM((DITE_CANTIDAD*DITE_VALOUNIT)*(1-(DITE_PORCDESC*0.01)))FROM DBXSCHEMA.DITEMS  WHERE PANO_ANO=" +año+ " and " +mes+ ">= 01 and PMES_MES< " +mes+ " and TMOV_TIPOMOVI = 90"));
				}
				//string veano=DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=91 AND PANO_ANO="+año+" ");
                string veano=DBFunctions.SingleData("SELECT SUM((DITE_CANTIDAD*DITE_VALOUNIT)*(1-(DITE_PORCDESC*0.01))) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+" >= 01 and PMES_MES<  "+mes+" and TMOV_TIPOMOVI = 91");
				if(veano.Equals(null) || veano.Equals(" ")  )
				{
					ventas2año=0;
				}	

				else
				{
					//ventas2año=Convert.ToDouble(DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=91 AND PANO_ANO="+año+" "));
                    ventas2año = Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(SUM((DITE_CANTIDAD*DITE_VALOUNIT)*(1-(DITE_PORCDESC*0.01))),0) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO=" + año + " and " + mes + " >= 01 and PMES_MES<  " + mes + " and TMOV_TIPOMOVI = 91"));
				}
				totvent1=ventas1año-ventas2año;
				VentasFaño=String.Format("{0:C}",totvent1);
				ventasano.Text=VentasFaño.ToString();
				// mes
				//string ventas1mesS=DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=90 AND PANO_ANO="+año+" AND PMES_MES="+mes+" ");
                string ventas1mesS=DBFunctions.SingleData("select SUM((DITE_CANTIDAD*DITE_VALOUNIT)*(1-(DITE_PORCDESC*0.01))) from DBXSCHEMA.DITEMS where PANO_ANO="+año+" and "+mes+"=PMES_MES  and TMOV_TIPOMOVI = 90");
				if(ventas1mesS.Equals(null) || ventas1mesS.Equals("") )
				{
					ventas1mes=0;
				}
				else
				{
					//ventas1mes=Convert.ToDouble(DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=90 AND PANO_ANO="+año+" AND PMES_MES="+mes+" "));
                    ventas1mes = Convert.ToDouble(DBFunctions.SingleData("select SUM((DITE_CANTIDAD*DITE_VALOUNIT)*(1-(DITE_PORCDESC*0.01))) from DBXSCHEMA.DITEMS where PANO_ANO="+año+" and "+mes+"=PMES_MES and TMOV_TIPOMOVI = 90"));
				}
				//string ventas2mesS=DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=91 AND PANO_ANO="+año+" AND PMES_MES="+mes+" ");
                string ventas2mesS=DBFunctions.SingleData("select sum((DITE_CANTIDAD*DITE_VALOUNIT)*(1-(DITE_PORCDESC*0.01))) from dbxschema.ditems where PANO_ANO="+año+" and "+mes+"=PMES_MES and TMOV_TIPOMOVI = 91");
				if(ventas2mesS.Equals(null) || ventas2mesS.Equals("") )
				{
					ventas2mes=0;
				}
				else
				{
					//ventas2mes=Convert.ToDouble(DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=91 AND PANO_ANO="+año+" AND PMES_MES="+mes+" "));
                    ventas2mes = Convert.ToDouble(DBFunctions.SingleData("select sum((DITE_CANTIDAD*DITE_VALOUNIT)*(1-(DITE_PORCDESC*0.01))) from dbxschema.ditems where PANO_ANO="+año+" and "+mes+"=PMES_MES and TMOV_TIPOMOVI = 91"));
				}
				totvent2=ventas1mes-ventas2mes;
				VentasFmes=String.Format("{0:C}",totvent2);
				ventasmes.Text=VentasFmes.ToString();
				//fin ventas
				//TRASNFERENCIAS
				//AÑO
				//string trans1añoS=DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=80 AND PANO_ANO="+año+" ");
                string trans1añoS=DBFunctions.SingleData("select sum((DITE_CANTIDAD*DITE_VALOUNIT)*(1-(DITE_PORCDESC*0.01))) FROM dbxschema.DITEMS WHERE DITEMS.PANO_ANO="+año+" and "+mes+" >= 01 and DITEMS.PMES_MES < "+mes+" and TMOV_TIPOMOVI = 80");
				if(trans1añoS.Equals(null) || trans1añoS.Equals("") )
				{
					trans1año=0;
				}
				else
				{
					//trans1año=Convert.ToDouble(DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=80 AND PANO_ANO="+año+" "));
                    trans1año = Convert.ToDouble(DBFunctions.SingleData("select sum((DITE_CANTIDAD*DITE_VALOUNIT)*(1-(DITE_PORCDESC*0.01))) FROM dbxschema.DITEMS WHERE DITEMS.PANO_ANO=" +año+ " and " +mes+ " >= 01 and DITEMS.PMES_MES < " +mes+ " and TMOV_TIPOMOVI = 80")); 
				}
				//string trans2añoS=DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=81 AND PANO_ANO="+año+" ");
                string trans2añoS=DBFunctions.SingleData("SELECT SUM((DITE_CANTIDAD*DITE_VALOUNIT)*(1-(DITE_PORCDESC*0.01))) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+" >= 01 and PMES_MES < "+mes+" and TMOV_TIPOMOVI = 81"); 
                if (trans2añoS.Equals(null) || trans2añoS.Equals(""))
				{
					trans2año=0;
				}
				else
				{
					//trans2año=Convert.ToDouble(DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=81 AND PANO_ANO="+año+" "));
                    trans2año = Convert.ToDouble(DBFunctions.SingleData("SELECT SUM((DITE_CANTIDAD*DITE_VALOUNIT)*(1-(DITE_PORCDESC*0.01))) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+" >= 01 and PMES_MES < "+mes+" and TMOV_TIPOMOVI = 81"));
				}
		
				double tottrans1=trans1año-trans2año;
				string transFaño=String.Format("{0:C}",tottrans1);
				transano.Text=transFaño.ToString();
				//MES
				//string trans1mesS=DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=80 AND PANO_ANO="+año+" AND PMES_MES="+mes+" ");
                string trans1mesS=DBFunctions.SingleData("SELECT SUM ((DITE_CANTIDAD*DITE_VALOUNIT)*(1-(DITE_PORCDESC*0.01))) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+"=PMES_MES  and TMOV_TIPOMOVI = 80");
				if(trans1mesS.Equals(null) || trans1mesS.Equals(""))
				{
					trans1mes=0;
				}
				else
				{
					//trans1mes=Convert.ToDouble(DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=80 AND PANO_ANO="+año+" AND PMES_MES="+mes+" "));
                    trans1mes = Convert.ToDouble(DBFunctions.SingleData("SELECT SUM ((DITE_CANTIDAD*DITE_VALOUNIT)*(1-(DITE_PORCDESC*0.01))) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+"=PMES_MES  and TMOV_TIPOMOVI = 80"));
				}
				//string trans2mesS=DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=81 AND PANO_ANO="+año+" AND PMES_MES="+mes+" ");
                string trans2mesS=DBFunctions.SingleData("SELECT SUM((DITE_CANTIDAD*DITE_VALOUNIT)*(1-(DITE_PORCDESC*0.01))) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+"=DITEMS.PMES_MES and TMOV_TIPOMOVI = 81");
				if(trans2mesS.Equals(null)|| trans2mesS.Equals(""))
				{
					trans2mes=0;
				}
				else
				{
					//trans2mes=Convert.ToDouble(DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=81 AND PANO_ANO="+año+" AND PMES_MES="+mes+" "));
                    trans2mes = Convert.ToDouble(DBFunctions.SingleData("SELECT SUM((DITE_CANTIDAD*DITE_VALOUNIT)*(1-(DITE_PORCDESC*0.01))) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+"=DITEMS.PMES_MES and TMOV_TIPOMOVI = 81"));
				}
				double tottrans2=trans1mes-trans2mes;
				string transFmes=String.Format("{0:C}",tottrans2);
				transmes.Text=transFmes.ToString();
				//fin trasnsferencias
				//COMPRAS
				//AÑO
				//string comp1añoS=DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=20 AND PANO_ANO="+año+" ");
                string comp1añoS=DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+" >= 01 and PMES_MES < "+mes+" and TMOV_TIPOMOVI IN (11,30) ");
				if(comp1añoS.Equals(null) || comp1añoS.Equals("") )
				{
					comp1año=0;
				}
				else
				{
					//comp1año=Convert.ToDouble(DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=20 AND PANO_ANO="+año+" "));
                    comp1año = Convert.ToDouble(DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+" >= 01 and PMES_MES < "+mes+" and TMOV_TIPOMOVI IN (11,30)"));
				}
				//string comp2añoS=DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=21 AND PANO_ANO="+año+" ");
                string comp2añoS=DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+" >= 01 and PMES_MES < "+mes+" and TMOV_TIPOMOVI = 31");
				if(comp2añoS.Equals(null) || comp2añoS.Equals("") )
				{
					comp2año=0;
				}
				else
				{
					//comp2año=Convert.ToDouble(DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=21 AND PANO_ANO="+año+" "));
                    comp2año = Convert.ToDouble(DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO=" +año+" and "+mes+" >= 01 and PMES_MES < "+mes+" and TMOV_TIPOMOVI = 31"));
				}
				double totcomp1=comp1año-comp2año;
				string compFaño=String.Format("{0:C}",totcomp1);
				compano.Text=compFaño.ToString();
				//MES
				//string comp1mesS=DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=20 AND PANO_ANO="+año+" AND PMES_MES="+mes+" ");
                string comp1mesS=DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+"=PMES_MES and TMOV_TIPOMOVI IN (11,30)");
				if(comp1mesS.Equals(null) || comp1mesS.Equals(""))
				{
					comp1mes=0;	
				}
				else
				{
					//comp1mes=Convert.ToDouble(DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=20 AND PANO_ANO="+año+" AND PMES_MES="+mes+" "));
                    comp1mes = Convert.ToDouble(DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+"=PMES_MES and TMOV_TIPOMOVI IN (11,30)"));
				}
				//string comp2mesS=DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=21 AND PANO_ANO="+año+" AND PMES_MES="+mes+" ");
                string comp2mesS=DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+"=DITEMS.PMES_MES and TMOV_TIPOMOVI = 31");
				if(comp2mesS.Equals(null) || comp2mesS.Equals(""))
				{
					comp2mes=0;
				}
				else
				{
					//comp2mes=Convert.ToDouble(DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=21 AND PANO_ANO="+año+" AND PMES_MES="+mes+" "));
                    comp2mes = Convert.ToDouble(DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO=" +año+" and "+mes+"=DITEMS.PMES_MES and TMOV_TIPOMOVI = 31"));
				}
				double totcomp2=comp1mes-comp2mes;
				string compFmes=String.Format("{0:C}",totcomp2);
				compmes.Text=compFmes.ToString();
				// FIN COMPRAS
				// AJUSTES
				//AÑO
				//string ajus1añoS=DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=50 AND PANO_ANO="+año+" ");
                string ajus1añoS=DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+" >= 01 and PMES_MES< "+mes+" and TMOV_TIPOMOVI = 50 ");
				if(ajus1añoS.Equals(null) || ajus1añoS.Equals(""))
				{
					ajus1año=0;
				}
				else
				{
					//ajus1año=Convert.ToDouble(DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=50 AND PANO_ANO="+año+" "));
                    ajus1año = Convert.ToDouble(DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+" >= 01 and PMES_MES< " +mes+" and TMOV_TIPOMOVI = 50"));
				}
                string ajus2añoS=DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO=" + año + " and " + mes + " >= 01 and PMES_MES< " + mes + " and TMOV_TIPOMOVI = 51 ");
                if (ajus2añoS.Equals(null) || ajus2añoS.Equals(""))
                {
                    ajus2año = 0;
                }
                else
                {
                    
                    ajus2año = Convert.ToDouble(DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO=" + año + " and " + mes + " >= 01 and PMES_MES< " + mes + " and TMOV_TIPOMOVI = 51"));
                }
                double totajus=ajus1año-ajus2año;
				string ajusFaño=String.Format("{0:C}",totajus);
				ajusano.Text=ajusFaño.ToString();
				//MES
				//string ajus1mesS=DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=50 AND PANO_ANO="+año+" AND PMES_MES="+mes+" ");
                string ajus1mesS=DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+"=PMES_MES and TMOV_TIPOMOVI = 50 ");
				if(ajus1mesS.Equals(null) || ajus1mesS.Equals("") )
				{
					ajus1mes=0;
				}
				else
				{
					//ajus1mes=Convert.ToDouble(DBFunctions.SingleData("select COALESCE(SUM(MACU_PRECIO),0) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=50 AND PANO_ANO="+año+" AND PMES_MES="+mes+" "));
                    ajus1mes = Convert.ToDouble(DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+ " and "+mes+"=PMES_MES and TMOV_TIPOMOVI = 50 "));
				}
                string ajus2mesS=DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+"=PMES_MES and TMOV_TIPOMOVI = 51 ");
                if (ajus2mesS.Equals(null) || ajus2mesS.Equals(""))
                {
                    ajus2mes = 0;
                }
                else
                {
                    ajus2mes = Convert.ToDouble(DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+"=PMES_MES and TMOV_TIPOMOVI = 51"));
                }
                double totaajusmes = ajus1mes - ajus2mes;
				string ajusFmes=String.Format("{0:C}",totaajusmes);
				ajusmes.Text=ajusFmes.ToString();
				// FIN AJUSTES
				//COSTOS VENTAS MOSTRADOR
				//AÑO
				//string cosven1añoS=DBFunctions.SingleData("select SUM(MACU_COSTO) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=90 AND PANO_ANO="+año+" ");
                string cosven1añoS=DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+" >= 01 and PMES_MES< "+mes+" and TMOV_TIPOMOVI = 90 ");
				if(cosven1añoS.Equals(null) || cosven1añoS.Equals(""))
				{
					cosven1año=0;
				}
				else
				{
					//cosven1año=Convert.ToDouble(DBFunctions.SingleData("select SUM(MACU_COSTO) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=90 AND PANO_ANO="+año+" "));
                    cosven1año = Convert.ToDouble(DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+" >= 01 and PMES_MES< "+mes+" and TMOV_TIPOMOVI = 90"));
				}
				//string cosven2añoS=DBFunctions.SingleData("select SUM(MACU_COSTO) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=91 AND PANO_ANO="+año+" ");
                string cosven2añoS=DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO=" +año+" and " +mes+" >= 01 and PMES_MES< "+mes+" and TMOV_TIPOMOVI = 91 ");
				if(cosven2añoS.Equals(null) || cosven2añoS.Equals(""))
				{
					cosven2año=0;
				}
				else
				{
					//cosven2año=Convert.ToDouble(DBFunctions.SingleData("select SUM(MACU_COSTO) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=91 AND PANO_ANO="+año+" "));
                    cosven2año = Convert.ToDouble(DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+" >= 01 and PMES_MES< " +mes+" and TMOV_TIPOMOVI = 91"));

				}
		
				double totcosven1=cosven1año-cosven2año;
				string cosvenFaño=String.Format("{0:C}",totcosven1);
				cosvenano.Text=cosvenFaño.ToString();
				//MES
				//string cosven1mesS=DBFunctions.SingleData("select SUM(MACU_COSTO) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=90 AND PANO_ANO="+año+" AND PMES_MES="+mes+" ");
                string cosven1mesS=DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+"=PMES_MES and TMOV_TIPOMOVI = 90 ");
				if(cosven1mesS.Equals(null) || cosven1mesS.Equals(""))
				{
					cosven1mes=0;
				}
				else
				{
					//cosven1mes=Convert.ToDouble(DBFunctions.SingleData("select SUM(MACU_COSTO) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=90 AND PANO_ANO="+año+" AND PMES_MES="+mes+" "));
                    cosven1mes = Convert.ToDouble(DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+"=PMES_MES and TMOV_TIPOMOVI =90"));
				}
				//string cosven2mesS=DBFunctions.SingleData("select SUM(MACU_COSTO) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=91 AND PANO_ANO="+año+" AND PMES_MES="+mes+" ");
                string cosven2mesS=DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO=" +año+"and "+mes+"=PMES_MES and TMOV_TIPOMOVI = 91");
				if(cosven2mesS.Equals(null) || cosven2mesS.Equals(""))
				{
					cosven2mes=0;
				}
				else
				{
					//cosven2mes=Convert.ToDouble(DBFunctions.SingleData("select SUM(MACU_COSTO) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=91 AND PANO_ANO="+año+" AND PMES_MES="+mes+" "));
                    cosven2mes = Convert.ToDouble(DBFunctions.SingleData("SELECT SUM(DITE_CANTIDAD*DITE_COSTPROM) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO="+año+" and "+mes+"=PMES_MES and TMOV_TIPOMOVI = 91"));
				}
				double totcosven2=cosven1mes-cosven2mes;
				string cosvenFmes=String.Format("{0:C}",totcosven2);
				cosvenmes.Text=cosvenFmes.ToString();
				// FIN COSTOS VENTAS
				
                //COSTO CONSUMOS INTERNOS
				//AÑO
                conin1año = Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(SUM(DITE_CANTIDAD*DITE_COSTPROM),0) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO=" + año + " and PMES_MES >= 01 and  PMES_MES < " + mes + " and  TMOV_TIPOMOVI = 60"));

                conin1mes = Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(SUM(DITE_CANTIDAD*DITE_COSTPROM),0) FROM DBXSCHEMA.DITEMS WHERE PANO_ANO=" + año + " and PMES_MES = " + mes + " and TMOV_TIPOMOVI = 60"));
				 
				string coninFmes=String.Format("{0:C}",conin1mes);
				coninmes.Text=coninFmes.ToString();
				//FIN CONSUMOS INTERNOS

				//COSTOS TRANSFERENCIA A TALLER
				//AÑO
				//string tranta1añoS=DBFunctions.SingleData("select SUM(MACU_COSTO) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=80 AND PANO_ANO="+año+" ");
                trans1año = Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(SUM(DITE_CANTIDAD*DITE_COSTPROM),0) FROM  DBXSCHEMA.DITEMS WHERE PANO_ANO=" + año + " and PMES_MES>= 01 and PMES_MES<" + mes + " and TMOV_TIPOMOVI = 80"));

                tranta2año = Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(SUM(DITE_CANTIDAD*DITE_COSTPROM),0) FROM  DBXSCHEMA.DITEMS WHERE PANO_ANO=" + año + " and PMES_MES>= 01 and PMES_MES<" + mes + " and TMOV_TIPOMOVI = 81"));

                double tottranta1 = trans1año - tranta2año;
				string trantaFaño=String.Format("{0:C}",tottranta1);
				trantano.Text=trantaFaño.ToString();
				//MES
				//string tranta1mesS=DBFunctions.SingleData("select SUM(MACU_COSTO) FROM DBXSCHEMA.MACUMULADOITEM WHERE TMOV_TIPOMOVI=80 AND PANO_ANO="+año+" AND PMES_MES="+mes+" ");
                tranta1mes = Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(SUM(DITE_CANTIDAD*DITE_COSTPROM),0) FROM  DBXSCHEMA.DITEMS WHERE PANO_ANO=" + año + " and " + mes + "=PMES_MES and TMOV_TIPOMOVI = 80"));
				
				tranta2mes = Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(SUM(DITE_CANTIDAD*DITE_COSTPROM),0) FROM  DBXSCHEMA.DITEMS WHERE PANO_ANO=" + año + " and " + mes + "=PMES_MES and TMOV_TIPOMOVI = 81"));
				 
				double tottranta2=tranta1mes-tranta2mes;
				string trantaFmes=String.Format("{0:C}",tottranta2);
				trantmes.Text=trantaFmes.ToString();
				// FIN COSTOS TRANSFERENCIA A TALLER
				//INVENTARIO FINAL
				//AÑO
		
				double InvFinalaño=inventarioinicial+totcomp1+ajus1año-totcosven1-tottranta1-conin1año;
				string invfinañoF=String.Format("{0:C}",InvFinalaño);
			//	invfinano.Text=invfinañoF.ToString();
				invfinano.Text=Convert.ToDouble(DBFunctions.SingleData("select SUM(Msal_cantactual*msal_costprom) FROM DBXSCHEMA.Msaldoitem WHERE pano_ano="+año+"")).ToString("C");
				//MES
				double InvFinalmes=inventarioinicial+totcomp2+ajus1mes-totcosven2-tottranta2-conin1mes;
				string invfinmes=String.Format("{0:C}",InvFinalmes);
				invfinalmes.Text=invfinmes.ToString();
				//FINAL INVENTARIO FINAL
				//UTILIDAD BRUTA
				//AÑO
				double utilidadbrutaño=1;
				utilidadbrutaño=(totvent1+tottrans1)-(totcosven1+tottranta1+conin1año);
				string utilbruaño=String.Format("{0:C}",utilidadbrutaño);
				utilano.Text=utilbruaño.ToString();
				//MES
				double utilidadbrutmes=1;
				utilidadbrutmes=(totvent2+tottrans2)-(totcosven2+tottranta2+conin1mes);
				string utilbrumes=String.Format("{0:C}",utilidadbrutmes);
				utilmes.Text=utilbrumes.ToString();
				//FIN UTILIDAD BRUTA
				//MARGEN UTILIDAD FINANCIERA
				//AÑO
				margenfin=(((totvent1+tottrans1)-(totcosven1+tottranta1))/(totvent1+tottranta1))*100;
				margenfin=Math.Round(margenfin,2);
				utilfinano.Text=margenfin.ToString();


				//MES
				margenfinmes=(((totvent2+tottrans2)-(totcosven2+tottranta2))/(totvent2+tottranta2))*100;
				margenfinmes=Math.Round(margenfinmes,2);
				utilfinmes.Text=margenfinmes.ToString();
				//FIN UTILIDAD FINANCIERA

				//MARGEN UTILIDAD REAL
				//AÑO
				margenreal=(((totvent1+tottrans1)-(totcosven1+tottranta1))/(cosven1año+tranta1año))*100;
				margenreal=Math.Round(margenreal,2);
				utilrealano.Text=margenreal.ToString();
				//MES
				margenrenmes = (((totvent2+tottrans2)-(totcosven2+tottranta2))/(cosven2año+tranta2año))*100;;
				margenrenmes = Math.Round(margenrenmes,2);
				utilrealmes.Text=margenrenmes.ToString();
				//INVENTARIO COSTO PROMEDIO
				//AÑO
				
				invsaldopromaño = Convert.ToDouble(DBFunctions.SingleData("select coalesce(sum(MSAL_CANTACTUAL * MSAL_COSTPROM),0) from DBXSCHEMA.MSALDOITEM WHERE PANO_ANO="+año+""));
				
				string invsaldoFaño = String.Format("{0:C}",invsaldopromaño);
				invcostp.Text=invsaldoFaño.ToString();
				
                invt2años = Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(sum(MSAL_CANTACTUAL * MSAL_COSTPROM),0) FROM DBXSCHEMA.MSALDOITEM WHERE PANO_ANO=" + año + " and (MSAL_ULTIVENT) < (select current date - 2 years from sysibm.sysdummy1)"));
				 
				string invt2añosF = String.Format("{0:C}",invt2años);
				inv2anos.Text=invt2añosF.ToString();
				
                invt1año = Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(sum(MSAL_CANTACTUAL * MSAL_COSTPROM),0) FROM DBXSCHEMA.MSALDOITEM WHERE PANO_ANO=" + año + " and (MSAL_ULTIVENT) >= (select current date - 2 years from sysibm.sysdummy1) and (MSAL_ULTIVENT)< (select current date - 1 years from sysibm.sysdummy1)"));
				 
				string invt1añosF = String.Format("{0:C}",invt1año);
                inv1ano.Text = invt1añosF.ToString();
				
                invt6meses = Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(sum(MSAL_CANTACTUAL * MSAL_COSTPROM),0) FROM DBXSCHEMA.MSALDOITEM WHERE PANO_ANO=" + año + " and (MSAL_ULTIVENT) >= (select current date - 1 years from sysibm.sysdummy1) and (MSAL_ULTIVENT) < (select current date - 6 months from sysibm.sysdummy1)"));
				 
				string invt6mesesF = String.Format("{0:C}",invt6meses);
				inv6meses.Text=invt6mesesF.ToString();
				//INVENTARIO ALTA ROTACION
				//
				invaltro = invsaldopromaño-(invt6meses+invt1año+invt2años);
				string invaltroS = String.Format("{0:C}",invaltro);
				invalro.Visible = true;
				invalro.Text = invaltroS.ToString();
			}
		}

		#region Código generado por el Diseñador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{

		}
		#endregion
	
		
	}
}
