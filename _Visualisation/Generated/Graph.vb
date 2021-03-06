Namespace Visualisation

	''' <summary></summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:53:58</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Visualisation\Generated\Graph.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Visualisation\Generated\Graph.tt", "1")> _
	Partial Public Class Graph(Of T)
		Inherits System.Object

		#Region " Public Constructors "

			''' <summary>Default Constructor</summary>
			Public Sub New()

				MyBase.New()

				m_Created = DateTime.Now()
				m_CreatedBy = "Leviathan"
			End Sub

			''' <summary>Parametered Constructor (1 Parameters)</summary>
			Public Sub New( _
				ByVal _Name As System.String _
			)

				MyBase.New()

				Name = _Name

				m_Created = DateTime.Now()
				m_CreatedBy = "Leviathan"
			End Sub

			''' <summary>Parametered Constructor (2 Parameters)</summary>
			Public Sub New( _
				ByVal _Name As System.String, _
				ByVal _Created As System.DateTime _
			)

				MyBase.New()

				Name = _Name
				Created = _Created

				m_CreatedBy = "Leviathan"
			End Sub

			''' <summary>Parametered Constructor (3 Parameters)</summary>
			Public Sub New( _
				ByVal _Name As System.String, _
				ByVal _Created As System.DateTime, _
				ByVal _CreatedBy As System.String _
			)

				MyBase.New()

				Name = _Name
				Created = _Created
				CreatedBy = _CreatedBy

			End Sub

		#End Region

		#Region " Public Constants "

			''' <summary>Public Shared Reference to the Name of the Property: Name</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_NAME As String = "Name"

			''' <summary>Public Shared Reference to the Name of the Property: Created</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_CREATED As String = "Created"

			''' <summary>Public Shared Reference to the Name of the Property: CreatedBy</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_CREATEDBY As String = "CreatedBy"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: Name</summary>
			''' <remarks></remarks>
			Private m_Name As System.String

			''' <summary>Private Data Storage Variable for Property: Created</summary>
			''' <remarks></remarks>
			Private m_Created As System.DateTime

			''' <summary>Private Data Storage Variable for Property: CreatedBy</summary>
			''' <remarks></remarks>
			Private m_CreatedBy As System.String

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

			''' <summary>Provides Access to the Property: Created</summary>
			''' <remarks></remarks>
			Public Property Created() As System.DateTime
				Get
					Return m_Created
				End Get
				Set(value As System.DateTime)
					m_Created = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: CreatedBy</summary>
			''' <remarks></remarks>
			Public Property CreatedBy() As System.String
				Get
					Return m_CreatedBy
				End Get
				Set(value As System.String)
					m_CreatedBy = value
				End Set
			End Property

		#End Region

	End Class

End Namespace