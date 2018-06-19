
using AMS.DB;
using System.Data;
using System.Data.Odbc;

namespace AMS.Reportes
{
    public class DatasForReport
    {
        protected int limitedSelection = 5;
        protected string infoMsg;
        protected string reportName;
        protected string reportSection;
        protected string pressFieldsStr;
        protected string sqlsStr;
        protected string operationsStr;
        protected string[] pressFields;
        protected string[] sqls;
        protected string[] operations;
        protected string[] paramsForReport;
        protected OdbcDataReader dr;
        protected OdbcDataReader dr2;
        protected DataRow dRow;
        protected DataSet ds;
        protected int i;
        protected int j;
        protected int k;

        public string[] PressFields
        {
            get
            {
                return this.pressFields;
            }
        }

        public string[] Operations
        {
            get
            {
                return this.operations;
            }
        }

        public string[] SQL
        {
            get
            {
                return this.sqls;
            }
        }

        public int LimitedSelection
        {
            set
            {
                this.limitedSelection = value;
            }
        }

        public string InfoMsg
        {
            get
            {
                return this.infoMsg;
            }
        }

        public DatasForReport(string name)
        {
            this.reportName = name;
            this.GetSqlDatas();
        }

        public void GetSqlDatas()
        {
            this.reportSection = DBFunctions.SingleData("SELECT srep_seccion FROM sreportes WHERE srep_nombre='" + this.reportName + "'");
            if (this.reportSection != "")
            {
                this.pressFieldsStr = DBFunctions.SingleData("SELECT srep_campos FROM sreportes WHERE srep_nombre='" + this.reportName + "'");
                this.sqlsStr = DBFunctions.SingleData("SELECT srep_sql FROM sreportes WHERE srep_nombre='" + this.reportName + "'");
                this.operationsStr = DBFunctions.SingleData("SELECT srep_operaciones FROM sreportes WHERE srep_nombre='" + this.reportName + "'");
            }
            else
                this.infoMsg = "No existe el reporte de nombre: " + this.reportName;
            char[] chArray = ":".ToCharArray();
            this.pressFields = this.pressFieldsStr.Split(chArray);
            this.sqls = this.sqlsStr.Split(chArray);
            this.operations = this.operationsStr.Split(chArray);
        }

        public DataSet GetDatas(bool limited, params string[] parameters)
        {
            this.paramsForReport = new string[parameters.Length];
            this.i = 0;
            this.k = 0;
            for (; this.i < this.sqls.Length; ++this.i)
            {
                if (this.sqls[this.i].IndexOf("@") != -1)
                {
                    string[] strArray = this.sqls[this.i].Split("@".ToCharArray());
                    string str = "";
                    if (parameters.Length == strArray.Length - 1)
                    {
                        for (this.j = 0; this.j < strArray.Length - 1; ++this.j)
                        {
                            str = string.Join(parameters[this.j], strArray, this.j, 2);
                            strArray[this.j + 1] = str;
                        }
                    }
                    else
                        this.infoMsg += "Error: el nmero de parmetros no coincide<br>";
                    this.sqls[this.i] = str;
                }
            }
            this.i = 0;
            this.k = 0;
            for (; this.i < this.sqls.Length; ++this.i)
            {
                DatasForReport datasForReport = this;
                string str = datasForReport.infoMsg + this.sqls[this.i] + "<br>";
                datasForReport.infoMsg = str;
            }
            this.ds = new DataSet();
            this.i = 0;
            this.k = 0;
            for (; this.i < this.sqls.Length; ++this.i)
                this.ds = limited ? DBFunctions.Request(this.ds, IncludeSchema.NO, this.sqls[this.i] + " FETCH FIRST " + this.limitedSelection.ToString() + " ROWS ONLY") : DBFunctions.Request(this.ds, IncludeSchema.NO, this.sqls[this.i]);
            this.infoMsg += DBFunctions.exceptions;
            return this.ds;
        }
    }
}
