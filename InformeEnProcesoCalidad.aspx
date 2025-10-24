<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InformeEnProcesoCalidad.aspx.vb" Inherits="InformeEnProcesoCalidad" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Motos Control Calidad Procesando</title>
    <link href="https://fonts.googleapis.com/css?family=Lato:300,400,700&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="css/style.css" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
    <link href="css/movesa_load_spinner.css" rel="stylesheet" />
    <style>
        .grid-container {
            overflow-x: auto; /* Habilita el desplazamiento horizontal en caso de que sea necesario */
        }

            .grid-container table {
                width: 100%; /* Asegura que el GridView utilice todo el ancho disponible */
            }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="overflow-y:scroll;">
        <div class="pos-f-t">
            <div class="collapse" id="navbarToggleExternalContent">
                <div class="bg-dark p-4">
                    <ul class="nav nav-tabs">
                        <li class="nav-item">
                            <a class="nav-link" href="MainDashBoard.aspx">
                                <img src="Imagenes/mnegra.png" width="30" height="25" alt="Portal Armado de Motos" />
                            </a>
                        </li>

                        <li class="nav-item dropdown">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Parametrizaciones</a>
                            <ul class="dropdown-menu">
                                <li><a class="dropdown-item" href="Contratistas.aspx">Contratistas</a></li>
                                <li><a class="dropdown-item" href="TrabajosAdicionales.aspx">Trabajos Adicionales</a></li>
                                <li><a class="dropdown-item" href="PrecioModelos.aspx">Precios Armado</a></li>
                                <li><a class="dropdown-item" href="PinturaColores.aspx">Pintura Colores</a></li>
                                <li><a class="dropdown-item" href="Usuarios.aspx">Usuarios</a></li>
                            </ul>
                        </li>

                        <li class="nav-item dropdown">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Motos en Caja</a>
                            <ul class="dropdown-menu">
                                <li><a class="dropdown-item" href="OrdendeTrabajo.aspx">Planificador de Trabajo</a></li>
                                <li><a class="dropdown-item" href="OrdenTrabajoConfirmacion.aspx">Confirmacion Supervisor</a></li>
                                <li><a class="dropdown-item" href="AutorizacionCreditos.aspx">Autorizacion Plan Trabajo Creditos</a></li>
                                <li><a class="dropdown-item" href="OrdenProduccion.aspx">Planificador de Trabajo</a></li>
                                <li><a class="dropdown-item" href="MotoenCaja.aspx">Asignar Moto</a></li>
                                <li><a class="dropdown-item" href="StockPorModelos.aspx">Stock Por Modelo</a></li>
                            </ul>
                        </li>

                        <li class="nav-item dropdown">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Armado</a>
                            <ul class="dropdown-menu">
                                <li><a class="dropdown-item" href="ListadoProceso.aspx">Motos En Proceso</a></li>
                            </ul>
                        </li>

                        <li class="nav-item dropdown">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Control de Calidad</a>
                            <ul class="dropdown-menu">
                                <li><a class="dropdown-item" href="ListadoCCalidad.aspx">Formulario Control Calidad Manual</a></li>
                                <li><a class="dropdown-item" href="InformeCCFinArmado.aspx">Moto Pendiente Control Calidad</a></li>
                                <li><a class="dropdown-item" href="InformeEnProcesoCalidad.aspx">Moto en Proceso de Calidad</a></li>
                                <li><a class="dropdown-item" href="InformeCC.aspx">Motos Control de Calidad Finalizado</a></li>
                            </ul>
                        </li>

                        <li class="nav-item dropdown">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Proceso Contable</a>
                            <ul class="dropdown-menu">
                                <li><a class="dropdown-item" href="ProcesoContable.aspx">Crear Liquidacion</a></li>
                                <li><a class="dropdown-item" href="GenerarPO.aspx">Crear Orden de Compra</a></li>
                                <li><a class="dropdown-item" href="ProcesoContableConsulta.aspx">Consulta Liquidaciones Anteriores</a></li>
                            </ul>
                        </li>

                        <li class="nav-item dropdown">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Disponibles</a>
                            <ul class="dropdown-menu">
                                <li><a class="dropdown-item" href="InformeMotosDisponibles.aspx">Motos Armadas Procesadas Portal</a></li>
                                <li><a class="dropdown-item" href="ExpedienteVehiculo.aspx">Expediente Vehiculo</a></li>
                            </ul>
                        </li>

                        <li class="nav-item dropdown">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">No Disponibles</a>
                            <ul class="dropdown-menu">
                                <li><a class="dropdown-item" href="InformeMotosNoDisponibles.aspx">Motos No Disponibles</a></li>
                                <li><a class="dropdown-item" href="ExpedienteVehiculoGarantia.aspx">Expediente Vehiculo Garantia / ND</a></li>
                                <li><a class="dropdown-item" href="RepuestosRetirados.aspx">Repuestos Retirados</a></li>
                                <li><a class="dropdown-item" href="OCRProveedor.aspx">Orden de Compra Proveedor</a></li>
                                <li><a class="dropdown-item" href="PedidoRepuestosFeedback.aspx">Subir Informacion Compra Repuesto</a></li>
                            </ul>
                        </li>

                        <li class="nav-item">
                            <a class="nav-link" href="Default.aspx">Salir</a>
                        </li>
                    </ul>
                    <h4 class="text-white">Bienvenido</h4>
                    <span class="text-muted">
                        <asp:Label ID="lblNombreUsuario" runat="server" Text="Usuario Conectado"></asp:Label>
                    </span>
                </div>
            </div>

            <nav class="navbar navbar-dark bg-dark">
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarToggleExternalContent" aria-controls="navbarToggleExternalContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
            </nav>
        </div>
        <div class="grid-container">

            <h1 class="text-center">Motos Control Calidad Procesando</h1>
            <style>
                .box {
                    display: flex;
                    align-items: stretch;
                    justify-content: center;
                    gap: 25px;
                }

                .btn {
                    padding: 30px;
                }
            </style>
            <div class="box">
                <div>
                    <a href="InformeCCFinArmado.aspx" class="btn btn-danger and-all-other-classes" style="color: inherit; text-decoration: none; display: block; text-align: center;">Motos Armada Para Control Calidad</a>
                </div>
                <div>
                    <a href="InformeEnProcesoCalidad.aspx" class="btn btn-warning and-all-other-classes" style="color: inherit; text-decoration: none; display: block; text-align: center;">Motos en Proceso de Control Calidad</a>
                </div>
                <div>
                    <a href="InformeCC.aspx" class="btn btn-success and-all-other-classes" style="color: inherit; text-decoration: none; display: block; text-align: center;">Moto Control de Calidad Finalizado</a>
                </div>
                <div>
                    <a href="InformeCCProcesoDetalle.aspx" class="btn btn-secondary and-all-other-classes"
                        style="color: inherit; text-decoration: none; display: block; text-align: center;">Motos En Proceso Pintura</a>
                </div>
            </div>
             <div class="container-fluid" style="overflow-x: scroll;overflow-y:scroll;">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                    CssClass="table table-bordered table-hover">
                    <HeaderStyle CssClass="thead-dark" />
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" />
                        <asp:BoundField DataField="SERIE" HeaderText="Serie" />
                        <asp:BoundField DataField="MODELO" HeaderText="Modelo" />
                        <asp:BoundField DataField="COLOR" HeaderText="Color" />
                        <asp:BoundField DataField="ESTATUS" HeaderText="Estado" />
                        <asp:BoundField DataField="ALMACEN" HeaderText="Almacen" />
                        <asp:BoundField DataField="ESTADO" HeaderText="Estatus" />
                        <asp:BoundField DataField="DIAS" HeaderText="Dias" />
                        <asp:BoundField DataField="AUDITORCCASIGNADO" HeaderText="Auditor CC" />
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/finish.png" Text="Terminar" CommandName="Terminar" HeaderText="Terminar">
                            <ControlStyle Height="50px" Width="50px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center" />
                        </asp:ButtonField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <center>
            <divs class="container">
                <footer class="main-footer">
                    <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
                </footer>
            </divs>
        </center>
        <asp:ScriptManager ID="sm1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    </form>
</body>
</html>
<script src="js/jquery.min.js"></script>
<script src="js/popper.js"></script>
<script src="js/bootstrap.min.js"></script>
<script src="js/main.js"></script>
<link href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.css" rel="stylesheet" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.js"></script>

<!-- DataTables  & Plugins -->
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.1/js/dataTables.buttons.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.html5.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.print.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.colVis.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
<link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />
<link href="https://cdn.datatables.net/buttons/1.7.1/css/buttons.dataTables.min.css" rel="stylesheet" type="text/css" />

<script type="text/javascript">
    $(function () {
        $("[id*=GridView1]").DataTable({
            dom: 'Bfrtip',
            buttons: [
                'excelHtml5',
                'pdfHtml5',
                'colvis'
            ],
            bLengthChange: true,
            lengthMenu: [[10, -1], [10, "All"]],
            bFilter: true,
            bSort: true,
            bPaginate: true,
            order: [[0, "desc"]]
        });
    });
</script>
