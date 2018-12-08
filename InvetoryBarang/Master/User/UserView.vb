Public Class UserView

    Dim selectedId As String = Nothing
    Dim updateDB As Boolean = False

    Private Sub UserView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        clearForm()
        txtPassword.UseSystemPasswordChar = True

        cmbAkses.Items.Add("Admin")
        cmbAkses.Items.Add("Manager")
        cmbAkses.DropDownStyle = ComboBoxStyle.DropDownList

        Me.aturDgv()
        Me.getItems()
    End Sub

    Protected Sub aturDgv()
        dgv.Columns.Clear()
        dgv.Columns.Add("Id", "Id")
        dgv.Columns("Id").Visible = False

        dgv.Columns.Add("User", "User")
        dgv.Columns.Add("Akses", "Akses")

        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.RowHeadersVisible = False
        dgv.ReadOnly = True
        dgv.ClearSelection()
    End Sub

    Public Sub clearForm()
        txtUsername.Clear()
        txtPassword.Clear()
        cmbAkses.SelectedIndex = -1
        cmbAkses.Enabled = True
        lblPasswordUpdate.Visible = False
        selectedId = Nothing
        updateDB = False
    End Sub

    Protected Function ValidasiForm(Optional ByVal update As Boolean = False)
        Dim validate As Boolean = False

        Try
            If update = True Then
                If txtUsername.Text = Nothing Or txtUsername.Text.Length < 5 Or cmbAkses.SelectedIndex = -1 Then
                    If txtPassword.Text <> Nothing Then
                        If txtPassword.Text = Nothing Or txtPassword.Text.Length < 5 Then
                            validate = False
                        Else
                            validate = True
                        End If
                    Else
                        validate = False
                    End If
                Else
                    validate = True
                End If
            Else
                If txtUsername.Text = Nothing Or txtPassword.Text = Nothing Or cmbAkses.SelectedIndex = -1 Or _
              txtUsername.Text.Length < 5 Or txtPassword.Text.Length < 5 Then
                    validate = False
                Else
                    validate = True
                End If
            End If
        Catch ex As Exception

        End Try

        Return validate
    End Function

    Protected Sub getItems(Optional key As String = Nothing)
        Try
            dgv.Rows.Clear()

            Dim query As String
            
            If key <> Nothing Then

                query = "SELECT * FROM users " & _
                        "WHERE username LIKE '%" & key & "%' OR role LIKE '%" & key & "%'"

                _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            Else
                query = "SELECT * FROM users"

                _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)
            End If

            _MySqlDataReader = _MySqlCommand.ExecuteReader

            If _MySqlDataReader.HasRows Then
                While _MySqlDataReader.Read

                    Dim data As String() = {
                        _MySqlDataReader.Item("id").ToString, _
                        _MySqlDataReader.Item("username").ToString, _
                        _MySqlDataReader.Item("role").ToString
                    }

                    dgv.Rows.Add(data)
                End While
            End If

            _MySqlCommand.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        _MySqlDataReader.Close()
    End Sub

    Private Function checkUserName(data As Object)
        Dim found As Boolean = False
        Try
            dgv.Rows.Clear()

            Dim query As String = "SELECT * FROM users WHERE username = '" & data(0) & "'"

            If updateDB = True And selectedId = LoginInformation.UserId Then
                query &= "AND NOT username = '" & LoginInformation.UserName & "'"
            ElseIf updateDB = True And selectedId <> Nothing Then
                query &= "AND NOT id = '" & selectedId & "'"
            End If

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            _MySqlDataReader = _MySqlCommand.ExecuteReader

            If _MySqlDataReader.HasRows Then
                found = True
            End If

            _MySqlCommand.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        _MySqlDataReader.Close()
        Return found
    End Function

    Private Sub btnBatal_Click(sender As Object, e As EventArgs) Handles btnBatal.Click
        clearForm()
    End Sub

    Private Sub btnSimpan_Click(sender As Object, e As EventArgs) Handles btnSimpan.Click
        Dim data As Object = {
            txtUsername.Text, _
            txtPassword.Text, _
            cmbAkses.SelectedItem
        }

        Select Case updateDB
            Case True

                If Not ValidasiForm(True) Then
                    MessageBox.Show("Tolong Isi Form Input", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Me.getItems()
                    Exit Sub
                End If

                If checkUserName(data) Then
                    MessageBox.Show("Username telah ada", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Me.getItems()
                    Exit Sub
                End If

                If updateData(data, selectedId) Then
                    MessageBox.Show("Berhasil mengedit data", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("Gagal mengedit data", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Case False

                If Not ValidasiForm() Then
                    MessageBox.Show("Tolong Isi Form Input", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Me.getItems()
                    Exit Sub
                End If

                If checkUserName(data) Then
                    MessageBox.Show("Username telah ada", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Me.getItems()
                    Exit Sub
                End If

                If insertData(data) Then
                    MessageBox.Show("Berhasil menambah data", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("Gagal menambah data", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
        End Select


        Me.getItems()
        clearForm()
    End Sub

    Private Sub btnKeluar_Click(sender As Object, e As EventArgs) Handles btnKeluar.Click
        clearForm()
        Me.Hide()
    End Sub

    Private Sub dgv_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv.CellClick
        If e.RowIndex < 0 Then
            Exit Sub
        End If

        With dgv.Rows(e.RowIndex)
            selectedId = .Cells("Id").Value
            txtUsername.Text = .Cells("User").Value
            cmbAkses.SelectedItem = .Cells("Akses").Value
        End With

        If selectedId = LoginInformation.UserId Then
            cmbAkses.Enabled = False
        Else
            cmbAkses.Enabled = True
        End If

        updateDB = True
        lblPasswordUpdate.Visible = True
    End Sub

    Public Function insertData(data As Object)
        Try
            Dim query As String = "INSERT INTO users VALUES ('', '" & data(0) & "', PASSWORD('" & data(1) & "'), '" & data(2) & "')"

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            _MySqlCommand.ExecuteNonQuery()

            _MySqlCommand.Dispose()

            Return True
        Catch ex As Exception
            MessageBox.Show(ex.Message)

            _MySqlCommand.Dispose()

            Return False
        End Try
    End Function

    Public Function updateData(data As Object, id As Integer)
        Try
            Dim query As String

            If data(1) = Nothing Then
                query = "UPDATE users SET username='" & data(0) & "', role='" & data(2) & "'" & _
                                  "WHERE id='" & id & "'"
            Else
                query = "UPDATE users SET username='" & data(0) & "', password=PASSWORD('" & data(1) & "'), role='" & data(2) & "'" & _
                                  "WHERE id='" & id & "'"
            End If

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            _MySqlCommand.ExecuteNonQuery()

            Return True
        Catch ex As Exception
            MessageBox.Show(ex.Message)

            Return False
        End Try
    End Function

    Public Function deleteData(id As Integer)
        Try
            Dim query As String = "DELETE FROM users WHERE id='" & id & "'"

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)
            With _MySqlCommand.Parameters
                .AddWithValue("@id", id)
            End With

            _MySqlCommand.ExecuteNonQuery()

            Return True
        Catch ex As Exception
            MessageBox.Show(ex.Message)

            Return False
        End Try
    End Function

    Private Sub btnHapus_Click(sender As Object, e As EventArgs) Handles btnHapus.Click

        If selectedId = Nothing Then
            MessageBox.Show("Tidak ada data yang dipilih", "Perintagan", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            If selectedId = LoginInformation.UserId Then
                MessageBox.Show("Tidak bisa menghapus user sendiri", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Information)

                clearForm()
                updateDB = False

                Exit Sub
            End If

            If deleteData(selectedId) Then
                MessageBox.Show("Berhasil menghapus data", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Gagal menghapus data", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If

        Me.getItems()
        clearForm()
        updateDB = False
        selectedId = Nothing
    End Sub

    Private Sub txtCari_TextChanged(sender As Object, e As EventArgs) Handles txtCari.TextChanged
        If txtCari.Text <> Nothing Then
            getItems(txtCari.Text)
        Else
            getItems()
        End If
    End Sub
End Class