Imports Leviathan.Visualisation

Namespace Commands

	''' <summary>
	''' This interface defines the Read-Only (e.g. No Responses) contract for 'higher level' writes, e.g. those not aware of width constraints etc.
	''' </summary>
	Public Interface ICommandsOutput

		''' <summary>
		''' Show Writeable Outputs (e.g. Messages, Cubes etc - these outputs can be logged for replay rendering).
		''' </summary>
		''' <param name="values">The Outputs themselves.</param>
		Sub Show_Outputs( _
			ParamArray ByVal values As IFixedWidthWriteable() _
		)

		''' <summary>
		''' Close Output (e.g. File Stream, Send Email)
		''' </summary>
		Sub Close()

	End Interface

End Namespace
