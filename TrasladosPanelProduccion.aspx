<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosPanelProduccion.aspx.vb" Inherits="TrasladosPanelProduccion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Planificador</title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.3.0/css/all.min.css" />
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css" />
    <link rel="stylesheet" href="plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css" />
    <link rel="stylesheet" href="plugins/icheck-bootstrap/icheck-bootstrap.min.css" />
    <link rel="stylesheet" href="plugins/jqvmap/jqvmap.min.css" />
    <link rel="stylesheet" href="dist/css/adminlte.min.css" />
    <link rel="stylesheet" href="plugins/overlayScrollbars/css/OverlayScrollbars.min.css" />
    <link rel="stylesheet" href="plugins/daterangepicker/daterangepicker.css" />
    <link rel="stylesheet" href="plugins/summernote/summernote-bs4.min.css" />

    <link
        href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css"
        rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"></script>

    <link
        rel="stylesheet"
        href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>

    <script src="https://d3js.org/d3.v6.min.js"></script>

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.4/semantic.min.css" />

    <link
        rel="stylesheet"
        href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.js"></script>
    <script
        src="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.js"
        integrity="sha512-gnoBksrDbaMnlE0rhhkcx3iwzvgBGz6mOEj4/Y5ZY09n55dYddx6+WYc72A55qEesV8VX2iMomteIwobeGK1BQ=="
        crossorigin="anonymous"
        referrerpolicy="no-referrer"></script>
    <link
        rel="stylesheet"
        href="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.css"
        integrity="sha512-3quBdRGJyLy79hzhDDcBzANW+mVqPctrGCfIPosHQtMKb3rKsCxfyslzwlz2wj1dT8A7UX+sEvDjaUv+WExQrA=="
        crossorigin="anonymous"
        referrerpolicy="no-referrer" />


    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/tom-select/2.3.1/css/tom-select.bootstrap4.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/tom-select/2.3.1/js/tom-select.complete.js"></script>
    <script src="index.js"></script>
    <style>
        .blue-cell {
            background: #ddf4ff;
            color: #2185d0;
        }

        thead input {
            width: 100% !important;
            padding: 3px !important;
            box-sizing: border-box !important;
        }

        .sticky-header th {
            position: sticky !important;
            top: 0 !important;
            z-index: 2 !important;
            background-color: #343a40 !important;
            color: white !important;
            border-bottom: 2px solid #dee2e6 !important;
            padding: 8px !important;
            text-align: left !important;
        }

        .sticky-grid {
            border-collapse: collapse !important;
            width: 100% !important;
        }

            .sticky-grid th,
            .sticky-grid td {
                border: 1px solid #dee2e6 !important;
            }

        table.fixedHeader-floating {
            position: fixed !important;
            background-color: white;
        }

            table.fixedHeader-floating.no-footer {
                border-bottom-width: 0;
            }

        table.fixedHeader-locked {
            position: absolute !important;
            background-color: white;
        }

        @media print {
            table.fixedHeader-floating {
                display: none;
            }
        }

        span {
            display: flex;
            justify-content: center;
            align-content: center;
        }

        .btn-lg-custom {
            height: 4rem;
            width: 100%;
        }

        .document-checkbox input[type="checkbox"] {
            transform: scale(1.5);
            width: 20px;
            height: 20px;
            cursor: pointer;
        }
    </style>
