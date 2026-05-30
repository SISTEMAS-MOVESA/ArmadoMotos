Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.Drawing
Imports System.Configuration
Imports System.Web.Services
Imports DocumentFormat.OpenXml.Spreadsheet

Partial Class ListadoCCalidad
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"

    Public Sub TrabajosAdicionales()
        Try
            If Not Me.IsPostBack Then
                Dim constr As String = sCon2
                Using con As SqlConnection = New SqlConnection(constr)
                    Using cmd As SqlCommand = New SqlCommand("SELECT [ID] ,[TRABAJO] FROM [ArmadoMotos].[dbo].[TRABAJOS] with(nolock) where [STATUS]=1")
                        cmd.CommandType = CommandType.Text
                        cmd.Connection = con
                        con.Open()
                        lstTrabajosAdicionales.DataSource = cmd.ExecuteReader()
                        lstTrabajosAdicionales.DataTextField = "TRABAJO"
                        lstTrabajosAdicionales.DataValueField = "ID"
                        lstTrabajosAdicionales.DataBind()
                        con.Close()
                    End Using
                End Using

            End If
        Catch ex As Exception
            Response.Write("TrabajosAdicionales " & ex.Message)
        End Try
    End Sub

    <WebMethod()>
    Public Shared Function SearchCustomers(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Using conn As SqlConnection = New SqlConnection()
            conn.ConnectionString = sCon2
            Using cmd As SqlCommand = New SqlCommand()
                cmd.CommandText = " SELECT top 10 [SERIE] FROM [dbo].[ARMADOMOTOS] T1 with(nolock) WHERE T1.[SERIE] LIKE '%' + @SearchText + '%'"
                'cmd.CommandText = " SELECT top 10 [SERIE] FROM [dbo].[ARMADOMOTOS] T1 with(nolock) WHERE T1.[ESTATUS] ='Calidad' and T1.[SERIE] LIKE '%' + @SearchText + '%'"
                cmd.Parameters.AddWithValue("@SearchText", prefixText)
                cmd.Connection = conn
                conn.Open()
                Dim customers As List(Of String) = New List(Of String)()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        customers.Add(sdr("SERIE").ToString())
                    End While
                End Using
                conn.Close()
                Return customers
            End Using
        End Using
    End Function
    Private Sub ListadoCCalidad_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    TrabajosAdicionales()
                    _CargarControlCalidad()

                    Select Case Session("Position").ToString()
                        Case "Iniciador"
                    'Response.Redirect("MainDashBoard.aspx")
                        Case "Supervisor Armado"
                            'Response.Redirect("MainDashBoard.aspx")
                    End Select

                    If Session("SerieCalidad").ToString <> "NULL" Then
                        BuscarSerie(Session("SerieCalidad").ToString)
                        txtSnMoto.Text = Session("SerieCalidad").ToString
                    End If
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("ListadoCCalidad.aspx")
    End Sub
    Public Sub CreateCCHistory(_idarmado As String, _datecreated As Date, _Serialnumber As String, _mecanicocode As String, _si As String, _no As String, _df As String, _ccuser As String, _observaciones As String, _errormecanico As String, _errorfabrica As String)
        Try
            Dim query As String = String.Empty
            query &= " INSERT INTO [dbo].[CCHISTORY]([IDARMADO],[DATECREATED],[SERIALNUMBER],[MECANICO],[SI],[NO],[DF],[CCUSER],[OBSERVACIONES],[EM],[EF]) " &
                            " VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11) "
            Using conn As New SqlConnection(sCon2)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", _idarmado)
                        .Parameters.AddWithValue("@p2", _datecreated)
                        .Parameters.AddWithValue("@p3", _Serialnumber)
                        .Parameters.AddWithValue("@p4", _mecanicocode)
                        .Parameters.AddWithValue("@p5", _si)
                        .Parameters.AddWithValue("@p6", _no)
                        .Parameters.AddWithValue("@p7", _df)
                        .Parameters.AddWithValue("@p8", _ccuser)
                        .Parameters.AddWithValue("@p9", _observaciones)
                        .Parameters.AddWithValue("@p10", _errormecanico)
                        .Parameters.AddWithValue("@p11", _errorfabrica)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using
        Catch ex As Exception
            Response.Write("CreateCCHistory " & ex.Message)
        End Try
    End Sub
    Public Sub UpdateMotoEstatusMainTable(_estatus As String, _control1 As String, _control2 As String, _usuariocc As String, _fechacontrolcalidad As Date, _observaciones As String, _Serie As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "UPDATE [dbo].[ARMADOMOTOS] " &
                    " Set [ESTATUS] = @p1 " &
                    " ,[CONTROL1] = @p2 " &
                    " ,[CONTROL2] = @p3 " &
                    " ,[CCALIDAD1USER] = @p4 " &
                    " ,[FECHACC1] = @p5 " &
                    " ,[OBSERVACIONES1] = @p6 " &
                    " ,[FINCC]=GETDATE() " &
                    " WHERE [SERIE] = @p7"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _estatus)
                cmd.Parameters.AddWithValue("@p2", _control1)
                cmd.Parameters.AddWithValue("@p3", _control2)
                cmd.Parameters.AddWithValue("@p4", _usuariocc)
                cmd.Parameters.AddWithValue("@p5", _fechacontrolcalidad)
                cmd.Parameters.AddWithValue("@p6", _observaciones)
                cmd.Parameters.AddWithValue("@p7", _Serie)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
            'Response.Redirect("ListadoCCalidad.aspx")
        Catch ex As Exception
            Response.Write("ListadoCCalidad " & ex.Message)
        End Try
    End Sub

    Public Sub UpdateInicioCalidad(_fechaInicioControlcalidad As Date, _Serie As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "UPDATE [dbo].[ARMADOMOTOS] " &
                    " Set [INICIOCC] = @p1 " &
                    " WHERE [SERIE] = @p2"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _fechaInicioControlcalidad)
                cmd.Parameters.AddWithValue("@p2", _Serie)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("UpdateInicioCalidad " & ex.Message)
        End Try
    End Sub

    Public Sub UpdateFinCalidad(_fechaFinCalidad As Date, _Serie As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "UPDATE [dbo].[ARMADOMOTOS] " &
                    " Set [FINCC] = @p1 " &
                    " WHERE [SERIE] = @p2"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _fechaFinCalidad)
                cmd.Parameters.AddWithValue("@p2", _Serie)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("UpdateFinCalidad " & ex.Message)
        End Try
    End Sub

    Public Sub UpdateReproceso(_estatus As String, _control1 As String, _usuariocc As String, _fechacontrolcalidad As Date, _observaciones As String, _Serie As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "UPDATE [dbo].[ARMADOMOTOS] " &
                    " Set [ESTATUS] = @p1 " &
                    " ,[CONTROL1] = @p2 " &
                    " ,[CCALIDAD1USER] = @p4 " &
                    " ,[FECHACC1] = @p5 " &
                    " ,[OBSERVACIONES2] = @p6 " &
                    " WHERE [SERIE] = @p7"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _estatus)
                cmd.Parameters.AddWithValue("@p2", _control1)
                cmd.Parameters.AddWithValue("@p4", _usuariocc)
                cmd.Parameters.AddWithValue("@p5", _fechacontrolcalidad)
                cmd.Parameters.AddWithValue("@p6", _observaciones)
                cmd.Parameters.AddWithValue("@p7", _Serie)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
            'Response.Redirect("ListadoCCalidad.aspx")
        Catch ex As Exception
            Response.Write("ListadoCCalidad " & ex.Message)
        End Try
    End Sub

    Public Sub UpdateDetalles(_estatus As String, _control1 As String, _usuariocc As String, _fechacontrolcalidad As Date, _observaciones As String, _Serie As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "UPDATE [dbo].[ARMADOMOTOS] " &
                    " Set [ESTATUS] = @p1 " &
                    " ,[CONTROL1] = @p2 " &
                    " ,[CCALIDAD1USER] = @p4 " &
                    " ,[FECHACC1] = @p5 " &
                    " ,[OBSERVACIONES2] = @p6 " &
                    " WHERE [SERIE] = @p7"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _estatus)
                cmd.Parameters.AddWithValue("@p2", _control1)
                cmd.Parameters.AddWithValue("@p4", _usuariocc)
                cmd.Parameters.AddWithValue("@p5", _fechacontrolcalidad)
                cmd.Parameters.AddWithValue("@p6", _observaciones)
                cmd.Parameters.AddWithValue("@p7", _Serie)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
            'Response.Redirect("ListadoCCalidad.aspx")
        Catch ex As Exception
            Response.Write("ListadoCCalidad " & ex.Message)
        End Try
    End Sub


    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try

            If drpUpdateTipoProblema.SelectedValue = "Seleccione" And txtUpdateDefecto.Text <> "Sin Prolemas" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "iziToast.warning({title: '', message: '<b>Seleccione el Origen del Reproceso Antes de Actualizar!!</b>', position: 'topRight'});", True)
                Exit Sub
            End If

            Session("SerieCalidad") = "NULL"
            KardexVehiculo(Date.Now, txtUpdateSerial.Text, lblMarca.Text, lblModelo.Text, lblColor.Text, "Fin Control Calidad",
                           IIf(txtUpdateCommentsProblema.Text.Length > 0, txtUpdateCommentsProblema.Text, drpCalidadUsers.SelectedItem.Text),
                           Session("UserCode").ToString, 0, drpCalidadUsers.SelectedValue.ToString)

            Dim list As New List(Of String)()
            For Each item As ListItem In lstTrabajosAdicionales.Items
                If item.Selected = True Then
                    InsertarTrabajosAdicionales(txtUpdateSerial.Text, item.Value, GetTrabajoPrice(item.Value), Date.Now, Session("UserCode").ToString)
                End If
            Next

            HCALIDAD(txtUpdateSerial.Text, Date.Now, Session("UserCode").ToString)

            If Session("SinProblemas").ToString = "SI" Then
                CreateCCHistory(txtUpdateIdArmado.Text, Date.Now, txtUpdateSerial.Text, txtUpdateMecanicoCode.Text, "1", "0", "0", Session("User").ToString, txtUpdateCommentsProblema.Text, "0", "0")
                UpdateMotoEstatusMainTable("Armada", "ARMADA", "PP", Session("User").ToString, Date.Now, "Control de Calidad Pasado, Supervisor: " & drpCalidadUsers.SelectedItem.Text, txtUpdateSerial.Text)
                UpdateFinCalidad(Date.Now, txtUpdateSerial.Text)
                UpdateOsrnSAP(txtUpdateSerial.Text, "04", drpCalidadUsers.SelectedValue.ToString)
            End If

            If Session("Reproceso").ToString = "SI" Then
                CreateCCHistory(txtUpdateIdArmado.Text, Date.Now, txtUpdateSerial.Text, txtUpdateMecanicoCode.Text, "0", "1", "0", Session("User").ToString, txtUpdateCommentsProblema.Text, "0", "0")
                UpdateReproceso("Reproceso", "REPROCESO", Session("User").ToString, Date.Now, txtUpdateCommentsProblema.Text & ", Supervisor: " & drpCalidadUsers.SelectedItem.Text, txtUpdateSerial.Text)
                UpdateFinCalidad(Date.Now, txtUpdateSerial.Text)
                UpdateOsrnSAP(txtUpdateSerial.Text, "02", drpCalidadUsers.SelectedValue.ToString)
            End If
            'en caso que la moto sale con detalles
            If Session("Defectuoso").ToString = "SI" Then
                Select Case drpUpdateTipoProblema.SelectedValue.ToString
                    Case "0" 'fabrica
                        CreateCCHistory(txtUpdateIdArmado.Text, Date.Now, txtUpdateSerial.Text, txtUpdateMecanicoCode.Text, "0", "0", "1", Session("User").ToString, txtUpdateCommentsProblema.Text, "0", "1")
                        UpdateMotoEstatusMainTable("Armada", "ARMADA", "PP", Session("User").ToString, Date.Now, "Pagar Moto Armada Piezas Faltantes Fabrica, Supervisor: " & drpCalidadUsers.SelectedItem.Text, txtUpdateSerial.Text)
                        UpdateFinCalidad(Date.Now, txtUpdateSerial.Text)
                        UpdateOsrnSAP(txtUpdateSerial.Text, "05", drpCalidadUsers.SelectedValue.ToString)
                        UPDATE_ESTADO_CC(txtUpdateSerial.Text, drpUpdateTipoProblema.SelectedItem.Text, Date.Now, Session("User"))
                    Case "1" 'mecanico
                        CreateCCHistory(txtUpdateIdArmado.Text, Date.Now, txtUpdateSerial.Text, txtUpdateMecanicoCode.Text, "0", "0", "1", Session("User").ToString, txtUpdateCommentsProblema.Text, "1", "0")
                        UpdateReproceso("Armada", "ARMADA", Session("User").ToString, Date.Now, txtUpdateCommentsProblema.Text & ", Supervisor:  " & drpCalidadUsers.SelectedItem.Text, txtUpdateSerial.Text)
                        UpdateFinCalidad(Date.Now, txtUpdateSerial.Text)
                        UpdateOsrnSAP(txtUpdateSerial.Text, "04", drpCalidadUsers.SelectedValue.ToString)
                        UPDATE_ESTADO_CC(txtUpdateSerial.Text, drpUpdateTipoProblema.SelectedItem.Text, Date.Now, Session("User"))
                    Case "2" 'desarme garantia
                        CreateCCHistory(txtUpdateIdArmado.Text, Date.Now, txtUpdateSerial.Text, txtUpdateMecanicoCode.Text, "0", "0", "1", Session("User").ToString, txtUpdateCommentsProblema.Text, "1", "0")
                        UpdateMotoEstatusMainTable("Garanta", "ARMADA", "PP", Session("User").ToString, Date.Now, "Pagar Moto Armada, pasa a Garantia, Supervisor: " & drpCalidadUsers.SelectedItem.Text, txtUpdateSerial.Text)
                        UpdateFinCalidad(Date.Now, txtUpdateSerial.Text)
                        UpdateOsrnSAP(txtUpdateSerial.Text, "04", drpCalidadUsers.SelectedValue.ToString)
                        TimeStampDetalle(txtUpdateSerial.Text, Date.Now)
                        UPDATE_ESTADO_CC(txtUpdateSerial.Text, drpUpdateTipoProblema.SelectedItem.Text, Date.Now, Session("User"))
                    Case "3" 'trabajos de pintura
                        CreateCCHistory(txtUpdateIdArmado.Text, Date.Now, txtUpdateSerial.Text, txtUpdateMecanicoCode.Text, "0", "0", "1", Session("User").ToString, txtUpdateCommentsProblema.Text, "1", "0")
                        UpdateMotoEstatusMainTable("Pintura", "ARMADA", "PP", Session("User").ToString, Date.Now, "Pagar Moto Armada, pasa a Pintura, Supervisor: " & drpCalidadUsers.SelectedItem.Text, txtUpdateSerial.Text)
                        UpdateFinCalidad(Date.Now, txtUpdateSerial.Text)
                        UpdateOsrnSAP(txtUpdateSerial.Text, "04", drpCalidadUsers.SelectedValue.ToString)
                        TimeStampDetalle(txtUpdateSerial.Text, Date.Now)
                        UPDATE_ESTADO_CC(txtUpdateSerial.Text, drpUpdateTipoProblema.SelectedItem.Text, Date.Now, Session("User"))
                    Case "4" 'Tornillo o tuerca quebrada
                        CreateCCHistory(txtUpdateIdArmado.Text, Date.Now, txtUpdateSerial.Text, txtUpdateMecanicoCode.Text, "0", "1", "0", Session("User").ToString, txtUpdateCommentsProblema.Text, "0", "0")
                        UpdateReproceso("Reproceso", "REPROCESO", Session("User").ToString, Date.Now, txtUpdateCommentsProblema.Text & ", Supervisor: " & drpCalidadUsers.SelectedItem.Text, txtUpdateSerial.Text)
                        UpdateFinCalidad(Date.Now, txtUpdateSerial.Text)
                        UpdateOsrnSAP(txtUpdateSerial.Text, "02", drpCalidadUsers.SelectedValue.ToString)
                        UPDATE_ESTADO_CC(txtUpdateSerial.Text, drpUpdateTipoProblema.SelectedItem.Text, Date.Now, Session("User"))
                    Case "5" 'Falla eléctrica
                        CreateCCHistory(txtUpdateIdArmado.Text, Date.Now, txtUpdateSerial.Text, txtUpdateMecanicoCode.Text, "0", "1", "0", Session("User").ToString, txtUpdateCommentsProblema.Text, "0", "0")
                        UpdateReproceso("Reproceso", "REPROCESO", Session("User").ToString, Date.Now, txtUpdateCommentsProblema.Text & ", Supervisor: " & drpCalidadUsers.SelectedItem.Text, txtUpdateSerial.Text)
                        UpdateFinCalidad(Date.Now, txtUpdateSerial.Text)
                        UpdateOsrnSAP(txtUpdateSerial.Text, "02", drpCalidadUsers.SelectedValue.ToString)
                        UPDATE_ESTADO_CC(txtUpdateSerial.Text, drpUpdateTipoProblema.SelectedItem.Text, Date.Now, Session("User"))
                    Case "6" 'Falla en motor
                        CreateCCHistory(txtUpdateIdArmado.Text, Date.Now, txtUpdateSerial.Text, txtUpdateMecanicoCode.Text, "0", "1", "0", Session("User").ToString, txtUpdateCommentsProblema.Text, "0", "0")
                        UpdateReproceso("Reproceso", "REPROCESO", Session("User").ToString, Date.Now, txtUpdateCommentsProblema.Text & ", Supervisor: " & drpCalidadUsers.SelectedItem.Text, txtUpdateSerial.Text)
                        UpdateFinCalidad(Date.Now, txtUpdateSerial.Text)
                        UpdateOsrnSAP(txtUpdateSerial.Text, "02", drpCalidadUsers.SelectedValue.ToString)
                        UPDATE_ESTADO_CC(txtUpdateSerial.Text, drpUpdateTipoProblema.SelectedItem.Text, Date.Now, Session("User"))
                    Case "7" 'Daño de fabrica u origen
                        CreateCCHistory(txtUpdateIdArmado.Text, Date.Now, txtUpdateSerial.Text, txtUpdateMecanicoCode.Text, "0", "0", "1", Session("User").ToString, txtUpdateCommentsProblema.Text, "0", "1")
                        UpdateMotoEstatusMainTable("Armada", "ARMADA", "PP", Session("User").ToString, Date.Now, "Pagar Moto Armada Piezas Faltantes Fabrica, Supervisor: " & drpCalidadUsers.SelectedItem.Text, txtUpdateSerial.Text)
                        UpdateFinCalidad(Date.Now, txtUpdateSerial.Text)
                        UpdateOsrnSAP(txtUpdateSerial.Text, "05", drpCalidadUsers.SelectedValue.ToString)
                        UPDATE_ESTADO_CC(txtUpdateSerial.Text, drpUpdateTipoProblema.SelectedItem.Text, Date.Now, Session("User"))
                End Select
            End If
            Response.Redirect("ListadoCCalidad.aspx")
        Catch ex As Exception
            Response.Write("btnUpdate_Click " & ex.Message)
        End Try
    End Sub
    Public Sub HCALIDAD(_Serie As String, _datecreated As Date, _usuario As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "INSERT INTO [dbo].[HCALIDAD] ([SERIE],[FECHAINGRESO],[USUARIO]) " &
                    " VALUES(@P1,@P2,@P3)"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _Serie)
                cmd.Parameters.AddWithValue("@p2", _datecreated)
                cmd.Parameters.AddWithValue("@p3", _usuario)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("HCALIDAD " & ex.Message)
        End Try
    End Sub
    Public Sub UPDATE_ESTADO_CC(SERIE As String, ESTADOCC As String, FECHAESTADOCC As Date, USUARIOESTADOCC As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "UPDATE  [ArmadoMotos].[dbo].[ARMADOMOTOS] SET [ESTADOCC] = @p2 " &
                    " ,[FECHAESTADOCC] = @p3 " &
                    " ,[USUARIOESTADOCC] = @p4" &
                    " where SERIE = @p1"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", SERIE)
                cmd.Parameters.AddWithValue("@p2", ESTADOCC)
                cmd.Parameters.AddWithValue("@p3", FECHAESTADOCC)
                cmd.Parameters.AddWithValue("@p4", USUARIOESTADOCC)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("UPDATE_ESTADO_CC " & ex.Message)
        End Try
    End Sub

    Public Sub TimeStampDetalle(_Serie As String, _timecapture As DateTime)
        Try
            Response.Write("<script language=javascript>console.log(`'" & ReplaceCharsForFileName(_Serie, " ") & "'`); </script>")


            Dim sCon As String = sCon2
            Dim sel As String
            sel = "update [ArmadoMotos].[dbo].[ARMADOMOTOS] set [DATEDETALLE] = @p1 where SERIE = @p2"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _timecapture)
                cmd.Parameters.AddWithValue("@p2", _Serie)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("TimeStampDetalle " & ex.Message)
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
    Public Sub InsertarTrabajosAdicionales(_SERIE As String, _IDTRABAJO As String, _PRECIO As String, _DATECREATED As Date, _USUARIO As String)
        Try
            Dim query As String = String.Empty
            query &= "INSERT INTO dbo.TRABAJOSADICIONALES (SERIE,IDTRABAJO,PRECIO,DATECREATED,USUARIO,ESTADO)" &
                        " VALUES (@p1,@p2,@p3,@p4,@p5,@p6)"
            Using conn As New SqlConnection(sCon2)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", _SERIE)
                        .Parameters.AddWithValue("@p2", _IDTRABAJO)
                        .Parameters.AddWithValue("@p3", _PRECIO)
                        .Parameters.AddWithValue("@p4", _DATECREATED)
                        .Parameters.AddWithValue("@p5", _USUARIO)
                        .Parameters.AddWithValue("@p6", "PENDIENTE")
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using
        Catch ex As Exception
            ' Verifica si el error es una violación de la restricción UNIQUE KEY
            If ex.Message.Contains("UNIQUE KEY") Then
                ' Muestra una alerta iziToast
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "iziToast.warning({title: '', message: '<b>No Puede Diplicar ese trabajo para este numero de serie!</b>', position: 'topRight'});", True)
            Else
                ' En caso de otros errores, se puede manejar de otra manera
                Response.Write("InsertarTrabajosAdicionales Error: " & ex.Message)
            End If
        End Try
    End Sub
    'Public Sub InsertarTrabajosAdicionales(_SERIE As String, _IDTRABAJO As String, _PRECIO As String, _DATECREATED As Date, _USUARIO As String)
    '    Try
    '        Dim query As String = String.Empty
    '        query &= "INSERT INTO dbo.TRABAJOSADICIONALES (SERIE,IDTRABAJO,PRECIO,DATECREATED,USUARIO)" &
    '                        " VALUES (@p1,@p2,@p3,@p4,@p5)"
    '        Using conn As New SqlConnection(sCon2)
    '            Using comm As New SqlCommand()
    '                With comm
    '                    .Connection = conn
    '                    .CommandType = CommandType.Text
    '                    .CommandText = query
    '                    .Parameters.AddWithValue("@p1", _SERIE)
    '                    .Parameters.AddWithValue("@p2", _IDTRABAJO)
    '                    .Parameters.AddWithValue("@p3", _PRECIO)
    '                    .Parameters.AddWithValue("@p4", _DATECREATED)
    '                    .Parameters.AddWithValue("@p5", _USUARIO)
    '                End With
    '                conn.Open()
    '                comm.ExecuteNonQuery()
    '                conn.Close()
    '            End Using
    '        End Using
    '    Catch ex As Exception
    '        Response.Write("InsertarTrabajosAdicionales " & ex.Message)
    '    End Try
    'End Sub

    Public Sub UpdateOsrnSAP(_SERIE As String, _estado As String, _idcalidad As String)
        Try
            Dim query As String = String.Empty
            query &= "UPDATE OSRN SET U_Estado_Produccion=@p2,U_Estado_Contabilidad='01',U_Calidad=@p3 WHERE MnfSerial=@p1"
            Using conn As New SqlConnection(sCon1)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", _SERIE)
                        .Parameters.AddWithValue("@p2", _estado)
                        .Parameters.AddWithValue("@p3", _idcalidad)
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
    Public Function GetTrabajoPrice(_IDTRABAJO As String) As Decimal
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "SELECT [PRECIO] FROM [ArmadoMotos].[dbo].[TRABAJOS] with(nolock) where [ID]=@p1"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _IDTRABAJO)
                con.Open()
                Dim t As Decimal = cmd.ExecuteScalar()
                con.Close()
                Return t
            End Using
        Catch ex As Exception
            Response.Write("GetTrabajoPrice " & ex.Message)
        End Try
    End Function
    Protected Sub btnBuscarMoto_Click(sender As Object, e As EventArgs) Handles btnBuscarMoto.Click
        Try
            BuscarSerie(txtSnMoto.Text)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub KardexVehiculo(DATELOG As Date, SERIE As String, MARCA As String, MODELO As String, COLOR As String, ACCION As String, COMENTARIOS As String, USUARIO As String, MECANICO As String, CALIDAD As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "INSERT INTO [dbo].[KARDEXVEHICULO] ([DATELOG],[SERIE],[MARCA],[MODELO],[COLOR],[ACCION],[COMENTARIOS],[USUARIO],[MECANICO],[CALIDAD]) " &
                    " VALUES(@P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8,@P9,@P10)"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", DATELOG)
                cmd.Parameters.AddWithValue("@p2", SERIE)
                cmd.Parameters.AddWithValue("@p3", MARCA)
                cmd.Parameters.AddWithValue("@p4", MODELO)
                cmd.Parameters.AddWithValue("@p5", COLOR)
                cmd.Parameters.AddWithValue("@p6", ACCION)
                cmd.Parameters.AddWithValue("@p7", COMENTARIOS)
                cmd.Parameters.AddWithValue("@p8", USUARIO)
                cmd.Parameters.AddWithValue("@p9", MECANICO)
                cmd.Parameters.AddWithValue("@p10", CALIDAD)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("RetiroDePiezas " & ex.Message)
        End Try
    End Sub
    Public Sub BuscarSerie(_SERIE As String)
        Try
            If _SERIE = "NULL" Then
                Exit Sub
            Else
                Dim dt As New DataTable
                Dim _con As New SqlConnection(sCon2)
                Dim Consulta As String = " SELECT * FROM [dbo].[ARMADOMOTOS] T1 with(nolock) inner join MECANICOS t2 with(nolock) on t1.MECANICOASIGNADO=t2.ID WHERE T1.[SERIE]='" & _SERIE & "' "
                'Dim Consulta As String = " SELECT * FROM [dbo].[ARMADOMOTOS] T1 with(nolock) inner join MECANICOS t2 with(nolock) on t1.MECANICOASIGNADO=t2.ID WHERE T1.[SERIE]='" & _SERIE & "' and [CONTROL1]='ARMADA' AND [CONTROL2]='NP' "
                Dim Comando As New SqlCommand(Consulta, _con)
                Dim drd As SqlDataReader
                _con.Open()
                drd = Comando.ExecuteReader()
                If drd.Read() Then
                    Session("IDArmadoMoto") = drd.Item("ID").ToString
                    Session("MecanicoCodee") = drd.Item("MECANICOCODE").ToString
                    lblItemcode.Text = drd.Item("ItemCode").ToString
                    lblItemname.Text = drd.Item("ITENMANE").ToString
                    lblModelo.Text = drd.Item("Modelo").ToString
                    lblYear.Text = drd.Item("YEAR").ToString
                    lblColor.Text = drd.Item("Color").ToString
                    lblMarca.Text = drd.Item("Marca").ToString
                    lblCilindros.Text = drd.Item("Cilindros").ToString
                    lblSerie.Text = drd.Item("SERIE").ToString
                    lblSerieMotor.Text = drd.Item("SERIEM").ToString
                    lblMotorM.Text = drd.Item("Motor").ToString
                    pnlBotonesCalidad.Visible = True
                    'UpdateInicioCalidad(Date.Now, txtSnMoto.Text)
                Else
                    'Response.Write("<script>alert('ERROR || La Serie ya Paso por el Proceso de Calidad, Verifique los datos');</script>")
                    lblItemcode.Text = ""
                    lblItemname.Text = ""
                    lblModelo.Text = ""
                    lblYear.Text = ""
                    lblColor.Text = ""
                    lblMarca.Text = ""
                    lblCilindros.Text = ""
                    lblSerie.Text = ""
                    lblSerieMotor.Text = ""
                    lblMotorM.Text = ""
                End If
                _con.Close()
            End If

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Redirect("ListadoCCalidad.aspx")
    End Sub
    Protected Sub btnSi_Click(sender As Object, e As EventArgs) Handles btnSi.Click
        pnlGridControlCalidad.Visible = False
        drpUpdateTipoProblema.Visible = False
        problemaOdefecto.Visible = False
        pnlUpdate.Visible = True
        'Session("User")
        txtUpdateIdArmado.Text = Session("IDArmadoMoto").ToString
        txtUpdateSerial.Text = txtSnMoto.Text
        txtUpdateDefecto.Text = "Sin Prolemas"
        txtUpdateMecanicoCode.Text = Session("MecanicoCodee").ToString
        problemaOdefecto.Visible = False

        Session("SinProblemas") = "SI"
        Session("Reproceso") = "NO"
        Session("Defectuoso") = "NO"
        'CreateCCHistory(GridView1.Rows(index).Cells(3).Text, Date.Now, GridView1.Rows(index).Cells(4).Text, GridView1.Rows(index).Cells(10).Text, "1", "0", "0", Session("User").ToString, "Control de Calidad Pasado", "0", "0")
        'UpdateMotoEstatusMainTable("Armada", "ARMADA", "PP", Session("User").ToString, Date.Now, "Control de Calidad Pasado", GridView1.Rows(index).Cells(4).Text)
    End Sub
    Protected Sub btnNo_Click(sender As Object, e As EventArgs) Handles btnNo.Click
        pnlUpdate.Visible = True
        pnlGridControlCalidad.Visible = False
        problemaOdefecto.Visible = False
        txtUpdateIdArmado.Text = Session("IDArmadoMoto").ToString
        txtUpdateSerial.Text = txtSnMoto.Text
        txtUpdateDefecto.Text = "Reproceso"
        txtUpdateMecanicoCode.Text = Session("MecanicoCodee").ToString
        drpUpdateTipoProblema.Visible = True
        Session("Reproceso") = "SI"
        Session("Defectuoso") = "NO"
        Session("SinProblemas") = "NO"

    End Sub
    Protected Sub btnDF_Click(sender As Object, e As EventArgs) Handles btnDF.Click
        pnlUpdate.Visible = True
        pnlPintura.Visible = True
        pnlGridControlCalidad.Visible = False
        problemaOdefecto.Visible = True
        txtUpdateIdArmado.Text = Session("IDArmadoMoto").ToString
        txtUpdateSerial.Text = txtSnMoto.Text
        txtUpdateDefecto.Text = "Desperfecto"
        txtUpdateMecanicoCode.Text = Session("MecanicoCodee").ToString
        Session("Reproceso") = "NO"
        Session("Defectuoso") = "SI"
        Session("SinProblemas") = "NO"
        Session("Detalles") = "Moto Con Detalles"
    End Sub

    Public Sub _CargarControlCalidad()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon2)
                Dim query As String = "	SELECT [ID],[USERNAME] FROM [ArmadoMotos].[dbo].[USUARIOS] with(nolock) where USERROL='CALIDAD'" '&
                '" where ID NOT IN (SELECT MECANICOASIGNADO FROM [dbo].[ARMADOMOTOS] WITH (NOLOCK) WHERE [ESTATUS] IN ('Asignada','Proceso'))"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                drpCalidadUsers.Dispose()
                drpCalidadUsers.DataTextField = "USERNAME"
                drpCalidadUsers.DataValueField = "ID"
                drpCalidadUsers.DataSource = dt
                drpCalidadUsers.DataBind()
                conn.Close()
            End Using
        Catch ex As Exception
            'ADD_LOG("_CargarAlmacenes", ex.Message)
            Response.Write("_CargarControlCalidad " & ex.Message)
        End Try
    End Sub

End Class
