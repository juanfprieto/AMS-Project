using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using AMS.DB;
using AMS.Inventarios;

namespace AMS.Automotriz
{
	public class Combo
	{
        private string codigo, descripcion, grupoCatalogo, listaPrecios, kilometraje, meses, Vigencia;
		private DataTable items, operaciones;
		private string processMsg="";
		
		public string Codigo{set{codigo = value;}get{return codigo;}}
		public string Descripcion{set{descripcion = value;}get{return descripcion;}}
		public string GrupoCatalogo{set{grupoCatalogo = value;}get{return grupoCatalogo;}}
        public string Kilometraje { set { kilometraje = value; } get { return kilometraje; } }
        public string Meses { set { meses = value; } get { return meses; } }
        public string ListaPrecios { set { listaPrecios = value; } get { return listaPrecios; } }
        public string vigencia { set { Vigencia = value; } get { return Vigencia; } }
		public DataTable Items{set{items = value;}get{return items;}}
		public DataTable Operaciones{set{operaciones = value;}get{return operaciones;}}
		public string ProcessMsg{get{return processMsg;}}
		
		public Combo()
		{
			items       = new DataTable();
			operaciones = new DataTable();
		}
		
		public Combo(string codigoCombo)
		{
			this.codigo         = codigoCombo;
            this.descripcion    = DBFunctions.SingleData("SELECT pkit_nombre    FROM pkit WHERE pkit_codigo='" + this.codigo + "'");
            this.grupoCatalogo  = DBFunctions.SingleData("SELECT pgru_grupo     FROM pkit WHERE pkit_codigo='" + this.codigo + "'");
            this.Kilometraje    = DBFunctions.SingleData("SELECT pkit_kilometr  FROM pkit WHERE pkit_codigo='" + this.codigo + "'");
            this.Meses          = DBFunctions.SingleData("SELECT pkit_meserepe  FROM pkit WHERE pkit_codigo='" + this.codigo + "'");
            this.listaPrecios   = DBFunctions.SingleData("SELECT ppre_codigo    FROM pkit WHERE pkit_codigo='" + this.codigo + "'");
            this.vigencia       = DBFunctions.SingleData("SELECT pkit_vigencia  FROM pkit WHERE pkit_codigo='" + this.codigo + "'");
            
			this.LoadItems();
			this.LoadOperaciones();
            
		}
		
		public Combo(DataTable dtItems, DataTable dtOperaciones)
		{
			items = new DataTable();
			items = dtItems;
			operaciones = new DataTable();
			operaciones = dtOperaciones;
		}
				
		public bool CommitValues()
		{
			bool status = false;
			if (this.Kilometraje != "" && this.Meses != "")
            {
                status = false;
                processMsg += "Error: Solo puede definir máximo una variable entre kilometraje específico y meses de repetición del kit<br><br>";
                return status;
            }

            ArrayList sqlStrings = new ArrayList();
           
            //Revisamos si existe este combo o no
            if (!DBFunctions.RecordExist("SELECT * FROM pkit WHERE pkit_codigo='" + this.codigo + "'"))
			{
				//Primero Grabamos en la tabla de pkit
                sqlStrings.Add("INSERT INTO pkit VALUES('" + this.codigo + "','" + this.descripcion + "','" + this.grupoCatalogo + "','" + this.listaPrecios + "','" + this.Kilometraje + "','" + this.Meses + "','" + this.vigencia + "')");
				//Ahora creamos los registros correspondientes a la tabla de items (mkititem)
                status = insertItemsOperaciones(sqlStrings);
			}
			else
				processMsg += "Error: Código de Combo Repetido<br><br>";
			return status;
		}
		
		public bool UpdateValues()
		{
            bool status = false;
		    if(this.Kilometraje != "" && this.Meses != "")
            {
                status = false;
                processMsg += "Error: Solo puede definir máximo una variable entre kilometraje específico y meses de repetición del kit<br><br>";
                return status;
            }
   
            ArrayList sqlStrings = new ArrayList();
		 
			//Primero eliminamos los registros correpondientes al combo en las tablas de a y mkitoperacion
			sqlStrings.Add("DELETE FROM mkititem WHERE mkit_codikit='"+this.codigo+"'");
			sqlStrings.Add("DELETE FROM mkitoperacion WHERE mkit_codikitoper='"+this.codigo+"'");
			//Ahora actualizamos la tabla de pkit
            sqlStrings.Add("UPDATE pkit SET pkit_nombre='" + this.descripcion + "', pgru_grupo='" + this.GrupoCatalogo + "', ppre_codigo='" + this.listaPrecios + "', pkit_kilometr ='" + this.Kilometraje + "', pkit_meserepe ='" + this.Meses + "', pkit_vigencia ='" + this.vigencia + "'  WHERE pkit_codigo='" + this.codigo + "'");
            //Ahora insertamos los nuevos valores de las tablas mkititem y mkitoperacionDgItems_DataBound
            status = insertItemsOperaciones(sqlStrings);
			return status;
		}

