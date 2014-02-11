Imports Leviathan.Commands
Imports Leviathan.Configuration.TypeConvertor
Imports Leviathan.Inspection.AnalyserQuery
Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Xml.Schema
Imports System.Xml.Serialization

Namespace Configuration

	Public Class XmlSerialiser

		#Region " Public Constants "

			Public Const ATTRIBUTE_ITERATE_FORMAT As String = "iterate-format"

			Public Const ATTRIBUTE_LOAD_FROM As String = "load-from"

		#End Region

		#Region " Public Methods "

			''' <summary>
			''' Public Method that Parses the Object and Creates the Xml.
			''' </summary>
			''' <remarks></remarks>
			Public Sub Write( _
				ByRef value As Object, _
				Optional ByVal previouslyParsed As ArrayList = Nothing _
			)

				Dim isRootObject As Boolean = False

				If previouslyParsed Is Nothing Then
					isRootObject = True
					previouslyParsed = New ArrayList()
				End If

				If Not value Is Nothing AndAlso Not previouslyParsed.Contains(value) Then

					previouslyParsed.Add(value)

					Dim valueType As TypeAnalyser = TypeAnalyser.GetInstance(value.GetType)

					If valueType.IsValue Then

						OutputWriter.WriteValue(value.ToString)

					ElseIf valueType.Type Is GetType(String) Then

						OutputWriter.WriteValue(value)

					ElseIf valueType.IsType Then

						OutputWriter.WriteValue(CType(value, Type).FullName)

					ElseIf Not isRootObject AndAlso valueType.IsComplexArray Then

						OutputWriter.WriteStartElement(valueType.ElementType.FullName & HYPHEN)

						For i As Integer = 0 To CType(value, Array).Length - 1

							Write(CType(value, Array)(i), previouslyParsed.Clone)

						Next

						OutputWriter.WriteEndElement()

					ElseIf Not isRootObject AndAlso valueType.IsSimpleArray Then

						OutputWriter.WriteValue(StringCommands.ObjectToSingleString(value, SEMI_COLON))

					ElseIf Not isRootObject AndAlso valueType.IsIList Then

						OutputWriter.WriteStartElement(valueType.FullName)

						For i As Integer = 0 To CType(value, IList).Count - 1

							Write(CType(value, IList)(i), previouslyParsed.Clone)

						Next

						OutputWriter.WriteEndElement()

					Else

						Dim l_fields As MemberAnalyser() = _
							valueType.ExecuteQuery(QUERY_MEMBERS_READABLE _
								.SetPresentAttribute(GetType(XmlElementAttribute)))

						If Not l_fields Is Nothing Then

							OutputWriter.WriteStartElement(valueType.FullName)

							For Each l_field As MemberAnalyser In l_fields

								Dim field_Value As Object = l_field.Read(value)

								If Not field_Value Is Nothing Then

									Dim element As XmlElementAttribute = _
										l_field.Member.GetCustomAttributes( _
											GetType(XmlElementAttribute), False)(0)

									OutputWriter.WriteStartElement(element.ElementName)

									Write(field_Value, previouslyParsed.Clone)

									OutputWriter.WriteEndElement()

								End If

							Next

							OutputWriter.WriteEndElement()

						Else

							OutputWriter.WriteValue(value.ToString())

						End If

					End If

				End If

				OutputWriter.Flush()

			End Sub

			Public Function Read( _
				Optional ByRef current_Value As Object = Nothing, _
				Optional ByVal ignore_Members As MemberAnalyser() = Nothing _
			) As Object

				' -- Get to the Root Element our current position.
				If InputReader.MoveToContent() = XmlNodeType.Element Then

				' -- Parse the Type Name from the Element Name.
				Dim parsed_Type As Boolean = False

				Dim current_Type As Type = Parser.Parse(InputReader.Name.Replace( _
					HYPHEN, SQUARE_BRACKET_START & SQUARE_BRACKET_END), parsed_Type, GetType(System.Type))

				If Not parsed_Type Then Throw New Exception(String.Format(EXCEPTION_SERIALISER_WRONGTYPENAME, _
					InputReader.NodeType.ToString, InputReader.Name, InputReader.Depth, InputReader.LineNumber))

				Dim current_Analyser As TypeAnalyser

				If current_Value Is Nothing Then

					' -- Create the current_Value object from the Parsed Type
					current_Analyser = TypeAnalyser.GetInstance(current_Type)
					current_Value = current_Analyser.Create()
					If current_Value Is Nothing Then Throw New Exception(String.Format(EXCEPTION_SERIALISER_TYPECREATION, _
						InputReader.NodeType.ToString, InputReader.Name, InputReader.Depth, InputReader.LineNumber))

				ElseIf Not TypeAnalyser.IsSubClassOf(current_Value.GetType, current_Type) Then

					' -- Check whether the supplied object is compatible with the Parsed Type
					Throw New Exception(String.Format(EXCEPTION_SERIALISER_INCOMPATIBLETYPE, _
						InputReader.NodeType.ToString, InputReader.Name, InputReader.Depth))

				Else

					' -- Get the Analyser for the Type (the bottom of the inheritance model)
					current_Analyser = TypeAnalyser.GetInstance(current_Value.GetType)

				End If

				If Not InputReader.IsEmptyElement Then

					Dim start_Name As String = InputReader.Name
					Dim start_Depth As Integer = InputReader.Depth

					' -- If we get here, then we're fine, we have a type and an object.
					Dim current_Fields As MemberAnalyser() = current_Analyser.ExecuteQuery( _
						AnalyserQuery.QUERY_MEMBERS_WRITEABLE.SetPresentAttribute(GetType(XmlElementAttribute)))

					While InputReader.Read

						If InputReader.NodeType = XmlNodeType.Element Then

							If current_Fields Is Nothing AndAlso current_Analyser.IsArray

								' -- If we're an Array, resize ourselves and append the new value to the end.
								Dim parsed_ArrayValue As Object = Read()
								If Not parsed_ArrayValue Is Nothing Then
									Array.Resize(current_Value, CType(current_Value, Array).Length + 1)
									CType(current_Value, Array)(CType(current_Value, Array).Length - 1) = parsed_ArrayValue
								End If

							ElseIf current_Fields Is Nothing AndAlso current_Analyser.IsIList

								' -- If we're an IList then add the new value.
								Dim parsed_ListValue As Object = Read()
								If Not parsed_ListValue Is Nothing Then CType(current_Value, IList).Add(parsed_ListValue)

							ElseIf Not current_Fields Is Nothing Then

								' -- Have to assume this is a Field, so we need to find it!
									Dim current_Field As MemberAnalyser = Nothing

									For i As Integer = 0 To current_Fields.Length - 1

										If String.Compare(current_Fields(i).Member.GetCustomAttributes( _
											GetType(XmlElementAttribute), False)(0).ElementName, InputReader.Name, True) = 0 Then

											current_Field = current_Fields(i)
											Exit For

										End If

									Next

									If current_Field Is Nothing Then Throw New Exception(String.Format(EXCEPTION_SERIALISER_NOFIELD, _
										InputReader.NodeType.ToString, InputReader.Name, InputReader.Depth, InputReader.LineNumber))

									If Not ignore_Members Is Nothing Then

										For i As Integer = 0 To ignore_Members.Length - 1

											If String.Compare(current_Field.FullName, ignore_Members(i).FullName, True) = 0 Then

												' -- We're not going to parse this field as we should ignore it!
												current_Field = Nothing
												InputReader.ReadSubtree()
												Exit For

											End If

										Next

									End If

									If Not current_Field Is Nothing Then

										If InputReader.HasAttributes ' -- This requires attribute-specific parsing.

											Dim value_List As New ArrayList()

											If Not String.IsNullOrEmpty(InputReader.GetAttribute(ATTRIBUTE_ITERATE_FORMAT)) Then

												Dim parsed_Iteration As Boolean

												Dim iteration As Array = CreateArray(Parser.Parse(InputReader.GetAttribute(ATTRIBUTE_ITERATE_FORMAT), _
													parsed_Iteration, GetType(Object)))

												If Not parsed_Iteration Then Throw New Exception(String.Format(EXCEPTION_SERIALISER_ITERATEFORMAT, _
													InputReader.NodeType.ToString, InputReader.Name, InputReader.Depth, InputReader.LineNumber))

												Dim iteration_Node As String = InputReader.ReadOuterXml

												For i As Integer = 0 To iteration.Length - 1

													Dim iteration_Reader As New XmlTextReader(iteration_Node, XmlNodeType.Element, _
														New XmlParserContext(Nothing, New XmlNamespaceManager(InputReader.NameTable), Nothing, XmlSpace.None))

													value_List.AddRange(CreateArray(New XmlSerialiser(iteration_Reader, Nothing, _
														Parser, iteration(i)).Read()))

													iteration_Reader.Close()

												Next

											ElseIf Not String.IsNullOrEmpty(InputReader.GetAttribute(ATTRIBUTE_LOAD_FROM)) Then

												Dim parsed_Streams As Boolean

												Dim streams As Stream() = Parser.Parse(InputReader.GetAttribute(ATTRIBUTE_LOAD_FROM), _
													parsed_Streams, GetType(IO.Stream).MakeArrayType)

												If Not streams Is Nothing Then

													For i As Integer = 0 To streams.Length - 1

														Try
															Dim stream_Reader As New XmlTextReader(streams(i))

															value_List.AddRange(CreateArray(New XmlSerialiser(stream_Reader, Nothing, Parser).Read()))

															stream_Reader.Close()
														Catch ex As Exception
															Throw New Exception(String.Format(EXCEPTION_SERIALISER_LOADFROM, _
																InputReader.NodeType.ToString, InputReader.Name, InputReader.Depth, InputReader.LineNumber), ex)
														End Try

													Next

												End If

											End If

											If current_Field.ReturnTypeAnalyser.IsArray Then

												current_Field.Write(current_Value, TypeAnalyser.ConvertToArray( _
													value_list, current_Field.ReturnTypeAnalyser.ElementType))

											ElseIf current_Field.ReturnTypeAnalyser.IsIList Then

												current_Field.Write(current_Value, value_List)

											ElseIf current_Field.ReturnTypeAnalyser.IsScalar AndAlso value_List.Count = 1

												current_Field.Write(current_Value, value_List(0))

											End If

										Else

											InputReader.Read() ' Read because we're currently on the Field Name Element, not the next Type Name!

											If InputReader.NodeType = XmlNodeType.Text Then ' We can use the String Parser to Populate it.

												If Not String.IsNullOrEmpty(InputReader.Value) Then

													Dim parsed_Value As Boolean = False
													Dim field_Value As Object = Parser.Parse(InputReader.Value, parsed_Value, current_Field.ReturnType)
													If parsed_Value Then current_Field.Write(current_Value, field_Value)

												End If

											Else ' -- This is a Complex Type.

												Dim field_Value As Object = Read(current_Field.Read(current_Value))
												If Not field_Value Is Nothing Then current_Field.Write(current_Value, field_Value)

											End If

										End If

									End If

								Else

									Throw New Exception(String.Format(EXCEPTION_SERIALISER_NONCONFIGURABLETYPE, _
										InputReader.NodeType.ToString, InputReader.Name, InputReader.Depth, InputReader.LineNumber))

								End If

							End If

							' -- Exits Look if we're back to the same level/element as we started on.
							If InputReader.NodeType = XmlNodeType.EndElement AndAlso _
								InputReader.Depth = start_Depth AndAlso InputReader.Name = start_Name Then Exit While

						End While

						' -- Progressing to the next Node for recursive calling.
						InputReader.Read()

					End If

				End If

				Return current_Value

			End Function

		#End Region

		#Region " Public Shared Methods "

			Public Shared Function CreateReader( _
				ByVal inputFile As String _
			) As XmlSerialiser

				If File.Exists(inputFile) Then

					Return CreateReader( _
						New FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))

				Else

					Throw New ArgumentException()

				End If

			End Function

			Public Shared Function CreateReader( _
				ByVal inputStream As Stream _
			) As XmlSerialiser

				If inputStream.CanRead Then

					Return CreateReader(New XmlTextReader(inputStream))

				Else

					Throw New ArgumentException()

				End If

			End Function

			Public Shared Function CreateReader( _
				ByVal inputReader As XmlReader _
			) As XmlSerialiser

				Return New XmlSerialiser(inputReader, Nothing, New FromString())

			End Function

			Public Shared Function CreateWriter( _
				ByVal outputFile As String _
			) As XmlSerialiser

				If Not File.Exists(outputFile) Then

					Return CreateWriter( _
						New FileStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None))

				Else

					Throw New ArgumentException()

				End If

			End Function

			Public Shared Function CreateWriter( _
				ByVal outputStream As Stream _
			) As XmlSerialiser

				If outputStream.CanWrite Then

					Dim writer As New XmlTextWriter(outputStream, Encoding.UTF8)
					writer.Formatting = Formatting.Indented
					writer.Indentation = 2

					Return CreateWriter(writer)

				Else

					Throw New ArgumentException()

				End If

			End Function

			Public Shared Function CreateWriter( _
				ByVal outputWriter As XmlWriter _
			) As XmlSerialiser

				Return New XmlSerialiser(Nothing, outputWriter, New FromString())

			End Function

			Public Shared Function GenerateSchema( _
				ByVal objType As Type _
			) As XmlSchema

				' TODO: Write Schema Generation Function
				Return Nothing

			End Function

			Public Shared Sub ReadXml( _
				ByRef obj As Object, _
				ByVal reader As XmlReader, _
				Optional ByVal belowObject As Type = Nothing _
			)

				Dim parsed_object As Object = CreateReader(reader).Read()

				If Not parsed_object Is Nothing Then

					TypeAnalyser.Integrate(obj, AnalyserQuery.QUERY_VARIABLES _
						.SetDeclaredBelowType(belowObject), parsed_object)

				End If

			End Sub

			Public Shared Sub WriteXml( _
				ByRef obj As Object, _
				ByVal writer As XmlWriter _
			)

				CreateWriter(writer).Write(obj)

			End Sub

		#End Region

	End Class

End Namespace
