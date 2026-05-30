<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UbicacionesMotos.aspx.vb" Inherits="UbicacionesMotos" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Parametrizaciones || Dashboard</title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback">
    <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css">
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css">
    <link rel="stylesheet" href="plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css">
    <link rel="stylesheet" href="plugins/icheck-bootstrap/icheck-bootstrap.min.css">
    <link rel="stylesheet" href="plugins/jqvmap/jqvmap.min.css">
    <link rel="stylesheet" href="dist/css/adminlte.min.css">
    <link rel="stylesheet" href="plugins/overlayScrollbars/css/OverlayScrollbars.min.css">
    <link rel="stylesheet" href="plugins/daterangepicker/daterangepicker.css">
    <link rel="stylesheet" href="plugins/summernote/summernote-bs4.min.css">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
    <!-- Incluye SweetAlert2 desde CDN si no lo tienes -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</head>
<body>
    <form id="form1" runat="server">
    
        <div class="container-fluid my-3">
            <div style="display: flex; justify-content: center; align-items: center; margin-bottom: 15px;">
                <h1 class="display-4">Ubicaciones Almacen Moto</h1>
            </div>
            <div style="display: flex; justify-content: center; align-items: center;">
                <div class="col-3">
                    <button type="button" class="btn btn-block btn-warning" onclick="regresar()">Regresar</button>
                </div>
                <div class="col-3">
                    <button type="button" class="btn btn-block btn-info" onclick="agregarUbicacion()">Agregar Ubicación</button>
                </div>
            </div>
                <div class="d-flex justify-content-center my-5">
                    <div class="row">
                        <div class="col-12">
                            <asp:GridView ID="gridUbicaciones" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered table-hover" Style="width: 100%">
                                <HeaderStyle CssClass="thead-dark" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" ImageUrl="~/Imagenes/edit.png" Text="Modificar" CommandName="Modificar">
                                        <ControlStyle Height="18px" Width="18px" />
                                        <ItemStyle Wrap="False" />
                                    </asp:ButtonField>
                                    <asp:ButtonField ButtonType="Image" ImageUrl="~/Imagenes/delete.png" Text="Eliminar" CommandName="Eliminar">
                                        <ControlStyle Height="18px" Width="18px" />
                                        <ItemStyle Wrap="False" />
                                    </asp:ButtonField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
        </div>
        <center>
            <div class="container">
                <footer class="main-footer">
                    <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
                </footer>
            </div>
        </center>
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    </form>
</body>
</html>

<script src="plugins/jquery/jquery.min.js"></script>
<script src="plugins/jquery-ui/jquery-ui.min.js"></script>
<script>
    $.widget.bridge('uibutton', $.ui.button)
</script>
<script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
<script src="plugins/chart.js/Chart.min.js"></script>
<script src="plugins/sparklines/sparkline.js"></script>
<script src="plugins/jqvmap/jquery.vmap.min.js"></script>
<script src="plugins/jqvmap/maps/jquery.vmap.usa.js"></script>
<script src="plugins/jquery-knob/jquery.knob.min.js"></script>
<script src="plugins/moment/moment.min.js"></script>
<script src="plugins/daterangepicker/daterangepicker.js"></script>
<script src="plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>
<script src="plugins/summernote/summernote-bs4.min.js"></script>
<script src="plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js"></script>
<!-- AdminLTE App -->
<script src="dist/js/adminlte.js"></script>

<script>
    function regresar() {
        window.location.href = "MenuParametrizaciones.aspx";
    }
    function agregarUbicacion() {
        Swal.fire({
            title: 'Agregar nueva ubicación',
            input: 'text',
            inputLabel: 'Nombre de la ubicación',
            inputPlaceholder: 'Ejemplo: Almacén Central',
            showCancelButton: true,
            confirmButtonText: 'Guardar',
            preConfirm: (ubicacion) => {
                if (!ubicacion || ubicacion.trim() === "") {
                    Swal.showValidationMessage('Debes ingresar un nombre');
                    return false;
                }
                // Hacer AJAX a WebMethod
                return fetch('UbicacionesMotos.aspx/InsertarUbicacion', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json; charset=utf-8' },
                    body: JSON.stringify({ ubicacion: ubicacion })
                })
                    .then(response => response.json())
                    .then(data => {
                        if (data.d === "ok") {
                            return true;
                        } else {
                            throw new Error(data.d);
                        }
                    })
                    .catch(error => {
                        Swal.showValidationMessage('Error: ' + error.message);
                        return false;
                    });
            }
        }).then((result) => {
            if (result.isConfirmed && result.value) {
                Swal.fire({
                    title: '¡Guardado!',
                    text: 'La ubicación ha sido agregada. Redirigiendo...',
                    icon: 'success',
                    allowOutsideClick: false,
                    showConfirmButton: false,
                    timer: 5000,
                    didOpen: () => {
                        Swal.showLoading();
                    }
                }).then(() => {
                    window.location.href = "UbicacionesMotos.aspx";
                });
            }
        });
    }

    //function agregarUbicacion() {
    //    Swal.fire({
    //        title: 'Agregar nueva ubicación',
    //        input: 'text',
    //        inputLabel: 'Nombre de la ubicación',
    //        inputPlaceholder: 'Ejemplo: Almacén Central',
    //        showCancelButton: true,
    //        confirmButtonText: 'Guardar',
    //        preConfirm: (ubicacion) => {
    //            if (!ubicacion || ubicacion.trim() === "") {
    //                Swal.showValidationMessage('Debes ingresar un nombre');
    //                return false;
    //            }
    //            // Hacer AJAX a WebMethod
    //            return fetch('UbicacionesMotos.aspx/InsertarUbicacion', {
    //                method: 'POST',
    //                headers: { 'Content-Type': 'application/json; charset=utf-8' },
    //                body: JSON.stringify({ ubicacion: ubicacion })
    //            })
    //                .then(response => response.json())
    //                .then(data => {
    //                    if (data.d === "ok") {
    //                        return true;
    //                    } else {
    //                        throw new Error(data.d);
    //                    }
    //                })
    //                .catch(error => {
    //                    Swal.showValidationMessage('Error: ' + error.message);
    //                    return false;
    //                });
    //        }
    //    }).then((result) => {
    //        if (result.isConfirmed && result.value) {
    //            Swal.fire('¡Guardado!', 'La ubicación ha sido agregada.', 'success');
    //            // Aquí puedes recargar una tabla, limpiar campos, etc.
    //        }
    //    });
    //}
</script>