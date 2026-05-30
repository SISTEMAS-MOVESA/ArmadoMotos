<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SeleccionarMoto.aspx.vb" Inherits="SeleccionarMoto" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Seleccione Moto</title>
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

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css">
<script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>


<!-- jQuery -->
<script src="plugins/jquery/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/2.9.2/umd/popper.min.js" integrity="sha512-2rNj2KJ+D8s1ceNasTIex6z4HWyOnEYLVC3FigGOmyQCZc2eBXKgOxQmo3oKLHyfcj53uz4QMsRCWNbLd32Q1g==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>

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


<!-- CSS para agregar márgenes y centrar el modal -->
<style>

</style>
</head>
<body>
    <form id="form1" runat="server">
         <!-- Inicio Modal Agregar Colores Moto-->

        <div class="container-fluid" id="maincontainer" runat="server">
            <h1 class="mx-3">Seleccionar Modelos y Colores a Enviar</h1>
            <div class="row mx-3">
                <asp:Label ID="lblruta" runat="server" Text=""></asp:Label>
                |
                <asp:Label ID="lblcardcode" runat="server" Text="Cardcode"></asp:Label>
                |
                <asp:Label ID="lblAlmOrigen" runat="server" Text="DCM00"></asp:Label>
                |
                <asp:Label ID="lblAlmDestino" runat="server" Text="almDestino"></asp:Label>
                |
                <asp:Label ID="lblSugerido" runat="server" Text="SUGERIDO"></asp:Label>
                |
                <asp:Label ID="lblCB" runat="server" Text="Cuadro Basico"></asp:Label>
                |
                <asp:Label ID="lblFISICO" runat="server" Text="Inv Ficiso"></asp:Label>
                |
                <asp:Label ID="lblFALTANTE" runat="server" Text="Faltante"></asp:Label>
                |
                <asp:Label ID="lblVTAA" runat="server" Text="Venta"></asp:Label>
                |
                <asp:Label ID="lblModeloCode" runat="server" Text="Codigo Modelo"></asp:Label>
                |
                <asp:Label ID="lblModeMoto" runat="server" Text="Modelo Moto"></asp:Label>
                |
                <asp:Label ID="lblplanId" runat="server" Text="Plan Id"></asp:Label>
            </div>
            <div class="row mx-3">
                <asp:GridView ID="gridColoresMoto" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" Width="100%">
                    <HeaderStyle CssClass="thead-dark" />
                    <Columns>
                        <asp:BoundField DataField="itemcode" HeaderText="Codigo Articulo" />
                        <asp:BoundField DataField="itemname" HeaderText="Descripcion Articulo" />
                        <asp:BoundField DataField="Modelo" HeaderText="Modelo" />
                        <asp:BoundField DataField="Espacios" HeaderText="Espacios" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="CEDIS" HeaderText="Inv. DCM00" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="SUCURSAL" HeaderText="Inv. Sucursal" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Cant">
                            <ItemTemplate>
                                <asp:TextBox ID="qty" runat="server" ClientIDMode="Static" Width="50px" Text="0" CssClass="txt_values text-right" type="Number" min="0"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

            </div>
        </div>
            <div class="row mx-3">
                <div class="col-6">
                    <asp:Button ID="btnCancelColoresMotos" runat="server" CssClass="btn btn-block btn-danger" Text="Cerrar Ventana" OnClientClick="window.close(); return false;" />
                </div>
                <div class="col-6">
                    <asp:Button ID="btnModalColoresMotos" runat="server" class="btn btn-block btn-success" Text="Agregar" />
                </div>
            </div>

  <!-- Fin Modal Agregar Colores Moto-->
    </form>
</body>
</html>
<script>
    setInterval(function () {
        fetch('/SeleccionarMoto.aspx');
    }, 5 * 60 * 1000)
</script>