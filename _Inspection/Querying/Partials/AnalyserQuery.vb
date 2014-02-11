Imports Leviathan.Caching
Imports System.Runtime.Serialization

Namespace Inspection

    Public Class AnalyserQuery
        Implements IPreQueryPart, IPostQueryPart

#Region " Public Shared Variables "
        ''' <summary>
        ''' Public Shared Reference to the Formatted Name of the Pre Parts Property.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared PROPERTY_PREPARTS As String = "PreParts"

        ''' <summary>
        ''' Public Shared Reference to the Formatted Name of the Post Parts Property.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared PROPERTY_POSTPARTS As String = "PostParts"

#End Region

#Region " Public Properties "

        ''' <summary>
        ''' Actual Return Type of this Query
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ActualReturnType() As System.Type
            Get
                Return GetReturnType(ReturnType)
            End Get
        End Property

        ''' <summary>
        ''' Provides Access to the Pre-Query Parts.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PreParts() As IPreQueryPart()
            Get
                Dim aryReturnList As New ArrayList

                For i As Integer = 0 To Parts.Count - 1

                    If TypeAnalyser.DoesTypeImplementInterface( _
                        Parts(i).GetType, GetType(IPreQueryPart)) Then

                        aryReturnList.Add(Parts(i))

                    End If

                Next

                Return aryReturnList.ToArray(GetType(IPreQueryPart))
            End Get
        End Property

        ''' <summary>
        ''' Provides Access to the Post-Query Parts.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PostParts() As IPostQueryPart()
            Get
                Dim aryReturnList As New ArrayList

                For i As Integer = 0 To Parts.Count - 1

                    If TypeAnalyser.DoesTypeImplementInterface( _
                        Parts(i).GetType, GetType(IPostQueryPart)) Then

                        aryReturnList.Add(Parts(i))

                    End If

                Next

                Return aryReturnList.ToArray(GetType(IPostQueryPart))
            End Get
        End Property

#End Region

