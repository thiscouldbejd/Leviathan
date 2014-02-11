#If TESTING Then

Public Class TestManipulatedObject_1

	Private m_Property1 As Integer

	Private m_Property2 As String

	Private m_Property3 As TestManipulatedObject_1

	Private m_Field1 As Integer

	Private m_Field2 As String

	Public Sub New()
	End Sub

	Public Sub New(ByVal property1 As Integer)
		Me.Property1 = property1
	End Sub

	Public Sub New(ByVal property1 As Integer, ByVal property2 As String)
		Me.New(property1)
		Me.Property2 = property2
	End Sub

	Public Sub New(ByVal property2 As String, ByVal property1 As Integer)
		Me.New(property1)
		Me.Property2 = property2
	End Sub

	Public Property Property1() As Integer
		Get
			Return m_Property1
		End Get
		Set(ByVal value As Integer)
			m_Property1 = value
		End Set
	End Property

	Public Property Property2() As String
		Get
			Return m_Property2
		End Get
		Set(ByVal value As String)
			m_Property2 = value
		End Set
	End Property

	Public Property Property3() As TestManipulatedObject_1
		Get
			Return m_Property3
		End Get
		Set(ByVal value As TestManipulatedObject_1)
			m_Property3 = value
		End Set
	End Property

	Public Sub SetField1(ByVal field1 As Integer)
		m_Field1 = field1
	End Sub

	Public Function GetField1() As Integer
		Return m_Field1
	End Function

	Public Sub SetField2(ByVal field2 As String)
		m_Field2 = field2
	End Sub

	Public Function GetField2() As String
		Return m_Field2
	End Function

End Class

#End If