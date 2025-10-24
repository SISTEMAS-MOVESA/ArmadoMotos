<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CrearClientes.aspx.vb" Inherits="CrearClientes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Clientes SKG</title>
    <style type="text/css">
        .auto-style1 {
            height: 39px;
        }
        .auto-style2 {
            height: 33px;
        }
    </style>

     <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
     <script>
        function textCount(val, controlname) {

          // alert(controlname);
            
            switch (controlname) {
                case 'txtNcliente':
                    var len = val.value.length;
                    if (len > 100) {
                        val.value = val.value.substring(0, 100);
                        e.preventDefault();
                    }
                    else {
                    $('#lblNclienteLen').text(100 - len);
                    }
                    break;
                case 'txtIdentidad':
                    var len = val.value.length;
                    if (len > 13) {
                        val.value = val.value.substring(0, 13);
                        e.preventDefault();
                    }
                    else {
                    $('#lblIdentidadLen').text(13 - len);
                        }
                    break;
                case 'txtRTN':
                    var len = val.value.length;
                    if (len > 14) {
                        val.value = val.value.substring(0, 14);
                        e.preventDefault();
                    }
                    else {
                        $('#lblRTN').text(14 - len);
                    }
                    break;
                case 'txtColonia':
                    var len = val.value.length;
                    if (len > 100) {
                        val.value = val.value.substring(0, 100);
                        e.preventDefault();
                    }
                    else {
                        $('#lblColonia').text(100 - len);
                    }
                    break;
                    txtCalle
                case 'txtCalle':
                    var len = val.value.length;
                    if (len > 100) {
                        val.value = val.value.substring(0, 100);
                        e.preventDefault();
                    }
                    else {
                        $('#lblCalle').text(100 - len);
                    }
                    break;
                case 'txtTelefono1':
                    var len = val.value.length;
                    if (len > 8) {
                        val.value = val.value.substring(0, 8);
                        e.preventDefault();
                    }
                    else {
                        $('#lblTel1').text(8 - len);
                    }
                    break;
                case 'txtTelefono2':
                    var len = val.value.length;
                    if (len > 8) {
                        val.value = val.value.substring(0, 8);
                        e.preventDefault();
                    }
                    else {
                        $('#lblTel2').text(8 - len);
                    }
                    break;
                case 'txtCelular':
                    var len = val.value.length;
                    if (len > 8) {
                        val.value = val.value.substring(0, 8);
                        e.preventDefault();
                    }
                    else {
                        $('#lblCelular').text(8 - len);
                    }
                    break;
                case 'txtCasa':
                    var len = val.value.length;
                    if (len > 20) {
                        val.value = val.value.substring(0, 20);
                        e.preventDefault();
                    }
                    else {
                        $('#lblCasa').text(20 - len);
                    }
                    break;
                case 'txtCorreo':
                    var len = val.value.length;
                    if (len > 100) {
                        val.value = val.value.substring(0, 100);
                        e.preventDefault();
                    }
                    else {
                        $('#lblCorreo').text(100 - len);
                    }
                    break;
                case 'txtCreadoPor':
                    var len = val.value.length;
                    if (len > 50) {
                        val.value = val.value.substring(0, 50);
                        e.preventDefault();
                    }
                    else {
                        $('#lblCreadoPor').text(50 - len);
                    }
                    break;
            }
        };
    </script>
   