#Region " Protected Methods "

        ''' <summary>
        ''' Binding Flags for a Reflection Bind.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function GetBindingFlags() As BindingFlags

            Dim bindingParts As IPreQueryPart() = Me.PreParts

            Dim returnFlags As BindingFlags = BindingFlags.DeclaredOnly

            If Not bindingParts Is Nothing Then

                For i As Integer = 0 To bindingParts.Length - 1

                    returnFlags = returnFlags Or bindingParts(i).BindingFlags

                Next

            End If

            Return returnFlags

        End Function

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Method to do Post-Query Compliancy Testing and conversion to a Typed Array.
        ''' </summary>
        ''' <param name="queryResults">The Collection of Results to Process.</param>
        ''' <returns>A Strongly-Typed Array of 0 or more Elements.</returns>
        ''' <remarks>A 0 Element Array means no elements matched, or too many were returned.</remarks>
        Public Function GetPostQueryReturnArray( _
            ByVal queryResults As ICollection _
        ) As Array

            Dim returnList As New ArrayList

            If Not queryResults Is Nothing Then

                For Each queryResult As Object In queryResults

                    If ReturnType = AnalyserType.AssemblyAnalyser Then

                        '''''''''''''''''''
                        ' NOT IMPLEMENTED '
                        '''''''''''''''''''
                        Throw New NotImplementedException

                    ElseIf ReturnType = AnalyserType.AttributeAnalyser Then

                        If CheckCompliancy(CType(queryResult, Attribute)) Then _
                            returnList.Add(New AttributeAnalyser(queryResult))

                    ElseIf ReturnType = AnalyserType.ConstructorAnalyser Then

                        If CheckCompliancy(CType(queryResult, ConstructorInfo)) Then _
                            returnList.Add( _
                                New MemberAnalyser( _
                                    CType(queryResult, ConstructorInfo).Name, _
                                    Nothing, Nothing, Nothing, Nothing, Nothing, queryResult))

                    ElseIf ReturnType = AnalyserType.ExceptionAnalyser Then

                        If CheckCompliancy(CType(queryResult, Exception)) Then _
                            returnList.Add(TypeAnalyser.GetInstance(queryResult))

                    ElseIf ReturnType = AnalyserType.FolderAnalyser Then

                        '''''''''''''''''''
                        ' NOT IMPLEMENTED '
                        '''''''''''''''''''
                        Throw New NotImplementedException

                    ElseIf ReturnType = AnalyserType.MemberAnalyser Then

                        If CType(queryResult, MemberInfo).MemberType = MemberTypes.Field Then

                            If CheckCompliancy(CType(queryResult, FieldInfo)) Then _
                                returnList.Add(New MemberAnalyser(CType(queryResult, FieldInfo).Name, _
                                    Nothing, Nothing, queryResult))

                        Else

                            Dim addMember As Boolean = CheckCompliancy(CType(queryResult, PropertyInfo))

                            If addMember Then

                                For i As Integer = 0 To returnList.Count - 1

                                    If CType(returnList(i), MemberAnalyser).Member.MemberType = MemberTypes.Property AndAlso _
                                        CType(returnList(i), MemberAnalyser).Property.Name = CType(queryResult, PropertyInfo).Name Then

                                        Dim params_1 As ParameterInfo() = CType(returnList(i), MemberAnalyser).Property.GetIndexParameters
                                        Dim params_2 As ParameterInfo() = CType(queryResult, PropertyInfo).GetIndexParameters

                                        If params_1.Length = params_2.Length Then

                                            addMember = False

                                            For j As Integer = 0 To params_1.Length - 1
                                                If Not params_1(j).ParameterType Is params_2(j).ParameterType Then _
                                                    addMember = True
                                                Exit For
                                            Next

                                        End If

                                        If Not addMember Then Exit For

                                    End If

                                Next

                            End If

                            If addMember Then returnList.Add(New MemberAnalyser(CType(queryResult, PropertyInfo).Name, _
                                Nothing, Nothing, Nothing, queryResult))

                        End If

                    ElseIf ReturnType = AnalyserType.MethodAnalyser Then

                        If CheckCompliancy(CType(queryResult, MethodInfo)) Then _
                            returnList.Add(New MemberAnalyser(CType(queryResult, MethodInfo).Name, _
                                Nothing, Nothing, Nothing, Nothing, queryResult))

                    ElseIf ReturnType = AnalyserType.TypeAnalyser Then

                        If CheckCompliancy(CType(queryResult, Type)) Then _
                            returnList.Add(TypeAnalyser.GetInstance(queryResult))

                    End If

                Next

            End If

            If NumberOfResults <= 0 OrElse returnList.Count = NumberOfResults Then

                Return returnList.ToArray(GetReturnType(ReturnType))

            Else

                Return Array.CreateInstance(GetReturnType(ReturnType), 0)

            End If

        End Function

        ''' <summary>
        ''' Allows Setting of the Type from which the return should be or inherited from.
        ''' </summary>
        ''' <param name="returnType">The Type from which the return should be or inherited from.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SetReturnTypeIsOrInheritedFromType( _
            ByVal returnType As Type _
        ) As AnalyserQuery

            Me.Parts.Add(New TypePart(returnType))
            Return Me

        End Function

        ''' <summary>
        ''' Allows Setting of the Interface Type from which the return should be implementing.
        ''' </summary>
        ''' <param name="interfaceType">The Type which the return should be implementing.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SetReturnTypeImplementsInterface( _
            ByVal interfaceType As Type _
        ) As AnalyserQuery

            Me.Parts.Add(New TypeImplementsPart(interfaceType))
            Return Me

        End Function

        ''' <summary>
        ''' Allows Setting of the Visibility of the Return Types.
        ''' </summary>
        ''' <param name="visibility"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SetVisibility( _
            ByVal visibility As MemberVisibility _
        ) As AnalyserQuery

            Me.Parts.Add(New VisibilityPart(visibility))
            Return Me

        End Function

        ''' <summary>
        ''' Allows Setting of the Location of the Return Types.
        ''' </summary>
        ''' <param name="location"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SetLocation( _
            ByVal location As MemberLocation _
        ) As AnalyserQuery

            Me.Parts.Add(New LocationPart(location))
            Return Me

        End Function

        ''' <summary>
        ''' Allows Setting of the Accessibility of the Return Types.
        ''' </summary>
        ''' <param name="accessibility"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SetAccessibility( _
            ByVal accessibility As MemberAccessibility _
        ) As AnalyserQuery

            Me.Parts.Add(New AccessibilityPart(accessibility))
            Return Me

        End Function

        ''' <summary>
        ''' Allows Setting of whether Return Types should be Variables (e.g. Fields).
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SetIsVariable( _
        ) As AnalyserQuery

            Me.Parts.Add(New VariablePart(True))
            Return Me

        End Function

        ''' <summary>
        ''' Allows Setting of whether Return Types should be Variables (e.g. Fields).
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SetNotIsVariable( _
        ) As AnalyserQuery

            Me.Parts.Add(New VariablePart(False))
            Return Me

        End Function

        ''' <summary>
        ''' Allow Setting of the name of the return member.
        ''' </summary>
        ''' <param name="name">The name to use.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SetName( _
            ByVal name As String _
        ) As AnalyserQuery

            Me.Parts.Add(New NamePart(New String() {name}))
            Return Me

        End Function

        ''' <summary>
        ''' Allow Setting of the names of the return member.
        ''' </summary>
        ''' <param name="names">The names to use.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SetNames( _
            ByVal ParamArray names() As String _
        ) As AnalyserQuery

            Me.Parts.Add(New NamePart(names))
            Return Me

        End Function

        ''' <summary>
        ''' Allow Setting of the argument count of the return member.
        ''' </summary>
        ''' <param name="argumentCount"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SetArgumentCount( _
            ByVal argumentCount As Integer _
        ) As AnalyserQuery

            Me.Parts.Add(New ArgumentPart(argumentCount))
            Return Me

        End Function

        ''' <summary>
        ''' Allows Adding of the Attributes which should be present on the return types.
        ''' </summary>
        ''' <param name="attributeType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SetPresentAttribute( _
            ByVal ParamArray attributeType() As System.Type _
        ) As AnalyserQuery

            If Not attributeType Is Nothing AndAlso attributeType.Length > 0 Then

                For i As Integer = 0 To attributeType.Length - 1

                    Me.Parts.Add(New AttributePart(attributeType(i)))

                Next

            End If

            Return Me

        End Function

        ''' <summary>
        ''' Allows Adding of the Attributes which shouldn't be present on the return types.
        ''' </summary>
        ''' <param name="attributeType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SetNotPresentAttribute( _
            ByVal ParamArray attributeType() As System.Type _
        ) As AnalyserQuery

            If Not attributeType Is Nothing AndAlso attributeType.Length > 0 Then

                For i As Integer = 0 To attributeType.Length - 1

                    Me.Parts.Add(New NoAttributePart(attributeType(i)))

                Next

            End If

            Return Me

        End Function

        ''' <summary>
        ''' Returns the Hashcode of this Query.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function GetHashCode() As Integer

            Return Simple.CombineHashCodes( _
                Me.GetType, ReturnType, DeclaredBelowType, _
                NumberOfResults, Parts.ToArray() _
            )

        End Function

