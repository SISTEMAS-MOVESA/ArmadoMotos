<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosCuadroBasico.aspx.vb" Inherits="TrasladosCuadroBasico" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Cuadro Básico V3 &mdash; Motor CBv3</title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.3.0/css/all.min.css" />
    <link rel="stylesheet" href="dist/css/adminlte.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>
    <script src="plugins/jquery/jquery.min.js"></script>
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.js"></script>
    <style>
        body { background:#f4f6f9; }
        .topnav { overflow:hidden; background:#272b35; }
        .topnav a { color:white; padding:14px 16px; text-decoration:none; font-size:17px; display:inline-block; }
        .topnav a:hover { background:#3b4252; }
        .topnav a.active { background:#1a6fbf; }

        .v3-header { padding:14px 24px; background:#fff; border-bottom:1px solid #dee2e6; display:flex; justify-content:space-between; align-items:center; }
        .v3-title  { font-size:20px; font-weight:700; margin:0; }
        .v3-sub    { font-size:12px; color:#6c757d; margin-top:2px; }

        /* Métricas */
        .metric-row { display:grid; gap:6px; margin:10px 0 4px; }
        .metric-row-1 { grid-template-columns: repeat(7, 1fr); }
        .metric-row-2 { grid-template-columns: repeat(7, 1fr); }
        .metric-card  { background:#fff; border-radius:6px; padding:7px 10px; border:1px solid #dee2e6; }
        .metric-label { font-size:10px; color:#6c757d; margin-bottom:1px; white-space:nowrap; overflow:hidden; text-overflow:ellipsis; }
        .metric-value { font-size:18px; font-weight:700; }
        .m-oro    { background:#FFFBEB; border-color:#FCD34D; }
        .m-plata  { background:#F8FAFC; border-color:#CBD5E1; }
        .m-bronce { background:#FFF7ED; border-color:#FDBA74; }
        .m-bajo   { background:#F9FAFB; border-color:#D1D5DB; }
        .m-critico { background:#FEE2E2; border-color:#FCA5A5; }
        .m-bajost  { background:#FEF9C3; border-color:#FDE047; }
        .m-ok      { background:#DCFCE7; border-color:#86EFAC; }
        .m-sobre   { background:#FEF3C7; border-color:#FCD34D; }
        .m-muerto  { background:#F1F5F9; border-color:#CBD5E1; }
        .m-res     { background:#FEE2E2; border-color:#FCA5A5; }
        .m-man     { background:#DCFCE7; border-color:#86EFAC; }
        .m-red     { background:#FEF9C3; border-color:#FDE047; }
        .m-def     { background:#EDE9FE; border-color:#A78BFA; }

        /* Filtros */
        .filters-bar { background:#fff; padding:12px 16px; border-radius:8px; border:1px solid #dee2e6;
                       margin-bottom:10px; display:grid; grid-template-columns:2fr 1fr 1fr 1fr 1fr auto auto; gap:8px; align-items:end; }
        .filters-bar label { font-size:12px; color:#6c757d; margin-bottom:2px; display:block; }

        /* Bulk */
        .bulk-bar { background:#272b35; color:white; padding:10px 16px; border-radius:8px; margin-bottom:8px;
                    display:flex; justify-content:space-between; align-items:center; font-size:14px; }
        .bulk-bar button { margin-left:6px; }

        /* Badges */
        .tier-badge { display:inline-block; padding:2px 7px; border-radius:5px; font-size:10px; font-weight:700; letter-spacing:.5px; }
        .tier-ORO    { background:#FEF3C7; color:#92400E; border:1px solid #F59E0B; }
        .tier-PLATA  { background:#F1F5F9; color:#334155; border:1px solid #94A3B8; }
        .tier-BRONCE { background:#FFF7ED; color:#7C2D12; border:1px solid #FB923C; }
        .tier-BAJO   { background:#F9FAFB; color:#6B7280; border:1px solid #D1D5DB; }

        .accion-badge { display:inline-block; padding:3px 9px; border-radius:6px; font-size:11px; font-weight:700; }
        .accion-RESURTIR  { background:#FEE2E2; color:#991B1B; }
        .accion-MANTENER  { background:#D1FAE5; color:#065F46; }
        .accion-REDUCIR   { background:#FEF3C7; color:#92400E; }
        .accion-RETIRAR   { background:#F1F5F9; color:#6B7280; }
        .accion-SIN_CB    { background:#EDE9FE; color:#5B21B6; }

        .rank-suc  { display:inline-block; background:#dbeafe; color:#1e3a5f; border-radius:4px; padding:1px 6px; font-size:11px; font-weight:700; }
        .rank-glob { display:inline-block; background:#f3e8ff; color:#4c1d95; border-radius:4px; padding:1px 6px; font-size:11px; font-weight:700; }

        .tend-CRE { color:#16a34a; font-weight:700; font-size:14px; }
        .tend-DEC { color:#dc2626; font-weight:700; font-size:14px; }
        .tend-EST { color:#6c757d; font-size:14px; }

        .delta-up   { color:#16a34a; font-size:10px; font-weight:700; margin-left:3px; }
        .delta-down { color:#dc2626; font-size:10px; font-weight:700; margin-left:3px; }
        .delta-eq   { color:#9ca3af; font-size:10px; margin-left:3px; }

        .deficit-val { font-weight:700; color:#dc2626; }
        .deficit-cero { color:#6c757d; }

        .cbv3-tip  { cursor:help; border-bottom:2px dashed #6c757d; font-weight:700; }
        .cob-tooltip { cursor:help; border-bottom:2px dashed #6c757d; font-weight:600; }

        /* Tooltip box compartido */
        #floatTip {
            position:fixed; display:none; z-index:99999; pointer-events:none;
            background:#1a2535; color:#e2e8f0;
            border:1px solid #2d4a6e; border-radius:10px;
            padding:14px 18px; min-width:260px; max-width:340px;
            box-shadow:0 8px 32px rgba(0,0,0,.45);
            font-size:12.5px; line-height:1.7;
        }
        #floatTip .ft-title { font-weight:700; font-size:13px; color:#7dd3fc; border-bottom:1px solid #2d4a6e; padding-bottom:6px; margin-bottom:8px; }
        #floatTip .ft-row   { display:flex; justify-content:space-between; gap:14px; }
        #floatTip .ft-lbl   { color:#94a3b8; }
        #floatTip .ft-val   { font-weight:600; color:#e2e8f0; white-space:nowrap; }
        #floatTip .ft-sep   { border:none; border-top:1px solid #2d4a6e; margin:5px 0; }
        #floatTip .ft-head  { font-weight:700; font-size:11.5px; color:#7dd3fc; margin-top:4px; }
        #floatTip .ft-hi    { color:#86efac; font-weight:700; }
        #floatTip .ft-result { margin-top:6px; padding-top:6px; border-top:1px solid #2d4a6e; font-size:15px; font-weight:700; color:#fde68a; text-align:center; }

        /* Grid */
        #gridV3 { font-size:12.5px; }
        #gridV3 thead th { background:#272b35; color:#fff; text-align:center; padding:9px 5px; position:sticky; top:0; z-index:2; }
        #gridV3 tbody td { vertical-align:middle; text-align:center; padding:5px 4px; }
        #gridV3 tbody td.col-modelo { text-align:left; min-width:160px; }
        .modelo-nombre { font-size:14px; font-weight:700; color:#212529; line-height:1.3; }
        .inv-grid { display:grid; grid-template-columns:1fr 1fr; gap:1px; font-size:10.5px; min-width:72px; }
        .inv-grid span { color:#374151; white-space:nowrap; }
        .inv-grid span.neg { color:#dc2626; }
        .accion-btn { padding:2px 9px; font-size:11px; }
    </style>
</head>
<body>
<form id="form1" runat="server">
    <asp:ScriptManager ID="sm1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

    <div class="topnav">
        <a href="TrasladosDashboardPlanner.aspx"><i class="fa fa-home"></i> Inicio</a>
        <a href="#">CB v1</a>
        <a href="TrasladosCuadroBasico.aspx" class="active">CB v3</a>
    </div>

    <div class="v3-header">
        <div>
            <div class="v3-title">Cuadro Básico &mdash; Motor CBv3 <span class="badge badge-primary">v3</span></div>
            <div class="v3-sub">
                Pesos W1=2.0 / W2=1.5 / W3=1.0 &nbsp;|&nbsp; LT=3d &nbsp;|&nbsp; Piso estacional=1.0 &nbsp;|&nbsp;
                Tendencia: crecimiento &gt;1.2×, declinación &lt;0.8× &nbsp;|&nbsp;
                Tiers ORO/PLATA/BRONCE/BAJO por Pareto semanal ponderado
            </div>
        </div>
        <div>
            <asp:Button ID="btnHistorial" runat="server" Text="Historial" CssClass="btn btn-outline-secondary btn-sm mr-1" />
        </div>
    </div>

    <div class="container-fluid mt-3">

        <%-- Controles de carga --%>
        <div class="row mb-2">
            <div class="col-md-4">
                <label style="font-size:12px;color:#6c757d;">Sucursal</label>
                <asp:DropDownList ID="drpSucursales" runat="server" Width="100%" CssClass="ui search dropdown fluid" AutoPostBack="false"></asp:DropDownList>
            </div>
            <div class="col-md-2 d-flex align-items-end">
                <asp:Button ID="btnCargar" runat="server" Text="Cargar" CssClass="btn btn-info btn-block" />
            </div>
            <div class="col-md-3 d-flex align-items-end">
                <asp:Button ID="btnAplicarSugerido" runat="server" Text="Aplicar CB v3 (todos visibles)"
                    CssClass="btn btn-success btn-block"
                    OnClientClick="return confirmarAplicarV3();" />
            </div>
        </div>

        <%-- Métricas fila 1: Por tier --%>
        <div class="metric-row metric-row-1">
            <div class="metric-card">
                <div class="metric-label">Modelos activos</div>
                <div class="metric-value"><asp:Literal ID="litTotal" runat="server" Text="0" /></div>
            </div>
            <div class="metric-card m-oro">
                <div class="metric-label">⭐ ORO</div>
                <div class="metric-value"><asp:Literal ID="litOro" runat="server" Text="0" /></div>
            </div>
            <div class="metric-card m-plata">
                <div class="metric-label">🥈 PLATA</div>
                <div class="metric-value"><asp:Literal ID="litPlata" runat="server" Text="0" /></div>
            </div>
            <div class="metric-card m-bronce">
                <div class="metric-label">🥉 BRONCE</div>
                <div class="metric-value"><asp:Literal ID="litBronce" runat="server" Text="0" /></div>
            </div>
            <div class="metric-card m-bajo">
                <div class="metric-label">🔵 BAJO</div>
                <div class="metric-value"><asp:Literal ID="litBajoT" runat="server" Text="0" /></div>
            </div>
            <div class="metric-card m-def">
                <div class="metric-label">Déficit total (uds)</div>
                <div class="metric-value"><asp:Literal ID="litDeficit" runat="server" Text="0" /></div>
            </div>
            <div class="metric-card m-muerto">
                <div class="metric-label">Muertos / Sin CB</div>
                <div class="metric-value"><asp:Literal ID="litMuerto" runat="server" Text="0" /></div>
            </div>
        </div>

        <%-- Métricas fila 2: Por estado/acción --%>
        <div class="metric-row metric-row-2">
            <div class="metric-card m-res">
                <div class="metric-label">RESURTIR</div>
                <div class="metric-value"><asp:Literal ID="litResurtir" runat="server" Text="0" /></div>
            </div>
            <div class="metric-card m-man">
                <div class="metric-label">MANTENER</div>
                <div class="metric-value"><asp:Literal ID="litMantener" runat="server" Text="0" /></div>
            </div>
            <div class="metric-card m-red">
                <div class="metric-label">REDUCIR / RETIRAR</div>
                <div class="metric-value"><asp:Literal ID="litReducir" runat="server" Text="0" /></div>
            </div>
            <div class="metric-card m-critico">
                <div class="metric-label">Críticos (inv=0)</div>
                <div class="metric-value"><asp:Literal ID="litCritico" runat="server" Text="0" /></div>
            </div>
            <div class="metric-card m-bajost">
                <div class="metric-label">Bajo CB</div>
                <div class="metric-value"><asp:Literal ID="litBajo" runat="server" Text="0" /></div>
            </div>
            <div class="metric-card m-ok">
                <div class="metric-label">OK</div>
                <div class="metric-value"><asp:Literal ID="litOk" runat="server" Text="0" /></div>
            </div>
            <div class="metric-card m-sobre">
                <div class="metric-label">Sobre-stock</div>
                <div class="metric-value"><asp:Literal ID="litSobreStock" runat="server" Text="0" /></div>
            </div>
        </div>

        <%-- Barra de filtros --%>
        <div class="filters-bar">
            <div>
                <label>Filtrar por modelo</label>
                <input type="text" id="fltModelo" class="form-control form-control-sm" placeholder="Ej: Terra, SX1..." />
            </div>
            <div>
                <label>Tier</label>
                <select id="fltTier" class="form-control form-control-sm">
                    <option value="">Todos</option>
                    <option>ORO</option><option>PLATA</option><option>BRONCE</option><option>BAJO</option>
                </select>
            </div>
            <div>
                <label>Acción CBv3</label>
                <select id="fltAccion" class="form-control form-control-sm">
                    <option value="">Todas</option>
                    <option>RESURTIR</option><option>MANTENER</option><option>REDUCIR</option><option>RETIRAR</option>
                </select>
            </div>
            <div>
                <label>Tendencia</label>
                <select id="fltTend" class="form-control form-control-sm">
                    <option value="">Todas</option>
                    <option value="CRECIENDO">↑ Creciendo</option>
                    <option value="ESTABLE">→ Estable</option>
                    <option value="DECRECIENDO">↓ Decreciendo</option>
                </select>
            </div>
            <div>
                <label>Tipo CB</label>
                <select id="fltGap" class="form-control form-control-sm">
                    <option value="">Todos</option>
                    <option value="abajo">CB &lt; Sug.</option>
                    <option value="arriba">CB &gt; Sug.</option>
                    <option value="igual">CB = Sug.</option>
                </select>
            </div>
            <div><label>&nbsp;</label><button type="button" id="btnFiltrar" class="btn btn-secondary btn-sm">Filtrar</button></div>
            <div><label>&nbsp;</label><button type="button" id="btnLimpiar" class="btn btn-light btn-sm">Limpiar</button></div>
        </div>

        <%-- Barra de acciones masivas --%>
        <div class="bulk-bar" id="bulkBar" style="display:none;">
            <div><strong><span id="bulkCount">0</span></strong> modelos seleccionados:</div>
            <div>
                <button type="button" class="btn btn-sm btn-light" onclick="bulkAction('mas1')">CB +1</button>
                <button type="button" class="btn btn-sm btn-light" onclick="bulkAction('menos1')">CB −1</button>
                <button type="button" class="btn btn-sm btn-success" onclick="bulkAction('sugerido')">Usar CB v3</button>
                <button type="button" class="btn btn-sm btn-warning" onclick="bulkAction('descontinuar')">Descontinuar (CB=0)</button>
            </div>
        </div>

        <%-- Grid principal --%>
        <div style="background:#fff; border:1px solid #dee2e6; border-radius:8px; overflow-x:auto;">
            <asp:GridView ID="gridV3" runat="server" AutoGenerateColumns="false"
                ClientIDMode="Static" CssClass="table table-hover mb-0" Width="100%"
                OnRowDataBound="gridV3_RowDataBound">
                <Columns>
                    <%-- 0: Checkbox --%>
                    <asp:TemplateField HeaderText="">
                        <HeaderTemplate><input type="checkbox" id="chkAll" title="Seleccionar todos" /></HeaderTemplate>
                        <ItemTemplate>
                            <input type="checkbox" class="chkRow"
                                data-modelo='<%# Eval("MODELO") %>'
                                data-cb='<%# Eval("CB_Actual") %>'
                                data-sug='<%# Eval("CB_Propuesto") %>'
                                data-vd='<%# ToI4(Eval("VtaDiaFinal")) %>'
                                data-cob='<%# Eval("CobDias") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- 1: Modelo --%>
                    <asp:TemplateField HeaderText="Modelo">
                        <ItemTemplate>
                            <div style="display:flex;align-items:flex-start;gap:6px;">
                                <div style="display:flex;flex-direction:column;gap:3px;flex-shrink:0;padding-top:2px;">
                                    <span class="rank-suc"  title="Ranking en sucursal">&#x1F3E0; <%# Eval("Ranking") %></span>
                                    <span class="rank-glob" title="Ranking global">&#x1F30E; <%# Eval("RankingGlobal") %></span>
                                </div>
                                <div>
                                    <div class="modelo-nombre"><%# Eval("MODELO") %></div>
                                    <div style="margin-top:2px;">
                                        <span class='tier-badge tier-<%# Eval("ModelTier") %>'><%# Eval("ModelTier") %></span>
                                        &nbsp;<span style="font-size:10px;color:#6c757d;"><%# Eval("CODE") %></span>
                                        &nbsp;<a href='<%# "TrasladosCargaMacroBI.aspx?modelo=" & Server.UrlEncode(Eval("MODELO").ToString()) %>'
                                               target="_blank" class="badge badge-info" onclick="event.stopPropagation();">R|E</a>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                        <ItemStyle CssClass="col-modelo" />
                    </asp:TemplateField>

                    <%-- 2: Tendencia + promedios de grupo --%>
                    <asp:TemplateField HeaderText="Tend. / Prom (u/sem)">
                        <ItemTemplate>
                            <div style="text-align:center;line-height:1.6;">
                                <%# TendIcon(Eval("TrendDir")) %>
                                <br/>
                                <span style="font-size:10px;color:#374151;" title="Prom S1-S4 / S5-S8 / S9-S12 (unidades/semana)">
                                    <%# String.Format("{0:0.00}", Eval("Prom1a4")) %> /
                                    <%# String.Format("{0:0.00}", Eval("Prom5a8")) %> /
                                    <%# String.Format("{0:0.00}", Eval("Prom9a12")) %>
                                </span>
                                <br/>
                                <span style="font-size:10px;color:#6c757d;" title="Índice de tendencia">
                                    idx=<%# String.Format("{0:0.00}", Eval("IdxTend")) %>
                                </span>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- 3: Vta mes --%>
                    <asp:TemplateField HeaderText="Vta mes">
                        <ItemTemplate><%# Convert.ToInt32(Eval("VtaMes")) %></ItemTemplate>
                    </asp:TemplateField>

                    <%-- 4: Inventario compacto --%>
                    <asp:TemplateField HeaderText="Fís / Dsp / Ped / Trn">
                        <ItemTemplate>
                            <div class="inv-grid">
                                <span title="Físico">Fís:<%# Eval("Fisico") %></span>
                                <span title="Despachos">Dsp:<%# Eval("Despacho") %></span>
                                <span title="Pedidos comprometidos" class='<%# If(SafeInt2(Eval("Comprometido"))>0,"neg","") %>'>Ped:<%# Eval("Comprometido") %></span>
                                <span title="Tránsito">Trn:<%# Eval("Transito") %></span>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- 5: Físico Ajustado --%>
                    <asp:TemplateField HeaderText="Fís. Adj.">
                        <ItemTemplate>
                            <strong><%# Eval("InvAjustado") %></strong>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- 6: Cobertura días (tooltip de inventario) --%>
                    <asp:TemplateField HeaderText="Cob. días">
                        <ItemTemplate>
                            <span class="cob-tooltip"
                                data-fisico='<%# Eval("Fisico") %>'
                                data-despacho='<%# Eval("Despacho") %>'
                                data-transito='<%# Eval("Transito") %>'
                                data-pedidos='<%# Eval("Comprometido") %>'
                                data-vdf='<%# ToI4(Eval("VtaDiaFinal")) %>'
                                data-cob='<%# ToI4(Eval("CobDias")) %>'>
                                <%# FmtCob(Eval("CobDias")) %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- 7: CB actual --%>
                    <asp:BoundField DataField="CB_Actual" HeaderText="CB act." />

                    <%-- 8: CB v3 propuesto (con tooltip CBv3 completo) --%>
                    <asp:TemplateField HeaderText="CB v3">
                        <ItemTemplate>
                            <span class="cbv3-tip"
                                data-modelo='<%# Eval("MODELO") %>'
                                data-tier='<%# Eval("ModelTier") %>'
                                data-storetier='<%# Eval("StoreTier") %>'
                                data-cycle='<%# GetCycleInt(Eval("StoreTier")) %>'
                                data-z='<%# GetZInt(Eval("ModelTier")) %>'
                                data-tf='<%# GetTFInt(Eval("ModelTier")) %>'
                                data-cbv3='<%# GetCBv3Attr(Eval("Prom1a4"),Eval("Prom5a8"),Eval("Prom9a12"),
                                    Eval("VtaDiaBase"),Eval("TrendDir"),Eval("AjTend"),
                                    Eval("VtaDiaFinal"),Eval("Sigma"),
                                    Eval("DCiclo"),Eval("DLT"),Eval("SS"),
                                    Eval("CBBase"),Eval("CBxTier")) %>'
                                data-cb='<%# Eval("CB_Actual") %>'
                                data-sug='<%# Eval("CB_Propuesto") %>'
                                data-vd='<%# ToI4(Eval("VtaDiaFinal")) %>'
                                data-cob='<%# Eval("CobDias") %>'>
                                <%# Eval("CB_Propuesto") %>
                                <%# GetDeltaHtml(Eval("CB_Actual"), Eval("CB_Propuesto")) %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- 9: Déficit --%>
                    <asp:TemplateField HeaderText="Déficit">
                        <ItemTemplate>
                            <span class='<%# If(SafeInt2(Eval("Deficit"))>0,"deficit-val","deficit-cero") %>'>
                                <%# If(SafeInt2(Eval("Deficit"))>0, Eval("Deficit").ToString(), "&mdash;") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- 10: Acción --%>
                    <asp:TemplateField HeaderText="Acción CBv3">
                        <ItemTemplate>
                            <span class='accion-badge accion-<%# Eval("Accion") %>'><%# Eval("Accion") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- 11: Últ. Venta --%>
                    <asp:TemplateField HeaderText="Últ. Venta">
                        <ItemTemplate>
                            <span style="font-size:11px;white-space:nowrap;">
                                <%# If(String.IsNullOrEmpty(Eval("UltVenta").ToString()), "&mdash;", Eval("UltVenta").ToString()) %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- 12: Últ. Traslado (coloreado por RowDataBound) --%>
                    <asp:TemplateField HeaderText="Últ. Traslado">
                        <ItemTemplate>
                            <span style="font-size:11px;white-space:nowrap;font-weight:600;">
                                <%# If(String.IsNullOrEmpty(Eval("UltTraslado").ToString()), "&mdash;", Eval("UltTraslado").ToString()) %>
                            </span>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <%-- 13: Botón Aceptar --%>
                    <asp:TemplateField HeaderText="Acción">
                        <ItemTemplate>
                            <button type="button" class="btn btn-sm btn-success accion-btn btnAceptarV3"
                                data-modelo='<%# Eval("MODELO") %>'
                                data-cb='<%# Eval("CB_Actual") %>'
                                data-sug='<%# Eval("CB_Propuesto") %>'
                                data-vd='<%# ToI4(Eval("VtaDiaFinal")) %>'
                                data-cob='<%# Eval("CobDias") %>'>Aceptar</button>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <div style="font-size:11px;color:#6c757d;margin-top:8px;">
            <strong>Leyenda:</strong>
            <span style="margin-left:8px;">⭐ ORO = top 60% ventas ponderadas · 🥈 PLATA = 60-80% · 🥉 BRONCE = 80-95% · 🔵 BAJO = resto</span> &nbsp;|&nbsp;
            <span>↑↓→ Tendencia = Prom(S1-S4) / Prom(S9-S12) vs umbrales 1.2 / 0.8</span> &nbsp;|&nbsp;
            <span>CB v3 = D_Ciclo + D_LT + SS, redondeado hacia arriba, ajustado por tier</span>
        </div>
    </div>

    <%-- Tooltip flotante compartido --%>
    <div id="floatTip"></div>

    <%-- Modal de motivo --%>
    <div class="modal fade" id="modalMotivo">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background:#272b35;color:white;">
                    <h5 class="modal-title">Motivo del cambio</h5>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label style="font-size:12px;">Motivo</label>
                        <select id="cboMotivo" class="form-control form-control-sm">
                            <option value="">&mdash; Seleccione &mdash;</option>
                            <option>Sugerencia CBv3</option>
                            <option>Temporada alta</option>
                            <option>Temporada baja</option>
                            <option>Reposición</option>
                            <option>Liquidación</option>
                            <option>Modelo descontinuado</option>
                            <option>Ajuste manual</option>
                        </select>
                    </div>
                    <div class="form-group">
                        <label style="font-size:12px;">Observación (opcional)</label>
                        <textarea id="txtObs" class="form-control form-control-sm" rows="2"></textarea>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-sm btn-secondary" data-dismiss="modal">Cancelar</button>
                    <button type="button" class="btn btn-sm btn-success" id="btnConfirmarMotivo">Confirmar</button>
                </div>
            </div>
        </div>
    </div>

</form>

<script>
    var WHS_ACTUAL   = '<%= drpSucursales.SelectedValue %>';
    var USUARIO      = '<%= Session("Name") %>';
    var pendingAction = null;

    // ─── Dropdown Semantic UI ─────────────────────────────
    $(function(){
        $("#drpSucursales").dropdown({ fullTextSearch: true });

        // ─── Seleccionar todos ───────────────────────────
        $('#chkAll').on('change', function(){
            $('.chkRow:visible').prop('checked', this.checked);
            actualizarBulkBar();
        });

        // ─── Filtros en tiempo real ───────────────────────
        $('#fltModelo').on('keyup', aplicarFiltros);
        $('#fltTier, #fltAccion, #fltTend, #fltGap').on('change', aplicarFiltros);
        $('#btnFiltrar').on('click', aplicarFiltros);
        $('#btnLimpiar').on('click', function(){
            $('#fltModelo').val('');
            $('#fltTier,#fltAccion,#fltTend,#fltGap').val('');
            $('#gridV3 tbody tr').show();
            actualizarBulkBar();
        });

        // ─── Checkbox rows ───────────────────────────────
        $(document).on('change', '.chkRow', actualizarBulkBar);

        // ─── Botón Aceptar individual ────────────────────
        $(document).on('click', '.btnAceptarV3', function(){
            var $b   = $(this);
            var mod  = $b.data('modelo');
            var cb   = parseInt($b.data('cb'))  || 0;
            var sug  = parseInt($b.data('sug')) || 0;
            var vd   = (parseInt($b.data('vd')) || 0) / 10000;
            var cob  = parseFloat($b.data('cob')) || 0;
            if (sug === cb){
                iziToast.info({title:'Sin cambio', message:'CB v3 = CB actual', position:'topRight'});
                return;
            }
            abrirModal({tipo:'individual', modelo:mod, cb_anterior:cb, cb_nuevo:sug, sugerido:sug, vd:vd, cob:cob, origen:'CBV3'});
        });

        // ─── Confirmar motivo ────────────────────────────
        $('#btnConfirmarMotivo').on('click', function(){
            var mot = $('#cboMotivo').val();
            if (!mot){ iziToast.warning({title:'Atención', message:'Seleccione un motivo', position:'topRight'}); return; }
            var obs = $('#txtObs').val().trim();
            $('#modalMotivo').modal('hide');
            ejecutarCambio(mot + (obs ? ' — ' + obs : ''));
        });

        // ─── Tooltip flotante (cob-tooltip y cbv3-tip) ───
        var $tip = $('#floatTip');
        $(document).on('mousemove', function(e){
            var $t = $(e.target).closest('.cob-tooltip, .cbv3-tip');
            if (!$t.length){ $tip.hide(); return; }
            var x = e.clientX + 18, y = e.clientY - 14;
            if (x + 350 > $(window).width()) x = e.clientX - 360;
            var html = $t.hasClass('cbv3-tip') ? buildCBv3Tip($t) : buildCobTip($t);
            $tip.html(html).css({display:'block', left:x+'px', top:y+'px'});
        });
    });

    // ─── Filtros ──────────────────────────────────────────
    function aplicarFiltros(){
        var q    = ($('#fltModelo').val()||'').toLowerCase();
        var tier = $('#fltTier').val();
        var acc  = $('#fltAccion').val();
        var tend = $('#fltTend').val();
        var gap  = $('#fltGap').val();

        $('#gridV3 tbody tr').each(function(){
            var $r    = $(this);
            var modelo = $r.find('td:eq(1)').text().toLowerCase();
            var tierRow = $r.find('.tier-badge').text().trim();
            var accRow  = $r.find('.accion-badge').text().trim();
            var cbvSug  = $r.find('.cbv3-tip');
            var tendRow = cbvSug.attr('data-cbv3') ? cbvSug.attr('data-cbv3').split('|')[4] : '';
            var cbAct   = parseInt(cbvSug.attr('data-cb'))  || 0;
            var cbSug2  = parseInt(cbvSug.attr('data-sug')) || 0;
            var ok = true;
            if (q    && modelo.indexOf(q) === -1)      ok = false;
            if (tier && tierRow !== tier)               ok = false;
            if (acc  && accRow  !== acc)                ok = false;
            if (tend && tendRow !== tend)               ok = false;
            if (gap === 'abajo'  && !(cbAct < cbSug2))  ok = false;
            if (gap === 'arriba' && !(cbAct > cbSug2))  ok = false;
            if (gap === 'igual'  && !(cbAct === cbSug2)) ok = false;
            $r.toggle(ok);
        });
        actualizarBulkBar();
    }

    // ─── Bulk bar ────────────────────────────────────────
    function actualizarBulkBar(){
        var n = $('.chkRow:checked').length;
        $('#bulkCount').text(n);
        $('#bulkBar').toggle(n > 0);
    }

    function bulkAction(tipo){
        var items = $('.chkRow:checked').map(function(){
            return {
                modelo: $(this).data('modelo'),
                cb:     parseInt($(this).data('cb'))  || 0,
                sug:    parseInt($(this).data('sug')) || 0,
                vd:     (parseInt($(this).data('vd')) || 0) / 10000,
                cob:    parseFloat($(this).data('cob')) || 0
            };
        }).get();
        if (!items.length) return;
        abrirModal({tipo:'masivo', tipoMasivo:tipo, items:items, origen:'MASIVO_V3'});
    }

    // ─── Modal y ejecución ───────────────────────────────
    function abrirModal(action){
        pendingAction = action;
        $('#cboMotivo').val(''); $('#txtObs').val('');
        $('#modalMotivo').modal('show');
    }

    function ejecutarCambio(motivo){
        if (!pendingAction) return;
        if (pendingAction.tipo === 'individual'){
            postUpdate(pendingAction.modelo, pendingAction.cb_anterior, pendingAction.cb_nuevo,
                       pendingAction.sugerido, pendingAction.vd, pendingAction.cob, motivo, pendingAction.origen);
        } else {
            var items  = pendingAction.items;
            var tipoM  = pendingAction.tipoMasivo;
            var total  = items.length, done = 0, fails = 0;
            items.forEach(function(it){
                var nuevoC = it.cb;
                if      (tipoM === 'mas1')         nuevoC = it.cb + 1;
                else if (tipoM === 'menos1')        nuevoC = Math.max(0, it.cb - 1);
                else if (tipoM === 'sugerido')      nuevoC = it.sug;
                else if (tipoM === 'descontinuar')  nuevoC = 0;
                if (nuevoC === it.cb){ done++; return; }
                $.ajax({
                    url:'UpdateCuadroBasicoV2.ashx', type:'POST',
                    data:{maximo:nuevoC, whscode:WHS_ACTUAL, modelo:it.modelo,
                          cb_anterior:it.cb, cb_sugerido:it.sug, venta_diaria:it.vd,
                          dias_cobertura:it.cob, motivo:motivo,
                          origen:tipoM==='descontinuar'?'DESCONTINUAR':'MASIVO_V3'},
                    success:function(){ done++; if(done===total) iziToast.success({title:'OK',message:'Procesados '+done,position:'topRight'}); },
                    error:function(){   fails++; done++; if(done===total) iziToast.warning({title:'Parcial',message:done+' procesados, '+fails+' errores',position:'topRight'}); }
                });
            });
        }
    }

    function postUpdate(modelo, cbAnt, cbNuevo, sugerido, vd, cob, motivo, origen){
        $.ajax({
            url:'UpdateCuadroBasicoV2.ashx', type:'POST',
            data:{maximo:cbNuevo, whscode:WHS_ACTUAL, modelo:modelo,
                  cb_anterior:cbAnt, cb_sugerido:sugerido, venta_diaria:vd,
                  dias_cobertura:cob, motivo:motivo, origen:origen},
            success:function(){
                iziToast.success({title:'Actualizado', message:modelo+' CB: '+cbAnt+' → '+cbNuevo, position:'topRight', timeout:2500});
                setTimeout(function(){ location.reload(); }, 900);
            },
            error:function(){ iziToast.error({title:'Error', message:'No se pudo actualizar '+modelo, position:'topRight'}); }
        });
    }

    function confirmarAplicarV3(){
        Swal.fire({
            title:'¿Aplicar CB v3 a todos?',
            text:'Se aplicará el CB v3 propuesto a todos los modelos visibles donde difiera del CB actual.',
            icon:'question', showCancelButton:true, confirmButtonText:'Sí, aplicar'
        }).then(function(r){ if (r.isConfirmed) __doPostBack('<%= btnAplicarSugerido.UniqueID %>', ''); });
        return false;
    }

    // ─── Tooltip: Cobertura (cob-tooltip) ────────────────
    function buildCobTip(el){
        var f   = parseInt(el.attr('data-fisico'))   || 0;
        var d   = parseInt(el.attr('data-despacho')) || 0;
        var t   = parseInt(el.attr('data-transito')) || 0;
        var p   = parseInt(el.attr('data-pedidos'))  || 0;
        var vd  = (parseInt(el.attr('data-vdf'))     || 0) / 10000;
        var cobRaw = parseInt(el.attr('data-cob'))   || 0;
        var adj = f + d + t - p;
        var cobTxt = vd > 0 ? (adj / vd).toFixed(1) + ' días' : '∞ días';

        return '<div class="ft-title">Cobertura = Fís. Ajustado ÷ Vta/día (CBv3)</div>' +
            row('Físico', f + ' uds') + row('+ Despachos', d + ' uds') +
            row('+ Tránsito', t + ' uds') + row('− Pedidos', p + ' uds') +
            '<div class="ft-row"><span class="ft-lbl ft-hi">= Fís. Ajustado</span><span class="ft-val ft-hi">' + adj + ' uds</span></div>' +
            '<hr class="ft-sep"/>' +
            row('÷ Vta/día (CBv3)', vd.toFixed(4) + ' uds/día') +
            '<div class="ft-result">= ' + cobTxt + '</div>';
    }

    // ─── Tooltip: Desglose CBv3 (cbv3-tip) ───────────────
    function buildCBv3Tip(el){
        var tier = el.attr('data-tier') || '';
        var st   = el.attr('data-storetier') || 'B';
        var cyc  = parseInt(el.attr('data-cycle')) || 7;
        var z    = (parseInt(el.attr('data-z'))  || 12800) / 10000;
        var tf   = (parseInt(el.attr('data-tf')) || 10000) / 10000;
        var cbv3 = (el.attr('data-cbv3') || '').split('|');
        if (cbv3.length < 13) return '<div class="ft-title">Sin datos CBv3</div>';

        var p1     = parseInt(cbv3[0])  / 10000;
        var p2     = parseInt(cbv3[1])  / 10000;
        var p3     = parseInt(cbv3[2])  / 10000;
        var vdb    = parseInt(cbv3[3])  / 10000;
        var tend   = cbv3[4];
        var ajtend = parseInt(cbv3[5])  / 10000;
        var vdf    = parseInt(cbv3[6])  / 10000;
        var sigma  = parseInt(cbv3[7])  / 10000;
        var dciclo = parseInt(cbv3[8])  / 10000;
        var dlt    = parseInt(cbv3[9])  / 10000;
        var ss     = parseInt(cbv3[10]) / 10000;
        var cbbase = parseInt(cbv3[11]) / 10000;
        var cbxtier= parseInt(cbv3[12]) / 10000;
        var cbprop = parseInt(el.attr('data-sug')) || 0;
        var cbact  = parseInt(el.attr('data-cb'))  || 0;
        var modelo = el.attr('data-modelo') || '';

        var tendIco = tend==='CRECIENDO' ? '↑ CRECIENDO' : (tend==='DECRECIENDO' ? '↓ DECRECIENDO' : '→ ESTABLE');
        var tendColor = tend==='CRECIENDO' ? '#86efac' : (tend==='DECRECIENDO' ? '#fca5a5' : '#94a3b8');

        return '<div class="ft-title">CB v3 &mdash; ' + modelo + '</div>' +
            row('Tier modelo', '<b>' + tier + '</b> (Z=' + z.toFixed(3) + ', Factor=' + tf.toFixed(2) + ')') +
            row('Tier tienda', st + ' (Ciclo=' + cyc + 'd · LT=3d)') +
            '<hr class="ft-sep"/>' +
            '<div class="ft-head">DEMANDA PONDERADA (W1=2.0 / W2=1.5 / W3=1.0)</div>' +
            row('Prom S1-S4 (×2.0)', p1.toFixed(3) + ' u/sem') +
            row('Prom S5-S8 (×1.5)', p2.toFixed(3) + ' u/sem') +
            row('Prom S9-S12 (×1.0)', p3.toFixed(3) + ' u/sem') +
            row('Vta/día base', vdb.toFixed(4)) +
            '<hr class="ft-sep"/>' +
            '<div class="ft-head" style="color:' + tendColor + '">TENDENCIA: ' + tendIco + ' (×' + ajtend.toFixed(2) + ')</div>' +
            row('Vta/día con tend.', (vdb * ajtend).toFixed(4)) +
            row('Vta/día FINAL', '<b>' + vdf.toFixed(4) + '</b>') +
            '<hr class="ft-sep"/>' +
            '<div class="ft-head">COMPONENTES CB = D_Ciclo + D_LT + SS</div>' +
            row('D_Ciclo (' + cyc + 'd)', dciclo.toFixed(4)) +
            row('D_LT (3d)', dlt.toFixed(4)) +
            row('SS (Z×σ×√' + (cyc+3) + ')', ss.toFixed(4) + '  (σ=' + sigma.toFixed(4) + ')') +
            row('CB base', cbbase.toFixed(4)) +
            row('× Factor tier', cbxtier.toFixed(4)) +
            '<div class="ft-result">CB PROPUESTO = ⌈' + cbxtier.toFixed(2) + '⌉ = ' + cbprop + ' u.</div>';
    }

    function row(lbl, val){
        return '<div class="ft-row"><span class="ft-lbl">' + lbl + '</span><span class="ft-val">' + val + '</span></div>';
    }
</script>
</body>
</html>
