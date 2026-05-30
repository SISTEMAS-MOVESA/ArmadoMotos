Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Public Class DbConfig

    Public Enum DBServer
        ARMADOMOTOS
        MOVESA
        MOVESAWEB
    End Enum

    Private Shared Function GetConnection(server As DBServer) As SqlConnection

        Dim connStr As String = ""

        Select Case server
            Case DBServer.ARMADOMOTOS
                'Armadomotos
                connStr = ConfigurationManager.ConnectionStrings("ARMADOMOTOS").ConnectionString

            Case DBServer.MOVESA
                'Movesa
                connStr = ConfigurationManager.ConnectionStrings("MOVESA").ConnectionString
            Case DBServer.MOVESAWEB
                'Movesa
                connStr = ConfigurationManager.ConnectionStrings("MOVESAWEB").ConnectionString
        End Select

        Return New SqlConnection(connStr)

    End Function


    '============================
    ' SELECT → DataTable
    '============================

    Public Shared Function GetDataTable(query As String,
                                        server As DBServer,
                                        Optional params As List(Of SqlParameter) = Nothing) As DataTable

        Dim dt As New DataTable

        Using conn As SqlConnection = GetConnection(server)

            Using cmd As New SqlCommand(query, conn)

                If params IsNot Nothing Then
                    cmd.Parameters.AddRange(params.ToArray())
                End If

                Using da As New SqlDataAdapter(cmd)

                    da.Fill(dt)

                End Using

            End Using

        End Using

        Return dt

    End Function


    '============================
    ' SELECT → Scalar
    '============================

    Public Shared Function ExecuteScalar(query As String,
                                         server As DBServer,
                                         Optional params As List(Of SqlParameter) = Nothing) As Object

        Using conn As SqlConnection = GetConnection(server)

            conn.Open()

            Using cmd As New SqlCommand(query, conn)

                If params IsNot Nothing Then
                    cmd.Parameters.AddRange(params.ToArray())
                End If

                Return cmd.ExecuteScalar()

            End Using

        End Using

    End Function


    '============================
    ' INSERT UPDATE DELETE
    '============================

    Public Shared Function ExecuteNonQuery(query As String,
                                           server As DBServer,
                                           Optional params As List(Of SqlParameter) = Nothing) As Integer

        Using conn As SqlConnection = GetConnection(server)

            conn.Open()

            Using cmd As New SqlCommand(query, conn)

                If params IsNot Nothing Then
                    cmd.Parameters.AddRange(params.ToArray())
                End If

                Return cmd.ExecuteNonQuery()

            End Using

        End Using

    End Function


    '============================
    ' STORED PROCEDURE
    '============================

    Public Shared Function ExecuteStoredProcedure(spName As String,
                                                   server As DBServer,
                                                   Optional params As List(Of SqlParameter) = Nothing) As DataTable

        Dim dt As New DataTable

        Using conn As SqlConnection = GetConnection(server)

            Using cmd As New SqlCommand(spName, conn)

                cmd.CommandType = CommandType.StoredProcedure

                If params IsNot Nothing Then
                    cmd.Parameters.AddRange(params.ToArray())
                End If

                Using da As New SqlDataAdapter(cmd)

                    da.Fill(dt)

                End Using

            End Using

        End Using

        Return dt

    End Function

End Class