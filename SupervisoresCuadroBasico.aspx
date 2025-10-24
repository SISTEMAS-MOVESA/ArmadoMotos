<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SupervisoresCuadroBasico.aspx.vb" Inherits="SupervisoresCuadroBasico" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Confirmacion Logistica</title>
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
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>


    <!-- jQuery -->
    <script src="plugins/jquery/jquery.min.js"></script>
    <script src="vendor/bootstrap-4.1/popper.min.js"></script>

    <!-- Bootstrap 4 -->
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
    <!-- ChartJS -->
    <script src="plugins/chart.js/Chart.min.js"></script>
    <!-- Sparkline -->
    <script src="plugins/sparklines/sparkline.js"></script>
    <!-- JQVMap -->
    <script src="plugins/jqvmap/jquery.vmap.min.js"></script>
    <script src="plugins/jqvmap/maps/jquery.vmap.usa.js"></script>
    <!-- jQuery Knob Chart -->
    <script src="plugins/jquery-knob/jquery.knob.min.js"></script>
    <!-- daterangepicker -->
    <script src="plugins/moment/moment.min.js"></script>
    <script src="plugins/daterangepicker/daterangepicker.js"></script>
    <!-- Tempusdominus Bootstrap 4 -->
    <script src="plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>
    <!-- Summernote -->
    <script src="plugins/summernote/summernote-bs4.min.js"></script>
    <!-- overlayScrollbars -->
    <script src="plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js"></script>
    <!-- AdminLTE App -->
    <script src="dist/js/adminlte.js"></script>

    <!-- DataTables  & Plugins -->
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables-bs4/js/dataTables.bootstrap4.min.js"></script>
    <script src="plugins/datatables-responsive/js/dataTables.responsive.min.js"></script>
    <script src="plugins/datatables-responsive/js/responsive.bootstrap4.min.js"></script>
    <script src="plugins/datatables-buttons/js/dataTables.buttons.min.js"></script>
    <script src="plugins/datatables-buttons/js/buttons.bootstrap4.min.js"></script>
    <script src="plugins/jszip/jszip.min.js"></script>
    <script src="plugins/pdfmake/pdfmake.min.js"></script>
    <script src="plugins/pdfmake/vfs_fonts.js"></script>
    <script src="plugins/datatables-buttons/js/buttons.html5.min.js"></script>
    <script src="plugins/datatables-buttons/js/buttons.print.min.js"></script>
    <script src="plugins/datatables-buttons/js/buttons.colVis.min.js"></script>

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

    <form id="form1" runat="server">
        <!-- Top Navigation Menu -->
        <div class="container-fluid">
            <div class="my-3" style="display: flex; justify-content: center; align-content: center; align-items: center;">
                <h1>CUADRO BASICO SUPERVISORES</h1>
            </div>
            <div class="row">
                <div class="col-2">
                    <asp:Button ID="btnIndiceGeneral" runat="server" CssClass="btn btn-secondary btn-lg-custom" Text="Indice General" />
                </div>
                <div class="col-2">
                    <asp:Button ID="btnIndiceHero" runat="server" CssClass="btn btn-danger btn-lg-custom" Text="Indice Hero" />
                </div>
                <div class="col-2">
                    <asp:Button ID="btnCuadroBasico" runat="server" CssClass="btn btn-secondary btn-lg-custom" Text="Cuadro Basico" />
                </div>
                <div class="col-2">
                    <asp:Button ID="btnCalendario" runat="server" CssClass="btn btn-secondary btn-lg-custom" Text="Calendario Despachos" />
                </div>
            </div>
            <div class="container-fluid my-3">
                <div class="row">
                    <div class="col-3">
                        <label for="drpSucursales">Sucursales</label>
                        <asp:DropDownList ID="drpSucursales" runat="server" Width="100%" class="ui search dropdown fluid" name="drpSucursales"></asp:DropDownList>
                    </div>
                    <div class="col-3">
                        <label for="drpSucursales">Cargar Sugerido</label>
                        <asp:Button ID="btnCargarSugerido" runat="server" class="btn btn-info btn-lg btn-block" Text="Cargar" Width="100%" Height="100%" />
                    </div>
                </div>
            </div>
            <asp:Label ID="labelError" runat="server"></asp:Label>
        </div>
        <div class="container-fluid">
            <script src="https://d3js.org/d3.v6.min.js"></script>
            <div class="container-fluid my-3">
                <div class="col-2">
                    <asp:TextBox ID="txtFilter" runat="server" Text="" placeholder="Filtrar Por Modelo"
                        CssClass="form-control" onkeyup="convertToUppercase('txtFilter')"></asp:TextBox>
                </div>
            </div>
            <script>
                $('#txtFilter').keyup(function () {
                    var _val = $(this).val().toLowerCase();
                    $('#gridCuadroBasico tbody tr').each(function () {
                        var _cell = $(this).find('td:eq(2)').text();
                        if (_val === '0' || _cell.toLowerCase().indexOf(_val) !== -1) {
                            $(this).show();
                        } else {
                            $(this).hide();
                        }
                    });
                });
            </script>
            <div class="ui long scrolling container fluid">
                <asp:GridView ID="gridCuadroBasico" runat="server" AutoGenerateColumns="false" class="ui last head foot stuck unstackable celled table" Width="100%">
                    <HeaderStyle CssClass="ui blue inverted table sticky-header" />
                    <Columns>
                        <asp:BoundField DataField="ROWID" HeaderText="RowId" />
                        <asp:BoundField DataField="CODE" HeaderText="Codigo" />
                        <asp:TemplateField HeaderText="Modelo">
                            <ItemTemplate>
                                <div class="ui raised segment">
                                    <a class="ui red ribbon label"><%# Eval("Ranking") %></a>
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
                        <asp:BoundField DataField="DCM00" HeaderText="Stock" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="Fisico" HeaderText="Fisico" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="CB" HeaderText="CB" ItemStyle-CssClass="heatmap-cell3" />
                        <%--<asp:TemplateField HeaderText="Modificar">
                            <ItemTemplate>
                                <div style="display: flex; align-items: center; justify-content: center;" class="itm-number-input">
                                    <button type="button" id="btnSubtract" class="btn btn-danger btn-sm mr-2 btnSubtract">-</button>
                                    <asp:TextBox ID="qty" runat="server" ClientIDMode="Static" Width="60px" Height="100%"
                                        Text='<%# Eval("CB") %>' CssClass="qty txt_values text-right" type="Number" min="0"
                                        data-modelo='<%# Eval("MODELO") %>'></asp:TextBox>
                                    <button type="button" id="btnAdd" class="btn btn-success btn-sm ml-2 btnAdd">+</button>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:BoundField DataField="Actual" HeaderText="Vta.A" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="A" HeaderText="-30" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="B" HeaderText="-60" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="C" HeaderText="-90" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="Total" HeaderText="Total" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="Despacho" HeaderText="Despachos" ItemStyle-CssClass="heatmap-cell3" />
                        <asp:BoundField DataField="UltVenta" HeaderText="UltVenta" />
                    </Columns>
                </asp:GridView>
            </div>
            <style>
                /* Encabezado sticky para el GridView */
                .sticky-header th {
                    position: sticky;
                    top: 0;
                    background-color: #2185d0; /* Color de fondo del encabezado (mismo que en HeaderStyle si quieres consistencia) */
                    z-index: 2; /* Elevar el encabezado sobre las filas del cuerpo */
                    text-align: center;
                }

                /* Asegúrate de que el contenedor tenga un desplazamiento adecuado */
                .scrollable-container {
                    max-height: 500px; /* Ajusta este valor según la altura deseada */
                    overflow-y: auto;
                }

                .heatmap-cell1,
                .heatmap-cell2 {
                    /* font-weight: bold;*/
                    font-size: larger;
                    color: white;
                    text-shadow: 1px 1px black;
                    text-align: center;
                }

                .heatmap-cell3 {
                    font-size: larger;
                    text-align: center;
                }
            </style>
            <script>
                var cells1 = document.querySelectorAll('.heatmap-cell1');
                var colorScale1 = d3.scaleSequential()
                    .domain(d3.extent(cells1, function (cell) { return parseFloat(cell.innerText); }))
                    .interpolator(d3.interpolateBlues);

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

        </div>
        <%--INICIO MODAL NUEVO ALMACEN--%>
        <div class="modal fade" id="modalAdd">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div style="background-color: #272b35;" class="modal-header">
                        <h4 style="color: white;" class=" modal-title">Nuevo Almacen</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span style="color: white;" aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col">
                                <!-- text input -->
                                <div class="form-group">
                                    <label>Codigo Almacen</label>
                                    <asp:TextBox ID="txtCodidoAlm" runat="server" class="form-control" onkeyup="convertToUppercase('txtCodidoAlm')" Text=""></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer justify-content-between">
                            <button type="button" class="btn btn-danger" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnCrearNuevaSucursal" runat="server" Text="Crear" class="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--FIN MODAL NUEVO ALMACEN--%>

        <%--INICIO MODAL NUEVO MODELO--%>
        <div class="modal fade" id="modalAdd_modelo">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div style="background-color: #272b35;" class="modal-header">
                        <h4 style="color: white;" class=" modal-title">Nuevo Modelo</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span style="color: white;" aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col">
                                <!-- text input -->
                                <div class="form-group">
                                    <label>Codigo Modelo</label>
                                    <asp:TextBox ID="txtModelo" runat="server" class="form-control" onkeyup="convertToUppercase('txtModelo')" Text=""></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer justify-content-between">
                            <button type="button" id="btnModeloCancel" class="btn btn-danger" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="btnModeloCrear" runat="server" Text="Crear" class="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--FIN MODAL NUEVO MODELO--%>

        <footer class="main-footer text-center">
            <strong><i class="ion-paintbrush"></i>WebDesing RJ - Copyright &copy; 2022 <a href="#">Movesa</a>.</strong> All rights reserved.
        </footer>
        <asp:ScriptManager ID="sm1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    </form>
