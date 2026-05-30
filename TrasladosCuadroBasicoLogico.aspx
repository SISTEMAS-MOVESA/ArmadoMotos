<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosCuadroBasicoLogico.aspx.vb" Inherits="TrasladosCuadroBasicoLogico" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Confirmacion Logistica</title>
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
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>


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

</head>
<body>

    <form id="form1" runat="server">

        <div class="jumbotron-fluid bg-dark">

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
        </div>

        
 <div class="container-fluid">
     <div class="row justify-content-center">
         <div class="col">
             <label for="drpSucursales">Sucursales</label>
             <asp:DropDownList ID="drpSucursales" runat="server" Width="100%" class="ui search dropdown fluid" name="drpSucursales"></asp:DropDownList>
         </div>
         <div class="col">
             <label for="drpSucursales">Cargar Sugerido</label>
             <asp:Button ID="btnCargarSugerido" runat="server" class="btn btn-info btn-lg btn-block" Text="Cargar" Width="100%" />
         </div>
         <div class="col">
             <label for="drpSucursales">Nueva Sucursal</label>
             <asp:Button ID="btnNuevaSucursal" runat="server" class="btn btn-warning btn-lg btn-block" Text="Nueva Sucursal" Width="100%" />
         </div>
         <div class="col">
             <label for="drpSucursales">Nuevo Movelo Moto</label>
             <asp:Button ID="btnNuevoModelo" runat="server" class="btn btn-warning btn-lg btn-block" Text="Nuevo Modelo" Width="100%" />
         </div>
     </div>
 </div>
 <div class="container-fluid">
     <div class="box">
         <div>
             <div class="container-fluid">
                 <h2>Cuadro Basico Modificar</h2>
                 <div class="ui long scrolling container fluid">
                     <asp:GridView ID="gridBasicoLogicoNuevo" runat="server" AutoGenerateColumns="false" class="ui last head foot stuck unstackable celled table" Width="100%">
                         <HeaderStyle CssClass="ui blue inverted table sticky-header" />
                         <Columns>
                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                        <asp:BoundField DataField="Espacios" HeaderText="Espacios" />
                        <%--<asp:BoundField DataField="Cantidad" HeaderText="Unidades" />--%>
                             <asp:TemplateField HeaderText="Modificar">
                                 <ItemTemplate>
                                     <div style="display: flex; align-items: center; justify-content: center;" class="itm-number-input">
                                         <button type="button" id="btnSubtract" class="btn btn-danger btn-sm mr-2 btnSubtract">-</button>
                                         <asp:TextBox ID="qty" runat="server" ClientIDMode="Static" Width="60px" Height="100%"
                                             Text='<%# Eval("Cantidad") %>' CssClass="qty txt_values text-right" type="Number" min="0"
                                             data-modelo='<%# Eval("Tipo") %>'></asp:TextBox>
                                         <button type="button" id="btnAdd" class="btn btn-success btn-sm ml-2 btnAdd">+</button>
                                     </div>
                                 </ItemTemplate>
                             </asp:TemplateField>
                         </Columns>
                     </asp:GridView>
                 </div>
             </div>
         </div>
         <div class="divider"></div>
         <div>
             <div class="container-fluid">
                 <h2>Fisico Actual</h2>
                 <div class="ui long scrolling container fluid">
                     <asp:GridView ID="gridBasicoLogicoActual" runat="server" AutoGenerateColumns="true" class="ui last head foot stuck unstackable celled table" Width="100%">
                         <HeaderStyle CssClass="ui blue inverted table sticky-header" />
                     </asp:GridView>
                 </div>
             </div>
         </div>
         <div class="divider"></div>
         <div class="container-fluid">
             <h2>Ubicaciones Fisicas Por Tipo</h2>
             <div class="ui long scrolling container fluid">
                 <asp:GridView ID="gridUbicacionesFisicasPorTipo" runat="server" AutoGenerateColumns="true" class="ui last head foot stuck unstackable celled table" Width="100%">
                     <HeaderStyle CssClass="ui blue inverted table sticky-header" />
                 </asp:GridView>
             </div>
         </div>
     </div>
 </div>
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
                <%--<asp:GridView ID="gridCuadroBasico" runat="server" AutoGenerateColumns="false" class="ui last head foot stuck unstackable celled table" Width="100%">
                    <HeaderStyle CssClass="ui blue inverted table sticky-header" />
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
                        <asp:BoundField DataField="Fisico" HeaderText="Fisico" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="CB" HeaderText="CB" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:TemplateField HeaderText="Modificar">
                            <ItemTemplate>
                                <div style="display: flex; align-items: center; justify-content: center;" class="itm-number-input">
                                    <button type="button" id="btnSubtract" class="btn btn-danger btn-sm mr-2 btnSubtract">-</button>
                                    <asp:TextBox ID="qty" runat="server" ClientIDMode="Static" Width="60px" Height="100%"
                                        Text='<%# Eval("CB") %>' CssClass="qty txt_values text-right" type="Number" min="0"
                                        data-modelo='<%# Eval("MODELO") %>'></asp:TextBox>
                                    <button type="button" id="btnAdd" class="btn btn-success btn-sm ml-2 btnAdd">+</button>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Actual" HeaderText="Vta.A" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="A" HeaderText="-30" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="B" HeaderText="-60" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="C" HeaderText="-90" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="Total" HeaderText="Total" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="UltVenta" HeaderText="UltVenta" />
                    </Columns>
                </asp:GridView>--%>
            </div>
            <style>
                /* Encabezado sticky para el GridView */
                .sticky-header th {
                    position: sticky;
                    top: 0;
                    background-color: #2185d0; /* Color de fondo del encabezado (mismo que en HeaderStyle si quieres consistencia) */
                    z-index: 2; /* Elevar el encabezado sobre las filas del cuerpo */
                    text-align: center;
                }

                /* Asegúrate de que el contenedor tenga un desplazamiento adecuado */
                .scrollable-container {
                    max-height: 500px; /* Ajusta este valor según la altura deseada */
                    overflow-y: auto;
                }

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

        </div>
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


