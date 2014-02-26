Imports Leviathan.Configuration
Imports Leviathan.Commands.StringCommands
Imports Leviathan.Visualisation
Imports System.Windows.Media
Imports IT = Leviathan.Visualisation.InformationType
Imports VL = Leviathan.Commands.VerbosityLevel
Imports TX = System.Text
Imports TH = System.Threading.ThreadPool

Namespace Commands

	Partial Public Class CommandExecution
		Implements ICommandsExecution

		#Region " Private Properties "

			''' <summary>
			''' Provides Access to the Verbosity of the Command.
			''' </summary>
			''' <remarks>
			''' Preference is given to Silent, then Debug, then Interactive, then Error.
			''' </remarks>
			Private ReadOnly Property Verbosity() As VerbosityLevel
				Get
					If IsSilentEnabled OrElse (InteractiveOutput Is Nothing AndAlso Outputs.Count = 0) Then
						Return VL.Silent
					ElseIf IsDebugEnabled AndAlso Not InteractiveOutput Is Nothing Then
						Return VL.Debug
					ElseIf IsVerboseEnabled AndAlso Not InteractiveOutput Is Nothing Then
						Return VL.Verbose
					ElseIf Not InteractiveOutput Is Nothing Then
						Return VL.Interactive
					ElseIf IsErrorsEnabled Then
						Return VL.Errors
					Else
						Return VL.Standard
					End If
				End Get
			End Property

		#End Region

		#Region " Private Methods "

			''' <summary>
			''' Method to Invoke a Command Method.
			''' </summary>
			''' <param name="command">The Object which contains the method.</param>
			''' <param name="method">The Method.</param>
			''' <param name="args">The String Args.</param>
			''' <returns>The return value from the method.</returns>
			''' <remarks></remarks>
			Private Function InvokeMethod( _
				ByVal command As Object, _
				ByVal method As CommandInterrogatedMethod, _
				ByVal args As String() _
			) As Object

				Dim parameters As Object() = Array.CreateInstance(GetType(Object), method.ParameterCount)

				For i As Integer = 0 To method.Parameters.Length - 1

					If i = method.Parameters.Length - 1 AndAlso method.ArgumentCountFlexible AndAlso args.Length > method.Parameters.Length Then

						If args.Length > i Then

							Dim aryList As New ArrayList

							For j As Integer = i To args.Length - 1

								Dim parsedObject As Object = Nothing

								If Not ParseArgument(method.Parameters(i).Type.GetElementType, args(j), parsedObject) Then Return Nothing

								aryList.Add(parsedObject)

							Next

							parameters(method.Parameters(i).Position) = aryList.ToArray(method.Parameters(i).Type.GetElementType)

						End If

					Else

						Dim parsedObject As Object = Nothing

						If Not ParseArgument(method.Parameters(i).Type, args(i), parsedObject) Then Throw New ParsingException(args(i), method.Parameters(i).Type)

						parameters(method.Parameters(i).Position) = parsedObject

					End If

				Next

				Dim output As Object = method.Invoke(command, parameters, Me)

				' -- HANDLE PERFORMANCE --
				If Not performance Is Nothing Then

					Dim commandDetails As New TX.StringBuilder, argDetails As New TX.StringBuilder

					' Append the Command Object Type if present
					If Not command Is Nothing Then commandDetails.Append(command.GetType.Name).Append(FULL_STOP)

					' Append the Command Method
					commandDetails.Append(method.ToString)

					' Append the Args if present
					If Not args Is Nothing AndAlso args.Length > 0 Then argDetails.Append(HYPHEN, 2).Append(Space, 2).Append(SQUARE_BRACKET_START) _
						.Append(ArrayToSingleString(args, Space)).Append(SQUARE_BRACKET_END)

					' Log the Event
					performance.LogEvent(commandDetails.ToString, argDetails.ToString)

				End If
				' ------------------------

				Return output

			End Function

			''' <summary>
			''' Method to Parse Command Argument.
			''' </summary>
			''' <param name="parseToType">The Type to Parse To.</param>
			''' <param name="stringArgument">The Argument to Parse.</param>
			''' <param name="parsedArgument">The Parsed Argument.</param>
			''' <returns>A boolean indicating success.</returns>
			''' <remarks>This method will execute Command Parameters first so that it's output should be redirected to the currently
			''' executing method. They should be recursively handled so that commands can be embedded in commands.
			''' </remarks>
			Private Function ParseArgument( _
				ByVal parseToType As Type, _
				ByVal stringArgument As String, _
				ByRef parsedArgument As Object _
			) As Boolean

				Dim parsedCorrectly As Boolean
				parsedArgument = StringParser.Parse(stringArgument, parsedCorrectly, parseToType)
				Return parsedCorrectly

			End Function

		#End Region

		#Region " Private Parsing Methods "

			''' <summary>
			''' Method to Act as a Parsing Callback for the Parser/Command Arguments.
			''' </summary>
			''' <param name="value">The Value to Parse.</param>
			''' <param name="successfulParse">Whether the Parse was Successful.</param>
			''' <returns>The Parsed Object.</returns>
			''' <remarks></remarks>
			Private Function ProcessCommandAsParsingMethod( _
				ByVal value As String, _
				ByRef successfulParse As Boolean, _
				ByVal typeToParseTo As Type _
			) As Object

				Dim output As Object = Env_Execute(ArgParser.ParseLine(value))
				successfulParse = Not (output Is Nothing)
				Return output

			End Function

			''' <summary>
			''' Method to Act as a Parsing Callback for the File/Directory Arguments.
			''' </summary>
			''' <param name="value">The Value to Parse.</param>
			''' <param name="successfulParse">Whether the Parse was Successful.</param>
			''' <returns>The Parsed Object.</returns>
			''' <remarks></remarks>
			Private Function ProcessLoadAsParsingMethod( _
				ByVal value As String, _
				ByRef successfulParse As Boolean, _
				ByVal typeToParseTo As Type _
			) As Object

				Dim streams As IO.Stream() = StringParser.Parse(value, successfulParse, GetType(IO.Stream).MakeArrayType)

				If successfulParse AndAlso Not streams Is Nothing Then

					Dim lstOutputs As New ArrayList

					For i As Integer = 0 To streams.Length - 1

						lstOutputs.AddRange(TypeAnalyser.CreateArray(XmlSerialiser.CreateReader(streams(i)).Read()))

					Next

					Return lstOutputs.ToArray(typeToParseTo)

				Else

					Return Nothing

				End If

			End Function

		#End Region

		#Region " ICommandsExecution Implementation "

			Private Function Env_Available( _
				ByVal level As VerbosityLevel _
			) As Boolean _
			Implements ICommandsExecution.Available

				Return Verbosity >= level

			End Function

			Private Function Env_BooleanResponse( _
				ByVal question As String, _
				Optional ByVal value As IFixedWidthWriteable = Nothing _
			) As Boolean _
			Implements ICommandsExecution.BooleanResponse

				If Env_Available(VL.Interactive) Then

					Dim remove_Outputs As Integer

					If value Is Nothing Then

						InteractiveOutput.Show_Outputs(New Message(QUESTION_TITLE, IT.Question, _
							New String() {String.Format(INTERACTIVE_QUESTION, question)}))
						remove_Outputs = 1

					Else

						InteractiveOutput.Show_Outputs(New Message(QUESTION_TITLE, IT.Question, _
							New String() {String.Format(INTERACTIVE_QUESTION, question)}), value)
						remove_Outputs = 2

					End If

					Dim response As Boolean

					Do While True

						Dim input As String = InteractiveOutput.Get_Response()

						If Not String.IsNullOrEmpty(input) Then

							If input.ToLower.StartsWith(INTERACTIVE_AFFIRMATIVE) Then

								response = True
								Exit Do

							ElseIf input.ToLower.StartsWith(INTERACTIVE_NEGATIVE) Then

								response = False
								Exit Do

							End If

						End If

					Loop

					InteractiveOutput.Remove_Outputs(remove_Outputs)

					Return response

				End If

				Return False

			End Function

			Private Function Env_StringResponse( _
				ByVal question As String, _
				Optional ByVal allow_NullResponse As Boolean = False, _
				Optional ByVal value As IFixedWidthWriteable = Nothing _
			) As String _
			Implements ICommandsExecution.StringResponse

				If Env_Available(VL.Interactive) Then

					Dim remove_Outputs As Integer

					If value Is Nothing Then

						InteractiveOutput.Show_Outputs(New Message(QUESTION_TITLE, IT.Question, New String() {question}))
						remove_Outputs = 1

					Else

						InteractiveOutput.Show_Outputs(New Message(QUESTION_TITLE, IT.Question, New String() {question}), value)
						remove_Outputs = 2

					End If

					Dim response As String = Nothing

					If allow_NullResponse Then

						response = InteractiveOutput.Get_Response()

					Else

						Do Until Not String.IsNullOrEmpty(response)

							response = InteractiveOutput.Get_Response()

						Loop

					End If

					InteractiveOutput.Remove_Outputs(remove_Outputs)

					Return response

				End If

				Return Nothing

			End Function

			Private Function Env_SelectionResponse( _
				ByVal values As IList, _
				Optional ByVal question As String = Nothing, _
				Optional ByVal show_Properties As String() = Nothing, _
				Optional ByVal null_Response As String = Nothing _
			) As Object _
			Implements ICommandsExecution.SelectionResponse

				If Env_Available(VL.Interactive) Then

					Dim formatter As New FormatCommands(Me)

					If Not show_Properties Is Nothing AndAlso show_Properties.Length > 0 Then _
						formatter.FieldsToOverride = FormatterProperty.Create(show_Properties).ToArray()

					Dim output As Cube = formatter.ProcessCommandStandard(values, "Choose From List", True)

					output.Columns.Insert(0, New FormatterProperty(String.Empty))

					For i As Integer = 0 To output.LastSlice.Rows.Count - 1

						output.LastSlice.Rows(i).Cells.Insert(0, New Cell(i + 1))

					Next

					Dim can_ReturnNull As Boolean = False

					If Not String.IsNullOrEmpty(null_Response) AndAlso output.Columns.Count > 1 Then

						can_ReturnNull = True

						Dim new_Row As New Row()

						For i As Integer = 0 To output.Columns.Count - 1

							If i = 0 Then

								new_Row.Add(0)

							ElseIf i = 1 Then

								new_Row.Add(null_Response)

							Else

								new_Row.Add()

							End If

						Next

						output.LastSlice.Rows.Insert(0, new_Row)

					End If

					InteractiveOutput.Show_Outputs(output)

					Dim response As String

					If question = Nothing Then

						response = Env_StringResponse(INTERACTIVE_LIST_SELECTION)

					Else

						response = Env_StringResponse(question)

					End If

					InteractiveOutput.Remove_Output()

					Dim rowNumber As Integer = 0

					If Integer.TryParse(response, rowNumber) Then

						If can_ReturnNull AndAlso rowNumber = 0 Then

							Return Nothing

						ElseIf rowNumber <= 0 OrElse rowNumber > values.Count Then

							Return Env_SelectionResponse(values, question, show_properties, null_Response)

						Else

							If values(rowNumber - 1) Is Nothing Then

								Return Env_SelectionResponse(values, question, show_properties, null_Response)

							Else

								Return values(rowNumber - 1)

							End If

						End If

					Else

						Return Env_SelectionResponse(values, question, show_properties, null_Response)

					End If

				End If

				Return Nothing

			End Function

			Private Sub Env_Warn_External( _
				ByVal ParamArray values As String() _
			) _
			Implements ICommandsExecution.Warn

				If Env_Available(VL.Standard) Then TH.QueueUserWorkItem(AddressOf Env_ShowOutputs_Internal, New IFixedWidthWriteable() {New Message(WARNING_TITLE, IT.Warning, values)})

			End Sub

			Private Sub Env_Debug_External( _
				ByVal ParamArray values As String() _
			) _
			Implements ICommandsExecution.[Debug]

				If Env_Available(VL.Debug) Then TH.QueueUserWorkItem(AddressOf Env_ShowOutputs_Internal, New IFixedWidthWriteable() {New Message(DEBUG_TITLE, IT.[Debug], values)})

			End Sub

			Private Sub Env_Log_External( _
				ByVal ParamArray values As String() _
			) _
			Implements ICommandsExecution.[Log]

				If Env_Available(VL.Verbose) Then TH.QueueUserWorkItem(AddressOf Env_ShowOutputs_Internal, New IFixedWidthWriteable() {New Message(LOG_TITLE, IT.Log, values)})

			End Sub

			Private Sub Env_Success_External( _
				ByVal ParamArray values As String() _
			) _
			Implements ICommandsExecution.Success

				If Env_Available(VL.Standard) Then TH.QueueUserWorkItem(AddressOf Env_ShowOutputs_Internal, New IFixedWidthWriteable() {New Message(SUCCESS_TITLE, IT.Success, values)})

			End Sub

			Private Sub Env_Show_External( _
				ByVal ParamArray values As IFixedWidthWriteable() _
			) _
			Implements ICommandsExecution.Show

				If Env_Available(VL.Standard) Then TH.QueueUserWorkItem(AddressOf Env_ShowOutputs_Internal, values)

			End Sub

			Private Sub Env_Progress_Internal( _
				ByVal value As CommandProgress _
			)

				If Progress.Contains(value.Name) Then

					If value.Value = 1 Then

						Progress.Remove(value.Name)
						InteractiveOutput.Show_Progress(value.Value, value.Name, Nothing, Nothing)

					Else

						Dim first_Entry As DictionaryEntry = Progress(value.Name)

						' Amount of Work Done so Far
						Dim valueDiff As Single = value.Value - first_Entry.Key

						' Amount of Time Taken so Far
						Dim timeDiffInTicks As Long = value.Queued.Subtract(first_Entry.Value).Ticks

						If valueDiff > 0 Then

							' Amount of time left = (Time Taken / Work Done) - Time Taken
							InteractiveOutput.Show_Progress(value.Value, value.Name, New TimeSpan(timeDiffInTicks), _
								New TimeSpan(Math.Round(timeDiffInTicks / valueDiff - timeDiffInTicks, 0)))

						Else

							InteractiveOutput.Show_Progress(value.Value, value.Name, Nothing,  Nothing)

						End If

					End If

				Else

					Progress.Add(Value.Name, New DictionaryEntry(value.Value, DateTime.Now))
					If value.Value > 0 Then InteractiveOutput.Show_Progress(value.Value, value.Name, Nothing, Nothing)

				End If

			End Sub

			Private Sub Env_Progress_External( _
				ByVal value As Single, _
				ByVal name As String, _
				Optional details As String = Nothing _
			) _
			Implements ICommandsExecution.Progress

				If Env_Available(VL.Interactive) Then _
					TH.QueueUserWorkItem(AddressOf Env_Progress_Internal, New CommandProgress(value, name, details, DateTime.Now))

			End Sub

			Private Function Env_Execute( _
				ByVal line As String _
			) As Object _
			Implements ICommandsExecution.Execute

				Return Env_Execute(ArgParser.ParseLine(line))

			End Function

			Private Function Env_Execute( _
				ByVal _args As String() _
			) As Object _
			Implements ICommandsExecution.Execute

				Dim output As Object = Nothing
				Dim command As CommandInterrogated = Nothing

				If Not _args Is Nothing AndAlso _args.Length > 0 Then

					If Env_Available(VL.Debug) Then Env_Debug_External(CType(TypeAnalyser.ActionFromArray( _
						_args, CollectionAction.AddObject, EXECUTION_DEBUG_PREPARSE_ARGS), String()))

					Try

						If Config.Matcher.MatchFromArgs(_args, command, Me) Then

							output = InvokeMethod(command.CommandObject, command.Method, _args)

							If command.Method.Type = CommandType.Process Then

								If CType(output, Boolean) = True Then

									Env_Success_External(String.Format(COMMAND_SUCCEEDED, command.FullName))

								Else

									Env_Warn_External(String.Format(COMMAND_FAILED, command.FullName))

								End If

							End If

						End If

					Catch aex As System.Threading.ThreadAbortException
					Catch pex As ParsingException

						Env_Error_External(String.Format(ERROR_PARSING_FAILED, pex.Argument, pex.Type.FullName))

					Catch ex As Exception

						If Not command Is Nothing Then

							Env_Error_External(String.Format(ERROR_COMMAND_GENERAL, command.FullName, TypeAnalyser.ExceptionToString(ex)))

						Else

							Env_Error_External(String.Format(ERROR_COMMAND_GENERAL, String.Empty, TypeAnalyser.ExceptionToString(ex)))

						End If

						If Env_Available(VL.Debug) Then

							Dim level As Integer = 1

							Do Until ex Is Nothing

								If Not String.IsNullOrEmpty(ex.StackTrace) Then

									Dim stackTrace As New List(Of String)(ex.StackTrace.Split(Environment.NewLine))
									stackTrace.Insert(0, String.Format(ERROR_COMMAND_DEBUG, level))
									Env_Debug_External(stackTrace.ToArray)

								End If

								ex = ex.InnerException

								level += 1

							Loop

						End If

					Finally

						' Call the IDisposable Interface on the Command (if supported)
						If Not command Is Nothing AndAlso Not command.Command.Analyser Is Nothing AndAlso TypeAnalyser.DoesTypeImplementInterface( _
							command.Command.Analyser.Type, GetType(IDisposable)) Then CType(command.CommandObject, IDisposable).Dispose()

					End Try

				End If

				Return output

			End Function

			Private Sub Env_Error_External( _
				ByVal ParamArray values As String() _
			)

				If Env_Available(VL.Errors) Then TH.QueueUserWorkItem(AddressOf Env_ShowOutputs_Internal, New IFixedWidthWriteable() {New Message(ERROR_TITLE, IT.[Error], values)})

			End Sub

			''' <summary>
			''' Apart from Real-Time Calls (those predicating a response) and Progress Calls, this should be the only method interacting with Outputs.
			''' </summary>
			''' <param name="values"></param>
			''' <remarks>As an 'Internal' Method, this doesn't check the verbosity level so callers should only pass appropriate messages.</remarks>
			Private Sub Env_ShowOutputs_Internal( _
				ByVal values As IFixedWidthWriteable() _
			)

				If Not InteractiveOutput Is Nothing Then InteractiveOutput.Show_Outputs(values)

				For i As Integer = 0 To Outputs.Count - 1

					Outputs(i).Show_Outputs(values)

				Next

			End Sub

		#End Region

		#Region " Public Methods "

			'''<summary>Method to Begin the Execution Cycle</summary>
			Public Sub Run()

				' -- Configure this Class from Flags and Configuration Files --
				ConfigurationFactory.GetInstance().Configure(Me, CommandInterrogatedFlag.ParseFlags(Me, Command, Args, Me))

				Args = ArgParser.ParseArray(Args)
				' -------------------------------------------------------------

				' -- Create File/Email Outputs if required --
				If Not FileOutputs Is Nothing AndAlso FileOutputs.Length > 0 Then Outputs.Add(New CommandFileOutput(FileOutputs))

				If Not EmailOutputs Is Nothing AndAlso EmailOutputs.Length > 0 Then Outputs.Add(New CommandEmailOutput(MailLog, MailSuppress, _
					MailServer, MailServerPort, MailServerSSL, MailServerDomain, MailServerUsername, MailServerPassword, MailFromAddress, MailFromDisplay, _
					MailReplyToAddress, MailReplyToDisplay, EmailOutputs, String.Format(EXECUTION_RUN, ArrayToSingleString(Args, SPACE), DateTime.Now.ToString("s"))))

				If Not HtmlEmailOutputs Is Nothing AndAlso HtmlEmailOutputs.Length > 0 Then Outputs.Add(New CommandEmailOutput(MailLog, MailSuppress, _
					MailServer, MailServerPort, MailServerSSL, MailServerDomain, MailServerUsername, MailServerPassword, MailFromAddress, MailFromDisplay, _
					MailReplyToAddress, MailReplyToDisplay, HtmlEmailOutputs, String.Format(EXECUTION_RUN, ArrayToSingleString(Args, SPACE), DateTime.Now.ToString("s")), True, MailWidth))
				' -------------------------------------------

				If IsSimpleHelpEnabled OrElse IsComplexHelpEnabled Then

					' -- Output Help (if required) --
					If Env_Available(VL.Standard) Then Env_Show_External(GenerateHelp(Args, IsComplexHelpEnabled))

					If IsTimedEnabled Then Env_Show_External(Performance.LogEvent(PERFORMANCE_WORK_HELP).GenerateResults())
					' ------------------------------

				Else

					StringParser.AddParsingMethod(ParameterRegex, New FromString.ParsingMethod(AddressOf ProcessCommandAsParsingMethod))

					StringParser.AddParsingMethod(FileRegex, New FromString.ParsingMethod(AddressOf ProcessLoadAsParsingMethod))

					Dim output As Object = Nothing

					If Iterations > 1 Then

						Dim single_ObjectType As Type = Nothing

						For i As Integer = 0 To Iterations - 1

							Dim single_Output As Object = Env_Execute(args)

							If Not single_Output Is Nothing Then

								If output Is Nothing Then

									output = New ArrayList()
									single_ObjectType = single_Output.GetType()

								End If

								CType(output, IList).Add(single_Output)

							End If

						Next

						If Not output Is Nothing AndAlso CType(output, IList).Count > 0 Then output = CType(output, ArrayList).ToArray(single_ObjectType)

					Else

						output = Env_Execute(args)

					End If

					Dim performanceForOutput As IFixedWidthWriteable() = Nothing

					If IsTimedEnabled AndAlso Env_Available(VL.Standard) Then performanceForOutput = Performance.GenerateResults()

					If Not output Is Nothing Then

						Dim output_Analyser As TypeAnalyser = TypeAnalyser.GetInstance(output.GetType)

						If output_Analyser.ImplementsInterface(GetType(IFixedWidthWriteable)) Then

							If Env_Available(VL.Standard) Then Env_Show_External(New IFixedWidthWriteable() {CType(output, IFixedWidthWriteable)})

						ElseIf output_Analyser.IsArray AndAlso CType(output, Array).Length > 0 AndAlso Not output(0) Is Nothing AndALso _
							TypeAnalyser.GetInstance(output(0).GetType).ImplementsInterface(GetType(IFixedWidthWriteable)) Then

							Dim array_Output(CType(output, Array).Length - 1) As IFixedWidthWriteable

							Array.Copy(output, array_Output, array_Output.Length)

							If Env_Available(VL.Standard) Then Env_Show_External(array_Output)

						End If

					End If

					If Not performanceForOutput Is Nothing Then Env_Show_External(performanceForOutput)

				End If

				' -- Temporary Thread Pause (to allow Output Threads to Complete) --
				System.Threading.Thread.Sleep(500)

				' -- Close All Outputs --
				For i As Integer = 0 To Outputs.Count - 1

					Outputs(i).Close()

				Next
				' -----------------------

			End Sub

		#End Region

		#Region " Help Generation "

			#Region " Private Shared Variables "

				''' <summary>
				''' Public Shared Reference to a Description Column.
				''' </summary>
				Private Shared HELP_COLUMN_SPACER As String = TILDA & SPACE & SPACE

				''' <summary>
				''' Public Shared Reference to the String Used to indicate a readable type.
				''' </summary>
				Private Shared HELP_FLAG_READABLE As String = HELP_FLAG_READABLE_DESCRIPTION.Substring(0, 1)

				''' <summary>
				''' Public Shared Reference to the String Used to indicate a writable type.
				''' </summary>
				Private Shared HELP_FLAG_WRITABLE As String = HELP_FLAG_WRITABLE_DESCRIPTION.Substring(0, 1)

				''' <summary>
				''' Public Shared Reference to the String Used to indicate a simple type.
				''' </summary>
				Private Shared HELP_FLAG_SIMPLE As String = HELP_FLAG_SIMPLE_DESCRIPTION.Substring(0, 1)

				''' <summary>
				''' Public Shared Reference to the String Used to indicate an array type.
				''' </summary>
				Private Shared HELP_FLAG_ARRAY As String = HELP_FLAG_ARRAY_DESCRIPTION.Substring(0, 1)

				''' <summary>
				''' Public Shared Reference to the String Used to indicate a collection type.
				''' </summary>
				Private Shared HELP_FLAG_COLLECTION As String = HELP_FLAG_COLLECTION_DESCRIPTION.Substring(0, 1)

				''' <summary>
				''' Public Shared Reference to the String Used to indicate a list type.
				''' </summary>
				Private Shared HELP_FLAG_LIST As String = HELP_FLAG_LIST_DESCRIPTION.Substring(0, 1)

				''' <summary>
				''' Public Shared Reference to a Array Parameter String.
				''' </summary>
				Private Shared HELP_ARRAY_PARAMETER As String = SPACE & FULL_STOP & FULL_STOP & FULL_STOP

			#End Region

			#Region " Public Methods "

				''' <summary>
				''' Method to Generate Help for a supplied set of Arguments.
				''' </summary>
				''' <param name="args">The Arguments.</param>
				''' <param name="showAllCommands">Whether all commands (including hidden commands) should be shown.</param>
				''' <returns></returns>
				Public Function GenerateHelp( _
					ByVal args As String(), _
					ByVal showAllCommands As Boolean _
				) As IFixedWidthWriteable()

					' ---- Create the Help List with the Entry Assembly Details/Shortcuts -----
					Dim lstHelp As New List(Of IFixedWidthWriteable)(New IFixedWidthWriteable() {GenerateGeneralAssemblyHelp(HELP_ENTRY_ASSEMBLY, _
						AssemblyAnalyser.GetInstance(Assembly.GetEntryAssembly),  Config.Started, Config.Finished, Config.Loaded), _
						GenerateKeyboardHelp()})
					' -------------------------------------------------------------------------

					Dim suggestions As CommandInterrogated() = Nothing

					If args Is Nothing OrElse args.Length = 0 Then

						' CASE 1: No Args Supplied so we'll Output Help for All Classes.
						lstHelp.Add(GenerateCommandsHelp(Config.InterrogatedCommands, showAllCommands))

					ElseIf Config.Matcher.SuggestFromArgs(args, suggestions) Then

						' CASE 2: Single Suggested Command, Single Suggested Method
						If suggestions.Length = 1 AndAlso (suggestions(0).HasSingleMethod OrElse Not suggestions(0).HasMethods) Then

							' ---- Command Assembly (only if different from entry assembly) ----
							If Not suggestions(0).Command.Analyser Is Nothing AndAlso Not suggestions(0).Command.Analyser.Type.Assembly Is _
								Assembly.GetEntryAssembly Then lstHelp.Add(GenerateGeneralAssemblyHelp(HELP_COMMAND_ASSEMBLY, _
									AssemblyAnalyser.GetInstance(suggestions(0).Command.Analyser.Type.Assembly)))
							' ---- Command Assembly ----

							' ---- Command Details ----
							lstHelp.Add(GenerateCommandHelp(suggestions(0).Command, suggestions(0).Method))
							' -------------------------

							' ---- Return Type ----
							Dim returnType As Type = Nothing

							If suggestions(0).HasSingleMethod AndAlso Not suggestions(0).Method.InvokableMethod Is Nothing AndAlso _
								suggestions(0).Method.InvokableMethod.ReturnTypeAnalyser.IsComplex Then _
								lstHelp.Add(GenerateTypeHelp(suggestions(0).Method.InvokableMethod.ReturnType, HELP_RETURN_TYPE))
							' ---------------------

						Else ' CASE 3: Multiple Suggested Commands or Multiple Suggested Methods

							' ---- Command Details ----
							lstHelp.Add(GenerateCommandsHelp(suggestions, True))
							' -------------------------

						End If

					Else ' CASE 4: Check to see if it's a Type

						' Firstly, output a message indicating that the command cannot be found.
						If Env_Available(VL.Standard) Then Env_Warn_External(String.Format(ERROR_COMMAND_UNKNOWN, args(0)))

						If args.Length = 1 Then

							Dim type_Parse As Boolean, type_Help As System.Type

							' Split a Multi-Part Type into two parts.
							If args(0).IndexOf(EXCLAMATION_MARK) > 0 Then args = args(0).Split(EXCLAMATION_MARK)

							' Attempt a Parse
							type_Help = StringParser.Parse(args(0), type_Parse, GetType(Type))

							' If the Parse is sucessful.
							If type_Parse Then

								If args.Length = 2 Then

									Dim allMembers As MemberInfo() = type_Help.GetMembers

									For i As Integer = 0 To allMembers.Length - 1

										If String.Compare(allMembers(i).Name, args(1), True) = 0 Then

											' If the name matches the second part, check the type of Member.
											If allMembers(i).MemberType = MemberTypes.Method Then

												If Not CType(allMembers(i), MethodInfo).ReturnType Is Nothing AndAlso _
													Not CType(allMembers(i), MethodInfo).ReturnType Is GetType(System.Void) Then _
														type_Help = CType(allMembers(i), MethodInfo).ReturnType

											ElseIf allMembers(i).MemberType = MemberTypes.Property Then

												type_Help = CType(allMembers(i), PropertyInfo).PropertyType

											ElseIf allMembers(i).MemberType = MemberTypes.Field Then

												type_Help = CType(allMembers(i), FieldInfo).FieldType

											ElseIf allMembers(i).MemberType = MemberTypes.NestedType OrElse allMembers(i).MemberType = MemberTypes.TypeInfo Then

												type_Help = allMembers(i)

											End If

											Exit For

										End If

									Next

								End If

								' ---- Type Assembly ----
								lstHelp.Add(GenerateGeneralAssemblyHelp(HELP_TYPE_ASSEMBLY, AssemblyAnalyser.GetInstance(type_Help.Assembly)))
								' -----------------------

								' ---- Type Details ----
								lstHelp.Add(GenerateTypeHelp(type_Help))
								' ----------------------

							End If

						End If

					End If

					Return lstHelp.ToArray

				End Function

			#End Region

			#Region " Private Shared Methods "

				Private Shared Function GetEnumValuesAsLine( _
					ByVal enumType As Type _
				) As String

					Dim sb As New TX.StringBuilder

					Dim enumValues() As String = [Enum].GetNames(enumType)

					If Not enumValues Is Nothing AndAlso enumValues.Length > 0 Then
						sb.Append(BRACKET_START)

						For j As Integer = 0 To enumValues.Length - 1

							If Not j = 0 Then sb.Append(PIPE)
							sb.Append(enumValues(j))

						Next

						sb.Append(BRACKET_END)

					End If

					Return sb.ToString

				End Function

				Private Shared Function GetAssemblyDisplayName( _
					ByVal assemblyName As String, _
					ByVal assemblyVersion As String _
				) As String

					If Not String.IsNullOrEmpty(assemblyName) AndAlso Not String.IsNullOrEmpty(assemblyVersion) Then _
						Return String.Concat(ANGLE_BRACKET_START, assemblyName, SPACE, assemblyVersion, ANGLE_BRACKET_END)

					Return Nothing

				End Function

				Private Shared Function GetAssemblyDisplayName( _
					ByVal commandClass As CommandInterrogatedClass _
				) As String

					Return GetAssemblyDisplayName(commandClass.AssemblyName, commandClass.AssemblyVersion)

				End Function

				Private Shared Function GetAssemblyDisplayName( _
					ByVal assembly As Assembly _
				) As String

					Return GetAssemblyDisplayName(assembly.GetName.Name, assembly.GetName.Version.ToString)

				End Function

				Private Shared Function GetCommandClassFlagsAsLine( _
					ByVal commandClass As CommandInterrogatedClass _
				) As String

					Dim command_Flags As New TX.StringBuilder

					For i As Integer = 0 To commandClass.Flags.Length - 1

						command_Flags.Append(commandClass.Flags(i).Name)

						If Not commandClass.Flags(i).FlagAttribute.ArgsDescription = Nothing Then

							command_Flags.Append(COLON)
							command_Flags.Append(commandClass.Flags(i).FlagAttribute.ArgsDescription)

						End If

						If i < commandClass.Flags.Length - 1 Then command_Flags.Append(Space)

					Next

					Return command_Flags.ToString

				End Function

				Private Shared Function GetCommandMethodParametersAsLine( _
					ByVal method As CommandInterrogatedMethod _
				) As String

					Dim command_Params As New TX.StringBuilder

					If Not method.Parameters Is Nothing AndAlso method.Parameters.Length > 0 Then

						Dim paramCount As Integer = 0

						For j As Integer = 0 To method.Parameters.Length - 1

							If Not String.IsNullOrEmpty(method.Parameters(j).Description) Then

								If paramCount > 0 Then command_Params.Append(Space, 2)
								command_Params.Append(SQUARE_BRACKET_START)

								command_Params.Append(method.Parameters(j).Description.ToUpper)
								If method.Parameters(j).Type.IsEnum Then command_Params.Append(GetEnumValuesAsLine(method.Parameters(j).Type))

								command_Params.Append(SQUARE_BRACKET_END)

								If method.ArgumentCountFlexible AndAlso j = method.Parameters.Length - 1 Then command_Params.Append(HELP_ARRAY_PARAMETER)

									paramCount += 1

							End If

						Next

					End If

					Return command_Params.ToString

				End Function

			#End Region

			#Region " Public Shared Methods "

				''' <summary>
				''' Method to Generate Help Details for an Assembly.
				''' </summary>
				''' <param name="assemblyHelpTitle">The Assembly Help Title (e.g. Entry or Command Assembly)</param>
				''' <param name="assembly">The Analysed Assembly</param>
				''' <returns></returns>
				''' <remarks></remarks>
				Public Shared Function GenerateGeneralAssemblyHelp( _
					ByVal assemblyHelpTitle As String, _
					ByVal assembly As AssemblyAnalyser, _
					ByVal Optional startLoad As DateTime = Nothing, _
					ByVal Optional endLoad As DateTime = Nothing, _
					ByVal Optional lastLoad As DateTime = Nothing _
				) As IFixedWidthWriteable

					' ---- Assembly Title -----
					Dim assemblyTitle As String

					Dim attrsTle As AttributeAnalyser() = assembly.ExecuteQuery(New AnalyserQuery() _
						.SetReturnType(AnalyserType.AttributeAnalyser).SetReturnTypeIsOrInheritedFromType(GetType(AssemblyTitleAttribute)))

					If Not attrsTle Is Nothing AndAlso attrsTle.Length = 1 Then

						assemblyTitle = CType(attrsTle(0).Attribute, AssemblyTitleAttribute).Title

					Else

						assemblyTitle = assembly.Assembly.GetName.Name

					End If
					' ------------------------

					' ---- Create the Rows ----
					Dim assembly_Rows As New List(Of Row)
					' -------------------------

					' ---- Assembly Version ----
					assembly_Rows.Add(New Row().Add(HELP_ROW_VERSION).Add(assembly.Assembly.GetName.Version.ToString))
					' --------------------------

					' ---- Assembly Description ----
					Dim attrsDesc As AttributeAnalyser() = assembly.ExecuteQuery(New AnalyserQuery() _
						.SetReturnType(AnalyserType.AttributeAnalyser).SetReturnTypeIsOrInheritedFromType(GetType(AssemblyDescriptionAttribute)))

					If Not attrsDesc Is Nothing AndAlso attrsDesc.Length = 1 Then assembly_Rows.Add(New Row().Add(HELP_ROW_DESCRIPTION) _
						.Add(CType(attrsDesc(0).Attribute, AssemblyDescriptionAttribute).Description))
					' ------------------------------

					' ---- Assembly Product ----
					Dim attrsProd As AttributeAnalyser() = assembly.ExecuteQuery(New AnalyserQuery() _
						.SetReturnType(AnalyserType.AttributeAnalyser).SetReturnTypeIsOrInheritedFromType(GetType(AssemblyProductAttribute)))

					If Not attrsProd Is Nothing AndAlso attrsProd.Length = 1 Then assembly_Rows.Add(New Row().Add(HELP_ROW_PRODUCT) _
						.Add(CType(attrsProd(0).Attribute, AssemblyProductAttribute).Product))
					' --------------------------

					' ---- Assembly Copyright ----
					Dim attrsCpy As AttributeAnalyser() = assembly.ExecuteQuery(New AnalyserQuery() _
						.SetReturnType(AnalyserType.AttributeAnalyser).SetReturnTypeIsOrInheritedFromType(GetType(AssemblyCopyrightAttribute)))

					If Not attrsCpy Is Nothing AndAlso attrsCpy.Length = 1 Then assembly_Rows.Add(New Row().Add(HELP_ROW_COPYRIGHT) _
						.Add(CType(attrsCpy(0).Attribute, AssemblyCopyrightAttribute).Copyright))
					' ----------------------------

					' ---- Runtime Version ----
					assembly_Rows.Add(New Row().Add(HELP_ROW_CLR).Add(assembly.Assembly.ImageRuntimeVersion))
					' -------------------------

					' ---- Loading Time ----
					If Not startLoad = Nothing AndAlso Not endLoad = Nothing Then assembly_Rows.Add(New Row().Add( _
						HELP_ROW_LOADINGDURATION).Add(New TimeSpanConvertor().ParseStringFromTimespan(endLoad.Subtract(startLoad), New Boolean)))
					' ----------------------

					' ---- Last Load Date/Time ----
					If Not lastLoad = Nothing Then assembly_Rows.Add(New Row().Add(HELP_ROW_LASTLOAD).Add(lastLoad.ToString("G")))
					' ----------------------

					Return Cube.Create(IT.Help, assemblyHelpTitle, HELP_ROW_TITLE, assemblyTitle).Add(New Slice(assembly_Rows))

				End Function

				''' <summary>
				''' Method to Generate Help Details for Keyboard Shortcuts.
				''' </summary>
				''' <returns></returns>
				''' <remarks></remarks>
				Public Shared Function GenerateKeyboardHelp() As IFixedWidthWriteable

					' ---- Create the Rows ----
					Dim keyboard_Rows As New List(Of Row)
					' -------------------------

					' ---- Help Key F1 ----
					keyboard_Rows.Add(New Row().Add(HELP_KEY_F1).Add(HELP_KEY_F1_DESC))
					' --------------------------

					' ---- Help Key F2 ----
					keyboard_Rows.Add(New Row().Add(HELP_KEY_F2).Add(HELP_KEY_F2_DESC))
					' --------------------------

					' ---- Help Key F3 ----
					keyboard_Rows.Add(New Row().Add(HELP_KEY_F3).Add(HELP_KEY_F3_DESC))
					' --------------------------

					' ---- Help Key F4 ----
					keyboard_Rows.Add(New Row().Add(HELP_KEY_F4).Add(HELP_KEY_F4_DESC))
					' --------------------------

					' ---- Help Key F5 ----
					keyboard_Rows.Add(New Row().Add(HELP_KEY_F5).Add(HELP_KEY_F5_DESC))
					' --------------------------

					' ---- Help Key F6 ----
					keyboard_Rows.Add(New Row().Add(HELP_KEY_F6).Add(HELP_KEY_F6_DESC))
					' --------------------------

					' ---- Help Key F7 ----
					keyboard_Rows.Add(New Row().Add(HELP_KEY_F7).Add(HELP_KEY_F7_DESC))
					' --------------------------

					' ---- Help Key F8 ----
					keyboard_Rows.Add(New Row().Add(HELP_KEY_F8).Add(HELP_KEY_F8_DESC))
					' --------------------------

					Return Cube.Create(IT.Help, HELP_KEY_TITLE, HELP_ROW_DESCRIPTION, HELP_ROW_KEY).Add(New Slice(keyboard_Rows))

				End Function


				''' <summary>
				''' Method to Generate Help Details for Command Classes.
				''' </summary>
				''' <param name="commandClasses">The Command Classes to Output</param>
				''' <param name="showAll">Whether all Classes (including hidden) should be Output</param>
				''' <returns></returns>
				''' <remarks></remarks>
				Public Shared Function GenerateCommandsHelp( _
					ByVal commandClasses As CommandInterrogated(), _
					ByVal showAll As Boolean _
				) As IFixedWidthWriteable

					' ---- Create the Rows ----
					Dim command_Rows As New List(Of Row)
					' -------------------------

					' ---- Interrogate the Commands ----
					For i As Integer = 0 To commandClasses.Length - 1

						If showAll OrElse Not commandClasses(i).Command.Hidden Then

							' ---- Interrogate the Flags ----
							Dim command_Flags As New TX.StringBuilder

							For j As Integer = 0 To commandClasses(i).Command.Flags.Length - 1

								command_Flags.Append(commandClasses(i).Command.Flags(j).Name)

								If Not String.IsNullOrEmpty(commandClasses(i).Command.Flags(j).FlagAttribute.ArgsDescription) Then _
									command_Flags.Append(COLON).Append(commandClasses(i).Command.Flags(j).FlagAttribute.ArgsDescription)

								command_Flags.Append(Space)

							Next
							' -------------------------------

							If commandClasses(i).Methods.Length = 0 Then

								' ---- Command and Flags as a Single Row ----
								If command_Flags.Length > 0 Then

									Dim command_Row As New Row()
									command_Rows.Add(command_Row.Add(commandClasses(i).Command.Name.ToLower).Add(HELP_COLUMN_FLAGS).Add(command_Flags.ToString.TrimEnd))

								End If
								' -------------------------------------------

							Else

								Dim lastCommandName As String = Nothing

								For j As Integer = 0 To commandClasses(i).Methods.Length - 1

									' ---- Create the Method Row ----
									Dim command_Row As New Row()
									' -------------------------------

									' ---- Command and Method Name ----
									Dim commandName As String = commandClasses(i).Command.Name.ToLower

									If Not String.IsNullOrEmpty(commandClasses(i).Methods(j).Name) Then _
										commandName = commandName & SPACE & commandClasses(i).Methods(j).Name

									If String.Compare(commandName, lastCommandName) <> 0 Then lastCommandName = commandName Else commandName = Nothing
									command_Row.Add(commandName)
									' ---------------------------------

									' ---- Command Type ----
									Dim commandType As String = Nothing
									If commandClasses(i).Methods(j).Type <> Commands.CommandType.Unknown Then _
										command_Row.Add(commandClasses(i).Methods(j).Type.ToString) Else command_Row.Add()
									' ----------------------

									' ---- Command Parameters ----
									command_Rows.Add(command_Row.Add(GetCommandMethodParametersAsLine(commandClasses(i).Methods(j))))
									' ----------------------------

								Next

								' ---- Flags as a Single Row ----
								If command_Flags.Length > 0 Then

									Dim flag_Row As New Row()
									command_Rows.Add(flag_Row.Add().Add(HELP_COLUMN_FLAGS).Add(command_Flags.ToString.TrimEnd))

								End If
								' -------------------------------------------

							End If

							' ---- Spacer Row ----
							command_Rows.Add(New Row())
							' --------------------

						End If

					Next
					' ----------------------------------

					' ---- Remove the Final Spacer and Return ----
					If command_Rows.Count > 1 Then command_Rows.RemoveAt(command_Rows.Count - 1)

					Return Cube.Create(IT.Help, HELP_COMMAND_DETAILS, HELP_COLUMN_COMMAND, HELP_COLUMN_TYPE, HELP_COLUMN_PARAMETERS) _
						.Add(New Slice(command_Rows))

				End Function

				''' <summary>
				''' Method to Generate Detailed Help Details for Command/Method Class.
				''' </summary>
				''' <param name="commandClass">The Command Class To Output</param>
				''' <param name="commandMethod">The Command Method To Output</param>
				''' <returns></returns>
				''' <remarks></remarks>
				Public Shared Function GenerateCommandHelp( _
					ByVal commandClass As CommandInterrogatedClass, _
					Optional ByVal commandMethod As CommandInterrogatedMethod = Nothing _
				) As IFixedWidthWriteable

					' ---- Create the Rows ----
					Dim command_Rows As New List(Of Row)
					' -------------------------

					' ---- Command and Method Name ----
					If commandMethod Is Nothing OrElse String.IsNullOrEmpty(commandMethod.Name) Then

						command_Rows.Add(New Row().Add(HELP_ROW_NAME).Add(commandClass.Name))

					Else

						command_Rows.Add(New Row().Add(HELP_ROW_NAME).Add(commandClass.Name & SPACE & commandMethod.Name))

					End If
					' ---------------------------------

					' ---- Command Parameters ----
					If Not commandMethod Is Nothing Then command_Rows.Add(New Row().Add(HELP_COLUMN_PARAMETERS) _
						.Add(GetCommandMethodParametersAsLine(commandMethod)))
					' ----------------------------

					' ---- Command Command Description ----
					command_Rows.Add(New Row().Add(HELP_ROW_COMMANDDESCRIPTION).Add(commandClass.Description))
					' -------------------------------------

					' ---- Command Method Description ----
					If Not commandMethod Is Nothing Then command_Rows.Add(New Row().Add(HELP_ROW_METHODDESCRIPTION).Add(commandMethod.Description))
					' ------------------------------------

					' ---- Command Method Details ----
					If Not commandMethod Is Nothing AndAlso Not commandMethod.FurtherDetails Is Nothing Then

						Dim details As Array = TypeAnalyser.ConvertToArray(commandMethod.FurtherDetails)

						For i As Integer = 0 To details.Length - 1

							If i = 0 Then

								command_Rows.Add(New Row().Add(HELP_COLUMN_DETAILS).Add(details(i)))

							Else

								command_Rows.Add(New Row().Add().Add(details(i)))

							End If

						Next

					End If
					' --------------------------------

					' ---- Interrogate Command Flags ----
					For Each commandFlag As CommandInterrogatedFlag In commandClass.Flags

						Dim commandFlagDetails As String = commandFlag.Name
						If Not String.IsNullOrEmpty(commandFlag.FlagAttribute.ArgsDescription) Then _
							commandFlagDetails = commandFlagDetails & COLON & commandFlag.FlagAttribute.ArgsDescription

						command_Rows.Add(New Row().Add(commandFlagDetails).Add(commandFlag.FlagAttribute.Description))

						If commandFlag.FieldType.IsEnum Then command_Rows.Add(New Row().Add().Add(GetEnumValuesAsLine(commandFlag.FieldType)))

					Next
					' -----------------------------------

					Return Cube.Create(IT.Help, HELP_COMMAND_DETAILS, HELP_COLUMN_TITLE, HELP_COLUMN_DESCRIPTION).Add(New Slice(command_Rows))

				End Function

				''' <summary>
				''' Method to Generate Help Details for a Type.
				''' </summary>
				''' <param name="value">The Type To Output</param>
				''' <returns></returns>
				''' <remarks></remarks>
				Public Shared Function GenerateTypeHelp( _
					ByVal value As Type, _
					Optional titlePrefix As String = Nothing _
				) As IFixedWidthWriteable

					If value.IsEnum Then ' CASE 1: Type is an Enum

						' ---- Create the Rows ----
						Dim enum_Rows As New List(Of Row)
						' -------------------------

						' ---- Interrogate the Enum ----
						Dim enumType As Type = [Enum].GetUnderlyingType(value)
						Dim values As Array = [Enum].GetValues(value)

						For i As Integer = 0 To values.Length - 1

							enum_Rows.Add(New Row().Add([Enum].GetName(value, values(i))).Add(Convert.ChangeType(values(i), enumType)))

						Next
						' ------------------------------

						If String.IsNullOrEmpty(titlePrefix) Then

							Return Cube.Create(IT.Help, HELP_ENUM_DETAILS, HELP_COLUMN_ENUM_NAME, HELP_COLUMN_ENUM_VALUE).Add(New Slice(enum_Rows))

						Else

							Return Cube.Create(IT.Help, String.Format("{0}: {1}", titlePrefix, HELP_ENUM_DETAILS), HELP_COLUMN_ENUM_NAME, HELP_COLUMN_ENUM_VALUE) _
								.Add(New Slice(enum_Rows))

						End If

					Else ' CASE 2: Type is a Normal Type

						' ---- Create the Rows ----
						Dim type_Rows As New List(Of Row)
						' -------------------------

						Dim props As PropertyInfo() = value.GetProperties(BindingFlags.Instance Or BindingFlags.Public)

						Dim attributeTypes As New Hashtable

						For i As Integer = 0 To props.Length - 1

							Dim prop_Analyser As TypeAnalyser = TypeAnalyser.GetInstance(props(i).PropertyType)

							Dim prop_Row As New Row()

							' ---- Property Name ----
							prop_Row.Add(props(i).Name).Add(prop_Analyser.Name)
							' -----------------------

							' ---- Property Type ----
							Dim typeDetails As New TX.StringBuilder

							If prop_Analyser.IsSimple OrElse prop_Analyser.IsSimpleArray Then typeDetails.Append(HELP_FLAG_SIMPLE)
							If prop_Analyser.IsArray Then typeDetails.Append(HELP_FLAG_ARRAY)
							If prop_Analyser.IsICollection Then typeDetails.Append(HELP_FLAG_COLLECTION)
							If prop_Analyser.IsIList Then typeDetails.Append(HELP_FLAG_LIST)

							' ---- Property Accessibility ----
							If props(i).CanRead Then typeDetails.Append(HELP_FLAG_READABLE)
							If props(i).CanWrite Then typeDetails.Append(HELP_FLAG_WRITABLE)
							' --------------------------------

							prop_Row.Add(typeDetails.ToString)
							' -----------------------

							' ---- Property Parameters ----
							prop_Row.Add(props(i).GetIndexParameters.Length)
							' -----------------------------

							' ---- Property Declared In ----
							prop_Row.Add(props(i).DeclaringType.Name)
							' ------------------------------

							' ---- Property Attributes ----
							Dim attrs As Object() = props(i).GetCustomAttributes(True)
							Dim typeAttributes As New System.Text.StringBuilder

							' ---- Interrogate the Attributes ----
							For j As Integer = 0 To attrs.Length - 1

								Dim attributeFlag As String = Nothing

								If attributeTypes.ContainsKey(attrs(j).GetType) Then

									attributeFlag = attributeTypes(attrs(j).GetType)

								Else

									Dim index As Integer = 0
									attributeFlag = attrs(j).GetType.Name.ToUpper.Substring(index, 1)

									Do Until Not attributeTypes.ContainsValue(attributeFlag)

										If (index + 1) = attrs(j).GetType.Name.Length Then

											If attributeFlag.StartsWith(SQUARE_BRACKET_START) AndAlso attributeFlag.EndsWith(SQUARE_BRACKET_END) Then

												attributeFlag = SQUARE_BRACKET_START & (Integer.Parse(attributeFlag.Trim( _
													New Char() {SQUARE_BRACKET_START, SQUARE_BRACKET_END})) + 1).ToString & SQUARE_BRACKET_END

											Else

												attributeFlag = SQUARE_BRACKET_START & FULL_STOP & SQUARE_BRACKET_END

											End If

										Else

											index += 1
											attributeFlag = attrs(j).GetType.Name.ToUpper.Substring(index, 1)

										End If

									Loop

									attributeTypes.Add(attrs(j).GetType, attributeFlag)

								End If

								If Not typeAttributes.ToString.Contains(attributeFlag) Then typeAttributes.Append(attributeFlag)

							Next
							' ------------------------------------

							type_Rows.Add(prop_Row.Add(typeAttributes.ToString))

						Next

						' ---- Add the Details Key ----
						type_Rows.Add(New Row()) ' Spacing Row

						type_Rows.Add(New Row().Add(HELP_COLUMN_DETAILS & SPACE & HELP_ROW_KEY))

						Dim flags As String() = New String() {HELP_FLAG_ARRAY, HELP_FLAG_COLLECTION, HELP_FLAG_LIST, HELP_FLAG_READABLE, HELP_FLAG_SIMPLE, _
						HELP_FLAG_WRITABLE}

						Dim flagDescriptions As String() = New String() {HELP_FLAG_ARRAY_DESCRIPTION, HELP_FLAG_COLLECTION_DESCRIPTION, _
						HELP_FLAG_LIST_DESCRIPTION, HELP_FLAG_READABLE_DESCRIPTION, HELP_FLAG_SIMPLE_DESCRIPTION, HELP_FLAG_WRITABLE_DESCRIPTION}

						For i As Integer = 0 To flags.Length - 1

							type_Rows.Add(New Row().Add(flags(i)).Add(flagDescriptions(i)))

						Next
						' -----------------------------

						' ---- Attributes Flag Key ----
						If attributeTypes.Count > 0 Then

							type_Rows.Add(New Row()) ' Spacing Row
							type_Rows.Add(New Row().Add(HELP_COLUMN_FLAGS & SPACE & HELP_ROW_KEY))

							For Each key As Type In attributeTypes.Keys

								type_Rows.Add(New Row().Add(attributeTypes(key)).Add(key.Name & SPACE & SPACE & GetAssemblyDisplayName(key.Assembly)))

							Next

						End If
						' ------------------------------

						If String.IsNullOrEmpty(titlePrefix) Then

							Return Cube.Create(IT.Help, value.FullName, HELP_COLUMN_PROPERTY, HELP_COLUMN_TYPE, HELP_COLUMN_DETAILS, HELP_COLUMN_ARGS, _
								HELP_COLUMN_DECLAREDIN, HELP_COLUMN_FLAGS).Add(New Slice(type_Rows))

						Else

							Return Cube.Create(IT.Help, String.Format("{0}: {1}", titlePrefix, value.FullName), HELP_COLUMN_PROPERTY, HELP_COLUMN_TYPE, _
								HELP_COLUMN_DETAILS, HELP_COLUMN_ARGS, HELP_COLUMN_DECLAREDIN, HELP_COLUMN_FLAGS).Add(New Slice(type_Rows))

						End If

					End If

				End Function

			#End Region

		#End Region

	End Class

End Namespace
