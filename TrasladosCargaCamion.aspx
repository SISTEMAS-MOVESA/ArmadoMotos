<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosCargaCamion.aspx.vb" Inherits="TrasladosCargaCamion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
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
        <!-- Main content -->
    <section class="content">
      <div class="container-fluid">
        <!-- Small boxes (Stat box) -->
        <div class="row">
        </div>
        <!-- /.row -->
        <!-- Main row -->
        <div class="row">
          <!-- Left col -->
          <section class="col-lg-12 connectedSortable">
            <!-- Custom tabs (Charts with tabs)-->
            <div class="card">
              <div class="card-header">
                <h3 class="m-0 text-center"> Carga de Camion</h3>
                <h6 class="m-0 text-center"> <asp:Label ID="lbluser" runat="server" text=""></asp:Label></h6>
              </div><!-- /.card-header -->
              <div class="card-body">
                <asp:Panel ID="pnlPrincipal" runat="server" DefaultButton="btnBuscar">
                  <div class="container-fluid">
                    <div class="row">
                      <div class="col justify-content-center">
                        <asp:TextBox ID="txtbuscar" CssClass="form-control" runat="server" Text="" AutoComplete="false" placeholder="Digite Codigo o Numero de Serie" Width="100%"></asp:TextBox>
                          <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServiceMethod="SearchSerial"
                            MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
                            TargetControlID="txtbuscar" FirstRowSelected="false">
                          </cc1:AutoCompleteExtender>
                      </div>
                    </div>
                    <br />
                    <div class="row">
                      <div class="col">
                        <asp:Button ID="btnBuscar" runat="server" class="btn btn-info btn-block" Text="Buscar Serie"/>
                      </div>
                    </div>
                    <br />
                    <div class="row">
                      <div class="col">
                        <asp:TextBox ID="txtItemcode" CssClass="form-control" runat="server" Text="" AutoComplete="false" placeholder="Articulo" readonly="true" Width="100%"></asp:TextBox>
                      </div>
                    </div>
                    <div class="row">
                      <div class="col">
                        <asp:TextBox ID="txtItemname" CssClass="form-control" runat="server" Text="" AutoComplete="false" placeholder="Descripcion" readonly="true" Width="100%"></asp:TextBox>

                      </div>
                    </div>
                    <div class="row">
                      <div class="col">
                        <asp:TextBox ID="txtModelo" CssClass="form-control" runat="server" Text="" AutoComplete="false" placeholder="Modelo" readonly="true" Width="100%"></asp:TextBox>

                      </div>
                    </div>
                    <div class="row">
                      <div class="col">
                        <asp:TextBox ID="txtAlmOrigenCodigo" CssClass="form-control" runat="server" Text="" AutoComplete="false" placeholder="Marca" readonly="true" Width="100%"></asp:TextBox>
                      </div>
                    </div>
                     <div class="row">
                      <div class="col">
                        <asp:TextBox ID="txtAlmOrigenNombre" CssClass="form-control" runat="server" Text="" AutoComplete="false" placeholder="Cod Almacen" readonly="true" Width="100%"></asp:TextBox>
                      </div>
                    </div>
                    <div class="row">
                      <div class="col">
                        <asp:TextBox ID="txtAlmacenDestinoCode" CssClass="form-control" runat="server" Text="" AutoComplete="false" placeholder="Marca" readonly="true" Width="100%"></asp:TextBox>

                      </div>
                    </div>
                     <div class="row">
                      <div class="col">
                        <asp:TextBox ID="txtAlmacenDestinoNombre" CssClass="form-control" runat="server" Text="" AutoComplete="false" placeholder="Cod Almacen" readonly="true" Width="100%"></asp:TextBox>
                      </div>
                    </div>
                     <div class="row">
                      <div class="col">
                        <asp:TextBox ID="txtCodigoCliente" CssClass="form-control" runat="server" Text="" AutoComplete="false" placeholder="Almacen" readonly="true" Width="100%"></asp:TextBox>
                      </div>
                    </div>
                    <br />
                    <div class="row">
                      <div class="col">
                        <asp:Button ID="btnCargar" runat="server" class="btn btn-primary btn-block" Text="Cargar Moto al Camion"/>
                      </div>
                    </div>

                    <br />  
                    <br />  
                    <br />  
                    <div class="row">
                      <div class="col">
                        <asp:Button ID="btnGuardar" runat="server" class="btn btn-primary btn-block" Text="Cerrar Camion"/>
                      </div>
                    </div>
                    <br />  
                    <div class="row">
                      <div class="col">
                        <asp:Button ID="btnClear" runat="server" class="btn btn-danger btn-block" Text="Limpiar Formulario"/>
                      </div>
                    </div>
                  </div>
                </asp:Panel>
              </div>
            <!-- /.card-body -->
            </div>
            <!-- /.card -->


          </section>
          <!-- /.Left col -->
          <!-- right col (We are only adding the ID to make the widgets sortable)-->
      
          <!-- right col -->
        </div>
        <!-- /.row (main row) -->
      </div><!-- /.container-fluid -->
    </section>
    <!-- /.content -->
  
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
   function showContent(typeofMsg,mensaje) {
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


<link href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.css" rel="stylesheet"/>
<script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.js"></script>
