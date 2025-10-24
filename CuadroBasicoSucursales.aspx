<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CuadroBasicoSucursales.aspx.vb" Inherits="CuadroBasicoSucursales" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Confirmacion Logistica</title>
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
        <asp:Label ID="labelError" runat="server"></asp:Label>
        <br />

        <div class="container-fluid">
            <script src="https://d3js.org/d3.v6.min.js"></script>
            <div class="container-fluid">
                <div class="col-2">
                    <asp:TextBox ID="txtFilter" runat="server" Text="" placeholder="Filtrar Por Modelo"
                        CssClass="form-control" onkeyup="convertToUppercase('txtFilter')"></asp:TextBox>
                </div>
            </div>
            <script>
                $('#txtFilter').keyup(function () {
                    var _val = $(this).val().toLowerCase();
                    $('#gridCuadroBasico tbody tr').each(function () {
                        var _cell = $(this).find('td:eq(2)').text();
                        if (_val === '0' || _cell.toLowerCase().indexOf(_val) !== -1) {
                            $(this).show();
                        } else {
                            $(this).hide();
                        }
                    });
                });
            </script>
            <hr />
            <div class="ui long scrolling container fluid">
                <asp:GridView ID="gridCuadroBasico" runat="server" AutoGenerateColumns="false" class="ui last head foot stuck unstackable celled table" Width="100%">
                    <HeaderStyle CssClass="ui blue inverted table" />
                    <Columns>
                        <asp:BoundField DataField="ROWID" HeaderText="RowId" />
                        <asp:BoundField DataField="CODE" HeaderText="Codigo" />
                        <asp:TemplateField HeaderText="Modelo">
                            <ItemTemplate>
                                <div class="ui raised segment">
                                    <a class="ui red ribbon label"><%# Eval("Ranking") %></a>
                                    <div class="flex" style="display: flex; justify-content: space-around; align-items: center;">
                                        <div>
                                            <asp:Label ID="lblModelo" runat="server" Text='<%# Eval("MODELO") %>' Style="margin-top: 3px; margin-bottom: 3px"></asp:Label>
                                        </div>
                                        <div>
                                            <asp:Button ID="btnBI" type="button" runat="server" Text="R | E" CssClass="mini ui button teal" CommandName="BI" CommandArgument='<%# Eval("ROWID") %>' />
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="3m" HeaderText="3m" ItemStyle-CssClass="heatmap-cell1" />
                        <asp:BoundField DataField="6m" HeaderText="6m" ItemStyle-CssClass="heatmap-cell1" />
                        <asp:BoundField DataField="12m" HeaderText="12m" ItemStyle-CssClass="heatmap-cell1" />
                        <asp:BoundField DataField="3mp" HeaderText="%3m" DataFormatString="{0:p}" ItemStyle-CssClass="heatmap-cell2" />
                        <asp:BoundField DataField="6mp" HeaderText="%6m" DataFormatString="{0:p}" ItemStyle-CssClass="heatmap-cell2" />
                        <asp:BoundField DataField="12mp" HeaderText="%12m" DataFormatString="{0:p}" ItemStyle-CssClass="heatmap-cell2" />
                        <asp:BoundField DataField="DCM00" HeaderText="Stock" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="CB" HeaderText="CB" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="Comprometido" HeaderText="Comp" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="Solicitado" HeaderText="Soli" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="Transito" HeaderText="Tran" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="Fisico" HeaderText="Fisico" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="Faltante" HeaderText="Falt" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="Actual" HeaderText="Vta.A" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="A" HeaderText="-30" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="B" HeaderText="-60" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="C" HeaderText="-90" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="Total" HeaderText="Total" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="UltVenta" HeaderText="UltVenta" />
                    </Columns>
                </asp:GridView>
            </div>
            <style>
                .heatmap-cell1,
                .heatmap-cell2 {
                    /* font-weight: bold;*/
                    /*font-size: larger;*/
                    color: white;
                    text-shadow: 1px 1px black;
                    text-align: center;
                }

                .heatmap-cell3 {
                    /*font-size: larger;*/
                    text-align: center;
                }
            </style>
            <script>
                var cells1 = document.querySelectorAll('.heatmap-cell1');
                var colorScale1 = d3.scaleSequential()
                    .domain(d3.extent(cells1, function (cell) { return parseFloat(cell.innerText); }))
                    .interpolator(d3.interpolateBlues);
                cells1.forEach(function (cell) {
                    var value = parseFloat(cell.innerText);
                    cell.style.backgroundColor = colorScale1(value);
                });
                var cells2 = document.querySelectorAll('.heatmap-cell2');

                var colorScale2 = d3.scaleSequential()
                    .domain(d3.extent(cells2, function (cell) { return parseFloat(cell.innerText); }))
                    .interpolator(d3.interpolateGreens)
                    .interpolator(function (t) { return d3.interpolateGreens(1 - t); });
                cells2.forEach(function (cell) {
                    var value = parseFloat(cell.innerText);
                    cell.style.backgroundColor = colorScale2(value);
                });
            </script>

            <%-- <div class="ui long scrolling container fluid">
                <asp:GridView ID="gridCuadroBasico" runat="server" AutoGenerateColumns="false" class="ui last head foot stuck unstackable celled table" Style="width: 100%">
                    <HeaderStyle CssClass="thead-dark" />
                    <Columns>
                        <asp:BoundField DataField="ROWID" HeaderText="RowId" />
                        <asp:BoundField DataField="CODE" HeaderText="Codigo" />
                        <asp:BoundField DataField="MODELO" HeaderText="Modelo" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="3m" HeaderText="Vta 3m" ItemStyle-CssClass="heatmap-cell1" />
                        <asp:BoundField DataField="6m" HeaderText="Vta 6m" ItemStyle-CssClass="heatmap-cell1" />
                        <asp:BoundField DataField="12m" HeaderText="Vta 12m" ItemStyle-CssClass="heatmap-cell1" />
                        <asp:BoundField DataField="3mp" HeaderText="% 3m" ItemStyle-CssClass="heatmap-cell2" />
                        <asp:BoundField DataField="6mp" HeaderText="% 6m" ItemStyle-CssClass="heatmap-cell2" />
                        <asp:BoundField DataField="12mp" HeaderText="% 12m" ItemStyle-CssClass="heatmap-cell2" />
                        <asp:BoundField DataField="DCM00" HeaderText="Stock" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="Fisico" HeaderText="Fisico" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="CB" HeaderText="CB" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="Actual" HeaderText="Vta.A" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="A" HeaderText="-30" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="B" HeaderText="-60" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="C" HeaderText="-90" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="Total" HeaderText="Total" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="UltVenta" HeaderText="UltVenta" />
                    </Columns>
                </asp:GridView>
            </div>
            <style>
                .heatmap-cell1,
                .heatmap-cell2 {
                    /* font-weight: bold;*/
                    font-size: larger;
                    color: white;
                    text-shadow: 1px 1px black;
                    text-align: center;
                }

                .heatmap-cell3 {
                    font-size: larger;
                    text-align: center;
                }
            </style>
            <script>
                // Obtener las celdas del GridView correspondientes al primer mapa de calor
                var cells1 = document.querySelectorAll('.heatmap-cell1');

                // Crear una escala de color para el primer mapa de calor
                var colorScale1 = d3.scaleSequential()
                    .domain(d3.extent(cells1, function (cell) { return parseFloat(cell.innerText); }))  // Rango de valores que se mapearán a la escala de color
                    .interpolator(d3.interpolateBlues);  // Combinación de colores (de azul claro a azul oscuro)

                // Recorrer las celdas del primer mapa de calor y aplicar el color de acuerdo a los valores
                cells1.forEach(function (cell) {
                    var value = parseFloat(cell.innerText);
                    cell.style.backgroundColor = colorScale1(value);
                });

                // Obtener las celdas del GridView correspondientes al segundo mapa de calor
                var cells2 = document.querySelectorAll('.heatmap-cell2');

                // Crear una escala de color invertida para el segundo mapa de calor
                var colorScale2 = d3.scaleSequential()
                    .domain(d3.extent(cells2, function (cell) { return parseFloat(cell.innerText); }))  // Rango de valores que se mapearán a la escala de color
                    .interpolator(d3.interpolateGreens)  // Combinación de colores (de verde claro a verde oscuro)
                    .interpolator(function (t) { return d3.interpolateGreens(1 - t); });  // Invertir la escala de color

                // Recorrer las celdas del segundo mapa de calor y aplicar el color de acuerdo a los valores
                cells2.forEach(function (cell) {
                    var value = parseFloat(cell.innerText);
                    cell.style.backgroundColor = colorScale2(value);
                });
            </script>

            <script>

                $("input.qty").keyup(function (e) {
                    let _value = parseInt($(e.target).val());
                    _value = _value <= 0 || _value == 0 ? 0 : _value;
                    $(e.target).val(_value);

                });

                $("button.btnPlus").click(function (e) {

                    let _tr = $(e.target).parents("td").eq(0);
                    let _value = parseInt($(_tr).find(".qty").val());
                    _value++;
                    $(_tr).find(".qty").val(_value);

                });

                $("button.btnMinus").click(function (e) {

                    let _tr = $(e.target).parents("td").eq(0);
                    let _value = parseInt($(_tr).find(".qty").val());
                    _value = _value <= 0 ? 0 : _value - 1;
                    $(_tr).find(".qty").val(_value);

                });
            </script>
        </div>--%>


            <%--INICIO MODAL NUEVO ALMACEN--%>
            <div class="modal fade" id="modalAdd">
                <div class="modal-dialog modal-sm">
                    <div class="modal-content">
                        <div style="background-color: #272b35;" class="modal-header">
                            <h4 style="color: white;" class=" modal-title">Nuevo Almacen</h4>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span style="color: white;" aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col">
                                    <!-- text input -->
                                    <div class="form-group">
                                        <label>Codigo Almacen</label>
                                        <asp:TextBox ID="txtCodidoAlm" runat="server" class="form-control" onkeyup="convertToUppercase('txtCodidoAlm')" Text=""></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer justify-content-between">
                                <button type="button" class="btn btn-danger" data-dismiss="modal">Cerrar</button>
                                <asp:Button ID="BtnCrearNuevaSucursal" runat="server" Text="Crear" class="btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--FIN MODAL NUEVO ALMACEN--%>

            <%--INICIO MODAL NUEVO MODELO--%>
            <div class="modal fade" id="modalAdd_modelo">
                <div class="modal-dialog modal-sm">
                    <div class="modal-content">
                        <div style="background-color: #272b35;" class="modal-header">
                            <h4 style="color: white;" class=" modal-title">Nuevo Modelo</h4>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span style="color: white;" aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col">
                                    <!-- text input -->
                                    <div class="form-group">
                                        <label>Codigo Modelo</label>
                                        <asp:TextBox ID="txtModelo" runat="server" class="form-control" onkeyup="convertToUppercase('txtModelo')" Text=""></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer justify-content-between">
                                <button type="button" id="btnModeloCancel" class="btn btn-danger" data-dismiss="modal">Cerrar</button>
                                <asp:Button ID="btnModeloCrear" runat="server" Text="Crear" class="btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--FIN MODAL NUEVO MODELO--%>

            <footer class="main-footer text-center">
                <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
            </footer>
            <asp:ScriptManager ID="sm1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
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

    //new DataTable('#gridCuadroBasico', {
    //    fixedHeader: {
    //        header: true,
    //        footer: true
    //    },
    //    "sort": false,
    //    paging: false,
    //    scrollCollapse: true,
    //    scrollX: true,
    //    scrollY: 700
    //});
