Public Class MainMenu

    Private Sub MainMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call Koneksi()
        txtNama.Text = "Login : " & LoginInformation.UserName
        txtTanggal.Text = Date.Now.ToString("dd MMMM yyyy")
        txtAkses.Text = "Akses : " & LoginInformation.UserRole
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        PermintaanView.ShowDialog()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        PenerimaanView.ShowDialog()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        PengirimanView.ShowDialog()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub PermintaanToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PermintaanToolStripMenuItem.Click
        PermintaanView.ShowDialog()
    End Sub

    Private Sub PenerimaanToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PenerimaanToolStripMenuItem.Click
        PenerimaanView.ShowDialog()
    End Sub

    Private Sub PengirimanToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PengirimanToolStripMenuItem.Click
        PengirimanView.ShowDialog()
    End Sub

    Private Sub LoginToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoginToolStripMenuItem.Click
        Login.Show()
        Me.Hide()
    End Sub
End Class