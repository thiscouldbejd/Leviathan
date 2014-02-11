#If TESTING Then

Imports Leviathan.Caching
Imports System.Configuration
Imports NUnit.Framework

Namespace Testing

    <TestFixture(Description:="Simple Cache Testing")> _
    Public Class SimpleCacheTests

        <Test(Description:="Tests the GetCacheSignature Method")> _
        Public Sub TestGetCacheSignature()

            Dim hashCodes As New ArrayList

            Dim methodCodes As Integer() = _
                New Integer() {1, 100, 999, 134243, 444332, 5, _
                               33, 3, 35, 2, 54, 12, 324324324}

            Dim parameterCodes As Integer()() = _
                New Integer()() { _
                    New Integer() {3, 342, 9399}, _
                    New Integer() {3, 63, 634}, _
                    New Integer() {4, 1}, _
                    New Integer() {1, 222, 334324}, _
                    New Integer() {1, 12, 3}, _
                    New Integer() {1, 23, 3}, _
                    New Integer() {1, 125, 3}, _
                    New Integer() {1, 123, 3}, _
                    New Integer() {1, 43535, 3}, _
                    New Integer() {1, 213, 3}, _
                    New Integer() {1, 2, 553}, _
                    New Integer() {891, 2, 3}, _
                    New Integer() {1, 0, 3}, _
                    New Integer() {0} _
                }

            For i As Integer = 0 To methodCodes.Length - 1

                For j As Integer = 0 To parameterCodes.Length - 1

                    Dim signatureCode As Int64 = _
                        Simple.GetCacheSignature(methodCodes(i), parameterCodes(j))

                    Assert.IsFalse(hashCodes.Contains(signatureCode))

                    hashCodes.Add(signatureCode)

                Next

            Next

        End Sub

        <Test(Description:="Tests the Get Method")> _
        Public Sub TestGet()

            Dim cache As Simple = _
                Simple.GetInstance(GetType(String).GetHashCode)

            Assert.IsNotNull(cache)

            cache.Set("Test1", "TestGet".GetHashCode, 1, 2, 3)
            cache.Set("Test2", "TestGet".GetHashCode, 1, 20, 30)
            cache.Set("Test3", "TestGet".GetHashCode, 1, 200, 300)
            cache.Set("Test4", "TestGet".GetHashCode, 1, 2000, 3000)
            cache.Set("Test5", "TestGet".GetHashCode, 1, 20000, 30000)
            cache.Set("Test6", "TestGet".GetHashCode, 1, 200000, 300000)

            Dim string_1 As String = Nothing
            Assert.IsTrue(cache.TryGet(string_1, "TestGet".GetHashCode, 1, 2, 3))
            Assert.AreEqual("Test1", string_1)

            Dim string_2 As String = Nothing
            Assert.IsTrue(cache.TryGet(string_2, "TestGet".GetHashCode, 1, 20, 30))
            Assert.AreEqual("Test2", string_2)

            Dim string_3 As String = Nothing
            Assert.IsTrue(cache.TryGet(string_3, "TestGet".GetHashCode, 1, 200, 300))
            Assert.AreEqual("Test3", string_3)

            Dim string_4 As String = Nothing
            Assert.IsTrue(cache.TryGet(string_4, "TestGet".GetHashCode, 1, 2000, 3000))
            Assert.AreEqual("Test4", string_4)

            Dim string_5 As String = Nothing
            Assert.IsTrue(cache.TryGet(string_5, "TestGet".GetHashCode, 1, 20000, 30000))
            Assert.AreEqual("Test5", string_5)

            Dim string_6 As String = Nothing
            Assert.IsTrue(cache.TryGet(string_6, "TestGet".GetHashCode, 1, 200000, 300000))
            Assert.AreEqual("Test6", string_6)

            cache = Nothing

            cache = Simple.GetInstance(GetType(String).GetHashCode)

            string_1 = Nothing
            Assert.IsTrue(cache.TryGet(string_1, "TestGet".GetHashCode, 1, 2, 3))
            Assert.AreEqual("Test1", string_1)

            string_2 = Nothing
            Assert.IsTrue(cache.TryGet(string_2, "TestGet".GetHashCode, 1, 20, 30))
            Assert.AreEqual("Test2", string_2)

            string_3 = Nothing
            Assert.IsTrue(cache.TryGet(string_3, "TestGet".GetHashCode, 1, 200, 300))
            Assert.AreEqual("Test3", string_3)

            string_4 = Nothing
            Assert.IsTrue(cache.TryGet(string_4, "TestGet".GetHashCode, 1, 2000, 3000))
            Assert.AreEqual("Test4", string_4)

            string_5 = Nothing
            Assert.IsTrue(cache.TryGet(string_5, "TestGet".GetHashCode, 1, 20000, 30000))
            Assert.AreEqual("Test5", string_5)

            string_6 = Nothing
            Assert.IsTrue(cache.TryGet(string_6, "TestGet".GetHashCode, 1, 200000, 300000))
            Assert.AreEqual("Test6", string_6)
        End Sub

    End Class

End Namespace

#End If
