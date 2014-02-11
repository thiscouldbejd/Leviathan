Imports System.IO
Imports System.Security.AccessControl
Imports System.Security.Principal
Imports System.Xml
Imports System.Xml.Serialization

Namespace Files

    Partial Public Class DirectoryRepresentation

#Region " Public Properties "

        Public ReadOnly Property HasChildren() As Boolean
            Get
                Return Not Children Is Nothing AndAlso Children.Length > 0
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Function ModifyACL( _
            ByVal directorySecurity As DirectorySecurity, _
            ByVal directoryModification As AccessControlModification _
        ) As Boolean

            Dim l_SecuritySuccess As Boolean = True

            If Not Rules Is Nothing AndAlso Rules.Length > 0 Then

                For i As Integer = 0 To Rules.Length - 1

                    If Not directorySecurity.ModifyAccessRule( _
                        directoryModification, _
                        Rules(i).Create, _
                        New Boolean _
                    ) Then l_SecuritySuccess = False

                Next

            End If

            Return l_SecuritySuccess

        End Function

        Private Function CommitACL( _
            ByVal directoryBase As DirectoryInfo, _
            ByVal directorySecurity As DirectorySecurity, _
            ByVal directoryModification As AccessControlModification _
        ) As Boolean

            Dim l_SecuritySuccess As Boolean = False

            If ModifyACL(directorySecurity, directoryModification) Then

                Try

                    directoryBase.SetAccessControl(directorySecurity)

                    l_SecuritySuccess = True

                Catch ex As Exception

                    l_SecuritySuccess = False

                End Try

            End If

            Return l_SecuritySuccess

        End Function

        Private Function IterateChildren( _
            ByVal directoryBase As DirectoryInfo, _
            ByVal modification As AccessControlModification, _
            Optional ByVal useModification As Boolean = True _
        ) As Boolean

            If Not Children Is Nothing AndAlso Children.Length > 0 Then

                Dim childDirectories As DirectoryInfo() = _
                    directoryBase.GetDirectories()

                If Not childDirectories Is Nothing _
                    AndAlso childDirectories.Length > 0 Then

                    For i As Integer = 0 To Children.Length - 1

                        For j As Integer = 0 To childDirectories.Length - 1

                            If String.Compare(Children(i).Name, _
                                childDirectories(j).Name, True) = 0 Then

                                If useModification Then

                                    If Not Children(i).ModifyACL(childDirectories(j), modification) Then _
                                        Return False

                                Else

                                    If Not Children(i).ReplaceACL(childDirectories(j)) Then _
                                        Return False

                                End If

                            End If

                        Next

                    Next

                End If

            End If

            Return True

        End Function

#End Region

#Region " Public Methods "

        Public Function ReplaceACL( _
            ByVal directoryBase As DirectoryInfo _
        ) As Boolean

            If IterateChildren(directoryBase, AccessControlModification.Add, False) Then

                Dim l_Security As DirectorySecurity = _
                directoryBase.GetAccessControl()

                Dim currentRules As AuthorizationRuleCollection = _
                    l_Security.GetAccessRules(True, False, GetType(NTAccount))

                For i As Integer = 0 To currentRules.Count - 1

                    l_Security.RemoveAccessRule(currentRules(i))

                Next

                Return CommitACL(directoryBase, _
                    l_Security, AccessControlModification.Add)

            Else

                Return False

            End If

        End Function

        Public Function ModifyACL( _
            ByVal directoryBase As DirectoryInfo, _
            ByVal modification As AccessControlModification _
        ) As Boolean

            If IterateChildren(directoryBase, modification, True) Then

                Return CommitACL(directoryBase, _
                    directoryBase.GetAccessControl(), modification)

            Else

                Return False

            End If

        End Function

#End Region

    End Class

End Namespace

