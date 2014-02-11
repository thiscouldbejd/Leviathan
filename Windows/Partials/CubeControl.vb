Imports Leviathan.Visualisation
Imports System.Globalization.CultureInfo
Imports System.Windows
Imports System.Windows.Controls.Primitives
Imports System.Windows.Media
Imports System.Windows.Threading
Imports IT = Leviathan.Visualisation.InformationType

Partial Public Class CubeControl
	Implements IScrollInfo
	Implements ICommandsOutputWriter

	#Region " Private Properties "

		Private ReadOnly Property TextWidth As Double
			Get
				Return ActualWidth - (Margin.Left + Margin.Right)
			End Get
		End Property

	#End Region

	#Region " Private Delegates "

		Private Delegate Sub InvalidateVisual_DELEGATE()

	#End Region

	#Region " Private Functions "

		Private Function CreateText( _
			ByVal maxTextWidth As Double _
		) As Boolean

			If Not Values Is Nothing AndAlso Values.Count > 0 Then

				Text_Value = New System.Text.StringBuilder(8192)
				Colours = New Dictionary(Of Integer, Brush)
				Last_Type = IT.General
				Last_Count = Values.Count

				Dim IdealCharacterWidth As Integer = 0

				For i As Integer = 0 To Last_Count - 1

					If i >= Values.Count Then Exit For

					If Not Values(i) Is Nothing Then

						If i > 0 Then Text_Value.AppendLine()
						Values(i).Write(CharacterWidth, Me, IdealCharacterWidth)

					End If

				Next

				If IdealCharacterWidth <> CharacterWidth Then

					IdealWidthDelta = (Math.Ceiling(IdealCharacterWidth * SingleCharacterWidth) + Margin.Left + Margin.Right) - ActualWidth

				Else

					IdealWidthDelta = 0

				End If

				If Text_Value.Length > 0 Then

					Text = New FormattedText(Text_Value.ToString, CurrentUICulture, FlowDirection.LeftToRight, _
						New Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Foreground)

					Dim lastBrush As Brush = Nothing
					Dim lastStart As Integer = -1

					For Each colourStart As Integer In Colours.Keys

						If lastStart >= 0 Then Text.SetForegroundBrush(lastBrush, lastStart, colourStart - lastStart)

						lastBrush = Colours(colourStart)
						lastStart = colourStart

					Next

					If lastStart >= 0 Then Text.SetForegroundBrush(lastBrush, lastStart, Text_Value.Length - lastStart)

					Text.MaxTextWidth = maxTextWidth
					m_ExtentHeight = Text.Height

					Return True

				End If

			ElseIf Not Values Is Nothing AndAlso (Values.Count = 0 And Last_Count > 0) Then ' HACK: Get Round Cleared Values

				Text_Value = New System.Text.StringBuilder(8192)
				Colours = New Dictionary(Of Integer, Brush)
				Last_Type = IT.General
				Last_Count = Values.Count

				Return True

			End If

			Return False

		End Function

	#End Region

	#Region " Protected Overrides "

		Protected Overrides Sub OnInitialized( _
			ByVal e As EventArgs _
		)

			SingleCharacterWidth = New FormattedText(HYPHEN, CurrentUICulture, FlowDirection.LeftToRight, _
				New Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Foreground).Width

		End Sub

		Protected Overrides Sub OnRender( _
			ByVal dc As DrawingContext _
		)

			MyBase.OnRender(dc)

			If ExtentWidth <> ActualWidth Then m_ExtentWidth = ActualWidth

			If ViewportWidth <> ActualWidth Then m_ViewportWidth = ActualWidth

			If ViewportHeight <> ActualHeight Then m_ViewportHeight = ActualHeight

			If CharacterWidth <> Math.Floor(TextWidth / SingleCharacterWidth) OrElse Values.Count <> Last_Count OrElse Last_Removed Then

				If Last_Removed Then

					SyncLock Me

						Last_Removed = False

					End Synclock

				End If

				CharacterWidth = Math.Floor(TextWidth / SingleCharacterWidth)

				If CreateText(TextWidth) Then _
					dc.DrawText(Text, New Point(0, 0))

			ElseIf Not Text Is Nothing Then

				dc.DrawText(Text, New Point(0, 0))

			End If

			If Not ScrollOwner Is Nothing Then ScrollOwner.InvalidateScrollInfo()

		End Sub

		Protected Overrides Sub OnRenderSizeChanged( _
			ByVal sizeInfo As SizeChangedInfo _
		)

			MyBase.OnRenderSizeChanged(sizeInfo)

			' The Control Height has actually been increased in size.
			If sizeInfo.HeightChanged AndAlso sizeInfo.PreviousSize.Height > 0 Then

				' The Control Height is larger than required.
				If ViewportHeight >= (ExtentHeight + Margin.Top + Margin.Bottom) Then

					Transform.Y = 0

				' We've already scrolled a little.
				ElseIf Transform.Y < 0 Then

					If ViewportHeight - Transform.Y > (ExtentHeight + Margin.Top + Margin.Bottom) Then

						Transform.Y = ViewportHeight - (ExtentHeight + Margin.Top + Margin.Bottom)

					ElseIf ViewportHeight + VerticalOffset >= (ExtentHeight + Margin.Top + Margin.Bottom) Then

						Transform.Y += (sizeInfo.NewSize.Height - sizeInfo.PreviousSize.Height)

					End If

				End If

				If Not ScrollOwner Is Nothing Then ScrollOwner.InvalidateScrollInfo()

			End If

		End Sub

	#End Region

	#Region " Public Functions "

		Private Last_Removed As Boolean = False

		Public Sub RemoveValues( _
			ByVal value_Count As Integer _
		)

			Synclock Values

				For i As Integer = (Values.Count - 1) To (Values.Count - (value_Count + 1)) Step -1

					If i < 0 Then Exit For
					Values.RemoveAt(i)

				Next

			End Synclock

			SyncLock Me

				Last_Removed = True

			End Synclock

			If Dispatcher.CheckAccess Then

				InvalidateVisual()

			Else

				Dispatcher.Invoke(DispatcherPriority.Background, New InvalidateVisual_DELEGATE(AddressOf InvalidateVisual))

			End If

		End Sub

		Public Sub AddValues( _
			ByVal _values As IFixedWidthWriteable() _
		)

			Synclock Values

				Values.AddRange(_values)

			End Synclock

			If Dispatcher.CheckAccess Then

				InvalidateVisual()

			Else

				Dispatcher.Invoke(DispatcherPriority.Background, New InvalidateVisual_DELEGATE(AddressOf InvalidateVisual))

			End If

		End Sub

	#End Region

	#Region " ICommandsOutputWriter Implementation "

		Private Sub ICommandsOutputWriter_Write( _
			ByVal value As String, _
			ByVal type As Leviathan.Visualisation.InformationType _
		) Implements Commands.ICommandsOutputWriter.Write

			If Not String.IsNullOrEmpty(value) Then

				If type <> IT.None AndAlso type <> Last_Type Then

					If type = IT.Debug Then

						Colours.Add(Text_Value.Length, New SolidColorBrush(COLOUR_DEBUG))

					ElseIf type = IT.Log Then

						Colours.Add(Text_Value.Length, New SolidColorBrush(COLOUR_LOG))

					ElseIf type = IT.[Error] Then

						Colours.Add(Text_Value.Length, New SolidColorBrush(COLOUR_ERROR))

					ElseIf type = IT.General Then

						Colours.Add(Text_Value.Length, Foreground)

					ElseIf type = IT.Information Then

						Colours.Add(Text_Value.Length, New SolidColorBrush(COLOUR_INFORMATION))

					ElseIf type = IT.Performance Then

						Colours.Add(Text_Value.Length, New SolidColorBrush(COLOUR_PERFORMANCE))

					ElseIf type = IT.Question Then

						Colours.Add(Text_Value.Length, New SolidColorBrush(COLOUR_QUESTION))

					ElseIf type = IT.Success Then

						Colours.Add(Text_Value.Length, New SolidColorBrush(COLOUR_SUCCESS))

					ElseIf type = IT.Warning Then

						Colours.Add(Text_Value.Length, New SolidColorBrush(COLOUR_WARNING))

					ElseIf type = IT.Help Then

						Colours.Add(Text_Value.Length, New SolidColorBrush(COLOUR_HELP))

					End If

					Last_Type = type

				End If

				Text_Value.Append(value)

			End If

		End Sub

		Private Function ICommandsOutputWriter_FixedWidthWrite( _
			ByRef value As String, _
			ByVal width As Integer, _
			ByVal type As Leviathan.Visualisation.InformationType _
		) As Boolean _
		Implements Commands.ICommandsOutputWriter.FixedWidthWrite

			Dim returnValue As Boolean
			Dim lengthWritten As Integer

			If Not String.IsNullOrEmpty(value) Then ICommandsOutputWriter_Write(MaxWidthWrite(value, width, lengthWritten, returnValue), type)

			' Write the Padding Spaces
			ICommandsOutputWriter_Write(New String(SPACE, width - lengthWritten), IT.None)

			Return returnValue

		End Function

		Private Sub ICommandsOutputWriter_TerminateLine( _
			Optional ByVal numberOfTimes As System.Int32 = 1 _
		) _
		Implements ICommandsOutputWriter.TerminateLine

			For i As Integer = 1 To numberOfTimes

				Text_Value.AppendLine()

			Next

		End Sub

	#End Region

	#Region " IScrollInfo Implementation "

		Private Sub LineDown() Implements IScrollInfo.LineDown

			SetVerticalOffset(VerticalOffset + 1)

		End Sub

		Private Sub LineUp() Implements IScrollInfo.LineUp

			SetVerticalOffset(VerticalOffset - 1)

		End Sub

		Private Sub MouseWheelDown() Implements IScrollInfo.MouseWheelDown

			SetVerticalOffset(VerticalOffset + 10)

		End Sub

		Private Sub MouseWheelUp() Implements IScrollInfo.MouseWheelUp

			SetVerticalOffset(VerticalOffset - 10)

		End Sub

		Private Sub PageDown() Implements IScrollInfo.PageDown

			SetVerticalOffset(VerticalOffset + ViewportHeight)

		End Sub

		Private Sub PageUp() Implements IScrollInfo.PageUp

			SetVerticalOffset(VerticalOffset - ViewportHeight)

		End Sub

		Private Sub SetVerticalOffset( _
			ByVal value As Double _
		) Implements IScrollInfo.SetVerticalOffset

			If value < 0 OrElse ViewportHeight >= ExtentHeight Then

				value = 0

			ElseIf value + ViewportHeight >= ExtentHeight Then

				value = ExtentHeight - ViewportHeight

			End If

			m_VerticalOffset = value

			Transform.Y = -value

			If Not ScrollOwner Is Nothing Then ScrollOwner.InvalidateScrollInfo()

		End Sub

		#Region " Not Implemented Methods "

			Private Sub Not_Implemented() Implements IScrollInfo.LineLeft, IScrollInfo.LineRight, IScrollInfo.MouseWheelLeft, _
				IScrollInfo.MouseWheelRight, IScrollInfo.PageLeft, IScrollInfo.PageRight
			End Sub

			Private Sub Not_Implemented( _
				ByVal value As Double _
			) Implements IScrollInfo.SetHorizontalOffset
			End Sub

			Private Function Not_Implemented( _
				ByVal visual As Visual, _
				ByVal rectangle As Rect _
			) As Rect Implements IScrollInfo.MakeVisible
			End Function

		#End Region

	#End Region

	#Region " Friend Shared Functions "

		Friend Shared Function MaxWidthWrite( _
			ByRef value As String, _
			ByVal width As Integer, _
			ByRef lengthWritten As Integer, _
			ByRef returnValue As Boolean _
		) As String

			Dim valueToWrite As String

			If value.Length <= width Then

				' If the value fits into the cell, write it out
				valueToWrite = value

				' Mark how many characters have been written
				lengthWritten = value.Length

			Else

				Dim lastSpace As Integer = value.Substring(0, width + 1).LastIndexOf(SPACE)

				If lastSpace >= Math.Floor(width / 2) Then

					' If there is a convenient space, split around that
					lengthWritten = lastSpace

				Else

					' Otherwise, split at width
					lengthWritten = width

				End If

				' Write whatever we can
				valueToWrite = value.Substring(0, lengthWritten)

				' Write the remainder to the Return Parameter
				value = value.Substring(lengthWritten).TrimStart(SPACE)

				' Inform the calling procedure that there is a remainder
				If Not String.IsNullOrEmpty(value) Then returnValue = True

			End If

			Return valueToWrite

		End Function

	#End Region

End Class
