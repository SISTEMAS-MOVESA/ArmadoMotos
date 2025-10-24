<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosConfirmLogistica.aspx.vb" Inherits="TrasladosConfirmLogistica" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Confirmacion Logistica</title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback" />
    <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css" />
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css" />
    <link rel="stylesheet" href="plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css" />
    <link rel="stylesheet" href="plugins/icheck-bootstrap/icheck-bootstrap.min.css" />
    <link rel="stylesheet" href="plugins/jqvmap/jqvmap.min.css" />
    <link rel="stylesheet" href="dist/css/adminlte.min.css" />
    <link rel="stylesheet" href="plugins/overlayScrollbars/css/OverlayScrollbars.min.css" />
    <link rel="stylesheet" href="plugins/daterangepicker/daterangepicker.css" />
    <link rel="stylesheet" href="plugins/summernote/summernote-bs4.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
    <%--<link href="css/movesa_load_spinner.css" rel="stylesheet" />--%>
    <%--<link href="css/TasksCards.css" rel="stylesheet" />--%>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css" />
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
        <br />
        <div class="d-flex justify-content-center">
            <div class="col-2">
                <label for="txtFechaArme">Fecha Provista de Preparacion</label>
                <asp:TextBox ID="txtFechaArme" runat="server" ClientIDMode="Static" type="date" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-2">
                <label for="txtFechaEnvio">Fecha Provista de Despacho.</label>
                <asp:TextBox ID="txtFechaEnvio" runat="server" ClientIDMode="Static" type="date" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <br />
        <div class="d-flex justify-content-center">

            <asp:GridView ID="gridPedidoTemp" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" Width="100%"
                OnRowDataBound="gridPedidoTemp_RowDataBound" ShowFooter="true">
                <HeaderStyle CssClass="thead-dark" />
                <Columns>
                    <asp:BoundField DataField="FECHA" HeaderText="Fecha" />
                    <asp:BoundField DataField="ID" HeaderText="Id" />
                    <asp:BoundField DataField="LID" HeaderText="Line Id" />
                    <asp:BoundField DataField="ALMACEN" HeaderText="Almacen" />
                    <asp:BoundField DataField="ESTADO" HeaderText="Observaciones" />
                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" />
                    <asp:BoundField DataField="ARTICULO" HeaderText="Articulo" />
                    <asp:BoundField DataField="MODELO" HeaderText="Modelo" />
                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion" />
                    <asp:BoundField DataField="ESPACIOS" HeaderText="Espacios" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="QTYLOG" HeaderText="Qty Sugerida Log" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="QTYSUC" HeaderText="Qty Confirmada Suc" ItemStyle-HorizontalAlign="Right" />
                    <asp:TemplateField HeaderText="Qty Aprobar Log">
                        <ItemTemplate>
                            <div class="d-flex align-items-center">
                                <asp:Button ID="btnMinus" runat="server" CssClass="btn btn-danger btn-sm mr-2" Text="-" OnClick="btnMinus_Click" />
                                <asp:TextBox ID="qty" runat="server" ClientIDMode="Static" Width="60px" Height="100%" CssClass="txt_values text-right" type="Number" min="0"></asp:TextBox>
                                <asp:Button ID="btnPlus" runat="server" CssClass="btn btn-success btn-sm ml-2" Text="+" OnClick="btnPlus_Click" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/delete.png" Text="Eliminar" CommandName="Eliminar" HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ControlStyle Height="30px" Width="30px" />
                        <ItemStyle Wrap="False" />
                    </asp:ButtonField>
                    <asp:BoundField DataField="CARDCODE" HeaderText="Cardcode" />
                    <asp:BoundField DataField="HEADERID" HeaderText="Macro ID" />
                </Columns>
            </asp:GridView>
        </div>
        <hr />
        <div class="d-flex justify-content-center">

            <div class="col-4">
                <asp:Button ID="btnCrearSolicitud" runat="server" class="btn btn-success btn-lg" Text="Crear Solicitud" Width="100%" Height="100%" />
            </div>
            <div class="col-4">
                <asp:Button ID="btnRegresar" runat="server" class="btn btn-dark btn-lg" Text="Regresar" Width="100%" Height="100%" />
            </div>
        </div>
        <hr />
       <%-- <div class="d-flex justify-content-center">
        <h2> HISTORIAL </h2>
            </div>
        <div class="d-flex justify-content-center">

            <asp:GridView ID="grisHistorial" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" Width="100%">
                <HeaderStyle CssClass="thead-dark" />
                <Columns>
                    <asp:BoundField DataField="FECHA" HeaderText="Fecha" />
                    <asp:BoundField DataField="DESTINO" HeaderText="Destino" />
                    <asp:BoundField DataField="ID" HeaderText="Id" />
                    <asp:BoundField DataField="LID" HeaderText="Line Id" />
                    <asp:BoundField DataField="ALMACEN" HeaderText="Almacen" />
                    <asp:BoundField DataField="RUTA" HeaderText="Ruta" />
                    <asp:BoundField DataField="OBSERACIONES" HeaderText="Observaciones" />
                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" />
                    <asp:BoundField DataField="ARTICULO" HeaderText="Articulo" />
                    <asp:BoundField DataField="MODELO" HeaderText="Modelo" />
                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion" />
                    <asp:BoundField DataField="ESPACIOS" HeaderText="Espacios" />
                    <asp:BoundField DataField="CANTIDAD" HeaderText="Qty Sucursal" />
                    <asp:BoundField DataField="QTYLOG" HeaderText="Logistica" />
                </Columns>
            </asp:GridView>
        </div>--%>
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

<link href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.css" rel="stylesheet" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.js"></script>
