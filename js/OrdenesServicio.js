function cargarDataClienteVehiculoOS(_serie) {
    $('#loadingModal').modal('show');
    document.getElementById('btnCrearCliente').setAttribute('disabled', '');
    var elemento = $(_serie).val();
    $('#osSerie').text('');
    $('#osMarca').text('');
    $('#osModelo').text('');
    $('#osAnio').text('');
    $('#osDescripcion').text('');
    $('#osColor').text('');
    $('#osMotor').text('');

    $('#listaClientes').html('');
    $.ajax(
        {
            type: 'POST',
            url: 'ordenesServicio/GetDatosVehiculoSAP',
            data: { _serie: elemento },//URL del action result que cargara la vista parcial
            success: function (result) {
                $("#loadingModal").removeClass('show');//ocultamos el modal
                $('body').removeClass('modal-open');//eliminamos la clase del body para poder hacer scroll
                $('.modal-backdrop').remove();//eliminamos el backdrop del modal
                console.log(result);
                if (result.titulo != null) {
                    if (result.titulo == "Error") {
                        notificacioError(result.titulo, result.desc);
                    }
                    if (result.titulo == "Cuidado") {
                        notificacionAlerta(result.titulo, result.desc);
                    }
                }
                if (result != null) {

                    document.getElementById('btnCrearCliente').removeAttribute('disabled');
                    var tp = result;
                    $('#osSerie').text(tp.vehiculo.chasis);
                    $('#osMarca').text(tp.vehiculo.marca);
                    $('#osModelo').text(tp.vehiculo.modelo);
                    $('#osAnio').text(tp.vehiculo.year);
                    $('#osDescripcion').text(tp.vehiculo.descripcion);
                    $('#osColor').text(tp.vehiculo.color);
                    $('#osMotor').text(tp.vehiculo.motor);

                    var contador = 0;
                    tp.clientes.forEach(function (item, indice, array) {
                        contador++;
                        var existe = false;
                        tp.clientesBlackLists.forEach(function (itemB, indice, array) {
                            if (item.cardCode == itemB.cardCode) {
                                existe = true;
                            }
                        });
                        var tr = $('<tr></tr>');
                        var tdOption = $('<td></td>');
                        var labelOption = $('<label class="custom-control custom-radio"></label >');
                        var disabledInput = "";
                        if (existe) {
                            disabledInput = "disabled";
                        }
                        var inputOption = $('<input id="' + item.cardCode + '" value="' + item.cardCode + '" type="radio" name="clienteSelect" class="custom-control-input" ' + disabledInput + ' />');
                        if (contador == 1 && !existe) {
                            inputOption = $('<input id="' + item.cardCode + '" value="' + item.cardCode + '" type="radio" checked = "" name="clienteSelect" class="custom-control-input"/>');
                        }
                        var spanOption = $('<span class="custom-control-label"></span>');
                        labelOption.append(inputOption);
                        labelOption.append(spanOption);
                        tdOption.append(labelOption);
                        var tdCardCode = $('<td class="sorting_1">' + item.cardCode + '</td>');
                        var tdnombre = $('<td>' + item.nombre + '</td>');
                        var tdIdentidad = $('<td>' + item.identidad + '</td>');
                        var tdCelular = $('<td>' + item.celular + '</td>');

                        tr.append(tdOption);
                        tr.append(tdCardCode);
                        tr.append(tdnombre);
                        tr.append(tdIdentidad);
                        tr.append(tdCelular);
                        $('#listaClientes').append(tr);
                    });
                } else {
                    notificacioError('Error', 'Se produjo un error al obtenerdatos del vahiculo');
                }

            },

            error: function (error) {
                $("#loadingModal").removeClass('show');//ocultamos el modal
                $('body').removeClass('modal-open');//eliminamos la clase del body para poder hacer scroll
                $('.modal-backdrop').remove();//eliminamos el backdrop del modal
                // si hay un error lanzara el mensaje de error
                notificacioError('Error', 'Se produjo un error al obtenerdatos del vahiculo');
                console.log(error);

            }
        });
}

function getDetallerOS(id) {

    var os = id;
    $.ajax(
        {
            type: 'POST',
            url: 'ordenesServicio/GetDetalleOrden',
            data: { id: os },
            success: function (result) {
                console.log(result);
                $('#idOs').text(result.idOS);
                $('#serie').text(result.vehiculo.serieVehiculo);
                $('#marca').text(result.vehiculo.marca);
                $('#modelo').text(result.vehiculo.modelo);
                $('#anio').text(result.vehiculo.year);

                $('#nombre').text(result.cliente.primerNombre + ' ' + result.cliente.primerApellido);
                $('#identidad').text(result.cliente.identidad);
                $('#celular').text(result.cliente.celular);

                $('#diagnostico').val(result.diagnostico);
                $('#observaciones').val(result.diagnostico);

                $('#exampleModal').modal('show');



            },
            error: function (error) {
                // si hay un error lanzara el mensaje de error
                notificacioError('Error', 'Se produjo un error al obtenerdatos de la orden de servicio');

            }
        });
}

