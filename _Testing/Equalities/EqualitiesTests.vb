#If TESTING Then

Imports Leviathan.Comparison
Imports System.Configuration
Imports NUnit.Framework

Namespace Testing

    <TestFixture(Description:="Equalities Testing")> _
    Public Class EqualitiesTests

        <Test(Description:="Tests the IsNothing Method")> _
        Public Sub TestIsNothing()

            Dim a As Object = Nothing

            Assert.IsTrue(IsNothing(a))

            Assert.IsTrue(IsNothing(Nothing))

        End Sub

    End Class

End Namespace

#End If