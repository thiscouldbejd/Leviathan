Imports System.Collections
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Xml.Serialization

Namespace Collections

	<Serializable(), ElementType(GetType(Object)), Name( _
			ResourceName:="AttributeExpansions", _
			ResourceContainingType:=GetType(GeneralCollection), _
			Name:="@nameDescriptionGeneralCollection@" _
	)> _
	Public Class GeneralCollection
		Implements IList
		Implements IBindingList
		Implements ITypedList
		Implements IComponent
		Implements IXmlSerializable

		#Region " Class Plumbing/Interface Code "

		#Region " IBindingList Implementation "

		#Region " Private Variables "

		''' <summary>
		''' Private Reference to whether a ListChanged event is raised when this collection changes or an item in the list changes.
		''' </summary>
		''' <remarks>For <see cref="IBindingList"/> Implementation.</remarks>
		Private m_SupportsChangeNotification As Boolean = True

		''' <summary>
		''' Private Reference to whether this collection supports searching using the Find method.
		''' </summary>
		''' <remarks>For <see cref="IBindingList"/> Implementation.</remarks>
		Private m_SupportsSearching As Boolean = True

		''' <summary>
		''' Private Reference to whether this collection supports sorting.
		''' </summary>
		''' <remarks>For <see cref="IBindingList"/> Implementation.</remarks>
		Private m_SupportsSorting As Boolean = True

		''' <summary>
		''' Reference to Whether Set/Edits Are Allowed by the Collection.
		''' </summary>
		''' <remarks>For <see cref="IBindingList"/> Implementation.</remarks>
		Private m_AllowEdit As Boolean = True
		
		''' <summary>
		''' Reference to Whether Adds Are Allowed by the Collection.
		''' </summary>
		''' <remarks>For <see cref="IBindingList"/> Implementation.</remarks>
		Private m_AllowNew As Boolean = True
		
		''' <summary>
		''' Reference to Whether Removes Are Allowed by the Collection.
		''' </summary>
		''' <remarks>For <see cref="IBindingList"/> Implementation.</remarks>
		Private m_AllowRemove As Boolean = True
		
		#End Region
		
		#Region " Public Events "
		
		''' <summary>
		''' Occurs when the list changes or an item in the list changes.
		''' </summary>
		''' <remarks>
		''' This event is raised if the underling IList object's data changes
		''' (assuming the underling IList also implements the IBindingList
		''' interface). It is also raised if the sort property or direction
		''' is changed to indicate that the view's data has changed.
		''' </remarks>
		Public Event ListChanged( _
			ByVal sender As Object, _
			ByVal e As System.ComponentModel.ListChangedEventArgs _
			) _
			Implements System.ComponentModel.IBindingList.ListChanged
		
		#End Region
		
		#Region " Public Properties "
		
		''' <summary>
		''' Gets whether you can update items in the list.
		''' </summary>
		''' <returns>true if you can update the items in the list; otherwise, false.</returns>
		Public ReadOnly Property AllowEdit() As Boolean _
			Implements System.ComponentModel.IBindingList.AllowEdit
			Get
				
				Return m_AllowEdit
				
			End Get
		End Property
		
		''' <summary>
		''' Gets whether you can add items to the list using AddNew.
		''' </summary>
		''' <returns>true if you can add items to the list using AddNew; otherwise, false.</returns>
		Public ReadOnly Property AllowNew() As Boolean _
			Implements System.ComponentModel.IBindingList.AllowNew
			Get
				
				Return m_AllowNew
				
			End Get
		End Property
		
		''' <summary>
		''' Gets whether you can remove items from the list, using Remove or RemoveAt.
		''' </summary>
		''' <returns>true if you can remove items from the list; otherwise, false.</returns>
		Public ReadOnly Property AllowRemove() As Boolean _
			Implements System.ComponentModel.IBindingList.AllowRemove
			Get
				
				Return m_AllowRemove
				
			End Get
		End Property
		
		''' <summary>
		''' Gets whether the items in the list are sorted.
		''' </summary>
		''' <value></value>
		''' <returns>true if ApplySort has been called and RemoveSort has not been called; otherwise, false.</returns>
		''' <exception cref="NotSupportedException">SupportsSorting is false.</exception>
		''' <remarks></remarks>
		Public ReadOnly Property IsSorted() As Boolean _
			Implements System.ComponentModel.IBindingList.IsSorted
			Get
				
				If SupportsSorting Then
					
					Return ((Not SortProperty Is Nothing) OrElse (Not SortProperties Is Nothing)) AndAlso Sorted
					
				Else
					
					Throw New NotSupportedException
					
				End If
				
			End Get
		End Property
		
		''' <summary>
		''' Gets the direction of the sort.
		''' </summary>
		''' <value></value>
		''' <returns>One of the ListSortDirection values.</returns>
		''' <exception cref="NotSupportedException">SupportsSorting is false.</exception>
		''' <remarks></remarks>
		Public ReadOnly Property SortDirection() As System.ComponentModel.ListSortDirection _
			Implements System.ComponentModel.IBindingList.SortDirection
			Get
				
				If SupportsSorting Then
					
					Return m_SortDirection
					
				Else
					
					Throw New NotSupportedException
					
				End If
				
			End Get
		End Property
		
		''' <summary>
		''' Gets the PropertyDescriptor that is being used for sorting.
		''' </summary>
		''' <value></value>
		''' <returns>The PropertyDescriptor that is being used for sorting.</returns>
		''' <exception cref="NotSupportedException">SupportsSorting is false.</exception>
		''' <remarks></remarks>
		Public ReadOnly Property SortProperty() As System.ComponentModel.PropertyDescriptor _
			Implements System.ComponentModel.IBindingList.SortProperty
			Get
				
				If SupportsSorting Then
					
					If Not SortProperties Is Nothing AndAlso SortProperties.Length > 0 Then
						
						Dim props As PropertyDescriptorCollection = TypeDescriptor.GetProperties(ElementType)
						
						For Each prop As PropertyDescriptor In props
							
							If String.Compare(prop.Name, SortProperties(0).ToString, True) = 0 Then
								
								Return prop
								
							End If
							
						Next
						
					End If
					
				Else
					
					Throw New NotSupportedException
					
				End If
				
				Return Nothing
				
			End Get
		End Property
		
		''' <summary>
		''' Gets whether a ListChanged event is raised when the list changes or an item in the list changes.
		''' </summary>
		''' <returns>true if a ListChanged event is raised when the list changes or when an item changes; otherwise, false.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property SupportsChangeNotification() As Boolean _
			Implements System.ComponentModel.IBindingList.SupportsChangeNotification
			Get
				
				Return m_SupportsChangeNotification
				
			End Get
		End Property
		
		''' <summary>
		''' Gets whether the list supports searching using the Find method.
		''' </summary>
		''' <returns>true if the list supports searching using the Find method; otherwise, false.</returns>
		Public ReadOnly Property SupportsSearching() As Boolean _
			Implements System.ComponentModel.IBindingList.SupportsSearching
			Get
				
				Return m_SupportsSearching
				
			End Get
		End Property
		
		''' <summary>
		''' Gets whether the list supports sorting.
		''' </summary>
		''' <returns>true if the list supports sorting; otherwise, false.</returns>
		Public ReadOnly Property SupportsSorting() As Boolean _
			Implements System.ComponentModel.IBindingList.SupportsSorting
			Get
				
				Return m_SupportsSorting
				
			End Get
		End Property
		
		#End Region
		
		#Region " Public Methods "
		
		''' <summary>
		''' Adds the PropertyDescriptor to the indexes used for searching.
		''' </summary>
		''' <param name="property">The PropertyDescriptor to add to the indexes used for searching.</param>
		''' <remarks>This Method is Not Implemented.</remarks>
		Public Sub AddIndex( _
			ByVal [property] As System.ComponentModel.PropertyDescriptor _
			) _
			Implements System.ComponentModel.IBindingList.AddIndex
			
			Throw New NotImplementedException
			
		End Sub
		
		''' <summary>
		''' Adds a new item to the list.
		''' </summary>
		''' <returns>The item added to the list.</returns>
		''' <exception cref="NotSupportedException">AllowNew is false.</exception>
		''' <remarks></remarks>
		Public Function AddNew() As Object _
			Implements System.ComponentModel.IBindingList.AddNew
			
			Dim new_Object As Object = TypeAnalyser.Create(ElementType)
			
			If Add(new_Object) >= 0 Then
				
				Return new_Object
				
			Else
				
				Return Nothing
				
			End If
			
		End Function
		
		''' <summary>
		''' Sorts the list based on a PropertyDescriptor and a ListSortDirection.
		''' </summary>
		''' <param name="property">A PropertyDescriptor for the property upon which to sort.</param>
		''' <param name="direction">The direction in which to sort the Collection.</param>
		''' <exception cref="NotSupportedException">SupportsSorting is false.</exception>
		Public Sub ApplySort( _
			ByVal [property] As System.ComponentModel.PropertyDescriptor, _
			ByVal direction As System.ComponentModel.ListSortDirection _
			) _
			Implements System.ComponentModel.IBindingList.ApplySort
			
			If Not [property] Is Nothing Then
				
				ApplySort([property].Name, direction)
				
			End If
			
		End Sub
		
		''' <summary>
		''' Sorts the list based on a Property Name and a ListSortDirection.
		''' </summary>
		''' <param name="property">A Name for the property upon which to sort.</param>
		''' <param name="direction">The direction in which to sort the Collection.</param>
		''' <exception cref="NotSupportedException">SupportsSorting is false.</exception>
		Public Sub ApplySort( _
			ByVal [property] As String, _
			ByVal direction As System.ComponentModel.ListSortDirection _
			)
			
			If SupportsSorting Then
				
				If Not String.IsNullOrEmpty([property]) Then
					
					Dim readableProp As New MemberAnalyser([property])
					
					readableProp.Parse(Nothing, Nothing, TypeAnalyser.GetInstance(Me.GetType).ElementTypeAnalyser)
					
					If Not readableProp Is Nothing Then
						
						m_SortProperties = New MemberAnalyser() {readableProp}
						m_SortDirection = direction
						
						Dim aryValue As Array = Array.CreateInstance(ElementType, Count)
						CopyTo(aryValue, 0)
						
						Array.Sort(aryValue, New Comparison.Comparer( _
							SortDirection, SortProperties))
						
						For i As Integer = 0 To aryValue.Length - 1
							Me.Item(i) = aryValue(i)
						Next
						
						Sorted = True
						
					End If
					
					
				End If
				
			Else
				
				Throw New NotSupportedException
				
			End If
			
		End Sub
		
		''' <summary>
		''' Returns the index of the row that has the given PropertyDescriptor.
		''' </summary>
		''' <param name="property">The PropertyDescriptor to search on.</param>
		''' <param name="key">The value of the property parameter to search for.</param>
		''' <returns>The index of the row that has the given PropertyDescriptor.</returns>
		''' <exception cref="NotSupportedException">SupportsSearching is false.</exception>
		''' <remarks></remarks>
		Public Function Find( _
			ByVal [property] As System.ComponentModel.PropertyDescriptor, _
			ByVal key As Object _
			) As Integer _
			Implements System.ComponentModel.IBindingList.Find
			
			If SupportsSearching Then
				
				Dim l_comparer As New Comparison.Comparer()
				
				For i As Integer = 0 To Count - 1
					
					If l_comparer.Compare([property].GetValue(Me.Item(i)), key) = 0 Then _
						Return i
					
				Next
				
				Return -1
				
			Else
				
				Throw New NotSupportedException
				
			End If
			
		End Function
		
		''' <summary>
		''' Removes the PropertyDescriptor from the indexes used for searching.
		''' </summary>
		''' <param name="property">The PropertyDescriptor to remove from the indexes used for searching.</param>
		''' <remarks>This Method is Not Implemented.</remarks>
		Public Sub RemoveIndex( _
			ByVal [property] As System.ComponentModel.PropertyDescriptor _
			) _
			Implements System.ComponentModel.IBindingList.RemoveIndex
			
			Throw New NotImplementedException
			
		End Sub
		
		''' <summary>
		''' Removes any sort applied using ApplySort.
		''' </summary>
		''' <exception cref="NotSupportedException">SupportsSorting is false.</exception>
		''' <remarks></remarks>
		Public Sub RemoveSort() _
			Implements System.ComponentModel.IBindingList.RemoveSort
			
			If SupportsSorting Then
				
				m_SortProperties = Nothing
				m_SortDirection = Nothing
				
				Sorted = False
				
			Else
				
				Throw New NotSupportedException
				
			End If
			
		End Sub
		
		#End Region
		
		#End Region
		
		#Region " ICollection Implementation "
		
		#Region " Public Properties "
		
		''' <summary>
		''' Gets the number of elements contained in the ICollection.
		''' </summary>
		''' <value></value>
		''' <returns>The number of elements contained in the ICollection.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property Count() As Integer _
			Implements System.Collections.ICollection.Count
			Get
				
			If List Is Nothing Then Return 0 Else Return List.Count
				
			End Get
		End Property
		
		''' <summary>
		''' Gets a value indicating whether access to the ICollection is synchronized (thread safe).
		''' </summary>
		''' <value></value>
		''' <returns>True if access to the ICollection is synchronized (thread safe); otherwise, False.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property IsSynchronized() As Boolean _
			Implements System.Collections.ICollection.IsSynchronized
			Get
				
				Return List.IsSynchronized
				
			End Get
		End Property
		
		''' <summary>
		''' Gets an object that can be used to synchronize access to the ICollection.
		''' </summary>
		''' <value></value>
		''' <returns>An object that can be used to synchronize access to the ICollection.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property SyncRoot() As Object _
			Implements System.Collections.ICollection.SyncRoot
			Get
				
				Return List.SyncRoot
				
			End Get
		End Property
		
		#End Region
		
		#Region " Public Methods "
		
		''' <summary>
		''' Copies the elements of the ICollection to an Array, starting at a particular Array index.
		''' </summary>
		''' <param name="array">The one-dimensional Array that is the destination of the elements copied from ICollection.
		''' The Array must have zero-based indexing.</param>
		''' <param name="index">The zero-based index in array at which copying begins.</param>
		''' <exception cref="ArgumentNullException">array is null.</exception>
		''' <exception cref="ArgumentOutOfRangeException">The type of the source ICollection cannot be cast automatically to the type of the destination array.</exception>
		''' <exception cref="ArgumentException">index is less than zero.</exception>
		''' <exception cref="ArgumentException">array is multidimensional.-or- index is equal to or greater than the length of array.
		''' -or- The number of elements in the source ICollection is greater than the available space from index to the end of the destination array.</exception>
		Public Sub CopyTo( _
			ByVal array As System.Array, _
			ByVal index As Integer _
			) _
			Implements System.Collections.ICollection.CopyTo
			
			List.CopyTo(array, index)
			
		End Sub
		
		#End Region
		
		#End Region
		
		#Region " IComponent Implementation "
		
		#Region " Private Variables "
		
		''' <summary>
		''' Reference to the ISite associated with the IComponent.
		''' </summary>
		''' <remarks></remarks>
		Private m_Site As ISite = Nothing
		
		#End Region
		
		#Region " Public Events "
		
		''' <summary>
		''' The Public Event Raised When the IComponent is Disposed.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Public Event Disposed( _
			ByVal sender As Object, _
			ByVal e As System.EventArgs _
			) _
			Implements System.ComponentModel.IComponent.Disposed
		
		#End Region
		
		#Region " Public Properties "
		
		''' <summary>
		''' Gets or sets the ISite associated with the IComponent.
		''' </summary>
		''' <value></value>
		''' <returns>The ISite object associated with the component; or null, if the component does not have a site.</returns>
		''' <remarks></remarks>
		Public Property Site() As System.ComponentModel.ISite Implements System.ComponentModel.IComponent.Site
			Get
				Return m_Site
			End Get
			Set(ByVal value As System.ComponentModel.ISite)
				m_Site = value
			End Set
		End Property
		
		#End Region
		
		#End Region
		
		#Region " IDisposable Implementation "
		
		#Region " Private Variables "
		
		Private disposedValue As Boolean = False        ' To detect redundant calls
		
		#End Region
		
		#Region " Protected Methods "
		
		' IDisposable
		Protected Overridable Sub Dispose(ByVal disposing As Boolean)
			If Not Me.disposedValue Then
				If disposing Then
					' TODO: free other state (managed objects).
				End If
				
				' TODO: free your own state (unmanaged objects).
				' TODO: set large fields to null.
			End If
			Me.disposedValue = True
		End Sub
		
		#End Region
		
		#Region " Public Methods "
		
		' This code added by Visual Basic to correctly implement the disposable pattern.
		Public Sub Dispose() Implements IDisposable.Dispose
			' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub
		
		#End Region
		
		#End Region
		
		#Region " IEnumerable Implementation "
		
		#Region " Public Methods "
		
		''' <summary>
		''' Returns an enumerator that iterates through a collection.
		''' </summary>
		''' <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
		''' <remarks>If the view is sorted, the enumerator provides a sorted
		''' view of the data. If the view is not sorted, the enumerator
		''' from the underlying IList object is used.</remarks>
		Public Function GetEnumerator() As System.Collections.IEnumerator _
			Implements System.Collections.IEnumerable.GetEnumerator
			
			Return List.GetEnumerator
			
		End Function
		
		#End Region
		
		#End Region
		
		#Region " IList Implementation "
		
		#Region " Public Properties "
		
		''' <summary>
		''' Gets a value indicating whether the IList has a fixed size.
		''' </summary>
		''' <returns>True if the IList has a fixed size; otherwise, False.</returns>
		Public ReadOnly Property IsFixedSize() As Boolean _
			Implements System.Collections.IList.IsFixedSize
			Get
				
				Return List.IsFixedSize
				
			End Get
		End Property
		
		''' <summary>
		''' Gets a value indicating whether the IList is read-only.
		''' </summary>
		''' <returns>True if the IList is read-only; otherwise, False.</returns>
		Public ReadOnly Property IsReadOnly() As Boolean _
			Implements System.Collections.IList.IsReadOnly
			Get
				
				Return List.IsReadOnly
				
			End Get
		End Property
		
		''' <summary>
		''' Gets or sets the element at the specified index.
		''' </summary>
		''' <remarks>
		''' <para>
		''' If the list is sorted, this returns the items in the appropriate sorted
		''' order. Otherwise the order will match that of the underlying IList object.
		''' </para>
		''' </remarks>
		''' <param name="index">The zero-based index of the element to get or set.</param>
		''' <exception cref="ArgumentOutOfRangeException">index is not a valid index in the IList.</exception>
		''' <exception cref="NotSupportedException">The property is set and the IList is read-only.</exception>
		''' <returns>The element at the specified index.</returns>
		Default Public Property Item( _
			ByVal index As Integer _
			) As Object _
			Implements System.Collections.IList.Item
			Get
				
				Return List.Item(index)
				
			End Get
			Set(ByVal value As Object)
				
				If OnSet(value) Then
					
					List.Item(index) = value
					
				End If
				
			End Set
		End Property
		
		#End Region
		
		#Region " Public Methods "
		
		''' <summary>
		''' Adds an item to the IList.
		''' </summary>
		''' <param name="value">The Object to Add to the Collection</param>
		''' <returns></returns>
		''' <exception cref="NotSupportedException">The IList is read-only.-or- The IList has a fixed size.</exception>
		''' <remarks></remarks>
		Public Function Add( _
			ByVal value As Object _
			) As Integer _
			Implements System.Collections.IList.Add
			
			If Not value Is Nothing _
				AndAlso OnAdd(value) Then
				
				Return List.Add(value)
				
			Else
				
				Return -1
				
			End If
			
		End Function
		
		''' <summary>
		''' Removes all items from the IList.
		''' </summary>
		''' <exception cref="NotSupportedException">The IList is read-only.</exception>
		''' <remarks></remarks>
		Public Sub Clear() _
			Implements System.Collections.IList.Clear
			
			If Not List Is Nothing Then
				
				For Each obj As Object In List
					
					Remove(obj)
					
				Next
				
			End If
			
		End Sub
		
		''' <summary>
		''' Determines whether the IList contains a specific value.
		''' </summary>
		''' <param name="value">The Object to locate in the IList.</param>
		''' <returns>True if the Object is found in the IList; otherwise, False.</returns>
		''' <remarks></remarks>
		Public Function Contains( _
			ByVal value As Object _
			) As Boolean _
			Implements System.Collections.IList.Contains
			
			Return List.Contains(value)
			
		End Function
		
		''' <summary>
		''' Determines the index of a specific item in the IList.
		''' </summary>
		''' <param name="value">The Object to locate in the IList.</param>
		''' <returns>The index of value if found in the list; otherwise, -1.</returns>
		''' <remarks>
		''' If the view is sorted then the index is the index
		''' within the sorted list, not the underlying IList object.
		''' </remarks>
		Public Function IndexOf( _
			ByVal value As Object _
			) As Integer _
			Implements System.Collections.IList.IndexOf
			
			Return List.IndexOf(value)
			
		End Function
		
		''' <summary>
		''' Inserts an item to the IList at the specified index.
		''' </summary>
		''' <param name="index">The zero-based index at which value should be inserted.</param>
		''' <param name="value">The Object to insert into the IList.</param>
		''' <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.IList"></see>. </exception>
		''' <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"></see> is read-only.-or- The <see cref="T:System.Collections.IList"></see> has a fixed size. </exception>
		''' <exception cref="T:System.NullReferenceException">value is null reference in the <see cref="T:System.Collections.IList"></see>.</exception>
		''' <remarks></remarks>
		Public Sub Insert( _
			ByVal index As Integer, _
			ByVal value As Object _
			) _
			Implements System.Collections.IList.Insert
			
			If Not List Is Nothing AndAlso OnInsert(index, value) Then
				
				List.Insert(index, value)
				
			End If
			
		End Sub
		
		''' <summary>
		''' Removes the first occurrence of a specific object from the IList.
		''' </summary>
		''' <param name="value">The Object to remove from the IList.</param>
		''' <remarks></remarks>
		Public Sub Remove( _
			ByVal value As Object _
			) _
			Implements System.Collections.IList.Remove
			
			If Not List Is Nothing AndAlso OnRemove(value) Then
				
				List.Remove(value)
				
			End If
			
		End Sub
		
		''' <summary>
		''' Removes the IList item at the specified index.
		''' </summary>
		''' <param name="index">The zero-based index of the item to remove.</param>
		''' <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the IList.</exception>
		''' <exception cref="NotSupportedException">The IList is read-only.-or- The IList has a fixed size.</exception>
		''' <remarks></remarks>
		Public Sub RemoveAt( _
			ByVal index As Integer _
			) _
			Implements System.Collections.IList.RemoveAt
			
			If Not List Is Nothing AndAlso OnRemove(List.Item(index)) Then
				
				List.RemoveAt(index)
				
			End If
			
		End Sub
		
		#End Region
		
		#End Region
		
		#Region " ITypedList Implementation "
		
		#Region " Public Methods "
		
		''' <summary>
		''' Returns the PropertyDescriptorCollection that represents the properties on each item used to bind data.
		''' </summary>
		''' <param name="listAccessors">An array of PropertyDescriptor objects to find in the collection as bindable. This can be null.</param>
		''' <returns>The PropertyDescriptorCollection that represents the properties on each item used to bind data.</returns>
		''' <remarks></remarks>
		Public Function GetItemProperties( _
			ByVal listAccessors() As System.ComponentModel.PropertyDescriptor _
			) As System.ComponentModel.PropertyDescriptorCollection _
			Implements System.ComponentModel.ITypedList.GetItemProperties
			
			Dim m_OriginalList As PropertyDescriptorCollection = TypeDescriptor.GetProperties(ElementType)
			Dim m_SortedList As PropertyDescriptorCollection = m_OriginalList.Sort( _
				New Comparison.Comparer(ListSortDirection.Ascending, SortProperties))
			Return m_SortedList
			
		End Function
		
		''' <summary>
		''' Returns the name of the list.
		''' </summary>
		''' <param name="listAccessors">An array of PropertyDescriptor objects, for which the list name is returned. This can be null.</param>
		''' <returns>The name of the list.</returns>
		''' <remarks></remarks>
		Public Function GetListName( _
			ByVal listAccessors() As System.ComponentModel.PropertyDescriptor _
			) As String Implements _
			System.ComponentModel.ITypedList.GetListName
			
			Return TypeAnalyser.GetInstance(ElementType).DisplayName()
			
		End Function
		
		#End Region
		
		#End Region
		
		#Region " IXmlSerialisable Implementation "
		
		#Region " Public Methods "
		
		Public Function GetSchema() As System.Xml.Schema.XmlSchema _
			Implements System.Xml.Serialization.IXmlSerializable.GetSchema
			
			Return Configuration.XmlSerialiser.GenerateSchema(Me.GetType)
			
		End Function
		
		Public Sub ReadXml( _
			ByVal reader As System.Xml.XmlReader _
			) Implements System.Xml.Serialization.IXmlSerializable.ReadXml
			
			Configuration.XmlSerialiser.ReadXml(Me, reader)
			
		End Sub
		
		Public Sub WriteXml( _
			ByVal writer As System.Xml.XmlWriter _
			) Implements System.Xml.Serialization.IXmlSerializable.WriteXml
			
			Configuration.XmlSerialiser.WriteXml(Me, writer)
			
		End Sub
		
		#End Region
		
		#End Region
		
		#End Region
		
		#Region " Public Shared Variables "
		
		''' <summary>
		''' Public Shared Reference to the Name of the Property: List
		''' </summary>
		Public Shared PROPERTY_LIST As String = "List"
		
		''' <summary>
		''' Public Shared Reference to the Name of the Property: Sort Properties
		''' </summary>
		Public Shared PROPERTY_SORTPROPERTIES As String = "SortProperties"
		
		''' <summary>
		''' Public Shared Reference to the Name of the Property: Sorted
		''' </summary>
		Public Shared PROPERTY_SORTED As String = "Sorted"
		
		''' <summary>
		''' Public Shared Reference to the Name of the Property: Element Type
		''' </summary>
		Public Shared PROPERTY_ELEMENTTYPE As String = "ElementType"
		
		#End Region
		
		#Region " Private Variables "
		
		''' <summary>
		''' Private Reference to the Internal Wrapped List of Objects.
		''' </summary>
		''' <remarks></remarks>
		Private m_List As IList
		
		''' <summary>
		''' Reference to the Direction of the Sort.
		''' </summary>
		''' <remarks></remarks>
		Private m_SortDirection As ListSortDirection
		
		''' <summary>
		''' Variable Reference to the Multi-Layered Properties upon which to Sort.
		''' </summary>
		''' <remarks></remarks>
		Private m_SortProperties As MemberAnalyser()
		
		''' <summary>
		''' Variable Reference to Whether the Collection has been Sorted.
		''' </summary>
		''' <remarks></remarks>
		Private m_Sorted As Boolean
		
		#End Region
		
		#Region " Protected Properties "
		
		''' <summary>
		''' Provides Access to the Interal Wrapped List of Objects.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<XmlElement(ElementName:="Items")> _
			Protected Property List() As IList
			Get
				Return m_List
			End Get
			Set(ByVal value As IList)
				m_List = value
			End Set
		End Property
		
		''' <summary>
		''' Provides Access to the Multi-Layered Property upon which to Sort.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Property SortProperties() As MemberAnalyser()
			Get
				Return m_SortProperties
			End Get
			Set(ByVal value As MemberAnalyser())
				m_SortProperties = value
			End Set
		End Property
		
		''' <summary>
		''' Provides Access to Whether the Collection has been Sorted.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Property Sorted() As Boolean
			Get
				Return m_Sorted
			End Get
			Set(ByVal value As Boolean)
				m_Sorted = value
			End Set
		End Property
		
		#End Region
		
		#Region " Public Properties "
		
		''' <summary>
		''' Provides Access to the Element Type of this Collection.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ElementType() As System.Type
			Get
				TypeAnalyser.GetElementType(List)
				Return TypeAnalyser.GetInstance(Me.GetType).ElementType
				
			End Get
		End Property
		
		#End Region
		
		#Region " Public Constructors "
		
		''' <summary>
		''' Default Public Constructor to Instantiate this Class.
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
			
			m_List = New ArrayList
			
		End Sub
		
		''' <summary>
		''' Public Constructor to Instantiate this Class.
		''' </summary>
		''' <param name="allowNew">Boolean Value indicating whether new objects can be added to the Collection.</param>
		''' <remarks></remarks>
		Public Sub New( _
			ByVal allowNew As Boolean _
			)
			Me.New()
			m_AllowNew = allowNew
			
		End Sub
		
		''' <summary>
		''' Public Constructor to Instantiate this Class.
		''' </summary>
		''' <param name="allowNew">Boolean Value indicating whether new objects can be added to the Collection.</param>
		''' <param name="allowEdit">Boolean Value indicating whether objects can be edited/set in this Collection.</param>
		''' <remarks></remarks>
		Public Sub New( _
			ByVal allowNew As Boolean, _
			ByVal allowEdit As Boolean _
			)
			
			Me.New(allowNew)
			m_AllowEdit = allowEdit
			
		End Sub
		
		''' <summary>
		''' Public Constructor to Instantiate this Class.
		''' </summary>
		''' <param name="allowNew">Boolean Value indicating whether new objects can be added to the Collection.</param>
		''' <param name="allowEdit">Boolean Value indicating whether objects can be edited/set in this Collection.</param>
		''' <param name="allowRemove">Boolean Value indicating whether objects can be removed from this Collection.</param>
		''' <remarks></remarks>
		Public Sub New( _
			ByVal allowNew As Boolean, _
			ByVal allowEdit As Boolean, _
			ByVal allowRemove As Boolean _
			)
			
			Me.New(allowNew, allowEdit)
			m_AllowRemove = allowRemove
			
		End Sub
		
		''' <summary>
		''' Default Public Constructor to Instantiate this Class.
		''' </summary>
		''' <remarks></remarks>
		Public Sub New( _
			ByVal list As IList _
		)
			
			m_List = list
			
		End Sub
		
		''' <summary>
		''' Public Constructor to Instantiate this Class.
		''' </summary>
		''' <param name="allowNew">Boolean Value indicating whether new objects can be added to the Collection.</param>
		''' <remarks></remarks>
		Public Sub New( _
			ByVal list As IList, _
			ByVal allowNew As Boolean _
			)
			Me.New(list)
			m_AllowNew = allowNew
			
		End Sub
		
		''' <summary>
		''' Public Constructor to Instantiate this Class.
		''' </summary>
		''' <param name="allowNew">Boolean Value indicating whether new objects can be added to the Collection.</param>
		''' <param name="allowEdit">Boolean Value indicating whether objects can be edited/set in this Collection.</param>
		''' <remarks></remarks>
		Public Sub New( _
			ByVal list As IList, _
			ByVal allowNew As Boolean, _
			ByVal allowEdit As Boolean _
			)
			
			Me.New(list, allowNew)
			m_AllowEdit = allowEdit
			
		End Sub
		
		''' <summary>
		''' Public Constructor to Instantiate this Class.
		''' </summary>
		''' <param name="allowNew">Boolean Value indicating whether new objects can be added to the Collection.</param>
		''' <param name="allowEdit">Boolean Value indicating whether objects can be edited/set in this Collection.</param>
		''' <param name="allowRemove">Boolean Value indicating whether objects can be removed from this Collection.</param>
		''' <remarks></remarks>
		Public Sub New( _
			ByVal list As IList, _
			ByVal allowNew As Boolean, _
			ByVal allowEdit As Boolean, _
			ByVal allowRemove As Boolean _
			)
			
			Me.New(list, allowNew, allowEdit)
			m_AllowRemove = allowRemove
			
		End Sub
		
		#End Region
		
		#Region " Protected Overridable Methods "
		
		''' <summary>
		''' Protected Overridable Method to provide Inheritors with a mechanism to
		''' invoke Custom Code/Validation when an Object is added to this Collection.
		''' </summary>
		''' <param name="value">The Object Added to this Collection.</param>
		''' <remarks></remarks>
		Protected Overridable Function OnAdd( _
			ByVal value As Object _
			) As Boolean
			
			Return (Not IsReadOnly AndAlso TypeAnalyser.IsSubClassOf(value.GetType, ElementType))
			
		End Function
		
		''' <summary>
		''' Protected Overridable Method to provide Inheritors with a mechanism to
		''' invoke Custom Code/Validation when an Object is set into this Collection.
		''' </summary>
		''' <param name="value">The Object Set into this Collection.</param>
		''' <remarks></remarks>
		Protected Overridable Function OnSet( _
			ByVal value As Object _
			) As Boolean
			
			Return (Not IsReadOnly AndAlso TypeAnalyser.IsSubClassOf(value.GetType, ElementType))
			
		End Function
		
		''' <summary>
		''' Protected Overridable Method to provide Inheritors with a mechanism to
		''' invoke Custom Code/Validation when an Object is inserted into this Collection.
		''' </summary>
		''' <param name="value">The Object Inserted into this Collection.</param>
		''' <remarks></remarks>
		Protected Overridable Function OnInsert( _
			ByVal index As Integer, _
			ByVal value As Object _
			) As Boolean
			
			Return (Not IsReadOnly AndAlso TypeAnalyser.IsSubClassOf(value.GetType, ElementType))
			
		End Function
		
		''' <summary>
		''' Protected Overridable Method to provide Inheritors with a mechanism to
		''' invoke Custom Code/Validation when an Object is removed from this Collection.
		''' </summary>
		''' <param name="value">The Object Removed from this Collection.</param>
		''' <remarks></remarks>
		Protected Overridable Function OnRemove( _
			ByVal value As Object _
			) As Boolean
			
			Return (Not IsReadOnly AndAlso TypeAnalyser.IsSubClassOf(value.GetType, ElementType))
			
		End Function
		
		#End Region
		
		#Region " Public Methods "
		
		''' <summary>
		''' Public Method which Adds a Range of Objects (as an IList) to this Collection.
		''' </summary>
		''' <param name="value">The Objects to Add.</param>
		''' <remarks></remarks>
		Public Function AddRange( _
			ByVal value As ICollection _
		) As GeneralCollection
			
			If Not List Is Nothing AndAlso Not value Is Nothing Then
				
				For Each obj As Object In value
					
					Add(obj)
					
				Next
				
			End If
			
			Return Me
			
		End Function
		
		''' <summary>
		''' Public Method which Removes a Range of Objects (as an IList) to this Collection.
		''' </summary>
		''' <param name="value">The Objects to Add.</param>
		''' <remarks></remarks>
		Public Function RemoveRange( _
			ByVal value As ICollection _
		) As GeneralCollection
			
			If Not List Is Nothing AndAlso Not value Is Nothing Then
				
				For Each obj As Object In value
					
					Remove(obj)
					
				Next
				
			End If
			
			Return Me
			
		End Function
		
		''' <summary>
		''' Public Method to Create and Return an Array of Objects from this Collection.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function ToArray() As Array
			
			Dim aryReturn As Array = Array.CreateInstance(ElementType, Count)
			
			Me.CopyTo(aryReturn, 0)
			
			Return aryReturn
			
		End Function
		
		#End Region
		
	End Class

End Namespace