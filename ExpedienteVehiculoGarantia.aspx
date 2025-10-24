<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ExpedienteVehiculoGarantia.aspx.vb" Inherits="ExpedienteVehiculoGarantia" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Expediente Veh. Garantia</title>
    <link href="https://fonts.googleapis.com/css?family=Lato:300,400,700&display=swap" rel="stylesheet"/>
	<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css"/>
	<link rel="stylesheet" href="css/style.css"/>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous"/>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js"crossorigin="anonymous"></script>
    <style>
                .contenedor{
                    width: 90% !important;
                    height: 90% !important;
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
                          <a class="nav-link" href="MainDashBoard.aspx"><img src="Imagenes/mnegra.png" width="30" height="25" alt="Portal Armado de Motos"/></a>
                        </li>
        
                        <asp:Panel ID="pnlParametrizaciones" runat="server" Visible="True">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Parametrizaciones</a>
                              <ul class="dropdown-menu">
                                  <asp:Panel ID="pnlContratistas" runat="server" Visible="True"><li><a class="dropdown-item" href="Contratistas.aspx">Contratistas</a></li></asp:Panel>
                                  <asp:Panel ID="pnlTrabajoAD" runat="server" Visible="True"><li><a class="dropdown-item" href="TrabajosAd.aspx">Trabajos Adicionales</a></li></asp:Panel>
                                  <asp:Panel ID="pnlPreciosArmado" runat="server" Visible="True"><li><a class="dropdown-item" href="PrecioModelos.aspx">Precios Armado</a></li></asp:Panel>
                                  <asp:Panel ID="pnlUsuarios" runat="server" Visible="True"><li><a class="dropdown-item" href="Usuarios.aspx">Usuarios</a></li></asp:Panel>
                              </ul>
                        </asp:Panel>
        
                      <asp:Panel ID="pnlAsignaciones" runat="server" Visible="True">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Motos en Caja</a>
                              <ul class="dropdown-menu">
                                  <asp:Panel ID="pnlMotoEncaja" runat="server" Visible="True"><li><a class="dropdown-item" href="MotoenCaja.aspx">Asignar Moto</a></li></asp:Panel>
                                  <asp:Panel ID="PnlStockGlobal" runat="server" Visible="True"><li><a class="dropdown-item" href="StockPorModelos.aspx">Stock Por Modelo</a></li></asp:Panel>
                              </ul>
                        </asp:Panel>
        
                      <asp:Panel ID="pnlArmado" runat="server" Visible="True">
                            <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Armado</a>
                              <ul class="dropdown-menu">
                                  <!-- <asp:Panel ID="pnlArmadoMotos" runat="server" Visible="True"><li><a class="dropdown-item" href="ArmadoMotos.aspx">Armado Motos</a></li></asp:Panel> -->
                                  <asp:Panel ID="pnlListadoMotosProceso" runat="server" Visible="True"><li><a class="dropdown-item" href="ListadoProceso.aspx">Motos En Proceso</a></li></asp:Panel>
                              </ul>
                        </asp:Panel>
        
                        <asp:Panel ID="PnlControlCalidad" runat="server" Visible="True">
                          <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Control de Calidad</a>
                            <ul class="dropdown-menu">
                                <asp:Panel ID="pnlListadoMotosCalidad" runat="server" Visible="True"><li><a class="dropdown-item" href="ListadoCCalidad.aspx">Motos En Control de Calidad</a></li></asp:Panel>
                                <asp:Panel ID="pnoInformeProcesadasCC" runat="server" Visible="True"><li><a class="dropdown-item" href="InformeCC.aspx">Informe de Motos Procesadas CC</a></li></asp:Panel>
                            </ul>
                      </asp:Panel>
        
                    <asp:Panel ID="pnlProcesoContable" runat="server" Visible="True">
                          <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Proceso Contable</a>
                            <ul class="dropdown-menu">
                                <asp:Panel ID="pnoCrearLiquidacion" runat="server" Visible="True"><li><a class="dropdown-item" href="ProcesoContable.aspx">Crear Liquidacion</a></li></asp:Panel>
                                <asp:Panel ID="pnlCrearPO" runat="server" Visible="True"><li><a class="dropdown-item" href="GenerarPO.aspx">Crear Orden de Compra</a></li></asp:Panel>
                                <asp:Panel ID="pnlConsultaLiquidaciones" runat="server" Visible="True"><li><a class="dropdown-item" href="ProcesoContableConsulta.aspx">Consulta Liquidaciones Anteiores</a></li></asp:Panel>
                            </ul>
                      </asp:Panel>
                      
                    <asp:Panel ID="pnlDisponibles" runat="server" Visible="True">
                          <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Disponibles</a>
                            <ul class="dropdown-menu">
                              <asp:Panel ID="pnlDisponibleArmadoSAP" runat="server" Visible="True"><li><a class="dropdown-item" href="InformeMotosDisponibles.aspx">Motos Armadas Procesadas Portal</a></li></asp:Panel>
                              <asp:Panel ID="pnlExpedienteVeh" runat="server" Visible="True"><li><a class="dropdown-item" href="ExpedienteVehiculo.aspx">Expediente Vehiculo</a></li></asp:Panel>
                            </ul>
                      </asp:Panel>                            
          
                      <asp:Panel ID="pnlNoDisponible" runat="server" Visible="True">
                        <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">No Disponibles</a>
                          <ul class="dropdown-menu">
                            <asp:Panel ID="pnlInformeNoDisponible" runat="server" Visible="True"><li><a class="dropdown-item" href="InformeMotosNoDisponibles.aspx">Motos No Disponibles</a></li></asp:Panel>
                            <asp:Panel ID="pnlExpedienteGarantia" runat="server" Visible="True"><li><a class="dropdown-item" href="ExpedienteVehiculoGarantia.aspx">Expediente Vehiculo Garantia / ND</a></li></asp:Panel>
                            <asp:Panel ID="pnlRepuestoRetirado" runat="server" Visible="True"><li><a class="dropdown-item" href="RepuestosRetirados.aspx">Repuestos Retirados</a></li></asp:Panel>
                            <asp:Panel ID="pnlOCRProveedor" runat="server" Visible="True"><li><a class="dropdown-item" href="OCRProveedor.aspx">Orden de Compra Proveedor</a></li></asp:Panel>
                            <asp:Panel ID="pnlPedidoRepuestosFBack" runat="server" Visible="True"><li><a class="dropdown-item" href="PedidoRepuestosFeedback.aspx">Subir Informacion Compra Repuesto</a></li></asp:Panel>
                          </ul>
                    </asp:Panel>                            
        
                    <li class="nav-item">
                          <a class="nav-link " href="Default.aspx">Salir</a>
                        </li>
                  </ul>
                <h4 class="text-white">Bienvenido</h4>
                <span class="text-muted"><asp:Label ID="lblNombreUsuario" runat="server" Text="Usuario Conectado"></asp:Label></span>
              </div>
            </div>
            <nav class="navbar navbar-dark bg-dark">
              <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarToggleExternalContent" aria-controls="navbarToggleExternalContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
              </button>
            </nav>
        </div>
        <br />    

        <asp:Panel ID="pnlInformacionPrincipal" runat="server" Visible="true">
            
  <%--JUMBOTRON--%>
         <div class="jumbotron">
                <h1 class="display-4">Expediente Vehiculo Garantia</h1>
                <hr class="my-4">
                <p class="lead">
                </p>
         </div>  
          <center>
          <div class="contenedor">

                              <div class="row">
                                <div class="col">
                                    <asp:TextBox ID="txtSnMoto" runat="server" class="form-control" autocomplete="off" placeholder="Digite el Numero de Serie" ></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServiceMethod="SearchCustomers"
                                    MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
                                    TargetControlID="txtSnMoto" FirstRowSelected="false"></cc1:AutoCompleteExtender>
                                </div>
                                    <div class="col">
                                        <asp:Button ID="btnBuscarMoto" runat="server" class="form-control btn btn-primary rounded submit px-3" text="Buscar Serie"/>
		                            </div>
                                  <div class="col">
                                    <asp:TextBox ID="txtEstadpSN" runat="server" class="form-control" ReadOnly="true" placeholder="Estado de la Serie"></asp:TextBox>
                                  </div>
                                  <div class="col">
                                    <asp:TextBox ID="txtExpediente" runat="server" class="form-control" ReadOnly="true" placeholder="Expediente"></asp:TextBox>
                                </div>   
                                  <div class="col">
                                    <asp:TextBox ID="txtEstadoProduccion" runat="server" class="form-control" ReadOnly="true" placeholder="Estado del Vehiculo"></asp:TextBox>
                                </div>                             
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
                                    <label for="lblItemname">Descripcion Moto</label>
                                    <asp:TextBox ID="lblItemname" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
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
                              </div>
	        </div>

              </asp:Panel>
              <br />
                    <asp:panel id="pnlBotonesDeshabilitar" runat="server" visible="false">
                        <center>
                            <div class="contenedor">
                                <div class="row">
                                    <div class="col">
                                        <asp:Button ID="btnHabilitar" runat="server" class="form-control btn btn-success rounded submit px-3" text="Habilitar Para Venta"/>
                                    </div>
                                    <div class="col">
                                        <asp:Button ID="btnDesHailitar" runat="server" class="form-control btn btn-danger rounded submit px-3" text="Deshabilitar / Desarmar"/>
                                    </div>                                
                                    <div class="col">
                                        <asp:Button ID="btnExtraerRepuestos" runat="server" class="form-control btn btn-warning rounded submit px-3" text="Extraer Piezas"/>
                                    </div>
                                    <div class="col">
                                        <asp:Button ID="btnArmarRepuestos" runat="server" class="form-control btn btn-info rounded submit px-3" text="Armar Piezas"/>
                                    </div>
                                    <div class="col">
                                        <asp:Button ID="btnPedirSAP" runat="server" class="form-control btn btn-dark rounded submit px-3" text="Crear Pedido Piezas SAP"/>
                                    </div>
                                </div>
                            </div>
                        </center> 
                    </asp:panel>
            <br />
                <asp:Panel id="pnlRetirodePiezas" runat="server" visible="false">
                <center>
                    <h3>Retirar Piezas</h3>
                </center>
                <center>
                    <div class="contenedor">
               <div class="row">
                    <div class="col-3">
                       <asp:Panel ID="buscarItemcode" runat="server" DefaultButton="btnBuscarItemcode"><asp:TextBox ID="txtItemcode" runat="server" class="form-control" placeholder="Codigo del Repuesto" ></asp:TextBox>
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" ServiceMethod="SearchByItemCode"
                        MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
                        TargetControlID="txtItemcode" FirstRowSelected="false"></cc1:AutoCompleteExtender>
                           <asp:Button ID="btnBuscarItemcode" runat="server" class="form-control btn btn-primary rounded submit px-3" text="Buscar Por Codigo" Visible="true"/>
                       </asp:Panel>
                    </div>
                    <div class="col-5">
                        <asp:Panel ID="buscarItemname" runat="server" DefaultButton="btnBuscarItemname"><asp:TextBox ID="txtItemName" runat="server" class="form-control" placeholder="Descripcion del Repuesto" ></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" ServiceMethod="SearchByItemName"
                            MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
                            TargetControlID="txtItemName" FirstRowSelected="false"></cc1:AutoCompleteExtender>
                           <asp:Button ID="btnBuscarItemname" runat="server" class="form-control btn btn-primary rounded submit px-3" text="Buscar Por Descripcion" Visible="true"/>
                        </asp:Panel>
                    </div>
                    <div class="col-1">
                        <asp:TextBox ID="txtQtyRep" runat="server" class="form-control" placeholder="Cantidad" ></asp:TextBox>
                    </div>
                    <div class="col-3">
                        <asp:TextBox ID="txtSerieReemplazo" runat="server" class="form-control" autocomplete="off" placeholder="Serie Moto Destino" Visible="false" ></asp:TextBox>
                    </div>
                </div>
              <br />
                    <div class="row">
                        <div class="col-9">
                        <asp:TextBox ID="txtComentarios" runat="server" class="form-control" placeholder="Comentarios Sobre el Retiro de la Pieza" ></asp:TextBox>
                        </div>
                        <div class="col-3">
                            <asp:Button ID="btnAgregarRepuesto" runat="server" class="form-control btn btn-primary rounded submit px-3" text="Agregar Repuesto"/>
                        </div>
                    </div>
              <br />
                    </div>
                </center> 
              </asp:Panel>

        <asp:Panel ID="pnlArmadoPiezas" runat="server" Visible="false">
                    <center>
                    <div class="contenedor">

                    <div class="row">
                        <div class="col">
                            <asp:ListBox ID="lstArmarRepuestos" runat="server" SelectionMode="Multiple" class="demo" Height="400px" Width="600px"></asp:ListBox>
                            <link rel="stylesheet" type="text/css" href="dual-listbox.css" />
                            <script type="text/javascript" src="dual-listbox.js"></script>
                            <script type="text/javascript">
                                new DualListbox('.demo', {
                                addEvent: function (value) { },
                                removeEvent: function (value) { },
                                availableTitle: 'Repuestos Pendientes',
                                selectedTitle: 'Repuestos Entregados',
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
                        <div class="row">
                            <div class="col">
                                <asp:TextBox ID="txtCommentsRearmado" runat="server" class="form-control" placeholder="Comentarios Sobre el Rearmado" ></asp:TextBox>
                            </div>
                        </div>
                        <br />
            <div class="row">
                <div class="col">
                    <asp:Button ID="btnUpdateArmarRepuesto" runat="server" class="form-control btn btn-primary rounded submit px-3" text="Confirmar Re-Armado"/>
                </div>
            </div>    
                    </div>
                </center> 

        </asp:Panel>
              <asp:Panel ID="pnlGridRepuestos" runat="server" Visible="true">
                  <center>
                  <h5>Repuestos Retirados En Proceso</h5>  
                    <div class="contenedor">
                    <div class="col">
                             <asp:GridView ID="GridView1" runat="server" CssClass="display compact" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField DataField="DATECREATED" HeaderText="Fecha Retiro" />
                                <asp:BoundField DataField="ITEMCODE" HeaderText="Codigo" />
                                <asp:BoundField DataField="ITEMNAME" HeaderText="Descripcion" />
                                <asp:BoundField DataField="QTY" HeaderText="Cantidad" />
                                <asp:BoundField DataField="DCR" HeaderText="Stock DCR" />
                                <asp:BoundField DataField="SERIEORIGEN" HeaderText="Serie Moto" />
                                <asp:BoundField DataField="COMENTARIOS" HeaderText="Comentarios del Retiro" />
                            </Columns>
                        </asp:GridView>
                        </div>
                    </div>
                    </center>
            </asp:Panel>
            <asp:Panel ID="pnlRepuestosResueltos" runat="server" Visible="True">
                  <center>
                  <h5>Historial de Repuestos Resueltos</h5>  
                    <div class="contenedor">
                    <div class="col">
                             <asp:GridView ID="GridView3" runat="server" CssClass="display compact" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField DataField="DATEUPDATED" HeaderText="Fecha Resolucion" />
                                <asp:BoundField DataField="ITEMCODE" HeaderText="Codigo" />
                                <asp:BoundField DataField="ITEMNAME" HeaderText="Descripcion" />
                                <asp:BoundField DataField="QTY" HeaderText="Cantidad" />
                                <asp:BoundField DataField="SERIEORIGEN" HeaderText="Serie Moto" />
                                <asp:BoundField DataField="COMENTARIOSRETIRO" HeaderText="Comentarios del Retiro" />
                            </Columns>
                        </asp:GridView>
                        </div>
                    </div>
                    </center>
            </asp:Panel>

            <asp:Panel ID="pnlKardex" runat="server" Visible="false">
                <center>
                  <h5>Historial de Proceso Interno</h5>  
                    <div class="contenedor">
                    <div class="col">
                           
                               <asp:GridView ID="GridView2" runat="server" CssClass="display compact" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField DataField="MnfSerial" HeaderText="Serie" />
                                <asp:BoundField DataField="Quantity" HeaderText="Cantidad" />
                                <asp:BoundField DataField="Documento" HeaderText="Tipo Documento" />
                                <asp:BoundField DataField="Docnum" HeaderText="Documento" />
                                <asp:BoundField DataField="DocDate" HeaderText="Fecha" />
                                <asp:BoundField DataField="LocCode" HeaderText="Cod. Almacen" />
                                <asp:BoundField DataField="Almacen" HeaderText="Nom. Almacen / Comentarios Deshab." />
                            </Columns>
                        </asp:GridView>
                        </div>
                    </div>
                    </center>
            </asp:Panel>
            <br />
        <asp:Panel ID="pnlHabilitarDeshabilitar" runat="server" Visible="false">
            <center>
                <div class="contenedor">
                    <div class="row">
                        <div class="col-9">
                            <asp:TextBox ID="txtComentariosAccion" runat="server" class="form-control" placeholder="Comentarios Sobre La Accion" ></asp:TextBox>
                        </div>
                        <div class="col-3">
                            <asp:DropDownList ID="drpOrigenRetiro" runat="server" class="btn btn-secondary dropdown-toggle; text-left" AutoPostBack="true">
                                <asp:ListItem>Seleccione Origen del Retiro</asp:ListItem>
                                <asp:ListItem>GARANTIA</asp:ListItem>
                                <asp:ListItem>FABRICA</asp:ListItem>
                                <asp:ListItem>GOLPE TRANSPORTISTA</asp:ListItem>
                                <asp:ListItem>GOLPE DISTRIBUIDOR</asp:ListItem>
                                <asp:ListItem>GOLPE COLABORADOR</asp:ListItem>
                                <asp:ListItem>DAÑO TIENDA</asp:ListItem>
                                <asp:ListItem>RESTAURACION DE OTRA MOTO</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col">
                            <asp:Button ID="btnConfirmarAccion" runat="server" class="form-control btn btn-primary rounded submit px-3" text="Confirmar Accion"/>
                        </div>
                    </div>
                </div>
            </center>
        </asp:Panel>
        <br />
        <center>
            <div class="contenedor">
                <div class="row">
                    <div class="col">
                        <asp:Button ID="btnExit" runat="server" class="form-control btn btn-secondary rounded px-3" text="Limpiar Formulario"/>
                    </div>
                </div>
            </div>
        </center>
        <center>
            <div class="container">
                <footer class="main-footer">
                    <strong> <i class="ion-paintbrush"></i> WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
                </footer>
            </div>
        </center>
    
    <script src="js/jquery.min.js"></script>
    <script src="js/popper.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/main.js"></script>
    <asp:ScriptManager ID="sm1" runat="server"  EnablePageMethods="true"></asp:ScriptManager>
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
        $("[id*=GridView2]").DataTable(
        {
            bLengthChange: true,
            lengthMenu: [[10, -1], [10, "All"]],
            bFilter: true,
            bSort: false,
            bPaginate: true
        });
        $("[id*=GridView3]").DataTable(
        {
            bLengthChange: true,
            lengthMenu: [[10, -1], [10, "All"]],
            bFilter: true,
            bSort: false,
            bPaginate: true
        });
    });
</script>
