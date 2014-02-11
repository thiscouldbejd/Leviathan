#If TESTING Then

Public Class TestAnalysedObject_4
	Implements TestAnalysedInterface

	Public Function Method1() As String _
	Implements TestAnalysedInterface.Method1
		Return Nothing
	End Function

	<TestAttribute_1(Name:="Method2Attribute")> _
	Public Function Method2() As String
		Return Nothing
	End Function

End Class

#End If