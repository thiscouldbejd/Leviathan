Namespace Files

	''' <summary>Class Representing an Directory.</summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:50:47</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Files\Generated\DirectoryRepresentation.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Files\Generated\DirectoryRepresentation.tt", "1")> _
	Partial Public Class DirectoryRepresentation
		Inherits System.Object

		#Region " Public Constructors "

			''' <summary>Default Constructor</summary>
			Public Sub New()

				MyBase.New()

			End Sub

			''' <summary>Parametered Constructor (1 Parameters)</summary>
			Public Sub New( _
				ByVal _Name As System.String _
			)

				MyBase.New()

				Name = _Name

			End Sub

			''' <summary>Parametered Constructor (2 Parameters)</summary>
			Public Sub New( _
				ByVal _Name As System.String, _
				ByVal _Rules As AccessRuleRepresentation() _
			)

				MyBase.New()

				Name = _Name
				Rules = _Rules

			End Sub

			''' <summary>Parametered Constructor (3 Parameters)</summary>
			Public Sub New( _
				ByVal _Name As System.String, _
				ByVal _Rules As AccessRuleRepresentation(), _
				ByVal _Children As DirectoryRepresentation() _
			)

				MyBase.New()

				Name = _Name
				Rules = _Rules
				Children = _Children

			End Sub

		#End Region

		#Region " Public Constants "

			''' <summary>Public Shared Reference to the Name of the Property: Name</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_NAME As String = "Name"

			''' <summary>Public Shared Reference to the Name of the Property: Rules</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_RULES As String = "Rules"

			''' <summary>Public Shared Reference to the Name of the Property: Children</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_CHILDREN As String = "Children"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: Name</summary>
			''' <remarks></remarks>
			Private m_Name As System.String

			''' <summary>Private Data Storage Variable for Property: Rules</summary>
			''' <remarks></remarks>
			Private m_Rules As AccessRuleRepresentation()

			''' <summary>Private Data Storage Variable for Property: Children</summary>
			''' <remarks></remarks>
			Private m_Children As DirectoryRepresentation()

		#End Region

		#Region " Public Properties "

			''' <summary>Provides Access to the Property: Name</summary>
			''' <remarks></remarks>
			Public Property Name() As System.String
				Get
					Return m_Name
				End Get
				Set(value As System.String)
					m_Name = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: Rules</summary>
			''' <remarks></remarks>
			Public Property Rules() As AccessRuleRepresentation()
				Get
					Return m_Rules
				End Get
				Set(value As AccessRuleRepresentation())
					m_Rules = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: Children</summary>
			''' <remarks></remarks>
			Public Property Children() As DirectoryRepresentation()
				Get
					Return m_Children
				End Get
				Set(value As DirectoryRepresentation())
					m_Children = value
				End Set
			End Property

		#End Region

	End Class

End Namespace