</head>
<body>
    <div class="ui active dimmer" style="position: fixed !important;" id="screen_loader">
        <div class="ui big text loader">Loading</div>
    </div>
    <form id="form1" runat="server">
        <!-- Top Navigation Menu -->
        <div class="topnav">
            <style>
                /* Style the navigation menu */
                .topnav {
                    overflow: hidden;
                    background-color: #333;
                    position: relative;
                }

                    /* Hide the links inside the navigation menu (except for logo/home) */
                    .topnav #myLinks {
                        display: none;
                    }

                    /* Style navigation menu links */
                    .topnav a {
                        color: white;
                        padding: 14px 16px;
                        text-decoration: none;
                        font-size: 17px;
                        display: block;
                    }

                        /* Style the hamburger menu */
                        .topnav a.icon {
                            background: black;
                            display: block;
                            position: absolute;
                            right: 0;
                            top: 0;
                        }

                        /* Add a grey background color on mouse-over */
                        .topnav a:hover {
                            background-color: #ddd;
                            color: black;
                        }

                /* Style the active link (or home/logo) */
                .active {
                    background-color: black;
                    color: white;
                }
            </style>
            <a href="TrasladosDashboardPlanner.aspx" class="active">
                <img src="Imagenes/mnegra.png" width="20" height="20" alt="" />
                Inicio
            </a>
            <!-- Navigation links (hidden by default) -->
            <div id="myLinks">
                <a href="Maindashboard.aspx">Menu Principal</a>
                <a href="TrasladosCuadroBasico.aspx">Cuadro Basico</a>
                <a href="TrasladosPanelProduccion.aspx">Panel Planificación</a>
                <a href="TrasladosDDashboard.aspx">Ver Planificación</a>
                <a href="TrasladosDespachos.aspx">Despachos Abiertos</a>
                <a href="TrasladosDespachosAbiertos.aspx">Asignar Camion</a>
                <a href="CerrarSesion.aspx">Cerrar Sesion</a>
            </div>
            <!-- "Hamburger menu" / "Bar icon" to toggle the navigation links -->
            <a href="javascript:void(0);" class="icon" onclick="myFunction()">
                <i class="fa fa-bars"></i>
            </a>
            <script>
                /* Toggle between showing and hiding the navigation menu links when the user clicks on the hamburger menu / bar icon */
                function myFunction() {
                    var x = document.getElementById("myLinks");
                    if (x.style.display === "block") {
                        x.style.display = "none";
                    } else {
                        x.style.display = "block";
                    }
                }
            </script>
        </div>


        <div class="container-fluid">
            <style>
                .custom-btn {
                    width: 160px; /* Tamaño fijo */
                    height: 80px; /* Alto uniforme */
                    white-space: normal; /* Permite salto de línea */
                    overflow: hidden; /* Evita que el texto se salga */
                    text-align: center;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    font-size: 16px; /* Tamaño de fuente uniforme */
                    word-wrap: break-word; /* Rompe palabras si es necesario */
                }
            </style>

            <div class="ui form">
                <div class="five fields">
                    <div class="field three wide">
                        <label>Periodo</label>
                        <select id="cbb-periodo" class="cbb-tomselect" data-value="2025">
                            <option selected="selected" value='2025'>PERIODO 2025</option>
                            <option value='2024'>PERIODO 2024</option>
                        </select>
                    </div>
                    <div class="field six wide">
                        <label>Semana</label>
                        <select id="cbb-semana" class="cbb-tomselect"></select>
                    </div>
                    <div class="field">
                        <label class="invisible">P</label>
                        <div class="ui blue icon button" id="btn-filtrar-indicador-semana"><i class="filter icon"></i></div>
                    </div>
                </div>
            </div>

            <div class="ui selectable very long scrolling container fluid px-0">
                <table class="ui head stuck unstackable tiny selectable celled table fluid tablesorter" id="tbl-indicador-despachos-headers">
                    <thead>
                        <tr>
                            <th colspan="2"></th>
                            <th colspan="5">KPIs</th>
                            <th colspan="6">PLANIFICADOS</th>
                            <th colspan="6">DESPACHADOS</th>
                        </tr>
                        <tr>
                            <th>CANAL</th>
                            <th>ZONA</th>

                            <th>META</th>
                            <th>MOTOS<br>
                                PLANIF.</th>
                            <th>MOTOS<br>
                                DESP.</th>
                            <th>LOGRO</th>
                            <th>PROY.</th>

                            <th>LU</th>
                            <th>MA</th>
                            <th>MI</th>
                            <th>JU</th>
                            <th>VI</th>
                            <th>SA</th>

                            <th>LU</th>
                            <th>MA</th>
                            <th>MI</th>
                            <th>JU</th>
                            <th>VI</th>
                            <th>SA</th>
                        </tr>
                    </thead>
                    <tbody></tbody>
                    <tfoot></tfoot>
                </table>
            </div>
            <br />

            <div class="ui selectable very long scrolling container fluid px-0">
                <table class="ui head stuck unstackable tiny selectable celled table fluid tablesorter" id="tbl-indicador-despachos-detalle">
                    <thead>
                        <tr>
                            <th colspan="2"></th>
                            <th colspan="5">KPIs</th>
                            <th colspan="6">PLANIFICADOS</th>
                            <th colspan="6">DESPACHADOS</th>
                        </tr>
                        <tr>
                            <th>CANAL</th>
                            <th>ZONA</th>

                            <th>META</th>
                            <th>MOTOS<br>
                                PLANIF.</th>
                            <th>MOTOS<br>
                                DESP.</th>
                            <th>LOGRO</th>
                            <th>PROY.</th>

                            <th>LU</th>
                            <th>MA</th>
                            <th>MI</th>
                            <th>JU</th>
                            <th>VI</th>
                            <th>SA</th>

                            <th>LU</th>
                            <th>MA</th>
                            <th>MI</th>
                            <th>JU</th>
                            <th>VI</th>
                            <th>SA</th>
                        </tr>
                    </thead>
                    <tbody></tbody>
                    <tfoot></tfoot>
                </table>
            </div>
            <%--            <div class="my-3" style="display: flex; justify-content: center; align-content: center; align-items: center; display: none;">
                <h1>ESTADISTICO | Semana: <%= DatePart(DateInterval.WeekOfYear, DateAdd(DateInterval.WeekOfYear, -1, Date.Now)) %>
                </h1>
            </div>
            <div class="row my-3" style="display: flex; justify-content: center; align-content: center; display: none;">
                <table
                    id="tablaEstadistica0"
                    class="table table-bordered text-center w-75">
                    <thead class="thead-dark">
                        <tr>
                            <th>Canal</th>
                            <th>Lunes</th>
                            <th>%</th>
                            <th>Martes</th>
                            <th>%</th>
                            <th>Miercoles</th>
                            <th>%</th>
                            <th>Jueves</th>
                            <th>%</th>
                            <th>Viernes</th>
                            <th>%</th>
                            <th>Sabado</th>
                            <th>%</th>
                            <th>Total</th>
                            <th>%</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="repeater2" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("Canal") %></td>
                                    <td><%# String.Format("{0:N0}", Eval("Lunes")) %></td>
                                    <td>
                                        <%# String.Format("{0:0.00}%", Eval("PorcLunes")) %>
                                    </td>
                                    <td><%# String.Format("{0:N0}", Eval("Martes")) %></td>
                                    <td>
                                        <%# String.Format("{0:0.00}%", Eval("PorcMartes")) %>
                                    </td>
                                    <td><%# String.Format("{0:N0}", Eval("Miércoles")) %></td>
                                    <td>
                                        <%# String.Format("{0:0.00}%", Eval("PorcMiércoles")) %>
                                    </td>
                                    <td><%# String.Format("{0:N0}", Eval("Jueves")) %></td>
                                    <td>
                                        <%# String.Format("{0:0.00}%", Eval("PorcJueves")) %>
                                    </td>
                                    <td><%# String.Format("{0:N0}", Eval("Viernes")) %></td>
                                    <td>
                                        <%# String.Format("{0:0.00}%", Eval("PorcViernes")) %>
                                    </td>
                                    <td><%# String.Format("{0:N0}", Eval("Sábado")) %></td>
                                    <td>
                                        <%# String.Format("{0:0.00}%", Eval("PorcSabado")) %>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                    <tfoot>
                        <tr>
                            <th>Totales</th>
                            <td>
                                <asp:Literal ID="litZeroTotalLunes" runat="server" />
                            </td>
                            <td>100%</td>
                            <td>
                                <asp:Literal ID="litZeroTotalMartes" runat="server" />
                            </td>
                            <td>100%</td>
                            <td>
                                <asp:Literal ID="litZeroTotalMiercoles" runat="server" />
                            </td>
                            <td>100%</td>
                            <td>
                                <asp:Literal ID="litZeroTotalJueves" runat="server" />
                            </td>
                            <td>100%</td>
                            <td>
                                <asp:Literal ID="litZeroTotalViernes" runat="server" />
                            </td>
                            <td>100%</td>
                            <td>
                                <asp:Literal ID="litZeroTotalSabado" runat="server" />
                            </td>
                            <td>100%</td>
                        </tr>
                    </tfoot>
                </table>
            </div>
            <div
                class="my-3" style="display: flex; justify-content: center; align-content: center; align-items: center;">
                <h4>ESTADISTICO | Semana: <%= DatePart(DateInterval.WeekOfYear, Date.Now) %> </h4>
            </div>
            <div class="row"  style="display: flex; justify-content: center; align-content: center; align-items: center;">
                <div class="col-2">
                    <!-- Incluye SweetAlert2 -->
                    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
                    <button id="btnExtraerDatos" class="btn btn-block btn-success">Extraer Datos del Panel</button>
                    <pre id="jsonResult" style="max-height: 300px; overflow: auto; background: #f7f7f7; padding: 8px"></pre>
                </div>
            </div>
            <div class="row my-3" style="display: flex; justify-content: center; align-content: center">
                <table
                    id="tablaEstadistica"
                    class="table table-bordered text-center w-75">
                    <thead class="thead-dark">
                        <tr>
                            <th>Canal</th>
                            <th>Lunes</th>
                            <th>%</th>
                            <th>Martes</th>
                            <th>%</th>
                            <th>Miércoles</th>
                            <th>%</th>
                            <th>Jueves</th>
                            <th>%</th>
                            <th>Viernes</th>
                            <th>%</th>
                            <th>Sábado</th>
                            <th>%</th>
                            <th>Total</th>
                            <th>%</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="repeater1" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("Canal") %></td>
                                    <td><%# String.Format("{0:N0}", Eval("Lunes")) %></td>
                                    <td><%# String.Format("{0:0.00}%", Eval("PorcLunes")) %></td>
                                    <td><%# String.Format("{0:N0}", Eval("Martes")) %></td>
                                    <td><%# String.Format("{0:0.00}%", Eval("PorcMartes")) %></td>
                                    <td><%# String.Format("{0:N0}", Eval("Miércoles")) %></td>
                                    <td><%# String.Format("{0:0.00}%", Eval("PorcMiércoles")) %></td>
                                    <td><%# String.Format("{0:N0}", Eval("Jueves")) %></td>
                                    <td><%# String.Format("{0:0.00}%", Eval("PorcJueves")) %></td>
                                    <td><%# String.Format("{0:N0}", Eval("Viernes")) %></td>
                                    <td><%# String.Format("{0:0.00}%", Eval("PorcViernes")) %></td>
                                    <td><%# String.Format("{0:N0}", Eval("Sábado")) %></td>
                                    <td><%# String.Format("{0:0.00}%", Eval("PorcSabado")) %></td>
                                    <td><%# String.Format("{0:N0}", Eval("TotalCanal")) %></td>
                                    <td><%# String.Format("{0:0.00}%", Eval("PorcCanal")) %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                    <tfoot>
                        <tr>
                            <th>Totales</th>
                            <td>
                                <asp:Literal ID="litTotalLunes" runat="server" /></td>
                            <td>100%</td>
                            <td>
                                <asp:Literal ID="litTotalMartes" runat="server" /></td>
                            <td>100%</td>
                            <td>
                                <asp:Literal ID="litTotalMiercoles" runat="server" /></td>
                            <td>100%</td>
                            <td>
                                <asp:Literal ID="litTotalJueves" runat="server" /></td>
                            <td>100%</td>
                            <td>
                                <asp:Literal ID="litTotalViernes" runat="server" /></td>
                            <td>100%</td>
                            <td>
                                <asp:Literal ID="litTotalSabado" runat="server" /></td>
                            <td>100%</td>
                            <td>
                                <asp:Literal ID="litTotalSemana" runat="server" /></td>
                            <td>
                                <asp:Literal ID="litPorcTotalSemana" runat="server" /></td>
                        </tr>
                    </tfoot>
                </table>
            </div>
            <div
                class="my-3"
                style="display: flex; justify-content: center; align-content: center; align-items: center;">
                <h4>PLANIFICACION | Semana: <%= DatePart(DateInterval.WeekOfYear,
            DateAdd(DateInterval.WeekOfYear, 1, Date.Now)) %>
                </h4>
            </div>
            <div
                class="row my-3"
                style="display: flex; justify-content: center; align-content: center">
                <div class="col-1">% Incremento Produccion</div>
                <div class="col-1">
                    <asp:TextBox ID="txtIncremento" runat="server" CssClass="form-control" placeholder="% Incremento" TextMode="Number" Text="15"></asp:TextBox>
                </div>

                <div class="col-1">Capacidad Diaria</div>
                <div class="col-1">
                    <asp:TextBox ID="txtCapacidadDiaria" runat="server" CssClass="form-control" placeholder="Capacidad" TextMode="Number" Text="120"></asp:TextBox>
                </div>
            </div>
            <div class="row my-3" style="display: flex; flex-direction:column; justify-content: center; align-content: center">
                <asp:Literal ID="litResumenCanal" runat="server" />

                <table
                    id="tablePronostico"
                    class="table table-bordered text-center w-75">
                    <thead class="thead-dark">
                        <tr>
                            <th>Canal</th>
                            <th>Ruta</th>
                            <th>% Parcipacion</th>
                            <th>Semana Anterior</th>
                            <th>Meta Calculada</th>
                            <th>Meta Asignada</th>
                            <th>% M Asignada</th>
                            <th>Despacho Asignado</th>
                            <th>Cargado</th>
                            <th>% Logro</th>
                            <th>% Proyeccion</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
            <div
                class="row my-3"
                style="display: flex; justify-content: center; align-content: center">
                <asp:Literal ID="litTablaCD" runat="server" />
            </div>--%>
        </div>
        <center>
            <div class="container">
                <footer class="main-footer">
                    <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy;
              2022 <a href="#">Movesa</a>.</strong>
                    All rights reserved.
                </footer>
            </div>
        </center>
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    </form>
</body>
</html>

