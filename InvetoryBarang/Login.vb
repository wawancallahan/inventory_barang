Public Class Login

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Application.Exit()
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        If Me.checkAuth(txtUsername.Text, txtPassword.Text) Then
            If Me.auth(txtUsername.Text, txtPassword.Text) Then
                MainMenu.Show()
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
        Return True
    End Function

    Protected Sub clearForm()
        txtUsername.Clear()
        txtPassword.Clear()
    End Sub

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        clearForm()
        txtPassword.UseSystemPasswordChar = True
    End Sub

    Private Sub txtPassword_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPassword.KeyPress
        If e.KeyChar = Chr(13) Then
            btnLogin.PerformClick()
        End If
    End Sub
End Class
