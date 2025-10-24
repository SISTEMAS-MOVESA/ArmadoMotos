Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.IO
Imports System.Web.Hosting
Imports ClosedXML.Excel
Imports DocumentFormat.OpenXml.ExtendedProperties
Imports SAPbobsCOM
Partial Class DescuentoOrdenesVenta
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

    Private Sub DescuentoOrdenesVenta_Load(sender As Object, e As EventArgs) Handles Me.Load
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
    Private Sub BindGrid()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim sql_string As String = "select	distinct " &
                    "(select case [status] when 1 then 'CERRAR ORDEN' ELSE 'SERIE DISPONIBLE' END from OSRI where ItemCode=t1.itemcode and suppserial=T1.U_MSERIE)[Estado] " &
                    " ,convert(char,t0.docdate,103)[Fecha],DATEDIFF(day,t0.docdate,GETDATE())[Dias], t0.docentry[DocEntry],t0.DocNum[Orden Vta],  " &
                    " t0.CardCode[Codigo],t0.CardName[Cliente],T1.U_MSERIE[Serie],T1.Dscription[Descripcion],t0.DocTotal[Total],t1.WhsCode [Sucursal]" &
                    " ,(select Dia_en_Movesa from movesaweb..Inventario_motos where SerieChasis=t1.u_mserie collate SQL_Latin1_General_CP1_CI_AS)[Dias Movesa] " &
                    " from	movesa..ORDR t0 with(Nolock) " &
                    " inner join " &
                    " movesa..RDR1 t1 with(nolock) " &
                    " on t0.DocEntry=t1.DocEntry " &
                    " inner join " &
                    " movesa..OITM t2 with(nolock) " &
                    " on t1.ItemCode=t2.ItemCode " &
                    " inner join " &
                    " movesa..OWHS t3 with(nolock) " &
                    " on t1.WhsCode=t3.WhsCode " &
                    " where	t2.ItmsGrpCod=154 " &
                    " and t0.DocStatus='O' " &
                    " and t3.U_Type='PRO'"
                Using cmd As New SqlCommand(sql_string)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridOrdenesAbiertas.DataSource = dt
                            gridOrdenesAbiertas.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridOrdenesAbiertas.UseAccessibleHeader = True
            gridOrdenesAbiertas.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Response.Write("BindGrid " & ex.Message)
        End Try
    End Sub
    Public Function GlobalConnecttoSAP() As Integer
        SCompany = New SAPbobsCOM.Company
        SCompany.Server = "192.168.1.3"
        SCompany.CompanyDB = "MOVESA"
        SCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012
        SCompany.DbUserName = "sa"
        SCompany.DbPassword = "M*l!n3r0s2k12"
        SCompany.UserName = "it"
        SCompany.Password = "polar"
        SCompany.language = SAPbobsCOM.BoSuppLangs.ln_Spanish_La
        SCompany.SLDServer = "192.168.1.9:40000"
        lRetCode = SCompany.Connect
        Return lRetCode
    End Function
    Protected Sub CloseOrder(docentry As Integer)
        Dim oOrder As SAPbobsCOM.Documents = Nothing

        Try
            ' Conexión a SAP
            GlobalConnecttoSAP()
            If lRetCode <> 0 Then
                Dim mensajeError As String = SCompany.GetLastErrorDescription().Replace("'", "\'")
                Dim script As String = "Swal.fire({ icon: 'error', title: 'Error de conexión', text: '" & mensajeError & "' });"
                ClientScript.RegisterStartupScript(Me.GetType(), "swalError", script, True)
                Exit Sub
            End If

            ' Obtener el objeto de orden
            oOrder = CType(SCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders), SAPbobsCOM.Documents)

            If oOrder.GetByKey(docentry) Then

                For lc As Integer = 0 To oOrder.Lines.Count - 1
                    oOrder.Lines.SetCurrentLine(lc)
                    If oOrder.Lines.LineStatus <> SAPbobsCOM.BoStatus.bost_Close Then
                        oOrder.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Close
                    End If
                Next

                Dim intStatus As Integer = oOrder.Update()
                If intStatus <> 0 Then
                    Dim errCode As Integer
                    Dim errMsg As String = ""
                    SCompany.GetLastError(errCode, errMsg)
                    Dim mensajeError As String = errMsg.Replace("'", "\'")
                    Dim script As String = "Swal.fire({ icon: 'error', title: 'oOrder.Update()', text: '" & mensajeError & "' });"
                    ClientScript.RegisterStartupScript(Me.GetType(), "swalUpdateError", script, True)
                End If
            End If

        Catch ex As Exception
            Dim mensajeError As String = ex.Message.Replace("'", "\'")
            Dim script As String = "Swal.fire({ icon: 'error', title: 'Error CloseOrder', text: '" & mensajeError & "' });"
            ClientScript.RegisterStartupScript(Me.GetType(), "swalCatchError", script, True)

        Finally
            ' Liberar recursos COM y limpiar memoria
            If oOrder IsNot Nothing Then
                Runtime.InteropServices.Marshal.ReleaseComObject(oOrder)
                oOrder = Nothing
            End If
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Sub

    'Protected Sub CloseOrder(docentry As Integer)
    '    Try
    '        ' Conexión a SAP
    '        GlobalConnecttoSAP()
    '        If lRetCode <> 0 Then
    '            Dim mensajeError As String = SCompany.GetLastErrorDescription().Replace("'", "\'")
    '            Dim script As String = "Swal.fire({ icon: 'error', title: 'Error de conexión', text: '" & mensajeError & "' });"
    '            ClientScript.RegisterStartupScript(Me.GetType(), "swalError", script, True)
    '            Exit Sub
    '        End If
    '        ' Cerrar orden en SAP
    '        Dim oOrder As SAPbobsCOM.Documents = CType(SCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders), SAPbobsCOM.Documents)

    '        If oOrder.GetByKey(docentry) Then
    '            For lc As Integer = 0 To oOrder.Lines.Count - 1
    '                oOrder.Lines.SetCurrentLine(lc)
    '                If oOrder.Lines.LineStatus <> SAPbobsCOM.BoStatus.bost_Close Then
    '                    oOrder.Lines.Delete()
    '                    'oOrder.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Close
    '                End If
    '            Next

    '            Dim intStatus As Integer = oOrder.Update()
    '            If intStatus <> 0 Then
    '                Dim errCode As Integer
    '                Dim errMsg As String = ""
    '                SCompany.GetLastError(errCode, errMsg)
    '                Dim mensajeError As String = SCompany.GetLastErrorDescription().Replace("'", "\'")
    '                Dim script As String = "Swal.fire({ icon: 'error', title: 'oOrder.Update()', text: '" & mensajeError & "' });"
    '                ClientScript.RegisterStartupScript(Me.GetType(), "swalError", script, True)
    '            End If
    '        End If

    '        Runtime.InteropServices.Marshal.ReleaseComObject(oOrder)
    '        oOrder = Nothing
    '    Catch ex As Exception
    '        Dim script As String = "Swal.fire({ icon: 'error', title: 'Error CloseOrder, text: '" & ex.Message.ToString.Replace("'", "\'") & "' });"
    '        ClientScript.RegisterStartupScript(Me.GetType(), "swalError", script, True)
    '    End Try
    'End Sub
    Private Sub gridOrdenesAbiertas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridOrdenesAbiertas.RowCommand
        Try
            If e.CommandName = "Ver" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridOrdenesAbiertas.Rows(index)

                ' Aquí puedes usar los datos de la fila si deseas mostrarlos dentro del modal (ej. con un Label, etc.)

                ' Mostrar el modal con jQuery desde el code-behind
                Dim script As String = "$(document).ready(function() { $('#modal').modal('show'); });"
                ClientScript.RegisterStartupScript(Me.GetType(), "showModal", script, True)
            End If

            If e.CommandName = "Cerrar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridOrdenesAbiertas.Rows(index)
                Dim OrdenCerrada As String = gvRow.Cells(5).Text & " - " & gvRow.Cells(6).Text
                Dim docEntry As String = gvRow.Cells(5).Text

                CloseOrder(Convert.ToInt32(docEntry))

                Dim script As String = "Swal.fire({ icon: 'success', title: 'Éxito', text: 'Orden eliminada: " & OrdenCerrada.Replace("'", "\'") & "' });"
                ClientScript.RegisterStartupScript(Me.GetType(), "swalSuccess", script, True)
            End If

            BindGrid()
        Catch ex As Exception
            Dim script As String = "Swal.fire({ icon: 'error', title: 'Error', text: '" & ex.Message.Replace("'", "\'") & "' });"
            ClientScript.RegisterStartupScript(Me.GetType(), "swalCatch", script, True)
        End Try
    End Sub

    'Private Sub gridOrdenesAbiertas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridOrdenesAbiertas.RowCommand
    '    Try
    '        If e.CommandName = "Ver" Then
    '            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
    '            Dim gvRow As GridViewRow = gridOrdenesAbiertas.Rows(index)
    '            'Session("SerieExpediente") = gridOrdenesAbiertas.Rows(index).Cells(4).Text
    '            'Response.Redirect("ExpedienteVehiculo.aspx")
    '        End If

    '        If e.CommandName = "Cerrar" Then
    '            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
    '            Dim gvRow As GridViewRow = gridOrdenesAbiertas.Rows(index)
    '            Dim OrdenCerrada As String = gvRow.Cells(5).Text & " - " & gvRow.Cells(6).Text
    '            Dim docEntry As String = gvRow.Cells(5).Text

    '            CloseOrder(Convert.ToInt32(docEntry))

    '            Dim script As String = "Swal.fire({ icon: 'success', title: 'Éxito', text: 'Orden eliminada: " & OrdenCerrada.Replace("'", "\'") & "' });"
    '            ClientScript.RegisterStartupScript(Me.GetType(), "swalSuccess", script, True)
    '        End If
    '        BindGrid()
    '    Catch ex As Exception
    '        Dim script As String = "Swal.fire({ icon: 'error', title: 'Error', text: '" & ex.Message.Replace("'", "\'") & "' });"
    '        ClientScript.RegisterStartupScript(Me.GetType(), "swalCatch", script, True)
    '    End Try
    'End Sub

End Class
