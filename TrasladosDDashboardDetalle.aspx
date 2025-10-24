<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosDDashboardDetalle.aspx.vb" Inherits="TrasladosDDashboardDetalle" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Planificiones Abiertas</title>
    <link rel="stylesheet" href="plugins/jqvmap/jqvmap.min.css" />
    <link rel="stylesheet" href="dist/css/adminlte.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>
    <script src="https://d3js.org/d3.v6.min.js"></script>
    <script src='https://cdn.jsdelivr.net/npm/sweetalert2@11'></script>
    <!-- jQuery primero -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <!-- Luego Bootstrap JS (usa la versión que coincida con tu Bootstrap CSS) -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css"/>


    <style>
        thead input {
            width: 100% !important;
            padding: 3px !important;
            box-sizing: border-box !important;
        }

        .sticky-header th {
            position: sticky !important;
            top: 0 !important;
            z-index: 2 !important;
            background-color: #343a40 !important; /* Color de fondo del encabezado */
            color: white !important; /* Color del texto */
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
            height: 4rem; /* Ajusta según necesidad */
            width: 100%;
        }

        td {
            align-content: center;
        }

        .document-checkbox input[type="checkbox"] {
            transform: scale(1.5); /* Aumenta el tamaño (1.5x) */
            width: 20px; /* Opcional: Ajustar tamaño exacto */
            height: 20px; /* Opcional: Ajustar tamaño exacto */
            cursor: pointer; /* Cambia el cursor al pasar sobre el checkbox */
        }

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

        .ui[class*="right ribbon"].label {
            left: calc(100% - 1rem - 7em) !important;
            padding-left: 1.2em;
            padding-right: calc(1rem + 1.2em);
        }

        /*     .ui.ui.raised.segment, .ui.ui.raised.segments {
            width: 150px;
        }*/


        .heatmap-cell1, .heatmap-cell2 {
            font-weight: bold;
            font-size: large;
            color: white;
            text-shadow: 1px 1px black;
            text-align: center;
            align-content: center;
        }

        :fullscreen {
            background: white; /* O el color que prefieras */
        }

        :-webkit-full-screen {
            background: white;
        }

        :-moz-full-screen {
            background: white;
        }

        :-ms-fullscreen {
            background: white;
        }

        .container-divs {
            display: flex;
            align-content: space-around;
            gap: 20px;
            font-size: larger;
            font-weight: 400;
        }

        .modalPanel {
            background-color: lightgray;
            padding: 25px;
            border: 2px double solid black;
            border-radius: 8px;
            max-width: 50%;
            max-height: 90vh;
            overflow-y: auto;
            margin: auto;
            position: fixed;
            top: 5%;
            left: 0;
            right: 0;
            z-index: 1001;
            left: 0px !important
        }

        #gridCapacidadPiso td:nth-child(6) {
            background-color: skyblue;
            color: black;
            font-weight: bold; /* opcional */
        }
    </style>
