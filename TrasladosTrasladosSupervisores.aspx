<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosTrasladosSupervisores.aspx.vb" Inherits="TrasladosTrasladosSupervisores" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Traslados || Pre-Solicitud</title>
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
    <link href="css/movesa_load_spinner.css" rel="stylesheet" />
    <!-- jQuery -->
    <script src="plugins/jquery/jquery.min.js"></script>
    <script src="vendor/bootstrap-4.1/popper.min.js"></script>

    <!-- Bootstrap 4 -->
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>

    <link href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.js"></script>

    <%--izitoast--%>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>

    <%--steeper--%>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/5.3.0/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/5.3.0/js/bootstrap.bundle.min.js"></script>

    <style>
    .stepper {
        display: flex;
        justify-content: space-between;
        margin-bottom: 20px;
    }

    .step {
        flex-grow: 1;
        text-align: center;
        position: relative;
    }

    .step-header {
        display: flex;
        flex-direction: column;
        align-items: center;
    }

    .step-number {
        width: 40px;
        height: 40px;
        display: flex;
        justify-content: center;
        align-items: center;
        font-size: 1.25rem;
    }

    .step.active .step-number {
        background-color: #007bff;
        color: white;
    }

    .step-content .content {
        display: none;
    }

    .step-content .content.active {
        display: block;
    }
</style>


</head>
<body>
    <form id="form1" runat="server">
        <!-- Navigation -->
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark static-top">
            <div class="container">
                <a class="navbar-brand" href="TrasladosDashboardSupervisores.aspx">
                    <img src="Imagenes/mnegra.png" width="50" height="40" alt="" />
                </a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarSupportedContent">
                    <ul class="navbar-nav ms-auto">
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="TrasladosDashboardSupervisores.aspx">Inicio</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="TrasladosPresolicitudSupervisores.aspx">Pre Solicitud</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="TrasladosTrasladosSupervisores.aspx">Traslados de Motos</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="TrasladosConfirmarSupervisor.aspx">Confirmacion Sugerido</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="Default.aspx">Cerrar Sesion</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>

        <br />
        <hr />
        <%--INICIO SECCION INGRESAR DATOS SOLICITUD PREPARACION--%>
        <section class="content">
            <div class="container-fluid">
                <!-- Main row -->
                <div class="row">
                    <div class="container mt-5">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="stepper d-flex flex-column flex-md-row">
                                    <div class="step active" data-step="1">
                                        <div class="step-header text-center">
                                            <div class="step-number rounded-circle bg-primary text-white">1</div>
                                            <div class="step-title mt-2">Paso 1</div>
                                        </div>
                                    </div>
                                    <div class="step" data-step="2">
                                        <div class="step-header text-center">
                                            <div class="step-number rounded-circle bg-secondary text-white">2</div>
                                            <div class="step-title mt-2">Paso 2</div>
                                        </div>
                                    </div>
                                    <div class="step" data-step="3">
                                        <div class="step-header text-center">
                                            <div class="step-number rounded-circle bg-secondary text-white">3</div>
                                            <div class="step-title mt-2">Paso 3</div>
                                        </div>
                                    </div>
                                </div>

                                <div class="step-content mt-4">
                                    <div id="step-1" class="content active">
                                        <h4>Seleccione la Serie de la Moto</h4>
                                        <div class="container-fluid">
                                            <div class="col">
                                                <asp:GridView ID="gridEncabezadoTransfer" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" Width="100%">
                                                    <HeaderStyle CssClass="thead-dark" />
                                                    <Columns>
                                                        <asp:BoundField DataField="DocEntry" HeaderText="Docentry" />
                                                        <asp:BoundField DataField="DocNum" HeaderText="Documento" />
                                                        <asp:BoundField DataField="ObjType" HeaderText="Objeto" ItemStyle-CssClass="heatmap-cell1" />
                                                        <asp:BoundField DataField="Filler" HeaderText="Alm Origen" ItemStyle-CssClass="heatmap-cell1" />
                                                        <asp:BoundField DataField="ToWhsCode" HeaderText="Alm Destino" ItemStyle-CssClass="heatmap-cell1" />
                                                        <asp:BoundField DataField="CardCode" HeaderText="Codigo Cliente" ItemStyle-CssClass="heatmap-cell2" />
                                                        <asp:BoundField DataField="Comments" HeaderText="Observaciones" ItemStyle-CssClass="heatmap-cell2" />
                                                        <asp:BoundField DataField="GroupNum" HeaderText="PL" ItemStyle-HorizontalAlign="Left" />
                                                        <%--<asp:ButtonField ButtonType="Image" ImageUrl="~/Images/bike.png" Text="Sugerir" CommandName="Sugerir" HeaderText="Sugerir">
            <ControlStyle Height="30px" Width="30" />
            <ItemStyle Wrap="False" />
        </asp:ButtonField>--%>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        <p>Este es el contenido del primer paso.</p>
                                        <button type="button" class="btn btn-primary next-step">Siguiente</button>
                                    </div>
                                    <div id="step-2" class="content">
                                        <h4>Contenido del Paso 2</h4>
                                        <p>Este es el contenido del segundo paso.</p>
                                        <button type="button" class="btn btn-primary next-step">Siguiente</button>
                                        <button type="button" class="btn btn-secondary prev-step">Anterior</button>
                                    </div>
                                    <div id="step-3" class="content">
                                        <h4>Contenido del Paso 3</h4>
                                        <p>Este es el contenido del tercer paso.</p>
                                        <button type="button" class="btn btn-secondary prev-step">Anterior</button>
                                        <button class="btn btn-success">Finalizar</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                </div>
            </div>
        </section>
        <%--FIN SECCION INGRESAR DATOS SOLICITUD PREPARACION--%>
       
        <div class="row">
        </div>

        <br>
        <center>
            <div class="container">
                <footer class="main-footer">
                    <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
                </footer>
            </div>
        </center>

        <asp:ScriptManager ID="sm1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

        <!-- DataTables  & Plugins -->
        <script src="plugins/datatables/jquery.dataTables.min.js"></script>
        <script src="plugins/datatables-bs4/js/dataTables.bootstrap4.min.js"></script>
        <script src="plugins/datatables-responsive/js/dataTables.responsive.min.js"></script>
        <script src="plugins/datatables-responsive/js/responsive.bootstrap4.min.js"></script>
        <script src="plugins/datatables-buttons/js/dataTables.buttons.min.js"></script>
        <script src="plugins/datatables-buttons/js/buttons.bootstrap4.min.js"></script>
        <script src="plugins/jszip/jszip.min.js"></script>
        <script src="plugins/pdfmake/pdfmake.min.js"></script>
        <script src="plugins/pdfmake/vfs_fonts.js"></script>
        <script src="plugins/datatables-buttons/js/buttons.html5.min.js"></script>
        <script src="plugins/datatables-buttons/js/buttons.print.min.js"></script>
        <script src="plugins/datatables-buttons/js/buttons.colVis.min.js"></script>


        <link href="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/css/toastr.min.css" rel="stylesheet" />
        <script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js"></script>

        <script type="text/javascript">
            function showContent(typeofMsg, mensaje) {
                toastr.options = {
                    "closeButton": true,
                    "debug": false,
                    "progressBar": true,
                    "preventDuplicates": false,
                    "positionClass": "toast-top-right",
                    "showDuration": "400",
                    "hideDuration": "1000",
                    "timeOut": "7000",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                }
                toastr[typeofMsg](mensaje);
            }
        </script>
    </form>


