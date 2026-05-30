<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AuditoriaMotoReingreso.aspx.vb" Inherits="AuditoriaMotoReingreso" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Auditoria Moto Reingreso</title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.3.0/css/all.min.css">
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
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/2.9.2/umd/popper.min.js" integrity="sha512-2rNj2KJ+D8s1ceNasTIex6z4HWyOnEYLVC3FigGOmyQCZc2eBXKgOxQmo3oKLHyfcj53uz4QMsRCWNbLd32Q1g==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/dropzone/5.9.3/dropzone.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/dropzone/5.9.3/dropzone.min.js"></script>
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/responsive/2.5.0/css/responsive.dataTables.min.css" />

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/toastify-js/src/toastify.min.css">
    <script src="https://cdn.jsdelivr.net/npm/toastify-js"></script>


    <style>
        .toast {
            position: fixed;
            bottom: 20px;
            right: 20px;
            padding: 15px;
            background-color: #28a745;
            color: #fff;
            border-radius: 5px;
            opacity: 0.9;
            font-size: 16px;
            z-index: 1000;
        }

            .toast.error {
                background-color: #dc3545;
            }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark static-top">
            <div class="container">
                <a class="navbar-brand" href="MainDashBoard.aspx">
                    <img src="Imagenes/mnegra.png" width="50" height="40" alt="" />
                </a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarSupportedContent">
                    <ul class="navbar-nav ms-auto">
                        <li class="nav-item">
                            <a class="nav-link" href="DashboardAuditoria.aspx">Inicio</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="AuditoriaInventarioArmadas.aspx">Inventario Motos Armadas</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#">Auditoria CC</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="AuditoriaInformeInventario.aspx">Informes</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="Default.aspx">Cerrar Sesion</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
        <div class="container-fluid px-4">
            <h1 class="mt-4">Auditoria Moto Retornada</h1>
            <div class="row">
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12 py-2">
                    <label for="txtSnMoto">Digite Número de Serie</label>
                    <asp:TextBox ID="txtSnMoto" runat="server" class="form-control" autocomplete="off" placeholder="Digite el Numero de Serie"></asp:TextBox>
                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServiceMethod="SearchCustomers"
                        MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
                        TargetControlID="txtSnMoto" FirstRowSelected="false">
                    </cc1:AutoCompleteExtender>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12 py-2">
                    <asp:Button ID="btnBusarSerie" runat="server" Text="Buscar Serie" CssClass="btn btn-info w-50 h-100" />
                </div>
            </div>
            <h2 class="my-3">Verificación de Datos</h2>
            <div class="row">
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <label for="txtCodigo">Codigo Moto</label>
                    <asp:TextBox ID="txtCodigo" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-lg-8 col-md-12 col-12 py-3">
                    <label for="txtDescripcion">Descripcion Moto</label>
                    <asp:TextBox ID="txtDescripcion" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <label for="txtSeriemoto">Serie Moto Seleccionada</label>
                    <asp:TextBox ID="txtSeriemoto" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <label for="txtSeriemotor">Serie Motor</label>
                    <asp:TextBox ID="txtSeriemotor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <label for="txtMarca">Marca Moto</label>
                    <asp:TextBox ID="txtMarca" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <label for="txtModelo">Modelo Moto</label>
                    <asp:TextBox ID="txtModelo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <label for="txtCilindros">Cilindraje Moto</label>
                    <asp:TextBox ID="txtCilindros" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <label for="txtColor">Color Moto</label>
                    <asp:TextBox ID="txtColor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <label for="txtALmacenSAP">Almacen SAP</label>
                    <asp:TextBox ID="txtALmacenSAP" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <label for="txtEstatusSerieSAP">Estatus Serie SAP</label>
                    <asp:TextBox ID="txtEstatusSerieSAP" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <label for="txtYears">Año Moto</label>
                    <asp:TextBox ID="txtYear" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <label for="txtArmado">Días Armado</label>
                    <asp:TextBox ID="txtArmado" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-12 col-sm-12 col-12  py-3">
                    <label for="txtAgeing">Días Antigüedad</label>
                    <asp:TextBox ID="txtAgeing" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-12 py-3">
                <h3 class="text-center">Subir Imágenes</h3>
                <div id="dropzoneForm" class="dropzone h-100">
                    <div class="dz-message">
                        Arrastra las imágenes aquí o haz clic para subir.
                    </div>
                </div>
            </div>
        </div>
        <hr />
        <div class="row">
                <div class="ui container mt-5" id="frm-main">

        <!-- Step Content -->
        <div id="step-1" class="step-content active-content">
            <h4>Inspección Visual Exterior</h4>
            <p>Revisar el estado exterior del vehículo.</p>
            <hr>
            <h3>Estado General del Chasis</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado General del Chasis" id="Excelente">
                <label class="form-check-label" for="Excelente">
                    Excelente
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado General del Chasis" id="Regular">
                <label class="form-check-label" for="Regular">
                    Regular
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado General del Chasis" id="Malo">
                <label class="form-check-label" for="Malo">
                    Malo
                </label>
            </div>
            <hr>
            <h3>Estado de La Pintura</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado de La Pintura" id="Sin Rayones ni Desgaste">
                <label class="form-check-label" for="Sin Rayones ni Desgaste">
                    Sin Rayones ni Desgaste
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado de La Pintura" id="Pequeños Rayones">
                <label class="form-check-label" for="Pequeños Rayones">
                    Pequeños Rayones
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado de La Pintura" id="Desgaste Notable">
                <label class="form-check-label" for="Desgaste Notable">
                    Desgaste Notable
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado de La Pintura" id="Daños Graves">
                <label class="form-check-label" for="Daños Graves">
                    Daños Graves
                </label>
            </div>
            <hr>
            <h3>Estado del Asiento</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado del Asiento" id="Sin Daños">
                <label class="form-check-label" for="Sin Daños">
                    Sin Daños
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado del Asiento" id="Desgaste Leve">
                <label class="form-check-label" for="Desgaste Leve">
                    Desgaste Leve
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado del Asiento" id="Desgaste Significativo">
                <label class="form-check-label" for="Desgaste Significativo">
                    Desgaste Significativo
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado del Asiento" id="Necesita Reemplazo">
                <label class="form-check-label" for="Necesita Reemplazo">
                    Necesita Reemplazo
                </label>
            </div>
            <hr>
            <h3>Condición de las Ruedas</h3>
            <h4>Delantera(s)</h4>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Condición de las Ruedas Delantera" id="Delantera Buena Condición">
                <label class="form-check-label" for="Delantera Buena Condición">
                    Buena Condición
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Condición de las Ruedas Delantera" id="Delantera Desgastada">
                <label class="form-check-label" for="Delantera Desgastada">
                    Desgastada
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Condición de las Ruedas Delantera" id="Delantera LLanta Cortada">
                <label class="form-check-label" for="Delantera LLanta Cortada">
                    LLanta Cortada
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Condición de las Ruedas Delantera" id="Delantera Llanta Punchada">
                <label class="form-check-label" for="Delantera Llanta Punchada">
                    Llanta Punchada
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Condición de las Ruedas Delantera" id="Delantera Llanta Hinchada">
                <label class="form-check-label" for="Delantera Llanta Hinchada">
                    Llanta Hinchada
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Condición de las Ruedas Delantera" id="Delantera Necesita Reemplazo">
                <label class="form-check-label" for="Delantera Necesita Reemplazo">
                    Necesita Reemplazo
                </label>
            </div>
            <h4>Trasera(s)</h4>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Condición de las Ruedas Trasera" id="Trasera Buena Condición">
                <label class="form-check-label" for="Trasera Buena Condición">
                    Buena Condición
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Condición de las Ruedas Trasera" id="Trasera Desgastada">
                <label class="form-check-label" for="Trasera Desgastada">
                    Desgastada
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Condición de las Ruedas Trasera" id="Trasera LLanta Cortada">
                <label class="form-check-label" for="Trasera LLanta Cortada">
                    LLanta Cortada
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Condición de las Ruedas Trasera" id="Trasera Llanta Punchada">
                <label class="form-check-label" for="Trasera Llanta Punchada">
                    Llanta Punchada
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Condición de las Ruedas Trasera" id="Trasera Llanta Hinchada">
                <label class="form-check-label" for="Trasera Llanta Hinchada">
                    Llanta Hinchada
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Condición de las Ruedas Trasera" id="Trasera Necesita Reemplazo">
                <label class="form-check-label" for="Trasera Necesita Reemplazo">
                    Necesita Reemplazo
                </label>
            </div>
            <hr>
            <h3>Estado de los Guardafangos</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado de los Guardafangos" id="Guardafango Sin Daños">
                <label class="form-check-label" for="Guardafango Sin Daños">
                    Sin Daños
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado de los Guardafangos" id="Guardafango Rayado">
                <label class="form-check-label" for="Guardafango Rayado">
                    Rayado
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado de los Guardafangos" id="Guardafango Quebrado">
                <label class="form-check-label" for="Guardafango Quebrado">
                    Quebrado
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado de los Guardafangos" id="Guardafango Necesita Reemplazo">
                <label class="form-check-label" for="Guardafango Necesita Reemplazo">
                    Necesita Reemplazo
                </label>
            </div>
        </div>
        <div id="step-2" class="step-content">
            <h4>Inspección de Componentes Mecánicos</h4>
            <p>Revisar los componentes mecánicos.</p>
            <hr>
            <h3>Motor</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Motor" id="Enciende Sin Problemas">
                <label class="form-check-label" for="Enciende Sin Problemas">
                    Enciende Sin Problemas
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Motor" id="Falla al Encender">
                <label class="form-check-label" for="Falla al Encender">
                    Falla al Encender
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Motor" id="Ruidos Extraños">
                <label class="form-check-label" for="Ruidos Extraños">
                    Ruidos Extraños
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Motor" id="Fugas de Aceite">
                <label class="form-check-label" for="Fugas de Aceite">
                    Fugas de Aceite
                </label>
            </div>
            <hr>
            <h3>Estado del Aceite</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado del Aceite" id="Nivel Correcto">
                <label class="form-check-label" for="Nivel Correcto">
                    Nivel Correcto
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado del Aceite" id="Bajo Nivel">
                <label class="form-check-label" for="Bajo Nivel">
                    Bajo Nivel
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado del Aceite" id="Aceite Sucio">
                <label class="form-check-label" for="Aceite Sucio">
                    Aceite Sucio
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado del Aceite" id="Necesita Cambio">
                <label class="form-check-label" for="Necesita Cambio">
                    Necesita Cambio
                </label>
            </div>
            <hr>

            <h3>Sistema de Transmisión</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Sistema de Transmisión" id="Transmisión Funciona Correctamente">
                <label class="form-check-label" for="Transmisión Funciona Correctamente">
                    Transmisión Funciona Correctamente
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Sistema de Transmisión" id="Transmisión Problemas Menores">
                <label class="form-check-label" for="Transmisión Problemas Menores">
                    Transmisión Problemas Menores
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Sistema de Transmisión" id="Transmision Desgaste Cadena">
                <label class="form-check-label" for="Transmision Desgaste Cadena">
                    Transmision Desgaste Cadena
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Sistema de Transmisión" id="Transmision Necesita Reparación">
                <label class="form-check-label" for="Transmision Necesita Reparación">
                    Transmision Necesita Reparación
                </label>
            </div>
            <hr>
            <h3>Sistema de Refrigeracion *(Si Aplica)</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Sistema Refrigeracion" id="Refrigeracion No Aplica">
                <label class="form-check-label" for="Refrigeracion No Aplica">
                    Refrigeracion No Aplica
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Sistema Refrigeracion" id="Refrigeracion Funciona Correctamente">
                <label class="form-check-label" for="Refrigeracion Funciona Correctamente">
                    Refrigeracion Funciona Correctamente
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Sistema Refrigeracion" id="Refrigeracion Tiene Fugas">
                <label class="form-check-label" for="Refrigeracion Tiene Fugas">
                    Refrigeracion Tiene Fugas
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Sistema Refrigeracion" id="Sobrecalentamiento">
                <label class="form-check-label" for="Sobrecalentamiento">
                    Sobrecalentamiento
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Sistema Refrigeracion" id="Nivel Refrigerante Bajo">
                <label class="form-check-label" for="Nivel Refrigerante Bajo">
                    Nivel Refrigerante Bajo
                </label>
            </div>
            <hr>
            <h3>Sistema de Escape</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Sistema de Escape" id="Sin Fugas">
                <label class="form-check-label" for="Sin Fugas">
                    Sin Fugas
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Sistema de Escape" id="Sello Escape Dañado">
                <label class="form-check-label" for="Sello Escape Dañado">
                    Sello Escape Dañado
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Sistema de Escape" id="Escape Golpeado">
                <label class="form-check-label" for="Escape Golpeado">
                    Escape Golpeado
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Sistema de Escape" id="Daños en el Silenciador">
                <label class="form-check-label" for="Daños en el Silenciador">
                    Daños en el Silenciador
                </label>
            </div>
        </div>
        <div id="step-3" class="step-content">
            <h4>Inspección de Componentes Eléctricos</h4>
            <p>Revisar el sistema eléctrico.</p>
            <hr>
            <h3>Luces Delanteras</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Luces Delanteras" id="Luz Delantera Funciona Correctamente">
                <label class="form-check-label" for="Luz Delantera Funciona Correctamente">
                    Luz Delantera Funciona Correctamente
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Luces Delanteras" id="Luz Delantera Tiene Falla en 1 Luz">
                <label class="form-check-label" for="Luz Delantera Tiene Falla en 1 Luz">
                    Luz Delantera Tiene Falla en 1 Luz
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Luces Delanteras" id="Luz delantera No Funcionan">
                <label class="form-check-label" for="Luz delantera No Funcionan">
                    Luz Delantera No Funcionan
                </label>
            </div>
            <hr>
            <h3>Luces Traseras</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Luces Traseras" id="Luz Trasera Funciona Correctamente">
                <label class="form-check-label" for="Luz Trasera Funciona Correctamente">
                    Luz Trasera Funciona Correctamente
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Luces Traseras" id="Luz Trasera Tiene Falla en 1 Luz">
                <label class="form-check-label" for="Luz Trasera Tiene Falla en 1 Luz">
                    Luz Trasera Tiene Falla en 1 Luz
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Luces Traseras" id="Luz Trasera No Funcionan">
                <label class="form-check-label" for="Luz Trasera No Funcionan">
                    Luz Trasera No Funcionan
                </label>
            </div>
            <hr>
            <h3>Luces Direccionales</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Luces Direccionales" id="Luz Direccional Funciona Correctamente">
                <label class="form-check-label" for="Luz Direccional Funciona Correctamente">
                    Luz Direccional Funciona Correctamente
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Luces Direccionales" id="Luz Direccional Tiene Falla en 1 Luz">
                <label class="form-check-label" for="Luz Direccional Tiene Falla en 1 Luz">
                    Luz Direccional Tiene Falla en 1 Luz
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Luces Direccionales" id="Luz Direccional No Funcionan">
                <label class="form-check-label" for="Luz Direccional No Funcionan">
                    Luz Direccional No Funcionan
                </label>
            </div>
            <hr>
            <h3>Luces de Frenos</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Luces de Frenos" id="Luz Frenos Funciona Correctamente">
                <label class="form-check-label" for="Luz Frenos Funciona Correctamente">
                    Luz Frenos Funciona Correctamente
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Luces de Frenos" id="Luz Frenos Tiene Falla en 1 Luz">
                <label class="form-check-label" for="Luz Frenos Tiene Falla en 1 Luz">
                    Luz Frenos Tiene Falla en 1 Luz
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Luces de Frenos" id="Luz Frenos No Funcionan">
                <label class="form-check-label" for="Luz Frenos No Funcionan">
                    Luz Frenos No Funcionan
                </label>
            </div>
            <hr>
            <h3>Bateria</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Bateria" id="Bateria en Buen Estado">
                <label class="form-check-label" for="Bateria en Buen Estado">
                    Bateria en Buen Estado
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Bateria" id="Bateria Carga Baja">
                <label class="form-check-label" for="Bateria Carga Baja">
                    Bateria Carga Baja
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Bateria" id="Requiere Reemplazo">
                <label class="form-check-label" for="Requiere Reemplazo">
                    Requiere Reemplazo
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Bateria" id="Fugas de Acido">
                <label class="form-check-label" for="Fugas de Acido">
                    Fugas de Acido
                </label>
            </div>
            <hr>
            <h3>Claxon / Pito</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Claxon / Pito" id="Claxon Funciona Correctamente">
                <label class="form-check-label" for="Claxon Funciona Correctamente">
                    Claxon Funciona Correctamente
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Claxon / Pito" id="Claxon No Funciona">
                <label class="form-check-label" for="Claxon No Funciona">
                    Claxon No Funciona
                </label>
            </div>
        </div>
        <div id="step-4" class="step-content">
            <h4>Inspección de Seguridad</h4>
            <p>Revisar los sistemas de seguridad.</p>
            <hr>
            <h3>Espejos Retrovisores</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Espejos Retrovisores" id="Espejos sin Daños">
                <label class="form-check-label" for="Espejos sin Daños">
                    Espejos sin Daños
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Espejos Retrovisores" id="Espejos Necesitan Reemplazo">
                <label class="form-check-label" for="Espejos Necesitan Reemplazo">
                    Espejos Necesitan Reemplazo
                </label>
            </div>
            <hr>
            <h3>Frenos</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Frenos" id="Cable Dañado">
                <label class="form-check-label" for="Cable Dañado">
                    Cable Dañado
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Frenos" id="Freno Alto">
                <label class="form-check-label" for="Freno Alto">
                    Freno Alto
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Frenos" id="Freno Bajo">
                <label class="form-check-label" for="Freno Bajo">
                    Freno Bajo
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Frenos" id="Freno Con Fallas">
                <label class="form-check-label" for="Freno Con Fallas">
                    Freno Con Fallas
                </label>
            </div>
            <hr>
            <h3>Pedales</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Pedales" id="Pedales Sin Daño">
                <label class="form-check-label" for="Pedales Sin Daño">
                    Pedales Sin Daño
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Pedales" id="Pedales con Desgaste Leve">
                <label class="form-check-label" for="Pedales con Desgaste Leve">
                    Pedales con Desgaste Leve
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Pedales" id="Pedales Con Desgaste Significativo">
                <label class="form-check-label" for="Pedales Con Desgaste Significativo">
                    Pedales Con Desgaste Significativo
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Pedales" id="Pedales Necesitan Reemplazo">
                <label class="form-check-label" for="Pedales Necesitan Reemplazo">
                    Pedales Necesitan Reemplazo
                </label>
            </div>
        </div>
        <div id="step-5" class="step-content">
            <h4>Prueba de Manejo</h4>
            <p>Realizar la prueba de manejo del vehículo.</p>
            <hr>
            <h3>Funcionamiento Motor</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Funcionamiento Motor" id="Motor Optimo">
                <label class="form-check-label" for="Motor Optimo">
                    Motor Optimo
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Funcionamiento Motor" id="Motor Con Ruidos Anormales">
                <label class="form-check-label" for="Motor Con Ruidos Anormales">
                    Motor Con Ruidos Anormales
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Funcionamiento Motor" id="Vibracion Excesiva">
                <label class="form-check-label" for="Vibracion Excesiva">
                    Vibracion Excesiva
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Funcionamiento Motor" id="Perdida de Potencia">
                <label class="form-check-label" for="Perdida de Potencia">
                    Perdida de Potencia
                </label>
            </div>
            <hr>
            <h3>Funcionamiento Transmision</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Funcionamiento Transmision" id="Cambios Suaves">
                <label class="form-check-label" for="Cambios Suaves">
                    Cambios Suaves
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Funcionamiento Transmision" id="Cambios Duros">
                <label class="form-check-label" for="Cambios Duros">
                    Cambios Duros
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Funcionamiento Transmision" id="Problemas de Embrague">
                <label class="form-check-label" for="Problemas de Embrague">
                    Problemas de Embrague
                </label>
            </div>
            <hr>
            <h3>Funcionamiento Frenos</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Funcionamiento Frenos" id="Frenos Manejo Optimo">
                <label class="form-check-label" for="Frenos Manejo Optimo">
                    Frenos Manejo Optimo
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Funcionamiento Frenos" id="Frenos Ruido al Frenar">
                <label class="form-check-label" for="Frenos Ruido al Frenar">
                    Frenos Ruido al Frenar
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Funcionamiento Frenos" id="Frenos Desgaste Pastilla">
                <label class="form-check-label" for="Frenos Desgaste Pastilla">
                    Frenos Desgaste Pastilla
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Funcionamiento Frenos" id="Frenos Fugas en el Sistema">
                <label class="form-check-label" for="Frenos Fugas en el Sistema">
                    Frenos Fugas en el Sistema
                </label>
            </div>
            <hr>
            <h3>Estabilidad y Suspension</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estabilidad y Suspension" id="Estabilidad Optima">
                <label class="form-check-label" for="Estabilidad Optima">
                    Estabilidad Optima
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estabilidad y Suspension" id="Vibraciones">
                <label class="form-check-label" for="Vibraciones">
                    Vibraciones
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estabilidad y Suspension" id="Inestabilidad al girar">
                <label class="form-check-label" for="Inestabilidad al girar">
                    Inestabilidad al girar
                </label>
            </div>
        </div>
        <div id="step-6" class="step-content">
            <h4>Observaciones Generales</h4>
            <p>Notas y observaciones finales.</p>
            <h3>Comentarios Adicionales</h3>
            <div class="form-check">
                <textarea name="ObservacionesG" id="ObservacionesG" class="form-control" rows="5" style="width: 100%;"></textarea>
                <script>
                    document.getElementById('ObservacionesG').addEventListener('input', function () {
                        this.value = this.value.toUpperCase();
                    });
                </script>
                <!-- <textarea name="ObservacionesG" id="ObservacionesG" class="form-control" rows="5" style="width: 100%;"></textarea> -->
            </div>
        </div>
                    <br />
        <div id="step-7" class="step-content">
            <h4>Resultado de la Auditoría</h4>
            <p>Resumir los hallazgos de la auditoría.</p>
            <hr>
            <h3>Estado Final Vehiculo</h3>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado Final Vehiculo" id="Vehiculo Aprobado">
                <label class="form-check-label" for="Vehiculo Aprobado">
                    Vehiculo Aprobado
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado Final Vehiculo" id="Vehiculo Aprobado Con Observaciones">
                <label class="form-check-label" for="Vehiculo Aprobado Con Observaciones">
                    Vehiculo Aprobado Con Observaciones
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado Final Vehiculo" id="Vehiculo Requiere Reparacion">
                <label class="form-check-label" for="Vehiculo Requiere Reparacion">
                    Vehiculo Requiere Reparacion
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado Final Vehiculo" id="Vehiculo No Aprobado">
                <label class="form-check-label" for="Vehiculo No Aprobado">
                    Vehiculo No Aprobado
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado Final Vehiculo" id="Vehiculo aprobado con detalles de pintura">
                <label class="form-check-label" for="Vehiculo aprobado con detalles de pintura">
                    Vehiculo aprobado con detalles de pintura
                </label>
            </div>
            <div class="form-check">
                <input class="form-check-input" type="radio" name="Estado Final Vehiculo" id="Inhabilitar Vehiculo">
                <label class="form-check-label" for="Inhabilitar Vehiculo">
                    Inhabilitar Vehiculo
                </label>
            </div>
        </div>
    </div>

        </div>
        <hr />
        <div class="row">
            <div class="col-12 py-3">
                <button type="button" class="btn btn-dark mt-3" onclick="sendJSONToHandler()">Guardar Datos</button>
            </div>
        </div>
        <asp:ScriptManager ID="sm1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    </form>
