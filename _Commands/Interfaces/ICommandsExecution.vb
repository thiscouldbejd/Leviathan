Imports Leviathan.Visualisation

Namespace Commands

	'''<summary>Contract for Execution Environment (e.g. parent to Commands)</summary>
	Public Interface ICommandsExecution

		''' <summary>
		''' Method used to determine whether a particular Message should be generated and outputted.
		''' </summary>
		''' <param name="level">The Message Level</param>
		''' <returns>Boolean Value indicating whether Verbosity Level is available.</returns>
		Function Available( _
			ByVal level As VerbosityLevel _
		) As Boolean

		''' <summary>
		''' Method to Process a Prompt for Boolean Response
		''' </summary>
		''' <param name="question">The Question to Accompany the Prompt.</param>
		''' <returns>A Boolean Response.</returns>
		''' <remarks>This should only be called at a Verbosity Level of Interactive or Above.</remarks>
		Function BooleanResponse( _
			ByVal question As String, _
			Optional ByVal value As IFixedWidthWriteable = Nothing _
		) As Boolean

		''' <summary>
		''' Method to Process a Prompt for String Response.
		''' </summary>
		''' <param name="question">The Question to Accompany the Prompt.</param>
		''' <returns>A String Response.</returns>
		''' <remarks>This should only be called at a Verbosity Level of Interactive or Above.</remarks>
		Function StringResponse( _
			ByVal question As String, _
			Optional ByVal allow_NullResponse As Boolean = False, _
			Optional ByVal value As IFixedWidthWriteable = Nothing _
		) As String

		''' <summary>
		''' Method to Process a Prompt for Selection Response.
		''' </summary>
		''' <param name="values">The Values to Choose From.</param>
		''' <param name="question">The Question to Accompany the Prompt.</param>
		''' <param name="show_properties">The Properties to show (e.g. column headings).</param>
		''' <param name="null_Response">The null response to use (e.g. Add New or Nothing). If missing, no null response will be permitted.</param>
		''' <returns>The selected object or nothing.</returns>
		''' <remarks>This should only be called at a Verbosity Level of Interactive or Above.</remarks>
		Function SelectionResponse( _
			ByVal values As IList, _
			Optional ByVal question As String = Nothing, _
			Optional ByVal show_properties As String() = Nothing, _
			Optional ByVal null_Response As String = Nothing _
		) As Object

		''' <summary>
		''' Method to Log an Internal Command Warning (not an Error/Unhandled Exception).
		''' </summary>
		''' <param name="values">The Warning Details</param>
		''' <remark>This should only be called at a Verbosity Level of Standard or Above.</remark>
		Sub Warn( _
			ByVal ParamArray values As String() _
		)

		''' <summary>
		''' Method to Log/Output An Internal Command Debug
		''' </summary>
		''' <param name="values">The Debug Details</param>
		''' <remark>This should only be called at a Verbosity Level of Debug or Above.</remark>
		Sub [Debug]( _
			ByVal ParamArray values As String() _
		)

		''' <summary>
		''' Method to Log/Output An Internal Command Log
		''' </summary>
		''' <param name="values">The Debug Details</param>
		''' <remark>This should only be called at a Verbosity Level of Verbose or Above.</remark>
		Sub Log( _
			ByVal ParamArray values As String() _
		)

		''' <summary>
		''' Method to Log/Output An Internal Command Success
		''' </summary>
		''' <param name="values">The Success Details</param>
		''' <remark>This should only be called at a Verbosity Level of Standard or Above.</remark>
		Sub Success( _
			ByVal ParamArray values As String() _
		)

		''' <summary>
		''' Method to Output A series of Cubes
		''' </summary>
		''' <param name="values">The Cubes</param>
		Sub Show( _
			ByVal ParamArray values As IFixedWidthWriteable() _
		)

		''' <summary>
		''' Method to Log/Output Progress.
		''' </summary>
		''' <param name="value">The Current Progress (as a proportion of 1, (0 = Not Started, 1 = Complete).</param>
		''' <param name="name">The Name/Identifier of the Work being done.</param>
		''' <param name="details">Optional Details of the Name.</param>
		''' <remarks>This should only be called at a Verbosity Level of Interactive or Above.</remarks>
		Sub Progress( _
			ByVal value As Single, _
			ByVal name As String, _
			Optional details As String = Nothing _
		)

		''' <summary>
		''' This Methods Executes a Command and Returns the Output.
		''' </summary>
		''' <param name="args">String Array Arguments</param>
		''' <returns>This can be called at any Verbosity Level.</returns>
		Function Execute( _
			ByVal args As String() _
		) As Object

		''' <summary>
		''' This Method Executes a Command and Returns the Output
		''' </summary>
		''' <param name="line">The Command as a single Line</param>
		''' <returns>This can be called at any Verbosity Level.</returns>
		Function Execute( _
			ByVal line As String _
		) As Object

		''' <summary>
		''' The From String Parser that Executing Commands can use.
		''' </summary>
		Property StringParser As FromString

	End Interface

End Namespace