</body>
</html>

<script>
    $(window).on('load', function () {
        $("#loftloader-wrapper").hide();
    });

    window.onbeforeunload = function (e) {
        $("#loftloader-wrapper").show();
    }
</script>

<link href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.css" rel="stylesheet" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.5.0/semantic.min.js"></script>

<script>
    $("#drpSucursales").dropdown(
        {
            "fullTextSearch": true
        });
</script>

<script>
    function convertToUppercase(elementId) {
        var textBox = document.getElementById(elementId);
        textBox.value = textBox.value.toUpperCase();
    }

    function updateDatabase(value, modelo) {
        const whsCode = $('#drpSucursales').val();

        console.log(value, whsCode, modelo);
        $.ajax({
            type: "POST",
            url: "UpdateCuadroBasico.ashx",
            data: {
                maximo: value,
                whscode: whsCode,
                modelo: modelo
            },
            success: function (response) {
                console.log("Actualización exitosa: " + response);
                iziToast.success({
                    title: 'Éxito',
                    message: 'La operación se ha realizado correctamente. Respuesta del servidor:' + response,
                    position: 'topRight',
                    timeout: 3000
                });
            },
            error: function (xhr, status, error) {
                console.error("Error al actualizar: " + error);
                iziToast.error({
                    title: 'Error',
                    message: 'Ha ocurrido un error durante la operación.',
                    position: 'topRight',
                    timeout: 5000
                });
            }
        });
    }
</script>

<script>
    $("input.qty").keyup(function (e) {
        let _value = parseInt($(e.target).val());
        _value = _value <= 0 || _value == 0 ? 0 : _value;
        $(e.target).val(_value);

    });

    $("button.btnAdd").click(function (e) {

        let _tr = $(this).parents("td").eq(0);
        let _value = parseInt($(_tr).find(".qty").val());
        _value++;
        $(_tr).find(".qty").val(_value);
        updateDatabase(_value, $(_tr).find(".qty").attr("data-modelo"));

    });
    $("button.btnSubtract").click(function (e) {
        let _parentContainer = $(this).parents("td").eq(0);
        let _qtyInput = $(_parentContainer).find(".qty");
        let _value = parseInt(_qtyInput.val());
        _value = _value > 0 ? _value - 1 : 0;
        _qtyInput.val(_value);

        updateDatabase(_value, $(_qtyInput).attr("data-modelo"));
    });
</script>
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
