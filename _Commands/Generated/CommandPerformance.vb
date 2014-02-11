Namespace Commands

	''' <summary>Command Performance Logging</summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:46:57</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Commands\Generated\CommandPerformance.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Commands\Generated\CommandPerformance.tt", "1")> _
	Partial Public Class CommandPerformance
		Inherits System.Object

		#Region " Public Constructors "

			''' <summary>Default Constructor</summary>
			Public Sub New()

				MyBase.New()

				m_Start = New CommandPerformanceSnapshot
				m_Events = New System.Collections.Generic.List(Of CommandPerformanceSnapshot)
			End Sub

			''' <summary>Parametered Constructor (1 Parameters)</summary>
			Public Sub New( _
				ByVal _Start As CommandPerformanceSnapshot _
			)

				MyBase.New()

				Start = _Start

				m_Events = New System.Collections.Generic.List(Of CommandPerformanceSnapshot)
			End Sub

			''' <summary>Parametered Constructor (2 Parameters)</summary>
			Public Sub New( _
				ByVal _Start As CommandPerformanceSnapshot, _
				ByVal _Events As System.Collections.Generic.List(Of CommandPerformanceSnapshot) _
			)

				MyBase.New()

				Start = _Start
				Events = _Events

			End Sub

		#End Region

		#Region " Public Constants "

			''' <summary>Public Shared Reference to the Name of the Property: Start</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_START As String = "Start"

			''' <summary>Public Shared Reference to the Name of the Property: Events</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_EVENTS As String = "Events"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: Start</summary>
			''' <remarks></remarks>
			Private m_Start As CommandPerformanceSnapshot

			''' <summary>Private Data Storage Variable for Property: Events</summary>
			''' <remarks></remarks>
			Private m_Events As System.Collections.Generic.List(Of CommandPerformanceSnapshot)

		#End Region

		#Region " Public Properties "

			''' <summary>Provides Access to the Property: Start</summary>
			''' <remarks></remarks>
			Public Property Start() As CommandPerformanceSnapshot
				Get
					Return m_Start
				End Get
				Set(value As CommandPerformanceSnapshot)
					m_Start = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: Events</summary>
			''' <remarks></remarks>
			Public Property Events() As System.Collections.Generic.List(Of CommandPerformanceSnapshot)
				Get
					Return m_Events
				End Get
				Set(value As System.Collections.Generic.List(Of CommandPerformanceSnapshot))
					m_Events = value
				End Set
			End Property

		#End Region

	End Class

End Namespace