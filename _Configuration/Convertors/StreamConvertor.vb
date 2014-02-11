Imports System.IO
Namespace Configuration

    Public Class StreamConvertor

#Region " Private Constants "

        ''' <summary>
        ''' Constant Defined the http prefix for a standard url.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const URL_PREFIX As String = "http"

#End Region

#Region " Public Parsing Methods "

        ''' <summary>
        ''' Public Parsing Method.
        ''' </summary>
        ''' <param name="value">The Value to Parse.</param>
        ''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
        ''' <returns>The Parsed Object or Nothing.</returns>
        ''' <remarks></remarks>
        Public Function ParseStreamFromString( _
            <ParsingInParameter()> ByVal value As String, _
            <ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
            <ParsingTypeParameter()> ByVal typeToParseTo As Type _
        ) As Object

            Dim aryReturn As IO.Stream() = ParseStreamsFromString(value, successfulParse, True)

            If successfulParse AndAlso _
                Not aryReturn Is Nothing Then

                If aryReturn.Length = 1 Then

                    Return aryReturn(0)

                Else

                    Return aryReturn

                End If


            Else

                Return Nothing

            End If

        End Function


        ''' <summary>
        ''' Public Parsing Method.
        ''' </summary>
        ''' <param name="value">The Value to Parse.</param>
        ''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
        ''' <returns>The Parsed Object or Nothing.</returns>
        ''' <remarks></remarks>
        Public Function ParseStreamsFromString( _
            <ParsingInParameter()> ByVal value As String, _
            <ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
            Optional ByVal createFile As Boolean = True _
        ) As IO.Stream()

            Dim return_StreamList As New ArrayList

            If Not String.IsNullOrEmpty(value) Then

                Try

                    If value.StartsWith(URL_PREFIX) Then

                        ' Web-based retrieval
                        Dim return_Stream As IO.Stream = _
                            New System.Net.WebClient().OpenRead(value)

                        If Not return_Stream Is Nothing Then _
                            return_StreamList.Add(return_Stream)

                    Else

                        ' File System Retrieval
                        Dim fileStreams As IO.FileStream() = _
                            New FileStreamConvertor().ParseFileStreamsFromString( _
                                value, successfulParse, createFile)

                        If Not fileStreams Is Nothing Then _
                            return_StreamList.AddRange(fileStreams)

                    End If

                Catch ex As Exception

                End Try

            End If

            If return_StreamList.Count = 0 Then
                successfulParse = False
                Return Nothing
            Else
                successfulParse = True
                Return return_StreamList.ToArray(GetType(IO.Stream))
            End If


        End Function

#End Region

    End Class

End Namespace