function getOrdenesServicioAbiertas() {
    $.ajax(
        {
            type: 'POST',
            url: 'ordenesServicio/GetOrdenesServicioAbietas',

            success: function (result) {
                console.log(result);
                $.each(result, function (key, value) {
                    console.log(value);


                    var tr = $('<tr onclick="getDetallerOS(' + value.idOS + ')" id="OS-' + value.idOS + '" data-cd="' + value.idOS + '" class="gradeA odd" role="row"></tr>');
                    var tdNo = $('<td class="sorting_1">' + value.idOS + '</td>');
                    var tdCliente = $('<td>' + value.cliente.primerNombre + ' ' + value.cliente.primerApellido + '</td>');
                    var tdFecha = $('<td>' + value.fecha + '</td>');


                    tr.append(tdNo);
                    tr.append(tdCliente);
                    tr.append(tdFecha);


                    $('#listaOSabiertas').append(tr);
                });

            },
            error: function (error) {
                // si hay un error lanzara el mensaje de error
                notificacioError('Error', 'Se produjo un error al obtenerdatos de las ordenes de servicio');

            }
        });
}

var ssg = false;

function testbtnval() {
    console.log($('input:radio[name=clienteSelect]:checked').val());
}

function crearOrdenServicio() {
    $('#loadingModal').modal('show');
    var data = new FormData();
    var servicioProgramado = false;
    if (validarFormOS()) {
        data.append('serieVehiculo', $('#osSerie').text());
        data.append('cardCode', $('input:radio[name=clienteSelect]:checked').val());
        data.append('kms', $('#kmsTacometro').val());
        data.append('cantidadGas', $('#cantidadComustible').val());
        data.append('descFotoIngreso', obtenerDataTablaId('itemsDetalles'));

        data.append('categoriMantenimiento', $('#cm').val());
        if ($('#cm').val() == 'snp') {

            data.append('idSR', 0);
        }
        else if ($('#cm').val() == 'sp') {
            data.append('idSR', serv);
            servicioProgramado = true;

        } else {
            data.append('idSR', 0);
        }

        var image = document.getElementById("pizarra").toDataURL("image/png");
        image = image.replace('data:image/png;base64,', '');

        data.append('imgTacometro', $('#fileImgTacometro').get(0).files[0]);

        data.append('firmaCliente', image);
        data.append('imgDoc1', $('#fileimgPrewDoc1').get(0).files[0]);
        data.append('imgDoc2', $('#fileimgPrewDoc2').get(0).files[0]);

        if ($('#fileimgPrewDoc3').val() == '') {
            data.append('imgDoc3', '');
        } else {
            data.append('imgDoc3', $('#fileimgPrewDoc3').get(0).files[0]);
        }


        if ($('#fileimgPrewDoc4').val() == '') {
            data.append('imgDoc4', '');
        } else {
            data.append('imgDoc4', $('#fileimgPrewDoc4').get(0).files[0]);
        }

        if ($('#fileimgPrewDoc5').val() == '') {
            data.append('imgDoc5', '');
        } else {
            data.append('imgDoc5', $('#fileimgPrewDoc5').get(0).files[0]);
        }




        data.append('comentarioCliente', obtenerDataTablaId('itemsComentarios'));

        data.append('idUsuario', $('#idUsuario').val());
        data.append('idTaller', $('#talleres').val());


        if ($("input[name='requiereSG']").is(':checked')) {
            data.append('existeSG', 'true');
            var dg = obtenerDataTabla();
            data.append('diagnostico', dg);
            data.append('resolucionProblema', '');
            ssg = true;
        } else {
            data.append('existeSG', 'false');
            data.append('diagnostico', '');
            data.append('resolucionProblema', $('#resolucionProblema').val());
            ssg = false;
        }


        $('#esperaOS').append('<div role="progressbar" style="width: 100%" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" class="progress-bar bg-success progress-bar-striped progress-bar-animated">Enviando Datos</div>');

        $.ajax(
            {
                type: 'POST',
                url: 'ordenesServicio/CrearOrdenServicio',
                contentType: false,
                processData: false,
                data: data,
                success: function (result) {
                    $("#loadingModal").removeClass('show');
                    $('#esperaOS').html('');

                    if (result.titulo == 'Cuidado') {
                        notificacionAlerta(result.titulo, result.desc);
                    }

                    if (result.titulo == 'Ok') {
                        notificacionPrimary(result.titulo, result.desc);
                        $('#listOrdenesServicio').click();
                        if (ssg) {
                            cargarVistaEnvDato('servicioGarantia/Index', 'contenido', 'li#nuevaOS', 'ul#opcionesOS li', 'active', result.idOS);
                            irArriba();
                        } else {
                            if (servicioProgramado) {
                                ssg = false;
                                cargarVistaEnvDato('ordenesServicio/OrdenServicioProgramadaPreview', 'contenido', 'li#nuevaOS', 'ul#opcionesOS li', 'active', result.idOS);
                                irArriba();
                            } else {
                                ssg = false;
                                location.reload();
                            }
                        }
                    }

                },
                error: function (error) {
                    // si hay un error lanzara el mensaje de error
                    $("#loadingModal").removeClass('show');
                    $('#esperaOS').html('');
                    notificacioError('Error', 'Se produjo un error al crear la orden de servicio.');

                }
            });
    } else {
        notificacionAlerta('Campos sin rellenar', 'Para crear la garantía debes de rellenar todos los campos requeridos, se te marcaran en rojo los necesarios.');
    }
}

