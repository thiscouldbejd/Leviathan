Imports System.Net
Imports System.Net.Dns

Namespace Configuration

	Public Class IPHostEntryConvertor

		#Region " Public Parsing Methods "

			''' <summary>
			''' Public Parsing Method.
			''' </summary>
			''' <param name="value">The Value to Parse.</param>
			''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
			''' <returns>The Parsed Object or Nothing.</returns>
			''' <remarks>This method can parse from known colours, RGB colours and HTML colours.</remarks>
			Public Function ParseIPHostEntryFromString( _
				<ParsingInParameterAttribute()> ByVal value As String, _
				<ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
				<ParsingTypeParameter()> ByVal typeToParseTo As Type _
			) As Object

				If Not String.IsNullOrEmpty(value) Then

					Dim hostEntry As IPHostEntry = Nothing

					Try
						hostEntry = GetHostEntry(value)
					Catch
					End Try

					If Not hostEntry Is Nothing Then
						successfulParse = True
						Return hostEntry
					End If

				End If

				successfulParse = False
				Return Nothing

			End Function

		#End Region

	End Class

End Namespace
