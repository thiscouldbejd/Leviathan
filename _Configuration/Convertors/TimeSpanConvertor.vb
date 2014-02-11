Namespace Configuration

    Public Class TimeSpanConvertor

#Region " Public Constants "

        Public Shared TIMESPAN_SUFFIXES As Char() = _
            New Char() {"s", "e", "c", "m", "i", "n", "h", "o", "u", "r", "d", "a", "y", "t"}

        Public Shared TIMESPAN_DAY As String = "day"

        Public Shared TIMESPAN_DAYS As String = TIMESPAN_DAY & "s"

        Public Shared TIMESPAN_DAY_SHORT As String = "d"

        Public Shared TIMESPAN_HOUR As String = "hour"

        Public Shared TIMESPAN_HOURS As String = TIMESPAN_HOUR & "s"

        Public Shared TIMESPAN_HOUR_SHORT As String = "h"

        Public Shared TIMESPAN_MIN As String = "min"

        Public Shared TIMESPAN_MINS As String = TIMESPAN_MIN & "s"

        Public Shared TIMESPAN_MINUTE As String = "minute"

        Public Shared TIMESPAN_MINUTES As String = TIMESPAN_MINUTE & "s"

        Public Shared TIMESPAN_MINUTE_SHORT As String = "m"

        Public Shared TIMESPAN_SEC As String = "sec"

        Public Shared TIMESPAN_SECS As String = TIMESPAN_SEC & "s"

        Public Shared TIMESPAN_SECOND As String = "second"

        Public Shared TIMESPAN_SECONDS As String = TIMESPAN_SECOND & "s"

        Public Shared TIMESPAN_SECOND_SHORT As String = "s"

        Public Shared TIMESPAN_MS As String = "msec"

        Public Shared TIMESPAN_MSS As String = TIMESPAN_MS & "s"

        Public Shared TIMESPAN_MILLISECOND As String = "millisecond"

        Public Shared TIMESPAN_MILLISECONDS As String = TIMESPAN_MILLISECOND & "s"

        Public Shared TIMESPAN_MILLISECOND_SHORT As String = "ms"

        Public Shared TIMESPAN_TICK As String = "tick"

        Public Shared TIMESPAN_TICKS As String = TIMESPAN_TICK & "s"

        Public Shared TIMESPAN_TICK_SHORT As String = "t"

#End Region

