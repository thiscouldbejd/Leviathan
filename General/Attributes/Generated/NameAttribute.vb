''' <summary>This attribute should be used to decorate a Class with a Friendly Name.</summary>
''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
''' <generator-date>10/02/2014 15:37:47</generator-date>
''' <generator-functions>1</generator-functions>
''' <generator-source>Leviathan\General\Attributes\Generated\NameAttribute.tt</generator-source>
''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
''' <generator-version>1</generator-version>
<System.CodeDom.Compiler.GeneratedCode("Leviathan\General\Attributes\Generated\NameAttribute.tt", "1")> _
<AttributeUsageAttribute(AttributeTargets.[Class], _
	AllowMultiple:=False, Inherited:=False)> _
Partial Public NotInheritable Class NameAttribute
	Inherits System.Attribute

	#Region " Public Constructors "

		''' <summary>Default Constructor</summary>
		Public Sub New()

			MyBase.New()

		End Sub

		''' <summary>Parametered Constructor (1 Parameters)</summary>
		Public Sub New( _
			ByVal _ResourceContainingType As System.Type _
		)

			MyBase.New()

			ResourceContainingType = _ResourceContainingType

		End Sub

		''' <summary>Parametered Constructor (2 Parameters)</summary>
		Public Sub New( _
			ByVal _ResourceContainingType As System.Type, _
			ByVal _ResourceName As System.String _
		)

			MyBase.New()

			ResourceContainingType = _ResourceContainingType
			ResourceName = _ResourceName

		End Sub

		''' <summary>Parametered Constructor (3 Parameters)</summary>
		Public Sub New( _
			ByVal _ResourceContainingType As System.Type, _
			ByVal _ResourceName As System.String, _
			ByVal _Name As System.String _
		)

			MyBase.New()

			ResourceContainingType = _ResourceContainingType
			ResourceName = _ResourceName
			Name = _Name

		End Sub

	#End Region

	#Region " Class Plumbing/Interface Code "

		#Region " Attributable Implementation "

			#Region " Public Shared Methods "

				Public Shared Function GetMember( _
					ByVal onType As Type _
				) As Leviathan.Inspection.MemberAnalyser

					Return GetSingleObject(GetMembers(onType))

				End Function

				Public Shared Function GetMembers( _
					ByVal onType As Type _
				) As Leviathan.Inspection.MemberAnalyser()

					Return Leviathan.Inspection.TypeAnalyser.GetInstance(onType) _
						.ExecuteQuery(Leviathan.Inspection.AnalyserQuery.QUERY_MEMBERS_READABLE _
							.SetPresentAttribute(GetType(NameAttribute)) _
					)

				End Function

			#End Region

		#End Region

		#Region " Resourced Implementation "

			#Region " Public Constants "

				''' <summary></summary>
				''' <remarks></remarks>
				Public Const RESOURCE_PREFIX As String = "@"

				''' <summary></summary>
				''' <remarks></remarks>
				Public Const RESOURCE_SUFFIX As String = "@"

			#End Region

			#Region " Private Properties "

				Private ReadOnly Property ResourceContainingAssembly() As System.Reflection.Assembly
					Get
						If ResourceContainingType Is Nothing Then

							If Not System.Reflection.Assembly.GetEntryAssembly Is Nothing Then

								Return System.Reflection.Assembly.GetEntryAssembly

							Else

								Return System.Reflection.Assembly.GetExecutingAssembly

							End If

						Else

							Return ResourceContainingType.Assembly

						End If
					End Get
				End Property

			#End Region

			#Region " Protected Methods "

				Protected Function GetResourcedValue( _
					ByVal value As String _
				) As String

					If Not String.IsNullOrEmpty(value) AndAlso value.StartsWith(RESOURCE_PREFIX) AndAlso _
						value.EndsWith(RESOURCE_SUFFIX) Then

						Return Resources.SingleResource( _
							ResourceContainingAssembly, ResourceName, _
							value.Substring(RESOURCE_PREFIX.Length, value.Length - _
							(RESOURCE_PREFIX.Length + RESOURCE_SUFFIX.Length)))

					Else

						Return value

					End If

				End Function

			#End Region

		#End Region

	#End Region

	#Region " Public Constants "

		''' <summary>Public Shared Reference to the Name of the Property: ResourceContainingType</summary>
		''' <remarks></remarks>
		Public Const PROPERTY_RESOURCECONTAININGTYPE As String = "ResourceContainingType"

		''' <summary>Public Shared Reference to the Name of the Property: ResourceName</summary>
		''' <remarks></remarks>
		Public Const PROPERTY_RESOURCENAME As String = "ResourceName"

		''' <summary>Public Shared Reference to the Name of the Property: Name</summary>
		''' <remarks></remarks>
		Public Const PROPERTY_NAME As String = "Name"

	#End Region

	#Region " Private Variables "

		''' <summary>Private Data Storage Variable for Property: ResourceContainingType</summary>
		''' <remarks></remarks>
		Private m_ResourceContainingType As System.Type

		''' <summary>Private Data Storage Variable for Property: ResourceName</summary>
		''' <remarks></remarks>
		Private m_ResourceName As System.String

		''' <summary>Private Data Storage Variable for Property: Name</summary>
		''' <remarks></remarks>
		Private m_Name As System.String

	#End Region

	#Region " Public Properties "

		''' <summary>Provides Access to the Property: ResourceContainingType</summary>
		''' <remarks></remarks>
		Public Property ResourceContainingType() As System.Type
			Get
				Return m_ResourceContainingType
			End Get
			Set(value As System.Type)
				m_ResourceContainingType = value
			End Set
		End Property

		''' <summary>Provides Access to the Property: ResourceName</summary>
		''' <remarks></remarks>
		Public Property ResourceName() As System.String
			Get
				Return m_ResourceName
			End Get
			Set(value As System.String)
				m_ResourceName = value
			End Set
		End Property

		''' <summary>Provides Access to the Property: Name</summary>
		''' <remarks></remarks>
		Public Property Name() As System.String
			Get
				Return m_Name
			End Get
			Set(value As System.String)
				m_Name = value
				m_Name = GetResourcedValue(m_Name)
			End Set
		End Property

	#End Region

End Class