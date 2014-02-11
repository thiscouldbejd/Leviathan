Namespace Configuration

    Public Class FileInfoConvertor

#Region " Public Parsing Methods "

        ''' <summary>
        ''' Public Parsing Method.
        ''' </summary>
        ''' <param name="value">The Value to Parse.</param>
        ''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
        ''' <returns>The Parsed Object or Nothing.</returns>
        ''' <remarks></remarks>
        Public Function ParseFileInfoFromString( _
            <ParsingInParameterAttribute()> ByVal value As String, _
            <ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
            <ParsingTypeParameter()> ByVal typeToParseTo As Type _
        ) As Object

            If String.IsNullOrEmpty(value) Then

                successfulParse = False
                Return Nothing

            Else

                If IO.File.Exists(value) Then

                    successfulParse = True
                    Return New IO.FileInfo(value)

                Else

                    successfulParse = False
                    Return Nothing

                End If

            End If

        End Function

#End Region

    End Class

End Namespace