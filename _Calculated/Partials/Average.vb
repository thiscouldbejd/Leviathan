Namespace Calculated

	Partial Public Class Average

		#Region " Public Methods "

			Public Overrides Function ToString() As String

				Return (Math.Round(Value / Count, DecimalPlaces)).ToString

			End Function

		#End Region

	End Class

End Namespace