#If TESTING Then

Imports System.Configuration
Imports NUnit.Framework

Namespace Testing

	<TestFixture()> _
	Public Class MemberAnalyserTests

		#Region " Parsing Tests "

			<Test(Description:="Checks Member Parsing")> _
			Public Sub Parse_Test_1()

				Dim prop_1 As New MemberAnalyser("Test")
				Assert.AreEqual(0, prop_1.Level)
				Assert.IsFalse(prop_1.IsMultiLeveled)

				Dim prop_2 As New MemberAnalyser("Test.Test")
				Assert.AreEqual(1, prop_2.Level)
				Assert.IsTrue(prop_2.IsMultiLeveled)

				Dim prop_3 As New MemberAnalyser("Test.Test.Test")
				Assert.AreEqual(2, prop_3.Level)
				Assert.IsTrue(prop_3.IsMultiLeveled)

				Dim prop_4 As New MemberAnalyser("Test.Test.Test.Test")
				Assert.AreEqual(3, prop_4.Level)
				Assert.IsTrue(prop_4.IsMultiLeveled)

			End Sub

			<Test(Description:="Checks Member Parsing")> _
			Public Sub Parse_Test_2()

				Dim prop_1 As MemberAnalyser = Nothing

				Assert.IsTrue(MemberAnalyser.TryParse("Test", prop_1))
				Assert.AreEqual(0, prop_1.Level)
				Assert.IsFalse(prop_1.IsMultiLeveled)

				Dim prop_2 As MemberAnalyser = Nothing

				Assert.IsTrue(MemberAnalyser.TryParse("Test.Test", prop_2))
				Assert.AreEqual(1, prop_2.Level)
				Assert.IsTrue(prop_2.IsMultiLeveled)

				Dim prop_3 As MemberAnalyser = Nothing

				Assert.IsTrue(MemberAnalyser.TryParse("Test.Test.Test", prop_3))
				Assert.AreEqual(2, prop_3.Level)
				Assert.IsTrue(prop_3.IsMultiLeveled)

				Dim prop_4 As MemberAnalyser = Nothing

				Assert.IsTrue(MemberAnalyser.TryParse("Test.Test.Test.Test", prop_4))
				Assert.AreEqual(3, prop_4.Level)
				Assert.IsTrue(prop_4.IsMultiLeveled)

			End Sub

		#End Region

		#Region " Comparison Tests "

			<Test(Description:="Checks Comparison of Member Analysers")> _
			Public Sub Compare_Test_1()

				Dim props_AsString_1 As String() = New String() _
				{"Test3", "Test1", "Test2"}

				Dim props_AsString_2 As String() = New String() _
				{"A.B", "A.B.B", "A.B.A", "A.A", "A", "E.F.A", "D.A", "C", "D"}

				Dim props_1(props_AsString_1.Length - 1) As MemberAnalyser

				Dim props_2(props_AsString_2.Length - 1) As MemberAnalyser

				For i As Integer = 0 To props_AsString_1.Length - 1

				MemberAnalyser.TryParse(props_AsString_1(i), props_1(i))

				Next

				For i As Integer = 0 To props_AsString_2.Length - 1

				MemberAnalyser.TryParse(props_AsString_2(i), props_2(i))

				Next

				Array.Sort(props_1, New Comparison.Comparer)

				Assert.AreEqual(props_AsString_1.Length, props_1.Length)
				Assert.AreEqual("Test1", props_1(0).Name)
				Assert.AreEqual("Test2", props_1(1).Name)
				Assert.AreEqual("Test3", props_1(2).Name)

				Array.Sort(props_2, New Comparison.Comparer)

				Assert.AreEqual(props_AsString_2.Length, props_2.Length)
				Assert.AreEqual("A", props_2(0).FullName)
				Assert.AreEqual("A.A", props_2(1).FullName)
				Assert.AreEqual("A.B", props_2(2).FullName)
				Assert.AreEqual("A.B.A", props_2(3).FullName)
				Assert.AreEqual("A.B.B", props_2(4).FullName)
				Assert.AreEqual("C", props_2(5).FullName)
				Assert.AreEqual("D", props_2(6).FullName)
				Assert.AreEqual("D.A", props_2(7).FullName)
				Assert.AreEqual("E.F.A", props_2(8).FullName)

			End Sub

		#End Region

	End Class

End Namespace

#End If