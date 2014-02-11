Imports Leviathan.Caching.Simple
Imports Leviathan.Comparison.Comparer
Namespace Commands

	Partial Public Class ObjectsCommands

		#Region " Protected Shared Methods "

			Protected Function VerifyMembers( _
				ByVal analyser As TypeAnalyser, _
				ByVal members As MemberAnalyser() _
			) As MemberAnalyser()

				If Not members Is Nothing Then

					Dim lstMembersToRemove As New ArrayList

					For i As Integer = 0 To members.Length - 1

						If Not members(i).Parse(Nothing, Nothing, analyser) Then

							lstMembersToRemove.Add(members(i))

							If Host.Available(VerbosityLevel.Debug) Then _
								Host.[Debug](String.Format(ERROR_MEMBER_NOTFOUND, members(i).FullName))

						End If

					Next

					If lstMembersToRemove.Count > 0 Then _
						members = ActionFromArray( _
						members, CollectionAction.RemoveObjects, _
						lstMembersToRemove)

					Return members

				Else

					Return New MemberAnalyser() {}

				End If

			End Function

		#End Region

		#Region " Public Command Methods "

			''' <summary>
			''' Method to Load/Hydrate an Object/s from an Xml Stream.
			''' </summary>
			''' <param name="source">Xml Stream to Load Object/s from.</param>
			''' <returns>The Objects</returns>
			''' <remarks></remarks>
			<Command( _
				ResourceContainingType:=GetType(ObjectsCommands), _
				ResourceName:="CommandDetails", _
				Name:="load", _
				Description:="@commandObjectsDescriptionLoad@" _
			)> _
			Public Function ProcessCommandLoadObjects( _
				<Configurable( _
					ResourceContainingType:=GetType(ObjectsCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandObjectsParameterDescriptionLoadPath@" _
				)> _
				ByVal source As IO.Stream _
			) As Object

				Return Configuration.XmlSerialiser.CreateReader(source).Read()

			End Function

			''' <summary>
			''' Method to Save an Object/s to an Xml Stream.
			''' </summary>
			''' <param name="value">The Object/s to Save.</param>
			''' <param name="destination">Xml Stream to Save Object/s to.</param>
			''' <returns>Boolean value indicating Success.</returns>
			''' <remarks></remarks>
			<Command( _
				ResourceContainingType:=GetType(ObjectsCommands), _
				ResourceName:="CommandDetails", _
				Name:="save", _
				Description:="@commandObjectsDescriptionSaved@" _
			)> _
			Public Function ProcessCommandSaveObjects( _
				<Configurable( _
					ResourceContainingType:=GetType(ObjectsCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandObjectsParameterDescriptionObjects@" _
				)> _
				ByVal value As Object, _
				<Configurable( _
					ResourceContainingType:=GetType(ObjectsCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandObjectsParameterDescriptionSavePath@" _
				)> _
				ByVal destination As IO.Stream _
			) As Boolean

				Configuration.XmlSerialiser.CreateWriter(destination).Write(value)

				Return True

			End Function

			''' <summary>
			''' Method to Select a number of Objects from a List.
			''' </summary>
			''' <param name="value">The Objects from which to Select.</param>
			''' <param name="number">The Number of Objects to Select.</param>
			''' <remarks></remarks>
			<Command( _
				ResourceContainingType:=GetType(ObjectsCommands), _
				ResourceName:="CommandDetails", _
				Name:="select", _
				Description:="@commandObjectsDescriptionSelect@" _
			)> _
			Public Function ProcessCommandSelectObjects( _
				<Configurable( _
					ResourceContainingType:=GetType(ObjectsCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandObjectsParameterDescriptionObjects@" _
				)> _
				ByVal value As Object, _
				<Configurable( _
					ResourceContainingType:=GetType(ObjectsCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandObjectsParameterDescriptionNumber@" _
				)> _
				ByVal number As Integer _
			) As IList

				If Not value Is Nothing Then _
					value = CreateArray(value)

				If Not value Is Nothing Then

					If number < CType(value, Array).Length Then

						If Host.StringResponse(String.Format(QUESTION_OBJECT_TOFEWOBJECTS, CType(value, Array).Length)) Then _
							Return New ArrayList(CType(value, Array))

					ElseIf number = CType(value, Array).Length Then

						Return New ArrayList(CType(value, Array))

					ElseIf number > CType(value, Array).Length Then

						Dim ret_List As New ArrayList

						For i As Integer = 0 To number - 1
							ret_List.Add(Host.SelectionResponse( _
							CType(value, Array), QUESTION_OBJECT_LISTSELECTION))
						Next

						Return ret_List

					End If

				End If

				Return Nothing

			End Function

			''' <summary>
			''' Method to Select Properties from Object(s)
			''' </summary>
			''' <param name="value">The Objects from which to Select.</param>
			''' <param name="properties">The Properties to Select.</param>
			''' <returns></returns>
			''' <remarks></remarks>
			<Command( _
				ResourceContainingType:=GetType(ObjectsCommands), _
				ResourceName:="CommandDetails", _
				Name:="properties", _
				Description:="@commandObjectsDescriptionSelectProperties@" _
			)> _
			Public Function ProcessCommandSelectProperties( _
				<Configurable( _
					ResourceContainingType:=GetType(ObjectsCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandObjectsParameterDescriptionObjects@" _
				)> _
				ByVal value As Object, _
				<Configurable( _
					ResourceContainingType:=GetType(ObjectsCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandObjectsParameterDescriptionProperties@" _
				)> _
				ByVal properties As MemberAnalyser() _
			) As IList

				If value Is Nothing OrElse properties Is Nothing OrElse properties.Length = 0 Then

					Return Nothing

				Else

					value = TypeAnalyser.CreateArray(value)

					Dim analyser As TypeAnalyser = TypeAnalyser.GetInstance(TypeAnalyser.GetElementType(value))

					For i As Integer = 0 To properties.Length - 1

						properties(i).Parse(Nothing, Nothing, analyser)

					Next

					Dim lstReturn As IList = MemberAnalyser.ReadMembers(value, properties)

					If CType(value, Array).Length = 1 AndAlso properties.Length = 1 Then

						Return lstReturn(0)(0)

					ElseIf properties.Length = 1 Then

						Return New ArrayList(CType(lstReturn(0), ICollection))

					Else

						Return lstReturn

					End If

				End If

			End Function

			<Command( _
				ResourceContainingType:=GetType(ObjectsCommands), _
				ResourceName:="CommandDetails", _
				Name:="split", _
				Description:="@commandObjectsDescriptionSplitObjects@" _
			)> _
			Public Function ProcessCommandSplitObjects( _
				<Configurable( _
					ResourceContainingType:=GetType(ObjectsCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandObjectsParameterDescriptionObjects@" _
				)> _
				ByVal values As ICollection, _
				<Configurable( _
					ResourceContainingType:=GetType(ObjectsCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandObjectsParameterDescriptionProperties@" _
				)> _
				ByVal ParamArray propertiesToSplit As MemberAnalyser() _
			) As List(Of ICollection)

				Dim objectType As Type = TypeAnalyser.GetElementType(values)

				Dim objectTypeAnalyser As TypeAnalyser = TypeAnalyser.GetInstance(objectType)

				propertiesToSplit = VerifyMembers(objectTypeAnalyser, propertiesToSplit)

				Dim splitValues As New Dictionary(Of Int32, IList)

				For Each value As Object In values

					Dim keyValues As IList = MemberAnalyser.ReadMembers(value, propertiesToSplit)

					Dim valueHash As Int32 = CombineHashCodes(TypeAnalyser.CreateArray(keyValues))

					If Not splitValues.ContainsKey(valueHash) Then _
					splitValues.Add(valueHash, New ArrayList)

					splitValues(valueHash).Add(value)

				Next

				Dim returnList As New List(Of ICollection)

				For Each key As Integer In splitValues.Keys

					returnList.Add(splitValues(key))

				Next

				Return returnList

			End Function

			''' <summary>
			''' Method to Remove Duplicates from an Array or IList.
			''' </summary>
			''' <param name="value">The Array/IList to process.</param>
			''' <remarks></remarks>
			<Command( _
				ResourceContainingType:=GetType(ObjectsCommands), _
				ResourceName:="CommandDetails", _
				Name:="split", _
				Description:="@commandObjectsDescriptionRemoveDuplicates@" _
			)> _
			Public Function ProcessCommandRemoveDuplicates( _
				<Configurable( _
					ResourceContainingType:=GetType(ObjectsCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandObjectsParameterDescriptionObjects@" _
				)> _
				ByVal value As Object _
			) As Object

				If Not FieldsMarkingDuplicates Is Nothing _
					AndAlso FieldsMarkingDuplicates.Length > 0 _
					AndAlso Not value Is Nothing Then

					Dim collAnalyser As TypeAnalyser = TypeAnalyser.GetInstance(value.GetType)

					If collAnalyser.IsArray OrElse collAnalyser.IsIList Then

						SortValues(Host, FieldsToSortAscending, _
						FieldsToSortDescending, value)

						Dim length As Integer

						If collAnalyser.IsArray Then

							length = CType(value, Array).Length

						ElseIf collAnalyser.IsIList Then

							length = CType(value, IList).Count

						End If

						If length > 0 Then

							Dim aryProcessed As New ArrayList
							Dim aryReturn As New ArrayList

							For i As Integer = 0 To length - 1

								If i = length Then Exit For

								If Not value(i) Is Nothing Then

									Dim obj_values As IList = _
										MemberAnalyser.ReadMembers(value(i), _
											AnalyserQuery.QUERY_MEMBERS_READABLE _
											.SetNames(FieldsMarkingDuplicates))

									If Not obj_values Is Nothing AndAlso obj_values.Count > 0 Then

										Dim foundMatch As Boolean = False

										For j As Integer = 0 To aryProcessed.Count - 1

											Dim match As Boolean = False

											For k As Integer = 0 To CType(aryProcessed(j), IList).Count - 1

												If AreEqual(obj_values(k), CType(aryProcessed(j), IList)(k)) Then
													match = True
												Else
													If match Then match = False
													Exit For
												End If

											Next

											If match Then
												foundMatch = True
												Exit For
											End If

										Next

										If foundMatch Then

											If collAnalyser.IsIList Then
												CType(value, IList).RemoveAt(i)
												i -= 1
												length -= 1
											End If

										Else

											If collAnalyser.IsArray Then aryReturn.Add(value(i))

											aryProcessed.Add(obj_values)

										End If

									End If

								End If

							Next

							If collAnalyser.IsArray Then _
								value = aryReturn.ToArray(collAnalyser.ElementType)

						End If

					End If

				End If

				Return value

			End Function

		#End Region

		#Region " Private Shared Methods "

			''' <summary>
			''' The Actual Sorting Method.
			''' </summary>
			''' <param name="host">Command Execution Host</param>
			''' <param name="ascendingSortFields">The Members to Sort Ascendingly</param>
			''' <param name="descendingSortFields">The Members to Sort Descendingly</param>
			''' <param name="value">The Array to Sort</param>
			Private Shared Sub PerformSort( _
				ByVal host As ICommandsExecution, _
				ByVal ascendingSortFields As MemberAnalyser(), _
				ByVal descendingSortFields As MemberAnalyser(), _
				ByRef value As Array _
			)

				If Not ascendingSortFields Is Nothing AndAlso ascendingSortFields.Length > 0 Then

					ProcessProperties(host, ascendingSortFields, TypeAnalyser.GetInstance(TypeAnalyser.GetElementType(value)))
					Array.Sort(value, New Comparison.Comparer(ComponentModel.ListSortDirection.Ascending, ascendingSortFields))

				End If

				If Not descendingSortFields Is Nothing AndAlso descendingSortFields.Length > 0 Then

					ProcessProperties(host, descendingSortFields, TypeAnalyser.GetInstance(TypeAnalyser.GetElementType(value)))
					Array.Sort(value, New Comparison.Comparer(ComponentModel.ListSortDirection.Descending, descendingSortFields))

				End If

			End Sub

		#End Region

		#Region " Public Shared Methods "

			''' <summary>
			''' Method to Sort an Object using the FieldsToSort Properties.
			''' </summary>
			''' <param name="host">Command Execution Host</param>
			''' <param name="ascendingSortFields">The Members to Sort Ascendingly</param>
			''' <param name="descendingSortFields">The Members to Sort Descendingly</param>
			''' <param name="value">The Object to Sort.</param>
			''' <remarks>The Sort will be performed by an <see cref="Comparison.Comparer"></see> using first
			''' the ascending sorts, then the descending sorts. This means the descending sorts will
			''' get priority.
			''' This will only work for objects that are either arrays or implement <see cref="IList" />.</remarks>
			Public Shared Sub SortValues( _
				ByVal host As ICommandsExecution, _
				ByVal ascendingSortFields As MemberAnalyser(), _
				ByVal descendingSortFields As MemberAnalyser(), _
				ByRef value As Object _
			)

				If Not value Is Nothing Then

					Dim analyser As TypeAnalyser = TypeAnalyser.GetInstance(value.GetType)

					If analyser.IsArray Then

						SortValues(host, ascendingSortFields, descendingSortFields, _
							CType(value, Array))

					ElseIf analyser.IsIList Then

						SortValues(host, ascendingSortFields, descendingSortFields, _
							CType(value, IList))

					End If

				End If

			End Sub

			''' <summary>
			''' Method to Sort an Array using the FieldsToSort Properties.
			''' </summary>
			''' <param name="host">Command Execution Host</param>
			''' <param name="ascendingSortFields">The Members to Sort Ascendingly</param>
			''' <param name="descendingSortFields">The Members to Sort Descendingly</param>
			''' <param name="value">The Array to Sort.</param>
			''' <remarks>The Sort will be performed by an <see cref="Comparison.Comparer"></see> using first
			''' the ascending sorts, then the descending sorts. This means the descending sorts will
			''' get priority.</remarks>
			Public Shared Sub SortValues( _
				ByVal host As ICommandsExecution, _
				ByVal ascendingSortFields As MemberAnalyser(), _
				ByVal descendingSortFields As MemberAnalyser(), _
				ByRef value As Array _
			)

				PerformSort(host, ascendingSortFields, descendingSortFields, value)

			End Sub

			''' <summary>
			''' Method to Sort an IList using the FieldsToSort Properties.
			''' </summary>
			''' <param name="host">Command Execution Host</param>
			''' <param name="ascendingSortFields">The Members to Sort Ascendingly</param>
			''' <param name="descendingSortFields">The Members to Sort Descendingly</param>
			''' <param name="value">The IList to Sort.</param>
			''' <remarks>The Sort will be performed by an <see cref="Comparison.Comparer"></see> using first
			''' the ascending sorts, then the descending sorts. This means the descending sorts will
			''' get priority.</remarks>
			Public Shared Sub SortValues( _
				ByVal host As ICommandsExecution, _
				ByVal ascendingSortFields As MemberAnalyser(), _
				ByVal descendingSortFields As MemberAnalyser(), _
				ByRef value As IList _
			)

				Dim aryValue As Array = CreateArray(value)

				PerformSort(host, ascendingSortFields, descendingSortFields, aryValue)

				For i As Integer = 0 To aryValue.Length - 1

					value(i) = aryValue(i)

				Next

			End Sub

			Public Shared Function ProcessProperty( _
				ByVal host As ICommandsExecution, _
				ByRef _property As MemberAnalyser, _
				Optional ByVal analyser As TypeAnalyser = Nothing _
			) As Boolean

				If Not analyser Is Nothing AndAlso Not _property.Parse(Nothing, Nothing, analyser) Then

					If Host.Available(VerbosityLevel.Debug) Then _
						Host.[Debug](String.Format(ERROR_MEMBER_NOTFOUND, _property.FullName))

					Return False

				End If

				Return True

			End Function

			Public Shared Sub ProcessProperties( _
				ByVal host As ICommandsExecution, _
				ByRef properties As Array, _
				Optional ByVal analyser As TypeAnalyser = Nothing _
			) 

				If Not analyser Is Nothing Then

					For i As Integer = 0 To properties.Length - 1

						If Not ProcessProperty(Host, properties(i), analyser) Then

							properties = ActionFromArray(properties, CollectionAction.RemoveIndex, i)
							i -= 1

						End If

						If i >= properties.Length - 1 Then Exit For

					Next

				End If

			End Sub

		#End Region

	End Class

End Namespace