Public Class SupplierView

    Dim selectedId As String = Nothing
    Dim updateDB As Boolean = False

    Private Sub SupplierView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dgv.Columns.Clear()
        Me.aturDgv()
        Me.getItems(dgv)
    End Sub

    Protected Sub aturDgv()
        dgv.Columns.Clear()
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
        selectedId = Nothing

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

    Protected Function validateForm()
        Dim validate As Boolean = False
        Try
            If txtNama.Text = Nothing Or txtNama.Text.Length < 4 Then
                validate = False
            Else
                validate = True
            End If
        Catch ex As Exception

        End Try

        Return validate
    End Function

    Private Sub btnSimpan_Click(sender As Object, e As EventArgs) Handles btnSimpan.Click
        Dim data As Object = {
           txtNama.Text, _
           txtAlamat.Text, _
           txtTelepon.Text
        }

        If Not validateForm() Then
            MessageBox.Show("Tolong Isi Form Input", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
            getItems(dgv)
            Exit Sub
        End If

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

    Protected Function SupplierHasData(id As String)
        Try
            Dim data As Integer = 0
            Dim query As String = "SELECT COUNT(*) AS purchase_order_details_count FROM purchase_order_details WHERE supplier_id = '" & id & "'"
            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)
            _MySqlDataReader = _MySqlCommand.ExecuteReader

            If _MySqlDataReader.HasRows Then
                If _MySqlDataReader.Read Then
                    data = _MySqlDataReader.Item("purchase_order_details_count")
                End If
            End If

            _MySqlDataReader.Dispose()

            Return data > 0
        Catch ex As Exception
            _MySqlDataReader.Dispose()
            Return False
        End Try
    End Function

    Private Sub btnHapus_Click(sender As Object, e As EventArgs) Handles btnHapus.Click
        If selectedId = Nothing Then
            MessageBox.Show("Tidak ada data yang dipilih", "Perintagan", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else

            If SupplierHasData(selectedId) Then
                MessageBox.Show("Gagal menghapus data, Supplier ini sudah terelasi pada sebuah Transaksi", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Exit Sub
            End If

            If deleteData(selectedId) Then
                MessageBox.Show("Berhasil menghapus data", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Gagal menghapus data", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            Me.getItems(dgv)
            clearForm()
            updateDB = False
        End If
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

    Private Sub txtTelepon_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTelepon.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = Convert.ToChar(Keys.Back))
    End Sub

    Private Sub txtCari_TextChanged(sender As Object, e As EventArgs) Handles txtCari.TextChanged
        If txtCari.Text <> Nothing Then
            getItems(dgv, txtCari.Text)
        Else
            getItems(dgv)
        End If
    End Sub
End Class