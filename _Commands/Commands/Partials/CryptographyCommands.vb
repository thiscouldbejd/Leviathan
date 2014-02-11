Imports Hermes.Cryptography
Imports System.Convert
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.Encoding
Imports IT = Leviathan.Visualisation.InformationType

Namespace Commands

	Partial Public Class CryptographyCommands

		#Region " Command Processing Commands "

			<Command( _
				ResourceContainingType:=GetType(CryptographyCommands), _
				ResourceName:="CommandDetails", _
				Name:="encrypt", _
				Description:="@commandCryptographyDescriptionEncrypt@" _
			)> _
			Public Function ProcessCommandEncrypt( _
				<Configurable( _
					ResourceContainingType:=GetType(CryptographyCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandCryptographyParameterDescriptionValue@" _
				)> _
				ByVal value As String, _
				<Configurable( _
					ResourceContainingType:=GetType(CryptographyCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandCryptographyParameterDescriptionKey@" _
				)> _
				ByVal key As String _
			) As Visualisation.IFixedWidthWriteable

				Return Visualisation.Message.Create(CRYPTOGRAPHY_COMMAND_RESULTS, _
					IT.General, CRYPTOGRAPHY_CIPHER_TEXT, Cipher.Encrypt(value, key))

			End Function

			<Command( _
				ResourceContainingType:=GetType(CryptographyCommands), _
				ResourceName:="CommandDetails", _
				Name:="hash", _
				Description:="@commandCryptographyDescriptionHash@" _
			)> _
			Public Function ProcessCommandHash( _
				<Configurable( _
					ResourceContainingType:=GetType(CryptographyCommands), _
					ResourceName:="CommandDetails", _
					Description:="@commandCryptographyParameterDescriptionValue@" _
				)> _
				ByVal value As String _
			) As Visualisation.IFixedWidthWriteable

				Return Visualisation.Message.Create(CRYPTOGRAPHY_COMMAND_RESULTS, _
					IT.General, CRYPTOGRAPHY_HASH_TEXT, _
					Cipher.Generate_Salted_Hash(value, Nothing, HashType.SHA1))

			End Function

		#End Region

	End Class

End Namespace