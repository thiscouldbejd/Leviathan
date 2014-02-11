Imports Leviathan.Commands
Imports Leviathan.Comparison.Comparer
Imports Leviathan.Visualisation
Imports System.Threading
Imports System.Threading.ThreadPool
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Threading

Partial Public Class OutputInterface
	Implements ICommandsInteractive

	#Region " Public Delegates "

		''' <summary>Delegate Used to Update Border Colour</summary>
		''' <param name="value">The new value for the colour.</param>
		''' <remarks></remarks>
		Public Delegate Sub UpdateBorderColourDelegate( _
			ByVal value As System.Windows.Media.Color _
		)

	#End Region

	#Region " Public Properties "

		Public ReadOnly Property CompletedWithoutOutput()
			Get
				Return Completed AndAlso (txtOutput.Values Is Nothing OrElse txtOutput.Values.Count = 0)
			End Get
		End Property

	#End Region

	#Region " Private Handling Methods "

		#Region " Opacity Handling "

			Private Delegate Sub Update_OpacityDelegate( _
				ByVal opacity_Delta As Double _
			)

			Private Sub Manage_Opacity( _
				ByVal timeinMs As Integer, _
				ByVal target_Opacity As Double _
			)

				If Not Window_Locked AndAlso Not Opacity_Locked Then

					InputInterface.Manage_Thread(OPACITY_THREAD, AddressOf Manage_Opacity, True, True)

					If (target_Opacity <> Opacity) Then

						Dim steps As Integer = Math.Floor(timeinMs / 20)

						Dim delta As Double = (target_Opacity - Opacity) / steps

						OPACITY_THREAD.Start(New Object() {steps, delta})

					End If

				End If

			End Sub

			Private Sub Manage_Opacity( _
				ByVal values As Object() _
			)

				If CInt(values(0)) > 0 Then

					Update_Opacity(CDbl(values(1)))

					Thread.Sleep(10)

					Manage_Opacity(New Object() {CInt(values(0)) - 1, values(1)})

				End If

			End Sub

			Private Sub Update_Opacity( _
				ByVal opacity_Delta As Double _
			)

				If Dispatcher.CheckAccess Then

					Opacity += opacity_Delta

				Else

					Dispatcher.Invoke(New Update_OpacityDelegate(AddressOf Me.Update_Opacity), DispatcherPriority.Background, opacity_Delta)

				End If

			End Sub

		#End Region

		#Region " Form Handlers "

			Private Sub ResizeToContent()

				If Not Window_Locked Then

					Dim isRightOrientated As Boolean = (Left = SystemParameters.FullPrimaryScreenWidth - Width)

					If txtOutput.IdealWidthDelta <> 0 Then

						If Width + txtOutput.IdealWidthDelta > MinWidth Then

							If Width + txtOutput.IdealWidthDelta > SystemParameters.FullPrimaryScreenWidth Then

								Width = SystemParameters.FullPrimaryScreenWidth

							Else

								Width = Width + txtOutput.IdealWidthDelta

							End If

							If Left + Width > SystemParameters.FullPrimaryScreenWidth OrElse isRightOrientated Then

								AlignRight()

							ElseIf Left < 0 Then

								Left = 0

							End If

						Else

							Width = MinWidth
							If IsRightOrientated Then AlignRight()

						End If

					End If

				End If

			End Sub

			Private Sub AlignLeft()

				If Not Window_Locked Then Left = 0

			End Sub

			Private Sub AlignRight()

				If Not Window_Locked Then Left = SystemParameters.FullPrimaryScreenWidth - Width

			End Sub

			Private Sub OutputInterface_Loaded( _
				ByVal sender As Object, _
				ByVal e As System.Windows.RoutedEventArgs _
			) Handles Me.Loaded

				statChars.Content = String.Format(CHARACTER_OUTPUT_WIDTH, txtOutput.CharacterWidth)

				If Command.Length >= (100 - Cell.SUFFIX_TRUNCATED.Length) Then

					statCommand.Content = Command.Substring(0, 100 - Cell.SUFFIX_TRUNCATED.Length) & Cell.SUFFIX_TRUNCATED

				Else

					statCommand.Content = Command

				End If

			End Sub

			Private Sub Move_Window( _
				ByVal sender As Object, _
				ByVal e As MouseButtonEventArgs _
			) Handles Me.MouseLeftButtonDown

				If Not Window_Locked Then MyBase.DragMove()

			End Sub

			Private Sub OutputInterface_KeyDown( _
				ByVal sender As Object, _
				ByVal e As System.Windows.Input.KeyEventArgs _
			) Handles Me.KeyDown

				If e.Key = Key.F3 Then

					AlignLeft()
					e.Handled = True

				ElseIf e.Key = Key.F4 Then

					ResizeToContent()
					e.Handled = True

				ElseIf e.Key = Key.F5 Then

					AlignRight()
					e.Handled = True

				ElseIf e.Key = Key.F6 Then

					Window_Locked = Not Window_Locked

					If Window_Locked Then
						ResizeMode = ResizeMode.NoResize
						UpdateBorderColour(COLOUR_LOCKED_BORDER)
					Else
						ResizeMode = ResizeMode.CanResizeWithGrip
						UpdateBorderColour(COLOUR_UNLOCKED_BORDER)
					End If

					e.Handled = True

				ElseIf e.Key = Key.F7

					TopMost = Not TopMost
					e.Handled = True

				ElseIf e.Key = Key.F8

					Opacity_Locked = Not Opacity_Locked
					e.Handled = True
					
				End If

			End Sub

			Private Sub OutputInterface_Deactivated( _
				ByVal sender As Object, _
				ByVal e As System.EventArgs _
			) Handles Me.Deactivated

				If Opacity <> OPACITY_INACTIVE Then Manage_Opacity(OPACITY_SLOW, OPACITY_INACTIVE)

			End Sub

			Private Sub OutputInterface_Activated( _
				ByVal sender As Object, _
				ByVal e As System.EventArgs _
			) Handles Me.Activated

				If Opacity <> OPACITY_ACTIVE Then Manage_Opacity(OPACITY_FAST, OPACITY_ACTIVE)

			End Sub

			Private Sub OutputInterface_MouseEnter( _
				ByVal sender As Object, _
				ByVal e As System.EventArgs _
			) Handles Me.MouseEnter

				If Not IsActive Then Manage_Opacity(OPACITY_FAST, OPACITY_ACTIVE)

			End Sub

			Private Sub OutputInterface_MouseLeave( _
				ByVal sender As Object, _
				ByVal e As System.EventArgs _
			) Handles Me.MouseLeave

				If Not IsActive Then Manage_Opacity(OPACITY_FAST, OPACITY_INACTIVE)

			End Sub

		#End Region

		#Region " Header Handlers "

			Private Sub highlight_Rectangle( _
				ByVal sender As Object, _
				ByVal e As System.Windows.Input.MouseEventArgs _
			) Handles rectTop.MouseEnter, rectMinimise.MouseEnter, rectClose.MouseEnter

				If Not Window_Locked Then CType(sender, Rectangle).Opacity += 0.3

			End Sub

			Private Sub unHighlight_Rectangle( _
				ByVal sender As Object, _
				ByVal e As System.Windows.Input.MouseEventArgs _
			) Handles rectTop.MouseLeave, rectMinimise.MouseLeave, rectClose.MouseLeave

				If Not Window_Locked Then CType(sender, Rectangle).Opacity -= 0.3

			End Sub

			Private Sub rectTop_MouseLeftButtonDown( _
				ByVal sender As Object, _
				ByVal e As System.Windows.Input.MouseButtonEventArgs _
			) Handles rectTop.MouseLeftButtonDown

				If e.ClickCount = 2 Then ResizeToContent()

			End Sub

			Private Sub rectMinimise_MouseLeftButtonDown( _
				ByVal sender As Object, _
				ByVal e As System.Windows.Input.MouseButtonEventArgs _
			) Handles rectMinimise.MouseLeftButtonDown

				If Not Window_Locked Then WindowState = Windows.WindowState.Minimized

			End Sub

			Private Sub rectClose_MouseLeftButtonDown( _
				ByVal sender As Object, _
				ByVal e As System.Windows.Input.MouseButtonEventArgs _
			) Handles rectClose.MouseLeftButtonDown

				If Not Window_Locked Then

					InputInterface.Manage_Thread(OPACITY_THREAD, Nothing, True, True)
					Me.Close()

				End If

			End Sub

		#End Region

		#Region " Footer Handlers "

			Private Delegate Sub ReDrawProgress_DELEGATE()

			Private Sub ReDrawProgress()

				If Dispatcher.CheckAccess Then

					If LastProgress > 0 AndAlso LastProgress < 1 Then

						Dim progress_Brush As New System.Windows.Media.LinearGradientBrush(Colors.Green, Colors.Green, 0)

						progress_Brush.Opacity = 0.5
						progress_Brush.StartPoint = New System.Windows.Point(0, 0.5)
						progress_Brush.EndPoint = New System.Windows.Point(1, 0.5)
						progress_Brush.GradientStops.Add(new GradientStop(System.Windows.Media.Colors.Black, lastProgress))
						statProgress.Background = progress_Brush

					Else

						statProgress.Background = Nothing

					End If

				Else

					Dispatcher.Invoke(DispatcherPriority.Normal, New ReDrawProgress_DELEGATE(AddressOf ReDrawProgress))

				End If

			End Sub

			Private Sub statCommand_MouseLeftButtonDown( _
				ByVal sender As Object, _
				ByVal e As System.Windows.Input.MouseButtonEventArgs _
			) Handles statCommand.MouseLeftButtonDown

				If e.ClickCount = 2 Then AlignLeft()

			End Sub

			Private Sub statChars_MouseLeftButtonDown( _
				ByVal sender As Object, _
				ByVal e As System.Windows.Input.MouseButtonEventArgs _
			) Handles statChars.MouseLeftButtonDown

				If e.ClickCount = 2 Then AlignRight()

			End Sub

		#End Region

		#Region " Control Handlers "

			Private Sub txtOutput_SizeChanged( _
				ByVal sender As Object, _
				ByVal e As System.Windows.SizeChangedEventArgs _
			) Handles txtOutput.SizeChanged

				statChars.Content = String.Format(CHARACTER_OUTPUT_WIDTH, CType(sender, CubeControl).CharacterWidth)

			End Sub

			Private Sub txtInput_KeyUp( _
				ByVal sender As Object, _
				ByVal e As System.Windows.Input.KeyEventArgs _
			) Handles txtInput.KeyUp

				If e.Key = Key.Enter Then

					ReturnInput = CType(sender, TextBox).Text
					ReturnedInput = True

				ElseIf e.Key = Key.C AndAlso EnumContains(e.KeyboardDevice.Modifiers, ModifierKeys.Control) Then

					ReturnedInput = True

				End If

				If ReturnedInput Then HideInputBox()

			End Sub

		#End Region

	#End Region

	#Region " ICommandsInteractive Implementation "

		Private Sub Output_Remove() _
		Implements ICommandsInteractive.Remove_Output

			Output_Remove(1)

		End Sub

		Private Sub Output_Remove( _
			ByVal output_Count As Integer _
		) _
		Implements ICommandsInteractive.Remove_Outputs

			txtOutput.RemoveValues(output_Count)

		End Sub

		Private Sub Output_Outputs( _
			ParamArray ByVal values As IFixedWidthWriteable() _
		) _
		Implements ICommandsInteractive.Show_Outputs

			txtOutput.AddValues(values)

		End Sub

		Private Delegate Sub Output_Progress_DELEGATE( _
			ByVal value As Single, _
			ByVal name As String, _
			ByVal timeTaken As TimeSpan, _
			ByVal timeToCompletion As TimeSpan _
		)

		Private Sub Output_Progress( _
			ByVal value As Single, _
			ByVal name As String, _
			ByVal timeTaken As TimeSpan, _
			ByVal timeToCompletion As TimeSpan _
		) _
		Implements ICommandsInteractive.Show_Progress

			If Dispatcher.CheckAccess Then

				LastProgress = value

				If value <= 0 OrElse value >= 1 Then

					statProgress.Content = Nothing
					statProgress.Background = Nothing

				Else

					If Not timeToCompletion = Nothing Then

						Dim valueTaken As String = New TimeSpanConvertor().ParseStringFromTimespan(timeTaken, New Boolean, True)

						Dim valueToCompletion As String = New TimeSpanConvertor().ParseStringFromTimespan(timeToCompletion, New Boolean, True)

						If String.IsNullOrEmpty(name) Then

							statProgress.Content = String.Format("{0}% ({1} taken|{2} left)", Math.Round(value * 100, 2).ToString, valueTaken, valueToCompletion)

						Else

							statProgress.Content = String.Format("{0}: {1}% - {2} taken|{3} left", name, Math.Round(value * 100, 2).ToString, valueTaken, _
								valueToCompletion)

						End If

					Else

						If String.IsNullOrEmpty(name) Then

							statProgress.Content = String.Format("{0}%", Math.Round(value * 100, 2).ToString)

						Else

							statProgress.Content = String.Format("{0}: {1}%", name, Math.Round(value * 100, 2).ToString)

						End If

					End If

					ReDrawProgress()

				End If

			Else

				Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, _
					New Output_Progress_DELEGATE(AddressOf Output_Progress), value, name, timeTaken, timeToCompletion)

			End If

		End Sub

		Private Delegate Sub Input_Response_DELEGATE()

		Private Function Input_Response() As String _
		Implements ICommandsInteractive.Get_Response

			ShowInputBox()

			Do Until ReturnedInput

				System.Threading.Thread.Sleep(100)

			Loop

			Return ReturnInput

		End Function

		Private Sub ShowInputBox()

			If Dispatcher.CheckAccess Then

				ReturnInput = Nothing
				ReturnedInput = False
				txtInput.Height = 20
				txtOutput.Margin = New Thickness(txtOutput.Margin.Left, txtOutput.Margin.Top + txtInput.Height + 2, txtOutput.Margin.Right, txtOutput.Margin.Bottom)
				txtInput.Focus()

			Else

				Dispatcher.Invoke(DispatcherPriority.Normal, New Input_Response_DELEGATE(AddressOf ShowInputBox))

			End If

		End Sub

		Private Sub HideInputBox()

			If Dispatcher.CheckAccess Then

				txtOutput.Margin = New Thickness(txtOutput.Margin.Left, txtOutput.Margin.Top - (txtInput.Height + 2), txtOutput.Margin.Right, _
					txtOutput.Margin.Bottom)
				txtInput.Height = 0
				txtInput.Text = Nothing

			Else

				Dispatcher.Invoke(DispatcherPriority.Normal, New Input_Response_DELEGATE(AddressOf HideInputBox))

			End If

		End Sub

	#End Region

	#Region " Private Delegated Methods "

		Private Sub UpdateBorderColour( _
			ByVal value As System.Windows.Media.Color _
		)

			If Dispatcher.CheckAccess Then

				Content.BorderBrush = New SolidColorBrush(value)

			Else

				Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Normal, _
					New UpdateBorderColourDelegate(AddressOf UpdateBorderColour), value)

			End If

		End Sub

	#End Region

End Class
