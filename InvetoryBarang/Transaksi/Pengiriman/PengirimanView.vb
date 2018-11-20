Public Class PengirimanView

    Private Sub PengirimanView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        aturDgv()
        clearForm()
        getItemsBarang()
    End Sub

    Public Sub aturDgv()
        dgv.EditMode = DataGridViewEditMode.EditOnEnter
        dgv.Columns.Add("Id", "Id")
        dgv.Columns("Id").Visible = False
        dgv.Columns.Add("Barang", "Barang")
        dgv.Columns.Add("Old Qty", "Old Qty")
        dgv.Columns("Old Qty").Visible = False
        dgv.Columns.Add("Jumlah", "Jumlah")
    End Sub

    Public Sub clearForm()
        clearDataDgv()
        txtTujuan.Clear()
        txtNo.Clear()
        dtpTanggal.Value = Date.Now
        dgv.Rows.Clear()
        dgv.ClearSelection()
        If dgv.RowCount > 0 Then
            dgv.CurrentRow.Selected = False
        End If
    End Sub

    Public Sub getItemsBarang()
        Try
            cmbBarang.Items.Clear()
            Dim query As String = "SELECT items.id AS items_id, items.name AS items_name, items.category AS items_category, items.unit AS items_unit," & _
                                  "stocks.qty AS stocks_qty, stocks.location AS stocks_location " & _
                                  "FROM items " & _
                                  "JOIN stocks ON items.id = stocks.item_id "

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            _MySqlDataReader = _MySqlCommand.ExecuteReader

            If _MySqlDataReader.HasRows Then
                While _MySqlDataReader.Read
                    cmbBarang.Items.Add(New ListObject(_MySqlDataReader.Item("items_name").ToString, _MySqlDataReader.Item("items_id").ToString, {_MySqlDataReader.Item("stocks_qty")}))
                End While
            End If

            _MySqlCommand.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        _MySqlDataReader.Close()
    End Sub

    Public Sub clearDataDgv()
        cmbBarang.SelectedIndex = -1
        txtQty.Clear()
        txtMax.Clear()
    End Sub

    Private Sub btnTambah_Click(sender As Object, e As EventArgs) Handles btnTambah.Click
        Dim itemSelected As ListObject = DirectCast(cmbBarang.SelectedItem, ListObject)

        Dim data() As String = {
            itemSelected.Value,
            itemSelected.Text,
            txtMax.Text,
            txtQty.Text
        }

        dgv.Rows.Add(data)
        clearDataDgv()
    End Sub

    Private Sub btnKeluar_Click(sender As Object, e As EventArgs) Handles btnKeluar.Click
        clearForm()
        Me.Hide()
    End Sub

    Private Sub btnBatal_Click(sender As Object, e As EventArgs) Handles btnBatal.Click
        clearForm()
    End Sub

    Private Sub btnSimpan_Click(sender As Object, e As EventArgs) Handles btnSimpan.Click
        Try
            Dim query As String = "INSERT INTO deliveries VALUES ('', @no, @date); SELECT LAST_INSERT_ID()"

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            With _MySqlCommand.Parameters
                .AddWithValue("@no", txtNo.Text)
                .AddWithValue("@date", dtpTanggal.Value)
            End With

            Dim deliveryId As Integer = CInt(_MySqlCommand.ExecuteScalar())

            _MySqlCommand.Dispose()

            For Each row As DataGridViewRow In dgv.Rows
                query = "INSERT INTO delivery_details VALUES ('', @delivery_id, @item_id, @qty)"

                _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

                With _MySqlCommand.Parameters
                    .AddWithValue("@delivery_id", deliveryId)
                    .AddWithValue("@item_id", row.Cells("Id").Value)
                    .AddWithValue("@qty", row.Cells("Jumlah").Value)
                End With

                _MySqlCommand.ExecuteNonQuery()

                Dim qty As Integer = 0

                query = "SELECT * FROM stocks WHERE item_id = '" & row.Cells("Id").Value & "'"

                _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

                _MySqlDataReader = _MySqlCommand.ExecuteReader
                If _MySqlDataReader.HasRows Then
                    While _MySqlDataReader.Read
                        qty += CInt(_MySqlDataReader.Item("qty").ToString)
                    End While
                End If

                _MySqlDataReader.Close()

                query = "UPDATE stocks SET qty = '" & qty - row.Cells("Jumlah").Value & "' " & _
                        "WHERE item_id = '" & row.Cells("Id").Value & "'"

                _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

                _MySqlCommand.ExecuteNonQuery()
            Next

            MessageBox.Show("Berhasil menambah data", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)

            clearForm()
            getItemsBarang()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmbBarang_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbBarang.SelectedIndexChanged
        If cmbBarang.SelectedIndex > -1 Then
            Dim itemSelected As ListObject = DirectCast(cmbBarang.SelectedItem, ListObject)
            txtMax.Text = itemSelected.Data(0)
        Else
            txtMax.Clear()
        End If
    End Sub
End Class