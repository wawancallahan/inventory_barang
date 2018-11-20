Public Class PenerimaanView

    Private Sub PenerimaanView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        clearForm()
        aturDgv()
        getItemsPermintaan()
    End Sub

    Public Sub aturDgv()
        dgv.Columns.Add("Id", "Id")
        dgv.Columns("Id").Visible = False
        dgv.Columns.Add("ItemId", "ItemId")
        dgv.Columns("ItemId").Visible = False
        dgv.Columns.Add("Nama", "Nama")
        dgv.Columns("Nama").ReadOnly = True
        dgv.Columns.Add("Kategori", "Kategori")
        dgv.Columns("Kategori").ReadOnly = True
        dgv.Columns.Add("Unit", "Unit")
        dgv.Columns("Unit").ReadOnly = True
        dgv.Columns.Add("Supplier", "Supplier")
        dgv.Columns("Supplier").ReadOnly = True
        dgv.Columns.Add("Qty Minta", "Qty Minta")
        dgv.Columns("Qty Minta").ReadOnly = True
        dgv.Columns.Add("Qty Diterima", "Qty Diterima")
        dgv.Columns("Qty Diterima").ReadOnly = True
        dgv.Columns.Add("Qty Datang", "Qty Datang")

        dgv.RowHeadersVisible = False
        dgv.EditMode = DataGridViewEditMode.EditOnEnter
        dgv.ClearSelection()
    End Sub

    Public Sub clearForm()
        txtNo.Clear()
        dtpTanggal.Value = Date.Now
        dtpTanggalPermintaan.Value = Date.Now
        txtJumlahPermintaan.Clear()
        cbSesuai.Checked = False
        cmbPermintaan.SelectedIndex = -1
        dgv.Rows.Clear()
        dgv.ClearSelection()
        If dgv.RowCount > 0 Then
            dgv.CurrentRow.Selected = False
        End If
    End Sub

    Public Sub getItemsPermintaan()
        Try
            cmbPermintaan.Items.Clear()
            Dim query As String = "SELECT * FROM purchase_orders WHERE status = 0"

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            _MySqlDataReader = _MySqlCommand.ExecuteReader

            If _MySqlDataReader.HasRows Then

                While _MySqlDataReader.Read
                    cmbPermintaan.Items.Add(New ListObject(_MySqlDataReader.Item("no").ToString(), _MySqlDataReader.Item("id").ToString()))
                End While

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        _MySqlDataReader.Close()
    End Sub

    Private Sub cmbPermintaan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPermintaan.SelectedIndexChanged
        If Not cmbPermintaan.SelectedIndex < 0 Then
            Try
                dgv.Rows.Clear()

                Dim itemSelected As ListObject = cmbPermintaan.SelectedItem
                Dim query As String = "SELECT purchase_order_details.id AS purchase_order_details_id, purchase_order_details.qty AS purchase_order_details_qty, purchase_order_details.qty_complete AS purchase_order_details_qty_complete, " & _
                                      "items.id AS items_id, items.name AS items_name, items.category AS items_category, items.unit AS items_unit, " & _
                                      "suppliers.name AS suppliers_name " & _
                                      "FROM purchase_order_details " & _
                                      "JOIN items ON purchase_order_details.item_id = items.id " & _
                                      "JOIN suppliers ON purchase_order_details.supplier_id = suppliers.id " & _
                                      "WHERE purchase_order_details.purchase_order_id = '" & itemSelected.Value & "'"

                _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

                _MySqlDataReader = _MySqlCommand.ExecuteReader

                If _MySqlDataReader.HasRows Then

                    Dim data(9) As String

                    While _MySqlDataReader.Read
                        data(0) = _MySqlDataReader.Item("purchase_order_details_id")
                        data(1) = _MySqlDataReader.Item("items_id")
                        data(2) = _MySqlDataReader.Item("items_name")
                        data(3) = _MySqlDataReader.Item("items_category")
                        data(4) = _MySqlDataReader.Item("items_unit")
                        data(5) = _MySqlDataReader.Item("suppliers_name")
                        data(6) = _MySqlDataReader.Item("purchase_order_details_qty")
                        data(7) = _MySqlDataReader.Item("purchase_order_details_qty_complete")
                        data(8) = ""

                        dgv.Rows.Add(data)
                    End While

                    txtJumlahPermintaan.Text = dgv.RowCount

                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try

            _MySqlDataReader.Close()
        End If
    End Sub

    Private Sub btnSimpan_Click(sender As Object, e As EventArgs) Handles btnSimpan.Click
        Try
            Dim itemSelected As ListObject = cmbPermintaan.SelectedItem

            Dim query As String

            If cbSesuai.Checked Then
                query = "UPDATE purchase_orders SET status = 1 WHERE id = '" & itemSelected.Value & "'"

                _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

                _MySqlCommand.ExecuteNonQuery()
            End If


            query = "INSERT INTO purchase_receipts VALUES ('', @no, @date, @purchase_order_id); SELECT LAST_INSERT_ID()"

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            With _MySqlCommand.Parameters
                .AddWithValue("@no", txtNo.Text)
                .AddWithValue("@date", dtpTanggal.Value)
                .AddWithValue("@purchase_order_id", itemSelected.Value)
            End With

            Dim purchaseReceiptId As Integer = CInt(_MySqlCommand.ExecuteScalar())

            _MySqlCommand.Dispose()

            For Each row As DataGridViewRow In dgv.Rows
                query = "INSERT INTO purchase_receipt_details VALUES ('', @purchase_receipt_id, @purchase_order_detail_id, @qty)"

                _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

                With _MySqlCommand.Parameters
                    .AddWithValue("@purchase_receipt_id", purchaseReceiptId)
                    .AddWithValue("@purchase_order_detail_id", row.Cells("Id").Value)
                    .AddWithValue("@qty", row.Cells("Qty Datang").Value)
                End With

                _MySqlCommand.ExecuteNonQuery()

                query = "UPDATE purchase_order_details SET qty_complete = '" & row.Cells("Qty Diterima").Value + row.Cells("Qty Datang").Value & "' " & _
                        "WHERE id = '" & row.Cells("Id").Value & "'"

                _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

                _MySqlCommand.ExecuteNonQuery()

                Dim qty As Integer = 0

                query = "SELECT * FROM stocks WHERE item_id = '" & row.Cells("ItemId").Value & "'"

                _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

                _MySqlDataReader = _MySqlCommand.ExecuteReader
                If _MySqlDataReader.HasRows Then
                    While _MySqlDataReader.Read
                        qty += CInt(_MySqlDataReader.Item("qty").ToString)
                    End While
                End If

                _MySqlDataReader.Close()

                query = "UPDATE stocks SET qty = '" & qty + row.Cells("Qty Datang").Value & "' " & _
                        "WHERE item_id = '" & row.Cells("ItemId").Value & "'"

                _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

                _MySqlCommand.ExecuteNonQuery()
            Next

            MessageBox.Show("Berhasil menambah data", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)

            clearForm()
            getItemsPermintaan()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        _MySqlDataReader.Close()
    End Sub
End Class