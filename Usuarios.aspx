<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Usuarios.aspx.vb" Inherits="Usuarios" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Parametrizaciones || Usuarios</title>

    <link href="https://fonts.googleapis.com/css?family=Lato:300,400,700&display=swap" rel="stylesheet"/>
	<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css"/>
	<link rel="stylesheet" href="css/style.css"/>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous"/>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js"crossorigin="anonymous"></script>
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
            <div class="jumbotron">
                <h1 class="display-4">Gestion de Usuarios</h1>
                <hr class="my-4">
                <p class="lead">
                        <asp:Button ID="btnIngreso" runat="server" class="btn btn-success rounded" text="Crear Usuario"/>
                </p>
            </div>  
        <asp:Panel ID="pnlGridUsuarios" runat="server" Visible="true">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" Style="width: 100%">
                            <HeaderStyle CssClass="thead-dark" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="~/Imagenes/edit.png" Text="Modificar" CommandName="Modificar"> <ControlStyle Height="18px" Width="18px" /><ItemStyle Wrap="False" /> </asp:ButtonField>
                                <asp:BoundField DataField="ID" HeaderText="ID" />
                                <asp:BoundField DataField="USERCODE" HeaderText="Codigo" />
                                <asp:BoundField DataField="USERNAME" HeaderText="Nombre Usuario" />
                                <asp:BoundField DataField="USEREMAIL" HeaderText="Correo" />
                                <asp:BoundField DataField="USERROL" HeaderText="Rol" />
                                <asp:BoundField DataField="USERESTATUS" HeaderText="Activo" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                </div>
        </asp:Panel>
        <asp:Panel ID="pnlCrearUsuario" runat="server" Visible="false">
            <center>
                        <style>
                        .contenedor{
                        width: 25% !important;
                        height: 25% !important;
                        }

                        </style>
                        <div class="contenedor">
                                    <div class="row">
                                        <div class="col">
                                            <label for="txtUserCode">Codigo de Usuario</label>
                                            <asp:TextBox ID="txtUserCode" runat="server" class="form-control" autocoplete="off" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col">
                                            <label for="txtUserPass">Clave</label>
                                            <asp:TextBox ID="txtUserPass" runat="server" class="form-control" autocoplete="off" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col">
                                            <label for="txtUserName">Nombre de Usuario</label>
                                            <asp:TextBox ID="txtUserName" runat="server" class="form-control" autocoplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col">
                                            <label for="txtEmail">Correo de Usuario</label>
                                            <asp:TextBox ID="txtEmail" runat="server" class="form-control" autocoplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col">
                                            <label for="drpRolUsuario">Rol de Usuario</label>
                                            <br />
                                            <asp:DropDownList ID="drpRolUsuario" runat="server" class="btn btn-secondary dropdown-toggle; text-left" >
                                                <asp:ListItem Selected="True">Seleccione Rol</asp:ListItem>
                                                <asp:ListItem Value="ADMIN">Administrador</asp:ListItem>
                                                <asp:ListItem Value="ASIGNA">Asignador</asp:ListItem>
                                                <asp:ListItem Value="EQUIPO">Jefe Equipo Mecanicos</asp:ListItem>
                                                <asp:ListItem Value="MECANICO">Mecanico</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                            <div class="row">
                                <div class="col">
                                    <label for="drpRolUsuario">Sucursal</label>
                                    <br />
                                    <asp:DropDownList ID="drpSucursales" runat="server" class="btn btn-secondary dropdown-toggle; text-left" >
                                    </asp:DropDownList>
                                </div>
                            </div>
                                    <br />
                                    <div class="row">
                                            <div class="col">
                                            <asp:Button ID="Button1" runat="server" class="form-control btn btn-primary rounded submit px-3" text="Crear Usuario"/>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col">
                                        <asp:Button     ID="btnExit" runat="server" class="form-control btn btn-secondary rounded px-3" text="Limpiar Formulario"/>
                                        </div>
                                    </div>
                                </div>
                </center>
        </asp:Panel>

             <asp:Panel ID="pnlUpdate" runat="server" Visible="false">
                <center>
                <style>
                .contenedor{
                    width: 25% !important;
                    height: 25% !important;
                }
                </style>
                <div class="contenedor" >
                    <div class="form-group">
                                    <div class="row">
                                        <div class="col">
                                            <label for="txtUpdateUserID">ID de Usuario</label>
                                            <asp:TextBox ID="txtUpdateUserID" runat="server" class="form-control" ReadOnly="true" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col">
                                            <label for="txtUpdateUserCode">Codigo de Usuario</label>
                                            <asp:TextBox ID="txtUpdateUserCode" runat="server" class="form-control" ReadOnly="true" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col">
                                            <label for="txtUpdateUserPass">Clave Usuario</label>
                                            <asp:TextBox ID="txtUpdateUserPass" runat="server" class="form-control" autocoplete="off" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col">
                                            <label for="txtUpdateUserName">Nombre de Usuario</label>
                                            <asp:TextBox ID="txtUpdateUserName" runat="server" class="form-control" autocoplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col">
                                            <label for="txtUpdateUSerEmail">Correo de Usuario</label>
                                            <asp:TextBox ID="txtUpdateUSerEmail" runat="server" class="form-control" autocoplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                        <br />
                        <div class="row">
                            <div class="col">
                                <asp:Button ID="btnUpdate" runat="server" class="form-control btn btn-primary rounded submit px-3" text="Actualizar"/>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col">
                                <asp:Button ID="btnCancel" runat="server" class="form-control btn btn-secondary rounded px-3" text="Regresar"/>
                            </div>
                        </div>
		            </div>
                </div>
                    </center>
        </asp:Panel>
        <br />
        <br>
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

    new DataTable('#GridView1', {
        fixedHeader: {
            header: true,
            footer: true
        },
        "sort": false,
        paging: false,
        scrollCollapse: true,
        scrollX: true,
        scrollY: 500
    });

</script>