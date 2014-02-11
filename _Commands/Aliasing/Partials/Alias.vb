Imports Leviathan.Commands.StringCommands
Imports System.Text
Imports System.Xml.Serialization

Namespace Commands.Aliasing

	Partial Public Class [Alias]

		#Region " Private Constants "

			Private Const ARGUMENT_NOW As String = "#NOW#"

			Private Const ARGUMENT_NOW_DISP As String = "#NOW-DISPLAY#"

			Private Const ARGUMENT_THREAD As String = "#THREAD#"

		#End Region

		#Region " Public Methods "

			Public Function Expansions( _
				ByVal args As Object() _
			) As String()

				Return ExpandExpansions(ExpandTo, args)

			End Function

			Public Overrides Function ToString() As String

				Return Command

			End Function

		#End Region

		#Region " Private Shared Methods "

			Private Shared Function Thread() As String

				Return System.Threading.Thread.CurrentThread.ManagedThreadId.ToString

			End Function

			Private Shared Function Now() As String

				Return DateTime.Now.ToString

			End Function

			Private Shared Function NowDisplay() As String

				Dim currentDt As DateTime = DateTime.Now

				Dim sb As New System.Text.StringBuilder

				sb.Append(currentDt.Year.ToString)
				sb.Append(HYPHEN)

				sb.Append(NumberToString(currentDt.Month, 2))
				sb.Append(HYPHEN)

				sb.Append(NumberToString(currentDt.Day, 2))
				sb.Append(Space)

				sb.Append(NumberToString(currentDt.Hour, 2))
				sb.Append(COLON)

				sb.Append(NumberToString(currentDt.Minute, 2))
				sb.Append(COLON)

				sb.Append(NumberToString(currentDt.Second, 2))

				Return sb.ToString

			End Function

			Private Shared Function ExpandExpansions( _
				ByVal expansions As Expansion(), _
				ByVal args As Object() _
			) As String()

				Dim aryExpansions As String() = Nothing

				If expansions Is Nothing Then

					aryExpansions = New String() {}

				Else

					aryExpansions = Array.CreateInstance(GetType(String), expansions.Length)

					For i As Integer = 0 To args.Length - 1

						args(i) = SafeArg(args(i))

					Next

					For i As Integer = 0 To expansions.Length - 1

						aryExpansions(i) = String.Format(expansions(i).Command, args) _
							.Replace(ARGUMENT_NOW, Now) _
							.Replace(ARGUMENT_NOW_DISP, NowDisplay) _
							.Replace(ARGUMENT_THREAD, Thread)

					Next

				End If

				Return aryExpansions

			End Function

		#End Region

		#Region " Public Shared Methods "

			Public Shared Function SafeArg( _
				ByVal arg As String _
			) As String

				If arg.IndexOf(Space) > 0 Then

					Dim sb As New StringBuilder
					sb.Append(QUOTE_DOUBLE)
					sb.Append(arg)
					sb.Append(QUOTE_DOUBLE)
					Return sb.ToString

				Else

					Return arg

				End If

			End Function

		#End Region

	End Class

End Namespace