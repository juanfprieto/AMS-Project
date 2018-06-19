/*Modal Dialog*/

//function inicio() {
//    var mensajeerror = "Boton derecho deshabilitado ¡Gracias por visitarnos!"; if (document.layers) window.captureEvents(Event.MOUSEDOWN); function bloquear(e) {
//        if (navigator.appName == 'Netscape' && (e.which == 2 || e.which == 3)) { alert(mensajeerror); return false; }
//        if (navigator.appName == 'Microsoft Internet Explorer' && (event.button == 2 || event.button == 3)) {
//            alert(mensajeerror); return false;
//        }
//    } window.onmousedown = bloquear; document.onmousedown = bloquear;

//    agregarEspera();
//    blockf12();
//}
//function blockf12() {
//    $("body").keydown(function (event) {

//        if (event.keyCode == 123) {
//            event.preventDefault();
//        }
//        else {
//            if (event.keyCode == 13) {
//                event.keyCode = 9;
//            }

//        }
//    });

//}

function loadSrc()
{
     var tip=location.href.substring((location.href.indexOf("?"))+1,location.href.indexOf("="));
     if(tip=="params")
     {	
     	var params = location.href.substring(location.href.indexOf("params"),location.href.length);
     	document.all.iFrameModalDialog.src = "AMS.Web.ModalDialog.aspx?" + params;
     	document.all.iFrameModalDialog.height = window.innerHeight - 20;
     }
     else if(tip=="table")
	 {
		 var params = location.href.substring(location.href.indexOf("table"),location.href.length);
		 document.all.iFrameModalDialog.src = "AMS.Web.ModalDialogIns.aspx?" + params;
		 document.all.iFrameModalDialog.height = window.innerHeight - 20;
	 }	
}

function Carga_ModalActFij()
{
	document.all.iFrameModalIngActFij.src="AMS.Web.ModalIngActFij.aspx";
}

function ModalDialogFacAdmin2(obj,table,select) {

    table = table.toUpperCase();
    if (table.indexOf(" START") == -1 && table.indexOf(" STOP") == -1 && table.indexOf("DATABASE ") == -1 && table.indexOf("TABLE ") == -1 && table.indexOf("INSERT ") == -1 && table.indexOf("UPDATE ") == -1 && table.indexOf("DELETE ") == -1 && table.indexOf("DROP ") == -1 && table.indexOf("ALTER ") == -1 && table.indexOf("TRUNCATE ") == -1) {
        var modalDialogHeight = "600";
        var modalDialogWidth = "700";
        var param;
        var i;
        var urlM;
        if (table != null) {
            urlM = "AMS.Web.ModalDialogIFrame.aspx?table=" + table + "&Ttemp=1&sql=" + select + "&action=insert";
            param = OpenModal(urlM, "AMS Seleccion", modalDialogWidth, modalDialogHeight);
            if (!param)
                param = window.document.getElementById('HDNINPT_MODALSELECT').value;
            if (param != null) {
                var sp = param.split(',');
                var objeto;
                for (i = 0; i < sp.length; i++) {
                    if (i == 0) {
                        objeto = obj.name;
                        if (document.all[objeto] != null)
                            document.all[objeto].value = sp[i];
                    }
                    else {
                        objeto = obj.name + String(i);
                        if (document.all[objeto] != null)
                            document.all[objeto].value = sp[i];
                    }
                }
            }
        }
        else
            alert("Error : No se ha ingresado una tabla");
    }
    else {
        alert("Inconsistencia en consulta. Por favor informar a Sistemas ECAS. SQL: " + table);
    }
}


//ModalDialog común.--------------
function ModalDialogFacAdmin(obj, table, select) {

    table = table.toUpperCase();
    if (table.indexOf(" START") == -1 && table.indexOf(" STOP") == -1 && table.indexOf("DATABASE ") == -1 && table.indexOf("TABLE ") == -1 && table.indexOf("INSERT ") == -1 && table.indexOf("UPDATE ") == -1 && table.indexOf("DELETE ") == -1 && table.indexOf("DROP ") == -1 && table.indexOf("ALTER ") == -1 && table.indexOf("TRUNCATE ") == -1) {
        var modalDialogHeight = "540";
        var modalDialogWidth = "580";

        var urlM;
        'mafj_codiacti AS Codigo,  mafj_descripcion AS Descripcion,   mafj_valohist AS Valor'
        select = select.toUpperCase();
        select = select.replace("SELECT", "").trim();
        var fromIndex = select.indexOf("FROM");
        select = select.substr(0, fromIndex).trim();
        var arrayCampos = select.split(",");
        var campos = "";
        for (var i = 0; i < arrayCampos.length; i++) {
            if(arrayCampos[i].indexOf(" AS ") != -1){
                campos += arrayCampos[i].substr(0, arrayCampos[i].indexOf(" AS ")).trim() + ",";
            }
            else if(arrayCampos[i].indexOf(" ") != -1){
                campos += arrayCampos[i].substr(0, arrayCampos[i].indexOf(" ")).trim();
            }
        }

        
        if (table != null) {
            //urlM = "AMS.Web.ModalDialogIFrame.aspx?table=" + table + "&Ttemp=1&sql=" + select + "&action=insert";
            //urlM = "AMS.Web.ModalDialog.aspx?params=" + table + "&Ins=1&valor=" + obj.value;
            urlM = "AMS.Web.ModalDialogIns.aspx?table=" + table + "&action=insert&campos=" + campos;

            //param = OpenModal(urlM, "AMS Seleccion", modalDialogWidth, modalDialogHeight);
            $("#dialog").html('<iframe src="' + urlM + '" height="530" widht="580" frameborder="0" ></iframe>' +
                        '<form name="formName"><input type="hidden" value="' + obj.id + '" name="objeton" id="objetoi"/>' +
                        '<input type="hidden" value="' + campos + '" name="funcion" id="funciones"/></form>');
            $("#dialog").css({ 'visibility': 'visible' });
            $("#dialog").dialog({ modal: true, title: 'Cuadro Dialogo', height: 590, width: 580 });
            $("#dialogI").html = obj;
            $(".ui-widget-overlay").css({ background: "#000", opacity: 0.46 });
        }
        else
            alert("Error : No se ha ingresado una tabla");
    }
    else {
        alert("Inconsistencia en consulta. Por favor informar a Sistemas ECAS. SQL: " + table);
    }
}

