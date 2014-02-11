Namespace Inspection

    Partial Public Class ArgumentPart
        Implements IPostQueryPart

#Region " IPostQueryPart Implementation "

        Public Overridable Function CheckCompliancy( _
            ByVal value As FieldInfo _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Return ArgumentCount <= 0

        End Function

        Public Overridable Function CheckCompliancy( _
            ByVal value As MethodInfo _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Return ArgumentCount < 0 OrElse (value.GetParameters.Length = ArgumentCount)

        End Function

        Public Function CheckCompliancy( _
            ByVal value As ConstructorInfo _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Return ArgumentCount < 0 OrElse (value.GetParameters.Length = ArgumentCount)

        End Function

        Public Overridable Function CheckCompliancy( _
            ByVal value As PropertyInfo _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Return ArgumentCount < 0 OrElse (value.GetIndexParameters.Length = ArgumentCount)

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

        Public Overridable Function CheckCompliancy( _
            ByVal value As System.Type _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Return False

        End Function

#End Region

    End Class

End Namespace