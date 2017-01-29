Public Class SuppliersDlg

    Public Sub New(dlg As BusinessReportDlg)
        MyBase.New(dlg)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Protected Overrides Sub setDataSet(ds As DataTable)
        getBusinessReportDlg().suppliersEntityDataSet = ds
    End Sub

    Public Overrides Function getDataSet() As DataTable
        getDataSet = getBusinessReportDlg().suppliersEntityDataSet
    End Function

    Protected Overrides Function getGridView() As BusinessRprtEntityGridView
        getGridView = suppliersGridView
    End Function

    Protected Overrides Sub setGridView(grdVw As BusinessRprtEntityGridView)
        suppliersGridView = grdVw
    End Sub

    Protected Overrides Function getBusinessRprtEntityGridView(busDlg As BusinessReportDlg) As BusinessRprtEntityGridView
        suppliersGridView.DataSource = busDlg.suppliersEntityDataSet
        getBusinessRprtEntityGridView = suppliersGridView
    End Function


    Private Sub SuppliersDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim busDlg As BusinessReportDlg = getBusinessReportDlg()
        Me.suppliersGridView.setupGridView()
        initializeSearchEntitiesText(searchEntitiesText)
    End Sub

    Private Sub addSupplierBtn_Click(sender As Object, e As EventArgs) Handles addSupplierBtn.Click
        MyBase.insertRowAtCurrentPos()
    End Sub

    Private Sub deleteSupplierBtn_Click(sender As Object, e As EventArgs) Handles deleteSupplierBtn.Click
        MyBase.deleteRow("Suppliers", 0, 1)
    End Sub

    Private Sub searchProductsText_TextChanged(sender As Object, e As EventArgs) Handles searchEntitiesText.TextChanged
        highlightRowsMatchingSearchString(Me.searchEntitiesText.Text)
    End Sub

    Private Sub saveBtn_Click(sender As Object, e As EventArgs) Handles saveBtn.Click
        saveBtnClicked()
    End Sub

    Private Sub searchEntitiesText_KeyDown(sender As Object, e As KeyEventArgs) Handles searchEntitiesText.KeyDown
        If (e.KeyCode = Keys.Enter) Then
            Me.selectEntityBtnClicked()
        ElseIf isControlKeyPressed(e) Then
            suppliersGridView.gotoNextSelectedRow(e)
        End If
    End Sub

    Private Sub selectEntityBtn_Click(sender As Object, e As EventArgs) Handles selectEntityBtn.Click
        Me.selectEntityBtnClicked()
    End Sub
End Class