//-----------------

function ModalDialog2(obj, table, controls, oIns) {

   table = table.toUpperCase();
   if (table.indexOf(" START") == -1 && table.indexOf(" STOP") == -1 && table.indexOf("DATABASE ") == -1 && table.indexOf("TABLE ") == -1 && table.indexOf("INSERT ") == -1 && table.indexOf("UPDATE ") == -1 && table.indexOf("DELETE ") == -1 && table.indexOf("DROP ") == -1 && table.indexOf("ALTER ") == -1 && table.indexOf("TRUNCATE ") == -1) {
       var modalDialogHeight = "500";
       var modalDialogWidth = "500";

       var control;
       var splitId;
       var newId;
       var param;
       var urlM;
       if (oIns != null)
           urlM = "AMS.Web.ModalDialogIFrame.aspx?params=" + table + "&Ins=1";
       else
           urlM = "AMS.Web.ModalDialogIFrame.aspx?params=" + table + "";
       param = OpenModal(urlM, "AMS Seleccion", modalDialogWidth, modalDialogHeight);
       if (!param)
           param = window.document.getElementById('HDNINPT_MODALSELECT').value;
       if (param != null) {
           var sp = param.split(',');
           var objeto;
           for (i = 0; i < sp.length; i++) {
               if (i == 0) {
                   objeto = obj.name;
                   if (document.all[objeto] != null)
                       document.all[objeto].value = sp[i];
               }
               else {
                   objeto = obj.name + String(i);
                   if (document.all[objeto] != null)
                       document.all[objeto].value = sp[i];
               }
           }
       }
   }
   else {
       alert("Inconsistencia en consulta. Por favor informar a Sistemas ECAS. SQL: " + table);
   }
}

function OpenModal(urlM,mTitle,wWidth,wHeight){
    window.document.getElementById('HDNINPT_MODALSELECT').value='';
    
    if(window.showModalDialog){
        return(showModalDialog(urlM,mTitle,"dialogHeight=" + wHeight + "px;dialogWidth=" + wWidth + "px"));
    }
    else
    {
        window.open(urlM, mTitle, 'height=' + wHeight + ',width=' + wWidth + ',toolbar=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no,modal=yes');
        return('');
    }
}

//Funcion para ajustar el alias de los codigos SQL, de manera que no aparesca el nombre real del campo.
function ajustarAliasSQL(sqlAjustado) {
    var sqlAjustadoAux = "";
    sqlAjustado = sqlAjustado.toUpperCase();

    if (sqlAjustado.indexOf("CONCAT") == -1 && sqlAjustado.indexOf("COALESCE") == -1 && sqlAjustado.indexOf("DISTINCT") == -1 && sqlAjustado.indexOf("(") == -1 && sqlAjustado.indexOf("*") == -1) {
        sqlAjustado = sqlAjustado.replace("SELECT", "").trim();

        var fromIndex = sqlAjustado.indexOf("FROM");
        sqlAjustadoAux = sqlAjustado.substr(fromIndex, (sqlAjustado.length - fromIndex));
        sqlAjustado = sqlAjustado.substr(0, fromIndex);

        var arrayCampos = sqlAjustado.split(",");
        var arrayCamposAux = [];
        var _index;
        var encabezadoAlias = "";

        for (var i = 0; i < arrayCampos.length; i++) {
            arrayCampos[i] = arrayCampos[i].trim();
            arrayCamposAux[i] = arrayCampos[i];
            _index = arrayCampos[i].indexOf("_") + 1;
            arrayCampos[i] = arrayCampos[i].substr(_index, (arrayCampos[i].length - _index));

            if (arrayCampos[i].indexOf(" AS ") == -1 && arrayCampos[i].indexOf(" ") == -1) {
                encabezadoAlias += arrayCamposAux[i] + " AS " + arrayCampos[i] + ",";
            }
            else {
                encabezadoAlias += arrayCamposAux[i] + ",";
            }
        }

        encabezadoAlias = encabezadoAlias.substr(0, encabezadoAlias.length - 1);
        sqlAjustadoAux = "SELECT " + encabezadoAlias + " " + sqlAjustadoAux;

        return sqlAjustadoAux;
    }
    else {
        return sqlAjustado;
    }
}

