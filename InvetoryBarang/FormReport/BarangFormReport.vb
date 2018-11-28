Public Class BarangFormReport

    Private Sub CrystalReportViewer1_Load(sender As Object, e As EventArgs) Handles CrystalReportViewer1.Load

    End Sub

    Private Sub BarangFormReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim ds As New DataSet
        Dim dt As New DataTable("DataTable1")
        dt.Columns.Add("no")
        dt.Columns.Add("nama")
        dt.Columns.Add("unit")
        dt.Columns.Add("kategori")
        dt.Columns.Add("stok")
        dt.Columns.Add("lokasi")

        Dim query As String = "SELECT items.id AS items_id, items.name AS items_name, items.category AS items_category, items.unit AS items_unit, " & _
                                " stocks.id AS stocks_id, stocks.item_id AS stocks_item_id, stocks.qty AS stocks_qty, stocks.location AS stocks_location " & _
                                " FROM items JOIN stocks ON items.id=stocks.item_id"

        Try
            Dim nomor As Integer = 1
            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)
            _MySqlDataReader = _MySqlCommand.ExecuteReader

            If _MySqlDataReader.HasRows Then
                While _MySqlDataReader.Read
                    dt.Rows.Add({
                                nomor,
                                _MySqlDataReader.Item("items_name"),
                                _MySqlDataReader.Item("items_unit"),
                                _MySqlDataReader.Item("items_category"),
                                _MySqlDataReader.Item("stocks_qty"),
                                _MySqlDataReader.Item("stocks_location")
                                })
                    nomor = nomor + 1
                End While
            End If



            Dim rpt As New BarangReport
            ds.Tables.Add(dt)
            rpt.SetDataSource(ds)

            CrystalReportViewer1.ReportSource = rpt

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        _MySqlDataReader.Close()

    End Sub
End Class