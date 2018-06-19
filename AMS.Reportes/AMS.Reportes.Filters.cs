
using AMS.DB;
using AMS.Forms;
using System;
using System.Data.Odbc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMS.Reportes
{
    public class Filters
    {
        protected int controlsNum = 0;
        protected int filtersNum;
        protected string filterType;
        protected string infoProcess;
        protected string[] filtersStr;
        protected PlaceHolder filterPlace;
        protected OdbcDataReader dr;

        public string InfoProcess
        {
            get
            {
                return this.infoProcess;
            }
        }

        public int FiltersNum
        {
            get
            {
                return this.filtersNum;
            }
        }

        public Filters(string reportName, PlaceHolder ph)
        {
            this.filterPlace = ph;
            string str = DBFunctions.SingleData("select srep_filtro from sreportes where srep_nombre='" + reportName + "'");
            if (str != "")
                this.SplitStringFilter(str);
            else
                this.infoProcess = "No se encuentra registrado reporte de nombre: " + reportName;
        }

        protected void SplitStringFilter(string str)
        {
            char[] chArray = ":".ToCharArray();
            string[] strArray = str.Split(chArray);
            this.filtersNum = strArray.Length;
            for (int index = 0; index < this.filtersNum; ++index)
                this.Interpreter(strArray[index]);
        }

        protected void Interpreter(string str)
        {
            if (str.IndexOf("SELECT") == -1)
                return;
            this.BuildFilterSelectionType(str);
        }

        protected void BuildFilterSelectionType(string str)
        {
            string str1 = "";
            string str2 = "";
            string[] strArray = str.Split(' ');
            for (int index = 0; index < strArray.Length; ++index)
            {
                if (strArray[index] == "FROM")
                {
                    str1 = strArray[index + 1];
                    break;
                }
            }
            DropDownList ddl = new DropDownList();
            Label label = new Label();
            try
            {
                str2 = DBFunctions.SingleData("SELECT remarks FROM sysibm.systables WHERE name='" + str1.ToUpper() + "'");
            }
            catch (Exception ex)
            {
                this.infoProcess = DBFunctions.exceptions + ":::" + ex.ToString();
            }
            label.Text = !(str2 != "") ? " [etiqueta]" : " " + str2 + ":";
            new DatasToControls().PutDatasIntoDropDownList(ddl, str);
            ++this.controlsNum;
            this.filterPlace.Controls.Add((Control)label);
            this.filterPlace.Controls.Add((Control)ddl);
            if (this.controlsNum != 4)
                return;
            this.filterPlace.Controls.Add((Control)new LiteralControl("<br>"));
        }

        protected void BuildFilterDateType(string[] strArray)
        {
        }
    }
}
