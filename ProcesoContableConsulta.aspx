<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProcesoContableConsulta.aspx.vb" Inherits="ProcesoContableConsulta" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Consulta Liquidaciones</title>
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
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js"crossorigin="anonymous"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="pos-f-t">
            <div class="collapse" id="navbarToggleExternalContent">
              <div class="bg-dark p-4">
                    <ul class="nav nav-tabs">
                      <li class="nav-item">
                          <a class="nav-link" href="MainDashBoard.aspx"><img src="Imagenes/mnegra.png" width="30" height="25" alt="Portal Armado de Motos"/></a>
                        </li>
        
                        <asp:Panel ID="pnlParametrizaciones" runat="server" Visible="True">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Parametrizaciones</a>
                              <ul class="dropdown-menu">
                                  <asp:Panel ID="pnlContratistas" runat="server" Visible="True"><li><a class="dropdown-item" href="Contratistas.aspx">Contratistas</a></li></asp:Panel>
                                  <asp:Panel ID="pnlTrabajoAD" runat="server" Visible="True"><li><a class="dropdown-item" href="TrabajosAd.aspx">Trabajos Adicionales</a></li></asp:Panel>
                                  <asp:Panel ID="pnlPreciosArmado" runat="server" Visible="True"><li><a class="dropdown-item" href="PrecioModelos.aspx">Precios Armado</a></li></asp:Panel>
                                  <asp:Panel ID="pnlUsuarios" runat="server" Visible="True"><li><a class="dropdown-item" href="Usuarios.aspx">Usuarios</a></li></asp:Panel>
                              </ul>
                        </asp:Panel>
        
                      <asp:Panel ID="pnlAsignaciones" runat="server" Visible="True">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Motos en Caja</a>
                              <ul class="dropdown-menu">
                                  <asp:Panel ID="pnlMotoEncaja" runat="server" Visible="True"><li><a class="dropdown-item" href="MotoenCaja.aspx">Asignar Moto</a></li></asp:Panel>
                                  <asp:Panel ID="PnlStockGlobal" runat="server" Visible="True"><li><a class="dropdown-item" href="StockPorModelos.aspx">Stock Por Modelo</a></li></asp:Panel>
                              </ul>
                        </asp:Panel>
        
                      <asp:Panel ID="pnlArmado" runat="server" Visible="True">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Armado</a>
                              <ul class="dropdown-menu">
                                  <asp:Panel ID="pnlListadoMotosProceso" runat="server" Visible="True"><li><a class="dropdown-item" href="ListadoProceso.aspx">Motos En Proceso</a></li></asp:Panel>
                              </ul>
                        </asp:Panel>
        
                        <asp:Panel ID="PnlControlCalidad" runat="server" Visible="True">
                          <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Control de Calidad</a>
                            <ul class="dropdown-menu">
                                <asp:Panel ID="pnlListadoMotosCalidad" runat="server" Visible="True"><li><a class="dropdown-item" href="ListadoCCalidad.aspx">Motos En Control de Calidad</a></li></asp:Panel>
                                <asp:Panel ID="pnoInformeProcesadasCC" runat="server" Visible="True"><li><a class="dropdown-item" href="InformeCC.aspx">Informe de Motos Procesadas CC</a></li></asp:Panel>
                            </ul>
                      </asp:Panel>
        
                    <asp:Panel ID="pnlProcesoContable" runat="server" Visible="True">
                          <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Proceso Contable</a>
                            <ul class="dropdown-menu">
                                <asp:Panel ID="pnoCrearLiquidacion" runat="server" Visible="True"><li><a class="dropdown-item" href="ProcesoContable.aspx">Crear Liquidacion</a></li></asp:Panel>
                                <asp:Panel ID="pnlCrearPO" runat="server" Visible="True"><li><a class="dropdown-item" href="GenerarPO.aspx">Crear Orden de Compra</a></li></asp:Panel>
                                <asp:Panel ID="pnlConsultaLiquidaciones" runat="server" Visible="True"><li><a class="dropdown-item" href="ProcesoContableConsulta.aspx">Consulta Liquidaciones Anteiores</a></li></asp:Panel>
                                </ul>
                      </asp:Panel>
                      
                    <asp:Panel ID="pnlDisponibles" runat="server" Visible="True">
                          <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Disponibles</a>
                            <ul class="dropdown-menu">
                              <asp:Panel ID="pnlDisponibleArmadoSAP" runat="server" Visible="True"><li><a class="dropdown-item" href="InformeMotosDisponibles.aspx">Motos Armadas Procesadas Portal</a></li></asp:Panel>
                              <asp:Panel ID="pnlExpedienteVeh" runat="server" Visible="True"><li><a class="dropdown-item" href="ExpedienteVehiculo.aspx">Expediente Vehiculo</a></li></asp:Panel>
                            </ul>
                      </asp:Panel>                            
          
                      <asp:Panel ID="pnlNoDisponible" runat="server" Visible="True">
                        <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">No Disponibles</a>
                          <ul class="dropdown-menu">
                            <asp:Panel ID="pnlInformeNoDisponible" runat="server" Visible="True"><li><a class="dropdown-item" href="InformeMotosNoDisponibles.aspx">Motos No Disponibles</a></li></asp:Panel>
                            <asp:Panel ID="pnlExpedienteGarantia" runat="server" Visible="True"><li><a class="dropdown-item" href="ExpedienteVehiculoGarantia.aspx">Expediente Vehiculo Garantia / ND</a></li></asp:Panel>
                            <asp:Panel ID="pnlRepuestoRetirado" runat="server" Visible="True"><li><a class="dropdown-item" href="RepuestosRetirados.aspx">Repuestos Retirados</a></li></asp:Panel>
                            <asp:Panel ID="pnlOCRProveedor" runat="server" Visible="True"><li><a class="dropdown-item" href="OCRProveedor.aspx">Orden de Compra Proveedor</a></li></asp:Panel>
                            <asp:Panel ID="pnlPedidoRepuestosFBack" runat="server" Visible="True"><li><a class="dropdown-item" href="PedidoRepuestosFeedback.aspx">Subir Informacion Compra Repuesto</a></li></asp:Panel>
                          </ul>
                    </asp:Panel>                            
        
                    <li class="nav-item">
                          <a class="nav-link " href="Default.aspx">Salir</a>
                        </li>
                  </ul>
                <h4 class="text-white">Bienvenido</h4>
                <span class="text-muted"><asp:Label ID="lblNombreUsuario" runat="server" Text="Usuario Conectado"></asp:Label></span>
              </div>
            </div>
            <nav class="navbar navbar-dark bg-dark">
              <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarToggleExternalContent" aria-controls="navbarToggleExternalContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
              </button>
            </nav>
        </div>
         <%--JUMBOTRON--%>
         <div class="container-fluid">
                <h1>Consulta Liquidaciones</h1>
                <hr class="my-4">
                <p class="lead">
         </div>
        <div class="container-fluid">
            <div class="row">
                <div class="col-3">
                    <asp:DropDownList ID="drpContratista" runat="server" class="btn btn-secondary dropdown-toggle; text-left" AutoPostBack="true" Width="100%"></asp:DropDownList>
                </div>
                <div class="col-2">
                    <asp:TextBox ID="txtDesde" runat="server" type="date" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-2">
                    <asp:TextBox ID="txtHasta" runat="server" type="date" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-2">
                    <asp:Button ID="btnConsultar" runat="server" class="btn btn-success rounded" Text="Consultar" Width="100%" />
                </div>
            </div>
            <hr />
            <asp:Panel ID="pnlGridControlCalidad" runat="server" Visible="true">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col">
                            <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered table-hover" Width="100%" AutoGenerateColumns="false">
                                <HeaderStyle CssClass="thead-dark" />
                                <Columns>
                                    <asp:BoundField DataField="FECHACREACION" HeaderText="Fecha" />
                                    <asp:BoundField DataField="LIQUIDACIONID" HeaderText="ID" />
                                    <asp:BoundField DataField="IDCALIDAD" HeaderText="Id Calidad" />
                                    <asp:BoundField DataField="SERIE" HeaderText="Serie" />
                                    <asp:BoundField DataField="MODELO" HeaderText="Modelo" />
                                    <asp:BoundField DataField="COLOR" HeaderText="Color" />
                                    <asp:BoundField DataField="MECANICOID" HeaderText="Mecanico" />
                                    <asp:BoundField DataField="CPROVEEDOR" HeaderText="Proveedor" />
                                    <asp:BoundField DataField="PRECIOARMADO" HeaderText="Precio Armado" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <center>
            <div class="container">
                <footer class="main-footer">
                    <strong> <i class="ion-paintbrush"></i> WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
                </footer>
            </div>
        </center>
    <asp:ScriptManager ID="sm1" runat="server"  EnablePageMethods="true"></asp:ScriptManager>
    </form>
</body>
</html>
    <script src="js/jquery.min.js"></script>
    <script src="js/popper.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/main.js"></script>
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
    $(function () {
        $("[id*=GridView1]").DataTable({
            dom: 'Bfrtip',
            buttons: [
                'excelHtml5',
                'pdfHtml5',
                'colvis'
            ],
            bLengthChange: true,
            lengthMenu: [[10, -1], [10, "All"]],
            bFilter: true,
            bSort: true,
            bPaginate: true,
            order: [[0, "desc"]]
        });
    });
</script>