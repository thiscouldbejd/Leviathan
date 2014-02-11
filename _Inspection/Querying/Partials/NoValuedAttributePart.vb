Namespace Inspection

    Partial Public Class NoValuedAttributePart
        Implements IPostQueryPart

#Region " IPostQueryPart Implementation "

        Public Function CheckCompliancy( _
            ByVal value As FieldInfo _
        ) As Boolean Implements IPostQueryPart.CheckCompliancy

            Return Not New ValuedAttributePart(AttributeType, PropertyValuesToMatch) _
                .CheckCompliancy(value)

        End Function

        Public Function CheckCompliancy( _
            ByVal value As MethodInfo _
        ) As Boolean Implements IPostQueryPart.CheckCompliancy

            Return Not New ValuedAttributePart(AttributeType, PropertyValuesToMatch) _
                .CheckCompliancy(value)

        End Function

        Public Function CheckCompliancy( _
            ByVal value As ConstructorInfo _
        ) As Boolean Implements IPostQueryPart.CheckCompliancy

            Return Not New ValuedAttributePart(AttributeType, PropertyValuesToMatch) _
                .CheckCompliancy(value)

        End Function

        Public Function CheckCompliancy( _
            ByVal value As PropertyInfo _
        ) As Boolean Implements IPostQueryPart.CheckCompliancy

            Return Not New ValuedAttributePart(AttributeType, PropertyValuesToMatch) _
                .CheckCompliancy(value)

        End Function

        Public Function CheckCompliancy( _
            ByVal value As System.Attribute _
        ) As Boolean Implements IPostQueryPart.CheckCompliancy

            Return Not New ValuedAttributePart(AttributeType, PropertyValuesToMatch) _
                .CheckCompliancy(value)

        End Function

        Public Function CheckCompliancy( _
            ByVal value As System.Exception _
        ) As Boolean Implements IPostQueryPart.CheckCompliancy

            Return Not New ValuedAttributePart(AttributeType, PropertyValuesToMatch) _
                .CheckCompliancy(value)

        End Function

        Public Function CheckCompliancy( _
            ByVal value As System.Type _
        ) As Boolean Implements IPostQueryPart.CheckCompliancy

            Return Not New ValuedAttributePart(AttributeType, PropertyValuesToMatch) _
                .CheckCompliancy(value)

        End Function

#End Region

    End Class

End Namespace