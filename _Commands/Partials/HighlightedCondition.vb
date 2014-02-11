Imports Leviathan.Visualisation
Imports System.Xml.Serialization

Namespace Commands

	Partial Public Class HighlightedCondition

		#Region " Public Properties "

			''' <summary>
			''' Whether it's Conditional
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Public ReadOnly Property Conditional() As System.Boolean
				Get
					Return Not String.IsNullOrEmpty(Condition)
				End Get
			End Property

		#End Region

		#Region " Public Methods "

			Public Function CheckCondition( _
				ByVal value As Object, _
				ByVal type As System.Type _
			) As Boolean

				If Not Conditional Then Return True

				If type.IsEnum Then

					Return String.Compare([Enum].GetName(type, value), Condition, True) = 0

				ElseIf type Is GetType(String) Then

					If Not String.IsNullOrEmpty(value) Then Return String.Compare(value, Condition, True) = 0

				Else

					If Not value Is Nothing AndAlso Not String.IsNullOrEmpty(value.ToString) Then _
						Return String.Compare(value.ToString, Condition, True) = 0

				End If

				Return False

			End Function

		#End Region

		#Region " Public Shared Methods "

			''' <summary>
			''' Method to Attempt a Parse From String for the Class.
			''' </summary>
			''' <param name="value">The Value to Parse.</param>
			''' <param name="result">The ByRef/Out Parameter to write the parsed object to.</param>
			''' <returns>A Boolean indicating whether the parse was successful.</returns>
			''' <remarks></remarks>
			Public Overloads Shared Function TryParse( _
				ByVal value As String, _
				ByRef result As HighlightedCondition _
			) As Boolean

				If Not String.IsNullOrEmpty(value) Then

					Dim l_StringCondition As String = Nothing
					Dim l_StringType As String = value

					If value.Contains(FORWARD_SLASH) Then

						l_StringCondition = value.Split(FORWARD_SLASH)(0)
						l_StringType = l_StringType.Split(FORWARD_SLASH)(1)

					End If

					Dim parsedType As Boolean

					Dim l_Type As InformationType = New EnumConvertor().ParseEnumFromString(l_StringType, parsedType, GetType(InformationType))

					If parsedType Then

						result = New HighlightedCondition(l_StringCondition, l_Type)

						Return True

					End If

				End If

				Return False

			End Function

		#End Region

	End Class

End Namespace