function obtenerDataTablaId(id) {
    var cadena = '';
    var table = document.getElementById(id);
    for (var i = 0, row; row = table.rows[i]; i++) {
        if (cadena == '') {
            var texto = row.cells[1].innerText;
            cadena = texto.replace(';', ' ');
        } else {
            var texto = row.cells[1].innerText;
            cadena += ' ; ' + texto.replace(';', ' ');
        }
    }
    return cadena;
}

function getServiciosRuedas(ruedas, serie) {
    $.ajax(
        {
            type: 'GET',
            url: 'ordenesServicio/getServiciosVehiculo',
            data: { serie: serie },
            success: function (result) {

                console.log(result);

                if (result != null) {
                    $('#servicios').html('');
                    $.each(result, function (key, value) {

                        var div = $('<div class="col-sm-2"  style="padding: 5px;"></div>');
                        var div2 = $('<div onclick="checkItem(' + value.idServicio + ')" id="DvCh-' + value.idServicio + '" style="border: solid #d7d7d7 1px;padding:5px;background: #f6f6f6;"></div>');
                        if (value.realizado) {
                            console.log('se realizo ' + value.idServicio);
                            div2 = $('<div  id="DvCh-' + value.idServicio + '" style="border: solid #d7d7d7 1px;padding:5px;background: #6edff8;"></div>');
                        }

                        var label = $('<label>');
                        label.addClass('custom-control custom-checkbox custom-control-inline negrita');

                        var inputCbx = $('<input>');
                        if (value.realizado) {
                            inputCbx.attr('checked', '');
                        }
                        inputCbx.addClass('custom-control-input');
                        inputCbx.attr('id', 'Ceck-' + value.idServicio);
                        inputCbx.attr('type', 'checkbox');
                        inputCbx.attr('data-kms', value.kms);
                        inputCbx.attr('data-ant', value.anterior);

                        if (value.realizado) {
                            inputCbx.attr('data-realizado', 'true');
                        }

                        inputCbx.attr('disabled', '');

                        var span = $('<span>');
                        span.addClass('custom-control-label');
                        span.text(value.descripcion);

                        label.append(inputCbx);
                        label.append(span);
                        div2.append(label);
                        div2.append($('<hr/>'));
                        div2.append($('<label class="negrita">' + value.kms + ' Kms</label>'));
                        div2.append('<div onclick="getTrabajoRealizar(' + value.idServicio + ')" class="btn-group btn-space"><button type="button" class="btn btn-xs btn-color btn-social btn-google-plus"><i class="fa fa-eye"></i></button><button type="button" class="btn btn-xs btn-secondary">Ver Detalle</button></div>');
                        div.append(div2);
                        $('#servicios').append(div);
                    });
                    $('#tablaMan').removeClass('d-none');
                    DeshabilitarOS();
                }

            },

            error: function (error) {
                // si hay un error lanzara el mensaje de error
                notificacioError('Error', 'Se produjo un error al obtenerdatos de las ordenes de servicio');

            }
        });
}



