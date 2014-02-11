Namespace Commands

    Partial Public Class CommandInterrogatedFlag

#Region " Public Properties "

        Public ReadOnly Property Name() As String
            Get
                If Not FlagAttribute Is Nothing Then Return FlagAttribute.ToString() Else Return String.Empty
            End Get
        End Property

#End Region

#Region " Friend Shared Methods "

        Friend Shared Function GetFlags( _
            ByVal commandAnalyser As TypeAnalyser _
        ) As CommandInterrogatedFlag()

            Dim fieldAnalysers As MemberAnalyser() = _
                commandAnalyser.ExecuteQuery( _
                    AnalyserQuery.QUERY_MEMBERS_READWRITE _
                    .SetPresentAttribute(GetType(ConfigurableAttribute)) _
                )

            Dim l_Flags As New SortedList

            If Not fieldAnalysers Is Nothing Then

                For Each fieldAnalyser As MemberAnalyser In fieldAnalysers

                    Dim attrs As ConfigurableAttribute() = _
                        fieldAnalyser.Member.GetCustomAttributes(GetType(ConfigurableAttribute), False)

                    For Each attr As ConfigurableAttribute In attrs

                        l_Flags.Add( _
                            attr.ToString(), _
                            New CommandInterrogatedFlag(attr, fieldAnalyser.Name, fieldAnalyser.ReturnType) _
                        )

                    Next

                Next

            End If

            Dim retArray As CommandInterrogatedFlag() = _
                Array.CreateInstance(GetType(CommandInterrogatedFlag), l_Flags.Count)

            l_Flags.Values.CopyTo(retArray, 0)

            Return retArray

        End Function

#End Region

#Region " Public Shared Methods "

        ''' <summary>
        ''' Method to Parse and Set the Parsable Flags of a object.
        ''' </summary>
        ''' <param name="objectToPopulate">The Object to parse flag values to.</param>
        ''' <param name="commandClass">The Command Class for the Object.</param>
        ''' <param name="args">The Arguments to be Parsed.</param>
        ''' <returns></returns>
        ''' <remarks>This will no reset the flags if no parsed value is present.</remarks>
        Public Shared Function ParseFlags( _
            ByRef objectToPopulate As Object, _
            ByVal commandClass As CommandInterrogatedClass, _
            ByRef args As String(), _
            ByVal host As ICommandsExecution _
        ) As MemberAnalyser()

            Dim processedArgs As New List(Of String)
            Dim processedMembers As New List(Of MemberAnalyser)

            Dim analyser As TypeAnalyser = _
                TypeAnalyser.GetInstance(objectToPopulate.GetType)

            For Each arg As String In args

                Dim field_Values As IList = New ArrayList()
                Dim field_Names As IList = New ArrayList()
                Dim flag_Names As IList = New ArrayList()

                For Each flag As CommandInterrogatedFlag In commandClass.Flags

                    If String.IsNullOrEmpty(flag.FlagAttribute.Prefix) OrElse _
                        arg.StartsWith(flag.FlagAttribute.Prefix) Then

                        Dim argName As String
                        Dim argValue As String

                        If arg.IndexOf(COLON) > 0 Then

                            argName = arg.Substring(0, arg.IndexOf(COLON)) _
                                .TrimStart(flag.FlagAttribute.Prefix)

                            argValue = arg.Substring(arg.IndexOf(COLON) + 1)

                        Else

                            argName = arg.TrimStart(flag.FlagAttribute.Prefix)
                            argValue = Boolean.TrueString

                        End If

                        Dim exactMatch As Boolean = False

                        If Not String.IsNullOrEmpty( _
                            Comparison.Comparer.MatchFuzzyString(argName, flag.FlagAttribute.Name, exactMatch, HYPHEN)) Then

                            Dim successful_Parse As Boolean

                            Dim parsed_Value As Object = host.StringParser.Parse( _
                                argValue, successful_Parse, flag.FieldType)

                            If successful_Parse AndAlso exactMatch Then

                                field_Values.Clear()
                                field_Values.Add(parsed_Value)
                                field_Names.Clear()
                                field_Names.Add(flag.FieldName)
                                flag_Names.Clear()

                                Exit For

                            ElseIf successful_Parse Then

                                flag_Names.Add(flag.Name)
                                field_Values.Add(parsed_Value)
                                field_Names.Add(flag.FieldName)

                            End If

                        End If
                    End If

                Next

                If field_Values.Count > 0 Then

                    Dim field_Name As String = Nothing
                    Dim field_Value As Object = Nothing

                    If field_Values.Count = 1 Then

                        field_Name = field_Names(0)
                        field_Value = field_Values(0)

                    Else

                        Dim flag_Name As String = host.SelectionResponse( _
                            flag_Names, String.Format(ERROR_FLAG_AMBIGUOUS, arg))

                        If Not String.IsNullOrEmpty(flag_Name) Then

                            Dim selected_Index As Integer = flag_Names.IndexOf(flag_Name)

                            field_Name = field_Names(selected_Index)
                            field_Value = field_Values(selected_Index)

                        End If

                    End If

                    If Not String.IsNullOrEmpty(field_Name) Then

                        Dim member As MemberAnalyser = _
                            analyser.GetMember(field_Name)

                        If Not member Is Nothing Then
                        	member.Write(objectToPopulate, field_Value)
                        	processedMembers.Add(member)
                        End If
                            

                    End If

                    processedArgs.Add(arg)

                End If

            Next

            ReconcileArraysBySubtraction(processedArgs.ToArray(), args)
                        
			Return processedMembers.ToArray()

        End Function

#End Region

    End Class

End Namespace