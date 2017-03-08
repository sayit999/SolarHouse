Public Class PurchasesRprtGridView
    Inherits BusinessReportGridView

    Public Overrides Sub setupGridView()
        MyBase.setupGridView()
    End Sub

    Protected Overrides Function isReversalTransaction(row As Integer) As Boolean
        Return isValidDataGridViewRow(row) AndAlso UIUtil.toBoolean(Rows(row).Cells("isPurchasePostedReversal").Value)
    End Function

    Protected Overrides Sub indicateTransactionReversed(row As Integer, Optional isReversed As Boolean = True)
        isProgramaticChange = True
        Rows(row).Cells("isPurchasePostedReversal").Value = UIUtil.toBinaryBooleanString(isReversed)
        Me.Rows(row).Cells("purchaseAmount").Value = -1 * UIUtil.zeroIfEmpty(Me.Rows(row).Cells("purchaseAmount").Value)
        Me.Rows(row).Cells("purchaseQty").Value = -1 * UIUtil.zeroIfEmpty(Me.Rows(row).Cells("purchaseQty").Value)
        Me.Rows(row).Cells("purchaseAmountPaidCash").Value = -1 * UIUtil.zeroIfEmpty(Me.Rows(row).Cells("purchaseAmountPaidCash").Value)
        Me.Rows(row).Cells("purchaseAmountPaidCredit").Value = -1 * UIUtil.zeroIfEmpty(Me.Rows(row).Cells("purchaseAmountPaidCredit").Value)
        isProgramaticChange = False
    End Sub


    Protected Overrides Sub indicateTransactionPosted(row As Integer, Optional isPosted As Boolean = True)
        Rows(row).Cells("isPurchasePosted").Value = UIUtil.toBinaryBooleanString(isPosted)
    End Sub

    Protected Overrides Function isTransactionPosted(row As Integer) As Boolean
        Return isValidDataGridViewRow(row) AndAlso UIUtil.toBoolean(Rows(row).Cells("isPurchasePosted").Value)
    End Function

    Protected Overrides Sub doValidateRow(row As Integer, ByRef result As RowValidationResult)
        If isReversalTransaction(row) Then
            If (StringUtil.isEmpty(Rows(row).Cells(getCommentColName()).Value)) Then
                addError(result, "Comment has to be entered", row, Columns(getCommentColName()).Index)
            End If
        Else
            MyBase.doValidateRow(row, result)
            If (StringUtil.isEmpty(Me.Rows(row).Cells("purchaseProductCode").Value)) Then
                addError(result, "Product code cannot be empty.", row, "purchaseProductCode")
            ElseIf (Not getBusinessReportDlg().isValidCode(Me.Rows(row).Cells("purchaseProductCode").Value, getBusinessReportDlg().productsEntityDataSet, "product_code")) Then
                addError(result, "Product code is Invalid.", row, "purchaseProductCode")
            End If

            If (StringUtil.isEmpty(Me.Rows(row).Cells("purchaseSupplierCode").Value)) Then
                addError(result, "Supplier code cannot be empty.", row, "purchaseSupplierCode")
            ElseIf (Not getBusinessReportDlg().isValidCode(Me.Rows(row).Cells("purchaseSupplierCode").Value, getBusinessReportDlg().suppliersEntityDataSet, "supplier_code")) Then
                addError(result, "Supplier code is Invalid.", row, "purchaseSupplierCode")
            End If

            If (StringUtil.isEmpty(Me.Rows(row).Cells("purchaseQty").Value)) Then
                addError(result, "Qty cannot be empty", row, "purchaseQty")
            ElseIf UIUtil.zeroIfEmpty(Me.Rows(row).Cells("purchaseQty").Value) < 0 Then
                addError(result, "Purchase Qty cannot be negative", row, "purchaseQty")
            End If

            If (StringUtil.isEmpty(Me.Rows(row).Cells("purchaseAmount").Value)) Then
                addError(result, "Purchase amount cannot be empty", row, "purchaseAmount")
            ElseIf UIUtil.zeroIfEmpty(Me.Rows(row).Cells("purchaseAmount").Value) < 0 Then
                addError(result, "Purchase amount cannot be negative", row, "purchaseAmount")
            ElseIf UIUtil.zeroIfEmpty(Me.Rows(row).Cells("purchaseAmount").Value) = 0 AndAlso StringUtil.isEmpty(Me.Rows(row).Cells("purchaseComments").Value) Then
                addError(result, "Enter why is the amount zero", row, "purchaseComments")
            End If

            If (StringUtil.isEmpty(Me.Rows(row).Cells("purchaseAmountPaidCash").Value) AndAlso StringUtil.isEmpty(Me.Rows(row).Cells("purchaseAmountPaidCredit").Value)) Then
                addError(result, "Enter either amount purchased cash or credit", row, "purchaseAmountPaidCash")
                addError(result, "Enter either amount purchased cash or credit", row, "purchaseAmountPaidCredit")
            ElseIf UIUtil.zeroIfEmpty(Me.Rows(row).Cells("purchaseAmountPaidCash").Value) < 0 Then
                addError(result, "Purchase amount cannot be negative", row, "purchaseAmountPaidCash")
            ElseIf UIUtil.zeroIfEmpty(Me.Rows(row).Cells("purchaseAmountPaidCredit").Value) < 0 Then
                addError(result, "Purchase amount cannot be negative", row, "purchaseAmountPaidCredit")
            ElseIf UIUtil.zeroIfEmpty(Rows(row).Cells("purchaseAmount").Value()) <> (UIUtil.zeroIfEmpty(Rows(row).Cells("purchaseAmountPaidCash").Value()) + UIUtil.zeroIfEmpty(Rows(row).Cells("purchaseAmountPaidCredit").Value())) Then
                Dim mesg As String = "Purchase Amount does not equal Amount Paid Cash + Paid Credit"
                addError(result, mesg, row, "purchaseAmount")
                addError(result, mesg, row, "purchaseAmountPaidCash")
                addError(result, mesg, row, "purchaseAmountPaidCredit")
            End If
        End If

    End Sub

    Public Overridable Sub loadBusinessReportDataFromGrid(busRprt As BusinessReportDAO.BusinessReport)
        MyBase.loadBusinessReportDataFromGrid(busRprt)
        Dim purchases As New Collection
        Dim purchase As BusinessReportDAO.PurchaseVO

        For i = 0 To (RowCount - 1)
            If Not Me.isRowEmpty(i) AndAlso Not isTransactionPosted(i) Then
                purchase = New BusinessReportDAO.PurchaseVO

                purchase.transactionId = UIUtil.subsIfEmpty(Rows(i).Cells("purchaseId").Value, BusinessReportDAO.NULL_NUMBER)
                purchase.tranDate = UIUtil.parseDate(Rows(i).Cells("purchasePurchasedOn").Value, Nothing)
                purchase.suplrCode = Rows(i).Cells("purchaseSupplierCode").Value
                purchase.qty = Rows(i).Cells("purchaseQty").Value
                purchase.prodCode = Rows(i).Cells("purchaseProductCode").Value
                purchase.cashPurchase = UIUtil.zeroIfEmpty(Rows(i).Cells("purchaseAmountPaidCash").Value)
                purchase.creditPurchase = UIUtil.zeroIfEmpty(Rows(i).Cells("purchaseAmountPaidCredit").Value)
                purchase.comments = Rows(i).Cells("purchaseComments").Value
                purchase.isReversal = UIUtil.toBoolean(Rows(i).Cells("isPurchasePostedReversal").Value)
                purchase.isPosted = UIUtil.toBoolean(Rows(i).Cells("isPurchasePosted").Value)
                purchase.isAmendment = UIUtil.toBoolean(Rows(i).Cells("is_purchase_amendment").Value)
                purchases.Add(purchase)
            End If
        Next i
        busRprt.purchases = purchases
    End Sub

    Public Function isProdPurchaseInErr(prodCode As String) As Boolean
        For r = 0 To Rows.Count - 1
            If (UIUtil.subsIfEmpty(Rows(r).Cells("purchaseProductCode").Value, "") = prodCode AndAlso Not StringUtil.isEmpty(Rows(r).ErrorText)) Then
                Return True
            End If
        Next
        Return False
    End Function

    Protected Overrides Sub gridCellValueChanged(colInd As Integer, rowInd As Integer)

        If Me.Columns("purchaseSupplierCode").Index = colInd Then
            Dim row As DataRow = getBusinessReportDlg().locateRow(Me.Rows(rowInd).Cells("purchaseSupplierCode").Value, "supplier_code", getBusinessReportDlg().suppliersEntityDataSet)
            If (Not IsNothing(row)) Then
                Me.Rows(rowInd).Cells("purchaseSupplierName").Value = row.Item("supplier_name")
            Else
                Me.Rows(rowInd).Cells("purchaseSupplierName").Value = ""
            End If
            Refresh()
        ElseIf Me.Columns("purchaseProductCode").Index = colInd Then
            Rows(rowInd).Cells(colInd).Value = UCase(Rows(rowInd).Cells(colInd).Value)
            Dim row As DataRow = getBusinessReportDlg().locateRow(Me.Rows(rowInd).Cells(colInd).Value, "product_code", getBusinessReportDlg().productsEntityDataSet)
            If (Not IsNothing(row)) Then
                Me.Rows(rowInd).Cells("purchaseProductName").Value = row.Item("product_name")
                Me.Rows(rowInd).Cells("purchaseQtyUomName").Value = row.Item("qty_uom_name")
            Else
                Me.Rows(rowInd).Cells("purchaseProductName").Value = ""
                Me.Rows(rowInd).Cells("purchaseQtyUomName").Value = ""
            End If
            Refresh()
        ElseIf Me.Columns("purchaseQty").Index = colInd Or Me.Columns("purchaseAmount").Index = colInd Then
            Me.Rows(rowInd).Cells("purchaseCostPerItem").Value = If(getNumber(rowInd, Columns("purchaseQty").Index) = 0, 0, getNumber(rowInd, Columns("purchaseAmount").Index) / getNumber(rowInd, Columns("purchaseQty").Index))

            Refresh()
        ElseIf Me.Columns("purchaseAmount").Index = colInd Then
            Me.Rows(rowInd).Cells("purchaseAmountPaidCash").Value = 0
            Me.Rows(rowInd).Cells("purchaseAmountPaidCredit").Value = 0
            Refresh()
        ElseIf Me.Columns("purchaseAmountPaidCash").Index = colInd Then
            Me.Rows(rowInd).Cells("purchaseAmountPaidCredit").Value = getNumber(rowInd, Columns("purchaseAmount").Index) - getNumber(rowInd, Columns("purchaseAmountPaidCash").Index)
            Refresh()
        ElseIf Me.Columns("purchaseAmountPaidCredit").Index = colInd Then
            Me.Rows(rowInd).Cells("purchaseAmountPaidCash").Value = getNumber(rowInd, Columns("purchaseAmount").Index) - getNumber(rowInd, Columns("purchaseAmountPaidCredit").Index)
            Refresh()
        End If

    End Sub

    Public Overrides Sub loadBusinessReportDataIntoGrid(busRprt As BusinessReportDAO.BusinessReport)
        MyBase.loadBusinessReportDataIntoGrid(busRprt)
        Dim purchases As Collection = busRprt.purchases
        If IsNothing(purchases) Then
            Return
        End If

        Dim lkupRow As DataRow
        Dim purchase As BusinessReportDAO.PurchaseVO
        For i = 1 To (purchases.Count)
            If (i > Rows.Count) Then
                Rows.Add()
            End If
            purchase = purchases(i)

            Rows(i - 1).Cells("purchaseId").Value = purchase.transactionId
            Rows(i - 1).Cells("purchasePurchasedOn").Value = UIUtil.toDateString(purchase.tranDate)
            Rows(i - 1).Cells("purchaseSupplierCode").Value = purchase.suplrCode
            Rows(i - 1).Cells("purchaseQty").Value = UIUtil.zeroIfEmptyStr(purchase.qty)
            Rows(i - 1).Cells("purchaseProductCode").Value = purchase.prodCode
            Rows(i - 1).Cells("purchaseAmount").Value = UIUtil.toAmtString(UIUtil.zeroIfEmpty(purchase.creditPurchase) + UIUtil.zeroIfEmpty(purchase.cashPurchase))
            Rows(i - 1).Cells("purchaseAmountPaidCash").Value = UIUtil.toAmtString(purchase.cashPurchase)
            Rows(i - 1).Cells("purchaseAmountPaidCredit").Value = UIUtil.toAmtString(purchase.creditPurchase)

            lkupRow = getBusinessReportDlg().locateRow(purchase.suplrCode, "supplier_code", getBusinessReportDlg().suppliersEntityDataSet)
            Me.Rows(i - 1).Cells("purchaseSupplierName").Value = If(Not IsNothing(lkupRow), lkupRow.Item("supplier_name"), "")

            lkupRow = getBusinessReportDlg().locateRow(purchase.prodCode, "product_code", getBusinessReportDlg().productsEntityDataSet)
            Me.Rows(i - 1).Cells("purchaseProductName").Value = If(Not IsNothing(lkupRow), lkupRow.Item("product_name"), "")
            Me.Rows(i - 1).Cells("purchaseQtyUomName").Value = If(Not IsNothing(lkupRow), lkupRow.Item("qty_uom_name"), "")

            Me.Rows(i - 1).Cells("purchaseCostPerItem").Value = If(getNumber(i - 1, Columns("purchaseQty").Index) = 0, 0, getNumber(i - 1, Columns("purchaseAmount").Index) / getNumber(i - 1, Columns("purchaseQty").Index))
            'Try
            '    Me.Rows(i - 1).Cells("purchaseCostPerItem").Value = getNumber(i - 1, Columns("purchaseAmount").Index) / getNumber(i - 1, Columns("purchaseQty").Index)
            'Catch ex As DivideByZeroException
            '    Me.Rows(i - 1).Cells("purchaseCostPerItem").Value = 0
            'End Try
            Rows(i - 1).Cells("purchaseComments").Value = purchase.comments
            Rows(i - 1).Cells("isPurchasePosted").Value = UIUtil.toBinaryBooleanString(purchase.isPosted)
            Rows(i - 1).Cells("isPurchasePostedReversal").Value = UIUtil.toBinaryBooleanString(purchase.isReversal)
            Rows(i - 1).Cells("is_purchase_amendment").Value = UIUtil.toBinaryBooleanString(purchase.isAmendment)
        Next i

        MyBase.loadBusinessReportDataIntoGrid(busRprt)
    End Sub

    Protected Overrides Function getIsAmendmentColName() As String
        Return "is_purchase_amendment"
    End Function

    Protected Overrides Function getCommentColName() As String
        Return "purchaseComments"
    End Function

    Public Sub getPurchaseTotals(ByRef cashPurchases As Double, ByRef creditPurchases As Double)
        cashPurchases = 0
        creditPurchases = 0
        For i = 0 To (RowCount - 1)
            If (Not isRowEmpty(i)) Then
                cashPurchases += UIUtil.parseDouble(Rows(i).Cells("purchaseAmountPaidCash").Value)
                creditPurchases += UIUtil.parseDouble(Rows(i).Cells("purchaseAmountPaidCredit").Value)
            End If
        Next i
    End Sub

    Public Function getQtyPurchased(row As Integer)
        If StringUtil.isEmpty(Rows(row).Cells(Columns("purchaseQty").Index).Value) Then
            getQtyPurchased = 0
        Else
            getQtyPurchased = Rows(row).Cells(Columns("purchaseQty").Index).Value
        End If

    End Function

    Public Overrides Sub deleteCurrentRow()
        deleteRow("Purchase", 0, 1)
    End Sub

    Protected Overrides Sub OnKeyUp(e As KeyEventArgs)
        If Not Me.getBusinessReportDlg().isReportSubmitted() AndAlso isControlSPressed(e) Then
            If Columns(CurrentCell.ColumnIndex).Name = "purchaseSupplierCode" Then
                Dim dlg As SuppliersDlg = New SuppliersDlg(getBusinessReportDlg)
                dlg.codeSelected = CurrentCell.Value
                If (dlg.ShowDialog(Me) = DialogResult.OK) Then
                    CurrentCell.Value = dlg.codeSelected
                    Rows(CurrentCell.RowIndex).Cells("purchaseSupplierName").Value = dlg.nameSelected
                    Me.CommitEdit(DataGridViewDataErrorContexts.Commit)
                    Refresh()
                    entityDataSetModified(dlg.wasModified())
                End If
                e.Handled = True
            ElseIf Columns(CurrentCell.ColumnIndex).Name = "purchaseProductCode" Then
                Dim dlg As ProductsDlg = New ProductsDlg(getBusinessReportDlg)
                dlg.codeSelected = CurrentCell.Value
                If (dlg.ShowDialog(Me) = DialogResult.OK) Then
                    If dlg.isEntitySelected Then
                        CurrentCell.Value = dlg.codeSelected
                        Rows(CurrentCell.RowIndex).Cells("purchaseProductName").Value = dlg.nameSelected
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





End Class