function DeshabilitarOS() {
    var kms = $('#kmsTacometro').val();


    $('div#servicios').each(function () {

        var elementos = $('label input', this);

        $.each(elementos, function (key, value) {
            var input = $(value);

            var realizado = input.attr('data-realizado');

            if (realizado === undefined) {

                input.attr('disabled', '');
                input.prop('checked', false);

                var tp = input.attr('id').split('-');
                var id = tp[1];
                $('#DvCh-' + id).attr('style', 'border: solid #d7d7d7 1px;padding:5px;background: #f6f6f6;');
                var dataKms = input.attr('data-kms');



                var kmsAnt = $('#Ceck-' + input.attr('data-ant')).attr('data-kms');

                if ((parseInt(kms) >= (parseInt(kmsAnt) + 100)) && (parseInt(kms) <= (parseInt(dataKms) + 99))) {
                    input.removeAttr("disabled");
                    $('#DvCh-' + id).attr('style', 'border: solid #d7d7d7 1px;padding:5px;background: #f3f890;');
                }

                if (parseInt(dataKms) == kms) {
                    input.removeAttr("disabled");
                    $('#DvCh-' + id).attr('style', 'border: solid #d7d7d7 1px;padding:5px;background: #f3f890;');
                }

            }



        });


    });

}



function obtenerServicioCheck() {

    var id = 0;
    $('div#servicios').each(function () {

        var elementos = $('label input', this);

        $.each(elementos, function (key, value) {
            var input = $(value);

            if (input.is(':checked') && !input.attr('disabled')) {

                id = input.attr('id');

            }

        });
    });
    return id;
}




function validarFormOS() {
    var formCompleto = true;

    if ($('#serieVehiculo').val() == '') {
        $('#serieVehiculo').addClass('input-alerta');
        formCompleto = false;
    }

    if ($('#osSerie').text() == '') {
        $('#serieVehiculo').addClass('input-alerta');
        formCompleto = false;
        notificacionAlerta('Buscar un vehículo.', 'Debes buscar un vehículo primero, por su número de serie para poder crear la orden de servicio');
    }

    if ($('#kmsTacometro').val() == '') {
        $('#kmsTacometro').addClass('input-alerta');
        formCompleto = false;
    }



    if ($('#fileImgTacometro').val() == '') {
        $('#alertaImgTaco').removeClass('d-none');
        formCompleto = false;
    }


    if ($('#fileimgPrewDoc1').val() == '') {
        $('#alertaImgDoc1').removeClass('d-none');
        formCompleto = false;
    }

    if ($('#fileimgPrewDoc2').val() == '') {
        $('#alertaImgDoc2').removeClass('d-none');
        formCompleto = false;
    }

    if ($('#comentarioCliente').val() == '') {
        $('#comentarioCliente').addClass('input-alerta');
        formCompleto = false;
    }

    if ($('#descRecepcion').val() == '') {
        $('#descRecepcion').addClass('input-alerta');
        formCompleto = false;
    }

    if ($('#idCliente').val() == '') {
        formCompleto = false;
    }

    if (!$("input:radio[name=clienteSelect]").is(':checked')) {
        formCompleto = false;
    }

    return formCompleto;
}


function validarFormOS2() {
    var formCompleto = true;


    if ($('#diagnostico').val() == '') {
        $('#diagnostico').addClass('input-alerta');
        formCompleto = false;

    }

    if ($('#observaciones').val() == '') {
        $('#observaciones').addClass('input-alerta');
        formCompleto = false;
    }


    return formCompleto;
}

function cerrarOrden() {

    if (validarFormOS2()) {
        var data = new FormData();
        data.append('idOS', $('#idOs').text());
        data.append('diagnostico', $('#diagnostico').val());
        data.append('recomendaciones', $('#observaciones').val());
        if ($('#solicitudSG').is(':checked')) {
            data.append('existeSG', 'true');
        } else {
            data.append('existeSG', 'false');

        }


        $.ajax(
            {
                type: 'POST',
                url: 'ordenesServicio/updateOrdenServicio',
                contentType: false,
                processData: false,
                data: data,
                success: function (result) {
                    console.log(result);
                    $('#OS-' + $('#idOs').text()).remove();
                    notificacionPrimary(result.titulo, result.desc);
                    $('#exampleModal').modal('hide');



                },
                error: function (error) {
                    // si hay un error lanzara el mensaje de error
                    notificacioError('Error', 'Se produjo un error al cerrar la orden de servicio.');

                }
            });
    } else {
        notificacionAlerta('Campos sin rellenar', 'Para crear la garantía debes de rellenar todos los campos requeridos, se te marcaran en rojo los necesarios.');
    }
}


function cargarServiciosProgramados(id) {
    if ($('#osSerie').text() != '') {

        if ($(id).val().trim() == 'sp') {

            var serie = $('#osSerie').text();
            getServiciosRuedas(2, serie);

        } else {
            $('#tablaMan').addClass('d-none');
            $('#servicios').html('');
        }
    } else {
        $('#tablaMan').addClass('d-none');
        $('#servicios').html('');
        console.log('no serie');
    }
}

var contador = 0;

