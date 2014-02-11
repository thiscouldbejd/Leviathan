Imports Leviathan.Comparison.Comparer

Namespace Commands

	Public Class SuggestedCommandInput

		#Region " Public Events "

			''' <summary>
			''' This Event is Fired when the Value/Position of this Input has been changed.
			''' </summary>
			''' <param name="value"></param>
			''' <remarks></remarks>
			Public Event Changed( _
				ByVal value As SuggestedCommandInput _
			)

		#End Region

		#Region " Public Properties "

			''' <summary>
			''' Length of the Text Value
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Public ReadOnly Property Length() As System.Int32
				Get
					If HasValue Then Return Value.Length Else Return 0
				End Get
			End Property

			''' <summary>
			''' Whether the Cursor is at the Start of the Text Value
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Public ReadOnly Property AtStart() As System.Int32
				Get
					Return Position = 0
				End Get
			End Property

			''' <summary>
			''' Whether the Cursor is at the End of the Text Value
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Public ReadOnly Property AtEnd() As System.Boolean
				Get
					Return Position = Value.Length
				End Get
			End Property

			''' <summary>
			''' Whether the Cursor is at the Middle of the Text Value
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Public ReadOnly Property InMiddle() As System.Boolean
				Get
					Return Not String.IsNullOrEmpty(Value)
				End Get
			End Property

			''' <summary>
			''' Whether the Object has a Text Value
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Public ReadOnly Property HasValue() As System.Boolean
				Get
					Return Position = Value.Length
				End Get
			End Property

		#End Region

		#Region " Public Methods "

			''' <summary>
			''' Method to set the Input Value.
			''' </summary>
			''' <param name="value"></param>
			''' <remarks></remarks>
			Public Sub SetValue( _
				ByVal value As String _
			)

				If String.IsNullOrEmpty(value) Then

					SetValue(value, 0)

				Else

					SetValue(value, value.Length)

				End If

			End Sub

			''' <summary>
			''' Method to set the Input Value/Position.
			''' </summary>
			''' <param name="value"></param>
			''' <param name="position"></param>
			''' <remarks></remarks>
			Public Sub SetValue( _
				ByVal value As String, _
				ByVal position As Integer _
			)

				If ThreadSafeUpdatePosition(position) Or _
					(ThreadSafeUpdateValue(value) AndAlso ThreadSafeUpdateParsedValue( _
						New CommandArgumentsParser().ParseLine(value) _
							) _
						) Then RaiseEvent Changed(Me)

			End Sub

			''' <summary>
			''' Method to append a String to the Value.
			''' </summary>
			''' <param name="_value">The value to append.</param>
			''' <remarks>The text will be appended at the current position and the position will be incremented.</remarks>
			Public Sub AppendValue( _
				ByVal _value As String _
			)

				If Not String.IsNullOrEmpty(_value) Then

					If AtEnd Then

						SetValue(Value & _value, Position + _value.Length)

					Else

						SetValue(Value.Insert(Position, _value), Position + _value.Length)

					End If

				End If

			End Sub

			''' <summary>
			''' Method to Remove Characters from the Value at the current point.
			''' </summary>
			''' <param name="count">Number of Characters to Remove (negative to remove backwards, position forwards)</param>
			''' <remarks></remarks>
			Public Sub Remove( _
				Byval count As Integer _
			)

				If count < 0 Then

					If Not AtStart Then

						If Position + count < 0 Then count = 0 - Position

						SetValue(Value.Remove(Position + count, 0 - count))

					End If

				Else

					If Not AtEnd Then

						If Position + count > Value.Length Then count = Value.Length - Position

						SetValue(Value.Remove(Position, count))

					End If

				End If

			End Sub

			''' <summary>
			''' Method to Increment the Cursor Position by a defined value.
			''' </summary>
			''' <param name="increment">The value to increment by.</param>
			''' <remarks></remarks>
			Public Sub IncrementPosition( _
				ByVal increment As Integer _
			)

				If Not AtEnd Then

					If Not Position + increment > Length AndAlso _
						Not Position + increment < 0 Then

						SetValue(Value, Position + increment)

					End If

				End If

			End Sub

			''' <summary>
			''' Method to Clear the Text Value/Position.
			''' </summary>
			''' <remarks></remarks>
			Public Sub Clear()

				If ThreadSafeUpdateValue(Nothing) Or _
					ThreadSafeUpdatePosition(Nothing) Or _
						ThreadSafeUpdateParsedValue(Nothing) Then _
							RaiseEvent Changed(Me)

			End Sub

		#End Region

	End Class

End Namespace