</script>
<link href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.css" rel="stylesheet" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.js"></script>

<script>
    $("#drpSucursales").dropdown(
        {
            "fullTextSearch": true
        });
</script>

<script>
    function recorrerTabla() {

        var table = document.getElementById("gridCuadroBasico");
        for (var i = 1, row; row = table.rows[i]; i++) {
            var almacen = $('#drpSucursales').val();
            var modelo = row.cells[2].innerHTML;
            var cuadro_basico = row.cells[111].innerHTML;
            var cambio = row.cells[122].querySelector('input.qty').value.toString();
            if (cuadro_basico === cambio) {
                //console.log("Los valores de las celdas son iguales en la línea " + (i+1) + " " + almacen + " " + modelo);
            } else {
                //console.log(almacen, modelo, cambio);
                actualizarFila(almacen, modelo, cambio);
            }
        }

        setTimeout(() => location.reload(), 5000)
    }

    function actualizarFila(almacen, modelo, maximo) {
        var xhr = new XMLHttpRequest();
        xhr.open("POST", "http://web.grupomovesa.com/inventario/actualizar.php", true);
        xhr.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
        xhr.send("almacen=" + almacen + "&modelo=" + modelo + "&maximo=" + maximo + "&actualizar=1");
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                toastr.success(xhr.responseText);
            }
        }
    }

    function convertToUppercase(elementId) {
        var textBox = document.getElementById(elementId);
        textBox.value = textBox.value.toUpperCase();
    }
</script>
