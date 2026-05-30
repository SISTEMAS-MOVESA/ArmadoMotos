<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Grafico.aspx.vb" Inherits="Grafico" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Quiebres de Inventario</title>

    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/4.6.0/css/bootstrap.min.css">

    <!-- Bootstrap Multiselect CSS -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.15/css/bootstrap-multiselect.css">


    <style>
        #myChart {
            width: auto !important;
            height: 750px !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Quiebres de Inventario</h2>
        <label for="ddlMes">Mes:</label>
        <select id="ddlMes">
            <option value="1">Enero</option>
            <option value="2">Febrero</option>
            <option value="3">Marzo</option>
            <option value="4">Abril</option>
            <option value="5">Mayo</option>
            <option value="6">Junio</option>
            <option value="7">Julio</option>
            <option value="8">Agosto</option>
            <option value="9">Septiembre</option>
            <option value="10">Octubre</option>
            <option value="11">Noviembre</option>
            <option value="12">Diciembre</option>
        </select>
        <label for="ddlAnio">Año:</label>
        <select id="ddlAnio">
            <option value="2024">2024</option>
            <option value="2025">2025</option>
        </select>
        <label for="ddlAlmacen">Almacén:</label>
        <select id="ddlAlmacen"></select>
        <%--<input type="text" id="ddlAlmacen" />--%>
        <button id="btnFiltrar" type="button">Filtrar</button>

        <div style="display: flex; align-content: initial;">
            <div class="div1 mx-3">
                <label for="ddlMarca">Seleccionar Marca</label>
                <select class="form-control" id="ddlMarca" multiple="multiple"></select>
            </div>
            <div class="div2 mx-3">
                <label for="ddlModelo">Seleccionar Modelo</label>
                <select class="form-control" id="ddlModelo" multiple="multiple"></select>
            </div>
        </div>
        <div class="container-fluid">
            <div class="row" style="display: flex; justify-content: center; justify-items: center;">
                <canvas id="myChart"></canvas>
            </div>
        </div>

        <!-- Incluir Spinner.js -->
        <script src="https://cdnjs.cloudflare.com/ajax/libs/spin.js/2.3.2/spin.min.js"></script>

        <!-- Contenedor para el Spinner -->
        <div id="spinner" style="display: none;"></div>


        <!-- jQuery -->
        <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

        <!-- Bootstrap JS -->
        <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/4.6.0/js/bootstrap.bundle.min.js"></script>

        <!-- Bootstrap Multiselect JS -->
        <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.15/js/bootstrap-multiselect.min.js"></script>

        <!-- Chart.js -->
        <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>


        <script>
            $(document).ready(function () {
                // Llamar a la función al cargar la página
                cargarAlmacenes();
            });

            function cargarAlmacenes() {
                $.ajax({
                    url: 'Grafico.aspx/GetWHS',
                    method: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({}),
                    dataType: 'json',
                    timeout: 120000, // ⏳ Aumenta el timeout a 120 segundos (120000 ms)
                    success: function (response) {
                        if (!response.d) {
                            console.error("No se recibieron datos del servidor.");
                            return;
                        }

                        // Parsear los datos del JSON
                        var datos = JSON.parse(response.d);

                        // Llenar dropdown Almacen
                        $("#ddlAlmacen").empty();
                        datos.forEach(almacen => {
                            $("#ddlAlmacen").append(`<option value="${almacen.Whscode}">${almacen.Whsname}</option>`);
                        });

                        // Seleccionar el primer almacén por defecto
                        if (datos.length > 0) {
                            $("#ddlAlmacen").val(datos[0].Whscode);
                        }
                    },
                    error: function (xhr, status, error) {
                        if (status === "timeout") {
                            console.error("⏳ Tiempo de espera agotado para la consulta AJAX.");
                            alert("El servidor tardó demasiado en responder. Inténtalo de nuevo más tarde.");
                        } else {
                            console.error("Error en la solicitud AJAX:", error);
                            alert("Ocurrió un error en el servidor. Por favor, revisa la consola para más detalles.");
                        }
                    }
                });
            }

        </script>

        <script>
            // Variables globales
            var ctx = document.getElementById("myChart").getContext("2d");
            var chart = null;
            var datos = []; // Almacena los datos del JSON

            // Función para cargar datos desde el servidor
            function cargarDatos(anio, mes, almacen) {
                // Configurar el spinner
                var target = document.getElementById("spinner");
                var spinner = new Spinner({
                    lines: 12, length: 10, width: 5, radius: 20, scale: 1,
                    corners: 1, color: "#000", fadeColor: "transparent",
                    speed: 1, rotate: 0, animation: "spinner-line-fade-quick",
                    position: "absolute"
                });

                $("#spinner").show();
                spinner.spin(target);

                $.ajax({
                    url: 'Grafico.aspx/GetData',
                    method: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ anio: anio, mes: mes, almacen: almacen }),
                    dataType: 'json',
                    timeout: 60000,
                    success: function (response) {
                        console.log("Respuesta cruda del servidor:", response);

                        try {
                            // ✅ Intentar parsear la respuesta
                            var parsedData = JSON.parse(response.d);
                            console.log("Datos parseados:", parsedData);

                            if (!Array.isArray(parsedData)) {
                                console.error("⚠️ La respuesta del servidor no es un array.");
                                alert("El servidor devolvió datos en un formato incorrecto.");
                                return;
                            }

                            // ✅ Asignar los datos al array global
                            datos = parsedData;

                            // ✅ Validar si realmente hay datos
                            if (datos.length === 0) {
                                console.warn("⚠️ No se encontraron datos válidos.");
                                alert("No hay datos disponibles para los parámetros ingresados.");
                                return;
                            }

                            // Llenar dropdowns y actualizar gráfico
                            llenarDropdowns(datos);
                            crearGrafico(datos);

                        } catch (error) {
                            console.error("⚠️ Error al parsear JSON:", error);
                            alert("Error al interpretar la respuesta del servidor.");
                        }
                    },
                    error: function (xhr, status, error) {
                        console.error("Error en AJAX:", error);
                        alert("Ocurrió un error al obtener los datos.");
                    },
                    complete: function () {
                        // ✅ Asegurar que el spinner se detiene en todos los casos
                        spinner.stop();
                        $("#spinner").hide();
                    }
                });
            }

            // Función para llenar los dropdowns de Marca y Modelo
            function llenarDropdowns(datos) {
                var marcas = [...new Set(datos.map(item => item.Marca))];

                $("#ddlMarca").empty();
                marcas.forEach(marca => {
                    $("#ddlMarca").append(`<option value="${marca}">${marca}</option>`);
                });

                $("#ddlMarca").multiselect({
                    selectAllJustVisible: true,
                    nonSelectedText: 'Seleccionar Marcas',
                    onChange: function () {
                        actualizarModelos(true);
                        actualizarGrafico();
                    }
                });

                $("#ddlModelo").multiselect({
                    selectAllJustVisible: true,
                    nonSelectedText: 'Seleccionar Modelos',
                    onChange: function () {
                        actualizarGrafico();
                    }
                });

                actualizarModelos(false);
            }

            // Función para actualizar el dropdown de Modelo
            function actualizarModelos(marcarModelos) {
                var marcasSeleccionadas = $("#ddlMarca").val() || [];
                var modelosFiltrados = marcasSeleccionadas.length > 0
                    ? [...new Set(datos.filter(item => marcasSeleccionadas.includes(item.Marca)).map(item => item.Modelo))]
                    : [...new Set(datos.map(item => item.Modelo))];

                $("#ddlModelo").empty();
                modelosFiltrados.forEach(modelo => {
                    $("#ddlModelo").append(`<option value="${modelo}" selected>${modelo}</option>`);
                });

                $("#ddlModelo").multiselect('rebuild');

                if (marcarModelos) {
                    $("#ddlModelo").multiselect('selectAll', false);
                    $("#ddlModelo").multiselect('updateButtonText');
                }
            }

            // Función para crear el gráfico inicial
            function crearGrafico(datos) {
                var labels = [...new Set(datos.map(item => item.Dia))];
                var datasets = [];

                var marcas = [...new Set(datos.map(item => item.Marca))];
                marcas.forEach(marca => {
                    var valores = labels.map(dia => {
                        var items = datos.filter(item => item.Dia === dia && item.Marca === marca);
                        return items.reduce((sum, item) => sum + Number(item.InvInicial || 0), 0);
                    });

                    datasets.push({
                        label: marca,
                        data: valores,
                        borderColor: generarColorHex(),
                        fill: false
                    });
                });

                if (chart) {
                    chart.destroy();
                }

                chart = new Chart(ctx, {
                    type: 'line',
                    data: { labels: labels, datasets: datasets },
                    options: { scales: { y: { beginAtZero: true } } }
                });
            }

            // Función para actualizar el gráfico
            function actualizarGrafico() {
                var marcasSeleccionadas = $("#ddlMarca").val() || [];
                var modelosSeleccionados = $("#ddlModelo").val() || [];

                var datosFiltrados = datos.filter(item =>
                    marcasSeleccionadas.includes(item.Marca) && modelosSeleccionados.includes(item.Modelo)
                );

                var labels = [...new Set(datosFiltrados.map(item => item.Dia))];
                var datasets = [];

                marcasSeleccionadas.forEach(marca => {
                    var valores = labels.map(dia => {
                        var items = datosFiltrados.filter(item => item.Dia === dia && item.Marca === marca);
                        return items.reduce((sum, item) => sum + Number(item.InvInicial || 0), 0);
                    });

                    datasets.push({
                        label: marca,
                        data: valores,
                        borderColor: generarColorHex(),
                        fill: false
                    });
                });

                chart.data.labels = labels;
                chart.data.datasets = datasets;
                chart.update();
            }

            function generarColorHex() {
                return '#' + Math.floor(Math.random() * 16777215).toString(16).padStart(6, '0');
            }

            // Evento para el botón de filtro
            $('#btnFiltrar').click(function () {
                var anio = $('#ddlAnio').val();
                var mes = $('#ddlMes').val();
                var almacen = $('#ddlAlmacen option:selected').val();
                cargarDatos(anio, mes, almacen);
            });

        </script>

        <%--<script>
            // Variables globales
            var ctx = document.getElementById("myChart").getContext("2d");
            var chart = null;
            var datos = []; // Almacena los datos del JSON

            // Función para cargar datos desde el servidor
            function cargarDatos(anio, mes, almacen) {
                // Configurar el spinner
                var target = document.getElementById("spinner");
                var spinner = new Spinner({
                    lines: 12, length: 10, width: 5, radius: 20, scale: 1,
                    corners: 1, color: "#000", fadeColor: "transparent",
                    speed: 1, rotate: 0, animation: "spinner-line-fade-quick",
                    position: "absolute"
                });

                $("#spinner").show();
                spinner.spin(target);

                $.ajax({
                    url: 'Grafico.aspx/GetData',
                    method: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ anio: anio, mes: mes, almacen: almacen }),
                    dataType: 'json',
                    timeout: 60000,
                    success: function (response) {
                        console.log("Respuesta del servidor:", response);

                        if (response.error) {
                            console.error("Error en la respuesta del servidor:", response.error);
                            alert("Error al obtener datos: " + response.error);
                            return;
                        }

                        // ✅ Convertir a array si no lo es
                        if (!Array.isArray(response)) {
                            console.warn("El servidor no devolvió un array. Intentando convertir...");
                            datos = Object.values(response);
                        } else {
                            datos = response;
                        }

                        console.log("Datos procesados:", datos);

                        // ✅ Validar si realmente hay datos
                        if (!Array.isArray(datos) || datos.length === 0) {
                            console.warn("⚠️ No se encontraron datos válidos.");
                            alert("No hay datos disponibles para los parámetros ingresados.");
                            return;
                        }

                        // Llenar dropdowns y actualizar gráfico
                        llenarDropdowns(datos);
                        crearGrafico(datos);
                    },
                    error: function (xhr, status, error) {
                        console.error("Error en AJAX:", error);
                        alert("Ocurrió un error al obtener los datos.");
                    },
                    complete: function () {
                        // Ocultar el spinner cuando la consulta finaliza
                        $("#spinner").hide();
                        spinner.stop();
                    }
                });
            } // <-- Cierre de la función cargarDatos

            // Función para llenar los dropdowns de Marca y Modelo
            function llenarDropdowns(datos) {
                var marcas = [...new Set(datos.map(item => item.Marca))];

                $("#ddlMarca").empty();
                marcas.forEach(marca => {
                    $("#ddlMarca").append(`<option value="${marca}">${marca}</option>`);
                });

                $("#ddlMarca").multiselect({
                    selectAllJustVisible: true,
                    nonSelectedText: 'Seleccionar Marcas',
                    onChange: function () {
                        actualizarModelos(true);
                        actualizarGrafico();
                    }
                });

                $("#ddlModelo").multiselect({
                    selectAllJustVisible: true,
                    nonSelectedText: 'Seleccionar Modelos',
                    onChange: function () {
                        actualizarGrafico();
                    }
                });

                actualizarModelos(false);
            }

            // Función para actualizar el dropdown de Modelo
            function actualizarModelos(marcarModelos) {
                var marcasSeleccionadas = $("#ddlMarca").val() || [];
                var modelosFiltrados = marcasSeleccionadas.length > 0
                    ? [...new Set(datos.filter(item => marcasSeleccionadas.includes(item.Marca)).map(item => item.Modelo))]
                    : [...new Set(datos.map(item => item.Modelo))];

                $("#ddlModelo").empty();
                modelosFiltrados.forEach(modelo => {
                    $("#ddlModelo").append(`<option value="${modelo}" selected>${modelo}</option>`);
                });

                $("#ddlModelo").multiselect('rebuild');

                if (marcarModelos) {
                    $("#ddlModelo").multiselect('selectAll', false);
                    $("#ddlModelo").multiselect('updateButtonText');
                }
            }

            // Función para crear el gráfico inicial
            function crearGrafico(datos) {
                var labels = [...new Set(datos.map(item => item.Dia))];
                var datasets = [];

                var marcas = [...new Set(datos.map(item => item.Marca))];
                marcas.forEach(marca => {
                    var valores = labels.map(dia => {
                        var items = datos.filter(item => item.Dia === dia && item.Marca === marca);
                        return items.reduce((sum, item) => sum + Number(item.InvInicial || 0), 0);
                    });

                    datasets.push({
                        label: marca,
                        data: valores,
                        borderColor: generarColorHex(),
                        fill: false
                    });
                });

                if (chart) {
                    chart.destroy();
                }

                chart = new Chart(ctx, {
                    type: 'line',
                    data: { labels: labels, datasets: datasets },
                    options: { scales: { y: { beginAtZero: true } } }
                });
            }

            // Función para actualizar el gráfico
            function actualizarGrafico() {
                var marcasSeleccionadas = $("#ddlMarca").val() || [];
                var modelosSeleccionados = $("#ddlModelo").val() || [];

                var datosFiltrados = datos.filter(item =>
                    marcasSeleccionadas.includes(item.Marca) && modelosSeleccionados.includes(item.Modelo)
                );

                var labels = [...new Set(datosFiltrados.map(item => item.Dia))];
                var datasets = [];

                marcasSeleccionadas.forEach(marca => {
                    var valores = labels.map(dia => {
                        var items = datosFiltrados.filter(item => item.Dia === dia && item.Marca === marca);
                        return items.reduce((sum, item) => sum + Number(item.InvInicial || 0), 0);
                    });

                    datasets.push({
                        label: marca,
                        data: valores,
                        borderColor: generarColorHex(),
                        fill: false
                    });
                });

                chart.data.labels = labels;
                chart.data.datasets = datasets;
                chart.update();
            }

            function generarColorHex() {
                return '#' + Math.floor(Math.random() * 16777215).toString(16).padStart(6, '0');
            }

            $('#btnFiltrar').click(function () {
                var anio = $('#ddlAnio').val();
                var mes = $('#ddlMes').val();
                var almacen = $('#ddlAlmacen option:selected').val(); // 🔹 Asegura que se obtiene el valor seleccionado correctamente
                //var almacen = $('#ddlAlmacen').val();
                cargarDatos(anio, mes, almacen);
            });
        </script>--%>

        <%--   <script>
            // Variables globales
            var ctx = document.getElementById("myChart").getContext("2d");
            var chart = null;
            var datos = []; // Almacena los datos del JSON

            // Función para cargar datos desde el servidor
            function cargarDatos(anio, mes, almacen) {
                // Configurar el spinner
                var target = document.getElementById("spinner");
                var spinner = new Spinner({
                    lines: 12, length: 10, width: 5, radius: 20, scale: 1,
                    corners: 1, color: "#000", fadeColor: "transparent",
                    speed: 1, rotate: 0, animation: "spinner-line-fade-quick",
                    position: "absolute"
                });

                $("#spinner").show();
                spinner.spin(target);

                $.ajax({
                    url: 'Grafico.aspx/GetData',
                    method: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ anio: anio, mes: mes, almacen: almacen }),
                    dataType: 'json',
                    timeout: 60000,
                    success: function (response) {
                        console.log("Respuesta del servidor:", response);

                        if (response.error) {
                            console.error("Error en la respuesta del servidor:", response.error);
                            alert("Error al obtener datos: " + response.error);
                            return;
                        }

                        // ✅ Convertir a array si no lo es
                        if (!Array.isArray(response)) {
                            console.warn("El servidor no devolvió un array. Intentando convertir...");
                            datos = Object.values(response);
                        } else {
                            datos = response;
                        }

                        console.log("Datos procesados:", datos);

                        // ✅ Validar si realmente hay datos
                        if (!Array.isArray(datos) || datos.length === 0) {
                            console.warn("⚠️ No se encontraron datos válidos.");
                            alert("No hay datos disponibles para los parámetros ingresados.");
                            return;
                        }

                        // Llenar dropdowns y actualizar gráfico
                        llenarDropdowns(datos);
                        crearGrafico(datos);
                    },
                    error: function (xhr, status, error) {
                        console.error("Error en AJAX:", error);
                        alert("Ocurrió un error al obtener los datos.");
                    },
                    complete: function () {
                        // Ocultar el spinner cuando la consulta finaliza
                        $("#spinner").hide();
                        spinner.stop();
                    }
                });

            // Función para llenar los dropdowns de Marca y Modelo
            function llenarDropdowns(datos) {
                var marcas = [...new Set(datos.map(item => item.Marca))];

                $("#ddlMarca").empty();
                marcas.forEach(marca => {
                    $("#ddlMarca").append(`<option value="${marca}">${marca}</option>`);
                });

                $("#ddlMarca").multiselect({
                    selectAllJustVisible: true,
                    nonSelectedText: 'Seleccionar Marcas',
                    onChange: function () {
                        actualizarModelos(true);
                        actualizarGrafico();
                    }
                });

                $("#ddlModelo").multiselect({
                    selectAllJustVisible: true,
                    nonSelectedText: 'Seleccionar Modelos',
                    onChange: function () {
                        actualizarGrafico();
                    }
                });

                actualizarModelos(false);
            }

            // Función para actualizar el dropdown de Modelo
            function actualizarModelos(marcarModelos) {
                var marcasSeleccionadas = $("#ddlMarca").val() || [];
                var modelosFiltrados = marcasSeleccionadas.length > 0
                    ? [...new Set(datos.filter(item => marcasSeleccionadas.includes(item.Marca)).map(item => item.Modelo))]
                    : [...new Set(datos.map(item => item.Modelo))];

                $("#ddlModelo").empty();
                modelosFiltrados.forEach(modelo => {
                    $("#ddlModelo").append(`<option value="${modelo}" selected>${modelo}</option>`);
                });

                $("#ddlModelo").multiselect('rebuild');

                if (marcarModelos) {
                    $("#ddlModelo").multiselect('selectAll', false);
                    $("#ddlModelo").multiselect('updateButtonText');
                }
            }

            // Función para crear el gráfico inicial
            function crearGrafico(datos) {
                var labels = [...new Set(datos.map(item => item.Dia))];
                var datasets = [];

                var marcas = [...new Set(datos.map(item => item.Marca))];
                marcas.forEach(marca => {
                    var valores = labels.map(dia => {
                        var items = datos.filter(item => item.Dia === dia && item.Marca === marca);
                        return items.reduce((sum, item) => sum + Number(item.InvInicial || 0), 0);
                    });

                    datasets.push({
                        label: marca,
                        data: valores,
                        borderColor: generarColorHex(),
                        fill: false
                    });
                });

                if (chart) {
                    chart.destroy();
                }

                chart = new Chart(ctx, {
                    type: 'line',
                    data: { labels: labels, datasets: datasets },
                    options: { scales: { y: { beginAtZero: true } } }
                });
            }

            // Función para actualizar el gráfico
            function actualizarGrafico() {
                var marcasSeleccionadas = $("#ddlMarca").val() || [];
                var modelosSeleccionados = $("#ddlModelo").val() || [];

                var datosFiltrados = datos.filter(item =>
                    marcasSeleccionadas.includes(item.Marca) && modelosSeleccionados.includes(item.Modelo)
                );

                var labels = [...new Set(datosFiltrados.map(item => item.Dia))];
                var datasets = [];

                marcasSeleccionadas.forEach(marca => {
                    var valores = labels.map(dia => {
                        var items = datosFiltrados.filter(item => item.Dia === dia && item.Marca === marca);
                        return items.reduce((sum, item) => sum + Number(item.InvInicial || 0), 0);
                    });

                    datasets.push({
                        label: marca,
                        data: valores,
                        borderColor: generarColorHex(),
                        fill: false
                    });
                });

                chart.data.labels = labels;
                chart.data.datasets = datasets;
                chart.update();
            }

            function generarColorHex() {
                return '#' + Math.floor(Math.random() * 16777215).toString(16).padStart(6, '0');
            }

            $('#btnFiltrar').click(function () {
                var anio = $('#ddlAnio').val();
                var mes = $('#ddlMes').val();
                var almacen = $('#ddlAlmacen option:selected').val(); // 🔹 Asegura que se obtiene el valor seleccionado correctamente
                //var almacen = $('#ddlAlmacen').val();
                cargarDatos(anio, mes, almacen);
            });
