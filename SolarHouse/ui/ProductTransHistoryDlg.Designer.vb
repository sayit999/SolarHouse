<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SubmittedProductAcbAndQtyDlg
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
        Me.productTransHistoryListView = New System.Windows.Forms.ListView()
        Me.tran_date = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tran_type = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.qty = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.qty_uom = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.unit_price = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.purchased_from = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.qty_avail = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.acb = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.comments = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.productLbl = New System.Windows.Forms.Label()
        Me.code = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SuspendLayout()
        '
        'productTransHistoryListView
        '
        Me.productTransHistoryListView.BackColor = System.Drawing.Color.White
        Me.productTransHistoryListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.tran_date, Me.tran_type, Me.qty, Me.qty_uom, Me.unit_price, Me.purchased_from, Me.qty_avail, Me.acb, Me.comments, Me.code})
        Me.productTransHistoryListView.Location = New System.Drawing.Point(12, 56)
        Me.productTransHistoryListView.Name = "productTransHistoryListView"
        Me.productTransHistoryListView.Size = New System.Drawing.Size(1342, 531)
        Me.productTransHistoryListView.TabIndex = 0
        Me.productTransHistoryListView.UseCompatibleStateImageBehavior = False
        Me.productTransHistoryListView.View = System.Windows.Forms.View.List
        '
        'tran_date
        '
        Me.tran_date.Text = "Date"
        Me.tran_date.Width = 80
        '
        'tran_type
        '
        Me.tran_type.Text = "Transaction"
        Me.tran_type.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.tran_type.Width = 100
        '
        'qty
        '
        Me.qty.Text = "Qty"
        Me.qty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.qty.Width = 70
        '
        'qty_uom
        '
        Me.qty_uom.Text = "uom"
        Me.qty_uom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.qty_uom.Width = 70
        '
        'unit_price
        '
        Me.unit_price.Text = "@"
        Me.unit_price.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.unit_price.Width = 75
        '
        'purchased_from
        '
        Me.purchased_from.Text = "Supplier"
        Me.purchased_from.Width = 180
        '
        'qty_avail
        '
        Me.qty_avail.Text = "Qty Available"
        Me.qty_avail.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.qty_avail.Width = 90
        '
        'acb
        '
        Me.acb.Text = "ACB"
        Me.acb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.acb.Width = 100
        '
        'comments
        '
        Me.comments.Text = "Comments"
        Me.comments.Width = 250
        '
        'productLbl
        '
        Me.productLbl.AutoSize = True
        Me.productLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.productLbl.Location = New System.Drawing.Point(12, 18)
        Me.productLbl.Name = "productLbl"
        Me.productLbl.Size = New System.Drawing.Size(0, 32)
        Me.productLbl.TabIndex = 1
        '
        'code
        '
        Me.code.Text = "code"
        '
        'SubmittedProductAcbAndQtyDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1379, 599)
        Me.Controls.Add(Me.productLbl)
        Me.Controls.Add(Me.productTransHistoryListView)
        Me.Name = "SubmittedProductAcbAndQtyDlg"
        Me.ShowIcon = False
        Me.Text = "Product Transaction History"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents productTransHistoryListView As ListView
    Friend WithEvents tran_date As ColumnHeader
    Friend WithEvents tran_type As ColumnHeader
    Friend WithEvents qty As ColumnHeader
    Friend WithEvents qty_uom As ColumnHeader
    Friend WithEvents unit_price As ColumnHeader
    Friend WithEvents qty_avail As ColumnHeader
    Friend WithEvents acb As ColumnHeader
    Friend WithEvents comments As ColumnHeader
    Friend WithEvents purchased_from As ColumnHeader
    Friend WithEvents productLbl As Label
    Friend WithEvents code As ColumnHeader
End Class
