Public Class PermintaanView

    Private Sub PermintaanView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.getItems()
        txtJumlah.ReadOnly = True
        txtJumlah.Text = 0
    End Sub

    Public Sub aturDgv()
        dgv.Columns.Add("Id", "Id")
        dgv.Columns("Id").Visible = False

        dgv.Columns.Add("Nama", "Nama")
        dgv.Columns.Add("Kategori", "Kategori")
        dgv.Columns.Add("Unit", "Unit")
        dgv.Columns.Add("Qty", "Qty")
        dgv.Columns.Add("Lokasi", "Lokasi")
        dgv.Columns.Add("Qty Minta", "Qty Minta")

        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.RowHeadersVisible = False
        dgv.ClearSelection()
    End Sub

    Public Sub clearForm()
        dgv.Rows.Clear()
        dgv.ClearSelection()
        If dgv.RowCount > 0 Then
            dgv.CurrentRow.Selected = False
        End If
        dtpTanggal.Value = Date.Now

    End Sub

    Public Sub getItems(Optional ByVal key As String = Nothing)
        Dim itemList As List(Of ListObject) = New List(Of ListObject)()

        Try
            Dim query As String

            If key <> Nothing Then

                query = "SELECT * FROM items " & _
                        "WHERE name LIKE '%" & key & "%'"

                _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            Else
                query = "SELECT * FROM items"

                _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)
            End If

            _MySqlDataReader = _MySqlCommand.ExecuteReader

            If _MySqlDataReader.HasRows Then
                While _MySqlDataReader.Read

                    itemList.Add(New ListObject(_MySqlDataReader.Item("name"), _MySqlDataReader.Item("id")))

                End While
            End If

            _MySqlCommand.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        LBItems.DataSource = itemList
        LBItems.ValueMember = "Value"
        LBItems.DisplayMember = "Text"

        _MySqlDataReader.Close()
    End Sub

    Private Sub btnBatal_Click(sender As Object, e As EventArgs) Handles btnBatal.Click

    End Sub

    Private Sub LBItems_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LBItems.SelectedIndexChanged

    End Sub
End Class