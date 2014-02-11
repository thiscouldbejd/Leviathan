Imports System.IO

Namespace Visualisation

	Partial Public Class Slice
	
		#Region " Public Properties "

			''' <summary>
			''' Whether the table has data
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Public ReadOnly Property HasData() As System.Boolean
				Get
					Return Not Rows Is Nothing AndAlso Rows.Count > 0
				End Get
			End Property

			Public ReadOnly Property LastRow() As Row
				Get
					If HasData Then
						Return Rows(Rows.Count - 1)
					Else
						Return Nothing
					End If
				End Get
			End Property
		#End Region

		#Region " Public Methods "

			Public Function Add( _
				ByVal ParamArray values() As Object _
			) As Slice
			
				Dim new_Row As New Row()
				
				For i As Integer = 0 To values.Length - 1
				
					new_Row.Add(values(i))
					
				Next
				
				Rows.Add(new_Row)
				
				Return Me
				
			End Function
			
		#End Region
		
	End Class
	
End Namespace
