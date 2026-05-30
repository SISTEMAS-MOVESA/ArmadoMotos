<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProcesoContable.aspx.vb" Inherits="ProcesoContable" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Crear Liquidaciones</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/dataTables.bootstrap5.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/2.4.1/css/buttons.bootstrap5.min.css" />
    <link rel="stylesheet" href="css/style.css" />
    <style>
        body { background: #f4f6f9; font-size: 0.85rem; }
        /* ── KPI cards ── */
        .kpi-card { background: #fff; border-radius: 8px; padding: 12px 20px; box-shadow: 0 1px 4px rgba(0,0,0,.1); min-width: 160px; }
        .kpi-card-btn { cursor: pointer; transition: box-shadow .15s, background .15s; user-select: none; }
        .kpi-card-btn:hover { box-shadow: 0 2px 8px rgba(220,53,69,.35); }
        .kpi-card-btn.active { background: #fff5f5; box-shadow: 0 0 0 2px #dc3545; }
        .kpi-label { font-size: 0.68rem; text-transform: uppercase; color: #6c757d; font-weight: 600; letter-spacing: .5px; margin-bottom: 2px; }
        .kpi-value { font-size: 1.7rem; font-weight: 700; line-height: 1.1; }
        /* ── toolbar ── */
        .toolbar-bar { background: #fff; border-bottom: 1px solid #dee2e6; padding: 8px 16px; display: flex; align-items: center; flex-wrap: wrap; gap: 6px; }
        /* ── grid ── */
        .grid-compact thead th, .grid-compact tbody td { font-size: 0.78rem !important; padding: 4px 6px !important; vertical-align: middle !important; }
        .grid-compact thead th { background: #343a40 !important; color: #fff !important; white-space: nowrap; }
        .card-scroll { overflow-x: auto; }
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
    <asp:HiddenField ID="hfToken"     runat="server" />
    <asp:HiddenField ID="hfSinPrecio" runat="server" Value="0" />

    <!-- ── Toolbar ───────────────────────────────────────── -->
    <div class="toolbar-bar">
        <asp:DropDownList ID="drpContratista" runat="server"
            CssClass="form-select form-select-sm" style="width:220px;" AutoPostBack="true" />
        <asp:Button ID="btnCrearLiquidacion" runat="server" CssClass="btn btn-sm btn-success"
            Text="Crear Liquidacion" OnClientClick="return confirmarLiquidacion();" />
        <asp:Button ID="btnVerPrecendentes" runat="server" CssClass="btn btn-sm btn-outline-secondary"
            Text="Ver Liquidaciones Anteriores" />
        <asp:Button ID="btnBack" runat="server" CssClass="btn btn-sm btn-outline-primary"
            Text="Regresar" Visible="false" />
    </div>

    <div class="container-fluid px-3 pt-3">

        <!-- ── KPI Row ────────────────────────────────────── -->
        <div class="d-flex gap-3 flex-wrap mb-3">
            <div class="kpi-card">
                <div class="kpi-label"><i class="fa fa-motorcycle"></i> Motos Pendientes</div>
                <div class="kpi-value text-primary"><asp:Label ID="lblKpiTotal" runat="server" Text="0" /></div>
            </div>
            <div class="kpi-card">
                <div class="kpi-label"><i class="fa fa-money"></i> Monto a Pagar</div>
                <div class="kpi-value text-success"><asp:Label ID="lblKpiMonto" runat="server" Text="L 0.00" /></div>
            </div>
            <div class="kpi-card kpi-card-btn border border-danger" id="kpiSinPrecioCard"
                 onclick="toggleFiltroSinPrecio(this)" title="Clic para filtrar motos sin precio">
                <div class="kpi-label text-danger">
                    <i class="fa fa-exclamation-triangle"></i> Sin Precio Armado
                    <span class="badge bg-danger ms-1" id="badgeFiltroActivo" style="display:none; font-size:0.6rem;">FILTRADO</span>
                </div>
                <div class="kpi-value text-danger"><asp:Label ID="lblKpiSinPrecio" runat="server" Text="0" /></div>
                <div style="font-size:0.63rem; color:#dc3545; margin-top:3px; opacity:.7;">
                    <i class="fa fa-filter"></i> Clic para filtrar
                </div>
            </div>
        </div>

        <!-- ── Grids ──────────────────────────────────────── -->
        <asp:Panel ID="pnlGridControlCalidad" runat="server" Visible="true">

            <!-- Pendientes -->
            <div class="card shadow-sm mb-3">
                <div class="card-header py-2" style="background:#1a252f; color:#fff;">
                    <strong><i class="fa fa-list me-2"></i>Liquidaciones Pendientes</strong>
                </div>
                <div class="card-body p-0 card-scroll">
                    <asp:GridView ID="GridView1" runat="server"
                        CssClass="table table-sm table-bordered table-hover grid-compact mb-0"
                        AutoGenerateColumns="false" ShowFooter="true" Width="100%">
                        <HeaderStyle CssClass="thead-dark" />
                        <Columns>
                            <asp:BoundField DataField="ID"       HeaderText="ID" />
                            <asp:BoundField DataField="SERIE"    HeaderText="Serie" />
                            <asp:BoundField DataField="MODELO"   HeaderText="Modelo" />
                            <asp:BoundField DataField="COLOR"    HeaderText="Color" />
                            <asp:BoundField DataField="Mecanico" HeaderText="Cod. Mecanico" />
                            <asp:BoundField DataField="BPCODE"   HeaderText="Cod. Proveedor" />
                            <asp:BoundField DataField="Cuenta"   HeaderText="Cuenta Contable" />
                            <asp:BoundField DataField="Precio"   HeaderText="Precio Armado"
                                DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <!-- Precedentes -->
            <div class="card shadow-sm mb-3">
                <div class="card-header py-2" style="background:#1a252f; color:#fff;">
                    <strong><i class="fa fa-history me-2"></i>Liquidaciones Anteriores</strong>
                </div>
                <div class="card-body p-0 card-scroll">
                    <asp:GridView ID="gridPrecedentes" runat="server"
                        CssClass="table table-sm table-bordered table-hover grid-compact mb-0"
                        AutoGenerateColumns="false" ShowFooter="true" Visible="false" Width="100%">
                        <HeaderStyle CssClass="thead-dark" />
                        <Columns>
                            <asp:BoundField DataField="DATECREATED"      HeaderText="Fecha" />
                            <asp:BoundField DataField="LIQUIDACIONID"    HeaderText="# Liquidacion" />
                            <asp:BoundField DataField="SERIE"            HeaderText="Serie" />
                            <asp:BoundField DataField="MODELO"           HeaderText="Modelo" />
                            <asp:BoundField DataField="COLOR"            HeaderText="Color" />
                            <asp:BoundField DataField="GRUPORESPONSABLE" HeaderText="Contratista" />
                            <asp:BoundField DataField="MECANICONAME"     HeaderText="Mecanico" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

        </asp:Panel>
    </div>

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
    var dtTable       = null;
    var filterActivo  = false;

    // ── DataTables custom filter (sin precio) ──────────────────────────────
    $.fn.dataTable.ext.search.push(function (settings, data, dataIndex) {
        if (!filterActivo) return true;
        // Precio NULL (modelo sin entrada en tabla de precios) = celda vacia en el grid
        // 0.00 = configurado en BD aunque sea cero, NO es sin precio
        var precio = data[7].replace(/ |\s/g, '');
        return precio === '';
    });

    $(function () {
        var dtOpts = {
            dom: 'Bfrtip',
            buttons: ['excelHtml5', 'pdfHtml5', 'colvis'],
            lengthMenu: [[50, -1], [50, 'Todos']],
            pageLength: 50,
            bFilter: true,
            bSort: false,
            bPaginate: true,
            language: { search: 'Buscar:', lengthMenu: 'Mostrar _MENU_', info: '_START_-_END_ de _TOTAL_' }
        };
        if ($('[id*=GridView1] tbody tr').length > 0) {
            dtTable = $('[id*=GridView1]').DataTable(dtOpts);
        }
        if ($('[id*=gridPrecedentes]:visible tbody tr').length > 0) {
            $('[id*=gridPrecedentes]').DataTable(dtOpts);
        }
    });

    function toggleFiltroSinPrecio(card) {
        filterActivo = !filterActivo;
        var $card  = $(card);
        var $badge = $('#badgeFiltroActivo');

        if (filterActivo) {
            $card.addClass('active');
            $badge.show();
        } else {
            $card.removeClass('active');
            $badge.hide();
        }

        if (dtTable) {
            dtTable.draw();
        } else {
            // fallback cuando DataTables no está inicializado (grid vacío)
            $('[id*=GridView1] tbody tr').each(function () {
                var precio = $.trim($(this).find('td').eq(7).text()).replace(/ /g, '');
                var sinPrecio = precio === '' || parseFloat(precio.replace(/,/g, '')) === 0;
                $(this).toggle(!filterActivo || sinPrecio);
            });
        }
    }

    function confirmarLiquidacion() {
        var total     = document.getElementById('<%= lblKpiTotal.ClientID %>').innerText;
        var monto     = document.getElementById('<%= lblKpiMonto.ClientID %>').innerText;
        var sinPrecio = parseInt(document.getElementById('<%= hfSinPrecio.ClientID %>').value) || 0;

        if (sinPrecio > 0) {
            Swal.fire({
                icon: 'error',
                title: 'No se puede crear la liquidacion',
                html: '<strong>' + sinPrecio + '</strong> moto(s) no tienen precio de armado configurado.<br><br>' +
                      'Use el boton <span class="badge bg-danger">SIN PRECIO ARMADO</span> para identificarlas, ' +
                      'configure los precios en <strong>Parametrizaciones &rarr; Precios Armado</strong> y vuelva a intentarlo.',
                confirmButtonText: 'Entendido',
                confirmButtonColor: '#dc3545'
            });
            return false;
        }

        Swal.fire({
            title: 'Confirmar Liquidacion',
            html: 'Se procesaran <strong>' + total + '</strong> moto(s) por un total de <strong>' + monto + '</strong>.',
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: '<i class="fa fa-check"></i> Crear',
            cancelButtonText: 'Cancelar',
            confirmButtonColor: '#198754',
            cancelButtonColor: '#6c757d'
        }).then(function (result) {
            if (result.isConfirmed) {
                var btn = document.getElementById('<%= btnCrearLiquidacion.ClientID %>');
                btn.disabled = true;
                btn.value = 'Procesando...';
                __doPostBack('<%= btnCrearLiquidacion.UniqueID %>', '');
            }
        });
        return false;
    }
</script>
</body>
</html>