var asociarObjetos = null;
//ModalDialog común.
function ModalDialog(obj, table, controls, oIns, padre, asociarObjs) {
    //table = table.toUpperCase();
    if (asociarObjs != undefined && asociarObjs != null)
        asociarObjetos = asociarObjs;

    table = ajustarAliasSQL(table); //Ajuste de alias en el SQL.

    if (table.indexOf(" START") == -1 && table.indexOf(" STOP") == -1 && table.indexOf("DATABASE ") == -1 && table.indexOf(" TABLE ") == -1 && table.indexOf("INSERT ") == -1 && table.indexOf("UPDATE ") == -1 && table.indexOf("DELETE ") == -1 && table.indexOf("DROP ") == -1 && table.indexOf("ALTER ") == -1 && table.indexOf("TRUNCATE ") == -1) {
        var modalDialogHeight = "540";
        var modalDialogWidth = "580";

        var control;
        var splitId;
        var newId;
        var param;
        var urlM = "";
        var urlM2 = "";
        if (oIns != null && padre != null) {
            urlM = "AMS.Web.ModalDialogIFrame.aspx?params=" + table + "&Ins=1&Padre=1";
            urlM2 = "AMS.Web.ModalDialog.aspx?params=" + table + "&Ins=1&Padre=1&valor=" + obj.value;
        }
        else if (oIns != null && padre == null) {
            urlM = "AMS.Web.ModalDialogIFrame.aspx?params=" + table + "&Ins=1";
            urlM2 = "AMS.Web.ModalDialog.aspx?params=" + table + "&Ins=1&valor=" + obj.value;
        }
        else if (oIns == null && padre != null) {
            urlM = "AMS.Web.ModalDialogIFrame.aspx?params=" + table + "&Padre=1";
            urlM2 = "AMS.Web.ModalDialog.aspx?params=" + table + "&Padre=1";
        }
        else {
            urlM = "AMS.Web.ModalDialogIFrame.aspx?params=" + table + "";
            urlM2 = "AMS.Web.ModalDialog.aspx?params=" + table + "";
        }

        //codigo con el Antiguo Modal Dialog de IExplorer 
        //param = OpenModal(urlM, "AMS Seleccion", modalDialogWidth, modalDialogHeight);

        $("#dialog").html('<iframe src="' + urlM2 + '" height="530" widht="580" frameborder="0" ></iframe>' +
                        '<form name="formName"><input type="hidden" value="' + obj.id + '" name="objeton" id="objetoi"/>' +
                        '<input type="hidden" value="' + oIns + '" name="funcion" id="funciones"/></form>');
        $("#dialog").css({ 'visibility': 'visible' });
        $("#dialog").dialog({ modal: true, title: 'Cuadro Dialogo', height: 590, width: 580 });
        $("#dialogI").html = obj;
        $(".ui-widget-overlay").css({ background: "#000", opacity: 0.46 });
    }
    else {
        alert("Inconsistencia en consulta. Por favor informar a Sistemas ECAS. SQL: " + table);
    }
}

//ModalDialog para inventarios.
function ModalDialogInventarios(obj, table, controls, oIns, valueLine) {
    table = table.toUpperCase();
    if (table.indexOf(" START") == -1 && table.indexOf(" STOP") == -1 && table.indexOf("DATABASE ") == -1 && table.indexOf("TABLE ") == -1 && table.indexOf("INSERT ") == -1 && table.indexOf("UPDATE ") == -1 && table.indexOf("DELETE ") == -1 && table.indexOf("DROP ") == -1 && table.indexOf("ALTER ") == -1 && table.indexOf("TRUNCATE ") == -1) {
        var modalDialogHeight = "500";
        var modalDialogWidth = "800";
        var control;
        var splitId;
        var newId;
        var param;
        var urlM = "";
        var urlM2 = "";

        if (oIns != null) {
            urlM = "AMS.Web.ModalDialogIFrame.aspx?params=" + table + "&Ins=1&Value=" + valueLine;
            urlM2 = "AMS.Web.ModalDialog.aspx?params=" + table + "&Ins=1&Value=" + valueLine + "&valor=" + obj.value;
        }
        else {
            urlM = "AMS.Web.ModalDialogIFrame.aspx?params=" + table + "&Value=" + valueLine;
            urlM2 = "AMS.Web.ModalDialog.aspx?params=" + table + "&Value=" + valueLine;
        }

        //param = OpenModal(urlM, "AMS Seleccion", modalDialogWidth, modalDialogHeight);

        $("#dialog").html('<iframe src="' + urlM2 + '" height="530" widht="700" frameborder="0" ></iframe>' +
                        '<form name="formName"><input type="hidden" value="' + obj.id + '" name="objeton" id="objetoi"/>' +
                        '<input type="hidden" value="' + oIns + '" name="funcion" id="funciones"/></form>');
        $("#dialog").css({ 'visibility': 'visible' });
        $("#dialog").dialog({ modal: true, title: 'Cuadro Dialogo', height: 590, width: 700 });
        $("#dialogI").html = obj;
    }
    else {
        alert("Inconsistencia en consulta. Por favor informar a Sistemas ECAS. SQL: " + table);
    }
}

