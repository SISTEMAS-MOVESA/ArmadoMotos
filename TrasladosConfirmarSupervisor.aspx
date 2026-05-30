<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosConfirmarSupervisor.aspx.vb" Inherits="TrasladosConfirmarSupervisor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Confirmacion Supervisor</title>
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


    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>

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
                            <a class="nav-link active" aria-current="page" href="TrasladosConfirmarSupervisor.aspx">Confirmacion Sugerido</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="Default.aspx">Cerrar Sesion</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>

        <div class="container-fluid">
            <div class="row my-3">
                <div class="col">
                    <label for="txtHeaderid">Digite el Numero de Sugerido</label>
                    <asp:TextBox ID="txtHeaderid" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col">
                    <label for="btnBuscarSugerido">Buscar</label>
                    <asp:Button ID="btnBuscarSugerido" runat="server" CssClass="btn btn-block btn-secondary" Text="Buscar" />
                </div>
                <div class="col">
                    <label for="btnGuardarPlanificacion">Guardar</label>
                    <asp:Button ID="btnGuardarPlanificacion" runat="server" class="btn btn-success" Text="Guardar" Width="100%" />
                </div>
                <div class="col">
                    <label for="txtEspaciosAsignados">Espacios Asignados</label>
                    <asp:TextBox ID="txtEspaciosAsignados" runat="server" CssClass="form-control text-right" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col">
                    <label for="txtEspaciosDisponibles">Espacios Disponibles</label>
                    <asp:TextBox ID="txtEspaciosDisponibles" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col">
                    <label for="txtAlmDestino">Almacen Destino</label>
                    <asp:TextBox ID="txtAlmDestino" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col">
                    <label for="txtNalmacenDestino">Almacen Destino</label>
                    <asp:TextBox ID="txtNalmacenDestino" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="contaiiner-fluid">
            <div class="card-header text-center">
                <h2>Supervisor: <%=Session("UserCode") %> |
                    <asp:Label ID="lblNSupervisor" runat="server"></asp:Label>
                    | <%=Session("ALMTRANSIT") %> | <%=Session("CODIGOSUP") %></h2>
                <br />
                <div class="container-fluid">
                    <div class="col">
                        <asp:GridView ID="gridPedidoTemp" runat="server" AutoGenerateColumns="false" class="ui unstackable celled long scrolling compact table" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="Id" />
                                <asp:BoundField DataField="HEADERID" HeaderText="HId" />
                                <asp:BoundField DataField="RUTA" HeaderText="Ruta" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ALMDESTINO" HeaderText="Destino" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ALMACEN" HeaderText="Destino" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ARTICULO" HeaderText="Articulo" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="MODELO" HeaderText="Modelo" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ESPACIOS" HeaderText="Espacios" />
                                <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" />
                                <asp:BoundField DataField="QTYLOGISTICA" HeaderText="Logistica" />
                                <asp:TemplateField HeaderText="Cant">
                                    <ItemTemplate>
                                        <asp:TextBox ID="qty" runat="server" ClientIDMode="Static" EnableViewState="true" Width="60px" Height="100%" CssClass="txt_values text-right" type="Number" min="0"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <footer class="main-footer text-center">
            <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
        </footer>

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

        <script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.js" integrity="sha512-Xo0Jh8MsOn72LGV8kU5LsclG7SUzJsWGhXbWcYs2MAmChkQzwiW/yTQwdJ8w6UA9C6EVG18GHb/TrYpYCjyAQw==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.css" integrity="sha512-KXol4x3sVoO+8ZsWPFI/r5KBVB/ssCGB5tsv2nVOKwLg33wTFP3fmnXa47FdSVIshVTgsYk/1734xSk9aFIa4A==" crossorigin="anonymous" referrerpolicy="no-referrer" />

        <script src="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.2/semantic.min.js" integrity="sha512-5cguXwRllb+6bcc2pogwIeQmQPXEzn2ddsqAexIBhh7FO1z5Hkek1J9mrK2+rmZCTU6b6pERxI7acnp1MpAg4Q==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.2/semantic.min.css" integrity="sha512-n//BDM4vMPvyca4bJjZPDh7hlqsQ7hqbP9RH18GF2hTXBY5amBwM2501M0GPiwCU/v9Tor2m13GOTFjk00tkQA==" crossorigin="anonymous" referrerpolicy="no-referrer" />


    </form>
</body>
</html>
    
