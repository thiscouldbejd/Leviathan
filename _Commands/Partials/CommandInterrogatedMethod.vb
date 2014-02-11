Namespace Commands

    Partial Public Class CommandInterrogatedMethod

#Region " Public Constants "

        Public Const DEFAULT_METHODNAME As String = ASTERISK

        Public Const DELINEATOR_PARAMNUMBER As String = "##"

#End Region

#Region " Public Properties "

        Public ReadOnly Property IsDefault() As Boolean
            Get
                Return String.IsNullOrEmpty(Name) OrElse String.Compare(Name, DEFAULT_METHODNAME) = 0
            End Get
        End Property

#End Region

#Region " Public Shared Methods "

        Public Shared Function GetMethods( _
            ByVal commandAnalyser As TypeAnalyser _
        ) As CommandInterrogatedMethod()

            Dim methods As MemberAnalyser() = _
                commandAnalyser.ExecuteQuery( _
                    AnalyserQuery.QUERY_METHODS_PUBLIC_INSTANCE _
                        .SetPresentAttribute(GetType(CommandAttribute)) _
                    )

            Dim l_Commands As New SortedList

            If Not methods Is Nothing Then

                For Each method As MemberAnalyser In methods

                    Dim commandParameters As New ArrayList
                    Dim paramCount As Integer = 0
                    Dim paramCountMinimum As Boolean = False
                    Dim methodType As CommandType

                    Dim command_Attribs As CommandAttribute() = _
                        method.Method.GetCustomAttributes(GetType(CommandAttribute), False)

                    If command_Attribs.Length > 0 Then

						Dim command_Parameters As ParameterInfo() = method.Method.GetParameters()
						
                        If CType(method.Method, MethodInfo).ReturnType Is Nothing OrElse _
                            CType(method.Method, MethodInfo).ReturnType Is GetType(System.Void) Then

                            methodType = CommandType.Unknown

                        ElseIf CType(method.Method, MethodInfo).ReturnType Is GetType(Boolean) Then

                            methodType = CommandType.Process

                        ElseIf CType(method.Method, MethodInfo).ReturnType Is GetType(Visualisation.IFixedWidthWriteable) Then

                            methodType = CommandType.Format

                        ElseIf command_Parameters.Length = 0 Then

                        	methodType = CommandType.Retrieval
                        	
                        Else

                            methodType = CommandType.Transform

                        End If

						For i As Integer = 0 To command_Parameters.Length - 1

                            Dim parameter_Attribs As ConfigurableAttribute() = _
                                command_Parameters(i).GetCustomAttributes(GetType(ConfigurableAttribute), True)

                            If parameter_Attribs.Length = 1 Then
                            	
                            	If DoesTypeImplementInterface(command_Parameters(i).ParameterType, GetType(Visualisation.IFixedWidthWriteable)) OrElse _
								(command_Parameters(i).ParameterType.IsArray AndAlso _
								DoesTypeImplementInterface(command_Parameters(i).ParameterType.GetElementType, GetType(Visualisation.IFixedWidthWriteable))) Then _
									methodType = CommandType.Output
                                
                            	If (command_Parameters(i).Position = command_Parameters.Length - 1) _
                                	AndAlso command_Parameters(i).ParameterType.IsArray Then paramCountMinimum = True
                            	
                            	Dim l_Name As String = parameter_Attribs(0).Name
                            	If String.IsNullOrEmpty(l_Name) Then _
                            		l_Name = command_Parameters(i).Name
                            	
	                            commandParameters.Add( _
	                                New CommandInterrogatedParameter( _
	                                    l_Name, _
	                                    parameter_Attribs(0).Description, _
	                                    command_Parameters(i).ParameterType, _
	                                    command_Parameters(i).Position) _
									)
                                        
                                paramCount += 1
                                
                            End If
                            
						Next

						For i As Integer = 0 To command_Attribs.Length - 1
							
							Dim command_Name As String, display_Name As String = String.Empty
							
                            If command_Attribs(i).Name = Nothing Then
                            	
                                command_Name = DEFAULT_METHODNAME & DELINEATOR_PARAMNUMBER & paramCount
                                
                            Else
                            	
                                command_Name = command_Attribs(i).Name & DELINEATOR_PARAMNUMBER & paramCount
                                display_Name = command_Attribs(i).Name.ToLower
                                
                            End If

                            l_Commands.Add(command_Name, _
                                New CommandInterrogatedMethod( _
                                    display_Name, command_Attribs(i).Description, methodType, paramCount, _
                                    paramCountMinimum, command_Parameters.Length, method, _
                                    commandParameters.ToArray(GetType(CommandInterrogatedParameter))))
                                    
						Next

                    End If

                Next

            End If

            Dim retArray As CommandInterrogatedMethod() = _
                Array.CreateInstance(GetType(CommandInterrogatedMethod), l_Commands.Count)

            l_Commands.Values.CopyTo(retArray, 0)

            Return retArray

        End Function

#End Region

#Region " Public Methods "

        Public Overridable Function Invoke( _
            ByVal invokedObject As Object, _
            ByVal parameters As Object(), _
            ByVal environment As ICommandsExecution _
        ) As Object

            Return InvokableMethod.Method.Invoke(invokedObject, parameters)

        End Function

        Public Overrides Function ToString() As String

            If Not InvokableMethod Is Nothing Then

                Return InvokableMethod.ToString

            ElseIf Not String.IsNullOrEmpty(Description) Then

                Return Description

            Else

                Return Name

            End If

        End Function
        
#End Region

    End Class

End Namespace