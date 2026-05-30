<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GenerarPO.aspx.vb" Inherits="GenerarPO" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Liquidaciones</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/dataTables.bootstrap5.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/2.4.1/css/buttons.bootstrap5.min.css" />
    <link rel="stylesheet" href="css/style.css" />
    <style>
        body { background: #f4f6f9; font-size: 0.85rem; }
        .kpi-card { background: #fff; border-radius: 8px; padding: 12px 20px; box-shadow: 0 1px 4px rgba(0,0,0,.1); min-width: 160px; }
        .kpi-label { font-size: 0.68rem; text-transform: uppercase; color: #6c757d; font-weight: 600; letter-spacing: .5px; margin-bottom: 2px; }
        .kpi-value { font-size: 1.7rem; font-weight: 700; line-height: 1.1; }
        .toolbar-bar { background: #fff; border-bottom: 1px solid #dee2e6; padding: 8px 16px; display: flex; align-items: center; flex-wrap: wrap; gap: 6px; }
        .rango-btn { font-size: 0.75rem; padding: 3px 9px; }
        .grid-compact thead th, .grid-compact tbody td { font-size: 0.78rem !important; padding: 4px 6px !important; vertical-align: middle !important; }
        .grid-compact thead th { background: #343a40 !important; color: #fff !important; white-space: nowrap; }
        .card-scroll { overflow-x: auto; }
        .row-liq td { background-color: #ede7f6 !important; }
        .row-ocs td { background-color: #fff8e1 !important; }
        .row-fps td { background-color: #e8f5e9 !important; }
        .legend-dot { width: 12px; height: 12px; border-radius: 50%; display: inline-block; margin-right: 3px; vertical-align: middle; }
    </style>
</head>
<body>
<form id="form1" runat="server">

    <!-- ── Navigation ─────────────────────────────────────── -->
    <div class="pos-f-t">
        <div class="collapse" id="navbarToggleExternalContent">
            <div class="bg-dark p-4">
                <ul class="nav nav-tabs">
                    <li class="nav-item">
                        <a class="nav-link" href="MainDashBoard.aspx"><img src="Imagenes/mnegra.png" width="30" height="25" alt="Portal Armado de Motos" /></a>
                    </li>
                    <asp:Panel ID="pnlParametrizaciones" runat="server" Visible="True">
                        <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Parametrizaciones</a>
                        <ul class="dropdown-menu">
                            <asp:Panel ID="pnlContratistas"  runat="server" Visible="True"><li><a class="dropdown-item" href="Contratistas.aspx">Contratistas</a></li></asp:Panel>
                            <asp:Panel ID="pnlTrabajoAD"     runat="server" Visible="True"><li><a class="dropdown-item" href="TrabajosAd.aspx">Trabajos Adicionales</a></li></asp:Panel>
                            <asp:Panel ID="pnlPreciosArmado" runat="server" Visible="True"><li><a class="dropdown-item" href="PrecioModelos.aspx">Precios Armado</a></li></asp:Panel>
                            <asp:Panel ID="pnlUsuarios"      runat="server" Visible="True"><li><a class="dropdown-item" href="Usuarios.aspx">Usuarios</a></li></asp:Panel>
                        </ul>
                    </asp:Panel>
                    <asp:Panel ID="pnlAsignaciones" runat="server" Visible="True">
                        <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Motos en Caja</a>
                        <ul class="dropdown-menu">
                            <asp:Panel ID="pnlMotoEncaja"  runat="server" Visible="True"><li><a class="dropdown-item" href="MotoenCaja.aspx">Asignar Moto</a></li></asp:Panel>
                            <asp:Panel ID="PnlStockGlobal" runat="server" Visible="True"><li><a class="dropdown-item" href="StockPorModelos.aspx">Stock Por Modelo</a></li></asp:Panel>
                        </ul>
                    </asp:Panel>
                    <asp:Panel ID="pnlArmado" runat="server" Visible="True">
                        <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Armado</a>
                        <ul class="dropdown-menu">
                            <asp:Panel ID="pnlListadoMotosProceso" runat="server" Visible="True"><li><a class="dropdown-item" href="ListadoProceso.aspx">Motos En Proceso</a></li></asp:Panel>
                        </ul>
                    </asp:Panel>
                    <asp:Panel ID="PnlControlCalidad" runat="server" Visible="True">
                        <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Control de Calidad</a>
                        <ul class="dropdown-menu">
                            <asp:Panel ID="pnlListadoMotosCalidad" runat="server" Visible="True"><li><a class="dropdown-item" href="ListadoCCalidad.aspx">Motos En Control de Calidad</a></li></asp:Panel>
                            <asp:Panel ID="pnoInformeProcesadasCC" runat="server" Visible="True"><li><a class="dropdown-item" href="InformeCC.aspx">Informe de Motos Procesadas CC</a></li></asp:Panel>
                        </ul>
                    </asp:Panel>
                    <asp:Panel ID="pnlProcesoContable" runat="server" Visible="True">
                        <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Proceso Contable</a>
                        <ul class="dropdown-menu">
                            <asp:Panel ID="pnoCrearLiquidacion"      runat="server" Visible="True"><li><a class="dropdown-item" href="ProcesoContable.aspx">Crear Liquidacion</a></li></asp:Panel>
                            <asp:Panel ID="pnlCrearPO"               runat="server" Visible="True"><li><a class="dropdown-item" href="GenerarPO.aspx">Crear Orden de Compra</a></li></asp:Panel>
                            <asp:Panel ID="pnlConsultaLiquidaciones" runat="server" Visible="True"><li><a class="dropdown-item" href="ProcesoContableConsulta.aspx">Consulta Liquidaciones Anteriores</a></li></asp:Panel>
                        </ul>
                    </asp:Panel>
                    <asp:Panel ID="pnlDisponibles" runat="server" Visible="True">
                        <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Disponibles</a>
                        <ul class="dropdown-menu">
                            <asp:Panel ID="pnlDisponibleArmadoSAP" runat="server" Visible="True"><li><a class="dropdown-item" href="InformeMotosDisponibles.aspx">Motos Armadas Procesadas Portal</a></li></asp:Panel>
                            <asp:Panel ID="pnlExpedienteVeh"        runat="server" Visible="True"><li><a class="dropdown-item" href="ExpedienteVehiculo.aspx">Expediente Vehiculo</a></li></asp:Panel>
                        </ul>
                    </asp:Panel>
                    <asp:Panel ID="pnlNoDisponible" runat="server" Visible="True">
                        <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">No Disponibles</a>
                        <ul class="dropdown-menu">
                            <asp:Panel ID="pnlInformeNoDisponible" runat="server" Visible="True"><li><a class="dropdown-item" href="InformeMotosNoDisponibles.aspx">Motos No Disponibles</a></li></asp:Panel>
                            <asp:Panel ID="pnlExpedienteGarantia"  runat="server" Visible="True"><li><a class="dropdown-item" href="ExpedienteVehiculoGarantia.aspx">Expediente Vehiculo Garantia / ND</a></li></asp:Panel>
                            <asp:Panel ID="pnlRepuestoRetirado"    runat="server" Visible="True"><li><a class="dropdown-item" href="RepuestosRetirados.aspx">Repuestos Retirados</a></li></asp:Panel>
                            <asp:Panel ID="pnlOCRProveedor"        runat="server" Visible="True"><li><a class="dropdown-item" href="OCRProveedor.aspx">Orden de Compra Proveedor</a></li></asp:Panel>
                            <asp:Panel ID="pnlPedidoRepuestosFBack" runat="server" Visible="True"><li><a class="dropdown-item" href="PedidoRepuestosFeedback.aspx">Subir Informacion Compra Repuesto</a></li></asp:Panel>
                        </ul>
                    </asp:Panel>
                    <li class="nav-item">
                        <a class="nav-link" href="Default.aspx">Salir</a>
                    </li>
                </ul>
                <h4 class="text-white mt-2">Bienvenido</h4>
                <span class="text-muted"><asp:Label ID="lblNombreUsuario" runat="server" Text="Usuario Conectado" /></span>
            </div>
        </div>
        <nav class="navbar navbar-dark bg-dark">
            <button class="navbar-toggler" type="button"
                data-bs-toggle="collapse" data-bs-target="#navbarToggleExternalContent"
                aria-controls="navbarToggleExternalContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
        </nav>
    </div>

    <asp:ScriptManager ID="sm1" runat="server" EnablePageMethods="true" />

    <!-- ── Grid panel ─────────────────────────────────────── -->
    <asp:Panel ID="pnlGrid" runat="server" Visible="true">

        <!-- Filter toolbar -->
        <div class="toolbar-bar">
            <button type="button" class="btn btn-sm btn-outline-secondary rango-btn" onclick="setRangoFiltro('mes')">Este mes</button>
            <button type="button" class="btn btn-sm btn-outline-secondary rango-btn" onclick="setRangoFiltro('mesant')">Mes anterior</button>
            <button type="button" class="btn btn-sm btn-outline-secondary rango-btn" onclick="setRangoFiltro('anio')">Este a&ntilde;o</button>
            <button type="button" class="btn btn-sm btn-outline-secondary rango-btn" onclick="setRangoFiltro('anioant')">A&ntilde;o anterior</button>
            <asp:TextBox ID="txtDesde" runat="server" TextMode="Date" CssClass="form-control form-control-sm" style="width:135px;" />
            <asp:TextBox ID="txtHasta" runat="server" TextMode="Date" CssClass="form-control form-control-sm" style="width:135px;" />
            <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-sm btn-primary" Text="Filtrar" />
            <span class="text-muted ms-2" style="font-size:0.75rem;"><asp:Label ID="lblRango" runat="server" /></span>
        </div>

        <div class="container-fluid px-3 pt-3">

            <!-- KPI Row -->
            <div class="d-flex gap-3 flex-wrap align-items-center mb-3">
                <div class="kpi-card">
                    <div class="kpi-label"><i class="fa fa-file-text-o"></i> Liquidaciones</div>
                    <div class="kpi-value text-primary"><asp:Label ID="lblKpiLiquidaciones" runat="server" Text="0" /></div>
                </div>
                <div class="kpi-card">
                    <div class="kpi-label"><i class="fa fa-motorcycle"></i> Total Motos</div>
                    <div class="kpi-value text-secondary"><asp:Label ID="lblKpiMotos" runat="server" Text="0" /></div>
                </div>
                <div class="kpi-card">
                    <div class="kpi-label"><i class="fa fa-money"></i> Monto Total</div>
                    <div class="kpi-value text-success"><asp:Label ID="lblKpiMonto" runat="server" Text="L 0.00" /></div>
                </div>
                <div class="ms-auto d-flex align-items-center gap-3" style="font-size:0.75rem; color:#6c757d;">
                    <span><span class="legend-dot" style="background:#c5a0e8;"></span>LIQ &ndash; Sin OC</span>
                    <span><span class="legend-dot" style="background:#ffd54f;"></span>OCS &ndash; OC Creada</span>
                    <span><span class="legend-dot" style="background:#81c784;"></span>FPS &ndash; Facturado</span>
                </div>
            </div>

            <!-- Grid card -->
            <div class="card shadow-sm mb-3">
                <div class="card-header py-2" style="background:#1a252f; color:#fff;">
                    <strong><i class="fa fa-list me-2"></i>Informe y Estatus de Liquidaciones</strong>
                </div>
                <div class="card-body p-0 card-scroll">
                    <asp:GridView ID="GridView1" runat="server"
                        CssClass="table table-sm table-bordered table-hover grid-compact mb-0"
                        AutoGenerateColumns="false" ShowFooter="true" Width="100%">
                        <HeaderStyle CssClass="thead-dark" />
                        <Columns>
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/Imagenes/edit.png"    Text="Crear PO" CommandName="Modificar" HeaderText="Crear PO"> <ControlStyle Height="28px" Width="28px" /><ItemStyle Wrap="False" /></asp:ButtonField>
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/Imagenes/pdffile.png" Text="Imprimir"  CommandName="Imprimir"  HeaderText="PDF">      <ControlStyle Height="28px" Width="28px" /><ItemStyle Wrap="False" /></asp:ButtonField>
                            <asp:BoundField DataField="ID"            HeaderText="# Liq." />
                            <asp:BoundField DataField="FECHACREACION" HeaderText="Fecha Creacion" />
                            <asp:BoundField DataField="CANTIDAD"      HeaderText="Motos"          DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="MONTO"         HeaderText="Monto"          DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="USERCODE"      HeaderText="Usuario" />
                            <asp:BoundField DataField="BPCODE"        HeaderText="Contratista" />
                            <asp:BoundField DataField="OCSAP"         HeaderText="# OC SAP" />
                            <asp:BoundField DataField="FOSAP"         HeaderText="Fecha OC" />
                            <asp:BoundField DataField="FPSAP"         HeaderText="# Factura" />
                            <asp:BoundField DataField="FFPSAP"        HeaderText="Fecha Factura" />
                            <asp:BoundField DataField="ESTATUS"       HeaderText="Estatus" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

        </div>
    </asp:Panel>

    <!-- ── Create PO panel ────────────────────────────────── -->
    <asp:Panel ID="pnlCreatePO" runat="server" Visible="false">
        <div class="container-fluid px-3 pt-3">
            <div class="row justify-content-center">
                <div class="col-md-5 col-lg-4">
                    <div class="card shadow-sm">
                        <div class="card-header py-2" style="background:#1a252f; color:#fff;">
                            <strong><i class="fa fa-shopping-cart me-2"></i>Crear Orden de Compra SAP</strong>
                        </div>
                        <div class="card-body">
                            <div class="mb-3">
                                <label class="form-label fw-semibold"># Liquidacion</label>
                                <asp:TextBox ID="txtNumeroLiquidacion" runat="server" CssClass="form-control form-control-sm" ReadOnly="true" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label fw-semibold">Codigo Contratista</label>
                                <asp:TextBox ID="txtBPCode" runat="server" CssClass="form-control form-control-sm" ReadOnly="true" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label fw-semibold">Numero Factura Proveedor</label>
                                <asp:TextBox ID="txtRefProveedor" runat="server" CssClass="form-control form-control-sm" />
                            </div>
                            <div class="d-grid gap-2">
                                <asp:Button ID="btnCrearPOSAP" runat="server" CssClass="btn btn-primary" Text="Crear Orden de Compra SAP" />
                                <asp:Button ID="btnExit" runat="server" CssClass="btn btn-secondary" Text="Cancelar / Regresar" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <footer class="text-center text-muted py-2 mt-2" style="font-size:0.72rem; border-top:1px solid #dee2e6; background:#fff;">
        WebDesign RJ &mdash; Copyright &copy; 2022 <a href="#">Movesa</a>. All rights reserved.
    </footer>

</form>

<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.bundle.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
<script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
<script src="https://cdn.datatables.net/1.13.6/js/dataTables.bootstrap5.min.js"></script>
<script src="https://cdn.datatables.net/buttons/2.4.1/js/dataTables.buttons.min.js"></script>
<script src="https://cdn.datatables.net/buttons/2.4.1/js/buttons.bootstrap5.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.10.1/jszip.min.js"></script>
<script src="https://cdn.datatables.net/buttons/2.4.1/js/buttons.html5.min.js"></script>
<script src="https://cdn.datatables.net/buttons/2.4.1/js/buttons.print.min.js"></script>
<script src="https://cdn.datatables.net/buttons/2.4.1/js/buttons.colVis.min.js"></script>

<script>
    $(function () {
        if ($('[id*=GridView1] tbody tr').length > 0) {
            $('[id*=GridView1]').DataTable({
                dom: 'Bfrtip',
                buttons: ['excelHtml5', 'pdfHtml5', 'colvis'],
                lengthMenu: [[25, 50, -1], [25, 50, 'Todos']],
                pageLength: 25,
                bFilter: true,
                bSort: true,
                bPaginate: true,
                order: [[2, 'desc']],
                columnDefs: [{ orderable: false, targets: [0, 1] }],
                language: { search: 'Buscar:', lengthMenu: 'Mostrar _MENU_', info: '_START_-_END_ de _TOTAL_' }
            });
        }
    });

    function setRangoFiltro(tipo) {
        var hoy = new Date();
        var desde, hasta;
        switch (tipo) {
            case 'mes':
                desde = new Date(hoy.getFullYear(), hoy.getMonth(), 1);
                hasta = hoy;
                break;
            case 'mesant':
                desde = new Date(hoy.getFullYear(), hoy.getMonth() - 1, 1);
                hasta = new Date(hoy.getFullYear(), hoy.getMonth(), 0);
                break;
            case 'anio':
                desde = new Date(hoy.getFullYear(), 0, 1);
                hasta = hoy;
                break;
            case 'anioant':
                desde = new Date(hoy.getFullYear() - 1, 0, 1);
                hasta = new Date(hoy.getFullYear() - 1, 11, 31);
                break;
            default: return;
        }
        var fmt = function (d) {
            return d.getFullYear() + '-' +
                   ('0' + (d.getMonth() + 1)).slice(-2) + '-' +
                   ('0' + d.getDate()).slice(-2);
        };
        document.getElementById('<%= txtDesde.ClientID %>').value = fmt(desde);
        document.getElementById('<%= txtHasta.ClientID %>').value = fmt(hasta);
        document.getElementById('<%= btnFiltrar.ClientID %>').click();
    }
</script>
</body>
</html>