//Carga ModalDialog de operaciones.
function ModalDialogAT(obj, objEX, table, controls, oIns, inf) {
    table = table.toUpperCase();
    if (table.indexOf(" START") == -1 && table.indexOf(" STOP") == -1 && table.indexOf("DATABASE ") == -1 && table.indexOf("TABLE ") == -1 && table.indexOf("INSERT ") == -1 && table.indexOf("UPDATE ") == -1 && table.indexOf("DELETE ") == -1 && table.indexOf("DROP ") == -1 && table.indexOf("ALTER ") == -1 && table.indexOf("TRUNCATE ") == -1) {
        var modalDialogHeight = "540";
        var modalDialogWidth = "580";
        var table2 = table;
        var control = 0;
        var suma = 0;

        if (document.all[objEX] != null) {
            if (document.all[objEX].value != null) {
                while (table2.indexOf("@") != -1)
                    table2 = table2.replace("@", String(document.all[objEX].value));
                control = 1;
            }
            else {
                alert("No se ha seleccionado un valor previo correspondiente al " + inf);
                table2 = inf;
                control = 2;
            }
        }

        if (control = 1) {
            var control;
            var splitId;
            var newId;
            var param;
            var urlM = "";
            var urlM2 = "";

            if (oIns != null) {
                urlM = "AMS.Web.ModalDialogIFrame.aspx?params=" + table2 + "&Ins=1";
                urlM2 = "AMS.Web.ModalDialog.aspx?params=" + table2 + "&Ins=1";
            }
            else {
                urlM = "AMS.Web.ModalDialogIFrame.aspx?params=" + table2 + "";
                urlM2 = "AMS.Web.ModalDialog.aspx?params=" + table2 + "";
            }

            //param = OpenModal(urlM, "AMS Seleccion", modalDialogWidth, modalDialogHeight);

            $("#dialog").html('<iframe src="' + urlM2 + '" height="530" widht="580" frameborder="0" ></iframe>' +
                        '<form name="formName"><input type="hidden" value="' + obj.id + '" name="objeton" id="objetoi"/>' +
                        '<input type="hidden" value="' + oIns + '" name="funcion" id="funciones"/></form>');
            $("#dialog").css({ 'visibility': 'visible' });
            $("#dialog").dialog({ modal: true, title: 'Cuadro Dialogo', height: 590, width: 580 });
            $("#dialogI").html = obj;
        }
    }
    else {
        alert("Inconsistencia en consulta. Por favor informar a Sistemas ECAS. SQL: " + table);
    }
}

//Carga ModalDialog de Pagos
function ModalDialogPagos(obj, table, campo, numeros) {
    table = table.toUpperCase();
    if (table.indexOf(" START") == -1 && table.indexOf(" STOP") == -1 && table.indexOf("DATABASE ") == -1 && table.indexOf("TABLE ") == -1 && table.indexOf("INSERT ") == -1 && table.indexOf("UPDATE ") == -1 && table.indexOf("DELETE ") == -1 && table.indexOf("DROP ") == -1 && table.indexOf("ALTER ") == -1 && table.indexOf("TRUNCATE ") == -1) {
        var modalDialogHeight = "500";
        var modalDialogWidth = "500";
        var param;
        var urlM = "";
        var urlM2 = "";
        var oIns = "";

        var arregloObjetosMod = null;
        if (numeros != null)
            arregloObjetosMod = numeros.split(',');

        if (numeros != null && arregloObjetosMod.length <= 1) {
            urlM = "AMS.Web.ModalDialogPagosIFrame.aspx?params=" + table + "&numeros=" + numeros + "";
            urlM2 = "AMS.Web.ModalDialog.aspx?params=" + table + "&numeros=" + numeros + "&proced=pagos";
        }
        else {
            urlM = "AMS.Web.ModalDialogPagosIFrame.aspx?params=" + table + "";
            urlM2 = "AMS.Web.ModalDialog.aspx?params=" + table + "&proced=pagos";
        }

        //    param = OpenModal(urlM, "AMS Seleccion", modalDialogWidth, modalDialogHeight);
        //    if (!param)
        //        param = window.document.getElementById('HDNINPT_MODALSELECT').value;

        $("#dialog").html('<iframe src="' + urlM2 + '" height="530" widht="580" frameborder="0" ></iframe>' +
                        '<form name="formName"><input type="hidden" value="' + obj.id + '" name="objeton" id="objetoi"/>' +
                        '<input type="hidden" value="' + oIns + '" name="funcion" id="funciones"/>' +
                        '<input type="hidden" value="' + campo + '" name="campo" id="campos"/>' +
                        '<input type="hidden" value="' + numeros + '" name="numero" id="numeros"/></form>');
        $("#dialog").css({ 'visibility': 'visible' });
        $("#dialog").dialog({ modal: true, title: 'Cuadro Dialogo', height: 590, width: 580 });
        $("#dialogI").html = obj;
    }
    else {
        alert("Inconsistencia en consulta. Por favor informar a Sistemas ECAS. SQL: " + table);
    }
}

