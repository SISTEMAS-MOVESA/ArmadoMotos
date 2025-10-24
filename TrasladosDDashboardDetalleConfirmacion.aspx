<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosDDashboardDetalleConfirmacion.aspx.vb" Inherits="TrasladosDDashboardDetalleConfirmacion" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
    <!-- jQuery primero -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <!-- Luego Bootstrap JS (usa la versión que coincida con tu Bootstrap CSS) -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css">


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

        .ui[class*="right ribbon"].label {
            left: calc(100% - 1rem - 7em) !important;
            padding-left: 1.2em;
            padding-right: calc(1rem + 1.2em);
        }

        /*     .ui.ui.raised.segment, .ui.ui.raised.segments {
            width: 150px;
        }*/


        .heatmap-cell1, .heatmap-cell2 {
            font-weight: bold;
            font-size: large;
            color: white;
            text-shadow: 1px 1px black;
            text-align: center;
            align-content: center;
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
            font-size:larger;
            font-weight:400;
        }

        .modalPanel {
            background-color: lightgray;
            padding: 25px;
            border: 2px double solid black;
            border-radius: 8px;
            max-width: 50%;
            max-height: 90vh;
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
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
        <div class="container-fluid my-3" style="display:flex; justify-content:center; justify-items:center">
            <table id="tbl-capacidad-instalada" class="table table-sm table-bordered table-hover w-50">
        <thead class="thead-dark">
            <tr>
                <th>Capacidad Instalada</th>
                <th class="text-center">LU</th>
                <th class="text-center">MA</th>
                <th class="text-center">MI</th>
                <th class="text-center">JU</th>
                <th class="text-center">VI</th>
                <th class="text-center">SA</th>
                <th class="text-center">DO</th>
            </tr>
        </thead>
        <tbody></tbody>
    </table>
        </div>
        <div class="ui grid mx-3">
            <div class="nine wide column" id="columna2">
                <h3>Motos Portal Pedidos (Facturadas / Entregas)</h3>
                <div class="container-divs my-2">
                    <div>
                        <asp:Label ID="lblUnidadesPortalPedidos" runat="server" Text="Unidades: 0"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblEspaciosPotalPedidos" runat="server" Text="Espacios: 0"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblTotRows" runat="server" Text="Lineas: 0"></asp:Label>
                    </div>
                    <div>Fecha Despacho:</div>
                    <div>
                        <asp:TextBox ID="txtFechaDespacho" runat="server" type="date" CssClass="form-control" Width="100%"></asp:TextBox>
                    </div>
                    <div>
                        <asp:TextBox ID="txtFilter" runat="server" Text="" placeholder="Filtrar Por Modelo"
                            CssClass="form-control txt-buscador" data-table="gridMotosPortalPedidos" onkeyup="convertToUppercase('txtFilter')"></asp:TextBox>
                    </div>
                    <script>
                        $(".txt-buscador[data-table]").on('keyup', function () {
                            var value = $(this).val().toLowerCase();
                            const table = $(this).attr("data-table");

                            $(`#${table} tbody tr`).filter(function () {
                                $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
                            });
                        });
                    </script>
<%--                    <div class="container-divs my-2">
                        <div class="col-3">
                        <asp:Button ID="eliminarTodaslasFilas" runat="server" CssClass="btn btn-block btn-danger" Text="Eliminar Todas" />
                        </div>
                    </div>--%>
                </div>
                <div id="contenido2" style="overflow-y: auto; max-height: 600px; width: 100%">
                        <asp:GridView ID="gridMotosPortalPedidos" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered table-hover" Width="100%"
                            HeaderStyle-Font-Size="12px"
                            EmptyDataText="No data" CellPadding="4"
                            ForeColor="#333333" GridLines="None">
                            <HeaderStyle CssClass="thead-dark" />
                            <Columns>
                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbDocument" runat="server" CssClass="document-checkbox" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="seven wide column" id="columna1">
                    <h3>Motos Despacho</h3>
                    <div class="container-divs">
                    <div>
                        Motos Agregadas
                    </div>
                    <div>
                        <asp:Label ID="lblUnidades" runat="server" Text="Unidades: 0"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblEspacios" runat="server" Text="Espacios: 0"></asp:Label>
                    </div>
                </div>
                <div id="contenido1" style="overflow-y: auto; max-height: 600px; width: 100%">
                    <asp:GridView ID="gridPedidoTemp" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" Width="100%">
                        <HeaderStyle CssClass="thead-dark" />
                        <Columns>
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/delete.png" Text="Eliminar" CommandName="Eliminar" HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ControlStyle Height="20px" Width="20px" />
                                <ItemStyle Wrap="False" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="RUTA" HeaderText="Ruta" />
                            <asp:BoundField DataField="ID" HeaderText="Id" />
                            <asp:BoundField DataField="ALMDESTINO" HeaderText="Alm" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="ARTICULO" HeaderText="Articulo" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="MODELO" HeaderText="Modelo" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="ESPACIOS" HeaderText="Espacios" />
                            <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" />
                            <asp:BoundField DataField="QTYLOGISTICA" HeaderText="Logistica" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="row mx-3">
               <%-- <div class="col-2">
                    <label for="txtFechaDespacho">Fecha Despacho</label>
                    <asp:TextBox ID="txtFechaDespacho" runat="server" type="date" CssClass="form-control" Width="100%"></asp:TextBox>
                </div>--%>
               <%-- <div class="col-2">
                    <asp:Button ID="btnCrearDespacho" runat="server" CssClass="btn btn-primary" Text="Crear Despacho" />
                </div>--%>
            </div>
        </div>
        <div id="contenido4" style="overflow-y: auto; max-height: 300px; width: auto">
            <div class="row mx-3 my-2">
                <div class="col">
                    <asp:Button ID="btnRegresar" runat="server" CssClass="btn btn-block btn-warning" Text="Regresar" />
                </div>
                <div class="col">
                    <asp:Button ID="btnContinuar" runat="server" CssClass="btn btn-block btn-info" Text="Crear Despacho" />
                </div>
            </div>
        </div>
 
        <center>
            <div class="container">
                <footer class="main-footer">
                    <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
                </footer>
            </div>
        </center>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.css" />
        <script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.js" integrity="sha512-gnoBksrDbaMnlE0rhhkcx3iwzvgBGz6mOEj4/Y5ZY09n55dYddx6+WYc72A55qEesV8VX2iMomteIwobeGK1BQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.css" integrity="sha512-3quBdRGJyLy79hzhDDcBzANW+mVqPctrGCfIPosHQtMKb3rKsCxfyslzwlz2wj1dT8A7UX+sEvDjaUv+WExQrA==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    </form>
</body>
</html>

<!-- AdminLTE App -->
<script src="dist/js/adminlte.js"></script>


<!-- DataTables  & Plugins -->
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
<link rel="stylesheet" href="https://cdn.datatables.net/fixedcolumns/4.3.0/css/fixedColumns.dataTables.min.css" />
<script src="https://cdn.datatables.net/fixedcolumns/4.3.0/js/dataTables.fixedColumns.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
<script src="index.js"></script>

<script>
    function cargar_capacidad_instalada() {

        $("#screen_loader").show();
        $.ajax({
            url: "http://192.168.1.70/portal/modulo_logistica/services/portalArmadoMotos.services.php?token=@WAyEterSOr",
            method: "POST",
            dataType: "JSON",
            data: { request: "resumen_capacidad_instalada_semana" },
            success(_response) {
                console.log(_response);

                $("#screen_loader").hide();
                $("#tbl-capacidad-instalada tbody ").empty();

                _response.forEach(item => {
                    const _class = item.CONCEPTO == "DISPONIBLES" ? "active" : "";
                    $("#tbl-capacidad-instalada tbody").append(
                        `<tr class="${_class}">
		                <td>${item.CONCEPTO}</td>
		                <td class="text-center">${__FORMATOS.UNIDADES(item.LU)}</td>
		                <td class="text-center">${__FORMATOS.UNIDADES(item.MA)}</td>
		                <td class="text-center">${__FORMATOS.UNIDADES(item.MI)}</td>
		                <td class="text-center">${__FORMATOS.UNIDADES(item.JU)}</td>
		                <td class="text-center">${__FORMATOS.UNIDADES(item.VI)}</td>
		                <td class="text-center">${__FORMATOS.UNIDADES(item.SA)}</td>
		                <td class="text-center">${__FORMATOS.UNIDADES(item.DO)}</td>
	                </tr>`
                    );
                });

            },
            error(_response) {
                $("#screen_loader").hide();
                console.log(_response);
            },
        });
    }

    cargar_capacidad_instalada();
</script>

<%--<script type="text/javascript">
    $("#gridMotosPortalPedidos").DataTable({
        "responsive": true,
        "lengthChange": false,
        "autoWidth": false,
        "buttons": ["excel"],
        "sort": true,
        "order": [[0, 'asc']],
        "deferRender": true,
        "bPaginate": false,
        //"lengthMenu": [[5, -1], [5, "All"]],
    }).buttons().container().appendTo('#gridMotosPortalPedidos_wrapper .col-md-6:eq(0)');

</script>--%>


<%--<script type="text/javascript">
    function marcarCoincidencias() {
        // Obtener los valores de la columna 2 (índice 1) del gridPedidoTemp
        var valoresPedidoTemp = [];
        $('#<%= gridPedidoTemp.ClientID %> tr').each(function () {
            var celda = $(this).find('td:eq(1)');
            if (celda.length > 0) {
                valoresPedidoTemp.push(celda.text().trim());
            }
        });

        // Recorrer gridMotosPortalPedidos y marcar coincidencias
        $('#<%= gridMotosPortalPedidos.ClientID %> tr').each(function () {
            var celda = $(this).find('td:eq(1)');
            if (celda.length > 0) {
                var valor = celda.text().trim();
                if (valoresPedidoTemp.includes(valor)) {
                    $(this).css('background-color', 'lightgreen');
                }
            }
        });
    }

    // Ejecutar al cargar la página
    $(document).ready(function () {
        marcarCoincidencias();
    });
</script>--%>

<script type="text/javascript">
    function marcarCoincidencias() {
        // Obtener los valores de la columna 2 (índice 1) del gridPedidoTemp
        var valoresPedidoTemp = [];
        $('#<%= gridPedidoTemp.ClientID %> tr').each(function () {
        var celda = $(this).find('td:eq(1)');
        if (celda.length > 0) {
            valoresPedidoTemp.push(celda.text().trim());
        }
    });

    // Recorrer gridMotosPortalPedidos: mostrar solo coincidencias, ocultar el resto
        $('#<%= gridMotosPortalPedidos.ClientID %> tr').each(function () {
            var celda = $(this).find('td:eq(1)');
            if (celda.length > 0) {
                var valor = celda.text().trim();
                if (valoresPedidoTemp.includes(valor)) {
                    $(this).css('background-color', 'lightgreen');
                    $(this).addClass("selected-row");
                    $(this).show(); // Asegurar que se muestra
                } else {
                    $(this).hide(); // Ocultar si no hay coincidencia
                }
            }
        });
    }

    <%-- function marcarCoincidencias() {
        // Obtener los valores de la columna 2 (índice 1) del gridPedidoTemp
        var valoresPedidoTemp = [];
        $('#<%= gridPedidoTemp.ClientID %> tr').each(function () {
            var celda = $(this).find('td:eq(1)');
            if (celda.length > 0) {
                valoresPedidoTemp.push(celda.text().trim());
            }
        });

        // Recorrer gridMotosPortalPedidos y marcar coincidencias
        $('#<%= gridMotosPortalPedidos.ClientID %> tr').each(function () {
            var celda = $(this).find('td:eq(1)');
            if (celda.length > 0) {
                var valor = celda.text().trim();
                if (valoresPedidoTemp.includes(valor)) {
                    $(this).css('background-color', 'lightgreen');
                    $(this).addClass("selected-row");
                }
            }
        });
    }--%>
    //sumar los espacios ya comprometidos
    function sumarEspacios() {
        let total = 0;
        let unidades = 0;
        let totalRows = 0;
        $("#gridMotosPortalPedidos tbody tr.selected-row").each(function () {
            let espacio = parseInt($(this).find("td:eq(12)").html())
            total += isNaN(espacio) ? 0 : espacio;
            unidades++
            totalRows++
        })
        console.log(total)
        $('#lblEspaciosPotalPedidos').html(`Espacios: ${total}`);
        $('#lblUnidadesPortalPedidos').html(`Unidades: ${unidades}`);
        $('#lblTotRows').html(`Lineas: ${totalRows}`);
    }

    // Ejecutar al cargar la página
    $(document).ready(function () {
        //marcarCoincidencias();
        sumarEspacios();
    });
</script>