</script>--%>
        <%--   <script>
            // Variables globales
            var ctx = document.getElementById("myChart").getContext("2d");
            var chart = null;
            var datos = []; // Almacenará los datos del JSON

            // Función para cargar datos desde el servidor

            function cargarDatos(anio, mes, almacen) {
                // Configurar el spinner
                var target = document.getElementById("spinner");
                var spinner = new Spinner({
                    lines: 12, // Número de líneas
                    length: 10, // Longitud de cada línea
                    width: 5, // Ancho de cada línea
                    radius: 20, // Radio del spinner
                    scale: 1, // Escala del spinner
                    corners: 1, // Bordes redondeados (1 = sí, 0 = no)
                    color: "#000", // Color
                    fadeColor: "transparent",
                    speed: 1, // Velocidad de giro
                    rotate: 0, // Rotación inicial
                    animation: "spinner-line-fade-quick", // Animación
                    position: "absolute"
                });

                // Mostrar el spinner antes de la llamada AJAX
                $("#spinner").show();
                spinner.spin(target);

                $.ajax({
                    url: 'Grafico.aspx/GetData',
                    method: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ anio: anio, mes: mes, almacen: almacen }),
                    dataType: 'json',
                    timeout: 60000,  // ⏳ Aumenta el timeout a 60 segundos (60000 ms)
                    success: function (response) {
                        if (!response.d) {
                            console.error("No se recibieron datos del servidor.");
                            return;
                        }

                        // Parsear los datos del JSON
                        datos = JSON.parse(response.d);

                        // Llenar los dropdowns de Marca y Modelo
                        llenarDropdowns(datos);

                        // Crear el gráfico inicial
                        crearGrafico(datos);
                    },
                    error: function (xhr, status, error) {
                        if (status === "timeout") {
                            console.error("⏳ Tiempo de espera agotado para la consulta AJAX.");
                            alert("El servidor tardó demasiado en responder. Inténtalo de nuevo más tarde.");
                        } else {
                            console.error("Error en la solicitud AJAX:", error);
                            alert("Ocurrió un error en el servidor. Por favor, revisa la consola para más detalles.");
                        }
                    },
                    complete: function () {
                        // Ocultar el spinner cuando la consulta finaliza
                        $("#spinner").hide();
                        spinner.stop();
                    }
                });
            }
            // Función para llenar los dropdowns de Marca y Modelo
            function llenarDropdowns(datos) {
                var marcas = [...new Set(datos.map(item => item.Marca))];

                // Llenar el dropdown de Marca
                $("#ddlMarca").empty();
                marcas.forEach(marca => {
                    $("#ddlMarca").append(`<option value="${marca}">${marca}</option>`);
                });

                // Inicializar el multiselect de Marca
                $("#ddlMarca").multiselect({
                    selectAllJustVisible: true,
                    nonSelectedText: 'Seleccionar Marcas',
                    onChange: function (option, checked, select) {
                        // Actualizar el dropdown de Modelo cuando cambia la selección de Marca
                        actualizarModelos(true);
                        actualizarGrafico();
                    }
                });

                // Inicializar el multiselect de Modelo
                $("#ddlModelo").multiselect({
                    selectAllJustVisible: true,
                    nonSelectedText: 'Seleccionar Modelos',
                    onChange: function (option, checked, select) {
                        actualizarGrafico();
                    }
                });

                // Llenar inicialmente el dropdown de Modelo con todos los modelos
                actualizarModelos(false);
            }

            // Función para actualizar el dropdown de Modelo y marcar los modelos automáticamente
            function actualizarModelos(marcarModelos) {
                var marcasSeleccionadas = $("#ddlMarca").val() || [];
                var modelosFiltrados = [];

                if (marcasSeleccionadas.length > 0) {
                    // Filtrar los modelos que pertenecen a las marcas seleccionadas
                    modelosFiltrados = [...new Set(datos
                        .filter(item => marcasSeleccionadas.includes(item.Marca))
                        .map(item => item.Modelo)
                    )];
                } else {
                    // Si no hay marcas seleccionadas, mostrar todos los modelos
                    modelosFiltrados = [...new Set(datos.map(item => item.Modelo))];
                }

                // Llenar el dropdown de Modelo con los modelos filtrados y marcarlos si es necesario
                $("#ddlModelo").empty();
                modelosFiltrados.forEach(modelo => {
                    $("#ddlModelo").append(`<option value="${modelo}" selected>${modelo}</option>`);
                });

                // Actualizar el multiselect de Modelo y marcar automáticamente los modelos
                $("#ddlModelo").multiselect('rebuild');

                if (marcarModelos) {
                    $("#ddlModelo").multiselect('selectAll', false);
                    $("#ddlModelo").multiselect('updateButtonText');
                }
            }

            // Función para crear el gráfico inicial con suma correcta
            function crearGrafico(datos) {
                var labels = [...new Set(datos.map(item => item.Dia))]; // Eje X: Días
                var datasets = [];

                // Crear un dataset por cada marca
                var marcas = [...new Set(datos.map(item => item.Marca))];
                marcas.forEach(marca => {
                    var valores = labels.map(dia => {
                        // Filtrar todos los registros de esa marca y día
                        var items = datos.filter(item => item.Dia === dia && item.Marca === marca);

                        // Sumar los valores de InvInicial
                        return items.reduce((sum, item) => sum + Number(item.InvInicial || 0), 0);
                    });

                    datasets.push({
                        label: marca,
                        data: valores,
                        borderColor: generarColorHex(), // Color único para cada marca
                        fill: false
                    });
                });

                // Destruir el gráfico anterior si existe
                if (chart) {
                    chart.destroy();
                }

                // Crear el nuevo gráfico
                chart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: labels,
                        datasets: datasets
                    },
                    options: {
                        scales: {
                            y: {
                                beginAtZero: true
                            }
                        }
                    }
                });
            }

            // Función para actualizar el gráfico según las selecciones con suma correcta
            function actualizarGrafico() {
                var marcasSeleccionadas = $("#ddlMarca").val() || [];
                var modelosSeleccionados = $("#ddlModelo").val() || [];

                // Filtrar los datos según las selecciones
                var datosFiltrados = datos.filter(item =>
                    marcasSeleccionadas.includes(item.Marca) &&
                    modelosSeleccionados.includes(item.Modelo)
                );

                var labels = [...new Set(datosFiltrados.map(item => item.Dia))];
                var datasets = [];

                marcasSeleccionadas.forEach(marca => {
                    var valores = labels.map(dia => {
                        // Filtrar todos los registros de esa marca y día
                        var items = datosFiltrados.filter(item => item.Dia === dia && item.Marca === marca);

                        // Sumar los valores de InvInicial
                        return items.reduce((sum, item) => sum + Number(item.InvInicial || 0), 0);
                    });

                    datasets.push({
                        label: marca,
                        data: valores,
                        borderColor: generarColorHex(),
                        fill: false
                    });
                });

                // Actualizar el gráfico
                chart.data.labels = labels;
                chart.data.datasets = datasets;
                chart.update();
            }

            // Función para generar colores hexadecimales aleatorios
            function generarColorHex() {
                return '#' + Math.floor(Math.random() * 16777215).toString(16).padStart(6, '0');
            }

            $('#btnFiltrar').click(function () {
                var anio = $('#ddlAnio').val();
                var mes = $('#ddlMes').val();
                var almacen = $('#ddlAlmacen option:selected').val(); // 🔹 Asegura que se obtiene el valor seleccionado correctamente
                //var almacen = $('#ddlAlmacen').val();
                cargarDatos(anio, mes, almacen);
            });
        </script>--%>
    </form>
</body>
</html>
