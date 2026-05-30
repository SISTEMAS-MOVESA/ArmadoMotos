<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrasladosCargaMacroIndirectoSupervisores.aspx.vb" Inherits="TrasladosCargaMacroIndirectoSupervisores" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Sugerido Logistica</title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback">
    <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css">
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css">
    <link rel="stylesheet" href="plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css">
    <link rel="stylesheet" href="plugins/icheck-bootstrap/icheck-bootstrap.min.css">
    <link rel="stylesheet" href="plugins/jqvmap/jqvmap.min.css">
    <link rel="stylesheet" href="dist/css/adminlte.min.css">
    <link rel="stylesheet" href="plugins/overlayScrollbars/css/OverlayScrollbars.min.css">
    <link rel="stylesheet" href="plugins/daterangepicker/daterangepicker.css">
    <link rel="stylesheet" href="plugins/summernote/summernote-bs4.min.css">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
    <link href="css/movesa_load_spinner.css" rel="stylesheet" />
    <link href="css/TasksCards.css" rel="stylesheet" />

    <%--izitoast--%>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/css/iziToast.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/izitoast/1.4.0/js/iziToast.min.js"></script>

    <style>
        .container {
            display: flex;
            margin-left: 0;
        }

        .top-column {
            flex: 3;
            margin-left: 20px;
            /*background-color: coral;*/
            border-radius: 10px;
        }

        .catalog-column {
            flex: 8;
            margin-left: 20px;
            /*background-color: darkgray;*/
            border-radius: 10px;
        }

        .buttons-column {
            flex: 1;
            border-radius: 10px;
        }

        .catalog-table {
            width: 100%;
        }

        /*.button {
            display: block;
            width: 200px;
            height: 50px;
            margin-bottom: 10px;
            text-align: center;
            background-color: #17a2b8;
            font-size: 18px;
            font-weight: bold;
            line-height: 50px;
            text-transform: uppercase;
            border-radius: 10px;*/ /* Ajusta el valor para obtener el nivel de redondez deseado */
        /*}*/

        /*.buttons-column {
            margin-top: 60px;
            margin-right: 20px;
        }*/

        .sticky-header th {
            position: sticky;
            top: 0;
            z-index: 2;
            background-color: #343a40; /* Color de fondo del encabezado */
            color: white; /* Color del texto */
            border-bottom: 2px solid #dee2e6;
            padding: 8px;
            text-align: left;
        }

        .sticky-grid {
            border-collapse: collapse;
            width: 100%;
        }

        .sticky-grid th,
        .sticky-grid td {
            border: 1px solid #dee2e6;
        }

    </style>

