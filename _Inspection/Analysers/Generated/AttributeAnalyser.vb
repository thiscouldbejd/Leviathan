Namespace Inspection

	''' <summary>Analysis Class for a Attribute.</summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:51:01</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Inspection\Analysers\Generated\AttributeAnalyser.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Inspection\Analysers\Generated\AttributeAnalyser.tt", "1")> _
	Partial Public Class AttributeAnalyser
		Inherits System.Object

		#Region " Public Constructors "

			''' <summary>Parametered Constructor (1 Parameters)</summary>
			Public Sub New( _
				ByVal _Attribute As System.Attribute _
			)

				MyBase.New()

				Attribute = _Attribute

			End Sub

		#End Region

		#Region " Public Constants "

			''' <summary>Public Shared Reference to the Name of the Property: Attribute</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_ATTRIBUTE As String = "Attribute"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: Attribute</summary>
			''' <remarks></remarks>
			Private m_Attribute As System.Attribute

		#End Region

		#Region " Public Properties "

			''' <summary>Provides Access to the Property: Attribute</summary>
			''' <remarks></remarks>
			Public Property Attribute() As System.Attribute
				Get
					Return m_Attribute
				End Get
				Set(value As System.Attribute)
					m_Attribute = value
				End Set
			End Property

		#End Region

	End Class

End Namespace