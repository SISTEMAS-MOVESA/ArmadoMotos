<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosDespachos.aspx.vb" Inherits="TrasladosDespachos" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Traslados Despachos</title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.3.0/css/all.min.css" />
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>
    <script src="https://d3js.org/d3.v6.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
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
        /* Estilo base del botón */
        .btn-group .btn-default {
            background-color: #f8f9fa; /* Color de fondo por defecto */
            border-color: #ccc; /* Color del borde */
            color: #fff; /* Color del texto */
            transition: background-color 0.3s ease; /* Transición suave */
        }
            /* Efecto hover */
            .btn-group .btn-default:hover {
                background-color: #28a745; /* Color de fondo al pasar el mouse (verde) */
                border-color: #28a745; /* Color del borde al pasar el mouse */
                color: #fff; /* Color del texto al pasar el mouse */
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
            <div class="row my-3 text-center">
                    <h3>Despachos Abiertos</h3>
                <div class="col-3">
                    <label for="txtCodigoAlmacen">Filtrar</label>
                    <asp:TextBox ID="txtFilter" runat="server" Text="" placeholder="Escriba Para Buscar"
                        CssClass="form-control txt-Filter" data-table="gridDespachosAbiertos" onkeyup="convertToUppercase('txtFilter')"></asp:TextBox>
                </div>
                <div class="col-3">
                <label for="btnVistaCalendario">Ir al Calendario</label>
                <asp:Button ID="btnVistaCalendario" runat="server" CssClass="btn btn-block btn-info w-100" Text="Calendario" />
                </div>
            </div>
            <div class="row">
                <asp:Label ID="lblPlanId" runat="server" Text="" Visible="false"></asp:Label>
                <asp:GridView ID="gridDespachosAbiertos" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover mt-3"
                    Width="100%" OnRowDataBound="gridDespachosAbiertos_RowDataBound">
                    <HeaderStyle CssClass="thead-dark sticky-header" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/view.png" Text="Ver" CommandName="Ver" HeaderText="Ver">
                            <ControlStyle Height="30px" Width="30" />
                            <ItemStyle Wrap="False" />
                        </asp:ButtonField>
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/delete.png" Text="Cerrar" CommandName="Cerrar" HeaderText="Cerrar">
                            <ControlStyle Height="30px" Width="30" />
                            <ItemStyle Wrap="False" />
                        </asp:ButtonField>
                        <%--<asp:TemplateField HeaderText="Eliminar Despacho">
                            <ItemTemplate>
                                <button type="button" class="btn btn-danger" onclick='eliminarDespacho(<%# Eval("PLANID") %> , <%# Eval("DespachoId") %>)'>Eliminar</button>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Progreso de Picking">
                            <ItemTemplate>
                                <div class="progress">
                                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar"
                                        aria-valuemin="0"
                                        aria-valuenow='<%# Eval("Preparado") %>'
                                        aria-valuemax='<%# Eval("Unds") %>'
                                        style='width: <%# Eval("PorcentajePick") %>%'>
                                        <%# FormatNumber(Eval("PorcentajePick"), 2, TriState.True) %>%
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Progreso de Carga">
                            <ItemTemplate>
                                <div class="progress">
                                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar"
                                        aria-valuemin="0"
                                        aria-valuenow='<%# Eval("Cargado") %>'
                                        aria-valuemax='<%# Eval("Unds") %>'
                                        style='width: <%# Eval("PorcentajeCarga") %>%'>
                                        <%# FormatNumber(Eval("PorcentajeCarga"), 2, TriState.True) %>%
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DespachoId" HeaderText="Despacho ID" />
                        <asp:BoundField DataField="PlanId" HeaderText="Plan ID" />
                        <asp:BoundField DataField="FechaDespacho" HeaderText="Fecha Despacho" />
                        <asp:BoundField DataField="DiasRestantes" HeaderText="Dias Restantes" />
                        <asp:BoundField DataField="Canal" HeaderText="Canal" />
                        <asp:BoundField DataField="Ruta" HeaderText="Ruta" />
                        <asp:BoundField DataField="Almacenes" HeaderText="Almacenes" />
                        <asp:BoundField DataField="TotalAlm" HeaderText="Total Alm" />
                        <asp:BoundField DataField="TotalFalt" HeaderText="Faltante" />
                        <asp:BoundField DataField="Unds" HeaderText="Total Unds" />
                        <asp:BoundField DataField="TotalEspacios" HeaderText="Total Espacios" />
                        <asp:BoundField DataField="Preparado" HeaderText="Preparado" />
                        <asp:BoundField DataField="Cargado" HeaderText="Cargado" />
                        <%--<asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbDocument" runat="server" CssClass="document-checkbox" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                </asp:GridView>
                <script>
                    function eliminarDespacho(idPlan, despachoId) {
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
                                    data: JSON.stringify({ id: idPlan, despachoId }),
                                    success: function (response) {
                                        if (response.success) {
                                            Swal.fire('Cerrado', 'El Despacho se ha cerrado exitosamente.', 'success').then(() => {
                                                location.assign("TrasladosDespachos.aspx");
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
            </div>
            <div class="row my-2">
                <div class="col-1">
                    <asp:Label ID="lblCurrentDespacho" runat="server"></asp:Label>
                </div>
                <div class="col-1">
                    <asp:Label ID="lblIdPlan" runat="server"></asp:Label>
                </div>
                <div class="col-1">
                    <asp:Label ID="lblRuta" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row my-2">
                <h3>Almacenes en Despacho</h3>
                <asp:GridView ID="gridAlmacenesDespachos" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered table-hover"
                    Width="100%" >
                    <HeaderStyle CssClass="thead-dark sticky-header" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/delete.png" Text="Eliminar" CommandName="Eliminar" HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ControlStyle Height="30px" Width="30px" />
                            <ItemStyle Wrap="False" />
                        </asp:ButtonField>
                        <%--<asp:ButtonField ButtonType="Image" ImageUrl="~/Images/chargeback.png" Text="Devolver" CommandName="Devolver" HeaderText="Devolver" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ControlStyle Height="30px" Width="30px" />
                            <ItemStyle Wrap="False" />
                        </asp:ButtonField>--%>
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
<!-- jQuery -->
<script src="plugins/jquery/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.11.0/umd/popper.min.js" integrity="sha384-b/U6ypiBEHpOf/4+1nzFpr53nxSS+GLCkfwBdFNTxtclqqenISfwAzpKaMNFNmj4" crossorigin="anonymous"></script>

<!-- AdminLTE App -->
<%--<script src="dist/js/adminlte.js"></script>--%>


<!-- DataTables  & Plugins -->
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

<!-- DataTables  & Plugins -->
<script src="plugins/datatables/jquery.dataTables.min.js"></script>
<script src="plugins/datatables-bs4/js/dataTables.bootstrap4.min.js"></script>
<script src="plugins/datatables-responsive/js/dataTables.responsive.min.js"></script>
<script src="plugins/datatables-responsive/js/responsive.bootstrap4.min.js"></script>
<script src="plugins/datatables-buttons/js/dataTables.buttons.min.js"></script>
<script src="plugins/datatables-buttons/js/buttons.bootstrap4.min.js"></script>
<script src="plugins/jszip/jszip.min.js"></script>
<script src="plugins/pdfmake/pdfmake.min.js"></script>
<script src="plugins/pdfmake/vfs_fonts.js"></script>
<script src="plugins/datatables-buttons/js/buttons.html5.min.js"></script>
<script src="plugins/datatables-buttons/js/buttons.print.min.js"></script>
<script src="plugins/datatables-buttons/js/buttons.colVis.min.js"></script>




<script>
    document.addEventListener("DOMContentLoaded", function () {
        // Selecciona los elementos .btn-group que tienen los atributos necesarios
        document.querySelectorAll('.btn-group[data-planid][data-destino][data-ruta][data-canal]').forEach(function (group) {
            // Obtener los atributos correctamente
            const headerId = group.getAttribute('data-headerid');
            const planId = group.getAttribute('data-planid');
            const destino = group.getAttribute('data-destino');
            const ruta = group.getAttribute('data-ruta');
            const canal = group.getAttribute('data-canal');

            // Obtener el elemento <a> dentro del grupo
            const linkElement = group.querySelector('a');

            if (linkElement) {
                if (canal === "CI") {
                    linkElement.href = `TrasladosCargaMacroIndirecto.aspx?headerId=${headerId}&planId=${planId}&almacen=${destino}&ruta=${ruta}&canal=${canal}`;
                } else {
                    linkElement.href = `TrasladosCargaMacro.aspx?headerId=${headerId}&planId=${planId}&almacen=${destino}&ruta=${ruta}&canal=${canal}`;
                }
            }
        }); // Cierre del forEach
    }); // Cierre del DOMContentLoaded
</script>

<script type="text/javascript">

    function convertToUppercase(elementId) {
        var textBox = document.getElementById(elementId);
        textBox.value = textBox.value.toUpperCase();
    }

    $("#gridAlmacenesDespachos").DataTable({
        "responsive": true,
        "lengthChange": false,
        "autoWidth": false,
        "buttons": ["excel"],
        "sort": true,
        "order": [[0, 'asc']],
        "deferRender": true,
        "bPaginate": false,
        //"lengthMenu": [[5, -1], [5, "All"]],
    }).buttons().container().appendTo('#gridAlmacenesDespachos_wrapper .col-md-6:eq(0)');
</script>
<script>
    $(".txt-Filter[data-table]").on('keyup', function () {
        var value = $(this).val().toLowerCase();
        const table = $(this).attr("data-table");

        $(`#${table} tbody tr`).filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });
</script>
<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
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

    setInterval(autoRefresh, 300000); // 5 minutos
</script>

<%--<script>
    // Define a function to refresh the page
    function autoRefresh() {
        window.location.reload(); // Reloads the current page
    }

    // Set an interval to call the autoRefresh function every 5 minutes
    // 5 minutes * 60 seconds/minute * 1000 milliseconds/second = 300000 milliseconds
    setInterval(autoRefresh, 300000);
</script>--%>