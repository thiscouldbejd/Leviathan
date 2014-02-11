Namespace Calculated

	Partial Public Class Bounded

		#Region " Public Properties "

			Public ReadOnly Property Average() As Average
				Get
					Return Average(2)
				End Get
			End Property

			Public ReadOnly Property Average( _
				ByVal decimal_Places As Integer _
			) As Average
				Get
					Return New Average(All.Count, All.Sum(Function(value) value))
				End Get
			End Property

			Public ReadOnly Property Count() As System.Int32
				Get
					Return All.Count
				End Get
			End Property

			Public ReadOnly Property Sum() As System.Int32
				Get
					Return All.Sum(Function(value) value)
				End Get
			End Property

		#End Region

		#Region " Public Methods "

			Public Function Add( _
				ByVal value As System.Double _
			)

				All.Add(value)
				Minimum = Math.Min(Minimum, value)
				Maximum = Math.Max(Maximum, value)

				Return Me

			End Function

			Public Overrides Function ToString() As String

				Return String.Empty

			End Function

		#End Region

	End Class

End Namespace