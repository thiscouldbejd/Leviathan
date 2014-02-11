Imports Leviathan.Comparison.Comparer

Namespace Commands

	Public Class SuggestedCommandHandler

		#Region " Public Events "

			''' <summary>
			''' This Event is Fired when the Foreground Display Text needs to be updated.
			''' </summary>
			''' <param name="value"></param>
			''' <remarks></remarks>
			Public Event UpdateInput( _
				ByVal value As String, _
				ByVal position As Integer _
			)

			''' <summary>
			''' This Event is Fired when the Background Display Text needs to be updated.
			''' </summary>
			''' <param name="value"></param>
			''' <remarks></remarks>
			Public Event UpdateOutput( _
				ByVal value As String _
			)

			''' <summary>
			''' This Event is Fired when a Command should be executed.
			''' </summary>
			''' <param name="value"></param>
			''' <remarks></remarks>
			Public Event ExecuteCommand( _
				ByVal value As String _
			)

			''' <summary>
			''' This Event is Fired when the Settings for the Handler Change.
			''' </summary>
			''' <param name="suggestionsEnabled"></param>
			''' <remarks></remarks>
			Public Event UpdateSettings( _
				ByVal suggestionsEnabled As Boolean _
			)

		#End Region

		#Region " Input Event Handlers "

			Private Sub Input_Change( _
				ByVal value As SuggestedCommandInput _
			) Handles m_Input.Changed

				RaiseEvent UpdateInput(value.Value, value.Position)

				If SuggestionsEnabled Then Output.Resuggest(value.ParsedValue)

			End Sub

		#End Region

		#Region " Output Event Handlers "


			Private Sub Output_Change( _
				ByVal value As SuggestedCommandOutput _
			) Handles m_Output.Changed

				' This method keeps ins/outs in sync with quotes and spaces etc
				Dim _output As String = value.Value

				If Not String.IsNullOrEmpty(m_Input.Value) AndAlso _
					Not String.IsNullOrEmpty(_output) Then

					Dim _input As Char() = m_Input.Value.ToCharArray()

					For i As Integer = 0 To _input.Length - 1

						For j As Integer = 0 To RetainedArguments.Length - 1

							If _input(i) = RetainedArguments(j) Then

								If _output.Length = i Then

									_output = _output & CStr(_input(i))

								ElseIf _output.Length > i AndAlso _output.SubString(i, 1) <> CStr(RetainedArguments(j)) AndAlso _
									Not (j > 0  AndAlso (_output.SubString(i, 1) = SQUARE_BRACKET_START OrElse _output.SubString(i, 1) = SQUARE_BRACKET_END)) Then

										_output = _output.Insert(i, CStr(_input(i)))

								End If

							End If
						
						Next
						
					Next

				End If

				RaiseEvent UpdateOutput(_output)

			End Sub

		#End Region

		#Region " Public Methods "

			Public Sub ProcessInput( _
				ByVal state As SuggestedCommandState _
			)

				Select Case state.CommandKey

					Case SuggestedCommandKey.Input

						Input.SetValue(state.InputValue, state.InputPosition)

					Case SuggestedCommandKey.Up

						Output.Cycle(-1)

					Case SuggestedCommandKey.Down

						Output.Cycle(1)

					Case SuggestedCommandKey.Right

						If Not String.IsNullOrEmpty(Output.Value) AndAlso _
							Input.HasValue AndAlso Input.AtEnd AndAlso _
							Output.Value.Length > Input.Length Then _
								Input.AppendValue(Output.Value.Substring(Input.Length, 1))

					Case SuggestedCommandKey.ReverseTab

						If Not String.IsNullOrEmpty(Output.Value) AndAlso _
							Input.HasValue AndAlso Input.AtEnd Then

							Dim pos As Integer = Input.Position - 1
							Dim previousDivider As Integer

							If Output.Value.Substring(pos).StartsWith(SQUARE_BRACKET_END) Then

								previousDivider = Output.Value.SubString(0, pos).LastIndexOfAny(New Char() {SQUARE_BRACKET_START}) - 1

							Else

								previousDivider = Output.Value.SubString(0, pos).LastIndexOfAny(New Char() {HYPHEN, SPACE})
								If previousDivider > 0 Then previousDivider += 1

							End If

							Input.Remove(previousDivider - Input.Position)

						End If

					Case SuggestedCommandKey.Tab

						If Not String.IsNullOrEmpty(Output.Value) AndAlso _
							Input.HasValue AndAlso Input.AtEnd AndAlso _
								Output.Value.Length > Input.Length Then

							Dim pos As Integer = Input.Position
							Dim nextDivider As Integer

							If Output.Value.Substring(pos).StartsWith(SQUARE_BRACKET_START) OrElse _
							 Output.Value.Substring(pos + 1).StartsWith(SQUARE_BRACKET_START) Then

							 	' TODO: This needs to have partial arguments e.g. when you back delete part of an arg [Cache Id] --> [Cache
								nextDivider = Output.Value.IndexOfAny(New Char() {SQUARE_BRACKET_END}, pos) + 1

							Else

								nextDivider = Output.Value.IndexOfAny(New Char() {HYPHEN, SPACE}, pos)
								If nextDivider < Output.Value.Length Then nextDivider += 1

							End If

							If nextDivider > 0 Then

								Input.AppendValue(Output.Value.Substring(Input.Length, nextDivider - Input.Length))

							Else

								' Must be the final part, so append to the end.
								Input.AppendValue(Output.Value.Substring(Input.Length))

							End If

						End If

					Case SuggestedCommandKey.Help

						If ThreadSafeUpdateSuggestionsEnabled(Not SuggestionsEnabled) Then _
							RaiseEvent UpdateSettings(SuggestionsEnabled)

						If Not SuggestionsEnabled Then

							Output.Clear()

						Else

							Input.SetValue(state.InputValue, state.InputPosition)

						End If

					Case SuggestedCommandKey.Enter

						If Not String.IsNullOrEmpty(Input.Value) Then

							Dim l_CommandToExecute As String = Input.Value

							Input.Clear()

							RaiseEvent ExecuteCommand(l_CommandToExecute)

						End If

				End Select

			End Sub

		#End Region

	End Class

End Namespace