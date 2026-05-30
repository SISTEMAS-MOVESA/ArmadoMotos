<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosCargaMacro.aspx.vb" Inherits="TrasladosCargaMacro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Cuadro Basico Sucursales</title>
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

        p.ex2 {
            font-size: 50px;
        }
    </style>

</head>
<body>

    <form id="form1" runat="server" style="overflow-y: auto !important;">
        <!-- Navigation -->
        <!-- Navigation -->
        <div class="ui inverted menu">
            <div class="ui container">
                <a class="header item" href="MainDashBoard.aspx">
                    <img src="Imagenes/mnegra.png" width="50" height="40" alt="" />
                </a>
                <div class="right menu">
                    <a class="item" href="TrasladosDashboard.aspx">Inicio</a>
                    <div class="ui simple dropdown item">
                        Pre Solicitudes
         <i class="dropdown icon"></i>
                        <div class="menu">
                            <a class="item" href="TrasladosCargaMacro.aspx">Carga Macro</a>
                            <a class="item" href="TrasladosConfirmarPreSucursal.aspx">Confirmacion Sucursal</a>
                            <a class="item" href="TrasladosPreSolicitud.aspx">Pre Solicitud</a>
                            <a class="item" href="TrasladosPreSolicitudSucursal.aspx">Pre Solicitud Sucursal</a>
                            <div class="divider"></div>
                            <a class="item" href="TrasladosPresolicitudesAbiertas.aspx">Flujo Pre-Solicitudes</a>
                            <a class="item" href="TrasladosSolicitudesAbiertas.aspx">Flujo Solicitudes</a>
                            <a class="item" href="TrasladosFlujoProduccion.aspx">Flujo Produccion</a>
                            <div class="divider"></div>
                            <a class="item" href="TrasladosCuadroBasico.aspx">Cuadro Basico</a>
                            <a class="item" href="TrasladosCuadroBasicoLogico.aspx">Cuadro Basico Logico</a>
                            <div class="divider"></div>
                            <a class="item" href="TrabajosAdicionalesDashboard.aspx">Trabajos Adicionales</a>
                            <div class="divider"></div>
                            <a class="item" href="Grafico.aspx" target="_blank">KPI Inventarios</a>
                            <div class="divider"></div>
                            <a class="item" href="TrasladosDashboardPlanner.aspx">Planificador</a>
                        </div>
                    </div>
                    <a class="item" href="TrasladosSolicitud.aspx">Solicitudes</a>
                    <a class="item" href="TrasladosForklift.aspx">Armado de Moto</a>
                    <a class="item" href="TrasladosPreparar.aspx">Carga Camion</a>
                    <a class="item" href="TrasladosCrearDocumentos.aspx">Generar Traslados</a>
                    <a class="item" href="TrasladosCrearDocumentos.aspx">Gestion de Transferencias</a>
                    <a class="item" href="Default.aspx">Cerrar Sesion</a>
                </div>
            </div>
        </div>
        <div class="container-fluid my-3">
            <div class="row">
                <div class="col-3">
                    <label for="drpSucursales">Seleccione Sucursal</label>
                    <asp:DropDownList ID="drpSucursales" runat="server" class="ui search dropdown fluid" name="drpSucursales"></asp:DropDownList>
                </div>
                <div class="col-1">
                    <label for="btnCargarSugerido">Cargar</label>
                    <asp:Button ID="btnCargarSugerido" runat="server" class="btn btn-warning" Text="Cargar" Width="100%" />
                </div>
                <div class="col-1">
                    <label for="txtEsoacioFisico">Espacio Fisico</label>
                    <asp:TextBox ID="txtEsoacioFisico" runat="server" CssClass="form-control text-right" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-1">
                    <label for="txtEspaciosAsignados">Esp Asignados</label>
                    <asp:TextBox ID="txtEspaciosAsignados" runat="server" CssClass="form-control text-right" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-3">
                    <label for="txtRuta">Ruta</label>
                    <asp:TextBox ID="txtRuta" runat="server" CssClass="form-control text-left" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-2">
                    <label for="txtCodigoCliente">Codigo Cliente</label>
                    <asp:TextBox ID="txtCodigoCliente" runat="server" CssClass="form-control text-left" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-2">
                    <label for="txtFechaArmado">Fecha Inicio Armado</label>
                    <asp:TextBox ID="txtFechaArmado" runat="server" type="Date" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-2">
                    <label for="txtFechaDespacho">Fecha Despacho</label>
                    <asp:TextBox ID="txtFechaDespacho" runat="server" type="Date" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-1">
                    <label for="btnNotificar">Notificar</label>
                    <asp:Button ID="btnNotificar" runat="server" class="ui green button" Text="Notificar" Width="100%" />
                    <script type="text/javascript">
                        $(document).ready(function () {
                            $('#<%= btnNotificar.ClientID %>').click(function () {
                                $('#<%= btnNotificar.ClientID %>').addClass("disabled");
                                iziToast.success({ title: 'OK!', message: 'Por Favor Espere!!', position: 'topRight', timeout: 5000 });
                                setTimeout(enableButton, 5000);
                            });
                        });

                        $('#<%= btnNotificar.ClientID %>').click(function () {
                            $('#<%= btnNotificar.ClientID %>').addClass("disabled");
                            iziToast.success({ title: 'OK!', message: 'Por Favor Espere!!', position: 'topRight', timeout: 5000 });
                            setTimeout(enableButton, 5000);
                        });
                    </script>
                </div>
                <div class="col-7">
                    <div class="row" style="display: flex; flex-direction: column">
                        <div class="col">
                            <label for="lblPlanId">Plan ID: </label>
                            <asp:Label ID="lblPlanId" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="col">
                            <label for="lblAlmacen">Almacen: </label>
                            <asp:Label ID="lblAlmacen" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="col">
                            <label for="lblRutaPlan">Ruta: </label>
                            <asp:Label ID="lblRutaPlan" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
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
                    <asp:BoundField DataField="Despacho" HeaderText="Despacho" ItemStyle-CssClass="heatmap-cell3" />
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
                    <asp:BoundField DataField="Logistica" HeaderText="Logistica" />
                    <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/bike.png" Text="Sugerir" CommandName="Sugerir" HeaderText="Sugerir">
                        <ControlStyle Height="30px" Width="30" />
                        <ItemStyle Wrap="False" />
                    </asp:ButtonField>

                </Columns>
            </asp:GridView>
        </div>
        <style>
            .heatmap-cell1, .heatmap-cell2 {
                font-weight: bold;
                font-size: large;
                color: white;
                text-shadow: 1px 1px black;
                text-align: center;
                align-content: center;
            }
        </style>
        <script>
            var cells1 = document.querySelectorAll('.heatmap-cell1');
            var colorScale1 = d3.scaleSequential()
                .domain(d3.extent(cells1, function (cell) { return parseFloat(cell.innerText); }))
                .interpolator(d3.interpolateOranges);
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
        <br />
        <div class="container-fluid">
            <div class="row my-2">
                <div class="col-2">
                    <asp:Button ID="btnEliminarPedidoTemp" runat="server" CssClass="btn btn-block btn-danger" Text="Eliminar Todo" />
                </div>
            </div>
            <div class="row my-4">
                <div class="col">
                    <asp:GridView ID="gridPedidoTemp" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" Width="100%">
                        <HeaderStyle CssClass="thead-dark" />
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="Id" />
                            <asp:BoundField DataField="ARTICULO" HeaderText="Articulo" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="MODELO" HeaderText="Modelo" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="ESPACIOS" HeaderText="Espacios" />
                            <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" />
                            <asp:BoundField DataField="QTYLOGISTICA" HeaderText="Logistica" />
                            <asp:BoundField DataField="OBSERVACIONES" HeaderText="Observaciones" ItemStyle-HorizontalAlign="Left" />
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/delete.png" Text="Eliminar" CommandName="Eliminar" HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ControlStyle Height="30px" Width="30px" />
                                <ItemStyle Wrap="False" />
                            </asp:ButtonField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <!-- Inicio Modal Agregar Colores Moto-->

        <div class="ui flyout fullscreen" id="ColoresMotos">
            <i class="close icon"></i>
            <div class="ui header">
                <i class="question icon"></i>
                <div class="content">
                    Seleccionar Modelos y Colores a Enviar
                </div>
            </div>
            <div class="scrolling content">
                <asp:Label ID="lblruta" runat="server" Text=""></asp:Label>
                |
                <asp:Label ID="lblcardcode" runat="server" Text=""></asp:Label>
                |
                <asp:Label ID="lblAlmOrigen" runat="server" Text="DCM00"></asp:Label>
                |
                <asp:Label ID="lblAlmDestino" runat="server" Text=""></asp:Label>
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
                <asp:Label ID="lblModeloCode" runat="server" Text="SUGERIDO"></asp:Label>
                |
                <asp:Label ID="lblModeMoto" runat="server" Text="SUGERIDO"></asp:Label>
                <hr />
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
            <div class="actions">
                <asp:Button ID="btnCancelColoresMotos" runat="server" class="ui red button" Text="Cancelar" />
                <asp:Button ID="btnModalColoresMotos" runat="server" class="ui green button" Text="Agregar" />
            </div>
        </div>
        <!-- Fin Modal Agregar Colores Moto-->


        <%--MODAL PANTALLA DE CONFIRMACION--%>
        <div class="ui basic modal">
            <div class="ui icon header">
                <i class="archive icon"></i>
                Documento Creado
            </div>
            <div class="content" style="display: flex; justify-content: center; justify-items: center;">
                <p class="ex2">
                    <asp:Label ID="lblDocumento" runat="server" Text=""></asp:Label>
                </p>

            </div>
            <div class="actions">
                <div class="ui green ok inverted button">
                    <i class="checkmark icon"></i>
                    Cerrar
                </div>
            </div>
        </div>



        <!-- Modal Existencias Globales HTML -->
        <div id="existenciasModal" class="ui modal">
            <i class="close icon"></i>
            <div class="header">Existencias Globales del Modelo</div>
            <div class="content" id="existenciasContent" style="max-height: 60vh; overflow-y: auto;">
            </div>
            <div class="actions" style="padding: 10px;">
                <button class="ui approve button">Cerrar</button>
            </div>
        </div>

        <script type="text/javascript">
            function showExistenciasModal(data) {
                document.getElementById('existenciasContent').innerHTML = data;
                $('#existenciasModal')
                    .modal({
                        centered: true,
                        dimmerSettings: {
                            opacity: 0.8
                        }
                    })
                    .modal('show');
            }
        </script>

        <!-- Modal RANKING HTML -->
        <div id="rankingModal" class="ui modal">
            <i class="close icon"></i>
            <div class="header">Ranking Global por Modelo</div>
            <div class="content" id="modalContent" style="max-height: 50vh; overflow-y: auto;">
            </div>
            <div class="actions" style="padding: 10px;">
                <button class="ui approve button">Cerrar</button>
            </div>
        </div>

        <script type="text/javascript">

            function showModal(data) {
                document.getElementById('modalContent').innerHTML = data;
                $('#rankingModal')
                    .modal({
                        centered: true,
                        dimmerSettings: {
                            opacity: 0.8
                        }
                    })
                    .modal('show');
            }
        </script>

        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.css" />
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.js" integrity="sha512-gnoBksrDbaMnlE0rhhkcx3iwzvgBGz6mOEj4/Y5ZY09n55dYddx6+WYc72A55qEesV8VX2iMomteIwobeGK1BQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.css" integrity="sha512-3quBdRGJyLy79hzhDDcBzANW+mVqPctrGCfIPosHQtMKb3rKsCxfyslzwlz2wj1dT8A7UX+sEvDjaUv+WExQrA==" crossorigin="anonymous" referrerpolicy="no-referrer" />

        <script>
            $('.ui.flyout').flyout({
                context: $("form:eq(0)")
            });
        </script>
        <!-- FIN Modal RANKING HTML -->

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


    function convertToUppercase(elementId) {
        var textBox = document.getElementById(elementId);
        textBox.value = textBox.value.toUpperCase();
    }

    $("#drpSucursales").dropdown(
        {
            "fullTextSearch": true
        });
</script>

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



<script type="text/javascript">
    $("#gridCuadroBasico").DataTable({
        "responsive": true,
        "lengthChange": false,
        "autoWidth": false,
        "buttons": ["excel"],
        "sort": true,
        "order": [[0, 'asc']],
        "deferRender": true,
        "bPaginate": false,
        //"lengthMenu": [[5, -1], [5, "All"]],
    }).buttons().container().appendTo('#gridCuadroBasico_wrapper .col-md-6:eq(0)');
</script>
