Public Class PermintaanView

    Dim ignoreSelectedIndexChanged As Boolean = True
    Dim ListItems As Object

    Private Sub PermintaanView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        clearForm()
        Me.getItems()
        txtJumlah.ReadOnly = True
        txtJumlah.Text = 0
        aturDgv()
        ignoreSelectedIndexChanged = False
        getItemsSupplier()
        txtNo.ReadOnly = True
        txtNo.Text = autoNomor()
    End Sub

    Protected Function autoNomor()
        Dim no As String

        Dim query As String = "SELECT COUNT(*) AS items_count FROM purchase_orders"

        _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

        _MySqlDataReader = _MySqlCommand.ExecuteReader

        If _MySqlDataReader.HasRows Then
            If _MySqlDataReader.Read Then
                no = CInt(_MySqlDataReader.Item("items_count")) + 1
            End If
        End If

        _MySqlCommand.Dispose()
        _MySqlDataReader.Close()

        Return Date.Now.ToString("ddMMyyyy") & no
    End Function

    Public Sub aturDgv()
        dgv.Columns.Clear()
        dgv.Columns.Add("Id", "Id")
        dgv.Columns("Id").Visible = False

        dgv.Columns.Add("Nama", "Nama")
        dgv.Columns.Add("Kategori", "Kategori")
        dgv.Columns.Add("Unit", "Unit")
        dgv.Columns.Add("Qty", "Qty")
        dgv.Columns.Add("Lokasi", "Lokasi")

        dgv.Columns.Add("SupplierId", "SupplierId")
        dgv.Columns("SupplierId").Visible = False

        dgv.Columns.Add("Supplier", "Supplier")
        dgv.Columns.Add("Qty Minta", "Qty Minta")

        dgv.RowHeadersVisible = False
        dgv.ReadOnly = True
        dgv.ClearSelection()
    End Sub

    Public Sub getItemsSupplier()
        Dim query As String = "SELECT * FROM suppliers"

        _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

        _MySqlDataReader = _MySqlCommand.ExecuteReader

        If _MySqlDataReader.HasRows Then

            While _MySqlDataReader.Read
                cmbSupplier.Items.Add(New ListObject(_MySqlDataReader.Item("name").ToString(), _MySqlDataReader.Item("id").ToString()))
            End While

        End If

        _MySqlCommand.Dispose()
        _MySqlDataReader.Close()
    End Sub

    Public Sub clearForm()
        dgv.Rows.Clear()
        dgv.ClearSelection()
        If dgv.RowCount > 0 Then
            dgv.CurrentRow.Selected = False
        End If
        txtNo.Clear()
        dtpTanggal.Value = Date.Now
        txtJumlah.Text = 0
        cmbBarang.SelectedIndex = -1
        cmbSupplier.SelectedIndex = -1
        txtQty.Clear()

        txtNo.Text = autoNomor()
    End Sub

    Public Sub getItems()

        Try
            Dim query As String = "SELECT * FROM items"

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            _MySqlDataReader = _MySqlCommand.ExecuteReader

            If _MySqlDataReader.HasRows Then
                While _MySqlDataReader.Read
                    cmbBarang.Items.Add(New ListObject(_MySqlDataReader.Item("name").ToString, _MySqlDataReader.Item("id").ToString))
                End While
            End If

            _MySqlCommand.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        _MySqlDataReader.Close()
    End Sub

    Private Sub btnBatal_Click(sender As Object, e As EventArgs) Handles btnBatal.Click
        clearForm()
    End Sub

    Private Sub dgv_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dgv.KeyPress
        On Error Resume Next
        If e.KeyChar = Chr(27) Then
            dgv.Rows.RemoveAt(dgv.CurrentRow.Index)
        End If

        jumlahBarang()
    End Sub

    Public Sub jumlahBarang()
        txtJumlah.Text = dgv.RowCount
    End Sub

    Private Sub btnSimpan_Click(sender As Object, e As EventArgs) Handles btnSimpan.Click
        Try

            If txtNo.Text = Nothing Or dtpTanggal.Value = Nothing Or dgv.Rows.Count <= 0 Then
                MessageBox.Show("Tolong Isi Form Input Dengan Benar", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            Dim query As String = "INSERT INTO purchase_orders VALUES ('', @no, @date, @status); SELECT LAST_INSERT_ID()"

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            With _MySqlCommand.Parameters
                .AddWithValue("@no", txtNo.Text)
                .AddWithValue("@date", dtpTanggal.Value)
                .AddWithValue("@status", False)
            End With

            Dim purchaseOrderId As Integer = CInt(_MySqlCommand.ExecuteScalar())

            _MySqlCommand.Dispose()

            For Each row As DataGridViewRow In dgv.Rows
                query = "INSERT INTO purchase_order_details VALUES ('', @purchase_order_id, @item_id, @supplier_id, @qty, '')"

                _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

                With _MySqlCommand.Parameters
                    .AddWithValue("@purchase_order_id", purchaseOrderId)
                    .AddWithValue("@item_id", row.Cells("Id").Value)
                    .AddWithValue("@supplier_id", row.Cells("SupplierId").Value)
                    .AddWithValue("@qty", row.Cells("Qty Minta").Value)
                End With

                _MySqlCommand.ExecuteNonQuery()
            Next

            MessageBox.Show("Berhasil menambah data", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)

            clearForm()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try


    End Sub

    Private Sub btnKeluar_Click(sender As Object, e As EventArgs) Handles btnKeluar.Click
        clearForm()
        Me.Hide()
    End Sub

    Private Sub btnTambah_Click(sender As Object, e As EventArgs) Handles btnTambah.Click
        Try

            If cmbBarang.SelectedIndex = -1 Or cmbSupplier.SelectedIndex = -1 Or _
                Not IsNumeric(txtQty.Text) Or CInt(txtQty.Text) <= 0 Then

                MessageBox.Show("Tolong Isi Form Input Dengan Benar", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            Dim data(9) As String

            Dim selectedItem As ListObject = cmbBarang.SelectedItem

            Dim query As String = "SELECT items.id AS items_id, items.name AS items_name, items.category AS items_category, items.unit AS items_unit," & _
                                    "stocks.qty AS stocks_qty, stocks.location AS stocks_location " & _
                                    "FROM items " & _
                                    "JOIN stocks ON items.id = stocks.item_id " & _
                                    "WHERE items.id = '" & selectedItem.Value & "'"

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            _MySqlDataReader = _MySqlCommand.ExecuteReader

            If _MySqlDataReader.HasRows Then
                While _MySqlDataReader.Read
                    data(0) = _MySqlDataReader.Item("items_id").ToString
                    data(1) = _MySqlDataReader.Item("items_name").ToString
                    data(2) = _MySqlDataReader.Item("items_category").ToString
                    data(3) = _MySqlDataReader.Item("items_unit").ToString
                    data(4) = _MySqlDataReader.Item("stocks_qty").ToString
                    data(5) = _MySqlDataReader.Item("stocks_location").ToString
                End While
            End If

            _MySqlCommand.Dispose()
            _MySqlDataReader.Close()

            Dim supplierSelected As ListObject = cmbSupplier.SelectedItem

            data(6) = supplierSelected.Value.ToString
            data(7) = supplierSelected.Text.ToString
            data(8) = txtQty.Text

            dgv.Rows.Add(data)
            jumlahBarang()

            cmbBarang.SelectedIndex = -1
            cmbSupplier.SelectedIndex = -1
            txtQty.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        _MySqlCommand.Dispose()
        _MySqlDataReader.Close()
    End Sub

    Private Sub txtQty_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtQty.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = Convert.ToChar(Keys.Back))
    End Sub
End Class