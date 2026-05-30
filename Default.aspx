<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Control Produccion</title>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="apple-touch-icon" sizes="180x180" href="dist\img\apple-touch-icon.png">
  <link rel="icon" type="image/png" sizes="32x32" href="dist\img\favicon-32x32.png">
  <link rel="icon" type="image/png" sizes="16x16" href="dist\img\favicon-16x16.png">
  <%--<link rel="manifest" href="dist\img\site.webmanifest">--%>
  
  <!-- Google Font: Source Sans Pro -->
  <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback">
  <!-- Font Awesome -->
  <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css">
  <!-- icheck bootstrap -->
  <link rel="stylesheet" href="plugins/icheck-bootstrap/icheck-bootstrap.min.css">
  <!-- Theme style -->
  <link rel="stylesheet" href="dist/css/adminlte.min.css">

</head>
<body>
    <form id="form1" runat="server">
<body class="hold-transition login-page" style="background-image: url(https://grupomovesa.com/wp-content/uploads/2022/04/PHO_STAGE_390-duke-21-header_SALL_AEPI_V1.jpeg); background-repeat: no-repeat; -webkit-background-size: cover; -moz-background-size: cover;
-o-background-size: cover; background-size: cover; background-position: center;">
<div class="login-box">
  <div class="login-logo">
    <h2 style="color: #fff; font-weight: bold; background-color: #303030eb; border-radius: 1vh;" >MOVESA</h2>
    <h4 style="color: #fff; font-weight: bold; background-color: #303030eb; border-radius: 1vh;">Motos que mueven Honduras</h4>
    <?php	if(isset($mensaje)) echo $mensaje;	?>
  </div>
  <!-- /.login-logo -->
  <div class="card" style="border-radius:2em;" >
    <div class="card-body login-card-body">
      <img src="dist\img\logo_movesa.jpg" style="display: block; margin: auto; padding: 2%; margin-bottom: 1em;" class="img-circle" width="220" height="180">

      <%--<form action="#" method="POST">--%>
        <div class="input-group mb-3">
            <asp:TextBox ID="txtusuario" runat="server" class="form-control" placeholder="Usuario" required></asp:TextBox>
          <div class="input-group-append">
            <div class="input-group-text">
              <span style="color: #000;" class="fas fa-user"></span>
            </div>
          </div>
        </div>
        <div class="input-group mb-3">
            <asp:TextBox ID="txtpassword" runat="server" class="form-control" placeholder="Contraseña" TextMode="Password" required></asp:TextBox>
          <div class="input-group-append">
            <div class="input-group-text">
              <span style="color: #000;" class="fas fa-lock"></span>
            </div>
          </div>
        </div>
         
        <div class="row">
          <div class="col-7">
            <div class="icheck-primary">
              <!-- SPACE DE COL 8 -->
            </div>
          </div>
          <!-- /.col -->
          <div class="col-5">
              <asp:Button ID="btnlogin" runat="server" class="btn btn-primary btn-block" Text="Iniciar"/>
          </div>
            
          <!-- /.col -->
        </div>
      <%--</form>--%>
    </div>
    <!-- /.login-card-body -->
  </div>
   <!-- Footer -->
            <footer class="sticky-footer bg-white">
                <div class="container my-auto">
                    <div class="copyright text-center my-auto">
                        <span>Portal Control Produccion &copy; 2022 | WebDesing: rj</span>
                    </div>
                </div>
            </footer>
            <!-- End of Footer -->
</div>
<!-- /.login-box -->

           
	
<!-- jQuery -->
<script src="plugins/jquery/jquery.min.js"></script>
<!-- Bootstrap 4 -->
<script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
<!-- AdminLTE App -->
<script src="dist/js/adminlte.min.js"></script>
<asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
</form>
</body>
</html>