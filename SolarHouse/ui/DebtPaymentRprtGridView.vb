Public Class DebtPaymentRprtGridView
    Inherits BusinessReportGridView

    Public Overrides Sub setupGridView()
        MyBase.setupGridView()
    End Sub

    Protected Overrides Function isReversalTransaction(row As Integer) As Boolean
        Return isValidDataGridViewRow(row) AndAlso UIUtil.toBoolean(Rows(row).Cells("isDebtPaymentPostedReversal").Value)
    End Function

    Protected Overrides Sub indicateTransactionReversed(row As Integer, Optional isReversed As Boolean = True)
        isProgramaticChange = True
        Rows(row).Cells("isDebtPaymentPostedReversal").Value = UIUtil.toBinaryBooleanString(isReversed)
        Me.Rows(row).Cells("debtPaymentAmtPaid").Value = -1 * Me.Rows(row).Cells("debtPaymentAmtPaid").Value
        isProgramaticChange = False
    End Sub


    Protected Overrides Function isTransactionPosted(row As Integer) As Boolean
        Return isValidDataGridViewRow(row) AndAlso UIUtil.toBoolean(Rows(row).Cells("isDebtPaymentPosted").Value)
    End Function

    Protected Overrides Sub indicateTransactionPosted(row As Integer, Optional isPosted As Boolean = True)
        Rows(row).Cells("isDebtPaymentPosted").Value = UIUtil.toBinaryBooleanString(isPosted)
    End Sub


    Protected Overrides Sub doValidateRow(row As Integer, ByRef result As RowValidationResult)
        If isReversalTransaction(row) Then
            If (StringUtil.isEmpty(Rows(row).Cells(getCommentColName()).Value)) Then
                addError(result, "Comment has to be entered", row, Columns(getCommentColName()).Index)
            End If
        Else
            MyBase.doValidateRow(row, result)
            If (StringUtil.isEmpty(Me.Rows(row).Cells("debtPaymentSupplierCode").Value)) Then
                addError(result, "Supplier code cannot be empty.", row, 1)
            ElseIf (Not getBusinessReportDlg().isValidCode(Me.Rows(row).Cells("debtPaymentSupplierCode").Value, getBusinessReportDlg().suppliersEntityDataSet, "supplier_code")) Then
                addError(result, "Supplier code is Invalid.", row, 1)
            End If
            If (StringUtil.isEmpty(Me.Rows(row).Cells("debtPaymentAmtPaid").Value)) Then
                addError(result, "Debt amount paid cannot be empty", row, "debtPaymentAmtPaid")
            End If
        End If

    End Sub

    Public Overridable Sub loadBusinessReportDataFromGrid(busRprt As BusinessReportDAO.BusinessReport)
        MyBase.loadBusinessReportDataFromGrid(busRprt)
        Dim debtPayments As New Collection
        Dim debtPayment As New BusinessReportDAO.DebtPaymentVO

        For i = 0 To (RowCount - 1)
            If (Not isRowEmpty(i) AndAlso Not isTransactionPosted(i)) Then
                debtPayment = New BusinessReportDAO.DebtPaymentVO
                debtPayment.transactionId = UIUtil.subsIfEmpty(Rows(i).Cells("debtPaymentId").Value, BusinessReportDAO.NULL_NUMBER)
                debtPayment.tranDate = UIUtil.parseDate(Rows(i).Cells("debtPaymentPaidOn").Value, Nothing)
                debtPayment.suplrCode = Rows(i).Cells("debtPaymentSupplierCode").Value
                debtPayment.debtAmtPaid = UIUtil.toAmtString(Rows(i).Cells("debtPaymentAmtPaid").Value)
                debtPayment.comments = Rows(i).Cells("debtPaymentComments").Value
                debtPayment.isReversal = UIUtil.toBoolean(Rows(i).Cells("isDebtPaymentPostedReversal").Value)
                debtPayment.isPosted = UIUtil.toBoolean(Rows(i).Cells("isDebtPaymentPosted").Value)
                debtPayment.isAmendment = UIUtil.toBoolean(Rows(i).Cells("is_debt_payment_amendment").Value)
                debtPayments.Add(debtPayment)
            End If

        Next i
        busRprt.debtPayments = debtPayments
    End Sub


    Public Overrides Sub loadBusinessReportDataIntoGrid(busRprt As BusinessReportDAO.BusinessReport)
        MyBase.loadBusinessReportDataIntoGrid(busRprt)
        Dim debtPayments As Collection = busRprt.debtPayments
        If IsNothing(debtPayments) Then
            Return
        End If

        Dim debtPayment As BusinessReportDAO.DebtPaymentVO
        For i = 1 To (debtPayments.Count)
            If (i > Rows.Count) Then
                Rows.Add()
            End If
            debtPayment = debtPayments(i)
            Rows(i - 1).Cells("debtPaymentId").Value = debtPayment.transactionId
            Rows(i - 1).Cells("debtPaymentPaidOn").Value = UIUtil.toDateString(debtPayment.tranDate)
            Rows(i - 1).Cells("debtPaymentSupplierCode").Value = debtPayment.suplrCode
            Rows(i - 1).Cells("debtPaymentAmtPaid").Value = debtPayment.debtAmtPaid
            Rows(i - 1).Cells("debtPaymentComments").Value = debtPayment.comments
            Dim lkupRow As DataRow = getBusinessReportDlg().locateRow(debtPayment.suplrCode, "supplier_code", getBusinessReportDlg().suppliersEntityDataSet)
            Me.Rows(i - 1).Cells("debtPaymentSupplierName").Value = If(Not IsNothing(lkupRow), lkupRow.Item("supplier_name"), "")
            Rows(i - 1).Cells("isDebtPaymentPosted").Value = UIUtil.toBinaryBooleanString(debtPayment.isPosted)
            Rows(i - 1).Cells("isDebtPaymentPostedReversal").Value = UIUtil.toBinaryBooleanString(debtPayment.isReversal)
            Rows(i - 1).Cells("is_debt_payment_amendment").Value = UIUtil.toBinaryBooleanString(debtPayment.isAmendment)
        Next i
        MyBase.loadBusinessReportDataIntoGrid(busRprt)
    End Sub

    Protected Overrides Function getIsAmendmentColName() As String
        Return "is_debt_payment_amendment"
    End Function

    Protected Overrides Function getCommentColName() As String
        Return "debtPaymentComments"
    End Function

    Public Overrides Sub deleteCurrentRow()
        deleteRow("Debt Payment", 0, 1)
    End Sub

    Protected Overrides Sub OnKeyUp(e As KeyEventArgs)
        If Not Me.getBusinessReportDlg().isReportSubmitted() AndAlso isControlSPressed(e) Then
            If Columns(Me.CurrentCell.ColumnIndex).Name = "debtPaymentSupplierCode" Then
                Dim dlg As SuppliersDlg = New SuppliersDlg(getBusinessReportDlg)
                dlg.codeSelected = CurrentCell.Value
                If (dlg.ShowDialog(Me) = DialogResult.OK) Then
                    If dlg.isEntitySelected Then
                        CurrentCell.Value = dlg.codeSelected
                        Rows(CurrentCell.RowIndex).Cells("debtPaymentSupplierName").Value = dlg.nameSelected
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

    Public Function getTotalDebtPaid() As Double
        Dim tot As Double = 0
        For i = 0 To (RowCount - 1)
            If (Not isRowEmpty(i)) Then
                tot += UIUtil.parseDouble(Rows(i).Cells("debtPaymentAmtPaid").Value)
            End If
        Next i
        getTotalDebtPaid = tot
    End Function



    Protected Overrides Sub gridCellValueChanged(colInd As Integer, rowInd As Integer)
        If Me.Columns("debtPaymentSupplierCode").Index = colInd Then
            Dim lkupRow As DataRow = getBusinessReportDlg().locateRow(Me.Rows(rowInd).Cells(colInd).Value, "supplier_code", getBusinessReportDlg().suppliersEntityDataSet)
            Me.Rows(rowInd).Cells("debtPaymentSupplierName").Value = If(Not IsNothing(lkupRow), lkupRow.Item("supplier_name"), "")
            Refresh()
        End If

    End Sub
End Class
