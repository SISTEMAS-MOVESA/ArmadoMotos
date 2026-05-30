<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VentasU7DKpi.aspx.vb" Inherits="VentasU7DKpi" ResponseEncoding="utf-8" ContentType="text/html" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>KPI Reabasto &#8211; Cobertura por Nota</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css" />
    <style>
        body { background: #f4f6f9; font-size: 0.85rem; }
        .navbar-brand { font-weight: 700; letter-spacing: 1px; }
        .kpi-card { border-radius: 10px; padding: 18px 22px; color: #fff; min-width: 140px; }
        .kpi-card .kpi-val { font-size: 2.2rem; font-weight: 700; line-height: 1; }
        .kpi-card .kpi-lbl { font-size: 0.75rem; opacity: 0.85; margin-top: 4px; }
        .kpi-total  { background: #2c3e50; }
        .kpi-res    { background: #27ae60; }
        .kpi-pend   { background: #e74c3c; }
        .kpi-cob    { background: #2980b9; }
        .kpi-notas  { background: #8e44ad; }
        .kpi-sinnota{ background: #d35400; }
        .progress   { height: 10px; border-radius: 5px; }
        .nav-tabs .nav-link.active { font-weight: 600; }
        table { font-size: 0.82rem; }
        th { background: #1a252f; color: #fff; white-space: nowrap; vertical-align: middle !important; }
        td { vertical-align: middle !important; }
        .pct-ok   { background: #d4edda; color: #155724; font-weight: 600; }
        .pct-warn { background: #fff3cd; color: #856404; font-weight: 600; }
        .pct-bad  { background: #f8d7da; color: #721c24; font-weight: 600; }
        .pend-hi  { background: #f8d7da !important; }
        .nota-sin { color: #aaa; font-style: italic; }
        .pareto-1  { background: #d4edda !important; color: #155724; font-weight: 700; }
        .pareto-2  { background: #cce5ff !important; color: #004085; font-weight: 700; }
        .pareto-3  { background: #fff3cd !important; color: #856404; font-weight: 700; }
        .pareto-4  { background: #f8d7da !important; color: #721c24; font-weight: 700; }
        .pareto-0  { background: #e2e3e5 !important; color: #383d41; }
        .bar-wrap  { background: #e9ecef; border-radius: 4px; height: 14px; min-width: 80px; }
        .bar-fill  { height: 14px; border-radius: 4px; }
        .dias-2   { background: #f8d7da !important; }
        .dias-1   { background: #fff3cd !important; }
        .table-hover tbody tr:hover td { filter: brightness(0.96); }
        .refresh-bar { display: flex; align-items: center; gap: 10px; }
        #lblUltAct { font-size: 0.75rem; color: #aaa; }
        .filter-bar { display: flex; align-items: center; gap: 8px; flex-wrap: wrap; padding: 10px 16px; background:#fff; border-bottom: 1px solid #dee2e6; }
        .filter-bar label { margin-bottom:0; font-weight:600; font-size:0.82rem; }
        .btn-quick { font-size: 0.78rem; }
        input[type="date"] { font-size: 0.82rem; }
        #lblRangoAct { font-size:0.78rem; color:#6c757d; }
    </style>
</head>
<body>
<form id="form1" runat="server">

<nav class="navbar navbar-dark" style="background:#1a252f;">
    <span class="navbar-brand">&#128202; Cobertura por Nota &#8211; Ventas</span>
    <div class="refresh-bar ml-auto">
        <a href="MainDashBoard.aspx" class="btn btn-sm btn-outline-light">&#8592; Dashboard</a>
        <a href="VentasU7DSankey.aspx" class="btn btn-sm btn-outline-light">&#9906; Ver Sankey</a>
        <span id="lblUltAct"><asp:Label ID="lblActualizado" runat="server" /></span>
        <asp:Button ID="btnRefresh" runat="server" Text="&#8635; Actualizar"
            CssClass="btn btn-sm btn-outline-light" OnClick="btnRefresh_Click" />
    </div>
</nav>

<!-- Barra de filtros de fecha -->
<div class="filter-bar">
    <label>Per&#237;odo:</label>
    <div class="btn-group btn-group-sm mr-1">
        <button type="button" class="btn btn-outline-secondary btn-quick" onclick="setRango('esteMes')">Este mes</button>
        <button type="button" class="btn btn-outline-secondary btn-quick" onclick="setRango('mesAnterior')">Mes anterior</button>
        <button type="button" class="btn btn-outline-secondary btn-quick" onclick="setRango('esteAnio')">Este a&#241;o</button>
        <button type="button" class="btn btn-outline-secondary btn-quick" onclick="setRango('anioAnterior')">A&#241;o anterior</button>
    </div>
    <label class="ml-2">Desde:</label>
    <asp:TextBox ID="txtDesde" runat="server" TextMode="Date" CssClass="form-control form-control-sm" style="width:140px;" />
    <label>Hasta:</label>
    <asp:TextBox ID="txtHasta" runat="server" TextMode="Date" CssClass="form-control form-control-sm" style="width:140px;" />
    <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-sm btn-dark" OnClick="btnFiltrar_Click" />
    <span id="lblRangoAct"><asp:Label ID="lblRango" runat="server" /></span>
</div>

<div class="container-fluid mt-3">

    <!-- KPI Cards -->
    <div class="d-flex flex-wrap mb-3" style="gap:12px;">
        <div class="kpi-card kpi-total">
            <div class="kpi-val"><asp:Label ID="lblTotal" runat="server" Text="—" /></div>
            <div class="kpi-lbl">Total Facturas (7d)</div>
        </div>
        <div class="kpi-card kpi-res">
            <div class="kpi-val"><asp:Label ID="lblResueltas" runat="server" Text="—" /></div>
            <div class="kpi-lbl">Resueltas</div>
        </div>
        <div class="kpi-card kpi-pend">
            <div class="kpi-val"><asp:Label ID="lblPendientes" runat="server" Text="—" /></div>
            <div class="kpi-lbl">Pendientes</div>
        </div>
        <div class="kpi-card kpi-cob" style="min-width:200px;">
            <div class="kpi-val"><asp:Label ID="lblCobertura" runat="server" Text="—" />%</div>
            <div class="kpi-lbl">% Cobertura Global</div>
            <div class="progress mt-2">
                <div class="progress-bar bg-light" id="progCobertura" runat="server" role="progressbar" style="width:0%"></div>
            </div>
        </div>
        <div class="kpi-card kpi-notas">
            <div class="kpi-val"><asp:Label ID="lblNotasUsadas" runat="server" Text="—" /></div>
            <div class="kpi-lbl">Notas distintas</div>
        </div>
        <div class="kpi-card kpi-sinnota">
            <div class="kpi-val"><asp:Label ID="lblSinNota" runat="server" Text="—" /></div>
            <div class="kpi-lbl">Pendientes sin Nota</div>
        </div>
    </div>

    <!-- Tabs -->
    <ul class="nav nav-tabs" id="mainTabs">
        <li class="nav-item">
            <a class="nav-link active" data-toggle="tab" href="#tabPareto">Resumen Pareto</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" data-toggle="tab" href="#tabNota">Por Nota</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" data-toggle="tab" href="#tabSinNota">
                Sin Nota
                <span class="badge badge-warning ml-1"><asp:Label ID="lblBadgeSinNota" runat="server" Text="0" /></span>
            </a>
        </li>
        <li class="nav-item">
            <a class="nav-link" data-toggle="tab" href="#tabDetalle">
                Detalle Pendientes
                <span class="badge badge-danger ml-1"><asp:Label ID="lblBadgePend" runat="server" Text="0" /></span>
            </a>
        </li>
    </ul>

    <div class="tab-content bg-white border border-top-0 p-3" style="border-radius:0 0 6px 6px;">

        <!-- Tab: Resumen Pareto -->
        <div class="tab-pane fade show active" id="tabPareto">
            <p class="font-weight-bold mb-3 text-center" style="font-size:1rem;">
                Resumen por Rango Pareto
                <small class="text-muted font-weight-normal">(&#250;ltimos 7 d&#237;as &#8212; solo registros con Pareto &gt; 0)</small>
            </p>
            <div class="row justify-content-center">
                <div class="col-md-8">
                    <asp:GridView ID="gvPareto" runat="server" CssClass="table table-bordered table-hover"
                        AutoGenerateColumns="False" GridLines="None"
                        OnRowDataBound="gvPareto_RowDataBound">
                        <HeaderStyle CssClass="text-center" />
                        <Columns>
                            <asp:BoundField DataField="Rango"      HeaderText="Rango Pareto"  ItemStyle-Font-Size="Medium" HeaderStyle-Font-Size="Medium" />
                            <asp:BoundField DataField="Cantidad"   HeaderText="Registros"     ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Medium" HeaderStyle-Font-Size="Medium" />
                            <asp:BoundField DataField="Resueltas"  HeaderText="Resueltas"     ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Medium" HeaderStyle-Font-Size="Medium" />
                            <asp:BoundField DataField="Pendientes" HeaderText="Pendientes"    ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Medium" HeaderStyle-Font-Size="Medium" />
                            <asp:BoundField DataField="PctTotal"   HeaderText="% del Total"   ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Medium" HeaderStyle-Font-Size="Medium" DataFormatString="{0:N1}" />
                        </Columns>
                        <EmptyDataTemplate><p class="text-muted mt-2 text-center">Sin datos con Pareto registrado en el per&#237;odo seleccionado.</p></EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- Tab: Por Nota -->
        <div class="tab-pane fade" id="tabNota">
            <p class="text-muted mb-2" style="font-size:0.78rem;">
                Agrupa todos los registros seg&#250;n la nota registrada. Las notas con mayor % de cobertura son las acciones m&#225;s efectivas.
            </p>
            <asp:GridView ID="gvNota" runat="server" CssClass="table table-bordered table-hover table-sm"
                AutoGenerateColumns="False" GridLines="None"
                OnRowDataBound="gvNota_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="Nota"      HeaderText="Nota / Acci&#243;n"  />
                    <asp:BoundField DataField="Total"     HeaderText="Total"     ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Resueltas" HeaderText="Resueltas" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Pendientes" HeaderText="Pendientes" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="PctOk"     HeaderText="% Cobertura" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:N1}" />
                    <asp:TemplateField HeaderText="Barra" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <div class="bar-wrap">
                                <div class="bar-fill" id="barNota" runat="server"></div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate><p class="text-muted mt-2">Sin datos en el per&#237;odo seleccionado.</p></EmptyDataTemplate>
            </asp:GridView>
        </div>

        <!-- Tab: Sin Nota -->
        <div class="tab-pane fade" id="tabSinNota">
            <p class="text-muted mb-2" style="font-size:0.78rem;">
                Pendientes que no tienen ninguna nota registrada. Son los casos que a&#250;n no tienen una acci&#243;n documentada.
            </p>
            <asp:GridView ID="gvSinNota" runat="server" CssClass="table table-bordered table-hover table-sm"
                AutoGenerateColumns="False" GridLines="None"
                OnRowDataBound="gvSinNota_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="Factura"    HeaderText="Factura"    />
                    <asp:BoundField DataField="Almacen"    HeaderText="Almac&#233;n"    />
                    <asp:BoundField DataField="Modelo"     HeaderText="Modelo"     />
                    <asp:BoundField DataField="FechaVenta" HeaderText="Fecha Venta" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="DiasAbierto" HeaderText="D&#237;as" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="DCM00"      HeaderText="DCM00"  ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="StockSuc"   HeaderText="Suc."   ItemStyle-HorizontalAlign="Center" />
                </Columns>
                <EmptyDataTemplate><p class="text-success mt-2 font-weight-bold">&#10003; Todos los pendientes tienen nota registrada.</p></EmptyDataTemplate>
            </asp:GridView>
        </div>

        <!-- Tab: Detalle Pendientes -->
        <div class="tab-pane fade" id="tabDetalle">
            <asp:GridView ID="gvDetalle" runat="server" CssClass="table table-bordered table-hover table-sm"
                AutoGenerateColumns="False" GridLines="None"
                OnRowDataBound="gvDetalle_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="Factura"    HeaderText="Factura"    />
                    <asp:BoundField DataField="Almacen"    HeaderText="Almac&#233;n"    />
                    <asp:BoundField DataField="Modelo"     HeaderText="Modelo"     />
                    <asp:BoundField DataField="FechaVenta" HeaderText="Fecha Venta" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="DiasAbierto" HeaderText="D&#237;as" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="DCM00"      HeaderText="DCM00"  ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="StockSuc"   HeaderText="Suc."   ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Transito"   HeaderText="Tr&#225;nsito" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Nota"       HeaderText="Nota"       />
                </Columns>
                <EmptyDataTemplate><p class="text-success mt-2 font-weight-bold">&#10003; Sin pendientes en el per&#237;odo seleccionado.</p></EmptyDataTemplate>
            </asp:GridView>
        </div>

    </div><!-- /tab-content -->
</div><!-- /container -->

<script src="https://cdn.jsdelivr.net/npm/jquery@3.6.0/dist/jquery.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
<script>
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
        else                             { desde = new Date(y-1, 0, 1); hasta = new Date(y-1, 11, 31); }
        document.getElementById('<%= txtDesde.ClientID %>').value = fmtDate(desde);
        document.getElementById('<%= txtHasta.ClientID %>').value = fmtDate(hasta);
        document.getElementById('<%= btnFiltrar.ClientID %>').click();
    }
</script>
</form>
</body>
</html>
