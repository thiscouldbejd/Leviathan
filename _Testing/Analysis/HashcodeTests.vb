#If TESTING Then

Imports Leviathan.Caching.Simple
Imports System.Configuration
Imports NUnit.Framework

Namespace Testing

    <TestFixture()> _
    Public Class HashcodeTests

        Private Function CheckHashCodeNotPresent( _
            ByRef list As IList, _
            ByVal value As Object _
        ) As Boolean

            Dim value_Hash As Integer = value.GetHashCode

            Dim retVal As Boolean = Not list.Contains(value_Hash)

            If retVal Then list.Add(value_Hash)

            Return retVal

        End Function

        <Test(Description:="Tests Simple HashCode")> _
        Public Sub CheckSimpleQuery_1()

            Dim query_1 As AnalyserQuery = _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetLocation(MemberLocation.All) _
                        .SetAccessibility(MemberAccessibility.ReadWrite) _
                        .SetNumberOfResults(1)

            Dim startTime As DateTime = Now

            Dim last_Hash As Integer

            For i As Integer = 0 To 100000

                Dim hash As Integer = query_1.GetHashCode

                If Not i = 0 Then

                    Assert.AreEqual(last_Hash, hash)

                End If

                last_Hash = hash

            Next

            startTime = Now

            For i As Integer = 0 To 100000

                Dim hash As Integer = CombineHashCodes(query_1.GetType, query_1)

                If Not i = 0 Then

                    Assert.AreEqual(last_Hash, hash)

                End If

                last_Hash = hash

            Next

        End Sub

        <Test(Description:="Tests Simple HashCode")> _
        Public Sub CheckSimpleQuery_2()

            Dim aryList As New ArrayList

            Assert.IsTrue( _
                CheckHashCodeNotPresent(aryList, _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetLocation(MemberLocation.All) _
                        .SetAccessibility(MemberAccessibility.ReadWrite) _
                        .SetNumberOfResults(1) _
                    ) _
                )
                   
            Assert.IsTrue( _
                CheckHashCodeNotPresent(aryList, _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetLocation(MemberLocation.All) _
                        .SetAccessibility(MemberAccessibility.ReadWrite) _
                        .SetNumberOfResults(2) _
                    ) _
                )

            Assert.IsTrue( _
                CheckHashCodeNotPresent(aryList, _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetLocation(MemberLocation.Static) _
                        .SetAccessibility(MemberAccessibility.ReadOnly) _
                        .SetNumberOfResults(1) _
                    ) _
                )

            Assert.IsTrue( _
                CheckHashCodeNotPresent(aryList, _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetVisibility(MemberVisibility.Private) _
                        .SetLocation(MemberLocation.Static) _
                        .SetAccessibility(MemberAccessibility.ReadOnly) _
                    ) _
                )

        End Sub

    End Class

End Namespace

#End If