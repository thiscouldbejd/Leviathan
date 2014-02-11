Namespace Commands

	Public Structure CommandProgress
	
		Public Value As Single
		Public Name As String
		Public Details As String
		Public Queued As DateTime
				
		Public Sub New( _
			ByVal value As Single, _
			ByVal name As String, _
			ByVal details As String, _
			ByVal queued As DateTime _
		)
		
			Me.Value = value
			Me.Name = name
			Me.Details = details
			Me.Queued = queued
			
		End Sub
		
	End Structure

End Namespace