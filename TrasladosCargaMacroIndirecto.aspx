<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosCargaMacroIndirecto.aspx.vb" Inherits="TrasladosCargaMacroIndirecto" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Sugerido Logistica</title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback">
    <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css">
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
    <link href="css/TasksCards.css" rel="stylesheet" />

    <%--izitoast--%>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>

    <style>
        .container {
            display: flex;
            margin-left: 0;
        }

        .top-column {
            flex: 3;
            margin-left: 20px;
            /*background-color: coral;*/
            border-radius: 10px;
        }

        .catalog-column {
            flex: 8;
            margin-left: 20px;
            /*background-color: darkgray;*/
            border-radius: 10px;
        }

        .buttons-column {
            flex: 1;
            border-radius: 10px;
        }

        .catalog-table {
            width: 100%;
        }

        /*.button {
            display: block;
            width: 200px;
            height: 50px;
            margin-bottom: 10px;
            text-align: center;
            background-color: #17a2b8;
            font-size: 18px;
            font-weight: bold;
            line-height: 50px;
            text-transform: uppercase;
            border-radius: 10px;*/ /* Ajusta el valor para obtener el nivel de redondez deseado */
        /*}*/

        /*.buttons-column {
            margin-top: 60px;
            margin-right: 20px;
        }*/

        .sticky-header th {
            position: sticky !important;
            top: 0 !important;
            z-index: 2 !important;
            background-color: #343a40 !important; /* Color de fondo del encabezado */
            color: white !important; /* Color del texto */
            border-bottom: 2px solid #dee2e6 !important;
            padding: 8px !important;
            text-align: left !important;
        }

        .sticky-grid {
            border-collapse: collapse;
            width: 100%;
        }

        .sticky-grid th,
        .sticky-grid td {
        border: 1px solid #dee2e6;
        }
        /* Ensure that the demo table scrolls */
        th, td { white-space: nowrap; }
        div.dataTables_wrapper {
            margin: 0 auto;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
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
        <div class="d-flex justify-content-start mt-3">
            <div class="col-3">
                <div class="row">
                    <div class="col">Estado</div>
                    <div class="col">
                        <asp:TextBox ID="txtEstatusCliente" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col">Limite Credito</div>
                    <div class="col">
                        <asp:TextBox ID="txtLimiteCredito" runat="server" class="form-control text-right" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col">Saldo de Cuenta</div>
                    <div class="col">
                        <asp:TextBox ID="txtSaldoCuenta" runat="server" class="form-control text-right" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col">Saldo Consignacion</div>
                    <div class="col">
                        <asp:TextBox ID="txtSaldoConsignacion" runat="server" class="form-control text-right" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col">
                        <asp:Label ID="lblNombreAlmacenHeader" runat="server" Text=""></asp:Label>
                    </div>
                </div>

                <div class="row my-2">
                    <div class="col p-1">
                        <asp:DropDownList ID="drpOptions" runat="server" CssClass="form-control btn btn-block btn-info text-left">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row my-2">
                    <div class="col">
                        <asp:Button ID="btnCargaGeneral" runat="server" Text="Completa" CssClass="btn btn-block btn-info" />
                    </div>
                    <div class="col">
                        <asp:Button ID="btnCadena" runat="server" Text="Cadena" CssClass="btn btn-block btn-info" />
                    </div>
                    <div class="col">
                        <asp:Button ID="btnConsigna" runat="server" Text="Dist" CssClass="btn btn-block btn-info" />
                    </div>
                </div>
                <div class="row my-2">
                    <div class="col">
                        <asp:DropDownList ID="drpRutaLogica" runat="server" CssClass="form-control btn btn-block btn-info text-left">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row my-2">
                    <div class="col">
                        <asp:Button ID="btnCargaGeneralRuta" runat="server" Text="Completa" CssClass="btn btn-block btn-secondary" />
                    </div>
                    <div class="col">
                        <asp:Button ID="btnCadenaRuta" runat="server" Text="Cadena" CssClass="btn btn-block btn-secondary" />
                    </div>
                    <div class="col">
                        <asp:Button ID="btnDistRuta" runat="server" Text="Dist" CssClass="btn btn-block btn-secondary" />
                    </div>
                </div>
                <div class="row my-2">
                    <div class="col">
                    <label for="txtPlanidManual" runat="server">Agregar a Planificacion</label>
                        <asp:TextBox ID="txtPlanidManual" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col">
                    <label for="btnPlanidManual" runat="server">Agregar</label>
                        <asp:Button id="btnPlanidManual" runat="server" CssClass="btn btn-block btn-warning" Text="Agregar PlanId"/>
                    </div>
                </div>
                <div class="row my-2">
                    <div class="col">
                        <label for="txtFechaArmado">Fecha Armado</label>
                        <asp:TextBox ID="txtFechaArmado" runat="server" type="date" class="form-control"></asp:TextBox>
                    </div>
                    <div class="col">
                        <label for="txtFechaDespacho">Fecha Despacho</label>
                        <asp:TextBox ID="txtFechaDespacho" runat="server" type="date" class="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="row my-2">
                    <div class="col">
                        <asp:Button ID="btnNotificar" runat="server" class="btn btn-block btn-success" Text="Notificar" Visible="false" />
                    </div>
                </div>
            </div>
            <div class="col-3">
                <table id="rotacion" class="table table-sm" width="100%">
                    <thead class="thead-dark">
                        <tr>
                            <th>#</th>
                            <th>Actual</th>
                            <th>-30</th>
                            <th>-60</th>
                            <th>-90</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>Stock Unds</td>
                            <td>
                                <asp:Label ID="lbls00" runat="server" Text="0"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbls30" runat="server" Text="0"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbls60" runat="server" Text="0"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbls90" runat="server" Text="0"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Unds. Vend</td>
                            <td>
                                <asp:Label ID="lblv00" runat="server" Text="0"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblv30" runat="server" Text="0"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblv60" runat="server" Text="0"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblv90" runat="server" Text="0"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>% Rotacion</td>
                            <td>
                                <asp:Label ID="lblr00" runat="server" Text="0"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblr30" runat="server" Text="0"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblr60" runat="server" Text="0"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblr90" runat="server" Text="0"></asp:Label>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="col-6">
                <asp:Button ID="btnEliminarPedidoTemp" runat="server" CssClass="btn btn-block btn-danger" Text="Eliminar Todo" />
                <br />
                <div class="container-fluid">
                    <asp:GridView ID="gridPedidoTemp" runat="server" AutoGenerateColumns="false" CssClass="table table-sm" Width="100%">
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
        <div class="container-fluid">
        </div>

        <button type="button" id="toggle-columna" class="btn btn-primary mb-2">Ocultar Columna</button>
        <div class="d-flex justify-content-start mt-3">
             <div class="col-4" id="columna-4">

                <asp:GridView ID="gridParetoClientes" runat="server" CssClass="table table-sm" AutoGenerateColumns="False" with="100%">
                    <HeaderStyle CssClass="sticky-header thead-dark" />
                    <Columns>
                        <asp:BoundField DataField="Row" HeaderText="" />
                        <asp:BoundField DataField="U_CardCode" HeaderText="Dist" />
                        <asp:BoundField DataField="Sup" HeaderText="Sup" />
                        <asp:BoundField DataField="Monto" HeaderText="Monto" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="ParetoPercent" HeaderText="% Pareto" DataFormatString="{0:p}" ItemStyle-HorizontalAlign="Right" />
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/arrowadd.jpg" Text="Cargar" CommandName="Cargar" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ControlStyle Height="30px" Width="30px" />
                            <ItemStyle Wrap="False" />
                        </asp:ButtonField>
                    </Columns>
                </asp:GridView>
            </div>
             <div class="col-8" id="columna-8">
                <script src="https://d3js.org/d3.v6.min.js"></script>
                <div class="container-fluid">
                    <asp:GridView ID="gridCuadroBasico" runat="server" AutoGenerateColumns="false" CssClass="table table-sm sticky-grid" Style="width: 100%">
                        <HeaderStyle CssClass="sticky-header thead-dark" />
                        <Columns>
                            <asp:BoundField DataField="ROWID" HeaderText="" />
                            <asp:BoundField DataField="CODE" HeaderText="" />
                            <asp:BoundField DataField="MODELO" HeaderText="Modelo" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="3m" HeaderText="V3m" ItemStyle-CssClass="heatmap-cell1" />
                            <asp:BoundField DataField="6m" HeaderText="V6m" ItemStyle-CssClass="heatmap-cell1" />
                            <asp:BoundField DataField="12m" HeaderText="V12m" ItemStyle-CssClass="heatmap-cell1" />
                            <asp:BoundField DataField="3mp" HeaderText="%3m" DataFormatString="{0:p}" ItemStyle-CssClass="heatmap-cell2" />
                            <asp:BoundField DataField="6mp" HeaderText="%6m" DataFormatString="{0:p}" ItemStyle-CssClass="heatmap-cell2" />
                            <asp:BoundField DataField="12mp" HeaderText="%12m" DataFormatString="{0:p}" ItemStyle-CssClass="heatmap-cell2" />
                            <asp:BoundField DataField="DCM00" HeaderText="DCM" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="CB" HeaderText="CB" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Despacho" HeaderText="Despacho" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Comprometido" HeaderText="Comp" />
                            <asp:BoundField DataField="Solicitado" HeaderText="Sol" />
                            <asp:BoundField DataField="Transito" HeaderText="Tran" />
                            <asp:BoundField DataField="Fisico" HeaderText="INV" />
                            <asp:BoundField DataField="Entrega" HeaderText="Ent" />
                            <asp:BoundField DataField="Faltante" HeaderText="Falt" />
                            <asp:BoundField DataField="Actual" HeaderText="Vta.A" />
                            <asp:BoundField DataField="A" HeaderText="-30" />
                            <asp:BoundField DataField="B" HeaderText="-60" />
                            <asp:BoundField DataField="C" HeaderText="-90" />
                            <asp:BoundField DataField="Total" HeaderText="Total" />
                            <asp:BoundField DataField="UltVenta" HeaderText="UltVtA" />
                            <asp:BoundField DataField="Logistica" HeaderText="L" />
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/bike.png" Text="S" CommandName="Sugerir" HeaderText="S">
                                <ControlStyle Height="30px" Width="30" />
                                <ItemStyle Wrap="False" />
                            </asp:ButtonField>
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

        </div>
        <div class="d-flex justify-content-start mt-3">
            <div class="row">
                <div class="col">
                    <b>Canal</b>
                    <br />
                    <asp:Label ID="lblCanal" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <b>Supervisor</b>
                    <br />
                    <asp:Label ID="lblSupervisor" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <b>Codigo</b>
                    <br />
                    <asp:Label ID="lblAlmacen" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <b>Whscode</b>
                    <br />
                    <asp:Label ID="lblWhscode" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <b>Almacen</b>
                    <br />
                    <asp:Label ID="lblNombreAlmacen" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <b>Correo</b>
                    <br />
                    <asp:Label ID="lblCorreo" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <b>RowIndex</b>
                    <br />
                    <asp:Label ID="lblRowIndex" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <b>Ruta</b>
                    <br />
                    <asp:Label ID="lblRutaWhs" runat="server"></asp:Label>
                </div>
            </div>
        </div>
        <!-- Inicio Modal Agregar Colores Moto-->
        <div class="modal fade" id="ColoresMotos" tabindex="-1" role="dialog" aria-labelledby="ColoresMotosLabel"
            aria-hidden="true">
            <div class="modal-dialog modal-xl" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="ColoresMotosLabel">Seleccionar Modelos y Colores a Enviar</h5>
                        <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">×</span>
                        </button>
                    </div>
                    <div class="modal-body">
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
                        <asp:Label ID="lblModeloCode" runat="server" Text="SUGERIDO"></asp:Label>
                        |
                        <asp:Label ID="lblModeloMoto" runat="server" Text="SUGERIDO"></asp:Label>
                        <hr />
                        <asp:GridView ID="gridColoresMoto" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" Width="100%">
                            <HeaderStyle CssClass="thead-dark" />
                            <Columns>
                                <asp:BoundField DataField="itemcode" HeaderText="Codigo" />
                                <asp:BoundField DataField="itemname" HeaderText="Descripcion" />
                                <asp:BoundField DataField="Modelo" HeaderText="Modelo" />
                                <asp:BoundField DataField="Espacios" HeaderText="Espacios" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="CEDIS" HeaderText="DCM00" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="SUCURSAL" HeaderText="Sucursal" ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderText="Cant">
                                    <ItemTemplate>
                                        <asp:TextBox ID="qty" runat="server" ClientIDMode="Static" Width="50px" Text="0" CssClass="txt_values text-right" type="Number" min="0"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView> 
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnCancelColoresMotos" data-dismiss="modal"  runat="server" class="btn btn-secondary" Text="Cancelar" />
                        <asp:Button ID="btnModalColoresMotos" data-dismiss="modal"  runat="server" class="btn btn-primary" Text="Agregar" />
                    </div>
                </div>
            </div>
        </div>
        <!-- Fin Modal Agregar Colores Moto-->

        <!-- Inicio Modal Confirmacion Docto-->
        <div class="modal fade" id="confirmacionDocto" tabindex="-1" role="dialog" aria-labelledby="confirmacionDoctoLabel" aria-hidden="true">
            <div class="modal-dialog modal-xl" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="confirmacionDoctoLabel">Documento Creado Con Exito!!!</h5>
                        <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">×</span>
                        </button>
                    </div>
                    <div class="modal-body" style="display: flex; justify-content: center; align-items: center;">
                        <h1>
                            <asp:Label ID="lblNuevoDocto" runat="server" Text="0"></asp:Label></h1>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnCerrarModal" runat="server" class="btn btn-primary" Text="Cerrar" />
                    </div>
                </div>
            </div>
        </div>
        <!-- Fin Modal Agregar Confirmacion Docto-->

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

        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.css" />
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.js" integrity="sha512-gnoBksrDbaMnlE0rhhkcx3iwzvgBGz6mOEj4/Y5ZY09n55dYddx6+WYc72A55qEesV8VX2iMomteIwobeGK1BQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.css" integrity="sha512-3quBdRGJyLy79hzhDDcBzANW+mVqPctrGCfIPosHQtMKb3rKsCxfyslzwlz2wj1dT8A7UX+sEvDjaUv+WExQrA==" crossorigin="anonymous" referrerpolicy="no-referrer" />


<%--        <!-- DataTables & Plugins -->
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
        <link href="https://cdn.datatables.net/fixedcolumns/5.0.0/css/fixedColumns.bootstrap4.min.css" rel="stylesheet">
        <script src="https://cdn.datatables.net/fixedcolumns/5.0.0/js/dataTables.fixedColumns.min.js"></script>--%>

        <!-- jQuery -->
        <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

        <!-- DataTables & Plugins -->
        <!-- DataTables CSS -->
        <link href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css" rel="stylesheet" />
        <link href="https://cdn.datatables.net/buttons/2.4.1/css/buttons.dataTables.min.css" rel="stylesheet" />
        <link href="https://cdn.datatables.net/fixedcolumns/4.3.0/css/fixedColumns.dataTables.min.css" rel="stylesheet" />
        <!-- DataTables & Plugins -->
        <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
        <script src="https://cdn.datatables.net/buttons/2.4.1/js/dataTables.buttons.min.js"></script>
        <script src="https://cdn.datatables.net/buttons/2.4.1/js/buttons.html5.min.js"></script>
        <script src="https://cdn.datatables.net/fixedcolumns/4.3.0/js/dataTables.fixedColumns.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.10.1/jszip.min.js"></script>

        <script type="text/javascript">
            $(function () {
                $("[id*=gridParetoClientes]").DataTable({
                    dom: 'Bfrtip',
                    buttons: [
                        'excelHtml5'
                    ],
                    responsive: false,
                    bLengthChange: true,
                    lengthMenu: [[10, -1], [10, "All"]],
                    bFilter: true,
                    bSort: true,
                    bPaginate: false,
                    order: [[2, "desc"]]
                });
            });
            $(function () {
                $("#gridCuadroBasico").DataTable({
                    dom: 'Bfrtip',
                    buttons: [
                        'excelHtml5'
                    ],
                    responsive: false,
                    bLengthChange: true,
                    lengthMenu: [[10, -1], [10, "All"]],
                    bFilter: true,
                    bSort: true,
                    order: [[0, "asc"]],
                    //fixedColumns: {
                    //    left: 3,   // Columnas fijas a la izquierda
                    //    right: 1   // Columnas fijas a la derecha
                    //},
                    paging: false
                });
            });
        </script>


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
<script type="text/javascript">

    function convertToUppercase(elementId) {
        var textBox = document.getElementById(elementId);
        textBox.value = textBox.value.toUpperCase();
    }

    $(document).ready(function () {
        $("#btnNotificar").prop("enabled", true);


        var x = $('#<%=txtEstatusCliente.ClientID%>').val();

        var creditline = parseFloat($('#<%=txtLimiteCredito.ClientID%>').val().replace(/,/g, ''));
        var saldogeneral = parseFloat($('#<%=txtSaldoCuenta.ClientID%>').val().replace(/,/g, ''));
        var saldoconsigna = parseFloat($('#<%=txtSaldoConsignacion.ClientID%>').val().replace(/,/g, ''));


        if (x == 'BLOQUEADO') {
            $("#btnNotificar").prop("disabled", true);
            iziToast.warning({ title: 'Alerta!', message: 'Cliente Bloqueado en SAP, Consulte con Creditos!!!', position: 'topRight', timeout: 5000 });
        }

        if ((saldogeneral + saldoconsigna) > creditline) {
            $("#btnNotificar").prop("disabled", true);
            iziToast.warning({ title: 'Alerta!', message: 'Cliente Excede su Linea de Creditos, Consulte con Creditos!!!', position: 'topRight', timeout: 5000 });
        }
    });
</script>
            <script>
                $(document).ready(function () {
                    $('#toggle-columna').click(function () {
                        var columna4 = $('#columna-4');
                        var columna8 = $('#columna-8');
                        var boton = $(this);

                        if (columna4.is(':visible')) {
                            columna4.hide();
                            columna8.removeClass('col-8').addClass('col-12');
                            boton.text('Mostrar Columna');
                        } else {
                            columna4.show();
                            columna8.removeClass('col-12').addClass('col-8');
                            boton.text('Ocultar Columna');
                        }
                    });
                });
            </script>