</body>
</html>

<script>
    $("#drpModelosMoto").dropdown(
        {
            "fullTextSearch": true
        });
    $("#drpMotoristas").dropdown(
        {
            "fullTextSearch": true
        });
    $("#drpMotivoTraslado").dropdown(
        {
            "fullTextSearch": true
        });
</script>

<script type="text/javascript">

    //Datatable responsive tables:
    $("#gridSolicitudAbierta").DataTable({
        "responsive": true,
        "lengthChange": false,
        "autoWidth": false,
        "buttons": ["excel", "pdf", "colvis"],
        "sort": true,
        "order": [[0, 'asc']],
        "deferRender": true,
        "lengthMenu": [[20, -1], [20, "All"]],
    }).buttons().container().appendTo('#gridSolicitudAbierta_wrapper .col-md-6:eq(0)');
</script>

<link href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.css" rel="stylesheet" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.js"></script>
<script type="text/javascript">
    var submit = 0;
    function CheckDouble() {
        if (++submit > 1) {
            alert('Esto a veces tarda unos segundos, tenga paciencia.!');
            return false;
        }
    }
</script>

<script>
    $(document).ready(function () {
        $('.next-step').click(function () {
            let currentStep = $(this).closest('.content');
            let currentStepNumber = currentStep.attr('id').split('-')[1];
            let nextStepNumber = parseInt(currentStepNumber) + 1;

            currentStep.removeClass('active');
            $(`#step-${nextStepNumber}`).addClass('active');

            $(`.step[data-step="${currentStepNumber}"] .step-number`).removeClass('bg-primary').addClass('bg-secondary');
            $(`.step[data-step="${nextStepNumber}"] .step-number`).removeClass('bg-secondary').addClass('bg-primary');
        });

        $('.prev-step').click(function () {
            let currentStep = $(this).closest('.content');
            let currentStepNumber = currentStep.attr('id').split('-')[1];
            let prevStepNumber = parseInt(currentStepNumber) - 1;

            currentStep.removeClass('active');
            $(`#step-${prevStepNumber}`).addClass('active');

            $(`.step[data-step="${currentStepNumber}"] .step-number`).removeClass('bg-primary').addClass('bg-secondary');
            $(`.step[data-step="${prevStepNumber}"] .step-number`).removeClass('bg-secondary').addClass('bg-primary');
        });
    });
</script>
