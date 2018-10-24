<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LowStockAlertDataGrid
    Inherits System.Windows.Forms.DataGridView

    'UserControl overrides dispose to clean up the component list.
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.prod_code = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.prod_name = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.prod_min_qty = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.prod_qty_avail = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.prod_qty_uom = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.prod_acb_cost = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.is_reorder = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'prod_code
        '
        Me.prod_code.DataPropertyName = "product_code"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.prod_code.DefaultCellStyle = DataGridViewCellStyle1
        Me.prod_code.HeaderText = "Code"
        Me.prod_code.Name = "prod_code"
        Me.prod_code.ReadOnly = True
        '
        'prod_name
        '
        Me.prod_name.DataPropertyName = "product_name"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.prod_name.DefaultCellStyle = DataGridViewCellStyle2
        Me.prod_name.HeaderText = "Jina"
        Me.prod_name.Name = "prod_name"
        Me.prod_name.ReadOnly = True
        Me.prod_name.Width = 300
        '
        'prod_min_qty
        '
        Me.prod_min_qty.DataPropertyName = "low_stock_alert_qty"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.prod_min_qty.DefaultCellStyle = DataGridViewCellStyle3
        Me.prod_min_qty.HeaderText = "Idadi Tuwenazo"
        Me.prod_min_qty.Name = "prod_min_qty"
        Me.prod_min_qty.ReadOnly = True
        '
        'prod_qty_avail
        '
        Me.prod_qty_avail.DataPropertyName = "qty_available"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.prod_qty_avail.DefaultCellStyle = DataGridViewCellStyle4
        Me.prod_qty_avail.HeaderText = "Idadi tunayo"
        Me.prod_qty_avail.Name = "prod_qty_avail"
        Me.prod_qty_avail.ReadOnly = True
        '
        'prod_qty_uom
        '
        Me.prod_qty_uom.DataPropertyName = "qty_uom_name"
        Me.prod_qty_uom.HeaderText = "UOM"
        Me.prod_qty_uom.Name = "prod_qty_uom"
        Me.prod_qty_uom.ReadOnly = True
        '
        'prod_acb_cost
        '
        Me.prod_acb_cost.DataPropertyName = "acb_cost"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle5.Format = "N0"
        DataGridViewCellStyle5.NullValue = Nothing
        Me.prod_acb_cost.DefaultCellStyle = DataGridViewCellStyle5
        Me.prod_acb_cost.HeaderText = "Bei"
        Me.prod_acb_cost.Name = "prod_acb_cost"
        Me.prod_acb_cost.ReadOnly = True
        '
        'is_reorder
        '
        Me.is_reorder.DataPropertyName = "is_reorder"
        Me.is_reorder.HeaderText = "Tu Agize Tena?"
        Me.is_reorder.Name = "is_reorder"
        Me.is_reorder.ReadOnly = True
        '
        'LowStockAlertDataGrid
        '
        Me.BackgroundColor = System.Drawing.SystemColors.Info
        Me.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.prod_code, Me.prod_name, Me.prod_min_qty, Me.prod_qty_avail, Me.prod_qty_uom, Me.prod_acb_cost, Me.is_reorder})
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents prod_code As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents prod_name As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents prod_min_qty As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents prod_qty_avail As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents prod_qty_uom As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents prod_acb_cost As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents is_reorder As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
