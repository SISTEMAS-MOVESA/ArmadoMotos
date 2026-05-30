<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosDashboardPlanner.aspx.vb" Inherits="TrasladosDashboardPlanner" EnableEventValidation="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Planificador</title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.3.0/css/all.min.css" />
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css" />
    <link rel="stylesheet" href="plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css" />
    <link rel="stylesheet" href="plugins/icheck-bootstrap/icheck-bootstrap.min.css" />
    <link rel="stylesheet" href="plugins/jqvmap/jqvmap.min.css" />
    <link rel="stylesheet" href="dist/css/adminlte.min.css" />
    <link rel="stylesheet" href="plugins/overlayScrollbars/css/OverlayScrollbars.min.css" />
    <link rel="stylesheet" href="plugins/daterangepicker/daterangepicker.css" />
    <link rel="stylesheet" href="plugins/summernote/summernote-bs4.min.css" />

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"></script>

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>

    <script src="https://d3js.org/d3.v6.min.js"></script>

    <script src='https://cdn.jsdelivr.net/npm/sweetalert2@11'></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
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
            background-color: #343a40 !important;
            color: white !important;
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
            height: 4rem;
            width: 100%;
        }

        .col-nombre-alm {
            font-size: 0.72rem;
            white-space: normal;
            word-break: break-word;
            max-width: 80px;
            line-height: 1.2;
        }

        /* ---- Topnav uniforme ---- */
        .topnav {
            background: #1a252f;
            display: flex;
            flex-direction: row;
            align-items: center;
            flex-wrap: wrap;
            padding: 0 8px;
            overflow: hidden;
        }
        .topnav .brand {
            font-weight: 700;
            font-size: 0.95rem;
            color: #fff;
            padding: 12px 16px;
            display: flex;
            align-items: center;
        }
        .topnav a {
            color: #cdd3d8;
            padding: 12px 14px;
            text-decoration: none;
            font-size: 0.85rem;
            display: inline-block;
            white-space: nowrap;
        }
        .topnav a:hover    { background: rgba(255,255,255,0.08); color: #fff; }
        .topnav a.nav-active { background: rgba(255,255,255,0.15); color: #fff; font-weight: 600; }
        .topnav a i { margin-right: 4px; }

        .document-checkbox input[type="checkbox"] {
            transform: scale(1.5);
            width: 20px;
            height: 20px;
            cursor: pointer;
        }
    </style>
</head>
<body>

    <form id="form1" runat="server">
        <!-- Top Navigation Menu -->
        <nav class="topnav">
            <span class="brand"><img src="Imagenes/mnegra.png" width="18" height="18" alt="" style="vertical-align:middle; margin-right:5px;"> Movesa</span>
            <a href="TrasladosDashboardPlanner.aspx" class="nav-active"><i class="fa fa-home"></i> Inicio</a>
            <a href="Maindashboard.aspx"><i class="fa fa-th-large"></i> Men&uacute; Principal</a>
            <a href="TrasladosCuadroBasico.aspx"><i class="fa fa-table"></i> Cuadro B&aacute;sico</a>
            <a href="TrasladosPanelProduccion.aspx"><i class="fa fa-calendar"></i> Panel Planificaci&oacute;n</a>
            <a href="TrasladosDDashboard.aspx"><i class="fa fa-eye"></i> Ver Planificaci&oacute;n</a>
            <a href="TrasladosDespachos.aspx"><i class="fa fa-truck"></i> Despachos Abiertos</a>
            <a href="TrasladosDespachosAbiertos.aspx"><i class="fa fa-plus-circle"></i> Asignar Cami&oacute;n</a>
            <a href="CerrarSesion.aspx" style="margin-left:auto;"><i class="fa fa-sign-out"></i> Cerrar Sesi&oacute;n</a>
        </nav>
        <div class="container-fluid">
            <div class="my-3" style="display: flex; justify-content: center; align-content: center; align-items: center;">
                <h1>PLANIFICADOR</h1>
            </div>
            <div class="row">
                <div class="col-2">
                    <asp:Button ID="btnCrearPlan" runat="server" CssClass="btn btn-secondary btn-lg-custom" Text="Agregar al Plan" />
                </div>
                <div class="col-2">
                    <asp:Button ID="btnCuadroBasico" runat="server" CssClass="btn btn-secondary btn-lg-custom" Text="Cuadro Basico" />
                </div>
                <div class="col-2">
                    <asp:Button ID="btnCalendario" runat="server" CssClass="btn btn-secondary btn-lg-custom" Text="Calendario Despachos" />
                </div>
            </div>
            <div class="row my-3">
                <div class="col-3">
                    <label for="txtCodigoAlmacen">Filtrar</label>
                    <asp:TextBox ID="txtFilter" runat="server" Text="" placeholder="Escriba Para Buscar"
                        CssClass="form-control txt-buscador" data-table="gridIndiceD"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <asp:GridView ID="gridIndiceD" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover"
                    Width="100%" OnRowDataBound="gridIndiceD_RowDataBound">
                    <HeaderStyle CssClass="thead-dark sticky-header" />
                    <Columns>
                        <asp:BoundField DataField="Ruta" HeaderText="Ruta" />
                        <asp:BoundField DataField="Canal" HeaderText="Canal" />
                        <asp:BoundField DataField="Categoria_Ranking" HeaderText="R Ranking" />
                        <asp:BoundField DataField="Leyenda" HeaderText="Leyenda" />
                        <asp:BoundField DataField="RANK_CANAL" HeaderText="Ranking" />
                        <asp:BoundField DataField="Categoria_Indice" HeaderText="R Desabasto" />
                        <asp:BoundField DataField="WHSCODE" HeaderText="Almacen" />
                        <asp:BoundField DataField="Nombre_Almacen" HeaderText="Nombre Almacen" ItemStyle-CssClass="col-nombre-alm" />
                        <asp:BoundField DataField="IND_1_40" HeaderText="1~40" ItemStyle-CssClass="heatmap-cell1" DataFormatString="{0:P2}" HtmlEncode="False" />
                        <asp:BoundField DataField="IND_41_70" HeaderText="41~70" ItemStyle-CssClass="heatmap-cell1" DataFormatString="{0:P2}" HtmlEncode="False" />
                        <asp:BoundField DataField="IND_GRAL" HeaderText="Indice" ItemStyle-CssClass="heatmap-cell1" DataFormatString="{0:P2}" HtmlEncode="False" />
                        <asp:BoundField DataField="Indice_Proyectado" HeaderText="Indice Proyectado" ItemStyle-CssClass="heatmap-cell4" DataFormatString="{0:P2}" HtmlEncode="False" />
                        <asp:BoundField DataField="SUM_CB" HeaderText="Cuadro" DataFormatString="{0:0}" />
                        <asp:BoundField DataField="SUM_DE" HeaderText="Despacho" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="SUM_CO" HeaderText="Comp" />
                        <asp:BoundField DataField="SUM_SO" HeaderText="Sol" />
                        <asp:BoundField DataField="SUM_TR" HeaderText="Tran" />
                        <asp:BoundField DataField="SUM_FI" HeaderText="Fis" DataFormatString="{0:0}"/>
                        <asp:BoundField DataField="SUM_FA" HeaderText="Fal" DataFormatString="{0:0}"/>
                        <asp:BoundField DataField="SUM_CANTIDAD" HeaderText="UV" DataFormatString="{0:0}"/>
                        <asp:BoundField DataField="SUM_FIAJ" HeaderText="Fisico Ajustado" DataFormatString="{0:0}"/>
                        <asp:BoundField DataField="SUM_FAIN" HeaderText="Faltante Ajustado" DataFormatString="{0:0}"/>
                        <asp:BoundField DataField="ULTRANSFER" HeaderText="ULTRANSFER" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False" />
                        <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbDocument" runat="server" CssClass="document-checkbox" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

          <div class="row my-3">
                <div class="col-2">
                    <button type="button" class="btn btn-info" onclick="abrirModalCrear()">Crear Planificación</button>
                    <script>
                        function abrirModalCrear() {
                            let totalMotos = 0;

                            $("#gridPlanificacionTemp tbody tr").each(function () {
                                let valorCelda = $(this).find("td").eq(12).text().trim();
                                let valorNumerico = parseFloat(valorCelda) || 0;
                                totalMotos += valorNumerico;
                            });

                            Swal.fire({
                                title: 'Crear Nueva Planificación',
                                html: `
                                        <div class="row" style="display: flex; align-items: center;">
                                            <div class="col">Fecha Inicio</div>
                                            <div class="col">
                                                <input id="txtFinicioPlan" class="swal2-input" type="date">
                                            </div>
                                        </div>
                                        <div class="row" style="display: flex; align-items: center;">
                                            <div class="col">Fecha Vencimiento</div>
                                            <div class="col">
                                                <input id="txtFVencePlan" class="swal2-input" type="date">
                                            </div>
                                        </div>
                                        <div class="row" style="display: flex; align-items: center;">
                                            <div class="col">Cantidad Motos</div>
                                            <div class="col">
                                                <input id="txtCantidadMotos" class="swal2-input" type="number" value="${totalMotos}" readonly>
                                            </div>
                                        </div>
                                    `,
                                showCancelButton: true,
                                confirmButtonText: 'Crear Planificación',
                                cancelButtonText: 'Cancelar',
                                preConfirm: () => {
                                    let fechaInicio = document.getElementById('txtFinicioPlan').value;
                                    let fechaVencimiento = document.getElementById('txtFVencePlan').value;
                                    let cantidadMotos = parseInt(document.getElementById('txtCantidadMotos').value) || 0;

                                    if (!fechaInicio || !fechaVencimiento) {
                                        Swal.showValidationMessage('Debe llenar ambos campos de fecha.');
                                        return false;
                                    }

                                    if (new Date(fechaInicio) > new Date(fechaVencimiento)) {
                                        Swal.showValidationMessage('La fecha de inicio no puede ser mayor que la fecha de vencimiento.');
                                        return false;
                                    }

                                    return { fechaInicio, fechaVencimiento, cantidadMotos };
                                }
                            }).then((result) => {
                                if (result.isConfirmed) {
                                    CrearPlanificacion(result.value);
                                }
                            });
                        }

                        function CrearPlanificacion(datos) {
                            $.ajax({
                                url: "PlanificacionCrearHandler.ashx",
                                type: "POST",
                                contentType: "application/json",
                                data: JSON.stringify(datos),
                                success: function (response) {
                                    if (response.success) {
                                        Swal.fire('Éxito', response.message, 'success').then(() => {
                                            UpdateAfterCrearPlanificacion(response.id);
                                            console.log("Planificación creada con éxito:", response.id);
                                        });
                                    } else {
                                        Swal.fire('Error', response.message, 'error');
                                    }
                                },
                                error: function () {
                                    Swal.fire('Error', 'No se pudo completar la solicitud.', 'error');
                                }
                            });
                        }

                        function UpdateAfterCrearPlanificacion(planificacionId) {
                            $.ajax({
                                url: "PlanificacionUpdateAfterCrearHandler.ashx",
                                type: "POST",
                                contentType: "application/json",
                                data: JSON.stringify({ id: planificacionId }),
                                success: function (response) {
                                    if (response.success) {
                                        console.log("Actualización posterior realizada:", response.smessage);
                                        console.log("ID:", response.id);
                                        location.assign("TrasladosDashboardPlanner.aspx");
                                    } else {
                                        console.error("Error en la actualización posterior:", response.message);
                                    }
                                },
                                error: function () {
                                    Swal.fire('Error', 'No se pudo completar la actualización.', 'error');
                                }
                            });
                        }
                    </script>
                </div>
                <div class="col-2">
                    <asp:DropDownList ID="drpPlanificacionesAbiertas" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="col-2">
                    <asp:Button ID="btnAgregarPlan" runat="server" CssClass="btn btn-secondary" Text="Agregar a Plan" />
                </div>
                <div class="col-2">
                    <asp:Button ID="btnEliminarLineas" runat="server" CssClass="btn btn-danger" Text="Eliminar Todas las Lineas" />
                </div>
            </div>
            <div class="row">
                <asp:GridView ID="gridPlanificacionTemp" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered table-hover"
                    Width="100%" OnRowDataBound="gridPlanificacionTemp_RowDataBound">
                    <HeaderStyle CssClass="thead-dark sticky-header" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/delete.png" Text="Eliminar" CommandName="Eliminar" HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ControlStyle Height="30px" Width="30px" />
                            <ItemStyle Wrap="False" />
                        </asp:ButtonField>
                    </Columns>
                </asp:GridView>
            </div>
            <style>
                .heatmap-cell1, .heatmap-cell2, .heatmap-cell3, .heatmap-cell4 {
                    font-weight: bold;
                    font-size: large;
                    color: black;
                    text-shadow: white 3px 0 10px;
                    text-align: center;
                    align-content: center;
                }
            </style>

            <script>
                var cells1 = document.querySelectorAll('.heatmap-cell1');
                var values = Array.from(cells1, cell => parseFloat(cell.innerText));
                var colorScale1 = d3.scaleSequential()
                    .domain(d3.extent(values))
                    .interpolator(d3.interpolateBlues);

                cells1.forEach(function (cell) {
                    var value = parseFloat(cell.innerText);
                    cell.style.backgroundColor = colorScale1(value);
                });

                var cells4 = document.querySelectorAll('.heatmap-cell4');
                var values4 = Array.from(cells4, cell => parseFloat(cell.innerText));
                var colorScale4 = d3.scaleSequential()
                    .domain(d3.extent(values4)) // Calcula el rango de valores (mínimo y máximo)
                    .interpolator(d3.interpolateBlues); // Gradiente de amarillo a rojo (d4-color-scheme)
                //.interpolator(d3.interpolateInferno); // Gradiente de amarillo a rojo (d4-color-scheme)

                cells4.forEach(function (cell) {
                    var value = parseFloat(cell.innerText);
                    cell.style.backgroundColor = colorScale4(value);
                });

            </script>
        </div>
        
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


    </form>
</body>
</html>
<!-- jQuery -->
<script src="plugins/jquery/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.11.0/umd/popper.min.js" integrity="sha384-b/U6ypiBEHpOf/4+1nzFpr53nxSS+GLCkfwBdFNTxtclqqenISfwAzpKaMNFNmj4" crossorigin="anonymous"></script>

<!-- AdminLTE App -->
<script src="dist/js/adminlte.js"></script>

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
        // Configurar el DataTable
        var table = $('#gridIndiceD').DataTable({
            dom: 'Bfrtip',
            buttons: ['excelHtml5'],
            fixedHeader: true,
            bPaginate: false,
            ordering: true,
            order: [[0, "asc"], [1, "asc"], [2, "asc"], [3, "asc"], [4, "asc"]],
            drawCallback: function () {
                applyMergeAll();
            }
        });

        function resetMerges(columnIndex) {
            $('#gridIndiceD tbody tr').each(function () {
                var cell = $(this).find('td').eq(columnIndex);
                cell.show();
                cell.removeAttr('rowspan');
            });
        }

        function mergeCellsIfEnabled(columnIndex) {
            var rows = $('#gridIndiceD tbody tr:visible'); // Solo filas visibles
            var previousValue = null;
            var rowspan = 1;
            var startRowIndex = 0;

            rows.each(function (index, row) {
                var cell = $(row).find('td').eq(columnIndex);
                var currentValue = cell.text();
                var shouldMerge = cell.attr('data-merge') === 'true';

                if (shouldMerge && currentValue === previousValue) {
                    cell.hide();
                    $(rows[startRowIndex]).find('td').eq(columnIndex).attr('rowspan', ++rowspan);
                } else {
                    previousValue = currentValue;
                    rowspan = 1;
                    startRowIndex = index;
                }
            });
        }

        // Fusiona todas las columnas deseadas
        function applyMergeAll() {
            // Indica aquí las columnas a fusionar
            [0, 1, 2, 3].forEach(function (colIdx) {
                resetMerges(colIdx);
                mergeCellsIfEnabled(colIdx);
            });
        }

        // Filtro usando DataTables API
        $(".txt-buscador[data-table]").on('keyup', function () {
            table.search($(this).val()).draw();
        });
    });
