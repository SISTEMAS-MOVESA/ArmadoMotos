<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VisorLogistica.aspx.vb" Inherits="VisorLogistica" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Confirmacion Logistica</title>
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
    <link href="css/TasksCards.css" rel="stylesheet" />

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
                            <a class="nav-link active" aria-current="page" href="TrasladosDashboard.aspx">Inicio</a>
                        </li>
                        <li class="nav-item dropdown">
                            <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Pre Solicitudes
                            </a>
                            <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                                <a class="dropdown-item" href="TrasladosPreSolicitud.aspx">Pre Solicitud</a>
                                <a class="dropdown-item" href="TrasladosCargaMacro.aspx">Presolicitudes Abiertas</a>
                                <div class="dropdown-divider"></div>
                                <a class="dropdown-item" href="TrasladosConfirmarPreSucursal.aspx">Confirmacion Presolicitud Sucursal</a>
                            </div>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="TrasladosSolicitud.aspx">Solicitud</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="TrasladosForklift.aspx">Armado de Moto</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="TrasladosPreparar.aspx">Carga Camion</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="TrasladosCrearDocumentos.aspx">Generar Traslados</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="TrasladosCrearDocumentos.aspx">Gestion de Transferencias</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="Default.aspx">Cerrar Sesion</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
        <br />
        <br />
        <div class="d-flex justify-content-center">
            <h2><b>Id:
                <asp:Label ID="lblIdMacro" runat="server" Text="" Style="font-size: 2vw; color: red;"></asp:Label>
                | 
             Almacen Destino:
                <asp:Label ID="lblAlmacenDestino" runat="server" Text="" Style="font-size: 2vw; color: red;"></asp:Label>
                |
             Ruta:
                <asp:Label ID="lblRuta" runat="server" Text="" Style="font-size: 2vw; color: red;"></asp:Label>
            </b></h2>
        </div>
        <hr />
     
        <div class="d-flex justify-content-center">
        <h2> HISTORIAL </h2>
            </div>
        <div class="d-flex justify-content-center">
            <asp:GridView ID="grisHistorial" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" Width="100%">
                <HeaderStyle CssClass="thead-dark" />
                <Columns>
                    <asp:BoundField DataField="MODELO" HeaderText="Modelo" />
                    <asp:BoundField DataField="ITEMCODE" HeaderText="Articulo" />
                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion" />
                    <asp:BoundField DataField="OBSERVACIONES" HeaderText="Observaciones" />
                    <asp:BoundField DataField="QTYFINAL" HeaderText="Cantidad" ItemStyle-HorizontalAlign="Center" />
                    <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/bike.png" Text="Seleccionar" CommandName="Seleccionar" HeaderText="Seleccionar">
                        <ControlStyle Height="30px" Width="30" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center" />
                    </asp:ButtonField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="container-fluid">
            <div class="row">
                <div class="col-12">
                    <asp:Button ID="btnBack" runat="server" CssClass="btn btn-block btn-info" Text="Regresar" />
                </div>
            </div>
        </div>
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
                    "positionClass": "toast-bottom-right",
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
<script type="text/javascript">
    $("#grisHistorial").DataTable({
        "responsive": true,
        "lengthChange": false,
        "autoWidth": false,
        "buttons": ["excel"],
        "sort": false,
        "order": [[0, 'asc']],
        "deferRender": true,
        "bPaginate": false,
        //"lengthMenu": [[5, -1], [5, "All"]],
    }).buttons().container().appendTo('#grisHistorial_wrapper .col-md-6:eq(0)');
</script>

<link href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.css" rel="stylesheet" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.js"></script>
