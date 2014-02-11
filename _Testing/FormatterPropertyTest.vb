#If TESTING Then

Imports Leviathan.Commands
Imports System.Configuration
Imports NUnit.Framework

Namespace Testing
    <TestFixture()> _
    Public Class FormatterPropertyTest

#Region " Public Test Methods "

        <Test()> Public Sub CheckParsing_1()

            ' False Testing
            TestFormatterPropertyParse(Nothing, False)

            TestFormatterPropertyParse("", False)

            TestFormatterPropertyParse(":1", False)

            ' Simple Testing
            TestFormatterPropertyParse("test", True, "test")

            TestFormatterPropertyParse("test:1", True, "test", 1)

            TestFormatterPropertyParse("test(Display):1", True, "test", 1, Nothing, "Display")

            TestFormatterPropertyParse("test(Display)[index]:1", True, "test", 1, "index", "Display")

            ' Jumbled Testing
            TestFormatterPropertyParse("test:1[index](Display)", True, "test", 1, "index", "Display")

            TestFormatterPropertyParse("test[index]:1(Display)", True, "test", 1, "index", "Display")

            TestFormatterPropertyParse("test[index](Display):1", True, "test", 1, "index", "Display")

        End Sub

        <Test()> Public Sub CheckParsing_2()

            Dim complexTestPropertyAsString As String = "test1:1[index1](Display1).test2(Display2):2[index2]"

            Dim complexTestProperty As FormatterProperty = Nothing

            Assert.IsTrue(FormatterProperty.TryParse(complexTestPropertyAsString, complexTestProperty))

            Assert.IsNotNull(complexTestProperty)

            Assert.AreEqual("test1", complexTestProperty.Name)
            Assert.AreEqual(1, complexTestProperty.Length)
            Assert.AreEqual("Display1", complexTestProperty.DisplayName)
            Assert.AreEqual("index1", complexTestProperty.Index)

            Assert.IsNotNull(complexTestProperty.Child)
            Assert.IsTrue(complexTestProperty.IsMultiLeveled)
            Assert.AreEqual(1, complexTestProperty.Level)

            Assert.AreEqual("test2", complexTestProperty.Child.Name)
            Assert.AreEqual(2, complexTestProperty.Child.Length)
            Assert.AreEqual("Display2", complexTestProperty.Child.DisplayName)
            Assert.AreEqual("index2", complexTestProperty.Child.Index)

            Assert.AreEqual("test1.test2", complexTestProperty.FullName)

        End Sub

        <Test()> Public Sub CheckParsing_3()

            Dim complexTestPropertyAsString As String = _
                "test1:1[index1](Display1).test2(Display2):2[index2].test3(Display3):3[index3]"

            Dim complexTestProperty As FormatterProperty = Nothing

            Assert.IsTrue(FormatterProperty.TryParse(complexTestPropertyAsString, complexTestProperty))

            Assert.IsNotNull(complexTestProperty)

            Assert.AreEqual("test1", complexTestProperty.Name)
            Assert.AreEqual(1, complexTestProperty.Length)
            Assert.AreEqual("Display1", complexTestProperty.DisplayName)
            Assert.AreEqual("index1", complexTestProperty.Index)

            Assert.IsNotNull(complexTestProperty.Child)
            Assert.IsTrue(complexTestProperty.IsMultiLeveled)
            Assert.AreEqual(2, complexTestProperty.Level)

            Assert.AreEqual("test2", complexTestProperty.Child.Name)
            Assert.AreEqual(2, complexTestProperty.Child.Length)
            Assert.AreEqual("Display2", complexTestProperty.Child.DisplayName)
            Assert.AreEqual("index2", complexTestProperty.Child.Index)

            Assert.IsNotNull(complexTestProperty.Child.Child)
            Assert.IsTrue(complexTestProperty.Child.IsMultiLeveled)
            Assert.AreEqual(1, complexTestProperty.Child.Level)

            Assert.AreEqual("test3", complexTestProperty.Child.Child.Name)
            Assert.AreEqual(3, complexTestProperty.Child.Child.Length)
            Assert.AreEqual("Display3", complexTestProperty.Child.Child.DisplayName)
            Assert.AreEqual("index3", complexTestProperty.Child.Child.Index)

            Assert.AreEqual("test1.test2.test3", complexTestProperty.FullName)
            Assert.AreEqual("test2.test3", complexTestProperty.Child.FullName)
            Assert.AreEqual("test3", complexTestProperty.Child.Child.FullName)

        End Sub

#End Region

#Region " Private Methods "

        Private Sub TestFormatterPropertyParse( _
                    ByVal propertyAsString As String, _
                    ByVal expectedResult As Boolean, _
                    Optional ByVal propertyName As String = Nothing, _
                    Optional ByVal propertyLength As Integer = 0, _
                    Optional ByVal propertyIndex As String = Nothing, _
                    Optional ByVal propertyDisplay As String = Nothing _
                )

            Dim testProperty As FormatterProperty = Nothing

            If Not expectedResult Then

                Assert.IsFalse(FormatterProperty.TryParse(propertyAsString, testProperty))

            Else

                Assert.IsTrue(FormatterProperty.TryParse(propertyAsString, testProperty))

                Assert.IsNotNull(testProperty)

                If String.IsNullOrEmpty(propertyName) Then
                    Assert.IsTrue(String.IsNullOrEmpty(testProperty.Name))
                Else
                    Assert.AreEqual(propertyName, testProperty.Name)
                End If

                Assert.AreEqual(testProperty.Length, propertyLength)

                If String.IsNullOrEmpty(propertyIndex) Then
                    Assert.IsTrue(String.IsNullOrEmpty(testProperty.Index))
                Else
                    Assert.AreEqual(propertyIndex, testProperty.Index)
                End If

                If Not String.IsNullOrEmpty(propertyDisplay) Then
                    Assert.AreEqual(propertyDisplay, testProperty.DisplayName)
                End If

            End If

        End Sub

#End Region

    End Class

End Namespace

#End If