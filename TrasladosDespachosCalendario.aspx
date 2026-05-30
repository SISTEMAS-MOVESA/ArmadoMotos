<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosDespachosCalendario.aspx.vb" Inherits="TrasladosDespachosCalendario" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Calendario de Despachos</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/fullcalendar@6.1.8/index.global.min.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://cdn.jsdelivr.net/npm/fullcalendar@6.1.8/index.global.min.js"></script>
    <style>
        body { background:#f4f6f9; font-size:0.85rem; }

        /* ── topnav ── */
        .topnav { background:#1a252f; display:flex; flex-direction:row; align-items:center; flex-wrap:wrap; padding:0 8px; }
        .topnav .brand { font-weight:700; font-size:0.95rem; color:#fff; padding:12px 16px; display:flex; align-items:center; }
        .topnav a { color:#cdd3d8; padding:12px 14px; text-decoration:none; font-size:0.85rem; display:inline-block; white-space:nowrap; }
        .topnav a:hover { background:rgba(255,255,255,0.08); color:#fff; }
        .topnav a.nav-active { background:rgba(255,255,255,0.15); color:#fff; font-weight:600; }
        .topnav a i { margin-right:4px; }

        /* ── calendar card ── */
        #cal-card .card-header { background:#1a252f; color:#fff; }
        #cal-card .card-body { padding:16px; }

        /* ── FullCalendar overrides ── */
        .fc .fc-toolbar-title { font-size:1.1rem; font-weight:700; }
        .fc .fc-button { font-size:0.78rem !important; padding:4px 10px !important; }
        .fc .fc-button-primary { background-color:#1a252f !important; border-color:#1a252f !important; }
        .fc .fc-button-primary:hover { background-color:#2c3e50 !important; border-color:#2c3e50 !important; }
        .fc .fc-button-primary:not(:disabled).fc-button-active { background-color:#007bff !important; border-color:#007bff !important; }
        .fc-daygrid-event { white-space:normal !important; overflow:visible !important; height:auto !important; }
        .fc-event-title { white-space:normal !important; overflow:visible !important; text-overflow:unset !important; }
        .fc-event-main { padding:2px 4px; }
        .fc-daygrid-event-dot { display:none !important; }
        .fc .fc-list-event:hover td { background:#f0f4f8 !important; cursor:pointer; }
        .fc .fc-list-event-time { font-size:0.72rem; }
        .fc-popover { z-index:9999 !important; }

        /* ── legend ── */
        .legend-dot { display:inline-block; width:12px; height:12px; border-radius:3px; margin-right:5px; }

        /* ── stat chips ── */
        .stat-chip { background:#e9ecef; border-radius:4px; padding:4px 12px; font-size:0.78rem; font-weight:600; }
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

    <div class="container-fluid py-3">

        <!-- toolbar row -->
        <div class="d-flex align-items-center mb-3" style="gap:10px; flex-wrap:wrap;">
            <a href="TrasladosDespachos.aspx" class="btn btn-sm btn-outline-secondary">
                <i class="fa fa-arrow-left"></i> Volver a Despachos
            </a>
            <h5 class="mb-0 ml-2" style="color:#1a252f; font-weight:700;">
                <i class="fa fa-calendar mr-1"></i> Calendario de Despachos
            </h5>
            <div class="ml-auto d-flex" style="gap:8px; flex-wrap:wrap;" id="statsBar">
                <span class="stat-chip"><i class="fa fa-truck"></i> Total: <strong id="statTotal">0</strong></span>
                <span class="stat-chip"><i class="fa fa-motorcycle"></i> Unidades: <strong id="statUnds">0</strong></span>
                <span class="stat-chip"><i class="fa fa-cube"></i> Espacios: <strong id="statEsp">0</strong></span>
            </div>
        </div>

        <!-- Calendar card -->
        <div class="card shadow-sm" id="cal-card">
            <div class="card-body p-2">
                <div id="calendar"></div>
            </div>
        </div>

    </div>

    <footer class="text-center text-muted py-2 mt-2" style="font-size:0.72rem; border-top:1px solid #dee2e6; background:#fff;">
        WebDesign RJ &mdash; Copyright &copy; 2022 <a href="#">Movesa</a>. All rights reserved.
    </footer>

</form>

<script>
    var PALETTE = ['#007bff','#28a745','#dc3545','#fd7e14','#6f42c1','#20c997','#e83e8c','#17a2b8','#ffc107','#343a40'];
    var rutaColorMap = {};
    var rutaIndex = 0;

    function colorForRuta(ruta) {
        if (!rutaColorMap[ruta]) {
            rutaColorMap[ruta] = PALETTE[rutaIndex % PALETTE.length];
            rutaIndex++;
        }
        return rutaColorMap[ruta];
    }

    function buildLegend() { /* removed */ }

    document.addEventListener('DOMContentLoaded', function () {
        var calendarEl = document.getElementById('calendar');

        var calendar = new FullCalendar.Calendar(calendarEl, {
            initialView: 'dayGridWeek',
            firstDay: 1,
            hiddenDays: [0],
            locale: 'es',
            timeZone: 'local',
            headerToolbar: {
                left:   'prev,next today',
                center: 'title',
                right:  'dayGridMonth,dayGridWeek,listWeek'
            },
            buttonText: {
                today:     'Hoy',
                month:     'Mes',
                week:      'Semana',
                listWeek:  'Lista'
            },
            eventDisplay: 'block',
            height: 'auto',
            contentHeight: 680,
            dayMaxEvents: 4,
            moreLinkText: function(n) { return '+' + n + ' mas'; },

            events: function (info, successCallback, failureCallback) {
                fetch('TrasladosDespachosCalendario.aspx/ObtenerEventos', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: '{}'
                })
                .then(function(r) { return r.json(); })
                .then(function(data) {
                    var events = (data.d || []).map(function(ev) {
                        return {
                            title:      ev.title,
                            start:      ev.start,
                            allDay:     true,
                            color:      colorForRuta(ev.ruta || 'SIN RUTA'),
                            extendedProps: {
                                planid:        ev.planid,
                                id:            ev.id,
                                ruta:          ev.ruta,
                                almdestino:    ev.almdestino,
                                nalmacen:      ev.nalmacen,
                                totalcantidad: ev.totalcantidad,
                                totalespacios: ev.totalespacios
                            }
                        };
                    });
                    // update stats
                    var totUnds = 0, totEsp = 0;
                    events.forEach(function(e) {
                        totUnds += parseInt(e.extendedProps.totalcantidad) || 0;
                        totEsp  += parseInt(e.extendedProps.totalespacios) || 0;
                    });
                    document.getElementById('statTotal').textContent = events.length;
                    document.getElementById('statUnds').textContent  = totUnds;
                    document.getElementById('statEsp').textContent   = totEsp;
                    buildLegend();
                    successCallback(events);
                })
                .catch(function(err) {
                    console.error('Error al cargar eventos:', err);
                    failureCallback(err);
                });
            },

            eventContent: function (arg) {
                var p = arg.event.extendedProps;
                var wrap = document.createElement('div');
                wrap.style.cssText = 'padding:2px 5px; border-radius:3px; cursor:pointer; line-height:1.3;';
                wrap.innerHTML =
                    '<span style="font-weight:700; font-size:0.72rem;">' + (p.ruta || '') + '</span>' +
                    '<span style="font-size:0.68rem; display:block; opacity:.9;">' + (p.almdestino || '') + ' &mdash; ' + (p.nalmacen || '') + '</span>' +
                    '<span style="font-size:0.68rem; opacity:.85;">' + (p.totalcantidad || 0) + ' unds &bull; ' + (p.totalespacios || 0) + ' esp</span>';
                return { domNodes: [wrap] };
            },

            eventClick: function (info) {
                // close FC popover so it doesn't sit on top of Swal
                document.querySelectorAll('.fc-popover').forEach(function(el) { el.remove(); });
                var p   = info.event.extendedProps;
                var col = info.event.backgroundColor;
                Swal.fire({
                    title: '<span style="color:' + col + '">Despacho #' + (p.id || '') + '</span>',
                    html:
                        '<table class="table table-sm table-bordered mt-2" style="font-size:0.82rem; text-align:left;">' +
                        '<tr><th>Plan ID</th><td>' + (p.planid || '-') + '</td></tr>' +
                        '<tr><th>Ruta</th><td>' + (p.ruta || '-') + '</td></tr>' +
                        '<tr><th>Almac&eacute;n</th><td>' + (p.almdestino || '-') + ' &mdash; ' + (p.nalmacen || '-') + '</td></tr>' +
                        '<tr><th>Fecha</th><td>' + info.event.startStr + '</td></tr>' +
                        '<tr><th>Unidades</th><td>' + (p.totalcantidad || 0) + '</td></tr>' +
                        '<tr><th>Espacios</th><td>' + (p.totalespacios || 0) + '</td></tr>' +
                        '</table>',
                    icon: 'info',
                    confirmButtonText: '<i class="fa fa-truck"></i> Ver Despachos',
                    confirmButtonColor: '#1a252f',
                    showCancelButton: true,
                    cancelButtonText: 'Cerrar',
                    cancelButtonColor: '#6c757d'
                }).then(function(result) {
                    if (result.isConfirmed) {
                        window.location.href = 'TrasladosDespachos.aspx';
                    }
                });
            },

            // list view: show all details inline
            eventDidMount: function(info) {
                if (info.view.type === 'listWeek') {
                    var p = info.event.extendedProps;
                    var td = info.el.querySelector('.fc-list-event-title');
                    if (td) {
                        td.innerHTML =
                            '<strong>' + (p.ruta || '') + '</strong> &mdash; ' +
                            (p.almdestino || '') + ' (' + (p.nalmacen || '') + ')' +
                            ' &nbsp;<span class="badge badge-secondary">' + (p.totalcantidad || 0) + ' unds</span>' +
                            ' <span class="badge badge-dark">' + (p.totalespacios || 0) + ' esp</span>' +
                            ' &nbsp;<small class="text-muted">Plan: ' + (p.planid || '') + '</small>';
                    }
                }
            }
        });

        calendar.render();
    });
</script>
</body>
</html>
