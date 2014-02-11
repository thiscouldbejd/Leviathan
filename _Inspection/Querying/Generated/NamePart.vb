Namespace Inspection

	''' <summary>Query Part for Name.</summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:52:37</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Inspection\Querying\Generated\NamePart.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Inspection\Querying\Generated\NamePart.tt", "1")> _
	Partial Public Class NamePart
		Inherits System.Object

		#Region " Public Constructors "

			''' <summary>Default Constructor</summary>
			Public Sub New()

				MyBase.New()

			End Sub

			''' <summary>Parametered Constructor (1 Parameters)</summary>
			Public Sub New( _
				ByVal _Names As System.String() _
			)

				MyBase.New()

				Names = _Names

			End Sub

		#End Region

		#Region " Class Plumbing/Interface Code "

			#Region " Hashable Implementation "

				#Region " Public Methods "

					''' <summary>Method to Generate a HashCode</summary>
					''' <remarks></remarks>
					Public Overrides Function GetHashCode() As System.Int32
						Return Leviathan.Caching.Simple.CombineHashCodes(GetType(NamePart), Names)
					End Function

				#End Region

			#End Region

			#Region " Settable Implementation "

				#Region " Public Methods "

					Public Function SetNames(_Names As System.String()) As NamePart

						Names = _Names
						Return Me

					End Function

				#End Region

			#End Region

		#End Region

		#Region " Public Constants "

			''' <summary>Public Shared Reference to the Name of the Property: Names</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_NAMES As String = "Names"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: Names</summary>
			''' <remarks></remarks>
			Private m_Names As System.String()

		#End Region

		#Region " Public Properties "

			''' <summary>Provides Access to the Property: Names</summary>
			''' <remarks></remarks>
			Public Property Names() As System.String()
				Get
					Return m_Names
				End Get
				Set(value As System.String())
					m_Names = value
				End Set
			End Property

		#End Region

	End Class

End Namespace