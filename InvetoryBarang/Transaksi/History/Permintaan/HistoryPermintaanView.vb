Public Class HistoryPermintaanView

    Dim tanggal As Boolean = False

    Dim selectedId As Boolean = False
    Dim status As Boolean

    Private Sub HistoryPermintaanView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        clearForm()
        aturDgv()
        getItems()

        If dgv.RowCount > 0 Then
            dgv.CurrentRow.Selected = False
        End If
    End Sub

    Protected Sub clearForm()
        dtpTanggalMulai.Value = Date.Now
        dtpTanggalAkhir.Value = Date.Now
        txtCari.Clear()
        dgv.Rows.Clear()
        dgv.ClearSelection()
        If dgv.RowCount > 0 Then
            dgv.CurrentRow.Selected = False
        End If
        tanggal = False
    End Sub

    Protected Sub aturDgv()
        dgv.Columns.Clear()
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.Columns.Add("Id", "Id")
        dgv.Columns("Id").Visible = False
        dgv.Columns.Add("No", "No")
        dgv.Columns.Add("Tanggal", "Tanggal")
        dgv.Columns.Add("Status", "Status")
        dgv.Columns.Add("Jumlah Barang", "Jumlah Barang")
        dgv.Columns.Add("status_permintaan", "status_permintaan")
        dgv.Columns("status_permintaan").Visible = False
    End Sub

    Protected Sub getItems(Optional ByVal key As String = Nothing)
        Try
            dgv.Rows.Clear()

            Dim query As String

            If key <> Nothing Then
                query = "SELECT purchase_orders.id AS purchase_orders_id, purchase_orders.no AS purchase_orders_no, purchase_orders.date AS purchase_orders_date, purchase_orders.status AS purchase_orders_status, " & _
                        " COUNT(purchase_order_details.purchase_order_id) AS purchase_order_details_count " & _
                        " FROM purchase_orders LEFT JOIN purchase_order_details on purchase_orders.id = purchase_order_details.purchase_order_id " & _
                        "  WHERE (no LIKE '%" & key & "%') "
                If tanggal = True Then
                    query &= " AND "
                End If
            Else
                query = "SELECT purchase_orders.id AS purchase_orders_id, purchase_orders.no AS purchase_orders_no, purchase_orders.date AS purchase_orders_date, purchase_orders.status AS purchase_orders_status, " & _
                        " COUNT(purchase_order_details.purchase_order_id) AS purchase_order_details_count " & _
                        " FROM purchase_orders LEFT JOIN purchase_order_details on purchase_orders.id = purchase_order_details.purchase_order_id "
                If tanggal = True Then
                    query &= " WHERE "
                End If
            End If

            If tanggal = True Then
                query &= " (date >= '" & dtpTanggalMulai.Value.ToString("yyyy-MM-dd") & " 00:00:00" & "' AND date <= '" & dtpTanggalAkhir.Value.ToString("yyyy-MM-dd") & " 23:59:59" & "') "
            End If

            query &= " GROUP BY purchase_orders.id"

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)
            _MySqlDataReader = _MySqlCommand.ExecuteReader

            If _MySqlDataReader.HasRows Then
                While _MySqlDataReader.Read
                    dgv.Rows.Add({
                                  _MySqlDataReader.Item("purchase_orders_id"),
                                  _MySqlDataReader.Item("purchase_orders_no"),
                                  Date.Parse(_MySqlDataReader.Item("purchase_orders_date")).ToString("dd-MMMM-yyyy"),
                                  If(_MySqlDataReader.Item("purchase_orders_status") = 1, "Sesuai", "Belum Sesuai"),
                                  _MySqlDataReader.Item("purchase_order_details_count"),
                                  If(_MySqlDataReader.Item("purchase_orders_status") = 1, True, False)
                                 })
                End While
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        _MySqlDataReader.Close()
    End Sub

    Private Sub txtCari_TextChanged(sender As Object, e As EventArgs) Handles txtCari.TextChanged
        If txtCari.Text <> Nothing Then
            getItems(txtCari.Text)
        Else
            getItems()
        End If
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        clearForm()
        getItems()
    End Sub

    Private Sub btnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        If dtpTanggalMulai.Value > dtpTanggalAkhir.Value Then
            MessageBox.Show("Tanggal mulai tidak boleh melebihi tanggal akhir", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            tanggal = True
            getItems(txtCari.Text)
        End If
    End Sub

    Private Sub dgv_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv.CellDoubleClick
        With dgv.CurrentRow
            Try
                With FormHistory
                    .dgv.Rows.Clear()
                    .dgv.Columns.Clear()
                    .dgv.Columns.Add("Nama", "Nama")
                    .dgv.Columns.Add("Kategori", "Kategori")
                    .dgv.Columns.Add("Unit", "Unit")
                    .dgv.Columns.Add("Supplier", "Supplier")
                    .dgv.Columns.Add("Qty Minta", "Qty Minta")
                    .dgv.Columns.Add("Qty Diterima", "Qty Diterima")
                End With

                Dim query As String = "SELECT purchase_order_details.id AS purchase_order_details_id, purchase_order_details.qty AS purchase_order_details_qty, purchase_order_details.qty_complete AS purchase_order_details_qty_complete, " & _
                                      "items.id AS items_id, items.name AS items_name, items.category AS items_category, items.unit AS items_unit, " & _
                                      "suppliers.name AS suppliers_name " & _
                                      "FROM purchase_order_details " & _
                                      "JOIN items ON purchase_order_details.item_id = items.id " & _
                                      "JOIN suppliers ON purchase_order_details.supplier_id = suppliers.id " & _
                                      "WHERE purchase_order_details.purchase_order_id = '" & .Cells(0).Value & "'"

                _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)
                _MySqlDataReader = _MySqlCommand.ExecuteReader

                If _MySqlDataReader.HasRows Then
                    While _MySqlDataReader.Read

                        FormHistory.dgv.Rows.Add({
                                      _MySqlDataReader.Item("items_name"),
                                      _MySqlDataReader.Item("items_category"),
                                      _MySqlDataReader.Item("items_unit"),
                                      _MySqlDataReader.Item("suppliers_name"),
                                      _MySqlDataReader.Item("purchase_order_details_qty"),
                                      _MySqlDataReader.Item("purchase_order_details_qty_complete")
                                     })
                    End While
                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try

            _MySqlDataReader.Close()

            FormHistory.ShowDialog()
        End With
    End Sub

    Private Sub dgv_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv.CellClick
        If e.RowIndex < 0 Then
            Exit Sub
        End If

        With dgv.Rows(e.RowIndex)
            selectedId = .Cells("Id").Value
            status = .Cells("status_permintaan").Value
        End With
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If selectedId = False Then
            MessageBox.Show("Pilih dahulu data yang ingin diganti statusnya", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Try
            Dim query As String = "UPDATE purchase_orders SET status = '" & If(status, 0, 1) & "'"
            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)
            _MySqlCommand.ExecuteNonQuery()

            clearForm()
            getItems()

            If dgv.RowCount > 0 Then
                dgv.CurrentRow.Selected = False
            End If

            MessageBox.Show("Status telah terganti", "Pemberitahuan", MessageBoxButtons.OK, MessageBoxIcon.Information)

            selectedId = False
            status = False
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class