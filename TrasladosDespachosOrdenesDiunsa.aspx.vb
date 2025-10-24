Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Web.Script.Serialization
Imports Newtonsoft.Json
Partial Class TrasladosDespachosOrdenesDiunsa
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public MyTable As New DataTable
    Public SQL_STRING As String
    Public sql_consulta As String

    Private Sub TrasladosDespachosOrdenesDiunsa_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try

            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    BindgridSugerido()
                End If
            Else
            End If
            lblDiunsa.Text = obtenerPedidosPendientesDiunsa()
        Catch ex As Exception
            Response.Write("<script>console.log('VisorLogistica_Load: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function obtenerPedidosPendientesDiunsa() As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "SELECT COUNT(DOCNUM)AS PEDIDOS_PENDIENTES FROM MOVESAWEB..TBL_REPORTES_VENTAS_DISTRIBUIDORES AS T0 WHERE T0.CARDCODE = 'CL104628' AND DOCTYPE = 'OP' AND T0.DOCSTATUS IN ('O')"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()

            Return t
        End Using
    End Function
    Private Sub BindgridSugerido()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)


                SQL_STRING = "SELECT T0.DOCNUM, T0.PARTNER_DOCNUM, T0.DOCSTATUS, FORMAT(T0.DOCDATE,'yyyy-MM-dd') AS DOCDATE, " &
                            " T0.WHSCODE, T2.WHSNAME, T0.ITEMCODE, T1.ITEMNAME,  " &
                            " FORMAT(T0.ETA,'yyyy-MM-dd') AS ETA, T0.COMMENTS " &
                            " From MOVESAWEB..TBL_REPORTES_VENTAS_DISTRIBUIDORES AS T0 " &
                            " INNER JOIN MOVESA..OITM AS T1 ON T1.ITEMCODE = T0.ITEMCODE COLLATE SQL_Latin1_General_CP1_CI_AS " &
                            " INNER Join MOVESA..OWHS AS T2 ON T2.WHSCODE = T0.WHSCODE COLLATE SQL_Latin1_General_CP1_CI_AS " &
                            " WHERE T0.CARDCODE = 'CL104628' AND DOCTYPE = 'OP' AND T0.DOCSTATUS IN ('O') " &
                            " ORDER BY T0.DOCSTATUS DESC, T0.DOCDATE "

                Using cmd As New SqlCommand(SQL_STRING)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridIndiceD.DataSource = dt
                            gridIndiceD.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridIndiceD.UseAccessibleHeader = True
            gridIndiceD.HeaderRow.TableSection = TableRowSection.TableHeader

        Catch ex As Exception
            Response.Write("<script>console.log('BindgridSugerido: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
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

End Class