//Carga ModalDialog de EXFactura
function ModalDialogEX(obj, table, controls, oIns, inf) {
    table = table.toUpperCase();
    if (table.indexOf(" START") == -1 && table.indexOf(" STOP") == -1 && table.indexOf("DATABASE ") == -1 && table.indexOf("TABLE ") == -1 && table.indexOf("INSERT ") == -1 && table.indexOf("UPDATE ") == -1 && table.indexOf("DELETE ") == -1 && table.indexOf("DROP ") == -1 && table.indexOf("ALTER ") == -1 && table.indexOf("TRUNCATE ") == -1) {
        var modalDialogHeight = "500";
        var modalDialogWidth = "500";
        var nomAux = obj.name + "EX";
        var splitTable = table.split('+');
        var table2 = "";
        var control = 0;
        var splitId = obj.id.split('_');
        if (document.all[String("_ctl1_" + splitId[splitId.length - 1] + "EX")] != null) {
            if (document.all[String("_ctl1_" + splitId[splitId.length - 1] + "EX")].value != null) {
                table2 = splitTable[0] + document.all[String("_ctl1_" + splitId[splitId.length - 1] + "EX")].value + splitTable[1];
                control = 1;
            }
            else {
                alert("No se ha seleccionado un valor previo correspondiente al " + inf);
                control = 2;
            }
        }
        if (control = 1) {
            var control;
            var splitId;
            var newId;
            var param;
            var urlM = "";
            var urlM2 = "";

            if (oIns != null) {
                urlM = "AMS.Web.ModalDialogIFrame.aspx?params=" + table2 + "&Ins=1";
                urlM2 = "AMS.Web.ModalDialog.aspx?params=" + table2 + "&Ins=1";
            }
            else {
                urlM = "AMS.Web.ModalDialogIFrame.aspx?params=" + table2 + "";
                urlM2 = "AMS.Web.ModalDialog.aspx?params=" + table2 + "";
            }

            //param = OpenModal(urlM, "AMS Seleccion", modalDialogWidth, modalDialogHeight);

            $("#dialog").html('<iframe src="' + urlM2 + '" height="530" widht="580" frameborder="0" ></iframe>' +
                        '<form name="formName"><input type="hidden" value="' + obj.id + '" name="objeton" id="objetoi"/>' +
                        '<input type="hidden" value="' + oIns + '" name="funcion" id="funciones"/></form>');
            $("#dialog").css({ 'visibility': 'visible' });
            $("#dialog").dialog({ modal: true, title: 'Cuadro Dialogo', height: 590, width: 580 });
            $("#dialogI").html = obj;
        }
        else
            alert("No se ha seleccionado un valor previo correspondiente al " + inf);
    }
    else {
        alert("Inconsistencia en consulta. Por favor informar a Sistemas ECAS. SQL: " + table);
    }
}

//Activación del evento onClick sobre el ModalDialog.
function terminarDialogo(param, proced) {
    var idObj = window.document.getElementById('objetoi').value;
    var objS = window.document.getElementById(idObj);
    var funciones = window.document.getElementById('funciones').value;
    
    if (proced == "pagos") {
        var campo = window.document.getElementById('campos').value;
        var numeros = window.document.getElementById('numeros').value;
        proceedPagos(objS, param, campo, numeros);
    }
    else {
        proceed(objS, param);
    }
    $("#dialog").dialog("close");

    if (funciones.length > 9) {
        window[funciones]();
    }

}

