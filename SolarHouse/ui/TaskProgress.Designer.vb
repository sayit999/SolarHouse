<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TaskProgress
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
        Me.progressBar = New System.Windows.Forms.ProgressBar()
        Me.progressMessageTxtBox = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'progressBar
        '
        Me.progressBar.Location = New System.Drawing.Point(12, 83)
        Me.progressBar.Name = "progressBar"
        Me.progressBar.Size = New System.Drawing.Size(382, 21)
        Me.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.progressBar.TabIndex = 0
        Me.progressBar.Value = 90
        '
        'progressMessageTxtBox
        '
        Me.progressMessageTxtBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.progressMessageTxtBox.Location = New System.Drawing.Point(13, 13)
        Me.progressMessageTxtBox.Multiline = True
        Me.progressMessageTxtBox.Name = "progressMessageTxtBox"
        Me.progressMessageTxtBox.ReadOnly = True
        Me.progressMessageTxtBox.Size = New System.Drawing.Size(381, 64)
        Me.progressMessageTxtBox.TabIndex = 1
        '
        'TaskProgress
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(406, 116)
        Me.Controls.Add(Me.progressMessageTxtBox)
        Me.Controls.Add(Me.progressBar)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TaskProgress"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "TaskProgress"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents progressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents progressMessageTxtBox As System.Windows.Forms.TextBox
End Class