</head>
<body>

    <form id="form1" runat="server">
        <!-- Navigation -->
        <div class="ui inverted menu">
            <div class="ui container">
                <a class="header item" href="MainDashBoard.aspx">
                    <img src="Imagenes/mnegra.png" width="50" height="40" alt="" />
                </a>
                <div class="right menu">
                    <a class="item" href="TrasladosDashboard.aspx">Inicio</a>
                    <div class="ui simple dropdown item">
                        Pre Solicitudes
                    <i class="dropdown icon"></i>
                        <div class="menu">
                            <a class="item" href="TrasladosCargaMacro.aspx">Carga Macro</a>
                            <a class="item" href="TrasladosConfirmarPreSucursal.aspx">Confirmacion Sucursal</a>
                            <a class="item" href="TrasladosPreSolicitud.aspx">Pre Solicitud</a>
                            <a class="item" href="TrasladosPreSolicitudSucursal.aspx">Pre Solicitud Sucursal</a>
                            <div class="divider"></div>
                            <a class="item" href="TrasladosPresolicitudesAbiertas.aspx">Flujo Pre-Solicitudes</a>
                            <a class="item" href="TrasladosSolicitudesAbiertas.aspx">Flujo Solicitudes</a>
                            <a class="item" href="TrasladosFlujoProduccion.aspx">Flujo Produccion</a>
                            <div class="divider"></div>
                            <a class="item" href="TrasladosCuadroBasico.aspx">Cuadro Basico</a>
                            <a class="item" href="TrasladosCuadroBasicoLogico.aspx">Cuadro Basico Logico</a>
                            <div class="divider"></div>
                            <a class="item" href="TrabajosAdicionalesDashboard.aspx">Trabajos Adicionales</a>
                            <div class="divider"></div>
                            <a class="item" href="Grafico.aspx" target="_blank">KPI Inventarios</a>
                            <div class="divider"></div>
                            <a class="item" href="TrasladosDashboardPlanner.aspx">Planificador</a>
                        </div>
                    </div>
                    <a class="item" href="TrasladosSolicitud.aspx">Solicitudes</a>
                    <a class="item" href="TrasladosForklift.aspx">Armado de Moto</a>
                    <a class="item" href="TrasladosPreparar.aspx">Carga Camion</a>
                    <a class="item" href="TrasladosCrearDocumentos.aspx">Generar Traslados</a>
                    <a class="item" href="TrasladosCrearDocumentos.aspx">Gestion de Transferencias</a>
                    <a class="item" href="Default.aspx">Cerrar Sesion</a>
                </div>
            </div>
        </div>
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
        <div class="ui grid mx-1">
            <div class="nine wide column" id="columna1">
                <div class="container-divs">
                    <div>
                        Planificación # <%= Request.QueryString("planid") %>
                    </div>
                    <div>
                        <asp:Label ID="lblRuta" runat="server" Text="Ruta"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblWhsCardcode" runat="server" Text="Ruta"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblAlmActual" runat="server" Text="Codigo"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblNombreAlmacen" runat="server" Text="Almacen"></asp:Label>
                    </div>
                </div>
                <%--CAMPOS PARA ALMACENES NO PRO, SALDOS Y BLOQUEO--%>
                <div class="row my-3">
                    <div class="col">
                        <label for="txtEstatusCliente">Estatus Cliente</label>
                        <asp:TextBox ID="txtEstatusCliente" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="col">
                        <label for="txtLimiteCredito">Limite Credito</label>
                        <asp:TextBox ID="txtLimiteCredito" runat="server" class="form-control text-right" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="col">
                        <label for="txtSaldoCuenta">Saldo Cuenta</label>
                        <asp:TextBox ID="txtSaldoCuenta" runat="server" class="form-control text-right" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="col">
                        <label for="txtSaldoConsignacion">Saldo Consignacion</label>
                        <asp:TextBox ID="txtSaldoConsignacion" runat="server" class="form-control text-right" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="row my-3">
                    <div class="col">
                        <label for="txtCodigoAlmacen">Filtrar</label>
                        <asp:TextBox ID="txtFilter" runat="server" Text="" placeholder="Escriba Para Buscar"
                            CssClass="form-control txt-buscador" data-table="gridAlmacenesDespachos" onkeyup="convertToUppercase('txtFilter')"></asp:TextBox>
                        <script>
                            $(".txt-buscador[data-table]").on('keyup', function () {
                                var value = $(this).val().toLowerCase();
                                const table = $(this).attr("data-table");

                                $(`#${table} tbody tr`).filter(function () {
                                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
                                });
                            });
                        </script>
                    </div>
                    <div class="col">
                        <label for="txtCodigoAlmacen">Codigo Almacen</label>
                        <asp:TextBox ID="txtCodigoAlmacen" runat="server" CssClass="form-control" placeholder="Codigo Almacen Agregar Manual"></asp:TextBox>
                    </div>
                    <div class="col">
                        <label for="btnAgregarAlmacenManual">Agregar</label>
                        <asp:Button ID="btnAgregarAlmacenManual" runat="server" class="btn btn-block btn-warning" Text="Agregar Alm" />
                    </div>
                </div>
                <div id="contenido1" style="overflow-y: auto; max-height: 500px; width: auto">
                    <asp:GridView ID="gridAlmacenesDespachos" runat="server" AutoGenerateColumns="false" CssClass="ui compact celled table selectable hoverable"
                        Width="100%">
                        <HeaderStyle CssClass="thead-dark sticky-header" />
                        <Columns>
                            <asp:BoundField DataField="Ruta" HeaderText="Ruta" />
                            <asp:BoundField DataField="Canal" HeaderText="Canal" />
                            <asp:BoundField DataField="CRanking" HeaderText="R Rank" />
                            <asp:BoundField DataField="CIndice" HeaderText="R Des" />
                            <asp:BoundField DataField="Codigo" HeaderText="Alm" />
                            <asp:BoundField DataField="Almacen" HeaderText="N Alm" />
                            <asp:BoundField DataField="Indice" HeaderText="Ind" ItemStyle-CssClass="heatmap-cell1" />
                            <asp:BoundField DataField="Indice_Proyectado" HeaderText="Ind Pro." ItemStyle-CssClass="heatmap-cell1" />
                            <asp:BoundField DataField="Cuadro" HeaderText="CB" />
                            <asp:BoundField DataField="Despacho" HeaderText="Des" />
                            <asp:BoundField DataField="COMP" HeaderText="Co" />
                            <asp:BoundField DataField="SOL" HeaderText="Sol" />
                            <asp:BoundField DataField="Transito" HeaderText="Tran" />
                            <asp:BoundField DataField="Fisico" HeaderText="Fis" />
                            <asp:BoundField DataField="Faltante" HeaderText="Fal" />
                            <asp:BoundField DataField="UNDS" HeaderText="Und" />
                            <asp:BoundField DataField="Headerid" HeaderText="#Plan" />
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/bike.png" Text="Sugerir" CommandName="Sugerir" HeaderText="Sugerir"
                                ItemStyle-HorizontalAlign="Center" FooterStyle-VerticalAlign="Middle">
                                <ControlStyle Height="20px" Width="20px" />
                                <ItemStyle Wrap="False" />
                            </asp:ButtonField>
                        </Columns>
                    </asp:GridView>
                    <style>
                        .heatmap-cell1 {
                            font-size: small;
                            color: white;
                            text-shadow: 1px 1px black;
                            text-align: center;
                            align-content: center;
                        }
                    </style>

                    <script>
                        var cells1 = document.querySelectorAll('.heatmap-cell1');
                        var values = Array.from(cells1, cell => parseFloat(cell.innerText));
                        var colorScale1 = d3.scaleSequential()
                            .domain(d3.extent(values)) // Calcula el rango de valores (mínimo y máximo)
                            .interpolator(d3.interpolateOranges); // Gradiente de amarillo a rojo (d3-color-scheme)

                        cells1.forEach(function (cell) {
                            var value = parseFloat(cell.innerText);
                            cell.style.backgroundColor = colorScale1(value);
                        });

                    </script>
                </div>
            </div>
            <div class="seven wide column" style="border-left: 2px solid black;" id="columna2">
                <div class="ui top attached tabular menu">
                    <a class="item active" data-tab="first" id="tab1">Seleccion Motos</a>
                    <a class="item" data-tab="second" id="tab2">Entregas/Facturas</a>
                    <a class="item" data-tab="third" id="tab3">Capacidad Piso Ventas</a>
                </div>
                <div class="ui bottom attached tab segment active" data-tab="first">
                    <div class="container-divs my-3">
                        <div>
                            Motos Portal CI
                        </div>
                        <div>
                            <asp:Label ID="lblUnidadesCI" runat="server" Text="Gen Unidades: 0"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="lblEspaciosCI" runat="server" Text="Gen Espacios: 0"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="lblFilterUnidadesCI" runat="server" Text="Unidades: 0"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="lblFilterEspaciosCI" runat="server" Text="Espacios: 0"></asp:Label>
                        </div>
                    </div>
                    <div class="container-divs my-3">
                        <div>
                            Motos Agregadas Sugerido
                        </div>
                        <div>
                            <asp:Label ID="lblUnidades" runat="server" Text="Unidades: 0"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="lblEspacios" runat="server" Text="Espacios: 0"></asp:Label>
                        </div>
                    </div>
                    <div id="contenido2" style="overflow-y: auto; max-height: 500px; width: auto">
                        <div class="row">
                            <div class="col">
                                <%--<asp:Button ID="btnEliminarPedidoTemp" runat="server" CssClass="btn btn-block btn-danger" Text="Eliminar Todo" />--%>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <asp:GridView ID="gridPedidoTemp" runat="server" AutoGenerateColumns="false" CssClass="ui compact celled table selectable hoverable" Width="100%">
                                    <HeaderStyle CssClass="thead-dark" />
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderText="Id" />
                                        <asp:BoundField DataField="ALMDESTINO" HeaderText="Alm" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ARTICULO" HeaderText="Articulo" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="MODELO" HeaderText="Modelo" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ESPACIOS" HeaderText="Espacios" />
                                        <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" />
                                        <asp:BoundField DataField="QTYLOGISTICA" HeaderText="Logistica" />
                                        <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/delete.png" Text="Eliminar" CommandName="Eliminar" HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center" FooterStyle-VerticalAlign="Middle">
                                            <ControlStyle Height="20px" Width="20px" />
                                            <ItemStyle Wrap="False" />
                                        </asp:ButtonField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="ui bottom attached tab segment" data-tab="second">
                    <div class="row my-3">
                        <div class="col-3">
                            <asp:TextBox ID="txtbuscadorPP" runat="server" Text="" placeholder="Escriba Para Buscar"
                                CssClass="form-control txt-buscadorPP" data-table="gridMotosPortalPedidos" onkeyup="convertToUppercase('txtbuscadorPP')"></asp:TextBox>
                            <script>
                                $(".txt-buscadorPP[data-table]").on('keyup', function () {
                                    var value = $(this).val().toLowerCase();
                                    const table = $(this).attr("data-table");

                                    $(`#${table} tbody tr`).filter(function () {
                                        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
                                    });
                                });
                            </script>
                        </div>
                    </div>
                    <div id="contenido5" style="overflow-y: auto; max-height: 500px; width: auto">
                        <asp:GridView ID="gridMotosPortalPedidos" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered table-hover" Width="100%"
                            HeaderStyle-Font-Size="12px"
                            EmptyDataText="No data" CellPadding="4"
                            ForeColor="#333333" GridLines="None">
                            <HeaderStyle CssClass="thead-dark" />
                            <Columns>
                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbDocument" runat="server" CssClass="document-checkbox" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="ui bottom attached tab segment" data-tab="third">
                    <div id="contenido6" style="overflow-y: auto; max-height: 600px; width: auto">
                        <b><asp:Label ID="lblCurrentWHscode" runat="server" Text=""></asp:Label></b>
                        <asp:GridView ID="gridCapacidadPiso" runat="server"
                            AutoGenerateColumns="false"
                            CssClass="table table-bordered table-hover"
                            Width="100%"
                            HeaderStyle-Font-Size="12px"
                            EmptyDataText="No data"
                            CellPadding="4"
                            ForeColor="#333333"
                            GridLines="None"
                            ShowFooter="true">
                            <HeaderStyle CssClass="thead-dark" />
                            <Columns>
                                <asp:BoundField DataField="SEGMENTO" HeaderText="Segmento" ItemStyle-HorizontalAlign="Left"/>
                                <asp:BoundField DataField="CB_AUTORIZADO" HeaderText="Cap Aut" ItemStyle-HorizontalAlign="Center"/>
                                <asp:BoundField DataField="CB_ACTUAL" HeaderText="Inv Piso" ItemStyle-HorizontalAlign="Center"/>
                                <asp:BoundField DataField="CB_TRANSITO" HeaderText="Transito" ItemStyle-HorizontalAlign="Center"/>
                                <asp:BoundField DataField="CB_DESPACHOS" HeaderText="Despachos" ItemStyle-HorizontalAlign="Center"/>
                                <asp:TemplateField HeaderText="Sugerido" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSugerido" runat="server" Text="0"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CB_PROYECTADO" HeaderText="Inv Proyectado" ItemStyle-HorizontalAlign="Center"/>
                                <asp:BoundField DataField="CB_RESTANTE" HeaderText="Exceso/Faltante" ItemStyle-HorizontalAlign="Center"/>
                                <asp:BoundField DataField="ABAST" HeaderText="% Abast" ItemStyle-HorizontalAlign="Center"/>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="main-container d-flex flex-row justify-content-center align-items-center" style="gap:160px">
                        <div class="prom1 me-3">
                            <div class="col">Indicador % Inv Piso</div>
                            <div class="col">
                                <asp:Label ID="lblPromedio1" runat="server" Text="0" Style="font-size: xx-large; font-weight: bold;"></asp:Label>
                                </div>
                        </div>
                        <div class="prom2">
                            <div class="col">Indicador % Inv Proyectado</div>
                            <div class="col">
                                <asp:Label ID="lblPromedio2" runat="server" Text="0" Style="font-size: xx-large; font-weight: bold;"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="ui grid mx-1">
            <div class="sixteen wide column" id="columna3">
                <div class="row">
                    <div class="col-2">
                        <asp:TextBox ID="txtfiltro" runat="server" Text="" placeholder="Escriba Para Buscar"
                            CssClass="form-control txt-filtro" data-table="gridCuadroBasico" onkeyup="convertToUppercase('txtfiltro')"></asp:TextBox>
                        <script>
                            $(".txt-filtro[data-table]").on('keyup', function () {
                                var value = $(this).val().toLowerCase();
                                const table = $(this).attr("data-table");

                                $(`#${table} tbody tr`).filter(function () {
                                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
                                });
                            });
                        </script>
                    </div>
                </div>
                <div id="contenido3" style="overflow-y: auto; max-height: 600px; width: 100%">
                    <asp:HiddenField ID="hfPlanId" runat="server" />
                    <asp:GridView ID="gridCuadroBasico" runat="server" AutoGenerateColumns="false" CssClass="ui compact celled table selectable hoverable"
                        Width="100%">
                        <HeaderStyle CssClass="thead-dark sticky-header" />
                        <Columns>
                            <asp:BoundField DataField="ROWID" HeaderText="Id" />
                            <asp:BoundField DataField="CODE" HeaderText="Cod" />
                            <asp:TemplateField HeaderText="Modelo">
                                <ItemTemplate>
                                    <div class="ui raised segment">
                                        <a class="ui red ribbon label">Ranking Global: <%# Eval("Ranking") %></a>
                                        <a class="ui orange right ribbon label">Ranking Local <%# Eval("ROWID") %></a>
                                        <div class="flex" style="display: flex; justify-content: space-around; align-items: center;">
                                            <div>
                                                <asp:Label ID="lblModelo" runat="server" Text='<%# Eval("MODELO") %>' Style="margin-top: 3px; margin-bottom: 3px"></asp:Label>
                                            </div>
                                            <div>
                                                <asp:Button ID="btnBI" type="button" runat="server" Text="R | E" CssClass="mini ui button teal" CommandName="BI" CommandArgument='<%# Eval("ROWID") %>' />
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="3m" HeaderText="3m" ItemStyle-CssClass="heatmap-cell1" />
                            <asp:BoundField DataField="6m" HeaderText="6m" ItemStyle-CssClass="heatmap-cell1" />
                            <asp:BoundField DataField="12m" HeaderText="12m" ItemStyle-CssClass="heatmap-cell1" />
                            <asp:BoundField DataField="3mp" HeaderText="%3m" DataFormatString="{0:p}" ItemStyle-CssClass="heatmap-cell2" />
                            <asp:BoundField DataField="6mp" HeaderText="%6m" DataFormatString="{0:p}" ItemStyle-CssClass="heatmap-cell2" />
                            <asp:BoundField DataField="12mp" HeaderText="%12m" DataFormatString="{0:p}" ItemStyle-CssClass="heatmap-cell2" />
                            <asp:BoundField DataField="DCM00" HeaderText="Stock" ItemStyle-CssClass="heatmap-cell3" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="CB" HeaderText="CB" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Despacho" HeaderText="Desp." ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Comprometido" HeaderText="Comp" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Solicitado" HeaderText="Soli" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Transito" HeaderText="Tran" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Fisico" HeaderText="Fis" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Faltante" HeaderText="Falt" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Actual" HeaderText="Vta.A" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="A" HeaderText="-30" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="B" HeaderText="-60" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="C" HeaderText="-90" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Total" HeaderText="Total" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="UltVenta" HeaderText="UltVenta" />
                            <asp:BoundField DataField="Logistica" HeaderText="Logistica" />
                            <asp:BoundField DataField="TipoMoto" HeaderText="Tipo" />
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/bike.png" Text="Sugerir" CommandName="Sugerir" HeaderText="Sugerir">
                                <ControlStyle Height="30px" Width="30" />
                                <ItemStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:ButtonField>
                        </Columns>
                    </asp:GridView>
                    <style>
                        .heatmap-cell1, .heatmap-cell2 {
                            font-weight: bold;
                            font-size: large;
                            color: white;
                            text-shadow: 1px 1px black;
                            text-align: center;
                            align-content: center;
                        }
                    </style>

                </div>
            </div>
        </div>
        <div id="contenido4" style="overflow-y: auto; max-height: 300px; width: auto">
            <div class="row mx-5 my-5">
                <div class="col">
                    <asp:Button ID="btnRegresar" runat="server" CssClass="btn btn-block btn-warning" Text="Regresar" />
                </div>
                <div class="col">
                    <asp:Button ID="btnContinuar" runat="server" CssClass="btn btn-block btn-info" Text="Continuar" />
                </div>
            </div>
        </div>

        <!-- Botón oculto para activar el modal -->
        <asp:Button ID="btnShowModal" runat="server" Style="display: none;" />

        <!-- Panel que contiene el contenido del modal -->
        <asp:Panel ID="pnlPopupColoresMotos" runat="server" CssClass="modalPanel" Style="display: none;">
            <div class="container-fluid" id="maincontainer" runat="server">
                <h1 class="mx-3">Seleccionar Modelos y Colores a Enviar</h1>

                <div class="row mx-3 mb-2">
                    <asp:Label ID="lblrutaWhs" runat="server" Text=""></asp:Label>
                    |
            <asp:Label ID="lblcardcode" runat="server" Text="Cardcode"></asp:Label>
                    |
            <asp:Label ID="lblAlmOrigen" runat="server" Text="DCM00"></asp:Label>
                    |
            <asp:Label ID="lblAlmDestino" runat="server" Text="almDestino"></asp:Label>
                    |
            <asp:Label ID="lblSugerido" runat="server" Text="SUGERIDO"></asp:Label>
                    |
            <asp:Label ID="lblCB" runat="server" Text="Cuadro Basico"></asp:Label>
                    |
            <asp:Label ID="lblFISICO" runat="server" Text="Inv Fisico"></asp:Label>
                    |
            <asp:Label ID="lblFALTANTE" runat="server" Text="Faltante"></asp:Label>
                    |
            <asp:Label ID="lblVTAA" runat="server" Text="Venta"></asp:Label>
                    |
            <asp:Label ID="lblModeloCode" runat="server" Text="Codigo Modelo"></asp:Label>
                    |
            <asp:Label ID="lblModeMoto" runat="server" Text="Modelo Moto"></asp:Label>
                    |
            <asp:Label ID="lblplanId" runat="server" Text="Plan Id"></asp:Label>
                </div>

                <div class="row mx-3" style="display: flex; align-content: center;">
                    <asp:GridView ID="gridColoresMoto" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" Width="100%">
                        <HeaderStyle CssClass="thead-dark" />
                        <Columns>
                            <asp:BoundField DataField="itemcode" HeaderText="Codigo Articulo" />
                            <asp:BoundField DataField="itemname" HeaderText="Descripcion Articulo" />
                            <asp:BoundField DataField="Modelo" HeaderText="Modelo" />
                            <asp:BoundField DataField="Espacios" HeaderText="Espacios" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="CEDIS" HeaderText="Inv. DCM00" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="SUCURSAL" HeaderText="Inv. Sucursal" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="Cant">
                                <ItemTemplate>
                                    <asp:TextBox ID="qty" runat="server" ClientIDMode="Static" Width="50px" Text="0" CssClass="txt_values text-right" TextMode="Number" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="row mx-3 mt-3">
                    <div class="col-6">
                        <asp:Button ID="btnCancelColoresMotos" runat="server" CssClass="btn btn-block btn-danger" Text="Cerrar Ventana" />
                    </div>
                    <div class="col-6">
                        <asp:Button ID="btnModalColoresMotos" runat="server" CssClass="btn btn-block btn-success" Text="Agregar" />
                    </div>
                </div>
            </div>
        </asp:Panel>

        <!-- ModalPopupExtender -->
        <asp:ModalPopupExtender
            ID="ModalPopupColoresMotos"
            runat="server"
            TargetControlID="btnShowModal"
            PopupControlID="pnlPopupColoresMotos"
            BackgroundCssClass="modalBackground"
            CancelControlID="btnCancelColoresMotos" />

        <center>
            <div class="container">
                <footer class="main-footer">
                    <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
                </footer>
            </div>
        </center>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.css" />
        <script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.js" integrity="sha512-gnoBksrDbaMnlE0rhhkcx3iwzvgBGz6mOEj4/Y5ZY09n55dYddx6+WYc72A55qEesV8VX2iMomteIwobeGK1BQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.css" integrity="sha512-3quBdRGJyLy79hzhDDcBzANW+mVqPctrGCfIPosHQtMKb3rKsCxfyslzwlz2wj1dT8A7UX+sEvDjaUv+WExQrA==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    </form>
