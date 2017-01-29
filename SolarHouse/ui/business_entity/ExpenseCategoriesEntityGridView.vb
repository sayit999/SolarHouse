Public Class ExpenseCategoriesEntityGridView
    Inherits BusinessRprtEntityGridView

    Public Overrides Sub setupGridView()

        MyBase.setupGridView()
    End Sub

    Protected Overrides Function insertEntityRow(rowAt As Integer) As DataRow
        Dim highcode As String = findTheNextCode("expense_category_code", "MTZ")
        Dim row As DataRow = MyBase.insertEntityRow(rowAt)
        row.Item(0) = highcode
        Dim insRowInd As Integer = locateGridRow(highcode)
        Me.FirstDisplayedScrollingRowIndex = insRowInd - 1
        Me.CurrentCell = Me.Rows(insRowInd).Cells("entity_name")
        Me.BeginEdit(True)
        Return row
    End Function

End Class
