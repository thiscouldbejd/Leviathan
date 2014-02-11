Imports System.Text

Namespace Configuration

    Public Class GuidConvertor

#Region " Public Parsing Methods "
        ''' <summary>
        ''' Public Parsing Method.
        ''' </summary>
        ''' <param name="value">The Value to Parse.</param>
        ''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
        ''' <returns>The Parsed Object or Nothing.</returns>
        Public Function ParseGuidFromString( _
            <ParsingInParameter()> ByVal value As String, _
            <ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
            <ParsingTypeParameter()> ByVal typeToParseTo As Type _
        ) As Object

            Dim return_Guid As Guid = Nothing

            If GuidTryParse(value, return_Guid) Then

                successfulParse = True
                Return return_Guid

            Else

                successfulParse = False
                Return Nothing

            End If

        End Function
#End Region

#Region " Public Shared Methods "

        ''' <summary>
        ''' Method to Attempt a Parse for a Guid Method.
        ''' </summary>
        ''' <param name="value">The Value to Parse.</param>
        ''' <param name="returnValue">The Output Value of the Parse (if successful).</param>
        ''' <returns>A Boolean Value signifying whether the Parse was successful.</returns>
        ''' <remarks>This method echoes the Other TryParse Methods available as shared functions through
        ''' certain types.</remarks>
        Public Shared Function GuidTryParse( _
            ByVal value As String, _
            ByRef returnValue As Guid _
        ) As Boolean
            Try
                If value = Nothing Then
                    Return False
                ElseIf value.Length = 32 Then
                    Dim sb As New StringBuilder
                    sb.Append(value.Substring(0, 8))
                    sb.Append("-")
                    sb.Append(value.Substring(8, 4))
                    sb.Append("-")
                    sb.Append(value.Substring(12, 4))
                    sb.Append("-")
                    sb.Append(value.Substring(16, 4))
                    sb.Append("-")
                    sb.Append(value.Substring(20, 12))
                    returnValue = New Guid(sb.ToString)
                    Return True
                ElseIf value.Length = 36 Then
                    If Not value.IndexOf(HYPHEN, 0) = 8 Then
                        Return False
                    End If
                    If Not value.IndexOf(HYPHEN, 9) = 13 Then
                        Return False
                    End If
                    If Not value.IndexOf(HYPHEN, 14) = 18 Then
                        Return False
                    End If
                    If Not value.IndexOf(HYPHEN, 19) = 23 Then
                        Return False
                    End If
                    returnValue = New Guid(value)
                    Return True
                ElseIf value.Length = 38 Then
                    If Not value.StartsWith(BRACE_START) _
                        OrElse Not value.EndsWith(BRACE_END) Then
                        Return False
                    End If
                    If Not value.IndexOf(HYPHEN, 0) = 9 Then
                        Return False
                    End If
                    If Not value.IndexOf(HYPHEN, 10) = 14 Then
                        Return False
                    End If
                    If Not value.IndexOf(HYPHEN, 15) = 19 Then
                        Return False
                    End If
                    If Not value.IndexOf(HYPHEN, 20) = 24 Then
                        Return False
                    End If
                    returnValue = New Guid(value)
                    Return True
                Else
                    ' Base64 Encoded
                    Dim guidBytes As Byte() = Convert.FromBase64String(value)
                    If guidBytes.Length = 16 Then
                        returnValue = New Guid(guidBytes)
                        Return True
                    Else
                        Return False
                    End If
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function
#End Region

    End Class

End Namespace
