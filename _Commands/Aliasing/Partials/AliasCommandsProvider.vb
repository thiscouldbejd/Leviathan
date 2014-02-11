Imports Leviathan.Configuration

Namespace Commands.Aliasing

	Partial Public Class AliasCommandsProvider
		Implements ICommandsProvider

		#Region " ICommandProvider Implementation "

			Public Function GetCommandClasses( _
				ByVal host As Leviathan.Commands.ICommandsExecution _
			) As CommandInterrogatedClass() _
			Implements ICommandsProvider.GetCommandClasses

				Dim retList As New Hashtable

				If Not Aliases Is Nothing Then

					For Each al As [Alias] In Aliases

						Dim shortcutClass As CommandInterrogatedClass

						If retList.Contains(al.Command) Then
							shortcutClass = retList(al.Command)
						Else
							shortcutClass = New CommandInterrogatedClass(al.Command, Nothing, al.Description, False)
							shortcutClass.Methods = New CommandInterrogatedMethod() {}
							shortcutClass.Flags = New CommandInterrogatedFlag() {}
							shortcutClass.Hidden = al.Hidden
						End If

						Dim aryMethods As New ArrayList(shortcutClass.Methods)

						Dim argumentLength As Integer = 0
						If Not al.Arguments Is Nothing Then argumentLength = al.Arguments.Length

						Dim params As CommandInterrogatedParameter() = _
						Array.CreateInstance(GetType(CommandInterrogatedParameter), argumentLength)

						If Not al.Arguments Is Nothing Then
							For i As Integer = 0 To al.Arguments.Length - 1
								params(i) = New CommandInterrogatedParameter( _
								al.Arguments(i), al.Arguments(i), GetType(String), i)
							Next
						End If

						aryMethods.Add(New AliasCommandInterrogatedMethod( _
							CommandType.Transform, Nothing, _
							al.Description, argumentLength, _
							al.LastArgumentArray, argumentLength, _
							Nothing, params, al))

						shortcutClass.Methods = aryMethods.ToArray(GetType(CommandInterrogatedMethod))

						retList(al.Command) = shortcutClass

					Next

				End If

				Dim retArray As CommandInterrogatedClass() = _
				Array.CreateInstance(GetType(CommandInterrogatedClass), retList.Count)

				Dim j As Integer = 0
				For Each key As Object In retList.Keys
					retArray(j) = retList(key)
					j += 1
				Next

				Return retArray

			End Function

		#End Region

	End Class

End Namespace