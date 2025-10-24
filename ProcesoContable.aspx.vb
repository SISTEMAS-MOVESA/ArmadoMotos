Imports System.IO
Imports System.Linq
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web
Imports SAPbobsCOM
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class ProcesoContable
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=Movesa;uid=sa;password=M*l!n3r0s2k12"

    Public SCompany As SAPbobsCOM.Company
    Public oSBObob As SAPbobsCOM.SBObob
    Public oRecordSet As SAPbobsCOM.Recordset
    Public xCompany As SAPbobsCOM.Company
    Public lErrCode As Integer
    Public lRetCode As Integer
    Public sErrMsg As String
    Public oGoodsRcpt As SAPbobsCOM.Documents

    Public Sub _CargarContratistas()
        Try
            Dim dt As New DataTable()
            Using conn As New SqlConnection(sCon1)
                Dim query As String = "SELECT [GRUPORESPONSABLE] ,[ID][BPCODE] FROM [ArmadoMotos].[dbo].[GRUPOS]"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                drpContratista.Dispose()
                drpContratista.DataTextField = "GRUPORESPONSABLE"
                drpContratista.DataValueField = "BPCODE"
                drpContratista.DataSource = dt
                drpContratista.DataBind()
                conn.Close()
            End Using
            BindGrid(drpContratista.SelectedValue.ToString())
        Catch ex As Exception
            'ADD_LOG("_CargarAlmacenes", ex.Message)
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub ProcesoContable_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    _CargarContratistas()
                    Select Case Session("Position").ToString()
                        Case "Iniciador"
                            Response.Redirect("MainDashBoard.aspx")
                        Case "Supervisor Armado"
                            Response.Redirect("MainDashBoard.aspx")
                        Case "Calidad"
                            Response.Redirect("MainDashBoard.aspx")
                    End Select
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub BindGrid(_BPCODE As String)
        Try
            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT t0.[ID], t0.[SERIE],t0.[MODELO],t0.[COLOR],t1.[ID] [Mecanico],t2.BPCODE,T2.GRUPORESPONSABLE,'_SYS00000004238' [Cuenta] " &
                                            ",(select CONVERT(DECIMAL(19,2),U_Precio) from [movesa].[dbo].[@amodelo] where name=t0.[modelo] collate Modern_Spanish_CI_AS) [Precio] " &
                                            " From [ArmadoMotos].[dbo].[ARMADOMOTOS] T0 INNER JOIN [ArmadoMotos].[dbo].[mecanicos] T1 " &
                                            " on t0.MECANICOASIGNADO=t1.ID inner join [ArmadoMotos].[dbo].[GRUPOS] T2 on t1.GRUPOID=t2.ID " &
                                            " where LIQUIDACIONID=0 and T0.[CONTROL1]='ARMADA' AND T0.[CONTROL2]='PP' AND T2.[ID] ='" & _BPCODE & "' ORDER BY t0.[MODELO]")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GridView1.DataSource = dt
                            GridView1.DataBind()
                            con.Close()
                            Dim total As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("Precio"))
                            GridView1.FooterRow.Cells(1).Text = "Total Motos: " & dt.Rows.Count
                            GridView1.FooterRow.Cells(1).Font.Bold = True
                            GridView1.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
                            GridView1.FooterRow.Cells(7).Text = "Monto a Pagar: " & total.ToString("N2")
                            GridView1.FooterRow.Cells(7).Font.Bold = True
                            GridView1.UseAccessibleHeader = True
                            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
                        End Using
                    End Using
                End Using
            End Using
        Catch ex As Exception
            'Response.Write("BindGrid " & ex.Message)
        End Try
    End Sub

    Private Sub drpContratista_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpContratista.SelectedIndexChanged
        'btnExportarExcel.Visible = False
        gridPrecedentes.Dispose()
        gridPrecedentes.DataSource = Nothing
        gridPrecedentes.Visible = False
        BindGrid(drpContratista.SelectedValue.ToString())
    End Sub

    'Public Function GlobalConnecttoSAP() As Integer
    '    SCompany = New SAPbobsCOM.Company With {
    '        .Server = "192.168.1.3",
    '        .CompanyDB = "MOVESA",
    '        .DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012,
    '        .DbUserName = "sa",
    '        .DbPassword = "M*l!n3r0s2k12",
    '        .UserName = "it",
    '        .Password = "polar",
    '        .language = SAPbobsCOM.BoSuppLangs.ln_Spanish_La,
    '        .SLDServer = "192.168.1.9:40000"
    '    }
    '    lRetCode = SCompany.Connect
    '    Return lRetCode
    'End Function
    Public Function CreateNewLiquidation(_fecha As Date, _cantidad As Integer, _monto As Decimal, _usercode As String) As Integer
        Dim sCon As String = sCon1
        Dim sel As String
        sel = "INSERT INTO [dbo].[LIQUIDACIONES] ([FECHACREACION],[CANTIDAD],[MONTO],[USERCODE]) " &
                " VALUES (@p1,@p2,@p3,@p4);Select CAST(scope_identity() As int)"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", _fecha)
            cmd.Parameters.AddWithValue("@p2", _cantidad)
            cmd.Parameters.AddWithValue("@p3", _monto)
            cmd.Parameters.AddWithValue("@p4", _usercode)
            con.Open()
            Dim t As Integer = cmd.ExecuteScalar()
            con.Close()
            Return t
        End Using
    End Function

    Public Sub CreateNewLiquidationDetalle(_LIQUIDACIONID As String, _IDCALIDAD As String, _SERIE As String, _MODELO As String, _COLOR As String,
                                            _MECANICOID As String, _CPROVEEDOR As String, _NPROVEEDOR As String, _CUENTAC As String, _PRECIOARMADO As String,
                                           _FECHACREACION As Date, _USUARIOCREACION As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = " INSERT INTO [dbo].[MOTOSLIQ]([LIQUIDACIONID],[IDCALIDAD],[SERIE],[MODELO],[COLOR],[MECANICOID] " &
                " ,[CPROVEEDOR],[NPROVEEDOR],[CUENTAC],[PRECIOARMADO],[FECHACREACION],[USUARIOCREACION]) " &
                " VALUES (@P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8,@P9,@P10,@P11,@P12)"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _LIQUIDACIONID)
                cmd.Parameters.AddWithValue("@p2", _IDCALIDAD)
                cmd.Parameters.AddWithValue("@p3", _SERIE)
                cmd.Parameters.AddWithValue("@p4", _MODELO)
                cmd.Parameters.AddWithValue("@p5", _COLOR)
                cmd.Parameters.AddWithValue("@p6", _MECANICOID)
                cmd.Parameters.AddWithValue("@p7", _CPROVEEDOR)
                cmd.Parameters.AddWithValue("@p8", _NPROVEEDOR)
                cmd.Parameters.AddWithValue("@p9", _CUENTAC)
                cmd.Parameters.AddWithValue("@p10", _PRECIOARMADO)
                cmd.Parameters.AddWithValue("@p11", _FECHACREACION)
                cmd.Parameters.AddWithValue("@p12", _USUARIOCREACION)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using

            If _CPROVEEDOR = "PL0000000" Then
                CloseLocalLiquidation(_LIQUIDACIONID)
            End If
        Catch ex As Exception
            Response.Write("CreateNewLiquidationDetalle " & ex.Message)
        End Try
    End Sub

    Public Sub UpdateMaintableLiquidacion(_LIQUIDACIONID As String, _FECHALIQUIDACION As Date, _SERIE As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "UPDATE [dbo].[ARMADOMOTOS] " &
                    " Set [LIQUIDACIONID] = @p1 " &
                    " ,[FECHALIQUIDACION] = @p2 " &
                    " WHERE [SERIE] = @p3"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _LIQUIDACIONID)
                cmd.Parameters.AddWithValue("@p2", _FECHALIQUIDACION)
                cmd.Parameters.AddWithValue("@p3", _SERIE)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("UpdateMaintableLiquidacion " & ex.Message)
        End Try
    End Sub

    Public Sub CloseLocalLiquidation(_LIQUIDACIONID As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "update LIQUIDACIONES SET CANCELED='Y' WHERE ID=@p1 "

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _LIQUIDACIONID)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("CloseLocalLiquidation " & ex.Message)
        End Try
    End Sub

    Private Sub ExportGridToExcel()
        Response.Clear()
        Response.Buffer = True
        Response.ClearContent()
        Response.ClearHeaders()
        Response.Charset = ""
        Dim FileName As String = "Proforma " & drpContratista.SelectedItem.ToString & " " & DateTime.Now & ".xls"
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
    Protected Sub btnCrearLiquidacion_Click(sender As Object, e As EventArgs) Handles btnCrearLiquidacion.Click
        Try
            'pnlGridPrincipal.Visible = False
            'pnlReturn.Visible = True

            Dim montototal As Decimal
            Dim cantidadtotal As Integer
            Dim NumeroLiquidacionRuntime As Integer
            montototal = 0
            cantidadtotal = 0
            For Each row As GridViewRow In GridView1.Rows
                montototal = montototal + row.Cells(7).Text
                cantidadtotal = cantidadtotal + 1
            Next
            NumeroLiquidacionRuntime = CreateNewLiquidation(Date.Now, cantidadtotal, montototal, Session("UserCode").ToString())

            For Each row As GridViewRow In GridView1.Rows
                UpdateOsrnSAP(row.Cells(1).Text, "04", "02")
                CreateNewLiquidationDetalle(NumeroLiquidacionRuntime, row.Cells(0).Text, row.Cells(1).Text, row.Cells(2).Text, row.Cells(3).Text,
                                            row.Cells(4).Text, row.Cells(5).Text, drpContratista.SelectedItem.Text, row.Cells(6).Text, Replace(row.Cells(7).Text, ",", ""), Date.Now, Session("UserCode").ToString())
                UpdateMaintableLiquidacion(NumeroLiquidacionRuntime, Date.Now, row.Cells(1).Text)
            Next
            Response.Redirect("GenerarPO.aspx")
        Catch ex As Exception
            Response.Write("btnCrearLiquidacion_Click " & ex.Message)
        End Try
    End Sub

    Public Sub UpdateOsrnSAP(_SERIE As String, _estadoproduccion As String, _estadopago As String)
        Try
            Dim query As String = String.Empty
            query &= "UPDATE OSRN SET U_Estado_Produccion=@p2,U_Estado_Contabilidad=@p3 WHERE MnfSerial=@p1"
            Using conn As New SqlConnection(sCon2)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", _SERIE)
                        .Parameters.AddWithValue("@p2", _estadoproduccion)
                        .Parameters.AddWithValue("@p3", _estadopago)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using
        Catch ex As Exception
            Response.Write("UpdateOsrnSAP " & ex.Message)
        End Try
    End Sub

    'Protected Sub btnExportarLiquidacion_Click(sender As Object, e As EventArgs) Handles btnExportarLiquidacion.Click
    '    ExportGridToExcel()
    'End Sub

    'Protected Sub ExportToPdf(ByVal StringProcedure As String, ByVal path As String, ByVal name As String)
    '    Try
    '        pnlGridPrincipal.Visible = False
    '        Dim conn = New SqlConnection(sCon1)
    '        Dim sqlstring As String = StringProcedure
    '        Response.Write(sqlstring)
    '        Using comando = New SqlCommand(sqlstring, conn)
    '            Using adaptador = New SqlDataAdapter(comando)
    '                Dim ds = New DataSet()
    '                adaptador.Fill(ds)
    '                Dim reporte = New ReportDocument()
    '                reporte.Load(Server.MapPath(path))
    '                reporte.SetDataSource(ds.Tables(0))
    '                Response.Buffer = False
    '                Response.ClearContent()
    '                Response.ClearHeaders()
    '                reporte.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, name & "_" & String.Format("{0:ddMMyyyyHHmm}", DateTime.Now))
    '                reporte.Close()
    '                reporte.Dispose()
    '            End Using
    '        End Using
    '    Catch ex As Exception
    '        Response.Write(name & " " & ex.Message)
    '    End Try
    'End Sub

    Protected Sub btnVerPrecendentes_Click(sender As Object, e As EventArgs) Handles btnVerPrecendentes.Click
        gridPrecedentes.Visible = True
        'btnExportarExcel.Visible = True
        GridView1.Visible = False
        btnCrearLiquidacion.Visible = False
        'btnExportarLiquidacion.Visible = False
        drpContratista.Visible = False
        btnBack.Visible = True
        BindGridLiquidacionesPrecedentes(drpContratista.SelectedValue.ToString())
    End Sub
    Private Sub BindGridLiquidacionesPrecedentes(_BPCODE As String)
        Try
            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT convert(char,t0.DATECREATED,103)[DATECREATED],t0.LIQUIDACIONID, t0.[SERIE],t0.[MODELO],t0.[COLOR],T2.GRUPORESPONSABLE,t1.MECANICONAME " &
                                            " From [ArmadoMotos].[dbo].[ARMADOMOTOS] T0 INNER JOIN [ArmadoMotos].[dbo].[mecanicos] T1   " &
                                            " on t0.MECANICOASIGNADO=t1.ID inner join [ArmadoMotos].[dbo].[GRUPOS] T2 on t1.GRUPOID=t2.ID   " &
                                            " where LIQUIDACIONID<>0 AND T2.[ID] =3 ORDER BY t0.LIQUIDACIONID,t0.[MODELO]")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridPrecedentes.DataSource = dt
                            gridPrecedentes.DataBind()
                            gridPrecedentes.UseAccessibleHeader = True
                            gridPrecedentes.HeaderRow.TableSection = TableRowSection.TableHeader
                        End Using
                        con.Close()
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Response.Write("BindGridLiquidacionesPrecedentes " & ex.Message)
        End Try
    End Sub

    'Protected Sub btnExportarExcel_Click(sender As Object, e As EventArgs) Handles btnExportarExcel.Click
    '    Response.Clear()
    '    Response.Buffer = True
    '    Response.ClearContent()
    '    Response.ClearHeaders()
    '    Response.Charset = ""
    '    Dim FileName As String = "Precedentes " & drpContratista.SelectedItem.ToString & " " & DateTime.Now & ".xls"
    '    Dim strwritter As StringWriter = New StringWriter()
    '    Dim htmltextwrtter As HtmlTextWriter = New HtmlTextWriter(strwritter)
    '    Response.Cache.SetCacheability(HttpCacheability.NoCache)
    '    Response.ContentType = "application/vnd.ms-excel"
    '    Response.AddHeader("Content-Disposition", "attachment;filename=" & FileName)
    '    gridPrecedentes.GridLines = GridLines.Both
    '    gridPrecedentes.HeaderStyle.Font.Bold = True
    '    gridPrecedentes.RenderControl(htmltextwrtter)
    '    Response.Write(strwritter.ToString())
    '    Response.[End]()
    'End Sub

    Protected Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Try
            Response.Redirect("ProcesoContable.aspx")
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
End Class
