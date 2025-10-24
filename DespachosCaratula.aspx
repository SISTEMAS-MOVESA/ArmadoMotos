<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DespachosCaratula.aspx.vb" Inherits="DespachosCaratula" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
     <title>Caratula Camion</title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.3.0/css/all.min.css" />
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"></script>

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>

    <script src="https://d3js.org/d3.v6.min.js"></script>

    <script src='https://cdn.jsdelivr.net/npm/sweetalert2@11'></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="mainDiv" style="border:2px double black;">
            <div class="datosVehiculo mx-3 my-2">
                <h1 class="text-center">Caratula de Despacho Camion <b><%= Request.QueryString("idcamion") %></b></h1>
                <div class="row mx-3">
                    <h5>Datos del Vehiculo</h5>
                    <div class="col">Empresa</div>
                    <div class="col">RTN</div>
                    <div class="col">Marca / Modelo</div>
                    <div class="col">Placa</div>
                </div>
                <div class="row mx-3">
                    <div class="col">
                        <asp:Label ID="txtEmpresa" runat="server" CssClass="form-control-sm"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:Label ID="txtRTN" runat="server" CssClass="form-control-sm"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:Label ID="txtMarca" runat="server" CssClass="form-control-sm"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:Label ID="txtPlaca" runat="server" CssClass="form-control-sm"></asp:Label>
                    </div>
                </div>

            </div>
            <div class="datosMotorista mx-3 my-2">
                <div class="row mx-3">
                    <h5>Datos del Motorista</h5>
                    <div class="col">Motorista</div>
                    <div class="col">Licencia</div>
                    <div class="col">Celular</div>
                </div>
            <div class="row mx-3">
                <div class="col">
                    <asp:Label ID="txtMotorista" runat="server" CssClass="form-control-sm"></asp:Label>
                </div>
                <div class="col">
                    <asp:Label ID="txtLicencia" runat="server" CssClass="form-control-sm"></asp:Label>
                </div>
                <div class="col">
                    <asp:Label ID="txtCelular" runat="server" CssClass="form-control-sm"></asp:Label>
                </div>
            </div>
            </div>
            <div class="detalleMotos mx-3 my-2">
                <div class="row mx-3 my-2">
                    <asp:Repeater ID="repeater1" runat="server">
                        <ItemTemplate>
                            <div style="border-style: solid; color: black; width: 100%; margin-bottom:5px;">
                                <div style="padding: 5px;">
                                    <b>ID:</b>
                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("IDCAMION") %>'></asp:Label>
                                    <b>Almacen Destino:</b>
                                    <asp:Label ID="lblCamion" runat="server" Text='<%# Eval("ALMDESTINO") + " " + Eval("WhsName") %>'></asp:Label>
                                    <b>Ruta:</b>
                                        <asp:Label ID="lblRuta" runat="server" Text='<%# Eval("RUTA") %>'></asp:Label>
                                    <asp:GridView ID="gvOrders" runat="server" CssClass="table table-sm"
                                        Width="100%" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:BoundField DataField="MODELO" HeaderText="Modelo" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ARTICULO" HeaderText="Codigo" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="SERIEASIGNADA" HeaderText="Serie Chasis" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="SerieMotor" HeaderText="Serie Motor" ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <br />
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div class="row mx-3 my-5">
                    <div class="row mx-3 my-2">
                        <div class="col text-center" style="border-top:2px solid black;"> BATERIAS DE ACIDO</div>
                        <div class="col text-center" style="border-top:2px solid black; border-left:2px solid black;">BATERIAS DE GEL</div>
                        <div class="col text-center" style="border-top:2px solid black; border-left:2px solid black;">KIT HERRAMIENTAS</div>
                        <div class="col text-center" style="border-top:2px solid black; border-left:2px solid black;">COMPONENTES</div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
