Imports System.Threading

Namespace Commands.Aliasing

	Public Class AliasCommandInterrogatedMethod

		#Region " Public Properties "

			Public Overrides Property FurtherDetails() As Object
				Get
					If Not [Alias] Is Nothing Then
						Dim aryExpansion As New ArrayList
						For i As Integer = 0 To [Alias].ExpandTo.Length - 1
							If i > 0 Then aryExpansion.Add(String.Empty)
							aryExpansion.Add([Alias].ExpandTo(i).Command)
						Next
						Return aryExpansion.ToArray(GetType(String))
					End If

					Return Nothing
				End Get
				Set(ByVal value As Object)
					MyBase.FurtherDetails = value
				End Set
			End Property

			Public Overrides Property Name() As String
				Get
					Return Nothing
				End Get
				Set(ByVal value As String)
					MyBase.Name = value
				End Set
			End Property

		#End Region

		#Region " Public Constructors "

			Public Sub New( _
				ByVal type As CommandType, _
				ByVal name As String, _
				ByVal description As String, _
				ByVal argumentCount As Integer, _
				ByVal argumentCountFlexible As Boolean, _
				ByVal parameterCount As Integer, _
				ByVal invokableMethod As MemberAnalyser, _
				ByVal parameters As CommandInterrogatedParameter(), _
				ByVal [alias] As [Alias] _
			)

				MyBase.New( _
					name, description, type, argumentCount, _
					argumentCountFlexible, parameterCount, invokableMethod, parameters)

				m_Alias = [alias]

			End Sub

		#End Region

		#Region " Public Methods "

			Public Overrides Function Invoke( _
				ByVal invokedObject As Object, _
				ByVal parameters As Object(), _
				ByVal environment As ICommandsExecution _
			) As Object

				Dim l_Return As New ArrayList

				Dim sequentialExpansions As String() = [Alias].Expansions(parameters)

				For i As Integer = 0 To sequentialExpansions.Length - 1

					Dim obj_Object As Object = environment.Execute(sequentialExpansions(i))

					If Not obj_Object Is Nothing Then

						If obj_Object.GetType().IsArray Then

							l_Return.AddRange(obj_Object)

						Else

							l_Return.Add(obj_Object)

						End If

					End If

				Next

				If l_Return Is Nothing OrElse l_Return.Count = 0 Then

					Return Nothing

				ElseIf l_Return.Count = 1 Then

					Return l_Return(0)

				Else

					Return ConvertToArray(l_Return)

				End If

			End Function

			Public Overrides Function ToString() As String

				If Not [Alias] Is Nothing Then

					Return [Alias].ToString

				Else

					Return MyBase.ToString

				End If

			End Function

		#End Region

	End Class

End Namespace