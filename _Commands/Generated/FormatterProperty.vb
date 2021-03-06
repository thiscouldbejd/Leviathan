Namespace Commands

	''' <summary>Property used in Formatting.</summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:47:35</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Commands\Generated\FormatterProperty.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Commands\Generated\FormatterProperty.tt", "1")> _
	Partial Public Class FormatterProperty
		Inherits Leviathan.Inspection.MemberAnalyser

		#Region " Public Constructors "

			''' <summary>Default Constructor</summary>
			Public Sub New()

				MyBase.New()

			End Sub

			''' <summary>Parametered Constructor (1 Parameters)</summary>
			Public Sub New( _
				ByVal _Length As System.Int32 _
			)

				MyBase.New()

				Length = _Length

			End Sub

			''' <summary>Parametered Constructor (2 Parameters)</summary>
			Public Sub New( _
				ByVal _Length As System.Int32, _
				ByVal _InternalDisplayName As System.String _
			)

				MyBase.New()

				Length = _Length
				InternalDisplayName = _InternalDisplayName

			End Sub

			''' <summary>Parametered Constructor (3 Parameters)</summary>
			Public Sub New( _
				ByVal _Length As System.Int32, _
				ByVal _InternalDisplayName As System.String, _
				ByVal _Index As System.Object _
			)

				MyBase.New()

				Length = _Length
				InternalDisplayName = _InternalDisplayName
				Index = _Index

			End Sub

			''' <summary>Parametered Constructor (4 Parameters)</summary>
			Public Sub New( _
				ByVal _Length As System.Int32, _
				ByVal _InternalDisplayName As System.String, _
				ByVal _Index As System.Object, _
				ByVal _Highlights As System.Collections.Generic.List(Of HighlightedCondition) _
			)

				MyBase.New()

				Length = _Length
				InternalDisplayName = _InternalDisplayName
				Index = _Index
				Highlights = _Highlights

			End Sub

		#End Region

		#Region " Public Constants "

			''' <summary>Public Shared Reference to the Name of the Property: Length</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_LENGTH As String = "Length"

			''' <summary>Public Shared Reference to the Name of the Property: InternalDisplayName</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_INTERNALDISPLAYNAME As String = "InternalDisplayName"

			''' <summary>Public Shared Reference to the Name of the Property: Index</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_INDEX As String = "Index"

			''' <summary>Public Shared Reference to the Name of the Property: Highlights</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_HIGHLIGHTS As String = "Highlights"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: Length</summary>
			''' <remarks></remarks>
			Private m_Length As System.Int32

			''' <summary>Private Data Storage Variable for Property: InternalDisplayName</summary>
			''' <remarks></remarks>
			Private m_InternalDisplayName As System.String

			''' <summary>Private Data Storage Variable for Property: Index</summary>
			''' <remarks></remarks>
			Private m_Index As System.Object

			''' <summary>Private Data Storage Variable for Property: Highlights</summary>
			''' <remarks></remarks>
			Private m_Highlights As System.Collections.Generic.List(Of HighlightedCondition)

		#End Region

		#Region " Public Properties "

			''' <summary>Provides Access to the Property: Length</summary>
			''' <remarks></remarks>
			Public Property Length() As System.Int32
				Get
					Return m_Length
				End Get
				Set(value As System.Int32)
					m_Length = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: InternalDisplayName</summary>
			''' <remarks></remarks>
			Public Property InternalDisplayName() As System.String
				Get
					Return m_InternalDisplayName
				End Get
				Set(value As System.String)
					m_InternalDisplayName = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: Index</summary>
			''' <remarks></remarks>
			Public Property Index() As System.Object
				Get
					Return m_Index
				End Get
				Set(value As System.Object)
					m_Index = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: Highlights</summary>
			''' <remarks></remarks>
			Public Property Highlights() As System.Collections.Generic.List(Of HighlightedCondition)
				Get
					Return m_Highlights
				End Get
				Set(value As System.Collections.Generic.List(Of HighlightedCondition))
					m_Highlights = value
				End Set
			End Property

		#End Region

	End Class

End Namespace