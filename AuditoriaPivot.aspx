<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AuditoriaPivot.aspx.vb" Inherits="AuditoriaPivot" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Auditoria || Pivot</title>
    
    <!-- Font and Icons -->
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
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/2.9.2/umd/popper.min.js" integrity="sha512-2rNj2KJ+D8s1ceNasTIex6z4HWyOnEYLVC3FigGOmyQCZc2eBXKgOxQmo3oKLHyfcj53uz4QMsRCWNbLd32Q1g==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>


    <!-- jQuery and jQuery UI -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

    <!-- PivotTable.js -->
    <link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/pivottable/2.23.0/pivot.min.css">
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pivottable/2.23.0/pivot.min.js"></script>

    <!-- Plotly.js for Heatmap -->
    <script src="https://cdn.plot.ly/plotly-latest.min.js"></script>

    <!-- Plotly renderers for PivotTable -->
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pivottable/2.23.0/plotly_renderers.min.js"></script>
    <!-- Export XLS for PivotTable -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.18.5/xlsx.full.min.js"></script>

</head>
<body>
    <form id="form1" runat="server">
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
                            <a class="nav-link" href="DashboardAuditoria.aspx">Inicio</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="AuditoriaInventarioArmadas.aspx">Inventario Motos Armadas</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#">Auditoria CC</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="AuditoriaInformeInventario.aspx">Informes</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="Default.aspx">Cerrar Sesion</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>

        <div class="container-fluid" style="flex:auto; justify-content:center;">
            <h1 class="text-center">Pivot Auditoria</h1>
            <a href="AuditoriaInformeInventario.aspx" class="btn btn-default btn-light">Regresar a Informe</a>
            <button id="exportBtn" type="button" class="btn btn-default btn-light">Exportar a XLS</button>
            <div class="row my-3">
                 <div id="output" style="margin-top: 20px;"></div>
            </div>
        </div>

        <div class="container" style="flex; justify-content:center;">
            <footer class="main-footer">
                <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
            </footer>
        </div>

        <asp:ScriptManager ID="sm1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    </form>
</body>
</html>

<!-- Pivot JS Logic -->
<script type="text/javascript">
    $(document).ready(function () {
        const container = document.getElementById("output");

        $.ajax({
            url: 'InventarioHandler.ashx',
            method: 'GET',
            dataType: 'json',
            success: function (data) {
                console.log(data);

                let dataPivot = [];
                data.forEach(item => {
                    const {
                        EstadoVeh,
                        Modelo,
                        Marca,
                        Pasillo,
                        Segmento,
                        Cilindros,
                        Color,
                        Usuario,
                        AUDITORIA,
                        PINTURA,
                        LIMPIEZA
                    } = item;

                    dataPivot.push({
                        EstadoVeh,
                        Modelo,
                        Marca,
                        Pasillo,
                        Segmento,
                        Cilindros,
                        Color,
                        Usuario,
                        AUDITORIA,
                        PINTURA,
                        LIMPIEZA
                    });
                });

                console.log(dataPivot);

                if ($.pivotUtilities && $.pivotUtilities.renderers) {
                    $(container).pivotUI(dataPivot, {
                        rows: ["Modelo"],
                        cols: ["EstadoVeh"],
                        vals: ["AUDITORIA", "PINTURA", "LIMPIEZA"],
                        aggregatorName: "Count",
                        rendererName: "Table",
                        filters: {
                            "EstadoVeh": true,
                            "Modelo": true,
                            "Marca": true
                        },
                        renderers: $.pivotUtilities.renderers
                    });
                } else {
                    console.error("PivotTable.js renderers no están disponibles. Asegúrate de cargar correctamente PivotTable.js.");
                }
            },
            error: function (err) {
                console.error('Error al obtener el JSON:', err);
            }
        });
    });

    $("#exportBtn").on("click", function () {
        let table = document.querySelector("table.pvtTable");
        if (!table) {
            alert("No hay tabla generada para exportar.");
            return;
        }
        let wb = XLSX.utils.table_to_book(table, {
            sheet: "Sheet1"
        });
        XLSX.writeFile(wb, "tabla_dinamica.xlsx");
    });

    //$(document).ready(function () {
    //    const container = document.getElementById("output");

    //    $.ajax({
    //        url: 'InventarioHandler.ashx', // Llamada al handler en lugar de PHP
    //        method: 'GET',
    //        dataType: 'json',
    //        success: function (data) {
    //            console.log(data);

    //            let dataPivot = [];
    //            data.forEach(item => {
    //                console.log(item);
    //                const {
    //                    EstadoVeh,  // Cambiado de "Marca" a "EstadoVeh" para coincidir con los datos del JSON
    //                    Modelo,
    //                    Mecanico,
    //                    Control,
    //                    encuestas // Esto depende de cómo esté estructurado en tu JSON
    //                } = item;

    //                // Procesamiento de encuestas dentro de cada item
    //                for (const estado in encuestas) {
    //                    if (Object.prototype.hasOwnProperty.call(encuestas, estado)) {
    //                        const options = encuestas[estado];
    //                        console.log(options, Array.isArray(options));
    //                        if (Array.isArray(options)) {
    //                            options.forEach(option => {
    //                                if (option && typeof option === 'object') {
    //                                    dataPivot.push({
    //                                        Estado: estado,
    //                                        PuntoInspeccion: option.label,
    //                                        EstadoVeh,  // Utiliza EstadoVeh como atributo para el reporte
    //                                        Modelo,
    //                                        Mecanico,
    //                                        Control,
    //                                        Valor: option.value
    //                                    });
    //                                }
    //                            });
    //                        } else {
    //                            console.warn(`La opción para el estado ${estado} no es un array:`, options);
    //                        }
    //                    }
    //                }
    //            });

    //            console.log(dataPivot);

    //            // Verificar si $.pivotUtilities y sus renderers están definidos
    //            if ($.pivotUtilities && $.pivotUtilities.renderers) {
    //                $(container).pivotUI(dataPivot, {
    //                    rows: ["Estado", "PuntoInspeccion"], // Filas de la tabla pivot
    //                    vals: ["Valor"], // Columna de valores
    //                    aggregatorName: "Sum", // Sumar los valores
    //                    rendererName: "Heatmap", // Tipo de renderer (Mapa de calor)
    //                    filters: {
    //                        "EstadoVeh": true, // Filtro de "EstadoVeh"
    //                        "Modelo": true,    // Filtro de "Modelo"
    //                        "Mecanico": true,  // Filtro de "Mecanico"
    //                        "Control": true    // Filtro de "Control"
    //                    },
    //                    renderers: $.pivotUtilities.renderers // Renderizadores de la tabla pivot
    //                });
    //            } else {
    //                console.error("PivotTable.js renderers no están disponibles. Asegúrate de cargar correctamente PivotTable.js.");
    //            }
    //        },
    //        error: function (err) {
    //            console.error('Error al obtener el JSON:', err);
    //        }
    //    });
    //});
</script>
