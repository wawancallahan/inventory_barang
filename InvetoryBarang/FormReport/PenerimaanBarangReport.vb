Public Class PenerimaanBarangReport

    Private Sub PemerimaanBarangReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim ds As New DataSet
        Dim dt As New DataTable("Penerimaan")
        dt.Columns.Add("no")
        dt.Columns.Add("no_penerimaan")
        dt.Columns.Add("nama")
        dt.Columns.Add("unit")
        dt.Columns.Add("kategori")
        dt.Columns.Add("pemasok")
        dt.Columns.Add("jumlah")

        Dim query As String = "SELECT purchase_receipt_details.qty AS purchase_receipt_details_qty, " & _
                                      " pod.items_name AS pod_items_name, pod.items_category AS pod_items_category, pod.items_unit AS pod_items_unit, " & _
                                      " pod.suppliers_name AS pod_suppliers_name, " & _
                                      " purchase_receipts.no AS purchase_receipts_no " & _
                                      " FROM purchase_receipt_details " & _
                                      " JOIN purchase_receipts ON purchase_receipt_details.purchase_receipt_id = purchase_receipts.id " & _
                                      " JOIN ( " & _
                                      " SELECT purchase_order_details.id AS purchase_order_details_id, purchase_order_details.qty AS purchase_order_details_qty, purchase_order_details.qty_complete AS purchase_order_details_qty_complete, " & _
                                      " items.name AS items_name, items.category AS items_category, items.unit AS items_unit, " & _
                                      " suppliers.name AS suppliers_name " & _
                                      " FROM purchase_order_details " & _
                                      " JOIN items ON purchase_order_details.item_id = items.id " & _
                                      " JOIN suppliers ON purchase_order_details.supplier_id = suppliers.id " & _
                                      " ) AS pod ON purchase_receipt_details.purchase_order_detail_id = pod.purchase_order_details_id "

        Try
            Dim nomor As Integer = 1
            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)
            _MySqlDataReader = _MySqlCommand.ExecuteReader

            If _MySqlDataReader.HasRows Then
                While _MySqlDataReader.Read
                    dt.Rows.Add({
                                nomor,
                                _MySqlDataReader.Item("purchase_receipts_no"),
                                _MySqlDataReader.Item("pod_items_name"),
                                _MySqlDataReader.Item("pod_items_unit"),
                                _MySqlDataReader.Item("pod_items_category"),
                                _MySqlDataReader.Item("pod_suppliers_name"),
                                _MySqlDataReader.Item("purchase_receipt_details_qty")
                                })
                    nomor = nomor + 1
                End While
            End If



            Dim rpt As New PenerimaanReport
            ds.Tables.Add(dt)
            rpt.SetDataSource(ds)

            CrystalReportViewer1.ReportSource = rpt

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        _MySqlDataReader.Close()
    End Sub
End Class