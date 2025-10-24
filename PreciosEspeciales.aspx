<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PreciosEspeciales.aspx.vb" Inherits="PreciosEspeciales" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Repuestos Actualizar Info Compra</title>
    <link href="https://fonts.googleapis.com/css?family=Lato:300,400,700&display=swap" rel="stylesheet"/>
	<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css"/>
	<link rel="stylesheet" href="css/style.css"/>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous"/>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js"crossorigin="anonymous"></script>

        <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
        .Grid td
        {
            background-color: #A1DCF2;
            color: black;
            font-size: 10pt;
            line-height:200%
        }
        .Grid th
        {
            background-color: #3AC0F2;
            color: White;
            font-size: 10pt;
            line-height:200%
        }
        .ChildGrid td
        {
            background-color: #eee !important;
            color: black;
            font-size: 10pt;
            line-height:200%
        }
        .ChildGrid th
        {
            background-color: #6C6C6C !important;
            color: White;
            font-size: 10pt;
            line-height:200%
        }
    </style>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "Imagenes/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "Imagenes/plus.png");
            $(this).closest("tr").next().remove();
        });
    </script>

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
                                <asp:Panel ID="pnlCrearLiquidacion" runat="server" Visible="True"><li><a class="dropdown-item" href="ProcesoContable.aspx">Crear Liquidacion</a></li></asp:Panel>
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
    <%--JUMBOTRON--%>
         <div class="jumbotron">
                <h1 class="display-4">Resumen Repuestos a Comprar</h1>
                <p class="lead">
                    <div>
                      <%--  <span class="btn btn-default btn-file"><asp:FileUpload ID="FileUpload1" runat="server" /></span>
                        <asp:Button ID="btnImport" runat="server" Text="Importar" OnClick="ImportExcel" class="btn btn-success rounded"/>
                        <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" class="btn btn-success rounded"/>--%>
                    </div>
                </p>
         </div>

        <div class="d-flex" style="margin-bottom: 15px;">
            <div class="col-6">
                <h1>Header OSPP</h1>
                <span class="btn btn-default btn-file">
                    <asp:FileUpload ID="FU_OSPP" runat="server" />
                </span>
                <asp:Button ID="Button1" runat="server" Text="Importar" OnClick="ImportExcel_ospp" class="btn btn-success rounded" />
            </div>
            <div class="col-6">
                <h1>Lines SPP1</h1>
                <span class="btn btn-default btn-file">
                    <asp:FileUpload ID="FU_SPP1" runat="server" />
                </span>
                <asp:Button ID="Button3" runat="server" Text="Importar" OnClick="ImportExcel_spp1" class="btn btn-success rounded" />
            </div>
        </div>
        <div class="d-flex" style="margin-bottom: 15px;">
            <div class="col-2">
                <label for="txtDate_from">Fecha Inicio</label>
                <asp:TextBox type="date" ID="txtDate_from" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-2">
                <label for="txtDate_to">Fecha Fin</label>
                <asp:TextBox type="date" ID="txtDate_to" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
         <div class="d-flex" style="margin-bottom: 15px;">
            <div class="col-6">
                <asp:Button ID="btnEnviaraSAP" runat="server" Text="Enviar a SAP" OnClick="CreateUpdateSpecialPriceList" class="btn btn-success rounded" />
                </div>
             </div>
        <hr />
        <div class="d-flex">
            <div class="col-6">
                <asp:Panel ID="pnlGridResumenRepuestos" runat="server" Visible="true">
                <div class="container-fluid">
                    <div class="col">
                        <asp:GridView ID="grid_ospp" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical">
                            <AlternatingRowStyle BackColor="#CCCCCC" />
                            <FooterStyle BackColor="#CCCCCC" />
                            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#808080" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#383838" />
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>   
                </div>
            <div class="col-6">
                <asp:Panel ID="Panel1" runat="server" Visible="true">
                <div class="container-fluid">
                    <div class="col">
                        <asp:GridView ID="grid_spp1" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical">
                            <AlternatingRowStyle BackColor="#CCCCCC" />
                            <FooterStyle BackColor="#CCCCCC" />
                            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#808080" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#383838" />
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>   
                </div>
            </div>


                
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
<%--<script type="text/javascript">
    $(function () {
        $("[id*=GridView1]").DataTable(
        {
            bLengthChange: true,
            lengthMenu: [[20, -1], [20, "All"]],
            order: [[1, "asc"]],
            bFilter: true,
            bSort: true,
            bPaginate: true
        });
    });
</script>--%>
