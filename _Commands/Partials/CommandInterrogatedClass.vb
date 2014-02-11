Namespace Commands

    Partial Public Class CommandInterrogatedClass

#Region " Public Properties "

        Public ReadOnly Property HasSimpleConstructor() As Boolean
            Get
                Return Not Analyser Is Nothing AndAlso Analyser.HasDefaultConstructor
            End Get
        End Property

        Public ReadOnly Property HasComplexConstructor() As Boolean
            Get
                Return Not Analyser Is Nothing AndAlso Not Analyser.HasDefaultConstructor
            End Get
        End Property

        Public ReadOnly Property HasDefaultMethod() As Boolean
            Get
                If Not Methods Is Nothing Then

                    For i As Integer = 0 To Methods.Length - 1

                        If Methods(i).IsDefault Then Return True

                    Next

                End If

                Return False
            End Get
        End Property

        Public ReadOnly Property HasParameterLessMethod() As Boolean
            Get
                If Not Methods Is Nothing Then

                    For i As Integer = 0 To Methods.Length - 1

                        If Methods(i).ArgumentCount = 0 Then Return True

                    Next

                End If

                Return False
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub PopulateFromConstructor()

            If Not Analyser Is Nothing Then

                Dim assembly_Analyser As AssemblyAnalyser = _
                    AssemblyAnalyser.GetInstance(Analyser.Type.Assembly)

                Dim attrsTle As AttributeAnalyser = _
                    GetSingleObject( _
                        assembly_Analyser.ExecuteQuery( _
                        New AnalyserQuery() _
                            .SetReturnType(AnalyserType.AttributeAnalyser) _
                            .SetReturnTypeIsOrInheritedFromType(GetType(AssemblyTitleAttribute) _
                        ) _
                    ) _
                )

                If Not attrsTle Is Nothing Then

                    AssemblyName = CType(attrsTle.Attribute, AssemblyTitleAttribute).Title

                Else

                    AssemblyName = assembly_Analyser.Assembly.GetName.Name

                End If

                AssemblyVersion = assembly_Analyser.Assembly.GetName.Version.ToString

                If Methods Is Nothing AndAlso Not Analyser Is Nothing Then _
                    Methods = CommandInterrogatedMethod.GetMethods(Analyser)

                If Flags Is Nothing AndAlso Not Analyser Is Nothing Then _
                    Flags = CommandInterrogatedFlag.GetFlags(Analyser)

            End If

        End Sub

#End Region

#Region " Public Shared Methods "

        Public Shared Function InterrogateClass( _
            ByVal type As System.Type _
        ) As CommandInterrogatedClass

            Return InterrogateClass(TypeAnalyser.GetInstance(type))

        End Function

        Public Shared Function InterrogateClass( _
            ByVal classAnalyser As TypeAnalyser _
        ) As CommandInterrogatedClass

            Dim attributeAnalyser As AttributeAnalyser = _
                classAnalyser.GetAttribute(GetType(CommandAttribute))

            If Not attributeAnalyser Is Nothing Then

                Dim attr As CommandAttribute = attributeAnalyser.Attribute

                Return New CommandInterrogatedClass(attr.Name, classAnalyser, attr.Description, attr.Hidden)

            Else

                Return Nothing

            End If

        End Function

        Public Shared Function InterrogateDirectory( _
            ByVal folder As IO.DirectoryInfo, _
            Optional ByVal host As Leviathan.Commands.ICommandsExecution = Nothing _
        ) As CommandInterrogatedClass()

            Dim aryClassList, aryNameList As New ArrayList

            Dim l_Analyser As FolderAnalyser = FolderAnalyser.GetInstance(folder, host)

            Dim tys As TypeAnalyser() = l_Analyser.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.TypeAnalyser) _
                        .SetVisibility(MemberVisibility.Public) _
                        .SetPresentAttribute(GetType(CommandAttribute)))

            If Not tys Is Nothing Then

                For Each ty As TypeAnalyser In tys

					Dim attr_Analyser As AttributeAnalyser = ty.GetAttribute(GetType(CommandAttribute))
					
					If Not attr_Analyser Is Nothing AndAlso Not attr_Analyser.Attribute Is Nothing Then
						
						Dim attr As CommandAttribute = attr_Analyser.Attribute

	                    If aryNameList.Contains(attr.Name.ToLower) Then
	
	                        Dim dealtWithClass As Boolean = False
	                        For i As Integer = 0 To aryClassList.Count - 1
	                            If CType(aryClassList(i), CommandInterrogatedClass).Name = attr.Name.ToLower Then
	
	                                If TypeAnalyser.IsSubClassOf(ty.Type, CType(aryClassList(i), CommandInterrogatedClass).Analyser.Type) Then
	                                    aryClassList(i) = New CommandInterrogatedClass(attr.Name, ty, attr.Description, attr.Hidden)
	                                    dealtWithClass = True
	                                    Exit For
	                                End If
	
	                            End If
	                        Next
	
	                        Dim nameSuffix As Integer = 1
	                        Do Until dealtWithClass
	                            Dim className As String = attr.Name.ToLower & nameSuffix
	                            If Not aryNameList.Contains(className) Then
	
	                                aryNameList.Add(className)
	                                aryClassList.Add(New CommandInterrogatedClass(className, ty, attr.Description, attr.Hidden))
	                                dealtWithClass = True
	
	                            Else
	
	                                nameSuffix += 1
	
	                            End If
	
	                        Loop
	
	                    Else
	
	                        aryNameList.Add(attr.Name.ToLower)
	                        aryClassList.Add(New CommandInterrogatedClass(attr.Name.ToLower, ty, attr.Description, attr.Hidden))
	
	                    End If
                    
					End If
                    
                Next

            End If

            Return aryClassList.ToArray(GetType(CommandInterrogatedClass))

        End Function

#End Region

    End Class

End Namespace