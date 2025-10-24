<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosCargaMacroBI.aspx.vb" Inherits="TrasladosCargaMacroBI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Analisis de Datos</title>
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

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>

    <!-- CSS para agregar márgenes y centrar el modal -->
    <style>
        .ui.modal {
            top: 50% !important;
            left: 50% !important;
            transform: translate(-50%, -50%) !important;
            position: fixed !important;
            margin: 0 !important;
        }

        #modalContent {
            max-height: 60vh;
            overflow-y: auto;
        }

        .ui.modal .actions {
            padding: 10px;
            margin: 0;
        }

        .sticky-header th {
            position: sticky;
            top: 0;
            background: #1b1c1d; /* Ajusta el color del encabezado según tus necesidades */
            z-index: 2;
        }
    </style>

</head>
<body>

    <form id="form1" runat="server">
        <!-- Navigation -->
        <div class="container-fluid my-3">
            <h1>ANALISIS BI</h1> <asp:Label ID="lblModeloMoto" runat="server"></asp:Label>
        </div>
        <div class="container-fluid">
            <div class="ui two column grid" style="margin-left: 20px; margin-right: 20px">
                <div class="column">
                    <div class="ui raised segment">
                        <a class="ui red ribbon label">Ranking Global</a>
                        <hr />
                        <p>Rankings Globales del Modelo 
                            <b>
                                <asp:Label ID="lblModelo" runat="server" Text=""></asp:Label>
                            </b>
                        </p>
                        <asp:DropDownList ID="drpModelosMotos" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                        <br />
                        <div style="max-height: 60vh; overflow-y: auto; position: relative;">
                            <div class="container-fluid">
                                <asp:GridView ID="gridRanking" runat="server" AutoGenerateColumns="false" CssClass="ui celled table sticky-header" Width="100%">
                                    <HeaderStyle CssClass="ui blue inverted table" />
                                    <Columns>
                                        <asp:BoundField DataField="Row" HeaderText="Ranking" />
                                        <asp:BoundField DataField="whscode" HeaderText="Codigo" />
                                        <asp:BoundField DataField="Nombre Almacen" HeaderText="Almacen" />
                                        <asp:BoundField DataField="Monto" HeaderText="Cantidad" DataFormatString="{0:N0}"/>
                                        <asp:BoundField DataField="ParetoPercent" HeaderText="%Pareto" DataFormatString="{0:P2}"/>
                                        <asp:BoundField DataField="CB" HeaderText="CB" />
                                        <asp:BoundField DataField="Comprometido" HeaderText="Comp" />
                                        <asp:BoundField DataField="Solicitado" HeaderText="Sol" />
                                        <asp:BoundField DataField="Transito" HeaderText="Tran" />
                                        <asp:BoundField DataField="Fisico" HeaderText="Fis" />
                                        <asp:TemplateField HeaderText="Falt">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFaltante" runat="server" Text="0"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="column">
                    <div class="ui segment">
                        <a class="ui red right ribbon label">Existencias Globales</a>
                        <hr />
                        <p>Distribucion Stock Por Canal</p>
                        <div style="max-height: 20vh; overflow-y: auto; position: relative;">
                            <div class="container-fluid">
                                <asp:GridView ID="gridExistencias" runat="server" AutoGenerateColumns="true" CssClass="ui celled table sticky-header" Width="100%">
                                    <HeaderStyle CssClass="ui blue inverted table" />
                                </asp:GridView>
                            </div>
                        </div>
                        <br />
                        <div style="max-height: 48vh; overflow-y: auto; position: relative;">
                            <div class="container-fluid">
                                <asp:GridView ID="gridStockAlmacen" runat="server" AutoGenerateColumns="true" CssClass="ui celled table sticky-header" Width="100%">
                                    <HeaderStyle CssClass="ui blue inverted table" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.css" />
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.js" integrity="sha512-gnoBksrDbaMnlE0rhhkcx3iwzvgBGz6mOEj4/Y5ZY09n55dYddx6+WYc72A55qEesV8VX2iMomteIwobeGK1BQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.css" integrity="sha512-3quBdRGJyLy79hzhDDcBzANW+mVqPctrGCfIPosHQtMKb3rKsCxfyslzwlz2wj1dT8A7UX+sEvDjaUv+WExQrA==" crossorigin="anonymous" referrerpolicy="no-referrer" />

      

        <footer class="main-footer text-center">
            <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
        </footer>
        <asp:ScriptManager ID="sm1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

    </form>
</body>
</html>
<!-- DataTables & Plugins -->
<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
<script src="https://cdn.datatables.net/buttons/1.7.1/js/dataTables.buttons.min.js"></script>
<script src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.html5.min.js"></script>
<script src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.print.min.js"></script>
<script src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.colVis.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
<link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />
<link href="https://cdn.datatables.net/buttons/1.7.1/css/buttons.dataTables.min.css" rel="stylesheet" type="text/css" />
<script src="https://cdn.datatables.net/responsive/2.5.0/js/dataTables.responsive.min.js"></script>

<script type="text/javascript">
    $(function () {
        $('table[id$="gridRanking"]').DataTable({
            dom: 'Bfrtip',
            buttons: [
                'excelHtml5',
            ],
            responsive: true,
            bLengthChange: true,
            bFilter: true,
            bSort: true,
            bPaginate: false,
            order: [[0, "asc"]]
        });
    });
    $(function () {
        $('table[id$="gridStockAlmacen"]').DataTable({
            dom: 'Bfrtip',
            buttons: [
                'excelHtml5',
            ],
            responsive: true,
            bLengthChange: true,
            bFilter: true,
            bSort: true,
            bPaginate: false,
            order: [[2, "desc"]]
        });
    });

</script>
