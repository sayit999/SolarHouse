Public Class PostedReversalCommentDlg
    Public Sub New(comment As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        commentsTxtBx.Text = comment
    End Sub

    Public Function getComment() As String
        Return commentsTxtBx.Text
    End Function

    Private Sub okBtn_Click(sender As Object, e As EventArgs) Handles okBtn.Click
        If StringUtil.isEmpty(commentsTxtBx.Text) Then
            MessageBox.Show("Please enter the reason why you reversing")
        Else
            DialogResult = DialogResult.OK
        End If

    End Sub
End Class