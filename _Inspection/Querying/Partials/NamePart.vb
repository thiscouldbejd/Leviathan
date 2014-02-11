Namespace Inspection

    Partial Public Class NamePart
        Implements IPostQueryPart

#Region " IPostQueryPart Implementation "

        Public Overridable Function CheckCompliancy( _
            ByVal value As FieldInfo _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            For i As Integer = 0 To Names.Length - 1
                If (String.Compare(value.Name, Names(i), True) = 0) Then Return True
            Next

            Return False

        End Function

        Public Overridable Function CheckCompliancy( _
            ByVal value As MethodInfo _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            For i As Integer = 0 To Names.Length - 1
                If (String.Compare(value.Name, Names(i), True) = 0) Then Return True
            Next

            Return False

        End Function

        Public Function CheckCompliancy( _
            ByVal value As ConstructorInfo _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            For i As Integer = 0 To Names.Length - 1
                If (String.Compare(value.Name, Names(i), True) = 0) Then Return True
            Next

            Return False

        End Function

        Public Overridable Function CheckCompliancy( _
            ByVal value As PropertyInfo _
        ) As Boolean _
        Implements IPostQueryPart.CheckCompliancy

            For i As Integer = 0 To Names.Length - 1
                If (String.Compare(value.Name, Names(i), True) = 0) Then Return True
            Next

            Return False

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

            For i As Integer = 0 To Names.Length - 1
                If (String.Compare(value.Name, Names(i), True) = 0) Then Return True
            Next

            Return False

        End Function

#End Region

    End Class

End Namespace