//detalles del vehiculo
function agregaLineaDetalles() {
    contador = 0;
    if ($('#detalle').val() == '') {
        $('#detalle').focus();
    } else {

        contador += 1;
        var linea = $('#detalle').val();
        var tr = $('<tr></tr>');
        var tdNum = $('<td>' + contador + '</td>');
        tr.attr('id', 'itemDetalle-' + contador);
        var td = $('<td>' + linea + '</td>');
        td.attr('id', 'itemDetalleString-' + contador);
        var tdAction = $('<td><button onclick="eliminarLineaDetalle(' + contador + ')" class="icon btn btn-danger" style="padding: 3px;"><i style="font-size: 1.8rem;" class="s7-trash"></i></button>  <button onclick="editarItemDetalle(' + contador + ')" class="icon btn btn-dark" style="padding: 5px;"><i style="font-size: 1.5rem;" class="far fa-edit"></i></button></td>');
        tr.append(tdNum);
        tr.append(td);
        tr.append(tdAction);
        $('#itemsDetalles').append(tr);
        $('#detalle').val('');
        $('#detalle').focus();
    }
}

function eliminarLineaDetalle(id) {
    $('#confirmaEleminar').addClass('modal-show');
    $('#confimItemDg').attr('onclick', 'elmininaItemDetalle(' + id + ')');
}

function elmininaItemDetalle(id) {
    $('#itemDetalle-' + id).remove();
    cerrarModalConf();
    reordenarTablaDetalle();
}
function reordenarTablaDetalle() {
    var n = 0;
    $('table tbody#itemsDetalles tr').each(function () {
        n += 1;
        $(this).attr('id', 'itemDetalle-' + n);
        $(this).find("td").eq(0).html(n);
        $(this).find("td").eq(2).html('<button onclick="eliminarLineaDetalle(' + n + ')" class="icon btn btn-danger" style="padding: 3px;"><i style="font-size: 1.8rem;" class="s7-trash"></i></button>  <button onclick="editarItemDetalle(' + contador + ')" class="icon btn btn-dark" style="padding: 5px;"><i style="font-size: 1.5rem;" class="far fa-edit"></i></button>');

    });
    contador = n;
}
function editarItemDetalle(id) {
    $('#editConf').addClass('modal-show');
    $('#confirmEdit').attr('onclick', 'editarElementoDetalle(' + id + ')');

}

function editarElementoDetalle(id) {
    cerrarModalCnEdit();
    $('#editItem').modal('show');
    var n = $('#itemDetalleString-' + id).html();
    $('#textEditItem').val(n);
    $('#textEditItem').focus();
    $('#guardarEdir').attr('onclick', 'cambiarValorDetalle(' + id + ')');
}

function cambiarValorDetalle(id) {
    var n = $('#textEditItem').val();
    $('#itemDetalleString-' + id).html(n);
    $('#editItem').modal('hide');
}
//end detalles del vehiculo


//comentarios del cliente
var contadorComentario = 0;
function agregaLineaComentario() {
    if ($('#comentario').val() == '') {
        $('#comentario').focus();
    } else {

        contadorComentario += 1;
        var linea = $('#comentario').val();
        var tr = $('<tr></tr>');
        var tdNum = $('<td>' + contadorComentario + '</td>');
        tr.attr('id', 'itemComentario-' + contadorComentario);
        var td = $('<td>' + linea + '</td>');
        td.attr('id', 'itemComentarioString-' + contadorComentario);
        var tdAction = $('<td><button onclick="eliminarLineaComentario(' + contadorComentario + ')" class="icon btn btn-danger" style="padding: 3px;"><i style="font-size: 1.8rem;" class="s7-trash"></i></button>  <button onclick="editarItemComentario(' + contadorComentario + ')" class="icon btn btn-dark" style="padding: 5px;"><i style="font-size: 1.5rem;" class="far fa-edit"></i></button></td>');
        tr.append(tdNum);
        tr.append(td);
        tr.append(tdAction);
        $('#itemsComentarios').append(tr);
        $('#comentario').val('');
        $('#comentario').focus();
    }
}

function eliminarLineaComentario(id) {
    $('#confirmaEleminar').addClass('modal-show');
    $('#confimItemDg').attr('onclick', 'elmininaItemComentario(' + id + ')');
}

function elmininaItemComentario(id) {
    $('#itemComentario-' + id).remove();
    cerrarModalConf();
    reordenarTablaComentarios();
}
function reordenarTablaComentarios() {
    var n = 0;
    $('table tbody#itemsComentarios tr').each(function () {
        n += 1;
        $(this).attr('id', 'itemComentario-' + n);
        $(this).find("td").eq(0).html(n);
        $(this).find("td").eq(2).html('<button onclick="eliminarLineaComentario(' + n + ')" class="icon btn btn-danger" style="padding: 3px;"><i style="font-size: 1.8rem;" class="s7-trash"></i></button>  <button onclick="editarItemComentario(' + contador + ')" class="icon btn btn-dark" style="padding: 5px;"><i style="font-size: 1.5rem;" class="far fa-edit"></i></button>');

    });
    contador = n;
}
function editarItemComentario(id) {
    $('#editConf').addClass('modal-show');
    $('#confirmEdit').attr('onclick', 'editarElementoComentario(' + id + ')');

}

