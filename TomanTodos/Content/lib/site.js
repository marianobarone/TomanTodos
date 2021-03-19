
function notification(status, cantidad, producto, sucursal, movimiento) {
    MsgPop.closeAll();
    MsgPop.displaySmall = true;

    if (status) {
        if (movimiento == "Adicion") {
            MsgPop.open({
                Type: "success",
                Content: "Se agregaron " + cantidad + " " + producto + " a la sucursal " + sucursal,
            });
        }
        else {
            MsgPop.open({
                Type: "success",
                Content: "Se descontaron " + cantidad + " " + producto + " de la sucursal " + sucursal,
            });
        }
    }
    else {
        MsgPop.open({
            Type: "error",
            Content: "Error! No se pudo actualizar el stock de " + producto,
        });
    }

}


$('[data-toggle="tooltip"]').tooltip()

//$('.count').each(function () {
//    $(this).prop('Counter', 0).animate({
//        Counter: $(this).text()
//    }, {
//        duration: 4000,
//        easing: 'swing',
//        step: function (now) {
//            $(this).text(Math.ceil(now));
//        }
//    });
//});

$('.easy-pie-chart').easyPieChart({
    //size: 100,
    //lineWidth: 6,
    //lineCap: "square",
    //barColor: "#22a7f0",
    //trackColor: "#ffffff",
    //scaleColor: !1,
    //easing: 'easeOutBounce',
    //animate: 5000,
    //onStep: function (from, to, percent) {
    //    $(this.el).find('.percent').text(Math.round(percent));
    //}
    animate: 2000,
    scaleColor: false,
    lineWidth: 12,
    lineCap: 'square',
    size: 100,
    trackColor: '#e5e5e5',
    barColor: '#3da0ea'
});

$('#prueba').DataTable({
    "paging": true,
    "searching": true,
    "info": false,
    "fixedHeader": true,
    "order": [],

    "aoColumnDefs": [

        { "orderable": false, "targets": 0 },
        { "orderable": false, "targets": 4 },
        { "orderable": false, "targets": 5 },
        { "orderable": false, "targets": 6 },

    ],

    "dom": "<'row'<'col-md-12 mt-3 pr-1' f>>" +
        "<'row'<'col-sm-12' tr>>" +
        "<'row'<'col-md-12 d-flex justify-content-center mt-3' p>>",

    //"dom": "<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'f>>" +
    //    "<'row'<'col-sm-12'tr>>" +
    //    "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",

    "scrollX": true,
    "scrollY": "400px",
    "scrollCollapse": true,

    "language": {
        "search": "_INPUT_",
        "search": '<div class="has-search"><span class="fa fa-search form-control-feedback"></span>',
        "searchPlaceholder": "Buscar...",
        "paginate": {
            "next": '<i class="fa fa-arrow-right">',
            "previous": '<i class="fa fa-arrow-left">'
        },
        "zeroRecords": "Lo lamento no hay pakis resultados",
        //"zeroRecords": "Nothing found - sorry",
    }
});

$('#tabla-movimientos').DataTable({
    "paging": true,
    "searching": true,
    "info": false,
    "fixedHeader": true,
    "order": [],

    "aoColumnDefs": [
        { "orderable": false, "targets": 0 },
    ],

    "dom": "<'row'<'col-md-6 mt-3 pr-1' l><'col-md-6 mt-3 pr-1' f>>" +
        "<'row'<'col-sm-12' tr>>" +
        "<'row'<'col-md-12 d-flex justify-content-center mt-3' p>>",

    "scrollX": true,
    "scrollY": "485px",
    "scrollCollapse": true,

    "language": {
        "search": "_INPUT_",
        "search": '<div class="has-search"><span class="fa fa-search form-control-feedback"></span>',
        "searchPlaceholder": "Buscar...",
        "paginate": {
            "next": '<i class="fa fa-arrow-right">',
            "previous": '<i class="fa fa-arrow-left">'
        },
    }
});

$('#index-table').DataTable({
    "paging": false,
    "searching": false,
    "info": false,
    "fixedHeader": true,
    "ordering": false,

    "scrollX": true,
    "scrollY": "183px",
    "scrollCollapse": true,

    //"language": {
    //    "search": "_INPUT_",
    //    "search": '<div class="has-search"><span class="fa fa-search form-control-feedback"></span>',
    //    "searchPlaceholder": "Buscar...",
    //    "paginate": {
    //        "next": '<i class="fa fa-arrow-right">',
    //        "previous": '<i class="fa fa-arrow-left">'
    //    },
    //}
});


$("#productoId").change(function () {
    var id = $('#productoId').val();

    //window.location = "/StockItems/IntercambiarProductos/?Id=" + productoId;

    //var url = '/StockItems/IntercambiarProductos/?Id=%' + id;
    //var url = '/StockItems/IntercambiarProductos?Id=%2052951ab0-46a7-4859-830a-1726eba5ae1c';
    //var url = '/StockItems/IntercambiarProductos/{0}'.replace('{0}', id);
    window.location.href = '/StockItems/IntercambiarProductos/{0}'.replace('{0}', id);;

    //window.location.href = '/StockItems/IntercambiarProductos/?Id=%' + id
      
    //$.ajax({
    //    url: '/StockItems/IntercambiarProductos',
    //    dataType: "json",
    //    type: "GET",
    //    cache: false,
    //    data: { productoId },
    //    success: function (data) {
    //        alert("anda");
    //        window.location.href = '/StockItems/IntercambiarProductos' 
    //    },
    //    error: function (xhr) {
    //        alert(xhr.statusText);
    //    }
    //});
});

