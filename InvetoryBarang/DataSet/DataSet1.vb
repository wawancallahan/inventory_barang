Partial Class DataSet1
    Partial Class DataTable1DataTable

        Private Sub DataTable1DataTable_DataTable1RowChanging(sender As Object, e As DataTable1RowChangeEvent) Handles Me.DataTable1RowChanging

        End Sub

        Private Sub DataTable1DataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.namaColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

End Class
