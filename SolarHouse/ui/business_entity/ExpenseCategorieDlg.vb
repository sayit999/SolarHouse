Public Class ExpenseCategorieDlg

    Public Sub New(dlg As BusinessReportDlg)
        MyBase.New(dlg)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Protected Overrides Sub setDataSet(ds As DataTable)
        getBusinessReportDlg().expenseCategoriesEntityDataSet = ds
    End Sub

    Public Overrides Function getDataSet() As DataTable
        If IsNothing(getBusinessReportDlg()) Then
            Return Nothing
        Else
            getDataSet = getBusinessReportDlg().expenseCategoriesEntityDataSet

        End If
    End Function

    Protected Overrides Function getGridView() As BusinessRprtEntityGridView
        getGridView = expensesEntityGridView
    End Function

    Protected Overrides Sub setGridView(grdVw As BusinessRprtEntityGridView)
        expensesEntityGridView = grdVw
    End Sub

    Protected Overrides Function getBusinessRprtEntityGridView(busDlg As BusinessReportDlg) As BusinessRprtEntityGridView
        expensesEntityGridView.DataSource = busDlg.expenseCategoriesEntityDataSet
        getBusinessRprtEntityGridView = expensesEntityGridView
    End Function

    Private Sub ExpenseCategorieDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim busDlg As BusinessReportDlg = getBusinessReportDlg()
        Me.expensesEntityGridView.setupGridView()
        initializeSearchEntitiesText(searchEntitiesText)
    End Sub

    Private Sub searchProductsText_TextChanged(sender As Object, e As EventArgs) Handles searchEntitiesText.TextChanged
        highlightRowsMatchingSearchString(searchEntitiesText.Text)
    End Sub

    Private Sub addExpCatBtn_Click(sender As Object, e As EventArgs) Handles addExpCatBtn.Click
        MyBase.insertRowAtCurrentPos()
    End Sub

    Private Sub delExpCatBtn_Click(sender As Object, e As EventArgs) Handles delExpCatBtn.Click
        MyBase.deleteRow("Expense Category", 0, 1)
    End Sub


    Private Sub saveBtn_Click(sender As Object, e As EventArgs) Handles saveBtn.Click
        saveBtnClicked()
    End Sub

    Private Sub searchEntitiesText_KeyDown(sender As Object, e As KeyEventArgs) Handles searchEntitiesText.KeyDown
        If (e.KeyCode = Keys.Enter) Then
            selectEntityBtnClicked()
        ElseIf isControlKeyPressed(e) Then
            expensesEntityGridView.gotoNextSelectedRow(e)
        End If

    End Sub

    Private Sub selectEntityBtn_Click(sender As Object, e As EventArgs) Handles selectEntityBtn.Click
        Me.selectEntityBtnClicked()
    End Sub
End Class