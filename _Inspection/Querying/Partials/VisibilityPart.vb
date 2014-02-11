Namespace Inspection

	Partial Public Class VisibilityPart
		Implements IPostQueryPart, IPreQueryPart

		#Region " IPostQueryPart Implementation "

			Public Function CheckCompliancy( _
				ByVal value As FieldInfo _
			) As Boolean _
			Implements IPostQueryPart.CheckCompliancy

				If (MemberVisibility.All Or Visibility) = Visibility Then

					Return True

				ElseIf (MemberVisibility.NonPublic Or Visibility) = Visibility Then

					Return value.IsPrivate OrElse value.IsFamily OrElse _
						value.IsAssembly OrElse value.IsFamilyOrAssembly

				ElseIf (MemberVisibility.Private Or Visibility) = Visibility Then

					Return value.IsPrivate

				ElseIf (MemberVisibility.Protected Or Visibility) = Visibility Then

					Return value.IsFamily

				ElseIf (MemberVisibility.ProtectedFriend Or Visibility) = Visibility Then

					Return value.IsFamilyAndAssembly

				ElseIf (MemberVisibility.Friend Or Visibility) = Visibility Then

					Return value.IsAssembly

				ElseIf (MemberVisibility.Public Or Visibility) = Visibility Then

					Return value.IsPublic

				Else

					Return False

				End If

			End Function

			Public Function CheckCompliancy( _
				ByVal value As MethodInfo _
			) As Boolean _
			Implements IPostQueryPart.CheckCompliancy

				If (MemberVisibility.All Or Visibility) = Visibility Then

					Return True

				ElseIf (MemberVisibility.NonPublic Or Visibility) = Visibility Then

					Return value.IsPrivate OrElse value.IsFamily OrElse _
						value.IsAssembly OrElse value.IsFamilyOrAssembly

				ElseIf (MemberVisibility.Private Or Visibility) = Visibility Then

					Return value.IsPrivate

				ElseIf (MemberVisibility.Protected Or Visibility) = Visibility Then

					Return value.IsFamily

				ElseIf (MemberVisibility.ProtectedFriend Or Visibility) = Visibility Then

					Return value.IsFamilyAndAssembly

				ElseIf (MemberVisibility.Friend Or Visibility) = Visibility Then

					Return value.IsAssembly

				ElseIf (MemberVisibility.Public Or Visibility) = Visibility Then

					Return value.IsPublic

				Else

					Return False

				End If

			End Function

			Public Function CheckCompliancy( _
				ByVal value As ConstructorInfo _
			) As Boolean _
			Implements IPostQueryPart.CheckCompliancy

				If (MemberVisibility.All Or Visibility) = Visibility Then

					Return True

				ElseIf (MemberVisibility.NonPublic Or Visibility) = Visibility Then

					Return value.IsPrivate OrElse value.IsFamily OrElse _
						value.IsAssembly OrElse value.IsFamilyOrAssembly

				ElseIf (MemberVisibility.Private Or Visibility) = Visibility Then

					Return value.IsPrivate

				ElseIf (MemberVisibility.Protected Or Visibility) = Visibility Then

					Return value.IsFamily

				ElseIf (MemberVisibility.ProtectedFriend Or Visibility) = Visibility Then

					Return value.IsFamilyAndAssembly

				ElseIf (MemberVisibility.Friend Or Visibility) = Visibility Then

					Return value.IsAssembly

				ElseIf (MemberVisibility.Public Or Visibility) = Visibility Then

					Return value.IsPublic

				Else

					Return False

				End If

			End Function

			Public Function CheckCompliancy( _
				ByVal value As PropertyInfo _
			) As Boolean _
			Implements IPostQueryPart.CheckCompliancy

				Return CheckCompliancy(value.GetAccessors(True)(0))

			End Function

			Public Function CheckCompliancy( _
				ByVal value As System.Attribute _
			) As Boolean _
			Implements IPostQueryPart.CheckCompliancy

				Return CheckCompliancy(value.GetType)

			End Function

			Public Function CheckCompliancy( _
				ByVal value As System.Exception _
			) As Boolean _
			Implements IPostQueryPart.CheckCompliancy

				Return CheckCompliancy(value.GetType)

			End Function

			Public Function CheckCompliancy( _
				ByVal value As System.Type _
			) As Boolean _
			Implements IPostQueryPart.CheckCompliancy

				If (MemberVisibility.All Or Visibility) = Visibility Then

					Return True

				ElseIf (MemberVisibility.NonPublic Or Visibility) = Visibility Then

					Return value.IsNotPublic

				ElseIf (MemberVisibility.Private Or Visibility) = Visibility Then

					Return value.IsNotPublic OrElse value.IsNestedPrivate

				ElseIf (MemberVisibility.Protected Or Visibility) = Visibility Then

					Return value.IsNestedFamily

				ElseIf (MemberVisibility.ProtectedFriend Or Visibility) = Visibility Then

					Return value.IsNestedFamANDAssem

				ElseIf (MemberVisibility.Friend Or Visibility) = Visibility Then

					Return value.IsNestedAssembly

				ElseIf (MemberVisibility.Public Or Visibility) = Visibility Then

					Return value.IsPublic

				Else

					Return False

				End If

			End Function

		#End Region

		#Region " IPreQueryPart Implementation "

			Public ReadOnly Property BindingFlags() As System.Reflection.BindingFlags _
			Implements IPreQueryPart.BindingFlags
				Get

					If (MemberVisibility.All Or Visibility) = Visibility Then

						Return BindingFlags.Public Or BindingFlags.NonPublic

					ElseIf (MemberVisibility.NonPublic Or Visibility) = Visibility Then

						Return BindingFlags.NonPublic

					ElseIf (MemberVisibility.Private Or Visibility) = Visibility Then

						Return BindingFlags.NonPublic

					ElseIf (MemberVisibility.Protected Or Visibility) = Visibility Then

						Return BindingFlags.NonPublic

					ElseIf (MemberVisibility.ProtectedFriend Or Visibility) = Visibility Then

						Return BindingFlags.NonPublic

					ElseIf (MemberVisibility.Friend Or Visibility) = Visibility Then

						Return BindingFlags.NonPublic

					ElseIf (MemberVisibility.Public Or Visibility) = Visibility Then

						Return BindingFlags.Public

					Else

						Return False

					End If

				End Get
			End Property

		#End Region

	End Class

End Namespace
