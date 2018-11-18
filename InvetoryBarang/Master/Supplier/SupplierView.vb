Public Class SupplierView

    Dim selectedId As String = Nothing
    Dim updateDB As Boolean = False

    Private Sub SupplierView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.aturDgv()
        Me.getItems(dgv)
    End Sub

    Protected Sub aturDgv()
        dgv.Columns.Add("Id", "Id")
        dgv.Columns("Id").Visible = False

        dgv.Columns.Add("Nama", "Nama")
        dgv.Columns.Add("Alamat", "Alamat")
        dgv.Columns.Add("Telepon", "Telepon")

        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.RowHeadersVisible = False
        dgv.ReadOnly = True
        dgv.ClearSelection()
    End Sub

    Protected Sub clearForm()
        txtNama.Clear()
        txtAlamat.Clear()
        txtTelepon.Clear()

        dgv.ClearSelection()
        updateDB = False

        If dgv.RowCount > 0 Then
            dgv.CurrentRow.Selected = False
        End If
    End Sub

    Protected Sub getItems(dgv As DataGridView, Optional key As String = Nothing)
        Try
            dgv.Rows.Clear()

            Dim query As String

            If key <> Nothing Then

                query = "SELECT * FROM suppliers " & _
                        "WHERE name LIKE '%" & key & "%' OR address LIKE '%" & key & "%' OR phone LIKE '%" & key & "%'"

                _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            Else
                query = "SELECT * FROM suppliers"

                _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)
            End If

            _MySqlDataReader = _MySqlCommand.ExecuteReader

            If _MySqlDataReader.HasRows Then
                While _MySqlDataReader.Read

                    Dim data As String() = {
                        _MySqlDataReader.Item("id").ToString, _
                        _MySqlDataReader.Item("name").ToString, _
                        _MySqlDataReader.Item("address").ToString, _
                        _MySqlDataReader.Item("phone").ToString
                    }

                    dgv.Rows.Add(data)
                End While
            End If

            _MySqlCommand.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        _MySqlDataReader.Close()
    End Sub

    Public Function insertData(data As Object)
        Try
            Dim query As String = "INSERT INTO suppliers VALUES ('', @nama, @alamat, @telepon)"

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            With _MySqlCommand.Parameters
                .AddWithValue("@nama", data(0))
                .AddWithValue("@alamat", data(1))
                .AddWithValue("@telepon", data(2))
            End With

            _MySqlCommand.ExecuteNonQuery()

            _MySqlCommand.Dispose()

            Return True
        Catch ex As Exception
            MessageBox.Show(ex.Message)

            _MySqlCommand.Dispose()

            Return False
        End Try
    End Function

    Public Function updateData(data As Object, id As Integer)
        Try
            Dim query As String = "UPDATE suppliers SET name=@nama, address=@alamat, phone=@telepon " & _
                                  "WHERE id=@id"

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            With _MySqlCommand.Parameters
                .AddWithValue("@nama", data(0))
                .AddWithValue("@alamat", data(1))
                .AddWithValue("@telepon", data(2))
                .AddWithValue("@id", id)
            End With

            _MySqlCommand.ExecuteNonQuery()

            Return True
        Catch ex As Exception
            MessageBox.Show(ex.Message)

            Return False
        End Try
    End Function

    Public Function deleteData(id As Integer)
        Try
            Dim query As String = "DELETE FROM suppliers WHERE id=@id"

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)
            With _MySqlCommand.Parameters
                .AddWithValue("@id", id)
            End With

            _MySqlCommand.ExecuteNonQuery()

            Return True
        Catch ex As Exception
            MessageBox.Show(ex.Message)

            Return False
        End Try
    End Function

    Private Sub btnSimpan_Click(sender As Object, e As EventArgs) Handles btnSimpan.Click
        Dim data As Object = {
           txtNama.Text, _
           txtAlamat.Text, _
           txtTelepon.Text
       }

        Select Case updateDB
            Case True
                If updateData(data, selectedId) Then
                    MessageBox.Show("Berhasil mengedit data", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("Gagal mengedit data", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Case False
                If insertData(data) Then
                    MessageBox.Show("Berhasil menambah data", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("Gagal menambah data", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
        End Select


        Me.getItems(dgv)
        clearForm()
    End Sub

    Private Sub btnBatal_Click(sender As Object, e As EventArgs) Handles btnBatal.Click
        clearForm()
    End Sub

    Private Sub btnKeluar_Click(sender As Object, e As EventArgs) Handles btnKeluar.Click
        clearForm()
        Me.Hide()
    End Sub

    Private Sub btnHapus_Click(sender As Object, e As EventArgs) Handles btnHapus.Click
        If deleteData(selectedId) Then
            MessageBox.Show("Berhasil menghapus data", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Gagal menghapus data", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        Me.getItems(dgv)
        clearForm()
        updateDB = False
    End Sub

    Private Sub dgv_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv.CellClick
        With dgv.Rows(e.RowIndex)
            selectedId = .Cells("Id").Value
            txtNama.Text = .Cells("Nama").Value
            txtAlamat.Text = .Cells("Alamat").Value
            txtTelepon.Text = .Cells("Telepon").Value
        End With

        updateDB = True
    End Sub
End Class