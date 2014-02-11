Namespace Caching

	''' <summary>Simple Caching Class</summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:40:09</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Caching\Generated\Simple.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Caching\Generated\Simple.tt", "1")> _
	Partial Public Class Simple
		Inherits System.Object

		#Region " Singleton Implementation "

			#Region " Private Constructors "

				''' <summary>Parametered Constructor (1 Parameters)</summary>
				Private Sub New( _
					ByVal _TypeHashCode As System.Int32 _
				)

					MyBase.New()

					m_TypeHashCode = _TypeHashCode

					m_Cache = New System.Collections.Hashtable
				End Sub

				''' <summary>Parametered Constructor (2 Parameters)</summary>
				Private Sub New( _
					ByVal _TypeHashCode As System.Int32, _
					ByVal _RaisesEvents As System.Boolean _
				)

					MyBase.New()

					m_TypeHashCode = _TypeHashCode
					m_RaisesEvents = _RaisesEvents

					m_Cache = New System.Collections.Hashtable
				End Sub

				''' <summary>Parametered Constructor (3 Parameters)</summary>
				Private Sub New( _
					ByVal _TypeHashCode As System.Int32, _
					ByVal _RaisesEvents As System.Boolean, _
					ByVal _Cache As System.Collections.Hashtable _
				)

					MyBase.New()

					m_TypeHashCode = _TypeHashCode
					m_RaisesEvents = _RaisesEvents
					m_Cache = _Cache

				End Sub

			#End Region

			#Region " Private Shared Variables "

				Private Shared Singleton_Instances As New Hashtable

				Private Shared Singleton_Instances_LOCK As New Object

			#End Region

			#Region " Public Shared Functions "

				Public Shared Function GetInstance( _
					ByVal _TypeHashCode As System.Int32 _
				) As Simple

					Dim instance_Hashcode As Integer = _
						Leviathan.Caching.Simple.CombineHashCodes(_TypeHashCode)

					SyncLock Singleton_Instances_LOCK

						If Not Singleton_Instances.Contains(instance_Hashcode) Then Singleton_Instances(instance_Hashcode) = _
							New Simple(_TypeHashCode)

					End SyncLock

					Return Singleton_Instances(instance_Hashcode)

				End Function

				Public Shared Function GetInstance( _
					ByVal _TypeHashCode As System.Int32, _
					ByVal _RaisesEvents As System.Boolean _
				) As Simple

					Dim instance_Hashcode As Integer = _
						Leviathan.Caching.Simple.CombineHashCodes(_TypeHashCode, _RaisesEvents)

					SyncLock Singleton_Instances_LOCK

						If Not Singleton_Instances.Contains(instance_Hashcode) Then Singleton_Instances(instance_Hashcode) = _
							New Simple(_TypeHashCode, _RaisesEvents)

					End SyncLock

					Return Singleton_Instances(instance_Hashcode)

				End Function

				Public Shared Function GetInstance( _
					ByVal _TypeHashCode As System.Int32, _
					ByVal _RaisesEvents As System.Boolean, _
					ByVal _Cache As System.Collections.Hashtable _
				) As Simple

					Dim instance_Hashcode As Integer = _
						Leviathan.Caching.Simple.CombineHashCodes(_TypeHashCode, _RaisesEvents, _Cache)

					SyncLock Singleton_Instances_LOCK

						If Not Singleton_Instances.Contains(instance_Hashcode) Then Singleton_Instances(instance_Hashcode) = _
							New Simple(_TypeHashCode, _RaisesEvents, _Cache)

					End SyncLock

					Return Singleton_Instances(instance_Hashcode)

				End Function


			#End Region

		#End Region

		#Region " Class Plumbing/Interface Code "

			#Region " Thread-Safe Updates Implementation "

				#Region " Private Variables "

					''' <summary></summary>
					''' <remarks></remarks>
					Private Cache_LOCK As New System.Object

					''' <summary></summary>
					''' <remarks></remarks>
					Private Cache_HASVALUE As System.Boolean

				#End Region

				#Region " Public Update Methods "

					Public Function ThreadSafeUpdateCache( _
						ByVal _Cache As System.Collections.Hashtable _
					) As Boolean

						SyncLock Cache_LOCK
							If Not Cache_HASVALUE OrElse Not Leviathan.Comparison.Comparer.AreEqual(Cache, _Cache) Then
								m_Cache = _Cache
								Cache_HASVALUE = True
								Return True
							Else
								Return False
							End If
						End SyncLock

					End Function

				#End Region

			#End Region

		#End Region

		#Region " Public Constants "

			''' <summary>Public Shared Reference to the Name of the Property: TypeHashCode</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_TYPEHASHCODE As String = "TypeHashCode"

			''' <summary>Public Shared Reference to the Name of the Property: RaisesEvents</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_RAISESEVENTS As String = "RaisesEvents"

			''' <summary>Public Shared Reference to the Name of the Property: Cache</summary>
			''' <remarks></remarks>
			Public Const PROPERTY_CACHE As String = "Cache"

		#End Region

		#Region " Private Variables "

			''' <summary>Private Data Storage Variable for Property: TypeHashCode</summary>
			''' <remarks></remarks>
			Private m_TypeHashCode As System.Int32

			''' <summary>Private Data Storage Variable for Property: RaisesEvents</summary>
			''' <remarks></remarks>
			Private m_RaisesEvents As System.Boolean

			''' <summary>Private Data Storage Variable for Property: Cache</summary>
			''' <remarks></remarks>
			Private m_Cache As System.Collections.Hashtable

		#End Region

		#Region " Public Properties "

			''' <summary>Provides Access to the Property: TypeHashCode</summary>
			''' <remarks></remarks>
			Public ReadOnly Property TypeHashCode() As System.Int32
				Get
					Return m_TypeHashCode
				End Get
			End Property

			''' <summary>Provides Access to the Property: RaisesEvents</summary>
			''' <remarks></remarks>
			Public ReadOnly Property RaisesEvents() As System.Boolean
				Get
					Return m_RaisesEvents
				End Get
			End Property

			''' <summary>Provides Access to the Property: Cache</summary>
			''' <remarks></remarks>
			Public ReadOnly Property Cache() As System.Collections.Hashtable
				Get
					Return m_Cache
				End Get
			End Property

		#End Region

	End Class

End Namespace