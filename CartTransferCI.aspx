<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CartTransferCI.aspx.vb" Inherits="CartTransferCI" %>

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

    <!-- Bootstrap 4 -->
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>

    <%--izitoast--%>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>

    <%--steeper--%>
    <%--<link href="https://stackpath.bootstrapcdn.com/bootstrap/5.3.0/css/bootstrap.min.css" rel="stylesheet" />--%>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/5.3.0/js/bootstrap.bundle.min.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <!-- Navigation -->
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark static-top">
            <div class="container">
                <a class="navbar-brand" href="TrasladosDashboardSupervisores.aspx">
                    <img src="Imagenes/mnegra.png" width="50" height="40" alt="" />
                </a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarSupportedContent">
                    <ul class="navbar-nav ms-auto">
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="TrasladosDashboardSupervisores.aspx">Inicio</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="TrasladosPresolicitudSupervisores.aspx">Pre Solicitud</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="TrasladosTrasladosSupervisores.aspx">Traslados de Motos</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="TrasladosConfirmarSupervisor.aspx">Confirmacion Sugerido</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="Default.aspx">Cerrar Sesion</a>
                        </li>
                    </ul>
                </div>
               <%-- <a href="CartTransferCI.aspx" class="shopping-cart-icon">
                    <i class="fas fa-shopping-cart"></i>
                    <span id="cartItemCount" class="badge badge-pill badge-danger">
                        <asp:Label ID="lblCartCount" runat="server" Text="0"></asp:Label>
                    </span>
                </a>--%>
            </div>
        </nav>
        <style>
            .box {
                display: flex;
                justify-content: center;
                margin-block-start: 5px;
                margin-block-end: 5px;
            }

            .container-fluid {
                margin-top: 5px;
                margin-bottom: 5px;
                padding: 5px;
            }

            .divider {
                width: 4px;
                margin: 6px 0;
                background: black;
                align-self: stretch;
            }
        </style>
        <div class="container-fluid">
            <div class="box">
                <div>
                    <div class="container-fluid">
                        <h2>Almacen Origen</h2>
                        <div class="row my-3">
                            <div class="col">
                                <label for="txtNalmacenOrigen">Nombre Almacen</label>
                                <asp:TextBox ID="txtNalmacenOrigen" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" ServiceMethod="SearchWhsname"
                                    MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
                                    TargetControlID="txtNalmacenOrigen" FirstRowSelected="false">
                                </cc1:AutoCompleteExtender>
                            </div>
                            <div class="col">
                                <label for="btnAlmOrigen">Buscar</label>
                                <asp:Button ID="btnAlmOrigen" runat="server" class="btn btn-dark btn-block" Text="Buscar Alm" UseSubmitBehavior="False" />
                            </div>
                        </div>
                        <div class="row my-3">
                            <div class="col">
                                <label for="txtCodeAlmacenOrigen">Cogido</label>
                                <asp:TextBox ID="txtCodeAlmacenOrigen" runat="server" CssClass="form-control" Width="100%" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col">
                                <label for="txtRutaAlmacenOrigen">Ruta</label>
                                <asp:TextBox ID="txtRutaAlmacenOrigen" runat="server" CssClass="form-control" Width="100%" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col">
                                <label for="txtEncargadoOrigen">Encargado</label>
                                <asp:TextBox ID="txtEncargadoOrigen" runat="server" CssClass="form-control" Width="100%" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col">
                                <label for="txtEncargadoOrigen">Cliente</label>
                                <asp:TextBox ID="txtCodigoClienteOrigen" runat="server" CssClass="form-control" Width="100%" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row my-3">
                            <div class="col">
                                <label for="txtEstatusCliente">Estado</label>
                                <asp:TextBox ID="txtEstatusCliente" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col text-right">
                                <label for="txtLimiteCredito">Limite</label>
                                <asp:TextBox ID="txtLimiteCredito" runat="server" class="form-control text-right" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col text-right">
                                <label for="txtSaldoCuenta">Saldo Cuenta</label>
                                <asp:TextBox ID="txtSaldoCuenta" runat="server" class="form-control text-right" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col text-right">
                                <label for="txtSaldoConsignacion">Saldo Consig</label>
                                <asp:TextBox ID="txtSaldoConsignacion" runat="server" class="form-control text-right" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="divider"></div>
                <div>
                    <div class="container-fluid">
                        <h2>Almacen Destino</h2>
                        <div class="row my-3">
                            <div class="col">
                                <label for="txtNalmacenDestino">Nombre Almacen</label>
                                <asp:TextBox ID="txtNalmacenDestino" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServiceMethod="SearchWhsname"
                                    MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
                                    TargetControlID="txtNalmacenDestino" FirstRowSelected="false">
                                </cc1:AutoCompleteExtender>
                            </div>
                            <div class="col">
                                <label for="btnAlmDestino">Buscar</label>
                                <asp:Button ID="btnAlmDestino" runat="server" class="btn btn-dark btn-block" Text="Buscar Alm" UseSubmitBehavior="False" />
                            </div>
                            <div class="col">
                                <label for="btnAlmDestino">Limpiar</label>
                                <asp:Button ID="btnLimpiarFormulario" runat="server" class="btn btn-danger btn-block" Text="Limpiar Formulario" UseSubmitBehavior="False" />
                            </div>
                        </div>
                        <div class="row my-3">
                            <div class="col">
                                <label for="txtCodeAlmDestino">Codigo</label>
                                <asp:TextBox ID="txtCodeAlmDestino" runat="server" CssClass="form-control" Width="100%" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col">
                                <label for="txtRutaAlmacenDestino">Ruta</label>
                                <asp:TextBox ID="txtRutaAlmacenDestino" runat="server" CssClass="form-control" Width="100%" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col">
                                <label for="txtEncargadoDestino">Encargado</label>
                                <asp:TextBox ID="txtEncargadoDestino" runat="server" CssClass="form-control" Width="100%" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col">
                                <label for="txtEncargadoOrigen">Cliente</label>
                                <asp:TextBox ID="txtCodigoClienteDestino" runat="server" CssClass="form-control" Width="100%" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row my-3">
                            <div class="col">
                                <label for="txtEstadoClienteAD">Estado</label>
                                <asp:TextBox ID="txtEstadoClienteAD" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col text-right">
                                <label for="txtLimiteClienteAD">Limite</label>
                                <asp:TextBox ID="txtLimiteClienteAD" runat="server" class="form-control text-right" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col text-right">
                                <label for="txtSaldoClienteAD">Saldo Cuenta</label>
                                <asp:TextBox ID="txtSaldoClienteAD" runat="server" class="form-control text-right" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col text-right">
                                <label for="txtSaldoConsignaClienteAD">Saldo Consig</label>
                                <asp:TextBox ID="txtSaldoConsignaClienteAD" runat="server" class="form-control text-right" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="d-flex" style="justify-content: center; align-items: center;">
            <div class="row">
                <div class="col">
                    <asp:ListBox ID="lstSeriesDisponiblesAlmacen" runat="server" SelectionMode="Multiple" class="demo" Height="400px" Width="600px"></asp:ListBox>
                    <link rel="stylesheet" type="text/css" href="dual-listbox.css" />
                    <script type="text/javascript" src="dual-listbox.js"></script>
                    <script type="text/javascript">
                        new DualListbox('.demo', {
                            addEvent: function (value) { },
                            removeEvent: function (value) { },
                            availableTitle: 'Series Disponibles',
                            selectedTitle: 'Series Seleccionados',
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
        <div class="container-fluid">
            <div class="row my-2">
                <div class="col">
                    <label for="txtComments">Comentarios de la Solicitud de Traslados</label>
                    <asp:TextBox ID="txtComments" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Comentarios del Traslado"></asp:TextBox>
                </div>
            </div>
            <div class="row my-2">
                <div class="col">
                    <label for="drpMotoristas">Seleccione Motivo Traslado</label>
                    <asp:DropDownList ID="drpMotivoTraslado" runat="server" class="ui search dropdown fluid" name="drpMotivoTraslado" Width="100%"></asp:DropDownList>
                </div>
                <div class="col">
                <label for="drpTipoTransporte">Seleccione Tipo Transporte</label>
                    <asp:DropDownList ID="drpTipoTransporte" runat="server" class="ui search dropdown fluid" name="drpMotoristas" Width="100%">
                        <asp:ListItem Value="01" Text="Transporte Propio"></asp:ListItem>
                        <asp:ListItem Value="02" Text="Transporte Externo"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col">
                    <label for="drpMotoristas">Seleccione Motorista</label>
                    <asp:DropDownList ID="drpMotoristas" runat="server" class="ui search dropdown fluid" name="drpMotoristas" Width="100%">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row my-2">
                <div class="col">
                    <label for="btnCrearSOlicitud">Crear Documento</label>
                    <asp:Button ID="btnCrearSOlicitud" runat="server" CssClass="btn btn-block btn-success" Text="Crear Solicitu de Traslado" />
                </div>
            </div>
        </div>
        <div class="d-flex" style="justify-content: center; align-items: center;">
            <div class="container">
                <footer class="main-footer">
                    <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
                </footer>
            </div>
        </div>

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

<script type="text/javascript">

    $("#gridInventarioMoto").DataTable({
        "responsive": true,
        "lengthChange": false,
        "autoWidth": false,
        "buttons": ["excel", "pdf", "colvis"],
        "sort": true,
        "order": [[0, 'asc']],
        "deferRender": true,
        "lengthMenu": [[20, -1], [20, "All"]],
    }).buttons().container().appendTo('#gridInventarioMoto_wrapper .col-md-6:eq(0)');

</script>

<link href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.css" rel="stylesheet" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.js"></script>
<script type="text/javascript">
    var submit = 0;
    function CheckDouble() {
        if (++submit > 1) {
            alert('Esto a veces tarda unos segundos, tenga paciencia.!');
            return false;
        }
    }
</script>


<link href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.css" rel="stylesheet" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.js"></script>

<script>
    $("#drpMotoristas").dropdown(
        {
            "fullTextSearch": true
        });
    $("#drpMotivoTraslado").dropdown(
        {
            "fullTextSearch": true
        });
</script>
