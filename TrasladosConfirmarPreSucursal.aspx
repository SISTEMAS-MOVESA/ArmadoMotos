<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosConfirmarPreSucursal.aspx.vb" Inherits="TrasladosConfirmarPreSucursal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Confirmacion Sucursal</title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.3.0/css/all.min.css" />
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
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>


    <style>
        thead input {
            width: 100%;
        }

        parent {
            Display: flex;
            align-items: center;
        }

        .jumbotron-fluid {
            position: fixed;
            top: 0;
            left: 0;
            right: 0;
            z-index: 999;
        }
    </style>
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
            <div class="row" style="justify-content: center;">
                <div class="col-2">
                    <label for="txtNumeroSugerido"># Sugerido</label>
                    <asp:TextBox ID="txtNumeroSugerido" runat="server" CssClass="form-control text-right" ></asp:TextBox>
                </div>
                <div class="col-2">
                    <label for="btnBuscarSugerido">Buscar Sugerido</label>
                    <asp:Button ID="btnBuscarSugerido" runat="server" class="btn btn-secondary" Text="Buscar" Width="100%" />
                </div>
                <div class="col-2">
                    <label for="btnGuardarPlanificacion">Enviar Confirmacion</label>
                    <asp:Button ID="btnGuardarPlanificacion" runat="server" class="btn btn-success" Text="Guardar" Width="100%" />
                </div>
                <div class="col-2">
                    <label for="txtEspaciosAsignados">Espacios Asignados</label>
                    <asp:TextBox ID="txtEspaciosAsignados" runat="server" CssClass="form-control text-right" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-2">
                    <label for="txtEspaciosDisponibles">Espacios Disponibles</label>
                    <asp:TextBox ID="txtEspaciosDisponibles" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
        <br />
                <div class="container-fluid">
                    <div class="col">
                        <asp:GridView ID="gridPedidoTemp" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" Width="100%">
                            <HeaderStyle CssClass="thead-dark" />
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="Id" />
                                <asp:BoundField DataField="HEADERID" HeaderText="HId" />
                                <asp:BoundField DataField="RUTA" HeaderText="Ruta" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ALMDESTINO" HeaderText="Destino" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ARTICULO" HeaderText="Articulo" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="MODELO" HeaderText="Modelo" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ESPACIOS" HeaderText="Espacios" />
                                <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" />
                                <asp:BoundField DataField="QTYLOGISTICA" HeaderText="Logistica" />
                                <asp:TemplateField HeaderText="Cant">
                                    <ItemTemplate>
                                        <asp:TextBox ID="qty" runat="server" ClientIDMode="Static" Width="60px" Height="100%" CssClass="txt_values text-right" type="Number" min="0"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CB" HeaderText="Cuadro Basico" />
                                <asp:BoundField DataField="FISICO" HeaderText="Fisico Suc" />
                                <asp:BoundField DataField="FALTANTE" HeaderText="Faltante" />
                                <asp:BoundField DataField="VTAA" HeaderText="Venta Mes" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
        <footer class="main-footer text-center">
            <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
        </footer>

        <%--    <script src="js/jquery.min.js"></script>
    <script src="js/popper.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/main.js"></script>--%>
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


<script>
    $(window).on('load', function () {
        $("#loftloader-wrapper").hide();
    });

    window.onbeforeunload = function (e) {
        $("#loftloader-wrapper").show();
    }
</script>
<script type="text/javascript">

    //$("table").DataTable({
    //    "responsive": true,
    //    "lengthChange": false,
    //    "autoWidth": false,
    //    "paging": false,
    //    "sort": true,
    //    "order": [[13, 'desc']],
    //    "deferRender": true,
    //});
    //$("table thead tr th").css({ "background": "black", "color": "white" });


    //$("table").each(function (i, table) {

    //    $(table).find("tbody tr").each(function (j, tr) {

    //        const faltante = $(tr).find("td").eq(13).html() == "" ? 0 : parseInt($(tr).find("td").eq(13).html());
    //        const venta_actual = $(tr).find("td").eq(14).html() == "" ? 0 : parseInt($(tr).find("td").eq(14).html());
    //        const basico = $(tr).find("td").eq(8).html() == "" ? 0 : parseInt($(tr).find("td").eq(8).html());

    //        if (venta_actual > basico) {
    //            $(tr).find("td").eq(8).css({ "background": "red", "color": "white" });
    //        }

    //        if (faltante > 0) {
    //            $(tr).find("td").eq(13).css({ "background": "orange", "color": "white" });
    //        }

    //    });


    //    //$(table).find(".txt_values").on("change keyup", function (e) {
    //    //    sumarEspacios();
    //    //});
    //});

    /*  $("")*/

    $(".tbl_detail").each(function (i, item) {
        $(item).find('th:nth-child(1), td:nth-child(1)').hide();
        $(item).find('th:nth-child(2), td:nth-child(2)').hide();
        $(item).find('th:nth-child(3), td:nth-child(3)').hide();
        $(item).find('th:nth-child(5), td:nth-child(5)').hide();
    });

</script>
<link href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.css" rel="stylesheet" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.js"></script>

<script>
    $("#drpSucursales").dropdown(
        {
            "fullTextSearch": true
        });

    function sumarEspacios() {

        var total = 0;

        $("table:eq(0) tbody tr").not($("table:eq(0)").find("tr").last()).each(function (i, tr) {
            var espacios = parseInt($(tr).find("td").eq(4).html());
            var cantidad = parseInt($(tr).find(".txt_values").val());
            console.log(tr, espacios, cantidad);
            total += espacios * cantidad;
        });

        $("table:eq(0)").find("tr").last().find("td").eq(4).html(total);
    }
</script>
<%--<script type="text/javascript">
    var submit = 0;
    function CheckDouble() {
        if (++submit > 1) {
            alert('Esto a veces tarda unos segundos, tenga paciencia.!');
            return false;
        }
    }
</script>--%>
<%--<script type="text/javascript">
    var isButtonDisabled = false;

    $('#<%= btnGuardarPlanificacion.ClientID %>').click(function () {
                        $('#<%= btnGuardarPlanificacion.ClientID %>').addClass("disabled");
                          iziToast.success({ title: 'OK!', message: 'Por Favor Espere!!', position: 'topRight', timeout: 5000 });
                          setTimeout(enableButton, 5000);
                      });
</script>--%>