<!-- AdminLTE App -->
<%--<script src="dist/js/adminlte.js"></script>--%>

<!-- DataTables  & Plugins -->
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.1/js/dataTables.buttons.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.html5.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.print.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.colVis.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
<link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />
<link href="https://cdn.datatables.net/buttons/1.7.1/css/buttons.dataTables.min.css" rel="stylesheet" type="text/css" />
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>


<script>

    const act_year = "<%= DateTime.Now.Year %>";
    const act_week = "<%= System.Globalization.CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) %>";
    $('#screen_loader').hide();

    constructTomSelects();

    $(`.txt-buscador[data-table]`).on('keyup', function () {
        var value = $(this).val().toLowerCase();
        const table = $(this).attr("data-table");

        $(`#${table} tbody tr`).filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });

    $(".item[data-tab]").on("click", function () {
        $(".item[data-tab]").removeClass("active");
        $(this).addClass("active");
    });

    $("#cbb-periodo").on("change", async function () {
        const periodo = $("#cbb-periodo").prop("tomselect").getValue();
        await rellenar_periodos_semanales(periodo);
    });

    $("#btn-filtrar-indicador-semana").on("click", function () {
        const periodo = $("#cbb-semana").prop("tomselect").getValue();
        const year = periodo.split("-")[0];
        const week = periodo.split("-")[1];
        cargar_periodo_semanal(year, week);
    })

    $(document).ready(async function () {
        await rellenar_periodos_semanales(act_year);
        $("#cbb-semana").prop("tomselect").setValue(`${act_year}-${act_week}`);
        cargar_periodo_semanal(act_year, act_week);
    })

    // ----------------

    async function obtener_periodos_semanales(periodo) {
        $("#screen_loader").show();
        let items = [];

        await $.ajax({
            url: "http://192.168.1.70/portal/modulo_logistica/services/metas.services.php?token=@WAyEterSOr",
            method: "POST",
            dataType: "JSON",
            data: { request: "obtener_periodos_semanales", data: { periodo } },
            success(_response) {
                $("#screen_loader").hide();
                items = _response;
            },
            error(_response) {
                $("#screen_loader").hide();
                console.log(_response);
            },
        });

        return items;
    }

    async function rellenar_periodos_semanales(periodo) {
        const semanas = await obtener_periodos_semanales(periodo);

        $("#cbb-semana").prop("tomselect").clearOptions();
        $("#cbb-semana").prop("tomselect").clear();
        semanas.forEach(item => {
            $("#cbb-semana").prop("tomselect").addOption({
                value: `${item.ANIO}-${item.NUM_SEMANA}`, text: `${item.ANIO}-${item.NUM_SEMANA} (${item.DESDE} / ${item.HASTA})`
            });
        });
    }


    function cargar_periodo_semanal(year, week) {

        const dias_semana = ["LU", "MA", "MI", "JU", "VI", "SA"];
        const renderRow = function (data, resaltarLlenos = true) {
            const tr = $("<tr></tr>");
            tr.append(`<td>${data.CANAL}</td>`);
            tr.append(`<td>${data.ZONA}</td>`);

            tr.append(`<td>${data.META}</td>`);
            tr.append(`<td>${data.PLANIFICADOS}</td>`);
            tr.append(`<td>${data.DESPACHADOS}</td>`);

            let _class = "";
            const logro = toDouble(data.LOGRO);
            if (logro >= 80) {
                _class = "green";
            } else if (logro >= 60 && logro < 80) {
                _class = "yellow";
            }

            tr.append(`<td class='${_class}'>${__FORMATOS.PORCENTAJE(data.LOGRO)}</td>`);
            tr.append(`<td>${__FORMATOS.PORCENTAJE(data.PROYECCION)}</td>`);

            dias_semana.forEach(dia => {
                const valor = toInt(data["P_" + dia]);
                const _class = valor > 0 && resaltarLlenos ? "blue" : "";
                tr.append(`<td class="${_class}">${__FORMATOS.UNIDADES(valor)}</td>`);
            });

            dias_semana.forEach(dia => {
                const valor = toInt(data["D_" + dia]);
                const _class = valor > 0 && resaltarLlenos ? "blue" : "";
                tr.append(`<td class="${_class}">${__FORMATOS.UNIDADES(valor)}</td>`);
            });

            return tr;
        }

        $("#screen_loader").show();
        $.ajax({
            url: "http://192.168.1.70/portal/modulo_logistica/services/metas.services.php?token=@WAyEterSOr",
            method: "POST",
            dataType: "JSON",
            data: { request: "obtener_detalle_periodo_semanal", data: { year, week } },
            success(_response) {
                console.log(_response);

                $("#screen_loader").hide();
                $("#tbl-indicador-despachos-headers").find("tbody,tfoot").empty();
                $("#tbl-indicador-despachos-detalle").find("tbody,tfoot").empty();

                const canales = Array.from(
                    new Map(_response.filter(x => x.NIVEL == 2).map(item => [item.CANAL, { CANAL: item.CANAL }])).values()
                );

                // ---------------------- HEADER ----------------------

                canales.forEach(c => {
                    const header = _response.filter(x => x.NIVEL == 2 && x.CANAL == c.CANAL);
                    const tr_h = renderRow(header[0], true);
                    //tr_h.addClass("active");
                    $("#tbl-indicador-despachos-headers tbody").append(tr_h);
                });

                const total_movesa = _response.filter(x => x.NIVEL == 1);
                const tr_m = renderRow(total_movesa[0], false);
                $("#tbl-indicador-despachos-headers tfoot").append(tr_m);

                // ---------------------- DETALLE ----------------------

                canales.forEach(c => {
                    const header = _response.filter(x => x.NIVEL == 2 && x.CANAL == c.CANAL);
                    const details = _response.filter(x => x.NIVEL == 3 && x.CANAL == c.CANAL);

                    details.forEach(item => {
                        const tr_d = renderRow(item, true);
                        $("#tbl-indicador-despachos-detalle tbody").append(tr_d);
                    });

                    const tr_h = renderRow(header[0], false);
                    tr_h.addClass("active");
                    $("#tbl-indicador-despachos-detalle tbody").append(tr_h);

                });

            },
            error(_response) {
                $("#screen_loader").hide();
                console.log(_response);
            },
        });
    }
