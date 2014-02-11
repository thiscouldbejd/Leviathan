Imports System.Drawing

Namespace Configuration

    Public Class FontConvertor

#Region " Private Shared Variables "
        ''' <summary>
        ''' Reference to a Default Font used in Parsing Methods.
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared Default_Font As FontFamily = FontFamily.GenericSansSerif
        
#End Region

#Region " Public Parsing Methods "
        ''' <summary>
        ''' Public Parsing Method.
        ''' </summary>
        ''' <param name="value">The Value to Parse.</param>
        ''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
        ''' <returns>The Parsed Object or Nothing.</returns>
        ''' <remarks></remarks>
        Public Function ParseFontFamilyFromString( _
            <ParsingInParameter()> ByVal value As String, _
            <ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
            <ParsingTypeParameter()> ByVal typeToParseTo As Type _
        ) As Object

            Dim return_Font As FontFamily

            If value = Nothing Then
                successfulParse = True
                Return Default_Font
            Else
                return_Font = New FontFamily(value)
                If return_Font.Equals(Nothing) Then
                    successfulParse = False
                    Return Nothing
                Else
                    successfulParse = True
                    Return return_Font
                End If
            End If

        End Function
        
#End Region

    End Class

End Namespace