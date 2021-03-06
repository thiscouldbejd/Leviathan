Imports System.IO
Namespace Configuration

	Public Class StreamReaderConvertor

		#Region " Public Parsing Methods "

			''' <summary>
			''' Public Parsing Method.
			''' </summary>
			''' <param name="value">The Value to Parse.</param>
			''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
			''' <returns>The Parsed Object or Nothing.</returns>
			''' <remarks></remarks>
			Public Function ParseStreamReaderFromString( _
				<ParsingInParameter()> ByVal value As String, _
				<ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
				<ParsingTypeParameter()> ByVal typeToParseTo As Type _
			) As Object

				Dim ary_Temp As Filestream() = New FileStreamConvertor().ParseFileStreamsFromString(value, successfulParse, False)

				If Not ary_Temp Is Nothing AndAlso ary_Temp.Length > 0 AndAlso successfulParse Then

					Dim return_StreamList As New ArrayList

					If ary_Temp.Length = 1 Then

						If ary_Temp(0).CanRead() Then Return New StreamReader(ary_Temp(0))

					Else

						Dim list_Return As New List(Of StreamReader)

						For i As Integer = 0 To ary_Temp.Length - 1

							If ary_Temp(i).CanRead() Then list_Return.Add(New StreamReader(ary_Temp(i)))

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
