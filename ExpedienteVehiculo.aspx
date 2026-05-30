<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ExpedienteVehiculo.aspx.vb" Inherits="ExpedienteVehiculo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Expediente Vehiculo</title>
    <link href="https://fonts.googleapis.com/css?family=Lato:300,400,700&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="css/style.css" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
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
                                <!-- <asp:Panel ID="pnlArmadoMotos" runat="server" Visible="True"><li><a class="dropdown-item" href="ArmadoMotos.aspx">Armado Motos</a></li></asp:Panel> -->
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
        <%--JUMBOTRON--%>
        <div class="jumbotron">
            <h1 class="display-4">Expediente Vehiculo Armado</h1>
            <hr class="my-4">
            <p class="lead">
            </p>
        </div>
        <asp:Panel ID="pnlGridControlCalidad" runat="server" Visible="true">
            <style>
                .contenedor {
                    width: 90% !important;
                    height: 90% !important;
                }
            </style>
            <center>
                <div class="contenedor">

                    <div class="row">
                        <div class="col">
                            <asp:TextBox ID="txtSnMoto" runat="server" class="form-control" autocomplete="off"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServiceMethod="SearchCustomers"
                                MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
                                TargetControlID="txtSnMoto" FirstRowSelected="false">
                            </cc1:AutoCompleteExtender>
                        </div>
                        <div class="col">
                            <asp:Button ID="btnBuscarMoto" runat="server" class="form-control btn btn-primary rounded submit px-3" Text="Buscar Serie" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtEstadpSN" runat="server" class="form-control" ReadOnly="true" placeholder="Estado de la Serie"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtEstadoProduccion" runat="server" class="form-control" ReadOnly="true" placeholder="Estado Produccion"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtEstadoMoto" runat="server" class="form-control" ReadOnly="true" placeholder="Estado del Vehiculo"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col">
                            <label for="lblItemcode">Articulo</label>
                            <asp:TextBox ID="lblItemcode" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col">
                            <label for="lblItemname">Descripcion Moto</label>
                            <asp:TextBox ID="lblItemname" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col">
                            <label for="lblModelo">Modelo</label>
                            <asp:TextBox ID="lblModelo" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col">
                            <label for="lblMarca">Marca</label>
                            <asp:TextBox ID="lblMarca" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col">
                            <label for="lblYear">Año</label>
                            <asp:TextBox ID="lblYear" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">

                        <div class="col">
                            <label for="lblCilindros">Cilindros</label>
                            <asp:TextBox ID="lblCilindros" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col">
                            <label for="lblColor">Color</label>
                            <asp:TextBox ID="lblColor" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col">
                            <label for="lblMotorM">Motor</label>
                            <asp:TextBox ID="lblMotorM" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col">
                            <label for="txtMecanico">Mecanico</label>
                            <asp:TextBox ID="txtMecanico" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col">
                            <label for="txtControlCalidad">Control Calidad</label>
                            <asp:TextBox ID="txtControlCalidad" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <br />

                    <asp:Panel ID="Panel1" runat="server" Visible="true">
                        <div class="container-fluid">
                            <div class="col">
                                <asp:GridView ID="GridView1" runat="server" CssClass="display compact" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundField DataField="MnfSerial" HeaderText="Serie" />
                                        <asp:BoundField DataField="Quantity" HeaderText="Cantidad" />
                                        <asp:BoundField DataField="Documento" HeaderText="Tipo Documento" />
                                        <asp:BoundField DataField="Docnum" HeaderText="Documento" />
                                        <asp:BoundField DataField="DocDate" HeaderText="Fecha" />
                                        <asp:BoundField DataField="LocCode" HeaderText="Cod. Almacen" />
                                        <asp:BoundField DataField="Almacen" HeaderText="Nom. Almacen" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </asp:Panel>
                    <br />

                    <div class="row">
                        <div class="col">
                            <asp:Button ID="btnExit" runat="server" class="form-control btn btn-secondary rounded px-3" Text="Limpiar Formulario" />
                        </div>
                        <div class="col">
                            <asp:Button ID="btnBack" runat="server" CssClass="form-control btn btn-secondary rounded px-3" Text="Regresar"     
                                OnClientClick="window.location='InformeMotosDisponibles.aspx'; return false;" />
                        </div>
                    </div>
                </div>
        </asp:Panel>
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
        <asp:ScriptManager ID="sm1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    </form>
</body>
</html>
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
<link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    $(function () {
        $("[id*=GridView1]").DataTable(
            {
                bLengthChange: true,
                lengthMenu: [[10, -1], [10, "All"]],
                bFilter: true,
                bSort: false,
                bPaginate: true
            });
    });
</script>
