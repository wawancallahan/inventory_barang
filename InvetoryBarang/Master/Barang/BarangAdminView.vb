Public Class BarangAdminView

    Private Sub BarangAdminView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dgv.Columns.Clear()
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

    Private Sub txtCari_TextChanged(sender As Object, e As EventArgs) Handles txtCari.TextChanged
        If txtCari.Text <> Nothing Then
            getItems(dgv, txtCari.Text)
        Else
            getItems(dgv)
        End If
    End Sub
End Class