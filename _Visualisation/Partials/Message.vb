Namespace Visualisation

	Partial Public Class Message
		Implements IFixedWidthWriteable

		#Region " Private Methods "

			Private Sub PostConstruction()

				Dim lst_Values As New List(Of String)(Values)
				Dim max_Length As Integer = 0

				For i As Integer = 0 To lst_Values.Count - 1

					If String.IsNullOrEmpty(lst_Values(i)) Then

						lst_Values.RemoveAt(i)
						If i >= lst_Values.Count - 1 Then Exit For Else i -= 1

					Else

						lst_Values(i) = lst_Values(i).Replace(Environment.NewLine, SPACE)
						max_Length = Math.Max(max_Length, lst_Values(i).Length)

					End If

				Next

				Values = lst_Values.ToArray()

				m_DisplayTitle = String.Concat("-- ", Title, " --")

				m_MaxLength = max_Length

			End Sub

		#End Region

		#Region " ICommandsFormatted Implementation "

			Private Function FixedWidthWrite( _
				ByVal intoWidth As Integer, _
				ByRef writer As ICommandsOutputWriter, _
				ByRef idealWidth As Integer _
			) As ICommandsOutputWriter _
			Implements IFixedWidthWriteable.Write

				Dim maxLength As Integer = System.Math.Min(MaxLength, intoWidth)
				idealWidth = Math.Max(idealWidth, MaxLength)

				' Write the Messages Title
				Dim messageTitle As String = DisplayTitle

				If messageTitle.Length < intoWidth Then

					maxLength = System.Math.Max(maxLength, messageTitle.Length)
					writer.Write(messageTitle, Type)
					writer.Write(New String(HYPHEN, maxLength - messageTitle.Length), Type)

				Else

					While writer.FixedWidthWrite(messageTitle, intoWidth, Type)
					writer.TerminateLine()
					End While

				End If

				writer.TerminateLine()
				' End

				' Write the Messages
				For i As Integer = 0 To Values.Length - 1

					Dim valueToWrite As String = Values(i)

					If valueToWrite.Length < intoWidth Then

						writer.Write(valueToWrite, Type)

					Else

						While writer.FixedWidthWrite(valueToWrite, intoWidth, Type)
							writer.TerminateLine()
						End While

					End If

					writer.TerminateLine()

				Next

				Return writer

			End Function

		#End Region

		#Region " Public Shared Methods "

			Public Shared Function Create( _
				ByVal title As String, _
				ByVal type As InformationType, _
				ByVal ParamArray values As String() _
			) As Message

				If String.IsNullOrEmpty(title) Then

					Select Case type

						Case InformationType.[Debug]

							title = DEBUG_TITLE

						Case InformationType.[Error]

							title = ERROR_TITLE

						Case InformationType.Information

							title = INFORMATION_TITLE

						Case InformationType.Performance

							title = PERFORMANCE_TITLE

						Case InformationType.Question

							title = QUESTION_TITLE

						Case InformationType.Success

							title = SUCCESS_TITLE

						Case InformationType.Warning

							title = WARNING_TITLE

						Case Else

							title = String.Empty

					End Select

				End If

				Return New Message(title, type, Values)

			End Function

		#End Region

	End Class

End Namespace
