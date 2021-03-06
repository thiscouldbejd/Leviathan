Namespace Commands

	''' <summary>This attribute should be applied to fields/properties in a provider class. These fields will then be parsed from the configuration. Do not use field initialised variables if applying this attribute to a variable as the configured variable will likely be over-written by the initialised variable due to the instantiation stack for an object. Use the Default Value property instead.</summary>
	''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
	''' <generator-date>10/02/2014 15:42:16</generator-date>
	''' <generator-functions>1</generator-functions>
	''' <generator-source>Leviathan\_Commands\Attributes\Generated\ParsingTypeParameterAttribute.tt</generator-source>
	''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
	''' <generator-version>1</generator-version>
	<System.CodeDom.Compiler.GeneratedCode("Leviathan\_Commands\Attributes\Generated\ParsingTypeParameterAttribute.tt", "1")> _
	<AttributeUsageAttribute(AttributeTargets.Parameter, _
		AllowMultiple:=False, Inherited:=True)> _
	Partial Public NotInheritable Class ParsingTypeParameterAttribute
		Inherits System.Attribute

		#Region " Public Constructors "

			''' <summary>Default Constructor</summary>
			Public Sub New()

				MyBase.New()

			End Sub

		#End Region

		#Region " Class Plumbing/Interface Code "

			#Region " Attributable Implementation "

				#Region " Public Shared Methods "

					Public Shared Function GetMember( _
						ByVal onType As Type _
					) As Leviathan.Inspection.MemberAnalyser

						Return GetSingleObject(GetMembers(onType))

					End Function

					Public Shared Function GetMembers( _
						ByVal onType As Type _
					) As Leviathan.Inspection.MemberAnalyser()

						Return Leviathan.Inspection.TypeAnalyser.GetInstance(onType) _
							.ExecuteQuery(Leviathan.Inspection.AnalyserQuery.QUERY_MEMBERS_READABLE _
								.SetPresentAttribute(GetType(ParsingTypeParameterAttribute)) _
						)

					End Function

				#End Region

			#End Region

		#End Region

	End Class

End Namespace