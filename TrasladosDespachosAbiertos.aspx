<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosDespachosAbiertos.aspx.vb" Inherits="TrasladosDespachosAbiertos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Traslados Despachos Abiertos</title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.3.0/css/all.min.css" />
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>
    <script src="https://d3js.org/d3.v6.min.js"></script>
    <script src='https://cdn.jsdelivr.net/npm/sweetalert2@11'></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />

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
        /* Estilo base del botón */
        .btn-group .btn-default {
            background-color: #f8f9fa; /* Color de fondo por defecto */
            border-color: #ccc; /* Color del borde */
            color: #333; /* Color del texto */
            transition: background-color 0.3s ease; /* Transición suave */
        }
            /* Efecto hover */
            .btn-group .btn-default:hover {
                background-color: #28a745; /* Color de fondo al pasar el mouse (verde) */
                border-color: #28a745; /* Color del borde al pasar el mouse */
                color: #fff; /* Color del texto al pasar el mouse */
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
            max-height: 400px;
            overflow-y: auto;
            margin: auto;
            position: fixed;
            top: 5%;
            left: 0;
            right: 0;
            z-index: 1001;
            left: 0px !important
        }
    </style>
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
        <div class="container-fluid mt-3">
            <asp:Label ID="lblPlanId" runat="server" Text="" Visible="false"></asp:Label>
            <div class="btnContainer">
                <div class="row">
                    <div class="col-2">
                        <asp:Button ID="btnCrearCamion" runat="server" CssClass="btn btn-block btn-info w-100" Text="Crear Camion" />
                    </div>
                    <div class="col-2">
                    </div>
                    <div class="col-2">
                        <asp:Button ID="btnAgregarCamion" runat="server" CssClass="btn btn-block btn-warning w-100" Text="Agregar a Camion" />
                    </div>
                    <div class="col-2">
                        <asp:DropDownList ID="drpListadoCamionesDisp" runat="server" CssClass="form-control w-100"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="ui top attached tabular menu">
                <a class="item active" data-tab="first" id="tab1">Despachos Abiertos</a>
                <a class="item" data-tab="second" id="tab2">Despachos Abiertos Detalle Motos</a>
                <a class="item" data-tab="third" id="tab3">Camiones Abiertos</a>
                <a class="item" data-tab="fourth" id="tab4">Detalle Camiones</a>
            </div>
            <%--CONTENIDO PRIMER TAB--%>
            <div class="ui bottom attached tab segment active" data-tab="first">
                <asp:GridView ID="gridDespachosAbiertos" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover mt-3"
                    Width="100%" OnRowDataBound="gridDespachosAbiertos_RowDataBound">
                    <HeaderStyle CssClass="thead-dark sticky-header" />
                    <Columns>
                        <asp:TemplateField HeaderText="Progreso de Picking">
                            <ItemTemplate>
                                <div class="progress">
                                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar"
                                        aria-valuemin="0"
                                        aria-valuenow='<%# Eval("Preparado") %>'
                                        aria-valuemax='<%# Eval("Unds") %>'
                                        style='width: <%# Eval("Porcentaje") %>%'>
                                        <%# FormatNumber(Eval("Porcentaje"), 2, TriState.True) %>%
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbDocument" runat="server" CssClass="document-checkbox" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DespachoId" HeaderText="Despacho ID" />
                        <asp:BoundField DataField="PlanId" HeaderText="Plan ID" />
                        <asp:BoundField DataField="FechaDespacho" HeaderText="Fecha Despacho" />
                        <asp:BoundField DataField="DiasRestantes" HeaderText="Dias Restantes" />
                        <asp:BoundField DataField="Canal" HeaderText="Canal" />
                        <asp:BoundField DataField="Ruta" HeaderText="Ruta" />
                        <asp:BoundField DataField="Almacenes" HeaderText="Almacenes" />
                        <asp:BoundField DataField="TotalAlm" HeaderText="Total Alm" />
                        <asp:BoundField DataField="TotalFalt" HeaderText="Faltante" />
                        <asp:BoundField DataField="Unds" HeaderText="Total Unds" />
                        <asp:BoundField DataField="TotalEspacios" HeaderText="Total Espacios" />
                        <asp:BoundField DataField="Preparado" HeaderText="Preparado" />
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/delete.png" Text="Eliminar" CommandName="Eliminar" HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ControlStyle Height="30px" Width="30px" />
                            <ItemStyle Wrap="False" />
                        </asp:ButtonField>

                        <%--<asp:TemplateField HeaderText="Cerrar Despacho">
                            <ItemTemplate>
                                <button type="button" class="btn btn-danger" onclick='cerrarDespachoCamionDespachado(<%# Eval("PLANID") %> , <%# Eval("DespachoId") %>)'>Cerrar Despacho</button>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                </asp:GridView>
            </div>
            <%--CONTENIDO SEGUNDO TAB--%>
            <div class="ui bottom attached tab segment" data-tab="second">
                <div class="row my3">
                    <div class="col-3">
                        <label for="txtCodigoAlmacen">Filtrar</label>
                        <asp:TextBox ID="txtFilter" runat="server" Text="" placeholder="Escriba Para Buscar"
                            CssClass="form-control txt-Filter" data-table="gridDespachosAbiertosMotos" onkeyup="convertToUppercase('txtFilter')"></asp:TextBox>
                    </div>
                    <div class="col-3">
                        <%--<label for="btnExportarExcelDespachoDetalle">Exportal a Excel</label>--%>
                        <button type="button" id="btnExportarExcelDespachoDetalle" onclick="exportarExcel('gridDespachosAbiertosMotos')" class="btn btn-block btn-success">Exportar</button>
                    </div>
                </div>
                <asp:GridView ID="gridDespachosAbiertosMotos" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered table-hover mt-3"
                    Width="100%">
                    <HeaderStyle CssClass="thead-dark sticky-header" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/delete.png" Text="Eliminar" CommandName="Eliminar" HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ControlStyle Height="30px" Width="30px" />
                            <ItemStyle Wrap="False" />
                        </asp:ButtonField>
                    </Columns>
                </asp:GridView>
            </div>
            <%--CONTENIDO TERCER TAB--%>
            <div class="ui bottom attached tab segment" data-tab="third">
                <asp:GridView ID="gridCamiones" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered table-hover mt-3"
                    Width="100%">
                    <HeaderStyle CssClass="thead-dark sticky-header" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/view.png" Text="Ver" CommandName="Ver" HeaderText="Ver" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ControlStyle Height="30px" Width="30px"  />
                            <ItemStyle Wrap="False" />
                        </asp:ButtonField>
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/Dacion04.gif" Text="Imprimir" CommandName="Imprimir" HeaderText="Imprimir" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ControlStyle Height="30px" Width="30px" />
                            <ItemStyle Wrap="False" />
                        </asp:ButtonField>
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/delete.png" Text="Eliminar" CommandName="Eliminar" HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ControlStyle Height="30px" Width="30px" />
                            <ItemStyle Wrap="False" />
                        </asp:ButtonField>
                    </Columns>
                </asp:GridView>
            </div>
            <%--CONTENIDO CUARTO TAB--%>
            <div class="ui bottom attached tab segment" data-tab="fourth">
                <asp:GridView ID="gridCamionesDetalle" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered table-hover mt-3" Width="100%">
                    <HeaderStyle CssClass="thead-dark sticky-header" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/delete.png" Text="Eliminar" CommandName="Eliminar" HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ControlStyle Height="30px" Width="30px" />
                            <ItemStyle Wrap="False" />
                        </asp:ButtonField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <!-- Botón oculto para activar el modal -->
        <asp:Button ID="btnShowModal" runat="server" Style="display: none;" />

        <!-- Panel que contiene el contenido del modal -->
        <asp:Panel ID="pnlPopupColoresMotos" runat="server" CssClass="modalPanel" Style="display: none;">
            <div class="container-fluid" id="maincontainer" runat="server">
                <h1 class="mx-3 my-3 text-center">Datos Camion</h1>

                <div class="row mx-3 my-3">
                    <div class="col-3">
                        Fecha Salida Camion
                    </div>
                    <div class="col-3">
                        <asp:TextBox ID="txtFechaCamion" runat="server" CssClass="form-control w-100" type="date"></asp:TextBox>
                    </div>
                </div>
                <div class="row mx-3 my-3">
                    <div class="col-3">
                        Seleccione Placa Vehiculo
                    </div>
                    <div class="col-3">
                        <asp:DropDownList ID="drpPlacas" runat="server" class="ui search dropdown fluid" name="drpPlacas">
                        </asp:DropDownList>
                    </div>
                    <div class="col-3">
                        Seleccione Motorista
                    </div>
                    <div class="col-3">
                        <asp:DropDownList ID="drpMotorista" runat="server" class="ui search dropdown fluid" name="drpMotorista">
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="row mx-3 my-3">
                    <div class="col-3">
                        <asp:Button ID="btnCancelColoresMotos" runat="server" CssClass="btn btn-block btn-danger" Text="Cerrar Ventana" />
                    </div>
                    <div class="col-3">
                        <asp:Button ID="btnCamionCrear" runat="server" CssClass="btn btn-block btn-success" Text="Agregar" />
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

        <div class="container-fkuid" style="display: flex; flex-direction: column; align-items: center">
            <footer class="main-footer">
                <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
            </footer>
        </div>
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.css" />
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.js" integrity="sha512-gnoBksrDbaMnlE0rhhkcx3iwzvgBGz6mOEj4/Y5ZY09n55dYddx6+WYc72A55qEesV8VX2iMomteIwobeGK1BQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.css" integrity="sha512-3quBdRGJyLy79hzhDDcBzANW+mVqPctrGCfIPosHQtMKb3rKsCxfyslzwlz2wj1dT8A7UX+sEvDjaUv+WExQrA==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    </form>
</body>
</html>
<!-- jQuery -->
<script src="plugins/jquery/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.11.0/umd/popper.min.js" integrity="sha384-b/U6ypiBEHpOf/4+1nzFpr53nxSS+GLCkfwBdFNTxtclqqenISfwAzpKaMNFNmj4" crossorigin="anonymous"></script>


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

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.css" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/semantic.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.js" integrity="sha512-gnoBksrDbaMnlE0rhhkcx3iwzvgBGz6mOEj4/Y5ZY09n55dYddx6+WYc72A55qEesV8VX2iMomteIwobeGK1BQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/fomantic-ui/2.9.3/semantic.min.css" integrity="sha512-3quBdRGJyLy79hzhDDcBzANW+mVqPctrGCfIPosHQtMKb3rKsCxfyslzwlz2wj1dT8A7UX+sEvDjaUv+WExQrA==" crossorigin="anonymous" referrerpolicy="no-referrer" />


