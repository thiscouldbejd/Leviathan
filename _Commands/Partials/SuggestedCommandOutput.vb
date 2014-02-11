Imports Leviathan.Comparison.Comparer
Namespace Commands

	Public Class SuggestedCommandOutput

		#Region " Public Events "

			''' <summary>
			''' This Event is Fired when the Value/Position of this Input has been changed.
			''' </summary>
			''' <param name="value"></param>
			''' <remarks></remarks>
			Public Event Changed( _
				ByVal value As SuggestedCommandOutput _
			)

		#End Region

		#Region " Private Properties "

			''' <summary>
			''' The Selected Suggestion.
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Private ReadOnly Property Suggestion() As String
				Get
					If Not Suggestions Is Nothing AndAlso Suggestions.Length > 0 Then
						Return Suggestions(Index)
					Else
						Return Nothing
					End If
				End Get
			End Property

		#End Region

		#Region " Public Methods "

			''' <summary>
			''' Method to Cycle through the suggestions.
			''' </summary>
			''' <param name="increment"></param>
			''' <remarks></remarks>
			Public Sub Cycle( _
				ByVal increment As Integer _
			)

				If Not Suggestions Is Nothing AndAlso increment <> 0 Then

					ThreadSafeUpdateIndex(CalculateCycleShift(Suggestions.Length, Index, increment))

					If ThreadSafeUpdateValue(Suggestion) Then RaiseEvent Changed(Me)

				End If

			End Sub

			''' <summary>
			''' Method to Resuggest.
			''' </summary>
			''' <param name="value"></param>
			''' <remarks></remarks>
			Public Sub Resuggest( _
				ByVal value As String() _
			)

				SyncLock Me

					If value Is Nothing OrElse value.Length = 0 Then

						Clear()

					Else

						If Not value Is InputValue Then

							Dim l_CurrentSuggestion As String = Suggestion

							Dim l_NewSuggestions As String() = Nothing

							If Config.Matcher.SuggestFromArgs(value, l_NewSuggestions) Then

								Dim new_Index As Integer = 0

								For i As Integer = 0 To l_NewSuggestions.Length - 1

									If String.Compare(l_NewSuggestions(i), l_CurrentSuggestion) = 0 Then

										new_Index = i
										Exit For

									End If

								Next

								If (ThreadSafeUpdateIndex(new_Index) Or ThreadSafeUpdateSuggestions(l_NewSuggestions)) AndAlso _
									ThreadSafeUpdateValue(Suggestion) Then RaiseEvent Changed(Me)

							Else

								Clear()

							End If

						End If

						ThreadSafeUpdateInputValue(value)

					End If

				End SyncLock

			End Sub

			''' <summary>
			''' Method to Clear the Text Value/Position.
			''' </summary>
			''' <remarks></remarks>
			Public Sub Clear()

				ThreadSafeUpdateIndex(Nothing)
				ThreadSafeUpdateSuggestions(Nothing)

				If ThreadSafeUpdateValue(Nothing) Then RaiseEvent Changed(Me)

			End Sub

		#End Region

		#Region " Friend Shared Methods "

			''' <summary>
			''' Method to Calculate the Cycle Offset for a given Increment (e.g. Cycle through a list).
			''' </summary>
			''' <param name="cycleLength">The length of the cycle, e.g. List Length.</param>
			''' <param name="currentPosition">The current position in the cycle (zero-based)</param>
			''' <param name="increment">The requested increment (can be positive or negative).</param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function CalculateCycleShift( _
				ByVal cycleLength As Integer, _
				ByVal currentPosition As Integer, _
				ByVal increment As Integer _
			) As Integer

				If (increment / cycleLength) >= 1 OrElse _
					(increment / cycleLength) <= -1 Then _
						Math.DivRem(increment, cycleLength, increment)

				If currentPosition + increment < 0 Then

					' Increment must be negative
					Return cycleLength + (currentPosition + increment)

				ElseIf currentPosition + increment >= cycleLength Then

					' Increment must be positive
					Return cycleLength - (currentPosition + increment)

				Else

					' Increment is within one cycles bound.
					Return currentPosition + increment

				End If

			End Function

		#End Region

	End Class

End Namespace