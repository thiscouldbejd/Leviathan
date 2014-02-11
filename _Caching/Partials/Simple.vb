Imports Leviathan.Comparison.Comparer

Namespace Caching

	Public Class Simple

		#Region " Public Events "

			''' <summary>
			''' Event Raised when the Contents of the Cache (rather than individual element updates) are changed.
			''' </summary>
			''' <param name="count">The New Count of Cache Elements.</param>
			Public Event ContentsChanged( _
				ByRef count As Integer _
			)

		#End Region

		#Region " Public Properties "

			Public Overridable ReadOnly Property Entries() As System.Collections.DictionaryEntry()
				Get

					Dim aryReturn(Cache.Count - 1) As DictionaryEntry

					Dim keys As ICollection = Cache.Keys
					Dim currentIndex As Integer = 0

					For Each key As Object In keys

						aryReturn(currentIndex) = New DictionaryEntry(key, Cache(key))
						currentIndex += 1

					Next

					Return aryReturn

				End Get
			End Property

		#End Region

		#Region " Private Methods "

			Private Function TryGet_Internal( _
				ByRef value As Object, _
				ByVal key As Object _
			) As Boolean

				SyncLock Cache_LOCK

					If Cache.ContainsKey(key) Then

						value = Cache(key)
						Return True

					Else

						Return False

					End If

				End SyncLock

			End Function

			Private Sub Set_Internal( _
				ByVal value As Object, _
				ByVal key As Object _
			)

				If RaisesEvents Then

					SyncLock Cache_LOCK

						Dim start_Count As Integer = start_Count = Cache.Count

							Cache(key) = value

							If start_Count <> Cache.Count Then _
								RaiseEvent ContentsChanged(Cache.Count)

					End SyncLock

				Else

					SyncLock Cache_LOCK

						Cache(key) = value

					End SyncLock

				End If

			End Sub

			Public Sub Remove_Internal( _
				ByVal key As Object _
			)

				If RaisesEvents Then

					SyncLock Cache_LOCK

						Dim start_Count As Integer = Cache.Count

						Cache.Remove(key)

						If start_Count <> Cache.Count Then _
							RaiseEvent ContentsChanged(Cache.Count)

					End SyncLock

				Else

					SyncLock Cache_LOCK

						Cache.Remove(key)

					End SyncLock

				End If

			End Sub

		#End Region

		#Region " Public Methods "

			''' <summary>
			''' Method to Provide the Ability to Retrieve Objects from the Cache.
			''' </summary>
			''' <param name="value">The Cached Object Returned from the cache (if True is returned).</param>
			''' <param name="methodHashcode">The Hashcode of the Method/Method Name making the cache call.</param>
			''' <param name="methodVariableHashCodes">The Parameters passed to the method making the cache call.</param>
			''' <returns>True if the object is retrieved from the Cache or False if the Object is not
			''' in the Cache.</returns>
			''' <remarks></remarks>
			Public Function TryGet( _
				ByRef value As Object, _
				ByVal methodHashCode As Integer, _
				ByVal ParamArray methodVariableHashCodes As Integer() _
			) As Boolean

				Return TryGet_Internal(value, GetCacheSignature(methodHashCode, methodVariableHashCodes))

			End Function

			''' <summary>
			''' Method to Provide the Ability to Retrieve Objects from the Cache.
			''' </summary>
			''' <param name="value">The Cached Object Returned from the cache (if True is returned).</param>
			''' <param name="name">The Name/Key of the Object Required.</param>
			''' <returns>True if the object is retrieved from the Cache or False if the Object is not
			''' in the Cache.</returns>
			''' <remarks></remarks>
			Public Function TryGet( _
				ByRef value As Object, _
				ByVal name As String _
			) As Boolean

				Return TryGet_Internal(value, name)

			End Function

			''' <summary>
			''' Method to Provide the Ability to Store Objects in the Cache.
			''' </summary>
			''' <param name="value">The Object to Cache.</param>
			''' <param name="methodHashCode">The Hashcode of the Method/Method Name making the cache call.</param>
			''' <param name="methodVariableHashCodes">The Parameters passed to the method making the cache call.</param>
			''' <remarks></remarks>
			Public Sub [Set]( _
				ByVal value As Object, _
				ByVal methodHashCode As Integer, _
				ByVal ParamArray methodVariableHashCodes As Integer() _
			)

				Set_Internal(value, GetCacheSignature(methodHashCode, methodVariableHashCodes))

			End Sub

			''' <summary>
			''' Method to Provide the Ability to Store Objects in the Cache.
			''' </summary>
			''' <param name="value">The Object to Cache.</param>
			''' <param name="name">The Name/Key of the Object being Set.</param>
			''' <remarks></remarks>
			Public Sub [Set]( _
				ByVal value As Object, _
				ByVal name As String _
			)

				Set_Internal(value, name)

			End Sub

			''' <summary>
			''' Method to Provide the Ability to Remove Objects from the Cache.
			''' </summary>
			''' <param name="methodHashCode">The Hashcode of the Method/Method Name making the cache call.</param>
			''' <param name="methodVariableHashCodes">The Parameters passed to the method making the cache call.</param>
			''' <remarks></remarks>
			Public Sub Remove( _
				ByVal methodHashCode As Integer, _
				ByVal ParamArray methodVariableHashCodes As Integer() _
			)

				Remove_Internal(GetCacheSignature(methodHashCode, methodVariableHashCodes))

			End Sub

			''' <summary>
			''' Method to Provide the Ability to Remove Objects from the Cache.
			''' </summary>
			''' <param name="name">The Name/Key of the Object being Removed.</param>
			''' <remarks></remarks>
			Public Sub Remove( _
				ByVal name As String _
			)

				Remove_Internal(name)

			End Sub

		#End Region

		#Region " Protected Shared Methods "

			Protected Friend Shared Function GetCacheSignature( _
				ByVal methodHashCode As Int32, _
				ByVal methodVariableHashCodes As Int32() _
			) As Int64

				Dim aryBytes(7) As Byte

				BitConverter.GetBytes(methodHashCode).CopyTo(aryBytes, 0)
				BitConverter.GetBytes(CombineHashCodes(methodVariableHashCodes)).CopyTo(aryBytes, 4)

				Return BitConverter.ToInt64(aryBytes, 0)

			End Function

			Protected Shared Function CombineHashCodes( _
				ByVal hashCodes As Int32() _
			) As Integer

				Dim hashCode As Int32 = 0

				If Not hashCodes Is Nothing Then

					For i As Integer = 0 To hashCodes.Length - 1

						hashCode = (hashCode << 5) Xor hashCodes(i)

					Next

				End If

				Return hashCode

			End Function

		#End Region

		#Region " Public Shared Methods "

			Public Shared Function CombineHashCodes( _
				ByVal ParamArray hashableObjects As Object() _
			) As Integer

				If Not hashableObjects Is Nothing Then

					Dim hashCodes(hashableObjects.Length - 1) As Integer

					For i As Integer = 0 To hashableObjects.Length - 1

						If Not IsNothing(hashableObjects(i)) Then

							If hashableObjects(i).GetType.IsArray Then

								hashCodes(i) = CombineHashCodes(CType(hashableObjects(i), Object()))

							Else

								hashCodes(i) = hashableObjects(i).GetHashCode

							End If

						End If

					Next

					Return CombineHashCodes(hashCodes)

				Else

					Return 0

				End If

			End Function

		#End Region

	End Class

End Namespace