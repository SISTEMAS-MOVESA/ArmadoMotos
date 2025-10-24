<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ListadoCCalidad.aspx.vb" Inherits="ListadoCCalidad" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Hoja Control Calidad</title>
    <link href="https://fonts.googleapis.com/css?family=Lato:300,400,700&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="css/style.css" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css"/>
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
                                <li><a class="dropdown-item" href="ListadoCCalidad.aspx">Formulario Control Calidad Manual</a></li>
                                <li><a class="dropdown-item" href="InformeCCFinArmado.aspx">Moto Pendiente Control Calidad</a></li>
                                <li><a class="dropdown-item" href="InformeEnProcesoCalidad.aspx">Moto en Proceso de Calidad</a></li>
                                <li><a class="dropdown-item" href="InformeCC.aspx">Motos Control de Calidad Finalizado</a></li>
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
        <div class="container-fluid">
            <h1 class="text-center">Hoja Control Calidad</h1>
            <hr class="my-4" />
        </div>
        <asp:Panel ID="pnlGridControlCalidad" runat="server" Visible="true">
            <div class="container-fluid">
                <div class="row">
                    <div class="col">
                        <asp:TextBox ID="txtSnMoto" runat="server" class="form-control" autocomplete="off" placeholder="Digite el Numero de Serie"></asp:TextBox>
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServiceMethod="SearchCustomers"
                            MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
                            TargetControlID="txtSnMoto" FirstRowSelected="false">
                        </cc1:AutoCompleteExtender>
                    </div>
                    <div class="col">
                        <asp:Button ID="btnBuscarMoto" runat="server" class="form-control btn btn-primary rounded submit px-3" Text="Buscar Serie" />
                    </div>
                    <br />
                </div>
                <div class="row">
                    <div class="col">
                        <label for="lblItemcode">Articulo</label>
                        <asp:TextBox ID="lblItemcode" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="col">
                        <label for="lblModelo">Modelo</label>
                        <asp:TextBox ID="lblModelo" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="col">
                        <label for="lblYear">Año</label>
                        <asp:TextBox ID="lblYear" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col">
                        <label for="lblItemname">Descripcion Moto</label>
                        <asp:TextBox ID="lblItemname" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col">
                        <label for="lblMarca">Marca</label>
                        <asp:TextBox ID="lblMarca" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="col">
                        <label for="lblCilindros">Cilindros</label>
                        <asp:TextBox ID="lblCilindros" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="col">
                        <label for="lblColor">Color</label>
                        <asp:TextBox ID="lblColor" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col">
                        <label for="lblSerie">Serie</label>
                        <asp:TextBox ID="lblSerie" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="col">
                        <label for="lblMotor">Serie Motor</label>
                        <asp:TextBox ID="lblSerieMotor" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="col">
                        <label for="lblMotorM">Motor</label>
                        <asp:TextBox ID="lblMotorM" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <br />
                <asp:Panel ID="pnlBotonesCalidad" runat="server" Visible="false">
                    <div class="row">
                        <div class="col">
                            <asp:Button ID="btnSi" runat="server" class="form-control btn btn-success rounded submit px-3" Text="Pasa Inspeccion" />
                        </div>
                        <div class="col">
                            <asp:Button ID="btnNo" runat="server" class="form-control btn btn-danger rounded submit px-3" Text="Reproceso" />
                        </div>
                        <div class="col">
                            <asp:Button ID="btnDF" runat="server" class="form-control btn btn-warning rounded submit px-3" Text="Detalles" />
                        </div>
                    </div>
                </asp:Panel>
                <br />
                <div class="row">
                    <div class="col">
                        <asp:Button ID="btnExit" runat="server" class="form-control btn btn-danger rounded px-3" Text="Limpiar Formulario" />
                    </div>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlUpdate" runat="server" Visible="false">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                            <label for="txtUpdateIdArmado">Id Armado</label>
                            <asp:TextBox ID="txtUpdateIdArmado" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                            <label for="txtUpdateSerial">Serie de Moto</label>
                            <asp:TextBox ID="txtUpdateSerial" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                            <label for="txtUpdateDefecto">Tipo de Problema</label>
                            <asp:TextBox ID="txtUpdateDefecto" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                            <label for="txtUpdateUserName">Mecanico Asignado</label>
                            <asp:TextBox ID="txtUpdateMecanicoCode" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-12">
                            <label for="txtUpdateCommentsProblema">Comentarios Sobre El Proceso</label>
                            <asp:TextBox ID="txtUpdateCommentsProblema" runat="server" class="form-control" autocoplete="off" TextMode="MultiLine" Height="200px"></asp:TextBox>
                        </div>
                        </div>
                    <div class="d-flex" style="justify-content: center; align-items: center;">
                        <div class="row">
                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                <label for="drpCalidadUsers">Supervisor Control Calidad</label>
                                <br />
                                <asp:DropDownList ID="drpCalidadUsers" runat="server" class="btn btn-secondary dropdown-toggle; text-left"></asp:DropDownList>
                            </div>
                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                <label for="drpMecanicos">Origen del Reproceso</label>
                                <br />
                                <asp:DropDownList ID="drpUpdateTipoProblema" runat="server" class="btn btn-primary dropdown-toggle; text-left">
                                    <asp:ListItem Selected="True" Value="Seleccione">Seleccione</asp:ListItem>
                                    <asp:ListItem Value="0">Pieza Faltante Origen</asp:ListItem>
                                    <asp:ListItem Value="1">Error Mecanico</asp:ListItem>
                                    <asp:ListItem Value="2">Desarmada por Garantia</asp:ListItem>
                                    <asp:ListItem Value="3">Trabajos de Pintura</asp:ListItem>
                                    <asp:ListItem Value="4">Tornillo o Tuerca Quebrada</asp:ListItem>
                                    <asp:ListItem Value="5">Falla Eléctrica</asp:ListItem>
                                    <asp:ListItem Value="6">Falla en Motor</asp:ListItem>
                                    <asp:ListItem Value="7">Daño de Fabrica u Origen</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <br />
                    <asp:Panel ID="pnlPintura" runat="server" Visible="false">
                        <div class="d-flex" style="justify-content:center; align-items: center;">
                            <div class="row">
                                <div class="col">
                                    <asp:ListBox ID="lstTrabajosAdicionales" runat="server" SelectionMode="Multiple" class="demo" Height="400px" Width="600px"></asp:ListBox>
                                    <link rel="stylesheet" type="text/css" href="dual-listbox.css" />
                                    <script type="text/javascript" src="dual-listbox.js"></script>
                                    <script type="text/javascript">
                                        new DualListbox('.demo', {
                                            addEvent: function (value) { },
                                            removeEvent: function (value) { },
                                            availableTitle: 'Trabajos Disponibles',
                                            selectedTitle: 'Trabajos Seleccionados',
                                            addButtonText: 'Agregar (>)',
                                            removeButtonText: 'Quitar (<)',
                                            addAllButtonText: 'Agregar Todas (>>)',
                                            removeAllButtonText: 'Quitar Todas (<<)'
                                        });
                                    </script>
                                    <style type="text/css">
                                        .dual-listbox .dual-listbox__button {
                                            margin-bottom: 5px;
                                            border: 0;
                                            background-color: #0090CB !important;
                                            padding: 10px;
                                            color: #fff;
                                        }
                                    </style>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="problemaOdefecto" runat="server">
                        <br />
                        <div class="row">
                            <div class="col">
                               
                            </div>
                        </div>
                        <br />
                        <br />
                    </asp:Panel>
                    <div class="row">
                        <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                            <asp:Button ID="btnUpdate" runat="server" class="form-control btn btn-primary rounded submit px-3" Text="Actualizar" />
                        </div>
                        <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                            <asp:Button ID="btnCancel" runat="server" class="form-control btn btn-secondary rounded px-3" Text="Regresar" />
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
