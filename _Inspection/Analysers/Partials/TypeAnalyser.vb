Imports Leviathan.Caching
Imports Leviathan.Commands.StringCommands
Imports Leviathan.Comparison.Comparer
Imports Leviathan.Collections
Imports Leviathan.Resources

Namespace Inspection

	Partial Public Class TypeAnalyser

		#Region " Public Constants "

			Public Const FORMAT_FIELD_REGEX As String = "(?<=\{)[A-z0-9][A-z]+[A-z0-9.]*(?=\})"

			Public Const FORMAT_FIELD_START As String = BRACE_START

			Public Const FORMAT_FIELD_END As String = BRACE_END

		#End Region

		#Region " Public Methods "

			''' <summary>
			''' This method will check whether the Type implements an Interface.
			''' </summary>
			''' <param name="interfaceType">The Interface Type to Check.</param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Function ImplementsInterface( _
				ByVal interfaceType As System.Type _
			) As Boolean

				Return TypeAnalyser.DoesTypeImplementInterface(Type, interfaceType)

			End Function

			''' <summary>
			''' This method will query for a single (first/only) Custom Attribute of the Type Supplied on the Analysed Type.
			''' </summary>
			''' <param name="attributeType">The Type of Attribute to Query for.</param>
			''' <returns>A Single Attribute Analyser or Nothing.</returns>
			''' <remarks></remarks>
			Public Function GetAttribute( _
				ByVal attributeType As System.Type _
			) As AttributeAnalyser

				Return GetSingleObject( _
					ExecuteQuery( _
						New AnalyserQuery() _
							.SetReturnType(AnalyserType.AttributeAnalyser) _
							.SetReturnTypeIsOrInheritedFromType(attributeType) _
							.SetNumberOfResults(1) _
						) _
					)

			End Function

			''' <summary>
			''' This method will query for a single (first/only) Member on the Analysed Type.
			''' </summary>
			''' <param name="name">The Name of the Member.</param>
			''' <returns>A Member Analyser or Nothing.</returns>
			''' <remarks></remarks>
			Public Function GetMember( _
				ByVal name As String _
			) As MemberAnalyser

				Return GetSingleObject( _
					ExecuteQuery( _
						New AnalyserQuery() _
							.SetReturnType(AnalyserType.MemberAnalyser) _
							.SetName(name) _
							.SetVisibility(MemberVisibility.All) _
							.SetLocation(MemberLocation.All) _
							.SetAccessibility(MemberAccessibility.All) _
							.SetNumberOfResults(1) _
						) _
					)

			End Function

			''' <summary>
			''' This method will get a the name of a single member (if only one exists) that
			''' returned the supplied type.
			''' </summary>
			''' <param name="returningType">The Supplied Type the Member should return.</param>
			''' <param name="checkChildren">Whether Children of the Current Type should be checked.</param>
			''' <param name="checkedTypes">Hashtable used for recursive calling to log checked types.</param>
			''' <returns>The analyser for the Member, or nothing.</returns>
			''' <remarks></remarks>
			Public Function GetSingleMember( _
				ByVal returningType As Type, _
				Optional ByVal checkChildren As Boolean = True, _
				Optional ByRef checkedTypes As Hashtable = Nothing _
			) As MemberAnalyser

				Dim cache As Simple = Simple.GetInstance(GetType(TypeAnalyser).GetHashCode)

				Dim cachedObject As MemberAnalyser = Nothing

				If Not cache.TryGet(cachedObject, METHOD_GETSINGLEMEMBER.GetHashCode, Type.GetHashCode, returningType.GetHashCode) Then

					cachedObject = GetSingleObject( _
						ExecuteQuery( _
							New AnalyserQuery() _
								.SetReturnType(AnalyserType.MemberAnalyser) _
								.SetReturnTypeIsOrInheritedFromType(returningType) _
								.SetVisibility(MemberVisibility.Public) _
								.SetLocation(MemberLocation.All) _
								.SetArgumentCount(0) _
								.SetNumberOfResults(1) _
							) _
						)

					If cachedObject Is Nothing Then

						Dim members As MemberAnalyser() = ExecuteQuery( _
							New AnalyserQuery() _
								.SetReturnType(AnalyserType.MemberAnalyser) _
								.SetReturnTypeIsOrInheritedFromType(returningType) _
								.SetVisibility(MemberVisibility.Public) _
								.SetArgumentCount(0) _
							)

						If Not members Is Nothing Then

							Dim lstMembers As New ArrayList
							If checkedTypes Is Nothing Then checkedTypes = New Hashtable

							For i As Integer = 0 To members.Length - 1

								Dim memberType As Type = members(i).ReturnType

								If Not checkedTypes.ContainsKey(memberType) Then

									checkedTypes.Add(memberType, Nothing)

									Dim childAnalyser As TypeAnalyser = TypeAnalyser.GetInstance(memberType)

									Dim childMember As MemberAnalyser = childAnalyser.GetSingleMember(returningType, True, checkedTypes)

									If Not childMember Is Nothing Then
										members(i).Child = childMember
										lstMembers.Add(members(i))
									End If

								End If

							Next

							If lstMembers.Count = 1 Then cachedObject = lstMembers(0)

						End If

					End If

					If Not cachedObject Is Nothing Then _
						cache.Set(cachedObject, METHOD_GETSINGLEMEMBER.GetHashCode, Type.GetHashCode, returningType.GetHashCode)

				End If

				Return cachedObject

			End Function

			''' <summary>
			''' This method will query for a single (first/only) Method on the Analysed Type.
			''' </summary>
			''' <param name="name">The Name of the Method.</param>
			''' <returns>A Method Analyser or Nothing.</returns>
			''' <remarks></remarks>
			Public Function GetMethod( _
				ByVal name As String _
			) As MemberAnalyser

				Return GetSingleObject( _
					ExecuteQuery( _
						New AnalyserQuery() _
							.SetReturnType(AnalyserType.MethodAnalyser) _
							.SetName(name) _
							.SetVisibility(MemberVisibility.All) _
							.SetLocation(MemberLocation.All) _
							.SetAccessibility(MemberAccessibility.All) _
							.SetNumberOfResults(1) _
						) _
					)

			End Function

			''' <summary>
			''' Method to Generate String Representation of a Particular Type.
			''' </summary>
			''' <returns></returns>
			''' <remarks>This provides a default ToString representation of this object.
			''' If it's a generic type it is the Full Name of the Type being analysed.</remarks>
			Public Overloads Function ToString() As String

				Return FullName

			End Function

			Public Function GetElementType()

				If IsArray Then

					Return Type.GetElementType

				ElseIf IsGeneric Then

					Return Type.GetGenericArguments(0)

				ElseIf IsIList Then

					Dim attr As AttributeAnalyser = GetAttribute(GetType(ElementTypeAttribute))

					If Not attr Is Nothing Then _
						Return CType(attr.Attribute, ElementTypeAttribute).Type

					attr = GetAttribute(GetType(DefaultMemberAttribute))

					If Not attr Is Nothing Then

						Dim members As MemberAnalyser() = _
							ExecuteQuery( _
								AnalyserQuery.QUERY_MEMBERS_READABLE _
									.SetName(CType(attr.Attribute, DefaultMemberAttribute).MemberName) _
								)

						If Not members Is Nothing AndAlso members.Length = 1 Then Return members(0).ReturnType

					Else

						Dim members As MemberInfo() = Type.GetDefaultMembers()

						If Not members Is Nothing AndAlso members.Length >= 1 Then _
							Return TypeAnalyser.GetMemberReturnType(members(0))

					End If

				Else

					Return Type

				End If

				Return Nothing

			End Function

			Public Function ExecuteQuery( _
				ByVal query As AnalyserQuery _
			) As Array

				Dim cache As Simple = Simple.GetInstance(GetType(TypeAnalyser).GetHashCode)

				Dim cachedList As Object() = Nothing

				If Not cache.TryGet(cachedList, METHOD_EXECUTEQUERY.GetHashCode, Type.GetHashCode, query.GetHashCode) Then

					Dim currentType As System.Type = Type

					'''''''''''''''''''''''''''''''''''''''''
					' Do the Initial Query                  '
					'''''''''''''''''''''''''''''''''''''''''
					Dim memberIList As New ArrayList

					Do Until (currentType Is Nothing) OrElse (currentType Is query.DeclaredBelowType)

						Select Case query.ReturnType

							Case AnalyserType.AssemblyAnalyser

								Throw New NotSupportedException

							Case AnalyserType.AttributeAnalyser

								memberIList.AddRange(currentType.GetCustomAttributes(True))

							Case AnalyserType.ConstructorAnalyser

								memberIList.AddRange(currentType.GetConstructors(query.BindingFlags))

							Case AnalyserType.ExceptionAnalyser

								Throw New NotSupportedException

							Case AnalyserType.FolderAnalyser

								Throw New NotSupportedException

							Case AnalyserType.MemberAnalyser

								Dim flags As BindingFlags = query.BindingFlags
								memberIList.AddRange(currentType.GetFields(flags))
								memberIList.AddRange(currentType.GetProperties(flags))

							Case AnalyserType.MethodAnalyser

								memberIList.AddRange(currentType.GetMethods(query.BindingFlags))

							Case AnalyserType.TypeAnalyser

								memberIList.AddRange(currentType.GetNestedTypes(query.BindingFlags))

						End Select

						currentType = currentType.BaseType

					Loop
					'''''''''''''''''''''''''''''''''''''''''

					cachedList = query.GetPostQueryReturnArray(memberIList)

					cache.Set(cachedList, METHOD_EXECUTEQUERY.GetHashCode, Type.GetHashCode, query.GetHashCode)

				End If

				If Not cachedList Is Nothing AndAlso cachedList.Length > 0 Then

					Return cachedList

				Else

					Return Nothing

				End If

			End Function

			Public Function GetDefaultConstructor() As MemberAnalyser

				Dim constructor As Reflection.ConstructorInfo = Type.GetConstructor(System.Type.EmptyTypes)

				If Not constructor Is Nothing Then

					Return New MemberAnalyser(constructor.Name, Nothing, Nothing, Nothing, Nothing, Nothing, constructor)

				Else

					Return GetSingleObject( _
						ExecuteQuery( _
							New AnalyserQuery() _
								.SetReturnType(AnalyserType.ConstructorAnalyser) _
								.SetVisibility(MemberVisibility.All) _
								.SetArgumentCount(0) _
								.SetNumberOfResults(1) _
							) _
						)

				End If

			End Function

			Public Function GetDisplayName() As String

				Dim attr As AttributeAnalyser = GetAttribute(GetType(NameAttribute))

				If Not attr Is Nothing Then
					Return CType(attr.Attribute, NameAttribute).Name
				Else
					Return Type.Name
				End If

			End Function

			Public Function Create() As Object

				Try

					If IsArray Then

						Return Array.CreateInstance(ElementType, 0)

					Else

						Return TypeAnalyser.Create(Type)

					End If

				Catch
				End Try

				Return Nothing

			End Function

		#End Region

		#Region " Public Shared Methods "

			''' <summary>
			''' This method will get the return type of a particular member (Property/Field/Method).
			''' </summary>
			''' <param name="member"></param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function GetMemberReturnType( _
				ByVal member As MemberInfo _
			) As Type

				If member.MemberType = MemberTypes.[Property] Then

					Return CType(member, PropertyInfo).PropertyType

				ElseIf member.MemberType = MemberTypes.Field Then

					Return CType(member, FieldInfo).FieldType

				ElseIf member.MemberType = MemberTypes.Method Then

					Return CType(member, MethodInfo).ReturnType

				ElseIf member.MemberType = MemberTypes.Constructor Then

					Return CType(member, MethodInfo).DeclaringType

				End If

				Return Nothing

			End Function

			''' <summary>
			''' This method will get the arguments types of a particular member (Property/Method).
			''' </summary>
			''' <param name="member"></param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function GetMemberArgumentSignature( _
				ByVal member As MemberInfo _
			) As Type()

				Dim indexParameters As ParameterInfo() = Nothing

				If member.MemberType = MemberTypes.[Property] Then

					indexParameters = CType(member, PropertyInfo).GetIndexParameters

				ElseIf member.MemberType = MemberTypes.Method Then

					indexParameters = CType(member, MethodInfo).GetParameters

				End If

				If Not indexParameters Is Nothing AndAlso indexParameters.Length > 0 Then

					Dim types(indexParameters.Length - 1) As Type

					For i As Integer = 0 To indexParameters.Length - 1
						types(i) = indexParameters(i).ParameterType
					Next

					Return types

				End If

				Return Nothing

			End Function

			''' <summary>
			''' Method to Check whether a Type implements a specific Interface Type.
			''' </summary>
			''' <param name="type">The Type to Test.</param>
			''' <returns>A Boolean Value indicating whether the Interface is, or is implemented by the Type.</returns>
			''' <remarks></remarks>
			Public Shared Function DoesTypeImplementInterface( _
				ByVal type As System.Type, _
				ByVal interfaceType As System.Type _
			) As Boolean

				If type.IsInterface AndAlso type Is interfaceType Then

					Return True

				Else

					Dim interfaces As Type() = type.GetInterfaces()
					If Not interfaces Is Nothing AndAlso interfaces.Length > 0 Then

						For i As Integer = 0 To interfaces.Length - 1

							If interfaces(i) Is interfaceType Then Return True

						Next

					End If

					Return False

				End If

			End Function

			''' <summary>
			''' Method to Check whether a Type is a Simple Type.
			''' </summary>
			''' <param name="type">The Type to Test.</param>
			''' <returns>A Boolean Value indicating whether the Type is a Simple Type.</returns>
			''' <remarks></remarks>
			Public Shared Function IsTypeSimpleType( _
				ByVal type As System.Type _
			) As Boolean

				If type Is GetType(System.String) OrElse _
					type Is GetType(System.Type) OrElse _
					type Is GetType(System.Int16) OrElse _
					type Is GetType(System.Int32) OrElse _
					type Is GetType(System.Int64) OrElse _
					type Is GetType(Short) OrElse _
					type Is GetType(Integer) OrElse _
					type Is GetType(Long) OrElse _
					type Is GetType(Single) OrElse _
					type Is GetType(Double) OrElse _
					type Is GetType(Type) OrElse _
					type Is GetType(DateTime) OrElse _
					type Is GetType(System.Void) OrElse _
					type.IsEnum Then

					Return True

				Else

					Return False

				End If

			End Function

			''' <summary>
			''' Method to Check whether a Type is a Numeric Type.
			''' </summary>
			''' <param name="type">The Type to Test.</param>
			''' <returns>A Boolean Value indicating whether the Type is a Numeric Type.</returns>
			''' <remarks></remarks>
			Public Shared Function IsTypeNumericType( _
				ByVal type As System.Type _
			) As Boolean

				If type Is GetType(System.Int16) OrElse _
					type Is GetType(System.Int32) OrElse _
					type Is GetType(System.Int64) OrElse _
					type Is GetType(Short) OrElse _
					type Is GetType(Integer) OrElse _
					type Is GetType(Long) OrElse _
					type Is GetType(Single) OrElse _
					type Is GetType(Double) OrElse _
					type Is GetType(Decimal) Then

					Return True

				Else

					Return False

				End If

			End Function

			''' <summary>
			''' This method is used to return the Names of all the Properties supplied.
			''' </summary>
			''' <param name="members">The Members for which the names are returned.</param>
			''' <returns>An array of <seealso cref="String"/> objects or Nothing.</returns>
			''' <remarks></remarks>
			Public Shared Function GetMemberNames( _
				ByVal members As MemberInfo() _
			) As String()

				If Not members Is Nothing Then

					Dim memberNames(members.Length - 1) As String

					For i As Integer = 0 To members.Length - 1

						memberNames(i) = members(i).Name

					Next

					Return memberNames

				End If

				Return Nothing

			End Function

			''' <summary>
			''' This method is used to check whether one class is a sub-type of the second class.
			''' It uses type name and assembly name matching.
			''' </summary>
			''' <param name="ty1">The type (inheritor) to check.</param>
			''' <param name="ty2">The parent type (inherited) to check for.</param>
			''' <returns>A boolean value indicating whether the first type is a sub-type of the second type.</returns>
			''' <remarks>This method searches right up the inheritance heirarchy, so will return True for
			''' parents and grandparents.</remarks>
			Public Shared Function IsSubClassOf( _
				ByVal ty1 As System.Type, _
				ByVal ty2 As System.Type _
			) As Boolean

				Dim tempType As System.Type = ty1

				Do Until tempType Is Nothing

					If tempType Is ty2 OrElse _
						(tempType.FullName = ty2.FullName AndAlso _
						tempType.AssemblyQualifiedName = ty2.AssemblyQualifiedName) Then

						Return True

					Else

						tempType = tempType.BaseType

					End If

				Loop

				Return False

			End Function

			''' <summary>
			''' This method is used to check whether one class can be accepted as another type (for method arguments etc).
			''' </summary>
			''' <param name="typeToAccept">The type proposed for acceptance.</param>
			''' <param name="expectedType">The expected type to check for.</param>
			''' <returns>A boolean value indicating whether the first type can be used in place of the expectedType.</returns>
			''' <remarks></remarks>
			Public Shared Function CanAccept( _
				ByVal typeToAccept As System.Type, _
				ByVal expectedType As System.Type _
			) As Boolean

				If expectedType Is GetType(String) Then

					Return True

				ElseIf typeToAccept Is expectedType Then

					Return True

				ElseIf IsSubClassOf(typeToAccept, expectedType) Then

					Return True

				ElseIf expectedType.IsArray AndAlso typeToAccept.IsArray Then

					Return IsSubClassOf(typeToAccept.GetElementType, expectedType.GetElementType)

				ElseIf expectedType.IsInterface Then

					Return DoesTypeImplementInterface(typeToAccept, expectedType)

				End If

				Return False

			End Function

			''' <summary>
			''' This method is used to get the most basic type from an array of Types.
			''' </summary>
			''' <param name="types">The Types to Get The Basic Type for.</param>
			''' <returns>The most basic (e.g. Common) Type.</returns>
			''' <remarks></remarks>
			Public Shared Function GetBasicType( _
				ByVal types As System.Type() _
			) As Type

				' Set the Return Type to the Default (e.g. most basic).
				Dim returnType As Type = GetType(Object)

				If Not types Is Nothing Then

					' Iterate the Types
					For i As Integer = 0 To types.Length - 1

						' If the current type is equal to the current return type, just ignore.
						If Not types(i) Is returnType Then

							If returnType Is GetType(Object) Then

								' If the current return type is the default, then set it the the current type.
								returnType = types(i)

							Else

								' We could have an issue here, as the current type is not descending from
								' the return type, so we need to walk the return types heirachy to find
								' a common ancestor.
								Do Until returnType Is GetType(Object) OrElse TypeAnalyser.IsSubClassOf(types(i), returnType)

									returnType = returnType.BaseType

								Loop

							End If

						End If

					Next

				End If

				Return returnType

			End Function

			''' <summary>
			''' Method to Generate String Representation of a Particular Exception.
			''' </summary>
			''' <returns></returns>
			''' <remarks>This method will check for the presence of a <see cref="DescriptionAttribute"/>
			''' attribute on the exception and use this if found. It will also output the Message and InnerException Message
			''' if present.</remarks>
			Public Shared Function ExceptionToString( _
				ByVal exception As Exception, _
				Optional ByVal sourceMethod As MethodBase = Nothing _
			) As String

				Dim analyser As TypeAnalyser = TypeAnalyser.GetInstance(exception.GetType)

				Dim attrDesc As AttributeAnalyser = analyser.GetAttribute(GetType(DescriptionAttribute))

				Dim desc As DescriptionAttribute = Nothing

				If Not attrDesc Is Nothing Then desc = attrDesc.Attribute

				Dim sb As New System.Text.StringBuilder

				Dim exceptionName As String

				If Not desc Is Nothing Then
					exceptionName = desc.Description
				Else
					exceptionName = analyser.FullName
				End If

				If Not sourceMethod Is Nothing Then

					sb.AppendFormat( _
						SingleResource( _
							GetType(TypeAnalyser).Assembly, _
							RESOURCEMANAGER_NAME_LOG, _
							"generalExceptionWithMethodAndType"), _
							exceptionName, sourceMethod.Name, sourceMethod.ReflectedType.ToString)

				Else

					sb.AppendFormat( _
						SingleResource( _
							GetType(TypeAnalyser).Assembly, _
							RESOURCEMANAGER_NAME_LOG, _
							"generalException"), _
							exceptionName)

				End If

				sb.AppendLine()

				If Not String.IsNullOrEmpty(exception.Message) Then
					sb.Append(COLON)
					sb.Append(SPACE)
					sb.Append(exception.Message)
				ElseIf Not String.IsNullOrEmpty(exception.ToString) Then
					sb.Append(COLON)
					sb.Append(SPACE)
					sb.Append(exception.ToString)
				End If

				Dim propertiesToDisplay As MemberAnalyser() = analyser.ExecuteQuery( _
					New AnalyserQuery() _
						.SetReturnType(AnalyserType.MemberAnalyser) _
						.SetVisibility(MemberVisibility.All) _
						.SetAccessibility(MemberAccessibility.All) _
						.SetPresentAttribute(GetType(DescriptionAttribute)))

				If Not propertiesToDisplay Is Nothing AndAlso propertiesToDisplay.Length > 0 Then

					sb.AppendLine()

					For i As Integer = 0 To propertiesToDisplay.Length - 1

						Dim prop_desc As DescriptionAttribute() = _
							propertiesToDisplay(i).Member.GetCustomAttributes(GetType(DescriptionAttribute), True)

						For j As Integer = 0 To prop_desc.Length - 1

							sb.AppendFormat( _
								SingleResource( _
									GetType(TypeAnalyser).Assembly, _
									RESOURCEMANAGER_NAME_LOG, _
									"generalExceptionProperties"), _
									prop_desc(j).Description, _
									propertiesToDisplay(i).Read(exception).ToString)

							If Not i = propertiesToDisplay.Length - 1 AndAlso Not j = prop_desc.Length - 1 Then sb.AppendLine()

						Next

					Next

				End If

				If Not exception.InnerException Is Nothing Then

					sb.AppendLine()

					sb.AppendFormat( _
						SingleResource( _
							GetType(TypeAnalyser).Assembly, _
							RESOURCEMANAGER_NAME_LOG, _
							"generalInnerException"), _
							ExceptionToString(exception.InnerException))

				End If

				Return sb.ToString

			End Function

			''' <summary>
			''' Method to Get an Element Type from an ICollection.
			''' </summary>
			''' <param name="sourceCollection">The ICollection to get the Element Type for (can be empty).</param>
			''' <returns>The Type of the Elements.</returns>
			''' <remarks>This method will check elements to find a common ancestor type.</remarks>
			Public Shared Function GetElementType( _
				ByVal sourceCollection As ICollection _
			) As Type

				Dim elementType As Type = GetType(Object)

				If Not sourceCollection Is Nothing Then

					Dim aryTypes As New ArrayList

					For Each singleObject As Object In sourceCollection

						If Not singleObject Is Nothing Then

							Dim singleElementType As Type = singleObject.GetType

							If Not aryTypes.Contains(singleElementType) Then aryTypes.Add(singleElementType)

						End If

					Next

					elementType = TypeAnalyser.GetBasicType(aryTypes.ToArray(GetType(Type)))

				End If

				Return elementType

			End Function

			''' <summary>
			''' Method to Add an Element to an ICollection.
			''' </summary>
			''' <param name="targetCollection">The ICollection to Add the Element to.</param>
			''' <param name="value">The Value to Add.</param>
			''' <remarks></remarks>
			Public Shared Sub AddElement( _
				ByRef targetCollection As ICollection, _
				ByVal value As Object _
			)

				If Not targetCollection Is Nothing Then

					Dim analyser As TypeAnalyser = _
						TypeAnalyser.GetInstance(targetCollection.GetType)

					If analyser.IsArray Then

						AddTo(CType(targetCollection, Array), value)

					ElseIf analyser.IsIList Then

						CType(targetCollection, IList).Add(value)

					End If

				End If

			End Sub

			''' <summary>
			''' This method will create a new instance of the target object.
			''' If no <paramref name="constructorArgs"/> are supplied, it will attempt to call the
			''' default parameter-less constructor if available.
			''' If <paramref name="constructorArgs"/> are supplied, it will attempt to match the number
			''' and type of these args to a constructor and call it.
			''' </summary>
			''' <param name="constructorArgs">The Arguments to supply to the constructor method.</param>
			''' <returns>This Object Manipulator</returns>
			''' <remarks>The Target Object Property of this class will contain the new object. If there is currently an object held there,
			''' the reference to this will be overwritten.</remarks>
			Public Shared Function Create( _
				ByVal objectType As Type, _
				ByVal ParamArray constructorArgs As DictionaryEntry() _
			) As Object

				Dim l_Analyser As TypeAnalyser = TypeAnalyser.GetInstance(objectType)

				If (constructorArgs Is Nothing OrElse constructorArgs.Length = 0) AndAlso _
					l_Analyser.HasDefaultConstructor Then

					Return Activator.CreateInstance(objectType, True)

				Else

					Dim constructors As MemberAnalyser() = _
						l_Analyser.ExecuteQuery( _
							New AnalyserQuery() _
								.SetReturnType(AnalyserType.ConstructorAnalyser) _
								.SetVisibility(MemberVisibility.Public) _
								.SetLocation(MemberLocation.All))

					For Each constructor As MemberAnalyser In constructors

						If Not constructor.Constructor.GetParameters Is Nothing AndAlso constructor.Constructor.GetParameters.Length = constructorArgs.Length Then

							Dim successfullMatches As Integer = constructorArgs.Length

							Dim aryConstructorArgs(constructor.Constructor.GetParameters.Length - 1) As Object

							For i As Integer = 0 To constructorArgs.Length - 1

								For j As Integer = 0 To constructor.Constructor.GetParameters.Length - 1

									If String.Compare(constructor.Constructor.GetParameters(j).Name, constructorArgs(i).Key.ToLower, True) = 0 Then

										aryConstructorArgs(j) = constructorArgs(i).Value
										successfullMatches -= 1
										Exit For

									End If

								Next

							Next

							If successfullMatches = 0 Then Return Activator.CreateInstance(objectType, aryConstructorArgs)

						End If

					Next

				End If

				Return Nothing

			End Function

			''' <summary>
			''' This method will 'wedge' the current Target object into the supplied type if possible.
			''' </summary>
			''' <param name="intoType">The Type to 'wedge' into.</param>
			''' <param name="successfulWedge">A ByRef/Out parameter indicating whether the wedge was successful.</param>
			''' <returns>The results of the wedge, which could be nothing, an object, an array, collection or list of objects.</returns>
			''' <remarks>The wedge function works by calling method in the analyser
			''' of the current object. If this is successful, it queries for the object/s contained by this property and returns them.
			''' The <paramref name="successfulWedge" /> parameter is used to indicate whether the wedge was successful as a Null/Nothing returned
			''' may indicate a failed wedge, or a Nothing/Null value which is the result of a legitimate wedge and should be used.</remarks>
			Public Shared Function Wedge( _
				ByVal value As Object, _
				ByVal intoType As System.Type, _
				ByRef successfulWedge As Boolean _
			) As Object

				If intoType Is Nothing Then

					successfulWedge = False
					Return Nothing

				ElseIf value Is Nothing Then

					successfulWedge = True
					Return Nothing

				Else

					Dim valueTypeAnalyser As TypeAnalyser = _
						TypeAnalyser.GetInstance(value.GetType)

					If TypeAnalyser.CanAccept(valueTypeAnalyser.Type, intoType) Then

						successfulWedge = True
						Return value

					ElseIf (valueTypeAnalyser.IsICollection AndAlso _
						TypeAnalyser.CanAccept(valueTypeAnalyser.ElementType, intoType)) Then

						If valueTypeAnalyser.IsArray AndAlso CType(value, Array).Length = 1 Then

							value = CType(value, Array)(0)

						ElseIf valueTypeAnalyser.IsIList AndAlso CType(value, IList).Count = 1 Then

							value = CType(value, IList)(0)

						End If

						successfulWedge = True
						Return value

					ElseIf (valueTypeAnalyser.IsIList AndAlso CType(value, IList).Count > 0 AndAlso _
						TypeAnalyser.CanAccept(value(0).GetType, intoType)) Then

						If CType(value, IList).Count = 1 Then value = CType(value, IList)(0)

						successfulWedge = True
						Return value

					Else

						Dim intoTypeAnalyser As TypeAnalyser = TypeAnalyser.GetInstance(intoType)

						If intoType.IsPrimitive OrElse intoTypeAnalyser.IsSimple Then

							Try

								Dim result As Object

								If intoType.IsEnum Then

									result = [Enum].Parse(intoType, value.ToString, True)

								ElseIf intoType Is GetType(Boolean) Then

									If valueTypeAnalyser.Type Is GetType(Int16) Then

										result = (CType(value, Int16) = 1)

									ElseIf valueTypeAnalyser.Type Is GetType(Int32) Then

										result = (CType(value, Int32) = 1)

									ElseIf valueTypeAnalyser.Type Is GetType(Int64) Then

										result = (CType(value, Int64) = 1)

									ElseIf valueTypeAnalyser.Type Is GetType(Single) Then

										result = (CType(value, Single) = 1)

									ElseIf valueTypeAnalyser.Type Is GetType(Double) Then

										result = (CType(value, Double) = 1)

									ElseIf valueTypeAnalyser.Type Is GetType(Long) Then

										result = (CType(value, Long) = 1)

									ElseIf valueTypeAnalyser.Type Is GetType(Byte) Then

										result = (CType(value, Byte) = 1)

									Else

										result = Boolean.Parse(value.ToString)

									End If

								Else

									result = Convert.ChangeType(value, intoType)

								End If

								successfulWedge = True
								Return result

							Catch ex As Exception

								successfulWedge = False
								Return Nothing

							End Try

						Else

							Dim wedgeMember As MemberAnalyser = valueTypeAnalyser.GetSingleMember(intoType)

							If wedgeMember Is Nothing Then

								successfulWedge = False
								Return Nothing

							Else

								successfulWedge = True
								Return wedgeMember.Read(value)

							End If

						End If

					End If

				End If

			End Function

		#End Region

		#Region " Array Methods "

			''' <summary>
			''' Method to Add an Object into the Array at the End.
			''' </summary>
			''' <param name="ary">The Array into which to Add.</param>
			''' <param name="itemToAdd">The Item to Add.</param>
			''' <returns>An Array with the new Item Added.</returns>
			''' <remarks>If the Array is Nothing then it will be created first.</remarks>
			Public Shared Function AddTo( _
				ByVal ary As Array, _
				ByVal itemToAdd As Object _
			) As Array

				If Not itemToAdd Is Nothing Then

					If ary Is Nothing Then

						ary = Array.CreateInstance(itemToAdd.GetType, 1)
						ary(0) = itemToAdd

					Else

						Dim newAry As Array = Array.CreateInstance(ary.GetType.GetElementType, ary.Length + 1)

						ary.CopyTo(newAry, 0)

						newAry(newAry.Length - 1) = itemToAdd

						ary = newAry

					End If

				End If

				Return ary

			End Function

			''' <summary>
			''' Method to Insert an Object into the Array at the specified Index.
			''' </summary>
			''' <param name="ary">The Array into which to Add.</param>
			''' <param name="itemToInsert">The Item to Insert.</param>
			''' <param name="index">The Index at which to insert.</param>
			''' <returns>An Array with the new Item inserted.</returns>
			''' <remarks></remarks>
			Public Shared Function InsertAt( _
				ByVal ary As Array, _
				ByVal itemToInsert As Object, _
				ByVal index As Integer _
			) As Array

				If Not ary Is Nothing AndAlso Not itemToInsert Is Nothing Then

					Dim insertAry As Array = Array.CreateInstance(itemToInsert.GetType, 1)
					insertAry(0) = itemToInsert

					Return InsertAt(ary, insertAry, index)

				Else

					Return ary

				End If

			End Function

			''' <summary>
			''' Method to Insert Objects into the Array at the specified Index.
			''' </summary>
			''' <param name="ary">The Array into which to Add.</param>
			''' <param name="itemsToInsert">The Items to Insert.</param>
			''' <param name="index">The Index at which to insert.</param>
			''' <returns>An Array with the new Items inserted.</returns>
			''' <remarks></remarks>
			Public Shared Function InsertAt( _
				ByVal ary As Array, _
				ByVal itemsToInsert As ICollection, _
				ByVal index As Integer _
			) As Array

				If Not ary Is Nothing AndAlso Not itemsToInsert Is Nothing Then

					If index > ary.Length - 1 Then index = ary.Length - 1

					Dim lstArray As New ArrayList(ary)

					lstArray.InsertRange(index, itemsToInsert)

					Return lstArray.ToArray(ary.GetType.GetElementType)

				Else

					Return ary

				End If

			End Function

			''' <summary>
			''' Method to Remove a Single Element from an Array.
			''' </summary>
			''' <param name="ary"></param>
			''' <param name="index"></param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function RemoveAt( _
				ByRef ary As Array, _
				ByVal index As Integer _
			) As Array

				Dim lst As New ArrayList(ary)
				lst.RemoveAt(index)
				Return lst.ToArray(ary.GetType.GetElementType)

			End Function

			''' <summary>
			''' Method to Action a Paricular Change in an Array.
			''' </summary>
			''' <param name="ary">The Array to Change.</param>
			''' <param name="action">The Action to Undertake.</param>
			''' <param name="value">The Value to Action.</param>
			''' <returns>The Changed Array.</returns>
			''' <remarks></remarks>
			Public Shared Function ActionFromArray( _
				ByVal ary As Array, _
				ByVal action As CollectionAction, _
				ByVal value As Object _
			) As Array

				Dim lstArray As New ArrayList(ary)

				Select Case action

					Case CollectionAction.AddObject

						lstArray.Add(value)

					Case CollectionAction.AddObjects

						lstArray.AddRange(value)

					Case CollectionAction.RemoveObject

						lstArray.Remove(value)

					Case CollectionAction.RemoveObjects

						For Each singleValue As Object In CType(value, ICollection)
							lstArray.Remove(singleValue)
						Next

					Case CollectionAction.RemoveIndex

						lstArray.RemoveAt(value)

				End Select

				Return lstArray.ToArray(ary.GetType.GetElementType)

			End Function

			''' <summary>
			''' Method to Remove a Defined Number of Elements from the start of an Array.
			''' </summary>
			''' <param name="sourceArray">The Array to be Removed From.</param>
			''' <param name="numberOfElementsToRemove">The Number of Elements to Remove.</param>
			''' <returns>The Trimed Array or Nothing if all Elements are removed.</returns>
			''' <remarks></remarks>
			Public Shared Function RemoveStartElements( _
				ByVal sourceArray As Array, _
				ByVal numberOfElementsToRemove As Integer _
			) As Array

				If sourceArray.Length > numberOfElementsToRemove Then

					Dim newArray As Array = Array.CreateInstance(sourceArray.GetType.GetElementType, _
						(sourceArray.Length - numberOfElementsToRemove))

					For i As Integer = numberOfElementsToRemove To sourceArray.Length - 1

						newArray(i - numberOfElementsToRemove) = sourceArray(i)

					Next

					Return newArray

				Else

					Return Array.CreateInstance(sourceArray.GetType.GetElementType, 0)

				End If

			End Function

			''' <summary>
			''' Method to Remove a Defined Number of Elements from the end of an Array.
			''' </summary>
			''' <param name="sourceArray">The Array to be Removed From.</param>
			''' <param name="numberOfElementsToRemove">The Number of Elements to Remove.</param>
			''' <returns>The Trimed Array or an Empty Array if all Elements are removed.</returns>
			''' <remarks></remarks>
			Public Shared Function RemoveEndElements( _
				ByVal sourceArray As Array, _
				ByVal numberOfElementsToRemove As Integer _
			) As Array

				If sourceArray.Length > numberOfElementsToRemove Then

					Dim newArray As Array = Array.CreateInstance(sourceArray.GetType.GetElementType, _
						(sourceArray.Length - numberOfElementsToRemove))

					For i As Integer = 0 To sourceArray.Length - (numberOfElementsToRemove + 1)

						newArray(i) = sourceArray(i)

					Next

					Return newArray

				Else

					Return Array.CreateInstance(sourceArray.GetType.GetElementType, 0)

				End If

			End Function

			''' <summary>
			''' Method to Remove the Contents of One IList from Another.
			''' </summary>
			''' <param name="itemsToRemove">The Array containing the elements to remove.</param>
			''' <param name="itemsToProcess">The Array to remove from.</param>
			''' <remarks></remarks>
			Public Shared Sub ReconcileArraysBySubtraction( _
				ByRef itemsToRemove As Array, _
				ByRef itemsToProcess As Array _
			)

				Dim aryReturn As New ArrayList

				For i As Integer = 0 To itemsToProcess.Length - 1

					Dim foundMatch As Boolean = False
					For j As Integer = 0 To itemsToRemove.Length - 1

						If Not itemsToProcess(i) Is Nothing Then

							If itemsToProcess(i).Equals(itemsToRemove(j)) Then
								foundMatch = True
								Exit For
							End If

						End If

					Next

					If Not foundMatch Then aryReturn.Add(itemsToProcess(i))

				Next

				itemsToProcess = aryReturn.ToArray(itemsToRemove.GetType.GetElementType)

			End Sub

			''' <summary>
			''' Method to Add the Contents of One IList from Another (without duplicates).
			''' </summary>
			''' <param name="itemsToAdd">The Array containing the elements to add.</param>
			''' <param name="itemsToProcess">The Array to add to.</param>
			''' <remarks></remarks>
			Public Shared Sub ReconcileArraysByAddition( _
				ByRef itemsToAdd As Array, _
				ByRef itemsToProcess As Array _
			)

				Dim aryReturn As New ArrayList

				aryReturn.AddRange(itemsToProcess)

				For i As Integer = 0 To itemsToAdd.Length - 1

					Dim foundMatch As Boolean = False

					For j As Integer = 0 To itemsToProcess.Length - 1

						If Not itemsToAdd(i) Is Nothing Then

							If itemsToAdd(i).Equals(itemsToProcess(j)) Then
								foundMatch = True
								Exit For
							End If

						End If

					Next

					If Not foundMatch Then aryReturn.Add(itemsToAdd(i))

				Next

				itemsToProcess = aryReturn.ToArray(itemsToProcess.GetType.GetElementType)

			End Sub

			''' <summary>
			''' Converts an Array to an Ilist.
			''' </summary>
			''' <param name="sourceArray">The Array to Convert.</param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function ConvertToIList( _
				ByVal sourceArray As Array _
			) As IList

				Return New ArrayList(sourceArray)

			End Function

			''' <summary>
			''' Method to Remove Duplicate Elements from an Array.
			''' </summary>
			''' <param name="sourceArray">The Array to be Removed From.</param>
			''' <returns>The Non-Duplicate Array or an Empty Array if all Elements are removed.</returns>
			''' <remarks></remarks>
			Public Shared Function RemoveDuplicates( _
				ByVal sourceArray As Array _
			) As Array

				Dim newArrayList As New ArrayList

				For i As Integer = 0 To sourceArray.Length - 1

					If Not newArrayList.Contains(sourceArray(i)) Then newArrayList.Add(sourceArray(i))

				Next

				Return newArrayList.ToArray(sourceArray.GetType.GetElementType)

			End Function

			''' <summary>
			''' Method to Get a Single Object from a one element Array.
			''' </summary>
			''' <param name="sourceArray">The Array to get the Value From.</param>
			''' <returns>An Object or Nothing.</returns>
			''' <remarks></remarks>
			Public Shared Function GetSingleObject( _
				ByVal sourceArray As Array _
			) As Object

				If Not sourceArray Is Nothing AndAlso sourceArray.Length = 1 Then

					Return sourceArray(0)

				Else

					Return Nothing

				End If

			End Function

			''' <summary>
			''' Method to Get a Single Object from at least a one element Array.
			''' </summary>
			''' <param name="sourceArray">The Array to get the Value From.</param>
			''' <returns>An Object or Nothing.</returns>
			''' <remarks></remarks>
			Public Shared Function GetFirstObject( _
				ByVal sourceArray As Array _
			) As Object

				If Not sourceArray Is Nothing AndAlso sourceArray.Length >= 1 Then

					Return sourceArray(0)

				Else

					Return Nothing

				End If

			End Function

			''' <summary>
			''' Method to Get an Element Type from an Array.
			''' </summary>
			''' <param name="sourceArray">The Array to get the Element Type for (can be empty).</param>
			''' <returns>The Type of the Elements.</returns>
			''' <remarks>This method will favour the declared type, but will check elements if required
			''' to find a common ancestor type.</remarks>
			Public Shared Function GetElementType( _
				ByVal sourceArray As Array _
			) As Type

				Return TypeAnalyser.GetElementType(CType(sourceArray, ICollection))

			End Function

			''' <summary>
			''' This method will return the Source Object/s as an Array.
			''' </summary>
			''' <param name="sourceValue">This can be a single object, an Array, an IList or ICollection.</param>
			''' <returns>An Array or Nothing.</returns>
			''' <remarks></remarks>
			Public Shared Function CreateArray( _
				ByVal sourceValue As Object _
			) As Array

				Dim retArray As Array = Nothing

				If Not sourceValue Is Nothing Then

					Dim sourceAnalyser As TypeAnalyser = TypeAnalyser.GetInstance(sourceValue.GetType)

					If sourceAnalyser.IsArray Then

						retArray = sourceValue

					ElseIf sourceAnalyser.IsIList Then

						retArray = Array.CreateInstance(GetElementType( _
							sourceValue), CType(sourceValue, IList).Count)

						CType(sourceValue, IList).CopyTo(retArray, 0)

					ElseIf sourceAnalyser.IsICollection Then

						retArray = Array.CreateInstance(GetElementType( _
							sourceValue), CType(sourceValue, ICollection).Count)

						CType(sourceValue, ICollection).CopyTo(retArray, 0)

					Else

						retArray = Array.CreateInstance(sourceAnalyser.Type, 1)

						retArray(0) = sourceValue

					End If

				End If

				Return retArray

			End Function

		#End Region

		#Region " List Methods "

			''' <summary>
			''' Method to Action a Paricular Change in an IList.
			''' </summary>
			''' <param name="lst">The IList to Change.</param>
			''' <param name="action">The Action to Undertake.</param>
			''' <param name="value">The Value to Action.</param>
			''' <returns>The Changed IList.</returns>
			''' <remarks></remarks>
			Public Shared Function ActionFromList( _
				ByVal lst As IList, _
				ByVal action As CollectionAction, _
				ByVal value As Object _
			) As IList

				Select Case action

					Case CollectionAction.AddObject

						lst.Add(value)

					Case CollectionAction.AddObjects

						For Each single_value As Object In CType(value, ICollection)
							lst.Add(single_value)
						Next

					Case CollectionAction.RemoveObject

						lst.Remove(value)

					Case CollectionAction.RemoveIndex

						lst.RemoveAt(value)

				End Select

				Return lst

			End Function

			''' <summary>
			''' Method to Remove a Defined Number of Elements from the start of an IList.
			''' </summary>
			''' <param name="sourceList">The IList to be Removed From.</param>
			''' <param name="numberOfElementsToRemove">The Number of Elements to Remove.</param>
			''' <returns>The Trimed IList or an Empty IList if all Elements are removed.</returns>
			''' <remarks></remarks>
			Public Shared Function RemoveStartElements( _
				ByVal sourceList As IList, _
				ByVal numberOfElementsToRemove As Integer _
			) As IList

				If sourceList.Count > numberOfElementsToRemove Then

					For i As Integer = 0 To numberOfElementsToRemove - 1
						sourceList.RemoveAt(0)
					Next

					Return sourceList

				Else

					Return New ArrayList

				End If

			End Function

			''' <summary>
			''' Method to Remove a Defined Number of Elements from the end of an IList.
			''' </summary>
			''' <param name="sourceList">The IList to be Removed From.</param>
			''' <param name="numberOfElementsToRemove">The Number of Elements to Remove.</param>
			''' <returns>The Trimed IList or an Empty IList if all Elements are removed.</returns>
			''' <remarks></remarks>
			Public Shared Function RemoveEndElements( _
				ByVal sourceList As IList, _
				ByVal numberOfElementsToRemove As Integer _
			) As IList

				If sourceList.Count > numberOfElementsToRemove Then

					Dim startIndex As Integer = sourceList.Count - numberOfElementsToRemove
					Dim endIndex As Integer = sourceList.Count - 1

					For i As Integer = startIndex To endIndex
						sourceList.RemoveAt(startIndex)
					Next

					Return sourceList

				Else

					Return New ArrayList

				End If

			End Function

			''' <summary>
			''' Method to Remove the Contents of One IList from Another.
			''' </summary>
			''' <param name="listToRemove">The IList containing the elements to remove.</param>
			''' <param name="listToProcess">The IList to remove from.</param>
			''' <remarks></remarks>
			Public Shared Sub ReconcileLists( _
				ByRef listToRemove As IList, _
				ByRef listToProcess As IList _
			)

				For i As Integer = 0 To listToRemove.Count - 1

					For j As Integer = 0 To listToProcess.Count - 1

						If Not listToProcess(j) Is Nothing Then

							If listToProcess(j).Equals(listToRemove(i)) Then
								listToProcess.RemoveAt(j)
								j -= 1
							End If

						End If

						If j = listToProcess.Count - 1 Then Exit For

					Next

				Next

			End Sub

			''' <summary>
			''' Converts an IList to an Array.
			''' </summary>
			''' <param name="sourceList">The IList to Convert.</param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function ConvertToArray( _
				ByVal sourceList As IList, _
				Optional ByVal arrayType As Type = Nothing _
			) As Array

				If Not sourceList Is Nothing Then

					If arrayType Is Nothing Then arrayType = GetType(Object)

					Dim returnArray As Array = Array.CreateInstance(arrayType, sourceList.Count)

					For i As Integer = 0 To sourceList.Count - 1
						returnArray(i) = sourceList(i)
					Next

					Return returnArray

				End If

				Return Nothing

			End Function

			''' <summary>
			''' Method to Remove Duplicate Elements from an IList.
			''' </summary>
			''' <param name="sourceList">The IList to be Removed From.</param>
			''' <returns>The Non-Duplicate IList or an Empty IList if all Elements are removed.</returns>
			''' <remarks></remarks>
			Public Shared Function RemoveDuplicates( _
				ByVal sourceList As IList _
			) As IList

				Dim newArrayList As New ArrayList

				For i As Integer = 0 To newArrayList.Count - 1

					If Not newArrayList.Contains(newArrayList(i)) Then newArrayList.Add(newArrayList(i))

				Next

				Return newArrayList

			End Function

			''' <summary>
			''' Method to Get a Single Object from EXACTLY a one element List.
			''' </summary>
			''' <param name="sourceList">The List to get the Value From.</param>
			''' <returns>An Object or Nothing.</returns>
			''' <remarks></remarks>
			Public Shared Function GetSingleObject( _
				ByVal sourceList As IList _
			) As Object

				If Not sourceList Is Nothing AndAlso sourceList.Count = 1 Then

					Return sourceList(0)

				Else

					Return Nothing

				End If

			End Function

			''' <summary>
			''' Method to Get a Single Object from at LEAST a one element List.
			''' </summary>
			''' <param name="sourceList">The Array to get the Value From.</param>
			''' <returns>An Object or Nothing.</returns>
			''' <remarks></remarks>
			Public Shared Function GetFirstObject( _
				ByVal sourceList As IList _
			) As Object

				If Not sourceList Is Nothing AndAlso sourceList.Count >= 1 Then

					Return sourceList(0)

				Else

					Return Nothing

				End If

			End Function

			''' <summary>
			''' Method to Get an Element Type from an IList.
			''' </summary>
			''' <param name="sourceList">The IList to get the Element Type for (can be empty).</param>
			''' <returns>The Type of the Elements.</returns>
			''' <remarks>This method will check elements to find a common ancestor type.</remarks>
			Public Shared Function GetElementType( _
				ByVal sourceList As IList _
			) As Type

				Return TypeAnalyser.GetElementType(CType(sourceList, ICollection))

			End Function

			''' <summary>
			''' This method will return the Source Object/s as an Array.
			''' </summary>
			''' <param name="sourceValue">This can be a single object, an Array, an IList or ICollection.</param>
			''' <returns>An Array or Nothing.</returns>
			''' <remarks></remarks>
			Public Shared Function CreateList( _
				ByVal sourceValue As Object _
			) As IList

				Dim retList As IList = Nothing

				If Not sourceValue Is Nothing Then

					Dim sourceAnalyser As TypeAnalyser = TypeAnalyser.GetInstance(sourceValue.GetType)

					If sourceAnalyser.IsArray Then

						retList = New ArrayList(CType(sourceValue, Array))

					ElseIf sourceAnalyser.IsIList Then

						retList = sourceValue

					ElseIf sourceAnalyser.IsICollection Then

						retList = New ArrayList(CType(sourceValue, ICollection))

					Else

						retList = New ArrayList
						retList.Add(sourceValue)

					End If

				End If

				Return retList

			End Function

		#End Region

		#Region " String Methods "

			''' <summary>
			''' Method to Format a String from a Source Object (similar to String.Format).
			''' </summary>
			''' <param name="value">The String to Format.</param>
			''' <param name="sourceObject">The Manipulator to Format From.</param>
			''' <param name="sourceObjectPrefix">The Prefix with which to Identify Fields (e.g. "Parent")</param>
			''' <param name="args">The Extra Args to Format From ({0} {1} etc).</param>
			''' <returns>The Formatted String.</returns>
			''' <remarks></remarks>
			Public Shared Function Format( _
				ByVal value As String, _
				ByVal sourceObject As Object, _
				ByVal sourceObjectPrefix As String, _
				ByVal ParamArray args As Object() _
			) As String

				Dim members As String() = GetRegularExpressionMatchValues(value, FORMAT_FIELD_REGEX)

				If Not members Is Nothing Then

					members = RemoveDuplicates(members)

					For Each member As String In members

						Dim bolUseMember As Boolean = False
						Dim memberName As String = Nothing

						If String.IsNullOrEmpty(sourceObjectPrefix) Then

							memberName = member
							bolUseMember = True

						ElseIf member.StartsWith(sourceObjectPrefix & FULL_STOP, StringComparison.InvariantCultureIgnoreCase) Then

							memberName = member.Split(FULL_STOP)(member.Split(FULL_STOP).Length - 1)
							bolUseMember = True

						End If

						If bolUseMember Then

							Dim memberObject As IList = MemberAnalyser.ReadMembers( _
								sourceObject, AnalyserQuery.QUERY_MEMBERS_READABLE _
								.SetName(memberName).SetNumberOfResults(1))

							Dim memberObjectString As String = String.Empty

							If Not memberObject Is Nothing AndAlso memberObject.Count = 1 AndAlso Not memberObject(0) Is Nothing Then

								' TODO: This is a bit of hack.
								If memberObject(0).GetType.IsArray AndAlso CType(memberObject(0), Array).Length = 1 Then

									memberObjectString = memberObject(0)(0).ToString

								Else

									memberObjectString = memberObject(0).ToString

								End If

							End If

							value = value.Replace(FORMAT_FIELD_START & member & FORMAT_FIELD_END, memberObjectString)

						End If

					Next

				End If

				If args Is Nothing OrElse args.Length = 0 Then

					Return value

				Else

					Return String.Format(value, args)

				End If

			End Function

			''' <summary>
			''' Method to Format a String from a Source Object (similar to String.Format).
			''' </summary>
			''' <param name="value">The String to Format.</param>
			''' <param name="sourceObject">The Object Manipulator to Format From.</param>
			''' <param name="args">The Extra Args to Format From ({0} {1} etc).</param>
			''' <returns>The Formatted String.</returns>
			''' <remarks></remarks>
			Public Shared Function Format( _
				ByVal value As String, _
				ByVal sourceObject As Object, _
				ByVal ParamArray args As Object() _
			) As String

				Return Format(value, sourceObject, Nothing, args)

			End Function

		#End Region

		#Region " Date/Time Methods "

			#Region " General Day Methods "

				''' <summary>
				''' Method to Return the Start of a Particular Day.
				''' </summary>
				''' <param name="baseDate">The Date to get the Start Of the Day for.</param>
				''' <returns>A DateTime representing the Start Of the Day.</returns>
				''' <remarks></remarks>
				Public Shared Function GetStartOfDay( _
						ByVal baseDate As DateTime _
				) As DateTime

					Return New DateTime( _
						baseDate.Year, _
						baseDate.Month, _
						baseDate.Day, _
						0, 0, 0, 0 _
					)

				End Function

				''' <summary>
				''' Method to Return the End of a Particular Day.
				''' </summary>
				''' <param name="baseDate">The Date to get the End Of the Day for.</param>
				''' <returns>A DateTime representing the End Of the Day.</returns>
				''' <remarks></remarks>
				Public Shared Function GetEndOfDay( _
					ByVal baseDate As DateTime _
				) As DateTime

					Return New DateTime( _
						baseDate.Year, _
						baseDate.Month, _
						baseDate.Day, _
						23, 59, 59, 999)

				End Function

			#End Region

			#Region " Today Methods "

				''' <summary>
				''' Method to Return the End of a Today.
				''' </summary>
				''' <returns>A DateTime representing the End Of the Today.</returns>
				''' <remarks></remarks>
				Public Shared Function GetStartOfToday() As DateTime

					Return GetStartOfDay(Now)

				End Function

				''' <summary>
				''' Method to Return the End of a Today.
				''' </summary>
				''' <returns>A DateTime representing the End Of the Today.</returns>
				''' <remarks></remarks>
				Public Shared Function GetEndOfToday() As DateTime

					Return GetEndOfDay(Now)

				End Function

			#End Region

			#Region " General Week Methods "

				''' <summary>
				''' Method to Return the Start of a Particular Week.
				''' </summary>
				''' <param name="baseDate">The Date to get the Start of the Week for.</param>
				''' <returns>A DateTime representing the Start of the Week.</returns>
				''' <remarks></remarks>
				Public Shared Function GetStartOfWeek( _
					ByVal baseDate As DateTime _
				) As DateTime

					Dim daysToAdd As Integer

					Select Case baseDate.DayOfWeek
						Case DayOfWeek.Monday
							daysToAdd = 0
						Case DayOfWeek.Tuesday
							daysToAdd = -1
						Case DayOfWeek.Wednesday
							daysToAdd = -2
						Case DayOfWeek.Thursday
							daysToAdd = -3
						Case DayOfWeek.Friday
							daysToAdd = -4
						Case DayOfWeek.Saturday
							daysToAdd = -5
						Case DayOfWeek.Sunday
							daysToAdd = -6
					End Select

					Return GetStartOfDay(baseDate.AddDays(daysToAdd))

				End Function

				''' <summary>
				''' Method to Return the End of a Particular Week.
				''' </summary>
				''' <param name="baseDate">The Date to get the End of the Week for.</param>
				''' <returns>A DateTime representing the End of the Week.</returns>
				''' <remarks></remarks>
				Public Shared Function GetEndOfWeek( _
					ByVal baseDate As DateTime _
				) As DateTime

					Dim daysToAdd As Integer

					Select Case baseDate.DayOfWeek
						Case DayOfWeek.Monday
							daysToAdd = 6
						Case DayOfWeek.Tuesday
							daysToAdd = 5
						Case DayOfWeek.Wednesday
							daysToAdd = 4
						Case DayOfWeek.Thursday
							daysToAdd = 3
						Case DayOfWeek.Friday
							daysToAdd = 2
						Case DayOfWeek.Saturday
							daysToAdd = 1
						Case DayOfWeek.Sunday
							daysToAdd = 0
					End Select

					Return GetStartOfDay(baseDate.AddDays(daysToAdd))

				End Function

			#End Region

			#Region " This Week Methods "

				''' <summary>
				''' Method to Return the Start of a this Week.
				''' </summary>
				''' <returns>A DateTime representing the Start of the Week.</returns>
				''' <remarks></remarks>
				Public Shared Function GetStartOfThisWeek() As DateTime

					Return GetStartOfWeek(Now)

				End Function

				''' <summary>
				''' Method to Return the End of a this Week.
				''' </summary>
				''' <returns>A DateTime representing the End of the Week.</returns>
				''' <remarks></remarks>
				Public Shared Function GetEndOfThisWeek() As DateTime

					Return GetEndOfWeek(Now)

				End Function

			#End Region

		#End Region

		#Region " Public Shared Methods "

			Public Shared Function CreateAndPopulate( _
				ByVal objectType As Type, _
				ByVal ParamArray values As DictionaryEntry() _
			) As Object

				Dim sourceObject As Object = Create(objectType)

				Populate(sourceObject, values)

				Return sourceObject

			End Function

			''' <summary>
			''' Method To Clone an Object.
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function Clone( _
				ByVal source As Object _
			) As Object

				If Not source Is Nothing Then

					Dim target As Object = TypeAnalyser.Create(source.GetType)

					Integrate(target, AnalyserQuery.QUERY_VARIABLES, source)

					Return target

				End If

				Return Nothing

			End Function

			Public Shared Sub Integrate( _
				ByRef target As Object, _
				ByVal ParamArray values As Object() _
			)

				Integrate(target, AnalyserQuery.QUERY_VARIABLES, values)

			End Sub

			''' <summary>
			''' Method to Integrate this object with one or more
			''' </summary>
			''' <param name="query">The Analyser Query defining the Members to use in the Integration.</param>
			''' <param name="values">The Objects to Integrate.</param>
			''' <remarks></remarks>
			Public Shared Sub Integrate( _
				ByRef target As Object, _
				ByVal query As AnalyserQuery, _
				ByVal ParamArray values() As Object _
			)

				If Not values Is Nothing Then

					Dim analyser As TypeAnalyser = TypeAnalyser.GetInstance(target.GetType)

					Dim members As MemberAnalyser() = analyser.ExecuteQuery(query)

					For i As Integer = 0 To values.Length - 1

						If Not values(i) Is Nothing AndAlso TypeAnalyser.IsSubClassOf(values(i).GetType, target.GetType) Then

							For j As Integer = 0 To members.Length - 1

								Dim memberAnalyser As TypeAnalyser = TypeAnalyser.GetInstance(members(i).ReturnType)

								Dim targetValue As Object = members(j).Read(target)
								Dim objectValue As Object = members(j).Read(values(i))

								If memberAnalyser.IsSimple Then

									If memberAnalyser.Type Is GetType(Guid) Then

										If Not CType(objectValue, Guid) = Guid.Empty Then members(j).Write(target, objectValue)

									ElseIf Not IsNothing(objectValue) Then

										members(j).Write(target, objectValue)

									End If

								ElseIf memberAnalyser.IsIList Then

									If Not objectValue Is Nothing Then

										If targetValue Is Nothing Then

											members(j).Write(target, objectValue)

										Else

											If Not targetValue Is objectValue Then

												For k As Integer = 0 To CType(objectValue, IList).Count - 1

													Dim obj As Object = CType(objectValue, IList)(j)

													If Not CType(targetValue, IList).Contains(obj) Then CType(targetValue, IList).Add(obj)

												Next

											End If

										End If

									End If

								ElseIf memberAnalyser.IsICollection Then

								Else

									If targetValue Is Nothing Then

										If Not objectValue Is Nothing Then members(j).Write(target, objectValue)

									ElseIf Not objectValue Is Nothing Then

										Integrate(targetValue, query, objectValue)

										members(j).Write(target, targetValue)

									End If

								End If

							Next

						End If

					Next

				End If

			End Sub

			''' <summary>
			''' This method will populate the current Target object with the supplied values.
			''' </summary>
			''' <param name="values">The Member Names/Values to use in the population.</param>
			''' <remarks></remarks>
			Public Overloads Shared Sub Populate( _
				ByRef target As Object, _
				ByVal ParamArray values() As DictionaryEntry _
			)

				If Not values Is Nothing Then

					For i As Integer = 0 To values.Length - 1

						Dim analyser As TypeAnalyser = TypeAnalyser.GetInstance(target.GetType)

						Dim member As MemberAnalyser = _
							GetSingleObject( _
								analyser.ExecuteQuery( _
									AnalyserQuery.QUERY_MEMBERS_WRITEABLE _
										.SetName(values(i).Key) _
									) _
								)

						If Not member Is Nothing Then member.Write(target, values(i).Value)

					Next

				End If

			End Sub

			''' <summary>
			''' This method will populate the current Target object with the supplied values.
			''' </summary>
			''' <param name="values">The Member Names/Values to use in the population.</param>
			''' <remarks></remarks>
			Public Overloads Shared Sub Populate( _
				ByRef target As Object, _
				ByVal values As IDictionary _
			)

				If Not values Is Nothing Then

					For Each key As MemberAnalyser In values.Keys

						If Not key Is Nothing Then key.Write(target, values.Item(key))

					Next

				End If

			End Sub

		#End Region

		#Region " Class Plumbing/Interface Code "

			#Region " Multi-Threaded Implementation "

				#Region " Private Variables "

					Private m_HasDefaultConstructor_LOCK As New Object
					Private m_HasDefaultConstructor_HASVALUE As Boolean

					Private m_IsValue_LOCK As New Object
					Private m_IsValue_HASVALUE As Boolean

					Private m_IsValueArray_LOCK As New Object
					Private m_IsValueArray_HASVALUE As Boolean

					Private m_IsType_LOCK As New Object
					Private m_IsType_HASVALUE As Boolean

					Private m_IsTypeArray_LOCK As New Object
					Private m_IsTypeArray_HASVALUE As Boolean

					Private m_IsNumerical_LOCK As New Object
					Private m_IsNumerical_HASVALUE As Boolean

					Private m_IsSimple_LOCK As New Object
					Private m_IsSimple_HASVALUE As Boolean

					Private m_IsSimpleArray_LOCK As New Object
					Private m_IsSimpleArray_HASVALUE As Boolean

					Private m_IsICollection_LOCK As New Object
					Private m_IsICollection_HASVALUE As Boolean

					Private m_IsIList_LOCK As New Object
					Private m_IsIList_HASVALUE As Boolean

					Private m_IsIComparable_LOCK As New Object
					Private m_IsIComparable_HASVALUE As Boolean

					Private m_IsArray_LOCK As New Object
					Private m_IsArray_HASVALUE As Boolean

					Private m_IsException_LOCK As New Object
					Private m_IsException_HASVALUE As Boolean

					Private m_IsGeneric_LOCK As New Object
					Private m_IsGeneric_HASVALUE As Boolean

					Private m_DisplayName_LOCK As New Object
					Private m_DisplayName_HASVALUE As Boolean

					Private m_ElementType_LOCK As New Object
					Private m_ElementType_HASVALUE As Boolean

				#End Region

				#Region " Public Methods "

					Public Function UpdateHasDefaultConstructor( _
						ByVal _HasDefaultConstructor As System.Boolean _
					) As Boolean

						SyncLock m_HasDefaultConstructor_LOCK

							If Not m_HasDefaultConstructor_HASVALUE OrElse Not AreEqual(m_HasDefaultConstructor, _HasDefaultConstructor) Then

								m_HasDefaultConstructor = _HasDefaultConstructor
								m_HasDefaultConstructor_HASVALUE = True
								Return True

							Else

								Return False

							End If

						End SyncLock

					End Function

					Public Function UpdateIsValue( _
						ByVal _IsValue As System.Boolean _
					) As Boolean

						SyncLock m_IsValue_LOCK

							If Not m_IsValue_HASVALUE OrElse Not AreEqual(m_IsValue, _IsValue) Then

								m_IsValue = _IsValue
								m_IsValue_HASVALUE = True
								Return True

							Else

								Return False

							End If

						End SyncLock

					End Function

					Public Function UpdateIsType( _
						ByVal _IsType As System.Boolean _
					) As Boolean

						SyncLock m_IsType_LOCK

							If Not m_IsType_HASVALUE OrElse Not AreEqual(m_IsType, _IsType) Then

								m_IsType = _IsType
								m_IsType_HASVALUE = True
								Return True

							Else

								Return False

							End If

						End SyncLock

					End Function

					Public Function UpdateIsNumerical( _
						ByVal _IsNumerical As System.Boolean _
					) As Boolean

						SyncLock m_IsNumerical_LOCK

							If Not m_IsNumerical_HASVALUE OrElse Not AreEqual(m_IsNumerical, _IsNumerical) Then

								m_IsNumerical = _IsNumerical
								m_IsNumerical_HASVALUE = True
								Return True

							Else

								Return False

							End If

						End SyncLock

					End Function

					Public Function UpdateIsSimple( _
						ByVal _IsSimple As System.Boolean _
					) As Boolean

						SyncLock m_IsSimple_LOCK

							If Not m_IsSimple_HASVALUE OrElse Not AreEqual(m_IsSimple, _IsSimple) Then

								m_IsSimple = _IsSimple
								m_IsSimple_HASVALUE = True
								Return True

							Else

								Return False

							End If

						End SyncLock

					End Function

					Public Function UpdateIsSimpleArray( _
						ByVal _IsSimpleArray As System.Boolean _
					) As Boolean

						SyncLock m_IsSimpleArray_LOCK

							If Not m_IsSimpleArray_HASVALUE OrElse Not AreEqual(m_IsSimpleArray, _IsSimpleArray) Then
								m_IsSimpleArray = _IsSimpleArray
								m_IsSimpleArray_HASVALUE = True
								Return True

							Else

								Return False

							End If

						End SyncLock

					End Function

					Public Function UpdateIsICollection( _
						ByVal _IsICollection As System.Boolean _
					) As Boolean

						SyncLock m_IsICollection_LOCK

							If Not m_IsICollection_HASVALUE OrElse Not AreEqual(m_IsICollection, _IsICollection) Then

								m_IsICollection = _IsICollection
								m_IsICollection_HASVALUE = True
								Return True

							Else

								Return False

							End If

						End SyncLock

					End Function

					Public Function UpdateIsIList( _
						ByVal _IsIList As System.Boolean _
					) As Boolean

						SyncLock m_IsIList_LOCK

							If Not m_IsIList_HASVALUE OrElse Not AreEqual(m_IsIList, _IsIList) Then

								m_IsIList = _IsIList
								m_IsIList_HASVALUE = True
								Return True

							Else

								Return False

							End If

						End SyncLock

					End Function

					Public Function UpdateIsIComparable(
						ByVal _IsIComparable As System.Boolean
					) As Boolean

						SyncLock m_IsIComparable_LOCK

							If Not m_IsIComparable_HASVALUE OrElse Not AreEqual(m_IsIComparable, _IsIComparable) Then

								m_IsIComparable = _IsIComparable
								m_IsIComparable_HASVALUE = True
								Return True

							Else

								Return False

							End If

						End SyncLock

					End Function

					Public Function UpdateIsArray( _
						ByVal _IsArray As System.Boolean _
					) As Boolean

						SyncLock m_IsArray_LOCK

							If Not m_IsArray_HASVALUE OrElse Not AreEqual(m_IsArray, _IsArray) Then

								m_IsArray = _IsArray
								m_IsArray_HASVALUE = True
								Return True

							Else

								Return False

							End If

						End SyncLock

					End Function

					Public Function UpdateIsException( _
						ByVal _IsException As System.Boolean _
					) As Boolean

						SyncLock m_IsException_LOCK

							If Not m_IsException_HASVALUE OrElse Not AreEqual(m_IsException, _IsException) Then

								m_IsException = _IsException
								m_IsException_HASVALUE = True
								Return True

							Else

								Return False

							End If

						End SyncLock

					End Function

					Public Function UpdateIsGeneric( _
						ByVal _IsGeneric As System.Boolean _
					) As Boolean

						SyncLock m_IsGeneric_LOCK

							If Not m_IsGeneric_HASVALUE OrElse Not AreEqual(m_IsGeneric, _IsGeneric) Then

								m_IsGeneric = _IsGeneric
								m_IsGeneric_HASVALUE = True
								Return True

							Else

								Return False

							End If

						End SyncLock

					End Function

					Public Function UpdateDisplayName( _
						ByVal _DisplayName As System.String _
					) As Boolean

						SyncLock m_DisplayName_LOCK

							If Not m_DisplayName_HASVALUE OrElse Not AreEqual(m_DisplayName, _DisplayName) Then

								m_DisplayName = _DisplayName
								m_DisplayName_HASVALUE = True
								Return True

							Else

								Return False

							End If

						End SyncLock

					End Function

					Public Function UpdateElementType(
						ByVal _ElementType As System.Type _
					) As Boolean

						SyncLock m_ElementType_LOCK

							If Not m_ElementType_HASVALUE OrElse Not AreEqual(m_ElementType, _ElementType) Then

								m_ElementType = _ElementType
								m_ElementType_HASVALUE = True
								Return True

							Else

								Return False

							End If

						End SyncLock

					End Function

				#End Region

			#End Region

		#End Region

		#Region " Public Constants Variables "

			''' <summary>Public Shared Reference to the Name of the Property: Name</summary>
			Public Const PROPERTY_NAME As String = "Name"

			''' <summary>Public Shared Reference to the Name of the Property: FullName</summary>
			Public Const PROPERTY_FULLNAME As String = "FullName"

			''' <summary>Public Shared Reference to the Name of the Property: HasDefaultConstructor</summary>
			Public Const PROPERTY_HASDEFAULTCONSTRUCTOR As String = "HasDefaultConstructor"

			''' <summary>Public Shared Reference to the Name of the Property: IsValue</summary>
			Public Const PROPERTY_ISVALUE As String = "IsValue"

			''' <summary>Public Shared Reference to the Name of the Property: IsType</summary>
			Public Const PROPERTY_ISTYPE As String = "IsType"

			''' <summary>Public Shared Reference to the Name of the Property: IsNumerical</summary>
			Public Const PROPERTY_ISNUMERICAL As String = "IsNumerical"

			''' <summary>Public Shared Reference to the Name of the Property: IsSimple</summary>
			Public Const PROPERTY_ISSIMPLE As String = "IsSimple"

			''' <summary>Public Shared Reference to the Name of the Property: IsComplex</summary>
			Public Const PROPERTY_ISCOMPLEX As String = "IsComplex"

			''' <summary>Public Shared Reference to the Name of the Property: IsComplexArray</summary>
			Public Const PROPERTY_ISCOMPLEXARRAY As String = "IsComplexArray"

			''' <summary>Public Shared Reference to the Name of the Property: IsSimpleArray</summary>
			Public Const PROPERTY_ISSIMPLEARRAY As String = "IsSimpleArray"

				''' <summary>Public Shared Reference to the Name of the Property: IsICollection</summary>
			Public Const PROPERTY_ISICOLLECTION As String = "IsICollection"

			''' <summary>Public Shared Reference to the Name of the Property: IsIList</summary>
			Public Const PROPERTY_ISILIST As String = "IsIList"

			''' <summary>Public Shared Reference to the Name of the Property: IsIComparable</summary>
			Public Const PROPERTY_ISICOMPARABLE As String = "IsIComparable"

			''' <summary>Public Shared Reference to the Name of the Property: IsArray</summary>
			Public Const PROPERTY_ISARRAY As String = "IsArray"

			''' <summary>Public Shared Reference to the Name of the Property: IsScalar</summary>
			Public Const PROPERTY_ISSCALAR As String = "IsScalar"

			''' <summary>Public Shared Reference to the Name of the Property: IsException</summary>
			Public Const PROPERTY_ISEXCEPTION As String = "IsException"

			''' <summary>Public Shared Reference to the Name of the Property: IsGeneric</summary>
			Public Const PROPERTY_ISGENERIC As String = "IsGeneric"

			''' <summary>Public Shared Reference to the Name of the Property: DisplayName</summary>
			Public Const PROPERTY_DISPLAYNAME As String = "DisplayName"

			''' <summary>Public Shared Reference to the Name of the Property: ElementType</summary>
			Public Const PROPERTY_ELEMENTTYPE As String = "ElementType"

			''' <summary>Public Shared Reference to the Name of the Property: ElementTypeAnalyser</summary>
			Public Const PROPERTY_ELEMENTTYPEANALYSER As String = "ElementTypeAnalyser"

			''' <summary>Public Shared Reference to the Name of the Property: GetSingleMember</summary>
			Public Const METHOD_GETSINGLEMEMBER As String = "GetSingleMember"

			''' <summary>Public Shared Reference to the Name of the Property: ExecuteQuery</summary>
			Public Const METHOD_EXECUTEQUERY As String = "ExecuteQuery"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: HasDefaultConstructor</summary>
			Private m_HasDefaultConstructor As System.Boolean

			''' <summary>Private Data Storage Variable for Property: IsValue</summary>
			Private m_IsValue As System.Boolean

			''' <summary>Private Data Storage Variable for Property: IsValueArray</summary>
			Private m_IsValueArray As System.Boolean

			''' <summary>Private Data Storage Variable for Property: IsType</summary>
			Private m_IsType As System.Boolean

			''' <summary>Private Data Storage Variable for Property: IsTypeArray</summary>
			Private m_IsTypeArray As System.Boolean

			''' <summary>Private Data Storage Variable for Property: IsNumerical</summary>
			Private m_IsNumerical As System.Boolean

			''' <summary>Private Data Storage Variable for Property: IsSimple</summary>
			Private m_IsSimple As System.Boolean

			''' <summary>Private Data Storage Variable for Property: IsSimpleArray</summary>
			Private m_IsSimpleArray As System.Boolean

			''' <summary>Private Data Storage Variable for Property: IsICollection</summary>
			Private m_IsICollection As System.Boolean

			''' <summary>Private Data Storage Variable for Property: IsIList</summary>
			Private m_IsIList As System.Boolean

			''' <summary>Private Data Storage Variable for Property: IsIComparable</summary>
			Private m_IsIComparable As System.Boolean

			''' <summary>Private Data Storage Variable for Property: IsArray</summary>
			Private m_IsArray As System.Boolean

			''' <summary>Private Data Storage Variable for Property: IsException</summary>
			Private m_IsException As System.Boolean

			''' <summary>Private Data Storage Variable for Property: IsGeneric</summary>
			Private m_IsGeneric As System.Boolean

			''' <summary>Private Data Storage Variable for Property: DisplayName</summary>
			Private m_DisplayName As System.String

			''' <summary>Private Data Storage Variable for Property: ElementType</summary>
			Private m_ElementType As System.Type

		#End Region

		#Region " Public Properties "

			''' <summary>Analysed Type Name</summary>
			''' <remarks>Provides Access to the Property: Name</remarks>
			Public Overridable ReadOnly Property Name() As System.String
				Get
					Return Type.Name
				End Get
			End Property

			''' <summary>Analysed Type Name</summary>
			''' <remarks>Provides Access to the Property: FullName</remarks>
			Public Overridable ReadOnly Property FullName() As System.String
					Get
							Return Type.FullName
					End Get
			End Property

			''' <summary>A Boolean value indicating whether the Type has a default (parameter-less) constructor method which is Public and Callable.</summary>
			''' <remarks>Provides Access to the Property: HasDefaultConstructor</remarks>
			Public Overridable ReadOnly Property HasDefaultConstructor() As System.Boolean
				Get
					SyncLock m_HasDefaultConstructor_LOCK

						If Not m_HasDefaultConstructor_HASVALUE Then

							m_HasDefaultConstructor = Not GetDefaultConstructor() Is Nothing
							m_HasDefaultConstructor_HASVALUE = True

						End If

					End SyncLock
					Return m_HasDefaultConstructor
				End Get
			End Property

			''' <summary>Whether the analysed Type is a Value Type.</summary>
			''' <remarks>Provides Access to the Property: IsValue</remarks>
			Public Overridable ReadOnly Property IsValue() As System.Boolean
				Get
					SyncLock m_IsValue_LOCK

						If Not m_IsValue_HASVALUE Then

							m_IsValue = Type.IsValueType
							m_IsValue_HASVALUE = True

						End If

					End SyncLock
					Return m_IsValue
				End Get
			End Property

			Public Overridable ReadOnly Property IsValueArray() As System.Boolean
				Get
					SyncLock m_IsValueArray_LOCK

						If Not m_IsValueArray_HASVALUE Then

							m_IsValueArray = IsArray AndAlso ElementType.IsValueType
							m_IsValueArray_HASVALUE = True

						End If

					End SyncLock
					Return m_IsValueArray
				End Get
			End Property

			''' <summary>Whether the analysed Type is a Type Type.</summary>
			''' <remarks>Provides Access to the Property: IsType</remarks>
			Public Overridable ReadOnly Property IsType() As System.Boolean
				Get
					SyncLock m_IsType_LOCK

						If Not m_IsType_HASVALUE Then

							m_IsType = IsSubClassOf(Type, GetType(System.Type))
							m_IsType_HASVALUE = True

						End If

					End SyncLock
					Return m_IsType
				End Get
			End Property

			Public Overridable ReadOnly Property IsTypeArray() As System.Boolean
				Get
					SyncLock m_IsTypeArray_LOCK

						If Not m_IsTypeArray_HASVALUE Then

							m_IsTypeArray = IsArray AndAlso IsSubClassOf(ElementType, GetType(System.Type))
							m_IsTypeArray_HASVALUE = True

						End If

					End SyncLock
					Return m_IsTypeArray
				End Get
			End Property

			''' <summary>Whether the analysed Type is a Numeric Type.</summary>
			''' <remarks>Provides Access to the Property: IsNumerical</remarks>
			Public Overridable ReadOnly Property IsNumerical() As System.Boolean
				Get
					SyncLock m_IsNumerical_LOCK

						If Not m_IsNumerical_HASVALUE Then

							m_IsNumerical = IsTypeNumericType(Type)
							m_IsNumerical_HASVALUE = True

						End If

					End SyncLock
					Return m_IsNumerical
				End Get
			End Property

			''' <summary>Whether the analysed Type is a Simple Type.</summary>
			''' <remarks>Provides Access to the Property: IsSimple</remarks>
			Public Overridable ReadOnly Property IsSimple() As System.Boolean
				Get
					SyncLock m_IsSimple_LOCK

						If Not m_IsSimple_HASVALUE Then

							m_IsSimple = IsTypeSimpleType(Type)
							m_IsSimple_HASVALUE = True

						End If

					End SyncLock
					Return m_IsSimple
				End Get
			End Property

			''' <summary>Whether the analysed Type is a Complex Type.</summary>
			''' <remarks>Provides Access to the Property: IsComplex</remarks>
			Public Overridable ReadOnly Property IsComplex() As System.Boolean
				Get
					Return Not IsSimple
				End Get
			End Property

			''' <summary>Whether the analysed Type is an Array of Complex Types.</summary>
			''' <remarks>Provides Access to the Property: IsComplexArray</remarks>
			Public Overridable ReadOnly Property IsComplexArray() As System.Boolean
				Get
					Return IsArray AndAlso Not IsSimpleArray
				End Get
			End Property

			''' <summary>Whether the analysed Type is an Array of Simple Types.</summary>
			''' <remarks>Provides Access to the Property: IsSimpleArray</remarks>
			Public Overridable ReadOnly Property IsSimpleArray() As System.Boolean
				Get
					SyncLock m_IsSimpleArray_LOCK

						If Not m_IsSimpleArray_HASVALUE Then

							m_IsSimpleArray = IsArray AndAlso IsTypeSimpleType(Type.GetElementType)
							m_IsSimpleArray_HASVALUE = True

						End If

					End SyncLock
					Return m_IsSimpleArray
				End Get
			End Property

			''' <summary>Whether the analysed Type is an ICollection.</summary>
			''' <remarks>Provides Access to the Property: IsICollection</remarks>
			Public Overridable ReadOnly Property IsICollection() As System.Boolean
				Get
					SyncLock m_IsICollection_LOCK

						If Not m_IsICollection_HASVALUE Then

							m_IsICollection = DoesTypeImplementInterface(Type, GetType(ICollection))
							m_IsICollection_HASVALUE = True

						End If

					End SyncLock
					Return m_IsICollection
				End Get
			End Property

			''' <summary>Whether the analysed Type is an IList.</summary>
			''' <remarks>Provides Access to the Property: IsIList</remarks>
			Public Overridable ReadOnly Property IsIList() As System.Boolean
				Get
					SyncLock m_IsIList_LOCK

						If Not m_IsIList_HASVALUE Then

							m_IsIList = DoesTypeImplementInterface(Type, GetType(IList))
							m_IsIList_HASVALUE = True

						End If

					End SyncLock
					Return m_IsIList
				End Get
			End Property

			''' <summary>Whether the analysed Type is IComparable.</summary>
			''' <remarks>Provides Access to the Property: IsIComparable</remarks>
			Public Overridable ReadOnly Property IsIComparable() As System.Boolean
				Get
					SyncLock m_IsIComparable_LOCK

						If Not m_IsIComparable_HASVALUE Then

							m_IsIComparable = DoesTypeImplementInterface(Type, GetType(IComparable))
							m_IsIComparable_HASVALUE = True

						End If

					End SyncLock
					Return m_IsIComparable
				End Get
			End Property

			''' <summary>Whether the analysed Type is an Array.</summary>
			''' <remarks>Provides Access to the Property: IsArray</remarks>
			Public Overridable ReadOnly Property IsArray() As System.Boolean
				Get
					SyncLock m_IsArray_LOCK

						If Not m_IsArray_HASVALUE Then

							m_IsArray = Type.IsArray
							m_IsArray_HASVALUE = True

						End If

					End SyncLock
					Return m_IsArray
				End Get
			End Property

			''' <summary>Whether the analysed Type is an Scalar (e.g. Not an ICollection).</summary>
			''' <remarks>Provides Access to the Property: IsScalar</remarks>
			Public Overridable ReadOnly Property IsScalar() As System.Boolean
				Get
					Return Not IsICollection
				End Get
			End Property

			''' <summary>Whether the analysed Type is an Exception Type.</summary>
			''' <remarks>Provides Access to the Property: IsException</remarks>
			Public Overridable ReadOnly Property IsException() As System.Boolean
				Get
					SyncLock m_IsException_LOCK

						If Not m_IsException_HASVALUE Then

							m_IsException = IsSubClassOf(Type, GetType(System.Exception))
							m_IsException_HASVALUE = True

						End If

					End SyncLock
					Return m_IsException
				End Get
			End Property

			''' <summary>Whether the analysed Type is a Generic Type.</summary>
			''' <remarks>Provides Access to the Property: IsGeneric</remarks>
			Public Overridable ReadOnly Property IsGeneric() As System.Boolean
				Get
					SyncLock m_IsGeneric_LOCK

						If Not m_IsGeneric_HASVALUE Then

							m_IsGeneric = Type.IsGenericType
							m_IsGeneric_HASVALUE = True

						End If

					End SyncLock
					Return m_IsGeneric
				End Get
			End Property

			''' <summary>Provides access to the friendly name for the analysed type.</summary>
			''' <remarks>Provides Access to the Property: DisplayName</remarks>
			Public Overridable ReadOnly Property DisplayName() As System.String
				Get
					SyncLock m_DisplayName_LOCK

						If Not m_DisplayName_HASVALUE Then

							m_DisplayName = GetDisplayName()
							m_DisplayName_HASVALUE = True

						End If

					End SyncLock
					Return m_DisplayName
				End Get
			End Property

			''' <summary>Provides access to the element type for the analysed type.</summary>
			''' <remarks>Provides Access to the Property: ElementType</remarks>
			Public Overridable ReadOnly Property ElementType() As System.Type
				Get
					SyncLock m_ElementType_LOCK

						If Not m_ElementType_HASVALUE Then

							m_ElementType = GetElementType()
							m_ElementType_HASVALUE = True

						End If

					End SyncLock
					Return m_ElementType
				End Get
			End Property

			''' <summary>Provides access to the analyser for the type of the analysed type.</summary>
			''' <remarks>Provides Access to the Property: ElementTypeAnalyser</remarks>
			Public Overridable ReadOnly Property ElementTypeAnalyser() As TypeAnalyser
				Get
					Return TypeAnalyser.GetInstance(ElementType)
				End Get
			End Property

		#End Region

	End Class

End Namespace
