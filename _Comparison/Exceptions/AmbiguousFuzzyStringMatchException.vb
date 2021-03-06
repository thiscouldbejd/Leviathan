Namespace Comparison

	''' <summary>Exception which should be thrown when a match request is Ambiguous.</summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:48:51</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Comparison\Exceptions\AmbiguousFuzzyStringMatchException.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Comparison\Exceptions\AmbiguousFuzzyStringMatchException.tt", "1")> _
	Partial Public Class AmbiguousFuzzyStringMatchException
		Inherits Exception

		#Region " Public Constructors "

			''' <summary>Default Constructor</summary>
			Public Sub New()

				MyBase.New()

			End Sub

			''' <summary>Parametered Constructor (1 Parameters)</summary>
			Public Sub New( _
				ByVal _AmbiguousName As System.String _
			)

				MyBase.New()

				AmbiguousName = _AmbiguousName

			End Sub

			''' <summary>Parametered Constructor (2 Parameters)</summary>
			Public Sub New( _
				ByVal _AmbiguousName As System.String, _
				ByVal _Matches As FuzzyMatch() _
			)

				MyBase.New()

				AmbiguousName = _AmbiguousName
				Matches = _Matches

			End Sub

		#End Region

		#Region " Public Constants "

			''' <summary>Public Shared Reference to the Name of the Property: AmbiguousName</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_AMBIGUOUSNAME As String = "AmbiguousName"

			''' <summary>Public Shared Reference to the Name of the Property: Matches</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_MATCHES As String = "Matches"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: AmbiguousName</summary>
			''' <remarks></remarks>
			Private m_AmbiguousName As System.String

			''' <summary>Private Data Storage Variable for Property: Matches</summary>
			''' <remarks></remarks>
			Private m_Matches As FuzzyMatch()

		#End Region

		#Region " Public Properties "

			''' <summary>Provides Access to the Property: AmbiguousName</summary>
			''' <remarks></remarks>
			Public Property AmbiguousName() As System.String
				Get
					Return m_AmbiguousName
				End Get
				Set(value As System.String)
					m_AmbiguousName = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: Matches</summary>
			''' <remarks></remarks>
			Public Property Matches() As FuzzyMatch()
				Get
					Return m_Matches
				End Get
				Set(value As FuzzyMatch())
					m_Matches = value
				End Set
			End Property

		#End Region

	End Class

End Namespace