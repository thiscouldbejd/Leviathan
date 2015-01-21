Imports Hermes.Cryptography
Imports Hermes.Email
Imports Leviathan.Visualisation
Imports System.Net.Mail
Imports IT = Leviathan.Visualisation.InformationType

Namespace Commands

	Partial Public Class CommandEmailOutput
		Implements ICommandsOutput
		Implements ICommandsOutputWriter

		#Region " Private Constants "

			Private Shared HTML_SPACE As String = "&nbsp"

		#End Region

		#Region " Protected Constants "

			Protected Shared COLOUR_BACKGROUND As String = "#000000"

			Protected Shared COLOUR_GENERAL As String = "#DDDDDD"

			Protected Shared COLOUR_PERFORMANCE As String = "#808000"

			Protected Shared COLOUR_SUCCESS As String = "#00FF00"

			Protected Shared COLOUR_INFORMATION As String = "#0000FF"

			Protected Shared COLOUR_WARNING As String = "#FFFF00"

			Protected Shared COLOUR_ERROR As String = "#FF0000"

			Protected Shared COLOUR_DEBUG As String = "#808080"

			Protected Shared COLOUR_LOG As String = "#808080"

			Protected Shared COLOUR_QUESTION As String = "#FFFFFF"

		#End Region

		#Region " Private Properties "

			Private ReadOnly Property Font_Size As Integer
				Get
					If CharacterWidth >= 300 Then
						Return 10
					ElseIf CharacterWidth >= 250 Then
						Return 11
					ElseIf CharacterWidth >= 200 Then
						Return 12
					Else
						Return 13
					End If
				End Get
			End Property

		#End Region

		#Region " Private Methods "

			Private Sub Write_HtmlStart()

				Email_Message.AppendLine("<html>")
				Email_Message.AppendLine(String.Format("<body lang='EN-GB' style='background-color: {0}'>", COLOUR_BACKGROUND))
				Email_Message.AppendLine(String.Format("<div style='FONT-FAMILY: Lucida Console, Courier New, Courier; COLOR: #000000; FONT-SIZE: {0}px'>", Font_Size))

			End Sub

			Private Sub Write_HtmlEnd()

				Email_Message.AppendLine("</div>")
				Email_Message.AppendLine("</body>")
				Email_Message.AppendLine("</html>")

			End Sub

		#End Region

		#Region " ICommandsOutput Implementation "

			Private Sub Write_Outputs( _
				ParamArray ByVal values As IFixedWidthWriteable() _
			) Implements ICommandsOutput.Show_Outputs

				If Not values Is Nothing Then

					For i As Integer = 0 To values.Count - 1

						If HasWritten OrElse i > 0 Then
							TerminateLine()
						ElseIf UseHtml Then
							Write_HtmlStart()
						End If

						HasWritten = True

						values(i).Write(CharacterWidth, Me, 0)

					Next

				End If

			End Sub

			Private Sub Write_Close() Implements ICommandsOutput.Close

				If Email_Message.BodyLength > 0 Then

					' -- Terminate the HTML if required --
					If UseHtml Then
						Write_HtmlEnd()
						Email_Message.Body_Format = BodyType.Html
					End If

					' -- Set the Message Subject --
					Email_Message.Subject = Subject

					' -- Set Up the Hermes Recipient List --
					Dim _distribution As New Distribution
					_distribution.Sending_Format = SendingType.Single_Recipient
					_distribution.Add(ToAddresses)

					' -- Set the From/Reply-To --
					Dim _from As System.Net.Mail.MailAddress
					If Not String.IsNullOrEmpty(FromAddress) Then
						_from = Hermes.Email.Manager.CreateAddress(FromAddress, FromDisplay)
					Else
						_from = Hermes.Email.Manager.CreateAddress(Cipher.Create_Password(12, 0).ToLower() & _
							Default_From_Domain, "Leviathan")
					End If
					Dim _replyTo As System.Net.Mail.MailAddress = Nothing
					If Not String.IsNullOrEmpty(ReplyToAddress) Then _replyTo = _
						Hermes.Email.Manager.CreateAddress(ReplyToAddress, ReplyToDisplay)

					' -- Set Up the Hermes Manager and Send Email --
					Dim _manager = new Hermes.Email.Manager()
					_manager.Suppress_Send = Suppress
					_manager.SMTP_MaxBatch = 20
					_manager.Client_Application = "Leviathan"
					If Not Log Is Nothing Then _manager.Logging_Directory = Log
					_manager.SMTP_Server = Hermes.Email.Manager.CreateServer(Server)
					_manager.SMTP_Port = ServerPort
					_manager.Use_SSL = ServerSSL
					_manager.Verify_SSL = ServerValidateCertificate
					If String.IsNullOrEmpty(ServerUsername) Then
						_manager.SMTP_Credential = Hermes.Email.Manager.CreateIntegratedCredential()
					Else
						_manager.SMTP_Credential = Hermes.Email.Manager.CreateCredential(ServerUsername, ServerPassword, ServerDomain)
					End If

					' -- Send Email --
					If _replyTo Is Nothing Then
						_manager.SendMessage(_from, _Distribution, Email_Message, Nothing, "Command Email Output")
					Else
						_manager.SendMessage(_from, _Distribution, Email_Message, Nothing, "Command Email Output", _replyTo)
					End If

				End If

			End Sub

		#End Region

		#Region " ICommandsOutputWriter Implementation "

			Private Sub Write( _
				ByVal value As String, _
				ByVal type As InformationType _
			) _
			Implements ICommandsOutputWriter.Write

				If Not String.IsNullOrEmpty(value) Then

					If UseHtml Then

						Select Case type

							Case IT.General

								Email_Message.AppendFormat("<font color='{0}'>{1}</font>", COLOUR_GENERAL, value)

							Case IT.Debug

								Email_Message.AppendFormat("<font color='{0}'>{1}</font>", COLOUR_DEBUG, value)

							Case IT.Log

								Email_Message.AppendFormat("<font color='{0}'>{1}</font>", COLOUR_LOG, value)

							Case IT.[Error]

								Email_Message.AppendFormat("<font color='{0}'>{1}</font>", COLOUR_ERROR, value)

							Case IT.Information

								Email_Message.AppendFormat("<font color='{0}'>{1}</font>", COLOUR_INFORMATION, value)

							Case IT.Performance

								Email_Message.AppendFormat("<font color='{0}'>{1}</font>", COLOUR_PERFORMANCE, value)

							Case IT.Success

								Email_Message.AppendFormat("<font color='{0}'>{1}</font>", COLOUR_SUCCESS, value)

							Case IT.Warning

								Email_Message.AppendFormat("<font color='{0}'>{1}</font>", COLOUR_WARNING, value)

							Case IT.Question

								Email_Message.AppendFormat("<font color='{0}'>{1}</font>", COLOUR_QUESTION, value)

							Case Else

								Email_Message.Append(value)

						End Select

					Else

						Email_Message.Append(value)

					End If

				End If

			End Sub

			Private Function FixedWidthWrite( _
				ByRef value As String, _
				ByVal width As Integer, _
				ByVal type as InformationType _
			) As Boolean _
			Implements ICommandsOutputWriter.FixedWidthWrite

				Dim returnValue As Boolean
				Dim lengthWritten As Integer

				If Not String.IsNullOrEmpty(value) Then _
					Write(CubeControl.MaxWidthWrite(value, width, lengthWritten, returnValue), type)

				' Write the Padding Spaces
				If UseHtml Then
					For i As Integer = 0 To (width - lengthWritten) - 1
						Write(HTML_SPACE, InformationType.None)
					Next
				Else
					Write(New String(SPACE, width - lengthWritten), InformationType.None)
				End If

				Return returnValue

			End Function

			Private Sub TerminateLine( _
				Optional numberOfTimes As System.Int32 = 1 _
			) _
			Implements ICommandsOutputWriter.TerminateLine

				For i As Integer = 1 To numberOfTimes

					If UseHtml Then
						Email_Message.AppendLine("<br />")
					Else
						Email_Message.AppendLine()
					End If

				Next

			End Sub

		#End Region

	End Class

End Namespace