</script>

<%--<script>
    document.addEventListener("DOMContentLoaded", async function () {
        const filas = document.querySelectorAll("#tablaCanalesCD tr");

        filas.forEach((fila) => {
            const primeraCelda = fila.querySelector("td");

            if (primeraCelda) {
                const texto = primeraCelda.textContent.trim().toUpperCase();
                if (texto === "CD") {
                    fila.style.backgroundColor = "#d4f7d4"; // verde claro
                } else if (texto === "CI") {
                    fila.style.backgroundColor = "#d4e7f7"; // azul claro
                }
            }
        });

        rellenar_tabla_pronosticos();
    });
</script>
<script>
    document.addEventListener("DOMContentLoaded", function () {
        const dias = [
            "Lunes",
            "Martes",
            "Miércoles",
            "Jueves",
            "Viernes",
            "Sábado",
        ];

        const tablaAnterior = document.getElementById("tablaEstadistica0");
        const tablaActual = document.getElementById("tablaEstadistica");

        const filasAnterior = tablaAnterior.querySelector("tbody").rows;
        const filasActual = tablaActual.querySelector("tbody").rows;

        for (let i = 0; i < filasActual.length; i++) {
            const filaActual = filasActual[i];
            const filaAnterior = filasAnterior[i];

            dias.forEach((dia, index) => {
                const colActual = filaActual.cells[index * 2 + 1]; // valores
                const colAnterior = filaAnterior.cells[index * 2 + 1];

                const valorActual =
                    parseInt(colActual.innerText.replace(/,/g, "")) || 0;
                const valorAnterior =
                    parseInt(colAnterior.innerText.replace(/,/g, "")) || 0;

                // Quitar íconos antiguos si existen
                const oldIcon = colActual.querySelector("span.arrow");
                if (oldIcon) oldIcon.remove();

                // Crear span con ícono SVG
                const span = document.createElement("span");
                span.classList.add("arrow");
                span.style.marginLeft = "5px";
                span.style.display = "inline-block";
                span.style.verticalAlign = "middle";

                if (valorActual > valorAnterior) {
                    // Flecha hacia arriba (verde)
                    span.innerHTML = `<svg xmlns="http://www.w3.org/2000/svg" width="12" height="12" fill="green" viewBox="0 0 16 16">
                    <path d="M8 3.293l6.364 6.364-1.414 1.414L8 6.121l-4.95 4.95-1.414-1.414L8 3.293z"/>
                </svg>`;
                } else if (valorActual < valorAnterior) {
                    // Flecha hacia abajo (rojo)
                    span.innerHTML = `<svg xmlns="http://www.w3.org/2000/svg" width="12" height="12" fill="red" viewBox="0 0 16 16">
                    <path d="M8 12.707l-6.364-6.364 1.414-1.414L8 10.879l4.95-4.95 1.414 1.414L8 12.707z"/>
                </svg>`;
                } else {
                    // Sin cambio (gris)
                    span.innerHTML = `<svg xmlns="http://www.w3.org/2000/svg" width="10" height="10" fill="gray" viewBox="0 0 16 16">
                    <circle cx="8" cy="8" r="2"/>
                </svg>`;
                }

                colActual.appendChild(span);
            });
        }
    });
