#If TESTING Then

Public Class TestManipulatedObject_4

	Private m_Property1 As Integer

	Private m_Property2 As TestManipulatedObject_1()

	Private m_Property3 As TestManipulatedObject_4()

	Public Property Property1() As Integer
		Get
			Return m_Property1
		End Get
		Set(ByVal value As Integer)
			m_Property1 = value
		End Set
	End Property

	Public Property Property2() As TestManipulatedObject_1()
		Get
			Return m_Property2
		End Get
		Set(ByVal value As TestManipulatedObject_1())
			m_Property2 = value
		End Set
	End Property

	Public Property Property3() As TestManipulatedObject_4()
		Get
			Return m_Property3
		End Get
		Set(ByVal value As TestManipulatedObject_4())
			m_Property3 = value
		End Set
	End Property

End Class

#End If