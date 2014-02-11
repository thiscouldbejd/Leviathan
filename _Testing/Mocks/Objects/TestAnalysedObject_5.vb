#If TESTING Then

Public Class TestAnalysedObject_5

	Public Sub New(ByVal value As Integer)
	End Sub

	Public Property Property1() As Integer
		Get
			Return 0
		End Get
		Set(ByVal value As Integer)
		End Set
	End Property

	Public Property Property1( _
		ByVal index As Integer _
	) As String
		Get
			Return Nothing
		End Get
		Set(ByVal value As String)
		End Set
	End Property

	Public Property Property1( _
		ByVal name As String _
	) As Boolean
		Get
			Return False
		End Get
		Set(ByVal value As Boolean)
		End Set
	End Property

	Public Property Property2( _
		ByVal index As Integer _
	) As String
		Get
			Return Nothing
		End Get
		Set(ByVal value As String)
		End Set
	End Property

	Public Property Property2( _
		ByVal include As Boolean _
	) As Boolean
		Get
			Return False
		End Get
		Set(ByVal value As Boolean)
		End Set
	End Property

End Class

#End If