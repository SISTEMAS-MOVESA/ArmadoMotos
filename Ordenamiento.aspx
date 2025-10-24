<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Ordenamiento.aspx.vb" Inherits="Ordenamiento" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Parametrizaciones || Ordenamiento</title>

    <link href="https://fonts.googleapis.com/css?family=Lato:300,400,700&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="css/style.css" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>

    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- jQuery (Necesario para algunas funciones de Bootstrap) -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>



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
        <h1 class="display-4">Ordenamiento de Modelos y Promociones</h1>
        <p class="lead">
            <asp:Button ID="btnAgregarModelo" runat="server" class="btn btn-success rounded" Text="Agregar Modelo" />
        </p>
        <asp:Panel ID="pnlGridUsuarios" runat="server" Visible="true">
            <div class="d-flex justify-content-center">
                <div class="row">
                    <div class="col-12">
                        <asp:GridView ID="gridOrdenamiento" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" Style="width: 100%">
                            <HeaderStyle CssClass="thead-dark" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="~/Imagenes/edit.png" Text="Modificar" CommandName="Modificar">
                                    <ControlStyle Height="18px" Width="18px" />
                                    <ItemStyle Wrap="False" />
                                </asp:ButtonField>
                                <asp:ButtonField ButtonType="Image" ImageUrl="~/Imagenes/delete.png" Text="Eliminar" CommandName="Eliminar">
                                    <ControlStyle Height="18px" Width="18px" />
                                    <ItemStyle Wrap="False" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="ID" HeaderText="Codigo" />
                                <asp:BoundField DataField="MODELO" HeaderText="Modelo" />
                                <asp:BoundField DataField="ACTIVOCD" HeaderText="Activo CD" />
                                <asp:BoundField DataField="ACTIVOCI" HeaderText="Acitvo CI" />
                                <asp:BoundField DataField="OFERTA" HeaderText="Acitvo Oferta" />
                                <asp:BoundField DataField="DESDE" HeaderText="Promo Desde" />
                                <asp:BoundField DataField="HASTA" HeaderText="Promo Hasta" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </asp:Panel>
      
         <!-- Modal -->
        <div class="modal fade" id="ordenamientoModal" tabindex="-1" aria-labelledby="ordenamientoModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="ordenamientoModalLabel">Agregar / Editar Ordenamiento</h5>
                        <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">×</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <asp:HiddenField ID="hfID" runat="server" />
                        <div class="mb-3">
                            <label class="form-label">Id</label>
                            <asp:TextBox ID="txtModalId" runat="server" CssClass="form-control" ReadOnly="true" Text="0"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label class="form-label">Modelo</label>
                            <asp:DropDownList ID="drpModalModelo" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="mb-3 form-check">
                            <asp:CheckBox ID="chkActivoCD" runat="server"/>
                            <label class="form-check-label">Activo CD</label>
                        </div>
                        <div class="mb-3 form-check">
                            <asp:CheckBox ID="chkActivoCI" runat="server"/>
                            <label class="form-check-label">Activo CI</label>
                        </div>
                        <div class="mb-3 form-check">
                            <asp:CheckBox ID="chkOferta" runat="server"/>
                            <label class="form-check-label">Oferta</label>
                        </div>
                        <div class="mb-3">
                            <label class="form-label fecha" style="display:none;">Desde</label>
                            <asp:TextBox ID="txtModalDesde" runat="server" CssClass="form-control" TextMode="Date" style="display:none;"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label class="form-label fecha" style="display:none;">Hasta</label>
                            <asp:TextBox ID="txtModalHasta" runat="server" CssClass="form-control" TextMode="Date" style="display:none;"></asp:TextBox>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnGuardarModal" runat="server" CssClass="btn btn-success" Text="Guardar"/>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal de Confirmación -->
        <div class="modal fade" id="confirmDeleteModal" tabindex="-1" aria-labelledby="confirmDeleteLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header bg-danger text-white">
                        <h5 class="modal-title" id="confirmDeleteLabel">Confirmar Eliminación</h5>
                        <button type="button" class="btn-close" data-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <p style="display:flex; align-content:center;align-items:center; justify-content:center;">¿Está seguro de que desea eliminar este registro?</p>
                        <asp:HiddenField ID="hfDeleteID" runat="server" />
                        <asp:Label ID="lblIdEliminar" runat="server" Text="" style="display:none;"></asp:Label>
                        <h2 style="display:flex; align-content:center;align-items:center; justify-content:center;"><asp:Label ID="lblModelo" runat="server" Text=""></asp:Label> </h2>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">No, cancelar</button>
                        <asp:Button ID="btnConfirmDelete" runat="server" CssClass="btn btn-danger" Text="Sí, eliminar"/>
                    </div>
                </div>
            </div>
        </div>

        <center>
            <div class="container">
                <footer class="main-footer">
                    <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
                </footer>
            </div>
        </center>
        <script src="js/jquery.min.js"></script>
        <script src="js/popper.js"></script>
        <script src="js/bootstrap.min.js"></script>
        <script src="js/main.js"></script>
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    </form>
</body>
</html>
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

<link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css"/>
<link rel="stylesheet" href="https://cdn.datatables.net/buttons/2.4.2/css/buttons.dataTables.min.css"/>
<script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
<script src="https://cdn.datatables.net/buttons/2.4.2/js/dataTables.buttons.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.10.1/jszip.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
<script src="https://cdn.datatables.net/buttons/2.4.2/js/buttons.html5.min.js"></script>
<script src="https://cdn.datatables.net/buttons/2.4.2/js/buttons.print.min.js"></script>



<script type="text/javascript">
    $(function () {
        $("[id*=gridOrdenamiento]").DataTable(
            {
                bLengthChange: true,
                lengthMenu: [[20, -1], [20, "All"]],
                order: [[2, "asc"]],
                bFilter: true,
                bSort: true,
                bPaginate: true,
                dom: 'Bfrtip',
                buttons: ['copy', 'excel'],  
            });
    });
</script>

<script>
    document.addEventListener("DOMContentLoaded", function () {
        var chkOferta = document.getElementById('<%= chkOferta.ClientID %>');
        var txtDesde = document.getElementById('<%= txtModalDesde.ClientID %>');
        var txtHasta = document.getElementById('<%= txtModalHasta.ClientID %>');
        var labelsFecha = document.querySelectorAll(".fecha");
       
        function toggleFechas() {
            var displayValue = chkOferta.checked ? "block" : "none";
            txtDesde.style.display = displayValue;
            txtHasta.style.display = displayValue;
            labelsFecha.forEach(function (label) {
                label.style.display = displayValue;
            });
        }
        chkOferta.addEventListener("change", toggleFechas);
        toggleFechas();
    });
</script>
