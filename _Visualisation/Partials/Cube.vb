Namespace Visualisation

	Partial Public Class Cube
		Implements IFixedWidthWriteable

		#Region " Public Properties "

			Public ReadOnly Property HasColumns() As Boolean
				Get
					Return Not Columns Is Nothing AndAlso Columns.Count > 0
				End Get
			End Property

			Public ReadOnly Property HasData() As Boolean
				Get
					Return Not Slices Is Nothing AndAlso Slices.Count > 0
				End Get
			End Property

			Public ReadOnly Property LastSlice As Slice
				Get
					Return Slices(Slices.Count - 1)
				End Get
			End Property

		#End Region

		#Region " Private Methods "

			Private Function GetColumnWidths( _
				ByVal columns As List(Of FormatterProperty), _
				ByVal rows As List(Of Row), _
				ByRef idealWidth As Integer, _
				Optional ByVal rowWidth As Integer = 0 _
			) As Integer()

				If columns.Count > 0 Then

					If ColumnLengths Is Nothing Then

						Dim l_maxColumnWidths(columns.Count - 1) As Integer

						For i As Integer = 0 To columns.Count - 1

							If Not columns(i) Is Nothing Then l_maxColumnWidths(i) = columns(i).DisplayNameLength

						Next

						For i As Integer = 0 To rows.Count - 1

							If rows(i).Cells.Count > 0 Then

								For j As Integer = 0 To columns.Count - 1

									If rows(i).Cells.Count >= (j - 1) AndAlso Not rows(i).Cells(j) Is Nothing Then l_maxColumnWidths(j) = _
										Math.Max(rows(i).Cells(j).StringValueLength, l_maxColumnWidths(j))

								Next

							End If

						Next

						Dim l_totalColumnWidth As Integer

						For i As Integer = 0 To l_maxColumnWidths.Length - 1

							l_totalColumnWidth += l_maxColumnWidths(i)

						Next

						TotalColumnLength = l_totalColumnWidth

						ColumnLengths = l_maxColumnWidths

					End If

					idealWidth = TotalColumnLength

					If rowWidth <= columns.Count Then

						Dim l_maxColumnWidths(columns.Count - 1) As Integer

						For i As Integer = 0 To columns.Count - 1

							l_maxColumnWidths(i) = 1

						Next

						Return l_maxColumnWidths

					Else

						Dim totalColumnWidth = TotalColumnLength
						Dim l_maxColumnWidths(ColumnLengths.Length - 1) As Integer
						Array.Copy(ColumnLengths, l_maxColumnWidths, ColumnLengths.Length)

						Do Until totalColumnWidth <= rowWidth

							Dim l_minWidth As Integer = Integer.MaxValue
							Dim l_maxWidth As Integer = Integer.MinValue

							For i As Integer = 0 To l_maxColumnWidths.Length - 1

								l_minWidth = Math.Min(l_maxColumnWidths(i), l_minWidth)
								l_maxWidth = Math.Max(l_maxColumnWidths(i), l_maxWidth)

							Next

							If l_minWidth * l_maxColumnWidths.Length > rowWidth Then l_minWidth = Math.Floor(rowWidth / l_maxColumnWidths.Length)

							For i As Integer = 0 To l_maxColumnWidths.Length - 1

								If l_maxColumnWidths(i) = l_maxWidth Then

									If (totalColumnWidth - (l_maxWidth - l_minWidth)) < rowWidth Then

										l_maxColumnWidths(i) -= (totalColumnWidth - rowWidth)

									Else

										l_maxColumnWidths(i) = l_minWidth

									End If

									Exit For

								End If

							Next

							totalColumnWidth = 0

							For i As Integer = 0 To l_maxColumnWidths.Length - 1

								totalColumnWidth += l_maxColumnWidths(i)

							Next

						Loop

						Return l_maxColumnWidths

					End If

				Else

					Return New Integer() {}

				End If

			End Function

		#End Region

		#Region " Public Methods "

			Public Function Add( _
				value As Slice _
			) As Cube

				Slices.Add(value)

				Return Me

			End Function

			Public Function CreateAdoXml() As String

				Dim memoryStream As New System.IO.MemoryStream()
				Dim textStream As New System.Xml.XmlTextWriter(memoryStream, System.Text.Encoding.ASCII)

				' Start the Root Element
				textStream.WriteStartElement("xml")
				textStream.WriteAttributeString("xmlns", "s", Nothing, "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882")
				textStream.WriteAttributeString("xmlns", "dt", Nothing, "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882")
				textStream.WriteAttributeString("xmlns", "rs", Nothing, "urn:schemas-microsoft-com:rowset")
				textStream.WriteAttributeString("xmlns", "z", Nothing, "#RowsetSchema")

				' Start the Schema Element
				textStream.WriteStartElement("Schema", "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882")
				textStream.WriteAttributeString("id", "RowsetSchema")

				' Start the ElementType Element
				textStream.WriteStartElement("ElementType", "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882")
				textStream.WriteAttributeString("name", "row")
				textStream.WriteAttributeString("content", "eltOnly")
				textStream.WriteAttributeString("updatable", "urn:schemas-microsoft-com:rowset", "true")

				For i As Integer = 0 To Columns.Count - 1

					' Start the AttributeType Element
					textStream.WriteStartElement("AttributeType", "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882")
					textStream.WriteAttributeString("name", "a" & (i + 1).ToString)
					textStream.WriteAttributeString("name", "urn:schemas-microsoft-com:rowset", Columns(i).DisplayName)
					textStream.WriteAttributeString("number", "urn:schemas-microsoft-com:rowset", (i + 1).ToString)
					textStream.WriteAttributeString("nullable", "urn:schemas-microsoft-com:rowset", "true")
					textStream.WriteAttributeString("maydefer", "urn:schemas-microsoft-com:rowset", "true")
					textStream.WriteAttributeString("write", "urn:schemas-microsoft-com:rowset", "true")

					' Start the DataType Element
					textStream.WriteStartElement("datatype", "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882")
					textStream.WriteAttributeString("type", "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882", "string")
					textStream.WriteAttributeString("maxLength", "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882", "255")
					textStream.WriteAttributeString("precision", "urn:schemas-microsoft-com:rowset", "0")

					' End the DataType Element
					textStream.WriteEndElement()

					' End the AttributeType Element
					textStream.WriteEndElement()

				Next

				' Start the Extends Element
				textStream.WriteStartElement("extends", "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882")
				textStream.WriteAttributeString("type", "rs:rowbase")

				' End the Extends Element
				textStream.WriteEndElement()

				' End the ElementType Element
				textStream.WriteEndElement()

				' End the Schema Element
				textStream.WriteEndElement()

				' Start the Data Element
				textStream.WriteStartElement("data", "urn:schemas-microsoft-com:rowset")

				For i As Integer = 0 To LastSlice.Rows.Count - 1

					' Start the Row Element
					textStream.WriteStartElement("row", "#RowsetSchema")

					For k As Integer = 0 To CType(LastSlice.Rows(i), IList).Count - 1

						If Not String.IsNullOrEmpty(CType(LastSlice.Rows(i), IList)(k)) Then

							textStream.WriteAttributeString("a" & (k + 1).ToString, CType(LastSlice.Rows(i), IList)(k))

						End If

					Next

					' End the Row Element
					textStream.WriteEndElement()

				Next

				' End the Data Element
				textStream.WriteEndElement()

				' End the Root Element
				textStream.WriteEndElement()

				' Finish and Return
				textStream.Flush()

				Dim strXml As String
				memoryStream.Position = 0
				Dim reader As New System.IO.StreamReader(memoryStream, System.Text.Encoding.ASCII)
				strXml = reader.ReadToEnd()
				reader.Close()
				textStream.Close()

				Return strXml

			End Function

		#End Region

		#Region " IFixedWidthWriteable Implementation "

			Private Function FixedWidthWrite( _
				ByVal intoWidth As Integer, _
				ByRef writer As ICommandsOutputWriter, _
				ByRef idealWidth As Integer _
			) As ICommandsOutputWriter _
			Implements IFixedWidthWriteable.Write

				If Not Title Is Nothing Then

					CType(Title, IFixedWidthWriteable).Write(intoWidth, writer, idealWidth).TerminateLine()

					If HasColumns Then

						Dim columnIdealWidth As Integer

						' This is the widths of each column, given the max width of the line we can use
						Dim l_ColumnWidths As Integer() = GetColumnWidths(Columns, LastSlice.Rows, columnIdealWidth, intoWidth - (Columns.Count - 1))

						idealWidth = Math.Max(idealWidth, columnIdealWidth + (Columns.Count - 1))

						Dim columnRow As New Row()
						Dim spacerRow As New Row()

						For i As Integer = 0 To Columns.Count - 1

							columnRow.Add(New Cell(Columns(i).ToString, 0, Title.Type))
							spacerRow.Add(New Cell(New String(HYPHEN, l_ColumnWidths(i)), 0, Title.Type))

						Next

						columnRow.Write(l_ColumnWidths, writer)
						spacerRow.Write(l_ColumnWidths, writer)

						If HasData AndAlso LastSlice.HasData Then

							For i As Integer = 0 To LastSlice.Rows.Count - 1

								If Not LastSlice.Rows(i) Is Nothing Then LastSlice.Rows(i).Write(l_ColumnWidths, writer)

							Next

						End If

					End If

				End If

				Return writer

			End Function

		#End Region

		#Region " Public Shared Methods "

			Public Shared Function Create( _
				ByVal type As InformationType, _
				ByVal title As String, _
				ByVal ParamArray columns As String() _
			) As Cube

				Return Cube.Create(type, title, FormatterProperty.Create(columns))

			End Function

			Public Shared Function Create( _
				ByVal type As InformationType, _
				ByVal title As String, _
				ByVal columns As List(Of FormatterProperty) _
			) As Cube

				Return New Cube(Message.Create(title, type), columns)

			End Function

		#End Region

	End Class

End Namespace
