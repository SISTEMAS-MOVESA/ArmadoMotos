<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ExpedienteVehiculo.aspx.vb" Inherits="ExpedienteVehiculo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
      <title>Ficha Vehiculo</title>
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
                              <label for="txtSnMoto">Serie</label>
                              <asp:TextBox ID="txtSnMoto" runat="server" class="form-control" autocomplete="off"></asp:TextBox>
                            <cc2:AutoCompleteExtender ServiceMethod="SearchCustomersByCode" MinimumPrefixLength="2" 
                            CompletionInterval="100" EnableCaching="false" CompletionSetCount="10" TargetControlID="txtSnMoto"
                            ID="AutoCompleteExtender4" runat="server" FirstRowSelected = "false"></cc2:AutoCompleteExtender>

			      		</div>
		            <div class="form-group">
		            </div>
		            <div class="form-group">
                        <asp:Button ID="btnBuscarMoto" runat="server" class="form-control btn btn-primary rounded submit px-3"
                            text="Buscar Serie"/>
		            </div>
                              <div class="row">
                                <div class="col">
                                    <label for="lblCardcode">Codigo Cliente</label>
                                    <asp:TextBox ID="lblCardcode" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <label for="lblDocnum">Factura</label>
                                    <asp:TextBox ID="lblDocnum" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                  <div class="col">
                                    <label for="lblDesembolso"># Desembolso</label>
                                    <asp:TextBox ID="lblDesembolso" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                              </div>
                            <div class="row">
                                <div class="col">
                                    <label for="lblDocdate">Fecha Factura</label>
                                    <asp:TextBox ID="lblDocdate" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <label for="lblNumCAI">Factura CAI</label>
                                    <asp:TextBox ID="lblNumCAI" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                              </div>
                              <div class="row">
                                <div class="col">
                                    <label for="lblCardname">Nombre Cliente</label>
                                    <asp:TextBox ID="lblCardname" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                              </div>
                              <div class="row">
                                <div class="col">
                                    <label for="lblIdentidad">Identidad</label>
                                    <asp:TextBox ID="lblIdentidad" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <label for="lblRTN">RTN</label>
                                    <asp:TextBox ID="lblRTN" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                   <div class="col">
                                    <label for="lblMotorM">Motor</label>
                                    <asp:TextBox ID="lblMotorM" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
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
                                    <label for="lblAduana">Aduana</label>
                                    <asp:TextBox ID="lblAduana" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <label for="lblPoliza">Poliza</label>
                                    <asp:TextBox ID="lblPoliza" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <label for="LblItem">Item</label>
                                    <asp:TextBox ID="LblItem" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                              </div>
                            <div class="row">
                                <div class="col">
                                    <label for="lblSerie">Serie</label>
                                    <asp:TextBox ID="lblSerie" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <label for="lblMotor">Serie Motor</label>
                                    <asp:TextBox ID="lblMotor" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                              </div>
                            <div class="row">
                                <div class="col">
                                    <label for="lblFpago">Fecha Pago Aduana</label>
                                    <asp:TextBox ID="lblFpago" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col">
                                     <label for="txtMontoMatricula">Valor Matricula</label>
                                    <asp:TextBox ID="txtMontoMatricula" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                               <div class="col">
                                    <label for="txtNumPlaca">Placa</label>
                                   <asp:TextBox ID="txtNumPlaca" runat="server" Class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                              </div>
                            <div class="row">
                                 <div class="col">
                                    <label for="txtGestor">Gestor</label>
                                    <asp:TextBox ID="txtGestor" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <label for="txtAlmacen">Almacen</label>
                                    <asp:TextBox ID="txtAlmacen" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <label for="lblMontoDacion">Monto Dacion</label>
                                    <asp:TextBox ID="lblMontoDacion" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                              </div>
                              <div class="row">
                                    <div class="col">
                                        <label for="txtComments1">Comentarios Recepcion Moto</label>
                                        <asp:TextBox ID="txtComments1" runat="server" TextMode="MultiLine" class="form-control" autocomplete="off" Width="400px" Height="300px"></asp:TextBox>
                                    </div>
                                    <div class="col">
                                        <label for="txtComments2">Comentarios Ficha Venta</label>
                                        <asp:TextBox ID="txtComments2" runat="server" TextMode="MultiLine" class="form-control" autocomplete="off" Width="400px" Height="300px"></asp:TextBox>
                                    </div>
                                </div>
                                        <br />
       

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
                                       <h5>Fotos Recepcion</h5>
                                        Subir Fotos <asp:ImageButton ID="btnSetOne" runat="server" Height="25px" Width="25px" ImageUrl="~/images/image.png" ToolTip="Subir Fotos Recuperacion" />
                                        <br />
                                        <asp:GridView ID="gridSetFotos1" runat="server" AutoGenerateColumns="False" >
                                                <Columns>
                                                    <asp:ImageField DataImageUrlField="IMG1" HeaderText="Frente"></asp:ImageField>
                                                    <asp:ImageField DataImageUrlField="IMG2" HeaderText="Atras"></asp:ImageField>
                                                    <asp:ImageField DataImageUrlField="IMG3" HeaderText="Derecha"></asp:ImageField>
                                                    <asp:ImageField DataImageUrlField="IMG4" HeaderText="Izquierda"></asp:ImageField>
                                                    <asp:ImageField DataImageUrlField="IMG5" HeaderText="Arriba"></asp:ImageField>
                                                    <asp:ImageField DataImageUrlField="IMG6" HeaderText="Tacometro"></asp:ImageField>
                                                    <asp:ImageField DataImageUrlField="IMG7" HeaderText="Serie"></asp:ImageField>
                                                </Columns>
                                            </asp:GridView>
                                            <br />
    
        <h5>Fotos Venta</h5>
        Subir Fotos <asp:ImageButton ID="btnSetTwo" runat="server" Height="25px" Width="25px" ImageUrl="~/images/image.png" ToolTip="Subir Fotos Para Venta" />
        <br />
            <asp:GridView ID="gridSetFotos2" runat="server" AutoGenerateColumns="False" >
                <Columns>
                    <asp:ImageField DataImageUrlField="IMG1" HeaderText="Frente"></asp:ImageField>
                    <asp:ImageField DataImageUrlField="IMG2" HeaderText="Atras"></asp:ImageField>
                    <asp:ImageField DataImageUrlField="IMG3" HeaderText="Derecha"></asp:ImageField>
                    <asp:ImageField DataImageUrlField="IMG4" HeaderText="Izquierda"></asp:ImageField>
                    <asp:ImageField DataImageUrlField="IMG5" HeaderText="Arriba"></asp:ImageField>
                    <asp:ImageField DataImageUrlField="IMG6" HeaderText="Tacometro"></asp:ImageField>
                    <asp:TemplateField HeaderText="Video">
                        <ItemTemplate>
                           <video  width="300" height="200"  autoplay muted controls>
                                  <source src='<%# Eval("VIDEO")%>' />
                           </video>
                            </ItemTemplate>
                        </asp:TemplateField>
                </Columns>
            </asp:GridView>
         <br />
        <div id="dialog" style="display: none">
            </div>
    <script src="js/jquery.min.js"></script>
    <script src="js/popper.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/main.js"></script>
    <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    </form>
</body>
</html>
            <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
            <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.24/themes/start/jquery-ui.css" />
            <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.24/jquery-ui.min.js"></script>
            <script type="text/javascript">
                $(function () {
                    $("#dialog").dialog({
                        autoOpen: false,
                        modal: true,
                        height: 600,
                        width: 600,
                        title: "Zoomed Image"
                    });
                    $("[id*=gridSetFotos1] img").click(function () {
                        $('#dialog').html('');
                        $('#dialog').append($(this).clone());
                        $('#dialog').dialog('open');
                    });
                    $("[id*=gridSetFotos2] img").click(function () {
                        $('#dialog').html('');
                        $('#dialog').append($(this).clone());
                        $('#dialog').dialog('open');
                    });
                });
            </script>