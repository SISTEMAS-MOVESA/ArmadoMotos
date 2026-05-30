<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VentasU7DSankey.aspx.vb" Inherits="VentasU7DSankey" ResponseEncoding="utf-8" ContentType="text/html" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Sankey &#8211; Modelos vs Notas</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css" />
    <style>
        body { background: #f4f6f9; font-size: 0.85rem; }
        .navbar-brand { font-weight: 700; letter-spacing: 1px; }
        .sankey-wrap { display: flex; gap: 12px; height: calc(100vh - 185px); }
        #chart_div  { flex: 1; height: 100%; overflow: hidden; }
        #legend_div { width: 200px; height: 100%; overflow-y: auto; border-left: 1px solid #e9ecef; padding-left: 12px; flex-shrink: 0; }
        .leg-item   { display: flex; align-items: center; gap: 6px; margin-bottom: 8px; }
        .leg-swatch { width: 12px; height: 12px; border-radius: 2px; flex-shrink: 0; }
        .leg-name   { flex: 1; font-size: 0.73rem; line-height: 1.25; word-break: break-word; color: #333; }
        .leg-pct    { font-size: 0.73rem; font-weight: 700; color: #1a252f; white-space: nowrap; }
        .leg-bar    { height: 4px; border-radius: 2px; margin-top: 2px; }
        .leg-title  { font-size: 0.75rem; font-weight: 700; color: #1a252f; margin-bottom: 10px; text-transform: uppercase; letter-spacing: 0.5px; }
        .filter-bar { display: flex; align-items: center; gap: 10px; flex-wrap: wrap; }
        .filter-sep { border-top: 1px solid #e9ecef; margin: 8px 0; }
        #nodata     { display: none; }
        input[type="date"] { font-size: 0.82rem; }
        .btn-quick  { font-size: 0.78rem; }
        #chart_div svg text { transition: font-size 0.15s, font-weight 0.15s; cursor: default; }
        #chart_div svg text:hover { font-size: 15px !important; font-weight: 700 !important; }
        .nav-tabs   { border-bottom: 2px solid #dee2e6; }
        .nav-tabs .nav-link.active { font-weight: 600; background: #fff; border-bottom-color: #fff; }
    </style>
</head>
<body>
<form id="form1" runat="server">

<nav class="navbar navbar-dark" style="background:#1a252f;">
    <span class="navbar-brand" id="navTitle">&#9906; Flujo &#8594; Notas</span>
    <a href="VentasU7DKpi.aspx" class="btn btn-sm btn-outline-light ml-auto">&#8592; KPI</a>
</nav>

<div class="container-fluid mt-2">

    <!-- Filtros -->
    <div class="card mb-0 px-3 py-2" style="border-radius:6px 6px 0 0; border-bottom:0;">

        <!-- Fila 1: Vista + Estado -->
        <div class="filter-bar mb-2">
            <span class="font-weight-bold mr-1">Vista:</span>
            <div class="btn-group btn-group-sm mr-3" id="btnVista">
                <button type="button" class="btn btn-dark active" data-vista="modelo">Por Modelo</button>
                <button type="button" class="btn btn-outline-dark" data-vista="almacen">Por Almac&#233;n</button>
            </div>
            <span class="font-weight-bold mr-1">Mostrar:</span>
            <div class="btn-group btn-group-sm" id="btnFiltro">
                <button type="button" class="btn btn-primary active"  data-filtro="todos">Todos</button>
                <button type="button" class="btn btn-outline-success" data-filtro="resueltos">Solo Resueltos</button>
                <button type="button" class="btn btn-outline-danger"  data-filtro="pendientes">Solo Pendientes</button>
            </div>
            <span class="ml-auto text-muted" style="font-size:0.73rem;">
                <asp:Label ID="lblActualizado" runat="server" />
            </span>
        </div>

        <div class="filter-sep"></div>

        <!-- Fila 2: Rango de fechas -->
        <div class="filter-bar">
            <span class="font-weight-bold mr-1">Per&#237;odo:</span>
            <div class="btn-group btn-group-sm">
                <button type="button" class="btn btn-outline-secondary btn-quick" onclick="setRango('esteMes')">Este mes</button>
                <button type="button" class="btn btn-outline-secondary btn-quick" onclick="setRango('mesAnterior')">Mes anterior</button>
                <button type="button" class="btn btn-outline-secondary btn-quick" onclick="setRango('esteAnio')">Este a&#241;o</button>
                <button type="button" class="btn btn-outline-secondary btn-quick" onclick="setRango('anioAnterior')">A&#241;o anterior</button>
            </div>
            <div class="d-flex align-items-center" style="gap:6px;">
                <label class="mb-0">Desde:</label>
                <asp:TextBox ID="txtDesde" runat="server" TextMode="Date" CssClass="form-control form-control-sm" style="width:140px;" />
            </div>
            <div class="d-flex align-items-center" style="gap:6px;">
                <label class="mb-0">Hasta:</label>
                <asp:TextBox ID="txtHasta" runat="server" TextMode="Date" CssClass="form-control form-control-sm" style="width:140px;" />
            </div>
            <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-sm btn-dark" OnClick="btnFiltrar_Click" />
            <asp:Label ID="lblRango" runat="server" CssClass="text-muted ml-1" style="font-size:0.75rem;" />
        </div>

    </div>

    <!-- Sankey -->
    <div class="card p-3" style="border-radius:0 0 6px 6px;">
        <div class="sankey-wrap">
            <div id="chart_div"></div>
            <div id="legend_div"></div>
        </div>
        <div id="nodata" class="text-center text-muted p-5">Sin datos para el filtro seleccionado.</div>
    </div>

</div>

<!-- Datos serializados desde servidor -->
<asp:Literal ID="litData" runat="server" />

<script src="https://cdn.jsdelivr.net/npm/jquery@3.6.0/dist/jquery.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
<script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
<script type="text/javascript">
    google.charts.load('current', { packages: ['sankey'] });
    google.charts.setOnLoadCallback(function () { dibujar(); });

    var _vista   = 'modelo';
    var _filtro  = 'todos';
    var _palette = ['#3366CC','#DC3912','#FF9900','#109618','#990099',
                    '#0099C6','#DD4477','#66AA00','#B82E2E','#316395',
                    '#994499','#22AA99','#AAAA11','#6633CC','#E67300'];

    // ---- Rangos rapidos ----
    function fmtDate(d) {
        var mm = ('0' + (d.getMonth() + 1)).slice(-2);
        var dd = ('0' + d.getDate()).slice(-2);
        return d.getFullYear() + '-' + mm + '-' + dd;
    }
    function setRango(tipo) {
        var hoy = new Date(), y = hoy.getFullYear(), m = hoy.getMonth();
        var desde, hasta;
        if      (tipo === 'esteMes')      { desde = new Date(y, m, 1);     hasta = hoy; }
        else if (tipo === 'mesAnterior')  { desde = new Date(y, m-1, 1);   hasta = new Date(y, m, 0); }
        else if (tipo === 'esteAnio')     { desde = new Date(y, 0, 1);     hasta = hoy; }
        else                              { desde = new Date(y-1, 0, 1);   hasta = new Date(y-1, 11, 31); }
        document.getElementById('<%= txtDesde.ClientID %>').value = fmtDate(desde);
        document.getElementById('<%= txtHasta.ClientID %>').value = fmtDate(hasta);
        document.getElementById('<%= btnFiltrar.ClientID %>').click();
    }

    // ---- Sankey ----
    function dibujar() {
        var dataset = (_vista === 'almacen') ? window._sankeyAlm : window._sankey;
        var filas   = dataset ? dataset[_filtro] : null;

        if (!filas || filas.length === 0) {
            document.getElementById('chart_div').style.display = 'none';
            document.getElementById('nodata').style.display = 'block';
            document.getElementById('legend_div').innerHTML = '';
            return;
        }
        document.getElementById('chart_div').style.display = 'block';
        document.getElementById('nodata').style.display = 'none';

        var colLabel = (_vista === 'almacen') ? 'Almacén' : 'Modelo';

        var data = new google.visualization.DataTable();
        data.addColumn('string', colLabel);
        data.addColumn('string', 'Nota');
        data.addColumn('number', 'Casos');
        data.addRows(ordenarFilas(filas));

        var h = document.getElementById('chart_div').offsetHeight;
        var options = {
            height: h,
            sankey: {
                node: { width: 20, nodePadding: 14, label: { fontSize: 13, color: '#1a252f', bold: false }, interactivity: true },
                link: { colorMode: 'gradient' }
            },
            tooltip: { isHtml: true }
        };

        var chart = new google.visualization.Sankey(document.getElementById('chart_div'));
        chart.draw(data, options);
        buildLegend(filas);
    }

    function ordenarFilas(filas) {
        var leftTotals = {}, noteTotals = {};
        filas.forEach(function (r) {
            leftTotals[r[0]] = (leftTotals[r[0]] || 0) + r[2];
            noteTotals[r[1]] = (noteTotals[r[1]] || 0) + r[2];
        });
        var lefts = Object.keys(leftTotals).sort(function (a, b) { return leftTotals[b] - leftTotals[a]; });
        var notes = Object.keys(noteTotals).sort(function (a, b) { return noteTotals[b] - noteTotals[a]; });
        var lookup = {};
        filas.forEach(function (r) { lookup[r[0] + '|||' + r[1]] = r[2]; });
        var sorted = [];
        lefts.forEach(function (l) {
            notes.forEach(function (n) {
                var v = lookup[l + '|||' + n];
                if (v) sorted.push([l, n, v]);
            });
        });
        return sorted;
    }

    function buildLegend(filas) {
        var totals = {}, order = [];
        filas.forEach(function (r) {
            if (!totals[r[1]]) { totals[r[1]] = 0; order.push(r[1]); }
            totals[r[1]] += r[2];
        });
        var grand = Object.keys(totals).reduce(function (s, k) { return s + totals[k]; }, 0);
        var sorted = order
            .map(function (k) { return { nota: k, cnt: totals[k], pct: grand > 0 ? totals[k] / grand * 100 : 0 }; })
            .sort(function (a, b) { return b.cnt - a.cnt; });
        var maxCnt = sorted.length > 0 ? sorted[0].cnt : 1;
        var html = '<div class="leg-title">% por Nota</div>';
        sorted.forEach(function (item, i) {
            var color = _palette[i % _palette.length];
            var barW  = Math.round(item.cnt / maxCnt * 100);
            html += '<div class="leg-item">' +
                    '<div class="leg-swatch" style="background:' + color + '"></div>' +
                    '<div class="leg-name">' + item.nota +
                        '<div class="leg-bar" style="width:' + barW + '%;background:' + color + '66;"></div>' +
                    '</div>' +
                    '<div class="leg-pct">' + item.pct.toFixed(1) + '%</div>' +
                    '</div>';
        });
        document.getElementById('legend_div').innerHTML = html;
    }

    // ---- Toggle vista ----
    $('#btnVista button').on('click', function () {
        _vista = $(this).data('vista');
        $('#btnVista button').removeClass('active btn-dark').addClass('btn-outline-dark');
        $(this).removeClass('btn-outline-dark').addClass('btn-dark active');
        var label = _vista === 'almacen' ? 'Flujo Almac&#233;n &#8594; Notas' : 'Flujo Modelo &#8594; Notas';
        document.getElementById('navTitle').innerHTML = '&#9906; ' + label;
        dibujar();
    });

    // ---- Toggle estado ----
    $('#btnFiltro button').on('click', function () {
        _filtro = $(this).data('filtro');
        $('#btnFiltro button').removeClass('active btn-primary btn-success btn-danger')
            .addClass(function () {
                var f = $(this).data('filtro');
                return f === 'resueltos' ? 'btn-outline-success' : f === 'pendientes' ? 'btn-outline-danger' : 'btn-outline-primary';
            });
        $(this).removeClass('btn-outline-primary btn-outline-success btn-outline-danger').addClass(function () {
            var f = $(this).data('filtro');
            return f === 'resueltos' ? 'btn-success active' : f === 'pendientes' ? 'btn-danger active' : 'btn-primary active';
        });
        dibujar();
    });
</script>
</form>
</body>
</html>