function editarElementoComentario(id) {
    cerrarModalCnEdit();
    $('#editItem').modal('show');
    var n = $('#itemComentarioString-' + id).html();
    $('#textEditItem').val(n);
    $('#textEditItem').focus();
    $('#guardarEdir').attr('onclick', 'cambiarValorComentario(' + id + ')');
}

function cambiarValorComentario(id) {
    var n = $('#textEditItem').val();
    $('#itemComentarioString-' + id).html(n);
    $('#editItem').modal('hide');
}
//end detalles del vehiculo

//detalles fotos evidencia
var contadorEvidencia = 0;
function agregaLineaDetallesEvidencia() {
    if ($('#detalleEvidencia').val() == '') {
        $('#detalleEvidencia').focus();
    } else {

        contadorEvidencia += 1;
        var linea = $('#detalleEvidencia').val();
        var tr = $('<tr></tr>');
        var tdNum = $('<td>' + contadorEvidencia + '</td>');
        tr.attr('id', 'itemDetalleEvidencia-' + contadorEvidencia);
        var td = $('<td>' + linea + '</td>');
        td.attr('id', 'itemDetalleEvidenciaString-' + contadorEvidencia);
        var tdAction = $('<td><button onclick="eliminarLineaDetalleEvidencia(' + contadorEvidencia + ')" class="icon btn btn-danger" style="padding: 3px;"><i style="font-size: 1.8rem;" class="s7-trash"></i></button>  <button onclick="editarItemDetalleEvidencia(' + contadorEvidencia + ')" class="icon btn btn-dark" style="padding: 5px;"><i style="font-size: 1.5rem;" class="far fa-edit"></i></button></td>');
        tr.append(tdNum);
        tr.append(td);
        tr.append(tdAction);
        $('#itemsDescripcionEvidencia').append(tr);
        $('#detalleEvidencia').val('');
        $('#detalleEvidencia').focus();
    }
}

function eliminarLineaDetalleEvidencia(id) {
    $('#confirmaEleminar').addClass('modal-show');
    $('#confimItemDg').attr('onclick', 'elmininaItemDetalleEvidencia(' + id + ')');
}

function elmininaItemDetalleEvidencia(id) {
    $('#itemDetalleEvidencia-' + id).remove();
    cerrarModalConf();
    reordenarTablaDetalleEvidencia();
}
function reordenarTablaDetalleEvidencia() {
    var n = 0;
    $('table tbody#itemsDescripcionEvidencia tr').each(function () {
        n += 1;
        $(this).attr('id', 'itemDetalleEvidencia-' + n);
        $(this).find("td").eq(0).html(n);
        $(this).find("td").eq(2).html('<button onclick="eliminarLineaDetalleEvidencia(' + n + ')" class="icon btn btn-danger" style="padding: 3px;"><i style="font-size: 1.8rem;" class="s7-trash"></i></button>  <button onclick="editarItemDetalleEvidencia(' + contador + ')" class="icon btn btn-dark" style="padding: 5px;"><i style="font-size: 1.5rem;" class="far fa-edit"></i></button>');

    });
    contador = n;
}
function editarItemDetalleEvidencia(id) {
    $('#editConf').addClass('modal-show');
    $('#confirmEdit').attr('onclick', 'editarElementoDetalleEvidencia(' + id + ')');

}

function editarElementoDetalleEvidencia(id) {
    cerrarModalCnEdit();
    $('#editItem').modal('show');
    var n = $('#itemDetalleEvidenciaString-' + id).html();
    $('#textEditItem').val(n);
    $('#textEditItem').focus();
    $('#guardarEdir').attr('onclick', 'cambiarValorDetalleEvidencia(' + id + ')');
}

function cambiarValorDetalleEvidencia(id) {
    var n = $('#textEditItem').val();
    $('#itemDetalleEvidenciaString-' + id).html(n);
    $('#editItem').modal('hide');
}
//end detalles fotos evidencia