        public bool insertItemsOperaciones(ArrayList sqlStrings)
        {
            bool status = false;
            int i;
            for (i = 0; i < this.items.Rows.Count; i++)
            {
                sqlStrings.Add("INSERT INTO mkititem VALUES('" + this.codigo + "','" + this.items.Rows[i][0] + "')");
                // se inserta en la relación de items con grupos si el item está registrado como item generico y no exite en mitemsgrupo
                if (this.items.Rows[i][3].ToString() == "Si" && !DBFunctions.RecordExist("SELECT MITE_CODIGO FROM MITEMSGRUPO WHERE MITE_CODIGO = '" + this.items.Rows[i][0] + "' AND PGRU_GRUPO = '" + this.grupoCatalogo + "' "))
                    sqlStrings.Add("INSERT INTO MITEMSGRUPO VALUES ('" + this.items.Rows[i][0] + "','" + this.grupoCatalogo + "'," + this.items.Rows[i][2] + ") ;");
            }
			//Ahora creamos los registros correspondientes a la tabla de operaciones (mkitoperacion)
			for(i=0;i<this.operaciones.Rows.Count;i++)
				sqlStrings.Add("INSERT INTO mkitoperacion VALUES('"+this.codigo+"','"+this.operaciones.Rows[i][0]+"')");
			if(DBFunctions.Transaction(sqlStrings))
			{
				status = true;
				processMsg += DBFunctions.exceptions + "<br>";
			}
			else
				processMsg += "Error: " + DBFunctions.exceptions + "<br><br>";
			return status;
        }

		public bool DeleteValues()
		{
			ArrayList sqlStrings = new ArrayList();
			bool status = false;
            sqlStrings.Add("DELETE FROM dordenkit WHERE pkit_codigo='" + this.codigo + "'");  // para eliminar tambien los kits aplicados
            sqlStrings.Add("DELETE FROM mkititem WHERE mkit_codikit='" + this.codigo + "'");
			sqlStrings.Add("DELETE FROM mkitoperacion WHERE mkit_codikitoper='"+this.codigo+"'");
            sqlStrings.Add("DELETE FROM pmantenimientoprogramado WHERE pkit_codigo='" + this.codigo + "'");
            sqlStrings.Add("DELETE FROM pkit WHERE pkit_codigo='" + this.codigo + "'");
            if (DBFunctions.Transaction(sqlStrings))
            {
                status = true;

                processMsg += DBFunctions.exceptions + "<br>";
            }
            else
            {
                DBFunctions.NonQuery("UPDATE PKIT SET PKIT_VIGENCIA = 'B' WHERE PKIT_CODIGO ='" + this.codigo + "'");
                processMsg += "No Se Eliminó El Kit Pero Su Estado Ha Cambiado a No Vigente " + DBFunctions.exceptions + "<br><br>";
            }
			return status;
		}
		
		private void PrepararItems()
		{
			items = new DataTable();
			items.Columns.Add(new DataColumn("ITEM",System.Type.GetType("System.String")));//0
			items.Columns.Add(new DataColumn("DESCRIPCION",System.Type.GetType("System.String")));//1
			items.Columns.Add(new DataColumn("CANTIDAD",System.Type.GetType("System.Double")));//2
            items.Columns.Add(new DataColumn("ITEM GENERICO", System.Type.GetType("System.String")));//3
		}
		
		private void PrepararOperaciones()
		{
			operaciones = new DataTable();
			operaciones.Columns.Add(new DataColumn("CODIGO",System.Type.GetType("System.String")));//0
			operaciones.Columns.Add(new DataColumn("DESCRIPCION",System.Type.GetType("System.String")));//1
			operaciones.Columns.Add(new DataColumn("TIEMPO",System.Type.GetType("System.Double")));//2
		}
		
