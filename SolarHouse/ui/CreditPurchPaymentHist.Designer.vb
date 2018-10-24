<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CreditPurchPaymentHistDlg
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
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.productLbl = New System.Windows.Forms.Label()
        Me.creditPurchPaymentHistoryGrdVw = New System.Windows.Forms.DataGridView()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.supplierNameCmbBx = New System.Windows.Forms.ComboBox()
        Me.lookupSupplierPaymentHistBtn = New System.Windows.Forms.Button()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn11 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn12 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn13 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn14 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.tran_type = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.tran_date = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.product_code = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.prod_name = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.qty = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.qty_uom = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.amt = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.balance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.supplier_name = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.comments = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.is_amendment = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.is_reversal = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.updated_on = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.sort_order = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.creditPurchPaymentHistoryGrdVw, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'productLbl
        '
        Me.productLbl.AutoSize = True
        Me.productLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.productLbl.Location = New System.Drawing.Point(9, 15)
        Me.productLbl.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.productLbl.Name = "productLbl"
        Me.productLbl.Size = New System.Drawing.Size(0, 26)
        Me.productLbl.TabIndex = 1
        '
        'creditPurchPaymentHistoryGrdVw
        '
        Me.creditPurchPaymentHistoryGrdVw.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.creditPurchPaymentHistoryGrdVw.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.creditPurchPaymentHistoryGrdVw.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.tran_type, Me.tran_date, Me.product_code, Me.prod_name, Me.qty, Me.qty_uom, Me.amt, Me.balance, Me.supplier_name, Me.comments, Me.is_amendment, Me.is_reversal, Me.updated_on, Me.sort_order})
        Me.creditPurchPaymentHistoryGrdVw.GridColor = System.Drawing.SystemColors.InactiveCaption
        Me.creditPurchPaymentHistoryGrdVw.Location = New System.Drawing.Point(20, 57)
        Me.creditPurchPaymentHistoryGrdVw.Name = "creditPurchPaymentHistoryGrdVw"
        Me.creditPurchPaymentHistoryGrdVw.Size = New System.Drawing.Size(1120, 418)
        Me.creditPurchPaymentHistoryGrdVw.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(17, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 16)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Supplier:"
        '
        'supplierNameCmbBx
        '
        Me.supplierNameCmbBx.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.supplierNameCmbBx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.supplierNameCmbBx.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.supplierNameCmbBx.FormattingEnabled = True
        Me.supplierNameCmbBx.Location = New System.Drawing.Point(82, 14)
        Me.supplierNameCmbBx.Name = "supplierNameCmbBx"
        Me.supplierNameCmbBx.Size = New System.Drawing.Size(258, 26)
        Me.supplierNameCmbBx.TabIndex = 5
        '
        'lookupSupplierPaymentHistBtn
        '
        Me.lookupSupplierPaymentHistBtn.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lookupSupplierPaymentHistBtn.Location = New System.Drawing.Point(355, 14)
        Me.lookupSupplierPaymentHistBtn.Name = "lookupSupplierPaymentHistBtn"
        Me.lookupSupplierPaymentHistBtn.Size = New System.Drawing.Size(75, 23)
        Me.lookupSupplierPaymentHistBtn.TabIndex = 6
        Me.lookupSupplierPaymentHistBtn.Text = "lookup"
        Me.lookupSupplierPaymentHistBtn.UseVisualStyleBackColor = True
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "tran_type"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.DataGridViewTextBoxColumn1.DefaultCellStyle = DataGridViewCellStyle8
        Me.DataGridViewTextBoxColumn1.HeaderText = "Column1"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Visible = False
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "tran_date"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.DataGridViewTextBoxColumn2.DefaultCellStyle = DataGridViewCellStyle9
        Me.DataGridViewTextBoxColumn2.HeaderText = "Date"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 75
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "product_code"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle10
        Me.DataGridViewTextBoxColumn3.HeaderText = "Product Code"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Width = 75
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "product_name"
        Me.DataGridViewTextBoxColumn4.HeaderText = "Product Name"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Width = 200
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "qty"
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle11.Format = "N0"
        DataGridViewCellStyle11.NullValue = Nothing
        Me.DataGridViewTextBoxColumn5.DefaultCellStyle = DataGridViewCellStyle11
        Me.DataGridViewTextBoxColumn5.HeaderText = "Qty"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.Width = 50
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "qty_uom"
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.DataGridViewTextBoxColumn6.DefaultCellStyle = DataGridViewCellStyle12
        Me.DataGridViewTextBoxColumn6.HeaderText = "UOM"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        Me.DataGridViewTextBoxColumn6.Width = 50
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.DataPropertyName = "amt"
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle13.Format = "N0"
        DataGridViewCellStyle13.NullValue = Nothing
        Me.DataGridViewTextBoxColumn7.DefaultCellStyle = DataGridViewCellStyle13
        Me.DataGridViewTextBoxColumn7.HeaderText = "Amount"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.ReadOnly = True
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.DataPropertyName = "supplier_name"
        DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle14.Format = "N0"
        DataGridViewCellStyle14.NullValue = Nothing
        Me.DataGridViewTextBoxColumn8.DefaultCellStyle = DataGridViewCellStyle14
        Me.DataGridViewTextBoxColumn8.HeaderText = "supplier_name"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        Me.DataGridViewTextBoxColumn8.ReadOnly = True
        Me.DataGridViewTextBoxColumn8.Visible = False
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.DataPropertyName = "comments"
        Me.DataGridViewTextBoxColumn9.HeaderText = "Comments"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        Me.DataGridViewTextBoxColumn9.ReadOnly = True
        Me.DataGridViewTextBoxColumn9.Visible = False
        Me.DataGridViewTextBoxColumn9.Width = 300
        '
        'DataGridViewTextBoxColumn10
        '
        Me.DataGridViewTextBoxColumn10.DataPropertyName = "is_amendment"
        Me.DataGridViewTextBoxColumn10.HeaderText = "is_amendment"
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        Me.DataGridViewTextBoxColumn10.ReadOnly = True
        Me.DataGridViewTextBoxColumn10.Visible = False
        Me.DataGridViewTextBoxColumn10.Width = 300
        '
        'DataGridViewTextBoxColumn11
        '
        Me.DataGridViewTextBoxColumn11.DataPropertyName = "is_reversal"
        Me.DataGridViewTextBoxColumn11.HeaderText = "is_reversal"
        Me.DataGridViewTextBoxColumn11.Name = "DataGridViewTextBoxColumn11"
        Me.DataGridViewTextBoxColumn11.ReadOnly = True
        Me.DataGridViewTextBoxColumn11.Visible = False
        '
        'DataGridViewTextBoxColumn12
        '
        Me.DataGridViewTextBoxColumn12.DataPropertyName = "updated_on"
        Me.DataGridViewTextBoxColumn12.HeaderText = "updated_on"
        Me.DataGridViewTextBoxColumn12.Name = "DataGridViewTextBoxColumn12"
        Me.DataGridViewTextBoxColumn12.ReadOnly = True
        Me.DataGridViewTextBoxColumn12.Visible = False
        '
        'DataGridViewTextBoxColumn13
        '
        Me.DataGridViewTextBoxColumn13.DataPropertyName = "updated_on"
        Me.DataGridViewTextBoxColumn13.HeaderText = "updated_on"
        Me.DataGridViewTextBoxColumn13.Name = "DataGridViewTextBoxColumn13"
        Me.DataGridViewTextBoxColumn13.ReadOnly = True
        Me.DataGridViewTextBoxColumn13.Visible = False
        '
        'DataGridViewTextBoxColumn14
        '
        Me.DataGridViewTextBoxColumn14.HeaderText = "sort_order"
        Me.DataGridViewTextBoxColumn14.Name = "DataGridViewTextBoxColumn14"
        Me.DataGridViewTextBoxColumn14.Visible = False
        '
        'tran_type
        '
        Me.tran_type.DataPropertyName = "tran_type"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.tran_type.DefaultCellStyle = DataGridViewCellStyle1
        Me.tran_type.HeaderText = "Type"
        Me.tran_type.Name = "tran_type"
        Me.tran_type.ReadOnly = True
        '
        'tran_date
        '
        Me.tran_date.DataPropertyName = "tran_date"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.Format = "dd/MM/yyyy"
        DataGridViewCellStyle2.NullValue = Nothing
        Me.tran_date.DefaultCellStyle = DataGridViewCellStyle2
        Me.tran_date.HeaderText = "Date"
        Me.tran_date.Name = "tran_date"
        Me.tran_date.ReadOnly = True
        Me.tran_date.Width = 75
        '
        'product_code
        '
        Me.product_code.DataPropertyName = "product_code"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.product_code.DefaultCellStyle = DataGridViewCellStyle3
        Me.product_code.HeaderText = "Product Code"
        Me.product_code.Name = "product_code"
        Me.product_code.ReadOnly = True
        Me.product_code.Width = 75
        '
        'prod_name
        '
        Me.prod_name.DataPropertyName = "prod_name"
        Me.prod_name.HeaderText = "Product Name"
        Me.prod_name.Name = "prod_name"
        Me.prod_name.ReadOnly = True
        Me.prod_name.Width = 200
        '
        'qty
        '
        Me.qty.DataPropertyName = "qty"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.Format = "N0"
        DataGridViewCellStyle4.NullValue = Nothing
        Me.qty.DefaultCellStyle = DataGridViewCellStyle4
        Me.qty.HeaderText = "Qty"
        Me.qty.Name = "qty"
        Me.qty.ReadOnly = True
        Me.qty.Width = 50
        '
        'qty_uom
        '
        Me.qty_uom.DataPropertyName = "qty_uom"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.qty_uom.DefaultCellStyle = DataGridViewCellStyle5
        Me.qty_uom.HeaderText = "UOM"
        Me.qty_uom.Name = "qty_uom"
        Me.qty_uom.ReadOnly = True
        Me.qty_uom.Width = 50
        '
        'amt
        '
        Me.amt.DataPropertyName = "amt"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle6.Format = "N0"
        DataGridViewCellStyle6.NullValue = Nothing
        Me.amt.DefaultCellStyle = DataGridViewCellStyle6
        Me.amt.HeaderText = "Amount"
        Me.amt.Name = "amt"
        Me.amt.ReadOnly = True
        '
        'balance
        '
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle7.Format = "N0"
        DataGridViewCellStyle7.NullValue = Nothing
        Me.balance.DefaultCellStyle = DataGridViewCellStyle7
        Me.balance.HeaderText = "Balance"
        Me.balance.Name = "balance"
        Me.balance.ReadOnly = True
        '
        'supplier_name
        '
        Me.supplier_name.DataPropertyName = "supplier_name"
        Me.supplier_name.HeaderText = "supplier_name"
        Me.supplier_name.Name = "supplier_name"
        Me.supplier_name.ReadOnly = True
        Me.supplier_name.Visible = False
        '
        'comments
        '
        Me.comments.DataPropertyName = "comments"
        Me.comments.HeaderText = "Comments"
        Me.comments.Name = "comments"
        Me.comments.ReadOnly = True
        Me.comments.Width = 300
        '
        'is_amendment
        '
        Me.is_amendment.DataPropertyName = "is_amendment"
        Me.is_amendment.HeaderText = "is_amendment"
        Me.is_amendment.Name = "is_amendment"
        Me.is_amendment.ReadOnly = True
        Me.is_amendment.Visible = False
        '
        'is_reversal
        '
        Me.is_reversal.DataPropertyName = "is_reversal"
        Me.is_reversal.HeaderText = "is_reversal"
        Me.is_reversal.Name = "is_reversal"
        Me.is_reversal.ReadOnly = True
        Me.is_reversal.Visible = False
        '
        'updated_on
        '
        Me.updated_on.DataPropertyName = "updated_on"
        Me.updated_on.HeaderText = "updated_on"
        Me.updated_on.Name = "updated_on"
        Me.updated_on.ReadOnly = True
        Me.updated_on.Visible = False
        '
        'sort_order
        '
        Me.sort_order.DataPropertyName = "sort_order"
        Me.sort_order.HeaderText = "sort_order"
        Me.sort_order.Name = "sort_order"
        Me.sort_order.Visible = False
        '
        'CreditPurchPaymentHistDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1172, 487)
        Me.Controls.Add(Me.lookupSupplierPaymentHistBtn)
        Me.Controls.Add(Me.supplierNameCmbBx)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.creditPurchPaymentHistoryGrdVw)
        Me.Controls.Add(Me.productLbl)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "CreditPurchPaymentHistDlg"
        Me.ShowIcon = False
        Me.Text = "Product Transaction History"
        CType(Me.creditPurchPaymentHistoryGrdVw, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents productLbl As Label
    Friend WithEvents creditPurchPaymentHistoryGrdVw As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents supplierNameCmbBx As System.Windows.Forms.ComboBox
    Friend WithEvents lookupSupplierPaymentHistBtn As System.Windows.Forms.Button
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn9 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn10 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn11 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn12 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn13 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn14 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents tran_type As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents tran_date As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents product_code As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents prod_name As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents qty As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents qty_uom As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents amt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents balance As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents supplier_name As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents comments As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents is_amendment As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents is_reversal As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents updated_on As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents sort_order As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
