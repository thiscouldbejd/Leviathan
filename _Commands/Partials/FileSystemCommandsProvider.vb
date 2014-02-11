Imports Leviathan.Configuration

Namespace Commands

	Partial Public Class FileSystemCommandsProvider
		Implements ICommandsProvider

		#Region " ICommandProvider Implementation "

			Public Function GetCommandClasses( _
				ByVal host As Leviathan.Commands.ICommandsExecution _
			) As CommandInterrogatedClass() _
				Implements ICommandsProvider.GetCommandClasses

				Return CommandInterrogatedClass.InterrogateDirectory(Directory, host)

			End Function

		#End Region

	End Class

End Namespace