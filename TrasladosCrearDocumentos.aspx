<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosCrearDocumentos.aspx.vb" Inherits="TrasladosCrearDocumentos" %>

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
    <link href="css/TasksCards.css" rel="stylesheet" />

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>


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
        <div class="d-flex justify-content-center">
            <asp:GridView ID="gridCargaCamion" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" Width="100%">
                <HeaderStyle CssClass="thead-dark" />
                <Columns>
                    <asp:BoundField DataField="CAMION" HeaderText="Id" />
                    <asp:BoundField DataField="PREIDHEADER" HeaderText="Header Id" />
                    <asp:BoundField DataField="WHSCODE" HeaderText="Cod. Almacen" />
                    <asp:BoundField DataField="DOCENTRYSAP" HeaderText="Documento SAP" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="Solicitado" HeaderText="Solicitado" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="Preparado" HeaderText="Preparado" ItemStyle-HorizontalAlign="Left" />
                    <%--<asp:BoundField DataField="Porcentaje" HeaderText="Porcentaje" ItemStyle-HorizontalAlign="Left" />--%>
                    <asp:TemplateField HeaderText="Barra de progreso">
                        <ItemTemplate>
                            <div class="progress">
                                <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar"
                                    aria-valuemin="0"
                                    aria-valuenow='<%# Eval("Preparado") %>'
                                    aria-valuemax='<%# Eval("Solicitado") %>'
                                    style='width: <%# Eval("Porcentaje") %>%'>
                                    <%# FormatNumber(Eval("Porcentaje"), 2, TriState.True) %>%
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:ButtonField ButtonType="Image" ImageUrl="~/images/5098814.png" Text="Editar" CommandName="Series" HeaderText="Series"
                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ControlStyle Height="50px" Width="50px" />
                        <ItemStyle Wrap="False" />
                    </asp:ButtonField>
                </Columns>
            </asp:GridView>
        </div>

        <br />


        <%--INICIO DETALLES DE SOLICITUD--%>
        <%-- <div class="container">
            <div class="row">
                <asp:Repeater ID="repeater1" runat="server">
                    <ItemTemplate>
                        <div class="col-xl-4 col-md-6">
                            <div class="card">
                                <div class="card-block">
                                    <div class="row align-items-center justify-content-center">
                                        <div class="col-auto">
                                            <img class="img-fluid rounded-circle" style="width: 70px;" src="images/5098814.png" alt="dashboard-user">
                                        </div>
                                        <div class="col">
                                            <h2># <%# Eval("PREIDHEADER") %></h2>
                                            <span>SUCURSAL: <%# Eval("WHSCODE") %></span><br />
                                            </span>Camion: <%# Eval("CAMION") %></span>
                                            <br />
                                            <span>Solicitado: <%# Eval("Solicitado") %></span>
                                            <br>
                                            <span>Preparado: <%# Eval("Preparado") %></span>
                                            <br />
                                            <div class="progress">
                                                <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar"
                                                    aria-valuemin="0"
                                                    aria-valuenow='<%# Eval("Preparado") %>'
                                                    aria-valuemax='<%# Eval("Solicitado") %>'
                                                    style='width: <%# Eval("Porcentaje") %>%'>
                                                    <%#FormatNumber(Eval("Porcentaje"), 2, TriState.True) %>%
                                                </div>
                                            </div>
                                            <hr />
                                            <span>Docentry SAP: <%# Eval("DOCENTRYSAP") %></span>
                                            <hr />
                                            <asp:Button ID="btnNueva" runat="server" class="btn btn-dark" CommandArgument='<%# Eval("CAMION")%>'
                                                Text="Crear Solicitud" Width="100%" Height="100%" OnClick="btnAll_Click" />
                                            <hr />
                                            <asp:Button ID="btnConvert" runat="server" class="btn btn-dark" CommandArgument='<%# Eval("DOCENTRYSAP") %>'
                                                Text="Convertir a Transferencia" Width="100%" Height="100%" OnClick="btn_Transfer" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>--%>


        <!-- Inicio Modal Confirmar Vehiculo-->
        <div class="modal fade" id="ModalMotorista" tabindex="-1" role="dialog" aria-labelledby="ModalMotoristaLabel"
            aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="ModalMotoristaLabel">Agregar Serie</h5>
                        <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">×</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-4">Numero de Camion</div>
                                <div class="col-8">
                                    <asp:Label ID="lblCamion" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-4">Seleccione Motivo Traslado</div>
                                <div class="col-8">
                                    <asp:DropDownList ID="drpMotivoTraslado" runat="server" class="ui search dropdown fluid" name="drpMotivoTraslado" Width="100%"></asp:DropDownList>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-4">Seleccione Tipo Transporte</div>
                                <div class="col-8">
                                    <asp:DropDownList ID="drpTipoTransporte" runat="server" class="ui search dropdown fluid" name="drpMotoristas" Width="100%">
                                        <asp:ListItem Value="01" Text="Transporte Propio"></asp:ListItem>
                                        <asp:ListItem Value="02" Text="Transporte Externo"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-4">Seleccione Motorista</div>
                                <div class="col-8">
                                    <asp:DropDownList ID="drpMotoristas" runat="server" class="ui search dropdown fluid" name="drpMotoristas" Width="100%">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-4">LLeva Casco?</div>
                                <div class="col-2">
                                    <asp:CheckBox ID="chkCasco" runat="server" CssClass="form-control" Checked="true"/>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-4">Codigo de Casco</div>
                                <div class="col-8">
                                    <asp:TextBox ID="txtCodigoCasco" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-4">Cantidad</div>
                                <div class="col-8">
                                    <asp:TextBox ID="txtQtyCasco" runat="server" CssClass="form-control" type="number"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button class="btn btn-secondary" type="button" data-dismiss="modal">Cancel</button>
                        <asp:Button ID="btnConfirmarSolicitudTraslado" runat="server" class="btn btn-primary" Text="Guardar Solicitud" />
                    </div>
                </div>
            </div>
        </div>
        <!-- Fin Modal Confirmar Vehiculo-->




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
<script type="text/javascript">
    //Datatable responsive tables:
    $("#gridCargaCamion").DataTable({
        "responsive": true,
        "lengthChange": false,
        "autoWidth": false,
        "buttons": ["excel", "pdf", "colvis"],
        "sort": true,
        "order": [[0, 'desc']],
        "deferRender": true,
        "lengthMenu": [[20, -1], [20, "All"]],
    }).buttons().container().appendTo('#gridCargaCamion_wrapper .col-md-6:eq(0)');
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
