<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrabajosAdicionalesDashboard.aspx.vb" Inherits="TrabajosAdicionalesDashboard" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Trabajos || Dashboard</title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.3.0/css/all.min.css">
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css">
    <link rel="stylesheet" href="plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css">
    <link rel="stylesheet" href="plugins/icheck-bootstrap/icheck-bootstrap.min.css">
    <link rel="stylesheet" href="plugins/jqvmap/jqvmap.min.css">
    <link rel="stylesheet" href="dist/css/adminlte.min.css">
    <link rel="stylesheet" href="plugins/overlayScrollbars/css/OverlayScrollbars.min.css">
    <link rel="stylesheet" href="plugins/daterangepicker/daterangepicker.css">
    <link rel="stylesheet" href="plugins/summernote/summernote-bs4.min.css">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
    <link href="css/movesa_load_spinner.css" rel="stylesheet" />

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>

</head>
<body>

    <form id="form1" runat="server">
        <!-- Navigation -->
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark static-top">
            <div class="container">
                <a class="navbar-brand" href="MainDashBoard.aspx">
                    <img src="Imagenes/mnegra.png" width="50" height="40" alt="" />
                </a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarSupportedContent">
                    <ul class="navbar-nav ms-auto">
                        <li class="nav-item">
                            <a class="nav-link" href="TrasladosDashboard.aspx">Inicio</a>
                        </li>
                        <li class="nav-item dropdown">
                            <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Pre Solicitudes
                            </a>
                            <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                                <a class="dropdown-item" href="TrasladosCargaMacro.aspx">Carga Macro</a>
                                <a class="dropdown-item" href="TrasladosConfirmarPreSucursal.aspx">Confirmacion Sucursal</a>
                                <a class="dropdown-item" href="TrasladosPreSolicitud.aspx">Pre Solicitud</a>
                                <a class="dropdown-item" href="TrasladosPreSolicitudSucursal.aspx">Pre Solicitud Sucursal</a>
                                <div class="dropdown-divider"></div>
                                <a class="dropdown-item" href="TrasladosPresolicitudesAbiertas.aspx">Flujo Pre-Solicitudes</a>
                                <a class="dropdown-item" href="TrasladosSolicitudesAbiertas.aspx">Flujo Solicitudes</a>
                                <a class="dropdown-item" href="TrasladosFlujoProduccion.aspx">Flujo Produccion</a>
                                <div class="dropdown-divider"></div>
                                <a class="dropdown-item" href="TrasladosCuadroBasico.aspx">Cuadro Basico</a>
                                <div class="dropdown-divider"></div>
                                <a class="dropdown-item" href="TrabajosAdicionalesDashboard.aspx">Trabajos Adicionales</a>
                            </div>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="TrasladosSolicitud.aspx">Solicitudes</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="TrasladosForklift.aspx">Armado de Moto</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="TrasladosPreparar.aspx">Carga Camion</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="TrasladosCrearDocumentos.aspx">Generar Traslados</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="TrasladosCrearDocumentos.aspx">Gestion de Transferencias</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="Default.aspx">Cerrar Sesion</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
        <br />
        <div class="row d-flex">
            <div class="col-4">
                <div class="row d-flex justify-content-center">
                    <div class="col-4">
                        <div class="small-box bg-gradient-info">
                            <div class="inner">
                                <p>Iniciar Trabajo</p>
                            </div>
                            <a href="TrasladosCargaMacro.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                        </div>
                    </div>

                    <div class="col-4">
                        <div class="small-box bg-gradient-warning">
                            <div class="inner">
                                <p>Crear Liquidacion</p>
                            </div>
                            <a href="TrasladosCargaMacroIndirecto.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-6">
                <div class="row d-flex justify-content-center">
                    <div id="radarEtapas" style="width: 100%; height: 200px;"></div>
                </div>
            </div>

        </div>
        <div>
            <div class="container-fluid p-5">
                <table id="matrizrep" class="table table-bordered table-striped">
                    <thead style="text-align: center">
                        <tr>
                            <th>#</th>
                            <th>Fecha</th>
                            <th>C. Almacen</th>
                            <th>N. Almacen</th>
                            <th>Ruta</th>
                            <th>Supervisor</th>
                            <th>Accion</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="repeater1" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td style="text-align: left;"><%# Eval("ID") %></td>
                                    <td style="text-align: left;"><%# Eval("DATECREATED") %></td>
                                    <td style="text-align: left;"><%# Eval("SERIE") %></td>
                                    <td style="text-align: left;"><%# Eval("ITENMANE") %></td>
                                    <td style="text-align: left;"><%# Eval("IDTRABAJO") %></td>
                                    <td style="text-align: left;"><%# Eval("TRABAJO") %></td>
                                    <td style="text-align: center;">
                                        <a href='<%# "TrabajosAdicionalesDetalle.aspx?id=" + CStr(Eval("ID")) %>' class="btn btn-info text-center active">Ir</a>
