<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MainDashBoard.aspx.vb" Inherits="MainDashBoard" %>

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
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/2.9.2/umd/popper.min.js" integrity="sha512-2rNj2KJ+D8s1ceNasTIex6z4HWyOnEYLVC3FigGOmyQCZc2eBXKgOxQmo3oKLHyfcj53uz4QMsRCWNbLd32Q1g==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>

</head>
<body>
    <form id="form1" runat="server">

        <div class="pos-f-t">
            <div class="collapse" id="navbarToggleExternalContent">
                <div class="bg-dark p-4">
                    <ul class="nav nav-tabs">
                        <li class="nav-item">
                            <a class="nav-link" href="MainDashBoard.aspx">
                                <img src="Imagenes/mnegra.png" width="30" height="25" alt="Portal Armado de Motos" /></a>
                        </li>

                        <asp:Panel ID="pnlParametrizaciones" runat="server" Visible="True">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Parametrizaciones</a>
                            <ul class="dropdown-menu">
                                <asp:Panel ID="pnlContratistas" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="Contratistas.aspx">Contratistas</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlTrabajoAD" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="TrabajosAd.aspx">Trabajos Adicionales</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlPreciosArmado" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="PrecioModelos.aspx">Precios Armado</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlPinturaColores" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="PinturaColores.aspx">Pintura Colores</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlUsuarios" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="Usuarios.aspx">Usuarios</a></li>
                                </asp:Panel>
                            </ul>
                        </asp:Panel>

                        <asp:Panel ID="pnlAsignaciones" runat="server" Visible="True">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Motos en Caja</a>
                            <ul class="dropdown-menu">
                                <asp:Panel ID="pnlPlanificadorTrabajo" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="OrdendeTrabajo.aspx">Planificador de Trabajo</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlOrdenTrabajoConfirmacion" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="OrdenTrabajoConfirmacion.aspx">Confirmacion Supervisor</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlAutorizacionesPlanificador" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="AutorizacionCreditos.aspx">Autorizacion Plan Trabajo Creditos</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlOrdendeProduccion" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="OrdenProduccion.aspx">Planificador de Trabajo</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlMotoEncaja" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="MotoenCaja.aspx">Asignar Moto</a></li>
                                </asp:Panel>
                                <asp:Panel ID="PnlStockGlobal" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="StockPorModelos.aspx">Stock Por Modelo</a></li>
                                </asp:Panel>
                            </ul>
                        </asp:Panel>

                        <asp:Panel ID="pnlArmado" runat="server" Visible="True">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Armado</a>
                            <ul class="dropdown-menu">
                                <asp:Panel ID="pnlListadoMotosProceso" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="ListadoProceso.aspx">Motos En Proceso</a></li>
                                </asp:Panel>
                            </ul>
                        </asp:Panel>

                        <asp:Panel ID="PnlControlCalidad" runat="server" Visible="True">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Control de Calidad</a>
                            <ul class="dropdown-menu">
                                <li><a class="dropdown-item" href="ListadoCCalidad.aspx">Formulario Control Calidad Manual</a></li>
                                <li><a class="dropdown-item" href="InformeCCFinArmado.aspx">Moto Pendiente Control Calidad</a></li>
                                <li><a class="dropdown-item" href="InformeEnProcesoCalidad.aspx">Moto en Proceso de Calidad</a></li>
                                <li><a class="dropdown-item" href="InformeCC.aspx">Motos Control de Calidad Finalizado</a></li>
                            </ul>
                        </asp:Panel>

                        <asp:Panel ID="pnlProcesoContable" runat="server" Visible="True">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Proceso Contable</a>
                            <ul class="dropdown-menu">
                                <asp:Panel ID="pnoCrearLiquidacion" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="ProcesoContable.aspx">Crear Liquidacion</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlCrearPO" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="GenerarPO.aspx">Crear Orden de Compra</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlConsultaLiquidaciones" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="ProcesoContableConsulta.aspx">Consulta Liquidaciones Anteiores</a></li>
                                </asp:Panel>
                            </ul>
                        </asp:Panel>

                        <asp:Panel ID="pnlDisponibles" runat="server" Visible="True">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Disponibles</a>
                            <ul class="dropdown-menu">
                                <asp:Panel ID="pnlDisponibleArmadoSAP" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="InformeMotosDisponibles.aspx">Motos Armadas Procesadas Portal</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlExpedienteVeh" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="ExpedienteVehiculo.aspx">Expediente Vehiculo</a></li>
                                </asp:Panel>
                            </ul>
                        </asp:Panel>

                        <asp:Panel ID="pnlNoDisponible" runat="server" Visible="True">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">No Disponibles</a>
                            <ul class="dropdown-menu">
                                <asp:Panel ID="pnlInformeNoDisponible" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="InformeMotosNoDisponibles.aspx">Motos No Disponibles</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlExpedienteGarantia" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="ExpedienteVehiculoGarantia.aspx">Expediente Vehiculo Garantia / ND</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlRepuestoRetirado" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="RepuestosRetirados.aspx">Repuestos Retirados</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlOCRProveedor" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="OCRProveedor.aspx">Orden de Compra Proveedor</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlPedidoRepuestosFBack" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="PedidoRepuestosFeedback.aspx">Subir Informacion Compra Repuesto</a></li>
                                </asp:Panel>
                            </ul>
                        </asp:Panel>

                        <li class="nav-item">
                            <a class="nav-link " href="Default.aspx">Salir</a>
                        </li>
                    </ul>
                    <h4 class="text-white">Bienvenido</h4>
                    <span class="text-muted">
                        <asp:Label ID="lblNombreUsuario" runat="server" Text="Usuario Conectado"></asp:Label></span>
                </div>
            </div>
            <nav class="navbar navbar-dark bg-dark">
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarToggleExternalContent" aria-controls="navbarToggleExternalContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
            </nav>
        </div>
        <br />
        <div class="container-fluid">

            <div class="row">
                <div class="col-12 col-sm-6 col-md-6 col-lg-2 col-xl-2">
                    <div class="small-box bg-info">
                        <div class="inner">
                            <a href="#" style="background: black; color: white;">
                                <h3>
                                    <asp:Label ID="lblMotosEnCaja" runat="server" Text="0"></asp:Label></h3>
                            </a>
                            <p>Motos en Caja</p>
                        </div>
                        <div class="icon">
                            <i class="ion-compose"></i>
                        </div>
                        <a href="MotoenCaja.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-6 col-lg-2 col-xl-2">
                    <div class="small-box bg-danger">
                        <div class="inner">
                            <a href="#" style="background: black; color: white;">
                                <h3>
                                    <asp:Label ID="lblMotosEnProceso" runat="server" Text="0"></asp:Label></h3>
                            </a>
                            <p>Armado</p>
                        </div>
                        <div class="icon">
                            <i class="ion-wrench"></i>
                        </div>
                        <a href="ListadoProceso.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-6 col-lg-2 col-xl-2">
                    <div class="small-box bg-warning">
                        <div class="inner">
                            <a href="#" style="background: black; color: white;">
                                <h3>
                                    <asp:Label ID="lblControlCalidad" runat="server" Text="0"></asp:Label></h3>
                            </a>
                            <p>Control de Calidad</p>
                        </div>
                        <div class="icon">
                            <i class="ion-bug"></i>
                        </div>
                        <a href="InformeCCFinArmado.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-6 col-lg-2 col-xl-2">
                    <div class="small-box bg-success">
                        <div class="inner">
                            <a href="#" style="background: black; color: white;">
                                <h3>
                                    <asp:Label ID="lblMotosDisponibles" runat="server" Text="0"></asp:Label></h3>
                            </a>
                            <p>Disponibles (Despacho)</p>
                        </div>
                        <div class="icon">
                            <i class="ion-log-out"></i>
                        </div>
                        <a href="InformeMotosDisponibles.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-6 col-lg-2 col-xl-2">
                    <div class="small-box bg-gradient-maroon">
                        <div class="inner">
                            <a href="#" style="background: black; color: white;">
                                <h3>
                                    <asp:Label ID="txtMotosNoDisponibles" runat="server" Text="0"></asp:Label></h3>
                            </a>
                            <p>Motos No Disponibles</p>
                        </div>
                        <div class="icon">
                            <i class="ion-log-in"></i>
                        </div>
                        <a href="InformeMotosNoDisponibles.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-6 col-lg-2 col-xl-2">
                    <div class="small-box bg-gradient-dark  ">
                        <div class="inner">
                            <a href="#" style="background: black; color: white;">
                                <h3>$</h3>
                            </a>
                            <p>Proceso Contable</p>
                        </div>
                        <div class="icon">
                            <i class="ion-cash"></i>
                        </div>
                        <a href="MenuProcesoCotable.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>

            </div>
            <div class="row">
                <div class="col-12 col-sm-6 col-md-6 col-lg-2 col-xl-2">
                    <div class="small-box bg-gradient-orange">
                        <div class="inner">
                            <h3>*</h3>
                            <p>Traslados en Transito</p>
                        </div>
                        <div class="icon">
                            <i class="fa-solid fa-cart-shopping"></i>
                        </div>
                        <a href="TrasladosDashboardTransito.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-6 col-lg-2 col-xl-2">
                    <div class="small-box bg-gradient-info">
                        <div class="inner">
                            <h3>*</h3>
                            <p>Planificacion Despachos</p>
                        </div>
                        <div class="icon">
                            <i class="fa-solid fa-truck"></i>
                        </div>
                        <a href="TrasladosDashboardPlanner.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-6 col-lg-2 col-xl-2">
                    <div class="small-box bg-gradient-secondary">
                        <div class="inner">
                            <h3>*</h3>
                            <p>Informes de Produccion</p>
                        </div>
                        <div class="icon">
                            <i class="fa-solid fa-chart-simple"></i>
                        </div>
                        <a href="InformesProduccion.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-6 col-lg-2 col-xl-2">
                    <div class="small-box bg-gradient-indigo">
                        <div class="inner">
                            <h3>*</h3>
                            <p>Parametrizaciones</p>
                        </div>
                        <div class="icon">
                            <i class="ion-gear-a"></i>
                        </div>
                        <a href="MenuParametrizaciones.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-6 col-lg-2 col-xl-2">
                    <div class="small-box bg-gradient-dark">
                        <div class="inner">
                            <h3>*</h3>
                            <p>Auditoria</p>
                        </div>
                        <div class="icon">
                            <i class="fa fa-pie-chart"></i>
                        </div>
                        <a href="DashboardAuditoria.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>

            </div>          
        </div>
        <div class="container-fluid">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered table-hover"
                Width="100%">
                <HeaderStyle CssClass="thead-dark sticky-header" />
                <Columns>
                </Columns>
            </asp:GridView>
        </div>
        <center>
            <div class="container">
                <footer class="main-footer">
                    <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
                </footer>
            </div>
        </center>

        <script src="plugins/jquery/jquery.min.js"></script>
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


