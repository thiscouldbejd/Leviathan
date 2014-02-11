Imports System.IO

Namespace Configuration

	Public Class FileStreamConvertor

		#Region " Private Constants "

			''' <summary>
			''' Constant Defined the * Wildcard.
			''' </summary>
			''' <remarks></remarks>
			Private Const WILDCARD As String = ASTERISK

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

			''' <summary>
			''' Constant Defined whether to Delete First.
			''' </summary>
			''' <remarks></remarks>
			Private Const DELETE As String = HASH

			''' <summary>
			''' Constant Defined whether to Use Multi-Leveled Search.
			''' </summary>
			''' <remarks></remarks>
			Private Const MULTI_LEVELLED As String = PERCENTAGE_MARK

		#End Region

		#Region " Public Parsing Methods "

			''' <summary>
			''' Public Parsing Method.
			''' </summary>
			''' <param name="value">The Value to Parse.</param>
			''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
			''' <returns>The Parsed Object or Nothing.</returns>
			''' <remarks></remarks>
			Public Function ParseFileStreamFromString( _
				<ParsingInParameter()> ByVal value As String, _
				<ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
				<ParsingTypeParameter()> ByVal typeToParseTo As Type _
			) As Object

				Dim aryReturn As IO.Stream() = ParseFileStreamsFromString(value, successfulParse, True)

				If successfulParse AndAlso Not aryReturn Is Nothing Then

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
			''' <remarks>
			''' Can support the following:
			''' Absolute - C:\Some Directory\Some File.txt
			''' Network - \\SomeServer\Some Directory\Some File.txt
			''' Relative to Current Directory - \Some File.txt
			''' Relative to Current Directory - Some Directory\Some File.txt
			''' Relative to Application Entry Directory - .Some File.txt
			''' Relative to Application Entry Directory - .\Some Directory\Some File.txt
			''' File Wildcards - \*.txt
			''' File Delete/Create - Start with #
			''' File Multi-Level Wildcards - Start with %
			''' </remarks>
			Public Function ParseFileStreamsFromString( _
				<ParsingInParameter()> ByVal value As String, _
				<ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
				Optional ByVal createFile As Boolean = True _
			) As IO.FileStream()

				Dim return_StreamList As New ArrayList

				If Not String.IsNullOrEmpty(value) AndAlso Not value.EndsWith(BACK_SLASH) Then

					Try

						Dim shouldDelete As Boolean = False
						Dim searchLevel As System.IO.SearchOption = SearchOption.TopDirectoryOnly

						If value.StartsWith(DELETE) Then

							value = value.TrimStart(DELETE)
							shouldDelete = True
							createFile = True

							If value.StartsWith(MULTI_LEVELLED) Then

								value = value.TrimStart(MULTI_LEVELLED)
								searchLevel = SearchOption.AllDirectories

							End If

						End If

						If value.StartsWith(MULTI_LEVELLED) Then

							value = value.TrimStart(MULTI_LEVELLED)
							searchLevel = SearchOption.AllDirectories

							If value.StartsWith(DELETE) Then

								value = value.TrimStart(DELETE)
								shouldDelete = True
								createFile = True

							End If

						End If

						If value.StartsWith(RELATIVE) Then

							value = value.TrimStart(RELATIVE)

							' Relative to Entry Point
							If Not value.StartsWith(BACK_SLASH) Then value = BACK_SLASH & value

							Dim directoryLocation As String = Nothing

							If Not [Assembly].GetEntryAssembly Is Nothing Then

								directoryLocation = [Assembly].GetEntryAssembly.Location

							ElseIf Not [Assembly].GetExecutingAssembly Is Nothing Then

								directoryLocation = [Assembly].GetExecutingAssembly.Location

							End If

							If Not String.IsNullOrEmpty(directoryLocation) Then

								value = New IO.FileInfo(directoryLocation).Directory.FullName & value

							Else

								createFile = False
								Return Nothing

							End If

						ElseIf value.StartsWith(NETWORK) Then

							' Network - Do Nothing

						ElseIf value.IndexOf(COLON) = 1 Then

							' AbsolutePath - Do Nothing

						Else

							' Relative to Current Point
							value = value.TrimStart(RELATIVE)

							If Not value.StartsWith(BACK_SLASH) Then value = BACK_SLASH & value

							value = New IO.DirectoryInfo(Environment.CurrentDirectory).FullName & value

						End If

						Dim baseDirectory As DirectoryInfo = New IO.DirectoryInfo(value.Substring(0, value.LastIndexOf(BACK_SLASH)))

						If baseDirectory.Exists Then

							Dim fileName As String = value.Substring(value.LastIndexOf(BACK_SLASH) + 1)

							Dim files As IO.FileInfo() = baseDirectory.GetFiles(fileName, searchLevel)

							If Not files Is Nothing AndAlso files.Length > 0 Then

								For i As Integer = 0 To files.Length - 1

									If shouldDelete Then

										files(i).Delete()

									Else

										Dim return_Stream As IO.FileStream = files(i).OpenRead()
										If Not return_Stream Is Nothing Then return_StreamList.Add(return_Stream)

									End If

								Next

								If shouldDelete Then files = Nothing

							End If

							If createFile AndAlso (files Is Nothing OrElse files.Length = 0) Then

								Dim return_Stream As IO.FileStream = New FileStream(value, FileMode.CreateNew, FileAccess.ReadWrite)

								If Not return_Stream Is Nothing Then return_StreamList.Add(return_Stream)

							End If

						End If

					Catch
					End Try

				End If

				If return_StreamList.Count = 0 Then

					successfulParse = False
					Return Nothing

				Else

					successfulParse = True
					Return return_StreamList.ToArray(GetType(IO.FileStream))

				End If

			End Function

		#End Region

	End Class

End Namespace
