#If TESTING Then

Public Class TestFormattableObject_1

	#Region " Private Variables "

		Private m_Property1 As Integer

		Private m_Property2 As String

		Private m_Property3 As TestFormattableObject_1

		Private m_Field1 As Integer

		Private m_Field2 As String

	#End Region

	#Region " Public Constructors "

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

	#End Region

	#Region " Public Properties "

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

		Public Property Property3() As TestFormattableObject_1
			Get
				Return m_Property3
			End Get
			Set(ByVal value As TestFormattableObject_1)
				m_Property3 = value
			End Set
		End Property

	#End Region

End Class

#End If