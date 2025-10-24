Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports DocumentFormat.OpenXml.InkML
Imports DocumentFormat.OpenXml.Spreadsheet
Imports System.Web.UI.HtmlControls

Partial Class OrdenTrabajoConfirmacion
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Dim MyTable As New DataTable


    Private Sub OrdenTrabajoConfirmacion_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    lblSupervisor.Text = Session("Name").ToString
                    LoadGrid()
                End If
            Else
            End If
        Catch ex As Exception
            Response.Write("OrdendeTrabajo_Load " & ex.Message)
        End Try
    End Sub
    Sub LoadGrid()
        Try
            Dim strSQL = "select (select id from OTHEADER with(nolock) where RUTA=t.RUTA and CAMION=t.CAMION)[Id],t.CAMION, T.RUTA from  " &
                                                " (SELECT distinct ROWID [CAMION],RUTA " &
                                                    " ,(SELECT [NAME] FROM MOVESA..[@CRESPONSABLEOWHS] WHERE CODE=(SELECT U_CATEGORIZACION   " &
                                                    " FROM MOVESA..OWHS WITH(NOLOCK) WHERE WHSCODE=CODALM COLLATE Modern_Spanish_CI_AS ))[RESPONSABLE]   " &
                                                    " FROM [dbo].[PLAN_ARMADO] With(nolock) where APROBADOL='N' AND APROBADOC='N' ) as t " &
                                                    " where t.RESPONSABLE='" & Session("Name") & "' "

            Using cmdSQL As New SqlCommand(strSQL, New SqlConnection(sCon2))

                cmdSQL.Connection.Open()
                MyTable.Load(cmdSQL.ExecuteReader)
                repeater1.DataSource = MyTable
                repeater1.DataBind()
                cmdSQL.Connection.Close()
            End Using
        Catch ex As Exception
            Response.Write("LoadGrid " & ex.Message)
        End Try
    End Sub
    Public Sub UpdateTotals(QtySup As String, fupdate As Date, id As Integer)
        Dim connection As SqlConnection
        connection = New SqlConnection(sCon2)
        connection.Open()
        Dim command As SqlCommand
        command = connection.CreateCommand()
        Dim sqlquery As String = "DECLARE @qty AS DECIMAL(19,0),@fupdate AS datetime,@id AS INT " &
        " SET @qty=" & QtySup & " " &
        " SET @fupdate='" & fupdate & "' " &
        " SET @id=" & id & " " &
        " UPDATE [dbo].[PLAN_ARMADO] SET [SUPERVISOR] = @qty,[FUPDATE] = @fupdate WHERE ID= @id"
        command.CommandText = sqlquery
        command.ExecuteNonQuery()
        connection.Close()
    End Sub
    Private Shared Function GetData(query As String) As DataTable
        Dim strConnString As String = sCon2
        Using con As New SqlConnection(strConnString)
            Using cmd As New SqlCommand()
                cmd.CommandText = query
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using ds As New DataSet()
                        Dim dt As New DataTable()
                        sda.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
            con.Close()
        End Using
    End Function
    Public Sub cargarAlmacenesDisponiblesModal()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon1)
                Dim query As String = "select whscode, WhsName from owhs t0 with(nolock)inner join " &
                                        " [@CRESPONSABLEOWHS] t1 with(nolock) on t0.U_Categorizacion=t1.Code " &
                                        " where t1.Name='" & Session("Name") & "'"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                drpModalAlmacenes.Dispose()
                drpModalAlmacenes.DataTextField = "WhsName"
                drpModalAlmacenes.DataValueField = "whscode"
                drpModalAlmacenes.DataSource = dt
                drpModalAlmacenes.DataBind()
                conn.Close()
            End Using

        Catch ex As Exception
            Response.Write("cargarAlmacenesDisponiblesModal " & ex.Message)
        End Try
    End Sub
    Public Sub cargarModalModeloMoto()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon1)
                Dim query As String = "select CODE,NAME from [@amodelo] where u_lg5='Y'"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                drpModalModelo.Dispose()
                drpModalModelo.DataTextField = "NAME"
                drpModalModelo.DataValueField = "CODE"
                drpModalModelo.DataSource = dt
                drpModalModelo.DataBind()
                conn.Close()
            End Using

        Catch ex As Exception
            Response.Write("cargarModalModeloMoto " & ex.Message)
        End Try
    End Sub
    Public Sub cargarModalDescripcionMoto(codemodelo As String)
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon1)
                Dim query As String = "select itemcode, itemcode + ' - ' +itemname[Itemname] from oitm with(nolock) where U_AMODELO='" & codemodelo & "' and ItmsGrpCod=154"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                drpModalDescripcion.Dispose()
                drpModalDescripcion.DataTextField = "Itemname"
                drpModalDescripcion.DataValueField = "itemcode"
                drpModalDescripcion.DataSource = dt
                drpModalDescripcion.DataBind()
                conn.Close()
            End Using

        Catch ex As Exception
            Response.Write("cargarModalModeloMoto " & ex.Message)
        End Try
    End Sub
    Private Sub btnNueva_Click(sender As Object, e As EventArgs) Handles btnNueva.Click
        cargarAlmacenesDisponiblesModal()
        cargarModalModeloMoto()
        cargarModalDescripcionMoto(drpModalModelo.SelectedValue.ToString)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ModalView", "<script>$('#ModalSolicitud').modal('show');</script>", False)

    End Sub
    Private Sub drpModalModelo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpModalModelo.SelectedIndexChanged
        Try
            cargarModalDescripcionMoto(drpModalModelo.SelectedValue.ToString)
            BuscarCuadroBasico(drpModalModelo.SelectedItem.Text, drpModalAlmacenes.SelectedValue.ToString)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ModalView", "<script>$('#ModalSolicitud').modal('show');</script>", False)
        Catch ex As Exception
            Response.Write("drpModalModelo_SelectedIndexChanged " & ex.Message)
        End Try
    End Sub
    Public Sub BuscarCuadroBasico(modelo As String, almacen As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = "SELECT [MAXIMO] FROM [MOVESAWeb].[dbo].[LOGISTICA_MAX_INV] where [WHSCODE]='" & almacen & "' and [MODELO]='" & modelo & "'"
            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                txtModalCuadroB.Text = drd.Item("MAXIMO").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write("BuscarCuadroBasico " & ex.Message)
        End Try
    End Sub
    Protected Sub btnGuardarPlanificacion_Click(sender As Object, e As EventArgs) Handles btnGuardarPlanificacion.Click
        Try
            For Each item As RepeaterItem In repeater1.Items
                Dim gvOrders As GridView = TryCast(item.FindControl("gvOrders"), GridView)
                For Each row As GridViewRow In gvOrders.Rows
                    Dim str As String = TryCast(row.FindControl("qty"), System.Web.UI.WebControls.TextBox).Text
                    'Response.Write("<script>console.log('===============================');</script>")
                    'Response.Write("<script>console.log('" & TryCast(item.FindControl("lblID"), Label).Text & "');</script>")
                    'Response.Write("<script>console.log('" & row.Cells(2).Text & "');</script>")
                    'Response.Write("<script>console.log('" & Page.Server.HtmlDecode(row.Cells(3).Text) & "');</script>")
                    'Response.Write("<script>console.log('" & row.Cells(4).Text & "');</script>")
                    'Response.Write("<script>console.log('" & row.Cells(5).Text & "');</script>")
                    'Response.Write("<script>console.log('" & row.Cells(6).Text & "');</script>")
                    'Response.Write("<script>console.log('" & str & "');</script>")
                    'Response.Write("<script>console.log('" & row.Cells(7).Text & "');</script>")
                    'Response.Write("<script>console.log('N');</script>")
                    'Response.Write("<script>console.log('ABIERTO');</script>")
                    'Response.Write("<script>console.log('" & Session("Name") & "');</script>")
                    'Response.Write("<script>console.log('0');</script>")

                    If str > 0 Then
                        InsertOtline(TryCast(item.FindControl("lblID"), Label).Text,
                    row.Cells(2).Text,
                    Page.Server.HtmlDecode(row.Cells(3).Text),
                    row.Cells(4).Text,
                    row.Cells(5).Text,
                    row.Cells(6).Text,
                    str,
                    row.Cells(7).Text,
                    "N",
                    "ABIERTO",
                    Session("Name"),
                    "0")
                        UpdateLineState(row.Cells(0).Text, row.Cells(2).Text, row.Cells(4).Text, row.Cells(5).Text)
                    End If
                Next
            Next
            Response.Redirect("OrdenTrabajoConfirmacion.aspx")
        Catch ex As Exception
            Response.Write("btnGuardarPlanificacion_Click " & ex.Message)
        End Try
    End Sub
    Public Sub UpdateLineState(rowid As String, codalm As String, modelo As String, articulo As String)
        Try
            Dim query As String = String.Empty
            query &= "update [ArmadoMotos].[dbo].[PLAN_ARMADO] set [APROBADOL]='Y' WHERE ROWID=@p1  AND CODALM=@p2 AND MODELO=@p3 AND ARTICULO=@p4"
            Using conn As New SqlConnection(sCon1)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", rowid)
                        .Parameters.AddWithValue("@p2", codalm)
                        .Parameters.AddWithValue("@p3", modelo)
                        .Parameters.AddWithValue("@p4", articulo)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using
        Catch ex As Exception
            Response.Write("UpdateLineState " & ex.Message)
        End Try
    End Sub
    Private Sub repeater1_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles repeater1.ItemDataBound
        Try
            Dim RepRow As Integer = e.Item.ItemIndex
            Dim PKId As Integer = MyTable.Rows(RepRow).Item("CAMION")
            Dim gvOrders As GridView = e.Item.FindControl("gvOrders")
            gvOrders.DataSource = GetData(String.Format("SELECT * FROM [ArmadoMotos].[dbo].[PLAN_ARMADO] where [ROWID]='{0}' and [APROBADOL]='N'", PKId))
            gvOrders.DataBind()
            gvOrders.ShowHeader = True
        Catch ex As Exception
            Response.Write("repeater1_ItemDataBound " & ex.Message)
        End Try
    End Sub
    Public Sub InsertOtline(hid As String, calmacen As String, nalmacen As String, modelo As String, codigo As String, descripcion As String, cantidad As String, espacios As String,
                            cancelado As String, estado As String, usuario As String, preference As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = " INSERT INTO [dbo].[OTLINES] ([HID],[CALMACEN],[NALMACEN],[MODELO],[CODIGO],[DESCRIPCION],[CANTIDAD],[ESPACIOS],[CANCELADO],[ESTADO],[USUARIO],[Preference]) " &
                    " VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12)"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", hid)
                cmd.Parameters.AddWithValue("@p2", calmacen)
                cmd.Parameters.AddWithValue("@p3", nalmacen)
                cmd.Parameters.AddWithValue("@p4", modelo)
                cmd.Parameters.AddWithValue("@p5", codigo)
                cmd.Parameters.AddWithValue("@p6", descripcion)
                cmd.Parameters.AddWithValue("@p7", cantidad)
                cmd.Parameters.AddWithValue("@p8", espacios)
                cmd.Parameters.AddWithValue("@p9", cancelado)
                cmd.Parameters.AddWithValue("@p10", estado)
                cmd.Parameters.AddWithValue("@p11", usuario)
                cmd.Parameters.AddWithValue("@p12", preference)
                con.Open()
                cmd.ExecuteScalar()
                'Response.Write("<script>console.log('" &  & "');</script>")
                con.Close()

            End Using
        Catch ex As Exception
            Response.Write("InsertOtline " & ex.Message)
        End Try
    End Sub
End Class