$('#intercambiar').on('click', function () {

    var productoId = $('#productoId').val();
    var sucursalOrigenId = $('#sucursalOrigenId').val();
    var sucursalDestinoId = $('#sucursalDestinoId').val();
    var cantidad = $('#cantIntercambiar').val();
    if (sucursalOrigenId == sucursalDestinoId || cantidad == "") {
        //alert("Las Sucursales no pueden ser las mismas");
        alert("ERROR! Revise los datos ingresados. Las sucursales no pueden ser las mismas y la cantidad no puede estar vacia");
    }

    else {
        //var ob =
        //{
        //    p: productoId,
        //    so: sucursalOrigenId,
        //    sd: sucursalDestinoId,
        //};

        $.ajax({
            url: '/StockItems/IntercambiarProductos',
            dataType: "json",
            type: "POST",
            cache: false,
            data: { productoId, sucursalOrigenId, sucursalDestinoId, cantidad },
            success: function (data) {
                MsgPop.closeAll();
                MsgPop.displaySmall = true;
                MsgPop.open({
                    Type: "success",
                    Content: "Se intercambiaron los productos entre sucursales"
                });
            },
            error: function (xhr) {
                //alert(xhr.responseText);
                MsgPop.closeAll();
                MsgPop.displaySmall = true;
                MsgPop.open({
                    Type: "error",
                    Content: "No se pudo realizar la transaccion solicitada"
                });
            }
        });
    }
    


    //$(function () {

    //    $('.easy-pie-chart').easyPieChart({
    //        //size: 100,
    //        //lineWidth: 6,
    //        //lineCap: "square",
    //        //barColor: "#22a7f0",
    //        //trackColor: "#ffffff",
    //        //scaleColor: !1,
    //        //easing: 'easeOutBounce',
    //        //animate: 5000,
    //        //onStep: function (from, to, percent) {
    //        //    $(this.el).find('.percent').text(Math.round(percent));
    //        //}
    //        animate: 2000,
    //        scaleColor: false,
    //        lineWidth: 12,
    //        lineCap: 'square',
    //        size: 100,
    //        trackColor: '#e5e5e5',
    //        barColor: '#3da0ea'
    //    });
    //});

    //$(function () {
    //    $('.easy-pie-chart')
    //        .each(function () {
    //            $(this).attr('data-percent', pxDemo.getRandomData(100, 0));
    //        })
    //        .easyPieChart({
    //            animate: 1000,
    //            size: 120,
    //            onStep: function (_from, _to, currentValue) {
    //                $(this.el).find('> span').text(Math.round(currentValue) + '%');
    //            },
    //        });
    //});


    //$('#intercambiar').on('click', function () {


    //    //$.ajax({
    //    //    type: "POST",                                              // tipo de request que estamos generando
    //    //    url: "/Productos/ActualizarStock",                    // URL al que vamos a hacer el pedido
    //    //    data: 10,                                                // data es un arreglo JSON que contiene los parámetros que 
    //    //    // van a ser recibidos por la función del servidor
    //    //    contentType: "application/json; charset=utf-8",            // tipo de contenido
    //    //    dataType: "json",                                          // formato de transmición de datos
    //    //    async: true,                                               // si es asincrónico o no
    //    //    success: function (resultado) {                            // función que va a ejecutar si el pedido fue exitoso
    //    //        alert("anda");
    //    //    },
    //    //    error: function (XMLHttpRequest, textStatus, errorThrown) { // función que va a ejecutar si hubo algún tipo de error en el pedido
    //    //        alert("errorThrown");
    //    //    }
    //    //});
    //    ///Productos/Edit / 52951ab0 - 46a7 - 4859 - 830a - 1726eba5ae1c
    //    //var productoId = $('#productoId').val();
    //    //var sucursalOrigenId = $('#sucursalOrigenId').val();
    //    //var sucursalDestinoId = $('#sucursalDestinoId').val();

    //    //var ob =
    //    //{
    //    //    p: productoId,
    //    //    so: sucursalOrigenId,
    //    //    sd: sucursalDestinoId,
    //    //};

    //    var p = $('#productoId').val();
    //    var so = $('#sucursalOrigenId').val();
    //    var sd = $('#sucursalDestinoId').val();
    //    $.ajax({
    //        url: '/Productos/Prueba',
    //        dataType: "json",
    //        type: "POST",
    //        cache: false,
    //        data: { p, so, sd },
    //        success: function (data) {
    //            if (data.success) {
    //                alert(data.message);
    //            }
    //        },
    //        error: function (xhr) {
    //            alert(xhr.responseText);
    //        }
    //    });

        //var ob =
        //{
        //    productoId: "uno",
        //    sucursalID: "dos",
        //    cantidad: "tres",
        //};
        //$.ajax({
        //    type: "POST",
        //    url: "/Productos/Prueba",
        //    data: {ob},
        //    //contentType: "application/json; charset=utf-8",
        //    dataType: "json",
        //    success: function (data) {
        //        alert(data.d);
        //    },
        //    error: function (data) {
        //        alert("fail");
        //    }
        //});

        //var ob =
        //{
        //    productoId: $('#id_1').val(),
        //    sucursalID: $('#suc_1').val(),
        //    cantidad: $('#numero_1').val(),
        //};

        //$.ajax({

        //    url: '/Productos/Prueba',
        //    method: "POST",
        //    data: { cant: 10 },
        //    //data: ob,
        //    success: function (data) {
        //        console.log(data);
        //    },
        //    error: function (data) {
        //        alert("fail");
        //    }
        //});
    });
