Namespace Inspection

    Partial Public Class TypePart
        Implements IPostQueryPart

#Region " IPostQueryPart Implementation "

        Public Function CheckCompliancy( _
            ByVal value As FieldInfo _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Return SubTypeOf Is Nothing OrElse _
                TypeAnalyser.IsSubClassOf(value.FieldType, SubTypeOf)

        End Function

        Public Function CheckCompliancy( _
            ByVal value As MethodInfo _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Return SubTypeOf Is Nothing OrElse (Not value.ReturnType Is Nothing _
                AndAlso TypeAnalyser.IsSubClassOf(value.ReturnType, SubTypeOf))

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

            Return SubTypeOf Is Nothing OrElse _
                TypeAnalyser.IsSubClassOf(value.PropertyType, SubTypeOf)

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

            Return SubTypeOf Is Nothing OrElse (Not value Is Nothing _
                AndAlso TypeAnalyser.IsSubClassOf(value, SubTypeOf))

        End Function

#End Region

    End Class

End Namespace