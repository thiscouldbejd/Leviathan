Namespace Inspection

	''' <summary>This attribute should declare the Type of object that is encapsulated by the collection.</summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:52:05</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Inspection\Querying\Generated\AnalyserQuery.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Inspection\Querying\Generated\AnalyserQuery.tt", "1")> _
	Partial Public Class AnalyserQuery
		Inherits System.Object

		#Region " Public Constructors "

			''' <summary>Default Constructor</summary>
			Public Sub New()

				MyBase.New()

				m_Parts = New System.Collections.ArrayList
			End Sub

			''' <summary>Parametered Constructor (1 Parameters)</summary>
			Public Sub New( _
				ByVal _ReturnType As AnalyserType _
			)

				MyBase.New()

				ReturnType = _ReturnType

				m_Parts = New System.Collections.ArrayList
			End Sub

			''' <summary>Parametered Constructor (2 Parameters)</summary>
			Public Sub New( _
				ByVal _ReturnType As AnalyserType, _
				ByVal _DeclaredBelowType As System.Type _
			)

				MyBase.New()

				ReturnType = _ReturnType
				DeclaredBelowType = _DeclaredBelowType

				m_Parts = New System.Collections.ArrayList
			End Sub

			''' <summary>Parametered Constructor (3 Parameters)</summary>
			Public Sub New( _
				ByVal _ReturnType As AnalyserType, _
				ByVal _DeclaredBelowType As System.Type, _
				ByVal _NumberOfResults As System.Int32 _
			)

				MyBase.New()

				ReturnType = _ReturnType
				DeclaredBelowType = _DeclaredBelowType
				NumberOfResults = _NumberOfResults

				m_Parts = New System.Collections.ArrayList
			End Sub

			''' <summary>Parametered Constructor (4 Parameters)</summary>
			Public Sub New( _
				ByVal _ReturnType As AnalyserType, _
				ByVal _DeclaredBelowType As System.Type, _
				ByVal _NumberOfResults As System.Int32, _
				ByVal _Parts As System.Collections.ArrayList _
			)

				MyBase.New()

				ReturnType = _ReturnType
				DeclaredBelowType = _DeclaredBelowType
				NumberOfResults = _NumberOfResults
				Parts = _Parts

			End Sub

		#End Region

		#Region " Class Plumbing/Interface Code "

			#Region " Settable Implementation "

				#Region " Public Methods "

					Public Function SetReturnType(_ReturnType As AnalyserType) As AnalyserQuery

						ReturnType = _ReturnType
						Return Me

					End Function

					Public Function SetDeclaredBelowType(_DeclaredBelowType As System.Type) As AnalyserQuery

						DeclaredBelowType = _DeclaredBelowType
						Return Me

					End Function

					Public Function SetNumberOfResults(_NumberOfResults As System.Int32) As AnalyserQuery

						NumberOfResults = _NumberOfResults
						Return Me

					End Function

					Public Function SetParts(_Parts As System.Collections.ArrayList) As AnalyserQuery

						Parts = _Parts
						Return Me

					End Function

				#End Region

			#End Region

		#End Region

		#Region " Public Constants "

			''' <summary>Public Shared Reference to the Name of the Property: ReturnType</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_RETURNTYPE As String = "ReturnType"

			''' <summary>Public Shared Reference to the Name of the Property: DeclaredBelowType</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_DECLAREDBELOWTYPE As String = "DeclaredBelowType"

			''' <summary>Public Shared Reference to the Name of the Property: NumberOfResults</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_NUMBEROFRESULTS As String = "NumberOfResults"

			''' <summary>Public Shared Reference to the Name of the Property: Parts</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_PARTS As String = "Parts"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: ReturnType</summary>
			''' <remarks></remarks>
			Private m_ReturnType As AnalyserType

			''' <summary>Private Data Storage Variable for Property: DeclaredBelowType</summary>
			''' <remarks></remarks>
			Private m_DeclaredBelowType As System.Type

			''' <summary>Private Data Storage Variable for Property: NumberOfResults</summary>
			''' <remarks></remarks>
			Private m_NumberOfResults As System.Int32

			''' <summary>Private Data Storage Variable for Property: Parts</summary>
			''' <remarks></remarks>
			Private m_Parts As System.Collections.ArrayList

		#End Region

		#Region " Public Properties "

			''' <summary>Provides Access to the Property: ReturnType</summary>
			''' <remarks></remarks>
			Public Property ReturnType() As AnalyserType
				Get
					Return m_ReturnType
				End Get
				Set(value As AnalyserType)
					m_ReturnType = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: DeclaredBelowType</summary>
			''' <remarks></remarks>
			Public Property DeclaredBelowType() As System.Type
				Get
					Return m_DeclaredBelowType
				End Get
				Set(value As System.Type)
					m_DeclaredBelowType = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: NumberOfResults</summary>
			''' <remarks></remarks>
			Public Property NumberOfResults() As System.Int32
				Get
					Return m_NumberOfResults
				End Get
				Set(value As System.Int32)
					m_NumberOfResults = value
				End Set
			End Property

			''' <summary>Provides Access to the Property: Parts</summary>
			''' <remarks></remarks>
			Public Property Parts() As System.Collections.ArrayList
				Get
					Return m_Parts
				End Get
				Set(value As System.Collections.ArrayList)
					m_Parts = value
				End Set
			End Property

		#End Region

	End Class

End Namespace