</script>

<script>
    function autoRefresh() {
        Swal.fire({
            title: 'Refrescando datos',
            text: 'Por favor espere...',
            allowOutsideClick: false,
            allowEscapeKey: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });

        setTimeout(() => {
            window.location.reload();
        }, 3000); // Espera 2 segundos antes de recargar para que se vea el mensaje
    }

    setInterval(autoRefresh, 900000); // 5 minutos
</script>

<script>
    $(document).ready(function () {
        $("#<%= gridIndiceD.ClientID %> tbody tr").each(function () {
            var ranking = $(this).find('td').eq(4).text().trim(); // 0-based index (4ta columna)
            var rankingTd = $(this).find('td').eq(4); // col 4 = 'Ranking'
            var whscode = $(this).find('td').eq(6).text().trim(); // 0-based index (6ta columna)
            var whscodeTd = $(this).find('td').eq(6); // col 6 = 'Almacen'
            var despachoTd = $(this).find('td').eq(13); // col 13 = 'Despacho'
            var despachoValue = despachoTd.text().trim();

            var hrefWhscode = 'TrasladosCuadroBasico.aspx?whscode=' + encodeURIComponent(whscode);
            var iconHtmlWhscodeTd = '<a href="' + hrefWhscode + '" title="Ver despachos" style="margin-left:8px;" target="_blank">'
                + whscode + ' <i class="fa fa-search" style="font-size: 18px; color: #007bff; cursor:pointer"></i></a>';
            whscodeTd.html(iconHtmlWhscodeTd);

            var hrefRanking = 'TrasladosCargaMacroBI.aspx';
            var iconHtmlRanking = '<a href="' + hrefRanking + '" title="Ver despachos" style="margin-left:8px;" target="_blank">'
                + ranking + ' <i class="fa fa-search" style="font-size: 18px; color: #007bff; cursor:pointer"></i></a>';
            rankingTd.html(iconHtmlRanking);




            if (parseInt(despachoValue) > 0) {
                var href = 'TrasladosDespachosDetalleAlmacen.aspx?whscode=' + encodeURIComponent(whscode);

                var iconHtml = '<a href="' + href + '" title="Ver despachos" style="margin-left:8px;" target="_blank">'
                    + despachoValue + ' <i class="fa fa-search" style="font-size: 18px; color: #007bff; cursor:pointer"></i></a>';

                despachoTd.html(iconHtml);
            } else {
                despachoTd.text(despachoValue);
            }
        });
    });
