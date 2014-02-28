Imports Leviathan.Commands.ObjectsCommands
Imports Leviathan.Commands.StringCommands
Imports Leviathan.Visualisation

Namespace Commands

	Partial Public Class FormatCommands

		#Region " Private Constants "

			Private Const METHOD_TOSTRING As String = "ToString"

			Private Const PIVOT_PRESENT As String = LETTER_X

			Private Const PIVOT_BACKGROUNDPRESENT As String = FULL_STOP

		#End Region

		#Region " Private Methods "

			''' <summary>
			''' Method to get(
			''' </summary>
			''' <param name="rows">The Rows to Get From.</param>
			''' <param name="index">The Row Index to Get.</param>
			''' <param name="columnCount">The number of Columns in a Row.</param>
			''' <param name="insertNew">Whether a new Row should be inserted (rather than an existing one retrieved).</param>
			''' <returns></returns>
			''' <remarks>If insertNew is specified, then the row will be inserted BEFORE the index, e.g. will be the new index.</remarks>
			Private Function GetRow( _
				ByRef rows As List(Of Row), _
				ByVal index As Integer, _
				ByVal columnCount As Integer, _
				Optional ByVal insertNew As Boolean = False _
			) As Row

				If insertNew AndAlso index < rows.Count Then

					rows.Insert(index, Row.Create(columnCount))

				Else

					Do Until index + 1 <= rows.Count

						rows.Add(Row.Create(columnCount))

					Loop

				End If

				Return rows(index)

			End Function

			''' <summary>
			''' Method to Generate the Formatted Table of Values.
			''' </summary>
			''' <param name="source">The Object to Use as the Source.</param>
			''' <param name="propertiesToGet">The Properties to Get (these should be sorted).</param>
			''' <remarks></remarks>
			Private Sub GetValuesForFormatter( _
				ByRef rows As List(Of Row), _
				ByVal source As Object, _
				ByVal propertiesToGet As FormatterProperty(), _
				ByVal totalPropertyCount As Integer, _
				Optional ByRef currentRow As Integer = 0, _
				Optional ByVal currentColumn As Integer = 0 _
			)

				Dim writtenRows As Integer = 1

				' This variable is just a count of the properties that are being formatted.
				Dim propertyCount As Integer = propertiesToGet.Length

				For i As Integer = 0 To propertyCount - 1

					With propertiesToGet(i)

						Dim property_Value As Object = .Read(source, True)

						' Obviously, if the value is nothing then there is no need to add to the return.
						If Not property_Value Is Nothing Then

							If .Level = 0 Then ' This is the actual property value we want to add to the return.

								If TypeAnalyser.GetInstance(property_Value.GetType).IsArray Then

									' Handle the Array here by populating the column one row at a time.
									For j As Integer = 0 To CType(property_Value, Array).Length - 1

										GetRow(rows, currentRow + j, totalPropertyCount, j + 1 > writtenRows)(currentColumn + i) = _
											Cell.Create(CType(property_Value, Array)(j), propertiesToGet(i))

									Next

									writtenRows = Math.Max(writtenRows, CType(property_Value, Array).Length)

								Else

									' Add the single-valued object to the first row.
									GetRow(rows, currentRow, totalPropertyCount)(currentColumn + i) = Cell.Create(property_Value, propertiesToGet(i))

								End If

							Else

								' First we need to get all the Properties that share this same parent value.
								Dim shared_Children As New List(Of FormatterProperty)(New FormatterProperty() {.Child})

								For j As Integer = i + 1 To propertiesToGet.Length - 1

									If .Equals(propertiesToGet(j), True) Then shared_Children.Add(propertiesToGet(j).Child)

								Next

								Dim writtenChildRows As Integer = currentRow

								If TypeAnalyser.GetInstance(property_Value.GetType).IsArray Then

									For j As Integer = 0 To CType(property_Value, Array).Length - 1

										GetValuesForFormatter(rows, CType(property_Value, Array)(j), shared_Children.ToArray, totalPropertyCount, writtenChildRows, _
											currentColumn + i)

									Next

								Else

									GetValuesForFormatter(rows, property_Value, shared_Children.ToArray, totalPropertyCount, writtenChildRows, currentColumn + i)

								End If

								writtenRows = Math.Max(writtenRows, writtenChildRows)
								i += (shared_Children.Count - 1)

							End If

						End If

					End With

				Next

				If writtenRows > 0 Then currentRow += writtenRows

			End Sub

		#End Region

		#Region " Protected Methods "

			''' <summary>
			''' Method to Return the Default Formatter Properties for an Object. This is all
			''' public properties if available and an analyser is provided, or just 'ToString' if
			''' not.
			''' </summary>
			''' <param name="analyser"></param>
			''' <returns></returns>
			Protected Overridable Function GetDefaultProperties( _
				Optional ByVal analyser As TypeAnalyser = Nothing _
			) As FormatterProperty()

				If analyser Is Nothing Then Return New FormatterProperty() {New FormatterProperty(METHOD_TOSTRING, 0)}

				Dim members As MemberAnalyser() = analyser.ExecuteQuery(AnalyserQuery.QUERY_PUBLIC_READABLE_PROPERTIESORFIELDS)

				If Not members Is Nothing AndAlso members.Length > 0 Then

					Dim returnProperties(members.Length - 1) As FormatterProperty

					For i As Integer = 0 To members.Length - 1

						returnProperties(i) = FormatterProperty.Parse(members(i).Name, analyser)

					Next

					Return returnProperties

				Else

					Return New FormatterProperty() {FormatterProperty.Parse(METHOD_TOSTRING, analyser)}

				End If

			End Function

			''' <summary>
			''' Method to aggregate the Overriding, Adding and Removing of Properties.
			''' </summary>
			''' <param name="analyser"></param>
			''' <returns></returns>
			Protected Function GetOutputProperties( _
				Optional ByVal analyser As TypeAnalyser = Nothing _
			) As FormatterProperty()

				If FieldsToOverride.Length > 0 Then

					ProcessProperties(Host, FieldsToOverride, analyser)
					Return FieldsToOverride

				Else

					Dim propertiesToShow As FormatterProperty() = GetDefaultProperties(analyser)

					If FieldsToAdd.Length > 0 Then

						ProcessProperties(Host, FieldsToAdd, analyser)

						If FieldsToAdd.Length > 0 Then TypeAnalyser.ReconcileArraysByAddition(FieldsToAdd, propertiesToShow)

					End If

					If FieldsToRemove.Length > 0 Then

						ProcessProperties(Host, FieldsToRemove, analyser)

						For i As Integer = 0 To FieldsToRemove.Length - 1

							For j As Integer = 0 To propertiesToShow.Length - 1

								If String.Compare(propertiesToShow(j).FullName, FieldsToRemove(i).FullName, True) = 0 Then

									propertiesToShow = ActionFromArray(propertiesToShow, CollectionAction.RemoveIndex, j)

									Exit For

								End If

							Next

						Next

					End If

					Return propertiesToShow

				End If

			End Function

			''' <summary>
			''' Method to Get the Values for Particular Properties of An Object.
			''' </summary>
			''' <param name="source">The Object to get the Values from.</param>
			''' <param name="propertiesToGet">The Names of the Properties to get the Values from.</param>
			''' <remarks></remarks>
			Protected Sub PopulateValuesForFormatter( _
				ByRef rows As List(Of Row), _
				ByVal source As Object, _
				ByVal ParamArray propertiesToGet As FormatterProperty() _
			)

				' -- Sort the properties into a new order to allow easy processing --
				Dim sortedPropertiesToGet(propertiesToGet.Length - 1) As FormatterProperty

				propertiesToGet.CopyTo(sortedPropertiesToGet, 0)

				Dim property_Analyser As TypeAnalyser = TypeAnalyser.GetInstance(GetType(FormatterProperty))

				' The order is to sort on 'Level' (e.g. Depth) first, then 'Full Name'.
				Array.Sort(sortedPropertiesToGet, New Comparison.Comparer(ComponentModel.ListSortDirection.Ascending, New MemberAnalyser() { _
					MemberAnalyser.Parse(FormatterProperty.PROPERTY_LEVEL, property_Analyser), _
					MemberAnalyser.Parse(FormatterProperty.PROPERTY_FULLNAME, property_Analyser)}))
				' -------------------------------------------------------------------

				' -- Create a Sort 'Mapping' between the original and new Sort --
				Dim correctIndexes(propertiesToGet.Length - 1) As Integer

				For i As Integer = 0 To sortedPropertiesToGet.Length - 1

					For j As Integer = 0 To propertiesToGet.Length - 1

						If sortedPropertiesToGet(i) Is propertiesToGet(j) Then

							correctIndexes(i) = j
							Exit For

						End If

					Next

				Next
				' ----------------------------------------------------------------

				' -- Get the Actual Formatted Values --
				Dim wronglySortedRows As New List(Of Row)

				GetValuesForFormatter(wronglySortedRows, source, sortedPropertiesToGet, sortedPropertiesToGet.Length)
				' -------------------------------------

				' -- Restore each of the result rows to the correct order --
				For i As Integer = 0 To wronglySortedRows.Count - 1

					Dim correctCells(correctIndexes.Length - 1) As Cell

					For j As Integer = 0 To correctIndexes.Length - 1

						If wronglySortedRows(i).Cells.Count > j Then correctCells(correctIndexes(j)) = wronglySortedRows(i)(j)

					Next

					' Add correct cells as a row
					rows.Add(New Row(New List(Of Cell)(correctCells)))

				Next
				' -----------------------------------------------------------

				End Sub

				''' <summary>
				''' Creates the Column Headings for a Pivot.
				''' </summary>
				''' <param name="pivot_Values"></param>
				''' <returns></returns>
				Protected Function CreatePivotColumns( _
					ByVal pivot_Length As Integer, _
					ByVal pivot_Values As Array _
				) As List(Of FormatterProperty)

					Dim return_Columns As New List(Of FormatterProperty)

					' -- Sort the values by Column to create the Columns --
					SortValues(Host, ColumnFieldsToSortAscending, ColumnFieldsToSortDescending, pivot_Values)

					For i As Integer = 0 To pivot_Length - 1

						Dim column_Value As String = ObjectToSingleString(MemberAnalyser.ReadMembers(pivot_Values(i), ColumnFields), PIPE)

						Dim column_Exists As Boolean = False

						For j As Integer = 0 To return_Columns.Count - 1

							If String.Compare(return_Columns(j).Name, column_Value, False) = 0 Then

								column_Exists = True
								Exit For

							End If

						Next

						If Not column_Exists Then return_Columns.Add(New FormatterProperty(column_Value))

						If Host.Available(VerbosityLevel.Interactive) Then Host.Progress(i + 1 / pivot_Length, "Creating Pivot Columns")

					Next
					' -----------------------------------------------------

					' -- Insert Row Heading into the Columns --
					return_Columns.Insert(0, New FormatterProperty(ObjectToSingleString(RowFields, PIPE)))
					' -----------------------------------------

					Return return_Columns

				End Function

				''' <summary>
				''' Create the Rows (with Data) for a Pivot).
				''' </summary>
				''' <param name="pivot_Columns"></param>
				''' <param name="pivot_Values"></param>
				Protected Sub PopulatePivotRows( _
					ByVal pivot_Length As Integer, _
					ByVal pivot_Type As ValueType, _
					ByVal pivot_Columns As List(Of FormatterProperty), _
					ByRef pivot_Rows As List(Of Row), _
					ByVal pivot_Values As Array _
				)

				' -- Sort the values by Row to create the Rows --
				SortValues(Host, RowFieldsToSortAscending, RowFieldsToSortDescending, pivot_Values)

				For i As Integer = 0 To pivot_Length - 1

					' Get the column & row reference strings, and the cell values
					Dim column_Value As String = ObjectToSingleString(MemberAnalyser.ReadMembers(pivot_Values(i), ColumnFields), PIPE)
					Dim row_Value As String = ObjectToSingleString(MemberAnalyser.ReadMembers(pivot_Values(i), RowFields), PIPE)
					Dim cell_Value As IList = MemberAnalyser.ReadMembers(pivot_Values(i), ValueFields)

					' Use this to keep tabs on whether the row already exists.
					Dim row_Index As Integer = -1

					' Search for the Row, exiting the loop if it's found and setting it's index.
					For j As Integer = 0 To pivot_Rows.Count - 1

						If String.Compare(pivot_Rows(j).Cells.Item(0).Value, row_Value, False) = 0 Then

							row_Index = j
							Exit For

						End If

					Next

					' If it's not been found, add a new row with the row title as the first cell.
					If row_Index < 0 Then

						Dim new_Row As New Row

						For j As Integer = 0 To pivot_Columns.Count - 1

							If pivot_Type = ValueType.NotPresent Then

								new_Row.Add(LETTER_X)

							Else

								new_Row.Add()

							End If

						Next

						pivot_Rows.Add(new_Row.[Set](0, row_Value))
						row_Index = pivot_Rows.Count - 1

					End If

					' Iterate the columns looking for a match.
					For j As Integer = 0 To pivot_Columns.Count - 1

						If String.Compare(pivot_Columns(j).Name, column_Value, False) = 0 Then

							' Iterate the cell values.
							For k As Integer = 0 To cell_Value.Count - 1

								If Not cell_Value(k) Is Nothing Then

									Select Case pivot_Type

										Case ValueType.Count

											If pivot_Rows(row_Index).Cells(j) Is Nothing Then

												pivot_Rows(row_Index).[Set](j, 1)

											Else

												pivot_Rows(row_Index).Cells(j).Value += 1

											End If

										Case ValueType.NotPresent

											pivot_Rows(row_Index).[Set](j, Nothing)
											Exit For

										Case ValueType.Present

											pivot_Rows(row_Index).[Set](j, PIVOT_PRESENT)
											Exit For

										Case ValueType.BackgroundPresent

											pivot_Rows(row_Index).[Set](j, PIVOT_BACKGROUNDPRESENT)
											Exit For

										Case ValueType.Sum

											If pivot_Rows(row_Index).Cells(j) Is Nothing Then

												pivot_Rows(row_Index).[Set](j, cell_Value(k))

											Else

												pivot_Rows(row_Index).Cells(j).Value += cell_Value(k)

											End If

										Case ValueType.Min

											If pivot_Rows(row_Index).Cells(j) Is Nothing Then

												pivot_Rows(row_Index).[Set](j, cell_Value(k))

											Else

												pivot_Rows(row_Index).Cells(j).Value = Math.Min(cell_Value(k), pivot_Rows(row_Index).Cells(j).Value)

											End If

										Case ValueType.Max

											If pivot_Rows(row_Index).Cells(j) Is Nothing Then

												pivot_Rows(row_Index).[Set](j, cell_Value(k))

											Else

												pivot_Rows(row_Index).Cells(j).Value = Math.Max(cell_Value(k), pivot_Rows(row_Index).Cells(j).Value)

											End If

										Case ValueType.Average

											If pivot_Rows(row_Index).Cells(j) Is Nothing Then

												pivot_Rows(row_Index).[Set](j, New Calculated.Average(1, cell_Value(k), DecimalPlaces))

											Else

												CType(pivot_Rows(row_Index).Cells(j).Value, Calculated.Average).Count += 1
												CType(pivot_Rows(row_Index).Cells(j).Value, Calculated.Average).Value += cell_Value(k)

											End If

										Case ValueType.Percentage

											If pivot_Rows(row_Index).Cells(j) Is Nothing Then

												pivot_Rows(row_Index).[Set](j, New Calculated.Percentage(1, cell_Value(k), DecimalPlaces))

											Else

												CType(pivot_Rows(row_Index).Cells(j).Value, Calculated.Percentage).Count += 1
												CType(pivot_Rows(row_Index).Cells(j).Value, Calculated.Percentage).Total += cell_Value(k)

											End If

										Case ValueType.Value

											If pivot_Rows(row_Index).Cells(j) Is Nothing Then

												pivot_Rows(row_Index).[Set](j, New Text.StringBuilder(cell_Value(k).ToString))

											Else

												CType(pivot_Rows(row_Index).Cells(j).Value, Text.StringBuilder).Append(cell_Value(k).ToString)

											End If

										End Select

									End If

								Next

							Exit For

						End If

					Next

					If Host.Available(VerbosityLevel.Interactive) Then Host.Progress(i + 1 / pivot_Length, "Creating Pivot Rows")

				Next
				' --------------------------------------------------

			End Sub

		#End Region

		#Region " Command Methods "

			''' <summary>
			''' Method to Output a specified <see cref="Object"/> in a Grid Representation.
			''' </summary>
			''' <param name="value">The Object to output,
			''' this can be a type, single object, IList, ICollection or Array.</param>
			''' <remarks></remarks>
			<Command( _
				ResourceContainingType:=GetType(FormatCommands), _
				ResourceName:="CommandDetails", _
				Name:="csv", _
				Description:="@commandFormatterCSVDescription@" _
			)> _
			Public Sub ProcessCommandCommaSeparatedValueFile( _
				<Configurable( _
					ResourceContainingType:=GetType(FormatCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFormatterParameterDescriptionObjects@" _
				)> _
				ByVal value As Object, _
				<Configurable( _
					ResourceContainingType:=GetType(FormatCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFormatterParameterDescriptionOutputFile@" _
				)> _
				ByVal output_File As IO.FileStream _
			)

				Dim return_Value As Cube = ProcessCommandStandard(value)

				Dim output_Writer As New IO.StreamWriter(output_File)

				For i As Integer = 0 To return_Value.Columns.Count - 1

					If i > 0 Then output_Writer.Write(COMMA)
					output_Writer.Write(return_Value.Columns(i).ToString)

				Next

				output_Writer.WriteLine()

				Dim output_Slice As Slice = return_value.LastSlice

				For i As Integer = 0 To output_Slice.Rows.Count - 1

					For j As Integer = 0 To output_Slice.Rows(i).Cells.Count - 1

						If j > 0 Then output_Writer.Write(COMMA)

						If Not output_Slice.Rows(i).Cells(j) Is Nothing Then _
							output_Writer.Write(output_Slice.Rows(i).Cells(j).ToString)

					Next

					output_Writer.WriteLine()

				Next

				output_Writer.Close()

			End Sub

			''' <summary>
			''' Method to Output a specified <see cref="Object"/> in a Grid Representation.
			''' </summary>
			''' <param name="value">The Object to output,
			''' this can be a type, single object, IList, ICollection or Array.</param>
			''' <returns>The Formatted Output.</returns>
			''' <remarks></remarks>
			<Command( _
				ResourceContainingType:=GetType(FormatCommands), _
				ResourceName:="CommandDetails", _
				Name:="standard", _
				Description:="@commandFormatterStandardDescription@" _
			)> _
			Public Function ProcessCommandStandard( _
				<Configurable( _
					ResourceContainingType:=GetType(FormatCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFormatterParameterDescriptionObjects@" _
				)> _
				ByVal value As Object _
			) As IFixedWidthWriteable

				Return ProcessCommandStandard(value, INFORMATION_TITLE)

			End Function

			''' <summary>
			''' Method to Output a specified <see cref="Object"/> in a Grid Representation.
			''' </summary>
			''' <param name="value">The Object to output,
			''' this can be a type, single object, IList, ICollection or Array.</param>
			''' <param name="title">The title of the Formatted Output</param>
			''' <returns>The Formatted Output.</returns>
			''' <remarks></remarks>
			<Command( _
				ResourceContainingType:=GetType(FormatCommands), _
				ResourceName:="CommandDetails", _
				Name:="standard", _
				Description:="@commandFormatterStandardDescription@" _
			)> _
			Public Function ProcessCommandStandard( _
				<Configurable( _
					ResourceContainingType:=GetType(FormatCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFormatterParameterDescriptionObjects@" _
				)> _
				ByVal value As Object, _
				<Configurable( _
					ResourceContainingType:=GetType(FormatCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFormatterParameterDescriptionTitle@" _
				)> _
				ByVal title As String _
			) As IFixedWidthWriteable

				Return ProcessCommandStandard(value, title, False)

			End Function

			''' <summary>
			''' Method to Output a specified <see cref="Object"/> in a Grid Representation.
			''' </summary>
			''' <param name="value">The Object to output,
			''' this can be a type, single object, IList, ICollection or Array.</param>
			''' <param name="title">The title of the Formatted Output</param>
			''' <returns>The Formatted Output.</returns>
			''' <remarks></remarks>
			<Command( _
				ResourceContainingType:=GetType(FormatCommands), _
				ResourceName:="CommandDetails", _
				Name:="standard", _
				Description:="@commandFormatterStandardDescription@" _
			)> _
			Public Function ProcessCommandStandard( _
				<Configurable( _
					ResourceContainingType:=GetType(FormatCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFormatterParameterDescriptionObjects@" _
				)> _
				ByVal value As Object, _
				<Configurable( _
					ResourceContainingType:=GetType(FormatCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFormatterParameterDescriptionTitle@" _
				)> _
				ByVal title As String, _
				<Configurable( _
					ResourceContainingType:=GetType(FormatCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFormatterParameterDescriptionReturnIfEmpty@" _
				)> _
				ByVal return_If_Empty As Boolean _
			) As IFixedWidthWriteable

				If Not value Is Nothing Then

					Dim analyser As TypeAnalyser = Nothing

					' Handle Empty Generic List Typing
					If return_If_Empty Then analyser = TypeAnalyser.GetInstance( _
						TypeAnalyser.GetInstance(value.GetType).GetElementType())

					' Ensure we can handle the value as an Array
					value = CreateArray(value)

					If analyser Is Nothing Then analyser = TypeAnalyser.GetInstance(GetElementType(value))

					' Perform a standard sort on the Array
					SortValues(Host, FieldsToSortAscending, FieldsToSortDescending, value)

					' Get the properties that we're going to output
					Dim properties As FormatterProperty() = GetOutputProperties(analyser)

					Dim formatted_Rows As New List(Of Row)

					' Iterate the Array and Add a row (or rows) for each Value
					For i As Integer = 0 To CType(value, Array).Length - 1

						PopulateValuesForFormatter(formatted_Rows, CType(value, Array)(i), properties)

					Next

					' Return if we have rows.
					If Not formatted_Rows.Count = 0 OrElse return_If_Empty Then Return Cube.Create(InformationType.Information, _
					title, New List(Of FormatterProperty)(properties)).Add(New Slice(formatted_Rows))

				End If

				Return Nothing

			End Function

			''' <summary>
			''' Method to Output a specified <see cref="Object"/> in a Pivot Representation.
			''' </summary>
			''' <param name="value">The Object to output,
			''' this can be a type, single object, IList, ICollection or Array.</param>
			''' <returns>The Formatted Output.</returns>
			''' <remarks></remarks>
			<Command( _
				ResourceContainingType:=GetType(FormatCommands), _
				ResourceName:="CommandDetails", _
				Name:="pivot", _
				Description:="@commandFormatterPivotDescription@" _
			)> _
			Public Function ProcessCommandPivot( _
				<Configurable( _
					ResourceContainingType:=GetType(FormatCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFormatterParameterDescriptionObjects@" _
				)> _
				ByVal value As Object _
			) As IFixedWidthWriteable

				Return ProcessCommandPivot(value, INFORMATION_TITLE)

			End Function

			''' <summary>
			''' Method to Output a specified <see cref="Object"/> in a Pivot Representation.
			''' </summary>
			''' <param name="value">The Object to output,
			''' this can be a type, single object, IList, ICollection or Array.</param>
			''' <param name="title">Title of the Output</param>
			''' <returns>The Formatted Output.</returns>
			''' <remarks></remarks>
			<Command( _
				ResourceContainingType:=GetType(FormatCommands), _
				ResourceName:="CommandDetails", _
				Name:="pivot", _
				Description:="@commandFormatterPivotDescription@" _
			)> _
			Public Function ProcessCommandPivot( _
				<Configurable( _
					ResourceContainingType:=GetType(FormatCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFormatterParameterDescriptionObjects@" _
				)> _
				ByVal value As Object, _
				<Configurable( _
					ResourceContainingType:=GetType(FormatCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFormatterParameterDescriptionTitle@" _
				)> _
				ByVal title As String _
			) As IFixedWidthWriteable

				If Not value Is Nothing Then

					' Ensure we can handle the value as an Array
					value = CreateArray(value)
					Dim value_Analyser As TypeAnalyser = TypeAnalyser.GetInstance(GetElementType(value))

					' Ensure the Columns, Rows & Value Properties exist on the Target Type
					ProcessProperties(Host, ColumnFields, value_Analyser)
					ProcessProperties(Host, RowFields, value_Analyser)
					ProcessProperties(Host, ValueFields, value_Analyser)

					' If everything exists, continue with the Pivot
					If value.Length > 0 AndAlso ColumnFields.Length > 0 AndAlso RowFields.Length > 0 AndAlso ValueFields.Length > 0 Then

						Dim pivot_Columns As List(Of FormatterProperty) = CreatePivotColumns(CType(value, Array).Length, value)

						Dim pivot_Rows As New List(Of Row)

						PopulatePivotRows(CType(value, Array).Length, ValueFieldType, pivot_Columns, pivot_Rows, value)

						Return Cube.Create(InformationType.Information, title, pivot_Columns).Add(New Slice(pivot_Rows))

					End If

				End If

				Return Nothing

			End Function

			''' <summary>
			''' Method to Output the differences/similarities between two or more specified <see cref="Object"/>.
			''' </summary>
			''' <param name="value">The Object to Use as the Background.</param>
			''' <param name="values">The Other Object to Use in the Foreground.</param>
			''' <returns>The Formatted Output.</returns>
			''' <remarks></remarks>
			<Command( _
				ResourceContainingType:=GetType(FormatCommands), _
				ResourceName:="CommandDetails", _
				Name:="overlay", _
				Description:="@commandFormatterOverlayDescription@" _
			)> _
			Public Function ProcessCommandOverlay( _
				<Configurable( _
					ResourceContainingType:=GetType(FormatCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFormatterParameterDescriptionOverlayFirstObject@" _
				)> _
				ByVal value As Object, _
				<Configurable( _
					ResourceContainingType:=GetType(FormatCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFormatterParameterDescriptionOverlayOtherObjects@" _
				)> _
				ByVal ParamArray values As Object() _
			) As IFixedWidthWriteable

				Return ProcessCommandOverlay(value, INFORMATION_TITLE, values)

			End Function

			''' <summary>
			''' Method to Output the differences/similarities between two or more specified <see cref="Object"/>.
			''' </summary>
			''' <param name="value">The Object to Use as the Background.</param>
			''' <param name="values">The Other Object to Use in the Foreground.</param>
			''' <returns>The Formatted Output.</returns>
			''' <remarks></remarks>
			<Command( _
				ResourceContainingType:=GetType(FormatCommands), _
				ResourceName:="CommandDetails", _
				Name:="overlay", _
				Description:="@commandFormatterOverlayDescription@" _
			)> _
			Public Function ProcessCommandOverlay( _
				<Configurable( _
					ResourceContainingType:=GetType(FormatCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFormatterParameterDescriptionOverlayFirstObject@" _
				)> _
				ByVal value As Object, _
				<Configurable( _
					ResourceContainingType:=GetType(FormatCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFormatterParameterDescriptionTitle@" _
				)> _
				ByVal title As String, _
				<Configurable( _
					ResourceContainingType:=GetType(FormatCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFormatterParameterDescriptionOverlayOtherObjects@" _
				)> _
				ByVal ParamArray values As Object() _
			) As IFixedWidthWriteable

				If Not value Is Nothing Then

					' Ensure we can handle the value as an Array
					value = CreateArray(value)
					Dim value_Analyser As TypeAnalyser = TypeAnalyser.GetInstance(GetElementType(value))

					' Ensure the Columns, Rows & Value Properties exist on the Target Type
					ProcessProperties(Host, ColumnFields, value_Analyser)
					ProcessProperties(Host, RowFields, value_Analyser)
					ProcessProperties(Host, ValueFields, value_Analyser)

					' If everything exists, continue with the Pivot
					If value.Length > 0 AndAlso ColumnFields.Length > 0 AndAlso RowFields.Length > 0 AndAlso ValueFields.Length > 0 Then

						Dim lstAll As New ArrayList()
						lstAll.AddRange(value)

						For i As Integer = 0 To values.Length - 1

							lstAll.AddRange(CreateArray(values(i)))

						Next

						Dim aryAll As Array = lstAll.ToArray

						Dim pivot_Columns As List(Of FormatterProperty) = CreatePivotColumns(aryAll.Length, aryAll)

						Dim pivot_Rows As New List(Of Row)

						PopulatePivotRows(CType(value, Array).Length, ValueType.BackgroundPresent, pivot_Columns, pivot_Rows, value)

						For i As Integer = 0 To values.Length - 1

							Dim aryValue As Array = CreateArray(values(i))
							PopulatePivotRows(aryValue.Length, ValueFieldType, pivot_Columns, pivot_Rows, aryValue)

						Next

						Return Cube.Create(InformationType.Information, title, pivot_Columns).Add(New Slice(pivot_Rows))

					End If

				End If

				Return Nothing

			End Function

		#End Region

	End Class

End Namespace