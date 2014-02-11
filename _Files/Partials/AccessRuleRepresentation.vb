Imports System.IO
Imports System.Security
Imports System.Security.AccessControl
Imports System.Security.Principal
Imports System.Xml
Imports System.Xml.Serialization

Namespace Files

    Partial Public Class AccessRuleRepresentation

#Region " Public Shared Variables "

        ''' <summary>
        ''' Public Shared Reference to the Formatted Name of the Propogation Property.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PROPERTY_COMPLEXPROPAGATION As String = "ComplexPropagation"

#End Region

#Region " Public WriteOnly Properties "

        <XmlElement(ElementName:=PROPERTY_COMPLEXPROPAGATION)> _
        Public WriteOnly Property ComplexPropagation() As ComplexPropagationFlags
            Set(ByVal value As ComplexPropagationFlags)

                If value = ComplexPropagationFlags.TargetDirectory Then

                    Me.Inheritance = InheritanceFlags.None
                    Me.Propagation = PropagationFlags.None

                ElseIf value = (ComplexPropagationFlags.TargetDirectory Or ComplexPropagationFlags.ChildFile Or ComplexPropagationFlags.DescendantFile) Then

                    Me.Inheritance = InheritanceFlags.ObjectInherit
                    Me.Propagation = PropagationFlags.None

                ElseIf value = (ComplexPropagationFlags.TargetDirectory Or ComplexPropagationFlags.ChildFile) Then

                    Me.Inheritance = InheritanceFlags.ObjectInherit
                    Me.Propagation = PropagationFlags.NoPropagateInherit

                ElseIf value = (ComplexPropagationFlags.ChildFile Or ComplexPropagationFlags.DescendantFile) Then

                    Me.Inheritance = InheritanceFlags.ObjectInherit
                    Me.Propagation = PropagationFlags.InheritOnly

                ElseIf value = (ComplexPropagationFlags.ChildFile) Then

                    Me.Inheritance = InheritanceFlags.ObjectInherit
                    Me.Propagation = PropagationFlags.InheritOnly Or PropagationFlags.NoPropagateInherit

                ElseIf value = (ComplexPropagationFlags.TargetDirectory Or ComplexPropagationFlags.ChildDirectory Or ComplexPropagationFlags.DescendantDirectory) Then

                    Me.Inheritance = InheritanceFlags.ContainerInherit
                    Me.Propagation = PropagationFlags.None

                ElseIf value = (ComplexPropagationFlags.TargetDirectory Or ComplexPropagationFlags.ChildDirectory) Then

                    Me.Inheritance = InheritanceFlags.ContainerInherit
                    Me.Propagation = PropagationFlags.NoPropagateInherit

                ElseIf value = (ComplexPropagationFlags.ChildDirectory Or ComplexPropagationFlags.DescendantDirectory) Then

                    Me.Inheritance = InheritanceFlags.ContainerInherit
                    Me.Propagation = PropagationFlags.InheritOnly

                ElseIf value = (ComplexPropagationFlags.ChildDirectory) Then

                    Me.Inheritance = InheritanceFlags.ContainerInherit
                    Me.Propagation = PropagationFlags.InheritOnly Or PropagationFlags.NoPropagateInherit

                ElseIf value = ComplexPropagationFlags.All Then

                    Me.Inheritance = InheritanceFlags.ContainerInherit Or InheritanceFlags.ObjectInherit
                    Me.Propagation = PropagationFlags.None

                ElseIf value = (ComplexPropagationFlags.TargetDirectory Or ComplexPropagationFlags.ChildDirectory Or ComplexPropagationFlags.ChildFile) Then

                    Me.Inheritance = InheritanceFlags.ContainerInherit Or InheritanceFlags.ObjectInherit
                    Me.Propagation = PropagationFlags.NoPropagateInherit

                ElseIf value = (ComplexPropagationFlags.ChildDirectory Or ComplexPropagationFlags.ChildFile Or ComplexPropagationFlags.DescendantDirectory Or ComplexPropagationFlags.DescendantFile) Then

                    Me.Inheritance = InheritanceFlags.ContainerInherit Or InheritanceFlags.ObjectInherit
                    Me.Propagation = PropagationFlags.InheritOnly

                ElseIf value = (ComplexPropagationFlags.ChildDirectory Or ComplexPropagationFlags.ChildFile) Then

                    Me.Inheritance = InheritanceFlags.ContainerInherit Or InheritanceFlags.ObjectInherit
                    Me.Propagation = PropagationFlags.NoPropagateInherit Or PropagationFlags.InheritOnly

                End If

            End Set
        End Property

#End Region

#Region " Public Methods "

        Public Function Create() As FileSystemAccessRule

            Dim ret_Rule As FileSystemAccessRule = _
                New FileSystemAccessRule( _
                    User, _
                    Right, _
                    Inheritance, _
                    Propagation, _
                    Type _
                )

            Return ret_Rule

        End Function

#End Region

    End Class

End Namespace