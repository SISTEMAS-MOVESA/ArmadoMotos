<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DashboardAuditoria.aspx.vb" Inherits="DashboardAuditoria" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Auditoria || Dashboard</title>
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
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/2.9.2/umd/popper.min.js" integrity="sha512-2rNj2KJ+D8s1ceNasTIex6z4HWyOnEYLVC3FigGOmyQCZc2eBXKgOxQmo3oKLHyfcj53uz4QMsRCWNbLd32Q1g==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>


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
        <div class="container-fluid mt-5">
            <div class="row" style="flex; justify-content: center">
                <div class="col-12 col-sm-6 col-md-6 col-lg-2 col-xl-2">
                    <div class="small-box bg-info">
                        <div class="inner">
                            <a href="#" style="background: black; color: white;">
                                <h3>
                                    <asp:Label ID="lblMotosEnCaja" runat="server" Text=""></asp:Label></h3>
                            </a>
                            <p>Inventario Motos Armadas</p>
                        </div>
                        <div class="icon">
                            <i class="fa fa-motorcycle" aria-hidden="true"></i>
                        </div>
                        <a href="AuditoriaInventarioArmadas.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-6 col-lg-2 col-xl-2">
                    <div class="small-box bg-secondary">
                        <div class="inner">
                            <a href="#" style="background: black; color: white;">
                                <h3>
                                    <asp:Label ID="lblControlCalidad" runat="server" Text=""></asp:Label></h3>
                            </a>
                            <p>Infomes de Auditoria</p>
                        </div>
                        <div class="icon">
                            <i class="fa fa-list" aria-hidden="true"></i>
                        </div>
                        <a href="AuditoriaInformeInventario.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-6 col-lg-2 col-xl-2">
                    <div class="small-box bg-info">
                        <div class="inner">
                            <a href="#" style="background: black; color: white;">
                                <h3>
                                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label></h3>
                            </a>
                            <p>Auditoria Motos Retornadas</p>
                        </div>
                        <div class="icon">
                           <i class="fa fa-undo" aria-hidden="true"></i>
                        </div>
                        <a href="AuditoriaMotoReingreso.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-6 col-lg-2 col-xl-2">
                    <div class="small-box bg-secondary">
                        <div class="inner">
                            <a href="#" style="background: black; color: white;">
                                <h3>
                                    <asp:Label ID="Label4" runat="server" Text=""></asp:Label></h3>
                            </a>
                            <p>Informe Motos Retornadas</p>
                        </div>
                        <div class="icon">
                           <i class="fa fa-list" aria-hidden="true"></i>
                        </div>
                        <a href="AuditoriaInformeMotosRetornadas.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>

            </div>
            <div class="row" style="flex; justify-content: center">
                <div class="col-12 col-sm-6 col-md-6 col-lg-2 col-xl-2">
                    <div class="small-box bg-info">
                        <div class="inner">
                            <a href="#" style="background: black; color: white;">
                                <h3>
                                    <asp:Label ID="Label2" runat="server" Text=""></asp:Label></h3>
                            </a>
                            <p>Cobro Repuestos</p>
                        </div>
                        <div class="icon">
                            <i class="fa fa-usd" aria-hidden="true"></i>
                        </div>
                        <a href="AuditoriaCobroRepuestos.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-6 col-lg-2 col-xl-2">
                    <div class="small-box bg-secondary">
                        <div class="inner">
                            <a href="#" style="background: black; color: white;">
                                <h3>
                                    <asp:Label ID="Label3" runat="server" Text=""></asp:Label></h3>
                            </a>
                            <p>Historial de Cobros</p>
                        </div>
                        <div class="icon">
                           <i class="fa fa-list" aria-hidden="true"></i>
                        </div>
                        <a href="AuditoriaInformeCobros.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <center>
            <div class="container">
                <footer class="main-footer">
                    <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
                </footer>
            </div>
        </center>

        <!-- jQuery -->
        <script src="plugins/jquery/jquery.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.11.0/umd/popper.min.js" integrity="sha384-b/U6ypiBEHpOf/4+1nzFpr53nxSS+GLCkfwBdFNTxtclqqenISfwAzpKaMNFNmj4" crossorigin="anonymous"></script>


        <script src="plugins/jquery-ui/jquery-ui.min.js"></script>
        <script>
            $.widget.bridge('uibutton', $.ui.button)
        </script>
        <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
        <script src="plugins/chart.js/Chart.min.js"></script>
        <script src="plugins/sparklines/sparkline.js"></script>
        <script src="plugins/jqvmap/jquery.vmap.min.js"></script>
        <script src="plugins/jqvmap/maps/jquery.vmap.usa.js"></script>
        <script src="plugins/jquery-knob/jquery.knob.min.js"></script>
        <script src="plugins/moment/moment.min.js"></script>
        <script src="plugins/daterangepicker/daterangepicker.js"></script>
        <script src="plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>
        <script src="plugins/summernote/summernote-bs4.min.js"></script>
        <script src="plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js"></script>

        <script src="dist/js/adminlte.js"></script>
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>

    </form>


</body>
</html>
<!-- jQuery -->
<script src="plugins/jquery/jquery.min.js"></script>
<!-- jQuery UI 1.11.4 -->
<script src="plugins/jquery-ui/jquery-ui.min.js"></script>
<!-- Resolve conflict in jQuery UI tooltip with Bootstrap tooltip -->
<script>
    $.widget.bridge('uibutton', $.ui.button)
</script>
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

<script type="text/javascript">
    //Datatable responsive tables:
    $("#GridView1").DataTable({
        "responsive": true,
        "lengthChange": false,
        "autoWidth": false,
        "buttons": ["excel", "pdf", "colvis"],
        "sort": true,
        "order": [[0, 'asc']],
        "deferRender": true,
        "paging": false
    }).buttons().container().appendTo('#GridView1_wrapper .col-md-6:eq(0)');
</script>

