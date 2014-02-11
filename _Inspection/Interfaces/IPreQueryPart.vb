Namespace Inspection

    ''' <summary>
    ''' Implementation of a Query Part that can be Handled Before Values have been Retrieved.
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IPreQueryPart

        ''' <summary>
        ''' Retrieves the Binding Flags needed for a Reflection-Based Query.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property BindingFlags() As BindingFlags

    End Interface

End Namespace