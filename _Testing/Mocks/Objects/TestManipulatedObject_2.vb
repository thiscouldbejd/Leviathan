#If TESTING Then

Public Class TestManipulatedObject_2

	Private m_PropertyArray As TestManipulatedObject_1()

	Public Property PropertyArray() As TestManipulatedObject_1()
		Get
			Return m_PropertyArray
		End Get
		Set(ByVal value As TestManipulatedObject_1())
			m_PropertyArray = value
		End Set
	End Property
End Class

#End If