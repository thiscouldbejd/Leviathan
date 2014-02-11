Namespace Commands

	''' <summary></summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:46:22</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Commands\Generated\CommandInterrogatedFlag.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Commands\Generated\CommandInterrogatedFlag.tt", "1")> _
	Partial Public Class CommandInterrogatedFlag
		Inherits System.Object
		Implements System.IComparable

		#Region " Public Constructors "

			''' <summary>Default Constructor</summary>
			Public Sub New()

				MyBase.New()

			End Sub

			''' <summary>Parametered Constructor (1 Parameters)</summary>
			Public Sub New( _
				ByVal _FlagAttribute As ConfigurableAttribute _
			)

				MyBase.New()

				FlagAttribute = _FlagAttribute

			End Sub

			''' <summary>Parametered Constructor (2 Parameters)</summary>
			Public Sub New( _
				ByVal _FlagAttribute As ConfigurableAttribute, _
				ByVal _FieldName As System.String _
			)

				MyBase.New()

				FlagAttribute = _FlagAttribute
				FieldName = _FieldName

			End Sub

			''' <summary>Parametered Constructor (3 Parameters)</summary>
			Public Sub New( _
				ByVal _FlagAttribute As ConfigurableAttribute, _
				ByVal _FieldName As System.String, _
				ByVal _FieldType As System.Type _
			)

				MyBase.New()

				FlagAttribute = _FlagAttribute
				FieldName = _FieldName
				FieldType = _FieldType

			End Sub

		#End Region

		#Region " Class Plumbing/Interface Code "

			#Region " IComparable Implementation "

				#Region " Public Methods "

					''' <summary>Comparison Method</summary>
					Public Overridable Function IComparable_CompareTo( _
						ByVal value As System.Object _
					) As System.Int32 Implements System.IComparable.CompareTo

						Dim typed_Value As CommandInterrogatedFlag = TryCast(value, CommandInterrogatedFlag)

						If typed_Value Is Nothing Then

							Throw New ArgumentException(String.Format("Value is not of comparable type: {0}", value.GetType.Name), "Value")

						Else

							Dim return_Value As Integer = 0

							If Not FlagAttribute Is Nothing Then return_Value = DirectCast(FlagAttribute, System.IComparable).CompareTo(typed_Value.FlagAttribute)
							If return_Value <> 0 Then Return return_Value

							Return return_Value

						End If

					End Function

				#End Region

			#End Region

		#End Region

		#Region " Public Constants "

			''' <summary>Public Shared Reference to the Name of the Property: FlagAttribute</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_FLAGATTRIBUTE As String = "FlagAttribute"

			''' <summary>Public Shared Reference to the Name of the Property: FieldName</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_FIELDNAME As String = "FieldName"

			''' <summary>Public Shared Reference to the Name of the Property: FieldType</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_FIELDTYPE As String = "FieldType"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: FlagAttribute</summary>
			''' <remarks></remarks>
			Private m_FlagAttribute As ConfigurableAttribute

			''' <summary>Private Data Storage Variable for Property: FieldName</summary>
			''' <remarks></remarks>
			Private m_FieldName As System.String

			''' <summary>Private Data Storage Variable for Property: FieldType</summary>
			''' <remarks></remarks>
			Private m_FieldType As System.Type

		#End Region

		#Region " Public Properties "

			''' <summary>Provides Access to the Property: FlagAttribute</summary>
			''' <remarks></remarks>
			Public Property FlagAttribute() As ConfigurableAttribute
				Get
					Return m_FlagAttribute
				End Get
				Set(value As ConfigurableAttribute)
					m_FlagAttribute = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: FieldName</summary>
			''' <remarks></remarks>
			Public Property FieldName() As System.String
				Get
					Return m_FieldName
				End Get
				Set(value As System.String)
					m_FieldName = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: FieldType</summary>
			''' <remarks></remarks>
			Public Property FieldType() As System.Type
				Get
					Return m_FieldType
				End Get
				Set(value As System.Type)
					m_FieldType = value
				End Set
			End Property

		#End Region

	End Class

End Namespace