<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Test.aspx.vb" Inherits="Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>SandBox</title>

</head>
<body>
    <form id="form1" runat="server">


        <asp:Button ID="btnCargarSugerido" runat="server" class="btn btn-warning" Text="Enviar Prueba" Width="10%" Height="100%" />
        <asp:label ID="lblError" runat="server"></asp:label>

     <asp:ScriptManager ID="sm1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    </form>
</body>
</html>


