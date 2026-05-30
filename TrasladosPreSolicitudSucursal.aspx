<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosPreSolicitudSucursal.aspx.vb" Inherits="TrasladosPreSolicitudSucursal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Traslados || Pre-Solicitud</title>
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
    <link href="css/movesa_load_spinner.css" rel="stylesheet" />
    <!-- jQuery -->
    <script src="plugins/jquery/jquery.min.js"></script>
    <script src="vendor/bootstrap-4.1/popper.min.js"></script>
    <!-- Bootstrap 4 -->
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>


</head>
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

        <%--INICIO SECCION INGRESAR DATOS SOLICITUD PREPARACION--%>
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
                                <h1 class="card-title"><b>Solicitudes de Traslados </b></h1>
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
                                    <div class="col-1">Motivo Traslado</div>
                                    <div class="col-3">
                                        <asp:DropDownList ID="drpMotivoTraslado" runat="server" class="ui search dropdown fluid" name="drpMotoristas" Width="100%">
                                            <asp:ListItem Selected="True" Value="0" Text="Seleccione Un Motivo"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Venta Inmediata"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Inventario"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Apertura CI"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Apertura CD"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-1">Fecha Deseada</div>
                                    <div class="col-3">
                                        <asp:TextBox ID="txtFechaDeseada" runat="server" type="date" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <hr />
                                <div class="row">
                                    <div class="col text-left font-weight-bold">Nombre Almacen Origen</div>
                                    <div class="col text-left font-weight-bold">Buscar</div>
                                    <div class="col text-left font-weight-bold">Codigo Almacen Origen</div>
                                    <div class="col text-left font-weight-bold">Ruta Almacen Origen</div>
                                    <div class="col text-left font-weight-bold">Encargado Almacen Origen</div>
                                </div>
                                <div class="row">
                                    <div class="col">
                                        <asp:TextBox ID="txtNalmacenOrigen" runat="server" CssClass="form-control" Width="100%" Text="Distribucion Central de Motos"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" ServiceMethod="SearchWhsname"
                                            MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
                                            TargetControlID="txtNalmacenOrigen" FirstRowSelected="false">
                                        </cc1:AutoCompleteExtender>
                                    </div>
                                    <div class="col">
                                        <asp:Button ID="btnAlmOrigen" runat="server" class="btn btn-dark btn-block" Text="Buscar Alm" UseSubmitBehavior="False" />
                                    </div>
                                    <div class="col">
                                        <asp:TextBox ID="txtCodeAlmacenOrigen" runat="server" CssClass="form-control" Width="100%" ReadOnly="true" Text="DCM00"></asp:TextBox>
                                    </div>
                                    <div class="col">
                                        <asp:TextBox ID="txtRutaAlmacenOrigen" runat="server" CssClass="form-control" Width="100%" ReadOnly="true" Text="SPS"></asp:TextBox>
                                    </div>
                                    <div class="col">
                                        <asp:TextBox ID="txtEncargadoOrigen" runat="server" CssClass="form-control" Width="100%" ReadOnly="true" Text="Jefe Logistica"></asp:TextBox>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col text-left font-weight-bold">Nombre Almacen Destino</div>
                                    <div class="col text-left font-weight-bold">Buscar</div>
                                    <div class="col text-left font-weight-bold">Codigo Almacen Destino</div>
                                    <div class="col text-left font-weight-bold">Ruta Almacen Destino</div>
                                    <div class="col text-left font-weight-bold">Encargado Almacen Destino</div>
                                </div>
                                <div class="row">
                                    <div class="col">
                                        <asp:TextBox ID="txtNalmacenDestino" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServiceMethod="SearchWhsname"
                                            MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
                                            TargetControlID="txtNalmacenDestino" FirstRowSelected="false">
                                        </cc1:AutoCompleteExtender>
                                    </div>
                                    <div class="col">
                                        <asp:Button ID="btnAlmDestino" runat="server" class="btn btn-dark btn-block" Text="Buscar Alm" UseSubmitBehavior="False" />
                                    </div>
                                    <div class="col">
                                        <asp:TextBox ID="txtCodeAlmDestino" runat="server" CssClass="form-control" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col">
                                        <asp:TextBox ID="txtRutaAlmacenDestino" runat="server" CssClass="form-control" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col">
                                        <asp:TextBox ID="txtEncargadoDestino" runat="server" CssClass="form-control" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="col">
                                        <asp:TextBox ID="txterror" runat="server" Visible="false" Text=""></asp:TextBox>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-6">Modelo Moto</div>
                                    <div class="col-1">Cantidad</div>
                                    <div class="col-2">Agregar</div>
                                    <div class="col-3">Codigo Cliente</div>
                                </div>
                                <div class="row">
                                    <div class="col-6">

                                        <asp:DropDownList ID="drpModelosMoto" name="drpModelosMoto" runat="server" class="ui search dropdown fluid" Width="100%"></asp:DropDownList>
                                    </div>
                                    <div class="col-1">
                                        <asp:TextBox ID="txtCantidad" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-2">
                                        <asp:Button ID="btnCrearSolicitud" runat="server" class="btn btn-success btn-block" Text="Agregar Moto" UseSubmitBehavior="False" />
                                    </div>
                                    <div class="col-3">
                                        <asp:TextBox ID="txtAlmacenCustomerDestino" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>

                                </div>
                                <hr />
                                <div class="row">
                                    <div class="col">
                                        <asp:Button ID="btnLimpiarFormulario" runat="server" class="btn btn-danger btn-block" Text="Limpiar Formulario" UseSubmitBehavior="False" />
                                    </div>

                                    <div class="col">
                                        <asp:Button ID="btnCrearCamion" runat="server" class="btn btn-success btn-block" Text="Crear Pre-Solicitud" UseSubmitBehavior="False" />
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
                                        <asp:GridView ID="gridPreSolicitudAbierta" runat="server" CssClass="table table-bordered table-hover"
                                            AutoGenerateColumns="false" ShowFooter="true">
                                            <HeaderStyle CssClass="thead-dark" />
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderText="Id" />
                                                <asp:BoundField DataField="ARTICULO" HeaderText="Articulo" />
                                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion" />
                                                <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ESPACIOS" HeaderText="Espacios" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ALMORIGEN" HeaderText="Alm. Origen" />
                                                <asp:BoundField DataField="NALMACENO" HeaderText="Nombre Alm.Origen" />
                                                <asp:BoundField DataField="ALMDESTINO" HeaderText="Alm. Destino" />
                                                <asp:BoundField DataField="NALMACEND" HeaderText="Nombre Alm. Destino" />
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

        <%--MODAL PANTALLA DE CONFIRMACION--%>
        <div class="ui basic modal">
            <div class="ui icon header">
                <i class="archive icon"></i>
                Documento Creado
            </div>
            <div class="content" style="display: flex; justify-content: center; align-items: center; flex-direction: column;">
                <p class="ex2">
                    <asp:Label ID="lblDocumento" runat="server" Text=""></asp:Label>
                </p>
            </div>
            <div class="actions" style="text-align: center;">
                <div class="ui green ok inverted button">
                    <i class="checkmark icon"></i>
                    Cerrar
                </div>
            </div>
        </div>
        <!-- Fin Modal Agregar Confirmacion Docto-->
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.css" />
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.js" integrity="sha512-gnoBksrDbaMnlE0rhhkcx3iwzvgBGz6mOEj4/Y5ZY09n55dYddx6+WYc72A55qEesV8VX2iMomteIwobeGK1BQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.css" integrity="sha512-3quBdRGJyLy79hzhDDcBzANW+mVqPctrGCfIPosHQtMKb3rKsCxfyslzwlz2wj1dT8A7UX+sEvDjaUv+WExQrA==" crossorigin="anonymous" referrerpolicy="no-referrer" />

        <center>
            <div class="container">
                <footer class="main-footer">
                    <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
                </footer>
            </div>
        </center>

        <asp:ScriptManager ID="sm1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

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
</script>

<script type="text/javascript">
    var submit = 0;
    function CheckDouble() {
        if (++submit > 1) {
            alert('Esto a veces tarda unos segundos, tenga paciencia.!');
            return false;
        }
    }
</script>
