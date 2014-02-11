Imports Hermes.Cryptography
Imports Leviathan.Files
Imports Leviathan.Visualisation
Imports System.IO
Imports System.Security
Imports System.Security.AccessControl.AccessControlModification
Imports System.Xml
Imports System.Xml.Xsl
Imports System.Xml.XPath
Imports IT = Leviathan.Visualisation.InformationType

Namespace Commands

	Public Class FilesCommands

		#Region " Private Methods "

			''' <summary>
			''' Method that creates/verifies a tree of folders.
			''' </summary>
			''' <param name="representation">The Directory Representations.</param>
			''' <param name="baseDirectory">The Base Directory Upon which to create/verify the Tree.</param>
			''' <returns>A Boolean value indicating Success.</returns>
			''' <remarks></remarks>
			Private Function VerifyDirectoryTree( _
				ByVal representation As DirectoryRepresentation(), _
				ByVal baseDirectory As DirectoryInfo _
			) As Boolean

				If Not representation Is Nothing _
					AndAlso representation.Length > 0 Then

					Dim childDirectories As DirectoryInfo() = baseDirectory.GetDirectories()

					For i As Integer = 0 To representation.Length - 1

						If representation(i).GetType Is GetType(DirectoryRepresentation) Then

							Dim childDirectory As DirectoryInfo = Nothing

							If Not childDirectories Is Nothing AndAlso childDirectories.Length > 0 Then

								For k As Integer = 0 To childDirectories.Length - 1

									If String.Compare(representation(i).Name, childDirectories(k).Name, True) = 0 Then

										childDirectory = childDirectories(k)
										Exit For

									End If

								Next

							End If

							If childDirectory Is Nothing Then childDirectory = baseDirectory.CreateSubdirectory(representation(i).Name)

							If representation(i).HasChildren AndAlso Not VerifyDirectoryTree(representation(i).Children, childDirectory) Then Return False

						End If

					Next

				End If

				Return True

			End Function

		#End Region

		#Region " Public Command Methods "

			''' <summary>
			''' Method to Transform an XML File using an XSL Stylesheet
			''' </summary>
			''' <param name="inputFile">The File to be transformed.</param>
			''' <param name="inputTemplate">The Stylesheet to Transform with.</param>
			''' <param name="outputFile">The Ouput of the Transformation.</param>
			''' <returns></returns>
			<Command( _
				ResourceContainingType:=GetType(FilesCommands), _
				ResourceName:="CommandDetails", _
				Name:="transform-xml", _
				Description:="@commandFilesDescriptionTransformXml@" _
			)> _
			Public Function ProcessCommandTransformXML( _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterInputFile@" _
				)> _
				ByVal inputFile As IO.FileStream, _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterTransformingFile@" _
				)> _
				ByVal inputTemplate As IO.FileStream, _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterOutputFile@" _
				)> _
				ByVal outputFile As IO.FileStream _
			) As Boolean

				Dim xmlInputReader As XmlReader = XmlReader.Create(inputFile)
				Dim xmlInputDoc As New XmlDocument
				xmlInputDoc.Load(xmlInputReader)

				Dim xslInputReader As XmlReader = XmlReader.Create(inputTemplate)
				Dim xslInputDoc As New XslCompiledTransform
				Dim settings As XsltSettings = New XsltSettings()
				settings.EnableScript = True
				settings.EnableDocumentFunction = True
				Dim resolver As New XmlUrlResolver()
				xslInputDoc.Load(xslInputReader, settings, resolver)

				Dim xmlOutputDoc As XmlWriter = XmlWriter.Create(outputFile)

				xslInputDoc.Transform(xmlInputDoc, xmlOutputDoc)

				Return True

			End Function

			''' <summary>
			''' Method to Rename all the Files in a Directory to Ordinals (e.g. 1,2,3,4).
			''' </summary>
			''' <param name="directories">The Directories which contain the files to Rename.</param>
			''' <returns>A Boolean Indicating Success.</returns>
			''' <remarks></remarks>
			<Command( _
				ResourceContainingType:=GetType(FilesCommands), _
				ResourceName:="CommandDetails", _
				Name:="rename-ordinal", _
				Description:="@commandFilesDescriptionRenameOrdinal@" _
			)> _
			Public Function ProcessCommandRenameOrdinal( _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterDirectories@" _
				)> _
				ByVal directories As IO.DirectoryInfo(), _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterPattern@" _
				)> _
				ByVal pattern As String, _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterStart@" _
				)> _
				ByVal start As Integer _
			) As Boolean

				If Not directories Is Nothing Then

					For i As Integer = 0 To directories.Length - 1

						Dim current As Integer = start

						Dim files As IO.FileInfo() = directories(i).GetFiles(pattern)

						Dim fileNames As String() = Array.CreateInstance(GetType(String), files.Length)

						For j As Integer = 0 To files.Length - 1
							fileNames(j) = files(j).FullName
						Next

						Array.Sort(fileNames)

						For j As Integer = 0 To fileNames.Length - 1

							Dim file As New IO.FileInfo(fileNames(j))

							Dim newFileName As String = System.IO.Path.Combine(file.DirectoryName, current & file.Extension)

							file.MoveTo(newFileName)

							current += 1

						Next

					Next

					Return True

				End If

				Return False

			End Function

			''' <summary>
			''' Method to Append Security on Files/Folders.
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			<Command( _
				ResourceContainingType:=GetType(FilesCommands), _
				ResourceName:="CommandDetails", _
				Name:="append-security", _
				Description:="@commandFilesDescriptionAppendSecurity@" _
			)> _
			Public Function ProcessCommandAppendSecurity( _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterDirectoryRepresentation@" _
				)> _
				ByVal representation As DirectoryRepresentation(), _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterBaseDirectory@" _
				)> _
				ByVal baseDirectory As DirectoryInfo _
			) As Boolean

				Return ProcessCommandAppendSecurity(representation, baseDirectory, False)

			End Function

			''' <summary>
			''' Method to Append Security on Files/Folders.
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			<Command( _
				ResourceContainingType:=GetType(FilesCommands), _
				ResourceName:="CommandDetails", _
				Name:="append-security", _
				Description:="@commandFilesDescriptionAppendSecurity@" _
			)> _
			Public Function ProcessCommandAppendSecurity( _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterDirectoryRepresentation@" _
				)> _
				ByVal representation As DirectoryRepresentation(), _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterBaseDirectory@" _
				)> _
				ByVal baseDirectory As DirectoryInfo, _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterCreateMissingDirectories@" _
				)> _
				ByVal createMissingDirectories As Boolean _
			) As Boolean

				If Not representation Is Nothing AndAlso representation.Length > 0 Then

					If createMissingDirectories Then _
						If Not VerifyDirectoryTree(representation, baseDirectory) Then Return False

					Dim childDirectories As DirectoryInfo() = baseDirectory.GetDirectories()

					If Not childDirectories Is Nothing AndAlso childDirectories.Length > 0 Then

						For i As Integer = 0 To representation.Length - 1

							For k As Integer = 0 To childDirectories.Length - 1

								If String.Compare(representation(i).Name, childDirectories(k).Name, True) = 0 Then

									If Not representation(i).ModifyACL(childDirectories(k), _
										AccessControl.AccessControlModification.Add) Then Return False

								End If

							Next

						Next

					End If

				End If

				Return True

			End Function

			''' <summary>
			''' Method to Replace Security on Files/Folders.
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			<Command( _
				ResourceContainingType:=GetType(FilesCommands), _
				ResourceName:="CommandDetails", _
				Name:="replace-security", _
				Description:="@commandFilesDescriptionReplaceSecurity@" _
			)> _
			Public Function ProcessCommandReplaceSecurity( _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterDirectoryRepresentation@" _
				)> _
				ByVal representation As DirectoryRepresentation(), _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterBaseDirectory@" _
				)> _
				ByVal baseDirectory As DirectoryInfo _
			) As Boolean

				Return ProcessCommandReplaceSecurity(representation, baseDirectory, False)

			End Function

			''' <summary>
			''' Method to Replace Security on Files/Folders.
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			<Command( _
				ResourceContainingType:=GetType(FilesCommands), _
				ResourceName:="CommandDetails", _
				Name:="replace-security", _
				Description:="@commandFilesDescriptionReplaceSecurity@" _
			)> _
			Public Function ProcessCommandReplaceSecurity( _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterDirectoryRepresentation@" _
				)> _
				ByVal representation As DirectoryRepresentation(), _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterBaseDirectory@" _
				)> _
				ByVal baseDirectory As DirectoryInfo, _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterCreateMissingDirectories@" _
				)> _
				ByVal createMissingDirectories As Boolean _
			) As Boolean

				If Not representation Is Nothing AndAlso representation.Length > 0 Then

					If createMissingDirectories Then _
						If Not VerifyDirectoryTree(representation, baseDirectory) Then Return False

					Dim childDirectories As DirectoryInfo() = baseDirectory.GetDirectories()

					If Not childDirectories Is Nothing AndAlso childDirectories.Length > 0 Then

						For i As Integer = 0 To representation.Length - 1

							For k As Integer = 0 To childDirectories.Length - 1

								If String.Compare(representation(i).Name, childDirectories(k).Name, True) = 0 Then

									If Not representation(i).ReplaceACL(childDirectories(k)) Then Return False

								End If

							Next

						Next

					End If

				End If

				Return True

			End Function

			''' <summary>
			''' Method to Audit Security on Files/Folders.
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			<Command( _
				ResourceContainingType:=GetType(FilesCommands), _
				ResourceName:="CommandDetails", _
				Name:="audit-security", _
				Description:="@commandFilesDescriptionAuditSecurity@" _
			)> _
			Public Function ProcessCommandAuditSecurity( _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterDirectoryRepresentation@" _
				)> _
				ByVal representation As DirectoryRepresentation(), _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterBaseDirectory@" _
				)> _
				ByVal baseDirectory As DirectoryInfo _
			) As Boolean

				'TODO: Implement This
				Throw New NotImplementedException()

			End Function

			<Command( _
				ResourceContainingType:=GetType(FilesCommands), _
				ResourceName:="CommandDetails", _
				Name:="remove-duplicates", _
				Description:="@commandFilesDescriptionRemoveDuplicates@" _
			)> _
			Public Function ProcessCommandRemoveDuplicates( _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterDirectories@" _
				)> _
				ByVal directories As IO.DirectoryInfo(), _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterPattern@" _
				)> _
				ByVal pattern As String _
			) As Boolean

				If Not directories Is Nothing Then

					Dim currentLengths As New Dictionary(Of Long, String)
					Dim duplicateLengths As New Dictionary(Of Long, Dictionary(Of String, List(Of String)))

					For i As Integer = 0 To directories.Length - 1

						Dim files As IO.FileInfo() = directories(i).GetFiles(pattern)

						For j As Integer = 0 To files.Length - 1

							If currentLengths.ContainsKey(files(j).Length) Then

								Dim lengthDictionary As New Dictionary(Of String, List(Of String))

								lengthDictionary.Add(Cipher.Generate_Hash(files(j), 64), _
									New List(Of String)(New String() {currentLengths(files(j).Length)}))

								duplicateLengths.Add(files(j).Length, lengthDictionary)

								currentLengths.Remove(files(j).Length)

							End If

							If duplicateLengths.ContainsKey(files(j).Length) Then

								Dim fileHash As String = Cipher.Generate_Hash(files(j), 64)

								If duplicateLengths(files(j).Length).ContainsKey(fileHash) Then

									duplicateLengths(files(j).Length)(fileHash).Add(files(j).FullName)

								Else

									duplicateLengths(files(j).Length).Add(fileHash, _
										New List(Of String)(New String() {files(j).FullName}))

								End If

							Else

								currentLengths.Add(files(j).Length, files(j).FullName)

							End If

						Next

						For Each duplicate_Sets As Dictionary(Of String, List(Of String)) In duplicateLengths.Values

							For Each key As String In duplicate_Sets.Keys

								If duplicate_Sets(key).Count > 1 Then

									Dim duplicate_Files As String() = duplicate_Sets(key).ToArray

									Dim sourceBytes As Byte() = IO.File.ReadAllBytes(duplicate_Files(0))

									For j As Integer = 1 To duplicate_Files.Length - 1

										Dim targetBytes As Byte() = IO.File.ReadAllBytes(duplicate_Files(j))

										If sourceBytes Is targetBytes Then

											' File.Delete(duplicate_Files(j))
											Console.Write(duplicate_Files(j) & " (DUPLICATE OF: " & duplicate_Files(0) & ")")

										End If

									Next

								End If

							Next

						Next

					Next

					Return True

				End If

				Return False

			End Function

			''' <summary>
			''' Method to Parse Robocopy Log Files.
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			<Command( _
				ResourceContainingType:=GetType(FilesCommands), _
				ResourceName:="CommandDetails", _
				Name:="parse-robocopy-logs", _
				Description:="@commandFilesDescriptionParseRobocopyLogs@" _
			)> _
			Public Function ProcessCommandParseRobocopyLogs( _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterPattern@" _
				)> _
				ByVal searchPattern As String, _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterBaseDirectory@" _
				)> _
				ByVal logDirectory As DirectoryInfo _
			) As IFixedWidthWriteable()

				Dim summary_Headers As String() = New String() { _
					"Total", "Copied", "Skipped", "Mismatch", "FAILED",  "Extras" _
				}

				Dim files As System.IO.FileInfo() = logDirectory.GetFiles(searchPattern)

				If files.Length > 0 Then

					Dim summary_Rows As New List(Of Row)
					Dim detail_Rows As New List(Of Row)

					For i As Integer = 0 To files.Length - 1

						Dim file_Name As String = files(i).Name

						Dim file_Modified As DateTime = files(i).LastWriteTime()

						Dim summary_Row As Row
						summary_Row = New Row().Add(file_Name).Add(file_Modified)

						Try

							Dim file_Stream As New System.IO.StreamReader(files(i).OpenRead())

							Do While file_Stream.Peek() >= 0

								Dim file_Line As String = file_Stream.ReadLine()

								If Not String.IsNullOrEmpty(file_Line) AndAlso _
									Not file_Line.Contains(HYPHEN) AndAlso _
									Not file_Line.Contains(BACK_SLASH) Then

									Dim term_Matches As Int32 = 0

									For j As Integer = 0 To summary_Headers.Length - 1

										If file_Line.Contains(summary_Headers(j)) Then

											term_Matches += 1

										Else

											Exit For

										End If

									Next

									If term_Matches = summary_Headers.Length Then

										Dim dirs_Line As String = file_Stream.ReadLine()
										Dim files_Line As String = file_Stream.ReadLine()
										Dim bytes_Line As String = file_Stream.ReadLine()
										Dim times_Line As String = file_Stream.ReadLine()

										Dim dirs_Values As String() = ParseRobocopyLogTableLine(dirs_Line)
										Dim files_Values As String() = ParseRobocopyLogTableLine(files_Line)
										Dim bytes_Values As String() = ParseRobocopyLogTableLine(bytes_Line)
										Dim times_Values As String() = ParseRobocopyLogTableLine(times_Line)

										If dirs_Values.Length = 6 AndAlso files_Values.Length = 6 AndAlso _
											bytes_Values.Length = 6 AndALso times_Values.Length = 6 Then

											If dirs_Values(4) = "0" AndAlso files_Values(4) = "0" AndAlso bytes_Values(4) = "0" Then
												summary_Row.Add(New Cell("Completed", 0, IT.Success))
											Else
												summary_Row.Add(New Cell("Completed/With Errors", 0, IT.Warning))
											End If

											summary_Row.Add(times_Values(0))

											Dim detail_Row As New Row()
											detail_Row.Add(file_Name)
											detail_Row.Add(String.Format("{0}/{1}/{2}", dirs_Values(1).Replace(SPACE, ""), _
												dirs_Values(5).Replace(SPACE, ""), dirs_Values(0).Replace(SPACE, "")))
											detail_Row.Add(String.Format("{0}/{1}/{2}", files_Values(1).Replace(SPACE, ""), _
												files_Values(5).Replace(SPACE, ""), files_Values(0).Replace(SPACE, "")))
											detail_Row.Add(String.Format("{0}/{1}/{2}", bytes_Values(1).Replace(SPACE, ""), _
												bytes_Values(5).Replace(SPACE, ""), bytes_Values(0).Replace(SPACE, "")))

											Dim dir_Plural As String
											If dirs_Values(4) <> "1" Then dir_Plural = "s" Else dir_Plural = ""
											Dim file_Plural As String
											If files_Values(4) <> "1" Then file_Plural = "s" Else file_Plural = ""

											If dirs_Values(4) <> "0" AndAlso files_Values(4) <> "0" Then
												detail_Row.Add(String.Format("{0} dir{1}, {2} file(3)", dirs_Values(4), dir_Plural, files_Values(4), file_Plural))
											ElseIf dirs_Values(4) <> "0" Then
												detail_Row.Add(String.Format("{0} dir{1}", dirs_Values(4), dir_Plural))
											ElseIf files_Values(4) <> "0" Then
												detail_Row.Add(String.Format("{0} file{1}", files_Values(4), file_Plural))
											Else
												detail_Row.Add()
											End If
											detail_Rows.Add(detail_Row)

										End If

										Exit Do

									End If

								End If

							Loop

							If summary_Row.Cells.Count = 2 Then _
								summary_Row.Add(New Cell("Did Not Complete", 0, IT.Error)).Add()

							file_Stream.Close()
							file_Stream = Nothing

						Catch ex As IO.IOException

							summary_Row.Add(New Cell("In Progress", 0, IT.Debug)).Add()

						End Try

						summary_Rows.Add(summary_Row)

					Next

					Return New Cube() { _
						Cube.Create(InformationType.Information, "Robocopy Summary", "File", "Completed", "State", "Took") _
							.Add(New Slice(summary_Rows)), _
						Cube.Create(InformationType.Information, "Robocopy Detail", "File", "Dirs (Upd/New/All)", "Files (Upd/New/All)", _
							"Bytes (Upd/New/All)", "Errors").Add(New Slice(detail_Rows)) _
						}

				Else

					Return Nothing

				End If

			End Function

			<Command( _
				ResourceContainingType:=GetType(FilesCommands), _
				ResourceName:="CommandDetails", _
				Name:="output-csv", _
				Description:="@commandFilesDescriptionOutputCsv@" _
			)> _
			Public Sub ProcessCommandOutputCsv( _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterDescriptionFormattedObjects@" _
				)> _
				ByVal value As Cube, _
				<Configurable( _
					ResourceContainingType:=GetType(FilesCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandFilesParameterOutputFile@" _
				)> _
				ByVal output As IO.FileStream _
			)

				If Not value Is Nothing Then

					Dim value_Slice As Slice = value.LastSlice

					If Not value_Slice Is Nothing Then

						Dim output_Writer As New System.IO.StreamWriter(output)

						For i As Integer = 0 To value.Columns.Count - 1

							If i > 0 Then output_Writer.Write(COMMA)
							output_Writer.Write(StringCommands.ObjectToSingleString(value.Columns(i).DisplayName, " | "))

						Next

						output_Writer.WriteLine()

						Dim rowCount As Integer = value_Slice.Rows.Count

						For i As Integer = 0 To rowCount - 1

							If Not value_Slice.Rows(i) Is Nothing AndAlso value_Slice.Rows(i).Cells.Count > 0 Then

								For j As Integer = 0 To value_Slice.Rows(i).Cells.Count - 1

									If j > 0 Then output_Writer.Write(COMMA)
									output_Writer.Write(StringCommands.ObjectToSingleString(value_Slice.Rows(i)(j), "; "))

								Next

							End If

							output_Writer.WriteLine()

							If Not Host Is Nothing AndAlso Host.Available(VerbosityLevel.Interactive) Then Host.Progress(i + 1 / rowCount, "Outputting Table Rows")

						Next

					End If

				End If

			End Sub

		#End Region

		#Region " Public Shared Methods "

			''' <summary>
			''' Gets the Document Name for a document (e.g. without path or extension).
			''' </summary>
			''' <param name="fileName"></param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function GetDocumentName( _
				ByVal fileName As String _
			) As String

				If fileName.Contains(BACK_SLASH) Then _
					fileName = fileName.Substring(fileName.LastIndexOf(BACK_SLASH) + 1)

				If fileName.Contains(FULL_STOP) Then _
					fileName = fileName.Substring(0, fileName.LastIndexOf(FULL_STOP))

				Return fileName

			End Function

			''' <summary>
			''' Gets the Extension (if present) for a Document Name/Path.
			''' </summary>
			''' <param name="fileName"></param>
			''' <returns>The String Extension.</returns>
			''' <remarks></remarks>
			Public Shared Function GetDocumentExtension( _
				ByVal fileName As String _
			) As String

				Dim l_Extension As String = Nothing

				If fileName.Contains(FULL_STOP) Then

					l_Extension = fileName.Substring(fileName.LastIndexOf(FULL_STOP) + 1)

				End If

				Return l_Extension

			End Function

			''' <summary>
			''' Checks whether the supplied Document Name/Path ends in one of the supplied Extensions.
			''' </summary>
			''' <param name="fileName"></param>
			''' <param name="fileExtensions"></param>
			''' <returns>A Boolean value indicating whether it is.</returns>
			''' <remarks></remarks>
			Public Shared Function CompareExtensions( _
				ByVal fileName As String, _
				ByVal ParamArray fileExtensions As String() _
			) As Boolean

				Dim l_Extension As String = GetDocumentExtension(fileName)

				For i As Integer = 0 To fileExtensions.Length - 1

					If String.Compare(l_Extension, fileExtensions(i), True) = 0 Then _
						Return True

				Next

				Return False

			End Function

			''' <summary>
			''' Changes the Extension (if present) for a Document Name/Path.
			''' </summary>
			''' <param name="fileName"></param>
			''' <param name="newExtension"></param>
			''' <returns>The New File Name.</returns>
			''' <remarks></remarks>
			Public Shared Function ChangeExtension( _
				ByVal fileName As String, _
				ByVal newExtension As String _
			) As String

				newExtension = newExtension.TrimStart(FULL_STOP)

				If fileName.Contains(FULL_STOP) Then

					Return fileName.Substring(0, fileName.LastIndexOf(FULL_STOP) + 1) & newExtension

				Else

					Return fileName & FULL_STOP & newExtension

				End If

			End Function

			''' <summary>
			''' Gets the Full Path of a File Name by appending the current directory if a directory
			''' has not been provided.
			''' </summary>
			''' <param name="fileName"></param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function GetFilePath( _
				ByVal fileName As String _
			) As String

				If fileName.Contains(BACK_SLASH) Then

					Return fileName

				Else

					Return Path.Combine(Environment.CurrentDirectory(), fileName)

				End If

			End Function

			''' <summary>
			''' Gets the Path of an existing File (if it exists).
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function TryGettingExistingFile( _
				ByVal fileName As String, _
				ByRef fileDetails As FileInfo _
			) As Boolean

				If Not String.IsNullOrEmpty(fileName) Then

					fileName = GetFilePath(fileName)

					If File.Exists(fileName) Then

						fileDetails = New FileInfo(fileName)
						Return True

					End If

				End If

				Return False

			End Function

			''' <summary>
			''' Gets the Path of a new file.
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function GetNewFile( _
				ByVal fileName As String _
			) As String

				If Not String.IsNullOrEmpty(fileName) Then

					If fileName.Contains(BACK_SLASH) Then

						If Directory.Exists(fileName.Substring(0, fileName.LastIndexOf(BACK_SLASH))) Then

							Return fileName

						End If

					Else

						Return Path.Combine(Environment.CurrentDirectory(), fileName)

					End If

				End If

				Return Nothing

			End Function

			''' <summary>
			''' Method to Get File Stream to write to.
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function GetFileStream( _
				ByVal fileName As String _
			) As Stream

				Dim existingFile As FileInfo = Nothing

				If TryGettingExistingFile(fileName, existingFile) Then

					Return New IO.FileStream(existingFile.FullName, FileMode.Append)

				Else

					Dim newFile As String = GetNewFile(fileName)

					If Not String.IsNullOrEmpty(newFile) Then _
						Return New FileStream(newFile, FileMode.CreateNew)

				End If

				Return Nothing

			End Function

			Public Shared Function ParseRobocopyLogTableLine( _
				ByVal line As String _
			) As String()

				If line.Length = 70 Then

					Dim return_String(5) As String

					For i = 1 To 6

						return_String(i - 1) = line.Substring(i * 10, 10).Trim()

					Next

					Return return_String

				Else

					Return Nothing

				End If

			End Function

		#End Region

	End Class

End Namespace