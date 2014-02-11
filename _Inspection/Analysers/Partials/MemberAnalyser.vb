Imports System.Text

Namespace Inspection

    Partial Public Class MemberAnalyser

#Region " Public Constants "

        ''' <summary>
        ''' Delineater used in conjunction with Multi-Level Members
        ''' </summary>
        ''' <remarks></remarks>
        Public Const MEMBER_DELINEATOR As String = FULL_STOP

        ''' <summary>
        ''' Constant for Generating Signatures
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SIGNATURE_NEW As String = "New"

        ''' <summary>
        ''' Constant for Generating Signatures
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SIGNATURE_AS As String = "As"

        ''' <summary>Public Shared Reference to the Name of the Property: Level</summary>
        ''' <remarks></remarks>
        Public Const PROPERTY_Level As String = "Level"

        ''' <summary>Public Shared Reference to the Name of the Property: FullName</summary>
        ''' <remarks></remarks>
        Public Const PROPERTY_FullName As String = "FullName"

#End Region

#Region " Public Properties "

        Public ReadOnly Property FullName() As String
            Get
                If Child Is Nothing Then Return Name Else Return Name & MEMBER_DELINEATOR & Child.FullName
            End Get
        End Property

        Public ReadOnly Property IsMultiLeveled() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Name) AndAlso Not Child Is Nothing
            End Get
        End Property

        Public ReadOnly Property Level() As Int32
            Get
                If String.IsNullOrEmpty(Name) OrElse Child Is Nothing Then Return 0 Else Return Child.Level + 1
            End Get
        End Property

        ''' <summary>
        ''' Whether the Member is a Field
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsField() As Boolean
            Get
                Return Not [Field] Is Nothing
            End Get
        End Property

        ''' <summary>
        ''' Whether the Member is a Property
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsProperty() As Boolean
            Get
                Return Not [Property] Is Nothing
            End Get
        End Property

        ''' <summary>
        ''' Whether the Member is a Method
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsMethod() As Boolean
            Get
                Return Not [Method] Is Nothing
            End Get
        End Property

        ''' <summary>
        ''' Whether the Member is a Constructor
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsConstructor() As Boolean
            Get
                Return Not [Constructor] Is Nothing
            End Get
        End Property

        Public Property Member() As System.Reflection.MemberInfo
            Get
                If IsField Then
                    Return Field
                ElseIf IsProperty Then
                    Return [Property]
                ElseIf IsMethod Then
                    Return [Method]
                ElseIf IsConstructor Then
                    Return Constructor
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As System.Reflection.MemberInfo)
                If value.MemberType = MemberTypes.Field Then Field = value Else If value.MemberType = MemberTypes.Property Then [Property] = value Else If value.MemberType = MemberTypes.Method Then Method = value Else If value.MemberType = MemberTypes.Constructor Then Constructor = value
            End Set
        End Property

        ''' <summary>
        ''' Type in which the Member was Declared
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Type() As System.Type
            Get
                If Not Member Is Nothing Then Return Member.DeclaringType Else Return Nothing
            End Get
        End Property

        ''' <summary>
        ''' The Return Type (eventual) of this Analyser
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ReturnType() As System.Type
            Get
                If IsMultiLeveled Then Return Child.ReturnType Else If Not Member Is Nothing Then Return TypeAnalyser.GetMemberReturnType(Member) Else Return Nothing
            End Get
        End Property

        ''' <summary>
        ''' The Return Type Analyser (eventual) of this Analyser
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ReturnTypeAnalyser() As TypeAnalyser
            Get
                If Not ReturnType Is Nothing Then Return TypeAnalyser.GetInstance(ReturnType) Else Return Nothing
            End Get
        End Property

        ''' <summary>
        ''' Type Signature of the Arguments
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ArgumentSignature() As System.Type()
            Get
                If Not Member Is Nothing Then Return TypeAnalyser.GetMemberArgumentSignature(Member) Else Return Nothing
            End Get
        End Property

        ''' <summary>
        ''' Length of the Arguments
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ArgumentSignatureLength() As Int32
            Get
                If ArgumentSignature Is Nothing Then Return 0 Else Return ArgumentSignature.Length
            End Get
        End Property

        ''' <summary>
        ''' Whether the Member is Readable
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsReadable() As Boolean
            Get
                Return IsField OrElse IsConstructor OrElse (IsProperty AndAlso [Property].CanRead) OrElse (IsMethod AndAlso Not Method.ReturnType Is Nothing)
            End Get
        End Property

        ''' <summary>
        ''' Whether the Member is Writeable
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsWriteable() As Boolean
            Get
                Return (IsField AndAlso Not Field.IsInitOnly) OrElse (IsProperty AndAlso [Property].CanWrite)
            End Get
        End Property

        ''' <summary>
        ''' Provides Access to the Name of the Property as a Multi-Leveled String Array.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Names() As String()
            Get
                If String.IsNullOrEmpty(Name) Then

                    Return New String() {}

                ElseIf Child Is Nothing Then

                    Return New String() {Name}

                Else

                    Return InsertAt(Child.Names, Name, 0)

                End If
            End Get
        End Property

        ''' <summary>
        ''' Whether the Member is Readable on a particular Analyser.
        ''' </summary>
        ''' <param name="onAnalyser">The Analyser to check on.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsReadable( _
           ByVal onAnalyser As TypeAnalyser _
       ) As Boolean
            Get

                If IsField OrElse (IsProperty AndAlso [Property].CanRead) Then

                    If TypeAnalyser.IsSubClassOf(onAnalyser.GetType, GetType(TypeAnalyser)) Then

                        If TypeAnalyser.IsSubClassOf(CType(onAnalyser, TypeAnalyser).Type, _
                            Member.DeclaringType) Then

                            Return True

                        End If

                    End If

                End If

                Return False

            End Get
        End Property

        ''' <summary>
        ''' Whether the Member is Writebale on a particular Analyser.
        ''' </summary>
        ''' <param name="onAnalyser">The Analyser to check on.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsWriteable( _
            ByVal onAnalyser As TypeAnalyser _
        ) As Boolean
            Get

                Return Me.IsWriteable AndAlso (onAnalyser.GetType Is GetType(TypeAnalyser) _
                    AndAlso TypeAnalyser.IsSubClassOf(CType(onAnalyser, TypeAnalyser).Type, _
                        Member.DeclaringType))

            End Get
        End Property

