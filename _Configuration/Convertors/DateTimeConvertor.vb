Imports Leviathan.Commands.StringCommands

Namespace Configuration

    Public Class DateTimeConvertor

#Region " Public Shared Methods "

        Public Shared Function ToXmlDateTime( _
            ByVal value As DateTime _
        ) As String

            Dim strBuilder As New System.Text.StringBuilder

            strBuilder.Append(value.Year)
            strBuilder.Append(HYPHEN)
            strBuilder.Append(NumberToString(value.Month, 2))
            strBuilder.Append(HYPHEN)
            strBuilder.Append(NumberToString(value.Day, 2))

            If value.Hour > 0 OrElse value.Minute > 0 OrElse value.Second > 0 Then

                strBuilder.Append(LETTER_T)
                strBuilder.Append(NumberToString(value.Hour, 2))
                strBuilder.Append(COLON)
                strBuilder.Append(NumberToString(value.Minute, 2))
                strBuilder.Append(COLON)
                strBuilder.Append(NumberToString(value.Second, 2))

            End If

            Return strBuilder.ToString

        End Function

#End Region

    End Class

End Namespace