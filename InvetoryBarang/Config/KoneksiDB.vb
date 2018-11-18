Imports MySql.Data.MySqlClient
Module KoneksiDB

    Public _MySqlConnection As MySqlConnection
    Public _MySqlCommand As MySqlCommand
    Public _MySqlDataReader As MySqlDataReader
    Public _MySqlDataAdapter As MySqlDataAdapter
    Public _DataSet As DataSet
    Public _DataTable As DataTable
    Public _MySqlTransaction As MySqlTransaction

    Public Sub Koneksi()
        Try
            _MySqlConnection = New MySqlConnection("server=localhost;database=praktikum_pv;uid=root;")
            If _MySqlConnection.State = ConnectionState.Closed Then
                _MySqlConnection.Open()
            End If
        Catch ex As Exception
            MessageBox.Show("Koneksi ke database bermasalah", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Module
