Imports System.Runtime.InteropServices
Imports System.Security

Namespace Commands

	Partial Public Class CommandPerformanceTimer

		#Region " Private Functions "

			Private Function FormatTime( _
				ByVal start_Time As Long, _
				ByVal end_Time As Long _
			) As Double

				Return (Convert.ToDouble(CLng((end_Time - start_Time))) / Convert.ToDouble(Me.ticksPerSecond))

			End Function

			Private Function GetTime() As Long

				Dim timeNow As Long
				CommandPerformanceTimer.QueryPerformanceCounter((timeNow))
				Return timeNow

			End Function

		#End Region

		#Region " Public Functions "

			Public Sub Start()

				If Not CommandPerformanceTimer.QueryPerformanceFrequency((Me.ticksPerSecond)) Then _
					Throw New ApplicationException("Performance counter not available, timer cannot be used")

				CommandPerformanceTimer.QueryPerformanceCounter((Me.startTime))

				Me.endTime = Me.startTime
				Me.lapTime = Me.startTime

			End Sub

			Public Sub [Stop]()

				CommandPerformanceTimer.QueryPerformanceCounter((Me.endTime))

			End Sub

			Public Function GetElapsedTime() As Double

				Return Me.FormatTime(Me.startTime, Me.GetTime)

			End Function

			Public Function GetLapTime() As Double

				Dim dt As Double = Me.FormatTime(Me.lapTime, Me.GetTime)
				Me.lapTime = Me.GetTime
				Return dt

			End Function

		#End Region

		#Region " COM Imported Functions "

			<SuppressUnmanagedCodeSecurity, DllImport("kernel32", CharSet:=CharSet.Ansi, SetLastError:=True, ExactSpelling:=True)> _
			Private Shared Function QueryPerformanceCounter(ByRef PerformanceCount As Long) As Boolean
			End Function

			<SuppressUnmanagedCodeSecurity, DllImport("kernel32", CharSet:=CharSet.Ansi, SetLastError:=True, ExactSpelling:=True)> _
			Private Shared Function QueryPerformanceFrequency(ByRef PerformanceFrequency As Long) As Boolean
			End Function

			<SuppressUnmanagedCodeSecurity, DllImport("winmm.dll", CharSet:=CharSet.Ansi, SetLastError:=True, ExactSpelling:=True)> _
			Private Shared Function timeGetTime() As Integer
			End Function

		#End Region

	End Class

End Namespace
