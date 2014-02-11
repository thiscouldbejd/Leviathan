Namespace Calculated
	
	Partial Public Class Percentage
		
		#Region " Public Methods "
			
			Public Overrides Function ToString() As String
				
				Return (Math.Round((Count / Total) * 100, DecimalPlaces)).ToString & PERCENTAGE_MARK
				
			End Function
			
		#End Region
		
	End Class

End Namespace