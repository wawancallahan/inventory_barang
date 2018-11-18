﻿Public Class ListObject
    Private _Text As String
    Private _Value As String

    Public Sub New(ByVal text As String, ByVal value As Object)
        Me.Text = text
        Me.Value = value
    End Sub

    Public Property Text As String
        Get
            Return Me._Text
        End Get
        Set(ByVal value As String)
            Me._Text = value
        End Set
    End Property

    Public Property Value As String
        Get
            Return Me._Value
        End Get
        Set(ByVal value As String)
            Me._Value = value
        End Set
    End Property
End Class
