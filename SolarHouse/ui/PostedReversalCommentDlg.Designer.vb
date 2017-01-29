<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PostedReversalCommentDlg
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.commentsTxtBx = New System.Windows.Forms.TextBox()
        Me.okBtn = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'commentsTxtBx
        '
        Me.commentsTxtBx.Location = New System.Drawing.Point(12, 12)
        Me.commentsTxtBx.Multiline = True
        Me.commentsTxtBx.Name = "commentsTxtBx"
        Me.commentsTxtBx.Size = New System.Drawing.Size(393, 128)
        Me.commentsTxtBx.TabIndex = 0
        '
        'okBtn
        '
        Me.okBtn.Location = New System.Drawing.Point(331, 150)
        Me.okBtn.Name = "okBtn"
        Me.okBtn.Size = New System.Drawing.Size(75, 23)
        Me.okBtn.TabIndex = 2
        Me.okBtn.Text = "OK"
        Me.okBtn.UseVisualStyleBackColor = True
        '
        'PostedReversalCommentDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(500, 199)
        Me.Controls.Add(Me.okBtn)
        Me.Controls.Add(Me.commentsTxtBx)
        Me.Name = "PostedReversalCommentDlg"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Enter why are you reversing"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents commentsTxtBx As TextBox
    Friend WithEvents okBtn As Button
End Class
