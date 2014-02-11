Imports System.Drawing

Namespace Configuration

	Public Class IntegerConvertor

		#Region " Public Parsing Methods "

			''' <summary>
			''' Public Parsing Method.
			''' </summary>
			''' <param name="value">The Value to Parse.</param>
			''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
			''' <returns>The Parsed Object or Nothing.</returns>
			''' <remarks></remarks>
			Public Function ParseIntegersFromString( _
				<ParsingInParameter()> ByVal value As String, _
				<ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
				<ParsingTypeParameter()> ByVal typeToParseTo As Type _
			) As Object

				If String.IsNullOrEmpty(value) OrElse value.ToLower = "h" OrElse value.ToLower = "0x" Then

					successfulParse = True
					Dim aryReturnList As New ArrayList
					aryReturnList.Add(0)
					Return aryReturnList.ToArray(typeToParseTo.GetElementType)

				ElseIf value.IndexOf(SEMI_COLON) >= 0 Then

					' If the return type is an array, split the input and handle recursively.
					Dim values As String() = value.Split(SEMI_COLON)
					Dim aryReturnList As New ArrayList

					For i As Integer = 0 To values.Length - 1

						Dim objParsed As Object = ParseIntegersFromString(values(i), successfulParse, typeToParseTo)

						If Not objParsed Is Nothing Then

							If objParsed.GetType.IsArray Then
								aryReturnList.AddRange(objParsed)
							Else
								aryReturnList.Add(objParsed)
							End If

						End If

					Next

					Return aryReturnList.ToArray(typeToParseTo.GetElementType)

				ElseIf value.IndexOf(HYPHEN) >= 0 Then

					Dim values As String() = value.Split(HYPHEN)
					If values.Length = 2 Then

						Dim objParsed_1 As Object = ParseIntegerFromString(values(0), successfulParse, typeToParseTo.GetElementType)
						If Not successfulParse Then Return Nothing
						Dim objParsed_2 As Object = ParseIntegerFromString(values(1), successfulParse, typeToParseTo.GetElementType)
						If Not successfulParse Then Return Nothing

						If objParsed_1 < objParsed_2 Then

							Dim aryReturnList As New ArrayList
							For i As Object = objParsed_1 To objParsed_2 Step 1
								aryReturnList.Add(i)
							Next
							Return aryReturnList.ToArray(typeToParseTo.GetElementType)

						ElseIf objParsed_1 > objParsed_2 Then

							Dim aryReturnList As New ArrayList
							For i As Object = objParsed_1 To objParsed_2 Step -1
								aryReturnList.Add(i)
							Next
							Return aryReturnList.ToArray(typeToParseTo.GetElementType)

						Else

							Dim aryReturnList As New ArrayList
							aryReturnList.Add(objParsed_1)
							Return aryReturnList.ToArray(typeToParseTo.GetElementType)

						End If

					Else

						successfulParse = False
						Return Nothing

					End If

				Else

					Dim aryReturnList As New ArrayList
					Dim objParsed As Object = ParseIntegerFromString(value, successfulParse, typeToParseTo.GetElementType)
					If Not objParsed Is Nothing Then
						If objParsed.GetType.IsArray Then
							aryReturnList.AddRange(objParsed)
						Else
							aryReturnList.Add(objParsed)
						End If
					End If
					Return aryReturnList.ToArray(typeToParseTo.GetElementType)

				End If

			End Function

			''' <summary>
			''' Public Parsing Method.
			''' </summary>
			''' <param name="value">The Value to Parse.</param>
			''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successful or not.</param>
			''' <returns>The Parsed Object or Nothing.</returns>
			''' <remarks></remarks>
			Public Function ParseIntegerFromString( _
				<ParsingInParameter()> ByVal value As String, _
				<ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
				<ParsingTypeParameter()> ByVal typeToParseTo As Type _
			) As Object

				Dim return_Value As Object = Nothing

				If String.IsNullOrEmpty(value) OrElse value.ToLower = "h" OrElse value.ToLower = "0x" Then

					successfulParse = True
					return_Value = 0

				ElseIf value.ToLower.StartsWith("h") Then

					If typeToParseTo = GetType(System.Byte) Then
						successfulParse = System.Byte.TryParse(value.SubString(1), System.Globalization.NumberStyles.HexNumber, Nothing, return_Value)
					ElseIf typeToParseTo = GetType(System.Int16) Then
						successfulParse = System.Int16.TryParse(value.SubString(1), System.Globalization.NumberStyles.HexNumber, Nothing, return_Value)
					ElseIf typeToParseTo = GetType(System.Int32) Then
						successfulParse = System.Int32.TryParse(value.SubString(1), System.Globalization.NumberStyles.HexNumber, Nothing, return_Value)
					ElseIf typeToParseTo = GetType(System.Int64) Then
						successfulParse = System.Int64.TryParse(value.SubString(1), System.Globalization.NumberStyles.HexNumber, Nothing, return_Value)
					End If

				ElseIf value.ToLower.StartsWith("&h") Then

					If typeToParseTo = GetType(System.Byte) Then
						successfulParse = System.Byte.TryParse(value.SubString(2), System.Globalization.NumberStyles.HexNumber, Nothing, return_Value)
					ElseIf typeToParseTo = GetType(System.Int16) Then
						successfulParse = System.Int16.TryParse(value.SubString(2), System.Globalization.NumberStyles.HexNumber, Nothing, return_Value)
					ElseIf typeToParseTo = GetType(System.Int32) Then
						successfulParse = System.Int32.TryParse(value.SubString(2), System.Globalization.NumberStyles.HexNumber, Nothing, return_Value)
					ElseIf typeToParseTo = GetType(System.Int64) Then
						successfulParse = System.Int64.TryParse(value.SubString(2), System.Globalization.NumberStyles.HexNumber, Nothing, return_Value)
					End If

				ElseIf value.ToLower.StartsWith("0x") Then

					If typeToParseTo = GetType(System.Byte) Then
						successfulParse = System.Byte.TryParse(value.SubString(2), System.Globalization.NumberStyles.HexNumber, Nothing, return_Value)
					ElseIf typeToParseTo = GetType(System.Int16) Then
						successfulParse = System.Int16.TryParse(value.SubString(2), System.Globalization.NumberStyles.HexNumber, Nothing, return_Value)
					ElseIf typeToParseTo = GetType(System.Int32) Then
						successfulParse = System.Int32.TryParse(value.SubString(2), System.Globalization.NumberStyles.HexNumber, Nothing, return_Value)
					ElseIf typeToParseTo = GetType(System.Int64) Then
						successfulParse = System.Int64.TryParse(value.SubString(2), System.Globalization.NumberStyles.HexNumber, Nothing, return_Value)
					End If

				Else

					If typeToParseTo = GetType(System.Byte) Then
						successfulParse = System.Byte.TryParse(value, return_Value)
					ElseIf typeToParseTo = GetType(System.Int16) Then
						successfulParse = System.Int16.TryParse(value, return_Value)
					ElseIf typeToParseTo = GetType(System.Int32) Then
						successfulParse = System.Int32.TryParse(value, return_Value)
					ElseIf typeToParseTo = GetType(System.Int64) Then
						successfulParse = System.Int64.TryParse(value, return_Value)
					End If

				End If

				Return return_Value

			End Function

		#End Region

	End Class

End Namespace
