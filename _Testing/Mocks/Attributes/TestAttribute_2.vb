#If TESTING Then

<AttributeUsage(AttributeTargets.All)> _
Public Class TestAttribute_2
    Inherits Attribute

    Private m_Number As Integer

    Public Property Number() As Integer
        Get
            Return m_Number
        End Get
        Set(ByVal value As Integer)
            m_Number = value
        End Set
    End Property
End Class

#End If