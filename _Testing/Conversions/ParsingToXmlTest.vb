#If DEBUG Then

Imports System.Configuration
Imports NUnit.Framework

Namespace Testing

	Public Class TestParsableObject

		<Xml.Serialization.XmlElementAttribute(ElementName:="AProperty1")> _
		Private m_Property1 As Integer

		<Leviathan.Configuration.ConfigurableAttribute(ElementName:="Prop3")> _
		Public Property3 As Integer

		<Leviathan.Configuration.ConfigurableAttribute("Prop4")> _
		Public Property4 As Integer

		<Xml.Serialization.XmlElementAttribute(ElementName:="AProperty2")> _
		Private m_IList1 As IList = New ArrayList

		Public Property Property1() As Integer
			Get
				Return m_Property1
			End Get
			Set(ByVal value As Integer)
				m_Property1 = value
			End Set
		End Property

		Public ReadOnly Property Property2() As IList
			Get
				Return m_IList1
			End Get
		End Property

	End Class

	Public Class TestParsableObject2

		<Xml.Serialization.XmlElementAttribute(ElementName:="AField1")> _
		Public m_Field1 As String

		<Xml.Serialization.XmlElementAttribute(ElementName:="AField2")> _
		Public m_Field2 As TestParsableObject

	End Class

	Public Class TestParsableObject3

		<Xml.Serialization.XmlElementAttribute(ElementName:="AField1")> _
		Public m_Field1 As TestParsableObject2

		<Xml.Serialization.XmlElementAttribute(ElementName:="AField2")> _
		Public m_Field2 As TestParsableObject2

		<Xml.Serialization.XmlElementAttribute(ElementName:="AField3")> _
		Public m_Field3 As Guid

	End Class

	<TestFixture()> _
	Public Class ParsingToXmlTest

		Private CONST BASE_PATH As String = "..\..\_Testing\Conversions\"
		
		<Test()> Public Sub CheckParsing1()

			Dim l_fileStream As New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_1.xml", IO.FileMode.Open)

			Dim parser_1 As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateReader(l_fileStream)

			Dim obj As TestParsedObject = parser_1.Read()

			l_fileStream.Close()
			l_fileStream = Nothing

			l_fileStream = New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_1_Temp.xml", IO.FileMode.Create)

			Dim parser_2 As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateWriter(l_fileStream)
			parser_2.OutputWriter.WriteStartDocument(True)

			parser_2.Write(obj)

			parser_2.OutputWriter.WriteEndDocument()
			parser_2.OutputWriter.Flush()


			l_fileStream.Close()
			l_fileStream = Nothing

			Dim l_xmlDoc_1 As New Xml.XmlDocument()
			l_xmlDoc_1.Load(BASE_PATH & "ParsingFromXmlTest_1.xml")

			Dim l_xmlDoc_2 As New Xml.XmlDocument()
			l_xmlDoc_2.Load(BASE_PATH & "ParsingFromXmlTest_1_Temp.xml")

			Assert.AreEqual(l_xmlDoc_1.InnerXml, l_xmlDoc_2.InnerXml)

			l_fileStream = New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_1_Temp.xml", IO.FileMode.Open)

			parser_1 = Configuration.XmlSerialiser.CreateReader(l_fileStream)

			obj = parser_1.Read()

			Assert.IsNotNull(obj)
			Assert.AreEqual(1, obj.m_Field1)
			Assert.AreEqual("Test1", obj.m_Field2)
			Assert.AreEqual(2, obj.Property1)
			Assert.AreEqual("Test2", obj.Property2)
			Assert.IsNull(obj.m_Array1)
			Assert.IsNull(obj.Property3)

			l_fileStream.Close()

			IO.File.Delete(BASE_PATH & "ParsingFromXmlTest_1_Temp.xml")

		End Sub

		<Test()> Public Sub CheckParsing2()

			Dim l_fileStream As New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_2.xml", IO.FileMode.Open)

			Dim parser_1 As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateReader(l_fileStream)

			Dim obj As TestParsedObject = parser_1.Read()

			l_fileStream.Close()
			l_fileStream = Nothing

			l_fileStream = New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_2_Temp.xml", IO.FileMode.Create)

			Dim parser_2 As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateWriter(l_fileStream)

			parser_2.OutputWriter.WriteStartDocument(True)

			parser_2.Write(obj)
			parser_2.OutputWriter.WriteEndDocument()
			parser_2.OutputWriter.Flush()
			l_fileStream.Close()
			l_fileStream = Nothing

			l_fileStream = New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_2_Temp.xml", IO.FileMode.Open)

			parser_1 = Configuration.XmlSerialiser.CreateReader(l_fileStream)

			obj = parser_1.Read()

			' Test First Part
			Assert.IsNotNull(obj)
			Assert.AreEqual(1, obj.m_Field1)
			Assert.AreEqual("Test1", obj.m_Field2)
			Assert.AreEqual(2, obj.Property1)
			Assert.AreEqual("Test2", obj.Property2)

			' Test Second Part
			Assert.IsNotNull(obj.m_Array1)
			Assert.AreEqual(1, obj.m_Array1.Length)
			Assert.IsNotNull(obj.m_Array1(0))
			Assert.AreEqual(3, obj.m_Array1(0).m_Field1)
			Assert.AreEqual("Test3", obj.m_Array1(0).m_Field2)
			Assert.AreEqual(4, obj.m_Array1(0).Property1)
			Assert.AreEqual("Test4", obj.m_Array1(0).Property2)

			' Test Third Part
			Assert.IsNotNull(obj.m_Array1(0).Property3)
			Assert.AreEqual(2, obj.m_Array1(0).Property3.Count)
			Assert.AreEqual(5, obj.m_Array1(0).Property3(0).m_Field1)
			Assert.AreEqual("Test6", obj.m_Array1(0).Property3(0).m_Field2)
			Assert.AreEqual(7, obj.m_Array1(0).Property3(1).Property1)
			Assert.AreEqual("Test8", obj.m_Array1(0).Property3(1).Property2)

			l_fileStream.Close()

			IO.File.Delete(BASE_PATH & "ParsingFromXmlTest_2_Temp.xml")

		End Sub

		<Test()> Public Sub CheckParsing3()

			Dim l_fileStream As New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_3.xml", IO.FileMode.Open)

			Dim parser_1 As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateReader(l_fileStream)

			Dim obj As TestParsedObjectCollection = parser_1.Read()

			l_fileStream.Close()
			l_fileStream = Nothing

			l_fileStream = New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_3_Temp.xml", IO.FileMode.Create)

			Dim parser_2 As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateWriter(l_fileStream)

			parser_2.OutputWriter.WriteStartDocument(True)

			parser_2.Write(obj)

			parser_2.OutputWriter.WriteEndDocument()
			parser_2.OutputWriter.Flush()

			l_fileStream.Close()
			l_fileStream = Nothing

			l_fileStream = New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_3_Temp.xml", IO.FileMode.Open)

			parser_1 = Configuration.XmlSerialiser.CreateReader(l_fileStream)

			obj = parser_1.Read()

			Assert.IsNotNull(obj)
			Assert.AreEqual("Test1", obj.Property1)

			Assert.IsNotNull(obj.Property2)
			Assert.AreEqual(1, obj.Property2.Count)
			Assert.IsNotNull(obj.Property2(0))
			Assert.AreEqual(1, obj.Property2(0).m_Field1)
			Assert.AreEqual("Test1", obj.Property2(0).m_Field2)
			Assert.AreEqual(2, obj.Property2(0).Property1)
			Assert.AreEqual("Test2", obj.Property2(0).Property2)
			Assert.IsNull(obj.Property2(0).m_Array1)
			Assert.IsNull(obj.Property2(0).Property3)

			l_fileStream.Close()

			IO.File.Delete(BASE_PATH & "ParsingFromXmlTest_3_Temp.xml")

		End Sub

		<Test()> Public Sub CheckParsing4()

			Dim l_fileStream As New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_4.xml", IO.FileMode.Open)

			Dim parser_1 As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateReader(l_fileStream)

			Dim obj As TestParsedObject2 = parser_1.Read()

			l_fileStream.Close()
			l_fileStream = Nothing

			l_fileStream = New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_4_Temp.xml", IO.FileMode.Create)

			Dim parser_2 As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateWriter(l_fileStream)

			parser_2.OutputWriter.WriteStartDocument(True)

			parser_2.Write(obj)

			parser_2.OutputWriter.WriteEndDocument()
			parser_2.OutputWriter.Flush()

			l_fileStream.Close()
			l_fileStream = Nothing

			l_fileStream = New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_4_Temp.xml", IO.FileMode.Open)

			parser_1 = Configuration.XmlSerialiser.CreateReader(l_fileStream)

			obj = parser_1.Read()

			Assert.IsNotNull(obj)
			Assert.AreEqual(DateTime.Parse("30/12/2006 11:00:01"), obj.m_Field1)

			Assert.IsNotNull(obj.m_Field2)
			Assert.AreEqual(GetType(TestParsedObject), obj.m_Field2.GetType)
			Assert.AreEqual(1, obj.m_Field2.m_Field1)
			Assert.AreEqual("Test1", obj.m_Field2.m_Field2)
			Assert.AreEqual(2, obj.m_Field2.Property1)
			Assert.AreEqual("Test2", obj.m_Field2.Property2)
			Assert.IsNull(obj.m_Field2.m_Array1)
			Assert.IsNull(obj.m_Field2.Property3)

			Assert.AreEqual(GetType(TestParsedObject), obj.m_Field3)

			l_fileStream.Close()

			IO.File.Delete(BASE_PATH & "ParsingFromXmlTest_4_Temp.xml")

		End Sub

		<Test()> Public Sub CheckParsing5()

			Dim l_fileStream As New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_5.xml", IO.FileMode.Open)

			Dim parser_1 As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateReader(l_fileStream)

			Dim obj As TestParsedObject2 = parser_1.Read()

			l_fileStream.Close()
			l_fileStream = Nothing

			l_fileStream = New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_5_Temp.xml", IO.FileMode.Create)

			Dim parser_2 As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateWriter(l_fileStream)

			parser_2.OutputWriter.WriteStartDocument(True)

			parser_2.Write(obj)

			parser_2.OutputWriter.WriteEndDocument()
			parser_2.OutputWriter.Flush()

			l_fileStream.Close()
			l_fileStream = Nothing

			l_fileStream = New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_5_Temp.xml", IO.FileMode.Open)

			parser_1 = Configuration.XmlSerialiser.CreateReader(l_fileStream)

			obj = parser_1.Read()

			Assert.IsNotNull(obj)
			Assert.AreEqual(DateTime.Parse("30/12/2006 11:00:01"), obj.m_Field1)

			Assert.IsNotNull(obj.m_Field2)
			Assert.AreEqual(GetType(TestParsedObject3), obj.m_Field2.GetType)
			Assert.AreEqual(1, obj.m_Field2.m_Field1)
			Assert.AreEqual("Test1", obj.m_Field2.m_Field2)
			Assert.AreEqual(2, obj.m_Field2.Property1)
			Assert.AreEqual("Test2", obj.m_Field2.Property2)
			Assert.IsNull(obj.m_Field2.m_Array1)
			Assert.IsNull(obj.m_Field2.Property3)

			Assert.AreEqual(GetType(TestParsedObject), obj.m_Field3)

			l_fileStream.Close()

			IO.File.Delete(BASE_PATH & "ParsingFromXmlTest_5_Temp.xml")

		End Sub

		<Test()> Public Sub CheckParsing6()

			Dim obj_1 As New TestParsableObject
			obj_1.Property1 = 10

			Dim obj_2 As New TestParsableObject2
			obj_2.m_Field1 = "Test"
			obj_2.m_Field2 = obj_1

			obj_1.Property2.Add(obj_2)

			Dim l_fileStream As New IO.FileStream(BASE_PATH & "ParsingToXmlTest_6.xml", IO.FileMode.Create)

			Dim parser_1 As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateWriter(l_fileStream)
			parser_1.OutputWriter.WriteStartDocument(True)

			parser_1.Write(obj_1)

			parser_1.OutputWriter.WriteEndDocument()
			parser_1.OutputWriter.Flush()

			l_fileStream.Close()
			l_fileStream = Nothing

			l_fileStream = New IO.FileStream(BASE_PATH & "ParsingToXmlTest_6.xml", IO.FileMode.Open)

			Dim parser_2 As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateReader(l_fileStream)

			Dim obj_3 As TestParsableObject = parser_2.Read()

			Assert.IsNotNull(obj_3)
			Assert.AreEqual(10, obj_3.Property1)

			Assert.IsNotNull(obj_3.Property2)
			Assert.AreEqual(1, obj_3.Property2.Count)
			Assert.AreEqual(GetType(TestParsableObject2), obj_3.Property2(0).GetType)
			Assert.AreEqual("Test", CType(obj_3.Property2(0), TestParsableObject2).m_Field1)
			Assert.IsNull(CType(obj_3.Property2(0), TestParsableObject2).m_Field2)

		End Sub

		<Test()> Public Sub CheckParsing7()

			Dim obj_1 As New TestParsableObject
			obj_1.Property1 = 10

			Dim obj_2 As New TestParsableObject2
			obj_2.m_Field1 = "Test"
			obj_2.m_Field2 = obj_1

			Dim obj_3 As New TestParsableObject3()
			obj_3.m_Field1 = obj_2
			obj_3.m_Field2 = obj_2

			Dim l_fileStream As New IO.FileStream(BASE_PATH & "ParsingToXmlTest_7.xml", IO.FileMode.Create)

			Dim parser_1 As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateWriter(l_fileStream)

			parser_1.OutputWriter.WriteStartDocument(True)

			parser_1.Write(obj_3)

			parser_1.OutputWriter.WriteEndDocument()
			parser_1.OutputWriter.Flush()

			l_fileStream.Close()
			l_fileStream = Nothing

			l_fileStream = New IO.FileStream(BASE_PATH & "ParsingToXmlTest_7.xml", IO.FileMode.Open)

			Dim parser_2 As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateReader(l_fileStream)

			Dim obj_4 As TestParsableObject3 = parser_2.Read()

			Assert.IsNotNull(obj_4)
			Assert.IsNotNull(obj_4.m_Field1)
			Assert.IsNotNull(obj_4.m_Field2)
			Assert.AreEqual("Test", obj_4.m_Field1.m_Field1)
			Assert.AreEqual("Test", obj_4.m_Field2.m_Field1)
			Assert.IsNotNull(obj_4.m_Field1.m_Field2)
			Assert.IsNotNull(obj_4.m_Field2.m_Field2)
			Assert.AreEqual(10, obj_4.m_Field1.m_Field2.Property1)
			Assert.AreEqual(10, obj_4.m_Field2.m_Field2.Property1)

		End Sub

		<Test()> Public Sub CheckParsing8()

			Dim g As Guid = Guid.NewGuid

			Dim obj_1 As New TestParsableObject3
			obj_1.m_Field3 = g

			Dim l_fileStream As New IO.FileStream(BASE_PATH & "ParsingToXmlTest_8.xml", IO.FileMode.Create)

			Dim parser_1 As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateWriter(l_fileStream)

			parser_1.OutputWriter.WriteStartDocument(True)

			parser_1.Write(obj_1)

			parser_1.OutputWriter.WriteEndDocument()
			parser_1.OutputWriter.Flush()

			l_fileStream.Close()
			l_fileStream = Nothing

			l_fileStream = New IO.FileStream(BASE_PATH & "ParsingToXmlTest_8.xml", IO.FileMode.Open)

			Dim parser_2 As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateReader(l_fileStream)

			Dim obj_2 As TestParsableObject3 = parser_2.Read()

			Assert.IsNotNull(obj_2)
			Assert.AreEqual(g, obj_2.m_Field3)

		End Sub

		<Test()> Public Sub CheckParsing9()

			Dim l_fileStream As New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_9_Temp.xml", IO.FileMode.Create)

			Dim parser_1 As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateWriter(l_fileStream)

			Dim obj As New TestParsableObject()
			obj.Property3 = 5
			obj.Property4 = 50

			parser_1.OutputWriter.WriteStartDocument(True)

			parser_1.Write(obj)
			parser_1.OutputWriter.WriteEndDocument()
			parser_1.OutputWriter.Flush()
			l_fileStream.Close()
			l_fileStream = Nothing

			IO.File.Delete(BASE_PATH & "ParsingFromXmlTest_9_Temp.xml")

		End Sub

	End Class

End Namespace

#End If
