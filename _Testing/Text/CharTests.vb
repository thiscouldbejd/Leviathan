#If TESTING Then

Imports Leviathan.Commands.StringCommands
Imports System.Configuration
Imports NUnit.Framework

Namespace Testing

    <TestFixture(Description:="Char Testing")> _
    Public Class CharTests

        <Test(Description:="Tests the IndexOfAny Method")> _
        Public Sub TestIndexOfAny()

            Assert.AreEqual(-1, IndexOfAny(String.Empty, New Char() {Char.Parse("T")}))

            Assert.AreEqual(0, IndexOfAny("Test_1", New Char() {Char.Parse("T")}))

            Assert.AreEqual(1, IndexOfAny("Test_1", New Char() {Char.Parse("e")}))

            Assert.AreEqual(1, IndexOfAny("Test_1", New Char() {Char.Parse("e"), Char.Parse("s")}))

            Assert.AreEqual(2, IndexOfAny("Test_1", New Char() {Char.Parse("u"), Char.Parse("s")}))

        End Sub

        <Test(Description:="Tests the IsAny Method")> _
        Public Sub TestIsAny()

            Assert.IsFalse(IsAny(String.Empty, New Char() {}))

            Assert.IsTrue(IsAny("t", New Char() {Char.Parse("t")}))

            Assert.IsTrue(IsAny("t", New Char() {Char.Parse("e"), Char.Parse("t")}))

            Assert.IsFalse(IsAny("t", New Char() {Char.Parse("e"), Char.Parse("s")}))

        End Sub

    End Class

End Namespace

#End If