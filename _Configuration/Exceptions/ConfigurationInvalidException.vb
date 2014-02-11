Namespace Configuration

	''' <summary>Exception which should be thrown when a required Provider Object is not available</summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:49:42</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Configuration\Exceptions\ConfigurationInvalidException.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Configuration\Exceptions\ConfigurationInvalidException.tt", "1")> _
	<Leviathan.Description(Description:="Exception which should be thrown when a required Provider Object is not available")> _
	Partial Public Class ConfigurationInvalidException
		Inherits Exception

		#Region " Public Constructors "

			''' <summary>Default Constructor</summary>
			Public Sub New()

				MyBase.New()

			End Sub

			''' <summary>Parametered Constructor (1 Parameters)</summary>
			Public Sub New( _
				ByVal _ProviderType As System.Type _
			)

				MyBase.New()

				ProviderType = _ProviderType

			End Sub

			''' <summary>Parametered Constructor (2 Parameters)</summary>
			Public Sub New( _
				ByVal _ProviderType As System.Type, _
				ByVal _ProviderAssembly As System.Reflection.Assembly _
			)

				MyBase.New()

				ProviderType = _ProviderType
				ProviderAssembly = _ProviderAssembly

			End Sub

			''' <summary>Parametered Constructor (3 Parameters)</summary>
			Public Sub New( _
				ByVal _ProviderType As System.Type, _
				ByVal _ProviderAssembly As System.Reflection.Assembly, _
				ByVal _ProviderSpecificName As System.String _
			)

				MyBase.New()

				ProviderType = _ProviderType
				ProviderAssembly = _ProviderAssembly
				ProviderSpecificName = _ProviderSpecificName

			End Sub

			''' <summary>Parametered Constructor (4 Parameters)</summary>
			Public Sub New( _
				ByVal _ProviderType As System.Type, _
				ByVal _ProviderAssembly As System.Reflection.Assembly, _
				ByVal _ProviderSpecificName As System.String, _
				ByVal _ConfigurationAttributeName As System.String _
			)

				MyBase.New()

				ProviderType = _ProviderType
				ProviderAssembly = _ProviderAssembly
				ProviderSpecificName = _ProviderSpecificName
				ConfigurationAttributeName = _ConfigurationAttributeName

			End Sub

			''' <summary>Parametered Constructor (5 Parameters)</summary>
			Public Sub New( _
				ByVal _ProviderType As System.Type, _
				ByVal _ProviderAssembly As System.Reflection.Assembly, _
				ByVal _ProviderSpecificName As System.String, _
				ByVal _ConfigurationAttributeName As System.String, _
				ByVal _ConfigurationAttributeValue As System.String _
			)

				MyBase.New()

				ProviderType = _ProviderType
				ProviderAssembly = _ProviderAssembly
				ProviderSpecificName = _ProviderSpecificName
				ConfigurationAttributeName = _ConfigurationAttributeName
				ConfigurationAttributeValue = _ConfigurationAttributeValue

			End Sub

		#End Region

		#Region " Public Constants "

			''' <summary>Public Shared Reference to the Name of the Property: ProviderType</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_PROVIDERTYPE As String = "ProviderType"

			''' <summary>Public Shared Reference to the Name of the Property: ProviderAssembly</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_PROVIDERASSEMBLY As String = "ProviderAssembly"

			''' <summary>Public Shared Reference to the Name of the Property: ProviderSpecificName</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_PROVIDERSPECIFICNAME As String = "ProviderSpecificName"

			''' <summary>Public Shared Reference to the Name of the Property: ConfigurationAttributeName</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_CONFIGURATIONATTRIBUTENAME As String = "ConfigurationAttributeName"

			''' <summary>Public Shared Reference to the Name of the Property: ConfigurationAttributeValue</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_CONFIGURATIONATTRIBUTEVALUE As String = "ConfigurationAttributeValue"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: ProviderType</summary>
			''' <remarks></remarks>
			Private m_ProviderType As System.Type

			''' <summary>Private Data Storage Variable for Property: ProviderAssembly</summary>
			''' <remarks></remarks>
			Private m_ProviderAssembly As System.Reflection.Assembly

			''' <summary>Private Data Storage Variable for Property: ProviderSpecificName</summary>
			''' <remarks></remarks>
			Private m_ProviderSpecificName As System.String

			''' <summary>Private Data Storage Variable for Property: ConfigurationAttributeName</summary>
			''' <remarks></remarks>
			Private m_ConfigurationAttributeName As System.String

			''' <summary>Private Data Storage Variable for Property: ConfigurationAttributeValue</summary>
			''' <remarks></remarks>
			Private m_ConfigurationAttributeValue As System.String

		#End Region

		#Region " Public Properties "

			''' <summary>Provides Access to the Property: ProviderType</summary>
			''' <remarks></remarks>
			<Leviathan.Description(Description:="Provider Type")> _
			Public Property ProviderType() As System.Type
				Get
					Return m_ProviderType
				End Get
				Set(value As System.Type)
					m_ProviderType = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: ProviderAssembly</summary>
			''' <remarks></remarks>
			<Leviathan.Description(Description:="Provider Assembly Source")> _
			Public Property ProviderAssembly() As System.Reflection.Assembly
				Get
					Return m_ProviderAssembly
				End Get
				Set(value As System.Reflection.Assembly)
					m_ProviderAssembly = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: ProviderSpecificName</summary>
			''' <remarks></remarks>
			<Leviathan.Description(Description:="Specific Provider Name")> _
			Public Property ProviderSpecificName() As System.String
				Get
					Return m_ProviderSpecificName
				End Get
				Set(value As System.String)
					m_ProviderSpecificName = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: ConfigurationAttributeName</summary>
			''' <remarks></remarks>
			<Leviathan.Description(Description:="Name of the XML Attribute which is Invalid")> _
			Public Property ConfigurationAttributeName() As System.String
				Get
					Return m_ConfigurationAttributeName
				End Get
				Set(value As System.String)
					m_ConfigurationAttributeName = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: ConfigurationAttributeValue</summary>
			''' <remarks></remarks>
			<Leviathan.Description(Description:="Value of the XML Attribute which is Invalid")> _
			Public Property ConfigurationAttributeValue() As System.String
				Get
					Return m_ConfigurationAttributeValue
				End Get
				Set(value As System.String)
					m_ConfigurationAttributeValue = value
				End Set
			End Property

		#End Region

	End Class

End Namespace