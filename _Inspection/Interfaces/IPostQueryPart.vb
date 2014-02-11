Namespace Inspection

    ''' <summary>
    ''' Implementation of a Query Part that can be Handled Once Values have been Retrieved.
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IPostQueryPart

        ''' <summary>
        ''' Checks the Compliancy of a Type.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function CheckCompliancy( _
            ByVal value As Type _
        ) As Boolean

        ''' <summary>
        ''' Checks the Compliancy of a Field.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function CheckCompliancy( _
            ByVal value As FieldInfo _
        ) As Boolean

        ''' <summary>
        ''' Checks the Compliancy of a Property.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function CheckCompliancy( _
            ByVal value As PropertyInfo _
        ) As Boolean

        ''' <summary>
        ''' Checks the Compliancy of a Method.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function CheckCompliancy( _
            ByVal value As MethodInfo _
        ) As Boolean

        ''' <summary>
        ''' Checks the Compliancy of a Method.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function CheckCompliancy( _
            ByVal value As ConstructorInfo _
        ) As Boolean

        ''' <summary>
        ''' Checks the Compliancy of an Attribute.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function CheckCompliancy( _
            ByVal value As Attribute _
        ) As Boolean

        ''' <summary>
        ''' Checks the Compliancy of an Exception.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function CheckCompliancy( _
            ByVal value As Exception _
        ) As Boolean

    End Interface

End Namespace

