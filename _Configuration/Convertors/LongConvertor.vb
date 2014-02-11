Namespace Configuration

    Public Class LongConvertor

#Region " Public Parsing Methods "
        ''' <summary>
        ''' Public Method Handling the Parsing of a Colour.
        ''' </summary>
        ''' <param name="value">The Colour Value to Parse.</param>
        ''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
        ''' <returns>The Parsed Object or Nothing.</returns>
        ''' <remarks></remarks>
        Public Function ParseStringFromLong( _
            <ParsingInParameterAttribute()> ByVal value As Long, _
            <ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
            <ParsingTypeParameter()> ByVal typeToParseTo As Type _
        ) As String

            Dim strBuilder As New System.Text.StringBuilder

            Dim stringValue As String = value.ToString
            Dim isNegative As Boolean = value < 0
            If isNegative Then stringValue = stringValue.TrimStart(HYPHEN)

            If stringValue.TrimStart(HYPHEN).Length > 3 Then

                Dim currentPosition As Integer
                Math.DivRem(stringValue.Length, 3, currentPosition)

                If currentPosition > 0 Then
                    strBuilder.Append(stringValue.Substring(0, currentPosition))
                    strBuilder.Append(",")
                End If

                Do Until (currentPosition + 3) > stringValue.Length
                    strBuilder.Append(stringValue.Substring(currentPosition, 3))
                    If (currentPosition + 3) < stringValue.Length Then
                        strBuilder.Append(",")
                    End If

                    currentPosition += 3
                Loop

            Else

                strBuilder.Append(stringValue)

            End If

            If isNegative Then strBuilder.Insert(0, HYPHEN)
            successfulParse = True
            Return strBuilder.ToString

        End Function
#End Region

    End Class

End Namespace