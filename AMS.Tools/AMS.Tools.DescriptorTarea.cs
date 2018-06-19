using System;
using System.Text.RegularExpressions;
using AMS.DB;
using System.Net;
using System.Configuration;
using System.Collections;
using System.Data;
using System.IO;
using System.Net.Mail;

namespace AMS.Tools
{
    public class DescriptorTarea
    {
        //-----------------------------------------------------------------------------------------------------------------------
        //Funciones Publicas Estaticas para la ejecucion de las tareas   --------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------

        //Ejecutar tarea programada: SA_08D
        //Verifica ordenes SAC pendientes por cerrar cada 8 dias (parametrizables). Y cierra las que cumplan 
        //en estado cerrar 8 o mas dias.
        public static void CierreAutomaticoOrdenesSAC(string dataBaseCliente, string parametrosFuncionTarea)
        {
            //ParametrosFuncionTarea
            //diasDeCierre
            //    0  
            String[] arrParams = parametrosFuncionTarea.Split(',');
            DataSet dsOrdenesSAC = new DataSet();

            DBFunctions.RequestGlobal(dsOrdenesSAC, IncludeSchema.NO, dataBaseCliente,
              @"select mo.mord_numero, mo.mord_fechaimplement, vm.nombre, vm.mnit_nit, (CURRENT DATE - mo.mord_fechaimplement) as DIAS from 
                mordenservicio mo, msolicitud ms, vmnit vm where mo.test_codigo = 'IM' 
                and (CURRENT DATE - mo.mord_fechaimplement) >= " + arrParams[0] + 
              @" and mo.msol_numero = ms.msol_numero and vm.mnit_nit = ms.mnit_nitcli
                order by mo.mord_numero; ");

            for (int k=0; k < dsOrdenesSAC.Tables[0].Rows.Count; k++) // 1; k++)//
            {
                string correos = "";
                string mensaje = "";
                int respuesta = 0;
                string nitCliente = dsOrdenesSAC.Tables[0].Rows[k]["MNIT_NIT"].ToString();
                string ordenNum = dsOrdenesSAC.Tables[0].Rows[k]["MORD_NUMERO"].ToString(); 
                string dias = dsOrdenesSAC.Tables[0].Rows[k]["DIAS"].ToString();

                DataSet ds = new DataSet();
                DBFunctions.RequestGlobal(ds, IncludeSchema.NO, dataBaseCliente,
                    @"SELECT mn.mnit_email, mn.mnit_nombres CONCAT ' ' CONCAT mn.mnit_nombre2 CONCAT ' ' CONCAT mn.mnit_apellidos CONCAT ' ' CONCAT mn.mnit_apellido2 
                    FROM MCONTACTO mc,   mnit mn WHERE mc.mnit_nitcli = '" + nitCliente + "' AND   mc.mnit_nitcon = mn.mnit_nit");
                
                int resultado = DBFunctions.NonQueryGlobal("UPDATE MORDENSERVICIO SET TEST_CODIGO = 'CE' " +
                             ",MORD_FECHACIERRE = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                             ",MORD_HORACIERRE='" + DateTime.Now.TimeOfDay.ToString().Substring(0, 8) + "' " +
                             "WHERE MORD_NUMERO = " + ordenNum, dataBaseCliente);
                
                for (int r = 0; r <  ds.Tables[0].Rows.Count && resultado > 0; r++) //1; r++)//
                {
                    mensaje =
                        @"<br>
	                <div style='background-color:#EEEFD9;width: 75%;border-radius: 10px;margin: auto;padding: 20px;'>
	                <b><font size='5'>Notificación de Cierre Automatico de Solicitud: No. " + ordenNum + @"</font></b><img style='position: absolute;width: 100px; right: 16%; top: 5%;' src='http://ams.ecas.co/sac/img/logoEcas.png' />
	                <br><br>
	                <b>Sr(a). " + ds.Tables[0].Rows[r][1].ToString() + @"</b>, <br>
	                Nos permitimos informarle que su solicitud, citada en la referencia, ha sido cerrada por el gestor Automatico, 
                    debido a que usted no realizó el cierre correspondiente y han transcurddio " + dias + @" días desde su implementación. Si tiene alguna duda o inconveniente le agradecemos se sirva informarnos.
	                <br><br>";
                    
                    mensaje += @"Nuevamente les recordamos cerrar siempre sus Ordenes de Servicio en estado IMPLEMENTADAS para evitar que el gestor auitomatico las cierre.
	                <img style='width: 6%;' src='http://ams.ecas.co/sac/img/SAC.logo.sac.jpg' />
                    <br><br>Cordial saludo,<br>
                    <i>eCAS-SAC. Móvil 316 606 85 31</i>
	                </div>
                    <br><br>";

                    correos = ds.Tables[0].Rows[r][0].ToString();

                    try
                    {
                        respuesta = Tools.Utils.EnviarMail(correos, "Respuesta a Solicitud Ecas No. " + ordenNum, mensaje, TipoCorreo.HTML, "");
                        if (r == 0) //envia correo copia a ecas solo en la primera vuelta.
                        {
                            Tools.Utils.EnviarMail("sacsistemasecas@gmail.com", "Respuesta a Solicitud Ecas No. " + ordenNum, mensaje, TipoCorreo.HTML, "");
                        }
                    }
                    catch (Exception err)
                    { }
                }
            }
        }

        //Ejecutar tarea programada: OT_15M
        //Obtiene el estado actual de las OT no facturadas de los ultimos 15 días. Crea archivo txt y lo envia por FTP.
        public static void FTPEstadoActualOT(string dataBaseCliente, string parametrosFuncionTarea)
        {
            //ParametrosFuncionTarea
            //almacen,idArchivos,usuario,contraseña
            //    0  ,    1     ,   2   ,    3
            String[] arrParams = parametrosFuncionTarea.Split(',');
            DataSet dsOT15 = new DataSet();
            
            DBFunctions.RequestGlobal(dsOT15, IncludeSchema.NO, dataBaseCliente,
                @"select 
                pdoc_codigo concat mord_numeorde AS RO_NUMBER,
                mord_entrada AS OPEN_DATE,
                mord_salida AS CLOSE_DATE,
                facobra AS CUSTOMER_LABOR,
                sum(facrep) AS CUSTOMER_PARTS,
                0 AS CUSTOMER_MISC,
                '' AS GOG,
                '' AS COMPLAINT_CODE,
                mord_obseclie AS COMPLAINT_DESC,
                pven_codigo AS ADVISOR,
                pven_codigo AS TECHNICIAN,
                mord_obserece AS COMMENTS,
                '' AS LABOR_HOURS,
                '' AS PROMISED_DATE,
                mnit_nit AS CUST,
                mnit_nombres AS FIRST_NAME,
                mnit_apellidos AS LAST_NAME,
                '' AS ADDRESS,
                '' AS CITY,
                '' AS STATE,
                '' AS ZIP,
                mnit_celular AS PHONE,
                mnit_email AS EMAIL,
                pmar_nombre AS MAKE,
                pcat_descripcion AS MODEL,
                mcat_anomode AS YEAR,
                mcat_vin AS VIN,
                mcat_numeultikilo AS MILEAGE
                from
                (select mo.pdoc_codigo, mo.mord_numeorde, mo.mord_entrada, mo.mord_salida,  mo.mord_obseclie, mo.mord_obserece, mo.mnit_nit, mtr.pdoc_factura, mtr.mfac_numero, dit.facrep, dor.facobra, pv.pven_codigo,
                m.mnit_nombres, m.mnit_apellidos, m.mnit_telefono, m.mnit_email, p.pmar_nombre, pca.pcat_descripcion, mca.mcat_anomode, mo.mcat_vin , mca.mcat_numeultikilo
                from morden mo, mordentransferencia mtr, mcatalogovehiculo mca, pcatalogovehiculo pca, pmarca p, 
                (select pdoc_codigo, dite_numedocu, sum(dite_cantidad * dite_costprom) as facrep from ditems group by pdoc_codigo, dite_numedocu) as dit,
                (select pdoc_codigo, mord_numeorde, sum(dord_valooper) as facobra from dordenoperacion group by pdoc_codigo, mord_numeorde) as dor,
                pvendedor pv, mnit m
                where 
                mo.pdoc_codigo = mtr.pdoc_codigo  and mo.mord_numeorde=mtr.mord_numeorde
                and mtr.pdoc_factura = dit.pdoc_codigo and mtr.mfac_numero=dit.dite_numedocu
                and mo.pdoc_codigo = dor.pdoc_codigo  and mo.mord_numeorde=dor.mord_numeorde
                and mo.test_estado = 'A'
                and (DAYS(CURRENT DATE) - DAYS(mo.mord_entrada) ) <= 15
                and pv.pven_codigo = (select pven_codigo as facobra from dordenoperacion where pdoc_codigo=mo.pdoc_codigo and mord_numeorde=mo.mord_numeorde fetch first row only)
                and m.mnit_nit=mo.mnit_nit
                and mca.mcat_vin = mo.mcat_vin
                and pca.pcat_codigo = mca.pcat_codigo
                and pca.pmar_codigo = p.pmar_codigo
                and mo.palm_almacen = '" + arrParams[0] + @"' 
                ) as ot
                group by pdoc_codigo, mord_numeorde, mord_entrada, mord_salida, mord_obseclie, mord_obserece, pdoc_factura, facobra, pven_codigo, mnit_nit,
                mnit_nombres, mnit_apellidos, mnit_telefono, mnit_email, pmar_nombre, pcat_descripcion, mcat_anomode, mcat_vin, mcat_numeultikilo order by ro_number;");

            DescriptorTarea dt = new DescriptorTarea();
            dt.CrearArchivoTxt(dsOT15, arrParams);
        }

        //Ejecutar tarea programada: OT_24H
        //Obtiene el estado actual de las OT ya facturadas en el día Actual. Crea archivo txt y lo envia por FTP.
        public static void FTPFacturaDiariaOT(string dataBaseCliente, string parametrosFuncionTarea)
        {
            //ParametrosFuncionTarea
            //almacen,idArchivos,usuario,contraseña
            //    0  ,    1     ,   2   ,    3
            String[] arrParams = parametrosFuncionTarea.Split(',');
            DataSet dsOT24 = new DataSet();

            DBFunctions.RequestGlobal(dsOT24, IncludeSchema.NO, dataBaseCliente,
                @"select mo.pdoc_codigo, mo.mord_numeorde,  DATE(mo.mord_salida) as mord_salida, do.pven_codigo, mo.mcat_vin, mc.mcat_anomode, pc.pcol_descripcion, mc.pcat_codigo, mc.mcat_numeultikilo,
                pca.pcat_descripcion, m.pmar_nombre, sum( do.dord_valooper) as valOp
                from morden mo, dordenoperacion do, ptempario pt, mcatalogovehiculo mc, pcolor pc, pcatalogovehiculo pca, pmarca m
                where
                mo.pdoc_codigo = do.pdoc_codigo and mo.mord_numeorde = do.mord_numeorde
                and do.ptem_operacion = pt.ptem_operacion
                and mc.mcat_vin = mo.mcat_vin
                and pc.pcol_codigo = mc.pcol_codigo
                and pca.pcat_codigo = mc.pcat_codigo
                and m.pmar_codigo = pca.pmar_codigo
                and(mo.test_estado = 'F' or mo.test_estado = 'E')
                and DATE(mo.mord_salida) = DATE(current timestamp)
                and mo.palm_almacen = '" + arrParams[0] + @"'
                group by mo.pdoc_codigo, mo.mord_numeorde, mo.mord_salida, do.pven_codigo, mo.mcat_vin, mc.mcat_anomode, pc.pcol_descripcion, mc.pcat_codigo, mc.mcat_numeultikilo,
                pca.pcat_descripcion, m.pmar_nombre
                order by mo.pdoc_codigo, mo.mord_numeorde;");

            DBFunctions.RequestGlobal(dsOT24, IncludeSchema.NO, dataBaseCliente,
                @"select mo.pdoc_codigo, mo.mord_numeorde,  do.ptem_operacion, pt.ptem_descripcion
                from morden mo, dordenoperacion do, ptempario pt, mcatalogovehiculo mc, pcolor pc, pcatalogovehiculo pca, pmarca m
                where
                mo.pdoc_codigo = do.pdoc_codigo and mo.mord_numeorde = do.mord_numeorde
                and do.ptem_operacion = pt.ptem_operacion
                and mc.mcat_vin = mo.mcat_vin
                and pc.pcol_codigo = mc.pcol_codigo
                and pca.pcat_codigo = mc.pcat_codigo
                and m.pmar_codigo = pca.pmar_codigo
                and(mo.test_estado = 'F' or mo.test_estado = 'E')
                and DATE(mo.mord_salida) = DATE(current timestamp)
                and mo.palm_almacen = '" + arrParams[0] + @"';");

            DescriptorTarea dt = new DescriptorTarea();
            dt.CrearArchivoTxt24(dsOT24, arrParams);
        }

        //Ejecutar tarea programada: CT_18H
        //Gestiona mediante correo a los clientes con citas de taller. 
        //Envia correo a clientes con citas para mañana. Envia correo a clientes que tubieron citas hoy y no asistieron. 
        public static void EmailClienteCitasTaller(string dataBaseCliente, string parametrosFuncionTarea)
        {
            //ParametrosFuncionTarea
            //Para activar opcion el parametro debe ser 1, en caso contrario definir como 0.
            //citasMañana,citasNoCumlidas,ActualizacionOT
            //      0    ,      1        ,   2   
            String[] arrParams = parametrosFuncionTarea.Split(',');
            DateTime t1Actual = DateTime.Now;
            
            string empresa = DBFunctions.SingleDataGlobal("select gemp_descripcion from GEMPRESA where gemp_nombre='" + dataBaseCliente + "';");

            //Personas con citas para mañana.
            if (arrParams[0]== "1")
            {
                DateTime t2DiaSiguiente = DateTime.Now.AddDays(1);
                DataSet dsCitasDiaSiguiente = new DataSet();
                DBFunctions.RequestGlobal(dsCitasDiaSiguiente, IncludeSchema.NO, dataBaseCliente,
                    @"select m.mcit_hora, m.pcat_codigo, m.mcit_placa, m.mcit_nombre, m.mcit_email, (pa.palm_descripcion CONCAT '. ' CONCAT pa.palm_direccion CONCAT ' Tel. ' CONCAT pa.palm_telefono1 CONCAT ' - ' CONCAT pa.palm_telefono2) as contacto
                from MCITATALLER m left join palmacen pa on m.palm_almacen = pa.palm_almacen where m.mcit_fecha = DATE(current timestamp) + 1 DAYS;");

                for (int k = 0; k < dsCitasDiaSiguiente.Tables[0].Rows.Count; k++)
                {
                    string email = dsCitasDiaSiguiente.Tables[0].Rows[k]["mcit_email"].ToString();
                    string hora = dsCitasDiaSiguiente.Tables[0].Rows[k]["mcit_hora"].ToString();
                    string placa = dsCitasDiaSiguiente.Tables[0].Rows[k]["mcit_placa"].ToString();
                    string cliente = dsCitasDiaSiguiente.Tables[0].Rows[k]["mcit_nombre"].ToString();
                    string contacto = dsCitasDiaSiguiente.Tables[0].Rows[k]["contacto"].ToString();
                    try
                    {
                        bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                        if (isEmail)
                        {
                            string encabezado = "Recordatorio, Próxima Cita:";
                            string info = "Sr(a). " + cliente;
                            string contenido = "Nos permitimos recordarle que su vehículo con placa: " + placa + ", tiene una cita de taller para el día de mañana: " + t2DiaSiguiente.ToString("yyyy-MM-dd") + ", a las: " + hora;
                            string pie = "Para más información comunicarse con atención al Cliente.<br>" + contacto;
                            string urlImagen = "http://ecas.co/images/" + dataBaseCliente + ".jpg"; //"http://ecas.co/images/icon.jpg";

                            string mensaje =
                            @"<style>.tarjeta { width: 42%;}
                            @media only screen and (max-width: 700px) { .tarjeta { width: 90%;} }
                            </style><div class='tarjeta' style='position: absolute; background-color: #c7d8a7;border-radius: 10px;margin: auto;padding: 20px;box-shadow: 1px 7px 9px #888888;'>
                            <img style='width: 20%; position: absolute; right: 2%;box-shadow: 1px 7px 9px #666666;' src='" + urlImagen + @"' /><br><br>
                            <b>
                            <div style='padding: 0 0 0 3px;font-size: 15px; color: #ffffff; font-family: sans-serif; letter-spacing: 10px;'>" + empresa + @"</div>
                            <div style='padding: 5px 0 0 0;color: #153643; font-family: sans-serif;font-size: 33px; line-height: 38px; font-weight: bold;'>" + encabezado + @"</div></b>
                            <br>" + info + @"
                            <br><br>
                            <b>Reciba un cordial saludo</b>, <br>
                            " + contenido + @"<br><br>
                            " + pie + @"
                            <br><br>
                            <b>Gracias por su atención.</b>
                            <br>
                            <i>Mensaje enviado a través del Sistema eCAS.</i>
                            </div>";

                            if (k == 0)
                                Utils.EnviarMail("crodriguez@ecas.co,ancal999@gmail.com", "Recordatorio, Próxima Cita", mensaje, TipoCorreo.HTML, "");
                        }
                    }
                    catch (Exception err) { }
                }
            }
            //Personas con citas hoy y no vinieron.
            if (arrParams[1]=="1")
            {
                DataSet dsCitasPerdidas = new DataSet();
                DBFunctions.RequestGlobal(dsCitasPerdidas, IncludeSchema.NO, dataBaseCliente,
                    @"select m.mcit_hora, m.pcat_codigo, m.mcit_placa, m.mcit_nombre, m.mcit_email, (pa.palm_descripcion CONCAT '. ' CONCAT pa.palm_direccion CONCAT ' Tel. ' CONCAT pa.palm_telefono1 CONCAT ' - ' CONCAT pa.palm_telefono2) as contacto 
                from MCITATALLER m left join palmacen pa on m.palm_almacen = pa.palm_almacen where mcit_fecha = DATE(current timestamp) and testcit_estacita = 'N';");

                for (int k = 0; k < dsCitasPerdidas.Tables[0].Rows.Count; k++)
                {
                    string email = dsCitasPerdidas.Tables[0].Rows[k]["mcit_email"].ToString();
                    string hora = dsCitasPerdidas.Tables[0].Rows[k]["mcit_hora"].ToString();
                    string placa = dsCitasPerdidas.Tables[0].Rows[k]["mcit_placa"].ToString();
                    string cliente = dsCitasPerdidas.Tables[0].Rows[k]["mcit_nombre"].ToString();
                    string contacto = dsCitasPerdidas.Tables[0].Rows[k]["contacto"].ToString();
                    try
                    {
                        bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                        if (isEmail)
                        {
                            string encabezado = "Recordatorio, Cita no cumplida:";
                            string info = "Sr(a). " + cliente;
                            string contenido = "La cita que agendó para el día: " + t1Actual.ToString("yyyy-MM-dd") + ", a las: " + hora + ", para su vehículo con placa: " + placa + ", se ha registrado como no cumplida.";
                            string pie = "Por favor comunicarse con servicio al cliente para re-agendar su cita.<br>" + contacto;
                            string urlImagen = "http://ecas.co/images/" + dataBaseCliente + ".jpg"; //"http://ecas.co/images/icon.jpg";

                            string mensaje =
                            @"<style>.tarjeta { width: 42%;}
                            @media only screen and (max-width: 700px) { .tarjeta { width: 90%;} }
                            </style><div class='tarjeta' style='position: absolute; background-color: #c7d8a7;border-radius: 10px;margin: auto;padding: 20px;box-shadow: 1px 7px 9px #888888;'>
                            <img style='width: 20%; position: absolute; right: 2%;box-shadow: 1px 7px 9px #666666;' src='" + urlImagen + @"' /><br><br>
                            <b>
                            <div style='padding: 0 0 0 3px;font-size: 15px; color: #ffffff; font-family: sans-serif; letter-spacing: 10px;'>" + empresa + @"</div>
                            <div style='padding: 5px 0 0 0;color: #153643; font-family: sans-serif;font-size: 33px; line-height: 38px; font-weight: bold;'>" + encabezado + @"</div></b>
                            <br>" + info + @"
                            <br><br>
                            <b>Reciba un cordial saludo</b>, <br>
                            " + contenido + @"<br><br>
                            " + pie + @"
                            <br><br>
                            <b>Gracias por su atención.</b>
                            <br>
                            <i>Mensaje enviado a través del Sistema eCAS.</i>
                            </div>";

                            if (k == 0)
                                Utils.EnviarMail("crodriguez@ecas.co,ancal999@gmail.com", "Recordatorio: Cita no cumplida", mensaje, TipoCorreo.HTML, "");
                        }
                    }
                    catch (Exception err) { }
                }
            }
            //Actualizaciones en repuestos y operaciones relacionados a una OT.
            if (arrParams[2]=="1")
            {
                DataSet dsMovimientoTaller = new DataSet();
                DBFunctions.RequestGlobal(dsMovimientoTaller, IncludeSchema.NO, dataBaseCliente,
                    @"select (m.pdoc_codigo CONCAT ' - ' CONCAT m.mord_numeorde) as OT,p.ptem_descripcion as OPERACION,  t.test_nombre as ESTADO,
                    n.nombre as CLIENTE, n.mnit_email as EMAIL,
                    (pa.palm_descripcion CONCAT '. ' CONCAT pa.palm_direccion CONCAT ' Tel. ' CONCAT pa.palm_telefono1 CONCAT ' - ' CONCAT pa.palm_telefono2) as CONTACTO, mc.mcat_placa as PLACA
                    from morden m left join dordenoperacion d on d.pdoc_codigo = m.pdoc_codigo and d.mord_numeorde = m.mord_numeorde left join ptempario p on p.ptem_operacion=d.ptem_operacion,
                    testadooperacion t, vmnit n, palmacen pa, DESTADISTICAOPERACION des, mcatalogovehiculo mc
                    where t.test_estaoper=d.test_estado
                    and n.mnit_nit=m.mnit_nit
                    and pa.palm_almacen=m.palm_almacen 
                    and m.pdoc_codigo=des.pdoc_codigo and m.mord_numeorde=des.mord_numeorde 
                    and m.test_estado='A' and DATE(des.destoper_hora)=DATE(current timestamp) 
                    and mc.mcat_vin = m.mcat_vin ORDER BY OT;");

                DataSet dsMovimientoRepuestos = new DataSet();
                DBFunctions.RequestGlobal(dsMovimientoRepuestos, IncludeSchema.NO, dataBaseCliente,
                    @"select (m.pdoc_codigo CONCAT ' - ' CONCAT m.mord_numeorde) as OT, (di.dite_cantidad - coalesce(did.dite_cantidad, 0)) concat ' - ' concat mi.mite_nombre as ITEM
                    from morden m left join mordentransferencia t on t.pdoc_codigo = m.pdoc_codigo and t.mord_numeorde = m.mord_numeorde ,
                    ditems di left join ditems did on di.pdoc_codigo=did.dite_prefdocurefe and di.dite_numedocu= did.dite_numedocurefe and di.mite_codigo=did.mite_codigo and did.tmov_tipomovi=81 , mitems mi, DESTADISTICAOPERACION des
                    where t.pdoc_factura=di.pdoc_codigo and t.mfac_numero=di.dite_numedocu and mi.mite_codigo=di.mite_codigo
                    and m.pdoc_codigo=des.pdoc_codigo and m.mord_numeorde=des.mord_numeorde 
                    and m.test_estado='A' and DATE(des.destoper_hora)=DATE(current timestamp) 
                    and (di.dite_cantidad - coalesce(did.dite_cantidad, 0)) > 0 ORDER BY OT;");

                for (int k = 0; k < dsMovimientoTaller.Tables[0].Rows.Count; k++)
                {
                    string ot = dsMovimientoTaller.Tables[0].Rows[k]["OT"].ToString();
                    string estado = dsMovimientoTaller.Tables[0].Rows[k]["ESTADO"].ToString();
                    string cliente = dsMovimientoTaller.Tables[0].Rows[k]["CLIENTE"].ToString();
                    string email = dsMovimientoTaller.Tables[0].Rows[k]["EMAIL"].ToString();
                    string contacto = dsMovimientoTaller.Tables[0].Rows[k]["CONTACTO"].ToString();
                    string placa = dsMovimientoTaller.Tables[0].Rows[k]["PLACA"].ToString();

                    try
                    {
                        bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                        if (isEmail)
                        {
                            string encabezado = "Actualización de Taller:";
                            string info = "Sr(a). " + cliente;
                            string contenido = "Su vehículo con placa: " + placa + ", registra el siguiente estado:<br><br>";
                            string pie = "Para más información comunicarse con atención al Cliente.<br>" + contacto;
                            string urlImagen = "http://ecas.co/images/" + dataBaseCliente + ".jpg"; //"http://ecas.co/images/icon.jpg";
                            string operacion = dsMovimientoTaller.Tables[0].Rows[k]["OPERACION"].ToString() + " - " + estado;
                            contenido += "<b>Operaciones:</b><br>" + operacion + "<br>";

                            //Adicion de operaciones.
                            for (int j = k + 1; j < dsMovimientoTaller.Tables[0].Rows.Count; j++)
                            {
                                string otNext = dsMovimientoTaller.Tables[0].Rows[j]["OT"].ToString();
                                if (otNext == ot)
                                {
                                    estado = dsMovimientoTaller.Tables[0].Rows[j]["ESTADO"].ToString(); ;
                                    operacion = dsMovimientoTaller.Tables[0].Rows[j]["OPERACION"].ToString() + " - " + estado;
                                    contenido += operacion + "<br>";
                                    k++;
                                }
                                else { break; }
                            }

                            //Adicion de repuestos.
                            contenido += "<br><b>Repuestos instalados:</b><br>";
                            DataRow [] drRep = dsMovimientoRepuestos.Tables[0].Select("OT = '" + ot + "'");
                            for (int h = 0; h < drRep.Length; h++)
                            {
                                contenido += drRep[h][1].ToString() + "<br>";
                            }

                            string mensaje =
                            @"<style>.tarjeta { width: 42%;}
                            @media only screen and (max-width: 700px) { .tarjeta { width: 90%;} }
                            </style><div class='tarjeta' style='position: absolute; background-color: #c7d8a7;border-radius: 10px;margin: auto;padding: 20px;box-shadow: 1px 7px 9px #888888;'>
                            <img style='width: 20%; position: absolute; right: 2%;box-shadow: 1px 7px 9px #666666;' src='" + urlImagen + @"' /><br><br>
                            <b>
                            <div style='padding: 0 0 0 3px;font-size: 15px; color: #ffffff; font-family: sans-serif; letter-spacing: 10px;'>" + empresa + @"</div>
                            <div style='padding: 5px 0 0 0;color: #153643; font-family: sans-serif;font-size: 33px; line-height: 38px; font-weight: bold;'>" + encabezado + @"</div></b>
                            <br>" + info + @"
                            <br><br>
                            <b>Reciba un cordial saludo</b>, <br>
                            " + contenido + @"<br><br>
                            " + pie + @"
                            <br><br>
                            <b>Gracias por su atención.</b>
                            <br>
                            <i>Mensaje enviado a través del Sistema eCAS.</i>
                            </div>";

                            if (k == 0)
                                Utils.EnviarMail("crodriguez@ecas.co,ancal999@gmail.com", "Actualizacion de Taller", mensaje, TipoCorreo.HTML, "");
                        }
                    }
                    catch (Exception err) { }
                }
            }
        }

        //Ejecutar tarea programada: NO ESTA DEFINIDA AUN..............
        private void DesasignacionAutomatica()
        {
            DataSet dsVehDesasignar = new DataSet();
            ArrayList sqlStrings = new ArrayList();
            String emailDestino = DBFunctions.SingleData("select mnit_email from mnit where mnit_nit=(select cveh_identencargado from cvehiculos fetch first row only);");
            String mensaje = "Email Informativo eCAS:<br><br>" +
                             "Los siguientes pedidos exceden 20 días en estado asignado, y no superan un monto de " +
                             "anticipo mayor a $2.000.000. Por lo que se han actualizado automáticamente  a estado " +
                             "Desasignado:<br><br>";

            String tabla = "<table border='1' width='450'>" +
                           "<tr>" +
                           "    <th>No. Inventario</th>" +
                           "    <th>Codigo Pedido</th>" +
                           "    <th>Número Pedido</th>" +
                           "    <th>Esatado</th>" +
                           "</tr>";

            DBFunctions.Request(dsVehDesasignar, IncludeSchema.NO,
                "select  ma.mveh_inventario, ma.pdoc_codigo, ma.mped_numepedi, ( DAYS(current date)-DAYS(DATE(masi_fechasig)) ) as dias " +
                "from MASIGNACIONVEHICULO ma, mvehiculo mv " +
                "where  ma.mveh_inventario = mv.mveh_inventario and mv.test_tipoesta = 30 and " +
                "( DAYS(current date)-DAYS(DATE(masi_fechasig)) ) > ( select cveh_diasasignacion from CVEHICULOS );");

            if (dsVehDesasignar.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < dsVehDesasignar.Tables[0].Rows.Count; i++)
                {
                    String numInv = dsVehDesasignar.Tables[0].Rows[i][0].ToString();
                    String codPed = dsVehDesasignar.Tables[0].Rows[i][1].ToString();
                    String numPed = dsVehDesasignar.Tables[0].Rows[i][2].ToString();

                    tabla += "<tr align='center'>" +
                             "   <td>" + numInv + "</td>" +
                             "   <td>" + codPed + "</td>" +
                             "   <td>" + numPed + "</td>" +
                             "   <td>Desasignado</td>" +
                             "</tr>";

                    sqlStrings.Add(
                        "DELETE from MASIGNACIONVEHICULO where mveh_inventario=" + numInv + " " +
                        "and pdoc_codigo='" + codPed + "' and mped_numepedi=" + numPed + ";");

                    sqlStrings.Add(
                        "UPDATE mpedidovehiculo SET test_tipoesta=10 WHERE pdoc_codigo='" + codPed + "' AND mped_numepedi=" + numPed + ";");

                    sqlStrings.Add(
                        "UPDATE mvehiculo SET test_tipoesta=20 WHERE mveh_inventario=" + numInv + ";");

                    String datos = numInv + "," + codPed + "," + numPed;
                    sqlStrings.Add(
                        "INSERT INTO MHISTORIAL_CAMBIOS VALUES(" +
                        "DEFAULT, 'MASIGNACIONVEHICULO', 'D', '" + datos + "', 'ecasweb', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');");
                }

                tabla += "</table>";
                mensaje += tabla + "<BR><BR>Atentamente:<BR>Servidor de información eCAS.";

                if (DBFunctions.Transaction(sqlStrings))
                {
                    Utils.EnviarMail(emailDestino, "Desasignacion de vehiculos", mensaje, TipoCorreo.HTML, "");
                }
            }
        }


        //-----------------------------------------------------------------------------------------------------------------------
        //Funciones Privadas para la ejecucion de las tareas   ------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------

        private string directorioArchivo = ConfigurationManager.AppSettings["PathToDownloads"];

        //Crear un archivo .txt según las especificaciones determinadas (Caso para ClearMechanic). Y enciarlo por FTP a servidores de ClearMechanic.
        //Referencia de tiempo: cada 15minutos.
        private void CrearArchivoTxt(DataSet dsOT, String [] arrParams)
        {
            //Patron nombre de archivo: CODIGO_EMPRESA-TIPO_REPETICION-MMDDYYYYHHMM  Ej: CO1035-WIP-050620161551.txt
            DateTime fecha = DateTime.Now;
            //Random r = new Random();

            //string nombreArchivo = "CO1035 -WIP-" + fecha.ToString("MMddyyyyHHmm") + ".txt";
            string nombreArchivo = arrParams[1] + "-WIP-" + fecha.ToString("MMddyyyyHHmm") + ".txt";

            StreamWriter sw = File.CreateText(directorioArchivo + nombreArchivo);

            //Encabezado de archivo.
            string line = "Ro Number|Open Date|Close Date|Customer Labor $|Customer Parts $|Customer Misc $|GOG $|Complaint Code|Complaint Desc.|Advisor #|Technician #|Comments|Labor Hours|Promised Date|Cust #|FirstName|LastName|Address|City|State|Zip|Phone|Email|Make|Model|Year|VIN|Mileage";
            sw.WriteLine(line);

            //Cuerpo de archivo.
            for (int i = 0; i < dsOT.Tables[0].Rows.Count; i++)
            {
                string fechaEntrada = "";
                string fechaSalida = "";

                if (dsOT.Tables[0].Rows[i][1].ToString() != "")
                    fechaEntrada = Convert.ToDateTime(dsOT.Tables[0].Rows[i][1].ToString()).ToString("yyyyMMdd");
                if (dsOT.Tables[0].Rows[i][2].ToString() != "")
                    fechaSalida = Convert.ToDateTime(dsOT.Tables[0].Rows[i][2].ToString()).ToString("yyyyMMdd");

                //         0         1      2         3      4     5     6  7          8                                                                                               9     10      11        12       13      14     15     16           17           18       19   20        21         22            23         24       25      26             27
                //Patron: 208496|20130108|20130108|600.00|368.51|18.43|0.00||SES LAMP IS ON, CHECK AND ADVISE $145.00 TO START..^RUNNING ROUGH^RT FRT HIGH BEAM INOP ADVISEBELTS WORN.|47|146^146^146||3.00 0.00 1.00|20130108|65402|LEONA|RUSSELL|530 E. 53RD CT.|MERRILLIVILLE|IN|46410|219-588-2869|bla@hotmail.com|FIAT|IDEA ADVENTURE|2015|9BD13531BF2275425|7856.0000
                line = dsOT.Tables[0].Rows[i][0].ToString() + "|" +
                       fechaEntrada + "|" +
                       fechaSalida + "|" +
                       dsOT.Tables[0].Rows[i][3].ToString().Replace(',', '.') + "|" +
                       dsOT.Tables[0].Rows[i][4].ToString().Replace(',', '.') + "|" +
                       dsOT.Tables[0].Rows[i][5].ToString().Replace(',', '.') + "|" +
                       dsOT.Tables[0].Rows[i][6].ToString().Replace(',', '.') + "|" +
                       dsOT.Tables[0].Rows[i][7].ToString() + "|" +
                       dsOT.Tables[0].Rows[i][8].ToString().Replace("\r\n", "") + "|" +
                       dsOT.Tables[0].Rows[i][9].ToString() + "|" +
                       dsOT.Tables[0].Rows[i][10].ToString() + "|" +
                       dsOT.Tables[0].Rows[i][11].ToString().Replace("\r\n", "") + "|" +
                       dsOT.Tables[0].Rows[i][12].ToString().Replace(',', '.') + "|" +
                       dsOT.Tables[0].Rows[i][13].ToString() + "|" +
                       dsOT.Tables[0].Rows[i][14].ToString() + "|" +
                       dsOT.Tables[0].Rows[i][15].ToString() + "|" +
                       dsOT.Tables[0].Rows[i][16].ToString() + "|" +
                       dsOT.Tables[0].Rows[i][17].ToString() + "|" +
                       dsOT.Tables[0].Rows[i][18].ToString() + "|" +
                       dsOT.Tables[0].Rows[i][19].ToString() + "|" +
                       dsOT.Tables[0].Rows[i][20].ToString() + "|" +
                       dsOT.Tables[0].Rows[i][21].ToString() + "|" +
                       dsOT.Tables[0].Rows[i][22].ToString() + "|" +
                       dsOT.Tables[0].Rows[i][23].ToString() + "|" +
                       dsOT.Tables[0].Rows[i][24].ToString() + "|" +
                       dsOT.Tables[0].Rows[i][25].ToString() + "|" +
                       dsOT.Tables[0].Rows[i][26].ToString() + "|" +
                       dsOT.Tables[0].Rows[i][27].ToString().Replace(',', '.');

                sw.WriteLine(line);
            }

            sw.Close();
            EnviarArchivoFTP(nombreArchivo, arrParams);
        }

        //Crear un archivo .txt según las especificaciones determinadas (Caso para ClearMechanic). Y enciarlo por FTP a servidores de ClearMechanic.
        //Referencia de tiempo: cada 24Horas.
        private void CrearArchivoTxt24(DataSet dsOT, String[] arrParams)
        {
            //Patron nombre de archivo: CODIGO_EMPRESA-TIPO_REPETICION-MMDDYYYYHHMM  Ej:  	CO1035-ROH-050620161551.txt 
            DateTime fecha = DateTime.Now;
            //Random r = new Random();

            //string nombreArchivo = "CO1035-ROH-" + fecha.ToString("MMddyyyyHHmm") + ".txt";
            string nombreArchivo = arrParams[1] + "-ROH-" + fecha.ToString("MMddyyyyHHmm") + ".txt";

            StreamWriter sw = File.CreateText(directorioArchivo + nombreArchivo);

            //Encabezado de archivo.
            string line = "Ro Number|Op Code|Op Description|Customer Labor $|Customer Parts $|Customer Misc $|GOG $|Labor Op Code|CWI Flag|Technician #|Close Date|VIN|Make|Model|Color/Trim|Year|Mileage";
            sw.WriteLine(line);

            //Cuerpo de archivo.
            for (int i = 0; i < dsOT.Tables[0].Rows.Count; i++)
            {
                //         0       1        2               3       4    5    6  7 8  9      10        11          12   13                      14         15   16
                //Patron: 208698|62BMZ|*ENGINE DIAGNOSIS |174.00|175.92|8.80|0.00||C|40|20130117|WBAHN83587DT76617|BM|750LI|BLK SAPPH METBLK PERF NASCALTH|07|43310
                line = dsOT.Tables[0].Rows[i][0].ToString() + dsOT.Tables[0].Rows[i][1].ToString() + "|";

                string codigosOT = "";
                string descripcionOT = "";
                //Datos de operaciones relacionados a una OT.
                DataRow[] drOps = dsOT.Tables[1].Select("PDOC_CODIGO = '" + dsOT.Tables[0].Rows[i][0].ToString() + "' AND MORD_NUMEORDE = " + dsOT.Tables[0].Rows[i][1].ToString());

                for (int k = 0; k < drOps.Length; k++)
                {
                    codigosOT += drOps[k].ItemArray[2] + ",";
                    descripcionOT += drOps[k].ItemArray[3] + ",";
                }
                codigosOT = codigosOT.Substring(0, codigosOT.Length - 1);
                descripcionOT = descripcionOT.Substring(0, descripcionOT.Length - 1);

                string fechaSalida = "";
                if (dsOT.Tables[0].Rows[i][2].ToString() != "")
                    fechaSalida = Convert.ToDateTime(dsOT.Tables[0].Rows[i][2].ToString()).ToString("yyyyMMdd");

                line += codigosOT + "|" + descripcionOT + "|" + dsOT.Tables[0].Rows[i][11].ToString().Replace(',', '.') + "||||||";
                line += dsOT.Tables[0].Rows[i][3].ToString() + "|" +
                        fechaSalida + "|" +
                        dsOT.Tables[0].Rows[i][4].ToString() + "|" +
                        dsOT.Tables[0].Rows[i][10].ToString() + "|" +
                        dsOT.Tables[0].Rows[i][7].ToString() + " " + dsOT.Tables[0].Rows[i][7].ToString() + "|" +
                        dsOT.Tables[0].Rows[i][6].ToString() + "|" +
                        dsOT.Tables[0].Rows[i][5].ToString() + "|" +
                        dsOT.Tables[0].Rows[i][8].ToString().Replace(',', '.');

                sw.WriteLine(line);
            }

            sw.Close();
            EnviarArchivoFTP(nombreArchivo, arrParams);
        }

        //Enviar un archivo de texto mediante FTP, según los parametros de conexión especificados. (basado ClearMechanic)
        private void EnviarArchivoFTP(string archivoTxt, String[] arrParams)
        {
            string ftpurl = "ftp://integration.clearmechanic.com/";
            string username = arrParams[2]; // "autoorionsur";
            string filename = archivoTxt;
            string password = arrParams[3]; // "n96098qY1o";

            FtpWebRequest ftpClient = (FtpWebRequest)FtpWebRequest.Create(ftpurl + "" + filename);
            ftpClient.Credentials = new System.Net.NetworkCredential(username, password);
            ftpClient.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
            ftpClient.UseBinary = true;
            ftpClient.KeepAlive = true;
            System.IO.FileInfo fi = new System.IO.FileInfo(directorioArchivo + filename);
            ftpClient.ContentLength = fi.Length;
            byte[] buffer = new byte[4097];
            int bytes = 0;
            int total_bytes = (int)fi.Length;
            System.IO.FileStream fs = fi.OpenRead();
            System.IO.Stream rs = ftpClient.GetRequestStream();
            while (total_bytes > 0)
            {
                bytes = fs.Read(buffer, 0, buffer.Length);
                rs.Write(buffer, 0, bytes);
                total_bytes = total_bytes - bytes;
            }
            //fs.Flush();
            fs.Close();
            rs.Close();
            FtpWebResponse uploadResponse = (FtpWebResponse)ftpClient.GetResponse();
            string value = uploadResponse.StatusDescription;
            uploadResponse.Close();
        }
    }
}