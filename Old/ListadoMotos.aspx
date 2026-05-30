<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ListadoMotos.aspx.vb" Inherits="ListadoMotos" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Motos Disponibles</title>
    <link href="https://fonts.googleapis.com/css?family=Lato:300,400,700&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="css/style.css" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>

    <style type="text/css">
        body {
            font-family: Arial;
            font-size: 10pt;
        }

        table {
            border: 1px solid #ccc;
            border-collapse: collapse;
        }

            table th {
                background-color: #F7F7F7;
                color: #333;
                font-weight: bold;
            }

            table th, table td {
                padding: 5px;
                border: 1px solid #ccc;
            }

            table img {
                height: 150px;
                width: 150px;
                cursor: pointer;
            }

        #dialog img {
            height: 530px;
            width: 560px;
            cursor: pointer;
        }
    </style>

    <style type="text/css">
        .modal {
            position: fixed;
            top: 0;
            left: 0;
            background-color: black;
            z-index: 99;
            opacity: 0.8;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }

        .loading {
            font-family: Arial;
            font-size: 10pt;
            border: 5px solid #67CFF5;
            width: 200px;
            height: 100px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
        }
    </style>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        function ShowProgress() {
            setTimeout(function () {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 200);
        }
        $('form').live("submit", function () {
            ShowProgress();
        });

    </script>
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

        <asp:GridView ID="gvImages" runat="server" AutoGenerateColumns="False" OnRowDataBound="OnRowDataBound">
            <Columns>
                 <asp:ButtonField ButtonType="Image" ImageUrl="~/Imagenes/add.png" Text="Agregar" CommandName="Agregar" >
                                <ControlStyle Height="18px" Width="18px" />
                                <ItemStyle Wrap="False" />
                            </asp:ButtonField>
                <asp:BoundField DataField="ITEMCODE" HeaderText="Codigo" />
                <asp:BoundField DataField="ITEMNAME" HeaderText="Descripcion" />
                <asp:BoundField DataField="SuppSerial" HeaderText="Serie" />
                <asp:BoundField DataField="WhsCode" HeaderText="Almacen" />
                <asp:BoundField DataField="U_SCOLOR" HeaderText="Color" />
                <asp:BoundField DataField="U_SAno" HeaderText="Año" />
                <asp:BoundField DataField="U_PrecioMat" HeaderText="Matricula" />
                <asp:BoundField DataField="U_PrecioVenta" HeaderText="PVenta" />
            </Columns>
        </asp:GridView>
            <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
            <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.24/themes/start/jquery-ui.css" />
            <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.24/jquery-ui.min.js"></script>
            <script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
            <link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />
            <script type="text/javascript">
                $(function () {
                    $("#dialog").dialog({
                        autoOpen: false,
                        modal: true,
                        height: 600,
                        width: 600,
                        title: "Zoomed Image"
                    });
                    $("[id*=gvImages] img").click(function () {
                        $('#dialog').html('');
                        $('#dialog').append($(this).clone());
                        $('#dialog').dialog('open');
                    });
                    $("[id*=gvImages]").DataTable({
                        bLengthChange: true,
                        lengthMenu: [[100, -1], [100, "All"]],
                        bFilter: true,
                        bSort: true,
                        bPaginate: true,
                        order: [[4, "desc"]],
                    });
                });
            </script>
            <div>
                <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>

            </div>
    </form>
</body>
</html>
