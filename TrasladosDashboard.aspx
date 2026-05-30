<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosDashboard.aspx.vb" Inherits="TrasladosDashboard" %>

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
    <%--<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>--%>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"></script>


    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>

    <style>
        thead input {
            width: 100% !important;
            padding: 3px !important;
            box-sizing: border-box !important;
        }

        .sticky-header th {
            position: sticky !important;
            top: 0 !important;
            z-index: 2 !important;
            background-color: #343a40 !important; /* Color de fondo del encabezado */
            color: white !important; /* Color del texto */
            border-bottom: 2px solid #dee2e6 !important;
            padding: 8px !important;
            text-align: left !important;
        }

        .sticky-grid {
            border-collapse: collapse !important;
            width: 100% !important;
        }

            .sticky-grid th,
            .sticky-grid td {
                border: 1px solid #dee2e6 !important;
            }
    </style>
</head>
<body>

    <form id="form1" runat="server">
        <!-- Navigation -->
        <div class="ui inverted menu">
            <div class="ui container">
                <a class="header item" href="MainDashBoard.aspx">
                    <img src="Imagenes/mnegra.png" width="50" height="40" alt="" />
                </a>
                <div class="right menu">
                    <a class="item" href="TrasladosDashboard.aspx">Inicio</a>
                    <div class="ui simple dropdown item">
                        Pre Solicitudes
                <i class="dropdown icon"></i>
                        <div class="menu">
                            <a class="item" href="TrasladosCargaMacro.aspx">Carga Macro</a>
                            <a class="item" href="TrasladosConfirmarPreSucursal.aspx">Confirmacion Sucursal</a>
                            <a class="item" href="TrasladosPreSolicitud.aspx">Pre Solicitud</a>
                            <a class="item" href="TrasladosPreSolicitudSucursal.aspx">Pre Solicitud Sucursal</a>
                            <div class="divider"></div>
                            <a class="item" href="TrasladosPresolicitudesAbiertas.aspx">Flujo Pre-Solicitudes</a>
                            <a class="item" href="TrasladosSolicitudesAbiertas.aspx">Flujo Solicitudes</a>
                            <a class="item" href="TrasladosFlujoProduccion.aspx">Flujo Produccion</a>
                            <div class="divider"></div>
                            <a class="item" href="TrasladosCuadroBasico.aspx">Cuadro Basico</a>
                            <a class="item" href="TrasladosCuadroBasicoLogico.aspx">Cuadro Basico Logico</a>
                            <div class="divider"></div>
                            <a class="item" href="TrabajosAdicionalesDashboard.aspx">Trabajos Adicionales</a>
                            <div class="divider"></div>
                            <a class="item" href="Grafico.aspx" target="_blank">KPI Inventarios</a>
                            <div class="divider"></div>
                            <a class="item" href="TrasladosDashboardPlanner.aspx">Planificador</a>
                        </div>
                    </div>
                    <a class="item" href="TrasladosSolicitud.aspx">Solicitudes</a>
                    <a class="item" href="TrasladosForklift.aspx">Armado de Moto</a>
                    <a class="item" href="TrasladosPreparar.aspx">Carga Camion</a>
                    <a class="item" href="TrasladosCrearDocumentos.aspx">Generar Traslados</a>
                    <a class="item" href="TrasladosCrearDocumentos.aspx">Gestion de Transferencias</a>
                    <a class="item" href="Default.aspx">Cerrar Sesion</a>
                </div>
            </div>
        </div>
        <div class="row" style="display: flex; justify-content: center; align-content: center; align-items: center;">
            <div class="col-md-1">
                <a href="TrasladosCargaMacro.aspx" class="btn btn-block btn-info btn-lg h-100">Sugerido CD</a>
            </div>
            <div class="col-md-1">
                <a href="TrasladosCargaMacroIndirecto.aspx" class="btn btn-block btn-warning btn-lg h-100">Sugerido CI</a>
            </div>
            <div class="col-md-1">
                <a href="TrasladosDashboardIndiceD.aspx" class="btn btn-block btn-dark btn-lg h-100">Indice D CD</a>
            </div>
            <div class="col-md-1">
                <a href="TrasladosDashboardIndiceDCI.aspx" class="btn btn-block btn-secondary btn-lg h-100">Indice D CI</a>
            </div>
            <div class="col-md-1">
                <a href="TrasladosDashboardMotocargo.aspx" class="btn btn-block btn-secondary btn-lg h-100">Motocargo</a>
            </div>
            <div class="col-md-1">
                <a href="TrasladosDashboardTransito.aspx" class="btn btn-block btn-secondary btn-lg h-100">Transitos</a>
            </div>
            <div class="col-md-1">
                <a href="TrasladosDashboardHero.aspx" class="btn btn-block btn-danger btn-lg h-100">Indice D Hero</a>
            </div>
            <div class="col-md-1 position-relative">
                <a href="TrasladosDespachosOrdenesDiunsa.aspx" class="btn btn-block btn-danger btn-lg h-100 position-relative">Pedidos Diunsa
                <span class="badge badge-dark position-absolute top-0 start-100 translate-middle">
                    <asp:Label Text="0" runat="server" ID="lblDiunsa" />
                </span>
                </a>
            </div>
        </div>
        <div style="height: 700px; overflow-y: auto;">
            <div class="container-fluid">
                <div class="ui top attached tabular menu">
                    <a class="item active" data-tab="first">Ordenes Pendientes</a>
                    <a class="item" data-tab="second">Ordenes en Proceso</a>
                    <a class="item" data-tab="third">Ordenes Finalizadas</a>
                </div>
                <div class="ui bottom attached tab segment active" data-tab="first">
                    <table id="matrizrep" class="table table-bordered table-striped" style="width: 100%">
                        <thead class="thead-dark" style="text-align: center">
                            <tr class="sticky-header">
                                <th>#</th>
                                <th>Fecha</th>
                                <th>Codigo</th>
                                <th>Almacen</th>
                                <th>Ruta</th>
                                <th>Supervisor</th>
                                <th>Fecha Armado</th>
                                <th>Fecha Despacho</th>
                                <th>Sugerido</th>
                                <th>Confirmar</th>
                                <th>Estado</th>
                                <th>Accion</th>
                            </tr>
                            <tr>
                                <!-- Fila para los filtros -->
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
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
                                        <td style="text-align: left;"><%# Eval("SUPERVISORASIGNADO") %></td>
                                        <td style="text-align: left;"><%# Eval("FechaArmado") %></td>
                                        <td style="text-align: left;"><%# Eval("FechaEntrega") %></td>
                                        <td style="text-align: center;">
                                            <div class="btn-group btn-group-toggle" data-toggle="buttons" <%# iif(CInt(Eval("ESTADO")) = 1, "", "hidden") %>>
                                                <label class="btn btn-default text-center active">
                                                    <asp:HyperLink ID="hlMatrizRepuestos" runat="server" CssClass="btn" NavigateUrl='<%# iif(CInt(Eval("ESTADO"))>=1,"VisorSugerido.aspx?id=" & Eval("ID") & "&destino=" & Eval("DESTINO") & "&ruta=" & Eval("Ruta"),"") %>'><i class="large checkmark green icon"></i></asp:HyperLink>
                                                </label>
                                            </div>
                                        </td>
                                        <td style="text-align: center;">
                                            <div class="btn-group btn-group-toggle" data-toggle="buttons"
                                                data-id="<%# Eval("ID") %>"
                                                data-destino="<%# Eval("DESTINO") %>"
                                                data-ruta="<%# Eval("RUTA") %>"
                                                data-etapa="<%# Eval("ETAPA") %>"
                                                data-estado="<%# Eval("ESTADO") %>">
                                                <label class="btn btn-default text-center active">
                                                    <a id="dynamicLink_<%# Eval("ID") %>" href="#" class="btn">
                                                        <i class="large checkmark green icon"></i>
                                                    </a>
                                                </label>
                                            </div>
                                        </td>
                                        <td style="text-align: center;">
                                            <label class="ui label blue"><%# Eval("ETAPA") %></label></td>
                                        <td style="display: flex; gap: 5px;">
                                            <asp:Button ID="btnEliminar" runat="server" class="btn btn-danger text-center active" Text="E" OnClick="btnAbrirFly_Click" CommandArgument='<%# Eval("ID") %>' ToolTip="Eliminar Documento" />
                                            <asp:Button ID="btnFinalizar" runat="server" class="btn btn-success text-center active" Text="F" OnClick="btnFinalizar_Click" CommandArgument='<%# Eval("ID") %>' ToolTip="Finalizar Documento" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
                <div class="ui bottom attached tab segment" data-tab="second">
                    <table id="matrizrep_en_proceso" class="table table-bordered table-striped" style="width: 100%">
                        <thead class="thead-dark" style="text-align: center">
                            <tr class="sticky-header">
                                <th>#</th>
                                <th>Fecha</th>
                                <th>Codigo</th>
                                <th>Almacen</th>
                                <th>Ruta</th>
                                <th>Supervisor</th>
                                <th>Fecha Armado</th>
                                <th>Fecha Despacho</th>
                                <%--<th>Sugerido</th>
                                <th>Confirm Suc</th>--%>
                                <th>En Preparacion</th>
                                <th>Picking</th>
                                <th>Estado</th>
                                <th>Accion</th>
                            </tr>
                            <tr>
                                <!-- Fila para los filtros -->
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <%--<th>
                                    <input type="text" /></th>--%>
                                <%--<th>
                                    <input type="text" /></th>--%>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="repeater3" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td style="text-align: left;"><%# Eval("ID") %></td>
                                        <td style="text-align: left;"><%# Eval("FECHA") %></td>
                                        <td style="text-align: left;"><%# Eval("DESTINO") %></td>
                                        <td style="text-align: left;"><%# Eval("NALMDESTINO") %></td>
                                        <td style="text-align: left;"><%# Eval("RUTA") %></td>
                                        <td style="text-align: left;"><%# Eval("SUPERVISORASIGNADO") %></td>
                                        <td style="text-align: left;"><%# Eval("FechaArmado") %></td>
                                        <td style="text-align: left;"><%# Eval("FechaEntrega") %></td>
                                        <%--<td style="text-align: center;">
                                            <div class="btn-group btn-group-toggle" data-toggle="buttons" <%# iif(CInt(Eval("ESTADO"))>=1,"","hidden") %>>
                                                <label class="btn btn-default text-center active">
                                                    <asp:HyperLink ID="hlMatrizRepuestos" runat="server" CssClass="btn" NavigateUrl='<%# iif(CInt(Eval("ESTADO"))>=1,"VisorSugerido.aspx?id=" & Eval("ID") & "&destino=" & Eval("DESTINO") & "&ruta=" & Eval("Ruta"),"") %>'><i class="large checkmark green icon"></i></asp:HyperLink>
                                                </label>
                                            </div>
                                        </td>
                                        <td style="text-align: center;">
                                            <div class="btn-group btn-group-toggle" data-toggle="buttons"
                                                data-id="<%# Eval("ID") %>"
                                                data-destino="<%# Eval("DESTINO") %>"
                                                data-ruta="<%# Eval("RUTA") %>"
                                                data-etapa="<%# Eval("ETAPA") %>"
                                                data-estado="<%# Eval("ESTADO") %>">
                                                <label class="btn btn-default text-center active">
                                                    <a id="dynamicLink_<%# Eval("ID") %>" href="#" class="btn">
                                                        <i class="large checkmark green icon"></i>
                                                    </a>
                                                </label>
                                            </div>
                                        </td>--%>
                                        <td style="text-align: center;">
                                            <div class="btn-group btn-group-toggle" data-toggle="buttons" <%# iif(CInt(Eval("ESTADO")) = 3, "", "hidden") %>>
                                                <label class="btn btn-default text-center active">
                                                    <asp:HyperLink ID="HyperLink6" runat="server" CssClass="btn" NavigateUrl='<%#IIf(CInt(Eval("ESTADO")) = 3, "VisorLogistica.aspx?id=" & Eval("ID") & "&destino=" & Eval("DESTINO") & "&ruta=" & Eval("Ruta"), "") %>'><i class="large checkmark green icon"></i></asp:HyperLink>
                                                </label>
                                            </div>
                                        </td>
                                        <td style="text-align: center;">
                                            <div class="btn-group btn-group-toggle" data-toggle="buttons" <%#IIf(CInt(Eval("ESTADO")) = 3, "", "hidden") %>>
                                                <label class="btn btn-default text-center active">
                                                    <asp:HyperLink ID="HyperLink2" runat="server" CssClass="btn" NavigateUrl='<%#IIf(CInt(Eval("ESTADO")) = 3, "TrasladosConfirmarSerie.aspx?macroid=" & Eval("ID"), "") %>'><i class="large checkmark green icon"></i></asp:HyperLink>
                                                </label>
                                            </div>
                                        </td>
                                        <td style="text-align: center;">
                                            <label class="ui label blue"><%# Eval("ETAPA") %></label></td>
                                        <td style="display: flex; gap: 5px;">
                                            <asp:Button ID="btnEliminar" runat="server" class="btn btn-danger text-center active" Text="E" OnClick="btnAbrirFly_Click" CommandArgument='<%# Eval("ID") %>' ToolTip="Eliminar Documento" />
                                            <asp:Button ID="btnFinalizar" runat="server" class="btn btn-success text-center active" Text="F" OnClick="btnFinalizar_Click" CommandArgument='<%# Eval("ID") %>' ToolTip="Finalizar Documento" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
                <div class="ui bottom attached tab segment" data-tab="third">
                    <table id="matrizrep_finalizados" class="table table-bordered table-striped" style="width: 100%">
                        <thead class="thead-dark" style="text-align: center">
                            <tr class="sticky-header">
                                <th>#</th>
                                <th>Fecha</th>
                                <th>Codigo</th>
                                <th>Almacen</th>
                                <th>Ruta</th>
                                <th>Accion</th>
                            </tr>
                            <tr>
                                <!-- Fila para los filtros -->
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                                <th>
                                    <input type="text" /></th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="repeater2" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td style="text-align: left;"><%# Eval("ID") %></td>
                                        <td style="text-align: left;"><%# Eval("FECHA") %></td>
                                        <td style="text-align: left;"><%# Eval("DESTINO") %></td>
                                        <td style="text-align: left;"><%# Eval("NALMDESTINO") %></td>
                                        <td style="text-align: left;"><%# Eval("RUTA") %></td>
                                        <td style="display: flex; gap: 5px;">
                                            <asp:Button ID="btnEliminar" runat="server" class="btn btn-danger text-center active" Text="Eliminar" OnClick="btnAbrirFly_Click" CommandArgument='<%# Eval("ID") %>' ToolTip="Eliminar Documento" />
                                            <asp:Button ID="btnActivar" runat="server" class="btn btn-success text-center active" Text="Activar" OnClick="btnActivar_Click" CommandArgument='<%# Eval("ID") %>' ToolTip="Activar Documento" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <%--ininicio modal pintura--%>
        <div class="ui flyout" id="IniciarPintura">
            <i class="close icon"></i>
            <div class="ui header">
                Esta Seguro de Eliminar Ese Documento?
            </div>
            <div class="content">
                <div class="row">
                    <div class="col-12">
                        <label for="drpEstadoVeh">ID Documento</label>
                        <asp:TextBox ID="txtFlyMiniId" runat="server" Text="" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="actions">
                <asp:Button ID="btnCancelar" runat="server" class="ui red button" Text="Cancelar" />
                <asp:Button ID="btnProcederEliminar" runat="server" class="ui green button" Text="Proceder Eliminar" />
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
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.css" />
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.js" integrity="sha512-gnoBksrDbaMnlE0rhhkcx3iwzvgBGz6mOEj4/Y5ZY09n55dYddx6+WYc72A55qEesV8VX2iMomteIwobeGK1BQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.css" integrity="sha512-3quBdRGJyLy79hzhDDcBzANW+mVqPctrGCfIPosHQtMKb3rKsCxfyslzwlz2wj1dT8A7UX+sEvDjaUv+WExQrA==" crossorigin="anonymous" referrerpolicy="no-referrer" />

        <script>
            $('.ui.flyout').flyout({
                context: $("form:eq(0)")
            });
        </script>

    </form>
