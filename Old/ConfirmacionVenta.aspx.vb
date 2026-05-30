Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.Drawing
Imports System
Imports System.Web.Script.Services
Imports System.Web.Services
Imports SAPbobsCOM
Partial Class ConfirmacionVenta
    Inherits System.Web.UI.Page
    Public SCompany As SAPbobsCOM.Company
    Public oSBObob As SAPbobsCOM.SBObob
    Public oRecordSet As SAPbobsCOM.Recordset
    Public xCompany As SAPbobsCOM.Company
    Public lErrCode As Integer
    Public lRetCode As Integer
    Public sErrMsg As String
    Public oGoodsRcpt As SAPbobsCOM.Documents
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.60;database=PRUEBAS;uid=sa;password=S@pB1Sql"

    Protected Sub btnBuscarMoto_Click(sender As Object, e As EventArgs) Handles btnBuscarMoto.Click
        BuscarSerieMotoParaVenta(txtRecordId.Text)
    End Sub

    Public Sub Audit(_fecha As String, _proceso As String, _usuario As String, _observaciones As String)
        Try
            Dim query As String = String.Empty
            query &= " INSERT INTO [MovesaWeb].[dbo].[ENTRADAOPSKG_AUDIT] ([FECHA],[PROCESO],[USUARIO],[OBSERVACIONES]) " &
                      " VALUES (@p1,@p2,@p3,@p4) "
            Using conn As New SqlConnection(sCon1)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", _fecha)
                        .Parameters.AddWithValue("@p2", _proceso)
                        .Parameters.AddWithValue("@p3", _usuario)
                        .Parameters.AddWithValue("@p4", _observaciones)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            ADD_LOG("Audit", ex.Message)
        End Try
    End Sub

    Public Sub ADD_LOG(_PROCESO As String, _ERROR As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "INSERT INTO [MovesaWeb].[dbo].[PortalSKG] ([FECHA],[PROCESO],[ERROR])" &
                   " VALUES (@p1,@p2,@p3)"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", Date.Now)
                cmd.Parameters.AddWithValue("@p2", _PROCESO)
                cmd.Parameters.AddWithValue("@p3", _ERROR)
                con.Open()
                Dim t As Integer = cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception

        End Try
    End Sub

    Public Sub BuscarSerieMotoParaVenta(_SERIE As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = "SELECT ID,CARDNAME,IDENTIDAD,MODELO,COLOR,SERIE,SERIEM " &
                                       " FROM [movesaweb].[dbo].[ENTRADAOPSKG] WITH(NOLOCK) WHERE SERIE='" & _SERIE & "'"

            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                txtModelo.Text = drd.Item("Modelo").ToString
                txtSerie.Text = drd.Item("Serie").ToString
                txtSerieM.Text = drd.Item("SerieM").ToString
            End If
            Audit(Date.Now, "BuscarSerieMotoParaVenta", Session("User").ToString, "Buscar: " & _SERIE)

        Catch ex As Exception
            ADD_LOG("BuscarSerieMotoParaVenta", ex.Message)
            Response.Write(ex.Message)
        End Try
    End Sub

    Public Sub Cargar_Imagen_Moto()
        If fuImage1.HasFile Then
            Try
                Dim filename As String = Path.GetFileName(fuImage1.FileName)
                fuImage1.SaveAs(Server.MapPath("~/ImagenesVenta/") & txtSerie.Text & "_FRENTE_" & System.IO.Path.GetExtension(fuImage1.FileName))
                Session("Img1") = "~/ImagenesVenta/" & txtSerie.Text & "_FRENTE_" & System.IO.Path.GetExtension(fuImage1.FileName)
            Catch ex As Exception
                Response.Write("Ocurrio un error al intentar subir la foto" + ex.Message)
                ADD_LOG("fuImage1", ex.Message)
            End Try
        End If

        If fuImage2.HasFile Then
            Try
                Dim filename As String = Path.GetFileName(fuImage2.FileName)
                fuImage2.SaveAs(Server.MapPath("~/ImagenesVenta/") & txtSerie.Text & "_ATRAS_" & System.IO.Path.GetExtension(fuImage2.FileName))
                Session("Img2") = "~/ImagenesVenta/" & txtSerie.Text & "_ATRAS_" & System.IO.Path.GetExtension(fuImage2.FileName)
            Catch ex As Exception
                Response.Write("Ocurrio un error al intentar subir la foto" + ex.Message)
                ADD_LOG("fuImage2", ex.Message)
            End Try
        End If

        If fuImage3.HasFile Then
            Try
                Dim filename As String = Path.GetFileName(fuImage3.FileName)
                fuImage3.SaveAs(Server.MapPath("~/ImagenesVenta/") & txtSerie.Text & "_DERECHA_" & System.IO.Path.GetExtension(fuImage3.FileName))
                Session("Img3") = "~/ImagenesVenta/" & txtSerie.Text & "_DERECHA_" & System.IO.Path.GetExtension(fuImage3.FileName)
            Catch ex As Exception
                Response.Write("Ocurrio un error al intentar subir la foto" + ex.Message)
                ADD_LOG("fuImage3", ex.Message)
            End Try
        End If

        If fuImage4.HasFile Then
            Try
                Dim filename As String = Path.GetFileName(fuImage4.FileName)
                fuImage4.SaveAs(Server.MapPath("~/ImagenesVenta/") & txtSerie.Text & "_IZQUIERDA_" & System.IO.Path.GetExtension(fuImage4.FileName))
                Session("Img4") = "~/ImagenesVenta/" & txtSerie.Text & "_IZQUIERDA_" & System.IO.Path.GetExtension(fuImage4.FileName)
            Catch ex As Exception
                Response.Write("Ocurrio un error al intentar subir la foto" + ex.Message)
                ADD_LOG("fuImage4", ex.Message)
            End Try
        End If

        If fuImage5.HasFile Then
            Try
                Dim filename As String = Path.GetFileName(fuImage5.FileName)
                fuImage5.SaveAs(Server.MapPath("~/ImagenesVenta/") & txtSerie.Text & "_TACOMETRO_" & System.IO.Path.GetExtension(fuImage5.FileName))
                Session("Img5") = "~/ImagenesVenta/" & txtSerie.Text & "_TACOMETRO_" & System.IO.Path.GetExtension(fuImage5.FileName)
            Catch ex As Exception
                Response.Write("Ocurrio un error al intentar subir la foto" + ex.Message)
                ADD_LOG("fuImage5", ex.Message)
            End Try
        End If

        If fuImage6.HasFile Then
            Try
                Dim filename As String = Path.GetFileName(fuImage6.FileName)
                fuImage6.SaveAs(Server.MapPath("~/ImagenesVenta/") & txtSerie.Text & "_ARRIBA_" & System.IO.Path.GetExtension(fuImage6.FileName))
                Session("Img6") = "~/ImagenesVenta/" & txtSerie.Text & "_ARRIBA_" & System.IO.Path.GetExtension(fuImage6.FileName)
            Catch ex As Exception
                Response.Write("Ocurrio un error al intentar subir la foto" + ex.Message)
                ADD_LOG("fuImage6", ex.Message)
            End Try
        End If
    End Sub

    Private Sub ConfirmacionVenta_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
            'GESTORC
            If Session("Position").ToString = "GESTORC" Then
                Response.Redirect("Menu.aspx")
            End If
            'SKA1001
            If Session("Position").ToString = "ADMIN" Then
                Response.Redirect("Menu.aspx")
            End If
            'VENDEDOR
            If Session("Position").ToString = "VENDEDOR" Then
                pnlSubMenuConfirmacion.Visible = False
                pnlSubMenuOrdenRecuperacion.Visible = False
                pnlSubMenuConfirmacionVenta.Visible = True
                pnlSubMenuPrecios.Visible = False
                pnlSubMenuReconciliaciones.Visible = False
            End If
            If Session("SerialNumberPhotoTwo").ToString() = "NULL" Then
            Else
                txtRecordId.Text = Session("SerialNumberPhotoTwo").ToString()
                BuscarSerieMotoParaVenta(Session("SerialNumberPhotoTwo").ToString())
            End If
        Catch ex As Exception
            ADD_LOG("Page_Load", ex.Message)
        End Try
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        If txtEstadoMoto.Text = "" Then
            Response.Write("<script>alert('Debe Debe Agregar los Comentarios de Reparaciones o Preparacion de la Moto!!!, Antes de Continuar');</script>")
            Exit Sub
        End If

        Cargar_Imagen_Moto()
        INSERT_ENTRADAOPSKG_PHOTOS_TWO(Date.Now, txtRecordId.Text, Session("Img1").ToString, Session("Img2").ToString,
                                       Session("Img3").ToString, Session("Img4").ToString, Session("Img5").ToString,
                                       Session("Img6").ToString, Session("User"), txtEstadoMoto.Text)
    End Sub
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Response.Redirect("ConfirmacionVenta.aspx")
    End Sub
    Public Sub INSERT_ENTRADAOPSKG_PHOTOS_TWO(_CREATEDATE As DateTime, _SERIE As String, _IMG01 As String, _IMG02 As String, _IMG03 As String, _IMG04 As String _
                                  , _IMG05 As String, _IMG06 As String, _USUARIO As String, _COMENTRARIOSFINALES As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "IF EXISTS (SELECT 1 FROM [MovesaWeb].[dbo].[ENTRADAOPSKG_PHOTOS_TWO] WHERE [SERIE] = @p2) " &
                  "   BEGIN " &
                  "       UPDATE [MovesaWeb].[dbo].[ENTRADAOPSKG_PHOTOS_TWO] " &
                  "       Set " &
                  "       [CREATEDATE]=@p1,[IMG1]=@p3,[IMG2]=@p4,[IMG3]=@p5,[IMG4]=@p6,[IMG5]=@p8,[IMG6]=@p9,[VIDEO]=@p10,[USUARIO]=@p11,[COMENTARIOSFINALES]=@p12 " &
                  "   End " &
                  "   ELSE " &
                  "   BEGIN " &
                  "          INSERT INTO [MovesaWeb].[dbo].[ENTRADAOPSKG_PHOTOS_TWO] " &
                  "          ([CREATEDATE],[SERIE],[IMG1],[IMG2],[IMG3],[IMG4],[IMG5],[IMG6],[VIDEO],[USUARIO],[COMENTARIOSFINALES]) " &
                  "          VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p8,@p9,@p10,@p11,@p12) " &
                  "   END"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _CREATEDATE)
                cmd.Parameters.AddWithValue("@p2", _SERIE)
                cmd.Parameters.AddWithValue("@p3", _IMG01)
                cmd.Parameters.AddWithValue("@p4", _IMG02)
                cmd.Parameters.AddWithValue("@p5", _IMG03)
                cmd.Parameters.AddWithValue("@p6", _IMG04)
                cmd.Parameters.AddWithValue("@p8", _IMG05)
                cmd.Parameters.AddWithValue("@p9", _IMG06)
                cmd.Parameters.AddWithValue("@p10", "ImagenesVideo/" & MyFileUpload.PostedFile.FileName)
                cmd.Parameters.AddWithValue("@p11", _USUARIO)
                cmd.Parameters.AddWithValue("@p12", _COMENTRARIOSFINALES)
                con.Open()
                Dim t As Integer = cmd.ExecuteNonQuery()
                con.Close()
            End Using
            Audit(Date.Now, "INSERT_ENTRADAOPSKG_PHOTOS_TWO", Session("User").ToString, "Serie: #" & _SERIE)
        Catch ex As Exception
            ADD_LOG("INSERT_ENTRADAOPSKG_PHOTOS_TWO", ex.Message & " Serie: #" & _SERIE)
        End Try
    End Sub
End Class