#End Region

#Region " IPreQueryPart Implementation "

        ''' <summary>
        ''' Method to Get Binding Flags for Reflection Bindings.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property BindingFlags() As System.Reflection.BindingFlags _
            Implements IPreQueryPart.BindingFlags
            Get
                Return GetBindingFlags()
            End Get
        End Property

#End Region

#Region " IPostQueryPart Implementation "

        ''' <summary>
        ''' Method to Check Post-Query Compliancy with Paths (IPostQueryPart).
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckCompliancy( _
            ByVal value As FieldInfo _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Dim postParts As IPostQueryPart() = Me.PostParts

            If Not postParts Is Nothing Then

                For i As Integer = 0 To postParts.Length - 1

                    If Not postParts(i).CheckCompliancy(value) Then Return False

                Next

            End If

            Return True

        End Function

        ''' <summary>
        ''' Method to Check Post-Query Compliancy with Paths (IPostQueryPart).
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckCompliancy( _
            ByVal value As MethodInfo _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Dim postParts As IPostQueryPart() = Me.PostParts

            If Not postParts Is Nothing Then

                For i As Integer = 0 To postParts.Length - 1

                    If Not postParts(i).CheckCompliancy(value) Then Return False

                Next

            End If

            Return True

        End Function

        ''' <summary>
        ''' Method to Check Post-Query Compliancy with Paths (IPostQueryPart).
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckCompliancy( _
            ByVal value As PropertyInfo _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Dim postParts As IPostQueryPart() = Me.PostParts

            If Not postParts Is Nothing Then

                For i As Integer = 0 To postParts.Length - 1

                    If Not postParts(i).CheckCompliancy(value) Then Return False

                Next

            End If

            Return True

        End Function

        ''' <summary>
        ''' Method to Check Post-Query Compliancy with Paths (IPostQueryPart).
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckCompliancy( _
            ByVal value As Attribute _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Dim postParts As IPostQueryPart() = Me.PostParts

            If Not postParts Is Nothing Then

                For i As Integer = 0 To postParts.Length - 1

                    If Not postParts(i).CheckCompliancy(value) Then Return False

                Next

            End If

            Return True

        End Function

        ''' <summary>
        ''' Method to Check Post-Query Compliancy with Paths (IPostQueryPart).
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckCompliancy( _
            ByVal value As Exception _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Dim postParts As IPostQueryPart() = Me.PostParts

            If Not postParts Is Nothing Then

                For i As Integer = 0 To postParts.Length - 1

                    If Not postParts(i).CheckCompliancy(value) Then Return False

                Next

            End If

            Return True

        End Function

        ''' <summary>
        ''' Method to Check Post-Query Compliancy with Paths (IPostQueryPart).
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckCompliancy( _
            ByVal value As System.Type _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Dim postParts As IPostQueryPart() = Me.PostParts

            If Not postParts Is Nothing Then

                For i As Integer = 0 To postParts.Length - 1

                    If Not postParts(i).CheckCompliancy(value) Then Return False

                Next

            End If

            Return True

        End Function

        ''' <summary>
        ''' Method to Check Post-Query Compliancy with Paths (IPostQueryPart).
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckCompliancy( _
            ByVal value As ConstructorInfo _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Dim postParts As IPostQueryPart() = Me.PostParts

            If Not postParts Is Nothing Then

                For i As Integer = 0 To postParts.Length - 1

                    If Not postParts(i).CheckCompliancy(value) Then Return False

                Next

            End If

            Return True

        End Function

#End Region

#Region " Public Shared Methods "

        ''' <summary>
        ''' Provides Access to the Actual Return Type of an EntityType.
        ''' </summary>
        ''' <returns>The Type of Nothing.</returns>
        ''' <remarks></remarks>
        Public Shared Function GetReturnType( _
            ByVal type As AnalyserType _
        ) As Type
            Select Case type

                Case AnalyserType.AssemblyAnalyser

                    Return GetType(AssemblyAnalyser)

                Case AnalyserType.AttributeAnalyser

                    Return GetType(AttributeAnalyser)

                Case AnalyserType.ExceptionAnalyser

                    Return GetType(TypeAnalyser)

                Case AnalyserType.FolderAnalyser

                    Return GetType(FolderAnalyser)

                Case AnalyserType.MemberAnalyser

                    Return GetType(MemberAnalyser)

                Case AnalyserType.MethodAnalyser

                    Return GetType(MemberAnalyser)

                Case AnalyserType.ConstructorAnalyser

                    Return GetType(MemberAnalyser)

                Case AnalyserType.TypeAnalyser

                    Return GetType(TypeAnalyser)

                Case Else

                    Return GetType(Object)

            End Select
        End Function

#End Region

#Region " Common Shared Queries "

#Region " Member Queries "

        Public Shared Function QUERY_PUBLIC_READABLE_PROPERTIES() As AnalyserQuery

            Return New AnalyserQuery(AnalyserType.MemberAnalyser) _
                .SetNotIsVariable() _
                .SetLocation(MemberLocation.Instance) _
                .SetVisibility(MemberVisibility.Public) _
                .SetAccessibility(MemberAccessibility.Readable) _
                .SetArgumentCount(0)

        End Function
        
        Public Shared Function QUERY_PUBLIC_READABLE_PROPERTIESORFIELDS() As AnalyserQuery

            Return New AnalyserQuery(AnalyserType.MemberAnalyser) _
                .SetLocation(MemberLocation.Instance) _
                .SetVisibility(MemberVisibility.Public) _
                .SetAccessibility(MemberAccessibility.Readable) _
                .SetArgumentCount(0)

        End Function

        Public Shared Function QUERY_MEMBERS() As AnalyserQuery

            Return New AnalyserQuery(AnalyserType.MemberAnalyser) _
                .SetLocation(MemberLocation.All) _
                .SetVisibility(MemberVisibility.All) _
                .SetArgumentCount(0)

        End Function

        Public Shared Function QUERY_MEMBERS_READABLE() As AnalyserQuery

            Return QUERY_MEMBERS _
                .SetAccessibility(MemberAccessibility.Readable)

        End Function

        Public Shared Function QUERY_MEMBERS_READWRITE() As AnalyserQuery

            Return QUERY_MEMBERS _
                .SetAccessibility(MemberAccessibility.All)

        End Function

        Public Shared Function QUERY_MEMBERS_WRITEABLE() As AnalyserQuery

            Return QUERY_MEMBERS _
                .SetAccessibility(MemberAccessibility.Writable)

        End Function

#End Region

#Region " Variable Queries "

        Public Shared Function QUERY_VARIABLES() As AnalyserQuery

            Return QUERY_MEMBERS _
                .SetAccessibility(MemberAccessibility.All) _
                .SetIsVariable()

        End Function

#End Region

#Region " Method Queries "

        Public Shared Function QUERY_METHODS_PUBLIC_INSTANCE() As AnalyserQuery

            Return New AnalyserQuery(AnalyserType.MethodAnalyser) _
                .SetLocation(MemberLocation.Instance) _
                .SetVisibility(MemberVisibility.Public)

        End Function

#End Region

#End Region

    End Class

End Namespace