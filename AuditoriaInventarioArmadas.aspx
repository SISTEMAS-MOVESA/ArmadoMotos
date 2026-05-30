<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AuditoriaInventarioArmadas.aspx.vb" Inherits="AuditoriaInventarioArmadas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Inventario Motos</title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.3.0/css/all.min.css">
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css">
    <link rel="stylesheet" href="plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css">
    <link rel="stylesheet" href="plugins/icheck-bootstrap/icheck-bootstrap.min.css">
    <link rel="stylesheet" href="plugins/jqvmap/jqvmap.min.css">
    <link rel="stylesheet" href="dist/css/adminlte.min.css">
    <link rel="stylesheet" href="plugins/overlayScrollbars/css/OverlayScrollbars.min.css">
    <link rel="stylesheet" href="plugins/daterangepicker/daterangepicker.css">
    <link rel="stylesheet" href="plugins/summernote/summernote-bs4.min.css">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
    <link href="css/movesa_load_spinner.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/2.9.2/umd/popper.min.js" integrity="sha512-2rNj2KJ+D8s1ceNasTIex6z4HWyOnEYLVC3FigGOmyQCZc2eBXKgOxQmo3oKLHyfcj53uz4QMsRCWNbLd32Q1g==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>

    <link href="https://cdnjs.cloudflare.com/ajax/libs/dropzone/5.9.3/dropzone.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/dropzone/5.9.3/dropzone.min.js"></script>
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/responsive/2.5.0/css/responsive.dataTables.min.css" />


