<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladoTareaCargaCamion.aspx.vb" Inherits="TrasladoTareaCargaCamion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Traslados || Preparar</title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback">
    <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css">
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
    <style>
        .center {
            margin: auto;
            width: 50%;
            /*border: 3px solid green;*/
            padding: 10px;
        }
    </style>

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
                                <a class="dropdown-item" href="TrasladosPresolicitudesAbiertas.aspx">Sugerido Confirmado</a>
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
                    </ul>
                </div>
            </div>
        </nav>

        <br />

        <div class="center">
            <div class="row">
                <div class="col">
                    <asp:ListBox ID="lstSeriesPreparadas" runat="server" SelectionMode="Multiple" class="demo" Height="400px" Width="100%"></asp:ListBox>
                    <link rel="stylesheet" type="text/css" href="dual-listbox.css" />
                    <script type="text/javascript" src="dual-listbox.js"></script>
                    <script type="text/javascript">
                        new DualListbox('.demo', {
                            addEvent: function (value) { },
                            removeEvent: function (value) { },
                            availableTitle: 'Seires Disponibles',
                            selectedTitle: 'Seires Seleccionados',
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

        <%--INICIO DETALLES DE SOLICITUD--%>
        <section class="content">
            <div class="container-fluid">
                <!-- Main row -->
                <br />
                <div class="row">
                    <!-- Left col -->
                    <section class="col-lg-12 connectedSortable">
                        <!-- Custom tabs (Charts with tabs)-->
                        <div class="card">
                            <div class="card-header">
                                <h3 class="card-title">Resumen Solicitud </h3>
                                <div class="card-tools">
                                    <div class="input-group input-group-sm" style="width: 100px;">
                                        <i class="fas fa-ticket-alt fa-2x"></i>

                                    </div>
                                </div>
                            </div>
                            <!-- /.card-header -->
                            <br />
                            <div class="card-body table-responsive p-0" style="height: 100%;">
                                <div class="container-fluid">
                                    <div class="col">
                                        <asp:GridView ID="gridSolicitudAbierta" runat="server" CssClass="table table-bordered table-hover" AutoGenerateColumns="false">
                                            <HeaderStyle CssClass="thead-dark" />
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderText="Id" />
                                                <asp:BoundField DataField="WHSCODE" HeaderText="Cod. Alm." />
                                                <asp:BoundField DataField="WHSNAME" HeaderText="Almacen" />
                                                <asp:BoundField DataField="MODELO" HeaderText="Modelo" />
                                                <asp:BoundField DataField="QTYSOLICITADA" HeaderText="Solicitado" />
                                                <asp:BoundField DataField="OBSERVACIONES" HeaderText="Observaciones" />
                                                <asp:BoundField DataField="DATECREATED" HeaderText="Fecha / Hora" />
                                                <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/delete.png" Text="Eliminar" CommandName="Eliminar" HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <ControlStyle Height="30px" Width="30px" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:ButtonField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /.card-body -->
                        <!-- /.card -->
                    </section>
                </div>
            </div>
        </section>
        <%--FIN DETALLES DE SOLICITUD--%>
        <div class="row">
            <div class="col">
                <asp:Button ID="btnCrearCamion" runat="server" class="btn btn-success btn-block" Text="Crear Solicitud" UseSubmitBehavior="False" />
            </div>
        </div>

        <br>
        <center>
          <div class="container">
              <footer class="main-footer">
                  <strong> <i class="ion-paintbrush"></i> WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
              </footer>
          </div>
      </center>

        <asp:ScriptManager ID="sm1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

        <!-- jQuery -->
        <script src="plugins/jquery/jquery.min.js"></script>
        <script src="vendor/bootstrap-4.1/popper.min.js"></script>
        <!-- Bootstrap 4 -->
        <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
        <!-- ChartJS -->
        <script src="plugins/chart.js/Chart.min.js"></script>
        <!-- Sparkline -->
        <script src="plugins/sparklines/sparkline.js"></script>
        <!-- JQVMap -->
        <script src="plugins/jqvmap/jquery.vmap.min.js"></script>
        <script src="plugins/jqvmap/maps/jquery.vmap.usa.js"></script>
        <!-- jQuery Knob Chart -->
        <script src="plugins/jquery-knob/jquery.knob.min.js"></script>
        <!-- daterangepicker -->
        <script src="plugins/moment/moment.min.js"></script>
        <script src="plugins/daterangepicker/daterangepicker.js"></script>
        <!-- Tempusdominus Bootstrap 4 -->
        <script src="plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>
        <!-- Summernote -->
        <script src="plugins/summernote/summernote-bs4.min.js"></script>
        <!-- overlayScrollbars -->
        <script src="plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js"></script>
        <!-- AdminLTE App -->
        <script src="dist/js/adminlte.js"></script>

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


        <link href="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/css/toastr.min.css" rel="stylesheet" />
        <script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js"></script>
        <script type="text/javascript">
            function showContent(typeofMsg, mensaje) {
                toastr.options = {
                    "closeButton": true,
                    "debug": false,
                    "progressBar": true,
                    "preventDuplicates": false,
                    "positionClass": "toast-top-right",
                    "showDuration": "400",
                    "hideDuration": "1000",
                    "timeOut": "7000",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                }
                toastr[typeofMsg](mensaje);
            }
        </script>
    </form>


</body>
</html>

<link href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.css" rel="stylesheet" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.js"></script>

<script>
    $("#drpModelosMoto").dropdown(
        {
            "fullTextSearch": true
        });
    $("#drpMotoristas").dropdown(
        {
            "fullTextSearch": true
        });
    $("#drpMotivoTraslado").dropdown(
        {
            "fullTextSearch": true
        });
</script>

<script type="text/javascript">
    //Datatable responsive tables:
    $("#gridSolicitudAbierta").DataTable({
        "responsive": true,
        "lengthChange": false,
        "autoWidth": false,
        "buttons": ["excel", "pdf", "colvis"],
        "sort": true,
        "order": [[0, 'asc']],
        "deferRender": true,
        "lengthMenu": [[20, -1], [20, "All"]],
    }).buttons().container().appendTo('#gridSolicitudAbierta_wrapper .col-md-6:eq(0)');

    $("#gridPresolicitudes").DataTable({
        "responsive": true,
        "lengthChange": false,
        "autoWidth": false,
        "buttons": ["excel", "pdf", "colvis"],
        "sort": true,
        "order": [[0, 'asc']],
        "deferRender": true,
        "lengthMenu": [[20, -1], [20, "All"]],
    });
</script>

