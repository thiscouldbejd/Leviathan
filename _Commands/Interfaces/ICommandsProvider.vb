Namespace Commands

    Public Interface ICommandsProvider

        ''' <summary>
        ''' Method to get Command Classes for Use in An Executor/Matcher.
        ''' </summary>
        ''' <returns>An Array of Command Classes.</returns>
        ''' <remarks></remarks>
        Function GetCommandClasses( _
        	ByVal host As Leviathan.Commands.ICommandsExecution _
        ) As CommandInterrogatedClass()

    End Interface

End Namespace