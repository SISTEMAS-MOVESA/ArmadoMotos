Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Partial Class _Default
    Inherits System.Web.UI.Page

    Public sCon1 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    'Private Function YourValidationFunction(ByVal UserName As String, ByVal Password As String) As Boolean
    '    Dim boolReturnValue As Boolean = False
    '    Dim strConnection As String = sCon1
    '    Dim sqlConnection As SqlConnection = New SqlConnection(strConnection)
    '    Dim SQLQuery As String = "SELECT * FROM [ArmadoMotos].[DBO].[USUARIOS] with(nolock) where [USERCODE]='" & Replace(UserName, ";", "--") & "'"
    '    Dim command As SqlCommand = New SqlCommand(SQLQuery, sqlConnection)
    '    Dim Dr As SqlDataReader
    '    sqlConnection.Open()
    '    Dr = command.ExecuteReader()
    '    While Dr.Read()
    '        Session("UserCode") = Dr("ID").ToString.Trim
    '        Session("IdMecanico") = Dr("IDMECANICO").ToString.Trim
    '        Session("User") = UserName
    '        Session("Name") = Dr("USERNAME").ToString.Trim
    '        Session("Position") = Dr("USERROL").ToString.Trim
    '        Session("TaskNumber") = "NULL"
    '        Session("SerieCalidad") = "NULL"
    '        Session("SerieExpediente") = "NULL"
    '        Session("USERROL") = Dr("USERROL").ToString.Trim
    '        If (UserName = Dr("USERCODE").ToString.Trim) And (Password = Dr("USERPASS").ToString.Trim) Then
    '            boolReturnValue = True
    '        End If
    '        Dr.Close()
    '        sqlConnection.Close()
    '        Return boolReturnValue
    '    End While
    '    Return boolReturnValue
    'End Function
    Protected Sub btnlogin_Click(sender As Object, e As EventArgs) Handles btnlogin.Click
        Try
            If ValidateUserAndPassword(UCase(txtusuario.Text), txtpassword.Text) Then
                'Response.Write("<script>console.log('" & Session("USERROL") & "');</script>")
                'Exit Sub

                Select Case Session("USERROL")
                    Case "Jefe Tienda"
                        Response.Redirect("MaindashboardSucursales.aspx")
                    Case "Supervisor"
                        Response.Redirect("SupervisoresIndiceGeneral.aspx")
                    Case "Placas"
                        Response.Redirect("CrearFreservaPlacas.aspx")
                    Case Else
                        Response.Redirect("Maindashboard.aspx")

                End Select

                'If Session("USERROL") = "Jefe Tienda" Then
                '    Response.Redirect("MaindashboardSucursales.aspx")
                'Else
                '    Response.Redirect("Maindashboard.aspx")
                'End If
            Else
                Response.Write("<script language=javascript>alert('Usuario o Password Incorrecto!!!');</script>")
            End If
        Catch ex As Exception
            Response.Write("btnLogin_Click " & ex.Message)
        End Try
    End Sub
    Private Function ValidateUserAndPassword(ByVal UserName As String, ByVal Password As String) As Boolean
        Dim boolReturnValue As Boolean = False
        Dim strConnection As String = sCon1
        Dim sqlConnection As SqlConnection = New SqlConnection(strConnection)
        Dim SQLQuery As String = "SELECT * FROM [ArmadoMotos].[DBO].[USUARIOS] with(nolock) where [USERCODE]='" & Replace(UserName, ";", "--") & "'"
        Dim command As SqlCommand = New SqlCommand(SQLQuery, sqlConnection)
        Dim Dr As SqlDataReader
        sqlConnection.Open()
        Dr = command.ExecuteReader()
        While Dr.Read()
            Session("UserCode") = Dr("ID").ToString.Trim
            Session("IdMecanico") = Dr("IDMECANICO").ToString.Trim
            Session("User") = UserName
            Session("Name") = Dr("USERNAME").ToString.Trim
            Session("Position") = Dr("USERROL").ToString.Trim
            Session("TaskNumber") = "NULL"
            Session("SerieCalidad") = "NULL"
            Session("SerieExpediente") = "NULL"
            Session("USERROL") = Dr("USERROL").ToString.Trim
            Session("USEREMAIL") = Dr("USEREMAIL").ToString.Trim
            Session("ALMTRANSIT") = Dr("SUCURSAL").ToString.Trim
            Session("CODIGOSUP") = Dr("CODIGOSUP").ToString.Trim
            If (Password = Dr("USERPASS").ToString.Trim) Then 'Getmd5(Dr("USERPASS").ToString.Trim)) Then
                boolReturnValue = True
            End If
            Dr.Close()
            Return boolReturnValue
        End While
        Return boolReturnValue
    End Function
End Class