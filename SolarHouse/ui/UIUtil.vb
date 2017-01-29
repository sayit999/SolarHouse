Imports System.Globalization
Public Class UIUtil

    Public Shared NULL_DATE As Date = New Date(1, 1, 1)
    Public Shared NULL_DATE_STR As String = "01/01/01"

    Public Shared Function splitVals(val As String, Optional delim As String = ",") As String()
        Dim vals As String() = Split(val, ",")
        For i As Integer = 0 To vals.Count - 1
            vals(i) = vals(i).Trim
        Next
        Return vals
    End Function

    Public Shared Function grdVwColNameToIndex(name As String, grd As DataGridView)
        Dim c As Integer = vbNull
        For c = 0 To grd.ColumnCount - 1
            If name.Equals(grd.Columns(c).Name) Then
                Exit For
            End If
        Next
        If c = vbNull Then
            Throw New Exception("No column name = " + name)
        End If
        grdVwColNameToIndex = c
    End Function

    Public Shared Function subsIfEmpty(value As Object, valToSub As Object) As Object
        If StringUtil.isEmpty(value) Then
            subsIfEmpty = valToSub
        Else
            subsIfEmpty = value
        End If
    End Function


    Public Shared Function zeroIfEmpty(value As Object) As Integer
        Return If(Not IsNumeric(value), 0, removeNumberFormating(value))
    End Function

    Public Shared Function zeroIfEmptyStr(str As String) As String
        zeroIfEmptyStr = zeroIfEmpty(str).ToString
    End Function

    Public Shared Function isBetween(a As Integer, b As Integer, val As Integer)
        isBetween = (a <= val) And (b >= val)
    End Function


    Public Shared Function removeNumberFormating(numFmtVal As String) As Double
        If (StringUtil.isEmpty(numFmtVal) OrElse Not IsNumeric(numFmtVal)) Then
            numFmtVal = "0"
        End If
        removeNumberFormating = Double.Parse(Replace(numFmtVal, ",", ""))
    End Function

    Public Shared Function isValidDate(val As Object) As Boolean
        Try
            parseDate(val)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Shared Function genDateCompoFormat(cmp As String, fmtChar As Char) As String
        Dim format As String = ""
        For i As Integer = 0 To cmp.Length - 1
            format += fmtChar
        Next
        Return format
    End Function

    Public Shared Function parseDate(val As Object) As Date
        If (StringUtil.isEmpty(val)) Then
            parseDate = Nothing
        Else
            Dim strDate As String = val
            Dim dateComponents As String() = strDate.Split("/")
            If dateComponents.Length = 3 Then
                Dim dateFormat As String
                dateFormat = genDateCompoFormat(dateComponents(0), "d")
                dateFormat += "/" + genDateCompoFormat(dateComponents(1), "M")
                dateFormat += "/" + genDateCompoFormat(dateComponents(2), "y")
                Try
                    Return Date.ParseExact(strDate, dateFormat, CultureInfo.InvariantCulture)
                Catch ex As FormatException
                    Return Nothing
                End Try
            Else
                Return Nothing
            End If
        End If
    End Function

    Public Shared Function parseDouble(str As String) As Double
        If (StringUtil.isEmpty(str) OrElse Not IsNumeric(str)) Then
            parseDouble = "0"
        Else
            parseDouble = Double.Parse(str)
        End If
    End Function

    Public Shared Function toAmtString(val As Double) As String
        Return FormatNumber(zeroIfEmpty(val), 0, TriState.True, TriState.False, True)
    End Function

    Public Shared Function toDateString(val As Date) As String
        If val = NULL_DATE Then
            Return ""
        Else
            Return Format(val, "dd/MM/yyyy")
        End If

    End Function

    Public Shared Function isNothingDate(dte As Date) As Boolean
        Return dte = NULL_DATE
    End Function

    Public Shared Function parseDate(obj As Object, dteToSubs As Date) As Date
        If StringUtil.isEmpty(obj) OrElse obj = NULL_DATE_STR Then
            Return dteToSubs
        ElseIf StringUtil.isEmpty(Replace(obj, "\", "").Trim) Then
            Return dteToSubs
        ElseIf Not isValidDate(obj) Then
            Return dteToSubs
        Else
            Return UIUtil.parseDate(obj)
        End If
    End Function

    Public Shared Function toBoolean(val As Object) As Boolean
        If (StringUtil.isEmpty(val)) Then
            Return False
        Else
            Return If(TypeOf val Is String, val = "1", val = 1)
        End If
    End Function

    Public Shared Function toBinaryBooleanString(val As Boolean) As String
        Return If(val, "1", "0")
    End Function

    Public Shared Function isEmpty(o As Object)
        Return IsNothing(o) OrElse (IsDBNull(o)) OrElse Len(Trim(o)) = 0
    End Function
End Class
