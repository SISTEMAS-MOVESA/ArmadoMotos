<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CreateCustomer.aspx.vb" Inherits="CreateCustomer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Crear Clientes</title>
	
    <link href="https://fonts.googleapis.com/css?family=Lato:300,400,700&display=swap" rel="stylesheet"/>
	<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css"/>
	<link rel="stylesheet" href="css/style.css"/>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous"/>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js"crossorigin="anonymous"></script>

</head>
<body>
    <form id="form1" runat="server">
          <ul class="nav nav-tabs">
             <li class="nav-item">
                <a class="nav-link" href="#"><img src="Imagenes/logoskg.jpg" width="30" height="30" alt=""/></a>
              </li>
            	<li class="nav-item">
                    <a class="nav-link " href="Menu.aspx">Menu Principal</a>
                </li>

              <asp:Panel ID="pnlMenuTarea" runat="server" Visible="True">
                  <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Tareas</a>
                    <ul class="dropdown-menu">
                        <asp:Panel ID="pnlSubMenuTareas" runat="server" Visible="True"><li><a class="dropdown-item" href="Task.aspx">Tareas Pendientes</a></li></asp:Panel>
                    </ul>
              </asp:Panel>

              <asp:Panel ID="pnlMenuCliente" runat="server" Visible="false">
                <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Clientes </a>
		            <ul class="dropdown-menu">
			          <asp:Panel ID="pnlSubMenuCrearClientes" runat="server" Visible="True"><li><a class="dropdown-item" href="CreateCustomer.aspx">Crear Clientes</a></li></asp:Panel>
		            </ul>
              </asp:Panel>

              <asp:Panel ID="pnlMenuDacion" runat="server" Visible="True">
                    <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">  Dacion en Pago </a>
		                <ul class="dropdown-menu">
			                <asp:Panel ID="pnlSubMenuOrdenRecuperacion" runat="server" Visible="True"><li><a class="dropdown-item" href="GoodsRcpt.aspx">Orden Recuperacion</a></li></asp:Panel>
			                <asp:Panel ID="pnlSubMenuConfirmacion" runat="server" Visible="True"><li><a class="dropdown-item" href="ConfirmacionDacion.aspx">Ingreso Inventario</a></li></asp:Panel>
                      <asp:Panel ID="pnlSubMenuConfirmacionVenta" runat="server" Visible="True"><li><a class="dropdown-item" href="ConfirmacionVenta.aspx">Ficha Para Venta</a></li></asp:Panel>
                      <asp:Panel ID="pnlSubMenuPrecios" runat="server" Visible="True"><li><a class="dropdown-item" href="Precios.aspx">Actualizacion de Precios </a></li></asp:Panel>
			                <!-- <asp:Panel ID="pnlSubMenuReconciliaciones" runat="server" Visible="True"><li><a class="dropdown-item" href="#">Reconciliaciones </a></li></asp:Panel> -->
		                </ul>
              </asp:Panel>

              <asp:Panel ID="pnlMenuInventario" runat="server" Visible="True">
            	<a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Inventarios </a>
		        <ul class="dropdown-menu">
			        <asp:Panel ID="pnlSubMenuInventario" runat="server" Visible="True"><li><a class="dropdown-item" href="ListadoMotos.aspx">Inventario Motos Disponibles</a></li></asp:Panel>
			        <!-- <asp:Panel ID="pnlSubMenuExpediente" runat="server" Visible="True"><li><a class="dropdown-item" href="ExpedienteVehiculo.aspx"> Expediente Vehiculo</a></li></asp:Panel> -->
		        </ul>
              </asp:Panel>
              
              <li class="nav-item">
                <a class="nav-link " href="Default.aspx">Salir</a>
              </li>              
              <li class="nav-item">
                <a class="nav-link "><asp:Label ID="lblNombreUsuario" runat="server" Text="Usuario Conectado:"></asp:Label></a>
              </li>              
        </ul>
             
  <section class="ftco-section">
		<div class="container">
			<div class="row justify-content-center">
                <style>
                .contenedor{
                    width: 100% !important;
                    height: 100% !important;
                }

                </style>
				<div class="contenedor">
					<div class="wrap">
						<div class="login-wrap p-4 p-md-5">
			      		<div class="form-group mt-3">

                                <label for="txtCodigoSKG">Codigo SKG Asignado</label>
                              <asp:TextBox ID="txtCodigoSKG" runat="server" class="form-control"></asp:TextBox>
			      		</div>
		            <div class="form-group">
		            </div>
		            <div class="form-group">
                        <asp:Button ID="Button1" runat="server" class="form-control btn btn-primary rounded submit px-3" text="Buscar Cliente"/>
		            </div>
                              <div class="row">
                                <div class="col">
                                    <label for="txtNcliente">Nombre Cliente</label>
                                    <asp:TextBox ID="txtNcliente" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                              </div>
                              <div class="row">
                                <div class="col">
                                    <label for="txtIdentidad">Identidad</label>
                                    <asp:TextBox ID="txtIdentidad" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <label for="txtRTN">RTN</label>
                                    <asp:TextBox ID="txtRTN" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <label for="drpSucursal">Sucursal</label>
                                    <br />
                                    <asp:DropDownList ID="drpSucursal" runat="server" class="btn btn-secondary dropdown-toggle; text-left"></asp:DropDownList>
                                </div>
                              </div>
                              <div class="row">
                                <div class="col">
                                    <label for="txtTelefono1">Telefono 1</label>
                                    <asp:TextBox ID="txtTelefono1" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <label for="txtTelefono2">Telefono 2</label>
                                    <asp:TextBox ID="txtTelefono2" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <label for="txtCelular">Celular</label>
                                    <asp:TextBox ID="txtCelular" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                              </div>
                            <div class="row">
                                <div class="col">
                                    <label for="drpGrupoClientes">Grupo de Cliente</label>
                                    <br />
                                    <asp:DropDownList ID="drpGrupoClientes" runat="server" class="btn btn-secondary dropdown-toggle; text-left"></asp:DropDownList>
                                </div>
                                <div class="col">
                                    <label for="drpDepartamento">Departamento</label>
                                    <br />
                                    <asp:DropDownList ID="drpDepartamento" runat="server" class="btn btn-secondary dropdown-toggle; text-left"></asp:DropDownList>
                                </div>
                                <div class="col">
                                    <label for="drpMunicipio">Municipio</label>
                                    <br />
                                    <asp:DropDownList ID="drpMunicipio" runat="server" class="btn btn-secondary dropdown-toggle; text-left"></asp:DropDownList>
                                </div>
                              </div>
                              <div class="row">
                                <div class="col">
                                    <label for="txtColonia">Colonia</label>
                                    <asp:TextBox ID="txtColonia" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <label for="txtCalle">Calle/Avenida</label>
                                    <asp:TextBox ID="txtCalle" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <label for="txtCasa">Numero Casa</label>
                                    <asp:TextBox ID="txtCasa" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                              </div>
                         <div class="row">
                                <div class="col">
                                    <label for="txtCorreo">Correo Electronico</label>
                                    <asp:TextBox ID="txtCorreo" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <label for="txtCreadoPor">Creado Por</label>
                                    <asp:TextBox ID="txtCreadoPor" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <label for="txtCasa">Codigo Vendedor</label>
                                    <br />
                                    <asp:DropDownList ID="drpVendedores" runat="server" class="btn btn-secondary dropdown-toggle; text-left"></asp:DropDownList>
                                </div>
                              </div>
                                 <br />
                                <div class="form-group">
                                <asp:Button ID="btn3" runat="server" class="form-control btn btn-primary rounded submit px-3"
                                text="Crear Cliente"/>
                                </div>
                                <div class="row">
                                <div class="col">
                                <asp:Button ID="btnExit" runat="server" class="form-control btn btn-secondary rounded px-3" text="Limpiar Formulario"/>
                                </div>
                              </div>
                    <div class="form-group">
                    </div>
		      </div>
				</div>
			</div>
		</div>
                </div>
	</section>


    <script src="js/jquery.min.js"></script>
    <script src="js/popper.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/main.js"></script>
    <div>
  <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>  
    </div>
    </form>
</body>
</html>