//Ubicar en pantalla los parametros obtenidos del ModalDialog.
function proceed(obj, param) {
   if(!param)
       param = window.document.getElementById('HDNINPT_MODALSELECT').value;

   if (param != null ) {
       var splitVals = param.split(',');
      obj.value = splitVals[0];

      if (obj.baseURI != undefined && obj.baseURI.indexOf("DBManager") != -1)
          return true;

      if (asociarObjetos == null) {
          var aux = obj.name + "a";
          if (document.all[aux] != null) {
              document.all[aux].value = splitVals[1];
          }
          else if (document.all[obj.name + "1"] != null)
              document.all[obj.name + "1"].value = splitVals[1];

          var aux2 = obj.name + "b";
          if (document.all[aux2] != null) {
              var valAnterior = document.all[aux2].value;
              document.all[aux2].value = splitVals[2];
              if (splitVals[2] == null || splitVals[2] == "&nbsp;")
                  document.all[aux2].value = valAnterior;
          }
          else if (document.all[obj.name + "2"] != null && splitVals[2] != undefined)
              document.all[obj.name + "2"].value = splitVals[2];

          var aux3 = obj.name + "c";
          if (document.all[aux3] != null) {
              var valAnterior = document.all[aux3].value;
              document.all[aux3].value = splitVals[3];
              if (splitVals[3] == null || splitVals[3] == "&nbsp;")
                  document.all[aux3].value = valAnterior;
          }
          var aux4 = obj.name + "d";
          if (document.all[aux4] != null) {
              var valAnterior = document.all[aux4].value;
              document.all[aux4].value = splitVals[4];
              if (splitVals[4] == null || splitVals[4] == "&nbsp;")
                  document.all[aux4].value = valAnterior;
          }
          var aux5 = obj.name + "e";
          if (document.all[aux5] != null) {
              var valAnterior = document.all[aux5].value;
              document.all[aux5].value = splitVals[5];
              if (splitVals[5] == null || splitVals[5] == "&nbsp;")
                  document.all[aux5].value = valAnterior;
          }
          var aux6 = obj.name + "f";
          if (document.all[aux6] != null) {
              var valAnterior = document.all[aux6].value;
              document.all[aux6].value = splitVals[6];
              if (splitVals[6] == null || splitVals[6] == "&nbsp;")
                  document.all[aux6].value = valAnterior;
          }
          var aux7 = obj.name + "g";
          if (document.all[aux7] != null) {
              var valAnterior = document.all[aux7].value;
              document.all[aux7].value = splitVals[7];
              if (splitVals[7] == null || splitVals[7] == "&nbsp;")
                  document.all[aux7].value = "";
              //document.all[aux7].value = valAnterior;
          }
          var aux8 = obj.name + "h";
          if (document.all[aux8] != null) {
              var valAnterior = document.all[aux8].value;
              document.all[aux8].value = splitVals[8];
              if (splitVals[8] == null || splitVals[8] == "&nbsp;")
                  document.all[aux8].value = valAnterior;
          }
      }
      else {
          asociarObjetos = null;
      }
      obj.focus();
      $("#" + obj.id).trigger("change");
    }
    return true;
}

//Ubicar en pantalla los parametros obtenidos del ModalDialogAT.
function proceedAT(obj, param) {
    if (!param)
        param = window.document.getElementById('HDNINPT_MODALSELECT').value;

    if (param != null) {
        var splitVals = param.split(',');
        obj.value = splitVals[0];
        var aux = obj.name + "a";
        if (document.all[aux] != null)
            document.all[aux].value = splitVals[1];
        var aux2 = obj.name + "b";
        if (document.all[aux2] != null)
            document.all[aux2].value = splitVals[2];
        var aux3 = obj.name + "c";
        if (document.all[aux3] != null)
            document.all[aux3].value = splitVals[3];
        var aux4 = obj.name + "d";
        if (document.all[aux4] != null)
            document.all[aux4].value = splitVals[4];
        var aux5 = obj.name + "e";
        if (document.all[aux5] != null)
            document.all[aux5].value = splitVals[5];
        var aux6 = obj.name + "f";
        if (document.all[aux6] != null)
            document.all[aux6].value = splitVals[6];
        var aux7 = obj.name + "g";
        if (document.all[aux7] != null)
            document.all[aux7].value = splitVals[7];
        var aux8 = obj.name + "h";
        if (document.all[aux8] != null)
            document.all[aux8].value = splitVals[8];

        if (controls.length != null && controls.length != 0) {
            splitId = obj.id.split('_');
            for (i = 0; i < controls.length; i++) {
                var str = controls[i];
                if (str.substr(0, 1) != "#")
                    newId = obj.id.replace(splitId[5], controls[i]);
                else
                    newId = str.substr(1, str.length);
                //alert(splitId[5]+":::"+controls[i]);
                control = document.getElementById(newId);
                control.value = splitVals[i + 1];
                control.click();
            }
        }
    }
    else
        alert("No se ha seleccionado un valor previo correspondiente al " + inf);
}

