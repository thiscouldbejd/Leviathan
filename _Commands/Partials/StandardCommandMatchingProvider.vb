Imports Leviathan.Configuration
Imports Leviathan.Commands.StringCommands

Namespace Commands

	Partial Public Class StandardCommandMatchingProvider
		Implements ICommandsMatcher

		#Region " Public Properties "

			''' <summary>
			''' Provides Access to a single command, indexed by name.
			''' </summary>
			''' <param name="cmd"></param>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Public ReadOnly Property Command( _
				ByVal cmd As String _
			) As CommandInterrogatedClass
				Get
					If Not cmd = Nothing AndAlso IndexedCommandClasses.ContainsKey(cmd.ToLower) Then _
						Return IndexedCommandClasses(cmd.ToLower)

					Return Nothing
				End Get
			End Property

		#End Region

		#Region " ICommandsMatcher Implementation "

			Public Sub Prepare( _
				commands As CommandInterrogatedClass() _
			) Implements ICommandsMatcher.Prepare

				For i As Integer = 0 To commands.Length - 1

					IndexedCommandClasses.Add(commands(i).Name.ToLower, commands(i))

				Next

				Dim l_Commands(IndexedCommandClasses.Count - 1) As String

				IndexedCommandClasses.Keys.CopyTo(l_Commands, 0)

				Me.Commands = l_Commands

			End Sub

			Public Function MatchFromArgs( _
				ByRef commandArgs As String(), _
				ByRef match As CommandInterrogated, _
				ByVal host As ICommandsExecution, _
				Optional ByVal returnNoCommand As Boolean = False _
			) As Boolean Implements ICommandsMatcher.MatchFromArgs

				Dim l_Return As Boolean = False

				' Set up the Command Variables
				Dim commandClassName As String = Nothing
				Dim commandClass As CommandInterrogatedClass = Nothing
				Dim commandClassObject As Object = Nothing

				Dim commandMethodName As String = Nothing

				Try

					commandClassName = Comparison.Comparer.MatchFuzzyString( _
						commandArgs(0), Commands, COMMAND_NAME_DELINEATOR)

				Catch ex As Comparison.AmbiguousFuzzyStringMatchException

					Dim m As Comparison.FuzzyMatch = host.SelectionResponse( _
						New ArrayList(ex.Matches), String.Format(ERROR_COMMAND_AMBIGUOUS, ex.AmbiguousName))
					If Not m Is Nothing Then commandClassName = m.Match

				End Try

				If Not String.IsNullOrEmpty(commandClassName) Then commandClass = Command(commandClassName)

				If commandClass Is Nothing Then

					If host.Available(VerbosityLevel.Standard) Then host.Warn(String.Format(ERROR_COMMAND_UNKNOWN, commandArgs(0)))

				ElseIf returnNoCommand Then

					' ---- FOUND IT ----
					match = New CommandInterrogated(commandClass)
					l_Return = True

				Else

					commandArgs = RemoveStartElements(commandArgs, 1)

					If commandClass.HasSimpleConstructor Then

						commandClassObject = TypeAnalyser.Create(commandClass.Analyser.Type)

					ElseIf commandClass.HasComplexConstructor Then

						commandClassObject = TypeAnalyser.Create(commandClass.Analyser.Type, _
							New DictionaryEntry("_host", host) _
						)

					End If

					If Not commandClassObject Is Nothing Then _
						ConfigurationFactory.GetInstance().Configure(commandClassObject, _
							CommandInterrogatedFlag.ParseFlags(commandClassObject, commandClass, commandArgs, host))

					If commandArgs.Length = 0 Then

						If commandClass.HasDefaultMethod AndAlso commandClass.HasParameterLessMethod Then

							' ---- FOUND IT ----
							match = New CommandInterrogated(commandClass, commandClassObject).AddMethodIndex(0)
							l_Return = True

						Else

							If host.Available(VerbosityLevel.Standard) Then host.Warn(String.Format(ERROR_COMMAND_NODEFAULT, commandClassName))

						End If

					Else

						Dim methods As New Dictionary(Of String, Integer)

						With commandClass

							For i As Integer = 0 To .Methods.Length - 1

								If .Methods(i).IsDefault Then

									If (.Methods(i).ArgumentCount = commandArgs.Length OrElse _
										(.Methods(i).ArgumentCountFlexible AndAlso commandArgs.Length > .Methods(i).ArgumentCount)) Then _
											methods.Add(CommandInterrogatedMethod.DEFAULT_METHODNAME, i)

								ElseIf (.Methods(i).ArgumentCount = commandArgs.Length - 1 OrElse _
									(.Methods(i).ArgumentCountFlexible AndAlso commandArgs.Length - 1 > .Methods(i).ArgumentCount)) Then

									Try

										methods.Add(.Methods(i).Name.ToLower, i)

									Catch ex As Exception

										' -- Could be a multi-threading based bug here --
										If host.Available(VerbosityLevel.Standard) Then host.Warn(String.Format( _
											"Exception Adding Method: {0} to the collection in command class {1}", .Methods(i).Name.ToLower, commandClassName))

									End Try

								End If

							Next

						End With

						Try

							Dim commandNames(methods.Count - 1) As String
							methods.Keys.CopyTo(commandNames, 0)

							commandMethodName = Comparison.Comparer.MatchFuzzyString(commandArgs(0), commandNames, COMMAND_NAME_DELINEATOR)

							If String.IsNullOrEmpty(commandMethodName) Then _
								commandMethodName = Comparison.Comparer.MatchFuzzyString(CommandInterrogatedMethod.DEFAULT_METHODNAME, _
									commandNames, COMMAND_NAME_DELINEATOR)

						Catch ex As Comparison.AmbiguousFuzzyStringMatchException

							commandMethodName = host.SelectionResponse( _
								New ArrayList(ex.Matches), String.Format(ERROR_COMMAND_AMBIGUOUS, ex.AmbiguousName))

						End Try

						If Not String.IsNullOrEmpty(commandMethodName) Then

							' ---- FOUND IT ----
							match = New CommandInterrogated(commandClass, commandClassObject).AddMethodIndex(methods(commandMethodName))
							l_Return = True

							If Not match.Method.IsDefault Then commandArgs = RemoveStartElements(commandArgs, 1)

						End If

						If Not l_Return AndAlso host.Available(VerbosityLevel.Standard) Then _
							host.Warn(String.Format(ERROR_COMMAND_UNKNOWN, commandArgs(0)))

					End If

				End If

				Return l_Return

			End Function

			Public Function SuggestFromArgs( _
				ByVal commandArgs() As String, _
				ByRef suggestions() As CommandInterrogated _
			) As Boolean Implements ICommandsMatcher.SuggestFromArgs

				Dim l_Suggestions As New List(Of CommandInterrogated)

				Dim commandClassNames As String()

				Try

					commandClassNames = New String() {Comparison.Comparer.MatchFuzzyString(commandArgs(0), Commands, COMMAND_NAME_DELINEATOR)}

				Catch ex As Comparison.AmbiguousFuzzyStringMatchException

					Dim aryNames(ex.Matches.Length - 1) As String
					For i As Integer = 0 To ex.Matches.Length - 1
						aryNames(i) = ex.Matches(i).Match
					Next
					commandClassNames = aryNames

				End Try

				If Not commandClassNames Is Nothing Then

					For i As Integer = 0 To commandClassNames.Length - 1

						If Not String.IsNullOrEmpty(commandClassNames(i)) Then

							Dim commandClass As CommandInterrogatedClass = Command(commandClassNames(i))

							If commandArgs.Length = 1 Then

								Dim l_Command As New CommandInterrogated(commandClass)
								For j As Integer = 0 To commandClass.Methods.Length - 1
									l_Command.AddMethodIndex(j)
								Next

								l_Suggestions.Add(l_Command)

							Else

								Dim methods As New Dictionary(Of String, Integer())

								With commandClass

									For j As Integer = 0 To .Methods.Length - 1

										If (.Methods(j).IsDefault AndAlso .Methods(j).ArgumentCount >= commandArgs.Length - 1) OrElse _
											(Not .Methods(j).IsDefault AndAlso .Methods(j).ArgumentCount >= commandArgs.Length - 2) Then

											Dim methodName As String

											If .Methods(j).IsDefault Then
												methodName = CommandInterrogatedMethod.DEFAULT_METHODNAME
											Else
												methodName = .Methods(j).Name.ToLower
											End If

											If methods.ContainsKey(methodName) Then

												methods(methodName) = AddTo(methods(methodName), j)

											Else

												methods.Add(methodName, New Integer() {j})

											End If

										End If

									Next

								End With

								Dim commandMethodNames As String() = Nothing

								Try

									Dim commandNames(methods.Count - 1) As String
									methods.Keys.CopyTo(commandNames, 0)

									Dim commandMethodName As String = _
										Comparison.Comparer.MatchFuzzyString(commandArgs(1), commandNames, COMMAND_NAME_DELINEATOR)

									If String.IsNullOrEmpty(commandMethodName) Then commandMethodName = _
										Comparison.Comparer.MatchFuzzyString(CommandInterrogatedMethod.DEFAULT_METHODNAME, commandNames, COMMAND_NAME_DELINEATOR)

									If Not String.IsNullOrEmpty(commandMethodName) Then _
										commandMethodNames = New String() {commandMethodName}

								Catch ex As Comparison.AmbiguousFuzzyStringMatchException

									Dim aryNames(ex.Matches.Length - 1) As String
									For j As Integer = 0 To ex.Matches.Length - 1
										aryNames(j) = ex.Matches(j).Match
									Next
									commandMethodNames = aryNames

								End Try

								If Not commandMethodNames Is Nothing Then

									Dim l_Command As New CommandInterrogated(commandClass)

									For j As Integer = 0 To commandMethodNames.Length - 1

										If Not String.IsNullOrEmpty(commandMethodNames(j)) Then _
											l_Command.AddMethodIndex(methods(commandMethodNames(j)))

									Next

									l_Suggestions.Add(l_Command)

								End If

							End If

						End If

					Next

				End If

				If l_Suggestions.Count > 0 Then

					suggestions = l_Suggestions.ToArray

					Return True

				Else

					Return False

				End If

			End Function

			Public Function SuggestFromArgs( _
				ByVal commandArgs As String(), _
				ByRef suggestions As String() _
			) As Boolean Implements ICommandsMatcher.SuggestFromArgs

				Dim l_Suggestions As New List(Of String)

				Dim l_CommandSuggestions As CommandInterrogated() = Nothing

				If SuggestFromArgs(commandArgs, l_CommandSuggestions) Then

					For i As Integer = 0 To l_CommandSuggestions.Length - 1

						l_Suggestions.AddRange(l_CommandSuggestions(i).ToStrings(commandArgs))

					Next

					l_Suggestions.Sort()
					suggestions = l_Suggestions.ToArray

					Return True

				End If

				Return False

			End Function

		#End Region

	End Class

End Namespace
