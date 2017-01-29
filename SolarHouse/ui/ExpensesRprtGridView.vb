Public Class ExpensesRprtGridView
    Inherits BusinessReportGridView

    Public Overrides Sub setupGridView()
        MyBase.setupGridView()
    End Sub

    Protected Overrides Function isReversalTransaction(row As Integer) As Boolean
        Return UIUtil.toBoolean(Rows(row).Cells("isExpensePostedReversal").Value)
    End Function

    Protected Overrides Sub indicateTransactionReversed(row As Integer, Optional isReversed As Boolean = True)
        isProgramaticChange = False
        Rows(row).Cells("isExpensePostedReversal").Value = UIUtil.toBinaryBooleanString(isReversed)
        Me.Rows(row).Cells("expenseAmount").Value = -1 * Me.Rows(row).Cells("expenseAmount").Value
        isProgramaticChange = False

    End Sub


    Protected Overrides Function isTransactionPosted(row As Integer) As Boolean
        If row >= 0 AndAlso row < Rows.Count Then
            Return UIUtil.toBoolean(Rows(row).Cells("isExpensePosted").Value)
        Else
            Return False
        End If

    End Function

    Protected Overrides Sub indicateTransactionPosted(row As Integer, Optional isPosted As Boolean = True)
        Rows(row).Cells("isExpensePosted").Value = UIUtil.toBinaryBooleanString(isPosted)
    End Sub

    Protected Overrides Sub doValidateRow(row As Integer, ByRef result As RowValidationResult)
        If isReversalTransaction(row) Then
            If (StringUtil.isEmpty(Rows(row).Cells(getCommentColName()).Value)) Then
                addError(result, "Comment has to be entered", row, Columns(getCommentColName()).Index)
            End If
        Else
            MyBase.doValidateRow(row, result)
            If (StringUtil.isEmpty(Me.Rows(row).Cells("expenseCategoryCode").Value)) Then
                addError(result, "Expense code cannot be empty.", row, 1)
            ElseIf (Not getBusinessReportDlg().isValidCode(Me.Rows(row).Cells("expenseCategoryCode").Value, getBusinessReportDlg().expenseCategoriesEntityDataSet, "expense_category_code")) Then
                addError(result, "Expense code is Invalid.", row, 1)
            End If
            If (StringUtil.isEmpty(Me.Rows(row).Cells("expenseAmount").Value)) Then
                addError(result, "Expense amount cannot be empty", row, "expenseAmount")
            End If
        End If

    End Sub

    Public Overridable Sub loadBusinessReportDataFromGrid(busRprt As BusinessReportDAO.BusinessReport)
        MyBase.loadBusinessReportDataFromGrid(busRprt)
        Dim expenses As New Collection
        Dim expense As BusinessReportDAO.ExpenseVO

        For i = 0 To (RowCount - 1)
            If (Not isRowEmpty(i) AndAlso Not isTransactionPosted(i)) Then
                expense = New BusinessReportDAO.ExpenseVO
                expense.transactionId = UIUtil.subsIfEmpty(Rows(i).Cells("expenseId").Value, BusinessReportDAO.NULL_NUMBER)
                expense.tranDate = UIUtil.parseDate(Rows(i).Cells("expenseExpensedOn").Value, Nothing)
                expense.expCatCode = Rows(i).Cells("expenseCategoryCode").Value
                expense.expAmt = UIUtil.toAmtString(Rows(i).Cells("expenseAmount").Value)
                expense.comments = Rows(i).Cells("expenseComments").Value
                expense.isReversal = UIUtil.toBoolean(Rows(i).Cells("isExpensePostedReversal").Value)
                expense.isPosted = UIUtil.toBoolean(Rows(i).Cells("isExpensePosted").Value)
                expense.isAmendment = UIUtil.toBoolean(Rows(i).Cells("is_expense_amendment").Value)
                expenses.Add(expense)
            End If

        Next i
        busRprt.expenses = expenses
    End Sub

    Public Overrides Sub loadBusinessReportDataIntoGrid(busRprt As BusinessReportDAO.BusinessReport)
        MyBase.loadBusinessReportDataIntoGrid(busRprt)
        Dim expenses As Collection = busRprt.expenses
        If IsNothing(expenses) Then
            Return
        End If
        Dim expense As BusinessReportDAO.ExpenseVO
        For i = 1 To (expenses.Count)
            If (i > Rows.Count) Then
                Rows.Add()
            End If
            expense = expenses(i)
            Rows(i - 1).Cells("expenseId").Value = expense.transactionId
            Rows(i - 1).Cells("expenseExpensedOn").Value = UIUtil.toDateString(expense.tranDate)
            Rows(i - 1).Cells("expenseCategoryCode").Value = expense.expCatCode
            Rows(i - 1).Cells("expenseAmount").Value = expense.expAmt
            Rows(i - 1).Cells("expenseComments").Value = expense.comments

            Dim lkupRow As DataRow = getBusinessReportDlg().locateRow(expense.expCatCode, "expense_category_code", getBusinessReportDlg().expenseCategoriesEntityDataSet)
            Me.Rows(i - 1).Cells("expenseExpenseCategoryName").Value = If(Not IsNothing(lkupRow), lkupRow.Item("expense_category_name"), "")

            Rows(i - 1).Cells("isExpensePosted").Value = UIUtil.toBinaryBooleanString(expense.isPosted)
            Rows(i - 1).Cells("isExpensePostedReversal").Value = UIUtil.toBinaryBooleanString(expense.isReversal)
            Rows(i - 1).Cells("is_expense_amendment").Value = UIUtil.toBinaryBooleanString(expense.isAmendment)

        Next i
        MyBase.loadBusinessReportDataIntoGrid(busRprt)
    End Sub

    Protected Overrides Function getIsAmendmentColName() As String
        Return "is_expense_amendment"
    End Function

    Protected Overrides Function getCommentColName() As String
        Return "expenseComments"
    End Function

    Public Overrides Sub deleteCurrentRow()
        deleteRow("Expense", 0, 1)
    End Sub

    Protected Overrides Sub OnKeyUp(e As KeyEventArgs)
        If Not Me.getBusinessReportDlg().isReportSubmitted() AndAlso isControlSPressed(e) Then
            If Columns(CurrentCell.ColumnIndex).Name = "expenseCategoryCode" Then
                Dim dlg As ExpenseCategorieDlg = New ExpenseCategorieDlg(getBusinessReportDlg)
                dlg.codeSelected = CurrentCell.Value
                If (dlg.ShowDialog(Me) = DialogResult.OK) Then
                    If dlg.isEntitySelected Then
                        CurrentCell.Value = dlg.codeSelected
                        Rows(CurrentCell.RowIndex).Cells("expenseExpenseCategoryName").Value = dlg.nameSelected
                    End If

                    Me.CommitEdit(DataGridViewDataErrorContexts.Commit)
                        Refresh()
                        entityDataSetModified(dlg.wasModified())
                    End If
                    e.Handled = True
            End If
        End If

        MyBase.OnKeyUp(e)
    End Sub

    Public Function getTotalExpenses() As Double
        Dim tot As Double = 0
        For i = 0 To (RowCount - 1)
            If (Not isRowEmpty(i)) Then
                tot += UIUtil.parseDouble(Rows(i).Cells("expenseAmount").Value)
            End If
        Next i
        getTotalExpenses = tot
    End Function

    Protected Overrides Sub gridCellValueChanged(colInd As Integer, rowInd As Integer)
        If Me.Columns("expenseCategoryCode").Index = colInd Then
            Dim row As DataRow = getBusinessReportDlg().locateRow(Me.Rows(rowInd).Cells(colInd).Value, "expense_category_code", getBusinessReportDlg().expenseCategoriesEntityDataSet)
            Me.Rows(rowInd).Cells("expenseExpenseCategoryName").Value = If(Not IsNothing(row), row.Item("expense_category_name"), "")
            Refresh()
        End If

    End Sub
End Class