</head>
<body>
    <form id="form1" runat="server">
        <!-- Navigation -->
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark static-top">
     <div class="container">
         <a class="navbar-brand" href="TrasladosDashboardSupervisores.aspx">
             <img src="Imagenes/mnegra.png" width="50" height="40" alt="" />
         </a>
         <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
             <span class="navbar-toggler-icon"></span>
         </button>
         <div class="collapse navbar-collapse" id="navbarSupportedContent">
             <ul class="navbar-nav ms-auto">
                 <li class="nav-item">
                     <a class="nav-link active" aria-current="page" href="TrasladosDashboardSupervisores.aspx">Inicio</a>
                 </li>
                 <li class="nav-item">
                     <a class="nav-link active" aria-current="page" href="TrasladosPresolicitudSupervisores.aspx">Pre Solicitud</a>
                 </li>
                 <li class="nav-item">
                     <a class="nav-link active" aria-current="page" href="TrasladosTrasladosSupervisores.aspx">Traslados de Motos</a>
                 </li>
                 <li class="nav-item">
                     <a class="nav-link active" aria-current="page" href="TrasladosConfirmarSupervisor.aspx">Confirmacion Sugerido</a>
                 </li>
                 <li class="nav-item">
                     <a class="nav-link active" aria-current="page" href="Default.aspx">Cerrar Sesion</a>
                 </li>
             </ul>
         </div>
     </div>
 </nav>
        <div class="d-flex justify-content-start mt-3">
            <div class="col-3">
                <div class="row">
                    <div class="col">Estado</div>
                    <div class="col">
                        <asp:TextBox ID="txtEstatusCliente" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col">Limite Credito</div>
                    <div class="col">
                        <asp:TextBox ID="txtLimiteCredito" runat="server" class="form-control text-right" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col">Saldo de Cuenta</div>
                    <div class="col">
                        <asp:TextBox ID="txtSaldoCuenta" runat="server" class="form-control text-right" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col">Saldo Consignacion</div>
                    <div class="col">
                        <asp:TextBox ID="txtSaldoConsignacion" runat="server" class="form-control text-right" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col">
                        <asp:Label ID="lblNombreAlmacenHeader" runat="server" Text=""></asp:Label>
                    </div>
                </div>

                <div class="row my-2">
                    <div class="col p-1">
                        <asp:DropDownList ID="drpOptions" runat="server" CssClass="form-control btn btn-block btn-info text-left">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row my-2">
                    <div class="col">
                        <asp:Button ID="btnCargaGeneral" runat="server" Text="Completa" CssClass="btn btn-block btn-info" />
                    </div>
                    <div class="col">
                        <asp:Button ID="btnCadena" runat="server" Text="Cadena" CssClass="btn btn-block btn-info" />
                    </div>
                    <div class="col">
                        <asp:Button ID="btnConsigna" runat="server" Text="Dist" CssClass="btn btn-block btn-info" />
                    </div>
                </div>
                <div class="row my-2">
                    <div class="col">
                        <asp:DropDownList ID="drpRutaLogica" runat="server" CssClass="form-control btn btn-block btn-info text-left">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row my-2">
                    <div class="col">
                        <asp:Button ID="btnCargaGeneralRuta" runat="server" Text="Completa" CssClass="btn btn-block btn-secondary" />
                    </div>
                    <div class="col">
                        <asp:Button ID="btnCadenaRuta" runat="server" Text="Cadena" CssClass="btn btn-block btn-secondary" />
                    </div>
                    <div class="col">
                        <asp:Button ID="btnDistRuta" runat="server" Text="Dist" CssClass="btn btn-block btn-secondary" />
                    </div>
                </div>
            </div>
            <div class="col-3">
                <table id="rotacion" class="table table-sm" width="100%">
                    <thead class="thead-dark">
                        <tr>
                            <th>#</th>
                            <th>Actual</th>
                            <th>-30</th>
                            <th>-60</th>
                            <th>-90</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>Stock Unds</td>
                            <td>
                                <asp:Label ID="lbls00" runat="server" Text="0"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbls30" runat="server" Text="0"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbls60" runat="server" Text="0"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbls90" runat="server" Text="0"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Unds. Vend</td>
                            <td>
                                <asp:Label ID="lblv00" runat="server" Text="0"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblv30" runat="server" Text="0"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblv60" runat="server" Text="0"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblv90" runat="server" Text="0"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>% Rotacion</td>
                            <td>
                                <asp:Label ID="lblr00" runat="server" Text="0"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblr30" runat="server" Text="0"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblr60" runat="server" Text="0"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblr90" runat="server" Text="0"></asp:Label>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="col-6">
                <div class="container-fluid">
                    <asp:GridView ID="gridPedidoTemp" runat="server" AutoGenerateColumns="false" CssClass="table table-sm" Width="100%">
                        <HeaderStyle CssClass="thead-dark" />
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="Id" />
                            <asp:BoundField DataField="ARTICULO" HeaderText="Articulo" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="MODELO" HeaderText="Modelo" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="ESPACIOS" HeaderText="Espacios" />
                            <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" />
                            <asp:BoundField DataField="QTYLOGISTICA" HeaderText="Logistica" />
                            <asp:BoundField DataField="OBSERVACIONES" HeaderText="Observaciones" ItemStyle-HorizontalAlign="Left" />
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/delete.png" Text="Eliminar" CommandName="Eliminar" HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ControlStyle Height="30px" Width="30px" />
                                <ItemStyle Wrap="False" />
                            </asp:ButtonField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="container-fluid">
           
        </div>

        <div class="d-flex justify-content-start mt-3">
            <div class="col-4">
                <asp:GridView ID="gridParetoClientes" runat="server" CssClass="table table-sm" AutoGenerateColumns="False" with="100%">
                    <HeaderStyle CssClass="sticky-header thead-dark" />
                    <Columns>
                        <asp:BoundField DataField="Row" HeaderText="" />
                        <asp:BoundField DataField="U_CardCode" HeaderText="Dist" />
                        <asp:BoundField DataField="Sup" HeaderText="Sup" />
                        <asp:BoundField DataField="Monto" HeaderText="Monto" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="ParetoPercent" HeaderText="% Pareto" DataFormatString="{0:p}" ItemStyle-HorizontalAlign="Right" />
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/arrowadd.jpg" Text="Cargar" CommandName="Cargar" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ControlStyle Height="30px" Width="30px" />
                            <ItemStyle Wrap="False" />
                        </asp:ButtonField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="col-8">
                <script src="https://d3js.org/d3.v6.min.js"></script>
                <div class="container-fluid">
                    <asp:GridView ID="gridCuadroBasico" runat="server" AutoGenerateColumns="false" CssClass="table table-sm sticky-grid" Style="width: 100%">
                        <HeaderStyle CssClass="sticky-header thead-dark" />
                        <Columns>
                            <asp:BoundField DataField="ROWID" HeaderText="" />
                            <asp:BoundField DataField="CODE" HeaderText="" />
                            <asp:BoundField DataField="MODELO" HeaderText="Modelo" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="3m" HeaderText="Vta 3m" ItemStyle-CssClass="heatmap-cell1" />
                            <asp:BoundField DataField="6m" HeaderText="Vta 6m" ItemStyle-CssClass="heatmap-cell1" />
                            <asp:BoundField DataField="12m" HeaderText="Vta 12m" ItemStyle-CssClass="heatmap-cell1" />
                            <asp:BoundField DataField="3mp" HeaderText="% 3m" DataFormatString="{0:p}" ItemStyle-CssClass="heatmap-cell2" />
                            <asp:BoundField DataField="6mp" HeaderText="% 6m" DataFormatString="{0:p}" ItemStyle-CssClass="heatmap-cell2" />
                            <asp:BoundField DataField="12mp" HeaderText="% 12m" DataFormatString="{0:p}" ItemStyle-CssClass="heatmap-cell2" />
                            <asp:BoundField DataField="DCM00" HeaderText="Stock" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="CB" HeaderText="CB" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Comprometido" HeaderText="Comp" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Solicitado" HeaderText="Soli" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Transito" HeaderText="Tran" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Fisico" HeaderText="Fisico" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Faltante" HeaderText="Falt" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Actual" HeaderText="Vta.A" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="A" HeaderText="-30" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="B" HeaderText="-60" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="C" HeaderText="-90" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Total" HeaderText="Total" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="UltVenta" HeaderText="UltVenta" />
                        </Columns>
                    </asp:GridView>

                    <%--<div style="overflow-y: auto; height: 1000px; position: relative;">
                        <asp:GridView ID="gridCuadroBasico" runat="server" AutoGenerateColumns="false" CssClass="table table-sm table-fixed-header" Style="width: 100%">
                            <HeaderStyle CssClass="thead-dark sticky-header" />
                            <Columns>
                                <asp:BoundField DataField="ROWID" HeaderText="" />
                                <asp:BoundField DataField="CODE" HeaderText="" />
                                <asp:BoundField DataField="MODELO" HeaderText="Modelo" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="3m" HeaderText="Vta 3m" ItemStyle-CssClass="heatmap-cell1" />
                                <asp:BoundField DataField="6m" HeaderText="Vta 6m" ItemStyle-CssClass="heatmap-cell1" />
                                <asp:BoundField DataField="12m" HeaderText="Vta 12m" ItemStyle-CssClass="heatmap-cell1" />
                                <asp:BoundField DataField="3mp" HeaderText="% 3m" DataFormatString="{0:p}" ItemStyle-CssClass="heatmap-cell2" />
                                <asp:BoundField DataField="6mp" HeaderText="% 6m" DataFormatString="{0:p}" ItemStyle-CssClass="heatmap-cell2" />
                                <asp:BoundField DataField="12mp" HeaderText="% 12m" DataFormatString="{0:p}" ItemStyle-CssClass="heatmap-cell2" />
                                <asp:BoundField DataField="DCM00" HeaderText="Stock" ItemStyle-CssClass="heatmap-cell3" />
                                <asp:BoundField DataField="CB" HeaderText="CB" ItemStyle-CssClass="heatmap-cell3" />
                                <asp:BoundField DataField="Comprometido" HeaderText="Comp" ItemStyle-CssClass="heatmap-cell3" />
                                <asp:BoundField DataField="Solicitado" HeaderText="Soli" ItemStyle-CssClass="heatmap-cell3" />
                                <asp:BoundField DataField="Transito" HeaderText="Tran" ItemStyle-CssClass="heatmap-cell3" />
                                <asp:BoundField DataField="Fisico" HeaderText="Fisico" ItemStyle-CssClass="heatmap-cell3" />
                                <asp:BoundField DataField="Faltante" HeaderText="Falt" ItemStyle-CssClass="heatmap-cell3" />
                                <asp:BoundField DataField="Actual" HeaderText="Vta.A" ItemStyle-CssClass="heatmap-cell3" />
                                <asp:BoundField DataField="A" HeaderText="-30" ItemStyle-CssClass="heatmap-cell3" />
                                <asp:BoundField DataField="B" HeaderText="-60" ItemStyle-CssClass="heatmap-cell3" />
                                <asp:BoundField DataField="C" HeaderText="-90" ItemStyle-CssClass="heatmap-cell3" />
                                <asp:BoundField DataField="Total" HeaderText="Total" ItemStyle-CssClass="heatmap-cell3" />
                                <asp:BoundField DataField="UltVenta" HeaderText="UltVenta" />
                                <asp:BoundField DataField="Logistica" HeaderText="L" />
                                <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/bike.png" Text="S" CommandName="Sugerir" HeaderText="S">
                                    <ControlStyle Height="30px" Width="30" />
                                    <ItemStyle Wrap="False" />
                                </asp:ButtonField>
                            </Columns>
                        </asp:GridView>
                    </div>--%>

                    <%-- <asp:GridView ID="gridCuadroBasico" runat="server" AutoGenerateColumns="false" CssClass="table table-sm" Style="width: 100%">
                        <HeaderStyle CssClass="thead-dark sticky-header" />
                        <Columns>
                            <asp:BoundField DataField="ROWID" HeaderText="" />
                            <asp:BoundField DataField="CODE" HeaderText="" />
                            <asp:BoundField DataField="MODELO" HeaderText="Modelo" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="3m" HeaderText="Vta 3m" ItemStyle-CssClass="heatmap-cell1" />
                            <asp:BoundField DataField="6m" HeaderText="Vta 6m" ItemStyle-CssClass="heatmap-cell1" />
                            <asp:BoundField DataField="12m" HeaderText="Vta 12m" ItemStyle-CssClass="heatmap-cell1" />
                            <asp:BoundField DataField="3mp" HeaderText="% 3m" DataFormatString="{0:p}" ItemStyle-CssClass="heatmap-cell2" />
                            <asp:BoundField DataField="6mp" HeaderText="% 6m" DataFormatString="{0:p}" ItemStyle-CssClass="heatmap-cell2" />
                            <asp:BoundField DataField="12mp" HeaderText="% 12m" DataFormatString="{0:p}" ItemStyle-CssClass="heatmap-cell2" />
                            <asp:BoundField DataField="DCM00" HeaderText="Stock" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="CB" HeaderText="CB" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Comprometido" HeaderText="Comp" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Solicitado" HeaderText="Soli" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Transito" HeaderText="Tran" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Fisico" HeaderText="Fisico" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Faltante" HeaderText="Falt" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Actual" HeaderText="Vta.A" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="A" HeaderText="-30" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="B" HeaderText="-60" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="C" HeaderText="-90" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="Total" HeaderText="Total" ItemStyle-CssClass="heatmap-cell3" />
                            <asp:BoundField DataField="UltVenta" HeaderText="UltVenta" />
                            <asp:BoundField DataField="Logistica" HeaderText="L" />
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/bike.png" Text="S" CommandName="Sugerir" HeaderText="S">
                                <ControlStyle Height="30px" Width="30" />
                                <ItemStyle Wrap="False" />
                            </asp:ButtonField>
                        </Columns>
                    </asp:GridView>--%>
                </div>
                <style>
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

        </div>
        <div class="d-flex justify-content-start mt-3">
            <div class="row">
                <div class="col">
                    <b>Canal</b>
                    <br />
                    <asp:Label ID="lblCanal" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <b>Supervisor</b>
                    <br />
                    <asp:Label ID="lblSupervisor" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <b>Codigo</b>
                    <br />
                    <asp:Label ID="lblAlmacen" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <b>Whscode</b>
                    <br />
                    <asp:Label ID="lblWhscode" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <b>Almacen</b>
                    <br />
                    <asp:Label ID="lblNombreAlmacen" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <b>Correo</b>
                    <br />
                    <asp:Label ID="lblCorreo" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <b>RowIndex</b>
                    <br />
                    <asp:Label ID="lblRowIndex" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <b>Ruta</b>
                    <br />
                    <asp:Label ID="lblRutaWhs" runat="server"></asp:Label>
                </div>
            </div>
        </div>
        <%--<div class="d-flex justify-content-center">
        </div>
        <div class="d-flex justify-content-center">
            <div class="col-2"></div>
        </div>--%>
        <!-- Inicio Modal Agregar Colores Moto-->
        <div class="modal fade" id="ColoresMotos" tabindex="-1" role="dialog" aria-labelledby="ColoresMotosLabel"
            aria-hidden="true">
            <div class="modal-dialog modal-xl" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="ColoresMotosLabel">Seleccionar Modelos y Colores a Enviar</h5>
                        <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">×</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <asp:Label ID="lblruta" runat="server" Text=""></asp:Label>
                        |
                        <asp:Label ID="lblcardcode" runat="server" Text=""></asp:Label>
                        |
                        <asp:Label ID="lblAlmOrigen" runat="server" Text="DCM00"></asp:Label>
                        |
                        <asp:Label ID="lblAlmDestino" runat="server" Text=""></asp:Label>
                        |
                        <asp:Label ID="lblSugerido" runat="server" Text="SUGERIDO"></asp:Label>
                        <hr />
                        <asp:GridView ID="gridColoresMoto" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" Width="100%">
                            <HeaderStyle CssClass="thead-dark" />
                            <Columns>
                                <asp:BoundField DataField="itemcode" HeaderText="Codigo" />
                                <asp:BoundField DataField="itemname" HeaderText="Descripcion" />
                                <asp:BoundField DataField="Modelo" HeaderText="Modelo" />
                                <asp:BoundField DataField="Espacios" HeaderText="Espacios" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="CEDIS" HeaderText="DCM00" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="SUCURSAL" HeaderText="Sucursal" ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderText="Cant">
                                    <ItemTemplate>
                                        <asp:TextBox ID="qty" runat="server" ClientIDMode="Static" Width="50px" Text="0" CssClass="txt_values text-right" type="Number" min="0"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnCancelColoresMotos" runat="server" class="btn btn-secondary" Text="Cancelar" />
                        <asp:Button ID="btnModalColoresMotos" runat="server" class="btn btn-primary" Text="Agregar" />
                    </div>
                </div>
            </div>
        </div>
        <!-- Fin Modal Agregar Colores Moto-->

        <asp:ScriptManager ID="sm1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

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


        <!-- DataTables & Plugins -->
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
        <script src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
        <script src="https://cdn.datatables.net/buttons/1.7.1/js/dataTables.buttons.min.js"></script>
        <script src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.html5.min.js"></script>
        <script src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.print.min.js"></script>
        <script src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.colVis.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
        <link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />
        <link href="https://cdn.datatables.net/buttons/1.7.1/css/buttons.dataTables.min.css" rel="stylesheet" type="text/css" />
        <script src="https://cdn.datatables.net/responsive/2.5.0/js/dataTables.responsive.min.js"></script>

        <script type="text/javascript">
            $(function () {
                $("[id*=gridParetoClientes]").DataTable({
                    dom: 'Bfrtip',
                    buttons: [
                        'excelHtml5'
                    ],
                    responsive: false,
                    bLengthChange: true,
                    lengthMenu: [[10, -1], [10, "All"]],
                    bFilter: true,
                    bSort: true,
                    bPaginate: false,
                    order: [[0, "asc"]]
                });
            });
            $(function () {
                $("[id*=gridCuadroBasico]").DataTable({
                    dom: 'Bfrtip',
                    buttons: [
                        'excelHtml5'
                    ],
                    responsive: false,
                    bLengthChange: true,
                    lengthMenu: [[10, -1], [10, "All"]],
                    bFilter: true,
                    bSort: true,
                    bPaginate: false,
                    order: [[0, "asc"]]
                });
            });
        </script>


        <link href="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/css/toastr.min.css" rel="stylesheet" />
        <script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js"></script>
        <script type="text/javascript">
            function showContent(typeofMsg, mensaje) {
                toastr.options = {
                    "closeButton": true,
                    "debug": false,
                    "progressBar": true,
                    "preventDuplicates": false,
                    "positionClass": "toast-top-right",
                    "showDuration": "400",
                    "hideDuration": "1000",
                    "timeOut": "7000",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                }
                toastr[typeofMsg](mensaje);
            }
        </script>
    </form>
