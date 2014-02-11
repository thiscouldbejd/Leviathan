Imports System.Collections
Imports System.ComponentModel
Imports System.Data

Namespace Collections

	Partial Public Class DataCollection(Of T)
		Implements IEnumerable(Of T)
		Implements IEnumerator(Of T)
		Implements ITypedList

		#Region " Private Methods "

			Private Sub PrepareReader()

				' Set up the reader, closing any existing readers if required
				If Not m_Reader Is Nothing Then m_Reader.Close()
				If m_Command.Connection.State = System.Data.ConnectionState.Closed Then m_Command.Connection.Open()
				m_Reader = m_Command.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
				m_Index = -1

				PrepareMappings(m_Reader)

				m_Initialised = True

			End Sub

		#End Region

		#Region " Protected Methods "

			Protected MustOverride Sub PrepareMappings( _
				ByVal reader As IDataReader _
			)

			Protected MustOverride Function HydrateObject( _
				ByVal reader As IDataReader _
			)

		#End Region

		#Region " IEnumerator Implementation "

			Private ReadOnly Property Current_UnTyped() As Object _
				Implements IEnumerator.Current
				Get
					Return Current
				End Get
			End Property

			Private Function MoveNext() As Boolean _
				Implements IEnumerator(Of T).MoveNext

				If Not m_Initialised Then PrepareReader()

				If Not m_Current Is Nothing Then m_Previous.Add(m_Current)

				If m_Reader.Read() Then

					m_Current = HydrateObject(m_Reader)

					m_Index += 1

					Return True

				Else

					m_Current = Nothing
					Return False

				End If

			End Function

			Private Sub Reset() _
				Implements IEnumerator(Of T).Reset

				m_Previous.Clear()
				PrepareReader()

			End Sub

		#End Region

		#Region " IEnumerable Implementation "

			Private Function GetEnumerator_UnTyped As IEnumerator _
				Implements IEnumerable.GetEnumerator

				Return GetEnumerator()

			End Function

			Private Function GetEnumerator As IEnumerator(Of T) _
				Implements IEnumerable(Of T).GetEnumerator

				If System.Threading.Thread.CurrentThread.ManagedThreadId = OwnerThread AndAlso Index = -1 Then

					Return Me

				Else

					Return Activator.CreateInstance(Me.GetType, m_Command)

				End If

			End Function

		#End Region

		#Region " IDisposable Implementation "

			Private Sub Dispose() _
			Implements IEnumerator(Of T).Dispose

				If Not Reader Is Nothing Then Reader.Close()

			End Sub

		#End Region

		#Region " ITypedList Implementation "

			Public Function GetItemProperties( _
				ByVal listAccessors() As System.ComponentModel.PropertyDescriptor _
			) As System.ComponentModel.PropertyDescriptorCollection _
			Implements System.ComponentModel.ITypedList.GetItemProperties

				Return System.ComponentModel.TypeDescriptor.GetProperties(GetType(T))

			End Function

			Public Function GetListName( _
				ByVal listAccessors() As System.ComponentModel.PropertyDescriptor _
			) As String _
			Implements System.ComponentModel.ITypedList.GetListName

				Return Leviathan.Inspection.TypeAnalyser.GetInstance(GetType(T)).DisplayName()

			End Function

		#End Region

	End Class

End Namespace