<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SubmittedProdsAcbAndQtyDlg
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
        Me.submittedPrdsAcbAndQtyListView = New System.Windows.Forms.ListView()
        Me.prod_code = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.prod_name = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.qty = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.uom = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.acb = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'submittedPrdsAcbAndQtyListView
        '
        Me.submittedPrdsAcbAndQtyListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.prod_code, Me.prod_name, Me.qty, Me.uom, Me.acb})
        Me.submittedPrdsAcbAndQtyListView.Location = New System.Drawing.Point(12, 53)
        Me.submittedPrdsAcbAndQtyListView.Name = "submittedPrdsAcbAndQtyListView"
        Me.submittedPrdsAcbAndQtyListView.Size = New System.Drawing.Size(819, 315)
        Me.submittedPrdsAcbAndQtyListView.TabIndex = 0
        Me.submittedPrdsAcbAndQtyListView.UseCompatibleStateImageBehavior = False
        '
        'prod_code
        '
        Me.prod_code.Text = "Code"
        Me.prod_code.Width = 100
        '
        'prod_name
        '
        Me.prod_name.Text = "Name"
        Me.prod_name.Width = 200
        '
        'qty
        '
        Me.qty.Text = "Remaining Qty"
        Me.qty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.qty.Width = 100
        '
        'uom
        '
        Me.uom.Text = "uom"
        Me.uom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.uom.Width = 100
        '
        'acb
        '
        Me.acb.Text = "acb"
        Me.acb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.acb.Width = 100
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(13, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(140, 20)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Report Commited"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 33)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(418, 17)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "The following Product Quanties are Left.  Verify they match Stock"
        '
        'SubmittedProdsAcbAndQtyDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(892, 434)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.submittedPrdsAcbAndQtyListView)
        Me.Name = "SubmittedProdsAcbAndQtyDlg"
        Me.ShowIcon = False
        Me.Text = "Submitted Products Qty And Cost"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents submittedPrdsAcbAndQtyListView As ListView
    Friend WithEvents Label1 As Label
    Friend WithEvents prod_code As ColumnHeader
    Friend WithEvents prod_name As ColumnHeader
    Friend WithEvents qty As ColumnHeader
    Friend WithEvents uom As ColumnHeader
    Friend WithEvents acb As ColumnHeader
    Friend WithEvents Label2 As Label
End Class
