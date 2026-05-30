<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosDespachos.aspx.vb" Inherits="TrasladosDespachos" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Despachos Abiertos</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <style>
        body { background:#f4f6f9; font-size:0.82rem; }

        /* ── topnav ── */
        .topnav { background:#1a252f; display:flex; flex-direction:row; align-items:center; flex-wrap:wrap; padding:0 8px; }
        .topnav .brand { font-weight:700; font-size:0.95rem; color:#fff; padding:12px 16px; display:flex; align-items:center; }
        .topnav a { color:#cdd3d8; padding:12px 14px; text-decoration:none; font-size:0.85rem; display:inline-block; white-space:nowrap; }
        .topnav a:hover { background:rgba(255,255,255,0.08); color:#fff; }
        .topnav a.nav-active { background:rgba(255,255,255,0.15); color:#fff; font-weight:600; }
        .topnav a i { margin-right:4px; }

        /* ── toolbar ── */
        .toolbar-bar { background:#fff; border-bottom:1px solid #dee2e6; padding:8px 16px; display:flex; align-items:center; flex-wrap:wrap; gap:6px; }
        .toolbar-bar label { margin:0; font-weight:600; font-size:0.78rem; }
        .rango-btn { font-size:0.75rem; padding:3px 9px; }

        /* ── compact grids ── */
        .grid-compact { width:100%; }
        .grid-compact thead th,
        .grid-compact tbody td { font-size:0.72rem !important; padding:3px 5px !important; vertical-align:middle !important; }
        .grid-compact thead th { position:sticky; top:0; z-index:2; background:#343a40 !important; color:#fff !important;
                                  border-bottom:2px solid #dee2e6 !important; white-space:nowrap; }

        .card-scroll { overflow-y:auto; overflow-x:auto; max-height:450px; }
        .card-scroll-sm { overflow-y:auto; overflow-x:auto; max-height:400px; }

        /* ── progress bars ── */
        .progress { height:18px; min-width:70px; }
        .progress-bar { font-size:0.68rem; line-height:18px; }
    </style>
</head>
<body>
<form id="form1" runat="server">

    <!-- Navigation -->
    <nav class="topnav">
        <span class="brand"><img src="Imagenes/mnegra.png" width="18" height="18" alt="" style="vertical-align:middle; margin-right:5px;" /> Movesa</span>
        <a href="TrasladosDashboardPlanner.aspx"><i class="fa fa-home"></i> Inicio</a>
        <a href="Maindashboard.aspx"><i class="fa fa-th-large"></i> Men&uacute; Principal</a>
        <a href="TrasladosCuadroBasico.aspx"><i class="fa fa-table"></i> Cuadro B&aacute;sico</a>
        <a href="TrasladosPanelProduccion.aspx"><i class="fa fa-calendar"></i> Panel Planificaci&oacute;n</a>
        <a href="TrasladosDDashboard.aspx"><i class="fa fa-eye"></i> Ver Planificaci&oacute;n</a>
        <a href="TrasladosDespachos.aspx" class="nav-active"><i class="fa fa-truck"></i> Despachos Abiertos</a>
        <a href="TrasladosDespachosAbiertos.aspx"><i class="fa fa-plus-circle"></i> Asignar Cami&oacute;n</a>
        <a href="CerrarSesion.aspx" style="margin-left:auto;"><i class="fa fa-sign-out"></i> Cerrar Sesi&oacute;n</a>
    </nav>

    <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>

    <!-- Filter bar -->
    <div class="toolbar-bar">
        <button type="button" class="btn btn-sm btn-outline-secondary rango-btn" onclick="setRango('mes')">Este mes</button>
        <button type="button" class="btn btn-sm btn-outline-secondary rango-btn" onclick="setRango('mesant')">Mes anterior</button>
        <button type="button" class="btn btn-sm btn-outline-secondary rango-btn" onclick="setRango('anio')">Este a&ntilde;o</button>
        <button type="button" class="btn btn-sm btn-outline-secondary rango-btn" onclick="setRango('anioant')">A&ntilde;o anterior</button>
        <asp:TextBox ID="txtDesde" runat="server" TextMode="Date" CssClass="form-control form-control-sm" style="width:135px;" />
        <asp:TextBox ID="txtHasta" runat="server" TextMode="Date" CssClass="form-control form-control-sm" style="width:135px;" />
        <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-sm btn-primary" Text="Filtrar" />
        <span class="ml-2 text-muted" style="font-size:0.75rem;"><asp:Label ID="lblRango" runat="server" /></span>
        <div class="ml-auto d-flex align-items-center" style="gap:6px;">
            <asp:TextBox ID="txtFilter" runat="server" Text="" placeholder="Buscar..." CssClass="form-control form-control-sm" style="width:180px;" />
            <asp:Button ID="btnVistaCalendario" runat="server" CssClass="btn btn-sm btn-info" Text="Calendario" />
        </div>
    </div>

    <!-- Hidden state labels -->
    <asp:Label ID="lblPlanId"         runat="server" Visible="false" />
    <asp:Label ID="lblCurrentDespacho" runat="server" Visible="false" />
    <asp:Label ID="lblIdPlan"         runat="server" Visible="false" />
    <asp:Label ID="lblRutaLabel"      runat="server" Visible="false" />
    <asp:Label ID="lblRuta"           runat="server" Visible="false" />

    <div class="container-fluid py-3">

        <!-- Card 1: Despachos Abiertos -->
        <div class="card shadow-sm mb-3">
            <div class="card-header py-2" style="background:#1a252f; color:#fff;">
                <strong><i class="fa fa-truck mr-2"></i>Despachos Abiertos</strong>
            </div>
            <div class="card-body p-0 card-scroll">
                <asp:GridView ID="gridDespachosAbiertos" runat="server"
                    AutoGenerateColumns="false"
                    CssClass="table table-sm table-bordered table-hover grid-compact mb-0"
                    Width="100%" EmptyDataText="Sin datos"
                    OnRowDataBound="gridDespachosAbiertos_RowDataBound">
                    <HeaderStyle CssClass="thead-dark" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/view.png"
                            Text="Ver" CommandName="Ver" HeaderText=""
                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="28px">
                            <ControlStyle Height="20px" Width="20px" />
                        </asp:ButtonField>
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/delete.png"
                            Text="Cerrar" CommandName="Cerrar" HeaderText=""
                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="28px">
                            <ControlStyle Height="20px" Width="20px" />
                        </asp:ButtonField>
                        <asp:TemplateField HeaderText="Pick %">
                            <ItemTemplate>
                                <div class="progress">
                                    <div class="progress-bar progress-bar-striped bg-info" role="progressbar"
                                        aria-valuemin="0"
                                        aria-valuenow='<%# Eval("Preparado") %>'
                                        aria-valuemax='<%# Eval("Unds") %>'
                                        style='width:<%# Eval("PorcentajePick") %>%'>
                                        <%# FormatNumber(Eval("PorcentajePick"), 1, TriState.True) %>%
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Carga %">
                            <ItemTemplate>
                                <div class="progress">
                                    <div class="progress-bar progress-bar-striped bg-success" role="progressbar"
                                        aria-valuemin="0"
                                        aria-valuenow='<%# Eval("Cargado") %>'
                                        aria-valuemax='<%# Eval("Unds") %>'
                                        style='width:<%# Eval("PorcentajeCarga") %>%'>
                                        <%# FormatNumber(Eval("PorcentajeCarga"), 1, TriState.True) %>%
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DespachoId"    HeaderText="Despacho" />
                        <asp:BoundField DataField="PlanId"        HeaderText="Plan" />
                        <asp:BoundField DataField="FechaDespacho" HeaderText="F.Despacho" />
                        <asp:BoundField DataField="DiasRestantes" HeaderText="D&iacute;as" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Canal"         HeaderText="Canal" />
                        <asp:BoundField DataField="Ruta"          HeaderText="Ruta" />
                        <asp:BoundField DataField="Almacenes"     HeaderText="Almacenes" />
                        <asp:BoundField DataField="TotalAlm"      HeaderText="Alm" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="TotalFalt"     HeaderText="Faltante" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Unds"          HeaderText="Unds" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="TotalEspacios" HeaderText="Espacios" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Preparado"     HeaderText="Preparado" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Cargado"       HeaderText="Cargado" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <!-- Card 2: Almacenes en Despacho -->
        <div class="card shadow-sm mb-3">
            <div class="card-header py-2" style="background:#1a252f; color:#fff;">
                <strong><i class="fa fa-list mr-2"></i>Almacenes en Despacho</strong>
                <span class="ml-3 text-warning" style="font-size:0.78rem;">
                    Despacho: <asp:Label ID="lblDespachoActivo" runat="server" Text="—" />
                    &nbsp;|&nbsp; Plan: <asp:Label ID="lblPlanActivo" runat="server" Text="—" />
                </span>
            </div>
            <div class="card-body p-0 card-scroll-sm">
                <asp:GridView ID="gridAlmacenesDespachos" runat="server"
                    AutoGenerateColumns="true"
                    CssClass="table table-sm table-bordered table-hover grid-compact mb-0"
                    Width="100%" EmptyDataText="Seleccione un despacho para ver el detalle">
                    <HeaderStyle CssClass="thead-dark" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/delete.png"
                            Text="Eliminar" CommandName="Eliminar" HeaderText=""
                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="28px">
                            <ControlStyle Height="20px" Width="20px" />
                        </asp:ButtonField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

    </div>

    <footer class="text-center text-muted py-2" style="font-size:0.72rem; border-top:1px solid #dee2e6; background:#fff;">
        WebDesign RJ &mdash; Copyright &copy; 2022 <a href="#">Movesa</a>. All rights reserved.
    </footer>

</form>

<script>
    // ── date range presets ──
    function setRango(tipo) {
        var hoy = new Date();
        var y = hoy.getFullYear(), m = hoy.getMonth();
        var d1, d2;
        if (tipo === 'mes') {
            d1 = new Date(y, m, 1);
            d2 = new Date(y, m + 1, 0);
        } else if (tipo === 'mesant') {
            d1 = new Date(y, m - 1, 1);
            d2 = new Date(y, m, 0);
        } else if (tipo === 'anio') {
            d1 = new Date(y, 0, 1);
            d2 = new Date(y, 11, 31);
        } else if (tipo === 'anioant') {
            d1 = new Date(y - 1, 0, 1);
            d2 = new Date(y - 1, 11, 31);
        }
        function fmt(d) { return d.getFullYear() + '-' + String(d.getMonth()+1).padStart(2,'0') + '-' + String(d.getDate()).padStart(2,'0'); }
        $('#<%= txtDesde.ClientID %>').val(fmt(d1));
        $('#<%= txtHasta.ClientID %>').val(fmt(d2));
        $('#<%= btnFiltrar.ClientID %>').click();
    }

    // ── text filter on gridDespachosAbiertos ──
    $('#<%= txtFilter.ClientID %>').on('keyup', function () {
        var val = $(this).val().toLowerCase();
        $('#<%= gridDespachosAbiertos.ClientID %> tbody tr').each(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(val) > -1);
        });
    });
</script>
</body>
</html>
