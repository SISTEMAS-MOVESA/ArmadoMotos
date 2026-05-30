<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DetalleCargaCamion.aspx.vb" Inherits="DetalleCargaCamion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Traslados Despachos Abiertos</title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.3.0/css/all.min.css" />
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"></script>

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>

    <script src="https://d3js.org/d3.v6.min.js"></script>

    <script src='https://cdn.jsdelivr.net/npm/sweetalert2@11'></script>
    <style>
        thead input {
            width: 100% !important;
            padding: 3px !important;
            box-sizing: border-box !important;
        }

        .sticky-header th {
            position: sticky !important;
            top: 0 !important;
            z-index: 2 !important;
            background-color: #343a40 !important; /* Color de fondo del encabezado */
            color: white !important; /* Color del texto */
            border-bottom: 2px solid #dee2e6 !important;
            padding: 8px !important;
            text-align: left !important;
        }

        .sticky-grid {
            border-collapse: collapse !important;
            width: 100% !important;
        }

            .sticky-grid th,
            .sticky-grid td {
                border: 1px solid #dee2e6 !important;
            }

        table.fixedHeader-floating {
            position: fixed !important;
            background-color: white;
        }

            table.fixedHeader-floating.no-footer {
                border-bottom-width: 0;
            }

        table.fixedHeader-locked {
            position: absolute !important;
            background-color: white;
        }

        @media print {
            table.fixedHeader-floating {
                display: none;
            }
        }

        span {
            display: flex;
            justify-content: center;
            align-content: center;
        }

        .btn-lg-custom {
            height: 4rem; /* Ajusta según necesidad */
            width: 100%;
        }

        td {
            align-content: center;
        }

        .document-checkbox input[type="checkbox"] {
            transform: scale(1.5); /* Aumenta el tamaño (1.5x) */
            width: 20px; /* Opcional: Ajustar tamaño exacto */
            height: 20px; /* Opcional: Ajustar tamaño exacto */
            cursor: pointer; /* Cambia el cursor al pasar sobre el checkbox */
        }
        /* Estilo base del botón */
        .btn-group .btn-default {
            background-color: #f8f9fa; /* Color de fondo por defecto */
            border-color: #ccc; /* Color del borde */
            color: #333; /* Color del texto */
            transition: background-color 0.3s ease; /* Transición suave */
        }
            /* Efecto hover */
            .btn-group .btn-default:hover {
                background-color: #28a745; /* Color de fondo al pasar el mouse (verde) */
                border-color: #28a745; /* Color del borde al pasar el mouse */
                color: #fff; /* Color del texto al pasar el mouse */
            }

        :fullscreen {
            background: white; /* O el color que prefieras */
        }

        :-webkit-full-screen {
            background: white;
        }

        :-moz-full-screen {
            background: white;
        }

        :-ms-fullscreen {
            background: white;
        }

        .container-divs {
            display: flex;
            align-content: space-around;
            gap: 20px;
            font-size: larger;
            font-weight: 400;
        }

        .modalPanel {
            background-color: lightgray;
            padding: 25px;
            border: 2px double solid black;
            border-radius: 8px;
            max-width: 50%;
            max-height: 400px;
            overflow-y: auto;
            margin: auto;
            position: fixed;
            top: 5%;
            left: 0;
            right: 0;
            z-index: 1001;
            left: 0px !important
        }
    </style>