</body>
</html>
<!-- jQuery -->
<script src="plugins/jquery/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.11.0/umd/popper.min.js" integrity="sha384-b/U6ypiBEHpOf/4+1nzFpr53nxSS+GLCkfwBdFNTxtclqqenISfwAzpKaMNFNmj4" crossorigin="anonymous"></script>

<%--<script src="vendor/bootstrap-4.1/popper.min.js"></script>--%>
<!-- Bootstrap 4 -->
<%--<script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>--%>
<!-- AdminLTE App -->
<script src="dist/js/adminlte.js"></script>

<!-- DataTables  & Plugins -->

<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.1/js/dataTables.buttons.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.html5.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.print.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.colVis.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
<link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />
<link href="https://cdn.datatables.net/buttons/1.7.1/css/buttons.dataTables.min.css" rel="stylesheet" type="text/css" />

<script type="text/javascript">
    $(function () {
        var table = $("[id*=matrizrep]").DataTable({
            dom: 'Bfrtip',
            buttons: [
                'excelHtml5',
                'pdfHtml5',
                'colvis'
            ],
            bLengthChange: true,
            lengthMenu: [[10, -1], [10, "All"]],
            bFilter: true,
            bPaginate: false,
            bSort: false,
        });

        // Filtros en cada columna
        $('#matrizrep thead tr:eq(1) th').each(function (i) {
            $('input', this).on('keyup change', function () {
                if (table.column(i).search() !== this.value) {
                    table
                        .column(i)
                        .search(this.value)
                        .draw();
                }
            });
        });
    });

    $(function () {
        // Verifica si ya hay una instancia de DataTable asociada
        if ($.fn.DataTable.isDataTable("#matrizrep_en_proceso")) {
            $("#matrizrep_en_proceso").DataTable().destroy(); // Destruye la instancia existente
        }

        // Inicializa la DataTable
        var table = $("#matrizrep_en_proceso").DataTable({
            dom: 'Bfrtip',
            buttons: [
                'excelHtml5',
                'pdfHtml5',
                'colvis'
            ],
            bLengthChange: true,
            lengthMenu: [[10, -1], [10, "All"]],
            bFilter: true,
            bPaginate: false,
            bSort: false,
        });

        // Filtros en cada columna
        $('#matrizrep_en_proceso thead tr:eq(1) th').each(function (i) {
            $('input', this).on('keyup change', function () {
                if (table.column(i).search() !== this.value) {
                    table
                        .column(i)
                        .search(this.value)
                        .draw();
                }
            });
        });
    });

    $(function () {
        // Verifica si ya hay una instancia de DataTable asociada
        if ($.fn.DataTable.isDataTable("#matrizrep_finalizados")) {
            $("#matrizrep_finalizados").DataTable().destroy(); // Destruye la instancia existente
        }

        // Inicializa la DataTable
        var table = $("#matrizrep_finalizados").DataTable({
            dom: 'Bfrtip',
            buttons: [
                'excelHtml5',
                'pdfHtml5',
                'colvis'
            ],
            bLengthChange: true,
            lengthMenu: [[10, -1], [10, "All"]],
            bFilter: true,
            bPaginate: false,
            bSort: false,
        });

        // Filtros en cada columna
        $('#matrizrep_finalizados thead tr:eq(1) th').each(function (i) {
            $('input', this).on('keyup change', function () {
                if (table.column(i).search() !== this.value) {
                    table
                        .column(i)
                        .search(this.value)
                        .draw();
                }
            });
        });
    });


