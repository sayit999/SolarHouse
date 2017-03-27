<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ProductsDlg
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
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.deleteProdBtn = New System.Windows.Forms.Button()
        Me.addProdBtn = New System.Windows.Forms.Button()
        Me.searchEntitiesText = New System.Windows.Forms.TextBox()
        Me.productsGridView = New SolarHouse.ProductsEntityGridView()
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
        Me.DataGridViewTextBoxColumn15 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn16 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.saveBtn = New System.Windows.Forms.Button()
        Me.selectEntityBtn = New System.Windows.Forms.Button()
        Me.code = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.entity_name = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.productsProdCatName = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.productsCategoryId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.productsQtyAvail = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.productsQtyUom = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.productsQtyUomId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.productsAcbCost = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.productsRetaildiscountroompercentage = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.productsMinretgrossprofitmarginpercentage = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.productsRetailSalePrice = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.prodIsReorder = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.productsComments = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.entity_id = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.productsGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'deleteProdBtn
        '
        Me.deleteProdBtn.Location = New System.Drawing.Point(872, 21)
        Me.deleteProdBtn.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.deleteProdBtn.Name = "deleteProdBtn"
        Me.deleteProdBtn.Size = New System.Drawing.Size(58, 21)
        Me.deleteProdBtn.TabIndex = 5
        Me.deleteProdBtn.Text = "delete"
        Me.deleteProdBtn.UseVisualStyleBackColor = True
        '
        'addProdBtn
        '
        Me.addProdBtn.Location = New System.Drawing.Point(810, 21)
        Me.addProdBtn.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.addProdBtn.Name = "addProdBtn"
        Me.addProdBtn.Size = New System.Drawing.Size(58, 21)
        Me.addProdBtn.TabIndex = 4
        Me.addProdBtn.Text = "insert"
        Me.addProdBtn.UseVisualStyleBackColor = True
        '
        'searchEntitiesText
        '
        Me.searchEntitiesText.Location = New System.Drawing.Point(9, 23)
        Me.searchEntitiesText.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.searchEntitiesText.Name = "searchEntitiesText"
        Me.searchEntitiesText.Size = New System.Drawing.Size(606, 20)
        Me.searchEntitiesText.TabIndex = 0
        '
        'productsGridView
        '
        Me.productsGridView.AllowUserToAddRows = False
        Me.productsGridView.AllowUserToDeleteRows = False
        Me.productsGridView.BackgroundColor = System.Drawing.SystemColors.ActiveCaption
        Me.productsGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.productsGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.productsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.productsGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.code, Me.entity_name, Me.productsProdCatName, Me.productsCategoryId, Me.productsQtyAvail, Me.productsQtyUom, Me.productsQtyUomId, Me.productsAcbCost, Me.productsRetaildiscountroompercentage, Me.productsMinretgrossprofitmarginpercentage, Me.productsRetailSalePrice, Me.prodIsReorder, Me.productsComments, Me.entity_id})
        Me.productsGridView.Location = New System.Drawing.Point(9, 58)
        Me.productsGridView.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.productsGridView.MultiSelect = False
        Me.productsGridView.Name = "productsGridView"
        Me.productsGridView.RowTemplate.Height = 24
        Me.productsGridView.Size = New System.Drawing.Size(984, 396)
        Me.productsGridView.TabIndex = 1
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "product_id"
        DataGridViewCellStyle11.BackColor = System.Drawing.Color.Aqua
        Me.DataGridViewTextBoxColumn1.DefaultCellStyle = DataGridViewCellStyle11
        Me.DataGridViewTextBoxColumn1.HeaderText = "product_id"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 50
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "product_code"
        DataGridViewCellStyle12.BackColor = System.Drawing.Color.Aqua
        Me.DataGridViewTextBoxColumn2.DefaultCellStyle = DataGridViewCellStyle12
        Me.DataGridViewTextBoxColumn2.HeaderText = "product_code"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.Width = 60
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "product_name"
        DataGridViewCellStyle13.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle13
        Me.DataGridViewTextBoxColumn3.HeaderText = "product_name"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Width = 400
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "product_category_id"
        DataGridViewCellStyle14.Format = "N0"
        DataGridViewCellStyle14.NullValue = Nothing
        Me.DataGridViewTextBoxColumn4.DefaultCellStyle = DataGridViewCellStyle14
        Me.DataGridViewTextBoxColumn4.HeaderText = "product_category_id"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.Width = 30
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "qty_available"
        Me.DataGridViewTextBoxColumn5.HeaderText = "qty_available"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.Width = 30
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "qty_uom"
        Me.DataGridViewTextBoxColumn6.HeaderText = "qty_uom"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.DataPropertyName = "acb_cost"
        Me.DataGridViewTextBoxColumn7.HeaderText = "acb_cost"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.DataPropertyName = "retail_discount_room_percentage"
        Me.DataGridViewTextBoxColumn8.HeaderText = "retail_discount_room_percentage"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        Me.DataGridViewTextBoxColumn8.ReadOnly = True
        Me.DataGridViewTextBoxColumn8.Visible = False
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.DataPropertyName = "min_ret_gross_profit_margin_percentage"
        Me.DataGridViewTextBoxColumn9.HeaderText = "min_ret_gross_profit_margin_percentage"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        Me.DataGridViewTextBoxColumn9.ReadOnly = True
        Me.DataGridViewTextBoxColumn9.Visible = False
        '
        'DataGridViewTextBoxColumn10
        '
        Me.DataGridViewTextBoxColumn10.DataPropertyName = "min_ws_gross_profit_margin_percentage"
        Me.DataGridViewTextBoxColumn10.HeaderText = "min_ws_gross_profit_margin_percentage"
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        Me.DataGridViewTextBoxColumn10.ReadOnly = True
        Me.DataGridViewTextBoxColumn10.Visible = False
        '
        'DataGridViewTextBoxColumn11
        '
        Me.DataGridViewTextBoxColumn11.DataPropertyName = "retail_sale_price"
        Me.DataGridViewTextBoxColumn11.HeaderText = "retail_sale_price"
        Me.DataGridViewTextBoxColumn11.Name = "DataGridViewTextBoxColumn11"
        Me.DataGridViewTextBoxColumn11.ReadOnly = True
        Me.DataGridViewTextBoxColumn11.Visible = False
        '
        'DataGridViewTextBoxColumn12
        '
        Me.DataGridViewTextBoxColumn12.DataPropertyName = "wholesale_sale_price"
        Me.DataGridViewTextBoxColumn12.HeaderText = "wholesale_sale_price"
        Me.DataGridViewTextBoxColumn12.Name = "DataGridViewTextBoxColumn12"
        Me.DataGridViewTextBoxColumn12.ReadOnly = True
        Me.DataGridViewTextBoxColumn12.Visible = False
        Me.DataGridViewTextBoxColumn12.Width = 400
        '
        'DataGridViewTextBoxColumn13
        '
        Me.DataGridViewTextBoxColumn13.DataPropertyName = "is_reorder"
        Me.DataGridViewTextBoxColumn13.HeaderText = "is_reorder"
        Me.DataGridViewTextBoxColumn13.Name = "DataGridViewTextBoxColumn13"
        Me.DataGridViewTextBoxColumn13.Visible = False
        Me.DataGridViewTextBoxColumn13.Width = 400
        '
        'DataGridViewTextBoxColumn14
        '
        Me.DataGridViewTextBoxColumn14.DataPropertyName = "comments"
        Me.DataGridViewTextBoxColumn14.HeaderText = "comments"
        Me.DataGridViewTextBoxColumn14.Name = "DataGridViewTextBoxColumn14"
        Me.DataGridViewTextBoxColumn14.Visible = False
        Me.DataGridViewTextBoxColumn14.Width = 400
        '
        'DataGridViewTextBoxColumn15
        '
        Me.DataGridViewTextBoxColumn15.DataPropertyName = "updated_on"
        DataGridViewCellStyle15.BackColor = System.Drawing.Color.Aqua
        Me.DataGridViewTextBoxColumn15.DefaultCellStyle = DataGridViewCellStyle15
        Me.DataGridViewTextBoxColumn15.HeaderText = "updated_on"
        Me.DataGridViewTextBoxColumn15.Name = "DataGridViewTextBoxColumn15"
        Me.DataGridViewTextBoxColumn15.ReadOnly = True
        Me.DataGridViewTextBoxColumn15.Visible = False
        Me.DataGridViewTextBoxColumn15.Width = 50
        '
        'DataGridViewTextBoxColumn16
        '
        Me.DataGridViewTextBoxColumn16.DataPropertyName = "updated_by"
        DataGridViewCellStyle16.BackColor = System.Drawing.Color.Aqua
        Me.DataGridViewTextBoxColumn16.DefaultCellStyle = DataGridViewCellStyle16
        Me.DataGridViewTextBoxColumn16.HeaderText = "updated_by"
        Me.DataGridViewTextBoxColumn16.Name = "DataGridViewTextBoxColumn16"
        Me.DataGridViewTextBoxColumn16.ReadOnly = True
        Me.DataGridViewTextBoxColumn16.Visible = False
        Me.DataGridViewTextBoxColumn16.Width = 50
        '
        'saveBtn
        '
        Me.saveBtn.Location = New System.Drawing.Point(934, 21)
        Me.saveBtn.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.saveBtn.Name = "saveBtn"
        Me.saveBtn.Size = New System.Drawing.Size(58, 21)
        Me.saveBtn.TabIndex = 8
        Me.saveBtn.Text = "save"
        Me.saveBtn.UseVisualStyleBackColor = True
        '
        'selectEntityBtn
        '
        Me.selectEntityBtn.Location = New System.Drawing.Point(620, 21)
        Me.selectEntityBtn.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.selectEntityBtn.Name = "selectEntityBtn"
        Me.selectEntityBtn.Size = New System.Drawing.Size(58, 21)
        Me.selectEntityBtn.TabIndex = 9
        Me.selectEntityBtn.Text = "select"
        Me.selectEntityBtn.UseVisualStyleBackColor = True
        '
        'code
        '
        Me.code.DataPropertyName = "product_code"
        Me.code.HeaderText = "Code"
        Me.code.Name = "code"
        Me.code.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.code.Width = 75
        '
        'entity_name
        '
        Me.entity_name.DataPropertyName = "product_name"
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.Aqua
        Me.entity_name.DefaultCellStyle = DataGridViewCellStyle2
        Me.entity_name.HeaderText = "Name"
        Me.entity_name.Name = "entity_name"
        Me.entity_name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.entity_name.Width = 300
        '
        'productsProdCatName
        '
        Me.productsProdCatName.DataPropertyName = "product_category_name"
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.PaleTurquoise
        Me.productsProdCatName.DefaultCellStyle = DataGridViewCellStyle3
        Me.productsProdCatName.HeaderText = "Category"
        Me.productsProdCatName.Name = "productsProdCatName"
        Me.productsProdCatName.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.productsProdCatName.Width = 200
        '
        'productsCategoryId
        '
        Me.productsCategoryId.DataPropertyName = "product_category_id"
        Me.productsCategoryId.HeaderText = "productsCategoryId"
        Me.productsCategoryId.Name = "productsCategoryId"
        Me.productsCategoryId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.productsCategoryId.Visible = False
        '
        'productsQtyAvail
        '
        Me.productsQtyAvail.DataPropertyName = "qty_available"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle4.Format = "N0"
        DataGridViewCellStyle4.NullValue = "0"
        Me.productsQtyAvail.DefaultCellStyle = DataGridViewCellStyle4
        Me.productsQtyAvail.HeaderText = "Qty Avail"
        Me.productsQtyAvail.Name = "productsQtyAvail"
        Me.productsQtyAvail.ReadOnly = True
        Me.productsQtyAvail.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.productsQtyAvail.Width = 75
        '
        'productsQtyUom
        '
        Me.productsQtyUom.DataPropertyName = "qty_uom_name"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.productsQtyUom.DefaultCellStyle = DataGridViewCellStyle5
        Me.productsQtyUom.HeaderText = "Qty UOM"
        Me.productsQtyUom.Name = "productsQtyUom"
        Me.productsQtyUom.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.productsQtyUom.Width = 75
        '
        'productsQtyUomId
        '
        Me.productsQtyUomId.DataPropertyName = "qty_uom_id"
        Me.productsQtyUomId.HeaderText = "productsQtyUomId"
        Me.productsQtyUomId.Name = "productsQtyUomId"
        Me.productsQtyUomId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.productsQtyUomId.Visible = False
        '
        'productsAcbCost
        '
        Me.productsAcbCost.DataPropertyName = "acb_cost"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle6.Format = "N0"
        DataGridViewCellStyle6.NullValue = "0"
        Me.productsAcbCost.DefaultCellStyle = DataGridViewCellStyle6
        Me.productsAcbCost.HeaderText = "Cost"
        Me.productsAcbCost.Name = "productsAcbCost"
        Me.productsAcbCost.ReadOnly = True
        Me.productsAcbCost.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'productsRetaildiscountroompercentage
        '
        Me.productsRetaildiscountroompercentage.DataPropertyName = "retail_discount_room_percentage"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle7.Format = "N0"
        DataGridViewCellStyle7.NullValue = "0"
        Me.productsRetaildiscountroompercentage.DefaultCellStyle = DataGridViewCellStyle7
        Me.productsRetaildiscountroompercentage.HeaderText = "Ret Disc. Room %"
        Me.productsRetaildiscountroompercentage.Name = "productsRetaildiscountroompercentage"
        Me.productsRetaildiscountroompercentage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'productsMinretgrossprofitmarginpercentage
        '
        Me.productsMinretgrossprofitmarginpercentage.DataPropertyName = "min_ret_gross_profit_margin_percentage"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle8.Format = "N0"
        DataGridViewCellStyle8.NullValue = "0"
        Me.productsMinretgrossprofitmarginpercentage.DefaultCellStyle = DataGridViewCellStyle8
        Me.productsMinretgrossprofitmarginpercentage.HeaderText = "Min Ret. GP Margin %"
        Me.productsMinretgrossprofitmarginpercentage.Name = "productsMinretgrossprofitmarginpercentage"
        Me.productsMinretgrossprofitmarginpercentage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'productsRetailSalePrice
        '
        Me.productsRetailSalePrice.DataPropertyName = "retail_sale_price"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle9.Format = "N0"
        DataGridViewCellStyle9.NullValue = "0"
        Me.productsRetailSalePrice.DefaultCellStyle = DataGridViewCellStyle9
        Me.productsRetailSalePrice.HeaderText = "Ret Sale Price"
        Me.productsRetailSalePrice.Name = "productsRetailSalePrice"
        Me.productsRetailSalePrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'prodIsReorder
        '
        Me.prodIsReorder.DataPropertyName = "is_reorder"
        Me.prodIsReorder.HeaderText = "Reorder?"
        Me.prodIsReorder.Name = "prodIsReorder"
        Me.prodIsReorder.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.prodIsReorder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'productsComments
        '
        Me.productsComments.DataPropertyName = "comments"
        Me.productsComments.HeaderText = "Comments"
        Me.productsComments.Name = "productsComments"
        Me.productsComments.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.productsComments.Width = 400
        '
        'entity_id
        '
        Me.entity_id.DataPropertyName = "product_id"
        DataGridViewCellStyle10.BackColor = System.Drawing.Color.Aqua
        Me.entity_id.DefaultCellStyle = DataGridViewCellStyle10
        Me.entity_id.HeaderText = "ID"
        Me.entity_id.Name = "entity_id"
        Me.entity_id.ReadOnly = True
        Me.entity_id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.entity_id.Width = 50
        '
        'ProductsDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1015, 476)
        Me.Controls.Add(Me.selectEntityBtn)
        Me.Controls.Add(Me.saveBtn)
        Me.Controls.Add(Me.deleteProdBtn)
        Me.Controls.Add(Me.addProdBtn)
        Me.Controls.Add(Me.searchEntitiesText)
        Me.Controls.Add(Me.productsGridView)
        Me.Name = "ProductsDlg"
        Me.Text = "Products"
        CType(Me.productsGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents productsGridView As ProductsEntityGridView
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
    Friend WithEvents DataGridViewTextBoxColumn11 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn12 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn13 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn14 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn15 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn16 As DataGridViewTextBoxColumn
    Friend WithEvents addProdBtn As Button
    Friend WithEvents deleteProdBtn As Button

    Friend WithEvents saveBtn As Button
    Friend WithEvents selectEntityBtn As Button
    Friend WithEvents code As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents entity_name As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents productsProdCatName As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents productsCategoryId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents productsQtyAvail As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents productsQtyUom As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents productsQtyUomId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents productsAcbCost As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents productsRetaildiscountroompercentage As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents productsMinretgrossprofitmarginpercentage As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents productsRetailSalePrice As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents prodIsReorder As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents productsComments As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents entity_id As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
