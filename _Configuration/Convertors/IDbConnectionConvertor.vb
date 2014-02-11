Imports System.Data

Namespace Configuration

	Public Class IDbConnectionConvertor

		#Region " Public Parsing Methods "

			''' <summary>
			''' Public Parsing Method.
			''' </summary>
			''' <param name="value">The Value to Parse.</param>
			''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
			''' <returns>The Parsed Object or Nothing.</returns>
			''' <remarks></remarks>
			Public Function ParseIDbConnectionFromString( _
				<ParsingInParameter()> ByVal value As String, _
				<ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
				<ParsingTypeParameter()> ByVal typeToParseTo As Type _
			) As Object

				Dim return_Connection As IDbConnection = Nothing

				If Not String.IsNullOrEmpty(value) Then

					value = value.ToLower()

					If value.StartsWith("provider=microsoft.jet.") OrElse value.StartsWith("provider=microsoft.ace.") Then

						return_Connection = New System.Data.OleDb.OleDbConnection(value)
						successfulParse  = True

					Else

						return_Connection =  New System.Data.SqlClient.SqlConnection(value)
						successfulParse  = True

					End If

				End If

				Return return_Connection

			End Function

		#End Region

	End Class

End Namespace
