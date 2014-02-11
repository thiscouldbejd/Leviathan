#If TESTING Then

<AttributeUsage(AttributeTargets.All, Inherited:=True), Serializable()> _
    Public Class TestAttribute_1
    Inherits Attribute

    Private m_Name As String

    Public Property Name() As String
        Get
            Return m_Name
        End Get
        Set(ByVal value As String)
            m_Name = value
        End Set
    End Property
End Class

#End If