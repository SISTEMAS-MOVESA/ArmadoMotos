Imports System.Data
Imports System.Data.SqlClient

Partial Class VisorLogistica
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public MyTable As New DataTable
    Public csvFilePath As String
    Public SQL_STRING As String
    Private Sub VisorLogistica_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    'BindgridSugerido(Request.QueryString("id"))
                    lblIdMacro.Text = Request.QueryString("id")
                    lblAlmacenDestino.Text = Request.QueryString("destino") & "  " & GetWhsName(Request.QueryString("destino"))
                    lblRuta.Text = Request.QueryString("ruta")
                    BindgridSugeridoHistorial(Request.QueryString("id"))
                    Page.Title = "Solicitud " & Request.QueryString("id") & " Destino " & Request.QueryString("destino") & " " & GetWhsName(Request.QueryString("destino"))
                End If
            Else
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('VisorLogistica_Load: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub BindgridSugeridoHistorial(macroid As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                SQL_STRING = "SELECT WHSCODE,WHSNAME,CODIGOCLIENTE,ITEMCODE,MODELO,DESCRIPCION,OBSERVACIONES " &
                " ,QTYLOG,QTYSUC,QTYFINAL " &
                " FROM ArmadoMotos.dbo.SOLCITUD_HEADER where MACROID = @macroId"
                Using cmd As New SqlCommand(SQL_STRING)
                    cmd.Parameters.AddWithValue("@macroId", macroid)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            grisHistorial.DataSource = dt
                            grisHistorial.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            grisHistorial.UseAccessibleHeader = True
            grisHistorial.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Response.Write("<script>console.log('BindgridSugeridoHistorial: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Protected Function EscapeJavaScriptString(ByVal str As String) As String
        Return str.Replace("'", "\'").Replace("""", "\""")
    End Function
    Public Function GetWhsName(whscode As String) As String
        Dim sCon As String = sCon1
        Dim sel As String
        sel = "SELECT whsname from owhs with(nolock) where whscode=@p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", whscode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('GetEspacios: " & t & "');</script>")
        End Using
    End Function
    Public Function ReplaceCharsForFileName(sName As String, sChr As String) As String
        Dim NewStr As String
        sName = Replace(sName, "/", sChr)
        sName = Replace(sName, "\", sChr)
        sName = Replace(sName, ":", sChr)
        sName = Replace(sName, "?", sChr)
        sName = Replace(sName, Chr(34), sChr)
        sName = Replace(sName, "<", sChr)
        sName = Replace(sName, ">", sChr)
        sName = Replace(sName, "|", sChr)
        sName = Replace(sName, "&", sChr)
        sName = Replace(sName, "%", sChr)
        sName = Replace(sName, "*", sChr)
        sName = Replace(sName, "'", sChr)
        sName = Replace(sName, "{", sChr)
        sName = Replace(sName, "[", sChr)
        sName = Replace(sName, "]", sChr)
        sName = Replace(sName, "}", sChr)
        sName = Replace(sName, "!", sChr)
        sName = Replace(sName, ",", sChr)
        NewStr = sName
        Return NewStr
    End Function
    Public Sub EliminarLinea(idlinea As Integer)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "DELETE FROM [PRESOLICITUD_LINES] where [ID] = @p1"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", idlinea)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('success','Moto Eliminada con Exito!<hr> EliminarLinea');", True)
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('EliminarLinea: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub SOLICITUD_HEADER(WHSCODEORIGEN As String, WHSNAMEORIGEN As String, WHSCODE As String, WHSNAME As String, CODIGOCLIENTE As String,
                                CODEMODELO As String, MODELO As String, QTYSOLICITADA As Integer,
                                QTYPREPARADA As Integer, OBSERVACIONES As String, USERCREATED As String, DATECREATED As DateTime,
                                ESTATUS As String, FECHA_ARMADO As DateTime, FECHA_ENTREGA As DateTime, DESCRIPCION As String,
                                PREIDHEADER As String, PRELIDHEADER As String, ITEMCODE As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = " INSERT INTO [dbo].[SOLCITUD_HEADER]([WHSCODEORIGEN],[WHSNAMEORIGEN],[WHSCODE],[WHSNAME],[CODIGOCLIENTE]," &
                  " [CODEMODELO],[MODELO],[QTYSOLICITADA],[QTYPREPARADA],[OBSERVACIONES],[USERCREATED],[DATECREATED]," &
                  " [ESTATUS],[FECHAARMADO],[FECHAENTREGA],[DESCRIPCION],[PREIDHEADER],[PRELIDHEADER],[MACROID],[ITEMCODE])" &
                    " VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17,@p18,@p19,@p20)"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", WHSCODEORIGEN)
                cmd.Parameters.AddWithValue("@p2", WHSNAMEORIGEN)
                cmd.Parameters.AddWithValue("@p3", WHSCODE)
                cmd.Parameters.AddWithValue("@p4", WHSNAME)
                cmd.Parameters.AddWithValue("@p5", CODIGOCLIENTE)
                cmd.Parameters.AddWithValue("@p6", CODEMODELO)
                cmd.Parameters.AddWithValue("@p7", MODELO)
                cmd.Parameters.AddWithValue("@p8", QTYSOLICITADA)
                cmd.Parameters.AddWithValue("@p9", QTYPREPARADA)
                cmd.Parameters.AddWithValue("@p10", OBSERVACIONES)
                cmd.Parameters.AddWithValue("@p11", USERCREATED)
                cmd.Parameters.AddWithValue("@p12", DATECREATED)
                cmd.Parameters.AddWithValue("@p13", ESTATUS)
                cmd.Parameters.AddWithValue("@p14", FECHA_ARMADO)
                cmd.Parameters.AddWithValue("@p15", FECHA_ENTREGA)
                cmd.Parameters.AddWithValue("@p16", DESCRIPCION)
                cmd.Parameters.AddWithValue("@p17", PREIDHEADER)
                cmd.Parameters.AddWithValue("@p18", PRELIDHEADER)
                cmd.Parameters.AddWithValue("@p19", Request.QueryString("id"))
                cmd.Parameters.AddWithValue("@p20", ITEMCODE)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('success','Solicitud Guardada con Exito!<hr> SOLICITUD_HEADER');", True)
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('SOLICITUD_HEADER: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub UpdateNextCamion(camion As String, usuario As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "update [ArmadoMotos].[dbo].[SOLCITUD_HEADER] Set CAMION=@p1, [ESTATUS] ='CAMION' where [ESTATUS] = 'SOLICITADO' AND [USERCREATED]=@p2"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", camion)
                cmd.Parameters.AddWithValue("@p2", usuario)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("<script>alert('UpdateNextCamion : " & EscapeJavaScriptString(ex.Message) & "');</script>")
        End Try
    End Sub
    Public Sub UpdateMacroHeader(estado As String, idmacroheader As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "update [ArmadoMotos].[dbo].[MACROINSERT_HEADER] Set ESTADO=@p1 where [ID]=@p2"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", estado)
                cmd.Parameters.AddWithValue("@p2", idmacroheader)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("<script>alert('UpdateNextCamion : " & EscapeJavaScriptString(ex.Message) & "');</script>")
        End Try
    End Sub
    Public Sub UpdatePreSolicitud(headerid As String, camion As String, usuario As String, lineheaderid As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "update [dbo].[PRESOLICITUD_HEADER] set ESTATUS='PROCESADO', CAMION=@p2, USERUPDATED=@p3,DATEUPDATED=getdate() where ID=@p1;" &
                "update [dbo].[PRESOLICITUD_LINES] set ESTADO='PROCESADO', USERUPDATED=@p3 where ID=@p4"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", headerid)
                cmd.Parameters.AddWithValue("@p2", camion)
                cmd.Parameters.AddWithValue("@p3", usuario)
                cmd.Parameters.AddWithValue("@p4", lineheaderid)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("<script>alert('UpdatePreSolicitud : " & EscapeJavaScriptString(ex.Message) & "');</script>")

        End Try
    End Sub
    Public Function GetNextCamion() As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "SELECT ISNULL(MAX([CAMION]),0)+1[Camion] FROM [ArmadoMotos].[dbo].[SOLCITUD_HEADER]"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
        End Using
    End Function
    Public Function GetCodigoModelo(modelo As String) As String
        Dim sCon As String = sCon1
        Dim sel As String
        sel = "select code from movesa..[@AMODELO] where [Name]='" & modelo & "'"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
        End Using
    End Function

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Try
            Response.Redirect("TrasladosDashboard.aspx")
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
End Class
