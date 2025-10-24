<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AuditoriaCobroRepuestos.aspx.vb" Inherits="AuditoriaCobroRepuestos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Inventario Motos</title>
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

    <link href="https://cdnjs.cloudflare.com/ajax/libs/dropzone/5.9.3/dropzone.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/dropzone/5.9.3/dropzone.min.js"></script>
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/responsive/2.5.0/css/responsive.dataTables.min.css" />

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.min.css">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>

</head>
<body>
    <form id="form1" runat="server">
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
                            <a class="nav-link" href="DashboardAuditoria.aspx">Inicio</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="AuditoriaInventarioArmadas.aspx">Inventario Motos Armadas</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#">Auditoria CC</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="AuditoriaInformeInventario.aspx">Informes</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="Default.aspx">Cerrar Sesion</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
        <div class="container-fluid px-4">
            <h1 class="mt-4">Informacion Cliente</h1>
            <div class="row mt-3">
                <div class="col-6">
                    <label for="txtNombreCliente">Digite Nombre del Cliente</label>
                    <asp:TextBox ID="txtNombreCliente" runat="server" CssClass="form-control"></asp:TextBox>
                    <cc1:AutoCompleteExtender ID="acCustomer" runat="server" ServiceMethod="SearchCustomers"
                        MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
                        TargetControlID="txtNombreCliente" FirstRowSelected="false">
                    </cc1:AutoCompleteExtender>
                </div>
                <div class="col-2">
                    <asp:Button ID="btnBuscarCliente" runat="server" CssClass="btn btn-block btn-info h-100" Text="Buscar Cliente" />
                </div>
            </div>
            <div class="row">
                <div class="col-2">
                    <label for="txtcardcode">Codigo cliente</label>
                    <asp:TextBox ID="txtcardcode" runat="server" CssClass="form-control" placeholder="Codigo Cliente" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-2">
                    <label for="txtRTN">RTN</label>
                    <asp:TextBox ID="txtRTN" runat="server" CssClass="form-control" Text="N/A"></asp:TextBox>
                </div>
                <div class="col-8">
                    <label for="txtCardName">Nombre Cliente</label>
                    <asp:TextBox ID="txtCardName" runat="server" CssClass="form-control" placeholder="Nombre Cliente" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <hr />
            <h1 class="mt-4">Agregar Repuesto</h1>
            <div class="row mt-3">
                <div class="col-6">
                    <label for="txtDescripcionRepuesto">Descripcion Repuesto</label>
                    <asp:TextBox ID="txtDescripcionRepuesto" runat="server" CssClass="form-control"></asp:TextBox>
                    <cc1:AutoCompleteExtender ID="acItems" runat="server" ServiceMethod="SearchItems"
                        MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
                        TargetControlID="txtDescripcionRepuesto" FirstRowSelected="false">
                    </cc1:AutoCompleteExtender>
                </div>
                <div class="col-2">
                    <asp:Button ID="btnBuscarInfoItem" runat="server" class="btn btn-secondary btn-block h-100" Text="Buscar" UseSubmitBehavior="False" />
                </div>

            </div>
            <div class="row mt-3">
                <div class="col-2">
                    <label for="txtItemcode">Codigo</label>
                    <asp:TextBox ID="txtItemcode" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-4">
                    <label for="txtItemname">Descripcion</label>
                    <asp:TextBox ID="txtItemname" runat="server" CssClass="form-control" ReadOnly="true" TextMode="MultiLine"></asp:TextBox>
                </div>
                <div class="col-1">
                    <label for="txtCantidad">Cantidad</label>
                    <asp:TextBox ID="txtCantidad" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-2">
                    <label for="txtPrecio">Precio</label>
                    <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-2">
                    <label for="txtImpuesto">Impuesto</label>
                    <asp:TextBox ID="txtImpuesto" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-1">
                    <label for="txtAlmacenCustomerDestino">Almacen</label>
                    <asp:TextBox ID="txtAlmacenCustomerDestino" runat="server" CssClass="form-control" ReadOnly="true" Text="DCR00"></asp:TextBox>
                </div>
            </div>
            <div class="row mt-3">
                <div class="col-1">
                    <label for="btnAgregarItem">Agregar</label>
                    <asp:Button ID="btnAgregarItem" runat="server" class="btn btn-success btn-block" Text="Agregar" UseSubmitBehavior="False" />
                </div>
            </div>
            <hr />
        </div>
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
                                <h3 class="card-title">Resumen Cobro </h3>
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
                                        <asp:GridView ID="gridOrdenTemporal" runat="server" CssClass="table table-bordered table-hover"
                                            AutoGenerateColumns="false" ShowFooter="true">
                                            <HeaderStyle CssClass="thead-dark" />
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderText="Id" />
                                                <asp:BoundField DataField="ITEMCODE" HeaderText="Articulo" />
                                                <asp:BoundField DataField="ITENMANE" HeaderText="Descripcion" />
                                                <asp:BoundField DataField="PRICE" HeaderText="Precio" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="IMPUESTO" HeaderText="Impuesto" />
                                                <asp:BoundField DataField="TOTALLINEA" HeaderText="Total Linea" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="TOTAL" HeaderText="Total General" ItemStyle-HorizontalAlign="Center" />
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
        <section class="content">
            <div class="container-fluid">
                <div class="row mt-2">
                    <div class="col-2">
                        <label for="txtSerieMoto">Serie de la Moto</label>
                        <asp:TextBox ID="txtSerieMoto" runat="server" CssClass="form-control" placeholder="Numero Serie de la Moto"></asp:TextBox>
                    </div>
                </div>
                <div class="row mt-2">
                    <div class="col-12">
                        <label for="txtComments">Comentarios del Documento</label>
                        <asp:TextBox ID="txtComments" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Comentarios del Documento"></asp:TextBox>
                    </div>
                </div>
                <div class="row mt-2">
                    <div class="col-2">
                            <asp:Button ID="btnEnviarSAP" runat="server" CssClass="btn btn-block btn-success" Text="Enviar Orden a SAP" />
                    </div>
                </div>
            </div>
        </section>
        <div class="container" style="flex; justify-content: center;">
            <footer class="main-footer">
                <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
            </footer>
        </div>
        <asp:ScriptManager ID="sm1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
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

<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
<link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />
<script src="https://cdn.datatables.net/responsive/2.5.0/js/dataTables.responsive.min.js"></script>

