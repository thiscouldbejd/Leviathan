Namespace Commands

    Partial Public Class CommandInterrogated

#Region " Private Functions "

        Private Function GetMethods() As CommandInterrogatedMethod()

            If HasMethods Then

                Dim aryReturn(MethodIndexes.Length - 1) As CommandInterrogatedMethod

                For i As Integer = 0 To MethodIndexes.Length - 1

                    aryReturn(i) = Command.Methods(MethodIndexes(i))

                Next

                Return aryReturn

            Else

                Return New CommandInterrogatedMethod() {}

            End If

        End Function

#End Region

#Region " Public Properties "

        ''' <summary>
        ''' The Command Methods
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Methods() As CommandInterrogatedMethod()
            Get
                Return GetMethods()
            End Get
        End Property

        ''' <summary>
        ''' The Command Method (if only one)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Method() As CommandInterrogatedMethod
            Get
                If HasSingleMethod Then Return Methods(0) Else Return Nothing
            End Get
        End Property

        ''' <summary>
        ''' Whether there are Methods
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property HasMethods() As Boolean
            Get
                Return Not Command Is Nothing AndAlso Not MethodIndexes Is Nothing AndAlso MethodIndexes.Length > 0
            End Get
        End Property

        ''' <summary>
        ''' Whether this is a single Method
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property HasSingleMethod() As Boolean
            Get
                Return HasMethods AndAlso MethodIndexes.Length = 1
            End Get
        End Property

        Public ReadOnly Property FullName() As String
            Get
                If Not Command Is Nothing AndAlso HasSingleMethod Then
                    If Method.IsDefault Then
                        Return Command.Name
                    Else
                        Return Command.Name & SPACE & Method.Name
                    End If
                Else
                    Return Nothing
                End If
            End Get
        End Property

#End Region

#Region " Public Functions "

        Public Function AddMethodIndex( _
            ByVal index As Integer _
        ) As CommandInterrogated

            MethodIndexes = TypeAnalyser.AddTo(MethodIndexes, index)

            Return Me

        End Function

        Public Function AddMethodIndex( _
            ByVal indexes As Integer() _
        ) As CommandInterrogated

            For i As Integer = 0 To indexes.Length - 1
                MethodIndexes = TypeAnalyser.AddTo(MethodIndexes, indexes(i))
            Next

            Return Me

        End Function

        Public Function ToStrings( _
            Optional ByVal args As String() = Nothing _
        ) As String()

            If HasMethods Then

                Dim l_Methods As CommandInterrogatedMethod() = Methods

                Dim aryReturn(l_Methods.Length - 1) As String

                For i As Integer = 0 To l_Methods.Length - 1

                    aryReturn(i) = CommandString(Command, l_Methods(i), args, True, False)

                Next

                Return aryReturn

            Else

                Return New String() {}

            End If

        End Function

#End Region

#Region " Private Shared Methods "

        Private Shared Function CommandString( _
            ByVal command As CommandInterrogatedClass, _
            ByVal method As CommandInterrogatedMethod, _
            ByVal args As String(), _
            Optional ByVal includeParameters As Boolean = False, _
            Optional ByVal includeDescriptionRatherThanName As Boolean = False _
        ) As String

            Dim commandStringBuilder As New System.Text.StringBuilder
            Dim argCount As Integer = 1

            If Not command Is Nothing Then

                If includeDescriptionRatherThanName Then

                    commandStringBuilder.Append(command.Description)

                Else

                    If Not args Is Nothing AndAlso args.Length > argCount Then

                        commandStringBuilder.Append(args(argCount - 1))

                    Else

                        commandStringBuilder.Append(command.Name)

                    End If

                End If

            End If

            If Not method Is Nothing Then

                If Not method.IsDefault Then

                    argCount += 1

                    If commandStringBuilder.Length > 0 Then commandStringBuilder.Append(SPACE)

                    If includeDescriptionRatherThanName Then

                        commandStringBuilder.Append(method.Description)

                    Else

                        If Not args Is Nothing AndAlso args.Length > argCount Then

                            commandStringBuilder.Append(args(argCount - 1))

                        Else

                            commandStringBuilder.Append(method.Name)

                        End If

                    End If

                End If

                If includeParameters Then

                    If method.ParameterCount > 0 Then

                        For i As Integer = 0 To method.Parameters.Length - 1

                            If commandStringBuilder.Length > 0 Then commandStringBuilder.Append(SPACE)

                            If includeDescriptionRatherThanName Then

                                commandStringBuilder.Append(SQUARE_BRACKET_START)
                                commandStringBuilder.Append(method.Parameters(i).Description)
                                commandStringBuilder.Append(SQUARE_BRACKET_END)

                            Else

                                If Not args Is Nothing AndAlso args.Length > argCount + i Then

                                    If (argCount + i) = args.Length - 1 AndAlso _
                                        (SQUARE_BRACKET_START & method.Parameters(i).Name).StartsWith(args((argCount) + i)) Then

                                        commandStringBuilder.Append(SQUARE_BRACKET_START)
                                        commandStringBuilder.Append(method.Parameters(i).Name)
                                        commandStringBuilder.Append(SQUARE_BRACKET_END)

                                    Else

                                        commandStringBuilder.Append(args((argCount) + i))

                                    End If

                                Else

                                    commandStringBuilder.Append(SQUARE_BRACKET_START)
                                    commandStringBuilder.Append(method.Parameters(i).Name)
                                    commandStringBuilder.Append(SQUARE_BRACKET_END)

                                End If

                            End If

                        Next

                    End If

                End If

            End If

            Return commandStringBuilder.ToString

        End Function

#End Region

    End Class

End Namespace
