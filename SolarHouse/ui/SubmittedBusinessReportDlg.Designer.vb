<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SubmittedBusinessReportDlg
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
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lowStockAlertGrdVw = New SolarHouse.LowStockAlertDataGrid()
        CType(Me.lowStockAlertGrdVw, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'submittedPrdsAcbAndQtyListView
        '
        Me.submittedPrdsAcbAndQtyListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.prod_code, Me.prod_name, Me.qty, Me.uom, Me.acb})
        Me.submittedPrdsAcbAndQtyListView.Location = New System.Drawing.Point(9, 43)
        Me.submittedPrdsAcbAndQtyListView.Margin = New System.Windows.Forms.Padding(2)
        Me.submittedPrdsAcbAndQtyListView.Name = "submittedPrdsAcbAndQtyListView"
        Me.submittedPrdsAcbAndQtyListView.Size = New System.Drawing.Size(615, 257)
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
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label2.Location = New System.Drawing.Point(10, 27)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(234, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Idadi ya Mzigo ime baki kama ifuatiyo:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(9, 323)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(129, 16)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Agiza mzigo ifuatiyo:"
        '
        'lowStockAlertGrdVw
        '
        Me.lowStockAlertGrdVw.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.lowStockAlertGrdVw.Location = New System.Drawing.Point(9, 342)
        Me.lowStockAlertGrdVw.Name = "lowStockAlertGrdVw"
        Me.lowStockAlertGrdVw.Size = New System.Drawing.Size(1008, 242)
        Me.lowStockAlertGrdVw.TabIndex = 5
        '
        'SubmittedProdsAcbAndQtyDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1056, 596)
        Me.Controls.Add(Me.lowStockAlertGrdVw)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.submittedPrdsAcbAndQtyListView)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "SubmittedProdsAcbAndQtyDlg"
        Me.ShowIcon = False
        Me.Text = "Submitted Products Qty And Cost"
        CType(Me.lowStockAlertGrdVw, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents submittedPrdsAcbAndQtyListView As ListView
    Friend WithEvents prod_code As ColumnHeader
    Friend WithEvents prod_name As ColumnHeader
    Friend WithEvents qty As ColumnHeader
    Friend WithEvents uom As ColumnHeader
    Friend WithEvents acb As ColumnHeader
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lowStockAlertGrdVw As SolarHouse.LowStockAlertDataGrid
End Class