//Ubicar en pantalla los parametros obtenidos del ModalDialogPagos.
function proceedPagos(obj, param, campo, numeros) {
    if (!param)
        param = window.document.getElementById('HDNINPT_MODALSELECT').value;
    var arregloObjetosMod = null;
    if (numeros != null) {
        var objetoMod;
        var spMod = param.split(',');
        arregloObjetosMod = numeros.split(',');
        if (arregloObjetosMod.length > 1) {
            for (i = 0; i < spMod.length-1; i++) {
                objetoMod = arregloObjetosMod[i];
                document.getElementById(objetoMod).value = spMod[i + 1];
                if(i == 1) {
                    var mydate = new Date(spMod[i + 1]);
                    var yyyy = mydate.getFullYear().toString();
                    var mm = (mydate.getMonth() + 1).toString(); // getMonth() is zero-based         
                    var dd = mydate.getDate().toString();

                    document.getElementById(objetoMod).value = yyyy + '-' + (mm[1] ? mm : "0" + mm[0]) + '-' + (dd[1] ? dd : "0" + dd[0]);
                }
            }
        }
    }
    //Si la consulta devuelve algo
    if (param != null) {
        var sp = param.split(',');
        var objeto;
        //OJO algun dia toca cambiar esos document.all por document.getElementById()
        //pa lo de los estandares XD
        //Divido el resultado y empiezo a asignarlo a los distintoas objetos
        for (i = 0; i < sp.length; i++) {
            //Si es el primer elemento del arreglo se lo asigno al objeto por defecto
            if (i == 0) {
                objeto = obj.name;
                if (document.all[objeto] != null)
                    document.all[objeto].value = sp[i];
            }
            //Si es otra posicion
            else {
                //Si es un pago con chequera (o sea un comp de egreso con cheque)
                if (numeros != null) {
                    //Si es la penultima posicion, reviso que la chequera maneje
                    //consecutivo y le sumo al valor devuelto, sino maneja consecutivo
                    //solo asigno un valor vacio
                    if (i == sp.length - 2) {
                        objeto = obj.name + String(i);
                        if (sp[sp.length - 1] == 'S') {
                            var mival = parseInt(sp[i]) + 1;
                            if (document.all[objeto] != null)
                                document.all[objeto].value = mival;
                        }
                        else {
                            if (document.all[objeto] != null)
                                document.all[objeto].value = '';
                        }
                    }
                    //Sino solo asigno el valor devuelto
                    else {
                        objeto = obj.name + String(i);
                        if (document.all[objeto] != null)
                            document.all[objeto].value = sp[i];
                    }
                }
                //Si no es un pago con chequera, sino una transferencia bancaria
                //hago el proceso normal
                else {
                    objeto = obj.name + String(i);
                    if (document.all[objeto] != null)
                        document.all[objeto].value = sp[i];
                }
            }
        }
        //Si especifico el campo del numero del documento, miro si lo dejo de solo
        //lectura (chequera con consecutivo) o normal (chequera sin consecutivo)
        if (campo != null) {
            if (sp[sp.length - 1] == 'S')
                campo.readOnly = true;
            else
                campo.readOnly = false;
        }
    }
}

//Ubicar en pantalla los parametros obtenidos del ModalDialogEX.
function proceedEX(obj, param) {
    if (!param)
        param = window.document.getElementById('HDNINPT_MODALSELECT').value;
    if (param != null) {
        var splitVals = param.split(',');
        obj.value = splitVals[0];
        var aux = obj.name + "a";
        if (document.all[aux] != null)
            document.all[aux].value = splitVals[1];
        var aux2 = obj.name + "b";
        if (document.all[aux2] != null)
            document.all[aux2].value = splitVals[2];
        var aux3 = obj.name + "c";
        if (document.all[aux3] != null)
            document.all[aux3].value = splitVals[3];
        var aux4 = obj.name + "d";
        if (document.all[aux4] != null)
            document.all[aux4].value = splitVals[4];
        var aux5 = obj.name + "e";
        if (document.all[aux5] != null)
            document.all[aux5].value = splitVals[5];
        var aux6 = obj.name + "f";
        if (document.all[aux6] != null)
            document.all[aux6].value = splitVals[6];
        var aux7 = obj.name + "g";
        if (document.all[aux7] != null)
            document.all[aux7].value = splitVals[7];
        var aux8 = obj.name + "h";
        if (document.all[aux8] != null)
            document.all[aux8].value = splitVals[8];
        if (controls.length != null && controls.length != 0) {
            splitId = obj.id.split('_');
            for (i = 0; i < controls.length; i++) {
                var str = controls[i];
                if (str.substr(0, 1) != "#")
                    newId = obj.id.replace(splitId[5], controls[i]);
                else
                    newId = str.substr(1, str.length);
                //alert(splitId[5]+":::"+controls[i]);
                control = document.getElementById(newId);
                control.value = splitVals[i + 1];
                control.click();
            }
        }
    }
}

function Cargar()
{
	var tip=location.href.substring((location.href.indexOf("?"))+1,location.href.indexOf("="));
    if(tip=="params")
    {	
    	var params = location.href.substring(location.href.indexOf("params"),location.href.length);
     	document.all.iFrameModalDialogPagos.src="AMS.Web.ModalDialogPagos.aspx?" + params;
    }
}

function Cargar_Datos()
{
	var tip=location.href.substring((location.href.indexOf("?"))+1,location.href.indexOf("="));
    if(tip=="tabla")
    {	
    	var params = location.href.substring(location.href.indexOf("tabla"),location.href.length);
     	document.all.iFrameModalDialogAyuda.src="AMS.Web.ModalDialogAyuda.aspx?" + params;
    }
}

