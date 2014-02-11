Imports Leviathan.Visualisation
Imports System.Runtime.InteropServices
Imports IT = Leviathan.Visualisation.InformationType

Namespace Commands

	Partial Public Class CommandConsoleOutput
		Implements ICommandsOutput
		Implements ICommandsOutputWriter

		#Region " Console Interface Methods "

			Declare Function AttachConsole Lib "kernel32.dll" (dwProcessId as Int32) As Boolean

			Declare Function AllocConsole Lib "kernel32.dll" () As Boolean

			Declare Function FreeConsole Lib "kernel32.dll" () As Boolean

		#End Region

		#Region " Protected Constants "

			Private Shared COLOUR_GENERAL As ConsoleColor = Console.ForegroundColor

			Private Shared COLOUR_PERFORMANCE As ConsoleColor = ConsoleColor.DarkYellow

			Private Shared COLOUR_SUCCESS As ConsoleColor = ConsoleColor.Green

			Private Shared COLOUR_INFORMATION As ConsoleColor = ConsoleColor.Blue

			Private Shared COLOUR_WARNING As ConsoleColor = ConsoleColor.Yellow

			Private Shared COLOUR_ERROR As ConsoleColor = ConsoleColor.Red

			Private Shared COLOUR_DEBUG As ConsoleColor = ConsoleColor.DarkGray

			Private Shared COLOUR_LOG As ConsoleColor = ConsoleColor.DarkGray

			Private Shared COLOUR_QUESTION As ConsoleColor = ConsoleColor.White

		#End Region

		#Region " Private Methods "

			Private Sub PostConstructorCall()

				If Not AttachConsole(-1) Then

					If Create AndAlso AllocConsole() Then Active = True

				Else

					Active = True

				End If

				If Active Then
					CharacterWidth = System.Console.BufferWidth()
					Title = Console.Title
				End If

			End Sub

		#End Region

		#Region " ICommandsOutput Implementation "

			Private Sub Write_Outputs( _
				ParamArray ByVal values As IFixedWidthWriteable() _
			) Implements ICommandsOutput.Show_Outputs

				If Active AndAlso Not values Is Nothing Then

					For i As Integer = 0 To values.Count - 1

						If Not HasWritten Then TerminateLine(2)
						HasWritten = True

						values(i).Write(CharacterWidth, Me, 0)

					Next

				End If

			End Sub

			Private Sub Write_Close() Implements ICommandsOutput.Close

				If Active Then
					Console.ForegroundColor = COLOUR_GENERAL
					FreeConsole()
				End If

			End Sub

		#End Region

		#Region " ICommandsOutputWriter Implementation "

			Private Sub Write( _
				ByVal value As String, _
				ByVal type As InformationType _
			) _
			Implements ICommandsOutputWriter.Write

				If Not String.IsNullOrEmpty(value) Then

					Select Case type

						Case IT.General

							Console.ForegroundColor = COLOUR_GENERAL

						Case IT.Debug

							Console.ForegroundColor = COLOUR_DEBUG

						Case IT.Log

							Console.ForegroundColor = COLOUR_LOG

						Case IT.[Error]

							Console.ForegroundColor = COLOUR_ERROR

						Case IT.Information

							Console.ForegroundColor = COLOUR_INFORMATION

						Case IT.Performance

							Console.ForegroundColor = COLOUR_PERFORMANCE

						Case IT.Question

							Console.ForegroundColor = COLOUR_QUESTION

						Case IT.Success

							Console.ForegroundColor = COLOUR_SUCCESS

						Case IT.Warning

							Console.ForegroundColor = COLOUR_WARNING

						Case Else

							Console.ForegroundColor = COLOUR_GENERAL

					End Select

					Console.Write(value)

				End If

			End Sub

			Private Function FixedWidthWrite( _
				ByRef value As String, _
				ByVal width As Integer, _
				ByVal type as InformationType _
			) As Boolean _
			Implements ICommandsOutputWriter.FixedWidthWrite

				Dim returnValue As Boolean
				Dim lengthWritten As Integer

				If Not String.IsNullOrEmpty(value) Then _
					Write(CubeControl.MaxWidthWrite(value, width, lengthWritten, returnValue), type)

				' Write the Padding Spaces
				Write(New String(SPACE, width - lengthWritten), InformationType.None)

				Return returnValue

			End Function

			Private Sub TerminateLine( _
				Optional numberOfTimes As System.Int32 = 1 _
			) _
			Implements ICommandsOutputWriter.TerminateLine

				For i As Integer = 1 To numberOfTimes

					Console.WriteLine()

				Next

			End Sub

		#End Region

	End Class

End Namespace
