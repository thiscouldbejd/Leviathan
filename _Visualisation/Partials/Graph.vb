Imports System.Collections.Generic
Imports System.IO

Namespace Visualisation

	Partial Public Class Graph(Of T)

		#Region " Protected Structures "

			Protected Structure Edge

				Public [From] As System.Int32
				Public [To] As System.Int32

				Public Sub New( _
					ByVal _from As System.Int32, _
					ByVal _to As System.Int32 _
				)

					[from] = _from
					[to] = _to

				End Sub

			End Structure

		#End Region

		#Region " Protected Variables "

			' List of Nodes Objects (Id'd by Index)
			Protected Nodes As New List(Of T)

			' List of Node Edges (Id'd By Index)
			Protected Edges As New List(Of Edge)

			' List of Value Objects (Id'd by Index)
			Protected Values As New List(Of System.Object)

			' List of Attribute Names (Id'd by Index)
			Protected Attribute_Names As New List(Of System.String)

			' List of Attribute Types (Id'd by Index)
			Protected Attribute_Types As New List(Of System.Type)

			' List Of Node Attributes (Indexed by Node Id)
			Protected Node_Attributes As New Dictionary(Of System.Int32, Dictionary(Of System.Int32, System.Int32))

			' List of Attribute Nodes (Indexed by Attribute Id)
			Protected Attribute_Nodes As New Dictionary(Of System.Int32, Dictionary(Of System.Int32, System.Int32))

			' List Of Node Attributes (Indexed by Edge Id)
			Protected Edge_Attributes As New Dictionary(Of System.Int32, Dictionary(Of System.Int32, System.Int32))

			' List of Attribute Edges (Indexed by Attribute Id)
			Protected Attribute_Edges As New Dictionary(Of System.Int32, Dictionary(Of System.Int32, System.Int32))

		#End Region

		#Region " Private Getting Methods "

			Private Function GetNodeFromId( _
				ByVal id As System.Int32 _
			) As System.Object

				' -- Check Input Argument Validity --
				If id < 1 Or id > Nodes.Count Then Throw New ArgumentException("id", "Node Id is not valid")

				Return Nodes(id - 1)

			End Function

			Private Function GetEdgeFromId( _
				ByVal id As System.Int32 _
			) As Edge

				' -- Check Input Argument Validity --
				If id < 1 Or id > Nodes.Count Then Throw New ArgumentException("id", "Edge Id is not valid")

				Return Edges(id - 1)

			End Function

			Private Function GetAttributeNameFromId( _
				ByVal id As System.Int32 _
			) As System.String

				' -- Check Input Argument Validity --
				If id < 1 Or id > Attribute_Types.Count Then Throw New ArgumentException("id", "Attribute Id is not valid")

				Return Attribute_Names(id - 1)

			End Function

			Private Function GetAttributeTypeFromId( _
				ByVal id As System.Int32 _
			) As System.Type

				' -- Check Input Argument Validity --
				If id < 1 Or id > Attribute_Types.Count Then Throw New ArgumentException("id", "Attribute Id is not valid")

				Return Attribute_Types(id - 1)

			End Function

			Private Function GetValueFromId( _
				ByVal id As System.Int32 _
			) As System.Object

				' -- Check Input Argument Validity --
				If id < 1 Or id > Values.Count Then Throw New ArgumentException("id", "Value Id is not valid")

				Return Values(id - 1)

			End Function

		#End Region

		#Region " Private Adding Methods "

			Public Function Add_Node( _
				ByVal value As T, _
				ByRef added As System.Boolean _
			) As System.Int32

				' -- Check Input Argument Validity --
				If value Is Nothing Then Throw New ArgumentException("value", "Node Value cannot be Null")

				Dim node_Id As Integer = Nodes.IndexOf(value)

				If node_Id < 0 Then

					Nodes.Add(value)
					node_Id = Nodes.Count
					added = True

				Else

					' ID is a one-bised (indexes are zero-based)
					node_Id += 1
					added = False

				End If

				Return node_Id

			End Function

			Public Function Add_Value( _
				ByVal value As Object, _
				ByRef added As System.Boolean _
			) As System.Int32

				' -- Check Input Argument Validity --
				If value Is Nothing Then Throw New ArgumentException("value", "Value cannot be Null")

				Dim value_Id As Integer = Values.IndexOf(value)

				If value_Id < 0 Then

					Values.Add(value)
					value_Id = Values.Count
					added = True

				Else

					' ID is a one-bised (indexes are zero-based)
					value_Id += 1
					added = False

				End If

				Return value_Id

			End Function

			Private Function Add_Attribute( _
				ByVal attribute_Name As String, _
				ByVal attribute_Type As Type, _
				ByRef added As System.Boolean _
			) As System.Int32

				' -- Check Input Argument Validity --
				If String.IsNullOrEmpty(attribute_Name) Then Throw New ArgumentException("attribute_Name", "Attribute Name cannot be Null or Empty")
				If attribute_Type Is Nothing Then Throw New ArgumentException("attribute_Type", "Attrbiute Type cannot be Null")

				Dim attribute_Id As Integer = Attribute_Names.IndexOf(attribute_Name)

				If attribute_Id < 0 Then

					Attribute_Names.Add(attribute_Name)
					Attribute_Types.Add(attribute_Type)
					attribute_Id = Attribute_Names.Count
					added = True

				Else

					' ID is a one-based (indexes are zero-based)
					attribute_Id += 1
					added = False
					If Not GetAttributeTypeFromId(attribute_Id) Is attribute_Type Then _
						Throw New Exception(String.Format("Attribute is of the wrong type, Expected: {0} but Got: {1}", _
							 GetAttributeTypeFromId(attribute_Id).Name, attribute_Type.Name))

				End If

				Return attribute_Id

			End Function

			Private Sub Add_AttributeLinks( _
				ByVal entity_Id As System.Int32, _
				ByVal attribute_Id As System.Int32, _
				ByVal value_Id As System.Int32, _
				ByRef entity_Lookups As Dictionary(Of System.Int32, Dictionary(Of System.Int32, System.Int32)), _
				ByRef attribute_Lookups As Dictionary(Of System.Int32, Dictionary(Of System.Int32, System.Int32)) _
			)

				' -- Set Up Both Sides of the Lookups --
				Dim existing_AttributeValues As Dictionary(Of System.Int32, System.Int32)
				Dim existing_EntityValues As Dictionary(Of System.Int32, System.Int32)

				If Not entity_Lookups.ContainsKey(entity_Id) Then

					existing_AttributeValues = New Dictionary(Of System.Int32, System.Int32)
					entity_Lookups.Add(entity_Id, existing_AttributeValues)

				Else

					existing_AttributeValues = entity_Lookups(entity_Id)

				End If

				If Not attribute_Lookups.ContainsKey(attribute_Id) Then

					existing_EntityValues = New Dictionary(Of System.Int32, System.Int32)
					attribute_Lookups.Add(attribute_Id, existing_EntityValues)

				Else

					existing_EntityValues = attribute_Lookups(attribute_Id)

				End If

				If existing_AttributeValues.ContainsKey(attribute_Id) Then

					existing_AttributeValues(attribute_Id) = value_Id

				Else

					existing_AttributeValues.Add(attribute_Id, value_Id)

				End If

				If existing_EntityValues.ContainsKey(entity_Id) Then

					existing_EntityValues(entity_Id) = value_Id

				Else

					existing_EntityValues.Add(entity_Id, value_Id)

				End If

			End Sub

		#End Region

		#Region " Public Node Methods "

			Public Sub Add_Node( _
				ByVal value As T _
			)

				Add_Node(value, New Boolean)

			End Sub

		#End Region

		#Region " Public Edge Methods "

			Public Sub Add_Edge( _
				ByVal [from] As T, _
				ByVal [to] As T, _
				ByVal attribute_Name As String, _
				ByVal attribute_Value As Object _
			)

				Add_Edge([from], [to], New String() {attribute_Name}, New Object() {attribute_Value})

			End Sub

			Public Sub Add_Edge( _
				ByVal [from] As T, _
				ByVal [to] As T, _
				ByVal attribute_Names As String(), _
				ByVal attribute_Values As Object() _
			)

				' -- Check Input Argument Validity --
				If [from] Is Nothing Then Throw New ArgumentException("from", "From Node cannot be Null")
				If [to] Is Nothing Then Throw New ArgumentException("to", "To Node cannot be Null")

				Dim from_Id As System.Int32 = Add_Node([from], New Boolean)
				Dim to_Id As System.Int32 = Add_Node([to], New Boolean)

				Edges.Add(New Edge(from_Id, to_Id))
				Dim edge_Id = Edges.Count()

				If Not attribute_Names Is Nothing AndAlso Not attribute_Values Is Nothing Then

					' -- Check Input Argument Validity --
					If attribute_Names.Length <> attribute_Values.Length Then Throw New ArgumentException("attribute_Names", "Attribute Names & Values Length don't Match")

					For i As Integer = 0 To attribute_Names.Length - 1

						' -- Check Input Argument Validity --
						If attribute_Values(i) Is Nothing Then Throw New ArgumentException("attribute_Values", String.Format("Attribute Value {0} cannot be Null", i))

						' -- Retrieve Ids --
						Dim attribute_Id As System.Int32 = Add_Attribute(attribute_Names(i), attribute_Values(i).GetType(), New Boolean)
						Dim value_Id As System.Int32 = Add_Value(attribute_Values(i), New Boolean)

						' -- Add Links to Both Views --
						Add_AttributeLinks(edge_Id, attribute_Id, value_Id, Edge_Attributes, Attribute_Edges)

					Next

				End If

			End Sub

			Public Sub Add_UndirectedEdge( _
				ByVal node_1 As T, _
				ByVal node_2 As T, _
				ByVal attribute_Names As String(), _
				ByVal attribute_Values As Object() _
			)

				Add_Edge(node_1, node_2, attribute_Names, attribute_Values)
				Add_Edge(node_2, node_1, attribute_Names, attribute_Values)

			End Sub

		#End Region

		#Region " Public Attribute Methods "

			Public Sub Add_Attribute( _
				ByVal node_Value As T, _
				ByVal attribute_Name As String, _
				ByVal attribute_Value As Object _
			)

				' -- Check Input Argument Validity --
				If attribute_Value Is Nothing Then Throw New ArgumentException("attribute_Value", "Attrbiute Value cannot be Null")

				' -- Retrieve Ids --
				Dim node_Id As System.Int32 = Add_Node(node_Value, New Boolean)
				Dim attribute_Id As System.Int32 = Add_Attribute(attribute_Name, attribute_Value.GetType(), New Boolean)
				Dim value_Id As System.Int32 = Add_Value(attribute_Value, New Boolean)

				' -- Add Links to Both Views --
				Add_AttributeLinks(node_Id, attribute_Id, value_Id, Node_Attributes, Attribute_Nodes)

			End Sub

		#End Region

		#Region " GML Output Methods "

			Private Sub WriteGMLAttributes( _
				ByRef writer As StreamWriter, _
				ByRef attributes As Dictionary(Of System.Int32, System.Int32) _
			)

				If Not attributes Is Nothing Then

					For Each attribute_Id As System.Int32 In attributes.Keys

						If GetAttributeTypeFromId(attribute_Id) Is GetType(String) Then

							writer.WriteLine(vbTab & vbTab & String.Format("{0} ""{1}""", _
								GetAttributeNameFromId(attribute_Id), GetValueFromId(attributes(attribute_Id))))

						Else

							writer.WriteLine(vbTab & vbTab & String.Format("{0} {1}", _
								GetAttributeNameFromId(attribute_Id), GetValueFromId(attributes(attribute_Id)).ToString()))

						End If

					Next

				End If

			End Sub

			Public Sub ToGML( _
				ByRef writer As StreamWriter _
			)

				' -- Start Graph and Write Meta-Data --
				writer.WriteLine("graph [")
				writer.WriteLine(vbTab & String.Format("comment ""{0}""", Name))
				writer.WriteLine(vbTab & "directed 1")
				writer.WriteLine(vbTab & "version 2")

				For i As Integer = 0 To Nodes.Count - 1

					' -- Write Each Node --
					writer.WriteLine(vbTab & "node [")

					' -- Write Details --
					writer.WriteLine(vbTab & vbTab & "id " & (i + 1).ToString)

					' -- Write the Relevant Attributes --
					WriteGMLAttributes(writer, Node_Attributes(i + 1))

					' -- End Node --
					writer.WriteLine(vbTab & "]")

				Next

				For i As Integer = 0 To Edges.Count - 1

					' -- Write Each Edge --
					writer.WriteLine(vbTab & "edge [")

					' -- Write Details --
					writer.WriteLine(vbTab & vbTab & "source " & Edges(i).[From].ToString)
					writer.WriteLine(vbTab & vbTab & "target " & Edges(i).[To].ToString)
					writer.WriteLine(vbTab & vbTab & "id " & (i + 1).ToString)

					' -- Write the Relevant Attributes --
					WriteGMLAttributes(writer, Edge_Attributes(i + 1))

					' -- End Edge --
					writer.WriteLine(vbTab & "]")

				Next

				' -- End Graph --
				writer.WriteLine("]")

			End Sub

		#End Region

		#Region " TLP Output Methods "

			Private Sub WriteTLPAttributes( _
				ByRef writer As StreamWriter, _
				ByRef attributes As Dictionary(Of System.Int32, System.Int32), _
				ByVal prefix As String, _
				ByVal quote_Value As Boolean _
			)

				For Each entity_Id As System.Int32 In attributes.Keys

					If quote_Value Then

						writer.WriteLine(vbTab & vbTab & String.Format("({0} {1} ""{2}"")", prefix, entity_Id, GetValueFromId(attributes(entity_Id))))

					Else

						writer.WriteLine(vbTab & vbTab & String.Format("({0} {1} {2})", prefix, entity_Id, GetValueFromId(attributes(entity_Id))))

					End If

				Next

			End Sub

			Public Sub ToTLP( _
				ByRef writer As StreamWriter _
			)

				' -- Start Graph and Write Meta-Data --
				writer.WriteLine("(tlp ""2.0""")

				writer.WriteLine(vbTab & String.Format("(date ""{0}"")", Created.ToString("dd-mm-yyyy")))
				writer.WriteLine(vbTab & String.Format("(author ""{0}"")", CreatedBy))
				writer.WriteLine(vbTab & String.Format("(comments ""{0}"")", Name))

				' -- Write All Nodes --
				writer.Write(vbTab & "(nodes")

				For i As Integer = 0 To Nodes.Count - 1

					' -- Write Each Node --
					writer.Write(String.Format(" {0}", i + 1))

				Next

				' -- End Nodes --
					writer.WriteLine(")")

				For i As Integer = 0 To Edges.Count - 1

					' -- Write Each Edge --
					writer.WriteLine(vbTab & String.Format("(edge {0} {1} {2})", i + 1, Edges(i).[From], Edges(i).[To]))

				Next

				For i As Integer = 0 to Attribute_Names.Count - 1

					Dim type_Name As String
					Dim attribute_Type As System.Type = Attribute_Types(i)

					If attribute_Type Is GetType(System.Byte) OrElse attribute_Type Is GetType(System.Int16) OrElse attribute_Type Is GetType(System.Int32) OrElse attribute_Type Is GetType(System.Int64) Then

						type_Name = "int"

					ElseIf attribute_Type Is GetType(System.Single) OrElse attribute_Type Is GetType(System.Double) Then

						type_Name = "double"

					ElseIf attribute_Type Is GetType(System.Boolean) Then

						type_Name = "bool"

					Else

						type_Name = "string"

					End If

					' -- Write Property/Attribute Meta-Data --
					writer.WriteLine(vbTab &  String.Format("(property 0 {0} ""{1}""", type_Name, Attribute_Names(i)))

					If attribute_Nodes.ContainsKey(i + 1) Then _
						WriteTLPAttributes(writer, attribute_Nodes(i + 1), "node", type_Name = "string")

					If attribute_Edges.ContainsKey(i + 1) Then _
						WriteTLPAttributes(writer, attribute_Edges(i + 1), "edge", type_Name = "string")

					' -- Write End Property
					writer.WriteLine(vbTab & ")")

				Next

				' -- End Graph --
				writer.WriteLine(")")

			End Sub

		#End Region

	End Class

End Namespace
