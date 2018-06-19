  $(document).ready(transformar);

    function transformar() {
        var data = [
	    ['Mecanico', 'Operario'],
	    ['tecnico', 'Operario'],
        ['Placa', 'Obra (Codigo de la Obra)'],
	    ['Color', 'Interventoría'],
        ['vin', '# Contrato'],
        ['Kilometraje', '% Avance'],
        ['Taller', 'Centro de Obra'],
        ['Taller:', 'Centro de Obra'],
        ['Entrada', 'id (identificacion) Obra'],
        ['Locker', 'Seccion / Etapa'],
        ['vinFecha entrada', 'Fecha Inicio'],
        ['Recepcionista', 'Director Obra'],
        ['Vehiculo', 'Obra'],
        ['automovil', 'Obra'],
        ['Catálogo Modelo', 'Tipo de Obra'],
        ['Motor', 'Poliza'],
        ['Año Modelo', 'Año Obra'],
        ['Código del Radio', 'Clave de Alarma'],
        ['Accesorios', 'Inventario de Elementos'],
        ['Vehiculo ha sido revisado en elevador', 'Requiere Permisos y Licencias '],
        ['Kits o Combos Disponibles', 'Fases de la Obra'],
        ['REPUESTO', 'Material'],
        ['CODIGO DEL KIT', 'Fase Obra'],
        ['MECANICO', 'ElementosOperario']
	];
        var htmlText = document.body.innerHTML ;
        for (var i = 0; i < data.length; i++) {
            var buscar1 = new RegExp(">" + data[i][0] + "<", "g");
            var buscar2 = new RegExp(">" + data[i][0], "g");
            var buscar3 = new RegExp(data[i][0] + "<", "g");
            var buscar4 = new RegExp( data[i][0] + " ", "g");
            var buscar5 = new RegExp( " " + data[i][0] , "g");

            htmlText = htmlText.replace(buscar1, '>' + data[i][1] + '<');
            htmlText = htmlText.replace(buscar2, '>' + data[i][1]);
            htmlText = htmlText.replace(buscar3, data[i][1] + '<');
            htmlText = htmlText.replace(buscar4, data[i][1] + ' ');
            htmlText = htmlText.replace(buscar5, ' ' + data[i][1]);

        }
            $("body").html(String(htmlText));
        return true;
    }
