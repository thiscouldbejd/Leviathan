Namespace Commands

	''' <summary></summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:43:24</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Commands\Commands\Generated\ObjectsCommands.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Commands\Commands\Generated\ObjectsCommands.tt", "1")> _
	<Leviathan.Commands.Command(ResourceContainingType:=GetType(ObjectsCommands), ResourceName:="CommandDetails", Name:="objects", Description:="@commandObjectsDescription@", Hidden:=False)> _
	Partial Public Class ObjectsCommands
		Inherits System.Object

		#Region " Public Constructors "

			''' <summary>Parametered Constructor (1 Parameters)</summary>
			Public Sub New( _
				ByVal _Host As Leviathan.Commands.ICommandsExecution _
			)

				MyBase.New()

				Host = _Host

				m_FieldsToSortAscending = New Leviathan.Inspection.MemberAnalyser() {}
				m_FieldsToSortDescending = New Leviathan.Inspection.MemberAnalyser() {}
			End Sub

			''' <summary>Parametered Constructor (2 Parameters)</summary>
			Public Sub New( _
				ByVal _Host As Leviathan.Commands.ICommandsExecution, _
				ByVal _FieldsToSortAscending As Leviathan.Inspection.MemberAnalyser() _
			)

				MyBase.New()

				Host = _Host
				FieldsToSortAscending = _FieldsToSortAscending

				m_FieldsToSortDescending = New Leviathan.Inspection.MemberAnalyser() {}
			End Sub

			''' <summary>Parametered Constructor (3 Parameters)</summary>
			Public Sub New( _
				ByVal _Host As Leviathan.Commands.ICommandsExecution, _
				ByVal _FieldsToSortAscending As Leviathan.Inspection.MemberAnalyser(), _
				ByVal _FieldsToSortDescending As Leviathan.Inspection.MemberAnalyser() _
			)

				MyBase.New()

				Host = _Host
				FieldsToSortAscending = _FieldsToSortAscending
				FieldsToSortDescending = _FieldsToSortDescending

			End Sub

			''' <summary>Parametered Constructor (4 Parameters)</summary>
			Public Sub New( _
				ByVal _Host As Leviathan.Commands.ICommandsExecution, _
				ByVal _FieldsToSortAscending As Leviathan.Inspection.MemberAnalyser(), _
				ByVal _FieldsToSortDescending As Leviathan.Inspection.MemberAnalyser(), _
				ByVal _FieldsMarkingDuplicates As System.String() _
			)

				MyBase.New()

				Host = _Host
				FieldsToSortAscending = _FieldsToSortAscending
				FieldsToSortDescending = _FieldsToSortDescending
				FieldsMarkingDuplicates = _FieldsMarkingDuplicates

			End Sub

		#End Region

		#Region " Public Constants "

			''' <summary>Public Shared Reference to the Name of the Property: Host</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_HOST As String = "Host"

			''' <summary>Public Shared Reference to the Name of the Property: FieldsToSortAscending</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_FIELDSTOSORTASCENDING As String = "FieldsToSortAscending"

			''' <summary>Public Shared Reference to the Name of the Property: FieldsToSortDescending</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_FIELDSTOSORTDESCENDING As String = "FieldsToSortDescending"

			''' <summary>Public Shared Reference to the Name of the Property: FieldsMarkingDuplicates</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_FIELDSMARKINGDUPLICATES As String = "FieldsMarkingDuplicates"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: Host</summary>
			''' <remarks></remarks>
			Private m_Host As Leviathan.Commands.ICommandsExecution

			''' <summary>Private Data Storage Variable for Property: FieldsToSortAscending</summary>
			''' <remarks></remarks>
			Private m_FieldsToSortAscending As Leviathan.Inspection.MemberAnalyser()

			''' <summary>Private Data Storage Variable for Property: FieldsToSortDescending</summary>
			''' <remarks></remarks>
			Private m_FieldsToSortDescending As Leviathan.Inspection.MemberAnalyser()

			''' <summary>Private Data Storage Variable for Property: FieldsMarkingDuplicates</summary>
			''' <remarks></remarks>
			Private m_FieldsMarkingDuplicates As System.String()

		#End Region

		#Region " Public Properties "

			''' <summary>Provides Access to the Property: Host</summary>
			''' <remarks></remarks>
			Public Property Host() As Leviathan.Commands.ICommandsExecution
				Get
					Return m_Host
				End Get
				Set(value As Leviathan.Commands.ICommandsExecution)
					m_Host = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: FieldsToSortAscending</summary>
			''' <remarks></remarks>
			<Leviathan.Configuration.Configurable("$", ResourceContainingType:=GetType(ObjectsCommands), ResourceName:="CommandDetails", Description:="@commandSortedDescriptionAscending@", ArgsDescription:="@commandFormatterParameterDescriptionProperties@", Prefix:="")> _
			Public Property FieldsToSortAscending() As Leviathan.Inspection.MemberAnalyser()
				Get
					Return m_FieldsToSortAscending
				End Get
				Set(value As Leviathan.Inspection.MemberAnalyser())
					m_FieldsToSortAscending = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: FieldsToSortDescending</summary>
			''' <remarks></remarks>
			<Leviathan.Configuration.Configurable("$$", ResourceContainingType:=GetType(ObjectsCommands), ResourceName:="CommandDetails", Description:="@commandSortedDescriptionDescending@", ArgsDescription:="@commandFormatterParameterDescriptionProperties@", Prefix:="")> _
			Public Property FieldsToSortDescending() As Leviathan.Inspection.MemberAnalyser()
				Get
					Return m_FieldsToSortDescending
				End Get
				Set(value As Leviathan.Inspection.MemberAnalyser())
					m_FieldsToSortDescending = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: FieldsMarkingDuplicates</summary>
			''' <remarks></remarks>
			<Leviathan.Configuration.Configurable("~", ResourceContainingType:=GetType(ObjectsCommands), ResourceName:="CommandDetails", Description:="@commandObjectsParameterDescriptionProperties@", ArgsDescription:="@commandObjectsParameterDescriptionDuplicateFields@", Prefix:="")> _
			Public Property FieldsMarkingDuplicates() As System.String()
				Get
					Return m_FieldsMarkingDuplicates
				End Get
				Set(value As System.String())
					m_FieldsMarkingDuplicates = value
				End Set
			End Property

		#End Region

	End Class

End Namespace