<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Contratistas.aspx.vb" Inherits="Contratistas" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Contratistas y Mecanicos</title>
    <style type="text/css">
        body {
            font-family: Arial;
            font-size: 10pt;
        }
    </style>
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
            <h1 class="display-4">Contratistas</h1>
            <hr class="my-4">
            <p class="lead">
                <asp:Button ID="btnAgregarContratista" runat="server" class="btn btn-success rounded" Text="Agregar Contratista" />
            </p>
        </div>
        <asp:Panel ID="pnlContratistasMain" runat="server" Visible="true">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <asp:GridView ID="GRID_1" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" Style="width: 100%">
                            <HeaderStyle CssClass="thead-dark" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="Imagenes/find.png" Text="Buscar" CommandName="Buscar" HeaderText="Mecanicos">
                                    <ControlStyle Height="18px" Width="18px" />
                                    <ItemStyle Wrap="False" />
                                </asp:ButtonField>
                                <asp:ButtonField ButtonType="Image" ImageUrl="Imagenes/edit.png" Text="Editar" CommandName="Editar" HeaderText="Editar">
                                    <ControlStyle Height="18px" Width="18px" />
                                    <ItemStyle Wrap="False" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="ID" HeaderText="Id" />
                                <asp:BoundField DataField="GRUPONAME" HeaderText="Codigo" />
                                <asp:BoundField DataField="GRUPORESPONSABLE" HeaderText="Nombre Contratista" />
                                <asp:BoundField DataField="BPCODE" HeaderText="Codigo SAP" />
                                <asp:BoundField DataField="GRUPOEMAILRESPONSABLE" HeaderText="Correo Contratista" />
                                <asp:BoundField DataField="GRUPOSTATUS" HeaderText="Activo" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <br />
        <br />
        <%--PANEL PRINCIPAL MECANICOS--%>
        <asp:Panel ID="pnlMecanicosDetalle" runat="server" Visible="false">
            <h3>Lista Mecanicos Contratista</h3>
            <asp:Button ID="btnAgregarMeca" runat="server" class="btn btn-success rounded" Text="Agregar Mecanico" />
            <br />
            <div class="container-fluid">


                <asp:GridView ID="GRID_2" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" Style="width: 100%">
                    <HeaderStyle CssClass="thead-dark" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" ImageUrl="Imagenes/edit.png" Text="Editar" CommandName="Editar" HeaderText="Editar">
                            <ControlStyle Height="18px" Width="18px" />
                            <ItemStyle Wrap="False" />
                        </asp:ButtonField>
                        <asp:BoundField DataField="ID" HeaderText="Id" />
                        <asp:BoundField DataField="GRUPOID" HeaderText="Codigo" />
                        <asp:BoundField DataField="MECANICONAME" HeaderText="Nombre Mecanico" />
                        <asp:BoundField DataField="MECANICOEMAIL" HeaderText="Correo Mecanico" />
                        <asp:BoundField DataField="MECANICOESTATUS" HeaderText="Activo" />
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>
        <%--PANEL CREAR CONTRATISTA--%>
        <asp:Panel ID="pnlCrearContratista" runat="server" Visible="false">
            <center>
                <h3>Crear Contratista</h3>
                <style>
                    .contenedor {
                        width: 25% !important;
                        height: 25% !important;
                    }
                </style>
                <div class="contenedor">
                    <div class="form-group">
                        <div class="row">
                            <div class="col">
                                <label for="txtCodigoContratista">Codigo Contratista</label>
                                <asp:TextBox ID="txtCodigoContratista" runat="server" class="form-control" autocoplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <label for="txtResponsableContratista">Nombre Responsable</label>
                                <asp:TextBox ID="txtResponsableContratista" runat="server" class="form-control" autocoplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <label for="txtEmailContratista">Correo de Contratista</label>
                                <asp:TextBox ID="txtEmailContratista" runat="server" class="form-control" autocoplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col">
                                <asp:Button ID="Button2" runat="server" class="form-control btn btn-primary rounded submit px-3" Text="Agregar Contratista" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col">
                                <asp:Button ID="Button3" runat="server" class="form-control btn btn-secondary rounded px-3" Text="Regresar" />
                            </div>
                        </div>
                    </div>
                </div>
            </center>
        </asp:Panel>
        <%--PANEL MODIFICAR CONTRATISTA--%>
        <asp:Panel ID="pnlModificarContratista" runat="server" Visible="false">
            <center>
                <h3>Actualizar Contratista</h3>
                <style>
                    .contenedor {
                        width: 25% !important;
                        height: 25% !important;
                    }
                </style>
                <div class="contenedor">
                    <div class="form-group">
                        <div class="row">
                            <div class="col">
                                <label for="txtUpdateContratistaID">ID</label>
                                <asp:TextBox ID="txtUpdateContratistaID" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <label for="txtUpdateUpdateConttratistaCode">Codigo Contratista</label>
                                <asp:TextBox ID="txtUpdateContratistaCode" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <label for="txtUpdateContratistaName">Actualizar Responsable Contratista</label>
                                <asp:TextBox ID="txtUpdateContratistaName" runat="server" class="form-control" autocoplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <label for="txtUpdateContratistaEmail">Actualizar Correo Responsable Contratista</label>
                                <asp:TextBox ID="txtUpdateContratistaEmail" runat="server" class="form-control" autocoplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <label for="drpEstadoContratista">Estado del Contratista</label>
                                <br />
                                <asp:DropDownList ID="drpUpdateEstadoContratista" runat="server" class="btn btn-secondary dropdown-toggle; text-left">
                                    <asp:ListItem Selected="True" Value="True">Activo</asp:ListItem>
                                    <asp:ListItem Value="False">Inactivo</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col">
                                <asp:Button ID="btnActualizarContratista" runat="server" class="form-control btn btn-primary rounded submit px-3" Text="Actualizar Datos Contratista" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col">
                                <asp:Button ID="btnRegresarContratista" runat="server" class="form-control btn btn-secondary rounded px-3" Text="Regresar" />
                            </div>
                        </div>
                    </div>
                </div>
            </center>
        </asp:Panel>
        <%--PANEL CREAR MECANICO--%>
        <asp:Panel ID="pnlCrearMecanico" runat="server" Visible="false">
            <center>
                <h3>Crear Mecanico</h3>
                <style>
                    .contenedor {
                        width: 25% !important;
                        height: 25% !important;
                    }
                </style>
                <div class="contenedor">
                    <div class="form-group">
                        <div class="row">
                            <div class="col">
                                <label for="drpGrupoContratistas">Grupo de Contratista</label>
                                <br />
                                <asp:DropDownList ID="drpGrupoContratistas" runat="server" class="btn btn-secondary dropdown-toggle; text-left">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <label for="txtNMecanico">Nombre Mecanico</label>
                                <asp:TextBox ID="txtNMecanico" runat="server" class="form-control" autocoplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <label for="txtEmailMeca">Correo de Mecanico</label>
                                <asp:TextBox ID="txtEmailMeca" runat="server" class="form-control" autocoplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col">
                                <asp:Button ID="btnAGegarMecanico" runat="server" class="form-control btn btn-primary rounded submit px-3" Text="Agregar Mecanico" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col">
                                <asp:Button ID="btnExit" runat="server" class="form-control btn btn-secondary rounded px-3" Text="Regresar" />
                            </div>
                        </div>
                    </div>
                </div>
            </center>
        </asp:Panel>
        <%--PANEL MODIFICAR MECANICO--%>
        <asp:Panel ID="pnlModificarMecanico" runat="server" Visible="false">
            <center>
                <style>
                    .contenedor {
                        width: 25% !important;
                        height: 25% !important;
                    }
                </style>
                <h3>Actualizar Mecanico</h3>
                <div class="contenedor">
                    <div class="form-group">
                        <div class="row">
                            <div class="col">
                                <label for="txtUpdateMecanicoID">ID</label>
                                <asp:TextBox ID="txtUpdateMecanicoID" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <label for="txtUpdateMecanicoGroupCode">Codigo Contratista</label>
                                <asp:TextBox ID="txtUpdateMecanicoGroupCode" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <label for="drpUpdateGrupoMeca">Modificar Grupo Contratista</label>
                                <br />
                                <asp:DropDownList ID="drpUpdateGrupoMeca" runat="server" class="btn btn-secondary dropdown-toggle; text-left">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <label for="txtUpdateMecaName">Actualizar Nombre Mecanico</label>
                                <asp:TextBox ID="txtUpdateMecaName" runat="server" class="form-control" autocoplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <label for="txtUpdateUSerEmail">Actualizar Correo Mecanico</label>
                                <asp:TextBox ID="txtUpdateMecaEmail" runat="server" class="form-control" autocoplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <label for="drpEstadoMecanico">Estado del Contratista</label>
                                <br />
                                <asp:DropDownList ID="drpUpdateEstadoMeca" runat="server" class="btn btn-secondary dropdown-toggle; text-left">
                                    <asp:ListItem Selected="True" Value="True">Activo</asp:ListItem>
                                    <asp:ListItem Value="False">Inactivo</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <br />
                        <br />
                        <div class="row">
                            <div class="col">
                                <asp:Button ID="btnUpdate" runat="server" class="form-control btn btn-primary rounded submit px-3" Text="Actualizar Datos Mecanico" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col">
                                <asp:Button ID="btnCancel" runat="server" class="form-control btn btn-secondary rounded px-3" Text="Regresar" />
                            </div>
                        </div>
                    </div>
                </div>
            </center>
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

    new DataTable('#GRID_1', {
        fixedHeader: {
            header: true,
            footer: true
        },
        "sort": false,
        paging: false,
        scrollCollapse: true,
        scrollX: true,
        scrollY: 700
    });

    new DataTable('#GRID_2', {
        fixedHeader: {
            header: true,
            footer: true
        },
        "sort": false,
        paging: false,
        scrollCollapse: true,
        scrollX: true,
        scrollY: 700
    });
</script>