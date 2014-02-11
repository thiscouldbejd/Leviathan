#If TESTING Then

	Public Class TestManipulatedObject_3

		Private m_PropertyArray As TestManipulatedObject_2()

		Public Property PropertyArray_2() As TestManipulatedObject_2()
			Get
				Return m_PropertyArray
			End Get
			Set(ByVal value As TestManipulatedObject_2())
				m_PropertyArray = value
			End Set
		End Property
	End Class

#End If