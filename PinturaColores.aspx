<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PinturaColores.aspx.vb" Inherits="PinturaColores" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Parametrizaciones || Colores Pintura</title>

    <link href="https://fonts.googleapis.com/css?family=Lato:300,400,700&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="css/style.css" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="pos-f-t">
            <div class="collapse" id="navbarToggleExternalContent">
                <div class="bg-dark p-4">
                    <ul class="nav nav-tabs">
                        <li class="nav-item">
                            <a class="nav-link" href="MainDashBoard.aspx">
                                <img src="Imagenes/mnegra.png" width="30" height="25" alt="Portal Armado de Motos" /></a>
                        </li>

                        <asp:Panel ID="pnlParametrizaciones" runat="server" Visible="True">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Parametrizaciones</a>
                            <ul class="dropdown-menu">
                                <asp:Panel ID="pnlContratistas" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="Contratistas.aspx">Contratistas</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlTrabajoAD" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="TrabajosAd.aspx">Trabajos Adicionales</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlPreciosArmado" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="PrecioModelos.aspx">Precios Armado</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlPinturaColores" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="PinturaColores.aspx">Pintura Colores</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlUsuarios" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="Usuarios.aspx">Usuarios</a></li>
                                </asp:Panel>
                            </ul>
                        </asp:Panel>

                        <asp:Panel ID="pnlAsignaciones" runat="server" Visible="True">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Motos en Caja</a>
                            <ul class="dropdown-menu">
                                <asp:Panel ID="pnlMotoEncaja" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="MotoenCaja.aspx">Asignar Moto</a></li>
                                </asp:Panel>
                                <asp:Panel ID="PnlStockGlobal" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="StockPorModelos.aspx">Stock Por Modelo</a></li>
                                </asp:Panel>
                            </ul>
                        </asp:Panel>

                        <asp:Panel ID="pnlArmado" runat="server" Visible="True">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Armado</a>
                            <ul class="dropdown-menu">
                                <asp:Panel ID="pnlListadoMotosProceso" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="ListadoProceso.aspx">Motos En Proceso</a></li>
                                </asp:Panel>
                            </ul>
                        </asp:Panel>

                        <asp:Panel ID="PnlControlCalidad" runat="server" Visible="True">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Control de Calidad</a>
                            <ul class="dropdown-menu">
                                <asp:Panel ID="pnlListadoMotosCalidad" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="ListadoCCalidad.aspx">Motos En Control de Calidad</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnoInformeProcesadasCC" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="InformeCC.aspx">Informe de Motos Procesadas CC</a></li>
                                </asp:Panel>
                            </ul>
                        </asp:Panel>

                        <asp:Panel ID="pnlProcesoContable" runat="server" Visible="True">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Proceso Contable</a>
                            <ul class="dropdown-menu">
                                <asp:Panel ID="pnoCrearLiquidacion" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="ProcesoContable.aspx">Crear Liquidacion</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlCrearPO" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="GenerarPO.aspx">Crear Orden de Compra</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlConsultaLiquidaciones" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="ProcesoContableConsulta.aspx">Consulta Liquidaciones Anteiores</a></li>
                                </asp:Panel>
                            </ul>
                        </asp:Panel>

                        <asp:Panel ID="pnlDisponibles" runat="server" Visible="True">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Disponibles</a>
                            <ul class="dropdown-menu">
                                <asp:Panel ID="pnlDisponibleArmadoSAP" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="InformeMotosDisponibles.aspx">Motos Armadas Procesadas Portal</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlExpedienteVeh" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="ExpedienteVehiculo.aspx">Expediente Vehiculo</a></li>
                                </asp:Panel>
                            </ul>
                        </asp:Panel>

                        <asp:Panel ID="pnlNoDisponible" runat="server" Visible="True">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">No Disponibles</a>
                            <ul class="dropdown-menu">
                                <asp:Panel ID="pnlInformeNoDisponible" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="InformeMotosNoDisponibles.aspx">Motos No Disponibles</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlExpedienteGarantia" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="ExpedienteVehiculoGarantia.aspx">Expediente Vehiculo Garantia / ND</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlRepuestoRetirado" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="RepuestosRetirados.aspx">Repuestos Retirados</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlOCRProveedor" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="OCRProveedor.aspx">Orden de Compra Proveedor</a></li>
                                </asp:Panel>
                                <asp:Panel ID="pnlPedidoRepuestosFBack" runat="server" Visible="True">
                                    <li><a class="dropdown-item" href="PedidoRepuestosFeedback.aspx">Subir Informacion Compra Repuesto</a></li>
                                </asp:Panel>
                            </ul>
                        </asp:Panel>

                        <li class="nav-item">
                            <a class="nav-link " href="Default.aspx">Salir</a>
                        </li>
                    </ul>
                    <h4 class="text-white">Bienvenido</h4>
                    <span class="text-muted">
                        <asp:Label ID="lblNombreUsuario" runat="server" Text="Usuario Conectado"></asp:Label></span>
                </div>
            </div>
            <nav class="navbar navbar-dark bg-dark">

                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarToggleExternalContent" aria-controls="navbarToggleExternalContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
            </nav>
        </div>
        <div class="jumbotron">
            <h1 class="display-4">Pintura Colores</h1>
            <hr class="my-4" />
            <p class="lead">
                <button type="button" class="btn btn-info" data-toggle="modal" data-target="#modalNuevoColor" data-whatever="@mdo">Agregar Color</button>
                <button type="button" class="btn btn-info" data-toggle="modal" data-target="#modalNuevoGalon" data-whatever="@mdo">Agregar Galon</button>
            </p>
        </div>
        <asp:Panel ID="pnlGridUsuarios" runat="server" Visible="true">
            <div class="d-flex justify-content-around">
                <div class="row">
                    <div class="col">
                        <h1>Colores</h1>
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" Style="width: 100%">
                            <HeaderStyle CssClass="thead-dark" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="~/Imagenes/edit.png" Text="Modificar" CommandName="Modificar">
                                    <ControlStyle Height="18px" Width="18px" />
                                    <ItemStyle Wrap="False" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="ID" HeaderText="Codigo" />
                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion Color" />
                                <asp:BoundField DataField="DATECREATED" HeaderText="Fecha" />
                                <asp:BoundField DataField="USERCREATED" HeaderText="Usuario" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="col">
                        <h1>Detalle de Galones</h1>
                        <asp:GridView ID="gridGalones" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" Style="width: 100%">
                            <HeaderStyle CssClass="thead-dark" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="~/Imagenes/edit.png" Text="Modificar" CommandName="Modificar">
                                    <ControlStyle Height="18px" Width="18px" />
                                    <ItemStyle Wrap="False" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="ID" HeaderText="Codigo" />
                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion Color" />
                                <asp:BoundField DataField="FECHACREACIO" HeaderText="Fecha Apertura Galon" />
                                <asp:BoundField DataField="FECHACONSUMO" HeaderText="Fecha Consumo Galon" />
                                <asp:BoundField DataField="DATECREATED" HeaderText="Fecha" />
                                <asp:BoundField DataField="USERCREATED" HeaderText="Usuario" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </asp:Panel>



        <%--MODAL NUEVO COLOR--%>
        <div class="modal fade" id="modalNuevoColor" tabindex="-1" role="dialog" aria-labelledby="modalNuevoColorLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalNuevoColorLabel">Nuevo Color</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label for="txtNuevoColor" class="col-form-label">Color:</label>
                            <asp:TextBox ID="txtNuevoColor" runat="server" CssClass="form-control" ToolTip="Escriba el Nombre del Color"></asp:TextBox>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        <asp:Button ID="btnCrearColor" runat="server" Text="Crear Color" class="btn btn-success" />
                    </div>
                </div>
            </div>
        </div>

        <%--MODAL NUEVO GALON DE PINTURA--%>
        <div class="modal fade" id="modalNuevoGalon" tabindex="-1" role="dialog" aria-labelledby="modalNuevoGalonLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalNuevoGalonLabel">Nuevo Galon</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label for="txtNuevoColor" class="col-form-label">Color:</label>
                            <asp:TextBox ID="txtFechaCreacion" runat="server" CssClass="form-control" type="date" ToolTip="Seleccione la Fecha de Compra"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="drpGalonPintura" class="col-form-label">Color:</label>
                            <br />
                            <asp:DropDownList ID="drpColorPintura" runat="server" class="btn btn-light dropdown-toggle dropdown-toggle-split text-left" Style="width: 100%;"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        <asp:Button ID="btnCrearGalon" runat="server" Text="Crear Galon" class="btn btn-success" />
                    </div>
                </div>
            </div>
        </div>


        <script src="js/jquery.min.js"></script>
        <script src="js/popper.js"></script>
        <script src="js/bootstrap.min.js"></script>
        <script src="js/main.js"></script>
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    </form>
</body>
</html>
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