#End Region

#Region " Protected Methods "

        ''' <summary>
        ''' This method performs a Read at this Member Level (e.g. Ignores Child).
        ''' </summary>
        ''' <param name="source">A Single Object to Read from (cannot be null).</param>
        ''' <param name="arguments">Optional Arguments for this Member.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function InternalReadFromObject( _
            ByVal source As Object, _
            Optional ByVal arguments As Object() = Nothing _
        ) As Object

            If IsField Then

                Return Field.GetValue(source)

            ElseIf IsProperty Then

                If arguments Is Nothing OrElse arguments.Length = 0 Then

                    Return [Property].GetValue(source, Nothing)

                Else

                    Return [Property].GetValue(source, arguments)

                End If

            ElseIf IsMethod Then

                Return Method.Invoke(source, arguments)

            End If

            Return Nothing

        End Function

        ''' <summary>
        ''' This method performs a Read at this Member Level (e.g. Ignores Child).
        ''' </summary>
        ''' <param name="source">Can be either a Single Object or Collection to Read from (cannot be null).</param>
        ''' <param name="arguments">Optional Arguments for this Member.</param>
        ''' <returns>Either a Single Object, an Array of Objects or Nothing</returns>
        ''' <remarks></remarks>
        Protected Overridable Function InternalReadFromObjectOrCollection( _
            ByVal source As Object, _
            Optional ByVal arguments As Object() = Nothing _
        ) As Object

            If IsReadable Then

                If TypeAnalyser.IsSubClassOf(source.GetType, Member.DeclaringType) Then

                    Return InternalReadFromObject(source, arguments)

                Else

                    Dim sourceAnalyser As TypeAnalyser = TypeAnalyser.GetInstance(source.GetType)

                    If sourceAnalyser.IsICollection AndAlso _
                        TypeAnalyser.IsSubClassOf(TypeAnalyser.GetElementType(source), Member.DeclaringType) Then

                        Dim returnList As New List(Of Object)

                        For Each single_Object As Object In CType(source, ICollection)

                            returnList.Add(InternalReadFromObject(single_Object, arguments))

                        Next

                        Return returnList.ToArray

                    End If

                End If

            End If

            Return Nothing

        End Function

        ''' <summary>
        ''' This method performs a Write at this Member Level (e.g. Ignores Child).
        ''' </summary>
        ''' <param name="target">A Single Object to Write to (cannot be null).</param>
        ''' <param name="value">The Value to Write (can be null).</param>
        ''' <param name="arguments">Optional Arguments for this Member.</param>
        ''' <returns>A Boolean Value indicating Success or Failure.</returns>
        ''' <remarks></remarks>
        Protected Overridable Function InternalWriteToObject( _
            ByVal target As Object, _
            ByVal value As Object, _
            Optional ByVal arguments As Object() = Nothing _
        )

            If IsField Then

                Field.SetValue(target, value)

                Return True

            ElseIf IsProperty Then

                If arguments Is Nothing OrElse arguments.Length = 0 Then

                    [Property].SetValue(target, value, Nothing)

                Else

                    [Property].SetValue(target, value, arguments)

                End If


                Return True

            End If

            Return False

        End Function

        ''' <summary>
        ''' This method performs a Write at this Member Level (e.g. Ignores Child).
        ''' </summary>
        ''' <param name="target">Can be either a Single Object or Collection to Write to (cannot be null).</param>
        ''' <param name="value">The Value to Write (can be null).</param>
        ''' <param name="arguments">Optional Arguments for this Member.</param>
        ''' <returns>A Boolean Value indicating Success (complete) or Failure.</returns>
        ''' <remarks></remarks>
        Protected Overridable Function InternalWriteToObjectOrCollection( _
            ByVal target As Object, _
            ByVal value As Object, _
            Optional ByVal arguments As Object() = Nothing _
        ) As Boolean

            If IsWriteable Then

                If TypeAnalyser.IsSubClassOf(target.GetType, Member.DeclaringType) Then

                    Return InternalWriteToObject(target, value, arguments)

                Else

                    Dim sourceAnalyser As TypeAnalyser = TypeAnalyser.GetInstance(target.GetType)

                    If sourceAnalyser.IsICollection AndAlso _
                        TypeAnalyser.IsSubClassOf(sourceAnalyser.ElementType, Member.DeclaringType) Then

                        For Each single_Target As Object In CType(target, ICollection)

                            If Not InternalWriteToObject(single_Target, value, arguments) Then _
                                Return False

                        Next

                        Return True

                    End If

                End If

            End If

            Return False

        End Function

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Method to Perform a Read (on a particular object).
        ''' </summary>
        ''' <param name="sourceObject">The Object upon which to Read.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Read( _
            ByVal sourceObject As Object, _
            Optional ByVal ignoreChildren As Boolean = False _
        ) As Object

            Return Read(sourceObject, DefaultArguments, ignoreChildren)

        End Function

        ''' <summary>
        ''' Method to Perform a Read (on a particular object).
        ''' </summary>
        ''' <param name="sourceObject">The Object upon which to Read.</param>
        ''' <param name="arguments">The Arguments to Use in the Read.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Read( _
            ByVal sourceObject As Object, _
            ByVal arguments() As Object, _
            Optional ByVal ignoreChildren As Boolean = False _
        ) As Object

            Dim childSourceObject As Object = _
                    InternalReadFromObjectOrCollection(sourceObject, arguments)

            If Not ignoreChildren AndAlso IsMultiLeveled AndAlso Not childSourceObject Is Nothing Then

                Return Child.Read(childSourceObject)

            Else

                Return childSourceObject

            End If

        End Function

        ''' <summary>
        ''' Method to Perform a Write (on a particular object).
        ''' </summary>
        ''' <param name="targetObject">The Object upon which to Write.</param>
        ''' <param name="value">The Value to Write.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Write( _
            ByRef targetObject As Object, _
            ByVal value As Object, _
            Optional ByVal ignoreChildren As Boolean = False _
        ) As Boolean

            Return Write(targetObject, value, DefaultArguments, ignoreChildren)

        End Function

        ''' <summary>
        ''' Method to Perform a Write (on a particular object).
        ''' </summary>
        ''' <param name="targetObject">The Object upon which to Write.</param>
        ''' <param name="value">The Value to Write.</param>
        ''' <param name="arguments">The Arguments to Use in the Write.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Write( _
            ByRef targetObject As Object, _
            ByVal value As Object, _
            ByVal arguments() As Object, _
            Optional ByVal ignoreChildren As Boolean = False _
        ) As Boolean

            If Not ignoreChildren AndAlso IsMultiLeveled Then

                Dim childTargetObject As Object = _
                    InternalReadFromObjectOrCollection(targetObject, arguments)

                ' This is essential to make single/collection work.
                If Not childTargetObject Is Nothing Then _
                    Return Child.Write(childTargetObject, value)

            Else

                Return InternalWriteToObjectOrCollection(targetObject, value, arguments)

            End If

            Return False

        End Function

        ''' <summary>
        ''' Method to Perform a Parse.
        ''' </summary>
        ''' <param name="value">The object to parse (as a string).</param>
        ''' <returns>A Boolean value indicating success.</returns>
        ''' <remarks></remarks>
        Public Overridable Function Parse( _
            Optional ByVal value As String = Nothing, _
            Optional ByVal currentType As Type = Nothing, _
            Optional ByVal onType As TypeAnalyser = Nothing, _
            Optional ByVal argumentCount As Integer = 0, _
            Optional ByVal accessiblity As MemberAccessibility = MemberAccessibility.Readable _
        ) As Boolean

            If Not String.IsNullOrEmpty(value) Then

                Dim child_value As String = Nothing

                If value.Contains(MEMBER_DELINEATOR) AndAlso value.IndexOf(MEMBER_DELINEATOR) > 0 Then

                    child_value = value.Substring(value.IndexOf(MEMBER_DELINEATOR) + 1)

                    If Not String.IsNullOrEmpty(child_value) Then

                        If currentType Is Nothing Then

                            Child = Activator.CreateInstance(Me.GetType)
                            Child.Parse(child_value)

                        ElseIf currentType Is GetType(MemberAnalyser) Then

                            Child = New MemberAnalyser(child_value)

                        Else

                            Child = Activator.CreateInstance(currentType)
                            Child.Parse(child_value)

                        End If

                    End If

                    m_Name = value.Substring(0, value.IndexOf(MEMBER_DELINEATOR))

                Else

                    m_Name = value

                End If

            End If

            If Not String.IsNullOrEmpty(Name) Then

                If Not onType Is Nothing Then

                    Dim analyser As MemberAnalyser = GetSingleObject( _
                        onType.ExecuteQuery( _
                            New AnalyserQuery() _
                                .SetReturnType(AnalyserType.MemberAnalyser) _
                                .SetVisibility(MemberVisibility.All) _
                                .SetName(Name) _
                                .SetLocation(MemberLocation.All) _
                                .SetAccessibility(accessiblity) _
                                .SetArgumentCount(argumentCount) _
                                .SetNumberOfResults(1) _
                            ) _
                        )

                    If analyser Is Nothing AndAlso onType.IsICollection Then

                        analyser = GetSingleObject( _
                            onType.ElementTypeAnalyser.ExecuteQuery( _
                                New AnalyserQuery() _
                                    .SetReturnType(AnalyserType.MemberAnalyser) _
                                    .SetVisibility(MemberVisibility.All) _
                                    .SetName(Name) _
                                    .SetLocation(MemberLocation.All) _
                                    .SetAccessibility(accessiblity) _
                                    .SetArgumentCount(argumentCount) _
                                    .SetNumberOfResults(1) _
                                ) _
                            )

                    End If

                    If analyser Is Nothing Then
                        analyser = GetSingleObject( _
                        onType.ExecuteQuery( _
                            New AnalyserQuery() _
                                .SetReturnType(AnalyserType.MethodAnalyser) _
                                .SetDeclaredBelowType(GetType(Object)) _
                                .SetVisibility(MemberVisibility.All) _
                                .SetName(Name) _
                                .SetLocation(MemberLocation.All) _
                                .SetAccessibility(accessiblity) _
                                .SetArgumentCount(argumentCount) _
                                .SetNumberOfResults(1) _
                            ) _
                        )
                    End If

                    If Not analyser Is Nothing Then

                        Name = analyser.Member.Name
                        Member = analyser.Member

                        If IsMultiLeveled Then Return Child.Parse(Nothing, currentType, analyser.ReturnTypeAnalyser, 0, accessiblity)

                    Else

                        Return False

                    End If

                End If

                Return True

            End If

            Return False

        End Function

        ''' <summary>
        ''' Method to Create a String Representation of the Member.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function ToString() As String

            If IsMethod Then

				Dim builder As New StringBuilder
				
                If Method.IsConstructor Then builder.Append(SIGNATURE_NEW) Else builder.Append(Name)

                builder.Append(BRACKET_START)

                Dim params As ParameterInfo() = Method.GetParameters

                If Not params Is Nothing AndAlso params.Length > 0 Then

                    For i As Integer = 0 To params.Length - 1

                        If i > 0 Then
                            builder.Append(COMMA)
                            builder.Append(SPACE)
                        End If

                        builder.Append(params(i).Name)

                        builder.Append(SPACE)
                        builder.Append(SIGNATURE_AS)
                        builder.Append(SPACE)
                        builder.Append(params(i).ParameterType.Name)

                    Next

                End If

                builder.Append(BRACKET_END)

				Return builder.ToString
				
			Else
                
                Return FullName
                
            End If

        End Function

        Public Overloads Function Equals( _
            ByVal value As MemberAnalyser _
        ) As Boolean

            Return Equals(value, False)

        End Function

        Public Overloads Function Equals( _
            ByVal value As MemberAnalyser, _
            ByVal ignoreChildren As Boolean _
        ) As Boolean

            If value Is Nothing Then

                Return False

            Else

                If String.Compare(Name, value.Name, True) <> 0 Then

                    Return False

                Else

                End If

                If Not Type Is value.Type Then Return False

                If Not ignoreChildren AndAlso Not Child Is Nothing Then Return Child.Equals(value.Child)

            End If

            Return True

        End Function

