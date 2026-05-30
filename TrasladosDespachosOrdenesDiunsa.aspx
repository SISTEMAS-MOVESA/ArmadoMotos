<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosDespachosOrdenesDiunsa.aspx.vb" Inherits="TrasladosDespachosOrdenesDiunsa" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Indice Desabastecimiento CD</title>
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
    <%--<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>--%>

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
            <div class="row" style="display: flex; justify-content: center; align-content: center; align-items: center;">
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
            </div>
        </div>
        <style>
            .document-checkbox input[type="checkbox"] {
                transform: scale(1.5); /* Aumenta el tamaño del checkbox */
                margin: 0 5px; /* Ajusta el margen si es necesario */
            }
        </style>
        <div style="height: 700px; overflow-y: auto; margin-top: 10px;">
            <div class="container-fluid">
                <div class="row">
                    <div class="col" style="display:flex; justify-content:center;justify-items:center;">
                        <h1>Ordenes Diunsa</h1>
                    </div>
                </div>

                <asp:GridView ID="gridIndiceD" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" Width="100%">
                    <HeaderStyle CssClass="thead-dark sticky-header" />
                    <Columns>
                        <asp:BoundField DataField="DOCNUM" HeaderText="# Documento" />
                        <asp:BoundField DataField="PARTNER_DOCNUM" HeaderText="#Pedido Diunsa" />
                        <asp:BoundField DataField="DOCDATE" HeaderText="Fecha Creacion" />
                        <asp:BoundField DataField="WHSCODE" HeaderText="Codigo Almacen" />
                        <asp:BoundField DataField="WHSNAME" HeaderText="Nombre almacen" />
                        <asp:BoundField DataField="ITEMCODE" HeaderText="Articulo" />
                        <asp:BoundField DataField="ITEMNAME" HeaderText="Descripcio" />
                        <asp:BoundField DataField="ETA" HeaderText="Fecha Deseada" />
                        <asp:BoundField DataField="COMMENTS" HeaderText="Comentarios" />
                        <asp:TemplateField HeaderText="Ver Sugerido">
                            <ItemTemplate>
                                <div style="display: flex; align-items: center; justify-content: center;" class="itm-number-input">
                                    <button type="button"
                                        class="btn btn-info btn-sm mr-2 cerrarPedido"
                                        data-docnum='<%# Eval("DOCNUM") %>'
                                        onclick="cerrar_pedido(<%# Eval("DOCNUM") %>)">
                                        <i class="fas fa-lock"></i><%# Eval("DOCNUM") %>
                                    </button>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
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
<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

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
            order: [[0, "asc"]]
        });
    });
</script>
<script>
    function cerrar_pedido(docnum) {
        Swal.fire({
            title: 'Ingrese los datos',
            html: `
                <div class="ui form">
                    <div class="field">
                        <label># Operacion Pedido Portal:</label>
                        <input class="swal2-input" type="text" value="${docnum}" disabled>
                    </div>
                    <div class="field">
                        <label>Serie:</label>
                        <input id="txt-serie" class="swal2-input" type="text" placeholder="Serie de chasis de motocicleta">
                    </div>
                    <div class="field">
                        <label>Numero Documento:</label>
                        <input id="txt-docnum" class="swal2-input" type="text" placeholder="# Solicitud de traslado">
                    </div>
                    <div class="field">
                        <label>Fecha de Envio:</label>
                        <input id="txt-fecha-envio" class="swal2-input" type="date">
                    </div>
                </div>
                `,
            focusConfirm: false,
            showCancelButton: true,
            allowOutsideClick: false,
            confirmButtonText: "Confirmar",
            preConfirm: () => {
                const transferRequest = document.getElementById("txt-docnum").value;
                const serialNumber = document.getElementById("txt-serie").value;
                const deliveryDate = document.getElementById("txt-fecha-envio").value;

                if (!docnum || !transferRequest || !serialNumber || !deliveryDate) {
                    Swal.showValidationMessage("Todos los campos son obligatorios");
                }

                return {
                    docnum,
                    transferRequest,
                    serialNumber,
                    deliveryDate
                };
            }
        }).then((result) => {
            if (result.isConfirmed) {
                console.log(result.value);

                $.ajax({
                    url: "https://web.grupomovesa.com/portal/modulo_distribuidores/services/diunsa.services.php?token=@WAyEterSOr",
                    method: "POST",
                    dataType: "JSON",
                    data: {
                        request: "cerrar_pedido",
                        data: result.value
                    },
                    success(_response) {
                        if (_response.Success) {
                            location.reload();
                        } else {
                            console.log(_response);
                            Swal.fire("Ha ocurrido un error inesperado...", _response.Message, "error").then(() => {
                                cerrar_pedido(docnum);
                            });
                        }
                    },
                    error(_response) {
                        console.log(_response);
                        Swal.fire("Ha ocurrido un error inesperado...", _response.responseText, "error").then(() => {
                            cerrar_pedido(docnum);
                        });;
                    }
                })
            }
        });
    }
</script>