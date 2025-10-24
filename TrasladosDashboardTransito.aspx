<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosDashboardTransito.aspx.vb" Inherits="TrasladosDashboardTransito" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Indice Desabastecimiento Motocargo</title>
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
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"></script>


    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>

    <script src="https://d3js.org/d3.v6.min.js"></script>

    <script src="https://canvasjs.com/assets/script/canvasjs.min.js"></script>

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

        /* From Uiverse.io by kamehame-ha */
        .cards {
            display: flex;
            flex-direction: row;
            gap: 10px;
        }

            .cards .red {
                background-color: #f43f5e;
            }

            .cards .blue {
                background-color: #3b82f6;
            }

            .cards .green {
                background-color: #22c55e;
            }

            .cards .card {
                display: flex;
                align-items: center;
                justify-content: center;
                flex-direction: column;
                text-align: center;
                height: 100px;
                width: 250px;
                border-radius: 10px;
                color: white;
                cursor: pointer;
                transition: 400ms;
            }

                .cards .card p.tip {
                    font-size: 2em;
                    font-weight: 700;
                }

                .cards .card p.second-text {
                    font-size: 1em;
                }

        /*  .cards .card:hover {
                    transform: scale(1.1, 1.1);
                }*/

        /* .cards:hover > .card:not(:hover) {
                filter: blur(10px);
                transform: scale(0.9, 0.9);
            }*/
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
        <div class="container-fluid">
            <%--<div class="row" style="display: flex; justify-content: center; align-content: center; align-items: center;">
                <div class="col-md-1">
                    <a href="TrasladosCargaMacro.aspx" class="btn btn-block btn-info btn-lg h-100">Sugerido CD</a>
                </div>
                <div class="col-md-1">
                    <a href="TrasladosCargaMacroIndirecto.aspx" class="btn btn-block btn-warning btn-lg h-100">Sugerido CI</a>
                </div>
                <div class="col-md-1">
                    <a href="TrasladosDashboardIndiceD.aspx" class="btn btn-block btn-dark btn-lg h-100">Indice D CD</a>
                </div>
                <div class="col-md-1">
                    <a href="TrasladosDashboardIndiceDCI.aspx" class="btn btn-block btn-secondary btn-lg h-100">Indice D CI</a>
                </div>
                <div class="col-md-1">
                    <a href="TrasladosDashboardMotocargo.aspx" class="btn btn-block btn-secondary btn-lg h-100">Motocargo</a>
                </div>
                <div class="col-md-1">
                    <a href="TrasladosDashboardTransito.aspx" class="btn btn-block btn-secondary btn-lg h-100">Transitos</a>
                </div>
                <div class="col-md-1">
                    <a href="TrasladosDashboardHero.aspx" class="btn btn-block btn-danger btn-lg h-100">Indice D Hero</a>
                </div>
                <div class="col-md-1 position-relative">
                    <a href="TrasladosDespachosOrdenesDiunsa.aspx" class="btn btn-block btn-danger btn-lg h-100 position-relative">Pedidos Diunsa
                    <span class="badge badge-dark position-absolute top-0 start-100 translate-middle">
                        <asp:Label Text="0" runat="server" ID="lblDiunsa" />
                    </span>
                    </a>
                </div>
            </div>--%>
            <div class="row mt-5" style="display: flex; justify-content: center; align-content: center; align-items: center;">

                <!-- From Uiverse.io by kamehame-ha -->
                <div class="cards" style="display: flex; justify-content: center; align-content: center; align-items: center;">
                    <div class="card red">
                        <p class="tip">
                            <asp:Label ID="lblPercent" runat="server" Text="0"></asp:Label>
                        </p>
                        <p class="second-text">
                            Motos +3 Dias:
                            <asp:Label ID="lblMotosTresDias" runat="server" Text="0"></asp:Label>
                            Total Transitos:
                            <asp:Label ID="lblTotalTransitos" runat="server" Text="0"></asp:Label>
                        </p>
                    </div>
                </div>
            </div>
        </div>
        <div style="height: 700px; overflow-y: auto; margin-top: 10px;">
            <div class="container-fluid">
                <asp:GridView ID="gridIndiceD" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" Width="100%">
                    <HeaderStyle CssClass="thead-dark sticky-header" />
                    <Columns>
                        <asp:BoundField DataField="ItemCode" HeaderText="Codigo" />
                        <asp:BoundField DataField="ItemName" HeaderText="Descripcion" />
                        <asp:BoundField DataField="SuppSerial" HeaderText="Serie" />
                        <asp:BoundField DataField="WhsCode" HeaderText="Sucursal" />
                        <asp:BoundField DataField="WhsName" HeaderText="Nombre Sucursal" />
                        <asp:BoundField DataField="Dias" HeaderText="Dias" ItemStyle-CssClass="heatmap-cell1" />
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha Traslado" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <style>
            .heatmap-cell1, .heatmap-cell2, .heatmap-cell3 {
                font-weight: bold;
                font-size: larger;
                color: white;
                text-shadow: 1px 1px black;
                text-align: center;
            }
        </style>

        <script>
            var cells1 = document.querySelectorAll('.heatmap-cell1');
            var values = Array.from(cells1, cell => parseFloat(cell.innerText));
            var colorScale1 = d3.scaleSequential()
                .domain(d3.extent(values)) // Calcula el rango de valores (mínimo y máximo)
                .interpolator(d3.interpolateOranges); // Gradiente de amarillo a rojo (d3-color-scheme)

            cells1.forEach(function (cell) {
                var value = parseFloat(cell.innerText);
                cell.style.backgroundColor = colorScale1(value);
            });


            var cells2 = document.querySelectorAll('.heatmap-cell2');
            var values2 = Array.from(cells2, cell => parseFloat(cell.innerText));
            var colorScale2 = d3.scaleSequential()
                .domain(d3.extent(values2)) // Calcula el rango de valores (mínimo y máximo)
                .interpolator(d3.interpolateOranges); // Gradiente de amarillo a rojo (d3-color-scheme)

            cells2.forEach(function (cell) {
                var value = parseFloat(cell.innerText);
                cell.style.backgroundColor = colorScale2(value);
            });


            var cells3 = document.querySelectorAll('.heatmap-cell3');
            var values3 = Array.from(cells3, cell => parseFloat(cell.innerText));
            var colorScale3 = d3.scaleSequential()
                .domain(d3.extent(values2)) // Calcula el rango de valores (mínimo y máximo)
                .interpolator(d3.interpolateOranges); // Gradiente de amarillo a rojo (d3-color-scheme)

            cells3.forEach(function (cell) {
                var value = parseFloat(cell.innerText);
                cell.style.backgroundColor = colorScale3(value);
            });

        </script>
        <center>
            <div class="container">
                <footer class="main-footer">
                    <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
                </footer>
            </div>
        </center>
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.css" />
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.js" integrity="sha512-gnoBksrDbaMnlE0rhhkcx3iwzvgBGz6mOEj4/Y5ZY09n55dYddx6+WYc72A55qEesV8VX2iMomteIwobeGK1BQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.css" integrity="sha512-3quBdRGJyLy79hzhDDcBzANW+mVqPctrGCfIPosHQtMKb3rKsCxfyslzwlz2wj1dT8A7UX+sEvDjaUv+WExQrA==" crossorigin="anonymous" referrerpolicy="no-referrer" />

        <script>
            $('.ui.flyout').flyout({
                context: $("form:eq(0)")
            });
        </script>

    </form>
