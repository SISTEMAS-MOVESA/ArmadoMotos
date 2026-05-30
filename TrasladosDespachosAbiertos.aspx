<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosDespachosAbiertos.aspx.vb" Inherits="TrasladosDespachosAbiertos" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Asignar Camion</title>
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
        .rango-btn { font-size:0.75rem; padding:3px 9px; }

        /* ── tabs ── */
        .nav-tabs .nav-link { font-size:0.8rem; padding:6px 14px; color:#495057; }
        .nav-tabs .nav-link.active { font-weight:700; color:#1a252f; border-bottom:2px solid #1a252f; }

        /* ── compact grids ── */
        .grid-compact { width:100%; }
        .grid-compact thead th,
        .grid-compact tbody td { font-size:0.72rem !important; padding:3px 5px !important; vertical-align:middle !important; }
        .grid-compact thead th { position:sticky; top:0; z-index:2; background:#343a40 !important; color:#fff !important; border-bottom:2px solid #dee2e6 !important; white-space:nowrap; }

        .card-scroll { overflow-y:auto; overflow-x:auto; max-height:480px; }

        /* ── progress bars ── */
        .progress { height:16px; min-width:60px; }
        .progress-bar { font-size:0.68rem; line-height:16px; }

        /* ── checkbox ── */
        .document-checkbox input[type="checkbox"] { transform:scale(1.3); cursor:pointer; }

        /* ── modal header ── */
        .modal-header { background:#1a252f; color:#fff; }
        .modal-header .close { color:#fff; opacity:.8; }
        .modal-header .close:hover { opacity:1; }
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
        <a href="TrasladosDespachos.aspx"><i class="fa fa-truck"></i> Despachos Abiertos</a>
        <a href="TrasladosDespachosAbiertos.aspx" class="nav-active"><i class="fa fa-plus-circle"></i> Asignar Cami&oacute;n</a>
        <a href="CerrarSesion.aspx" style="margin-left:auto;"><i class="fa fa-sign-out"></i> Cerrar Sesi&oacute;n</a>
    </nav>

    <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <asp:Label ID="lblPlanId" runat="server" Visible="false" />

    <!-- Toolbar -->
    <div class="toolbar-bar">
        <button type="button" class="btn btn-sm btn-outline-secondary rango-btn" onclick="setRango('mes')">Este mes</button>
        <button type="button" class="btn btn-sm btn-outline-secondary rango-btn" onclick="setRango('mesant')">Mes anterior</button>
        <button type="button" class="btn btn-sm btn-outline-secondary rango-btn" onclick="setRango('anio')">Este a&ntilde;o</button>
        <button type="button" class="btn btn-sm btn-outline-secondary rango-btn" onclick="setRango('anioant')">A&ntilde;o anterior</button>
        <asp:TextBox ID="txtDesde" runat="server" TextMode="Date" CssClass="form-control form-control-sm" style="width:135px;" />
        <asp:TextBox ID="txtHasta" runat="server" TextMode="Date" CssClass="form-control form-control-sm" style="width:135px;" />
        <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-sm btn-primary" Text="Filtrar" />
        <span class="text-muted ml-1" style="font-size:0.75rem;"><asp:Label ID="lblRango" runat="server" /></span>
        <div class="ml-auto d-flex align-items-center" style="gap:6px; flex-wrap:wrap;">
            <asp:Button ID="btnCrearCamion" runat="server" CssClass="btn btn-sm btn-info"
                Text="Crear Camion" OnClientClick="$('#modalCamion').modal('show'); return false;" />
            <asp:Button ID="btnAgregarCamion" runat="server" CssClass="btn btn-sm btn-warning" Text="Agregar a Camion" />
            <asp:DropDownList ID="drpListadoCamionesDisp" runat="server" CssClass="form-control form-control-sm" style="width:160px;" />
        </div>
    </div>

    <div class="container-fluid py-3">

        <!-- Tabs -->
        <ul class="nav nav-tabs mb-0" id="mainTabs" role="tablist">
            <li class="nav-item">
                <a class="nav-link active" id="tab1-tab" data-toggle="tab" href="#tab1" role="tab">
                    <i class="fa fa-truck mr-1"></i>Despachos Abiertos
                </a>
            </li>
            <li class="nav-item">
                <a class="nav-link" id="tab2-tab" data-toggle="tab" href="#tab2" role="tab">
                    <i class="fa fa-list mr-1"></i>Detalle Motos
                </a>
            </li>
            <li class="nav-item">
                <a class="nav-link" id="tab3-tab" data-toggle="tab" href="#tab3" role="tab">
                    <i class="fa fa-car mr-1"></i>Camiones Abiertos
                </a>
            </li>
            <li class="nav-item">
                <a class="nav-link" id="tab4-tab" data-toggle="tab" href="#tab4" role="tab">
                    <i class="fa fa-align-justify mr-1"></i>Detalle Camiones
                </a>
            </li>
        </ul>

        <div class="tab-content border border-top-0 bg-white rounded-bottom shadow-sm">

            <!-- Tab 1: Despachos Abiertos -->
            <div class="tab-pane fade show active p-2" id="tab1" role="tabpanel">
                <div class="card-scroll">
                    <asp:GridView ID="gridDespachosAbiertos" runat="server"
                        AutoGenerateColumns="false"
                        CssClass="table table-sm table-bordered table-hover grid-compact mb-0"
                        Width="100%" EmptyDataText="Sin datos"
                        OnRowDataBound="gridDespachosAbiertos_RowDataBound">
                        <HeaderStyle CssClass="thead-dark" />
                        <Columns>
                            <asp:TemplateField HeaderText="Pick %">
                                <ItemTemplate>
                                    <div class="progress">
                                        <div class="progress-bar progress-bar-striped bg-info" role="progressbar"
                                            aria-valuemin="0"
                                            aria-valuenow='<%# Eval("Preparado") %>'
                                            aria-valuemax='<%# Eval("Unds") %>'
                                            style='width:<%# Eval("Porcentaje") %>%'>
                                            <%# FormatNumber(Eval("Porcentaje"), 1, TriState.True) %>%
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="28px">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbDocument" runat="server" CssClass="document-checkbox" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DespachoId"    HeaderText="Despacho" />
                            <asp:BoundField DataField="PlanId"        HeaderText="Plan" />
                            <asp:BoundField DataField="FechaDespacho" HeaderText="F.Despacho" />
                            <asp:BoundField DataField="DiasRestantes" HeaderText="Dias" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Canal"         HeaderText="Canal" />
                            <asp:BoundField DataField="Ruta"          HeaderText="Ruta" />
                            <asp:BoundField DataField="Almacenes"     HeaderText="Almacenes" />
                            <asp:BoundField DataField="TotalAlm"      HeaderText="Alm" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="TotalFalt"     HeaderText="Faltante" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Unds"          HeaderText="Unds" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="TotalEspacios" HeaderText="Espacios" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Preparado"     HeaderText="Preparado" ItemStyle-HorizontalAlign="Center" />
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/delete.png"
                                Text="Eliminar" CommandName="Eliminar" HeaderText=""
                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="28px">
                                <ControlStyle Height="18px" Width="18px" />
                            </asp:ButtonField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <!-- Tab 2: Detalle Motos -->
            <div class="tab-pane fade p-2" id="tab2" role="tabpanel">
                <div class="d-flex align-items-center mb-2" style="gap:8px;">
                    <asp:TextBox ID="txtFilter" runat="server" Text="" placeholder="Filtrar..."
                        CssClass="form-control form-control-sm" style="width:200px;" />
                    <button type="button" class="btn btn-sm btn-success" onclick="exportarExcel('gridDespachosAbiertosMotos')">
                        <i class="fa fa-file-excel-o mr-1"></i>Exportar
                    </button>
                </div>
                <div class="card-scroll">
                    <asp:GridView ID="gridDespachosAbiertosMotos" runat="server"
                        AutoGenerateColumns="true"
                        CssClass="table table-sm table-bordered table-hover grid-compact mb-0"
                        Width="100%" EmptyDataText="Sin datos"
                        OnRowDataBound="gridDespachosAbiertosMotos_RowDataBound">
                        <HeaderStyle CssClass="thead-dark" />
                        <Columns>
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/delete.png"
                                Text="Eliminar" CommandName="Eliminar" HeaderText=""
                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="28px">
                                <ControlStyle Height="18px" Width="18px" />
                            </asp:ButtonField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <!-- Tab 3: Camiones Abiertos -->
            <div class="tab-pane fade p-2" id="tab3" role="tabpanel">
                <div class="card-scroll">
                    <asp:GridView ID="gridCamiones" runat="server"
                        AutoGenerateColumns="true"
                        CssClass="table table-sm table-bordered table-hover grid-compact mb-0"
                        Width="100%" EmptyDataText="Sin datos">
                        <HeaderStyle CssClass="thead-dark" />
                        <Columns>
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/view.png"
                                Text="Ver" CommandName="Ver" HeaderText=""
                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="28px">
                                <ControlStyle Height="18px" Width="18px" />
                            </asp:ButtonField>
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/Dacion04.gif"
                                Text="Imprimir" CommandName="Imprimir" HeaderText=""
                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="28px">
                                <ControlStyle Height="18px" Width="18px" />
                            </asp:ButtonField>
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/delete.png"
                                Text="Eliminar" CommandName="Eliminar" HeaderText=""
                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="28px">
                                <ControlStyle Height="18px" Width="18px" />
                            </asp:ButtonField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <!-- Tab 4: Detalle Camiones -->
            <div class="tab-pane fade p-2" id="tab4" role="tabpanel">
                <div class="card-scroll">
                    <asp:GridView ID="gridCamionesDetalle" runat="server"
                        AutoGenerateColumns="true"
                        CssClass="table table-sm table-bordered table-hover grid-compact mb-0"
                        Width="100%" EmptyDataText="Sin datos">
                        <HeaderStyle CssClass="thead-dark" />
                        <Columns>
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/delete.png"
                                Text="Eliminar" CommandName="Eliminar" HeaderText=""
                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="28px">
                                <ControlStyle Height="18px" Width="18px" />
                            </asp:ButtonField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

        </div>
    </div>

    <!-- Modal: Crear Camion -->
    <div class="modal fade" id="modalCamion" tabindex="-1" role="dialog" aria-labelledby="modalCamionTitle" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalCamionTitle"><i class="fa fa-truck mr-2"></i>Datos del Cami&oacute;n</h5>
                    <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label class="font-weight-bold">Fecha Salida</label>
                        <asp:TextBox ID="txtFechaCamion" runat="server" TextMode="Date" CssClass="form-control form-control-sm" />
                    </div>
                    <div class="form-row">
                        <div class="form-group col-6">
                            <label class="font-weight-bold">Placa Veh&iacute;culo</label>
                            <asp:DropDownList ID="drpPlacas" runat="server" CssClass="form-control form-control-sm" />
                        </div>
                        <div class="form-group col-6">
                            <label class="font-weight-bold">Motorista</label>
                            <asp:DropDownList ID="drpMotorista" runat="server" CssClass="form-control form-control-sm" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnCancelColoresMotos" runat="server" CssClass="btn btn-sm btn-secondary"
                        Text="Cancelar" OnClientClick="$('#modalCamion').modal('hide'); return false;" />
                    <asp:Button ID="btnCamionCrear" runat="server" CssClass="btn btn-sm btn-success" Text="Crear Camion" />
                </div>
            </div>
        </div>
    </div>

    <footer class="text-center text-muted py-2 mt-2" style="font-size:0.72rem; border-top:1px solid #dee2e6; background:#fff;">
        WebDesign RJ &mdash; Copyright &copy; 2022 <a href="#">Movesa</a>. All rights reserved.
    </footer>

</form>

<script>
    // ── date range presets ──
    function setRango(tipo) {
        var hoy = new Date();
        var y = hoy.getFullYear(), m = hoy.getMonth();
        var d1, d2;
        if (tipo === 'mes')    { d1 = new Date(y,m,1);   d2 = new Date(y,m+1,0); }
        else if (tipo === 'mesant') { d1 = new Date(y,m-1,1); d2 = new Date(y,m,0); }
        else if (tipo === 'anio')   { d1 = new Date(y,0,1);   d2 = new Date(y,11,31); }
        else if (tipo === 'anioant'){ d1 = new Date(y-1,0,1); d2 = new Date(y-1,11,31); }
        function fmt(d) { return d.getFullYear()+'-'+String(d.getMonth()+1).padStart(2,'0')+'-'+String(d.getDate()).padStart(2,'0'); }
        $('#<%= txtDesde.ClientID %>').val(fmt(d1));
        $('#<%= txtHasta.ClientID %>').val(fmt(d2));
        $('#<%= btnFiltrar.ClientID %>').click();
    }

    // ── text filter for tab 2 grid ──
    $('#<%= txtFilter.ClientID %>').on('keyup', function () {
        var val = $(this).val().toLowerCase();
        $('#<%= gridDespachosAbiertosMotos.ClientID %> tbody tr').each(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(val) > -1);
        });
    });

    // ── export to Excel (visible rows only) ──
    function exportarExcel(gridId) {
        var tablaOriginal = document.getElementById(gridId);
        if (!tablaOriginal) { alert('No se encontró la tabla.'); return; }
        var tablaNueva = document.createElement('table');
        tablaNueva.border = 1;
        for (var i = 0; i < tablaOriginal.rows.length; i++) {
            var filaOrig = tablaOriginal.rows[i];
            if (filaOrig.style.display === 'none') continue;
            var filaNueva = tablaNueva.insertRow(-1);
            for (var j = 1; j < filaOrig.cells.length; j++) {
                var c = filaNueva.insertCell(-1);
                c.innerHTML = filaOrig.cells[j].innerText || filaOrig.cells[j].textContent;
            }
        }
        var html = '<html xmlns:x="urn:schemas-microsoft-com:office:excel"><head><meta charset="UTF-8"></head><body>' + tablaNueva.outerHTML + '</body></html>';
        var blob = new Blob([html], { type: 'application/vnd.ms-excel' });
        var url = window.URL.createObjectURL(blob);
        var a = document.createElement('a');
        a.href = url; a.download = 'despachos_motos_' + new Date().toISOString().slice(0,10) + '.xls';
        document.body.appendChild(a); a.click(); document.body.removeChild(a);
    }
</script>
</body>
</html>
