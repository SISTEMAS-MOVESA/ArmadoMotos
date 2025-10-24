<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ConfirmacionVenta.aspx.vb" Inherits="ConfirmacionVenta" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">



<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Ficha Para Venta</title>


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

           <center>
               <h2>Subir Video del Estado de la Moto</h2>
                                        <input type="file" name="postedFile" runat="server" id="MyFileUpload" />
                                        <input type="button" id="btnUpload" value="Upload" />
                                        <progress id="fileProgress" style="display: none"></progress>
                                        <hr />
                                        <span id="lblMessage" style="color: Green"></span>
                                        <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
                                        <script type="text/javascript">
                                            $("body").on("click", "#btnUpload", function () {
                                                $.ajax({
                                                    url: 'HandlerVB.ashx',
                                                    type: 'POST',
                                                    data: new FormData($('form')[0]),
                                                    cache: false,
                                                    contentType: false,
                                                    processData: false,
                                                    success: function (file) {
                                                        $("#fileProgress").hide();
                                                        $("#lblMessage").html("<b>" + file.name + "</b> has been uploaded.");
                                                    },
                                                    xhr: function () {
                                                        var fileXhr = $.ajaxSettings.xhr();
                                                        if (fileXhr.upload) {
                                                            $("progress").show();
                                                            fileXhr.upload.addEventListener("progress", function (e) {
                                                                if (e.lengthComputable) {
                                                                    $("#fileProgress").attr({
                                                                        value: e.loaded,
                                                                        max: e.total
                                                                    });
                                                                }
                                                            }, false);
                                                        }
                                                        return fileXhr;
                                                    }
                                                });
                                            });
                                        </script>
            </center>
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
                                    <label for="txtRecordId">Numero de Serie</label>
                                    <asp:TextBox ID="txtRecordId" runat="server" class="form-control" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:Button ID="btnBuscarMoto" runat="server" class="form-control btn btn-primary rounded submit px-3" Text="Buscar Serie" />
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
                                     </div>
                            
                                <br />
                                
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
                                    <br />
                                <div class="row">
                                <div class="col">
                                 
                                </div>
                                <div class="col">
                                </div>
                                </div>
                                <div class="row">
                                    <div class="col">
                                        <br />
                                        <label for="txtEstadoMoto">Comentarios Reparaciones Hechas</label>
                                        <asp:TextBox ID="txtEstadoMoto" runat="server" TextMode="MultiLine" class="form-control" autocomplete="off" Width="400px" Height="300px"></asp:TextBox>
                                    </div>
                                    <div class="col">
                                    </div>
                                </div>
                                <br/>
                               
                                <div class="row">
                                    <div class="col">
                                        <br/>
                                        <asp:Button ID="Button2" runat="server" class="form-control btn btn-primary rounded submit px-3" Text="Subir Fotos Venta" />
                                    </div>
                                </div>
                                 <div class="row">
                                    <div class="col">
                                        <br/>
                                        <asp:Button ID="Button1" runat="server" class="form-control btn btn-secondary rounded submit px-3" Text="Limpiar Formulario" />
                                    </div>
                                </div>
                                

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
</body>
</html>
