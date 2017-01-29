Public Class SuppliersEntityGridView
    Inherits BusinessRprtEntityGridView

    Public Overrides Sub setupGridView()
        MyBase.setupGridView()
    End Sub

    Protected Overrides Function insertEntityRow(rowAt As Integer) As DataRow
        Dim highcode As String = findTheNextCode("supplier_code", "SPLR")
        Dim row As DataRow = MyBase.insertEntityRow(rowAt)
        row.Item(0) = highcode
        Dim insRowInd As Integer = locateGridRow(highcode)
        Me.FirstDisplayedScrollingRowIndex = insRowInd - 1
        Me.CurrentCell = Me.Rows(insRowInd).Cells("entity_name")
        Me.BeginEdit(True)
    End Function

End Class
