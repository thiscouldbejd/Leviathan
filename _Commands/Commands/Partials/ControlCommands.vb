Imports Leviathan.Caching
Imports Leviathan.Visualisation
Imports IT = Leviathan.Visualisation.InformationType
Imports System.Diagnostics

Namespace Commands

	Partial Public Class ControlCommands

		#Region " Private Methods "

			''' <summary>
			''' Method to Act as a Parsing Callback for the Parser/Command Arguments.
			''' </summary>
			''' <param name="value">The Value to Parse.</param>
			''' <param name="successfulParse">Whether the Parse was Successful.</param>
			''' <returns>The Parsed Object.</returns>
			''' <remarks></remarks>
			Private Function ProcessIterationAsParsingMethod( _
				ByVal value As String, _
				ByRef successfulParse As Boolean, _
				ByVal typeToParseTo As Type _
			) As Object

				If value = ASTERISK Then

					successfulParse = True
					Return IterationSource

				Else

					Dim retObject As Object = Nothing

					Dim analyser As TypeAnalyser = _
						TypeAnalyser.GetInstance(IterationSource.GetType)

					Dim members As MemberAnalyser() = _
						analyser.ExecuteQuery(AnalyserQuery.QUERY_MEMBERS_READABLE _
							.SetName(value) _
						)

					If Not members Is Nothing AndAlso members.Length = 1 Then

						retObject = members(0).Read(IterationSource)

						'TODO: Is this correct? Should Blank cells return Null?
						If Not retObject Is DBNull.Value Then
							successfulParse = True
						Else
							successfulParse = False
						End If

					Else

						successfulParse = False

					End If

					If Not retObject Is Nothing AndAlso (retObject.GetType Is GetType(String) _
						OrElse retObject.GetType.IsArray AndAlso CType(retObject, Array).Length > 0 AndAlso _
						CType(retObject, Array)(0).GetType Is GetType(String)) Then

						If retObject.GetType.IsArray AndAlso Not typeToParseTo.IsArray Then

							Return Host.StringParser.Parse(CType(retObject, Array)(0), successfulParse, typeToParseTo)

						Else

							Return Host.StringParser.Parse(retObject, successfulParse, typeToParseTo)

						End If

					Else

						Return retObject

					End If

				End If

			End Function

			''' <summary>
			''' Method to Prepare an Iteration by created the Array and Adding a Parsing Method.
			''' </summary>
			''' <param name="source">The Source Items from which to create the Array.</param>
			''' <returns>An Array of Source Items.</returns>
			''' <remarks></remarks>
			Private Function PrepareIteration( _
				ByVal source As Object _
			) As Object

				If Not source Is Nothing Then

					Dim arySource As Array = CreateArray(source)

					Host.StringParser.AddParsingMethod(ParameterRegex, _
						New FromString.ParsingMethod(AddressOf ProcessIterationAsParsingMethod))

					Return arySource

				Else

					Return Nothing

				End If

			End Function

			''' <summary>
			''' Method to Complete an Iteration by Removing a Parsing Method.
			''' </summary>
			''' <remarks></remarks>
			Private Sub CompleteIteration()

				Host.StringParser.RemoveParsingMethod(ParameterRegex)

			End Sub

		#End Region

		#Region " Conditional Command Methods "

			<Command( _
				ResourceContainingType:=GetType(ControlCommands), _
				ResourceName:="CommandDetails", _
				Name:="conditional-success", _
				Description:="@commandControlDescriptionConditionalSuccess@" _
			)> _
			Public Sub ProcessCommandConditionalSuccess( _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterDescriptionExecutionCommand@" _
				)> _
				ByVal command As String, _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterDescriptionSource@" _
				)> _
				ByVal source As Object _
			)

				If Not source Is Nothing AndAlso source.GetType Is GetType(Boolean) AndAlso _
					CType(source, Boolean) = True Then Host.Execute(command)

			End Sub

			<Command( _
				ResourceContainingType:=GetType(ControlCommands), _
				ResourceName:="CommandDetails", _
				Name:="conditional-failure", _
				Description:="@commandControlDescriptionConditionalFailure@" _
			)> _
			Public Sub ProcessCommandConditionalFailure( _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterDescriptionExecutionCommand@" _
				)> _
				ByVal command As String, _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterDescriptionSource@" _
				)> _
				ByVal source As Object _
			)

				If Not source Is Nothing AndAlso source.GetType Is GetType(Boolean) AndAlso _
					CType(source, Boolean) = False Then Host.Execute(command)

			End Sub

			<Command( _
				ResourceContainingType:=GetType(ControlCommands), _
				ResourceName:="CommandDetails", _
				Name:="conditional-exists", _
				Description:="@commandControlDescriptionConditionalExists@" _
			)> _
			Public Sub ProcessCommandExists( _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterDescriptionExecutionCommand@" _
				)> _
				ByVal command As String, _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterDescriptionSource@" _
				)> _
				ByVal source As Object _
			)

				If Not source Is Nothing Then Host.Execute(command)

			End Sub

		#End Region

		#Region " Iteration Command Methods "

			<Command( _
				ResourceContainingType:=GetType(ControlCommands), _
				ResourceName:="CommandDetails", _
				Name:="iterate", _
				Description:="@commandControlDescriptionIterate@" _
			)> _
			Public Sub ProcessCommandIterate( _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterDescriptionExecutionCommand@" _
				)> _
				ByVal command As String, _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterDescriptionSource@" _
				)> _
				ByVal source As Object _
			)

				Dim sources As Array = PrepareIteration(source)

				If Not sources Is Nothing Then

					For i As Integer = 0 To sources.Length - 1

						IterationSource = sources(i)

						Host.Execute(command)

					Next

					CompleteIteration()

				End If

			End Sub

			<Command( _
				ResourceContainingType:=GetType(ControlCommands), _
				ResourceName:="CommandDetails", _
				Name:="iterate-aggregate", _
				Description:="@commandControlDescriptionIterateAggregate@" _
			)> _
			Public Function ProcessCommandAggregate( _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterDescriptionExecutionCommand@" _
				)> _
				ByVal command As String, _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterDescriptionSource@" _
				)> _
				ByVal source As Object _
			) As IList

				Dim lstReturn As ArrayList = Nothing

				Dim sources As Array = PrepareIteration(source)

				If Not sources Is Nothing Then

					lstReturn = New ArrayList

					For i As Integer = 0 To sources.Length - 1

						IterationSource = sources(i)

						Dim l_returnObject As Object = Host.Execute(command)

						If Not l_returnObject Is Nothing Then

							If TypeAnalyser.GetInstance(l_returnObject.GetType).IsICollection Then

								lstReturn.AddRange(l_returnObject)

							Else

								lstReturn.Add(l_returnObject)

							End If

						End If

					Next

					CompleteIteration()

				End If

				Return lstReturn

			End Function

		#End Region

		#Region " Interval Command Methods "

			<Command( _
				ResourceContainingType:=GetType(ControlCommands), _
				ResourceName:="CommandDetails", _
				Name:="interval", _
				Description:="@commandControlDescriptionInterval@" _
			)> _
			Public Sub ProcessCommandInterval( _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterDescriptionExecutionCommand@" _
				)> _
				ByVal command As String, _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterDescriptionIntervalSpan@" _
				)> _
				ByVal interval As TimeSpan _
			)

				ProcessCommandInterval(command, interval, 0)

			End Sub

			<Command( _
				ResourceContainingType:=GetType(ControlCommands), _
				ResourceName:="CommandDetails", _
				Name:="interval", _
				Description:="@commandControlDescriptionInterval@" _
			)> _
			Public Sub ProcessCommandInterval( _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterDescriptionExecutionCommand@" _
				)> _
				ByVal command As String, _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterDescriptionIntervalSpan@" _
				)> _
				ByVal interval As TimeSpan, _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterDescriptionIntervalRepetitions@" _
				)> _
				ByVal repetitions As Integer _
			)

				Dim l_CurrentRepitition As Integer = 1
				Dim l_SleepTime As Double = interval.TotalMilliseconds

				Do Until l_CurrentRepitition > repetitions

					System.Threading.Thread.Sleep(l_SleepTime)

					Host.Execute(command)

					l_CurrentRepitition += 1

				Loop

			End Sub

		#End Region

		#Region " Store Command Methods "

			<Command( _
				ResourceContainingType:=GetType(ControlCommands), _
				ResourceName:="CommandDetails", _
				Name:="append", _
				Description:="@commandControlDescriptionAppend@" _
			)> _
			Public Sub ProcessCommandAppend( _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterDescriptionObjects@" _
				)> _
				ByVal obj As Object, _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterDescriptionName@" _
				)> _
				ByVal name As String _
			)

				If Not obj Is Nothing AndAlso Not String.IsNullOrEmpty(name) Then

					Dim tempObject As Object = Nothing

					If Not Cache.TryGet(tempObject, name) OrElse tempObject Is Nothing Then

						Cache.[Set](obj, name)

					Else

						Dim newObject As IList = CreateList(tempObject)

						newObject = ActionFromList(newObject, CollectionAction.AddObjects, _
							CreateList(obj))

						Cache.[Set](newObject, name)

					End If

				End If

			End Sub

			<Command( _
				ResourceContainingType:=GetType(ControlCommands), _
				ResourceName:="CommandDetails", _
				Name:="put", _
				Description:="@commandControlDescriptionPut@" _
			)> _
			Public Sub ProcessCommandPut( _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterDescriptionObjects@" _
				)> _
				ByVal obj As Object, _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterDescriptionName@" _
				)> _
				ByVal name As String _
			)

				If Not obj Is Nothing AndAlso Not String.IsNullOrEmpty(name) Then _
					Cache.[Set](obj, name)

			End Sub

			<Command( _
				ResourceContainingType:=GetType(ControlCommands), _
				ResourceName:="CommandDetails", _
				Name:="get", _
				Description:="@commandControlDescriptionGet@" _
			)> _
			Public Function ProcessCommandGet( _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterDescriptionName@" _
				)> _
				ByVal name As String _
			) As Object

				Dim returnObject As Object = Nothing

				If Not String.IsNullOrEmpty(name) AndAlso Cache.TryGet(returnObject, name) Then _
					Return returnObject

				Return Nothing

			End Function

			<Command( _
				ResourceContainingType:=GetType(ControlCommands), _
				ResourceName:="CommandDetails", _
				Name:="enumerate", _
				Description:="@commandControlDescriptionEnumerate@" _
			)> _
			Public Function ProcessCommandEnumerate() As IFixedWidthWriteable

				Dim cacheList As DictionaryEntry() = Cache.Entries

				Dim result_Rows As New List(Of Row)

				For i As Integer = 0 To cacheList.Length - 1

					If cacheList(i).Value Is Nothing Then

						result_Rows.Add(New Row() _
							.Add(cacheList(i).Key) _
							.Add() _
							.Add() _
						)

					Else

						result_Rows.Add(New Row() _
							.Add(cacheList(i).Key) _
							.Add(cacheList(i).Value.GetType()) _
							.Add(cacheList(i).Value) _
						)

					End If

				Next

				Return Cube.Create(IT.Information, "Cache Contents", "Key", "Type", "Value") _
						.Add(New Slice(result_Rows))

			End Function

			<Command( _
				ResourceContainingType:=GetType(ControlCommands), _
				ResourceName:="CommandDetails", _
				Name:="evict", _
				Description:="@commandControlDescriptionEvict@" _
			)> _
			Public Sub ProcessCommandEvict( _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterDescriptionName@" _
				)> _
				ByVal name As String _
			)

				If Not String.IsNullOrEmpty(name) Then Cache.Remove(name)

			End Sub

			<Command( _
				ResourceContainingType:=GetType(ControlCommands), _
				ResourceName:="CommandDetails", _
				Name:="persist", _
				Description:="@commandControlDescriptionPersist@" _
			)> _
			Public Sub ProcessCommandPersist( _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterOutputFile@" _
				)> _
				ByVal output_Directory As IO.DirectoryInfo _
			)

				Dim cacheList As DictionaryEntry() = Cache.Entries

				For i As Integer = 0 To cacheList.Length - 1

					Dim file_Name As String = IO.Path.Combine( _
						output_Directory.FullName, _
						String.Format("{0}.xml", cacheList(i).Key))

					If Not IO.File.Exists(file_Name) Then _
						XmlSerialiser.CreateWriter(file_Name).Write(cacheList(i).Value)

				Next

			End Sub

			<Command( _
				ResourceContainingType:=GetType(ControlCommands), _
				ResourceName:="CommandDetails", _
				Name:="persist", _
				Description:="@commandControlDescriptionPersist@" _
			)> _
			Public Sub ProcessCommandPersist( _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterDescriptionName@" _
				)> _
				ByVal name As String, _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterOutputFile@" _
				)> _
				ByVal output_File As IO.FileStream _
			)

				Dim cache_Object As Object = ProcessCommandGet(name)

				If Not cache_Object Is Nothing Then _
					XmlSerialiser.CreateWriter(output_File).Write(cache_Object)

			End Sub

			<Command( _
				ResourceContainingType:=GetType(ControlCommands), _
				ResourceName:="CommandDetails", _
				Name:="reload-commands", _
				Description:="@commandControlDescriptionCommandReload@" _
			)> _
			Public Sub ProcessCommandReload()

				AssemblyEntry.Load(Host)

			End Sub

			<Command( _
				ResourceContainingType:=GetType(ControlCommands), _
				ResourceName:="CommandDetails", _
				Name:="environmental-variables", _
				Description:="@commandControlDescriptionEnvironmentalVariables@" _
			)> _
			Public Function ProcessCommandVariable( _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterVariableName@" _
				)> _
				ByVal variable_Name As String _
			) As IFixedWidthWriteable

				Dim variable_Rows As New List(Of Row)

				variable_Rows.Add(New Row().Add(variable_Name).Add(System.Environment.GetEnvironmentVariable(variable_Name, EnvironmentVariableTarget.Machine)).Add("Machine"))
				variable_Rows.Add(New Row().Add(variable_Name).Add(System.Environment.GetEnvironmentVariable(variable_Name, EnvironmentVariableTarget.Process)).Add("Process"))
				variable_Rows.Add(New Row().Add(variable_Name).Add(System.Environment.GetEnvironmentVariable(variable_Name, EnvironmentVariableTarget.User)).Add("User"))
				
				Return 	Cube.Create(InformationType.Information, "Environmental Variables Summary", "Name", "Value", "Type").Add(New Slice(variable_Rows))
				
			End Function

			<Command( _
				ResourceContainingType:=GetType(ControlCommands), _
				ResourceName:="CommandDetails", _
				Name:="cmd-execute", _
				Description:="@commandControlDescriptionCommandLineEecutable@" _
			)> _
			Public Function ProcessCommandCommandLineEecutable( _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterExecutableName@" _
				)> _
				ByVal executable_Name As String _
			) As Boolean

				Return ProcessCommandCommandLineEecutable(executable_Name, String.Empty)

			End Function

			<Command( _
				ResourceContainingType:=GetType(ControlCommands), _
				ResourceName:="CommandDetails", _
				Name:="cmd-execute", _
				Description:="@commandControlDescriptionCommandLineEecutable@" _
			)> _
			Public Function ProcessCommandCommandLineEecutable( _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterExecutableName@" _
				)> _
				ByVal executable_Name As String, _
				<Configurable( _
					ResourceContainingType:=GetType(ControlCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandControlParameterCommandLineParameters@" _
				)> _
				ByVal command_Parameters As String _
			) As Boolean

				Dim cmd_Exe As String = "c:\windows\system32\cmd.exe"

				If Not System.IO.File.Exists(executable_Name) Then

					Return False

				Else

					Dim ps As New ProcessStartInfo
					ps.FileName = cmd_Exe
					ps.RedirectStandardInput = True
					ps.RedirectStandardOutput = True
					ps.RedirectStandardError = True
					
					ps.UseShellExecute = False
					ps.CreateNoWindow = True

					Dim p As New Process
					p.StartInfo = ps

					AddHandler p.ErrorDataReceived, AddressOf cmd_Error
					AddHandler p.OutputDataReceived, AddressOf cmd_DataReceived 
					p.EnableRaisingEvents = True

					p.Start()
					p.BeginOutputReadLine()
					p.BeginErrorReadLine()

					If String.IsNullOrEmpty(command_Parameters) Then
						p.StandardInput.WriteLine(executable_Name)
					Else
						p.StandardInput.WriteLine(executable_Name & " " & command_Parameters)
					End If

					p.StandardInput.WriteLine("exit")

					'Dim output_Err As String = p.StandardError.ReadToEnd()
					'Dim output_Std As String = p.StandardOutput.ReadToEnd()
					
					p.WaitForExit()

					Dim err_Level As Integer = p.ExitCode

					If err_Level <> 0 Then

						Host.[Warn]("Error: " & err_Level)
						'If Not String.IsNullOrEmpty(output_Err) Then Host.Warn(output_Err)
						Return False

					Else

						'If Not String.IsNullOrEmpty(output_Err) Then

						''	Host.Warn(output_Err)
						
						'ElseIf Not String.IsNullOrEmpty(output_Std) Then

						''	Host.Success(output_Std)

						'End If

						Return True

					End If

				End If

			End Function

		#End Region

		#Region " Private Command Line Event Handling Procedures "

			Private Sub cmd_DataReceived( _
				ByVal sender As Object, _
				ByVal e As DataReceivedEventArgs _
			)

				If Not e.Data Is Nothing Then Host.Success(e.Data)

			End Sub

			Private Sub cmd_Error( _
				ByVal sender As Object, _
				ByVal e As DataReceivedEventArgs _
			)

				If Not e.Data Is Nothing Then Host.Warn(e.Data)
				
			End Sub

		#End Region

	End Class

End Namespace