<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ExpenseCategorieDlg
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.expensesEntityGridView = New SolarHouse.ExpenseCategoriesEntityGridView()
        Me.code = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.entity_name = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.exp_desc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.id = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.addExpCatBtn = New System.Windows.Forms.Button()
        Me.searchEntitiesText = New System.Windows.Forms.TextBox()
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
        Me.delExpCatBtn = New System.Windows.Forms.Button()
        Me.saveBtn = New System.Windows.Forms.Button()
        Me.selectEntityBtn = New System.Windows.Forms.Button()
        CType(Me.expensesEntityGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'expensesEntityGridView
        '
        Me.expensesEntityGridView.AllowUserToAddRows = False
        Me.expensesEntityGridView.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.expensesEntityGridView.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.expensesEntityGridView.BackgroundColor = System.Drawing.SystemColors.ActiveCaption
        Me.expensesEntityGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.expensesEntityGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.expensesEntityGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.expensesEntityGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.code, Me.entity_name, Me.exp_desc, Me.id})
        Me.expensesEntityGridView.Location = New System.Drawing.Point(12, 56)
        Me.expensesEntityGridView.MultiSelect = False
        Me.expensesEntityGridView.Name = "expensesEntityGridView"
        Me.expensesEntityGridView.RowTemplate.Height = 24
        Me.expensesEntityGridView.Size = New System.Drawing.Size(1342, 447)
        Me.expensesEntityGridView.TabIndex = 2
        '
        'code
        '
        Me.code.DataPropertyName = "expense_category_code"
        Me.code.HeaderText = "Code"
        Me.code.Name = "code"
        Me.code.ReadOnly = True
        Me.code.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'entity_name
        '
        Me.entity_name.DataPropertyName = "expense_category_name"
        Me.entity_name.HeaderText = "Name"
        Me.entity_name.Name = "entity_name"
        Me.entity_name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.entity_name.Width = 300
        '
        'exp_desc
        '
        Me.exp_desc.DataPropertyName = "expense_category_description"
        Me.exp_desc.HeaderText = "Description"
        Me.exp_desc.Name = "exp_desc"
        Me.exp_desc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.exp_desc.Width = 500
        '
        'id
        '
        Me.id.DataPropertyName = "expense_category_id"
        Me.id.HeaderText = "ID"
        Me.id.Name = "id"
        Me.id.ReadOnly = True
        Me.id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'addExpCatBtn
        '
        Me.addExpCatBtn.Location = New System.Drawing.Point(1087, 10)
        Me.addExpCatBtn.Name = "addExpCatBtn"
        Me.addExpCatBtn.Size = New System.Drawing.Size(85, 26)
        Me.addExpCatBtn.TabIndex = 12
        Me.addExpCatBtn.Text = "insert"
        Me.addExpCatBtn.UseVisualStyleBackColor = True
        '
        'searchEntitiesText
        '
        Me.searchEntitiesText.Location = New System.Drawing.Point(12, 12)
        Me.searchEntitiesText.Name = "searchEntitiesText"
        Me.searchEntitiesText.Size = New System.Drawing.Size(807, 22)
        Me.searchEntitiesText.TabIndex = 0
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "expense_category_code"
        Me.DataGridViewTextBoxColumn1.HeaderText = "Code"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "expense_category_name"
        Me.DataGridViewTextBoxColumn2.FillWeight = 200.0!
        Me.DataGridViewTextBoxColumn2.HeaderText = "name"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.Width = 200
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "expense_category_description"
        Me.DataGridViewTextBoxColumn3.HeaderText = "Description"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.Width = 400
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "expense_category_id"
        Me.DataGridViewTextBoxColumn4.HeaderText = "ID"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "expense_category_id"
        Me.DataGridViewTextBoxColumn5.HeaderText = "expense_category_id"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "expense_category_code"
        Me.DataGridViewTextBoxColumn6.HeaderText = "expense_category_code"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.DataPropertyName = "expense_category_name"
        Me.DataGridViewTextBoxColumn7.HeaderText = "expense_category_name"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.DataPropertyName = "expense_category_description"
        Me.DataGridViewTextBoxColumn8.HeaderText = "expense_category_description"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.DataPropertyName = "updated_on"
        Me.DataGridViewTextBoxColumn9.HeaderText = "updated_on"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        '
        'DataGridViewTextBoxColumn10
        '
        Me.DataGridViewTextBoxColumn10.DataPropertyName = "updated_by"
        Me.DataGridViewTextBoxColumn10.HeaderText = "updated_by"
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        '
        'delExpCatBtn
        '
        Me.delExpCatBtn.Location = New System.Drawing.Point(1178, 10)
        Me.delExpCatBtn.Name = "delExpCatBtn"
        Me.delExpCatBtn.Size = New System.Drawing.Size(85, 26)
        Me.delExpCatBtn.TabIndex = 13
        Me.delExpCatBtn.Text = "delete"
        Me.delExpCatBtn.UseVisualStyleBackColor = True
        '
        'saveBtn
        '
        Me.saveBtn.Location = New System.Drawing.Point(1269, 10)
        Me.saveBtn.Name = "saveBtn"
        Me.saveBtn.Size = New System.Drawing.Size(85, 26)
        Me.saveBtn.TabIndex = 15
        Me.saveBtn.Text = "save"
        Me.saveBtn.UseVisualStyleBackColor = True
        '
        'selectEntityBtn
        '
        Me.selectEntityBtn.Location = New System.Drawing.Point(825, 10)
        Me.selectEntityBtn.Name = "selectEntityBtn"
        Me.selectEntityBtn.Size = New System.Drawing.Size(85, 26)
        Me.selectEntityBtn.TabIndex = 16
        Me.selectEntityBtn.Text = "select"
        Me.selectEntityBtn.UseVisualStyleBackColor = True
        '
        'ExpenseCategorieDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1381, 527)
        Me.Controls.Add(Me.selectEntityBtn)
        Me.Controls.Add(Me.saveBtn)
        Me.Controls.Add(Me.delExpCatBtn)
        Me.Controls.Add(Me.addExpCatBtn)
        Me.Controls.Add(Me.searchEntitiesText)
        Me.Controls.Add(Me.expensesEntityGridView)
        Me.Name = "ExpenseCategorieDlg"
        Me.Text = "Expense Categories"
        CType(Me.expensesEntityGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents expensesEntityGridView As ExpenseCategoriesEntityGridView
    Friend WithEvents addExpCatBtn As Button
    Friend WithEvents searchEntitiesText As TextBox

    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn9 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn10 As DataGridViewTextBoxColumn
    Friend WithEvents delExpCatBtn As Button
    Friend WithEvents saveBtn As Button
    Friend WithEvents code As DataGridViewTextBoxColumn
    Friend WithEvents entity_name As DataGridViewTextBoxColumn
    Friend WithEvents exp_desc As DataGridViewTextBoxColumn
    Friend WithEvents id As DataGridViewTextBoxColumn
    Friend WithEvents selectEntityBtn As Button
End Class