#End Region

#Region " Public Shared Methods "

        ''' <summary>
        ''' Method to Attempt a Parse From String for the Class.
        ''' </summary>
        ''' <param name="value">The Value to Parse.</param>
        ''' <param name="result">The ByRef/Out Parameter to write the parsed object to.</param>
        ''' <returns>A Boolean indicating whether the parse was successful.</returns>
        ''' <remarks></remarks>
        Public Shared Function TryParse( _
            ByVal value As String, _
            ByRef result As MemberAnalyser _
        ) As Boolean

            result = New MemberAnalyser

            Return result.Parse(value)

        End Function
        
        ''' <summary>
        ''' Method to preform a quick parse for a particular Member on a Type.
        ''' </summary>
        ''' <param name="value">The Name of the Member.</param>
        ''' <param name="onType">The Type on which it exists (as an Analyser)</param>
        ''' <returns></returns>
        Public Shared Function Parse( _
        	ByVal value As String, _
        	ByVal onType As TypeAnalyser _
        ) As MemberAnalyser
        
        	Dim return_Member As New MemberAnalyser()
        	If return_Member.Parse(value, Nothing, onType) Then Return return_Member
        	
        	Return Nothing
	            
        End Function

        ''' <summary>
        ''' This method combines property names to create the name of a multi-layer property.
        ''' </summary>
        ''' <param name="memberNames">The member names to combine.</param>
        ''' <returns>A string representing the name of the multi-layer property, or Nothing.</returns>
        ''' <remarks></remarks>
        Public Shared Function CombineMembers( _
            ByVal ParamArray memberNames As String() _
        ) As String

            If memberNames Is Nothing OrElse memberNames.Length = 0 Then

                Return Nothing

            ElseIf memberNames.Length = 1 Then

                Return memberNames(0)

            Else

                Dim sb As New System.Text.StringBuilder()

                For i As Integer = 0 To memberNames.Length - 2
                    If Not memberNames(i) = Nothing Then
                        sb.Append(memberNames(i))
                        sb.Append(MemberAnalyser.MEMBER_DELINEATOR)
                    End If
                Next

                sb.Append(memberNames(memberNames.Length - 1))

                Return sb.ToString

            End If

        End Function

        ''' <summary>
        ''' This method combines member names to create the name of a multi-layer member.
        ''' </summary>
        ''' <param name="members">The members to combine.</param>
        ''' <returns>A string representing the name of the multi-layer member, or Nothing.</returns>
        ''' <remarks></remarks>
        Public Shared Function CombineMembers( _
            ByVal ParamArray members As MemberAnalyser() _
        ) As String

            If Not members Is Nothing Then

                Dim lstMemberNames As New ArrayList

                For i As Integer = 0 To members.Length - 1

                    lstMemberNames.AddRange(members(i).Names)

                Next

                Dim aryMemberNames As String() = lstMemberNames.ToArray(GetType(String))

                Return CombineMembers(aryMemberNames)

            End If

            Return Nothing

        End Function

        ''' <summary>
        ''' This method will read values from a source object according to the members supplied.
        ''' </summary>
        ''' <param name="source">The Object to Read From.</param>
        ''' <param name="members">The Members to Read.</param>
        ''' <returns>An IList of Values.</returns>
        ''' <remarks></remarks>
        Public Shared Function ReadMembers( _
            ByVal source As Object, _
            ByVal members() As MemberAnalyser _
        ) As IList

            Dim return_List As New ArrayList

            If Not members Is Nothing Then

                For i As Integer = 0 To members.Length - 1

                    return_List.Add(members(i).Read(source))

                Next

            End If

            Return return_List

        End Function

        ''' <summary>
        ''' This method will read values from a source object according the the query supplied.
        ''' </summary>
        ''' <param name="source">The Object to Read From.</param>
        ''' <param name="query">The Query to Read.</param>
        ''' <returns>An IList of Values.</returns>
        ''' <remarks></remarks>
        Public Shared Function ReadMembers( _
            ByVal source As Object, _
            ByVal query As AnalyserQuery _
        ) As IList

            Dim return_List As New ArrayList

            If Not source Is Nothing Then

                return_List = ReadMembers( _
                        source, TypeAnalyser.GetInstance(source.GetType) _
                            .ExecuteQuery(query) _
                        )

            End If

            Return return_List

        End Function

        ''' <summary>
        ''' Create a Safe Member Name.
        ''' </summary>
        ''' <param name="name"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function SafeMemberName( _
            ByVal name As String _
        ) As String

            Dim aryReplacedStrings As String() = New String() {HYPHEN.ToString, SPACE.ToString}

            For i As Integer = 0 To aryReplacedStrings.Length - 1

                name = name.Replace(aryReplacedStrings(i), String.Empty)

            Next

            Return name.Trim()

        End Function

#End Region

    End Class

End Namespace
