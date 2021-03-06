﻿Public Class PengirimanView

    Private Sub PengirimanView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        aturDgv()
        clearForm()
        getItemsBarang()
        txtNo.ReadOnly = True
        txtNo.Text = autoNomor()
        txtMax.ReadOnly = True
        cmbBarang.DropDownStyle = ComboBoxStyle.DropDownList
    End Sub

    Protected Function autoNomor()
        Dim no As String = 0

        Dim query As String = "SELECT COUNT(*) AS items_count FROM purchase_receipts"

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
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.EditMode = DataGridViewEditMode.EditOnEnter
        dgv.Columns.Add("Id", "Id")
        dgv.Columns("Id").Visible = False
        dgv.Columns.Add("Barang", "Barang")
        dgv.Columns.Add("Old Qty", "Old Qty")
        dgv.Columns("Old Qty").Visible = False
        dgv.Columns.Add("Jumlah", "Jumlah")
        dgv.Columns.Add("item", "item")
        dgv.Columns("item").Visible = False
    End Sub

    Public Sub clearForm()
        clearDataDgv()
        txtTujuan.Clear()
        txtNo.Clear()
        dtpTanggal.Value = Date.Now
        dgv.Rows.Clear()
        dgv.ClearSelection()
        If dgv.RowCount > 0 Then
            dgv.CurrentRow.Selected = False
        End If
        txtNo.Text = autoNomor()
    End Sub

    Public Sub getItemsBarang()
        Try
            cmbBarang.Items.Clear()
            Dim query As String = "SELECT items.id AS items_id, items.name AS items_name, items.category AS items_category, items.unit AS items_unit," & _
                                  "stocks.qty AS stocks_qty, stocks.location AS stocks_location " & _
                                  "FROM items " & _
                                  "JOIN stocks ON items.id = stocks.item_id "

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            _MySqlDataReader = _MySqlCommand.ExecuteReader

            If _MySqlDataReader.HasRows Then
                While _MySqlDataReader.Read
                    cmbBarang.Items.Add(New ListObject(_MySqlDataReader.Item("items_name").ToString, _MySqlDataReader.Item("items_id").ToString, {_MySqlDataReader.Item("stocks_qty")}))
                End While
            End If

            _MySqlCommand.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        _MySqlDataReader.Close()
    End Sub

    Public Sub clearDataDgv()
        cmbBarang.SelectedIndex = -1
        txtQty.Clear()
        txtMax.Clear()
    End Sub

    Private Sub btnTambah_Click(sender As Object, e As EventArgs) Handles btnTambah.Click

        If cmbBarang.SelectedIndex = -1 Or txtQty.Text = Nothing Or Not IsNumeric(txtQty.Text) Then
            MessageBox.Show("Tolong Isi Form Input Dengan Benar", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        ElseIf CInt(txtQty.Text) <= 0 Then
            MessageBox.Show("Qty tidak boleh kurang atau sama dengan nol (0)", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If CInt(txtQty.Text) > CInt(txtMax.Text) Then
            MessageBox.Show("Qty tidak boleh melebihi Qty Max", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim itemSelected As ListObject = DirectCast(cmbBarang.SelectedItem, ListObject)

        Dim dataItem As Object = itemSelected.Data
        dataItem(0) = CInt(dataItem(0)) - CInt(txtQty.Text)

        itemSelected.Data = dataItem

        dgv.Rows.Add({
                     itemSelected.Value,
                     itemSelected.Text,
                     txtMax.Text,
                     txtQty.Text,
                     itemSelected
                     })
        clearDataDgv()
    End Sub

    Private Sub btnKeluar_Click(sender As Object, e As EventArgs) Handles btnKeluar.Click
        clearForm()
        Me.Hide()
    End Sub

    Private Sub btnBatal_Click(sender As Object, e As EventArgs) Handles btnBatal.Click
        clearForm()
        getItemsBarang()
    End Sub

    Protected Function validateForm()
        Dim validate As Boolean = False
        Try
            If txtNo.Text = Nothing Or txtTujuan.Text = Nothing Or _
                dtpTanggal.Value = Nothing Then
                validate = False
            Else
                validate = True
            End If
        Catch ex As Exception

        End Try

        Return validate
    End Function

    Private Sub btnSimpan_Click(sender As Object, e As EventArgs) Handles btnSimpan.Click
        Try
            If Not validateForm() Then
                MessageBox.Show("Tolong Isi Form Input Dengan Benar", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If


            Dim query As String = "INSERT INTO deliveries VALUES ('', @no, @date, @destination); SELECT LAST_INSERT_ID()"

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            With _MySqlCommand.Parameters
                .AddWithValue("@no", txtNo.Text)
                .AddWithValue("@date", dtpTanggal.Value)
                .AddWithValue("@destination", txtTujuan.Text)
            End With

            Dim deliveryId As Integer = CInt(_MySqlCommand.ExecuteScalar())

            _MySqlCommand.Dispose()

            For Each row As DataGridViewRow In dgv.Rows
                query = "INSERT INTO delivery_details VALUES ('', @delivery_id, @item_id, @qty)"

                _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

                With _MySqlCommand.Parameters
                    .AddWithValue("@delivery_id", deliveryId)
                    .AddWithValue("@item_id", row.Cells("Id").Value)
                    .AddWithValue("@qty", row.Cells("Jumlah").Value)
                End With

                _MySqlCommand.ExecuteNonQuery()

                Dim qty As Integer = 0

                query = "SELECT * FROM stocks WHERE item_id = '" & row.Cells("Id").Value & "'"

                _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

                _MySqlDataReader = _MySqlCommand.ExecuteReader
                If _MySqlDataReader.HasRows Then
                    While _MySqlDataReader.Read
                        qty += CInt(_MySqlDataReader.Item("qty").ToString)
                    End While
                End If

                _MySqlDataReader.Close()

                query = "UPDATE stocks SET qty = '" & qty - row.Cells("Jumlah").Value & "' " & _
                        "WHERE item_id = '" & row.Cells("Id").Value & "'"

                _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

                _MySqlCommand.ExecuteNonQuery()
            Next

            MessageBox.Show("Berhasil menambah data", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)

            clearForm()
            getItemsBarang()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmbBarang_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbBarang.SelectedIndexChanged
        If cmbBarang.SelectedIndex > -1 Then
            Dim itemSelected As ListObject = DirectCast(cmbBarang.SelectedItem, ListObject)
            txtMax.Text = itemSelected.Data(0)
        Else
            txtMax.Clear()
        End If
    End Sub

    Private Sub txtQty_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtQty.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = Convert.ToChar(Keys.Back))
    End Sub

    Private Sub dgv_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dgv.KeyPress
        On Error Resume Next
        If e.KeyChar = Chr(27) Then
            Dim itemSelected As ListObject = DirectCast(dgv.CurrentRow.Cells(4).Value, ListObject)
            Dim dataItem As Object = itemSelected.Data
            dataItem(0) = CInt(dataItem(0)) + CInt((dgv.CurrentRow.Cells(3).Value))

            itemSelected.Data = dataItem

            dgv.Rows.RemoveAt(dgv.CurrentRow.Index)
            cmbBarang.SelectedIndex = -1
            txtMax.Clear()
            txtQty.Clear()
        End If
    End Sub
End Class