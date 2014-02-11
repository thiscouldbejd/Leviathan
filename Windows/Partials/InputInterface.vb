Imports Leviathan.Commands.CommandArgumentsParser
Imports Leviathan.Commands.SuggestedCommandOutput
Imports Leviathan.Comparison.Comparer
Imports System.Globalization.CultureInfo
Imports System.Threading
Imports System.Threading.ThreadPool
Imports System.Windows
Imports System.Windows.Media
Imports System.Windows.Media.Animation

Partial Public Class InputInterface

	#Region " Public Delegates "

		''' <summary>Delegate Used to Update Output/Background Control</summary>
		''' <param name="value">The new value for the text.</param>
		''' <remarks></remarks>
		Public Delegate Sub UpdateOutputDelegate( _
			ByVal value As String _
		)

		''' <summary>Delegate Used to Update Input Control.</summary>
		''' <param name="value">The new value for the text.</param>
		''' <param name="position">The new position for the cursor in the text.</param>
		''' <remarks></remarks>
		Public Delegate Sub UpdateInputDelegate( _
			ByVal value As String, _
			ByVal position As Integer _
		)

		''' <summary>Delegate Used to Update Input Control.</summary>
		''' <param name="value">The new value for the text.</param>
		''' <remarks></remarks>
		Public Delegate Sub UpdateValueDelegate( _
			ByRef value As Integer _
		)

		''' <summary>Delegate Used to Update 'Suggestions Enabled' Control</summary>
		''' <param name="suggestionsEnabled">Whether Suggestions are Enabled or Disabled</param>
		''' <remarks></remarks>
		Public Delegate Sub UpdateSettingsDelegate( _
			ByVal suggestionsEnabled As Boolean _
		)

		''' <summary>Delegate Used to Execute Actual Command</summary>
		''' <param name="command">Command to Execute</param>
		''' <remarks></remarks>
		Public Delegate Sub ExecuteCommandDelegate( _
			ByVal command As String _
		)

		''' <summary>Delegate used to Inform that Command Execution Thread has completed Execution</summary>
		''' <param name="id">The ID of the Executing Window.</param>
		''' <param name="completed">Whether Execution has been properly completed.</param>
		''' <remarks></remarks>
		Public Delegate Sub ExecutionCompletedDelegate( _
			ByVal id As Int32, _
			ByVal completed As Boolean _
		)

		''' <summary></summary>
		''' <param name="count"></param>
		''' <remarks></remarks>
		Public Delegate Sub UpdateExecuteStatusDelgate( _
			ByVal count As Int32 _
		)

		''' <summary>Delegate Used to Update Border Colour</summary>
		''' <param name="value">The new value for the colour.</param>
		''' <remarks></remarks>
		Public Delegate Sub UpdateBorderColourDelegate( _
			ByVal value As System.Windows.Media.Color _
		)

	#End Region

	#Region " Private Handling Methods "

		#Region " Cache Status Box Handlers "

			Private Sub Cache_CacheContentsChanged( _
				ByRef newContentCount As Integer _
			) Handles m_Cache.ContentsChanged

				CachePosition = 0
				CacheList.Clear()

				If newContentCount = 1 Then

					CacheList.Add(String.Format(SETTINGS_CACHE, newContentCount))

				ElseIf newContentCount > 1 Then

					CacheList.Add(String.Format(SETTINGS_CACHES, newContentCount))

				End If

				Dim cacheEntries As DictionaryEntry() = Cache.Entries()

				If cacheEntries.Length > 0 Then

					For i As Integer = 0 To cacheEntries.Length - 1

						If cacheEntries(i).Value Is Nothing Then

							CacheList.Add(String.Format(SETTINGS_CACHELIST_NULL, cacheEntries(i).Key))

						Else

							CacheList.Add(String.Format(SETTINGS_CACHELIST_VALUE, cacheEntries(i).Key, cacheEntries(i).Value.ToString))

						End If

					Next

				End If

				CycleCache(0)

			End Sub

		#End Region

		#Region " Input Text Box Handlers "

			Private Sub txtInput_KeyUp( _
				ByVal sender As Object, _
				ByVal e As System.Windows.Input.KeyEventArgs _
			) Handles txtInput.KeyUp

				If e.Key = Key.Up Then

					If EnumContains(e.KeyboardDevice.Modifiers, ModifierKeys.Control, ModifierKeys.Alt) Then

						CycleCache(-1)

					ElseIf EnumContains(e.KeyboardDevice.Modifiers, ModifierKeys.Control) Then

						CycleRecentCommands(-1)

					Else

						QueueUserWorkItem(AddressOf Handler.ProcessInput, _
							New SuggestedCommandState(SuggestedCommandKey.Up, CType(sender, TextBox).Text, CType(sender, TextBox).SelectionStart) _
						)

					End If

					e.Handled = True

				ElseIf e.Key = Key.Down Then

					If EnumContains(e.KeyboardDevice.Modifiers, ModifierKeys.Control, ModifierKeys.Alt) Then

						CycleCache(1)

					ElseIf EnumContains(e.KeyboardDevice.Modifiers, ModifierKeys.Control) Then

						CycleRecentCommands(1)

					Else

						QueueUserWorkItem(AddressOf Handler.ProcessInput, _
							New SuggestedCommandState(SuggestedCommandKey.Down, CType(sender, TextBox).Text, CType(sender, TextBox).SelectionStart) _
						)

					End If

					e.Handled = True

				ElseIf e.Key = Key.Right Then

					If String.IsNullOrEmpty(CType(sender, TextBox).Text) OrElse _
						CType(sender, TextBox).SelectionStart = CType(sender, TextBox).Text.Length Then

						QueueUserWorkItem(AddressOf Handler.ProcessInput, _
							New SuggestedCommandState(SuggestedCommandKey.Right, CType(sender, TextBox).Text, CType(sender, TextBox).SelectionStart))

						e.Handled = True

					End If

				ElseIf e.Key = Key.C AndAlso EnumContains(e.KeyboardDevice.Modifiers, ModifierKeys.Control) Then

					If ExecutingThreads.Count > 0 AndAlso ExecutingThreads(ExecutingThreads.Count - 1).IsAlive Then _
						ExecutionCompleted(Integer.Parse(ExecutingThreads(ExecutingThreads.Count - 1).Name.Substring( _
							0, ExecutingThreads(ExecutingThreads.Count - 1).Name.IndexOf(COLON))), False)

					e.Handled = True

				End If

			End Sub

			Private Sub txtInput_KeyDown( _
				ByVal sender As Object, _
				ByVal e As System.Windows.Input.KeyEventArgs _
			) Handles txtInput.KeyDown

				' Handles 'Command' Keys
				If e.Key = Key.Enter Then

					InputInterface.Manage_Thread(InputChangedThread, Nothing, False, True)

					QueueUserWorkItem(AddressOf Handler.ProcessInput, _
						New SuggestedCommandState(SuggestedCommandKey.Enter, CType(sender, TextBox).Text, CType(sender, TextBox).SelectionStart))

					e.Handled = True

				ElseIf e.Key = Key.Tab Then

					If EnumContains(e.KeyboardDevice.Modifiers, ModifierKeys.Shift) Then

						QueueUserWorkItem(AddressOf Handler.ProcessInput, _
							New SuggestedCommandState(SuggestedCommandKey.ReverseTab, CType(sender, TextBox).Text, CType(sender, TextBox).SelectionStart))

					Else

						QueueUserWorkItem(AddressOf Handler.ProcessInput, _
							New SuggestedCommandState(SuggestedCommandKey.Tab, CType(sender, TextBox).Text, CType(sender, TextBox).SelectionStart))

					End If

					e.Handled = True

				ElseIf e.Key = Key.F1 Then

					QueueUserWorkItem(AddressOf Handler.ProcessInput, _
						New SuggestedCommandState(SuggestedCommandKey.Help, CType(sender, TextBox).Text, CType(sender, TextBox).SelectionStart))
					e.Handled = True

				ElseIf e.Key = Key.F2 Then

					AssemblyEntry.Load()
					e.Handled = True

				ElseIf e.Key = Key.F3 Then

					AlignLeft()
					e.Handled = True

				ElseIf e.Key = Key.F4 Then

					ResizeToContent()
					e.Handled = True

				ElseIf e.Key = Key.F5 Then

					AlignRight()
					e.Handled = True

				ElseIf e.Key = Key.F6

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

			Private Sub txtInput_Changed( _
				ByVal sender As Object, _
				ByVal e As System.Windows.RoutedEventArgs _
			) Handles txtInput.SelectionChanged

				InputInterface.Manage_Thread(InputChangedThread, AddressOf Handler.ProcessInput, True, True)

				InputChangedThread.Start(New SuggestedCommandState( _
					SuggestedCommandKey.Input, CType(sender, TextBox).Text, CType(sender, TextBox).SelectionStart))

			End Sub

			Private Sub txtInput_SizeChanged( _
				ByVal sender As Object, _
				ByVal e As System.Windows.SizeChangedEventArgs _
			) Handles txtInput.SizeChanged

				If e.NewSize.Width > 0 AndAlso SingleCharacterWidth > 0 Then _
					InputBoxCharacterWidth = Math.Round(e.NewSize.Width / SingleCharacterWidth, 0)

				UpdateCaret()

			End Sub

			Private Sub UpdateCaret()

				Dim linePosition As Integer = 1

				If txtInput.SelectionStart > 0 Then

					Dim caretPosition As Integer = txtInput.SelectionStart

					For i As Integer = 0 To txtInput.LineCount - 1

						If i = txtInput.LineCount - 1 OrElse _
							caretPosition < txtInput.GetCharacterIndexFromLineIndex(i + 1) Then

							caretPosition -= txtInput.GetCharacterIndexFromLineIndex(i)

							If i = txtInput.LineCount - 1 Then linePosition = txtInput.LineCount Else linePosition += i
							Exit For

						End If

					Next

					rctCaret.Margin = New Thickness(txtPrompt.Width + (caretPosition * SingleCharacterWidth) + 2, _
						(linePosition * SingleLineHeight) + 2, rctCaret.Margin.Right, rctCaret.Margin.Bottom)

				ElseIf txtPrompt.Width > 0 Then

					rctCaret.Margin = New Thickness(txtPrompt.Width + 2, (linePosition * SingleLineHeight) + 2, _
						rctCaret.Margin.Right, rctCaret.Margin.Bottom)

				Else

					rctCaret.Margin = New Thickness(0, 0, 0, 0)

				End If

				rctCaret.Width = SingleCharacterWidth

			End Sub

		#End Region

		#Region " Form Handlers "

			Private Sub ResizeToContent()

				If Not Window_Locked Then

					Dim isRightOrientated As Boolean = (Left = SystemParameters.FullPrimaryScreenWidth - Width)

					If Not String.IsNullOrEmpty(txtInput.Text) Then

						Dim totalWidth As Double = (txtInput.Text.Length * SingleCharacterWidth) + _
							txtInput.Margin.Left + txtInput.Margin.Right

						Dim currentOffset As Double = ActualWidth - txtInput.ActualWidth

						If totalWidth + currentOffset > MinWidth Then

							If totalWidth + currentOffset > SystemParameters.FullPrimaryScreenWidth Then

								Width = SystemParameters.FullPrimaryScreenWidth

							Else

								Width = totalWidth + currentOffset

							End If

							If Left + Width > SystemParameters.FullPrimaryScreenWidth OrElse isRightOrientated Then

								AlignRight()

							ElseIf Left < 0 Then

								Left = 0

							End If

						Else

							Width = MinWidth
							If isRightOrientated Then AlignRight()

						End If

					Else

						Width = MinWidth
						Height = MinHeight
						If isRightOrientated Then AlignRight()

					End If

				End If

			End Sub

			Private Sub AlignLeft()

				If Not Window_Locked Then Left = 0

			End Sub

			Private Sub AlignRight()

				If Not Window_Locked Then Left = SystemParameters.FullPrimaryScreenWidth - Width

			End Sub

			Private Sub InputInterface_Closed( _
				ByVal sender As Object, _
				ByVal e As System.EventArgs _
			) Handles Me.Closed

				For i As Integer = 0 To ExecutingWindows.Count - 1

					ExecutingWindows(i).Close()

				Next

			End Sub

			Private Sub InputInterface_Loaded( _
				ByVal sender As Object, _
				ByVal e As System.Windows.RoutedEventArgs _
			) Handles Me.Loaded

				Title = WPF_MODE_TITLE
				Prompt = WPF_MODE_PROMPT

				statStatus.Content = STATUS_WAITING
				statStatus.Foreground = New SolidColorBrush(COLOUR_NONACTIVE_TEXT)

				Dim promptText As New FormattedText(Prompt, CurrentUICulture, FlowDirection.LeftToRight, _
					New Typeface(txtPrompt.FontFamily, txtPrompt.FontStyle, txtPrompt.FontWeight, txtPrompt.FontStretch), _
					txtPrompt.FontSize, txtPrompt.Foreground)

				txtPrompt.Width = promptText.Width + 3
				txtPrompt.AppendText(Prompt)

				Dim measurement_Text As New FormattedText(DIGIT_ZERO, CurrentUICulture, FlowDirection.LeftToRight, _
					New Typeface(txtInput.FontFamily, txtInput.FontStyle, txtInput.FontWeight, txtInput.FontStretch), _
					txtInput.FontSize, txtInput.Foreground)

				SingleCharacterWidth = measurement_Text.Width
				SingleLineHeight = measurement_Text.Height + measurement_Text.LineHeight

				txtInput.Margin = New Thickness(txtInput.Margin.Left + (txtPrompt.Width - 1), _
					txtInput.Margin.Top, txtInput.Margin.Right, txtInput.Margin.Bottom)

				txtInputBackground.Margin = txtInput.Margin

				txtInput.Focus()

				Handler_UpdateSettings(Handler.SuggestionsEnabled)

				Config.Finished = DateTime.Now

			End Sub

			Private Sub Move_Window( _
				ByVal sender As Object, _
				ByVal e As MouseButtonEventArgs _
			) Handles Me.MouseLeftButtonDown

				If Not Window_Locked Then MyBase.DragMove()

			End Sub

		#End Region

		#Region " Suggestion Handlers "

			Private Sub Handler_UpdateInput( _
				ByVal newValue As String, _
				ByVal newCursorPosition As Integer _
			) Handles m_Handler.UpdateInput

				If Dispatcher.CheckAccess Then

					RemoveHandler txtInput.SelectionChanged, AddressOf Me.txtInput_Changed

					txtInput.Text = newValue

					If txtInput.SelectionStart <> newCursorPosition Then
						txtInput.SelectionStart = newCursorPosition
						txtInput.SelectionLength = 0
					End If

					UpdateCaret()

					AddHandler txtInput.SelectionChanged, AddressOf Me.txtInput_Changed

				Else

					' Update using the UI Thread Invoke.
					Dispatcher.Invoke(New UpdateInputDelegate(AddressOf Handler_UpdateInput), newValue, newCursorPosition)

				End If

			End Sub

			Private Sub Handler_UpdateOutput( _
				ByVal newValue As String _
			) Handles m_Handler.UpdateOutput

				If Dispatcher.CheckAccess Then

					txtInputBackground.Text = newValue

				Else

					' Update using the UI Thread Invoke.
					Dispatcher.Invoke(New UpdateOutputDelegate(AddressOf Handler_UpdateOutput), newValue)

				End If

			End Sub

			Private Sub Handler_UpdateSettings( _
				ByVal suggestionsEnabled As Boolean _
			) Handles m_Handler.UpdateSettings

				If Dispatcher.CheckAccess Then

					If suggestionsEnabled Then

						statSuggestionsEnabled.Foreground = New SolidColorBrush(COLOUR_ACTIVE_TEXT)

					Else

						statSuggestionsEnabled.Foreground = New SolidColorBrush(COLOUR_NONACTIVE_TEXT)

					End If

					statSuggestionsEnabled.Content = String.Format(SETTINGS_SUGGESTIONS, suggestionsEnabled.ToString)

				Else

					Dispatcher.Invoke(New UpdateSettingsDelegate(AddressOf Handler_UpdateSettings), suggestionsEnabled)

				End If

			End Sub

			Private Sub Handler_ExecuteCommand( _
				ByVal command As String _
			) Handles m_Handler.ExecuteCommand

				If Dispatcher.CheckAccess Then

					If String.Compare(command, WPF_MODE_EXIT, True) = 0 Then

						Me.Close()

					ElseIf String.Compare(command, WPF_MODE_CLEAR, True) = 0 Then

						SyncLock ExecutingThreads_LOCK

							For i As Integer = 0 To ExecutingWindows.Count - 1

								If ExecutingWindows(i).Completed Then

									ExecutingWindows(i).ShowActivated = False
									ExecutingWindows(i).Close()
									ExecutingWindows.RemoveAt(i)

									i -= 1

								End If

								If i >= ExecutingWindows.Count - 1 Then Exit For

							Next

						End SyncLock

					Else

						Dim executionThread As New System.Threading.Thread(AddressOf ExecuteCommand)

						' -- TEMP for XAML Conversion --
						' executionThread.SetApartmentState(ApartmentState.STA)
						' executionThread.IsBackground = False
						' ------------------------------

						ExecutedThreadsCount += 1
						executionThread.Name = String.Format("{0}: {1}", ExecutedThreadsCount, command)

						Dim frmOutput As New OutputInterface(command, ExecutedThreadsCount)
						If Opacity_Locked Then
							frmOutput.OPACITY_ACTIVE = 1
							frmOutput.Opacity_Locked = True
						End If
						frmOutput.Title = WPF_MODE_OUTPUTTITLE

						SyncLock ExecutingThreads_LOCK

							ExecutingThreads.Add(executionThread)

							ExecutingWindows.Add(frmOutput)

							If RecentCommands.Contains(command) Then RecentCommands.Remove(command)

							RecentCommands.Add(command)
							RecentCommandPosition = 0

							If ExecutingThreads.Count >= 1 Then

								If ExecutingThreads.Count = 1 Then
									statStatus.Content = STATUS_WORKING_SINGULAR
								Else
									statStatus.Content = String.Format(STATUS_WORKING_MULTIPLE, ExecutingThreads.Count)
								End If

								statStatus.Foreground = New SolidColorBrush(COLOUR_ACTIVE_TEXT)

							Else

								statStatus.Content = STATUS_WAITING
								statStatus.Foreground = New SolidColorBrush(COLOUR_NONACTIVE_TEXT)

							End If

						End SyncLock

						AddHandler frmOutput.Closed, AddressOf OutputInterface_Closed

						frmOutput.Show()
						executionThread.Start(frmOutput)

						BringIntoView()
						Focus()
						txtInput.Focus()

					End If

				Else

					Dispatcher.Invoke(New ExecuteCommandDelegate(AddressOf Handler_ExecuteCommand), command)

				End If

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

				If Not Window_Locked Then Me.Close()

			End Sub

		#End Region

		#Region " Footer Handlers "

			Private Sub statStatus_MouseLeftButtonDown( _
				ByVal sender As Object, _
				ByVal e As System.Windows.Input.MouseButtonEventArgs _
			) Handles statStatus.MouseLeftButtonDown

				If Not Window_Locked AndAlso e.ClickCount = 2 Then Left = 0

			End Sub

			Private Sub statSuggestionsEnabled_MouseLeftButtonDown( _
				ByVal sender As Object, _
				ByVal e As System.Windows.Input.MouseButtonEventArgs _
			) Handles statSuggestionsEnabled.MouseLeftButtonDown

				If Not Window_Locked AndAlso e.ClickCount = 2 Then Left = SystemParameters.FullPrimaryScreenWidth - Width

			End Sub

		#End Region

	#End Region

	#Region " Private Methods "

		'''<summary>Cycles through the Cache/Updates cache content box</summary>
		'''<remarks>If 0 is passed, the content box will be cleared if cache is empty</remarks>
		Private Sub CycleCache( _
			ByVal interval As Integer _
		)

			If CacheList.Count > 0 OrElse interval = 0 Then

				If Dispatcher.CheckAccess Then

					If CacheList.Count = 0 Then

						statCache.Content = Nothing

					Else

						If interval <> 0 Then CachePosition = CalculateCycleShift(CacheList.Count, CachePosition, interval)
						statCache.Content = CacheList(CachePosition)

						If CachePosition <> 0 Then _
							Clipboard.SetText(CacheList(CachePosition).Substring(0, CacheList(CachePosition).LastIndexOf(COLON)))

					End If

				Else

					Dispatcher.Invoke(New UpdateExecuteStatusDelgate(AddressOf CycleCache), interval)

				End If

			End If

		End Sub

		Private Sub CycleRecentCommands( _
			ByVal interval As Integer _
		)

			If RecentCommands.Count > 1 Then

				RecentCommandPosition = CalculateCycleShift(RecentCommands.Count, RecentCommandPosition, interval)

				Dim recentCommand As String = RecentCommands(RecentCommandPosition)

				If String.IsNullOrEmpty(recentCommand) Then

					Handler.Input.Clear()

				Else

					Handler.Input.SetValue(recentCommand, recentCommand.Length)

				End If

			End If

		End Sub

		''' <summary></summary>
		''' <param name="frmOutput"></param>
		Private Sub ExecuteCommand( _
			ByVal frmOutput As OutputInterface _
		)

			Dim executor As New CommandExecution(CommandLineParse(frmOutput.Command), frmOutput)

			executor.Run()

			ExecutionCompleted(frmOutput.Id, True)

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

		''' <summary></summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		Private Sub OutputInterface_Closed( _
			ByVal sender As Object, _
			ByVal e As System.EventArgs _
		)

			If Not CType(sender, OutputInterface).Completed Then ExecutionCompleted(CType(sender, OutputInterface).Id, False)

		End Sub

		''' <summary></summary>
		''' <param name="id"></param>
		''' <param name="completed"></param>
		Private Sub ExecutionCompleted( _
			ByVal id As Int32, _
			ByVal completed As Boolean _
		)

			If Dispatcher.CheckAccess Then

				SyncLock ExecutingThreads_LOCK

					For i As Integer = 0 To ExecutingThreads.Count - 1

						If ExecutingThreads(i).Name.StartsWith(id.ToString() & COLON) Then

							If Not completed AndAlso ExecutingThreads(i).IsAlive Then ExecutingThreads(i).Abort

							ExecutingThreads.RemoveAt(i)

							If ExecutingThreads.Count >= 1 Then

								If ExecutingThreads.Count = 1 Then
									statStatus.Content = STATUS_WORKING_SINGULAR
								Else
									statStatus.Content = String.Format(STATUS_WORKING_MULTIPLE, ExecutingThreads.Count)
								End If

								statStatus.Foreground = New SolidColorBrush(COLOUR_ACTIVE_TEXT)

							Else

								statStatus.Content = STATUS_WAITING
								statStatus.Foreground = New SolidColorBrush(COLOUR_NONACTIVE_TEXT)

							End If

						Exit For

					End If

				Next

				For i As Integer = 0 To ExecutingWindows.Count - 1

					If ExecutingWindows(i).Id = id Then

						RemoveHandler ExecutingWindows(i).Closed, AddressOf OutputInterface_Closed

						ExecutingWindows(i).Completed = completed

						Thread.Sleep(100)

						If ExecutingWindows(i).CompletedWithoutOutput Then

							ExecutingWindows(i).Close()

							ExecutingWindows.RemoveAt(i)

						ElseIf Not ExecutingWindows(i).Completed Then

							ExecutingWindows.RemoveAt(i)

						End If

						Exit For

					End If

				Next

				End SyncLock

			Else

				Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Normal, _
					New ExecutionCompletedDelegate(AddressOf ExecutionCompleted), id, completed)

			End If

		End Sub

	#End Region

	#Region " Public Shared Methods "

		Public Shared Sub Manage_Thread( _
			ByRef value As System.Threading.Thread, _
			ByVal startDelegate As System.Threading.ParameterizedThreadStart, _
			ByVal abort As Boolean, _
			ByVal wait As Boolean, _
			Optional ByVal interval As Int32 = 5, _
			Optional ByVal totalWaits As Int32 = 100 _
		)

			If Not value Is Nothing Then

				If value.IsAlive AndAlso abort Then value.Abort

				If wait Then

					Dim current_Loop As Integer = 0

					Do While value.IsAlive

						Thread.Sleep(interval)

						current_Loop += 1
						If current_Loop >= totalWaits Then Exit Do

					Loop

				End If

			End If

			If Not startDelegate Is Nothing Then value = New System.Threading.Thread(startDelegate)

		End Sub

	#End Region

End Class
