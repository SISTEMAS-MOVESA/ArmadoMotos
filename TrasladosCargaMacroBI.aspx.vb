
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Partial Class TrasladosCargaMacroBI
    Inherits System.Web.UI.Page

    Private Sub TrasladosCargaMacroBI_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    If Request.Params("modelo") Is Nothing Then
                        CargarModelos()
                        drpModelosMotos.SelectedValue = "93"
                        BindGridExistncias(drpModelosMotos.SelectedItem.Text.Trim)
                        BindGridRankings(drpModelosMotos.SelectedValue.ToString.Trim)
                        BindGridStocks(drpModelosMotos.SelectedItem.Text.Trim)
                    Else
                        CargarModelos()
                        drpModelosMotos.SelectedItem.Text = Request.Params("modelo")
                        Dim modelo As String = Request.Params("modelo")
                        BindGridRankings(getModeloCode(modelo))
                        BindGridExistncias(modelo)
                        BindGridStocks(modelo)
                    End If
                End If
            Else
            End If
            Response.Write("<script>console.log('gridRanking: " & ReplaceCharsForFileName(drpModelosMotos.SelectedItem.Text.Trim, " ") & "');</script>")
        Catch ex As Exception
            Response.Write("OrdendeTrabajo_Load " & ex.Message)
        End Try
    End Sub

    Public Sub BindGridRankings(modeloText As String)
        Try
            Response.Write("<script>console.log('Codigo de Modelo: " & modeloText & "');</script>")

            Dim SQL_string As String = System.IO.File.ReadAllText(Server.MapPath("~/Querys/TrasladosCargaMacroBI_BindGridRankings.sql"))

            Dim params As New List(Of SqlParameter) From {
                New SqlParameter("@modelo", modeloText)
            }

            Dim dt As DataTable = DbConfig.GetDataTable(SQL_string, DbConfig.DBServer.ARMADOMOTOS, params)

            gridRanking.DataSource = dt
            gridRanking.DataBind()

            gridRanking.UseAccessibleHeader = True
            If gridRanking.HeaderRow IsNot Nothing Then
                gridRanking.HeaderRow.TableSection = TableRowSection.TableHeader
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridRanking: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Sub CargarModelos()
        Try
            Dim query As String = " select [Code],[Name] from [@AMODELO] where U_LG5='Y' order by [Name]"

            Dim dt As DataTable = DbConfig.GetDataTable(query, DbConfig.DBServer.MOVESA)

            drpModelosMotos.Dispose()
            drpModelosMotos.DataTextField = "Name"
            drpModelosMotos.DataValueField = "Code"
            drpModelosMotos.DataSource = dt
            drpModelosMotos.DataBind()

            If Request.Params("modelo") Is Nothing Then
                'BindGridExistncias(drpModelosMotos.SelectedValue.ToString)
            Else
                drpModelosMotos.SelectedValue = Request.Params("modelo")
            End If
        Catch ex As Exception
            Response.Write("CargarSucursales " & ex.Message)
        End Try
    End Sub

    Public Sub BindGridExistncias(modeloText As String)
        Try
            Dim query As String = "SELECT " &
                "(SELECT convert(int,SUM(t200.ONHAND)) " &
                "FROM movesa..OITW t200 WITH(NOLOCK) " &
                "FULL JOIN movesa..OITM T201 WITH(NOLOCK) " &
                "ON t200.ItemCode = T201.ItemCode " &
                "FULL JOIN movesa..[@AMODELO] t202 WITH(NOLOCK) " &
                "ON (t201.U_AMODELO = t202.Code OR t201.U_MODELO = t202.Code) " &
                "WHERE t202.name = @modelo AND T201.ItmsGrpCod = 154) [INV_MOVESA], " &
                "(SELECT convert(int,SUM(t200.ONHAND)) " &
                "FROM movesa..OITW t200 WITH(NOLOCK) " &
                "FULL JOIN movesa..OITM T201 WITH(NOLOCK) " &
                "ON t200.ItemCode = T201.ItemCode " &
                "FULL JOIN movesa..[@AMODELO] t202 WITH(NOLOCK) " &
                "ON (t201.U_AMODELO = t202.Code OR t201.U_MODELO = t202.Code) " &
                "WHERE t202.name = @modelo AND t200.WhsCode IN ('DCM00', 'TCEDI', 'CDM00') AND T201.ItmsGrpCod = 154) [INV_DCM00], " &
                "(SELECT convert(int,SUM(t200.ONHAND)) " &
                "FROM movesa..OITW t200 WITH(NOLOCK) " &
                "FULL JOIN movesa..OITM T201 WITH(NOLOCK) " &
                "ON t200.ItemCode = T201.ItemCode " &
                "FULL JOIN movesa..[@AMODELO] t202 WITH(NOLOCK) " &
                "ON (t201.U_AMODELO = t202.Code OR t201.U_MODELO = t202.Code) " &
                "INNER JOIN MOVESA..OWHS T203 WITH(NOLOCK) " &
                "ON T200.WhsCode = T203.WhsCode " &
                "WHERE t202.name = @modelo AND t200.WhsCode NOT IN ('DCM00', 'TCEDI', 'CDM00') AND T201.ItmsGrpCod = 154 AND T203.U_TYPE = 'PRO') [INV_CD], " &
                "(SELECT convert(int,SUM(t200.ONHAND)) " &
                "FROM movesa..OITW t200 WITH(NOLOCK) " &
                "FULL JOIN movesa..OITM T201 WITH(NOLOCK) " &
                "ON t200.ItemCode = T201.ItemCode " &
                "FULL JOIN movesa..[@AMODELO] t202 WITH(NOLOCK) " &
                "ON (t201.U_AMODELO = t202.Code OR t201.U_MODELO = t202.Code) " &
                "INNER JOIN MOVESA..OWHS T203 WITH(NOLOCK) " &
                "ON T200.WhsCode = T203.WhsCode " &
                "WHERE t202.name = @modelo AND t200.WhsCode NOT IN ('DCM00', 'TCEDI', 'CDM00') AND T201.ItmsGrpCod = 154 AND T203.U_TYPE <> 'PRO') [INV_CI]"

            Dim params As New List(Of SqlParameter) From {
                New SqlParameter("@modelo", modeloText)
            }

            Dim dt As DataTable = DbConfig.GetDataTable(query, DbConfig.DBServer.ARMADOMOTOS, params)

            gridExistencias.DataSource = dt
            gridExistencias.DataBind()

            gridExistencias.UseAccessibleHeader = True
            If gridExistencias.HeaderRow IsNot Nothing Then
                gridExistencias.HeaderRow.TableSection = TableRowSection.TableHeader
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridExistencias: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Sub BindGridStocks(modeloText As String)
        Try
            Response.Write("<script>console.log('BindGridStocks: " & modeloText & "');</script>")

            Dim query As String = " Select t200.WhsCode [Codigo], t203.WhsName [Nombre Almacen], convert(int,SUM(t200.ONHAND)) [Existencia] " &
                " FROM movesa..OITW t200 With(NOLOCK) " &
                " INNER JOIN movesa..OITM T201 With(NOLOCK) On t200.ItemCode = T201.ItemCode " &
                " INNER JOIN movesa..[@AMODELO] t202 With(NOLOCK) On t202.Code = t201.U_AMODELO " &
                " INNER JOIN movesa..OWHS T203 With(NOLOCK) On T200.WhsCode = T203.WhsCode " &
                " WHERE t202.Name = rtrim('Terra180') And T201.ItmsGrpCod = 154 " &
                " GROUP BY t200.WhsCode, t203.WhsName HAVING SUM(t200.ONHAND) > 0 " &
                " union all  " &
                " Select t200.WhsCode [Codigo], t203.WhsName [Nombre Almacen], convert(int,SUM(t200.ONHAND)) [Existencia] " &
                " FROM movesa..OITW t200 With(NOLOCK) " &
                " INNER JOIN movesa..OITM T201 With(NOLOCK) On t200.ItemCode = T201.ItemCode " &
                " INNER JOIN movesa..[@AMODELO] t202 With(NOLOCK) On t202.Code = t201.U_MODELO " &
                " INNER JOIN movesa..OWHS T203 With(NOLOCK) On T200.WhsCode = T203.WhsCode " &
                " WHERE t202.Name = rtrim('Terra180') And T201.ItmsGrpCod = 154 " &
                " GROUP BY t200.WhsCode, t203.WhsName HAVING SUM(t200.ONHAND) > 0 " &
                " order by 3 desc"

            Dim params As New List(Of SqlParameter) From {
                New SqlParameter("@modelo", If(CObj(modeloText), CObj(String.Empty)))
            }

            Dim dt As DataTable = DbConfig.GetDataTable(query, DbConfig.DBServer.MOVESA, params)

            gridStockAlmacen.EmptyDataText = "No se encontraron existencias para el modelo consultado."
            gridStockAlmacen.DataSource = dt
            gridStockAlmacen.DataBind()

            gridStockAlmacen.UseAccessibleHeader = True
            If gridStockAlmacen.HeaderRow IsNot Nothing Then
                gridStockAlmacen.HeaderRow.TableSection = TableRowSection.TableHeader
            End If

        Catch ex As Exception
            Response.Write("<script>console.log('BindGridStocks: " &
            ReplaceCharsForFileName(ex.Message, " ") &
            " | Stack: " & ReplaceCharsForFileName(ex.StackTrace, " ") &
            "');</script>")
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
        NewStr = sName
        Return NewStr
    End Function

    Private Sub drpModelosMotos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpModelosMotos.SelectedIndexChanged
        Try
            Response.Write("<script>console.log('drpModelosMotos_SelectedIndexChanged');</script>")

            BindGridExistncias(drpModelosMotos.SelectedItem.Text.Trim)
            BindGridRankings(drpModelosMotos.SelectedValue.ToString.Trim)
            BindGridStocks(drpModelosMotos.SelectedItem.Text.Trim)
        Catch ex As Exception
            Response.Write("<script>console.log('drpModelosMotos_SelectedIndexChanged: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Function getModeloCode(modelo As String) As String
        Dim sel As String = "SELECT CODE FROM MOVESA..[@AMODELO] WHERE [NAME] = @p1"

        Dim params As New List(Of SqlParameter) From {
            New SqlParameter("@p1", modelo)
        }

        Dim result As Object = DbConfig.ExecuteScalar(sel, DbConfig.DBServer.MOVESA, params)

        If result Is Nothing OrElse result Is DBNull.Value Then
            Return String.Empty
        End If

        Return result.ToString()
    End Function

End Class 'Imports System.Data
'Imports System.Data.SqlClient
'Imports System.IO
'Imports System.Net.Mail
'Imports iTextSharp.text
'Imports iTextSharp.text.pdf

'Partial Class TrasladosCargaMacroBI
'    Inherits System.Web.UI.Page
'    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
'    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
'    Private Sub TrasladosCargaMacroBI_Load(sender As Object, e As EventArgs) Handles Me.Load
'        Try
'            If IsPostBack = False Then
'                If Session("Name") Is vbNullString Then
'                    Response.Redirect("Default.aspx")
'                Else
'                    If Request.Params("modelo") Is Nothing Then
'                        CargarModelos()
'                        drpModelosMotos.SelectedValue = "93"
'                        BindGridExistncias(drpModelosMotos.SelectedItem.Text.Trim)
'                        BindGridRankings(drpModelosMotos.SelectedValue.ToString.Trim)
'                        BindGridStocks(drpModelosMotos.SelectedItem.Text.Trim)
'                    Else
'                        CargarModelos()
'                        drpModelosMotos.SelectedItem.Text = Request.Params("modelo")
'                        Dim modelo As String = Request.Params("modelo")
'                        BindGridRankings(getModeloCode(modelo))
'                        BindGridExistncias(modelo)
'                        BindGridStocks(modelo)
'                    End If
'                End If
'            Else
'            End If
'            Response.Write("<script>console.log('gridRanking: " & ReplaceCharsForFileName(drpModelosMotos.SelectedItem.Text.Trim, " ") & "');</script>")
'        Catch ex As Exception
'            Response.Write("OrdendeTrabajo_Load " & ex.Message)
'        End Try
'    End Sub
'    Public Sub BindGridRankings(modeloText As String)
'        Try
'            Response.Write("<script>console.log('Codigo de Modelo: " & modeloText & "');</script>")

'            Dim constr As String = sCon2
'            Using con As New SqlConnection(constr)
'                Dim SQL_string As String = System.IO.File.ReadAllText(Server.MapPath("~/Querys/TrasladosCargaMacroBI_BindGridRankings.sql"))
'                Using cmd As New SqlCommand(SQL_string)
'                    cmd.Parameters.AddWithValue("@modelo", modeloText)
'                    Using sda As New SqlDataAdapter()
'                        cmd.Connection = con
'                        sda.SelectCommand = cmd
'                        Using dt As New DataTable()
'                            sda.Fill(dt)
'                            gridRanking.DataSource = dt
'                            gridRanking.DataBind()
'                            con.Close()
'                        End Using
'                    End Using
'                End Using
'            End Using
'            gridRanking.UseAccessibleHeader = True
'            gridRanking.HeaderRow.TableSection = TableRowSection.TableHeader
'        Catch ex As Exception
'            Response.Write("<script>console.log('gridRanking: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
'        End Try
'    End Sub
'    Public Sub CargarModelos()
'        Try
'            Dim dt As DataTable = New DataTable()
'            Using conn As SqlConnection = New SqlConnection(sCon1)
'                Dim query As String = " select [Code],[Name] from [@AMODELO] where U_LG5='Y' order by [Name]"
'                Dim cmd As SqlCommand = New SqlCommand(query, conn)
'                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
'                da.Fill(dt)
'            End Using
'            drpModelosMotos.Dispose()
'            drpModelosMotos.DataTextField = "Name"
'            drpModelosMotos.DataValueField = "Code"
'            drpModelosMotos.DataSource = dt
'            drpModelosMotos.DataBind()

'            If Request.Params("modelo") Is Nothing Then
'                'BindGridExistncias(drpModelosMotos.SelectedValue.ToString)
'            Else
'                drpModelosMotos.SelectedValue = Request.Params("modelo")
'            End If
'        Catch ex As Exception
'            Response.Write("CargarSucursales " & ex.Message)
'        End Try
'    End Sub
'    Public Sub BindGridExistncias(modeloText As String)
'        Try
'            Dim constr As String = sCon2
'            Using con As New SqlConnection(constr)
'                Dim query As String = "SELECT " &
'                "(SELECT convert(int,SUM(t200.ONHAND)) " &
'                "FROM movesa..OITW t200 WITH(NOLOCK) " &
'                "FULL JOIN movesa..OITM T201 WITH(NOLOCK) " &
'                "ON t200.ItemCode = T201.ItemCode " &
'                "FULL JOIN movesa..[@AMODELO] t202 WITH(NOLOCK) " &
'                "ON (t201.U_AMODELO = t202.Code OR t201.U_MODELO = t202.Code) " &
'                "WHERE t202.name = @modelo AND T201.ItmsGrpCod = 154) [INV_MOVESA], " &
'                "(SELECT convert(int,SUM(t200.ONHAND)) " &
'                "FROM movesa..OITW t200 WITH(NOLOCK) " &
'                "FULL JOIN movesa..OITM T201 WITH(NOLOCK) " &
'                "ON t200.ItemCode = T201.ItemCode " &
'                "FULL JOIN movesa..[@AMODELO] t202 WITH(NOLOCK) " &
'                "ON (t201.U_AMODELO = t202.Code OR t201.U_MODELO = t202.Code) " &
'                "WHERE t202.name = @modelo AND t200.WhsCode IN ('DCM00', 'TCEDI', 'CDM00') AND T201.ItmsGrpCod = 154) [INV_DCM00], " &
'                "(SELECT convert(int,SUM(t200.ONHAND)) " &
'                "FROM movesa..OITW t200 WITH(NOLOCK) " &
'                "FULL JOIN movesa..OITM T201 WITH(NOLOCK) " &
'                "ON t200.ItemCode = T201.ItemCode " &
'                "FULL JOIN movesa..[@AMODELO] t202 WITH(NOLOCK) " &
'                "ON (t201.U_AMODELO = t202.Code OR t201.U_MODELO = t202.Code) " &
'                "INNER JOIN MOVESA..OWHS T203 WITH(NOLOCK) " &
'                "ON T200.WhsCode = T203.WhsCode " &
'                "WHERE t202.name = @modelo AND t200.WhsCode NOT IN ('DCM00', 'TCEDI', 'CDM00') AND T201.ItmsGrpCod = 154 AND T203.U_TYPE = 'PRO') [INV_CD], " &
'                "(SELECT convert(int,SUM(t200.ONHAND)) " &
'                "FROM movesa..OITW t200 WITH(NOLOCK) " &
'                "FULL JOIN movesa..OITM T201 WITH(NOLOCK) " &
'                "ON t200.ItemCode = T201.ItemCode " &
'                "FULL JOIN movesa..[@AMODELO] t202 WITH(NOLOCK) " &
'                "ON (t201.U_AMODELO = t202.Code OR t201.U_MODELO = t202.Code) " &
'                "INNER JOIN MOVESA..OWHS T203 WITH(NOLOCK) " &
'                "ON T200.WhsCode = T203.WhsCode " &
'                "WHERE t202.name = @modelo AND t200.WhsCode NOT IN ('DCM00', 'TCEDI', 'CDM00') AND T201.ItmsGrpCod = 154 AND T203.U_TYPE <> 'PRO') [INV_CI]"
'                Using cmd As New SqlCommand(query)
'                    cmd.Parameters.AddWithValue("@modelo", modeloText)
'                    Using sda As New SqlDataAdapter()
'                        cmd.Connection = con
'                        sda.SelectCommand = cmd
'                        Using dt As New DataTable()
'                            sda.Fill(dt)
'                            gridExistencias.DataSource = dt
'                            gridExistencias.DataBind()
'                            con.Close()
'                        End Using
'                    End Using
'                End Using
'            End Using
'            gridExistencias.UseAccessibleHeader = True
'            gridExistencias.HeaderRow.TableSection = TableRowSection.TableHeader
'        Catch ex As Exception
'            Response.Write("<script>console.log('gridExistencias: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
'        End Try
'    End Sub

'    Public Sub BindGridStocks(modeloText As String)
'        Try
'            Response.Write("<script>console.log('BindGridStocks: " & modeloText & "');</script>")
'            Dim constr As String = sCon1
'            Using con As New SqlConnection(constr)
'                Dim query As String = " Select t200.WhsCode [Codigo], t203.WhsName [Nombre Almacen], convert(int,SUM(t200.ONHAND)) [Existencia] " &
'                    " FROM movesa..OITW t200 With(NOLOCK) " &
'                    " INNER JOIN movesa..OITM T201 With(NOLOCK) On t200.ItemCode = T201.ItemCode " &
'                    " INNER JOIN movesa..[@AMODELO] t202 With(NOLOCK) On t202.Code = t201.U_AMODELO " &
'                    " INNER JOIN movesa..OWHS T203 With(NOLOCK) On T200.WhsCode = T203.WhsCode " &
'                    " WHERE t202.Name = rtrim('Terra180') And T201.ItmsGrpCod = 154 " &
'                    " GROUP BY t200.WhsCode, t203.WhsName HAVING SUM(t200.ONHAND) > 0 " &
'                    " union all  " &
'                    " Select t200.WhsCode [Codigo], t203.WhsName [Nombre Almacen], convert(int,SUM(t200.ONHAND)) [Existencia] " &
'                    " FROM movesa..OITW t200 With(NOLOCK) " &
'                    " INNER JOIN movesa..OITM T201 With(NOLOCK) On t200.ItemCode = T201.ItemCode " &
'                    " INNER JOIN movesa..[@AMODELO] t202 With(NOLOCK) On t202.Code = t201.U_MODELO " &
'                    " INNER JOIN movesa..OWHS T203 With(NOLOCK) On T200.WhsCode = T203.WhsCode " &
'                    " WHERE t202.Name = rtrim('Terra180') And T201.ItmsGrpCod = 154 " &
'                    " GROUP BY t200.WhsCode, t203.WhsName HAVING SUM(t200.ONHAND) > 0 " &
'                    " order by 3 desc"

'                Using cmd As New SqlCommand(query)
'                    cmd.Parameters.AddWithValue("@modelo", If(modeloText, String.Empty))
'                    Using sda As New SqlDataAdapter()
'                        cmd.Connection = con
'                        sda.SelectCommand = cmd
'                        Using dt As New DataTable()
'                            sda.Fill(dt)
'                            gridStockAlmacen.EmptyDataText = "No se encontraron existencias para el modelo consultado."
'                            gridStockAlmacen.DataSource = dt
'                            gridStockAlmacen.DataBind()
'                        End Using
'                    End Using
'                End Using
'            End Using

'            gridStockAlmacen.UseAccessibleHeader = True
'            If gridStockAlmacen.HeaderRow IsNot Nothing Then
'                gridStockAlmacen.HeaderRow.TableSection = TableRowSection.TableHeader
'            End If

'        Catch ex As Exception
'            Response.Write("<script>console.log('BindGridStocks: " &
'            ReplaceCharsForFileName(ex.Message, " ") &
'            " | Stack: " & ReplaceCharsForFileName(ex.StackTrace, " ") &
'            "');</script>")
'        End Try
'    End Sub
'    Public Function ReplaceCharsForFileName(sName As String, sChr As String) As String
'        Dim NewStr As String
'        sName = Replace(sName, "/", sChr)
'        sName = Replace(sName, "\", sChr)
'        sName = Replace(sName, ":", sChr)
'        sName = Replace(sName, "?", sChr)
'        sName = Replace(sName, Chr(34), sChr)
'        sName = Replace(sName, "<", sChr)
'        sName = Replace(sName, ">", sChr)
'        sName = Replace(sName, "|", sChr)
'        sName = Replace(sName, "&", sChr)
'        sName = Replace(sName, "%", sChr)
'        sName = Replace(sName, "*", sChr)
'        sName = Replace(sName, "'", sChr)
'        sName = Replace(sName, "{", sChr)
'        sName = Replace(sName, "[", sChr)
'        sName = Replace(sName, "]", sChr)
'        sName = Replace(sName, "}", sChr)
'        sName = Replace(sName, "!", sChr)
'        NewStr = sName
'        Return NewStr

'    End Function

'    Private Sub drpModelosMotos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpModelosMotos.SelectedIndexChanged
'        Try
'            Response.Write("<script>console.log('drpModelosMotos_SelectedIndexChanged');</script>")

'            BindGridExistncias(drpModelosMotos.SelectedItem.Text.Trim)
'            BindGridRankings(drpModelosMotos.SelectedValue.ToString.Trim)
'            BindGridStocks(drpModelosMotos.SelectedItem.Text.Trim)
'        Catch ex As Exception
'            Response.Write("<script>console.log('drpModelosMotos_SelectedIndexChanged: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
'        End Try
'    End Sub
'    Public Function getModeloCode(modelo As String) As String
'        Dim sCon As String = sCon1
'        Dim sel As String

'        sel = "SELECT CODE FROM MOVESA..[@AMODELO] WHERE [NAME] = @p1"

'        Using con As New SqlConnection(sCon)
'            Dim cmd As New SqlCommand(sel, con)
'            cmd.Parameters.AddWithValue("@p1", modelo)
'            con.Open()
'            Dim t As String = cmd.ExecuteScalar()
'            con.Close()
'            Return t
'        End Using
'    End Function
'End Class
