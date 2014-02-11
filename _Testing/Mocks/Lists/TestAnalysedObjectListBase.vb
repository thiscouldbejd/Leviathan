#If TESTING Then

Public Class TestAnalysedObjectListBase
	Implements IList

	#Region " IList Implementation "

		Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
		End Sub

		Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
			Get
				Return 0
			End Get
		End Property

		Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
			Get
				Return True
			End Get
		End Property

		Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
			Get
				Return Nothing
			End Get
		End Property

		Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
			Return Nothing
		End Function

		Public Function Add(ByVal value As Object) As Integer Implements System.Collections.IList.Add
			Return 0
		End Function

		Public Sub Clear() Implements System.Collections.IList.Clear
		End Sub

		Public Function Contains(ByVal value As Object) As Boolean Implements System.Collections.IList.Contains
			Return False
		End Function

		Public Function IndexOf(ByVal value As Object) As Integer Implements System.Collections.IList.IndexOf
			Return -1
		End Function

		Public Sub Insert(ByVal index As Integer, ByVal value As Object) Implements System.Collections.IList.Insert
		End Sub

		Public ReadOnly Property IsFixedSize() As Boolean Implements System.Collections.IList.IsFixedSize
			Get
				Return False
			End Get
		End Property

		Public ReadOnly Property IsReadOnly() As Boolean Implements System.Collections.IList.IsReadOnly
			Get
				Return False
			End Get
		End Property

		Default Public Overridable Property Item(ByVal index As Integer) As Object Implements System.Collections.IList.Item
			Get
				Return Nothing
			End Get
			Set(ByVal value As Object)
			End Set
		End Property

		Public Sub Remove(ByVal value As Object) Implements System.Collections.IList.Remove
		End Sub

		Public Sub RemoveAt(ByVal index As Integer) Implements System.Collections.IList.RemoveAt
		End Sub

	#End Region

End Class

#End If