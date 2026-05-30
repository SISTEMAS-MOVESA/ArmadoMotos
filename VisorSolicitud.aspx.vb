Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Web.UI.DataVisualization.Charting
Imports DocumentFormat.OpenXml.ExtendedProperties
Imports SAPbobsCOM

Partial Class VisorSolicitud
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public MyTable As New DataTable
    Public csvFilePath As String
    Public SQL_STRING As String
    Public sql_consulta As String
    Public SCompany As SAPbobsCOM.Company
    Public oSBObob As SAPbobsCOM.SBObob
    Public oRecordSet As SAPbobsCOM.Recordset
    Public xCompany As SAPbobsCOM.Company
    Public lErrCode As Integer
    Public lRetCode As Integer
    Public sErrMsg As String
    Public argumento As String
    Public IdentidadMotorista As String
    Public MACRO_ID As String

    Private Sub VisorSolicitud_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    BindgridSugerido(Request.QueryString("id"))
                    lblIdMacro.Text = Request.QueryString("id")
                    lblAlmacenDestino.Text = GetWhsName(Request.QueryString("destino"))
                    lblRuta.Text = Request.QueryString("ruta")
                End If
            Else
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('VisorLogistica_Load: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub BindgridSugerido(macroid As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                SQL_STRING = "Select DISTINCT T1.Filler,T1.ToWhsCode," &
                " (SELECT WHSNAME FROM MOVESA..OWHS WITH(NOLOCK) WHERE WHSCODE=T1.ToWhsCode)[Almacen], " &
                " T2.ItemCode,T2.Dscription,T2.U_MSERIE" &
                " FROM [ArmadoMotos].[dbo].[SOLCITUD_HEADER] T0 WITH(NOLOCK) LEFT JOIN MOVESA..OWTQ T1 WITH(NOLOCK) " &
                " ON T0.DOCENTRYSAP=T1.DOCENTRY inner join MOVESA..WTQ1 T2 WITH(NOLOCK) ON T1.DocEntry=T2.DocEntry " &
                " WHERE MACROID=" & macroid & ""
                Using cmd As New SqlCommand(SQL_STRING)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridPedidoTemp.DataSource = dt
                            gridPedidoTemp.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridPedidoTemp.UseAccessibleHeader = True
            gridPedidoTemp.HeaderRow.TableSection = TableRowSection.TableHeader
            'Response.Write("<script>console.log('BindgridSugerido: " & ReplaceCharsForFileName(SQL_STRING, " ") & "');</script>")

        Catch ex As Exception
            Response.Write("<script>console.log('BindgridSugerido: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
            Response.Write("<script>console.log('BindgridSugerido: " & ReplaceCharsForFileName(SQL_STRING, " ") & "');</script>")

        End Try
    End Sub

    Private Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Try
            Response.Redirect("TrasladosDashboard.aspx")
        Catch ex As Exception
            Response.Write("<script>console.log('btnRegresar_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub btnTraslado_Click(sender As Object, e As EventArgs) Handles btnTraslado.Click
        Try
            CreateStockTransfer(Request.QueryString("id"))
        Catch ex As Exception
            Response.Write("<script>console.log('btnTraslado_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub

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
    Public Function GlobalConnecttoSAP() As Integer
        SCompany = New SAPbobsCOM.Company
        SCompany = New SAPbobsCOM.Company
        SCompany.Server = "192.168.1.3"
        SCompany.CompanyDB = "MOVESA"
        SCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012
        SCompany.DbUserName = "sa"
        SCompany.DbPassword = "M*l!n3r0s2k12"
        SCompany.UserName = "itpro"
        SCompany.Password = "eeal96"
        SCompany.language = SAPbobsCOM.BoSuppLangs.ln_Spanish_La
        SCompany.SLDServer = "192.168.1.9:40000"
        lRetCode = SCompany.Connect
        Return lRetCode
    End Function
    Public Sub CreateStockTransfer(CAMIONID As String)

        Try
            If GlobalConnecttoSAP() <> 0 Then
                Response.Write("<script>alert('Error al Intentar Conectarse a SAP');</script>")
                Exit Sub
            End If


            MACRO_ID = CAMIONID
            Dim oStockTransfer As SAPbobsCOM.StockTransfer = SCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer)
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                sql_consulta = "SELECT DISTINCT [WHSCODEORIGEN],[WHSCODE],[CODIGOCLIENTE]," &
                                        " [CAMION],[DOCENTRYSAP] FROM [ArmadoMotos].[dbo].[SOLCITUD_HEADER] where [MACROID]=" & CAMIONID & " "
                Response.Write("<script>console.log('sql_consulta: " & ReplaceCharsForFileName(sql_consulta, " ") & "');</script>")

                Using cmd As New SqlCommand(sql_consulta)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            For Each row As DataRow In dt.Rows
                                oStockTransfer.Series = 1003
                                oStockTransfer.DocDate = DateTime.Now
                                oStockTransfer.FromWarehouse = row.Item("WHSCODEORIGEN").ToString()
                                oStockTransfer.ToWarehouse = row.Item("WHSCODE").ToString()
                                'oStockTransfer.CardCode = row.Item("CODIGOCLIENTE").ToString()
                                'oStockTransfer.Comments = "CAMION: " & row.Item("CAMION").ToString() & " / " & row.Item("OBSERVACIONES").ToString()
                                'oStockTransfer.JournalMemo = "Portal_Produccion"
                                'oStockTransfer.PriceList = -2
                                'oStockTransfer.UserFields.Fields.Item("U_P_MODELO").Value = row.Item("CAMION").ToString()
                                'oStockTransfer.UserFields.Fields.Item("U_N_Cliente").Value = row.Item("OBSERVACIONES").ToString()
                                ''Informacion del transportista
                                'oStockTransfer.UserFields.Fields.Item("U_IdentConductor").Value = IdentidadMotorista
                                'oStockTransfer.UserFields.Fields.Item("U_Mottras").Value = drpMotivoTraslado.SelectedValue.ToString
                                'oStockTransfer.UserFields.Fields.Item("U_TipoTransporte").Value = drpTipoTransporte.SelectedValue.ToString

                                Dim constr2 As String = sCon2
                                Using con2 As New SqlConnection(constr2)

                                    Dim SQL_STRING As String = " select t0.docentry,t0.linenum,t0.ItemCode,isnull(t0.U_MSERIE,'N/S')[U_MSERIE], " &
                                    " (Select sysnumber from movesa..osrn With(nolock) where MnfSerial=t0.U_MSERIE)[SYSNUMBER] " &
                                    " from	movesa..wtq1 t0 with(nolock) where t0.DocEntry=" & row.Item("DOCENTRYSAP").ToString() & ""

                                    'Dim SQL_STRING As String = " SELECT	 T0.[DOCENTRYSAP], t0.[MACROID] " &
                                    '" ,(Select ObjType FROM MOVESA..OWTQ With(NOLOCK) WHERE DocEntry=T0.[DOCENTRYSAP])[OBJECT] " &
                                    '" ,(Select LINENUM FROM MOVESA..WTQ1 With(NOLOCK) WHERE DocEntry=T0. DOCENTRYSAP  And ITEMCODE=T1.ARTICULO COLLATE Modern_Spanish_CI_AS And U_MSERIE=t1.SERIE COLLATE Modern_Spanish_CI_AS) [LINENUM] " &
                                    '" ,T1.ARTICULO, T1.SERIE " &
                                    '" ,(Select sysnumber from movesa..osrn With(nolock) where MnfSerial=T1.SERIE COLLATE Modern_Spanish_CI_AS And ITEMCODE=T1.ARTICULO COLLATE Modern_Spanish_CI_AS)[SYSNUMBER] " &
                                    '" FROM [ArmadoMotos].[dbo].[SOLCITUD_HEADER] T0 inner join [ArmadoMotos].[dbo].[SOLCITUD_LINES] T1 " &
                                    '" On T0.ID=T1.HEADERID where t0.DOCENTRYSAP=" & row.Item("DOCENTRYSAP").ToString() & ""

                                    Response.Write("<script>console.log('SQL_STRING: " & ReplaceCharsForFileName(SQL_STRING, " ") & "');</script>")

                                    Using cmd2 As New SqlCommand(SQL_STRING)
                                        'Using cmd2 As New SqlCommand("Select * from SOLCITUD_LINES where CAMIONID=" & CAMIONID & "  And WHSCODE='" & row.Item("WHSCODE").ToString() & "' ")
                                        Using sda2 As New SqlDataAdapter()
                                            cmd2.Connection = con2
                                            sda2.SelectCommand = cmd2
                                            Using dt2 As New DataTable()
                                                sda2.Fill(dt2)
                                                For Each row2 As DataRow In dt2.Rows
                                                    ' Add the item lines
                                                    oStockTransfer.Lines.BaseEntry = CInt(row2.Item("docentry").ToString())
                                                    'oStockTransfer.Lines.BaseType = 67
                                                    oStockTransfer.Lines.BaseLine = CInt(row2.Item("linenum").ToString())
                                                    oStockTransfer.Lines.ItemCode = row2.Item("ItemCode").ToString()
                                                    'oStockTransfer.Lines.UserFields.Fields.Item("U_MSERIE").Value = row2.Item("U_MSERIE").ToString()
                                                    oStockTransfer.Lines.Quantity = 1 ' Quantity
                                                    Response.Write("<script>console.log('Lines: " & ReplaceCharsForFileName(row2.Item("docentry").ToString() & " " & row2.Item("linenum").ToString() & " " & row2.Item("ItemCode").ToString(), " ") & "');</script>")


                                                    If row2.Item("U_MSERIE").ToString() <> "N/S" Then
                                                        oStockTransfer.Lines.SerialNumbers.SystemSerialNumber = row2.Item("SYSNUMBER").ToString()
                                                        oStockTransfer.Lines.SerialNumbers.Add()
                                                    End If

                                                    oStockTransfer.Lines.Add()
                                                Next
                                            End Using
                                        End Using
                                    End Using
                                    con2.Close()
                                End Using

                                Dim iError As Integer = oStockTransfer.Add()
                                If iError <> 0 Then
                                    'Dim sErrMsg As String = SCompany.GetLastErrorCode()
                                    'Throw New Exception(sErrMsg)
                                    Response.Write("<script>alert('Stock transfer request Error: " & SCompany.GetLastErrorCode() & " - " & SCompany.GetLastErrorDescription() & "');</script>")

                                Else
                                    Dim sDocNum As String = SCompany.GetNewObjectKey()
                                    UpdateDocentrySAP(sDocNum, MACRO_ID)
                                    Response.Write("<script>alert('Stock transfer request created successfully. DocNum: " & sDocNum & "');</script>")
                                    'Console.WriteLine("Stock transfer request created successfully. DocNum: " + sDocNum)
                                End If
                            Next

                        End Using
                    End Using
                End Using
                con.Close()
            End Using

        Catch ex As Exception
            'Response.Write(ex.Message)
            Response.Write("<script>console.log('CreateStockTransfer: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub
    Public Sub UpdateDocentrySAP(docentrysap As String, MACROID As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "update [ArmadoMotos].[dbo].[MACROINSERT_HEADER] set ESTADO=6,DOCENTRYSAP=@p1 WHERE ID=@p2"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", docentrysap)
                cmd.Parameters.AddWithValue("@p2", MACROID)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('success','Camion Creado con Exito!');", True)
            End Using
        Catch ex As Exception
            Response.Write("EliminarLinea " & ex.Message)
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
