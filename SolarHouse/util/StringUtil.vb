Public Class StringUtil
    Public Shared Function isEmpty(str As Object)
        Return IsNothing(str) OrElse (IsDBNull(str)) OrElse Len(Trim(str)) = 0
    End Function

    Public Shared Function isNullThenEmptyStr(v As Object)
        Return If(isEmpty(v), "", v.ToString())
    End Function

End Class
