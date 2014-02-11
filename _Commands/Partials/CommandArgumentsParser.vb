Imports Leviathan.Commands.StringCommands
Imports System.Text

Namespace Commands

	Partial Public Class CommandArgumentsParser

		#Region " Public Properties "

			Public ReadOnly Property AllArguments As Char()
				Get
					Dim retList As New List(Of Char)
					If Not RetainedArguments Is Nothing Then retList.AddRange(RetainedArguments)
					If Not NonRetainedArguments Is Nothing then retList.AddRange(NonRetainedArguments)
					Return retList.ToArray()
				End Get
			End Property

		#End Region

		#Region " Friend Parsing Methods "

			Friend Function IsCharArgument( _
				ByVal argChar As Char, _
				ParamArray furtherArguments as Char() _
			) As Boolean

				If Not furtherArguments Is Nothing Then
					For i As Integer = 0 To furtherArguments.Length - 1
						If argChar = furtherArguments(i) Then Return True
					Next
				End If

				If Not RetainedArguments Is Nothing Then
					For i As Integer = 0 To RetainedArguments.Length - 1
						If argChar = RetainedArguments(i) Then Return True
					Next
				End If

				If Not NonRetainedArguments Is Nothing Then
					For i As Integer = 0 To NonRetainedArguments.Length - 1
						If argChar = NonRetainedArguments(i) Then Return True
					Next
				End If

				Return False

			End Function

			Friend Function IsArgumentEnd( _
				ByVal argChar As Char, _
				ByVal argPosition As Integer, _
				ByVal args As Char() _
			) As Boolean

				' The First Argument cannot be an End
				If argPosition = 0 Then Return False

				' The Last Argument must be an End
				If argPosition = args.Length - 1 Then Return True

				' A Space must be an End
				If argChar = SPACE Then Return True

				If IsCharArgument(argChar) Then
					' Is A End Argument

					' If Proceeded by a normal character then cannot be an End
					If Not IsCharArgument(args(argPosition + 1), SPACE) Then Return False

					' If Preceeded by a normal character/space then must be an End
					If Not IsCharArgument(args(argPosition - 1)) Then Return True

					' If Preceeded by an End Argument then can be End.
					Return IsArgumentEnd(args(argPosition - 1), argPosition - 1, args)

				Else

					Return False

				End If

			End Function

			Friend Function IsArgumentStart( _
				ByVal argChar As Char, _
				ByVal argPosition As Integer, _
				ByVal args As Char() _
			) As Boolean

				' The Last Argument cannot be a Start
				If (argPosition = args.Length - 1) Then Return False

				If IsCharArgument(argChar)Then
					' Is A Start Argument

					' The First Argument can be a Start
					If argPosition = 0 Then Return True

					' If Embedded between two Chars it can be a Start Argument
					If Not IsCharArgument(args(argPosition - 1), SPACE) AndAlso _
						Not IsCharArgument(args(argPosition + 1), SPACE) Then Return True

					' Is Preceeded By Space or Other Start Argument
					Return args(argPosition - 1) = SPACE _
						OrElse IsArgumentStart(args(argPosition - 1), argPosition - 1, args)

				Else

					Return False

				End If

			End Function

			Friend Function IsArgumentRetained(ByVal argChar As Char) As Boolean

				If argChar = SPACE Then Return False

				If Not NonRetainedArguments Is Nothing Then
					For i As Integer = 0 To NonRetainedArguments.Length - 1
						If argChar = NonRetainedArguments(i) Then Return False
					Next
				End If

				Return True

			End Function

		#End Region

		#Region " Public Parsing Methods "

			''' <summary>
			''' Method to Parse an Array of Arguments
			''' </summary>
			''' <param name="args">The Arguments as an Array of Strings.</param>
			''' <returns>The Arguments as an Array of Strings.</returns>
			''' <remarks></remarks>
			Public Function ParseArray( _
				ByVal args As String() _
			) As String()

				If args Is Nothing Then

					Return Nothing

				ElseIf args.Length = 0 Then

					Return args

				Else

					Dim sb As New StringBuilder

					For i As Integer = 0 To args.Length - 1

						If i > 0 Then sb.Append(SPACE)

						If args(i).IndexOf(SPACE) > 0 AndAlso _
							Not (args(i).StartsWith(QUOTE_DOUBLE) _
							AndAlso args(i).EndsWith(QUOTE_DOUBLE)) _
							AndAlso Not (args(i).StartsWith(QUOTE_SINGLE) _
							AndAlso args(i).EndsWith(QUOTE_SINGLE) _
						) Then

							sb.Append(QUOTE_DOUBLE)

							sb.Append(args(i))

							sb.Append(QUOTE_DOUBLE)

						Else

							sb.Append(args(i))

						End If

					Next

					Return ParseLine(sb.ToString)

				End If

			End Function

			''' <summary>
			''' Method to Parse an Argument Line into it's constinuent Arguments.
			''' </summary>
			''' <param name="arg">The Argument to be Parsed.</param>
			''' <returns>The Arguments as an Array of Strings.</returns>
			''' <remarks>This won't work properly with </remarks>
			Public Function ParseLine( _
				ByVal arg As String _
			) As String()

				If Not String.IsNullOrEmpty(arg) Then

					arg = arg.Trim(SPACE)

					' Simple Case, just split and return!
					If IndexOfAny(arg, AllArguments) < 0 Then _
						Return arg.Split(New Char() {Char.Parse(SPACE)}, StringSplitOptions.RemoveEmptyEntries)

					Dim ch As Char() = arg.ToCharArray()
					Dim current_Depth As Integer = 0, t As Integer = ch.Length - 1

					Dim singleArg As New StringBuilder, returnArgs As New ArrayList, depth_Markers As New Hashtable

					For i As Integer = 0 To t

						Dim c As Char = ch(i)

						If IsArgumentStart(c, i, ch) Then

							current_Depth += 1
							depth_Markers.Add(current_Depth, c)
							If (current_Depth > 1) OrElse IsArgumentRetained(c) Then singleArg.Append(c)

						ElseIf IsArgumentEnd(c, i, ch) Then

							If depth_Markers.ContainsKey(current_Depth) _
								AndAlso depth_Markers(current_Depth) = c Then

								depth_Markers.Remove(current_Depth)
								current_Depth -= 1

							End If

							' The i = t is to help mitigate the bug regarding question marks in the middle of strings
							If current_Depth = 0 OrElse i = t OrElse _
								(current_Depth = 1 AndAlso IndexOfAny(arg, AllArguments, i + 1) < 0) Then

								If IsArgumentRetained(c) Then singleArg.Append(c)
								If Not singleArg.ToString.Trim(SPACE) = Nothing Then returnArgs.Add(singleArg.ToString)
								singleArg = New StringBuilder
								depth_Markers.Clear()
								current_Depth = 0

							Else

								singleArg.Append(c)

							End If

						Else

							singleArg.Append(c)

						End If

					Next

					Return returnArgs.ToArray(GetType(String))

				Else

					Return Nothing

				End If

			End Function

		#End Region

		#Region " Public Shared Methods "

			''' <summary>
			''' This Method will parse arguments as the Command Line does
			''' (e.g. double quoted arguments will appear as one arg).
			''' </summary>
			''' <param name="commandLineArguments">The Line Arg to Parse.</param>
			''' <returns>An Array of Arguments.</returns>
			''' <remarks></remarks>
			Public Shared Function CommandLineParse( _
				ByVal commandLineArguments As String _
			) As String()

				Dim args As String() = commandLineArguments.Split(SPACE)

				Dim arrayArgs As New ArrayList

				For i As Integer = 0 To args.Length - 1

					If args(i).StartsWith(QUOTE_DOUBLE) Then

						If args(i).EndsWith(QUOTE_DOUBLE) Then

							arrayArgs.Add(args(i).Trim(QUOTE_DOUBLE))

						Else

							Dim argBuilder As New System.Text.StringBuilder( _
								args(i).TrimStart(QUOTE_DOUBLE))

							For j As Integer = i + 1 To args.Length - 1

								argBuilder.Append(SPACE)

								If args(j).EndsWith(QUOTE_DOUBLE) Then

									argBuilder.Append(args(j).TrimEnd(QUOTE_DOUBLE))
									i = j
									Exit For

								Else

									argBuilder.Append(args(j))

								End If

							Next

							arrayArgs.Add(argBuilder.ToString)

						End If

					Else

						arrayArgs.Add(args(i))

					End If

				Next

				Return arrayArgs.ToArray(GetType(String))

			End Function

		#End Region

	End Class

End Namespace
