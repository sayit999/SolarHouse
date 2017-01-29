Public Class MiscUtil



    Public Shared Function appendPathComponent(path As String, pathComponent As String) As String
        Dim absPath As String = path.Trim()

        If (absPath.LastIndexOf("\") <> path.Length - 1) Then
            absPath += "\"
        End If
        absPath += pathComponent.Trim()
        appendPathComponent = absPath
    End Function
End Class
