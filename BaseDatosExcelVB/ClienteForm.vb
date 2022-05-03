Imports System.IO
Imports System.Data.OleDb

Public Class ClienteForm
    Dim dtCliente As DataTable
    Dim connection = New Connection

    Private Sub verificaCamposSalCodea(sender As Object, e As EventArgs) Handles et_name.LostFocus, et_homeAddress.LostFocus, et_email.LostFocus
        Dim txt As New TextBox
        txt = sender
        If txt.Text = "" Then
            txt.Text = "<-complete los datos->"
        End If
        sender = txt
    End Sub

    Private Sub verificaCamposEntrada(sender As Object, e As EventArgs) Handles et_name.GotFocus, et_homeAddress.GotFocus, et_email.GotFocus
        Dim txt As New TextBox
        txt = sender
        If txt.Text = "<-complete los datos->" Then
            txt.Text = ""
        End If
        sender = txt
    End Sub

    Public Sub Llenar()
        Dim cadena As String = "provider=Microsoft.Jet.OLEDB.4.0;Data Source='basedatos.xls';Extended Properties=Excel 8.0;"

        Dim conn As New OleDbConnection(cadena)

        conn.Open()

        Dim da As New OleDbDataAdapter("select * from [cliente$]", conn)
        Dim ds As New DataSet
        da.Fill(ds)

        dtCliente = New DataTable
        dtCliente = ds.Tables(0)

        Dim bs As New BindingSource
        bs.DataSource = dtCliente

        bs.Filter = "Name like '%" & et_search.Text & "%'"

        DataGridView1.DataSource = bs

        connection.disconnect(conn)



    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Llenar()
        LimpiarCampos()
    End Sub


    Private Sub LlenaTexto()
        et_code.Text = DataGridView1.CurrentRow.Cells("Code").Value.ToString
        et_name.Text = DataGridView1.CurrentRow.Cells("Name").Value.ToString
        et_date.Value = DataGridView1.CurrentRow.Cells("Date_to_Birth").Value
        et_email.Text = DataGridView1.CurrentRow.Cells("Email").Value.ToString
        et_homeAddress.Text = DataGridView1.CurrentRow.Cells("Home_Address").Value.ToString

    End Sub

    Public Sub LimpiarCampos()
        et_code.Clear()
        et_name.Clear()
        et_email.Clear()
        et_homeAddress.Clear()
        et_date.Value = Date.Now

        et_name.Focus()

    End Sub

    Private Sub CrearCode()
        Dim ValorCode As Integer = 0

        For Each Fila As DataRow In dtCliente.Rows
            If IsNumeric(Fila("Code")) = True Then
                If CInt(Fila("Code")) > CInt(ValorCode) Then
                    ValorCode = Fila("Code")
                End If

            End If
        Next

        ValorCode += 1
        et_code.Text = ValorCode

        et_name.Focus()
    End Sub




    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        Dim cadena As String = "provider=Microsoft.Jet.OLEDB.4.0;Data Source='basedatos.xls';Extended Properties=Excel 8.0;"


        Dim conn As New OleDbConnection(cadena)
        conn.Open()

        Dim cmd As New OleDbCommand("update [cliente$] set Code='0',Name='',Email='',Home_Address='' where Code='" & et_code.Text & "'", conn)
        cmd.ExecuteNonQuery()

        conn.Close()
        Llenar()
    End Sub

    Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        Dim cadena As String = "provider=Microsoft.Jet.OLEDB.4.0;Data Source='basedatos.xls';Extended Properties=Excel 8.0;"


        Dim conn As New OleDbConnection(cadena)
        conn.Open()

        Dim cmd As New OleDbCommand("update [cliente$] set Name='" & et_name.Text & "',Date_to_Birth='" & et_date.Text & "',Email='" & et_email.Text & "',Home_Address='" & et_homeAddress.Text & "' where Code='" & et_code.Text & "'", conn)
        cmd.ExecuteNonQuery()

        conn.Close()

        Llenar()
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim cadena As String = "provider=Microsoft.Jet.OLEDB.4.0;Data Source='basedatos.xls';Extended Properties=Excel 8.0;"

        Dim conn As New OleDbConnection(cadena)
        conn.Open()

        Dim bs As New BindingSource
        bs.DataSource = dtCliente

        bs.Filter = "Code ='0'"

        If bs.Count > 0 Then
            Dim cmd As New OleDbCommand("update [cliente$] set Code='" & et_code.Text & "',Name='" & et_name.Text & "',Date_to_Birth='" & et_date.Text & "',Email='" & et_email.Text & "',Home_Address='" & et_homeAddress.Text & "' where Code='0'", conn)
            cmd.ExecuteNonQuery()
        Else
            Dim cmd As New OleDbCommand("INSERT INTO [cliente$] values('" & et_code.Text & "','" & et_name.Text & "','" & et_date.Text & "','" & et_email.Text & "','" & et_homeAddress.Text & "')", conn)
            cmd.ExecuteNonQuery()
        End If

        conn.Close()
        Llenar()
        LimpiarCampos()

    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarCampos()

        CrearCode()

    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles et_search.TextChanged
        Llenar()
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If DataGridView1.RowCount > 0 Then
            LlenaTexto()
        Else
            LimpiarCampos()
        End If
    End Sub
End Class