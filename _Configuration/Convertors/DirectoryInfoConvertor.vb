Imports System.IO
Imports System.IO.Path

Namespace Configuration

    Public Class DirectoryInfoConvertor

		#Region " Private Constants "

			''' <summary>
			''' Constant Defined the . Relative Path.
			''' </summary>
			''' <remarks></remarks>
			Private Const RELATIVE As String = FULL_STOP

			''' <summary>
			''' Constant Defined the \\ Network Path.
			''' </summary>
			''' <remarks></remarks>
			Private Const NETWORK As String = BACK_SLASH & BACK_SLASH

			''' <summary>
			''' Constant Defined the : Drive Path.
			''' </summary>
			''' <remarks></remarks>
			Private Const DRIVE As String = COLON

		#End Region

		#Region " Public Parsing Methods "

			''' <summary>
			''' Public Parsing Method.
			''' </summary>
			''' <param name="value">The Value to Parse.</param>
			''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
			''' <returns>The Parsed Object or Nothing.</returns>
			''' <remarks>
			''' Can support the following:
			''' Absolute - C:\Some Directory\Some File.txt
			''' Network - \\SomeServer\Some Directory\Some File.txt
			''' Relative to Current Directory - \Some File.txt
			''' Relative to Current Directory - Some Directory\Some File.txt
			''' Relative to Application Entry Directory - .Some File.txt
			''' Relative to Application Entry Directory - .\Some Directory\Some File.txt
			''' </remarks>
			Public Function ParseDirectoryInfoFromString( _
				<ParsingInParameterAttribute()> ByVal value As String, _
				<ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
				<ParsingTypeParameter()> ByVal typeToParseTo As Type _
			) As Object

				If String.IsNullOrEmpty(value) Then

					successfulParse = False
					Return Nothing

				Else

					If value.StartsWith(RELATIVE) Then

						value = value.TrimStart(RELATIVE)

						' Relative to Entry Point
						If Not String.IsNullOrEmpty(value) AndAlso value.StartsWith(BACK_SLASH) Then _
							value = BACK_SLASH & value

						Dim assemblyLocation As String = Nothing

						If Not [Assembly].GetEntryAssembly Is Nothing Then

							assemblyLocation = [Assembly].GetEntryAssembly.Location

						ElseIf Not [Assembly].GetExecutingAssembly Is Nothing Then

							assemblyLocation = [Assembly].GetExecutingAssembly.Location

						End If

						If Not String.IsNullOrEmpty(assemblyLocation) Then

							value = Combine(New IO.FileInfo(assemblyLocation).Directory.FullName, value)

						Else

							successfulParse = False
							Return Nothing

						End If

					ElseIf value.StartsWith(NETWORK) Then ' -- Network (Do Nothing)
					ElseIf value.IndexOf(COLON) = 1 Then ' -- Absolute Path (Do Nothing)
					Else ' -- Relative to Current Point

						value = value.TrimStart(RELATIVE)

						If Not value.EndsWith(BACK_SLASH) Then value &= BACK_SLASH

						value = Combine(New IO.DirectoryInfo(Environment.CurrentDirectory).FullName, value)

					End If

					If IO.Directory.Exists(value) Then

						successfulParse = True
						Return New IO.DirectoryInfo(value)

					Else

						successfulParse = False
						Return Nothing

					End If

				End If

			End Function

		#End Region

	End Class

End Namespace