</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark static-top">
            <div class="container">
                <a class="navbar-brand" href="MainDashBoard.aspx">
                    <img src="Imagenes/mnegra.png" width="50" height="40" alt="" />
                </a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarSupportedContent">
                    <ul class="navbar-nav ms-auto">
                        <li class="nav-item">
                            <a class="nav-link" href="DashboardAuditoria.aspx">Inicio</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="AuditoriaInventarioArmadas.aspx">Inventario Motos Armadas</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#">Auditoria CC</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="AuditoriaInformeInventario.aspx">Informes</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="Default.aspx">Cerrar Sesion</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
        <div class="container-fluid px-4">
            <h1 class="mt-4">Inventario de Motos</h1>
            <div class="row">
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12 py-2">
                    <label for="txtSnMoto">Digite Número de Serie</label>
                    <asp:TextBox ID="txtSnMoto" runat="server" class="form-control" autocomplete="off" placeholder="Digite el Numero de Serie"></asp:TextBox>
                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServiceMethod="SearchCustomers"
                        MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
                        TargetControlID="txtSnMoto" FirstRowSelected="false">
                    </cc1:AutoCompleteExtender>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12 py-2">
                    <asp:Button ID="btnBusarSerie" runat="server" Text="Buscar Serie" CssClass="btn btn-info w-50 h-100" />
                </div>
            </div>
            <h2 class="my-3">Verificación de Datos</h2>
            <div class="row">
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12 py-3">
                    <label for="drpEstadoVeh">Estado del Vechiculo</label>
                    <asp:DropDownList ID="drpEstadoVeh" runat="server" CssClass="form-control">
                        <asp:ListItem Selected="True" Value="0" Text="Seleccione"></asp:ListItem>
                        <asp:ListItem Value="Buen Estado" Text="Buen Estado"></asp:ListItem>
                        <asp:ListItem Value="Mal Estado" Text="Mal Estado"></asp:ListItem>
                        <asp:ListItem Value="Detalle Pinruta" Text="Detalles en Pintura"></asp:ListItem>
                        <asp:ListItem Value="Piezas Faltantes" Text="Piezas Faltantes"></asp:ListItem>
                        <asp:ListItem Value="Desarmada" Text="Desarmada"></asp:ListItem>
                        <asp:ListItem Value="Limpieza" Text="Requere Limpieza"></asp:ListItem>
                        <asp:ListItem Value="Chocada" Text="Moto Chocada"></asp:ListItem>
                        <asp:ListItem Value="Quemada" Text="Moto Quemada"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12 py-3">
                    <label for="drpOrigenDestino">Origen / Destino</label>
                    <asp:DropDownList ID="drpOrigenDestino" runat="server" CssClass="form-control">
                        <asp:ListItem Selected="True" Value="0" Text="Seleccione"></asp:ListItem>
                        <asp:ListItem Value="DESPACHO CD" Text="Moto para Despacho"></asp:ListItem>
                        <asp:ListItem Value="INVSEG" Text="Inventario Seguridad"></asp:ListItem>
                        <asp:ListItem Value="GARANTIA" Text="Moto Garantía"></asp:ListItem>
                        <asp:ListItem Value="DEVOLUCION" Text="Moto Devolución Venta"></asp:ListItem>
                        <asp:ListItem Value="DECOMISADA" Text="Moto Decomisada"></asp:ListItem>
                        <asp:ListItem Value="Retorno CI" Text="Moto retornada CI"></asp:ListItem>
                        <asp:ListItem Value="Retorno CD" Text="Moto retornada CD"></asp:ListItem>
                        <asp:ListItem Value="Dañada en Transporte" Text="Moto dañada en transporte"></asp:ListItem>
                        <asp:ListItem Value="Daño Fabrica" Text="Daño Fabrica u Origen"></asp:ListItem>
                        <asp:ListItem Value="Muestra" Text="Motos de Muestra"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12 py-3">
                    <label for="drpPasillo">Ubicación Pasillo</label>
                    <asp:DropDownList ID="drpPasillo" runat="server" CssClass="form-control">
                        <asp:ListItem Selected="True" Value="0" Text="Seleccione"></asp:ListItem>
                        <asp:ListItem Value="1" Text="1"></asp:ListItem>
                        <asp:ListItem Value="2" Text="2"></asp:ListItem>
                        <asp:ListItem Value="3" Text="3"></asp:ListItem>
                        <asp:ListItem Value="4" Text="4"></asp:ListItem>
                        <asp:ListItem Value="5" Text="5"></asp:ListItem>
                        <asp:ListItem Value="6" Text="6"></asp:ListItem>
                        <asp:ListItem Value="7" Text="7"></asp:ListItem>
                        <asp:ListItem Value="8" Text="8"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12 py-3">
                    <label for="drpSegmento">Ubicación Segmento</label>
                    <asp:DropDownList ID="drpSegmento" runat="server" CssClass="form-control">
                        <asp:ListItem Selected="True" Value="0" Text="Seleccione"></asp:ListItem>
                        <asp:ListItem Value="A" Text="A"></asp:ListItem>
                        <asp:ListItem Value="B" Text="B"></asp:ListItem>
                        <asp:ListItem Value="MESANINE" Text="MESANINE"></asp:ListItem>
                        <asp:ListItem Value="ANDEN" Text="ANDEN PARQUEO"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12 py-3">
                    <asp:CheckBox ID="chkLimpieza" runat="server" Text="Requiere Limpieza"/>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <asp:CheckBox ID="chkAuditoria" runat="server" Text="Requiere Control Auditoria"/>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <asp:CheckBox ID="chkPintura" runat="server" Text="Requiere Pintura" />
                </div>
            </div>
            <div class="row">
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <label for="txtCodigo">Codigo Moto</label>
                    <asp:TextBox ID="txtCodigo" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-lg-8 col-md-12 col-12 py-3">
                    <label for="txtDescripcion">Descripcion Moto</label>
                    <asp:TextBox ID="txtDescripcion" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <label for="txtSeriemoto">Serie Moto Seleccionada</label>
                    <asp:TextBox ID="txtSeriemoto" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <label for="txtSeriemotor">Serie Motor</label>
                    <asp:TextBox ID="txtSeriemotor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <label for="txtMarca">Marca Moto</label>
                    <asp:TextBox ID="txtMarca" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <label for="txtModelo">Modelo Moto</label>
                    <asp:TextBox ID="txtModelo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <label for="txtCilindros">Cilindraje Moto</label>
                    <asp:TextBox ID="txtCilindros" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <label for="txtColor">Color Moto</label>
                    <asp:TextBox ID="txtColor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <label for="txtALmacenSAP">Almacen SAP</label>
                    <asp:TextBox ID="txtALmacenSAP" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <label for="txtEstatusSerieSAP">Estatus Serie SAP</label>
                    <asp:TextBox ID="txtEstatusSerieSAP" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12 py-3">
                    <label for="txtYears">Año Moto</label>
                    <asp:TextBox ID="txtYear" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12 py-3">
                    <label for="txtArmado">Días Armado</label>
                    <asp:TextBox ID="txtArmado" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12 py-3">
                    <label for="txtAgeing">Días Antigüedad</label>
                    <asp:TextBox ID="txtAgeing" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-12 py-3">
                <label for="txtObservaciones">Observaciones Generales del Vehiculo</label>
                <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control" TextMode="MultiLine" Height="200px"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-12 py-3">
                <h3 class="text-center">Subir Imágenes</h3>
                <div id="dropzoneForm" class="dropzone h-100">
                    <div class="dz-message">
                        Arrastra las imágenes aquí o haz clic para subir.
                    </div>
                </div>
            </div>
        </div>
        <hr />
        <div class="row">
            <div class="col-12 py-3">
                <h3 class="text-center">Kardex Vehículo</h3>
                <div class="container-fluid">
                    <asp:GridView ID="gridKardex" runat="server" CssClass="display compact" AutoGenerateColumns="false">
                        <HeaderStyle CssClass="thead-dark" />
                        <Columns>
                            <asp:BoundField DataField="LocCode" HeaderText="Código" />
                            <asp:BoundField DataField="Almacen" HeaderText="Almacén" />
                            <asp:BoundField DataField="Quantity" HeaderText="Cantidad" />
                            <asp:BoundField DataField="Documento" HeaderText="Tipo Documento" />
                            <asp:BoundField DataField="Docnum" HeaderText="Documento" />
                            <asp:BoundField DataField="DocDate" HeaderText="Fecha" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-12 py-3">
                <asp:Button ID="btnGuardarInfo" runat="server" CssClass="btn btn-success mt-3 w-30 h-100" Text="Guardar Datos" />
            </div>
        </div>
        <asp:ScriptManager ID="sm1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    </form>
