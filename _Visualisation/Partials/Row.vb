Imports System.IO

Namespace Visualisation

	Partial Public Class Row

		#Region " Public Constants "

			Public Const COLUMN_SPACER As Char = SPACE

		#End Region

		#Region " Public Properties "

			Public ReadOnly Property Count() As Integer
				Get
					Return Cells.Count
				End Get
			End Property

			Default Public Property Item( _
				ByVal index As Integer _
			) As Cell
				Get
					Return Cells(index)
				End Get
				Set(ByVal value As Cell)
					Cells(index) = value
				End Set
			End Property

		#End Region

		#Region " Public Methods "

			Public Overridable Function Write( _
				ByVal widths As Integer(), _
				ByRef writer As ICommandsOutputWriter _
			) As ICommandsOutputWriter

				Dim remainderRow As Row = Nothing

				For i As Integer = 0 To Cells.Count - 1

					If i > 0 Then writer.Write(COLUMN_SPACER, InformationType.None)

					If Cells(i) Is Nothing Then

						writer.FixedWidthWrite(Nothing, widths(i), InformationType.None)

					Else

						Dim stringValue As String = Cells(i).ToString

						If writer.FixedWidthWrite(stringValue, widths(i), Cells(i).Type) Then

							If remainderRow Is Nothing Then

								remainderRow = Row.Create(i)

							Else

								For j As Integer = remainderRow.Cells.Count To i - 1

									remainderRow.Cells.Add(Nothing)

								Next

							End If

							remainderRow.Cells.Add(New Cell(stringValue, Cells(i).TruncationLength, Cells(i).Type))

						End If

					End If

				Next

				writer.TerminateLine()

				If Not remainderRow Is Nothing Then

					Return remainderRow.Write(widths, writer)

				Else

					Return writer

				End If

			End Function

			Public Function Add() As Row

				Return Add(Nothing)

			End Function

			Public Function Add( _
				ByVal value As Object _
			) As Row

				Return Add(value, InformationType.General)

			End Function

			Public Function Add( _
				ByVal value As Object, _
				ByVal type As InformationType _
			) As Row

				If value Is Nothing Then

					Cells.Add(Nothing)

				ElseIf value.GetType Is GetType(Cell) Then

					Cells.Add(value)

				Else

					Cells.Add(New Cell(value, 0, type))

				End If

				Return Me

			End Function

			Public Function Insert( _
				ByVal index As Integer _
			) As Row

				Return Insert(index, Nothing)

			End Function

			Public Function Insert( _
				ByVal index As Integer, _
				ByVal value As Object _
			) As Row

				Return Insert(index, value, InformationType.General)

			End Function

			Public Function Insert( _
				ByVal index As Integer, _
				ByVal value As Object, _
				ByVal type As InformationType _
			) As Row

				If value Is Nothing Then

					Cells.Insert(index, Nothing)

				ElseIf value.GetType Is GetType(Cell) Then

					Cells.Insert(index, value)

				Else

					Cells.Insert(index, New Cell(value, 0, type))

				End If

				Return Me

			End Function

			Public Function [Get]( _
				ByVal index As Integer _
			) As Object

				If index >=0 AndAlso index <= Cells.Count - 1 AndAlso Not Cells(index) Is Nothing Then Return Cells(index).Value

				Return Nothing

			End Function

			Public Function [Set]( _
				ByVal index As Integer, _
				ByVal value As Object _
			) As Row

				If index >=0 AndAlso index <= Cells.Count - 1 Then

					If value Is Nothing Then

						Cells(index) = Nothing

					ElseIf value.GetType Is GetType(Cell) Then

						Cells(index) = value

					Else

						Cells(index) = New Cell(value)

					End If

				End If

				Return Me

			End Function

			Public Function Update( _
				ByVal index As Integer, _
				ByVal value As Object _
			) As Row

				If index >=0 AndAlso index <= Cells.Count - 1 AndAlso Not Cells(index) Is Nothing Then

					If value.GetType Is GetType(Cell) Then

						Cells(index) = value

					Else

						Cells(index).Value = value

					End If

				End If

				Return Me

			End Function

		#End Region

		#Region " Public Shared Methods "

			Public Shared Function Create( _
				ByVal count As Integer _
			) As Row

				Dim newRow As New Row()

				For i As Integer = 0 to count - 1

					NewRow.Add()

				Next

				Return newRow

			End Function

			Public Shared Function Create( _
				ByVal ParamArray rowValues As Cell() _
			) As Row

				Return Create(New List(Of Cell)(rowValues))

			End Function

			Public Shared Function Create( _
				ByVal rowValues As List(Of Cell) _
			) As Row

				Return New Row(rowValues)

			End Function

			Public Shared Sub Remove_Duplicates( _
				ByVal cell_Indexes As Integer(), _
				ByRef row As Row, _
				ByRef rows As List(Of Row) _
			)

				For Each cell_Index As Integer In cell_Indexes

					If Not row(cell_Index) Is Nothing Then

						For k As Integer = (rows.Count - 1) To 0 Step -1

							If Not rows(k)(cell_Index) Is Nothing Then

								If rows(k)(cell_Index).ToString() = row(cell_Index).ToString Then row(cell_Index) = Nothing
								Exit For

							End If

						Next

					End If

				Next

			End Sub

		#End Region

	End Class

End Namespace
