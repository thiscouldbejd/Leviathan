#If TESTING Then

Public Class TestFormattableObject_3

	#Region " Private Variables "

		Private m_Property1 As Integer

		Private m_PropertyArray As TestFormattableObject_2()

	#End Region

	#Region " Public Constructors "

		Public Sub New()
		End Sub

		Public Sub New(ByVal propertyArray As TestFormattableObject_2())
			Me.PropertyArray_2 = propertyArray
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

		Public Property PropertyArray_2() As TestFormattableObject_2()
			Get
				Return m_PropertyArray
			End Get
			Set(ByVal value As TestFormattableObject_2())
				m_PropertyArray = value
			End Set
		End Property

	#End Region

End Class

#End If