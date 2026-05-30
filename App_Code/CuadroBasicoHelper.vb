Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

Public Class CuadroBasicoHelper

    Public Shared Function ActualizarCB(whscode As String, modelo As String,
                                        cbAnterior As Integer, cbNuevo As Integer,
                                        cbSugerido As Integer, ventaDiaria As Decimal,
                                        diasCobertura As Object,
                                        usuario As String, ip As String,
                                        motivo As String, origen As String) As Integer

        ' 1) Upsert en LOGISTICA_MAX_INV
        Dim queryUpsert As String =
            "IF EXISTS (SELECT 1 FROM [MOVESAWeb].[dbo].[LOGISTICA_MAX_INV] WHERE WHSCODE=@whs AND MODELO=@mod) " &
            "UPDATE [MOVESAWeb].[dbo].[LOGISTICA_MAX_INV] SET MAXIMO=@cb WHERE WHSCODE=@whs AND MODELO=@mod " &
            "ELSE INSERT INTO [MOVESAWeb].[dbo].[LOGISTICA_MAX_INV] (WHSCODE,MODELO,MAXIMO) VALUES (@whs,@mod,@cb)"

        Dim pUpd As New List(Of SqlParameter) From {
            New SqlParameter("@cb", cbNuevo),
            New SqlParameter("@whs", whscode),
            New SqlParameter("@mod", modelo)
        }
        DbConfig.ExecuteNonQuery(queryUpsert, DbConfig.DBServer.ARMADOMOTOS, pUpd)

        ' 2) Insert en CB_HISTORIAL
        Dim queryHist As String =
            "INSERT INTO [ArmadoMotos].[dbo].[CB_HISTORIAL] " &
            " (WHSCODE,MODELO,CB_ANTERIOR,CB_NUEVO,CB_SUGERIDO,VENTA_DIARIA,DIAS_COBERTURA,USUARIO,MOTIVO,ORIGEN,IP_CLIENTE) " &
            " VALUES (@whs,@mod,@cba,@cbn,@cbs,@vd,@cob,@user,@motivo,@origen,@ip)"

        Dim pHist As New List(Of SqlParameter) From {
            New SqlParameter("@whs", whscode),
            New SqlParameter("@mod", modelo),
            New SqlParameter("@cba", cbAnterior),
            New SqlParameter("@cbn", cbNuevo),
            New SqlParameter("@cbs", cbSugerido),
            New SqlParameter("@vd", ventaDiaria),
            New SqlParameter("@cob", If(diasCobertura Is Nothing OrElse diasCobertura Is DBNull.Value, CObj(DBNull.Value), diasCobertura)),
            New SqlParameter("@user", If(String.IsNullOrEmpty(usuario), "anon", usuario)),
            New SqlParameter("@motivo", If(String.IsNullOrEmpty(motivo), CObj(DBNull.Value), CObj(motivo))),
            New SqlParameter("@origen", origen),
            New SqlParameter("@ip", If(String.IsNullOrEmpty(ip), CObj(DBNull.Value), CObj(ip)))
        }

        Return DbConfig.ExecuteNonQuery(queryHist, DbConfig.DBServer.ARMADOMOTOS, pHist)
    End Function

End Class