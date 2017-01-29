Public Class ProductsEntityGridView
    Inherits BusinessRprtEntityGridView

    Private Const NEW_CODE_TEMP_VAL As String = "<new code>"

    Public Overrides Sub setupGridView()
        MyBase.setupGridView()

    End Sub

    Protected Overrides Sub doValidateRow(row As Integer, ByRef result As RowValidationResult)
        MyBase.doValidateRow(row, result)

        If LCase(UIUtil.subsIfEmpty(Rows(row).Cells("code").Value, "")) = NEW_CODE_TEMP_VAL Then
            addError(result, " is not a valid value.  Enter a valid code", row, Rows(row).Cells("code").ColumnIndex)
        End If

        If (StringUtil.isEmpty(Rows(row).Cells("productsProdCatName").Value)) Then
            addError(result, "Product Category cannot be empty", row, "productsProdCatName")
        End If

        Dim i As Integer = UIUtil.zeroIfEmpty(Rows(row).Cells("productsQtyAvail").Value)
        If (i < 0) Then
            addError(result, "Qty cannot be negative", row, "productsQtyAvail")
        End If

        If (StringUtil.isEmpty(Rows(row).Cells("productsQtyUom").Value)) Then
            addError(result, "UOM cannot be empty", row, "productsQtyUom")
        End If
    End Sub


    Protected Overrides Sub OnCellValueChanged(e As DataGridViewCellEventArgs)
        MyBase.OnCellValueChanged(e)
        If e.RowIndex < 0 And e.RowIndex >= Rows.Count Then
            Exit Sub
        End If
        If (UIUtil.grdVwColNameToIndex("code", Me) = e.ColumnIndex) Then
        ElseIf (UIUtil.grdVwColNameToIndex("productsProdCatName", Me) = e.ColumnIndex) Then
            Rows(e.RowIndex).Cells("productsCategoryId").Value = lookupId(Rows(e.RowIndex).Cells(e.ColumnIndex).Value, getBusinessReportDlg().catDataSet, "name", "id")
        ElseIf (UIUtil.grdVwColNameToIndex("productsQtyUom", Me) = e.ColumnIndex) Then
            Rows(e.RowIndex).Cells("productsQtyUomId").Value = lookupId(Rows(e.RowIndex).Cells(e.ColumnIndex).Value, getBusinessReportDlg().qtyUomDataSet, "name", "id")
        End If
    End Sub


    Protected Overrides Sub OnCellBeginEdit(e As DataGridViewCellCancelEventArgs)
        If e.ColumnIndex = Columns("code").Index AndAlso UIUtil.subsIfEmpty(CurrentCell.Value, "") = NEW_CODE_TEMP_VAL Then
            CurrentCell.Value = ""
        End If
        MyBase.OnCellBeginEdit(e)
    End Sub

    'Protected Overrides Function insertEntityRow(rowAt As Integer) As DataRow
    '    Dim highcode As String = NEW_CODE_TEMP_VAL
    '    Dim row As DataRow = MyBase.insertEntityRow(rowAt)
    '    row.Item(0) = highcode
    '    Dim insRowInd As Integer = locateGridRow(highcode)
    '    Me.FirstDisplayedScrollingRowIndex = insRowInd
    '    Me.CurrentCell = Me.Rows(insRowInd).Cells("code")
    '    'Me.BeginEdit(True)
    'End Function

    Protected Overrides Sub OnKeyUp(e As KeyEventArgs)
        If isControlSPressed(e) Then
            If Columns(CurrentCell.ColumnIndex).Name = "productsQtyAvail" Then
                Dim dlg As SubmittedProductAcbAndQtyDlg = New SubmittedProductAcbAndQtyDlg(Rows(CurrentCell.RowIndex).Cells("code").Value)
                dlg.ShowDialog(Me)
                e.Handled = False
            End If
        End If

        MyBase.OnKeyUp(e)
    End Sub

    Protected Function findRowToFindCodeToFindNextCodeFor(rowAt As Integer, dt As DataTable) As String
        If rowAt > 0 Then
            Return rowAt - 1
        ElseIf rowAt < dt.Rows.Count - 1 Then
            Return rowAt + 1
        Else
            Return -1
        End If
    End Function

    Protected Function findCodeToFindNextCodeFor(rowAt As Integer, rowToFindBaseCode As Integer, dt As DataTable) As String

        Dim codeToBaseOn As String = Nothing
        If rowToFindBaseCode >= 0 Then
            codeToBaseOn = dt.Rows(rowToFindBaseCode).Item("product_code").ToString()
            Dim i As Integer
            For i = 0 To codeToBaseOn.Length - 1
                If IsNumeric(codeToBaseOn(i)) Then
                    Exit For
                End If
            Next
            codeToBaseOn = codeToBaseOn.Substring(0, i)
            Return findTheNextCode("product_code", codeToBaseOn)
        Else
            Return NEW_CODE_TEMP_VAL
        End If

    End Function

    Protected Overrides Function insertEntityRow(rowAt As Integer) As DataRow
        Dim dt As DataTable = Me.DataSource
        Dim rowToFindBaseCode As Integer = findRowToFindCodeToFindNextCodeFor(rowAt, dt)
        Dim col As Integer
        Dim highcode As String = findCodeToFindNextCodeFor(rowAt, rowToFindBaseCode, dt)
        Dim row As DataRow = MyBase.insertEntityRow(rowAt)
        row.Item(dt.Columns.IndexOf("product_code")) = highcode

        col = dt.Columns.IndexOf("product_category_name")
        row.Item(col) = dt.Rows(rowToFindBaseCode).Item(col)

        col = dt.Columns.IndexOf("product_category_id")
        row.Item(col) = dt.Rows(rowToFindBaseCode).Item(col)

        col = dt.Columns.IndexOf("qty_uom_name")
        row.Item(col) = dt.Rows(rowToFindBaseCode).Item(col)

        col = dt.Columns.IndexOf("qty_uom_id")
        row.Item(col) = dt.Rows(rowToFindBaseCode).Item(col)

        col = dt.Columns.IndexOf("is_reorder")
        row.Item(col) = UIUtil.toBinaryBooleanString(True)

        Dim insRowInd As Integer = locateGridRow(highcode)
        Me.FirstDisplayedScrollingRowIndex = insRowInd - 1
        Me.CurrentCell = Me.Rows(insRowInd).Cells("entity_name")
        Me.BeginEdit(True)
        Return row
    End Function

End Class
