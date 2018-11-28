Public Class FormReport

    Private Sub FormReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call Koneksi()

        Dim rpt As New BarangReport

        Dim ds As New DataSet
        Dim dt As New DataTable("DataTable1")
        dt.Columns.Add("nama")

        Dim query As String = "SELECT * FROM items"
        _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)
        _MySqlDataReader = _MySqlCommand.ExecuteReader

        If _MySqlDataReader.HasRows Then
            While _MySqlDataReader.Read
                dt.Rows.Add(_MySqlDataReader.Item("id"))
            End While
        End If

        _MySqlDataReader.Close()

        ds.Tables.Add(dt)

        rpt.SetDataSource(ds)

        CrystalReportViewer1.ReportSource = rpt
    End Sub

End Class