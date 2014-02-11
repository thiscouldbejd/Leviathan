#If TESTING Then

Imports System.Configuration
Imports NUnit.Framework

Namespace Testing

    <TestFixture()> _
    Public Class ArrayManipulatorTests

#Region " Simple Tests "

        <Test(Description:="Checks the Removal of Start Elements from an Array")> _
        Public Sub CheckRemoveStartElements()

            Dim TestArray As String() = New String() _
                {"Test_1", "Test_2", "Test_3", "Test_4", "Test_5"}

            TestArray = RemoveStartElements(TestArray, 1)
            CheckArray(TestArray, "Test_2", "Test_3", "Test_4", "Test_5")

            TestArray = RemoveStartElements(TestArray, 3)
            CheckArray(TestArray, "Test_5")

            TestArray = RemoveStartElements(TestArray, 1)
            CheckArray(TestArray)

            TestArray = New String() _
                {"Test_1", "Test_2", "Test_3", "Test_4", "Test_5"}

            TestArray = RemoveStartElements(TestArray, 6)
            CheckArray(TestArray)

        End Sub

        <Test(Description:="Checks the Removal of End Elements from an Array")> _
        Public Sub CheckRemoveEndElements()

            Dim TestArray As String() = New String() _
                {"Test_1", "Test_2", "Test_3", "Test_4", "Test_5"}

            TestArray = RemoveEndElements(TestArray, 1)
            CheckArray(TestArray, "Test_1", "Test_2", "Test_3", "Test_4")

            TestArray = RemoveEndElements(TestArray, 3)
            CheckArray(TestArray, "Test_1")

            TestArray = RemoveEndElements(TestArray, 1)
            CheckArray(TestArray)

            TestArray = New String() _
                {"Test_1", "Test_2", "Test_3", "Test_4", "Test_5"}

            TestArray = RemoveEndElements(TestArray, 6)
            CheckArray(TestArray)

        End Sub

        <Test(Description:="Checks the Reconciliation of Elements from an Array")> _
        Public Sub CheckReconcileArrays()

            Dim TestArray_1 As String() = New String() _
                {"Test_3", "Test_4", "Test_5"}

            Dim TestArray_2 As String() = New String() _
                {"Test_1", "Test_2", "Test_3", "Test_4", "Test_5"}

            ReconcileArraysBySubtraction(TestArray_1, TestArray_2)
            CheckArray(TestArray_2, "Test_1", "Test_2")

            Dim TestArray_3 As String() = New String() _
                {"Test_1", "Test_2", "Test_3", "Test_4", "Test_5"}

            Dim TestArray_4 As String() = New String() _
                {"Test_1", "Test_2", "Test_3", "Test_4", "Test_5"}

            ReconcileArraysBySubtraction(TestArray_3, TestArray_4)
            CheckArray(TestArray_4)

            Dim TestArray_5 As String() = New String() _
                {}

            Dim TestArray_6 As String() = New String() _
                {"Test_1", "Test_2", "Test_3", "Test_4", "Test_5"}

            ReconcileArraysBySubtraction(TestArray_5, TestArray_6)
            CheckArray(TestArray_6, "Test_1", "Test_2", "Test_3", "Test_4", "Test_5")

        End Sub

        <Test(Description:="Checks the Element Types of an Array")> _
        Public Sub CheckElementTypes()

            Dim TestArray_1 As String() = New String() _
                {"Test_3", "Test_4", "Test_5"}
            Assert.AreEqual(GetType(String), _
                GetElementType(TestArray_1))

            Dim TestArray_2 As Object() = New Object() _
                {"Test_3", "Test_4", "Test_5"}
            Assert.AreEqual(GetType(String), _
                GetElementType(TestArray_2))

            Dim TestArray_3 As Integer() = New Integer() _
                {1, 2, 3}
            Assert.AreEqual(GetType(Integer), _
                GetElementType(TestArray_3))

            Dim TestArray_4 As Object() = New Object() _
                {1, 2, 3}
            Assert.AreEqual(GetType(Integer), _
                GetElementType(TestArray_4))

            Dim TestArray_5 As TestAnalysedObject_1() = New TestAnalysedObject_1() _
                {New TestAnalysedObject_1(), New TestAnalysedObject_1()}
            Assert.AreEqual(GetType(TestAnalysedObject_1), _
                GetElementType(TestArray_5))

            Dim TestArray_6 As Object() = New Object() _
                {New TestAnalysedObject_1(), New TestAnalysedObject_2()}
            Assert.AreEqual(GetType(Object), _
                GetElementType(TestArray_6))

            Dim TestArray_7 As Object() = New Object() _
                {New TestAnalysedObject_1(), New TestAnalysedObject_6()}
            Assert.AreEqual(GetType(TestAnalysedObject_1), _
                GetElementType(TestArray_7))

        End Sub

        <Test(Description:="Checks the Creation of an Array")> _
        Public Sub CheckToArray_1()

            CheckArray( _
                CreateArray("Test_1"), _
                "Test_1" _
            )

        End Sub

        <Test()> Public Sub CheckToArray_2()

            CheckArray( _
                CreateArray(New String() {"Test_1", "Test_2"}), _
                "Test_1", "Test_2" _
            )

        End Sub

        <Test()> Public Sub CheckToArray_3()

            Dim l As New ArrayList
            l.Add("Test_1")
            l.Add("Test_2")
            l.Add("Test_3")

            CheckArray( _
                CreateArray(l), _
                "Test_1", "Test_2", "Test_3" _
            )

        End Sub

#End Region

#Region " Private Methods "

        Private Sub CheckArray( _
            ByVal ary As Array, _
            ByVal ParamArray expectedObjects As Object() _
        )

            If expectedObjects Is Nothing Then expectedObjects = _
                Array.CreateInstance(GetType(Object), 0)

            Assert.IsNotNull(ary)
            Assert.AreEqual(ary.Length, expectedObjects.Length)

            For i As Integer = 0 To expectedObjects.Length - 1
                Assert.AreEqual(expectedObjects(i), ary(i))
            Next

        End Sub

#End Region

    End Class

End Namespace

#End If
