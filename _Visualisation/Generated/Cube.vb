Namespace Visualisation

	''' <summary></summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:53:47</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Visualisation\Generated\Cube.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Visualisation\Generated\Cube.tt", "1")> _
	Partial Public Class Cube
		Inherits System.Object

		#Region " Public Constructors "

			''' <summary>Default Constructor</summary>
			Public Sub New()

				MyBase.New()

				m_Columns = New System.Collections.Generic.List(Of Leviathan.Commands.FormatterProperty)
				m_Slices = New System.Collections.Generic.List(Of Slice)
			End Sub

			''' <summary>Parametered Constructor (1 Parameters)</summary>
			Public Sub New( _
				ByVal _Title As Message _
			)

				MyBase.New()

				Title = _Title

				m_Columns = New System.Collections.Generic.List(Of Leviathan.Commands.FormatterProperty)
				m_Slices = New System.Collections.Generic.List(Of Slice)
			End Sub

			''' <summary>Parametered Constructor (2 Parameters)</summary>
			Public Sub New( _
				ByVal _Title As Message, _
				ByVal _Columns As System.Collections.Generic.List(Of Leviathan.Commands.FormatterProperty) _
			)

				MyBase.New()

				Title = _Title
				Columns = _Columns

				m_Slices = New System.Collections.Generic.List(Of Slice)
			End Sub

			''' <summary>Parametered Constructor (3 Parameters)</summary>
			Public Sub New( _
				ByVal _Title As Message, _
				ByVal _Columns As System.Collections.Generic.List(Of Leviathan.Commands.FormatterProperty), _
				ByVal _Slices As System.Collections.Generic.List(Of Slice) _
			)

				MyBase.New()

				Title = _Title
				Columns = _Columns
				Slices = _Slices

			End Sub

			''' <summary>Parametered Constructor (4 Parameters)</summary>
			Public Sub New( _
				ByVal _Title As Message, _
				ByVal _Columns As System.Collections.Generic.List(Of Leviathan.Commands.FormatterProperty), _
				ByVal _Slices As System.Collections.Generic.List(Of Slice), _
				ByVal _ColumnLengths As System.Int32() _
			)

				MyBase.New()

				Title = _Title
				Columns = _Columns
				Slices = _Slices
				ColumnLengths = _ColumnLengths

			End Sub

			''' <summary>Parametered Constructor (5 Parameters)</summary>
			Public Sub New( _
				ByVal _Title As Message, _
				ByVal _Columns As System.Collections.Generic.List(Of Leviathan.Commands.FormatterProperty), _
				ByVal _Slices As System.Collections.Generic.List(Of Slice), _
				ByVal _ColumnLengths As System.Int32(), _
				ByVal _TotalColumnLength As System.Int32 _
			)

				MyBase.New()

				Title = _Title
				Columns = _Columns
				Slices = _Slices
				ColumnLengths = _ColumnLengths
				TotalColumnLength = _TotalColumnLength

			End Sub

		#End Region

		#Region " Public Constants "

			''' <summary>Public Shared Reference to the Name of the Property: Title</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_TITLE As String = "Title"

			''' <summary>Public Shared Reference to the Name of the Property: Columns</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_COLUMNS As String = "Columns"

			''' <summary>Public Shared Reference to the Name of the Property: Slices</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_SLICES As String = "Slices"

			''' <summary>Public Shared Reference to the Name of the Property: ColumnLengths</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_COLUMNLENGTHS As String = "ColumnLengths"

			''' <summary>Public Shared Reference to the Name of the Property: TotalColumnLength</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_TOTALCOLUMNLENGTH As String = "TotalColumnLength"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: Title</summary>
			''' <remarks></remarks>
			Private m_Title As Message

			''' <summary>Private Data Storage Variable for Property: Columns</summary>
			''' <remarks></remarks>
			Private m_Columns As System.Collections.Generic.List(Of Leviathan.Commands.FormatterProperty)

			''' <summary>Private Data Storage Variable for Property: Slices</summary>
			''' <remarks></remarks>
			Private m_Slices As System.Collections.Generic.List(Of Slice)

			''' <summary>Private Data Storage Variable for Property: ColumnLengths</summary>
			''' <remarks></remarks>
			Private m_ColumnLengths As System.Int32()

			''' <summary>Private Data Storage Variable for Property: TotalColumnLength</summary>
			''' <remarks></remarks>
			Private m_TotalColumnLength As System.Int32

		#End Region

		#Region " Public Properties "

			''' <summary>Provides Access to the Property: Title</summary>
			''' <remarks></remarks>
			Public Property Title() As Message
				Get
					Return m_Title
				End Get
				Set(value As Message)
					m_Title = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: Columns</summary>
			''' <remarks></remarks>
			Public Property Columns() As System.Collections.Generic.List(Of Leviathan.Commands.FormatterProperty)
				Get
					Return m_Columns
				End Get
				Set(value As System.Collections.Generic.List(Of Leviathan.Commands.FormatterProperty))
					m_Columns = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: Slices</summary>
			''' <remarks></remarks>
			Public Property Slices() As System.Collections.Generic.List(Of Slice)
				Get
					Return m_Slices
				End Get
				Set(value As System.Collections.Generic.List(Of Slice))
					m_Slices = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: ColumnLengths</summary>
			''' <remarks></remarks>
			Public Property ColumnLengths() As System.Int32()
				Get
					Return m_ColumnLengths
				End Get
				Set(value As System.Int32())
					m_ColumnLengths = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: TotalColumnLength</summary>
			''' <remarks></remarks>
			Public Property TotalColumnLength() As System.Int32
				Get
					Return m_TotalColumnLength
				End Get
				Set(value As System.Int32)
					m_TotalColumnLength = value
				End Set
			End Property

		#End Region

	End Class

End Namespace