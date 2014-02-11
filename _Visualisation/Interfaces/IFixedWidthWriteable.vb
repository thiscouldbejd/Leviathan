Namespace Visualisation

	Public Interface IFixedWidthWriteable

		Function Write( _
			ByVal intoWidth As Integer, _
			ByRef writer As ICommandsOutputWriter, _
			ByRef idealWidth As Integer _
		) As ICommandsOutputWriter

	End Interface

End Namespace
