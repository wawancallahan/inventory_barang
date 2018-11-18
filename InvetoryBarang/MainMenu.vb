Public Class MainMenu

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        BarangView.ShowDialog()
    End Sub

    Private Sub MainMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call Koneksi()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        SupplierView.ShowDialog()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        PermintaanView.ShowDialog()
    End Sub
End Class