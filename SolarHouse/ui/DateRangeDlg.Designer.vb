<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DateRangeDlg
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
        Me.fromDateTxtBx = New System.Windows.Forms.MaskedTextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.toDateTxtBx = New System.Windows.Forms.MaskedTextBox()
        Me.okBtn = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'fromDateTxtBx
        '
        Me.fromDateTxtBx.Location = New System.Drawing.Point(82, 17)
        Me.fromDateTxtBx.Mask = "00/00/0000"
        Me.fromDateTxtBx.Name = "fromDateTxtBx"
        Me.fromDateTxtBx.Size = New System.Drawing.Size(100, 22)
        Me.fromDateTxtBx.TabIndex = 0
        Me.fromDateTxtBx.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.fromDateTxtBx.ValidatingType = GetType(Date)
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(32, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 17)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "From:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(197, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 17)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "To:"
        '
        'toDateTxtBx
        '
        Me.toDateTxtBx.Location = New System.Drawing.Point(232, 17)
        Me.toDateTxtBx.Mask = "00/00/0000"
        Me.toDateTxtBx.Name = "toDateTxtBx"
        Me.toDateTxtBx.Size = New System.Drawing.Size(100, 22)
        Me.toDateTxtBx.TabIndex = 1
        Me.toDateTxtBx.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.toDateTxtBx.ValidatingType = GetType(Date)
        '
        'okBtn
        '
        Me.okBtn.Location = New System.Drawing.Point(251, 62)
        Me.okBtn.Name = "okBtn"
        Me.okBtn.Size = New System.Drawing.Size(75, 23)
        Me.okBtn.TabIndex = 2
        Me.okBtn.Text = "OK"
        Me.okBtn.UseVisualStyleBackColor = True
        '
        'DateRangeDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(368, 107)
        Me.Controls.Add(Me.okBtn)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.toDateTxtBx)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.fromDateTxtBx)
        Me.Name = "DateRangeDlg"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Enter Date Range"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents fromDateTxtBx As MaskedTextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents toDateTxtBx As MaskedTextBox
    Friend WithEvents okBtn As Button
End Class
