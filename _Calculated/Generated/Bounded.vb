Namespace Calculated

	''' <summary></summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:40:23</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Calculated\Generated\Bounded.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Calculated\Generated\Bounded.tt", "1")> _
	Partial Public Class Bounded
		Inherits System.Object

		#Region " Public Constructors "

			''' <summary>Default Constructor</summary>
			Public Sub New()

				MyBase.New()

				m_All = New System.Collections.Generic.List(Of System.Double)
				m_Minimum = System.Double.MaxValue
				m_Maximum = System.Double.MinValue
			End Sub

			''' <summary>Parametered Constructor (1 Parameters)</summary>
			Public Sub New( _
				ByVal _All As System.Collections.Generic.List(Of System.Double) _
			)

				MyBase.New()

				All = _All

				m_Minimum = System.Double.MaxValue
				m_Maximum = System.Double.MinValue
			End Sub

			''' <summary>Parametered Constructor (2 Parameters)</summary>
			Public Sub New( _
				ByVal _All As System.Collections.Generic.List(Of System.Double), _
				ByVal _Minimum As System.Double _
			)

				MyBase.New()

				All = _All
				Minimum = _Minimum

				m_Maximum = System.Double.MinValue
			End Sub

			''' <summary>Parametered Constructor (3 Parameters)</summary>
			Public Sub New( _
				ByVal _All As System.Collections.Generic.List(Of System.Double), _
				ByVal _Minimum As System.Double, _
				ByVal _Maximum As System.Double _
			)

				MyBase.New()

				All = _All
				Minimum = _Minimum
				Maximum = _Maximum

			End Sub

		#End Region

		#Region " Public Constants "

			''' <summary>Public Shared Reference to the Name of the Property: All</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_ALL As String = "All"

			''' <summary>Public Shared Reference to the Name of the Property: Minimum</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_MINIMUM As String = "Minimum"

			''' <summary>Public Shared Reference to the Name of the Property: Maximum</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_MAXIMUM As String = "Maximum"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: All</summary>
			''' <remarks></remarks>
			Private m_All As System.Collections.Generic.List(Of System.Double)

			''' <summary>Private Data Storage Variable for Property: Minimum</summary>
			''' <remarks></remarks>
			Private m_Minimum As System.Double

			''' <summary>Private Data Storage Variable for Property: Maximum</summary>
			''' <remarks></remarks>
			Private m_Maximum As System.Double

		#End Region

		#Region " Public Properties "

			''' <summary>Provides Access to the Property: All</summary>
			''' <remarks></remarks>
			Public Property All() As System.Collections.Generic.List(Of System.Double)
				Get
					Return m_All
				End Get
				Set(value As System.Collections.Generic.List(Of System.Double))
					m_All = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: Minimum</summary>
			''' <remarks></remarks>
			Public Property Minimum() As System.Double
				Get
					Return m_Minimum
				End Get
				Set(value As System.Double)
					m_Minimum = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: Maximum</summary>
			''' <remarks></remarks>
			Public Property Maximum() As System.Double
				Get
					Return m_Maximum
				End Get
				Set(value As System.Double)
					m_Maximum = value
				End Set
			End Property

		#End Region

	End Class

End Namespace