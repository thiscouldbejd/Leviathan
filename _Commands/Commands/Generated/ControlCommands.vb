Namespace Commands

	''' <summary></summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:42:19</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Commands\Commands\Generated\ControlCommands.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Commands\Commands\Generated\ControlCommands.tt", "1")> _
	<Leviathan.Commands.Command(ResourceContainingType:=GetType(ControlCommands), ResourceName:="CommandDetails", Name:="control", Description:="@commandControlDescription@", Hidden:=False)> _
	Partial Public Class ControlCommands
		Inherits System.Object

		#Region " Public Constructors "

			''' <summary>Parametered Constructor (1 Parameters)</summary>
			Public Sub New( _
				ByVal _Host As Leviathan.Commands.ICommandsExecution _
			)

				MyBase.New()

				Host = _Host

				m_ParameterRegex = "(?<=^\!)[A-z0-9_*].*(?=\!$)"
				m_Cache = Leviathan.Caching.Simple.GetInstance(Me.GetType().GetHashCode(), True)
			End Sub

			''' <summary>Parametered Constructor (2 Parameters)</summary>
			Public Sub New( _
				ByVal _Host As Leviathan.Commands.ICommandsExecution, _
				ByVal _ParameterRegex As System.String _
			)

				MyBase.New()

				Host = _Host
				ParameterRegex = _ParameterRegex

				m_Cache = Leviathan.Caching.Simple.GetInstance(Me.GetType().GetHashCode(), True)
			End Sub

			''' <summary>Parametered Constructor (3 Parameters)</summary>
			Public Sub New( _
				ByVal _Host As Leviathan.Commands.ICommandsExecution, _
				ByVal _ParameterRegex As System.String, _
				ByVal _IterationSource As System.Object _
			)

				MyBase.New()

				Host = _Host
				ParameterRegex = _ParameterRegex
				IterationSource = _IterationSource

				m_Cache = Leviathan.Caching.Simple.GetInstance(Me.GetType().GetHashCode(), True)
			End Sub

			''' <summary>Parametered Constructor (4 Parameters)</summary>
			Public Sub New( _
				ByVal _Host As Leviathan.Commands.ICommandsExecution, _
				ByVal _ParameterRegex As System.String, _
				ByVal _IterationSource As System.Object, _
				ByVal _Cache As Leviathan.Caching.Simple _
			)

				MyBase.New()

				Host = _Host
				ParameterRegex = _ParameterRegex
				IterationSource = _IterationSource
				Cache = _Cache

			End Sub

		#End Region

		#Region " Public Constants "

			''' <summary>Public Shared Reference to the Name of the Property: Host</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_HOST As String = "Host"

			''' <summary>Public Shared Reference to the Name of the Property: ParameterRegex</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_PARAMETERREGEX As String = "ParameterRegex"

			''' <summary>Public Shared Reference to the Name of the Property: IterationSource</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_ITERATIONSOURCE As String = "IterationSource"

			''' <summary>Public Shared Reference to the Name of the Property: Cache</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_CACHE As String = "Cache"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: Host</summary>
			''' <remarks></remarks>
			Private m_Host As Leviathan.Commands.ICommandsExecution

			''' <summary>Private Data Storage Variable for Property: ParameterRegex</summary>
			''' <remarks></remarks>
			Private m_ParameterRegex As System.String

			''' <summary>Private Data Storage Variable for Property: IterationSource</summary>
			''' <remarks></remarks>
			Private m_IterationSource As System.Object

			''' <summary>Private Data Storage Variable for Property: Cache</summary>
			''' <remarks></remarks>
			Private m_Cache As Leviathan.Caching.Simple

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

			''' <summary>Provides Access to the Property: ParameterRegex</summary>
			''' <remarks></remarks>
			Public Property ParameterRegex() As System.String
				Get
					Return m_ParameterRegex
				End Get
				Set(value As System.String)
					m_ParameterRegex = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: IterationSource</summary>
			''' <remarks></remarks>
			Public Property IterationSource() As System.Object
				Get
					Return m_IterationSource
				End Get
				Set(value As System.Object)
					m_IterationSource = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: Cache</summary>
			''' <remarks></remarks>
			Public Property Cache() As Leviathan.Caching.Simple
				Get
					Return m_Cache
				End Get
				Set(value As Leviathan.Caching.Simple)
					m_Cache = value
				End Set
			End Property

		#End Region

	End Class

End Namespace