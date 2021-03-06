Namespace Configuration

	''' <summary>Serialising Class for Xml, provides in and out functionality</summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:50:27</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Configuration\Generated\XmlSerialiser.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Configuration\Generated\XmlSerialiser.tt", "1")> _
	Partial Public Class XmlSerialiser
		Inherits System.Object
		Implements System.IDisposable

		#Region " Public Constructors "

			''' <summary>Default Constructor</summary>
			Public Sub New()

				MyBase.New()

				m_Parser = New FromString
			End Sub

			''' <summary>Parametered Constructor (1 Parameters)</summary>
			Public Sub New( _
				ByVal _InputReader As System.Xml.XmlTextReader _
			)

				MyBase.New()

				m_InputReader = _InputReader

				m_Parser = New FromString
			End Sub

			''' <summary>Parametered Constructor (2 Parameters)</summary>
			Public Sub New( _
				ByVal _InputReader As System.Xml.XmlTextReader, _
				ByVal _OutputWriter As System.Xml.XmlTextWriter _
			)

				MyBase.New()

				m_InputReader = _InputReader
				m_OutputWriter = _OutputWriter

				m_Parser = New FromString
			End Sub

			''' <summary>Parametered Constructor (3 Parameters)</summary>
			Public Sub New( _
				ByVal _InputReader As System.Xml.XmlTextReader, _
				ByVal _OutputWriter As System.Xml.XmlTextWriter, _
				ByVal _Parser As FromString _
			)

				MyBase.New()

				m_InputReader = _InputReader
				m_OutputWriter = _OutputWriter
				m_Parser = _Parser

			End Sub

			''' <summary>Parametered Constructor (4 Parameters)</summary>
			Public Sub New( _
				ByVal _InputReader As System.Xml.XmlTextReader, _
				ByVal _OutputWriter As System.Xml.XmlTextWriter, _
				ByVal _Parser As FromString, _
				ByVal _Formatting_Value As System.Object _
			)

				MyBase.New()

				m_InputReader = _InputReader
				m_OutputWriter = _OutputWriter
				m_Parser = _Parser
				Formatting_Value = _Formatting_Value

			End Sub

		#End Region

		#Region " Class Plumbing/Interface Code "

			#Region " IDisposable Implementation "

				#Region " Private Variables "

					''' <summary></summary>
					''' <remarks></remarks>
					Private IDisposable_DisposedCalled As System.Boolean

				#End Region

				#Region " Public Methods "

					Public Sub IDisposable_Dispose() Implements IDisposable.Dispose

						If Not IDisposable_DisposedCalled Then

							IDisposable_DisposedCalled = True

							If Not InputReader Is Nothing Then CType(InputReader, IDisposable).Dispose()


							If Not OutputWriter Is Nothing Then CType(OutputWriter, IDisposable).Dispose()

						End If

					End Sub

				#End Region

			#End Region

		#End Region

		#Region " Public Constants "

			''' <summary>Public Shared Reference to the Name of the Property: InputReader</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_INPUTREADER As String = "InputReader"

			''' <summary>Public Shared Reference to the Name of the Property: OutputWriter</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_OUTPUTWRITER As String = "OutputWriter"

			''' <summary>Public Shared Reference to the Name of the Property: Parser</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_PARSER As String = "Parser"

			''' <summary>Public Shared Reference to the Name of the Property: Formatting_Value</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_FORMATTING_VALUE As String = "Formatting_Value"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: InputReader</summary>
			''' <remarks></remarks>
			Private m_InputReader As System.Xml.XmlTextReader

			''' <summary>Private Data Storage Variable for Property: OutputWriter</summary>
			''' <remarks></remarks>
			Private m_OutputWriter As System.Xml.XmlTextWriter

			''' <summary>Private Data Storage Variable for Property: Parser</summary>
			''' <remarks></remarks>
			Private m_Parser As FromString

			''' <summary>Private Data Storage Variable for Property: Formatting_Value</summary>
			''' <remarks></remarks>
			Private m_Formatting_Value As System.Object

		#End Region

		#Region " Public Properties "

			''' <summary>Provides Access to the Property: InputReader</summary>
			''' <remarks></remarks>
			Public ReadOnly Property InputReader() As System.Xml.XmlTextReader
				Get
					Return m_InputReader
				End Get
			End Property

			''' <summary>Provides Access to the Property: OutputWriter</summary>
			''' <remarks></remarks>
			Public ReadOnly Property OutputWriter() As System.Xml.XmlTextWriter
				Get
					Return m_OutputWriter
				End Get
			End Property

			''' <summary>Provides Access to the Property: Parser</summary>
			''' <remarks></remarks>
			Public ReadOnly Property Parser() As FromString
				Get
					Return m_Parser
				End Get
			End Property

			''' <summary>Provides Access to the Property: Formatting_Value</summary>
			''' <remarks></remarks>
			Public Property Formatting_Value() As System.Object
				Get
					Return m_Formatting_Value
				End Get
				Set(value As System.Object)
					m_Formatting_Value = value
				End Set
			End Property

		#End Region

	End Class

End Namespace