<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MaindashboardSucursales.aspx.vb" Inherits="MaindashboardSucursales" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Ensamble Motos || Dashboard</title>
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


</head>f
<body>


    <form id="form1" runat="server">

 <!-- Navigation -->
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark static-top">
            <div class="container">
                <a class="navbar-brand" href="MaindashboardSucursales.aspx">
                    <img src="Imagenes/mnegra.png" width="50" height="40" alt="" />
                </a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarSupportedContent">
                    <ul class="navbar-nav ms-auto">
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="MaindashboardSucursales.aspx">Inicio</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="TrasladosPreSolicitud.aspx">Pre Solicitud</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="TrasladosConfirmarPreSucursal.aspx">Confirmacion Sugerido</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="CuadroBasicoSucursales.aspx">Cuadro Basico Sucursal</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="Default.aspx">Cerrar Sesion</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
        <br />
        <hr />
        <center>
            <h2>Jefe Tienda: <%=Session("UserCode") %> |
                     <%=Session("Name") %>
                    | <%=Session("ALMTRANSIT") %> | <%=Session("CODIGOSUP") %></h2>
        </center>
        <br />
        <div class="d-flex justify-content-center" style="gap: 20px;">
            <div class="card border-success mb-3" style="max-width: 18rem;">
                <div class="card-header bg-transparent border-danger">Pre Solicitud</div>
                <div class="card-body text-success">
                </div>
                <div class="card-footer bg-danger border-warning">
                    <a href="TrasladosPreSolicitud.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="card border-success mb-3" style="max-width: 18rem;">
                <div class="card-header bg-transparent border-info">Confirmacion </div>
                <div class="card-body text-success">
                </div>
                <div class="card-footer bg-info border-info">
                    <a href="TrasladosConfirmarPreSucursal.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="card border-success mb-3" style="max-width: 18rem;">
                <div class="card-header bg-transparent border-warning">Cuadro Basico</div>
                <div class="card-body text-success">
                </div>
                <div class="card-footer bg-warning border-warning">
                    <a href="CuadroBasicoSucursales.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
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

        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>

    </form>


</body>
</html>
<!-- jQuery -->
<script src="plugins/jquery/jquery.min.js"></script>
<script src="vendor/bootstrap-4.1/popper.min.js"></script>
<!-- Bootstrap 4 -->
<script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
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
    $("#matrizrep").DataTable({
        "responsive": true,
        "lengthChange": false,
        "autoWidth": false,
        "buttons": ["excel", "pdf", "colvis"],
        "sort": true,
        "order": [[0, 'desc']],
        "deferRender": true,
        "lengthMenu": [[20, -1], [20, "All"]],
    }).buttons().container().appendTo('#matrizrep_wrapper .col-md-6:eq(0)');
</script>

<link href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.css" rel="stylesheet" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.js"></script>
