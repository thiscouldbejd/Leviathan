Imports Leviathan.Configuration

'''<summary>Entry Point for Application.</summary>
'''<remarks>Makes Decision whether to execute directly (if there are Args) or Load the Input Form (if there aren't)</remarks>
Public Module AssemblyEntry

	#Region " Public Shared Variables "

		Public Config As AssemblyConfig

	#End Region

	#Region " Public Methods "

		Public Sub Load( _
			Optional ByVal host As Leviathan.Commands.ICommandsExecution = Nothing _
		)

			Dim l_Config As New AssemblyConfig

			If Not Config Is Nothing Then l_Config.SetStarted(Config.Started).SetFinished(Config.Finished)

			ConfigurationFactory.GetInstance().Configure(l_Config)

			With l_Config

				' -- Process the Command Classes
				For i As Integer = 0 To .Sources.Length - 1

					Dim arrayToAdd As CommandInterrogatedClass() = .Sources(i).GetCommandClasses(host)
					Dim originalLength As Integer = .CommandClasses.Length
					Array.Resize(.CommandClasses, originalLength + arrayToAdd.Length)
					Array.Copy(arrayToAdd, 0, .CommandClasses, originalLength, arrayToAdd.Length)

				Next

				Array.Sort(.CommandClasses)

				' -- Process the Interrogated Commands
				Dim l_InterrogatedCommands(.CommandClasses.Length - 1) As CommandInterrogated

				For i As Integer = 0 To .CommandClasses.Length - 1

					l_InterrogatedCommands(i) = New CommandInterrogated(.CommandClasses(i))

					For j As Integer = 0 To l_InterrogatedCommands(i).Command.Methods.Length - 1

						l_InterrogatedCommands(i).AddMethodIndex(j)

					Next

				Next

				.InterrogatedCommands = l_InterrogatedCommands

				.Matcher.Prepare(.CommandClasses)

				.Loaded = DateTime.Now

			End With

			Config = l_Config

		End Sub

		Public Sub Main( _
			args As String() _
		)

			' -- Load and Configure the Command Environment --
			Load()

			' -- Process the Actual Command --
			If args Is Nothing OrElse args.Length = 0 Then

				Dim finished As Boolean = New InputInterface().ShowDialog()

			Else

				Dim executor As New CommandExecution(args)

				Dim output As New CommandConsoleOutput()
				If output.Active Then executor.Outputs.Add(output)

				executor.Run()

			End If

		End Sub

	#End Region

End Module