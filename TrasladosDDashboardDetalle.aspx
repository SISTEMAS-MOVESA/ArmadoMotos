<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosDDashboardDetalle.aspx.vb" Inherits="TrasladosDDashboardDetalle" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Planificación de Despacho</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>
    <script src="https://d3js.org/d3.v6.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <style>
        body { font-family: 'Segoe UI', sans-serif; background: #f4f6f9; font-size: 13px; }

        /* ---- Topnav uniforme ---- */
        .topnav {
            background: #1a252f;
            display: flex;
            flex-direction: row;
            align-items: center;
            flex-wrap: wrap;
            padding: 0 8px;
            overflow: hidden;
        }
        .topnav .brand { font-weight: 700; font-size: 0.95rem; color: #fff; padding: 12px 16px; display: flex; align-items: center; }
        .topnav a { color: #cdd3d8; padding: 12px 14px; text-decoration: none; font-size: 0.85rem; display: inline-block; white-space: nowrap; }
        .topnav a:hover      { background: rgba(255,255,255,0.08); color: #fff; }
        .topnav a.nav-active { background: rgba(255,255,255,0.15); color: #fff; font-weight: 600; }
        .topnav a i { margin-right: 4px; }

        .kpi-bar { background: #fff; border-bottom: 2px solid #dee2e6; padding: 10px 16px; display: flex; gap: 24px; align-items: center; flex-wrap: wrap; }
        .kpi-item { text-align: center; min-width: 90px; }
        .kpi-value { font-size: 22px; font-weight: 700; color: #1a252f; line-height: 1; }
        .kpi-label { font-size: 11px; color: #6c757d; text-transform: uppercase; letter-spacing: .5px; }
        .kpi-item.danger  .kpi-value { color: #dc3545; }
        .kpi-item.success .kpi-value { color: #28a745; }
        .kpi-item.warning .kpi-value { color: #f0a500; }

        .page-body { display: flex; height: calc(100vh - 110px); overflow: hidden; }
        .panel-left  { width: 420px; min-width: 420px; border-right: 2px solid #dee2e6; display: flex; flex-direction: column; background: #fff; transition: width 0.28s ease, min-width 0.28s ease; }
        .panel-right { flex: 1; display: flex; flex-direction: column; background: #fff; overflow: hidden; }

        .panel-left.collapsed { width: 42px !important; min-width: 42px !important; overflow: hidden; }
        .panel-left.collapsed .panel-body,
        .panel-left.collapsed .cliente-strip,
        .panel-left.collapsed .alm-add-bar { display: none !important; }
        .panel-left.collapsed .ph-subtitle,
        .panel-left.collapsed .ph-title    { display: none !important; }
        .panel-left.collapsed .panel-header { justify-content: center; padding: 10px 4px; }

        .btn-toggle-panel { background: none; border: none; color: #fff; font-size: 15px; cursor: pointer; padding: 0 2px; line-height: 1; flex-shrink: 0; }
        .btn-toggle-panel:focus { outline: none; }

        .panel-header { padding: 10px 14px; background: #1a252f; color: #fff; font-weight: 600; font-size: 13px; display: flex; justify-content: space-between; align-items: center; }
        .panel-body   { flex: 1; overflow-y: auto; }

        .grid-compact th { background: #343a40; color: #fff; padding: 6px 8px; position: sticky; top: 0; z-index: 2; white-space: nowrap; cursor: pointer; user-select: none; }
        .grid-compact th:hover { background: #495057; }
        .grid-compact td { padding: 5px 8px; border-bottom: 1px solid #e9ecef; vertical-align: middle; white-space: nowrap; }
        .grid-compact tr:hover td { background: #f0f4ff; }
        .grid-compact tr.selected-alm td { background: #d4edda !important; font-weight: 600; }
        .grid-compact { width: 100%; border-collapse: collapse; font-size: 12px; }

        .heat { font-weight: 700; text-align: center; color: #fff; text-shadow: 0 1px 2px rgba(0,0,0,.5); border-radius: 3px; }

        .nav-tabs .nav-link { font-size: 12px; padding: 6px 14px; color: #495057; }
        .nav-tabs .nav-link.active { font-weight: 700; color: #1a252f; }

        .search-box { font-size: 12px; height: 30px; }

        .cliente-strip { background: #f8f9fa; border-bottom: 1px solid #dee2e6; padding: 6px 14px; display: flex; gap: 20px; font-size: 12px; flex-wrap: wrap; }
        .cliente-field label { color: #6c757d; font-size: 10px; text-transform: uppercase; letter-spacing: .4px; display: block; margin: 0; }
        .cliente-field .val  { font-weight: 700; color: #1a252f; }

        .alm-add-bar { padding: 8px 14px; background: #f8f9fa; border-bottom: 1px solid #dee2e6; display: flex; gap: 8px; }
        .btn-xs { padding: 2px 8px; font-size: 11px; }

        .modelo-card { padding: 4px 0; }
        .modelo-name { font-weight: 600; font-size: 12px; }
        .modelo-rank { font-size: 10px; color: #6c757d; }

        .u6-header { background: #1a252f; color: #fff; cursor: pointer; user-select: none; padding: 5px 12px;
                     display: flex; justify-content: space-between; align-items: center; border-radius: 4px 4px 0 0; }

        .badge-pareto-a { background:#28a745; color:#fff; padding:1px 6px; border-radius:8px; font-size:11px; font-weight:700; }
        .badge-pareto-b { background:#f0a500; color:#fff; padding:1px 6px; border-radius:8px; font-size:11px; font-weight:700; }
        .badge-pareto-c { background:#dc3545; color:#fff; padding:1px 6px; border-radius:8px; font-size:11px; font-weight:700; }
        .badge-mercadeo { background:#17a2b8; color:#fff; padding:1px 6px; border-radius:8px; font-size:11px; }

        .chk-u6     { width:15px; height:15px; cursor:pointer; }
        .chk-cuadro { width:15px; height:15px; cursor:pointer; }

        .factura-num { font-weight:600; color:#1a252f; cursor:help; border-bottom:1px dashed #6c757d; }

        .lupa-desp { color:#6c757d; font-size:13px; text-decoration:none; margin-left:4px; }
        .lupa-desp:hover { color:#1a252f; }

        .nota-u6-sel { font-size:11px; min-width:210px; height:26px; border-radius:3px; border:1px solid #ced4da; }

        .desp-has-tip { cursor:help; border-bottom:1px dashed #1a252f; font-weight:700; }
        .tooltip-inner  { max-width:340px; text-align:left; font-size:11px; }

        .sin-tareas-msg { text-align:center; padding:18px; color:#6c757d; font-style:italic; font-size:13px; }

        .dcm-stock-ok   { background:#cce5ff !important; font-weight:700; }
        .dcm-stock-zero { background:#ffc7ce !important; color:#c00; font-weight:700; }

        /* Botón expandir fila inline (DataTables style) */
        .btn-expand-row { display:inline-block; width:20px; height:20px; background:#1a252f; color:#fff;
                          text-align:center; line-height:18px; border-radius:3px; text-decoration:none;
                          font-size:15px; font-weight:700; cursor:pointer; border:none; padding:0; }
        .btn-expand-row:hover { background:#f0a500; color:#fff; text-decoration:none; }
        .btn-expand-row.expanded { background:#28a745; }

        /* Fila detalle inline — estilo DataTables row details */
        .inline-detail-row td { background:#fff8e1 !important; padding:0 !important; border-left:4px solid #f0a500 !important; }
        #divInlineDetail { padding:5px 10px 6px 12px; background:#fff8e1; font-size:11px; }
        #divInlineDetail .detail-header { background:none; color:#1a252f; padding:0 0 3px; border-radius:0;
                                          margin-bottom:3px; display:flex; justify-content:space-between; align-items:center;
                                          font-size:11px; line-height:1.4; border-bottom:1px solid #e0c060; }
        #divInlineDetail .detail-info-bar { display:flex; gap:12px; margin-bottom:3px; flex-wrap:wrap; font-size:10px; }
        #divInlineDetail .colors-wrap { max-height:88px; overflow-y:auto; border:1px solid #dee2e6; border-radius:3px; }
        #divInlineDetail .grid-compact th { padding:2px 5px !important; font-size:10px !important; }
        #divInlineDetail .grid-compact td { padding:2px 5px !important; font-size:11px !important; }
        #divInlineDetail input[type=number] { height:22px !important; padding:1px 4px !important; font-size:11px !important; width:46px !important; }
        #divInlineDetail .btn { padding:1px 8px !important; font-size:11px !important; }
        #divInlineDetail .row.mb-2 { margin-bottom:3px !important; }
        #divInlineDetail .mt-2 { margin-top:3px !important; }
        .btn-cerrar-ci { background:none; border:none; color:#888; font-size:12px; cursor:pointer; line-height:1; padding:0 2px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>

        <asp:HiddenField ID="hfU6CodModelo"     runat="server" />
        <asp:HiddenField ID="hfU6Queue"         runat="server" />
        <asp:HiddenField ID="hfColorSelections" runat="server" />
        <asp:Button     ID="btnAbrirU6Modal" runat="server" Style="display:none;" />

        <!-- NAVBAR -->
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

        <!-- KPI BAR -->
        <div class="kpi-bar">
            <div class="kpi-item">
                <div class="kpi-value"><%= Request.QueryString("planid") %></div>
                <div class="kpi-label">Plan #</div>
            </div>
            <div class="kpi-separator" style="width:1px;background:#dee2e6;height:36px;"></div>
            <div class="kpi-item">
                <div class="kpi-value"><asp:Label ID="lblRuta"          runat="server" Text="—" /></div>
                <div class="kpi-label">Ruta</div>
            </div>
            <div class="kpi-item">
                <div class="kpi-value"><asp:Label ID="lblAlmActual"     runat="server" Text="—" /></div>
                <div class="kpi-label">Almacén activo</div>
            </div>
            <div class="kpi-item">
                <div style="font-size:13px;font-weight:600;"><asp:Label ID="lblNombreAlmacen" runat="server" Text="—" /></div>
                <div class="kpi-label">Nombre</div>
            </div>
            <div class="kpi-separator" style="width:1px;background:#dee2e6;height:36px;"></div>
            <div class="kpi-item warning">
                <div class="kpi-value"><asp:Label ID="lblUnidades"   runat="server" Text="0" /></div>
                <div class="kpi-label">Unidades sugeridas</div>
            </div>
            <div class="kpi-item">
                <div class="kpi-value"><asp:Label ID="lblEspacios"   runat="server" Text="0" /></div>
                <div class="kpi-label">Espacios usados</div>
            </div>
            <div class="kpi-item">
                <div class="kpi-value"><asp:Label ID="lblUnidadesCI" runat="server" Text="0" /></div>
                <div class="kpi-label">Portal CI</div>
            </div>
            <div class="ml-auto d-flex" style="gap:8px;">
                <asp:Button ID="btnRegresar"  runat="server" CssClass="btn btn-sm btn-outline-secondary" Text="← Regresar" />
                <asp:Button ID="btnContinuar" runat="server" CssClass="btn btn-sm btn-success"           Text="Continuar →" />
            </div>
        </div>

        <!-- BODY -->
        <div class="page-body">

            <!-- PANEL IZQUIERDO -->
            <div class="panel-left" id="panelLeft">
                <div class="panel-header">
                    <span class="ph-title">Almacenes del Plan</span>
                    <span class="ph-subtitle" style="font-weight:400;font-size:11px;margin-left:8px;">Haz clic en un almacén para ver su cuadro</span>
                    <button type="button" class="btn-toggle-panel ml-auto" id="btnToggleLeft" title="Colapsar panel">◀</button>
                </div>
                <div class="cliente-strip">
                    <div class="cliente-field">
                        <label>Estatus</label>
                        <asp:TextBox ID="txtEstatusCliente"    runat="server" ReadOnly="true" CssClass="val" Style="border:none;background:transparent;font-weight:700;width:90px;" />
                    </div>
                    <div class="cliente-field">
                        <label>Límite Crédito</label>
                        <asp:TextBox ID="txtLimiteCredito"     runat="server" ReadOnly="true" CssClass="val" Style="border:none;background:transparent;width:100px;text-align:right;" />
                    </div>
                    <div class="cliente-field">
                        <label>Saldo</label>
                        <asp:TextBox ID="txtSaldoCuenta"       runat="server" ReadOnly="true" CssClass="val" Style="border:none;background:transparent;width:100px;text-align:right;" />
                    </div>
                    <div class="cliente-field">
                        <label>Saldo Consig.</label>
                        <asp:TextBox ID="txtSaldoConsignacion" runat="server" ReadOnly="true" CssClass="val" Style="border:none;background:transparent;width:100px;text-align:right;" />
                    </div>
                </div>
                <div class="alm-add-bar">
                    <input type="text" id="txtFiltroAlm" placeholder="Buscar almacén..." class="form-control search-box" style="flex:1;" />
                    <asp:TextBox ID="txtCodigoAlmacen"     runat="server" CssClass="form-control search-box" placeholder="Cód. almacén" Style="width:110px;" />
                    <asp:Button  ID="btnAgregarAlmacenManual" runat="server" CssClass="btn btn-sm btn-warning" Text="+ Agregar" />
                </div>
                <div class="panel-body">
                    <asp:GridView ID="gridAlmacenesDespachos" runat="server" AutoGenerateColumns="false" CssClass="grid-compact" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="Ruta"     HeaderText="Ruta" />
                            <asp:BoundField DataField="Codigo"   HeaderText="Cód." />
                            <asp:BoundField DataField="Almacen"  HeaderText="Almacén" />
                            <asp:BoundField DataField="Cuadro"   HeaderText="CB"         ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Fisico"   HeaderText="Físico"     ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Faltante" HeaderText="Faltante"   ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Despacho" HeaderText="Despachado" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Indice"   HeaderText="Índice"     ItemStyle-CssClass="heat" ItemStyle-HorizontalAlign="Center" />
                            <asp:ButtonField ButtonType="Link" Text="Ver ›" CommandName="Sugerir"
                                HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="text-primary font-weight-bold" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <!-- PANEL DERECHO -->
            <div class="panel-right">
                <ul class="nav nav-tabs px-3 pt-2">
                    <li class="nav-item"><a class="nav-link active" data-toggle="tab" href="#tabSeleccion">Selección de Motos</a></li>
                    <li class="nav-item"><a class="nav-link"        data-toggle="tab" href="#tabPortal">Pedidos Portal</a></li>
                </ul>
                <div class="tab-content flex-1" style="overflow:hidden;display:flex;flex-direction:column;">

                    <!-- TAB SELECCIÓN -->
                    <div id="tabSeleccion" class="tab-pane fade show active" style="flex:1;overflow-y:auto;padding:10px 14px;">

                        <!-- Sugerido actual -->
                        <div class="card mb-2 border-0 shadow-sm">
                            <div class="card-body py-2 px-3">
                                <div style="overflow-x:auto;max-height:180px;overflow-y:auto;">
                                    <asp:GridView ID="gridPedidoTemp" runat="server" AutoGenerateColumns="false" CssClass="grid-compact" Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="ID"           HeaderText="ID" />
                                            <asp:BoundField DataField="ALMDESTINO"   HeaderText="Almacén" />
                                            <asp:BoundField DataField="ARTICULO"     HeaderText="Artículo" />
                                            <asp:BoundField DataField="MODELO"       HeaderText="Modelo" />
                                            <asp:BoundField DataField="ESPACIOS"     HeaderText="Esp."   ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="CANTIDAD"     HeaderText="Cant."  ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="QTYLOGISTICA" HeaderText="Total"  ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="RUV"          HeaderText="RUV"    ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="RCB"          HeaderText="RCB"    ItemStyle-HorizontalAlign="Center" />
                                            <asp:ButtonField ButtonType="Link" Text="✕" CommandName="Eliminar"
                                                HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="text-danger" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

                        <!-- ══════════════════════════════════════════════════════════
                             PANEL: VENTAS ÚLTIMOS 7 DÍAS — 1 fila por venta (DocNum)
                             Columnas:
                               0=Expand(+)  1=TaskID  2=Chk  3=Factura  4=Modelo
                               5=FechaVenta  6=UltTraslado  7=Pareto  8=RankSuc
                               9=Mercadeo  10=StockSuc  11=StockDCM
                               12=Desp(lupa→TrasladosDespachosDetalleAlmacen)
                               13=Trans  14=CB  15=EnPedido  16=Nota(dropdown)
                             ══════════════════════════════════════════════════════════ -->
                        <div class="card mb-2 border-0 shadow-sm" id="cardUltimas6">
                            <div class="u6-header" onclick="toggleUltimas6()">
                                <span style="font-size:12px;font-weight:700;">
                                    &#9889;&nbsp;<asp:Label ID="lblU6Titulo" runat="server" Text="Ventas Recientes - Demanda Inmediata (1 fila por venta)" />
                                </span>
                                <span style="font-size:11px;background:rgba(255,255,255,.18);padding:1px 10px;border-radius:10px;">
                                    <asp:Label ID="lblResumen6" runat="server" Text="—" />
                                </span>
                                <span id="chevronU6" style="font-size:11px;opacity:.7;">&#9660;</span>
                            </div>
                            <div id="bodyUltimas6">
                                <div style="overflow-x:auto; max-height:calc(100vh - 480px); min-height:180px; overflow-y:auto;">
                                    <asp:GridView ID="gridUltimas6" runat="server" AutoGenerateColumns="false"
                                        CssClass="grid-compact" Width="100%"
                                        EmptyDataText="<div class='sin-tareas-msg'>&#128269; No se encontraron ventas recientes pendientes para este almac&eacute;n</div>">
                                        <Columns>
                                            <%-- Col 0: Expandir fila detalle inline (JS puro, sin postback) --%>
                                            <asp:TemplateField HeaderText="" ItemStyle-Width="24px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <button type="button"
                                                        class="btn-expand-row"
                                                        data-cod='<%# HttpUtility.HtmlAttributeEncode(Eval("CodModelo").ToString()) %>'
                                                        data-nom='<%# HttpUtility.HtmlAttributeEncode(Eval("Modelo").ToString()) %>'
                                                        onclick="expandInline(this,'u6')"
                                                        title="Ver colores disponibles">+</button>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- Col 1: ID Tarea --%>
                                            <asp:BoundField DataField="TaskID" HeaderText="ID" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="36px" />
                                            <%-- Col 2: Checkbox masivo --%>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <input type="checkbox" id="chkU6All" onclick="toggleAllU6(this)" title="Sel. todos" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="checkbox" class="chk-u6"
                                                        data-cod='<%# Eval("CodModelo") %>'
                                                        data-modelo='<%# Eval("Modelo") %>'
                                                        data-docnum='<%# Eval("DocNum") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- Col 3: Nº Factura con tooltip Serie + Color --%>
                                            <asp:TemplateField HeaderText="Factura">
                                                <ItemTemplate>
                                                    <span class="factura-num" title='<%# "Serie: " & Eval("Serie").ToString() & " | Color: " & Eval("Color").ToString() %>'>
                                                        <%# Eval("DocNum") %>
                                                    </span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- Col 4: Modelo --%>
                                            <asp:BoundField DataField="Modelo" HeaderText="Modelo" />
                                            <%-- Col 5: Fecha Venta --%>
                                            <asp:BoundField DataField="FechaVenta" HeaderText="Últ.Venta" DataFormatString="{0:dd/MM/yy}" />
                                            <%-- Col 6: Último traslado recibido --%>
                                            <asp:BoundField DataField="UltTraslado" HeaderText="Últ.Traslado" />
                                            <%-- Col 7: Pareto --%>
                                            <asp:BoundField DataField="Pareto" HeaderText="Pareto" ItemStyle-HorizontalAlign="Center" />
                                            <%-- Col 8: Rank Suc (desde hfROWID del cuadro básico) --%>
                                            <asp:BoundField DataField="RankSuc" HeaderText="Rank Suc." ItemStyle-HorizontalAlign="Center" />
                                            <%-- Col 9: Mercadeo --%>
                                            <asp:BoundField DataField="Mercadeo" HeaderText="Mercadeo" ItemStyle-HorizontalAlign="Center" />
                                            <%-- Col 10: Stock Sucursal --%>
                                            <asp:BoundField DataField="StockSuc" HeaderText="Stock Suc." ItemStyle-HorizontalAlign="Center" />
                                            <%-- Col 11: Stock DCM00 --%>
                                            <asp:BoundField DataField="StockDCM" HeaderText="Stock DCM" ItemStyle-HorizontalAlign="Center" />
                                            <%-- Col 12: Despachos abiertos + lupa --%>
                                            <asp:TemplateField HeaderText="Despacho">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDespU6" runat="server" Text='<%# Eval("Desp") %>' />
                                                    <a href='<%# "TrasladosDespachosDetalleAlmacen.aspx?whscode=" & hfCurrentWhs.Value %>'
                                                       target="_blank" class="lupa-desp" title="Ver despachos abiertos del almacén">&#128269;</a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- Col 13: Tránsito --%>
                                            <asp:BoundField DataField="Trans" HeaderText="Tránsito" ItemStyle-HorizontalAlign="Center" />
                                            <%-- Col 14: Cuadro Básico --%>
                                            <asp:BoundField DataField="CB" HeaderText="CB" ItemStyle-HorizontalAlign="Center" />
                                            <%-- Col 15: Faltante = CB - (Stock + Desp + Tránsito) --%>
                                            <asp:BoundField DataField="Faltante" HeaderText="Fal" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="36px" />
                                            <%-- Col 16: En Pedido --%>
                                            <asp:BoundField DataField="EnPedido" HeaderText="En Pedido" ItemStyle-HorizontalAlign="Center" />
                                            <%-- Col 17: Nota/Eventualidad dropdown, clave localStorage = DocNum --%>
                                            <asp:TemplateField HeaderText="Nota / Eventualidad">
                                                <ItemTemplate>
                                                    <select class="nota-u6 nota-u6-sel"
                                                            data-docnum='<%# Eval("DocNum") %>'
                                                            data-codmodelo='<%# Eval("CodModelo") %>'
                                                            data-fechafactura='<%# Eval("FechaVenta", "{0:yyyy-MM-dd}") %>'
                                                            data-pareto='<%# Eval("Pareto") %>'
                                                            onchange="applyNotaColor(this)">
                                                        <option value="">-- Sin observación --</option>
                                                        <option value="DESPACHO EN PROCESO EN OTRA PLANIFICACION">DESPACHO EN PROCESO EN OTRA PLANIFICACION</option>
                                                        <option value="STOCK INSUFICIENTE DCM">STOCK INSUFICIENTE DCM</option>
                                                        <option value="CUADRO BASICO CUBIERTO">CUADRO BASICO CUBIERTO</option>
                                                        <option value="MODELO NO AUTORIZADO EN CONSIGNACION">MODELO NO AUTORIZADO EN CONSIGNACION</option>
                                                        <option value="DEMANDA CUBIERTA">DEMANDA CUBIERTA</option>
                                                    </select>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <!-- Leyenda -->
                                <div class="d-flex align-items-center px-3 py-1"
                                     style="background:#f8f9fa;border-top:1px solid #dee2e6;gap:10px;flex-wrap:wrap;">
                                    <div style="font-size:10px;color:#555;display:flex;gap:10px;align-items:center;">
                                        <span><span style="display:inline-block;width:12px;height:12px;background:#c6efce;border-radius:2px;margin-right:3px;"></span>Demanda Cubierta</span>
                                        <span><span style="display:inline-block;width:12px;height:12px;background:#ffeb9c;border-radius:2px;margin-right:3px;"></span>Evaluar</span>
                                        <span><span style="display:inline-block;width:12px;height:12px;background:#ffc7ce;border-radius:2px;margin-right:3px;"></span>Acción Inmediata</span>
                                        <span><span class="badge-pareto-a">A</span> Top Pareto</span>
                                        <span><span style="background:#17a2b8;color:#fff;padding:1px 5px;border-radius:8px;font-size:10px;">M</span> Mercadeo</span>
                                        <span style="margin-left:8px;color:#dc3545;font-weight:600;">&#9888; Selecciona una Nota/Eventualidad en cada fila antes de continuar</span>
                                        <span style="margin-left:8px;font-size:10px;color:#155724;background:#d4edda;padding:2px 10px;border-radius:8px;border:1px solid #c3e6cb;white-space:nowrap;">
                                            <strong>CD</strong> = 15&nbsp;d&iacute;as &nbsp;|&nbsp; <strong>CI</strong> = 30&nbsp;d&iacute;as
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Cuadro básico -->
                        <div class="d-flex align-items-center mb-2" style="gap:8px;">
                            <input type="checkbox" id="chkCuadroAll" onchange="toggleAllCuadro(this)" title="Sel. todos" style="width:15px;height:15px;" />
                            <input type="text" id="txtfiltro" placeholder="Buscar modelo..." class="form-control search-box" style="width:200px;" data-target="gridCuadroBasico" />
                            <small class="text-muted">Clic en <strong>+</strong> → despliega colores inline</small>
                        </div>
                        <asp:HiddenField ID="hfPlanId"     runat="server" />
                        <asp:HiddenField ID="hfCurrentWhs" runat="server" />
                        <div style="overflow-x:auto;">
                            <asp:GridView ID="gridCuadroBasico" runat="server" AutoGenerateColumns="false" CssClass="grid-compact" Width="100%">
                                <Columns>
                                    <%-- Col 0: Checkbox --%>
                                    <asp:TemplateField>
                                        <HeaderTemplate><input type="checkbox" onclick="toggleAllCuadro(this)" /></HeaderTemplate>
                                        <ItemTemplate>
                                            <input type="checkbox" class="chk-cuadro"
                                                data-cod='<%# Eval("CODE") %>' data-modelo='<%# Eval("MODELO") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ROWID"  HeaderText="#"    ItemStyle-Width="30px" />
                                    <asp:BoundField DataField="CODE"   HeaderText="Cód." ItemStyle-Width="55px" />
                                    <%-- Col 3: TemplateField Modelo con hfROWID para preservar ROWID original --%>
                                    <asp:TemplateField HeaderText="Modelo">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfROWID" runat="server" Value='<%# Eval("ROWID") %>' />
                                            <div class="modelo-card">
                                                <div class="modelo-name"><asp:Label ID="lblModelo" runat="server" Text='<%# Eval("MODELO") %>' /></div>
                                                <div class="modelo-rank">
                                                    Rank Global: <%# Eval("Ranking") %> | Local: <%# Eval("ROWID") %>
                                                    <asp:Button ID="btnBI" runat="server" Text="Ranking/Enc." CssClass="btn btn-xs btn-outline-info ml-1"
                                                        CommandName="BI" CommandArgument='<%# Eval("MODELO") %>' />
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="3m"           HeaderText="Vta 3m"    ItemStyle-CssClass="heat" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="6m"           HeaderText="Vta 6m"    ItemStyle-CssClass="heat" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="12m"          HeaderText="Vta 12m"   ItemStyle-CssClass="heat" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="3mp"          HeaderText="%3m"       ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="6mp"          HeaderText="%6m"       ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="12mp"         HeaderText="%12m"      ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="DCM00"        HeaderText="DCM Stock" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="CB"           HeaderText="CB"        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Despacho"     HeaderText="Desp"      ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Comprometido" HeaderText="Comp"      ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Solicitado"   HeaderText="Sol"       ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Transito"     HeaderText="Trán"      ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Fisico"       HeaderText="Físico"    ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Faltante"     HeaderText="Fal"       ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Actual"       HeaderText="VtaA"      ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="A"            HeaderText="-30"       ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="B"            HeaderText="-60"       ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="C"            HeaderText="-90"       ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Total"        HeaderText="Total 4M"  ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="UltVenta"     HeaderText="Últ. Venta" />
                                    <asp:BoundField DataField="TipoMoto"     HeaderText="Tipo" />
                                    <asp:BoundField DataField="AGG_RUV"   HeaderText="RUV"   ItemStyle-HorizontalAlign="Center" ItemStyle-Width="38px" />
                                    <asp:BoundField DataField="AGG_RCB"   HeaderText="RCB"   ItemStyle-HorizontalAlign="Center" ItemStyle-Width="38px" />
                                    <asp:BoundField DataField="AGG_TOTAL" HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="42px" />
                                    <%-- Sugerir: JS puro, sin postback, botón + compacto --%>
                                    <asp:TemplateField HeaderText="" ItemStyle-Width="24px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <button type="button"
                                                class="btn-expand-row"
                                                data-cod='<%# HttpUtility.HtmlAttributeEncode(Eval("CODE").ToString()) %>'
                                                data-nom='<%# HttpUtility.HtmlAttributeEncode(Eval("MODELO").ToString()) %>'
                                                data-cb='<%# Eval("CB") %>'
                                                data-fis='<%# Eval("Fisico") %>'
                                                data-fal='<%# Eval("CB") %>'
                                                data-vta='<%# Eval("Actual") %>'
                                                onclick="expandInline(this,'cb')"
                                                title="Seleccionar colores">+</button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>

                    </div><!-- /tabSeleccion -->
                    <!-- TAB Pedidos Portal -->
                    <div id="tabPortal" class="tab-pane fade" style="padding:10px 14px;overflow-y:auto;">
                        <div class="mb-2">
                            <input type="text" id="txtbuscadorPP" placeholder="Buscar..." class="form-control search-box" style="width:220px;" data-target="gridMotosPortalPedidos" />
                        </div>
                        <asp:GridView ID="gridMotosPortalPedidos" runat="server" AutoGenerateColumns="true"
                            CssClass="grid-compact" Width="100%" EmptyDataText="Sin pedidos">
                        </asp:GridView>
                    </div>

                </div>
            </div>
        </div>

        <!-- ═══════════════════════════════════════════════════════════════════
             PANEL INLINE DETALLE — comparte gridColoresMoto entre ambas grillas
             Se mueve en DOM (insertDetailRow) para aparecer como fila hija
             ═══════════════════════════════════════════════════════════════════ -->
        <div id="divInlineDetail" style="display:none;">
            <div class="detail-header">
                <span>
                    <asp:Label ID="lblrutaWhs"    runat="server" CssClass="badge badge-secondary mr-1" />
                    <asp:Label ID="lblAlmOrigen"  runat="server" CssClass="badge badge-info mr-1" Text="DCM00" />
                    <asp:Label ID="lblAlmDestino" runat="server" CssClass="badge badge-primary mr-1" />
                    <asp:Label ID="lblModeMoto"   runat="server" CssClass="badge badge-dark mr-1" />
                    &nbsp;— Seleccionar unidades a enviar
                </span>
                <button type="button" class="btn-cerrar-ci" onclick="closeDetailRow()">✕</button>
            </div>
            <div class="detail-info-bar">
                <span><span class="text-muted">CB:</span> <strong><asp:Label ID="lblCB" runat="server" /></strong></span>
                <span><span class="text-muted">Físico:</span> <strong><asp:Label ID="lblFISICO" runat="server" /></strong></span>
                <span><span class="text-muted">Faltante:</span> <strong><asp:Label ID="lblFALTANTE" runat="server" /></strong></span>
                <span><span class="text-muted">Venta:</span> <strong><asp:Label ID="lblVTAA" runat="server" /></strong></span>
            </div>
            <asp:Label ID="lblcardcode"   runat="server" CssClass="d-none" />
            <asp:Label ID="lblSugerido"   runat="server" CssClass="d-none" />
            <asp:Label ID="lblModeloCode" runat="server" CssClass="d-none" />
            <asp:Label ID="lblplanId"     runat="server" CssClass="d-none" />
            <asp:Label ID="Label1"        runat="server" CssClass="d-none" />
            <asp:Button ID="btnShowModal" runat="server" Style="display:none;" />
            <asp:Panel  ID="pnlPopupColoresMotos" runat="server" Style="display:none;" />

            <div class="colors-wrap">
                <%-- Tabla dinámica JS para flujo individual (sin postback) --%>
                <div id="coloresDynWrap" style="display:none;">
                    <table id="tblColoresDyn" class="grid-compact" style="width:100%;">
                        <thead><tr>
                            <th>Código</th><th>Descripción</th><th>Modelo</th>
                            <th style="text-align:center;">Esp.</th>
                            <th style="text-align:center;">CEDIS</th>
                            <th style="text-align:center;">Suc.</th>
                            <th style="text-align:center;">Cant.</th>
                        </tr></thead>
                        <tbody id="tblColoresDynBody"></tbody>
                    </table>
                </div>
                <%-- GridView servidor para flujo masivo (postback) --%>
                <div id="coloresGridWrap" style="display:none;">
                    <asp:GridView ID="gridColoresMoto" runat="server" AutoGenerateColumns="false" CssClass="grid-compact" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="itemcode" HeaderText="Código" />
                            <asp:BoundField DataField="itemname" HeaderText="Descripción" />
                            <asp:BoundField DataField="Modelo"   HeaderText="Modelo" />
                            <asp:BoundField DataField="Espacios" HeaderText="Esp." ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="CEDIS"    HeaderText="CEDIS"    ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="SUCURSAL" HeaderText="Suc." ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="Cant.">
                                <ItemTemplate>
                                    <asp:TextBox ID="qty" runat="server" Width="46px" Text="0" TextMode="Number"
                                        CssClass="form-control form-control-sm text-center" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="mt-2 d-flex" style="gap:6px;">
                <asp:Button ID="btnModalColoresMotos"  runat="server" CssClass="btn btn-sm btn-success"          Text="&#10003; Agregar" />
                <button type="button" class="btn btn-sm btn-outline-secondary" onclick="closeDetailRow()">&#10005; Cerrar</button>
                <asp:Button ID="btnCancelColoresMotos" runat="server" Style="display:none;" />
            </div>
        </div>

        <!-- MODAL: flujo Agregar Masivo U6 (gridColoresMoto se mueve aquí por JS) -->
        <div class="modal fade" id="modalColores" tabindex="-1">
            <div class="modal-dialog" style="max-width:80%;">
                <div class="modal-content">
                    <div class="modal-header py-2">
                        <h6 class="modal-title">Agregar Masivo — Seleccionar unidades</h6>
                        <span class="ml-2"><asp:Label ID="lblModeMotoModal" runat="server" CssClass="badge badge-dark" /></span>
                    </div>
                    <div class="modal-body py-2">
                        <div id="modalColoresGridWrap"></div>
                    </div>
                    <div class="modal-footer py-2">
                        <asp:Button ID="btnCancelColoresMotosModal" runat="server" CssClass="btn btn-sm btn-outline-secondary" Text="Cerrar" />
                        <asp:Button ID="btnModalColoresMotosModal"  runat="server" CssClass="btn btn-sm btn-success"           Text="Agregar al despacho" />
                    </div>
                </div>
            </div>
        </div>

        <script>
            var _usuarioSession = '<%= System.Web.HttpUtility.JavaScriptStringEncode(If(Session("User") IsNot Nothing, Session("User").ToString(), "")) %>';

            // ── Verifica que todas las filas de U7D tengan nota seleccionada
            //    Retorna true si OK, false + toast si hay pendientes
            function checkU6Resolved() {
                var pendientes = [];
                document.querySelectorAll('.nota-u6').forEach(function(sel) {
                    if (!sel.value) {
                        var tr = sel.closest('tr');
                        var facEl = tr ? tr.querySelector('.factura-num') : null;
                        var modEl = tr ? tr.cells[4] : null;
                        var label = (facEl ? facEl.innerText.trim() : '?') +
                                    (modEl ? ' (' + modEl.innerText.trim() + ')' : '');
                        pendientes.push(label);
                    }
                });
                if (pendientes.length === 0) return true;
                Swal.fire({
                    icon: 'warning',
                    title: 'Filas sin resolver (' + pendientes.length + ')',
                    html: 'Selecciona una <b>Nota / Eventualidad</b> en cada fila de <i>Ventas 7 D&iacute;as</i> antes de agregar motos al despacho:<br><br>' +
                          '<small style="text-align:left;display:block;">' + pendientes.map(function(x){ return '&bull; ' + x; }).join('<br>') + '</small>',
                    confirmButtonText: 'Entendido'
                });
                return false;
            }

            // ── Recopila notas con valor para enviar al servidor
            function getNotasToSave(whs) {
                var notas = [];
                document.querySelectorAll('.nota-u6').forEach(function(sel) {
                    if (!sel.value) return;
                    var docnum = parseInt(sel.getAttribute('data-docnum')) || 0;
                    if (!docnum) return;
                    var codmodelo    = sel.getAttribute('data-codmodelo')    || '';
                    var fechafactura = sel.getAttribute('data-fechafactura') || '';
                    var pareto       = parseFloat(sel.getAttribute('data-pareto')) || 0;
                    notas.push({ docnum: docnum, codmodelo: codmodelo, nota: sel.value, fechafactura: fechafactura, pareto: pareto });
                });
                return notas;
            }

            // ── Persistir notas en BD (fire-and-forget, devuelve Promise)
            function saveNotasToServer(whs) {
                var notas = getNotasToSave(whs);
                if (!notas.length || !whs) return $.when();
                return $.ajax({
                    url: 'GuardarNotasU7DHandler.ashx',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({ whscode: whs, usuario: _usuarioSession, notas: notas })
                });
            }

            // ── Filtro genérico
            $(document).on('keyup', 'input[data-target]', function () {
                var val = $(this).val().toLowerCase(), tbl = $(this).data('target');
                $('#' + tbl + ' tbody tr').each(function () { $(this).toggle($(this).text().toLowerCase().indexOf(val) > -1); });
            });

            // ── Filtro almacenes
            $('#txtFiltroAlm').on('keyup', function () {
                var val = $(this).val().toLowerCase(), tbl = '<%= gridAlmacenesDespachos.ClientID %>';
                $('#' + tbl + ' tbody tr').each(function () { $(this).toggle($(this).text().toLowerCase().indexOf(val) > -1); });
            });

            // ── Heatmap D3
            function applyHeatmap(selector, interpolator) {
                var cells = document.querySelectorAll(selector);
                if (!cells.length) return;
                var vals = Array.from(cells, c => parseFloat(c.innerText) || 0);
                var ext  = d3.extent(vals);
                if (ext[0] === ext[1]) return;
                var scale = d3.scaleSequential().domain(ext).interpolator(interpolator);
                cells.forEach(function (c) { c.style.backgroundColor = scale(parseFloat(c.innerText) || 0); });
            }

            // ── Colapso panel izquierdo
            function collapseLeft() { $('#panelLeft').addClass('collapsed'); $('#btnToggleLeft').html('▶').attr('title','Expandir panel'); try{sessionStorage.setItem('leftPanelCollapsed','1');}catch(e){} }
            function expandLeft()   { $('#panelLeft').removeClass('collapsed'); $('#btnToggleLeft').html('◀').attr('title','Colapsar panel'); try{sessionStorage.setItem('leftPanelCollapsed','0');}catch(e){} }
            $('#btnToggleLeft').on('click', function (e) { e.stopPropagation(); if ($('#panelLeft').hasClass('collapsed')) expandLeft(); else collapseLeft(); });

            $('#<%= gridAlmacenesDespachos.ClientID %> tbody').on('click', 'a', function (e) {
                // Bloquear cambio de almacén si hay filas U7D sin observación
                var u7dRows = document.querySelectorAll('.nota-u6');
                if (u7dRows.length && !checkU6Resolved()) {
                    e.preventDefault();
                    e.stopImmediatePropagation();
                    return false;
                }
                $('#<%= gridAlmacenesDespachos.ClientID %> tbody tr').removeClass('selected-alm');
                $(this).closest('tr').addClass('selected-alm');
                try{sessionStorage.setItem('leftPanelCollapsed','1');}catch(e){}
            });

            // ── Document ready
            $(document).ready(function () {
                try {
                    if (sessionStorage.getItem('leftPanelCollapsed') === '1') {
                        $('#panelLeft').css('transition','none').addClass('collapsed');
                        $('#btnToggleLeft').html('▶').attr('title','Expandir panel');
                        setTimeout(function(){ $('#panelLeft').css('transition',''); }, 50);
                    }
                } catch(e){}
                applyHeatmap('.heat', d3.interpolateOranges);
                cargarNotasU6();
                $('[data-toggle="tooltip"]').tooltip({ html: true, boundary: 'window', trigger: 'hover' });
            });

            // ══════════════════════════════════════════════════════════════════
            // INLINE DETAIL ROW — estilo DataTables row details
            // Mueve #divInlineDetail como fila hija en la tabla destino
            // ══════════════════════════════════════════════════════════════════
            var _detailParentNode = null;

            // Devuelve el ancestro scrollable más cercano (overflow auto/scroll con scroll real)
            function getScrollParent(el) {
                var parent = el.parentElement;
                while (parent) {
                    var ov = getComputedStyle(parent).overflowY;
                    if ((ov === 'auto' || ov === 'scroll') && parent.scrollHeight > parent.clientHeight) {
                        return parent;
                    }
                    parent = parent.parentElement;
                }
                return null;
            }

            function insertDetailRow(tbodySelector, rowIndex) {
                closeDetailRow(); // cierra si había una abierta

                var tbody = document.querySelector(tbodySelector);
                if (!tbody) return;
                var rows = Array.from(tbody.querySelectorAll(':scope > tr'));
                if (rowIndex < 0 || rowIndex >= rows.length) return;

                var targetRow = rows[rowIndex];
                var colCount  = targetRow.cells.length;

                var detail = document.getElementById('divInlineDetail');
                _detailParentNode = detail.parentNode;

                // Crear fila contenedora
                var tr = document.createElement('tr');
                tr.id = 'trInlineDetail';
                tr.className = 'inline-detail-row';

                var td = document.createElement('td');
                td.colSpan = colCount;
                td.style.padding = '0';

                // Mover el div real (con controles ASP.NET) dentro del td
                detail.style.display = '';
                td.appendChild(detail);
                tr.appendChild(td);
                targetRow.insertAdjacentElement('afterend', tr);

                // Marcar botón expand activo
                var expandBtns = targetRow.querySelectorAll('.btn-expand-row');
                expandBtns.forEach(function(b){ b.classList.add('expanded'); b.innerText = '−'; });

                // Scroll: mostrar la fila detalle justo debajo de la fila clickeada
                setTimeout(function() {
                    var scrollContainer = getScrollParent(tbody);
                    if (scrollContainer) {
                        var contRect   = scrollContainer.getBoundingClientRect();
                        var detailTr   = document.getElementById('trInlineDetail');
                        if (detailTr) {
                            var detailRect = detailTr.getBoundingClientRect();
                            if (detailRect.bottom > contRect.bottom) {
                                scrollContainer.scrollTop += detailRect.bottom - contRect.bottom + 10;
                            } else if (detailRect.top < contRect.top) {
                                scrollContainer.scrollTop += detailRect.top - contRect.top - 4;
                            }
                        }
                    }
                }, 80);
            }

            function closeDetailRow() {
                var tr     = document.getElementById('trInlineDetail');
                var detail = document.getElementById('divInlineDetail');

                if (detail) {
                    detail.style.display = 'none';
                    if (_detailParentNode) {
                        _detailParentNode.appendChild(detail);
                    } else {
                        document.getElementById('form1').appendChild(detail);
                    }
                    _detailParentNode = null;
                }
                if (tr) tr.remove();

                // Resetear todos los botones expand
                document.querySelectorAll('.btn-expand-row').forEach(function(b){
                    b.classList.remove('expanded'); b.innerText = '+';
                });
            }

            // Alias para compatibilidad con llamadas desde VB RegisterStartupScript
            function cerrarPanelColores() { closeDetailRow(); }
            function abrirPanelColores()  { /* no-op, reemplazado por insertDetailRow */ }

            // Mueve divInlineDetail al modal para flujo masivo U6
            function showColoresInModal() {
                closeDetailRow();
                // Masivo: mostrar gridView servidor, ocultar tabla dinámica
                document.getElementById('coloresDynWrap').style.display  = 'none';
                document.getElementById('coloresGridWrap').style.display = '';
                var wrap   = document.getElementById('modalColoresGridWrap');
                var detail = document.getElementById('divInlineDetail');
                if (wrap && detail) {
                    _detailParentNode = detail.parentNode;
                    detail.style.display = '';
                    wrap.appendChild(detail);
                }
                $('#modalColores').modal('show');
            }

            // ══════════════════════════════════════════════════════════════════
            // EXPAND INLINE SIN POSTBACK — lee de _coloresData precargado
            // ══════════════════════════════════════════════════════════════════
            window._coloresData     = window._coloresData     || {};
            window._coloresCardCode = window._coloresCardCode || '';

            function expandInline(btn, source) {
                var codModelo = btn.getAttribute('data-cod') || '';
                var modeloNom = btn.getAttribute('data-nom') || codModelo;
                var tr    = btn.closest('tr');
                var tbody = tr.parentElement;
                var rows  = Array.from(tbody.querySelectorAll(':scope > tr'));
                var rowIndex = rows.indexOf(tr);

                // Toggle: cerrar si ya está abierta esta fila
                var existingTr = document.getElementById('trInlineDetail');
                if (existingTr && existingTr.previousElementSibling === tr) {
                    closeDetailRow(); return;
                }

                var whs = document.getElementById('<%= hfCurrentWhs.ClientID %>').value;
                if (!whs) {
                    iziToast.warning({title:'Atención',message:'Selecciona un almacén primero.',position:'topRight',timeout:3000});
                    return;
                }

                // Fallback sessionStorage si _coloresData no está en memoria
                if (!window._coloresData || !Object.keys(window._coloresData).length) {
                    try { window._coloresData = JSON.parse(sessionStorage.getItem('_colData_' + whs) || '{}'); } catch(e) {}
                }
                if (!window._coloresCardCode) {
                    try { window._coloresCardCode = sessionStorage.getItem('_colCC_' + whs) || ''; } catch(e) {}
                }

                var colors = (window._coloresData || {})[codModelo] || [];

                // Setear etiquetas de encabezado (solo display)
                function setTxt(id, val) { var el = document.getElementById(id); if (el) el.innerText = val; }
                setTxt('<%= lblrutaWhs.ClientID %>',    document.getElementById('<%= lblRuta.ClientID %>').innerText || '—');
                setTxt('<%= lblAlmOrigen.ClientID %>',  'DCM00');
                setTxt('<%= lblAlmDestino.ClientID %>', whs);
                setTxt('<%= lblModeMoto.ClientID %>',   modeloNom);
                setTxt('<%= lblModeloCode.ClientID %>', codModelo);
                setTxt('<%= lblcardcode.ClientID %>',   window._coloresCardCode);

                if (source === 'cb') {
                    setTxt('<%= lblCB.ClientID %>',       btn.getAttribute('data-cb')  || '0');
                    setTxt('<%= lblFISICO.ClientID %>',   btn.getAttribute('data-fis') || '0');
                    setTxt('<%= lblFALTANTE.ClientID %>', btn.getAttribute('data-fal') || '0');
                    setTxt('<%= lblVTAA.ClientID %>',     btn.getAttribute('data-vta') || '0');
                } else {
                    setTxt('<%= lblCB.ClientID %>',       '0');
                    setTxt('<%= lblFISICO.ClientID %>',   '0');
                    setTxt('<%= lblFALTANTE.ClientID %>', '0');
                    setTxt('<%= lblVTAA.ClientID %>',     '0');
                }

                // Guardar source para que el interceptor lo envíe al servidor
                window._expandSource = source;

                // Mostrar tabla dinámica, ocultar gridView masivo
                buildColoresTable(colors);
                document.getElementById('coloresDynWrap').style.display  = '';
                document.getElementById('coloresGridWrap').style.display = 'none';

                // Insertar fila inline sin postback
                var tableId = tr.closest('table').id;
                insertDetailRow('#' + tableId + ' tbody', rowIndex);
            }

            function buildColoresTable(colors) {
                var tbody = document.getElementById('tblColoresDynBody');
                if (!tbody) return;
                if (!colors || !colors.length) {
                    tbody.innerHTML = '<tr><td colspan="7" style="text-align:center;color:#888;padding:6px;font-size:11px;">Sin stock disponible en DCM00</td></tr>';
                    return;
                }
                var html = '';
                colors.forEach(function(c) {
                    html += '<tr data-code="' + _esc(c.code) + '" data-name="' + _esc(c.name) + '" data-modelo="' + _esc(c.modelo) + '" data-esp="' + (c.esp||0) + '">' +
                        '<td>' + _esc(c.code) + '</td>' +
                        '<td>' + _esc(c.name) + '</td>' +
                        '<td>' + _esc(c.modelo) + '</td>' +
                        '<td style="text-align:center;">' + (c.esp||0) + '</td>' +
                        '<td style="text-align:center;">' + (c.cedis||0) + '</td>' +
                        '<td style="text-align:center;">' + (c.suc||0) + '</td>' +
                        '<td style="text-align:center;"><input type="number" class="colorqty-input form-control form-control-sm text-center" value="0" min="0" style="width:46px;height:22px;padding:1px 4px;font-size:11px;" /></td>' +
                        '</tr>';
                });
                tbody.innerHTML = html;
            }

            function _esc(s) {
                return String(s||'').replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;').replace(/"/g,'&quot;');
            }

            // Interceptar Agregar: serializar selección a hfColorSelections y guardar notas antes del postback
            $(document).on('click', '#<%= btnModalColoresMotos.ClientID %>', function(e) {
                var dynWrap = document.getElementById('coloresDynWrap');
                if (!dynWrap || dynWrap.style.display === 'none') {
                    // Flujo masivo: limpiar hfColorSelections para que VB use gridColoresMoto
                    document.getElementById('<%= hfColorSelections.ClientID %>').value = '';
                } else {
                    function getTxt(id) { var el = document.getElementById(id); return el ? el.innerText.trim() : ''; }
                    var sels = [];
                    document.querySelectorAll('#tblColoresDynBody tr').forEach(function(row) {
                        var inp = row.querySelector('.colorqty-input');
                        if (!inp) return;
                        var qty = parseInt(inp.value) || 0;
                        if (qty > 0) sels.push({
                            code:   row.dataset.code   || '',
                            name:   row.dataset.name   || '',
                            modelo: row.dataset.modelo || '',
                            esp:    parseFloat(row.dataset.esp) || 0,
                            qty:    qty
                        });
                    });
                    var payload = {
                        ruta:       getTxt('<%= lblrutaWhs.ClientID %>'),
                        almOrigen:  getTxt('<%= lblAlmOrigen.ClientID %>') || 'DCM00',
                        almDestino: getTxt('<%= lblAlmDestino.ClientID %>'),
                        cardcode:   getTxt('<%= lblcardcode.ClientID %>') || window._coloresCardCode,
                        cb:         getTxt('<%= lblCB.ClientID %>'),
                        fisico:     getTxt('<%= lblFISICO.ClientID %>'),
                        faltante:   getTxt('<%= lblFALTANTE.ClientID %>'),
                        vtaa:       getTxt('<%= lblVTAA.ClientID %>'),
                        modeloCode: getTxt('<%= lblModeloCode.ClientID %>'),
                        source:     window._expandSource || 'cb',
                        items:      sels
                    };
                    document.getElementById('<%= hfColorSelections.ClientID %>').value = JSON.stringify(payload);
                }

                // Guardar notas en BD antes del postback (solo si hay notas pendientes)
                var whs = document.getElementById('<%= hfCurrentWhs.ClientID %>').value;
                var notasToSave = getNotasToSave(whs);
                if (notasToSave.length && whs) {
                    e.preventDefault();
                    $.ajax({
                        url: 'GuardarNotasU7DHandler.ashx',
                        type: 'POST',
                        contentType: 'application/json',
                        data: JSON.stringify({ whscode: whs, usuario: _usuarioSession, notas: notasToSave })
                    }).always(function() {
                        __doPostBack('<%= btnModalColoresMotos.UniqueID %>', '');
                    });
                }
            });

            // ══ Colapso demanda 7 días
            function toggleUltimas6() {
                var body = document.getElementById('bodyUltimas6'), chev = document.getElementById('chevronU6');
                if (body.style.display === 'none') { body.style.display=''; chev.innerHTML='&#9660;'; }
                else { body.style.display='none'; chev.innerHTML='&#9654;'; }
            }

            // ══ NOTAS localStorage — clave por DocNum
            function notaKey(whscode, docnum) { return 'nota_u6_' + whscode + '_doc_' + docnum; }

            function applyNotaColor(sel) {
                var tr = sel.closest('tr');
                if (!tr) return;
                if (!tr.hasAttribute('data-orig-bg')) {
                    tr.setAttribute('data-orig-bg',    tr.style.backgroundColor || '');
                    tr.setAttribute('data-orig-color',  tr.style.color           || '');
                }
                if (sel.value === 'DEMANDA CUBIERTA') {
                    tr.style.backgroundColor = '#c6efce';
                    tr.style.color           = '';
                } else {
                    tr.style.backgroundColor = tr.getAttribute('data-orig-bg');
                    tr.style.color           = tr.getAttribute('data-orig-color');
                }
            }

            function guardarNotasU6() {
                var whs = document.getElementById('<%= hfCurrentWhs.ClientID %>').value;
                document.querySelectorAll('.nota-u6').forEach(function(sel) {
                    var docnum = sel.getAttribute('data-docnum');
                    if (docnum) { try{ localStorage.setItem(notaKey(whs, docnum), sel.value); }catch(e){} }
                });
                saveNotasToServer(whs);
                var msg = document.getElementById('msgNotasU6');
                msg.style.display='inline';
                setTimeout(function(){ msg.style.display='none'; }, 2500);
            }

            function cargarNotasU6() {
                var whs = document.getElementById('<%= hfCurrentWhs.ClientID %>').value;
                if (!whs) return;
                document.querySelectorAll('.nota-u6').forEach(function(sel) {
                    var docnum = sel.getAttribute('data-docnum');
                    if (docnum) {
                        try{ var val = localStorage.getItem(notaKey(whs, docnum)); if(val) sel.value = val; }catch(e){}
                    }
                    applyNotaColor(sel);
                });
            }

            // ══ Checkboxes
            function toggleAllU6(master)     { document.querySelectorAll('.chk-u6').forEach(function(c){ c.checked=master.checked; }); }
            function toggleAllCuadro(master) { document.querySelectorAll('.chk-cuadro').forEach(function(c){ c.checked=master.checked; }); }

            // ══ Agregar masivo U6 (usa modal para cola de modelos)
            var u6Queue = [];
            function agregarMasivoU6() {
                // Agregar masivo U6 solo requiere que las filas marcadas tengan nota
                var sinNota = [];
                document.querySelectorAll('.chk-u6:checked').forEach(function(chk){
                    var tr = chk.closest('tr');
                    var sel = tr ? tr.querySelector('.nota-u6') : null;
                    if (sel && !sel.value) {
                        var facEl = tr.querySelector('.factura-num');
                        var modEl = tr.cells[4];
                        sinNota.push((facEl ? facEl.innerText.trim() : '?') +
                                     (modEl ? ' (' + modEl.innerText.trim() + ')' : ''));
                    }
                });
                if (sinNota.length > 0) {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Filas marcadas sin nota (' + sinNota.length + ')',
                        html: 'Selecciona una <b>Nota / Eventualidad</b> en cada fila marcada:<br><br>' +
                              '<small>' + sinNota.map(function(x){ return '&bull; ' + x; }).join('<br>') + '</small>',
                        confirmButtonText: 'Entendido'
                    });
                    return;
                }
                u6Queue = [];
                document.querySelectorAll('.chk-u6:checked').forEach(function(chk){
                    var cod = chk.getAttribute('data-cod');
                    if (cod && u6Queue.indexOf(cod) === -1) u6Queue.push(cod);
                });
                if (u6Queue.length === 0) {
                    iziToast.warning({title:'Atención',message:'Marca al menos una moto con el checkbox',position:'topRight',timeout:3000});
                    return;
                }
                // Guardar notas en BD antes de procesar
                var whs = document.getElementById('<%= hfCurrentWhs.ClientID %>').value;
                saveNotasToServer(whs).always(function() { procesarSiguienteU6(); });
            }
            function procesarSiguienteU6() {
                if (u6Queue.length === 0) return;
                var cod = u6Queue.shift();
                document.getElementById('<%= hfU6CodModelo.ClientID %>').value = cod;
                document.getElementById('<%= hfU6Queue.ClientID %>').value     = u6Queue.join(',');
                __doPostBack('<%= btnAbrirU6Modal.UniqueID %>', '');
            }

            // ══ Continuar: validar U7D → guardar notas → postback
            $(document).on('click', '#<%= btnContinuar.ClientID %>', function(e) {
                var u7dRows = document.querySelectorAll('.nota-u6');
                if (!u7dRows.length) return; // sin filas U7D cargadas, permitir
                if (!checkU6Resolved()) {
                    e.preventDefault();
                    return false;
                }
                // Guardar notas antes de redirigir
                var whs = document.getElementById('<%= hfCurrentWhs.ClientID %>').value;
                var notasToSave = getNotasToSave(whs);
                if (notasToSave.length && whs) {
                    e.preventDefault();
                    saveNotasToServer(whs).always(function() {
                        __doPostBack('<%= btnContinuar.UniqueID %>', '');
                    });
                }
            });

            // ══ Cerrar plan
            function cerrarPlan(idPlan) {
                Swal.fire({
                    title: '¿Cerrar esta planificación?', text: 'Esta acción no se puede deshacer.', icon: 'warning',
                    showCancelButton: true, confirmButtonText: 'Sí, cerrar', cancelButtonText: 'Cancelar'
                }).then(r => {
                    if (r.isConfirmed) {
                        $.post("CerrarPlanificacionHandler.ashx", JSON.stringify({ id: idPlan }), function (resp) {
                            if (resp.success) Swal.fire('Cerrado', 'Planificación cerrada.', 'success').then(() => location.assign("TrasladosDDashboard.aspx"));
                            else Swal.fire('Error', resp.message, 'error');
                        }, 'json').fail(() => Swal.fire('Error', 'No se pudo completar.', 'error'));
                    }
                });
            }
        </script>

    </form>
</body>
</html>
