Imports Leviathan.Visualisation

Namespace Commands

	Partial Public Class CommandPerformance
	
		#Region " Public Methods "
		
			''' <summary>
			''' Method to Log a Performance Event.
			''' </summary>
			''' <param name="workDone">The Work that was done since the last Event or the Start of the Process</param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Function LogEvent( _
				ByVal workDone As String _
			) As CommandPerformance
			
				Return LogEvent(workDone, Nothing)
				
			End Function
			
			''' <summary>
			''' Method to Log a Performance Event.
			''' </summary>
			''' <param name="workDone">The Work that was done since the last Event or the Start of the Process</param>
			''' <param name="workDetails">The Work Details (e.g. Arguments)</param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Function LogEvent( _
				ByVal workDone As String, _
				ByVal workDetails As String _
			) As CommandPerformance
			
				Dim [event] As New CommandPerformanceSnapshot(workDone, workDetails, Events.Count + 1)
				Events.Add([event])
				
				Return Me
				
			End Function
			
			''' <summary>
			''' Method to Generate Performance Results for Display.
			''' </summary>
			''' <returns>A Formatted Table.</returns>
			''' <remarks>Need to create an aggregate table with max, min, average sd etc results for each method...use workDone as a key?</remarks>
			Public Function GenerateResults() As IFixedWidthWriteable()
			
				Dim methodPerformance As New Slice()
				
				Dim lastMemory As Int64 = Start.WorkingSet, lastCreated As DateTime = Start.Created
				Dim aryEvents As CommandPerformanceSnapshot() = Events.ToArray
				
				' -- Iterate through the Events --
				For i As Integer = 0 To aryEvents.Length - 1
				
					' Calculate the Time Taken/Memory Used from Last Event
					aryEvents(i).TimeTaken = aryEvents(i).Created.Subtract(lastCreated)
					aryEvents(i).MemoryUsed = Math.Round((aryEvents(i).WorkingSet - lastMemory) / 1024)
					
					If i < aryEvents.Length - 2 OrElse i > 0 AndAlso (aryEvents(i).WorkDone = aryEvents(i - 1).WorkDone) Then
					
						' Set the Last Event Details to the Current Event Details
						lastMemory = aryEvents(i).WorkingSet
						lastCreated = aryEvents(i).Created
						
					Else ' If the next Event is the Last Event, set back to the Start (as every command has a root!) unless it's a repeated command!
					
						lastMemory = Start.WorkingSet
						lastCreated = Start.Created
						
					End If
					
				Next
				
				' Sort the Events (most time consuming on top!)
				Array.Sort(aryEvents, New Comparison.Comparer(ComponentModel.ListSortDirection.Descending))
				
				' Aggregate Timings
				Dim aggregatePerCommandTimings As New Dictionary(Of String, List(Of Int64))
				
				' -- Iterate through the Events --
				For i As Integer = 0 To aryEvents.Length - 1
				
					' Add the Formatted Row for Performance
					methodPerformance.Add(aryEvents(i).WorkDone, _
						New TimeSpanConvertor().ParseStringFromTimespan(aryEvents(i).TimeTaken, New Boolean), _
						New LongConvertor().ParseStringFromLong(aryEvents(i).MemoryUsed, New Boolean, Nothing) & "kb", _
						aryEvents(i).Order _
					)
					
					' Add a Details Row if required!
					If Not String.IsNullOrEmpty(aryEvents(i).Details) Then _
						methodPerformance.Rows.Add(New Row().Add(New Cell(aryEvents(i).Details, 80)).Add().Add().Add())
						
					If Not aggregatePerCommandTimings.ContainsKey(aryEvents(i).WorkDone) Then _
						aggregatePerCommandTimings.Add(aryEvents(i).WorkDone, New List(Of Int64))
						
					aggregatePerCommandTimings(aryEvents(i).WorkDone).Add(aryEvents(i).TimeTaken.Ticks)
					
				Next
				
				Dim aggregatePerformance As Slice = Nothing
				
				For Each command As String In aggregatePerCommandTimings.Keys
				
					If aggregatePerCommandTimings(command).Count > 1 Then
					
						If aggregatePerformance Is Nothing Then aggregatePerformance = New Slice()
						
						Dim min As Int64 = Int64.MaxValue
						Dim max As Int64 = Int64.MinValue
						Dim count As Int64 = aggregatePerCommandTimings(command).Count
						Dim total As Int64
						
						For Each value As Int64 in aggregatePerCommandTimings(command)
						
							min = Math.Min(min, value)
							max = Math.Max(max, value)
							total += value
							
						Next
						
						aggregatePerformance.Add(command, count, _
							New TimeSpanConvertor().ParseStringFromTimespan(New TimeSpan(min), New Boolean), _
							New TimeSpanConvertor().ParseStringFromTimespan(New TimeSpan(max), New Boolean), _
							New TimeSpanConvertor().ParseStringFromTimespan(New TimeSpan(Convert.ToInt64(Math.Round(total/count, 0))), New Boolean)
						)
						
					End If
					
				Next
				
				If aggregatePerformance Is Nothing Then
					
					Return New IFixedWidthWriteable() {Cube.Create(InformationType.Performance, PERFORMANCE_TITLE, PERFORMANCE_COLUMN_WORKDONE, PERFORMANCE_COLUMN_TOOK, _
						PERFORMANCE_COLUMN_MEMWORKINGSET, PERFORMANCE_COLUMN_ORDER).Add(methodPerformance)}
					
				Else
					
					Return New IFixedWidthWriteable() { _
						Cube.Create(InformationType.Performance, PERFORMANCE_TITLE, PERFORMANCE_COLUMN_WORKDONE, PERFORMANCE_COLUMN_TOOK, _
							PERFORMANCE_COLUMN_MEMWORKINGSET, PERFORMANCE_COLUMN_ORDER).Add(methodPerformance), _
						Cube.Create(InformationType.Performance, PERFORMANCE_TITLE, PERFORMANCE_COLUMN_WORKDONE, PERFORMANCE_COLUMN_COUNT, _
							PERFORMANCE_COLUMN_MIN, PERFORMANCE_COLUMN_MAX, PERFORMANCE_COLUMN_AVERAGE).Add(aggregatePerformance) _
					}
					
				End If
				
			End Function
			
		#End Region
		
	End Class
	
End Namespace