</body>
</html>

<!-- AdminLTE App -->
<script src="dist/js/adminlte.js"></script>


<!-- DataTables  & Plugins -->
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
<link rel="stylesheet" href="https://cdn.datatables.net/fixedcolumns/4.3.0/css/fixedColumns.dataTables.min.css" />
<script src="https://cdn.datatables.net/fixedcolumns/4.3.0/js/dataTables.fixedColumns.min.js"></script>

<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

<script>
    function convertToUppercase(elementId) {
        var textBox = document.getElementById(elementId);
        textBox.value = textBox.value.toUpperCase();
    }
</script>
<script>
    function toggleFullscreen(columnId, contentId, button) {
        const element = document.getElementById(columnId);
        const content = document.getElementById(contentId);

        if (document.fullscreenElement) {
            // Ya está en fullscreen -> salir
            document.exitFullscreen();
            button.innerText = "Expandir";
            content.style.maxHeight = "300px";

            // CORREGIDO: cuando el que sale es contenido4
            if (contentId === 'contenido4') {
                const segmentos = document.querySelectorAll('.ui.raised.segment, .ui.raised.segments');
                segmentos.forEach(segment => {
                    segment.style.setProperty('width', '80px', 'important');
                });

            }
        } else {
            // Entrar a fullscreen
            element.requestFullscreen().then(() => {
                button.innerText = "Retornar";
                content.style.maxHeight = "600px";

                // Opcional: si quieres hacer algo solo para contenido4 también aquí
                if (contentId === 'contenido4') {
                    const segmentos = document.querySelectorAll('.ui.raised.segment, .ui.raised.segments');
                    segmentos.forEach(segment => {
                        segment.style.setProperty('width', '200px', 'important');
                    });
                }
            }).catch((err) => {
                alert(`Error al intentar pantalla completa: ${err.message} (${err.name})`);
            });
        }
    }

    // Para asegurar que al salir de fullscreen se restauren todos los botones y alturas
    document.addEventListener('fullscreenchange', () => {
        if (!document.fullscreenElement) {
            // Salió de fullscreen, restaurar botones y estilos
            const buttons = document.querySelectorAll('.ui.button');
            buttons.forEach(button => {
                button.innerText = "Expandir";
            });

            const contenidos = document.querySelectorAll('[id^="contenido"]');
            contenidos.forEach(content => {
                content.style.maxHeight = "300px";
            });
        }
    });

