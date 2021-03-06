﻿Public Class Login

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Application.Exit()
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        If Me.checkAuth(txtUsername.Text, txtPassword.Text) Then
            If Me.auth(txtUsername.Text, txtPassword.Text) Then

                Select Case LoginInformation.UserRole
                    Case "Admin"
                        MainMenu.Show()
                    Case "Manager"
                        MainMenuDirektur.Show()
                    Case Else
                        MessageBox.Show("Username atau password tidak sesuai", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        Exit Sub
                End Select
                clearForm()
                Me.Hide()
            Else
                MessageBox.Show("Username atau password tidak sesuai", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            MessageBox.Show("Tolong cek kembali inputan anda", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Protected Function checkAuth(Optional username As String = Nothing, Optional password As String = Nothing)
        If username = Nothing Or password = Nothing Then
            Return False
        End If

        Return True
    End Function

    Protected Function auth(Optional username As String = Nothing, Optional password As String = Nothing)
        Dim toLogin As Boolean = False
        Try
            Dim query As String = "SELECT * FROM users WHERE BINARY username = '" & username & "' AND BINARY password = PASSWORD('" & password & "')"

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            _MySqlDataReader = _MySqlCommand.ExecuteReader

            If _MySqlDataReader.HasRows Then

                While _MySqlDataReader.Read
                    LoginInformation.UserId = _MySqlDataReader.Item("id")
                    LoginInformation.UserName = _MySqlDataReader.Item("username")
                    LoginInformation.UserRole = _MySqlDataReader.Item("role")
                End While

                toLogin = True
            End If

            _MySqlCommand.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        _MySqlDataReader.Close()

        Return toLogin
    End Function

    Protected Sub clearForm()
        txtUsername.Clear()
        txtPassword.Clear()
    End Sub

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        clearForm()
        txtPassword.UseSystemPasswordChar = True

        Call Koneksi()
        Call UserSeeder()
    End Sub

    Private Sub txtPassword_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPassword.KeyPress
        If e.KeyChar = Chr(13) Then
            btnLogin.PerformClick()
        End If
    End Sub

    Private Sub btnLogin_MouseHover(sender As Object, e As EventArgs) Handles btnLogin.MouseHover
        Cursor = Cursors.Hand
    End Sub

    Private Sub btnLogin_MouseLeave(sender As Object, e As EventArgs) Handles btnLogin.MouseLeave
        Cursor = Cursors.Default
    End Sub

    Private Sub btnExit_MouseHover(sender As Object, e As EventArgs) Handles btnExit.MouseHover
        Cursor = Cursors.Hand
    End Sub

    Private Sub btnExit_MouseLeave(sender As Object, e As EventArgs) Handles btnExit.MouseLeave
        Cursor = Cursors.Default
    End Sub
End Class
