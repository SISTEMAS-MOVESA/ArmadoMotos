<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SapNewItems.aspx.vb" Inherits="SapNewItems" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Crear Articulos SAP</title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback" />
    <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css" />
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css" />
    <link rel="stylesheet" href="plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css" />
    <link rel="stylesheet" href="plugins/icheck-bootstrap/icheck-bootstrap.min.css" />
    <link rel="stylesheet" href="plugins/jqvmap/jqvmap.min.css" />
    <link rel="stylesheet" href="dist/css/adminlte.min.css" />
    <link rel="stylesheet" href="plugins/overlayScrollbars/css/OverlayScrollbars.min.css" />
    <link rel="stylesheet" href="plugins/daterangepicker/daterangepicker.css" />
    <link rel="stylesheet" href="plugins/summernote/summernote-bs4.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>


    <style>
        .col-1, .col-2, .col-3, .col-6 {
            margin-top: 10px;
            margin-bottom: 10px;
        }

        .theme-checkbox {
            --toggle-size: 16px;
            -webkit-appearance: none;
            -moz-appearance: none;
            appearance: none;
            width: 6.25em;
            height: 3.125em;
            background: -webkit-gradient(linear, left top, right top, color-stop(50%, #efefef), color-stop(50%, #2a2a2a)) no-repeat;
            background: -o-linear-gradient(left, #efefef 50%, #2a2a2a 50%) no-repeat;
            background: linear-gradient(to right, #efefef 50%, #2a2a2a 50%) no-repeat;
            background-size: 205%;
            background-position: 0;
            -webkit-transition: 0.4s;
            -o-transition: 0.4s;
            transition: 0.4s;
            border-radius: 99em;
            position: relative;
            cursor: pointer;
            font-size: var(--toggle-size);
        }

            .theme-checkbox::before {
                content: "";
                width: 2.25em;
                height: 2.25em;
                position: absolute;
                top: 0.438em;
                left: 0.438em;
                background: -webkit-gradient(linear, left top, right top, color-stop(50%, #efefef), color-stop(50%, #2a2a2a)) no-repeat;
                background: -o-linear-gradient(left, #efefef 50%, #2a2a2a 50%) no-repeat;
                background: linear-gradient(to right, #efefef 50%, #2a2a2a 50%) no-repeat;
                background-size: 205%;
                background-position: 100%;
                border-radius: 50%;
                -webkit-transition: 0.4s;
                -o-transition: 0.4s;
                transition: 0.4s;
            }

            .theme-checkbox:checked::before {
                left: calc(100% - 2.25em - 0.438em);
                background-position: 0;
            }

            .theme-checkbox:checked {
                background-position: 100%;
            }
    </style>

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
        <div class="container-fluid my-3">
            <h1 class="text-center">Creacion Items SAP | Garantias</h1>
            <div class="row">
                    <div class="col-1">
                        Codigo de Articulo
                    </div>
                    <div class="col-2">
                        <asp:TextBox ID="txtItemcode" runat="server" placeholder="Codigo de Articulo" Width="100%" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvItemcode" runat="server" ControlToValidate="txtItemcode" ErrorMessage="Este campo es obligatorio." CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <div class="col-1">
                        Codigo de Barra
                    </div>
                    <div class="col-2">
                        <asp:TextBox ID="txtCodeBars" runat="server" placeholder="Codigo de Barras" Width="100%" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCodeBars" runat="server" ControlToValidate="txtCodeBars" ErrorMessage="Este campo es obligatorio." CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <div class="col-1">
                        Codigo Fabricante
                    </div>
                    <div class="col-2">
                        <asp:TextBox ID="txtSuppCatNum" runat="server" placeholder="Codigo de Proveedor" Width="100%" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvSuppCatNum" runat="server" ControlToValidate="txtSuppCatNum" ErrorMessage="Este campo es obligatorio." CssClass="text-danger" Display="Dynamic" />
                    </div>
                </div>

                <hr />

                <div class="row">
                    <div class="col-1">
                        Descripcion Español
                    </div>
                    <div class="col-8">
                        <asp:TextBox ID="txtItenmane" runat="server" placeholder="Descripcion Español" Width="100%" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvItenmane" runat="server" ControlToValidate="txtItenmane" ErrorMessage="Este campo es obligatorio." CssClass="text-danger" Display="Dynamic" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-1">
                        Descripcion Ingles
                    </div>
                    <div class="col-8">
                        <asp:TextBox ID="txtFrgnName" runat="server" placeholder="Descripcion Ingles" Width="100%" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFrgnName" runat="server" ControlToValidate="txtFrgnName" ErrorMessage="Este campo es obligatorio." CssClass="text-danger" Display="Dynamic" />
                    </div>
                </div>

            <div class="row">
                <div class="col-1">
                    Grupo de Articulos
                </div>
                <div class="col-3">
                    <asp:DropDownList ID="drpGrupoArticulos" runat="server" class="ui search dropdown fluid" name="drpGrupoArticulos">
                        <asp:ListItem Selected="True" Value="0" Text="Seleccione un Grupo de Articulos"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-1">
                    Sub Grupo
                </div>
                <div class="col-3">
                    <asp:TextBox ID="txtSubGrupo" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvSubGrupo" runat="server" ControlToValidate="txtFrgnName" ErrorMessage="Este campo es obligatorio." CssClass="text-danger" Display="Dynamic" />
                </div>
            </div>
            <div class="row">
                <div class="col-1">
                    Proveedor
                </div>
                <div class="col-3">
                    <asp:DropDownList ID="drpProveedores" runat="server" class="ui search dropdown fluid" name="drpProveedores">
                        <asp:ListItem Selected="True" Value="0" Text="Seleccione un Grupo de Articulos"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-1">
                    Arancel
                </div>
                <div class="col-3">
                    <asp:DropDownList ID="drpArancel" runat="server" class="ui search dropdown fluid" name="drpArancel">
                        <asp:ListItem Selected="True" Value="0" Text="Seleccione un Sub Grupo"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-1">
                    Art de Inv
                </div>
                <div class="col-1">
                    <input id="checkInvItem" runat="server" type="checkbox" class="theme-checkbox checked" checked onclick="return false;" />
                </div>
                <div class="col-1">
                    Art Compra
                </div>
                <div class="col-1">
                    <input id="checkInvPur" runat="server" type="checkbox" class="theme-checkbox checked" checked onclick="return false;" />

                </div>
                <div class="col-1">
                    Art de Venta
                </div>
                <div class="col-1">
                    <input id="checkInvSales" runat="server" type="checkbox" class="theme-checkbox checked" checked onclick="return false;" />
                </div>
            </div>
            <div class="row">
                <div class="col-1">
                    Gestion x almacen
                </div>
                <div class="col-1">
                    <input id="checkManageByWhs" runat="server" type="checkbox" class="theme-checkbox checked" checked onclick="return false;" />
                </div>
                <div class="col-1">
                    Propiedad 64
                </div>
                <div class="col-1">
                    <input id="checkProperty64" runat="server" type="checkbox" class="theme-checkbox checked" checked />
                </div>
                <div class="col-1">
                    Propiedad 59
                </div>
                <div class="col-1">
                    <input id="checkProperty59" runat="server" type="checkbox" class="theme-checkbox checked" checked />
                </div>
                <div class="col-1">
                    COMPRAS
                </div>
                <div class="col-2">
                    <asp:TextBox ID="txtOEM" runat="server" Text="OEM" Width="100%" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvOEM" runat="server" ControlToValidate="txtFrgnName" ErrorMessage="Este campo es obligatorio." CssClass="text-danger" Display="Dynamic" />

                </div>
            </div>
            <div class="row">
                <div class="col-1">
                    Seleccione un Modelo
                </div>
                <div class="col-3">
                    <asp:DropDownList ID="drpModelo" runat="server" class="ui search dropdown fluid" name="drpModelo">
                        <asp:ListItem Selected="True" Value="0" Text="Seleccione un Modelo"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <hr />
            <div class="d-flex justify-content-center">
                <div class="col-2">
                    <asp:Button ID="btnLimpiarFormulario" runat="server" CssClass="btn btn-danger" Text="Limpiar Formulario" CausesValidation="False" />
                    <%--<asp:Button ID="btnLimpiarFormulario" runat="server" CssClass="btn btn-danger" Text="Limpiar Formulario" with="200px" CausesValidation="False"/>--%>
                </div>
                <div class="col-2">
                    <asp:Button ID="btnVerificarExiste" runat="server" CssClass="btn btn-info" Text="Verificar Si Existe" CausesValidation="False" />
                </div>
                <div class="col-2">
                    <asp:Button ID="btnActualizar" runat="server" CssClass="btn btn-warning" Text="Actualziar Articulo" />

                </div>
                <div class="col-2">
                    <asp:Button ID="btnCrearItem" runat="server" CssClass="btn btn-success" Text="Crear Articulo"/>

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
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    </form>
</body>
</html>
<script src="plugins/jquery/jquery.min.js"></script>
<script src="plugins/jquery-ui/jquery-ui.min.js"></script>

<script src="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.2/semantic.min.js" integrity="sha512-5cguXwRllb+6bcc2pogwIeQmQPXEzn2ddsqAexIBhh7FO1z5Hkek1J9mrK2+rmZCTU6b6pERxI7acnp1MpAg4Q==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.2/semantic.min.css" integrity="sha512-n//BDM4vMPvyca4bJjZPDh7hlqsQ7hqbP9RH18GF2hTXBY5amBwM2501M0GPiwCU/v9Tor2m13GOTFjk00tkQA==" crossorigin="anonymous" referrerpolicy="no-referrer" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.js" integrity="sha512-Xo0Jh8MsOn72LGV8kU5LsclG7SUzJsWGhXbWcYs2MAmChkQzwiW/yTQwdJ8w6UA9C6EVG18GHb/TrYpYCjyAQw==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.css" integrity="sha512-KXol4x3sVoO+8ZsWPFI/r5KBVB/ssCGB5tsv2nVOKwLg33wTFP3fmnXa47FdSVIshVTgsYk/1734xSk9aFIa4A==" crossorigin="anonymous" referrerpolicy="no-referrer" />

<script>

    function convertToUppercase(elementId) {
        var textBox = document.getElementById(elementId);
        textBox.value = textBox.value.toUpperCase();
    }

    $("#drpProveedores").dropdown(
        {
            "fullTextSearch": true
        });
    $("#drpArancel").dropdown(
        {
            "fullTextSearch": true
        });
    $("#drpSubGrupo").dropdown(
        {
            "fullTextSearch": true
        });
    $("#drpGrupoArticulos").dropdown(
        {
            "fullTextSearch": true
        });
    $("#drpModelo").dropdown(
        {
            "fullTextSearch": true
        });

    $("#drpArancel").dropdown(
        {
            "fullTextSearch": true
        });
</script>


<script>
    $.widget.bridge('uibutton', $.ui.button)    
</script>
<script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
<script src="plugins/chart.js/Chart.min.js"></script>
<script src="plugins/sparklines/sparkline.js"></script>
<script src="plugins/jqvmap/jquery.vmap.min.js"></script>
<script src="plugins/jqvmap/maps/jquery.vmap.usa.js"></script>
<script src="plugins/jquery-knob/jquery.knob.min.js"></script>
<script src="plugins/moment/moment.min.js"></script>
<script src="plugins/daterangepicker/daterangepicker.js"></script>
<script src="plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>
<script src="plugins/summernote/summernote-bs4.min.js"></script>
<script src="plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js"></script>
<!-- AdminLTE App -->
<script src="dist/js/adminlte.js"></script>
