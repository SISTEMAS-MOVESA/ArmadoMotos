Imports System.Data
Imports System.Data.SqlClient
Partial Class TrasladosDashboardTransito
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public MyTable As New DataTable
    Public SQL_STRING As String
    Public sql_consulta As String

    Private Sub TrasladosDashboardTransito_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    BindgridSugerido()
                End If
            Else
            End If
            'lblDiunsa.Text = obtenerPedidosPendientesDiunsa()
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
            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)


                SQL_STRING = "  SELECT	T1.ItemCode " &
                             "          ,T1.ItemName " &
                             "          ,T0.SuppSerial " &
                             "          ,t0.WhsCode " &
                             "          ,t2.WhsName " &
                             "          ,(SELECT TOP 1 DATEDIFF(DAY,T6.DocDate,GETDATE()) FROM SRI1 T6 WITH(NOLOCK) WHERE T6.SysSerial=t0.SysSerial AND T0.ItemCode=T6.ItemCode  ORDER BY T6.DocDate DESC)[Dias] " &
                             "          ,(SELECT TOP 1 convert(char,T6.DocDate,103) FROM SRI1 T6 WITH(NOLOCK) WHERE T6.SysSerial=t0.SysSerial AND T0.ItemCode=T6.ItemCode  ORDER BY T6.DocDate DESC)[Fecha] " &
                             "    FROM	OSRI T0 WITH(NOLOCK) " &
                             "          INNER JOIN OITM T1 WITH(NOLOCK) " &
                             "          ON T0.ItemCode=T1.ItemCode " &
                             "          INNER JOIN  " &
                             "          OWHS T2 WITH(NOLOCK) " &
                             "          ON T0.WhsCode=T2.WhsCode " &
                             "    WHERE	T0.STATUS=0 " &
                             "          AND T1.ITMSGRPCOD=154 " &
                             "          AND LEFT(T0.WhsCode,1)='T' " &
                             "          AND T2.U_Type='PRO'"

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

            Dim cantidad_redflags As Integer = 0
            For Each row As GridViewRow In gridIndiceD.Rows

                If row.Cells(5).Text >= 3 Then
                    row.Cells(6).BackColor = System.Drawing.Color.LightCoral
                    cantidad_redflags += 1
                End If
            Next

            lblPercent.Text = FormatPercent(cantidad_redflags / gridIndiceD.Rows.Count, 2, TriState.True)
            lblTotalTransitos.Text = gridIndiceD.Rows.Count
            lblMotosTresDias.Text = cantidad_redflags
            Response.Write("<script>console.log('Redflags: " & ReplaceCharsForFileName(cantidad_redflags, " ") & "');</script>")


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
