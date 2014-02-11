Imports Leviathan.Visualisation

Namespace Commands

	Partial Public Class CommandFileOutput
		Implements ICommandsOutput
		Implements ICommandsOutputWriter

		#Region " ICommandsOutput Implementation "

			Private Sub Write_Outputs( _
				ParamArray ByVal values As IFixedWidthWriteable() _
			) Implements ICommandsOutput.Show_Outputs

				If Not values Is Nothing Then

					For i As Integer = 0 To values.Count - 1

						If HasWritten OrElse i > 0 Then TerminateLine()
						HasWritten = True

						values(i).Write(CharacterWidth, Me, 0)

					Next

				End If

			End Sub

			Private Sub Write_Close() Implements ICommandsOutput.Close

				For i As Integer = 0 To ToFiles.Length - 1

					ToFiles(i).Close()

				Next

			End Sub

		#End Region

		#Region " ICommandsOutputWriter Implementation "

			Private Sub Write( _
				ByVal value As String, _
				ByVal type As InformationType _
			) _
			Implements ICommandsOutputWriter.Write

				If Not String.IsNullOrEmpty(value) Then

					For i As Integer = 0 To ToFiles.Length - 1

						If ToFiles(i).CanWrite Then ToFiles(i).Write(Encoder.GetBytes(value), 0, Encoder.GetByteCount(value))

					Next

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

					Write(Environment.NewLine, InformationType.None)

				Next

			End Sub

		#End Region

	End Class

End Namespace
