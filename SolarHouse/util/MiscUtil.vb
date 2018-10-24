Public Class MiscUtil



    Public Shared Function appendPathComponent(path As String, pathComponent As String) As String
        Dim absPath As String = path.Trim()

        If (absPath.LastIndexOf("\") <> path.Length - 1) Then
            absPath += "\"
        End If
        absPath += pathComponent.Trim()
        appendPathComponent = absPath
    End Function

    Public Shared Function addAll(c1 As Collection, c2 As Collection) As Collection
        Dim c As New Collection
        If (Not IsNothing(c1)) Then
            For r As Integer = 1 To c1.Count
                c.Add(c1(r))
            Next
        End If
        If (Not IsNothing(c2)) Then

            For r As Integer = 1 To c2.Count
                c.Add(c2(r))
            Next
        End If

        Return c
    End Function

End Class