</script>

<script>
    $(document).ready(function () {
        // Obtén el ID renderizado de tu GridView
        var gridId = "<%= gridIndiceD.ClientID %>";

        // Busca los índices de las columnas relevantes usando el encabezado
        var headers = $("#" + gridId + " thead th");
        var idx1_40 = -1, idx41_70 = -1, idxGral = -1;
        headers.each(function (i) {
            var text = $(this).text().trim();
            if (text === "1~40") idx1_40 = i;
            //if (text === "41~70") idx41_70 = i;
            //if (text === "Indice") idxGral = i;
        });

        $("#" + gridId + " tbody tr").each(function () {
            // Solo filas de datos, no encabezados ni agrupadores
            if ($(this).find("td").length < headers.length - 1) return; // puedes ajustar si tienes columnas ocultas

            [idx1_40, idx41_70, idxGral].forEach(function (idx) {
                if (idx < 0) return;
                var $cell = $($(this).find("td")[idx]);
                var txt = $cell.text().replace("%", "").replace(",", ".").trim();
                var val = parseFloat(txt);

                if (!isNaN(val) && val >= 50) {
                    $cell.css("background", "#ff4444").css("color", "white").css("font-weight", "bold");
                }
            }, this);
        });
    });

</script>

<!-- ===== TOOLTIP MODELOS FALTANTES ===== -->
<style>
    #ttipFaltante {
        display: none;
        position: fixed;
        z-index: 9999;
        background: #1a252f;
        color: #fff;
        border-radius: 6px;
        padding: 8px 12px;
        font-size: 0.78rem;
        max-width: 280px;
        box-shadow: 0 4px 14px rgba(0,0,0,0.35);
        pointer-events: none;
        line-height: 1.5;
    }
    #ttipFaltante .ttip-title { font-weight: 700; font-size: 0.8rem; margin-bottom: 5px; border-bottom: 1px solid rgba(255,255,255,0.2); padding-bottom: 3px; }
    #ttipFaltante .ttip-row  { display: flex; justify-content: space-between; gap: 10px; }
    #ttipFaltante .ttip-fain { font-weight: 700; color: #ff6b6b; white-space: nowrap; }
    #ttipFaltante .ttip-load { color: #aaa; font-style: italic; }
