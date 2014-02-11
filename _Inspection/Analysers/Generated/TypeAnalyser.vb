Namespace Inspection

	''' <summary>Analysis Class for a Type.</summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:51:36</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Inspection\Analysers\Generated\TypeAnalyser.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Inspection\Analysers\Generated\TypeAnalyser.tt", "1")> _
	Partial Public Class TypeAnalyser
		Inherits System.Object

		#Region " Singleton Implementation "

			#Region " Private Constructors "

				''' <summary>Parametered Constructor (1 Parameters)</summary>
				Private Sub New( _
					ByVal _Type As System.Type _
				)

					MyBase.New()

					m_Type = _Type

				End Sub

			#End Region

			#Region " Private Shared Variables "

				Private Shared Singleton_Instances As New Hashtable

				Private Shared Singleton_Instances_LOCK As New Object

			#End Region

			#Region " Public Shared Functions "

				Public Shared Function GetInstance( _
					ByVal _Type As System.Type _
				) As TypeAnalyser

					Dim instance_Hashcode As Integer = _
						Leviathan.Caching.Simple.CombineHashCodes(_Type)

					SyncLock Singleton_Instances_LOCK

						If Not Singleton_Instances.Contains(instance_Hashcode) Then Singleton_Instances(instance_Hashcode) = _
							New TypeAnalyser(_Type)

					End SyncLock

					Return Singleton_Instances(instance_Hashcode)

				End Function


			#End Region

		#End Region

		#Region " Public Constants "

			''' <summary>Public Shared Reference to the Name of the Property: Type</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_TYPE As String = "Type"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: Type</summary>
			''' <remarks></remarks>
			Private m_Type As System.Type

		#End Region

		#Region " Public Properties "

			''' <summary>Provides Access to the Property: Type</summary>
			''' <remarks></remarks>
			Public ReadOnly Property [Type]() As System.Type
				Get
					Return m_Type
				End Get
			End Property

		#End Region

	End Class

End Namespace