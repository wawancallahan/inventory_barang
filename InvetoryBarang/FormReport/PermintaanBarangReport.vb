Public Class PermintaanBarangReport

    Private Sub PermintaanBarang_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim ds As New DataSet
        Dim dt As New DataTable("Permintaan")
        dt.Columns.Add("no")
        dt.Columns.Add("no_permintaan")
        dt.Columns.Add("nama")
        dt.Columns.Add("unit")
        dt.Columns.Add("kategori")
        dt.Columns.Add("pemasok")
        dt.Columns.Add("jumlah")

        Dim query As String = "SELECT purchase_order_details.id AS purchase_order_details_id, purchase_order_details.qty AS purchase_order_details_qty, purchase_order_details.qty_complete AS purchase_order_details_qty_complete, " & _
                                      "items.id AS items_id, items.name AS items_name, items.category AS items_category, items.unit AS items_unit, " & _
                                      "suppliers.name AS suppliers_name, " & _
                                      "purchase_orders.no AS purchase_orders_no " & _
                                      "FROM purchase_order_details " & _
                                      "JOIN items ON purchase_order_details.item_id = items.id " & _
                                      "JOIN suppliers ON purchase_order_details.supplier_id = suppliers.id " & _
                                      "JOIN purchase_orders ON purchase_order_details.purchase_order_id = purchase_orders.id"

        Try
            Dim nomor As Integer = 1
            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)
            _MySqlDataReader = _MySqlCommand.ExecuteReader

            If _MySqlDataReader.HasRows Then
                While _MySqlDataReader.Read
                    dt.Rows.Add({
                                nomor,
                                _MySqlDataReader.Item("purchase_orders_no"),
                                _MySqlDataReader.Item("items_name"),
                                _MySqlDataReader.Item("items_unit"),
                                _MySqlDataReader.Item("items_category"),
                                _MySqlDataReader.Item("suppliers_name"),
                                _MySqlDataReader.Item("purchase_order_details_qty")
                                })
                    nomor = nomor + 1
                End While
            End If



            Dim rpt As New PermintaanReport
            ds.Tables.Add(dt)
            rpt.SetDataSource(ds)

            CrystalReportViewer1.ReportSource = rpt

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        _MySqlDataReader.Close()

    End Sub
End Class