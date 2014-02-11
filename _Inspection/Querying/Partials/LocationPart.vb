Namespace Inspection

	Partial Public Class LocationPart
		Implements IPostQueryPart, IPreQueryPart

		#Region " IPostQueryPart Implementation "

			Public Function CheckCompliancy( _
				ByVal value As FieldInfo _
			) As Boolean _
			Implements IPostQueryPart.CheckCompliancy

				If (MemberLocation.All Or Location) = Location Then

					Return True

				ElseIf (MemberLocation.Instance Or Location) = Location Then

					Return Not value.IsStatic

				ElseIf (MemberLocation.Static Or Location) = Location Then

					Return value.IsStatic

				Else

					Return False

				End If

			End Function

			Public Function CheckCompliancy( _
				ByVal value As MethodInfo _
			) As Boolean _
			Implements IPostQueryPart.CheckCompliancy

				If (MemberLocation.All Or Location) = Location Then

					Return True

				ElseIf (MemberLocation.Instance Or Location) = Location Then

					Return Not value.IsStatic

				ElseIf (MemberLocation.Static Or Location) = Location Then

					Return value.IsStatic

				Else

					Return False

				End If

			End Function

			Public Function CheckCompliancy( _
				ByVal value As ConstructorInfo _
			) As Boolean _
			Implements IPostQueryPart.CheckCompliancy

				If (MemberLocation.All Or Location) = Location Then

					Return True

				ElseIf (MemberLocation.Instance Or Location) = Location Then

					Return Not value.IsStatic

				ElseIf (MemberLocation.Static Or Location) = Location Then

					Return value.IsStatic

				Else

					Return False

				End If

			End Function

			Public Function CheckCompliancy( _
				ByVal value As PropertyInfo _
			) As Boolean _
			Implements IPostQueryPart.CheckCompliancy

				If (MemberLocation.All Or Location) = Location Then

					Return True

				ElseIf (MemberLocation.Instance Or Location) = Location Then

					Return True

				ElseIf (MemberLocation.Static Or Location) = Location Then

					Return True

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

				If (MemberLocation.All Or Location) = Location Then

					Return True

				ElseIf (MemberLocation.Instance Or Location) = Location Then

					Return True

				ElseIf (MemberLocation.Static Or Location) = Location Then

					Return True

				Else

					Return False

				End If

			End Function

		#End Region

		#Region " IPreQueryPart Implementation "

			Public ReadOnly Property BindingFlags() As System.Reflection.BindingFlags _
			Implements IPreQueryPart.BindingFlags
				Get

					If (MemberLocation.All Or Location) = Location Then

						Return Reflection.BindingFlags.Static Or Reflection.BindingFlags.Instance

					ElseIf (MemberLocation.Instance Or Location) = Location Then

						Return Reflection.BindingFlags.Instance

					ElseIf (MemberLocation.Static Or Location) = Location Then

						Return Reflection.BindingFlags.Static

					Else

						Return False

					End If

				End Get
			End Property

		#End Region

	End Class

End Namespace