</script>
<script>
    document.addEventListener("DOMContentLoaded", function () {
        const tabla = document.getElementById("tablaCanalesCD");
        const filas = tabla.querySelectorAll("tbody tr");

        // Índices de columnas de semanas (valores numéricos): Semana 19 a 25
        const indicesSemanas = [2, 4, 6, 8, 10, 12, 14];

        filas.forEach((fila) => {
            const celdas = fila.querySelectorAll("td");

            if (celdas.length >= 15) {
                for (let i = 1; i < indicesSemanas.length; i++) {
                    const idxActual = indicesSemanas[i];
                    const idxAnterior = indicesSemanas[i - 1];

                    const celdaActual = celdas[idxActual];
                    const celdaAnterior = celdas[idxAnterior];

                    const valorActual = parseFloat(celdaActual.textContent.trim());
                    const valorAnterior = parseFloat(celdaAnterior.textContent.trim());

                    if (!isNaN(valorActual) && !isNaN(valorAnterior)) {
                        const contenedor = document.createElement("div");
                        contenedor.style.display = "flex";
                        contenedor.style.flexDirection = "row";
                        contenedor.style.alignItems = "center";
                        contenedor.style.justifyContent = "center";

                        const spanValor = document.createElement("div");
                        spanValor.textContent = valorActual;

                        const spanDelta = document.createElement("div");

                        if (valorActual > valorAnterior) {
                            // Flecha arriba (verde)
                            spanDelta.innerHTML = `
                        <svg xmlns="http://www.w3.org/2000/svg" width="12" height="12" fill="green" viewBox="0 0 16 16">
                            <path d="M8 3.293l6.364 6.364-1.414 1.414L8 6.121l-4.95 4.95-1.414-1.414L8 3.293z"/>
                        </svg>`;
                        } else if (valorActual < valorAnterior) {
                            // Flecha abajo (rojo)
                            spanDelta.innerHTML = `
                        <svg xmlns="http://www.w3.org/2000/svg" width="12" height="12" fill="red" viewBox="0 0 16 16">
                            <path d="M8 12.707l-6.364-6.364 1.414-1.414L8 10.879l4.95-4.95 1.414 1.414L8 12.707z"/>
                        </svg>`;
                        } else {
                            // Punto gris (sin cambio)
                            spanDelta.innerHTML = `
                            <svg xmlns="http://www.w3.org/2000/svg" width="10" height="10" fill="gray" viewBox="0 0 16 16">
                            <circle cx="8" cy="8" r="2"/>
                            </svg>`;

                            //    spanDelta.innerHTML = `
                            //<svg xmlns="http://www.w3.org/2000/svg" width="10" height="10" fill="gray" viewBox="0 0 16 16">
                            //    <circle cx="8" cy="8" r="2"/>
                            //</svg>`;
                        }

                        contenedor.appendChild(spanValor);
                        contenedor.appendChild(spanDelta);
                        celdaActual.innerHTML = ""; // Limpiar celda
                        celdaActual.appendChild(contenedor);
                    }
                }
            }
        });
    });
