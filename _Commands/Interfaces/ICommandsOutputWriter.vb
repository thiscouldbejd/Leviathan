Namespace Commands

	''' <summary>
	''' This Interface defines the low-level Contract to actually write to the Output.
	''' </summary>
	Public Interface ICommandsOutputWriter

		''' <summary>
		''' Perform a standard write (don't log).
		''' </summary>
		''' <param name="value">The value written (can be NULL)</param>
		''' <param name="type">The Information Type of the value</param>
		Sub Write( _
			ByVal value As String, _
			ByVal type As Leviathan.Visualisation.InformationType _
		)

		''' <summary>
		''' Perform a write within a Constrained Width (e.g. wrap the text).
		''' </summary>
		''' <param name="value">The value written (can be NULL)</param>
		''' <param name="width"></param>
		''' <param name="type"></param>
		''' <returns></returns>
		Function FixedWidthWrite( _
			ByRef value As String, _
			ByVal width As Integer, _
			ByVal type as Leviathan.Visualisation.InformationType _
		) As Boolean

		''' <summary>
		''' Perform a line termination.
		''' </summary>
		Sub TerminateLine( _
			Optional numberOfTimes As System.Int32 = 1 _
		)

	End Interface

End Namespace
