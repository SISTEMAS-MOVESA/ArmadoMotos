<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosDespachosAbiertosConfirmacion.aspx.vb"
    Inherits="TrasladosDespachosAbiertosConfirmacion" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

        <!DOCTYPE html>

        <html xmlns="http://www.w3.org/1999/xhtml">

        <head runat="server">
            <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
            <title>Seleccion de Series al Camion</title>
            <link rel="stylesheet"
                href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback" />
            <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.3.0/css/all.min.css" />
            <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css" />
            <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css"
                rel="stylesheet" />
            <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"></script>
            <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css" />
            <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>
            <script src="https://d3js.org/d3.v6.min.js"></script>
            <script src='https://cdn.jsdelivr.net/npm/sweetalert2@11'></script>


        </head>

        <body>

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
                <div class="container" style="max-width: 700px; padding: 20px;">
                    <h1 class="text-center mb-4">Datos Camion</h1>
                    <!-- Fecha -->
                    <div class="row mb-3 justify-content-center align-items-center">
                        <div class="col-3 text-end">
                            <label for="txtFechaCamion">Fecha Salida Camion</label>
                        </div>
                        <div class="col-3">
                            <asp:TextBox ID="txtFechaCamion" runat="server" CssClass="form-control" TextMode="DateTime">
                            </asp:TextBox>
                        </div>
                        <div class="col-3">
                            Seleccione Placa
                        </div>
                        <div class="col-3">
                            <asp:DropDownList ID="drpPlacas" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row mb-3 justify-content-center align-items-center">
                        <div class="col-3">
                            Espacios Disponibles
                        </div>
                        <div class="col-1">
                            <div id="espaciosCamion" class="mt-2 text-info"
                                style="font-size: xx-large; font-weight: bolder;"></div>
                        </div>
                        <div class="col-3">
                            Espacios Restantes
                        </div>
                        <div class="col-1">
                            <div id="espaciosRestantes" class="mt-2 text-danger"
                                style="font-size: xx-large; font-weight: bolder;"></div>

                        </div>
                    </div>

                    <!-- Placa y Motorista -->
                    <%-- <div class="row mb-3 justify-content-center align-items-center">
                        <div class="col-6 text-end">
                            <button type="button" onclick="obtenerDetalleCamion(10088)">Ver Detalle</button>
                        </div>
                </div>--%>

                </div>

                <div class="ui active dimmer" style="position: fixed !important;" id="screen_loader">
                    <div class="ui big text loader">Loading</div>
                </div>

                <!-- ASIGNAR SERIES A CAMION -->
                <div class="ui fluid container" id="frm-series-despachos">
                    <div class="ui dividing header">Series en Despachos disponibles</div>
                    <div class="ui grid three column">
                        <div class="column seven wide">
                            <h5 class="ui centered header" id="lbl-disponibles">Series Disponibles</h5>
                            <table class="ui tiny table" id="tbl-series-disponibles">
                                <thead>
                                    <tr>
                                        <th>DESPACHO</th>
                                        <th>RUTA</th>
                                        <th>TIPO</th>
                                        <th>ALMACEN</th>
                                        <th>ARTICULO</th>
                                        <th>SERIE</th>
                                        <th>ESPACIOS</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>
                        <div class="column two wide">
                            <div class="ui vertical fluid buttons mt-5">
                                <div class="ui icon button btn-seleccionar" data-selected="Y"><i
                                        class="right angle large icon"></i></div>
                                <div class="ui icon button btn-seleccionar-todo" data-selected="Y"><i
                                        class=" double right angle large icon"></i></div>
                                <div class="ui icon button btn-seleccionar" data-selected="N"><i
                                        class="left angle large icon"></i></div>
                                <div class="ui icon button btn-seleccionar-todo" data-selected="N"><i
                                        class="double left angle large icon"></i></div>
                            </div>
                        </div>
                        <div class="column seven wide">
                            <h5 class="ui centered header" id="lbl-seleccionadas">Series Seleccionadas</h5>
                            <table class="ui tiny table" id="tbl-series-seleccionadas">
                                <thead>
                                    <tr>
                                        <th>DESPACHO</th>
                                        <th>RUTA</th>
                                        <th>TIPO</th>
                                        <th>ALMACEN</th>
                                        <th>ARTICULO</th>
                                        <th>SERIE</th>
                                        <th>ESPACIOS</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <br>
                <!-- ASIGNAR SERIES DE RETIRO -->
                <div class="ui fluid container" id="frm-series-retiros">
                    <div class="ui dividing header">Retiros Disponibles</div>
                    <div class="ui grid three column">
                        <div class="column seven wide">
                            <h5 class="ui centered header" id="lbl-retiros-disponibles">Retiros Disponibles</h5>
                            <div class="ui fluid input icon">
                                <input class="txt-buscador" type="text" placeholder="Buscar..."
                                    data-table="tbl-retiros-disponibles"><i class="search icon"></i>
                            </div>
                            <div class="ui long scrolling container px-0 mt-2">
                                <table class="ui tiny table" id="tbl-retiros-disponibles">
                                    <thead>
                                        <tr>
                                            <th># SOL. RETIRO</th>
                                            <th>RUTA</th>
                                            <th>CAUSA</th>
                                            <th>ALMACEN</th>
                                            <th>ARTICULO</th>
                                            <th>SERIE</th>
                                            <th>ESPACIOS</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                        </div>
                        <div class="column two wide">
                            <div class="ui vertical fluid buttons mt-5">
                                <div class="ui icon button btn-seleccionar" data-selected="Y"><i
                                        class="right angle large icon"></i></div>
                                <div class="ui icon button btn-seleccionar-todo" data-selected="Y"><i
                                        class=" double right angle large icon"></i></div>
                                <div class="ui icon button btn-seleccionar" data-selected="N"><i
                                        class="left angle large icon"></i></div>
                                <div class="ui icon button btn-seleccionar-todo" data-selected="N"><i
                                        class="double left angle large icon"></i></div>
                            </div>
                        </div>
                        <div class="column seven wide">
                            <h5 class="ui centered header" id="lbl-retiros-seleccionadas">Retiros Seleccionados</h5>
                            <div class="ui fluid input icon">
                                <input class="txt-buscador" type="text" placeholder="Buscar..."
                                    data-table="tbl-retiros-seleccionadas"><i class="search icon"></i>
                            </div>
                            <div class="ui long scrolling container px-0 mt-2"></div>
                            <table class="ui tiny table" id="tbl-retiros-seleccionadas">
                                <thead>
                                    <tr>
                                        <th># SOL. RETIRO</th>
                                        <th>RUTA</th>
                                        <th>CAUSA</th>
                                        <th>ALMACEN</th>
                                        <th>ARTICULO</th>
                                        <th>SERIE</th>
                                        <th>ESPACIOS</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>
                    </div>
                </div>
                </div>


                <div class="container-fluid my-3"
                    style="display: flex; flex-direction: row; align-content: center; align-items: center; justify-content: center; gap:20px;">
                    <div>
                        <asp:Button ID="btnCancelar" runat="server" Text="Regresar" CssClass="btn btn-warning btn-lg"
                            OnClientClick="window.location.href='TrasladosDespachosAbiertos.aspx'; return false;" />
                    </div>
                    <div>
                        <button class="btn btn-primary btn-lg" type="button" id="btn-crear-camion">Crear Camion</button>
                    </div>
                </div>

                <div class="container-fkuid" style="display: flex; flex-direction: column; align-items: center">
                    <footer class="main-footer">
                        <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a
                                href="#">Movesa</a>.</strong> All rights reserved.
                    </footer>
                </div>
                <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
                <link rel="stylesheet"
                    href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.css" />
                <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
                <script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.js"></script>
                <script src="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.js"
                    integrity="sha512-gnoBksrDbaMnlE0rhhkcx3iwzvgBGz6mOEj4/Y5ZY09n55dYddx6+WYc72A55qEesV8VX2iMomteIwobeGK1BQ=="
                    crossorigin="anonymous" referrerpolicy="no-referrer"></script>
                <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.css"
                    integrity="sha512-3quBdRGJyLy79hzhDDcBzANW+mVqPctrGCfIPosHQtMKb3rKsCxfyslzwlz2wj1dT8A7UX+sEvDjaUv+WExQrA=="
                    crossorigin="anonymous" referrerpolicy="no-referrer" />
            </form>
        </body>

        </html>
        <!-- jQuery -->
        <script src="plugins/jquery/jquery.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.11.0/umd/popper.min.js"
            integrity="sha384-b/U6ypiBEHpOf/4+1nzFpr53nxSS+GLCkfwBdFNTxtclqqenISfwAzpKaMNFNmj4"
            crossorigin="anonymous"></script>


        <!-- DataTables  & Plugins -->
        <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
        <script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
        <script type="text/javascript"
            src="https://cdn.datatables.net/buttons/1.7.1/js/dataTables.buttons.min.js"></script>
        <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.html5.min.js"></script>
        <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.print.min.js"></script>
        <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.colVis.min.js"></script>
        <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
        <script type="text/javascript"
            src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
        <script type="text/javascript"
            src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
        <link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />
        <link href="https://cdn.datatables.net/buttons/1.7.1/css/buttons.dataTables.min.css" rel="stylesheet"
            type="text/css" />

        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.css" />
        <script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.js"
            integrity="sha512-gnoBksrDbaMnlE0rhhkcx3iwzvgBGz6mOEj4/Y5ZY09n55dYddx6+WYc72A55qEesV8VX2iMomteIwobeGK1BQ=="
            crossorigin="anonymous" referrerpolicy="no-referrer"></script>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.css"
            integrity="sha512-3quBdRGJyLy79hzhDDcBzANW+mVqPctrGCfIPosHQtMKb3rKsCxfyslzwlz2wj1dT8A7UX+sEvDjaUv+WExQrA=="
            crossorigin="anonymous" referrerpolicy="no-referrer" />


        <script>

            $("#drpPlacas").dropdown({ "fullTextSearch": true });
            $("#drpMotorista").dropdown({ "fullTextSearch": true });

            $(`.txt-buscador[data-table]`).on('keyup', function () {
                var value = $(this).val().toLowerCase();
                const table = $(this).attr("data-table");

                $(`#${table} tbody tr`).filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
                });
            });
        </script>

        <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

        <script>
            function getURLParameter(name) {
                var url = window.location.search.substring(1);
                var params = url.split('&');

                for (var i = 0; i < params.length; i++) {
                    var param = params[i].split('=');
                    if (param[0] === name) {
                        return param[1] === undefined ? true : decodeURIComponent(param[1]);
                    }
                }
            }

            async function confirmar(title, text, confirmText = "Sí, continuar", cancelText = "Cancelar") {
                const resultado = await Swal.fire({
                    title: title,
                    html: text,
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: confirmText,
                    cancelButtonText: cancelText
                });

                return resultado.isConfirmed;
            }

            async function confirmacion_con_razon(title, text, obligatorio = true, confirmText = "Sí, continuar", cancelText = "Cancelar") {
                const { value: comments, isConfirmed } = await Swal.fire({
                    title: title,
                    html: text,
                    input: 'text',
                    inputPlaceholder: 'Escribe tu comentario aqui...',
                    showCancelButton: true,
                    confirmButtonText: confirmText,
                    cancelButtonText: cancelText,
                    inputValidator: (value) => {
                        if (!value && obligatorio) {
                            return 'Debes escribir algo';
                        }
                    }
                });

                return { isConfirmed, comments };
            }
        </script>
        <!--SERIES DESPACHOS -->
        <script>
            //--------------------------

            const series = {};
            let seleccionadas = [];

            $("#frm-series-despachos .btn-seleccionar[data-selected]").on("click", function () {
                const status = $(this).attr("data-selected");

                $("#frm-series-despachos").find("#tbl-series-disponibles tbody, #tbl-series-seleccionadas tbody").find("tr.active").each(function () {
                    const serie = $(this).attr("data-serie");
                    actualizar_serie(serie, status);
                });

                rellenar_series();
            });

            $("#frm-series-despachos .btn-seleccionar-todo[data-selected]").on("click", function () {
                const status = $(this).attr("data-selected");

                for (const key in series) {
                    if (Object.prototype.hasOwnProperty.call(series, key)) {
                        actualizar_serie(series[key].SERIAL_NUMBER, status)
                    }
                }
                rellenar_series();
            });

            async function obtener_series() {
                let despachos = getURLParameter("datos");
                despachos = JSON.parse(atob(despachos));
                console.log(despachos);

                for (const item of despachos) {
                    const despachoid = item.despachoid;
                    if (despachoid) {
                        try {
                            const response = await $.ajax({
                                url: `GetDetalleCamion.ashx?despachoid=${encodeURIComponent(despachoid)}`,
                                method: "GET",
                                dataType: "JSON"
                            });

                            console.log(response);
                            response.forEach(element => {
                                series[element.SERIAL_NUMBER] = element;
                            });

                        } catch (error) {
                            console.error("Error al obtener series:", error);
                        }
                    }
                }
            }

            function rellenar_series() {
                $("#frm-series-despachos #tbl-series-disponibles tbody").empty();
                $("#frm-series-despachos #tbl-series-seleccionadas tbody").empty();

                let items_seleccionados = seleccionadas.length;

                let items_disponibles = 0;
                let totalSlots = 0;


                seleccionadas.forEach(serie => {
                    const element = series[serie];
                    totalSlots += parseInt(element.SLOT);
                    console.log(serie, element);
                    const tr = $(
                        `<tr data-serie="${element.SERIAL_NUMBER}" class="cursor pointer">
                     <td>${element.DOCNUM}</td>
                     <td>${element.ROAD}</td>
                     <td class="${element.LINETYPE == "PEDIDO" ? "blue" : ""}">${element.LINETYPE}</td>
                     <td><b>${element.TO_WHSCODE}</b><br>${element.TO_WHSNAME}</td>
                     <td><b>${element.ITEMCODE}</b><br>${element.ITEMNAME}</td>
                     <td>${element.SERIAL_NUMBER}</td>
                     <td>${element.SLOT}</td>
                 </tr>`
                    );

                    $("#frm-series-despachos #tbl-series-seleccionadas tbody").append(tr);
                });

                for (const key in series) {
                    if (Object.prototype.hasOwnProperty.call(series, key)) {
                        const element = series[key];
                        if (element.SELECTED == "N") {
                            items_disponibles++;
                            const tr = $(
                                `<tr data-serie="${element.SERIAL_NUMBER}" class="cursor pointer">
                             <td>${element.DOCNUM}</td>
                             <td>${element.ROAD}</td>
                             <td class="${element.LINETYPE == "PEDIDO" ? "blue" : ""}">${element.LINETYPE}</td>
                             <td><b>${element.TO_WHSCODE}</b><br>${element.TO_WHSNAME}</td>
                             <td><b>${element.ITEMCODE}</b><br>${element.ITEMNAME}</td>
                             <td>${element.SERIAL_NUMBER}</td>
                             <td>${element.SLOT}</td>
                         </tr>`
                            );
                            $("#frm-series-despachos #tbl-series-disponibles tbody").append(tr);
                        }
                    }
                }

                $("#espaciosRestantes").html(`${totalSlots}`);
                $("#frm-series-despachos #lbl-disponibles").html(`Series Disponibles (${items_disponibles})`);
                $("#frm-series-despachos #lbl-seleccionadas").html(`Series Seleccionadas (${items_seleccionados})`);


                let espaciosCamion = parseInt($("#espaciosCamion").text()) || 0;
                $("#espaciosRestantes").text(espaciosCamion - totalSlots);

                $("#frm-series-despachos").find("#tbl-series-disponibles tbody, #tbl-series-seleccionadas tbody").find("tr").on("click", function () {
                    $("#frm-series-despachos").find("#tbl-series-disponibles tbody, #tbl-series-seleccionadas tbody").find("tr").removeClass("active");
                    $(this).addClass("active");
                });

                $("#frm-series-despachos #tbl-series-disponibles tbody tr").on("dblclick", function () {
                    const serie = $(this).attr("data-serie");
                    actualizar_serie(serie, "Y");
                    rellenar_series();
                });

                $("#frm-series-despachos #tbl-series-seleccionadas tbody tr").on("dblclick", function () {
                    const serie = $(this).attr("data-serie");
                    actualizar_serie(serie, "N");
                    rellenar_series();
                });
            }

            function actualizar_serie(serie, status) {
                console.log(serie, status);
                series[serie].SELECTED = status;

                if (status == "Y" && seleccionadas.filter(s => s == serie).length == 0) {
                    seleccionadas.push(serie);
                } else if (status == "N") {
                    seleccionadas = seleccionadas.filter(s => s !== serie);
                }
            }
        </script>

        <!--SERIES RETIROS -->
        <script>
            //--------------------------

            const series_retiros = {};
            let seleccionadas_retiros = [];

            $("#frm-series-retiros .btn-seleccionar[data-selected]").on("click", function () {
                const status = $(this).attr("data-selected");

                $("#frm-series-retiros").find("#tbl-retiros-disponibles tbody, #tbl-retiros-seleccionadas tbody").find("tr.active").each(function () {
                    const serie = $(this).attr("data-serie");
                    actualizar_serie_retiro(serie, status);
                });

                rellenar_series_retiros();
            });

            $("#frm-series-retiros .btn-seleccionar-todo[data-selected]").on("click", function () {
                const status = $(this).attr("data-selected");

                for (const key in series_retiros) {
                    if (Object.prototype.hasOwnProperty.call(series_retiros, key)) {
                        actualizar_serie_retiro(series_retiros[key].SERIAL_NUMBER, status)
                    }
                }
                rellenar_series_retiros();
            });

            async function obtener_series_retiros() {
                let items = [];
                $("#screen_loader").show();
                await $.ajax({
                    url: "http://192.168.1.70/portal/modulo_logistica/services/portalArmadoMotos.services.php?token=@WAyEterSOr",
                    method: "POST",
                    dataType: "JSON",
                    data: { request: "obtener_solicitud_retiros_abiertas" },
                    success(_response) {
                        $("#screen_loader").hide();
                        items = _response;
                    },
                    error(_response) {
                        $("#screen_loader").hide();
                        console.log(_response);
                        items = [];
                    }
                });

                items.forEach(element => {
                    element.SELECTED = "N";
                    series_retiros[element.SERIAL_NUMBER] = element;
                });
            }

            function rellenar_series_retiros() {
                $("#frm-series-retiros #tbl-retiros-disponibles tbody").empty();
                $("#frm-series-retiros #tbl-retiros-seleccionadas tbody").empty();

                let items_seleccionados = seleccionadas_retiros.length;

                let items_disponibles = 0;
                let totalSlots = 0;

                const rutas = Array.from(
                    new Map(Object.values(series).map(item => [item.ROAD, item.ROAD])).values()
                );

                seleccionadas_retiros.forEach(serie => {
                    const element = series_retiros[serie];
                    totalSlots += parseInt(element.SLOT);
                    console.log(serie, element);
                    const tr = $(
                        `<tr data-serie="${element.SERIAL_NUMBER}" class="cursor pointer">
                     <td>${element.DOCNUM}</td>
                     <td class="${rutas.includes(element.RUTA) ? "blue" : ""}">${element.RUTA}</td>
                     <td>${element.DOCTYPE}</td>
                     <td><b>${element.WHSCODE}</b><br>${element.WHSNAME}</td>
                     <td><b>${element.ITEMCODE}</b><br>${element.ITEMNAME}</td>
                     <td>${element.SERIAL_NUMBER}</td>
                     <td>${element.SLOTS}</td>
                 </tr>`
                    );

                    $("#frm-series-retiros #tbl-retiros-seleccionadas tbody").append(tr);
                });

                for (const key in series_retiros) {
                    if (Object.prototype.hasOwnProperty.call(series_retiros, key)) {
                        const element = series_retiros[key];
                        if (element.SELECTED == "N") {
                            items_disponibles++;
                            const tr = $(
                                `<tr data-serie="${element.SERIAL_NUMBER}" class="cursor pointer">
                            <td>${element.DOCNUM}</td>
                            <td class="${rutas.includes(element.RUTA) ? "blue" : ""}">${element.RUTA}</td>
                            <td>${element.DOCTYPE}</td>
                            <td><b>${element.WHSCODE}</b><br>${element.WHSNAME}</td>
                            <td><b>${element.ITEMCODE}</b><br>${element.ITEMNAME}</td>
                            <td>${element.SERIAL_NUMBER}</td>
                            <td>${element.SLOTS}</td>
                        </tr>`
                            );
                            $("#frm-series-retiros #tbl-retiros-disponibles tbody").append(tr);
                        }
                    }
                }

                $("#frm-series-retiros #lbl-retiros-disponibles").html(`Series Disponibles (${items_disponibles})`);
                $("#frm-series-retiros #lbl-retiros-seleccionadas").html(`Series Seleccionadas (${items_seleccionados})`);

                $("#frm-series-retiros").find("#tbl-retiros-disponibles tbody, #tbl-retiros-seleccionadas tbody").find("tr").on("click", function () {
                    $("#frm-series-retiros").find("#tbl-retiros-disponibles tbody, #tbl-retiros-seleccionadas tbody").find("tr").removeClass("active");
                    $(this).addClass("active");
                });

                $("#frm-series-retiros #tbl-retiros-disponibles tbody tr").on("dblclick", function () {
                    const serie = $(this).attr("data-serie");
                    actualizar_serie_retiro(serie, "Y");
                    rellenar_series_retiros();
                });

                $("#frm-series-retiros #tbl-retiros-seleccionadas tbody tr").on("dblclick", function () {
                    const serie = $(this).attr("data-serie");
                    actualizar_serie_retiro(serie, "N");
                    rellenar_series_retiros();
                });
            }

            function actualizar_serie_retiro(serie, status) {
                console.log(serie, status);
                series_retiros[serie].SELECTED = status;

                if (status == "Y" && seleccionadas_retiros.filter(s => s == serie).length == 0) {
                    seleccionadas_retiros.push(serie);
                } else if (status == "N") {
                    seleccionadas_retiros = seleccionadas_retiros.filter(s => s !== serie);
                }
            }
        </script>


        <script>

            (async function () {
                await obtener_series();
                await obtener_series_retiros();

                rellenar_series();
                rellenar_series_retiros();

            })()

            $("#btn-crear-camion").on("click", async function () {
                await crearCamion();
            });

            async function crearCamion() {
                const fecha_salida = $("#txtFechaCamion").val();
                const despachos = [];
                const retiros = [];

                seleccionadas.forEach(s => {
                    despachos.push(series[s].LINENUM);
                });

                seleccionadas_retiros.forEach(s => {
                    retiros.push(series_retiros[s].LINENUM);
                });

                if (fecha_salida == 0) {
                    Swal.fire({ icon: 'warning', title: 'Selecciona una fecha de salida para el camion' });
                    return;
                }

                if (seleccionadas.length == 0) {
                    Swal.fire({ icon: 'warning', title: 'No hay series seleccionadas', text: 'Por favor, seleccione al menos una serie antes de continuar.' });
                    return;
                }

                let comments = "";
                if (seleccionadas_retiros.length == 0) {
                    const confirmado = await confirmacion_con_razon(
                        "No has seleccionado ningun retiro para este camion!",
                        `Explica una breve razon de por no se asignan retiros a este camion`,
                        true, "Continuar", "Cancelar"
                    );
                    console.log(confirmado);
                    if (!confirmado.isConfirmed) return;
                    comments = confirmado.comments;
                } else {
                    const confirmado = await confirmacion_con_razon(
                        "Estas seguro de continuar?\Una vez creado no podras deshacer esta operacion.",
                        `Se creara un documento [CAMION] con: <br><br>
                (${despachos.length}) series de despacho<br>
                (${retiros.length}) series de retiro`,
                        false, "Continuar", "Cancelar"
                    );

                    console.log(confirmado);
                    if (!confirmado.isConfirmed) return;
                    comments = confirmado.comments;
                }


                const data = { fecha_salida, despachos, retiros, comments };
                console.log(data);

                $("#screen_loader").show();
                $.ajax({
                    url: "http://192.168.1.70/portal/modulo_logistica/services/portalArmadoMotos.services.php?token=@WAyEterSOr",
                    method: "POST",
                    dataType: "JSON",
                    data: { request: "crear_camion", data },
                    success(_response) {
                        $("#screen_loader").hide();
                        console.log(_response);
                        if (_response.Success) {
                            Swal.fire({
                                icon: 'success', title: `Camion Creado con exito!\n# Camion: ${_response.Data.CAMION_ID}`
                            }).then(() => {
                                location.assign('TrasladosDespachosAbiertos.aspx');
                            });
                        } else {
                            Swal.fire({
                                icon: 'error', title: 'Error al crear el camión', text: _response.Message || 'Ha ocurrido un error inesperado.'
                            });
                        }
                    },
                    error(_response) {
                        $("#screen_loader").hide();
                        console.error(_response);
                        Swal.fire({
                            icon: 'error', title: 'Server Error!', html: _response.responseText || 'Error inesperado al comunicarse con el servidor.'
                        });
                    }
                })

                //fetch("UpdateOrdenamiento.ashx", {
                //    method: "POST",
                //    headers: {
                //        "Content-Type": "application/json"
                //    },
                //    body: JSON.stringify(data)
                //})
                //.then(res => res.json())
                //.then(res => {
                //    console.log(res);
                //    if (res.success) {
                //        Swal.fire({
                //            icon: 'success',
                //            title: res.message
                //        }).then(() => {
                //            location.assign('TrasladosDespachosAbiertos.aspx');
                //        });
                //    } else {
                //        Swal.fire({
                //            icon: 'error',
                //            title: 'Error al crear el camión',
                //            text: res.message || 'Ha ocurrido un error inesperado.'
                //        });
                //    }
                //})
                //.catch(err => {
                //    console.error(err);
                //    Swal.fire({
                //        icon: 'error',
                //        title: 'Error del servidor',
                //        text: err.message || 'Error inesperado al comunicarse con el servidor.'
                //    });
                //});
            }
        </script>

        <script>
            document.addEventListener("DOMContentLoaded", async () => {
                const ddl = document.getElementById("drpPlacas");

                // Al cargar la página
                if (ddl.value) {
                    const placa = ddl.options[ddl.selectedIndex].text;
                    await obtenerEspaciosCamion(placa);
                }

                // Cuando cambia la selección
                ddl.addEventListener("change", async function () {
                    const placa = this.options[this.selectedIndex].text;
                    if (placa) {
                        await obtenerEspaciosCamion(placa);
                    }
                });
            });

            // Consulta al handler
            async function obtenerEspaciosCamion(placa) {
                try {
                    const response = await fetch(`getEspaciosCamion.ashx?placa=${encodeURIComponent(placa)}`);
                    const data = await response.json();

                    if (data.error) {
                        console.error("Error:", data.error);
                        return;
                    }

                    console.log("Datos recibidos:", data);

                    const espacios = data.length > 0 ? data[0].ESPACIOS : "Sin datos";
                    document.getElementById("espaciosCamion").textContent = espacios;
                } catch (error) {
                    console.error("Error al consultar espacios:", error);
                }
            }
        </script>