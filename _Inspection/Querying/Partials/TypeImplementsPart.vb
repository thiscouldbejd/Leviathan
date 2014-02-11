Namespace Inspection

    Partial Public Class TypeImplementsPart
        Implements IPostQueryPart

#Region " IPostQueryPart Implementation "

        Public Function CheckCompliancy( _
            ByVal value As FieldInfo _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Return [Implements] Is Nothing OrElse ([Implements].IsInterface AndAlso _
                TypeAnalyser.DoesTypeImplementInterface(value.FieldType, [Implements]))

        End Function

        Public Function CheckCompliancy( _
            ByVal value As MethodInfo _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Return [Implements] Is Nothing OrElse ([Implements].IsInterface AndAlso _
                Not value.ReturnType Is Nothing AndAlso _
                TypeAnalyser.DoesTypeImplementInterface(value.ReturnType, [Implements]))

        End Function

        Public Function CheckCompliancy( _
            ByVal value As ConstructorInfo _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Return False

        End Function

        Public Function CheckCompliancy( _
            ByVal value As PropertyInfo _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Return [Implements] Is Nothing OrElse ([Implements].IsInterface AndAlso _
                TypeAnalyser.DoesTypeImplementInterface(value.PropertyType, [Implements]))

        End Function

        Public Function CheckCompliancy( _
            ByVal value As System.Attribute _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Return CheckCompliancy(value.GetType)

        End Function

        Public Function CheckCompliancy( _
            ByVal value As System.Exception _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Return CheckCompliancy(value.GetType)

        End Function

        Public Function CheckCompliancy( _
            ByVal value As System.Type _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Return [Implements] Is Nothing OrElse ([Implements].IsInterface AndAlso _
                TypeAnalyser.DoesTypeImplementInterface(value, [Implements]))

        End Function

#End Region

    End Class

End Namespace