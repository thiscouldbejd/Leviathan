#If TESTING Then

Imports Leviathan.Comparison.Comparer
Imports System.Configuration
Imports NUnit.Framework

Namespace Testing

    <TestFixture()> _
    Public Class AnalyserQueryTests

        #Region " General Testing "

                <Test(Description:="Visibility Enum Testing")> _
                    Public Sub VisibilityEnumTest()

                    Dim queryPart As New VisibilityPart(MemberVisibility.All)

                    Assert.IsTrue(queryPart.Visibility = MemberVisibility.All)
                    Assert.IsTrue(EnumContains(queryPart.Visibility, MemberVisibility.NonPublic))
                    Assert.IsTrue(EnumContains(queryPart.Visibility, MemberVisibility.Public))

                    queryPart.Visibility = MemberVisibility.NonPublic Or MemberVisibility.Public

                    Assert.IsFalse(queryPart.Visibility = MemberVisibility.All)
                    Assert.IsTrue(EnumContains(queryPart.Visibility, MemberVisibility.NonPublic))
                    Assert.IsTrue(EnumContains(queryPart.Visibility, MemberVisibility.Public))

                    queryPart.Visibility = MemberVisibility.NonPublic

                    Assert.IsFalse(queryPart.Visibility = MemberVisibility.All)
                    Assert.IsTrue(EnumContains(queryPart.Visibility, MemberVisibility.NonPublic))
                    Assert.IsFalse(EnumContains(queryPart.Visibility, MemberVisibility.Public))

                    queryPart.Visibility = MemberVisibility.Public

                    Assert.IsFalse(queryPart.Visibility = MemberVisibility.All)
                    Assert.IsFalse(EnumContains(queryPart.Visibility, MemberVisibility.NonPublic))
                    Assert.IsTrue(EnumContains(queryPart.Visibility, MemberVisibility.Public))

                End Sub

                <Test(Description:="Accessiblity Enum Testing")> Public Sub AccessiblityEnumTest()

                    Dim queryPart As New AccessibilityPart(MemberAccessibility.All)

                    Assert.IsTrue(queryPart.Accessibility = MemberAccessibility.All)
                    Assert.IsTrue(queryPart.Accessibility Or MemberAccessibility.Readable = MemberAccessibility.Readable)
                    Assert.IsTrue(queryPart.Accessibility Or MemberAccessibility.Readable = MemberAccessibility.ReadOnly)
                    Assert.IsTrue(queryPart.Accessibility Or MemberAccessibility.Readable = MemberAccessibility.ReadWrite)
                    Assert.IsTrue(queryPart.Accessibility Or MemberAccessibility.Readable = MemberAccessibility.Writable)
                    Assert.IsTrue(queryPart.Accessibility Or MemberAccessibility.Readable = MemberAccessibility.WriteOnly)

                    queryPart.Accessibility = MemberAccessibility.Readable

                    Assert.IsTrue(queryPart.Accessibility And MemberAccessibility.Readable)
                    Assert.IsFalse(queryPart.Accessibility And MemberAccessibility.ReadOnly)
                    Assert.IsFalse(queryPart.Accessibility And MemberAccessibility.ReadWrite)
                    Assert.IsFalse(queryPart.Accessibility And MemberAccessibility.Writable)
                    Assert.IsFalse(queryPart.Accessibility And MemberAccessibility.WriteOnly)

                End Sub
                
        #End Region

