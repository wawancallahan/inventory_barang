Public Class ListObject
    Private _Text As String
    Private _Value As String
    Private _Data As Object

    Public Sub New(ByVal text As String, ByVal value As String, Optional ByVal data As Object = Nothing)
        Me.Text = text
        Me.Value = value
        Me.Data = data
    End Sub

    Public Property Text() As String
        Get
            Return Me._Text
        End Get
        Set(ByVal value As String)
            Me._Text = value
        End Set
    End Property

    Public Property Value() As String
        Get
            Return Me._Value
        End Get
        Set(ByVal value As String)
            Me._Value = value
        End Set
    End Property

    Public Property Data() As Object
        Get
            Return Me._Data
        End Get
        Set(ByVal data As Object)
            Me._Data = data
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return Me._Text
    End Function
End Class