</head>
<body>
    <form id="form1" runat="server">
    <center>
        <div>
    <table>
        <tr>
            <td colspan="6"> <center>
                <img src="Imagenes/logomovesa.jpg" alt="" width="200" height="100"/>
                    </center></td>
        </tr>
        <tr>
            <td class="auto-style2">Nombre Cliente</td>
            <td class="auto-style2">
                <asp:TextBox ID="txtNcliente" runat="server" AutoCompleteType="Disabled" 
                    Width="200px" MaxLength="100"
                    onkeyup="textCount(this,'txtNcliente')"></asp:TextBox>
            </td>
            <td class="auto-style2">
                (<asp:Label ID="lblNclienteLen" ForeColor="#0066ff" Text="100" runat="server" />/100)
            </td>
            <td class="auto-style2">Departamento</td>
            <td class="auto-style2">
                <asp:DropDownList ID="drpDepartamento" runat="server" Width="200px" AutoPostBack="true">
                </asp:DropDownList>
            </td>
            
            <td class="auto-style2">
            </td>
            
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                        ControlToValidate="txtNcliente" ErrorMessage="Se Requiere Nombre"
                        SetFocusOnError="True" ForeColor="#FF3300" ></asp:RequiredFieldValidator></td>
            
            <td>
                &nbsp;</td>
            
            <td>&nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>Identidad</td>
            <td>
                <asp:TextBox ID="txtIdentidad" runat="server" AutoCompleteType="Disabled" Width="200px"
                    MaxLength="13"
                    onkeyup="textCount(this,'txtIdentidad')"></asp:TextBox>
            </td>
            
            <td>
            (<asp:Label ID="lblIdentidadLen" ForeColor="#0066FF" Text="13" runat="server" />/13)
            </td>
            
            <td>Municipio</td>
            <td>
                <asp:DropDownList ID="drpMunicipio" runat="server" Width="200px" AutoPostBack="true">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                        ControlToValidate="txtIdentidad" ErrorMessage="Se Requiere Identidad"
                        SetFocusOnError="True" ForeColor="#FF3300" ></asp:RequiredFieldValidator></td>
            
            <td>
                &nbsp;</td>
            
            <td>&nbsp;</td>
            <td style="margin-left: 80px">
                &nbsp;</td>
            <td>
                 &nbsp;</td>
        </tr>
        <tr>
            <td>RTN Cliente</td>
            <td>
                <asp:TextBox ID="txtRTN" runat="server" AutoCompleteType="Disabled" Width="200px"
                    MaxLength="14"
                    onkeyup="textCount(this,'txtRTN')"></asp:TextBox>
            </td>
            
            <td>
                (<asp:Label ID="lblRTN" ForeColor="#0066ff" Text="14" runat="server" />/14)
            </td>
            
            <td>Colonia/Barrio</td>
            <td style="margin-left: 80px">
                <asp:TextBox ID="txtColonia" runat="server" AutoCompleteType="Disabled" Width="200px"
                    MaxLength="100"
                    onkeyup="textCount(this,'txtColonia')"></asp:TextBox>
            </td>
            <td>
                 (<asp:Label ID="lblColonia" ForeColor="#0066ff" Text="100" runat="server" />/100)
           </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                        ControlToValidate="txtRTN" ErrorMessage="Se Requiere RTN"
                        SetFocusOnError="True" ForeColor="#FF3300" ></asp:RequiredFieldValidator>--%></td>
            
            <td>&nbsp;</td>
            
            <td>&nbsp;</td>
            <td>
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                        ControlToValidate="txtColonia" ErrorMessage="Se Requiere Direccion"
                        SetFocusOnError="True" ForeColor="#FF3300" ></asp:RequiredFieldValidator></td>
            <td>
                 &nbsp;</td>
        </tr>

        <tr>
            <td>Grupo Cliente</td>
            <td>
                <asp:DropDownList ID="drpGrupoClientes" runat="server" Width="200px">
                </asp:DropDownList>
            </td>
            
            <td>&nbsp;</td>
            
            <td>Calle/Avenida</td>
            <td>
                <asp:TextBox ID="txtCalle" runat="server" AutoCompleteType="Disabled" Width="200px"
                    MaxLength="100"
                    onkeyup="textCount(this,'txtCalle')"></asp:TextBox>
            </td>
            <td>
                 (<asp:Label ID="lblCalle" ForeColor="#0066ff" Text="100" runat="server" />/100)
           </td>
        </tr>

        <tr>
            <td>Telefono Negocio1</td>
            <td>
                <asp:TextBox ID="txtTelefono1" runat="server" AutoCompleteType="Disabled" Width="200px"
                    MaxLength="8"
                    onkeyup="textCount(this,'txtTelefono1')"></asp:TextBox>
            </td>
            
            <td> (<asp:Label ID="lblTel1" ForeColor="#0066ff" Text="8" runat="server" />/8)
           </td>
            
            <td>Casa</td>
            <td>
                <asp:TextBox ID="txtCasa" runat="server" AutoCompleteType="Disabled" Width="200px"
                    MaxLength="20"
                    onkeyup="textCount(this,'txtCasa')"></asp:TextBox>
            </td>
            <td>
                 (<asp:Label ID="lblCasa" ForeColor="#0066ff" Text="20" runat="server" />/20)
           </td>
        </tr>

        <tr>
            <td>Telefono Negocio 2</td>
            <td>
                <asp:TextBox ID="txtTelefono2" runat="server" AutoCompleteType="Disabled" Width="200px"
                    MaxLength="8"
                    onkeyup="textCount(this,'txtTelefono2')"></asp:TextBox>
            </td>
            
            <td> 
                (<asp:Label ID="lblTel2" ForeColor="#0066ff" Text="8" runat="server" />/8)
           </td>
            
            <td>Sucursal</td>
            <td>
                <asp:DropDownList ID="drpSucursal" runat="server" Width="200px">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;</td>
        </tr>

        <tr>
            <td>Telefono Movil Neg.</td>
            <td>
                <asp:TextBox ID="txtCelular" runat="server" AutoCompleteType="Disabled" Width="200px"
                    MaxLength="8"
                    onkeyup="textCount(this,'txtCelular')"></asp:TextBox>
            </td>
            
            <td> (<asp:Label ID="lblCelular" ForeColor="#0066ff" Text="8" runat="server" />/8)

           </td>
            
            <td>Creado Por</td>
            <td>
                <asp:TextBox ID="txtCreadoPor" runat="server" AutoCompleteType="Disabled" Width="200px"
                     MaxLength="50"
                    onkeyup="textCount(this,'txtCreadoPor')"></asp:TextBox>
            </td>
            <td>
                 (<asp:Label ID="lblCreadoPor" ForeColor="#0066ff" Text="50" runat="server" />/50)
           </td>
        </tr>

        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                        ControlToValidate="txtCelular" ErrorMessage="Se Requiere Celular"
                        SetFocusOnError="True" ForeColor="#FF3300" ></asp:RequiredFieldValidator>
                        
            </td>
            <td> &nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>

        <tr>
            <td>Correo Negocio</td>
            <td>
                <asp:TextBox ID="txtCorreo" runat="server" AutoCompleteType="Disabled" Width="200px"
                    MaxLength="100"
                    onkeyup="textCount(this,'txtCorreo')"></asp:TextBox>
                        
            </td>
            <td> (<asp:Label ID="lblCorreo" ForeColor="#0066ff" Text="100" runat="server" />/100)
           </td>
            <td></td>
            <td></td>
            <td></td>
        </tr>

        <tr>
            <td>&nbsp;</td>
            <td>
                        
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                        ControlToValidate="txtCorreo" ErrorMessage="Se Requiere Direccion"
                        SetFocusOnError="True" ForeColor="#FF3300" ></asp:RequiredFieldValidator>

            </td>
            
            <td>&nbsp;</td>
            
            <td>

                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                        ErrorMessage="Formato Incorrecto" ControlToValidate="txtCorreo"
                        SetFocusOnError="True"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="#FF3300"></asp:RegularExpressionValidator>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>

        <tr>
            <td>Vendedor</td>
            <td>
                <asp:DropDownList ID="drpVendedores" runat="server" Width="200px">
                </asp:DropDownList>
            </td>
            
            <td>&nbsp;</td>
            
            <td>Codigo Asignado</td>
            <td>
                <h1><asp:TextBox ID="txtCodigoSKG" runat="server"></asp:TextBox>
                    <%--<asp:Label ID="lblNewCode" runat="server" Text="Codigo Nuevo" Visible="false"></asp:Label>--%></h1>
            </td>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Buscar" CausesValidation="false" />
            </td>
        </tr>

        <tr>
            <td>Comentarios</td>
            <td colspan="4">
                <asp:TextBox ID="txtComentarios" runat="server" AutoCompleteType="Disabled" Width="100%"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>

        <tr>
            <td class="auto-style1">
            </td>
            <td class="auto-style1">
                <center><asp:Button ID="btn1" runat="server" Text="Salir" Width="150px" CausesValidation="false"/></center>
            </td>
            <td class="auto-style1">
                &nbsp;</td>
            <td class="auto-style1">
                <center><asp:Button ID="btn2" runat="server" Text="Borrar Datos" Width="150px" CausesValidation="false"/></center>
            </td>
            <td class="auto-style1">
                <center><asp:Button ID="btn3" runat="server" Text="Crear Cliente" Width="150px" /></center>
            </td>
            <td class="auto-style1">
                &nbsp;</td>
        </tr>

    </table>
    </div>
    </center>
    <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    </form>
</body>
</html>
