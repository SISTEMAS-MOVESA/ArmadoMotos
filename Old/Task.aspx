<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Task.aspx.vb" Inherits="Task" EnableEventValidation = "false" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tareas Pendientes</title>
    <link href="https://fonts.googleapis.com/css?family=Lato:300,400,700&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="css/style.css" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>


</head>
<body>
    <form id="form1" runat="server">
      
        <asp:Panel ID="pnlGrid" runat="server">
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
            <br />
            <asp:Button ID="btnIngreso" runat="server" class="btn btn-success rounded" text="Exportar a Excel"/>
            <br />
            <br />
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                OnPageIndexChanging="OnPageIndexChanging" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical">
                <AlternatingRowStyle BackColor="#CCCCCC" />
                <Columns>
                    <asp:ButtonField ButtonType="Image" ImageUrl="~/Imagenes/add.png"
                        Text="Agregar" CommandName="Agregar">
                        <ControlStyle Height="18px" Width="18px" />
                        <ItemStyle Wrap="False" />
                    </asp:ButtonField>
                    <asp:ButtonField ButtonType="Image" ImageUrl="~/Imagenes/edit.png"
                        Text="Modificar" CommandName="Modificar">
                        <ControlStyle Height="18px" Width="18px" />
                        <ItemStyle Wrap="False" />
                    </asp:ButtonField>
                    <asp:BoundField DataField="ID" HeaderText="ID" />
                    <asp:BoundField DataField="DOCNUM" HeaderText="Documento" />
                    <asp:BoundField DataField="DIAS" HeaderText="Dias" />
                    <asp:BoundField DataField="CARDNAME" HeaderText="Nombre Cliente" />
                    <asp:BoundField DataField="IDENTIDAD" HeaderText="Identidad" />
                    <asp:BoundField DataField="MODELO" HeaderText="Modelo" />
                    <asp:BoundField DataField="COLOR" HeaderText="Color" />
                    <asp:BoundField DataField="SERIE" HeaderText="Serie" />
                    <asp:BoundField DataField="SERIEM" HeaderText="Serie Motor" />
                    <asp:BoundField DataField="GESTOR" HeaderText="Gestor" />

                </Columns>
             
            </asp:GridView>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlUpdate" runat="server" Visible="false">
            <center>
              <style>
                  .contenedor {
                      width: 25% !important;
                      height: 25% !important;
                  }
              </style>
        <div class="contenedor">
            <div class="login-wrap p-4 p-md-5">

            <div class="form-group">
                        <div class="row">
                            <div class="col" style="text-align: center;">
                                <label for="txtOrder">Numero de Orden</label>
                                <asp:TextBox ID="txtOrder" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>                            
                        <div class="row">
                            <div class="col">
                                <label for="drpOpciones">Modificar Estatus Orden</label>
                                <br />
                                <asp:DropDownList ID="drpOpciones" runat="server" class="btn btn-secondary dropdown-toggle; text-left" >
                                            <asp:ListItem Selected="True">Seleccione</asp:ListItem>
                                            <asp:ListItem Value="Cancelar">CANCELAR</asp:ListItem>
                                        </asp:DropDownList>
                            </div>
                        </div>
                 <div class="row">
                            <div class="col">
                                <label for="txtValorMatricula">Modificar Valor Matricula</label>
                                <asp:TextBox ID="txtValorMatricula" runat="server" class="form-control" textmode="Number"></asp:TextBox>
                                
                            </div>
                        </div>
                 <br />

                        <div class="row">
                            <div class="col">
                                <asp:Button ID="btnModificarVMatricula" runat="server" class="form-control btn btn-primary rounded submit px-3" text="Actualizar Valor Matricula"/>
                            </div>
                        </div>
                <br /><br />

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
            </div>
        </center>
        </asp:Panel>

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
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
<link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    $(function () {
        $("[id*=GridView1]").DataTable(
        {
            bLengthChange: true,
            lengthMenu: [[100, -1], [100, "All"]],
            bFilter: true,
            bSort: true,
            bPaginate: true,
            order: [[4, "desc"]],
        });
    });
</script>
