<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ConfirmacionDacion.aspx.vb" Inherits="ConfirmacionDacion" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Ingreso Inventario</title>


      <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
    </style>

    <link href="https://fonts.googleapis.com/css?family=Lato:300,400,700&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="css/style.css" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>



  
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
                        .contenedor {
                            width: 100% !important;
                            height: 100% !important;
                        }
                    </style>
                    <div class="contenedor">
                        <div class="wrap">
                           <div class="login-wrap p-4 p-md-5">
                                <div class="form-group">
                                    <label for="txtRecordId">Numero de Orden</label>
                                    <asp:TextBox ID="txtRecordId" runat="server" class="form-control" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:Button ID="btnBuscarMoto" runat="server" class="form-control btn btn-primary rounded submit px-3" Text="Buscar Orden" />
                                </div>


                                <div class="row">
                                    <div class="col">
                                        <label for="txtCardname">Cliente</label>
                                        <asp:TextBox ID="txtCardname" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col">
                                        <label for="txtIdentidad">Identidad</label>
                                        <asp:TextBox ID="txtIdentidad" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col">
                                        <label for="txtColor">Color</label>
                                        <asp:TextBox ID="txtColor" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col">
                                        <label for="txtSerie">Serie</label>
                                        <asp:TextBox ID="txtSerie" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col">
                                        <label for="lblSerieM">Serie Motor</label>
                                        <asp:TextBox ID="txtSerieM" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col">
                                        <label for="txtModelo">Modelo</label>
                                        <asp:TextBox ID="txtModelo" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    </div>
                                     <div class="row">
                                     <div class="col">
                                    <label for="drpAlmacenes">Confirme Almacen de Ingreso</label>
                                    <br />
                                        <asp:DropDownList ID="drpAlmacenes" runat="server" class="btn btn-secondary dropdown-toggle; text-left" >
                                        </asp:DropDownList>
                                </div>
                                 <div class="col">
                                    <label for="drpCombustible">Nivel de Combustible</label>
                                    <br />
                                        <asp:DropDownList ID="drpCombustible" runat="server" class="btn btn-secondary dropdown-toggle; text-left">
                                            <asp:ListItem Selected="True" Value="0">Seleccione Nivel</asp:ListItem>
                                            <asp:ListItem Value="1" >Vacio</asp:ListItem>
                                            <asp:ListItem Value="2">Un Cuarto</asp:ListItem>
                                            <asp:ListItem Value="3">Medio Tanque</asp:ListItem>
                                            <asp:ListItem Value="5">Tres Cuartos</asp:ListItem>
                                            <asp:ListItem Value="5">Lleno</asp:ListItem>
                                        </asp:DropDownList>
                                </div>
                               <div class="col">
                                    <label for="drpPlaca">Tiene Placa?</label>
                                   <br />
                                    <asp:DropDownList ID="drpPlaca" runat="server" class="btn btn-secondary dropdown-toggle; text-left">
                                            <asp:ListItem Selected="True" Value="0">Seleccione Opcion</asp:ListItem>
                                            <asp:ListItem Value="True" >SI</asp:ListItem>
                                            <asp:ListItem Value="False">NO</asp:ListItem>
                                        </asp:DropDownList>
                                    
                                </div>
                               
                              </div>
                            
                                <br />
                                
                                <asp:Panel ID="pnlFotosMoto" runat="server">
                                <div class="row">
                                    <div class="col">
                                        <img src="Imagenes/moto1.png" id="moto1" alt="Frente" style="width: 20%" />
                                        <asp:FileUpload ID="fuImage1" runat="server" onchange="document.getElementById('moto1').src = window.URL.createObjectURL(this.files[0])" />
                                    </div>
                                    <div class="col">
                                        <img src="Imagenes/moto2.png" id="moto2" alt="Atras" style="width: 20%" />
                                        <asp:FileUpload ID="fuImage2" runat="server" onchange="document.getElementById('moto2').src = window.URL.createObjectURL(this.files[0])" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col">
                                        <img src="Imagenes/moto3.png" id="moto3" alt="Derecha" style="width: 20%" />
                                        <asp:FileUpload ID="fuImage3" runat="server" onchange="document.getElementById('moto3').src = window.URL.createObjectURL(this.files[0])" />
                                    </div>
                                    <div class="col">
                                        <img src="Imagenes/moto4.png" id="moto4" alt="Izquierda" style="width: 20%" />
                                        <asp:FileUpload ID="fuImage4" runat="server" onchange="document.getElementById('moto4').src = window.URL.createObjectURL(this.files[0])" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col">
                                        <img src="Imagenes/moto5.png" id="moto5" alt="Arriba" style="width: 20%" />
                                        <asp:FileUpload ID="fuImage5" runat="server" onchange="document.getElementById('moto5').src = window.URL.createObjectURL(this.files[0])" />
                                    </div>
                                    <div class="col">
                                        <img src="Imagenes/moto6.png" id="moto6" alt="Tacometro" style="width: 20%" />
                                        <asp:FileUpload ID="fuImage6" runat="server" onchange="document.getElementById('moto6').src = window.URL.createObjectURL(this.files[0])" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col">
                                        <img src="Imagenes/moto7.png" id="moto7" alt="Tacometro" style="width: 20%" />
                                        <asp:FileUpload ID="fuImage7" runat="server" onchange="document.getElementById('moto7').src = window.URL.createObjectURL(this.files[0])" />
                                    </div>
                                    <div class="col">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col">
                                        <br />
                                        <label for="txtEstadoMoto">Comentarios del Estado de la Moto al Recibirla</label>
                                        <asp:TextBox ID="txtEstadoMoto" runat="server" TextMode="MultiLine" class="form-control" autocomplete="off" Width="400px" Height="300px"></asp:TextBox>
                                    </div>
                                    <div class="col">
                                    </div>
                                </div>
                                <br/>
                               
                                <div class="row">
                                    <div class="col">
                                        <asp:Button ID="Button2" runat="server" class="form-control btn btn-primary rounded submit px-3" Text="Subir Fotos Moto" />
                                    </div>
                                </div>
                                    </asp:Panel>
                                <br/>
                                <asp:Panel ID="pnlFirma" runat="server" Visible="false">
                                <div class="row">
                                    <div class="col">
                                    <div class="tools">
                                    <center>
                                    <a href="#colors_sketch" data-tool="marker">Marcador</a> <a href="#colors_sketch" data-tool="eraser">
                                            Borrar</a>
                                        </center>

                                                </div>
                                        <br />
                                            <center>
                                            <canvas id="colors_sketch" width="500" height="200" style="border:1px solid #000000;"></canvas>
                                            </center>
                                        <br />
                                         </div>
                                    </div>
                                <div class="row">
                                    <div class="col">
                                            <asp:HiddenField ID="hfImageData" runat="server" />
                                            <asp:Button ID="btnSave" Text="Guardar Firma" runat="server" OnClick="Save"
                                                OnClientClick="return ConvertToImage(this)" class="form-control btn btn-primary rounded submit px-3"/>
                                </div>            
                                </div>
                                    </asp:Panel>
                                <br />
                                <div class="row">
                                    <div class="col">
                                            <asp:HiddenField ID="HiddenField1" runat="server" />
                                            <asp:Button ID="Button3" Text="Guardar Ingreso" runat="server" class="form-control btn btn-primary rounded submit px-3"/>
                                </div>            
                                </div>
                                <br/>
                                 <div class="row">
                                    <div class="col">
                                        <asp:Button ID="Button1" runat="server" class="form-control btn btn-secondary rounded submit px-3" Text="Limpiar Formulario" />
                                    </div>
                                </div>
                                <br />
                        <asp:Panel ID="InfoPanel" runat="server" class="alert alert-success alert-dismissable" Visible="False">
                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true"></button>
                            <i class="fa-lg fa fa-bullhorn"></i>
                            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                        </asp:Panel>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>



        



        <script src='script.js'></script>
        <script src="js/jquery.min.js"></script>
        <script src="js/popper.js"></script>
        <script src="js/bootstrap.min.js"></script>
        <script src="js/main.js"></script>
        <div>
            <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
        </div>
    </form>

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="https://cdn.rawgit.com/mobomo/sketch.js/master/lib/sketch.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#colors_sketch').sketch();
            $(".tools a").eq(0).attr("style", "color:#000");
            $(".tools a").click(function () {
                $(".tools a").removeAttr("style");
                $(this).attr("style", "color:#000");
            });
        });
        function ConvertToImage(btnSave) {
            var base64 = $('#colors_sketch')[0].toDataURL();
            $("[id*=hfImageData]").val(base64);
            __doPostBack(btnSave.name, "");
        };
    </script>



</body>
</html>

