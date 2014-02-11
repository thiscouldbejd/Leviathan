Imports System.IO
Namespace Configuration

	Public Class StreamWriterConvertor

		#Region " Public Parsing Methods "

			''' <summary>
			''' Public Parsing Method.
			''' </summary>
			''' <param name="value">The Value to Parse.</param>
			''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
			''' <returns>The Parsed Object or Nothing.</returns>
			''' <remarks></remarks>
			Public Function ParseStreamWriterFromString( _
				<ParsingInParameter()> ByVal value As String, _
				<ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
				<ParsingTypeParameter()> ByVal typeToParseTo As Type _
			) As Object

				Dim ary_Temp As Filestream() = New FileStreamConvertor().ParseFileStreamsFromString(value, successfulParse, True)

				If Not ary_Temp Is Nothing AndAlso ary_Temp.Length > 0 AndAlso successfulParse Then

					Dim return_StreamList As New ArrayList

					If ary_Temp.Length = 1 Then

						If ary_Temp(0).CanWrite() Then Return New StreamWriter(ary_Temp(0))

					Else

						Dim list_Return As New List(Of StreamWriter)

						For i As Integer = 0 To ary_Temp.Length - 1

							If ary_Temp(i).CanWrite() Then list_Return.Add(New StreamWriter(ary_Temp(i)))

						Next

						Return list_Return.ToArray()

					End If

				End If

				successfulParse = False
				Return Nothing

			End Function

		#End Region

	End Class

End Namespace