function agregaLineaDiagnostico() {
    if ($('#diagnostico').val() == '') {
        $('#diagnostico').focus();
    } else {

        contador += 1;
        var linea = $('#diagnostico').val();
        var tr = $('<tr></tr>');
        var tdNum = $('<td>' + contador + '</td>');
        tr.attr('id', 'item-' + contador);
        var td = $('<td>' + linea + '</td>');
        td.attr('id', 'itemE-' + contador);
        var tdAction = $('<td><button onclick="eliminarLineaDiagnostico(' + contador + ')" class="icon btn btn-danger" style="padding: 3px;"><i style="font-size: 1.8rem;" class="s7-trash"></i></button>  <button onclick="editarItemDiagnostico(' + contador + ')" class="icon btn btn-dark" style="padding: 5px;"><i style="font-size: 1.5rem;" class="far fa-edit"></i></button></td>');
        tr.append(tdNum);
        tr.append(td);
        tr.append(tdAction);
        $('#itemsDiagnostico').append(tr);
        $('#diagnostico').val('');
        $('#diagnostico').focus();
    }
}

function eliminarLineaDiagnostico(id) {
    $('#confirmaEleminar').addClass('modal-show');
    $('#confimItemDg').attr('onclick', 'elmininaItem(' + id + ')');

}

function cerrarModalConf() {
    $('#confirmaEleminar').removeClass('modal-show');
    $('#confimItemDg').attr('onclick', '');
}

function cerrarModalCnEdit() {
    $('#editConf').removeClass('modal-show');
    $('#confirmEdit').attr('onclick', '');
}


function editarItemDiagnostico(id) {
    $('#editConf').addClass('modal-show');
    $('#confirmEdit').attr('onclick', 'editarElemento(' + id + ')');

}

function editarElemento(id) {
    cerrarModalCnEdit();
    $('#editItem').modal('show');
    var n = $('#itemE-' + id).html();
    $('#textEditItem').val(n);
    $('#textEditItem').focus();
    $('#guardarEdir').attr('onclick', 'cambiarValor(' + id + ')');
}

function elmininaItem(id) {
    $('#item-' + id).remove();
    cerrarModalConf();
    reordenarTabla();
}

function cambiarValor(id) {
    var n = $('#textEditItem').val();
    $('#itemE-' + id).html(n);
    $('#editItem').modal('hide');
}

function reordenarTabla() {
    var n = 0;
    $('table tbody#itemsDiagnostico tr').each(function () {
        n += 1;
        $(this).attr('id', 'item-' + n);
        $(this).find("td").eq(0).html(n);
        $(this).find("td").eq(2).html('<button onclick="eliminarLineaDiagnostico(' + n + ')" class="icon btn btn-danger" style="padding: 3px;"><i style="font-size: 1.8rem;" class="s7-trash"></i></button>  <button onclick="editarItemDiagnostico(' + contador + ')" class="icon btn btn-dark" style="padding: 5px;"><i style="font-size: 1.5rem;" class="far fa-edit"></i></button>');

    });
    contador = n;
}



function obtenerDataTabla() {
    var cadena = '';
    $('table tbody#itemsDiagnostico tr').each(function () {
        if (cadena == '') {
            cadena = $(this).find("td").eq(1).html();
        } else {
            cadena += ';' + $(this).find("td").eq(1).html();
        }

    });
    return cadena;
}


function checkItem(id) {
    if (!$('#Ceck-' + id).attr('disabled')) {

        if ($('#Ceck-' + id).is(':checked')) {
            $('#DvCh-' + id).attr('style', 'border: solid #d7d7d7 1px;padding:5px;background: #f3f890;');
            serv = 0;
            $('#Ceck-' + id).prop('checked', false);

        } else {
            serv = id;
            $('#DvCh-' + id).attr('style', 'border: solid #d7d7d7 1px;padding:5px;background: #54e467;');
            console.log(serv);
            $('#Ceck-' + id).prop('checked', true);
        }
    }

}


function getTrabajoRealizar(id) {
    $.ajax(
        {
            type: 'GET',
            url: 'ordenesServicio/getDetallerTrabajoRealiza',
            data: { id: id },//URL del action result que cargara la vista parcial
            success: function (result) {
                if (result != '') {
                    $('#listTraR').html('');
                    $.each(result, function (key, value) {
                        $('#listTraR').append('<li>' + value + '</li>');
                    });
                    $('#modalDetalleTrabajo').modal('show');
                }


            },

            error: function (error) {
                // si hay un error lanzara el mensaje de error
                notificacioError('Error', 'Se produjo un error al obtenerdatos del vahiculo');
                console.log(error);

            }
        });
}

function aprobarSGConfirma(idSSG) {
    $('#confirmaAprovacion').addClass('modal-show');
    $('#btnConfimaAprovacion').attr('onclick', 'aprobarSG(' + idSSG + ')');
}