</head>
<body>

    <form id="form1" runat="server">
        <!-- Navigation -->
        <div class="ui inverted menu">
            <div class="ui container">
                <a class="header item" href="MainDashBoard.aspx">
                    <img src="Imagenes/mnegra.png" width="50" height="40" alt="" />
                </a>
                <div class="right menu">
                    <a class="item" href="TrasladosDashboard.aspx">Inicio</a>
                    <div class="ui simple dropdown item">
                        Pre Solicitudes
                    <i class="dropdown icon"></i>
                        <div class="menu">
                            <a class="item" href="TrasladosCargaMacro.aspx">Carga Macro</a>
                            <a class="item" href="TrasladosConfirmarPreSucursal.aspx">Confirmacion Sucursal</a>
                            <a class="item" href="TrasladosPreSolicitud.aspx">Pre Solicitud</a>
                            <a class="item" href="TrasladosPreSolicitudSucursal.aspx">Pre Solicitud Sucursal</a>
                            <div class="divider"></div>
                            <a class="item" href="TrasladosPresolicitudesAbiertas.aspx">Flujo Pre-Solicitudes</a>
                            <a class="item" href="TrasladosSolicitudesAbiertas.aspx">Flujo Solicitudes</a>
                            <a class="item" href="TrasladosFlujoProduccion.aspx">Flujo Produccion</a>
                            <div class="divider"></div>
                            <a class="item" href="TrasladosCuadroBasico.aspx">Cuadro Basico</a>
                            <a class="item" href="TrasladosCuadroBasicoLogico.aspx">Cuadro Basico Logico</a>
                            <div class="divider"></div>
                            <a class="item" href="TrabajosAdicionalesDashboard.aspx">Trabajos Adicionales</a>
                            <div class="divider"></div>
                            <a class="item" href="Grafico.aspx" target="_blank">KPI Inventarios</a>
                            <div class="divider"></div>
                            <a class="item" href="TrasladosDashboardPlanner.aspx">Planificador</a>
                        </div>
                    </div>
                    <a class="item" href="TrasladosSolicitud.aspx">Solicitudes</a>
                    <a class="item" href="TrasladosForklift.aspx">Armado de Moto</a>
                    <a class="item" href="TrasladosPreparar.aspx">Carga Camion</a>
                    <a class="item" href="TrasladosCrearDocumentos.aspx">Generar Traslados</a>
                    <a class="item" href="TrasladosCrearDocumentos.aspx">Gestion de Transferencias</a>
                    <a class="item" href="Default.aspx">Cerrar Sesion</a>
                </div>
            </div>
        </div>
        <!-- Botones -->
        <div class="container-fluid">
            <style>
                .custom-btn {
                    width: 160px; /* Tamaño fijo */
                    height: 80px; /* Alto uniforme */
                    white-space: normal; /* Permite salto de línea */
                    overflow: hidden; /* Evita que el texto se salga */
                    text-align: center;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    font-size: 16px; /* Tamaño de fuente uniforme */
                    word-wrap: break-word; /* Rompe palabras si es necesario */
                }
            </style>

            <div class="row" style="display: flex; justify-content: center; align-items: center; gap: 10px;">
                <div class="col-md-1">
                    <a href="TrasladosDashboardPlanner.aspx" class="btn btn-secondary btn-lg custom-btn">Crear Planificación</a>
                </div>
                <div class="col-md-1">
                    <a href="TrasladosDDashboard.aspx" class="btn btn-secondary btn-lg custom-btn">Ver Planificación</a>
                </div>
                <div class="col-md-1">
                    <a href="TrasladosDespachos.aspx" class="btn btn-secondary btn-lg custom-btn">Despachos Abiertos</a>
                </div>
                <div class="col-md-1">
                    <a href="TrasladosDespachosAbiertos.aspx" class="btn btn-secondary btn-lg custom-btn">Despachos Detalle</a>
                </div>
                <div class="col-md-1">
                    <a href="TrasladosPiking.aspx" class="btn btn-warning btn-lg custom-btn">Picking Despacho</a>
                </div>
            </div>
        </div>
        <div class="container-fluid mt-3">
            <asp:Panel ID="pnlSeleccion" runat="server" Style="display: flex; justify-items: center; justify-content: center;">
                <div class="d-flex" style="justify-content: center; align-items: center;">
                    <div class="row">
                        <div class="col">
                            <asp:ListBox ID="lstMotosySeries" runat="server" SelectionMode="Multiple" class="demo" Height="400px" Width="600px"></asp:ListBox>
                            <link rel="stylesheet" type="text/css" href="dual-listbox.css" />
                            <script type="text/javascript" src="dual-listbox.js"></script>
                            <script type="text/javascript">
                                new DualListbox('.demo', {
                                    addEvent: function (value) { },
                                    removeEvent: function (value) { },
                                    availableTitle: 'Motos Disponibles',
                                    selectedTitle: 'Motos Seleccionados',
                                    addButtonText: 'Agregar (>)',
                                    removeButtonText: 'Quitar (<)',
                                    addAllButtonText: 'Agregar Todas (>>)',
                                    removeAllButtonText: 'Quitar Todas (<<)'
                                });
                            </script>
                            <style type="text/css">
                                .dual-listbox .dual-listbox__button {
                                    margin-bottom: 5px;
                                    border: 0;
                                    background-color: #0090CB !important;
                                    padding: 10px;
                                    color: #fff;
                                }
                            </style>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <div class="container-fluid mt-3" style="display:flex;align-content:center;align-items:center;justify-content:center;">
                <div class="col-4">
                    <button type="button" onclick="actualizarOrdenamiento()" class="btn btn-primary">
                        Guardar Ordenamiento
                    </button>
                    <%--<asp:Button ID="btnCrearCaratula" runat="server" CssClass="btn btn-block btn-info w-100" Text="Continuar a Caratula" onclick="actualizarOrdenamiento()" />--%>
                </div>
            </div>
        </div>
        <div class="container-fkuid" style="display: flex; flex-direction: column; align-items: center">
            <footer class="main-footer">
                <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
            </footer>
        </div>
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.css" />
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.js" integrity="sha512-gnoBksrDbaMnlE0rhhkcx3iwzvgBGz6mOEj4/Y5ZY09n55dYddx6+WYc72A55qEesV8VX2iMomteIwobeGK1BQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.css" integrity="sha512-3quBdRGJyLy79hzhDDcBzANW+mVqPctrGCfIPosHQtMKb3rKsCxfyslzwlz2wj1dT8A7UX+sEvDjaUv+WExQrA==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    </form>
</body>
</html>
<!-- jQuery -->
<script src="plugins/jquery/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.11.0/umd/popper.min.js" integrity="sha384-b/U6ypiBEHpOf/4+1nzFpr53nxSS+GLCkfwBdFNTxtclqqenISfwAzpKaMNFNmj4" crossorigin="anonymous"></script>


