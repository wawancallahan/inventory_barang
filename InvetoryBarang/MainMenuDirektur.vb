﻿Public Class MainMenuDirektur

    Private Sub MainMenuDirektur_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call Koneksi()
        txtNama.Text = "Login : " & LoginInformation.UserName
        txtTanggal.Text = Date.Now.ToString("dd MMMM yyyy")
        txtAkses.Text = "Akses : " & LoginInformation.UserRole
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        SupplierView.ShowDialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        BarangView.ShowDialog()
    End Sub

    Private Sub UserToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UserToolStripMenuItem.Click
        UserView.ShowDialog()
    End Sub

    Private Sub BarangToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BarangToolStripMenuItem.Click
        BarangView.ShowDialog()
    End Sub

    Private Sub SupplierToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SupplierToolStripMenuItem.Click
        SupplierView.ShowDialog()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub PermintaanToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles PermintaanToolStripMenuItem1.Click
        HistoryPermintaanView.ShowDialog()
    End Sub

    Private Sub PenerimaanToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles PenerimaanToolStripMenuItem1.Click
        HistoryPenerimaanView.ShowDialog()
    End Sub

    Private Sub PengirimanToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles PengirimanToolStripMenuItem1.Click
        HistoryPengirimanView.ShowDialog()
    End Sub

    Private Sub LoginToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoginToolStripMenuItem.Click
        Login.Show()
        Me.Hide()
    End Sub

    Private Sub BarangToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles BarangToolStripMenuItem1.Click
        BarangFormReport.ShowDialog()
    End Sub

    Private Sub PermintaanToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PermintaanToolStripMenuItem.Click
        PermintaanBarangReport.ShowDialog()

    End Sub

    Private Sub PengirimanToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PengirimanToolStripMenuItem.Click
        PengirimanBarangReport.ShowDialog()

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        UserView.ShowDialog()
    End Sub
End Class