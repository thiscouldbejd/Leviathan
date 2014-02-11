Imports Leviathan.Caching
Imports Leviathan.Comparison.Comparer
Imports System.IO
Imports System.IO.Directory
Imports System.IO.Path

Namespace Inspection

    Partial Public Class AssemblyAnalyser

		#Region " Public Constants "
	
	        ''' <summary>
	        ''' Public Constant Reference to the Name of the ParseTypeFromString Method.
	        ''' </summary>
	        ''' <remarks></remarks>
	        Public Const METHOD_FINDTYPE As String = "FindType"
	
			''' <summary>
        	''' Prefix used to indicate a file
        	''' </summary>
        	''' <remarks></remarks>
        	Public Const FILE_PREFIX As String = "file:///"
        
		#End Region

		#Region " Public Properties "

	        ''' <summary>
	        ''' Whether the supplied Assembly was loaded correctly
	        ''' </summary>
	        ''' <value></value>
	        ''' <returns></returns>
	        ''' <remarks></remarks>
	        Public ReadOnly Property Loaded() As Boolean
	            Get
	                Return Not Assembly Is Nothing
	            End Get
	        End Property
	
	        ''' <summary>
	        ''' Full Assembly Name
	        ''' </summary>
	        ''' <value></value>
	        ''' <returns></returns>
	        ''' <remarks></remarks>
	        Public ReadOnly Property Name() As String
	            Get
	                Return Assembly.FullName
	            End Get
	        End Property

		#End Region

		#Region " Private Methods "

			''' <summary>
			''' Loads the Relevant Assemblies Associated with this one.
			''' </summary>
			Private Sub PostConstructorCall()
				
				Dim referencedAndProximalAssemblies As New List(Of Assembly)
				
				referencedAndProximalAssemblies.Add(Assembly)

				Dim referencedAssemblies As AssemblyName() = Assembly.GetReferencedAssemblies()
					
				For i As Integer = 0 To referencedAssemblies.Length - 1

					Try

						referencedAndProximalAssemblies.Add(Reflection.Assembly.Load(referencedAssemblies(i)))

					Catch fex As System.IO.FileNotFoundException
					Catch lex As System.IO.FileLoadException
					End Try
						
				Next
					
				Dim files As New List(Of String)

				Dim location_Dir As String = GetSafeDirectoryName(Assembly.Location)
				Dim codebase_Dir As String = GetSafeDirectoryName(Assembly.CodeBase)
					
				files.AddRange(GetFiles(location_Dir, FolderAnalyser.EXTENSION_DLL, SearchOption.AllDirectories))

				files.AddRange(GetFiles(location_Dir, FolderAnalyser.EXTENSION_EXE, SearchOption.AllDirectories))
						
				If Not String.Compare(location_Dir, codebase_Dir, True) = 0 Then
						
					files.AddRange(GetFiles(codebase_Dir, FolderAnalyser.EXTENSION_DLL, SearchOption.AllDirectories))
							
					files.AddRange(GetFiles(codebase_Dir, FolderAnalyser.EXTENSION_EXE, SearchOption.AllDirectories))
						
				End If

				For i As Integer = 0 To files.Count - 1

					Try

						Dim loadedAssembly As Assembly = Reflection.Assembly.LoadFrom(files(i))
						
						If Not referencedAndProximalAssemblies.Contains(loadedAssembly) Then _
							referencedAndProximalAssemblies.Add(loadedAssembly)

					Catch ex As Exception
#If DEBUG Then
						' Print Out Exception to the Debug Listener if thrown.
						Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.ToString)
						Debug.Print(files(i))
						Debug.Print(TypeAnalyser.ExceptionToString(ex))
						Debug.Flush()
#End If
					End Try
                        
				Next
				
				Assemblies = referencedAndProximalAssemblies.ToArray()

			End Sub
		
	        ''' <summary>
	        ''' This finds a particular type in an Array of Types.
	        ''' </summary>
	        ''' <param name="typeName">The Type Name to Find.</param>
	        ''' <param name="types">The Types to Search in.</param>
	        ''' <param name="ambiguousMatches">A ByRef Parameter which will be populated with Types that
	        ''' ambigiously match the <paramref name="typeName"/> parameter.</param>
	        ''' <returns>A Type with a Name matching that supplied, or Nothing if no Types match exactly.</returns>
	        ''' <remarks>Ambigious matching will be performed and the ambigious types returned by way of
	        ''' a ByRef parameter (<paramref name="ambiguousMatches"/> if no exact matches are found.</remarks>
	        Private Function FindType( _
	            ByVal typeName As String, _
	            ByVal types As System.Type(), _
	            ByRef ambiguousMatches As List(Of Type) _
	        ) As System.Type
	
	            For i As Integer = 0 To types.Length - 1
	
	                Dim exactMatch As Boolean = False
	                Dim typeMatch As String = MatchFuzzyString(typeName, types(i).Name.ToLower, exactMatch, FULL_STOP)
	
	                If Not String.IsNullOrEmpty(typeMatch) Then _
	                    If exactMatch Then Return types(i) Else ambiguousMatches.Add(types(i))
	
	                typeMatch = MatchFuzzyString(typeName, types(i).FullName.ToLower, exactMatch, FULL_STOP)
	
	                If Not String.IsNullOrEmpty(typeMatch) Then _
	                    If exactMatch Then Return types(i) Else ambiguousMatches.Add(types(i))
	
	            Next
	
	            Return Nothing
	
	        End Function
	
	        ''' <summary>
	        ''' This makes a safe directory name from a file path (including URIs).
	        ''' </summary>
	        ''' <param name="filePath"></param>
	        ''' <returns></returns>
	        ''' <remarks></remarks>
	        Private Function GetSafeDirectoryName( _
	        	ByVal filePath As String _
	        ) As String
	        
	            If filePath.StartsWith(FILE_PREFIX) Then
	            	
	                filePath = filePath.Substring(8)
	                filePath = filePath.Replace(FORWARD_SLASH, BACK_SLASH)
	                
	            End If
	            Return IO.Path.GetDirectoryName(filePath)
	            
	        End Function

		#End Region

#Region " Public Methods "

        ''' <summary>
        ''' This method will return an Array of <seealso cref="TypeAnalyser"/> objects
        ''' for each Type in the target Assembly that inherits from (optional) the
        ''' supplied type.
        ''' </summary>
        ''' <param name="objectsInheritedFrom">Objects should be inherited from this type.</param>
        ''' <returns>An Array of <seealso cref="TypeAnalyser"/> objects or Nothing if no
        ''' suitable types are found.</returns>
        ''' <remarks></remarks>
        Public Function GetTypeAnalysers( _
            Optional ByVal objectsInheritedFrom As System.Type = Nothing _
        ) As TypeAnalyser()

            If Not Assembly Is Nothing Then
                Dim types() As System.Type = Assembly.GetTypes()
                Array.Sort(types, New Comparison.Comparer)
                Dim retList As New ArrayList
                For Each ty As System.Type In types
                    If objectsInheritedFrom Is Nothing OrElse _
                        TypeAnalyser.IsSubClassOf(ty, objectsInheritedFrom) Then

                        retList.Add(TypeAnalyser.GetInstance(ty))

                    End If
                Next
                Return retList.ToArray(GetType(TypeAnalyser))
            Else
                Return Nothing
            End If

        End Function

        ''' <summary>
        ''' This method will search for all Types which match the name supplied in both the
        ''' target Assembly and also all it's referenced Assemblies too.
        ''' </summary>
        ''' <param name="typeName">The name of the Type to search for.</param>
        ''' <returns>An array of <seealso cref="type"/> objects which match the name,
        ''' or Nothing if none are found.</returns>
        ''' <remarks>A array containing a single type will be returned if the type is found
        ''' in the target Assembly. If it isn't, the search is widened to the Referenced Assemblies.
        ''' If nothing is found in there, the Method will then search the directory from which the
        ''' target Assembly was loaded to try to find the type in any other Assemblies.</remarks>
        Public Function FindType( _
            ByVal typeName As String _
        ) As System.Type()

            Dim cache As Simple = Simple.GetInstance(GetType(AssemblyAnalyser).GetHashCode)

            Dim ret_Types As System.Type() = Nothing

            If Not cache.TryGet(ret_Types, METHOD_FINDTYPE.GetHashCode, Name.GetHashCode, typeName.GetHashCode) Then

                ' -- Full Type Name --
                If typeName.Contains(FULL_STOP) Then
                	
                	For i As Integer = 0 To Assemblies.Length - 1
                		
                		Dim found_Type As Type = Assemblies(i).GetType(typeName, False, True)
                		
                		If Not found_Type Is Nothing Then
                			
                			ret_Types = New System.Type() {found_Type}
                			Exit For
                			
                		End If
                		
                	Next

                Else ' -- Partial Type Name --
                	
                	Dim found_TypeList As New List(Of Type)
                	
                	For i As Integer = 0 To Assemblies.Length - 1
                		
                		Dim found_Type As Type = FindType(typeName, Assemblies(i).GetTypes, found_TypeList)
                		
                		If Not found_Type Is Nothing Then
                			
                			ret_Types = New System.Type() {found_Type}
                			Exit For
                			
                		End If
                		
                	Next
                	
                	If ret_Types Is Nothing AndAlso found_TypeList.Count > 0 Then ret_Types = found_TypeList.ToArray()
                	                                    
                End If
                
                cache.Set(ret_Types, METHOD_FINDTYPE.GetHashCode, Name.GetHashCode, typeName.GetHashCode)
            
            End If
            
            Return ret_types

        End Function

        Public Function ExecuteQuery( _
            ByVal query As AnalyserQuery _
        ) As Array

            Dim cache As Simple = Simple.GetInstance(GetType(AssemblyAnalyser).GetHashCode)

            Dim cachedList As Object() = Nothing

            If Not cache.TryGet(cachedList, METHOD_EXECUTEQUERY.GetHashCode, Assembly.GetHashCode, query.GetHashCode) Then

                Dim memberArrayList As Array = Nothing

                If query.ReturnType = AnalyserType.AttributeAnalyser Then

                    memberArrayList = Assembly.GetCustomAttributes(False)

                ElseIf query.ReturnType = AnalyserType.TypeAnalyser Then

                    memberArrayList = Assembly.GetTypes()

                End If

                cachedList = query.GetPostQueryReturnArray(memberArrayList)

                cache.Set(cachedList, METHOD_EXECUTEQUERY.GetHashCode, Assembly.GetHashCode, query.GetHashCode)

            End If

            If Not cachedList Is Nothing AndAlso cachedList.Length > 0 Then Return cachedList

            Return Nothing

        End Function

#End Region

#Region " Public Shared Methods "

        ''' <summary>
        ''' Method to get a Singleton Instance of this Class.
        ''' </summary>
        ''' <param name="path">Path to Assembly to Analyse.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetInstance( _
            ByVal path As String _
        ) As AssemblyAnalyser

            Dim l_Assembly As Assembly = Nothing

            Try
                If Not System.IO.File.Exists(path) Then
                    Throw New System.IO.FileNotFoundException("Could Not Find Assembly File - " & path)
                End If

                l_Assembly = Reflection.[Assembly].LoadFrom(path)

            Catch ex As BadImageFormatException
            Catch ex As System.Exception

#If DEBUG Then
                ' Print Out Exception to the Debug Listener if thrown.
                Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.ToString)
                Debug.Print(path)
                Debug.Print(TypeAnalyser.ExceptionToString(ex))
                Debug.Flush()
#End If

            End Try

            Return GetInstance(l_Assembly)

        End Function

#End Region

    End Class

End Namespace