function aprobarSG(idSSG) {
    $('#confirmaAprovacion').removeClass('modal-show');
    $('#modalProgress').addClass('modal-show');
    $('#btnConfimaAprovacion').attr('onclick', '');
    var data = new FormData();
    data.append('idSSG', idSSG);
    $.ajax(
        {
            type: 'POST',
            url: 'servicioGarantia/aprobarOrden',
            contentType: false,
            processData: false,
            data: data,
            success: function (result) {
                $('#modalProgress').removeClass('modal-show');
                console.log(result);
                if (result.titulo === "Ok") {
                    irArriba();
                    notificacionPrimary(result.titulo, result.desc);
                    cargarVistaConEnvDato('ordenesServicio/getDetalleOrdenServicio', 'contenido', $('#idOS').val());
                } else {
                    notificacionAlerta(result.titulo, result.desc);
                }

            },
            error: function (error) {
                $('#modalProgress').removeClass('modal-show');
                // si hay un error lanzara el mensaje de error
                notificacioError('Error', 'Se produjo un error al intentar crear servicio de Garantia.');

            }
        });
}


function searManoObra() {
    var desc = $('#busquedaManoObra').val();
    $.ajax(
        {
            type: 'GET',
            url: 'Manejador/getServicios',
            data: { buscar: desc },//URL del action result que cargara la vista parcial
            success: function (result) {
                console.log('Respuesta del server ' + result);
                if (result != '' && result != null) {
                    $('#DetalleTarifario').html('');
                    $.each(result, function (key, value) {
                        var tr = $('<tr>');
                        var tdID = $('<td>' + value.id + '</td>');
                        var tdDesc = $('<td>' + value.descMdo2 + '</td>');
                        var tdPrecio = $('<td>' + monedaChange(value.precio) + '</td>');
                        var tdAdd = $('<td><button class=" btn btn-info" onclick="addLstManoModal(\'' + value.id + '\')"><i class="fas fa-plus"></i> Agregar</button></td>');
                        tr.append(tdID);
                        tr.append(tdDesc);
                        tr.append(tdPrecio);
                        tr.append(tdAdd);
                        $('#DetalleTarifario').append(tr);
                    });

                }
            },
            error: function (error) {
                // si hay un error lanzara el mensaje de error
                notificacioError('Error', 'Se produjo un error al obtenerdatos del vahiculo');
                console.log(error);
            }
        });
}

var serv = 0;



function getOrdenesServicioFecha() {
    var fechIni = $('#fechaIni').val();
    var fechFin = $('#fechaFin').val();
    $.ajax(
        {
            type: 'GET',
            url: 'ordenesServicio/MostrarHistorialOrden',
            data: { ini: fechIni, fin: fechFin },//URL del action result que cargara la vista parcial
            success: function (result) {
                $('#ResultadosFiltro').html(result);
            },
            error: function (error) {
                // si hay un error lanzara el mensaje de error
                notificacioError('Error', 'Se produjo un error al obtenerdatos del vahiculo');
                console.log(error);
            }
        });
}

function buscarOrdenServ() {
    var idOs = $('#buscarOS').val();
    $.ajax(
        {
            type: 'GET',
            url: 'ordenesServicio/getOrdenServicioID',
            data: { idOs: idOs },//URL del action result que cargara la vista parcial
            success: function (result) {
                $('#ResultadosFiltro').html(result);
            },
            error: function (error) {
                // si hay un error lanzara el mensaje de error
                notificacioError('Error', 'Se produjo un error al obtenerdatos del vahiculo');
                console.log(error);
            }
        });
}


function CrearCliente() {
    $.ajax(
        {
            type: 'POST',
            url: 'ordenesServicio/CrearCliente',
            data: {
                identidad: $('#identidad').val(),
                rtn: $('#rtn').val(),
                nombre: $('#nombre').val(),
                celular: $('#celular').val(),
                telefono: $('#telefono').val(),
                telefono2: $('#telefono2').val(),
                correoElectronico: $('#correoElectronico').val(),
                departamento: $('#departamento option:selected').text(),
                codDepartamento: $('#departamento').val(),
                colonia: $('#colonia').val(),
                calle: $('#calle').val(),
                referencias: $('#referencias').val(),
                notas: $('#notas').val(),
                serie: $('#osSerie').text()
            },
            success: function (result) {
                if (result.titulo != null) {
                    if (result.titulo == "Error") {
                        notificacioError(result.titulo, result.desc);
                    }
                    if (result.titulo == "Cuidado") {
                        notificacionAlerta(result.titulo, result.desc);
                    }
                    if (result.titulo == "ok") {
                        notificacionPrimary(result.titulo, result.desc);
                        cargarDataClienteVehiculoOS('#serieVehiculo');
                    }
                } else {
                    notificacioError('Error', 'Se produjo un error al crear el cliente');
                }
            },
            error: function (error) {
                // si hay un error lanzara el mensaje de error
                notificacioError('Error', 'Se produjo un error al crear el cliente');

            }
        });
}