</style>
<div id="ttipFaltante"></div>
<script>
    (function () {
        var $tip   = $('#ttipFaltante');
        var cache  = {};
        var hdrMap = { 8: '1_40', 9: '41_70', 10: 'gral' }; // índices de columna → rango

        // Marca las celdas heatmap con datos para identificarlas
        $(document).ready(function () {
            markHeatCells();
        });

        function markHeatCells() {
            var $rows = $('#gridIndiceD tbody tr');
            $rows.each(function () {
                var $tds = $(this).find('td');
                var whscode = $tds.eq(6).text().trim();
                // quita posible HTML del link que agrega el otro script
                whscode = whscode.replace(/\s*🔍.*/g, '').trim();

                $.each(hdrMap, function (colIdx, rango) {
                    $tds.eq(parseInt(colIdx))
                        .attr('data-tip-whs', whscode)
                        .attr('data-tip-rango', rango)
                        .css('cursor', 'pointer');
                });
            });
        }

        // Delegación de eventos para que funcione tras redraw de DataTables
        $(document).on('mouseenter', '#gridIndiceD td[data-tip-whs]', function (e) {
            var whs   = $(this).attr('data-tip-whs');
            var rango = $(this).attr('data-tip-rango');
            if (!whs) return;

            var key = whs + '|' + rango;
            positionTip(e);
            if (cache[key] !== undefined) {
                showTip(cache[key], whs, rango);
            } else {
                $tip.html('<div class="ttip-load">Cargando...</div>').show();
                $.getJSON('GetModFaltanteHandler.ashx', { whscode: whs, rango: rango }, function (data) {
                    cache[key] = data;
                    showTip(data, whs, rango);
                }).fail(function () {
                    $tip.html('<div class="ttip-load">Error al cargar</div>').show();
                });
            }
        });

        $(document).on('mousemove', '#gridIndiceD td[data-tip-whs]', function (e) {
            positionTip(e);
        });

        $(document).on('mouseleave', '#gridIndiceD td[data-tip-whs]', function () {
            $tip.hide();
        });

        function positionTip(e) {
            var x = e.clientX + 14, y = e.clientY + 14;
            if (x + 290 > window.innerWidth)  x = e.clientX - 294;
            if (y + 200 > window.innerHeight) y = e.clientY - 210;
            $tip.css({ left: x, top: y });
        }

        function showTip(data, whs, rango) {
            var label = rango === '1_40' ? 'Pareto 1-40%' : rango === '41_70' ? 'Pareto 41-70%' : 'Pareto General';
            if (!data || data.length === 0) {
                $tip.html('<div class="ttip-title">' + whs + ' — ' + label + '</div><div class="ttip-load">Sin faltantes</div>').show();
                return;
            }
            var html = '<div class="ttip-title">' + whs + ' — ' + label + '</div>';
            data.forEach(function (r) {
                html += '<div class="ttip-row"><span>' + r.m + '</span>'
                      + '<span class="ttip-fain">'
                      + 'Stock: ' + r.fi + ' &nbsp;Falta: <b>' + r.f + '</b> &nbsp;CB: ' + r.cb
                      + '</span></div>';
            });
            $tip.html(html).show();
        }

        // Re-marcar celdas luego de cada redraw de DataTables
        $(document).on('draw.dt', '#gridIndiceD', function () {
            markHeatCells();
        });
    })();
</script>