</body>
</html>
<script type="text/javascript">

    function convertToUppercase(elementId) {
        var textBox = document.getElementById(elementId);
        textBox.value = textBox.value.toUpperCase();
    }

    $(document).ready(function () {
        $("#btnNotificar").prop("enabled", true);


        var x = $('#<%=txtEstatusCliente.ClientID%>').val();

        var creditline = parseFloat($('#<%=txtLimiteCredito.ClientID%>').val().replace(/,/g, ''));
        var saldogeneral = parseFloat($('#<%=txtSaldoCuenta.ClientID%>').val().replace(/,/g, ''));
        var saldoconsigna = parseFloat($('#<%=txtSaldoConsignacion.ClientID%>').val().replace(/,/g, ''));


        if (x == 'BLOQUEADO') {
            $("#btnNotificar").prop("disabled", true);
            iziToast.warning({ title: 'Alerta!', message: 'Cliente Bloqueado en SAP, Consulte con Creditos!!!', position: 'topRight', timeout: 5000 });
        }

        if ((saldogeneral + saldoconsigna) > creditline) {
            $("#btnNotificar").prop("disabled", true);
            iziToast.warning({ title: 'Alerta!', message: 'Cliente Excede su Linea de Creditos, Consulte con Creditos!!!', position: 'topRight', timeout: 5000 });
        }
    });
</script>
