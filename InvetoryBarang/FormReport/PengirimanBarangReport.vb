Public Class PengirimanBarangReport

    Private Sub PengirimanBarangReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim ds As New DataSet
        Dim dt As New DataTable("pengiriman")
        dt.Columns.Add("no")
        dt.Columns.Add("no_pengiriman")
        dt.Columns.Add("tujuan")
        dt.Columns.Add("nama")
        dt.Columns.Add("kategori")
        dt.Columns.Add("unit")
        dt.Columns.Add("jumlah")

        Dim query As String = "SELECT delivery_details.qty AS delivery_details_qty, " & _
                                      "items.id AS items_id, items.name AS items_name, items.category AS items_category, items.unit AS items_unit, " & _
                                      "deliveries.no AS deliveries_no, deliveries.destination AS deliveries_destination " & _
                                      "FROM delivery_details " & _
                                      "JOIN items ON delivery_details.item_id = items.id " & _
                                      "JOIN deliveries ON delivery_details.delivery_id = deliveries.id"


        Try
            Dim nomor As Integer = 1
            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)
            _MySqlDataReader = _MySqlCommand.ExecuteReader

            If _MySqlDataReader.HasRows Then
                While _MySqlDataReader.Read
                    dt.Rows.Add({
                                nomor,
                                _MySqlDataReader.Item("deliveries_no"),
                                _MySqlDataReader.Item("deliveries_destination"),
                                _MySqlDataReader.Item("items_name"),
                                _MySqlDataReader.Item("items_category"),
                                _MySqlDataReader.Item("items_unit"),
                                _MySqlDataReader.Item("delivery_details_qty")
                                })
                    nomor = nomor + 1
                End While
            End If



            Dim rpt As New PengirimanReport
            ds.Tables.Add(dt)
            rpt.SetDataSource(ds)

            CrystalReportViewer1.ReportSource = rpt

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        _MySqlDataReader.Close()

    End Sub
End Class