<!-- DataTables  & Plugins -->
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.1/js/dataTables.buttons.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.html5.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.print.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.colVis.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
<link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />
<link href="https://cdn.datatables.net/buttons/1.7.1/css/buttons.dataTables.min.css" rel="stylesheet" type="text/css" />

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.css" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.js" integrity="sha512-gnoBksrDbaMnlE0rhhkcx3iwzvgBGz6mOEj4/Y5ZY09n55dYddx6+WYc72A55qEesV8VX2iMomteIwobeGK1BQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.css" integrity="sha512-3quBdRGJyLy79hzhDDcBzANW+mVqPctrGCfIPosHQtMKb3rKsCxfyslzwlz2wj1dT8A7UX+sEvDjaUv+WExQrA==" crossorigin="anonymous" referrerpolicy="no-referrer" />


<script>
   
    $("#drpPlacas").dropdown(
        {
            "fullTextSearch": true
        });
    $("#drpMotorista").dropdown(
        {
            "fullTextSearch": true
        });


    //async function actualizarOrdenamiento() {
    //    const items = Array.from(document.querySelectorAll('.dual-listbox__selected .dual-listbox__item'))
    //        .map(item => ({
    //            id: item.dataset.id,
    //            order: parseInt(item.dataset.order)
    //        }));

    //    try {
    //        const response = await fetch('UpdateOrdenamiento.ashx', {
    //            method: 'POST',
    //            headers: { 'Content-Type': 'application/json' },
    //            body: JSON.stringify(items)
    //        });

    //        const result = await response.json();

    //        if (!response.ok || !result.success) {
    //            throw new Error(result.message || 'Error desconocido');
    //        }

    //        console.log(`Éxito: ${result.message}. Items actualizados: ${result.itemsUpdated}`);
    //        alert(`Ordenamiento guardado correctamente (${result.itemsUpdated} motos)`);

    //    } catch (error) {
    //        console.error('Error al actualizar:', error);
    //        alert(`Error: ${error.message}\n\nDetalles en consola (F12)`);
    //    }
    //}
</script>

<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
<script>
    function actualizarOrdenamiento() {
        // Obtener idcamion de la URL
        const urlParams = new URLSearchParams(window.location.search);
        const idcamion = urlParams.get('idcamion');
        const motorista = urlParams.get('motorista');

        // Validar que exista idcamion
        if (!idcamion) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'No se encontró el ID del camión en la URL'
            });
            return false;
        }

        // Obtener items seleccionados
        const items = Array.from(document.querySelectorAll('.dual-listbox__selected .dual-listbox__item'))
            .map(item => ({
                id: item.dataset.id,
                order: parseInt(item.dataset.order)
            }));

        console.log(items);

        // Mostrar loader mientras se procesa
        Swal.fire({
            title: 'Guardando ordenamiento',
            html: 'Por favor espere...',
            allowOutsideClick: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });

        // Enviar datos al handler
        fetch('UpdateOrdenamiento.ashx', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(items)
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    Swal.fire({
                        icon: 'success',
                        title: '¡Éxito!',
                        html: `Se actualizaron <b>${data.itemsUpdated}</b> motos en el orden de carga<br><br>
                       <small>Serán redirigido a la carátula del camión</small>`,
                        showConfirmButton: true,
                        timer: 5000,
                        timerProgressBar: true
                    }).then((result) => {
                        // Abrir despachosCaratula en nueva pestaña con idcamion
                        window.open(`DespachosCaratula.aspx?idcamion=${idcamion}&motorista=${motorista}`, '_blank');
                        window.location.href = 'TrasladosDespachosAbiertos.aspx';
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: data.message || 'Ocurrió un error al guardar'
                    });
                }
            })
            .catch(error => {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error en la conexión: ' + error.message
                });
            });

        return false; // Previene el postback
    }
</script>
