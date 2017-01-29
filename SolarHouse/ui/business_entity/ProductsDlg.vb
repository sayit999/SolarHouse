Public Class ProductsDlg

    Public Sub New(dlg As BusinessReportDlg)
        MyBase.New(dlg)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Protected Overrides Sub setDataSet(ds As DataTable)
        getBusinessReportDlg().productsEntityDataSet = ds
    End Sub

    Public Overrides Function getDataSet() As DataTable
        getDataSet = getBusinessReportDlg().productsEntityDataSet
    End Function

    Protected Overrides Function getGridView() As BusinessRprtEntityGridView
        getGridView = productsGridView
    End Function

    Protected Overrides Sub setGridView(grdVw As BusinessRprtEntityGridView)
        productsGridView = grdVw
    End Sub

    Protected Overrides Function getBusinessRprtEntityGridView(busDlg As BusinessReportDlg) As BusinessRprtEntityGridView
        productsGridView.DataSource = busDlg.productsEntityDataSet
        getBusinessRprtEntityGridView = productsGridView
    End Function

    Private Sub ProductsDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim busDlg As BusinessReportDlg = getBusinessReportDlg()
        Dim cmBoxCol As New DataGridViewComboBoxColumn
        Dim catds As DataTable
        Dim qtyUomDS As DataTable

        Dim i As Integer
        catds = busDlg.catDataSet
        cmBoxCol = CType(productsGridView.Columns("productsProdCatName"), DataGridViewComboBoxColumn)
        cmBoxCol.Sorted = True
        For i = 0 To catds.Rows.Count - 1
            cmBoxCol.Items.Add(catds.Rows(i).Item("name"))
        Next

        qtyUomDS = busDlg.qtyUomDataSet
        cmBoxCol = CType(productsGridView.Columns("productsQtyUom"), DataGridViewComboBoxColumn)
        cmBoxCol.Sorted = True
        For i = 0 To qtyUomDS.Rows.Count - 1
            cmBoxCol.Items.Add(qtyUomDS.Rows(i).Item(1))
        Next


        productsGridView.setupGridView()
        initializeSearchEntitiesText(searchEntitiesText)

    End Sub


    Private Sub searchProductsText_TextChanged(sender As Object, e As EventArgs) Handles searchEntitiesText.TextChanged
        highlightRowsMatchingSearchString(searchEntitiesText.Text)
    End Sub

    Private Sub addProdBtn_Click(sender As Object, e As EventArgs) Handles addProdBtn.Click
        MyBase.insertRowAtCurrentPos()
    End Sub

    Private Sub deleteProdBtn_Click(sender As Object, e As EventArgs) Handles deleteProdBtn.Click
        MyBase.deleteRow("Products", 0, 1)
    End Sub


    Private Sub saveBtn_Click(sender As Object, e As EventArgs) Handles saveBtn.Click
        saveBtnClicked()
    End Sub

    Private Sub searchEntitiesText_KeyDown(sender As Object, e As KeyEventArgs) Handles searchEntitiesText.KeyDown, searchEntitiesText.KeyUp
        If (e.KeyCode = Keys.Enter) Then
            selectEntityBtnClicked()
        ElseIf isControlKeyPressed(e) Then
            productsGridView.gotoNextSelectedRow(e)
            e.Handled = False
        End If
    End Sub

    Private Sub selectEntityBtn_Click(sender As Object, e As EventArgs) Handles selectEntityBtn.Click
        selectEntityBtnClicked()
    End Sub



End Class