</script>


<link href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.css" rel="stylesheet" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.js" integrity="sha512-gnoBksrDbaMnlE0rhhkcx3iwzvgBGz6mOEj4/Y5ZY09n55dYddx6+WYc72A55qEesV8VX2iMomteIwobeGK1BQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.css" integrity="sha512-3quBdRGJyLy79hzhDDcBzANW+mVqPctrGCfIPosHQtMKb3rKsCxfyslzwlz2wj1dT8A7UX+sEvDjaUv+WExQrA==" crossorigin="anonymous" referrerpolicy="no-referrer" />

<script>

    $('.ui.dropdown').dropdown();



    document.addEventListener("DOMContentLoaded", function () {
        document.querySelectorAll('.btn-group[data-etapa]').forEach(function (group) {

            
            const etapa = group.getAttribute('data-etapa');
            const estado = parseInt(group.getAttribute('data-estado'), 10);
            const id = group.getAttribute('data-id');
            const destino = group.getAttribute('data-destino');
            const ruta = group.getAttribute('data-ruta');
            const linkElement = document.getElementById(`dynamicLink_${id}`);

            if (linkElement) {
                if (estado >= 2) {
                    if (etapa === "PRE SOLICITUD CI") {
                        linkElement.href = `TrasladosCargaMacroIndirecto.aspx?id=${id}&destino=${destino}&ruta=${ruta}&etapa=${etapa}`;
                    } else if (etapa === "PRE SOLICITUD CD") {
                        linkElement.href = `TrasladosCargaMacro.aspx?id=${id}&destino=${destino}&ruta=${ruta}&etapa=${etapa}`;
                    } else {
                        linkElement.href = `TrasladosConfirmLogistica.aspx?id=${id}&destino=${destino}&ruta=${ruta}&etapa=${etapa}`;
                    }
                } else {
                    group.style.display = "none";
                    //linkElement.href = `TrasladosConfirmLogistica.aspx?id=${id}&destino=${destino}&ruta=${ruta}&etapa=${etapa}`;
                }
            }
        });
    });

    $('.menu .item').tab();


</script>