<link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css" />
<link rel="stylesheet" href="https://cdn.datatables.net/buttons/2.4.2/css/buttons.dataTables.min.css" />
<script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
<script src="https://cdn.datatables.net/buttons/2.4.2/js/dataTables.buttons.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.10.1/jszip.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
<script src="https://cdn.datatables.net/buttons/2.4.2/js/buttons.html5.min.js"></script>
<script src="https://cdn.datatables.net/buttons/2.4.2/js/buttons.print.min.js"></script>



<script type="text/javascript">
    $(function () {
        $("[id*=GridView1]").DataTable(
            {
                bLengthChange: true,
                lengthMenu: [[20, -1], [20, "All"]],
                order: [[1, "asc"]],
                bFilter: true,
                bSort: true,
                bPaginate: true,
                dom: 'Bfrtip',
                buttons: ['copy', 'csv', 'excel', 'pdf', 'print'],
            });
    });
    $(function () {
        $("[id*=gridGalones]").DataTable(
            {
                bLengthChange: true,
                lengthMenu: [[20, -1], [20, "All"]],
                order: [[1, "asc"]],
                bFilter: true,
                bSort: true,
                bPaginate: true,
                dom: 'Bfrtip',
                buttons: ['copy', 'csv', 'excel', 'pdf', 'print'],
            });
    });
</script>