<%--                                        <a href='<%# "TrabajosAdicionalesDetalle.aspx?id=" + CStr(Eval("ID")) %>'>
                                            <button class="btn btn-info text-center active">IR</button>
                                        </a>--%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
        <center>
            <div class="container">
                <footer class="main-footer">
                    <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
                </footer>
            </div>
        </center>

        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>

    </form>


</body>
</html>
<!-- jQuery -->
<script src="plugins/jquery/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.11.0/umd/popper.min.js" integrity="sha384-b/U6ypiBEHpOf/4+1nzFpr53nxSS+GLCkfwBdFNTxtclqqenISfwAzpKaMNFNmj4" crossorigin="anonymous"></script>
<link href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.css" rel="stylesheet" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.js"></script>


<link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css" />
<link rel="stylesheet" href="https://cdn.datatables.net/buttons/2.4.2/css/buttons.dataTables.min.css" />
<script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
<script src="https://cdn.datatables.net/buttons/2.4.2/js/dataTables.buttons.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.10.1/jszip.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
<script src="https://cdn.datatables.net/buttons/2.4.2/js/buttons.html5.min.js"></script>
<script src="https://cdn.datatables.net/buttons/2.4.2/js/buttons.print.min.js"></script>



<%--CHART--%>
<script>
    $(document).ready(function () {
        $.ajax({
            type: "POST",
            url: "TrabajosAdicionalesDashboard.aspx/GetRadarChartDataEtapas",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var chartData = JSON.parse(data.d);

                var options = {
                    chart: {
                        height: 350,
                        type: 'radar',
                    },
                    dataLabels: {
                        enabled: true
                    },
                    plotOptions: {
                        radar: {
                            size: 140,
                            polygons: {
                                strokeColors: '#e9e9e9',
                                fill: {
                                    colors: ['#f8f8f8', '#fff']
                                }
                            }
                        }
                    },
                    title: {
                        text: 'Distribucion Trabajos Adicionales'
                    },
                    colors: ['#FF4560'],
                    markers: {
                        size: 4,
                        colors: ['#fff'],
                        strokeColor: '#FF4560',
                        strokeWidth: 2,
                    },
                    markers: {
                        size: 4
                    },
                    series: [{
                        name: 'Conteo',
                        data: chartData.map(function (item) { return item.Conteo; })
                    }],
                    xaxis: {
                        categories: chartData.map(function (item) { return item.ShippingLine; })
                    }
                };

                var chart = new ApexCharts(document.querySelector("#radarEtapas"), options);
                chart.render();
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
</script>

<script type="text/javascript">
    $(document).ready(function () {
        // Setup - add a text input to each footer cell
        $('#matrizrep thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#matrizrep thead');

        var table = $('#matrizrep').DataTable({
            orderCellsTop: true,
            fixedHeader: true,
            order: [[0, 'desc']],
            dom: 'Bfrtip',
            buttons: ['copy', 'csv', 'excel', 'pdf', 'print'],
            initComplete: function () {
                var api = this.api();

                // For each column
                api
                    .columns()
                    .eq(0)
                    .each(function (colIdx) {
                        // Set the header cell to contain the input element
                        var cell = $('.filters th').eq(
                            $(api.column(colIdx).header()).index()
                        );
                        var title = $(cell).text();
                        $(cell).html('<input type="text" placeholder="' + title + '" />');

                        // On every keypress in this input
                        $(
                            'input',
                            $('.filters th').eq($(api.column(colIdx).header()).index())
                        )
                            .off('keyup change')
                            .on('change', function (e) {
                                // Get the search value
                                $(this).attr('title', $(this).val());
                                var regexr = '({search})'; //$(this).parents('th').find('select').val();

                                var cursorPosition = this.selectionStart;
                                // Search the column for that value
                                api
                                    .column(colIdx)
                                    .search(
                                        this.value != ''
                                            ? regexr.replace('{search}', '(((' + this.value + ')))')
                                            : '',
                                        this.value != '',
                                        this.value == ''
                                    )
                                    .draw();
                            })
                            .on('keyup', function (e) {
                                e.stopPropagation();

                                $(this).trigger('change');
                                $(this)
                                    .focus()[0]
                                    .setSelectionRange(cursorPosition, cursorPosition);
                            });
                    });
            },
        });
    });
</script>
