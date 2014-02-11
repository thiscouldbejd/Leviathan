Imports Leviathan.Comparison.Comparer

Namespace Inspection

    Partial Public Class ValuedAttributePart
        Implements IPostQueryPart

#Region " Private Methods "

        Private Overloads Function CheckCompliancy( _
            ByVal attributes As Attribute() _
        ) As Boolean

            For i As Integer = 0 To PropertyValuesToMatch.Length - 1

                Dim prop As PropertyInfo = AttributeType.GetProperty(PropertyValuesToMatch(i).Key)

                Dim ret As Boolean = False

                If Not prop Is Nothing Then

                    For j As Integer = 0 To attributes.Length - 1

                        If AreEqual(PropertyValuesToMatch(i).Value, _
                            prop.GetValue(attributes(j), Nothing)) Then _
                                ret = True

                    Next

                End If

                If Not ret Then Return False

            Next

            Return True

        End Function

#End Region

#Region " IPostQueryPart Implementation "

        Public Overloads Function CheckCompliancy( _
            ByVal value As FieldInfo _
        ) As Boolean Implements IPostQueryPart.CheckCompliancy

            If New AttributePart(AttributeType).CheckCompliancy(value) Then _
                Return CheckCompliancy(value.GetCustomAttributes(AttributeType, True))

            Return False

        End Function

        Public Overloads Function CheckCompliancy( _
            ByVal value As MethodInfo _
        ) As Boolean Implements IPostQueryPart.CheckCompliancy

            If New AttributePart(AttributeType).CheckCompliancy(value) Then _
                Return CheckCompliancy(value.GetCustomAttributes(AttributeType, True))

            Return False

        End Function

        Public Overloads Function CheckCompliancy( _
            ByVal value As ConstructorInfo _
        ) As Boolean Implements IPostQueryPart.CheckCompliancy

            If New AttributePart(AttributeType).CheckCompliancy(value) Then _
                Return CheckCompliancy(value.GetCustomAttributes(AttributeType, True))

            Return False

        End Function

        Public Overloads Function CheckCompliancy( _
            ByVal value As PropertyInfo _
        ) As Boolean Implements IPostQueryPart.CheckCompliancy

            If New AttributePart(AttributeType).CheckCompliancy(value) Then _
                Return CheckCompliancy(value.GetCustomAttributes(AttributeType, True))

            Return False

        End Function

        Public Overloads Function CheckCompliancy( _
            ByVal value As System.Attribute _
        ) As Boolean Implements IPostQueryPart.CheckCompliancy

            Return CheckCompliancy(value.GetType)

        End Function

        Public Overloads Function CheckCompliancy( _
            ByVal value As System.Exception _
        ) As Boolean Implements IPostQueryPart.CheckCompliancy

            Return CheckCompliancy(value.GetType)

        End Function

        Public Overloads Function CheckCompliancy( _
            ByVal value As System.Type _
        ) As Boolean Implements IPostQueryPart.CheckCompliancy

            If New AttributePart(AttributeType).CheckCompliancy(value) Then _
                Return CheckCompliancy(value.GetCustomAttributes(AttributeType, True))

            Return False

        End Function

#End Region

    End Class

End Namespace