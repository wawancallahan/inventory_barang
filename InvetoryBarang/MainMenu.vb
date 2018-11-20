Public Class MainMenu

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        BarangView.ShowDialog()
    End Sub

    Private Sub MainMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call Koneksi()
        txtNama.Text = "Login : "
        txtTanggal.Text = Date.Now.ToString("dd MMMM yyyy")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        SupplierView.ShowDialog()
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

    Private Sub UserToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UserToolStripMenuItem.Click

    End Sub

    Private Sub BarangToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BarangToolStripMenuItem.Click
        BarangView.ShowDialog()
    End Sub

    Private Sub SupplierToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SupplierToolStripMenuItem.Click
        SupplierView.ShowDialog()
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

    Private Sub CrystalReportViewer1_Load(sender As Object, e As EventArgs)

    End Sub
End Class