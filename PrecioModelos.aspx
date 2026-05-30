<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PrecioModelos.aspx.vb" Inherits="PrecioModelos" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Parametrizaciones || Modelos y Precios Armado</title>

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
        <h1 class="display-4">Precios de Armado</h1>
        <p class="lead">
            <asp:Button ID="btnIngreso" runat="server" class="btn btn-success rounded" Text="Agregar Modelo" />
        </p>
        <asp:Panel ID="pnlGridUsuarios" runat="server" Visible="true">
            <div class="d-flex justify-content-center">
                <div class="row">
                    <div class="col-12">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" Style="width: 100%">
                            <HeaderStyle CssClass="thead-dark" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="~/Imagenes/edit.png" Text="Modificar" CommandName="Modificar">
                                    <ControlStyle Height="18px" Width="18px" />
                                    <ItemStyle Wrap="False" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="CODE" HeaderText="Codigo" />
                                <asp:BoundField DataField="NAME" HeaderText="Modelo" />
                                <asp:BoundField DataField="U_LG5" HeaderText="Activo CB" />
                                <asp:BoundField DataField="U_LG6" HeaderText="Inv Reserva" />
                                <asp:BoundField DataField="U_LG7" HeaderText="Tipo Moto" />
                                <asp:BoundField DataField="U_Orden" HeaderText="Ordenamiento" />
                                <asp:BoundField DataField="U_Precio" HeaderText="Precio Armado" ItemStyle-HorizontalAlign="Right" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlCrearUsuario" runat="server" Visible="false">
            <div class="d-flex justify-content-center align-items-center">
                <div class="container" style="max-width: 600px;">
                    <div class="row">
                        <div class="col">
                            <label for="txtCreateNextCode">Código Nuevo</label>
                            <asp:TextBox ID="txtCreateNextCode" runat="server" class="form-control" autocoplete="off" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col">
                            <label for="txtCreateNombreModelo">Nombre de Modelo</label>
                            <asp:TextBox ID="txtCreateNombreModelo" runat="server" class="form-control" autocoplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col">
                            <label for="txtCreatePrecioArmado">Precio Armado</label>
                            <asp:TextBox ID="txtCreatePrecioArmado" runat="server" class="form-control" autocoplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col">
                            <label for="drpCreateDisponibleCB">Disponible para Cuadro Básico?</label>
                            <br />
                            <asp:DropDownList ID="drpCreateDisponibleCB" runat="server" class="btn btn-secondary dropdown-toggle text-left" data-toggle="dropdown" AutoPostBack="true">
                                <asp:ListItem Selected="True" Value="Y">Activo</asp:ListItem>
                                <asp:ListItem Value="N">Inactivo</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col">
                            <label for="drpCreateTipoMoto">Tipo de Moto</label>
                            <br />
                            <asp:DropDownList ID="drpCreateTipoMoto" runat="server" class="btn btn-secondary dropdown-toggle text-left" data-toggle="dropdown" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row mt-2">
                        <div class="col">
                            <asp:Button ID="btnCreateModelo" runat="server" class="form-control btn btn-primary rounded submit px-3" Text="Crear Modelo" />
                        </div>
                        <div class="col">
                            <asp:Button ID="btnExit" runat="server" class="form-control btn btn-secondary rounded px-3" Text="Regresar" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlUpdate" runat="server" Visible="false">
            <div class="d-flex justify-content-center align-items-center">
                <div class="container" style="max-width: 600px;">
                    <div class="row">
                        <div class="col">
                            <label for="txtUpdateCodeModelo">Código de Modelo</label>
                            <asp:TextBox ID="txtUpdateCodeModelo" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col">
                            <label for="txtUpdateNombreModelo">Nombre Modelo Moto</label>
                            <asp:TextBox ID="txtUpdateNombreModelo" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col">
                            <label for="txtInvReserva">Inventario Reserva</label>
                            <asp:TextBox ID="txtUpdateInvReserva" runat="server" class="form-control" autocoplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col">
                            <label for="txtUpdatePrecioArmado">Precio Armado</label>
                            <asp:TextBox ID="txtUpdatePrecioArmado" runat="server" class="form-control" autocoplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col">
                            <label for="drpUpdateCreateDisponibleCB">Disponible para Cuadro Básico?</label>
                            <br />
                            <asp:DropDownList ID="drpUpdateDisponibleCB" runat="server" class="btn btn-secondary dropdown-toggle text-left" data-toggle="dropdown" AutoPostBack="false" Width="100%">
                                <asp:ListItem Selected="True" Value="Y">Activo</asp:ListItem>
                                <asp:ListItem Value="N">Inactivo</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col">
                            <label for="drpUpdateTipoMoto">Tipo de Moto</label>
                            <br />
                            <asp:DropDownList ID="drpUpdateTipoMoto" runat="server" class="btn btn-secondary dropdown-toggle text-left" data-toggle="dropdown" AutoPostBack="false" Width="100%">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col">
                            <asp:Button ID="btnUpdateModelo" runat="server" class="form-control btn btn-primary rounded submit px-3" Text="Actualizar Modelo" />
                        </div>
                        <div class="col">
                            <asp:Button ID="btnCancel" runat="server" class="form-control btn btn-secondary rounded px-3" Text="Regresar" />
                        </div>
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
</script>