</body>
</html>
<!-- jQuery -->
<script src="plugins/jquery/jquery.min.js"></script>
<!-- jQuery UI 1.11.4 -->
<script src="plugins/jquery-ui/jquery-ui.min.js"></script>
<!-- Resolve conflict in jQuery UI tooltip with Bootstrap tooltip -->
<script>
    $.widget.bridge('uibutton', $.ui.button)
</script>
<!-- Bootstrap 4 -->
<script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
<!-- ChartJS -->
<script src="plugins/chart.js/Chart.min.js"></script>
<!-- Sparkline -->
<script src="plugins/sparklines/sparkline.js"></script>
<!-- JQVMap -->
<script src="plugins/jqvmap/jquery.vmap.min.js"></script>
<script src="plugins/jqvmap/maps/jquery.vmap.usa.js"></script>
<!-- jQuery Knob Chart -->
<script src="plugins/jquery-knob/jquery.knob.min.js"></script>
<!-- daterangepicker -->
<script src="plugins/moment/moment.min.js"></script>
<script src="plugins/daterangepicker/daterangepicker.js"></script>
<!-- Tempusdominus Bootstrap 4 -->
<script src="plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>
<!-- Summernote -->
<script src="plugins/summernote/summernote-bs4.min.js"></script>
<!-- overlayScrollbars -->
<script src="plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js"></script>
<!-- AdminLTE App -->
<script src="dist/js/adminlte.js"></script>

<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
<link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />
<script src="https://cdn.datatables.net/responsive/2.5.0/js/dataTables.responsive.min.js"></script>

<script type="text/javascript">
    $(function () {
        $("[id*=gridKardex]").DataTable(
            {
                bLengthChange: true,
                lengthMenu: [[10, -1], [10, "All"]],
                bFilter: true,
                bSort: false,
                bPaginate: false,
                responsive: true
            });
    });
</script>


<script>
    // Configuración Dropzone
    Dropzone.autoDiscover = false;
    var myDropzone = new Dropzone("#dropzoneForm", {
        url: "UploadHandler.ashx",
        paramName: "file",
        maxFilesize: 10,
        init: function () {
            this.on("sending", function (file, xhr, formData) {
                let serieMoto = $('#txtSeriemoto').val();
                formData.append("serieMoto", serieMoto);
                console.log(serieMoto);
            });
        }
    });
</script>
