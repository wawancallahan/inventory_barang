Public Class BarangView

    Dim selectedId As String = Nothing
    Dim updateDB As Boolean = False

    Private Sub BarangView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.aturDgv()
        Me.getItems(dgv)
    End Sub

    Protected Sub aturDgv()
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
    End Sub

    Protected Sub getItems(dgv As DataGridView, Optional key As String = Nothing)
        Try
            dgv.Rows.Clear()

            Dim query As String

            If key <> Nothing Then

                query = "SELECT * FROM items " & _
                        "JOIN stocks ON items.id = stocks.id " & _
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

    Private Sub btnSimpan_Click(sender As Object, e As EventArgs) Handles btnSimpan.Click
        Dim data As Object = {
            txtNama.Text, _
            txtKategori.Text, _
            txtUnit.Text, _
            txtQty.Text, _
            txtLokasi.Text
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
            txtKategori.Text = .Cells("Kategori").Value
            txtLokasi.Text = .Cells("Lokasi").Value
            txtUnit.Text = .Cells("Unit").Value
            txtQty.Text = .Cells("Qty").Value
        End With

        updateDB = True
    End Sub
End Class