#If TESTING Then

Imports System.Configuration
Imports NUnit.Framework

Namespace Testing

	#Region " Test Parsed Objects "

		Public Class TestParsedObject

			<Xml.Serialization.XmlElementAttribute(ElementName:="AField1")> _
			Public m_Field1 As Integer

			<Xml.Serialization.XmlElementAttribute(ElementName:="AField2")> _
			Public m_Field2 As String

			<Xml.Serialization.XmlElementAttribute(ElementName:="AProperty1")> _
			Private m_Property1 As Integer

			<Xml.Serialization.XmlElementAttribute(ElementName:="AProperty2")> _
			Private m_Property2 As String

			<Xml.Serialization.XmlElementAttribute(ElementName:="ArrayChildren")> _
			Public m_Array1 As TestParsedObject()

			<Xml.Serialization.XmlElementAttribute(ElementName:="ListChildren")> _
			Private m_IList1 As IList

			Public Property Property1() As Integer
				Get
					Return m_Property1
				End Get
				Set(ByVal value As Integer)
					m_Property1 = value
				End Set
			End Property

			Public Property Property2() As String
				Get
					Return m_Property2
				End Get
				Set(ByVal value As String)
					m_Property2 = value
				End Set
			End Property

			Public ReadOnly Property Property3() As IList
				Get
					Return m_IList1
				End Get
			End Property

		End Class

		Public Class TestParsedObject2

			<Xml.Serialization.XmlElementAttribute(ElementName:="AField1")> _
			Public m_Field1 As DateTime

			<Xml.Serialization.XmlElementAttribute(ElementName:="AField2")> _
			Public m_Field2 As TestParsedObject

			<Xml.Serialization.XmlElementAttribute(ElementName:="AField3")> _
			Public m_Field3 As Type

		End Class

		Public Class TestParsedObject3
			Inherits TestParsedObject
		End Class

		Public Class TestParsedObject4

			<Xml.Serialization.XmlElementAttribute(ElementName:="AField1")> _
			Public AField1 As String()

		End Class

		Public Class TestParsedObjectCollection
			Implements IList

			#Region " IList Implementation "

				Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
					Property2.CopyTo(array, index)
				End Sub

				Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
					Get
						Return Property2.Count
					End Get
				End Property

				Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
					Get
						Return Property2.IsSynchronized
					End Get
				End Property

				Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
					Get
						Return Property2.SyncRoot
					End Get
				End Property

				Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
					Return Property2.GetEnumerator
				End Function

				Public Function Add(ByVal value As Object) As Integer Implements System.Collections.IList.Add
					Return Property2.Add(value)
				End Function

				Public Sub Clear() Implements System.Collections.IList.Clear
					Property2.Clear()
				End Sub

				Public Function Contains(ByVal value As Object) As Boolean Implements System.Collections.IList.Contains
					Return Property2.Contains(value)
				End Function

				Public Function IndexOf(ByVal value As Object) As Integer Implements System.Collections.IList.IndexOf
					Return Property2.IndexOf(value)
				End Function

				Public Sub Insert(ByVal index As Integer, ByVal value As Object) Implements System.Collections.IList.Insert
					Property2.Insert(index, value)
				End Sub

				Public ReadOnly Property IsFixedSize() As Boolean Implements System.Collections.IList.IsFixedSize
					Get
						Return Property2.IsFixedSize
					End Get
				End Property

				Public ReadOnly Property IsReadOnly() As Boolean Implements System.Collections.IList.IsReadOnly
					Get
						Return Property2.IsReadOnly
					End Get
				End Property

				Default Public Property Item(ByVal index As Integer) As Object Implements System.Collections.IList.Item
					Get
						Return Property2.Item(index)
					End Get
					Set(ByVal value As Object)
						Property2.Item(index) = value
					End Set
				End Property

				Public Sub Remove(ByVal value As Object) Implements System.Collections.IList.Remove
					Property2.Remove(value)
				End Sub

				Public Sub RemoveAt(ByVal index As Integer) Implements System.Collections.IList.RemoveAt
					Property2.RemoveAt(index)
				End Sub

			#End Region

			<Xml.Serialization.XmlElementAttribute(ElementName:="AProperty1")> _
			Private m_Property1 As String

			<Xml.Serialization.XmlElementAttribute(ElementName:="ListChildren")> _
			Private m_IList1 As IList

			Public Property Property1() As String
				Get
					Return m_Property1
				End Get
				Set(ByVal value As String)
					m_Property1 = value
				End Set
			End Property

			Public ReadOnly Property Property2() As IList
				Get
					Return m_IList1
				End Get
			End Property

		End Class

	#End Region

	<TestFixture()> _
	Public Class ParsingFromXmlTest

		Private CONST BASE_PATH As String = "..\..\_Testing\Conversions\"

		<Test()> Public Sub CheckParsing1()

			Dim l_FileStream As New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_1.xml", IO.FileMode.Open, IO.FileAccess.Read)

			Dim parser As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateReader(l_FileStream)

			Dim obj As TestParsedObject = parser.Read()

			Assert.IsNotNull(obj)
			Assert.AreEqual(1, obj.m_Field1)
			Assert.AreEqual("Test1", obj.m_Field2)
			Assert.AreEqual(2, obj.Property1)
			Assert.AreEqual("Test2", obj.Property2)
			Assert.IsNull(obj.m_Array1)
			Assert.IsNull(obj.Property3)

			l_FileStream.Close()
			l_FileStream = Nothing

		End Sub

		<Test()> Public Sub CheckParsing2()

			Dim l_FileStream As New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_2.xml", IO.FileMode.Open, IO.FileAccess.Read)

			Dim parser As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateReader(l_FileStream)

			Dim obj As TestParsedObject = parser.Read()

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

			l_FileStream.Close()
			l_FileStream = Nothing

		End Sub

		<Test()> Public Sub CheckParsing3()

			Dim l_FileStream As New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_3.xml", IO.FileMode.Open, IO.FileAccess.Read)

			Dim parser As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateReader(l_FileStream)

			Dim obj As TestParsedObjectCollection = parser.Read()

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

			l_FileStream.Close()
			l_FileStream = Nothing

		End Sub

		<Test()> Public Sub CheckParsing4()

			Dim l_FileStream As New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_4.xml", IO.FileMode.Open, IO.FileAccess.Read)

			Dim parser As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateReader(l_FileStream)

			Dim obj As TestParsedObject2 = parser.Read()

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

			l_FileStream.Close()
			l_FileStream = Nothing

		End Sub

		<Test()> Public Sub CheckParsing5()

			Dim l_FileStream As New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_5.xml", IO.FileMode.Open, IO.FileAccess.Read)

			Dim parser As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateReader(l_FileStream)

			Dim obj As TestParsedObject2 = parser.Read()

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

			l_FileStream.Close()
			l_FileStream = Nothing

		End Sub

		<Test()> Public Sub CheckParsing6()

			Dim l_FileStream As New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_6.xml", IO.FileMode.Open, IO.FileAccess.Read)

			Dim parser As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateReader(l_FileStream)

			Dim obj As TestParsedObject4 = parser.Read()

			Assert.IsNotNull(obj)
			Assert.IsNotNull(obj.AField1)
			Assert.AreEqual(2, obj.AField1.Length)
			Assert.AreEqual(New String() {"Test1", "Test2"}, obj.AField1)

			l_FileStream.Close()
			l_FileStream = Nothing

		End Sub

		<Test()> Public Sub CheckParsing7()

			Dim l_FileStream As New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_7.xml", IO.FileMode.Open, IO.FileAccess.Read)

			Dim parser As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateReader(l_FileStream)

			Dim obj As TestParsedObject4 = parser.Read()

			Assert.IsNotNull(obj)
			Assert.IsNotNull(obj.AField1)
			Assert.AreEqual(2, obj.AField1.Length)
			Assert.AreEqual(New String() {"Test1", "Test2"}, obj.AField1)

			l_FileStream.Close()
			l_FileStream = Nothing

		End Sub

		<Test()> Public Sub CheckParsing8()

			Dim l_FileStream As New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_8.xml", IO.FileMode.Open, IO.FileAccess.Read)

			Dim parser As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateReader(l_FileStream)

			Dim obj As TestParsedObject2 = parser.Read()

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

			l_FileStream.Close()
			l_FileStream = Nothing

		End Sub

		<Test()> Public Sub CheckParsing9()

			Dim l_FileStream As New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_9.xml", IO.FileMode.Open, IO.FileAccess.Read)

			Dim parser As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateReader(l_FileStream)

			Dim obj As TestParsedObject = parser.Read()

			Assert.IsNotNull(obj)
			Assert.AreEqual(4, obj.m_Array1.Length)

			Assert.AreEqual(1, obj.m_Array1(0).m_Field1)
			Assert.AreEqual(2, obj.m_Array1(1).m_Field1)
			Assert.AreEqual(3, obj.m_Array1(2).m_Field1)
			Assert.AreEqual(4, obj.m_Array1(3).m_Field1)

			l_FileStream.Close()
			l_FileStream = Nothing

		End Sub

		<Test()> Public Sub CheckParsing10()

			Dim l_FileStream As New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_10.xml", IO.FileMode.Open, IO.FileAccess.Read)

			Dim parser As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateReader(l_FileStream)

			Dim obj As TestParsedObject() = parser.Read()

			Assert.IsNotNull(obj)
			Assert.AreEqual(1, obj.Length)

			Assert.AreEqual(1, obj(0).m_Field1)

			l_FileStream.Close()
			l_FileStream = Nothing

		End Sub

		<Test()> Public Sub CheckParsing11()

			Dim l_FileStream As New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_11.xml", IO.FileMode.Open, IO.FileAccess.Read)

			Dim parser As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateReader(l_FileStream)

			Dim obj As TestParsedObject() = parser.Read()

			Assert.IsNotNull(obj)
			Assert.AreEqual(4, obj.Length)

			Assert.AreEqual(1, obj(0).m_Field1)
			Assert.AreEqual(2, obj(1).m_Field1)
			Assert.AreEqual(3, obj(2).m_Field1)
			Assert.AreEqual(4, obj(3).m_Field1)

			l_FileStream.Close()
			l_FileStream = Nothing

		End Sub

		<Test()> Public Sub CheckParsing12()

			Dim l_FileStream As New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_12.xml", IO.FileMode.Open, IO.FileAccess.Read)

			Dim parser As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateReader(l_FileStream)

			Dim obj As TestParsedObject = parser.Read()

			Assert.IsNotNull(obj)
			Assert.AreEqual(1, obj.m_Field1)

			l_FileStream.Close()
			l_FileStream = Nothing

		End Sub

		<Test()> Public Sub CheckParsing13()

			Dim l_FileStream As New IO.FileStream(BASE_PATH & "ParsingFromXmlTest_13.xml", IO.FileMode.Open, IO.FileAccess.Read)
			Dim parser As Configuration.XmlSerialiser = Configuration.XmlSerialiser.CreateReader(l_FileStream)

			Dim obj As TestParsedObject = parser.Read()

			' Test First Part
			Assert.IsNotNull(obj)
			Assert.AreEqual(1, obj.m_Field1)

			' Test Second Part
			Assert.IsNotNull(obj.m_Array1)
			Assert.AreEqual(2, obj.m_Array1.Length)

			Assert.IsNotNull(obj.m_Array1(0))
			Assert.AreEqual(2, obj.m_Array1(0).m_Field1)

			Assert.IsNotNull(obj.m_Array1(1))
			Assert.AreEqual(3, obj.m_Array1(1).m_Field1)

			l_FileStream.Close()
			l_FileStream = Nothing

		End Sub

	End Class

End Namespace

#End If
