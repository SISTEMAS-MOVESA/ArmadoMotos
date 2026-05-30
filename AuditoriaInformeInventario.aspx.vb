Imports System.Data
Imports System.Data.SqlClient

Partial Class AuditoriaInformeInventario
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Private Sub AuditoriaInformeInventario_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
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
                Using cmd As New SqlCommand("SELECT [Id],[EstadoVeh],[OrigenDestino],[Pasillo],[Segmento],[Codigo],  " &
                                            " [Marca],[Modelo],[Color], [SerieMoto], [YYYY],(SELECT DATEDIFF(DAY,FINCC,GETDATE())  " &
                                            " FROM ARMADOMOTOS..ARMADOMOTOS WHERE SERIE=T00.SerieMoto) [DiasArmado] " &
                                            " ,isnull((SELECT top 1 datediff(day,t2.DocDate,getdate()) FROM MOVESA..OSRN T0 With(nolock)  " &
                                            " inner join MOVESA..ITL1 T1 with(nolock) " &
                                            "  On T1.SysNumber=T0.SysNumber And T1.ItemCode = T0.ItemCode inner join   " &
                                            " MOVESA..OITL T2 with(nolock) on T1.LogEntry = T2.LogEntry where t0.MnfSerial =T00.SerieMoto   " &
                                            " COLLATE Modern_Spanish_CI_AS And t2.DocType=20),0) " &
                                            " [DiasIngreso], [Observaciones], [AUDITORIA], [LIMPIEZA], [PINTURA] " &
                                            " FROM [ArmadoMotos].[dbo].[InventarioVehiculos] T00 WHERE ESTADOLOGISTICA='ACTIVO'")
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
            'Response.Write("BindGrid " & ex.Message)
        End Try
    End Sub
    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Try
            BindGridByDate(txtDesde.Text, txtHasta.Text)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub BindGridByDate(desde As DateTime, hasta As DateTime)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("Select [ID],[ITEMCODE],[ITENMANE],[SERIE],[MODELO],[COLOR],[DATECREATED],[ESTATUS], " &
                                            "(Select [MECANICONAME] FROM [dbo].[MECANICOS] With(NOLOCK) WHERE ID = MECANICOASIGNADO) As [MECANICOASIGNADO], " &
                                            "[DATECREATED],[FIRSTUPDATEDATE],[SECONDUPDATEDATE],[INICIOCC],[FINCC] , " &
                                            "(Select USERNAME FROM USUARIOS With(NOLOCK) WHERE USERCODE = ArmadoMotos.CCALIDAD1USER) As [Usuario] " &
                                            "FROM [dbo].[ARMADOMOTOS] With(NOLOCK) " &
                                            "WHERE ESTATUS <> 'ERROR' AND CANCELED = 'N' " &
                                            "AND SECONDUPDATEDATE BETWEEN @d1 AND @d2", con)
                    ' Agregar los parámetros a la consulta
                    cmd.Parameters.AddWithValue("@d1", desde)
                    cmd.Parameters.AddWithValue("@d2", hasta)

                    Using sda As New SqlDataAdapter(cmd)
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GridView1.DataSource = dt
                            GridView1.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using

            ' Configurar el GridView
            GridView1.UseAccessibleHeader = True
            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Response.Write("BindGridByDate " & ex.Message)
        End Try
    End Sub
    Protected Sub btnPivot_Click(sender As Object, e As EventArgs) Handles btnPivot.Click
        Try
            Response.Redirect("AuditoriaPivot.aspx")
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
End Class