function ModalDialogAyuda(tabla,campo) {
    var table = tabla.toUpperCase();
    if (table.indexOf(" START") == -1 && table.indexOf(" STOP") == -1 && table.indexOf("DATABASE ") == -1 && table.indexOf("TABLE ") == -1 && table.indexOf("INSERT ") == -1 && table.indexOf("UPDATE ") == -1 && table.indexOf("DELETE ") == -1 && table.indexOf("DROP ") == -1 && table.indexOf("ALTER ") == -1 && table.indexOf("TRUNCATE ") == -1) {
        var modalDialogHeight = "400";
        var modalDialogWidth = "330";
        var param;
        var urlM;
        if (tabla != null && campo != null) {
            urlM = "AMS.Web.ModalDialogAyudaIFrame.aspx?tabla=" + tabla + "&campo=" + campo + "";
            param = OpenModal(urlM, "AMS Seleccion", modalDialogWidth, modalDialogHeight);
        }
        else {
            alert("No hay tabla o no hay campo");
            return;
        }
    }
    else {
        alert("Inconsistencia en consulta. Por favor informar a Sistemas ECAS. SQL: " + table);
    }
}

function ModalDialogObj(obj, table, controls, oIns) {

   table = table.toUpperCase();
   if (table.indexOf(" START") == -1 && table.indexOf(" STOP") == -1 && table.indexOf("DATABASE ") == -1 && table.indexOf("TABLE ") == -1 && table.indexOf("INSERT ") == -1 && table.indexOf("UPDATE ") == -1 && table.indexOf("DELETE ") == -1 && table.indexOf("DROP ") == -1 && table.indexOf("ALTER ") == -1 && table.indexOf("TRUNCATE ") == -1) {

       var modalDialogHeight = "500";
       var modalDialogWidth = "500";

       var control;
       var splitId;
       var newId;
       var param;
       var urlM;
       if (oIns != null)
           urlM = "AMS.Web.ModalDialogIFrame.aspx?params=" + table + "&Ins=1";
       else
           urlM = "AMS.Web.ModalDialogIFrame.aspx?params=" + table + "";
       param = OpenModal(urlM, "AMS Seleccion", modalDialogWidth, modalDialogHeight);
       if (!param)
           param = window.document.getElementById('HDNINPT_MODALSELECT').value;
       if (param != null) {
           var splitVals = param.split(',');
           obj.value = splitVals[0];
           if (controls.length != null && controls.length != 0) {
               splitId = obj.id.split('_');
               for (i = 0; i < controls.length; i++) {
                   var str = controls[i];
                   if (str.substr(0, 1) != "#")
                       newId = obj.id.replace(splitId[5], controls[i]);
                   else
                       newId = str.substr(1, str.length);
                   //alert(splitId[5]+":::"+controls[i]);
                   control = document.getElementById(newId);
                   control.value = splitVals[i + 1];
                   control.click();
               }

           }
       }
   }
   else {
       alert("Inconsistencia en consulta. Por favor informar a Sistemas ECAS. SQL: " + table);
   }
}

function Cargar_IFrame()
{
	var params = location.href.substring(location.href.indexOf("process"),location.href.length);
    var ifr=document.getElementById("frm");
    ifr.src="AMS.Web.ModalConfiguracion.aspx?" + params;
}

function ModalConf()
{
	var wd="600";
	var hg="600";
	var urlM="AMS.Web.ModalConfiguracion.aspx?process=Tools.ConfiguracionInicial&mod=1";
	param = OpenModal(urlM,"AMS Seleccion",wd,hg);
}

function GetValueA(param)
{
    window.returnValue = param;
    window.close();
}
function GetValue(param)
{
     if(window.parent.opener){
        window.parent.opener.document.getElementById('HDNINPT_MODALSELECT').value=param;
        window.parent.close();
    }
    else{
        window.returnValue=param;
        window.close();
    }
}

function ModalDialogCont(tamano, titulo, valor, sql) 
{
    var tamX = 0;
    var tamY = 0;
    var urlCont = 'AMS.Web.ModalDialogCont.aspx';

    valor = document.getElementById(valor).value;
    sql = sql.replace(/[$]/g, valor);
    urlCont += '?sql=' + sql;
    switch (tamano) {
        case 'P':
            tamX = 400;
            tamY = 320;
            break;
        case 'M':
            tamX = 550;
            tamY = 320;
            break;
        case 'A':
            tamX = 700;
            tamY = 500;
            break;
        default:
            return;
    }

    $("#modalCont").html('<iframe src="' + urlCont + '" height="' + (tamY-50) + '" widht="' + tamX + '" frameborder="0" ></iframe>' +
                        '<form name="formName"><input type="hidden" value="NN" name="p1" id="p1"/></form>');
    $("#modalCont").css({ 'visibility': 'visible' });
    $("#modalCont").dialog({ modal: true, title: titulo, height: tamY, width: tamX });
    $(".ui-widget-overlay").css({ background: "#000", opacity: 0.2 });
}
