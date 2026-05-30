<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosDDashboardDetalleConfirmacion.aspx.vb" Inherits="TrasladosDDashboardDetalleConfirmacion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Confirmacion Despacho</title>
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

        /* ── action toolbar ── */
        .toolbar-bar { background:#fff; border-bottom:1px solid #dee2e6; padding:8px 16px; display:flex; align-items:center; flex-wrap:wrap; gap:8px; }
        .toolbar-bar label { margin:0; font-weight:600; font-size:0.8rem; white-space:nowrap; }

        /* ── compact grids ── */
        .grid-compact { table-layout:fixed; width:100%; }
        .grid-compact thead th,
        .grid-compact tbody td { font-size:0.72rem !important; padding:3px 5px !important; white-space:normal !important; word-break:break-word; overflow:hidden; }
        .grid-compact thead th { position:sticky; top:0; z-index:2; background:#343a40; color:#fff; border-bottom:2px solid #dee2e6; }

        .card-scroll { overflow-y:auto; overflow-x:hidden; max-height:420px; }
        .card-scroll-sm { overflow-y:auto; overflow-x:hidden; max-height:360px; }

        /* ── stat badges ── */
        .stat-badge { background:#e9ecef; border-radius:4px; padding:3px 10px; font-size:0.78rem; font-weight:600; white-space:nowrap; }

        /* ── checkbox ── */
        .document-checkbox input[type="checkbox"] { transform:scale(1.3); cursor:pointer; }

        /* ── selected rows ── */
        tr.selected-row td { background-color:#d4edda !important; }
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
        <a href="TrasladosDDashboard.aspx" class="nav-active"><i class="fa fa-eye"></i> Ver Planificaci&oacute;n</a>
        <a href="TrasladosDespachos.aspx"><i class="fa fa-truck"></i> Despachos Abiertos</a>
        <a href="TrasladosDespachosAbiertos.aspx"><i class="fa fa-plus-circle"></i> Asignar Cami&oacute;n</a>
        <a href="CerrarSesion.aspx" style="margin-left:auto;"><i class="fa fa-sign-out"></i> Cerrar Sesi&oacute;n</a>
    </nav>

    <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>

    <!-- Action toolbar -->
    <div class="toolbar-bar">
        <label><i class="fa fa-calendar"></i> Fecha Despacho:</label>
        <asp:TextBox ID="txtFechaDespacho" runat="server" TextMode="Date" CssClass="form-control form-control-sm" style="width:160px;" />
        <asp:Button ID="btnRegresar" runat="server" CssClass="btn btn-sm btn-warning" Text="&#xf060; Regresar" />
        <asp:Button ID="btnContinuar" runat="server" CssClass="btn btn-sm btn-success" Text="&#xf00c; Crear Despacho" />
        <div class="ml-auto d-flex align-items-center" style="gap:6px;">
            <span class="stat-badge"><i class="fa fa-list"></i> <asp:Label ID="lblTotRows" runat="server" Text="Lineas: 0" /></span>
            <span class="stat-badge"><i class="fa fa-motorcycle"></i> <asp:Label ID="lblUnidadesPortalPedidos" runat="server" Text="Unidades: 0" /></span>
            <span class="stat-badge"><i class="fa fa-cube"></i> <asp:Label ID="lblEspaciosPotalPedidos" runat="server" Text="Espacios: 0" /></span>
        </div>
    </div>

    <div class="container-fluid py-3">

        <!-- Card 1: Motos Portal Pedidos -->
        <div class="card shadow-sm mb-3">
            <div class="card-header py-2 d-flex align-items-center" style="background:#1a252f; color:#fff;">
                <strong><i class="fa fa-shopping-cart mr-2"></i>Motos Portal Pedidos <small class="ml-2" style="font-weight:400; opacity:.8;">(Facturadas / Entregas)</small></strong>
                <div class="ml-auto">
                    <asp:TextBox ID="txtFilter" runat="server" Text="" placeholder="Filtrar por modelo..."
                        CssClass="form-control form-control-sm" style="width:200px;" />
                </div>
            </div>
            <div class="card-body p-0 card-scroll">
                <asp:GridView ID="gridMotosPortalPedidos" runat="server"
                    AutoGenerateColumns="true"
                    CssClass="table table-sm table-bordered table-hover grid-compact mb-0"
                    Width="100%" EmptyDataText="Sin datos" GridLines="None">
                    <HeaderStyle CssClass="thead-dark" />
                    <Columns>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbDocument" runat="server" CssClass="document-checkbox" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <!-- Card 2: Motos Despacho -->
        <div class="card shadow-sm mb-3">
            <div class="card-header py-2 d-flex align-items-center" style="background:#1a252f; color:#fff;">
                <strong><i class="fa fa-truck mr-2"></i>Motos Despacho</strong>
                <div class="ml-auto d-flex" style="gap:6px;">
                    <span class="stat-badge" style="background:rgba(255,255,255,0.15); color:#fff;">
                        <asp:Label ID="lblUnidades" runat="server" Text="Unidades: 0" />
                    </span>
                    <span class="stat-badge" style="background:rgba(255,255,255,0.15); color:#fff;">
                        <asp:Label ID="lblEspacios" runat="server" Text="Espacios: 0" />
                    </span>
                </div>
            </div>
            <div class="card-body p-0 card-scroll-sm">
                <asp:GridView ID="gridPedidoTemp" runat="server"
                    AutoGenerateColumns="false"
                    CssClass="table table-sm table-bordered table-hover grid-compact mb-0"
                    Width="100%" EmptyDataText="Sin datos" GridLines="None">
                    <HeaderStyle CssClass="thead-dark" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/delete.png"
                            Text="Eliminar" CommandName="Eliminar" HeaderText=""
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="30px">
                            <ControlStyle Height="16px" Width="16px" />
                        </asp:ButtonField>
                        <asp:BoundField DataField="RUTA"        HeaderText="Ruta"      ItemStyle-Width="40px" />
                        <asp:BoundField DataField="ID"          HeaderText="Id"        ItemStyle-Width="40px" />
                        <asp:BoundField DataField="ALMDESTINO"  HeaderText="Alm"       ItemStyle-Width="50px" />
                        <asp:BoundField DataField="ARTICULO"    HeaderText="Art&iacute;culo" ItemStyle-Width="80px" />
                        <asp:BoundField DataField="MODELO"      HeaderText="Modelo"    ItemStyle-Width="120px" />
                        <asp:BoundField DataField="ESPACIOS"    HeaderText="Esp"       ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="CANTIDAD"    HeaderText="Cant"      ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="QTYLOGISTICA" HeaderText="Log"      ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="OBSERVACIONES" HeaderText="Obs"     ItemStyle-Width="100px" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

    </div>

    <footer class="text-center text-muted py-2" style="font-size:0.72rem; border-top:1px solid #dee2e6; background:#fff;">
        WebDesign RJ &mdash; Copyright &copy; 2022 <a href="#">Movesa</a>. All rights reserved.
    </footer>

</form>

<script type="text/javascript">
    // ── filter for gridMotosPortalPedidos ──
    $('#<%= txtFilter.ClientID %>').on('keyup', function () {
        var val = $(this).val().toLowerCase();
        $('#<%= gridMotosPortalPedidos.ClientID %> tbody tr').each(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(val) > -1);
        });
        sumarEspacios();
    });

    // ── mark coincidences between despacho temp and portal pedidos ──
    function marcarCoincidencias() {
        var valoresPedidoTemp = [];
        $('#<%= gridPedidoTemp.ClientID %> tr').each(function () {
            var celda = $(this).find('td:eq(1)');
            if (celda.length > 0) valoresPedidoTemp.push(celda.text().trim());
        });
        var hayDespacho = valoresPedidoTemp.length > 0;
        $('#<%= gridMotosPortalPedidos.ClientID %> tr').each(function () {
            var celda = $(this).find('td:eq(1)');
            if (celda.length > 0) {
                var valor = celda.text().trim();
                if (valoresPedidoTemp.includes(valor)) {
                    $(this).addClass('selected-row').show();
                } else if (hayDespacho) {
                    $(this).hide();
                } else {
                    $(this).show();
                }
            }
        });
    }

    // ── sum spaces/units from selected rows ──
    function sumarEspacios() {
        var total = 0, unidades = 0, totalRows = 0;
        $('#<%= gridMotosPortalPedidos.ClientID %> tbody tr.selected-row:visible').each(function () {
            var espacio = parseInt($(this).find('td:eq(12)').text());
            total += isNaN(espacio) ? 0 : espacio;
            unidades++;
            totalRows++;
        });
        $('#<%= lblEspaciosPotalPedidos.ClientID %>').text('Espacios: ' + total);
        $('#<%= lblUnidadesPortalPedidos.ClientID %>').text('Unidades: ' + unidades);
        $('#<%= lblTotRows.ClientID %>').text('Lineas: ' + totalRows);
    }

    $(document).ready(function () {
        marcarCoincidencias();
        sumarEspacios();
    });
</script>
</body>
</html>
