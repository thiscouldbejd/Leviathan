Imports Leviathan.Inspection.AnalyserQuery
Imports System.IO
Imports System.Xml

Namespace Configuration

	Partial Public Class ConfigurationFactory

		#Region " Private Constants "

			Private Const DEFAULT_SUFFIX As String = "config"

			Private Const DEFAULT_DIRECTORY As String = "configuration"

		#End Region

		#Region " Private Methods "

			Private Sub Configure( _
				ByRef target As Object, _
				ByVal analyser As TypeAnalyser, _
				ByVal ignore As MemberAnalyser(), _
				Optional ByVal name As String = Nothing, _
				Optional ByVal suffix As String = DEFAULT_SUFFIX, _
				Optional ByVal directory As String = DEFAULT_DIRECTORY _
			)

				If String.IsNullOrEmpty(name) Then name = analyser.FullName
				Dim foundConfig As Boolean

				Dim configs As Stream() = Files.ParseFileStreamsFromString( _
					String.Format(".{0}\{1}.{2}.xml", directory, name, suffix), foundConfig, False)

				Dim config As Stream

				If Not foundConfig Then

					config = analyser.Type.Assembly.GetManifestResourceStream( _
						String.Format("{0}.{1}.{2}.xml", Base, name, suffix))

					If config Is Nothing Then exit Sub

				Else

					config = configs(0)

				End If

				Dim config_Reader As New XmlTextReader(config)

				target = New XmlSerialiser(config_Reader, Nothing, Parser).Read(target, ignore)

				config_Reader.Close()

			End Sub

		#End Region

		#Region " Public Methods "

			Public Sub Configure( _
				ByRef target As Object, _
				ByVal name As String, _
				ByVal suffix As String, _
				ByVal directory As String, _
				ParamArray ByVal ignore As MemberAnalyser() _
			)

				Configure(target, TypeAnalyser.GetInstance(target.GetType), ignore, name, suffix, directory)

			End Sub

			Public Sub Configure( _
				ByRef target As Object, _
				ParamArray ByVal ignore As MemberAnalyser() _
			)

				Configure(target, TypeAnalyser.GetInstance(target.GetType), ignore)

			End Sub

		#End Region

	End Class

End Namespace
