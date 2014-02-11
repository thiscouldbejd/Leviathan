Namespace Configuration

    Public Class EnumConvertor

#Region " Public Parsing Methods "
        ''' <summary>
        ''' Public Parsing Method.
        ''' </summary>
        ''' <param name="value">The Value to Parse.</param>
        ''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
        ''' <param name="typeToParseTo">Parameter used to specify an Enum Type to Parse to.</param>
        ''' <returns>The Parsed Object or Nothing.</returns>
        Public Function ParseEnumFromString( _
            <ParsingInParameter()> ByVal value As Object, _
            <ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
            <ParsingTypeParameter()> ByVal typeToParseTo As Type) As [Enum]

            If IsNumeric(value) Then

                successfulParse = True
                Return [Enum].Parse(typeToParseTo, value)

            Else

                If typeToParseTo.IsDefined(GetType(FlagsAttribute), True) Then
                    Dim values As String() = _
                        value.Split(New Char() {SEMI_COLON}, StringSplitOptions.RemoveEmptyEntries)

                    Dim e As Object = Nothing

                    For i As Integer = 0 To values.Length - 1
                        If i = 0 OrElse e = Nothing Then
                            e = ParseSingleEnumFromString(values(i), successfulParse, typeToParseTo)
                        Else
                            e = e Or ParseSingleEnumFromString(values(i), successfulParse, typeToParseTo)
                        End If
                    Next

                    Return e
                Else
                    Return ParseSingleEnumFromString(value, successfulParse, typeToParseTo)
                End If

            End If

        End Function
#End Region

#Region " Private Shared Methods "
        ''' <summary>
        ''' Private Method Handling the Parsing of a Single Enum Type.
        ''' </summary>
        ''' <param name="value">The String Value to Parse.</param>
        ''' <param name="enumType">The Type of Object to Parse To.</param>
        ''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successfull or not.</param>
        ''' <returns>The Parsed Object or Nothing.</returns>
        ''' <remarks></remarks>
        Private Function ParseSingleEnumFromString( _
            <ParsingInParameter()> ByVal value As String, _
            <ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
            <ParsingTypeParameter()> ByVal enumType As Type _
        ) As Object

            Dim names() As String = [Enum].GetNames(enumType)

            For Each name As String In names
                If value.ToLower = name.ToLower Then
                    successfulParse = True
                    Return [Enum].Parse(enumType, name)
                End If
            Next

            Return Nothing

        End Function
#End Region

    End Class

End Namespace