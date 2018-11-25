Public Class HistoryPermintaanView

    Dim tanggal As Boolean = False

    Private Sub HistoryPermintaanView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        clearForm()
        aturDgv()
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
        dgv.Columns.Add("Status", "Status")
        dgv.Columns.Add("Jumlah Barang", "Jumlah Barang")
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
                                  _MySqlDataReader.Item("purchase_order_details_count")
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
End Class