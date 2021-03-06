Namespace Inspection

	''' <summary>Query Part for Argument Count.</summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:52:17</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Inspection\Querying\Generated\ArgumentPart.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Inspection\Querying\Generated\ArgumentPart.tt", "1")> _
	Partial Public Class ArgumentPart
		Inherits System.Object

		#Region " Public Constructors "

			''' <summary>Default Constructor</summary>
			Public Sub New()

				MyBase.New()

				m_ArgumentCount = 1
			End Sub

			''' <summary>Parametered Constructor (1 Parameters)</summary>
			Public Sub New( _
				ByVal _ArgumentCount As System.Int32 _
			)

				MyBase.New()

				ArgumentCount = _ArgumentCount

			End Sub

		#End Region

		#Region " Class Plumbing/Interface Code "

			#Region " Hashable Implementation "

				#Region " Public Methods "

					''' <summary>Method to Generate a HashCode</summary>
					''' <remarks></remarks>
					Public Overrides Function GetHashCode() As System.Int32
						Return Leviathan.Caching.Simple.CombineHashCodes(GetType(ArgumentPart), ArgumentCount)
					End Function

				#End Region

			#End Region

			#Region " Settable Implementation "

				#Region " Public Methods "

					Public Function SetArgumentCount(_ArgumentCount As System.Int32) As ArgumentPart

						ArgumentCount = _ArgumentCount
						Return Me

					End Function

				#End Region

			#End Region

		#End Region

		#Region " Public Constants "

			''' <summary>Public Shared Reference to the Name of the Property: ArgumentCount</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_ARGUMENTCOUNT As String = "ArgumentCount"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: ArgumentCount</summary>
			''' <remarks></remarks>
			Private m_ArgumentCount As System.Int32

		#End Region

		#Region " Public Properties "

			''' <summary>Provides Access to the Property: ArgumentCount</summary>
			''' <remarks></remarks>
			Public Property ArgumentCount() As System.Int32
				Get
					Return m_ArgumentCount
				End Get
				Set(value As System.Int32)
					m_ArgumentCount = value
				End Set
			End Property

		#End Region

	End Class

End Namespace