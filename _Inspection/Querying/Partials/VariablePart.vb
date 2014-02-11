Namespace Inspection

    Partial Public Class VariablePart
        Implements IPostQueryPart

#Region " IPostQueryPart Implementation "

        Public Function CheckCompliancy( _
            ByVal value As FieldInfo _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            If Me.Value Then

                Return Not (value.IsStatic)

            Else

                Return value.IsStatic

            End If

        End Function

        Public Function CheckCompliancy( _
            ByVal value As MethodInfo _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Return Not Me.Value

        End Function

        Public Function CheckCompliancy( _
            ByVal value As ConstructorInfo _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Return Not Me.Value

        End Function

        Public Function CheckCompliancy( _
            ByVal value As PropertyInfo _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            Return Not Me.Value

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

            Return Not Me.Value

        End Function

#End Region

    End Class

End Namespace