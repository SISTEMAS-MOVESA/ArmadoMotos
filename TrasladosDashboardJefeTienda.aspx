<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosDashboardJefeTienda.aspx.vb" Inherits="TrasladosDashboardJefeTienda" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Traslados || Dashboard</title>
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
        <%--    <!-- Navigation -->
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
                            <a class="nav-link active" aria-current="page" href="TrasladosDashboard.aspx">Inicio</a>
                        </li>
                        <li class="nav-item dropdown">
                            <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Pre Solicitudes
                            </a>
                            <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                                <a class="dropdown-item" href="TrasladosPreSolicitud.aspx">Pre Solicitud</a>
                                <a class="dropdown-item" href="TrasladosCargaMacro.aspx">Presolicitudes Abiertas</a>
                                <div class="dropdown-divider"></div>
                                <a class="dropdown-item" href="TrasladosConfirmarPreSucursal.aspx">Confirmacion Presolicitud Sucursal</a>
                            </div>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="TrasladosSolicitud.aspx">Solicitud</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="TrasladosForklift.aspx">Armado de Moto</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="TrasladosPreparar.aspx">Carga Camion</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="TrasladosCrearDocumentos.aspx">Generar Traslados</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="TrasladosCrearDocumentos.aspx">Gestion de Transferencias</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>--%>
        <br />
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-3 col-6">
                    <div class="small-box bg-gradient-indigo">
                        <div class="inner">
                            <h3>1/6</h3>
                            <p>Pre-Solicitud de Traslado</p>
                        </div>
                        <div class="icon">
                            <i class="fas fa-motorcycle"></i>
                        </div>
                        <a href="TrasladosPreSolicitud.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>

                <div class="col-lg-3 col-6">
                    <div class="small-box bg-gradient-red">
                        <div class="inner">
                            <h3>2/6</h3>
                            <p>Confirmar Sugerido Logistica</p>
                        </div>
                        <div class="icon">
                            <i class="fa-solid fa-wand-magic-sparkles"></i>
                        </div>
                        <a href="TrasladosSolicitud.aspx" class="small-box-footer">Ingresar <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>

            </div>
        </div>
         <div class="container-fluid p-5">
                <table id="matrizrep" class="table table-bordered table-striped">
                    <thead style="text-align: center">
                        <tr>
                            <th>#</th>
                            <th>Fecha</th>
                            <th>C. Almacen</th>
                            <th>N. Almacen</th>
                            <th>Ruta</th>
                            <th>Sugerido</th>
                            <th>Confirmacion</th>
                            <th>Logistica</th>
                            <th>Carga </th>
                            <th>Solicitud</th>
                            <th>Transfer</th>
                            <th>Guia</th>
                            <th>Estado</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="repeater1" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td style="text-align: left;"><%# Eval("ID") %></td>
                                    <td style="text-align: left;"><%# Eval("FECHA") %></td>
                                    <td style="text-align: left;"><%# Eval("DESTINO") %></td>
                                    <td style="text-align: left;"><%# Eval("NALMDESTINO") %></td>
                                    <td style="text-align: left;"><%# Eval("RUTA") %></td>
                                    <td style="text-align: center;">
                                        <div class="btn-group btn-group-toggle" data-toggle="buttons" <%# iif(CInt(Eval("ESTADO"))>=1,"","hidden") %>>
                                            <label class="btn btn-default text-center active">
                                                <asp:HyperLink ID="hlMatrizRepuestos" runat="server" CssClass="btn" NavigateUrl='<%# iif(CInt(Eval("ESTADO"))>=1,"VisorSugerido.aspx?id=" & Eval("ID") & "&destino=" & Eval("DESTINO") & "&ruta=" & Eval("Ruta"),"") %>'><i class="large checkmark green icon"></i></asp:HyperLink>
                                            </label>
                                        </div>
                                    </td>
                                    <td style="text-align: center;">
                                        <div class="btn-group btn-group-toggle" data-toggle="buttons" <%# iif(CInt(Eval("ESTADO"))>=2,"","hidden") %>>
                                            <label class="btn btn-default text-center active">
                                                <asp:HyperLink ID="HyperLink7" runat="server" CssClass="btn" NavigateUrl='<%#IIf(CInt(Eval("ESTADO")) >= 2 And CInt(Eval("ESTADO")) >= 2, "VisorSucursal.aspx?id=" & Eval("ID") & "&destino=" & Eval("DESTINO") & "&ruta=" & Eval("Ruta"), "") %>'><i class="large checkmark green icon"></i></asp:HyperLink>
                                            </label>
                                        </div>
                                    </td>
                                    <td style="text-align: center;">
                                        <div class="btn-group btn-group-toggle" data-toggle="buttons" <%# iif(CInt(Eval("ESTADO"))>=3 ,"","hidden") %>>
                                            <label class="btn btn-default text-center active">
                                                <asp:HyperLink ID="HyperLink6" runat="server" CssClass="btn" NavigateUrl='<%# iif(CInt(Eval("ESTADO"))>=3 ,"VisorLogistica.aspx?id=" & Eval("ID") & "&destino=" & Eval("DESTINO") & "&ruta=" & Eval("Ruta"),"") %>'><i class="large checkmark green icon"></i></asp:HyperLink>
                                            </label>
                                        </div>
                                    </td>
                                    <td style="text-align: center;">
                                        <div class="btn-group btn-group-toggle" data-toggle="buttons" <%# iif(CInt(Eval("ESTADO"))>=4 ,"","hidden") %>>
                                            <label class="btn btn-default text-center active">
                                                <asp:HyperLink ID="HyperLink2" runat="server" CssClass="btn" NavigateUrl='<%# iif(CInt(Eval("ESTADO"))>=4 ,"VisorCarga.aspx?id=" & Eval("ID") & "&destino=" & Eval("DESTINO") & "&ruta=" & Eval("Ruta"),"") %>'><i class="large checkmark green icon"></i></asp:HyperLink>
                                            </label>
                                        </div>
                                    </td>
                                    <td style="text-align: center;">
                                        <div class="btn-group btn-group-toggle" data-toggle="buttons" <%# iif(CInt(Eval("ESTADO"))>=5 ,"","hidden") %>>
                                            <label class="btn btn-default text-center active">
                                                <asp:HyperLink ID="HyperLink4" runat="server" CssClass="btn" NavigateUrl='<%# iif(CInt(Eval("ESTADO"))>=5 ,"VisorSolicitud.aspx?id=" & Eval("ID") & "&destino=" & Eval("DESTINO") & "&ruta=" & Eval("Ruta"),"") %>'><i class="large checkmark green icon"></i></asp:HyperLink>
                                            </label>
                                        </div>
                                    </td>
                                    <td style="text-align: center;">
                                        <div class="btn-group btn-group-toggle" data-toggle="buttons" <%# iif(CInt(Eval("ESTADO"))>=6 ,"","hidden") %>>
                                            <label class="btn btn-default text-center active">
                                                <asp:HyperLink ID="HyperLink3" runat="server" CssClass="btn" NavigateUrl='<%# iif(CInt(Eval("ESTADO"))>=6 ,"VisorTransfer.aspx?id=" & Eval("ID") & "&destino=" & Eval("DESTINO") & "&ruta=" & Eval("Ruta"),"") %>'><i class="large checkmark green icon"></i></asp:HyperLink>
                                            </label>
                                        </div>
                                    </td>
                                    <td style="text-align: center;">
                                        <div class="btn-group btn-group-toggle" data-toggle="buttons" <%# iif(CInt(Eval("ESTADO"))>=7 ,"","hidden") %>>
                                            <label class="btn btn-default text-center active">
                                                <asp:HyperLink ID="HyperLink1" runat="server" CssClass="btn" NavigateUrl='<%# iif(CInt(Eval("ESTADO"))>=7 ,"VisorGuia.aspx?id=" & Eval("ID") & "&destino=" & Eval("DESTINO") & "&ruta=" & Eval("Ruta"),"") %>'><i class="large checkmark green icon"></i></asp:HyperLink>
                                                <%--large checkmark green icon--%>
                                            </label>
                                        </div>
                                    </td>
                                    <td style="text-align: center;">
                                        <label class="ui label blue"><%# Eval("ETAPA") %></label></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        <br>
        <center>
          <div class="container">
              <footer class="main-footer">
                  <strong> <i class="ion-paintbrush"></i> WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
              </footer>
          </div>
      </center>

        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>

    </form>


</body>
</html>
<!-- jQuery -->
<script src="plugins/jquery/jquery.min.js"></script>
<script src="vendor/bootstrap-4.1/popper.min.js"></script>
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

<script type="text/javascript">
    //Datatable responsive tables:
    $("#GridView1").DataTable({
        "responsive": true,
        "lengthChange": false,
        "autoWidth": false,
        "buttons": ["excel", "pdf", "colvis"],
        "sort": true,
        "order": [[0, 'asc']],
        "deferRender": true,
        "lengthMenu": [[20, -1], [20, "All"]],
    }).buttons().container().appendTo('#GridView1_wrapper .col-md-6:eq(0)');
</script>

<script>
    $(window).on('load', function () {
        $("#loftloader-wrapper").hide();
    });

    window.onbeforeunload = function (e) {
        $("#loftloader-wrapper").show();
    }
</script>

<link href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.css" rel="stylesheet" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.js"></script>
