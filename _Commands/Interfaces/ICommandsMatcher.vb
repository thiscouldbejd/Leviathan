Namespace Commands

	'''<summary>Contract for Classes that can 'match' String Args to Commands (including suggestions)</summary>
	Public Interface ICommandsMatcher

		''' <summary>
		''' Method to Prepare the Matcher.
		''' </summary>
		''' <param name="commands"></param>
		Sub Prepare( _
			commands As CommandInterrogatedClass() _
		)

		''' <summary>
		''' Method to Match a Method on a Command Class to the supplied Args.
		''' </summary>
		''' <param name="commandArgs">The Args.</param>
		''' <param name="match">The Match (will be Null if False returned).</param>
		''' <returns>A Boolean value indicating whether the Match was successful.</returns>
		''' <remarks></remarks>
		Function MatchFromArgs( _
			ByRef commandArgs As String(), _
			ByRef match As CommandInterrogated, _
			ByVal host As ICommandsExecution, _
			Optional ByVal returnNoCommand As Boolean = False _
		) As Boolean

		''' <summary>
		''' Method to Suggest Methods on Command Classes to the supplied Args.
		''' </summary>
		''' <param name="commandArgs">The Args.</param>
		''' <param name="suggestions">The Suggestions (will be Null if False returned).</param>
		''' <returns>A Boolean value indicating whether the Suggest was successful.</returns>
		''' <remarks></remarks>
		Function SuggestFromArgs( _
			ByVal commandArgs As String(), _
			ByRef suggestions As String() _
		) As Boolean

		''' <summary>
		''' Method to Suggest Methods on Command Classes to the supplied Args.
		''' </summary>
		''' <param name="commandArgs">The Args.</param>
		''' <param name="suggestions">The Suggestions (will be Null if False returned).</param>
		''' <returns>A Boolean value indicating whether the Suggest was successful.</returns>
		''' <remarks></remarks>
		Function SuggestFromArgs( _
			ByVal commandArgs As String(), _
			ByRef suggestions As CommandInterrogated() _
		) As Boolean

	End Interface

End Namespace
