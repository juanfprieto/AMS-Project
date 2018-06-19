using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.Odbc;
using AMS.DB;
using AMS.Tools;


namespace AMS.Nomina
{
	
	
	public partial class Organigrama: System.Web.UI.UserControl
	{
		protected DataSet dscargos = new DataSet();
		
		protected void Page_Load(object sender , EventArgs e)
		{
			this.armarestructura();
			
			
			
			
		}
		
		protected void armarestructura()
		{
			DBFunctions.Request(dscargos,IncludeSchema.NO,"select distinct A.pcar_codicargo,A.pcar_nombcarg,A.pcar_padre,C.mnit_nombres,C.mnit_apellidos from dbxschema.pcargoempleado A,dbxschema.mempleado B,dbxschema.mnit C ,dbxschema.pcargoempleado D WHERE  A.pcar_codicargo=B.pcar_codicargo and C.mnit_nit=B.mnit_nit and D.pcar_padre=A.pcar_codicargo and A.pcar_padre is null");
			//Padre general
			//lb.Text+="select distinct A.pcar_codicargo,A.pcar_nombcarg,A.pcar_padre,C.mnit_nombres,C.mnit_apellidos from dbxschema.pcargoempleado A,dbxschema.mempleado B,dbxschema.mnit C ,dbxschema.pcargoempleado D WHERE  A.pcar_codicargo=B.pcar_codicargo and C.mnit_nit=B.mnit_nit and D.pcar_padre=A.pcar_codicargo and A.pcar_padre is null <br> ";
			if (dscargos.Tables[0].Rows.Count!=0)
			{
				lbOrganigrama.Text+="<table style=\"BACKGROUND-COLOR: white\" align=\"center\" ><tbody><tr>";
				lbOrganigrama.Text+="<td>";
				lbOrganigrama.Text+=" <center>"+dscargos.Tables[0].Rows[0][0].ToString()+"-"+dscargos.Tables[0].Rows[0][1].ToString()+"<br>"+dscargos.Tables[0].Rows[0][3].ToString()+"</center> ";
				lbOrganigrama.Text+="</td></tr> </tbody></table><br>";
				armarhijosniveles(dscargos);	
			}
			else
			{
                Utils.MostrarAlerta(Response, "Porfavor Configure correctamente el Organigrama de su Empresa, no ahy ningun empleado asignado al Cargo Principal");
			}
		}
		
		protected void armarhijosniveles(DataSet dshijos)
		{
			int i,j;
			
			
			//lb.Text+="entre armarhijosniveles hacer select de padres <br>";
			string select="select pcar_codicargo,pcar_nombcarg,pcar_padre from dbxschema.pcargoempleado ";
			select+="where pcar_padre='xx' ";
			
			if (dshijos.Tables[0].Rows.Count>0)
			{
				
			
			for (i=0;i<dshijos.Tables[0].Rows.Count;i++)
			{
				select+="or pcar_padre='"+dshijos.Tables[0].Rows[i][0].ToString().Trim()+"' ";
			}
			//lb.Text=dshijos.Tables[0].Rows.Count.ToString();
			//lb.Text+=""+select+"";
			DataSet dssubhijos= new DataSet();
			DBFunctions.Request(dssubhijos,IncludeSchema.NO," "+select+" ");
		//lb.Text+=""+select+"<br> ";
			lbOrganigrama.Text+="<table style=\"BACKGROUND-COLOR: white\" align=\"center\" ><tbody><tr>";
				for (j=0;j<dssubhijos.Tables[0].Rows.Count;j++)
				{
					lbOrganigrama.Text+="<td>";
					lbOrganigrama.Text+="<table><tbody><tr><td>";
					string nombreCargo=DBFunctions.SingleData("select pcar_nombcarg from dbxschema.pcargoempleado where pcar_codicargo='"+dssubhijos.Tables[0].Rows[j][2].ToString()+"'");
					lbOrganigrama.Text+=" <center>"+dssubhijos.Tables[0].Rows[j][0].ToString()+"-"+dssubhijos.Tables[0].Rows[j][1].ToString()+" <br> SubOrdinado Cod. Cargo: <br> "+dssubhijos.Tables[0].Rows[j][2].ToString()+"-"+nombreCargo+" </center>";
					
					//consulta de nombres
					//DataSet dsnombres= new DataSet();
					//DBFunctions.Request(dsnombres,IncludeSchema.NO," select distinct C.mnit_nombres,C.mnit_apellidos from dbxschema.pcargoempleado A,dbxschema.mempleado B,dbxschema.mnit C ,dbxschema.pcargoempleado D WHERE  A.pcar_codicargo=B.pcar_codicargo and C.mnit_nit=B.mnit_nit and D.pcar_padre=A.pcar_codicargo and  A.pcar_codicargo='"+dssubhijos.Tables[0].Rows[j][0].ToString().Trim()+"'  ");
					//lb.Text+="entre a buscar nombres de un padre <br>";
					//lb.Text+="select  C.mnit_nombres,C.mnit_apellidos from dbxschema.pcargoempleado A,dbxschema.mempleado B,dbxschema.mnit C ,dbxschema.pcargoempleado D WHERE  A.pcar_codicargo=B.pcar_codicargo and C.mnit_nit=B.mnit_nit and D.pcar_padre=A.pcar_codicargo and  A.pcar_codicargo='"+dssubhijos.Tables[0].Rows[j][0].ToString()+"'  <br> ";
					//if (dsnombres.Tables[0].Rows.Count>0)
					//{
					//	for (k=0;k<dsnombres.Tables[0].Rows.Count;k++)
					//	{
					//		lbOrganigrama.Text+="<center>"+dsnombres.Tables[0].Rows[k][0].ToString()+" "+dsnombres.Tables[0].Rows[k][1].ToString()+"</center> ";
					//	}
						
					//}
					
					lbOrganigrama.Text+="</td></tr></tbody></table>";
					//lbOrganigrama.Text+="</br>";
				}
		lbOrganigrama.Text+="</tr></tbody></table><br>";
				
				armarhijosniveles(dssubhijos);
			}
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
		}
		#endregion

		
		
		
	}
	
	
	
}

