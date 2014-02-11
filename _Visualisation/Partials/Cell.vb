Imports System.IO

Namespace Visualisation

	Partial Public Class Cell

		#Region " Public Constants "

			''' <summary>
			''' String Used to Suffix a Truncated String
			''' </summary>
			''' <remarks></remarks>
			Public Const SUFFIX_TRUNCATED As String = ".."

			''' <summary>
			''' String Used to Replace a New Line
			''' </summary>
			''' <remarks></remarks>
			Public Const REPLACE_NEWLINE As String = " | "

		#End Region

		#Region " Public Properties "

			''' <summary>
			''' The Object Formatted As a String
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Public ReadOnly Property StringValue() As String
				Get
					Return StringValue(-1)
				End Get
			End Property

			''' <summary>
			''' The Length of the Object Formatted As a String (Not Truncated)
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Public ReadOnly Property StringValueLength() As System.Int32
				Get
					Return StringValue(-1).Length
				End Get
			End Property

			''' <summary>The Object Formatted As a String.</summary>
			''' <remarks>Provides Access to the Property: StringValue</remarks>
			Public Overridable ReadOnly Property StringValue( _
				ByVal maxLength As Integer _
			) As System.String
				Get
					Try
						If maxLength < 0 Then

							Return GetStringValue(Value)

						Else

							Return GetTruncatedStringValue(Value, Math.Min(maxLength, TruncationLength))

						End If
					Catch ex As Exception

						Type = InformationType.Error
						Return ERROR_TITLE

					End Try
				End Get
			End Property

		#End Region

		#Region " Public Methods "

			''' <summary>
			''' Overrides the Default ToString Representation to return StringValue.
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Overrides Function ToString() As String
					Return StringValue
			End Function

		#End Region

		#Region " Private Shared Methods "

			''' <summary>
			''' Shared Method to get the String Value of the Object.
			''' </summary>
			''' <param name="value">The Object to get the String Representation of.</param>
			''' <returns>String.Empty or a String Representing the Object.</returns>
			''' <remarks></remarks>
			Private Shared Function GetStringValue( _
				ByVal value As Object _
			) As String

				Dim string_Value As String = String.Empty

				If Not value Is Nothing Then

					string_Value = value.ToString
					string_Value = string_Value.Trim

					If string_Value.Contains(Environment.NewLine) Then string_Value = string_Value.Replace(Environment.NewLine, REPLACE_NEWLINE)

				End If

				Return string_Value

			End Function

			''' <summary>
			''' Shared Method to get the String Value of the Object.
			''' </summary>
			''' <param name="value">The Object to get the String Representation of.</param>
			''' <param name="length">The Length at which the String Value should be Truncated (0 Or Less = No Truncation)</param>
			''' <returns>String.Empty or a String Representing the Object.</returns>
			''' <remarks></remarks>
			Private Shared Function GetTruncatedStringValue( _
				ByVal value As Object, _
				ByVal length As Integer _
			) As String

				Dim strValue As String = GetStringValue(value)

				If length > 0 AndAlso strValue.Length > length Then strValue = strValue.Substring(0, length - SUFFIX_TRUNCATED.Length) & SUFFIX_TRUNCATED

				Return strValue

			End Function

		#End Region

		#Region " Public Shared Methods "

			Public Shared Function Create( _
				ByVal value As Object, _
				ByVal prop As FormatterProperty _
			) As Cell

				Dim retObject As New Cell(value, prop.Length)

				For i As Integer = 0 To prop.FinalHighlights.Count - 1

					If prop.FinalHighlights(i).CheckCondition(retObject.Value, prop.ReturnType) Then

						retObject.Type = prop.FinalHighlights(i).Type
						Exit For

					End If

				Next

				Return retObject

			End Function

			Public Shared Function Create( _
				ParamArray ByVal values As Object() _
			) As List(Of Cell)

				Dim return_List As New List(Of Cell)

				If Not values Is Nothing Then

					For i As Integer = 0 To values.Length - 1

						return_List.Add(New Cell(values(i)))

					Next

				End If

				Return return_List

			End Function

		#End Region

	End Class

End Namespace