<script>
    $(window).on('load', function () {
        $("#loftloader-wrapper").hide();
    });

    window.onbeforeunload = function (e) {
        $("#loftloader-wrapper").show();
    }
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
    function convertToUppercase(elementId) {
        var textBox = document.getElementById(elementId);
        textBox.value = textBox.value.toUpperCase();
    }

    function updateDatabase(value, modelo) {
        const whsCode = $('#drpSucursales').val();

        console.log(value, whsCode, modelo);
        $.ajax({
            type: "POST",
            url: "UpdateCuadroBasico.ashx",
            data: {
                maximo: value,
                whscode: whsCode,
                modelo: modelo
            },
            success: function (response) {
                console.log("Actualización exitosa: " + response);
                iziToast.success({
                    title: 'Éxito',
                    message: 'La operación se ha realizado correctamente. Respuesta del servidor:' + response,
                    position: 'topRight',
                    timeout: 3000
                });
            },
            error: function (xhr, status, error) {
                console.error("Error al actualizar: " + error);
                iziToast.error({
                    title: 'Error',
                    message: 'Ha ocurrido un error durante la operación.',
                    position: 'topRight',
                    timeout: 5000
                });
            }
        });
    }
</script>

<script>
    $("input.qty").keyup(function (e) {
        let _value = parseInt($(e.target).val());
        _value = _value <= 0 || _value == 0 ? 0 : _value;
        $(e.target).val(_value);

    });

    $("button.btnAdd").click(function (e) {

        let _tr = $(this).parents("td").eq(0);
        let _value = parseInt($(_tr).find(".qty").val());
        _value++;
        $(_tr).find(".qty").val(_value);
        updateDatabase(_value, $(_tr).find(".qty").attr("data-modelo"));

    });
    $("button.btnSubtract").click(function (e) {
        let _parentContainer = $(this).parents("td").eq(0);
        let _qtyInput = $(_parentContainer).find(".qty");
        let _value = parseInt(_qtyInput.val());
        _value = _value > 0 ? _value - 1 : 0;
        _qtyInput.val(_value);

        updateDatabase(_value, $(_qtyInput).attr("data-modelo"));
    });
</script>
