Imports Leviathan.Caching
Imports System.Resources

Public Class Resources

	#Region " Resource Manager Names "

		''' <summary>
		''' Public Const Reference to the Name of the Resource Manager for Attribute Expansions.
		''' </summary>
		''' <remarks></remarks>
		Public Const RESOURCEMANAGER_NAME_ATTRIBUTE As String = "AttributeExpansions"

		''' <summary>
		''' Public Const Reference to the Name of the Resource Manager for Exception Messages.
		''' </summary>
		''' <remarks></remarks>
		Public Const RESOURCEMANAGER_NAME_EXCEPTION As String = "ExceptionMessages"

		''' <summary>
		''' Public Const Reference to the Name of the Resource Manager for Log Messages.
		''' </summary>
		''' <remarks></remarks>
		Public Const RESOURCEMANAGER_NAME_LOG As String = "LogMessages"

		''' <summary>
		''' Public Const Reference to the Name of the Resource Manager for Outputs.
		''' </summary>
		''' <remarks></remarks>
		Public Const RESOURCEMANAGER_NAME_OUTPUT As String = "OutputMessages"

		''' <summary>
		''' Public Const Reference to the Name of the Resource Manager for String Formats.
		''' </summary>
		''' <remarks></remarks>
		Public Const RESOURCEMANAGER_NAME_FORMATS As String = "StringFormats"

		''' <summary>
		''' Public Constant Reference to the Name of the Resources Property.
		''' </summary>
		''' <remarks></remarks>
		Public Const PROPERTY_RESOURCES As String = "Resources"

		''' <summary>
		''' Public Constant Reference to the Name of the Single Resource Property.
		''' </summary>
		''' <remarks></remarks>
		Public Const PROPERTY_SINGLERESOURCE As String = "SingleResource"

	#End Region

	#Region " Punctuation / Symbols "

		Public Const AMPERSAND As Char = "&"c

		Public Const ANGLE_BRACKET_END As Char = ">"c

		Public Const ANGLE_BRACKET_START As Char = "<"c

		Public Const ASTERISK As Char = "*"c

		Public Const AT_SIGN As Char = "@"c

		Public Const BACK_SLASH As Char = "\"c

		Public Const BRACE_END As Char = "}"c

		Public Const BRACE_START As Char = "{"c

		Public Const BRACKET_END As Char = ")"c

		Public Const BRACKET_START As Char = "("c

		Public Const COLON As Char = ":"c

		Public Const COMMA As Char = ","c

		Public Const DOLLAR As Char = "$"c

		Public Const EXCLAMATION_MARK As Char = "!"c

		Public Const EQUALS_SIGN As Char = "="c

		Public Const FORWARD_SLASH As Char = "/"c

		Public Const FULL_STOP As Char = "."c

		Public Const HASH As Char = "#"c

		Public Const HYPHEN As Char = "-"c

		Public Const PERCENTAGE_MARK As Char = "%"c

		Public Const PIPE As Char = "|"c

		Public Const PLUS As Char = "+"c

		Public Const QUESTION_MARK As Char = "?"c

		Public Const QUOTE_SINGLE As Char = "'"c

		Public Const QUOTE_DOUBLE As Char = """"c

		Public Const SEMI_COLON As Char = ";"c

		Public Const SPACE As Char = " "c

		Public Const SQUARE_BRACKET_END As Char = "]"c

		Public Const SQUARE_BRACKET_START As Char = "["c

		Public Const TILDA As Char = "~"c

		Public Const [TAB] As Char = Microsoft.VisualBasic.ChrW(&H9)

		Public Const UNDER_SCORE As Char = "_"c

	#End Region

	#Region " Digits "

		Public Const DIGIT_ZERO As Char = "0"c

		Public Const DIGIT_ONE As Char = "1"c

		Public Const DIGIT_TWO As Char = "2"c

		Public Const DIGIT_THREE As Char = "3"c

		Public Const DIGIT_FOUR As Char = "4"c

		Public Const DIGIT_FIVE As Char = "5"c

		Public Const DIGIT_SIX As Char = "6"c

		Public Const DIGIT_SEVEN As Char = "7"c

		Public Const DIGIT_EIGHT As Char = "8"c

		Public Const DIGIT_NINE As Char = "9"c

		Public Shared DIGITS As Char() = _
			New Char() _
				{ _
					DIGIT_ZERO, _
					DIGIT_ONE, _
					DIGIT_TWO, _
					DIGIT_THREE, _
					DIGIT_FOUR, _
					DIGIT_FIVE, _
					DIGIT_SIX, _
					DIGIT_SEVEN, _
					DIGIT_EIGHT, _
					DIGIT_NINE _
				}

	#End Region

	#Region " Letters "

		Public Const LETTER_A As Char = "A"c

		Public Const LETTER_B As Char = "B"c

		Public Const LETTER_C As Char = "c"c

		Public Const LETTER_D As Char = "D"c

		Public Const LETTER_E As Char = "E"c

		Public Const LETTER_F As Char = "F"c

		Public Const LETTER_G As Char = "G"c

		Public Const LETTER_H As Char = "H"c

		Public Const LETTER_I As Char = "I"c

		Public Const LETTER_J As Char = "J"c

		Public Const LETTER_K As Char = "K"c

		Public Const LETTER_L As Char = "L"c

		Public Const LETTER_M As Char = "M"c

		Public Const LETTER_N As Char = "N"c

		Public Const LETTER_O As Char = "O"c

		Public Const LETTER_P As Char = "P"c

		Public Const LETTER_Q As Char = "Q"c

		Public Const LETTER_R As Char = "R"c

		Public Const LETTER_S As Char = "S"c

		Public Const LETTER_T As Char = "T"c

		Public Const LETTER_U As Char = "U"c

		Public Const LETTER_V As Char = "V"c

		Public Const LETTER_W As Char = "W"c

		Public Const LETTER_X As Char = "X"c

		Public Const LETTER_Y As Char = "Y"c

		Public Const LETTER_Z As Char = "Z"c

		Public Shared UPPERCASE_LETTERS As Char() = _
			New Char() _
				{ _
					LETTER_A, LETTER_B, LETTER_C, LETTER_D, LETTER_E, LETTER_F, LETTER_G, _
					LETTER_H, LETTER_I, LETTER_J, LETTER_K, LETTER_L, LETTER_M, LETTER_N, _
					LETTER_O, LETTER_P, LETTER_Q, LETTER_R, LETTER_S, LETTER_T, LETTER_U, _
					LETTER_V, LETTER_W, LETTER_X, LETTER_Y, LETTER_Z _
				}

		Public Shared LOWERCASE_LETTERS As Char() = _
			New Char() _
				{ _
					Char.ToLower(LETTER_A), Char.ToLower(LETTER_B), Char.ToLower(LETTER_C), _
					Char.ToLower(LETTER_D), Char.ToLower(LETTER_E), Char.ToLower(LETTER_F), _
					Char.ToLower(LETTER_G), Char.ToLower(LETTER_H), Char.ToLower(LETTER_I), _
					Char.ToLower(LETTER_J), Char.ToLower(LETTER_K), Char.ToLower(LETTER_L), _
					Char.ToLower(LETTER_M), Char.ToLower(LETTER_N), Char.ToLower(LETTER_O), _
					Char.ToLower(LETTER_P), Char.ToLower(LETTER_Q), Char.ToLower(LETTER_R), _
					Char.ToLower(LETTER_S), Char.ToLower(LETTER_T), Char.ToLower(LETTER_U), _
					Char.ToLower(LETTER_V), Char.ToLower(LETTER_W), Char.ToLower(LETTER_X), _
					Char.ToLower(LETTER_Y), Char.ToLower(LETTER_Z) _
				}

	#End Region

	#Region " Phonetic Digits "

		Public Const PHONETIC_ZERO As String = "Zero"

		Public Const PHONETIC_ONE As String = "One"

		Public Const PHONETIC_TWO As String = "Two"

		Public Const PHONETIC_THREE As String = "Three"

		Public Const PHONETIC_FOUR As String = "Four"

		Public Const PHONETIC_FIVE As String = "Five"

		Public Const PHONETIC_SIX As String = "Six"

		Public Const PHONETIC_SEVEN As String = "Seven"

		Public Const PHONETIC_EIGHT As String = "Eight"

		Public Const PHONETIC_NINE As String = "Nine"

		Public Shared PHONETIC_DIGITS As String() = _
			New String() _
				{ _
					PHONETIC_ZERO, PHONETIC_ONE, PHONETIC_TWO, _
					PHONETIC_THREE, PHONETIC_FOUR, PHONETIC_FIVE, _
					PHONETIC_SIX, PHONETIC_SEVEN, PHONETIC_EIGHT, PHONETIC_NINE _
				}

	#End Region

	#Region " Phonetic Letters "

		Public Const PHONETIC_LOWERCASE_A As String = "Alpha"

		Public Const PHONETIC_LOWERCASE_B As String = "Bravo"

		Public Const PHONETIC_LOWERCASE_C As String = "charlie"

		Public Const PHONETIC_LOWERCASE_D As String = "Delta"

		Public Const PHONETIC_LOWERCASE_E As String = "Echo"

		Public Const PHONETIC_LOWERCASE_F As String = "Foxtrot"

		Public Const PHONETIC_LOWERCASE_G As String = "Golf"

		Public Const PHONETIC_LOWERCASE_H As String = "Hotel"

		Public Const PHONETIC_LOWERCASE_I As String = "India"

		Public Const PHONETIC_LOWERCASE_J As String = "Juliet"

		Public Const PHONETIC_LOWERCASE_K As String = "Kilo"

		Public Const PHONETIC_LOWERCASE_L As String = "Lima"

		Public Const PHONETIC_LOWERCASE_M As String = "Mike"

		Public Const PHONETIC_LOWERCASE_N As String = "November"

		Public Const PHONETIC_LOWERCASE_O As String = "Oscar"

		Public Const PHONETIC_LOWERCASE_P As String = "Papa"

		Public Const PHONETIC_LOWERCASE_Q As String = "Quebec"

		Public Const PHONETIC_LOWERCASE_R As String = "Romeo"

		Public Const PHONETIC_LOWERCASE_S As String = "Sierra"

		Public Const PHONETIC_LOWERCASE_T As String = "Tango"

		Public Const PHONETIC_LOWERCASE_U As String = "Uniform"

		Public Const PHONETIC_LOWERCASE_V As String = "Victor"

		Public Const PHONETIC_LOWERCASE_W As String = "Whiskey"

		Public Const PHONETIC_LOWERCASE_X As String = "X-Ray"

		Public Const PHONETIC_LOWERCASE_Y As String = "Yankee"

		Public Const PHONETIC_LOWERCASE_Z As String = "Zulu"

		Public Shared PHONETIC_LOWERCASE_LETTERS As String() = _
			New String() _
				{ _
					PHONETIC_LOWERCASE_A, PHONETIC_LOWERCASE_B, PHONETIC_LOWERCASE_C, _
					PHONETIC_LOWERCASE_D, PHONETIC_LOWERCASE_E, PHONETIC_LOWERCASE_F, _
					PHONETIC_LOWERCASE_G, PHONETIC_LOWERCASE_H, PHONETIC_LOWERCASE_I, _
					PHONETIC_LOWERCASE_J, PHONETIC_LOWERCASE_K, PHONETIC_LOWERCASE_L, _
					PHONETIC_LOWERCASE_M, PHONETIC_LOWERCASE_N, PHONETIC_LOWERCASE_O, _
					PHONETIC_LOWERCASE_P, PHONETIC_LOWERCASE_Q, PHONETIC_LOWERCASE_R, _
					PHONETIC_LOWERCASE_S, PHONETIC_LOWERCASE_T, PHONETIC_LOWERCASE_U, _
					PHONETIC_LOWERCASE_V, PHONETIC_LOWERCASE_W, PHONETIC_LOWERCASE_X, _
					PHONETIC_LOWERCASE_Y, PHONETIC_LOWERCASE_Z _
				}

		Public Shared PHONETIC_UPPERCASE_A As String = PHONETIC_LOWERCASE_A.ToUpper

		Public Shared PHONETIC_UPPERCASE_B As String = PHONETIC_LOWERCASE_B.ToUpper

		Public Shared PHONETIC_UPPERCASE_C As String = PHONETIC_LOWERCASE_C.ToUpper

		Public Shared PHONETIC_UPPERCASE_D As String = PHONETIC_LOWERCASE_D.ToUpper

		Public Shared PHONETIC_UPPERCASE_E As String = PHONETIC_LOWERCASE_E.ToUpper

		Public Shared PHONETIC_UPPERCASE_F As String = PHONETIC_LOWERCASE_F.ToUpper

		Public Shared PHONETIC_UPPERCASE_G As String = PHONETIC_LOWERCASE_G.ToUpper

		Public Shared PHONETIC_UPPERCASE_H As String = PHONETIC_LOWERCASE_H.ToUpper

		Public Shared PHONETIC_UPPERCASE_I As String = PHONETIC_LOWERCASE_I.ToUpper

		Public Shared PHONETIC_UPPERCASE_J As String = PHONETIC_LOWERCASE_J.ToUpper

		Public Shared PHONETIC_UPPERCASE_K As String = PHONETIC_LOWERCASE_K.ToUpper

		Public Shared PHONETIC_UPPERCASE_L As String = PHONETIC_LOWERCASE_L.ToUpper

		Public Shared PHONETIC_UPPERCASE_M As String = PHONETIC_LOWERCASE_M.ToUpper

		Public Shared PHONETIC_UPPERCASE_N As String = PHONETIC_LOWERCASE_N.ToUpper

		Public Shared PHONETIC_UPPERCASE_O As String = PHONETIC_LOWERCASE_O.ToUpper

		Public Shared PHONETIC_UPPERCASE_P As String = PHONETIC_LOWERCASE_P.ToUpper

		Public Shared PHONETIC_UPPERCASE_Q As String = PHONETIC_LOWERCASE_Q.ToUpper

		Public Shared PHONETIC_UPPERCASE_R As String = PHONETIC_LOWERCASE_R.ToUpper

		Public Shared PHONETIC_UPPERCASE_S As String = PHONETIC_LOWERCASE_S.ToUpper

		Public Shared PHONETIC_UPPERCASE_T As String = PHONETIC_LOWERCASE_T.ToUpper

		Public Shared PHONETIC_UPPERCASE_U As String = PHONETIC_LOWERCASE_U.ToUpper

		Public Shared PHONETIC_UPPERCASE_V As String = PHONETIC_LOWERCASE_V.ToUpper

		Public Shared PHONETIC_UPPERCASE_W As String = PHONETIC_LOWERCASE_W.ToUpper

		Public Shared PHONETIC_UPPERCASE_X As String = PHONETIC_LOWERCASE_X.ToUpper

		Public Shared PHONETIC_UPPERCASE_Y As String = PHONETIC_LOWERCASE_Y.ToUpper

		Public Shared PHONETIC_UPPERCASE_Z As String = PHONETIC_LOWERCASE_Z.ToUpper

		Public Shared PHONETIC_UPPERCASE_LETTERS As String() = _
			New String() _
				{ _
					PHONETIC_UPPERCASE_A, PHONETIC_UPPERCASE_B, PHONETIC_UPPERCASE_C, _
					PHONETIC_UPPERCASE_D, PHONETIC_UPPERCASE_E, PHONETIC_UPPERCASE_F, _
					PHONETIC_UPPERCASE_G, PHONETIC_UPPERCASE_H, PHONETIC_UPPERCASE_I, _
					PHONETIC_UPPERCASE_J, PHONETIC_UPPERCASE_K, PHONETIC_UPPERCASE_L, _
					PHONETIC_UPPERCASE_M, PHONETIC_UPPERCASE_N, PHONETIC_UPPERCASE_O, _
					PHONETIC_UPPERCASE_P, PHONETIC_UPPERCASE_Q, PHONETIC_UPPERCASE_R, _
					PHONETIC_UPPERCASE_S, PHONETIC_UPPERCASE_T, PHONETIC_UPPERCASE_U, _
					PHONETIC_UPPERCASE_V, PHONETIC_UPPERCASE_W, PHONETIC_UPPERCASE_X, _
					PHONETIC_UPPERCASE_Y, PHONETIC_UPPERCASE_Z _
				}

	#End Region

	#Region " Private Shared Variables "

		Private Shared ResType As Type = GetType(Leviathan.Resources)

	#End Region

	#Region " Public Shared Variables "

		''' <summary>
		''' Provides Access to the Delineator Used in Command Naming.
		''' </summary>
		''' <remarks></remarks>
		Public Shared COMMAND_NAME_DELINEATOR As String = HYPHEN

		''' <summary>
		''' Provides Access to an Output Message for the Command No Default Error.
		''' </summary>
		''' <remarks></remarks>
		Public Shared ERROR_COMMAND_NODEFAULT As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "errorCommandNoDefault")

		''' <summary>
		''' Provides Access to an Output Message for the Command Unknown Error.
		''' </summary>
		''' <remarks></remarks>
		Public Shared ReadOnly ERROR_COMMAND_UNKNOWN As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "errorCommandUnknown")

		''' <summary>
		''' Provides Access to an Output Message for the Command Help Type Interrogation Error.
		''' </summary>
		''' <remarks></remarks>
		Public Shared ReadOnly ERROR_COMMAND_HELP_TYPEINTERROGATION As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "errorCommandHelpTypeInterrogation")

		''' <summary>
		''' Provides Access to an Output Message for the Command Ambiguous Error.
		''' </summary>
		''' <remarks></remarks>
		Public Shared ERROR_COMMAND_AMBIGUOUS As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "errorCommandAmbiguous")

		''' <summary>
		''' Provides Access to an Output Message for the Command Aborted Error.
		''' </summary>
		''' <remarks></remarks>
		Public Shared ERROR_COMMAND_ABORTED As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "errorCommandAborted")

		''' <summary>
		''' Provides Access to an Output Message for the Command General Error.
		''' </summary>
		''' <remarks></remarks>
		Public Shared ERROR_COMMAND_GENERAL As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "errorCommandGeneral")

		''' <summary>
		''' Provides Access to an Output Message for the Command Debug Error.
		''' </summary>
		''' <remarks></remarks>
		Public Shared ERROR_COMMAND_DEBUG As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "errorCommandDebug")

		''' <summary>
		''' Provides Access to an Output Message for the Parsing Failed Error.
		''' </summary>
		''' <remarks></remarks>
		Public Shared ERROR_PARSING_FAILED As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "errorParsingStringArgumentFailed")

		''' <summary>
		''' Provides Access to an Output Message for the Flag Ambiguous Error.
		''' </summary>
		''' <remarks></remarks>
		Public Shared ERROR_FLAG_AMBIGUOUS As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "errorFlagAmbiguous")

		''' <summary>
		''' Provides Access to an Output Message for the Flag Ambiguous Error.
		''' </summary>
		''' <remarks></remarks>
		Public Shared ERROR_MEMBER_NOTFOUND As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "errorMemberNotFound")

		''' <summary>
		''' Provides Access to an Output Message for the Command Succeeded Message.
		''' </summary>
		''' <remarks></remarks>
		Public Shared COMMAND_SUCCEEDED As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "messageCommandSucceeded")

		''' <summary>
		''' Provides Access to an Output Message for the Command Failed Message.
		''' </summary>
		''' <remarks></remarks>
		Public Shared COMMAND_FAILED As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "messageCommandFailed")

		''' <summary>
		''' Provides Access to an Output Message for the Help Column Title.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_COLUMN_TITLE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpColumnTitle")

		''' <summary>
		''' Provides Access to an Output Message for the Command Column Title.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_COLUMN_COMMAND As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpColumnCommand")

		''' <summary>
		''' Provides Access to an Output Message for the Flag Column Title.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_COLUMN_FLAGS As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpColumnFlags")

		''' <summary>
		''' Provides Access to an Output Message for the Parameters Column Title.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_COLUMN_PARAMETERS As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpColumnParameters")

		''' <summary>
		''' Provides Access to an Output Message for the Description Column Title.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_COLUMN_DESCRIPTION As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpColumnDescription")

		''' <summary>
		''' Provides Access to an Output Message for the Help Column Type.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_COLUMN_TYPE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpColumnType")

		''' <summary>
		''' Provides Access to an Output Message for the Title Row.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_ROW_TITLE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpRowTitle")

		''' <summary>
		''' Provides Access to an Output Message for the Version Row.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_ROW_VERSION As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpRowVersion")

		''' <summary>
		''' Provides Access to an Output Message for the Copyright Row.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_ROW_COPYRIGHT As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpRowCopyright")

		''' <summary>
		''' Provides Access to an Output Message for the Description Row.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_ROW_DESCRIPTION As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpRowDescription")

		''' <summary>
		''' Provides Access to an Output Message for the Product Row.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_ROW_PRODUCT As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpRowProduct")

		''' <summary>
		''' Provides Access to an Output Message for the CLR Row.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_ROW_CLR As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpRowClr")

		''' <summary>
		''' Provides Access to an Output Message for the Name Row.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_ROW_NAME As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpRowName")

		''' <summary>
		''' Provides Access to an Output Message for the Command Description Row.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_ROW_COMMANDDESCRIPTION As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpRowCommandDescription")

		''' <summary>
		''' Provides Access to an Output Message for the Method Description Row.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_ROW_METHODDESCRIPTION As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpRowMethodDescription")

		''' <summary>
		''' Provides Access to an Output Message for the Loading Duration Row.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_ROW_LOADINGDURATION As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpRowLoadingDuration")

		''' <summary>
		''' Provides Access to an Output Message for the Command Last Load Row.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_ROW_LASTLOAD As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpRowLastLoad")

		''' <summary>
		''' Provides Access to an Output Message for the String Used to indicate a Readable type.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_FLAG_READABLE_DESCRIPTION As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpFlagReadable")

		''' <summary>
		''' Provides Access to an Output Message for the String Used to indicate a Writable type.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_FLAG_WRITABLE_DESCRIPTION As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpFlagWritable")

		''' <summary>
		''' Provides Access to an Output Message for the String Used to indicate a Simple type.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_FLAG_SIMPLE_DESCRIPTION As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpFlagSimple")

		''' <summary>
		''' Provides Access to an Output Message for the String Used to indicate a Array type.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_FLAG_ARRAY_DESCRIPTION As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpFlagArray")

		''' <summary>
		''' Provides Access to an Output Message for the String Used to indicate a Collection type.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_FLAG_COLLECTION_DESCRIPTION As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpFlagCollection")

		''' <summary>
		''' Provides Access to an Output Message for the String Used to indicate a List type.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_FLAG_LIST_DESCRIPTION As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpFlagList")

		''' <summary>
		''' Provides Access to an Output Message for the Property Column Title.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_COLUMN_PROPERTY As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpColumnProperty")

		''' <summary>
		''' Provides Access to an Output Message for the Details Column Title.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_COLUMN_DETAILS As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpColumnDetails")

		''' <summary>
		''' Provides Access to an Output Message for the Args Column Title.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_COLUMN_ARGS As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpColumnArgs")

		''' <summary>
		''' Provides Access to an Output Message for the DeclaredIn Column Title.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_COLUMN_DECLAREDIN As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpColumnDeclaredIn")

		''' <summary>
		''' Provides Access to an Output Message for the Key Row Title.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_ROW_KEY As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpRowKey")

		''' <summary>
		''' Provides Access to an Output Message for the Enum Name Column Title.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_COLUMN_ENUM_NAME As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpColumnEnumName")

		''' <summary>
		''' Provides Access to an Output Message for the Enum Value Column Title.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_COLUMN_ENUM_VALUE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpColumnEnumValue")

		''' <summary>
		''' Provides Access to an Output Message for the CLR Entry Assembly.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_ENTRY_ASSEMBLY As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpEntryAssembly")

		''' <summary>
		''' Provides Access to an Output Message for the Command Declaring Assembly.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_COMMAND_ASSEMBLY As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpCommandAssembly")

		''' <summary>
		''' Provides Access to an Output Message for the Type Declaring Assembly.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_TYPE_ASSEMBLY As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpTypeAssembly")

		''' <summary>
		''' Provides Access to an Output Message for the Command Details.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_COMMAND_DETAILS As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpTitleCommandDetails")

		''' <summary>
		''' Provides Access to an Output Message for the Further Details.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_COMMAND_FURTHERDETAILS As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpTitleCommandFurtherDetails")

		''' <summary>
		''' Provides Access to an Output Message for the Flag Details.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_COMMAND_FLAGDETAILS As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpTitleCommandFlagDetails")

		''' <summary>
		''' Provides Access to an Output Message for the Type Details.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_TYPE_DETAILS As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpTitleTypeDetails")

		''' <summary>
		''' Provides Access to an Output Message for the Enum Details.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_ENUM_DETAILS As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpTitleEnumDetails")

		''' <summary>
		''' Provides Access to an Output Message for the Return Type.
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_RETURN_TYPE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpTitleReturnType")

		''' <summary>
		''' Provides Access to an Keyboard Shortcut Title
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_KEY_TITLE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpKeyTitle")

		''' <summary>
		''' Provides Access to an Key Value for F1
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_KEY_F1 As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpKeyF1")

		''' <summary>
		''' Provides Access to an Key Value for F2
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_KEY_F2 As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpKeyF2")

		''' <summary>
		''' Provides Access to an Key Value for F3
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_KEY_F3 As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpKeyF3")

		''' <summary>
		''' Provides Access to an Key Value for F4
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_KEY_F4 As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpKeyF4")

		''' <summary>
		''' Provides Access to an Key Value for F5
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_KEY_F5 As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpKeyF5")

		''' <summary>
		''' Provides Access to an Key Value for F6
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_KEY_F6 As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpKeyF6")

		''' <summary>
		''' Provides Access to an Key Value for F7
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_KEY_F7 As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpKeyF7")

		''' <summary>
		''' Provides Access to an Key Value for F8
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_KEY_F8 As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpKeyF8")

		''' <summary>
		''' Provides Access to an Description Value for F1
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_KEY_F1_DESC As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpKeyF1Description")

		''' <summary>
		''' Provides Access to an Description Value for F2
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_KEY_F2_DESC As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpKeyF2Description")

		''' <summary>
		''' Provides Access to an Description Value for F3
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_KEY_F3_DESC As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpKeyF3Description")

		''' <summary>
		''' Provides Access to an Description Value for F4
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_KEY_F4_DESC As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpKeyF4Description")

		''' <summary>
		''' Provides Access to an Description Value for F5
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_KEY_F5_DESC As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpKeyF5Description")

		''' <summary>
		''' Provides Access to an Description Value for F6
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_KEY_F6_DESC As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpKeyF6Description")

		''' <summary>
		''' Provides Access to an Description Value for F7
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_KEY_F7_DESC As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpKeyF7Description")

		''' <summary>
		''' Provides Access to an Description Value for F8
		''' </summary>
		''' <remarks></remarks>
		Public Shared HELP_KEY_F8_DESC As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "helpKeyF8Description")

		''' <summary>
		''' Provides Access to an Output Message for the Execution Run Message.
		''' </summary>
		''' <remarks></remarks>
		Public Shared EXECUTION_RUN As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "executionRun")

		''' <summary>
		''' Provides Access to an Output Message for the Execution Pre-Parse Args Message.
		''' </summary>
		''' <remarks></remarks>
		Public Shared EXECUTION_DEBUG_PREPARSE_ARGS As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "executionDebugPreParseArgs")

		''' <summary>
		''' Provides Access to an Output Message for the Execution Post-Parse Args Message.
		''' </summary>
		''' <remarks></remarks>
		Public Shared EXECUTION_DEBUG_POSTPARSE_ARGS As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "executionDebugPostParseArgs")

		''' <summary>
		''' Provides Access to an Output Message for Performance.
		''' </summary>
		''' <remarks></remarks>
		Public Shared PERFORMANCE_COLUMN_TITLE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "performanceColumnTitle")

		''' <summary>
		''' Provides Access to an Output Message for Performance Column Work-Done.
		''' </summary>
		''' <remarks></remarks>
		Public Shared PERFORMANCE_COLUMN_WORKDONE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "performanceColumnWorkDone")

		''' <summary>
		''' Provides Access to an Output Message for Performance Column Took.
		''' </summary>
		''' <remarks></remarks>
		Public Shared PERFORMANCE_COLUMN_TOOK As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "performanceColumnTook")

		''' <summary>
		''' Provides Access to an Output Message for Performance Column Memory Working Set.
		''' </summary>
		''' <remarks></remarks>
		Public Shared PERFORMANCE_COLUMN_MEMWORKINGSET As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "performanceColumnMemWorkingSet")

		''' <summary>
		''' Provides Access to an Output Message for Performance Column Order.
		''' </summary>
		''' <remarks></remarks>
		Public Shared PERFORMANCE_COLUMN_ORDER As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "performanceColumnOrder")

		''' <summary>
		''' Provides Access to an Output Message for Performance Column Count.
		''' </summary>
		''' <remarks></remarks>
		Public Shared PERFORMANCE_COLUMN_COUNT As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "performanceColumnCount")

		''' <summary>
		''' Provides Access to an Output Message for Performance Column Min.
		''' </summary>
		''' <remarks></remarks>
		Public Shared PERFORMANCE_COLUMN_MIN As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "performanceColumnMin")

		''' <summary>
		''' Provides Access to an Output Message for Performance Column Max.
		''' </summary>
		''' <remarks></remarks>
		Public Shared PERFORMANCE_COLUMN_MAX As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "performanceColumnMax")

		''' <summary>
		''' Provides Access to an Output Message for Performance Column Average.
		''' </summary>
		''' <remarks></remarks>
		Public Shared PERFORMANCE_COLUMN_AVERAGE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "performanceColumnAverage")

		''' <summary>
		''' Provides Access to an Output Message for Performance Work Help.
		''' </summary>
		''' <remarks></remarks>
		Public Shared PERFORMANCE_WORK_HELP As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "performanceWorkHelp")

		''' <summary>
		''' Provides Access to an Output Message for Too Few Objects for Selection.
		''' </summary>
		''' <remarks></remarks>
		Public Shared QUESTION_OBJECT_TOFEWOBJECTS As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "questionSelectToFewObjects")

		''' <summary>
		''' Provides Access to an Output Message to List Selection.
		''' </summary>
		''' <remarks></remarks>
		Public Shared QUESTION_OBJECT_LISTSELECTION As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "questionListSelection")

		''' <summary>
		''' Provides Access to an Output Message.
		''' </summary>
		''' <remarks></remarks>
		Public Shared CRYPTOGRAPHY_COMMAND_RESULTS As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "cryptographyCommandResults")

		''' <summary>
		''' Provides Access to an Output Message.
		''' </summary>
		''' <remarks></remarks>
		Public Shared CRYPTOGRAPHY_CIPHER_TEXT As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "cryptographyCipherText")

		''' <summary>
		''' Provides Access to an Output Message.
		''' </summary>
		''' <remarks></remarks>
		Public Shared CRYPTOGRAPHY_PLAIN_TEXT As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "cryptographyPlainText")

		''' <summary>
		''' Provides Access to an Output Message.
		''' </summary>
		''' <remarks></remarks>
		Public Shared CRYPTOGRAPHY_HASH_TEXT As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "cryptographyHashText")

		''' <summary>
		''' Provides Access to an Output Message for the WPF Mode Prompt.
		''' </summary>
		''' <remarks></remarks>
		Public Shared WPF_MODE_PROMPT As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "promptWpfMode")

		''' <summary>
		''' Provides Access to an Output Message for the WPF Mode Exit Command.
		''' </summary>
		''' <remarks></remarks>
		Public Shared WPF_MODE_EXIT As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "exitWpfMode")

		''' <summary>
		''' Provides Access to an Output Message for the WPF Mode Title.
		''' </summary>
		''' <remarks></remarks>
		Public Shared WPF_MODE_TITLE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "titleWpfMode")

		''' <summary>
		''' Provides Access to an Output Message for the WPF Output Title.
		''' </summary>
		''' <remarks></remarks>
		Public Shared WPF_MODE_OUTPUTTITLE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "ouputTitleWpfMode")

		''' <summary>
		''' Provides Access to an Output Message for the WPF Mode Clear Command.
		''' </summary>
		''' <remarks></remarks>
		Public Shared WPF_MODE_CLEAR As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "clearWpfMode")

		''' <summary>
		''' Provides Access to an Output Message for the Question Section Title.
		''' </summary>
		''' <remarks></remarks>
		Public Shared QUESTION_SECTION_TITLE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "questionSectionTitle")

		''' <summary>
		''' Provides Access to the Title for Debug Outputs.
		''' </summary>
		''' <remarks></remarks>
		Public Shared DEBUG_TITLE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "titleDebug")

		''' <summary>
		''' Provides Access to the Title for Log/Verbose Outputs.
		''' </summary>
		''' <remarks></remarks>
		Public Shared LOG_TITLE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "titleLog")

		''' <summary>
		''' Provides Access to the Title for Error Outputs.
		''' </summary>
		''' <remarks></remarks>
		Public Shared ERROR_TITLE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "titleError")

		''' <summary>
		''' Provides Access to the Title for Information Outputs.
		''' </summary>
		''' <remarks></remarks>
		Public Shared INFORMATION_TITLE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "titleInformation")

		''' <summary>
		''' Provides Access to the Title for Performance Outputs.
		''' </summary>
		''' <remarks></remarks>
		Public Shared PERFORMANCE_TITLE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "titlePerformance")

		''' <summary>
		''' Provides Access to the Title for Question Outputs.
		''' </summary>
		''' <remarks></remarks>
		Public Shared QUESTION_TITLE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "titleQuestion")

		''' <summary>
		''' Provides Access to the Title for Success Outputs.
		''' </summary>
		''' <remarks></remarks>
		Public Shared SUCCESS_TITLE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "titleSuccess")

		''' <summary>
		''' Provides Access to the Title for Warning Outputs.
		''' </summary>
		''' <remarks></remarks>
		Public Shared WARNING_TITLE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "titleWarning")

		''' <summary>
		''' Provides Access to the Interactive Question.
		''' </summary>
		''' <remarks></remarks>
		Public Shared INTERACTIVE_QUESTION As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "promptInteractiveQuestion")

		''' <summary>
		''' Provides Access to the Interactive Affirmative.
		''' </summary>
		''' <remarks></remarks>
		Public Shared INTERACTIVE_AFFIRMATIVE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "promptInteractiveAffirmative")

		''' <summary>
		''' Provides Access to the Interactive Affirmative.
		''' </summary>
		''' <remarks></remarks>
		Public Shared INTERACTIVE_NEGATIVE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "promptInteractiveNegative")

		''' <summary>
		''' Provides Access to the Interactive Affirmative.
		''' </summary>
		''' <remarks></remarks>
		Public Shared INTERACTIVE_CHANGE_CONFIRMATION As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "promptInteractiveChangeConfirmation")

		''' <summary>
		''' Provides Access to the Interactive List Selection.
		''' </summary>
		''' <remarks></remarks>
		Public Shared INTERACTIVE_LIST_SELECTION As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "promptInteractiveListSelection")

		''' <summary>
		''' Provides Access to the Character Output Width.
		''' </summary>
		''' <remarks></remarks>
		Public Shared CHARACTER_OUTPUT_WIDTH As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "characterWidthWpfMode")

		''' <summary>
		''' Provides Access to the Status Waiting.
		''' </summary>
		''' <remarks></remarks>
		Public Shared STATUS_WAITING As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "statusWaitingWpfMode")

		''' <summary>
		''' Provides Access to the Status Working.
		''' </summary>
		''' <remarks></remarks>
		Public Shared STATUS_WORKING_SINGULAR As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "statusWorkingSingularWpfMode")

		''' <summary>
		''' Provides Access to the Status Working.
		''' </summary>
		''' <remarks></remarks>
		Public Shared STATUS_WORKING_MULTIPLE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "statusWorkingMultipleWpfMode")

		''' <summary>
		''' Provides Access to the Status Working.
		''' </summary>
		''' <remarks></remarks>
		Public Shared STATUS_PENDING As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "statusPendingWpfMode")

		''' <summary>
		''' Provides Access to the Suggestions Settings.
		''' </summary>
		''' <remarks></remarks>
		Public Shared SETTINGS_SUGGESTIONS As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "settingsWpfSuggestions")

		''' <summary>
		''' Provides Access to the Suggestions Settings.
		''' </summary>
		''' <remarks></remarks>
		Public Shared SETTINGS_RELOAD As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "settingsWpfReload")

		''' <summary>
		''' Provides Access to the Status Working.
		''' </summary>
		''' <remarks></remarks>
		Public Shared COMMAND_LAST_RUN As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "commandWpfLastRun")

		''' <summary>
		''' Provides Access to the Cache Settings.
		''' </summary>
		''' <remarks></remarks>
		Public Shared SETTINGS_CACHE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "settingsWpfCache")

		''' <summary>
		''' Provides Access to the Cache Settings.
		''' </summary>
		''' <remarks></remarks>
		Public Shared SETTINGS_CACHES As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "settingsWpfCaches")


		''' <summary>
		''' Provides Access to the Cache Settings.
		''' </summary>
		''' <remarks></remarks>
		Public Shared SETTINGS_CACHELIST_NULL As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "settingsWpfCacheListNull")

		''' <summary>
		''' Provides Access to the Cache Settings.
		''' </summary>
		''' <remarks></remarks>
		Public Shared SETTINGS_CACHELIST_VALUE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "settingsWpfCacheListValue")

		''' <summary>
		''' Provides Access to an Exception Message.
		''' </summary>
		''' <remarks></remarks>
		Public Shared EXCEPTION_SERIALISER_NOFIELD As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_EXCEPTION, "serialiserExceptionNoField")

		''' <summary>
		''' Provides Access to an Exception Message.
		''' </summary>
		''' <remarks></remarks>
		Public Shared EXCEPTION_SERIALISER_WRONGTYPENAME As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_EXCEPTION, "serialiserExceptionWrongTypeName")

		''' <summary>
		''' Provides Access to an Exception Message.
		''' </summary>
		''' <remarks></remarks>
		Public Shared EXCEPTION_SERIALISER_TYPECREATION As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_EXCEPTION, "serialiserExceptionTypeCreation")

		''' <summary>
		''' Provides Access to an Exception Message.
		''' </summary>
		''' <remarks></remarks>
		Public Shared EXCEPTION_SERIALISER_ITERATEFORMAT As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_EXCEPTION, "serialiserExceptionIterateFormat")

		''' <summary>
		''' Provides Access to an Exception Message.
		''' </summary>
		''' <remarks></remarks>
		Public Shared EXCEPTION_SERIALISER_LOADFROM As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_EXCEPTION, "serialiserExceptionLoadFrom")

		''' <summary>
		''' Provides Access to an Exception Message.
		''' </summary>
		''' <remarks></remarks>
		Public Shared EXCEPTION_SERIALISER_INCOMPATIBLETYPE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_EXCEPTION, "serialiserExceptionIncompatibleType")

		''' <summary>
		''' Provides Access to an Exception Message.
		''' </summary>
		''' <remarks></remarks>
		Public Shared EXCEPTION_SERIALISER_NONCONFIGURABLETYPE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_EXCEPTION, "serialiserExceptionNonConfigurableType")

		''' <summary>
		''' Provides Access to an Command Loading Message.
		''' </summary>
		''' <remarks></remarks>
		Public Shared COMMAND_LOADING_ANALYSING As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "commandLoadingAnalysing")

		''' <summary>
		''' Provides Access to an Command Loading Message.
		''' </summary>
		''' <remarks></remarks>
		Public Shared COMMAND_LOADING_IGNORE As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "commandLoadingIgnore")

					''' <summary>
		''' Provides Access to an Command Loading Message.
		''' </summary>
		''' <remarks></remarks>
		Public Shared COMMAND_LOADING_FAILED As String = _
			SingleResource(ResType, RESOURCEMANAGER_NAME_OUTPUT, "commandLoadingFailed")

	#End Region

	#Region " Public Shared Read-Only Properties "

		''' <summary>
		''' Provides Access to a ResourceManager for a particular Resource contained within a 
		''' Particular Assembly.
		''' </summary>
		''' <param name="containingAssembly">The Assembly containing the Resources.</param>
		''' <param name="resourcesName">The Name of the Resources.</param>
		''' <value></value>
		''' <returns>A Resource Manager.</returns>
		''' <remarks></remarks>
		Public Shared ReadOnly Property Resources( _
			ByVal containingAssembly As Assembly, _
			ByVal resourcesName As String _
		) As ResourceManager

			Get

				Dim cache As Simple = Simple.GetInstance(GetType(Resources).GetHashCode)

				Dim manager As ResourceManager = Nothing

				If Not cache.TryGet(manager, PROPERTY_RESOURCES.GetHashCode, _
					containingAssembly.GetHashCode, resourcesName.GetHashCode) Then

					Dim allResources As String() = containingAssembly.GetManifestResourceNames()

					If Not allResources Is Nothing AndAlso allResources.Length > 0 Then

						For i As Integer = 0 To allResources.Length - 1

							If allResources(i).Contains(FULL_STOP & resourcesName & FULL_STOP) Then

								manager = New ResourceManager( _
									allResources(i).Substring(0, allResources(i). _
									IndexOf(FULL_STOP & resourcesName & FULL_STOP)) & _
									FULL_STOP & resourcesName, containingAssembly)

								Exit For

							End If

						Next

					End If

					cache.Set(manager, PROPERTY_RESOURCES.GetHashCode, _
						containingAssembly.GetHashCode, resourcesName.GetHashCode)

				End If

				Return manager

			End Get

		End Property

		''' <summary>
		''' Provides Access to a ResourceManager for a particular Resource contained within a
		''' Particular Type.
		''' </summary>
		''' <param name="containingType">The Type containing the Resources.</param>
		''' <param name="resourcesName">The Name of the Resources.</param>
		''' <value></value>
		''' <returns>A Resource Manager.</returns>
		''' <remarks></remarks>
		Public Shared ReadOnly Property Resources( _
			ByVal containingType As System.Type, _
			ByVal resourcesName As String _
		) As ResourceManager

			Get

				Return Resources(containingType.Assembly, resourcesName)

			End Get

		End Property

		''' <summary>
		''' Provides Access to a Single Resource String for a Particular Assembly, Resource Name and Key.
		''' </summary>
		''' <param name="containingAssembly">The Assembly containing the Resources.</param>
		''' <param name="resourcesName">The Name of the Resources.</param>
		''' <param name="resourceKey">The Key of the Resource String.</param>
		''' <value></value>
		''' <returns>A String.</returns>
		''' <remarks></remarks>
		Public Shared ReadOnly Property SingleResource( _
			ByVal containingAssembly As System.Reflection.Assembly, _
			ByVal resourcesName As String, _
			ByVal resourceKey As String _
		) As String

			Get

				Return Resources(containingAssembly, resourcesName).GetString(resourceKey, System.Globalization.CultureInfo.CurrentUICulture)

			End Get

		End Property

		''' <summary>
		''' Provides Access to a Single Resource String for a Particular Assembly, Resource Name and Key.
		''' </summary>
		''' <param name="containingType">The Type containing the Resources.</param>
		''' <param name="resourcesName">The Name of the Resources.</param>
		''' <param name="resourceKey">The Key of the Resource String.</param>
		''' <value></value>
		''' <returns>A String.</returns>
		''' <remarks></remarks>
		Public Shared ReadOnly Property SingleResource( _
			ByVal containingType As System.Type, _
			ByVal resourcesName As String, _
			ByVal resourceKey As String _
		) As String

			Get

				Return SingleResource(containingType.Assembly, resourcesName, resourceKey)

			End Get

		End Property

	#End Region

End Class
