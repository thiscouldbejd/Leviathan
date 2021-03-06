Namespace Commands

	''' <summary></summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:43:45</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Commands\Generated\CommandConsoleOutput.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Commands\Generated\CommandConsoleOutput.tt", "1")> _
	Partial Public Class CommandConsoleOutput
		Inherits System.Object

		#Region " Public Constructors "

			''' <summary>Default Constructor</summary>
			Public Sub New()

				MyBase.New()

				PostConstructorCall()

			End Sub

			''' <summary>Parametered Constructor (1 Parameters)</summary>
			Public Sub New( _
				ByVal _Create As System.Boolean _
			)

				MyBase.New()

				Create = _Create

				PostConstructorCall()

			End Sub

			''' <summary>Parametered Constructor (2 Parameters)</summary>
			Public Sub New( _
				ByVal _Create As System.Boolean, _
				ByVal _CharacterWidth As System.Int32 _
			)

				MyBase.New()

				Create = _Create
				CharacterWidth = _CharacterWidth

				PostConstructorCall()

			End Sub

			''' <summary>Parametered Constructor (3 Parameters)</summary>
			Public Sub New( _
				ByVal _Create As System.Boolean, _
				ByVal _CharacterWidth As System.Int32, _
				ByVal _Title As System.String _
			)

				MyBase.New()

				Create = _Create
				CharacterWidth = _CharacterWidth
				Title = _Title

				PostConstructorCall()

			End Sub

			''' <summary>Parametered Constructor (4 Parameters)</summary>
			Public Sub New( _
				ByVal _Create As System.Boolean, _
				ByVal _CharacterWidth As System.Int32, _
				ByVal _Title As System.String, _
				ByVal _Active As System.Boolean _
			)

				MyBase.New()

				Create = _Create
				CharacterWidth = _CharacterWidth
				Title = _Title
				Active = _Active

				PostConstructorCall()

			End Sub

			''' <summary>Parametered Constructor (5 Parameters)</summary>
			Public Sub New( _
				ByVal _Create As System.Boolean, _
				ByVal _CharacterWidth As System.Int32, _
				ByVal _Title As System.String, _
				ByVal _Active As System.Boolean, _
				ByVal _HasWritten As System.Boolean _
			)

				MyBase.New()

				Create = _Create
				CharacterWidth = _CharacterWidth
				Title = _Title
				Active = _Active
				HasWritten = _HasWritten

				PostConstructorCall()

			End Sub

		#End Region

		#Region " Public Constants "

			''' <summary>Public Shared Reference to the Name of the Property: Create</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_CREATE As String = "Create"

			''' <summary>Public Shared Reference to the Name of the Property: CharacterWidth</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_CHARACTERWIDTH As String = "CharacterWidth"

			''' <summary>Public Shared Reference to the Name of the Property: Title</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_TITLE As String = "Title"

			''' <summary>Public Shared Reference to the Name of the Property: Active</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_ACTIVE As String = "Active"

			''' <summary>Public Shared Reference to the Name of the Property: HasWritten</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_HASWRITTEN As String = "HasWritten"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: Create</summary>
			''' <remarks></remarks>
			Private m_Create As System.Boolean

			''' <summary>Private Data Storage Variable for Property: CharacterWidth</summary>
			''' <remarks></remarks>
			Private m_CharacterWidth As System.Int32

			''' <summary>Private Data Storage Variable for Property: Title</summary>
			''' <remarks></remarks>
			Private m_Title As System.String

			''' <summary>Private Data Storage Variable for Property: Active</summary>
			''' <remarks></remarks>
			Private m_Active As System.Boolean

			''' <summary>Private Data Storage Variable for Property: HasWritten</summary>
			''' <remarks></remarks>
			Private m_HasWritten As System.Boolean

		#End Region

		#Region " Public Properties "

			''' <summary>Provides Access to the Property: Create</summary>
			''' <remarks></remarks>
			Public Property Create() As System.Boolean
				Get
					Return m_Create
				End Get
				Set(value As System.Boolean)
					m_Create = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: CharacterWidth</summary>
			''' <remarks></remarks>
			Public Property CharacterWidth() As System.Int32
				Get
					Return m_CharacterWidth
				End Get
				Set(value As System.Int32)
					m_CharacterWidth = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: Title</summary>
			''' <remarks></remarks>
			Public Property Title() As System.String
				Get
					Return m_Title
				End Get
				Set(value As System.String)
					m_Title = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: Active</summary>
			''' <remarks></remarks>
			Public Property Active() As System.Boolean
				Get
					Return m_Active
				End Get
				Set(value As System.Boolean)
					m_Active = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: HasWritten</summary>
			''' <remarks></remarks>
			Public Property HasWritten() As System.Boolean
				Get
					Return m_HasWritten
				End Get
				Set(value As System.Boolean)
					m_HasWritten = value
				End Set
			End Property

		#End Region

	End Class

End Namespace