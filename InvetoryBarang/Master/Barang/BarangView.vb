Public Class BarangView

    Dim selectedId As String = Nothing
    Dim updateDB As Boolean = False

    Private Sub BarangView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dgv.Columns.Clear()
        Me.aturDgv()
        Me.getItems(dgv)
    End Sub

    Protected Sub aturDgv()
        dgv.Columns.Clear()
        dgv.Columns.Add("Id", "Id")
        dgv.Columns("Id").Visible = False

        dgv.Columns.Add("Nama", "Nama")
        dgv.Columns.Add("Kategori", "Kategori")
        dgv.Columns.Add("Unit", "Unit")
        dgv.Columns.Add("Qty", "Qty")
        dgv.Columns.Add("Lokasi", "Lokasi")

        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.RowHeadersVisible = False
        dgv.ReadOnly = True
        dgv.ClearSelection()
    End Sub

    Protected Sub clearForm()
        txtNama.Clear()
        txtKategori.Clear()
        txtLokasi.Clear()
        txtUnit.Clear()
        txtQty.Clear()

        dgv.ClearSelection()
        If dgv.RowCount > 0 Then
            dgv.CurrentRow.Selected = False
        End If
        updateDB = False
        selectedId = Nothing
    End Sub

    Protected Sub getItems(dgv As DataGridView, Optional key As String = Nothing)
        Try
            dgv.Rows.Clear()

            Dim query As String

            If key <> Nothing Then

                query = "SELECT items.id AS items_id, items.name AS items_name, items.category AS items_category, items.unit AS items_unit," & _
                        "stocks.qty AS stocks_qty, stocks.location AS stocks_location " & _
                        "FROM items " & _
                        "JOIN stocks ON items.id = stocks.item_id " & _
                        "WHERE items.name LIKE '%" & key & "%' OR items.category LIKE '%" & key & "%' OR items.unit LIKE '%" & key & "%' " & _
                        "OR stocks.location LIKE '%" & key & "%'"

                _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            Else
                query = "SELECT items.id AS items_id, items.name AS items_name, items.category AS items_category, items.unit AS items_unit," & _
                        "stocks.qty AS stocks_qty, stocks.location AS stocks_location " & _
                        "FROM items " & _
                        "JOIN stocks ON items.id = stocks.item_id "
                _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)
            End If

            _MySqlDataReader = _MySqlCommand.ExecuteReader

            If _MySqlDataReader.HasRows Then

                While _MySqlDataReader.Read

                    Dim data As String() = {
                        _MySqlDataReader.Item("items_id").ToString, _
                        _MySqlDataReader.Item("items_name").ToString, _
                        _MySqlDataReader.Item("items_category").ToString, _
                        _MySqlDataReader.Item("items_unit").ToString, _
                        _MySqlDataReader.Item("stocks_qty").ToString, _
                        _MySqlDataReader.Item("stocks_location").ToString
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
            Dim query As String = "INSERT INTO items VALUES ('', @nama, @kategori, @unit); SELECT LAST_INSERT_ID()"

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            With _MySqlCommand.Parameters
                .AddWithValue("@nama", data(0))
                .AddWithValue("@kategori", data(1))
                .AddWithValue("@unit", data(2))
            End With

            Dim itemId As Integer = CInt(_MySqlCommand.ExecuteScalar())

            _MySqlCommand.Dispose()

            query = "INSERT INTO stocks VALUES ('', @item_id, @qty, @lokasi)"

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            With _MySqlCommand.Parameters
                .AddWithValue("@item_id", itemId.ToString)
                .AddWithValue("@qty", data(3))
                .AddWithValue("@lokasi", data(4))
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
            Dim query As String = "UPDATE items SET name=@nama, category=@kategori, unit=@unit " & _
                                  "WHERE id=@id; " & _
                                  "UPDATE stocks SET qty=@qty, location=@lokasi " & _
                                  "WHERE item_id=@id"

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            With _MySqlCommand.Parameters
                .AddWithValue("@nama", data(0))
                .AddWithValue("@kategori", data(1))
                .AddWithValue("@unit", data(2))
                .AddWithValue("@qty", data(3))
                .AddWithValue("@lokasi", data(4))
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
            Dim query As String = "DELETE FROM stocks WHERE item_id=@id;" & _
                                  "DELETE FROM items WHERE id=@id"

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
            If txtNama.Text = Nothing Or txtKategori.Text = Nothing Or txtUnit.Text = Nothing Or _
                txtQty.Text = Nothing Or Not IsNumeric(txtQty.Text) Or CInt(Val(txtQty.Text)) < 0 Then
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
            txtKategori.Text, _
            txtUnit.Text, _
            txtQty.Text, _
            txtLokasi.Text
        }

        If Not validateForm() Then
            MessageBox.Show("Tolong Isi Form Input Dengan Benar", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    Public Function BarangHasData(id As String)
        Try
            Dim data As Integer = 0
            Dim query As String = "SELECT COUNT(*) AS purchase_order_details_count FROM purchase_order_details WHERE item_id = '" & id & "'"
            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)
            _MySqlDataReader = _MySqlCommand.ExecuteReader

            If _MySqlDataReader.HasRows Then
                If _MySqlDataReader.Read Then
                    data = data + _MySqlDataReader.Item("purchase_order_details_count")
                End If
            End If

            _MySqlDataReader.Dispose()

            query = "SELECT COUNT(*) AS delivery_details_count FROM delivery_details WHERE item_id = '" & id & "'"
            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)
            _MySqlDataReader = _MySqlCommand.ExecuteReader

            If _MySqlDataReader.HasRows Then
                If _MySqlDataReader.Read Then
                    data = data + _MySqlDataReader.Item("delivery_details_count")
                End If
            End If

            _MySqlDataReader.Dispose()

            Return data > 0
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub btnHapus_Click(sender As Object, e As EventArgs) Handles btnHapus.Click
        If selectedId = Nothing Then
            MessageBox.Show("Tidak ada data yang dipilih", "Perintagan", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else

            If BarangHasData(selectedId) Then
                MessageBox.Show("Gagal menghapus data, Barang ini sudah terelasi pada sebuah Transaksi", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Information)

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
            selectedId = Nothing
        End If
    End Sub

    Private Sub dgv_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv.CellClick
        If e.RowIndex < 0 Then
            Exit Sub
        End If

        With dgv.Rows(e.RowIndex)
            selectedId = .Cells("Id").Value
            txtNama.Text = .Cells("Nama").Value
            txtKategori.Text = .Cells("Kategori").Value
            txtLokasi.Text = .Cells("Lokasi").Value
            txtUnit.Text = .Cells("Unit").Value
            txtQty.Text = .Cells("Qty").Value
        End With

        updateDB = True
    End Sub

    Private Sub txtQty_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtQty.KeyPress
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