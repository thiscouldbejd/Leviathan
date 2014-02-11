#If TESTING Then

Public Class TestAnalysedObjectList_2
	Inherits TestAnalysedObjectListBase

	Default Public Overloads Property Item(ByVal index As Integer) As TestAnalysedObject_1
		Get
			Return Nothing
		End Get
		Set(ByVal value As TestAnalysedObject_1)
		End Set
	End Property

End Class

#End If