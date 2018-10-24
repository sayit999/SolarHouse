Public Class TextBoxErrorPictureBox
    Inherits PictureBox
    Private errorText As String = ""
    Private owner As BusinessReportDlg


    Public Sub New()
        Hide()

    End Sub

    Public Sub addErrorText(errorText As String)
        If (Me.errorText.Length > 0) Then
            Me.errorText = Me.errorText + ","
        End If
        Me.errorText = Me.errorText + errorText
        setErrorText(Me.errorText)
    End Sub

    Public Sub setErrorText(errorText As String)
        Me.errorText = errorText
        If (errorText.Length > 0) Then
            Show()
        Else
            Hide()

        End If
    End Sub

    Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
        If Visible Then
            MessageBox.Show(owner, errorText)

        End If

    End Sub


    Public Sub init(parent As BusinessReportDlg)
        owner = parent
    End Sub

    Public Function isError() As Boolean
        Return errorText.Length > 0
    End Function
End Class
