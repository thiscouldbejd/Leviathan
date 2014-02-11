Imports System.Net.NetworkInformation
Imports System.Text.RegularExpressions

Namespace Configuration

    Public Class MacAddressConvertor

#Region " Private Shared Variables "

        ''' <summary>
        ''' Reference to a Valid MAc Address Characters used in Parsing Methods.
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared Valid_Characters As String = "[^0-9A-Fa-f]"

        Private Shared ByteArray_Length As Integer = 6

#End Region

#Region " Public Parsing Methods "

        ''' <summary>
        ''' Public Parsing Method.
        ''' </summary>
        ''' <param name="value">The Value to Parse.</param>
        ''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
        ''' <returns>The Parsed Object or Nothing.</returns>
        ''' <remarks>This method can parse from known colours, RGB colours and HTML colours.</remarks>
        Public Function ParseMacAddressFromString( _
            <ParsingInParameterAttribute()> ByVal value As String, _
            <ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
            <ParsingTypeParameter()> ByVal typeToParseTo As Type _
        ) As Object

            If Not String.IsNullOrEmpty(value) Then

				value = Regex.Replace(value, Valid_Characters, String.Empty)

				If value.Length = (ByteArray_Length * 2) Then

					Dim macBytes(ByteArray_Length - 1) As Byte

					For i As Integer = 0 To macBytes.Length - 1

						macBytes(i) = Byte.Parse(value.Substring(i * 2, 2), Globalization.NumberStyles.HexNumber)

					Next

					successfulParse = True
					Return New System.Net.NetworkInformation.PhysicalAddress(macBytes)

				End If

            End If

            successfulParse = False
            Return Nothing

        End Function

        ''' <summary>
        ''' Public Method Handling the Parsing of a MAC Address.
        ''' </summary>
        ''' <param name="value">The MAC Address to Parse.</param>
        ''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
        ''' <returns>The Parsed Object or Nothing.</returns>
        ''' <remarks></remarks>
        Public Function ParseStringFromMacAddress( _
            <ParsingInParameterAttribute()> ByVal value As PhysicalAddress, _
            <ParsingSuccessParameter()> ByRef successfulParse As Boolean) As String

            Return ParseStringFromMacAddress(value.GetAddressBytes, successfulParse)

        End Function

        ''' <summary>
        ''' Public Method Handling the Parsing of a MAC Address.
        ''' </summary>
        ''' <param name="value">The MAC Address to Parse.</param>
        ''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
        ''' <returns>The Parsed Object or Nothing.</returns>
        ''' <remarks></remarks>
        Public Function ParseStringFromMacAddress( _
            <ParsingInParameterAttribute()> ByVal value As Byte(), _
            <ParsingSuccessParameter()> ByRef successfulParse As Boolean) As String

            If Not value Is Nothing AndAlso value.Length = ByteArray_Length Then

                successfulParse = True

                Return value(0).ToString(LETTER_X) & value(1).ToString(LETTER_X) & FULL_STOP & _
                    value(2).ToString(LETTER_X) & value(3).ToString(LETTER_X) & FULL_STOP & _
                    value(4).ToString(LETTER_X) & value(5).ToString(LETTER_X)

            End If

            successfulParse = False
            Return String.Empty

        End Function

#End Region

    End Class

End Namespace