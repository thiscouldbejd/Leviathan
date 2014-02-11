#If TESTING Then

Public Class TestFormattableObject_2

	#Region " Private Variables "

		Private m_Property1 As Integer

		Private m_PropertyArray As TestFormattableObject_1()

	#End Region

	#Region " Public Constructors "

		Public Sub New()
		End Sub

		Public Sub New(ByVal propertyArray As TestFormattableObject_1())
			Me.PropertyArray = propertyArray
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

		Public Property PropertyArray() As TestFormattableObject_1()
			Get
				Return m_PropertyArray
			End Get
			Set(ByVal value As TestFormattableObject_1())
				m_PropertyArray = value
			End Set
		End Property

	#End Region

End Class

#End If