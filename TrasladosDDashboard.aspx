<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosDDashboard.aspx.vb" Inherits="TrasladosDDashboard" EnableEventValidation="false" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Planificaciones</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/buttons/1.7.1/css/buttons.dataTables.min.css" rel="stylesheet" />
    <script src='https://cdn.jsdelivr.net/npm/sweetalert2@11'></script>
    <script src="https://d3js.org/d3.v6.min.js"></script>
    <style>
        body { background: #f0f2f5; font-size: 0.85rem; }

        /* ---- Navbar ---- */
        .topnav {
            background: #1a252f;
            display: flex;
            flex-direction: row;
            align-items: center;
            flex-wrap: wrap;
            padding: 0 8px;
        }
        .topnav a {
            color: #cdd3d8;
            padding: 12px 14px;
            text-decoration: none;
            font-size: 0.88rem;
            display: inline-block;
            white-space: nowrap;
        }
        .topnav a:hover      { background: rgba(255,255,255,0.08); color: #fff; }
        .topnav a.nav-active { background: rgba(255,255,255,0.15); color: #fff; font-weight: 600; }
        .topnav a i { margin-right: 4px; }
        .topnav .brand { font-weight: 700; font-size: 0.95rem; color: #fff; padding: 12px 16px; display: flex; align-items: center; }

        /* ---- Filter bar ---- */
        .filter-bar {
            background: #fff;
            border-bottom: 1px solid #dee2e6;
            padding: 10px 16px;
            display: flex;
            align-items: center;
            flex-wrap: wrap;
            gap: 8px;
        }
        .filter-bar label { margin: 0; font-weight: 600; font-size: 0.82rem; }
        .btn-quick { font-size: 0.78rem; }
        input[type="date"] { font-size: 0.82rem; }

        /* ---- Cards ---- */
        .card { border: none; border-radius: 8px; box-shadow: 0 1px 6px rgba(0,0,0,0.08); }
        .card-header {
            background: #1a252f;
            color: #fff;
            font-weight: 600;
            font-size: 0.88rem;
            border-radius: 8px 8px 0 0 !important;
            padding: 10px 14px;
        }
        .card-header .badge-count {
            background: rgba(255,255,255,0.2);
            border-radius: 12px;
            padding: 2px 8px;
            font-size: 0.78rem;
        }

        /* ---- Tables ---- */
        .table { font-size: 0.8rem; margin-bottom: 0; }
        .table thead th {
            background: #2c3e50;
            color: #fff;
            border: none;
            padding: 8px 10px;
            white-space: nowrap;
            font-weight: 600;
            font-size: 0.78rem;
        }
        .table tbody td { padding: 6px 10px; vertical-align: middle; }
        .table-hover tbody tr:hover { background: #eaf2ff; }

        /* ---- Heatmap ---- */
        .heatmap-cell {
            font-weight: 700;
            text-align: center;
            color: #fff;
            text-shadow: 0 1px 2px rgba(0,0,0,0.4);
        }

        /* ---- Plan badge ---- */
        .plan-id  { font-weight: 700; color: #1a252f; }
        .est-abierto { background:#27ae60; color:#fff; padding:2px 8px; border-radius:10px; font-size:0.72rem; }

        /* ---- Sticky header dentro del scroll ---- */
        .card-body table thead th { position: sticky; top: 0; z-index: 2; }

        /* ---- Grids compactos sin scroll horizontal ---- */
        .grid-compact { table-layout: fixed; width: 100%; }
        .grid-compact thead th,
        .grid-compact tbody td {
            font-size: 0.72rem !important;
            padding: 4px 5px !important;
            white-space: normal !important;
            word-break: break-word;
            overflow: hidden;
        }
        .grid-compact thead th { text-align: center; }
        /* Anchos fijos por columna - gridDespachos (8 cols) */
        #gridDespachos col.c0  { width: 45px; }
        #gridDespachos col.c1  { width: 72px; }
        #gridDespachos col.c2  { width: 72px; }
        #gridDespachos col.c3  { width: 55px; }
        #gridDespachos col.c4  { width: 68px; }
        #gridDespachos col.c5  { width: 80px; }
        #gridDespachos col.c6  { width: 36px; }
        #gridDespachos col.c7  { width: 52px; }
        /* Anchos fijos - gridDespachosAbiertos (8 cols) */
        #gridDespachosAbiertos col.c0 { width: 55px; }
        #gridDespachosAbiertos col.c1 { width: 45px; }
        #gridDespachosAbiertos col.c2 { width: 72px; }
        #gridDespachosAbiertos col.c3 { width: 72px; }
        #gridDespachosAbiertos col.c4 { width: 62px; }
        #gridDespachosAbiertos col.c5 { width: 55px; }
        #gridDespachosAbiertos col.c6 { width: 62px; }
        #gridDespachosAbiertos col.c7 { width: 52px; }

        /* ---- Nombre almacen compact ---- */
        .col-alm-nombre { font-size: 0.72rem; max-width: 90px; white-space: normal; word-break: break-word; line-height: 1.2; }

        /* ---- Rango label ---- */
        #lblRango { font-size: 0.75rem; color: #6c757d; }

        /* ---- Detail grid section ---- */
        #secDetalle { display: none; }
    </style>
</head>
<body>
<form id="form1" runat="server">

<!-- ========== NAVBAR ========== -->
<nav class="topnav">
    <span class="brand"><img src="Imagenes/mnegra.png" width="18" height="18" alt="" style="vertical-align:middle; margin-right:5px;"> Movesa</span>
    <a href="TrasladosDashboardPlanner.aspx"><i class="fa fa-home"></i> Inicio</a>
    <a href="Maindashboard.aspx"><i class="fa fa-th-large"></i> Men&uacute; Principal</a>
    <a href="TrasladosCuadroBasico.aspx"><i class="fa fa-table"></i> Cuadro B&aacute;sico</a>
    <a href="TrasladosPanelProduccion.aspx"><i class="fa fa-calendar"></i> Panel Planificaci&oacute;n</a>
    <a href="TrasladosDDashboard.aspx" class="nav-active"><i class="fa fa-eye"></i> Ver Planificaci&oacute;n</a>
    <a href="TrasladosDespachos.aspx"><i class="fa fa-truck"></i> Despachos Abiertos</a>
    <a href="TrasladosDespachosAbiertos.aspx"><i class="fa fa-plus-circle"></i> Asignar Cami&oacute;n</a>
    <a href="CerrarSesion.aspx" style="margin-left:auto;"><i class="fa fa-sign-out"></i> Cerrar Sesi&oacute;n</a>
</nav>

<!-- ========== FILTER BAR ========== -->
<div class="filter-bar">
    <label>Per&iacute;odo:</label>
    <div class="btn-group btn-group-sm">
        <button type="button" class="btn btn-outline-secondary btn-quick" onclick="setRango('esteMes')">Este mes</button>
        <button type="button" class="btn btn-outline-secondary btn-quick" onclick="setRango('mesAnterior')">Mes anterior</button>
        <button type="button" class="btn btn-outline-secondary btn-quick" onclick="setRango('esteAnio')">Este a&ntilde;o</button>
        <button type="button" class="btn btn-outline-secondary btn-quick" onclick="setRango('anioAnterior')">A&ntilde;o anterior</button>
    </div>
    <div class="d-flex align-items-center" style="gap:5px;">
        <label>Desde:</label>
        <asp:TextBox ID="txtDesde" runat="server" TextMode="Date" CssClass="form-control form-control-sm" style="width:140px;" />
    </div>
    <div class="d-flex align-items-center" style="gap:5px;">
        <label>Hasta:</label>
        <asp:TextBox ID="txtHasta" runat="server" TextMode="Date" CssClass="form-control form-control-sm" style="width:140px;" />
    </div>
    <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-sm btn-dark" OnClick="btnFiltrar_Click" />
    <asp:Label ID="lblRango" runat="server" />
</div>

<!-- ========== PAGE TITLE ========== -->
<div class="container-fluid mt-3 mb-2">
    <h5 class="font-weight-bold" style="color:#1a252f;">
        <i class="fa fa-calendar-check-o mr-1"></i> Planificaciones
    </h5>
</div>

<!-- ========== TWO GRIDS ROW ========== -->
<div class="container-fluid">
    <div class="row">

        <!-- Planificaciones Abiertas -->
        <div class="col-12 col-lg-5 mb-3">
            <div class="card h-100">
                <div class="card-header d-flex align-items-center justify-content-between">
                    <span><i class="fa fa-list-alt mr-1"></i> Planificaciones Abiertas</span>
                </div>
                <div class="card-body p-0" style="overflow-y:auto; overflow-x:hidden; max-height:450px;">
                    <asp:Label ID="lblPlanId" runat="server" Text="" Visible="false" />
                    <asp:GridView ID="gridDespachos" runat="server" AutoGenerateColumns="false"
                        CssClass="table table-bordered table-hover mb-0 grid-compact" Width="100%">
                        <HeaderStyle CssClass="thead-dark" />
                        <Columns>
                            <asp:BoundField DataField="ID"               HeaderText="# Plan"      ItemStyle-CssClass="plan-id" />
                            <asp:BoundField DataField="FECHACREACION"    HeaderText="Creaci&oacute;n" />
                            <asp:BoundField DataField="FECHAVENCIMIENTO" HeaderText="Vencimiento" />
                            <asp:BoundField DataField="FALTANTE"         HeaderText="Faltante" />
                            <asp:BoundField DataField="ESTADO"           HeaderText="Estado" />
                            <asp:BoundField DataField="USUARIO"          HeaderText="Usuario" />
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/view.png"
                                CommandName="Ver" HeaderText="Ver">
                                <ControlStyle Height="26px" Width="26px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Center" />
                            </asp:ButtonField>
                            <asp:TemplateField HeaderText="Accion" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <button type="button" class="btn btn-sm btn-danger py-0"
                                        onclick='cerrarPlan(<%# Eval("ID") %>)'>Cerrar</button>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- Despachos Abiertos -->
        <div class="col-12 col-lg-7 mb-3">
            <div class="card h-100">
                <div class="card-header d-flex align-items-center justify-content-between">
                    <span><i class="fa fa-truck mr-1"></i> Despachos Abiertos</span>
                </div>
                <div class="card-body p-0" style="overflow-y:auto; overflow-x:hidden; max-height:450px;">
                    <asp:GridView ID="gridDespachosAbiertos" runat="server" AutoGenerateColumns="false"
                        CssClass="table table-bordered table-hover mb-0 grid-compact" Width="100%">
                        <HeaderStyle CssClass="thead-dark" />
                        <Columns>
                            <asp:BoundField DataField="Id Despacho"       HeaderText="# Despacho"  ItemStyle-CssClass="plan-id" />
                            <asp:BoundField DataField="PLANID"            HeaderText="# Plan" />
                            <asp:BoundField DataField="Fecha Creacion"    HeaderText="Creaci&oacute;n" />
                            <asp:BoundField DataField="Fecha Vencimiento" HeaderText="Vencimiento" />
                            <asp:BoundField DataField="ESTADO"            HeaderText="Estado" />
                            <asp:BoundField DataField="Almacenes"         HeaderText="Almacenes" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Tot Faltante"      HeaderText="Tot. Faltante" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="Accion" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <button type="button" class="btn btn-sm btn-danger py-0"
                                        onclick='cerrarDespacho(<%# Eval("PLANID") %>)'>Cerrar</button>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

    </div><!-- /row -->

    <!-- ========== DETAIL GRID ========== -->
    <div id="secDetalle" class="row mb-4">
        <div class="col-12">
            <div class="card">
                <div class="card-header d-flex align-items-center justify-content-between">
                    <span><i class="fa fa-table mr-1"></i> Detalle del Plan <span id="lblDetalleId" class="badge-count ml-1"></span></span>
                    <button type="button" class="btn btn-sm btn-outline-light" onclick="document.getElementById('secDetalle').style.display='none'">
                        <i class="fa fa-times"></i>
                    </button>
                </div>
                <div class="card-body p-0" style="overflow-x:auto;">
                    <asp:GridView ID="gridAlmacenesDespachos" runat="server" AutoGenerateColumns="false"
                        CssClass="table table-bordered table-hover mb-0" Width="100%"
                        OnRowDataBound="gridIndiceD_RowDataBound">
                        <HeaderStyle CssClass="thead-dark" />
                        <Columns>
                            <asp:BoundField DataField="Ruta"             HeaderText="Ruta" />
                            <asp:BoundField DataField="Canal"            HeaderText="Canal" />
                            <asp:BoundField DataField="CRanking"         HeaderText="R Rank" />
                            <asp:BoundField DataField="CIndice"          HeaderText="R Desab" />
                            <asp:BoundField DataField="Leyenda"          HeaderText="Leyenda" />
                            <asp:BoundField DataField="Ranking"          HeaderText="Ranking" />
                            <asp:BoundField DataField="Codigo"           HeaderText="Almacen" />
                            <asp:BoundField DataField="Almacen"          HeaderText="Nombre" ItemStyle-CssClass="col-alm-nombre" />
                            <asp:BoundField DataField="Indice"           HeaderText="&Iacute;ndice"    ItemStyle-CssClass="heatmap-cell" />
                            <asp:BoundField DataField="Indice_Proyectado" HeaderText="&Iacute;nd.Proy" ItemStyle-CssClass="heatmap-cell" />
                            <asp:BoundField DataField="Cuadro"           HeaderText="CB" />
                            <asp:BoundField DataField="COMP"             HeaderText="Comp" />
                            <asp:BoundField DataField="Despacho"         HeaderText="Desp" />
                            <asp:BoundField DataField="SOL"              HeaderText="Sol" />
                            <asp:BoundField DataField="Transito"         HeaderText="Tr&aacute;n" />
                            <asp:BoundField DataField="Fisico"           HeaderText="F&iacute;s" />
                            <asp:BoundField DataField="Faltante"         HeaderText="Faltante" />
                            <asp:BoundField DataField="UNDS"             HeaderText="UV" />
                            <asp:BoundField DataField="Headerid"         HeaderText="# Plan" />
                            <asp:TemplateField HeaderText="Sel" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbDocument" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

</div><!-- /container-fluid -->

<footer class="text-center text-muted py-3" style="font-size:0.72rem;">
    WebDesign RJ &copy; 2022 Movesa. All rights reserved.
</footer>

<asp:ScriptManager ID="sm1" runat="server" />

<!-- ========== SCRIPTS ========== -->
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
<script src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
<script src="https://cdn.datatables.net/buttons/1.7.1/js/dataTables.buttons.min.js"></script>
<script src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.html5.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>

<script>
    // ---- Rangos rapidos ----
    function fmtDate(d) {
        var mm = ('0' + (d.getMonth() + 1)).slice(-2);
        var dd = ('0' + d.getDate()).slice(-2);
        return d.getFullYear() + '-' + mm + '-' + dd;
    }
    function setRango(tipo) {
        var hoy = new Date(), y = hoy.getFullYear(), m = hoy.getMonth();
        var desde, hasta;
        if      (tipo === 'esteMes')     { desde = new Date(y, m, 1);   hasta = hoy; }
        else if (tipo === 'mesAnterior') { desde = new Date(y, m-1, 1); hasta = new Date(y, m, 0); }
        else if (tipo === 'esteAnio')    { desde = new Date(y, 0, 1);   hasta = hoy; }
        else                            { desde = new Date(y-1, 0, 1); hasta = new Date(y-1, 11, 31); }
        document.getElementById('<%= txtDesde.ClientID %>').value = fmtDate(desde);
        document.getElementById('<%= txtHasta.ClientID %>').value = fmtDate(hasta);
        document.getElementById('<%= btnFiltrar.ClientID %>').click();
    }

    // ---- DataTables ----
    $(document).ready(function () {
        $('#<%= gridDespachosAbiertos.ClientID %>').DataTable({
            dom: 'Bfrtip',
            buttons: [{ extend: 'excelHtml5', text: 'Excel', title: 'Despachos Abiertos' }],
            bPaginate: false,
            ordering: true,
            order: [[0, 'desc']]
        });
        $('#<%= gridDespachos.ClientID %>').DataTable({
            dom: 'frtip',
            bPaginate: false,
            ordering: true,
            order: [[0, 'desc']]
        });
    });

    // ---- Heatmap coloring ----
    $(document).ready(function () {
        var cells = document.querySelectorAll('.heatmap-cell');
        var vals  = Array.from(cells, function (c) { return parseFloat(c.innerText) || 0; });
        var scale = d3.scaleSequential().domain(d3.extent(vals)).interpolator(d3.interpolateOranges);
        cells.forEach(function (c) { c.style.backgroundColor = scale(parseFloat(c.innerText) || 0); });
    });

    // ---- Estado badge color ----
    $(document).ready(function () {
        $('#<%= gridDespachos.ClientID %> tbody tr, #<%= gridDespachosAbiertos.ClientID %> tbody tr').each(function () {
            $(this).find('td').each(function () {
                var txt = $(this).text().trim();
                if (txt === 'ABIERTO')  $(this).html('<span class="est-abierto">ABIERTO</span>');
            });
        });
    });

    // ---- cerrarPlan / cerrarDespacho ----
    function cerrarPlan(idPlan) {
        Swal.fire({
            title: '&iquest;Cerrar esta planificaci&oacute;n?',
            text: 'Esta acci&oacute;n no se puede deshacer.',
            icon: 'warning', showCancelButton: true,
            confirmButtonText: 'S&iacute;, cerrar', cancelButtonText: 'Cancelar'
        }).then(function (r) {
            if (r.isConfirmed) {
                $.ajax({
                    url: 'CerrarPlanificacionHandler.ashx', type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({ id: idPlan }),
                    success: function (res) {
                        if (res.success) Swal.fire('Cerrado', 'Planificaci&oacute;n cerrada.', 'success').then(function () { location.reload(); });
                        else Swal.fire('Error', res.message, 'error');
                    },
                    error: function () { Swal.fire('Error', 'No se pudo completar.', 'error'); }
                });
            }
        });
    }
    function cerrarDespacho(idPlan) {
        Swal.fire({
            title: '&iquest;Cerrar este despacho?',
            text: 'Esta acci&oacute;n no se puede deshacer.',
            icon: 'warning', showCancelButton: true,
            confirmButtonText: 'S&iacute;, cerrar', cancelButtonText: 'Cancelar'
        }).then(function (r) {
            if (r.isConfirmed) {
                $.ajax({
                    url: 'CerrarDespachoHandler.ashx', type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({ id: idPlan }),
                    success: function (res) {
                        if (res.success) Swal.fire('Cerrado', 'Despacho cerrado.', 'success').then(function () { location.reload(); });
                        else Swal.fire('Error', res.message, 'error');
                    },
                    error: function () { Swal.fire('Error', 'No se pudo completar.', 'error'); }
                });
            }
        });
    }
</script>

</form>
</body>
</html>
