#If TESTING Then

Public Class TestAnalysedObjectMethod

	<TestAttribute_1(Name:="Test1")> _
	Public Sub Method1( _
		ByVal parameter1 As Object _
	)
	End Sub

	<TestAttribute_2(Number:=1)> _
	Public Sub Method2( _
		ByVal parameter2 As Object _
	)
	End Sub

	<TestAttribute_1(Name:="Test2"), TestAttribute_2(Number:=2)> _
	Public Sub Method3( _
		ByVal parameter1 As Object, _
		ByVal parameter2 As Object _
	)
	End Sub

End Class

#End If