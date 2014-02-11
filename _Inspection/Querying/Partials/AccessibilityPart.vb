Namespace Inspection

	Partial Public Class AccessibilityPart
		Implements IPostQueryPart

		#Region " IPostQueryPart Implementation "

			Public Function CheckCompliancy( _
				ByVal value As FieldInfo _
			) As Boolean _
			Implements IPostQueryPart.CheckCompliancy

				If (MemberAccessibility.All Or Accessibility) = Accessibility Then

					Return True

				ElseIf (MemberAccessibility.Readable Or Accessibility) = Accessibility Then

					Return True

				ElseIf (MemberAccessibility.ReadOnly Or Accessibility) = Accessibility Then

					Return value.IsInitOnly OrElse value.IsLiteral

				ElseIf (MemberAccessibility.ReadWrite Or Accessibility) = Accessibility Then

					Return Not value.IsInitOnly AndAlso Not value.IsLiteral

				ElseIf (MemberAccessibility.Writable Or Accessibility) = Accessibility Then

					Return Not value.IsInitOnly AndAlso Not value.IsLiteral

				ElseIf (MemberAccessibility.WriteOnly Or Accessibility) = Accessibility Then

					Return False

				Else

					Return False

				End If

			End Function

			Public Function CheckCompliancy( _
				ByVal value As MethodInfo _
			) As Boolean _
			Implements IPostQueryPart.CheckCompliancy

				If (MemberAccessibility.All Or Accessibility) = Accessibility Then

					Return True

				ElseIf (MemberAccessibility.Readable Or Accessibility) = Accessibility Then

					Return Not value.ReturnType Is Nothing

				ElseIf (MemberAccessibility.ReadOnly Or Accessibility) = Accessibility Then

					Return value.GetParameters.Length = 0 AndAlso Not value.ReturnType Is Nothing

				ElseIf (MemberAccessibility.ReadWrite Or Accessibility) = Accessibility Then

					Return Not value.ReturnType Is Nothing

				ElseIf (MemberAccessibility.Writable Or Accessibility) = Accessibility Then

					Return value.GetParameters.Length > 0

				ElseIf (MemberAccessibility.WriteOnly Or Accessibility) = Accessibility Then

					Return value.GetParameters.Length > 0 AndAlso value.ReturnType Is Nothing

				Else

					Return False

				End If

			End Function

			Public Function CheckCompliancy( _
				ByVal value As PropertyInfo _
			) As Boolean _
			Implements IPostQueryPart.CheckCompliancy

				If (MemberAccessibility.All Or Accessibility) = Accessibility Then

					Return True

				ElseIf (MemberAccessibility.Readable Or Accessibility) = Accessibility Then

					Return value.CanRead

				ElseIf (MemberAccessibility.ReadOnly Or Accessibility) = Accessibility Then

					Return value.CanRead AndAlso Not value.CanWrite

				ElseIf (MemberAccessibility.ReadWrite Or Accessibility) = Accessibility Then

					Return value.CanRead AndAlso value.CanWrite

				ElseIf (MemberAccessibility.Writable Or Accessibility) = Accessibility Then

					Return value.CanWrite

				ElseIf (MemberAccessibility.WriteOnly Or Accessibility) = Accessibility Then

					Return Not value.CanRead AndAlso value.CanWrite

				Else

					Return False

				End If

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

				Return True

			End Function

			Public Function CheckCompliancy( _
				ByVal value As ConstructorInfo _
			) As Boolean _
			Implements IPostQueryPart.CheckCompliancy

				If Accessibility Or MemberAccessibility.All Then

					Return True

				Else

					Return False

				End If

			End Function

		#End Region

	End Class

End Namespace
