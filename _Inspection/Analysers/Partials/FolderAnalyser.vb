Imports Leviathan.Caching
Imports Leviathan.Comparison.Comparer
Imports VL = Leviathan.Commands.VerbosityLevel

Namespace Inspection

	Partial Public Class FolderAnalyser

		#Region " Private Variables "

			Private throw_Exceptions As Boolean = False

		#End Region

		#Region " Public Constants "

			''' <summary>
			''' Suffix used to indicate a dll
			''' </summary>
			''' <remarks></remarks>
			Public Const EXTENSION_DLL As String = "*.dll"

			''' <summary>
			''' Suffix used to indicate a exe
			''' </summary>
			''' <remarks></remarks>
			Public Const EXTENSION_EXE As String = "*.exe"

			''' <summary>
			''' Suffix used to indicate a Resources dll
			''' </summary>
			''' <remarks></remarks>
			Public Const SUFFIX_RESOURCES As String = ".resources.dll"

			''' <summary>
			''' Prefix used to indicate a System dll
			''' </summary>
			''' <remarks></remarks>
			Public Const PREFIX_SYSTEM As String = "System."

			''' <summary>
			''' Prefix used to indicate a Microsoft dll
			''' </summary>
			''' <remarks></remarks>
			Public Const PREFIX_MICROSOFT As String = "Microsoft."

			''' <summary>
			''' Prefix used to indicate a Office dll
			''' </summary>
			''' <remarks></remarks>
			Public Const PREFIX_OFFICE As String = "office."

		#End Region

		#Region " Public Properties "

			Public Overridable ReadOnly Property AssemblyFilePaths() As System.IO.FileInfo()
				Get
					SyncLock m_AssemblyFilePaths_LOCK

						If Not m_AssemblyFilePaths_HASVALUE Then

							m_AssemblyFilePaths = AnalysePath(Folder, Host, throw_Exceptions)
							m_AssemblyFilePaths_HASVALUE = True

						End If

					End SyncLock
					Return m_AssemblyFilePaths
				End Get
			End Property

		#End Region

		#Region " Public Methods "

			Public Function ExecuteQuery( _
				ByVal query As AnalyserQuery _
			) As Array

				Dim cache As Simple = Simple.GetInstance(GetType(FolderAnalyser).GetHashCode)

				Dim cachedList As Object() = Nothing

				If Not cache.TryGet(cachedList, METHOD_EXECUTEQUERY.GetHashCode, Folder.GetHashCode, query.GetHashCode) Then

					Dim memberArrayList As New ArrayList

					For Each file As IO.FileInfo In AssemblyFilePaths

						Try

							Dim [assembly] As Reflection.[Assembly] = Reflection.Assembly.LoadFrom(file.FullName)

							If Not [assembly] Is Nothing Then memberArrayList.AddRange([assembly].GetTypes)

						Catch ex As System.Exception

							If throw_Exceptions Then Throw ex

						End Try

					Next

					cachedList = query.GetPostQueryReturnArray(memberArrayList)

					cache.Set(cachedList, METHOD_EXECUTEQUERY.GetHashCode, Folder.GetHashCode, query.GetHashCode)

				End If

				If Not cachedList Is Nothing AndAlso cachedList.Length > 0 Then Return cachedList

				Return Nothing

			End Function

			Public Function UpdateAssemblyFilePaths( _
				ByVal _AssemblyFilePaths As System.IO.FileInfo() _
			) As Boolean

				SyncLock m_AssemblyFilePaths_LOCK

					If Not m_AssemblyFilePaths_HASVALUE OrElse Not AreEqual(m_AssemblyFilePaths, _AssemblyFilePaths) Then

						m_AssemblyFilePaths = _AssemblyFilePaths
						m_AssemblyFilePaths_HASVALUE = True
						Return True

					Else

						Return False

					End If

				End SyncLock

			End Function

		#End Region

		#Region " Public Shared Methods "

			''' <summary>
			''' This method will verify an Assembly is a .Net/IL Assembly and that it can be loaded.
			''' </summary>
			''' <param name="assemblyFile">The path to the Assembly.</param>
			''' <returns>A Boolean value indicating whether the Assembly is valid and can be loaded.</returns>
			''' <remarks></remarks>
			Public Shared Function VerifyAssemblyFile( _
				ByVal assemblyFile As IO.FileInfo, _
				Optional ByVal host As Leviathan.Commands.ICommandsExecution = Nothing, _
				Optional ByVal throw_Exceptions As Boolean = False _
			) As Boolean

				Try

					If Not host Is Nothing AndAlso host.Available(VL.Verbose) Then _
						Host.Log(String.Format(Resources.COMMAND_LOADING_ANALYSING, assemblyFile.FullName))

					If assemblyFile.Name.EndsWith(SUFFIX_RESOURCES) OrElse _
						assemblyFile.Name.StartsWith(PREFIX_SYSTEM) OrElse _
						assemblyFile.Name.StartsWith(PREFIX_MICROSOFT) OrElse _
						assemblyFile.Name.StartsWith(PREFIX_OFFICE) Then

						If Not host Is Nothing AndAlso host.Available(VL.Verbose) Then _
							Host.Log(String.Format(Resources.COMMAND_LOADING_IGNORE, assemblyFile.Name))

						Return False

					Else

						Dim test_assmbly As [Assembly] = Assembly.LoadFrom(assemblyFile.FullName)
						Return True

					End If

				Catch ex As BadImageFormatException

					If Not host Is Nothing Then
						If host.Available(VL.Verbose) Then _
							Host.Log(String.Format(Resources.COMMAND_LOADING_FAILED, assemblyFile.Name))
						If host.Available(VL.Standard) Then _
							host.Warn(ex.ToString)
					End If

					If throw_Exceptions Then Throw ex Else Return False

				Catch ex As System.Exception

					If Not host Is Nothing Then
						If host.Available(VL.Verbose) Then _
							Host.Log(String.Format(Resources.COMMAND_LOADING_FAILED, assemblyFile.Name))
						If host.Available(VL.Standard) Then _
							host.Warn(ex.ToString)
					End If

					If throw_Exceptions Then Throw ex Else Return False

				End Try

			End Function

			Public Shared Function AnalysePath( _
				ByVal folder As IO.DirectoryInfo, _
				Optional ByVal host As Leviathan.Commands.ICommandsExecution = Nothing, _
				Optional ByVal throw_Exceptions As Boolean = False _
			) As IO.FileInfo()

				If Not folder.Exists Then Throw New ArgumentException()

				Dim fileList As New ArrayList()

				fileList.AddRange(folder.GetFiles(EXTENSION_DLL, IO.SearchOption.AllDirectories))
				fileList.AddRange(folder.GetFiles(EXTENSION_EXE, IO.SearchOption.AllDirectories))

				For i As Integer = 0 To fileList.Count - 1

					If i <= fileList.Count - 1 Then

						If Not VerifyAssemblyFile(fileList(i), host, throw_Exceptions) Then

							fileList.RemoveAt(i)
							i -= 1

						End If

					End If

				Next

				Return fileList.ToArray(GetType(IO.FileInfo))

			End Function

		#End Region

	End Class

End Namespace
