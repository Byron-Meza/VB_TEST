Imports System.Data.OleDb

Public Class Connection

    Public Sub connect(conn As OleDbConnection)


    End Sub

    Public Sub disconnect(conn As OleDbConnection)
        Dim cadena As String = "provider=Microsoft.Jet.OLEDB.4.0;Data Source='basedatos.xls';Extended Properties=Excel 8.0;"

        conn.Close()

    End Sub

End Class
