#If TESTING Then

Imports Leviathan.Commands
Imports Leviathan.Visualisation
Imports System.Configuration
Imports NUnit.Framework

Namespace Testing

	<TestFixture()> Public Class StandardFormatterTest
	
		<Test()> Public Sub CheckSimpleIListFormat()
		
			Dim objList As New ArrayList()
			
			For i As Integer = 0 To 9
			
				objList.Add(New TestFormattableObject_1(i, "Value_" & i.ToString))
				
			Next
			
			Dim formatter As New FormatCommands(Nothing)
			
			formatter.FieldsToOverride = FormatterProperty.Create("Property1").ToArray()
			
			Dim results_1 As Cube = formatter.ProcessCommandStandard(objList, Nothing)
			
			formatter.FieldsToOverride = FormatterProperty.Create("Property2").ToArray()
			
			Dim results_2 As Cube = formatter.ProcessCommandStandard(objList, Nothing)
			
			formatter.FieldsToOverride = FormatterProperty.Create("Property1","Property2").ToArray()
			
			Dim results_3 As Cube = formatter.ProcessCommandStandard(objList, Nothing)
			
			Assert.IsNotNull(results_1)
			Assert.AreEqual(1, results_1.Columns.Count)
			Assert.AreEqual(10, results_1.LastSlice.Rows.Count)
			For i As Integer = 0 To results_1.LastSlice.Rows.Count - 1
			
				Assert.AreEqual(i.ToString, results_1.LastSlice.Rows(i)(0).ToString)
				
			Next
			
			Assert.IsNotNull(results_2)
			Assert.AreEqual(1, results_2.Columns.Count)
			Assert.AreEqual(10, results_2.LastSlice.Rows.Count)
			For i As Integer = 0 To results_1.LastSlice.Rows.Count - 1
			
				Assert.AreEqual("Value_" & i.ToString, results_2.LastSlice.Rows(i)(0).ToString)
				
			Next
			
			Assert.IsNotNull(results_3)
			Assert.AreEqual(2, results_3.Columns.Count)
			Assert.AreEqual(10, results_3.LastSlice.Rows.Count)
			For i As Integer = 0 To results_1.LastSlice.Rows.Count - 1
			
				Assert.AreEqual(i.ToString, results_3.LastSlice.Rows(i)(0).ToString)
				
				Assert.AreEqual("Value_" & i.ToString, results_3.LastSlice.Rows(i)(1).ToString)
				
			Next
			
		End Sub
		
		<Test()> Public Sub CheckSimpleArrayFormat()
		
			Dim objList As New ArrayList()
			
			For i As Integer = 0 To 9
			
				objList.Add(New TestFormattableObject_1(i, "Value_" & i.ToString))
				
			Next
			
			Dim objArray As TestFormattableObject_1() = objList.ToArray(GetType(TestFormattableObject_1))
			
			Dim formatter As New FormatCommands(Nothing)
			
			formatter.FieldsToOverride = FormatterProperty.Create("Property1").ToArray()
			
			Dim results_1 As Cube = formatter.ProcessCommandStandard(objArray, Nothing)
			
			formatter.FieldsToOverride = FormatterProperty.Create("Property2").ToArray()
			
			Dim results_2 As Cube = formatter.ProcessCommandStandard(objArray, Nothing)
			
			formatter.FieldsToOverride = FormatterProperty.Create("Property1", "Property2").ToArray()
			
			Dim results_3 As Cube = formatter.ProcessCommandStandard(objArray, Nothing)
			
			Assert.IsNotNull(results_1)
			Assert.AreEqual(1, results_1.Columns.Count)
			Assert.AreEqual(10, results_1.LastSlice.Rows.Count)
			For i As Integer = 0 To results_1.LastSlice.Rows.Count - 1
			
				Assert.AreEqual(i.ToString, results_1.LastSlice.Rows(i)(0).ToString)
				
			Next
			
			Assert.IsNotNull(results_2)
			Assert.AreEqual(1, results_2.Columns.Count)
			Assert.AreEqual(10, results_2.LastSlice.Rows.Count)
			For i As Integer = 0 To results_1.LastSlice.Rows.Count - 1
			
				Assert.AreEqual("Value_" & i.ToString, results_2.LastSlice.Rows(i)(0).ToString)
				
			Next
			
			Assert.IsNotNull(results_3)
			Assert.AreEqual(2, results_3.Columns.Count)
			Assert.AreEqual(10, results_3.LastSlice.Rows.Count)
			For i As Integer = 0 To results_1.LastSlice.Rows.Count - 1
			
				Assert.AreEqual(i.ToString, CType(results_3.LastSlice.Rows(i), Row)(0).ToString)
				Assert.AreEqual("Value_" & i.ToString, results_3.LastSlice.Rows(i)(1).ToString)
				
			Next
			
		End Sub
		
		<Test()> Public Sub CheckIListFormat_1()
		
			Dim objList As New ArrayList()
			
			For i As Integer = 0 To 9
			
				Dim obj As New TestFormattableObject_1(i, "Value_" & i.ToString)
				obj.Property3 = New TestFormattableObject_1(i + 10, "Value_" & (i + 10).ToString)
				objList.Add(obj)
				
			Next
			
			Dim formatter As New FormatCommands(Nothing)
			
			formatter.FieldsToOverride = FormatterProperty.Create("Property1", "Property3.Property1").ToArray()
			
			Dim results_1 As Cube = formatter.ProcessCommandStandard(objList, Nothing)
			
			formatter.FieldsToOverride = FormatterProperty.Create("Property2", "Property3.Property2").ToArray()
			
			Dim results_2 As Cube = formatter.ProcessCommandStandard(objList, Nothing)
			
			formatter.FieldsToOverride = FormatterProperty.Create("Property1", "Property2", "Property3.Property1", "Property3.Property2").ToArray()
			
			Dim results_3 As Cube = formatter.ProcessCommandStandard(objList, Nothing)
			
			Assert.IsNotNull(results_1)
			Assert.AreEqual(2, results_1.Columns.Count)
			Assert.AreEqual(10, results_1.LastSlice.Rows.Count)
			For i As Integer = 0 To results_1.LastSlice.Rows.Count - 1
			
				Assert.AreEqual(i.ToString, results_1.LastSlice.Rows(i)(0).ToString)
				Assert.AreEqual((i + 10).ToString, results_1.LastSlice.Rows(i)(1).ToString)
				
			Next
			
			Assert.IsNotNull(results_2)
			Assert.AreEqual(2, results_2.Columns.Count)
			Assert.AreEqual(10, results_2.LastSlice.Rows.Count)
			For i As Integer = 0 To results_1.LastSlice.Rows.Count - 1
			
				Assert.AreEqual("Value_" & i.ToString, results_2.LastSlice.Rows(i)(0).ToString)
				Assert.AreEqual("Value_" & (i + 10).ToString, results_2.LastSlice.Rows(i)(1).ToString)
				
			Next
			
			Assert.IsNotNull(results_3)
			Assert.AreEqual(4, results_3.Columns.Count)
			Assert.AreEqual(10, results_3.LastSlice.Rows.Count)
			For i As Integer = 0 To results_3.LastSlice.Rows.Count - 1
			
				Assert.AreEqual(i.ToString, results_3.LastSlice.Rows(i)(0).ToString)
				Assert.AreEqual("Value_" & i.ToString, results_3.LastSlice.Rows(i)(1).ToString)
				Assert.AreEqual((i + 10).ToString, results_3.LastSlice.Rows(i)(2).ToString)
				Assert.AreEqual("Value_" & (i + 10).ToString, results_3.LastSlice.Rows(i)(3).ToString)
				
			Next
			
		End Sub
		
		<Test()> Public Sub CheckIListFormat_2()
		
			Dim objList As New ArrayList()
			
			Dim obj_1 As New TestFormattableObject_2()
			obj_1.Property1 = 1
			objList.Add(obj_1)
			
			Dim obj_2 As New TestFormattableObject_2()
			obj_2.Property1 = 2
			objList.Add(obj_2)
			
			Dim obj_3 As New TestFormattableObject_2()
			obj_3.Property1 = 3
			Dim obj_3_List As New ArrayList()
			For i As Integer = 0 To 9
			
				Dim obj As New TestFormattableObject_1(i, "Value_" & i.ToString)
				obj_3_List.Add(obj)
				
			Next
			obj_3.PropertyArray = obj_3_List.ToArray(GetType(TestFormattableObject_1))
			objList.Add(obj_3)
			
			Dim formatter As New FormatCommands(Nothing)
			
			formatter.FieldsToOverride = FormatterProperty.Create("Property1", "PropertyArray.Property1", "PropertyArray.Property2").ToArray()
			
			Dim results_1 As Cube = formatter.ProcessCommandStandard(objList, Nothing)
			
			Assert.IsNotNull(results_1)
			Assert.AreEqual(3, results_1.Columns.Count)
			Assert.AreEqual(12, results_1.LastSlice.Rows.Count)
			
			Assert.AreEqual("1", results_1.LastSlice.Rows(0)(0).ToString)
			Assert.IsNull(results_1.LastSlice.Rows(0)(1))
			Assert.IsNull(results_1.LastSlice.Rows(0)(2))
			
			Assert.AreEqual("2", results_1.LastSlice.Rows(1)(0).ToString)
			Assert.IsNull(results_1.LastSlice.Rows(1)(1))
			Assert.IsNull(results_1.LastSlice.Rows(1)(2))
			
			Assert.AreEqual("3", results_1.LastSlice.Rows(2)(0).ToString)
			For i As Integer = 0 To 9
			
				Assert.AreEqual(i.ToString, results_1.LastSlice.Rows(i + 2)(1).ToString)
				Assert.AreEqual("Value_" & i.ToString, results_1.LastSlice.Rows(i + 2)(2).ToString)
				
			Next
			
		End Sub
		
		<Test()> Public Sub CheckIListFormat_3() 'Object.Array.Property
		
			Dim objList As New ArrayList()
			
			Dim obj_1 As New TestFormattableObject_2
			obj_1.Property1 = 1
			objList.Add(obj_1)
			
			Dim obj_2 As New TestFormattableObject_2
			obj_2.Property1 = 2
			objList.Add(obj_2)
			
			Dim obj_3 As New TestFormattableObject_2
			obj_3.Property1 = 3
			
			Dim obj_c_List As New ArrayList()
			
			Dim obj_c_1 As New TestFormattableObject_1()
			obj_c_1.Property1 = 1
			obj_c_List.Add(obj_c_1)
			
			Dim obj_c_2 As New TestFormattableObject_1()
			obj_c_2.Property1 = 2
			obj_c_List.Add(obj_c_2)
			
			Dim obj_c_3 As New TestFormattableObject_1()
			obj_c_3.Property1 = 3
			obj_c_List.Add(obj_c_3)
			
			obj_3.PropertyArray = obj_c_List.ToArray(GetType(TestFormattableObject_1))
			objList.Add(obj_3)
			
			Dim obj_Parent As New TestFormattableObject_3
			obj_Parent.Property1 = 10
			obj_Parent.PropertyArray_2 = objList.ToArray(GetType(TestFormattableObject_2))
			
			Dim formatter As New FormatCommands(Nothing)
			
			formatter.FieldsToOverride = FormatterProperty.Create("Property1", "PropertyArray_2.Property1", "PropertyArray_2.PropertyArray.Property1") _
				.ToArray()
				
			Dim results_1 As Cube = formatter.ProcessCommandStandard(obj_Parent, Nothing)
			
			Assert.IsNotNull(results_1)
			Assert.AreEqual(3, results_1.Columns.Count)
			Assert.AreEqual(5, results_1.LastSlice.Rows.Count)
			
			Assert.AreEqual("10", results_1.LastSlice.Rows(0)(0).ToString)
			
			For i As Integer = 1 To 3
			
				Assert.AreEqual(i.ToString, results_1.LastSlice.Rows(i - 1)(1).ToString)
				Assert.AreEqual(i.ToString, results_1.LastSlice.Rows(i + 1)(2).ToString)
				
			Next
			
		End Sub
		
	End Class
	
End Namespace

#End If