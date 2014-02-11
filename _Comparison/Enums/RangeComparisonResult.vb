Namespace Comparison

	''' <summary>Enum representing Date Comparison Results.</summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:48:50</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Comparison\Enums\RangeComparisonResult.tt</generator-source>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Comparison\Enums\RangeComparisonResult.tt", "1")> _
	<Flags()> _
	Public Enum RangeComparisonResult As System.Int32

		''' <summary>NO_OVERLAP</summary>
		NO_OVERLAP = 1

		''' <summary>END_OVERLAP</summary>
		END_OVERLAP = 2

		''' <summary>START_OVERLAP</summary>
		START_OVERLAP = 4

		''' <summary>FULL_OVERLAP</summary>
		FULL_OVERLAP = 8

		''' <summary>FULL_ENCLOSURE</summary>
		FULL_ENCLOSURE = 16

		''' <summary>SAME</summary>
		SAME = 32

		''' <summary>CLEAR</summary>
		CLEAR = 1

		''' <summary>NOTCLEAR</summary>
		NOTCLEAR = 62

		''' <summary>All Flags</summary>
		All = NO_OVERLAP Or _
			END_OVERLAP Or _
			START_OVERLAP Or _
			FULL_OVERLAP Or _
			FULL_ENCLOSURE Or _
			SAME Or _
			CLEAR Or _
			NOTCLEAR

	End Enum

End Namespace