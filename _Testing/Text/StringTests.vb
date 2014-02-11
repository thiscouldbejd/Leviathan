#If TESTING Then

Imports Leviathan.Commands.StringCommands
Imports Leviathan.Comparison
Imports Leviathan.Comparison.Comparer
Imports System.Configuration
Imports NUnit.Framework

Namespace Testing

    <TestFixture(Description:="String Testing")> _
    Public Class StringsTests

        <Test(Description:="Tests the IndexOfAny Method")> _
        Public Sub TestIndexOfAny()

            Assert.AreEqual(-1, IndexOfAny(String.Empty, New String() {"Test"}))

            Assert.AreEqual(0, IndexOfAny("Test_1", New String() {String.Empty}))

            Assert.AreEqual(0, IndexOfAny("Test_1", New String() {"Test"}))

            Assert.AreEqual(0, IndexOfAny("Test_1 Test_2", New String() {"Test"}))

            Assert.AreEqual(5, IndexOfAny("Test_1 Test_2", New String() {"1"}))

            Assert.AreEqual(12, IndexOfAny("Test_1 Test_2", New String() {"2"}))

            Assert.AreEqual(5, IndexOfAny("Test_1 Test_2", New String() {"1", "2"}))

        End Sub

        <Test(Description:="Tests the IsAny Method")> _
        Public Sub TestIsAny()

            Assert.IsFalse(IsAny(String.Empty, New String() {"Test_1"}))

            Assert.IsTrue(IsAny("Test_1", New String() {"Test_1"}))

            Assert.IsFalse(IsAny("Test_1", New String() {"Test_2"}))

            Assert.IsFalse(IsAny("Test_1", New String() {"Test"}))

            Assert.IsFalse(IsAny("Test_1", New String() {"2", "Test"}))

            Assert.IsFalse(IsAny("Test_2", New String() {"2", "Test"}))

            Assert.IsTrue(IsAny("Test_2", New String() {"Test_1", "Test_2"}))

        End Sub

        <Test(Description:="Tests the IsNumeric Method")> _
        Public Sub TestIsNumeric()

            Assert.IsFalse(IsNumeric(Nothing))

            Assert.IsFalse(IsNumeric(String.Empty))

            Assert.IsFalse(IsNumeric("a"))

            Assert.IsFalse(IsNumeric("BB"))

            Assert.IsTrue(IsNumeric("1"))

            Assert.IsTrue(IsNumeric("1456546"))

            Assert.IsTrue(IsNumeric("145.6546"))

        End Sub

        <Test(Description:="Tests the MatchFuzzyString Method")> _
        Public Sub FuzzyMatching_1()

            Dim string_1 As String = "test"
            Dim string_1_Output As String = "test"
            Assert.IsTrue(string_1_Output = MatchFuzzyString(string_1, _
                New String() {"tEsT", "Nothing", "Ouch"}))

            Dim string_2 As String = "test1"
            Dim string_2_Output As String = "test1"
            Assert.IsFalse(string_2_Output = MatchFuzzyString(string_2, _
                New String() {"tEsT", "Nothing", "Ouch"}))

            Dim string_3 As String = "te"
            Dim string_3_Output As String = "test"
            Assert.IsTrue(string_3_Output = MatchFuzzyString(string_3, _
                New String() {"tEsT", "Nothing", "Ouch"}))

        End Sub

        <Test(Description:="Tests the MatchFuzzyString Method")> _
        Public Sub FuzzyMatching_2()

            Dim string_1 As String = "test-command"
            Dim string_1_Output As String = "test-command"
            Assert.IsTrue(string_1_Output = MatchFuzzyString(string_1, _
                New String() {"tEsT-COMMAND", "Nothing", "Ouch"}, "-"))

            Dim string_2 As String = "t-c"
            Dim string_2_Output As String = "test-command"
            Assert.IsTrue(string_2_Output = MatchFuzzyString(string_2, _
                New String() {"tEsT-COMMAND", "Nothing", "Ouch"}, "-"))

        End Sub

        <Test(Description:="Tests the MatchFuzzyString Method")> _
        Public Sub FuzzyMatching_3()

            Dim hasFiredException As Boolean = False

            Dim string_1 As String = "test"
            Try
                MatchFuzzyString(string_1, _
                                New String() {"tEsT1", "tEsT2", "tEsT3"})
            Catch ex As AmbiguousFuzzyStringMatchException
                Assert.AreEqual(string_1, ex.AmbiguousName)
                Assert.AreEqual(3, ex.Matches.Length)
                hasFiredException = True
            End Try

            Assert.IsTrue(hasFiredException)

        End Sub

        <Test(Description:="Tests the GetRegularExpressionMatchValue Method")> _
        Public Sub TestMatch()

            Dim match_1 As String = GetRegularExpressionMatchValue( _
                "{Server}/exchange/", _
                "(?<=\{)[A-z0-9][A-z]+[A-z0-9]*(?=\})")

            Assert.AreEqual("Server", match_1)

            Dim match_2 As String = GetRegularExpressionMatchValue( _
                "{Server]/exchange/", _
                "(?<=\{)[A-z0-9][A-z]+[A-z0-9]*(?=\})")

            Assert.IsTrue(String.IsNullOrEmpty(match_2))

        End Sub

        <Test(Description:="Tests the GetRegularExpressionMatchValues Method")> _
        Public Sub TestMatches()

            Dim matches_1 As String() = GetRegularExpressionMatchValues( _
                "{Server}/exchange/{Username}{0}{gg9}{3io}", _
                "(?<=\{)[A-z0-9][A-z]+[A-z0-9]*(?=\})")

            Assert.IsNotNull(matches_1)
            Assert.AreEqual(4, matches_1.Length)

            Assert.AreEqual("Server", matches_1(0))
            Assert.AreEqual("Username", matches_1(1))
            Assert.AreEqual("gg9", matches_1(2))
            Assert.AreEqual("3io", matches_1(3))


        End Sub

        <Test(Description:="Tests the CapitaliseString Method")> _
        Public Sub TestCapitalisation()

            Assert.AreEqual("Person.Given Name", CapitaliseString("person.given name", New String() {" ", "."}))
            Assert.AreEqual("Given Name", CapitaliseString("given name", New String() {" "}))

        End Sub

        <Test(Description:="Tests the CamelCaseWords Method")> _
        Public Sub TestCamelCasing()

            Assert.AreEqual("Given Name", CamelCaseWords("GivenName"))
            Assert.AreEqual("Givenname", CamelCaseWords("Givenname"))
            Assert.AreEqual("Given Name", CamelCaseWords("given name"))
            Assert.AreEqual("Person.Given Name", CamelCaseWords("Person.GivenName"))
            Assert.AreEqual("Anr", CamelCaseWords("ANR"))

        End Sub

    End Class

End Namespace

#End If