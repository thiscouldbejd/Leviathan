Namespace Inspection

	''' <summary>Query Part for Attribute Presence.</summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:52:25</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Inspection\Querying\Generated\AttributePart.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Inspection\Querying\Generated\AttributePart.tt", "1")> _
	Partial Public Class AttributePart
		Inherits System.Object

		#Region " Public Constructors "

			''' <summary>Default Constructor</summary>
			Public Sub New()

				MyBase.New()

			End Sub

			''' <summary>Parametered Constructor (1 Parameters)</summary>
			Public Sub New( _
				ByVal _AttributeType As System.Type _
			)

				MyBase.New()

				AttributeType = _AttributeType

			End Sub

		#End Region

		#Region " Class Plumbing/Interface Code "

			#Region " Hashable Implementation "

				#Region " Public Methods "

					''' <summary>Method to Generate a HashCode</summary>
					''' <remarks></remarks>
					Public Overrides Function GetHashCode() As System.Int32
						Return Leviathan.Caching.Simple.CombineHashCodes(GetType(AttributePart), AttributeType)
					End Function

				#End Region

			#End Region

			#Region " Settable Implementation "

				#Region " Public Methods "

					Public Function SetAttributeType(_AttributeType As System.Type) As AttributePart

						AttributeType = _AttributeType
						Return Me

					End Function

				#End Region

			#End Region

		#End Region

		#Region " Public Constants "

			''' <summary>Public Shared Reference to the Name of the Property: AttributeType</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_ATTRIBUTETYPE As String = "AttributeType"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: AttributeType</summary>
			''' <remarks></remarks>
			Private m_AttributeType As System.Type

		#End Region

		#Region " Public Properties "

			''' <summary>Provides Access to the Property: AttributeType</summary>
			''' <remarks></remarks>
			Public Property AttributeType() As System.Type
				Get
					Return m_AttributeType
				End Get
				Set(value As System.Type)
					m_AttributeType = value
				End Set
			End Property

		#End Region

	End Class

End Namespace