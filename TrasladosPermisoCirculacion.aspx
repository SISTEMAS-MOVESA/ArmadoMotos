<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosPermisoCirculacion.aspx.vb"
    Inherits="TrasladosPermisoCirculacion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Boleta de Circulacion</title>
    <link rel="stylesheet"
        href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.3.0/css/all.min.css">
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css">
    <link rel="stylesheet" href="plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css">
    <link rel="stylesheet" href="plugins/icheck-bootstrap/icheck-bootstrap.min.css">
    <link rel="stylesheet" href="plugins/jqvmap/jqvmap.min.css">
    <link rel="stylesheet" href="dist/css/adminlte.min.css">
    <link rel="stylesheet" href="plugins/overlayScrollbars/css/OverlayScrollbars.min.css">
    <link rel="stylesheet" href="plugins/daterangepicker/daterangepicker.css">
    <link rel="stylesheet" href="plugins/summernote/summernote-bs4.min.css">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js"
        crossorigin="anonymous"></script>

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>

    <%-- <style>
            .col-4 {
            position: relative; /* Para que los elementos absolutos sean relativos a este div */
            }

            .col-4::before {
            content: "";
            position: absolute;
            top: 50%; /* Ajusta esta propiedad para cambiar la posición vertical de la línea */
            left: 0;
            width: 100%;
            height: 1px; /* Grosor de la línea */
            background-color: black; /* Color de la línea */
            }

            </style>--%>
</head>

<body>
    <form id="form1" runat="server">
        <div class="d-flex justify-content-center">
            <img src="Imagenes/logomovesa.jpg" width="250" height="150" />
        </div>
        <div class="d-flex justify-content-center">
            <p>
                <h3>CONSTANCIA NO. SPS-MOV-1-2021-RV</h3>
            </p>
            <br />
        </div>
        <br />
        <br />
        <div class="container-fluid vertical-center-row">
            <p>
                <h3>La empresa: MOVESA por medio de la presente HACE CONSTAR que ha vendido a:</h3>
                <br />
            </p>
        </div>
        <div class="container-fluid">
            <div class="row">
                <div class="col-3"><strong>CLIENTE</strong></div>
                <div class="col-5">
                    ___________________________________________________
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-3"><strong>IDENTIDAD</strong></div>
                <div class="col-5">
                   ___________________________________________________
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-3"><strong>FACTURA</strong></div>
                <div class="col-5">
                    ___________________________________________________
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-3"><strong>FECHA FACTURA</strong></div>
                <div class="col-5">
                    ___________________________________________________
                </div>
            </div>
        </div>
        <hr />
        <div class="container-fluid vertical-center-row">
            <h3>El vehículo cuyas características son las siguientes:</h3>
            <br />
        </div>
        <div class="container-fluid">
            <div class="row">
                <div class="col-4">
                    <h2>Marca</h2>
                </div>
                <div class="col 5">
                    <h2>
                        <asp:Label runat="server" ID="lblMarca" Text=""></asp:Label></h2>
                </div>
            </div>
            <div class="row">
                <div class="col-4">
                    <h2>Modelo</h2>
                </div>
                <div class="col 5">
                    <h2>
                        <asp:Label runat="server" ID="lblModelo" Text=""></asp:Label></h2>
                </div>
            </div>
            <div class="row">
                <div class="col-4">
                    <h2>Tipo Vehiculo</h2>
                </div>
                <div class="col 5">
                    <h2>
                        <asp:Label runat="server" ID="lblTipo" Text=""></asp:Label></h2>
                </div>
            </div>
            <div class="row">
                <div class="col-4">
                    <h2>No. Chasis</h2>
                </div>
                <div class="col 5">
                    <h2>
                        <asp:Label runat="server" ID="lblChasis" Text=""></asp:Label></h2>
                </div>
            </div>
            <div class="row">
                <div class="col-4">
                    <h2>No. Motor</h2>
                </div>
                <div class="col 5">
                    <h2>
                        <asp:Label runat="server" ID="lblMotor" Text=""></asp:Label></h2>
                </div>
            </div>
            <div class="row">
                <div class="col-4">
                    <h2>No. VIN</h2>
                </div>
                <div class="col 5">
                    <h2>
                        <asp:Label runat="server" ID="lblVin" Text=""></asp:Label></h2>
                </div>
            </div>
            <div class="row">
                <div class="col-4">
                    <h2>Color</h2>
                </div>
                <div class="col 5">
                    <h2>
                        <asp:Label runat="server" ID="lblColor" Text=""></asp:Label></h2>
                </div>
            </div>
            <div class="row">
                <div class="col-4">
                    <h2>Año</h2>
                </div>
                <div class="col 5">
                    <h2>
                        <asp:Label runat="server" ID="lblYear" Text=""></asp:Label></h2>
                </div>
            </div>
            <p>
                <h4>En aplicación al Artículo 6 del Decreto 50-2016 “Amnistía Vehicular” existe un plazo de 30 días calendario para el registro (inscripción) de un vehículo nuevo o importado, periodo que se contabiliza a partir de la fecha de venta que indica la factura.
                </h4>

            </p>
            <p>
                <h4>En tal sentido, se solicita a las autoridades de la DIRECCIÓN NACIONAL DE TRANSITO, su valiosa colaboración para que se respete el plazo otorgado al usuario para que pueda circular sin placas, hasta que transcurran los 30 días calendario otorgados por la Ley.
                </h4>
            </p>
            <br />
             <div class="row">
                <div class="col">__________________________________________</div>
            </div>
            <div class="row">
                <div class="col">Jefe Departamento Placas</div>
            </div>
            <div class="row">
                <div class="col"><strong>Ana Maritza Aguilera</strong></div>
            </div>

            <img src="Imagenes/firmaplacas.png" width="850"/>
        </div>
    </form>
</body>

</html>
