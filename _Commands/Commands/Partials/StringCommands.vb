Imports System.String
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Text.RegularExpressions.Regex

Namespace Commands

	Partial Public Class StringCommands

		#Region " Private Variables "

			Private Shared TWO_LETTER_WORDS As String() = New String() {"as", "at", "am", "an", "be", "by", "do", "go", "he", "hi", "if", "in", "is", "me", "no", "oh", "or", "up", "we"}

			Private Shared THREE_LETTER_WORDS As String() = New String() {"the", "and", "for", "are", "but", "not", "you", "all", "any", "can", "her", "was", "one", "our", "out", "day", "get", "has", "him", "his", "how", "man", "new", "now", "old", "see", "two", "way", "who", "boy", "did", "its", "let", "put", "say", "she", "too", "use", "dad", "mum", "act", "bar", "car", "dew", "eat", "far", "gym", "hey", "ink", "jet", "key", "log", "mad", "nap", "odd", "pal", "ram", "saw", "tan", "tap", "urn", "rod", "vet", "wad", "wed", "yap", "zoo", "too"}

		#End Region

		#Region " Public Command Methods "

			<Command( _
				ResourceContainingType:=GetType(StringCommands), _
				ResourceName:="CommandDetails", _
				Name:="named-format", _
				Description:="@commandStringsDescriptionNamedFormat@" _
			)> _
			Public Function NamedFormat( _
				<Configurable( _
					ResourceContainingType:=GetType(StringCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterInputFile@" _
				)> _
				ByVal input As IO.StreamReader, _
				<Configurable( _
					ResourceContainingType:=GetType(StringCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterOutputFile@" _
				)> _
				ByVal output As IO.StreamWriter, _
				<Configurable( _
					ResourceContainingType:=GetType(StringCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandStringsParameterNames@" _
				)> _
				ByVal names As System.String(), _
				<Configurable( _
					ResourceContainingType:=GetType(StringCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandStringsParameterValues@" _
				)> _
				ByVal values As System.String() _
			) As Boolean

				If names.Length < values.Length  Then

					Return False

				Else

					Dim input_String As String = input.ReadToEnd
					input.Close()
					input = Nothing

					Dim format_Objects As New System.Collections.Generic.Dictionary(Of System.String, System.Object)

					For i As Integer = 0 To names.Length - 1

						If Not String.IsNullOrEmpty(names(i)) Then

							If values.Length < (i + 1) OrElse String.IsNullOrEmpty(values(i)) Then

								format_Objects.Add(names(i), Nothing)

							Else

								format_Objects.Add(names(i), values(i))

							End If

						End If

					Next

					Dim output_String As String = Hermes.Email.Message.NamedFormat(input_String, format_Objects)

					output.Write(output_String)
					output.Close()
					output = Nothing

					Return True

				End If

			End Function

		#End Region

		#Region " Public Shared Methods "

			''' <summary>
			''' Method to Help Find the Index an any particular Char within a String.
			''' </summary>
			''' <param name="stringToSearch">The String to Search.</param>
			''' <param name="anyOf">The Chars to Look For.</param>
			''' <returns>The First index of any of the strings, or -1 if not found.</returns>
			''' <remarks></remarks>
			Public Shared Function IndexOfAny( _
				ByVal stringToSearch As String, _
				ByVal anyOf As Char(), _
				Optional byval startIndex as Integer = 0 _
			) As Integer

				Dim foundIndex As Integer = -1

				For i As Integer = 0 To anyOf.Length - 1

					If stringToSearch.IndexOf(anyOf(i), startIndex) > foundIndex Then

						Return stringToSearch.IndexOf(anyOf(i), startIndex)

					End If

				Next

				Return foundIndex

			End Function

			''' <summary>
			''' Method to Help Find whether a string is any one of an array of Chars.
			''' </summary>
			''' <param name="stringToSearch">The String to Search.</param>
			''' <param name="anyOf">The Strings to Look For.</param>
			''' <returns>A Boolean Value.</returns>
			''' <remarks>Case Insensitive.</remarks>
			Public Shared Function IsAny( _
				ByVal stringToSearch As String, _
				ByVal anyOf As Char() _
			) As Boolean

				If Not anyOf Is Nothing Then

					For i As Integer = 0 To anyOf.Length - 1

						If stringToSearch.Equals(anyOf(i).ToString, _
							StringComparison.InvariantCultureIgnoreCase) Then _
							Return True

					Next

				End If

				Return False

			End Function

			Public Shared Function NumberToString( _
				ByVal value As Integer, _
				Optional ByVal places As Integer = 0 _
			) As String

				Dim s As String = Convert.ToString(value)
				While s.Length < places
					s = DIGIT_ZERO & s
				End While
				Return s

			End Function

			''' <summary>
			''' Method to Help Find the Index an any particular String within a String.
			''' </summary>
			''' <param name="stringToSearch">The String to Search.</param>
			''' <param name="anyOf">The Strings to Look For.</param>
			''' <returns>The First index of any of the strings, or -1 if not found.</returns>
			''' <remarks></remarks>
			Public Shared Function IndexOfAny( _
				ByVal stringToSearch As String, _
				ByVal anyOf As String() _
			) As Integer

				Dim foundIndex As Integer = -1
				For i As Integer = 0 To anyOf.Length - 1
					If stringToSearch.IndexOf(anyOf(i)) > foundIndex Then
						Return stringToSearch.IndexOf(anyOf(i))
					End If
				Next
				Return foundIndex

			End Function

			''' <summary>
			''' Method to Help Find whether a string is any one of an array of String.
			''' </summary>
			''' <param name="stringToSearch">The String to Search.</param>
			''' <param name="anyOf">The Strings to Look For.</param>
			''' <returns>A Boolean Value.</returns>
			''' <remarks>Case Insensitive.</remarks>
			Public Shared Function IsAny( _
				ByVal stringToSearch As String, _
				ByVal anyOf As String() _
			) As Boolean

				If Not anyOf Is Nothing Then

					For i As Integer = 0 To anyOf.Length - 1

						If stringToSearch.Equals(anyOf(i), _
							StringComparison.InvariantCultureIgnoreCase) Then Return True

					Next

				End If

				Return False

			End Function

			''' <summary>
			''' Method to Remove Start/End Quotes from a String.
			''' </summary>
			''' <param name="value">The String to Remove from.</param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function RemoveNonAlphaNumericCharacters( _
				ByVal value As String _
			) As String

				If Not String.IsNullOrEmpty(value) Then

					Dim ret_String As New StringBuilder()

					For i As Integer = 0 To value.Length - 1

						If IsAlphaNumeric(value.Substring(i, 1)) Then _
							ret_String.Append(value.Substring(i, 1))

					Next

					Return ret_String.ToString()

				Else

					Return value

				End if

			End Function

			''' <summary>
			''' Method to test whether a supplied string is AlphaNumeric or Not.
			''' </summary>
			''' <param name="obj">The String to Test.</param>
			''' <returns>A Boolean Value indicating whether the string was AlphaNumeric (can include ? and space).</returns>
			''' <remarks></remarks>
			Public Shared Function IsAlphaNumeric( _
				ByVal obj As String _
			) As Boolean

				If obj = Nothing OrElse obj.Length = 0 Then
					Return False
				Else
					For i As Integer = 0 To obj.Length - 1
						If Not obj.Chars(i).Equals(QUESTION_MARK) AndAlso _
							Not obj.Chars(i).Equals(SPACE) AndAlso _
							Not Char.IsLetterOrDigit(obj.Chars(i)) Then

							Return False

						End If
					Next

					Return True
				End If

			End Function

			''' <summary>
			''' Method to test whether a supplied string is Numeric or Not.
			''' </summary>
			''' <param name="obj">The String to Test.</param>
			''' <returns>A Boolean Value indicating whether the String was Numeric.</returns>
			''' <remarks></remarks>
			Public Shared Function IsNumeric( _
				ByVal obj As String _
			) As Boolean

				If obj = Nothing OrElse obj.Length = 0 Then
					Return False
				Else
					For i As Integer = 0 To obj.Length - 1
						Select Case obj.Substring(i, 1)
							Case DIGIT_ZERO
							Case DIGIT_ONE
							Case DIGIT_TWO
							Case DIGIT_THREE
							Case DIGIT_FOUR
							Case DIGIT_FIVE
							Case DIGIT_SIX
							Case DIGIT_SEVEN
							Case DIGIT_EIGHT
							Case DIGIT_NINE
							Case FULL_STOP
							Case Else
								Return False
						End Select
					Next

					Return True
				End If

			End Function

			''' <summary>
			''' Method to Generate a Single String from an Object.
			''' </summary>
			''' <param name="value">Object (can be array).</param>
			''' <param name="separatorChar">The char to separate array elements.</param>
			''' <param name="nullResponse">The String Representation to use if the Object is Nothing.</param>
			''' <returns>A Single String</returns>
			''' <remarks></remarks>
			Public Shared Function ObjectToSingleString( _
				ByVal value As Object, _
				ByVal separatorChar As Char, _
				Optional ByVal nullResponse As String = Nothing _
			) As String

				If Not value Is Nothing Then

					Dim analyser As TypeAnalyser = TypeAnalyser.GetInstance(value.GetType)

					If analyser.IsArray Then

						If CType(value, Array).Length > 0 Then

							Dim values(CType(value, Array).Length - 1) As String

							For i As Integer = 0 To CType(value, Array).Length - 1

								values(i) = CType(value, Array)(i).ToString

							Next

							Return ArrayToSingleString(values, separatorChar)

						End If

					ElseIf analyser.IsIList Then

						If CType(value, IList).Count > 0 Then

							Dim values(CType(value, IList).Count - 1) As String

							For i As Integer = 0 To CType(value, IList).Count - 1

								If Not CType(value, IList)(i) Is Nothing Then _
									values(i) = CType(value, IList)(i).ToString

							Next

							Return ArrayToSingleString(values, separatorChar)

						End If

					ElseIf analyser.IsICollection Then

						Dim values As New List(Of String)

						For Each single_Value As Object In CType(value, ICollection)

							values.Add(single_Value.ToString)

						Next

						If values.Count > 0 Then Return ArrayToSingleString(values.ToArray(), separatorChar)

					Else

						Return value.ToString

					End If

				End If

				If String.IsNullOrEmpty(nullResponse) Then

					Return String.Empty

				Else

					Return nullResponse

				End If

			End Function

			''' <summary>
			''' Method to Generate a Single String from an Array of Strings.
			''' </summary>
			''' <param name="values">The Array of Strings.</param>
			''' <param name="separatorChar">The Character to separate the element Strings.</param>
			''' <returns>A Single String</returns>
			''' <remarks></remarks>
			Public Shared Function ArrayToSingleString( _
				ByVal values As String(), _
				ByVal separatorChar As Char _
			) As String

				Dim sb As New StringBuilder

				For i As Integer = 0 To values.Length - 1
					If sb.Length > 0 Then sb.Append(separatorChar)
					If Not values(i) = Nothing Then sb.Append(values(i))
				Next

				Return sb.ToString

			End Function

			''' <summary>
			''' Method to Remove Start/End Quotes from a String.
			''' </summary>
			''' <param name="value">The String to Remove from.</param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function RemoveQuotes( _
				ByVal value As String _
			) As String

				Return value.Trim( _
					New Char() {QUOTE_SINGLE, QUOTE_DOUBLE} _
				)

			End Function

			''' <summary>
			''' Method to Validate a given String against a Regular Expression.
			''' </summary>
			''' <param name="stringToBeValidated">The String to Validate.</param>
			''' <param name="validationRegex">The Regex to Validate Against.</param>
			''' <returns>A Boolean value indicating whether the string is valid.</returns>
			''' <remarks>Will return True if the Regular Expression is Empty.</remarks>
			Public Shared Function ValidatesAgainstRegularExpression( _
				ByVal stringToBeValidated As String, _
				ByVal validationRegex As String _
			) As Boolean

				If IsNullOrEmpty(validationRegex) Then

					Return True

				Else

					If stringToBeValidated = Nothing Then stringToBeValidated = String.Empty

					Return IsMatch(stringToBeValidated, validationRegex, _
						RegexOptions.IgnorePatternWhitespace)

				End If

			End Function

			''' <summary>
			''' Method to Match a given String against a Regular Expression.
			''' </summary>
			''' <param name="stringToBeChecked">The String to Check.</param>
			''' <param name="matchRegex">The Regex to Match Against.</param>
			''' <returns>A match.</returns>
			''' <remarks>Will return Nothing if the Regular Expression/String to Check is Empty.</remarks>
			Public Shared Function GetRegularExpressionMatch( _
				ByRef stringToBeChecked As String, _
				ByVal matchRegex As String _
			) As Match

				If Not IsNullOrEmpty(matchRegex) AndAlso Not IsNullOrEmpty(stringToBeChecked) Then

					Dim regexMatch As Match = New Regex(matchRegex).Match(stringToBeChecked)

					If regexMatch.Success Then Return regexMatch

				End If

				Return Nothing

			End Function

			''' <summary>
			''' Method to Match a given String against a Regular Expression.
			''' </summary>
			''' <param name="stringToBeChecked">The String to Check.</param>
			''' <param name="matchRegex">The Regex to Match Against.</param>
			''' <returns>A match value.</returns>
			''' <remarks>Will return Nothing if the Regular Expression/String to Check is Empty.</remarks>
			Public Shared Function GetRegularExpressionMatchValue( _
				ByRef stringToBeChecked As String, _
				ByVal matchRegex As String _
			) As String

				Dim regexMatch As Match = GetRegularExpressionMatch(stringToBeChecked, matchRegex)

				If Not regexMatch Is Nothing Then

					Return regexMatch.Value

				Else

					Return Nothing

				End If

			End Function

			''' <summary>
			''' Method to Match a given String against a Regular Expression.
			''' </summary>
			''' <param name="stringToBeChecked">The String to Check.</param>
			''' <param name="matchRegex">The Regex to Match Against.</param>
			''' <returns>An array of matches.</returns>
			''' <remarks>Will return Nothing if the Regular Expression/String to Check is Empty.</remarks>
			Public Shared Function GetRegularExpressionMatches( _
				ByRef stringToBeChecked As String, _
				ByVal matchRegex As String _
			) As Match()

				If Not IsNullOrEmpty(matchRegex) AndAlso Not IsNullOrEmpty(stringToBeChecked) Then

					Dim regexMatches As MatchCollection = New Regex(matchRegex).Matches(stringToBeChecked)

					If Not regexMatches Is Nothing AndAlso regexMatches.Count > 0 Then

						Dim aryRegexMatches(regexMatches.Count - 1) As Match

						regexMatches.CopyTo(aryRegexMatches, 0)

						Return aryRegexMatches

					End If

				End If

				Return Nothing

			End Function

			''' <summary>
			''' Method to Match a given String against a Regular Expression.
			''' </summary>
			''' <param name="stringToBeChecked">The String to Check.</param>
			''' <param name="matchRegex">The Regex to Match Against.</param>
			''' <returns>An array of matches.</returns>
			''' <remarks>Will return Nothing if the Regular Expression/String to Check is Empty.</remarks>
			Public Shared Function GetRegularExpressionMatchValues( _
				ByRef stringToBeChecked As String, _
				ByVal matchRegex As String _
			) As String()

				Dim regexMatches As Match() = GetRegularExpressionMatches(stringToBeChecked, matchRegex)

				If Not regexMatches Is Nothing Then

					Dim retValueArray(regexMatches.Length - 1) As String

					For i As Integer = 0 To regexMatches.Length - 1
						retValueArray(i) = regexMatches(i).Value
					Next

					Return retValueArray

				Else

					Return Nothing

				End If

			End Function

			''' <summary>
			''' Formats a String by Capitalised the First Letter of each word.
			''' </summary>
			''' <param name="value">The String to Format.</param>
			''' <returns></returns>
			''' <remarks>Words are identified by delineators (including spaces, - and ').</remarks>
			Public Shared Function CapitaliseString( _
				ByVal value As String _
			) As String

				Return CapitaliseString(value, New String() {SPACE, HYPHEN, QUOTE_SINGLE})

			End Function

			''' <summary>
			''' Formats a String by Capitalised the First Letter of each word.
			''' </summary>
			''' <param name="value">The String to Format.</param>
			''' <param name="delineators">The Delineators to Identify Words.</param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function CapitaliseString( _
				ByVal value As String, _
				ByVal ParamArray delineators As String() _
			) As String

				Dim hasDelineator As Boolean = False
				For Each delineator As Char In delineators
					If value.IndexOf(delineator) > 0 Then

						If Not hasDelineator Then hasDelineator = True

						Dim values As String() = value.Split(delineator)

						Dim sb As New StringBuilder

						For i As Integer = 0 To values.Length - 1

							sb.Append(CapitaliseWord(values(i), True))

							If i < values.Length - 1 Then sb.Append(delineator)

						Next

						value = sb.ToString

					End If
				Next

				If Not hasDelineator Then value = CapitaliseWord(value)

				Return value

			End Function

			''' <summary>
			''' Capitalises a word properly (makes the first letter a Upper case and the rest lower case).
			''' </summary>
			''' <param name="value">The Word to format.</param>
			''' <param name="retainOtherCapitalisation">Whether to retain existing Capitalisation.</param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function CapitaliseWord( _
				ByVal value As String, _
				Optional ByVal retainOtherCapitalisation As Boolean = False _
			) As String

				If Not String.IsNullOrEmpty(value) Then

					If Not retainOtherCapitalisation AndAlso value.Length = 2 Then

						retainOtherCapitalisation = True

						For i As Integer = 0 To TWO_LETTER_WORDS.Length - 1

							If String.Compare(value, TWO_LETTER_WORDS(i), True) = 0 Then
								retainOtherCapitalisation = False
								Exit For
							End If

						Next

					ElseIf value.Length = 3 Then

						retainOtherCapitalisation = True

						For i As Integer = 0 To THREE_LETTER_WORDS.Length - 1

							If String.Compare(value, THREE_LETTER_WORDS(i), True) = 0 Then
								retainOtherCapitalisation = False
								Exit For
							End If

						Next

					End If

					If retainOtherCapitalisation Then

						value = value.Substring(0, 1).ToUpper & value.Substring(1)

					Else

						value = value.Substring(0, 1).ToUpper & value.Substring(1).ToLower

					End If

				End If

				Return value

			End Function

			''' <summary>
			''' Camel-Cases a Word.
			''' </summary>
			''' <param name="value">The Word to Format.</param>
			''' <returns></returns>
			''' <remarks>GivenName would become Given Name, Person.GivenName would become Person.Given Name, ANR would become as Anr</remarks>
			Public Shared Function CamelCaseWords( _
				ByVal value As String _
			) As String

				If String.IsNullOrEmpty(value) Then

					Return String.Empty

				ElseIf value.IndexOf(SPACE) > 0 Then

					Dim words As String() = value.Split(SPACE)

					Dim sb As New System.Text.StringBuilder

					For i As Integer = 0 To words.Length - 1

						If i > 0 Then sb.Append(SPACE)
						sb.Append(CamelCaseWords(words(i)))

					Next

					Return sb.ToString

				Else

					Dim sb As New System.Text.StringBuilder

					Dim valueChars As Char() = value.ToCharArray

					For i As Integer = 0 To valueChars.Length - 1

						Dim currentChar As Char = valueChars(i)

						If i > 0 Then

							Dim previousChar As Char = valueChars(i - 1)

							If Char.IsUpper(currentChar) AndAlso _
								Not Char.IsUpper(previousChar) AndAlso _
								Not Char.IsPunctuation(previousChar) AndAlso _
								Not Char.IsSymbol(previousChar) Then sb.Append(SPACE)

						End If

						sb.Append(Char.ToLower(currentChar, _
							System.Globalization.CultureInfo.CurrentCulture))

					Next

					Return CapitaliseString(sb.ToString, New String() _
						{SPACE, HYPHEN, QUOTE_DOUBLE, QUOTE_SINGLE, FULL_STOP})

				End If

			End Function

			''' <summary>
			''' String Parsing for Simple Types.
			''' </summary>
			''' <param name="value">The Value to Parse.</param>
			''' <param name="intoType">The Type to Parse To.</param>
			''' <param name="parsedValue">The Parsed Value.</param>
			''' <returns>A Boolean indicating Parsing Success.</returns>
			''' <remarks></remarks>
			Public Shared Function TrySimpleParse( _
				ByVal value As String, _
				ByVal intoType As Type, _
				ByRef parsedValue As Object _
			) As Boolean

				If intoType Is GetType(String) Then

					Return value

				ElseIf intoType Is GetType(Boolean) Then

					Dim returnValue As Boolean
					If Boolean.TryParse(value, returnValue) Then
						parsedValue = returnValue
						Return True
					Else
						Return False
					End If

				ElseIf intoType Is GetType(Int16) OrElse intoType Is GetType(Short) Then

					Dim returnValue As Int16
					If Int16.TryParse(value, returnValue) Then
						parsedValue = returnValue
						Return True
					Else
						Return False
					End If

				ElseIf intoType Is GetType(Int32) OrElse intoType Is GetType(Integer) Then

					Dim returnValue As Int32
					If Int32.TryParse(value, returnValue) Then
						parsedValue = returnValue
						Return True
					Else
						Return False
					End If

				ElseIf intoType Is GetType(Int64) OrElse intoType Is GetType(Long) Then

					Dim returnValue As Int64
					If Int64.TryParse(value, returnValue) Then
						parsedValue = returnValue
						Return True
					Else
						Return False
					End If

				ElseIf intoType Is GetType(UInt16) Then

					Dim returnValue As UInt16
					If UInt16.TryParse(value, returnValue) Then
						parsedValue = returnValue
						Return True
					Else
						Return False
					End If

				ElseIf intoType Is GetType(UInt32) Then

					Dim returnValue As UInt32
					If UInt32.TryParse(value, returnValue) Then
						parsedValue = returnValue
						Return True
					Else
						Return False
					End If

				ElseIf intoType Is GetType(UInt64) Then

					Dim returnValue As UInt64
					If UInt64.TryParse(value, returnValue) Then
						parsedValue = returnValue
						Return True
					Else
						Return False
					End If

				ElseIf intoType Is GetType(Decimal) Then

					Dim returnValue As Decimal
					If Decimal.TryParse(value, returnValue) Then
						parsedValue = returnValue
						Return True
					Else
						Return False
					End If

				Else

					Return False

				End If

			End Function

		#End Region

	End Class

End Namespace
