Module DatabaseSeeder
    Public Sub UserSeeder()
        Try
            Dim user_count As Integer = 0

            Dim query As String = "SELECT COUNT(*) AS user_count FROM users"

            _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)

            _MySqlDataReader = _MySqlCommand.ExecuteReader

            If _MySqlDataReader.HasRows Then

                If _MySqlDataReader.Read Then
                    user_count = _MySqlDataReader.Item("user_count")
                End If
            End If

            _MySqlCommand.Dispose()
            _MySqlDataReader.Close()

            If user_count <= 0 Then
                Dim users() As String = {
                    "Admin",
                    "Direktur"
                }

                For Each user As String In users
                    query = "INSERT INTO users VALUES ('', '" & user & "', PASSWORD('123'), '" & user & "')"

                    _MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand(query, _MySqlConnection)
                    _MySqlCommand.ExecuteNonQuery()
                Next
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        _MySqlDataReader.Close()
    End Sub
End Module
