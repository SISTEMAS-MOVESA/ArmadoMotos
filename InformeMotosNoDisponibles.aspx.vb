Imports System.IO
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web
Partial Class InformeMotosNoDisponibles
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Private Sub BindGrid()
        Try
            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)

                Dim sql_string As String = " SELECT T0.[ItemCode] " &
" , T0.[ItemName],T1.MnfSerial,T1.distnumber,t1.LotNumber,T5.Name [Marca],T4.NAME [Cilindros],T3.Name [Motor] " &
" ,T2.Name [Modelo],T6.Name [Color] " &
" ,(select whscode from OSRI with(nolock) where suppserial=t1.MnfSerial And ItemCode=t1.ItemCode)[WhsCode] " &
" ,(SELECT Owhs.[Whsname] FROM Owhs with(nolock) WHERE Owhs.[whscode]=(select whscode from OSRI with(nolock) where suppserial=t1.MnfSerial And ItemCode=t1.ItemCode) ) [Almacen] " &
" ,(SELECT TOP 1 DATEDIFF(DAY,T6.DocDate,GETDATE()) FROM SRI1 T6 WITH(NOLOCK) WHERE T1.SysNumber=T6.SysSerial And T0.ItemCode=T6.ItemCode  ORDER BY T6.DocDate DESC)[Dias]  " &
" From OITM T0 with(nolock) INNER Join OSRN T1 with(nolock) ON T0.ItemCode = T1.ItemCode   " &
" INNER Join [@AMODELO] T2 WITH(NOLOCK) ON T0.U_AMODELO=T2.CODE    " &
" INNER Join [@AMOTOR] T3 WITH(NOLOCK) ON T0.U_AMOTOR=T3.Code   " &
" INNER Join [@ACILINDROS] T4 WITH(NOLOCK) ON T0.U_ACILINDROS=T4.Code   " &
" INNER Join [@AMARCA] T5 WITH(NOLOCK) ON T0.U_AMARCA=T5.CODE  " &
" INNER Join [@SCOLOR] T6 WITH(NOLOCK) ON T0.U_ACOLOR=T6.CODE  " &
" WHERE t0.ItmsGrpCod = 154 And T1.[U_Estado_Produccion] ='05'"


                'Dim sql_string As String = " SELECT T0.[ItemCode],T0.[ItemName],T1.[SuppSerial],T1.[IntrSerial],t1.BatchId, " &
                '                        " T5.Name [Marca],T4.NAME [Cilindros], T3.Name [Motor],T2.Name [Modelo],T1.[WhsCode],T6.Name [Color], " &
                '                        " (SELECT Owhs.[Whsname] FROM Owhs with(nolock) WHERE Owhs.[whscode]=T1.[WhsCode] ) [Almacen] " &
                '                        " ,(SELECT TOP 1 DATEDIFF(DAY,T6.DocDate,GETDATE()) FROM SRI1 T6 WITH(NOLOCK) WHERE T1.SysSerial=T6.SysSerial AND T0.ItemCode=T6.ItemCode  ORDER BY T6.DocDate DESC)[Dias] " &
                '                        " FROM OITM T0 with(nolock) INNER JOIN OSRI T1 with(nolock) ON T0.ItemCode = T1.ItemCode " &
                '                        " INNER JOIN [@AMODELO] T2 WITH(NOLOCK) ON T0.U_AMODELO=T2.CODE  " &
                '                        " INNER JOIN [@AMOTOR] T3 WITH(NOLOCK) ON T0.U_AMOTOR=T3.Code " &
                '                        " INNER JOIN [@ACILINDROS] T4 WITH(NOLOCK) ON T0.U_ACILINDROS=T4.Code " &
                '                        " INNER JOIN [@AMARCA] T5 WITH(NOLOCK) ON T0.U_AMARCA=T5.CODE " &
                '                        " INNER JOIN [@SCOLOR] T6 WITH(NOLOCK) ON T0.U_ACOLOR=T6.CODE " &
                '                        " WHERE T1.[U_Estado_Produccion]='05'"

                Using cmd As New SqlCommand(sql_string)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GridView1.DataSource = dt
                            GridView1.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            GridView1.UseAccessibleHeader = True
            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Response.Write("BindGrid " & ex.Message)
        End Try
    End Sub

    Private Sub InformeMotosNoDisponibles_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    BindGrid()
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Protected Sub btnIngreso_Click(sender As Object, e As EventArgs) Handles btnIngreso.Click
        ExportGridToExcel()
    End Sub
    Private Sub ExportGridToExcel()
        Response.Clear()
        Response.Buffer = True
        Response.ClearContent()
        Response.ClearHeaders()
        Response.Charset = ""
        Dim FileName As String = "MotosNoDisponibles " & DateTime.Now & ".xls"
        Dim strwritter As StringWriter = New StringWriter()
        Dim htmltextwrtter As HtmlTextWriter = New HtmlTextWriter(strwritter)
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=" & FileName)
        GridView1.GridLines = GridLines.Both
        GridView1.HeaderStyle.Font.Bold = True
        GridView1.RenderControl(htmltextwrtter)
        Response.Write(strwritter.ToString())
        Response.[End]()
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        Try
            If e.CommandName = "Expediente" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = GridView1.Rows(index)
                Session("SerieExpediente") = GridView1.Rows(index).Cells(1).Text
                Response.Redirect("ExpedienteVehiculoGarantia.aspx")
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles btnFiltrar.Click
        pnlMotosListasParaArmado.Visible = True
        pnlGridControlCalidad.Visible = False
        BindGridMotosListasArmadoCompleto()
    End Sub

    Private Sub BindGridMotosListasArmadoCompleto()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand(" WITH RepuestosSeries  " &
                                                " AS " &
                                                " (SELECT [SERIEORIGEN],[ITEMCODE],[ITEMNAME],[QTY] " &
                                                " , case when (select onhand  " &
                                                " from movesa..OITW with(nolock) " &
                                                " where whscode='DCR00' AND ITEMCODE=[RETIROREPUESTOS].ITEMCODE collate Modern_Spanish_CI_AS)>[QTY] then [QTY] else  " &
                                                " (select onhand  " &
                                                " from movesa..OITW with(nolock)  " &
                                                " where whscode='DCR00' AND ITEMCODE=[RETIROREPUESTOS].ITEMCODE collate Modern_Spanish_CI_AS) end [DCR] " &
                                                " FROM [ArmadoMotos].[dbo].[RETIROREPUESTOS] WHERE [ESTATUS]='1' " &
                                                " ) " &
                                                " SELECT SerieOrigen FROM RepuestosSeries GROUP BY SerieOrigen having sum(qty)-sum(dcr)=0 ORDER BY SerieOrigen;")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GridMotosListasParaArmar.DataSource = dt
                            GridMotosListasParaArmar.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            GridMotosListasParaArmar.UseAccessibleHeader = True
            GridMotosListasParaArmar.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Response.Write("BindGridMotosListasArmadoCompleto " & ex.Message)
        End Try
    End Sub
    Protected Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Response.Redirect("InformeMotosNoDisponibles.aspx")
    End Sub

    Private Sub GridMotosListasParaArmar_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridMotosListasParaArmar.RowCommand
        Try
            If e.CommandName = "Expediente" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = GridMotosListasParaArmar.Rows(index)
                Session("SerieExpediente") = GridMotosListasParaArmar.Rows(index).Cells(1).Text

                Response.Write("<script>")
                Response.Write("window.open('ExpedienteVehiculoGarantia.aspx','_blank')")
                Response.Write("</script>")
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