#Region " HashCode Testing "

        <Test(Description:="Tests Member Querying with Visibility and Location.")> _
       Public Sub CheckHashCodes_1()

            Dim lstHashCodes As New ArrayList

            Dim hash_1 As Integer = _
                New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetLocation(MemberLocation.All).GetHashCode
            Assert.IsFalse(lstHashCodes.Contains(hash_1))
            lstHashCodes.Add(hash_1)

            Dim hash_2 As Integer = _
                New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetVisibility(MemberVisibility.Public) _
                        .SetLocation(MemberLocation.All).GetHashCode
            Assert.IsFalse(lstHashCodes.Contains(hash_2))
            lstHashCodes.Add(hash_2)

            Dim hash_3 As Integer = _
                New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetVisibility(MemberVisibility.NonPublic) _
                        .SetLocation(MemberLocation.All).GetHashCode
            Assert.IsFalse(lstHashCodes.Contains(hash_3))
            lstHashCodes.Add(hash_3)

            Dim hash_4 As Integer = _
                New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetVisibility(MemberVisibility.Protected) _
                        .SetLocation(MemberLocation.All).GetHashCode
            Assert.IsFalse(lstHashCodes.Contains(hash_4))
            lstHashCodes.Add(hash_4)

            Dim hash_5 As Integer = _
                New AnalyserQuery() _
                        .SetReturnType(AnalyserType.ConstructorAnalyser) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetLocation(MemberLocation.All) _
                        .SetDeclaredBelowType(GetType(Object)).GetHashCode
            Assert.IsFalse(lstHashCodes.Contains(hash_5))
            lstHashCodes.Add(hash_5)

            Dim hash_6 As Integer = _
                New AnalyserQuery() _
                        .SetReturnType(AnalyserType.ConstructorAnalyser) _
                        .SetArgumentCount(1) _
                        .SetLocation(MemberLocation.All) _
                        .SetVisibility(MemberVisibility.All).GetHashCode
            Assert.IsFalse(lstHashCodes.Contains(hash_6))
            lstHashCodes.Add(hash_6)

            Dim hash_7 As Integer = _
                New AnalyserQuery() _
                            .SetReturnType(AnalyserType.ConstructorAnalyser) _
                            .SetArgumentCount(2) _
                            .SetLocation(MemberLocation.All) _
                            .SetVisibility(MemberVisibility.All).GetHashCode
            Assert.IsFalse(lstHashCodes.Contains(hash_7))
            lstHashCodes.Add(hash_7)

            Dim hash_8 As Integer = _
            New AnalyserQuery() _
                            .SetReturnType(AnalyserType.MethodAnalyser) _
                            .SetArgumentCount(1) _
                            .SetVisibility(MemberVisibility.All) _
                            .SetLocation(MemberLocation.All).GetHashCode
            Assert.IsFalse(lstHashCodes.Contains(hash_8))
            lstHashCodes.Add(hash_8)

            Dim hash_9 As Integer = _
            New AnalyserQuery() _
                            .SetReturnType(AnalyserType.MethodAnalyser) _
                            .SetArgumentCount(2) _
                            .SetVisibility(MemberVisibility.All) _
                            .SetLocation(MemberLocation.All).GetHashCode
            Assert.IsFalse(lstHashCodes.Contains(hash_9))
            lstHashCodes.Add(hash_9)

            Dim hash_10 As Integer = _
            New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetLocation(MemberLocation.All) _
                        .SetIsVariable().GetHashCode
            Assert.IsFalse(lstHashCodes.Contains(hash_10))
            lstHashCodes.Add(hash_10)

            Dim hash_11 As Integer = _
            New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetLocation(MemberLocation.All) _
                        .SetAccessibility(MemberAccessibility.ReadOnly).GetHashCode
            Assert.IsFalse(lstHashCodes.Contains(hash_11))
            lstHashCodes.Add(hash_11)

            Dim hash_12 As Integer = _
                        New AnalyserQuery() _
                                    .SetReturnType(AnalyserType.MemberAnalyser) _
                                    .SetVisibility(MemberVisibility.All) _
                                    .SetLocation(MemberLocation.All) _
                                    .SetAccessibility(MemberAccessibility.WriteOnly).GetHashCode
            Assert.IsFalse(lstHashCodes.Contains(hash_12))
            lstHashCodes.Add(hash_12)

        End Sub

        <Test(Description:="Tests Hashcode Collisions.")> _
        Public Sub ChechHashCodes_2()

            Dim hashCodes As New ArrayList
            Dim typeHashCodes As New ArrayList

            For i As Integer = 0 To 1000

                Dim q As AnalyserQuery = New AnalyserQuery() _
                    .SetReturnType(AnalyserType.MemberAnalyser) _
                    .SetName("Index") _
                    .SetVisibility(MemberVisibility.All) _
                    .SetLocation(MemberLocation.All) _
                    .SetAccessibility(MemberAccessibility.All) _
                    .SetNumberOfResults(1)

                Dim hash_Code As Integer = q.GetHashCode
                Dim type_Hash_Code As Integer = q.ReturnType.GetHashCode

                hashCodes.Add(hash_Code)
                typeHashCodes.Add(type_Hash_Code)

            Next

            typeHashCodes.Sort()
            hashCodes.Sort()

            For i As Integer = 1 To typeHashCodes.Count - 1

                If typeHashCodes(i) <> typeHashCodes(i - 1) Then Assert.Fail("Different Type HashCodes: {0} {1}", typeHashCodes(i), typeHashCodes(i - 1))

            Next

            For i As Integer = 1 To hashCodes.Count - 1

                If hashCodes(i) <> hashCodes(i - 1) Then Assert.Fail("Different HashCodes: {0} {1}", hashCodes(i), hashCodes(i - 1))

            Next

        End Sub

#End Region

    End Class

End Namespace

#End If