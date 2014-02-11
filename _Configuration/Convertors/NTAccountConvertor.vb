Imports System.Security.Principal

Namespace Configuration

	Public Class NTAccountConvertor

		#Region " Private Shared Variables "

			Private Shared BUILTIN_GROUPS As String() = New String() _
			{"Account Operators", "Administrators", "Backup Operators", _
			"Guests", "Power Users", "Print Operators", "Replicator", _
			"System Operators", "Users"}

			Private Shared BUILTIN_GROUP_PREFIX As String = "BUILTIN"

		#End Region

		#Region " Public Parsing Methods "

			''' <summary>
			''' Public Parsing Method.
			''' </summary>
			''' <param name="value">The Value to Parse.</param>
			''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
			''' <returns>The Parsed Object or Nothing.</returns>
			''' <remarks></remarks>
			Public Function ParseNTAccountFromString( _
			<ParsingInParameterAttribute()> ByVal value As String, _
			<ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
			<ParsingTypeParameter()> ByVal typeToParseTo As Type _
			) As Object

				Dim ret_Account As NTAccount = Nothing

				If String.IsNullOrEmpty(value) Then

					successfulParse = False
					Return ret_Account

				Else

					If value.Contains(BACK_SLASH) Then

						ret_Account = New NTAccount( _
						value.Substring(0, value.IndexOf(BACK_SLASH)), _
						value.Substring(value.IndexOf(BACK_SLASH) + 1) _
						)

					Else

						For i As Integer = 0 To BUILTIN_GROUPS.Length - 1

							If String.Compare(value, BUILTIN_GROUPS(i), True) = 0 Then

								ret_Account = New NTAccount(BUILTIN_GROUP_PREFIX, value)
								Exit For

							End If

						Next

						If ret_Account Is Nothing Then _
							ret_Account = New NTAccount( _
								Environment.UserDomainName, value _
							)

					End If

					Try

						Dim ret_AccountSid As SecurityIdentifier = ret_Account.Translate(GetType(SecurityIdentifier))

						successfulParse = (Not ret_AccountSid Is Nothing)

					Catch ex As IdentityNotMappedException

						successfulParse = False

					End Try

					Return ret_Account

				End If

			End Function

		#End Region

	End Class

End Namespace