<script>
    $('.menu .item').tab();

    $("#drpPlacas").dropdown(
        {
            "fullTextSearch": true
        });
    $("#drpMotorista").dropdown(
        {
            "fullTextSearch": true
        });

</script>
<script>
    function cerrarDespachoCamionDespachado(idPlan, despachoId) {
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
                    url: "CerrarDespachoCargaCamionHandler.ashx",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({ id: idPlan, despachoId }),
                    success: function (response) {
                        if (response.success) {
                            Swal.fire('Cerrado', 'El Despacho se ha cerrado exitosamente.', 'success').then(() => {
                                location.assign("TrasladosDespachosAbiertos.aspx");
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
    function convertToUppercase(elementId) {
        var textBox = document.getElementById(elementId);
        textBox.value = textBox.value.toUpperCase();
    }
</script>
<script>
    $(".txt-Filter[data-table]").on('keyup', function () {
        var value = $(this).val().toLowerCase();
        const table = $(this).attr("data-table");

        $(`#${table} tbody tr`).filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });
</script>

<script>
    function exportarExcel(gridId) {
        var tablaOriginal = document.getElementById(gridId);
        if (!tablaOriginal) {
            alert("No se encontró la tabla.");
            return;
        }

        var tablaNueva = document.createElement('table');
        tablaNueva.border = 1;
        tablaNueva.style.borderCollapse = 'collapse';

        // Recorremos solo filas visibles
        for (var i = 0; i < tablaOriginal.rows.length; i++) {
            var filaOriginal = tablaOriginal.rows[i];

            // Saltar filas ocultas (por filtrado)
            if (filaOriginal.style.display === 'none') continue;

            var filaNueva = tablaNueva.insertRow(-1);

            for (var j = 0; j < filaOriginal.cells.length; j++) {
                // Omitimos la columna 0 (Eliminar)
                if (j === 0) continue;

                var celdaNueva = filaNueva.insertCell(-1);
                celdaNueva.innerHTML = filaOriginal.cells[j].innerText || filaOriginal.cells[j].textContent;
            }
        }

        var html = `
            <html xmlns:x="urn:schemas-microsoft-com:office:excel">
            <head>
                <meta charset="UTF-8">
                <style>
                    td, th { border: 1px solid #000; padding: 4px; font-family: Arial; }
                </style>
            </head>
            <body>
                ${tablaNueva.outerHTML}
            </body>
            </html>`;

        var blob = new Blob([html], { type: 'application/vnd.ms-excel' });
        var url = window.URL.createObjectURL(blob);
        var link = document.createElement('a');
        link.href = url;
        link.download = 'despachos_motos_' + new Date().toISOString().slice(0, 10) + '.xls';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }
</script>
