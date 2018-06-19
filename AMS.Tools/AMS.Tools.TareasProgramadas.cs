using System;
using System.Web;
using AMS.DB;
using bd.WebScheduledTasks;
using System.Data;

namespace AMS.Tools
{
    public class TareasProgramadas : IScheduleable
    {

        #region IScheduleable Members
        
        public bool IsRunning
        {
            get { return false; }
        }

        public bool IsTimeToRun
        {
            get { return true; }
        }

        //Iniciar Administrador de Tareas Programadas del Servidor.
        public void Run(HttpApplicationState application, System.Web.Caching.Cache cache)
        {
            //Obtener todas las tareas registradas en la tabla Global GTAREAS.
            DataSet dsTareasGlobal = new DataSet();

            DBFunctions.RequestGlobal(dsTareasGlobal, IncludeSchema.NO, 
                "global", "select gtar_descriptor, gtar_database, gtar_params, gtar_hora, gtar_activa from gtareas where gtar_activa = 'S';");
            
            //Recorrer, validar y ejecutar todas las tareas definidas.
            for (int t=0; t < dsTareasGlobal.Tables[0].Rows.Count; t++)
            {
                string horaTarea = dsTareasGlobal.Tables[0].Rows[t]["gtar_hora"].ToString();

                bool ejecutarTarea = ValidarTarea(horaTarea);

                if (ejecutarTarea)
                {
                    string idTarea = dsTareasGlobal.Tables[0].Rows[t]["gtar_descriptor"].ToString();
                    string dataBaseCliente = dsTareasGlobal.Tables[0].Rows[t]["gtar_database"].ToString();
                    string parametrosFuncionTarea = dsTareasGlobal.Tables[0].Rows[t]["gtar_params"].ToString();

                    //Identificar tarea y ejecutarla. 
                    //De acuerdo a la base de datos Cliente definida y los parametros asociados a cada función (Descritos en la tabla GTIPOTAREAS).
                    //Nota: Nuevas tareas deberan adicionarse aquí, y estar definidas en la clase DescriptorTarea.cs y en la Base de Datos Global en GTAREAS y GTIPOTAREAS.
                    switch (idTarea)
                    {
                        case "OT_15M":
                            DescriptorTarea.FTPEstadoActualOT(dataBaseCliente, parametrosFuncionTarea);
                            break;
                        case "OT_24H":
                            DescriptorTarea.FTPFacturaDiariaOT(dataBaseCliente, parametrosFuncionTarea);
                            break;
                        case "CT_18H":
                            DescriptorTarea.EmailClienteCitasTaller(dataBaseCliente, parametrosFuncionTarea);
                            break;
                        case "SA_08D":
                            DescriptorTarea.CierreAutomaticoOrdenesSAC(dataBaseCliente, parametrosFuncionTarea);
                            break;
                    }
                }
            }
        }

        //Valida si la tarea en cuestion esta activa. En caso de estar activa; identifica si es una tarea frecuente en el día, o si es una tarea que solo
        //debe ejecutarse una vez en el día a una determinada hora.
        private bool ValidarTarea(string horaTarea)
        {
            bool ejecutar = false;
            
            //Si la horaTarea no esta definida, se ejecuta la tarea ya que es una tarea de ejecución frecuente en el día.
            //El tiempo que se demora en ejecutar dependerá del paramentro (en segundos) definido en  web.config: <scheduledTasks><scheduler interval = "15"> 
            if (horaTarea == "")
            {
                ejecutar = true;
            }
            else
            {
                //Calculo de tiempo para validar la ejecución de tareas a una determinada hora.
                DateTime t1Actual = DateTime.Now;
                DateTime t2Tarea = Convert.ToDateTime(horaTarea);
                TimeSpan ts = t1Actual - t2Tarea;
                int difMin = ts.Minutes;
                int difHor = ts.Hours;

                //Se ejecuta cuando ya se ha llegado a la hora definida para esta tarea. Bajo un rango de +/- de 15min de la hora definida.
                if (difMin <= 35 && difMin >= 0 && difHor == 0)  
                {
                    ejecutar = true;
                }
            }
            
            return ejecutar;
        }
        
        #endregion
    }
}