#If TESTING Then

Imports NUnit.Framework
Imports System.Configuration

Namespace Testing

    <TestFixture()> _
    Public Class ListManipulatorTests

#Region " Simple Tests "

        <Test(Description:="Checks the Removal of Start Elements from a List")> _
        Public Sub CheckRemoveStartElements()

            Dim TestList As New ArrayList(New String() _
                {"Test_1", "Test_2", "Test_3", "Test_4", "Test_5"})

            TestList = RemoveStartElements(TestList, 1)
            CheckList(TestList, "Test_2", "Test_3", "Test_4", "Test_5")

            TestList = RemoveStartElements(TestList, 3)
            CheckList(TestList, "Test_5")

            TestList = RemoveStartElements(TestList, 1)
            CheckList(TestList)

            TestList = New ArrayList(New String() _
                {"Test_1", "Test_2", "Test_3", "Test_4", "Test_5"})

            TestList = RemoveStartElements(TestList, 6)
            CheckList(TestList)

        End Sub

        <Test(Description:="Checks the Removal of End Elements from a List")> _
        Public Sub CheckRemoveEndElements()

            Dim TestList As New ArrayList(New String() _
                {"Test_1", "Test_2", "Test_3", "Test_4", "Test_5"})

            TestList = RemoveEndElements(TestList, 1)
            CheckList(TestList, "Test_1", "Test_2", "Test_3", "Test_4")

            TestList = RemoveEndElements(TestList, 3)
            CheckList(TestList, "Test_1")

            TestList = RemoveEndElements(TestList, 1)
            CheckList(TestList)

            TestList = New ArrayList(New String() _
                {"Test_1", "Test_2", "Test_3", "Test_4", "Test_5"})

            TestList = RemoveEndElements(TestList, 6)
            CheckList(TestList)

        End Sub

        <Test(Description:="Checks the Reconciliation of Elements from a List")> _
        Public Sub CheckReconcileLists()

            Dim TestList_1 As New ArrayList(New String() _
                {"Test_3", "Test_4", "Test_5"})

            Dim TestList_2 As New ArrayList(New String() _
                {"Test_1", "Test_2", "Test_3", "Test_4", "Test_5"})

            ReconcileLists(TestList_1, TestList_2)
            CheckList(TestList_2, "Test_1", "Test_2")

            Dim TestList_3 As New ArrayList(New String() _
                {"Test_1", "Test_2", "Test_3", "Test_4", "Test_5"})

            Dim TestList_4 As New ArrayList(New String() _
                {"Test_1", "Test_2", "Test_3", "Test_4", "Test_5"})

            ReconcileLists(TestList_3, TestList_4)
            CheckList(TestList_4)

            Dim TestList_5 As New ArrayList(New String() _
                {})

            Dim TestList_6 As New ArrayList(New String() _
                {"Test_1", "Test_2", "Test_3", "Test_4", "Test_5"})

            ReconcileLists(TestList_5, TestList_6)
            CheckList(TestList_6, "Test_1", "Test_2", "Test_3", "Test_4", "Test_5")

        End Sub

        <Test(Description:="Checks the Element Types of an List")> _
        Public Sub CheckElementTypes()

            Dim TestList_1 As New ArrayList()
            TestList_1.Add("Test_3")
            TestList_1.Add("Test_4")
            TestList_1.Add("Test_5")
            Assert.AreEqual(GetType(String), _
                GetElementType(TestList_1))

            Dim TestList_2 As New ArrayList()
            TestList_2.Add(1)
            TestList_2.Add(2)
            TestList_2.Add(3)
            Assert.AreEqual(GetType(Integer), _
                GetElementType(TestList_2))

            Dim TestList_3 As New ArrayList()
            TestList_3.Add(New TestAnalysedObject_1())
            TestList_3.Add(New TestAnalysedObject_1())
            Assert.AreEqual(GetType(TestAnalysedObject_1), _
                GetElementType(TestList_3))

            Dim TestList_4 As New ArrayList()
            TestList_4.Add(New TestAnalysedObject_1())
            TestList_4.Add(New TestAnalysedObject_2())
            Assert.AreEqual(GetType(Object), _
                GetElementType(TestList_4))

            Dim TestList_5 As New ArrayList()
            TestList_5.Add(New TestAnalysedObject_1())
            TestList_5.Add(New TestAnalysedObject_6())
            Assert.AreEqual(GetType(TestAnalysedObject_1), _
                GetElementType(TestList_5))

        End Sub

#End Region

#Region " Private Methods "

        Private Sub CheckList( _
            ByVal lst As IList, _
            ByVal ParamArray expectedObjects As Object() _
        )

            If expectedObjects Is Nothing Then expectedObjects = _
                Array.CreateInstance(GetType(Object), 0)

            Assert.IsNotNull(lst)
            Assert.AreEqual(lst.Count, expectedObjects.Length)

            For i As Integer = 0 To expectedObjects.Length - 1
                Assert.AreEqual(expectedObjects(i), lst(i))
            Next

        End Sub

#End Region

    End Class

End Namespace

#End If