</script>

<script>
    var cells1 = document.querySelectorAll('.heatmap-cell1');
    var colorScale1 = d3.scaleSequential()
        .domain(d3.extent(cells1, function (cell) { return parseFloat(cell.innerText); }))
        .interpolator(d3.interpolateOranges);
    cells1.forEach(function (cell) {
        var value = parseFloat(cell.innerText);
        cell.style.backgroundColor = colorScale1(value);
    });
    var cells2 = document.querySelectorAll('.heatmap-cell2');

    var colorScale2 = d3.scaleSequential()
        .domain(d3.extent(cells2, function (cell) { return parseFloat(cell.innerText); }))
        .interpolator(d3.interpolateGreens)
        .interpolator(function (t) { return d3.interpolateGreens(1 - t); });
    cells2.forEach(function (cell) {
        var value = parseFloat(cell.innerText);
        cell.style.backgroundColor = colorScale2(value);
    });
</script>


<script>
    $('.menu .item').tab();


    function cerrarPlan(idPlan) {
        Swal.fire({
            title: '¿Está seguro de cerrar esta planificación?',
            text: 'Esta acción no se puede deshacer.',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, cerrar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: "CerrarPlanificacionHandler.ashx",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({ id: idPlan }),
                    success: function (response) {
                        if (response.success) {
                            Swal.fire('Cerrado', 'La planificación se ha cerrado exitosamente.', 'success').then(() => {
                                location.assign("TrasladosDDashboard.aspx");
                            });
                        } else {
                            Swal.fire('Error', response.message, 'error');
                        }
                    },
                    error: function () {
                        Swal.fire('Error', 'No se pudo completar la solicitud.', 'error');
                    }
                });
            }
        });
    }
