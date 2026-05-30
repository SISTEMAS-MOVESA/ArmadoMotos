<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AuditoriaInformeInventario.aspx.vb" Inherits="AuditoriaInformeInventario" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Auditoria || Informes</title>
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
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/2.9.2/umd/popper.min.js" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/responsive/2.5.0/css/responsive.dataTables.min.css" />

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
        <div class="container-fluid" style="flex:auto; justify-content:center;">
            <h1 class="text-center">Informe Motos Auditadas</h1>
            <div class="row my-3">
                <div class="col-2">
                    <label for="txtDesde">Desde</label>
                    <asp:TextBox ID="txtDesde" runat="server" type="date" CssClass="form-control" Width="100%"></asp:TextBox>
                </div>
                <div class="col-2">
                    <label for="txtHasta">Hasta</label>
                    <asp:TextBox ID="txtHasta" runat="server" type="date" CssClass="form-control" Width="100%"></asp:TextBox>
                </div>
                <div class="col-3">
                    <label for="btnBuscar" style="justify-content:center;">Cargar</label>
                    <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-success justify-content-center align-content-center" Text="Cargar Datos" Width="100%" />
                </div>
                <div class="col-3">
                    <label for="btnPivot" style="justify-content: center;">Pivot</label>
                    <asp:Button ID="btnPivot" runat="server" CssClass="btn btn-success justify-content-center align-content-center" Text="Ir al Pivot" Width="100%" />
                </div>
            </div>
            <div class="row my-3">
                <div class="table-responsive">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered table-hover" Width="100%">
                        <HeaderStyle CssClass="thead-dark" />
                        <Columns>
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/Imagenes/find2.png" Text="Expediente" CommandName="Expediente" HeaderText="Expediente">
                                <ControlStyle Height="50px" Width="50px" />
                                <ItemStyle Wrap="False" />
                            </asp:ButtonField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="container" style="flex; justify-content:center;">
            <footer class="main-footer">
                <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
            </footer>
        </div>
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
        $("[id*=GridView1]").DataTable({
            dom: 'Bfrtip',
            buttons: [
                'excelHtml5',
                'pdfHtml5',
                'colvis'
            ],
            responsive: true,
            bLengthChange: true,
            lengthMenu: [[10, -1], [10, "All"]],
            bFilter: true,
            bSort: true,
            bPaginate: true,
            order: [[1, "desc"]]
        });
    });
</script>
