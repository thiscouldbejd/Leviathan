Imports Leviathan.Commands.StringCommands
Imports System.Drawing
Imports System.Text.RegularExpressions

Namespace Configuration

	Partial Public Class FromString

		#Region " Public Constants "

			Public Const ARRAY_DELINEATOR As String = ";"

			Public Const METHOD_TRYPARSE As String = "TryParse"

		#End Region

		#Region " Public Delegates "

			Public Delegate Function ParsingMethod( _
				<ParsingInParameter()> ByVal valueToParse As String, _
				<ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
				<ParsingTypeParameter()> ByVal typeToParseTo As System.Type _
			) As Object

		#End Region

		#Region " Private Methods "

			Private Sub Populate()

				TypeParsingMethods.Add(GetType(Guid), _
					New ParsingMethod(AddressOf New GuidConvertor().ParseGuidFromString))

				TypeParsingMethods.Add(GetType(Nullable(Of Guid)), _
					New ParsingMethod(AddressOf New GuidConvertor().ParseGuidFromString))

				TypeParsingMethods.Add(GetType(Type), _
					New ParsingMethod(AddressOf New TypeConvertor().ParseTypeFromString))

				TypeParsingMethods.Add(GetType(FontFamily), _
					New ParsingMethod(AddressOf New FontConvertor().ParseFontFamilyFromString))

				TypeParsingMethods.Add(GetType(Color), _
					New ParsingMethod(AddressOf New ColourConvertor().ParseColourFromString))

				TypeParsingMethods.Add(GetType(IO.Stream), _
					New ParsingMethod(AddressOf New StreamConvertor().ParseStreamFromString))

				TypeParsingMethods.Add(GetType(IO.FileStream), _
					New ParsingMethod(AddressOf New FileStreamConvertor().ParseFileStreamFromString))

				TypeParsingMethods.Add(GetType(IO.StreamWriter), _
					New ParsingMethod(AddressOf New StreamWriterConvertor().ParseStreamWriterFromString))

				TypeParsingMethods.Add(GetType(IO.StreamReader), _
					New ParsingMethod(AddressOf New StreamReaderConvertor().ParseStreamReaderFromString))

				TypeParsingMethods.Add(GetType(TimeSpan), _
					New ParsingMethod(AddressOf New TimeSpanConvertor().ParseTimeSpanFromString))

				TypeParsingMethods.Add(GetType(System.Net.NetworkInformation.PhysicalAddress), _
					New ParsingMethod(AddressOf New MacAddressConvertor().ParseMacAddressFromString))

				TypeParsingMethods.Add(GetType(System.Net.IPHostEntry), _
					New ParsingMethod(AddressOf New IPHostEntryConvertor().ParseIPHostEntryFromString))

				TypeParsingMethods.Add(GetType(System.Net.IPAddress), _
					New ParsingMethod(AddressOf New IPAddressConvertor().ParseIPAddressFromString))

				TypeParsingMethods.Add(GetType(IO.DirectoryInfo), _
					New ParsingMethod(AddressOf New DirectoryInfoConvertor().ParseDirectoryInfoFromString))

				TypeParsingMethods.Add(GetType(IO.FileInfo), _
					New ParsingMethod(AddressOf New FileInfoConvertor().ParseFileInfoFromString))

				TypeParsingMethods.Add(GetType(Security.Principal.NTAccount), _
					New ParsingMethod(AddressOf New NTAccountConvertor().ParseNTAccountFromString))

				TypeParsingMethods.Add(GetType(System.Data.IDbConnection), _
					New ParsingMethod(AddressOf New IDbConnectionConvertor().ParseIDbConnectionFromString))

				TypeParsingMethods.Add(GetType(System.Byte), _
					New ParsingMethod(AddressOf New IntegerConvertor().ParseIntegerFromString))

				TypeParsingMethods.Add(GetType(System.Byte()), _
					New ParsingMethod(AddressOf New IntegerConvertor().ParseIntegersFromString))

				TypeParsingMethods.Add(GetType(System.Int16), _
					New ParsingMethod(AddressOf New IntegerConvertor().ParseIntegerFromString))

				TypeParsingMethods.Add(GetType(System.Int16()), _
					New ParsingMethod(AddressOf New IntegerConvertor().ParseIntegersFromString))

				TypeParsingMethods.Add(GetType(System.Int32), _
					New ParsingMethod(AddressOf New IntegerConvertor().ParseIntegerFromString))

				TypeParsingMethods.Add(GetType(System.Int32()), _
					New ParsingMethod(AddressOf New IntegerConvertor().ParseIntegersFromString))

				TypeParsingMethods.Add(GetType(System.Int64), _
					New ParsingMethod(AddressOf New IntegerConvertor().ParseIntegerFromString))

				TypeParsingMethods.Add(GetType(System.Int64()), _
					New ParsingMethod(AddressOf New IntegerConvertor().ParseIntegersFromString))

			End Sub

		#End Region

		#Region " Public Methods "

			''' <summary>
			''' Method to add a Delegate Parsing Method for a particular Type.
			''' </summary>
			''' <param name="forType">The Type to Add the Method for.</param>
			''' <param name="method">The Method to Add.</param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Function AddParsingMethod( _
				ByVal forType As Type, _
				ByVal method As ParsingMethod _
			) As FromString

				TypeParsingMethods(forType) = method

				Return Me

			End Function

			''' <summary>
			''' Method to remove a Delegate Parsing Method for a particular Type.
			''' </summary>
			''' <param name="forType">The Type to Remove the Method for.</param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Function RemoveParsingMethod( _
				ByVal forType As Type _
			) As FromString

				TypeParsingMethods.Remove(forType)

				Return Me

			End Function

			''' <summary>
			''' Method to add a Delegate Parsing Method for a particular Regex.
			''' </summary>
			''' <param name="regex">The Regex to Add the Method for.</param>
			''' <param name="method">The Method to Add.</param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Function AddParsingMethod( _
				ByVal regex As String, _
				ByVal method As ParsingMethod _
			) As FromString

				RegexParsingMethods(regex) = method

				Return Me

			End Function

			''' <summary>
			''' Method to remove a Delegate Parsing Method for a particular Regex.
			''' </summary>
			''' <param name="regex">The Regex to Remove the Method for.</param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Function RemoveParsingMethod( _
				ByVal regex As String _
			) As FromString

				RegexParsingMethods.Remove(regex)

				Return Me

			End Function

			''' <summary>
			''' Public Method that provides Parsing Functionality.
			''' </summary>
			''' <param name="value">The String Value to Parse.</param>
			''' <param name="typeToParseTo">The Type of Object to Parse To.</param>
			''' <param name="successfulParse">A ByRef/Out Parameter used to indicate whether the Parse was successfull or not.</param>
			''' <returns>The Parsed Object or Nothing.</returns>
			''' <remarks>This method may call supplied delegates if present/required. It may also use the IDataProvider (although it won't if no data objects
			''' are required.</remarks>
			Public Function Parse( _
				<ParsingInParameter()> ByVal value As String, _
				<ParsingSuccessParameter()> ByRef successfulParse As Boolean, _
				<ParsingTypeParameter()> ByVal typeToParseTo As System.Type _
			) As Object

				' TODO: Temporary addtion of: OrElse typeToParseTo Is Nothing
				' This is related to work on the web-dav provider
				If String.IsNullOrEmpty(value) Then

					' Nothing to Parse, so return a Null value.
					successfulParse = True
					Return Nothing

				Else

					' Check the Regex Parsers First.
					For Each key As String In RegexParsingMethods.Keys

						Dim regexExpression As New Regex(key)

						Dim match As String = GetRegularExpressionMatchValue(value, key)

						If Not String.IsNullOrEmpty(match) Then _
							Return CType(RegexParsingMethods(key), ParsingMethod).Invoke(match, successfulParse, typeToParseTo)

					Next

					If typeToParseTo Is GetType(String) _
						OrElse typeToParseTo Is GetType(Object) Then

						' Check for the obvious 'quick' returns.
						successfulParse = True
						Return value

					ElseIf typeToParseTo.IsEnum Then

						' Enums are a special case as array values are handled as <flags>
						Return New EnumConvertor().ParseEnumFromString(value, successfulParse, typeToParseTo)

					ElseIf typeToParseTo.IsArray Then

						' Perform a normal parse.
						For Each key As Type In TypeParsingMethods.Keys

							If key Is typeToParseTo Then Return CType(TypeParsingMethods(key), _
								ParsingMethod).Invoke(value, successfulParse, typeToParseTo)

						Next
						
						' If the return type is an array, split the input and handle recursively.
						Dim values As String() = value.Split(SEMI_COLON)
						Dim aryReturnList As New ArrayList

						For i As Integer = 0 To values.Length - 1

							Dim objParsed As Object = Parse(values(i), successfulParse, typeToParseTo.GetElementType)

							If Not objParsed Is Nothing Then

								If objParsed.GetType.IsArray Then
									aryReturnList.AddRange(objParsed)
								Else
									aryReturnList.Add(objParsed)
								End If

							End If

						Next

						Return aryReturnList.ToArray(typeToParseTo.GetElementType)

					Else

						' Perform a normal parse.
						For Each key As Type In TypeParsingMethods.Keys

							If key Is typeToParseTo Then Return CType(TypeParsingMethods(key), _
								ParsingMethod).Invoke(value, successfulParse, typeToParseTo)

						Next

						' Try looking for a Parsing Method (which takes a Parser)
						Dim parse_Meth As MethodInfo, parse_Meth_ParamTypes As Type(), parse_Meth_Params As Object()

						parse_Meth_ParamTypes = New System.Type() {GetType(String), typeToParseTo.MakeByRefType, GetType(FromString)}

						parse_Meth = typeToParseTo.GetMethod(METHOD_TRYPARSE, parse_Meth_ParamTypes)

						If Not parse_Meth Is Nothing AndAlso parse_Meth.IsStatic Then

							parse_Meth_Params = New Object(){value, Nothing, Me}

							If parse_Meth.Invoke(Nothing, parse_Meth_Params) = True Then

								successfulParse = True
								Return parse_Meth_Params(1)

							End If

						End If

						' Try looking for a Parsing Method (which doesn't take a Parser)
						parse_Meth_ParamTypes = New System.Type() {GetType(String), typeToParseTo.MakeByRefType}

						parse_Meth = typeToParseTo.GetMethod(METHOD_TRYPARSE, parse_Meth_ParamTypes)

						If Not parse_Meth Is Nothing AndAlso parse_Meth.IsStatic Then

							parse_Meth_Params = New Object() {value, Nothing}

							If parse_Meth.Invoke(Nothing, parse_Meth_Params) = True Then

								successfulParse = True
								Return parse_Meth_Params(1)

							End If

						End If

					End If

				End If

				' Nothing has worked, so give up and admit defeat!
				successfulParse = False
				Return Nothing

			End Function

		#End Region

	End Class

End Namespace