</script>

<script>

    function rellenar_tabla_pronosticos() {

        $.ajax({
            url: "http://192.168.1.70/portal/modulo_logistica/services/portalArmadoMotos.services.php?token=@WAyEterSOr",
            method: "POST",
            dataType: "JSON",
            data: { request: "zonas_x_canal" },
            success(_response) {
                $("#tablePronostico tbody").empty();

                const canales = Array.from(
                    new Map(_response.map(item => [item.CANAL, { CANAL: item.CANAL }])).values()
                );

                console.log(canales);

                canales.forEach(c => {
                    const zonas = _response.filter(x => x.CANAL == c.CANAL);

                    zonas.forEach(z => {
                        $("#tablePronostico tbody").append(
                            `<tr data-key="${z.CANAL}-${z.ZONA}">
                            <td>${z.CANAL}</td>
                            <td>${z.ZONA}</td>
                            <td><input type='number' class="form-control-sm text-center txtPercentSemAnt" readonly></td>
                            <td><input type='number' class="form-control-sm text-center txtSemanaAnterior" readonly></td>
                            <td><input type='number' class="form-control-sm text-center txtMeta" readonly></td>
                            <td><input type='number' class="form-control-sm text-center txtMetaPropuesta blue-cell" value='0'></td>
                            <td><input type='number' class="form-control-sm text-center txtPMetaPropuesta blue-cell" value='0'></td>
                            <td><input type='number' class="form-control-sm text-center txtDespacho" readonly></td>
                            <td><input type='number' class="form-control-sm text-center txtCamion" readonly></td>
                            <td><input type='number' class="form-control-sm text-center txtLogro" readonly></td>
                            <td><input type='number' class="form-control-sm text-center txtProyeccion" readonly></td>
                        </tr>`
                        );
                    });

                    $("#tablePronostico tbody").append(
                        `<tr data-key="${c.CANAL}-TOTALES" style='background-color: deepskyblue;color: #fff;'>
                        <td>${c.CANAL}</td>
                        <td>TOTALES</td>
                        <td><input class="form-control-sm text-center txtPercentSemAnt" readonly></td>
                        <td><input class="form-control-sm text-center txtSemanaAnterior" readonly></td>
                        <td><input class="form-control-sm text-center txtMeta" readonly></td>
                        <td><input class="form-control-sm text-center txtMetaPropuesta" readonly></td>
                        <td><input class="form-control-sm text-center txtPMetaPropuesta" readonly></td>
                        <td><input class="form-control-sm text-center txtDespacho" readonly></td>
                        <td><input class="form-control-sm text-center txtCamion" readonly></td>
                        <td><input class="form-control-sm text-center txtLogro" readonly></td>
                        <td><input class="form-control-sm text-center txtProyeccion" readonly></td>
                    </tr>`
                    );

                    rellenar_despachos_tabla_pronosticos();
                    rellenar_ventas_tabla_pronosticos();
                })
            },
            error(_response) {
                console.log(_response);
            }
        });
    }

    function rellenar_despachos_tabla_pronosticos() {
        $.ajax({
            url: "http://192.168.1.70/portal/modulo_logistica/services/portalArmadoMotos.services.php?token=@WAyEterSOr",
            method: "POST",
            dataType: "JSON",
            data: { request: "resumen_despachos_DMC" },
            success(_response) {
                _response.forEach(x => {
                    // Referencia al tr correspondiente
                    let $tr = $(`#tablePronostico tr[data-key="${x.CANAL}-${x.ZONA}"]`);

                    // Setear valores de despacho y camion
                    $tr.find(".txtDespacho").val(x.DESPACHOS_ABIERTOS);
                    $tr.find(".txtCamion").val(x.CAMIONES_CERRADOS);

                    // Obtener meta calculada (asegúrate que el value es numérico)
                    let meta = parseFloat($tr.find(".txtMeta").val()) || 0;
                    let despachos = parseFloat(x.DESPACHOS_ABIERTOS) || 0;
                    let camiones = parseFloat(x.CAMIONES_CERRADOS) || 0;

                    // Calcular %Logro y %Proyección
                    let porcLogro = (meta > 0) ? (camiones / meta) * 100 : 0;
                    let porcProyeccion = (meta > 0) ? ((despachos + camiones) / meta) * 100 : 0;

                    // Asignar los resultados a los campos
                    $tr.find(".txtLogro").val(porcLogro.toFixed(2));
                    $tr.find(".txtProyeccion").val(porcProyeccion.toFixed(2));
                });
                actualizarTotalesYResumen();
            }
        });
    }

    function rellenar_ventas_tabla_pronosticos() {
        const incrementoInput = document.getElementById('txtIncremento');
        const incremento = parseFloat(incrementoInput?.value || 0);

        const tablaSuperior = document.getElementById("tablePronostico");
        const tablaInferior = document.getElementById("tablaCanalesCD");
        if (!tablaSuperior || !tablaInferior) return;

        const filasInferior = tablaInferior.querySelectorAll("tbody tr");
        const filasSuperior = tablaSuperior.querySelectorAll("tbody tr");

        filasInferior.forEach(fila => {
            const celdas = fila.querySelectorAll("td");
            const canal = celdas[0].innerText.trim();
            const ruta = celdas[1].innerText.trim();
            const percent_sem_anterior = parseFloat(celdas[13]?.innerText.trim()) || 0;
            const semana_anterior = parseInt(celdas[12]?.innerText.trim()) || 0;
            const semanafinal = parseInt(celdas[12]?.innerText.trim()) || 0;

            filasSuperior.forEach(filaSup => {
                const canalSup = filaSup.children[0]?.innerText.trim();
                const rutaSup = filaSup.children[1]?.innerText.trim();

                if (canal === canalSup && ruta === rutaSup) {
                    const inputPercentSemanaAnteior = filaSup.children[2]?.querySelector("input");
                    const inputSemanaAnterior = filaSup.children[3]?.querySelector("input");
                    const inputMeta = filaSup.children[4]?.querySelector("input");
                    const inputDespachado = filaSup.children[5]?.querySelector("input");

                    if (inputPercentSemanaAnteior) inputPercentSemanaAnteior.value = percent_sem_anterior;
                    if (inputSemanaAnterior) inputSemanaAnterior.value = Math.round(semana_anterior);
                    if (inputMeta) inputMeta.value = Math.round(semana_anterior * (1 + incremento / 100));
                }
            });
        });
    }

    function actualizarTotalesYResumen() {
        const porcIncremento = parseFloat($("#<%=txtIncremento.ClientID%>").val()) || 0;
        const canales = ["CD", "CI"];

        // Variables para resumen general (tabla roja)
        let totSemanaAnterior = 0, totalMetaGeneral = 0, totalMetaAsignadaGeneral = 0, totalDespachoGeneral = 0, totalCamionGeneral = 0, totalPMetaAsignada = 0 ;

        canales.forEach(canal => {
            // Totales canal para ambas tablas
            let totalPercentSemanaAnterior = 0, totalSemanaAnterior = 0, totalMeta = 0, totalMetaAsignada = 0, totalDespacho = 0, totalCamion = 0;

            $(`#tablePronostico tbody tr[data-key^="${canal}-"]`).each(function () {
                const key = $(this).attr('data-key');
                if (key && key.endsWith("TOTALES")) return;

                totalPercentSemanaAnterior += parseFloat($(this).find('.txtPercentSemAnt').val()) || 0;
                totalSemanaAnterior += parseInt($(this).find('.txtSemanaAnterior').val()) || 0;
                totalMeta += parseInt($(this).find('.txtMeta').val()) || 0;
                totalMetaAsignada += parseInt($(this).find('.txtMetaPropuesta').val()) || 0;
                totalPMetaAsignada += parseInt($(this).find('.txtPMetaPropuesta').val()) || 0;
                totalDespacho += parseInt($(this).find('.txtDespacho').val()) || 0;
                totalCamion += parseInt($(this).find('.txtCamion').val()) || 0;
            });

            // --- Actualiza fila TOTALES (azul) de la tabla grande ---
            const $trTotal = $(`#tablePronostico tbody tr[data-key="${canal}-TOTALES"]`);
            $trTotal.find('.txtPercentSemAnt').val(totalPercentSemanaAnterior.toFixed(2));
            $trTotal.find('.txtSemanaAnterior').val(totalSemanaAnterior);
            $trTotal.find('.txtMeta').val(totalMeta);
            $trTotal.find('.txtMetaPropuesta').val(totalMetaAsignada);
            $trTotal.find('.txtPMetaPropuesta').val(totalPMetaAsignada);
            $trTotal.find('.txtDespacho').val(totalDespacho);
            $trTotal.find('.txtCamion').val(totalCamion);
            $trTotal.find('.txtLogro').val(totalMeta > 0 ? (totalCamion / totalMeta * 100).toFixed(2) : "0.00");
            $trTotal.find('.txtProyeccion').val(totalMeta > 0 ? ((totalDespacho + totalCamion) / totalMeta * 100).toFixed(2) : "0.00");

            // --- Suma para tabla resumen ---
            totSemanaAnterior += totalSemanaAnterior;
            totalMetaGeneral += totalMeta;
            totalMetaAsignadaGeneral += totalMetaAsignada;
            totalDespachoGeneral += totalDespacho;
            totalCamionGeneral += totalCamion;

            // --- Actualiza fila resumen (roja) de canal ---
            //const metaCalculada = totalMeta * (1 + porcIncremento / 100);
            const metaCalculada = totalMeta;
            const porcMA = (metaCalculada > 0) ? (totalMetaAsignada / metaCalculada) * 100 : 0;
            const porcLogro = (metaCalculada > 0) ? (totalCamion / metaCalculada) * 100 : 0;
            const porcProyeccion = (metaCalculada > 0) ? ((totalDespacho + totalCamion) / metaCalculada) * 100 : 0;
            let $filaResumen = $("#resumenGralCanal tr").filter(function () {
                return $(this).find("td:first").text().trim() === canal;
            });
            if ($filaResumen.length) {
                $filaResumen.find("td").eq(1).text(totalSemanaAnterior.toFixed(0));
                $filaResumen.find("td").eq(3).text(metaCalculada.toFixed(0));
                //$filaResumen.find("td").eq(4).text(totalMetaAsignada.toFixed(0));
                //$filaResumen.find("td").eq(5).text(porcMA.toFixed(2) + "%");
                $filaResumen.find("td").eq(6).text(totalDespacho);
                $filaResumen.find("td").eq(7).text(totalCamion);
                $filaResumen.find("td").eq(8).text(porcLogro.toFixed(2));
                $filaResumen.find("td").eq(9).text(porcProyeccion.toFixed(2));
            }
        });

        // --- Fila Totales en tabla de resumen (roja) ---
        const totalGeneralSemAnterior = totSemanaAnterior;
        const metaCalculadaGeneral = totalMetaGeneral;
        const porcMAGeneral = (metaCalculadaGeneral > 0) ? (totalMetaAsignadaGeneral / metaCalculadaGeneral) * 100 : 0;
        const porcLogroGeneral = (metaCalculadaGeneral > 0) ? (totalCamionGeneral / metaCalculadaGeneral) * 100 : 0;
        const porcProyeccionGeneral = (metaCalculadaGeneral > 0) ? ((totalDespachoGeneral + totalCamionGeneral) / metaCalculadaGeneral) * 100 : 0;
        let $filaResumenTotales = $("#resumenGralCanal tr").filter(function () {
            return $(this).find("td:first").text().trim().toUpperCase() === "TOTALES";
        });
        if ($filaResumenTotales.length) {
            $filaResumenTotales.find("td").eq(1).text(totalGeneralSemAnterior.toFixed(0));
            $filaResumenTotales.find("td").eq(3).text(metaCalculadaGeneral.toFixed(0));
            $filaResumenTotales.find("td").eq(6).text(totalDespachoGeneral);
            $filaResumenTotales.find("td").eq(7).text(totalCamionGeneral);
            $filaResumenTotales.find("td").eq(8).text(porcLogroGeneral.toFixed(2));
            $filaResumenTotales.find("td").eq(9).text(porcProyeccionGeneral.toFixed(2));
        }
    }


</script--%>
<script>
    function autoRefresh() {
        Swal.fire({
            title: 'Refrescando datos',
            text: 'Por favor espere...',
            allowOutsideClick: false,
            allowEscapeKey: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });

        setTimeout(() => {
            window.location.reload();
        }, 3000); // Espera 2 segundos antes de recargar para que se vea el mensaje
    }

    setInterval(autoRefresh, 900000); // 5 minutos
</script>
