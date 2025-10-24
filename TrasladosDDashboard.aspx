<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosDDashboard.aspx.vb" Inherits="TrasladosDDashboard" EnableEventValidation="false" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Planificiones Abiertas</title>
    <link rel="stylesheet" href="plugins/jqvmap/jqvmap.min.css" />
    <link rel="stylesheet" href="dist/css/adminlte.min.css" />
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
    </style>
</head>
<body>

    <form id="form1" runat="server">
        <!-- Top Navigation Menu -->
        <div class="topnav">
            <style>
                /* Style the navigation menu */
                .topnav {
                    overflow: hidden;
                    background-color: #333;
                    position: relative;
                }

                    /* Hide the links inside the navigation menu (except for logo/home) */
                    .topnav #myLinks {
                        display: none;
                    }

                    /* Style navigation menu links */
                    .topnav a {
                        color: white;
                        padding: 14px 16px;
                        text-decoration: none;
                        font-size: 17px;
                        display: block;
                    }

                        /* Style the hamburger menu */
                        .topnav a.icon {
                            background: black;
                            display: block;
                            position: absolute;
                            right: 0;
                            top: 0;
                        }

                        /* Add a grey background color on mouse-over */
                        .topnav a:hover {
                            background-color: #ddd;
                            color: black;
                        }

                /* Style the active link (or home/logo) */
                .active {
                    background-color: black;
                    color: white;
                }
            </style>
            <a href="TrasladosDashboardPlanner.aspx" class="active">
                <img src="Imagenes/mnegra.png" width="20" height="20" alt="" />
                Inicio
            </a>
            <!-- Navigation links (hidden by default) -->
            <div id="myLinks">
                <a href="Maindashboard.aspx">Menu Principal</a>
                <a href="TrasladosCuadroBasico.aspx">Cuadro Basico</a>
                <a href="TrasladosPanelProduccion.aspx">Panel Planificación</a>
                <a href="TrasladosDDashboard.aspx">Ver Planificación</a>
                <a href="TrasladosDespachos.aspx">Despachos Abiertos</a>
                <a href="TrasladosDespachosAbiertos.aspx">Asignar Camion</a>
                <a href="CerrarSesion.aspx">Cerrar Sesion</a>
            </div>
            <!-- "Hamburger menu" / "Bar icon" to toggle the navigation links -->
            <a href="javascript:void(0);" class="icon" onclick="myFunction()">
                <i class="fa fa-bars"></i>
            </a>
            <script>
                /* Toggle between showing and hiding the navigation menu links when the user clicks on the hamburger menu / bar icon */
                function myFunction() {
                    var x = document.getElementById("myLinks");
                    if (x.style.display === "block") {
                        x.style.display = "none";
                    } else {
                        x.style.display = "block";
                    }
                }
            </script>
        </div>
        <div class="container-fluid">
            <div class="my-3" style="display: flex; justify-content: center; align-content: center; align-items: center;">
                <h1>Planificaciones</h1>
            </div>
            <div style="justify-content: center;">
                <div class="ui segment">
                    <div class="ui two column very relaxed grid">
                        <div class="column">
                            <h3>Planificaciones Abiertas</h3>
                            <asp:Label ID="lblPlanId" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:GridView ID="gridDespachos" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover"
                                Width="100%">
                                <HeaderStyle CssClass="thead-dark sticky-header" />
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="Plan Id" />
                                    <asp:BoundField DataField="FECHAVENCIMIENTO" HeaderText="Vencimiento" />
                                    <asp:BoundField DataField="FALTANTE" HeaderText="Unds Faltantes" />
                                    <asp:BoundField DataField="ESTADO" HeaderText="Estado" />
                                    <asp:BoundField DataField="USUARIO" HeaderText="Usuario" />
                                    <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/view.png" Text="Ver" CommandName="Ver" HeaderText="Ver">
                                        <ControlStyle Height="30px" Width="30" />
                                        <ItemStyle Wrap="False" />
                                    </asp:ButtonField>
                                    <asp:TemplateField HeaderText="Agregar Plan">
                                        <ItemTemplate>
                                            <button type="button" class="btn btn-danger" onclick='cerrarPlan(<%# Eval("ID") %>)'>Cerrar</button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="column">
                            <h3>Planificaciones Abiertas</h3>
                            <asp:GridView ID="gridDespachosAbiertos" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover"
                                Width="100%">
                                <HeaderStyle CssClass="thead-dark sticky-header" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbDocument" runat="server" CssClass="document-checkbox" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Agregar Plan">
                                        <ItemTemplate>
                                            <button type="button" class="btn btn-danger" onclick='cerrarDespacho(<%# Eval("PLANID") %>)'>Cerrar</button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PLANID" HeaderText="Plan Id" />
                                    <asp:BoundField DataField="Id Planificacion" HeaderText="Id Despacho" />
                                    <asp:BoundField DataField="Fecha Vencimiento" HeaderText="Fecha Despacho" />
                                    <asp:BoundField DataField="ESTADO" HeaderText="Estado" />
                                    <asp:BoundField DataField="Suma Almacenes" HeaderText="Tot. Almacenes" />
                                    <asp:BoundField DataField="Suma Fantalte" HeaderText="Tot. Faltante" />

                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="ui vertical divider">
                        <>
                    </div>
                </div>
            </div>
            <div class="row my-2">
                <asp:GridView ID="gridAlmacenesDespachos" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover"
                    Width="100%" OnRowDataBound="gridIndiceD_RowDataBound">
                    <HeaderStyle CssClass="thead-dark sticky-header" />
                    <Columns>
                        <asp:BoundField DataField="Ruta" HeaderText="Ruta" />
                        <asp:BoundField DataField="Canal" HeaderText="Canal" />
                        <asp:BoundField DataField="CRanking" HeaderText="R Ranking" />
                        <asp:BoundField DataField="CIndice" HeaderText="R Desabasto" />
                        <asp:BoundField DataField="Leyenda" HeaderText="Leyenda" />
                        <asp:BoundField DataField="Ranking" HeaderText="Ranking" />
                        <asp:BoundField DataField="Codigo" HeaderText="Almacen" />
                        <asp:BoundField DataField="Almacen" HeaderText="Nombre Almacen" />
                        <asp:BoundField DataField="Indice" HeaderText="Indice" ItemStyle-CssClass="heatmap-cell1" />
                        <asp:BoundField DataField="Indice_Proyectado" HeaderText="Indice Proyectado" ItemStyle-CssClass="heatmap-cell1" />
                        <asp:BoundField DataField="Cuadro" HeaderText="CB" />
                        <asp:BoundField DataField="COMP" HeaderText="Comp" />
                        <asp:BoundField DataField="Despacho" HeaderText="Despacho" />
                        <asp:BoundField DataField="SOL" HeaderText="Sol" />
                        <asp:BoundField DataField="Transito" HeaderText="Tran" />
                        <asp:BoundField DataField="Fisico" HeaderText="Fis" />
                        <asp:BoundField DataField="Faltante" HeaderText="Faltante" />
                        <asp:BoundField DataField="UNDS" HeaderText="Unidades" />
                        <asp:BoundField DataField="Headerid" HeaderText="# Plan" />
                        <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbDocument" runat="server" CssClass="document-checkbox" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
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

            <!-- jQuery and Bootstrap JS (required for Bootstrap 4) -->
            <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
            <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>

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
                .domain(d3.extent(values))
                .interpolator(d3.interpolateOranges);

            cells1.forEach(function (cell) {
                var value = parseFloat(cell.innerText);
                cell.style.backgroundColor = colorScale1(value);
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
    </form>
</body>
</html>

<!-- AdminLTE App -->
<script src="dist/js/adminlte.js"></script>

<!-- Librerías necesarias -->
<link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />
<link href="https://cdn.datatables.net/buttons/1.7.1/css/buttons.dataTables.min.css" rel="stylesheet" type="text/css" />

<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
<script src="https://cdn.datatables.net/buttons/1.7.1/js/dataTables.buttons.min.js"></script>
<script src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.html5.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>

<script>
    $(document).ready(function () {
        var table = $("#gridDespachosAbiertos").DataTable({
            responsive: true,
            lengthChange: false,
            autoWidth: false,
            deferRender: true,
            bPaginate: false,
            sort: true,
            order: [[2, 'desc']],
            dom: 'Bfrtip',
            buttons: [
                {
                    extend: 'excelHtml5',
                    text: 'Exportar Excel',
                    title: 'Despachos Abiertos',
                    exportOptions: {
                        columns: ':visible'
                    }
                }
            ]
        });

        // Evento del botón HTML5
        $("#btnExportExcel").on("click", function () {
            table.button('.buttons-excel').trigger();
        });
    });
</script>
<%--<!-- DataTables  & Plugins -->
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
    $("#gridDespachosAbiertos").DataTable({
        "responsive": true,
        "lengthChange": false,
        "autoWidth": false,
        "buttons": ["excel"],
        "sort": true,
        "order": [[2, 'desc']],
        "deferRender": true,
        "bPaginate": false,
    }).buttons().container().appendTo('#gridDespachosAbiertos_wrapper .col-md-6:eq(0)');

</script>--%>

<script type="text/javascript">
    document.addEventListener("change", function (event) {
        // Verifica si el evento proviene de un input type="checkbox" dentro de un span con la clase "document-checkbox"
        if (event.target.tagName === "INPUT" && event.target.type === "checkbox" && event.target.closest("span.document-checkbox") && event.target.closest("#gridDespachosAbiertos")) {
            const grid = document.querySelector("#gridDespachosAbiertos");
            const checkboxes = grid.querySelectorAll("span.document-checkbox input[type='checkbox']");

            if (event.target.checked) {
                // Si se marca un checkbox, deshabilita los demás en este grid
                checkboxes.forEach(cb => {
                    if (cb !== event.target) {
                        cb.disabled = true;
                    }
                });
            } else {
                // Si se desmarca, habilita todos los checkboxes nuevamente en este grid
                checkboxes.forEach(cb => cb.disabled = false);
            }
        }
    });
</script>
<script>
    function cerrarPlan(idPlan) {
        Swal.fire({
            title: '¿Está seguro de cerrar esta planificación?',
            text: 'Esta acción no se puede deshacer.',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, cerrar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: "CerrarPlanificacionHandler.ashx",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({ id: idPlan }),
                    success: function (response) {
                        if (response.success) {
                            Swal.fire('Cerrado', 'La planificación se ha cerrado exitosamente.', 'success').then(() => {
                                location.assign("TrasladosDDashboard.aspx");
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
        });
    }
</script>
<script>
    function cerrarDespacho(idPlan) {
        Swal.fire({
            title: '¿Está seguro de cerrar este Despacho?',
            text: 'Esta acción no se puede deshacer.',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, cerrar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: "CerrarDespachoHandler.ashx",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({ id: idPlan }),
                    success: function (response) {
                        if (response.success) {
                            Swal.fire('Cerrado', 'El Despacho se ha cerrado exitosamente.', 'success').then(() => {
                                location.assign("TrasladosDDashboard.aspx");
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
        });
    }
</script>
<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
<%--<script type="text/javascript">
    function confirmarCrearDespacho() {
        Swal.fire({
            title: "Crear nuevo despacho",
            html: `
                    <div class="row" style="display: flex; align-items: center;">
                        <div class="col">Fecha Inicio</div>
                        <div class="col">
                            <input id="fechaInicio" class="swal2-input" type="date">
                        </div>
                    </div>
                    <div class="row" style="display: flex; align-items: center;">
                        <div class="col">Fecha Vencimiento</div>
                        <div class="col">
                            <input id="fechaVence" class="swal2-input" type="date">
                        </div>
                    </div>
                `,
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Crear Despacho",
            cancelButtonText: "Cancelar",
            reverseButtons: true,
            preConfirm: () => {
                const fechaInicio = document.getElementById("fechaInicio").value;
                const fechaVence = document.getElementById("fechaVence").value;

                if (!fechaInicio || !fechaVence) {
                    Swal.showValidationMessage("Debe ingresar ambas fechas.");
                    return false;
                }
                document.getElementById('<%= hfFechasDespacho.ClientID %>').value = fechaInicio + "|" + fechaVence;
                return true;
            }
        }).then((result) => {
            if (result.isConfirmed) {
                __doPostBack('<%= btnCrearDespacho.UniqueID %>', '');
                                            }
                                        });
    }
</script>--%>
