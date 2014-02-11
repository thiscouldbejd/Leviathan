Imports Leviathan.Resources
Imports System.Collections
Imports System.Collections.Specialized
Imports System.ComponentModel

Namespace Comparison

	Partial Public Class Comparer
		Implements IComparer

		#Region " Public Constants "

			''' <summary>
			''' Public Constant Reference to the Name of the IsNothing Method.
			''' </summary>
			''' <remarks></remarks>
			Public Const METHOD_ISNOTHING As String = "IsNothing"

			''' <summary>
			''' Public Constant Reference to the Name of the AreEqual Method.
			''' </summary>
			''' <remarks></remarks>
			Public Const METHOD_AREEQUAL As String = "AreEqual"

		#End Region

		#Region " Public Properties "

			''' <summary>
			''' The Directional Modifier for the Sort
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Public ReadOnly Property DirectionModifier() As System.Int32
				Get
					If (SortDirection = System.ComponentModel.ListSortDirection.Ascending) Then Return 1 Else Return -1
				End Get
			End Property

		#End Region

		#Region " IComparer Implementation "

			''' <summary>
			''' Compares an object with another object of the same type.
			''' </summary>
			''' <param name="x">An object to compare.</param>
			''' <param name="y">An object to compare with the other object.</param>
			''' <returns>A 32-bit signed integer that indicates the relative order of the objects being compared.
			''' The return value has these meanings:
			''' Value Meaning Less than zero x is less than y.
			''' Zero x is equal to obj.
			''' Greater than zero x is greater than y.</returns>
			''' <exception cref="ArgumentException">Supplied Object is not the same type as this instance.</exception>
			''' <remarks></remarks>
			Public Function Compare( _
				ByVal x As Object, _
				ByVal y As Object _
			) As Integer _
			Implements IComparer.Compare

				If y Is Nothing Then

					Return 1 * DirectionModifier

				ElseIf x Is Nothing Then

					Return -1 * DirectionModifier

				ElseIf y.GetType Is GetType(System.Type) AndAlso y.GetType Is GetType(System.Type) Then

					Return CompareObjects(DirectionModifier, _
						CType(x, System.Type).Name, CType(y, System.Type).Name)

				ElseIf (y.GetType Is x.GetType) OrElse (y.GetType.IsSubclassOf(x.GetType)) Then

					If (SortProperties Is Nothing OrElse SortProperties.Length = 0) Then

						Return CompareObjects(DirectionModifier, x, y)

					Else

						Return CompareObjects(DirectionModifier, x, y, SortProperties)

					End If

				Else

					Throw New ArgumentException(SingleResource(Me.GetType.Assembly, _
						RESOURCEMANAGER_NAME_EXCEPTION, "comparerArgumentUnexpected"))

				End If

			End Function

		#End Region

		#Region " Protected Shared Functions "

			''' <summary>
			''' Method to Perform Object Comparison.
			''' </summary>
			''' <param name="modifiedDirection">Effective Direction of the Sort.</param>
			''' <param name="x">Object 1 (Inherited from <seealso cref="Object"/>).</param>
			''' <param name="y">Object 2 (Inherited from <seealso cref="Object"/>).</param>
			''' <param name="sortProperties">Properties to use in the sort.</param>
			''' <returns>Comparison Result as an Integer.</returns>
			''' <remarks></remarks>
			Protected Shared Function CompareObjects( _
				ByVal modifiedDirection As Integer, _
				ByVal x As Object, _
				ByVal y As Object, _
				ByVal sortProperties As MemberAnalyser() _
			) As Integer

				' Declare the return value
				Dim comp_Value As Integer = 0

				' Iterate each of the Sort Properties
				If (Not sortProperties Is Nothing) AndAlso sortProperties.Length > 0 Then

					For i As Integer = 0 To sortProperties.Length - 1

						If sortProperties(i).ArgumentSignatureLength = 0 Then

							' Get the values
							Dim propValue_x As Object = sortProperties(i).Read(x)
							Dim propValue_y As Object = sortProperties(i).Read(y)

							comp_Value = CompareObjects(modifiedDirection, propValue_x, propValue_y)

						End If

						' There is a difference in this iteration, so exit the loop to return.
						If comp_Value <> 0 Then Exit For

					Next

				End If

				Return comp_Value

			End Function

			''' <summary>
			''' Method to Perform Object Comparison.
			''' </summary>
			''' <param name="modifiedDirection">Effective Direction of the Sort.</param>
			''' <param name="x">Object 1 (Inherited from <seealso cref="Object"/>).</param>
			''' <param name="y">Object 2 (Inherited from <seealso cref="Object"/>).</param>
			''' <returns>Comparison Result as an Integer.</returns>
			''' <remarks></remarks>
			Protected Shared Function CompareObjects( _
				ByVal modifiedDirection As Integer, _
				ByVal x As Object, _
				ByVal y As Object _
			) As Integer

				Dim comp_Value As Integer = 0

				If IsNothing(x) Then

					If IsNothing(y) Then

						comp_Value = 0

					Else

						comp_Value = -1

					End If

				ElseIf IsNothing(y) Then

					If IsNothing(x) Then

						comp_Value = 0

					Else

						comp_Value = 1

					End If

				Else

					Dim analyser As TypeAnalyser = TypeAnalyser.GetInstance(x.GetType)

					If analyser.IsSimple Then

						If (x < y) Then

							comp_Value = -1

						ElseIf (x > y) Then

							comp_Value = 1

						Else

							comp_Value = 0

						End If

					Else

						Dim comparableProperties As MemberAnalyser() = _
							analyser.ExecuteQuery( _
								New AnalyserQuery() _
									.SetReturnType(AnalyserType.MemberAnalyser) _
									.SetPresentAttribute(GetType(ComparableAttribute)) _
									.SetAccessibility(MemberAccessibility.Readable) _
									.SetVisibility(MemberVisibility.All) _
									.SetLocation(MemberLocation.Instance) _
								)

						If comparableProperties Is Nothing OrElse comparableProperties.Length = 0 Then

							If analyser.IsIComparable Then

								comp_Value = CType(x, IComparable).CompareTo(y)

							Else

								comp_Value = String.Compare(x.ToString, y.ToString)

							End If

						Else

							Return CompareObjects(modifiedDirection, x, y, comparableProperties)

						End If

					End If

				End If

				Return comp_Value * modifiedDirection

			End Function

		#End Region

		#Region " Public Shared Methods "

			''' <summary>
			''' Method that checks the Presence of an Enum Value.
			''' </summary>
			''' <param name="actualValue">The Value to Check.</param>
			''' <param name="valueRequired">The Value Required.</param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function EnumContains( _
				ByVal actualValue As Int64, _
				ParamArray ByVal valueRequired As Int64() _
			) As Boolean

				Dim retValue As Boolean = (Not valueRequired Is Nothing AndAlso valueRequired.Length > 0)

				For i As Integer = 0 To valueRequired.Length - 1
					If Not (actualValue And valueRequired(i)) = valueRequired(i) Then Return False
				Next

				Return retValue

			End Function

			''' <summary>
			''' Method that checks the Presence of an Enum Value.
			''' </summary>
			''' <param name="actualValue">The Value to Check.</param>
			''' <param name="valueRequired">The Value Required.</param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function EnumContains( _
				ByVal actualValue As Integer, _
				ParamArray ByVal valueRequired As Integer() _
			) As Boolean

				Dim retValue As Boolean = (Not valueRequired Is Nothing AndAlso valueRequired.Length > 0)

				For i As Integer = 0 To valueRequired.Length - 1
					If Not (actualValue And valueRequired(i)) = valueRequired(i) Then Return False
				Next

				Return retValue

			End Function

			''' <summary>
			''' Method that checks the Presence of an Enum Value.
			''' </summary>
			''' <param name="actualValue">The Value to Check.</param>
			''' <param name="valueRequired">The Value Required.</param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function EnumContains( _
				ByVal actualValue As Byte, _
				ParamArray ByVal valueRequired As Byte() _
			) As Boolean

				Dim retValue As Boolean = (Not valueRequired Is Nothing AndAlso valueRequired.Length > 0)

				For i As Integer = 0 To valueRequired.Length - 1
					If Not (actualValue And valueRequired(i)) = valueRequired(i) Then Return False
				Next

				Return retValue

			End Function

			''' <summary>
			''' Method to Remove the Presence of an Enum Value.
			''' </summary>
			''' <param name="actualValue">The Value to Change.</param>
			''' <param name="valueToRemove">The Value to Remove.</param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function EnumRemove( _
				ByVal actualValue As Integer, _
				ByVal valueToRemove As Integer _
			) As Integer

				If Not (actualValue And valueToRemove) = valueToRemove Then

					Return actualValue Xor valueToRemove

				Else

					Return actualValue

				End If

			End Function

			''' <summary>
			''' Method to Add the Presence of an Enum Value.
			''' </summary>
			''' <param name="actualValue">The Value to Change.</param>
			''' <param name="valueToAdd">The Value to Add.</param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function EnumAdd( _
				ByVal actualValue As Integer, _
				ByVal valueToAdd As Integer _
			) As Integer

				If Not (actualValue And valueToAdd) = valueToAdd Then

					Return actualValue Or valueToAdd

				Else

					Return actualValue

				End If

			End Function

			''' <summary>
			''' Method that Tests whether an Object is Nothing.
			''' </summary>
			''' <param name="value">The Object to Test.</param>
			''' <returns>A Boolean Value indicating whether the Object is Nothing.</returns>
			''' <remarks>This Method will handle both Value and Reference Types.</remarks>
			Public Shared Function IsNothing( _
				ByVal value As Object _
			) As Boolean

				Return (value Is Nothing OrElse (value Is System.DBNull.Value) OrElse _
					(value.GetType.IsValueType AndAlso value.Equals(Nothing) AndAlso _
					Not (value.GetType Is GetType(Int16) OrElse value.GetType Is GetType(Int32) OrElse _
					value.GetType Is GetType(Int64) OrElse value.GetType Is GetType(Single) OrElse _
					value.GetType Is GetType(Double) OrElse value.GetType Is GetType(Boolean))))

			End Function

			''' <summary>
			''' Method that Tests whether an Object is Nothing.
			''' </summary>
			''' <param name="value">The Object to Test.</param>
			''' <param name="valueType">The Type of the Object to Test.</param>
			''' <returns>A Boolean Value indicating whether the Object is Nothing.</returns>
			''' <remarks>This Method will handle both Value and Reference Types.</remarks>
			Public Shared Function IsNothing( _
				ByVal value As Object, _
				ByVal valueType As System.Type _
			) As Boolean

				Return (value Is Nothing OrElse (value Is System.DBNull.Value) OrElse _
					(valueType.IsValueType AndAlso value.Equals(Nothing) AndAlso _
					Not (valueType Is GetType(Int16) OrElse valueType Is GetType(Int32) OrElse _
					valueType Is GetType(Int64) OrElse valueType Is GetType(Single) OrElse _
					valueType Is GetType(Double) OrElse valueType Is GetType(Boolean))))

			End Function

			''' <summary>
			''' Method to Check whether Two Objects are Equal.
			''' </summary>
			''' <param name="value_1">The First Object to Check.</param>
			''' <param name="value_2">The Second Object to Check.</param>
			''' <returns>A Boolean Value indicating whether the two objects are equal.</returns>
			''' <remarks>This Method will handle both Value and Reference Types.</remarks>
			Public Shared Function AreEqual( _
				ByVal value_1 As Object, _
				ByVal value_2 As Object _
			) As Boolean

				If IsNothing(value_1) AndAlso IsNothing(value_2) Then

					Return True

				ElseIf IsNothing(value_1) OrElse IsNothing(value_2) Then

					Return False

				ElseIf Not value_1.GetType.Equals(value_2.GetType) Then

					Return False

				Else

					If value_1.GetType.IsValueType OrElse value_1.GetType Is GetType(Int16) OrElse value_1.GetType Is GetType(Int32) OrElse _
						value_1.GetType Is GetType(Int64) OrElse value_1.GetType Is GetType(Single) OrElse _
						value_1.GetType Is GetType(Double) OrElse value_1.GetType Is GetType(Boolean) OrElse _
						value_1.GetType Is GetType(String) Then

						Return value_1 = value_2

					Else

						Return value_1.Equals(value_2)

					End If

				End If

			End Function

			''' <summary>
			''' Function To Determine the Relationship between two Time Ranges.
			''' </summary>
			''' <param name="range_1_Start">The Start of the First Range.</param>
			''' <param name="range_2_Start">The Start of the Second Range.</param>
			''' <param name="range_Duration">The Duration of the Ranges.</param>
			''' <returns>A Result indicating the relationship between both ranges.
			''' NoOverlap: There is no relationship between the two ranges (e.g. they don't intersect).
			''' Same: The two ranges are functionally the same (start and end at the same point in time).
			''' FullOverlap: Range 1 fully overlaps Range 2 (it starts before and ends after).
			''' FullEnclosure: Range 1 is fully enclosed by Range 2 (it starts after and end before).
			''' StartOverlap: Range 1 overlaps the start of Range 2 (it starts before and ends before).
			''' EndtOverlap: Range 1 overlaps the end of Range 2 (it starts after and ends after).
			''' </returns>
			''' <remarks></remarks>
			Public Shared Function CompareTemporalRanges( _
				ByVal range_1_Start As DateTime, _
				ByVal range_2_Start As DateTime, _
				ByVal range_Duration As TimeSpan _
			) As RangeComparisonResult

				Return CompareTemporalRanges( _
					range_1_Start, _
					range_1_Start.Add(range_Duration), _
					range_2_Start, _
					range_2_Start.Add(range_Duration) _
				)

			End Function

			''' <summary>
			''' Function To Determine the Relationship between two Time Ranges.
			''' </summary>
			''' <param name="range_1_Start">The Start of the First Range.</param>
			''' <param name="range_1_Duration">The Duration of the First Range.</param>
			''' <param name="range_2_Start">The Start of the Second Range.</param>
			''' <param name="range_2_Duration">The Duration of the First Range.</param>
			''' <returns>A Result indicating the relationship between both ranges.
			''' NoOverlap: There is no relationship between the two ranges (e.g. they don't intersect).
			''' Same: The two ranges are functionally the same (start and end at the same point in time).
			''' FullOverlap: Range 1 fully overlaps Range 2 (it starts before and ends after).
			''' FullEnclosure: Range 1 is fully enclosed by Range 2 (it starts after and end before).
			''' StartOverlap: Range 1 overlaps the start of Range 2 (it starts before and ends before).
			''' EndtOverlap: Range 1 overlaps the end of Range 2 (it starts after and ends after).
			''' </returns>
			''' <remarks></remarks>
			Public Shared Function CompareTemporalRanges( _
				ByVal range_1_Start As DateTime, _
				ByVal range_1_Duration As TimeSpan, _
				ByVal range_2_Start As DateTime, _
				ByVal range_2_Duration As TimeSpan _
			) As RangeComparisonResult

				Return CompareTemporalRanges( _
					range_1_Start, _
					range_1_Start.Add(range_1_Duration), _
					range_2_Start, _
					range_2_Start.Add(range_2_Duration) _
				)

			End Function

			''' <summary>
			''' Function To Determine the Relationship between two Time Ranges.
			''' </summary>
			''' <param name="range_1_Date">The Date (Start and End) of the First Range.</param>
			''' <param name="range_2_Start">The Start of the Second Range.</param>
			''' <param name="range_2_End">The End of the First Range.</param>
			''' <returns>A Result indicating the relationship between both ranges.
			''' NoOverlap: There is no relationship between the two ranges (e.g. they don't intersect).
			''' Same: The two ranges are functionally the same (start and end at the same point in time).
			''' FullOverlap: Range 1 fully overlaps Range 2 (it starts before and ends after).
			''' FullEnclosure: Range 1 is fully enclosed by Range 2 (it starts after and end before).
			''' StartOverlap: Range 1 overlaps the start of Range 2 (it starts before and ends before).
			''' EndtOverlap: Range 1 overlaps the end of Range 2 (it starts after and ends after).
			''' </returns>
			''' <remarks></remarks>
			Public Shared Function CompareTemporalRanges( _
				ByVal range_1_Date As DateTime, _
				ByVal range_2_Start As DateTime, _
				ByVal range_2_End As DateTime _
			) As RangeComparisonResult

				Return CompareTemporalRanges( _
					range_1_Date, _
					range_1_Date, _
					range_2_Start, _
					range_2_End _
				)

			End Function

			''' <summary>
			''' Function To Determine the Relationship between two Time Ranges.
			''' </summary>
			''' <param name="range_1_Start">The Start of the First Range.</param>
			''' <param name="range_1_End">The End of the First Range.</param>
			''' <param name="range_2_Start">The Start of the Second Range.</param>
			''' <param name="range_2_End">The End of the First Range.</param>
			''' <returns>A Result indicating the relationship between both ranges.
			''' NoOverlap: There is no relationship between the two ranges (e.g. they don't intersect).
			''' Same: The two ranges are functionally the same (start and end at the same point in time).
			''' FullOverlap: Range 1 fully overlaps Range 2 (it starts before and ends after).
			''' FullEnclosure: Range 1 is fully enclosed by Range 2 (it starts after and end before).
			''' StartOverlap: Range 1 overlaps the start of Range 2 (it starts before and ends before).
			''' EndtOverlap: Range 1 overlaps the end of Range 2 (it starts after and ends after).
			''' </returns>
			''' <remarks></remarks>
			Public Shared Function CompareTemporalRanges( _
				ByVal range_1_Start As DateTime, _
				ByVal range_1_End As DateTime, _
				ByVal range_2_Start As DateTime, _
				ByVal range_2_End As DateTime _
			) As RangeComparisonResult

				If range_1_Start > range_1_End Then

					Throw New ArgumentException(String.Format(SingleResource(GetType(TypeAnalyser), _
						RESOURCEMANAGER_NAME_EXCEPTION, "temporalComparisonsRangeInvalid"), 1))

				ElseIf range_2_Start > range_2_End Then

					Throw New ArgumentException(String.Format(SingleResource(GetType(TypeAnalyser), _
						RESOURCEMANAGER_NAME_EXCEPTION, "temporalComparisonsRangeInvalid"), 2))

				Else

					If range_1_End <= range_2_Start _
						OrElse range_1_Start >= range_2_End Then

						Return RangeComparisonResult.NO_OVERLAP

					ElseIf range_1_Start = range_2_Start _
						AndAlso range_1_End = range_2_End Then

						Return RangeComparisonResult.SAME

					ElseIf range_1_Start <= range_2_Start _
						AndAlso range_1_End >= range_2_End Then

						Return RangeComparisonResult.FULL_OVERLAP

					ElseIf range_1_Start >= range_2_Start _
						AndAlso range_1_End <= range_2_End Then

						Return RangeComparisonResult.FULL_ENCLOSURE

					ElseIf range_1_Start < range_2_Start _
						AndAlso range_1_End < range_2_End Then

						Return RangeComparisonResult.START_OVERLAP

					ElseIf range_1_Start > range_2_Start _
						AndAlso range_1_End > range_2_End Then

						Return RangeComparisonResult.START_OVERLAP

					Else

						Return RangeComparisonResult.NO_OVERLAP

					End If

				End If

			End Function

			''' <summary>
			''' Method to Match a Name Argument to a Second Names.
			''' </summary>
			''' <param name="first_Name">The Name to be Matched.</param>
			''' <param name="second_Name">The Names to Match Against.</param>
			''' <param name="isExactMatch">A ByRef/In Parameter that if supplied as True, will only match exact and
			''' will also be used to indicate whether or not the match was exact.</param>
			''' <param name="useDelineator">Whether a delineator should be used.</param>
			''' <param name="delineatingCharacter">The Character used to Delineate Fuzzy Matches (e.g. abc-def = a.d)</param>
			''' <returns>The Name Matched or Nothing if no matches.</returns>
			''' <remarks></remarks>
			Public Shared Function MatchFuzzyString( _
				ByVal first_Name As String, _
				ByVal second_Name As String, _
				ByRef isExactMatch As Boolean, _
				ByVal useDelineator As Boolean, _
				ByVal delineatingCharacter As String _
			) As String

				If String.Compare(second_Name, first_Name, True) = 0 Then

					isExactMatch = True
					Return second_Name

				ElseIf Not isExactMatch AndAlso second_Name.StartsWith(first_Name) Then

					Return second_Name

				ElseIf Not isExactMatch AndAlso useDelineator Then

					If second_Name.Contains(delineatingCharacter) AndAlso _
						first_Name.Contains(delineatingCharacter) Then

						Dim fNames As String() = first_Name.Split(delineatingCharacter)
						Dim sNames As String() = second_Name.Split(delineatingCharacter)

						If fNames.Length = sNames.Length Then

							For j As Integer = 0 To fNames.Length - 1

								If sNames(j).StartsWith(fNames(j)) Then

									If j = fNames.Length - 1 Then

										Return second_Name

									End If

								Else

									Exit For

								End If

							Next

						End If

					End If

				End If

				Return Nothing

			End Function

			''' <summary>
			''' Method to Match a Name Argument to a Second Names.
			''' </summary>
			''' <param name="first_Name">The Name to be Matched.</param>
			''' <param name="second_Name">The Names to Match Against.</param>
			''' <param name="isExactMatch">A ByRef/In Parameter that if supplied as True, will only match exact and
			''' will also be used to indicate whether or not the match was exact.</param>
			''' <param name="delineatingCharacter">The Character used to Delineate Fuzzy Matches (e.g. abc-def = a.d)</param>
			''' <returns>The Name Matched or Nothing if no matches.</returns>
			''' <remarks></remarks>
			Public Shared Function MatchFuzzyString( _
				ByVal first_Name As String, _
				ByVal second_Name As String, _
				ByRef isExactMatch As Boolean, _
				Optional ByVal delineatingCharacter As String = Nothing _
			) As String

				Return MatchFuzzyString( _
					first_Name, _
					second_Name, _
					isExactMatch, _
					Not String.IsNullOrEmpty(delineatingCharacter), _
					delineatingCharacter _
				)

			End Function

			''' <summary>
			''' Method to Match a Name Argument to an Array of Names.
			''' </summary>
			''' <param name="name">The Name to be Matched.</param>
			''' <param name="names">The Names to Match Against.</param>
			''' <returns>The Name Matched or Nothing if no matches.</returns>
			''' <remarks></remarks>
			''' <exception cref="AmbiguousFuzzyStringMatchException">Thrown when the Match is Ambigious.</exception>
			Public Shared Function MatchFuzzyString( _
				ByVal name As String, _
				ByVal names As String(), _
				Optional ByVal delineatingCharacter As String = Nothing _
			) As String

				If name = Nothing Then

					Return Nothing

				Else

					Dim fName As String = name.ToLower, sName As String

					Dim useDelineator As Boolean = Not String.IsNullOrEmpty(delineatingCharacter)

					Dim aryMatches As New ArrayList

					For i As Integer = 0 To names.Length - 1

						If Not String.IsNullOrEmpty(names(i)) Then

							sName = names(i).ToLower

							Dim matchExact As Boolean = False
							Dim match As String = MatchFuzzyString(fName, sName, matchExact, useDelineator, delineatingCharacter)

							If Not String.IsNullOrEmpty(match) Then
								If matchExact Then
									Return sName
								Else
									aryMatches.Add(sName)
								End If
							End If

						End If

					Next

					If aryMatches.Count = 0 Then

						Return Nothing

					ElseIf aryMatches.Count = 1 Then

						Return aryMatches(0)

					Else

						Dim matches(aryMatches.Count - 1) As FuzzyMatch
						For i As Integer = 0 To aryMatches.Count - 1
							matches(i) = New FuzzyMatch(aryMatches(i))
						Next
						Throw New AmbiguousFuzzyStringMatchException(name, matches)

					End If

				End If

			End Function

		#End Region

	End Class

End Namespace