</body>
</html>
<!-- jQuery -->
<script src="plugins/jquery/jquery.min.js"></script>
<!-- jQuery UI 1.11.4 -->
<script src="plugins/jquery-ui/jquery-ui.min.js"></script>
<!-- Resolve conflict in jQuery UI tooltip with Bootstrap tooltip -->
<script>
    $.widget.bridge('uibutton', $.ui.button)
</script>
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

<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
<link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />
<script src="https://cdn.datatables.net/responsive/2.5.0/js/dataTables.responsive.min.js"></script>

<script type="text/javascript">
    $(function () {
        $("[id*=gridKardex]").DataTable(
            {
                bLengthChange: true,
                lengthMenu: [[10, -1], [10, "All"]],
                bFilter: true,
                bSort: false,
                bPaginate: false,
                responsive: true
            });
    });
</script>


<script>
    Dropzone.autoDiscover = false;
    var myDropzone = new Dropzone("#dropzoneForm", {
        url: "UploadHandlerReingreso.ashx",
        paramName: "file",
        maxFilesize: 10,
        init: function () {
            this.on("sending", function (file, xhr, formData) {
                let serieMoto = $('#txtSeriemoto').val();
                formData.append("serieMoto", serieMoto);
                console.log(serieMoto);
            });
        }
    });

    function generateJSON() {
        const radios = document.querySelectorAll('input[type="radio"]');
        const jsonResult = {};

        radios.forEach(radio => {
            if (radio.checked) {
                const name = radio.name;
                const id = radio.id;
                if (!jsonResult[name]) {
                    jsonResult[name] = id;
                }
            }
        });

        const jsonData = {
            serie: $("#txtSeriemoto").val(),
            itemcode: $("#txtCodigo").val(),
            itemname: $("#txtDescripcion").val(),
            seriemotor: $("#txtSeriemotor").val(),
            marca: $("#txtMarca").val(),
            modelo: $("#txtModelo").val(),
            cilindros: $("#txtCilindros").val(),
            color: $("#txtColor").val(),
            almacen: $("#txtALmacenSAP").val(),
            estatus_serie: $("#txtEstatusSerieSAP").val(),
            yearmoto: $("#txtYear").val(),
            dias_armado: $("#txtArmado").val(),
            ageing: $("#txtAgeing").val(),
            comentarios: $("#ObservacionesG").val(),
            data: jsonResult
        };

        console.log("JSON generado:", JSON.stringify(jsonData, null, 4));
        return jsonData;
    }

    function sendJSONToHandler() {
        const jsonData = generateJSON();

        $.ajax({
            url: 'InventarioHandlerReingreso.ashx',
            type: 'POST',
            data: JSON.stringify(jsonData),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (response) {
                console.log("Respuesta del servidor:", response);
                showToast("Datos grabados exitosamente.");
                setTimeout(function () {
                    window.location.href = "AuditoriaMotoReingreso.aspx";
                }, 5000);
            },
            error: function (xhr, status, error) {
                console.error("Error al enviar los datos:", error);
                showToast("Hubo un error al enviar los datos.", "error");
            }
        });
    }

    function showToast(message, type = "success") {
        const toast = document.createElement("div");
        toast.className = `toast ${type}`;
        toast.innerText = message;
        document.body.appendChild(toast);
        setTimeout(function () {
            toast.remove();
        }, 3000);
    }

    function handleGenerateJSON() {
        const jsonData = generateJSON();
        console.log("Resultado final de generateJSON:", jsonData);
    }
</script>

