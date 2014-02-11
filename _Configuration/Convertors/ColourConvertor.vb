Imports Leviathan.Caching
Imports System.Drawing

Namespace Configuration

    Public Class ColourConvertor

#Region " Public Constants "

        ''' <summary>
        ''' Public Constant Reference to the Name of the ParseColourFromString Method.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const METHOD_PARSECOLOURFROMSTRING As String = "ParseColourFromString"

#End Region

#Region " Private Shared Variables "

        ''' <summary>
        ''' Reference to a Default Colour used in Parsing Methods.
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared Default_Colour As System.Drawing.Color = System.Drawing.Color.White

#End Region

#Region " Public Parsing Methods "

        ''' <summary>
        ''' Public Parsing Method.
        ''' </summary>
        ''' <param name="value">The Value to Parse.</param>
        ''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
        ''' <returns>The Parsed Object or Nothing.</returns>
        ''' <remarks>This method can parse from known colours, RGB colours and HTML colours.</remarks>
        Public Function ParseColourFromString( _
            <ParsingInParameterAttribute()> ByVal value As String, _
            <ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
            <ParsingTypeParameter()> ByVal typeToParseTo As Type _
        ) As Object

            Dim ret_Colour As Color

            If value = Nothing OrElse value = HASH Then

                successfulParse = True
                Return Default_Colour

            Else

                Dim cache As Simple = Simple.GetInstance(GetType(ColorConverter).GetHashCode)

                If Not cache.TryGet(ret_Colour, METHOD_PARSECOLOURFROMSTRING.GetHashCode, value.GetHashCode) Then

                    If value.StartsWith(HASH) _
                        Then value = value.TrimStart(HASH)

                    If [Enum].IsDefined(GetType(System.Drawing.KnownColor), value) Then
                        ret_Colour = System.Drawing.Color.FromName(value)
                    ElseIf value.Length = 6 Then

                        ret_Colour = System.Drawing.Color.FromArgb( _
                            Integer.Parse(value.Substring(0, 2), Globalization.NumberStyles.HexNumber), _
                            Integer.Parse(value.Substring(2, 2), Globalization.NumberStyles.HexNumber), _
                            Integer.Parse(value.Substring(4, 2), Globalization.NumberStyles.HexNumber))

                    ElseIf value.Length = 8 Then

                        ret_Colour = System.Drawing.Color.FromArgb( _
                            Integer.Parse(value.Substring(0, 2), Globalization.NumberStyles.HexNumber), _
                            Integer.Parse(value.Substring(2, 2), Globalization.NumberStyles.HexNumber), _
                            Integer.Parse(value.Substring(4, 2), Globalization.NumberStyles.HexNumber), _
                            Integer.Parse(value.Substring(6, 2), Globalization.NumberStyles.HexNumber))

                    Else

                        ret_Colour = Default_Colour

                    End If

                    cache.Set(ret_Colour, METHOD_PARSECOLOURFROMSTRING.GetHashCode, value.GetHashCode)

                    successfulParse = True
                    Return ret_Colour

                Else

                    successfulParse = True
                    Return ret_Colour

                End If

            End If

        End Function

        ''' <summary>
        ''' Public Method Handling the Parsing of a Colour.
        ''' </summary>
        ''' <param name="value">The Colour Value to Parse.</param>
        ''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
        ''' <returns>The Parsed Object or Nothing.</returns>
        ''' <remarks></remarks>
        Public Function ParseStringFromColour( _
            <ParsingInParameterAttribute()> ByVal value As Color, _
            <ParsingSuccessParameter()> ByRef successfulParse As Boolean _
        ) As String

            If Not value = Nothing Then

                If value.IsKnownColor Then

                    successfulParse = True
                    Return value.Name

                Else

                    successfulParse = True
                    Return value.R.ToString("X") & value.G.ToString("X") & value.B.ToString("X")

                End If

            Else

                successfulParse = False
                Return String.Empty

            End If

        End Function

#End Region

    End Class

End Namespace