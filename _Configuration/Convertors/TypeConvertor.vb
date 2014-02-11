Imports Leviathan.Caching
Namespace Configuration

	Public Class TypeConvertor

		#Region " Public Constants "

			''' <summary>
			''' Public Constant Reference to the Name of the ParseTypeFromString Method.
			''' </summary>
			''' <remarks></remarks>
			Public Const METHOD_PARSETYPEFROMSTRING As String = "ParseTypeFromString"

		#End Region

		#Region " Public Methods "

			''' <summary>
			''' Private Method Handling the Parsing of a Type Type.
			''' </summary>
			''' <param name="value">The String Value to Parse.</param>
			''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successfull or not.</param>
			''' <returns>The Parsed Object or Nothing.</returns>
			''' <remarks></remarks>
			Public Function ParseTypeFromString( _
				<ParsingInParameter()> ByVal value As String, _
				<ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
				<ParsingTypeParameter()> ByVal typeToParseTo As Type _
			) As Object

				Dim cache As Simple = Simple.GetInstance(GetType(TypeConvertor).GetHashCode)

				Dim ret_Type As Type = Nothing

				If Not cache.TryGet(ret_Type, METHOD_PARSETYPEFROMSTRING.GetHashCode, value.GetHashCode) Then

					ret_Type = System.Type.GetType(value, False, True)

					If ret_Type Is Nothing Then

						Dim isArray As Boolean = value.EndsWith(SQUARE_BRACKET_START & SQUARE_BRACKET_END)

						If isArray Then value = value.TrimEnd(SQUARE_BRACKET_START, SQUARE_BRACKET_END)

						Dim search_Assembly As Assembly = Reflection.Assembly.GetEntryAssembly

						If search_Assembly Is Nothing Then _
							search_Assembly = Reflection.Assembly.GetCallingAssembly

						Dim types As System.Type() = Nothing

						If Not search_Assembly Is Nothing Then _
							types = AssemblyAnalyser.GetInstance(search_Assembly).FindType(value)

						If Not types Is Nothing AndAlso types.Length = 1 Then

							ret_Type = types(0)
							If isArray Then
								ret_Type = ret_Type.MakeArrayType
								value = value & SQUARE_BRACKET_START & SQUARE_BRACKET_END
							End If

						End If

					End If

					If ret_Type Is Nothing Then

						successfulParse = False
						Return Nothing

					Else

						cache.Set(ret_Type, METHOD_PARSETYPEFROMSTRING.GetHashCode, value.GetHashCode)

						successfulParse = True
						Return ret_Type

					End If

				Else

					successfulParse = True
					Return ret_Type

				End If

			End Function

		#End Region

	End Class

End Namespace