		private void LoadItems()
		{
			this.PrepararItems();
			DataSet ds = new DataSet();
			//string tem = "SELECT DISTINCT MITE.mite_codigo , MITE.mite_nombre , CASE WHEN MITE.mite_codigo NOT IN (SELECT mite_codigo FROM mitemsgrupo WHERE pgru_grupo = '"+this.grupoCatalogo+"' AND mig_cantidaduso IS NOT NULL) THEN MITE.mite_usoxvehi WHEN MITE.mite_codigo IN (SELECT mite_codigo FROM mitemsgrupo WHERE pgru_grupo = '"+this.grupoCatalogo+"' AND mig_cantidaduso IS NOT NULL) THEN MIG.mig_cantidaduso END AS CANTIDAD FROM mitems MITE, mitemsgrupo MIG, pkit PKIT, mkititem MKI WHERE (((MITE.mite_codigo=MIG.mite_codigo AND MIG.pgru_grupo='"+this.grupoCatalogo+"') OR (MITE.mite_indigeneric = 'S' AND MITE.mite_codigo NOT IN (SELECT mite_codigo FROM mitemsgrupo WHERE pgru_grupo = '"+this.grupoCatalogo+"' AND mig_cantidaduso IS NOT NULL)))) AND PKIT.pkit_codigo = '"+this.codigo+"' AND PKIT.pkit_codigo = MKI.mkit_codikit AND MKI.mkit_coditem = MITE.mite_codigo ORDER BY MITE.mite_codigo";
			//string tem="SELECT MKIT.mkit_coditem,MITE.mite_nombre FROM dbxschema.mkititem MKIT,dbxschema.mitems MITE WHERE MKIT.mkit_coditem=MITE.mite_codigo AND MKIT.mkit_codikit='"+this.codigo+"'";
            string tem = @"SELECT MKIT.mkit_coditem,
                           MI.mite_nombre,
                           coalesce(mig_cantidaduso,mi.mite_usoxvehi) AS cantidad,
                           CASE
                             WHEN MI.mite_indigeneric = 'S' THEN 'Si'
                             ELSE 'No'
                           END AS uso 
                    FROM  dbxschema.mkititem MKIT       
                         LEFT JOIN dbxschema.mitems MI ON MKIT.mkit_coditem = MI.mite_codigo
  	                     LEFT JOIN dbxschema.Pkit PK ON  MKIt.MKIT_CODIKIT = PK.PKIT_CODIGO 
                         LEFT JOIN mitemsgrupo mig ON mig.pgru_grupo = '"+this.grupoCatalogo+"' AND mig.mite_codigo = mi.mite_codigo AND MI.mite_indigeneric = 'S' WHERE MKIT.mkit_codikit = '"+this.codigo+"'	;";
            DBFunctions.Request(ds,IncludeSchema.NO,tem);
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				DataRow fila = items.NewRow();
				fila[0] = ds.Tables[0].Rows[i][0].ToString();
				fila[1] = ds.Tables[0].Rows[i][1].ToString();
				double cantidad=0;
			//	try{cantidad  =Convert.ToDouble(DBFunctions.SingleData("SELECT CASE WHEN MITE.mite_codigo NOT IN (SELECT mite_codigo FROM dbxschema.mitemsgrupo WHERE pgru_grupo = '"+this.grupoCatalogo+"' AND mig_cantidaduso IS NOT NULL) THEN MITE.mite_usoxvehi WHEN MITE.mite_codigo IN (SELECT mite_codigo FROM dbxschema.mitemsgrupo WHERE pgru_grupo = '"+this.grupoCatalogo+"' AND mig_cantidaduso IS NOT NULL) THEN MIG.mig_cantidaduso END AS CANTIDAD FROM dbxschema.mitems MITE, dbxschema.mitemsgrupo MIG WHERE ((MITE.mite_codigo=MIG.mite_codigo AND MIG.pgru_grupo='"+this.grupoCatalogo+"') OR (MITE.mite_indigeneric = 'S' AND MITE.mite_codigo NOT IN (SELECT mite_codigo FROM dbxschema.mitemsgrupo WHERE pgru_grupo = '"+this.grupoCatalogo+"' AND mig_cantidaduso IS NOT NULL))) AND MITE.mite_codigo='"+ds.Tables[0].Rows[i][0].ToString()+"'"));}
			//	catch{cantidad=Convert.ToDouble(DBFunctions.SingleData("SELECT mite_usoxvehi FROM mitems WHERE mite_codigo='"+ds.Tables[0].Rows[i][0].ToString()+"'"));}
				try{cantidad=Convert.ToDouble(Convert.ToDouble(ds.Tables[0].Rows[i][2]));}
                catch {cantidad = 1.00;}
                fila[2] = cantidad;
                fila[3] = ds.Tables[0].Rows[i][3].ToString();
				this.items.Rows.Add(fila);
			}
		}
		
		private void LoadOperaciones()
		{
			this.PrepararOperaciones();
			DataSet ds = new DataSet();
            string sqlOpers = @"SELECT PTEM.ptem_operacion, PTEM.ptem_descripcion, COALESCE(PTIE.ptie_tiemclie, PTEM_TIEMPOESTANDAR)  
                 FROM  mkitoperacion MKO, ptempario PTEM  
                 LEFT  JOIN ptiempotaller PTIE ON PTIE.ptie_grupcata = '" + grupoCatalogo + @"' AND PTEM.ptem_operacion = PTIE.ptie_tempario  
                 WHERE MKO.mkit_codikitoper = '" + this.codigo + @"' AND PTEM.ptem_operacion = MKO.mkit_operacion ";
            DBFunctions.Request(ds, IncludeSchema.NO, sqlOpers);
            for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				DataRow fila = operaciones.NewRow();
				fila[0] = ds.Tables[0].Rows[i][0].ToString();
				fila[1] = ds.Tables[0].Rows[i][1].ToString();
				if(ds.Tables[0].Rows[i][2].ToString()!="")
					fila[2] = Convert.ToDouble(ds.Tables[0].Rows[i][2]);
				this.operaciones.Rows.Add(fila);
			}
		}
	}
}