</script>
<script>
    function cerrarDespacho(idPlan) {
        Swal.fire({
            title: '¿Está seguro de cerrar este Despacho?',
            text: 'Esta acción no se puede deshacer.',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, cerrar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: "CerrarDespachoHandler.ashx",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({ id: idPlan }),
                    success: function (response) {
                        if (response.success) {
                            Swal.fire('Cerrado', 'El Despacho se ha cerrado exitosamente.', 'success').then(() => {
                                location.assign("TrasladosDDashboard.aspx");
                            });
                        } else {
                            Swal.fire('Error', response.message, 'error');
                        }
                    },
                    error: function () {
                        Swal.fire('Error', 'No se pudo completar la solicitud.', 'error');
                    }
                });
            }
        });
    }
</script>

<script type="text/javascript">
    $("#gridCuadroBasico").DataTable({
        "responsive": true,
        "lengthChange": false,
        "autoWidth": false,
        "buttons": ["excel"],
        "sort": true,
        "order": [[0, 'asc']],
        "deferRender": true,
        "bPaginate": false,
        //"lengthMenu": [[5, -1], [5, "All"]],
    }).buttons().container().appendTo('#gridCuadroBasico_wrapper .col-md-6:eq(0)');

</script>

<script>
    function openPopup(url) {
        var width = 900;
        var height = 600;
        var left = (window.innerWidth - width) / 2;
        var top = (window.innerHeight - height) / 2;

        if (left < 0) left = 0;
        if (top < 0) top = 0;

        var popupWindow = window.open(url, 'Popup', 'width=' + width + ',height=' + height + ',left=' + left + ',top=' + top + ',resizable=yes,scrollbars=yes');
        popupWindow.focus();
    }
