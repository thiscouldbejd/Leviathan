#If TESTING Then

Imports System.Configuration
Imports NUnit.Framework

Namespace Testing

    <TestFixture()> _
    Public Class ObjectManipulatorTests

#Region " Creation Tests "

        <Test(Description:="Checks the Creation of Objects")> _
        Public Sub CheckObjectCreation()

            Dim obj As TestManipulatedObject_1 = _
                TypeAnalyser.Create( _
                    GetType(TestManipulatedObject_1))

            Assert.IsNotNull(obj)

            obj = Nothing

        End Sub

#End Region

#Region " Population/Integration Tests "

        <Test(Description:="Checks Manipulation Population")> _
        Public Sub CheckPopulation()

            Dim obj As New TestManipulatedObject_1

            Populate(obj, _
                New DictionaryEntry() { _
                    New DictionaryEntry("Property1", 1), _
                    New DictionaryEntry("Property2", "Test") _
                } _
            )
            Assert.IsNotNull(obj)
            Assert.AreEqual(1, obj.Property1)
            Assert.AreEqual("Test", obj.Property2)

            Populate(obj, _
                New DictionaryEntry() { _
                    New DictionaryEntry("m_Field1", 2), _
                    New DictionaryEntry("m_Field2", "Test2") _
                } _
            )
            Assert.IsNotNull(obj)
            Assert.AreEqual(1, obj.Property1)
            Assert.AreEqual("Test", obj.Property2)
            Assert.AreEqual(2, obj.GetField1)
            Assert.AreEqual("Test2", obj.GetField2)

        End Sub

        <Test(Description:="Checks Manipulation Integration")> _
        Public Sub CheckIntegration()

            Dim obj_1 As New TestManipulatedObject_1(1)

            Dim obj_2 As New TestManipulatedObject_1
            obj_2.Property2 = "Test"
            obj_2.Property3 = New TestManipulatedObject_1(2)

            TypeAnalyser.Integrate(obj_1, obj_2)

            Assert.IsNotNull(obj_1)
            Assert.IsNotNull(obj_2)

            Assert.AreEqual(0, obj_1.Property1)
            Assert.AreEqual("Test", obj_1.Property2)

            Assert.IsNotNull(obj_1.Property3)
            Assert.AreEqual(2, obj_1.Property3.Property1)

        End Sub

#End Region

#Region " Read/Write Tests "

        <Test(Description:="Checks Manipulation Read")> _
        Public Sub CheckRead()

            Dim obj As New TestManipulatedObject_1

            obj.Property1 = 1
            obj.Property3 = New TestManipulatedObject_1
            obj.Property3.Property2 = "Test"

            Dim analyser As TypeAnalyser = TypeAnalyser.GetInstance(GetType(TestManipulatedObject_1))

            Dim member_1 As New MemberAnalyser("Property1")
            Assert.IsTrue(member_1.Parse(Nothing, Nothing, analyser))

            Assert.AreEqual(1, member_1.Read(obj))

            Dim member_2 As New MemberAnalyser("Property3.Property2")
            Assert.IsTrue(member_2.Parse(Nothing, Nothing, analyser))

            Assert.AreEqual("Test", member_2.Read(obj))

        End Sub

        <Test(Description:="Checks Manipulation Write")> _
        Public Sub CheckWrite()

            Dim obj As New TestManipulatedObject_1
            obj.Property3 = New TestManipulatedObject_1

            Dim analyser As TypeAnalyser = TypeAnalyser.GetInstance(GetType(TestManipulatedObject_1))

            Dim member_1 As New MemberAnalyser("Property1")
            Assert.IsTrue(member_1.Parse(Nothing, Nothing, analyser))
            Assert.IsTrue(member_1.Write(obj, 1))

            Assert.IsNotNull(obj)
            Assert.AreEqual(1, obj.Property1)

            Dim member_2 As New MemberAnalyser("Property3.Property2")
            Assert.IsTrue(member_2.Parse(Nothing, Nothing, analyser))
            Assert.IsTrue(member_2.Write(obj, "Test"))

            Assert.IsNotNull(obj)
            Assert.AreEqual("Test", obj.Property3.Property2)

        End Sub

#End Region

    End Class

End Namespace

#End If