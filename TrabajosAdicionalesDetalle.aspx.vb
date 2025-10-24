Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
Imports System.Web.Services
Imports System.Web.UI.WebControls.Expressions
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Partial Class TrabajosAdicionalesDetalle
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=Movesa;uid=sa;password=M*l!n3r0s2k12"
    Private Sub TrabajosAdicionalesDetalle_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    'lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    'LoadGrid()
                    buscarDatosTrabajoAdicional(Request.QueryString("id"))

                End If
            Else
            End If
        Catch ex As Exception
            Response.Write("OrdendeTrabajo_Load " & ex.Message)
        End Try
    End Sub
    Public Sub buscarDatosTrabajoAdicional(id As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon2)
            Dim Consulta As String = " SELECT	T0.[ID],T0.[SERIE],T2.ITENMANE,T0.[IDTRABAJO],T1.[TRABAJO],T0.[DATECREATED] " &
                                        " From [ArmadoMotos].[dbo].[TRABAJOSADICIONALES] T0 " &
                                        " INNER JOIN [ArmadoMotos].[dbo].TRABAJOS T1 " &
                                        " On T0.IDTRABAJO = T1.ID INNER JOIN [ArmadoMotos].DBO.ARMADOMOTOS T2 " &
                                        " ON T0.SERIE=T2.SERIE " &
                                        " where t0.[ID]=" & id & " "

            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                txtId.Text = drd.Item("ID").ToString
                txtFecha.Text = drd.Item("DATECREATED").ToString
                txtSerie.Text = drd.Item("SERIE").ToString
                txtInfoMoto.Text = drd.Item("ITENMANE").ToString
                txtIdTrabajo.Text = drd.Item("IDTRABAJO").ToString
                txtDescripcionTrabajo.Text = drd.Item("TRABAJO").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

End Class
