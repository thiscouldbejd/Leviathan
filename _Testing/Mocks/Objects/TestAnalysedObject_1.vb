#If TESTING Then

<TestAttribute_1(Name:="TestAnalysedObjectAttribute")> _
    Public Class TestAnalysedObject_1

    Private m_Field1 As Object

    Private m_Field2 As String

    Public Field3 As Object

    <TestAttribute_1(Name:="Property1Attribute")> _
    Public Property Property1() As Object
        Get
            Return Nothing
        End Get
        Set(ByVal value As Object)
        End Set
    End Property

    <TestAttribute_2(Number:=2)> _
    Public ReadOnly Property Property2() As Object
        Get
            Return Nothing
        End Get
    End Property

    Public WriteOnly Property Property3() As Object
        Set(ByVal value As Object)
        End Set
    End Property

    <TestAttribute_1(Name:="Property4Attribute"), TestAttribute_2(Number:=4)> _
    Private Property Property4() As String
        Get
            Return Nothing
        End Get
        Set(ByVal value As String)
        End Set
    End Property

    Protected Property Property5() As Integer
        Get
            Return Nothing
        End Get
        Set(ByVal value As Integer)
        End Set
    End Property

    Public Sub Method1(ByVal parameter1 As Object, ByVal parameter2 As Object)
    End Sub

    Public Sub New()
    End Sub

    Public Sub New(ByVal parameter1 As Object)
    End Sub

    Public Sub New(ByVal parameter1 As Object, ByVal parameter2 As Object)
    End Sub

    Public Sub New(ByVal parameter3 As Integer, ByVal parameter2 As String)
    End Sub

End Class

#End If