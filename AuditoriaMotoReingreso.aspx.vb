Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Partial Class AuditoriaMotoReingreso
    Inherits System.Web.UI.Page

    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"

    <WebMethod()>
    Public Shared Function SearchCustomers(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Using conn As SqlConnection = New SqlConnection()
            conn.ConnectionString = sCon1
            Using cmd As SqlCommand = New SqlCommand()

                cmd.CommandText = " SELECT top 10 T1.MnfSerial FROM OSRN T1 with(nolock) INNER JOIN OITM T0 WITH(NOLOCK) " &
                    " ON T1.ITEMCODE=T0.ITEMCODE WHERE T1.ITEMCODE<>'PLA001' and isnull(T1.U_VIN,'-')<>'N' AND T0.ItmsGrpCod='154'  " &
                    " AND T1.[MnfSerial] LIKE '%' + @SearchText + '%'"
                cmd.Parameters.AddWithValue("@SearchText", prefixText)
                cmd.Connection = conn
                conn.Open()
                Dim customers As List(Of String) = New List(Of String)()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        customers.Add(sdr("MnfSerial").ToString())
                    End While
                End Using
                conn.Close()
                Return customers
            End Using
        End Using
    End Function

    Public Sub BuscarSerie(_SERIE As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = " SELECT	T0.[ItemCode],T0.[ItemName],T1.[SuppSerial],T1.[IntrSerial],t1.BatchId, " &
                                        " T5.Name [Marca],T4.NAME [Cilindros], T3.Name [Motor],T2.Name [Modelo],T1.[WhsCode],T6.Name [Color], " &
                                        " (SELECT top 1 Owhs.[Whsname] FROM Owhs with(nolock) WHERE Owhs.[whscode]=T1.[WhsCode] ) [Almacen] " &
                                        " ,isnull((select top 1 datediff(day, DATECREATED, getdate()) from ArmadoMotos..ARMADOMOTOS where SERIE=@p1),0) [DiasArmado] " &
                                        " ,isnull((SELECT top 1 datediff(day,t2.DocDate,getdate()) FROM OSRN T0 With(nolock) inner join ITL1 T1 with(nolock) " &
                                        " On T1.SysNumber=T0.SysNumber And T1.ItemCode = T0.ItemCode inner join " &
                                        " OITL T2 with(nolock) on T1.LogEntry = T2.LogEntry where t0.MnfSerial =@p1  And t2.DocType=20),0) [Dias] " &
                                        " ,(select whscode from movesa..osri where status=0 and suppserial=@p1) + ' ' + (select whsname from movesa..owhs with(nolock) where  status=0 and whscode=(select whscode from movesa..osri where  status=0 and suppserial=@p1)) [AlmacenSerie] " &
                                        " ,(select CASE Status WHEN 0 then 'Disponible' ELSE 'No Disponible' END from movesa..osri where  status=0 and suppserial=@p1)[EstatusSAP] " &
                                        " FROM OITM T0 With(nolock) INNER JOIN OSRI T1 With(nolock) On T0.ItemCode = T1.ItemCode " &
                                        " FULL JOIN [@AMODELO] T2 With(NOLOCK) On T0.U_AMODELO=T2.CODE  " &
                                        " FULL JOIN [@AMOTOR] T3 With(NOLOCK) On T0.U_AMOTOR=T3.Code " &
                                        " FULL JOIN [@ACILINDROS] T4 With(NOLOCK) On T0.U_ACILINDROS=T4.Code " &
                                        " FULL JOIN [@AMARCA] T5 With(NOLOCK) On T0.U_AMARCA=T5.CODE " &
                                        " INNER JOIN [@SCOLOR] T6 With(NOLOCK) On T0.U_ACOLOR=T6.CODE " &
                                        " WHERE T1.[SuppSerial]= @p1"
            Dim Comando As New SqlCommand(Consulta, _con)
            Comando.Parameters.AddWithValue("@p1", _SERIE)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                txtCodigo.Text = drd.Item("ItemCode").ToString
                txtDescripcion.Text = drd.Item("ItemName").ToString
                txtModelo.Text = drd.Item("Modelo").ToString
                txtMarca.Text = drd.Item("Marca").ToString
                txtCilindros.Text = drd.Item("Cilindros").ToString
                txtSeriemoto.Text = drd.Item("SuppSerial").ToString
                txtSeriemotor.Text = drd.Item("IntrSerial").ToString
                txtColor.Text = drd.Item("Color").ToString
                txtYear.Text = drd.Item("BatchId").ToString
                txtArmado.Text = drd.Item("DiasArmado").ToString
                txtAgeing.Text = drd.Item("Dias").ToString
                txtALmacenSAP.Text = drd.Item("AlmacenSerie").ToString
                txtEstatusSerieSAP.Text = drd.Item("EstatusSAP").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    'Protected Sub btnGuardarInfo_Click(sender As Object, e As EventArgs) Handles btnGuardarInfo.Click

    'End Sub

    Protected Sub btnBusarSerie_Click(sender As Object, e As EventArgs) Handles btnBusarSerie.Click
        Try
            BuscarSerie(txtSnMoto.Text)
            'bindGridKardex(txtSnMoto.Text)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub AuditoriaMotoReingreso_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub
End Class
