<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosSolicitud.aspx.vb" Inherits="TrasladosSolicitud" %>

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
    <link href="css/cc1ModalPopupExtemder.css" rel="stylesheet" />

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

        <div class="container-fluid">
            <div class="row">
                <div class="col-2">Cargar Todas las Rutas</div>
                <div class="col-2"># Pre-Solicitud</div>
                <div class="col-2">Buscar</div>
            </div>
            <div class="row">
                <div class="col-2">
                    <asp:Button ID="btnMOdalCargarRutas" runat="server" Text="Ver Rutas" class="btn btn-primary btn-lg" Width="100%" />
                </div>
                <div class="col-2">
                    <asp:TextBox ID="txtPresolicitud" runat="server" CssClass="form-control" Text="" placeholder="Digite # Documento" Height="100%"></asp:TextBox>
                </div>
                <div class="col-2">
                    <asp:Button ID="btnBuscarSolicitud" runat="server" Text="Buscar Documento" class="btn btn-dark btn-lg" Width="100%" />
                </div>
            </div>
            <br />
            <asp:LinkButton Text="" ID="lnkFake" runat="server" />
            <cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="lnkFake"
                CancelControlID="btnClose" BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
                <div class="header">
                    <h1>Detalle de Solicitudes en Rutas Seleccionadas</h1>
                </div>
                <div class="body">
                    <div class="row">
                        <div class="col-2"><b>Fecha Armado</b></div>
                        <div class="col-2">
                            <asp:TextBox ID="txtFechaArme" runat="server" ClientIDMode="Static" type="date" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-2"><b>Fecha Entrega</b></div>
                        <div class="col-2">
                            <asp:TextBox ID="txtFechaEnvio" runat="server" ClientIDMode="Static" type="date" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <hr />

                    <asp:GridView ID="gridTaskFlow" runat="server" CssClass="table table-bordered table-hover" Width="100%" AutoGenerateColumns="true">
                        <Columns>
                            <asp:TemplateField HeaderText="Seleccionar">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbDocument" runat="server" Height="40px" Width="40px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="footer" align="center">
                    <asp:Button ID="btnProcesar" runat="server" Text="Procesar Solicitud" class="btn btn-primary btn-lg" />
                    <asp:Button ID="btnClose" runat="server" Text="Cerrar Ventana" class="btn btn-danger btn-lg" />
                </div>
            </asp:Panel>
        </div>
        <br />
        <%--INICIO SECCION INGRESAR DATOS SOLICITUD PREPARACION--%>
        <section class="content">
            <div class="container-fluid">
                <!-- Main row -->
                <div class="row">
                    <!-- Left col -->
                    <section class="col-lg-12 connectedSortable">
                        <!-- Custom tabs (Charts with tabs)-->
                        <div class="card">
                            <div class="card-header">
                                <h3 class="card-title">Solicitudes de Traslados </h3>
                                <%--|  <asp:Label ID="lblIdCotizacion" runat="server" Text="0"></asp:Label>  |  <asp:Label ID="lblLinesId" runat="server" Text="0"></asp:Label>--%>
                                <div class="card-tools">
                                    <div class="input-group input-group-sm" style="width: 100px;">
                                        <i class="fas fa-compass fa-2x"></i>
                                    </div>
                                </div>
                            </div>
                            <!-- /.card-header -->
                            <br />
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-6">Nombre Almacen Origen</div>
                                    <div class="col-3">Buscar</div>
                                    <div class="col-3">Almacen Origen</div>
                                </div>
                                <div class="row">
                                    <div class="col-6">
                                        <asp:TextBox ID="txtNalmacenOrigen" runat="server" CssClass="form-control" Width="100%" Text="Distribucion Central de Motos"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" ServiceMethod="SearchWhsname"
                                            MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
                                            TargetControlID="txtNalmacenOrigen" FirstRowSelected="false">
                                        </cc1:AutoCompleteExtender>
                                    </div>
                                    <div class="col-3">
                                        <asp:Button ID="btnAlmOrigen" runat="server" class="btn btn-dark btn-block" Text="Buscar Alm" UseSubmitBehavior="False" />
                                    </div>
                                    <div class="col-3">
                                        <asp:TextBox ID="txtCodeAlmacenOrigen" runat="server" CssClass="form-control" Width="100%" ReadOnly="true" Text="DCM00"></asp:TextBox>
                                    </div>
                                </div>
                                <br />

                                <div class="row">
                                    <div class="col-6">Nombre Almacen Destino</div>
                                    <div class="col-3">Buscar</div>
                                    <div class="col-3">Almacen Destino</div>
                                </div>
                                <div class="row">
                                    <div class="col-6">
                                        <asp:TextBox ID="txtAlmacenName" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServiceMethod="SearchWhsname"
                                            MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
                                            TargetControlID="txtAlmacenName" FirstRowSelected="false">
                                        </cc1:AutoCompleteExtender>
                                    </div>
                                    <div class="col-3">
                                        <asp:Button ID="btnBuscarAlmacen" runat="server" class="btn btn-dark btn-block" Text="Buscar Alm" UseSubmitBehavior="False" />
                                    </div>
                                    <div class="col-3">
                                        <asp:TextBox ID="txtAlmacenCode" runat="server" CssClass="form-control" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-6">Modelo Moto</div>
                                    <div class="col-3">Cantidad</div>
                                    <div class="col-3">Codigo Cliente</div>
                                </div>
                                <div class="row">
                                    <div class="col-6">
                                        <asp:DropDownList ID="drpModelosMoto" runat="server" class="ui search dropdown fluid" name="drpModelosMoto" Width="100%"></asp:DropDownList>
                                    </div>
                                    <div class="col-3">
                                        <asp:TextBox ID="txtCantidad" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-3">
                                        <asp:TextBox ID="txtAlmacenCustomer" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col">
                                        <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control" placeholder="Observaciones" TextMode="MultiLine" Height="100px"></asp:TextBox>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-1">Motorista</div>
                                    <div class="col-5">
                                        <asp:DropDownList ID="drpMotoristas" runat="server" class="ui search dropdown fluid" name="drpMotoristas" Width="100%"></asp:DropDownList>
                                    </div>
                                    <div class="col-1">Motivo Traslado</div>
                                    <div class="col-5">
                                        <asp:DropDownList ID="drpMotivoTraslado" runat="server" class="ui search dropdown fluid" name="drpMotoristas" Width="100%">
                                            <asp:ListItem Selected="True" Value="0" Text="Seleccione Un Motivo"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Venta Inmediata"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Inventario"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Solicitud Canal"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Solicitud Distribuidor"></asp:ListItem>
                                        </asp:DropDownList>

                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col">Fecha Prevista de Armado</div>
                                    <div class="col">
                                        <asp:TextBox ID="txtFechaArmado" runat="server" type="date" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col">Fecha Prevista de Entrega</div>
                                    <div class="col">
                                        <asp:TextBox ID="txtFechaEntrega" runat="server" type="date" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <hr />
                                <div class="row">
                                    <div class="col">
                                        <asp:Button ID="btnLimpiarFormulario" runat="server" class="btn btn-danger btn-block" Text="Limpiar Formulario" UseSubmitBehavior="False" />
                                    </div>
                                    <div class="col">
                                        <asp:Button ID="btnCrearSolicitud" runat="server" class="btn btn-success btn-block" Text="Agregar Motos " UseSubmitBehavior="False" />
                                    </div>
                                </div>
                            </div>
                            <!-- /.card-body -->
                        </div>
                        <!-- /.card -->
                    </section>
                </div>
            </div>
        </section>
        <%--FIN SECCION INGRESAR DATOS SOLICITUD PREPARACION--%>
        <asp:Panel ID="pnlVentanaManual" runat="server" Visible="false">
            <div class="ui segment">
                <div class="ui grid">
                    <div class="six wide column">

                        <div class="container-fluid table-responsive">

                            <asp:GridView ID="gridPresolicitudes" runat="server" CssClass="table table-bordered table-hover" name="gridPresolicitudes" Width="100%" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="Id" />
                                    <asp:BoundField DataField="ALMORIGEN" HeaderText="Origen" />
                                    <asp:BoundField DataField="ALMDESTINO" HeaderText="Destino" />
                                    <asp:BoundField DataField="MODELO" HeaderText="Modelo" />
                                    <asp:BoundField DataField="CANTIDAD" HeaderText="Qty" />
                                    <%--<asp:BoundField DataField="ESPACIOS" HeaderText="Espacios" />--%>
                                    <asp:BoundField DataField="FECHADESEADA" HeaderText="Fecha Deseada" />
                                    <asp:BoundField DataField="OBSERACIONES" HeaderText="Comentarios" />
                                    <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/arrowadd.jpg" Text="Agregar" CommandName="Agregar" HeaderText="Agregar" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ControlStyle Height="30px" Width="30px" />
                                        <ItemStyle Wrap="False" />
                                    </asp:ButtonField>
                                </Columns>
                            </asp:GridView>


                        </div>
                    </div>
                    <div class="ten wide column">
                    </div>
                </div>

            </div>

        </asp:Panel>
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
                                        <asp:GridView ID="gridSolicitudAbierta" runat="server" CssClass="table table-bordered table-hover"
                                            AutoGenerateColumns="false" ShowFooter="true">
                                            <HeaderStyle CssClass="thead-dark" />
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderText="Id" />
                                                <asp:BoundField DataField="WHSCODE" HeaderText="Cod. Alm." />
                                                <asp:BoundField DataField="WHSNAME" HeaderText="Almacen" />
                                                <asp:BoundField DataField="MODELO" HeaderText="Modelo" />
                                                <asp:BoundField DataField="QTYSOLICITADA" HeaderText="Solicitado" />
                                                <asp:BoundField DataField="OBSERVACIONES" HeaderText="Observaciones" />
                                                <asp:BoundField DataField="DATECREATED" HeaderText="Fecha / Hora" />
                                                <asp:BoundField DataField="PREIDHEADER" HeaderText="Pre Id Header" />
                                                <asp:BoundField DataField="PRELIDHEADER" HeaderText="Pre Id Linea" />
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

        <!-- Inicio Modal Solicitud Supervisor-->
        <div class="modal fade" id="modalRutas" tabindex="-1" role="dialog" aria-labelledby="modalRutasLabel"
            aria-hidden="true">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalRutasLabel">Rutas Disponibles</h5>
                        <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">×</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="center">
                            <div class="row">
                                <div class="col">
                                    <asp:ListBox ID="lstRutasDisponibles" runat="server" SelectionMode="Multiple" class="demo" Height="400px" Width="100%"></asp:ListBox>
                                    <link rel="stylesheet" type="text/css" href="dual-listbox.css" />
                                    <script type="text/javascript" src="dual-listbox.js"></script>
                                    <script type="text/javascript">
                                        new DualListbox('.demo', {
                                            addEvent: function (value) { },
                                            removeEvent: function (value) { },
                                            availableTitle: 'Rutas Disponibles',
                                            selectedTitle: 'Rutas Seleccionados',
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
                    </div>
                    <div class="modal-footer">
                        <button class="btn btn-secondary" type="button" data-dismiss="modal">Cancel</button>
                        <asp:Button ID="btnGuardarSolicitud" runat="server" class="btn btn-primary" Text="Guardar Solicitud" />
                    </div>
                </div>
            </div>
        </div>
        <!-- Fin Modal Solicitud Supervisor-->


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
                    "positionClass": "toast-bottom-width",
                    "showDuration": "1000",
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
    $('#gridTaskFlow').DataTable({
        "scrollY": '600px',
        "scrollCollapse": true,
        "paging": false,
    });


</script>
<link href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.css" rel="stylesheet" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.js"></script>

<script>
    $("#lstMultiSelectRutas").dropdown(
        {
            "fullTextSearch": true
        });

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