</body>
</html>
<!-- jQuery -->
<script src="plugins/jquery/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.11.0/umd/popper.min.js" integrity="sha384-b/U6ypiBEHpOf/4+1nzFpr53nxSS+GLCkfwBdFNTxtclqqenISfwAzpKaMNFNmj4" crossorigin="anonymous"></script>

<%--<script src="vendor/bootstrap-4.1/popper.min.js"></script>--%>
<!-- Bootstrap 4 -->
<%--<script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>--%>
<!-- AdminLTE App -->
<script src="dist/js/adminlte.js"></script>

<!-- DataTables  & Plugins -->

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

<script type="text/javascript">
    $(document).ready(function () {
        // Setup - add a text input to each footer cell
        $('#gridIndiceD thead tr').clone(true).appendTo('#gridIndiceD thead');
        $('#gridIndiceD thead tr:eq(1) th').each(function (i) {
            var title = $(this).text();
            $(this).html('<input type="text" placeholder="' + title + '" />');

            $('input', this).on('keyup change', function () {
                if (table.column(i).search() !== this.value) {
                    table
                        .column(i)
                        .search(this.value)
                        .draw();
                }
            });
        });

        var table = $('#gridIndiceD').DataTable({
            dom: 'Bfrtip',
            buttons: [
                'excelHtml5',
                'pdfHtml5',
                'colvis'
            ],
            orderCellsTop: true,
            fixedHeader: true,
            bPaginate: false,
            order: [[5, "desc"]]
        });
    });
</script>
