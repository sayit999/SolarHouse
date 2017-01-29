<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SuppliersDlg
    Inherits BusinessReportEntityDlg

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.deleteSupplierBtn = New System.Windows.Forms.Button()
        Me.addSupplierBtn = New System.Windows.Forms.Button()
        Me.searchEntitiesText = New System.Windows.Forms.TextBox()
        Me.suppliersGridView = New SolarHouse.SuppliersEntityGridView()
        Me.code = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.entity_name = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.suppliersComments = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.id = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.saveBtn = New System.Windows.Forms.Button()
        Me.selectEntityBtn = New System.Windows.Forms.Button()
        CType(Me.suppliersGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'deleteSupplierBtn
        '
        Me.deleteSupplierBtn.Location = New System.Drawing.Point(1200, 28)
        Me.deleteSupplierBtn.Name = "deleteSupplierBtn"
        Me.deleteSupplierBtn.Size = New System.Drawing.Size(67, 26)
        Me.deleteSupplierBtn.TabIndex = 8
        Me.deleteSupplierBtn.Text = "delete"
        Me.deleteSupplierBtn.UseVisualStyleBackColor = True
        '
        'addSupplierBtn
        '
        Me.addSupplierBtn.Location = New System.Drawing.Point(1125, 28)
        Me.addSupplierBtn.Name = "addSupplierBtn"
        Me.addSupplierBtn.Size = New System.Drawing.Size(67, 26)
        Me.addSupplierBtn.TabIndex = 7
        Me.addSupplierBtn.Text = "insert"
        Me.addSupplierBtn.UseVisualStyleBackColor = True
        '
        'searchEntitiesText
        '
        Me.searchEntitiesText.Location = New System.Drawing.Point(16, 30)
        Me.searchEntitiesText.Name = "searchEntitiesText"
        Me.searchEntitiesText.Size = New System.Drawing.Size(807, 22)
        Me.searchEntitiesText.TabIndex = 6
        '
        'suppliersGridView
        '
        Me.suppliersGridView.AllowUserToAddRows = False
        Me.suppliersGridView.AllowUserToDeleteRows = False
        Me.suppliersGridView.BackgroundColor = System.Drawing.SystemColors.ActiveCaption
        Me.suppliersGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.suppliersGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.suppliersGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.suppliersGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.code, Me.entity_name, Me.suppliersComments, Me.id})
        Me.suppliersGridView.Location = New System.Drawing.Point(12, 72)
        Me.suppliersGridView.MultiSelect = False
        Me.suppliersGridView.Name = "suppliersGridView"
        Me.suppliersGridView.RowTemplate.Height = 24
        Me.suppliersGridView.Size = New System.Drawing.Size(1330, 499)
        Me.suppliersGridView.TabIndex = 9
        '
        'code
        '
        Me.code.DataPropertyName = "supplier_code"
        Me.code.HeaderText = "Code"
        Me.code.Name = "code"
        Me.code.ReadOnly = True
        Me.code.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'entity_name
        '
        Me.entity_name.DataPropertyName = "supplier_name"
        Me.entity_name.HeaderText = "Name"
        Me.entity_name.Name = "entity_name"
        Me.entity_name.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.entity_name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.entity_name.Width = 275
        '
        'suppliersComments
        '
        Me.suppliersComments.DataPropertyName = "comments"
        Me.suppliersComments.HeaderText = "Comments"
        Me.suppliersComments.Name = "suppliersComments"
        Me.suppliersComments.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.suppliersComments.Width = 400
        '
        'id
        '
        Me.id.DataPropertyName = "supplier_id"
        Me.id.HeaderText = "ID"
        Me.id.Name = "id"
        Me.id.ReadOnly = True
        Me.id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.id.Width = 75
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "supplier_code"
        Me.DataGridViewTextBoxColumn1.HeaderText = "supplier_code"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "comments"
        Me.DataGridViewTextBoxColumn2.HeaderText = "comments"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn2.Width = 400
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "supplier_id"
        Me.DataGridViewTextBoxColumn3.HeaderText = "supplier_id"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Width = 400
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "supplier_id"
        Me.DataGridViewTextBoxColumn4.HeaderText = "ID"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        '
        'saveBtn
        '
        Me.saveBtn.Location = New System.Drawing.Point(1275, 28)
        Me.saveBtn.Name = "saveBtn"
        Me.saveBtn.Size = New System.Drawing.Size(67, 26)
        Me.saveBtn.TabIndex = 11
        Me.saveBtn.Text = "save"
        Me.saveBtn.UseVisualStyleBackColor = True
        '
        'selectEntityBtn
        '
        Me.selectEntityBtn.Location = New System.Drawing.Point(829, 28)
        Me.selectEntityBtn.Name = "selectEntityBtn"
        Me.selectEntityBtn.Size = New System.Drawing.Size(67, 26)
        Me.selectEntityBtn.TabIndex = 12
        Me.selectEntityBtn.Text = "select"
        Me.selectEntityBtn.UseVisualStyleBackColor = True
        '
        'SuppliersDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1363, 590)
        Me.Controls.Add(Me.selectEntityBtn)
        Me.Controls.Add(Me.saveBtn)
        Me.Controls.Add(Me.suppliersGridView)
        Me.Controls.Add(Me.deleteSupplierBtn)
        Me.Controls.Add(Me.addSupplierBtn)
        Me.Controls.Add(Me.searchEntitiesText)
        Me.Name = "SuppliersDlg"
        Me.Text = "Suppliers"
        CType(Me.suppliersGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents deleteSupplierBtn As Button
    Friend WithEvents addSupplierBtn As Button
    Friend WithEvents searchEntitiesText As TextBox
    Friend WithEvents suppliersGridView As SuppliersEntityGridView
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents saveBtn As Button
    Friend WithEvents code As DataGridViewTextBoxColumn
    Friend WithEvents entity_name As DataGridViewTextBoxColumn
    Friend WithEvents suppliersComments As DataGridViewTextBoxColumn
    Friend WithEvents id As DataGridViewTextBoxColumn
    Friend WithEvents selectEntityBtn As Button
End Class
