Namespace Inspection

	''' <summary>Query Part for Accessibility (e.g. Read/Write, Read-Only etc).</summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:51:58</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Inspection\Querying\Generated\AccessibilityPart.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Inspection\Querying\Generated\AccessibilityPart.tt", "1")> _
	Partial Public Class AccessibilityPart
		Inherits System.Object

		#Region " Public Constructors "

			''' <summary>Default Constructor</summary>
			Public Sub New()

				MyBase.New()

				m_Accessibility = MemberAccessibility.All
			End Sub

			''' <summary>Parametered Constructor (1 Parameters)</summary>
			Public Sub New( _
				ByVal _Accessibility As MemberAccessibility _
			)

				MyBase.New()

				Accessibility = _Accessibility

			End Sub

		#End Region

		#Region " Class Plumbing/Interface Code "

			#Region " Hashable Implementation "

				#Region " Public Methods "

					''' <summary>Method to Generate a HashCode</summary>
					''' <remarks></remarks>
					Public Overrides Function GetHashCode() As System.Int32
						Return Leviathan.Caching.Simple.CombineHashCodes(GetType(AccessibilityPart), Accessibility)
					End Function

				#End Region

			#End Region

			#Region " Settable Implementation "

				#Region " Public Methods "

					Public Function SetAccessibility(_Accessibility As MemberAccessibility) As AccessibilityPart

						Accessibility = _Accessibility
						Return Me

					End Function

				#End Region

			#End Region

		#End Region

		#Region " Public Constants "

			''' <summary>Public Shared Reference to the Name of the Property: Accessibility</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_ACCESSIBILITY As String = "Accessibility"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: Accessibility</summary>
			''' <remarks></remarks>
			Private m_Accessibility As MemberAccessibility

		#End Region

		#Region " Public Properties "

			''' <summary>Provides Access to the Property: Accessibility</summary>
			''' <remarks></remarks>
			Public Property Accessibility() As MemberAccessibility
				Get
					Return m_Accessibility
				End Get
				Set(value As MemberAccessibility)
					m_Accessibility = value
				End Set
			End Property

		#End Region

	End Class

End Namespace