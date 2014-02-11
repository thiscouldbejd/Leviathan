Namespace Inspection

	''' <summary>Exception which should be thrown when a Member is Not Found.</summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:51:50</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Inspection\Exceptions\MemberNotFoundException.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Inspection\Exceptions\MemberNotFoundException.tt", "1")> _
	Partial Public NotInheritable Class MemberNotFoundException
		Inherits Exception
		Implements System.IFormattable

		#Region " Public Constructors "

			''' <summary>Default Constructor</summary>
			Public Sub New()

				MyBase.New()

			End Sub

			''' <summary>Parametered Constructor (1 Parameters)</summary>
			Public Sub New( _
				ByVal _Name As System.String _
			)

				MyBase.New()

				Name = _Name

			End Sub

			''' <summary>Parametered Constructor (2 Parameters)</summary>
			Public Sub New( _
				ByVal _Name As System.String, _
				ByVal _Type As System.Type _
			)

				MyBase.New()

				Name = _Name
				[Type] = _Type

			End Sub

		#End Region

		#Region " Class Plumbing/Interface Code "

			#Region " IFormattable Implementation "

				#Region " Public Constants "

					''' <summary>Public Shared Reference to the Name of the Property: AsString</summary>
					''' <remarks></remarks>
					Public Const PROPERTY_ASSTRING As String = "AsString"

				#End Region

				#Region " Public Properties "

					''' <summary></summary>
					''' <remarks></remarks>
					Public ReadOnly Property AsString() As System.String
						Get
							Return Me.ToString()
						End Get
					End Property

				#End Region

				#Region " Public Shared Methods "

					Public Shared Function ToString_default( _
						ByVal Name As System.String, _
						ByVal Type As System.Type _
					) As String

						Return String.Format( _
							"{0} not found on {1}", _
							Name, _
							Type)

					End Function

				#End Region

				#Region " Public Methods "

					Public Overloads Overrides Function ToString() As String

						Return Me.ToString(String.Empty, Nothing)

					End Function

					Public Overloads Function ToString( _
						ByVal format As String _
					) As String

						If String.IsNullOrEmpty(format) OrElse String.Compare(format, "default", True) = 0 Then

							Return ToString_default( _
								Name, _
								Type _
							)

						End If

						Return String.Empty

					End Function

					Public Overloads Function ToString( _
						ByVal format As String, _
						ByVal formatProvider As System.IFormatProvider _
					) As String Implements System.IFormattable.ToString

						If String.IsNullOrEmpty(format) OrElse String.Compare(format, "default", True) = 0 Then	

							Return ToString_default( _
								Name, _
								Type _
							)

						End If

						Return String.Empty

					End Function

				#End Region

			#End Region

		#End Region

		#Region " Public Constants "

			''' <summary>Public Shared Reference to the Name of the Property: Name</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_NAME As String = "Name"

			''' <summary>Public Shared Reference to the Name of the Property: Type</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_TYPE As String = "Type"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: Name</summary>
			''' <remarks></remarks>
			Private m_Name As System.String

			''' <summary>Private Data Storage Variable for Property: Type</summary>
			''' <remarks></remarks>
			Private m_Type As System.Type

		#End Region

		#Region " Public Properties "

			''' <summary>Provides Access to the Property: Name</summary>
			''' <remarks></remarks>
			Public Property Name() As System.String
				Get
					Return m_Name
				End Get
				Set(value As System.String)
					m_Name = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: Type</summary>
			''' <remarks></remarks>
			Public Property [Type]() As System.Type
				Get
					Return m_Type
				End Get
				Set(value As System.Type)
					m_Type = value
				End Set
			End Property

		#End Region

	End Class

End Namespace