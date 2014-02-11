Imports System.Net

Namespace Configuration

    Public Class IPAddressConvertor

#Region " Public Parsing Methods "

        ''' <summary>
        ''' Public Parsing Method.
        ''' </summary>
        ''' <param name="value">The Value to Parse.</param>
        ''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
        ''' <returns>The Parsed Object or Nothing.</returns>
        ''' <remarks></remarks>
        Public Function ParseIPAddressFromString( _
            <ParsingInParameterAttribute()> ByVal value As String, _
            <ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
            <ParsingTypeParameter()> ByVal typeToParseTo As Type _
        ) As Object

            If Not String.IsNullOrEmpty(value) Then

                Dim retValue As IPAddress = Nothing

                If value.IndexOf(HYPHEN) > 0 AndAlso value.Split(HYPHEN).Length = 4 Then

                    Dim addressBytes As Byte() = Array.CreateInstance(GetType(Byte), 4)

                    Dim addressValues As String() = value.Split(HYPHEN)

                    For i As Integer = 0 To addressValues.Length - 1
                        addressBytes(i) = Byte.Parse(addressValues(i), Globalization.NumberStyles.HexNumber)
                    Next

                    retValue = New IPAddress(addressBytes)

                    successfulParse = True
                    Return retValue

                ElseIf value.Length = 4 Then

                    Dim addressBytes As Byte() = System.Text.ASCIIEncoding.ASCII.GetBytes(value)

                    retValue = New IPAddress(addressBytes)

                    successfulParse = True
                    Return retValue

                Else

                    successfulParse = IPAddress.TryParse(value, retValue)

                    Return retValue

                End If

            End If

            successfulParse = False
            Return Nothing

        End Function

#End Region

    End Class

End Namespace