#Region " Public Parsing Methods "

        ''' <summary>
        ''' Public Parsing Method.
        ''' </summary>
        ''' <param name="value">The Value to Parse.</param>
        ''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
        ''' <returns>The Parsed Object or Nothing.</returns>
        ''' <remarks>This method can only currently parse simple timespans.</remarks>
        Public Function ParseTimeSpanFromString( _
            <ParsingInParameterAttribute()> ByVal value As String, _
            <ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
            <ParsingTypeParameter()> ByVal typeToParseTo As Type _
        ) As Object

            Dim ret_TimeSpan As TimeSpan

            If value.EndsWith(TIMESPAN_SEC) OrElse _
                value.EndsWith(TIMESPAN_SECS) OrElse _
                value.EndsWith(TIMESPAN_SECOND) OrElse _
                value.EndsWith(TIMESPAN_SECONDS) Then

                ret_TimeSpan = New TimeSpan(0, 0, 0, _
                    Integer.Parse(value.TrimEnd(TIMESPAN_SUFFIXES), _
                    System.Globalization.CultureInfo.InvariantCulture))
                successfulParse = True

            ElseIf value.EndsWith(TIMESPAN_MIN) OrElse _
                value.EndsWith(TIMESPAN_MINS) OrElse _
                value.EndsWith(TIMESPAN_MINUTE) OrElse _
                value.EndsWith(TIMESPAN_MINUTES) OrElse _
                value.EndsWith(TIMESPAN_MINUTE_SHORT) Then

                ret_TimeSpan = New TimeSpan(0, 0, _
                    Integer.Parse(value.TrimEnd(TIMESPAN_SUFFIXES), _
                    System.Globalization.CultureInfo.InvariantCulture), 0)
                successfulParse = True

            ElseIf value.EndsWith(TIMESPAN_HOUR) OrElse _
                value.EndsWith(TIMESPAN_HOURS) OrElse _
                value.EndsWith(TIMESPAN_HOUR_SHORT) Then

                ret_TimeSpan = New TimeSpan(0, _
                    Integer.Parse(value.TrimEnd(TIMESPAN_SUFFIXES), _
                    System.Globalization.CultureInfo.InvariantCulture), 0, 0)
                successfulParse = True

            ElseIf value.EndsWith(TIMESPAN_DAY) OrElse _
                value.EndsWith(TIMESPAN_DAYS) OrElse _
                value.EndsWith(TIMESPAN_DAY_SHORT) Then

                ret_TimeSpan = New TimeSpan( _
                    Integer.Parse(value.TrimEnd(TIMESPAN_SUFFIXES), _
                    System.Globalization.CultureInfo.InvariantCulture), 0, 0, 0)
                successfulParse = True

            End If

            Return ret_TimeSpan

        End Function

        ''' <summary>
        ''' Public Method Handling the Parsing of a Colour.
        ''' </summary>
        ''' <param name="value">The Colour Value to Parse.</param>
        ''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
        ''' <returns>The Parsed Object or Nothing.</returns>
        ''' <remarks></remarks>
        Public Function ParseStringFromTimespan( _
            <ParsingInParameterAttribute()> ByVal value As TimeSpan, _
            <ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
            Optional ByVal shortFormat As Boolean = False _
        ) As String

            If Not value = Nothing Then

                Dim hasParent As Boolean = False
                Dim strBuilder As New System.Text.StringBuilder
                If Not value.Days = 0 Then

                    hasParent = True
                    strBuilder.Append(value.Days)
                    If Not shortFormat Then strBuilder.Append(SPACE)
                    If shortFormat Then
                        strBuilder.Append(TIMESPAN_DAY_SHORT)
                    ElseIf value.Days > 1 OrElse value.Days < -1 Then
                        strBuilder.Append(TIMESPAN_DAYS)
                    Else
                        strBuilder.Append(TIMESPAN_DAY)
                    End If

                End If

                If Not value.Hours = 0 Then
                    If hasParent Then
                        strBuilder.Append(COMMA)
                        strBuilder.Append(SPACE)
                    End If
                    hasParent = True
                    strBuilder.Append(value.Hours)
                    If Not shortFormat Then strBuilder.Append(SPACE)
                    If shortFormat Then
                        strBuilder.Append(TIMESPAN_HOUR_SHORT)
                    ElseIf value.Hours > 1 OrElse value.Hours < -1 Then
                        strBuilder.Append(TIMESPAN_HOURS)
                    Else
                        strBuilder.Append(TIMESPAN_HOUR)
                    End If

                End If

                If Not value.Minutes = 0 Then

                    If hasParent Then
                        strBuilder.Append(COMMA)
                        strBuilder.Append(SPACE)
                    End If
                    hasParent = True
                    strBuilder.Append(value.Minutes)
                    If Not shortFormat Then strBuilder.Append(SPACE)
                    If shortFormat Then
                        strBuilder.Append(TIMESPAN_MINUTE_SHORT)
                    ElseIf value.Minutes > 1 OrElse value.Minutes < -1 Then
                        strBuilder.Append(TIMESPAN_MINS)
                    Else
                        strBuilder.Append(TIMESPAN_MIN)
                    End If

                End If

                If Not value.Seconds = 0 Then

                    If hasParent Then
                        strBuilder.Append(COMMA)
                        strBuilder.Append(SPACE)
                    End If
                    hasParent = True
                    strBuilder.Append(value.Seconds)
                    If Not shortFormat Then strBuilder.Append(SPACE)
                    If shortFormat Then
                        strBuilder.Append(TIMESPAN_SECOND_SHORT)
                    ElseIf value.Seconds > 1 OrElse value.Seconds < -1 Then
                        strBuilder.Append(TIMESPAN_SECS)
                    Else
                        strBuilder.Append(TIMESPAN_SEC)
                    End If

                End If

                If value.Days = 0 _
                    AndAlso value.Hours = 0 _
                    AndAlso value.Minutes = 0 _
                    AndAlso value.Seconds < 60 _
                    AndAlso value.Milliseconds > 0 Then

                    If hasParent Then
                        strBuilder.Append(COMMA)
                        strBuilder.Append(SPACE)
                    End If
                    hasParent = True
                    strBuilder.Append(value.Milliseconds)
                    If Not shortFormat Then strBuilder.Append(SPACE)
                    If shortFormat Then
                        strBuilder.Append(TIMESPAN_MILLISECOND_SHORT)
                    ElseIf value.Milliseconds > 1 OrElse value.Milliseconds < -1 Then
                        strBuilder.Append(TIMESPAN_MSS)
                    Else
                        strBuilder.Append(TIMESPAN_MS)
                    End If

                End If

                If value.Days = 0 _
                    AndAlso value.Hours = 0 _
                    AndAlso value.Minutes = 0 _
                    AndAlso value.Seconds = 0 _
                    AndAlso value.Milliseconds < 10 Then

                    If hasParent Then
                        strBuilder.Append(COMMA)
                        strBuilder.Append(SPACE)
                    End If
                    hasParent = True
                    strBuilder.Append(value.Ticks)
                    If Not shortFormat Then strBuilder.Append(SPACE)
                    If shortFormat Then
                        strBuilder.Append(TIMESPAN_TICK_SHORT)
                    ElseIf value.Ticks > 1 OrElse value.Ticks < -1 Then
                        strBuilder.Append(TIMESPAN_TICKS)
                    Else
                        strBuilder.Append(TIMESPAN_TICK)
                    End If

                End If

                successfulParse = True
                Return strBuilder.ToString

            Else

                successfulParse = False
                Return String.Empty

            End If

        End Function

#End Region

    End Class

End Namespace