</script>

<%--<script type="text/javascript">
    function marcarCoincidencias() {
        // Obtener los valores de la columna 2 (índice 1) del gridPedidoTemp
        var valoresPedidoTemp = [];
        $('#<%= gridAlmacenesDespachos.ClientID %> tr').each(function () {
            var celda = $(this).find('td:eq(0)');
            if (celda.length > 0) {
                valoresPedidoTemp.push(celda.text().trim());
            }
        });

        // Recorrer gridMotosPortalPedidos y marcar coincidencias
        $('#<%= gridMotosPortalPedidos.ClientID %> tr').each(function () {
            var celda = $(this).find('td:eq(1)');
            if (celda.length > 0) {
                var valor = celda.text().trim();
                if (valoresPedidoTemp.includes(valor)) {
                    $(this).css('background-color', 'lightgreen');
                    $(this).addClass("selected-row");
                }
            }
        });
    }
    //sumar los espacios ya comprometidos
    function sumarEspacios() {
        let total = 0;
        let unidades = 0;
        $("#gridMotosPortalPedidos tbody tr.selected-row").each(function () {
            let espacio = parseInt($(this).find("td:eq(12)").html())
            total += isNaN(espacio) ? 0 : espacio;
            unidades++
        })
        console.log(total)
        $('#lblEspaciosPotalPedidos').html(`Espacios: ${total}`);
        $('#lblUnidadesPortalPedidos').html(`Unidades: ${unidades}`);
    }

    // Ejecutar al cargar la página
    $(document).ready(function () {
        marcarCoincidencias();
        sumarEspacios();
    });
</script>--%>