Imports Leviathan.Commands.StringCommands
Imports System.Xml.Serialization

Namespace Commands

	Partial Public Class FormatterProperty
		Inherits MemberAnalyser

		#Region " Public Constants "

			''' <summary>
			''' Name of the Final Length Property
			''' </summary>
			''' <remarks></remarks>
			Public Const PROPERTY_FINALLENGTH As String = "FinalLength"

			''' <summary>
			''' Name of the Display Name Property
			''' </summary>
			''' <remarks></remarks>
			Public Const PROPERTY_DISPLAYNAME As String = "DisplayName"

		#End Region

		#Region " Public Properties "

			''' <summary>
			''' Provides Access to the Child of this Property (e.g. for multi-leveled properties).
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Overloads Property Child() As FormatterProperty
				Get
					Return MyBase.Child
				End Get
				Set(ByVal value As FormatterProperty)
					MyBase.Child = value
				End Set
			End Property

			''' <summary>
			''' Provides Access to the Final Length (e.g the Final Child's Length).
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Public ReadOnly Property FinalLength() As Integer
				Get
					If Not Child Is Nothing Then

						Return Child.FinalLength

					Else

						Return Length

					End If
				End Get
			End Property

			Public ReadOnly Property FinalHighlights() As List(Of HighlightedCondition)
				Get
					If Not Highlights Is Nothing Then

						Return Highlights

					ElseIf Not Child Is Nothing AndAlso Not Child.Highlights Is Nothing Then

						Return Child.Highlights

					Else

						Return New List(Of HighlightedCondition)

					End If
				End Get
			End Property

			''' <summary>
			''' Provides Access to the Display Name.
			''' </summary>
			''' <value></value>
			''' <returns>The Type.</returns>
			''' <remarks></remarks>
			Public ReadOnly Property DisplayName() As String
				Get
					If Not String.IsNullOrEmpty(InternalDisplayName) Then

						Return CamelCaseWords(InternalDisplayName)

					ElseIf Not Child Is Nothing Then

						Return Child.DisplayName

					Else

						Return CamelCaseWords(Name)

					End If
				End Get
			End Property

			''' <summary>
			''' Provides Access to the Length of the Display Name.
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Public ReadOnly Property DisplayNameLength() As Integer
				Get
					Return DisplayName.Length()
				End Get
			End Property

		#End Region

		#Region " Public Constructors "

			''' <summary>
			''' Public Constructor to Instantiate the Class.
			''' </summary>
			''' <param name="name">The Name to Populate this Class with.</param>
			''' <remarks></remarks>
			Public Sub New( _
				ByVal name As String _
			)

				MyBase.New(name)

			End Sub

			''' <summary>
			''' Public Constructor to Instantiate the Class.
			''' </summary>
			''' <param name="name">The Name to Populate this Class with.</param>
			''' <param name="length">The Length to Populate this Class with.</param>
			''' <remarks></remarks>
			Public Sub New( _
				ByVal name As String, _
				ByVal length As Integer _
			)

				Me.New(name)
				Me.Length = length

			End Sub

			''' <summary>
			''' Public Constructor to Instantiate the Class.
			''' </summary>
			''' <param name="name">The Name to Populate this Class with.</param>
			''' <param name="length">The Length to Populate this Class with.</param>
			''' <param name="displayName">The Display Name to Populate this Class with.</param>
			''' <remarks></remarks>
			Public Sub New( _
				ByVal name As String, _
				ByVal length As Integer, _
				ByVal displayName As String _
			)

				Me.New(name, length)
				Me.InternalDisplayName = displayName

			End Sub

			''' <summary>
			''' Public Constructor to Instantiate the Class.
			''' </summary>
			''' <param name="name">The Name to Populate this Class with.</param>
			''' <param name="length">The Length to Populate this Class with.</param>
			''' <param name="displayName">The Display Name to Populate this Class with.</param>
			''' <param name="index">The Index to Populate this Class with.</param>
			''' <remarks></remarks>
			Public Sub New( _
				ByVal name As String, _
				ByVal length As Integer, _
				ByVal displayName As String, _
				ByVal index As Object _
			)

				Me.New(name, length, displayName)

				If Not index Is Nothing _
					AndAlso index.GetType Is GetType(String) Then

					Dim indexInteger As Integer
					If Integer.TryParse(index, indexInteger) Then index = indexInteger

				End If

				Me.Index = index

			End Sub

		#End Region

		#Region " Protected Methods "

			''' <summary>
			''' Method to Parse the Object from a String.
			''' </summary>
			''' <param name="value">The Property as a String.</param>
			''' <remarks>e.g. field or property names[opt. index value](opt. display name)*hightlight condition!type;...*:opt. length</remarks>
			Public Overrides Function Parse( _
				Optional ByVal value As String = Nothing, _
				Optional ByVal currentType As System.Type = Nothing, _
				Optional ByVal onType As TypeAnalyser = Nothing, _
				Optional ByVal argumentCount As Integer = 0, _
				Optional ByVal accessiblity As MemberAccessibility = MemberAccessibility.Readable _
			) As Boolean

				Dim regexLengthMatch As System.Text.RegularExpressions.Match = _
					GetRegularExpressionMatch(value, "(?<=\:)[0-9]*")

				If Not regexLengthMatch Is Nothing AndAlso Not value.Substring(0, regexLengthMatch.Index).Contains(MEMBER_DELINEATOR) Then

					Integer.TryParse(regexLengthMatch.Value, Length)

					value = value.Remove(regexLengthMatch.Index - 1, regexLengthMatch.Length + 1)

				End If

				Dim regexDisplayMatch As System.Text.RegularExpressions.Match = _
					GetRegularExpressionMatch(value, "(?<=\()[A-z0-9]+[A-z0-9-\s]*(?=\))")

				If Not regexDisplayMatch Is Nothing AndAlso Not value.Substring(0, regexDisplayMatch.Index).Contains(MEMBER_DELINEATOR) Then

					InternalDisplayName = regexDisplayMatch.Value
					value = value.Remove(regexDisplayMatch.Index - 1, regexDisplayMatch.Length + 2)

				End If

				Dim regexIndexMatch As System.Text.RegularExpressions.Match = _
					GetRegularExpressionMatch(value, "(?<=\[)[A-z0-9]+[A-z0-9]*(?=\])")

				If Not regexIndexMatch Is Nothing AndAlso Not value.Substring(0, regexIndexMatch.Index).Contains(MEMBER_DELINEATOR) Then

					Index = regexIndexMatch.Value
					value = value.Remove(regexIndexMatch.Index - 1, regexIndexMatch.Length + 2)

				End If

				Dim regexHighlightMatch As System.Text.RegularExpressions.Match = GetRegularExpressionMatch(value, "(?<=\*).+(?=\*)")

				If Not regexHighlightMatch Is Nothing AndAlso Not value.Substring(0, regexHighlightMatch.Index).Contains(MEMBER_DELINEATOR) Then

					Dim conditions As String() = regexHighlightMatch.Value.Split(EXCLAMATION_MARK)

					For i As Integer = 0 To conditions.Length - 1

						Dim condition As HighlightedCondition = Nothing
						If HighlightedCondition.TryParse(conditions(i), condition) Then

							If Highlights Is Nothing Then Highlights = New List(Of HighlightedCondition)
							Highlights.Add(condition)

						End If

					Next

					value = value.Remove(regexHighlightMatch.Index - 1, regexHighlightMatch.Length + 2)

				End If

				Return MyBase.Parse(value, currentType, onType, argumentCount, accessiblity)

			End Function

		#End Region

		#Region " Public Methods "

			Public Overrides Function ToString() As String

				Return DisplayName

			End Function

		#End Region

		#Region " Public Shared Methods "

			''' <summary>
			''' Method to Attempt a Parse From String for the Class.
			''' </summary>
			''' <param name="value">The Value to Parse.</param>
			''' <param name="result">The ByRef/Out Parameter to write the parsed object to.</param>
			''' <returns>A Boolean indicating whether the parse was successful.</returns>
			''' <remarks></remarks>
			Public Overloads Shared Function TryParse( _
				ByVal value As String, _
				ByRef result As FormatterProperty _
			) As Boolean

				result = New FormatterProperty()

				Return result.Parse(value, GetType(FormatterProperty))

			End Function

			''' <summary>
			''' Method to preform a quick parse for a particular Property on a Type.
			''' </summary>
			''' <param name="value">The Name of the Property/Text Value</param>
			''' <param name="onType">The Type on which it exists (as an Analyser)</param>
			''' <returns></returns>
			Public Overloads Shared Function Parse( _
				ByVal value As String, _
				ByVal onType As TypeAnalyser _
			) As FormatterProperty

				Dim return_Property As New FormatterProperty()
				If return_Property.Parse(value, Nothing, onType) Then Return return_Property

				Return Nothing

			End Function

			''' <summary>
			''' Method to Create a List of Simple Formatter Properties
			''' </summary>
			''' <param name="names">The Names of the Properties.</param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function Create( _
				ByVal ParamArray names As String() _
			) As List(Of FormatterProperty)

				Dim returnList As New List(Of FormatterProperty)

				If Not names Is Nothing Then

					For i As Integer = 0 To names.Length - 1
						returnList.Add(New FormatterProperty(names(i)))
					Next

				End If

				Return returnList

			End Function

		#End Region

	End Class

End Namespace
