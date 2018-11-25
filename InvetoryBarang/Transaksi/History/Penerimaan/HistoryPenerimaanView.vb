Public Class HistoryPenerimaanView

    Dim tanggal As Boolean = False

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        clearForm()
        getItems()
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
        dgv.Columns.Add("Id", "Id")
        dgv.Columns("Id").Visible = False
        dgv.Columns.Add("No", "No")
        dgv.Columns.Add("Tanggal", "Tanggal")
        dgv.Columns.Add("Jumlah Barang", "Jumlah Barang")
        dgv.Columns.Add("No Permintaan", "No Permintaan")
    End Sub

    Protected Sub getItems(Optional ByVal key As String = Nothing)
        Try
            dgv.Rows.Clear()

            Dim query As String

            If key <> Nothing Then
                query = "SELECT purchase_receipts.id AS purchase_receipts_id, purchase_receipts.no AS purchase_receipts_no, purchase_receipts.date AS purchase_receipts_date, " & _
                        " COUNT(purchase_receipt_details.purchase_receipt_id) AS purchase_receipt_details_count, " & _
                        " purchase_orders.no AS purchase_orders_no " & _
                        " FROM purchase_receipts LEFT JOIN purchase_receipt_details ON purchase_receipts.id = purchase_receipt_details.purchase_receipt_id " & _
                        " JOIN purchase_orders ON purchase_receipts.purchase_order_id = purchase_orders.id " & _
                        " WHERE (purchase_receipts.no LIKE '%" & key & "%' OR purchase_orders.no LIKE '%" & key & "%') "
                If tanggal = True Then
                    query &= " AND "
                End If
            Else
                query = "SELECT purchase_receipts.id AS purchase_receipts_id, purchase_receipts.no AS purchase_receipts_no, purchase_receipts.date AS purchase_receipts_date, " & _
                        " COUNT(purchase_receipt_details.purchase_receipt_id) AS purchase_receipt_details_count, " & _
                        " purchase_orders.no AS purchase_orders_no " & _
                        " FROM purchase_receipts LEFT JOIN purchase_receipt_details ON purchase_receipts.id = purchase_receipt_details.purchase_receipt_id " & _
                        " JOIN purchase_orders ON purchase_receipts.purchase_order_id = purchase_orders.id "
                If tanggal = True Then
                    query &= " WHERE "
                End If
            End If

            If tanggal = True Then
                query &= " (purchase_receipts.date >= '" & dtpTanggalMulai.Value.ToString("yyyy-MM-dd") & " 00:00:00" & "' AND purchase_receipts.date <= '" & dtpTanggalAkhir.Value.ToString("yyyy-MM-dd") & " 23:59:59" & "') "
            End If

            query &= " GROUP BY purchase_receipts.id"

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)
            _MySqlDataReader = _MySqlCommand.ExecuteReader

            If _MySqlDataReader.HasRows Then
                While _MySqlDataReader.Read
                    dgv.Rows.Add({
                                  _MySqlDataReader.Item("purchase_receipts_id"),
                                  _MySqlDataReader.Item("purchase_receipts_no"),
                                  Date.Parse(_MySqlDataReader.Item("purchase_receipts_date")).ToString("dd-MMMM-yyyy"),
                                  _MySqlDataReader.Item("purchase_receipt_details_count"),
                                  _MySqlDataReader.Item("purchase_orders_no")
                                 })
                End While
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        _MySqlDataReader.Close()
    End Sub

    Private Sub btnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        If dtpTanggalMulai.Value > dtpTanggalAkhir.Value Then
            MessageBox.Show("Tanggal mulai tidak boleh melebihi tanggal akhir", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            tanggal = True
            getItems(txtCari.Text)
        End If
    End Sub

    Private Sub txtCari_TextChanged(sender As Object, e As EventArgs) Handles txtCari.TextChanged
        If txtCari.Text <> Nothing Then
            getItems(txtCari.Text)
        Else
            getItems()
        End If
    End Sub

    Private Sub HistoryPenerimaanView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        clearForm()
        aturDgv()
        getItems()
    End Sub
End Class