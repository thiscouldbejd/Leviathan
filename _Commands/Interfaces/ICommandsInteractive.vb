Imports Leviathan.Visualisation

Namespace Commands

	''' <summary>
	''' This interface defines the 'Read-Write' (e.g. with responses) contract for 'higher level' interactive writes, e.g. those not aware of width constraints etc.
	''' </summary>
	Public Interface ICommandsInteractive

		''' <summary>
		''' Remove Last Output.
		''' </summary>
		Sub Remove_Output()

		''' <summary>
		''' Remove Last 'n' Outputs.
		''' </summary>
		Sub Remove_Outputs( _
			ByVal output_Count As Integer _
		)

		''' <summary>
		''' Show Outputs (Messages, Cubes etc - these outputs can be logged for replay rendering).
		''' </summary>
		''' <param name="values">The Outputs themselves.</param>
		Sub Show_Outputs( _
			ParamArray ByVal values As IFixedWidthWriteable() _
		)

		''' <summary>
		''' Show the Progress of the Current Command / Sub Command / Algorithm
		''' </summary>
		Sub Show_Progress( _
			ByVal value As Single, _
			ByVal name As String, _
			ByVal timeTaken As TimeSpan, _
			ByVal timeToCompletion As TimeSpan _
		)

		''' <summary>
		''' Get a Response from the Output Window.
		''' </summary>
		''' <returns></returns>
		Function Get_Response() As String

	End Interface

End Namespace
